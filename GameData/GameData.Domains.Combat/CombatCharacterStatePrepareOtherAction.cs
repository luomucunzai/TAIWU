using System;
using Config;
using GameData.Common;
using GameData.Domains.Character;
using GameData.Domains.Item;
using GameData.Domains.Taiwu.Profession;
using GameData.Domains.Taiwu.Profession.SkillsData;
using GameData.Utilities;

namespace GameData.Domains.Combat;

public class CombatCharacterStatePrepareOtherAction : CombatCharacterStateBase
{
	private short _totalPrepareFrame;

	private short _leftPrepareFrame;

	public CombatCharacterStatePrepareOtherAction(CombatDomain combatDomain, CombatCharacter combatChar)
		: base(combatDomain, combatChar, CombatCharacterStateType.PrepareOtherAction)
	{
	}

	public override void OnEnter()
	{
		DataContext dataContext = CombatChar.GetDataContext();
		if (CombatChar.GetPreparingOtherAction() < 0)
		{
			sbyte needUseOtherAction = CombatChar.NeedUseOtherAction;
			CombatChar.SetNeedUseOtherAction(dataContext, -1);
			if (!CheckOtherActionUsable(needUseOtherAction))
			{
				TranslateProperState();
				return;
			}
			DoOtherActionCost(dataContext, needUseOtherAction);
			short otherActionPrepareFrame = CombatChar.GetOtherActionPrepareFrame(needUseOtherAction);
			_leftPrepareFrame = (_totalPrepareFrame = otherActionPrepareFrame);
			CombatChar.NeedInterruptSurrender = false;
			CombatChar.SetPreparingOtherAction(needUseOtherAction, dataContext);
			if (CombatChar.GetOtherActionPreparePercent() != 0)
			{
				CombatChar.SetOtherActionPreparePercent(0, dataContext);
			}
			CombatChar.CanFleeOutOfRange = false;
		}
		CurrentCombatDomain.SetProperLoopAniAndParticle(dataContext, CombatChar);
		CurrentCombatDomain.UpdateAllTeammateCommandUsable(dataContext, CombatChar.IsAlly, ETeammateCommandImplement.InterruptOtherAction);
	}

	public override bool OnUpdate()
	{
		if (!base.OnUpdate())
		{
			return false;
		}
		if (CombatChar.GetPreparingOtherAction() < 0)
		{
			TranslateProperState();
			return false;
		}
		if (CombatChar.GetPreparingOtherAction() == 4)
		{
			if (CombatChar.NeedInterruptSurrender)
			{
				TranslateProperState();
			}
			return false;
		}
		if (_leftPrepareFrame <= 0)
		{
			AdaptableLog.TagWarning("CombatCharacterStatePrepareOtherAction", PredefinedLog.Instance[(short)5].Info, appendWarningMessage: true);
			TranslateProperState();
			return false;
		}
		_leftPrepareFrame--;
		byte b = (byte)((_totalPrepareFrame - _leftPrepareFrame) * 100 / _totalPrepareFrame);
		if (b != CombatChar.GetOtherActionPreparePercent())
		{
			CombatChar.SetOtherActionPreparePercent(b, CombatChar.GetDataContext());
		}
		if (_leftPrepareFrame == 0)
		{
			DataContext dataContext = CombatChar.GetDataContext();
			sbyte preparingOtherAction = CombatChar.GetPreparingOtherAction();
			switch (preparingOtherAction)
			{
			case 0:
				CurrentCombatDomain.HealInjuryInCombat(dataContext, CombatChar, CombatChar);
				break;
			case 1:
				CurrentCombatDomain.HealPoisonInCombat(dataContext, CombatChar, CombatChar);
				break;
			case 2:
				CurrentCombatDomain.Flee(dataContext, CombatChar);
				break;
			case 3:
				CombatChar.NeedAnimalAttack = true;
				break;
			}
			if (preparingOtherAction != 3 && CombatChar.NeedUseOtherAction != -1)
			{
				ReEnter(dataContext);
			}
			else
			{
				TranslateProperState();
			}
			if (CombatDomain.OtherActionSpecialEffectId.Length > preparingOtherAction)
			{
				CombatChar.GetShowEffectList().ShowEffectList.Add(new ShowSpecialEffectDisplayData(CombatChar.GetId(), CombatDomain.OtherActionSpecialEffectId[preparingOtherAction], 0, ItemKey.Invalid));
			}
		}
		if (CombatChar.NeedUseSkillFreeId >= 0)
		{
			CombatChar.StateMachine.TranslateState(CombatCharacterStateType.PrepareSkill);
		}
		if (CombatChar.NeedNormalAttack)
		{
			CombatChar.StateMachine.TranslateState(CombatCharacterStateType.PrepareAttack);
		}
		return false;
	}

