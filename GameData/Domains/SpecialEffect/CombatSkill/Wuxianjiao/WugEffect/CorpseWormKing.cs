using System;
using GameData.Domains.Item;

namespace GameData.Domains.SpecialEffect.CombatSkill.Wuxianjiao.WugEffect
{
	// Token: 0x02000357 RID: 855
	public class CorpseWormKing : CorpseWormBase
	{
		// Token: 0x06003534 RID: 13620 RVA: 0x0022BE0B File Offset: 0x0022A00B
		public CorpseWormKing()
		{
		}

		// Token: 0x06003535 RID: 13621 RVA: 0x0022BE15 File Offset: 0x0022A015
		public CorpseWormKing(int charId) : base(charId, 12544, ItemDomain.GetWugTemplateId(4, 5), 473)
		{
		}
	}
}
