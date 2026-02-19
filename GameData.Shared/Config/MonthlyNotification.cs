using System;
using System.Collections.Generic;
using Config.Common;

namespace Config;

[Serializable]
public class MonthlyNotification : ConfigData<MonthlyNotificationItem, short>
{
	public static class DefKey
	{
		public const short SolarTerm0 = 0;

		public const short SolarTerm1 = 1;

		public const short SolarTerm2 = 2;

		public const short SolarTerm3 = 3;

		public const short SolarTerm4 = 4;

		public const short SolarTerm5 = 5;

		public const short SolarTerm6 = 6;

		public const short SolarTerm7 = 7;

		public const short SolarTerm8 = 8;

		public const short SolarTerm9 = 9;

		public const short SolarTerm10 = 10;

		public const short SolarTerm11 = 11;

		public const short GraveDestroyed = 12;

		public const short IncomeFromNest = 13;

		public const short LoseItemCausedByWarehouseFull = 14;

		public const short Assassinated = 15;

		public const short AssassinatedDueToKillerToken = 16;

		public const short Die = 17;

		public const short InfectXiangshuPartially = 18;

		public const short InfectXiangshuCompletely = 19;

		public const short CreateHatredByPrison = 20;

		public const short EscapeFromPrison = 21;

		public const short CricketEndLife = 22;

		public const short LoseResourceCausedByInventoryFull = 23;

		public const short LoseItemCausedByInventoryFull = 24;

		public const short CreateHatred = 25;

		public const short DecreaseHatred = 26;

		public const short ConfessLoveAndSucceed = 27;

		public const short SeverLove = 28;

		public const short Marriage = 29;

		public const short BecomeFriend = 30;

		public const short DecreaseFriendship = 31;

		public const short BecomeSwornBrotherOrSister = 32;

		public const short SeverFriendship = 33;

		public const short AdoptBoy = 34;

		public const short AdoptGirl = 35;

		public const short RecognizeFather = 36;

		public const short RecognizeMother = 37;

		public const short MakeLove = 38;

		public const short RapeFailure = 39;

		public const short MotherGiveBirthToBoy = 40;

		public const short MotherGiveBirthToGirl = 41;

		public const short FatherGetBoy = 42;

		public const short FatherGetGirl = 43;

		public const short GiveBirthToCricket = 44;

		public const short MotherLoseFetus = 45;

		public const short GoToJoinOrganization = 46;

		public const short JoinOrganization = 47;

		public const short GoToAppointment = 48;

		public const short WaitingForAppointment = 49;

		public const short AppointmentExpired = 50;

		public const short AppointmentCancelled = 51;

		public const short GoToRescue = 52;

		public const short RescuePrisoner = 53;

		public const short ReleasePrisoner = 54;

		public const short Disappear = 55;

		public const short GoToRevenge = 56;

		public const short GoToProtect = 57;

		public const short ProtectRelativeOrFriend = 58;

		public const short SectUpgrade = 59;

		public const short CivilianSettlementUpgrade = 60;

		public const short FactionUpgrade = 61;

		public const short StealResourceFailure = 62;

		public const short StealResourceSuccess = 63;

		public const short CheatResourceFailure = 64;

		public const short RobResourceFailure = 65;

		public const short DigResource = 66;

		public const short StealItemFailure = 67;

		public const short StealItemSuccess = 68;

		public const short CheatItemFailure = 69;

		public const short RobItemFailure = 70;

		public const short DigItem = 71;

		public const short StealLifeSkillFailure = 72;

		public const short StealLifeSkillSuccess = 73;

		public const short CheatLifeSkillFailure = 74;

		public const short StealCombatSkillFailure = 75;

		public const short StealCombatSkillSuccess = 76;

		public const short CheatCombatSkillFailure = 77;

		public const short GivePresentResource = 78;

		public const short GivePresentItem = 79;

		public const short TeachLifeSkillSuccess = 80;

		public const short TeachLifeSkillFailure = 81;

		public const short TeachCombatSkillSuccess = 82;

		public const short TeachCombatSkillFailure = 83;

		public const short AmuseOthersByMusic = 84;

		public const short AmuseOthersByChess = 85;

		public const short AmuseOthersByPoem = 86;

		public const short AmuseOthersByPainting = 87;

		public const short MakeFamousItem = 88;

		public const short EnlightenedByDaoism = 89;

		public const short EnlightenedByBuddhism = 90;

		public const short PractiseDivination = 91;

		public const short UnexpectedlyGetRareItem = 92;

		public const short UnexpectedlyGetResource = 93;

		public const short UnexpectedlyGetCombatSkill = 94;

		public const short UnexpectedlyGetLifeSkill = 95;

		public const short UnexpectedlyGetHealth = 96;

		public const short UnexpectedlyHealOuterInjury = 97;

		public const short UnexpectedlyHealInnerInjury = 98;

		public const short UnexpectedlyHealPoison = 99;

		public const short UnexpectedlyHealQi = 100;

		public const short UnexpectedlyLoseRareItem = 101;

		public const short UnexpectedlyLoseResource = 102;

		public const short UnexpectedlyLoseCombatSkill = 103;

		public const short UnexpectedlyLoseLifeSkill = 104;

		public const short UnexpectedlyLoseHealth = 105;

		public const short UnexpectedlySufferOuterInjury = 106;

		public const short UnexpectedlySufferInneInjury = 107;

		public const short UnexpectedlySufferPoison = 108;

		public const short UnexpectedlySufferDisorderOfQi = 109;

		public const short BuildingResourceIncreased = 110;

		public const short BuildingResourceSpread = 111;

		public const short BuildingDamaged = 112;

		public const short BuildingRuined = 113;

		public const short BuildingConstructionCompleted = 114;

		public const short BuildingUpgradingCompleted = 115;

		public const short BuildingDemolitionCompleted = 116;

		public const short BuildingIncome = 117;

		public const short DispatchInPlace = 118;

		public const short FindViciousBeggarsNest = 119;

		public const short FindThievesCamp = 120;

		public const short FindBanditsStronghold = 121;

		public const short FindTraitorsGang = 122;

		public const short FindVillainsValley = 123;

		public const short FindMixiangzhen = 124;

		public const short FindMassGrave = 125;

		public const short FindHereticHome = 126;

		public const short KidnappedByHeresy = 127;

		public const short KidnappedByHeart = 128;

		public const short KidnappedBySoumoulou = 129;

		public const short KidnappedByWorldWeary = 130;

		public const short MarketAppeared = 131;

		public const short TownCombatAppeared = 132;

		public const short CricketsAppeared = 133;

		public const short StartCricketContest = 134;

		public const short LifeCompetitionAppeared = 135;

		public const short StartSectJuniorContest = 136;

		public const short StartSectIntermediateContest = 137;

		public const short StartSectSeniorContest = 138;

		public const short JoustForSpouse = 139;

		public const short MarryNotice = 217;

		public const short XiangshuAvatarAppeared = 140;

		public const short MonvBringDisaster = 141;

		public const short DayueYaochangBringDisaster = 142;

		public const short JiuhanvBringDisaster = 143;

		public const short JinHuangervBringDisaster = 144;

		public const short YiYihouvBringDisaster = 145;

		public const short WeiQivBringDisaster = 146;

		public const short YixiangvBringDisaster = 147;

		public const short XuefengBringDisaster = 148;

		public const short ShuFangvBringDisaster = 149;

		public const short MonvSaveSuffering = 150;

		public const short DayueYaochangSaveSuffering = 151;

		public const short JiuhanvSaveSuffering = 152;

		public const short JinHuangervSaveSuffering = 153;

		public const short YiYihouvSaveSuffering = 154;

		public const short WeiQivSaveSuffering = 155;

		public const short YixiangvSaveSuffering = 156;

		public const short XuefengSaveSuffering = 157;

		public const short ShuFangvSaveSuffering = 158;

		public const short CivilianDisappear = 159;

		public const short MerchantGoTravelling = 160;

		public const short ChickenEscaped = 161;

		public const short NaturalDisasterOccurred = 162;

		public const short Reincarnation = 163;

		public const short AccumulatedSkillPowerLost = 164;

		public const short TaiwuVillageDestructed = 165;

		public const short RebirthAsJuniorXiangshu = 166;

		public const short LegendaryBookAppeared = 167;

		public const short WulinConferenceWithoutParticipant = 168;

		public const short WulinConferenceInPreparing = 169;

		public const short WulinConferenceInProgress = 170;

		public const short XiangshuKilling = 171;

		public const short MonthlyNormalInformation = 172;

		public const short MonthlySecretInformation = 173;

		public const short SecretInformationWillExpire = 174;

		public const short SecretInformationExpired = 175;

		public const short YirenAppearInTaiwuArea = 176;

		public const short WesternMerchantBackAfterLong = 177;

		public const short WesternMerchantLoseContact = 178;

		public const short WesternMerchantBackSucceed = 179;

		public const short GainAuthority = 180;

		public const short FemaleJoustForSpouseReady = 181;

		public const short StartSectNormalCompetition = 182;

		public const short EscapeWithForeverLover = 183;

		public const short DisasterAndPreciousMaterial = 184;

		public const short HeroesDefendMorality = 185;

		public const short IncomeFromNestViciousBeggars = 186;

		public const short IncomeFromNestThievesCamp = 187;

		public const short IncomeFromNestBanditsStronghold = 188;

		public const short IncomeFromNestVillainsValley = 189;

		public const short IncomeFromNestRighteousLow = 190;

		public const short IncomeFromNestRighteousMiddle = 191;

		public const short BuildingWorkerDie = 192;

		public const short StoneHouseInfectedKidnapped = 193;

		public const short WesternMerchanLost = 194;

		public const short WesternMerchanFindMirage = 195;

		public const short WesternMerchanFindBigfoot = 196;

		public const short WesternMerchanFindPlant = 197;

		public const short WesternMerchanFindAnimal = 198;

		public const short WesternMerchanGetInformation = 199;

		public const short WesternMerchanFindSettlement = 200;

		public const short WesternMerchanFindWeather = 201;

		public const short WesternMerchanFindWreckage = 202;

		public const short WesternMerchanHelpPasserby = 203;

		public const short WesternMerchanGetHelp = 204;

		public const short WesternMerchanFindVenison = 205;

		public const short WesternMerchanFindFruit = 206;

		public const short WesternMerchanFindVillage = 207;

		public const short WesternMerchanMeetMerchan = 208;

		public const short WesternMerchanMeetTheif = 209;

		public const short WesternMerchanGoodsDamage = 210;

		public const short WesternMerchanUnacclimatized = 211;

		public const short WesternMerchanLackReplenishment = 212;

		public const short AboutToDie = 213;

		public const short EnemyNestDemise = 214;

		public const short SecretInformationBroadcast = 215;

		public const short ReadingEvent = 216;

		public const short EnemyNestGrow = 219;

		public const short RandomEnemyGrow = 218;

		public const short RandomEnemyDecay = 220;

		public const short XiangshuGetStrengthened = 221;

		public const short LegendaryBookShocked = 222;

		public const short LegendaryBookInsane = 223;

		public const short LegendaryBookConsumed = 224;

		public const short LegendaryBookLost = 225;

		public const short FightForNewLegendaryBook = 226;

		public const short FightForLegendaryBookAbandoned = 227;

		public const short FightForLegendaryBookOwnerDie = 228;

		public const short FightForLegendaryBookOwnerConsumed = 229;

		public const short LegendaryBookAppear = 230;

		public const short ChallengeForLegendaryBook = 231;

		public const short RobLegendaryBook = 232;

		public const short VillagerLeftForLegendaryBook = 233;

		public const short HappyBirthday = 234;

		public const short PoisonMakeLoss = 238;

		public const short RottenPoisonDiffuse = 239;

		public const short PoisonDestroyFace = 240;

		public const short IllusoryPoisonDiffuse = 241;

		public const short PoisonDisturbMindAttckSuccess = 242;

		public const short PoisonDisturbMindEmpoisonSuccess = 243;

		public const short PoisonDisturbMindSneakAttckSuccess = 244;

		public const short PoisonDisturbMindRapeSuccess = 245;

		public const short PoisonDisturbMindAttckFalse = 246;

		public const short PoisonDisturbMindEmpoisonFalse = 247;

		public const short PoisonDisturbMindSneakAttckFalse = 248;

		public const short PoisonDisturbMindRapeFalse = 249;

		public const short SectMainStoryXuehouJixiKillsPeople = 250;

		public const short SectMainStoryYuanshanAbsorbInfectedPeople = 251;

		public const short SectMainStoryShixiangAdventure = 252;

		public const short SectMainStoryXuehouJixiGone = 253;

		public const short WulinConferenceWinner = 254;

		public const short SectMainStoryEmeiInfighting = 255;

		public const short SectMainStoryWhiteGibbonReturns = 256;

		public const short SectMainStoryXuehouJixiGoneAgain = 257;

		public const short SectMainStoryXuehouJixiRescue = 258;

		public const short SectMainStoryXuehouJixiGoneFinal = 259;

		public const short SectMainStoryKongsangTripodVesselCures = 260;

		public const short SectMainStoryKongsangTripodVesselDetoxifies = 261;

		public const short SectMainStoryKongsangTripodVesselRemovesQiDisorder = 262;

		public const short SectMainStoryKongsangTripodVesselRestoresHealth = 263;

		public const short ReincarnationNewWithLocation = 264;

		public const short SectMainStoryWudangVillagersInjured = 265;

		public const short SectMainStoryWudangVillagerCasualty = 266;

		public const short KillHereticRandomEnemy = 267;

		public const short DefeatedByHereticRandomEnemy = 268;

		public const short KillRighteousRandomEnemy = 269;

		public const short DefeatedByRighteousRandomEnemy = 270;

		public const short KillAnimal = 271;

		public const short DefeatedByAnimal = 272;

		public const short DieFromEnemyNest = 273;

		public const short Dummy0 = 235;

		public const short Dummy1 = 236;

		public const short Dummy2 = 237;

		public const short MiscarriageAndReincarnation = 274;

		public const short MiscarriageAndReincarnationMotherDies = 275;

		public const short MiscarriageAndReincarnationMotherKilled = 276;

		public const short SectMainStoryEmeiShiReturns = 277;

		public const short SectMainStoryEmeiDoomOfEmei = 278;

		public const short EscapeFromEnemyNest = 279;

		public const short SavedFromEnemyNest = 280;

		public const short CultureDecline = 281;

		public const short FiveLoongArise = 282;

		public const short JiaoPoolAccident = 283;

		public const short JiaoGoHome = 284;

		public const short JiaoBrokeThroughTheShell = 285;

		public const short JiaoHasReachedAnAdultAge = 286;

		public const short DLCLoongRidingEffectQiuniu = 287;

		public const short DLCLoongRidingEffectYazi = 288;

		public const short DLCLoongRidingEffectChaofeng = 289;

		public const short DLCLoongRidingEffectPulao = 290;

		public const short DLCLoongRidingEffectSuanni = 291;

		public const short DLCLoongRidingEffectBaxia = 292;

		public const short DLCLoongRidingEffectBian = 293;

		public const short DLCLoongRidingEffectFuxi = 294;

		public const short DLCLoongRidingEffectChiwen = 295;

		public const short JiaoLayEggs = 296;

		public const short JiaoTamingPointsLow = 297;

		public const short DieFromAge = 298;

		public const short DieFromPoorHealth = 299;

		public const short KilledInPubilc = 300;

		public const short SectMainStoryJingangHaunted = 301;

		public const short SectMainStoryJingangFollowedByGhost = 302;

		public const short SectMainStoryJingangWrongdoing = 303;

		public const short SectMainStoryJingangPray = 304;

		public const short SectMainStoryJingangFameDistribution = 305;

		public const short WugKingParasitiferDead = 306;

		public const short WugKingDead = 307;

		public const short WugKingDeadSpecial = 308;

		public const short SectMainStoryJingangFamousFakeMonk = 309;

		public const short SectMainStoryJingangRockFleshed = 310;

		public const short SectMainStoryWuxianParanoiaAppeared = 311;

		public const short SectMainStoryJingangVillagerFlee = 312;

		public const short SectMainStoryRanshanSanZongBiWu = 313;

		public const short GiveUpLegendaryBookSuccessHuaJu = 314;

		public const short GiveUpLegendaryBookSuccessXuanZhi = 315;

		public const short GiveUpLegendaryBookSuccessYingJiao = 316;

		public const short GiveUpLegendaryBookFailureHuaJu = 317;

		public const short GiveUpLegendaryBookFailureXuanZhi = 318;

		public const short GiveUpLegendaryBookFailureYingJiao = 319;

		public const short GiveUpLegendaryBookLoseBookHuaJu = 323;

		public const short GiveUpLegendaryBookLoseBookXuanZhi = 324;

		public const short GiveUpLegendaryBookLoseBookYingJiao = 325;

		public const short GiveUpLegendaryBookLoseTargetHuaJu = 320;

		public const short GiveUpLegendaryBookLoseTargetXuanZhi = 321;

		public const short GiveUpLegendaryBookLoseTargetYingJiao = 322;

		public const short LifeLinkHealing = 326;

		public const short LifeLinkDamage = 327;

		public const short SectMainStoryBaihuaLeukoKills = 328;

		public const short SectMainStoryBaihuaMelanoKills = 329;

		public const short SectMainStoryBaihuaLeukoHelps = 330;

		public const short SectMainStoryBaihuaMelanoHelps = 331;

		public const short SectMainStoryBaihuaManicLow = 332;

		public const short SectMainStoryBaihuaManicHigh = 333;

		public const short LoopingEvent = 334;

		public const short FiveElementsChange = 335;

		public const short ResourcesCollectionCompleted = 336;

		public const short SectMainStoryFulongSacrifice = 337;

		public const short SectMainStoryFulongFeatherDrop = 338;

		public const short MarketComing = 339;

		public const short TownCombatComing = 341;

		public const short CricketContestComing = 343;

		public const short LifeCompetitionComing = 342;

		public const short SectNormalCompetitionComing = 340;

		public const short JoustForSpouseComing = 344;

		public const short DyingNotice = 345;

		public const short InjuredNotice = 346;

		public const short TrappedNotice = 347;

		public const short SectMainStoryFulongFightSucceed = 348;

		public const short SectMainStoryFulongFightFail = 349;

		public const short SectMainStoryFulongFamilyFightFail = 350;

		public const short SectMainStoryFulongRobbery = 351;

		public const short SectMainStoryFulongFamilyRobbery = 352;

		public const short DeliverInPrison0 = 353;

		public const short DeliverInPrison1 = 354;

		public const short DieInPrison = 355;

		public const short AssassinatedInPrison = 356;

		public const short AssassinatedDueToKillerTokenInPrison = 357;

		public const short ImprisonAndAbandonBaby0 = 358;

		public const short ImprisonAndAbandonBaby1 = 359;

		public const short ResourceMigration = 360;

		public const short ChickenSecretInformation = 361;

		public const short XiangshuNormalInformation = 362;

		public const short SectMainStoryFulongFireVanishes = 363;

		public const short SectMainStoryFulongLooting = 364;

		public const short SectMainStoryWudangTreesGrow = 365;

		public const short SectMainStoryZhujianSwordTestCeremony = 366;

		public const short InvestedCaravanMove = 367;

		public const short InvestedCaravanPassSettlement = 368;

		public const short InvestedCaravanPassLowCultureSettlement = 369;

		public const short InvestedCaravanPassHighCultureSettlement = 370;

		public const short InvestedCaravanPassLowSafetySettlement = 371;

		public const short InvestedCaravanPassHighSafetySettlement = 372;

		public const short InvestedCaravanPassLowSafetyLowCultureSettlement = 373;

		public const short InvestedCaravanPassLowSafetyHighCultureSettlement = 374;

		public const short InvestedCaravanPassHighSafetyLowCultureSettlement = 375;

		public const short InvestedCaravanPassHighSafetyHighCultureSettlement = 376;

		public const short InvestedCaravanArrive = 377;

		public const short InvestedCaravanIsRobbed = 378;

		public const short InvestedCaravanIsRobbedAndFailed = 379;

		public const short BuildingUpgradingHolded = 380;

		public const short PunishmentLost0 = 381;

		public const short PunishmentLost1 = 382;

		public const short OutsiderMakeHarvest = 383;

		public const short TaiwuVillageCraftObjectsFinished = 384;

		public const short OutsiderMakeHarvest1 = 385;

		public const short TaiwuVillagerDied = 386;

		public const short SectMainStoryRemakeEmeiHomocideCase = 387;

		public const short SectMainStoryRemakeEmeiRumor = 388;

		public const short DieNotice = 389;

		public const short WantedNotice = 390;

		public const short SectMainStoryYuanshanJuemo = 391;

		public const short CoreMaterialIncome = 392;

		public const short FamilyGetInfected = 393;

		public const short FamilyDieByInfected = 394;

		public const short FocusedGetInfected = 395;

		public const short FocusedDieByInfected = 396;

		public const short NormalVillagersInjured = 397;

		public const short NormalVillagerCasualty = 398;

		public const short NormalTreesGrow = 399;

		public const short VillagerCraftFinished0 = 400;

		public const short VillagerCraftFinished1 = 401;

		public const short VillagerCraftFinished2 = 402;

		public const short VillagerCraftFinished3 = 403;

		public const short NpcCraftFinished0 = 404;

		public const short NpcCraftFinished1 = 405;

		public const short NpcCraftFinished2 = 406;

		public const short NpcCraftFinished3 = 407;

		public const short NpcLongDistanceMarriage0 = 408;

		public const short NpcLongDistanceMarriage1 = 409;

		public const short NpcLongDistanceMarriage2 = 410;

		public const short WithoutFood = 411;
	}

