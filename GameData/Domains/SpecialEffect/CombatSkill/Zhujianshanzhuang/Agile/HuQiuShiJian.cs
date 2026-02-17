using System;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Agile;

namespace GameData.Domains.SpecialEffect.CombatSkill.Zhujianshanzhuang.Agile
{
	// Token: 0x020001EA RID: 490
	public class HuQiuShiJian : AttackChangeMobility
	{
		// Token: 0x06002E1D RID: 11805 RVA: 0x0020DC89 File Offset: 0x0020BE89
		public HuQiuShiJian()
		{
		}

		// Token: 0x06002E1E RID: 11806 RVA: 0x0020DC93 File Offset: 0x0020BE93
		public HuQiuShiJian(CombatSkillKey skillKey) : base(skillKey, 9503)
		{
			this.RequireWeaponSubType = 8;
		}
	}
}
