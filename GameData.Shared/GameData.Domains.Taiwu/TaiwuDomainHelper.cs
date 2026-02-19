using System.Collections.Generic;

namespace GameData.Domains.Taiwu;

public static class TaiwuDomainHelper
{
	public static class DataIds
	{
		public const ushort TaiwuCharId = 0;

		public const ushort TaiwuGenerationsCount = 1;

		public const ushort CricketLuckPoint = 2;

		public const ushort PreviousTaiwuIds = 3;

		public const ushort NeedToEscape = 4;

		public const ushort ReceivedItems = 5;

		public const ushort ReceivedCharacters = 6;

		public const ushort WarehouseItems = 7;

		public const ushort WarehouseMaxLoad = 8;

		public const ushort WarehouseCurrLoad = 9;

		public const ushort BuildingSpaceLimit = 10;

		public const ushort BuildingSpaceCurr = 11;

		public const ushort BuildingSpaceExtraAdd = 12;

		public const ushort ProsperousConstruction = 13;

		public const ushort CombatSkills = 14;

		public const ushort LifeSkills = 15;

		public const ushort CombatSkillPlans = 16;

		public const ushort CurrCombatSkillPlanId = 17;

		public const ushort CurrLifeSkillAttainmentPanelPlanIndex = 18;

		public const ushort SkillBreakPlateObsoleteDict = 19;

		public const ushort SkillBreakBonusDict = 20;

		public const ushort TeachTaiwuLifeSkillDict = 21;

		public const ushort TeachTaiwuCombatSkillDict = 22;

		public const ushort CombatSkillAttainmentPanelPlans = 23;

		public const ushort CurrCombatSkillAttainmentPanelPlanIds = 24;

		public const ushort MoveTimeCostPercent = 25;

		public const ushort WeaponInnerRatios = 26;

		public const ushort WeaponCurrInnerRatios = 27;

		public const ushort Appointments = 28;

		public const ushort BabyBonusMainAttributes = 29;

		public const ushort BabyBonusLifeSkillQualifications = 30;

		public const ushort BabyBonusCombatSkillQualifications = 31;

		public const ushort EquipmentsPlans = 32;

		public const ushort CurrEquipmentPlanId = 33;

		public const ushort GroupCharIds = 34;

		public const ushort CombatGroupCharIds = 35;

		public const ushort TaiwuGroupMaxCount = 36;

		public const ushort LegacyPointDict = 37;

		public const ushort LegacyPoint = 38;

		public const ushort AvailableLegacyList = 39;

		public const ushort LegacyPassingState = 40;

		public const ushort SuccessorCandidates = 41;

		public const ushort StateNewCharacterLegacyGrowingGrades = 42;

		public const ushort NotLearnCombatSkillReadingProgress = 43;

		public const ushort NotLearnLifeSkillReadingProgress = 44;

		public const ushort ReadingBooks = 45;

		public const ushort CurReadingBook = 46;

		public const ushort ReferenceBooks = 47;

		public const ushort ReferenceBookSlotUnlockStates = 48;

		public const ushort ReadingEventTriggered = 49;

		public const ushort ReadInCombatCount = 50;

		public const ushort HealingOuterInjuryRestriction = 51;

		public const ushort HealingInnerInjuryRestriction = 52;

		public const ushort NeiliAllocationTypeRestriction = 53;

		public const ushort VisitedSettlements = 54;

		public const ushort TaiwuVillageSettlementId = 55;

		public const ushort VillagerWork = 56;

		public const ushort VillagerWorkLocations = 57;

		public const ushort MaterialResourceMaxCount = 58;

		public const ushort ResourceChange = 59;

		public const ushort WorkLocationMaxCount = 60;

		public const ushort TotalVillagerCount = 61;

		public const ushort TotalAdultVillagerCount = 62;

		public const ushort AvailableVillagerCount = 63;

		public const ushort IsTaiwuDieOfCombatWithXiangshu = 64;

		public const ushort VillagerLearnLifeSkillsFromSect = 65;

		public const ushort VillagerLearnCombatSkillsFromSect = 66;

		public const ushort OverweightSanctionPercent = 67;

		public const ushort ReferenceSkillSlotUnlockStates = 68;

		public const ushort TaiwuGroupWorstInjuries = 69;

		public const ushort TotalResources = 70;

		public const ushort TaiwuSpecialGroup = 71;

		public const ushort TaiwuGearMateGroup = 72;

		public const ushort CanBreakOut = 73;

		public const ushort TroughMaxLoad = 74;

		public const ushort TroughCurrLoad = 75;

		public const ushort ClothingDurability = 76;
	}

	public static class MethodIds
	{
		public const ushort GetAllVisitedSettlements = 0;

		public const ushort SetVillagerCollectResourceWork = 1;

		public const ushort SetVillagerCollectTributeWork = 2;

		public const ushort SetVillagerKeepGraveWork = 3;

		public const ushort SetVillagerIdleWork = 4;

		public const ushort StopVillagerWork = 5;

		public const ushort StopVillagerCollectResourceWork = 6;

		public const ushort GetCollectResourceWorkDataList = 7;

		public const ushort ExpelVillager = 8;

		public const ushort GetVillagerStatusDisplayDataList = 9;

		public const ushort GetAllVillagersStatus = 10;

		public const ushort GetAllVillagersAvailableForWork = 11;

		public const ushort CalcResourceChangeByVillageWork = 12;

		public const ushort CalcResourceChangeByBuildingEarn = 13;

		public const ushort CalcResourceChangeByBuildingMaintain = 14;

		public const ushort GetAllWarehouseItems = 15;

		public const ushort GetWarehouseItemsBySubType = 16;

		public const ushort SwitchEquipmentPlan = 17;

		public const ushort GmCmd_AddResource = 18;

		public const ushort GmCmd_AddLegacyPoint = 19;

		public const ushort GmCmd_AddExp = 20;

		public const ushort GmCmd_SetTaiwuCombatSkillActiveState = 21;

