using System;
using GameData.Domains.CombatSkill;

namespace GameData.Domains.SpecialEffect.CombatSkill.XiangShu.Attack.MoNv
{
	// Token: 0x020002F4 RID: 756
	public class LuanCaiYing : AutoAttackAndAddPower
	{
		// Token: 0x0600337E RID: 13182 RVA: 0x0022537B File Offset: 0x0022357B
		public LuanCaiYing()
		{
		}

		// Token: 0x0600337F RID: 13183 RVA: 0x00225385 File Offset: 0x00223585
		public LuanCaiYing(CombatSkillKey skillKey) : base(skillKey, 17005)
		{
			this.AttackRepeatTimes = 3;
			this.AddPowerUnit = 20;
		}
	}
}
