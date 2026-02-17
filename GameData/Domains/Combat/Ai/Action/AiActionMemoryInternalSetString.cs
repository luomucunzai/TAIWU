using System;
using System.Collections.Generic;

namespace GameData.Domains.Combat.Ai.Action
{
	// Token: 0x020007CD RID: 1997
	[AiAction(EAiActionType.MemoryInternalSetString)]
	public class AiActionMemoryInternalSetString : AiActionMemoryInternalSetBase<string>
	{
		// Token: 0x06006A64 RID: 27236 RVA: 0x003BC939 File Offset: 0x003BAB39
		public AiActionMemoryInternalSetString(IReadOnlyList<string> strings) : base(strings)
		{
		}

		// Token: 0x06006A65 RID: 27237 RVA: 0x003BC944 File Offset: 0x003BAB44
		protected override IDictionary<string, string> GetMemoryDict(AiMemoryNew memory)
		{
			return memory.Strings;
		}
	}
}
