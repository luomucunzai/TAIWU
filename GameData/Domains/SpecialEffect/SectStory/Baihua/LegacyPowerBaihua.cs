using System;

namespace GameData.Domains.SpecialEffect.SectStory.Baihua
{
	// Token: 0x020000FA RID: 250
	public class LegacyPowerBaihua : LegacyPower
	{
		// Token: 0x170000F7 RID: 247
		// (get) Token: 0x060029A1 RID: 10657 RVA: 0x002013E9 File Offset: 0x001FF5E9
		protected override short CombatStateId
		{
			get
			{
				return 228;
			}
		}

		// Token: 0x170000F8 RID: 248
		// (get) Token: 0x060029A2 RID: 10658 RVA: 0x002013F0 File Offset: 0x001FF5F0
		protected override sbyte OrgTemplateId
		{
			get
			{
				return 3;
			}
		}

		// Token: 0x060029A3 RID: 10659 RVA: 0x002013F3 File Offset: 0x001FF5F3
		public LegacyPowerBaihua(int charId) : base(charId)
		{
		}
	}
}
