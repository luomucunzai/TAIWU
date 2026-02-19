using System;
using Config;
using GameData.Common;
using GameData.DomainEvents;

namespace GameData.Domains.Combat;

public class CombatCharacterStateCastSkill : CombatCharacterStateBase
{
	private short _prepareFinishAniFrame;

	private short _skillAniFrame;

	private readonly short[] _damageFrame = new short[4];

	private short _skillAniFrameCounter;

	private CombatSkillItem _configData;

	private bool _outOfRange;

	private short _attackEndWaitFrame;

	private int _translateStateDelayedFrame;

	public CombatCharacterStateCastSkill(CombatDomain combatDomain, CombatCharacter combatChar)
		: base(combatDomain, combatChar, CombatCharacterStateType.CastSkill)
	{
		RequireDelayFallen = true;
		IsUpdateOnPause = true;
	}

	public override void OnEnter()
	{
		_translateStateDelayedFrame = -1;
		DataContext dataContext = CombatChar.GetDataContext();
		short preparingSkillId = CombatChar.GetPreparingSkillId();
		CombatCharacter combatCharacter = CurrentCombatDomain.GetCombatCharacter(!CombatChar.IsAlly, tryGetCoverCharacter: true);
		_configData = Config.CombatSkill.Instance[preparingSkillId];
		_attackEndWaitFrame = 6;
		if (_configData.EquipType == 1)
		{
			CombatChar.SkillAttackBodyPart = CurrentCombatDomain.GetAttackBodyPart(CombatChar, combatCharacter, dataContext.Random, _configData.TemplateId, -1, -1);
		}
		Events.RaisePrepareSkillChangeDistance(dataContext, CombatChar, combatCharacter, preparingSkillId);
		Events.RaisePrepareSkillEnd(dataContext, CombatChar.GetId(), CombatChar.IsAlly, preparingSkillId);
		if (_configData.EquipType == 1)
		{
			CurrentCombatDomain.CalcAttackSkillDataCompare(CombatContext.Create(CombatChar, null, -1, preparingSkillId));
			_outOfRange = !CurrentCombatDomain.InAttackRange(CombatChar);
			if (!_outOfRange)
			{
				short distance = ((_configData.PlayerCastBossSkillDistance == null || CombatChar.BossConfig != null) ? _configData.DistanceWhenFourStepAnimation[0] : _configData.PlayerCastBossSkillDistance[0]);
				CurrentCombatDomain.SetDisplayPosition(dataContext, CombatChar.IsAlly, CurrentCombatDomain.GetDisplayPosition(CombatChar.IsAlly, distance));
			}
			if (CurrentCombatDomain.IsPlayingMoveAni(combatCharacter))
			{
				combatCharacter.SetAnimationToLoop(CurrentCombatDomain.GetIdleAni(combatCharacter), dataContext);
			}
			PlayPrepareFinishAni();
			Events.RaiseCastAttackSkillBegin(dataContext, CombatChar, combatCharacter, preparingSkillId);
		}
		CombatChar.SetPreparingSkillId(-1, dataContext);
	}

	public override void OnExit()
	{
		DataContext dataContext = CombatChar.GetDataContext();
		CombatChar.SetSkillPreparePercent(0, dataContext);
		CombatChar.SetSkillSoundToPlay(string.Empty, dataContext);
		CombatChar.SetParticleToPlay(string.Empty, dataContext);
	}

