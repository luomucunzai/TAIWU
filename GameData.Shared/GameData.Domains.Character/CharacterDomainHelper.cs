using System.Collections.Generic;

namespace GameData.Domains.Character;

public static class CharacterDomainHelper
{
	public static class DataIds
	{
		public const ushort Objects = 0;

		public const ushort NextObjectId = 1;

		public const ushort DeadCharacters = 2;

		public const ushort DeadCharDeletionStates = 3;

		public const ushort RecentDeadCharacters = 4;

		public const ushort WaitingReincarnationChars = 5;

		public const ushort Graves = 6;

		public const ushort PregnantStates = 7;

		public const ushort PregnancyLockEndDates = 8;

		public const ushort UnguardedChars = 9;

		public const ushort KidnappedChars = 10;

		public const ushort Relations = 11;

		public const ushort ActualBloodParents = 12;

		public const ushort CharacterGroups = 13;

		public const ushort JoinGroupDates = 14;

		public const ushort DebtsToTaiwu = 15;

		public const ushort SoldLibrarySkillBooks = 16;

		public const ushort UsedCombatResourceObsoletes = 17;

		public const ushort AvatarElementGrowthProgress = 18;

		public const ushort TargetedForAssassination = 19;

		public const ushort PrioritizedActions = 20;

		public const ushort CrossAreaMoveInfos = 21;

		public const ushort OngoingVengeances = 22;

		public const ushort MournedOthersChars = 23;

		public const ushort PregeneratedCityTownGuards = 24;

		public const ushort PregeneratedRandomEnemies = 25;

		public const ushort ForceRebelLocation = 26;

		public const ushort ForceKindLocation = 27;

		public const ushort AvoidDeathCharId = 28;

		public const ushort SubscriberOrders = 29;
	}

	public static class MethodIds
	{
		public const ushort CreateProtagonist = 0;

		public const ushort GetRelatedCharactersForRelations = 1;

		public const ushort TryCreateRelation = 2;

		public const ushort GetGenealogy = 3;

		public const ushort GenerateRandomHanName = 4;

		public const ushort GenerateRandomZangName = 5;

		public const ushort GenerateRandomChildName = 6;

		public const ushort GetNameRelatedDataList = 7;

		public const ushort GetNameRelatedData = 8;

		public const ushort GetNameAndLifeRelatedDataList = 9;

		public const ushort GetNameAndLifeRelatedData = 10;

		public const ushort GetFavorability = 11;

		public const ushort GmCmd_GetAllGroupMembers = 12;

		public const ushort GmCmd_GenerateRandomRefinedItemToCharacter = 13;

		public const ushort GmCmd_ChangeInjury = 14;

		public const ushort GmCmd_ChangePoisonByType = 15;

		public const ushort GmCmd_ForgetCombatSkill = 16;

		public const ushort GmCmd_RevokeCombatSkill = 17;

		public const ushort GmCmd_SetLearnedLifeSkills = 18;

		public const ushort GmCmd_GetCricket = 19;

		public const ushort GmCmd_AddRelation = 20;

		public const ushort GetGroupSet = 21;

		public const ushort TransferResourcesWithDebt = 22;

		public const ushort TransferInventoryItemWithDebt = 23;

		public const ushort ChangeEquipment = 24;

		public const ushort CreateInventoryItem = 25;

		public const ushort GetInventoryItems = 26;

		public const ushort GetAllInventoryItems = 27;

		public const ushort GetInventoryItemAmount = 28;

		public const ushort GetAllEquipmentItems = 29;

		public const ushort GetInventoryItemDisplayData = 30;

		public const ushort InventoryContainsItem = 31;

		public const ushort AddEatingItem = 32;

		public const ushort GetCurrMaxEatingSlotsCount = 33;

		public const ushort MerchantHasNewGoods = 34;

		public const ushort AddKidnappedCharacter = 35;

		public const ushort TransferKidnappedCharacters = 36;

		public const ushort ChangeKidnappedCharacterRope = 37;

		public const ushort GetKidnapMaxSlotCount = 38;

		public const ushort TransferKidnappedCharacter = 39;

		public const ushort RemoveKidnappedCharacter = 40;

		public const ushort GetDisplayingAge = 41;

		public const ushort GetMainAttributesRecoveries = 42;

		public const ushort GetInscriptionStatus = 43;

		public const ushort GetCharacterBirthDate = 44;

