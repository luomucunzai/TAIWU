using System;
using GameData.Domains.Item;

namespace GameData.Domains.SpecialEffect.CombatSkill.Wuxianjiao.WugEffect
{
	// Token: 0x0200036C RID: 876
	public class GoldenSilkwormKing : GoldenSilkwormBase
	{
		// Token: 0x06003580 RID: 13696 RVA: 0x0022CE15 File Offset: 0x0022B015
		public GoldenSilkwormKing()
		{
		}

		// Token: 0x06003581 RID: 13697 RVA: 0x0022CE1F File Offset: 0x0022B01F
		public GoldenSilkwormKing(int charId) : base(charId, 12546, ItemDomain.GetWugTemplateId(6, 5), 475)
		{
		}
	}
}
