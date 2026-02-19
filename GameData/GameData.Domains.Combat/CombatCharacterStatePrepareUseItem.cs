using System.Collections.Generic;
using Config;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Character;
using GameData.Domains.CombatSkill;
using GameData.Domains.Item;
using GameData.Utilities;

namespace GameData.Domains.Combat;

public class CombatCharacterStatePrepareUseItem : CombatCharacterStateBase
{
	private sbyte _itemUseType;

	private List<sbyte> _itemTargetBodyParts;

	public CombatCharacterStatePrepareUseItem(CombatDomain combatDomain, CombatCharacter combatChar)
		: base(combatDomain, combatChar, CombatCharacterStateType.PrepareUseItem)
	{
	}

	public override void OnEnter()
	{
		if (CombatChar.GetPreparingItem().IsValid())
		{
			return;
		}
		base.OnEnter();
		AutoUpdateDelayCall = false;
		DataContext dataContext = CombatChar.GetDataContext();
		ItemKey needUseItem = CombatChar.NeedUseItem;
		ItemKey needRepairItem = CombatChar.NeedRepairItem;
		_itemUseType = CombatChar.ItemUseType;
		_itemTargetBodyParts = CombatChar.ItemTargetBodyParts;
		CombatChar.SetNeedUseItem(dataContext, ItemKey.Invalid);
		CombatChar.NeedRepairItem = ItemKey.Invalid;
		CombatChar.ItemUseType = -1;
		CombatChar.ItemTargetBodyParts = null;
		if (!CheckItemKeyIsValid(needUseItem))
		{
			ClearStateAndTranslateState();
			return;
		}
		if (needUseItem.ItemType == 12 && SharedConstValue.SwordFragment2BossId.ContainsKey(needUseItem.TemplateId))
		{
			short swordFragmentCurrSkill = DomainManager.Item.GetSwordFragmentCurrSkill(needUseItem);
			List<short> learnedCombatSkills = CombatChar.GetCharacter().GetLearnedCombatSkills();
			if (!learnedCombatSkills.Contains(swordFragmentCurrSkill))
			{
				sbyte behaviorType = CombatChar.GetCharacter().GetBehaviorType();
				byte pageInternalIndex = CombatSkillStateHelper.GetPageInternalIndex(behaviorType, 0, 0);
				ushort readingState = CombatSkillStateHelper.SetPageRead(0, pageInternalIndex);
				ushort activationState = CombatSkillStateHelper.SetPageActive(0, pageInternalIndex);
				for (byte b = 1; b <= 5; b++)
				{
					byte pageInternalIndex2 = CombatSkillStateHelper.GetPageInternalIndex(behaviorType, 0, b);
					readingState = CombatSkillStateHelper.SetPageRead(readingState, pageInternalIndex2);
					activationState = CombatSkillStateHelper.SetPageActive(activationState, pageInternalIndex2);
				}
				GameData.Domains.CombatSkill.CombatSkill combatSkill = DomainManager.CombatSkill.CreateCombatSkill(CombatChar.GetId(), swordFragmentCurrSkill, readingState);
				combatSkill.SetActivationState(activationState, dataContext);
				learnedCombatSkills.Add(swordFragmentCurrSkill);
				CombatChar.GetCharacter().SetLearnedCombatSkills(learnedCombatSkills, dataContext);
				CombatChar.ForgetAfterCombatSkills.Add(swordFragmentCurrSkill);
				DomainManager.SpecialEffect.Add(dataContext, CombatChar.GetId(), swordFragmentCurrSkill, 1, -1);
			}
			CombatChar.GetCharacter().ChangeXiangshuInfection(dataContext, GlobalConfig.Instance.UseSwordFragmentAddXiangshuInfection);
			CurrentCombatDomain.CastSkillFree(dataContext, CombatChar, swordFragmentCurrSkill, ECombatCastFreePriority.SwordFragment);
			CombatChar.StateMachine.TranslateState(CombatCharacterStateType.PrepareSkill);
			return;
		}
		int consumedFeatureMedals = needUseItem.GetConsumedFeatureMedals();
		if (!(CombatChar.IsAlly ? (CurrentCombatDomain.GetSelfTeamWisdomCount() >= consumedFeatureMedals) : (CurrentCombatDomain.GetEnemyTeamWisdomCount() >= consumedFeatureMedals)))
		{
			ClearStateAndTranslateState();
			return;
		}
		if (consumedFeatureMedals > 0)
		{
			CurrentCombatDomain.CostWisdom(dataContext, CombatChar.IsAlly, consumedFeatureMedals);
		}
		int num = needUseItem.GetConfigAs<ICombatItemConfig>()?.UseFrame ?? 0;
		CombatChar.SetPreparingItem(needUseItem, dataContext);
		if (CombatChar.GetUseItemPreparePercent() != 0)
		{
			CombatChar.SetUseItemPreparePercent(0, dataContext);
		}
		sbyte itemType = needUseItem.ItemType;
		bool flag = ((itemType == 7 || itemType == 9) ? true : false);
		if (flag || (needUseItem.ItemType == 8 && _itemUseType == 0))
		{
			goto IL_036c;
		}
		if (needUseItem.ItemType == 12)
		{
			short templateId = needUseItem.TemplateId;
			if (templateId >= 0 && templateId <= 8)
			{
				goto IL_036c;
			}
		}
		if (needUseItem.ItemType == 8)
		{
			CombatItemUseItem prepareThrowPoison = CombatItemUse.DefValue.PrepareThrowPoison;
			CombatChar.SetAnimationToPlayOnce(prepareThrowPoison.Animation, dataContext);
			CombatChar.SetParticleToPlay(prepareThrowPoison.Particle, dataContext);
			CombatChar.SetSkillSoundToPlay(prepareThrowPoison.Sound, dataContext);
		}
		else if (needUseItem.ItemType == 6)
		{
			CombatChar.RepairingItem = needRepairItem;
			ItemBase baseItem = DomainManager.Item.GetBaseItem(needRepairItem);
			if (baseItem.GetMaxDurability() == baseItem.GetCurrDurability())
			{
				ClearStateAndTranslateState();
				return;
			}
			num *= baseItem.GetMaxDurability() - baseItem.GetCurrDurability();
			num *= ((baseItem.GetCurrDurability() > 0) ? 1 : 2);
			CombatItemUseItem combatItemUseItem = CombatItemUse.Instance[(short)3];
			CombatChar.SpecialAnimationLoop = combatItemUseItem.Animation;
			CombatChar.SetAnimationToLoop(combatItemUseItem.Animation, dataContext);
			CombatChar.SetParticleToLoop(combatItemUseItem.Particle, dataContext);
		}
		else
		{
			if (needUseItem.ItemType == 12)
			{
				short templateId = needUseItem.TemplateId;
				if (templateId >= 73 && templateId <= 81)
				{
					CombatItemUseItem combatItemUseItem2 = CombatItemUse.Instance[(short)4];
					CombatChar.SpecialAnimationLoop = combatItemUseItem2.Animation;
					CombatChar.SetAnimationToLoop(combatItemUseItem2.Animation, dataContext);
					CombatChar.SetParticleToLoop(combatItemUseItem2.Particle, dataContext);
					CombatChar.SetSoundToLoop(combatItemUseItem2.Sound, dataContext);
					goto IL_0619;
				}
			}
			if (needUseItem.ItemType == 12)
			{
				short templateId = needUseItem.TemplateId;
				if (templateId >= 375 && templateId <= 384)
				{
					CombatItemUseItem combatItemUseItem3 = CombatItemUse.Instance[Config.Misc.Instance[needUseItem.TemplateId].CombatPrepareUseEffect];
					CombatChar.SetAnimationToPlayOnce(combatItemUseItem3.Animation, dataContext);
					CombatChar.SetParticleToPlay(combatItemUseItem3.Particle, dataContext);
					goto IL_0619;
				}
			}
			CombatItemUseItem combatItemUseItem4 = CombatItemUse.Instance[(short)2];
			CombatChar.SetAnimationToPlayOnce(combatItemUseItem4.Animation, dataContext);
		}
		goto IL_0619;
		IL_0619:
		DomainManager.Combat.UpdateAllTeammateCommandUsable(dataContext, CombatChar.IsAlly, ETeammateCommandImplement.InterruptOtherAction);
		DelayCall(OnPrepared, OnPrepareTickPercent, num);
		return;
		IL_036c:
		CombatItemUseItem eatItem = CombatItemUse.DefValue.EatItem;
		CombatChar.SetAnimationToPlayOnce(eatItem.Animation, dataContext);
		CombatChar.SetParticleToPlay((CombatChar.BossConfig == null) ? eatItem.Particle : CombatChar.BossConfig.EatParticles[CombatChar.GetBossPhase()], dataContext);
		goto IL_0619;
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
		if (CheckInterruptPrepare())
		{
			return false;
		}
		UpdateDelayCall();
		return false;
	}

