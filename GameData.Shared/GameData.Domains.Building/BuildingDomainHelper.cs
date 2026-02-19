using System.Collections.Generic;

namespace GameData.Domains.Building;

public static class BuildingDomainHelper
{
	public static class DataIds
	{
		public const ushort BuildingAreas = 0;

		public const ushort BuildingBlocks = 1;

		public const ushort TaiwuBuildingAreas = 2;

		public const ushort CollectBuildingResourceType = 3;

		public const ushort BuildingOperatorDict = 4;

		public const ushort CustomBuildingName = 5;

		public const ushort NewCompleteOperationBuildings = 6;

		public const ushort ChickenBlessingInfoData = 7;

		public const ushort Chicken = 8;

		public const ushort CollectionCrickets = 9;

		public const ushort CollectionCricketJars = 10;

		public const ushort CollectionCricketRegen = 11;

		public const ushort MakeItemDict = 12;

		public const ushort Residences = 13;

		public const ushort ComfortableHouses = 14;

		public const ushort Homeless = 15;

		public const ushort SamsaraPlatformAddMainAttributes = 16;

		public const ushort SamsaraPlatformAddCombatSkillQualifications = 17;

		public const ushort SamsaraPlatformAddLifeSkillQualifications = 18;

		public const ushort SamsaraPlatformSlots = 19;

		public const ushort SamsaraPlatformBornDict = 20;

		public const ushort CollectBuildingEarningsData = 21;

		public const ushort ShopManagerDict = 22;

		public const ushort TeaHorseCaravanData = 23;

		public const ushort ShrineBuyTimes = 24;
	}

	public static class MethodIds
	{
		public const ushort GetShopEventDataList = 0;

		public const ushort SetShopManager = 1;

		public const ushort SetCollectBuildingResourceType = 2;

		public const ushort ClearBuildingBlockEarningsData = 3;

		public const ushort CalcResourceOutput = 4;

		public const ushort GetBuildingEarningData = 5;

		public const ushort GetBuildingOperatesData = 6;

		public const ushort GetBuildingBuildPeopleAttainments = 7;

		public const ushort AcceptBuildingBlockCollectEarning = 8;

		public const ushort AcceptBuildingBlockCollectEarningQuick = 9;

		public const ushort AcceptBuildingBlockRecruitPeople = 10;

		public const ushort AcceptBuildingBlockRecruitPeopleQuick = 11;

		public const ushort ShopBuildingSoldItemAdd = 12;

		public const ushort ShopBuildingSoldItemChange = 13;

		public const ushort ShopBuildingSoldItemReceive = 14;

		public const ushort ShopBuildingSoldItemReceiveQuick = 15;

		public const ushort QuickCollectShopItem = 16;

		public const ushort QuickCollectShopItemCount = 17;

		public const ushort QuickCollectShopSoldItem = 18;

		public const ushort QuickCollectShopSoldItemCount = 19;

		public const ushort QuickRecruitPeople = 20;

		public const ushort QuickRecruitPeopleCount = 21;

		public const ushort QuickCollectBuildingEarn = 22;

		public const ushort QuickCollectBuildingEarnCount = 23;

		public const ushort AddFixBook = 24;

		public const ushort ChangeFixBook = 25;

		public const ushort ReceiveFixBook = 26;

		public const ushort GetFixBookProgress = 27;

		public const ushort SetTeaHorseCaravanState = 28;

		public const ushort ExchangeItemToReplenishment = 29;

		public const ushort StartSearchReplenishment = 30;

		public const ushort QuickGetExchangeItem = 31;

		public const ushort GetShrineDisplayData = 32;

		public const ushort TeachSkill = 33;

		public const ushort CricketCollectionAdd = 34;

		public const ushort CricketCollectionRemove = 35;

		public const ushort GetCollectionCrickets = 36;

		public const ushort GetCollectionJars = 37;

		public const ushort GetCollectionCricketRegen = 38;

		public const ushort GetAuthorityGain = 39;

		public const ushort GmCmd_BuildImmediately = 40;

		public const ushort GmCmd_RemoveBuildingImmediately = 41;

		public const ushort StartMakeItem = 42;

		public const ushort CheckMakeCondition = 43;

		public const ushort GetMakeItems = 44;

