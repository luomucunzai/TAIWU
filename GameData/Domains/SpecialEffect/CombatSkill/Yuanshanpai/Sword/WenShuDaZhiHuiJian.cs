using System;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Attack;

namespace GameData.Domains.SpecialEffect.CombatSkill.Yuanshanpai.Sword
{
	// Token: 0x020001F5 RID: 501
	public class WenShuDaZhiHuiJian : StrengthenByWrongWeapon
	{
		// Token: 0x06002E54 RID: 11860 RVA: 0x0020E6D0 File Offset: 0x0020C8D0
		public WenShuDaZhiHuiJian()
		{
		}

		// Token: 0x06002E55 RID: 11861 RVA: 0x0020E6DA File Offset: 0x0020C8DA
		public WenShuDaZhiHuiJian(CombatSkillKey skillKey) : base(skillKey, 5204)
		{
			this.RequireWeaponSubType = 8;
		}
	}
}
