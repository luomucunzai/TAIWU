using System;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Attack;

namespace GameData.Domains.SpecialEffect.CombatSkill.Yuanshanpai.Sword
{
	// Token: 0x020001F3 RID: 499
	public class QiShiErChenJian : StrengthenOnSwitchWeapon
	{
		// Token: 0x06002E4D RID: 11853 RVA: 0x0020E687 File Offset: 0x0020C887
		public QiShiErChenJian()
		{
		}

		// Token: 0x06002E4E RID: 11854 RVA: 0x0020E691 File Offset: 0x0020C891
		public QiShiErChenJian(CombatSkillKey skillKey) : base(skillKey, 5202)
		{
			this.RequireWeaponSubType = 8;
		}
	}
}
