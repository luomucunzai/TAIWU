using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using GameData.Domains.Character.Ai.GeneralAction;

namespace GameData.Domains.Character.ParallelModifications
{
	// Token: 0x0200082C RID: 2092
	public class PeriAdvanceMonthGeneralActionModification
	{
		// Token: 0x1700048B RID: 1163
		// (get) Token: 0x06007578 RID: 30072 RVA: 0x0044A4D4 File Offset: 0x004486D4
		public bool IsChanged
		{
			get
			{
				return this.PerformedActions.Count > 0;
			}
		}

		// Token: 0x06007579 RID: 30073 RVA: 0x0044A4E4 File Offset: 0x004486E4
		public PeriAdvanceMonthGeneralActionModification(Character character)
		{
			this.Character = character;
			this.PerformedActions = new List<ValueTuple<Character, IGeneralAction>>();
		}

		// Token: 0x04001F80 RID: 8064
		public Character Character;

		// Token: 0x04001F81 RID: 8065
		[TupleElementNames(new string[]
		{
			"targetChar",
			null
		})]
		public List<ValueTuple<Character, IGeneralAction>> PerformedActions;
	}
}
