using System;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Neigong;

namespace GameData.Domains.SpecialEffect.CombatSkill.Zhujianshanzhuang.Neigong
{
	// Token: 0x020001D0 RID: 464
	public class WuJinJiaBingPian : ReduceFiveElementsDamage
	{
		// Token: 0x06002D2D RID: 11565 RVA: 0x0020A813 File Offset: 0x00208A13
		public WuJinJiaBingPian()
		{
		}

		// Token: 0x06002D2E RID: 11566 RVA: 0x0020A81D File Offset: 0x00208A1D
		public WuJinJiaBingPian(CombatSkillKey skillKey) : base(skillKey, 9002)
		{
			this.RequireSelfFiveElementsType = 3;
			this.AffectFiveElementsType = (base.IsDirect ? 0 : 2);
		}
	}
}
