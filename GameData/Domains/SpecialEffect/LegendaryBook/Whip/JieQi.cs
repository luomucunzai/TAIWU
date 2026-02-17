using System;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.LegendaryBook.Common;

namespace GameData.Domains.SpecialEffect.LegendaryBook.Whip
{
	// Token: 0x0200011A RID: 282
	public class JieQi : AddDamage
	{
		// Token: 0x06002A25 RID: 10789 RVA: 0x0020206D File Offset: 0x0020026D
		public JieQi()
		{
		}

		// Token: 0x06002A26 RID: 10790 RVA: 0x00202077 File Offset: 0x00200277
		public JieQi(CombatSkillKey skillKey) : base(skillKey, 41102)
		{
		}

		// Token: 0x06002A27 RID: 10791 RVA: 0x00202088 File Offset: 0x00200288
		protected override int GetAddDamagePercent()
		{
			return 180 * (30000 - base.CurrEnemyChar.GetBreathValue()) / 30000;
		}
	}
}
