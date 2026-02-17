using System;
using System.Collections.Generic;

namespace GameData.Domains.Combat.Ai.Action
{
	// Token: 0x020007C9 RID: 1993
	[AiAction(EAiActionType.MemorySetString)]
	public class AiActionMemorySetString : AiActionCommonBase
	{
		// Token: 0x06006A5C RID: 27228 RVA: 0x003BC808 File Offset: 0x003BAA08
		public AiActionMemorySetString(IReadOnlyList<string> strings, IReadOnlyList<int> ints)
		{
			this._key = strings[0];
			this._value = strings[1];
		}

		// Token: 0x06006A5D RID: 27229 RVA: 0x003BC82C File Offset: 0x003BAA2C
		public override void Execute(AiMemoryNew memory, IAiParticipant participant)
		{
			memory.Strings[this._key] = this._value;
		}

		// Token: 0x04001D5D RID: 7517
		private readonly string _key;

		// Token: 0x04001D5E RID: 7518
		private readonly string _value;
	}
}
