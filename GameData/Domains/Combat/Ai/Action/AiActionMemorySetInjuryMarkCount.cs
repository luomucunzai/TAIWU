using System;
using System.Collections.Generic;

namespace GameData.Domains.Combat.Ai.Action
{
	// Token: 0x020007D1 RID: 2001
	[AiAction(EAiActionType.MemorySetInjuryMarkCount)]
	public class AiActionMemorySetInjuryMarkCount : AiActionMemorySetMarkCountBase
	{
		// Token: 0x06006A6C RID: 27244 RVA: 0x003BC986 File Offset: 0x003BAB86
		public AiActionMemorySetInjuryMarkCount(IReadOnlyList<string> strings, IReadOnlyList<int> ints) : base(strings, ints)
		{
		}

		// Token: 0x06006A6D RID: 27245 RVA: 0x003BC992 File Offset: 0x003BAB92
		protected override int GetMarkCount(DefeatMarkCollection marks)
		{
			return marks.GetTotalInjuryCount();
		}
	}
}
