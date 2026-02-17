using System;

namespace GameData.Domains.SpecialEffect.Animal.Beast.Carrier
{
	// Token: 0x0200063C RID: 1596
	public class Tiger0 : TigerBase
	{
		// Token: 0x1700031B RID: 795
		// (get) Token: 0x0600464D RID: 17997 RVA: 0x0027449B File Offset: 0x0027269B
		protected override short CombatStateId
		{
			get
			{
				return 156;
			}
		}

		// Token: 0x1700031C RID: 796
		// (get) Token: 0x0600464E RID: 17998 RVA: 0x002744A2 File Offset: 0x002726A2
		protected override int AddDamagePercentUnit
		{
			get
			{
				return 20;
			}
		}

		// Token: 0x0600464F RID: 17999 RVA: 0x002744A6 File Offset: 0x002726A6
		public Tiger0()
		{
		}

		// Token: 0x06004650 RID: 18000 RVA: 0x002744B0 File Offset: 0x002726B0
		public Tiger0(int charId) : base(charId)
		{
		}
	}
}