	public static class DefValue
	{
		public static MonthlyNotificationItem SolarTerm0 => Instance[(short)0];

		public static MonthlyNotificationItem SolarTerm1 => Instance[(short)1];

		public static MonthlyNotificationItem SolarTerm2 => Instance[(short)2];

		public static MonthlyNotificationItem SolarTerm3 => Instance[(short)3];

		public static MonthlyNotificationItem SolarTerm4 => Instance[(short)4];

		public static MonthlyNotificationItem SolarTerm5 => Instance[(short)5];

		public static MonthlyNotificationItem SolarTerm6 => Instance[(short)6];

		public static MonthlyNotificationItem SolarTerm7 => Instance[(short)7];

		public static MonthlyNotificationItem SolarTerm8 => Instance[(short)8];

		public static MonthlyNotificationItem SolarTerm9 => Instance[(short)9];

		public static MonthlyNotificationItem SolarTerm10 => Instance[(short)10];

		public static MonthlyNotificationItem SolarTerm11 => Instance[(short)11];

		public static MonthlyNotificationItem GraveDestroyed => Instance[(short)12];

		public static MonthlyNotificationItem IncomeFromNest => Instance[(short)13];

		public static MonthlyNotificationItem LoseItemCausedByWarehouseFull => Instance[(short)14];

		public static MonthlyNotificationItem Assassinated => Instance[(short)15];

		public static MonthlyNotificationItem AssassinatedDueToKillerToken => Instance[(short)16];

		public static MonthlyNotificationItem Die => Instance[(short)17];

		public static MonthlyNotificationItem InfectXiangshuPartially => Instance[(short)18];

		public static MonthlyNotificationItem InfectXiangshuCompletely => Instance[(short)19];

		public static MonthlyNotificationItem CreateHatredByPrison => Instance[(short)20];

		public static MonthlyNotificationItem EscapeFromPrison => Instance[(short)21];

		public static MonthlyNotificationItem CricketEndLife => Instance[(short)22];

		public static MonthlyNotificationItem LoseResourceCausedByInventoryFull => Instance[(short)23];

		public static MonthlyNotificationItem LoseItemCausedByInventoryFull => Instance[(short)24];

		public static MonthlyNotificationItem CreateHatred => Instance[(short)25];

		public static MonthlyNotificationItem DecreaseHatred => Instance[(short)26];

		public static MonthlyNotificationItem ConfessLoveAndSucceed => Instance[(short)27];

		public static MonthlyNotificationItem SeverLove => Instance[(short)28];

		public static MonthlyNotificationItem Marriage => Instance[(short)29];

		public static MonthlyNotificationItem BecomeFriend => Instance[(short)30];

		public static MonthlyNotificationItem DecreaseFriendship => Instance[(short)31];

		public static MonthlyNotificationItem BecomeSwornBrotherOrSister => Instance[(short)32];

		public static MonthlyNotificationItem SeverFriendship => Instance[(short)33];

		public static MonthlyNotificationItem AdoptBoy => Instance[(short)34];

		public static MonthlyNotificationItem AdoptGirl => Instance[(short)35];

		public static MonthlyNotificationItem RecognizeFather => Instance[(short)36];

		public static MonthlyNotificationItem RecognizeMother => Instance[(short)37];

		public static MonthlyNotificationItem MakeLove => Instance[(short)38];

		public static MonthlyNotificationItem RapeFailure => Instance[(short)39];

		public static MonthlyNotificationItem MotherGiveBirthToBoy => Instance[(short)40];

		public static MonthlyNotificationItem MotherGiveBirthToGirl => Instance[(short)41];

		public static MonthlyNotificationItem FatherGetBoy => Instance[(short)42];

		public static MonthlyNotificationItem FatherGetGirl => Instance[(short)43];

		public static MonthlyNotificationItem GiveBirthToCricket => Instance[(short)44];

		public static MonthlyNotificationItem MotherLoseFetus => Instance[(short)45];

		public static MonthlyNotificationItem GoToJoinOrganization => Instance[(short)46];

		public static MonthlyNotificationItem JoinOrganization => Instance[(short)47];

		public static MonthlyNotificationItem GoToAppointment => Instance[(short)48];

		public static MonthlyNotificationItem WaitingForAppointment => Instance[(short)49];

		public static MonthlyNotificationItem AppointmentExpired => Instance[(short)50];

		public static MonthlyNotificationItem AppointmentCancelled => Instance[(short)51];

		public static MonthlyNotificationItem GoToRescue => Instance[(short)52];

		public static MonthlyNotificationItem RescuePrisoner => Instance[(short)53];

		public static MonthlyNotificationItem ReleasePrisoner => Instance[(short)54];

		public static MonthlyNotificationItem Disappear => Instance[(short)55];

		public static MonthlyNotificationItem GoToRevenge => Instance[(short)56];

		public static MonthlyNotificationItem GoToProtect => Instance[(short)57];

		public static MonthlyNotificationItem ProtectRelativeOrFriend => Instance[(short)58];

		public static MonthlyNotificationItem SectUpgrade => Instance[(short)59];

		public static MonthlyNotificationItem CivilianSettlementUpgrade => Instance[(short)60];

		public static MonthlyNotificationItem FactionUpgrade => Instance[(short)61];

		public static MonthlyNotificationItem StealResourceFailure => Instance[(short)62];

		public static MonthlyNotificationItem StealResourceSuccess => Instance[(short)63];

		public static MonthlyNotificationItem CheatResourceFailure => Instance[(short)64];

		public static MonthlyNotificationItem RobResourceFailure => Instance[(short)65];

		public static MonthlyNotificationItem DigResource => Instance[(short)66];

		public static MonthlyNotificationItem StealItemFailure => Instance[(short)67];

		public static MonthlyNotificationItem StealItemSuccess => Instance[(short)68];

		public static MonthlyNotificationItem CheatItemFailure => Instance[(short)69];

		public static MonthlyNotificationItem RobItemFailure => Instance[(short)70];

		public static MonthlyNotificationItem DigItem => Instance[(short)71];

		public static MonthlyNotificationItem StealLifeSkillFailure => Instance[(short)72];

		public static MonthlyNotificationItem StealLifeSkillSuccess => Instance[(short)73];

		public static MonthlyNotificationItem CheatLifeSkillFailure => Instance[(short)74];

		public static MonthlyNotificationItem StealCombatSkillFailure => Instance[(short)75];

		public static MonthlyNotificationItem StealCombatSkillSuccess => Instance[(short)76];

		public static MonthlyNotificationItem CheatCombatSkillFailure => Instance[(short)77];

		public static MonthlyNotificationItem GivePresentResource => Instance[(short)78];

		public static MonthlyNotificationItem GivePresentItem => Instance[(short)79];

		public static MonthlyNotificationItem TeachLifeSkillSuccess => Instance[(short)80];

		public static MonthlyNotificationItem TeachLifeSkillFailure => Instance[(short)81];

		public static MonthlyNotificationItem TeachCombatSkillSuccess => Instance[(short)82];

		public static MonthlyNotificationItem TeachCombatSkillFailure => Instance[(short)83];

		public static MonthlyNotificationItem AmuseOthersByMusic => Instance[(short)84];

		public static MonthlyNotificationItem AmuseOthersByChess => Instance[(short)85];

		public static MonthlyNotificationItem AmuseOthersByPoem => Instance[(short)86];

		public static MonthlyNotificationItem AmuseOthersByPainting => Instance[(short)87];

		public static MonthlyNotificationItem MakeFamousItem => Instance[(short)88];

		public static MonthlyNotificationItem EnlightenedByDaoism => Instance[(short)89];

		public static MonthlyNotificationItem EnlightenedByBuddhism => Instance[(short)90];

		public static MonthlyNotificationItem PractiseDivination => Instance[(short)91];

		public static MonthlyNotificationItem UnexpectedlyGetRareItem => Instance[(short)92];

		public static MonthlyNotificationItem UnexpectedlyGetResource => Instance[(short)93];

		public static MonthlyNotificationItem UnexpectedlyGetCombatSkill => Instance[(short)94];

		public static MonthlyNotificationItem UnexpectedlyGetLifeSkill => Instance[(short)95];

		public static MonthlyNotificationItem UnexpectedlyGetHealth => Instance[(short)96];

		public static MonthlyNotificationItem UnexpectedlyHealOuterInjury => Instance[(short)97];

		public static MonthlyNotificationItem UnexpectedlyHealInnerInjury => Instance[(short)98];

		public static MonthlyNotificationItem UnexpectedlyHealPoison => Instance[(short)99];

		public static MonthlyNotificationItem UnexpectedlyHealQi => Instance[(short)100];

		public static MonthlyNotificationItem UnexpectedlyLoseRareItem => Instance[(short)101];

		public static MonthlyNotificationItem UnexpectedlyLoseResource => Instance[(short)102];

		public static MonthlyNotificationItem UnexpectedlyLoseCombatSkill => Instance[(short)103];

		public static MonthlyNotificationItem UnexpectedlyLoseLifeSkill => Instance[(short)104];

		public static MonthlyNotificationItem UnexpectedlyLoseHealth => Instance[(short)105];

		public static MonthlyNotificationItem UnexpectedlySufferOuterInjury => Instance[(short)106];

		public static MonthlyNotificationItem UnexpectedlySufferInneInjury => Instance[(short)107];

		public static MonthlyNotificationItem UnexpectedlySufferPoison => Instance[(short)108];

		public static MonthlyNotificationItem UnexpectedlySufferDisorderOfQi => Instance[(short)109];

		public static MonthlyNotificationItem BuildingResourceIncreased => Instance[(short)110];

		public static MonthlyNotificationItem BuildingResourceSpread => Instance[(short)111];

		public static MonthlyNotificationItem BuildingDamaged => Instance[(short)112];

		public static MonthlyNotificationItem BuildingRuined => Instance[(short)113];

		public static MonthlyNotificationItem BuildingConstructionCompleted => Instance[(short)114];

		public static MonthlyNotificationItem BuildingUpgradingCompleted => Instance[(short)115];

		public static MonthlyNotificationItem BuildingDemolitionCompleted => Instance[(short)116];

		public static MonthlyNotificationItem BuildingIncome => Instance[(short)117];

		public static MonthlyNotificationItem DispatchInPlace => Instance[(short)118];

		public static MonthlyNotificationItem FindViciousBeggarsNest => Instance[(short)119];

		public static MonthlyNotificationItem FindThievesCamp => Instance[(short)120];

		public static MonthlyNotificationItem FindBanditsStronghold => Instance[(short)121];

		public static MonthlyNotificationItem FindTraitorsGang => Instance[(short)122];

		public static MonthlyNotificationItem FindVillainsValley => Instance[(short)123];

		public static MonthlyNotificationItem FindMixiangzhen => Instance[(short)124];

		public static MonthlyNotificationItem FindMassGrave => Instance[(short)125];

		public static MonthlyNotificationItem FindHereticHome => Instance[(short)126];

		public static MonthlyNotificationItem KidnappedByHeresy => Instance[(short)127];

		public static MonthlyNotificationItem KidnappedByHeart => Instance[(short)128];

		public static MonthlyNotificationItem KidnappedBySoumoulou => Instance[(short)129];

		public static MonthlyNotificationItem KidnappedByWorldWeary => Instance[(short)130];

		public static MonthlyNotificationItem MarketAppeared => Instance[(short)131];

		public static MonthlyNotificationItem TownCombatAppeared => Instance[(short)132];

		public static MonthlyNotificationItem CricketsAppeared => Instance[(short)133];

		public static MonthlyNotificationItem StartCricketContest => Instance[(short)134];

		public static MonthlyNotificationItem LifeCompetitionAppeared => Instance[(short)135];

		public static MonthlyNotificationItem StartSectJuniorContest => Instance[(short)136];

		public static MonthlyNotificationItem StartSectIntermediateContest => Instance[(short)137];

		public static MonthlyNotificationItem StartSectSeniorContest => Instance[(short)138];

		public static MonthlyNotificationItem JoustForSpouse => Instance[(short)139];

		public static MonthlyNotificationItem MarryNotice => Instance[(short)217];

		public static MonthlyNotificationItem XiangshuAvatarAppeared => Instance[(short)140];

		public static MonthlyNotificationItem MonvBringDisaster => Instance[(short)141];

		public static MonthlyNotificationItem DayueYaochangBringDisaster => Instance[(short)142];

		public static MonthlyNotificationItem JiuhanvBringDisaster => Instance[(short)143];

		public static MonthlyNotificationItem JinHuangervBringDisaster => Instance[(short)144];

		public static MonthlyNotificationItem YiYihouvBringDisaster => Instance[(short)145];

		public static MonthlyNotificationItem WeiQivBringDisaster => Instance[(short)146];

		public static MonthlyNotificationItem YixiangvBringDisaster => Instance[(short)147];

		public static MonthlyNotificationItem XuefengBringDisaster => Instance[(short)148];

		public static MonthlyNotificationItem ShuFangvBringDisaster => Instance[(short)149];

		public static MonthlyNotificationItem MonvSaveSuffering => Instance[(short)150];

		public static MonthlyNotificationItem DayueYaochangSaveSuffering => Instance[(short)151];

		public static MonthlyNotificationItem JiuhanvSaveSuffering => Instance[(short)152];

		public static MonthlyNotificationItem JinHuangervSaveSuffering => Instance[(short)153];

		public static MonthlyNotificationItem YiYihouvSaveSuffering => Instance[(short)154];

		public static MonthlyNotificationItem WeiQivSaveSuffering => Instance[(short)155];

		public static MonthlyNotificationItem YixiangvSaveSuffering => Instance[(short)156];

		public static MonthlyNotificationItem XuefengSaveSuffering => Instance[(short)157];

		public static MonthlyNotificationItem ShuFangvSaveSuffering => Instance[(short)158];

		public static MonthlyNotificationItem CivilianDisappear => Instance[(short)159];

		public static MonthlyNotificationItem MerchantGoTravelling => Instance[(short)160];

		public static MonthlyNotificationItem ChickenEscaped => Instance[(short)161];

		public static MonthlyNotificationItem NaturalDisasterOccurred => Instance[(short)162];

		public static MonthlyNotificationItem Reincarnation => Instance[(short)163];

		public static MonthlyNotificationItem AccumulatedSkillPowerLost => Instance[(short)164];

		public static MonthlyNotificationItem TaiwuVillageDestructed => Instance[(short)165];

		public static MonthlyNotificationItem RebirthAsJuniorXiangshu => Instance[(short)166];

		public static MonthlyNotificationItem LegendaryBookAppeared => Instance[(short)167];

		public static MonthlyNotificationItem WulinConferenceWithoutParticipant => Instance[(short)168];

		public static MonthlyNotificationItem WulinConferenceInPreparing => Instance[(short)169];

		public static MonthlyNotificationItem WulinConferenceInProgress => Instance[(short)170];

		public static MonthlyNotificationItem XiangshuKilling => Instance[(short)171];

		public static MonthlyNotificationItem MonthlyNormalInformation => Instance[(short)172];

		public static MonthlyNotificationItem MonthlySecretInformation => Instance[(short)173];

		public static MonthlyNotificationItem SecretInformationWillExpire => Instance[(short)174];

		public static MonthlyNotificationItem SecretInformationExpired => Instance[(short)175];

		public static MonthlyNotificationItem YirenAppearInTaiwuArea => Instance[(short)176];

		public static MonthlyNotificationItem WesternMerchantBackAfterLong => Instance[(short)177];

		public static MonthlyNotificationItem WesternMerchantLoseContact => Instance[(short)178];

		public static MonthlyNotificationItem WesternMerchantBackSucceed => Instance[(short)179];

		public static MonthlyNotificationItem GainAuthority => Instance[(short)180];

		public static MonthlyNotificationItem FemaleJoustForSpouseReady => Instance[(short)181];

		public static MonthlyNotificationItem StartSectNormalCompetition => Instance[(short)182];

		public static MonthlyNotificationItem EscapeWithForeverLover => Instance[(short)183];

		public static MonthlyNotificationItem DisasterAndPreciousMaterial => Instance[(short)184];

		public static MonthlyNotificationItem HeroesDefendMorality => Instance[(short)185];

		public static MonthlyNotificationItem IncomeFromNestViciousBeggars => Instance[(short)186];

		public static MonthlyNotificationItem IncomeFromNestThievesCamp => Instance[(short)187];

		public static MonthlyNotificationItem IncomeFromNestBanditsStronghold => Instance[(short)188];

		public static MonthlyNotificationItem IncomeFromNestVillainsValley => Instance[(short)189];

		public static MonthlyNotificationItem IncomeFromNestRighteousLow => Instance[(short)190];

		public static MonthlyNotificationItem IncomeFromNestRighteousMiddle => Instance[(short)191];

		public static MonthlyNotificationItem BuildingWorkerDie => Instance[(short)192];

		public static MonthlyNotificationItem StoneHouseInfectedKidnapped => Instance[(short)193];

		public static MonthlyNotificationItem WesternMerchanLost => Instance[(short)194];

		public static MonthlyNotificationItem WesternMerchanFindMirage => Instance[(short)195];

		public static MonthlyNotificationItem WesternMerchanFindBigfoot => Instance[(short)196];

		public static MonthlyNotificationItem WesternMerchanFindPlant => Instance[(short)197];

		public static MonthlyNotificationItem WesternMerchanFindAnimal => Instance[(short)198];

		public static MonthlyNotificationItem WesternMerchanGetInformation => Instance[(short)199];

		public static MonthlyNotificationItem WesternMerchanFindSettlement => Instance[(short)200];

		public static MonthlyNotificationItem WesternMerchanFindWeather => Instance[(short)201];

		public static MonthlyNotificationItem WesternMerchanFindWreckage => Instance[(short)202];

		public static MonthlyNotificationItem WesternMerchanHelpPasserby => Instance[(short)203];

		public static MonthlyNotificationItem WesternMerchanGetHelp => Instance[(short)204];

		public static MonthlyNotificationItem WesternMerchanFindVenison => Instance[(short)205];

		public static MonthlyNotificationItem WesternMerchanFindFruit => Instance[(short)206];

		public static MonthlyNotificationItem WesternMerchanFindVillage => Instance[(short)207];

		public static MonthlyNotificationItem WesternMerchanMeetMerchan => Instance[(short)208];

		public static MonthlyNotificationItem WesternMerchanMeetTheif => Instance[(short)209];

		public static MonthlyNotificationItem WesternMerchanGoodsDamage => Instance[(short)210];

		public static MonthlyNotificationItem WesternMerchanUnacclimatized => Instance[(short)211];

		public static MonthlyNotificationItem WesternMerchanLackReplenishment => Instance[(short)212];

		public static MonthlyNotificationItem AboutToDie => Instance[(short)213];

		public static MonthlyNotificationItem EnemyNestDemise => Instance[(short)214];

		public static MonthlyNotificationItem SecretInformationBroadcast => Instance[(short)215];

		public static MonthlyNotificationItem ReadingEvent => Instance[(short)216];

		public static MonthlyNotificationItem EnemyNestGrow => Instance[(short)219];

		public static MonthlyNotificationItem RandomEnemyGrow => Instance[(short)218];

		public static MonthlyNotificationItem RandomEnemyDecay => Instance[(short)220];

		public static MonthlyNotificationItem XiangshuGetStrengthened => Instance[(short)221];

		public static MonthlyNotificationItem LegendaryBookShocked => Instance[(short)222];

		public static MonthlyNotificationItem LegendaryBookInsane => Instance[(short)223];

		public static MonthlyNotificationItem LegendaryBookConsumed => Instance[(short)224];

		public static MonthlyNotificationItem LegendaryBookLost => Instance[(short)225];

		public static MonthlyNotificationItem FightForNewLegendaryBook => Instance[(short)226];

		public static MonthlyNotificationItem FightForLegendaryBookAbandoned => Instance[(short)227];

		public static MonthlyNotificationItem FightForLegendaryBookOwnerDie => Instance[(short)228];

		public static MonthlyNotificationItem FightForLegendaryBookOwnerConsumed => Instance[(short)229];

		public static MonthlyNotificationItem LegendaryBookAppear => Instance[(short)230];

		public static MonthlyNotificationItem ChallengeForLegendaryBook => Instance[(short)231];

		public static MonthlyNotificationItem RobLegendaryBook => Instance[(short)232];

		public static MonthlyNotificationItem VillagerLeftForLegendaryBook => Instance[(short)233];

		public static MonthlyNotificationItem HappyBirthday => Instance[(short)234];

		public static MonthlyNotificationItem PoisonMakeLoss => Instance[(short)238];

		public static MonthlyNotificationItem RottenPoisonDiffuse => Instance[(short)239];

		public static MonthlyNotificationItem PoisonDestroyFace => Instance[(short)240];

		public static MonthlyNotificationItem IllusoryPoisonDiffuse => Instance[(short)241];

		public static MonthlyNotificationItem PoisonDisturbMindAttckSuccess => Instance[(short)242];

		public static MonthlyNotificationItem PoisonDisturbMindEmpoisonSuccess => Instance[(short)243];

		public static MonthlyNotificationItem PoisonDisturbMindSneakAttckSuccess => Instance[(short)244];

		public static MonthlyNotificationItem PoisonDisturbMindRapeSuccess => Instance[(short)245];

		public static MonthlyNotificationItem PoisonDisturbMindAttckFalse => Instance[(short)246];

		public static MonthlyNotificationItem PoisonDisturbMindEmpoisonFalse => Instance[(short)247];

		public static MonthlyNotificationItem PoisonDisturbMindSneakAttckFalse => Instance[(short)248];

		public static MonthlyNotificationItem PoisonDisturbMindRapeFalse => Instance[(short)249];

		public static MonthlyNotificationItem SectMainStoryXuehouJixiKillsPeople => Instance[(short)250];

