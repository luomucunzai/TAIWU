using System;
using System.Collections.Generic;

namespace GameData.Domains.TaiwuEvent.EventManager
{
	// Token: 0x020000CD RID: 205
	public class HeadEventCollection
	{
		// Token: 0x06001C78 RID: 7288 RVA: 0x00182804 File Offset: 0x00180A04
		public void Trigger<T0, T1>(EventArgBox argBox)
		{
			EventArgBox tmpArgBox = null;
			foreach (TaiwuEvent taiwuEvent in this._headEventList)
			{
				bool flag = tmpArgBox == null;
				if (flag)
				{
					tmpArgBox = DomainManager.TaiwuEvent.GetEventArgBox();
				}
				taiwuEvent.ArgBox = tmpArgBox;
				bool flag2 = taiwuEvent.EventConfig.CheckCondition();
				if (flag2)
				{
					DomainManager.TaiwuEvent.AddTriggeredEvent(taiwuEvent);
					tmpArgBox = null;
				}
				else
				{
					taiwuEvent.ArgBox = null;
				}
			}
		}

		// Token: 0x04000693 RID: 1683
		public List<TaiwuEvent> _headEventList;
	}
}
