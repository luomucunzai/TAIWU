using System;
using GameData.Combat.Math;

namespace GameData.Domains.SpecialEffect.Animal.Beast.Carrier
{
	// Token: 0x0200062B RID: 1579
	public class Eagle1 : EagleBase
	{
		// Token: 0x170002FF RID: 767
		// (get) Token: 0x06004601 RID: 17921 RVA: 0x00273E43 File Offset: 0x00272043
		protected override short CombatStateId
		{
			get
			{
				return 158;
			}
		}

		// Token: 0x17000300 RID: 768
		// (get) Token: 0x06004602 RID: 17922 RVA: 0x00273E4A File Offset: 0x0027204A
		protected override CValuePercent AddCriticalOddsPercent
		{
			get
			{
				return 60;
			}
		}

		// Token: 0x06004603 RID: 17923 RVA: 0x00273E53 File Offset: 0x00272053
		public Eagle1()
		{
		}

		// Token: 0x06004604 RID: 17924 RVA: 0x00273E5D File Offset: 0x0027205D
		public Eagle1(int charId) : base(charId)
		{
		}
	}
}
