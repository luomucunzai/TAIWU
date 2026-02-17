using System;
using System.Collections.Generic;

namespace GameData.Domains.Combat.Ai.Action
{
	// Token: 0x020007CE RID: 1998
	[AiAction(EAiActionType.MemoryInternalSetBoolean)]
	public class AiActionMemoryInternalSetBoolean : AiActionMemoryInternalSetBase<bool>
	{
		// Token: 0x06006A66 RID: 27238 RVA: 0x003BC94C File Offset: 0x003BAB4C
		public AiActionMemoryInternalSetBoolean(IReadOnlyList<string> strings) : base(strings)
		{
		}

		// Token: 0x06006A67 RID: 27239 RVA: 0x003BC957 File Offset: 0x003BAB57
		protected override IDictionary<string, bool> GetMemoryDict(AiMemoryNew memory)
		{
			return memory.Booleans;
		}
	}
}
