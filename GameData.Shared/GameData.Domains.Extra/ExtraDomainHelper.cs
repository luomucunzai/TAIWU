using System.Collections.Generic;

namespace GameData.Domains.Extra;

public static class ExtraDomainHelper
{
	public static class DataIds
	{
		public const ushort CharacterSecretInformationUsedCounts = 0;

		public const ushort SecretInformationOccurredLocations = 1;

		public const ushort SecretInformationBroadcastList = 2;

		public const ushort ExchangedSpecialWeaponList = 3;

		public const ushort CaravanStayDays = 4;

		public const ushort MerchantCharToType = 5;

		public const ushort MerchantCharToExtraGoods = 6;

		public const ushort StationInited = 7;

		public const ushort ManualChangeEquipGroupCharIds = 8;

		public const ushort StoneRoomCharList = 9;

		public const ushort HideSkeletonEquipSlots = 10;

		public const ushort CombatSkillOrderPlans = 11;

		public const ushort LocationMarkHashSet = 12;

		public const ushort TaiwuExtraBonuses = 13;

		public const ushort AutoWorkBlockIndexList = 14;

		public const ushort AutoSoldBlockIndexList = 15;

		public const ushort SecretInformationBroadcastNotifyList = 16;

		public const ushort TreasuryItems = 17;

		public const ushort TroughItems = 18;

		public const ushort XiangshuIdInKungfuPracticeRoom = 19;

		public const ushort RemovedSpecialRelations = 20;

		public const ushort ShopArrangeResultFirstList = 21;

		public const ushort AutoExpandBlockIndexList = 22;

		public const ushort WorldCharacterExtraTitles = 23;

		public const ushort ReadingEventBookIdList = 24;

		public const ushort ClearedSkillPlateStepInfo = 25;

		public const ushort TravelingEventCollection = 26;

		public const ushort DlcArgBox = 27;

		public const ushort PrevTriggeredTravelingEvents = 28;

		public const ushort GainsInTravel = 29;

		public const ushort MaxApprovingRateTemporaryBonus = 30;

		public const ushort LifeSkillCombatCardDict = 31;

		public const ushort LifeSkillCombatUsedCardDict = 32;

		public const ushort LifeSkillCombatReadBookPageDict = 33;

		public const ushort LifeSkillCombatNewCardDict = 34;

		public const ushort CharacterAiActionRestrictions = 35;

		public const ushort ExtraExternalRelationStates = 36;

		public const ushort SecretInformationBroadcastNotifyExtraList = 37;

		public const ushort CharTeammateCommandDict = 38;

		public const ushort NicknameDict = 39;

		public const ushort LegendaryBookBreakPlateCounts = 40;

		public const ushort ContestForLegendaryBookChars = 41;

		public const ushort LegendaryBookWeaponSlot = 42;

		public const ushort LegendaryBookWeaponEffectId = 43;

		public const ushort LegendaryBookSkillSlot = 44;

		public const ushort LegendaryBookSkillEffectId = 45;

		public const ushort LegendaryBookBonusCountYin = 46;

		public const ushort LegendaryBookBonusCountYang = 47;

		public const ushort PrevLegendaryBookOwnerCopies = 48;

		public const ushort LegendaryBookShockedMonths = 49;

		public const ushort CombatSkillBreakPlateLastClearTimeList = 50;

		public const ushort CombatSkillCurrBreakPlateIndex = 51;

		public const ushort CombatSkillBreakPlateObsoleteList = 52;

		public const ushort LegendaryBookConsumedCharIds = 53;

		public const ushort CharacterAiActionCooldowns = 54;

		public const ushort CharacterTemporaryFeatures = 55;

		public const ushort ExtraTriggeredTasks = 56;

		public const ushort TaskSortingOrder = 57;

		public const ushort LegendaryBookHiddenCharIds = 58;

		public const ushort CharacterAiActionSuccessRateAdjusts = 59;

		public const ushort LoveDataDict = 60;

		public const ushort PreviousLoverSet = 61;

		public const ushort ConfessLoveFailedSet = 62;

		public const ushort LoveTokenDataDict = 63;

		public const ushort ObsoleteTaiwuProfessions = 64;

		public const ushort TaiwuCurrProfessionId = 65;

		public const ushort SavedSouls = 66;

		public const ushort CricketIsSmart = 67;

		public const ushort CricketIsIdentified = 68;

		public const ushort TaiwuInteractionCooldowns = 69;

		public const ushort CharacterCustomDisplayNames = 70;

		public const ushort AnimalAreaData = 71;

		public const ushort HandledOneShotEvents = 72;

		public const ushort HunterAnimals = 73;

		public const ushort WorldVersionInfo = 74;

		public const ushort SectMainStoryEventArgBoxes = 75;

		public const ushort ChangeTrickBodyPart = 76;

		public const ushort ChangeTrickIndex = 77;

		public const ushort VoiceWeaponInnerRatio = 78;

		public const ushort SectXuehouBloodLightLocations = 79;

		public const ushort CharacterAvatarSnapshot = 80;

		public const ushort BrokenAreaMaterials = 81;

		public const ushort SectWudangFairylandData = 82;

		public const ushort TreasureMaterialFailedTimes = 83;

		public const ushort SectYuanshanDemonEffectIds = 84;

		public const ushort TaiwuMaxNeiliAllocation = 85;

		public const ushort CharacterMasteredCombatSkills = 86;

		public const ushort MasteredCombatSkillPlans = 87;

		public const ushort BreakoutDifficulty = 88;

		public const ushort ReadingDifficulty = 89;

		public const ushort LoopingDifficulty = 90;

		public const ushort CombatSkillBreakPlateLastForceBreakoutStepsCount = 91;

		public const ushort PregeneratedFixedEnemies = 92;

		public const ushort AutoCheckInComfortableList = 93;

		public const ushort AutoCheckInResidenceList = 94;

		public const ushort ReadingEventReferenceBooks = 95;

		public const ushort MerchantTypeToDebts = 96;

		public const ushort MerchantBuyBackDict = 97;

		public const ushort MerchantMaxLevelBuyBackData = 98;

		public const ushort CaravanBuyBackDict = 99;

		public const ushort FirstLegendaryBookDelay = 100;

		public const ushort LegaciesBuildingTemplateIdList = 101;

		public const ushort IsDreamBack = 102;

		public const ushort AbridgedDreamBackCharacters = 103;

		public const ushort UnlockedWorkingVillagers = 104;

		public const ushort DreamBackLifeRecords = 105;

		public const ushort ActionPointCurrMonth = 106;

		public const ushort ActionPointPrevMonth = 107;

		public const ushort AdvancedTeammateCommandDict = 108;

		public const ushort LastMonthNotificationsIndex = 109;

		public const ushort PreviousMonthlyNotifications = 110;

		public const ushort ActionPointPreserved = 111;

		public const ushort VillageWorkQualificationImprove = 112;

		public const ushort ReadInLifeSkillCombatCount = 113;

		public const ushort SectWudangHeavenlyTree = 114;

		public const ushort SectWudangHeavenlyTreeList = 115;

		public const ushort SectWudangLingBaoDark = 116;

		public const ushort SectWudangLingBaoLight = 117;

		public const ushort ShixiangBarbarianMasterIdList = 118;

		public const ushort LegaciesBuildingList = 119;

		public const ushort GroupCharacterEquipmentRecord = 120;

		public const ushort SectEmeiSkillBreakBonus = 121;

		public const ushort InformationSettings = 122;

		public const ushort SectXuannvUnlockedMusicList = 123;

		public const ushort SectXuannvPlayerPlayMode = 124;

		public const ushort SectXuannvPlayerMusicId = 125;

		public const ushort SectXuannvPlayerIsEnabled = 126;

		public const ushort SectEmeiBreakBonusTemplateIds = 127;

		public const ushort MirrorCharacters = 128;

		public const ushort SectEmeiBloodLocations = 129;

		public const ushort SectEmeiExp = 130;

		public const ushort EnemyPracticeLevel = 131;

		public const ushort CharacterPoisonImmunities = 132;

		public const ushort TaiwuInformationReceivedHistories = 133;

		public const ushort SectXuannvEvaluatedMusicList = 134;

		public const ushort CombatSkillJumpThreshold = 135;

		public const ushort DreamBackUnlockStates = 136;

		public const ushort DreamBackArchiveBackup = 137;

		public const ushort OverwrittenTaiwu = 138;

		public const ushort DreamBackLocationData = 139;

		public const ushort DreamBackGenealogyObsoleted = 140;

		public const ushort ConflictCombatSkills = 141;

		public const ushort CharacterRevealedHobbies = 142;

		public const ushort FinalDateBeforeDreamBack = 143;

		public const ushort DreamBackTaiwu = 144;

		public const ushort ConflictEffectWrappers = 145;

		public const ushort DreamBackGlobalEventArgBox = 146;

		public const ushort DreamBackDlcEventArgBox = 147;

		public const ushort DreamBackSectMainStoryEventArgBox = 148;

		public const ushort DejaVuEventCharacters = 149;

		public const ushort TaiwuPropertyPermanentBonuses = 150;

		public const ushort SelectedUniqueLegacies = 151;

		public const ushort TaiwuAddOneWayRelationCoolDown = 152;

		public const ushort CarrierTamePoint = 153;

		public const ushort DreamBackPreviousTaiwuCharIds = 154;

		public const ushort FleeCarrierLocation = 155;

		public const ushort FavorabilityChange = 156;

		public const ushort CanResetWorldSettings = 157;

		public const ushort SettlementExtraData = 158;

		public const ushort DlcArgBoxes = 159;

		public const ushort JiaoPools = 160;

		public const ushort Jiaos = 161;

		public const ushort ChildrenOfLoong = 162;

		public const ushort FiveLoongDict = 163;

		public const ushort JiaoPoolRecords = 164;

		public const ushort DlcEntries = 165;

		public const ushort JiaoPoolStatus = 166;

		public const ushort ChoosyRemainUpgradeRateDict = 167;

		public const ushort ChoosyRemainUpgradeCountDict = 168;

		public const ushort BuildingMoneyPrestigeSuccessRateCompensation = 169;

		public const ushort OwnedClothingSet = 170;

		public const ushort ClothingDisplayModifications = 171;

		public const ushort DisableEnemyAi = 172;

		public const ushort EnemyUnyieldingFallen = 173;

		public const ushort LastTargetDistance = 174;

		public const ushort WeaveClothingDisplaySetting = 175;

		public const ushort EmptyToolKey = 176;

		public const ushort SectWuxianWugJugPoisons = 177;

		public const ushort LocationSupportingBlockResourceTotal = 178;

		public const ushort CharacterPrioritizedActionCooldowns = 179;

		public const ushort CricketCollectionDataList = 180;

		public const ushort RecruitCharacterDataLists = 181;

		public const ushort SectJingangPossessionCharacters = 182;

		public const ushort MixedPoisonEffectTriggerDates = 183;

		public const ushort CricketExtraAge = 184;

		public const ushort EnemyNestInitializationDates = 185;

		public const ushort SecretInformationOccurrence = 186;

		public const ushort SecretInformationHandleMap = 187;

		public const ushort SecretInformationCharacterOwn = 188;

		public const ushort SecretInformationSecretInformationCharacterExtraInfoCollectionMap = 189;

		public const ushort SecretInformationCharacterRelationshipSnapshotCollectionMap = 190;