		public static MonthlyNotificationItem SectMainStoryYuanshanAbsorbInfectedPeople => Instance[(short)251];

		public static MonthlyNotificationItem SectMainStoryShixiangAdventure => Instance[(short)252];

		public static MonthlyNotificationItem SectMainStoryXuehouJixiGone => Instance[(short)253];

		public static MonthlyNotificationItem WulinConferenceWinner => Instance[(short)254];

		public static MonthlyNotificationItem SectMainStoryEmeiInfighting => Instance[(short)255];

		public static MonthlyNotificationItem SectMainStoryWhiteGibbonReturns => Instance[(short)256];

		public static MonthlyNotificationItem SectMainStoryXuehouJixiGoneAgain => Instance[(short)257];

		public static MonthlyNotificationItem SectMainStoryXuehouJixiRescue => Instance[(short)258];

		public static MonthlyNotificationItem SectMainStoryXuehouJixiGoneFinal => Instance[(short)259];

		public static MonthlyNotificationItem SectMainStoryKongsangTripodVesselCures => Instance[(short)260];

		public static MonthlyNotificationItem SectMainStoryKongsangTripodVesselDetoxifies => Instance[(short)261];

		public static MonthlyNotificationItem SectMainStoryKongsangTripodVesselRemovesQiDisorder => Instance[(short)262];

		public static MonthlyNotificationItem SectMainStoryKongsangTripodVesselRestoresHealth => Instance[(short)263];

		public static MonthlyNotificationItem ReincarnationNewWithLocation => Instance[(short)264];

		public static MonthlyNotificationItem SectMainStoryWudangVillagersInjured => Instance[(short)265];

		public static MonthlyNotificationItem SectMainStoryWudangVillagerCasualty => Instance[(short)266];

		public static MonthlyNotificationItem KillHereticRandomEnemy => Instance[(short)267];

		public static MonthlyNotificationItem DefeatedByHereticRandomEnemy => Instance[(short)268];

		public static MonthlyNotificationItem KillRighteousRandomEnemy => Instance[(short)269];

		public static MonthlyNotificationItem DefeatedByRighteousRandomEnemy => Instance[(short)270];

		public static MonthlyNotificationItem KillAnimal => Instance[(short)271];

		public static MonthlyNotificationItem DefeatedByAnimal => Instance[(short)272];

		public static MonthlyNotificationItem DieFromEnemyNest => Instance[(short)273];

		public static MonthlyNotificationItem Dummy0 => Instance[(short)235];

		public static MonthlyNotificationItem Dummy1 => Instance[(short)236];

		public static MonthlyNotificationItem Dummy2 => Instance[(short)237];

		public static MonthlyNotificationItem MiscarriageAndReincarnation => Instance[(short)274];

		public static MonthlyNotificationItem MiscarriageAndReincarnationMotherDies => Instance[(short)275];

		public static MonthlyNotificationItem MiscarriageAndReincarnationMotherKilled => Instance[(short)276];

		public static MonthlyNotificationItem SectMainStoryEmeiShiReturns => Instance[(short)277];

		public static MonthlyNotificationItem SectMainStoryEmeiDoomOfEmei => Instance[(short)278];

		public static MonthlyNotificationItem EscapeFromEnemyNest => Instance[(short)279];

		public static MonthlyNotificationItem SavedFromEnemyNest => Instance[(short)280];

		public static MonthlyNotificationItem CultureDecline => Instance[(short)281];

		public static MonthlyNotificationItem FiveLoongArise => Instance[(short)282];

		public static MonthlyNotificationItem JiaoPoolAccident => Instance[(short)283];

		public static MonthlyNotificationItem JiaoGoHome => Instance[(short)284];

		public static MonthlyNotificationItem JiaoBrokeThroughTheShell => Instance[(short)285];

		public static MonthlyNotificationItem JiaoHasReachedAnAdultAge => Instance[(short)286];

		public static MonthlyNotificationItem DLCLoongRidingEffectQiuniu => Instance[(short)287];

		public static MonthlyNotificationItem DLCLoongRidingEffectYazi => Instance[(short)288];

		public static MonthlyNotificationItem DLCLoongRidingEffectChaofeng => Instance[(short)289];

		public static MonthlyNotificationItem DLCLoongRidingEffectPulao => Instance[(short)290];

		public static MonthlyNotificationItem DLCLoongRidingEffectSuanni => Instance[(short)291];

		public static MonthlyNotificationItem DLCLoongRidingEffectBaxia => Instance[(short)292];

		public static MonthlyNotificationItem DLCLoongRidingEffectBian => Instance[(short)293];

		public static MonthlyNotificationItem DLCLoongRidingEffectFuxi => Instance[(short)294];

		public static MonthlyNotificationItem DLCLoongRidingEffectChiwen => Instance[(short)295];

		public static MonthlyNotificationItem JiaoLayEggs => Instance[(short)296];

		public static MonthlyNotificationItem JiaoTamingPointsLow => Instance[(short)297];

		public static MonthlyNotificationItem DieFromAge => Instance[(short)298];

		public static MonthlyNotificationItem DieFromPoorHealth => Instance[(short)299];

		public static MonthlyNotificationItem KilledInPubilc => Instance[(short)300];

		public static MonthlyNotificationItem SectMainStoryJingangHaunted => Instance[(short)301];

		public static MonthlyNotificationItem SectMainStoryJingangFollowedByGhost => Instance[(short)302];

		public static MonthlyNotificationItem SectMainStoryJingangWrongdoing => Instance[(short)303];

		public static MonthlyNotificationItem SectMainStoryJingangPray => Instance[(short)304];

		public static MonthlyNotificationItem SectMainStoryJingangFameDistribution => Instance[(short)305];

		public static MonthlyNotificationItem WugKingParasitiferDead => Instance[(short)306];

		public static MonthlyNotificationItem WugKingDead => Instance[(short)307];

		public static MonthlyNotificationItem WugKingDeadSpecial => Instance[(short)308];

		public static MonthlyNotificationItem SectMainStoryJingangFamousFakeMonk => Instance[(short)309];

		public static MonthlyNotificationItem SectMainStoryJingangRockFleshed => Instance[(short)310];

		public static MonthlyNotificationItem SectMainStoryWuxianParanoiaAppeared => Instance[(short)311];

		public static MonthlyNotificationItem SectMainStoryJingangVillagerFlee => Instance[(short)312];

		public static MonthlyNotificationItem SectMainStoryRanshanSanZongBiWu => Instance[(short)313];

		public static MonthlyNotificationItem GiveUpLegendaryBookSuccessHuaJu => Instance[(short)314];

		public static MonthlyNotificationItem GiveUpLegendaryBookSuccessXuanZhi => Instance[(short)315];

		public static MonthlyNotificationItem GiveUpLegendaryBookSuccessYingJiao => Instance[(short)316];

		public static MonthlyNotificationItem GiveUpLegendaryBookFailureHuaJu => Instance[(short)317];

		public static MonthlyNotificationItem GiveUpLegendaryBookFailureXuanZhi => Instance[(short)318];

		public static MonthlyNotificationItem GiveUpLegendaryBookFailureYingJiao => Instance[(short)319];

		public static MonthlyNotificationItem GiveUpLegendaryBookLoseBookHuaJu => Instance[(short)323];

		public static MonthlyNotificationItem GiveUpLegendaryBookLoseBookXuanZhi => Instance[(short)324];

		public static MonthlyNotificationItem GiveUpLegendaryBookLoseBookYingJiao => Instance[(short)325];

		public static MonthlyNotificationItem GiveUpLegendaryBookLoseTargetHuaJu => Instance[(short)320];

		public static MonthlyNotificationItem GiveUpLegendaryBookLoseTargetXuanZhi => Instance[(short)321];

		public static MonthlyNotificationItem GiveUpLegendaryBookLoseTargetYingJiao => Instance[(short)322];

		public static MonthlyNotificationItem LifeLinkHealing => Instance[(short)326];

		public static MonthlyNotificationItem LifeLinkDamage => Instance[(short)327];

		public static MonthlyNotificationItem SectMainStoryBaihuaLeukoKills => Instance[(short)328];

		public static MonthlyNotificationItem SectMainStoryBaihuaMelanoKills => Instance[(short)329];

		public static MonthlyNotificationItem SectMainStoryBaihuaLeukoHelps => Instance[(short)330];

		public static MonthlyNotificationItem SectMainStoryBaihuaMelanoHelps => Instance[(short)331];

		public static MonthlyNotificationItem SectMainStoryBaihuaManicLow => Instance[(short)332];

		public static MonthlyNotificationItem SectMainStoryBaihuaManicHigh => Instance[(short)333];

		public static MonthlyNotificationItem LoopingEvent => Instance[(short)334];

		public static MonthlyNotificationItem FiveElementsChange => Instance[(short)335];

		public static MonthlyNotificationItem ResourcesCollectionCompleted => Instance[(short)336];

		public static MonthlyNotificationItem SectMainStoryFulongSacrifice => Instance[(short)337];

		public static MonthlyNotificationItem SectMainStoryFulongFeatherDrop => Instance[(short)338];

		public static MonthlyNotificationItem MarketComing => Instance[(short)339];

		public static MonthlyNotificationItem TownCombatComing => Instance[(short)341];

		public static MonthlyNotificationItem CricketContestComing => Instance[(short)343];

		public static MonthlyNotificationItem LifeCompetitionComing => Instance[(short)342];

		public static MonthlyNotificationItem SectNormalCompetitionComing => Instance[(short)340];

		public static MonthlyNotificationItem JoustForSpouseComing => Instance[(short)344];

		public static MonthlyNotificationItem DyingNotice => Instance[(short)345];

		public static MonthlyNotificationItem InjuredNotice => Instance[(short)346];

		public static MonthlyNotificationItem TrappedNotice => Instance[(short)347];

		public static MonthlyNotificationItem SectMainStoryFulongFightSucceed => Instance[(short)348];

		public static MonthlyNotificationItem SectMainStoryFulongFightFail => Instance[(short)349];

		public static MonthlyNotificationItem SectMainStoryFulongFamilyFightFail => Instance[(short)350];

		public static MonthlyNotificationItem SectMainStoryFulongRobbery => Instance[(short)351];

		public static MonthlyNotificationItem SectMainStoryFulongFamilyRobbery => Instance[(short)352];

		public static MonthlyNotificationItem DeliverInPrison0 => Instance[(short)353];

		public static MonthlyNotificationItem DeliverInPrison1 => Instance[(short)354];

		public static MonthlyNotificationItem DieInPrison => Instance[(short)355];

		public static MonthlyNotificationItem AssassinatedInPrison => Instance[(short)356];

		public static MonthlyNotificationItem AssassinatedDueToKillerTokenInPrison => Instance[(short)357];

		public static MonthlyNotificationItem ImprisonAndAbandonBaby0 => Instance[(short)358];

		public static MonthlyNotificationItem ImprisonAndAbandonBaby1 => Instance[(short)359];

		public static MonthlyNotificationItem ResourceMigration => Instance[(short)360];

		public static MonthlyNotificationItem ChickenSecretInformation => Instance[(short)361];

		public static MonthlyNotificationItem XiangshuNormalInformation => Instance[(short)362];

		public static MonthlyNotificationItem SectMainStoryFulongFireVanishes => Instance[(short)363];

		public static MonthlyNotificationItem SectMainStoryFulongLooting => Instance[(short)364];

		public static MonthlyNotificationItem SectMainStoryWudangTreesGrow => Instance[(short)365];

		public static MonthlyNotificationItem SectMainStoryZhujianSwordTestCeremony => Instance[(short)366];

		public static MonthlyNotificationItem InvestedCaravanMove => Instance[(short)367];

		public static MonthlyNotificationItem InvestedCaravanPassSettlement => Instance[(short)368];

		public static MonthlyNotificationItem InvestedCaravanPassLowCultureSettlement => Instance[(short)369];

		public static MonthlyNotificationItem InvestedCaravanPassHighCultureSettlement => Instance[(short)370];

		public static MonthlyNotificationItem InvestedCaravanPassLowSafetySettlement => Instance[(short)371];

		public static MonthlyNotificationItem InvestedCaravanPassHighSafetySettlement => Instance[(short)372];

		public static MonthlyNotificationItem InvestedCaravanPassLowSafetyLowCultureSettlement => Instance[(short)373];

		public static MonthlyNotificationItem InvestedCaravanPassLowSafetyHighCultureSettlement => Instance[(short)374];

		public static MonthlyNotificationItem InvestedCaravanPassHighSafetyLowCultureSettlement => Instance[(short)375];

		public static MonthlyNotificationItem InvestedCaravanPassHighSafetyHighCultureSettlement => Instance[(short)376];

		public static MonthlyNotificationItem InvestedCaravanArrive => Instance[(short)377];

		public static MonthlyNotificationItem InvestedCaravanIsRobbed => Instance[(short)378];

		public static MonthlyNotificationItem InvestedCaravanIsRobbedAndFailed => Instance[(short)379];

		public static MonthlyNotificationItem BuildingUpgradingHolded => Instance[(short)380];

		public static MonthlyNotificationItem PunishmentLost0 => Instance[(short)381];

		public static MonthlyNotificationItem PunishmentLost1 => Instance[(short)382];

		public static MonthlyNotificationItem OutsiderMakeHarvest => Instance[(short)383];

		public static MonthlyNotificationItem TaiwuVillageCraftObjectsFinished => Instance[(short)384];

		public static MonthlyNotificationItem OutsiderMakeHarvest1 => Instance[(short)385];

		public static MonthlyNotificationItem TaiwuVillagerDied => Instance[(short)386];

		public static MonthlyNotificationItem SectMainStoryRemakeEmeiHomocideCase => Instance[(short)387];

		public static MonthlyNotificationItem SectMainStoryRemakeEmeiRumor => Instance[(short)388];

		public static MonthlyNotificationItem DieNotice => Instance[(short)389];

		public static MonthlyNotificationItem WantedNotice => Instance[(short)390];

		public static MonthlyNotificationItem SectMainStoryYuanshanJuemo => Instance[(short)391];

		public static MonthlyNotificationItem CoreMaterialIncome => Instance[(short)392];

		public static MonthlyNotificationItem FamilyGetInfected => Instance[(short)393];

		public static MonthlyNotificationItem FamilyDieByInfected => Instance[(short)394];

		public static MonthlyNotificationItem FocusedGetInfected => Instance[(short)395];

		public static MonthlyNotificationItem FocusedDieByInfected => Instance[(short)396];

		public static MonthlyNotificationItem NormalVillagersInjured => Instance[(short)397];

		public static MonthlyNotificationItem NormalVillagerCasualty => Instance[(short)398];

		public static MonthlyNotificationItem NormalTreesGrow => Instance[(short)399];

		public static MonthlyNotificationItem VillagerCraftFinished0 => Instance[(short)400];

		public static MonthlyNotificationItem VillagerCraftFinished1 => Instance[(short)401];

		public static MonthlyNotificationItem VillagerCraftFinished2 => Instance[(short)402];

		public static MonthlyNotificationItem VillagerCraftFinished3 => Instance[(short)403];

		public static MonthlyNotificationItem NpcCraftFinished0 => Instance[(short)404];

		public static MonthlyNotificationItem NpcCraftFinished1 => Instance[(short)405];

		public static MonthlyNotificationItem NpcCraftFinished2 => Instance[(short)406];

		public static MonthlyNotificationItem NpcCraftFinished3 => Instance[(short)407];

		public static MonthlyNotificationItem NpcLongDistanceMarriage0 => Instance[(short)408];

		public static MonthlyNotificationItem NpcLongDistanceMarriage1 => Instance[(short)409];

		public static MonthlyNotificationItem NpcLongDistanceMarriage2 => Instance[(short)410];

		public static MonthlyNotificationItem WithoutFood => Instance[(short)411];
	}

	public static MonthlyNotification Instance = new MonthlyNotification();

	private readonly HashSet<string> RequiredFields = new HashSet<string> { "SortingGroup", "TemplateId", "Name", "Icon", "Desc", "MergeableParameters", "ValueCheckParameters", "MergeDesc" };

	internal override int ToInt(short value)
	{
		return value;
	}

	internal override short ToTemplateId(int value)
	{
		return (short)value;
	}

