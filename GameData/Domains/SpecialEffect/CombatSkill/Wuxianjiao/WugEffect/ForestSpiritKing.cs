using System;
using GameData.Domains.Item;

namespace GameData.Domains.SpecialEffect.CombatSkill.Wuxianjiao.WugEffect
{
	// Token: 0x02000365 RID: 869
	public class ForestSpiritKing : ForestSpiritBase
	{
		// Token: 0x06003563 RID: 13667 RVA: 0x0022C758 File Offset: 0x0022A958
		public ForestSpiritKing()
		{
		}

		// Token: 0x06003564 RID: 13668 RVA: 0x0022C762 File Offset: 0x0022A962
		public ForestSpiritKing(int charId) : base(charId, 12541, ItemDomain.GetWugTemplateId(1, 5), 470)
		{
		}
	}
}
