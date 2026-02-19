using System.Collections.Generic;
using Config;
using GameData.Domains.Adventure;
using GameData.Domains.Character;
using GameData.Domains.Item;
using GameData.Domains.Map;
using GameData.Domains.Organization;
using GameData.Domains.Taiwu.Profession;
using GameData.Domains.Taiwu.Profession.SkillsData;
using GameData.Domains.TaiwuEvent;
using GameData.Domains.TaiwuEvent.MonthlyEventActions;
using GameData.Utilities;

namespace GameData.Domains.World;

public static class WorldStateDataHelper
{
	public static void DetectWarehouseOverload(this ref WorldStateData data)
	{
		int warehouseCurrLoad = DomainManager.Taiwu.GetWarehouseCurrLoad();
		int warehouseMaxLoad = DomainManager.Taiwu.GetWarehouseMaxLoad();
		if (warehouseCurrLoad > warehouseMaxLoad)
		{
			data.SetWorldState(9);
			data.AddOverloadStorageType(WorldStateData.EStorageType.Warehouse);
		}
		int troughCurrLoad = DomainManager.Taiwu.GetTroughCurrLoad();
		int troughMaxLoad = DomainManager.Taiwu.GetTroughMaxLoad();
		if (troughCurrLoad > troughMaxLoad)
		{
			data.SetWorldState(9);
			data.AddOverloadStorageType(WorldStateData.EStorageType.Trough);
		}
	}

	public unsafe static void DetectResourceOverload(this ref WorldStateData data)
	{
		int materialResourceMaxCount = DomainManager.Taiwu.GetMaterialResourceMaxCount();
		ResourceInts totalResources = DomainManager.Taiwu.GetTotalResources();
		for (sbyte b = 0; b < 6; b++)
		{
			if (totalResources.Items[b] > materialResourceMaxCount)
			{
				data.SetWorldState(10);
				data.AddOverloadingResourceType(b);
			}
		}
	}

	public static void DetectInventoryOverload(this ref WorldStateData data)
	{
		List<IntPair> overweightSanctionPercent = DomainManager.Taiwu.GetOverweightSanctionPercent();
		if (overweightSanctionPercent.Count > 0)
		{
			data.SetWorldState(8);
		}
	}

	public static void DetectInjuries(this ref WorldStateData data)
	{
		GameData.Domains.Character.Character taiwu = DomainManager.Taiwu.GetTaiwu();
		Injuries injuries = taiwu.GetInjuries();
		for (sbyte b = 0; b < 7; b++)
		{
			var (b2, b3) = injuries.Get(b);
			if (b2 >= 2)
			{
				data.AddOuterInjuryBodyPart(b);
			}
			if (b3 >= 2)
			{
				data.AddInnerInjuryBodyPart(b);
			}
		}
		if (data.AnyOuterInjury())
		{
			data.SetWorldState(11);
		}
		if (data.AnyInnerInjury())
		{
			data.SetWorldState(12);
		}
	}

	public unsafe static void DetectPoisons(this ref WorldStateData data)
	{
		GameData.Domains.Character.Character taiwu = DomainManager.Taiwu.GetTaiwu();
		PoisonInts poisoned = taiwu.GetPoisoned();
		for (sbyte b = 0; b < 6; b++)
		{
			sbyte b2 = PoisonsAndLevels.CalcPoisonedLevel(poisoned.Items[b]);
			if (b2 > 0)
			{
				data.SetWorldState(14);
				data.AddPoisonType(b);
			}
		}
	}

	public static void DetectDisorderOfQi(this ref WorldStateData data)
	{
		GameData.Domains.Character.Character taiwu = DomainManager.Taiwu.GetTaiwu();
		short disorderOfQi = taiwu.GetDisorderOfQi();
		if (DisorderLevelOfQi.GetDisorderLevelOfQi(disorderOfQi) != 0)
		{
			data.SetWorldState(13);
		}
	}

	public static void DetectTeammateInjuries(this ref WorldStateData data)
	{
		int taiwuCharId = DomainManager.Taiwu.GetTaiwuCharId();
		HashSet<int> collection = DomainManager.Taiwu.GetGroupCharIds().GetCollection();
		foreach (int item in collection)
		{
			if (item == taiwuCharId)
			{
				continue;
			}
			Injuries injuries = DomainManager.Character.GetElement_Objects(item).GetInjuries();
			for (sbyte b = 0; b < 7; b++)
			{
				var (b2, b3) = injuries.Get(b);
				if (b2 >= 2 || b3 >= 2)
				{
					data.SetWorldState(37);
					return;
				}
			}
		}
	}

