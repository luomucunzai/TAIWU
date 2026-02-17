using System;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.LegendaryBook.Common;

namespace GameData.Domains.SpecialEffect.LegendaryBook.Special
{
	// Token: 0x02000135 RID: 309
	public class ShouZhi : ReduceGridCost
	{
		// Token: 0x06002A6F RID: 10863 RVA: 0x00202973 File Offset: 0x00200B73
		public ShouZhi()
		{
		}

		// Token: 0x06002A70 RID: 10864 RVA: 0x0020297D File Offset: 0x00200B7D
		public ShouZhi(CombatSkillKey skillKey) : base(skillKey, 41005)
		{
		}
	}
}
