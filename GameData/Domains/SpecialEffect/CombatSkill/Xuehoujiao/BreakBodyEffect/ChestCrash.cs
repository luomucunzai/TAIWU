using System;

namespace GameData.Domains.SpecialEffect.CombatSkill.Xuehoujiao.BreakBodyEffect
{
	// Token: 0x0200024F RID: 591
	public class ChestCrash : ChestBreakBase
	{
		// Token: 0x06003013 RID: 12307 RVA: 0x00215E17 File Offset: 0x00214017
		public ChestCrash()
		{
		}

		// Token: 0x06003014 RID: 12308 RVA: 0x00215E21 File Offset: 0x00214021
		public ChestCrash(int charId) : base(charId, 15500)
		{
			this.IsInner = false;
			this.FeatureId = 249;
		}
	}
}
