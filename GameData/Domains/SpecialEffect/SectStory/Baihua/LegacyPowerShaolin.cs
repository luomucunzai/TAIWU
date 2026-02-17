using System;

namespace GameData.Domains.SpecialEffect.SectStory.Baihua
{
	// Token: 0x020000F8 RID: 248
	public class LegacyPowerShaolin : LegacyPower
	{
		// Token: 0x170000F3 RID: 243
		// (get) Token: 0x0600299B RID: 10651 RVA: 0x002013BF File Offset: 0x001FF5BF
		protected override short CombatStateId
		{
			get
			{
				return 226;
			}
		}

		// Token: 0x170000F4 RID: 244
		// (get) Token: 0x0600299C RID: 10652 RVA: 0x002013C6 File Offset: 0x001FF5C6
		protected override sbyte OrgTemplateId
		{
			get
			{
				return 1;
			}
		}

		// Token: 0x0600299D RID: 10653 RVA: 0x002013C9 File Offset: 0x001FF5C9
		public LegacyPowerShaolin(int charId) : base(charId)
		{
		}
	}
}