		public const ushort GetMaxWorthCanBeLentToTaiwu = 45;

		public const ushort CheckFavorabilityBeforeTransferring = 46;

		public const ushort GetClothingDisplayId = 47;

		public const ushort GetCharacterDisplayDataList = 48;

		public const ushort GetCharacterLifeSkillAttainmentList = 49;

		public const ushort GetTaiwuRelatedGraveDisplayDataList = 50;

		public const ushort GetGraveDisplayDataList = 51;

		public const ushort GetCharacterDisplayDataListForRelations = 52;

		public const ushort GetSomeoneKidnapCharacters = 53;

		public const ushort GetCharacterAttributeDisplayData = 54;

		public const ushort GetCharacterSamsaraData = 55;

		public const ushort GetEquipmentCompareData = 56;

		public const ushort GetGroupCharDisplayDataList = 57;

		public const ushort GetDefeatMarkCountList = 58;

		public const ushort GetFeatureMedalValue = 59;

		public const ushort GetFeatureMedalValueList = 60;

		public const ushort GetFixedCharacterIdByTemplateId = 61;

		public const ushort GmCmd_SimulateNpcCombat = 62;

		public const ushort GmCmd_GetAllCharacterName = 63;

		public const ushort GmCmd_ChangeFavorability = 64;

		public const ushort GmCmd_ClearFameActionRecords = 65;

		public const ushort GmCmd_RecordFameAction = 66;

		public const ushort GmCmd_CreateRandomIntelligentCharacters = 67;

		public const ushort GmCmd_ForceChangeOrganization = 68;

		public const ushort GmCmd_ForceChangeGrade = 69;

		public const ushort GmCmd_Die = 70;

		public const ushort GmCmd_GetAliveCharByPreexistenceChar = 71;

		public const ushort GmCmd_LogCharacterSamsaraInfo = 72;

		public const ushort GmCmd_EditExtraNeiliAllocation = 73;

		public const ushort GmCmd_MakeCharacterKidnapped = 74;

		public const ushort GmCmd_MoveIntelligentCharacter = 75;

		public const ushort GmCmd_RandomizeRelationShipsInSettlement = 76;

		public const ushort GmCmd_MakeCharacterHaveSex = 77;

		public const ushort GmCmd_GetCharacterPregnancyLockEndDates = 78;

		public const ushort GmCmd_GetCharacterActualBloodParents = 79;

		public const ushort CharacterShaveAvatar = 80;

		public const ushort AllocateNeili = 81;

		public const ushort DeallocateNeili = 82;

		public const ushort GetChangeOfQiDisorder = 83;

		public const ushort GetUsableCombatResources = 84;

		public const ushort GetCombatSkillSlotCounts = 85;

		public const ushort SetCombatSkillSlot = 86;

		public const ushort GetCombatSkillAttainment = 87;

		public const ushort GetAllCombatSkillAttainment = 88;

		public const ushort GetLifeSkillAttainment = 89;

		public const ushort GetAllLifeSkillAttainment = 90;

		public const ushort LearnCombatSkill = 91;

		public const ushort LearnLifeSkill = 92;

		public const ushort TryGetDeadCharacter = 93;

		public const ushort GetTitles = 94;

		public const ushort GetHighestGradeCombatSkillById = 95;

		public const ushort GetLeftMaxHealth = 96;

		public const ushort GetHealthRecovery = 97;

		public const ushort GetAvatarRelatedDataList = 98;

		public const ushort GetAvatarData = 99;

		public const ushort TransferInventoryItemListWithDebt = 100;

		public const ushort GetFameType = 101;

		public const ushort GetAvatarRelatedData = 102;

		public const ushort GetCharacterListWisdomCount = 103;

		public const ushort SortCharacterListByMaxCombatSkill = 104;

		public const ushort GetCharacterMaxCombatSkillAttainment = 105;

		public const ushort GetDebts = 106;

		public const ushort GetItemPowerInfo = 107;

		public const ushort CalcMaxFavorabilityToTaiwuById = 108;

		public const ushort CheckDebtChange = 109;

		public const ushort GetCharacterWisdomCountById = 110;

		public const ushort TransferInventoryItemFromAToB = 111;

		public const ushort GmCmd_SetCurReadingEvent = 112;

		public const ushort GmCmd_SetAdventurePersonalities = 113;

		public const ushort GmCmd_AddFeature = 114;

