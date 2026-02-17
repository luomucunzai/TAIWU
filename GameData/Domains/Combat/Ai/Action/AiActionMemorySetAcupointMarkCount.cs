using System;
using System.Collections.Generic;

namespace GameData.Domains.Combat.Ai.Action
{
	// Token: 0x020007D3 RID: 2003
	[AiAction(EAiActionType.MemorySetAcupointMarkCount)]
	public class AiActionMemorySetAcupointMarkCount : AiActionMemorySetMarkCountBase
	{
		// Token: 0x06006A70 RID: 27248 RVA: 0x003BC9AE File Offset: 0x003BABAE
		public AiActionMemorySetAcupointMarkCount(IReadOnlyList<string> strings, IReadOnlyList<int> ints) : base(strings, ints)
		{
		}

		// Token: 0x06006A71 RID: 27249 RVA: 0x003BC9BA File Offset: 0x003BABBA
		protected override int GetMarkCount(DefeatMarkCollection marks)
		{
			return marks.GetTotalAcupointCount();
		}
	}
}