	private void CreateItems0()
	{
		_dataArray.Add(new MonthlyNotificationItem(0, 0, "sp_monthnotify_0_0", 1, new string[6] { "", "", "", "", "", "" }, null, EMonthlyNotificationSectionType.Biography, 0, 0, null, -1, 2, 0));
		_dataArray.Add(new MonthlyNotificationItem(1, 3, "sp_monthnotify_0_1", 4, new string[6] { "", "", "", "", "", "" }, null, EMonthlyNotificationSectionType.Biography, 0, 0, null, -1, 5, 0));
		_dataArray.Add(new MonthlyNotificationItem(2, 6, "sp_monthnotify_0_2", 7, new string[6] { "", "", "", "", "", "" }, null, EMonthlyNotificationSectionType.Biography, 0, 0, null, -1, 8, 0));
		_dataArray.Add(new MonthlyNotificationItem(3, 9, "sp_monthnotify_0_3", 10, new string[6] { "", "", "", "", "", "" }, null, EMonthlyNotificationSectionType.Biography, 0, 0, null, -1, 11, 0));
		_dataArray.Add(new MonthlyNotificationItem(4, 12, "sp_monthnotify_0_4", 13, new string[6] { "", "", "", "", "", "" }, null, EMonthlyNotificationSectionType.Biography, 0, 0, null, -1, 14, 0));
		_dataArray.Add(new MonthlyNotificationItem(5, 15, "sp_monthnotify_0_5", 16, new string[6] { "", "", "", "", "", "" }, null, EMonthlyNotificationSectionType.Biography, 0, 0, null, -1, 17, 0));
		_dataArray.Add(new MonthlyNotificationItem(6, 18, "sp_monthnotify_0_6", 19, new string[6] { "", "", "", "", "", "" }, null, EMonthlyNotificationSectionType.Biography, 0, 0, null, -1, 20, 0));
		_dataArray.Add(new MonthlyNotificationItem(7, 21, "sp_monthnotify_0_7", 22, new string[6] { "", "", "", "", "", "" }, null, EMonthlyNotificationSectionType.Biography, 0, 0, null, -1, 23, 0));
		_dataArray.Add(new MonthlyNotificationItem(8, 24, "sp_monthnotify_0_8", 25, new string[6] { "", "", "", "", "", "" }, null, EMonthlyNotificationSectionType.Biography, 0, 0, null, -1, 26, 0));
		_dataArray.Add(new MonthlyNotificationItem(9, 27, "sp_monthnotify_0_9", 28, new string[6] { "", "", "", "", "", "" }, null, EMonthlyNotificationSectionType.Biography, 0, 0, null, -1, 29, 0));
		_dataArray.Add(new MonthlyNotificationItem(10, 30, "sp_monthnotify_0_10", 31, new string[6] { "", "", "", "", "", "" }, null, EMonthlyNotificationSectionType.Biography, 0, 0, null, -1, 32, 0));
		_dataArray.Add(new MonthlyNotificationItem(11, 33, "sp_monthnotify_0_11", 34, new string[6] { "", "", "", "", "", "" }, null, EMonthlyNotificationSectionType.Biography, 0, 0, null, -1, 35, 0));
		_dataArray.Add(new MonthlyNotificationItem(12, 36, "sp_monthnotify_1_0", 37, new string[6] { "Character", "", "", "", "", "" }, null, EMonthlyNotificationSectionType.Gossip, 1, 67, null, -1, 38, 0));
		_dataArray.Add(new MonthlyNotificationItem(13, 39, "sp_monthnotify_1_1", 40, new string[6] { "Character", "Location", "Character", "Adventure", "", "" }, null, EMonthlyNotificationSectionType.TaiwuVillage, 1, 145, null, -1, 41, 0));
		_dataArray.Add(new MonthlyNotificationItem(14, 42, "sp_monthnotify_1_2", 43, new string[6] { "Item", "", "", "", "", "" }, new List<sbyte> { 0 }, EMonthlyNotificationSectionType.Biography, 0, 7, null, -1, 44, 0));
		_dataArray.Add(new MonthlyNotificationItem(15, 45, "sp_monthnotify_2_0", 46, new string[6] { "Character", "Location", "", "", "", "" }, null, EMonthlyNotificationSectionType.Worldwide, 2, 3, null, -1, 47, 0));
		_dataArray.Add(new MonthlyNotificationItem(16, 48, "sp_monthnotify_2_1", 49, new string[6] { "Character", "Location", "", "", "", "" }, null, EMonthlyNotificationSectionType.Worldwide, 2, 4, null, -1, 50, 0));
		_dataArray.Add(new MonthlyNotificationItem(17, 51, "sp_monthnotify_2_2", 52, new string[6] { "Character", "Location", "", "", "", "" }, null, EMonthlyNotificationSectionType.Gossip, 2, 64, null, -1, 53, 0));
		_dataArray.Add(new MonthlyNotificationItem(18, 54, "sp_monthnotify_2_3", 55, new string[6] { "Character", "Location", "", "", "", "" }, null, EMonthlyNotificationSectionType.Worldwide, 2, 70, null, -1, 56, 0));
		_dataArray.Add(new MonthlyNotificationItem(19, 57, "sp_monthnotify_2_4", 58, new string[6] { "Character", "Location", "", "", "", "" }, null, EMonthlyNotificationSectionType.Worldwide, 2, 71, null, -1, 59, 0));
		_dataArray.Add(new MonthlyNotificationItem(20, 60, "sp_monthnotify_3_0", 61, new string[6] { "Character", "Location", "Character", "", "", "" }, null, EMonthlyNotificationSectionType.Gossip, 2, 9, null, -1, 62, 0));
		_dataArray.Add(new MonthlyNotificationItem(21, 63, "sp_monthnotify_3_1", 64, new string[6] { "Character", "Location", "Character", "", "", "" }, null, EMonthlyNotificationSectionType.Gossip, 2, 2, null, -1, 65, 0));
		_dataArray.Add(new MonthlyNotificationItem(22, 66, "sp_monthnotify_4_0", 67, new string[6] { "Cricket", "", "", "", "", "" }, null, EMonthlyNotificationSectionType.Biography, 0, 1, null, -1, 68, 0));
		_dataArray.Add(new MonthlyNotificationItem(23, 69, "sp_monthnotify_4_1", 70, new string[6] { "Character", "Resource", "", "", "", "" }, new List<sbyte> { 1 }, EMonthlyNotificationSectionType.Biography, 0, 5, null, -1, 71, 0));
		_dataArray.Add(new MonthlyNotificationItem(24, 72, "sp_monthnotify_4_2", 73, new string[6] { "Character", "Item", "", "", "", "" }, new List<sbyte> { 1 }, EMonthlyNotificationSectionType.Biography, 0, 6, null, -1, 74, 0));
		_dataArray.Add(new MonthlyNotificationItem(25, 75, "sp_monthnotify_5_0", 76, new string[6] { "Character", "Location", "Character", "", "", "" }, null, EMonthlyNotificationSectionType.Gossip, 2, 8, null, -1, 77, 0));
		_dataArray.Add(new MonthlyNotificationItem(26, 78, "sp_monthnotify_5_1", 79, new string[6] { "Character", "Location", "Character", "", "", "" }, null, EMonthlyNotificationSectionType.Gossip, 2, 10, null, -1, 80, 0));
		_dataArray.Add(new MonthlyNotificationItem(27, 81, "sp_monthnotify_5_2", 82, new string[6] { "Character", "Location", "Character", "", "", "" }, null, EMonthlyNotificationSectionType.Gossip, 2, 11, null, -1, 83, 0));
		_dataArray.Add(new MonthlyNotificationItem(28, 84, "sp_monthnotify_5_3", 85, new string[6] { "Character", "Location", "Character", "", "", "" }, null, EMonthlyNotificationSectionType.Gossip, 2, 12, null, -1, 86, 0));
		_dataArray.Add(new MonthlyNotificationItem(29, 87, "sp_monthnotify_5_4", 88, new string[6] { "Character", "Location", "Character", "", "", "" }, null, EMonthlyNotificationSectionType.Gossip, 2, 13, null, -1, 89, 0));
		_dataArray.Add(new MonthlyNotificationItem(30, 90, "sp_monthnotify_5_5", 91, new string[6] { "Character", "Location", "Character", "", "", "" }, null, EMonthlyNotificationSectionType.Gossip, 2, 14, null, -1, 92, 0));
		_dataArray.Add(new MonthlyNotificationItem(31, 93, "sp_monthnotify_5_6", 94, new string[6] { "Character", "Location", "Character", "", "", "" }, null, EMonthlyNotificationSectionType.Gossip, 2, 15, null, -1, 95, 0));
		_dataArray.Add(new MonthlyNotificationItem(32, 96, "sp_monthnotify_5_7", 97, new string[6] { "Character", "Location", "Character", "", "", "" }, null, EMonthlyNotificationSectionType.Gossip, 2, 16, null, -1, 98, 0));
		_dataArray.Add(new MonthlyNotificationItem(33, 99, "sp_monthnotify_5_8", 100, new string[6] { "Character", "Location", "Character", "", "", "" }, null, EMonthlyNotificationSectionType.Gossip, 2, 17, null, -1, 101, 0));
		_dataArray.Add(new MonthlyNotificationItem(34, 102, "sp_monthnotify_5_9", 103, new string[6] { "Character", "Location", "Character", "", "", "" }, null, EMonthlyNotificationSectionType.Gossip, 2, 18, null, -1, 104, 0));
		_dataArray.Add(new MonthlyNotificationItem(35, 105, "sp_monthnotify_5_10", 106, new string[6] { "Character", "Location", "Character", "", "", "" }, null, EMonthlyNotificationSectionType.Gossip, 2, 18, null, -1, 107, 0));
		_dataArray.Add(new MonthlyNotificationItem(36, 108, "sp_monthnotify_5_11", 109, new string[6] { "Character", "Location", "Character", "", "", "" }, null, EMonthlyNotificationSectionType.Gossip, 2, 19, null, -1, 110, 0));
		_dataArray.Add(new MonthlyNotificationItem(37, 111, "sp_monthnotify_5_12", 112, new string[6] { "Character", "Location", "Character", "", "", "" }, null, EMonthlyNotificationSectionType.Gossip, 2, 19, null, -1, 113, 0));
		_dataArray.Add(new MonthlyNotificationItem(38, 114, "sp_monthnotify_5_13", 115, new string[6] { "Character", "Location", "Character", "", "", "" }, null, EMonthlyNotificationSectionType.Gossip, 2, 20, null, -1, 116, 0));
		_dataArray.Add(new MonthlyNotificationItem(39, 117, "sp_monthnotify_5_14", 118, new string[6] { "Character", "Location", "Character", "", "", "" }, null, EMonthlyNotificationSectionType.Gossip, 2, 21, null, -1, 119, 0));
		_dataArray.Add(new MonthlyNotificationItem(40, 120, "sp_monthnotify_6_0", 121, new string[6] { "Character", "Location", "", "", "", "" }, null, EMonthlyNotificationSectionType.Gossip, 2, 22, null, -1, 122, 0));
		_dataArray.Add(new MonthlyNotificationItem(41, 123, "sp_monthnotify_6_1", 124, new string[6] { "Character", "Location", "", "", "", "" }, null, EMonthlyNotificationSectionType.Gossip, 2, 22, null, -1, 125, 0));
		_dataArray.Add(new MonthlyNotificationItem(42, 120, "sp_monthnotify_6_0", 126, new string[6] { "Character", "", "", "", "", "" }, null, EMonthlyNotificationSectionType.Gossip, 2, 22, null, -1, 127, 0));
		_dataArray.Add(new MonthlyNotificationItem(43, 123, "sp_monthnotify_6_1", 128, new string[6] { "Character", "", "", "", "", "" }, null, EMonthlyNotificationSectionType.Gossip, 2, 22, null, -1, 129, 0));
		_dataArray.Add(new MonthlyNotificationItem(44, 130, "sp_monthnotify_6_2", 131, new string[6] { "Character", "", "", "", "", "" }, null, EMonthlyNotificationSectionType.Gossip, 2, 23, null, -1, 132, 0));
		_dataArray.Add(new MonthlyNotificationItem(45, 133, "sp_monthnotify_6_3", 134, new string[6] { "Character", "Location", "", "", "", "" }, null, EMonthlyNotificationSectionType.Gossip, 2, 24, null, -1, 135, 0));
		_dataArray.Add(new MonthlyNotificationItem(46, 136, "sp_monthnotify_7_0", 137, new string[6] { "Character", "Settlement", "", "", "", "" }, null, EMonthlyNotificationSectionType.Gossip, 2, 25, null, -1, 138, 0));
		_dataArray.Add(new MonthlyNotificationItem(47, 139, "sp_monthnotify_7_1", 140, new string[6] { "Character", "Settlement", "", "", "", "" }, null, EMonthlyNotificationSectionType.Gossip, 2, 26, null, -1, 141, 0));
		_dataArray.Add(new MonthlyNotificationItem(48, 142, "sp_monthnotify_7_2", 143, new string[6] { "Character", "Location", "", "", "", "" }, null, EMonthlyNotificationSectionType.Gossip, 2, 27, null, -1, 144, 0));
		_dataArray.Add(new MonthlyNotificationItem(49, 145, "sp_monthnotify_7_3", 146, new string[6] { "Character", "Location", "Character", "", "", "" }, null, EMonthlyNotificationSectionType.Gossip, 2, 27, null, -1, 147, 0));
		_dataArray.Add(new MonthlyNotificationItem(50, 148, "sp_monthnotify_7_4", 149, new string[6] { "Character", "Location", "Character", "", "", "" }, null, EMonthlyNotificationSectionType.Gossip, 2, 27, null, -1, 150, 0));
		_dataArray.Add(new MonthlyNotificationItem(51, 151, "sp_monthnotify_7_5", 152, new string[6] { "Character", "Location", "Character", "", "", "" }, null, EMonthlyNotificationSectionType.Gossip, 2, 27, null, -1, 153, 0));
		_dataArray.Add(new MonthlyNotificationItem(52, 154, "sp_monthnotify_7_6", 155, new string[6] { "Character", "Character", "Character", "", "", "" }, null, EMonthlyNotificationSectionType.Gossip, 2, 28, null, -1, 156, 0));
		_dataArray.Add(new MonthlyNotificationItem(53, 157, "sp_monthnotify_7_7", 158, new string[6] { "Character", "Character", "Character", "", "", "" }, null, EMonthlyNotificationSectionType.Gossip, 2, 28, null, -1, 159, 0));
		_dataArray.Add(new MonthlyNotificationItem(54, 160, "sp_monthnotify_7_8", 161, new string[6] { "Character", "Location", "Character", "", "", "" }, null, EMonthlyNotificationSectionType.Gossip, 2, 28, null, -1, 162, 0));
		_dataArray.Add(new MonthlyNotificationItem(55, 163, "sp_monthnotify_7_9", 164, new string[6] { "Character", "Location", "", "", "", "" }, null, EMonthlyNotificationSectionType.Gossip, 2, 45, null, -1, 165, 0));
		_dataArray.Add(new MonthlyNotificationItem(56, 166, "sp_monthnotify_7_10", 167, new string[6] { "Character", "Character", "", "", "", "" }, null, EMonthlyNotificationSectionType.Gossip, 2, 29, null, -1, 168, 0));
		_dataArray.Add(new MonthlyNotificationItem(57, 169, "sp_monthnotify_7_11", 170, new string[6] { "Character", "Character", "", "", "", "" }, null, EMonthlyNotificationSectionType.Gossip, 2, 30, null, -1, 171, 0));
		_dataArray.Add(new MonthlyNotificationItem(58, 172, "sp_monthnotify_7_12", 173, new string[6] { "Character", "Character", "Character", "", "", "" }, null, EMonthlyNotificationSectionType.Gossip, 2, 30, null, -1, 174, 0));
		_dataArray.Add(new MonthlyNotificationItem(59, 175, "sp_monthnotify_7_13", 176, new string[6] { "Character", "Settlement", "OrgGrade", "", "", "" }, null, EMonthlyNotificationSectionType.Worldwide, 0, 165, null, -1, 177, 0));
	}

	private void CreateItems1()
	{
		_dataArray.Add(new MonthlyNotificationItem(60, 178, "sp_monthnotify_7_14", 176, new string[6] { "Character", "Settlement", "OrgGrade", "", "", "" }, null, EMonthlyNotificationSectionType.Worldwide, 0, 166, null, -1, 179, 0));
		_dataArray.Add(new MonthlyNotificationItem(61, 180, "sp_monthnotify_7_15", 181, new string[6] { "Character", "Settlement", "", "", "", "" }, null, EMonthlyNotificationSectionType.Worldwide, 0, 167, null, -1, 182, 0));
		_dataArray.Add(new MonthlyNotificationItem(62, 183, "sp_monthnotify_8_0", 184, new string[6] { "Character", "Location", "Character", "Resource", "", "" }, null, EMonthlyNotificationSectionType.Gossip, 2, 168, null, -1, 185, 0));
		_dataArray.Add(new MonthlyNotificationItem(63, 183, "sp_monthnotify_8_1", 186, new string[6] { "Character", "Location", "Character", "Resource", "", "" }, null, EMonthlyNotificationSectionType.Gossip, 2, 168, null, -1, 187, 0));
		_dataArray.Add(new MonthlyNotificationItem(64, 188, "sp_monthnotify_8_2", 189, new string[6] { "Character", "Location", "Character", "Resource", "", "" }, null, EMonthlyNotificationSectionType.Gossip, 2, 169, null, -1, 190, 0));
		_dataArray.Add(new MonthlyNotificationItem(65, 191, "sp_monthnotify_8_3", 192, new string[6] { "Character", "Location", "Character", "Resource", "", "" }, null, EMonthlyNotificationSectionType.Gossip, 2, 170, null, -1, 193, 0));
		_dataArray.Add(new MonthlyNotificationItem(66, 194, "sp_monthnotify_8_4", 195, new string[6] { "Character", "Location", "Character", "Resource", "", "" }, null, EMonthlyNotificationSectionType.Gossip, 2, 171, null, -1, 196, 0));
		_dataArray.Add(new MonthlyNotificationItem(67, 197, "sp_monthnotify_8_0", 184, new string[6] { "Character", "Location", "Character", "Item", "", "" }, null, EMonthlyNotificationSectionType.Gossip, 2, 172, null, -1, 198, 0));
		_dataArray.Add(new MonthlyNotificationItem(68, 197, "sp_monthnotify_8_1", 199, new string[6] { "Character", "Location", "Character", "Item", "", "" }, null, EMonthlyNotificationSectionType.Gossip, 2, 172, null, -1, 200, 0));
		_dataArray.Add(new MonthlyNotificationItem(69, 201, "sp_monthnotify_8_2", 189, new string[6] { "Character", "Location", "Character", "Item", "", "" }, null, EMonthlyNotificationSectionType.Gossip, 2, 173, null, -1, 202, 0));
		_dataArray.Add(new MonthlyNotificationItem(70, 203, "sp_monthnotify_8_3", 192, new string[6] { "Character", "Location", "Character", "Item", "", "" }, null, EMonthlyNotificationSectionType.Gossip, 2, 174, null, -1, 204, 0));
		_dataArray.Add(new MonthlyNotificationItem(71, 205, "sp_monthnotify_8_4", 206, new string[6] { "Character", "Location", "Character", "Item", "", "" }, null, EMonthlyNotificationSectionType.Gossip, 2, 175, null, -1, 207, 0));
		_dataArray.Add(new MonthlyNotificationItem(72, 208, "sp_monthnotify_8_5", 209, new string[6] { "Character", "Location", "Character", "LifeSkill", "", "" }, null, EMonthlyNotificationSectionType.Gossip, 2, 176, null, -1, 210, 0));
		_dataArray.Add(new MonthlyNotificationItem(73, 208, "sp_monthnotify_8_6", 211, new string[6] { "Character", "Location", "Character", "LifeSkill", "", "" }, null, EMonthlyNotificationSectionType.Gossip, 2, 176, null, -1, 212, 0));
		_dataArray.Add(new MonthlyNotificationItem(74, 213, "sp_monthnotify_8_7", 214, new string[6] { "Character", "Location", "Character", "LifeSkill", "", "" }, null, EMonthlyNotificationSectionType.Gossip, 2, 177, null, -1, 215, 0));
		_dataArray.Add(new MonthlyNotificationItem(75, 216, "sp_monthnotify_8_8", 209, new string[6] { "Character", "Location", "Character", "CombatSkill", "", "" }, null, EMonthlyNotificationSectionType.Gossip, 2, 178, null, -1, 217, 0));
		_dataArray.Add(new MonthlyNotificationItem(76, 216, "sp_monthnotify_8_9", 211, new string[6] { "Character", "Location", "Character", "CombatSkill", "", "" }, null, EMonthlyNotificationSectionType.Gossip, 2, 178, null, -1, 218, 0));
		_dataArray.Add(new MonthlyNotificationItem(77, 219, "sp_monthnotify_8_10", 214, new string[6] { "Character", "Location", "Character", "CombatSkill", "", "" }, null, EMonthlyNotificationSectionType.Gossip, 2, 179, null, -1, 220, 0));
		_dataArray.Add(new MonthlyNotificationItem(78, 221, "sp_monthnotify_8_11", 222, new string[6] { "Character", "Location", "Resource", "Character", "", "" }, null, EMonthlyNotificationSectionType.Gossip, 2, 180, null, -1, 223, 0));
		_dataArray.Add(new MonthlyNotificationItem(79, 224, "sp_monthnotify_8_12", 222, new string[6] { "Character", "Location", "Item", "Character", "", "" }, null, EMonthlyNotificationSectionType.Gossip, 2, 181, null, -1, 225, 0));
		_dataArray.Add(new MonthlyNotificationItem(80, 226, "sp_monthnotify_8_13", 227, new string[6] { "Character", "Location", "Character", "LifeSkill", "", "" }, null, EMonthlyNotificationSectionType.Gossip, 2, 182, null, -1, 228, 0));
		_dataArray.Add(new MonthlyNotificationItem(81, 226, "sp_monthnotify_8_14", 229, new string[6] { "Character", "Location", "Character", "LifeSkill", "", "" }, null, EMonthlyNotificationSectionType.Gossip, 2, 182, null, -1, 230, 0));
		_dataArray.Add(new MonthlyNotificationItem(82, 231, "sp_monthnotify_8_15", 232, new string[6] { "Character", "Location", "Character", "CombatSkill", "", "" }, null, EMonthlyNotificationSectionType.Gossip, 2, 183, null, -1, 233, 0));
		_dataArray.Add(new MonthlyNotificationItem(83, 231, "sp_monthnotify_8_16", 234, new string[6] { "Character", "Location", "Character", "CombatSkill", "", "" }, null, EMonthlyNotificationSectionType.Gossip, 2, 183, null, -1, 235, 0));
		_dataArray.Add(new MonthlyNotificationItem(84, 236, "sp_monthnotify_9_0", 237, new string[6] { "Character", "Location", "Character", "", "", "" }, null, EMonthlyNotificationSectionType.Gossip, 1, 157, null, -1, 238, 0));
		_dataArray.Add(new MonthlyNotificationItem(85, 239, "sp_monthnotify_9_1", 240, new string[6] { "Character", "Location", "Character", "", "", "" }, null, EMonthlyNotificationSectionType.Gossip, 1, 158, null, -1, 241, 0));
		_dataArray.Add(new MonthlyNotificationItem(86, 242, "sp_monthnotify_9_2", 243, new string[6] { "Character", "Location", "Character", "", "", "" }, null, EMonthlyNotificationSectionType.Gossip, 1, 159, null, -1, 244, 0));
		_dataArray.Add(new MonthlyNotificationItem(87, 245, "sp_monthnotify_9_3", 246, new string[6] { "Character", "Location", "Character", "", "", "" }, null, EMonthlyNotificationSectionType.Gossip, 1, 160, null, -1, 247, 0));
		_dataArray.Add(new MonthlyNotificationItem(88, 248, "sp_monthnotify_9_4", 249, new string[6] { "Character", "Location", "Item", "", "", "" }, null, EMonthlyNotificationSectionType.Worldwide, 0, 161, null, -1, 250, 0));
		_dataArray.Add(new MonthlyNotificationItem(89, 251, "sp_monthnotify_9_5", 252, new string[6] { "Character", "Location", "Character", "LifeSkillType", "", "" }, null, EMonthlyNotificationSectionType.Gossip, 2, 162, null, -1, 253, 0));
		_dataArray.Add(new MonthlyNotificationItem(90, 254, "sp_monthnotify_9_6", 255, new string[6] { "Character", "Location", "Character", "LifeSkillType", "", "" }, null, EMonthlyNotificationSectionType.Gossip, 2, 163, null, -1, 256, 0));
		_dataArray.Add(new MonthlyNotificationItem(91, 257, "sp_monthnotify_9_7", 258, new string[6] { "Character", "Location", "Character", "", "", "" }, null, EMonthlyNotificationSectionType.Gossip, 2, 164, null, -1, 259, 0));
		_dataArray.Add(new MonthlyNotificationItem(92, 260, "sp_monthnotify_10_0", 261, new string[6] { "Character", "Location", "Item", "", "", "" }, null, EMonthlyNotificationSectionType.Worldwide, 2, 184, null, -1, 262, 0));
		_dataArray.Add(new MonthlyNotificationItem(93, 260, "sp_monthnotify_10_0", 263, new string[6] { "Character", "Location", "Integer", "Resource", "", "" }, null, EMonthlyNotificationSectionType.Worldwide, 2, 184, null, -1, 264, 0));
		_dataArray.Add(new MonthlyNotificationItem(94, 265, "sp_monthnotify_10_1", 266, new string[6] { "Character", "Location", "Item", "", "", "" }, null, EMonthlyNotificationSectionType.Worldwide, 2, 185, null, -1, 267, 0));
		_dataArray.Add(new MonthlyNotificationItem(95, 265, "sp_monthnotify_10_1", 268, new string[6] { "Character", "Location", "Item", "", "", "" }, null, EMonthlyNotificationSectionType.Worldwide, 2, 186, null, -1, 269, 0));
		_dataArray.Add(new MonthlyNotificationItem(96, 270, "sp_monthnotify_10_2", 271, new string[6] { "Character", "Location", "Integer", "", "", "" }, null, EMonthlyNotificationSectionType.Worldwide, 2, 187, null, -1, 272, 0));
		_dataArray.Add(new MonthlyNotificationItem(97, 270, "sp_monthnotify_10_2", 273, new string[6] { "Character", "Location", "Integer", "", "", "" }, null, EMonthlyNotificationSectionType.Worldwide, 2, 187, null, -1, 274, 0));
		_dataArray.Add(new MonthlyNotificationItem(98, 270, "sp_monthnotify_10_2", 275, new string[6] { "Character", "Location", "Integer", "", "", "" }, null, EMonthlyNotificationSectionType.Worldwide, 2, 187, null, -1, 276, 0));
		_dataArray.Add(new MonthlyNotificationItem(99, 270, "sp_monthnotify_10_2", 277, new string[6] { "Character", "Location", "PoisonType", "", "", "" }, null, EMonthlyNotificationSectionType.Worldwide, 2, 187, null, -1, 278, 0));
		_dataArray.Add(new MonthlyNotificationItem(100, 270, "sp_monthnotify_10_3", 279, new string[6] { "Character", "Location", "", "", "", "" }, null, EMonthlyNotificationSectionType.Worldwide, 2, 187, null, -1, 280, 0));
		_dataArray.Add(new MonthlyNotificationItem(101, 281, "sp_monthnotify_10_4", 282, new string[6] { "Character", "Location", "Item", "", "", "" }, null, EMonthlyNotificationSectionType.Worldwide, 2, 188, null, -1, 283, 0));
		_dataArray.Add(new MonthlyNotificationItem(102, 281, "sp_monthnotify_10_5", 284, new string[6] { "Character", "Location", "Integer", "Resource", "", "" }, null, EMonthlyNotificationSectionType.Worldwide, 2, 188, null, -1, 285, 0));
		_dataArray.Add(new MonthlyNotificationItem(103, 286, "sp_monthnotify_10_6", 287, new string[6] { "Character", "Location", "Item", "", "", "" }, null, EMonthlyNotificationSectionType.Worldwide, 2, 189, null, -1, 288, 0));
		_dataArray.Add(new MonthlyNotificationItem(104, 286, "sp_monthnotify_10_6", 289, new string[6] { "Character", "Location", "Item", "", "", "" }, null, EMonthlyNotificationSectionType.Worldwide, 2, 189, null, -1, 290, 0));
		_dataArray.Add(new MonthlyNotificationItem(105, 291, "sp_monthnotify_10_7", 292, new string[6] { "Character", "Location", "Integer", "", "", "" }, null, EMonthlyNotificationSectionType.Worldwide, 2, 190, null, -1, 293, 0));
		_dataArray.Add(new MonthlyNotificationItem(106, 291, "sp_monthnotify_10_7", 294, new string[6] { "Character", "Location", "Integer", "", "", "" }, null, EMonthlyNotificationSectionType.Worldwide, 2, 190, null, -1, 295, 0));
		_dataArray.Add(new MonthlyNotificationItem(107, 291, "sp_monthnotify_10_7", 296, new string[6] { "Character", "Location", "Integer", "", "", "" }, null, EMonthlyNotificationSectionType.Worldwide, 2, 190, null, -1, 297, 0));
		_dataArray.Add(new MonthlyNotificationItem(108, 291, "sp_monthnotify_10_7", 298, new string[6] { "Character", "Location", "PoisonType", "", "", "" }, null, EMonthlyNotificationSectionType.Worldwide, 2, 190, null, -1, 299, 0));
		_dataArray.Add(new MonthlyNotificationItem(109, 291, "sp_monthnotify_10_8", 300, new string[6] { "Character", "Location", "", "", "", "" }, null, EMonthlyNotificationSectionType.Worldwide, 2, 190, null, -1, 301, 0));
		_dataArray.Add(new MonthlyNotificationItem(110, 302, "sp_monthnotify_11_0", 303, new string[6] { "Settlement", "Building", "", "", "", "" }, new List<sbyte> { 1 }, EMonthlyNotificationSectionType.TaiwuVillage, 1, 136, new List<sbyte> { 1 }, 1, 304, 1));
		_dataArray.Add(new MonthlyNotificationItem(111, 305, "sp_monthnotify_11_1", 306, new string[6] { "Settlement", "Building", "", "", "", "" }, new List<sbyte> { 1 }, EMonthlyNotificationSectionType.TaiwuVillage, 1, 137, new List<sbyte> { 1 }, 1, 307, 1));
		_dataArray.Add(new MonthlyNotificationItem(112, 308, "sp_monthnotify_11_2", 309, new string[6] { "Settlement", "Building", "", "", "", "" }, new List<sbyte> { 1 }, EMonthlyNotificationSectionType.TaiwuVillage, 0, 138, null, -1, 310, 0));
		_dataArray.Add(new MonthlyNotificationItem(113, 311, "sp_monthnotify_11_3", 312, new string[6] { "Settlement", "Building", "", "", "", "" }, new List<sbyte> { 1 }, EMonthlyNotificationSectionType.TaiwuVillage, 0, 139, null, -1, 313, 0));
		_dataArray.Add(new MonthlyNotificationItem(114, 314, "sp_monthnotify_11_4", 315, new string[6] { "Settlement", "Building", "", "", "", "" }, new List<sbyte> { 1 }, EMonthlyNotificationSectionType.TaiwuVillage, 1, 140, null, 3, 316, 2));
		_dataArray.Add(new MonthlyNotificationItem(115, 317, "sp_monthnotify_11_5", 318, new string[6] { "Settlement", "Building", "", "", "", "" }, new List<sbyte> { 1 }, EMonthlyNotificationSectionType.TaiwuVillage, 1, 141, null, 3, 319, 2));
		_dataArray.Add(new MonthlyNotificationItem(116, 320, "sp_monthnotify_11_6", 321, new string[6] { "Settlement", "Building", "", "", "", "" }, new List<sbyte> { 1 }, EMonthlyNotificationSectionType.TaiwuVillage, 1, 142, null, 3, 322, 2));
		_dataArray.Add(new MonthlyNotificationItem(117, 323, "sp_monthnotify_11_7", 324, new string[6] { "Settlement", "Building", "", "", "", "" }, new List<sbyte> { 1 }, EMonthlyNotificationSectionType.TaiwuVillage, 1, 143, null, 1, 325, 3));
		_dataArray.Add(new MonthlyNotificationItem(118, 326, "sp_monthnotify_11_8", 327, new string[6] { "Location", "Character", "", "", "", "" }, null, EMonthlyNotificationSectionType.TaiwuVillage, 1, 144, null, -1, 328, 0));
		_dataArray.Add(new MonthlyNotificationItem(119, 329, "sp_monthnotify_12_0", 330, new string[6] { "Location", "", "", "", "", "" }, null, EMonthlyNotificationSectionType.Worldwide, 1, 121, null, -1, 331, 0));
	}

