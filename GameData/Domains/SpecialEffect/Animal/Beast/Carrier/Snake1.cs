using System;

namespace GameData.Domains.SpecialEffect.Animal.Beast.Carrier
{
	// Token: 0x0200063A RID: 1594
	public class Snake1 : SnakeBase
	{
		// Token: 0x17000318 RID: 792
		// (get) Token: 0x06004643 RID: 17987 RVA: 0x0027434F File Offset: 0x0027254F
		protected override short CombatStateId
		{
			get
			{
				return 162;
			}
		}

		// Token: 0x17000319 RID: 793
		// (get) Token: 0x06004644 RID: 17988 RVA: 0x00274356 File Offset: 0x00272556
		protected override int ChangeHealEffect
		{
			get
			{
				return 40;
			}
		}

		// Token: 0x06004645 RID: 17989 RVA: 0x0027435A File Offset: 0x0027255A
		public Snake1()
		{
		}

		// Token: 0x06004646 RID: 17990 RVA: 0x00274364 File Offset: 0x00272564
		public Snake1(int charId) : base(charId)
		{
		}
	}
}
