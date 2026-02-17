using System;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Attack;

namespace GameData.Domains.SpecialEffect.CombatSkill.Yuanshanpai.Leg
{
	// Token: 0x02000201 RID: 513
	public class WuDingKaiShanTui : AttackSkillFiveElementsType
	{
		// Token: 0x06002E9E RID: 11934 RVA: 0x00210047 File Offset: 0x0020E247
		public WuDingKaiShanTui()
		{
		}

		// Token: 0x06002E9F RID: 11935 RVA: 0x00210051 File Offset: 0x0020E251
		public WuDingKaiShanTui(CombatSkillKey skillKey) : base(skillKey, 5105)
		{
			this.AffectFiveElementsType = 2;
		}
	}
}
