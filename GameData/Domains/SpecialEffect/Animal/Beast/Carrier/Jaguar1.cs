using System;

namespace GameData.Domains.SpecialEffect.Animal.Beast.Carrier
{
	// Token: 0x0200062E RID: 1582
	public class Jaguar1 : JaguarBase
	{
		// Token: 0x17000304 RID: 772
		// (get) Token: 0x0600460E RID: 17934 RVA: 0x00273F21 File Offset: 0x00272121
		protected override short CombatStateId
		{
			get
			{
				return 163;
			}
		}

		// Token: 0x17000305 RID: 773
		// (get) Token: 0x0600460F RID: 17935 RVA: 0x00273F28 File Offset: 0x00272128
		protected override int FightBackPower
		{
			get
			{
				return 40;
			}
		}

		// Token: 0x06004610 RID: 17936 RVA: 0x00273F2C File Offset: 0x0027212C
		public Jaguar1()
		{
		}

		// Token: 0x06004611 RID: 17937 RVA: 0x00273F36 File Offset: 0x00272136
		public Jaguar1(int charId) : base(charId)
		{
		}
	}
}