	private bool CheckOtherActionUsable(sbyte actionType)
	{
		if ((actionType < 0 || actionType >= 5) ? true : false)
		{
			return false;
		}
		if (1 == 0)
		{
		}
		bool flag = actionType switch
		{
			0 => CombatChar.GetHealInjuryCount() > 0, 
			1 => CombatChar.GetHealPoisonCount() > 0, 
			2 => CombatChar.CanFleeOutOfRange || CombatChar.GetOtherActionCanUse()[2], 
			3 => CombatChar.GetAnimalAttackCount() > 0, 
			_ => true, 
		};
		if (1 == 0)
		{
		}
		if (!flag)
		{
			return false;
		}
		return !CombatChar.IsAlly || CombatChar.GetOtherActionCanUse()[actionType];
	}

	private void DoOtherActionCost(DataContext context, sbyte actionType)
	{
		switch (actionType)
		{
		case 0:
			CombatChar.SetHealInjuryCount((byte)Math.Max(CombatChar.GetHealInjuryCount() - 1, 0), context);
			DomainManager.Character.UseCombatResources(context, CombatChar.GetId(), EHealActionType.Healing, 1);
			break;
		case 1:
			CombatChar.SetHealPoisonCount((byte)Math.Max(CombatChar.GetHealPoisonCount() - 1, 0), context);
			DomainManager.Character.UseCombatResources(context, CombatChar.GetId(), EHealActionType.Detox, 1);
			break;
		case 3:
		{
			CombatChar.SetAnimalAttackCount((sbyte)Math.Max(CombatChar.GetAnimalAttackCount() - 1, 0), context);
			ProfessionData professionData = DomainManager.Extra.GetProfessionData(1);
			((HunterSkillsData)professionData.SkillsData).UsedCarrierAnimalAttackCount++;
			DomainManager.Extra.SetProfessionData(context, professionData);
			break;
		}
		case 2:
			break;
		}
	}

	private void ReEnter(DataContext context)
	{
		CombatChar.SetPreparingOtherAction(-1, context);
		OnEnter();
	}

	private void TranslateProperState()
	{
		DataContext dataContext = CombatChar.GetDataContext();
		OtherActionTypeItem otherActionTypeItem = Config.OtherActionType.Instance[CombatChar.GetPreparingOtherAction()];
		CombatChar.SetPreparingOtherAction(-1, dataContext);
		if (otherActionTypeItem != null)
		{
			if (CombatChar.IsActorSkeleton && !string.IsNullOrEmpty(otherActionTypeItem.PrepareEndAnim))
			{
				CombatChar.SetAnimationToPlayOnce(otherActionTypeItem.PrepareEndAnim, dataContext);
			}
			if (CombatChar.IsActorSkeleton && !string.IsNullOrEmpty(otherActionTypeItem.PrepareEndParticle))
			{
				CombatChar.SetParticleToPlay(otherActionTypeItem.PrepareEndParticle, dataContext);
			}
		}
		CombatCharacterStateType properState = CombatChar.StateMachine.GetProperState();
		if (properState != CombatCharacterStateType.PrepareOtherAction)
		{
			CombatChar.StateMachine.TranslateState();
		}
		else
		{
			OnEnter();
		}
	}
}
