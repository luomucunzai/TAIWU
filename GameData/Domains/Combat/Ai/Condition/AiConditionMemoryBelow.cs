using System;
using System.Collections.Generic;
using GameData.Combat.Math;

namespace GameData.Domains.Combat.Ai.Condition
{
	// Token: 0x0200077F RID: 1919
	[AiCondition(EAiConditionType.MemoryBelow)]
	public class AiConditionMemoryBelow : AiConditionCommonBase
	{
		// Token: 0x060069AE RID: 27054 RVA: 0x003BA8BA File Offset: 0x003B8ABA
		public AiConditionMemoryBelow(IReadOnlyList<string> strings)
		{
			this._key = strings[0];
			this._valueExpression = CExpression.FromBase64(strings[1]);
		}

		// Token: 0x060069AF RID: 27055 RVA: 0x003BA8E4 File Offset: 0x003B8AE4
		public override bool Check(AiMemoryNew memory, IAiParticipant participant)
		{
			int value;
			return memory.Ints.TryGetValue(this._key, out value) && value < this._valueExpression.Calc(participant as IExpressionConverter);
		}

		// Token: 0x04001D26 RID: 7462
		private readonly string _key;

		// Token: 0x04001D27 RID: 7463
		private readonly CExpression _valueExpression;
	}
}
