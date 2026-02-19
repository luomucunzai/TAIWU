using System.Collections.Generic;
using Config.EventConfig;
using GameData.Domains.Map;
using GameData.Domains.TaiwuEvent.Enum;
using GameData.Serializer;
using GameData.Utilities;
using NLog;

namespace GameData.Domains.TaiwuEvent.EventManager;

public class TutorialEventManager : EventManagerBase
{
	private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

	private readonly Dictionary<string, TaiwuEvent> _eventDictionary;

	private readonly List<TaiwuEvent> _headEventList;

	private EventArgBox _argBox;

	public TutorialEventManager()
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
		List<TaiwuEventItem> eventsByType = package.GetEventsByType(EEventType.TutorialEvent);
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
		_argBox = DomainManager.TaiwuEvent.GetEventArgBox();
	}

	public override void ClearExtendOptions()
	{
		foreach (KeyValuePair<string, TaiwuEvent> item in _eventDictionary)
		{
			item.Value.ExtendEventOptions?.Clear();
		}
		_argBox?.Clear();
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

	public override void OnEventTrigger_EnterTutorialChapter(short arg0)
	{
		_argBox.Set("ChapterIndex", arg0);
		DomainManager.TutorialChapter.SetTutorialEventArgBox(arg0, _argBox);
		foreach (TaiwuEvent headEvent in _headEventList)
		{
			if (headEvent.EventConfig.TriggerType == EventTrigger.EnterTutorialChapter)
			{
				headEvent.ArgBox = _argBox;
				if (headEvent.EventConfig.CheckCondition())
				{
					DomainManager.TaiwuEvent.AddTriggeredEvent(headEvent);
				}
			}
		}
	}

	public override void OnEventTrigger_TaiwuBlockChanged(Location arg0, Location arg1)
	{
		_argBox.Set("BlockFrom", (ISerializableGameData)(object)arg0);
		_argBox.Set("BlockTo", (ISerializableGameData)(object)arg1);
		foreach (TaiwuEvent headEvent in _headEventList)
		{
			if (headEvent.EventConfig.TriggerType == EventTrigger.TaiwuBlockChanged)
			{
				headEvent.ArgBox = _argBox;
				if (headEvent.EventConfig.CheckCondition())
				{
					DomainManager.TaiwuEvent.AddTriggeredEvent(headEvent);
				}
				else
				{
					headEvent.ArgBox = null;
				}
			}
		}
	}

	public override void OnEventTrigger_BlackMaskAnimationComplete(bool arg0)
	{
		_argBox.Set("MaskVisible", arg0);
		foreach (TaiwuEvent headEvent in _headEventList)
		{
			if (headEvent.EventConfig.TriggerType == EventTrigger.BlackMaskAnimationComplete)
			{
				headEvent.ArgBox = _argBox;
				if (headEvent.EventConfig.CheckCondition())
				{
					DomainManager.TaiwuEvent.AddTriggeredEvent(headEvent);
				}
				else
				{
					headEvent.ArgBox = null;
				}
			}
		}
	}

	public override void OnEventTrigger_CombatOpening(int arg0)
	{
		_argBox.Set("CharacterId", arg0);
		foreach (TaiwuEvent headEvent in _headEventList)
		{
			if (headEvent.EventConfig.TriggerType == EventTrigger.CombatOpening)
			{
				headEvent.ArgBox = _argBox;
				if (headEvent.EventConfig.CheckCondition())
				{
					DomainManager.TaiwuEvent.AddTriggeredEvent(headEvent);
				}
				else
				{
					headEvent.ArgBox = null;
				}
			}
		}
	}

	public override void OnEventTrigger_NewGameMonth()
	{
		foreach (TaiwuEvent headEvent in _headEventList)
		{
			if (headEvent.EventConfig.TriggerType == EventTrigger.NewGameMonth)
			{
				headEvent.ArgBox = _argBox;
				if (headEvent.EventConfig.CheckCondition())
				{
					DomainManager.TaiwuEvent.AddTriggeredEvent(headEvent);
				}
				else
				{
					headEvent.ArgBox = null;
				}
			}
		}
	}

	public override void OnEventTrigger_TryMoveWhenMoveDisabled()
	{
		foreach (TaiwuEvent headEvent in _headEventList)
		{
			if (headEvent.EventConfig.TriggerType == EventTrigger.TryMoveWhenMoveDisabled && !DomainManager.TaiwuEvent.IsTriggeredEvent(headEvent.EventGuid))
			{
				headEvent.ArgBox = _argBox;
				if (headEvent.EventConfig.CheckCondition())
				{
					DomainManager.TaiwuEvent.AddTriggeredEvent(headEvent);
				}
				else
				{
					headEvent.ArgBox = null;
				}
			}
		}
	}

	public override void OnEventTrigger_TryMoveToInvalidLocationInTutorial()
	{
		foreach (TaiwuEvent headEvent in _headEventList)
		{
			if (headEvent.EventConfig.TriggerType == EventTrigger.TryMoveToInvalidLocationInTutorial && !DomainManager.TaiwuEvent.IsTriggeredEvent(headEvent.EventGuid))
			{
				headEvent.ArgBox = _argBox;
				if (headEvent.EventConfig.CheckCondition())
				{
					DomainManager.TaiwuEvent.AddTriggeredEvent(headEvent);
				}
				else
				{
					headEvent.ArgBox = null;
				}
			}
		}
	}
}
