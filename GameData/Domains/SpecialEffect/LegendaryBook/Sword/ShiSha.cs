using System;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.LegendaryBook.Common;

namespace GameData.Domains.SpecialEffect.LegendaryBook.Sword
{
	// Token: 0x02000128 RID: 296
	public class ShiSha : AddDamage
	{
		// Token: 0x06002A49 RID: 10825 RVA: 0x002023B2 File Offset: 0x002005B2
		public ShiSha()
		{
		}

		// Token: 0x06002A4A RID: 10826 RVA: 0x002023BC File Offset: 0x002005BC
		public ShiSha(CombatSkillKey skillKey) : base(skillKey, 40702)
		{
		}

		// Token: 0x06002A4B RID: 10827 RVA: 0x002023CC File Offset: 0x002005CC
		protected override int GetAddDamagePercent()
		{
			int trickCount = base.EnemyChar.UsableTrickCount;
			return 20 * (9 - trickCount);
		}

		// Token: 0x04000CEF RID: 3311
		private const short AddDamageUnit = 20;
	}
}
