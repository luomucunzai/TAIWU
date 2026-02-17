using System;
using GameData.Combat.Math;

namespace GameData.Domains.SpecialEffect.Animal.Beast.Carrier
{
	// Token: 0x0200062A RID: 1578
	public class Eagle0 : EagleBase
	{
		// Token: 0x170002FD RID: 765
		// (get) Token: 0x060045FD RID: 17917 RVA: 0x00273E1E File Offset: 0x0027201E
		protected override short CombatStateId
		{
			get
			{
				return 149;
			}
		}

		// Token: 0x170002FE RID: 766
		// (get) Token: 0x060045FE RID: 17918 RVA: 0x00273E25 File Offset: 0x00272025
		protected override CValuePercent AddCriticalOddsPercent
		{
			get
			{
				return 40;
			}
		}

		// Token: 0x060045FF RID: 17919 RVA: 0x00273E2E File Offset: 0x0027202E
		public Eagle0()
		{
		}

		// Token: 0x06004600 RID: 17920 RVA: 0x00273E38 File Offset: 0x00272038
		public Eagle0(int charId) : base(charId)
		{
		}
	}
}
