using System;
using System.Collections.Generic;
using Config.Common;

namespace Config;

[Serializable]
public class MonthlyEvent : ConfigData<MonthlyEventItem, short>
{
	public static class DefKey
	{
		public const short ReadingEvent = 0;

		public const short TaiwuDeath = 1;

		public const short AreaTotallyDestoryed = 2;

		public const short TaiwuInfected = 3;

		public const short TaiwuInfectedPartially = 4;

		public const short RandomEnemyAttack = 5;

		public const short RandomAnimalAttack = 6;

		public const short RandomRighteousAttack = 7;

		public const short InfectedCharacterAttack = 8;

		public const short HumanSkeletonAttack = 9;

		public const short CricketInDream = 10;

		public const short GiveBirthToCricketTaiwu = 11;

		public const short GiveBirthToCricketWife = 12;

		public const short PrenatalEducationTaiwu = 13;

		public const short AbortionTaiwu = 14;

		public const short LoseFetusWife = 15;

		public const short MotherFetusBothDieTaiwu = 16;

		public const short MotherFetusBothDieWife = 17;

		public const short DystociaLoseFetusTaiwu = 18;

		public const short DystociaLoseFetusWife = 19;

		public const short HaveChildBoyTaiwu = 20;

		public const short HaveChildGirlTaiwu = 21;

		public const short HaveChildBoyWife = 22;

		public const short HaveChildGirlWife = 23;

		public const short DystociaButHaveChildBoyTaiwu = 24;

		public const short DystociaButHaveChildGirlTaiwu = 25;

		public const short DystociaButHaveChildBoyWife = 26;

		public const short DystociaButHaveChildGirlWife = 27;

		public const short DystociaAndHaveChildBoyTaiwu = 28;

		public const short DystociaAndHaveChildGirlTaiwu = 29;

		public const short DystociaAndHaveChildBoyWife = 30;

		public const short DystociaAndHaveChildGirlWife = 31;

		public const short AbandonedBabyInVilliage = 32;

		public const short ChildZhuazhou = 33;

		public const short TeachChild = 34;

		public const short ReachAdulthood = 35;

		public const short CaptiveHaveChild = 36;

		public const short CaptiveBecomeEnemy = 37;

		public const short GroupGetMarried = 38;

		public const short SpringMarket = 39;

		public const short SummerTownCompetition = 40;

		public const short AutumnCricketContest = 41;

		public const short WinterLifeCompetition = 42;

		public const short MakeEnemy = 43;

		public const short SeverEnemy = 44;

		public const short Adore = 45;

		public const short Confess = 46;

		public const short Breakup = 47;

		public const short ProposeMarriage = 48;

		public const short BecomeFriend = 49;

		public const short SeverFriendship = 50;

		public const short BecomeSwornBrotherOrSister = 51;

		public const short SeverSwornBrotherhood = 52;

		public const short GetAdoptedByFather = 53;

		public const short GetAdoptedByMother = 54;

		public const short AdoptSon = 55;

		public const short AdoptDaughter = 56;

		public const short Die = 57;

		public const short EscapeFromPrison = 58;

		public const short AppointmentCancelled = 59;

		public const short RevengeAttack = 60;

		public const short AskProtectByRevengeAttack = 61;

		public const short CatchEnemyPoison = 62;

		public const short EnemyPoisonAndEscape = 63;

		public const short CatchEnemyPlotHarm = 64;

		public const short EnemyPlotHarmAndEscape = 65;

		public const short RequestHealOuterInjuryByItem = 66;

		public const short RequestHealOuterInjuryByResource = 67;

		public const short RequestHealInnerInjuryByItem = 68;

		public const short RequestHealInnerInjuryByResource = 69;

		public const short RequestHealPoisonByItem = 70;

		public const short RequestHealPoisonByResource = 71;

		public const short RequestHealth = 72;

		public const short RequestHealDisorderOfQi = 73;

		public const short RequestNeili = 74;

		public const short RequestKillWug = 75;

		public const short RequestFood = 76;

		public const short RequestTeaWine = 77;

		public const short RequestResource = 78;

		public const short RequestItem = 79;

		public const short RequestRepairItem = 80;

		public const short RequestAddPoisonToItem = 81;

		public const short RequestInstructionOnLifeSkill = 82;

		public const short RequestInstructionOnCombatSkill = 83;

		public const short RequestInstructionOnReadingLifeSkill = 84;

		public const short RequestInstructionOnReadingCombatSkill = 85;

		public const short RequestInstructionOnBreakout = 86;

		public const short RequestPlayCombat = 87;

		public const short RequestNormalCombat = 88;

		public const short RequestLifeSkillBattle = 89;

		public const short RequestCricketBattle = 90;

		public const short RescueKidnappedCharacterSecretlyButBeCaught = 91;

		public const short RescueKidnappedCharacterSecretlyAndEscape = 92;

		public const short RescueKidnappedCharacterWithWit = 93;

		public const short RescueKidnappedCharacterWithForce = 94;

		public const short StealResourceButBeCaught = 95;

		public const short StealResourceAndEscape = 96;

		public const short ScamResource = 97;

		public const short RobResource = 98;

		public const short StealItemButBeCaught = 99;

		public const short StealItemAndEscape = 100;

		public const short ScamItem = 101;

		public const short RobItem = 102;

		public const short StealLifeSkillButBeCaught = 103;

		public const short StealLifeSkillAndEscape = 104;

		public const short ScamLifeSkill = 105;

		public const short StealCombatSkillButBeCaught = 106;

		public const short StealCombatSkillAndEscape = 107;

		public const short ScamCombatSkill = 108;

		public const short AdviseExtendFavours = 109;

		public const short AdviseWinPeopleSupport = 110;

		public const short AdviseMerchantFavor = 111;

		public const short AdviseTeaWine = 112;

		public const short AdviseSales = 113;

		public const short AdviseHealInjury = 114;

		public const short AdviseHealPoison = 115;

		public const short AdviseRepairItem = 116;

		public const short AdviseBarb = 117;

		public const short AskForMoney = 118;

		public const short WulinConferenceTaiwuAbsent = 119;

		public const short WulinConferenceAskForHelp = 120;

		public const short TaiwuVillageBeDestoryed = 121;

		public const short ForeverLoverBePunished = 122;

		public const short VillageWoodenManByMonv = 123;

		public const short VillageWoodenManByDayueYaochang = 124;

		public const short VillageWoodenManByJiuhan = 125;

		public const short VillageWoodenManByJinHuanger = 126;

		public const short VillageWoodenManByYiYihou = 127;

		public const short VillageWoodenManByWeiQi = 128;

		public const short VillageWoodenManByYixiang = 129;

		public const short VillageWoodenManByXuefeng = 130;

		public const short VillageWoodenManByShuFang = 131;

		public const short TaiwuNotAttendingWedding = 132;

		public const short TaiwuAlreadyMarried = 133;

		public const short ChallengeForLegendaryBook = 134;

		public const short RequestLegendaryBook = 135;

		public const short ExchangeLegendaryBookByMoney = 136;

		public const short ExchangeLegendaryBookByAuthority = 137;

		public const short ExchangeLegendaryBookByExperience = 138;

		public const short StealLegendaryBookAndEscape = 139;

		public const short StealLegendaryBookGotCaught = 140;

		public const short ScamLegendaryBook = 141;

		public const short RobLegendaryBook = 142;

		public const short LegendaryBookShockedAttack = 143;

		public const short LegendaryBookInsaneAttack = 144;

		public const short LegendaryBookConsumedAttack = 145;

		public const short SwordTombGetStronger = 146;

		public const short SwordTombBackToNormal = 147;

		public const short FightForNewLegendaryBook = 148;

		public const short FightForLegendaryBookAbandoned = 149;

		public const short FightForLegendaryBookOwnerDie = 150;

		public const short FightForLegendaryBookOwnerConsumed = 151;

		public const short DateWithLoverEveryday = 152;

		public const short HappyBirthdayTaiwu = 153;

		public const short LoveAnniversary = 154;

		public const short NeglectedLover = 155;

		public const short LoverBecomeJealous = 156;

		public const short LoversBecomeJealousAndViolent = 157;

		public const short PregnancyWithLover = 158;

		public const short BeggerSkill2TargetUnavailable = 159;

		public const short BeggarSkill2TargetBrought = 160;

		public const short BeggarSkill2TargetDeadAndMissing = 161;

		public const short BeggarSkill2TargetDead = 162;

		public const short BeggarSkill2TargetNoneExistent = 163;

		public const short TaiwuTribulation = 164;

		public const short TaiwuComingSuccess = 165;

		public const short TaiwuComingDefeated = 166;

		public const short TaiwuFreeAndunFettered = 167;

		public const short SectMainStoryKongsangTargetFound = 168;

		public const short SectMainStoryKongsangAdventure = 169;

		public const short SectMainStoryKongsangProsperous = 170;

		public const short SectMainStoryKongsangFailing = 171;

		public const short SectMainStoryXuehouGraveDigging = 172;

		public const short SectMainStoryXuehouGraveDiggingNormal = 173;

		public const short SectMainStoryXuehouStrangeDeath = 174;

		public const short SectMainStoryXuehouOldManAppears = 175;

		public const short SectMainStoryXuehouOldManReturns = 176;

		public const short SectMainStoryXuehouOnBloodBlock = 177;

		public const short SectMainStoryXuehouOldManAttacks = 178;

		public const short SectMainStoryXuehouHarmoniousTaiwu = 179;

		public const short SectMainStoryXuehouFeedJixi = 180;

		public const short SectMainStoryXuehouMythInVillage = 181;

		public const short SectMainStoryXuehouProtectJixi = 182;

		public const short SectMainStoryXuehouJixiAskForFood = 183;

		public const short SectMainStoryXuehouJixiFeedChicken = 184;

		public const short SectMainStoryXuehouJixiKills = 185;

		public const short SectMainStoryXuehouVillageWork = 186;

		public const short SectMainStoryXuehouFinale = 187;

		public const short SectMainStoryXuehouProsperous = 188;

		public const short SectMainStoryXuehouFailing = 189;

		public const short SectMainStoryShaolinTowerFalling = 190;

		public const short SectMainStoryShaolinTwoMonksTalk = 191;

		public const short SectMainStoryShaolinDreamFirst = 192;

		public const short SectMainStoryShaolinLearning = 193;

		public const short SectMainStoryShaolinNotEnough = 194;

		public const short SectMainStoryShaolinChallenge = 195;

		public const short SectMainStoryShaolinEndChallenge = 196;

		public const short SectMainStoryShaolinNeverLearnChallenge = 197;

		public const short SectMainStoryShaolinProsperous = 198;

		public const short SectMainStoryShaolinFailing = 199;

		public const short SectMainStoryXuannvPrologue = 200;

		public const short SectMainStoryXuannvWithSister = 201;

		public const short SectMainStoryXuannvReincarnationDeath = 202;

		public const short SectMainStoryXuannvProsperous = 203;

		public const short SectMainStoryXuannvFailing = 204;

		public const short SectMainStoryWudangChat = 205;

		public const short SectMainStoryWudangRequest = 206;

		public const short SectMainStoryWudangSeekSite = 207;

		public const short SectMainStoryWudangProsperous = 208;

		public const short SectMainStoryWudangFailing = 209;

		public const short SectMainStoryYuanshanInfectedCharacterAttack = 210;

		public const short SectMainStoryYuanshanDisciplesInfected = 211;

		public const short SectMainStoryYuanshanLastMonsterAppear = 212;

		public const short SectMainStoryYuanshanProsperous = 213;

		public const short SectMainStoryShixiangEnemyAttack = 214;

		public const short SectMainStoryShixiangLetterFrom = 215;

		public const short SectMainStoryShixiangNotLetter = 216;

		public const short SectMainStoryShixiangDuel = 217;

		public const short SectMainStoryShixiangFailing = 218;

		public const short SectMainStoryJingangPeopleSuffering = 219;

		public const short SectMainStoryJingangAttack = 220;

		public const short SectMainStoryJingangMonkMurdered = 221;

		public const short SectMainStoryJingangExorcism = 222;

		public const short SectMainStoryJingangGhostAppears = 223;

		public const short SectMainStoryJingangHearsay = 224;

		public const short SectMainStoryJingangProsperous = 225;

		public const short SectMainStoryJingangFailing = 226;

		public const short SectMainStoryWuxianPoisonousWug = 227;

		public const short SectMainStoryWuxianProsperous = 228;

		public const short SectMainStoryWuxianFailing0 = 229;

		public const short SectMainStoryWuxianFailing1 = 230;

		public const short SectMainStoryWuxianStrangeThings = 231;

		public const short SectMainStoryWuxianPoison = 232;

		public const short SectMainStoryWuxianAssault = 233;

		public const short SectMainStoryEmeiProsperous = 234;

		public const short SectMainStoryEmeiFailing = 235;

		public const short SectMainStoryJieqingAssassinationPlot = 236;

		public const short SectMainStoryJieqingAssassinationGeneral = 237;

		public const short SectMainStoryJieqingProsperous = 238;

		public const short SectMainStoryJieqingFailing = 239;

		public const short SuicideToken = 240;

		public const short SectMainStoryXuehouEmptyGrave = 241;

		public const short SectMainStoryXuehouLookingForTaiwu = 242;

		public const short SectMainStoryXuehouComing = 243;

		public const short SectMainStoryRanshanPaperCraneFromYufuFaction = 244;

		public const short SectMainStoryRanshanPaperCraneFromShenjianFaction = 245;

		public const short SectMainStoryRanshanPaperCraneFromYinyangFaction = 246;

		public const short SectMainStoryRanshanThreeApprentice = 247;

		public const short SectMainStoryRanshanHuajuStory = 248;

		public const short SectMainStoryRanshanXuanzhiStory = 249;

		public const short SectMainStoryRanshanYingjiaoStory = 250;

		public const short SectMainStoryRanshanThreeApprenticeRequest = 251;

		public const short SectMainStoryRanshanThreeApprenticeReturn = 252;

		public const short SectMainStoryRanshanThreeFactionCompetetion = 253;

		public const short SectMainStoryRanshanProsperous = 254;

		public const short SectMainStoryRanshanFailing = 255;

		public const short SectMainStoryShaolinDreamOfReadingSutra = 256;

		public const short SectMainStoryShaolinDreamOfNewTaiwu = 257;

		public const short SectMainStoryShaolinEnlightenment = 258;

		public const short SectMainStoryShaolinNotEnoughCommon = 259;

		public const short SectMainStoryShixiangRequestBook = 260;

		public const short SectMainStoryShixiangRequestLifeSkill = 261;

		public const short SectMainStoryShixiangGoodNews = 262;

		public const short SectMainStoryShixiangProsperous = 263;

		public const short SectMainStoryShaolinChallengeCommon = 264;

		public const short SectMainStoryShaolinEndChallengeCommon = 265;

		public const short SectMainStoryShaolinNeverLearnChallengeCommon = 266;

		public const short SectMainStoryShixiangLetterFrom2 = 267;

		public const short SectMainStoryShixiangGoodNews2 = 268;

		public const short SectMainStoryShixiangEnemyAttack2 = 269;

		public const short SectMainStoryShixiangStrange = 270;

		public const short SectMainStoryShixiangEnemyAttack3 = 271;

		public const short SectMainStoryWudangProtectHeavenlyTree = 272;

		public const short SectMainStoryWudangHeavenlyTreeDestroyed = 273;

		public const short SectMainStoryWudangGiftsReceived = 274;

		public const short SectMainStoryWudangMeetingImmortal = 275;

		public const short SectMainStoryWudangGuardHeavenlyTree = 276;

		public const short SectMainStoryWudangGiftsReceived2 = 277;

		public const short SectMainStoryXuannvLetter = 278;

		public const short SectMainStoryXuannvHealing = 279;

		public const short SectMainStoryXuannvDeadMessage = 280;

		public const short SectMainStoryXuannvMirrorDream = 281;

		public const short SectMainStoryXuannvReincarnationDeath2 = 282;

		public const short SectMainStoryXuannvStrangeMoan = 283;

		public const short SectMainStoryXuannvFirstTrack = 284;

		public const short SectMainStoryXuannvMeetJuner = 285;

		public const short SectMainStoryWudangHeavenlyTreeDestroyed2 = 286;

		public const short MirrorCreatedImpostureXiangshuInfected = 287;

		public const short SectMainStoryWudangProtectHeavenlyTree2 = 288;

		public const short CrossArchiveReunionWithAcquaintance = 289;

		public const short TeachCombatSkill = 290;

		public const short Pregnant = 291;

		public const short TamingCarriers = 292;

		public const short FiveLoongLetterFromTaiwuVillage = 293;

		public const short JiaoGrowold = 294;

		public const short DLCLoongRidingEffectQiuniu = 295;

		public const short DLCLoongRidingEffectYazi = 296;

		public const short DLCLoongRidingEffectChaofeng = 297;

		public const short DLCLoongRidingEffectPulao = 298;

		public const short DLCLoongRidingEffectSuanni = 299;

		public const short DLCLoongRidingEffectBaxia = 300;

		public const short DLCLoongRidingEffectBian = 301;

		public const short DLCLoongRidingEffectFuxi = 302;

		public const short DLCLoongRidingEffectChiwen = 303;

		public const short MinionLoongAttack = 304;

		public const short DLCLoongJiaoGrowUp = 305;

		public const short SectMainStoryWuxianGiftsReceived = 306;

		public const short SectMainStoryJingangVisitorsArrive = 307;

		public const short SectMainStoryJingangLettersFromJingang = 308;

		public const short SectMainStoryJingangPiety = 309;

		public const short SectMainStoryJingangSutraSecrets = 310;

		public const short SectMainStoryJingangRitualsInDream = 311;

		public const short SectMainStoryJingangSutraDisappears = 312;

		public const short SectMainStoryJingangResidentsSufferingContinues = 313;

		public const short SectMainStoryJingangReincarnation = 314;

		public const short SectMainStoryJingangGhostVanishes = 315;

		public const short SectMainStoryWuxianMiaoWoman = 316;

		public const short SectMainStoryRanshanDragonGate = 317;

		public const short SectMainStoryRanshanMessage = 318;

		public const short SectMainStoryRanshanAfterQinglang = 319;

		public const short SectMainStoryRanshanSanshiLeave = 320;

		public const short SectMainStoryBaihuaEndenmic = 321;

		public const short SectMainStoryBaihuaDreamAboutPastFirst = 322;

		public const short SectMainStoryBaihuaDreamAboutPastLast = 323;

		public const short SectMainStoryBaihuaMelanoArrived = 324;

		public const short SectMainStoryBaihuaProsperous = 325;

		public const short SectMainStoryBaihuaFailing = 326;

		public const short SectMainStoryBaihuaLeukoKills = 327;

		public const short MerchantVisit = 328;

		public const short ToRepayKindness = 329;

		public const short SectMainStoryBaihuaAmbushLeuko = 330;

		public const short SectMainStoryBaihuaMelanoKills = 331;

		public const short SectMainStoryBaihuaAmbushMelano = 332;

		public const short SectMainStoryBaihuaLeukoAssistsMelano = 333;

		public const short SectMainStoryBaihuaMelanoAssistsLeuko = 334;

		public const short SectMainStoryBaihuaManicAttack = 335;

		public const short SectMainStoryBaihuaAnonymReturns = 336;

		public const short SectMainStoryBaihuaGifts = 337;

		public const short SectMainStoryBaihuaMelanoPlay = 338;

		public const short SectMainStoryBaihuaLeukoPlay = 339;

		public const short SectMainStoryBaihuaLeukoMelanoPlay = 340;

		public const short SectMainStoryFulongDiasterAppear = 341;

		public const short SectMainStoryFulongShadow = 342;

		public const short SectMainStoryFulongLazuliLetter = 343;

		public const short SectMainStoryFulongProsperous = 344;

		public const short SectMainStoryFulongFailing = 345;

		public const short HuntCriminal = 346;

		public const short SentenceCompleted = 347;

		public const short SectMainStoryFulongRobTaiwu = 348;

		public const short SectMainStoryFulongInterfereRobbery = 349;

		public const short SectMainStoryFulongProtect = 350;

		public const short SectMainStoryFulongFireFighting = 351;

		public const short SectMainStoryFulongAftermath = 352;

		public const short AdviseHealDisorderOfQi = 353;

		public const short AdviseHealHealth = 354;

		public const short TaiWuVillagerClothing = 355;

		public const short HuntCriminalTaiwu = 356;

		public const short SectMainStoryZhujianHeir = 357;

		public const short SectMainStoryZhujianHazyRain = 358;

		public const short SectMainStoryZhujianTongshengSpeaks = 359;

		public const short SectMainStoryZhujianHuichuntang = 360;

		public const short SectMainStoryZhujianProsperous = 361;

		public const short SectMainStoryZhujianFailing = 362;

		public const short JieQingPunishmentAssassin = 363;

		public const short TaiwuBeHuntedHunterDie = 364;

		public const short WardOffXiangshuProtection = 365;

		public const short ProfessionDukeReceiveCricket = 366;

		public const short CricketInDreamTaiwuPartnerPregnant = 367;

		public const short SectMainStoryShaolinDharmaCave = 368;

		public const short TaiwuVillageStoneClaimed = 369;

		public const short TaiwuVillagerAdoptOrphan = 370;

		public const short SectMainStoryRemakeEmeiPrologue = 371;

		public const short SectMainStoryRemakeEmeiMessage = 372;

		public const short SectMainStoryRemakeEmeiHomocideCase = 373;

		public const short SectMainStoryRemakeEmeiChanges = 374;

		public const short SectMainStoryRemakeEmeiMyth = 375;

		public const short SectMainStoryRemakeEmeiFinale = 376;

