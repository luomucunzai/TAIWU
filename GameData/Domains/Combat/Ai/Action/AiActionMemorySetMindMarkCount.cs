using System;
using System.Collections.Generic;

namespace GameData.Domains.Combat.Ai.Action
{
	// Token: 0x020007D5 RID: 2005
	[AiAction(EAiActionType.MemorySetMindMarkCount)]
	public class AiActionMemorySetMindMarkCount : AiActionMemorySetMarkCountBase
	{
		// Token: 0x06006A74 RID: 27252 RVA: 0x003BC9DB File Offset: 0x003BABDB
		public AiActionMemorySetMindMarkCount(IReadOnlyList<string> strings, IReadOnlyList<int> ints) : base(strings, ints)
		{
		}

		// Token: 0x06006A75 RID: 27253 RVA: 0x003BC9E7 File Offset: 0x003BABE7
		protected override int GetMarkCount(DefeatMarkCollection marks)
		{
			return marks.MindMarkList.Count;
		}
	}
}
