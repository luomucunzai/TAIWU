using System;
using System.Collections.Generic;

namespace GameData.Domains.Combat.Ai.Condition
{
	// Token: 0x02000779 RID: 1913
	[AiCondition(EAiConditionType.MemoryEqualBoolean)]
	public class AiConditionMemoryEqualBoolean : AiConditionCommonBase
	{
		// Token: 0x060069A2 RID: 27042 RVA: 0x003BA666 File Offset: 0x003B8866
		public AiConditionMemoryEqualBoolean(IReadOnlyList<string> strings, IReadOnlyList<int> ints)
		{
			this._key = strings[0];
			this._value = (ints[0] == 1);
		}

		// Token: 0x060069A3 RID: 27043 RVA: 0x003BA690 File Offset: 0x003B8890
		public override bool Check(AiMemoryNew memory, IAiParticipant participant)
		{
			bool value;
			return memory.Booleans.TryGetValue(this._key, out value) && value == this._value;
		}

		// Token: 0x04001D1A RID: 7450
		private readonly string _key;

		// Token: 0x04001D1B RID: 7451
		private readonly bool _value;
	}
}
