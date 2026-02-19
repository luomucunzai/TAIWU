using System;
using System.Collections.Generic;
using Config;
using Config.EventConfig;
using GameData.Domains.Adventure;
using GameData.Domains.TaiwuEvent.Enum;
using GameData.Domains.TaiwuEvent.EventHelper;
using GameData.Serializer;
using GameData.Utilities;
using NLog;

namespace GameData.Domains.TaiwuEvent.EventManager;

internal class AdventureEventManager : EventManagerBase
{
	private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

	private Dictionary<string, TaiwuEvent> _eventDictionary;

	private Dictionary<string, TaiwuEvent> _monitoringEventDictionary;

	private AdventureItem _adventureConfig;

	private EventArgBox _argBox;

	public AdventureEventManager()
	{
		_eventDictionary = new Dictionary<string, TaiwuEvent>();
		_monitoringEventDictionary = new Dictionary<string, TaiwuEvent>();
	}

	public void SetNewAdventureByConfigData(AdventureItem configData, EventArgBox argBox)
	{
		_adventureConfig = configData;
		_argBox = argBox;
		Logger.Info("SetNewAdventureByConfigData: " + configData.Name);
		_monitoringEventDictionary.Clear();
		for (int i = 0; i < _adventureConfig.StartNodes.Count; i++)
		{
			AddEvent(_adventureConfig.StartNodes[i].EventId.ToString());
		}
		for (int j = 0; j < _adventureConfig.TransferNodes.Count; j++)
		{
			AddEvent(_adventureConfig.TransferNodes[j].EventId.ToString());
		}
		for (int k = 0; k < _adventureConfig.EndNodes.Count; k++)
		{
			AddEvent(_adventureConfig.EndNodes[k].EventId.ToString());
		}
		foreach (AdventureBaseBranch baseBranch in _adventureConfig.BaseBranches)
		{
			AddEvent(baseBranch.GlobalEvent);
			AdventurePersonalityContentWeights[] personalityContentWeights = baseBranch.PersonalityContentWeights;
			foreach (AdventurePersonalityContentWeights adventurePersonalityContentWeights in personalityContentWeights)
			{
				(string, short)[] eventWeights = adventurePersonalityContentWeights.EventWeights;
				for (int m = 0; m < eventWeights.Length; m++)
				{
					(string, short) tuple = eventWeights[m];
					AddEvent(tuple.Item1);
				}
				(string, short)[] bonusWeights = adventurePersonalityContentWeights.BonusWeights;
				for (int n = 0; n < bonusWeights.Length; n++)
				{
					(string, short) tuple2 = bonusWeights[n];
					AddEvent(tuple2.Item1);
				}
			}
		}
		foreach (AdventureAdvancedBranch advancedBranch in _adventureConfig.AdvancedBranches)
		{
			AddEvent(advancedBranch.GlobalEvent);
			AdventurePersonalityContentWeights[] personalityContentWeights2 = advancedBranch.PersonalityContentWeights;
			foreach (AdventurePersonalityContentWeights adventurePersonalityContentWeights2 in personalityContentWeights2)
			{
				(string, short)[] eventWeights2 = adventurePersonalityContentWeights2.EventWeights;
				for (int num2 = 0; num2 < eventWeights2.Length; num2++)
				{
					(string, short) tuple3 = eventWeights2[num2];
					AddEvent(tuple3.Item1);
				}
				(string, short)[] bonusWeights2 = adventurePersonalityContentWeights2.BonusWeights;
				for (int num3 = 0; num3 < bonusWeights2.Length; num3++)
				{
					(string, short) tuple4 = bonusWeights2[num3];
					AddEvent(tuple4.Item1);
				}
			}
		}
		for (int num4 = 0; num4 < _adventureConfig.AdventureParams.Count; num4++)
		{
			string item = _adventureConfig.AdventureParams[num4].Item1;
			if (!string.IsNullOrEmpty(item))
			{
				GameData.Domains.TaiwuEvent.EventHelper.EventHelper.SetAdventureParameter(item, 0);
			}
		}
		Logger.Info("Trigger enter event: " + _adventureConfig.EnterEvent);
		TaiwuEvent taiwuEvent = GetEvent(_adventureConfig.EnterEvent);
		if (taiwuEvent == null)
		{
			throw new Exception("Failed to find enter event " + _adventureConfig.EnterEvent + " of adventure " + _adventureConfig.Name);
		}
		taiwuEvent.ArgBox = _argBox;
		if (taiwuEvent.EventConfig.CheckCondition())
		{
			DomainManager.TaiwuEvent.AddTriggeredEvent(taiwuEvent);
		}
	}

