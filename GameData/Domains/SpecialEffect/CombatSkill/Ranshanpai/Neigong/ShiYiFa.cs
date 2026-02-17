using System;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Neigong;

namespace GameData.Domains.SpecialEffect.CombatSkill.Ranshanpai.Neigong
{
	// Token: 0x02000455 RID: 1109
	public class ShiYiFa : ReduceFiveElementsDamage
	{
		// Token: 0x06003AA2 RID: 15010 RVA: 0x00244536 File Offset: 0x00242736
		public ShiYiFa()
		{
		}

		// Token: 0x06003AA3 RID: 15011 RVA: 0x00244540 File Offset: 0x00242740
		public ShiYiFa(CombatSkillKey skillKey) : base(skillKey, 7002)
		{
			this.RequireSelfFiveElementsType = 1;
			this.AffectFiveElementsType = (base.IsDirect ? 4 : 0);
		}
	}
}