		public const ushort JoinGroup = 22;

		public const ushort LeaveGroup = 23;

		public const ushort CompletePassingLegacy = 24;

		public const ushort SelectLegacy = 25;

		public const ushort FindSuccessorCandidates = 26;

		public const ushort ConfirmChosenSuccessor = 27;

		public const ushort SetReferenceBook = 28;

		public const ushort SetReadingBook = 29;

		public const ushort GetCurReadingStrategies = 30;

		public const ushort SetReadingStrategy = 31;

		public const ushort ClearPageStrategy = 32;

		public const ushort GetRandomSelectableStrategies = 33;

		public const ushort CheckNotInInventoryBooks = 34;

		public const ushort GetTotalReadingProgress = 35;

		public const ushort GetCurrReadingEventBonusRate = 36;

		public const ushort GetCurrReadingEfficiency = 37;

		public const ushort WarehouseAdd = 38;

		public const ushort WarehouseRemove = 39;

		public const ushort PutItemIntoWarehouse = 40;

		public const ushort TakeOutItemFromWarehouse = 41;

		public const ushort CanTransferItemToWarehouse = 42;

		public const ushort CalcBuildingResourceOutput = 43;

		public const ushort TransferAllItems = 44;

		public const ushort SelectCombatSkillAttainmentPanelPlan = 45;

		public const ushort GetGenericGridAllocation = 46;

		public const ushort AllocateGenericGrid = 47;

		public const ushort DeallocateGenericGrid = 48;

		public const ushort UpdateCombatSkillPlan = 49;

		public const ushort GetBreakPlateData = 50;

		public const ushort EnterSkillBreakPlate = 51;

		public const ushort ClearBreakPlate = 52;

		public const ushort SelectSkillBreakGrid = 53;

		public const ushort EscapeToAdjacentBlock = 54;

		public const ushort GetCanOperateItemDisplayDataInVillage = 55;

		public const ushort PutItemListIntoWarehouse = 56;

		public const ushort WarehouseAddList = 57;

		public const ushort TakeOutItemListFromWarehouse = 58;

		public const ushort WarehouseRemoveList = 59;

		public const ushort WarehouseDiscardItem = 60;

		public const ushort WarehouseDiscardItemList = 61;

		public const ushort GetTaiwuAllItems = 62;

		public const ushort TransferItem = 63;

		public const ushort GetAllTroughItems = 64;

		public const ushort TransferItemList = 65;

		public const ushort GetAllTreasuryItems = 66;

		public const ushort GetTotalReadingProgressList = 67;

		public const ushort CalcResourceChangeByAutoExpand = 68;

		public const ushort CalcAutoExpandNotSatisfyIndex = 69;

		public const ushort GetRelationCharacterCountOnBlock = 70;

		public const ushort ApplyLifeSkillCombatResult = 71;

		public const ushort PickLifeSkillCombatCharacterUseItem = 72;

		public const ushort PickLifeSkillCombatCharacterUseSecretInformation = 73;

		public const ushort GetConsumedCountOnBlock = 74;

		public const ushort GetRefBonusSpeed = 75;

		public const ushort FindTaiwuBuilding = 76;

		public const ushort ChoosyGetMaterial = 77;

		public const ushort GetCannotOperateItemDisplayDataInInventory = 78;

		public const ushort GetInventoryOverloadedGroupCharNames = 79;

		public const ushort SetAutoAllocateNeiliToMax = 80;

		public const ushort LifeSkillCombatCurrentPlayerGetNotUsableEffectCardTemplateIds = 81;

		public const ushort LifeSkillCombatStart = 82;

		public const ushort LifeSkillCombatTerminate = 83;

		public const ushort LifeSkillCombatCurrentPlayerCommitOperation = 84;

		public const ushort LifeSkillCombatCurrentPlayerGetUsableOperationList = 85;

		public const ushort LifeSkillCombatCurrentPlayerSimulateCancelOperation = 86;

		public const ushort LifeSkillCombatCurrentPlayerGetSecondPhaseUsableOperationList = 87;

		public const ushort LifeSkillCombatCurrentPlayerCalcUsableFirstPhaseEffectCardInfo = 88;

		public const ushort LifeSkillCombatCurrentPlayerSimulateCommitOperation = 89;

		public const ushort LifeSkillCombatCurrentPlayerAiPickOperation = 90;

		public const ushort LifeSkillCombatCurrentPlayerCommitOperationPreview = 91;

		public const ushort LifeSkillCombatCurrentPlayerAcceptForceSilent = 92;

		public const ushort MasteredSkillWillChangePlan = 93;

		public const ushort GetVillagersForWork = 94;

		public const ushort GetSeverelyInjuredGroupCharNames = 95;

		public const ushort GetItemCount = 96;

		public const ushort GetTaiwuVillagerMapBlockData = 97;

		public const ushort StopVillagerWorkOptional = 98;

		public const ushort GetLegacyMaxPointByType = 99;

		public const ushort GetCurrReadingBanByWug = 100;

		public const ushort GetPastLifeRelationCharacterCountOnBlock = 101;

		public const ushort GmCmd_MarkAllCarrierFullTamePoint = 102;

		public const ushort GetSelectMapBlockHasMerchantId = 103;

		public const ushort GmCmd_LifeSkillCombatAiSetAlwaysUseForceAdversary = 104;

		public const ushort ActiveReadOnce = 105;

		public const ushort ActiveNeigongLoopingOnce = 106;

		public const ushort AppendCombatSkillPlan = 107;

		public const ushort CopyCombatSkillPlan = 108;

		public const ushort ClearCombatSkillPlan = 109;

		public const ushort DeleteCombatSkillPlan = 110;

		public const ushort GetLegacyMaxPointAndTimesListByType = 111;

		public const ushort SetQiArtStrategy = 112;

		public const ushort GetCurrentBookAvailableReadingStrategies = 113;

		public const ushort GetLoopingNeigongQiArtStrategies = 114;

