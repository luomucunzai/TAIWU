using System;
using GameData.Domains.Item;

namespace GameData.Domains.SpecialEffect.CombatSkill.Wuxianjiao.WugEffect
{
	// Token: 0x02000373 RID: 883
	public class IceSilkwormKing : IceSilkwormBase
	{
		// Token: 0x0600359B RID: 13723 RVA: 0x0022D2E2 File Offset: 0x0022B4E2
		public IceSilkwormKing()
		{
		}

		// Token: 0x0600359C RID: 13724 RVA: 0x0022D2EC File Offset: 0x0022B4EC
		public IceSilkwormKing(int charId) : base(charId, 12545, ItemDomain.GetWugTemplateId(5, 5), 474)
		{
		}
	}
}
