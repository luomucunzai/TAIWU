using System;
using System.Collections.Generic;

namespace GameData.Domains.Combat.Ai.Action
{
	// Token: 0x020007CA RID: 1994
	[AiAction(EAiActionType.MemorySetBoolean)]
	public class AiActionMemorySetBoolean : AiActionCommonBase
	{
		// Token: 0x06006A5E RID: 27230 RVA: 0x003BC847 File Offset: 0x003BAA47
		public AiActionMemorySetBoolean(IReadOnlyList<string> strings, IReadOnlyList<int> ints)
		{
			this._key = strings[0];
			this._value = (ints[0] == 1);
		}

		// Token: 0x06006A5F RID: 27231 RVA: 0x003BC86E File Offset: 0x003BAA6E
		public override void Execute(AiMemoryNew memory, IAiParticipant participant)
		{
			memory.Booleans[this._key] = this._value;
		}

		// Token: 0x04001D5F RID: 7519
		private readonly string _key;

		// Token: 0x04001D60 RID: 7520
		private readonly bool _value;
	}
}