		public const ushort SecretInformationShopCharacterData = 191;

		public const ushort ChangeTrickIsFlaw = 192;

		public const ushort NextAnimalId = 193;

		public const ushort Animals = 194;

		public const ushort SettlementTreasuries = 195;

		public const ushort PrevMartialArtTournamentWinners = 196;

		public const ushort BookStrategiesExpireTime = 197;

		public const ushort UnlockedCombatSkillPlanCount = 198;

		public const ushort ActiveLoopingProgress = 199;

		public const ushort ActiveReadingProgress = 200;

		public const ushort CharacterExtraNeiliAllocationProgress = 201;

		public const ushort QiArtStrategyList = 202;

		public const ushort QiArtStrategyExpireTimeList = 203;

		public const ushort LegacyPointTimesDict = 204;

		public const ushort SectRanshanThreeCorpses = 205;

		public const ushort SectBaihuaLifeLinkData = 206;

		public const ushort SamsaraPlatformRecordCollection = 207;

		public const ushort AvailableReadingStrategyMap = 208;

		public const ushort ReferenceSkillList = 209;

		public const ushort AvailableQiArtStrategyMap = 210;

		public const ushort LoopingEventSkillIdList = 211;

		public const ushort QiArtStrategyMap = 212;

		public const ushort QiArtStrategyExpireTimeMap = 213;

		public const ushort LoopInLifeSkillCombatCount = 214;

		public const ushort LoopInCombatCount = 215;

		public const ushort SettlementTreasuryRecordCollections = 216;

		public const ushort MonthlyNotificationSortingGroups = 217;

		public const ushort FollowingNpcList = 218;

		public const ushort SectEmeiBreakBonusData = 219;

		public const ushort FollowingNickNameMap = 220;

		public const ushort MerchantOverFavorDataArray = 221;

		public const ushort ChangedTeammateCharIds = 222;

		public const ushort SettlementPrisons = 223;

		public const ushort SectFulongOrgMemberChickens = 224;

		public const ushort SectFulongInFlameAreas = 225;

		public const ushort SectFulongOutLaws = 226;

		public const ushort SettlementPrisonRecordCollections = 227;

		public const ushort VillagerRoles = 228;

		public const ushort SectFulongLoseFeatherChickens = 229;

		public const ushort StockStorage = 230;

		public const ushort CraftStorage = 231;

		public const ushort MedicineStorage = 232;

		public const ushort FullPoisonEffects = 233;

		public const ushort FoodStorage = 234;

		public const ushort TaiwuVillageStoragesRecordCollection = 235;

		public const ushort ItemPriceFluctuation = 236;

		public const ushort VillagerRoleNickNameMap = 237;

		public const ushort VillagerRoleMaxUnlockCounts = 238;

		public const ushort BuildingResourceOutputSettings = 239;

		public const ushort BlockRecoveryUnlockDates = 240;

		public const ushort TaiwuVillageStorageSettingDict = 241;

		public const ushort CharacterConsummateLevelProgresses = 242;

		public const ushort TaiwuWantedFirstInteractOrganizationMember = 243;

		public const ushort AreaSpiritualDebt = 244;

		public const ushort TaiwuProfessions = 245;

		public const ushort TaiwuProfessionSkillSlots = 246;

		public const ushort CharacterSpecialGroup = 247;

		public const ushort CricketPlaceExtraData = 248;

		public const ushort BranchMerchantData = 249;

		public const ushort CharacterCombatSkillConfigurations = 250;

		public const ushort CharacterEquippedCombatSkills = 251;

		public const ushort InteractedCharacterList = 252;

		public const ushort FriendOrFamilyInteractionCooldownDict = 253;

		public const ushort SectZhujianGearMates = 254;

		public const ushort ProfessionUpgrade = 255;

		public const ushort FollowMovementCharacters = 256;

		public const ushort TriggeredAddSeniorityPoints = 257;

		public const ushort TaiwuGiftItems = 258;

		public const ushort SectZhujianThiefList = 259;

		public const ushort CharacterProfessions = 260;

		public const ushort CaravanExtraDataDict = 261;

		public const ushort ProtectCaravanTime = 262;

		public const ushort SectZhujianAreaMerchantTypeDict = 263;

		public const ushort MerchantExtraGoods = 264;

		public const ushort ExorcismEnabled = 265;

		public const ushort SectStorySpecialMerchant = 266;

		public const ushort IsExtraProfessionSkillUnlocked = 267;

		public const ushort IsDirectTraveling = 268;

		public const ushort KidnappedTravelData = 269;

		public const ushort VillagerLastInfluencePowerGrade = 270;

		public const ushort VillagerTreasuryNeeds = 271;

		public const ushort CharacterAvatarExtraDataDict = 272;

		public const ushort LootYield = 273;

		public const ushort CityPunishmentSeverityCustomizeDict = 274;

		public const ushort CharacterSkillBreakBonuses = 275;

		public const ushort CombatSkillProficiencies = 276;

		public const ushort SkillBreakPlates = 277;

		public const ushort CombatSkillBreakPlateList = 278;

		public const ushort SectShaolinDemonSlayerData = 279;

		public const ushort KongsangCharacterFeaturePoisonedProb = 280;

		public const ushort PickupDict = 281;

		public const ushort TaiwuSelectedDebateCardType = 282;

		public const ushort VillagerRoleRecords = 283;

		public const ushort BuildingBlockDataEx = 284;

		public const ushort VillagerRoleAutoActionStates = 285;

		public const ushort BuildingArrangementSettingPresetData = 286;

		public const ushort BuildingArtisanOrders = 287;

		public const ushort TaiwuVillagerPotentialData = 288;

		public const ushort NpcArtisanOrders = 289;

		public const ushort ShopVillagerQualificationImprove = 290;

		public const ushort BigEvents = 291;

		public const ushort HasGetShuiHuoYingQiGongSkillBookByArchiveFix = 292;

		public const ushort FarmerAutoCollectStorageType = 293;

		public const ushort WoodenXiangshuAvatarSelectedFeatures = 294;

		public const ushort BuildingAreaEffectProgresses = 295;

		public const ushort PreviousLegendaryBookOwners = 296;

		public const ushort VillagerSkillLegacyPointDict = 297;

		public const ushort KilledByYufuCharactersBinary = 298;

		public const ushort DyingCharacters = 299;

		public const ushort ProficiencyEnoughSkills = 300;

		public const ushort SectYuanshanThreeVitals = 301;

		public const ushort DreamBackGenealogy = 302;

		public const ushort ResourceBlockExtraData = 303;

		public const ushort UnlockedFeastTypes = 304;

		public const ushort Feasts = 305;

		public const ushort SettlementMemberFeatures = 306;

		public const ushort SettlementLayeredTreasuries = 307;

		public const ushort BuildingDefaultStoreLocation = 308;

		public const ushort TaiwuVillageVowOrgTemplateCrossArchiveDict = 309;

		public const ushort CricketCombatPlans = 310;

		public const ushort LastCricketPlanIndex = 311;

		public const ushort CharacterDarkAshCounterData = 312;

		public const ushort FuyuFaith = 313;
	}

	public static class MethodIds
	{
		public const ushort AddManualChangeEquipGroupChar = 0;

		public const ushort RemoveManualChangeEquipGroupChar = 1;

		public const ushort AddHideSkeletonEquipSlot = 2;

		public const ushort RemoveHideSkeletonEquipSlot = 3;

		public const ushort SetCombatSkillOrderPlan = 4;

		public const ushort AddLocationMark = 5;

		public const ushort RemoveLocationMark = 6;

		public const ushort GetAllLocationMark = 7;

		public const ushort TroughRemove = 8;

		public const ushort TreasuryAdd = 9;

		public const ushort TreasuryAddList = 10;

		public const ushort TreasuryRemove = 11;

		public const ushort TreasuryRemoveList = 12;

		public const ushort TroughAdd = 13;

		public const ushort TroughAddList = 14;

		public const ushort TroughRemoveList = 15;

		public const ushort AddReadingEventBookId = 16;

		public const ushort RemoveReadingEventBookId = 17;

		public const ushort GetAllLifeSkillCombatUsedCard = 18;

		public const ushort GetAllLifeSkillCombatCard = 19;

		public const ushort SetLifeSkillCombatUsedCard = 20;

		public const ushort GetCharacterLifeSkillCombatUsedCard = 21;

		public const ushort GetLifeSkillCombatUsedCard = 22;

		public const ushort GetAllLifeSkillCombatNewCard = 23;

		public const ushort SetLifeSkillCombatCardNotNew = 24;

		public const ushort GetCharTeammateCommands = 25;

		public const ushort SetLegendaryBookWeaponSlot = 26;

		public const ushort SetLegendaryBookSkillSlot = 27;

		public const ushort UnlockLegendaryBookBreakPlate = 28;

		public const ushort UnlockLegendaryBookBonus = 29;

		public const ushort ChangeCombatSkillBreakPlate = 30;

		public const ushort EnterUnlockBreakPlateCombat = 31;

		public const ushort SetTopTask = 32;

		public const ushort GmCmd_SetLoveData = 33;

		public const ushort GmCmd_RemoveLoveData = 34;

		public const ushort GmCmd_RemoveLoveEvent = 35;

		public const ushort ChangeProfession = 36;

		public const ushort ExecuteActiveProfessionSkill = 37;

		public const ushort IsProfessionalSkillUnlocked = 38;

		public const ushort CanExecuteProfessionSkill = 39;

		public const ushort SetProfessionTestSetting = 40;

		public const ushort GetCharacterCustomDisplayName = 41;

		public const ushort GetTianJieFuLuCount = 42;

		public const ushort GmCmd_GenerateTreasure = 43;

		public const ushort FindTreasure = 44;

		public const ushort CheckSpecialCondition = 45;

		public const ushort ConfirmExecuteSkill = 46;

		public const ushort FindTreasureExpect = 47;

		public const ushort GetLocationHasHunterAnimal = 48;

		public const ushort SetProfessionSeniorityCurrent = 49;

		public const ushort UnlockAllProfessionSkills = 50;

		public const ushort SetProfessionSeniorityTarget = 51;

		public const ushort GmCmd_Profession_SetBuddhistMonkSavedSoulCount = 52;

		public const ushort GmCmd_Profession_SetTempleVisited = 53;

		public const ushort InitAiLifeSkillCombatUsedCard = 54;

		public const ushort InvokeSeniorityCachedEvent = 55;

		public const ushort GmCmd_Profession_RecoverHunterCarrierAttackCount = 56;

		public const ushort GetBlockMerchantTypes = 57;

		public const ushort GmCmd_RemoveTriggeredExtraTask = 58;

		public const ushort GmCmd_AddExtraTask = 59;

		public const ushort GetCharacterMasteredCombatSkills = 60;

		public const ushort AddCharacterMasteredCombatSkill = 61;

		public const ushort RemoveCharacterMasteredCombatSkill = 62;

		public const ushort InvokeFindExtraTreasureEvent = 63;

		public const ushort SetAdvancedTeammateCommands = 64;

		public const ushort CancelAdvancedTeammateCommands = 65;

		public const ushort GetAllPreviousMonthlyNotifications = 66;

		public const ushort GetPreviousMonthlyNotifications = 67;

		public const ushort PreserveActionPoint = 68;

		public const ushort GetPreserveDay = 69;

