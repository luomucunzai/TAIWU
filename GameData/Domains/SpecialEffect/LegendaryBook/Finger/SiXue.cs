using System;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.LegendaryBook.Common;
using GameData.Utilities;

namespace GameData.Domains.SpecialEffect.LegendaryBook.Finger
{
	// Token: 0x02000167 RID: 359
	public class SiXue : AddDamage
	{
		// Token: 0x06002B23 RID: 11043 RVA: 0x00204A91 File Offset: 0x00202C91
		public SiXue()
		{
		}

		// Token: 0x06002B24 RID: 11044 RVA: 0x00204A9B File Offset: 0x00202C9B
		public SiXue(CombatSkillKey skillKey) : base(skillKey, 40402)
		{
		}

		// Token: 0x06002B25 RID: 11045 RVA: 0x00204AAC File Offset: 0x00202CAC
		protected override int GetAddDamagePercent()
		{
			int acupointCount = base.CurrEnemyChar.GetAcupointCount().Sum();
			return 20 * acupointCount;
		}

		// Token: 0x04000D35 RID: 3381
		private const short AddDamageUnit = 20;
	}
}
