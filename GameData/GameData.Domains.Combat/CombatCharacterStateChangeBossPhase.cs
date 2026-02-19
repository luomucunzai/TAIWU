using System;
using System.Collections.Generic;
using Config;
using GameData.Common;

namespace GameData.Domains.Combat;

public class CombatCharacterStateChangeBossPhase : CombatCharacterStateBase
{
	private const float ChangePhaseEffectTime = 10f;

	private const string SceneRuptureSound = "ui_battle_rupture";

	private short _setDataFrame;

	private short _setIdleAniFrame;

	private short _effectFrame;

	public CombatCharacterStateChangeBossPhase(CombatDomain combatDomain, CombatCharacter combatChar)
		: base(combatDomain, combatChar, CombatCharacterStateType.ChangeBossPhase)
	{
		IsUpdateOnPause = true;
	}

	public override void OnEnter()
	{
		DataContext dataContext = CombatChar.GetDataContext();
		int bossPhase = CombatChar.GetBossPhase();
		BossItem bossConfig = CombatChar.BossConfig;
		string failAnimation = bossConfig.FailAnimation;
		string fullAniName = bossConfig.AniPrefix[bossPhase] + failAnimation;
		bool flag = bossConfig.TemplateId == 10;
		short num = (short)AnimDataCollection.GetDurationFrame(fullAniName);
		_setDataFrame = 1;
		_setIdleAniFrame = ((flag && bossPhase == 4) ? ((short)(_effectFrame - 240)) : num);
		_effectFrame = ((bossConfig.HasSceneChangeEffect && CurrentCombatDomain.CombatConfig.Scene >= 0 && !flag) ? ((short)Math.Round(600.0)) : num);
		CombatChar.NeedChangeBossPhase = false;
		if (!CurrentCombatDomain.CombatConfig.StartInSecondPhase)
		{
			CombatChar.SetAnimationToPlayOnce(failAnimation, dataContext);
			CombatChar.SetAnimationToLoop(null, dataContext);
			CombatChar.SetParticleToPlay(bossConfig.FailParticles[bossPhase], dataContext);
			CombatChar.SetDieSoundToPlay(bossConfig.FailSounds[bossPhase], dataContext);
			if (bossConfig.HasSceneChangeEffect && CurrentCombatDomain.CombatConfig.Scene >= 0 && !flag && bossConfig.TemplateId != 9)
			{
				CombatChar.SetSkillSoundToPlay("ui_battle_rupture", dataContext);
			}
		}
		else
		{
			_setIdleAniFrame = 1;
			_effectFrame = 2;
		}
		List<string> failPlayerAni = bossConfig.FailPlayerAni;
		if (failPlayerAni != null && failPlayerAni.Count > bossPhase)
		{
			CurrentCombatDomain.GetCombatCharacter(!CombatChar.IsAlly).SetAnimationToPlayOnce(bossConfig.FailPlayerAni[bossPhase], dataContext);
			CombatChar.SetDisplayPosition(CurrentCombatDomain.GetDisplayPosition(CombatChar.IsAlly, bossConfig.FailAniDistance[bossPhase]), dataContext);
		}
	}

	public override void OnExit()
	{
	}

	public override bool OnUpdate()
	{
		if (!base.OnUpdate())
		{
			return false;
		}
		if (_setDataFrame > 0)
		{
			_setDataFrame--;
			if (_setDataFrame == 0)
			{
				CombatChar.SetBossPhase((sbyte)(CombatChar.GetBossPhase() + 1), CombatChar.GetDataContext());
			}
		}
		if (_setIdleAniFrame > 0)
		{
			_setIdleAniFrame--;
			if (_setIdleAniFrame == 0)
			{
				CombatChar.SetAnimationToLoop(CurrentCombatDomain.GetIdleAni(CombatChar), CombatChar.GetDataContext());
			}
		}
		if (_effectFrame > 0)
		{
			_effectFrame--;
			if (_effectFrame == 0)
			{
				DataContext dataContext = CombatChar.GetDataContext();
				if (CombatChar.ChangeBossPhaseEffectId >= 0)
				{
					DomainManager.Combat.ShowSpecialEffectTips(CombatChar.GetId(), CombatChar.ChangeBossPhaseEffectId, 0);
					CombatChar.SetXiangshuEffectId((short)CombatChar.ChangeBossPhaseEffectId, dataContext);
					CombatChar.ChangeBossPhaseEffectId = -1;
				}
				CombatChar.SetDisplayPosition(int.MinValue, dataContext);
				CombatChar.StateMachine.TranslateState();
			}
		}
		return false;
	}
}
