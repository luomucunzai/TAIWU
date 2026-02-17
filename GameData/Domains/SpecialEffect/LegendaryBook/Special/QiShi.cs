using System;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.LegendaryBook.Common;

namespace GameData.Domains.SpecialEffect.LegendaryBook.Special
{
	// Token: 0x02000134 RID: 308
	public class QiShi : AddDamage
	{
		// Token: 0x06002A6C RID: 10860 RVA: 0x00202928 File Offset: 0x00200B28
		public QiShi()
		{
		}

		// Token: 0x06002A6D RID: 10861 RVA: 0x00202932 File Offset: 0x00200B32
		public QiShi(CombatSkillKey skillKey) : base(skillKey, 41002)
		{
		}

		// Token: 0x06002A6E RID: 10862 RVA: 0x00202944 File Offset: 0x00200B44
		protected override int GetAddDamagePercent()
		{
			return 180 * (4000 - base.CurrEnemyChar.GetStanceValue()) / 4000;
		}
	}
}
