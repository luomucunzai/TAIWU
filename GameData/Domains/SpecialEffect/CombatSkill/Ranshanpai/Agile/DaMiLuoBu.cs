using System;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Agile;

namespace GameData.Domains.SpecialEffect.CombatSkill.Ranshanpai.Agile
{
	// Token: 0x0200046A RID: 1130
	public class DaMiLuoBu : AttackChangeMobility
	{
		// Token: 0x06003B23 RID: 15139 RVA: 0x00246C1E File Offset: 0x00244E1E
		public DaMiLuoBu()
		{
		}

		// Token: 0x06003B24 RID: 15140 RVA: 0x00246C28 File Offset: 0x00244E28
		public DaMiLuoBu(CombatSkillKey skillKey) : base(skillKey, 7402)
		{
			this.RequireWeaponSubType = 13;
		}
	}
}
