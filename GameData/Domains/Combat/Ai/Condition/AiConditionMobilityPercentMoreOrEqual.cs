using System;
using System.Collections.Generic;
using GameData.Combat.Math;

namespace GameData.Domains.Combat.Ai.Condition
{
	// Token: 0x02000786 RID: 1926
	[AiCondition(EAiConditionType.MobilityPercentMoreOrEqual)]
	public class AiConditionMobilityPercentMoreOrEqual : AiConditionCheckCharExpressionBase
	{
		// Token: 0x060069BC RID: 27068 RVA: 0x003BAB2D File Offset: 0x003B8D2D
		public AiConditionMobilityPercentMoreOrEqual(IReadOnlyList<string> strings, IReadOnlyList<int> ints) : base(strings, ints)
		{
		}

		// Token: 0x060069BD RID: 27069 RVA: 0x003BAB3C File Offset: 0x003B8D3C
		protected override bool Check(CombatCharacter checkChar, int expressionResult)
		{
			int percent = CValuePercent.ParseInt(checkChar.GetMobilityValue(), MoveSpecialConstants.MaxMobility);
			return percent >= expressionResult;
		}
	}
}
