using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
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

namespace GameData.Domains.World.Task
{
	// Token: 0x02000031 RID: 49
	public static class TaskConditionChecker
	{
		// Token: 0x06000EE2 RID: 3810 RVA: 0x000F9038 File Offset: 0x000F7238
		public static bool CheckCondition(int conditionId)
		{
			return TaskConditionChecker.CheckCondition(TaskCondition.Instance[conditionId]);
		}

		// Token: 0x06000EE3 RID: 3811 RVA: 0x000F905C File Offset: 0x000F725C
		public static bool CheckCondition(TaskConditionItem condition)
		{
			ETaskConditionType type = condition.Type;
			if (!true)
			{
			}
			bool flag;
			switch (type)
			{
			case ETaskConditionType.AdventureVisible:
				flag = TaskConditionChecker.CheckAdventureVisible(condition);
				break;
			case ETaskConditionType.CharacterExists:
				flag = TaskConditionChecker.CheckCharacterExists(condition);
				break;
			case ETaskConditionType.CharacterAtMapBlock:
				flag = TaskConditionChecker.CheckCharacterAtMapBlock(condition);
				break;
			case ETaskConditionType.CharacterAtMapArea:
				flag = TaskConditionChecker.CheckCharacterAtMapArea(condition);
				break;
			case ETaskConditionType.CharacterAtAdventure:
				flag = TaskConditionChecker.CheckCharacterAtAdventureSite(condition);
				break;
			case ETaskConditionType.CharacterHasItems:
				flag = TaskConditionChecker.CheckCharacterHasItems(condition);
				break;
			case ETaskConditionType.CharacterHasItemSubType:
				flag = TaskConditionChecker.CheckCharacterHasItemSubType(condition);
				break;
			case ETaskConditionType.FavorabilityToTaiwu:
				flag = TaskConditionChecker.CheckFavorabilityToTaiwu(condition);
				break;
			case ETaskConditionType.SettlementHasBuilding:
				flag = TaskConditionChecker.CheckSettlementHasBuilding(condition);
				break;
			case ETaskConditionType.FunctionUnlocked:
				flag = DomainManager.World.GetWorldFunctionsStatus((byte)condition.IntParam);
				break;
			case ETaskConditionType.JuniorXiangshuTaskStatus:
				flag = TaskConditionChecker.CheckJuniorXiangshuTaskStatus(condition);
				break;
			case ETaskConditionType.JuniorXiangshuTaskCompleteAmount:
				flag = TaskConditionChecker.CheckJuniorXiangshuTaskCompleteAmount(condition);
				break;
			case ETaskConditionType.SwordTombStatus:
				flag = TaskConditionChecker.CheckSwordTombStatus(condition);
				break;
			case ETaskConditionType.MartialArtTournamentPreparing:
				flag = TaskConditionChecker.CheckMartialArtTournamentPreparing(condition);
				break;
			case ETaskConditionType.GlobalArgBoxValueRange:
				flag = TaskConditionChecker.CheckGlobalArgBoxValueRange(condition);
				break;
			case ETaskConditionType.GlobalArgBoxKeyExists:
				flag = TaskConditionChecker.CheckGlobalArgBoxKeyExists(condition);
				break;
			case ETaskConditionType.ConditionAnd:
				flag = condition.AndTaskCondition.TrueForAll(new Predicate<int>(TaskConditionChecker.CheckCondition));
				break;
			case ETaskConditionType.ConditionOr:
				flag = condition.OrTaskCondition.Exists(new Predicate<int>(TaskConditionChecker.CheckCondition));
				break;
			case ETaskConditionType.IsInAdventure:
				flag = TaskConditionChecker.CheckIsInAdventure(condition);
				break;
			case ETaskConditionType.ProfessionSkillValid:
				flag = TaskConditionChecker.CheckProfessionSkillValid(condition);
				break;
			case ETaskConditionType.StateTemplateVisited:
				flag = TaskConditionChecker.CheckStateTempleVisited(condition);
				break;
			default:
				flag = false;
				break;
			}
			if (!true)
			{
			}
			bool result = flag;
			return condition.IsReverseCondition != result;
		}