		public const short SectMainStoryRemakeEmeiProsperous = 377;

		public const short SectMainStoryRemakeEmeiFailing = 378;

		public const short SectMainStoryRemakeYuanshanPrison = 379;

		public const short SectMainStoryRemakeYuanshanFailing = 380;

		public const short NormalHeavenlyTreeDestroyed = 381;

		public const short NormalGuardHeavenlyTree = 382;

		public const short SectMainStoryRemakeYuanshanMoon = 383;

		public const short DLCYearOfHorseCloth = 384;
	}

	public static class DefValue
	{
		public static MonthlyEventItem ReadingEvent => Instance[(short)0];

		public static MonthlyEventItem TaiwuDeath => Instance[(short)1];

		public static MonthlyEventItem AreaTotallyDestoryed => Instance[(short)2];

		public static MonthlyEventItem TaiwuInfected => Instance[(short)3];

		public static MonthlyEventItem TaiwuInfectedPartially => Instance[(short)4];

		public static MonthlyEventItem RandomEnemyAttack => Instance[(short)5];

		public static MonthlyEventItem RandomAnimalAttack => Instance[(short)6];

		public static MonthlyEventItem RandomRighteousAttack => Instance[(short)7];

		public static MonthlyEventItem InfectedCharacterAttack => Instance[(short)8];

		public static MonthlyEventItem HumanSkeletonAttack => Instance[(short)9];

		public static MonthlyEventItem CricketInDream => Instance[(short)10];

		public static MonthlyEventItem GiveBirthToCricketTaiwu => Instance[(short)11];

		public static MonthlyEventItem GiveBirthToCricketWife => Instance[(short)12];

		public static MonthlyEventItem PrenatalEducationTaiwu => Instance[(short)13];

		public static MonthlyEventItem AbortionTaiwu => Instance[(short)14];

		public static MonthlyEventItem LoseFetusWife => Instance[(short)15];

		public static MonthlyEventItem MotherFetusBothDieTaiwu => Instance[(short)16];

		public static MonthlyEventItem MotherFetusBothDieWife => Instance[(short)17];

		public static MonthlyEventItem DystociaLoseFetusTaiwu => Instance[(short)18];

		public static MonthlyEventItem DystociaLoseFetusWife => Instance[(short)19];

		public static MonthlyEventItem HaveChildBoyTaiwu => Instance[(short)20];

		public static MonthlyEventItem HaveChildGirlTaiwu => Instance[(short)21];

		public static MonthlyEventItem HaveChildBoyWife => Instance[(short)22];

		public static MonthlyEventItem HaveChildGirlWife => Instance[(short)23];

		public static MonthlyEventItem DystociaButHaveChildBoyTaiwu => Instance[(short)24];

		public static MonthlyEventItem DystociaButHaveChildGirlTaiwu => Instance[(short)25];

		public static MonthlyEventItem DystociaButHaveChildBoyWife => Instance[(short)26];

		public static MonthlyEventItem DystociaButHaveChildGirlWife => Instance[(short)27];

		public static MonthlyEventItem DystociaAndHaveChildBoyTaiwu => Instance[(short)28];

		public static MonthlyEventItem DystociaAndHaveChildGirlTaiwu => Instance[(short)29];

		public static MonthlyEventItem DystociaAndHaveChildBoyWife => Instance[(short)30];

		public static MonthlyEventItem DystociaAndHaveChildGirlWife => Instance[(short)31];

		public static MonthlyEventItem AbandonedBabyInVilliage => Instance[(short)32];

		public static MonthlyEventItem ChildZhuazhou => Instance[(short)33];

		public static MonthlyEventItem TeachChild => Instance[(short)34];

		public static MonthlyEventItem ReachAdulthood => Instance[(short)35];

		public static MonthlyEventItem CaptiveHaveChild => Instance[(short)36];

		public static MonthlyEventItem CaptiveBecomeEnemy => Instance[(short)37];

		public static MonthlyEventItem GroupGetMarried => Instance[(short)38];

		public static MonthlyEventItem SpringMarket => Instance[(short)39];

		public static MonthlyEventItem SummerTownCompetition => Instance[(short)40];

		public static MonthlyEventItem AutumnCricketContest => Instance[(short)41];

		public static MonthlyEventItem WinterLifeCompetition => Instance[(short)42];

		public static MonthlyEventItem MakeEnemy => Instance[(short)43];

		public static MonthlyEventItem SeverEnemy => Instance[(short)44];

		public static MonthlyEventItem Adore => Instance[(short)45];

		public static MonthlyEventItem Confess => Instance[(short)46];

		public static MonthlyEventItem Breakup => Instance[(short)47];

		public static MonthlyEventItem ProposeMarriage => Instance[(short)48];

		public static MonthlyEventItem BecomeFriend => Instance[(short)49];

		public static MonthlyEventItem SeverFriendship => Instance[(short)50];

		public static MonthlyEventItem BecomeSwornBrotherOrSister => Instance[(short)51];

		public static MonthlyEventItem SeverSwornBrotherhood => Instance[(short)52];

		public static MonthlyEventItem GetAdoptedByFather => Instance[(short)53];

		public static MonthlyEventItem GetAdoptedByMother => Instance[(short)54];

		public static MonthlyEventItem AdoptSon => Instance[(short)55];

		public static MonthlyEventItem AdoptDaughter => Instance[(short)56];

		public static MonthlyEventItem Die => Instance[(short)57];

		public static MonthlyEventItem EscapeFromPrison => Instance[(short)58];

		public static MonthlyEventItem AppointmentCancelled => Instance[(short)59];

		public static MonthlyEventItem RevengeAttack => Instance[(short)60];

		public static MonthlyEventItem AskProtectByRevengeAttack => Instance[(short)61];

		public static MonthlyEventItem CatchEnemyPoison => Instance[(short)62];

		public static MonthlyEventItem EnemyPoisonAndEscape => Instance[(short)63];

		public static MonthlyEventItem CatchEnemyPlotHarm => Instance[(short)64];

		public static MonthlyEventItem EnemyPlotHarmAndEscape => Instance[(short)65];

		public static MonthlyEventItem RequestHealOuterInjuryByItem => Instance[(short)66];

		public static MonthlyEventItem RequestHealOuterInjuryByResource => Instance[(short)67];

		public static MonthlyEventItem RequestHealInnerInjuryByItem => Instance[(short)68];

		public static MonthlyEventItem RequestHealInnerInjuryByResource => Instance[(short)69];

		public static MonthlyEventItem RequestHealPoisonByItem => Instance[(short)70];

		public static MonthlyEventItem RequestHealPoisonByResource => Instance[(short)71];

		public static MonthlyEventItem RequestHealth => Instance[(short)72];

		public static MonthlyEventItem RequestHealDisorderOfQi => Instance[(short)73];

		public static MonthlyEventItem RequestNeili => Instance[(short)74];

		public static MonthlyEventItem RequestKillWug => Instance[(short)75];

		public static MonthlyEventItem RequestFood => Instance[(short)76];

		public static MonthlyEventItem RequestTeaWine => Instance[(short)77];

		public static MonthlyEventItem RequestResource => Instance[(short)78];

		public static MonthlyEventItem RequestItem => Instance[(short)79];

		public static MonthlyEventItem RequestRepairItem => Instance[(short)80];

		public static MonthlyEventItem RequestAddPoisonToItem => Instance[(short)81];

		public static MonthlyEventItem RequestInstructionOnLifeSkill => Instance[(short)82];

		public static MonthlyEventItem RequestInstructionOnCombatSkill => Instance[(short)83];

		public static MonthlyEventItem RequestInstructionOnReadingLifeSkill => Instance[(short)84];

		public static MonthlyEventItem RequestInstructionOnReadingCombatSkill => Instance[(short)85];

		public static MonthlyEventItem RequestInstructionOnBreakout => Instance[(short)86];

		public static MonthlyEventItem RequestPlayCombat => Instance[(short)87];

		public static MonthlyEventItem RequestNormalCombat => Instance[(short)88];

		public static MonthlyEventItem RequestLifeSkillBattle => Instance[(short)89];

		public static MonthlyEventItem RequestCricketBattle => Instance[(short)90];

		public static MonthlyEventItem RescueKidnappedCharacterSecretlyButBeCaught => Instance[(short)91];

		public static MonthlyEventItem RescueKidnappedCharacterSecretlyAndEscape => Instance[(short)92];

		public static MonthlyEventItem RescueKidnappedCharacterWithWit => Instance[(short)93];

		public static MonthlyEventItem RescueKidnappedCharacterWithForce => Instance[(short)94];

		public static MonthlyEventItem StealResourceButBeCaught => Instance[(short)95];

		public static MonthlyEventItem StealResourceAndEscape => Instance[(short)96];

		public static MonthlyEventItem ScamResource => Instance[(short)97];

		public static MonthlyEventItem RobResource => Instance[(short)98];

		public static MonthlyEventItem StealItemButBeCaught => Instance[(short)99];

		public static MonthlyEventItem StealItemAndEscape => Instance[(short)100];

		public static MonthlyEventItem ScamItem => Instance[(short)101];

		public static MonthlyEventItem RobItem => Instance[(short)102];

		public static MonthlyEventItem StealLifeSkillButBeCaught => Instance[(short)103];

		public static MonthlyEventItem StealLifeSkillAndEscape => Instance[(short)104];

		public static MonthlyEventItem ScamLifeSkill => Instance[(short)105];

		public static MonthlyEventItem StealCombatSkillButBeCaught => Instance[(short)106];

		public static MonthlyEventItem StealCombatSkillAndEscape => Instance[(short)107];

		public static MonthlyEventItem ScamCombatSkill => Instance[(short)108];

		public static MonthlyEventItem AdviseExtendFavours => Instance[(short)109];

		public static MonthlyEventItem AdviseWinPeopleSupport => Instance[(short)110];

		public static MonthlyEventItem AdviseMerchantFavor => Instance[(short)111];

		public static MonthlyEventItem AdviseTeaWine => Instance[(short)112];

		public static MonthlyEventItem AdviseSales => Instance[(short)113];

		public static MonthlyEventItem AdviseHealInjury => Instance[(short)114];

		public static MonthlyEventItem AdviseHealPoison => Instance[(short)115];

		public static MonthlyEventItem AdviseRepairItem => Instance[(short)116];

		public static MonthlyEventItem AdviseBarb => Instance[(short)117];

		public static MonthlyEventItem AskForMoney => Instance[(short)118];

		public static MonthlyEventItem WulinConferenceTaiwuAbsent => Instance[(short)119];

		public static MonthlyEventItem WulinConferenceAskForHelp => Instance[(short)120];

		public static MonthlyEventItem TaiwuVillageBeDestoryed => Instance[(short)121];

		public static MonthlyEventItem ForeverLoverBePunished => Instance[(short)122];

		public static MonthlyEventItem VillageWoodenManByMonv => Instance[(short)123];

		public static MonthlyEventItem VillageWoodenManByDayueYaochang => Instance[(short)124];

		public static MonthlyEventItem VillageWoodenManByJiuhan => Instance[(short)125];

		public static MonthlyEventItem VillageWoodenManByJinHuanger => Instance[(short)126];

		public static MonthlyEventItem VillageWoodenManByYiYihou => Instance[(short)127];

		public static MonthlyEventItem VillageWoodenManByWeiQi => Instance[(short)128];

		public static MonthlyEventItem VillageWoodenManByYixiang => Instance[(short)129];

		public static MonthlyEventItem VillageWoodenManByXuefeng => Instance[(short)130];

		public static MonthlyEventItem VillageWoodenManByShuFang => Instance[(short)131];

		public static MonthlyEventItem TaiwuNotAttendingWedding => Instance[(short)132];

		public static MonthlyEventItem TaiwuAlreadyMarried => Instance[(short)133];

		public static MonthlyEventItem ChallengeForLegendaryBook => Instance[(short)134];

		public static MonthlyEventItem RequestLegendaryBook => Instance[(short)135];

		public static MonthlyEventItem ExchangeLegendaryBookByMoney => Instance[(short)136];

		public static MonthlyEventItem ExchangeLegendaryBookByAuthority => Instance[(short)137];

		public static MonthlyEventItem ExchangeLegendaryBookByExperience => Instance[(short)138];

		public static MonthlyEventItem StealLegendaryBookAndEscape => Instance[(short)139];

		public static MonthlyEventItem StealLegendaryBookGotCaught => Instance[(short)140];

		public static MonthlyEventItem ScamLegendaryBook => Instance[(short)141];

		public static MonthlyEventItem RobLegendaryBook => Instance[(short)142];

		public static MonthlyEventItem LegendaryBookShockedAttack => Instance[(short)143];

		public static MonthlyEventItem LegendaryBookInsaneAttack => Instance[(short)144];

		public static MonthlyEventItem LegendaryBookConsumedAttack => Instance[(short)145];

		public static MonthlyEventItem SwordTombGetStronger => Instance[(short)146];

		public static MonthlyEventItem SwordTombBackToNormal => Instance[(short)147];

		public static MonthlyEventItem FightForNewLegendaryBook => Instance[(short)148];

		public static MonthlyEventItem FightForLegendaryBookAbandoned => Instance[(short)149];

		public static MonthlyEventItem FightForLegendaryBookOwnerDie => Instance[(short)150];

		public static MonthlyEventItem FightForLegendaryBookOwnerConsumed => Instance[(short)151];

		public static MonthlyEventItem DateWithLoverEveryday => Instance[(short)152];

		public static MonthlyEventItem HappyBirthdayTaiwu => Instance[(short)153];

		public static MonthlyEventItem LoveAnniversary => Instance[(short)154];

		public static MonthlyEventItem NeglectedLover => Instance[(short)155];

		public static MonthlyEventItem LoverBecomeJealous => Instance[(short)156];

		public static MonthlyEventItem LoversBecomeJealousAndViolent => Instance[(short)157];

		public static MonthlyEventItem PregnancyWithLover => Instance[(short)158];

		public static MonthlyEventItem BeggerSkill2TargetUnavailable => Instance[(short)159];

		public static MonthlyEventItem BeggarSkill2TargetBrought => Instance[(short)160];

		public static MonthlyEventItem BeggarSkill2TargetDeadAndMissing => Instance[(short)161];

		public static MonthlyEventItem BeggarSkill2TargetDead => Instance[(short)162];

		public static MonthlyEventItem BeggarSkill2TargetNoneExistent => Instance[(short)163];

		public static MonthlyEventItem TaiwuTribulation => Instance[(short)164];

		public static MonthlyEventItem TaiwuComingSuccess => Instance[(short)165];

		public static MonthlyEventItem TaiwuComingDefeated => Instance[(short)166];

		public static MonthlyEventItem TaiwuFreeAndunFettered => Instance[(short)167];

		public static MonthlyEventItem SectMainStoryKongsangTargetFound => Instance[(short)168];

		public static MonthlyEventItem SectMainStoryKongsangAdventure => Instance[(short)169];

		public static MonthlyEventItem SectMainStoryKongsangProsperous => Instance[(short)170];

		public static MonthlyEventItem SectMainStoryKongsangFailing => Instance[(short)171];

		public static MonthlyEventItem SectMainStoryXuehouGraveDigging => Instance[(short)172];

		public static MonthlyEventItem SectMainStoryXuehouGraveDiggingNormal => Instance[(short)173];

		public static MonthlyEventItem SectMainStoryXuehouStrangeDeath => Instance[(short)174];

		public static MonthlyEventItem SectMainStoryXuehouOldManAppears => Instance[(short)175];

		public static MonthlyEventItem SectMainStoryXuehouOldManReturns => Instance[(short)176];

		public static MonthlyEventItem SectMainStoryXuehouOnBloodBlock => Instance[(short)177];

		public static MonthlyEventItem SectMainStoryXuehouOldManAttacks => Instance[(short)178];

		public static MonthlyEventItem SectMainStoryXuehouHarmoniousTaiwu => Instance[(short)179];

		public static MonthlyEventItem SectMainStoryXuehouFeedJixi => Instance[(short)180];

		public static MonthlyEventItem SectMainStoryXuehouMythInVillage => Instance[(short)181];

		public static MonthlyEventItem SectMainStoryXuehouProtectJixi => Instance[(short)182];

		public static MonthlyEventItem SectMainStoryXuehouJixiAskForFood => Instance[(short)183];

		public static MonthlyEventItem SectMainStoryXuehouJixiFeedChicken => Instance[(short)184];

		public static MonthlyEventItem SectMainStoryXuehouJixiKills => Instance[(short)185];

		public static MonthlyEventItem SectMainStoryXuehouVillageWork => Instance[(short)186];

		public static MonthlyEventItem SectMainStoryXuehouFinale => Instance[(short)187];

		public static MonthlyEventItem SectMainStoryXuehouProsperous => Instance[(short)188];

		public static MonthlyEventItem SectMainStoryXuehouFailing => Instance[(short)189];

		public static MonthlyEventItem SectMainStoryShaolinTowerFalling => Instance[(short)190];

		public static MonthlyEventItem SectMainStoryShaolinTwoMonksTalk => Instance[(short)191];

		public static MonthlyEventItem SectMainStoryShaolinDreamFirst => Instance[(short)192];

		public static MonthlyEventItem SectMainStoryShaolinLearning => Instance[(short)193];

		public static MonthlyEventItem SectMainStoryShaolinNotEnough => Instance[(short)194];

		public static MonthlyEventItem SectMainStoryShaolinChallenge => Instance[(short)195];

		public static MonthlyEventItem SectMainStoryShaolinEndChallenge => Instance[(short)196];

		public static MonthlyEventItem SectMainStoryShaolinNeverLearnChallenge => Instance[(short)197];

		public static MonthlyEventItem SectMainStoryShaolinProsperous => Instance[(short)198];

		public static MonthlyEventItem SectMainStoryShaolinFailing => Instance[(short)199];

		public static MonthlyEventItem SectMainStoryXuannvPrologue => Instance[(short)200];

		public static MonthlyEventItem SectMainStoryXuannvWithSister => Instance[(short)201];

		public static MonthlyEventItem SectMainStoryXuannvReincarnationDeath => Instance[(short)202];

		public static MonthlyEventItem SectMainStoryXuannvProsperous => Instance[(short)203];

		public static MonthlyEventItem SectMainStoryXuannvFailing => Instance[(short)204];

		public static MonthlyEventItem SectMainStoryWudangChat => Instance[(short)205];

		public static MonthlyEventItem SectMainStoryWudangRequest => Instance[(short)206];

		public static MonthlyEventItem SectMainStoryWudangSeekSite => Instance[(short)207];

		public static MonthlyEventItem SectMainStoryWudangProsperous => Instance[(short)208];

		public static MonthlyEventItem SectMainStoryWudangFailing => Instance[(short)209];

		public static MonthlyEventItem SectMainStoryYuanshanInfectedCharacterAttack => Instance[(short)210];

		public static MonthlyEventItem SectMainStoryYuanshanDisciplesInfected => Instance[(short)211];

		public static MonthlyEventItem SectMainStoryYuanshanLastMonsterAppear => Instance[(short)212];

		public static MonthlyEventItem SectMainStoryYuanshanProsperous => Instance[(short)213];

		public static MonthlyEventItem SectMainStoryShixiangEnemyAttack => Instance[(short)214];

		public static MonthlyEventItem SectMainStoryShixiangLetterFrom => Instance[(short)215];

		public static MonthlyEventItem SectMainStoryShixiangNotLetter => Instance[(short)216];

		public static MonthlyEventItem SectMainStoryShixiangDuel => Instance[(short)217];

		public static MonthlyEventItem SectMainStoryShixiangFailing => Instance[(short)218];

		public static MonthlyEventItem SectMainStoryJingangPeopleSuffering => Instance[(short)219];

		public static MonthlyEventItem SectMainStoryJingangAttack => Instance[(short)220];

		public static MonthlyEventItem SectMainStoryJingangMonkMurdered => Instance[(short)221];

		public static MonthlyEventItem SectMainStoryJingangExorcism => Instance[(short)222];

		public static MonthlyEventItem SectMainStoryJingangGhostAppears => Instance[(short)223];

		public static MonthlyEventItem SectMainStoryJingangHearsay => Instance[(short)224];

		public static MonthlyEventItem SectMainStoryJingangProsperous => Instance[(short)225];

		public static MonthlyEventItem SectMainStoryJingangFailing => Instance[(short)226];

		public static MonthlyEventItem SectMainStoryWuxianPoisonousWug => Instance[(short)227];

		public static MonthlyEventItem SectMainStoryWuxianProsperous => Instance[(short)228];

		public static MonthlyEventItem SectMainStoryWuxianFailing0 => Instance[(short)229];

		public static MonthlyEventItem SectMainStoryWuxianFailing1 => Instance[(short)230];

		public static MonthlyEventItem SectMainStoryWuxianStrangeThings => Instance[(short)231];

		public static MonthlyEventItem SectMainStoryWuxianPoison => Instance[(short)232];

		public static MonthlyEventItem SectMainStoryWuxianAssault => Instance[(short)233];

		public static MonthlyEventItem SectMainStoryEmeiProsperous => Instance[(short)234];

		public static MonthlyEventItem SectMainStoryEmeiFailing => Instance[(short)235];

		public static MonthlyEventItem SectMainStoryJieqingAssassinationPlot => Instance[(short)236];

		public static MonthlyEventItem SectMainStoryJieqingAssassinationGeneral => Instance[(short)237];

		public static MonthlyEventItem SectMainStoryJieqingProsperous => Instance[(short)238];

		public static MonthlyEventItem SectMainStoryJieqingFailing => Instance[(short)239];

		public static MonthlyEventItem SuicideToken => Instance[(short)240];

		public static MonthlyEventItem SectMainStoryXuehouEmptyGrave => Instance[(short)241];

