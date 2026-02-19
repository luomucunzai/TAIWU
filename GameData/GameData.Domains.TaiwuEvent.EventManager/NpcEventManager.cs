using System;
using System.Collections.Generic;
using Config.EventConfig;
using GameData.Domains.Map;
using GameData.Domains.TaiwuEvent.Enum;
using GameData.Serializer;
using GameData.Utilities;
using NLog;

namespace GameData.Domains.TaiwuEvent.EventManager;

public class NpcEventManager : EventManagerBase
{
	private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

	private Dictionary<string, TaiwuEvent> _eventDictionary;

	private List<TaiwuEvent> _headEventList;

	public NpcEventManager()
	{
		_eventDictionary = new Dictionary<string, TaiwuEvent>();
		_headEventList = new List<TaiwuEvent>();
	}

	public override TaiwuEvent GetEvent(string eventGuid)
	{
		_eventDictionary.TryGetValue(eventGuid, out var value);
		return value;
	}

	public override void HandleEventPackage(EventPackage package)
	{
		List<TaiwuEventItem> eventsByType = package.GetEventsByType(EEventType.NpcInteractEvent);
		for (int i = 0; i < eventsByType.Count; i++)
		{
			TaiwuEvent taiwuEvent = new TaiwuEvent
			{
				EventGuid = eventsByType[i].Guid.ToString(),
				EventConfig = eventsByType[i],
				ExtendEventOptions = new List<(string, string)>()
			};
			if (!eventsByType[i].IsHeadEvent && eventsByType[i].TriggerType != EventTrigger.None)
			{
				AdaptableLog.Warning($"event {eventsByType[i].Guid} selected a TriggerType but IsHeadEvent set as false,this means TriggerType will not take effect");
			}
			if (!_eventDictionary.ContainsKey(eventsByType[i].Guid.ToString()))
			{
				_eventDictionary.Add(eventsByType[i].Guid.ToString(), taiwuEvent);
				if (eventsByType[i].IsHeadEvent)
				{
					_headEventList.Add(taiwuEvent);
				}
				continue;
			}
			_eventDictionary[taiwuEvent.EventGuid] = taiwuEvent;
			if (eventsByType[i].IsHeadEvent)
			{
				int num = _headEventList.FindIndex((TaiwuEvent e) => e.EventGuid == taiwuEvent.EventGuid);
				if (num >= 0)
				{
					_headEventList[num] = taiwuEvent;
				}
				else
				{
					_headEventList.Add(taiwuEvent);
				}
			}
		}
		foreach (KeyValuePair<string, TaiwuEvent> item in _eventDictionary)
		{
			item.Value.EventConfig.TaiwuEvent = item.Value;
		}
	}

	public override void ClearExtendOptions()
	{
		foreach (KeyValuePair<string, TaiwuEvent> item in _eventDictionary)
		{
			item.Value.ExtendEventOptions?.Clear();
		}
	}

	public override void Reset()
	{
		_eventDictionary.Clear();
		_headEventList.Clear();
	}

	public override void UnloadPackage(EventPackage package)
	{
		List<string> list = new List<string>();
		foreach (KeyValuePair<string, TaiwuEvent> item in _eventDictionary)
		{
			if (item.Value.EventConfig.Package.Equals(package))
			{
				list.Add(item.Key);
			}
		}
		foreach (string item2 in list)
		{
			if (_eventDictionary.TryGetValue(item2, out var value))
			{
				_eventDictionary.Remove(item2);
				_headEventList.Remove(value);
			}
		}
	}

