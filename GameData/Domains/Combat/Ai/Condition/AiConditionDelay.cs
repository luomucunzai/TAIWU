using System;
using System.Collections.Generic;
using GameData.Combat.Math;
using GameData.Utilities;

namespace GameData.Domains.Combat.Ai.Condition
{
	// Token: 0x0200075C RID: 1884
	[AiCondition(EAiConditionType.Delay)]
	public class AiConditionDelay : AiConditionCommonBase
	{
		// Token: 0x06006967 RID: 26983 RVA: 0x003B9F9A File Offset: 0x003B819A
		public AiConditionDelay(IReadOnlyList<string> strings)
		{
			this._delayExpression = CExpression.FromBase64(strings[0]);
		}

		// Token: 0x06006968 RID: 26984 RVA: 0x003B9FB8 File Offset: 0x003B81B8
		public override bool Check(AiMemoryNew memory, IAiParticipant participant)
		{
			int delay = this._delayExpression.Calc(participant as IExpressionConverter);
			int times = memory.Ints.GetOrDefault(base.RuntimeIdStr) + 1;
			memory.Ints[base.RuntimeIdStr] = times;
			return times % delay == 0;
		}

		// Token: 0x04001D08 RID: 7432
		private readonly CExpression _delayExpression;
	}
}