	public void TriggerGlobalEvent(AdventureMapPoint curNode)
	{
		int branchIndex = DomainManager.Adventure.GetCurMapTrunk().BranchIndex;
		string text = ((branchIndex < _adventureConfig.BaseBranches.Count) ? _adventureConfig.BaseBranches[branchIndex].GlobalEvent : _adventureConfig.AdvancedBranches[branchIndex - _adventureConfig.BaseBranches.Count].GlobalEvent);
		if (!string.IsNullOrEmpty(text))
		{
			TaiwuEvent taiwuEvent = GetEvent(text);
			_argBox.Set("AdventurePoint", (ISerializableGameData)(object)curNode);
			taiwuEvent.ArgBox = _argBox;
			if (taiwuEvent.EventConfig.CheckCondition())
			{
				DomainManager.TaiwuEvent.AddTriggeredEvent(taiwuEvent);
			}
		}
	}

	public void TriggerExtraEvent(string guid, AdventureMapPoint curNode)
	{
		if (curNode.NodeContentType != 10)
		{
			throw new Exception($"Incorrect node content type for TriggerExtraEvent: {curNode.NodeContentType} given.");
		}
		if (!string.IsNullOrEmpty(guid))
		{
			TaiwuEvent taiwuEvent = GetEvent(guid);
			if (taiwuEvent.EventConfig.TriggerType != EventTrigger.AdventureEnterNode)
			{
				throw new Exception($"Incorrect event trigger type for TriggerExtraEvent with event {guid}: {taiwuEvent.EventConfig.TriggerType} given.");
			}
			_argBox.Set("AdventurePoint", (ISerializableGameData)(object)curNode);
			taiwuEvent.ArgBox = _argBox;
			if (taiwuEvent.EventConfig.CheckCondition())
			{
				DomainManager.TaiwuEvent.AddTriggeredEvent(taiwuEvent);
			}
		}
	}

	public void SetNewAdventure(short adventureId, EventArgBox argBox)
	{
		_adventureConfig = Config.Adventure.Instance.GetItem(adventureId);
		if (_adventureConfig == null)
		{
			throw new Exception($"failed get adventure config of id {adventureId}");
		}
		Logger.Info($"SetNewAdventure: {adventureId}_{_adventureConfig.Name}");
		_argBox = argBox;
		_monitoringEventDictionary.Clear();
		for (int i = 0; i < _adventureConfig.StartNodes.Count; i++)
		{
			AddEvent(_adventureConfig.StartNodes[i].EventId.ToString());
		}
		for (int j = 0; j < _adventureConfig.TransferNodes.Count; j++)
		{
			AddEvent(_adventureConfig.TransferNodes[j].EventId.ToString());
		}
		for (int k = 0; k < _adventureConfig.EndNodes.Count; k++)
		{
			AddEvent(_adventureConfig.EndNodes[k].EventId.ToString());
		}
		foreach (AdventureBaseBranch baseBranch in _adventureConfig.BaseBranches)
		{
			AddEvent(baseBranch.GlobalEvent);
			AdventurePersonalityContentWeights[] personalityContentWeights = baseBranch.PersonalityContentWeights;
			foreach (AdventurePersonalityContentWeights adventurePersonalityContentWeights in personalityContentWeights)
			{
				(string, short)[] eventWeights = adventurePersonalityContentWeights.EventWeights;
				for (int m = 0; m < eventWeights.Length; m++)
				{
					(string, short) tuple = eventWeights[m];
					AddEvent(tuple.Item1);
				}
				(string, short)[] bonusWeights = adventurePersonalityContentWeights.BonusWeights;
				for (int n = 0; n < bonusWeights.Length; n++)
				{
					(string, short) tuple2 = bonusWeights[n];
					AddEvent(tuple2.Item1);
				}
			}
		}
		foreach (AdventureAdvancedBranch advancedBranch in _adventureConfig.AdvancedBranches)
		{
			AddEvent(advancedBranch.GlobalEvent);
			AdventurePersonalityContentWeights[] personalityContentWeights2 = advancedBranch.PersonalityContentWeights;
			foreach (AdventurePersonalityContentWeights adventurePersonalityContentWeights2 in personalityContentWeights2)
			{
				(string, short)[] eventWeights2 = adventurePersonalityContentWeights2.EventWeights;
				for (int num2 = 0; num2 < eventWeights2.Length; num2++)
				{
					(string, short) tuple3 = eventWeights2[num2];
					AddEvent(tuple3.Item1);
				}
				(string, short)[] bonusWeights2 = adventurePersonalityContentWeights2.BonusWeights;
				for (int num3 = 0; num3 < bonusWeights2.Length; num3++)
				{
					(string, short) tuple4 = bonusWeights2[num3];
					AddEvent(tuple4.Item1);
				}
			}
		}
		for (int num4 = 0; num4 < _adventureConfig.AdventureParams.Count; num4++)
		{
			string item = _adventureConfig.AdventureParams[num4].Item1;
			if (!string.IsNullOrEmpty(item))
			{
				GameData.Domains.TaiwuEvent.EventHelper.EventHelper.SetAdventureParameter(item, 0);
			}
		}
		Logger.Info("Trigger enter event: " + _adventureConfig.EnterEvent);
		TaiwuEvent taiwuEvent = GetEvent(_adventureConfig.EnterEvent);
		if (taiwuEvent == null)
		{
			throw new Exception("Failed to find enter event " + _adventureConfig.EnterEvent + " of adventure " + _adventureConfig.Name);
		}
		taiwuEvent.ArgBox = _argBox;
		if (taiwuEvent.EventConfig.CheckCondition())
		{
			DomainManager.TaiwuEvent.AddTriggeredEvent(taiwuEvent);
		}
	}

