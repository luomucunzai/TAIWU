using System;

namespace GameData.Domains.SpecialEffect.SectStory.Baihua
{
	// Token: 0x02000101 RID: 257
	public class LegacyPowerKongsang : LegacyPower
	{
		// Token: 0x17000105 RID: 261
		// (get) Token: 0x060029B6 RID: 10678 RVA: 0x0020147D File Offset: 0x001FF67D
		protected override short CombatStateId
		{
			get
			{
				return 235;
			}
		}

		// Token: 0x17000106 RID: 262
		// (get) Token: 0x060029B7 RID: 10679 RVA: 0x00201484 File Offset: 0x001FF684
		protected override sbyte OrgTemplateId
		{
			get
			{
				return 10;
			}
		}

		// Token: 0x060029B8 RID: 10680 RVA: 0x00201488 File Offset: 0x001FF688
		public LegacyPowerKongsang(int charId) : base(charId)
		{
		}
	}
}
