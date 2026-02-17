using System;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.LegendaryBook.Common;

namespace GameData.Domains.SpecialEffect.LegendaryBook.ControllableShot
{
	// Token: 0x02000169 RID: 361
	public class BianJie : AddDamage
	{
		// Token: 0x06002B28 RID: 11048 RVA: 0x00204AED File Offset: 0x00202CED
		public BianJie()
		{
		}

		// Token: 0x06002B29 RID: 11049 RVA: 0x00204AF7 File Offset: 0x00202CF7
		public BianJie(CombatSkillKey skillKey) : base(skillKey, 41202)
		{
		}

		// Token: 0x06002B2A RID: 11050 RVA: 0x00204B08 File Offset: 0x00202D08
		protected override int GetAddDamagePercent()
		{
			return (int)(15 * base.CombatChar.GetChangeTrickCount());
		}

		// Token: 0x04000D36 RID: 3382
		private const short AddDamageUnit = 15;
	}
}