		public const ushort SetReferenceCombatSkillAt = 115;

		public const ushort GetLoopingNeigongQiArtStrategyDisplayDatas = 116;

		public const ushort GetLoopingNeigongAvailableQiArtStrategies = 117;

		public const ushort ClearCurrentLoopingNeigongEvent = 118;

		public const ushort SetTaiwuLoopingNeigong = 119;

		public const ushort DeleteTaiwuFeature = 120;

		public const ushort GmCmd_TaiwuActiveLoopingApply = 121;

		public const ushort GetIsFollowingNpcListMax = 122;

		public const ushort GetFollowingNpcListMaxCount = 123;

		public const ushort GmCmd_FollowRandomNpc = 124;

		public const ushort TaiwuFollowNpc = 125;

		public const ushort TaiwuUnfollowNpc = 126;

		public const ushort SetFollowingNpcNickName = 127;

		public const ushort GetFollowingNpcNickName = 128;

		public const ushort GetFollowingNpcNickNameId = 129;

		public const ushort GetVillagerRoleCharacterDisplayDataList = 130;

		public const ushort GetVillagerRoleCharacterDisplayData = 131;

		public const ushort BatchSetVillagerRole = 132;

		public const ushort DispatchVillagerArrangement = 133;

		public const ushort RecallVillager = 134;

		public const ushort AssignTargetItem = 135;

		public const ushort GetVillagerRoleDisplayData = 136;

		public const ushort AssignArrangementIncreaseOrDecrease = 137;

		public const ushort GetAllVillagerRoleDisplayData = 138;

		public const ushort GetAllItems = 139;

		public const ushort TransferResource = 140;

		public const ushort GetVillagerRoleTipsDisplayData = 141;

		public const ushort SetVillagerRole = 142;

		public const ushort SetVillagerMigrateWork = 143;

		public const ushort GetVillagerRoleNpcNickName = 144;

		public const ushort SetVillagerRoleNickName = 145;

		public const ushort GetVillagersAvailableForVillagerRole = 146;

		public const ushort GetAllResources = 147;

		public const ushort GetAllSwordTombDisplayDataForDispatch = 148;

		public const ushort GetVillagerRoleCharacterSlimDisplayData = 149;

		public const ushort GetAllWarehouseItemsExcludeValueZero = 150;

		public const ushort GmCmd_FillLegacyPoint = 151;

		public const ushort SetVillagerRoleActionSetting = 152;

		public const ushort GetVillagerRoleActionSetting = 153;

		public const ushort GetAllVillagerRoleActionSetting = 154;

		public const ushort GetVillagerRoleExecuteFixedActionFailReasons = 155;

		public const ushort SetMerchantType = 156;

		public const ushort GetMerchantType = 157;

		public const ushort GetProfessionTipDisplayData = 158;

		public const ushort GetExpByRereading = 159;

		public const ushort EnterMerchant = 160;

		public const ushort GetReadingResult = 161;

		public const ushort GetVillagerTreasuryNeed = 162;

		public const ushort GetVillagerClassesDict = 163;

		public const ushort GetVillagerListClassArray = 164;

		public const ushort GetTreasuryItemNeededCharDict = 165;

		public const ushort GetTreasuryNeededItemList = 166;

		public const ushort GetDyingGroupCharNames = 167;

		public const ushort GetBreakBaseCostExp = 168;

		public const ushort SetBonusRelation = 169;

		public const ushort SetBonusExp = 170;

		public const ushort SetBonusItem = 171;

		public const ushort SetActivePage = 172;

		public const ushort GetAvailableRelationBonuses = 173;

		public const ushort GetVillagerCollectStorageType = 174;

		public const ushort SetVillagerCollectStorageType = 175;

		public const ushort ClearBonus = 176;

		public const ushort TaiwuAddFeature = 177;

		public const ushort GetRandomLegaciesInGroup = 178;

		public const ushort GetGroupBabyCount = 179;

		public const ushort GetStrategyRoomLevel = 180;

		public const ushort SetBonusFriend = 181;

		public const ushort GmCmd_ShowUnlockedDebateStrategy = 182;

		public const ushort GmCmd_ChangeGamePoint = 183;

		public const ushort GmCmd_SetForceAiBribery = 184;

		public const ushort DebateGameOver = 185;

		public const ushort DebateGameSetTaiwuAi = 186;

		public const ushort DebateGameMakeMove = 187;

		public const ushort DebateGameNextState = 188;

		public const ushort DebateGamePickSpectators = 189;

		public const ushort DebateGameInitialize = 190;

		public const ushort GetAiBriberyDataOnPrepareLifeSkillCombat = 191;

		public const ushort DebateGameCastStrategy = 192;

		public const ushort GmCmd_GetDebateStrategyCard = 193;

		public const ushort GmCmd_ChangeStrategyPoint = 194;

		public const ushort GmCmd_ChangeBases = 195;

		public const ushort GetNewUnlockedDebateStrategyList = 196;

		public const ushort GmCmd_ChangePressure = 197;

		public const ushort DebateGameSetTaiwuSelectedCardTypes = 198;

		public const ushort DebateGameGetTaiwuSelectedCardTypes = 199;

		public const ushort GmCmd_AddAiOwnedCard = 200;

		public const ushort GmCmd_EmptyAiOwnedCard = 201;

		public const ushort DebateGameTryForceWin = 202;

		public const ushort SetVillagerDevelopWork = 203;

		public const ushort GetVillagerRoleCharacterDisplayDataOnPanel = 204;

		public const ushort GetIsTaiwuFirstByLuck = 205;

		public const ushort GmCmd_AddNodeEffect = 206;

		public const ushort GetVillagerFarmerMigrateResourceSuccessRateBonus = 207;

		public const ushort GetVillagerRoleHeadTotalAuthorityCost = 208;

		public const ushort GetAllChildAvailableForWork = 209;

		public const ushort GetTaiwuVillageSpaceLimitInfo = 210;

