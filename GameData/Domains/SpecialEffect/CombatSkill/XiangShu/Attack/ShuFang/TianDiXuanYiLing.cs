using System;
using GameData.Domains.CombatSkill;

namespace GameData.Domains.SpecialEffect.CombatSkill.XiangShu.Attack.ShuFang
{
	// Token: 0x020002E7 RID: 743
	public class TianDiXuanYiLing : SilenceSkillAndMinAttribute
	{
		// Token: 0x06003334 RID: 13108 RVA: 0x00223683 File Offset: 0x00221883
		public TianDiXuanYiLing()
		{
		}

		// Token: 0x06003335 RID: 13109 RVA: 0x0022368D File Offset: 0x0022188D
		public TianDiXuanYiLing(CombatSkillKey skillKey) : base(skillKey, 17082)
		{
			this.SilenceSkillCount = 3;
		}
	}
}
