using System;
using System.Collections.Generic;
using GameData.Combat.Math;

namespace GameData.Domains.Combat.Ai.Condition
{
	// Token: 0x0200075B RID: 1883
	[AiCondition(EAiConditionType.StancePercentMoreOrEqual)]
	public class AiConditionStancePercentMoreOrEqual : AiConditionCheckCharExpressionBase
	{
		// Token: 0x06006965 RID: 26981 RVA: 0x003B9F62 File Offset: 0x003B8162
		public AiConditionStancePercentMoreOrEqual(IReadOnlyList<string> strings, IReadOnlyList<int> ints) : base(strings, ints)
		{
		}

		// Token: 0x06006966 RID: 26982 RVA: 0x003B9F70 File Offset: 0x003B8170
		protected override bool Check(CombatCharacter checkChar, int expressionResult)
		{
			int percent = CValuePercent.ParseInt(checkChar.GetStanceValue(), 4000);
			return percent >= expressionResult;
		}
	}
}
