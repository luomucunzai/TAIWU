using System;

namespace GameData.Domains.SpecialEffect.SectStory.Baihua
{
	// Token: 0x02000105 RID: 261
	public class LegacyPowerFulong : LegacyPower
	{
		// Token: 0x1700010D RID: 269
		// (get) Token: 0x060029C2 RID: 10690 RVA: 0x002014D5 File Offset: 0x001FF6D5
		protected override short CombatStateId
		{
			get
			{
				return 239;
			}
		}

		// Token: 0x1700010E RID: 270
		// (get) Token: 0x060029C3 RID: 10691 RVA: 0x002014DC File Offset: 0x001FF6DC
		protected override sbyte OrgTemplateId
		{
			get
			{
				return 14;
			}
		}

		// Token: 0x060029C4 RID: 10692 RVA: 0x002014E0 File Offset: 0x001FF6E0
		public LegacyPowerFulong(int charId) : base(charId)
		{
		}
	}
}