		public const ushort GetMakingItemData = 45;

		public const ushort RepairItem = 46;

		public const ushort CheckRepairConditionIsMeet = 47;

		public const ushort AddItemPoison = 48;

		public const ushort CheckAddPoisonCondition = 49;

		public const ushort RemoveItemPoison = 50;

		public const ushort CheckRemovePoisonCondition = 51;

		public const ushort RefineItem = 52;

		public const ushort CheckRefineCondition = 53;

		public const ushort Build = 54;

		public const ushort Upgrade = 55;

		public const ushort Remove = 56;

		public const ushort SetStopOperation = 57;

		public const ushort SetOperator = 58;

		public const ushort SetBuildingMaintenance = 59;

		public const ushort Repair = 60;

		public const ushort ConfirmPlanBuilding = 61;

		public const ushort AddToResidence = 62;

		public const ushort RemoveFromResidence = 63;

		public const ushort ReplaceCharacterInResidence = 64;

		public const ushort ReplaceCharacterInComfortableHouse = 65;

		public const ushort AddToComfortableHouse = 66;

		public const ushort RemoveFromComfortableHouse = 67;

		public const ushort QuickFillResidence = 68;

		public const ushort GetCharsInResidence = 69;

		public const ushort GetAllResidents = 70;

		public const ushort GetCharsInComfortableHouse = 71;

		public const ushort GetHomeless = 72;

		public const ushort GetSamsaraPlatformCharList = 73;

		public const ushort SetSamsaraPlatformChar = 74;

		public const ushort SamsaraPlatformReborn = 75;

		public const ushort GetBuildingAreaData = 76;

		public const ushort GetBuildingBlockList = 77;

		public const ushort GetBuildingBlockData = 78;

		public const ushort SetBuildingCustomName = 79;

		public const ushort GetEmptyBlockCount = 80;

		public const ushort AddChicken = 81;

		public const ushort RemoveChicken = 82;

		public const ushort RemoveAllChicken = 83;

		public const ushort MoveChicken = 84;

		public const ushort TransferChicken = 85;

		public const ushort GetLocationChicken = 86;

		public const ushort GetSettlementChickenList = 87;

		public const ushort GetChickenData = 88;

		public const ushort FeedChicken = 89;

		public const ushort InitMapBlockChicken = 90;

		public const ushort IsHaveChickenKing = 91;

		public const ushort RemoveAllFormResidence = 92;

		public const ushort NearDependBuildings = 93;

		public const ushort GetBuildingAttainment = 94;

		public const ushort GetAttainmentOfBuilding = 95;

		public const ushort CalcResourceOutputCount = 96;

		public const ushort DealInfectedPeople = 97;

		public const ushort QuickCollectSingleShopItem = 98;

		public const ushort QuickCollectSingleShopSoldItem = 99;

		public const ushort QuickRecruitSingleBuildingPeople = 100;

		public const ushort QuickFillComfortableHouse = 101;

		public const ushort RemoveAllFromComfortableHouse = 102;

		public const ushort SortedComfortableHousePeople = 103;

		public const ushort GetMakeResult = 104;

		public const ushort GetSutraReadingRoomBuffValue = 105;

		public const ushort SetBuildingAutoWork = 106;

		public const ushort GetBuildingIsAutoWork = 107;

		public const ushort ShopBuildingMultiChangeSoldItem = 108;

		public const ushort RepairItemList = 109;

		public const ushort SetBuildingAutoSold = 110;

		public const ushort GetBuildingIsAutoSold = 111;

		public const ushort GetXiangshuIdInKungfuRoom = 112;

		public const ushort RepairItemOptional = 113;

		public const ushort SetShopIsResultFirst = 114;

		public const ushort GetShopIsResultFirst = 115;

		public const ushort SetBuildingAutoExpandUpTop = 116;

		public const ushort SetBuildingAutoExpandDown = 117;

		public const ushort GetBuildingIsAutoExpand = 118;

		public const ushort SetBuildingAutoExpand = 119;

		public const ushort SetBuildingAutoExpandDownBottom = 120;

		public const ushort SetBuildingAutoExpandUp = 121;

		public const ushort SetNickNameByChickenId = 122;

		public const ushort GetChickensNicknameByIdList = 123;

		public const ushort GetSettlementChickenDataList = 124;