	private void CreateItems2()
	{
		_dataArray.Add(new MonthlyNotificationItem(120, 332, "sp_monthnotify_12_1", 333, new string[6] { "Location", "", "", "", "", "" }, null, EMonthlyNotificationSectionType.Worldwide, 1, 122, null, -1, 334, 0));
		_dataArray.Add(new MonthlyNotificationItem(121, 335, "sp_monthnotify_12_2", 336, new string[6] { "Location", "", "", "", "", "" }, null, EMonthlyNotificationSectionType.Worldwide, 1, 123, null, -1, 337, 0));
		_dataArray.Add(new MonthlyNotificationItem(122, 338, "sp_monthnotify_12_3", 339, new string[6] { "Location", "", "", "", "", "" }, null, EMonthlyNotificationSectionType.Worldwide, 1, 124, null, -1, 340, 0));
		_dataArray.Add(new MonthlyNotificationItem(123, 341, "sp_monthnotify_12_4", 342, new string[6] { "Location", "", "", "", "", "" }, null, EMonthlyNotificationSectionType.Worldwide, 1, 125, null, -1, 343, 0));
		_dataArray.Add(new MonthlyNotificationItem(124, 344, "sp_monthnotify_12_5", 345, new string[6] { "Location", "", "", "", "", "" }, null, EMonthlyNotificationSectionType.Worldwide, 1, 126, null, -1, 346, 0));
		_dataArray.Add(new MonthlyNotificationItem(125, 347, "sp_monthnotify_12_6", 348, new string[6] { "Location", "", "", "", "", "" }, null, EMonthlyNotificationSectionType.Worldwide, 1, 127, null, -1, 349, 0));
		_dataArray.Add(new MonthlyNotificationItem(126, 350, "sp_monthnotify_12_7", 351, new string[6] { "Location", "", "", "", "", "" }, null, EMonthlyNotificationSectionType.Worldwide, 1, 128, null, -1, 352, 0));
		_dataArray.Add(new MonthlyNotificationItem(127, 353, "sp_monthnotify_12_8", 354, new string[6] { "Character", "Location", "", "", "", "" }, null, EMonthlyNotificationSectionType.Worldwide, 0, 129, null, -1, 355, 0));
		_dataArray.Add(new MonthlyNotificationItem(128, 356, "sp_monthnotify_12_9", 357, new string[6] { "Character", "Location", "", "", "", "" }, null, EMonthlyNotificationSectionType.Worldwide, 0, 130, null, -1, 358, 0));
		_dataArray.Add(new MonthlyNotificationItem(129, 359, "sp_monthnotify_12_10", 360, new string[6] { "Location", "", "", "", "", "" }, null, EMonthlyNotificationSectionType.Worldwide, 0, 131, null, -1, 361, 0));
		_dataArray.Add(new MonthlyNotificationItem(130, 362, "sp_monthnotify_12_11", 363, new string[6] { "Character", "Location", "", "", "", "" }, null, EMonthlyNotificationSectionType.Worldwide, 0, 132, null, -1, 364, 0));
		_dataArray.Add(new MonthlyNotificationItem(131, 365, "sp_monthnotify_13_0", 366, new string[6] { "", "", "", "", "", "" }, null, EMonthlyNotificationSectionType.Worldwide, 0, 152, null, -1, 367, 0));
		_dataArray.Add(new MonthlyNotificationItem(132, 368, "sp_monthnotify_13_1", 369, new string[6] { "Settlement", "Adventure", "", "", "", "" }, null, EMonthlyNotificationSectionType.Worldwide, 0, 153, null, -1, 370, 0));
		_dataArray.Add(new MonthlyNotificationItem(133, 371, "sp_monthnotify_13_2", 372, new string[6] { "", "", "", "", "", "" }, null, EMonthlyNotificationSectionType.Worldwide, 0, 154, null, -1, 373, 0));
		_dataArray.Add(new MonthlyNotificationItem(134, 374, "sp_monthnotify_13_3", 375, new string[6] { "Settlement", "", "", "", "", "" }, null, EMonthlyNotificationSectionType.Worldwide, 0, 155, null, -1, 376, 0));
		_dataArray.Add(new MonthlyNotificationItem(135, 377, "sp_monthnotify_13_4", 378, new string[6] { "Settlement", "LifeSkillType", "", "", "", "" }, null, EMonthlyNotificationSectionType.Worldwide, 0, 156, null, -1, 379, 0));
		_dataArray.Add(new MonthlyNotificationItem(136, 380, "sp_monthnotify_13_5", 381, new string[6] { "Settlement", "", "", "", "", "" }, null, EMonthlyNotificationSectionType.Worldwide, 0, -1, null, -1, 382, 0));
		_dataArray.Add(new MonthlyNotificationItem(137, 380, "sp_monthnotify_13_6", 383, new string[6] { "Settlement", "", "", "", "", "" }, null, EMonthlyNotificationSectionType.Worldwide, 0, -1, null, -1, 384, 0));
		_dataArray.Add(new MonthlyNotificationItem(138, 385, "sp_monthnotify_13_7", 386, new string[6] { "Settlement", "", "", "", "", "" }, null, EMonthlyNotificationSectionType.Worldwide, 0, -1, null, -1, 387, 0));
		_dataArray.Add(new MonthlyNotificationItem(139, 388, "sp_monthnotify_13_8", 389, new string[6] { "Settlement", "", "", "", "", "" }, null, EMonthlyNotificationSectionType.Worldwide, 0, 42, null, -1, 390, 0));
		_dataArray.Add(new MonthlyNotificationItem(140, 394, "sp_monthnotify_14_0", 395, new string[6] { "SwordTomb", "", "", "", "", "" }, null, EMonthlyNotificationSectionType.Worldwide, 0, 68, null, -1, 396, 0));
		_dataArray.Add(new MonthlyNotificationItem(141, 397, "sp_monthnotify_14_1", 398, new string[6] { "Location", "Character", "", "", "", "" }, null, EMonthlyNotificationSectionType.Worldwide, 0, 99, null, -1, 399, 0));
		_dataArray.Add(new MonthlyNotificationItem(142, 400, "sp_monthnotify_14_2", 401, new string[6] { "Location", "Character", "", "", "", "" }, null, EMonthlyNotificationSectionType.Worldwide, 0, 100, null, -1, 402, 0));
		_dataArray.Add(new MonthlyNotificationItem(143, 403, "sp_monthnotify_14_3", 404, new string[6] { "Location", "", "", "", "", "" }, null, EMonthlyNotificationSectionType.Worldwide, 0, 101, null, -1, 405, 0));
		_dataArray.Add(new MonthlyNotificationItem(144, 406, "sp_monthnotify_14_4", 407, new string[6] { "Location", "Character", "", "", "", "" }, null, EMonthlyNotificationSectionType.Worldwide, 0, 102, null, -1, 408, 0));
		_dataArray.Add(new MonthlyNotificationItem(145, 409, "sp_monthnotify_14_5", 410, new string[6] { "Location", "", "", "", "", "" }, null, EMonthlyNotificationSectionType.Worldwide, 0, 103, null, -1, 411, 0));
		_dataArray.Add(new MonthlyNotificationItem(146, 412, "sp_monthnotify_14_6", 413, new string[6] { "Location", "Character", "", "", "", "" }, null, EMonthlyNotificationSectionType.Worldwide, 0, 104, null, -1, 414, 0));
		_dataArray.Add(new MonthlyNotificationItem(147, 415, "sp_monthnotify_14_7", 416, new string[6] { "Location", "", "", "", "", "" }, null, EMonthlyNotificationSectionType.Worldwide, 0, 105, null, -1, 417, 0));
		_dataArray.Add(new MonthlyNotificationItem(148, 418, "sp_monthnotify_14_8", 419, new string[6] { "Location", "", "", "", "", "" }, null, EMonthlyNotificationSectionType.Worldwide, 0, 106, null, -1, 420, 0));
		_dataArray.Add(new MonthlyNotificationItem(149, 421, "sp_monthnotify_14_9", 422, new string[6] { "Location", "Character", "", "", "", "" }, null, EMonthlyNotificationSectionType.Worldwide, 0, 107, null, -1, 423, 0));
		_dataArray.Add(new MonthlyNotificationItem(150, 424, "sp_monthnotify_14_10", 425, new string[6] { "Character", "Location", "", "", "", "" }, null, EMonthlyNotificationSectionType.Worldwide, 0, 108, null, -1, 426, 0));
		_dataArray.Add(new MonthlyNotificationItem(151, 427, "sp_monthnotify_14_11", 428, new string[6] { "Character", "Location", "", "", "", "" }, null, EMonthlyNotificationSectionType.Worldwide, 0, 109, null, -1, 429, 0));
		_dataArray.Add(new MonthlyNotificationItem(152, 430, "sp_monthnotify_14_12", 431, new string[6] { "Location", "", "", "", "", "" }, null, EMonthlyNotificationSectionType.Worldwide, 0, 110, null, -1, 432, 0));
		_dataArray.Add(new MonthlyNotificationItem(153, 433, "sp_monthnotify_14_13", 434, new string[6] { "Character", "Location", "", "", "", "" }, null, EMonthlyNotificationSectionType.Worldwide, 0, 111, null, -1, 435, 0));
		_dataArray.Add(new MonthlyNotificationItem(154, 436, "sp_monthnotify_14_14", 437, new string[6] { "Location", "", "", "", "", "" }, null, EMonthlyNotificationSectionType.Worldwide, 0, 112, null, -1, 438, 0));
		_dataArray.Add(new MonthlyNotificationItem(155, 439, "sp_monthnotify_14_15", 440, new string[6] { "Character", "Location", "Character", "", "", "" }, null, EMonthlyNotificationSectionType.Worldwide, 0, 113, null, -1, 441, 0));
		_dataArray.Add(new MonthlyNotificationItem(156, 442, "sp_monthnotify_14_16", 443, new string[6] { "Location", "", "", "", "", "" }, null, EMonthlyNotificationSectionType.Worldwide, 0, 114, null, -1, 444, 0));
		_dataArray.Add(new MonthlyNotificationItem(157, 445, "sp_monthnotify_14_17", 446, new string[6] { "Character", "Location", "", "", "", "" }, null, EMonthlyNotificationSectionType.Worldwide, 0, 115, null, -1, 447, 0));
		_dataArray.Add(new MonthlyNotificationItem(158, 448, "sp_monthnotify_14_18", 449, new string[6] { "Character", "Location", "", "", "", "" }, null, EMonthlyNotificationSectionType.Worldwide, 0, 116, null, -1, 450, 0));
		_dataArray.Add(new MonthlyNotificationItem(159, 451, "sp_monthnotify_14_19", 452, new string[6] { "Character", "Location", "", "", "", "" }, null, EMonthlyNotificationSectionType.Worldwide, 0, -1, null, -1, 453, 0));
		_dataArray.Add(new MonthlyNotificationItem(160, 454, "sp_monthnotify_15_0", 455, new string[6] { "Settlement", "MerchantType", "", "", "", "" }, new List<sbyte> { 1 }, EMonthlyNotificationSectionType.Worldwide, 0, 31, new List<sbyte> { 0 }, 1, 456, 1));
		_dataArray.Add(new MonthlyNotificationItem(161, 457, "sp_monthnotify_15_1", 458, new string[6] { "Chicken", "", "", "", "", "" }, new List<sbyte> { 0 }, EMonthlyNotificationSectionType.TaiwuVillage, 0, 146, null, -1, 459, 0));
		_dataArray.Add(new MonthlyNotificationItem(162, 460, "sp_monthnotify_15_2", 461, new string[6] { "Location", "Integer", "Integer", "", "", "" }, null, EMonthlyNotificationSectionType.Worldwide, 0, 32, null, -1, 462, 0));
		_dataArray.Add(new MonthlyNotificationItem(163, 463, "sp_monthnotify_15_3", 464, new string[6] { "Character", "Character", "Settlement", "", "", "" }, null, EMonthlyNotificationSectionType.Gossip, 0, 65, null, -1, 465, 0));
		_dataArray.Add(new MonthlyNotificationItem(164, 466, "sp_monthnotify_15_4", 467, new string[6] { "CombatSkill", "", "", "", "", "" }, null, EMonthlyNotificationSectionType.Biography, 0, 44, null, -1, 468, 0));
		_dataArray.Add(new MonthlyNotificationItem(165, 469, "sp_monthnotify_16_0", 470, new string[6] { "Settlement", "Building", "", "", "", "" }, null, EMonthlyNotificationSectionType.TaiwuVillage, 0, 75, null, -1, 471, 0));
		_dataArray.Add(new MonthlyNotificationItem(166, 472, "sp_monthnotify_16_1", 473, new string[6] { "", "", "", "", "", "" }, null, EMonthlyNotificationSectionType.Biography, 0, 69, null, -1, 474, 0));
		_dataArray.Add(new MonthlyNotificationItem(167, 475, "sp_monthnotify_16_2", 476, new string[6] { "Character", "Location", "Item", "", "", "" }, null, EMonthlyNotificationSectionType.Worldwide, 0, 91, null, -1, 477, 0));
		_dataArray.Add(new MonthlyNotificationItem(168, 478, "sp_monthnotify_16_3", 479, new string[6] { "", "", "", "", "", "" }, null, EMonthlyNotificationSectionType.Worldwide, 0, 33, null, -1, 480, 0));
		_dataArray.Add(new MonthlyNotificationItem(169, 481, "sp_monthnotify_17_0", 482, new string[6] { "", "", "", "", "", "" }, null, EMonthlyNotificationSectionType.Worldwide, 0, 33, null, -1, 483, 0));
		_dataArray.Add(new MonthlyNotificationItem(170, 484, "sp_monthnotify_17_1", 485, new string[6] { "Location", "", "", "", "", "" }, null, EMonthlyNotificationSectionType.Worldwide, 0, 33, null, -1, 486, 0));
		_dataArray.Add(new MonthlyNotificationItem(171, 487, "sp_monthnotify_17_2", 488, new string[6] { "Location", "Integer", "", "", "", "" }, null, EMonthlyNotificationSectionType.Worldwide, 0, 75, null, -1, 489, 0));
		_dataArray.Add(new MonthlyNotificationItem(172, 490, "sp_monthnotify_17_3", 491, new string[6] { "", "", "", "", "", "" }, null, EMonthlyNotificationSectionType.Biography, 1, 117, null, -1, 492, 0));
		_dataArray.Add(new MonthlyNotificationItem(173, 493, "sp_monthnotify_17_4", 494, new string[6] { "", "", "", "", "", "" }, null, EMonthlyNotificationSectionType.Biography, 1, 118, null, -1, 495, 0));
		_dataArray.Add(new MonthlyNotificationItem(174, 496, "sp_monthnotify_17_5", 497, new string[6] { "", "", "", "", "", "" }, null, EMonthlyNotificationSectionType.Biography, 1, 120, null, -1, 498, 0));
		_dataArray.Add(new MonthlyNotificationItem(175, 499, "sp_monthnotify_17_6", 500, new string[6] { "", "", "", "", "", "" }, null, EMonthlyNotificationSectionType.Biography, 1, 120, null, -1, 501, 0));
		_dataArray.Add(new MonthlyNotificationItem(176, 502, "sp_monthnotify_17_7", 503, new string[6] { "Character", "", "", "", "", "" }, null, EMonthlyNotificationSectionType.TaiwuVillage, 0, 75, null, -1, 504, 0));
		_dataArray.Add(new MonthlyNotificationItem(177, 505, "sp_monthnotify_17_8", 506, new string[6] { "", "", "", "", "", "" }, null, EMonthlyNotificationSectionType.TaiwuVillage, 0, 149, null, -1, 507, 0));
		_dataArray.Add(new MonthlyNotificationItem(178, 508, "sp_monthnotify_17_9", 509, new string[6] { "", "", "", "", "", "" }, null, EMonthlyNotificationSectionType.TaiwuVillage, 0, 150, null, -1, 510, 0));
		_dataArray.Add(new MonthlyNotificationItem(179, 511, "sp_monthnotify_17_10", 512, new string[6] { "", "", "", "", "", "" }, null, EMonthlyNotificationSectionType.TaiwuVillage, 0, 151, null, -1, 513, 0));
	}