	public override bool OnUpdate()
	{
		if (!base.OnUpdate())
		{
			return false;
		}
		if (_configData.EquipType == 1)
		{
			if (_prepareFinishAniFrame > 0)
			{
				_prepareFinishAniFrame--;
				if (_prepareFinishAniFrame == 0)
				{
					DataContext dataContext = CombatChar.GetDataContext();
					bool flag = CombatChar.BossConfig != null;
					string text = ((string.IsNullOrEmpty(_configData.PlayerCastBossSkillAni) || flag) ? _configData.CastAnimation : _configData.PlayerCastBossSkillAni);
					string text2 = ((_configData.Type != 13 || flag) ? "" : CurrentCombatDomain.GetMusicWeaponNameFix(CombatChar.GetWeaponData()));
					text = ((!flag) ? (text + text2) : (CombatChar.BossConfig.AniPrefix[CombatChar.GetBossPhase()] + text));
					AnimData animData = AnimDataCollection.Data[text];
					_skillAniFrame = (short)Math.Round(animData.Duration * 60f);
					_skillAniFrame += _attackEndWaitFrame;
					for (int i = 0; i < 4; i++)
					{
						_damageFrame[i] = (short)Math.Round(animData.Events[$"act{i + 1}"][0] * 60f);
					}
					_skillAniFrameCounter = 0;
					CombatChar.SetAnimationToPlayOnce((!flag) ? text : _configData.CastAnimation, dataContext);
					if (!string.IsNullOrEmpty(_configData.CastParticle))
					{
						string text3 = ((string.IsNullOrEmpty(_configData.PlayerCastBossSkillParticle) || flag) ? _configData.CastParticle : _configData.PlayerCastBossSkillParticle);
						CombatChar.SetParticleToPlay((!flag) ? (text3 + text2) : _configData.CastParticle, dataContext);
					}
					if (!string.IsNullOrEmpty(_configData.CastSoundEffect))
					{
						string text4 = ((string.IsNullOrEmpty(_configData.PlayerCastBossSkillSound) || flag) ? _configData.CastSoundEffect : _configData.PlayerCastBossSkillSound);
						CombatChar.SetSkillSoundToPlay(text4 + text2, dataContext);
					}
					if (!string.IsNullOrEmpty(_configData.CastPetAnimation) && flag)
					{
						CombatChar.SetSkillPetAnimation(_configData.CastPetAnimation, dataContext);
					}
					if (!string.IsNullOrEmpty(_configData.CastPetParticle) && flag)
					{
						CombatChar.SetPetParticle(_configData.CastPetParticle, dataContext);
					}
					CombatChar.SetAnimationToLoop(CurrentCombatDomain.GetIdleAni(CombatChar), dataContext);
				}
				return false;
			}
			_skillAniFrameCounter++;
			for (int j = 0; j < _damageFrame.Length; j++)
			{
				if (_damageFrame[j] != _skillAniFrameCounter)
				{
					continue;
				}
				DataContext dataContext2 = CombatChar.GetDataContext();
				if (j == 3 || CombatChar.SkillHitType[j] >= 0)
				{
					if (_outOfRange)
					{
						CombatChar.SetAttackOutOfRange(attackOutOfRange: true, dataContext2);
					}
					else
					{
						CurrentCombatDomain.CalcSkillAttack(CombatContext.Create(CombatChar, null, -1, -1), j);
					}
					if (j == 3 && _configData.WeaponDurableCost > 0)
					{
						CurrentCombatDomain.CostDurability(dataContext2, CombatChar, CurrentCombatDomain.GetUsingWeaponKey(CombatChar), _configData.WeaponDurableCost);
					}
				}
				if (!_outOfRange)
				{
					short distance = ((_configData.PlayerCastBossSkillDistance == null || CombatChar.BossConfig != null) ? _configData.DistanceWhenFourStepAnimation[j + 1] : _configData.PlayerCastBossSkillDistance[j + 1]);
					CurrentCombatDomain.SetDisplayPosition(dataContext2, !CombatChar.IsAlly, CurrentCombatDomain.GetDisplayPosition(!CombatChar.IsAlly, distance));
				}
				Events.RaiseAttackSkillAttackEndOfAll(dataContext2, CombatChar, j);
				if (j < 3)
				{
					CombatChar.SetAttackSkillAttackIndex((byte)(j + 1), dataContext2);
				}
				break;
			}
			if (_skillAniFrameCounter == _skillAniFrame - _attackEndWaitFrame)
			{
				DataContext dataContext3 = CombatChar.GetDataContext();
				sbyte power = (sbyte)CombatChar.GetAttackSkillPower();
				CombatChar.SetPerformingSkillId(-1, dataContext3);
				CombatChar.SetAttackSkillPower(0, dataContext3);
				CombatChar.SetSkillPetAnimation(null, dataContext3);
				CurrentCombatDomain.SetDisplayPosition(dataContext3, CombatChar.IsAlly, int.MinValue);
				CurrentCombatDomain.SetDisplayPosition(dataContext3, !CombatChar.IsAlly, int.MinValue);
				int finalCriticalOdds = CurrentCombatDomain.GetFinalCriticalOdds(CombatChar);
				CurrentCombatDomain.ClearDamageCompareData(dataContext3);
				DomainManager.Combat.RaiseCastSkillEnd(dataContext3, CombatChar.GetId(), CombatChar.IsAlly, _configData.TemplateId, power, interrupt: false, finalCriticalOdds);
				CombatCharacter combatChar = CombatChar;
				CombatCharacter combatCharacter = DomainManager.Combat.GetCombatCharacter(!combatChar.IsAlly, tryGetCoverCharacter: true);
				DomainManager.Combat.AddToCheckFallenSet(combatChar.GetId());
				DomainManager.Combat.AddToCheckFallenSet(combatCharacter.GetId());
				if (CombatChar.GetAutoCastingSkill() && CombatChar.NeedUseSkillFreeId < 0)
				{
					CombatChar.SetAutoCastingSkill(autoCastingSkill: false, dataContext3);
				}
			}
			if (_skillAniFrameCounter >= _skillAniFrame)
			{
				DataContext dataContext4 = CombatChar.GetDataContext();
				CombatCharacter combatCharacter2 = CurrentCombatDomain.GetCombatCharacter(!CombatChar.IsAlly, tryGetCoverCharacter: true);
				if (combatCharacter2.GetAnimationToLoop() == CurrentCombatDomain.GetIdleAni(combatCharacter2))
				{
					CurrentCombatDomain.SetProperLoopAniAndParticle(dataContext4, combatCharacter2);
				}
				CombatChar.StateMachine.TranslateState();
			}
		}
		else if (_translateStateDelayedFrame < 0)
		{
			DataContext dataContext5 = CombatChar.GetDataContext();
			CurrentCombatDomain.ApplyAgileOrDefenseSkill(CombatChar, _configData);
			CombatChar.SetPerformingSkillId(-1, dataContext5);
			_translateStateDelayedFrame = ((_configData.EquipType == 2) ? 1 : 0);
			DomainManager.Combat.RaiseCastSkillEnd(dataContext5, CombatChar.GetId(), CombatChar.IsAlly, _configData.TemplateId, 0);
		}
		if (_translateStateDelayedFrame < 0)
		{
			return false;
		}
		if (_translateStateDelayedFrame <= 0)
		{
			CombatChar.StateMachine.TranslateState();
		}
		_translateStateDelayedFrame--;
		return false;
	}

	private void PlayPrepareFinishAni()
	{
		DataContext dataContext = CombatChar.GetDataContext();
		bool flag = CombatChar.BossConfig != null;
		string text = ((_configData.Type != 13) ? "" : CurrentCombatDomain.GetMusicWeaponNameFix(CombatChar.GetWeaponData()));
		string text2 = ((string.IsNullOrEmpty(_configData.PlayerCastBossSkillPrepareAni) || flag) ? (_configData.PrepareAnimation + text + "_1_1") : (_configData.PlayerCastBossSkillPrepareAni + text + "_1_1"));
		string text3 = ((!flag) ? text2 : "C_007_1");
		float duration = AnimDataCollection.Data[(!flag) ? text3 : (CombatChar.BossConfig.AniPrefix[CombatChar.GetBossPhase()] + "C_007_1")].Duration;
		_prepareFinishAniFrame = (short)Math.Round(duration * 60f + 3f);
		CombatChar.SetAnimationToPlayOnce(text3, dataContext);
		CombatChar.SetSkillSoundToPlay("se_combat_preskill", dataContext);
	}
}
