using System;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.LegendaryBook.Common;

namespace GameData.Domains.SpecialEffect.LegendaryBook.Finger
{
	// Token: 0x02000164 RID: 356
	public class JiePo : InterruptEnemyCast
	{
		// Token: 0x06002B1D RID: 11037 RVA: 0x00204A43 File Offset: 0x00202C43
		public JiePo()
		{
		}

		// Token: 0x06002B1E RID: 11038 RVA: 0x00204A4D File Offset: 0x00202C4D
		public JiePo(CombatSkillKey skillKey) : base(skillKey, 40403)
		{
		}
	}
}
