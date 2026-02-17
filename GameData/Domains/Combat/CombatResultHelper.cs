using System;
using System.Collections.Generic;
using GameData.Domains.Character;
using GameData.Domains.Character.Display;
using GameData.Domains.Item;
using GameData.Domains.Item.Display;

namespace GameData.Domains.Combat
{
	// Token: 0x020006B9 RID: 1721
	public static class CombatResultHelper
	{
		// Token: 0x06006624 RID: 26148 RVA: 0x003AA630 File Offset: 0x003A8830
		public static void Reset(this CombatResultDisplayData displayData)
		{
			displayData.CombatStatus = -1;
			displayData.SnapshotBeforeCombat = CombatResultHelper.CreateSnapshot();
			displayData.Exp = 0;
			displayData.Resource.Initialize();
			displayData.AreaSpiritualDebt = 0;
			displayData.ShowReadingEvent = false;
			List<sbyte> evaluationList = displayData.EvaluationList;
			if (evaluationList != null)
			{
				evaluationList.Clear();
			}
			List<ItemDisplayData> itemList = displayData.ItemList;
			if (itemList != null)
			{
				itemList.Clear();
			}
			List<CharacterDisplayData> charList = displayData.CharList;
			if (charList != null)
			{
				charList.Clear();
			}
			List<short> legacyTemplateIds = displayData.LegacyTemplateIds;
			if (legacyTemplateIds != null)
			{
				legacyTemplateIds.Clear();
			}
			Dictionary<short, int> changedProficiencies = displayData.ChangedProficiencies;
			if (changedProficiencies != null)
			{
				changedProficiencies.Clear();
			}
			Dictionary<short, int> changedProficienciesDelta = displayData.ChangedProficienciesDelta;
			if (changedProficienciesDelta != null)
			{
				changedProficienciesDelta.Clear();
			}
			Dictionary<ItemKey, int> itemSrcCharDict = displayData.ItemSrcCharDict;
			if (itemSrcCharDict != null)
			{
				itemSrcCharDict.Clear();
			}
		}

		// Token: 0x06006625 RID: 26149 RVA: 0x003AA6F0 File Offset: 0x003A88F0
		public unsafe static CombatResultSnapshot CreateSnapshot()
		{
			Character taiwu = DomainManager.Taiwu.GetTaiwu();
			int taiwuId = taiwu.GetId();
			short areaId = taiwu.GetLocation().IsValid() ? taiwu.GetLocation().AreaId : taiwu.GetValidLocation().AreaId;
			return new CombatResultSnapshot
			{
				Exp = taiwu.GetExp(),
				Resource = *taiwu.GetResources(),
				AreaSpiritualDebt = DomainManager.Extra.GetAreaSpiritualDebt(areaId),
				CanEatingMaxCount = taiwu.GetCurrMaxEatingSlotsCount(),
				EatingItemList = *taiwu.GetEatingItems(),
				Injuries = taiwu.GetInjuries(),
				Poisons = *taiwu.GetPoisoned(),
				PoisonResists = *taiwu.GetPoisonResists(),
				ImmunePoisonExtra = DomainManager.Extra.GetPoisonImmunities(taiwuId),
				MainAttribute = taiwu.GetMainAttributesRecoveries(),
				DisorderOfQi = taiwu.GetDisorderOfQi(),
				ChangeOfQiDisorder = taiwu.GetChangeOfQiDisorder(),
				TemplateId = taiwu.Template.TemplateId,
				DisplayAge = DomainManager.Character.GetDisplayingAge(taiwuId),
				ActualAge = taiwu.GetActualAge(),
				BirthMonth = taiwu.GetBirthMonth(),
				Health = taiwu.GetHealth(),
				LeftMaxHealth = taiwu.GetLeftMaxHealth(false),
				HealthRecovery = taiwu.GetHealthRecovery(taiwu.IsCompletelyInfected()),
				CreatingType = taiwu.GetCreatingType()
			};
		}
	}
}
