using System;

namespace GameData.Domains.SpecialEffect.CombatSkill.Xuehoujiao.BreakBodyEffect
{
	// Token: 0x02000259 RID: 601
	public class LegHurt : LegBreakBase
	{
		// Token: 0x06003034 RID: 12340 RVA: 0x0021659F File Offset: 0x0021479F
		public LegHurt()
		{
		}

		// Token: 0x06003035 RID: 12341 RVA: 0x002165A9 File Offset: 0x002147A9
		public LegHurt(int charId) : base(charId, 15509)
		{
			this.IsInner = true;
			this.FeatureId = 254;
		}
	}
}
