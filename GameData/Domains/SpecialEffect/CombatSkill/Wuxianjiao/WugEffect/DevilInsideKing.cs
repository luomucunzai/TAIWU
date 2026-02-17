using System;
using GameData.Domains.Item;

namespace GameData.Domains.SpecialEffect.CombatSkill.Wuxianjiao.WugEffect
{
	// Token: 0x0200035E RID: 862
	public class DevilInsideKing : DevilInsideBase
	{
		// Token: 0x0600354C RID: 13644 RVA: 0x0022C22B File Offset: 0x0022A42B
		public DevilInsideKing()
		{
		}

		// Token: 0x0600354D RID: 13645 RVA: 0x0022C235 File Offset: 0x0022A435
		public DevilInsideKing(int charId) : base(charId, 12543, ItemDomain.GetWugTemplateId(3, 5), 472)
		{
		}
	}
}
