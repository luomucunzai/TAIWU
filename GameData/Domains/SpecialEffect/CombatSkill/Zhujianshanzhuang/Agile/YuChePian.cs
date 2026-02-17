using System;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Agile;

namespace GameData.Domains.SpecialEffect.CombatSkill.Zhujianshanzhuang.Agile
{
	// Token: 0x020001EE RID: 494
	public class YuChePian : AttackChangeMobility
	{
		// Token: 0x06002E3A RID: 11834 RVA: 0x0020E336 File Offset: 0x0020C536
		public YuChePian()
		{
		}

		// Token: 0x06002E3B RID: 11835 RVA: 0x0020E340 File Offset: 0x0020C540
		public YuChePian(CombatSkillKey skillKey) : base(skillKey, 9501)
		{
			this.RequireWeaponSubType = 12;
		}
	}
}
