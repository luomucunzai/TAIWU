using System;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Neigong;

namespace GameData.Domains.SpecialEffect.CombatSkill.Fulongtan.Neigong
{
	// Token: 0x02000516 RID: 1302
	public class LiHuoZhenQi : AddFiveElementsDamage
	{
		// Token: 0x06003EEC RID: 16108 RVA: 0x002577F9 File Offset: 0x002559F9
		public LiHuoZhenQi()
		{
		}

		// Token: 0x06003EED RID: 16109 RVA: 0x00257803 File Offset: 0x00255A03
		public LiHuoZhenQi(CombatSkillKey skillKey) : base(skillKey, 14002)
		{
			this.RequireSelfFiveElementsType = 3;
			this.AffectFiveElementsType = (base.IsDirect ? 0 : 2);
		}
	}
}
