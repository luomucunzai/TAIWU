using System;
using System.Collections.Generic;

namespace GameData.Domains.Combat.Ai.Action
{
	// Token: 0x020007D0 RID: 2000
	[AiAction(EAiActionType.MemorySetAllMarkCount)]
	public class AiActionMemorySetAllMarkCount : AiActionMemorySetMarkCountBase
	{
		// Token: 0x06006A6A RID: 27242 RVA: 0x003BC972 File Offset: 0x003BAB72
		public AiActionMemorySetAllMarkCount(IReadOnlyList<string> strings, IReadOnlyList<int> ints) : base(strings, ints)
		{
		}

		// Token: 0x06006A6B RID: 27243 RVA: 0x003BC97E File Offset: 0x003BAB7E
		protected override int GetMarkCount(DefeatMarkCollection marks)
		{
			return marks.GetTotalCount();
		}
	}
}
