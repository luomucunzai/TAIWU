using System;

namespace GameData.Domains.SpecialEffect.SectStory.Baihua
{
	// Token: 0x02000102 RID: 258
	public class LegacyPowerJingang : LegacyPower
	{
		// Token: 0x17000107 RID: 263
		// (get) Token: 0x060029B9 RID: 10681 RVA: 0x00201493 File Offset: 0x001FF693
		protected override short CombatStateId
		{
			get
			{
				return 236;
			}
		}

		// Token: 0x17000108 RID: 264
		// (get) Token: 0x060029BA RID: 10682 RVA: 0x0020149A File Offset: 0x001FF69A
		protected override sbyte OrgTemplateId
		{
			get
			{
				return 11;
			}
		}

		// Token: 0x060029BB RID: 10683 RVA: 0x0020149E File Offset: 0x001FF69E
		public LegacyPowerJingang(int charId) : base(charId)
		{
		}
	}
}
