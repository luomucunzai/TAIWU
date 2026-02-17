using System;
using System.Collections.Generic;

namespace GameData.Domains.Combat.Ai.Condition
{
	// Token: 0x0200075F RID: 1887
	[AiCondition(EAiConditionType.BlockPercentLess)]
	public class AiConditionBlockPercentLess : AiConditionCheckCharExpressionBase
	{
		// Token: 0x0600696D RID: 26989 RVA: 0x003BA09A File Offset: 0x003B829A
		public AiConditionBlockPercentLess(IReadOnlyList<string> strings, IReadOnlyList<int> ints) : base(strings, ints)
		{
		}

		// Token: 0x0600696E RID: 26990 RVA: 0x003BA0A8 File Offset: 0x003B82A8
		protected override bool Check(CombatCharacter checkChar, int expressionResult)
		{
			return false;
		}
	}
}
