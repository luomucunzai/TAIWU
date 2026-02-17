using System;
using System.Collections.Generic;

namespace GameData.Domains.Combat.Ai.Condition
{
	// Token: 0x0200073D RID: 1853
	[AiCondition(EAiConditionType.CastingProgressLess)]
	public class AiConditionCastingProgressLess : AiConditionCheckCharExpressionBase
	{
		// Token: 0x06006928 RID: 26920 RVA: 0x003B9773 File Offset: 0x003B7973
		public AiConditionCastingProgressLess(IReadOnlyList<string> strings, IReadOnlyList<int> ints) : base(strings, ints)
		{
		}

		// Token: 0x06006929 RID: 26921 RVA: 0x003B9780 File Offset: 0x003B7980
		protected override bool Check(CombatCharacter checkChar, int expressionResult)
		{
			bool flag = checkChar.GetPreparingSkillId() < 0;
			return !flag && (int)checkChar.GetSkillPreparePercent() < expressionResult;
		}
	}
}
