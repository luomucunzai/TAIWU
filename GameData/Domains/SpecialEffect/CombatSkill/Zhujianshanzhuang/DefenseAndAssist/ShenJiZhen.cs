using System;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Assist;

namespace GameData.Domains.SpecialEffect.CombatSkill.Zhujianshanzhuang.DefenseAndAssist
{
	// Token: 0x020001D6 RID: 470
	public class ShenJiZhen : BuffTeammateCommand
	{
		// Token: 0x06002D56 RID: 11606 RVA: 0x0020B522 File Offset: 0x00209722
		public ShenJiZhen()
		{
		}

		// Token: 0x06002D57 RID: 11607 RVA: 0x0020B52C File Offset: 0x0020972C
		public ShenJiZhen(CombatSkillKey skillKey) : base(skillKey, 9703)
		{
			this.AffectTeammateCommandType = new ETeammateCommandImplement[]
			{
				ETeammateCommandImplement.AddHit,
				ETeammateCommandImplement.AddAvoid
			};
			this.CommandPowerUpPercent = 50;
		}
	}
}