		public const ushort GmCmd_SetFeatures = 115;

		public const ushort GmCmd_RemoveFeature = 116;

		public const ushort GmCmd_RemoveRelation = 117;

		public const ushort IsReclusive = 118;

		public const ushort GetInventoryEquipment = 119;

		public const ushort GetFilteredCharacterCounts = 120;

		public const ushort ClearCharacterSortFilter = 121;

		public const ushort UpdateSortFilterSettings = 122;

		public const ushort InitializeCharacterSortFilter = 123;

		public const ushort FindNameInCurrentSortFilter = 124;

		public const ushort GetCharacterDisplayDataListForUltimateSelect = 125;

		public const ushort GetMaxSortingTypeCharIds = 126;

		public const ushort GmCmd_SetCurrNeili = 127;

		public const ushort GmCmd_AddPoisonedInventoryItem = 128;

		public const ushort GmCmd_AddPoisonedEatingItem = 129;

		public const ushort GmCmd_SetCharBaseNeiliProportionOfFiveElements = 130;

		public const ushort GetRelationBetweenCharacters = 131;

		public const ushort GetInventoryItemsByItemType = 132;

		public const ushort GetCharacterDisplayData = 133;

		public const ushort UnequipAllCombatSkills = 134;

		public const ushort AutoEquipCombatSkills = 135;

		public const ushort GmCmd_AddCharacterExtraTitle = 136;

		public const ushort TryAddAndApplyOneWayRelation = 137;

		public const ushort MoveFuyuHiltLocation = 138;

		public const ushort TryRemoveOneWayRelation = 139;

		public const ushort GetCharacterLoveAndHateItemInfo = 140;

		public const ushort IsTemporaryIntelligentCharacter = 141;

		public const ushort GetMixedPoisonTypeRelatedMarkCountArray = 142;

		public const ushort SimulateEatingEffect = 143;

		public const ushort GetCharacterDisplayDataForTooltip = 144;

		public const ushort GmCmd_SetCurLoopingEvent = 145;

		public const ushort GetHealthType = 146;

		public const ushort CalcFavorabilityDelta = 147;

		public const ushort GetCharacterLocationDisplayData = 148;

		public const ushort GetCharacterDisplayDataForMapBlock = 149;

		public const ushort GetCharacterWisdomList = 150;

		public const ushort GetOrCreateSwordTombCharacterIdForNormalInformation = 151;

		public const ushort GetAllInventoryItemsExcludeValueZero = 152;

		public const ushort GetAddConsummateLevelRequiredMonth = 153;

		public const ushort GmCmd_ResetHairGrowth = 154;

		public const ushort GetAllItemsByAreaId = 155;

		public const ushort SetCombatSkillPlanLock = 156;

		public const ushort AppendCombatSkillPlan = 157;

		public const ushort DuplicateCurrentCombatSkillPlan = 158;

		public const ushort UpdateCombatSkillPlan = 159;

		public const ushort GetCurrentPlanIdAndPlanCount = 160;

		public const ushort DeleteCombatSkillPlan = 161;

		public const ushort IsInteractedWithTaiwu = 162;

		public const ushort IsNeiliAllocationLocked = 163;

		public const ushort AllocateGenericGrid = 164;

		public const ushort DeallocateGenericGrid = 165;

		public const ushort IsCombatSkillEquipmentLocked = 166;

		public const ushort AutoAllocateNeili = 167;

		public const ushort AutoSetCombatSkillAttainmentPanels = 168;

		public const ushort AutoEquipItems = 169;

		public const ushort SetCombatSkillAttainmentLock = 170;

		public const ushort IsCombatSkillAttainmentLocked = 171;

		public const ushort GetGenericGridAllocation = 172;

		public const ushort SetNeiliAllocationLock = 173;

		public const ushort RemoveEquippedCombatSkill = 174;

		public const ushort AddEquippedCombatSkill = 175;

		public const ushort GetCombatSkillExtraSlotCounts = 176;

		public const ushort GetCharacterAllBodyPartExists = 177;

		public const ushort GmCmd_ChangeCharDisorderOfQi = 178;

		public const ushort GenerateRandomName = 179;

		public const ushort GetCharacterDisplayDataListForRelationsWithRelationType = 180;

		public const ushort GetPhysiologicalAge = 181;

		public const ushort GetPhysiologicalAgeAffector = 182;

