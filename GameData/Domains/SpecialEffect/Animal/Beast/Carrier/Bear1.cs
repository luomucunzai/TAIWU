using System;

namespace GameData.Domains.SpecialEffect.Animal.Beast.Carrier
{
	// Token: 0x02000625 RID: 1573
	public class Bear1 : BearBase
	{
		// Token: 0x170002F5 RID: 757
		// (get) Token: 0x060045E6 RID: 17894 RVA: 0x00273C82 File Offset: 0x00271E82
		protected override short CombatStateId
		{
			get
			{
				return 160;
			}
		}

		// Token: 0x170002F6 RID: 758
		// (get) Token: 0x060045E7 RID: 17895 RVA: 0x00273C89 File Offset: 0x00271E89
		protected override int AddOrReduceDirectFatalDamagePercent
		{
			get
			{
				return 40;
			}
		}

		// Token: 0x060045E8 RID: 17896 RVA: 0x00273C8D File Offset: 0x00271E8D
		public Bear1()
		{
		}

		// Token: 0x060045E9 RID: 17897 RVA: 0x00273C97 File Offset: 0x00271E97
		public Bear1(int charId) : base(charId)
		{
		}
	}
}
