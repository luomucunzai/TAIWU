using System;

namespace GameData.Domains.SpecialEffect.CombatSkill.Xuehoujiao.BreakBodyEffect
{
	// Token: 0x0200024B RID: 587
	public class BellyHurt : BellyBreakBase
	{
		// Token: 0x06003002 RID: 12290 RVA: 0x00215877 File Offset: 0x00213A77
		public BellyHurt()
		{
		}

		// Token: 0x06003003 RID: 12291 RVA: 0x00215881 File Offset: 0x00213A81
		public BellyHurt(int charId) : base(charId, 15503)
		{
			this.IsInner = true;
			this.FeatureId = 250;
		}
	}
}