		public const ushort GetAllHeavenlyTrees = 70;

		public const ushort GetHeavenlyTreeNearBlocks = 71;

		public const ushort GetInformationSettings = 72;

		public const ushort RemoveEmeiSkillBreakBonus = 73;

		public const ushort AddEmeiSkillBreakBonus = 74;

		public const ushort GetEmeiBreakBonusDisplayData = 75;

		public const ushort GetEmeiBreakBonusCollection = 76;

		public const ushort GetPoisonImmunities = 77;

		public const ushort GetDreamBackTaiwuRelatedCharactersForRelations = 78;

		public const ushort GetDreamBackTaiwuGenealogy = 79;

		public const ushort GetCharacterDisplayDataListForDreamBackRelations = 80;

		public const ushort GetDreamBackLifeRecordByDate = 81;

		public const ushort GetNameAndLifeRelatedDataListForDreamBack = 82;

		public const ushort IsCharacterHatingItemRevealed = 83;

		public const ushort IsCharacterLovingItemRevealed = 84;

		public const ushort IsCharacterHobbyRevealed = 85;

		public const ushort SetCharacterRevealedHobbies = 86;

		public const ushort GetConflictCombatSkill = 87;

		public const ushort GetAllDreamBackLifeRecords = 88;

		public const ushort GetDreamBackTaiwuBirthAndEndDates = 89;

		public const ushort IsCurrentTaiwuOverwrittenByDreamBack = 90;

		public const ushort ApplyConflictCombatSkillResult = 91;

		public const ushort HaveConflictCombatSkill = 92;

		public const ushort AddTaiwuOneWayRelationCoolDown = 93;

		public const ushort IsTaiwuAbleToAddOneWayRelation = 94;

		public const ushort GetTaiwuAddOneWayRelationCoolDown = 95;

		public const ushort FeedCarrier = 96;

		public const ushort GetCarrierTamePoint = 97;

		public const ushort GetAllCharacterPropertyBonusData = 98;

		public const ushort GetDreamBackCharacterDisplayDataList = 99;

		public const ushort GetCarrierMaxTamePoint = 100;

		public const ushort GetCurrMaxJiaoPoolCount = 101;

		public const ushort GmCmd_FindFiveLoongLocation = 102;

		public const ushort GetJiaoPoolBlockStyle = 103;

		public const ushort SetJiaoPoolBlockStyle = 104;

		public const ushort GetChildrenOfLoongById = 105;

		public const ushort GetJiaoPoolList = 106;

		public const ushort GetJiaoById = 107;

		public const ushort GetJiaoPoolAllJiaoData = 108;

		public const ushort GetJiaoPoolBreedingProcess = 109;

		public const ushort PutJiaoInPool = 110;

		public const ushort PutAnotherJiaoInPool = 111;

		public const ushort PutJiaoOutOfPool = 112;

		public const ushort ChangeNurturance = 113;

		public const ushort ChangeJiaoName = 114;

		public const ushort DisableJiaoPool = 115;

		public const ushort EnableJiaoPool = 116;

		public const ushort GetJiaoLoongNameRelatedDataList = 117;

		public const ushort GetAllJiaoForPool = 118;

		public const ushort GetAllJiaoForEvolve = 119;

		public const ushort GetJiaoByItemKey = 120;

		public const ushort GetJiaosByItemKeys = 121;

		public const ushort GetChildrenOfLoongByItemKey = 122;

		public const ushort PutEggIntoPool = 123;

		public const ushort JiaoPoolInteract = 124;

		public const ushort GmCmd_AddJiao = 125;

		public const ushort GmCmd_PutJiaoInFirstPool = 126;

		public const ushort GmCmd_AddChildOfLoong = 127;

		public const ushort JiaoEvolveToChildOfLoong = 128;

		public const ushort GetJiaoEvolutionChoice = 129;

		public const ushort ResetJiaoPoolStatus = 130;

		public const ushort GetAllAdultJiao = 131;

		public const ushort GetAllEvolvingJiao = 132;

		public const ushort GetJiaoTemplateIdByCarrierTemplateId = 133;

		public const ushort CalcResourceChangeByJiaoPool = 134;

		public const ushort IsOwnedChildrenOfLoong = 135;

		public const ushort GetNextRandomChildrenOfLoong = 136;

		public const ushort GmCmd_AddFleeCarrier = 137;

		public const ushort GetIsJiaoPoolOpen = 138;

		public const ushort FillJiaoRecordArgumentCollection = 139;

		public const ushort GetJiaoEvolutionPageStatus = 140;

		public const ushort GetIsBabysittingMode = 141;

		public const ushort SetIsBabysittingMode = 142;

		public const ushort GetFiveLoongDictCount = 143;

		public const ushort GetJiaoLoongNameRelatedData = 144;

		public const ushort IsJiaoAbleToPet = 145;

		public const ushort PetJiao = 146;

		public const ushort JiaoPoolPetJiao = 147;

		public const ushort GetTaiwuAddOneWayRelationResultCode = 148;

		public const ushort RequestRecruitCharacterData = 149;

		public const ushort GetCharacterTemporaryFeaturesExpireDate = 150;

		public const ushort GmCmd_AddThreeCorpses = 151;

		public const ushort ApplyRanshanThreeCorpsesLegendaryBookKeepingResult = 152;

		public const ushort GetItemListForRanshanTreeCorpsesLegendaryBookKeeping = 153;

		public const ushort GmCmd_AddDisplayEventLegendaryBookKeeping = 154;

		public const ushort SetRanshanThreeCorpsesCharacterTarget = 155;

		public const ushort GetOrInitExtraNeiliAllocationProgress = 156;

		public const ushort GetBookStrategiesExpireTime = 157;

		public const ushort SetMonthlyNotificationSortingGroup = 158;

		public const ushort SetCharTeammateCommandsManual = 159;

		public const ushort GetCharAdvancedTeammateCommands = 160;

		public const ushort IsStoneRoomFull = 161;

		public const ushort ExtinguishFulongInFlameArea = 162;

		public const ushort TriggerFulongInFlameAreaMine = 163;

		public const ushort ApplyFulongInFlameAreaFullyExtinguished = 164;

		public const ushort GmCmd_GenerateFulongFlameArea = 165;

		public const ushort GetTaiwuVillageStoragesRecordCollection = 166;

		public const ushort FulongSpecialInteractOpen = 167;

		public const ushort HunterSkill_AnimalCharacterToItem = 168;

		public const ushort ConfirmProfessionSkillsEquipment = 169;

		public const ushort GmCmd_CastTasterUltimateOnCurrentBlock = 170;

		public const ushort EatTianJieFuLu = 171;

		public const ushort CheckAristocratUltimateSpecialCondition = 172;

		public const ushort CheckBeggarUltimateSpecialCondition = 173;

		public const ushort CheckTasterUltimateSpecialCondition = 174;

		public const ushort GM_GetFriendOrFamilySendGift = 175;

		public const ushort GmCmd_CreateGearMate = 176;

		public const ushort GetGearMateRepairEffect = 177;

		public const ushort RepairGearMate = 178;

		public const ushort GetGearMateRepairRequirement = 179;

		public const ushort GetGearMateAvailableRepairCount = 180;

		public const ushort GetGearMateRepairRequirementDisplayDatas = 181;

		public const ushort UpgradeGearMate = 182;

		public const ushort GetCharacterConsummateLevelProgress = 183;

		public const ushort GetMartialArtistCreateGoodRandomEnemyAndBadRandomEnemyCount = 184;

		public const ushort GetGearMateById = 185;

		public const ushort GetCharacterCurrentProfession = 186;

		public const ushort CheckSpecialCondition_SavageSkill_1 = 187;

		public const ushort GetMerchantExtraGoods = 188;

		public const ushort SetProfessionExtraSeniority = 189;

		public const ushort GmCmd_SetCharacterCurrProfessionSeniority = 190;

		public const ushort CanShowProfessionSkillUnlocked = 191;

		public const ushort GetGearMateBreakoutCombatSkillBanReasonList = 192;

		public const ushort SetDukeSkill3Crickets = 193;

		public const ushort GetAllSkillBooksGearMateCanRead = 194;

		public const ushort CanIdentifyCricket = 195;

		public const ushort CanUpgradeCricket = 196;

		public const ushort CanConvertToAnimalCharacter = 197;

		public const ushort GetCharacterAvatarExtraData = 198;

		public const ushort GetJiaoLoongDisplayDataByItemKey = 199;

		public const ushort UpdateCityPunishmentSeverityCustomizeData = 200;

		public const ushort GmCmd_SetCharacterProficiencies = 201;

		public const ushort GmCmd_CreateRandomEnemyAroundHeavenlyTree = 202;

		public const ushort RemoveTreasuryItemList = 203;

		public const ushort RemoveTreasuryItem = 204;

		public const ushort AddTreasuryItemList = 205;

		public const ushort AddTreasuryItem = 206;

		public const ushort GmCmd_ShowUnlockedProfessionSkill = 207;

		public const ushort UnlockBuildingLevelSlot = 208;

		public const ushort SetVillagerRoleAutoActionState = 209;

		public const ushort UpgradeResourceBuilding = 210;

		public const ushort ChangeBuildingArrangementSettingPresetData = 211;

		public const ushort ChangeBuildingArrangementPresetId = 212;

		public const ushort AddMaterialToArtisanOrder = 213;

		public const ushort AddResourceToArtisanOrder = 214;

		public const ushort GetArtisanOrderProductionPool = 215;

		public const ushort SetArtisanOrderProductionType = 216;

		public const ushort SetArtisanOrderStorageType = 217;

		public const ushort GetNpcArtisanOrder = 218;

		public const ushort InterceptArtisanOrder = 219;

		public const ushort GetBuildingArtisanOrder = 220;

		public const ushort CreateArtisanOrder = 221;

		public const ushort GetCustomizePunishmentSeverityCost = 222;

		public const ushort GetProductionPoolPreview = 223;

		public const ushort ArtisanOrderDebate = 224;

		public const ushort UpgradeSlotBuilding = 225;

		public const ushort GetArtisanOrderMaterialPreview = 226;

		public const ushort GetArtisanOrderCanProduceItemSubType = 227;

		public const ushort WillCustomizePunishmentBreakWithoutVillagerHead = 228;

		public const ushort SetFarmerAutoCollectStorageType = 229;

		public const ushort UpdateWoodenXiangshuAvatarSelectedFeatures = 230;

		public const ushort SetBuildingArrangementSetting = 231;

		public const ushort GmCmd_GetBuildingAreaEffectProgresses = 232;

		public const ushort GmCmd_SetBuildingAreaEffectProgresses = 233;

		public const ushort GmCmd_ReleaseAllKilledByLongYufuCharacters = 234;

		public const ushort GmCmd_RecordKilledByLongYufuCharacter = 235;

		public const ushort GmCmd_VitalInfectionInOut = 236;

		public const ushort CheckSpecialCondition_HunterSkill2 = 237;

		public const ushort GetThreeVitalsCharDataList = 238;

		public const ushort GmCmd_InitThreeVitals = 239;

		public const ushort GetThreeVitalsTargetCharDataList = 240;

		public const ushort TransferInfectionBetweenVitalAndCharacter = 241;

		public const ushort SetVitalInPrison = 242;

		public const ushort GetBuildingArtisanOrderAfterUpdate = 243;

