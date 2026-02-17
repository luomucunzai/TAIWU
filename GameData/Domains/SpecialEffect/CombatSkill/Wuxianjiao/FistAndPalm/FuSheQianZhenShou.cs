using System;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Attack;

namespace GameData.Domains.SpecialEffect.CombatSkill.Wuxianjiao.FistAndPalm
{
	// Token: 0x02000393 RID: 915
	public class FuSheQianZhenShou : PoisonDisableAgileOrDefense
	{
		// Token: 0x06003656 RID: 13910 RVA: 0x002305A0 File Offset: 0x0022E7A0
		public FuSheQianZhenShou()
		{
		}

		// Token: 0x06003657 RID: 13911 RVA: 0x002305AA File Offset: 0x0022E7AA
		public FuSheQianZhenShou(CombatSkillKey skillKey) : base(skillKey, 12101)
		{
			this.RequirePoisonType = 5;
		}
	}
}
