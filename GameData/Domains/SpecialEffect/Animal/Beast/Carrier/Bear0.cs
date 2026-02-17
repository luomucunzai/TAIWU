using System;

namespace GameData.Domains.SpecialEffect.Animal.Beast.Carrier
{
	// Token: 0x02000624 RID: 1572
	public class Bear0 : BearBase
	{
		// Token: 0x170002F3 RID: 755
		// (get) Token: 0x060045E2 RID: 17890 RVA: 0x00273C62 File Offset: 0x00271E62
		protected override short CombatStateId
		{
			get
			{
				return 151;
			}
		}

		// Token: 0x170002F4 RID: 756
		// (get) Token: 0x060045E3 RID: 17891 RVA: 0x00273C69 File Offset: 0x00271E69
		protected override int AddOrReduceDirectFatalDamagePercent
		{
			get
			{
				return 25;
			}
		}

		// Token: 0x060045E4 RID: 17892 RVA: 0x00273C6D File Offset: 0x00271E6D
		public Bear0()
		{
		}

		// Token: 0x060045E5 RID: 17893 RVA: 0x00273C77 File Offset: 0x00271E77
		public Bear0(int charId) : base(charId)
		{
		}
	}
}
