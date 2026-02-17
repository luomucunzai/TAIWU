using System;

namespace GameData.Domains.SpecialEffect.CombatSkill.Xuehoujiao.BreakBodyEffect
{
	// Token: 0x0200024A RID: 586
	public class BellyCrash : BellyBreakBase
	{
		// Token: 0x06003000 RID: 12288 RVA: 0x0021584B File Offset: 0x00213A4B
		public BellyCrash()
		{
		}

		// Token: 0x06003001 RID: 12289 RVA: 0x00215855 File Offset: 0x00213A55
		public BellyCrash(int charId) : base(charId, 15502)
		{
			this.IsInner = false;
			this.FeatureId = 251;
		}
	}
}
