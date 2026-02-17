using System;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Assist;

namespace GameData.Domains.SpecialEffect.CombatSkill.Shaolinpai.DefenseAndAssist
{
	// Token: 0x02000438 RID: 1080
	public class XiaoLuoHanGunZhen : BuffTeammateCommand
	{
		// Token: 0x060039E3 RID: 14819 RVA: 0x00240EED File Offset: 0x0023F0ED
		public XiaoLuoHanGunZhen()
		{
		}

		// Token: 0x060039E4 RID: 14820 RVA: 0x00240EF7 File Offset: 0x0023F0F7
		public XiaoLuoHanGunZhen(CombatSkillKey skillKey) : base(skillKey, 1605)
		{
			this.AffectTeammateCommandType = new ETeammateCommandImplement[]
			{
				ETeammateCommandImplement.Defend
			};
			this.CommandPowerUpPercent = 50;
		}
	}
}
