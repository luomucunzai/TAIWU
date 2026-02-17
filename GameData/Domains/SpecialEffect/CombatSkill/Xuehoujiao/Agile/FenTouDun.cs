using System;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Agile;

namespace GameData.Domains.SpecialEffect.CombatSkill.Xuehoujiao.Agile
{
	// Token: 0x0200025B RID: 603
	public class FenTouDun : AttackChangeMobility
	{
		// Token: 0x0600303D RID: 12349 RVA: 0x002167A9 File Offset: 0x002149A9
		public FenTouDun()
		{
		}

		// Token: 0x0600303E RID: 12350 RVA: 0x002167B3 File Offset: 0x002149B3
		public FenTouDun(CombatSkillKey skillKey) : base(skillKey, 15601)
		{
			this.RequireWeaponSubType = 15;
		}
	}
}
