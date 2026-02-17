using System;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.LegendaryBook.Common;

namespace GameData.Domains.SpecialEffect.LegendaryBook.Whip
{
	// Token: 0x02000119 RID: 281
	public class JiePo : InterruptEnemyCast
	{
		// Token: 0x06002A23 RID: 10787 RVA: 0x00202053 File Offset: 0x00200253
		public JiePo()
		{
		}

		// Token: 0x06002A24 RID: 10788 RVA: 0x0020205D File Offset: 0x0020025D
		public JiePo(CombatSkillKey skillKey) : base(skillKey, 41103)
		{
		}
	}
}
