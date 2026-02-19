using System;
using System.Collections.Generic;
using Config;
using Config.ConfigCells;
using GameData.Domains.Adventure;
using GameData.Domains.Building;
using GameData.Domains.Character;
using GameData.Domains.Character.Filters;
using GameData.Domains.Item;
using GameData.Domains.Map;
using GameData.Domains.Taiwu.Profession;
using GameData.Domains.Taiwu.Profession.SkillsData;
using GameData.Domains.TaiwuEvent;
using GameData.Domains.TaiwuEvent.MonthlyEventActions;

namespace GameData.Domains.World.Task;

public static class TaskConditionChecker
{
	public static bool CheckCondition(int conditionId)
	{
		return CheckCondition(TaskCondition.Instance[conditionId]);
	}

	public static bool CheckCondition(TaskConditionItem condition)
	{
		ETaskConditionType type = condition.Type;
		if (1 == 0)
		{
		}
		bool flag = type switch
		{
			ETaskConditionType.AdventureVisible => CheckAdventureVisible(condition), 
			ETaskConditionType.CharacterExists => CheckCharacterExists(condition), 
			ETaskConditionType.CharacterAtMapBlock => CheckCharacterAtMapBlock(condition), 
			ETaskConditionType.CharacterAtMapArea => CheckCharacterAtMapArea(condition), 
			ETaskConditionType.CharacterAtAdventure => CheckCharacterAtAdventureSite(condition), 
			ETaskConditionType.CharacterHasItems => CheckCharacterHasItems(condition), 
			ETaskConditionType.CharacterHasItemSubType => CheckCharacterHasItemSubType(condition), 
			ETaskConditionType.FavorabilityToTaiwu => CheckFavorabilityToTaiwu(condition), 
			ETaskConditionType.SettlementHasBuilding => CheckSettlementHasBuilding(condition), 
			ETaskConditionType.FunctionUnlocked => DomainManager.World.GetWorldFunctionsStatus((byte)condition.IntParam), 
			ETaskConditionType.JuniorXiangshuTaskStatus => CheckJuniorXiangshuTaskStatus(condition), 
			ETaskConditionType.SwordTombStatus => CheckSwordTombStatus(condition), 
			ETaskConditionType.GlobalArgBoxValueRange => CheckGlobalArgBoxValueRange(condition), 
			ETaskConditionType.GlobalArgBoxKeyExists => CheckGlobalArgBoxKeyExists(condition), 
			ETaskConditionType.JuniorXiangshuTaskCompleteAmount => CheckJuniorXiangshuTaskCompleteAmount(condition), 
			ETaskConditionType.MartialArtTournamentPreparing => CheckMartialArtTournamentPreparing(condition), 
			ETaskConditionType.ConditionAnd => condition.AndTaskCondition.TrueForAll(CheckCondition), 
			ETaskConditionType.ConditionOr => condition.OrTaskCondition.Exists(CheckCondition), 
			ETaskConditionType.IsInAdventure => CheckIsInAdventure(condition), 
			ETaskConditionType.ProfessionSkillValid => CheckProfessionSkillValid(condition), 
			ETaskConditionType.StateTemplateVisited => CheckStateTempleVisited(condition), 
			_ => false, 
		};
		if (1 == 0)
		{
		}
		bool flag2 = flag;
		return condition.IsReverseCondition != flag2;
	}

	private static GameData.Domains.Character.Character GetCharacterArgument(TaskConditionItem condition)
	{
		switch (condition.CharacterType)
		{
		case ETaskConditionCharacterType.Taiwu:
			return DomainManager.Taiwu.GetTaiwu();
		case ETaskConditionCharacterType.FixedCharacter:
		{
			DomainManager.Character.TryGetFixedCharacterByTemplateId(condition.CharacterTemplateId, out var character3);
			return character3;
		}
		case ETaskConditionCharacterType.ConvertedFixedCharacter:
		{
			DomainManager.Character.TryGetConvertedFixedCharacterByTemplateId(condition.CharacterTemplateId, out var character2);
			return character2;
		}
		case ETaskConditionCharacterType.XiangshuAvatar:
		{
			sbyte b = Math.Min(DomainManager.World.GetXiangshuLevel(), 8);
			short templateId = (short)(condition.CharacterTemplateId + b);
			DomainManager.Character.TryGetFixedCharacterByTemplateId(templateId, out var character);
			return character;
		}
		default:
			return null;
		}
	}

