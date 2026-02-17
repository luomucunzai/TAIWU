using System;

namespace GameData.Domains.SpecialEffect.CombatSkill.Xuehoujiao.BreakBodyEffect
{
	// Token: 0x02000255 RID: 597
	public class HeadCrash : HeadBreakBase
	{
		// Token: 0x06003029 RID: 12329 RVA: 0x0021641D File Offset: 0x0021461D
		public HeadCrash()
		{
		}

		// Token: 0x0600302A RID: 12330 RVA: 0x00216427 File Offset: 0x00214627
		public HeadCrash(int charId) : base(charId, 15504)
		{
			this.IsInner = false;
			this.FeatureId = 247;
		}
	}
}
