using System;
using GameData.Domains.Item;

namespace GameData.Domains.SpecialEffect.CombatSkill.Wuxianjiao.WugEffect
{
	// Token: 0x02000364 RID: 868
	public class ForestSpiritGrown : ForestSpiritBase
	{
		// Token: 0x06003561 RID: 13665 RVA: 0x0022C732 File Offset: 0x0022A932
		public ForestSpiritGrown()
		{
		}

		// Token: 0x06003562 RID: 13666 RVA: 0x0022C73C File Offset: 0x0022A93C
		public ForestSpiritGrown(int charId) : base(charId, 12509, ItemDomain.GetWugTemplateId(1, 4), 470)
		{
		}
	}
}