		public const ushort GetGroupNeiliConflictingCharDataList = 211;

		public const ushort DebateGameResetCards = 212;

		public const ushort DebateGameRemoveCards = 213;

		public const ushort SetLastCricketPlan = 214;

		public const ushort RequestValidCricketPlan = 215;

		public const ushort SetCricketPlan = 216;

		public const ushort ClearCricketPlan = 217;

		public const ushort GetLastCricketPlan = 218;
	}

	public const ushort DataCount = 77;

	public static readonly Dictionary<string, ushort> FieldName2DataId = new Dictionary<string, ushort>
	{
		{ "TaiwuCharId", 0 },
		{ "TaiwuGenerationsCount", 1 },
		{ "CricketLuckPoint", 2 },
		{ "PreviousTaiwuIds", 3 },
		{ "NeedToEscape", 4 },
		{ "ReceivedItems", 5 },
		{ "ReceivedCharacters", 6 },
		{ "WarehouseItems", 7 },
		{ "WarehouseMaxLoad", 8 },
		{ "WarehouseCurrLoad", 9 },
		{ "BuildingSpaceLimit", 10 },
		{ "BuildingSpaceCurr", 11 },
		{ "BuildingSpaceExtraAdd", 12 },
		{ "ProsperousConstruction", 13 },
		{ "CombatSkills", 14 },
		{ "LifeSkills", 15 },
		{ "CombatSkillPlans", 16 },
		{ "CurrCombatSkillPlanId", 17 },
		{ "CurrLifeSkillAttainmentPanelPlanIndex", 18 },
		{ "SkillBreakPlateObsoleteDict", 19 },
		{ "SkillBreakBonusDict", 20 },
		{ "TeachTaiwuLifeSkillDict", 21 },
		{ "TeachTaiwuCombatSkillDict", 22 },
		{ "CombatSkillAttainmentPanelPlans", 23 },
		{ "CurrCombatSkillAttainmentPanelPlanIds", 24 },
		{ "MoveTimeCostPercent", 25 },
		{ "WeaponInnerRatios", 26 },
		{ "WeaponCurrInnerRatios", 27 },
		{ "Appointments", 28 },
		{ "BabyBonusMainAttributes", 29 },
		{ "BabyBonusLifeSkillQualifications", 30 },
		{ "BabyBonusCombatSkillQualifications", 31 },
		{ "EquipmentsPlans", 32 },
		{ "CurrEquipmentPlanId", 33 },
		{ "GroupCharIds", 34 },
		{ "CombatGroupCharIds", 35 },
		{ "TaiwuGroupMaxCount", 36 },
		{ "LegacyPointDict", 37 },
		{ "LegacyPoint", 38 },
		{ "AvailableLegacyList", 39 },
		{ "LegacyPassingState", 40 },
		{ "SuccessorCandidates", 41 },
		{ "StateNewCharacterLegacyGrowingGrades", 42 },
		{ "NotLearnCombatSkillReadingProgress", 43 },
		{ "NotLearnLifeSkillReadingProgress", 44 },
		{ "ReadingBooks", 45 },
		{ "CurReadingBook", 46 },
		{ "ReferenceBooks", 47 },
		{ "ReferenceBookSlotUnlockStates", 48 },
		{ "ReadingEventTriggered", 49 },
		{ "ReadInCombatCount", 50 },
		{ "HealingOuterInjuryRestriction", 51 },
		{ "HealingInnerInjuryRestriction", 52 },
		{ "NeiliAllocationTypeRestriction", 53 },
		{ "VisitedSettlements", 54 },
		{ "TaiwuVillageSettlementId", 55 },
		{ "VillagerWork", 56 },
		{ "VillagerWorkLocations", 57 },
		{ "MaterialResourceMaxCount", 58 },
		{ "ResourceChange", 59 },
		{ "WorkLocationMaxCount", 60 },
		{ "TotalVillagerCount", 61 },
		{ "TotalAdultVillagerCount", 62 },
		{ "AvailableVillagerCount", 63 },
		{ "IsTaiwuDieOfCombatWithXiangshu", 64 },
		{ "VillagerLearnLifeSkillsFromSect", 65 },
		{ "VillagerLearnCombatSkillsFromSect", 66 },
		{ "OverweightSanctionPercent", 67 },
		{ "ReferenceSkillSlotUnlockStates", 68 },
		{ "TaiwuGroupWorstInjuries", 69 },
		{ "TotalResources", 70 },
		{ "TaiwuSpecialGroup", 71 },
		{ "TaiwuGearMateGroup", 72 },
		{ "CanBreakOut", 73 },
		{ "TroughMaxLoad", 74 },
		{ "TroughCurrLoad", 75 },
		{ "ClothingDurability", 76 }
	};

