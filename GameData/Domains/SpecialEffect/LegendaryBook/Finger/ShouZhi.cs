using System;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.LegendaryBook.Common;

namespace GameData.Domains.SpecialEffect.LegendaryBook.Finger
{
	// Token: 0x02000166 RID: 358
	public class ShouZhi : ReduceGridCost
	{
		// Token: 0x06002B21 RID: 11041 RVA: 0x00204A77 File Offset: 0x00202C77
		public ShouZhi()
		{
		}

		// Token: 0x06002B22 RID: 11042 RVA: 0x00204A81 File Offset: 0x00202C81
		public ShouZhi(CombatSkillKey skillKey) : base(skillKey, 40405)
		{
		}
	}
}
