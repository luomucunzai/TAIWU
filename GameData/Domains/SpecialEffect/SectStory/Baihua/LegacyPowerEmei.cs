using System;

namespace GameData.Domains.SpecialEffect.SectStory.Baihua
{
	// Token: 0x020000F9 RID: 249
	public class LegacyPowerEmei : LegacyPower
	{
		// Token: 0x170000F5 RID: 245
		// (get) Token: 0x0600299E RID: 10654 RVA: 0x002013D4 File Offset: 0x001FF5D4
		protected override short CombatStateId
		{
			get
			{
				return 227;
			}
		}

		// Token: 0x170000F6 RID: 246
		// (get) Token: 0x0600299F RID: 10655 RVA: 0x002013DB File Offset: 0x001FF5DB
		protected override sbyte OrgTemplateId
		{
			get
			{
				return 2;
			}
		}

		// Token: 0x060029A0 RID: 10656 RVA: 0x002013DE File Offset: 0x001FF5DE
		public LegacyPowerEmei(int charId) : base(charId)
		{
		}
	}
}
