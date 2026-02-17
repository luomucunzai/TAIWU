using System;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Agile;

namespace GameData.Domains.SpecialEffect.CombatSkill.Baihuagu.Agile
{
	// Token: 0x020005DE RID: 1502
	public class FeiZhenChuanSuoShu : AttackChangeMobility
	{
		// Token: 0x0600445D RID: 17501 RVA: 0x0026F58B File Offset: 0x0026D78B
		public FeiZhenChuanSuoShu()
		{
		}

		// Token: 0x0600445E RID: 17502 RVA: 0x0026F595 File Offset: 0x0026D795
		public FeiZhenChuanSuoShu(CombatSkillKey skillKey) : base(skillKey, 3402)
		{
			this.RequireWeaponSubType = 0;
		}
	}
}
