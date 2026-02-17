using System;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Attack;

namespace GameData.Domains.SpecialEffect.CombatSkill.Yuanshanpai.Blade
{
	// Token: 0x0200020D RID: 525
	public class HuBuBaJiDao : StrengthenOnSwitchWeapon
	{
		// Token: 0x06002EE0 RID: 12000 RVA: 0x00210FCA File Offset: 0x0020F1CA
		public HuBuBaJiDao()
		{
		}

		// Token: 0x06002EE1 RID: 12001 RVA: 0x00210FD4 File Offset: 0x0020F1D4
		public HuBuBaJiDao(CombatSkillKey skillKey) : base(skillKey, 5302)
		{
			this.RequireWeaponSubType = 9;
		}
	}
}