	public static bool CheckAdventureVisible(TaskConditionItem condition)
	{
		switch (condition.AreaType)
		{
		case ETaskConditionAreaType.TaiwuVillageArea:
		{
			short areaId2 = DomainManager.Taiwu.GetTaiwuVillageLocation().AreaId;
			return CheckAdventureVisibleInArea(areaId2, condition.Adventure);
		}
		case ETaskConditionAreaType.CurrentArea:
		{
			short areaId = DomainManager.Taiwu.GetTaiwu().GetLocation().AreaId;
			return CheckAdventureVisibleInArea(areaId, condition.Adventure);
		}
		default:
		{
			for (short num = 0; num < 139; num++)
			{
				if (CheckAdventureVisibleInArea(num, condition.Adventure))
				{
					return true;
				}
			}
			return false;
		}
		}
	}

	private static bool CheckAdventureVisibleInArea(short areaId, short adventureTemplateId)
	{
		if (areaId < 0)
		{
			return false;
		}
		AreaAdventureData adventuresInArea = DomainManager.Adventure.GetAdventuresInArea(areaId);
		foreach (AdventureSiteData value in adventuresInArea.AdventureSites.Values)
		{
			if (value.TemplateId == adventureTemplateId && value.SiteState == 1)
			{
				return true;
			}
		}
		return false;
	}

	public static bool CheckCharacterAtAdventureSite(TaskConditionItem condition)
	{
		GameData.Domains.Character.Character characterArgument = GetCharacterArgument(condition);
		if (characterArgument == null)
		{
			return false;
		}
		if (!characterArgument.GetLocation().IsValid())
		{
			return false;
		}
		return CharacterMatchers.MatchAtVisibleAdventureSite(characterArgument, condition.Adventure);
	}

	public static bool CheckCharacterAtMapBlock(TaskConditionItem condition)
	{
		GameData.Domains.Character.Character characterArgument = GetCharacterArgument(condition);
		if (characterArgument == null)
		{
			return false;
		}
		Location location = characterArgument.GetLocation();
		if (!location.IsValid())
		{
			return false;
		}
		MapBlockData block = DomainManager.Map.GetBlock(location);
		return condition.MapBlockList.Contains(block.TemplateId);
	}

	public static bool CheckCharacterAtMapArea(TaskConditionItem condition)
	{
		GameData.Domains.Character.Character characterArgument = GetCharacterArgument(condition);
		if (characterArgument == null)
		{
			return false;
		}
		short areaId = characterArgument.GetLocation().AreaId;
		if (areaId < 0)
		{
			return false;
		}
		return condition.AreaType switch
		{
			ETaskConditionAreaType.TaiwuVillageArea => DomainManager.Taiwu.GetTaiwuVillageLocation().AreaId == areaId, 
			ETaskConditionAreaType.CurrentArea => areaId == DomainManager.Taiwu.GetTaiwu().GetLocation().AreaId, 
			_ => true, 
		};
	}

	public static bool CheckCharacterExists(TaskConditionItem condition)
	{
		GameData.Domains.Character.Character characterArgument = GetCharacterArgument(condition);
		return characterArgument != null;
	}

	public static bool CheckCharacterHasItems(TaskConditionItem condition)
	{
		GameData.Domains.Character.Character characterArgument = GetCharacterArgument(condition);
		if (characterArgument == null)
		{
			return false;
		}
		foreach (PresetItemWithCount item in condition.Items)
		{
			if (!CharacterMatchers.MatchHasInventoryItem(characterArgument, item.ItemType, item.TemplateId, item.Count))
			{
				return false;
			}
		}
		return true;
	}

