using System;
using GameData.Combat.Math;

namespace GameData.Domains.Combat
{
	// Token: 0x020006AB RID: 1707
	public static class WorsenConstants
	{
		// Token: 0x0600628A RID: 25226 RVA: 0x003816CF File Offset: 0x0037F8CF
		public static CValuePercent CalcPoisonPercent(CValueMultiplier poisonLevel)
		{
			return WorsenConstants.HighPercent * poisonLevel;
		}

		// Token: 0x04001AE9 RID: 6889
		public static readonly CValuePercent[] WorsenFatalPercent = new CValuePercent[]
		{
			10,
			20,
			40,
			70,
			110,
			160
		};

		// Token: 0x04001AEA RID: 6890
		public static readonly CValuePercent DefaultPercent = 80;

		// Token: 0x04001AEB RID: 6891
		public static readonly CValuePercent LowPercent = 40;

		// Token: 0x04001AEC RID: 6892
		public static readonly CValuePercent HighPercent = 120;

		// Token: 0x04001AED RID: 6893
		public static readonly CValuePercent SpecialPercentBaiXie = 160;

		// Token: 0x04001AEE RID: 6894
		public static readonly CValuePercent SpecialPercentMingYunWuJianYu = 240;

		// Token: 0x04001AEF RID: 6895
		public static readonly CValuePercent SpecialPercentLoongFire = 160;
	}
}
