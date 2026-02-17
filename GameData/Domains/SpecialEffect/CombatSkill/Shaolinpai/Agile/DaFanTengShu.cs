using System;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Agile;

namespace GameData.Domains.SpecialEffect.CombatSkill.Shaolinpai.Agile
{
	// Token: 0x0200043C RID: 1084
	public class DaFanTengShu : AttackChangeMobility
	{
		// Token: 0x060039FA RID: 14842 RVA: 0x00241717 File Offset: 0x0023F917
		public DaFanTengShu()
		{
		}

		// Token: 0x060039FB RID: 14843 RVA: 0x00241721 File Offset: 0x0023F921
		public DaFanTengShu(CombatSkillKey skillKey) : base(skillKey, 1403)
		{
			this.RequireWeaponSubType = 10;
		}
	}
}
