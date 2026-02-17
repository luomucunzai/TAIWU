using System;
using System.Collections.Generic;

namespace GameData.Domains.Combat.Ai.Condition
{
	// Token: 0x02000742 RID: 1858
	[AiCondition(EAiConditionType.AnyAffectingDefense)]
	public class AiConditionAnyAffectingDefense : AiConditionAnyAffecting
	{
		// Token: 0x06006933 RID: 26931 RVA: 0x003B98DA File Offset: 0x003B7ADA
		public AiConditionAnyAffectingDefense(IReadOnlyList<int> ints) : base(ints)
		{
		}

		// Token: 0x06006934 RID: 26932 RVA: 0x003B98E5 File Offset: 0x003B7AE5
		protected override short GetAffectingSkillId(CombatCharacter combatChar)
		{
			return combatChar.GetAffectingDefendSkillId();
		}
	}
}
