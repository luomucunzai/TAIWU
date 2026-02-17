using System;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Neigong;

namespace GameData.Domains.SpecialEffect.CombatSkill.Jingangzong.Neigong
{
	// Token: 0x020004B9 RID: 1209
	public class SanMiHeYingFa : AddFiveElementsDamage
	{
		// Token: 0x06003CF3 RID: 15603 RVA: 0x0024F6F6 File Offset: 0x0024D8F6
		public SanMiHeYingFa()
		{
		}

		// Token: 0x06003CF4 RID: 15604 RVA: 0x0024F700 File Offset: 0x0024D900
		public SanMiHeYingFa(CombatSkillKey skillKey) : base(skillKey, 11002)
		{
			this.RequireSelfFiveElementsType = 0;
			this.AffectFiveElementsType = (base.IsDirect ? 1 : 3);
		}
	}
}
