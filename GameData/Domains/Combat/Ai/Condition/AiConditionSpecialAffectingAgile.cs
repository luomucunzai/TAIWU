using System;
using System.Collections.Generic;

namespace GameData.Domains.Combat.Ai.Condition
{
	// Token: 0x02000741 RID: 1857
	[AiCondition(EAiConditionType.SpecialAffectingAgile)]
	public class AiConditionSpecialAffectingAgile : AiConditionSpecialAffecting
	{
		// Token: 0x06006931 RID: 26929 RVA: 0x003B98C7 File Offset: 0x003B7AC7
		public AiConditionSpecialAffectingAgile(IReadOnlyList<int> ints) : base(ints)
		{
		}

		// Token: 0x06006932 RID: 26930 RVA: 0x003B98D2 File Offset: 0x003B7AD2
		protected override short GetAffectingSkillId(CombatCharacter combatChar)
		{
			return combatChar.GetAffectingMoveSkillId();
		}
	}
}