	private bool CheckInterruptPrepare()
	{
		bool flag = !CombatChar.GetValidItems().Contains(CombatChar.GetPreparingItem());
		bool flag2 = CombatChar.GetPreparingItem().ItemType == 6 && DomainManager.Item.TryGetBaseItem(CombatChar.RepairingItem) == null;
		if (!flag && !flag2)
		{
			return false;
		}
		ClearStateAndTranslateState();
		return true;
	}

	private void OnPrepareTickPercent(int preparePercent)
	{
		if (preparePercent != CombatChar.GetUseItemPreparePercent())
		{
			CombatChar.SetUseItemPreparePercent((byte)preparePercent, CombatChar.GetDataContext());
		}
	}

	private void OnPrepared()
	{
		DataContext dataContext = CombatChar.GetDataContext();
		ItemKey preparingItem = CombatChar.GetPreparingItem();
		bool flag = false;
		if (preparingItem.ItemType != 7 && preparingItem.ItemType != 9 && (preparingItem.ItemType != 8 || _itemUseType != 0))
		{
			if (preparingItem.ItemType == 12)
			{
				short templateId = preparingItem.TemplateId;
				if (templateId >= 0 && templateId <= 8)
				{
					goto IL_0090;
				}
			}
			if (!ItemTemplateHelper.IsTianJieFuLu(preparingItem.ItemType, preparingItem.TemplateId) && (preparingItem.ItemType != 5 || !DomainManager.Extra.IsProfessionalSkillUnlockedAndEquipped(44)))
			{
				if (preparingItem.ItemType == 6)
				{
					short durability = DomainManager.Building.RepairItemOptional(dataContext, CombatChar.GetId(), preparingItem, CombatChar.RepairingItem, 1).Durability;
					if (DomainManager.Combat.TryGetElement_WeaponDataDict(CombatChar.RepairingItem.Id, out var element))
					{
						element.SetDurability(durability, dataContext);
					}
					DomainManager.Combat.EnsureOldDurability(CombatChar.RepairingItem);
					CombatChar.SetParticleToLoop(null, dataContext);
				}
				else
				{
					if (preparingItem.ItemType == 12)
					{
						short templateId = preparingItem.TemplateId;
						if (templateId >= 200 && templateId <= 209)
						{
							goto IL_023f;
						}
					}
					flag = true;
				}
				goto IL_023f;
			}
		}
		goto IL_0090;
		IL_0090:
		if (ItemTemplateHelper.IsTianJieFuLu(preparingItem.ItemType, preparingItem.TemplateId))
		{
			DomainManager.Extra.EatTianJieFuLu(dataContext, CombatChar.GetId(), preparingItem, ItemTemplateHelper.GetTianJieFuLuCountUnit());
		}
		else
		{
			DomainManager.Character.AddEatingItem(dataContext, CombatChar.GetId(), preparingItem, _itemTargetBodyParts);
		}
		SyncInjuryAndPoison();
		if (!EatingItems.IsWug(preparingItem))
		{
			Events.RaiseUsedMedicine(dataContext, CombatChar.GetId(), preparingItem);
		}
		else
		{
			CurrentCombatDomain.ShowWugKingEffectTips(dataContext, CombatChar.GetId(), CombatChar.GetId());
		}
		if (CombatChar.GetOldDisorderOfQi() > CombatChar.GetCharacter().GetDisorderOfQi())
		{
			CombatChar.SetOldDisorderOfQi(CombatChar.GetCharacter().GetDisorderOfQi(), dataContext);
		}
		goto IL_023f;
		IL_023f:
		CombatChar.SpecialAnimationLoop = null;
		if (flag)
		{
			CombatChar.UsingItem = CombatChar.GetPreparingItem();
			CombatChar.StateMachine.TranslateState(CombatCharacterStateType.UseItem);
		}
		else if (CombatChar.NeedUseItem.IsValid())
		{
			ClearState();
			OnEnter();
		}
		else
		{
			ClearStateAndTranslateState();
		}
	}

