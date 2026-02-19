using System;
using Config;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.CombatSkill;

namespace GameData.Domains.Combat;

public struct MoveData
{
	private CombatCharacter _combatChar;

	public short MoveCd;

	private short _frameCounter;

	private short _aniTotalFrame;

	private bool _moveCdLongerThanAni;

	public short JumpMoveSkillId;

	public bool CanPartlyJump;

	public short MaxJumpForwardDist;

	public short MaxJumpBackwardDist;

	public int PrepareProgressUnit;

	public byte JumpPrepareDirection;

	private sbyte _reducePrepareFrameCounter;

	public short CanMoveForwardInSkillPrepareDist;

	public short CanMoveBackwardInSkillPrepareDist;

	public int JumpPreparedProgress { get; private set; }

	public void Init(DataContext context, CombatCharacter combatChar)
	{
		_combatChar = combatChar;
		JumpMoveSkillId = -1;
		CanPartlyJump = false;
		MaxJumpForwardDist = 0;
		MaxJumpBackwardDist = 0;
		Reset();
		ResetJumpState(context, calcPreparedMove: false);
		ClearSkillPrepareMoveDist();
	}

	public void Reset()
	{
		MoveCd = 0;
		_frameCounter = -1;
		_aniTotalFrame = -1;
		_moveCdLongerThanAni = false;
		_combatChar.SetAnimationTimeScale(1f, _combatChar.GetDataContext());
	}

	public void ResetJumpState(DataContext context, bool calcPreparedMove = true)
	{
		if (JumpPreparedProgress != 0 || _combatChar.GetJumpPreparedDistance() != 0)
		{
			if (calcPreparedMove && CanPartlyJump && _combatChar.GetJumpPreparedDistance() >= 10)
			{
				StartMove(context, _combatChar.GetJumpPreparedDistance() / 10 * 10, isJump: true);
			}
			JumpPreparedProgress = 0;
			_reducePrepareFrameCounter = 0;
			if (_combatChar.GetJumpPrepareProgress() > 0)
			{
				_combatChar.SetJumpPrepareProgress(0, context);
			}
			if (_combatChar.GetJumpPreparedDistance() > 0)
			{
				_combatChar.SetJumpPreparedDistance(0, context);
			}
			if (!_combatChar.NeedPauseJumpMove)
			{
				_combatChar.PauseJumpMoveSkillId = -1;
			}
		}
	}

	public void UpdateJumpPrepare(DataContext context)
	{
		if (_combatChar.GetAnimationToLoop() != "C_007")
		{
			_combatChar.SetAnimationToLoop("C_007", context);
		}
		JumpPreparedProgress += GetJumpSpeed();
		if (JumpPreparedProgress >= PrepareProgressUnit)
		{
			short num = (short)(_combatChar.GetJumpPreparedDistance() + 10);
			short num2 = (_combatChar.MoveForward ? MaxJumpForwardDist : MaxJumpBackwardDist);
			JumpPreparedProgress -= PrepareProgressUnit;
			UpdateJumpProgress(context);
			_combatChar.SetJumpPreparedDistance((short)((num < num2) ? num : 0), context);
			if (num >= num2)
			{
				StartMove(context, num2, isJump: true);
			}
		}
		else
		{
			UpdateJumpProgress(context);
		}
	}

	private int GetJumpSpeed()
	{
		return CombatSkillDomain.CalcJumpSpeed(_combatChar.GetId(), JumpMoveSkillId);
	}

	public void ReduceJumpPrepare(DataContext context)
	{
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		_reducePrepareFrameCounter++;
		if (_reducePrepareFrameCounter >= MoveSpecialConstants.ReduceJumpProgressFrame)
		{
			_reducePrepareFrameCounter = 0;
			JumpPreparedProgress -= PrepareProgressUnit * MoveSpecialConstants.ReduceJumpProgressPercent;
			if (JumpPreparedProgress <= 0 && _combatChar.GetJumpPreparedDistance() > 0)
			{
				short jumpPreparedDistance = (short)(_combatChar.GetJumpPreparedDistance() - 10);
				_combatChar.SetJumpPreparedDistance(jumpPreparedDistance, context);
				JumpPreparedProgress += PrepareProgressUnit;
			}
			if (_combatChar.GetJumpPreparedDistance() <= 0)
			{
				JumpPreparedProgress = Math.Max(JumpPreparedProgress, 0);
			}
			UpdateJumpProgress(context);
		}
	}

	private void UpdateJumpProgress(DataContext context)
	{
		sbyte b = (sbyte)(JumpPreparedProgress * 100 / PrepareProgressUnit);
		if (_combatChar.GetJumpPrepareProgress() != b)
		{
			_combatChar.SetJumpPrepareProgress(b, context);
		}
	}

	public bool IsJumpMove(bool forward)
	{
		return (forward ? _combatChar.MoveData.MaxJumpForwardDist : _combatChar.MoveData.MaxJumpBackwardDist) > 0;
	}

	public bool PreparingJumpMove()
	{
		return PrepareProgressUnit > 0;
	}

	public void StartMove(DataContext context, int moveDist = 1, bool isJump = false)
	{
		int id = _combatChar.GetId();
		bool isMove = DomainManager.SpecialEffect.ModifyData(id, -1, 157, dataValue: true);
		MoveCd = _combatChar.GetMoveCd();
		SetMoveAnimation(context, isJump, isMove);
		if (isJump)
		{
			moveDist = DomainManager.SpecialEffect.ModifyValue(_combatChar.GetId(), JumpMoveSkillId, 165, moveDist, _combatChar.MoveForward ? 1 : 0);
			SetJumpAnimation(context);
		}
		Events.RaiseMoveBegin(context, _combatChar, moveDist, isJump);
		MoveChangeDistance(context, moveDist, isJump);
		Events.RaiseMoveEnd(context, _combatChar, moveDist, isJump);
		DoMoveCost(context);
		UpdateCanMoveInSkillPrepareDist(moveDist);
	}

