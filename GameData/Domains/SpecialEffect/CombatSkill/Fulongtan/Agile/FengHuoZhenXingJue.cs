using System;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Agile;

namespace GameData.Domains.SpecialEffect.CombatSkill.Fulongtan.Agile
{
	// Token: 0x02000533 RID: 1331
	public class FengHuoZhenXingJue : AttackChangeMobility
	{
		// Token: 0x06003F9C RID: 16284 RVA: 0x0025AADD File Offset: 0x00258CDD
		public FengHuoZhenXingJue()
		{
		}

		// Token: 0x06003F9D RID: 16285 RVA: 0x0025AAE7 File Offset: 0x00258CE7
		public FengHuoZhenXingJue(CombatSkillKey skillKey) : base(skillKey, 14404)
		{
			this.RequireWeaponSubType = 9;
		}
	}
}
