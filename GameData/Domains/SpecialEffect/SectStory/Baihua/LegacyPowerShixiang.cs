using System;

namespace GameData.Domains.SpecialEffect.SectStory.Baihua
{
	// Token: 0x020000FD RID: 253
	public class LegacyPowerShixiang : LegacyPower
	{
		// Token: 0x170000FD RID: 253
		// (get) Token: 0x060029AA RID: 10666 RVA: 0x00201428 File Offset: 0x001FF628
		protected override short CombatStateId
		{
			get
			{
				return 231;
			}
		}

		// Token: 0x170000FE RID: 254
		// (get) Token: 0x060029AB RID: 10667 RVA: 0x0020142F File Offset: 0x001FF62F
		protected override sbyte OrgTemplateId
		{
			get
			{
				return 6;
			}
		}

		// Token: 0x060029AC RID: 10668 RVA: 0x00201432 File Offset: 0x001FF632
		public LegacyPowerShixiang(int charId) : base(charId)
		{
		}
	}
}
