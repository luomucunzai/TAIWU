using System;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.LegendaryBook.Common;

namespace GameData.Domains.SpecialEffect.LegendaryBook.Throw
{
	// Token: 0x02000120 RID: 288
	public class JiePo : InterruptEnemyCast
	{
		// Token: 0x06002A36 RID: 10806 RVA: 0x0020223A File Offset: 0x0020043A
		public JiePo()
		{
		}

		// Token: 0x06002A37 RID: 10807 RVA: 0x00202244 File Offset: 0x00200444
		public JiePo(CombatSkillKey skillKey) : base(skillKey, 40603)
		{
		}
	}
}
