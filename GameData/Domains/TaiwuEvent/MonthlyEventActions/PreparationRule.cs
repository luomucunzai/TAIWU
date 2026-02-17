using System;

namespace GameData.Domains.TaiwuEvent.MonthlyEventActions
{
	// Token: 0x02000094 RID: 148
	public struct PreparationRule
	{
		// Token: 0x040005BC RID: 1468
		[Obsolete]
		public sbyte StartMonth;

		// Token: 0x040005BD RID: 1469
		public short PreparationDuration;

		// Token: 0x040005BE RID: 1470
		public bool CanStartEarly;

		// Token: 0x040005BF RID: 1471
		public short Interval;
	}
}
