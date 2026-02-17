using System;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Agile;

namespace GameData.Domains.SpecialEffect.CombatSkill.Jieqingmen.Agile
{
	// Token: 0x02000509 RID: 1289
	public class WuYouBu : AttackChangeMobility
	{
		// Token: 0x06003EBF RID: 16063 RVA: 0x0025709F File Offset: 0x0025529F
		public WuYouBu()
		{
		}

		// Token: 0x06003EC0 RID: 16064 RVA: 0x002570A9 File Offset: 0x002552A9
		public WuYouBu(CombatSkillKey skillKey) : base(skillKey, 13402)
		{
			this.RequireWeaponSubType = 2;
		}
	}
}
