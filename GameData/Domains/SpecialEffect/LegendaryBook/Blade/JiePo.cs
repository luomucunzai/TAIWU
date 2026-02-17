using System;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.LegendaryBook.Common;

namespace GameData.Domains.SpecialEffect.LegendaryBook.Blade
{
	// Token: 0x0200017A RID: 378
	public class JiePo : InterruptEnemyCast
	{
		// Token: 0x06002B66 RID: 11110 RVA: 0x0020545A File Offset: 0x0020365A
		public JiePo()
		{
		}

		// Token: 0x06002B67 RID: 11111 RVA: 0x00205464 File Offset: 0x00203664
		public JiePo(CombatSkillKey skillKey) : base(skillKey, 40803)
		{
		}
	}
}
