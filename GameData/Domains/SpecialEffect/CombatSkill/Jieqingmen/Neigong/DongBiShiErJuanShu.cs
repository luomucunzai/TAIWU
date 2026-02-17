using System;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Neigong;

namespace GameData.Domains.SpecialEffect.CombatSkill.Jieqingmen.Neigong
{
	// Token: 0x020004EC RID: 1260
	public class DongBiShiErJuanShu : ReduceFiveElementsDamage
	{
		// Token: 0x06003E21 RID: 15905 RVA: 0x00254CFD File Offset: 0x00252EFD
		public DongBiShiErJuanShu()
		{
		}

		// Token: 0x06003E22 RID: 15906 RVA: 0x00254D07 File Offset: 0x00252F07
		public DongBiShiErJuanShu(CombatSkillKey skillKey) : base(skillKey, 13002)
		{
			this.RequireSelfFiveElementsType = 2;
			this.AffectFiveElementsType = (base.IsDirect ? 3 : 4);
		}
	}
}
