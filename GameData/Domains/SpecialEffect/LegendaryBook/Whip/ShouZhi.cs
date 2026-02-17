using System;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.LegendaryBook.Common;

namespace GameData.Domains.SpecialEffect.LegendaryBook.Whip
{
	// Token: 0x0200011C RID: 284
	public class ShouZhi : ReduceGridCost
	{
		// Token: 0x06002A2A RID: 10794 RVA: 0x002020D1 File Offset: 0x002002D1
		public ShouZhi()
		{
		}

		// Token: 0x06002A2B RID: 10795 RVA: 0x002020DB File Offset: 0x002002DB
		public ShouZhi(CombatSkillKey skillKey) : base(skillKey, 41105)
		{
		}
	}
}