		public const ushort SetTeaHorseCaravanWeather = 125;

		public const ushort GetComfortableIsAutoCheckIn = 126;

		public const ushort GetResidenceIsAutoCheckIn = 127;

		public const ushort SetComfortableAutoCheckIn = 128;

		public const ushort SetResidenceAutoCheckIn = 129;

		public const ushort GmCmd_AddLegacyBuilding = 130;

		public const ushort SetUnlockedWorkingVillagers = 131;

		public const ushort CanAutoExpand = 132;

		public const ushort WeaveClothingItem = 133;

		public const ushort GmCmd_GetChickenData = 134;

		public const ushort GetPossessionPreview = 135;

		public const ushort TrySwapSoulCeremony = 136;

		public const ushort GetBackTeaHorseCarryItem = 137;

		public const ushort AddItemToTeaHorseCarryItem = 138;

		public const ushort SetTemporaryPossessionCharacterAvatar = 139;

		public const ushort GetSwapSoulCeremonyBodyCharIdList = 140;

		public const ushort GetBuildingShopManagerAutoArrangeSorted = 141;

		public const ushort SectMainStoryJingangClickMonkSoulBtn = 142;

		public const ushort RejectBuildingBlockRecruitPeople = 143;

		public const ushort RejectBuildingBlockRecruitPeopleQuick = 144;

		public const ushort GetShopManagementYieldTipsData = 145;

		public const ushort CalculateBuildingManageHarvestSuccessRate = 146;

		public const ushort GetOrCreateShopEventCollection = 147;

		public const ushort GetSamsaraPlatformRecord = 148;

		public const ushort GetSwapSoulCeremonySoulCharIdList = 149;

		public const ushort CricketCollectionBatchAddCricketJar = 150;

		public const ushort CricketCollectionBatchAddCricket = 151;

		public const ushort CricketCollectionBatchRemoveJar = 152;

		public const ushort CricketCollectionBatchRemoveCricket = 153;

		public const ushort GetCricketOrJarFromSourceStorage = 154;

		public const ushort SmartOperateCricketOrJarCollection = 155;

		public const ushort GetBatchButtonEnableState = 156;

		public const ushort CalculateBuildingManageHarvestSuccessRates = 157;

		public const ushort Collect = 158;

		public const ushort UnsetFulongChicken = 159;

		public const ushort SetFulongChicken = 160;

		public const ushort GetChickenDataList = 161;

		public const ushort GetChickenNicknameList = 162;

		public const ushort GetSettlementChickenIdList = 163;

		public const ushort GetChickensNicknameByLocation = 164;

		public const ushort AllChickenInTaiwuVillage = 165;

		public const ushort GetVillagerRoleExtraEffectUnlockState = 166;

		public const ushort ClickChickenMap = 167;

		public const ushort ClickChickenSign = 168;

		public const ushort IsInFulongSeekFeatherTask = 169;

		public const ushort SetBuildingResourceOutputSetting = 170;

		public const ushort GetBuildingResourceOutputSetting = 171;

		public const ushort GetBuildingExceptionData = 172;

		public const ushort AllDependBuildingAvailable = 173;

		public const ushort PracticingCombatSkillInPracticeRoom = 174;

		public const ushort HasShopManagerLeader = 175;

		public const ushort QuickArrangeShopManager = 176;

		public const ushort QuickArrangeBuildOperator = 177;

		public const ushort ShopBuildingCanTeach = 178;

		public const ushort GetOperationLeftTime = 179;

		public const ushort GetBuildingOperationLeftTime = 180;

		public const ushort GetShopBuildingTeachBookData = 181;

		public const ushort CalcExtraTaiwuGroupMaxCountByStrategyRoom = 182;

		public const ushort GetTaiwuCanFixBookItemDataList = 183;

		public const ushort GetResidenceInfo = 184;

		public const ushort GetTaiwuVillageResourceBlockEffect = 185;

		public const ushort GetTaiwuLocationResourceBlockEffect = 186;

		public const ushort GetTaiwuVillageResourceBlockEffectInfo = 187;

		public const ushort CanQuickArrangeShopManager = 188;

		public const ushort GetBuildingFormulaContextBridge = 189;

		public const ushort GetBuildingEffectForMake = 190;

