using System;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Attack;

namespace GameData.Domains.SpecialEffect.CombatSkill.Jieqingmen.Sword
{
	// Token: 0x020004EA RID: 1258
	public class YouMingJianFa : AttackSkillFiveElementsType
	{
		// Token: 0x06003E1C RID: 15900 RVA: 0x00254CB8 File Offset: 0x00252EB8
		public YouMingJianFa()
		{
		}

		// Token: 0x06003E1D RID: 15901 RVA: 0x00254CC2 File Offset: 0x00252EC2
		public YouMingJianFa(CombatSkillKey skillKey) : base(skillKey, 13205)
		{
			this.AffectFiveElementsType = 3;
		}
	}
}
