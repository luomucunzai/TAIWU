using System;
using GameData.Combat.Math;

namespace GameData.Domains.SpecialEffect.Animal.Beast.Carrier
{
	// Token: 0x02000636 RID: 1590
	public class Pig0 : PigBase
	{
		// Token: 0x17000311 RID: 785
		// (get) Token: 0x06004632 RID: 17970 RVA: 0x00274246 File Offset: 0x00272446
		protected override short CombatStateId
		{
			get
			{
				return 150;
			}
		}

		// Token: 0x17000312 RID: 786
		// (get) Token: 0x06004633 RID: 17971 RVA: 0x0027424D File Offset: 0x0027244D
		protected override CValuePercent AddCriticalOddsPercent
		{
			get
			{
				return 40;
			}
		}

		// Token: 0x06004634 RID: 17972 RVA: 0x00274256 File Offset: 0x00272456
		public Pig0()
		{
		}

		// Token: 0x06004635 RID: 17973 RVA: 0x00274260 File Offset: 0x00272460
		public Pig0(int charId) : base(charId)
		{
		}
	}
}
