using System;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Agile;

namespace GameData.Domains.SpecialEffect.CombatSkill.Baihuagu.Agile
{
	// Token: 0x020005E0 RID: 1504
	public class QianNiuHuanShenBu : AttackChangeMobility
	{
		// Token: 0x06004462 RID: 17506 RVA: 0x0026F5F3 File Offset: 0x0026D7F3
		public QianNiuHuanShenBu()
		{
		}

		// Token: 0x06004463 RID: 17507 RVA: 0x0026F5FD File Offset: 0x0026D7FD
		public QianNiuHuanShenBu(CombatSkillKey skillKey) : base(skillKey, 3401)
		{
			this.RequireWeaponSubType = 3;
		}
	}
}
