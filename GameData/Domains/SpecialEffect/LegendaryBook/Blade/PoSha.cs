using System;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.LegendaryBook.Common;
using GameData.Utilities;

namespace GameData.Domains.SpecialEffect.LegendaryBook.Blade
{
	// Token: 0x0200017C RID: 380
	public class PoSha : AddDamage
	{
		// Token: 0x06002B6A RID: 11114 RVA: 0x0020548E File Offset: 0x0020368E
		public PoSha()
		{
		}

		// Token: 0x06002B6B RID: 11115 RVA: 0x00205498 File Offset: 0x00203698
		public PoSha(CombatSkillKey skillKey) : base(skillKey, 40802)
		{
		}

		// Token: 0x06002B6C RID: 11116 RVA: 0x002054A8 File Offset: 0x002036A8
		protected override int GetAddDamagePercent()
		{
			int flawCount = base.CurrEnemyChar.GetFlawCount().Sum();
			return 20 * flawCount;
		}

		// Token: 0x04000D40 RID: 3392
		private const short AddDamageUnit = 20;
	}
}
