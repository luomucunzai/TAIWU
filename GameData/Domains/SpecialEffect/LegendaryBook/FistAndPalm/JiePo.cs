using System;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.LegendaryBook.Common;

namespace GameData.Domains.SpecialEffect.LegendaryBook.FistAndPalm
{
	// Token: 0x0200015D RID: 349
	public class JiePo : InterruptEnemyCast
	{
		// Token: 0x06002B08 RID: 11016 RVA: 0x00204842 File Offset: 0x00202A42
		public JiePo()
		{
		}

		// Token: 0x06002B09 RID: 11017 RVA: 0x0020484C File Offset: 0x00202A4C
		public JiePo(CombatSkillKey skillKey) : base(skillKey, 40303)
		{
		}
	}
}
