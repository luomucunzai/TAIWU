using System;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.LegendaryBook.Common;

namespace GameData.Domains.SpecialEffect.LegendaryBook.Polearm
{
	// Token: 0x0200013E RID: 318
	public class JiePo : InterruptEnemyCast
	{
		// Token: 0x06002A8C RID: 10892 RVA: 0x00202C8B File Offset: 0x00200E8B
		public JiePo()
		{
		}

		// Token: 0x06002A8D RID: 10893 RVA: 0x00202C95 File Offset: 0x00200E95
		public JiePo(CombatSkillKey skillKey) : base(skillKey, 40903)
		{
		}
	}
}
