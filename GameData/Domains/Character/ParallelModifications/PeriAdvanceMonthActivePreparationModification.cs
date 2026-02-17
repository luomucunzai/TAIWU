using System;
using System.Collections.Generic;
using GameData.Domains.Item;

namespace GameData.Domains.Character.ParallelModifications
{
	// Token: 0x0200082A RID: 2090
	public class PeriAdvanceMonthActivePreparationModification
	{
		// Token: 0x17000489 RID: 1161
		// (get) Token: 0x06007574 RID: 30068 RVA: 0x0044A417 File Offset: 0x00448617
		public bool IsChanged
		{
			get
			{
				return this.ResourcesChanged || this.PersonalNeedChanged || this.PoisonsToUse != null || (this.FeedingCarrierKey.IsValid() && this.FeedingFoodKey.IsValid());
			}
		}

		// Token: 0x06007575 RID: 30069 RVA: 0x0044A44F File Offset: 0x0044864F
		public PeriAdvanceMonthActivePreparationModification(Character character)
		{
			this.Character = character;
			this.PoisonsToUse = null;
			this.EquipmentSlotToAddPoison = -1;
			this.FeedingCarrierKey = ItemKey.Invalid;
			this.FeedingFoodKey = ItemKey.Invalid;
		}

		// Token: 0x04001F6F RID: 8047
		public readonly Character Character;

		// Token: 0x04001F70 RID: 8048
		public bool ResourcesChanged;

		// Token: 0x04001F71 RID: 8049
		public bool PersonalNeedChanged;

		// Token: 0x04001F72 RID: 8050
		public List<ItemBase> ItemsFixed;

		// Token: 0x04001F73 RID: 8051
		public HashSet<int> CraftToolsUsed;

		// Token: 0x04001F74 RID: 8052
		public ItemKey[] PoisonsToUse;

		// Token: 0x04001F75 RID: 8053
		public sbyte EquipmentSlotToAddPoison;

		// Token: 0x04001F76 RID: 8054
		public ItemKey FeedingCarrierKey;

		// Token: 0x04001F77 RID: 8055
		public ItemKey FeedingFoodKey;
	}
}