		public const ushort GetCanSelectThreeVitalsDisplayData = 244;

		public const ushort AreVitalsDemon = 245;

		public const ushort GetOppositeThreeVitalsCharDataList = 246;

		public const ushort SetVitalHasPlayedComeAnim = 247;

		public const ushort GetResourceBlockProducingCoreCooldown = 248;

		public const ushort FeastAddDish = 249;

		public const ushort FeastSetAutoRefill = 250;

		public const ushort GetFeast = 251;

		public const ushort FeastRemoveDish = 252;

		public const ushort FeastReceiveGift = 253;

		public const ushort AddResourceItemToArtisanOrder = 254;

		public const ushort IsFeastException = 255;

		public const ushort UseFeastThanksLetter = 256;

		public const ushort FeastQuickRefill = 257;

		public const ushort FeastSetTargetType = 258;

		public const ushort SetBuildingSoldItemSetting = 259;
	}

	public const ushort DataCount = 314;

	public static readonly Dictionary<string, ushort> FieldName2DataId = new Dictionary<string, ushort>
	{
		{ "CharacterSecretInformationUsedCounts", 0 },
		{ "SecretInformationOccurredLocations", 1 },
		{ "SecretInformationBroadcastList", 2 },
		{ "ExchangedSpecialWeaponList", 3 },
		{ "CaravanStayDays", 4 },
		{ "MerchantCharToType", 5 },
		{ "MerchantCharToExtraGoods", 6 },
		{ "StationInited", 7 },
		{ "ManualChangeEquipGroupCharIds", 8 },
		{ "StoneRoomCharList", 9 },
		{ "HideSkeletonEquipSlots", 10 },
		{ "CombatSkillOrderPlans", 11 },
		{ "LocationMarkHashSet", 12 },
		{ "TaiwuExtraBonuses", 13 },
		{ "AutoWorkBlockIndexList", 14 },
		{ "AutoSoldBlockIndexList", 15 },
		{ "SecretInformationBroadcastNotifyList", 16 },
		{ "TreasuryItems", 17 },
		{ "TroughItems", 18 },
		{ "XiangshuIdInKungfuPracticeRoom", 19 },
		{ "RemovedSpecialRelations", 20 },
		{ "ShopArrangeResultFirstList", 21 },
		{ "AutoExpandBlockIndexList", 22 },
		{ "WorldCharacterExtraTitles", 23 },
		{ "ReadingEventBookIdList", 24 },
		{ "ClearedSkillPlateStepInfo", 25 },
		{ "TravelingEventCollection", 26 },
		{ "DlcArgBox", 27 },
		{ "PrevTriggeredTravelingEvents", 28 },
		{ "GainsInTravel", 29 },
		{ "MaxApprovingRateTemporaryBonus", 30 },
		{ "LifeSkillCombatCardDict", 31 },
		{ "LifeSkillCombatUsedCardDict", 32 },
		{ "LifeSkillCombatReadBookPageDict", 33 },
		{ "LifeSkillCombatNewCardDict", 34 },
		{ "CharacterAiActionRestrictions", 35 },
		{ "ExtraExternalRelationStates", 36 },
		{ "SecretInformationBroadcastNotifyExtraList", 37 },
		{ "CharTeammateCommandDict", 38 },
		{ "NicknameDict", 39 },
		{ "LegendaryBookBreakPlateCounts", 40 },
		{ "ContestForLegendaryBookChars", 41 },
		{ "LegendaryBookWeaponSlot", 42 },
		{ "LegendaryBookWeaponEffectId", 43 },
		{ "LegendaryBookSkillSlot", 44 },
		{ "LegendaryBookSkillEffectId", 45 },
		{ "LegendaryBookBonusCountYin", 46 },
		{ "LegendaryBookBonusCountYang", 47 },
		{ "PrevLegendaryBookOwnerCopies", 48 },
		{ "LegendaryBookShockedMonths", 49 },
		{ "CombatSkillBreakPlateLastClearTimeList", 50 },
		{ "CombatSkillCurrBreakPlateIndex", 51 },
		{ "CombatSkillBreakPlateObsoleteList", 52 },
		{ "LegendaryBookConsumedCharIds", 53 },
		{ "CharacterAiActionCooldowns", 54 },
		{ "CharacterTemporaryFeatures", 55 },
		{ "ExtraTriggeredTasks", 56 },
		{ "TaskSortingOrder", 57 },
		{ "LegendaryBookHiddenCharIds", 58 },
		{ "CharacterAiActionSuccessRateAdjusts", 59 },
		{ "LoveDataDict", 60 },
		{ "PreviousLoverSet", 61 },
		{ "ConfessLoveFailedSet", 62 },
		{ "LoveTokenDataDict", 63 },
		{ "ObsoleteTaiwuProfessions", 64 },
		{ "TaiwuCurrProfessionId", 65 },
		{ "SavedSouls", 66 },
		{ "CricketIsSmart", 67 },
		{ "CricketIsIdentified", 68 },
		{ "TaiwuInteractionCooldowns", 69 },
		{ "CharacterCustomDisplayNames", 70 },
		{ "AnimalAreaData", 71 },
		{ "HandledOneShotEvents", 72 },
		{ "HunterAnimals", 73 },
		{ "WorldVersionInfo", 74 },
		{ "SectMainStoryEventArgBoxes", 75 },
		{ "ChangeTrickBodyPart", 76 },
		{ "ChangeTrickIndex", 77 },
		{ "VoiceWeaponInnerRatio", 78 },
		{ "SectXuehouBloodLightLocations", 79 },
		{ "CharacterAvatarSnapshot", 80 },
		{ "BrokenAreaMaterials", 81 },
		{ "SectWudangFairylandData", 82 },
		{ "TreasureMaterialFailedTimes", 83 },
		{ "SectYuanshanDemonEffectIds", 84 },
		{ "TaiwuMaxNeiliAllocation", 85 },
		{ "CharacterMasteredCombatSkills", 86 },
		{ "MasteredCombatSkillPlans", 87 },
		{ "BreakoutDifficulty", 88 },
		{ "ReadingDifficulty", 89 },
		{ "LoopingDifficulty", 90 },
		{ "CombatSkillBreakPlateLastForceBreakoutStepsCount", 91 },
		{ "PregeneratedFixedEnemies", 92 },
		{ "AutoCheckInComfortableList", 93 },
		{ "AutoCheckInResidenceList", 94 },
		{ "ReadingEventReferenceBooks", 95 },
		{ "MerchantTypeToDebts", 96 },
		{ "MerchantBuyBackDict", 97 },
		{ "MerchantMaxLevelBuyBackData", 98 },
		{ "CaravanBuyBackDict", 99 },
		{ "FirstLegendaryBookDelay", 100 },
		{ "LegaciesBuildingTemplateIdList", 101 },
		{ "IsDreamBack", 102 },
		{ "AbridgedDreamBackCharacters", 103 },
		{ "UnlockedWorkingVillagers", 104 },
		{ "DreamBackLifeRecords", 105 },
		{ "ActionPointCurrMonth", 106 },
		{ "ActionPointPrevMonth", 107 },
		{ "AdvancedTeammateCommandDict", 108 },
		{ "LastMonthNotificationsIndex", 109 },
		{ "PreviousMonthlyNotifications", 110 },
		{ "ActionPointPreserved", 111 },
		{ "VillageWorkQualificationImprove", 112 },
		{ "ReadInLifeSkillCombatCount", 113 },
		{ "SectWudangHeavenlyTree", 114 },
		{ "SectWudangHeavenlyTreeList", 115 },
		{ "SectWudangLingBaoDark", 116 },
		{ "SectWudangLingBaoLight", 117 },
		{ "ShixiangBarbarianMasterIdList", 118 },
		{ "LegaciesBuildingList", 119 },
		{ "GroupCharacterEquipmentRecord", 120 },
		{ "SectEmeiSkillBreakBonus", 121 },
		{ "InformationSettings", 122 },
		{ "SectXuannvUnlockedMusicList", 123 },
		{ "SectXuannvPlayerPlayMode", 124 },
		{ "SectXuannvPlayerMusicId", 125 },
		{ "SectXuannvPlayerIsEnabled", 126 },
		{ "SectEmeiBreakBonusTemplateIds", 127 },
		{ "MirrorCharacters", 128 },
		{ "SectEmeiBloodLocations", 129 },
		{ "SectEmeiExp", 130 },
		{ "EnemyPracticeLevel", 131 },
		{ "CharacterPoisonImmunities", 132 },
		{ "TaiwuInformationReceivedHistories", 133 },
		{ "SectXuannvEvaluatedMusicList", 134 },
		{ "CombatSkillJumpThreshold", 135 },
		{ "DreamBackUnlockStates", 136 },
		{ "DreamBackArchiveBackup", 137 },
		{ "OverwrittenTaiwu", 138 },
		{ "DreamBackLocationData", 139 },
		{ "DreamBackGenealogyObsoleted", 140 },
		{ "ConflictCombatSkills", 141 },
		{ "CharacterRevealedHobbies", 142 },
		{ "FinalDateBeforeDreamBack", 143 },
		{ "DreamBackTaiwu", 144 },
		{ "ConflictEffectWrappers", 145 },
		{ "DreamBackGlobalEventArgBox", 146 },
		{ "DreamBackDlcEventArgBox", 147 },
		{ "DreamBackSectMainStoryEventArgBox", 148 },
		{ "DejaVuEventCharacters", 149 },
		{ "TaiwuPropertyPermanentBonuses", 150 },
		{ "SelectedUniqueLegacies", 151 },
		{ "TaiwuAddOneWayRelationCoolDown", 152 },
		{ "CarrierTamePoint", 153 },
		{ "DreamBackPreviousTaiwuCharIds", 154 },
		{ "FleeCarrierLocation", 155 },
		{ "FavorabilityChange", 156 },
		{ "CanResetWorldSettings", 157 },
		{ "SettlementExtraData", 158 },
		{ "DlcArgBoxes", 159 },
		{ "JiaoPools", 160 },
		{ "Jiaos", 161 },
		{ "ChildrenOfLoong", 162 },
		{ "FiveLoongDict", 163 },
		{ "JiaoPoolRecords", 164 },
		{ "DlcEntries", 165 },
		{ "JiaoPoolStatus", 166 },
		{ "ChoosyRemainUpgradeRateDict", 167 },
		{ "ChoosyRemainUpgradeCountDict", 168 },
		{ "BuildingMoneyPrestigeSuccessRateCompensation", 169 },
		{ "OwnedClothingSet", 170 },
		{ "ClothingDisplayModifications", 171 },
		{ "DisableEnemyAi", 172 },
		{ "EnemyUnyieldingFallen", 173 },
		{ "LastTargetDistance", 174 },
		{ "WeaveClothingDisplaySetting", 175 },
		{ "EmptyToolKey", 176 },
		{ "SectWuxianWugJugPoisons", 177 },
		{ "LocationSupportingBlockResourceTotal", 178 },
		{ "CharacterPrioritizedActionCooldowns", 179 },
		{ "CricketCollectionDataList", 180 },
		{ "RecruitCharacterDataLists", 181 },
		{ "SectJingangPossessionCharacters", 182 },
		{ "MixedPoisonEffectTriggerDates", 183 },
		{ "CricketExtraAge", 184 },
		{ "EnemyNestInitializationDates", 185 },
		{ "SecretInformationOccurrence", 186 },
		{ "SecretInformationHandleMap", 187 },
		{ "SecretInformationCharacterOwn", 188 },
		{ "SecretInformationSecretInformationCharacterExtraInfoCollectionMap", 189 },
		{ "SecretInformationCharacterRelationshipSnapshotCollectionMap", 190 },
		{ "SecretInformationShopCharacterData", 191 },
		{ "ChangeTrickIsFlaw", 192 },
		{ "NextAnimalId", 193 },
		{ "Animals", 194 },
		{ "SettlementTreasuries", 195 },
		{ "PrevMartialArtTournamentWinners", 196 },
		{ "BookStrategiesExpireTime", 197 },
		{ "UnlockedCombatSkillPlanCount", 198 },
		{ "ActiveLoopingProgress", 199 },
		{ "ActiveReadingProgress", 200 },
		{ "CharacterExtraNeiliAllocationProgress", 201 },
		{ "QiArtStrategyList", 202 },
		{ "QiArtStrategyExpireTimeList", 203 },
		{ "LegacyPointTimesDict", 204 },
		{ "SectRanshanThreeCorpses", 205 },
		{ "SectBaihuaLifeLinkData", 206 },
		{ "SamsaraPlatformRecordCollection", 207 },
		{ "AvailableReadingStrategyMap", 208 },
		{ "ReferenceSkillList", 209 },
		{ "AvailableQiArtStrategyMap", 210 },
		{ "LoopingEventSkillIdList", 211 },
		{ "QiArtStrategyMap", 212 },
		{ "QiArtStrategyExpireTimeMap", 213 },
		{ "LoopInLifeSkillCombatCount", 214 },
		{ "LoopInCombatCount", 215 },
		{ "SettlementTreasuryRecordCollections", 216 },
		{ "MonthlyNotificationSortingGroups", 217 },
		{ "FollowingNpcList", 218 },
		{ "SectEmeiBreakBonusData", 219 },
		{ "FollowingNickNameMap", 220 },
		{ "MerchantOverFavorDataArray", 221 },
		{ "ChangedTeammateCharIds", 222 },
		{ "SettlementPrisons", 223 },
		{ "SectFulongOrgMemberChickens", 224 },
		{ "SectFulongInFlameAreas", 225 },
		{ "SectFulongOutLaws", 226 },
		{ "SettlementPrisonRecordCollections", 227 },
		{ "VillagerRoles", 228 },
		{ "SectFulongLoseFeatherChickens", 229 },
		{ "StockStorage", 230 },
		{ "CraftStorage", 231 },
		{ "MedicineStorage", 232 },
		{ "FullPoisonEffects", 233 },
		{ "FoodStorage", 234 },
		{ "TaiwuVillageStoragesRecordCollection", 235 },
		{ "ItemPriceFluctuation", 236 },
		{ "VillagerRoleNickNameMap", 237 },
		{ "VillagerRoleMaxUnlockCounts", 238 },
		{ "BuildingResourceOutputSettings", 239 },
		{ "BlockRecoveryUnlockDates", 240 },
		{ "TaiwuVillageStorageSettingDict", 241 },
		{ "CharacterConsummateLevelProgresses", 242 },
		{ "TaiwuWantedFirstInteractOrganizationMember", 243 },
		{ "AreaSpiritualDebt", 244 },
		{ "TaiwuProfessions", 245 },
		{ "TaiwuProfessionSkillSlots", 246 },
		{ "CharacterSpecialGroup", 247 },
		{ "CricketPlaceExtraData", 248 },
		{ "BranchMerchantData", 249 },
		{ "CharacterCombatSkillConfigurations", 250 },
		{ "CharacterEquippedCombatSkills", 251 },
		{ "InteractedCharacterList", 252 },
		{ "FriendOrFamilyInteractionCooldownDict", 253 },
		{ "SectZhujianGearMates", 254 },
		{ "ProfessionUpgrade", 255 },
		{ "FollowMovementCharacters", 256 },
		{ "TriggeredAddSeniorityPoints", 257 },
		{ "TaiwuGiftItems", 258 },
		{ "SectZhujianThiefList", 259 },
		{ "CharacterProfessions", 260 },
		{ "CaravanExtraDataDict", 261 },
		{ "ProtectCaravanTime", 262 },
		{ "SectZhujianAreaMerchantTypeDict", 263 },
		{ "MerchantExtraGoods", 264 },
		{ "ExorcismEnabled", 265 },
		{ "SectStorySpecialMerchant", 266 },
		{ "IsExtraProfessionSkillUnlocked", 267 },
		{ "IsDirectTraveling", 268 },
		{ "KidnappedTravelData", 269 },
		{ "VillagerLastInfluencePowerGrade", 270 },
		{ "VillagerTreasuryNeeds", 271 },
		{ "CharacterAvatarExtraDataDict", 272 },
		{ "LootYield", 273 },
		{ "CityPunishmentSeverityCustomizeDict", 274 },
		{ "CharacterSkillBreakBonuses", 275 },
		{ "CombatSkillProficiencies", 276 },
		{ "SkillBreakPlates", 277 },
		{ "CombatSkillBreakPlateList", 278 },
		{ "SectShaolinDemonSlayerData", 279 },
		{ "KongsangCharacterFeaturePoisonedProb", 280 },
		{ "PickupDict", 281 },
		{ "TaiwuSelectedDebateCardType", 282 },
		{ "VillagerRoleRecords", 283 },
		{ "BuildingBlockDataEx", 284 },
		{ "VillagerRoleAutoActionStates", 285 },
		{ "BuildingArrangementSettingPresetData", 286 },
		{ "BuildingArtisanOrders", 287 },
		{ "TaiwuVillagerPotentialData", 288 },
		{ "NpcArtisanOrders", 289 },
		{ "ShopVillagerQualificationImprove", 290 },
		{ "BigEvents", 291 },
		{ "HasGetShuiHuoYingQiGongSkillBookByArchiveFix", 292 },
		{ "FarmerAutoCollectStorageType", 293 },
		{ "WoodenXiangshuAvatarSelectedFeatures", 294 },
		{ "BuildingAreaEffectProgresses", 295 },
		{ "PreviousLegendaryBookOwners", 296 },
		{ "VillagerSkillLegacyPointDict", 297 },
		{ "KilledByYufuCharactersBinary", 298 },
		{ "DyingCharacters", 299 },
		{ "ProficiencyEnoughSkills", 300 },
		{ "SectYuanshanThreeVitals", 301 },
		{ "DreamBackGenealogy", 302 },
		{ "ResourceBlockExtraData", 303 },
		{ "UnlockedFeastTypes", 304 },
		{ "Feasts", 305 },
		{ "SettlementMemberFeatures", 306 },
		{ "SettlementLayeredTreasuries", 307 },
		{ "BuildingDefaultStoreLocation", 308 },
		{ "TaiwuVillageVowOrgTemplateCrossArchiveDict", 309 },
		{ "CricketCombatPlans", 310 },
		{ "LastCricketPlanIndex", 311 },
		{ "CharacterDarkAshCounterData", 312 },
		{ "FuyuFaith", 313 }
	};

