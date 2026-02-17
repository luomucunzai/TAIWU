using System;
using GameData.Domains.Item;

namespace GameData.Domains.SpecialEffect.CombatSkill.Wuxianjiao.WugEffect
{
	// Token: 0x02000377 RID: 887
	public class RedEyeGrowingGood1 : RedEyeBase
	{
		// Token: 0x060035AE RID: 13742 RVA: 0x0022D8E7 File Offset: 0x0022BAE7
		public RedEyeGrowingGood1()
		{
		}

		// Token: 0x060035AF RID: 13743 RVA: 0x0022D8F1 File Offset: 0x0022BAF1
		public RedEyeGrowingGood1(int charId) : base(charId, 12500, ItemDomain.GetWugTemplateId(0, 0), 1195)
		{
		}
	}
}
