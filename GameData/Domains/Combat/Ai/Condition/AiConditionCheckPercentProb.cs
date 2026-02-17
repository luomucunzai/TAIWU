using System;
using System.Collections.Generic;
using GameData.Combat.Math;
using GameData.Utilities;
using Redzen.Random;

namespace GameData.Domains.Combat.Ai.Condition
{
	// Token: 0x0200074B RID: 1867
	[AiCondition(EAiConditionType.CheckPercentProb)]
	public class AiConditionCheckPercentProb : AiConditionCommonBase
	{
		// Token: 0x06006945 RID: 26949 RVA: 0x003B9ABD File Offset: 0x003B7CBD
		public AiConditionCheckPercentProb(IReadOnlyList<string> strings)
		{
			this._expression = CExpression.FromBase64(strings[0]);
			this._randomSource = RandomDefaults.CreateRandomSource();
		}

		// Token: 0x06006946 RID: 26950 RVA: 0x003B9AE4 File Offset: 0x003B7CE4
		public override bool Check(AiMemoryNew memory, IAiParticipant participant)
		{
			int prob = this._expression.Calc(participant as IExpressionConverter);
			return this._randomSource.CheckPercentProb(prob);
		}

		// Token: 0x04001CFB RID: 7419
		private readonly CExpression _expression;

		// Token: 0x04001CFC RID: 7420
		private readonly IRandomSource _randomSource;
	}
}
