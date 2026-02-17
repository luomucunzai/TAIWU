using System;
using System.Collections.Generic;

namespace GameData.Domains.Combat.Ai.Action
{
	// Token: 0x020007D2 RID: 2002
	[AiAction(EAiActionType.MemorySetFlawMarkCount)]
	public class AiActionMemorySetFlawMarkCount : AiActionMemorySetMarkCountBase
	{
		// Token: 0x06006A6E RID: 27246 RVA: 0x003BC99A File Offset: 0x003BAB9A
		public AiActionMemorySetFlawMarkCount(IReadOnlyList<string> strings, IReadOnlyList<int> ints) : base(strings, ints)
		{
		}

		// Token: 0x06006A6F RID: 27247 RVA: 0x003BC9A6 File Offset: 0x003BABA6
		protected override int GetMarkCount(DefeatMarkCollection marks)
		{
			return marks.GetTotalFlawCount();
		}
	}
}
