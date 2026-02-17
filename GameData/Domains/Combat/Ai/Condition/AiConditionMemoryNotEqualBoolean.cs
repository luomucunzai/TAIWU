using System;
using System.Collections.Generic;

namespace GameData.Domains.Combat.Ai.Condition
{
	// Token: 0x0200077C RID: 1916
	[AiCondition(EAiConditionType.MemoryNotEqualBoolean)]
	public class AiConditionMemoryNotEqualBoolean : AiConditionCommonBase
	{
		// Token: 0x060069A8 RID: 27048 RVA: 0x003BA786 File Offset: 0x003B8986
		public AiConditionMemoryNotEqualBoolean(IReadOnlyList<string> strings, IReadOnlyList<int> ints)
		{
			this._key = strings[0];
			this._value = (ints[0] == 1);
		}

		// Token: 0x060069A9 RID: 27049 RVA: 0x003BA7B0 File Offset: 0x003B89B0
		public override bool Check(AiMemoryNew memory, IAiParticipant participant)
		{
			bool value;
			return !memory.Booleans.TryGetValue(this._key, out value) || value != this._value;
		}

		// Token: 0x04001D20 RID: 7456
		private readonly string _key;

		// Token: 0x04001D21 RID: 7457
		private readonly bool _value;
	}
}
