using System;

namespace GameData.Domains.SpecialEffect.SectStory.Baihua
{
	// Token: 0x02000106 RID: 262
	public class LegacyPowerXuehou : LegacyPower
	{
		// Token: 0x1700010F RID: 271
		// (get) Token: 0x060029C5 RID: 10693 RVA: 0x002014EB File Offset: 0x001FF6EB
		protected override short CombatStateId
		{
			get
			{
				return 240;
			}
		}

		// Token: 0x17000110 RID: 272
		// (get) Token: 0x060029C6 RID: 10694 RVA: 0x002014F2 File Offset: 0x001FF6F2
		protected override sbyte OrgTemplateId
		{
			get
			{
				return 15;
			}
		}

		// Token: 0x060029C7 RID: 10695 RVA: 0x002014F6 File Offset: 0x001FF6F6
		public LegacyPowerXuehou(int charId) : base(charId)
		{
		}
	}
}