		public const ushort GmCmd_BuildingCollectPerform = 191;

		public const ushort GmCmd_BeatMinionPerform = 192;

		public const ushort GetStoreLocation = 193;

		public const ushort SetStoreLocation = 194;

		public const ushort GetFeastTargetCharList = 195;

		public const ushort TryShowNotifications = 196;

		public const ushort QuickRemoveShopSoldItem = 197;

		public const ushort QuickAddShopSoldItem = 198;
	}

	public const ushort DataCount = 25;

	public static readonly Dictionary<string, ushort> FieldName2DataId = new Dictionary<string, ushort>
	{
		{ "BuildingAreas", 0 },
		{ "BuildingBlocks", 1 },
		{ "TaiwuBuildingAreas", 2 },
		{ "CollectBuildingResourceType", 3 },
		{ "BuildingOperatorDict", 4 },
		{ "CustomBuildingName", 5 },
		{ "NewCompleteOperationBuildings", 6 },
		{ "ChickenBlessingInfoData", 7 },
		{ "Chicken", 8 },
		{ "CollectionCrickets", 9 },
		{ "CollectionCricketJars", 10 },
		{ "CollectionCricketRegen", 11 },
		{ "MakeItemDict", 12 },
		{ "Residences", 13 },
		{ "ComfortableHouses", 14 },
		{ "Homeless", 15 },
		{ "SamsaraPlatformAddMainAttributes", 16 },
		{ "SamsaraPlatformAddCombatSkillQualifications", 17 },
		{ "SamsaraPlatformAddLifeSkillQualifications", 18 },
		{ "SamsaraPlatformSlots", 19 },
		{ "SamsaraPlatformBornDict", 20 },
		{ "CollectBuildingEarningsData", 21 },
		{ "ShopManagerDict", 22 },
		{ "TeaHorseCaravanData", 23 },
		{ "ShrineBuyTimes", 24 }
	};

	public static readonly string[] DataId2FieldName = new string[25]
	{
		"BuildingAreas", "BuildingBlocks", "TaiwuBuildingAreas", "CollectBuildingResourceType", "BuildingOperatorDict", "CustomBuildingName", "NewCompleteOperationBuildings", "ChickenBlessingInfoData", "Chicken", "CollectionCrickets",
		"CollectionCricketJars", "CollectionCricketRegen", "MakeItemDict", "Residences", "ComfortableHouses", "Homeless", "SamsaraPlatformAddMainAttributes", "SamsaraPlatformAddCombatSkillQualifications", "SamsaraPlatformAddLifeSkillQualifications", "SamsaraPlatformSlots",
		"SamsaraPlatformBornDict", "CollectBuildingEarningsData", "ShopManagerDict", "TeaHorseCaravanData", "ShrineBuyTimes"
	};

	public static readonly string[][] DataId2ObjectFieldId2FieldName = new string[25][];

