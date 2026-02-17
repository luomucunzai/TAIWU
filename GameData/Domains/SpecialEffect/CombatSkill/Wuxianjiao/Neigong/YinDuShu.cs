using System;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Neigong;

namespace GameData.Domains.SpecialEffect.CombatSkill.Wuxianjiao.Neigong
{
	// Token: 0x02000392 RID: 914
	public class YinDuShu : BaseSectNeigong
	{
		// Token: 0x06003654 RID: 13908 RVA: 0x0023057E File Offset: 0x0022E77E
		public YinDuShu()
		{
		}

		// Token: 0x06003655 RID: 13909 RVA: 0x00230588 File Offset: 0x0022E788
		public YinDuShu(CombatSkillKey skillKey) : base(skillKey, 12000)
		{
			this.SectId = 12;
		}
	}
}
