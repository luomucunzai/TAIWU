using System;
using System.Collections.Generic;
using Config;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.CombatSkill;

namespace GameData.Domains.Combat;

public class CombatCharacterStatePrepareSkill : CombatCharacterStateBase
{
	public CombatCharacterStatePrepareSkill(CombatDomain combatDomain, CombatCharacter combatChar)
		: base(combatDomain, combatChar, CombatCharacterStateType.PrepareSkill)
	{
	}

	public override void OnEnter()
	{
		DataContext dataContext = CombatChar.GetDataContext();
		short num;
		if (CombatChar.NeedUseSkillFreeId >= 0)
		{
			num = CombatChar.NeedUseSkillFreeId;
			List<CastFreeData> castFreeDataList = CombatChar.CastFreeDataList;
			ECombatCastFreePriority priority = castFreeDataList[castFreeDataList.Count - 1].Priority;
			CombatChar.CastFreeDataList.RemoveAt(CombatChar.CastFreeDataList.Count - 1);
			CombatChar.SetAutoCastingSkill(autoCastingSkill: false, dataContext);
			if (!CurrentCombatDomain.CanCastSkill(CombatChar, num, costFree: true) && priority != ECombatCastFreePriority.Gm)
			{
				CombatChar.StateMachine.TranslateState();
				return;
			}
			if (priority != ECombatCastFreePriority.Gm)
			{
				CombatChar.SetAutoCastingSkill(autoCastingSkill: true, dataContext);
			}
			CombatChar.MoveData.ResetJumpState(dataContext, calcPreparedMove: false);
		}
		else if (CombatChar.NeedChangeSkill)
		{
			num = CombatChar.NeedUseSkillId;
			CombatChar.SetNeedUseSkillId(dataContext, -1);
			CombatChar.SetAutoCastingSkill(autoCastingSkill: false, dataContext);
			if (num == CombatChar.NeedAddEffectAgileSkillId || num == CombatChar.GetAffectingMoveSkillId())
			{
				CombatChar.StateMachine.TranslateState();
				return;
			}
			if (!CurrentCombatDomain.CanCastSkill(CombatChar, num))
			{
				CombatChar.StateMachine.TranslateState();
				return;
			}
			CurrentCombatDomain.DoCombatSkillCost(dataContext, CombatChar, num);
		}
		else
		{
			num = -1;
		}
		if (num >= 0)
		{
			if (CombatChar.GetAffectingMoveSkillId() >= 0 && DomainManager.CombatSkill.GetSkillType(CombatChar.GetId(), num) == 5)
			{
				Events.RaiseCastLegSkillWithAgile(dataContext, CombatChar, num);
			}
			num = (short)DomainManager.SpecialEffect.ModifyData(CombatChar.GetId(), (short)(-1), (ushort)156, (int)num, -1, -1, -1);
			CombatSkillItem combatSkillItem = Config.CombatSkill.Instance[num];
			GameData.Domains.CombatSkill.CombatSkill element_CombatSkills = DomainManager.CombatSkill.GetElement_CombatSkills(new CombatSkillKey(CombatChar.GetId(), num));
			int num2 = element_CombatSkills.GetPrepareTotalProgress();
			if (CurrentCombatDomain.SkillBodyPartHasHeavyInjury(CombatChar, num))
			{
				num2 *= 2;
			}
			CombatChar.SkillPrepareTotalProgress = num2;
			CombatChar.SkillPrepareCurrProgress = 0;
			CombatChar.SetPreparingSkillId(num, dataContext);
			DomainManager.Combat.UpdateAllTeammateCommandUsable(dataContext, CombatChar.IsAlly, -1);
			Events.RaisePrepareSkillEffectNotYetCreated(dataContext, CombatChar, num);
			if (combatSkillItem.EquipType == 1)
			{
				DomainManager.SpecialEffect.Add(dataContext, CombatChar.GetId(), num, 0, -1);
			}
			Events.RaisePrepareSkillBegin(dataContext, CombatChar.GetId(), CombatChar.IsAlly, num);
		}
		CurrentCombatDomain.SetProperLoopAniAndParticle(dataContext, CombatChar);
		if (DomainManager.Combat.TryGetCombatSkillData(CombatChar.GetId(), CombatChar.GetPreparingSkillId(), out var combatSkillData) && combatSkillData.GetSilencing())
		{
			DomainManager.Combat.InterruptSkill(dataContext, CombatChar);
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
		if (DomainManager.Combat.GetCombatCharacter(!CombatChar.IsAlly).NeedChangeBossPhase)
		{
			return false;
		}
		if (CombatChar.GetPreparingSkillId() < 0)
		{
			CombatCharacterStateType properState = CombatChar.StateMachine.GetProperState();
			CombatChar.MoveData.ClearSkillPrepareMoveDist();
			if (properState != CombatCharacterStateType.PrepareSkill)
			{
				CombatChar.StateMachine.TranslateState(properState);
			}
			else
			{
				OnEnter();
			}
			return false;
		}
		DataContext dataContext = CombatChar.GetDataContext();
		int val = CombatChar.SkillPrepareCurrProgress + CurrentCombatDomain.GetSkillPrepareSpeed(CombatChar);
		CombatChar.SkillPrepareCurrProgress = Math.Min(val, CombatChar.SkillPrepareTotalProgress);
		byte b = (byte)CValuePercent.ParseInt(CombatChar.SkillPrepareCurrProgress, CombatChar.SkillPrepareTotalProgress);
		if (b != CombatChar.GetSkillPreparePercent())
		{
			CombatChar.SetSkillPreparePercent(b, CombatChar.GetDataContext());
			Events.RaisePrepareSkillProgressChange(CombatChar.GetDataContext(), CombatChar.GetId(), CombatChar.IsAlly, CombatChar.GetPreparingSkillId(), (sbyte)b);
		}
		if (CombatChar.SkillPrepareCurrProgress == CombatChar.SkillPrepareTotalProgress)
		{
			short preparingSkillId = CombatChar.GetPreparingSkillId();
			CombatSkillItem combatSkillItem = Config.CombatSkill.Instance[preparingSkillId];
			CurrentCombatDomain.CalcSkillQiDisorderAndInjury(CombatChar, combatSkillItem);
			if (CurrentCombatDomain.IsMainCharacter(CombatChar))
			{
				CurrentCombatDomain.UpdateAllTeammateCommandUsable(dataContext, CombatChar.IsAlly, -1);
			}
			if (combatSkillItem.EquipType == 1 && CurrentCombatDomain.IsMainCharacter(CombatChar))
			{
				CurrentCombatDomain.ForceAllTeammateLeaveCombatField(dataContext, CombatChar.IsAlly);
			}
			CombatChar.MoveData.ClearSkillPrepareMoveDist();
			CombatChar.StateMachine.TranslateState((!CurrentCombatDomain.IsCharacterFallen(CombatChar)) ? CombatCharacterStateType.CastSkill : CombatCharacterStateType.Idle);
			return false;
		}
		if (CombatChar.NeedChangeSkill)
		{
			if (Config.CombatSkill.Instance[CombatChar.NeedUseSkillId].EquipType == 1)
			{
				Events.RaiseChangePreparingSkillBegin(dataContext, CombatChar.GetId(), CombatChar.GetPreparingSkillId(), CombatChar.NeedUseSkillId);
				OnEnter();
			}
			else
			{
				CurrentCombatDomain.CastAgileOrDefenseWithoutPrepare(CombatChar, CombatChar.NeedUseSkillId);
				CombatChar.SetNeedUseSkillId(CombatChar.GetDataContext(), -1);
			}
		}
		if (CombatChar.NeedNormalAttack)
		{
			CombatChar.StateMachine.TranslateState(CombatCharacterStateType.PrepareAttack);
		}
		if (CombatChar.NeedUnlockAttack)
		{
			CombatChar.StateMachine.TranslateState(CombatCharacterStateType.UnlockAttack);
		}
		if (CombatChar.NeedShowChangeTrick)
		{
			CombatChar.StateMachine.TranslateState(CombatCharacterStateType.SelectChangeTrick);
		}
		return false;
	}
}
