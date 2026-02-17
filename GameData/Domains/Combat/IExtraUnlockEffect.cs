using System;
using GameData.Common;

namespace GameData.Domains.Combat
{
	// Token: 0x020006C5 RID: 1733
	public interface IExtraUnlockEffect
	{
		// Token: 0x17000402 RID: 1026
		// (get) Token: 0x060066DD RID: 26333
		bool IsDirect { get; }

		// Token: 0x060066DE RID: 26334
		void DoAffectAfterCost(DataContext context, int weaponIndex);
	}
}
