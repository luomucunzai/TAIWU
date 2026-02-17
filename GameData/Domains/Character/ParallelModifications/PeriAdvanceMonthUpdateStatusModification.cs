using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using GameData.Domains.Item;

namespace GameData.Domains.Character.ParallelModifications
{
	// Token: 0x02000831 RID: 2097
	public class PeriAdvanceMonthUpdateStatusModification
	{
		// Token: 0x06007581 RID: 30081 RVA: 0x0044A5FB File Offset: 0x004487FB
		public PeriAdvanceMonthUpdateStatusModification(Character character)
		{
			this.Character = character;
			this.XiangshuInfectionFeatureChanged = -1;
			this.NewClothingTemplateId = -1;
		}

		// Token: 0x04001F97 RID: 8087
		public readonly Character Character;

		// Token: 0x04001F98 RID: 8088
		[TupleElementNames(new string[]
		{
			"itemKey",
			"amount"
		})]
		public List<ValueTuple<ItemKey, int>> ItemsToBeUsed;

		// Token: 0x04001F99 RID: 8089
		public List<ItemKey> RemovedWugKings;

		// Token: 0x04001F9A RID: 8090
		public List<ItemKey> RemovedSafetyWugKings;

		// Token: 0x04001F9B RID: 8091
		public List<short> RemovedWugs;

		// Token: 0x04001F9C RID: 8092
		public bool IsAssassinated;

		// Token: 0x04001F9D RID: 8093
		public bool ResourcesChanged;

		// Token: 0x04001F9E RID: 8094
		public bool EatingItemsChanged;

		// Token: 0x04001F9F RID: 8095
		public bool CurrNeiliChanged;

		// Token: 0x04001FA0 RID: 8096
		public bool QiDisorderChanged;

		// Token: 0x04001FA1 RID: 8097
		public bool InjuriesChanged;

		// Token: 0x04001FA2 RID: 8098
		public bool PoisonedChanged;

		// Token: 0x04001FA3 RID: 8099
		public bool XiangshuInfectionChanged;

		// Token: 0x04001FA4 RID: 8100
		public short XiangshuInfectionFeatureChanged;

		// Token: 0x04001FA5 RID: 8101
		public bool FeaturesChanged;

		// Token: 0x04001FA6 RID: 8102
		public bool RecreateTeammateCommands;

		// Token: 0x04001FA7 RID: 8103
		public bool PersonalNeedsChanged;

		// Token: 0x04001FA8 RID: 8104
		public bool CurrAgeChanged;

		// Token: 0x04001FA9 RID: 8105
		public bool ActualAgeChanged;

		// Token: 0x04001FAA RID: 8106
		public bool HealthChanged;

		// Token: 0x04001FAB RID: 8107
		public bool MaxHealthChanged;

		// Token: 0x04001FAC RID: 8108
		public bool CurrMainAttributesChanged;

		// Token: 0x04001FAD RID: 8109
		public bool HobbyChanged;

		// Token: 0x04001FAE RID: 8110
		[TupleElementNames(new string[]
		{
			"charId",
			"favorability"
		})]
		public List<ValueTuple<int, short>> FavorabilitiesOfRelatedChars;

		// Token: 0x04001FAF RID: 8111
		public short NewClothingTemplateId;

		// Token: 0x04001FB0 RID: 8112
		public PregnantStateModification PregnantStateModification;

		// Token: 0x04001FB1 RID: 8113
		public sbyte UsedHealingCount;

		// Token: 0x04001FB2 RID: 8114
		public sbyte UsedDetoxCount;

		// Token: 0x04001FB3 RID: 8115
		public sbyte UsedBreathingCount;

		// Token: 0x04001FB4 RID: 8116
		public sbyte UsedRecoverCount;

		// Token: 0x04001FB5 RID: 8117
		public bool InventoryChanged;

		// Token: 0x04001FB6 RID: 8118
		public bool HappinessChanged;

		// Token: 0x04001FB7 RID: 8119
		public List<ItemKey> ConsumedForbiddenFoodsOrWines;

		// Token: 0x04001FB8 RID: 8120
		public List<ValueTuple<sbyte, int>> MixedPoisonEffects;
	}
}
