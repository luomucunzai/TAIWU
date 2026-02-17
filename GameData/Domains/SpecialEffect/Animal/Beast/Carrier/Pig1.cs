using System;
using GameData.Combat.Math;

namespace GameData.Domains.SpecialEffect.Animal.Beast.Carrier
{
	// Token: 0x02000637 RID: 1591
	public class Pig1 : PigBase
	{
		// Token: 0x17000313 RID: 787
		// (get) Token: 0x06004636 RID: 17974 RVA: 0x0027426B File Offset: 0x0027246B
		protected override short CombatStateId
		{
			get
			{
				return 159;
			}
		}

		// Token: 0x17000314 RID: 788
		// (get) Token: 0x06004637 RID: 17975 RVA: 0x00274272 File Offset: 0x00272472
		protected override CValuePercent AddCriticalOddsPercent
		{
			get
			{
				return 60;
			}
		}

		// Token: 0x06004638 RID: 17976 RVA: 0x0027427B File Offset: 0x0027247B
		public Pig1()
		{
		}

		// Token: 0x06004639 RID: 17977 RVA: 0x00274285 File Offset: 0x00272485
		public Pig1(int charId) : base(charId)
		{
		}
	}
}