	private void ClearState()
	{
		DataContext dataContext = CombatChar.GetDataContext();
		CombatChar.SetPreparingItem(ItemKey.Invalid, dataContext);
		CombatChar.RepairingItem = ItemKey.Invalid;
		CombatChar.SetParticleToLoop(null, dataContext);
		CombatChar.SpecialAnimationLoop = null;
	}

	private void ClearStateAndTranslateState()
	{
		ClearState();
		CombatChar.StateMachine.TranslateState();
	}

	private unsafe void SyncInjuryAndPoison()
	{
		DataContext dataContext = CombatChar.GetDataContext();
		PoisonInts poisoned = CombatChar.GetCharacter().GetPoisoned();
		PoisonInts poison = CombatChar.GetPoison();
		DomainManager.Combat.SetPoisons(dataContext, CombatChar, poisoned);
		for (sbyte b = 0; b < 6; b++)
		{
			int num = poisoned.Items[b] - poison.Items[b];
			if (num > 0)
			{
				Events.RaiseAddPoison(dataContext, -1, CombatChar.GetId(), b, 0, num, -1, canBounce: false);
			}
		}
		DomainManager.Combat.SetInjuries(dataContext, CombatChar, CombatChar.GetCharacter().GetInjuries());
	}

	private bool CheckItemKeyIsValid(ItemKey itemKey)
	{
		if (!itemKey.IsValid())
		{
			return false;
		}
		sbyte itemType = itemKey.ItemType;
		if (1 == 0)
		{
		}
		GameData.Domains.Item.CraftTool element;
		bool result = ((itemType != 6) ? CombatChar.GetValidItems().Contains(itemKey) : (DomainManager.Item.TryGetElement_CraftTools(itemKey.Id, out element) && element.GetCurrDurability() >= 0));
		if (1 == 0)
		{
		}
		return result;
	}
}
