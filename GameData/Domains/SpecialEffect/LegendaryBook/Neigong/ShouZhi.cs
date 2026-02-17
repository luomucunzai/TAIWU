using System;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.LegendaryBook.Common;

namespace GameData.Domains.SpecialEffect.LegendaryBook.Neigong
{
	// Token: 0x02000154 RID: 340
	public class ShouZhi : ReduceGridCost
	{
		// Token: 0x06002AED RID: 10989 RVA: 0x00204519 File Offset: 0x00202719
		public ShouZhi()
		{
		}

		// Token: 0x06002AEE RID: 10990 RVA: 0x00204523 File Offset: 0x00202723
		public ShouZhi(CombatSkillKey skillKey) : base(skillKey, 40006)
		{
		}
	}
}
