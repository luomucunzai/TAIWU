using System;
using GameData.Domains.Item;

namespace GameData.Domains.SpecialEffect.CombatSkill.Wuxianjiao.WugEffect
{
	// Token: 0x02000372 RID: 882
	public class IceSilkwormGrown : IceSilkwormBase
	{
		// Token: 0x06003599 RID: 13721 RVA: 0x0022D2BC File Offset: 0x0022B4BC
		public IceSilkwormGrown()
		{
		}

		// Token: 0x0600359A RID: 13722 RVA: 0x0022D2C6 File Offset: 0x0022B4C6
		public IceSilkwormGrown(int charId) : base(charId, 12529, ItemDomain.GetWugTemplateId(5, 4), 474)
		{
		}
	}
}
