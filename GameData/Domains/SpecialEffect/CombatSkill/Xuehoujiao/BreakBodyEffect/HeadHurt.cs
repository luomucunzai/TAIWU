using System;

namespace GameData.Domains.SpecialEffect.CombatSkill.Xuehoujiao.BreakBodyEffect
{
	// Token: 0x02000256 RID: 598
	public class HeadHurt : HeadBreakBase
	{
		// Token: 0x0600302B RID: 12331 RVA: 0x00216449 File Offset: 0x00214649
		public HeadHurt()
		{
		}

		// Token: 0x0600302C RID: 12332 RVA: 0x00216453 File Offset: 0x00214653
		public HeadHurt(int charId) : base(charId, 15505)
		{
			this.IsInner = true;
			this.FeatureId = 246;
		}
	}
}
