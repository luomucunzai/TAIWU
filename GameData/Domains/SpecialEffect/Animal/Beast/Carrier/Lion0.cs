using System;

namespace GameData.Domains.SpecialEffect.Animal.Beast.Carrier
{
	// Token: 0x02000630 RID: 1584
	public class Lion0 : LionBase
	{
		// Token: 0x17000307 RID: 775
		// (get) Token: 0x06004617 RID: 17943 RVA: 0x00273FC2 File Offset: 0x002721C2
		protected override short CombatStateId
		{
			get
			{
				return 155;
			}
		}

		// Token: 0x17000308 RID: 776
		// (get) Token: 0x06004618 RID: 17944 RVA: 0x00273FC9 File Offset: 0x002721C9
		protected override int AddOrReduceCostPercent
		{
			get
			{
				return 25;
			}
		}

		// Token: 0x06004619 RID: 17945 RVA: 0x00273FCD File Offset: 0x002721CD
		public Lion0()
		{
		}

		// Token: 0x0600461A RID: 17946 RVA: 0x00273FD7 File Offset: 0x002721D7
		public Lion0(int charId) : base(charId)
		{
		}
	}
}
