using System;
using GameData.Domains.Item;

namespace GameData.Domains.SpecialEffect.CombatSkill.Wuxianjiao.WugEffect
{
	// Token: 0x0200035D RID: 861
	public class DevilInsideGrown : DevilInsideBase
	{
		// Token: 0x0600354A RID: 13642 RVA: 0x0022C205 File Offset: 0x0022A405
		public DevilInsideGrown()
		{
		}

		// Token: 0x0600354B RID: 13643 RVA: 0x0022C20F File Offset: 0x0022A40F
		public DevilInsideGrown(int charId) : base(charId, 12519, ItemDomain.GetWugTemplateId(3, 4), 472)
		{
		}
	}
}
