using System;

namespace GameData.Domains.SpecialEffect.Animal.Beast.Carrier
{
	// Token: 0x02000631 RID: 1585
	public class Lion1 : LionBase
	{
		// Token: 0x17000309 RID: 777
		// (get) Token: 0x0600461B RID: 17947 RVA: 0x00273FE2 File Offset: 0x002721E2
		protected override short CombatStateId
		{
			get
			{
				return 164;
			}
		}

		// Token: 0x1700030A RID: 778
		// (get) Token: 0x0600461C RID: 17948 RVA: 0x00273FE9 File Offset: 0x002721E9
		protected override int AddOrReduceCostPercent
		{
			get
			{
				return 40;
			}
		}

		// Token: 0x0600461D RID: 17949 RVA: 0x00273FED File Offset: 0x002721ED
		public Lion1()
		{
		}

		// Token: 0x0600461E RID: 17950 RVA: 0x00273FF7 File Offset: 0x002721F7
		public Lion1(int charId) : base(charId)
		{
		}
	}
}
