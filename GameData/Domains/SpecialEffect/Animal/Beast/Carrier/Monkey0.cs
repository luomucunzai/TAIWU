using System;

namespace GameData.Domains.SpecialEffect.Animal.Beast.Carrier
{
	// Token: 0x02000633 RID: 1587
	public class Monkey0 : MonkeyBase
	{
		// Token: 0x1700030C RID: 780
		// (get) Token: 0x06004625 RID: 17957 RVA: 0x0027412D File Offset: 0x0027232D
		protected override short CombatStateId
		{
			get
			{
				return 148;
			}
		}

		// Token: 0x1700030D RID: 781
		// (get) Token: 0x06004626 RID: 17958 RVA: 0x00274134 File Offset: 0x00272334
		protected override int PowerAddOrReduceRatio
		{
			get
			{
				return 25;
			}
		}

		// Token: 0x06004627 RID: 17959 RVA: 0x00274138 File Offset: 0x00272338
		public Monkey0()
		{
		}

		// Token: 0x06004628 RID: 17960 RVA: 0x00274142 File Offset: 0x00272342
		public Monkey0(int charId) : base(charId)
		{
		}
	}
}