	public void AddEvent(string eventGuid)
	{
		if (!string.IsNullOrEmpty(eventGuid) && !_monitoringEventDictionary.ContainsKey(eventGuid))
		{
			TaiwuEvent taiwuEvent = GetEvent(eventGuid);
			if (taiwuEvent != null)
			{
				_monitoringEventDictionary.Add(taiwuEvent.EventGuid, taiwuEvent);
			}
		}
	}

	public override TaiwuEvent GetEvent(string eventGuid)
	{
		_eventDictionary.TryGetValue(eventGuid, out var value);
		return value;
	}

	public override void HandleEventPackage(EventPackage package)
	{
		List<TaiwuEventItem> eventsByType = package.GetEventsByType(EEventType.AdventureEvent);
		for (int i = 0; i < eventsByType.Count; i++)
		{
			TaiwuEvent taiwuEvent = new TaiwuEvent
			{
				EventGuid = eventsByType[i].Guid.ToString(),
				EventConfig = eventsByType[i],
				ExtendEventOptions = new List<(string, string)>()
			};
			if (!_eventDictionary.ContainsKey(eventsByType[i].Guid.ToString()))
			{
				_eventDictionary.Add(eventsByType[i].Guid.ToString(), taiwuEvent);
			}
			else
			{
				_eventDictionary[taiwuEvent.EventGuid] = taiwuEvent;
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
		_monitoringEventDictionary.Clear();
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
			_eventDictionary.Remove(item2);
			_monitoringEventDictionary.Remove(item2);
		}
	}

	public override void OnEventTrigger_AdventureReachStartNode(short arg0)
	{
		AdventureStartNode adventureStartNode = _adventureConfig.StartNodes[arg0];
		_argBox.Set("AdventureNodeKey", adventureStartNode.NodeKey);
		if (_monitoringEventDictionary.TryGetValue(adventureStartNode.EventId.ToString(), out var value))
		{
			if (value.EventConfig.TriggerType != EventTrigger.AdventureReachStartNode)
			{
				throw new Exception($"Incorrect event trigger type for EventTrigger_AdventureReachStartNode with event {value.EventGuid}: {value.EventConfig.TriggerType} given.");
			}
			value.ArgBox = _argBox;
			if (value.EventConfig.CheckCondition())
			{
				DomainManager.TaiwuEvent.AddTriggeredEvent(value);
			}
		}
	}

	public override void OnEventTrigger_AdventureReachTransferNode(short arg0)
	{
		int count = _adventureConfig.StartNodes.Count;
		AdventureTransferNode adventureTransferNode = _adventureConfig.TransferNodes[arg0 - count];
		_argBox.Set("AdventureNodeKey", adventureTransferNode.NodeKey);
		if (_monitoringEventDictionary.TryGetValue(adventureTransferNode.EventId.ToString(), out var value))
		{
			if (value.EventConfig.TriggerType != EventTrigger.AdventureReachTransferNode)
			{
				throw new Exception($"Incorrect event trigger type for EventTrigger_AdventureReachTransferNode with event {value.EventGuid}: {value.EventConfig.TriggerType} given.");
			}
			value.ArgBox = _argBox;
			if (value.EventConfig.CheckCondition())
			{
				DomainManager.TaiwuEvent.AddTriggeredEvent(value);
			}
		}
	}

	public override void OnEventTrigger_AdventureReachEndNode(short arg0)
	{
		int count = _adventureConfig.StartNodes.Count;
		int count2 = _adventureConfig.TransferNodes.Count;
		AdventureEndNode adventureEndNode = _adventureConfig.EndNodes[arg0 - count - count2];
		_argBox.Set("AdventureNodeKey", adventureEndNode.NodeKey);
		if (_monitoringEventDictionary.TryGetValue(adventureEndNode.EventId.ToString(), out var value))
		{
			if (value.EventConfig.TriggerType != EventTrigger.AdventureReachEndNode)
			{
				throw new Exception($"Incorrect event trigger type for EventTrigger_AdventureReachEndNode with event {value.EventGuid}: {value.EventConfig.TriggerType} given.");
			}
			value.ArgBox = _argBox;
			if (value.EventConfig.CheckCondition())
			{
				DomainManager.TaiwuEvent.AddTriggeredEvent(value);
			}
		}
	}

	public override void OnEventTrigger_AdventureEnterNode(AdventureMapPoint arg0)
	{
		_argBox.Set("AdventurePoint", (ISerializableGameData)(object)arg0);
		int nodeContentIndex = arg0.NodeContentIndex;
		int affiliatedBranchIdx = arg0.AffiliatedBranchIdx;
		AdventurePersonalityContentWeights[] array = ((affiliatedBranchIdx < _adventureConfig.BaseBranches.Count) ? _adventureConfig.BaseBranches[affiliatedBranchIdx].PersonalityContentWeights : _adventureConfig.AdvancedBranches[affiliatedBranchIdx - _adventureConfig.BaseBranches.Count].PersonalityContentWeights);
		if (!array.CheckIndex(arg0.SevenElementType))
		{
			throw new Exception($"Invalid seven element type {arg0.SevenElementType} of node {arg0.GetDetailedInfo()}");
		}
		AdventurePersonalityContentWeights adventurePersonalityContentWeights = array[arg0.SevenElementType];
		sbyte nodeContentType = arg0.NodeContentType;
		if (1 == 0)
		{
		}
		(string, short)[] array2 = nodeContentType switch
		{
			0 => adventurePersonalityContentWeights.EventWeights, 
			3 => adventurePersonalityContentWeights.BonusWeights, 
			_ => throw new Exception($"Incorrect node content type for EventTrigger_AdventureEnterNode: {arg0.NodeContentType} given."), 
		};
		if (1 == 0)
		{
		}
		(string, short)[] array3 = array2;
		if (!array3.CheckIndex(nodeContentIndex))
		{
			throw new Exception($"Invalid eventIndex {nodeContentIndex} of node {arg0.GetDetailedInfo()}");
		}
		string item = array3[nodeContentIndex].Item1;
		if (_monitoringEventDictionary.TryGetValue(item, out var value))
		{
			if (value.EventConfig.TriggerType != EventTrigger.AdventureEnterNode)
			{
				throw new Exception($"Incorrect event trigger type for EventTrigger_AdventureEnterNode with event {item}: {value.EventConfig.TriggerType} given.");
			}
			value.ArgBox = _argBox;
			if (value.EventConfig.CheckCondition())
			{
				DomainManager.TaiwuEvent.AddTriggeredEvent(value);
			}
		}
	}

	public override void OnEventTrigger_CombatOpening(int arg0)
	{
		if (_argBox == null || _monitoringEventDictionary.Count <= 0)
		{
			return;
		}
		_argBox.Set("CharacterId", arg0);
		foreach (KeyValuePair<string, TaiwuEvent> item in _monitoringEventDictionary)
		{
			if (item.Value.EventConfig.TriggerType == EventTrigger.CombatOpening)
			{
				item.Value.ArgBox = _argBox;
				if (item.Value.EventConfig.CheckCondition())
				{
					DomainManager.TaiwuEvent.AddTriggeredEvent(item.Value);
				}
				else
				{
					item.Value.ArgBox = null;
				}
			}
		}
	}
}
