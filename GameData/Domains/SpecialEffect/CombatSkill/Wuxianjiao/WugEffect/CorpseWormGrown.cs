using System;
using GameData.Domains.Item;

namespace GameData.Domains.SpecialEffect.CombatSkill.Wuxianjiao.WugEffect
{
	// Token: 0x02000356 RID: 854
	public class CorpseWormGrown : CorpseWormBase
	{
		// Token: 0x06003532 RID: 13618 RVA: 0x0022BDE5 File Offset: 0x00229FE5
		public CorpseWormGrown()
		{
		}

		// Token: 0x06003533 RID: 13619 RVA: 0x0022BDEF File Offset: 0x00229FEF
		public CorpseWormGrown(int charId) : base(charId, 12524, ItemDomain.GetWugTemplateId(4, 4), 473)
		{
		}
	}
}
