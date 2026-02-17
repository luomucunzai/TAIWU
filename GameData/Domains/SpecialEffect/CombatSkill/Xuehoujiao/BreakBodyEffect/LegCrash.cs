using System;

namespace GameData.Domains.SpecialEffect.CombatSkill.Xuehoujiao.BreakBodyEffect
{
	// Token: 0x02000258 RID: 600
	public class LegCrash : LegBreakBase
	{
		// Token: 0x06003032 RID: 12338 RVA: 0x00216573 File Offset: 0x00214773
		public LegCrash()
		{
		}

		// Token: 0x06003033 RID: 12339 RVA: 0x0021657D File Offset: 0x0021477D
		public LegCrash(int charId) : base(charId, 15508)
		{
			this.IsInner = false;
			this.FeatureId = 255;
		}
	}
}
