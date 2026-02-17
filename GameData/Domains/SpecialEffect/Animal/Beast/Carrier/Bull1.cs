using System;

namespace GameData.Domains.SpecialEffect.Animal.Beast.Carrier
{
	// Token: 0x02000628 RID: 1576
	public class Bull1 : BullBase
	{
		// Token: 0x170002FA RID: 762
		// (get) Token: 0x060045F4 RID: 17908 RVA: 0x00273D7E File Offset: 0x00271F7E
		protected override short CombatStateId
		{
			get
			{
				return 161;
			}
		}

		// Token: 0x170002FB RID: 763
		// (get) Token: 0x060045F5 RID: 17909 RVA: 0x00273D85 File Offset: 0x00271F85
		protected override int BouncePowerAddPercent
		{
			get
			{
				return 40;
			}
		}

		// Token: 0x060045F6 RID: 17910 RVA: 0x00273D89 File Offset: 0x00271F89
		public Bull1()
		{
		}

		// Token: 0x060045F7 RID: 17911 RVA: 0x00273D93 File Offset: 0x00271F93
		public Bull1(int charId) : base(charId)
		{
		}
	}
}