	private void CreateItems3()
	{
		_dataArray.Add(new MonthlyNotificationItem(180, 514, "sp_monthnotify_17_11", 515, new string[6] { "Character", "Integer", "", "", "", "" }, new List<sbyte> { 1 }, EMonthlyNotificationSectionType.Biography, 0, 119, null, -1, 516, 0));
		_dataArray.Add(new MonthlyNotificationItem(181, 388, "sp_monthnotify_17_12", 517, new string[6] { "Location", "", "", "", "", "" }, null, EMonthlyNotificationSectionType.Biography, 0, 43, null, -1, 518, 0));
		_dataArray.Add(new MonthlyNotificationItem(182, 519, "sp_monthnotify_13_7", 520, new string[6] { "Settlement", "", "", "", "", "" }, new List<sbyte> { 0 }, EMonthlyNotificationSectionType.Worldwide, 0, 34, null, -1, 521, 0));
		_dataArray.Add(new MonthlyNotificationItem(183, 522, "sp_monthnotify_17_13", 523, new string[6] { "Location", "", "", "", "", "" }, null, EMonthlyNotificationSectionType.Biography, 0, 35, null, -1, 524, 0));
		_dataArray.Add(new MonthlyNotificationItem(184, 525, "sp_monthnotify_17_14", 526, new string[6] { "Location", "", "", "", "", "" }, null, EMonthlyNotificationSectionType.Worldwide, 0, 36, null, -1, 527, 0));
		_dataArray.Add(new MonthlyNotificationItem(185, 528, "sp_monthnotify_17_15", 529, new string[6] { "Location", "", "", "", "", "" }, null, EMonthlyNotificationSectionType.Worldwide, 0, -1, null, -1, 530, 0));
		_dataArray.Add(new MonthlyNotificationItem(186, 531, "sp_monthnotify_18_0", 532, new string[6] { "Character", "Location", "Character", "Adventure", "", "" }, null, EMonthlyNotificationSectionType.TaiwuVillage, 1, -1, null, -1, 533, 0));
		_dataArray.Add(new MonthlyNotificationItem(187, 534, "sp_monthnotify_18_1", 535, new string[6] { "Character", "Location", "Character", "Adventure", "", "" }, null, EMonthlyNotificationSectionType.TaiwuVillage, 1, -1, null, -1, 536, 0));
		_dataArray.Add(new MonthlyNotificationItem(188, 537, "sp_monthnotify_18_2", 538, new string[6] { "Character", "Location", "Character", "Adventure", "", "" }, null, EMonthlyNotificationSectionType.TaiwuVillage, 1, -1, null, -1, 539, 0));
		_dataArray.Add(new MonthlyNotificationItem(189, 540, "sp_monthnotify_18_3", 541, new string[6] { "Character", "Location", "Character", "Adventure", "", "" }, null, EMonthlyNotificationSectionType.TaiwuVillage, 1, -1, null, -1, 542, 0));
		_dataArray.Add(new MonthlyNotificationItem(190, 543, "sp_monthnotify_18_4", 544, new string[6] { "Character", "Location", "Character", "Adventure", "", "" }, null, EMonthlyNotificationSectionType.TaiwuVillage, 1, -1, null, -1, 545, 0));
		_dataArray.Add(new MonthlyNotificationItem(191, 546, "sp_monthnotify_18_5", 547, new string[6] { "Character", "Location", "Character", "Adventure", "", "" }, null, EMonthlyNotificationSectionType.TaiwuVillage, 1, -1, null, -1, 548, 0));
		_dataArray.Add(new MonthlyNotificationItem(192, 549, "sp_monthnotify_17_16", 550, new string[6] { "Character", "Location", "Building", "Character", "", "" }, null, EMonthlyNotificationSectionType.TaiwuVillage, 0, -1, null, -1, 551, 0));
		_dataArray.Add(new MonthlyNotificationItem(193, 552, "sp_monthnotify_17_17", 553, new string[6] { "Settlement", "Character", "", "", "", "" }, new List<sbyte> { 1 }, EMonthlyNotificationSectionType.TaiwuVillage, 1, 72, null, 3, 554, 2));
		_dataArray.Add(new MonthlyNotificationItem(194, 555, "sp_monthnotify_17_18", 556, new string[6] { "", "", "", "", "", "" }, null, EMonthlyNotificationSectionType.TaiwuVillage, 1, 147, null, -1, 557, 0));
		_dataArray.Add(new MonthlyNotificationItem(195, 558, "sp_monthnotify_19_0", 559, new string[6] { "", "", "", "", "", "" }, null, EMonthlyNotificationSectionType.TaiwuVillage, 1, 147, null, -1, 560, 0));
		_dataArray.Add(new MonthlyNotificationItem(196, 561, "sp_monthnotify_19_1", 562, new string[6] { "", "", "", "", "", "" }, null, EMonthlyNotificationSectionType.TaiwuVillage, 1, 147, null, -1, 563, 0));
		_dataArray.Add(new MonthlyNotificationItem(197, 564, "sp_monthnotify_19_3", 565, new string[6] { "", "", "", "", "", "" }, null, EMonthlyNotificationSectionType.TaiwuVillage, 1, 147, null, -1, 566, 0));
		_dataArray.Add(new MonthlyNotificationItem(198, 567, "sp_monthnotify_19_2", 568, new string[6] { "", "", "", "", "", "" }, null, EMonthlyNotificationSectionType.TaiwuVillage, 1, 147, null, -1, 569, 0));
		_dataArray.Add(new MonthlyNotificationItem(199, 570, "sp_monthnotify_19_4", 571, new string[6] { "", "", "", "", "", "" }, null, EMonthlyNotificationSectionType.TaiwuVillage, 1, 147, null, -1, 572, 0));
		_dataArray.Add(new MonthlyNotificationItem(200, 573, "sp_monthnotify_19_5", 574, new string[6] { "", "", "", "", "", "" }, null, EMonthlyNotificationSectionType.TaiwuVillage, 1, 147, null, -1, 575, 0));
		_dataArray.Add(new MonthlyNotificationItem(201, 576, "sp_monthnotify_19_6", 577, new string[6] { "", "", "", "", "", "" }, null, EMonthlyNotificationSectionType.TaiwuVillage, 1, 147, null, -1, 578, 0));
		_dataArray.Add(new MonthlyNotificationItem(202, 579, "sp_monthnotify_19_7", 580, new string[6] { "", "", "", "", "", "" }, null, EMonthlyNotificationSectionType.TaiwuVillage, 1, 147, null, -1, 581, 0));
		_dataArray.Add(new MonthlyNotificationItem(203, 582, "sp_monthnotify_19_8", 583, new string[6] { "", "", "", "", "", "" }, null, EMonthlyNotificationSectionType.TaiwuVillage, 1, 147, null, -1, 584, 0));
		_dataArray.Add(new MonthlyNotificationItem(204, 585, "sp_monthnotify_19_9", 586, new string[6] { "", "", "", "", "", "" }, null, EMonthlyNotificationSectionType.TaiwuVillage, 1, 147, null, -1, 587, 0));
		_dataArray.Add(new MonthlyNotificationItem(205, 588, "sp_monthnotify_19_10", 589, new string[6] { "", "", "", "", "", "" }, null, EMonthlyNotificationSectionType.TaiwuVillage, 1, 147, null, -1, 590, 0));
		_dataArray.Add(new MonthlyNotificationItem(206, 591, "sp_monthnotify_19_11", 592, new string[6] { "", "", "", "", "", "" }, null, EMonthlyNotificationSectionType.TaiwuVillage, 1, 147, null, -1, 593, 0));
		_dataArray.Add(new MonthlyNotificationItem(207, 594, "sp_monthnotify_19_12", 595, new string[6] { "", "", "", "", "", "" }, null, EMonthlyNotificationSectionType.TaiwuVillage, 1, 147, null, -1, 596, 0));
		_dataArray.Add(new MonthlyNotificationItem(208, 597, "sp_monthnotify_19_13", 598, new string[6] { "", "", "", "", "", "" }, null, EMonthlyNotificationSectionType.TaiwuVillage, 1, 147, null, -1, 599, 0));
		_dataArray.Add(new MonthlyNotificationItem(209, 600, "sp_monthnotify_19_14", 601, new string[6] { "", "", "", "", "", "" }, null, EMonthlyNotificationSectionType.TaiwuVillage, 1, 147, null, -1, 602, 0));
		_dataArray.Add(new MonthlyNotificationItem(210, 603, "sp_monthnotify_19_15", 604, new string[6] { "", "", "", "", "", "" }, null, EMonthlyNotificationSectionType.TaiwuVillage, 1, 147, null, -1, 605, 0));
		_dataArray.Add(new MonthlyNotificationItem(211, 606, "sp_monthnotify_19_16", 607, new string[6] { "", "", "", "", "", "" }, null, EMonthlyNotificationSectionType.TaiwuVillage, 1, 147, null, -1, 608, 0));
		_dataArray.Add(new MonthlyNotificationItem(212, 609, "sp_monthnotify_19_17", 610, new string[6] { "", "", "", "", "", "" }, null, EMonthlyNotificationSectionType.TaiwuVillage, 0, 148, null, -1, 611, 0));
		_dataArray.Add(new MonthlyNotificationItem(213, 612, "sp_monthnotify_17_20", 613, new string[6] { "Character", "", "", "", "", "" }, null, EMonthlyNotificationSectionType.Biography, 0, 60, null, -1, 614, 0));
		_dataArray.Add(new MonthlyNotificationItem(214, 615, "sp_monthnotify_18_6", 616, new string[6] { "Location", "Adventure", "", "", "", "" }, null, EMonthlyNotificationSectionType.Worldwide, 0, 135, null, -1, 617, 0));
		_dataArray.Add(new MonthlyNotificationItem(215, 618, "sp_monthnotify_17_22", 619, new string[6] { "", "", "", "", "", "" }, null, EMonthlyNotificationSectionType.Biography, 0, -1, null, -1, 620, 0));
		_dataArray.Add(new MonthlyNotificationItem(216, 621, "sp_monthnotify_15_14", 622, new string[6] { "ItemKey", "", "", "", "", "" }, null, EMonthlyNotificationSectionType.Biography, 0, 73, null, -1, 623, 0));
		_dataArray.Add(new MonthlyNotificationItem(217, 391, "sp_monthnotify_5_4", 392, new string[6] { "Character", "Character", "Location", "", "", "" }, null, EMonthlyNotificationSectionType.Gossip, 2, 46, null, -1, 393, 0));
		_dataArray.Add(new MonthlyNotificationItem(218, 627, "sp_monthnotify_20_8", 628, new string[6] { "Location", "", "", "", "", "" }, null, EMonthlyNotificationSectionType.Worldwide, 0, 98, null, -1, 629, 0));
		_dataArray.Add(new MonthlyNotificationItem(219, 624, "sp_monthnotify_20_1", 625, new string[6] { "Location", "", "", "", "", "" }, null, EMonthlyNotificationSectionType.Worldwide, 0, -1, null, -1, 626, 0));
		_dataArray.Add(new MonthlyNotificationItem(220, 630, "sp_monthnotify_20_7", 631, new string[6] { "Location", "", "", "", "", "" }, null, EMonthlyNotificationSectionType.Worldwide, 0, 98, null, -1, 632, 0));
		_dataArray.Add(new MonthlyNotificationItem(221, 633, "sp_monthnotify_20_11", 634, new string[6] { "", "", "", "", "", "" }, null, EMonthlyNotificationSectionType.Worldwide, 0, 98, null, -1, 635, 0));
		_dataArray.Add(new MonthlyNotificationItem(222, 636, "sp_monthnotify_20_13", 637, new string[6] { "Character", "Location", "Item", "", "", "" }, null, EMonthlyNotificationSectionType.Worldwide, 0, 95, null, -1, 638, 0));
		_dataArray.Add(new MonthlyNotificationItem(223, 639, "sp_monthnotify_20_12", 640, new string[6] { "Character", "Location", "Item", "", "", "" }, null, EMonthlyNotificationSectionType.Worldwide, 0, 96, null, -1, 641, 0));
		_dataArray.Add(new MonthlyNotificationItem(224, 642, "sp_monthnotify_20_3", 643, new string[6] { "Character", "Location", "Item", "", "", "" }, null, EMonthlyNotificationSectionType.Worldwide, 0, 97, null, -1, 644, 0));
		_dataArray.Add(new MonthlyNotificationItem(225, 645, "sp_monthnotify_20_4", 646, new string[6] { "Character", "Location", "Item", "", "", "" }, null, EMonthlyNotificationSectionType.Worldwide, 0, 93, null, -1, 647, 0));
		_dataArray.Add(new MonthlyNotificationItem(226, 648, "sp_monthnotify_20_6", 649, new string[6] { "Location", "Item", "", "", "", "" }, null, EMonthlyNotificationSectionType.Worldwide, 0, 90, null, -1, 650, 0));
		_dataArray.Add(new MonthlyNotificationItem(227, 648, "sp_monthnotify_20_6", 651, new string[6] { "Character", "Location", "Item", "", "", "" }, null, EMonthlyNotificationSectionType.Worldwide, 0, 90, null, -1, 652, 0));
		_dataArray.Add(new MonthlyNotificationItem(228, 648, "sp_monthnotify_20_6", 653, new string[6] { "Character", "Location", "Item", "", "", "" }, null, EMonthlyNotificationSectionType.Worldwide, 0, 90, null, -1, 654, 0));
		_dataArray.Add(new MonthlyNotificationItem(229, 648, "sp_monthnotify_20_6", 655, new string[6] { "Character", "Location", "Item", "", "", "" }, null, EMonthlyNotificationSectionType.Worldwide, 0, 90, null, -1, 656, 0));
		_dataArray.Add(new MonthlyNotificationItem(230, 648, "sp_monthnotify_20_0", 657, new string[6] { "Character", "Location", "Item", "", "", "" }, null, EMonthlyNotificationSectionType.Worldwide, 0, 90, null, -1, 658, 0));
		_dataArray.Add(new MonthlyNotificationItem(231, 659, "sp_monthnotify_20_5", 660, new string[6] { "Character", "Location", "Character", "Item", "", "" }, null, EMonthlyNotificationSectionType.Gossip, 0, 92, null, -1, 661, 0));
		_dataArray.Add(new MonthlyNotificationItem(232, 662, "sp_monthnotify_20_10", 663, new string[6] { "Character", "Location", "Character", "Item", "", "" }, null, EMonthlyNotificationSectionType.Gossip, 0, 92, null, -1, 664, 0));
		_dataArray.Add(new MonthlyNotificationItem(233, 665, "sp_monthnotify_20_14", 666, new string[6] { "Character", "", "", "", "", "" }, null, EMonthlyNotificationSectionType.Biography, 0, 94, null, -1, 667, 0));
		_dataArray.Add(new MonthlyNotificationItem(234, 668, null, 669, new string[6] { "Month", "Character", "", "", "", "" }, null, EMonthlyNotificationSectionType.Biography, 2, 37, null, -1, 670, 0));
		_dataArray.Add(new MonthlyNotificationItem(235, 700, "sp_monthnotify_22_0", 701, new string[6] { "Location", "Integer", "", "", "", "" }, null, EMonthlyNotificationSectionType.Biography, 0, -1, null, -1, 765, 0));
		_dataArray.Add(new MonthlyNotificationItem(236, 703, "sp_monthlyevent_2", 766, new string[6] { "Character", "", "", "", "", "" }, null, EMonthlyNotificationSectionType.Biography, 0, -1, null, -1, 767, 0));
		_dataArray.Add(new MonthlyNotificationItem(237, 705, "sp_monthnotify_22_4", 706, new string[6] { "", "", "", "", "", "" }, null, EMonthlyNotificationSectionType.Biography, 0, -1, null, -1, 768, 0));
		_dataArray.Add(new MonthlyNotificationItem(238, 671, "sp_monthnotify_21_0", 672, new string[6] { "Character", "Location", "Item", "", "", "" }, null, EMonthlyNotificationSectionType.Gossip, 2, 85, null, -1, 673, 0));
		_dataArray.Add(new MonthlyNotificationItem(239, 674, "sp_monthnotify_21_1", 675, new string[6] { "Character", "Location", "", "", "", "" }, null, EMonthlyNotificationSectionType.Gossip, 2, 86, null, -1, 676, 0));
	}

