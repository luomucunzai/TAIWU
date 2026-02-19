using System;
using System.Collections.Generic;
using Config;
using GameData.Domains.Character;
using GameData.Domains.Character.Filters;
using GameData.Domains.Map;
using GameData.Domains.Organization;
using GameData.Domains.TaiwuEvent.DisplayEvent;
using GameData.Domains.TaiwuEvent.EventHelper;
using GameData.Domains.TaiwuEvent.MonthlyEventActions;
using GameData.Utilities;

namespace GameData.Domains.TaiwuEvent.FunctionDefinition;

public class WorldFunctions
{
	[EventFunction(51)]
	private static void AdvanceDays(EventScriptRuntime runtime, int days)
	{
		DomainManager.World.AdvanceDaysInMonth(runtime.Context, days);
	}

	[EventFunction(52)]
	private static void ChangeMainStoryLineProgress(EventScriptRuntime runtime, short progress)
	{
		DomainManager.World.ChangeMainStoryLineProgress(runtime.Context, progress);
	}

	[EventFunction(53)]
	private static void SetWorldFunctionsStatus(EventScriptRuntime runtime, byte worldFunctionType)
	{
		DomainManager.World.SetWorldFunctionsStatus(runtime.Context, worldFunctionType);
		if (worldFunctionType == 25)
		{
			MonthlyActionKey key = new MonthlyActionKey(2, 0);
			DomainManager.TaiwuEvent.HandleMonthlyAction(runtime.Context, key);
		}
	}

	[EventFunction(68)]
	private static void TriggerExtraTask(EventScriptRuntime runtime, int taskChainId, int taskInfoId)
	{
		DomainManager.Extra.TriggerExtraTask(runtime.Context, taskChainId, taskInfoId);
	}

	[EventFunction(69)]
	private static void FinishExtraTask(EventScriptRuntime runtime, int taskChainId, int taskInfoId)
	{
		DomainManager.Extra.FinishTriggeredExtraTask(runtime.Context, taskChainId, taskInfoId);
	}

	[EventFunction(70)]
	private static void FinishExtraTaskChain(EventScriptRuntime runtime, int taskChainId)
	{
		DomainManager.Extra.FinishAllTaskInChain(runtime.Context, taskChainId);
	}

	[EventFunction(115)]
	private static void TriggerSectMainStoryEndingCountDown(EventScriptRuntime runtime, sbyte orgTemplateId, bool isGoodEnding)
	{
		DomainManager.World.TriggerSectMainStoryEndingCountDown(runtime.Context, orgTemplateId, isGoodEnding);
	}

	[EventFunction(116)]
	private static void SetSectMainStoryEnding(EventScriptRuntime runtime, sbyte orgTemplateId, bool isGoodEnding, int informationIndex, bool addInformation, string nextEvent)
	{
		GameData.Domains.TaiwuEvent.EventHelper.EventHelper.SectMainStoryEnd(orgTemplateId, isGoodEnding, informationIndex, nextEvent, runtime.Current.ArgBox, addInformation);
	}

	[EventFunction(93)]
	private static EventActorData CreateEventActor(EventScriptRuntime runtime, short actorTemplateId)
	{
		return EventActorDataHelper.CreateActor(runtime.Context.Random, actorTemplateId);
	}

	[EventFunction(114)]
	private static int GetIntelligentCharacterByFilter(EventScriptRuntime runtime, short characterFilterRuleId, short areaTemplateId, sbyte searchRangeType, bool createNew)
	{
		List<Predicate<GameData.Domains.Character.Character>> predicates = new List<Predicate<GameData.Domains.Character.Character>>();
		List<GameData.Domains.Character.Character> list = new List<GameData.Domains.Character.Character>();
		GameData.Domains.Character.Filters.CharacterFilterRules.ToPredicates(characterFilterRuleId, predicates, -1);
		short areaIdByAreaTemplateId = DomainManager.Map.GetAreaIdByAreaTemplateId(areaTemplateId);
		switch (searchRangeType)
		{
		case 0:
			MapCharacterFilter.Find(predicates, list, areaIdByAreaTemplateId);
			break;
		case 1:
		{
			List<short> list2 = ObjectPool<List<short>>.Instance.Get();
			sbyte stateIdByAreaId = DomainManager.Map.GetStateIdByAreaId(areaIdByAreaTemplateId);
			DomainManager.Map.GetAllAreaInState(stateIdByAreaId, list2);
			MapCharacterFilter.ParallelFind(predicates, list, list2);
			ObjectPool<List<short>>.Instance.Return(list2);
			break;
		}
		case 2:
			MapCharacterFilter.ParallelFind(predicates, list, 0, 135);
			break;
		}
		if (list.Count > 0)
		{
			return list.GetRandom(runtime.Context.Random).GetId();
		}
		if (!createNew)
		{
			return -1;
		}
		GameData.Domains.Character.Character character = DomainManager.Character.CreateTemporaryIntelligentCharacter(runtime.Context, characterFilterRuleId, DomainManager.Taiwu.GetTaiwu().GetValidLocation());
		DomainManager.Character.ConvertTemporaryIntelligentCharacter(runtime.Context, character);
		return character.GetId();
	}

