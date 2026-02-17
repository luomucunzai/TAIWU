using System;
using System.Collections.Generic;

namespace GameData.Domains.Combat.Ai.Condition
{
	// Token: 0x02000746 RID: 1862
	[AiCondition(EAiConditionType.LearnCombatSkill)]
	public class AiConditionLearnCombatSkill : AiConditionCheckCharBase
	{
		// Token: 0x0600693B RID: 26939 RVA: 0x003B99B5 File Offset: 0x003B7BB5
		public AiConditionLearnCombatSkill(IReadOnlyList<int> ints) : base(ints)
		{
			this._skillId = (short)ints[1];
		}

		// Token: 0x0600693C RID: 26940 RVA: 0x003B99D0 File Offset: 0x003B7BD0
		protected override bool Check(CombatCharacter checkChar)
		{
			return checkChar.GetCharacter().GetLearnedCombatSkills().Contains(this._skillId);
		}

		// Token: 0x04001CF8 RID: 7416
		private readonly short _skillId;
	}
}
