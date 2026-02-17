using System;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.LegendaryBook.Common;

namespace GameData.Domains.SpecialEffect.LegendaryBook.Blade
{
	// Token: 0x0200017D RID: 381
	public class ShouZhi : ReduceGridCost
	{
		// Token: 0x06002B6D RID: 11117 RVA: 0x002054CF File Offset: 0x002036CF
		public ShouZhi()
		{
		}

		// Token: 0x06002B6E RID: 11118 RVA: 0x002054D9 File Offset: 0x002036D9
		public ShouZhi(CombatSkillKey skillKey) : base(skillKey, 40805)
		{
		}
	}
}
