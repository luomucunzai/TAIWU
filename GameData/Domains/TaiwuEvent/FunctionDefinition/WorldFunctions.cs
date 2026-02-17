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

namespace GameData.Domains.TaiwuEvent.FunctionDefinition
{
	// Token: 0x020000AC RID: 172
	public class WorldFunctions
	{
		// Token: 0x06001B40 RID: 6976 RVA: 0x0017B850 File Offset: 0x00179A50
		[EventFunction(51)]
		private static void AdvanceDays(EventScriptRuntime runtime, int days)
		{
			DomainManager.World.AdvanceDaysInMonth(runtime.Context, days);
		}

		// Token: 0x06001B41 RID: 6977 RVA: 0x0017B865 File Offset: 0x00179A65
		[EventFunction(52)]
		private static void ChangeMainStoryLineProgress(EventScriptRuntime runtime, short progress)
		{
			DomainManager.World.ChangeMainStoryLineProgress(runtime.Context, progress);
		}

		// Token: 0x06001B42 RID: 6978 RVA: 0x0017B87C File Offset: 0x00179A7C
		[EventFunction(53)]
		private static void SetWorldFunctionsStatus(EventScriptRuntime runtime, byte worldFunctionType)
		{
			DomainManager.World.SetWorldFunctionsStatus(runtime.Context, worldFunctionType);
			bool flag = worldFunctionType == 25;
			if (flag)
			{
				MonthlyActionKey key = new MonthlyActionKey(2, 0);
				DomainManager.TaiwuEvent.HandleMonthlyAction(runtime.Context, key);
			}
		}

		// Token: 0x06001B43 RID: 6979 RVA: 0x0017B8C2 File Offset: 0x00179AC2
		[EventFunction(68)]
		private static void TriggerExtraTask(EventScriptRuntime runtime, int taskChainId, int taskInfoId)
		{
			DomainManager.Extra.TriggerExtraTask(runtime.Context, taskChainId, taskInfoId);
		}

		// Token: 0x06001B44 RID: 6980 RVA: 0x0017B8D8 File Offset: 0x00179AD8
		[EventFunction(69)]
		private static void FinishExtraTask(EventScriptRuntime runtime, int taskChainId, int taskInfoId)
		{
			DomainManager.Extra.FinishTriggeredExtraTask(runtime.Context, taskChainId, taskInfoId);
		}

		// Token: 0x06001B45 RID: 6981 RVA: 0x0017B8EE File Offset: 0x00179AEE
		[EventFunction(70)]
		private static void FinishExtraTaskChain(EventScriptRuntime runtime, int taskChainId)
		{
			DomainManager.Extra.FinishAllTaskInChain(runtime.Context, taskChainId);
		}

		// Token: 0x06001B46 RID: 6982 RVA: 0x0017B903 File Offset: 0x00179B03
		[EventFunction(115)]
		private static void TriggerSectMainStoryEndingCountDown(EventScriptRuntime runtime, sbyte orgTemplateId, bool isGoodEnding)
		{
			DomainManager.World.TriggerSectMainStoryEndingCountDown(runtime.Context, orgTemplateId, isGoodEnding);
		}

		// Token: 0x06001B47 RID: 6983 RVA: 0x0017B919 File Offset: 0x00179B19
		[EventFunction(116)]
		private static void SetSectMainStoryEnding(EventScriptRuntime runtime, sbyte orgTemplateId, bool isGoodEnding, int informationIndex, bool addInformation, string nextEvent)
		{
			EventHelper.SectMainStoryEnd(orgTemplateId, isGoodEnding, informationIndex, nextEvent, runtime.Current.ArgBox, addInformation);
		}

		// Token: 0x06001B48 RID: 6984 RVA: 0x0017B934 File Offset: 0x00179B34
		[EventFunction(93)]
		private static EventActorData CreateEventActor(EventScriptRuntime runtime, short actorTemplateId)
		{
			return EventActorDataHelper.CreateActor(runtime.Context.Random, actorTemplateId);
		}

