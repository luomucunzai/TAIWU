using System;
using GameData.Domains.Item;

namespace GameData.Domains.SpecialEffect.CombatSkill.Wuxianjiao.WugEffect
{
	// Token: 0x02000348 RID: 840
	public class AzureMarrowGrown : AzureMarrowBase
	{
		// Token: 0x060034FD RID: 13565 RVA: 0x0022B177 File Offset: 0x00229377
		public AzureMarrowGrown()
		{
		}

		// Token: 0x060034FE RID: 13566 RVA: 0x0022B181 File Offset: 0x00229381
		public AzureMarrowGrown(int charId) : base(charId, 12539, ItemDomain.GetWugTemplateId(7, 4), 476)
		{
		}
	}
}