		public static MonthlyEventItem SectMainStoryXuehouLookingForTaiwu => Instance[(short)242];

		public static MonthlyEventItem SectMainStoryXuehouComing => Instance[(short)243];

		public static MonthlyEventItem SectMainStoryRanshanPaperCraneFromYufuFaction => Instance[(short)244];

		public static MonthlyEventItem SectMainStoryRanshanPaperCraneFromShenjianFaction => Instance[(short)245];

		public static MonthlyEventItem SectMainStoryRanshanPaperCraneFromYinyangFaction => Instance[(short)246];

		public static MonthlyEventItem SectMainStoryRanshanThreeApprentice => Instance[(short)247];

		public static MonthlyEventItem SectMainStoryRanshanHuajuStory => Instance[(short)248];

		public static MonthlyEventItem SectMainStoryRanshanXuanzhiStory => Instance[(short)249];

		public static MonthlyEventItem SectMainStoryRanshanYingjiaoStory => Instance[(short)250];

		public static MonthlyEventItem SectMainStoryRanshanThreeApprenticeRequest => Instance[(short)251];

		public static MonthlyEventItem SectMainStoryRanshanThreeApprenticeReturn => Instance[(short)252];

		public static MonthlyEventItem SectMainStoryRanshanThreeFactionCompetetion => Instance[(short)253];

		public static MonthlyEventItem SectMainStoryRanshanProsperous => Instance[(short)254];

		public static MonthlyEventItem SectMainStoryRanshanFailing => Instance[(short)255];

		public static MonthlyEventItem SectMainStoryShaolinDreamOfReadingSutra => Instance[(short)256];

		public static MonthlyEventItem SectMainStoryShaolinDreamOfNewTaiwu => Instance[(short)257];

		public static MonthlyEventItem SectMainStoryShaolinEnlightenment => Instance[(short)258];

		public static MonthlyEventItem SectMainStoryShaolinNotEnoughCommon => Instance[(short)259];

		public static MonthlyEventItem SectMainStoryShixiangRequestBook => Instance[(short)260];

		public static MonthlyEventItem SectMainStoryShixiangRequestLifeSkill => Instance[(short)261];

		public static MonthlyEventItem SectMainStoryShixiangGoodNews => Instance[(short)262];

		public static MonthlyEventItem SectMainStoryShixiangProsperous => Instance[(short)263];

		public static MonthlyEventItem SectMainStoryShaolinChallengeCommon => Instance[(short)264];

		public static MonthlyEventItem SectMainStoryShaolinEndChallengeCommon => Instance[(short)265];

		public static MonthlyEventItem SectMainStoryShaolinNeverLearnChallengeCommon => Instance[(short)266];

		public static MonthlyEventItem SectMainStoryShixiangLetterFrom2 => Instance[(short)267];

		public static MonthlyEventItem SectMainStoryShixiangGoodNews2 => Instance[(short)268];

		public static MonthlyEventItem SectMainStoryShixiangEnemyAttack2 => Instance[(short)269];

		public static MonthlyEventItem SectMainStoryShixiangStrange => Instance[(short)270];

		public static MonthlyEventItem SectMainStoryShixiangEnemyAttack3 => Instance[(short)271];

		public static MonthlyEventItem SectMainStoryWudangProtectHeavenlyTree => Instance[(short)272];

		public static MonthlyEventItem SectMainStoryWudangHeavenlyTreeDestroyed => Instance[(short)273];

		public static MonthlyEventItem SectMainStoryWudangGiftsReceived => Instance[(short)274];

		public static MonthlyEventItem SectMainStoryWudangMeetingImmortal => Instance[(short)275];

		public static MonthlyEventItem SectMainStoryWudangGuardHeavenlyTree => Instance[(short)276];

		public static MonthlyEventItem SectMainStoryWudangGiftsReceived2 => Instance[(short)277];

		public static MonthlyEventItem SectMainStoryXuannvLetter => Instance[(short)278];

		public static MonthlyEventItem SectMainStoryXuannvHealing => Instance[(short)279];

		public static MonthlyEventItem SectMainStoryXuannvDeadMessage => Instance[(short)280];

		public static MonthlyEventItem SectMainStoryXuannvMirrorDream => Instance[(short)281];

		public static MonthlyEventItem SectMainStoryXuannvReincarnationDeath2 => Instance[(short)282];

		public static MonthlyEventItem SectMainStoryXuannvStrangeMoan => Instance[(short)283];

		public static MonthlyEventItem SectMainStoryXuannvFirstTrack => Instance[(short)284];

		public static MonthlyEventItem SectMainStoryXuannvMeetJuner => Instance[(short)285];

		public static MonthlyEventItem SectMainStoryWudangHeavenlyTreeDestroyed2 => Instance[(short)286];

		public static MonthlyEventItem MirrorCreatedImpostureXiangshuInfected => Instance[(short)287];

		public static MonthlyEventItem SectMainStoryWudangProtectHeavenlyTree2 => Instance[(short)288];

		public static MonthlyEventItem CrossArchiveReunionWithAcquaintance => Instance[(short)289];

		public static MonthlyEventItem TeachCombatSkill => Instance[(short)290];

		public static MonthlyEventItem Pregnant => Instance[(short)291];

		public static MonthlyEventItem TamingCarriers => Instance[(short)292];

		public static MonthlyEventItem FiveLoongLetterFromTaiwuVillage => Instance[(short)293];

		public static MonthlyEventItem JiaoGrowold => Instance[(short)294];

		public static MonthlyEventItem DLCLoongRidingEffectQiuniu => Instance[(short)295];

		public static MonthlyEventItem DLCLoongRidingEffectYazi => Instance[(short)296];

		public static MonthlyEventItem DLCLoongRidingEffectChaofeng => Instance[(short)297];

		public static MonthlyEventItem DLCLoongRidingEffectPulao => Instance[(short)298];

		public static MonthlyEventItem DLCLoongRidingEffectSuanni => Instance[(short)299];

		public static MonthlyEventItem DLCLoongRidingEffectBaxia => Instance[(short)300];

		public static MonthlyEventItem DLCLoongRidingEffectBian => Instance[(short)301];

		public static MonthlyEventItem DLCLoongRidingEffectFuxi => Instance[(short)302];

		public static MonthlyEventItem DLCLoongRidingEffectChiwen => Instance[(short)303];

		public static MonthlyEventItem MinionLoongAttack => Instance[(short)304];

		public static MonthlyEventItem DLCLoongJiaoGrowUp => Instance[(short)305];

		public static MonthlyEventItem SectMainStoryWuxianGiftsReceived => Instance[(short)306];

		public static MonthlyEventItem SectMainStoryJingangVisitorsArrive => Instance[(short)307];

		public static MonthlyEventItem SectMainStoryJingangLettersFromJingang => Instance[(short)308];

		public static MonthlyEventItem SectMainStoryJingangPiety => Instance[(short)309];

		public static MonthlyEventItem SectMainStoryJingangSutraSecrets => Instance[(short)310];

		public static MonthlyEventItem SectMainStoryJingangRitualsInDream => Instance[(short)311];

		public static MonthlyEventItem SectMainStoryJingangSutraDisappears => Instance[(short)312];

		public static MonthlyEventItem SectMainStoryJingangResidentsSufferingContinues => Instance[(short)313];

		public static MonthlyEventItem SectMainStoryJingangReincarnation => Instance[(short)314];

		public static MonthlyEventItem SectMainStoryJingangGhostVanishes => Instance[(short)315];

		public static MonthlyEventItem SectMainStoryWuxianMiaoWoman => Instance[(short)316];

		public static MonthlyEventItem SectMainStoryRanshanDragonGate => Instance[(short)317];

		public static MonthlyEventItem SectMainStoryRanshanMessage => Instance[(short)318];

		public static MonthlyEventItem SectMainStoryRanshanAfterQinglang => Instance[(short)319];

		public static MonthlyEventItem SectMainStoryRanshanSanshiLeave => Instance[(short)320];

		public static MonthlyEventItem SectMainStoryBaihuaEndenmic => Instance[(short)321];

		public static MonthlyEventItem SectMainStoryBaihuaDreamAboutPastFirst => Instance[(short)322];

		public static MonthlyEventItem SectMainStoryBaihuaDreamAboutPastLast => Instance[(short)323];

		public static MonthlyEventItem SectMainStoryBaihuaMelanoArrived => Instance[(short)324];

		public static MonthlyEventItem SectMainStoryBaihuaProsperous => Instance[(short)325];

		public static MonthlyEventItem SectMainStoryBaihuaFailing => Instance[(short)326];

		public static MonthlyEventItem SectMainStoryBaihuaLeukoKills => Instance[(short)327];

		public static MonthlyEventItem MerchantVisit => Instance[(short)328];

		public static MonthlyEventItem ToRepayKindness => Instance[(short)329];

		public static MonthlyEventItem SectMainStoryBaihuaAmbushLeuko => Instance[(short)330];

		public static MonthlyEventItem SectMainStoryBaihuaMelanoKills => Instance[(short)331];

		public static MonthlyEventItem SectMainStoryBaihuaAmbushMelano => Instance[(short)332];

		public static MonthlyEventItem SectMainStoryBaihuaLeukoAssistsMelano => Instance[(short)333];

		public static MonthlyEventItem SectMainStoryBaihuaMelanoAssistsLeuko => Instance[(short)334];

		public static MonthlyEventItem SectMainStoryBaihuaManicAttack => Instance[(short)335];

		public static MonthlyEventItem SectMainStoryBaihuaAnonymReturns => Instance[(short)336];

		public static MonthlyEventItem SectMainStoryBaihuaGifts => Instance[(short)337];

		public static MonthlyEventItem SectMainStoryBaihuaMelanoPlay => Instance[(short)338];

		public static MonthlyEventItem SectMainStoryBaihuaLeukoPlay => Instance[(short)339];

		public static MonthlyEventItem SectMainStoryBaihuaLeukoMelanoPlay => Instance[(short)340];

		public static MonthlyEventItem SectMainStoryFulongDiasterAppear => Instance[(short)341];

		public static MonthlyEventItem SectMainStoryFulongShadow => Instance[(short)342];

		public static MonthlyEventItem SectMainStoryFulongLazuliLetter => Instance[(short)343];

		public static MonthlyEventItem SectMainStoryFulongProsperous => Instance[(short)344];

		public static MonthlyEventItem SectMainStoryFulongFailing => Instance[(short)345];

		public static MonthlyEventItem HuntCriminal => Instance[(short)346];

		public static MonthlyEventItem SentenceCompleted => Instance[(short)347];

		public static MonthlyEventItem SectMainStoryFulongRobTaiwu => Instance[(short)348];

		public static MonthlyEventItem SectMainStoryFulongInterfereRobbery => Instance[(short)349];

		public static MonthlyEventItem SectMainStoryFulongProtect => Instance[(short)350];

		public static MonthlyEventItem SectMainStoryFulongFireFighting => Instance[(short)351];

		public static MonthlyEventItem SectMainStoryFulongAftermath => Instance[(short)352];

		public static MonthlyEventItem AdviseHealDisorderOfQi => Instance[(short)353];

		public static MonthlyEventItem AdviseHealHealth => Instance[(short)354];

		public static MonthlyEventItem TaiWuVillagerClothing => Instance[(short)355];

		public static MonthlyEventItem HuntCriminalTaiwu => Instance[(short)356];

		public static MonthlyEventItem SectMainStoryZhujianHeir => Instance[(short)357];

		public static MonthlyEventItem SectMainStoryZhujianHazyRain => Instance[(short)358];

		public static MonthlyEventItem SectMainStoryZhujianTongshengSpeaks => Instance[(short)359];

		public static MonthlyEventItem SectMainStoryZhujianHuichuntang => Instance[(short)360];

		public static MonthlyEventItem SectMainStoryZhujianProsperous => Instance[(short)361];

		public static MonthlyEventItem SectMainStoryZhujianFailing => Instance[(short)362];

		public static MonthlyEventItem JieQingPunishmentAssassin => Instance[(short)363];

		public static MonthlyEventItem TaiwuBeHuntedHunterDie => Instance[(short)364];

		public static MonthlyEventItem WardOffXiangshuProtection => Instance[(short)365];

		public static MonthlyEventItem ProfessionDukeReceiveCricket => Instance[(short)366];

		public static MonthlyEventItem CricketInDreamTaiwuPartnerPregnant => Instance[(short)367];

		public static MonthlyEventItem SectMainStoryShaolinDharmaCave => Instance[(short)368];

		public static MonthlyEventItem TaiwuVillageStoneClaimed => Instance[(short)369];

		public static MonthlyEventItem TaiwuVillagerAdoptOrphan => Instance[(short)370];

		public static MonthlyEventItem SectMainStoryRemakeEmeiPrologue => Instance[(short)371];

		public static MonthlyEventItem SectMainStoryRemakeEmeiMessage => Instance[(short)372];

		public static MonthlyEventItem SectMainStoryRemakeEmeiHomocideCase => Instance[(short)373];

		public static MonthlyEventItem SectMainStoryRemakeEmeiChanges => Instance[(short)374];

		public static MonthlyEventItem SectMainStoryRemakeEmeiMyth => Instance[(short)375];

		public static MonthlyEventItem SectMainStoryRemakeEmeiFinale => Instance[(short)376];

		public static MonthlyEventItem SectMainStoryRemakeEmeiProsperous => Instance[(short)377];

		public static MonthlyEventItem SectMainStoryRemakeEmeiFailing => Instance[(short)378];

		public static MonthlyEventItem SectMainStoryRemakeYuanshanPrison => Instance[(short)379];

		public static MonthlyEventItem SectMainStoryRemakeYuanshanFailing => Instance[(short)380];

		public static MonthlyEventItem NormalHeavenlyTreeDestroyed => Instance[(short)381];

		public static MonthlyEventItem NormalGuardHeavenlyTree => Instance[(short)382];

		public static MonthlyEventItem SectMainStoryRemakeYuanshanMoon => Instance[(short)383];

		public static MonthlyEventItem DLCYearOfHorseCloth => Instance[(short)384];
	}

	public static MonthlyEvent Instance = new MonthlyEvent();

