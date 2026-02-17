using System;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.LegendaryBook.Common;

namespace GameData.Domains.SpecialEffect.LegendaryBook.CombatMusic
{
	// Token: 0x02000176 RID: 374
	public class KuangSheng : AddDamage
	{
		// Token: 0x06002B5A RID: 11098 RVA: 0x002052DF File Offset: 0x002034DF
		public KuangSheng()
		{
		}

		// Token: 0x06002B5B RID: 11099 RVA: 0x002052E9 File Offset: 0x002034E9
		public KuangSheng(CombatSkillKey skillKey) : base(skillKey, 41302)
		{
		}

		// Token: 0x06002B5C RID: 11100 RVA: 0x002052FC File Offset: 0x002034FC
		protected override int GetAddDamagePercent()
		{
			return (int)(30 * base.CombatChar.GetTrickCount(9));
		}

		// Token: 0x04000D3F RID: 3391
		private const short AddDamageUnit = 30;
	}
}
