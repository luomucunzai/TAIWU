using System;
using System.Collections.Generic;
using GameData.Domains.CombatSkill;

namespace GameData.Domains.Character.ParallelModifications
{
	// Token: 0x02000832 RID: 2098
	public class PracticeAndBreakoutModification
	{
		// Token: 0x06007582 RID: 30082 RVA: 0x0044A61A File Offset: 0x0044881A
		public PracticeAndBreakoutModification(Character character)
		{
			this.Character = character;
		}

		// Token: 0x04001FB9 RID: 8121
		public Character Character;

		// Token: 0x04001FBA RID: 8122
		public List<CombatSkillInitialBreakoutData> BrokenOutCombatSkills;

		// Token: 0x04001FBB RID: 8123
		public List<CombatSkill> FailedToBreakoutCombatSkills;

		// Token: 0x04001FBC RID: 8124
		public int NewExp;

		// Token: 0x04001FBD RID: 8125
		public Injuries NewInjuries;

		// Token: 0x04001FBE RID: 8126
		public short NewDisorderOfQi;

		// Token: 0x04001FBF RID: 8127
		public bool PersonalNeedsChanged;
	}
}
