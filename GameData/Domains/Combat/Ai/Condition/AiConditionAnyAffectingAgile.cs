using System;
using System.Collections.Generic;

namespace GameData.Domains.Combat.Ai.Condition
{
	// Token: 0x02000740 RID: 1856
	[AiCondition(EAiConditionType.AnyAffectingAgile)]
	public class AiConditionAnyAffectingAgile : AiConditionAnyAffecting
	{
		// Token: 0x0600692F RID: 26927 RVA: 0x003B98B4 File Offset: 0x003B7AB4
		public AiConditionAnyAffectingAgile(IReadOnlyList<int> ints) : base(ints)
		{
		}

		// Token: 0x06006930 RID: 26928 RVA: 0x003B98BF File Offset: 0x003B7ABF
		protected override short GetAffectingSkillId(CombatCharacter combatChar)
		{
			return combatChar.GetAffectingMoveSkillId();
		}
	}
}