		// Token: 0x06000EE4 RID: 3812 RVA: 0x000F91FC File Offset: 0x000F73FC
		private static GameData.Domains.Character.Character GetCharacterArgument(TaskConditionItem condition)
		{
			GameData.Domains.Character.Character result;
			switch (condition.CharacterType)
			{
			case ETaskConditionCharacterType.Taiwu:
				result = DomainManager.Taiwu.GetTaiwu();
				break;
			case ETaskConditionCharacterType.FixedCharacter:
			{
				GameData.Domains.Character.Character character;
				DomainManager.Character.TryGetFixedCharacterByTemplateId(condition.CharacterTemplateId, out character);
				result = character;
				break;
			}
			case ETaskConditionCharacterType.ConvertedFixedCharacter:
			{
				GameData.Domains.Character.Character character2;
				DomainManager.Character.TryGetConvertedFixedCharacterByTemplateId(condition.CharacterTemplateId, out character2);
				result = character2;
				break;
			}
			case ETaskConditionCharacterType.XiangshuAvatar:
			{
				sbyte level = Math.Min(DomainManager.World.GetXiangshuLevel(), 8);
				short charTemplateId = condition.CharacterTemplateId + (short)level;
				GameData.Domains.Character.Character character3;
				DomainManager.Character.TryGetFixedCharacterByTemplateId(charTemplateId, out character3);
				result = character3;
				break;
			}
			default:
				result = null;
				break;
			}
			return result;
		}

		// Token: 0x06000EE5 RID: 3813 RVA: 0x000F92A4 File Offset: 0x000F74A4
		public static bool CheckAdventureVisible(TaskConditionItem condition)
		{
			ETaskConditionAreaType areaType = condition.AreaType;
			ETaskConditionAreaType etaskConditionAreaType = areaType;
			bool result;
			if (etaskConditionAreaType != ETaskConditionAreaType.CurrentArea)
			{
				if (etaskConditionAreaType != ETaskConditionAreaType.TaiwuVillageArea)
				{
					for (short areaId = 0; areaId < 139; areaId += 1)
					{
						bool flag = TaskConditionChecker.CheckAdventureVisibleInArea(areaId, condition.Adventure);
						if (flag)
						{
							return true;
						}
					}
					result = false;
				}
				else
				{
					short areaId2 = DomainManager.Taiwu.GetTaiwuVillageLocation().AreaId;
					result = TaskConditionChecker.CheckAdventureVisibleInArea(areaId2, condition.Adventure);
				}
			}
			else
			{
				short areaId3 = DomainManager.Taiwu.GetTaiwu().GetLocation().AreaId;
				result = TaskConditionChecker.CheckAdventureVisibleInArea(areaId3, condition.Adventure);
			}
			return result;
		}

		// Token: 0x06000EE6 RID: 3814 RVA: 0x000F9348 File Offset: 0x000F7548
		private static bool CheckAdventureVisibleInArea(short areaId, short adventureTemplateId)
		{
			bool flag = areaId < 0;
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				AreaAdventureData adventuresInArea = DomainManager.Adventure.GetAdventuresInArea(areaId);
				foreach (AdventureSiteData site in adventuresInArea.AdventureSites.Values)
				{
					bool flag2 = site.TemplateId == adventureTemplateId && site.SiteState == 1;
					if (flag2)
					{
						return true;
					}
				}
				result = false;
			}
			return result;
		}