	public static readonly string[] DataId2FieldName = new string[77]
	{
		"TaiwuCharId", "TaiwuGenerationsCount", "CricketLuckPoint", "PreviousTaiwuIds", "NeedToEscape", "ReceivedItems", "ReceivedCharacters", "WarehouseItems", "WarehouseMaxLoad", "WarehouseCurrLoad",
		"BuildingSpaceLimit", "BuildingSpaceCurr", "BuildingSpaceExtraAdd", "ProsperousConstruction", "CombatSkills", "LifeSkills", "CombatSkillPlans", "CurrCombatSkillPlanId", "CurrLifeSkillAttainmentPanelPlanIndex", "SkillBreakPlateObsoleteDict",
		"SkillBreakBonusDict", "TeachTaiwuLifeSkillDict", "TeachTaiwuCombatSkillDict", "CombatSkillAttainmentPanelPlans", "CurrCombatSkillAttainmentPanelPlanIds", "MoveTimeCostPercent", "WeaponInnerRatios", "WeaponCurrInnerRatios", "Appointments", "BabyBonusMainAttributes",
		"BabyBonusLifeSkillQualifications", "BabyBonusCombatSkillQualifications", "EquipmentsPlans", "CurrEquipmentPlanId", "GroupCharIds", "CombatGroupCharIds", "TaiwuGroupMaxCount", "LegacyPointDict", "LegacyPoint", "AvailableLegacyList",
		"LegacyPassingState", "SuccessorCandidates", "StateNewCharacterLegacyGrowingGrades", "NotLearnCombatSkillReadingProgress", "NotLearnLifeSkillReadingProgress", "ReadingBooks", "CurReadingBook", "ReferenceBooks", "ReferenceBookSlotUnlockStates", "ReadingEventTriggered",
		"ReadInCombatCount", "HealingOuterInjuryRestriction", "HealingInnerInjuryRestriction", "NeiliAllocationTypeRestriction", "VisitedSettlements", "TaiwuVillageSettlementId", "VillagerWork", "VillagerWorkLocations", "MaterialResourceMaxCount", "ResourceChange",
		"WorkLocationMaxCount", "TotalVillagerCount", "TotalAdultVillagerCount", "AvailableVillagerCount", "IsTaiwuDieOfCombatWithXiangshu", "VillagerLearnLifeSkillsFromSect", "VillagerLearnCombatSkillsFromSect", "OverweightSanctionPercent", "ReferenceSkillSlotUnlockStates", "TaiwuGroupWorstInjuries",
		"TotalResources", "TaiwuSpecialGroup", "TaiwuGearMateGroup", "CanBreakOut", "TroughMaxLoad", "TroughCurrLoad", "ClothingDurability"
	};

	public static readonly string[][] DataId2ObjectFieldId2FieldName = new string[77][];

