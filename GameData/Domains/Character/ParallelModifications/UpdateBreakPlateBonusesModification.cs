using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using GameData.Domains.Item;
using GameData.Domains.Taiwu;
using GameData.Utilities;

namespace GameData.Domains.Character.ParallelModifications
{
	// Token: 0x02000836 RID: 2102
	public class UpdateBreakPlateBonusesModification
	{
		// Token: 0x06007586 RID: 30086 RVA: 0x0044A68E File Offset: 0x0044888E
		public UpdateBreakPlateBonusesModification(Character character)
		{
			this.Character = character;
		}

		// Token: 0x04001FD6 RID: 8150
		public Character Character;

		// Token: 0x04001FD7 RID: 8151
		[TupleElementNames(new string[]
		{
			"skillTemplateId",
			"startIndex",
			"bonuses"
		})]
		public List<ValueTuple<short, int, SerializableList<SkillBreakPlateBonus>>> ModifiedBonuses;

		// Token: 0x04001FD8 RID: 8152
		public bool PersonalNeedsUpdated;

		// Token: 0x04001FD9 RID: 8153
		public List<ItemKey> ToDeleteItems;

		// Token: 0x04001FDA RID: 8154
		public int ExpCost;
	}
}
