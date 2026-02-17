using System;

namespace GameData.Domains.SpecialEffect.Animal.Beast.Carrier
{
	// Token: 0x02000627 RID: 1575
	public class Bull0 : BullBase
	{
		// Token: 0x170002F8 RID: 760
		// (get) Token: 0x060045F0 RID: 17904 RVA: 0x00273D5E File Offset: 0x00271F5E
		protected override short CombatStateId
		{
			get
			{
				return 152;
			}
		}

		// Token: 0x170002F9 RID: 761
		// (get) Token: 0x060045F1 RID: 17905 RVA: 0x00273D65 File Offset: 0x00271F65
		protected override int BouncePowerAddPercent
		{
			get
			{
				return 25;
			}
		}

		// Token: 0x060045F2 RID: 17906 RVA: 0x00273D69 File Offset: 0x00271F69
		public Bull0()
		{
		}

		// Token: 0x060045F3 RID: 17907 RVA: 0x00273D73 File Offset: 0x00271F73
		public Bull0(int charId) : base(charId)
		{
		}
	}
}
