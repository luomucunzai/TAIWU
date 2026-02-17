using System;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.LegendaryBook.Common;

namespace GameData.Domains.SpecialEffect.LegendaryBook.Throw
{
	// Token: 0x02000123 RID: 291
	public class YuanSha : AddDamage
	{
		// Token: 0x06002A3C RID: 10812 RVA: 0x00202288 File Offset: 0x00200488
		public YuanSha()
		{
		}

		// Token: 0x06002A3D RID: 10813 RVA: 0x00202292 File Offset: 0x00200492
		public YuanSha(CombatSkillKey skillKey) : base(skillKey, 40602)
		{
		}

		// Token: 0x06002A3E RID: 10814 RVA: 0x002022A4 File Offset: 0x002004A4
		protected override int GetAddDamagePercent()
		{
			short distance = DomainManager.Combat.GetCurrentDistance();
			return (int)((distance < 50) ? 0 : (40 + (distance - 50) / 10 * 20));
		}

		// Token: 0x04000CEB RID: 3307
		private const short MinDistance = 50;

		// Token: 0x04000CEC RID: 3308
		private const short BaseAddDamage = 40;

		// Token: 0x04000CED RID: 3309
		private const short AddDamageUnit = 20;
	}
}
