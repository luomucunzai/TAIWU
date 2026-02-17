using System;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.LegendaryBook.Common;

namespace GameData.Domains.SpecialEffect.LegendaryBook.Special
{
	// Token: 0x02000132 RID: 306
	public class JiePo : InterruptEnemyCast
	{
		// Token: 0x06002A68 RID: 10856 RVA: 0x002028F4 File Offset: 0x00200AF4
		public JiePo()
		{
		}

		// Token: 0x06002A69 RID: 10857 RVA: 0x002028FE File Offset: 0x00200AFE
		public JiePo(CombatSkillKey skillKey) : base(skillKey, 41003)
		{
		}
	}
}
