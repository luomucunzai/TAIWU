using System;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Agile;

namespace GameData.Domains.SpecialEffect.CombatSkill.Jingangzong.Agile
{
	// Token: 0x020004D7 RID: 1239
	public class JinGangZuoFa : AttackChangeMobility
	{
		// Token: 0x06003D8E RID: 15758 RVA: 0x002525B9 File Offset: 0x002507B9
		public JinGangZuoFa()
		{
		}

		// Token: 0x06003D8F RID: 15759 RVA: 0x002525C3 File Offset: 0x002507C3
		public JinGangZuoFa(CombatSkillKey skillKey) : base(skillKey, 11502)
		{
			this.RequireWeaponSubType = 5;
		}
	}
}