		// Token: 0x06001B49 RID: 6985 RVA: 0x0017B95C File Offset: 0x00179B5C
		[EventFunction(114)]
		private static int GetIntelligentCharacterByFilter(EventScriptRuntime runtime, short characterFilterRuleId, short areaTemplateId, sbyte searchRangeType, bool createNew)
		{
			List<Predicate<GameData.Domains.Character.Character>> predicates = new List<Predicate<GameData.Domains.Character.Character>>();
			List<GameData.Domains.Character.Character> foundCharacters = new List<GameData.Domains.Character.Character>();
			GameData.Domains.Character.Filters.CharacterFilterRules.ToPredicates(characterFilterRuleId, predicates, -1);
			short areaId = DomainManager.Map.GetAreaIdByAreaTemplateId(areaTemplateId);
			switch (searchRangeType)
			{
			case 0:
				MapCharacterFilter.Find(predicates, foundCharacters, areaId, false);
				break;
			case 1:
			{
				List<short> areaList = ObjectPool<List<short>>.Instance.Get();
				sbyte curStateId = DomainManager.Map.GetStateIdByAreaId(areaId);
				DomainManager.Map.GetAllAreaInState(curStateId, areaList);
				MapCharacterFilter.ParallelFind(predicates, foundCharacters, areaList, false);
				ObjectPool<List<short>>.Instance.Return(areaList);
				break;
			}
			case 2:
				MapCharacterFilter.ParallelFind(predicates, foundCharacters, 0, 135, false);
				break;
			}
			bool flag = foundCharacters.Count > 0;
			int result;
			if (flag)
			{
				result = foundCharacters.GetRandom(runtime.Context.Random).GetId();
			}
			else
			{
				bool flag2 = !createNew;
				if (flag2)
				{
					result = -1;
				}
				else
				{
					GameData.Domains.Character.Character character = DomainManager.Character.CreateTemporaryIntelligentCharacter(runtime.Context, characterFilterRuleId, DomainManager.Taiwu.GetTaiwu().GetValidLocation());
					DomainManager.Character.ConvertTemporaryIntelligentCharacter(runtime.Context, character);
					result = character.GetId();
				}
			}
			return result;
		}

		// Token: 0x06001B4A RID: 6986 RVA: 0x0017BA80 File Offset: 0x00179C80
		[EventFunction(92)]
		private static int CreateEnemyCharacter(EventScriptRuntime runtime, short characterTemplateId, bool adjustByXiangshuLevel)
		{
			CharacterItem template = Config.Character.Instance[characterTemplateId];
			bool flag = adjustByXiangshuLevel && template.GroupId >= 0;
			if (flag)
			{
				sbyte consummateLevel = DomainManager.Taiwu.GetTaiwu().GetConsummateLevel();
				short adjustedTemplateId = CharacterDomain.GetCharacterTemplateIdInGroup(template.GroupId, consummateLevel);
				template = Config.Character.Instance[adjustedTemplateId];
			}
			return EventHelper.CreateNonIntelligentCharacter(template.TemplateId);
		}

		// Token: 0x06001B4B RID: 6987 RVA: 0x0017BAEC File Offset: 0x00179CEC
		[EventFunction(99)]
		private static int GetFixedCharacter(EventScriptRuntime runtime, short characterTemplateId)
		{
			return EventHelper.GetOrCreateFixedCharacterByTemplateId(characterTemplateId).GetId();
		}

