using System;

namespace GameData.Domains.SpecialEffect.CombatSkill.Xuehoujiao.BreakBodyEffect
{
	// Token: 0x02000250 RID: 592
	public class ChestHurt : ChestBreakBase
	{
		// Token: 0x06003015 RID: 12309 RVA: 0x00215E43 File Offset: 0x00214043
		public ChestHurt()
		{
		}

		// Token: 0x06003016 RID: 12310 RVA: 0x00215E4D File Offset: 0x0021404D
		public ChestHurt(int charId) : base(charId, 15501)
		{
			this.IsInner = true;
			this.FeatureId = 248;
		}
	}
}
