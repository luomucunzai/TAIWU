using System;
using System.Collections.Generic;

namespace GameData.Domains.Combat.Ai.Condition
{
	// Token: 0x02000743 RID: 1859
	[AiCondition(EAiConditionType.SpecialAffectingDefense)]
	public class AiConditionSpecialAffectingDefense : AiConditionSpecialAffecting
	{
		// Token: 0x06006935 RID: 26933 RVA: 0x003B98ED File Offset: 0x003B7AED
		public AiConditionSpecialAffectingDefense(IReadOnlyList<int> ints) : base(ints)
		{
		}

		// Token: 0x06006936 RID: 26934 RVA: 0x003B98F8 File Offset: 0x003B7AF8
		protected override short GetAffectingSkillId(CombatCharacter combatChar)
		{
			return combatChar.GetAffectingDefendSkillId();
		}
	}
}
