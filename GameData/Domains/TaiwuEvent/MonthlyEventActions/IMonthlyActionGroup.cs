using System;

namespace GameData.Domains.TaiwuEvent.MonthlyEventActions
{
	// Token: 0x02000090 RID: 144
	public interface IMonthlyActionGroup
	{
		// Token: 0x0600193F RID: 6463
		void DeactivateSubAction(short areaId, short blockId, bool isComplete);

		// Token: 0x06001940 RID: 6464
		ConfigMonthlyAction GetConfigAction(short areaId, short blockId);
	}
}
