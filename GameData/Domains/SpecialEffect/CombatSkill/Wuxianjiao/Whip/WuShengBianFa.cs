using System;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Attack;

namespace GameData.Domains.SpecialEffect.CombatSkill.Wuxianjiao.Whip
{
	// Token: 0x02000383 RID: 899
	public class WuShengBianFa : ReverseNext
	{
		// Token: 0x06003605 RID: 13829 RVA: 0x0022EEF5 File Offset: 0x0022D0F5
		public WuShengBianFa()
		{
		}

		// Token: 0x06003606 RID: 13830 RVA: 0x0022EEFF File Offset: 0x0022D0FF
		public WuShengBianFa(CombatSkillKey skillKey) : base(skillKey, 12402)
		{
			this.AffectSectId = 12;
			this.AffectSkillType = 11;
		}
	}
}
