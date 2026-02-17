using System;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.LegendaryBook.Common;

namespace GameData.Domains.SpecialEffect.LegendaryBook.Throw
{
	// Token: 0x02000122 RID: 290
	public class ShouZhi : ReduceGridCost
	{
		// Token: 0x06002A3A RID: 10810 RVA: 0x0020226E File Offset: 0x0020046E
		public ShouZhi()
		{
		}

		// Token: 0x06002A3B RID: 10811 RVA: 0x00202278 File Offset: 0x00200478
		public ShouZhi(CombatSkillKey skillKey) : base(skillKey, 40605)
		{
		}
	}
}
