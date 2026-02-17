using System;
using GameData.Domains.Item;

namespace GameData.Domains.SpecialEffect.CombatSkill.Wuxianjiao.WugEffect
{
	// Token: 0x0200036B RID: 875
	public class GoldenSilkwormGrown : GoldenSilkwormBase
	{
		// Token: 0x0600357E RID: 13694 RVA: 0x0022CDEF File Offset: 0x0022AFEF
		public GoldenSilkwormGrown()
		{
		}

		// Token: 0x0600357F RID: 13695 RVA: 0x0022CDF9 File Offset: 0x0022AFF9
		public GoldenSilkwormGrown(int charId) : base(charId, 12534, ItemDomain.GetWugTemplateId(6, 4), 475)
		{
		}
	}
}
