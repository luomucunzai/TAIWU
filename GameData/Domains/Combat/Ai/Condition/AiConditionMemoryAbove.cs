using System;
using System.Collections.Generic;
using GameData.Combat.Math;

namespace GameData.Domains.Combat.Ai.Condition
{
	// Token: 0x0200077E RID: 1918
	[AiCondition(EAiConditionType.MemoryAbove)]
	public class AiConditionMemoryAbove : AiConditionCommonBase
	{
		// Token: 0x060069AC RID: 27052 RVA: 0x003BA851 File Offset: 0x003B8A51
		public AiConditionMemoryAbove(IReadOnlyList<string> strings)
		{
			this._key = strings[0];
			this._valueExpression = CExpression.FromBase64(strings[1]);
		}

		// Token: 0x060069AD RID: 27053 RVA: 0x003BA87C File Offset: 0x003B8A7C
		public override bool Check(AiMemoryNew memory, IAiParticipant participant)
		{
			int value;
			return memory.Ints.TryGetValue(this._key, out value) && value > this._valueExpression.Calc(participant as IExpressionConverter);
		}

		// Token: 0x04001D24 RID: 7460
		private readonly string _key;

		// Token: 0x04001D25 RID: 7461
		private readonly CExpression _valueExpression;
	}
}