	public static readonly Dictionary<string, ushort> MethodName2MethodId = new Dictionary<string, ushort>
	{
		{ "GetShopEventDataList", 0 },
		{ "SetShopManager", 1 },
		{ "SetCollectBuildingResourceType", 2 },
		{ "ClearBuildingBlockEarningsData", 3 },
		{ "CalcResourceOutput", 4 },
		{ "GetBuildingEarningData", 5 },
		{ "GetBuildingOperatesData", 6 },
		{ "GetBuildingBuildPeopleAttainments", 7 },
		{ "AcceptBuildingBlockCollectEarning", 8 },
		{ "AcceptBuildingBlockCollectEarningQuick", 9 },
		{ "AcceptBuildingBlockRecruitPeople", 10 },
		{ "AcceptBuildingBlockRecruitPeopleQuick", 11 },
		{ "ShopBuildingSoldItemAdd", 12 },
		{ "ShopBuildingSoldItemChange", 13 },
		{ "ShopBuildingSoldItemReceive", 14 },
		{ "ShopBuildingSoldItemReceiveQuick", 15 },
		{ "QuickCollectShopItem", 16 },
		{ "QuickCollectShopItemCount", 17 },
		{ "QuickCollectShopSoldItem", 18 },
		{ "QuickCollectShopSoldItemCount", 19 },
		{ "QuickRecruitPeople", 20 },
		{ "QuickRecruitPeopleCount", 21 },
		{ "QuickCollectBuildingEarn", 22 },
		{ "QuickCollectBuildingEarnCount", 23 },
		{ "AddFixBook", 24 },
		{ "ChangeFixBook", 25 },
		{ "ReceiveFixBook", 26 },
		{ "GetFixBookProgress", 27 },
		{ "SetTeaHorseCaravanState", 28 },
		{ "ExchangeItemToReplenishment", 29 },
		{ "StartSearchReplenishment", 30 },
		{ "QuickGetExchangeItem", 31 },
		{ "GetShrineDisplayData", 32 },
		{ "TeachSkill", 33 },
		{ "CricketCollectionAdd", 34 },
		{ "CricketCollectionRemove", 35 },
		{ "GetCollectionCrickets", 36 },
		{ "GetCollectionJars", 37 },
		{ "GetCollectionCricketRegen", 38 },
		{ "GetAuthorityGain", 39 },
		{ "GmCmd_BuildImmediately", 40 },
		{ "GmCmd_RemoveBuildingImmediately", 41 },
		{ "StartMakeItem", 42 },
		{ "CheckMakeCondition", 43 },
		{ "GetMakeItems", 44 },
		{ "GetMakingItemData", 45 },
		{ "RepairItem", 46 },
		{ "CheckRepairConditionIsMeet", 47 },
		{ "AddItemPoison", 48 },
		{ "CheckAddPoisonCondition", 49 },
		{ "RemoveItemPoison", 50 },
		{ "CheckRemovePoisonCondition", 51 },
		{ "RefineItem", 52 },
		{ "CheckRefineCondition", 53 },
		{ "Build", 54 },
		{ "Upgrade", 55 },
		{ "Remove", 56 },
		{ "SetStopOperation", 57 },
		{ "SetOperator", 58 },
		{ "SetBuildingMaintenance", 59 },
		{ "Repair", 60 },
		{ "ConfirmPlanBuilding", 61 },
		{ "AddToResidence", 62 },
		{ "RemoveFromResidence", 63 },
		{ "ReplaceCharacterInResidence", 64 },
		{ "ReplaceCharacterInComfortableHouse", 65 },
		{ "AddToComfortableHouse", 66 },
		{ "RemoveFromComfortableHouse", 67 },
		{ "QuickFillResidence", 68 },
		{ "GetCharsInResidence", 69 },
		{ "GetAllResidents", 70 },
		{ "GetCharsInComfortableHouse", 71 },
		{ "GetHomeless", 72 },
		{ "GetSamsaraPlatformCharList", 73 },
		{ "SetSamsaraPlatformChar", 74 },
		{ "SamsaraPlatformReborn", 75 },
		{ "GetBuildingAreaData", 76 },
		{ "GetBuildingBlockList", 77 },
		{ "GetBuildingBlockData", 78 },
		{ "SetBuildingCustomName", 79 },
		{ "GetEmptyBlockCount", 80 },
		{ "AddChicken", 81 },
		{ "RemoveChicken", 82 },
		{ "RemoveAllChicken", 83 },
		{ "MoveChicken", 84 },
		{ "TransferChicken", 85 },
		{ "GetLocationChicken", 86 },
		{ "GetSettlementChickenList", 87 },
		{ "GetChickenData", 88 },
		{ "FeedChicken", 89 },
		{ "InitMapBlockChicken", 90 },
		{ "IsHaveChickenKing", 91 },
		{ "RemoveAllFormResidence", 92 },
		{ "NearDependBuildings", 93 },
		{ "GetBuildingAttainment", 94 },
		{ "GetAttainmentOfBuilding", 95 },
		{ "CalcResourceOutputCount", 96 },
		{ "DealInfectedPeople", 97 },
		{ "QuickCollectSingleShopItem", 98 },
		{ "QuickCollectSingleShopSoldItem", 99 },
		{ "QuickRecruitSingleBuildingPeople", 100 },
		{ "QuickFillComfortableHouse", 101 },
		{ "RemoveAllFromComfortableHouse", 102 },
		{ "SortedComfortableHousePeople", 103 },
		{ "GetMakeResult", 104 },
		{ "GetSutraReadingRoomBuffValue", 105 },
		{ "SetBuildingAutoWork", 106 },
		{ "GetBuildingIsAutoWork", 107 },
		{ "ShopBuildingMultiChangeSoldItem", 108 },
		{ "RepairItemList", 109 },
		{ "SetBuildingAutoSold", 110 },
		{ "GetBuildingIsAutoSold", 111 },
		{ "GetXiangshuIdInKungfuRoom", 112 },
		{ "RepairItemOptional", 113 },
		{ "SetShopIsResultFirst", 114 },
		{ "GetShopIsResultFirst", 115 },
		{ "SetBuildingAutoExpandUpTop", 116 },
		{ "SetBuildingAutoExpandDown", 117 },
		{ "GetBuildingIsAutoExpand", 118 },
		{ "SetBuildingAutoExpand", 119 },
		{ "SetBuildingAutoExpandDownBottom", 120 },
		{ "SetBuildingAutoExpandUp", 121 },
		{ "SetNickNameByChickenId", 122 },
		{ "GetChickensNicknameByIdList", 123 },
		{ "GetSettlementChickenDataList", 124 },
		{ "SetTeaHorseCaravanWeather", 125 },
		{ "GetComfortableIsAutoCheckIn", 126 },
		{ "GetResidenceIsAutoCheckIn", 127 },
		{ "SetComfortableAutoCheckIn", 128 },
		{ "SetResidenceAutoCheckIn", 129 },
		{ "GmCmd_AddLegacyBuilding", 130 },
		{ "SetUnlockedWorkingVillagers", 131 },
		{ "CanAutoExpand", 132 },
		{ "WeaveClothingItem", 133 },
		{ "GmCmd_GetChickenData", 134 },
		{ "GetPossessionPreview", 135 },
		{ "TrySwapSoulCeremony", 136 },
		{ "GetBackTeaHorseCarryItem", 137 },
		{ "AddItemToTeaHorseCarryItem", 138 },
		{ "SetTemporaryPossessionCharacterAvatar", 139 },
		{ "GetSwapSoulCeremonyBodyCharIdList", 140 },
		{ "GetBuildingShopManagerAutoArrangeSorted", 141 },
		{ "SectMainStoryJingangClickMonkSoulBtn", 142 },
		{ "RejectBuildingBlockRecruitPeople", 143 },
		{ "RejectBuildingBlockRecruitPeopleQuick", 144 },
		{ "GetShopManagementYieldTipsData", 145 },
		{ "CalculateBuildingManageHarvestSuccessRate", 146 },
		{ "GetOrCreateShopEventCollection", 147 },
		{ "GetSamsaraPlatformRecord", 148 },
		{ "GetSwapSoulCeremonySoulCharIdList", 149 },
		{ "CricketCollectionBatchAddCricketJar", 150 },
		{ "CricketCollectionBatchAddCricket", 151 },
		{ "CricketCollectionBatchRemoveJar", 152 },
		{ "CricketCollectionBatchRemoveCricket", 153 },
		{ "GetCricketOrJarFromSourceStorage", 154 },
		{ "SmartOperateCricketOrJarCollection", 155 },
		{ "GetBatchButtonEnableState", 156 },
		{ "CalculateBuildingManageHarvestSuccessRates", 157 },
		{ "Collect", 158 },
		{ "UnsetFulongChicken", 159 },
		{ "SetFulongChicken", 160 },
		{ "GetChickenDataList", 161 },
		{ "GetChickenNicknameList", 162 },
		{ "GetSettlementChickenIdList", 163 },
		{ "GetChickensNicknameByLocation", 164 },
		{ "AllChickenInTaiwuVillage", 165 },
		{ "GetVillagerRoleExtraEffectUnlockState", 166 },
		{ "ClickChickenMap", 167 },
		{ "ClickChickenSign", 168 },
		{ "IsInFulongSeekFeatherTask", 169 },
		{ "SetBuildingResourceOutputSetting", 170 },
		{ "GetBuildingResourceOutputSetting", 171 },
		{ "GetBuildingExceptionData", 172 },
		{ "AllDependBuildingAvailable", 173 },
		{ "PracticingCombatSkillInPracticeRoom", 174 },
		{ "HasShopManagerLeader", 175 },
		{ "QuickArrangeShopManager", 176 },
		{ "QuickArrangeBuildOperator", 177 },
		{ "ShopBuildingCanTeach", 178 },
		{ "GetOperationLeftTime", 179 },
		{ "GetBuildingOperationLeftTime", 180 },
		{ "GetShopBuildingTeachBookData", 181 },
		{ "CalcExtraTaiwuGroupMaxCountByStrategyRoom", 182 },
		{ "GetTaiwuCanFixBookItemDataList", 183 },
		{ "GetResidenceInfo", 184 },
		{ "GetTaiwuVillageResourceBlockEffect", 185 },
		{ "GetTaiwuLocationResourceBlockEffect", 186 },
		{ "GetTaiwuVillageResourceBlockEffectInfo", 187 },
		{ "CanQuickArrangeShopManager", 188 },
		{ "GetBuildingFormulaContextBridge", 189 },
		{ "GetBuildingEffectForMake", 190 },
		{ "GmCmd_BuildingCollectPerform", 191 },
		{ "GmCmd_BeatMinionPerform", 192 },
		{ "GetStoreLocation", 193 },
		{ "SetStoreLocation", 194 },
		{ "GetFeastTargetCharList", 195 },
		{ "TryShowNotifications", 196 },
		{ "QuickRemoveShopSoldItem", 197 },
		{ "QuickAddShopSoldItem", 198 }
	};

