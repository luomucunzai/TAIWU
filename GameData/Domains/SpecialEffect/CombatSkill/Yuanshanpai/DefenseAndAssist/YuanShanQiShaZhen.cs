using System;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Assist;

namespace GameData.Domains.SpecialEffect.CombatSkill.Yuanshanpai.DefenseAndAssist
{
	// Token: 0x0200020B RID: 523
	public class YuanShanQiShaZhen : BuffTeammateCommand
	{
		// Token: 0x06002EDA RID: 11994 RVA: 0x00210F77 File Offset: 0x0020F177
		public YuanShanQiShaZhen()
		{
		}

		// Token: 0x06002EDB RID: 11995 RVA: 0x00210F81 File Offset: 0x0020F181
		public YuanShanQiShaZhen(CombatSkillKey skillKey) : base(skillKey, 5602)
		{
			this.AffectTeammateCommandType = new ETeammateCommandImplement[]
			{
				ETeammateCommandImplement.Attack
			};
			this.CommandPowerUpPercent = 40;
		}
	}
}