	public static readonly Dictionary<string, ushort> MethodName2MethodId = new Dictionary<string, ushort>
	{
		{ "GetAllVisitedSettlements", 0 },
		{ "SetVillagerCollectResourceWork", 1 },
		{ "SetVillagerCollectTributeWork", 2 },
		{ "SetVillagerKeepGraveWork", 3 },
		{ "SetVillagerIdleWork", 4 },
		{ "StopVillagerWork", 5 },
		{ "StopVillagerCollectResourceWork", 6 },
		{ "GetCollectResourceWorkDataList", 7 },
		{ "ExpelVillager", 8 },
		{ "GetVillagerStatusDisplayDataList", 9 },
		{ "GetAllVillagersStatus", 10 },
		{ "GetAllVillagersAvailableForWork", 11 },
		{ "CalcResourceChangeByVillageWork", 12 },
		{ "CalcResourceChangeByBuildingEarn", 13 },
		{ "CalcResourceChangeByBuildingMaintain", 14 },
		{ "GetAllWarehouseItems", 15 },
		{ "GetWarehouseItemsBySubType", 16 },
		{ "SwitchEquipmentPlan", 17 },
		{ "GmCmd_AddResource", 18 },
		{ "GmCmd_AddLegacyPoint", 19 },
		{ "GmCmd_AddExp", 20 },
		{ "GmCmd_SetTaiwuCombatSkillActiveState", 21 },
		{ "JoinGroup", 22 },
		{ "LeaveGroup", 23 },
		{ "CompletePassingLegacy", 24 },
		{ "SelectLegacy", 25 },
		{ "FindSuccessorCandidates", 26 },
		{ "ConfirmChosenSuccessor", 27 },
		{ "SetReferenceBook", 28 },
		{ "SetReadingBook", 29 },
		{ "GetCurReadingStrategies", 30 },
		{ "SetReadingStrategy", 31 },
		{ "ClearPageStrategy", 32 },
		{ "GetRandomSelectableStrategies", 33 },
		{ "CheckNotInInventoryBooks", 34 },
		{ "GetTotalReadingProgress", 35 },
		{ "GetCurrReadingEventBonusRate", 36 },
		{ "GetCurrReadingEfficiency", 37 },
		{ "WarehouseAdd", 38 },
		{ "WarehouseRemove", 39 },
		{ "PutItemIntoWarehouse", 40 },
		{ "TakeOutItemFromWarehouse", 41 },
		{ "CanTransferItemToWarehouse", 42 },
		{ "CalcBuildingResourceOutput", 43 },
		{ "TransferAllItems", 44 },
		{ "SelectCombatSkillAttainmentPanelPlan", 45 },
		{ "GetGenericGridAllocation", 46 },
		{ "AllocateGenericGrid", 47 },
		{ "DeallocateGenericGrid", 48 },
		{ "UpdateCombatSkillPlan", 49 },
		{ "GetBreakPlateData", 50 },
		{ "EnterSkillBreakPlate", 51 },
		{ "ClearBreakPlate", 52 },
		{ "SelectSkillBreakGrid", 53 },
		{ "EscapeToAdjacentBlock", 54 },
		{ "GetCanOperateItemDisplayDataInVillage", 55 },
		{ "PutItemListIntoWarehouse", 56 },
		{ "WarehouseAddList", 57 },
		{ "TakeOutItemListFromWarehouse", 58 },
		{ "WarehouseRemoveList", 59 },
		{ "WarehouseDiscardItem", 60 },
		{ "WarehouseDiscardItemList", 61 },
		{ "GetTaiwuAllItems", 62 },
		{ "TransferItem", 63 },
		{ "GetAllTroughItems", 64 },
		{ "TransferItemList", 65 },
		{ "GetAllTreasuryItems", 66 },
		{ "GetTotalReadingProgressList", 67 },
		{ "CalcResourceChangeByAutoExpand", 68 },
		{ "CalcAutoExpandNotSatisfyIndex", 69 },
		{ "GetRelationCharacterCountOnBlock", 70 },
		{ "ApplyLifeSkillCombatResult", 71 },
		{ "PickLifeSkillCombatCharacterUseItem", 72 },
		{ "PickLifeSkillCombatCharacterUseSecretInformation", 73 },
		{ "GetConsumedCountOnBlock", 74 },
		{ "GetRefBonusSpeed", 75 },
		{ "FindTaiwuBuilding", 76 },
		{ "ChoosyGetMaterial", 77 },
		{ "GetCannotOperateItemDisplayDataInInventory", 78 },
		{ "GetInventoryOverloadedGroupCharNames", 79 },
		{ "SetAutoAllocateNeiliToMax", 80 },
		{ "LifeSkillCombatCurrentPlayerGetNotUsableEffectCardTemplateIds", 81 },
		{ "LifeSkillCombatStart", 82 },
		{ "LifeSkillCombatTerminate", 83 },
		{ "LifeSkillCombatCurrentPlayerCommitOperation", 84 },
		{ "LifeSkillCombatCurrentPlayerGetUsableOperationList", 85 },
		{ "LifeSkillCombatCurrentPlayerSimulateCancelOperation", 86 },
		{ "LifeSkillCombatCurrentPlayerGetSecondPhaseUsableOperationList", 87 },
		{ "LifeSkillCombatCurrentPlayerCalcUsableFirstPhaseEffectCardInfo", 88 },
		{ "LifeSkillCombatCurrentPlayerSimulateCommitOperation", 89 },
		{ "LifeSkillCombatCurrentPlayerAiPickOperation", 90 },
		{ "LifeSkillCombatCurrentPlayerCommitOperationPreview", 91 },
		{ "LifeSkillCombatCurrentPlayerAcceptForceSilent", 92 },
		{ "MasteredSkillWillChangePlan", 93 },
		{ "GetVillagersForWork", 94 },
		{ "GetSeverelyInjuredGroupCharNames", 95 },
		{ "GetItemCount", 96 },
		{ "GetTaiwuVillagerMapBlockData", 97 },
		{ "StopVillagerWorkOptional", 98 },
		{ "GetLegacyMaxPointByType", 99 },
		{ "GetCurrReadingBanByWug", 100 },
		{ "GetPastLifeRelationCharacterCountOnBlock", 101 },
		{ "GmCmd_MarkAllCarrierFullTamePoint", 102 },
		{ "GetSelectMapBlockHasMerchantId", 103 },
		{ "GmCmd_LifeSkillCombatAiSetAlwaysUseForceAdversary", 104 },
		{ "ActiveReadOnce", 105 },
		{ "ActiveNeigongLoopingOnce", 106 },
		{ "AppendCombatSkillPlan", 107 },
		{ "CopyCombatSkillPlan", 108 },
		{ "ClearCombatSkillPlan", 109 },
		{ "DeleteCombatSkillPlan", 110 },
		{ "GetLegacyMaxPointAndTimesListByType", 111 },
		{ "SetQiArtStrategy", 112 },
		{ "GetCurrentBookAvailableReadingStrategies", 113 },
		{ "GetLoopingNeigongQiArtStrategies", 114 },
		{ "SetReferenceCombatSkillAt", 115 },
		{ "GetLoopingNeigongQiArtStrategyDisplayDatas", 116 },
		{ "GetLoopingNeigongAvailableQiArtStrategies", 117 },
		{ "ClearCurrentLoopingNeigongEvent", 118 },
		{ "SetTaiwuLoopingNeigong", 119 },
		{ "DeleteTaiwuFeature", 120 },
		{ "GmCmd_TaiwuActiveLoopingApply", 121 },
		{ "GetIsFollowingNpcListMax", 122 },
		{ "GetFollowingNpcListMaxCount", 123 },
		{ "GmCmd_FollowRandomNpc", 124 },
		{ "TaiwuFollowNpc", 125 },
		{ "TaiwuUnfollowNpc", 126 },
		{ "SetFollowingNpcNickName", 127 },
		{ "GetFollowingNpcNickName", 128 },
		{ "GetFollowingNpcNickNameId", 129 },
		{ "GetVillagerRoleCharacterDisplayDataList", 130 },
		{ "GetVillagerRoleCharacterDisplayData", 131 },
		{ "BatchSetVillagerRole", 132 },
		{ "DispatchVillagerArrangement", 133 },
		{ "RecallVillager", 134 },
		{ "AssignTargetItem", 135 },
		{ "GetVillagerRoleDisplayData", 136 },
		{ "AssignArrangementIncreaseOrDecrease", 137 },
		{ "GetAllVillagerRoleDisplayData", 138 },
		{ "GetAllItems", 139 },
		{ "TransferResource", 140 },
		{ "GetVillagerRoleTipsDisplayData", 141 },
		{ "SetVillagerRole", 142 },
		{ "SetVillagerMigrateWork", 143 },
		{ "GetVillagerRoleNpcNickName", 144 },
		{ "SetVillagerRoleNickName", 145 },
		{ "GetVillagersAvailableForVillagerRole", 146 },
		{ "GetAllResources", 147 },
		{ "GetAllSwordTombDisplayDataForDispatch", 148 },
		{ "GetVillagerRoleCharacterSlimDisplayData", 149 },
		{ "GetAllWarehouseItemsExcludeValueZero", 150 },
		{ "GmCmd_FillLegacyPoint", 151 },
		{ "SetVillagerRoleActionSetting", 152 },
		{ "GetVillagerRoleActionSetting", 153 },
		{ "GetAllVillagerRoleActionSetting", 154 },
		{ "GetVillagerRoleExecuteFixedActionFailReasons", 155 },
		{ "SetMerchantType", 156 },
		{ "GetMerchantType", 157 },
		{ "GetProfessionTipDisplayData", 158 },
		{ "GetExpByRereading", 159 },
		{ "EnterMerchant", 160 },
		{ "GetReadingResult", 161 },
		{ "GetVillagerTreasuryNeed", 162 },
		{ "GetVillagerClassesDict", 163 },
		{ "GetVillagerListClassArray", 164 },
		{ "GetTreasuryItemNeededCharDict", 165 },
		{ "GetTreasuryNeededItemList", 166 },
		{ "GetDyingGroupCharNames", 167 },
		{ "GetBreakBaseCostExp", 168 },
		{ "SetBonusRelation", 169 },
		{ "SetBonusExp", 170 },
		{ "SetBonusItem", 171 },
		{ "SetActivePage", 172 },
		{ "GetAvailableRelationBonuses", 173 },
		{ "GetVillagerCollectStorageType", 174 },
		{ "SetVillagerCollectStorageType", 175 },
		{ "ClearBonus", 176 },
		{ "TaiwuAddFeature", 177 },
		{ "GetRandomLegaciesInGroup", 178 },
		{ "GetGroupBabyCount", 179 },
		{ "GetStrategyRoomLevel", 180 },
		{ "SetBonusFriend", 181 },
		{ "GmCmd_ShowUnlockedDebateStrategy", 182 },
		{ "GmCmd_ChangeGamePoint", 183 },
		{ "GmCmd_SetForceAiBribery", 184 },
		{ "DebateGameOver", 185 },
		{ "DebateGameSetTaiwuAi", 186 },
		{ "DebateGameMakeMove", 187 },
		{ "DebateGameNextState", 188 },
		{ "DebateGamePickSpectators", 189 },
		{ "DebateGameInitialize", 190 },
		{ "GetAiBriberyDataOnPrepareLifeSkillCombat", 191 },
		{ "DebateGameCastStrategy", 192 },
		{ "GmCmd_GetDebateStrategyCard", 193 },
		{ "GmCmd_ChangeStrategyPoint", 194 },
		{ "GmCmd_ChangeBases", 195 },
		{ "GetNewUnlockedDebateStrategyList", 196 },
		{ "GmCmd_ChangePressure", 197 },
		{ "DebateGameSetTaiwuSelectedCardTypes", 198 },
		{ "DebateGameGetTaiwuSelectedCardTypes", 199 },
		{ "GmCmd_AddAiOwnedCard", 200 },
		{ "GmCmd_EmptyAiOwnedCard", 201 },
		{ "DebateGameTryForceWin", 202 },
		{ "SetVillagerDevelopWork", 203 },
		{ "GetVillagerRoleCharacterDisplayDataOnPanel", 204 },
		{ "GetIsTaiwuFirstByLuck", 205 },
		{ "GmCmd_AddNodeEffect", 206 },
		{ "GetVillagerFarmerMigrateResourceSuccessRateBonus", 207 },
		{ "GetVillagerRoleHeadTotalAuthorityCost", 208 },
		{ "GetAllChildAvailableForWork", 209 },
		{ "GetTaiwuVillageSpaceLimitInfo", 210 },
		{ "GetGroupNeiliConflictingCharDataList", 211 },
		{ "DebateGameResetCards", 212 },
		{ "DebateGameRemoveCards", 213 },
		{ "SetLastCricketPlan", 214 },
		{ "RequestValidCricketPlan", 215 },
		{ "SetCricketPlan", 216 },
		{ "ClearCricketPlan", 217 },
		{ "GetLastCricketPlan", 218 }
	};