	[EventFunction(92)]
	private static int CreateEnemyCharacter(EventScriptRuntime runtime, short characterTemplateId, bool adjustByXiangshuLevel)
	{
		CharacterItem characterItem = Config.Character.Instance[characterTemplateId];
		if (adjustByXiangshuLevel && characterItem.GroupId >= 0)
		{
			sbyte consummateLevel = DomainManager.Taiwu.GetTaiwu().GetConsummateLevel();
			short characterTemplateIdInGroup = CharacterDomain.GetCharacterTemplateIdInGroup(characterItem.GroupId, consummateLevel);
			characterItem = Config.Character.Instance[characterTemplateIdInGroup];
		}
		return GameData.Domains.TaiwuEvent.EventHelper.EventHelper.CreateNonIntelligentCharacter(characterItem.TemplateId);
	}

	[EventFunction(99)]
	private static int GetFixedCharacter(EventScriptRuntime runtime, short characterTemplateId)
	{
		return GameData.Domains.TaiwuEvent.EventHelper.EventHelper.GetOrCreateFixedCharacterByTemplateId(characterTemplateId).GetId();
	}

	[EventFunction(100)]
	private static void MoveCharacter(EventScriptRuntime runtime, GameData.Domains.Character.Character character, MapBlockData block)
	{
		int id = character.GetId();
		Location targetLocation = block?.GetLocation() ?? Location.Invalid;
		switch (character.GetCreatingType())
		{
		case 0:
			GameData.Domains.TaiwuEvent.EventHelper.EventHelper.MoveFixedCharacter(character, targetLocation);
			break;
		case 1:
			if (!DomainManager.Character.IsTemporaryIntelligentCharacter(id) && !DomainManager.Taiwu.IsInGroup(id))
			{
				if (targetLocation.IsValid())
				{
					GameData.Domains.TaiwuEvent.EventHelper.EventHelper.MoveIntelligentCharacter(character, targetLocation);
				}
				else
				{
					GameData.Domains.TaiwuEvent.EventHelper.EventHelper.HideIntelligentCharacter(character);
				}
			}
			break;
		case 3:
			GameData.Domains.TaiwuEvent.EventHelper.EventHelper.MoveFixedEnemy(character, targetLocation);
			break;
		case 2:
			break;
		}
	}

	[EventFunction(139)]
	private static void SectStoryZhujianCreateCatchableThief(EventScriptRuntime runtime, short areaTemplateId)
	{
		short areaIdByAreaTemplateId = DomainManager.Map.GetAreaIdByAreaTemplateId(areaTemplateId);
		DomainManager.World.CreateNewThief(runtime.Context, areaIdByAreaTemplateId);
	}

	[EventFunction(150)]
	private static void SectStoryZhujianCreateGearMate(EventScriptRuntime runtime)
	{
		GameData.Domains.Character.Character character = DomainManager.Extra.CreateGearMate(runtime.Context, 836);
		DomainManager.Extra.GearMateJoinGroup(runtime.Context, character.GetId());
	}

	[EventFunction(154)]
	private static int GetOtherSmallSettlement(EventScriptRuntime runtime, short areaTemplateId, Settlement settlement)
	{
		short areaIdByAreaTemplateId = DomainManager.Map.GetAreaIdByAreaTemplateId(areaTemplateId);
		List<short> list = new List<short>();
		DomainManager.Map.GetAreaSettlementIds(areaIdByAreaTemplateId, list);
		foreach (short item in list)
		{
			if (item != settlement.GetId())
			{
				return item;
			}
		}
		throw new InvalidOperationException("no available settlement in current area.");
	}

	[EventFunction(149)]
	private static void AddBuilding(EventScriptRuntime runtime, short blockTemplateId, Settlement settlement, bool forcePlace, bool closeToCenter)
	{
		Location location = settlement.GetLocation();
		DomainManager.Building.PlaceBuildingAtBlock(runtime.Context, location.AreaId, location.BlockId, blockTemplateId, forcePlace, !closeToCenter);
	}

	[EventFunction(151)]
	private static void SectStoryZhujianAddAreaMerchantType(EventScriptRuntime runtime, short areaTemplateId, sbyte merchantType)
	{
		DomainManager.Extra.SetSectZhujianAreaMerchantType(runtime.Context, areaTemplateId, merchantType);
		DomainManager.World.UpdateAreaMerchantType(runtime.Context);
	}

	[EventFunction(152)]
	private static void SectStoryZhujianRemoveAreaMerchantType(EventScriptRuntime runtime, short areaTemplateId)
	{
		DomainManager.Extra.RemoveSectZhujianAreaMerchantType(runtime.Context, areaTemplateId);
	}
}
