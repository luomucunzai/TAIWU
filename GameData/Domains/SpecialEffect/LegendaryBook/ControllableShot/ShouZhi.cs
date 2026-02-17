using System;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.LegendaryBook.Common;

namespace GameData.Domains.SpecialEffect.LegendaryBook.ControllableShot
{
	// Token: 0x0200016D RID: 365
	public class ShouZhi : ReduceGridCost
	{
		// Token: 0x06002B33 RID: 11059 RVA: 0x00204BF9 File Offset: 0x00202DF9
		public ShouZhi()
		{
		}

		// Token: 0x06002B34 RID: 11060 RVA: 0x00204C03 File Offset: 0x00202E03
		public ShouZhi(CombatSkillKey skillKey) : base(skillKey, 41205)
		{
		}
	}
}
