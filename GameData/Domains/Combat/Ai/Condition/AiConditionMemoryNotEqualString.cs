using System;
using System.Collections.Generic;

namespace GameData.Domains.Combat.Ai.Condition
{
	// Token: 0x0200077B RID: 1915
	[AiCondition(EAiConditionType.MemoryNotEqualString)]
	public class AiConditionMemoryNotEqualString : AiConditionCommonBase
	{
		// Token: 0x060069A6 RID: 27046 RVA: 0x003BA72A File Offset: 0x003B892A
		public AiConditionMemoryNotEqualString(IReadOnlyList<string> strings)
		{
			this._key = strings[0];
			this._value = strings[1];
		}

		// Token: 0x060069A7 RID: 27047 RVA: 0x003BA750 File Offset: 0x003B8950
		public override bool Check(AiMemoryNew memory, IAiParticipant participant)
		{
			string value;
			return !memory.Strings.TryGetValue(this._key, out value) || value != this._value;
		}

		// Token: 0x04001D1E RID: 7454
		private readonly string _key;

		// Token: 0x04001D1F RID: 7455
		private readonly string _value;
	}
}
