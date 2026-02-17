using System;

namespace GameData.Domains.SpecialEffect.SectStory.Baihua
{
	// Token: 0x020000FC RID: 252
	public class LegacyPowerYuanshan : LegacyPower
	{
		// Token: 0x170000FB RID: 251
		// (get) Token: 0x060029A7 RID: 10663 RVA: 0x00201413 File Offset: 0x001FF613
		protected override short CombatStateId
		{
			get
			{
				return 230;
			}
		}

		// Token: 0x170000FC RID: 252
		// (get) Token: 0x060029A8 RID: 10664 RVA: 0x0020141A File Offset: 0x001FF61A
		protected override sbyte OrgTemplateId
		{
			get
			{
				return 5;
			}
		}

		// Token: 0x060029A9 RID: 10665 RVA: 0x0020141D File Offset: 0x001FF61D
		public LegacyPowerYuanshan(int charId) : base(charId)
		{
		}
	}
}