		public const ushort GetKidnappedCharacterDisplayData = 183;

		public const ushort CalcItemsFavorabilityDelta = 184;

		public const ushort IsCarrierDurabilityRunningOut = 185;

		public const ushort GmCmd_ForceChangeOrganizationByName = 186;
	}

	public const ushort DataCount = 30;

	public static readonly Dictionary<string, ushort> FieldName2DataId = new Dictionary<string, ushort>
	{
		{ "Objects", 0 },
		{ "NextObjectId", 1 },
		{ "DeadCharacters", 2 },
		{ "DeadCharDeletionStates", 3 },
		{ "RecentDeadCharacters", 4 },
		{ "WaitingReincarnationChars", 5 },
		{ "Graves", 6 },
		{ "PregnantStates", 7 },
		{ "PregnancyLockEndDates", 8 },
		{ "UnguardedChars", 9 },
		{ "KidnappedChars", 10 },
		{ "Relations", 11 },
		{ "ActualBloodParents", 12 },
		{ "CharacterGroups", 13 },
		{ "JoinGroupDates", 14 },
		{ "DebtsToTaiwu", 15 },
		{ "SoldLibrarySkillBooks", 16 },
		{ "UsedCombatResourceObsoletes", 17 },
		{ "AvatarElementGrowthProgress", 18 },
		{ "TargetedForAssassination", 19 },
		{ "PrioritizedActions", 20 },
		{ "CrossAreaMoveInfos", 21 },
		{ "OngoingVengeances", 22 },
		{ "MournedOthersChars", 23 },
		{ "PregeneratedCityTownGuards", 24 },
		{ "PregeneratedRandomEnemies", 25 },
		{ "ForceRebelLocation", 26 },
		{ "ForceKindLocation", 27 },
		{ "AvoidDeathCharId", 28 },
		{ "SubscriberOrders", 29 }
	};

	public static readonly string[] DataId2FieldName = new string[30]
	{
		"Objects", "NextObjectId", "DeadCharacters", "DeadCharDeletionStates", "RecentDeadCharacters", "WaitingReincarnationChars", "Graves", "PregnantStates", "PregnancyLockEndDates", "UnguardedChars",
		"KidnappedChars", "Relations", "ActualBloodParents", "CharacterGroups", "JoinGroupDates", "DebtsToTaiwu", "SoldLibrarySkillBooks", "UsedCombatResourceObsoletes", "AvatarElementGrowthProgress", "TargetedForAssassination",
		"PrioritizedActions", "CrossAreaMoveInfos", "OngoingVengeances", "MournedOthersChars", "PregeneratedCityTownGuards", "PregeneratedRandomEnemies", "ForceRebelLocation", "ForceKindLocation", "AvoidDeathCharId", "SubscriberOrders"
	};

	public static readonly string[][] DataId2ObjectFieldId2FieldName;

	public static readonly Dictionary<string, ushort> MethodName2MethodId;

	public static readonly string[] MethodId2MethodName;

