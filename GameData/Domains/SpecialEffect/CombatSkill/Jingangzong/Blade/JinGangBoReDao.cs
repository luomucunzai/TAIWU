using System;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Attack;

namespace GameData.Domains.SpecialEffect.CombatSkill.Jingangzong.Blade
{
	// Token: 0x020004CD RID: 1229
	public class JinGangBoReDao : AttackSkillFiveElementsType
	{
		// Token: 0x06003D5E RID: 15710 RVA: 0x0025179B File Offset: 0x0024F99B
		public JinGangBoReDao()
		{
		}

		// Token: 0x06003D5F RID: 15711 RVA: 0x002517A5 File Offset: 0x0024F9A5
		public JinGangBoReDao(CombatSkillKey skillKey) : base(skillKey, 11205)
		{
			this.AffectFiveElementsType = 1;
		}
	}
}
