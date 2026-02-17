using System;

namespace GameData.Domains.SpecialEffect.SectStory.Baihua
{
	// Token: 0x02000103 RID: 259
	public class LegacyPowerWuxian : LegacyPower
	{
		// Token: 0x17000109 RID: 265
		// (get) Token: 0x060029BC RID: 10684 RVA: 0x002014A9 File Offset: 0x001FF6A9
		protected override short CombatStateId
		{
			get
			{
				return 237;
			}
		}

		// Token: 0x1700010A RID: 266
		// (get) Token: 0x060029BD RID: 10685 RVA: 0x002014B0 File Offset: 0x001FF6B0
		protected override sbyte OrgTemplateId
		{
			get
			{
				return 12;
			}
		}

		// Token: 0x060029BE RID: 10686 RVA: 0x002014B4 File Offset: 0x001FF6B4
		public LegacyPowerWuxian(int charId) : base(charId)
		{
		}
	}
}
