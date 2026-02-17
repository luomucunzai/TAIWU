using System;

namespace GameData.Domains.SpecialEffect.Animal.Beast.Carrier
{
	// Token: 0x02000639 RID: 1593
	public class Snake0 : SnakeBase
	{
		// Token: 0x17000316 RID: 790
		// (get) Token: 0x0600463F RID: 17983 RVA: 0x0027432F File Offset: 0x0027252F
		protected override short CombatStateId
		{
			get
			{
				return 153;
			}
		}

		// Token: 0x17000317 RID: 791
		// (get) Token: 0x06004640 RID: 17984 RVA: 0x00274336 File Offset: 0x00272536
		protected override int ChangeHealEffect
		{
			get
			{
				return 25;
			}
		}

		// Token: 0x06004641 RID: 17985 RVA: 0x0027433A File Offset: 0x0027253A
		public Snake0()
		{
		}

		// Token: 0x06004642 RID: 17986 RVA: 0x00274344 File Offset: 0x00272544
		public Snake0(int charId) : base(charId)
		{
		}
	}
}