	public static void DetectMainStory(this ref WorldStateData data)
	{
		if (DomainManager.Extra.GetExorcismEnabled())
		{
			data.SetWorldState(50);
		}
	}

	public static void DetectXiangshuAvatars(this ref WorldStateData data)
	{
		Location taiwuVillageLocation = DomainManager.Taiwu.GetTaiwuVillageLocation();
		sbyte xiangshuLevel = DomainManager.World.GetXiangshuLevel();
		AreaAdventureData adventuresInArea = DomainManager.Adventure.GetAdventuresInArea(taiwuVillageLocation.AreaId);
		foreach (var (num2, adventureSiteData2) in adventuresInArea.AdventureSites)
		{
			if (adventureSiteData2.SiteState != 1)
			{
				continue;
			}
			sbyte xiangshuAvatarIdBySwordTomb = XiangshuAvatarIds.GetXiangshuAvatarIdBySwordTomb(adventureSiteData2.TemplateId);
			if (xiangshuAvatarIdBySwordTomb < 0)
			{
				continue;
			}
			if (adventureSiteData2.RemainingMonths > 0)
			{
				data.SetWorldState(17);
				data.AddAwakeningXiangshuAvatar(xiangshuAvatarIdBySwordTomb);
				continue;
			}
			short currentLevelXiangshuTemplateId = XiangshuAvatarIds.GetCurrentLevelXiangshuTemplateId(xiangshuAvatarIdBySwordTomb, xiangshuLevel, isWeakened: true);
			if (DomainManager.Character.TryGetFixedCharacterByTemplateId(currentLevelXiangshuTemplateId, out var character) && character.GetLocation().IsValid())
			{
				data.SetWorldState(18);
				data.AddAttackingXiangshuAvatar(xiangshuAvatarIdBySwordTomb);
			}
		}
	}

	public static void DetectXiangshuInvasionProgress(this ref WorldStateData data)
	{
		if (!DomainManager.TutorialChapter.GetInGuiding() && DomainManager.World.GetMainStoryLineProgress() > 2)
		{
			data.SetWorldState(SharedMethods.GetInvasionWorldStateTemplateId());
		}
	}

	public static void DetectXiangshuInfection(this ref WorldStateData data)
	{
		GameData.Domains.Character.Character taiwu = DomainManager.Taiwu.GetTaiwu();
		byte xiangshuInfection = taiwu.GetXiangshuInfection();
		if (xiangshuInfection >= 200)
		{
			data.SetWorldState(16);
		}
		else if (xiangshuInfection >= 100)
		{
			data.SetWorldState(15);
		}
	}

	public static void DetectMartialArtTournament(this ref WorldStateData data)
	{
		MonthlyActionKey key = new MonthlyActionKey(2, 0);
		MonthlyActionBase monthlyAction = DomainManager.TaiwuEvent.GetMonthlyAction(key);
		if (monthlyAction != null)
		{
			switch (monthlyAction.State)
			{
			case 1:
				data.SetWorldState(19);
				break;
			case 2:
			case 3:
				data.SetWorldState(20);
				break;
			case 5:
				data.SetWorldState(21);
				break;
			case 4:
				break;
			}
		}
	}

	public static void DetectChangeWorldCreation(this ref WorldStateData data)
	{
		if (DomainManager.Extra.GetCanResetWorldSettings())
		{
			data.SetWorldState(38);
		}
	}

	public static void DetectLoongDebuff(this ref WorldStateData data)
	{
		int taiwuCharId = DomainManager.Taiwu.GetTaiwuCharId();
		foreach (var (_, loongInfo2) in DomainManager.Extra.FiveLoongDict)
		{
			if (loongInfo2.CharacterDebuffCounts != null && loongInfo2.CharacterDebuffCounts.TryGetValue(taiwuCharId, out var value) && value > 0)
			{
				data.SetWorldState(loongInfo2.ConfigData.WorldState);
			}
		}
	}

	public static void DetectInFulongFlameArea(this ref WorldStateData data)
	{
		if (DomainManager.Map.GetIsTaiwuInFulongFlameArea())
		{
			data.SetWorldState(44);
		}
	}

