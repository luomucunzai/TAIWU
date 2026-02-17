using System;
using System.Collections.Generic;

namespace GameData.Domains.Combat.Ai.Action
{
	// Token: 0x020007CF RID: 1999
	[AiAction(EAiActionType.MemoryInternalSet)]
	public class AiActionMemoryInternalSet : AiActionMemoryInternalSetBase<int>
	{
		// Token: 0x06006A68 RID: 27240 RVA: 0x003BC95F File Offset: 0x003BAB5F
		public AiActionMemoryInternalSet(IReadOnlyList<string> strings) : base(strings)
		{
		}

		// Token: 0x06006A69 RID: 27241 RVA: 0x003BC96A File Offset: 0x003BAB6A
		protected override IDictionary<string, int> GetMemoryDict(AiMemoryNew memory)
		{
			return memory.Ints;
		}
	}
}
