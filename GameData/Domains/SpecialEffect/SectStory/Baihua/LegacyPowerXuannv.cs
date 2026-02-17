using System;

namespace GameData.Domains.SpecialEffect.SectStory.Baihua
{
	// Token: 0x020000FF RID: 255
	public class LegacyPowerXuannv : LegacyPower
	{
		// Token: 0x17000101 RID: 257
		// (get) Token: 0x060029B0 RID: 10672 RVA: 0x00201452 File Offset: 0x001FF652
		protected override short CombatStateId
		{
			get
			{
				return 233;
			}
		}

		// Token: 0x17000102 RID: 258
		// (get) Token: 0x060029B1 RID: 10673 RVA: 0x00201459 File Offset: 0x001FF659
		protected override sbyte OrgTemplateId
		{
			get
			{
				return 8;
			}
		}

		// Token: 0x060029B2 RID: 10674 RVA: 0x0020145C File Offset: 0x001FF65C
		public LegacyPowerXuannv(int charId) : base(charId)
		{
		}
	}
}
