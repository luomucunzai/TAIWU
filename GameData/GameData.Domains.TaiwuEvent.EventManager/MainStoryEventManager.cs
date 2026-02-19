using System.Collections.Generic;
using Config.EventConfig;
using GameData.Domains.Building;
using GameData.Domains.Map;
using GameData.Domains.TaiwuEvent.Enum;
using GameData.Serializer;
using GameData.Utilities;
using NLog;

namespace GameData.Domains.TaiwuEvent.EventManager;

public class MainStoryEventManager : EventManagerBase
{
	private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

	private Dictionary<string, TaiwuEvent> _eventDictionary;

	private List<TaiwuEvent> _headEventList;

	public MainStoryEventManager()
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
		List<TaiwuEventItem> eventsByType = package.GetEventsByType(EEventType.MainStoryEvent);
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

	public override void OnEventTrigger_RecordEnterGame(short arg0)
	{
		EventArgBox eventArgBox = null;
		foreach (TaiwuEvent headEvent in _headEventList)
		{
			if (headEvent.EventConfig.TriggerType == EventTrigger.RecordEnterGame)
			{
				if (eventArgBox == null)
				{
					eventArgBox = DomainManager.TaiwuEvent.GetEventArgBox();
				}
				eventArgBox.Set("MainStoryLine", arg0);
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

	public override void OnEventTrigger_NewGameMonth()
	{
		EventArgBox eventArgBox = null;
		foreach (TaiwuEvent headEvent in _headEventList)
		{
			if (headEvent.EventConfig.TriggerType == EventTrigger.NewGameMonth)
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

	public override void OnEventTrigger_CombatWithXiangshuMinionComplete(short arg0)
	{
		EventArgBox eventArgBox = null;
		foreach (TaiwuEvent headEvent in _headEventList)
		{
			if (headEvent.EventConfig.TriggerType == EventTrigger.CombatWithXiangshuMinionComplete)
			{
				if (eventArgBox == null)
				{
					eventArgBox = DomainManager.TaiwuEvent.GetEventArgBox();
				}
				eventArgBox.Set("TemplateId", arg0);
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

	public override void OnEventTrigger_BlackMaskAnimationComplete(bool arg0)
	{
		EventArgBox eventArgBox = null;
		foreach (TaiwuEvent headEvent in _headEventList)
		{
			if (headEvent.EventConfig.TriggerType == EventTrigger.BlackMaskAnimationComplete)
			{
				if (eventArgBox == null)
				{
					eventArgBox = DomainManager.TaiwuEvent.GetEventArgBox();
				}
				eventArgBox.Set("MaskVisible", arg0);
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

	public override void OnEventTrigger_ConstructComplete(BuildingBlockKey arg0, short arg1, sbyte arg2)
	{
		EventArgBox eventArgBox = null;
		foreach (TaiwuEvent headEvent in _headEventList)
		{
			if (headEvent.EventConfig.TriggerType == EventTrigger.ConstructComplete)
			{
				if (eventArgBox == null)
				{
					eventArgBox = DomainManager.TaiwuEvent.GetEventArgBox();
				}
				eventArgBox.Set("BuildingBlockKey", (ISerializableGameData)(object)arg0);
				eventArgBox.Set("TemplateId", arg1);
				eventArgBox.Set("Level", arg2);
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

	public override void OnEventTrigger_CombatOpening(int arg0)
	{
		EventArgBox eventArgBox = null;
		foreach (TaiwuEvent headEvent in _headEventList)
		{
			if (headEvent.EventConfig.TriggerType == EventTrigger.CombatOpening)
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

	public override void OnEventTrigger_MakingSystemOpened(BuildingBlockKey arg0, short arg1)
	{
		EventArgBox eventArgBox = null;
		foreach (TaiwuEvent headEvent in _headEventList)
		{
			if (headEvent.EventConfig.TriggerType == EventTrigger.MakingSystemOpened)
			{
				if (eventArgBox == null)
				{
					eventArgBox = DomainManager.TaiwuEvent.GetEventArgBox();
				}
				eventArgBox.Set("BuildingBlockKey", (ISerializableGameData)(object)arg0);
				eventArgBox.Set("TemplateId", arg1);
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

	public override void OnEventTrigger_SectBuildingClicked(short arg0)
	{
		EventArgBox eventArgBox = null;
		foreach (TaiwuEvent headEvent in _headEventList)
		{
			if (headEvent.EventConfig.TriggerType == EventTrigger.SectBuildingClicked)
			{
				if (eventArgBox == null)
				{
					eventArgBox = DomainManager.TaiwuEvent.GetEventArgBox();
				}
				eventArgBox.Set("BuildingTemplateId", arg0);
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

	public override void OnEventTrigger_CollectedMakingSystemItem(BuildingBlockKey arg0, short arg1, bool arg2)
	{
		EventArgBox eventArgBox = null;
		foreach (TaiwuEvent headEvent in _headEventList)
		{
			if (headEvent.EventConfig.TriggerType == EventTrigger.CollectedMakingSystemItem)
			{
				if (eventArgBox == null)
				{
					eventArgBox = DomainManager.TaiwuEvent.GetEventArgBox();
				}
				eventArgBox.Set("BuildingBlockKey", (ISerializableGameData)(object)arg0);
				eventArgBox.Set("TemplateId", arg1);
				eventArgBox.Set("ShowingGetItem", arg2);
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

	public override void OnEventTrigger_MainStoryFinishCatchCricket(bool arg0)
	{
		EventArgBox eventArgBox = null;
		foreach (TaiwuEvent headEvent in _headEventList)
		{
			if (headEvent.EventConfig.TriggerType == EventTrigger.MainStoryFinishCatchCricket)
			{
				if (eventArgBox == null)
				{
					eventArgBox = DomainManager.TaiwuEvent.GetEventArgBox();
				}
				eventArgBox.Set("CricketCatchSuccess", arg0);
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

	public override void OnEventTrigger_PurpleBambooAvatarClicked(int arg0, sbyte arg1)
	{
		EventArgBox eventArgBox = null;
		foreach (TaiwuEvent headEvent in _headEventList)
		{
			if (headEvent.EventConfig.TriggerType == EventTrigger.PurpleBambooAvatarClicked)
			{
				if (eventArgBox == null)
				{
					eventArgBox = DomainManager.TaiwuEvent.GetEventArgBox();
				}
				eventArgBox.Set("CharacterId", arg0);
				eventArgBox.Set("XiangshuAvatarId", arg1);
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

	public override void OnEventTrigger_UserLoadDreamBackArchive()
	{
		EventArgBox eventArgBox = null;
		foreach (TaiwuEvent headEvent in _headEventList)
		{
			if (headEvent.EventConfig.TriggerType == EventTrigger.UserLoadDreamBackArchive)
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

	public override void OnEventTrigger_TaiwuVillageDestroyed()
	{
		EventArgBox eventArgBox = null;
		foreach (TaiwuEvent headEvent in _headEventList)
		{
			if (headEvent.EventConfig.TriggerType == EventTrigger.TaiwuVillageDestroyed)
			{
				if (eventArgBox == null)
				{
					eventArgBox = DomainManager.TaiwuEvent.GetEventArgBox();
				}
				headEvent.ArgBox = eventArgBox;
				if (headEvent.EventConfig.CheckCondition())
				{
					DomainManager.World.SetTaiwuVillageDestroyed();
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

	public override void OnEventTrigger_TaiwuCrossArchive()
	{
		EventArgBox eventArgBox = null;
		foreach (TaiwuEvent headEvent in _headEventList)
		{
			if (headEvent.EventConfig.TriggerType == EventTrigger.TaiwuCrossArchive)
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

	public override void OnEventTrigger_TaiwuCrossArchiveFindMemory(sbyte arg0)
	{
		EventArgBox eventArgBox = null;
		foreach (TaiwuEvent headEvent in _headEventList)
		{
			if (headEvent.EventConfig.TriggerType == EventTrigger.TaiwuCrossArchiveFindMemory)
			{
				if (eventArgBox == null)
				{
					eventArgBox = DomainManager.TaiwuEvent.GetEventArgBox();
				}
				eventArgBox.Set("DreamBackUnlockStateType", arg0);
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

	public override void OnEventTrigger_ConfirmEnterSwordTomb()
	{
		EventArgBox eventArgBox = null;
		foreach (TaiwuEvent headEvent in _headEventList)
		{
			if (headEvent.EventConfig.TriggerType == EventTrigger.ConfirmEnterSwordTomb)
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

	public override void OnEventTrigger_AdventureRemoved(short arg0, Location arg1, bool arg2)
	{
		EventArgBox eventArgBox = null;
		foreach (TaiwuEvent headEvent in _headEventList)
		{
			if (headEvent.EventConfig.TriggerType == EventTrigger.AdventureRemoved)
			{
				if (eventArgBox == null)
				{
					eventArgBox = DomainManager.TaiwuEvent.GetEventArgBox();
				}
				eventArgBox.Set("AdventureTemplateId", arg0);
				eventArgBox.Set("AdventureLocation", (ISerializableGameData)(object)arg1);
				eventArgBox.Set("IsComplete", arg2);
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