	public override void OnEventTrigger_TaiwuBlockChanged(Location arg0, Location arg1)
	{
		EventArgBox eventArgBox = null;
		foreach (TaiwuEvent headEvent in _headEventList)
		{
			if (headEvent.EventConfig.TriggerType == EventTrigger.TaiwuBlockChanged && (!DomainManager.TaiwuEvent.IsTriggeredEvent(headEvent.EventGuid) || headEvent.ArgBox == null))
			{
				if (eventArgBox == null)
				{
					eventArgBox = DomainManager.TaiwuEvent.GetEventArgBox();
				}
				eventArgBox.Set("BlockFrom", (ISerializableGameData)(object)arg0);
				eventArgBox.Set("BlockTo", (ISerializableGameData)(object)arg1);
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
		if (eventArgBox != null)
		{
			DomainManager.TaiwuEvent.ReturnArgBox(eventArgBox);
		}
	}

	public override void OnEventTrigger_CharacterClicked(int arg0)
	{
		EventArgBox eventArgBox = null;
		foreach (TaiwuEvent headEvent in _headEventList)
		{
			if (headEvent.EventConfig.TriggerType == EventTrigger.CharacterClicked)
			{
				if (eventArgBox == null)
				{
					eventArgBox = DomainManager.TaiwuEvent.GetEventArgBox();
				}
				eventArgBox.Set("CharacterId", arg0);
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
		if (eventArgBox != null)
		{
			DomainManager.TaiwuEvent.ReturnArgBox(eventArgBox);
		}
	}

	public override void OnEventTrigger_LetTeammateLeaveGroup(int arg0)
	{
		EventArgBox eventArgBox = null;
		foreach (TaiwuEvent headEvent in _headEventList)
		{
			if (headEvent.EventConfig.TriggerType == EventTrigger.LetTeammateLeaveGroup)
			{
				if (eventArgBox == null)
				{
					eventArgBox = DomainManager.TaiwuEvent.GetEventArgBox();
				}
				eventArgBox.Set("CharacterId", arg0);
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
		if (eventArgBox != null)
		{
			DomainManager.TaiwuEvent.ReturnArgBox(eventArgBox);
		}
	}

	public override void OnEventTrigger_CaravanClicked(int arg0)
	{
		EventArgBox eventArgBox = null;
		foreach (TaiwuEvent headEvent in _headEventList)
		{
			if (headEvent.EventConfig.TriggerType == EventTrigger.CaravanClicked)
			{
				if (eventArgBox == null)
				{
					eventArgBox = DomainManager.TaiwuEvent.GetEventArgBox();
				}
				eventArgBox.Set("CaravanId", arg0);
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
		if (eventArgBox != null)
		{
			DomainManager.TaiwuEvent.ReturnArgBox(eventArgBox);
		}
	}

	public override void OnEventTrigger_KidnappedCharacterClicked(int arg0)
	{
		EventArgBox eventArgBox = null;
		foreach (TaiwuEvent headEvent in _headEventList)
		{
			if (headEvent.EventConfig.TriggerType == EventTrigger.KidnappedCharacterClicked)
			{
				if (eventArgBox == null)
				{
					eventArgBox = DomainManager.TaiwuEvent.GetEventArgBox();
				}
				eventArgBox.Set("CharacterId", arg0);
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
		if (eventArgBox != null)
		{
			DomainManager.TaiwuEvent.ReturnArgBox(eventArgBox);
		}
	}

	public override void OnEventTrigger_TeammateMonthAdvance(int arg0)
	{
		EventArgBox eventArgBox = null;
		for (int i = 0; i < _headEventList.Count; i++)
		{
			TaiwuEvent taiwuEvent = _headEventList[i];
			if (taiwuEvent.EventConfig.TriggerType == EventTrigger.TeammateMonthAdvance)
			{
				if (eventArgBox == null)
				{
					eventArgBox = DomainManager.TaiwuEvent.GetEventArgBox();
				}
				if (DomainManager.TaiwuEvent.IsTriggeredEvent(taiwuEvent.EventGuid))
				{
					TaiwuEventItem eventConfig = taiwuEvent.EventConfig;
					taiwuEvent = new TaiwuEvent(taiwuEvent);
					taiwuEvent.EventConfig = eventConfig;
					taiwuEvent.EventGuid = Guid.NewGuid().ToString();
					eventArgBox.Set("EventInstanceGuid", taiwuEvent.EventGuid);
				}
				eventArgBox.Set("CharacterId", arg0);
				taiwuEvent.ArgBox = eventArgBox;
				if (taiwuEvent.EventConfig.CheckCondition())
				{
					DomainManager.TaiwuEvent.AddTriggeredEvent(taiwuEvent);
					eventArgBox = null;
				}
				else
				{
					taiwuEvent.ArgBox = null;
				}
			}
		}
		if (eventArgBox != null)
		{
			DomainManager.TaiwuEvent.ReturnArgBox(eventArgBox);
		}
	}

	public override void OnEventTrigger_SameBlockWithTaiwuWhenMonthAdvance(int arg0)
	{
		EventArgBox eventArgBox = null;
		for (int i = 0; i < _headEventList.Count; i++)
		{
			TaiwuEvent taiwuEvent = _headEventList[i];
			if (taiwuEvent.EventConfig.TriggerType == EventTrigger.SameBlockWithTaiwuWhenMonthAdvance)
			{
				if (DomainManager.TaiwuEvent.IsTriggeredEvent(taiwuEvent.EventGuid))
				{
					taiwuEvent = new TaiwuEvent(taiwuEvent);
				}
				taiwuEvent.EventGuid = Guid.NewGuid().ToString();
				if (eventArgBox == null)
				{
					eventArgBox = DomainManager.TaiwuEvent.GetEventArgBox();
				}
				eventArgBox.Set("CharacterId", arg0);
				taiwuEvent.ArgBox = eventArgBox;
				if (taiwuEvent.EventConfig.CheckCondition())
				{
					DomainManager.TaiwuEvent.AddTriggeredEvent(taiwuEvent);
					eventArgBox = null;
				}
				else
				{
					taiwuEvent.ArgBox = null;
				}
			}
		}
		if (eventArgBox != null)
		{
			DomainManager.TaiwuEvent.ReturnArgBox(eventArgBox);
		}
	}

	public override void OnEventTrigger_SameBlockWithRandomEnemyOnNewMonth()
	{
		EventArgBox eventArgBox = null;
		foreach (TaiwuEvent headEvent in _headEventList)
		{
			if (headEvent.EventConfig.TriggerType == EventTrigger.SameBlockWithRandomEnemyOnNewMonth)
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
		if (eventArgBox != null)
		{
			DomainManager.TaiwuEvent.ReturnArgBox(eventArgBox);
		}
	}

	public override void OnEventTrigger_SameBlockWithTaiwuOnNewMonthSpecial(int arg0)
	{
		EventArgBox eventArgBox = null;
		for (int i = 0; i < _headEventList.Count; i++)
		{
			TaiwuEvent taiwuEvent = _headEventList[i];
			if (taiwuEvent.EventConfig.TriggerType == EventTrigger.SameBlockWithTaiwuOnNewMonthSpecial)
			{
				if (DomainManager.TaiwuEvent.IsTriggeredEvent(taiwuEvent.EventGuid))
				{
					taiwuEvent = new TaiwuEvent(taiwuEvent);
				}
				taiwuEvent.EventGuid = Guid.NewGuid().ToString();
				if (eventArgBox == null)
				{
					eventArgBox = DomainManager.TaiwuEvent.GetEventArgBox();
				}
				eventArgBox.Set("CharacterId", arg0);
				taiwuEvent.ArgBox = eventArgBox;
				if (taiwuEvent.EventConfig.CheckCondition())
				{
					DomainManager.TaiwuEvent.AddTriggeredEvent(taiwuEvent);
					eventArgBox = null;
				}
				else
				{
					taiwuEvent.ArgBox = null;
				}
			}
		}
		if (eventArgBox != null)
		{
			DomainManager.TaiwuEvent.ReturnArgBox(eventArgBox);
		}
	}

	public override void OnEventTrigger_AnimalAvatarClicked(int arg0)
	{
		EventArgBox eventArgBox = null;
		foreach (TaiwuEvent headEvent in _headEventList)
		{
			if (headEvent.EventConfig.TriggerType == EventTrigger.AnimalAvatarClicked)
			{
				if (eventArgBox == null)
				{
					eventArgBox = DomainManager.TaiwuEvent.GetEventArgBox();
				}
				eventArgBox.Set("AnimalId", arg0);
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
		if (eventArgBox != null)
		{
			DomainManager.TaiwuEvent.ReturnArgBox(eventArgBox);
		}
	}

	public override void OnEventTrigger_NpcTombClicked(int arg0)
	{
		EventArgBox eventArgBox = null;
		foreach (TaiwuEvent headEvent in _headEventList)
		{
			if (headEvent.EventConfig.TriggerType == EventTrigger.NpcTombClicked)
			{
				if (eventArgBox == null)
				{
					eventArgBox = DomainManager.TaiwuEvent.GetEventArgBox();
				}
				eventArgBox.Set("TombId", arg0);
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
		if (eventArgBox != null)
		{
			DomainManager.TaiwuEvent.ReturnArgBox(eventArgBox);
		}
	}
}