	private readonly HashSet<string> RequiredFields = new HashSet<string> { "TemplateId", "Name", "Event", "Icon", "Desc", "MergeableParameters" };

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
		_dataArray.Add(new MonthlyEventItem(0, 0, EMonthlyEventType.NormalEvent, null, "sp_monthlyevent_2", 1, new string[7] { "ItemKey", "", "", "", "", "", "" }, null, 0, node: false));
		_dataArray.Add(new MonthlyEventItem(1, 2, EMonthlyEventType.LockedEvent, "24b66f5e-cd47-486c-ad8f-6e069bd8dd71", "sp_monthlyevent_0", 3, new string[7] { "Character", "Location", "", "", "", "", "" }, null, 0, node: false));
		_dataArray.Add(new MonthlyEventItem(2, 4, EMonthlyEventType.LockedEvent, "84d406db-8da7-4128-b52b-92ede33eff20", "sp_monthlyevent_0", 5, new string[7] { "Character", "", "", "", "", "", "" }, null, 0, node: false));
		_dataArray.Add(new MonthlyEventItem(3, 6, EMonthlyEventType.LockedEvent, "24b66f5e-cd47-486c-ad8f-6e069bd8dd71", "sp_monthlyevent_1", 7, new string[7] { "Character", "Location", "", "", "", "", "" }, null, 0, node: false));
		_dataArray.Add(new MonthlyEventItem(4, 8, EMonthlyEventType.SpecialEvent, "ffdda15e-c734-4bd3-842f-cb1e0170f4ca", "sp_monthlyevent_5", 9, new string[7] { "Character", "Location", "", "", "", "", "" }, null, 0, node: false));
		_dataArray.Add(new MonthlyEventItem(5, 10, EMonthlyEventType.SpecialEvent, "1e7e65eb-7889-49a8-a88c-0fb53b2bce08", "sp_monthlyevent_3", 11, new string[7] { "Location", "CharacterTemplate", "", "", "", "", "" }, null, 0, node: false));
		_dataArray.Add(new MonthlyEventItem(6, 12, EMonthlyEventType.SpecialEvent, "31f73af4-dfe0-4780-891b-af84e2112024", "sp_monthlyevent_86", 13, new string[7] { "Location", "CharacterTemplate", "", "", "", "", "" }, null, 0, node: false));
		_dataArray.Add(new MonthlyEventItem(7, 14, EMonthlyEventType.SpecialEvent, "6839f74d-a797-4057-9d3c-54526c75b17c", "sp_monthlyevent_4", 15, new string[7] { "Location", "CharacterTemplate", "", "", "", "", "" }, null, 0, node: false));
		_dataArray.Add(new MonthlyEventItem(8, 16, EMonthlyEventType.SpecialEvent, "35d08ee0-3f8b-4100-957a-b9a601ed400b", "sp_monthlyevent_88", 17, new string[7] { "Location", "Character", "", "", "", "", "" }, null, 0, node: false));
		_dataArray.Add(new MonthlyEventItem(9, 18, EMonthlyEventType.SpecialEvent, "b741c428-7bab-4856-b6cf-d55b2ad78d93", "sp_monthlyevent_88", 19, new string[7] { "Location", "Character", "", "", "", "", "" }, null, 0, node: false));
		_dataArray.Add(new MonthlyEventItem(10, 20, EMonthlyEventType.NormalEvent, "8556787a-6a5f-413e-a2f6-56ed2d4330f1", "sp_monthlyevent_6", 21, new string[7] { "Character", "Location", "", "", "", "", "" }, null, 0, node: false));
		_dataArray.Add(new MonthlyEventItem(11, 22, EMonthlyEventType.NormalEvent, "eea2f834-8d4d-456c-bd4e-eb41f9554011", "sp_monthlyevent_7", 23, new string[7] { "Character", "", "", "", "", "", "" }, null, 0, node: false));
		_dataArray.Add(new MonthlyEventItem(12, 22, EMonthlyEventType.NormalEvent, "a5d13b65-9503-496d-bea2-4f89196ca6f7", "sp_monthlyevent_8", 23, new string[7] { "Character", "", "", "", "", "", "" }, null, 0, node: false));
		_dataArray.Add(new MonthlyEventItem(13, 24, EMonthlyEventType.NormalEvent, "a73cc160-a95d-42c3-b986-a0353df434f0", "sp_monthlyevent_83", 25, new string[7] { "Character", "", "", "", "", "", "" }, null, 0, node: false));
		_dataArray.Add(new MonthlyEventItem(14, 26, EMonthlyEventType.NormalEvent, "b2d104a6-b1ea-4cbb-8043-54d8da07176c", "sp_monthlyevent_9", 27, new string[7] { "Character", "Location", "", "", "", "", "" }, null, 0, node: false));
		_dataArray.Add(new MonthlyEventItem(15, 26, EMonthlyEventType.NormalEvent, "be5c842e-d69f-40e3-961d-b49ba7186fc2", "sp_monthlyevent_9", 27, new string[7] { "Character", "Location", "", "", "", "", "" }, null, 0, node: false));
		_dataArray.Add(new MonthlyEventItem(16, 28, EMonthlyEventType.LockedEvent, "c6a8fff1-8d8e-4f14-91b8-3bf49bdd1a29", "sp_monthlyevent_10", 29, new string[7] { "Character", "Location", "", "", "", "", "" }, null, 0, node: false));
		_dataArray.Add(new MonthlyEventItem(17, 28, EMonthlyEventType.NormalEvent, "1f5bbc25-26db-46cc-bd85-54833cf2367a", "sp_monthlyevent_10", 29, new string[7] { "Character", "Location", "", "", "", "", "" }, null, 0, node: false));
		_dataArray.Add(new MonthlyEventItem(18, 30, EMonthlyEventType.NormalEvent, "7c8b6585-2d4a-4ae6-a0f8-a2228486271c", "sp_monthlyevent_9", 31, new string[7] { "Character", "Location", "", "", "", "", "" }, null, 0, node: false));
		_dataArray.Add(new MonthlyEventItem(19, 30, EMonthlyEventType.NormalEvent, "26c12b8e-2808-43d7-a00e-af970cb459bf", "sp_monthlyevent_9", 31, new string[7] { "Character", "Location", "", "", "", "", "" }, null, 0, node: false));
		_dataArray.Add(new MonthlyEventItem(20, 32, EMonthlyEventType.NormalEvent, "a86f7e1e-921b-42b8-9555-dec674e2df25", "sp_monthlyevent_11", 33, new string[7] { "Character", "Location", "Character", "", "", "", "" }, null, 0, node: false));
		_dataArray.Add(new MonthlyEventItem(21, 34, EMonthlyEventType.NormalEvent, "2425d411-1f0b-4482-b610-89ef5a7db33f", "sp_monthlyevent_12", 35, new string[7] { "Character", "Location", "Character", "", "", "", "" }, null, 0, node: false));
		_dataArray.Add(new MonthlyEventItem(22, 32, EMonthlyEventType.NormalEvent, "699239c9-8293-4b21-b479-daf07983156e", "sp_monthlyevent_11", 33, new string[7] { "Character", "Location", "Character", "", "", "", "" }, null, 0, node: false));
		_dataArray.Add(new MonthlyEventItem(23, 34, EMonthlyEventType.NormalEvent, "1612d6ee-f4fe-4ce1-9174-4fe434a8225a", "sp_monthlyevent_12", 35, new string[7] { "Character", "Location", "Character", "", "", "", "" }, null, 0, node: false));
		_dataArray.Add(new MonthlyEventItem(24, 36, EMonthlyEventType.LockedEvent, "4a3a0d8a-3140-400f-b444-f9ecb209cfba", "sp_monthlyevent_13", 37, new string[7] { "Character", "Location", "Character", "", "", "", "" }, null, 0, node: false));
		_dataArray.Add(new MonthlyEventItem(25, 38, EMonthlyEventType.LockedEvent, "2a0e5ea0-8418-4cc3-ba47-20bd9b2c5707", "sp_monthlyevent_13", 39, new string[7] { "Character", "Location", "Character", "", "", "", "" }, null, 0, node: false));
		_dataArray.Add(new MonthlyEventItem(26, 36, EMonthlyEventType.NormalEvent, "2c353211-ec1c-49dd-94cc-b396821348de", "sp_monthlyevent_13", 37, new string[7] { "Character", "Location", "Character", "", "", "", "" }, null, 0, node: false));
		_dataArray.Add(new MonthlyEventItem(27, 38, EMonthlyEventType.NormalEvent, "ac07bc64-eb8b-4214-92e4-15a63b77af40", "sp_monthlyevent_13", 39, new string[7] { "Character", "Location", "Character", "", "", "", "" }, null, 0, node: false));
		_dataArray.Add(new MonthlyEventItem(28, 40, EMonthlyEventType.NormalEvent, "a572973e-af1e-4ff8-8e12-884b28671281", "sp_monthlyevent_11", 41, new string[7] { "Character", "Location", "Character", "", "", "", "" }, null, 0, node: false));
		_dataArray.Add(new MonthlyEventItem(29, 42, EMonthlyEventType.NormalEvent, "fd9e4d4d-c8c1-4858-880b-7d6cb01f959b", "sp_monthlyevent_12", 43, new string[7] { "Character", "Location", "Character", "", "", "", "" }, null, 0, node: false));
		_dataArray.Add(new MonthlyEventItem(30, 40, EMonthlyEventType.NormalEvent, "78513fac-0ba8-4c46-b892-26d8d1dc79f3", "sp_monthlyevent_11", 41, new string[7] { "Character", "Location", "Character", "", "", "", "" }, null, 0, node: false));
		_dataArray.Add(new MonthlyEventItem(31, 42, EMonthlyEventType.NormalEvent, "a224107b-2909-4f02-882e-a84f2cad38ee", "sp_monthlyevent_12", 43, new string[7] { "Character", "Location", "Character", "", "", "", "" }, null, 0, node: false));
		_dataArray.Add(new MonthlyEventItem(32, 44, EMonthlyEventType.NormalEvent, "8812d517-ca0a-4f9d-83e8-936376ae11c4", "sp_monthlyevent_14", 45, new string[7] { "Character", "", "", "", "", "", "" }, null, 0, node: false));
		_dataArray.Add(new MonthlyEventItem(33, 46, EMonthlyEventType.NormalEvent, "3da0ed73-3abe-47c4-9990-91beeca7e831", "sp_monthlyevent_15", 47, new string[7] { "Character", "", "", "", "", "", "" }, null, 0, node: false));
		_dataArray.Add(new MonthlyEventItem(34, 48, EMonthlyEventType.NormalEvent, "90f67007-3c35-4b12-990e-66babdd88fed", "sp_monthlyevent_16", 49, new string[7] { "Character", "", "", "", "", "", "" }, null, 0, node: false));
		_dataArray.Add(new MonthlyEventItem(35, 50, EMonthlyEventType.NormalEvent, "e7921be6-80b9-41e1-90e4-956461e282ba", "sp_monthlyevent_17", 51, new string[7] { "Character", "", "", "", "", "", "" }, null, 0, node: false));
		_dataArray.Add(new MonthlyEventItem(36, 52, EMonthlyEventType.NormalEvent, "2c295340-baa0-44a6-a8f4-858a959b2fb9", "sp_monthlyevent_18", 53, new string[7] { "Character", "Character", "Character", "Character", "Character", "Character", "Character" }, null, 0, node: false));
		_dataArray.Add(new MonthlyEventItem(37, 54, EMonthlyEventType.NormalEvent, "ffe2e080-8c18-4e8c-9073-e78d04fa061b", "sp_monthlyevent_19", 55, new string[7] { "Character", "Location", "Character", "", "", "", "" }, null, 0, node: false));
		_dataArray.Add(new MonthlyEventItem(38, 56, EMonthlyEventType.NormalEvent, "e1f06054-115f-48d6-b8c2-eeb8f62bafe0", "sp_monthlyevent_20", 57, new string[7] { "Character", "Character", "", "", "", "", "" }, null, 0, node: false));
		_dataArray.Add(new MonthlyEventItem(39, 58, EMonthlyEventType.NormalEvent, "e859cd22-0fae-4356-8ae3-903c7bc82972", "sp_monthlyevent_21", 59, new string[7] { "", "", "", "", "", "", "" }, null, 0, node: false));
		_dataArray.Add(new MonthlyEventItem(40, 60, EMonthlyEventType.NormalEvent, "4fbe0a50-be4f-42fd-9341-7b8498bae3e1", "sp_monthlyevent_22", 61, new string[7] { "Location", "", "", "", "", "", "" }, null, 0, node: false));
		_dataArray.Add(new MonthlyEventItem(41, 62, EMonthlyEventType.NormalEvent, "80f68c9a-9506-4814-9215-5e8dd2698719", "sp_monthlyevent_23", 63, new string[7] { "Location", "", "", "", "", "", "" }, null, 0, node: false));
		_dataArray.Add(new MonthlyEventItem(42, 64, EMonthlyEventType.NormalEvent, "8d3de6a1-1d88-4bfa-99be-6722ed08b8f3", "sp_monthlyevent_24", 65, new string[7] { "LifeSkill", "Location", "", "", "", "", "" }, null, 0, node: false));
		_dataArray.Add(new MonthlyEventItem(43, 66, EMonthlyEventType.NormalEvent, "471ea810-7be4-454c-8375-d6f2477a93ac", "sp_monthlyevent_25", 67, new string[7] { "Character", "Location", "Character", "", "", "", "" }, null, 0, node: false));
		_dataArray.Add(new MonthlyEventItem(44, 68, EMonthlyEventType.NormalEvent, "b335cd4b-0ce3-4882-a5bb-f2d2ad699b11", "sp_monthlyevent_26", 69, new string[7] { "Character", "Location", "Character", "", "", "", "" }, null, 0, node: false));
		_dataArray.Add(new MonthlyEventItem(45, 70, EMonthlyEventType.NormalEvent, "11ce2ba2-5abc-4f9e-9fbc-8892f03cd8f6", "sp_monthlyevent_27", 71, new string[7] { "Character", "Location", "Character", "", "", "", "" }, null, 0, node: false));
		_dataArray.Add(new MonthlyEventItem(46, 72, EMonthlyEventType.NormalEvent, "8ce2db54-994d-4790-bfe3-6cedd7473277", "sp_monthlyevent_28", 73, new string[7] { "Character", "Location", "Character", "", "", "", "" }, null, 0, node: false));
		_dataArray.Add(new MonthlyEventItem(47, 74, EMonthlyEventType.NormalEvent, "63a3c0e9-cf75-4c03-9453-48841d3e9fa9", "sp_monthlyevent_29", 75, new string[7] { "Character", "Location", "Character", "", "", "", "" }, null, 0, node: false));
		_dataArray.Add(new MonthlyEventItem(48, 76, EMonthlyEventType.NormalEvent, "9c81352d-b715-4554-85fc-50322fe428f6", "sp_monthlyevent_30", 77, new string[7] { "Character", "Location", "Character", "", "", "", "" }, null, 0, node: false));
		_dataArray.Add(new MonthlyEventItem(49, 78, EMonthlyEventType.NormalEvent, "a546e498-7128-49df-ab14-09a6474cbc66", "sp_monthlyevent_31", 79, new string[7] { "Character", "Location", "Character", "", "", "", "" }, null, 0, node: false));
		_dataArray.Add(new MonthlyEventItem(50, 80, EMonthlyEventType.NormalEvent, "abce40cd-8b67-41a6-8015-d927d6f6ef3b", "sp_monthlyevent_32", 81, new string[7] { "Character", "Location", "Character", "", "", "", "" }, null, 0, node: false));
		_dataArray.Add(new MonthlyEventItem(51, 82, EMonthlyEventType.NormalEvent, "9ba8de0b-1ffa-4083-9024-d349ec65cad3", "sp_monthlyevent_33", 83, new string[7] { "Character", "Location", "Character", "", "", "", "" }, null, 0, node: false));
		_dataArray.Add(new MonthlyEventItem(52, 84, EMonthlyEventType.NormalEvent, "9836cfcf-3ff9-4724-b7d1-be307fee808b", "sp_monthlyevent_34", 85, new string[7] { "Character", "Location", "Character", "", "", "", "" }, null, 0, node: false));
		_dataArray.Add(new MonthlyEventItem(53, 86, EMonthlyEventType.NormalEvent, "f81b923a-110d-4956-8fab-802650fb5afd", "sp_monthlyevent_35", 87, new string[7] { "Character", "Location", "Character", "", "", "", "" }, null, 0, node: false));
		_dataArray.Add(new MonthlyEventItem(54, 88, EMonthlyEventType.NormalEvent, "135e6b5f-afb8-4bbe-a50f-0e040a8ba52a", "sp_monthlyevent_36", 89, new string[7] { "Character", "Location", "Character", "", "", "", "" }, null, 0, node: false));
		_dataArray.Add(new MonthlyEventItem(55, 90, EMonthlyEventType.NormalEvent, "940bdcc2-59f8-483b-86e9-d80ced06fd40", "sp_monthlyevent_37", 91, new string[7] { "Character", "Location", "Character", "", "", "", "" }, null, 0, node: false));
		_dataArray.Add(new MonthlyEventItem(56, 92, EMonthlyEventType.NormalEvent, "6a905b7f-80e2-49dd-9539-a7e79f57454d", "sp_monthlyevent_38", 93, new string[7] { "Character", "Location", "Character", "", "", "", "" }, null, 0, node: false));
		_dataArray.Add(new MonthlyEventItem(57, 94, EMonthlyEventType.NormalEvent, "20eec777-8c29-45d1-9acc-7474942ab49c", "sp_monthlyevent_39", 95, new string[7] { "Character", "Location", "", "", "", "", "" }, null, 0, node: false));
		_dataArray.Add(new MonthlyEventItem(58, 96, EMonthlyEventType.NormalEvent, "cb6c9034-4da1-4d27-8b32-4ce6034d75d2", "sp_monthlyevent_40", 97, new string[7] { "Character", "Location", "Character", "", "", "", "" }, null, 0, node: false));
		_dataArray.Add(new MonthlyEventItem(59, 98, EMonthlyEventType.NormalEvent, "e7dfbf0a-8fec-4e33-b4f7-2fc93a9799f1", "sp_monthlyevent_41", 99, new string[7] { "Character", "Location", "", "", "", "", "" }, null, 0, node: false));
	}

	private void CreateItems1()
	{
		_dataArray.Add(new MonthlyEventItem(60, 100, EMonthlyEventType.SpecialEvent, "98c52d02-36aa-4293-9d40-5573f3322dac", "sp_monthlyevent_42", 101, new string[7] { "Character", "Location", "Character", "", "", "", "" }, null, 0, node: false));
		_dataArray.Add(new MonthlyEventItem(61, 102, EMonthlyEventType.NormalEvent, "17093897-d8d9-4d7d-86ed-229cc4e85afd", "sp_monthlyevent_43", 103, new string[7] { "Character", "Location", "Character", "Character", "", "", "" }, null, 0, node: false));
		_dataArray.Add(new MonthlyEventItem(62, 104, EMonthlyEventType.NormalEvent, "79ab2055-ccab-41b9-af31-c9e5918a7893", "sp_monthlyevent_44", 105, new string[7] { "Character", "Location", "Character", "", "", "", "" }, null, 0, node: false));
		_dataArray.Add(new MonthlyEventItem(63, 104, EMonthlyEventType.NormalEvent, "4f418b2e-be18-4c1a-aa3a-306fcf929357", "sp_monthlyevent_44", 106, new string[7] { "Character", "Location", "", "", "", "", "" }, null, 0, node: false));
		_dataArray.Add(new MonthlyEventItem(64, 107, EMonthlyEventType.NormalEvent, "60d93b13-33b6-4be7-9a3b-7253275a2696", "sp_monthlyevent_45", 108, new string[7] { "Character", "Location", "Character", "", "", "", "" }, null, 0, node: false));
		_dataArray.Add(new MonthlyEventItem(65, 107, EMonthlyEventType.NormalEvent, "b1e95ac4-0d8e-4b9e-954e-0ccd7c5e7c41", "sp_monthlyevent_45", 109, new string[7] { "Character", "Location", "", "", "", "", "" }, null, 0, node: false));
		_dataArray.Add(new MonthlyEventItem(66, 110, EMonthlyEventType.NormalEvent, "3af56371-f736-4aa9-ae9b-c3c98ba0f4b0", "sp_monthlyevent_46", 111, new string[7] { "Character", "Location", "Character", "ItemKey", "BodyPartType", "", "" }, null, 0, node: false));
		_dataArray.Add(new MonthlyEventItem(67, 110, EMonthlyEventType.NormalEvent, "d58fceec-2d53-4532-9893-d834db626b35", "sp_monthlyevent_46", 112, new string[7] { "Character", "Location", "Character", "Integer", "", "", "" }, null, 0, node: false));
		_dataArray.Add(new MonthlyEventItem(68, 113, EMonthlyEventType.NormalEvent, "3f335f54-d401-4951-9b9e-61c82c7d5bbc", "sp_monthlyevent_46", 114, new string[7] { "Character", "Location", "Character", "ItemKey", "BodyPartType", "", "" }, null, 0, node: false));
		_dataArray.Add(new MonthlyEventItem(69, 113, EMonthlyEventType.NormalEvent, "3c71d6c1-f5a0-4e20-9eca-280dde1b2f84", "sp_monthlyevent_46", 115, new string[7] { "Character", "Location", "Character", "Integer", "", "", "" }, null, 0, node: false));
		_dataArray.Add(new MonthlyEventItem(70, 116, EMonthlyEventType.NormalEvent, "1deadcc9-cca4-4091-8276-5f3a8b136fe0", "sp_monthlyevent_47", 117, new string[7] { "Character", "Location", "Character", "ItemKey", "PoisonType", "", "" }, null, 0, node: false));
		_dataArray.Add(new MonthlyEventItem(71, 116, EMonthlyEventType.NormalEvent, "d799f11c-7ff7-4f9e-83a6-192753411e7b", "sp_monthlyevent_47", 118, new string[7] { "Character", "Location", "Character", "Integer", "PoisonType", "", "" }, null, 0, node: false));
		_dataArray.Add(new MonthlyEventItem(72, 119, EMonthlyEventType.NormalEvent, "66fe331f-7bb2-422b-9a3e-892859eedb55", "sp_monthlyevent_48", 120, new string[7] { "Character", "Location", "Character", "ItemKey", "", "", "" }, null, 0, node: false));
		_dataArray.Add(new MonthlyEventItem(73, 121, EMonthlyEventType.NormalEvent, "03b005e1-e0c6-4726-abc7-4e3121116049", "sp_monthlyevent_48", 122, new string[7] { "Character", "Location", "Character", "ItemKey", "", "", "" }, null, 0, node: false));
		_dataArray.Add(new MonthlyEventItem(74, 123, EMonthlyEventType.NormalEvent, "d2e80cb3-4c6f-471d-83f9-422bbcab2d7f", "sp_monthlyevent_48", 124, new string[7] { "Character", "Location", "Character", "ItemKey", "", "", "" }, null, 0, node: false));
		_dataArray.Add(new MonthlyEventItem(75, 125, EMonthlyEventType.NormalEvent, "d7ec7e02-ee62-4137-a8cb-a4961ec28bb0", "sp_monthlyevent_47", 126, new string[7] { "Character", "Location", "Character", "ItemKey", "ItemKey", "", "" }, null, 0, node: false));
		_dataArray.Add(new MonthlyEventItem(76, 127, EMonthlyEventType.NormalEvent, "560cee7d-3955-4650-8fbf-cf66e80a321d", "sp_monthlyevent_49", 128, new string[7] { "Character", "Location", "Character", "ItemKey", "Integer", "", "" }, null, 0, node: false));
		_dataArray.Add(new MonthlyEventItem(77, 129, EMonthlyEventType.NormalEvent, "e6a1795d-e1a1-46af-b977-f821fc1441f5", "sp_monthlyevent_49", 130, new string[7] { "Character", "Location", "Character", "ItemKey", "", "", "" }, null, 0, node: false));
		_dataArray.Add(new MonthlyEventItem(78, 131, EMonthlyEventType.NormalEvent, "16bddd08-6083-4c11-9342-78922ed19b6b", "sp_monthlyevent_49", 132, new string[7] { "Character", "Location", "Character", "Integer", "Resource", "", "" }, null, 0, node: false));
		_dataArray.Add(new MonthlyEventItem(79, 133, EMonthlyEventType.NormalEvent, "7ac0429a-a48f-4adb-be4b-4cb767d978e4", "sp_monthlyevent_49", 134, new string[7] { "Character", "Location", "Character", "ItemKey", "Integer", "", "" }, null, 0, node: false));
		_dataArray.Add(new MonthlyEventItem(80, 135, EMonthlyEventType.NormalEvent, "a61df3c7-c29b-4a81-b4c8-da7651c931e8", "sp_monthlyevent_50", 136, new string[7] { "Character", "Location", "Character", "ItemKey", "ItemKey", "Integer", "Resource" }, null, 0, node: false));
		_dataArray.Add(new MonthlyEventItem(81, 137, EMonthlyEventType.NormalEvent, "7386d3bc-3ebe-4603-b771-1f61f7a166b2", "sp_monthlyevent_50", 138, new string[7] { "Character", "Location", "Character", "ItemKey", "ItemKey", "", "" }, null, 0, node: false));
		_dataArray.Add(new MonthlyEventItem(82, 139, EMonthlyEventType.NormalEvent, "cbb59e89-a125-42ab-8692-e522c44a0bc8", "sp_monthlyevent_51", 140, new string[7] { "Character", "Location", "Character", "Item", "Integer", "", "" }, null, 0, node: false));
		_dataArray.Add(new MonthlyEventItem(83, 141, EMonthlyEventType.NormalEvent, "eb7f0c2a-60f9-4221-97e5-662d513620c1", "sp_monthlyevent_51", 140, new string[7] { "Character", "Location", "Character", "Item", "Integer", "Integer", "Integer" }, null, 0, node: false));
		_dataArray.Add(new MonthlyEventItem(84, 142, EMonthlyEventType.NormalEvent, "1735b4a9-4ece-4ff9-83f3-fa005cfa33e8", "sp_monthlyevent_52", 143, new string[7] { "Character", "Location", "Character", "ItemKey", "Integer", "", "" }, null, 0, node: false));
		_dataArray.Add(new MonthlyEventItem(85, 144, EMonthlyEventType.NormalEvent, "9804522b-8380-4950-9996-f15a8387d802", "sp_monthlyevent_53", 143, new string[7] { "Character", "Location", "Character", "ItemKey", "Integer", "Integer", "" }, null, 0, node: false));
		_dataArray.Add(new MonthlyEventItem(86, 145, EMonthlyEventType.NormalEvent, "6c91f53d-6eb8-43e5-812d-e1838cab5c57", "sp_monthlyevent_84", 146, new string[7] { "Character", "Location", "Character", "CombatSkill", "", "", "" }, null, 0, node: false));
		_dataArray.Add(new MonthlyEventItem(87, 147, EMonthlyEventType.NormalEvent, "c485c693-5ff0-45ec-933c-62c75a045fee", "sp_monthlyevent_54", 148, new string[7] { "Character", "Location", "Character", "", "", "", "" }, null, 0, node: false));
		_dataArray.Add(new MonthlyEventItem(88, 149, EMonthlyEventType.NormalEvent, "7d52876b-b8e7-421c-a4a7-a93916431aa3", "sp_monthlyevent_55", 150, new string[7] { "Character", "Location", "Character", "", "", "", "" }, null, 0, node: false));
		_dataArray.Add(new MonthlyEventItem(89, 151, EMonthlyEventType.NormalEvent, "c2019d63-3dc0-481c-9a9d-8f11afdea032", "sp_monthlyevent_56", 152, new string[7] { "Character", "Location", "Character", "", "", "", "" }, null, 0, node: false));
		_dataArray.Add(new MonthlyEventItem(90, 153, EMonthlyEventType.NormalEvent, "79c5748c-aac6-4e77-b32d-32bfd59be5f5", "sp_monthlyevent_57", 154, new string[7] { "Character", "Location", "Character", "", "", "", "" }, null, 0, node: false));
		_dataArray.Add(new MonthlyEventItem(91, 155, EMonthlyEventType.NormalEvent, "24dd3ebb-4d21-492c-95e8-f7bd68c720ce", "sp_monthlyevent_58", 156, new string[7] { "Character", "Location", "Character", "Character", "", "", "" }, null, 0, node: false));
		_dataArray.Add(new MonthlyEventItem(92, 157, EMonthlyEventType.NormalEvent, "6ea53aad-a7a9-42d4-b604-4cb449cc307b", "sp_monthlyevent_59", 158, new string[7] { "Character", "Location", "Character", "", "", "", "" }, null, 0, node: false));
		_dataArray.Add(new MonthlyEventItem(93, 159, EMonthlyEventType.SpecialEvent, "e81ef0d1-64ac-466d-9c12-730c0da1c6e7", "sp_monthlyevent_60", 160, new string[7] { "Character", "Location", "Character", "Character", "", "", "" }, null, 0, node: false));
		_dataArray.Add(new MonthlyEventItem(94, 161, EMonthlyEventType.SpecialEvent, "202a620d-cee9-4df1-a48d-08a0a12e1dcc", "sp_monthlyevent_61", 162, new string[7] { "Character", "Location", "Character", "Character", "", "", "" }, null, 0, node: false));
		_dataArray.Add(new MonthlyEventItem(95, 163, EMonthlyEventType.NormalEvent, "7862ad2f-0d39-4f35-bf68-ed9817cf9d3b", "sp_monthlyevent_62", 164, new string[7] { "Character", "Location", "Character", "Resource", "Integer", "", "" }, null, 0, node: false));
		_dataArray.Add(new MonthlyEventItem(96, 165, EMonthlyEventType.NormalEvent, "2e6e8d8e-1b7e-4826-8a88-c7d548a56281", "sp_monthlyevent_63", 166, new string[7] { "Character", "Location", "Character", "Resource", "Integer", "", "" }, null, 0, node: false));
		_dataArray.Add(new MonthlyEventItem(97, 167, EMonthlyEventType.SpecialEvent, "217e62da-2a6a-4496-905d-9722a84e4a38", "sp_monthlyevent_65", 168, new string[7] { "Character", "Location", "Character", "Resource", "Integer", "", "" }, null, 0, node: false));
		_dataArray.Add(new MonthlyEventItem(98, 169, EMonthlyEventType.SpecialEvent, "5392cdf7-2f52-4c14-9416-3025676d9dcc", "sp_monthlyevent_64", 170, new string[7] { "Character", "Location", "Character", "Resource", "Integer", "", "" }, null, 0, node: false));
		_dataArray.Add(new MonthlyEventItem(99, 171, EMonthlyEventType.NormalEvent, "7c4b526f-e634-4cac-94af-e64ae059910f", "sp_monthlyevent_62", 172, new string[7] { "Character", "Location", "Character", "ItemKey", "", "", "" }, null, 0, node: false));
		_dataArray.Add(new MonthlyEventItem(100, 173, EMonthlyEventType.NormalEvent, "369a71c6-319a-4427-8729-df9fba616682", "sp_monthlyevent_63", 174, new string[7] { "Character", "Location", "Character", "ItemKey", "", "", "" }, null, 0, node: false));
		_dataArray.Add(new MonthlyEventItem(101, 175, EMonthlyEventType.SpecialEvent, "32e1edf9-4c1c-4adf-819b-a3009e650a32", "sp_monthlyevent_65", 168, new string[7] { "Character", "Location", "Character", "ItemKey", "", "", "" }, null, 0, node: false));
		_dataArray.Add(new MonthlyEventItem(102, 176, EMonthlyEventType.SpecialEvent, "f3341e73-d607-4d0d-b3c1-ed4c5ccd0e74", "sp_monthlyevent_64", 170, new string[7] { "Character", "Location", "Character", "ItemKey", "", "", "" }, null, 0, node: false));
		_dataArray.Add(new MonthlyEventItem(103, 177, EMonthlyEventType.NormalEvent, "3abe4e48-fab7-4619-bfc9-656c06dead27", "sp_monthlyevent_66", 178, new string[7] { "Character", "Location", "Character", "Item", "Integer", "", "" }, null, 0, node: false));
		_dataArray.Add(new MonthlyEventItem(104, 179, EMonthlyEventType.NormalEvent, "266de680-6160-4a10-af71-678481915019", "sp_monthlyevent_67", 180, new string[7] { "Character", "Location", "Character", "Item", "Integer", "", "" }, null, 0, node: false));
		_dataArray.Add(new MonthlyEventItem(105, 181, EMonthlyEventType.SpecialEvent, "e8912b09-71bd-4254-93b2-ed6266f5f8fa", "sp_monthlyevent_68", 182, new string[7] { "Character", "Location", "Character", "Item", "Integer", "", "" }, null, 0, node: false));
		_dataArray.Add(new MonthlyEventItem(106, 183, EMonthlyEventType.NormalEvent, "e4df3573-eaa7-4bec-a7f0-b528e2078d0f", "sp_monthlyevent_69", 178, new string[7] { "Character", "Location", "Character", "Item", "Integer", "Integer", "Integer" }, null, 0, node: false));
		_dataArray.Add(new MonthlyEventItem(107, 184, EMonthlyEventType.NormalEvent, "9fc43b8f-11cd-434a-b324-6a7ad2c45bd5", "sp_monthlyevent_70", 180, new string[7] { "Character", "Location", "Character", "Item", "Integer", "Integer", "Integer" }, null, 0, node: false));
		_dataArray.Add(new MonthlyEventItem(108, 185, EMonthlyEventType.SpecialEvent, "514e2102-26fb-4fe8-9a79-48e919c5acaa", "sp_monthlyevent_71", 182, new string[7] { "Character", "Location", "Character", "Item", "Integer", "Integer", "Integer" }, null, 0, node: false));
		_dataArray.Add(new MonthlyEventItem(109, 186, EMonthlyEventType.NormalEvent, "eb338657-9fec-4129-bbbe-3e4390606d7e", "sp_monthlyevent_72", 187, new string[7] { "Character", "Location", "Character", "", "", "", "" }, null, 0, node: false));
		_dataArray.Add(new MonthlyEventItem(110, 188, EMonthlyEventType.NormalEvent, "657c9c77-5690-4e32-bbfc-245b44489374", "sp_monthlyevent_73", 189, new string[7] { "Character", "Location", "Character", "", "", "", "" }, null, 0, node: false));
		_dataArray.Add(new MonthlyEventItem(111, 190, EMonthlyEventType.NormalEvent, "dec0683d-fade-4a91-94b7-125cf2394aab", "sp_monthlyevent_74", 191, new string[7] { "Character", "Location", "Character", "", "", "", "" }, null, 0, node: false));
		_dataArray.Add(new MonthlyEventItem(112, 192, EMonthlyEventType.NormalEvent, "13a9669f-1e5b-4519-99df-1123c41b0132", "sp_monthlyevent_75", 193, new string[7] { "Character", "Location", "Character", "ItemKey", "ItemKey", "", "" }, null, 0, node: false));
		_dataArray.Add(new MonthlyEventItem(113, 194, EMonthlyEventType.NormalEvent, "c86ac53a-cc8b-4208-bed6-366ac194572d", "sp_monthlyevent_76", 195, new string[7] { "Character", "Location", "Character", "ItemKey", "Integer", "Integer", "" }, null, 0, node: false));
		_dataArray.Add(new MonthlyEventItem(114, 196, EMonthlyEventType.NormalEvent, "08d64ad1-2df9-4155-bcd4-bf7b8c367d58", "sp_monthlyevent_77", 197, new string[7] { "Character", "Location", "Character", "Integer", "", "", "" }, null, 0, node: false));
		_dataArray.Add(new MonthlyEventItem(115, 196, EMonthlyEventType.NormalEvent, "55854256-94d7-4227-8ca2-ff074dcc9d1f", "sp_monthlyevent_77", 198, new string[7] { "Character", "Location", "Character", "Integer", "", "", "" }, null, 0, node: false));
		_dataArray.Add(new MonthlyEventItem(116, 199, EMonthlyEventType.NormalEvent, "fdcb8aaf-ff9f-4bcb-981c-4e4f7c8d7648", "sp_monthlyevent_78", 200, new string[7] { "Character", "Location", "Character", "ItemKey", "ItemKey", "Resource", "Integer" }, null, 0, node: false));
		_dataArray.Add(new MonthlyEventItem(117, 201, EMonthlyEventType.NormalEvent, "99aca42c-8d46-499e-88b3-843f790c4c4c", "sp_monthlyevent_79", 202, new string[7] { "Character", "Location", "Character", "", "", "", "" }, null, 0, node: false));
		_dataArray.Add(new MonthlyEventItem(118, 203, EMonthlyEventType.NormalEvent, "2a523b5e-2660-45bd-a684-acfd42dcd604", "sp_monthlyevent_80", 204, new string[7] { "Character", "Location", "Character", "", "", "", "" }, null, 0, node: false));
		_dataArray.Add(new MonthlyEventItem(119, 205, EMonthlyEventType.NormalEvent, "cf1dca5b-7d9f-4e76-8d10-e2e59a24053b", "sp_monthlyevent_81", 206, new string[7] { "", "", "", "", "", "", "" }, null, 0, node: false));
	}

	private void CreateItems2()
	{
		_dataArray.Add(new MonthlyEventItem(120, 207, EMonthlyEventType.NormalEvent, "d66412f3-357e-4ab4-bc29-707c391f1114", "sp_monthlyevent_82", 208, new string[7] { "Settlement", "Character", "", "", "", "", "" }, null, 0, node: false));
		_dataArray.Add(new MonthlyEventItem(121, 209, EMonthlyEventType.LockedEvent, "cce87696-5bc6-4006-80a0-e4df96277208", "sp_monthlyevent_85", 210, new string[7] { "", "", "", "", "", "", "" }, null, 0, node: false));
		_dataArray.Add(new MonthlyEventItem(122, 211, EMonthlyEventType.NormalEvent, "3b78ef2e-89b5-48b3-bd48-94d8aa6a11e7", "sp_monthlyevent_87", 212, new string[7] { "Character", "", "", "", "", "", "" }, null, 0, node: false));
		_dataArray.Add(new MonthlyEventItem(123, 213, EMonthlyEventType.NormalEvent, "83d6b576-3458-4c2e-a2d9-8d7c0c0cc6a5", "sp_monthlyevent_89", 214, new string[7] { "", "", "", "", "", "", "" }, null, 0, node: false));
		_dataArray.Add(new MonthlyEventItem(124, 213, EMonthlyEventType.NormalEvent, "0940e147-8e6f-40be-be38-246081334f67", "sp_monthlyevent_89", 214, new string[7] { "", "", "", "", "", "", "" }, null, 0, node: false));
		_dataArray.Add(new MonthlyEventItem(125, 213, EMonthlyEventType.NormalEvent, "c8743e2b-29e1-434f-b0a5-61c9c7e47879", "sp_monthlyevent_89", 214, new string[7] { "", "", "", "", "", "", "" }, null, 0, node: false));
		_dataArray.Add(new MonthlyEventItem(126, 213, EMonthlyEventType.NormalEvent, "9c76da25-c084-499b-bb1a-30b25376edf6", "sp_monthlyevent_89", 214, new string[7] { "", "", "", "", "", "", "" }, null, 0, node: false));
		_dataArray.Add(new MonthlyEventItem(127, 213, EMonthlyEventType.NormalEvent, "bf5d6073-4ebe-4808-8f7c-e6514320f8ca", "sp_monthlyevent_89", 214, new string[7] { "", "", "", "", "", "", "" }, null, 0, node: false));
		_dataArray.Add(new MonthlyEventItem(128, 213, EMonthlyEventType.NormalEvent, "c7ce95b6-7e99-4873-9670-d7364b263615", "sp_monthlyevent_89", 214, new string[7] { "", "", "", "", "", "", "" }, null, 0, node: false));
		_dataArray.Add(new MonthlyEventItem(129, 213, EMonthlyEventType.NormalEvent, "2eb7d4aa-ee04-42e1-97e7-86351308d20d", "sp_monthlyevent_89", 214, new string[7] { "", "", "", "", "", "", "" }, null, 0, node: false));
		_dataArray.Add(new MonthlyEventItem(130, 213, EMonthlyEventType.NormalEvent, "f4cd2829-71b3-4d74-b618-242800ab1274", "sp_monthlyevent_89", 214, new string[7] { "", "", "", "", "", "", "" }, null, 0, node: false));
		_dataArray.Add(new MonthlyEventItem(131, 213, EMonthlyEventType.NormalEvent, "399fb669-fae9-41f1-a69a-57d040b54199", "sp_monthlyevent_89", 214, new string[7] { "", "", "", "", "", "", "" }, null, 0, node: false));
		_dataArray.Add(new MonthlyEventItem(132, 215, EMonthlyEventType.NormalEvent, "0555df77-0f9a-4ec0-ad45-882b4c579ecd", "sp_monthlyevent_90", 216, new string[7] { "Character", "Character", "", "", "", "", "" }, null, 0, node: false));
		_dataArray.Add(new MonthlyEventItem(133, 215, EMonthlyEventType.NormalEvent, "7bcdde30-708e-4e10-8d55-7befc82e5e1b", "sp_monthlyevent_90", 217, new string[7] { "Character", "Character", "", "", "", "", "" }, null, 0, node: false));
		_dataArray.Add(new MonthlyEventItem(134, 218, EMonthlyEventType.SpecialEvent, "d5eed465-57c2-471d-be63-c731173581e5", "sp_monthlyevent_96", 219, new string[7] { "Character", "Location", "Character", "ItemKey", "", "", "" }, null, 0, node: false));
		_dataArray.Add(new MonthlyEventItem(135, 220, EMonthlyEventType.NormalEvent, "885a4366-a3e4-4687-8f53-fde6e9eee1e8", "sp_monthlyevent_98", 221, new string[7] { "Character", "Location", "Character", "ItemKey", "", "", "" }, null, 0, node: false));
		_dataArray.Add(new MonthlyEventItem(136, 222, EMonthlyEventType.NormalEvent, "e16be2e5-0112-467d-b91f-1e60e94fffb4", "sp_monthlyevent_95", 223, new string[7] { "Character", "Location", "Character", "ItemKey", "", "", "" }, null, 0, node: false));
		_dataArray.Add(new MonthlyEventItem(137, 222, EMonthlyEventType.NormalEvent, "92bcf2b1-3711-4336-aa6a-28b63f7d188f", "sp_monthlyevent_95", 223, new string[7] { "Character", "Location", "Character", "ItemKey", "", "", "" }, null, 0, node: false));
		_dataArray.Add(new MonthlyEventItem(138, 222, EMonthlyEventType.NormalEvent, "cbacdd62-ff8d-4e6f-8655-5d924b40daef", "sp_monthlyevent_95", 223, new string[7] { "Character", "Location", "Character", "ItemKey", "", "", "" }, null, 0, node: false));
		_dataArray.Add(new MonthlyEventItem(139, 224, EMonthlyEventType.NormalEvent, "24a28e9b-0d14-41d1-8c67-ef2f96ec5f72", "sp_monthlyevent_97", 225, new string[7] { "Character", "Location", "Character", "ItemKey", "Integer", "", "" }, null, 0, node: false));
		_dataArray.Add(new MonthlyEventItem(140, 226, EMonthlyEventType.NormalEvent, "8a56b164-7115-4c7f-bf5e-d0cabe2e63ad", "sp_monthlyevent_97", 227, new string[7] { "Character", "Location", "Character", "ItemKey", "Integer", "", "" }, null, 0, node: false));
		_dataArray.Add(new MonthlyEventItem(141, 228, EMonthlyEventType.SpecialEvent, "e5d3f23a-7468-461e-ae95-b873ec052b86", "sp_monthlyevent_92", 229, new string[7] { "Character", "Location", "Character", "ItemKey", "Integer", "", "" }, null, 0, node: false));
		_dataArray.Add(new MonthlyEventItem(142, 230, EMonthlyEventType.SpecialEvent, "6b98acdc-46d3-4604-add5-f17ff20cad41", "sp_monthlyevent_91", 231, new string[7] { "Character", "Location", "Character", "ItemKey", "Integer", "", "" }, null, 0, node: false));
		_dataArray.Add(new MonthlyEventItem(143, 232, EMonthlyEventType.SpecialEvent, "bea538e6-21ea-49c4-b82b-8e9934cef884", "sp_monthlyevent_99", 233, new string[7] { "Character", "Location", "Character", "", "", "", "" }, null, 0, node: false));
		_dataArray.Add(new MonthlyEventItem(144, 232, EMonthlyEventType.SpecialEvent, "030aaeef-e59e-4add-a8ce-e34254430cb9", "sp_monthlyevent_99", 233, new string[7] { "Character", "Location", "Character", "", "", "", "" }, null, 0, node: false));
		_dataArray.Add(new MonthlyEventItem(145, 232, EMonthlyEventType.SpecialEvent, "70d9be73-bcff-4e02-a470-55b40827a685", "sp_monthlyevent_99", 233, new string[7] { "Character", "Location", "Character", "", "", "", "" }, null, 0, node: false));
		_dataArray.Add(new MonthlyEventItem(146, 234, EMonthlyEventType.NormalEvent, "0e77f1c4-3e7f-4b51-a437-9706a2627e21", "sp_monthlyevent_94", 235, new string[7] { "ItemKey", "", "", "", "", "", "" }, null, 0, node: false));
		_dataArray.Add(new MonthlyEventItem(147, 236, EMonthlyEventType.NormalEvent, "255ce011-b2bb-44bc-9dc7-7c7f8b869a63", "sp_monthlyevent_93", 237, new string[7] { "ItemKey", "", "", "", "", "", "" }, null, 0, node: false));
		_dataArray.Add(new MonthlyEventItem(148, 238, EMonthlyEventType.NormalEvent, "5f5bbf2e-96d7-4766-9f27-e558a4a6de6b", "sp_monthlyevent_100", 239, new string[7] { "Location", "ItemKey", "", "", "", "", "" }, null, 0, node: false));
		_dataArray.Add(new MonthlyEventItem(149, 238, EMonthlyEventType.NormalEvent, "5e6309b0-b489-4302-868b-16c67f33b726", "sp_monthlyevent_100", 240, new string[7] { "Character", "Location", "ItemKey", "", "", "", "" }, null, 0, node: false));
		_dataArray.Add(new MonthlyEventItem(150, 238, EMonthlyEventType.NormalEvent, "74f2ea8b-1258-4d67-98fe-7e2a68dd0c90", "sp_monthlyevent_100", 241, new string[7] { "Character", "Location", "ItemKey", "", "", "", "" }, null, 0, node: false));
		_dataArray.Add(new MonthlyEventItem(151, 238, EMonthlyEventType.NormalEvent, "a2195715-ad40-45d6-8786-356fe4f68a59", "sp_monthlyevent_100", 242, new string[7] { "Character", "Location", "ItemKey", "", "", "", "" }, null, 0, node: false));
		_dataArray.Add(new MonthlyEventItem(152, 243, EMonthlyEventType.NormalEvent, "b987500e-a0a2-4c7b-9cfd-968fd422d09a", "sp_monthlyevent_83", 244, new string[7] { "Character", "Character", "", "", "", "", "" }, null, 0, node: false));
		_dataArray.Add(new MonthlyEventItem(153, 245, EMonthlyEventType.NormalEvent, "66ef6731-20e3-4439-ae76-d353485ba95a", "sp_monthlyevent_83", 246, new string[7] { "Character", "Month", "", "", "", "", "" }, null, 0, node: false));
		_dataArray.Add(new MonthlyEventItem(154, 247, EMonthlyEventType.NormalEvent, "33c8f19d-edd3-4f0f-a578-7ac855328edc", "sp_monthlyevent_83", 248, new string[7] { "Character", "Character", "Integer", "", "", "", "" }, null, 0, node: false));
		_dataArray.Add(new MonthlyEventItem(155, 249, EMonthlyEventType.NormalEvent, "4799bea8-5be9-4c06-9db9-8972c6803c26", "sp_monthlyevent_83", 250, new string[7] { "Character", "", "", "", "", "", "" }, null, 0, node: false));
		_dataArray.Add(new MonthlyEventItem(156, 251, EMonthlyEventType.SpecialEvent, "849c544c-c42f-42bc-a8b0-aa8166f25aa4", "sp_monthlyevent_83", 252, new string[7] { "Character", "Character", "", "", "", "", "" }, null, 0, node: false));
		_dataArray.Add(new MonthlyEventItem(157, 253, EMonthlyEventType.SpecialEvent, "50791f39-a584-4077-855c-712f7a9aebd0", "sp_monthlyevent_83", 254, new string[7] { "Character", "Character", "Character", "Character", "", "", "" }, null, 0, node: false));
		_dataArray.Add(new MonthlyEventItem(158, 255, EMonthlyEventType.NormalEvent, "ed865595-294f-4f01-a8b5-05471ddb7fbc", "sp_monthlyevent_83", 256, new string[7] { "Character", "Character", "", "", "", "", "" }, null, 0, node: false));
		_dataArray.Add(new MonthlyEventItem(159, 257, EMonthlyEventType.SpecialEvent, "27b43a01-08a1-43a5-9db6-9d763e98df92", "sp_monthlyevent_102", 258, new string[7] { "Character", "", "", "", "", "", "" }, null, 0, node: false));
		_dataArray.Add(new MonthlyEventItem(160, 257, EMonthlyEventType.SpecialEvent, "51909004-b87d-43a8-8314-c4c2b16069e4", "sp_monthlyevent_102", 258, new string[7] { "Character", "Location", "", "", "", "", "" }, null, 0, node: false));
		_dataArray.Add(new MonthlyEventItem(161, 257, EMonthlyEventType.SpecialEvent, "60d5fd24-78a0-4c69-8464-6045b69e4c3c", "sp_monthlyevent_102", 258, new string[7] { "Character", "", "", "", "", "", "" }, null, 0, node: false));
		_dataArray.Add(new MonthlyEventItem(162, 257, EMonthlyEventType.SpecialEvent, "b2c38eb6-111e-4af1-9e0a-161f48eaaa0f", "sp_monthlyevent_102", 258, new string[7] { "Character", "Location", "", "", "", "", "" }, null, 0, node: false));
		_dataArray.Add(new MonthlyEventItem(163, 257, EMonthlyEventType.SpecialEvent, "f519f090-9b64-478d-976b-d076a4642b08", "sp_monthlyevent_102", 258, new string[7] { "Text", "", "", "", "", "", "" }, null, 0, node: false));
		_dataArray.Add(new MonthlyEventItem(164, 259, EMonthlyEventType.LockedEvent, "29104798-3e21-4b86-a0b7-623a4595435b", "sp_monthlyevent_101", 260, new string[7] { "Character", "Location", "", "", "", "", "" }, null, 0, node: false));
		_dataArray.Add(new MonthlyEventItem(165, 261, EMonthlyEventType.NormalEvent, "922803b9-05b1-4e0a-ae1e-a131a9a02a03", "sp_monthlyevent_104", 262, new string[7] { "Character", "Character", "Location", "Character", "", "", "" }, null, 0, node: false));
		_dataArray.Add(new MonthlyEventItem(166, 263, EMonthlyEventType.NormalEvent, "41bc794d-cf11-429d-b9f8-70cc50004cc4", "sp_monthlyevent_105", 264, new string[7] { "Character", "Character", "", "", "", "", "" }, null, 0, node: false));
		_dataArray.Add(new MonthlyEventItem(167, 265, EMonthlyEventType.SpecialEvent, "b9011e4f-7c31-43c5-b84d-3b5ce32bdaf3", "sp_monthlyevent_106", 266, new string[7] { "Character", "", "", "", "", "", "" }, null, 0, node: false));
		_dataArray.Add(new MonthlyEventItem(168, 267, EMonthlyEventType.SpecialEvent, "0568abcc-847a-4b57-a1d4-e65f0fe4dd67", "sp_monthlyevent_112", 268, new string[7] { "", "", "", "", "", "", "" }, null, 0, node: false));
		_dataArray.Add(new MonthlyEventItem(169, 269, EMonthlyEventType.SpecialEvent, "6b55253d-f236-4de3-a3bf-f6073edeb39c", "sp_monthlyevent_111", 270, new string[7] { "", "", "", "", "", "", "" }, null, 0, node: false));
		_dataArray.Add(new MonthlyEventItem(170, 271, EMonthlyEventType.SpecialEvent, "45ef8b55-17b8-4329-9076-14510b775434", "sp_monthlyevent_109", 272, new string[7] { "", "", "", "", "", "", "" }, null, 0, node: false));
		_dataArray.Add(new MonthlyEventItem(171, 273, EMonthlyEventType.SpecialEvent, "28651c0f-ef75-46fe-bdc5-df0818a21fe1", "sp_monthlyevent_110", 274, new string[7] { "", "", "", "", "", "", "" }, null, 0, node: false));
		_dataArray.Add(new MonthlyEventItem(172, 275, EMonthlyEventType.SpecialEvent, "c1c1d957-f0d2-4d57-8094-21ff8060db0c", "sp_monthlyevent_127", 276, new string[7] { "", "", "", "", "", "", "" }, null, 0, node: false));
		_dataArray.Add(new MonthlyEventItem(173, 275, EMonthlyEventType.NormalEvent, "c1c1d957-f0d2-4d57-8094-21ff8060db0c", "sp_monthlyevent_127", 276, new string[7] { "", "", "", "", "", "", "" }, null, 0, node: false));
		_dataArray.Add(new MonthlyEventItem(174, 277, EMonthlyEventType.SpecialEvent, "d8878cc0-c603-4a29-b02a-d29706c24673", "sp_monthlyevent_127", 278, new string[7] { "", "", "", "", "", "", "" }, null, 0, node: false));
		_dataArray.Add(new MonthlyEventItem(175, 279, EMonthlyEventType.SpecialEvent, "e385827e-842e-455f-b53e-69526114a43e", "sp_monthlyevent_113", 280, new string[7] { "", "", "", "", "", "", "" }, null, 5, node: false));
		_dataArray.Add(new MonthlyEventItem(176, 281, EMonthlyEventType.SpecialEvent, "90e3fba9-635b-47e6-b855-52f44bf0fb08", "sp_monthlyevent_114", 282, new string[7] { "", "", "", "", "", "", "" }, null, 0, node: false));
		_dataArray.Add(new MonthlyEventItem(177, 283, EMonthlyEventType.SpecialEvent, "da6711cd-34e9-4e9e-b69a-1f157a42127e", "sp_monthlyevent_115", 284, new string[7] { "", "", "", "", "", "", "" }, null, 5, node: false));
		_dataArray.Add(new MonthlyEventItem(178, 285, EMonthlyEventType.SpecialEvent, "2a7bd0ce-5501-4903-895d-b12ab055f9f1", "sp_monthlyevent_116", 286, new string[7] { "", "", "", "", "", "", "" }, null, 5, node: false));
		_dataArray.Add(new MonthlyEventItem(179, 287, EMonthlyEventType.SpecialEvent, "8cc2a127-f3fb-47a7-b532-d7c720aadcbb", "sp_monthlyevent_117", 288, new string[7] { "", "", "", "", "", "", "" }, null, 3, node: false));
	}

	private void CreateItems3()
	{
		_dataArray.Add(new MonthlyEventItem(180, 289, EMonthlyEventType.SpecialEvent, "5a31b540-169d-4d0c-9f9c-1a0e916c1b90", "sp_monthlyevent_118", 290, new string[7] { "", "", "", "", "", "", "" }, null, 3, node: false));
		_dataArray.Add(new MonthlyEventItem(181, 291, EMonthlyEventType.SpecialEvent, "aa41ddfb-7486-4936-a4e0-95de149e0866", "sp_monthlyevent_119", 292, new string[7] { "", "", "", "", "", "", "" }, null, 5, node: false));
		_dataArray.Add(new MonthlyEventItem(182, 293, EMonthlyEventType.SpecialEvent, "0c0d8040-e32a-4c9b-89e8-d2a66deb77ad", "sp_monthlyevent_120", 294, new string[7] { "", "", "", "", "", "", "" }, null, 3, node: false));
		_dataArray.Add(new MonthlyEventItem(183, 295, EMonthlyEventType.SpecialEvent, "78902930-82c0-45ca-a8cf-d8b69fe45b9e", "sp_monthlyevent_121", 296, new string[7] { "", "", "", "", "", "", "" }, null, 5, node: false));
		_dataArray.Add(new MonthlyEventItem(184, 297, EMonthlyEventType.SpecialEvent, "68208a24-c2ee-4aee-baab-af39b8d520a6", "sp_monthlyevent_122", 298, new string[7] { "", "", "", "", "", "", "" }, null, 3, node: false));
		_dataArray.Add(new MonthlyEventItem(185, 299, EMonthlyEventType.SpecialEvent, "a7fd1ac2-dd4e-4bb2-b3ef-6c5c58d8ec54", "sp_monthlyevent_123", 300, new string[7] { "", "", "", "", "", "", "" }, null, 3, node: false));
		_dataArray.Add(new MonthlyEventItem(186, 301, EMonthlyEventType.SpecialEvent, "1f55792d-0cee-4fe4-8cfc-9bd14cc8bc0f", "sp_monthlyevent_124", 302, new string[7] { "", "", "", "", "", "", "" }, null, 5, node: false));
		_dataArray.Add(new MonthlyEventItem(187, 303, EMonthlyEventType.SpecialEvent, "dcad7254-ec58-4cc2-9137-d03ac4e13f02", "sp_monthlyevent_125", 304, new string[7] { "", "", "", "", "", "", "" }, null, 15, node: false));
		_dataArray.Add(new MonthlyEventItem(188, 305, EMonthlyEventType.SpecialEvent, "067d958c-97b8-4f83-bcbe-7ecabe60e4cb", "sp_monthlyevent_109", 306, new string[7] { "", "", "", "", "", "", "" }, null, 0, node: false));
		_dataArray.Add(new MonthlyEventItem(189, 307, EMonthlyEventType.SpecialEvent, "71808dbc-54e0-44b6-8294-0552e255d74c", "sp_monthlyevent_110", 308, new string[7] { "", "", "", "", "", "", "" }, null, 0, node: false));
		_dataArray.Add(new MonthlyEventItem(190, 309, EMonthlyEventType.NormalEvent, "08f5d9e1-7129-434c-ae5c-8e93a821cb73", "sp_monthlyevent_129", 310, new string[7] { "", "", "", "", "", "", "" }, null, 3, node: false));
		_dataArray.Add(new MonthlyEventItem(191, 311, EMonthlyEventType.SpecialEvent, "60d1ec84-bbb4-4ea6-8166-958256cff1cc", "sp_monthlyevent_130", 312, new string[7] { "", "", "", "", "", "", "" }, null, 5, node: false));
		_dataArray.Add(new MonthlyEventItem(192, 313, EMonthlyEventType.SpecialEvent, "3347bc78-93b7-4c5f-9b06-c6a216e6947c", "sp_monthlyevent_131", 314, new string[7] { "", "", "", "", "", "", "" }, null, 10, node: false));
		_dataArray.Add(new MonthlyEventItem(193, 315, EMonthlyEventType.SpecialEvent, "fcfce344-8973-47e7-809a-7c68cad15500", "sp_monthlyevent_131", 316, new string[7] { "", "", "", "", "", "", "" }, null, 10, node: false));
		_dataArray.Add(new MonthlyEventItem(194, 317, EMonthlyEventType.NormalEvent, "fd31d2eb-bdfd-4ee1-932d-ee4aa9da6ed1", "sp_monthlyevent_132", 318, new string[7] { "", "", "", "", "", "", "" }, null, 5, node: false));
		_dataArray.Add(new MonthlyEventItem(195, 319, EMonthlyEventType.NormalEvent, "2e13b2da-b596-4fd5-a0a6-2621887e9d7f", "sp_monthlyevent_133", 320, new string[7] { "", "", "", "", "", "", "" }, null, 10, node: false));
		_dataArray.Add(new MonthlyEventItem(196, 321, EMonthlyEventType.NormalEvent, "6847842a-bd1e-4794-a939-263d2f7a144e", "sp_monthlyevent_134", 322, new string[7] { "", "", "", "", "", "", "" }, null, 15, node: false));
		_dataArray.Add(new MonthlyEventItem(197, 321, EMonthlyEventType.NormalEvent, "bea0d4a5-9bf8-4cff-8169-2512213ff7b4", "sp_monthlyevent_134", 323, new string[7] { "", "", "", "", "", "", "" }, null, 15, node: false));
		_dataArray.Add(new MonthlyEventItem(198, 324, EMonthlyEventType.SpecialEvent, "57fae04a-a3e9-4a27-af66-07e0987482dd", "sp_monthlyevent_109", 325, new string[7] { "", "", "", "", "", "", "" }, null, 0, node: false));
		_dataArray.Add(new MonthlyEventItem(199, 326, EMonthlyEventType.SpecialEvent, "5ad8be88-9798-4595-bbfe-905a0c60a0a9", "sp_monthlyevent_110", 327, new string[7] { "", "", "", "", "", "", "" }, null, 0, node: false));
		_dataArray.Add(new MonthlyEventItem(200, 328, EMonthlyEventType.SpecialEvent, "b3df5518-fcbb-4407-ae57-ab49fba4dd95", "sp_monthlyevent_152", 329, new string[7] { "", "", "", "", "", "", "" }, null, 10, node: false));
		_dataArray.Add(new MonthlyEventItem(201, 330, EMonthlyEventType.SpecialEvent, "a17b887c-9028-409d-ba61-ea34c72fd978", "sp_monthlyevent_153", 331, new string[7] { "", "", "", "", "", "", "" }, null, 5, node: false));
		_dataArray.Add(new MonthlyEventItem(202, 332, EMonthlyEventType.SpecialEvent, "989bf6bb-ae10-40da-9eb7-f13dc2d8ecbc", "sp_monthlyevent_154", 333, new string[7] { "Character", "", "", "", "", "", "" }, null, 10, node: false));
		_dataArray.Add(new MonthlyEventItem(203, 334, EMonthlyEventType.SpecialEvent, "84c837c7-504c-455b-b65f-aa9d6d162354", "sp_monthlyevent_109", 335, new string[7] { "", "", "", "", "", "", "" }, null, 0, node: false));
		_dataArray.Add(new MonthlyEventItem(204, 336, EMonthlyEventType.SpecialEvent, "3e7ba69b-59b4-40ed-afd4-42c8efef5090", "sp_monthlyevent_110", 337, new string[7] { "", "", "", "", "", "", "" }, null, 0, node: false));
		_dataArray.Add(new MonthlyEventItem(205, 338, EMonthlyEventType.NormalEvent, "03546449-7d1a-453e-8676-32b4eecbb76a", "sp_monthlyevent_149", 339, new string[7] { "", "", "", "", "", "", "" }, null, 0, node: false));
		_dataArray.Add(new MonthlyEventItem(206, 340, EMonthlyEventType.SpecialEvent, "af960b8f-513f-4f89-af10-a07e6180a707", "sp_monthlyevent_147", 341, new string[7] { "Character", "Location", "", "", "", "", "" }, null, 5, node: false));
		_dataArray.Add(new MonthlyEventItem(207, 342, EMonthlyEventType.NormalEvent, "70bebdd1-a192-4175-bf4a-51e4bc9b91a2", "sp_monthlyevent_151", 343, new string[7] { "Location", "Character", "", "", "", "", "" }, null, 0, node: false));
		_dataArray.Add(new MonthlyEventItem(208, 344, EMonthlyEventType.SpecialEvent, "2a78b810-5146-4ca3-b867-2650b453d1de", "sp_monthlyevent_109", 345, new string[7] { "", "", "", "", "", "", "" }, null, 10, node: false));
		_dataArray.Add(new MonthlyEventItem(209, 346, EMonthlyEventType.SpecialEvent, "4e19e07c-13fb-4118-92f1-122f1d234cc0", "sp_monthlyevent_110", 347, new string[7] { "", "", "", "", "", "", "" }, null, 10, node: false));
		_dataArray.Add(new MonthlyEventItem(210, 16, EMonthlyEventType.SpecialEvent, "926bcace-f1b1-4b3d-8872-d51defaf8cfc", "sp_monthlyevent_110", 17, new string[7] { "Location", "Character", "", "", "", "", "" }, null, 0, node: false));
		_dataArray.Add(new MonthlyEventItem(211, 348, EMonthlyEventType.SpecialEvent, "d96e9f5a-04d3-444a-816d-c4df7fb26a0a", "sp_monthlyevent_110", 349, new string[7] { "", "", "", "", "", "", "" }, null, 0, node: false));
		_dataArray.Add(new MonthlyEventItem(212, 350, EMonthlyEventType.SpecialEvent, "fed84ccf-8d37-4c68-b593-2ee460c3adca", "sp_monthlyevent_110", 351, new string[7] { "", "", "", "", "", "", "" }, null, 0, node: false));
		_dataArray.Add(new MonthlyEventItem(213, 352, EMonthlyEventType.SpecialEvent, "83c11763-4c6c-414d-ab16-5f15e091faa2", "sp_monthlyevent_109", 353, new string[7] { "", "", "", "", "", "", "" }, null, 0, node: false));
		_dataArray.Add(new MonthlyEventItem(214, 354, EMonthlyEventType.SpecialEvent, "2367b03a-7001-4cea-b8d0-6dd401b2199e", "sp_monthlyevent_137", 355, new string[7] { "", "", "", "", "", "", "" }, null, 5, node: false));
		_dataArray.Add(new MonthlyEventItem(215, 356, EMonthlyEventType.SpecialEvent, "2ad527af-f91d-46d3-a9a5-882c60089d4c", "sp_monthlyevent_138", 357, new string[7] { "Location", "", "", "", "", "", "" }, null, 3, node: false));
		_dataArray.Add(new MonthlyEventItem(216, 358, EMonthlyEventType.SpecialEvent, "0e74c5a1-e79a-4449-9067-d7530be59840", "sp_monthlyevent_139", 359, new string[7] { "Location", "", "", "", "", "", "" }, null, 3, node: false));
		_dataArray.Add(new MonthlyEventItem(217, 360, EMonthlyEventType.NormalEvent, "2f802ef3-8e97-4f61-a991-88f5fbe3bf44", "sp_monthlyevent_140", 361, new string[7] { "Character", "Location", "Character", "", "", "", "" }, null, 0, node: false));
		_dataArray.Add(new MonthlyEventItem(218, 362, EMonthlyEventType.SpecialEvent, "f30fe1fc-bd13-45cf-b290-4e470f5820c6", "sp_monthlyevent_110", 363, new string[7] { "Location", "", "", "", "", "", "" }, null, 10, node: false));
		_dataArray.Add(new MonthlyEventItem(219, 364, EMonthlyEventType.SpecialEvent, "c598abbf-c486-4ecb-be6f-123b6a29b712", null, 365, new string[7] { "Character", "Location", "", "", "", "", "" }, null, 0, node: false));
		_dataArray.Add(new MonthlyEventItem(220, 366, EMonthlyEventType.SpecialEvent, "f75577f5-d198-4e4a-8467-f7cffc7c63cc", "sp_monthlyevent_183", 367, new string[7] { "", "", "", "", "", "", "" }, null, 3, node: false));
		_dataArray.Add(new MonthlyEventItem(221, 368, EMonthlyEventType.SpecialEvent, "676bffe7-07b2-409d-873e-3055fda6abd8", "sp_monthlyevent_179", 369, new string[7] { "", "", "", "", "", "", "" }, null, 5, node: false));
		_dataArray.Add(new MonthlyEventItem(222, 370, EMonthlyEventType.SpecialEvent, "ed39ab40-2295-4a0b-b0fb-4ed6865b5091", null, 371, new string[7] { "Character", "", "", "", "", "", "" }, null, 0, node: false));
		_dataArray.Add(new MonthlyEventItem(223, 372, EMonthlyEventType.SpecialEvent, "fe50e807-84d5-4da3-b8fd-e56d04ff3ff7", "sp_monthlyevent_180", 373, new string[7] { "", "", "", "", "", "", "" }, null, 10, node: false));
		_dataArray.Add(new MonthlyEventItem(224, 374, EMonthlyEventType.NormalEvent, "5c8591ab-b9b4-44ce-bc6d-a1cc6c5d9c75", "sp_monthlyevent_181", 375, new string[7] { "", "", "", "", "", "", "" }, null, 0, node: false));
		_dataArray.Add(new MonthlyEventItem(225, 376, EMonthlyEventType.SpecialEvent, "437cce36-d3c1-481c-af00-75edcf125fcb", "sp_monthlyevent_109", 377, new string[7] { "", "", "", "", "", "", "" }, null, 10, node: false));
		_dataArray.Add(new MonthlyEventItem(226, 378, EMonthlyEventType.SpecialEvent, "b5c4c984-0587-4da0-b292-1f20548dbd95", "sp_monthlyevent_110", 379, new string[7] { "", "", "", "", "", "", "" }, null, 10, node: false));
		_dataArray.Add(new MonthlyEventItem(227, 380, EMonthlyEventType.SpecialEvent, "aaf1e541-cd5d-4778-85d9-2326a95a2ca5", "sp_monthlyevent_193", 381, new string[7] { "Character", "", "", "", "", "", "" }, null, 3, node: false));
		_dataArray.Add(new MonthlyEventItem(228, 382, EMonthlyEventType.SpecialEvent, "5f0a82a3-2731-4ce3-b4d3-862cc68697ed", "sp_monthlyevent_109", 383, new string[7] { "", "", "", "", "", "", "" }, null, 10, node: false));
		_dataArray.Add(new MonthlyEventItem(229, 384, EMonthlyEventType.SpecialEvent, "6b8df43e-efc2-4aa2-a7eb-49fcf02d73d5", "sp_monthlyevent_110", 385, new string[7] { "", "", "", "", "", "", "" }, null, 10, node: false));
		_dataArray.Add(new MonthlyEventItem(230, 384, EMonthlyEventType.SpecialEvent, "c10c1a24-dafe-491a-9eea-64b08123539b", "sp_monthlyevent_110", 385, new string[7] { "", "", "", "", "", "", "" }, null, 10, node: false));
		_dataArray.Add(new MonthlyEventItem(231, 386, EMonthlyEventType.SpecialEvent, "a9db6b16-b367-44b1-9b76-80892ed1f173", "sp_monthlyevent_195", 387, new string[7] { "", "", "", "", "", "", "" }, null, 3, node: false));
		_dataArray.Add(new MonthlyEventItem(232, 388, EMonthlyEventType.SpecialEvent, null, "sp_monthlyevent_196", 389, new string[7] { "", "", "", "", "", "", "" }, null, 0, node: false));
		_dataArray.Add(new MonthlyEventItem(233, 388, EMonthlyEventType.NormalEvent, "3f76f33a-8abc-4879-ad6b-c651aa52398a", "sp_monthlyevent_196", 390, new string[7] { "Character", "Location", "", "", "", "", "" }, null, 0, node: false));
		_dataArray.Add(new MonthlyEventItem(234, 391, EMonthlyEventType.SpecialEvent, "df121bbe-9b67-4e26-b7c1-8ba4244be999", "sp_monthlyevent_109", 392, new string[7] { "", "", "", "", "", "", "" }, null, 0, node: false));
		_dataArray.Add(new MonthlyEventItem(235, 393, EMonthlyEventType.SpecialEvent, "60c5f0d2-e269-4770-a39e-875926aaf5c9", "sp_monthlyevent_110", 394, new string[7] { "", "", "", "", "", "", "" }, null, 0, node: false));
		_dataArray.Add(new MonthlyEventItem(236, 395, EMonthlyEventType.SpecialEvent, null, "sp_monthlyevent_110", 396, new string[7] { "", "", "", "", "", "", "" }, null, 0, node: false));
		_dataArray.Add(new MonthlyEventItem(237, 395, EMonthlyEventType.SpecialEvent, null, "sp_monthlyevent_110", 397, new string[7] { "", "", "", "", "", "", "" }, null, 0, node: false));
		_dataArray.Add(new MonthlyEventItem(238, 398, EMonthlyEventType.SpecialEvent, null, "sp_monthlyevent_109", 398, new string[7] { "", "", "", "", "", "", "" }, null, 0, node: false));
		_dataArray.Add(new MonthlyEventItem(239, 399, EMonthlyEventType.SpecialEvent, null, "sp_monthlyevent_110", 399, new string[7] { "", "", "", "", "", "", "" }, null, 0, node: false));
	}

	private void CreateItems4()
	{
		_dataArray.Add(new MonthlyEventItem(240, 400, EMonthlyEventType.LockedEvent, null, "sp_monthlyevent_110", 401, new string[7] { "", "", "", "", "", "", "" }, null, 0, node: false));
		_dataArray.Add(new MonthlyEventItem(241, 402, EMonthlyEventType.NormalEvent, "fe479d84-2400-4751-9f3a-99af9e2b9f88", "sp_monthlyevent_126", 403, new string[7] { "", "", "", "", "", "", "" }, null, 3, node: false));
		_dataArray.Add(new MonthlyEventItem(242, 404, EMonthlyEventType.SpecialEvent, "65c3a6ab-3443-4da9-b9f1-ab35e514cef3", "sp_monthlyevent_127", 405, new string[7] { "Character", "", "", "", "", "", "" }, null, 3, node: false));
		_dataArray.Add(new MonthlyEventItem(243, 406, EMonthlyEventType.SpecialEvent, "40e09cf5-15c9-4c41-961a-26003ba91e07", "sp_monthlyevent_128", 407, new string[7] { "", "", "", "", "", "", "" }, null, 5, node: false));
		_dataArray.Add(new MonthlyEventItem(244, 408, EMonthlyEventType.SpecialEvent, "85b0efd1-4674-4a9a-8c75-2266726ec428", "sp_monthlyevent_201", 409, new string[7] { "Character", "Location", "", "", "", "", "" }, null, 3, node: false));
		_dataArray.Add(new MonthlyEventItem(245, 410, EMonthlyEventType.SpecialEvent, "c973d8d3-3bf1-4b7d-9f0b-e1cdd0e90947", "sp_monthlyevent_202", 411, new string[7] { "Character", "Location", "", "", "", "", "" }, null, 3, node: false));
		_dataArray.Add(new MonthlyEventItem(246, 412, EMonthlyEventType.SpecialEvent, "f653a3a1-3481-433c-9413-cef06235dd8a", "sp_monthlyevent_203", 413, new string[7] { "Character", "Location", "", "", "", "", "" }, null, 3, node: false));
		_dataArray.Add(new MonthlyEventItem(247, 414, EMonthlyEventType.SpecialEvent, null, "sp_monthlyevent_110", 415, new string[7] { "", "", "", "", "", "", "" }, null, 0, node: false));
		_dataArray.Add(new MonthlyEventItem(248, 416, EMonthlyEventType.SpecialEvent, null, "sp_monthlyevent_110", 417, new string[7] { "", "", "", "", "", "", "" }, null, 0, node: false));
		_dataArray.Add(new MonthlyEventItem(249, 418, EMonthlyEventType.SpecialEvent, null, "sp_monthlyevent_110", 419, new string[7] { "", "", "", "", "", "", "" }, null, 0, node: false));
		_dataArray.Add(new MonthlyEventItem(250, 420, EMonthlyEventType.SpecialEvent, null, "sp_monthlyevent_110", 421, new string[7] { "", "", "", "", "", "", "" }, null, 0, node: false));
		_dataArray.Add(new MonthlyEventItem(251, 422, EMonthlyEventType.SpecialEvent, null, "sp_monthlyevent_110", 423, new string[7] { "", "", "", "", "", "", "" }, null, 0, node: false));
		_dataArray.Add(new MonthlyEventItem(252, 424, EMonthlyEventType.SpecialEvent, null, "sp_monthlyevent_110", 425, new string[7] { "", "", "", "", "", "", "" }, null, 0, node: false));
		_dataArray.Add(new MonthlyEventItem(253, 426, EMonthlyEventType.NormalEvent, null, "sp_monthlyevent_110", 427, new string[7] { "", "", "", "", "", "", "" }, null, 0, node: false));
		_dataArray.Add(new MonthlyEventItem(254, 428, EMonthlyEventType.SpecialEvent, "6a39447f-793d-4b27-b7e9-568824a42a77", "sp_monthlyevent_109", 429, new string[7] { "", "", "", "", "", "", "" }, null, 1, node: false));
		_dataArray.Add(new MonthlyEventItem(255, 430, EMonthlyEventType.SpecialEvent, "96c45490-928f-49e0-afc8-2072c8d0ddf7", "sp_monthlyevent_110", 431, new string[7] { "", "", "", "", "", "", "" }, null, 1, node: false));
		_dataArray.Add(new MonthlyEventItem(256, 313, EMonthlyEventType.SpecialEvent, "20a4c183-a9ee-4381-865a-798fcb3d557b", "sp_monthlyevent_131", 314, new string[7] { "", "", "", "", "", "", "" }, null, 15, node: false));
		_dataArray.Add(new MonthlyEventItem(257, 313, EMonthlyEventType.SpecialEvent, "fa0acdb4-0250-4991-b953-1ac00bc6122c", "sp_monthlyevent_131", 314, new string[7] { "", "", "", "", "", "", "" }, null, 15, node: false));
		_dataArray.Add(new MonthlyEventItem(258, 432, EMonthlyEventType.SpecialEvent, "3b261a2c-eea8-4a2a-af07-4be334a00837", "sp_monthlyevent_136", 433, new string[7] { "", "", "", "", "", "", "" }, null, 10, node: false));
		_dataArray.Add(new MonthlyEventItem(259, 317, EMonthlyEventType.NormalEvent, "fd31d2eb-bdfd-4ee1-932d-ee4aa9da6ed1", "sp_monthlyevent_132", 318, new string[7] { "", "", "", "", "", "", "" }, null, 3, node: false));
		_dataArray.Add(new MonthlyEventItem(260, 434, EMonthlyEventType.NormalEvent, "2ff827bb-2f5e-494e-9d42-ee6838f3ac61", "sp_monthlyevent_140", 435, new string[7] { "Character", "Location", "ItemKey", "Character", "", "", "" }, null, 3, node: false));
		_dataArray.Add(new MonthlyEventItem(261, 434, EMonthlyEventType.NormalEvent, "a86a19e6-e10a-4ea2-94fa-318f97418e0e", "sp_monthlyevent_140", 436, new string[7] { "Character", "Location", "ItemKey", "Character", "", "", "" }, null, 3, node: false));
		_dataArray.Add(new MonthlyEventItem(262, 437, EMonthlyEventType.NormalEvent, "c16f1945-aa6b-4e2f-8278-daad37c08ab3", "sp_monthlyevent_141", 438, new string[7] { "", "", "", "", "", "", "" }, null, 3, node: false));
		_dataArray.Add(new MonthlyEventItem(263, 439, EMonthlyEventType.SpecialEvent, "b719c0b9-8f69-409e-a20a-6c16f0f8056a", "sp_monthlyevent_109", 440, new string[7] { "Location", "", "", "", "", "", "" }, null, 10, node: false));
		_dataArray.Add(new MonthlyEventItem(264, 441, EMonthlyEventType.NormalEvent, "2e13b2da-b596-4fd5-a0a6-2621887e9d7f", "sp_monthlyevent_133", 320, new string[7] { "", "", "", "", "", "", "" }, null, 0, node: false));
		_dataArray.Add(new MonthlyEventItem(265, 321, EMonthlyEventType.NormalEvent, "6847842a-bd1e-4794-a939-263d2f7a144e", "sp_monthlyevent_134", 322, new string[7] { "", "", "", "", "", "", "" }, null, 0, node: false));
		_dataArray.Add(new MonthlyEventItem(266, 321, EMonthlyEventType.NormalEvent, "bea0d4a5-9bf8-4cff-8169-2512213ff7b4", "sp_monthlyevent_134", 323, new string[7] { "", "", "", "", "", "", "" }, null, 0, node: false));
		_dataArray.Add(new MonthlyEventItem(267, 442, EMonthlyEventType.SpecialEvent, "956394bd-b511-41cc-ab62-d037470ec1c8", "sp_monthlyevent_138", 443, new string[7] { "Character", "", "", "", "", "", "" }, null, 5, node: false));
		_dataArray.Add(new MonthlyEventItem(268, 437, EMonthlyEventType.NormalEvent, "038f7b78-93dc-4b64-81de-bdfd9d8979bd", "sp_monthlyevent_141", 438, new string[7] { "", "", "", "", "", "", "" }, null, 3, node: false));
		_dataArray.Add(new MonthlyEventItem(269, 444, EMonthlyEventType.SpecialEvent, "38e13a25-c1db-4850-a71a-91f84a0a9509", "sp_monthlyevent_142", 445, new string[7] { "", "", "", "", "", "", "" }, null, 10, node: false));
		_dataArray.Add(new MonthlyEventItem(270, 446, EMonthlyEventType.SpecialEvent, "c0e7e5e5-d4db-4499-b2bf-2de32038ddd0", "sp_monthlyevent_143", 447, new string[7] { "", "", "", "", "", "", "" }, null, 0, node: false));
		_dataArray.Add(new MonthlyEventItem(271, 448, EMonthlyEventType.SpecialEvent, "dad2487c-6534-4130-a2da-aec5784fa83d", "sp_monthlyevent_141", 449, new string[7] { "", "", "", "", "", "", "" }, null, 5, node: false));
		_dataArray.Add(new MonthlyEventItem(272, 450, EMonthlyEventType.NormalEvent, "f87e13e8-96e5-4590-a979-312530279465", "sp_monthlyevent_145", 451, new string[7] { "", "", "", "", "", "", "" }, null, 0, node: false));
		_dataArray.Add(new MonthlyEventItem(273, 452, EMonthlyEventType.NormalEvent, "45870f83-4919-4241-b15b-a3f6e644ec47", "sp_monthlyevent_146", 453, new string[7] { "Location", "", "", "", "", "", "" }, null, 0, node: false));
		_dataArray.Add(new MonthlyEventItem(274, 454, EMonthlyEventType.SpecialEvent, "72431c90-738c-4944-8832-c6b8148c14e2", "sp_monthlyevent_147", 455, new string[7] { "Character", "Location", "", "", "", "", "" }, null, 5, node: false));
		_dataArray.Add(new MonthlyEventItem(275, 456, EMonthlyEventType.SpecialEvent, "ee6f37d3-32e0-47e3-9c25-d082ee1240cd", "sp_monthlyevent_148", 457, new string[7] { "Character", "Location", "Character", "Location", "", "", "" }, null, 10, node: false));
		_dataArray.Add(new MonthlyEventItem(276, 458, EMonthlyEventType.SpecialEvent, "0f499b42-2aba-4987-a60b-a00d5e75b84e", "sp_monthlyevent_145", 459, new string[7] { "Character", "Location", "", "", "", "", "" }, null, 5, node: false));
		_dataArray.Add(new MonthlyEventItem(277, 454, EMonthlyEventType.SpecialEvent, "6dea23a6-af77-4a03-b41b-d8ff0308252a", "sp_monthlyevent_147", 460, new string[7] { "Character", "Location", "", "", "", "", "" }, null, 5, node: false));
		_dataArray.Add(new MonthlyEventItem(278, 461, EMonthlyEventType.SpecialEvent, "290e09c4-e3f3-4891-8d99-780fbcf36c15", "sp_monthlyevent_155", 462, new string[7] { "Character", "", "", "", "", "", "" }, null, 10, node: false));
		_dataArray.Add(new MonthlyEventItem(279, 463, EMonthlyEventType.SpecialEvent, "6b9402df-12b2-400d-9132-b294b2bda8c3", "sp_monthlyevent_156", 464, new string[7] { "Character", "", "", "", "", "", "" }, null, 5, node: false));
		_dataArray.Add(new MonthlyEventItem(280, 465, EMonthlyEventType.SpecialEvent, "2f1aad09-e04e-41e6-84b9-0639980ade8b", "sp_monthlyevent_157", 466, new string[7] { "Character", "", "", "", "", "", "" }, null, 15, node: false));
		_dataArray.Add(new MonthlyEventItem(281, 467, EMonthlyEventType.NormalEvent, "18cdf205-9302-4054-baed-a1ac6a541f53", "sp_monthlyevent_158", 468, new string[7] { "", "", "", "", "", "", "" }, null, 3, node: false));
		_dataArray.Add(new MonthlyEventItem(282, 332, EMonthlyEventType.SpecialEvent, "82826966-26b7-4b56-829e-80851430fc25", "sp_monthlyevent_154", 469, new string[7] { "Character", "", "", "", "", "", "" }, null, 10, node: false));
		_dataArray.Add(new MonthlyEventItem(283, 470, EMonthlyEventType.SpecialEvent, "0177e00a-f900-457a-abd3-ac7bffd3cbcf", "sp_monthlyevent_159", 471, new string[7] { "", "", "", "", "", "", "" }, null, 3, node: false));
		_dataArray.Add(new MonthlyEventItem(284, 472, EMonthlyEventType.SpecialEvent, "610b431a-a4a3-46e7-9119-9a6f34e42858", "sp_monthlyevent_160", 473, new string[7] { "", "", "", "", "", "", "" }, null, 5, node: false));
		_dataArray.Add(new MonthlyEventItem(285, 474, EMonthlyEventType.SpecialEvent, "56f6b54f-2e83-46eb-8798-d263d542a61c", "sp_monthlyevent_153", 475, new string[7] { "Character", "", "", "", "", "", "" }, null, 15, node: false));
		_dataArray.Add(new MonthlyEventItem(286, 476, EMonthlyEventType.NormalEvent, "7f06e8eb-e8c2-4364-ab5d-b4425ba5b7a5", "sp_monthlyevent_161", 477, new string[7] { "Location", "", "", "", "", "", "" }, null, 0, node: false));
		_dataArray.Add(new MonthlyEventItem(287, 478, EMonthlyEventType.SpecialEvent, "1d97e2a9-ff13-4f07-81de-a9ae167f1f5d", "sp_monthlyevent_162", 479, new string[7] { "Character", "Location", "", "", "", "", "" }, null, 15, node: false));
		_dataArray.Add(new MonthlyEventItem(288, 450, EMonthlyEventType.NormalEvent, "78442db8-b67b-4bff-a01b-18a59907eca4", "sp_monthlyevent_248", 480, new string[7] { "", "", "", "", "", "", "" }, null, 0, node: false));
		_dataArray.Add(new MonthlyEventItem(289, 481, EMonthlyEventType.SpecialEvent, "b2f60857-1468-4241-9884-5f3533e0194d", "sp_monthlyevent_163", 482, new string[7] { "Character", "Location", "", "", "", "", "" }, null, 1, node: false));
		_dataArray.Add(new MonthlyEventItem(290, 483, EMonthlyEventType.NormalEvent, "54a700fe-f097-4cec-a5cd-9cbb68587524", "sp_monthlyevent_164", 484, new string[7] { "Character", "Location", "Character", "CombatSkill", "", "", "" }, null, 0, node: false));
		_dataArray.Add(new MonthlyEventItem(291, 485, EMonthlyEventType.NormalEvent, "11211fd5-8cfd-4113-a948-f5e587cdea1a", "sp_monthlyevent_165", 486, new string[7] { "Character", "Location", "", "", "", "", "" }, null, 0, node: false));
		_dataArray.Add(new MonthlyEventItem(292, 487, EMonthlyEventType.SpecialEvent, "1a53cd4f-b022-46ff-915c-0da6a6044a41", "sp_monthlyevent_166", 488, new string[7] { "CharacterTemplate", "Location", "ItemKey", "Location", "", "", "" }, null, 0, node: false));
		_dataArray.Add(new MonthlyEventItem(293, 489, EMonthlyEventType.SpecialEvent, "583308f4-2ade-4ec5-831e-dcfca90d4e98", "sp_monthlyevent_167", 490, new string[7] { "", "", "", "", "", "", "" }, null, 1, node: false));
		_dataArray.Add(new MonthlyEventItem(294, 491, EMonthlyEventType.NormalEvent, "8e36522c-c6df-4516-a478-724a06cc3fec", "sp_monthlyevent_168", 492, new string[7] { "Location", "JiaoLoong", "", "", "", "", "" }, null, 0, node: false));
		_dataArray.Add(new MonthlyEventItem(295, 493, EMonthlyEventType.NormalEvent, "02c1b765-7d54-4c3a-a3ab-7718e2e20092", "sp_monthlyevent_169", 494, new string[7] { "Character", "JiaoLoong", "", "", "", "", "" }, null, 0, node: false));
		_dataArray.Add(new MonthlyEventItem(296, 495, EMonthlyEventType.NormalEvent, "11dcfeeb-4c76-41cf-b08a-8fc907457706", "sp_monthlyevent_170", 494, new string[7] { "Character", "JiaoLoong", "Character", "", "", "", "" }, null, 0, node: false));
		_dataArray.Add(new MonthlyEventItem(297, 496, EMonthlyEventType.NormalEvent, "4991f260-4991-4f1e-9cec-681d8202a71c", "sp_monthlyevent_171", 494, new string[7] { "Character", "JiaoLoong", "Character", "", "", "", "" }, null, 0, node: false));
		_dataArray.Add(new MonthlyEventItem(298, 497, EMonthlyEventType.NormalEvent, "8248d3a2-4688-4c38-a09a-e4ddd3a28e69", "sp_monthlyevent_172", 494, new string[7] { "Character", "JiaoLoong", "Cricket", "", "", "", "" }, null, 0, node: false));
		_dataArray.Add(new MonthlyEventItem(299, 498, EMonthlyEventType.NormalEvent, "a3fb0a88-1cee-4b52-b587-676af7a2c0cc", "sp_monthlyevent_173", 494, new string[7] { "Character", "JiaoLoong", "Item", "Integer", "", "", "" }, null, 0, node: false));
	}

	private void CreateItems5()
	{
		_dataArray.Add(new MonthlyEventItem(300, 499, EMonthlyEventType.NormalEvent, "843fefb2-3dae-47e6-897a-8d343c5c8f89", "sp_monthlyevent_174", 494, new string[7] { "Character", "JiaoLoong", "", "", "", "", "" }, null, 0, node: false));
		_dataArray.Add(new MonthlyEventItem(301, 500, EMonthlyEventType.NormalEvent, "919d1214-b3ae-46e0-84a0-21898e812138", "sp_monthlyevent_175", 494, new string[7] { "Character", "JiaoLoong", "", "", "", "", "" }, null, 0, node: false));
		_dataArray.Add(new MonthlyEventItem(302, 501, EMonthlyEventType.NormalEvent, "d774a0d8-62c5-4691-88b9-51ff3c981c29", "sp_monthlyevent_176", 494, new string[7] { "Character", "JiaoLoong", "Item", "Integer", "", "", "" }, null, 0, node: false));
		_dataArray.Add(new MonthlyEventItem(303, 502, EMonthlyEventType.NormalEvent, "c7a6bef9-8418-4ff4-9554-99826ad85792", "sp_monthlyevent_177", 494, new string[7] { "Character", "JiaoLoong", "", "", "", "", "" }, null, 0, node: false));
		_dataArray.Add(new MonthlyEventItem(304, 503, EMonthlyEventType.SpecialEvent, "6be0d9de-1e17-49b6-a25b-bf0b162d30fb", "sp_monthlyevent_178", 504, new string[7] { "Character", "Location", "CharacterTemplate", "", "", "", "" }, null, 0, node: false));
		_dataArray.Add(new MonthlyEventItem(305, 505, EMonthlyEventType.SpecialEvent, "0db9a1eb-1206-4de6-bd06-dad4fc294af2", "sp_monthlyevent_168", 506, new string[7] { "Location", "JiaoLoong", "", "", "", "", "" }, null, 0, node: false));
		_dataArray.Add(new MonthlyEventItem(306, 507, EMonthlyEventType.SpecialEvent, "99b77a8f-0bd2-409b-a647-c2c11a10cd6c", "sp_monthlyevent_194", 508, new string[7] { "Character", "Location", "", "", "", "", "" }, null, 5, node: false));
		_dataArray.Add(new MonthlyEventItem(307, 509, EMonthlyEventType.SpecialEvent, "076691a0-9b1a-4d8a-a2e7-81802d55744f", "sp_monthlyevent_182", 510, new string[7] { "", "", "", "", "", "", "" }, null, 5, node: false));
		_dataArray.Add(new MonthlyEventItem(308, 511, EMonthlyEventType.NormalEvent, "35edba36-194a-47dd-8198-17f1fcb48855", "sp_monthlyevent_184", 512, new string[7] { "Character", "", "", "", "", "", "" }, null, 0, node: false));
		_dataArray.Add(new MonthlyEventItem(309, 513, EMonthlyEventType.SpecialEvent, "fc5dd1ce-07bb-41c9-a49c-28317470f30a", "sp_monthlyevent_185", 514, new string[7] { "Character", "", "", "", "", "", "" }, null, 5, node: false));
		_dataArray.Add(new MonthlyEventItem(310, 515, EMonthlyEventType.SpecialEvent, "f0c3de42-c5b1-41e4-8a65-d0c1b03c2be5", "sp_monthlyevent_191", 516, new string[7] { "", "", "", "", "", "", "" }, null, 10, node: false));
		_dataArray.Add(new MonthlyEventItem(311, 517, EMonthlyEventType.SpecialEvent, "cef4e4b5-6f81-4da1-937e-ffe0dbb837b8", "sp_monthlyevent_186", 518, new string[7] { "", "", "", "", "", "", "" }, null, 3, node: false));
		_dataArray.Add(new MonthlyEventItem(312, 519, EMonthlyEventType.SpecialEvent, "b48ad5b0-cdde-488f-8ced-a589e9b5d065", "sp_monthlyevent_192", 520, new string[7] { "Character", "", "", "", "", "", "" }, null, 15, node: false));
		_dataArray.Add(new MonthlyEventItem(313, 521, EMonthlyEventType.SpecialEvent, "38eb8243-00ca-4b34-b9aa-6f37142fc360", "sp_monthlyevent_190", 522, new string[7] { "", "", "", "", "", "", "" }, null, 2, node: false));
		_dataArray.Add(new MonthlyEventItem(314, 523, EMonthlyEventType.SpecialEvent, "5110afaa-5b5f-4a8f-ab7a-affe2142b541", "sp_monthlyevent_187", 524, new string[7] { "Character", "", "", "", "", "", "" }, null, 3, node: false));
		_dataArray.Add(new MonthlyEventItem(315, 525, EMonthlyEventType.SpecialEvent, "5381846f-e1e6-4720-9601-6aadd02313ec", "sp_monthlyevent_189", 526, new string[7] { "", "", "", "", "", "", "" }, null, 3, node: false));
		_dataArray.Add(new MonthlyEventItem(316, 527, EMonthlyEventType.SpecialEvent, "90e11369-75ee-45d7-a1ea-752432cfc7d9", "sp_monthlyevent_197", 528, new string[7] { "Location", "", "", "", "", "", "" }, null, 5, node: false));
		_dataArray.Add(new MonthlyEventItem(317, 529, EMonthlyEventType.SpecialEvent, "e0bdd40f-b647-4534-b11e-efee1d634c9f", "sp_monthlyevent_198", 530, new string[7] { "", "", "", "", "", "", "" }, null, 3, node: false));
		_dataArray.Add(new MonthlyEventItem(318, 531, EMonthlyEventType.NormalEvent, "c55bb515-e8bd-4d7b-b8f4-40ef920ed614", "sp_monthlyevent_199", 532, new string[7] { "Character", "", "", "", "", "", "" }, null, 0, node: false));
		_dataArray.Add(new MonthlyEventItem(319, 533, EMonthlyEventType.SpecialEvent, "6b84caac-74c7-4761-b334-edea12fdb200", "sp_monthlyevent_200", 534, new string[7] { "Character", "Location", "", "", "", "", "" }, null, 5, node: false));
		_dataArray.Add(new MonthlyEventItem(320, 535, EMonthlyEventType.SpecialEvent, "6121c37a-f66a-421c-84dc-687ed3da8568", "sp_monthlyevent_204", 536, new string[7] { "Character", "", "", "", "", "", "" }, null, 15, node: false));
		_dataArray.Add(new MonthlyEventItem(321, 537, EMonthlyEventType.SpecialEvent, "c3a4819a-d6c5-41a8-a1a7-9fc32e33f6b2", "sp_monthlyevent_207", 538, new string[7] { "", "", "", "", "", "", "" }, null, 5, node: false));
		_dataArray.Add(new MonthlyEventItem(322, 539, EMonthlyEventType.SpecialEvent, "28d549b7-455b-4c36-af78-0503c0ffb10a", "sp_monthlyevent_208", 540, new string[7] { "Character", "", "", "", "", "", "" }, null, 5, node: false));
		_dataArray.Add(new MonthlyEventItem(323, 541, EMonthlyEventType.SpecialEvent, "bf5b18e1-1f00-49be-80e2-643bd140244b", "sp_monthlyevent_208", 542, new string[7] { "Character", "", "", "", "", "", "" }, null, 5, node: false));
		_dataArray.Add(new MonthlyEventItem(324, 543, EMonthlyEventType.SpecialEvent, "846051d7-ad0b-4bcb-a30b-6cebb0c1dd3d", "sp_monthlyevent_209", 544, new string[7] { "Character", "", "", "", "", "", "" }, null, 3, node: false));
		_dataArray.Add(new MonthlyEventItem(325, 545, EMonthlyEventType.SpecialEvent, "913fed24-2151-4a4a-98ad-9f2521346617", "sp_monthlyevent_109", 546, new string[7] { "", "", "", "", "", "", "" }, null, 1, node: false));
		_dataArray.Add(new MonthlyEventItem(326, 547, EMonthlyEventType.SpecialEvent, "5c2dc280-8ef4-4db0-a257-c51aacd26988", "sp_monthlyevent_110", 548, new string[7] { "", "", "", "", "", "", "" }, null, 1, node: false));
		_dataArray.Add(new MonthlyEventItem(327, 549, EMonthlyEventType.SpecialEvent, "9bef300e-ef1e-469f-bbc6-51150d229421", "sp_monthlyevent_222", 550, new string[7] { "Location", "", "", "", "", "", "" }, null, 3, node: false));
		_dataArray.Add(new MonthlyEventItem(328, 551, EMonthlyEventType.SpecialEvent, "1e31b702-bf2f-4b0a-98b3-aee867a030a5", "sp_monthlyevent_211", 552, new string[7] { "", "", "", "", "", "", "" }, null, 0, node: false));
		_dataArray.Add(new MonthlyEventItem(329, 553, EMonthlyEventType.SpecialEvent, "0a9a6fc4-c89c-4fd3-b9c9-617bec8d5fcd", "sp_monthlyevent_224", 554, new string[7] { "Location", "", "", "", "", "", "" }, null, 0, node: false));
		_dataArray.Add(new MonthlyEventItem(330, 555, EMonthlyEventType.SpecialEvent, "8404e31c-de4d-4e12-9348-58b3ac3281a9", "sp_monthlyevent_212", 556, new string[7] { "", "", "", "", "", "", "" }, null, 10, node: false));
		_dataArray.Add(new MonthlyEventItem(331, 557, EMonthlyEventType.SpecialEvent, "0423001e-3c3b-444c-8489-316577f0eede", "sp_monthlyevent_223", 558, new string[7] { "Location", "", "", "", "", "", "" }, null, 3, node: false));
		_dataArray.Add(new MonthlyEventItem(332, 559, EMonthlyEventType.SpecialEvent, "a8d1cbf3-fa48-4531-be5e-7a5b18328b95", "sp_monthlyevent_213", 560, new string[7] { "", "", "", "", "", "", "" }, null, 10, node: false));
		_dataArray.Add(new MonthlyEventItem(333, 561, EMonthlyEventType.SpecialEvent, "6b485099-1127-463c-9c03-15e5ce447c77", "sp_monthlyevent_214", 562, new string[7] { "", "", "", "", "", "", "" }, null, 10, node: false));
		_dataArray.Add(new MonthlyEventItem(334, 563, EMonthlyEventType.SpecialEvent, "1eeb6a45-7b08-444e-aaee-90058e390433", "sp_monthlyevent_215", 564, new string[7] { "", "", "", "", "", "", "" }, null, 10, node: false));
		_dataArray.Add(new MonthlyEventItem(335, 565, EMonthlyEventType.SpecialEvent, "0fc236fd-a4b8-46aa-a79c-548590829b8b", "sp_monthlyevent_216", 566, new string[7] { "Character", "Location", "", "", "", "", "" }, null, 0, node: false));
		_dataArray.Add(new MonthlyEventItem(336, 567, EMonthlyEventType.SpecialEvent, "930e3d91-1359-429e-89fe-5f51d30c64b7", "sp_monthlyevent_217", 568, new string[7] { "", "", "", "", "", "", "" }, null, 10, node: false));
		_dataArray.Add(new MonthlyEventItem(337, 569, EMonthlyEventType.SpecialEvent, "c8c22efb-45d9-43aa-a5d2-0205d7d67d07", "sp_monthlyevent_218", 570, new string[7] { "Location", "", "", "", "", "", "" }, null, 3, node: false));
		_dataArray.Add(new MonthlyEventItem(338, 571, EMonthlyEventType.SpecialEvent, "e263db09-a1bf-4c67-9523-68120b08a2d9", "sp_monthlyevent_219", 572, new string[7] { "", "", "", "", "", "", "" }, null, 3, node: false));
		_dataArray.Add(new MonthlyEventItem(339, 573, EMonthlyEventType.SpecialEvent, "0bfac0cb-d58e-4407-be7c-c9ebe6a2a6f4", "sp_monthlyevent_220", 572, new string[7] { "", "", "", "", "", "", "" }, null, 3, node: false));
		_dataArray.Add(new MonthlyEventItem(340, 574, EMonthlyEventType.NormalEvent, "770e03ff-41d1-4cf2-b708-b51139e8ebb7", "sp_monthlyevent_221", 572, new string[7] { "", "", "", "", "", "", "" }, null, 3, node: false));
		_dataArray.Add(new MonthlyEventItem(341, 575, EMonthlyEventType.SpecialEvent, "b70eadf1-eb09-495c-a79a-87afdf0afe3e", "sp_monthlyevent_227", 576, new string[7] { "", "", "", "", "", "", "" }, null, 3, node: false));
		_dataArray.Add(new MonthlyEventItem(342, 577, EMonthlyEventType.SpecialEvent, "3d9ad75c-b824-41a9-9a6a-4713f7577aed", "sp_monthlyevent_228", 578, new string[7] { "", "", "", "", "", "", "" }, null, 3, node: false));
		_dataArray.Add(new MonthlyEventItem(343, 579, EMonthlyEventType.SpecialEvent, "1ead611e-b08e-459d-8080-5d4b7cbaa6e8", "sp_monthlyevent_229", 580, new string[7] { "", "", "", "", "", "", "" }, null, 3, node: false));
		_dataArray.Add(new MonthlyEventItem(344, 581, EMonthlyEventType.SpecialEvent, "4f3f231f-8ecd-4a92-95c7-4aa20c45521c", "sp_monthlyevent_109", 582, new string[7] { "", "", "", "", "", "", "" }, null, 1, node: false));
		_dataArray.Add(new MonthlyEventItem(345, 583, EMonthlyEventType.SpecialEvent, "57202af8-fe7c-4eb4-a138-597a4885033d", "sp_monthlyevent_110", 584, new string[7] { "", "", "", "", "", "", "" }, null, 1, node: false));
		_dataArray.Add(new MonthlyEventItem(346, 585, EMonthlyEventType.SpecialEvent, "4181db5d-146e-4041-a2b8-a67b51e3c784", "sp_monthlyevent_225", 586, new string[7] { "Character", "Character", "Character", "", "", "", "" }, null, 0, node: false));
		_dataArray.Add(new MonthlyEventItem(347, 587, EMonthlyEventType.SpecialEvent, "045159b3-1dcb-41b1-90cb-e2dc87584fee", "sp_monthlyevent_226", 588, new string[7] { "Character", "Settlement", "", "", "", "", "" }, null, 0, node: false));
		_dataArray.Add(new MonthlyEventItem(348, 589, EMonthlyEventType.SpecialEvent, "8cb7e0f9-b760-4371-a22a-3fcd6511a485", "sp_monthlyevent_230", 590, new string[7] { "Location", "Character", "", "", "", "", "" }, null, 3, node: false));
		_dataArray.Add(new MonthlyEventItem(349, 589, EMonthlyEventType.NormalEvent, "5aa1a3db-2ce4-4b2f-89ce-71af8a9d7344", "sp_monthlyevent_230", 591, new string[7] { "Character", "Location", "Character", "", "", "", "" }, null, 0, node: false));
		_dataArray.Add(new MonthlyEventItem(350, 592, EMonthlyEventType.NormalEvent, "c260de56-3539-4f31-b483-cfd007694f21", "sp_monthlyevent_231", 593, new string[7] { "Character", "Location", "Character", "Character", "", "", "" }, null, 0, node: false));
		_dataArray.Add(new MonthlyEventItem(351, 594, EMonthlyEventType.NormalEvent, "ab430470-0b26-4741-980b-f3e7c6ad9829", "sp_monthlyevent_232", 595, new string[7] { "Character", "", "", "", "", "", "" }, null, 0, node: false));
		_dataArray.Add(new MonthlyEventItem(352, 596, EMonthlyEventType.NormalEvent, "ff47d62e-9266-48cd-892f-84c6cfd194f5", "sp_monthlyevent_234", 597, new string[7] { "", "", "", "", "", "", "" }, null, 0, node: false));
		_dataArray.Add(new MonthlyEventItem(353, 196, EMonthlyEventType.NormalEvent, "293b9c34-011f-4a84-b5d4-d98f6ab37b64", "sp_monthlyevent_77", 598, new string[7] { "Character", "Location", "Character", "Integer", "", "", "" }, null, 0, node: false));
		_dataArray.Add(new MonthlyEventItem(354, 196, EMonthlyEventType.NormalEvent, "128ba978-1862-41d5-94e6-88ed537586d8", "sp_monthlyevent_77", 599, new string[7] { "Character", "Location", "Character", "Integer", "", "", "" }, null, 0, node: false));
		_dataArray.Add(new MonthlyEventItem(355, 600, EMonthlyEventType.SpecialEvent, "eb3b3430-7a82-4c95-b132-b97f03927302", "sp_monthlyevent_235", 601, new string[7] { "OrgGrade", "Item", "", "", "", "", "" }, null, 0, node: false));
		_dataArray.Add(new MonthlyEventItem(356, 585, EMonthlyEventType.SpecialEvent, "d742c92c-470a-496c-8b4a-eccddcc8baee", "sp_monthlyevent_225", 602, new string[7] { "Character", "Character", "Location", "", "", "", "" }, null, 0, node: false));
		_dataArray.Add(new MonthlyEventItem(357, 603, EMonthlyEventType.SpecialEvent, "312200ad-cd01-4017-b72b-5553f53acfa0", "sp_monthlyevent_237", 604, new string[7] { "Character", "Location", "", "", "", "", "" }, null, 2, node: false));
		_dataArray.Add(new MonthlyEventItem(358, 605, EMonthlyEventType.SpecialEvent, "ab6e5243-7190-46d5-b705-2e0b9d5f2928", "sp_monthlyevent_238", 606, new string[7] { "Character", "", "", "", "", "", "" }, null, 2, node: false));
		_dataArray.Add(new MonthlyEventItem(359, 607, EMonthlyEventType.SpecialEvent, "44e9bb3d-ef56-48e1-964d-f88a5d6b1362", "sp_monthlyevent_239", 608, new string[7] { "Character", "Location", "", "", "", "", "" }, null, 2, node: false));
	}

	private void CreateItems6()
	{
		_dataArray.Add(new MonthlyEventItem(360, 609, EMonthlyEventType.SpecialEvent, "a7ba8c41-b982-4a94-b38e-f5b5591121ad", "sp_monthlyevent_240", 610, new string[7] { "Character", "Location", "", "", "", "", "" }, null, 2, node: false));
		_dataArray.Add(new MonthlyEventItem(361, 611, EMonthlyEventType.SpecialEvent, "ea10b5f6-2949-4f98-b357-29f2138c0e02", "sp_monthlyevent_109", 612, new string[7] { "", "", "", "", "", "", "" }, null, 1, node: false));
		_dataArray.Add(new MonthlyEventItem(362, 613, EMonthlyEventType.SpecialEvent, "0961ceb0-22a9-4f8d-8fb0-473a5428e807", "sp_monthlyevent_110", 614, new string[7] { "", "", "", "", "", "", "" }, null, 1, node: false));
		_dataArray.Add(new MonthlyEventItem(363, 615, EMonthlyEventType.SpecialEvent, "90d1b5bf-647e-4bf9-95b2-1eb27c839383", "sp_monthlyevent_236", 616, new string[7] { "Character", "Location", "Character", "", "", "", "" }, null, 0, node: false));
		_dataArray.Add(new MonthlyEventItem(364, 617, EMonthlyEventType.SpecialEvent, "cf762b69-2903-443a-a81c-35b6795a48c1", "sp_monthlyevent_241", 618, new string[7] { "Character", "Character", "", "", "", "", "" }, null, 0, node: false));
		_dataArray.Add(new MonthlyEventItem(365, 619, EMonthlyEventType.SpecialEvent, "0a5307c0-cf82-49d2-939f-decb93bc8e80", "sp_monthlyevent_242", 620, new string[7] { "Character", "Integer", "", "", "", "", "" }, null, 0, node: false));
		_dataArray.Add(new MonthlyEventItem(366, 621, EMonthlyEventType.SpecialEvent, "bee8d7e8-96a4-425f-a7ae-8849fbd27d73", "sp_monthlyevent_243", 622, new string[7] { "Character", "", "", "", "", "", "" }, null, 0, node: false));
		_dataArray.Add(new MonthlyEventItem(367, 20, EMonthlyEventType.NormalEvent, "82c1d4f9-e47c-4c09-8ed8-2fb3c72e30a4", "sp_monthlyevent_6", 21, new string[7] { "Character", "Location", "Character", "", "", "", "" }, null, 0, node: false));
		_dataArray.Add(new MonthlyEventItem(368, 623, EMonthlyEventType.SpecialEvent, "07fcefdf-5ccd-41fb-bac6-7544cc5602e4", "sp_monthlyevent_135", 624, new string[7] { "Character", "", "", "", "", "", "" }, null, 1, node: false));
		_dataArray.Add(new MonthlyEventItem(369, 625, EMonthlyEventType.NormalEvent, "11b71084-0d5e-4f1b-aec0-e2e2a73ecc78", "sp_monthlyevent_244", 626, new string[7] { "Character", "Settlement", "Settlement", "Integer", "", "", "" }, null, 0, node: false));
		_dataArray.Add(new MonthlyEventItem(370, 627, EMonthlyEventType.NormalEvent, "35dbcaf7-a830-419e-9fea-2b2cf88b8bfb", "sp_monthlyevent_14", 628, new string[7] { "Character", "Character", "Integer", "", "", "", "" }, null, 0, node: false));
		_dataArray.Add(new MonthlyEventItem(371, 629, EMonthlyEventType.SpecialEvent, "ee2708fa-645a-4d07-bd64-eef4b79938bc", null, 630, new string[7] { "", "", "", "", "", "", "" }, null, 1, node: false));
		_dataArray.Add(new MonthlyEventItem(372, 631, EMonthlyEventType.SpecialEvent, "a96cca24-7fd8-4d0e-9670-bf20a0bb6137", null, 630, new string[7] { "", "", "", "", "", "", "" }, null, 1, node: false));
		_dataArray.Add(new MonthlyEventItem(373, 632, EMonthlyEventType.SpecialEvent, "d1a19482-a1c3-4087-874a-b40b67051951", null, 630, new string[7] { "", "", "", "", "", "", "" }, null, 1, node: false));
		_dataArray.Add(new MonthlyEventItem(374, 633, EMonthlyEventType.SpecialEvent, "7664cffa-b65a-4162-9c8d-ac175c77a949", null, 630, new string[7] { "", "", "", "", "", "", "" }, null, 1, node: false));
		_dataArray.Add(new MonthlyEventItem(375, 634, EMonthlyEventType.SpecialEvent, "a62e2e34-7594-4de2-9a05-157e7af96b6d", null, 630, new string[7] { "", "", "", "", "", "", "" }, null, 1, node: false));
		_dataArray.Add(new MonthlyEventItem(376, 635, EMonthlyEventType.SpecialEvent, "92db3904-0439-495a-be53-e4a2c28e3c4c", null, 630, new string[7] { "", "", "", "", "", "", "" }, null, 1, node: false));
		_dataArray.Add(new MonthlyEventItem(377, 391, EMonthlyEventType.SpecialEvent, "8da76179-8ebf-4c9f-9a1d-9bb91013971c", null, 630, new string[7] { "", "", "", "", "", "", "" }, null, 1, node: false));
		_dataArray.Add(new MonthlyEventItem(378, 393, EMonthlyEventType.SpecialEvent, "2c7c4461-7cf4-414b-aa81-8bd7d972124a", null, 630, new string[7] { "", "", "", "", "", "", "" }, null, 1, node: false));
		_dataArray.Add(new MonthlyEventItem(379, 350, EMonthlyEventType.SpecialEvent, "b854baa1-4de8-4f4d-8916-be69d94935ba", "sp_monthlyevent_245", 636, new string[7] { "Character", "", "", "", "", "", "" }, null, 1, node: false));
		_dataArray.Add(new MonthlyEventItem(380, 352, EMonthlyEventType.SpecialEvent, "51b5d1fd-1ab8-4470-9e39-abf5e16a99aa", "sp_monthlyevent_246", 637, new string[7] { "", "", "", "", "", "", "" }, null, 1, node: false));
		_dataArray.Add(new MonthlyEventItem(381, 452, EMonthlyEventType.NormalEvent, "45870f83-4919-4241-b15b-a3f6e644ec47", "sp_monthlyevent_247", 453, new string[7] { "Location", "", "", "", "", "", "" }, null, 0, node: false));
		_dataArray.Add(new MonthlyEventItem(382, 458, EMonthlyEventType.SpecialEvent, "0f499b42-2aba-4987-a60b-a00d5e75b84e", "sp_monthlyevent_248", 459, new string[7] { "Character", "Location", "", "", "", "", "" }, null, 5, node: false));
		_dataArray.Add(new MonthlyEventItem(383, 638, EMonthlyEventType.SpecialEvent, "7717d608-aff7-4f66-8a89-1442351471bb", "sp_monthlyevent_249", 639, new string[7] { "", "", "", "", "", "", "" }, null, 1, node: false));
		_dataArray.Add(new MonthlyEventItem(384, 640, EMonthlyEventType.NormalEvent, "638c8756-5f2d-4cae-ac99-c2d8638d719b", "sp_monthlyevent_276", 641, new string[7] { "", "", "", "", "", "", "" }, null, 0, node: false));
	}

	public override void Init()
	{
		base.Init();
		_dataArray = new List<MonthlyEventItem>(385);
		CreateItems0();
		CreateItems1();
		CreateItems2();
		CreateItems3();
		CreateItems4();
		CreateItems5();
		CreateItems6();
	}
}