	public static bool CheckCharacterHasItemSubType(TaskConditionItem condition)
	{
		GameData.Domains.Character.Character characterArgument = GetCharacterArgument(condition);
		if (characterArgument == null)
		{
			return false;
		}
		Dictionary<ItemKey, int> items = characterArgument.GetInventory().Items;
		foreach (ItemKey key in items.Keys)
		{
			if (ItemTemplateHelper.GetItemSubType(key.ItemType, key.TemplateId) == condition.IntParam)
			{
				return true;
			}
		}
		return false;
	}

	public static bool CheckFavorabilityToTaiwu(TaskConditionItem condition)
	{
		GameData.Domains.Character.Character characterArgument = GetCharacterArgument(condition);
		if (characterArgument == null)
		{
			return false;
		}
		int taiwuCharId = DomainManager.Taiwu.GetTaiwuCharId();
		short favorability = DomainManager.Character.GetFavorability(characterArgument.GetId(), taiwuCharId);
		return favorability >= condition.ValueRange.First && favorability < condition.ValueRange.Second;
	}

	public static bool CheckSettlementHasBuilding(TaskConditionItem condition)
	{
		foreach (short mapBlock in condition.MapBlockList)
		{
			switch (mapBlock)
			{
			case 0:
			{
				Location taiwuVillageLocation = DomainManager.Taiwu.GetTaiwuVillageLocation();
				BuildingAreaData buildingAreaData = DomainManager.Building.GetBuildingAreaData(taiwuVillageLocation);
				BuildingBlockKey blockKey = BuildingDomain.FindBuildingKey(taiwuVillageLocation, buildingAreaData, condition.Building);
				if (DomainManager.Building.BuildingBlockLevel(blockKey) > 0)
				{
					return true;
				}
				continue;
			}
			case 17:
				if (FindInArea(135, mapBlock, condition.Building))
				{
					return true;
				}
				continue;
			case 18:
				if (FindInArea(136, mapBlock, condition.Building))
				{
					return true;
				}
				continue;
			case 16:
				if (FindInArea(137, mapBlock, condition.Building))
				{
					return true;
				}
				continue;
			}
			for (short num = 0; num < 45; num++)
			{
				if (FindInArea(num, mapBlock, condition.Building))
				{
					return true;
				}
			}
		}
		return false;
		static bool FindInArea(short areaId, short blockTemplateId, short buildingTemplateId)
		{
			MapAreaData element_Areas = DomainManager.Map.GetElement_Areas(areaId);
			SettlementInfo[] settlementInfos = element_Areas.SettlementInfos;
			for (int i = 0; i < settlementInfos.Length; i++)
			{
				SettlementInfo settlementInfo = settlementInfos[i];
				if (settlementInfo.SettlementId >= 0)
				{
					MapBlockData block = DomainManager.Map.GetBlock(areaId, settlementInfo.BlockId);
					if (block.TemplateId == blockTemplateId)
					{
						Location location = new Location(areaId, settlementInfo.BlockId);
						BuildingAreaData buildingAreaData2 = DomainManager.Building.GetBuildingAreaData(location);
						BuildingBlockKey blockKey2 = BuildingDomain.FindBuildingKey(location, buildingAreaData2, buildingTemplateId);
						if (DomainManager.Building.BuildingBlockLevel(blockKey2) > 0)
						{
							return true;
						}
					}
				}
			}
			return false;
		}
	}

	public static bool CheckJuniorXiangshuTaskStatus(TaskConditionItem condition)
	{
		sbyte xiangshuAvatarIdByCharacterTemplateId = XiangshuAvatarIds.GetXiangshuAvatarIdByCharacterTemplateId(condition.CharacterTemplateId);
		XiangshuAvatarTaskStatus element_XiangshuAvatarTaskStatuses = DomainManager.World.GetElement_XiangshuAvatarTaskStatuses(xiangshuAvatarIdByCharacterTemplateId);
		return element_XiangshuAvatarTaskStatuses.JuniorXiangshuTaskStatus >= condition.ValueRange.First && element_XiangshuAvatarTaskStatuses.JuniorXiangshuTaskStatus < condition.ValueRange.Second;
	}