		// Token: 0x06000EE7 RID: 3815 RVA: 0x000F93DC File Offset: 0x000F75DC
		public static bool CheckCharacterAtAdventureSite(TaskConditionItem condition)
		{
			GameData.Domains.Character.Character character = TaskConditionChecker.GetCharacterArgument(condition);
			bool flag = character == null;
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				bool flag2 = !character.GetLocation().IsValid();
				result = (!flag2 && CharacterMatchers.MatchAtVisibleAdventureSite(character, condition.Adventure));
			}
			return result;
		}

		// Token: 0x06000EE8 RID: 3816 RVA: 0x000F9428 File Offset: 0x000F7628
		public static bool CheckCharacterAtMapBlock(TaskConditionItem condition)
		{
			GameData.Domains.Character.Character character = TaskConditionChecker.GetCharacterArgument(condition);
			bool flag = character == null;
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				Location location = character.GetLocation();
				bool flag2 = !location.IsValid();
				if (flag2)
				{
					result = false;
				}
				else
				{
					MapBlockData blockData = DomainManager.Map.GetBlock(location);
					result = condition.MapBlockList.Contains(blockData.TemplateId);
				}
			}
			return result;
		}

		// Token: 0x06000EE9 RID: 3817 RVA: 0x000F948C File Offset: 0x000F768C
		public static bool CheckCharacterAtMapArea(TaskConditionItem condition)
		{
			GameData.Domains.Character.Character character = TaskConditionChecker.GetCharacterArgument(condition);
			bool flag = character == null;
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				short areaId = character.GetLocation().AreaId;
				bool flag2 = areaId < 0;
				if (flag2)
				{
					result = false;
				}
				else
				{
					ETaskConditionAreaType areaType = condition.AreaType;
					ETaskConditionAreaType etaskConditionAreaType = areaType;
					if (etaskConditionAreaType != ETaskConditionAreaType.CurrentArea)
					{
						if (etaskConditionAreaType != ETaskConditionAreaType.TaiwuVillageArea)
						{
							result = true;
						}
						else
						{
							Location location = DomainManager.Taiwu.GetTaiwuVillageLocation();
							result = (location.AreaId == areaId);
						}
					}
					else
					{
						Location location2 = DomainManager.Taiwu.GetTaiwu().GetLocation();
						result = (areaId == location2.AreaId);
					}
				}
			}
			return result;
		}

		// Token: 0x06000EEA RID: 3818 RVA: 0x000F9520 File Offset: 0x000F7720
		public static bool CheckCharacterExists(TaskConditionItem condition)
		{
			GameData.Domains.Character.Character character = TaskConditionChecker.GetCharacterArgument(condition);
			return character != null;
		}

		// Token: 0x06000EEB RID: 3819 RVA: 0x000F9540 File Offset: 0x000F7740
		public static bool CheckCharacterHasItems(TaskConditionItem condition)
		{
			GameData.Domains.Character.Character character = TaskConditionChecker.GetCharacterArgument(condition);
			bool flag = character == null;
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				foreach (PresetItemWithCount itemToCheck in condition.Items)
				{
					bool flag2 = !CharacterMatchers.MatchHasInventoryItem(character, itemToCheck.ItemType, itemToCheck.TemplateId, itemToCheck.Count);
					if (flag2)
					{
						return false;
					}
				}
				result = true;
			}
			return result;
		}

		// Token: 0x06000EEC RID: 3820 RVA: 0x000F95D4 File Offset: 0x000F77D4
		public static bool CheckCharacterHasItemSubType(TaskConditionItem condition)
		{
			GameData.Domains.Character.Character character = TaskConditionChecker.GetCharacterArgument(condition);
			bool flag = character == null;
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				Dictionary<ItemKey, int> inventoryItem = character.GetInventory().Items;
				foreach (ItemKey itemKey in inventoryItem.Keys)
				{
					bool flag2 = (int)ItemTemplateHelper.GetItemSubType(itemKey.ItemType, itemKey.TemplateId) == condition.IntParam;
					if (flag2)
					{
						return true;
					}
				}
				result = false;
			}
			return result;
		}

		// Token: 0x06000EED RID: 3821 RVA: 0x000F9670 File Offset: 0x000F7870
		public static bool CheckFavorabilityToTaiwu(TaskConditionItem condition)
		{
			GameData.Domains.Character.Character character = TaskConditionChecker.GetCharacterArgument(condition);
			bool flag = character == null;
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				int taiwuCharId = DomainManager.Taiwu.GetTaiwuCharId();
				short favorability = DomainManager.Character.GetFavorability(character.GetId(), taiwuCharId);
				result = ((int)favorability >= condition.ValueRange.First && (int)favorability < condition.ValueRange.Second);
			}
			return result;
		}

		// Token: 0x06000EEE RID: 3822 RVA: 0x000F96D4 File Offset: 0x000F78D4
		public static bool CheckSettlementHasBuilding(TaskConditionItem condition)
		{
			foreach (short mapBlockTemplateId in condition.MapBlockList)
			{
				short num = mapBlockTemplateId;
				short num2 = num;
				if (num2 != 0)
				{
					switch (num2)
					{
					case 16:
					{
						bool flag = TaskConditionChecker.<CheckSettlementHasBuilding>g__FindInArea|12_0(137, mapBlockTemplateId, condition.Building);
						if (flag)
						{
							return true;
						}
						break;
					}
					case 17:
					{
						bool flag2 = TaskConditionChecker.<CheckSettlementHasBuilding>g__FindInArea|12_0(135, mapBlockTemplateId, condition.Building);
						if (flag2)
						{
							return true;
						}
						break;
					}
					case 18:
					{
						bool flag3 = TaskConditionChecker.<CheckSettlementHasBuilding>g__FindInArea|12_0(136, mapBlockTemplateId, condition.Building);
						if (flag3)
						{
							return true;
						}
						break;
					}
					default:
						for (short areaId = 0; areaId < 45; areaId += 1)
						{
							bool flag4 = TaskConditionChecker.<CheckSettlementHasBuilding>g__FindInArea|12_0(areaId, mapBlockTemplateId, condition.Building);
							if (flag4)
							{
								return true;
							}
						}
						break;
					}
				}
				else
				{
					Location location = DomainManager.Taiwu.GetTaiwuVillageLocation();
					BuildingAreaData buildingArea = DomainManager.Building.GetBuildingAreaData(location);
					BuildingBlockKey buildingKey = BuildingDomain.FindBuildingKey(location, buildingArea, condition.Building, true);
					bool flag5 = DomainManager.Building.BuildingBlockLevel(buildingKey) > 0;
					if (flag5)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06000EEF RID: 3823 RVA: 0x000F9840 File Offset: 0x000F7A40
		public static bool CheckJuniorXiangshuTaskStatus(TaskConditionItem condition)
		{
			sbyte avatarId = XiangshuAvatarIds.GetXiangshuAvatarIdByCharacterTemplateId(condition.CharacterTemplateId);
			XiangshuAvatarTaskStatus taskStatus = DomainManager.World.GetElement_XiangshuAvatarTaskStatuses((int)avatarId);
			return (int)taskStatus.JuniorXiangshuTaskStatus >= condition.ValueRange.First && (int)taskStatus.JuniorXiangshuTaskStatus < condition.ValueRange.Second;
		}

		// Token: 0x06000EF0 RID: 3824 RVA: 0x000F9894 File Offset: 0x000F7A94
		public static bool CheckSwordTombStatus(TaskConditionItem condition)
		{
			sbyte avatarId = XiangshuAvatarIds.GetXiangshuAvatarIdByCharacterTemplateId(condition.CharacterTemplateId);
			XiangshuAvatarTaskStatus taskStatus = DomainManager.World.GetElement_XiangshuAvatarTaskStatuses((int)avatarId);
			return (int)taskStatus.SwordTombStatus == condition.IntParam;
		}

		// Token: 0x06000EF1 RID: 3825 RVA: 0x000F98CC File Offset: 0x000F7ACC
		public static bool CheckGlobalArgBoxValueRange(TaskConditionItem condition)
		{
			EventArgBox argBox = DomainManager.TaiwuEvent.GetGlobalEventArgumentBox();
			int val = 0;
			bool flag = !argBox.Get(condition.GlobalArgBoxKey, ref val);
			return !flag && val >= condition.ValueRange.First && val < condition.ValueRange.Second;
		}

		// Token: 0x06000EF2 RID: 3826 RVA: 0x000F9924 File Offset: 0x000F7B24
		public static bool CheckGlobalArgBoxKeyExists(TaskConditionItem condition)
		{
			EventArgBox argBox = DomainManager.TaiwuEvent.GetGlobalEventArgumentBox();
			return argBox.Contains<bool>(condition.GlobalArgBoxKey) || argBox.Contains<int>(condition.GlobalArgBoxKey) || argBox.Contains<string>(condition.GlobalArgBoxKey);
		}

		// Token: 0x06000EF3 RID: 3827 RVA: 0x000F996C File Offset: 0x000F7B6C
		public static bool CheckJuniorXiangshuTaskCompleteAmount(TaskConditionItem condition)
		{
			int amount = 0;
			for (sbyte avatarId = 0; avatarId < 9; avatarId += 1)
			{
				XiangshuAvatarTaskStatus taskStatus = DomainManager.World.GetElement_XiangshuAvatarTaskStatuses((int)avatarId);
				bool flag = taskStatus.JuniorXiangshuTaskStatus > 4;
				if (flag)
				{
					amount++;
				}
			}
			return amount >= condition.ValueRange.First && amount < condition.ValueRange.Second;
		}

		// Token: 0x06000EF4 RID: 3828 RVA: 0x000F99D4 File Offset: 0x000F7BD4
		public static bool CheckMartialArtTournamentPreparing(TaskConditionItem condition)
		{
			MonthlyActionKey actionKey = new MonthlyActionKey(2, 0);
			MartialArtTournamentMonthlyAction monthlyAction = (MartialArtTournamentMonthlyAction)DomainManager.TaiwuEvent.GetMonthlyAction(actionKey);
			return monthlyAction.State == 1;
		}

		// Token: 0x06000EF5 RID: 3829 RVA: 0x000F9A0C File Offset: 0x000F7C0C
		public static bool CheckIsInAdventure(TaskConditionItem condition)
		{
			short adventureId = DomainManager.Adventure.GetCurAdventureId();
			return adventureId == condition.Adventure;
		}

		// Token: 0x06000EF6 RID: 3830 RVA: 0x000F9A34 File Offset: 0x000F7C34
		public static bool CheckProfessionSkillValid(TaskConditionItem condition)
		{
			int skillId = condition.ProfessionSkill;
			bool flag = !DomainManager.Extra.IsProfessionalSkillUnlockedAndEquipped(skillId);
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				ProfessionSkillItem skillCfg = ProfessionSkill.Instance[skillId];
				ProfessionData professionData = DomainManager.Extra.GetProfessionData(skillCfg.Profession);
				bool flag2 = !professionData.HadBeenUnlocked[(int)(skillCfg.Level - 1)];
				if (flag2)
				{
					result = false;
				}
				else
				{
					bool flag3 = !ProfessionSkillHandle.CheckSpecialCondition(professionData, (int)(skillCfg.Level - 1));
					result = !flag3;
				}
			}
			return result;
		}

		// Token: 0x06000EF7 RID: 3831 RVA: 0x000F9AC0 File Offset: 0x000F7CC0
		public static bool CheckStateTempleVisited(TaskConditionItem condition)
		{
			ProfessionData professionData = DomainManager.Extra.GetProfessionData(12);
			bool flag = professionData == null;
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				TravelingBuddhistMonkSkillsData skillsData = professionData.GetSkillsData<TravelingBuddhistMonkSkillsData>();
				sbyte stateId = DomainManager.Map.GetStateIdByStateTemplateId((short)condition.StateTemplateId);
				result = (skillsData.StateHasTemple(stateId) && skillsData.IsStateTempleVisited(stateId));
			}
			return result;
		}

		// Token: 0x06000EF8 RID: 3832 RVA: 0x000F9B1C File Offset: 0x000F7D1C
		[CompilerGenerated]
		internal static bool <CheckSettlementHasBuilding>g__FindInArea|12_0(short areaId, short blockTemplateId, short buildingTemplateId)
		{
			MapAreaData areaData = DomainManager.Map.GetElement_Areas((int)areaId);
			foreach (SettlementInfo settlementInfo in areaData.SettlementInfos)
			{
				bool flag = settlementInfo.SettlementId < 0;
				if (!flag)
				{
					MapBlockData block = DomainManager.Map.GetBlock(areaId, settlementInfo.BlockId);
					bool flag2 = block.TemplateId != blockTemplateId;
					if (!flag2)
					{
						Location location = new Location(areaId, settlementInfo.BlockId);
						BuildingAreaData buildingArea = DomainManager.Building.GetBuildingAreaData(location);
						BuildingBlockKey buildingKey = BuildingDomain.FindBuildingKey(location, buildingArea, buildingTemplateId, true);
						bool flag3 = DomainManager.Building.BuildingBlockLevel(buildingKey) > 0;
						if (flag3)
						{
							return true;
						}
					}
				}
			}
			return false;
		}
	}
}
