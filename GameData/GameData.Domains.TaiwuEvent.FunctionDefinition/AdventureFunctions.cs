using System.Collections.Generic;
using Config;
using GameData.Domains.Map;
using GameData.Domains.TaiwuEvent.MonthlyEventActions;
using GameData.GameDataBridge;
using GameData.Utilities;

namespace GameData.Domains.TaiwuEvent.FunctionDefinition;

public class AdventureFunctions
{
	private static HashSet<short> _adventureIdWithMonthlyActionSet;

	[EventFunction(145)]
	private static void CreateConfigMonthlyAction(EventScriptRuntime runtime, short monthlyActionId, short areaTemplateId)
	{
		short areaIdByAreaTemplateId = DomainManager.Map.GetAreaIdByAreaTemplateId(areaTemplateId);
		DomainManager.TaiwuEvent.AddWrappedConfigAction(runtime.Context, monthlyActionId, areaIdByAreaTemplateId);
	}

	[EventFunction(67)]
	private static void CreateAdventureSite(EventScriptRuntime runtime, short adventureId, MapBlockData mapBlockData)
	{
		Location location = mapBlockData?.GetLocation() ?? DomainManager.Taiwu.GetTaiwu().GetValidLocation();
		InitializeAdventureIdToMonthlyActionMap();
		if (_adventureIdWithMonthlyActionSet.Contains(adventureId))
		{
			AdaptableLog.Warning(Config.Adventure.Instance[adventureId].Name + " should be created with a monthly action. Use instead.");
		}
		DomainManager.Adventure.TryCreateAdventureSite(runtime.Context, location.AreaId, location.BlockId, adventureId, MonthlyActionKey.Invalid);
	}

	private static void InitializeAdventureIdToMonthlyActionMap()
	{
		if (_adventureIdWithMonthlyActionSet != null)
		{
			return;
		}
		_adventureIdWithMonthlyActionSet = new HashSet<short>();
		foreach (MonthlyActionsItem item in (IEnumerable<MonthlyActionsItem>)MonthlyActions.Instance)
		{
			if (item.AdventureId >= 0)
			{
				_adventureIdWithMonthlyActionSet.Add(item.AdventureId);
			}
		}
	}

	[EventFunction(119)]
	private static void GenerateAdventureMap(EventScriptRuntime runtime, string startNodeKey)
	{
		DomainManager.Adventure.GenerateAdventureMap(runtime.Context, startNodeKey);
	}

	[EventFunction(124)]
	private static void ExitAdventure(EventScriptRuntime runtime, bool isAdventureCompleted, string postAdventureEvent = null)
	{
		short curAdventureId = DomainManager.Adventure.GetCurAdventureId();
		if (curAdventureId >= 0)
		{
			DomainManager.Adventure.ExitAdventure(runtime.Context, isAdventureCompleted);
			if (!string.IsNullOrEmpty(postAdventureEvent))
			{
				EventArgBox eventArgBox = DomainManager.TaiwuEvent.GetEventArgBox();
				eventArgBox.Set("AdventureId", curAdventureId);
				eventArgBox.Set("IsComplete", isAdventureCompleted);
				DomainManager.TaiwuEvent.SetListenerWithActionName(postAdventureEvent, eventArgBox, "ExitAdventure");
			}
			GameData.GameDataBridge.GameDataBridge.AddDisplayEvent(DisplayEventType.ExitAdventure);
		}
	}

	[EventFunction(125)]
	private static void FinishAdventureEvent(EventScriptRuntime runtime)
	{
		DomainManager.Adventure.OnEventHandleFinished(runtime.Context);
	}

	[EventFunction(126)]
	private static void SelectAdventureBranch(EventScriptRuntime runtime, string branchKey)
	{
		DomainManager.Adventure.SelectBranch(runtime.Context, branchKey);
	}

	[EventFunction(144)]
	private static int GetAdventureCharacterCount(EventScriptRuntime runtime, bool isMajor, int groupId)
	{
		return isMajor ? runtime.Current.ArgBox.GetAdventureMajorCharacterCount(groupId) : runtime.Current.ArgBox.GetAdventureParticipateCharacterCount(groupId);
	}

	[EventFunction(143)]
	private static int GetAdventureCharacter(EventScriptRuntime runtime, bool isMajor, int groupId, int index)
	{
		return isMajor ? runtime.Current.ArgBox.GetAdventureMajorCharacter(groupId, index).GetId() : runtime.Current.ArgBox.GetAdventureParticipateCharacter(groupId, index).GetId();
	}
}
