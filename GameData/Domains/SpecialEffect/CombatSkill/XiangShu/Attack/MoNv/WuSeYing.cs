using System;
using GameData.Domains.CombatSkill;

namespace GameData.Domains.SpecialEffect.CombatSkill.XiangShu.Attack.MoNv
{
	// Token: 0x020002F6 RID: 758
	public class WuSeYing : AutoAttackAndAddPower
	{
		// Token: 0x06003387 RID: 13191 RVA: 0x002255CE File Offset: 0x002237CE
		public WuSeYing()
		{
		}

		// Token: 0x06003388 RID: 13192 RVA: 0x002255D8 File Offset: 0x002237D8
		public WuSeYing(CombatSkillKey skillKey) : base(skillKey, 17002)
		{
			this.AttackRepeatTimes = 1;
			this.AddPowerUnit = 10;
		}
	}
}
