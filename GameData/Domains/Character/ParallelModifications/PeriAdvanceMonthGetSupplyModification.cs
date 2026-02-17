using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace GameData.Domains.Character.ParallelModifications
{
	// Token: 0x0200082D RID: 2093
	public class PeriAdvanceMonthGetSupplyModification
	{
		// Token: 0x0600757A RID: 30074 RVA: 0x0044A500 File Offset: 0x00448700
		public PeriAdvanceMonthGetSupplyModification(Character character)
		{
			this.Character = character;
		}

		// Token: 0x1700048C RID: 1164
		// (get) Token: 0x0600757B RID: 30075 RVA: 0x0044A511 File Offset: 0x00448711
		public bool IsChanged
		{
			get
			{
				return this.PersonalNeedChanged || this.ResourceChanged || this.ItemsToCreate != null;
			}
		}

		// Token: 0x0600757C RID: 30076 RVA: 0x0044A52F File Offset: 0x0044872F
		public void AddItemToCreate(sbyte type, short templateId, int amount)
		{
			if (this.ItemsToCreate == null)
			{
				this.ItemsToCreate = new List<ValueTuple<sbyte, short, int>>();
			}
			this.ItemsToCreate.Add(new ValueTuple<sbyte, short, int>(type, templateId, amount));
		}

		// Token: 0x04001F82 RID: 8066
		public Character Character;

		// Token: 0x04001F83 RID: 8067
		public bool PersonalNeedChanged;

		// Token: 0x04001F84 RID: 8068
		public bool ResourceChanged;

		// Token: 0x04001F85 RID: 8069
		[TupleElementNames(new string[]
		{
			"type",
			"templateId",
			"amount"
		})]
		public List<ValueTuple<sbyte, short, int>> ItemsToCreate;
	}
}
