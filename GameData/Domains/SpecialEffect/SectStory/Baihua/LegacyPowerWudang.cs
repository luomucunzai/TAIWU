using System;

namespace GameData.Domains.SpecialEffect.SectStory.Baihua
{
	// Token: 0x020000FB RID: 251
	public class LegacyPowerWudang : LegacyPower
	{
		// Token: 0x170000F9 RID: 249
		// (get) Token: 0x060029A4 RID: 10660 RVA: 0x002013FE File Offset: 0x001FF5FE
		protected override short CombatStateId
		{
			get
			{
				return 229;
			}
		}

		// Token: 0x170000FA RID: 250
		// (get) Token: 0x060029A5 RID: 10661 RVA: 0x00201405 File Offset: 0x001FF605
		protected override sbyte OrgTemplateId
		{
			get
			{
				return 4;
			}
		}

		// Token: 0x060029A6 RID: 10662 RVA: 0x00201408 File Offset: 0x001FF608
		public LegacyPowerWudang(int charId) : base(charId)
		{
		}
	}
}
