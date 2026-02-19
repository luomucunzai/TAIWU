using System;
using System.Collections.Generic;
using Config.EventConfig;
using GameData.Domains.Extra;
using GameData.Domains.Item;
using GameData.Domains.Item.Display;
using GameData.Domains.Map;
using GameData.Domains.TaiwuEvent.Enum;
using GameData.Serializer;
using GameData.Utilities;
using NLog;

namespace GameData.Domains.TaiwuEvent.EventManager;

public class GlobalCommonEventManager : EventManagerBase
{
	private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

	private Dictionary<string, TaiwuEvent> _eventDictionary;

	private List<TaiwuEvent> _headEventList;

	private EventArgBox _argBox;

	public GlobalCommonEventManager()
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
		List<TaiwuEventItem> eventsByType = package.GetEventsByType(EEventType.GlobalCommonEvent);
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

	public override void OnEventTrigger_NeedToPassLegacy(bool arg0, string arg1)
	{
		EventArgBox eventArgBox = null;
		foreach (TaiwuEvent headEvent in _headEventList)
		{
			if (headEvent.EventConfig.TriggerType == EventTrigger.NeedToPassLegacy)
			{
				if (eventArgBox == null)
				{
					eventArgBox = DomainManager.TaiwuEvent.GetEventArgBox();
				}
				eventArgBox.Set("IsTaiwuDying", arg0);
				eventArgBox.Set("OnFinishPassingLegacyEvent", arg1);
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

	public override void OnEventTrigger_OnSectSpecialBuildingClicked(short arg0)
	{
		EventArgBox eventArgBox = null;
		foreach (TaiwuEvent headEvent in _headEventList)
		{
			if (headEvent.EventConfig.TriggerType == EventTrigger.OnSectSpecialBuildingClicked)
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

	public override void OnEventTrigger_OnSettlementTreasuryBuildingClicked(short arg0, byte arg1, sbyte arg2)
	{
		EventArgBox eventArgBox = null;
		foreach (TaiwuEvent headEvent in _headEventList)
		{
			if (headEvent.EventConfig.TriggerType == EventTrigger.OnSettlementTreasuryBuildingClicked)
			{
				if (eventArgBox == null)
				{
					eventArgBox = DomainManager.TaiwuEvent.GetEventArgBox();
				}
				eventArgBox.Set("BuildingTemplateId", arg0);
				eventArgBox.Set("CustomStatus", arg1);
				eventArgBox.Set("CurrentPage", arg2);
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

	public override void OnEventTrigger_SecretInformationBroadcast(int arg0)
	{
		EventArgBox eventArgBox = null;
		for (int i = 0; i < _headEventList.Count; i++)
		{
			TaiwuEvent taiwuEvent = _headEventList[i];
			if (taiwuEvent.EventConfig.TriggerType == EventTrigger.SecretInformationBroadcast)
			{
				if (eventArgBox == null)
				{
					eventArgBox = DomainManager.TaiwuEvent.GetEventArgBox();
				}
				eventArgBox.Set("metaDataId", arg0);
				if (DomainManager.TaiwuEvent.IsTriggeredEvent(taiwuEvent.EventGuid))
				{
					TaiwuEvent taiwuEvent2 = new TaiwuEvent();
					taiwuEvent2.EventGuid = Guid.NewGuid().ToString();
					taiwuEvent2.ArgBox = eventArgBox;
					taiwuEvent2.EventConfig = taiwuEvent.EventConfig;
					taiwuEvent2.ExtendEventOptions = new List<(string, string)>(taiwuEvent.ExtendEventOptions);
					taiwuEvent = taiwuEvent2;
				}
				else
				{
					taiwuEvent.ArgBox = eventArgBox;
				}
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

	public override void OnEventTrigger_LifeSkillCombatForceSilent(int arg0, sbyte arg1, sbyte arg2)
	{
		EventArgBox eventArgBox = null;
		foreach (TaiwuEvent headEvent in _headEventList)
		{
			if (headEvent.EventConfig.TriggerType == EventTrigger.LifeSkillCombatForceSilent)
			{
				if (eventArgBox == null)
				{
					eventArgBox = DomainManager.TaiwuEvent.GetEventArgBox();
				}
				eventArgBox.Set("CharacterId", arg0);
				eventArgBox.Set("ConcessionCount", arg1);
				eventArgBox.Set("InducementCount", arg2);
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

	public override void OnEventTrigger_ProfessionExperienceChange(int arg0, int arg1, int arg2)
	{
		EventArgBox eventArgBox = null;
		foreach (TaiwuEvent headEvent in _headEventList)
		{
			if (headEvent.EventConfig.TriggerType == EventTrigger.ProfessionExperienceChange && !DomainManager.TaiwuEvent.IsTriggeredEvent(headEvent.EventGuid))
			{
				if (eventArgBox == null)
				{
					eventArgBox = DomainManager.TaiwuEvent.GetEventArgBox();
				}
				eventArgBox.Set("ProfessionTemplateId", arg0);
				eventArgBox.Set("PrevExp", arg1);
				eventArgBox.Set("NewExp", arg2);
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

	public override void OnEventTrigger_ProfessionSkillClicked(int arg0)
	{
		EventArgBox eventArgBox = null;
		foreach (TaiwuEvent headEvent in _headEventList)
		{
			if (headEvent.EventConfig.TriggerType == EventTrigger.ProfessionSkillClicked)
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

	public override void OnEventTrigger_TaiwuGotTianjieFulu(int arg0, ItemKey arg1, int arg2)
	{
		EventArgBox eventArgBox = null;
		foreach (TaiwuEvent headEvent in _headEventList)
		{
			if (headEvent.EventConfig.TriggerType == EventTrigger.TaiwuGotTianjieFulu && !DomainManager.TaiwuEvent.IsTriggeredEvent(headEvent.EventGuid))
			{
				if (eventArgBox == null)
				{
					eventArgBox = DomainManager.TaiwuEvent.GetEventArgBox();
				}
				eventArgBox.Set("CharacterId", arg0);
				eventArgBox.Set("ItemKey", (ISerializableGameData)(object)arg1);
				eventArgBox.Set("Count", arg2);
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

	public override void OnEventTrigger_TaiwuSaveCountChange(int arg0)
	{
		EventArgBox eventArgBox = null;
		foreach (TaiwuEvent headEvent in _headEventList)
		{
			if (headEvent.EventConfig.TriggerType == EventTrigger.TaiwuSaveCountChange && !DomainManager.TaiwuEvent.IsTriggeredEvent(headEvent.EventGuid))
			{
				if (eventArgBox == null)
				{
					eventArgBox = DomainManager.TaiwuEvent.GetEventArgBox();
				}
				eventArgBox.Set("SaveCount", arg0);
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

	public override void OnEventTrigger_CharacterTemplateClicked(short arg0)
	{
		EventArgBox eventArgBox = null;
		foreach (TaiwuEvent headEvent in _headEventList)
		{
			if (headEvent.EventConfig.TriggerType == EventTrigger.CharacterTemplateClicked)
			{
				if (eventArgBox == null)
				{
					eventArgBox = DomainManager.TaiwuEvent.GetEventArgBox();
				}
				eventArgBox.Set("CharacterTemplateId", arg0);
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

	public override void OnEventTrigger_DlcLoongPutJiaoEggs(int arg0, ItemKey arg1)
	{
		EventArgBox eventArgBox = null;
		foreach (TaiwuEvent headEvent in _headEventList)
		{
			if (headEvent.EventConfig.TriggerType == EventTrigger.DlcLoongPutJiaoEggs)
			{
				if (eventArgBox == null)
				{
					eventArgBox = DomainManager.TaiwuEvent.GetEventArgBox();
				}
				eventArgBox.Set("PoolId", arg0);
				eventArgBox.Set("EggItemKey", (ISerializableGameData)(object)arg1);
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

	public override void OnEventTrigger_DlcLoongInteractJiao(int arg0)
	{
		EventArgBox eventArgBox = null;
		foreach (TaiwuEvent headEvent in _headEventList)
		{
			if (headEvent.EventConfig.TriggerType == EventTrigger.DlcLoongInteractJiao)
			{
				if (eventArgBox == null)
				{
					eventArgBox = DomainManager.TaiwuEvent.GetEventArgBox();
				}
				eventArgBox.Set("PoolId", arg0);
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

	public override void OnEventTrigger_DlcLoongPetJiao(int arg0)
	{
		EventArgBox eventArgBox = null;
		foreach (TaiwuEvent headEvent in _headEventList)
		{
			if (headEvent.EventConfig.TriggerType == EventTrigger.DlcLoongPetJiao)
			{
				if (eventArgBox == null)
				{
					eventArgBox = DomainManager.TaiwuEvent.GetEventArgBox();
				}
				eventArgBox.Set("PoolId", arg0);
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

	public override void OnEventTrigger_CloseUI(string arg0, bool arg1, int arg2)
	{
		EventArgBox eventArgBox = null;
		foreach (TaiwuEvent headEvent in _headEventList)
		{
			if (headEvent.EventConfig.TriggerType == EventTrigger.CloseUI)
			{
				if (eventArgBox == null)
				{
					eventArgBox = DomainManager.TaiwuEvent.GetEventArgBox();
				}
				eventArgBox.Set("UIName", arg0);
				eventArgBox.Set("PresetBool", arg1);
				eventArgBox.Set("PresetInt", arg2);
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

	public override void OnEventTrigger_TaiwuFindMaterial(int arg0, TreasureFindResult arg1)
	{
		EventArgBox eventArgBox = null;
		foreach (TaiwuEvent headEvent in _headEventList)
		{
			if (headEvent.EventConfig.TriggerType == EventTrigger.TaiwuFindMaterial)
			{
				if (eventArgBox == null)
				{
					eventArgBox = DomainManager.TaiwuEvent.GetEventArgBox();
				}
				eventArgBox.Set("BrokenLevel", arg0);
				eventArgBox.Set("FindResult", (ISerializableGameData)(object)arg1);
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

	public override void OnEventTrigger_TaiwuFindExtraTreasure(TreasureFindResult arg0)
	{
		EventArgBox eventArgBox = null;
		foreach (TaiwuEvent headEvent in _headEventList)
		{
			if (headEvent.EventConfig.TriggerType == EventTrigger.TaiwuFindExtraTreasure)
			{
				if (eventArgBox == null)
				{
					eventArgBox = DomainManager.TaiwuEvent.GetEventArgBox();
				}
				eventArgBox.Set("FindResult", (ISerializableGameData)(object)arg0);
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

	public override void OnEventTrigger_TaiwuCollectWudangHeavenlyTreeSeed(sbyte arg0)
	{
		EventArgBox eventArgBox = null;
		foreach (TaiwuEvent headEvent in _headEventList)
		{
			if (headEvent.EventConfig.TriggerType == EventTrigger.TaiwuCollectWudangHeavenlyTreeSeed)
			{
				if (eventArgBox == null)
				{
					eventArgBox = DomainManager.TaiwuEvent.GetEventArgBox();
				}
				eventArgBox.Set("ResourceType", arg0);
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

	public override void OnEventTrigger_TaiwuVillagerExpelled(int arg0)
	{
		EventArgBox eventArgBox = null;
		foreach (TaiwuEvent headEvent in _headEventList)
		{
			if (headEvent.EventConfig.TriggerType == EventTrigger.TaiwuVillagerExpelled)
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

	public override void OnEventTrigger_JingangSectMainStoryReborn()
	{
		EventArgBox eventArgBox = null;
		foreach (TaiwuEvent headEvent in _headEventList)
		{
			if (headEvent.EventConfig.TriggerType == EventTrigger.JingangSectMainStoryReborn)
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

	public override void OnEventTrigger_JingangSectMainStoryMonkSoul()
	{
		EventArgBox eventArgBox = null;
		foreach (TaiwuEvent headEvent in _headEventList)
		{
			if (headEvent.EventConfig.TriggerType == EventTrigger.JingangSectMainStoryMonkSoul)
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

	public override void OnEventTrigger_OperateInventoryItem(int arg0, sbyte arg1, ItemDisplayData arg2)
	{
		EventArgBox eventArgBox = null;
		foreach (TaiwuEvent headEvent in _headEventList)
		{
			if (headEvent.EventConfig.TriggerType == EventTrigger.OperateInventoryItem)
			{
				headEvent.EventConfig.IgnoreShowingEvent = true;
				if (eventArgBox == null)
				{
					eventArgBox = DomainManager.TaiwuEvent.GetEventArgBox();
				}
				eventArgBox.Set("RoleTaiwu", DomainManager.Taiwu.GetTaiwuCharId());
				eventArgBox.Set("CharacterId", arg0);
				eventArgBox.Set("InventoryItemOperationType", arg1);
				eventArgBox.Set("GetItemKeyCount", arg2.Amount);
				eventArgBox.Set("GetItemKey", (ISerializableGameData)(object)arg2.Key);
				eventArgBox.Set("SelectItemKey", (ISerializableGameData)(object)arg2.Key);
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

	public override void OnEventTrigger_OnShixiangDrumClickedManyTimes()
	{
		EventArgBox eventArgBox = null;
		foreach (TaiwuEvent headEvent in _headEventList)
		{
			if (headEvent.EventConfig.TriggerType == EventTrigger.OnShixiangDrumClickedManyTimes)
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

	public override void OnEventTrigger_OnClickedPrisonBtn(short arg0)
	{
		EventArgBox eventArgBox = null;
		foreach (TaiwuEvent headEvent in _headEventList)
		{
			if (headEvent.EventConfig.TriggerType == EventTrigger.OnClickedPrisonBtn)
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

	public override void OnEventTrigger_OnClickedSendPrisonBtn()
	{
		EventArgBox eventArgBox = null;
		foreach (TaiwuEvent headEvent in _headEventList)
		{
			if (headEvent.EventConfig.TriggerType == EventTrigger.OnClickedSendPrisonBtn)
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

	public override void OnEventTrigger_InteractPrisoner(int arg0, int arg1)
	{
		EventArgBox eventArgBox = null;
		foreach (TaiwuEvent headEvent in _headEventList)
		{
			if (headEvent.EventConfig.TriggerType == EventTrigger.InteractPrisoner)
			{
				if (eventArgBox == null)
				{
					eventArgBox = DomainManager.TaiwuEvent.GetEventArgBox();
				}
				eventArgBox.Set("CharacterId", arg0);
				eventArgBox.Set("InteractPrisonerType", arg1);
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

	public override void OnEventTrigger_ClickChicken(int arg0, short arg1)
	{
		EventArgBox eventArgBox = null;
		foreach (TaiwuEvent headEvent in _headEventList)
		{
			if (headEvent.EventConfig.TriggerType == EventTrigger.ClickChicken)
			{
				if (eventArgBox == null)
				{
					eventArgBox = DomainManager.TaiwuEvent.GetEventArgBox();
				}
				eventArgBox.Set("ChickenId", arg0);
				eventArgBox.Set("ChickenTemplateId", arg1);
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

	public override void OnEventTrigger_SoulWitheringBellTransfer()
	{
		EventArgBox eventArgBox = null;
		foreach (TaiwuEvent headEvent in _headEventList)
		{
			if (headEvent.EventConfig.TriggerType == EventTrigger.SoulWitheringBellTransfer)
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

	public override void OnEventTrigger_CatchThief(sbyte arg0, bool arg1)
	{
		EventArgBox eventArgBox = null;
		foreach (TaiwuEvent headEvent in _headEventList)
		{
			if (headEvent.EventConfig.TriggerType == EventTrigger.CatchThief)
			{
				if (eventArgBox == null)
				{
					eventArgBox = DomainManager.TaiwuEvent.GetEventArgBox();
				}
				eventArgBox.Set("ThiefLevel", arg0);
				eventArgBox.Set("IsTimeout", arg1);
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

	public override void OnEventTrigger_TaiwuBeHuntedArrivedSect(int characterId)
	{
		EventArgBox eventArgBox = null;
		foreach (TaiwuEvent headEvent in _headEventList)
		{
			if (headEvent.EventConfig.TriggerType == EventTrigger.TaiwuBeHuntedArrivedSect)
			{
				if (eventArgBox == null)
				{
					eventArgBox = DomainManager.TaiwuEvent.GetEventArgBox();
				}
				eventArgBox.Set("CharacterId", characterId);
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

	public override void OnEventTrigger_TaiwuBeHuntedHunterDie(int characterId)
	{
		EventArgBox eventArgBox = null;
		foreach (TaiwuEvent headEvent in _headEventList)
		{
			if (headEvent.EventConfig.TriggerType == EventTrigger.TaiwuBeHuntedHunterDie)
			{
				if (eventArgBox == null)
				{
					eventArgBox = DomainManager.TaiwuEvent.GetEventArgBox();
				}
				eventArgBox.Set("CharacterId", characterId);
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

	public override void OnEventTrigger_StartSectShaolinDemonSlayer(int bossIndex)
	{
		EventArgBox eventArgBox = null;
		foreach (TaiwuEvent headEvent in _headEventList)
		{
			if (headEvent.EventConfig.TriggerType == EventTrigger.StartSectShaolinDemonSlayer)
			{
				if (eventArgBox == null)
				{
					eventArgBox = DomainManager.TaiwuEvent.GetEventArgBox();
				}
				eventArgBox.Set("BossIndex", bossIndex);
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

	public override void OnEventTrigger_TriggerMapPickupEvent(Location location, bool isEvent)
	{
		EventArgBox eventArgBox = null;
		foreach (TaiwuEvent headEvent in _headEventList)
		{
			if (headEvent.EventConfig.TriggerType == EventTrigger.TriggerMapPickupEvent)
			{
				if (eventArgBox == null)
				{
					eventArgBox = DomainManager.TaiwuEvent.GetEventArgBox();
				}
				eventArgBox.Set("Location", (ISerializableGameData)(object)location);
				eventArgBox.Set("IsEvent", isEvent);
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

	public override void OnEventTrigger_FixedCharacterClicked(int arg0, short arg1)
	{
		EventArgBox eventArgBox = null;
		foreach (TaiwuEvent headEvent in _headEventList)
		{
			if (headEvent.EventConfig.TriggerType == EventTrigger.FixedCharacterClicked)
			{
				if (eventArgBox == null)
				{
					eventArgBox = DomainManager.TaiwuEvent.GetEventArgBox();
				}
				eventArgBox.Set("CharacterId", arg0);
				eventArgBox.Set("CharacterTemplateId", arg1);
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

	public override void OnEventTrigger_FixedEnemyClicked(int arg0, short arg1)
	{
		EventArgBox eventArgBox = null;
		foreach (TaiwuEvent headEvent in _headEventList)
		{
			if (headEvent.EventConfig.TriggerType == EventTrigger.FixedEnemyClicked)
			{
				if (eventArgBox == null)
				{
					eventArgBox = DomainManager.TaiwuEvent.GetEventArgBox();
				}
				eventArgBox.Set("CharacterId", arg0);
				eventArgBox.Set("CharacterTemplateId", arg1);
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

	public override void OnEventTrigger_TaiwuDeportVitals(int arg0, bool arg1)
	{
		EventArgBox eventArgBox = null;
		foreach (TaiwuEvent headEvent in _headEventList)
		{
			if (headEvent.EventConfig.TriggerType == EventTrigger.TaiwuDeportVitals)
			{
				if (eventArgBox == null)
				{
					eventArgBox = DomainManager.TaiwuEvent.GetEventArgBox();
				}
				eventArgBox.Set("VitalType", arg0);
				eventArgBox.Set("IsGoodEnd", arg1);
				DomainManager.Extra.SetCurrentVitalIndex(arg0);
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

	public override void OnEventTrigger_SwitchToGuardedPage(byte arg0, sbyte arg1)
	{
		EventArgBox eventArgBox = null;
		foreach (TaiwuEvent headEvent in _headEventList)
		{
			if (headEvent.EventConfig.TriggerType == EventTrigger.SwitchToGuardedPage)
			{
				if (eventArgBox == null)
				{
					eventArgBox = DomainManager.TaiwuEvent.GetEventArgBox();
				}
				eventArgBox.Set("CustomStatus", arg0);
				eventArgBox.Set("CurrentPage", arg1);
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
