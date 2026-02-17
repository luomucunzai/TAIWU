using System;
using System.Collections.Generic;
using GameData.Combat.Math;

namespace GameData.Domains.Combat.Ai.Condition
{
	// Token: 0x0200075A RID: 1882
	[AiCondition(EAiConditionType.BreathPercentMoreOrEqual)]
	public class AiConditionBreathPercentMoreOrEqual : AiConditionCheckCharExpressionBase
	{
		// Token: 0x06006963 RID: 26979 RVA: 0x003B9F29 File Offset: 0x003B8129
		public AiConditionBreathPercentMoreOrEqual(IReadOnlyList<string> strings, IReadOnlyList<int> ints) : base(strings, ints)
		{
		}

		// Token: 0x06006964 RID: 26980 RVA: 0x003B9F38 File Offset: 0x003B8138
		protected override bool Check(CombatCharacter checkChar, int expressionResult)
		{
			int percent = CValuePercent.ParseInt(checkChar.GetBreathValue(), 30000);
			return percent >= expressionResult;
		}
	}
}
