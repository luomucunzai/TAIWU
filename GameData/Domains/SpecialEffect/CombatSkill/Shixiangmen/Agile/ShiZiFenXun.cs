using System;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Agile;

namespace GameData.Domains.SpecialEffect.CombatSkill.Shixiangmen.Agile
{
	// Token: 0x0200040E RID: 1038
	public class ShiZiFenXun : AttackChangeMobility
	{
		// Token: 0x06003902 RID: 14594 RVA: 0x0023CC79 File Offset: 0x0023AE79
		public ShiZiFenXun()
		{
		}

		// Token: 0x06003903 RID: 14595 RVA: 0x0023CC83 File Offset: 0x0023AE83
		public ShiZiFenXun(CombatSkillKey skillKey) : base(skillKey, 6403)
		{
			this.RequireWeaponSubType = 4;
		}
	}
}