	static CharacterDomainHelper()
	{
		string[][] array = new string[30][];
		array[0] = CharacterHelper.FieldId2FieldName;
		array[6] = GraveHelper.FieldId2FieldName;
		DataId2ObjectFieldId2FieldName = array;
		MethodName2MethodId = new Dictionary<string, ushort>
		{
			{ "CreateProtagonist", 0 },
			{ "GetRelatedCharactersForRelations", 1 },
			{ "TryCreateRelation", 2 },
			{ "GetGenealogy", 3 },
			{ "GenerateRandomHanName", 4 },
			{ "GenerateRandomZangName", 5 },
			{ "GenerateRandomChildName", 6 },
			{ "GetNameRelatedDataList", 7 },
			{ "GetNameRelatedData", 8 },
			{ "GetNameAndLifeRelatedDataList", 9 },
			{ "GetNameAndLifeRelatedData", 10 },
			{ "GetFavorability", 11 },
			{ "GmCmd_GetAllGroupMembers", 12 },
			{ "GmCmd_GenerateRandomRefinedItemToCharacter", 13 },
			{ "GmCmd_ChangeInjury", 14 },
			{ "GmCmd_ChangePoisonByType", 15 },
			{ "GmCmd_ForgetCombatSkill", 16 },
			{ "GmCmd_RevokeCombatSkill", 17 },
			{ "GmCmd_SetLearnedLifeSkills", 18 },
			{ "GmCmd_GetCricket", 19 },
			{ "GmCmd_AddRelation", 20 },
			{ "GetGroupSet", 21 },
			{ "TransferResourcesWithDebt", 22 },
			{ "TransferInventoryItemWithDebt", 23 },
			{ "ChangeEquipment", 24 },
			{ "CreateInventoryItem", 25 },
			{ "GetInventoryItems", 26 },
			{ "GetAllInventoryItems", 27 },
			{ "GetInventoryItemAmount", 28 },
			{ "GetAllEquipmentItems", 29 },
			{ "GetInventoryItemDisplayData", 30 },
			{ "InventoryContainsItem", 31 },
			{ "AddEatingItem", 32 },
			{ "GetCurrMaxEatingSlotsCount", 33 },
			{ "MerchantHasNewGoods", 34 },
			{ "AddKidnappedCharacter", 35 },
			{ "TransferKidnappedCharacters", 36 },
			{ "ChangeKidnappedCharacterRope", 37 },
			{ "GetKidnapMaxSlotCount", 38 },
			{ "TransferKidnappedCharacter", 39 },
			{ "RemoveKidnappedCharacter", 40 },
			{ "GetDisplayingAge", 41 },
			{ "GetMainAttributesRecoveries", 42 },
			{ "GetInscriptionStatus", 43 },
			{ "GetCharacterBirthDate", 44 },
			{ "GetMaxWorthCanBeLentToTaiwu", 45 },
			{ "CheckFavorabilityBeforeTransferring", 46 },
			{ "GetClothingDisplayId", 47 },
			{ "GetCharacterDisplayDataList", 48 },
			{ "GetCharacterLifeSkillAttainmentList", 49 },
			{ "GetTaiwuRelatedGraveDisplayDataList", 50 },
			{ "GetGraveDisplayDataList", 51 },
			{ "GetCharacterDisplayDataListForRelations", 52 },
			{ "GetSomeoneKidnapCharacters", 53 },
			{ "GetCharacterAttributeDisplayData", 54 },
			{ "GetCharacterSamsaraData", 55 },
			{ "GetEquipmentCompareData", 56 },
			{ "GetGroupCharDisplayDataList", 57 },
			{ "GetDefeatMarkCountList", 58 },
			{ "GetFeatureMedalValue", 59 },
			{ "GetFeatureMedalValueList", 60 },
			{ "GetFixedCharacterIdByTemplateId", 61 },
			{ "GmCmd_SimulateNpcCombat", 62 },
			{ "GmCmd_GetAllCharacterName", 63 },
			{ "GmCmd_ChangeFavorability", 64 },
			{ "GmCmd_ClearFameActionRecords", 65 },
			{ "GmCmd_RecordFameAction", 66 },
			{ "GmCmd_CreateRandomIntelligentCharacters", 67 },
			{ "GmCmd_ForceChangeOrganization", 68 },
			{ "GmCmd_ForceChangeGrade", 69 },
			{ "GmCmd_Die", 70 },
			{ "GmCmd_GetAliveCharByPreexistenceChar", 71 },
			{ "GmCmd_LogCharacterSamsaraInfo", 72 },
			{ "GmCmd_EditExtraNeiliAllocation", 73 },
			{ "GmCmd_MakeCharacterKidnapped", 74 },
			{ "GmCmd_MoveIntelligentCharacter", 75 },
			{ "GmCmd_RandomizeRelationShipsInSettlement", 76 },
			{ "GmCmd_MakeCharacterHaveSex", 77 },
			{ "GmCmd_GetCharacterPregnancyLockEndDates", 78 },
			{ "GmCmd_GetCharacterActualBloodParents", 79 },
			{ "CharacterShaveAvatar", 80 },
			{ "AllocateNeili", 81 },
			{ "DeallocateNeili", 82 },
			{ "GetChangeOfQiDisorder", 83 },
			{ "GetUsableCombatResources", 84 },
			{ "GetCombatSkillSlotCounts", 85 },
			{ "SetCombatSkillSlot", 86 },
			{ "GetCombatSkillAttainment", 87 },
			{ "GetAllCombatSkillAttainment", 88 },
			{ "GetLifeSkillAttainment", 89 },
			{ "GetAllLifeSkillAttainment", 90 },
			{ "LearnCombatSkill", 91 },
			{ "LearnLifeSkill", 92 },
			{ "TryGetDeadCharacter", 93 },
			{ "GetTitles", 94 },
			{ "GetHighestGradeCombatSkillById", 95 },
			{ "GetLeftMaxHealth", 96 },
			{ "GetHealthRecovery", 97 },
			{ "GetAvatarRelatedDataList", 98 },
			{ "GetAvatarData", 99 },
			{ "TransferInventoryItemListWithDebt", 100 },
			{ "GetFameType", 101 },
			{ "GetAvatarRelatedData", 102 },
			{ "GetCharacterListWisdomCount", 103 },
			{ "SortCharacterListByMaxCombatSkill", 104 },
			{ "GetCharacterMaxCombatSkillAttainment", 105 },
			{ "GetDebts", 106 },
			{ "GetItemPowerInfo", 107 },
			{ "CalcMaxFavorabilityToTaiwuById", 108 },
			{ "CheckDebtChange", 109 },
			{ "GetCharacterWisdomCountById", 110 },
			{ "TransferInventoryItemFromAToB", 111 },
			{ "GmCmd_SetCurReadingEvent", 112 },
			{ "GmCmd_SetAdventurePersonalities", 113 },
			{ "GmCmd_AddFeature", 114 },
			{ "GmCmd_SetFeatures", 115 },
			{ "GmCmd_RemoveFeature", 116 },
			{ "GmCmd_RemoveRelation", 117 },
			{ "IsReclusive", 118 },
			{ "GetInventoryEquipment", 119 },
			{ "GetFilteredCharacterCounts", 120 },
			{ "ClearCharacterSortFilter", 121 },
			{ "UpdateSortFilterSettings", 122 },
			{ "InitializeCharacterSortFilter", 123 },
			{ "FindNameInCurrentSortFilter", 124 },
			{ "GetCharacterDisplayDataListForUltimateSelect", 125 },
			{ "GetMaxSortingTypeCharIds", 126 },
			{ "GmCmd_SetCurrNeili", 127 },
			{ "GmCmd_AddPoisonedInventoryItem", 128 },
			{ "GmCmd_AddPoisonedEatingItem", 129 },
			{ "GmCmd_SetCharBaseNeiliProportionOfFiveElements", 130 },
			{ "GetRelationBetweenCharacters", 131 },
			{ "GetInventoryItemsByItemType", 132 },
			{ "GetCharacterDisplayData", 133 },
			{ "UnequipAllCombatSkills", 134 },
			{ "AutoEquipCombatSkills", 135 },
			{ "GmCmd_AddCharacterExtraTitle", 136 },
			{ "TryAddAndApplyOneWayRelation", 137 },
			{ "MoveFuyuHiltLocation", 138 },
			{ "TryRemoveOneWayRelation", 139 },
			{ "GetCharacterLoveAndHateItemInfo", 140 },
			{ "IsTemporaryIntelligentCharacter", 141 },
			{ "GetMixedPoisonTypeRelatedMarkCountArray", 142 },
			{ "SimulateEatingEffect", 143 },
			{ "GetCharacterDisplayDataForTooltip", 144 },
			{ "GmCmd_SetCurLoopingEvent", 145 },
			{ "GetHealthType", 146 },
			{ "CalcFavorabilityDelta", 147 },
			{ "GetCharacterLocationDisplayData", 148 },
			{ "GetCharacterDisplayDataForMapBlock", 149 },
			{ "GetCharacterWisdomList", 150 },
			{ "GetOrCreateSwordTombCharacterIdForNormalInformation", 151 },
			{ "GetAllInventoryItemsExcludeValueZero", 152 },
			{ "GetAddConsummateLevelRequiredMonth", 153 },
			{ "GmCmd_ResetHairGrowth", 154 },
			{ "GetAllItemsByAreaId", 155 },
			{ "SetCombatSkillPlanLock", 156 },
			{ "AppendCombatSkillPlan", 157 },
			{ "DuplicateCurrentCombatSkillPlan", 158 },
			{ "UpdateCombatSkillPlan", 159 },
			{ "GetCurrentPlanIdAndPlanCount", 160 },
			{ "DeleteCombatSkillPlan", 161 },
			{ "IsInteractedWithTaiwu", 162 },
			{ "IsNeiliAllocationLocked", 163 },
			{ "AllocateGenericGrid", 164 },
			{ "DeallocateGenericGrid", 165 },
			{ "IsCombatSkillEquipmentLocked", 166 },
			{ "AutoAllocateNeili", 167 },
			{ "AutoSetCombatSkillAttainmentPanels", 168 },
			{ "AutoEquipItems", 169 },
			{ "SetCombatSkillAttainmentLock", 170 },
			{ "IsCombatSkillAttainmentLocked", 171 },
			{ "GetGenericGridAllocation", 172 },
			{ "SetNeiliAllocationLock", 173 },
			{ "RemoveEquippedCombatSkill", 174 },
			{ "AddEquippedCombatSkill", 175 },
			{ "GetCombatSkillExtraSlotCounts", 176 },
			{ "GetCharacterAllBodyPartExists", 177 },
			{ "GmCmd_ChangeCharDisorderOfQi", 178 },
			{ "GenerateRandomName", 179 },
			{ "GetCharacterDisplayDataListForRelationsWithRelationType", 180 },
			{ "GetPhysiologicalAge", 181 },
			{ "GetPhysiologicalAgeAffector", 182 },
			{ "GetKidnappedCharacterDisplayData", 183 },
			{ "CalcItemsFavorabilityDelta", 184 },
			{ "IsCarrierDurabilityRunningOut", 185 },
			{ "GmCmd_ForceChangeOrganizationByName", 186 }
		};
		MethodId2MethodName = new string[187]
		{
			"CreateProtagonist", "GetRelatedCharactersForRelations", "TryCreateRelation", "GetGenealogy", "GenerateRandomHanName", "GenerateRandomZangName", "GenerateRandomChildName", "GetNameRelatedDataList", "GetNameRelatedData", "GetNameAndLifeRelatedDataList",
			"GetNameAndLifeRelatedData", "GetFavorability", "GmCmd_GetAllGroupMembers", "GmCmd_GenerateRandomRefinedItemToCharacter", "GmCmd_ChangeInjury", "GmCmd_ChangePoisonByType", "GmCmd_ForgetCombatSkill", "GmCmd_RevokeCombatSkill", "GmCmd_SetLearnedLifeSkills", "GmCmd_GetCricket",
			"GmCmd_AddRelation", "GetGroupSet", "TransferResourcesWithDebt", "TransferInventoryItemWithDebt", "ChangeEquipment", "CreateInventoryItem", "GetInventoryItems", "GetAllInventoryItems", "GetInventoryItemAmount", "GetAllEquipmentItems",
			"GetInventoryItemDisplayData", "InventoryContainsItem", "AddEatingItem", "GetCurrMaxEatingSlotsCount", "MerchantHasNewGoods", "AddKidnappedCharacter", "TransferKidnappedCharacters", "ChangeKidnappedCharacterRope", "GetKidnapMaxSlotCount", "TransferKidnappedCharacter",
			"RemoveKidnappedCharacter", "GetDisplayingAge", "GetMainAttributesRecoveries", "GetInscriptionStatus", "GetCharacterBirthDate", "GetMaxWorthCanBeLentToTaiwu", "CheckFavorabilityBeforeTransferring", "GetClothingDisplayId", "GetCharacterDisplayDataList", "GetCharacterLifeSkillAttainmentList",
			"GetTaiwuRelatedGraveDisplayDataList", "GetGraveDisplayDataList", "GetCharacterDisplayDataListForRelations", "GetSomeoneKidnapCharacters", "GetCharacterAttributeDisplayData", "GetCharacterSamsaraData", "GetEquipmentCompareData", "GetGroupCharDisplayDataList", "GetDefeatMarkCountList", "GetFeatureMedalValue",
			"GetFeatureMedalValueList", "GetFixedCharacterIdByTemplateId", "GmCmd_SimulateNpcCombat", "GmCmd_GetAllCharacterName", "GmCmd_ChangeFavorability", "GmCmd_ClearFameActionRecords", "GmCmd_RecordFameAction", "GmCmd_CreateRandomIntelligentCharacters", "GmCmd_ForceChangeOrganization", "GmCmd_ForceChangeGrade",
			"GmCmd_Die", "GmCmd_GetAliveCharByPreexistenceChar", "GmCmd_LogCharacterSamsaraInfo", "GmCmd_EditExtraNeiliAllocation", "GmCmd_MakeCharacterKidnapped", "GmCmd_MoveIntelligentCharacter", "GmCmd_RandomizeRelationShipsInSettlement", "GmCmd_MakeCharacterHaveSex", "GmCmd_GetCharacterPregnancyLockEndDates", "GmCmd_GetCharacterActualBloodParents",
			"CharacterShaveAvatar", "AllocateNeili", "DeallocateNeili", "GetChangeOfQiDisorder", "GetUsableCombatResources", "GetCombatSkillSlotCounts", "SetCombatSkillSlot", "GetCombatSkillAttainment", "GetAllCombatSkillAttainment", "GetLifeSkillAttainment",
			"GetAllLifeSkillAttainment", "LearnCombatSkill", "LearnLifeSkill", "TryGetDeadCharacter", "GetTitles", "GetHighestGradeCombatSkillById", "GetLeftMaxHealth", "GetHealthRecovery", "GetAvatarRelatedDataList", "GetAvatarData",
			"TransferInventoryItemListWithDebt", "GetFameType", "GetAvatarRelatedData", "GetCharacterListWisdomCount", "SortCharacterListByMaxCombatSkill", "GetCharacterMaxCombatSkillAttainment", "GetDebts", "GetItemPowerInfo", "CalcMaxFavorabilityToTaiwuById", "CheckDebtChange",
			"GetCharacterWisdomCountById", "TransferInventoryItemFromAToB", "GmCmd_SetCurReadingEvent", "GmCmd_SetAdventurePersonalities", "GmCmd_AddFeature", "GmCmd_SetFeatures", "GmCmd_RemoveFeature", "GmCmd_RemoveRelation", "IsReclusive", "GetInventoryEquipment",
			"GetFilteredCharacterCounts", "ClearCharacterSortFilter", "UpdateSortFilterSettings", "InitializeCharacterSortFilter", "FindNameInCurrentSortFilter", "GetCharacterDisplayDataListForUltimateSelect", "GetMaxSortingTypeCharIds", "GmCmd_SetCurrNeili", "GmCmd_AddPoisonedInventoryItem", "GmCmd_AddPoisonedEatingItem",
			"GmCmd_SetCharBaseNeiliProportionOfFiveElements", "GetRelationBetweenCharacters", "GetInventoryItemsByItemType", "GetCharacterDisplayData", "UnequipAllCombatSkills", "AutoEquipCombatSkills", "GmCmd_AddCharacterExtraTitle", "TryAddAndApplyOneWayRelation", "MoveFuyuHiltLocation", "TryRemoveOneWayRelation",
			"GetCharacterLoveAndHateItemInfo", "IsTemporaryIntelligentCharacter", "GetMixedPoisonTypeRelatedMarkCountArray", "SimulateEatingEffect", "GetCharacterDisplayDataForTooltip", "GmCmd_SetCurLoopingEvent", "GetHealthType", "CalcFavorabilityDelta", "GetCharacterLocationDisplayData", "GetCharacterDisplayDataForMapBlock",
			"GetCharacterWisdomList", "GetOrCreateSwordTombCharacterIdForNormalInformation", "GetAllInventoryItemsExcludeValueZero", "GetAddConsummateLevelRequiredMonth", "GmCmd_ResetHairGrowth", "GetAllItemsByAreaId", "SetCombatSkillPlanLock", "AppendCombatSkillPlan", "DuplicateCurrentCombatSkillPlan", "UpdateCombatSkillPlan",
			"GetCurrentPlanIdAndPlanCount", "DeleteCombatSkillPlan", "IsInteractedWithTaiwu", "IsNeiliAllocationLocked", "AllocateGenericGrid", "DeallocateGenericGrid", "IsCombatSkillEquipmentLocked", "AutoAllocateNeili", "AutoSetCombatSkillAttainmentPanels", "AutoEquipItems",
			"SetCombatSkillAttainmentLock", "IsCombatSkillAttainmentLocked", "GetGenericGridAllocation", "SetNeiliAllocationLock", "RemoveEquippedCombatSkill", "AddEquippedCombatSkill", "GetCombatSkillExtraSlotCounts", "GetCharacterAllBodyPartExists", "GmCmd_ChangeCharDisorderOfQi", "GenerateRandomName",
			"GetCharacterDisplayDataListForRelationsWithRelationType", "GetPhysiologicalAge", "GetPhysiologicalAgeAffector", "GetKidnappedCharacterDisplayData", "CalcItemsFavorabilityDelta", "IsCarrierDurabilityRunningOut", "GmCmd_ForceChangeOrganizationByName"
		};
	}
}
