using System;
using System.Collections.Generic;

namespace GameData.Domains.Combat.Ai.Condition
{
	// Token: 0x02000778 RID: 1912
	[AiCondition(EAiConditionType.MemoryEqualString)]
	public class AiConditionMemoryEqualString : AiConditionCommonBase
	{
		// Token: 0x060069A0 RID: 27040 RVA: 0x003BA60B File Offset: 0x003B880B
		public AiConditionMemoryEqualString(IReadOnlyList<string> strings, IReadOnlyList<int> ints)
		{
			this._key = strings[0];
			this._value = strings[1];
		}

		// Token: 0x060069A1 RID: 27041 RVA: 0x003BA630 File Offset: 0x003B8830
		public override bool Check(AiMemoryNew memory, IAiParticipant participant)
		{
			string value;
			return memory.Strings.TryGetValue(this._key, out value) && value == this._value;
		}

		// Token: 0x04001D18 RID: 7448
		private readonly string _key;

		// Token: 0x04001D19 RID: 7449
		private readonly string _value;
	}
}
