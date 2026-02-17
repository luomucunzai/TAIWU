using System;
using System.Collections.Generic;
using Config;
using GameData.Domains.Character;
using GameData.Domains.Map;
using GameData.Domains.Organization;
using GameData.Domains.TaiwuEvent.EventHelper;
using GameData.Utilities;

namespace GameData.Domains.TaiwuEvent.FunctionDefinition
{
	// Token: 0x020000A7 RID: 167
	public class MapFunctions
	{
		// Token: 0x06001B14 RID: 6932 RVA: 0x0017AF85 File Offset: 0x00179185
		[EventFunction(54)]
		private static void ChangeSpiritualDebtByAreaId(EventScriptRuntime runtime, MapAreaData areaData, int changeValue)
		{
			DomainManager.Extra.ChangeAreaSpiritualDebt(runtime.Context, areaData.GetId(), changeValue, true, true);
		}

		// Token: 0x06001B15 RID: 6933 RVA: 0x0017AFA2 File Offset: 0x001791A2
		[EventFunction(57)]
		private static void SetBlockAndViewRangeVisible(EventScriptRuntime runtime, MapBlockData mapBlockData)
		{
			DomainManager.Map.SetBlockAndViewRangeVisible(runtime.Context, mapBlockData.AreaId, mapBlockData.BlockId);
		}

		// Token: 0x06001B16 RID: 6934 RVA: 0x0017AFC2 File Offset: 0x001791C2
		[EventFunction(89)]
		private static void StartCombat(EventScriptRuntime runtime, GameData.Domains.Character.Character targetChar, short combatConfigId, string onFinishEvent, bool noGuard)
		{
			EventHelper.StartCombat(targetChar.GetId(), combatConfigId, onFinishEvent, runtime.Current.ArgBox, noGuard);
		}

		// Token: 0x06001B17 RID: 6935 RVA: 0x0017AFE0 File Offset: 0x001791E0
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

		// Token: 0x06001B18 RID: 6936 RVA: 0x0017B02C File Offset: 0x0017922C
		[EventFunction(167)]
		private static MapBlockData FilterMapBlockInRange(EventScriptRuntime runtime, MapBlockData mapBlockData, int minDistance, int maxDistance, short matcherTemplateId)
		{
			List<MapBlockData> blockDataList = ObjectPool<List<MapBlockData>>.Instance.Get();
			DomainManager.Map.GetLocationByDistance(mapBlockData.GetLocation(), minDistance, maxDistance, ref blockDataList);
			MapBlockMatcherItem matcher = MapBlockMatcher.Instance[matcherTemplateId];
			for (int i = blockDataList.Count - 1; i >= 0; i--)
			{
				bool flag = !matcher.Match(blockDataList[i]);
				if (flag)
				{
					CollectionUtils.SwapAndRemove<MapBlockData>(blockDataList, i);
				}
			}
			MapBlockData selectedBlock = blockDataList.GetRandomOrDefault(runtime.Context.Random, null);
			ObjectPool<List<MapBlockData>>.Instance.Return(blockDataList);
			return selectedBlock;
		}

		// Token: 0x06001B19 RID: 6937 RVA: 0x0017B0C8 File Offset: 0x001792C8
		[EventFunction(199)]
		private static MapBlockData GetSettlementMapBlock(EventScriptRuntime runtime, Settlement settlement)
		{
			Location location = settlement.GetLocation();
			return DomainManager.Map.GetBlock(location);
		}

		// Token: 0x06001B1A RID: 6938 RVA: 0x0017B0EC File Offset: 0x001792EC
		[EventFunction(198)]
		private static MapBlockData GetCharacterCurrentMapBlock(EventScriptRuntime runtime, GameData.Domains.Character.Character character)
		{
			Location location = character.GetValidLocation();
			return DomainManager.Map.GetBlock(location);
		}

		// Token: 0x06001B1B RID: 6939 RVA: 0x0017B110 File Offset: 0x00179310
		[EventFunction(261)]
		private static MapAreaData GetCharacterCurrentMapArea(EventScriptRuntime runtime, GameData.Domains.Character.Character character)
		{
			Location location = character.GetValidLocation();
			return DomainManager.Map.GetElement_Areas((int)location.AreaId);
		}

		// Token: 0x06001B1C RID: 6940 RVA: 0x0017B13C File Offset: 0x0017933C
		[EventFunction(262)]
		private static MapAreaData GetSettlementMapArea(EventScriptRuntime runtime, Settlement settlement)
		{
			Location location = settlement.GetLocation();
			return DomainManager.Map.GetElement_Areas((int)location.AreaId);
		}
	}
}