	public static void DetectTribulation(this ref WorldStateData data)
	{
		ProfessionData professionData = DomainManager.Extra.GetProfessionData(5);
		TaoistMonkSkillsData skillsData = professionData.GetSkillsData<TaoistMonkSkillsData>();
		if (skillsData.IsTriggeringTribulation)
		{
			data.SetWorldState(45);
		}
	}

	public static void DetectSectMainStory(this ref WorldStateData data)
	{
		if (!DomainManager.World.CheckCurrMainStoryLineProgressInRange(16, 28))
		{
			return;
		}
		GameData.Domains.Character.Character taiwu = DomainManager.Taiwu.GetTaiwu();
		Location location = taiwu.GetLocation();
		if (!location.IsValid())
		{
			location = taiwu.GetValidLocation();
		}
		if (MapAreaData.IsBrokenArea(location.AreaId))
		{
			return;
		}
		MapAreaData element_Areas = DomainManager.Map.GetElement_Areas(location.AreaId);
		MapAreaItem config = element_Areas.GetConfig();
		MapStateItem mapStateItem = MapState.Instance[config.StateID];
		if (mapStateItem.SectID < 0 || DomainManager.World.SectMainStoryTriggeredThisMonth(mapStateItem.SectID) || !DomainManager.World.CheckSectMainStoryAvailable(mapStateItem.SectID))
		{
			return;
		}
		sbyte taskReadyWorldState = Config.Organization.Instance[mapStateItem.SectID].TaskReadyWorldState;
		if (taskReadyWorldState < 0)
		{
			return;
		}
		WorldStateItem worldStateItem = WorldState.Instance[taskReadyWorldState];
		if (worldStateItem.TriggerArea < 0 || element_Areas.GetTemplateId() == worldStateItem.TriggerArea)
		{
			EventArgBox sectMainStoryEventArgBox = DomainManager.Extra.GetSectMainStoryEventArgBox(mapStateItem.SectID);
			sbyte sectID = mapStateItem.SectID;
			if (1 == 0)
			{
			}
			bool flag = sectID switch
			{
				1 => !sectMainStoryEventArgBox.GetBool("ConchShip_PresetKey_ShaolinLearnedAny"), 
				2 => !sectMainStoryEventArgBox.Contains<int>("ConchShip_PresetKey_EmeiAdventureTwoAppearDate"), 
				_ => true, 
			};
			if (1 == 0)
			{
			}
			if (flag)
			{
				data.SetWorldState(taskReadyWorldState);
			}
		}
	}

	public static void DetectTaiwuWanted(this ref WorldStateData data)
	{
		if (DomainManager.World.GetMainStoryLineProgress() < 16)
		{
			return;
		}
		foreach (int item in DomainManager.Taiwu.GetGroupCharIds().GetCollection())
		{
			sbyte sectOrgTemplateId;
			SettlementBounty bounty = DomainManager.Organization.GetBounty(item, out sectOrgTemplateId);
			if (bounty == null || sectOrgTemplateId < 0)
			{
				continue;
			}
			data.SetWorldState(49);
			break;
		}
	}

	public static void DetectTeammateDying(this ref WorldStateData data)
	{
		HashSet<int> collection = DomainManager.Taiwu.GetGroupCharIds().GetCollection();
		foreach (int item in collection)
		{
			GameData.Domains.Character.Character element_Objects = DomainManager.Character.GetElement_Objects(item);
			EHealthType healthType = element_Objects.GetHealthType();
			if ((uint)healthType <= 1u)
			{
				data.SetWorldState(51);
				break;
			}
		}
	}

	public static void DetectHomelessVillager(this ref WorldStateData data)
	{
		if (DomainManager.Building.GetHomeless().GetCount() > 0 && !DomainManager.TutorialChapter.GetInGuiding() && DomainManager.World.GetMainStoryLineProgress() >= 9)
		{
			data.SetWorldState(52);
		}
	}

	public static void DetectNeiliConflicting(this ref WorldStateData data)
	{
		HashSet<int> collection = DomainManager.Taiwu.GetGroupCharIds().GetCollection();
		foreach (int item in collection)
		{
			GameData.Domains.Character.Character element_Objects = DomainManager.Character.GetElement_Objects(item);
			NeiliTypeItem neiliTypeItem = NeiliType.Instance[element_Objects.GetNeiliType()];
			if (neiliTypeItem.ShowConflictingWorldState)
			{
				data.SetWorldState(53);
				break;
			}
		}
	}
}