	public static bool CheckSwordTombStatus(TaskConditionItem condition)
	{
		sbyte xiangshuAvatarIdByCharacterTemplateId = XiangshuAvatarIds.GetXiangshuAvatarIdByCharacterTemplateId(condition.CharacterTemplateId);
		return DomainManager.World.GetElement_XiangshuAvatarTaskStatuses(xiangshuAvatarIdByCharacterTemplateId).SwordTombStatus == condition.IntParam;
	}

	public static bool CheckGlobalArgBoxValueRange(TaskConditionItem condition)
	{
		EventArgBox globalEventArgumentBox = DomainManager.TaiwuEvent.GetGlobalEventArgumentBox();
		int arg = 0;
		if (!globalEventArgumentBox.Get(condition.GlobalArgBoxKey, ref arg))
		{
			return false;
		}
		return arg >= condition.ValueRange.First && arg < condition.ValueRange.Second;
	}

	public static bool CheckGlobalArgBoxKeyExists(TaskConditionItem condition)
	{
		EventArgBox globalEventArgumentBox = DomainManager.TaiwuEvent.GetGlobalEventArgumentBox();
		return globalEventArgumentBox.Contains<bool>(condition.GlobalArgBoxKey) || globalEventArgumentBox.Contains<int>(condition.GlobalArgBoxKey) || globalEventArgumentBox.Contains<string>(condition.GlobalArgBoxKey);
	}

	public static bool CheckJuniorXiangshuTaskCompleteAmount(TaskConditionItem condition)
	{
		int num = 0;
		for (sbyte b = 0; b < 9; b++)
		{
			if (DomainManager.World.GetElement_XiangshuAvatarTaskStatuses(b).JuniorXiangshuTaskStatus > 4)
			{
				num++;
			}
		}
		return num >= condition.ValueRange.First && num < condition.ValueRange.Second;
	}

	public static bool CheckMartialArtTournamentPreparing(TaskConditionItem condition)
	{
		MonthlyActionKey key = new MonthlyActionKey(2, 0);
		MartialArtTournamentMonthlyAction martialArtTournamentMonthlyAction = (MartialArtTournamentMonthlyAction)DomainManager.TaiwuEvent.GetMonthlyAction(key);
		return martialArtTournamentMonthlyAction.State == 1;
	}

	public static bool CheckIsInAdventure(TaskConditionItem condition)
	{
		short curAdventureId = DomainManager.Adventure.GetCurAdventureId();
		return curAdventureId == condition.Adventure;
	}

	public static bool CheckProfessionSkillValid(TaskConditionItem condition)
	{
		int professionSkill = condition.ProfessionSkill;
		if (!DomainManager.Extra.IsProfessionalSkillUnlockedAndEquipped(professionSkill))
		{
			return false;
		}
		ProfessionSkillItem professionSkillItem = ProfessionSkill.Instance[professionSkill];
		ProfessionData professionData = DomainManager.Extra.GetProfessionData(professionSkillItem.Profession);
		if (!professionData.HadBeenUnlocked[professionSkillItem.Level - 1])
		{
			return false;
		}
		if (!ProfessionSkillHandle.CheckSpecialCondition(professionData, professionSkillItem.Level - 1))
		{
			return false;
		}
		return true;
	}

	public static bool CheckStateTempleVisited(TaskConditionItem condition)
	{
		ProfessionData professionData = DomainManager.Extra.GetProfessionData(12);
		if (professionData == null)
		{
			return false;
		}
		TravelingBuddhistMonkSkillsData skillsData = professionData.GetSkillsData<TravelingBuddhistMonkSkillsData>();
		sbyte stateIdByStateTemplateId = DomainManager.Map.GetStateIdByStateTemplateId(condition.StateTemplateId);
		return skillsData.StateHasTemple(stateIdByStateTemplateId) && skillsData.IsStateTempleVisited(stateIdByStateTemplateId);
	}
}
