using System;
using GameData.Domains.Item;

namespace GameData.Domains.SpecialEffect.CombatSkill.Wuxianjiao.WugEffect
{
	// Token: 0x0200037A RID: 890
	public class RedEyeKing : RedEyeBase
	{
		// Token: 0x060035B4 RID: 13748 RVA: 0x0022D959 File Offset: 0x0022BB59
		public RedEyeKing()
		{
		}

		// Token: 0x060035B5 RID: 13749 RVA: 0x0022D963 File Offset: 0x0022BB63
		public RedEyeKing(int charId) : base(charId, 12540, ItemDomain.GetWugTemplateId(0, 5), 469)
		{
		}
	}
}