	public void UpdateMove(DataContext context, CombatDomain currentCombatDomain)
	{
		MoveCd--;
		_frameCounter++;
		if (_frameCounter == _aniTotalFrame)
		{
			if (_moveCdLongerThanAni)
			{
				currentCombatDomain.SetProperLoopAniAndParticle(context, _combatChar);
			}
			else
			{
				_frameCounter = 0;
			}
		}
		if (MoveCd == 0 && _combatChar.KeepMoving && !currentCombatDomain.CanMove(_combatChar, _combatChar.MoveForward))
		{
			currentCombatDomain.SetProperLoopAniAndParticle(context, _combatChar);
		}
	}

	public void ClearSkillPrepareMoveDist()
	{
		CanMoveForwardInSkillPrepareDist = 0;
		CanMoveBackwardInSkillPrepareDist = 0;
	}

	private void DoMoveCost(DataContext context)
	{
		short affectingMoveSkillId = _combatChar.GetAffectingMoveSkillId();
		if (affectingMoveSkillId >= 0)
		{
			int num = DomainManager.Combat.GetSkillMoveCostMobility(_combatChar, affectingMoveSkillId);
			if (Config.CombatSkill.Instance[affectingMoveSkillId].MaxJumpDistance > 0 && (_combatChar.MoveForward ? (MaxJumpForwardDist <= 0) : (MaxJumpBackwardDist <= 0)))
			{
				num = num * GlobalConfig.Instance.AgileSkillNonJumpDirectionCostMobilityPercent / 100;
			}
			if (num > 0)
			{
				DomainManager.Combat.ChangeMobilityValue(context, _combatChar, -num);
			}
		}
	}

	private void SetMoveAnimation(DataContext context, bool isJump, bool isMove)
	{
		string text;
		bool flag;
		if (isMove)
		{
			(text, flag) = DomainManager.Combat.SetProperLoopAniAndParticle(context, _combatChar, getMoveAni: true);
		}
		else
		{
			text = DomainManager.Combat.GetIdleAni(_combatChar);
			flag = text != _combatChar.GetAnimationToLoop();
			if (flag)
			{
				_combatChar.SetAnimationToLoop(text, context);
			}
		}
		float num = (isJump ? 1f : Math.Max(3f / (float)MoveCd, 1f));
		if (flag || !(Math.Abs(_combatChar.GetAnimationTimeScale() - num) < 0.1f))
		{
			if (text != null)
			{
				AnimData animData = AnimDataCollection.Data[text];
				_aniTotalFrame = (short)(animData.Duration * 60f / num);
				_frameCounter = 0;
				_moveCdLongerThanAni = MoveCd > _aniTotalFrame;
				_combatChar.SetAnimationTimeScale(num, context);
			}
			else
			{
				_aniTotalFrame = -1;
			}
		}
	}

	private void SetJumpAnimation(DataContext context)
	{
		string animationToPlayOnce = (_combatChar.MoveForward ? "M_003_fly" : "M_004_fly");
		_combatChar.SetAnimationToPlayOnce(animationToPlayOnce, context);
		DomainManager.Combat.PlayWhooshSound(context, _combatChar);
		if (_combatChar.BossConfig?.JumpMoveParticles != null)
		{
			_combatChar.SetParticleToPlay(_combatChar.BossConfig.JumpMoveParticles[(!_combatChar.MoveForward) ? 1 : 0], context);
		}
		if (_combatChar.AnimalConfig?.JumpMoveParticles != null)
		{
			_combatChar.SetParticleToPlay(_combatChar.AnimalConfig.JumpMoveParticles[(!_combatChar.MoveForward) ? 1 : 0], context);
		}
	}

	private void MoveChangeDistance(DataContext context, int moveDist, bool isJump)
	{
		if (isJump && _combatChar.GetAffectingMoveSkillId() == 757)
		{
			short targetDistance = _combatChar.AiController.GetTargetDistance();
			if (targetDistance >= 0)
			{
				DomainManager.Combat.ChangeDistance(context, _combatChar, targetDistance - DomainManager.Combat.GetCurrentDistance(), isForced: false, canStop: false);
			}
			return;
		}
		int num = (_combatChar.MoveForward ? (-moveDist) : moveDist);
		if (isJump && _combatChar.PauseJumpMoveSkillId >= 0 && _combatChar.GetPreparingOtherAction() < 0 && _combatChar.GetPreparingSkillId() < 0 && Config.CombatSkill.Instance[_combatChar.PauseJumpMoveSkillId].JumpAni != null)
		{
			_combatChar.PauseJumpMoveDistance = num;
			_combatChar.NeedPauseJumpMove = true;
		}
		else
		{
			DomainManager.Combat.ChangeDistance(context, _combatChar, num);
		}
	}

	private void UpdateCanMoveInSkillPrepareDist(int moveDist)
	{
		if (_combatChar.GetPreparingSkillId() >= 0)
		{
			if (_combatChar.MoveForward)
			{
				CanMoveForwardInSkillPrepareDist = (short)(CanMoveForwardInSkillPrepareDist - Math.Abs(moveDist));
			}
			else
			{
				CanMoveBackwardInSkillPrepareDist = (short)(CanMoveBackwardInSkillPrepareDist - Math.Abs(moveDist));
			}
		}
	}
}