	public static readonly string[] DataId2FieldName = new string[314]
	{
		"CharacterSecretInformationUsedCounts", "SecretInformationOccurredLocations", "SecretInformationBroadcastList", "ExchangedSpecialWeaponList", "CaravanStayDays", "MerchantCharToType", "MerchantCharToExtraGoods", "StationInited", "ManualChangeEquipGroupCharIds", "StoneRoomCharList",
		"HideSkeletonEquipSlots", "CombatSkillOrderPlans", "LocationMarkHashSet", "TaiwuExtraBonuses", "AutoWorkBlockIndexList", "AutoSoldBlockIndexList", "SecretInformationBroadcastNotifyList", "TreasuryItems", "TroughItems", "XiangshuIdInKungfuPracticeRoom",
		"RemovedSpecialRelations", "ShopArrangeResultFirstList", "AutoExpandBlockIndexList", "WorldCharacterExtraTitles", "ReadingEventBookIdList", "ClearedSkillPlateStepInfo", "TravelingEventCollection", "DlcArgBox", "PrevTriggeredTravelingEvents", "GainsInTravel",
		"MaxApprovingRateTemporaryBonus", "LifeSkillCombatCardDict", "LifeSkillCombatUsedCardDict", "LifeSkillCombatReadBookPageDict", "LifeSkillCombatNewCardDict", "CharacterAiActionRestrictions", "ExtraExternalRelationStates", "SecretInformationBroadcastNotifyExtraList", "CharTeammateCommandDict", "NicknameDict",
		"LegendaryBookBreakPlateCounts", "ContestForLegendaryBookChars", "LegendaryBookWeaponSlot", "LegendaryBookWeaponEffectId", "LegendaryBookSkillSlot", "LegendaryBookSkillEffectId", "LegendaryBookBonusCountYin", "LegendaryBookBonusCountYang", "PrevLegendaryBookOwnerCopies", "LegendaryBookShockedMonths",
		"CombatSkillBreakPlateLastClearTimeList", "CombatSkillCurrBreakPlateIndex", "CombatSkillBreakPlateObsoleteList", "LegendaryBookConsumedCharIds", "CharacterAiActionCooldowns", "CharacterTemporaryFeatures", "ExtraTriggeredTasks", "TaskSortingOrder", "LegendaryBookHiddenCharIds", "CharacterAiActionSuccessRateAdjusts",
		"LoveDataDict", "PreviousLoverSet", "ConfessLoveFailedSet", "LoveTokenDataDict", "ObsoleteTaiwuProfessions", "TaiwuCurrProfessionId", "SavedSouls", "CricketIsSmart", "CricketIsIdentified", "TaiwuInteractionCooldowns",
		"CharacterCustomDisplayNames", "AnimalAreaData", "HandledOneShotEvents", "HunterAnimals", "WorldVersionInfo", "SectMainStoryEventArgBoxes", "ChangeTrickBodyPart", "ChangeTrickIndex", "VoiceWeaponInnerRatio", "SectXuehouBloodLightLocations",
		"CharacterAvatarSnapshot", "BrokenAreaMaterials", "SectWudangFairylandData", "TreasureMaterialFailedTimes", "SectYuanshanDemonEffectIds", "TaiwuMaxNeiliAllocation", "CharacterMasteredCombatSkills", "MasteredCombatSkillPlans", "BreakoutDifficulty", "ReadingDifficulty",
		"LoopingDifficulty", "CombatSkillBreakPlateLastForceBreakoutStepsCount", "PregeneratedFixedEnemies", "AutoCheckInComfortableList", "AutoCheckInResidenceList", "ReadingEventReferenceBooks", "MerchantTypeToDebts", "MerchantBuyBackDict", "MerchantMaxLevelBuyBackData", "CaravanBuyBackDict",
		"FirstLegendaryBookDelay", "LegaciesBuildingTemplateIdList", "IsDreamBack", "AbridgedDreamBackCharacters", "UnlockedWorkingVillagers", "DreamBackLifeRecords", "ActionPointCurrMonth", "ActionPointPrevMonth", "AdvancedTeammateCommandDict", "LastMonthNotificationsIndex",
		"PreviousMonthlyNotifications", "ActionPointPreserved", "VillageWorkQualificationImprove", "ReadInLifeSkillCombatCount", "SectWudangHeavenlyTree", "SectWudangHeavenlyTreeList", "SectWudangLingBaoDark", "SectWudangLingBaoLight", "ShixiangBarbarianMasterIdList", "LegaciesBuildingList",
		"GroupCharacterEquipmentRecord", "SectEmeiSkillBreakBonus", "InformationSettings", "SectXuannvUnlockedMusicList", "SectXuannvPlayerPlayMode", "SectXuannvPlayerMusicId", "SectXuannvPlayerIsEnabled", "SectEmeiBreakBonusTemplateIds", "MirrorCharacters", "SectEmeiBloodLocations",
		"SectEmeiExp", "EnemyPracticeLevel", "CharacterPoisonImmunities", "TaiwuInformationReceivedHistories", "SectXuannvEvaluatedMusicList", "CombatSkillJumpThreshold", "DreamBackUnlockStates", "DreamBackArchiveBackup", "OverwrittenTaiwu", "DreamBackLocationData",
		"DreamBackGenealogyObsoleted", "ConflictCombatSkills", "CharacterRevealedHobbies", "FinalDateBeforeDreamBack", "DreamBackTaiwu", "ConflictEffectWrappers", "DreamBackGlobalEventArgBox", "DreamBackDlcEventArgBox", "DreamBackSectMainStoryEventArgBox", "DejaVuEventCharacters",
		"TaiwuPropertyPermanentBonuses", "SelectedUniqueLegacies", "TaiwuAddOneWayRelationCoolDown", "CarrierTamePoint", "DreamBackPreviousTaiwuCharIds", "FleeCarrierLocation", "FavorabilityChange", "CanResetWorldSettings", "SettlementExtraData", "DlcArgBoxes",
		"JiaoPools", "Jiaos", "ChildrenOfLoong", "FiveLoongDict", "JiaoPoolRecords", "DlcEntries", "JiaoPoolStatus", "ChoosyRemainUpgradeRateDict", "ChoosyRemainUpgradeCountDict", "BuildingMoneyPrestigeSuccessRateCompensation",
		"OwnedClothingSet", "ClothingDisplayModifications", "DisableEnemyAi", "EnemyUnyieldingFallen", "LastTargetDistance", "WeaveClothingDisplaySetting", "EmptyToolKey", "SectWuxianWugJugPoisons", "LocationSupportingBlockResourceTotal", "CharacterPrioritizedActionCooldowns",
		"CricketCollectionDataList", "RecruitCharacterDataLists", "SectJingangPossessionCharacters", "MixedPoisonEffectTriggerDates", "CricketExtraAge", "EnemyNestInitializationDates", "SecretInformationOccurrence", "SecretInformationHandleMap", "SecretInformationCharacterOwn", "SecretInformationSecretInformationCharacterExtraInfoCollectionMap",
		"SecretInformationCharacterRelationshipSnapshotCollectionMap", "SecretInformationShopCharacterData", "ChangeTrickIsFlaw", "NextAnimalId", "Animals", "SettlementTreasuries", "PrevMartialArtTournamentWinners", "BookStrategiesExpireTime", "UnlockedCombatSkillPlanCount", "ActiveLoopingProgress",
		"ActiveReadingProgress", "CharacterExtraNeiliAllocationProgress", "QiArtStrategyList", "QiArtStrategyExpireTimeList", "LegacyPointTimesDict", "SectRanshanThreeCorpses", "SectBaihuaLifeLinkData", "SamsaraPlatformRecordCollection", "AvailableReadingStrategyMap", "ReferenceSkillList",
		"AvailableQiArtStrategyMap", "LoopingEventSkillIdList", "QiArtStrategyMap", "QiArtStrategyExpireTimeMap", "LoopInLifeSkillCombatCount", "LoopInCombatCount", "SettlementTreasuryRecordCollections", "MonthlyNotificationSortingGroups", "FollowingNpcList", "SectEmeiBreakBonusData",
		"FollowingNickNameMap", "MerchantOverFavorDataArray", "ChangedTeammateCharIds", "SettlementPrisons", "SectFulongOrgMemberChickens", "SectFulongInFlameAreas", "SectFulongOutLaws", "SettlementPrisonRecordCollections", "VillagerRoles", "SectFulongLoseFeatherChickens",
		"StockStorage", "CraftStorage", "MedicineStorage", "FullPoisonEffects", "FoodStorage", "TaiwuVillageStoragesRecordCollection", "ItemPriceFluctuation", "VillagerRoleNickNameMap", "VillagerRoleMaxUnlockCounts", "BuildingResourceOutputSettings",
		"BlockRecoveryUnlockDates", "TaiwuVillageStorageSettingDict", "CharacterConsummateLevelProgresses", "TaiwuWantedFirstInteractOrganizationMember", "AreaSpiritualDebt", "TaiwuProfessions", "TaiwuProfessionSkillSlots", "CharacterSpecialGroup", "CricketPlaceExtraData", "BranchMerchantData",
		"CharacterCombatSkillConfigurations", "CharacterEquippedCombatSkills", "InteractedCharacterList", "FriendOrFamilyInteractionCooldownDict", "SectZhujianGearMates", "ProfessionUpgrade", "FollowMovementCharacters", "TriggeredAddSeniorityPoints", "TaiwuGiftItems", "SectZhujianThiefList",
		"CharacterProfessions", "CaravanExtraDataDict", "ProtectCaravanTime", "SectZhujianAreaMerchantTypeDict", "MerchantExtraGoods", "ExorcismEnabled", "SectStorySpecialMerchant", "IsExtraProfessionSkillUnlocked", "IsDirectTraveling", "KidnappedTravelData",
		"VillagerLastInfluencePowerGrade", "VillagerTreasuryNeeds", "CharacterAvatarExtraDataDict", "LootYield", "CityPunishmentSeverityCustomizeDict", "CharacterSkillBreakBonuses", "CombatSkillProficiencies", "SkillBreakPlates", "CombatSkillBreakPlateList", "SectShaolinDemonSlayerData",
		"KongsangCharacterFeaturePoisonedProb", "PickupDict", "TaiwuSelectedDebateCardType", "VillagerRoleRecords", "BuildingBlockDataEx", "VillagerRoleAutoActionStates", "BuildingArrangementSettingPresetData", "BuildingArtisanOrders", "TaiwuVillagerPotentialData", "NpcArtisanOrders",
		"ShopVillagerQualificationImprove", "BigEvents", "HasGetShuiHuoYingQiGongSkillBookByArchiveFix", "FarmerAutoCollectStorageType", "WoodenXiangshuAvatarSelectedFeatures", "BuildingAreaEffectProgresses", "PreviousLegendaryBookOwners", "VillagerSkillLegacyPointDict", "KilledByYufuCharactersBinary", "DyingCharacters",
		"ProficiencyEnoughSkills", "SectYuanshanThreeVitals", "DreamBackGenealogy", "ResourceBlockExtraData", "UnlockedFeastTypes", "Feasts", "SettlementMemberFeatures", "SettlementLayeredTreasuries", "BuildingDefaultStoreLocation", "TaiwuVillageVowOrgTemplateCrossArchiveDict",
		"CricketCombatPlans", "LastCricketPlanIndex", "CharacterDarkAshCounterData", "FuyuFaith"
	};

