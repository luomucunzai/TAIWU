using System;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.LegendaryBook.Common;

namespace GameData.Domains.SpecialEffect.LegendaryBook.ControllableShot
{
	// Token: 0x0200016A RID: 362
	public class JiePo : InterruptEnemyCast
	{
		// Token: 0x06002B2B RID: 11051 RVA: 0x00204B28 File Offset: 0x00202D28
		public JiePo()
		{
		}

		// Token: 0x06002B2C RID: 11052 RVA: 0x00204B32 File Offset: 0x00202D32
		public JiePo(CombatSkillKey skillKey) : base(skillKey, 41203)
		{
		}
	}
}
