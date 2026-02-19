using System.Collections.Generic;
using Config;
using GameData.Domains.Character;
using GameData.Domains.Map;
using GameData.Domains.Organization;
using GameData.Domains.TaiwuEvent.EventHelper;
using GameData.Utilities;

namespace GameData.Domains.TaiwuEvent.FunctionDefinition;

public class MapFunctions
{
	[EventFunction(54)]
	private static void ChangeSpiritualDebtByAreaId(EventScriptRuntime runtime, MapAreaData areaData, int changeValue)
	{
		DomainManager.Extra.ChangeAreaSpiritualDebt(runtime.Context, areaData.GetId(), changeValue);
	}

	[EventFunction(57)]
	private static void SetBlockAndViewRangeVisible(EventScriptRuntime runtime, MapBlockData mapBlockData)
	{
		DomainManager.Map.SetBlockAndViewRangeVisible(runtime.Context, mapBlockData.AreaId, mapBlockData.BlockId);
	}

	[EventFunction(89)]
	private static void StartCombat(EventScriptRuntime runtime, GameData.Domains.Character.Character targetChar, short combatConfigId, string onFinishEvent, bool noGuard)
	{
		GameData.Domains.TaiwuEvent.EventHelper.EventHelper.StartCombat(targetChar.GetId(), combatConfigId, onFinishEvent, runtime.Current.ArgBox, noGuard);
	}

	[EventFunction(184)]
	private static void SwitchEmeiBlood(EventScriptRuntime runtime, MapBlockData mapBlockData, bool isOn)
	{
		Location location = new Location(mapBlockData.AreaId, mapBlockData.BlockId);
		if (isOn)
		{
			DomainManager.Extra.TurnOnEmeiBlood(runtime.Context, location);
		}
		else
		{
			DomainManager.Extra.TurnOffEmeiBlood(runtime.Context, location);
		}
	}

	[EventFunction(167)]
	private static MapBlockData FilterMapBlockInRange(EventScriptRuntime runtime, MapBlockData mapBlockData, int minDistance, int maxDistance, short matcherTemplateId)
	{
		List<MapBlockData> mapBlockList = ObjectPool<List<MapBlockData>>.Instance.Get();
		DomainManager.Map.GetLocationByDistance(mapBlockData.GetLocation(), minDistance, maxDistance, ref mapBlockList);
		MapBlockMatcherItem matcherItem = MapBlockMatcher.Instance[matcherTemplateId];
		for (int num = mapBlockList.Count - 1; num >= 0; num--)
		{
			if (!matcherItem.Match(mapBlockList[num]))
			{
				CollectionUtils.SwapAndRemove(mapBlockList, num);
			}
		}
		MapBlockData randomOrDefault = mapBlockList.GetRandomOrDefault(runtime.Context.Random, null);
		ObjectPool<List<MapBlockData>>.Instance.Return(mapBlockList);
		return randomOrDefault;
	}

	[EventFunction(199)]
	private static MapBlockData GetSettlementMapBlock(EventScriptRuntime runtime, Settlement settlement)
	{
		Location location = settlement.GetLocation();
		return DomainManager.Map.GetBlock(location);
	}

	[EventFunction(198)]
	private static MapBlockData GetCharacterCurrentMapBlock(EventScriptRuntime runtime, GameData.Domains.Character.Character character)
	{
		Location validLocation = character.GetValidLocation();
		return DomainManager.Map.GetBlock(validLocation);
	}

	[EventFunction(261)]
	private static MapAreaData GetCharacterCurrentMapArea(EventScriptRuntime runtime, GameData.Domains.Character.Character character)
	{
		Location validLocation = character.GetValidLocation();
		return DomainManager.Map.GetElement_Areas(validLocation.AreaId);
	}

	[EventFunction(262)]
	private static MapAreaData GetSettlementMapArea(EventScriptRuntime runtime, Settlement settlement)
	{
		Location location = settlement.GetLocation();
		return DomainManager.Map.GetElement_Areas(location.AreaId);
	}
}