	public static readonly string[][] DataId2ObjectFieldId2FieldName = new string[314][];

	public static readonly Dictionary<string, ushort> MethodName2MethodId = new Dictionary<string, ushort>
	{
		{ "AddManualChangeEquipGroupChar", 0 },
		{ "RemoveManualChangeEquipGroupChar", 1 },
		{ "AddHideSkeletonEquipSlot", 2 },
		{ "RemoveHideSkeletonEquipSlot", 3 },
		{ "SetCombatSkillOrderPlan", 4 },
		{ "AddLocationMark", 5 },
		{ "RemoveLocationMark", 6 },
		{ "GetAllLocationMark", 7 },
		{ "TroughRemove", 8 },
		{ "TreasuryAdd", 9 },
		{ "TreasuryAddList", 10 },
		{ "TreasuryRemove", 11 },
		{ "TreasuryRemoveList", 12 },
		{ "TroughAdd", 13 },
		{ "TroughAddList", 14 },
		{ "TroughRemoveList", 15 },
		{ "AddReadingEventBookId", 16 },
		{ "RemoveReadingEventBookId", 17 },
		{ "GetAllLifeSkillCombatUsedCard", 18 },
		{ "GetAllLifeSkillCombatCard", 19 },
		{ "SetLifeSkillCombatUsedCard", 20 },
		{ "GetCharacterLifeSkillCombatUsedCard", 21 },
		{ "GetLifeSkillCombatUsedCard", 22 },
		{ "GetAllLifeSkillCombatNewCard", 23 },
		{ "SetLifeSkillCombatCardNotNew", 24 },
		{ "GetCharTeammateCommands", 25 },
		{ "SetLegendaryBookWeaponSlot", 26 },
		{ "SetLegendaryBookSkillSlot", 27 },
		{ "UnlockLegendaryBookBreakPlate", 28 },
		{ "UnlockLegendaryBookBonus", 29 },
		{ "ChangeCombatSkillBreakPlate", 30 },
		{ "EnterUnlockBreakPlateCombat", 31 },
		{ "SetTopTask", 32 },
		{ "GmCmd_SetLoveData", 33 },
		{ "GmCmd_RemoveLoveData", 34 },
		{ "GmCmd_RemoveLoveEvent", 35 },
		{ "ChangeProfession", 36 },
		{ "ExecuteActiveProfessionSkill", 37 },
		{ "IsProfessionalSkillUnlocked", 38 },
		{ "CanExecuteProfessionSkill", 39 },
		{ "SetProfessionTestSetting", 40 },
		{ "GetCharacterCustomDisplayName", 41 },
		{ "GetTianJieFuLuCount", 42 },
		{ "GmCmd_GenerateTreasure", 43 },
		{ "FindTreasure", 44 },
		{ "CheckSpecialCondition", 45 },
		{ "ConfirmExecuteSkill", 46 },
		{ "FindTreasureExpect", 47 },
		{ "GetLocationHasHunterAnimal", 48 },
		{ "SetProfessionSeniorityCurrent", 49 },
		{ "UnlockAllProfessionSkills", 50 },
		{ "SetProfessionSeniorityTarget", 51 },
		{ "GmCmd_Profession_SetBuddhistMonkSavedSoulCount", 52 },
		{ "GmCmd_Profession_SetTempleVisited", 53 },
		{ "InitAiLifeSkillCombatUsedCard", 54 },
		{ "InvokeSeniorityCachedEvent", 55 },
		{ "GmCmd_Profession_RecoverHunterCarrierAttackCount", 56 },
		{ "GetBlockMerchantTypes", 57 },
		{ "GmCmd_RemoveTriggeredExtraTask", 58 },
		{ "GmCmd_AddExtraTask", 59 },
		{ "GetCharacterMasteredCombatSkills", 60 },
		{ "AddCharacterMasteredCombatSkill", 61 },
		{ "RemoveCharacterMasteredCombatSkill", 62 },
		{ "InvokeFindExtraTreasureEvent", 63 },
		{ "SetAdvancedTeammateCommands", 64 },
		{ "CancelAdvancedTeammateCommands", 65 },
		{ "GetAllPreviousMonthlyNotifications", 66 },
		{ "GetPreviousMonthlyNotifications", 67 },
		{ "PreserveActionPoint", 68 },
		{ "GetPreserveDay", 69 },
		{ "GetAllHeavenlyTrees", 70 },
		{ "GetHeavenlyTreeNearBlocks", 71 },
		{ "GetInformationSettings", 72 },
		{ "RemoveEmeiSkillBreakBonus", 73 },
		{ "AddEmeiSkillBreakBonus", 74 },
		{ "GetEmeiBreakBonusDisplayData", 75 },
		{ "GetEmeiBreakBonusCollection", 76 },
		{ "GetPoisonImmunities", 77 },
		{ "GetDreamBackTaiwuRelatedCharactersForRelations", 78 },
		{ "GetDreamBackTaiwuGenealogy", 79 },
		{ "GetCharacterDisplayDataListForDreamBackRelations", 80 },
		{ "GetDreamBackLifeRecordByDate", 81 },
		{ "GetNameAndLifeRelatedDataListForDreamBack", 82 },
		{ "IsCharacterHatingItemRevealed", 83 },
		{ "IsCharacterLovingItemRevealed", 84 },
		{ "IsCharacterHobbyRevealed", 85 },
		{ "SetCharacterRevealedHobbies", 86 },
		{ "GetConflictCombatSkill", 87 },
		{ "GetAllDreamBackLifeRecords", 88 },
		{ "GetDreamBackTaiwuBirthAndEndDates", 89 },
		{ "IsCurrentTaiwuOverwrittenByDreamBack", 90 },
		{ "ApplyConflictCombatSkillResult", 91 },
		{ "HaveConflictCombatSkill", 92 },
		{ "AddTaiwuOneWayRelationCoolDown", 93 },
		{ "IsTaiwuAbleToAddOneWayRelation", 94 },
		{ "GetTaiwuAddOneWayRelationCoolDown", 95 },
		{ "FeedCarrier", 96 },
		{ "GetCarrierTamePoint", 97 },
		{ "GetAllCharacterPropertyBonusData", 98 },
		{ "GetDreamBackCharacterDisplayDataList", 99 },
		{ "GetCarrierMaxTamePoint", 100 },
		{ "GetCurrMaxJiaoPoolCount", 101 },
		{ "GmCmd_FindFiveLoongLocation", 102 },
		{ "GetJiaoPoolBlockStyle", 103 },
		{ "SetJiaoPoolBlockStyle", 104 },
		{ "GetChildrenOfLoongById", 105 },
		{ "GetJiaoPoolList", 106 },
		{ "GetJiaoById", 107 },
		{ "GetJiaoPoolAllJiaoData", 108 },
		{ "GetJiaoPoolBreedingProcess", 109 },
		{ "PutJiaoInPool", 110 },
		{ "PutAnotherJiaoInPool", 111 },
		{ "PutJiaoOutOfPool", 112 },
		{ "ChangeNurturance", 113 },
		{ "ChangeJiaoName", 114 },
		{ "DisableJiaoPool", 115 },
		{ "EnableJiaoPool", 116 },
		{ "GetJiaoLoongNameRelatedDataList", 117 },
		{ "GetAllJiaoForPool", 118 },
		{ "GetAllJiaoForEvolve", 119 },
		{ "GetJiaoByItemKey", 120 },
		{ "GetJiaosByItemKeys", 121 },
		{ "GetChildrenOfLoongByItemKey", 122 },
		{ "PutEggIntoPool", 123 },
		{ "JiaoPoolInteract", 124 },
		{ "GmCmd_AddJiao", 125 },
		{ "GmCmd_PutJiaoInFirstPool", 126 },
		{ "GmCmd_AddChildOfLoong", 127 },
		{ "JiaoEvolveToChildOfLoong", 128 },
		{ "GetJiaoEvolutionChoice", 129 },
		{ "ResetJiaoPoolStatus", 130 },
		{ "GetAllAdultJiao", 131 },
		{ "GetAllEvolvingJiao", 132 },
		{ "GetJiaoTemplateIdByCarrierTemplateId", 133 },
		{ "CalcResourceChangeByJiaoPool", 134 },
		{ "IsOwnedChildrenOfLoong", 135 },
		{ "GetNextRandomChildrenOfLoong", 136 },
		{ "GmCmd_AddFleeCarrier", 137 },
		{ "GetIsJiaoPoolOpen", 138 },
		{ "FillJiaoRecordArgumentCollection", 139 },
		{ "GetJiaoEvolutionPageStatus", 140 },
		{ "GetIsBabysittingMode", 141 },
		{ "SetIsBabysittingMode", 142 },
		{ "GetFiveLoongDictCount", 143 },
		{ "GetJiaoLoongNameRelatedData", 144 },
		{ "IsJiaoAbleToPet", 145 },
		{ "PetJiao", 146 },
		{ "JiaoPoolPetJiao", 147 },
		{ "GetTaiwuAddOneWayRelationResultCode", 148 },
		{ "RequestRecruitCharacterData", 149 },
		{ "GetCharacterTemporaryFeaturesExpireDate", 150 },
		{ "GmCmd_AddThreeCorpses", 151 },
		{ "ApplyRanshanThreeCorpsesLegendaryBookKeepingResult", 152 },
		{ "GetItemListForRanshanTreeCorpsesLegendaryBookKeeping", 153 },
		{ "GmCmd_AddDisplayEventLegendaryBookKeeping", 154 },
		{ "SetRanshanThreeCorpsesCharacterTarget", 155 },
		{ "GetOrInitExtraNeiliAllocationProgress", 156 },
		{ "GetBookStrategiesExpireTime", 157 },
		{ "SetMonthlyNotificationSortingGroup", 158 },
		{ "SetCharTeammateCommandsManual", 159 },
		{ "GetCharAdvancedTeammateCommands", 160 },
		{ "IsStoneRoomFull", 161 },
		{ "ExtinguishFulongInFlameArea", 162 },
		{ "TriggerFulongInFlameAreaMine", 163 },
		{ "ApplyFulongInFlameAreaFullyExtinguished", 164 },
		{ "GmCmd_GenerateFulongFlameArea", 165 },
		{ "GetTaiwuVillageStoragesRecordCollection", 166 },
		{ "FulongSpecialInteractOpen", 167 },
		{ "HunterSkill_AnimalCharacterToItem", 168 },
		{ "ConfirmProfessionSkillsEquipment", 169 },
		{ "GmCmd_CastTasterUltimateOnCurrentBlock", 170 },
		{ "EatTianJieFuLu", 171 },
		{ "CheckAristocratUltimateSpecialCondition", 172 },
		{ "CheckBeggarUltimateSpecialCondition", 173 },
		{ "CheckTasterUltimateSpecialCondition", 174 },
		{ "GM_GetFriendOrFamilySendGift", 175 },
		{ "GmCmd_CreateGearMate", 176 },
		{ "GetGearMateRepairEffect", 177 },
		{ "RepairGearMate", 178 },
		{ "GetGearMateRepairRequirement", 179 },
		{ "GetGearMateAvailableRepairCount", 180 },
		{ "GetGearMateRepairRequirementDisplayDatas", 181 },
		{ "UpgradeGearMate", 182 },
		{ "GetCharacterConsummateLevelProgress", 183 },
		{ "GetMartialArtistCreateGoodRandomEnemyAndBadRandomEnemyCount", 184 },
		{ "GetGearMateById", 185 },
		{ "GetCharacterCurrentProfession", 186 },
		{ "CheckSpecialCondition_SavageSkill_1", 187 },
		{ "GetMerchantExtraGoods", 188 },
		{ "SetProfessionExtraSeniority", 189 },
		{ "GmCmd_SetCharacterCurrProfessionSeniority", 190 },
		{ "CanShowProfessionSkillUnlocked", 191 },
		{ "GetGearMateBreakoutCombatSkillBanReasonList", 192 },
		{ "SetDukeSkill3Crickets", 193 },
		{ "GetAllSkillBooksGearMateCanRead", 194 },
		{ "CanIdentifyCricket", 195 },
		{ "CanUpgradeCricket", 196 },
		{ "CanConvertToAnimalCharacter", 197 },
		{ "GetCharacterAvatarExtraData", 198 },
		{ "GetJiaoLoongDisplayDataByItemKey", 199 },
		{ "UpdateCityPunishmentSeverityCustomizeData", 200 },
		{ "GmCmd_SetCharacterProficiencies", 201 },
		{ "GmCmd_CreateRandomEnemyAroundHeavenlyTree", 202 },
		{ "RemoveTreasuryItemList", 203 },
		{ "RemoveTreasuryItem", 204 },
		{ "AddTreasuryItemList", 205 },
		{ "AddTreasuryItem", 206 },
		{ "GmCmd_ShowUnlockedProfessionSkill", 207 },
		{ "UnlockBuildingLevelSlot", 208 },
		{ "SetVillagerRoleAutoActionState", 209 },
		{ "UpgradeResourceBuilding", 210 },
		{ "ChangeBuildingArrangementSettingPresetData", 211 },
		{ "ChangeBuildingArrangementPresetId", 212 },
		{ "AddMaterialToArtisanOrder", 213 },
		{ "AddResourceToArtisanOrder", 214 },
		{ "GetArtisanOrderProductionPool", 215 },
		{ "SetArtisanOrderProductionType", 216 },
		{ "SetArtisanOrderStorageType", 217 },
		{ "GetNpcArtisanOrder", 218 },
		{ "InterceptArtisanOrder", 219 },
		{ "GetBuildingArtisanOrder", 220 },
		{ "CreateArtisanOrder", 221 },
		{ "GetCustomizePunishmentSeverityCost", 222 },
		{ "GetProductionPoolPreview", 223 },
		{ "ArtisanOrderDebate", 224 },
		{ "UpgradeSlotBuilding", 225 },
		{ "GetArtisanOrderMaterialPreview", 226 },
		{ "GetArtisanOrderCanProduceItemSubType", 227 },
		{ "WillCustomizePunishmentBreakWithoutVillagerHead", 228 },
		{ "SetFarmerAutoCollectStorageType", 229 },
		{ "UpdateWoodenXiangshuAvatarSelectedFeatures", 230 },
		{ "SetBuildingArrangementSetting", 231 },
		{ "GmCmd_GetBuildingAreaEffectProgresses", 232 },
		{ "GmCmd_SetBuildingAreaEffectProgresses", 233 },
		{ "GmCmd_ReleaseAllKilledByLongYufuCharacters", 234 },
		{ "GmCmd_RecordKilledByLongYufuCharacter", 235 },
		{ "GmCmd_VitalInfectionInOut", 236 },
		{ "CheckSpecialCondition_HunterSkill2", 237 },
		{ "GetThreeVitalsCharDataList", 238 },
		{ "GmCmd_InitThreeVitals", 239 },
		{ "GetThreeVitalsTargetCharDataList", 240 },
		{ "TransferInfectionBetweenVitalAndCharacter", 241 },
		{ "SetVitalInPrison", 242 },
		{ "GetBuildingArtisanOrderAfterUpdate", 243 },
		{ "GetCanSelectThreeVitalsDisplayData", 244 },
		{ "AreVitalsDemon", 245 },
		{ "GetOppositeThreeVitalsCharDataList", 246 },
		{ "SetVitalHasPlayedComeAnim", 247 },
		{ "GetResourceBlockProducingCoreCooldown", 248 },
		{ "FeastAddDish", 249 },
		{ "FeastSetAutoRefill", 250 },
		{ "GetFeast", 251 },
		{ "FeastRemoveDish", 252 },
		{ "FeastReceiveGift", 253 },
		{ "AddResourceItemToArtisanOrder", 254 },
		{ "IsFeastException", 255 },
		{ "UseFeastThanksLetter", 256 },
		{ "FeastQuickRefill", 257 },
		{ "FeastSetTargetType", 258 },
		{ "SetBuildingSoldItemSetting", 259 }
	};

