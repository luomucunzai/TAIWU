using System;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.LegendaryBook.Common;

namespace GameData.Domains.SpecialEffect.LegendaryBook.Leg
{
	// Token: 0x0200015A RID: 346
	public class ShouZhi : ReduceGridCost
	{
		// Token: 0x06002B01 RID: 11009 RVA: 0x002047B3 File Offset: 0x002029B3
		public ShouZhi()
		{
		}

		// Token: 0x06002B02 RID: 11010 RVA: 0x002047BD File Offset: 0x002029BD
		public ShouZhi(CombatSkillKey skillKey) : base(skillKey, 40505)
		{
		}
	}
}
