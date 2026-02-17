using System;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Attack;

namespace GameData.Domains.SpecialEffect.CombatSkill.Xuannvpai.FistAndPalm
{
	// Token: 0x02000271 RID: 625
	public class HanBingShenZhang : StrengthenPoison
	{
		// Token: 0x06003099 RID: 12441 RVA: 0x00217D9B File Offset: 0x00215F9B
		public HanBingShenZhang()
		{
		}

		// Token: 0x0600309A RID: 12442 RVA: 0x00217DA5 File Offset: 0x00215FA5
		public HanBingShenZhang(CombatSkillKey skillKey) : base(skillKey, 8106)
		{
			this.AffectPoisonType = 2;
		}
	}
}
