using System;

namespace GameData.Domains.SpecialEffect.CombatSkill.Xuehoujiao.BreakBodyEffect
{
	// Token: 0x02000252 RID: 594
	public class HandCrash : HandBreakBase
	{
		// Token: 0x0600301C RID: 12316 RVA: 0x00215F62 File Offset: 0x00214162
		public HandCrash()
		{
		}

		// Token: 0x0600301D RID: 12317 RVA: 0x00215F6C File Offset: 0x0021416C
		public HandCrash(int charId) : base(charId, 15506)
		{
			this.IsInner = false;
			this.FeatureId = 253;
		}
	}
}
