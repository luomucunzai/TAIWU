using System;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Assist;

namespace GameData.Domains.SpecialEffect.CombatSkill.Wudangpai.DefenseAndAssist
{
	// Token: 0x020003D7 RID: 983
	public class SanCaiJianZhen : BuffTeammateCommand
	{
		// Token: 0x060037B8 RID: 14264 RVA: 0x00236EBF File Offset: 0x002350BF
		public SanCaiJianZhen()
		{
		}

		// Token: 0x060037B9 RID: 14265 RVA: 0x00236EC9 File Offset: 0x002350C9
		public SanCaiJianZhen(CombatSkillKey skillKey) : base(skillKey, 4601)
		{
			this.AffectTeammateCommandType = new ETeammateCommandImplement[]
			{
				ETeammateCommandImplement.HealFlaw,
				ETeammateCommandImplement.HealAcupoint
			};
			this.CommandPowerUpPercent = 100;
		}
	}
}