	private void CreateItems4()
	{
		_dataArray.Add(new MonthlyNotificationItem(240, 677, "sp_monthnotify_21_2", 678, new string[6] { "Character", "", "", "", "", "" }, null, EMonthlyNotificationSectionType.Gossip, 2, 87, null, -1, 679, 0));
		_dataArray.Add(new MonthlyNotificationItem(241, 680, "sp_monthnotify_21_3", 681, new string[6] { "Character", "Location", "", "", "", "" }, null, EMonthlyNotificationSectionType.Gossip, 2, 88, null, -1, 682, 0));
		_dataArray.Add(new MonthlyNotificationItem(242, 683, "sp_monthnotify_21_4", 684, new string[6] { "Character", "Location", "Character", "", "", "" }, null, EMonthlyNotificationSectionType.Gossip, 2, 89, null, -1, 685, 0));
		_dataArray.Add(new MonthlyNotificationItem(243, 683, "sp_monthnotify_21_5", 686, new string[6] { "Character", "Location", "Character", "", "", "" }, null, EMonthlyNotificationSectionType.Gossip, 2, 89, null, -1, 687, 0));
		_dataArray.Add(new MonthlyNotificationItem(244, 683, "sp_monthnotify_21_6", 688, new string[6] { "Character", "Location", "Character", "", "", "" }, null, EMonthlyNotificationSectionType.Gossip, 2, 89, null, -1, 689, 0));
		_dataArray.Add(new MonthlyNotificationItem(245, 683, "sp_monthnotify_21_7", 690, new string[6] { "Character", "Location", "Character", "", "", "" }, null, EMonthlyNotificationSectionType.Gossip, 2, 89, null, -1, 691, 0));
		_dataArray.Add(new MonthlyNotificationItem(246, 683, "sp_monthnotify_21_4", 692, new string[6] { "Character", "Location", "Character", "", "", "" }, null, EMonthlyNotificationSectionType.Gossip, 2, 89, null, -1, 693, 0));
		_dataArray.Add(new MonthlyNotificationItem(247, 683, "sp_monthnotify_21_5", 694, new string[6] { "Character", "Location", "Character", "", "", "" }, null, EMonthlyNotificationSectionType.Gossip, 2, 89, null, -1, 695, 0));
		_dataArray.Add(new MonthlyNotificationItem(248, 683, "sp_monthnotify_21_6", 696, new string[6] { "Character", "Location", "Character", "", "", "" }, null, EMonthlyNotificationSectionType.Gossip, 2, 89, null, -1, 697, 0));
		_dataArray.Add(new MonthlyNotificationItem(249, 683, "sp_monthnotify_21_7", 698, new string[6] { "Character", "Location", "Character", "", "", "" }, null, EMonthlyNotificationSectionType.Gossip, 2, 89, null, -1, 699, 0));
		_dataArray.Add(new MonthlyNotificationItem(250, 700, "sp_monthnotify_22_0", 701, new string[6] { "Location", "Integer", "", "", "", "" }, null, EMonthlyNotificationSectionType.Biography, 0, 76, null, -1, 702, 0));
		_dataArray.Add(new MonthlyNotificationItem(251, 703, "sp_monthlyevent_2", 619, new string[6] { "Character", "", "", "", "", "" }, null, EMonthlyNotificationSectionType.Biography, 0, -1, null, -1, 704, 0));
		_dataArray.Add(new MonthlyNotificationItem(252, 705, "sp_monthnotify_22_4", 706, new string[6] { "", "", "", "", "", "" }, null, EMonthlyNotificationSectionType.Biography, 0, 77, null, -1, 707, 0));
		_dataArray.Add(new MonthlyNotificationItem(253, 708, "sp_monthnotify_22_1", 709, new string[6] { "", "", "", "", "", "" }, null, EMonthlyNotificationSectionType.Biography, 0, 76, null, -1, 710, 0));
		_dataArray.Add(new MonthlyNotificationItem(254, 711, "sp_monthnotify_17_23", 712, new string[6] { "Settlement", "", "", "", "", "" }, null, EMonthlyNotificationSectionType.Worldwide, 0, 33, null, -1, 713, 0));
		_dataArray.Add(new MonthlyNotificationItem(255, 714, "sp_monthnotify_22_7", 715, new string[6] { "Character", "Location", "", "", "", "" }, null, EMonthlyNotificationSectionType.Biography, 1, 78, null, -1, 716, 0));
		_dataArray.Add(new MonthlyNotificationItem(256, 717, "sp_monthnotify_22_8", 718, new string[6] { "", "", "", "", "", "" }, null, EMonthlyNotificationSectionType.Biography, 0, 78, null, -1, 719, 0));
		_dataArray.Add(new MonthlyNotificationItem(257, 708, "sp_monthnotify_22_1", 720, new string[6] { "", "", "", "", "", "" }, null, EMonthlyNotificationSectionType.Biography, 0, 76, null, -1, 721, 0));
		_dataArray.Add(new MonthlyNotificationItem(258, 722, "sp_monthnotify_22_2", 723, new string[6] { "Character", "", "", "", "", "" }, null, EMonthlyNotificationSectionType.Biography, 0, 76, null, -1, 724, 0));
		_dataArray.Add(new MonthlyNotificationItem(259, 725, "sp_monthnotify_22_1", 726, new string[6] { "Character", "", "", "", "", "" }, null, EMonthlyNotificationSectionType.Biography, 0, 76, null, -1, 727, 0));
		_dataArray.Add(new MonthlyNotificationItem(260, 728, "sp_monthnotify_22_3", 729, new string[6] { "Location", "", "", "", "", "" }, null, EMonthlyNotificationSectionType.Biography, 0, 79, null, -1, 730, 0));
		_dataArray.Add(new MonthlyNotificationItem(261, 728, "sp_monthnotify_22_3", 731, new string[6] { "Location", "", "", "", "", "" }, null, EMonthlyNotificationSectionType.Biography, 0, 79, null, -1, 732, 0));
		_dataArray.Add(new MonthlyNotificationItem(262, 728, "sp_monthnotify_22_3", 733, new string[6] { "Location", "", "", "", "", "" }, null, EMonthlyNotificationSectionType.Biography, 0, 79, null, -1, 734, 0));
		_dataArray.Add(new MonthlyNotificationItem(263, 728, "sp_monthnotify_22_3", 735, new string[6] { "Location", "", "", "", "", "" }, null, EMonthlyNotificationSectionType.Biography, 0, 79, null, -1, 736, 0));
		_dataArray.Add(new MonthlyNotificationItem(264, 463, "sp_monthnotify_15_3", 464, new string[6] { "Character", "Character", "Location", "", "", "" }, null, EMonthlyNotificationSectionType.Gossip, 0, 65, null, -1, 737, 0));
		_dataArray.Add(new MonthlyNotificationItem(265, 738, "sp_monthnotify_22_5", 739, new string[6] { "Integer", "", "", "", "", "" }, null, EMonthlyNotificationSectionType.Biography, 0, 80, null, -1, 740, 0));
		_dataArray.Add(new MonthlyNotificationItem(266, 741, "sp_monthnotify_22_6", 742, new string[6] { "Character", "Location", "", "", "", "" }, null, EMonthlyNotificationSectionType.Biography, 0, 80, null, -1, 743, 0));
		_dataArray.Add(new MonthlyNotificationItem(267, 744, "sp_monthnotify_15_6", 745, new string[6] { "Character", "Location", "CharacterTemplate", "", "", "" }, null, EMonthlyNotificationSectionType.Worldwide, 2, 38, null, -1, 746, 0));
		_dataArray.Add(new MonthlyNotificationItem(268, 747, "sp_monthnotify_15_7", 748, new string[6] { "Character", "Location", "CharacterTemplate", "", "", "" }, null, EMonthlyNotificationSectionType.Worldwide, 2, 38, null, -1, 749, 0));
		_dataArray.Add(new MonthlyNotificationItem(269, 750, "sp_monthnotify_15_8", 751, new string[6] { "Character", "Location", "CharacterTemplate", "", "", "" }, null, EMonthlyNotificationSectionType.Worldwide, 2, 39, null, -1, 752, 0));
		_dataArray.Add(new MonthlyNotificationItem(270, 753, "sp_monthnotify_15_9", 754, new string[6] { "Character", "Location", "CharacterTemplate", "", "", "" }, null, EMonthlyNotificationSectionType.Worldwide, 2, 39, null, -1, 755, 0));
		_dataArray.Add(new MonthlyNotificationItem(271, 756, "sp_monthnotify_15_10", 757, new string[6] { "Character", "Location", "CharacterTemplate", "", "", "" }, null, EMonthlyNotificationSectionType.Worldwide, 2, 40, null, -1, 758, 0));
		_dataArray.Add(new MonthlyNotificationItem(272, 759, "sp_monthnotify_15_11", 760, new string[6] { "Character", "Location", "CharacterTemplate", "", "", "" }, null, EMonthlyNotificationSectionType.Worldwide, 2, 40, null, -1, 761, 0));
		_dataArray.Add(new MonthlyNotificationItem(273, 762, "sp_monthnotify_15_12", 763, new string[6] { "Character", "Location", "Adventure", "", "", "" }, null, EMonthlyNotificationSectionType.Worldwide, 2, 134, null, -1, 764, 0));
		_dataArray.Add(new MonthlyNotificationItem(274, 769, "sp_monthnotify_15_5", 770, new string[6] { "Character", "Location", "Character", "", "", "" }, null, EMonthlyNotificationSectionType.Gossip, 2, 66, null, -1, 771, 0));
		_dataArray.Add(new MonthlyNotificationItem(275, 769, "sp_monthnotify_15_5", 772, new string[6] { "Character", "Location", "Character", "", "", "" }, null, EMonthlyNotificationSectionType.Gossip, 2, 66, null, -1, 773, 0));
		_dataArray.Add(new MonthlyNotificationItem(276, 769, "sp_monthnotify_15_5", 774, new string[6] { "Character", "Location", "Character", "", "", "" }, null, EMonthlyNotificationSectionType.Gossip, 2, 66, null, -1, 775, 0));
		_dataArray.Add(new MonthlyNotificationItem(277, 776, "sp_monthnotify_22_9", 777, new string[6] { "", "", "", "", "", "" }, null, EMonthlyNotificationSectionType.Biography, 0, 78, null, -1, 778, 0));
		_dataArray.Add(new MonthlyNotificationItem(278, 779, "sp_monthnotify_22_10", 780, new string[6] { "", "", "", "", "", "" }, null, EMonthlyNotificationSectionType.Biography, 0, 78, null, -1, 781, 0));
		_dataArray.Add(new MonthlyNotificationItem(279, 782, "sp_monthnotify_12_12", 783, new string[6] { "Character", "Location", "Adventure", "", "", "" }, null, EMonthlyNotificationSectionType.Worldwide, 2, 133, null, -1, 784, 0));
		_dataArray.Add(new MonthlyNotificationItem(280, 782, "sp_monthnotify_12_13", 785, new string[6] { "Character", "Location", "Adventure", "Character", "", "" }, null, EMonthlyNotificationSectionType.Worldwide, 2, 133, null, -1, 786, 0));
		_dataArray.Add(new MonthlyNotificationItem(281, 787, "sp_monthnotify_15_13", 788, new string[6] { "Settlement", "Character", "Building", "", "", "" }, null, EMonthlyNotificationSectionType.TaiwuVillage, 0, 41, null, -1, 789, 0));
		_dataArray.Add(new MonthlyNotificationItem(282, 790, "sp_monthnotify_22_24", 791, new string[6] { "Location", "CharacterTemplate", "", "", "", "" }, null, EMonthlyNotificationSectionType.Worldwide, 0, 47, null, -1, 792, 0));
		_dataArray.Add(new MonthlyNotificationItem(283, 793, "sp_monthnotify_22_11", 794, new string[6] { "Location", "JiaoLoong", "", "", "", "" }, null, EMonthlyNotificationSectionType.TaiwuVillage, 0, 48, null, -1, 795, 0));
		_dataArray.Add(new MonthlyNotificationItem(284, 796, "sp_monthnotify_22_12", 797, new string[6] { "Location", "JiaoLoong", "", "", "", "" }, null, EMonthlyNotificationSectionType.TaiwuVillage, 0, 49, null, -1, 798, 0));
		_dataArray.Add(new MonthlyNotificationItem(285, 799, "sp_monthnotify_22_13", 800, new string[6] { "Location", "JiaoLoong", "", "", "", "" }, null, EMonthlyNotificationSectionType.TaiwuVillage, 0, 50, null, -1, 801, 0));
		_dataArray.Add(new MonthlyNotificationItem(286, 802, "sp_monthnotify_22_14", 803, new string[6] { "Location", "JiaoLoong", "", "", "", "" }, null, EMonthlyNotificationSectionType.TaiwuVillage, 0, 50, null, -1, 804, 0));
		_dataArray.Add(new MonthlyNotificationItem(287, 805, "sp_monthnotify_22_15", 806, new string[6] { "Character", "Location", "JiaoLoong", "", "", "" }, null, EMonthlyNotificationSectionType.Biography, 0, 51, null, -1, 807, 0));
		_dataArray.Add(new MonthlyNotificationItem(288, 808, "sp_monthnotify_22_16 ", 809, new string[6] { "Character", "Location", "JiaoLoong", "", "", "" }, null, EMonthlyNotificationSectionType.Biography, 0, 52, null, -1, 810, 0));
		_dataArray.Add(new MonthlyNotificationItem(289, 811, "sp_monthnotify_22_17", 812, new string[6] { "Location", "JiaoLoong", "", "", "", "" }, null, EMonthlyNotificationSectionType.Biography, 0, 53, null, -1, 813, 0));
		_dataArray.Add(new MonthlyNotificationItem(290, 814, "sp_monthnotify_22_18", 815, new string[6] { "JiaoLoong", "Cricket", "", "", "", "" }, null, EMonthlyNotificationSectionType.Biography, 0, 54, null, -1, 816, 0));
		_dataArray.Add(new MonthlyNotificationItem(291, 817, "sp_monthnotify_22_19", 818, new string[6] { "Location", "JiaoLoong", "Item", "Integer", "", "" }, null, EMonthlyNotificationSectionType.Biography, 0, 55, null, -1, 819, 0));
		_dataArray.Add(new MonthlyNotificationItem(292, 820, "sp_monthnotify_22_20", 821, new string[6] { "Character", "Location", "JiaoLoong", "", "", "" }, null, EMonthlyNotificationSectionType.Biography, 0, 56, null, -1, 822, 0));
		_dataArray.Add(new MonthlyNotificationItem(293, 823, "sp_monthnotify_22_21", 824, new string[6] { "Location", "JiaoLoong", "", "", "", "" }, null, EMonthlyNotificationSectionType.Biography, 0, 57, null, -1, 825, 0));
		_dataArray.Add(new MonthlyNotificationItem(294, 826, "sp_monthnotify_22_22", 827, new string[6] { "Location", "JiaoLoong", "Item", "Integer", "", "" }, null, EMonthlyNotificationSectionType.Biography, 0, 58, null, -1, 828, 0));
		_dataArray.Add(new MonthlyNotificationItem(295, 829, "sp_monthnotify_22_23", 830, new string[6] { "Location", "JiaoLoong", "", "", "", "" }, null, EMonthlyNotificationSectionType.Biography, 0, 59, null, -1, 831, 0));
		_dataArray.Add(new MonthlyNotificationItem(296, 832, "sp_monthnotify_22_25", 833, new string[6] { "Location", "JiaoLoong", "JiaoLoong", "", "", "" }, null, EMonthlyNotificationSectionType.TaiwuVillage, 0, 50, null, -1, 834, 0));
		_dataArray.Add(new MonthlyNotificationItem(297, 835, "sp_monthnotify_22_12", 836, new string[6] { "Location", "JiaoLoong", "", "", "", "" }, new List<sbyte> { 1 }, EMonthlyNotificationSectionType.TaiwuVillage, 0, 50, null, -1, 837, 0));
		_dataArray.Add(new MonthlyNotificationItem(298, 838, "sp_monthnotify_2_2", 839, new string[6] { "Character", "Location", "", "", "", "" }, null, EMonthlyNotificationSectionType.Gossip, 2, 61, null, -1, 840, 0));
		_dataArray.Add(new MonthlyNotificationItem(299, 841, "sp_monthnotify_2_2", 842, new string[6] { "Character", "Location", "", "", "", "" }, null, EMonthlyNotificationSectionType.Gossip, 2, 62, null, -1, 843, 0));
	}

	private void CreateItems5()
	{
		_dataArray.Add(new MonthlyNotificationItem(300, 844, "sp_monthnotify_2_0", 845, new string[6] { "Character", "Location", "Character", "", "", "" }, null, EMonthlyNotificationSectionType.Gossip, 2, 63, null, -1, 846, 0));
		_dataArray.Add(new MonthlyNotificationItem(301, 847, "sp_monthnotify_22_30", 848, new string[6] { "Character", "", "", "", "", "" }, null, EMonthlyNotificationSectionType.Biography, 0, 82, null, -1, 849, 0));
		_dataArray.Add(new MonthlyNotificationItem(302, 850, "sp_monthnotify_22_29", 851, new string[6] { "Character", "", "", "", "", "" }, null, EMonthlyNotificationSectionType.Biography, 0, 82, null, -1, 852, 0));
		_dataArray.Add(new MonthlyNotificationItem(303, 853, "sp_monthnotify_22_27", 854, new string[6] { "", "", "", "", "", "" }, null, EMonthlyNotificationSectionType.Worldwide, 0, 82, null, -1, 855, 0));
		_dataArray.Add(new MonthlyNotificationItem(304, 856, "sp_monthnotify_22_28", 857, new string[6] { "", "", "", "", "", "" }, null, EMonthlyNotificationSectionType.Worldwide, 0, 82, null, -1, 858, 0));
		_dataArray.Add(new MonthlyNotificationItem(305, 859, "sp_monthnotify_22_26", 860, new string[6] { "Character", "Location", "", "", "", "" }, null, EMonthlyNotificationSectionType.Worldwide, 0, 82, null, -1, 861, 0));
		_dataArray.Add(new MonthlyNotificationItem(306, 862, "sp_monthnotify_22_31", 863, new string[6] { "Item", "Location", "Character", "", "", "" }, null, EMonthlyNotificationSectionType.Worldwide, 0, 81, null, -1, 864, 0));
		_dataArray.Add(new MonthlyNotificationItem(307, 865, "sp_monthnotify_22_32", 866, new string[6] { "Character", "Item", "", "", "", "" }, null, EMonthlyNotificationSectionType.Worldwide, 0, 81, null, -1, 867, 0));
		_dataArray.Add(new MonthlyNotificationItem(308, 865, "sp_monthnotify_22_32", 868, new string[6] { "Character", "Item", "", "", "", "" }, null, EMonthlyNotificationSectionType.Worldwide, 0, 81, null, -1, 869, 0));
		_dataArray.Add(new MonthlyNotificationItem(309, 870, "sp_monthnotify_22_33", 871, new string[6] { "", "", "", "", "", "" }, null, EMonthlyNotificationSectionType.Worldwide, 0, 82, null, -1, 872, 0));
		_dataArray.Add(new MonthlyNotificationItem(310, 873, "sp_monthnotify_22_34", 874, new string[6] { "", "", "", "", "", "" }, null, EMonthlyNotificationSectionType.Worldwide, 0, 82, null, -1, 875, 0));
		_dataArray.Add(new MonthlyNotificationItem(311, 876, "sp_monthnotify_22_35", 877, new string[6] { "Character", "", "", "", "", "" }, null, EMonthlyNotificationSectionType.Worldwide, 0, 81, null, -1, 878, 0));
		_dataArray.Add(new MonthlyNotificationItem(312, 879, "sp_monthnotify_22_26", 880, new string[6] { "Character", "", "", "", "", "" }, null, EMonthlyNotificationSectionType.Worldwide, 0, 82, null, -1, 881, 0));
		_dataArray.Add(new MonthlyNotificationItem(313, 882, "sp_monthnotify_22_36", 883, new string[6] { "", "", "", "", "", "" }, null, EMonthlyNotificationSectionType.Worldwide, 0, 83, null, -1, 884, 0));
		_dataArray.Add(new MonthlyNotificationItem(314, 885, "sp_monthnotify_22_37", 886, new string[6] { "Character", "Item", "Location", "", "", "" }, null, EMonthlyNotificationSectionType.Biography, 0, 83, null, -1, 887, 0));
		_dataArray.Add(new MonthlyNotificationItem(315, 885, "sp_monthnotify_22_38", 888, new string[6] { "Character", "Item", "Location", "", "", "" }, null, EMonthlyNotificationSectionType.Biography, 0, 83, null, -1, 889, 0));
		_dataArray.Add(new MonthlyNotificationItem(316, 885, "sp_monthnotify_22_39", 890, new string[6] { "Character", "Item", "Location", "", "", "" }, null, EMonthlyNotificationSectionType.Biography, 0, 83, null, -1, 891, 0));
		_dataArray.Add(new MonthlyNotificationItem(317, 885, "sp_monthnotify_22_40", 892, new string[6] { "Character", "Item", "", "", "", "" }, null, EMonthlyNotificationSectionType.Biography, 0, 83, null, -1, 893, 0));
		_dataArray.Add(new MonthlyNotificationItem(318, 885, "sp_monthnotify_22_41", 894, new string[6] { "Character", "Item", "", "", "", "" }, null, EMonthlyNotificationSectionType.Biography, 0, 83, null, -1, 895, 0));
		_dataArray.Add(new MonthlyNotificationItem(319, 885, "sp_monthnotify_22_42", 896, new string[6] { "Character", "Item", "", "", "", "" }, null, EMonthlyNotificationSectionType.Biography, 0, 83, null, -1, 897, 0));
		_dataArray.Add(new MonthlyNotificationItem(320, 885, "sp_monthnotify_22_40", 904, new string[6] { "Character", "", "", "", "", "" }, null, EMonthlyNotificationSectionType.Biography, 0, 83, null, -1, 905, 0));
		_dataArray.Add(new MonthlyNotificationItem(321, 885, "sp_monthnotify_22_41", 906, new string[6] { "Character", "", "", "", "", "" }, null, EMonthlyNotificationSectionType.Biography, 0, 83, null, -1, 907, 0));
		_dataArray.Add(new MonthlyNotificationItem(322, 885, "sp_monthnotify_22_42", 908, new string[6] { "Character", "", "", "", "", "" }, null, EMonthlyNotificationSectionType.Biography, 0, 83, null, -1, 909, 0));
		_dataArray.Add(new MonthlyNotificationItem(323, 885, "sp_monthnotify_22_40", 898, new string[6] { "Character", "Item", "", "", "", "" }, null, EMonthlyNotificationSectionType.Biography, 0, 83, null, -1, 899, 0));
		_dataArray.Add(new MonthlyNotificationItem(324, 885, "sp_monthnotify_22_41", 900, new string[6] { "Character", "Item", "", "", "", "" }, null, EMonthlyNotificationSectionType.Biography, 0, 83, null, -1, 901, 0));
		_dataArray.Add(new MonthlyNotificationItem(325, 885, "sp_monthnotify_22_42", 902, new string[6] { "Character", "Item", "", "", "", "" }, null, EMonthlyNotificationSectionType.Biography, 0, 83, null, -1, 903, 0));
		_dataArray.Add(new MonthlyNotificationItem(326, 910, "sp_monthnotify_22_43", 911, new string[6] { "Character", "", "", "", "", "" }, new List<sbyte> { 0 }, EMonthlyNotificationSectionType.Biography, 0, 84, null, -1, 912, 0));
		_dataArray.Add(new MonthlyNotificationItem(327, 910, "sp_monthnotify_22_44", 913, new string[6] { "Character", "", "", "", "", "" }, new List<sbyte> { 0 }, EMonthlyNotificationSectionType.Biography, 0, 84, null, -1, 914, 0));
		_dataArray.Add(new MonthlyNotificationItem(328, 915, "sp_monthnotify_22_46", 916, new string[6] { "Location", "", "", "", "", "" }, null, EMonthlyNotificationSectionType.Worldwide, 0, 84, null, -1, 917, 0));
		_dataArray.Add(new MonthlyNotificationItem(329, 918, "sp_monthnotify_22_47", 919, new string[6] { "Location", "", "", "", "", "" }, null, EMonthlyNotificationSectionType.Worldwide, 0, 84, null, -1, 920, 0));
		_dataArray.Add(new MonthlyNotificationItem(330, 921, "sp_monthnotify_22_48", 922, new string[6] { "", "", "", "", "", "" }, null, EMonthlyNotificationSectionType.Biography, 0, 84, null, -1, 923, 0));
		_dataArray.Add(new MonthlyNotificationItem(331, 924, "sp_monthnotify_22_49", 925, new string[6] { "", "", "", "", "", "" }, null, EMonthlyNotificationSectionType.Biography, 0, 84, null, -1, 926, 0));
		_dataArray.Add(new MonthlyNotificationItem(332, 927, "sp_monthnotify_22_50", 928, new string[6] { "Location", "", "", "", "", "" }, null, EMonthlyNotificationSectionType.Worldwide, 0, 84, null, -1, 929, 0));
		_dataArray.Add(new MonthlyNotificationItem(333, 930, "sp_monthnotify_22_51", 931, new string[6] { "Location", "", "", "", "", "" }, null, EMonthlyNotificationSectionType.Worldwide, 0, 84, null, -1, 932, 0));
		_dataArray.Add(new MonthlyNotificationItem(334, 933, "sp_monthnotify_15_15", 934, new string[6] { "CombatSkill", "", "", "", "", "" }, null, EMonthlyNotificationSectionType.Biography, 0, 74, null, -1, 935, 0));
		_dataArray.Add(new MonthlyNotificationItem(335, 936, "sp_monthnotify_22_52", 937, new string[6] { "", "", "", "", "", "" }, null, EMonthlyNotificationSectionType.Biography, 0, 84, null, -1, 938, 0));
		_dataArray.Add(new MonthlyNotificationItem(336, 939, "sp_monthnotify_11_9", 940, new string[6] { "Settlement", "Building", "", "", "", "" }, null, EMonthlyNotificationSectionType.TaiwuVillage, 0, 191, null, -1, 941, 0));
		_dataArray.Add(new MonthlyNotificationItem(337, 942, "sp_monthnotify_22_53", 943, new string[6] { "", "", "", "", "", "" }, null, EMonthlyNotificationSectionType.Worldwide, 0, 194, null, -1, 944, 0));
		_dataArray.Add(new MonthlyNotificationItem(338, 945, "sp_monthnotify_22_54", 946, new string[6] { "Chicken", "", "", "", "", "" }, new List<sbyte> { 0 }, EMonthlyNotificationSectionType.Worldwide, 0, 194, null, -1, 947, 0));
		_dataArray.Add(new MonthlyNotificationItem(339, 948, "sp_monthnotify_13_9", 949, new string[6] { "Integer", "", "", "", "", "" }, null, EMonthlyNotificationSectionType.Worldwide, 0, 152, null, -1, 950, 0));
		_dataArray.Add(new MonthlyNotificationItem(340, 960, "sp_monthnotify_13_14", 961, new string[6] { "Settlement", "Integer", "", "", "", "" }, new List<sbyte> { 0 }, EMonthlyNotificationSectionType.Worldwide, 0, 34, null, -1, 962, 0));
		_dataArray.Add(new MonthlyNotificationItem(341, 951, "sp_monthnotify_13_10", 952, new string[6] { "Settlement", "Integer", "Adventure", "", "", "" }, null, EMonthlyNotificationSectionType.Worldwide, 0, 153, null, -1, 953, 0));
		_dataArray.Add(new MonthlyNotificationItem(342, 957, "sp_monthnotify_13_12", 958, new string[6] { "Settlement", "Integer", "Adventure", "", "", "" }, null, EMonthlyNotificationSectionType.Worldwide, 0, 156, null, -1, 959, 0));
		_dataArray.Add(new MonthlyNotificationItem(343, 954, "sp_monthnotify_13_11", 955, new string[6] { "Settlement", "Integer", "", "", "", "" }, null, EMonthlyNotificationSectionType.Worldwide, 0, 155, null, -1, 956, 0));
		_dataArray.Add(new MonthlyNotificationItem(344, 963, "sp_monthnotify_13_13", 964, new string[6] { "Settlement", "Integer", "", "", "", "" }, null, EMonthlyNotificationSectionType.Worldwide, 0, 42, null, -1, 965, 0));
		_dataArray.Add(new MonthlyNotificationItem(345, 966, "sp_monthnotify_15_18", 967, new string[6] { "Character", "", "", "", "", "" }, new List<sbyte> { 0 }, EMonthlyNotificationSectionType.Biography, 0, 192, null, -1, 968, 0));
		_dataArray.Add(new MonthlyNotificationItem(346, 969, "sp_monthnotify_15_17", 970, new string[6] { "Character", "", "", "", "", "" }, new List<sbyte> { 0 }, EMonthlyNotificationSectionType.Biography, 0, 192, null, -1, 971, 0));
		_dataArray.Add(new MonthlyNotificationItem(347, 972, "sp_monthnotify_15_16", 973, new string[6] { "Character", "", "", "", "", "" }, new List<sbyte> { 0 }, EMonthlyNotificationSectionType.Biography, 0, 192, null, -1, 974, 0));
		_dataArray.Add(new MonthlyNotificationItem(348, 975, "sp_monthnotify_22_55", 976, new string[6] { "Character", "Settlement", "", "", "", "" }, new List<sbyte> { 0 }, EMonthlyNotificationSectionType.Worldwide, 0, 194, new List<sbyte> { 1 }, -1, 977, 0));
		_dataArray.Add(new MonthlyNotificationItem(349, 978, "sp_monthnotify_22_56", 979, new string[6] { "Character", "Settlement", "", "", "", "" }, new List<sbyte> { 0 }, EMonthlyNotificationSectionType.Worldwide, 0, 194, new List<sbyte> { 1 }, -1, 980, 0));
		_dataArray.Add(new MonthlyNotificationItem(350, 978, "sp_monthnotify_22_56", 979, new string[6] { "Character", "Settlement", "", "", "", "" }, new List<sbyte> { 0 }, EMonthlyNotificationSectionType.Worldwide, 0, 194, new List<sbyte> { 1 }, -1, 981, 0));
		_dataArray.Add(new MonthlyNotificationItem(351, 978, "sp_monthnotify_22_56", 982, new string[6] { "Character", "Settlement", "", "", "", "" }, new List<sbyte> { 0 }, EMonthlyNotificationSectionType.Worldwide, 0, 194, new List<sbyte> { 1 }, -1, 983, 0));
		_dataArray.Add(new MonthlyNotificationItem(352, 978, "sp_monthnotify_22_56", 982, new string[6] { "Character", "Settlement", "", "", "", "" }, new List<sbyte> { 0 }, EMonthlyNotificationSectionType.Worldwide, 0, 194, new List<sbyte> { 1 }, -1, 984, 0));
		_dataArray.Add(new MonthlyNotificationItem(353, 985, "sp_monthnotify_7_16", 986, new string[6] { "Character", "Settlement", "Character", "", "", "" }, null, EMonthlyNotificationSectionType.Worldwide, 0, 193, null, -1, 987, 0));
		_dataArray.Add(new MonthlyNotificationItem(354, 985, "sp_monthnotify_7_17", 988, new string[6] { "Character", "Settlement", "Character", "", "", "" }, null, EMonthlyNotificationSectionType.Worldwide, 0, 193, null, -1, 989, 0));
		_dataArray.Add(new MonthlyNotificationItem(355, 990, "sp_monthnotify_7_18", 991, new string[6] { "Character", "Settlement", "", "", "", "" }, null, EMonthlyNotificationSectionType.Worldwide, 0, 193, null, -1, 992, 0));
		_dataArray.Add(new MonthlyNotificationItem(356, 45, "sp_monthnotify_7_19", 993, new string[6] { "Character", "Settlement", "", "", "", "" }, null, EMonthlyNotificationSectionType.Worldwide, 0, 193, null, -1, 994, 0));
		_dataArray.Add(new MonthlyNotificationItem(357, 48, "sp_monthnotify_7_19", 995, new string[6] { "Character", "Settlement", "", "", "", "" }, null, EMonthlyNotificationSectionType.Worldwide, 0, 193, null, -1, 996, 0));
		_dataArray.Add(new MonthlyNotificationItem(358, 985, "sp_monthnotify_7_20", 997, new string[6] { "Character", "Settlement", "Character", "", "", "" }, null, EMonthlyNotificationSectionType.Worldwide, 0, 193, null, -1, 998, 0));
		_dataArray.Add(new MonthlyNotificationItem(359, 985, "sp_monthnotify_7_17", 999, new string[6] { "Character", "Settlement", "Character", "", "", "" }, null, EMonthlyNotificationSectionType.Worldwide, 0, 193, null, -1, 1000, 0));
	}

