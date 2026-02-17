using System;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.LegendaryBook.Common;

namespace GameData.Domains.SpecialEffect.LegendaryBook.Leg
{
	// Token: 0x02000158 RID: 344
	public class JiePo : InterruptEnemyCast
	{
		// Token: 0x06002AFD RID: 11005 RVA: 0x0020477F File Offset: 0x0020297F
		public JiePo()
		{
		}

		// Token: 0x06002AFE RID: 11006 RVA: 0x00204789 File Offset: 0x00202989
		public JiePo(CombatSkillKey skillKey) : base(skillKey, 40503)
		{
		}
	}
}
