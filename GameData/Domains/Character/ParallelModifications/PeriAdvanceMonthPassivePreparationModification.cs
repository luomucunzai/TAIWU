using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using GameData.Domains.Item;
using GameData.Domains.Map;

namespace GameData.Domains.Character.ParallelModifications
{
	// Token: 0x0200082E RID: 2094
	public class PeriAdvanceMonthPassivePreparationModification
	{
		// Token: 0x0600757D RID: 30077 RVA: 0x0044A559 File Offset: 0x00448759
		public PeriAdvanceMonthPassivePreparationModification(Character character)
		{
			this.Character = character;
		}

		// Token: 0x04001F86 RID: 8070
		public Character Character;

		// Token: 0x04001F87 RID: 8071
		[TupleElementNames(new string[]
		{
			"block",
			"itemKey",
			"amount"
		})]
		public List<ValueTuple<MapBlockData, ItemKey, int>> ItemsToBeLost;

		// Token: 0x04001F88 RID: 8072
		public bool PersonalNeedsChanged;
	}
}