		// Token: 0x06001B4C RID: 6988 RVA: 0x0017BB0C File Offset: 0x00179D0C
		[EventFunction(100)]
		private static void MoveCharacter(EventScriptRuntime runtime, GameData.Domains.Character.Character character, MapBlockData block)
		{
			int charId = character.GetId();
			Location location = (block != null) ? block.GetLocation() : Location.Invalid;
			switch (character.GetCreatingType())
			{
			case 0:
				EventHelper.MoveFixedCharacter(character, location);
				break;
			case 1:
			{
				bool flag = DomainManager.Character.IsTemporaryIntelligentCharacter(charId);
				if (!flag)
				{
					bool flag2 = DomainManager.Taiwu.IsInGroup(charId);
					if (!flag2)
					{
						bool flag3 = location.IsValid();
						if (flag3)
						{
							EventHelper.MoveIntelligentCharacter(character, location);
						}
						else
						{
							EventHelper.HideIntelligentCharacter(character);
						}
					}
				}
				break;
			}
			case 3:
				EventHelper.MoveFixedEnemy(character, location);
				break;
			}
		}

		// Token: 0x06001B4D RID: 6989 RVA: 0x0017BBB0 File Offset: 0x00179DB0
		[EventFunction(139)]
		private static void SectStoryZhujianCreateCatchableThief(EventScriptRuntime runtime, short areaTemplateId)
		{
			short areaId = DomainManager.Map.GetAreaIdByAreaTemplateId(areaTemplateId);
			DomainManager.World.CreateNewThief(runtime.Context, areaId);
		}

		// Token: 0x06001B4E RID: 6990 RVA: 0x0017BBDC File Offset: 0x00179DDC
		[EventFunction(150)]
		private static void SectStoryZhujianCreateGearMate(EventScriptRuntime runtime)
		{
			GameData.Domains.Character.Character character = DomainManager.Extra.CreateGearMate(runtime.Context, 836, null);
			DomainManager.Extra.GearMateJoinGroup(runtime.Context, character.GetId());
		}

		// Token: 0x06001B4F RID: 6991 RVA: 0x0017BC18 File Offset: 0x00179E18
		[EventFunction(154)]
		private static int GetOtherSmallSettlement(EventScriptRuntime runtime, short areaTemplateId, Settlement settlement)
		{
			short areaId = DomainManager.Map.GetAreaIdByAreaTemplateId(areaTemplateId);
			List<short> settlementIds = new List<short>();
			DomainManager.Map.GetAreaSettlementIds(areaId, settlementIds, false, false);
			foreach (short item in settlementIds)
			{
				bool flag = item != settlement.GetId();
				if (flag)
				{
					return (int)item;
				}
			}
			throw new InvalidOperationException("no available settlement in current area.");
		}

		// Token: 0x06001B50 RID: 6992 RVA: 0x0017BCA8 File Offset: 0x00179EA8
		[EventFunction(149)]
		private static void AddBuilding(EventScriptRuntime runtime, short blockTemplateId, Settlement settlement, bool forcePlace, bool closeToCenter)
		{
			Location location = settlement.GetLocation();
			DomainManager.Building.PlaceBuildingAtBlock(runtime.Context, location.AreaId, location.BlockId, blockTemplateId, forcePlace, !closeToCenter);
		}

		// Token: 0x06001B51 RID: 6993 RVA: 0x0017BCE1 File Offset: 0x00179EE1
		[EventFunction(151)]
		private static void SectStoryZhujianAddAreaMerchantType(EventScriptRuntime runtime, short areaTemplateId, sbyte merchantType)
		{
			DomainManager.Extra.SetSectZhujianAreaMerchantType(runtime.Context, areaTemplateId, merchantType);
			DomainManager.World.UpdateAreaMerchantType(runtime.Context);
		}

		// Token: 0x06001B52 RID: 6994 RVA: 0x0017BD08 File Offset: 0x00179F08
		[EventFunction(152)]
		private static void SectStoryZhujianRemoveAreaMerchantType(EventScriptRuntime runtime, short areaTemplateId)
		{
			DomainManager.Extra.RemoveSectZhujianAreaMerchantType(runtime.Context, areaTemplateId);
		}
	}
}
