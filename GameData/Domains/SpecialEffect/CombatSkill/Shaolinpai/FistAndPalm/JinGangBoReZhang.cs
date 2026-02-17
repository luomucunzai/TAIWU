using System;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Attack;

namespace GameData.Domains.SpecialEffect.CombatSkill.Shaolinpai.FistAndPalm
{
	// Token: 0x02000426 RID: 1062
	public class JinGangBoReZhang : AttackNeiliFiveElementsType
	{
		// Token: 0x06003975 RID: 14709 RVA: 0x0023EA5D File Offset: 0x0023CC5D
		public JinGangBoReZhang()
		{
		}

		// Token: 0x06003976 RID: 14710 RVA: 0x0023EA67 File Offset: 0x0023CC67
		public JinGangBoReZhang(CombatSkillKey skillKey) : base(skillKey, 1106)
		{
			this.AffectFiveElementsType = 1;
		}
	}
}
