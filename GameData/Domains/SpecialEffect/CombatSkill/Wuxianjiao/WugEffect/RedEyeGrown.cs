using System;
using GameData.Domains.Item;

namespace GameData.Domains.SpecialEffect.CombatSkill.Wuxianjiao.WugEffect
{
	// Token: 0x02000379 RID: 889
	public class RedEyeGrown : RedEyeBase
	{
		// Token: 0x060035B2 RID: 13746 RVA: 0x0022D933 File Offset: 0x0022BB33
		public RedEyeGrown()
		{
		}

		// Token: 0x060035B3 RID: 13747 RVA: 0x0022D93D File Offset: 0x0022BB3D
		public RedEyeGrown(int charId) : base(charId, 12504, ItemDomain.GetWugTemplateId(0, 4), 469)
		{
		}
	}
}
