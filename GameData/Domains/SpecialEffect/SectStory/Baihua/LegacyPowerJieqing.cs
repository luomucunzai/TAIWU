using System;

namespace GameData.Domains.SpecialEffect.SectStory.Baihua
{
	// Token: 0x02000104 RID: 260
	public class LegacyPowerJieqing : LegacyPower
	{
		// Token: 0x1700010B RID: 267
		// (get) Token: 0x060029BF RID: 10687 RVA: 0x002014BF File Offset: 0x001FF6BF
		protected override short CombatStateId
		{
			get
			{
				return 238;
			}
		}

		// Token: 0x1700010C RID: 268
		// (get) Token: 0x060029C0 RID: 10688 RVA: 0x002014C6 File Offset: 0x001FF6C6
		protected override sbyte OrgTemplateId
		{
			get
			{
				return 13;
			}
		}

		// Token: 0x060029C1 RID: 10689 RVA: 0x002014CA File Offset: 0x001FF6CA
		public LegacyPowerJieqing(int charId) : base(charId)
		{
		}
	}
}
