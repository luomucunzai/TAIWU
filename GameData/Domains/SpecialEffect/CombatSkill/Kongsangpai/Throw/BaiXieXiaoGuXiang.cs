using System;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Attack;

namespace GameData.Domains.SpecialEffect.CombatSkill.Kongsangpai.Throw
{
	// Token: 0x02000476 RID: 1142
	public class BaiXieXiaoGuXiang : StrengthenPoison
	{
		// Token: 0x06003B69 RID: 15209 RVA: 0x00247CD7 File Offset: 0x00245ED7
		public BaiXieXiaoGuXiang()
		{
		}

		// Token: 0x06003B6A RID: 15210 RVA: 0x00247CE1 File Offset: 0x00245EE1
		public BaiXieXiaoGuXiang(CombatSkillKey skillKey) : base(skillKey, 10406)
		{
			this.AffectPoisonType = 1;
		}
	}
}
