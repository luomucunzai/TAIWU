using System.Collections.Generic;

namespace GameData.Domains.TaiwuEvent.EventManager;

public class HeadEventCollection
{
	public List<TaiwuEvent> _headEventList;

	public void Trigger<T0, T1>(EventArgBox argBox)
	{
		EventArgBox eventArgBox = null;
		foreach (TaiwuEvent headEvent in _headEventList)
		{
			if (eventArgBox == null)
			{
				eventArgBox = DomainManager.TaiwuEvent.GetEventArgBox();
			}
			headEvent.ArgBox = eventArgBox;
			if (headEvent.EventConfig.CheckCondition())
			{
				DomainManager.TaiwuEvent.AddTriggeredEvent(headEvent);
				eventArgBox = null;
			}
			else
			{
				headEvent.ArgBox = null;
			}
		}
	}
}
