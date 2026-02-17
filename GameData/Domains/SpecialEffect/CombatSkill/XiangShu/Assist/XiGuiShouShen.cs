using System;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Assist;

namespace GameData.Domains.SpecialEffect.CombatSkill.XiangShu.Assist
{
	// Token: 0x02000332 RID: 818
	public class XiGuiShouShen : AddDamageByHitType
	{
		// Token: 0x06003482 RID: 13442 RVA: 0x00228E1A File Offset: 0x0022701A
		public XiGuiShouShen()
		{
		}

		// Token: 0x06003483 RID: 13443 RVA: 0x00228E24 File Offset: 0x00227024
		public XiGuiShouShen(CombatSkillKey skillKey) : base(skillKey, 16406)
		{
			this.HitType = 2;
		}
	}
}