	public static readonly string[] MethodId2MethodName = new string[219]
	{
		"GetAllVisitedSettlements", "SetVillagerCollectResourceWork", "SetVillagerCollectTributeWork", "SetVillagerKeepGraveWork", "SetVillagerIdleWork", "StopVillagerWork", "StopVillagerCollectResourceWork", "GetCollectResourceWorkDataList", "ExpelVillager", "GetVillagerStatusDisplayDataList",
		"GetAllVillagersStatus", "GetAllVillagersAvailableForWork", "CalcResourceChangeByVillageWork", "CalcResourceChangeByBuildingEarn", "CalcResourceChangeByBuildingMaintain", "GetAllWarehouseItems", "GetWarehouseItemsBySubType", "SwitchEquipmentPlan", "GmCmd_AddResource", "GmCmd_AddLegacyPoint",
		"GmCmd_AddExp", "GmCmd_SetTaiwuCombatSkillActiveState", "JoinGroup", "LeaveGroup", "CompletePassingLegacy", "SelectLegacy", "FindSuccessorCandidates", "ConfirmChosenSuccessor", "SetReferenceBook", "SetReadingBook",
		"GetCurReadingStrategies", "SetReadingStrategy", "ClearPageStrategy", "GetRandomSelectableStrategies", "CheckNotInInventoryBooks", "GetTotalReadingProgress", "GetCurrReadingEventBonusRate", "GetCurrReadingEfficiency", "WarehouseAdd", "WarehouseRemove",
		"PutItemIntoWarehouse", "TakeOutItemFromWarehouse", "CanTransferItemToWarehouse", "CalcBuildingResourceOutput", "TransferAllItems", "SelectCombatSkillAttainmentPanelPlan", "GetGenericGridAllocation", "AllocateGenericGrid", "DeallocateGenericGrid", "UpdateCombatSkillPlan",
		"GetBreakPlateData", "EnterSkillBreakPlate", "ClearBreakPlate", "SelectSkillBreakGrid", "EscapeToAdjacentBlock", "GetCanOperateItemDisplayDataInVillage", "PutItemListIntoWarehouse", "WarehouseAddList", "TakeOutItemListFromWarehouse", "WarehouseRemoveList",
		"WarehouseDiscardItem", "WarehouseDiscardItemList", "GetTaiwuAllItems", "TransferItem", "GetAllTroughItems", "TransferItemList", "GetAllTreasuryItems", "GetTotalReadingProgressList", "CalcResourceChangeByAutoExpand", "CalcAutoExpandNotSatisfyIndex",
		"GetRelationCharacterCountOnBlock", "ApplyLifeSkillCombatResult", "PickLifeSkillCombatCharacterUseItem", "PickLifeSkillCombatCharacterUseSecretInformation", "GetConsumedCountOnBlock", "GetRefBonusSpeed", "FindTaiwuBuilding", "ChoosyGetMaterial", "GetCannotOperateItemDisplayDataInInventory", "GetInventoryOverloadedGroupCharNames",
		"SetAutoAllocateNeiliToMax", "LifeSkillCombatCurrentPlayerGetNotUsableEffectCardTemplateIds", "LifeSkillCombatStart", "LifeSkillCombatTerminate", "LifeSkillCombatCurrentPlayerCommitOperation", "LifeSkillCombatCurrentPlayerGetUsableOperationList", "LifeSkillCombatCurrentPlayerSimulateCancelOperation", "LifeSkillCombatCurrentPlayerGetSecondPhaseUsableOperationList", "LifeSkillCombatCurrentPlayerCalcUsableFirstPhaseEffectCardInfo", "LifeSkillCombatCurrentPlayerSimulateCommitOperation",
		"LifeSkillCombatCurrentPlayerAiPickOperation", "LifeSkillCombatCurrentPlayerCommitOperationPreview", "LifeSkillCombatCurrentPlayerAcceptForceSilent", "MasteredSkillWillChangePlan", "GetVillagersForWork", "GetSeverelyInjuredGroupCharNames", "GetItemCount", "GetTaiwuVillagerMapBlockData", "StopVillagerWorkOptional", "GetLegacyMaxPointByType",
		"GetCurrReadingBanByWug", "GetPastLifeRelationCharacterCountOnBlock", "GmCmd_MarkAllCarrierFullTamePoint", "GetSelectMapBlockHasMerchantId", "GmCmd_LifeSkillCombatAiSetAlwaysUseForceAdversary", "ActiveReadOnce", "ActiveNeigongLoopingOnce", "AppendCombatSkillPlan", "CopyCombatSkillPlan", "ClearCombatSkillPlan",
		"DeleteCombatSkillPlan", "GetLegacyMaxPointAndTimesListByType", "SetQiArtStrategy", "GetCurrentBookAvailableReadingStrategies", "GetLoopingNeigongQiArtStrategies", "SetReferenceCombatSkillAt", "GetLoopingNeigongQiArtStrategyDisplayDatas", "GetLoopingNeigongAvailableQiArtStrategies", "ClearCurrentLoopingNeigongEvent", "SetTaiwuLoopingNeigong",
		"DeleteTaiwuFeature", "GmCmd_TaiwuActiveLoopingApply", "GetIsFollowingNpcListMax", "GetFollowingNpcListMaxCount", "GmCmd_FollowRandomNpc", "TaiwuFollowNpc", "TaiwuUnfollowNpc", "SetFollowingNpcNickName", "GetFollowingNpcNickName", "GetFollowingNpcNickNameId",
		"GetVillagerRoleCharacterDisplayDataList", "GetVillagerRoleCharacterDisplayData", "BatchSetVillagerRole", "DispatchVillagerArrangement", "RecallVillager", "AssignTargetItem", "GetVillagerRoleDisplayData", "AssignArrangementIncreaseOrDecrease", "GetAllVillagerRoleDisplayData", "GetAllItems",
		"TransferResource", "GetVillagerRoleTipsDisplayData", "SetVillagerRole", "SetVillagerMigrateWork", "GetVillagerRoleNpcNickName", "SetVillagerRoleNickName", "GetVillagersAvailableForVillagerRole", "GetAllResources", "GetAllSwordTombDisplayDataForDispatch", "GetVillagerRoleCharacterSlimDisplayData",
		"GetAllWarehouseItemsExcludeValueZero", "GmCmd_FillLegacyPoint", "SetVillagerRoleActionSetting", "GetVillagerRoleActionSetting", "GetAllVillagerRoleActionSetting", "GetVillagerRoleExecuteFixedActionFailReasons", "SetMerchantType", "GetMerchantType", "GetProfessionTipDisplayData", "GetExpByRereading",
		"EnterMerchant", "GetReadingResult", "GetVillagerTreasuryNeed", "GetVillagerClassesDict", "GetVillagerListClassArray", "GetTreasuryItemNeededCharDict", "GetTreasuryNeededItemList", "GetDyingGroupCharNames", "GetBreakBaseCostExp", "SetBonusRelation",
		"SetBonusExp", "SetBonusItem", "SetActivePage", "GetAvailableRelationBonuses", "GetVillagerCollectStorageType", "SetVillagerCollectStorageType", "ClearBonus", "TaiwuAddFeature", "GetRandomLegaciesInGroup", "GetGroupBabyCount",
		"GetStrategyRoomLevel", "SetBonusFriend", "GmCmd_ShowUnlockedDebateStrategy", "GmCmd_ChangeGamePoint", "GmCmd_SetForceAiBribery", "DebateGameOver", "DebateGameSetTaiwuAi", "DebateGameMakeMove", "DebateGameNextState", "DebateGamePickSpectators",
		"DebateGameInitialize", "GetAiBriberyDataOnPrepareLifeSkillCombat", "DebateGameCastStrategy", "GmCmd_GetDebateStrategyCard", "GmCmd_ChangeStrategyPoint", "GmCmd_ChangeBases", "GetNewUnlockedDebateStrategyList", "GmCmd_ChangePressure", "DebateGameSetTaiwuSelectedCardTypes", "DebateGameGetTaiwuSelectedCardTypes",
		"GmCmd_AddAiOwnedCard", "GmCmd_EmptyAiOwnedCard", "DebateGameTryForceWin", "SetVillagerDevelopWork", "GetVillagerRoleCharacterDisplayDataOnPanel", "GetIsTaiwuFirstByLuck", "GmCmd_AddNodeEffect", "GetVillagerFarmerMigrateResourceSuccessRateBonus", "GetVillagerRoleHeadTotalAuthorityCost", "GetAllChildAvailableForWork",
		"GetTaiwuVillageSpaceLimitInfo", "GetGroupNeiliConflictingCharDataList", "DebateGameResetCards", "DebateGameRemoveCards", "SetLastCricketPlan", "RequestValidCricketPlan", "SetCricketPlan", "ClearCricketPlan", "GetLastCricketPlan"
	};
}
