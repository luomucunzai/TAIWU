using System;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Neigong;

namespace GameData.Domains.SpecialEffect.CombatSkill.Xuannvpai.Neigong
{
	// Token: 0x02000263 RID: 611
	public class WangShuYuYueGong : AddFiveElementsDamage
	{
		// Token: 0x0600305A RID: 12378 RVA: 0x00216C31 File Offset: 0x00214E31
		public WangShuYuYueGong()
		{
		}

		// Token: 0x0600305B RID: 12379 RVA: 0x00216C3B File Offset: 0x00214E3B
		public WangShuYuYueGong(CombatSkillKey skillKey) : base(skillKey, 8002)
		{
			this.RequireSelfFiveElementsType = 2;
			this.AffectFiveElementsType = (base.IsDirect ? 3 : 4);
		}
	}
}
