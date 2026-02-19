using System;
using Config;
using GameData.Common;

namespace GameData.Domains.Combat;

public class CombatCharacterStateJumpMove : CombatCharacterStateBase
{
	private short _aniFrame;

	private short _changeDistanceFrame;

	public CombatCharacterStateJumpMove(CombatDomain combatDomain, CombatCharacter combatChar)
		: base(combatDomain, combatChar, CombatCharacterStateType.JumpMove)
	{
		IsUpdateOnPause = true;
	}

	public override void OnEnter()
	{
		DataContext dataContext = CombatChar.GetDataContext();
		CombatSkillItem combatSkillItem = Config.CombatSkill.Instance[CombatChar.PauseJumpMoveSkillId];
		string text = combatSkillItem.JumpAni[(!CombatChar.MoveForward) ? 1u : 0u];
		string particleToPlay = combatSkillItem.JumpParticle[(!CombatChar.MoveForward) ? 1u : 0u];
		float jumpChangeDistanceDuration = ((combatSkillItem.JumpChangeDistanceDuration > 0) ? ((float)combatSkillItem.JumpChangeDistanceDuration / 60f) : AnimDataCollection.Data[text].Duration);
		CombatChar.NeedPauseJumpMove = false;
		CombatChar.SetAnimationToPlayOnce(text, dataContext);
		CombatChar.SetParticleToPlay(particleToPlay, dataContext);
		CombatChar.SetJumpChangeDistanceDuration(jumpChangeDistanceDuration, dataContext);
		CurrentCombatDomain.UpdateAllTeammateCommandUsable(dataContext, CombatChar.IsAlly, -1);
		_aniFrame = (short)AnimDataCollection.GetDurationFrame(text);
		_changeDistanceFrame = Math.Max(combatSkillItem.JumpChangeDistanceFrame, (short)1);
	}

	public override void OnExit()
	{
		CombatChar.SetJumpChangeDistanceDuration(-1f, CombatChar.GetDataContext());
		if (CombatChar.MoveData.JumpMoveSkillId < 0)
		{
			CombatChar.PauseJumpMoveSkillId = -1;
		}
	}

	public override bool OnUpdate()
	{
		if (!base.OnUpdate())
		{
			return false;
		}
		if (_changeDistanceFrame > 0)
		{
			_changeDistanceFrame--;
			if (_changeDistanceFrame == 0)
			{
				DomainManager.Combat.ChangeDistance(CombatChar.GetDataContext(), CombatChar, CombatChar.PauseJumpMoveDistance);
			}
		}
		if (_aniFrame > 0)
		{
			_aniFrame--;
			if (_aniFrame == 0)
			{
				CombatChar.StateMachine.TranslateState();
			}
		}
		return false;
	}
}
