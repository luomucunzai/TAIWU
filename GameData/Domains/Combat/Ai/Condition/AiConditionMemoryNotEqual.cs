using System;
using System.Collections.Generic;
using GameData.Combat.Math;

namespace GameData.Domains.Combat.Ai.Condition
{
	// Token: 0x0200077D RID: 1917
	[AiCondition(EAiConditionType.MemoryNotEqual)]
	public class AiConditionMemoryNotEqual : AiConditionCommonBase
	{
		// Token: 0x060069AA RID: 27050 RVA: 0x003BA7E6 File Offset: 0x003B89E6
		public AiConditionMemoryNotEqual(IReadOnlyList<string> strings)
		{
			this._key = strings[0];
			this._valueExpression = CExpression.FromBase64(strings[1]);
		}

		// Token: 0x060069AB RID: 27051 RVA: 0x003BA810 File Offset: 0x003B8A10
		public override bool Check(AiMemoryNew memory, IAiParticipant participant)
		{
			int value;
			return !memory.Ints.TryGetValue(this._key, out value) || value != this._valueExpression.Calc(participant as IExpressionConverter);
		}

		// Token: 0x04001D22 RID: 7458
		private readonly string _key;

		// Token: 0x04001D23 RID: 7459
		private readonly CExpression _valueExpression;
	}
}
