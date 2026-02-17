using System;

namespace GameData.Domains.SpecialEffect.Animal.Beast.Carrier
{
	// Token: 0x0200062D RID: 1581
	public class Jaguar0 : JaguarBase
	{
		// Token: 0x17000302 RID: 770
		// (get) Token: 0x0600460A RID: 17930 RVA: 0x00273F01 File Offset: 0x00272101
		protected override short CombatStateId
		{
			get
			{
				return 154;
			}
		}

		// Token: 0x17000303 RID: 771
		// (get) Token: 0x0600460B RID: 17931 RVA: 0x00273F08 File Offset: 0x00272108
		protected override int FightBackPower
		{
			get
			{
				return 25;
			}
		}

		// Token: 0x0600460C RID: 17932 RVA: 0x00273F0C File Offset: 0x0027210C
		public Jaguar0()
		{
		}

		// Token: 0x0600460D RID: 17933 RVA: 0x00273F16 File Offset: 0x00272116
		public Jaguar0(int charId) : base(charId)
		{
		}
	}
}
