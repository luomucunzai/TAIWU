using System;

namespace GameData.Domains.Combat
{
	// Token: 0x020006FE RID: 1790
	public static class TeammateCommandImplementExtensions
	{
		// Token: 0x060067D3 RID: 26579 RVA: 0x003B2590 File Offset: 0x003B0790
		public static bool IsPushOrPull(this ETeammateCommandImplement implement)
		{
			return implement - ETeammateCommandImplement.Push <= 1 || implement == ETeammateCommandImplement.PushOrPullIntoDanger;
		}

		// Token: 0x060067D4 RID: 26580 RVA: 0x003B25B4 File Offset: 0x003B07B4
		public static bool IsAttack(this ETeammateCommandImplement implement)
		{
			return implement == ETeammateCommandImplement.Attack || implement == ETeammateCommandImplement.GearMateA;
		}

		// Token: 0x060067D5 RID: 26581 RVA: 0x003B25D8 File Offset: 0x003B07D8
		public static bool IsDefend(this ETeammateCommandImplement implement)
		{
			return implement == ETeammateCommandImplement.Defend || implement == ETeammateCommandImplement.GearMateB;
		}
	}
}
