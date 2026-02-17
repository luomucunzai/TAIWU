using System;
using System.Collections.Generic;

namespace GameData.Domains.Combat.Ai.Condition
{
	// Token: 0x0200073C RID: 1852
	[AiCondition(EAiConditionType.CastingProgressMoreOrEqual)]
	public class AiConditionCastingProgressMoreOrEqual : AiConditionCheckCharExpressionBase
	{
		// Token: 0x06006926 RID: 26918 RVA: 0x003B9737 File Offset: 0x003B7937
		public AiConditionCastingProgressMoreOrEqual(IReadOnlyList<string> strings, IReadOnlyList<int> ints) : base(strings, ints)
		{
		}

		// Token: 0x06006927 RID: 26919 RVA: 0x003B9744 File Offset: 0x003B7944
		protected override bool Check(CombatCharacter checkChar, int expressionResult)
		{
			bool flag = checkChar.GetPreparingSkillId() < 0;
			return !flag && (int)checkChar.GetSkillPreparePercent() >= expressionResult;
		}
	}
}
