using System;
using GameData.Domains.Item;

namespace GameData.Domains.SpecialEffect.CombatSkill.Wuxianjiao.WugEffect
{
	// Token: 0x0200034F RID: 847
	public class BlackBloodGrown : BlackBloodBase
	{
		// Token: 0x06003517 RID: 13591 RVA: 0x0022B71A File Offset: 0x0022991A
		public BlackBloodGrown()
		{
		}

		// Token: 0x06003518 RID: 13592 RVA: 0x0022B724 File Offset: 0x00229924
		public BlackBloodGrown(int charId) : base(charId, 12514, ItemDomain.GetWugTemplateId(2, 4), 471)
		{
		}
	}
}