	private void CreateItems6()
	{
		_dataArray.Add(new MonthlyNotificationItem(360, 1001, "sp_monthnotify_17_26", 1002, new string[6] { "Character", "Location", "Item", "", "", "" }, null, EMonthlyNotificationSectionType.TaiwuVillage, 0, 191, null, -1, 1003, 0));
		_dataArray.Add(new MonthlyNotificationItem(361, 493, "sp_monthnotify_17_27", 1004, new string[6] { "Character", "Location", "Character", "", "", "" }, null, EMonthlyNotificationSectionType.Biography, 0, 118, null, -1, 1005, 0));
		_dataArray.Add(new MonthlyNotificationItem(362, 490, "sp_monthnotify_17_28", 1006, new string[6] { "Character", "SwordTomb", "CharacterTemplate", "", "", "" }, null, EMonthlyNotificationSectionType.Biography, 0, 117, null, -1, 1007, 0));
		_dataArray.Add(new MonthlyNotificationItem(363, 1008, "sp_monthnotify_22_57", 1009, new string[6] { "", "", "", "", "", "" }, null, EMonthlyNotificationSectionType.Biography, 0, 194, null, -1, 1010, 0));
		_dataArray.Add(new MonthlyNotificationItem(364, 1011, "sp_monthnotify_22_56", 1012, new string[6] { "", "", "", "", "", "" }, null, EMonthlyNotificationSectionType.Worldwide, 0, 194, null, -1, 1013, 0));
		_dataArray.Add(new MonthlyNotificationItem(365, 1014, "sp_monthnotify_22_58", 1015, new string[6] { "Location", "", "", "", "", "" }, null, EMonthlyNotificationSectionType.Biography, 0, 80, null, -1, 1016, 0));
		_dataArray.Add(new MonthlyNotificationItem(366, 1017, "sp_monthnotify_22_36", 1018, new string[6] { "", "", "", "", "", "" }, null, EMonthlyNotificationSectionType.Worldwide, 0, 195, null, -1, 1019, 0));
		_dataArray.Add(new MonthlyNotificationItem(367, 1020, "sp_monthnotify_19_18", 1021, new string[6] { "Merchant", "Settlement", "Integer", "", "", "" }, null, EMonthlyNotificationSectionType.Worldwide, 0, 31, null, -1, 1022, 0));
		_dataArray.Add(new MonthlyNotificationItem(368, 1023, "sp_monthnotify_19_19", 1024, new string[6] { "Merchant", "Settlement", "Integer", "Settlement", "", "" }, null, EMonthlyNotificationSectionType.Worldwide, 0, 31, null, -1, 1025, 0));
		_dataArray.Add(new MonthlyNotificationItem(369, 1023, "sp_monthnotify_19_19", 1026, new string[6] { "Merchant", "Settlement", "Integer", "Settlement", "Integer", "" }, null, EMonthlyNotificationSectionType.Worldwide, 0, 31, null, -1, 1027, 0));
		_dataArray.Add(new MonthlyNotificationItem(370, 1023, "sp_monthnotify_19_19", 1028, new string[6] { "Merchant", "Settlement", "Integer", "Settlement", "Integer", "" }, null, EMonthlyNotificationSectionType.Worldwide, 0, 31, null, -1, 1029, 0));
		_dataArray.Add(new MonthlyNotificationItem(371, 1023, "sp_monthnotify_19_19", 1030, new string[6] { "Merchant", "Settlement", "Integer", "Settlement", "Integer", "" }, null, EMonthlyNotificationSectionType.Worldwide, 0, 31, null, -1, 1031, 0));
		_dataArray.Add(new MonthlyNotificationItem(372, 1023, "sp_monthnotify_19_19", 1032, new string[6] { "Merchant", "Settlement", "Integer", "Settlement", "Integer", "" }, null, EMonthlyNotificationSectionType.Worldwide, 0, 31, null, -1, 1033, 0));
		_dataArray.Add(new MonthlyNotificationItem(373, 1023, "sp_monthnotify_19_19", 1034, new string[6] { "Merchant", "Settlement", "Integer", "Settlement", "Integer", "Integer" }, null, EMonthlyNotificationSectionType.Worldwide, 0, 31, null, -1, 1035, 0));
		_dataArray.Add(new MonthlyNotificationItem(374, 1023, "sp_monthnotify_19_19", 1036, new string[6] { "Merchant", "Settlement", "Integer", "Settlement", "Integer", "Integer" }, null, EMonthlyNotificationSectionType.Worldwide, 0, 31, null, -1, 1037, 0));
		_dataArray.Add(new MonthlyNotificationItem(375, 1023, "sp_monthnotify_19_19", 1038, new string[6] { "Merchant", "Settlement", "Integer", "Settlement", "Integer", "Integer" }, null, EMonthlyNotificationSectionType.Worldwide, 0, 31, null, -1, 1039, 0));
		_dataArray.Add(new MonthlyNotificationItem(376, 1023, "sp_monthnotify_19_19", 1040, new string[6] { "Merchant", "Settlement", "Integer", "Settlement", "Integer", "Integer" }, null, EMonthlyNotificationSectionType.Worldwide, 0, 31, null, -1, 1041, 0));
		_dataArray.Add(new MonthlyNotificationItem(377, 1042, "sp_monthnotify_19_22", 1043, new string[6] { "Merchant", "Settlement", "Integer", "", "", "" }, null, EMonthlyNotificationSectionType.Worldwide, 0, 31, null, -1, 1044, 0));
		_dataArray.Add(new MonthlyNotificationItem(378, 1045, "sp_monthnotify_19_20", 1046, new string[6] { "Merchant", "Location", "", "", "", "" }, null, EMonthlyNotificationSectionType.Worldwide, 0, 31, null, -1, 1047, 0));
		_dataArray.Add(new MonthlyNotificationItem(379, 1048, "sp_monthnotify_19_21", 1049, new string[6] { "Merchant", "Location", "Integer", "", "", "" }, null, EMonthlyNotificationSectionType.Worldwide, 0, 31, null, -1, 1050, 0));
		_dataArray.Add(new MonthlyNotificationItem(380, 1051, "sp_monthnotify_11_10", 1052, new string[6] { "Settlement", "Building", "", "", "", "" }, new List<sbyte> { 1 }, EMonthlyNotificationSectionType.TaiwuVillage, 0, 196, null, -1, 1053, 0));
		_dataArray.Add(new MonthlyNotificationItem(381, 1054, "sp_monthnotify_11_13", 1055, new string[6] { "Settlement", "PunishmentType", "", "", "", "" }, null, EMonthlyNotificationSectionType.Biography, 0, 197, null, -1, 1056, 0));
		_dataArray.Add(new MonthlyNotificationItem(382, 1054, "sp_monthnotify_11_13", 1057, new string[6] { "Settlement", "PunishmentType", "", "", "", "" }, null, EMonthlyNotificationSectionType.Biography, 0, 197, null, -1, 1058, 0));
		_dataArray.Add(new MonthlyNotificationItem(383, 1059, "sp_monthnotify_11_14", 1060, new string[6] { "Character", "Settlement", "", "", "", "" }, null, EMonthlyNotificationSectionType.TaiwuVillage, 0, 198, null, -1, 1061, 0));
		_dataArray.Add(new MonthlyNotificationItem(384, 1062, "sp_monthnotify_11_14", 1063, new string[6] { "Item", "", "", "", "", "" }, new List<sbyte> { 0 }, EMonthlyNotificationSectionType.TaiwuVillage, 0, 198, null, -1, 1064, 0));
		_dataArray.Add(new MonthlyNotificationItem(385, 1059, "sp_monthnotify_11_14", 1060, new string[6] { "Character", "Settlement", "Item", "", "", "" }, null, EMonthlyNotificationSectionType.TaiwuVillage, 0, 198, null, -1, 1065, 0));
		_dataArray.Add(new MonthlyNotificationItem(386, 1066, "sp_monthnotify_11_15", 1067, new string[6] { "Character", "OrgGrade", "Location", "", "", "" }, null, EMonthlyNotificationSectionType.TaiwuVillage, 0, 64, null, -1, 1068, 0));
		_dataArray.Add(new MonthlyNotificationItem(387, 1069, null, 1070, new string[6] { "", "", "", "", "", "" }, null, EMonthlyNotificationSectionType.Worldwide, 0, -1, null, -1, 1071, 0));
		_dataArray.Add(new MonthlyNotificationItem(388, 1072, null, 1073, new string[6] { "", "", "", "", "", "" }, null, EMonthlyNotificationSectionType.Worldwide, 0, -1, null, -1, 1074, 0));
		_dataArray.Add(new MonthlyNotificationItem(389, 1075, "sp_monthnotify_17_21", 1076, new string[6] { "Character", "Location", "", "", "", "" }, new List<sbyte> { 0 }, EMonthlyNotificationSectionType.Biography, 0, 192, null, -1, 1077, 0));
		_dataArray.Add(new MonthlyNotificationItem(390, 1078, "sp_monthnotify_22_60", 1079, new string[6] { "Character", "Settlement", "", "", "", "" }, new List<sbyte> { 0 }, EMonthlyNotificationSectionType.Biography, 0, 192, null, -1, 1080, 0));
		_dataArray.Add(new MonthlyNotificationItem(391, 1081, "sp_monthnotify_22_59", 1082, new string[6] { "", "", "", "", "", "" }, null, EMonthlyNotificationSectionType.Worldwide, 0, 199, null, -1, 1083, 0));
		_dataArray.Add(new MonthlyNotificationItem(392, 1084, "sp_monthnotify_11_17", 1085, new string[6] { "Building", "Item", "", "", "", "" }, null, EMonthlyNotificationSectionType.TaiwuVillage, 0, 143, null, -1, 1086, 0));
		_dataArray.Add(new MonthlyNotificationItem(393, 1087, "sp_monthnotify_22_63", 1088, new string[6] { "Character", "Integer", "", "", "", "" }, null, EMonthlyNotificationSectionType.Gossip, 0, 200, null, -1, 1089, 0));
		_dataArray.Add(new MonthlyNotificationItem(394, 1090, "sp_monthnotify_22_64", 1091, new string[6] { "Character", "Location", "", "", "", "" }, new List<sbyte> { 0 }, EMonthlyNotificationSectionType.Gossip, 0, 200, null, -1, 1092, 0));
		_dataArray.Add(new MonthlyNotificationItem(395, 1087, "sp_monthnotify_22_61", 1093, new string[6] { "Character", "Integer", "", "", "", "" }, null, EMonthlyNotificationSectionType.Biography, 0, 200, null, -1, 1094, 0));
		_dataArray.Add(new MonthlyNotificationItem(396, 1090, "sp_monthnotify_22_62", 1095, new string[6] { "Character", "Location", "", "", "", "" }, new List<sbyte> { 0 }, EMonthlyNotificationSectionType.Biography, 0, 200, null, -1, 1096, 0));
		_dataArray.Add(new MonthlyNotificationItem(397, 738, "sp_monthnotify_22_65", 739, new string[6] { "Integer", "", "", "", "", "" }, null, EMonthlyNotificationSectionType.Biography, 0, 201, null, -1, 1097, 0));
		_dataArray.Add(new MonthlyNotificationItem(398, 741, "sp_monthnotify_22_66", 742, new string[6] { "Character", "Location", "", "", "", "" }, null, EMonthlyNotificationSectionType.Biography, 0, 201, null, -1, 1098, 0));
		_dataArray.Add(new MonthlyNotificationItem(399, 1014, "sp_monthnotify_22_67", 1015, new string[6] { "Location", "", "", "", "", "" }, null, EMonthlyNotificationSectionType.Biography, 0, 201, null, -1, 1099, 0));
		_dataArray.Add(new MonthlyNotificationItem(400, 1062, "sp_monthnotify_11_14", 1100, new string[6] { "Building", "Item", "Character", "", "", "" }, new List<sbyte> { 0 }, EMonthlyNotificationSectionType.TaiwuVillage, 0, 198, null, -1, 1101, 0));
		_dataArray.Add(new MonthlyNotificationItem(401, 1062, "sp_monthnotify_11_14", 1102, new string[6] { "Building", "Item", "", "", "", "" }, new List<sbyte> { 0 }, EMonthlyNotificationSectionType.TaiwuVillage, 0, 198, null, -1, 1103, 0));
		_dataArray.Add(new MonthlyNotificationItem(402, 1062, "sp_monthnotify_11_14", 1104, new string[6] { "Building", "Item", "", "", "", "" }, new List<sbyte> { 0 }, EMonthlyNotificationSectionType.TaiwuVillage, 0, 198, null, -1, 1105, 0));
		_dataArray.Add(new MonthlyNotificationItem(403, 1062, "sp_monthnotify_11_14", 1106, new string[6] { "Building", "Item", "", "", "", "" }, new List<sbyte> { 0 }, EMonthlyNotificationSectionType.TaiwuVillage, 0, 198, null, -1, 1107, 0));
		_dataArray.Add(new MonthlyNotificationItem(404, 1059, "sp_monthnotify_11_14", 1108, new string[6] { "Character", "Settlement", "Item", "Character", "", "" }, null, EMonthlyNotificationSectionType.TaiwuVillage, 0, 198, null, -1, 1109, 0));
		_dataArray.Add(new MonthlyNotificationItem(405, 1059, "sp_monthnotify_11_14", 1110, new string[6] { "Character", "Settlement", "Item", "", "", "" }, null, EMonthlyNotificationSectionType.TaiwuVillage, 0, 198, null, -1, 1111, 0));
		_dataArray.Add(new MonthlyNotificationItem(406, 1059, "sp_monthnotify_11_14", 1112, new string[6] { "Character", "Settlement", "Item", "", "", "" }, null, EMonthlyNotificationSectionType.TaiwuVillage, 0, 198, null, -1, 1113, 0));
		_dataArray.Add(new MonthlyNotificationItem(407, 1059, "sp_monthnotify_11_14", 1114, new string[6] { "Character", "Settlement", "Item", "", "", "" }, null, EMonthlyNotificationSectionType.TaiwuVillage, 0, 198, null, -1, 1115, 0));
		_dataArray.Add(new MonthlyNotificationItem(408, 1116, "sp_monthnotify_22_68", 1117, new string[6] { "Settlement", "Character", "Settlement", "Character", "", "" }, null, EMonthlyNotificationSectionType.Gossip, 0, 202, null, -1, 1118, 0));
		_dataArray.Add(new MonthlyNotificationItem(409, 1116, "sp_monthnotify_22_68", 1119, new string[6] { "Settlement", "Character", "Settlement", "Character", "", "" }, null, EMonthlyNotificationSectionType.Gossip, 0, 202, null, -1, 1120, 0));
		_dataArray.Add(new MonthlyNotificationItem(410, 1116, "sp_monthnotify_22_68", 1121, new string[6] { "Settlement", "Character", "Settlement", "Character", "", "" }, null, EMonthlyNotificationSectionType.Gossip, 0, 202, null, -1, 1122, 0));
		_dataArray.Add(new MonthlyNotificationItem(411, 1123, "sp_monthnotify_7_4", 1124, new string[6] { "Character", "", "", "", "", "" }, new List<sbyte> { 0 }, EMonthlyNotificationSectionType.TaiwuVillage, 0, 203, null, -1, 1125, 0));
	}

	public override void Init()
	{
		base.Init();
		_dataArray = new List<MonthlyNotificationItem>(412);
		CreateItems0();
		CreateItems1();
		CreateItems2();
		CreateItems3();
		CreateItems4();
		CreateItems5();
		CreateItems6();
	}
}
