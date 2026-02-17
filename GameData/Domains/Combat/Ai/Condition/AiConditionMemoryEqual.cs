using System;
using System.Collections.Generic;
using GameData.Combat.Math;

namespace GameData.Domains.Combat.Ai.Condition
{
	// Token: 0x0200077A RID: 1914
	[AiCondition(EAiConditionType.MemoryEqual)]
	public class AiConditionMemoryEqual : AiConditionCommonBase
	{
		// Token: 0x060069A4 RID: 27044 RVA: 0x003BA6C3 File Offset: 0x003B88C3
		public AiConditionMemoryEqual(IReadOnlyList<string> strings)
		{
			this._key = strings[0];
			this._valueExpression = CExpression.FromBase64(strings[1]);
		}

		// Token: 0x060069A5 RID: 27045 RVA: 0x003BA6EC File Offset: 0x003B88EC
		public override bool Check(AiMemoryNew memory, IAiParticipant participant)
		{
			int value;
			return memory.Ints.TryGetValue(this._key, out value) && value == this._valueExpression.Calc(participant as IExpressionConverter);
		}

		// Token: 0x04001D1C RID: 7452
		private readonly string _key;

		// Token: 0x04001D1D RID: 7453
		private readonly CExpression _valueExpression;
	}
}
