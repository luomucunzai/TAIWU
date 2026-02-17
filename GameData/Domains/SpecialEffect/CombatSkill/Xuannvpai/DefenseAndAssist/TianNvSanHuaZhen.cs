using System;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Assist;

namespace GameData.Domains.SpecialEffect.CombatSkill.Xuannvpai.DefenseAndAssist
{
	// Token: 0x02000285 RID: 645
	public class TianNvSanHuaZhen : BuffTeammateCommand
	{
		// Token: 0x06003104 RID: 12548 RVA: 0x002196EF File Offset: 0x002178EF
		public TianNvSanHuaZhen()
		{
		}

		// Token: 0x06003105 RID: 12549 RVA: 0x002196F9 File Offset: 0x002178F9
		public TianNvSanHuaZhen(CombatSkillKey skillKey) : base(skillKey, 8603)
		{
			this.AffectTeammateCommandType = new ETeammateCommandImplement[]
			{
				ETeammateCommandImplement.Push,
				ETeammateCommandImplement.Pull
			};
			this.CommandPowerUpPercent = 100;
		}
	}
}
