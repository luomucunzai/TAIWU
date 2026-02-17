using System;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.LegendaryBook.Common;

namespace GameData.Domains.SpecialEffect.LegendaryBook.Leg
{
	// Token: 0x0200015B RID: 347
	public class XianSha : AddDamage
	{
		// Token: 0x06002B03 RID: 11011 RVA: 0x002047CD File Offset: 0x002029CD
		public XianSha()
		{
		}

		// Token: 0x06002B04 RID: 11012 RVA: 0x002047D7 File Offset: 0x002029D7
		public XianSha(CombatSkillKey skillKey) : base(skillKey, 40502)
		{
		}

		// Token: 0x06002B05 RID: 11013 RVA: 0x002047E8 File Offset: 0x002029E8
		protected override int GetAddDamagePercent()
		{
			int mobilityPercent = base.CurrEnemyChar.GetMobilityValue() * 100 / MoveSpecialConstants.MaxMobility;
			return (mobilityPercent < 50) ? 180 : (180 * (100 - mobilityPercent) / 50);
		}

		// Token: 0x04000D2E RID: 3374
		private const short MaxDamageMobilityPercent = 50;
	}
}
