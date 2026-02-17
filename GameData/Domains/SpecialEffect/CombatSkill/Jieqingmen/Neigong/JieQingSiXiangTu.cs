using System;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Neigong;

namespace GameData.Domains.SpecialEffect.CombatSkill.Jieqingmen.Neigong
{
	// Token: 0x020004EE RID: 1262
	public class JieQingSiXiangTu : BaseSectNeigong
	{
		// Token: 0x06003E25 RID: 15909 RVA: 0x00254D51 File Offset: 0x00252F51
		public JieQingSiXiangTu()
		{
		}

		// Token: 0x06003E26 RID: 15910 RVA: 0x00254D5B File Offset: 0x00252F5B
		public JieQingSiXiangTu(CombatSkillKey skillKey) : base(skillKey, 13000)
		{
			this.SectId = 13;
		}
	}
}
