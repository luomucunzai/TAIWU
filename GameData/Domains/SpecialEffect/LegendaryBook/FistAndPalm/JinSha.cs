using System;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.LegendaryBook.Common;

namespace GameData.Domains.SpecialEffect.LegendaryBook.FistAndPalm
{
	// Token: 0x0200015E RID: 350
	public class JinSha : AddDamage
	{
		// Token: 0x06002B0A RID: 11018 RVA: 0x0020485C File Offset: 0x00202A5C
		public JinSha()
		{
		}

		// Token: 0x06002B0B RID: 11019 RVA: 0x00204866 File Offset: 0x00202A66
		public JinSha(CombatSkillKey skillKey) : base(skillKey, 40302)
		{
		}

		// Token: 0x06002B0C RID: 11020 RVA: 0x00204878 File Offset: 0x00202A78
		protected override int GetAddDamagePercent()
		{
			short distance = DomainManager.Combat.GetCurrentDistance();
			return (int)((distance > 50) ? 0 : (60 + (50 - distance) / 10 * 40));
		}

		// Token: 0x04000D2F RID: 3375
		private const short MaxDistance = 50;

		// Token: 0x04000D30 RID: 3376
		private const short BaseAddDamage = 60;

		// Token: 0x04000D31 RID: 3377
		private const short AddDamageUnit = 40;
	}
}
