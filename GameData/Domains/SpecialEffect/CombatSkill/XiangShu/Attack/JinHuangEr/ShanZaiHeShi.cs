using System;
using GameData.Domains.CombatSkill;

namespace GameData.Domains.SpecialEffect.CombatSkill.XiangShu.Attack.JinHuangEr
{
	// Token: 0x02000310 RID: 784
	public class ShanZaiHeShi : RepeatNormalAttack
	{
		// Token: 0x060033F5 RID: 13301 RVA: 0x00227242 File Offset: 0x00225442
		public ShanZaiHeShi()
		{
		}

		// Token: 0x060033F6 RID: 13302 RVA: 0x0022724C File Offset: 0x0022544C
		public ShanZaiHeShi(CombatSkillKey skillKey) : base(skillKey, 17033)
		{
			this.RepeatTimes = 3;
		}
	}
}
