using System;

namespace GameData.Domains.SpecialEffect.CombatSkill.Xuehoujiao.BreakBodyEffect
{
	// Token: 0x02000253 RID: 595
	public class HandHurt : HandBreakBase
	{
		// Token: 0x0600301E RID: 12318 RVA: 0x00215F8E File Offset: 0x0021418E
		public HandHurt()
		{
		}

		// Token: 0x0600301F RID: 12319 RVA: 0x00215F98 File Offset: 0x00214198
		public HandHurt(int charId) : base(charId, 15507)
		{
			this.IsInner = true;
			this.FeatureId = 252;
		}
	}
}
