using System;

namespace GameData.Domains.SpecialEffect.Animal.Beast.Carrier
{
	// Token: 0x0200063D RID: 1597
	public class Tiger1 : TigerBase
	{
		// Token: 0x1700031D RID: 797
		// (get) Token: 0x06004651 RID: 18001 RVA: 0x002744BB File Offset: 0x002726BB
		protected override short CombatStateId
		{
			get
			{
				return 165;
			}
		}

		// Token: 0x1700031E RID: 798
		// (get) Token: 0x06004652 RID: 18002 RVA: 0x002744C2 File Offset: 0x002726C2
		protected override int AddDamagePercentUnit
		{
			get
			{
				return 40;
			}
		}

		// Token: 0x06004653 RID: 18003 RVA: 0x002744C6 File Offset: 0x002726C6
		public Tiger1()
		{
		}

		// Token: 0x06004654 RID: 18004 RVA: 0x002744D0 File Offset: 0x002726D0
		public Tiger1(int charId) : base(charId)
		{
		}
	}
}