	public static readonly string[] MethodId2MethodName = new string[199]
	{
		"GetShopEventDataList", "SetShopManager", "SetCollectBuildingResourceType", "ClearBuildingBlockEarningsData", "CalcResourceOutput", "GetBuildingEarningData", "GetBuildingOperatesData", "GetBuildingBuildPeopleAttainments", "AcceptBuildingBlockCollectEarning", "AcceptBuildingBlockCollectEarningQuick",
		"AcceptBuildingBlockRecruitPeople", "AcceptBuildingBlockRecruitPeopleQuick", "ShopBuildingSoldItemAdd", "ShopBuildingSoldItemChange", "ShopBuildingSoldItemReceive", "ShopBuildingSoldItemReceiveQuick", "QuickCollectShopItem", "QuickCollectShopItemCount", "QuickCollectShopSoldItem", "QuickCollectShopSoldItemCount",
		"QuickRecruitPeople", "QuickRecruitPeopleCount", "QuickCollectBuildingEarn", "QuickCollectBuildingEarnCount", "AddFixBook", "ChangeFixBook", "ReceiveFixBook", "GetFixBookProgress", "SetTeaHorseCaravanState", "ExchangeItemToReplenishment",
		"StartSearchReplenishment", "QuickGetExchangeItem", "GetShrineDisplayData", "TeachSkill", "CricketCollectionAdd", "CricketCollectionRemove", "GetCollectionCrickets", "GetCollectionJars", "GetCollectionCricketRegen", "GetAuthorityGain",
		"GmCmd_BuildImmediately", "GmCmd_RemoveBuildingImmediately", "StartMakeItem", "CheckMakeCondition", "GetMakeItems", "GetMakingItemData", "RepairItem", "CheckRepairConditionIsMeet", "AddItemPoison", "CheckAddPoisonCondition",
		"RemoveItemPoison", "CheckRemovePoisonCondition", "RefineItem", "CheckRefineCondition", "Build", "Upgrade", "Remove", "SetStopOperation", "SetOperator", "SetBuildingMaintenance",
		"Repair", "ConfirmPlanBuilding", "AddToResidence", "RemoveFromResidence", "ReplaceCharacterInResidence", "ReplaceCharacterInComfortableHouse", "AddToComfortableHouse", "RemoveFromComfortableHouse", "QuickFillResidence", "GetCharsInResidence",
		"GetAllResidents", "GetCharsInComfortableHouse", "GetHomeless", "GetSamsaraPlatformCharList", "SetSamsaraPlatformChar", "SamsaraPlatformReborn", "GetBuildingAreaData", "GetBuildingBlockList", "GetBuildingBlockData", "SetBuildingCustomName",
		"GetEmptyBlockCount", "AddChicken", "RemoveChicken", "RemoveAllChicken", "MoveChicken", "TransferChicken", "GetLocationChicken", "GetSettlementChickenList", "GetChickenData", "FeedChicken",
		"InitMapBlockChicken", "IsHaveChickenKing", "RemoveAllFormResidence", "NearDependBuildings", "GetBuildingAttainment", "GetAttainmentOfBuilding", "CalcResourceOutputCount", "DealInfectedPeople", "QuickCollectSingleShopItem", "QuickCollectSingleShopSoldItem",
		"QuickRecruitSingleBuildingPeople", "QuickFillComfortableHouse", "RemoveAllFromComfortableHouse", "SortedComfortableHousePeople", "GetMakeResult", "GetSutraReadingRoomBuffValue", "SetBuildingAutoWork", "GetBuildingIsAutoWork", "ShopBuildingMultiChangeSoldItem", "RepairItemList",
		"SetBuildingAutoSold", "GetBuildingIsAutoSold", "GetXiangshuIdInKungfuRoom", "RepairItemOptional", "SetShopIsResultFirst", "GetShopIsResultFirst", "SetBuildingAutoExpandUpTop", "SetBuildingAutoExpandDown", "GetBuildingIsAutoExpand", "SetBuildingAutoExpand",
		"SetBuildingAutoExpandDownBottom", "SetBuildingAutoExpandUp", "SetNickNameByChickenId", "GetChickensNicknameByIdList", "GetSettlementChickenDataList", "SetTeaHorseCaravanWeather", "GetComfortableIsAutoCheckIn", "GetResidenceIsAutoCheckIn", "SetComfortableAutoCheckIn", "SetResidenceAutoCheckIn",
		"GmCmd_AddLegacyBuilding", "SetUnlockedWorkingVillagers", "CanAutoExpand", "WeaveClothingItem", "GmCmd_GetChickenData", "GetPossessionPreview", "TrySwapSoulCeremony", "GetBackTeaHorseCarryItem", "AddItemToTeaHorseCarryItem", "SetTemporaryPossessionCharacterAvatar",
		"GetSwapSoulCeremonyBodyCharIdList", "GetBuildingShopManagerAutoArrangeSorted", "SectMainStoryJingangClickMonkSoulBtn", "RejectBuildingBlockRecruitPeople", "RejectBuildingBlockRecruitPeopleQuick", "GetShopManagementYieldTipsData", "CalculateBuildingManageHarvestSuccessRate", "GetOrCreateShopEventCollection", "GetSamsaraPlatformRecord", "GetSwapSoulCeremonySoulCharIdList",
		"CricketCollectionBatchAddCricketJar", "CricketCollectionBatchAddCricket", "CricketCollectionBatchRemoveJar", "CricketCollectionBatchRemoveCricket", "GetCricketOrJarFromSourceStorage", "SmartOperateCricketOrJarCollection", "GetBatchButtonEnableState", "CalculateBuildingManageHarvestSuccessRates", "Collect", "UnsetFulongChicken",
		"SetFulongChicken", "GetChickenDataList", "GetChickenNicknameList", "GetSettlementChickenIdList", "GetChickensNicknameByLocation", "AllChickenInTaiwuVillage", "GetVillagerRoleExtraEffectUnlockState", "ClickChickenMap", "ClickChickenSign", "IsInFulongSeekFeatherTask",
		"SetBuildingResourceOutputSetting", "GetBuildingResourceOutputSetting", "GetBuildingExceptionData", "AllDependBuildingAvailable", "PracticingCombatSkillInPracticeRoom", "HasShopManagerLeader", "QuickArrangeShopManager", "QuickArrangeBuildOperator", "ShopBuildingCanTeach", "GetOperationLeftTime",
		"GetBuildingOperationLeftTime", "GetShopBuildingTeachBookData", "CalcExtraTaiwuGroupMaxCountByStrategyRoom", "GetTaiwuCanFixBookItemDataList", "GetResidenceInfo", "GetTaiwuVillageResourceBlockEffect", "GetTaiwuLocationResourceBlockEffect", "GetTaiwuVillageResourceBlockEffectInfo", "CanQuickArrangeShopManager", "GetBuildingFormulaContextBridge",
		"GetBuildingEffectForMake", "GmCmd_BuildingCollectPerform", "GmCmd_BeatMinionPerform", "GetStoreLocation", "SetStoreLocation", "GetFeastTargetCharList", "TryShowNotifications", "QuickRemoveShopSoldItem", "QuickAddShopSoldItem"
	};
}