	public static readonly string[] MethodId2MethodName = new string[260]
	{
		"AddManualChangeEquipGroupChar", "RemoveManualChangeEquipGroupChar", "AddHideSkeletonEquipSlot", "RemoveHideSkeletonEquipSlot", "SetCombatSkillOrderPlan", "AddLocationMark", "RemoveLocationMark", "GetAllLocationMark", "TroughRemove", "TreasuryAdd",
		"TreasuryAddList", "TreasuryRemove", "TreasuryRemoveList", "TroughAdd", "TroughAddList", "TroughRemoveList", "AddReadingEventBookId", "RemoveReadingEventBookId", "GetAllLifeSkillCombatUsedCard", "GetAllLifeSkillCombatCard",
		"SetLifeSkillCombatUsedCard", "GetCharacterLifeSkillCombatUsedCard", "GetLifeSkillCombatUsedCard", "GetAllLifeSkillCombatNewCard", "SetLifeSkillCombatCardNotNew", "GetCharTeammateCommands", "SetLegendaryBookWeaponSlot", "SetLegendaryBookSkillSlot", "UnlockLegendaryBookBreakPlate", "UnlockLegendaryBookBonus",
		"ChangeCombatSkillBreakPlate", "EnterUnlockBreakPlateCombat", "SetTopTask", "GmCmd_SetLoveData", "GmCmd_RemoveLoveData", "GmCmd_RemoveLoveEvent", "ChangeProfession", "ExecuteActiveProfessionSkill", "IsProfessionalSkillUnlocked", "CanExecuteProfessionSkill",
		"SetProfessionTestSetting", "GetCharacterCustomDisplayName", "GetTianJieFuLuCount", "GmCmd_GenerateTreasure", "FindTreasure", "CheckSpecialCondition", "ConfirmExecuteSkill", "FindTreasureExpect", "GetLocationHasHunterAnimal", "SetProfessionSeniorityCurrent",
		"UnlockAllProfessionSkills", "SetProfessionSeniorityTarget", "GmCmd_Profession_SetBuddhistMonkSavedSoulCount", "GmCmd_Profession_SetTempleVisited", "InitAiLifeSkillCombatUsedCard", "InvokeSeniorityCachedEvent", "GmCmd_Profession_RecoverHunterCarrierAttackCount", "GetBlockMerchantTypes", "GmCmd_RemoveTriggeredExtraTask", "GmCmd_AddExtraTask",
		"GetCharacterMasteredCombatSkills", "AddCharacterMasteredCombatSkill", "RemoveCharacterMasteredCombatSkill", "InvokeFindExtraTreasureEvent", "SetAdvancedTeammateCommands", "CancelAdvancedTeammateCommands", "GetAllPreviousMonthlyNotifications", "GetPreviousMonthlyNotifications", "PreserveActionPoint", "GetPreserveDay",
		"GetAllHeavenlyTrees", "GetHeavenlyTreeNearBlocks", "GetInformationSettings", "RemoveEmeiSkillBreakBonus", "AddEmeiSkillBreakBonus", "GetEmeiBreakBonusDisplayData", "GetEmeiBreakBonusCollection", "GetPoisonImmunities", "GetDreamBackTaiwuRelatedCharactersForRelations", "GetDreamBackTaiwuGenealogy",
		"GetCharacterDisplayDataListForDreamBackRelations", "GetDreamBackLifeRecordByDate", "GetNameAndLifeRelatedDataListForDreamBack", "IsCharacterHatingItemRevealed", "IsCharacterLovingItemRevealed", "IsCharacterHobbyRevealed", "SetCharacterRevealedHobbies", "GetConflictCombatSkill", "GetAllDreamBackLifeRecords", "GetDreamBackTaiwuBirthAndEndDates",
		"IsCurrentTaiwuOverwrittenByDreamBack", "ApplyConflictCombatSkillResult", "HaveConflictCombatSkill", "AddTaiwuOneWayRelationCoolDown", "IsTaiwuAbleToAddOneWayRelation", "GetTaiwuAddOneWayRelationCoolDown", "FeedCarrier", "GetCarrierTamePoint", "GetAllCharacterPropertyBonusData", "GetDreamBackCharacterDisplayDataList",
		"GetCarrierMaxTamePoint", "GetCurrMaxJiaoPoolCount", "GmCmd_FindFiveLoongLocation", "GetJiaoPoolBlockStyle", "SetJiaoPoolBlockStyle", "GetChildrenOfLoongById", "GetJiaoPoolList", "GetJiaoById", "GetJiaoPoolAllJiaoData", "GetJiaoPoolBreedingProcess",
		"PutJiaoInPool", "PutAnotherJiaoInPool", "PutJiaoOutOfPool", "ChangeNurturance", "ChangeJiaoName", "DisableJiaoPool", "EnableJiaoPool", "GetJiaoLoongNameRelatedDataList", "GetAllJiaoForPool", "GetAllJiaoForEvolve",
		"GetJiaoByItemKey", "GetJiaosByItemKeys", "GetChildrenOfLoongByItemKey", "PutEggIntoPool", "JiaoPoolInteract", "GmCmd_AddJiao", "GmCmd_PutJiaoInFirstPool", "GmCmd_AddChildOfLoong", "JiaoEvolveToChildOfLoong", "GetJiaoEvolutionChoice",
		"ResetJiaoPoolStatus", "GetAllAdultJiao", "GetAllEvolvingJiao", "GetJiaoTemplateIdByCarrierTemplateId", "CalcResourceChangeByJiaoPool", "IsOwnedChildrenOfLoong", "GetNextRandomChildrenOfLoong", "GmCmd_AddFleeCarrier", "GetIsJiaoPoolOpen", "FillJiaoRecordArgumentCollection",
		"GetJiaoEvolutionPageStatus", "GetIsBabysittingMode", "SetIsBabysittingMode", "GetFiveLoongDictCount", "GetJiaoLoongNameRelatedData", "IsJiaoAbleToPet", "PetJiao", "JiaoPoolPetJiao", "GetTaiwuAddOneWayRelationResultCode", "RequestRecruitCharacterData",
		"GetCharacterTemporaryFeaturesExpireDate", "GmCmd_AddThreeCorpses", "ApplyRanshanThreeCorpsesLegendaryBookKeepingResult", "GetItemListForRanshanTreeCorpsesLegendaryBookKeeping", "GmCmd_AddDisplayEventLegendaryBookKeeping", "SetRanshanThreeCorpsesCharacterTarget", "GetOrInitExtraNeiliAllocationProgress", "GetBookStrategiesExpireTime", "SetMonthlyNotificationSortingGroup", "SetCharTeammateCommandsManual",
		"GetCharAdvancedTeammateCommands", "IsStoneRoomFull", "ExtinguishFulongInFlameArea", "TriggerFulongInFlameAreaMine", "ApplyFulongInFlameAreaFullyExtinguished", "GmCmd_GenerateFulongFlameArea", "GetTaiwuVillageStoragesRecordCollection", "FulongSpecialInteractOpen", "HunterSkill_AnimalCharacterToItem", "ConfirmProfessionSkillsEquipment",
		"GmCmd_CastTasterUltimateOnCurrentBlock", "EatTianJieFuLu", "CheckAristocratUltimateSpecialCondition", "CheckBeggarUltimateSpecialCondition", "CheckTasterUltimateSpecialCondition", "GM_GetFriendOrFamilySendGift", "GmCmd_CreateGearMate", "GetGearMateRepairEffect", "RepairGearMate", "GetGearMateRepairRequirement",
		"GetGearMateAvailableRepairCount", "GetGearMateRepairRequirementDisplayDatas", "UpgradeGearMate", "GetCharacterConsummateLevelProgress", "GetMartialArtistCreateGoodRandomEnemyAndBadRandomEnemyCount", "GetGearMateById", "GetCharacterCurrentProfession", "CheckSpecialCondition_SavageSkill_1", "GetMerchantExtraGoods", "SetProfessionExtraSeniority",
		"GmCmd_SetCharacterCurrProfessionSeniority", "CanShowProfessionSkillUnlocked", "GetGearMateBreakoutCombatSkillBanReasonList", "SetDukeSkill3Crickets", "GetAllSkillBooksGearMateCanRead", "CanIdentifyCricket", "CanUpgradeCricket", "CanConvertToAnimalCharacter", "GetCharacterAvatarExtraData", "GetJiaoLoongDisplayDataByItemKey",
		"UpdateCityPunishmentSeverityCustomizeData", "GmCmd_SetCharacterProficiencies", "GmCmd_CreateRandomEnemyAroundHeavenlyTree", "RemoveTreasuryItemList", "RemoveTreasuryItem", "AddTreasuryItemList", "AddTreasuryItem", "GmCmd_ShowUnlockedProfessionSkill", "UnlockBuildingLevelSlot", "SetVillagerRoleAutoActionState",
		"UpgradeResourceBuilding", "ChangeBuildingArrangementSettingPresetData", "ChangeBuildingArrangementPresetId", "AddMaterialToArtisanOrder", "AddResourceToArtisanOrder", "GetArtisanOrderProductionPool", "SetArtisanOrderProductionType", "SetArtisanOrderStorageType", "GetNpcArtisanOrder", "InterceptArtisanOrder",
		"GetBuildingArtisanOrder", "CreateArtisanOrder", "GetCustomizePunishmentSeverityCost", "GetProductionPoolPreview", "ArtisanOrderDebate", "UpgradeSlotBuilding", "GetArtisanOrderMaterialPreview", "GetArtisanOrderCanProduceItemSubType", "WillCustomizePunishmentBreakWithoutVillagerHead", "SetFarmerAutoCollectStorageType",
		"UpdateWoodenXiangshuAvatarSelectedFeatures", "SetBuildingArrangementSetting", "GmCmd_GetBuildingAreaEffectProgresses", "GmCmd_SetBuildingAreaEffectProgresses", "GmCmd_ReleaseAllKilledByLongYufuCharacters", "GmCmd_RecordKilledByLongYufuCharacter", "GmCmd_VitalInfectionInOut", "CheckSpecialCondition_HunterSkill2", "GetThreeVitalsCharDataList", "GmCmd_InitThreeVitals",
		"GetThreeVitalsTargetCharDataList", "TransferInfectionBetweenVitalAndCharacter", "SetVitalInPrison", "GetBuildingArtisanOrderAfterUpdate", "GetCanSelectThreeVitalsDisplayData", "AreVitalsDemon", "GetOppositeThreeVitalsCharDataList", "SetVitalHasPlayedComeAnim", "GetResourceBlockProducingCoreCooldown", "FeastAddDish",
		"FeastSetAutoRefill", "GetFeast", "FeastRemoveDish", "FeastReceiveGift", "AddResourceItemToArtisanOrder", "IsFeastException", "UseFeastThanksLetter", "FeastQuickRefill", "FeastSetTargetType", "SetBuildingSoldItemSetting"
	};
}
