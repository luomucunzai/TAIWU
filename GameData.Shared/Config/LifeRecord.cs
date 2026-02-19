using System;
using System.Collections.Generic;
using Config.Common;

namespace Config;

[Serializable]
public class LifeRecord : ConfigData<LifeRecordItem, short>
{
	public static class DefKey
	{
		public const short Die = 0;

		public const short XiangshuPartiallyInfected = 1;

		public const short XiangshuCompletelyInfected = 2;

		public const short MotherLoseFetus = 3;

		public const short FatherLoseFetus = 4;

		public const short AbandonChild = 5;

		public const short ChildGetAbandoned = 6;

		public const short GiveBirthToCricket = 7;

		public const short GiveBirthToBoy = 8;

		public const short GiveBirthToGirl = 9;

		public const short BecomeFatherToNewBornBoy = 10;

		public const short BecomeFatherToNewBornGirl = 11;

		public const short BuildGrave = 12;

		public const short MonkBreakRule = 13;

		public const short KidnappedCharacterEscaped = 14;

		public const short EscapeFromKidnapping = 15;

		public const short ReadBookSucceed = 16;

		public const short ReadBookFail = 17;

		public const short BreakoutSucceed = 18;

		public const short BreakoutFail = 19;

		public const short LearnCombatSkill = 20;

		public const short LearnLifeSkill = 21;

		public const short RepairItem = 22;

		public const short AddPoisonToItem = 23;

		public const short LoseOverloadingResource = 24;

		public const short LoseOverloadingItem = 25;

		public const short MakeEnemy = 26;

		public const short SeverEnemy = 27;

		public const short BeMadeEnemy = 28;

		public const short SeveredEnemy = 29;

		public const short Adore = 30;

		public const short LoveAtFirstSight = 31;

		public const short ConfessLoveSucceed = 32;

		public const short ConfessLoveFail = 33;

		public const short AcceptConfessLove = 34;

		public const short RefuseConfessLove = 35;

		public const short BreakupMutually = 36;

		public const short DumpLover = 37;

		public const short GetDumppedByLover = 38;

		public const short ProposeMarriageSucceed = 39;

		public const short ProposeMarriageFail = 40;

		public const short RefuseMarriageProposal = 41;

		public const short BecomeFriend = 42;

		public const short SeverFriendship = 43;

		public const short BecomeSwornBrotherOrSister = 44;

		public const short SeverSwornBrotherhood = 45;

		public const short GetAdoptedByFather = 46;

		public const short GetAdoptedByMother = 47;

		public const short AdoptSon = 48;

		public const short AdoptDaughter = 49;

		public const short CreateFaction = 50;

		public const short JoinFaction = 51;

		public const short LeaveFaction = 52;

		public const short FactionRecruitSucceed = 53;

		public const short FactionRecruitFail = 54;

		public const short AgreeToJoinFaction = 55;

		public const short RefuseToJoinFaction = 56;

		public const short DecideToJoinSect = 57;

		public const short DecideToFullfillAppointment = 58;

		public const short DecideToProtect = 59;

		public const short DecideToRescue = 60;

		public const short DecideToMourn = 61;

		public const short DecideToVisit = 62;

		public const short DecideToFindLostItem = 63;

		public const short DecideToFindSpecialMaterial = 64;

		public const short DecideToRevenge = 65;

		public const short DecideToParticipateAdventure = 66;

		public const short JoinSectFail = 67;

		public const short JoinSectSucceed = 68;

		public const short CanNoLongerFullFillAppointment = 69;

		public const short WaitForAppointment = 70;

		public const short FullFillAppointment = 71;

		public const short FinishProtection = 72;

		public const short OfferProtection = 73;

		public const short FinishRescue = 74;

		public const short FinishMourning = 75;

		public const short MaintainGrave = 76;

		public const short UpgradeGrave = 77;

		public const short FinishVisit = 78;

		public const short FinishFIndingLostItem = 79;

		public const short FinishFIndingSpecialMaterial = 80;

		public const short FindLostItemSucceed = 81;

		public const short FindLostItemFail = 82;

		public const short FindSpecialMaterialSucceed = 83;

		public const short FinishTakingRevenge = 84;

		public const short MajorVictoryInCombat = 85;

		public const short MajorFailureInCombat = 86;

		public const short VictoryInCombat = 87;

		public const short FailureInCombat = 88;

		public const short EnemyEscape = 89;

		public const short LoseAndEscape = 90;

		public const short KillInPublic = 91;

		public const short KillInPrivate = 92;

		public const short KidnapInPublic = 93;

		public const short KidnapInPrivate = 94;

		public const short ReleaseLoser = 95;

		public const short GetKidnappedInPublic = 96;

		public const short GetKidnappedInPrivate = 97;

		public const short GetReleasedByWinner = 98;

		public const short AgreeToProtect = 99;

		public const short RefuseToProtect = 100;

		public const short FinishAdventure = 101;

		public const short RequestHealOuterInjurySucceed = 102;

		public const short RequestHealInnerInjurySucceed = 103;

		public const short RequestDetoxPoisonSucceed = 104;

		public const short RequestHealthSucceed = 105;

		public const short RequestHealDisorderOfQiSucceed = 106;

		public const short RequestNeiliSucceed = 107;

		public const short RequestKillWugSucceed = 108;

		public const short RequestFoodSucceed = 109;

		public const short RequestTeaWineSucceed = 110;

		public const short RequestResourceSucceed = 111;

		public const short RequestItemSucceed = 112;

		public const short RequestRepairItemSucceed = 113;

		public const short RequestAddPoisonToItemSucceed = 114;

		public const short RequestInstructionOnLifeSkillSucceed = 115;

		public const short RequestInstructionOnCombatSkillSucceed = 116;

		public const short RequestInstructionOnLifeSkillFailToLearn = 117;

		public const short RequestInstructionOnCombatSkillFailToLearn = 118;

		public const short RequestInstructionOnReadingSucceed = 119;

		public const short RequestInstructionOnBreakoutSucceed = 120;

		public const short RequestHealOuterInjuryFail = 121;

		public const short RequestHealInnerInjuryFail = 122;

		public const short RequestDetoxPoisonFail = 123;

		public const short RequestHealthFail = 124;

		public const short RequestHealDisorderOfQiFail = 125;

		public const short RequestNeiliFail = 126;

		public const short RequestKillWugFail = 127;

		public const short RequestFoodFail = 128;

		public const short RequestTeaWineFail = 129;

		public const short RequestResourceFail = 130;

		public const short RequestItemFail = 131;

		public const short RequestRepairItemFail = 132;

		public const short RequestAddPoisonToItemFail = 133;

		public const short RequestInstructionOnLifeSkillFail = 134;

		public const short RequestInstructionOnCombatSkillFail = 135;

		public const short RequestInstructionOnReadingFail = 136;

		public const short RequestInstructionOnBreakoutFail = 137;

		public const short AcceptRequestHealOuterInjury = 138;

		public const short AcceptRequestHealInnerInjury = 139;

		public const short AcceptRequestDetoxPoison = 140;

		public const short AcceptRequestHealth = 141;

		public const short AcceptRequestHealDisorderOfQi = 142;

		public const short AcceptRequestNeili = 143;

		public const short AcceptRequestKillWug = 144;

		public const short AcceptRequestFood = 145;

		public const short AcceptRequestTeaWine = 146;

		public const short AcceptRequestResource = 147;

		public const short AcceptRequestItem = 148;

		public const short AcceptRequestRepairItem = 149;

		public const short AcceptRequestAddPoisonToItem = 150;

		public const short AcceptRequestInstructionOnLifeSkill = 151;

		public const short AcceptRequestInstructionOnCombatSkill = 152;

		public const short AcceptRequestInstructionOnLifeSkillButFail = 153;

		public const short AcceptRequestInstructionOnCombatSkillButFail = 154;

		public const short AcceptRequestInstructionOnReading = 155;

		public const short AcceptRequestInstructionOnBreakout = 156;

		public const short RefuseRequestHealOuterInjury = 157;

		public const short RefuseRequestHealInnerInjury = 158;

		public const short RefuseRequestDetoxPoison = 159;

		public const short RefuseRequestHealth = 160;

		public const short RefuseRequestHealDisorderOfQi = 161;

		public const short RefuseRequestNeili = 162;

		public const short RefuseRequestKillWug = 163;

		public const short RefuseRequestFood = 164;

		public const short RefuseRequestTeaWine = 165;

		public const short RefuseRequestResource = 166;

		public const short RefuseRequestItem = 167;

		public const short RefuseRequestRepairItem = 168;

		public const short RefuseRequestAddPoisonToItem = 169;

		public const short RefuseRequestInstructionOnLifeSkill = 170;

		public const short RefuseRequestInstructionOnCombatSkill = 171;

		public const short RefuseRequestInstructionOnReading = 172;

		public const short RefuseRequestInstructionOnBreakout = 173;

		public const short RescueKidnappedCharacterSecretlyFail1 = 174;

		public const short RescueKidnappedCharacterSecretlyFail2 = 175;

		public const short RescueKidnappedCharacterSecretlyFail3 = 176;

		public const short RescueKidnappedCharacterSecretlyFail4 = 177;

		public const short RescueKidnappedCharacterSecretlySucceed = 178;

		public const short RescueKidnappedCharacterSecretlySucceedAndEscaped = 179;

		public const short KidnappedCharacterGetRescuedSecretly = 180;

		public const short RescueKidnappedCharacterWithWitFail1 = 181;

		public const short RescueKidnappedCharacterWithWitFail2 = 182;

		public const short RescueKidnappedCharacterWithWitFail3 = 183;

		public const short RescueKidnappedCharacterWithWitFail4 = 184;

		public const short RescueKidnappedCharacterWithWitSucceed = 185;

		public const short RescueKidnappedCharacterWithWitSucceedAndEscaped = 186;

		public const short KidnappedCharacterGetRescuedWithWit = 187;

		public const short RescueKidnappedCharacterWithForceFail1 = 188;

		public const short RescueKidnappedCharacterWithForceFail2 = 189;

		public const short RescueKidnappedCharacterWithForceFail3 = 190;

		public const short RescueKidnappedCharacterWithForceFail4 = 191;

		public const short RescueKidnappedCharacterWithForceSucceed = 192;

		public const short RescueKidnappedCharacterWithForceSucceedAndEscaped = 193;

		public const short KidnappedCharacterGetRescuedWithForce = 194;

		public const short PoisonEnemyFail1 = 195;

		public const short PoisonEnemyFail2 = 196;

		public const short PoisonEnemyFail3 = 197;

		public const short PoisonEnemyFail4 = 198;

		public const short PoisonEnemySucceed = 199;

		public const short PoisonEnemySucceedAndEscaped = 200;

		public const short GetPoisonedByEnemySucceed = 201;

		public const short PlotHarmEnemyFail1 = 202;

		public const short PlotHarmEnemyFail2 = 203;

		public const short PlotHarmEnemyFail3 = 204;

		public const short PlotHarmEnemyFail4 = 205;

		public const short PlotHarmEnemySucceed = 206;

		public const short PlotHarmEnemySucceedAndEscaped = 207;

		public const short GetPlottedAgainstSucceed = 208;

		public const short StealResourceFail1 = 209;

		public const short StealResourceFail2 = 210;

		public const short StealResourceFail3 = 211;

		public const short StealResourceFail4 = 212;

		public const short StealResourceSucceed = 213;

		public const short StealResourceSucceedAndEscaped = 214;

		public const short StealResourceFailAndBeatenUp = 215;

		public const short ResourceGetStolenSucceed = 216;

		public const short BeatUpResourceStealer = 217;

		public const short ScamResourceFail1 = 218;

		public const short ScamResourceFail2 = 219;

		public const short ScamResourceFail3 = 220;

		public const short ScamResourceFail4 = 221;

		public const short ScamResourceSucceed = 222;

		public const short ScamResourceSucceedAndEscaped = 223;

		public const short ScamResourceFailAndBeatenUp = 224;

		public const short ResourceGetScammedSucceed = 225;

		public const short BeatUpResourceScammer = 226;

		public const short RobResourceFail1 = 227;

		public const short RobResourceFail2 = 228;

		public const short RobResourceFail3 = 229;

		public const short RobResourceFail4 = 230;

		public const short RobResourceSucceed = 231;

		public const short RobResourceSucceedAndEscaped = 232;

		public const short RobResourceFailAndBeatenUp = 233;

		public const short ResourceGetRobbedSucceed = 234;

		public const short BeatUpResourceRobber = 235;

		public const short StealItemFail1 = 236;

		public const short StealItemFail2 = 237;

		public const short StealItemFail3 = 238;

		public const short StealItemFail4 = 239;

		public const short StealItemSucceed = 240;

		public const short StealItemSucceedAndEscaped = 241;

		public const short StealItemSucceedAndBeatenUp = 242;

		public const short ItemGetStolenSucceed = 243;

		public const short BeatUpItemStealer = 244;

		public const short ScamItemFail1 = 245;

		public const short ScamItemFail2 = 246;

		public const short ScamItemFail3 = 247;

		public const short ScamItemFail4 = 248;

		public const short ScamItemSucceed = 249;

		public const short ScamItemSucceedAndEscaped = 250;

		public const short ScamItemFailAndBeatenUp = 251;

		public const short ItemGetScammedSucceed = 252;

		public const short BeatUpItemScammer = 253;

		public const short RobItemFail1 = 254;

		public const short RobItemFail2 = 255;

		public const short RobItemFail3 = 256;

		public const short RobItemFail4 = 257;

		public const short RobItemSucceed = 258;

		public const short RobItemSucceedAndEscaped = 259;

		public const short RobItemFailAndBeatenUp = 260;

		public const short ItemGetRobbedSucceed = 261;

		public const short BeatUpItemRobber = 262;

		public const short RobResourceFromGraveSucceed = 263;

		public const short RobResourceFromGraveFail = 264;

		public const short RobItemFromGraveSucceed = 265;

		public const short RobItemFromGraveFail = 266;

		public const short StealLifeSkillFail1 = 267;

		public const short StealLifeSkillFail2 = 268;

		public const short StealLifeSkillFail3 = 269;

		public const short StealLifeSkillFail4 = 270;

		public const short StealLifeSkillSucceed = 271;

		public const short StealLifeSkillSucceedAndEscaped = 272;

		public const short LifeSkillGetStolenSucceed = 273;

		public const short ScamLifeSkillFail1 = 274;

		public const short ScamLifeSkillFail2 = 275;

		public const short ScamLifeSkillFail3 = 276;

		public const short ScamLifeSkillFail4 = 277;

		public const short ScamLifeSkillSucceed = 278;

		public const short ScamLifeSkillSucceedAndEscaped = 279;

		public const short LifeSkillGetScammedSucceed = 280;

		public const short StealCombatSkillFail1 = 281;

		public const short StealCombatSkillFail2 = 282;

		public const short StealCombatSkillFail3 = 283;

		public const short StealCombatSkillFail4 = 284;

		public const short StealCombatSkillSucceed = 285;

		public const short StealCombatSkillSucceedAndEscaped = 286;

		public const short CombatSkillGetStolenSucceed = 287;

		public const short ScamCombatSkillFail1 = 288;

		public const short ScamCombatSkillFail2 = 289;

		public const short ScamCombatSkillFail3 = 290;

		public const short ScamCombatSkillFail4 = 291;

		public const short ScamCombatSkillSucceed = 292;

		public const short ScamCombatSkillSucceedAndEscaped = 293;

		public const short CombatSkillGetScammedSucceed = 294;

		public const short LifeSkillBattleWin = 295;

		public const short LifeSkillBattleLose = 296;

		public const short ExchangeResource = 297;

		public const short GiveResource = 298;

		public const short PurchaseItem = 299;

		public const short SellItem = 300;

		public const short GiveItem = 301;

		public const short GivePoisonousItem = 302;

		public const short GetResourceAsGift = 303;

		public const short GetItemAsGift = 304;

		public const short RefusePoisonousGift = 305;

		public const short InstructLifeSkill = 306;

		public const short InstructCombatSkill = 307;

		public const short LearnLifeSkillWithInstructionSucceed = 308;

		public const short LearnLifeSkillWithInstructionFail = 309;

		public const short LearnCombatSkillWithInstructionSucceed = 310;

		public const short LearnCombatSkillWithInstructionFail = 311;

		public const short InviteToDrinkSucceed = 312;

		public const short InviteToDrinkFail = 313;

		public const short SellSucceed = 314;

		public const short SellFail = 315;

		public const short CureSucceed = 316;

		public const short RepairItemSucceed = 317;

		public const short BarbSucceed = 318;

		public const short BarbMistake = 319;

		public const short BarbFail = 320;

		public const short AskForMoneySucceed = 321;

		public const short AskForMoneyFail = 322;

		public const short EntertainWithMusic = 323;

		public const short EntertainWithChess = 324;

		public const short EntertainWithPoem = 325;

		public const short EntertainWithPainting = 326;

		public const short AcceptInviteToDrink = 327;

		public const short RefuseInviteToDrink = 328;

		public const short AcceptSell = 329;

		public const short RefuseSell = 330;

		public const short AcceptCure = 331;

		public const short AcceptRepairItem = 332;

		public const short GetBarbSucceed = 333;

		public const short GetBarbMistake = 334;

		public const short GetBarbFail = 335;

		public const short AcceptAskForMoney = 336;

		public const short RefuseAskForMoney = 337;

		public const short AcceptEntertainWithMusic = 338;

		public const short AcceptEntertainWithChess = 339;

		public const short AcceptEntertainWithPoem = 340;

		public const short AcceptEntertainWithPainting = 341;

		public const short MakeItem = 342;

		public const short TaoismAwakeningSucceed = 343;

		public const short TaoismAwakeningFail = 344;

		public const short BuddismAwakeningSucceed = 345;

		public const short BuddismAwakeningFail = 346;

		public const short TaoismGetAwakenedSucceed = 347;

		public const short TaoismGetAwakenedFail = 348;

		public const short BuddismGetAwakenedSucceed = 349;

		public const short BuddismGetAwakenedFail = 350;

		public const short CollectTeaWineSucceed = 351;

		public const short CollectTeaWineFail = 352;

		public const short DivinationSucceed = 353;

		public const short DivinationFail = 354;

		public const short CricketBattleWin = 355;

		public const short CricketBattleLose = 356;

		public const short MakeLoveIllegal = 357;

		public const short RapeFail = 358;

		public const short RapeSucceed = 359;

		public const short ReleaseKidnappedCharacter = 360;

		public const short GetRapedFail = 361;

		public const short GetRapedSucceed = 362;

		public const short GetReleasedByKidnapper = 363;

		public const short MerchantGetNewProduct = 364;

		public const short UnexpectedResourceGain = 365;

		public const short UnexpectedItemGain = 366;

		public const short UnexpectedSkillBookGain = 367;

		public const short UnexpectedHealthCure = 368;

		public const short UnexpectedOuterInjuryCure = 369;

		public const short UnexpectedInnerInjuryCure = 370;

		public const short UnexpectedPoisonCure = 371;

		public const short UnexpectedDisorderOfQiCure = 372;

		public const short UnexpectedResourceLose = 373;

		public const short UnexpectedItemLose = 374;

		public const short UnexpectedSkillBookLose = 375;

		public const short UnexpectedHealthHarm = 376;

		public const short UnexpectedOuterInjuryHarm = 377;

		public const short UnexpectedInnerInjuryHarm = 378;

		public const short UnexpectedPoisonHarm = 379;

		public const short UnexpectedDisorderOfQiHarm = 380;

		public const short KillHereticRandomEnemy = 381;

		public const short KillRighteousRandomEnemy = 382;

		public const short DefeatedByHereticRandomEnemy = 383;

		public const short DefeatedByRighteousRandomEnemy = 384;

		public const short MonvBad = 385;

		public const short DayueYaochangBad = 386;

		public const short JinHuangerBad = 387;

		public const short YiyihouBad = 388;

		public const short WeiQiBad = 389;

		public const short YixiangBad = 390;

		public const short ShufangBad = 391;

		public const short JixiBad = 392;

		public const short MonvGood = 393;

		public const short DayueYaochangGood = 394;

		public const short JinHuangerGood = 395;

		public const short YiyihouGood = 396;

		public const short WeiQiGood = 397;

		public const short YixiangGood = 398;

		public const short XuefengGood = 399;

		public const short ShufangGood = 400;

		public const short PregnantWithSamsara0 = 401;

		public const short PregnantWithSamsara1 = 402;

		public const short PregnantWithSamsara2 = 403;

		public const short PregnantWithSamsara3 = 404;

		public const short PregnantWithSamsara4 = 405;

		public const short PregnantWithSamsara5 = 406;

		public const short GainAuthority = 407;

		public const short SectPunishNormal = 408;

		public const short SectPunishElope = 409;

		public const short ExpelVillager = 410;

		public const short SavedFromInfection = 411;

		public const short ChangeGrade = 412;

		public const short ExpelledByTaiwu = 413;

		public const short InsteadSectPunishElope = 414;

		public const short AvoidSectPunishElope = 415;

		public const short JoinJoustForSpouse = 416;

		public const short GetHusbandByJoustForSpouse = 417;

		public const short GetWifeByJoustForSpouse = 418;

		public const short NoHusbandByJoustForSpouse = 419;

		public const short SectCompetitionBeWinner = 420;

		public const short SectCompetitionBeParticipant = 421;

		public const short SectCompetitionBeHost = 422;

		public const short WulinConferenceBeParticipant = 423;

		public const short WulinConferenceBeWinner = 424;

		public const short WulinConferenceBeWinnerButTaiwu = 425;

		public const short WulinConferenceBeHost = 426;

		public const short WulinConferenceBeKilledByYufu = 427;

		public const short WulinConferenceDonation = 428;

		public const short BeAttackedAndDieByWuYingLing = 429;

		public const short NaturalDisasterGiveDeath = 430;

		public const short NaturalDisasterHappen = 431;

		public const short NaturalDisasterButSurvive = 432;

		public const short NormalInformationChangeLovingItemSubType = 433;

		public const short NormalInformationChangeHatingItemSubType = 434;

		public const short NormalInformationChangeIdealSect = 435;

		public const short NormalInformationChangeBaseMorality = 436;

		public const short NormalInformationChangeLifeSkillTypeInterest = 437;

		public const short RobGraveEncounterSkeleton = 438;

		public const short RobGraveFailed = 439;

		public const short SectPunishLevelLowest = 440;

		public const short PrincipalSectPunishLevelMiddle = 441;

		public const short PrincipalSectPunishLevelHighest = 442;

		public const short NonPrincipalSectPunishLevelLowest = 443;

		public const short NonPrincipalSectPunishLevelHighest = 444;

		public const short BecomeSwornSiblingByThreatened = 445;

		public const short MarriedByThreatened = 446;

		public const short GetAdoptedFatherByThreatened = 447;

		public const short GetAdoptedMotherByThreatened = 448;

		public const short GetAdoptedSonByThreatened = 449;

		public const short GetAdoptedDaughterByThreatened = 450;

		public const short AddMentorByThreatened = 451;

		public const short SeverSwornSiblingByThreatened = 452;

		public const short DivorceByThreatened = 453;

		public const short SeverMentorByThreatened = 454;

		public const short SeverAdoptiveFatherByThreatened = 455;

		public const short SeverAdoptiveMotherByThreatened = 456;

		public const short SeverAdoptiveSonByThreatened = 457;

		public const short SeverAdoptiveDaughterByThreatened = 458;

		public const short GetThreatenedAdoptiveFather = 459;

		public const short GetThreatenedAdoptiveMother = 460;

		public const short GetThreatenedAdoptiveSon = 461;

		public const short GetThreatenedAdoptiveDaughter = 462;

		public const short ApproveTaiwuByThreatened = 463;

		public const short FourSeasonsAdventureBeParticipant = 464;

		public const short FourSeasonsAdventureBeWinner = 465;

		public const short EndAdored = 466;

		public const short GetMentor = 467;

		public const short GetMentee = 468;

		public const short SeverAdoptiveParent = 469;

		public const short SeverAdoptiveChild = 470;

		public const short SeverMentor = 471;

		public const short SeverMentee = 472;

		public const short Divorce = 473;

		public const short ThreatenSucceed = 474;

		public const short AdmonishSucceed = 475;

		public const short ChangeBehaviorTypeByAdmonishedGood = 476;

		public const short ReduceDebtByAdmonished = 477;

		public const short ReduceDebtByThreatened = 478;

		public const short ChangeBehaviorTypeByAdmonishedBad = 479;

		public const short GainLegendaryBook = 480;

		public const short BoostedByLegendaryBooks = 481;

		public const short ActCrazy = 482;

		public const short LegendaryBookShocked = 483;

		public const short LegendaryBookInsane = 484;

		public const short LegendaryBookConsumed = 485;

		public const short DecideToContestForLegendaryBook = 486;

		public const short FinishContestForLegendaryBook = 487;

		public const short LegendaryBookChallengeWin = 488;

		public const short LegendaryBookChallengeLose = 489;

		public const short AcceptLegendaryBookChallengeWin = 490;

		public const short AcceptLegendaryBookChallengeLose = 491;

		public const short AcceptLegendaryBookChallengeEscape = 492;

		public const short LegendaryBookChallengeEscaped = 493;

		public const short LegendaryBookChallengeSelfEscaped = 524;

		public const short AcceptLegendaryBookChallengeEnemyEscaped = 525;

		public const short RefuseRequestLegendaryBookChallenge = 494;

		public const short RequestLegendaryBookChallengeFail = 495;

		public const short AcceptRequestLegendaryBook = 496;

		public const short RequestLegendaryBookSucceed = 497;

		public const short RequestLegendaryBookFail = 498;

		public const short RefuseRequestLegendaryBook = 499;

		public const short AcceptRequestExchangeLegendaryBook = 500;

		public const short RequestExchangeLegendaryBookSucceed = 501;

		public const short RefuseRequestExchangeLegendaryBook = 502;

		public const short RequestExchangeLegendaryBookFail = 503;

		public const short GiveLegendaryBookFail = 504;

		public const short RefuseGiveLegendaryBook = 505;

		public const short DefeatLegendaryBookInsaneJust = 506;

		public const short DefeatLegendaryBookInsaneKind = 507;

		public const short DefeatLegendaryBookInsaneEven = 508;

		public const short DefeatLegendaryBookInsaneRebel = 509;

		public const short DefeatLegendaryBookInsaneEgoistic = 510;

		public const short LegendaryBookInsaneDefeatedJust = 511;

		public const short LegendaryBookInsaneDefeatedKind = 512;

		public const short LegendaryBookInsaneDefeatedEven = 513;

		public const short LegendaryBookInsaneDefeatedRebel = 514;

		public const short LegendaryBookInsaneDefeatedEgoistic = 515;

		public const short ShockedInsaneEscaped = 516;

		public const short ReleaseShockedInsane = 517;

		public const short UnderAttackEscaped = 518;

		public const short ReleaseUnderAttack = 519;

		public const short DefeatConsumed = 520;

		public const short BeDefetedByConsumed = 521;

		public const short AcceptRequestExchangeLegendaryBookByExp = 522;

		public const short RequestExchangeLegendaryBookSucceedByExp = 523;

		public const short ResignPositionToStudyLegendaryBook = 526;

		public const short SoundOutLoverMind = 527;

		public const short SoundOutMind = 528;

		public const short RedeemMindSucceed = 529;

		public const short RedeemMindFail = 530;

		public const short AcceptRedeemMind = 531;

		public const short RefuseRedeemMind = 532;

		public const short FirstDateWithLover = 533;

		public const short FirstDateWithTaiwu = 534;

		public const short SelectLoverToken = 535;

		public const short SelectLoverToken2 = 536;

		public const short DateWithLover = 537;

		public const short DateWithLover2 = 538;

		public const short TillDeathDoUsPart = 539;

		public const short CelebrateBirthday = 540;

		public const short CelebrateSelfBirthday = 541;

		public const short CelebrateAnniversary = 542;

		public const short BeCaughtCheating = 543;

		public const short CaughtCheating = 544;

		public const short PregnancyWithWife = 545;

		public const short PregnancyWithHusband = 546;

		public const short TeaTasting = 547;

		public const short TeaTastingLifeSkillBattleWin = 548;

		public const short TeaTastingLifeSkillBattleLose = 549;

		public const short TeaTastingDisorderOfQi = 550;

		public const short WineTasting = 551;

		public const short WineTastingLifeSkillBattleWin = 552;

		public const short WineTastingLifeSkillBattleLose = 553;

		public const short WineTastingDisorderOfQi = 554;

		public const short FirstNameChanged = 555;

		public const short LifeSkillModel = 556;

		public const short CombatSkillModel = 557;

		public const short PromoteReputation = 558;

		public const short ReputationPromoted = 559;

		public const short CapabilityCultivated = 560;

		public const short BroughtToTaiwuByBeggars = 561;

		public const short DiscardRevengeForCivilianSkill = 562;

		public const short CivilianSkillDissolveResentment = 563;

		public const short PersuadeWithdrawlFromOrganization = 564;

		public const short WithdrawlFromOrganization = 565;

		public const short FreeMedicalConsultation = 566;

		public const short OfferTreasures = 567;

		public const short ReceiveOfferedTreasures = 568;

		public const short ForcefulPurchase = 569;

		public const short ForcefulSale = 570;

		public const short BegForMoney = 571;

		public const short AbsurdlyForceToLeave = 572;

		public const short AbsurdlyForcedToLeave = 573;

		public const short DiagnoseWithMedicine = 574;

		public const short DiagnosedWithMedicine = 575;

		public const short DiagnoseWithNonMedicine = 576;

		public const short DiagnosedWithWrongMedicine = 577;

		public const short ExtendLifeSpan = 578;

		public const short LifeSpanExtended = 579;

		public const short PersuadeToBecomeMonk = 580;

		public const short BecomeMonkPersuaded = 581;

		public const short FailToPersuadeToBecomeMonk = 582;

		public const short ExpiateDeadSouls = 583;

		public const short ExociseXiangshuInfectionVictoryInCombat = 584;

		public const short BecomeExociseXiangshuInfectionVictoryInCombat = 585;

		public const short ExociseXiangshuInfectionVictoryInCombatDefeated = 604;

		public const short TribulationSucceeded = 586;

		public const short TribulationFailed = 587;

		public const short TribulationCanceled = 588;

		public const short TribulationContinued = 589;

		public const short GuidingEvilToGoodSucceed = 590;

		public const short GuidingEvilGoodSucceed = 591;

		public const short GuidingEvilToGoodFail = 592;

		public const short VisitBuddhismTemples = 593;

		public const short EpiphanyThruVisitTemples = 594;

		public const short EpiphanyThruVisitTemplesCombatSkill = 595;

		public const short EpiphanyThruVisitTemplesLifeSkill = 596;

		public const short EpiphanyThruVisitTemplesExperience = 605;

		public const short DivineUnexpectedGain = 597;

		public const short DivineUnexpectedHarm = 598;

		public const short ExchangeFates = 599;

		public const short BecomeExchangeFates = 600;

		public const short ImmortalityGained = 601;

		public const short ImmortalityLost = 602;

		public const short ImmortalityRegained = 603;

		public const short TaiwuReincarnation = 606;

		public const short TaiwuReincarnationPregnancy = 607;

		public const short MixPoisonHotRedRotten = 608;

		public const short MixPoisonHotRottenIllusory = 609;

		public const short MixPoisonHotRottenGloomy = 610;

		public const short MixPoisonHotRottenCold = 611;

		public const short MixPoisonRedRottenIllusory = 612;

		public const short MixPoisonRedRottenGloomy = 613;

		public const short MixPoisonRedRottenCold = 614;

		public const short MixPoisonHotRedIllusory = 615;

		public const short MixPoisonHotRedGloomy = 616;

		public const short MixPoisonHotRedCold = 617;

		public const short MixPoisonGloomyColdIllusory = 618;

		public const short MixPoisonRottenGloomyCold = 619;

		public const short MixPoisonHotGloomyCold = 620;

		public const short MixPoisonRedGloomyCold = 621;

		public const short MixPoisonRottenColdIllusory = 622;

		public const short MixPoisonHotColdIllusory = 623;

		public const short MixPoisonRedColdIllusory = 624;

		public const short MixPoisonRottenGloomyIllusory = 625;

		public const short MixPoisonHotGloomyIllusory = 626;

		public const short MixPoisonRedGloomyIllusory = 627;

		public const short DiggingXiangshuMinionCombatLost = 628;

		public const short DiggingXiangshuMinionCombatWon = 629;

		public const short SectMainStoryXuehouJixiKills = 630;

		public const short SectMainStoryWudangTreasure = 631;

		public const short SectMainStoryXuannvJoinOrg = 632;

		public const short SectMainStoryYuanshanGetAbsorbed = 633;

		public const short SectMainStoryYuanshanResistSucceed = 634;

		public const short SectMainStoryYuanshanResistOrdinary = 635;

		public const short SectMainStoryYuanshanResistFailed = 636;

		public const short SectMainStoryXuehouZombieKills = 637;

		public const short SectMainStoryShixiangSkillEnemy = 638;

		public const short SectMainStoryWuxianMethysis0 = 639;

		public const short SectMainStoryWuxianPoison = 640;

		public const short SectMainStoryWuxianAssault = 641;

		public const short SectMainStoryWuxianMethysis1 = 642;

		public const short SectMainStoryEmeiInfighting = 643;

		public const short SectMainStoryJieqingAssassin = 644;

		public const short WulinConferencePraiseAndGifts = 645;

		public const short NormalInformationChangeIdealSectNegative = 646;

		public const short SectMainStoryXuehouJixiRescueTaiwu = 647;

		public const short SectMainStoryRanshanJoinThreeFactionCompetetion = 648;

		public const short SectMainStoryRanshanThreeFactionCompetetionWin = 649;

		public const short SectMainStoryRanshanThreeFactionCompetetionLose = 650;

		public const short GainExpByStroll = 651;

		public const short GainExpByReadingOldBook = 652;

		public const short PunishedAlongsideSpouse = 653;

		public const short DecideToAdoptFoundling = 654;

		public const short AdoptFoundlingFail = 655;

		public const short AdoptFoundlingSucceed = 656;

		public const short FoundlingGetAdopted = 657;

		public const short ClaimFoundlingSucceed = 658;

		public const short FoundlingGetClaimed = 659;

		public const short SectMainStoryWudangVillagerKilled = 660;

		public const short SectMainStoryShixiangFallIll = 661;

		public const short KillAnimal = 662;

		public const short DefeatedByAnimal = 663;

		public const short EnterEnemyNest = 664;

		public const short DieFromEnemyNest = 665;

		public const short EscapeFromEnemyNest = 666;

		public const short GetSecretSpreadInVeryHighProbability = 667;

		public const short GetSecretSpreadInHighProbability = 668;

		public const short GetSecretSpreadInLowProbability = 669;

		public const short GetSecretSpreadInVeryLowProbability = 670;

		public const short SpreadSecretFail = 671;

		public const short SpreadSecretSuccess = 672;

		public const short HeardSecretSpreadInVeryHighProbability = 673;

		public const short HeardSecretSpreadInHighProbability = 674;

		public const short HeardSecretSpreadInLowProbability = 675;

		public const short HeardSecretSpreadInVeryLowProbability = 676;

		public const short RequestKeepSecretFail = 677;

		public const short RequestKeepSecretSuccess = 678;

		public const short BeRequestedToKeepSecret = 679;

		public const short ThreadNeedleMatchFail = 680;

		public const short ThreadNeedleSeparateFail = 681;

		public const short ThreadNeedleMatchSuccess = 682;

		public const short ThreadNeedleSeparateSuccess = 683;

		public const short ThreadNeedleBeMatched1 = 684;

		public const short ThreadNeedleBeSeparated1 = 685;

		public const short ThreadNeedleBeMatched2 = 686;

		public const short ThreadNeedleBeSeparated2 = 687;

		public const short SpreadSecretKnown = 688;

		public const short SectMainStoryXuannvBirthOfMirrorCreatedImposture = 689;

		public const short EscapeFromEnemyNestBySelf = 690;

		public const short SaveFromInfection = 691;

		public const short SaveFromEnemyNest = 692;

		public const short SaveFromEnemyNestFailed = 695;

		public const short TameCarrierSucceed = 693;

		public const short TameCarrierFail = 694;

		public const short ReleaseCarrier = 696;

		public const short DLCLoongRidingEffectQiuniuAudience = 697;

		public const short DLCLoongRidingEffectQiuniu = 698;

		public const short DLCLoongRidingEffectYazi = 699;

		public const short DLCLoongRidingEffectChaofeng = 700;

		public const short DLCLoongRidingEffectPulao = 701;

		public const short DLCLoongRidingEffectSuanni = 702;

		public const short DLCLoongRidingEffectBaxia = 703;

		public const short DLCLoongRidingEffectBian = 704;

		public const short DLCLoongRidingEffectFuxi = 705;

		public const short DLCLoongRidingEffectChiwen = 706;

		public const short DefeatLoong = 707;

		public const short DefeatedByLoong = 708;

		public const short DLCLoongRidingEffectYazi2 = 709;

		public const short DieFromAge = 710;

		public const short DieFromPoorHealth = 711;

		public const short KilledInPublic = 713;

		public const short KilledInPrivate = 712;

		public const short KilledAfterXiangshuInfected = 714;

		public const short Assassinated = 715;

		public const short KilledByXiangshu = 716;

		public const short PurchaseItem1 = 717;

		public const short SellItem1 = 718;

		public const short CleanBodyReincarnationSuccess = 719;

		public const short CleanBodyReincarnationFail = 720;

		public const short EvilBodyReincarnationSuccess = 721;

		public const short EvilBodyReincarnationFail = 722;

		public const short WugKingForestSpiritBecomeEnemy = 723;

		public const short SecretMakeEnemy = 724;

		public const short SecretBeMadeEnemy = 725;

		public const short CleanBodyDefeatAnimal = 726;

		public const short EvilBodyDefeatAnimal = 727;

		public const short CleanBodyDefeatHereticRandomEnemy = 728;

		public const short EvilBodyDefeatHereticRandomEnemy = 729;

		public const short CleanBodyDefeatRighteousRandomEnemy = 730;

		public const short EvilBodyDefeatRighteousRandomEnemy = 731;

		public const short WuxianParanoiaAdded = 732;

		public const short WuxianParanoiaAttack = 733;

		public const short WuxianParanoiaErased = 734;

		public const short WugKingRedEyeLoseItem = 735;

		public const short WugForestSpiritReduceFavorability = 736;

		public const short WugKingForestSpiritBeBecomeEnemy = 737;

		public const short WugKingBlackBloodChangeDisorderOfQi = 738;

		public const short WugDevilInsideXiangshuInfection = 739;

		public const short WugCorpseWormChangeHealth = 740;

		public const short WugKingIceSilkwormLoseNeili = 741;

		public const short WugKingGoldenSilkwormEatGrownWug = 742;

		public const short WugAzureMarrowAddPoison = 743;

		public const short WugAzureMarrowAddWug = 744;

		public const short WugAzureMarrowBeAddWug = 745;

		public const short WuxianParanoiaErased2 = 746;

		public const short WuxianDecreasedMood = 747;

		public const short WuxianDecreasedFavorability = 748;

		public const short WuxianQiDecline = 749;

		public const short WuxianPoisoning = 750;

		public const short WuxianLoseItem = 751;

		public const short WugDevilInsideChangeHappiness = 752;

		public const short WugRedEyeChangeToGrown = 753;

		public const short WugForestSpiritChangeToGrown = 754;

		public const short WugBlackBloodChangeToGrown = 755;

		public const short WugDevilInsideChangeToGrown = 756;

		public const short WugCorpseWormChangeToGrown = 757;

		public const short WugCorpseWormBeChangeToGrown = 758;

		public const short WugIceSilkwormChangeToGrown = 759;

		public const short WugGoldenSilkwormChangeToGrown = 760;

		public const short WugAzureMarrowChangeToGrown = 761;

		public const short WugAzureMarrowBeChangeToGrown = 762;

		public const short ManageLearnLifeSkillSuccess = 763;

		public const short ManageLearnCombatSkillSuccess = 764;

		public const short ManageLearnLifeSkillFail = 765;

		public const short ManageLearnCombatSkillFail = 766;

		public const short ManageLifeSkillAbilityUp = 767;

		public const short ManageCombatSkillAbilityUp = 768;

		public const short SmallVillagerXiangshuCompletelyInfected = 769;

		public const short SmallVillagerSavedFromInfection = 770;

		public const short SmallVillagerSaveFromInfection = 771;

		public const short StorageResourceToTreasury = 772;

		public const short StorageItemToTreasury = 773;

		public const short TakeResourceFromTreasury = 774;

		public const short TakeItemFromTreasury = 775;

		public const short TaiwuStorageResourceToTreasury = 822;

		public const short TaiwuStorageItemToTreasury = 823;

		public const short TaiwuTakeResourceFromTreasury = 824;

		public const short TaiwuTakeItemFromTreasury = 825;

		public const short DecideToGuardTreasury = 776;

		public const short FinishGuardingTreasury = 777;

		public const short IntrudeTreasuryCancelSupportMakeEnemy = 778;

		public const short IntrudeTreasuryBeCancelSupportMakeEnemy = 779;

		public const short IntrudeTreasuryCancelSupport = 780;

		public const short IntrudeTreasuryBeCancelSupport = 781;

		public const short IntrudeTreasuryMakeEnemyOthers = 782;

		public const short IntrudeTreasuryBeMakeEnemyOthers = 783;

		public const short IntrudeTreasuryLostMorale = 784;

		public const short IntrudeTreasuryBeLostMorale = 785;

		public const short IntrudeTreasuryBeLostMorale2 = 786;

		public const short PlunderTreasuryCancelSupportMakeEnemy = 787;

		public const short PlunderTreasuryBeCancelSupportMakeEnemy = 788;

		public const short PlunderTreasuryCancelSupport = 789;

		public const short PlunderTreasuryBeCancelSupport = 790;

		public const short PlunderTreasuryMakeEnemyOthers = 791;

		public const short PlunderTreasuryBeMakeEnemyOthers = 792;

		public const short PlunderTreasuryLostMorale = 793;

		public const short PlunderTreasuryBeLostMorale = 794;

		public const short PlunderTreasuryBeLostMorale2 = 795;

		public const short DonateTreasuryProvideSupport = 796;

		public const short DonateTreasuryBeProvideSupport = 797;

		public const short DonateTreasuryGetMorale = 798;

		public const short DonateTreasuryBeGetMorale = 799;

		public const short DonateTreasuryGetMorale2 = 800;

		public const short TreasuryDistributeResource = 826;

		public const short TreasuryDistributeItem = 801;

		public const short PoisonEnemyFail12 = 802;

		public const short PoisonEnemyFail22 = 803;

		public const short PoisonEnemyFail32 = 804;

		public const short PoisonEnemyFail42 = 805;

		public const short PoisonEnemySucceed2 = 806;

		public const short PoisonEnemySucceedAndEscaped2 = 807;

		public const short GetPoisonedByEnemySucceed2 = 808;

		public const short PlotHarmEnemyFail12 = 809;

		public const short PlotHarmEnemyFail22 = 810;

		public const short PlotHarmEnemyFail32 = 811;

		public const short PlotHarmEnemyFail42 = 812;

		public const short PlotHarmEnemySucceed2 = 813;

		public const short PlotHarmEnemySucceedAndEscaped2 = 814;

		public const short GetPlottedAgainstSucceed2 = 815;

		public const short SectMainStoryBaihuaManiaLow = 816;

		public const short SectMainStoryBaihuaManiaHigh = 817;

		public const short SectMainStoryBaihuaManiaAttack = 818;

		public const short SectMainStoryBaihuaManiaAttacked = 819;

		public const short SectMainStoryBaihuaManiaCure = 820;

		public const short SectMainStoryBaihuaManiaCured = 821;

		public const short GiveUpLegendaryBookSuccessHuaJu = 827;

		public const short GiveUpLegendaryBookSuccessXuanZhi = 828;

		public const short GiveUpLegendaryBookSuccessYingJiao = 829;

		public const short SecretMakeEnemy2 = 830;

		public const short SecretBeMadeEnemy2 = 831;

		public const short DecideToHuntFugitive = 832;

		public const short FinishHuntFugitive = 833;

		public const short DecideToEscapePunishment = 834;

		public const short FinishEscapePunishment = 835;

		public const short DecideToSeekAsylum = 836;

		public const short FinishSeekAsylum = 837;

		public const short SeekAsylumSuccess = 838;

		public const short DecideToEscortPrisoner = 839;

		public const short EscortPrisonerSucceed = 840;

		public const short ImprisonedShaoLin = 841;

		public const short ImprisonedEmei1 = 842;

		public const short ImprisonedEmei2 = 843;

		public const short ImprisonedBaihua = 844;

		public const short ImprisonedWudang = 845;

		public const short ImprisonedYuanshan = 846;

		public const short ImprisonedShingXiang = 847;

		public const short ImprisonedRanShan = 848;

		public const short ImprisonedXuanNv = 849;

		public const short ImprisonedZhuJian = 850;

		public const short ImprisonedKongSang = 851;

		public const short ImprisonedJinGang = 852;

		public const short ImprisonedWuXian = 853;

		public const short ImprisonedJieQing1 = 854;

		public const short ImprisonedJieQing2 = 855;

		public const short ImprisonedFuLong = 856;

		public const short ImprisonedXueHou = 857;

		public const short IntrudePrisonCancelSupportMakeEnemyNpc = 858;

		public const short IntrudePrisonCancelSupportMakeEnemyTaiwu = 859;

		public const short IntrudePrisonCancelSupportNpc = 860;

		public const short IntrudePrisonCancelSupportTaiwu = 861;

		public const short IntrudePrisonMakeEnemyOthersNpc = 862;

		public const short IntrudePrisonMakeEnemyOthersTaiwu = 863;

		public const short RequestTheReleaseOfTheCriminalNpc = 864;

		public const short RequestTheReleaseOfTheCriminalTaiwu = 865;

		public const short ImprisonedXiangshuInfectedSupportIncreaseAndFavorabilityNpc = 866;

		public const short ImprisonedXiangshuInfectedSupportIncreaseAndFavorabilityTaiwu = 867;

		public const short ImprisonedXiangshuInfectedIncreaseFavorabilityNpc = 868;

		public const short ImprisonedXiangshuInfectedIncreaseFavorabilityTaiwu = 869;

		public const short ImprisonedXiangshuInfectedNpc = 870;

		public const short ImprisonedXiangshuInfectedTaiwu = 871;

		public const short RobbedFromPrisonNpc = 872;

		public const short PrisonBreakIntrudePrisonCancelSupportMakeEnemyNpc = 873;

		public const short PrisonBreakIntrudePrisonCancelSupportMakeEnemyTaiwu = 874;

		public const short PrisonBreakIntrudePrisonCancelSupportNpc = 875;

		public const short PrisonBreakIntrudePrisonCancelSupportTaiwu = 876;

		public const short PrisonBreakIntrudePrisonMakeEnemyOthersNpc = 877;

		public const short PrisonBreakIntrudePrisonMakeEnemyOthersTaiwu = 878;

		public const short ResistArrestIntrudePrisonCancelSupportMakeEnemyNpc = 879;

		public const short ResistArresPrisonBreakIntrudePrisonCancelSupportMakeEnemyTaiwu = 880;

		public const short ResistArresPrisonBreakIntrudePrisonCancelSupportNpc = 881;

		public const short ResistArresPrisonBreakIntrudePrisonCancelSupportTaiwu = 882;

		public const short ResistArresPrisonBreakIntrudePrisonMakeEnemyOthersNpc = 883;

		public const short ResistArresPrisonBreakIntrudePrisonMakeEnemyOthersTaiwu = 884;

		public const short ArrestFailedCaptor = 885;

		public const short ArrestFailedCriminal = 886;

		public const short ResistArresEngageInBattleTaiwu = 887;

		public const short ArrestedSuccessfullyCaptor = 888;

		public const short ArrestedSuccessfullyCriminal = 889;

		public const short ReceiveCriminalsCaptor = 890;

		public const short ReceiveCriminalsTaiwu = 891;

		public const short ReceiveCriminalsCriminal = 892;

		public const short BuyHandOverTheCriminalCaptor = 893;

		public const short BuyHandOverTheCriminalTaiwu = 894;

		public const short LifeSkillBattleHandOverTheCriminalCaptor = 895;

		public const short LifeSkillBattleHandOverTheCriminalTaiwu = 896;

		public const short LifeSkillBattleLoseHandOverTheCriminalCaptor = 897;

		public const short LifeSkillBattleLoseHandOverTheCriminalTaiwu = 898;

		public const short VictoryInCombatHandOverTheCriminalCaptor = 899;

		public const short VictoryInCombatHandOverTheCriminalTaiwu = 900;

		public const short FailureInCombatHandOverTheCriminalCaptor = 901;

		public const short FailureInCombatHandOverTheCriminalTaiwu = 902;

		public const short SectMainStoryFulongFightSucceed = 903;

		public const short SectMainStoryFulongFightFail = 904;

		public const short SectMainStoryFulongRobbery = 905;

		public const short SectMainStoryFulongRobberKilledByTaiwu = 906;

		public const short SectMainStoryFulongProtect = 907;

		public const short HonestSectPunishLevel1 = 908;

		public const short HonestSectPunishLevel2 = 909;

		public const short HonestSectPunishLevel3 = 910;

		public const short HonestSectPunishLevel4 = 911;

		public const short HonestSectPunishLevel5 = 912;

		public const short HonestSectPunishTogetherWithSpouseLevel5 = 913;

		public const short ArrestedSectPunishLevel1 = 914;

		public const short ArrestedSectPunishLevel2 = 915;

		public const short ArrestedSectPunishLevel3 = 916;

		public const short ArrestedSectPunishLevel4 = 917;

		public const short ArrestedSectPunishLevel5 = 918;

		public const short ArrestedSectPunishTogetherWithSpouseLevel5 = 919;

		public const short BeImplicatedSectPunishLevel5 = 920;

		public const short BeReleasedUponCompletionOfASentence = 921;

		public const short PrisonBreak = 922;

		public const short SendingToPrison1Taiwu = 923;

		public const short SendingToPrison2Taiwu = 924;

		public const short SendingToPrisonCriminal = 925;

		public const short SentToPrisonTaiwu = 926;

		public const short SentToPrisonCriminal = 927;

		public const short CatchCriminalsWinTaiwu = 928;

		public const short CatchCriminalsWinCriminal = 929;

		public const short CatchCriminalsFailedTaiwu = 930;

		public const short CatchCriminalsFailedCriminal = 931;

		public const short BuyHandOverTheCriminalCaptorByExp = 932;

		public const short BuyHandOverTheCriminalTaiwuByExp = 933;

		public const short SendingToPrison1TaiwuByExp = 934;

		public const short VillagerMigrateResources = 935;

		public const short VillagerCookingIngredient = 936;

		public const short VillagerMakingItem = 937;

		public const short VillagerRepairItem0 = 938;

		public const short VillagerRepairItem1 = 939;

		public const short VillagerDisassembleItem0 = 940;

		public const short VillagerDisassembleItem1 = 941;

		public const short VillagerRefiningMedicine = 942;

		public const short VillagerDetoxify0 = 943;

		public const short VillagerDetoxify1 = 944;

		public const short VillagerEnvenomedItem = 945;

		public const short VillagerSoldItem = 946;

		public const short VillagerBuyItem = 947;

		public const short VillagerSeverEnemy = 948;

		public const short VillagerEmotionUp = 949;

		public const short VillagerMakeFriends = 950;

		public const short VillagerGetMarried = 951;

		public const short VillagerBecomeBrothers = 952;

		public const short VillagerAdopt = 953;

		public const short VillagerTreatment0 = 954;

		public const short VillagerTreatment1 = 955;

		public const short VillagerBeTreatment0 = 956;

		public const short VillagerBeTreatment1 = 957;

		public const short XiangshuInfectedPrisonTaiwuVillage = 958;

		public const short XiangshuInfectedPrisonSettlement = 959;

		public const short VillagerBeRepairItem1 = 960;

		public const short TaiwuVillagerTakeItem = 961;

		public const short TaiwuVillagerStorageItem = 962;

		public const short TaiwuVillagerStorageResources = 963;

		public const short TaiwuVillagerTakeResources = 964;

		public const short LiteratiEntertainingUp = 965;

		public const short LiteratiEntertainingDown = 966;

		public const short LiteratiBuildingRelationshipUp = 967;

		public const short LiteratiBuildingRelationshipDown = 968;

		public const short LiteratiSpreadingInfluenceUp = 969;

		public const short LiteratiSpreadingInfluenceDown = 970;

		public const short SwordTombKeeperBuildingRelationshipUp = 971;

		public const short SwordTombKeeperBuildingRelationshipDown = 972;

		public const short SwordTombKeeperSpreadingInfluenceUp = 973;

		public const short SwordTombKeeperSpreadingInfluenceDown = 974;

		public const short InquireSwordTomb = 975;

		public const short GuardingSwordTomb = 976;

		public const short VillagerPrioritizedActions = 977;

		public const short VillagerPrioritizedActionsStop = 978;

		public const short EnvenomedItemOverload = 979;

		public const short DetoxifyItemOverload = 980;

		public const short VillagerEnvenomedItemOverload = 981;

		public const short VillagerDetoxifyItemOverload = 982;

		public const short VillagerCookingIngredientFailed0 = 983;

		public const short VillagerCookingIngredientFailed1 = 984;

		public const short VillagerMakingItemFailed0 = 985;

		public const short VillagerMakingItemFailed1 = 986;

		public const short VillagerRepairFailed = 987;

		public const short VillagerDisassembleItemFailed = 988;

		public const short VillagerRefiningMedicineFailed0 = 989;

		public const short VillagerRefiningMedicineFailed1 = 990;

		public const short VillagerAddPoisonToItemFailed = 991;

		public const short VillagerDetoxItemFailed = 992;

		public const short VillagerDistanceFailed0 = 993;

		public const short VillagerDistanceFailed1 = 994;

		public const short VillagerDistanceFailed2 = 995;

		public const short VillagerAttainmentsFailed = 996;

		public const short TaiwuPunishmentTongyong = 997;

		public const short TaiwuPunishmentShaolin = 998;

		public const short TaiwuPunishmentEmei = 999;

		public const short TaiwuPunishmentBaihua = 1000;

		public const short TaiwuPunishmentWudang = 1001;

		public const short TaiwuPunishmentYuanshan = 1002;

		public const short TaiwuPunishmentShingXiang = 1003;

		public const short TaiwuPunishmentRanShan = 1004;

		public const short TaiwuPunishmentXuanNv = 1005;

		public const short TaiwuPunishmentZhuJian = 1006;

		public const short TaiwuPunishmentKongSang = 1007;

		public const short TaiwuPunishmentJinGang = 1008;

		public const short TaiwuPunishmentWuXian = 1009;

		public const short TaiwuPunishmentJieQing = 1010;

		public const short TaiwuPunishmentFuLong = 1011;

		public const short TaiwuPunishmentXueHou = 1012;

		public const short SectPunishLevel5Expel = 1013;

		public const short BeImplicatedSectPunishLevel5New = 1014;

		public const short BeImplicatedSectPunishLevel5Expel = 1015;

		public const short ResistArrestIntrudePrisonCancelSupportMakeEnemyNpcGuard = 1016;

		public const short ResistArresPrisonBreakIntrudePrisonCancelSupportMakeEnemyTaiwuWanted = 1017;

		public const short ResistArresPrisonBreakIntrudePrisonCancelSupportNpcGuard = 1018;

		public const short ResistArresPrisonBreakIntrudePrisonCancelSupportTaiwuWanted = 1019;

		public const short ResistArresPrisonBreakIntrudePrisonMakeEnemyOthersNpcGuard = 1020;

		public const short ResistArresPrisonBreakIntrudePrisonMakeEnemyOthersTaiwuWanted = 1021;

		public const short ForgiveForCivilianSkill = 1022;

		public const short BeggarEatSomeoneFood = 1023;

		public const short SomeoneFoodEatedByBeggar = 1024;

		public const short AristocratReleasePrisoner = 1025;

		public const short PrisonerBeReleaseByAristocrat = 1026;

		public const short JieQingPunishmentAssassinSetOut = 1027;

		public const short JieQingPunishmentAssassinSucceed = 1028;

		public const short JieQingPunishmentAssassinBeSucceed = 1029;

		public const short JieQingPunishmentAssassinFailed = 1030;

		public const short JieQingPunishmentAssassinBeFailed = 1031;

		public const short JieQingPunishmentAssassinGiveUp = 1032;

		public const short ExociseXiangshuInfectionVictoryInCombatDie = 1033;

		public const short BecomeExociseXiangshuInfectionVictoryInCombatDie = 1034;

		public const short ArrestFailedTaiwu = 1035;

		public const short ArrestedSuccessfullyTaiwu = 1036;

		public const short LifeSkillBattleLoseAndTheArrestFailedCaptor = 1037;

		public const short LifeSkillBattleWinAndAvoidArrestTaiwu = 1038;

		public const short LifeSkillBattleWinAndSuccessfulArrestCaptor = 1039;

		public const short LifeSkillBattleLoseAndWasArrestedTaiwu = 1040;

		public const short FailedArrestForBriberyCaptorByAuthority = 1041;

		public const short BribeSucceededInAvoidingArrestTaiwuByAuthority = 1042;

		public const short FailedArrestForBriberyCaptorByExp = 1043;

		public const short BribeSucceededInAvoidingArrestTaiwuByExp = 1044;

		public const short FailedArrestForBriberyCaptorByMoney = 1045;

		public const short BribeSucceededInAvoidingArrestTaiwuByMoney = 1046;

		public const short SubmitToCaptureMeeklyTaiwu = 1047;

		public const short SubmitToCaptureMeeklyCaptor = 1048;

		public const short NormalInformationChangeProfession = 1049;

		public const short FeedTheAnimal = 1050;

		public const short ProfessionDoctorLifeTransition = 1051;

		public const short ProfessionDoctorLifeTransitionTaiwu = 1052;

		public const short CombatSkillKeyPointComprehensionByExp = 1053;

		public const short CombatSkillKeyPointComprehensionByItems = 1054;

		public const short CombatSkillKeyPointComprehensionByLoveRelationship = 1055;

		public const short CombatSkillKeyPointComprehensionByHatredRelationship = 1056;

		public const short SpiritualDebtKongsangPoisoned = 1057;

		public const short MartialArtistSkill3NPCItemDropCaseA = 1058;

		public const short MartialArtistSkill3NPCItemDropCaseB = 1059;

		public const short SectPunishElopeSucceedJust = 1060;

		public const short SectPunishElopeSucceedKind = 1061;

		public const short SectPunishElopeSucceedEven = 1062;

		public const short SectPunishElopeSucceed = 1063;

		public const short VillagerGetRefineItem = 1064;

		public const short VillagerUpgradeRefineItem = 1065;

		public const short VillagerTreatmentTaiwu = 1066;

		public const short VillagerReduceXiangshuInfect = 1067;

		public const short VillagerEarnMoney = 1068;

		public const short VillagerBeEarnedMoney = 1069;

		public const short VillagerBeSoldItem = 1070;

		public const short VillagerBePurchasedItem = 1071;

		public const short VillagerGetMerchantFavorability = 1072;

		public const short VillagerGetMerchantFavorabilityTaiwu = 1073;

		public const short LiteratiBeEntertainedUp = 1074;

		public const short LiteratiBeEntertainedDown = 1075;

		public const short LiteratiSpreadingInfluenceCultureUp = 1076;

		public const short LiteratiSpreadingInfluenceCultureDown = 1077;

		public const short LiteratiSpreadingInfluenceSafetyUp = 1078;

		public const short LiteratiSpreadingInfluenceSafetyDown = 1079;

		public const short LiteratiConnectRelationshipUp = 1080;

		public const short LiteratiConnectRelationshipDown = 1081;

		public const short LiteratiConnectRelationshipUpTaiwu = 1082;

		public const short LiteratiConnectRelationshipDownTaiwu = 1083;

		public const short LiteratiBeConnectedRelationshipUp = 1084;

		public const short LiteratiBeConnectedRelationshipDown = 1085;

		public const short GuardingSwordTombXiangshuInfectUp = 1086;

		public const short GuardingSwordTombSucceed = 1087;

		public const short VillagerMakeEnemy = 1088;

		public const short VillagerConfessLoveSucceed = 1089;

		public const short OrderProduct = 1090;

		public const short ReceiveProduct = 1091;

		public const short BeOrderProduct = 1095;

		public const short BeReceiveProduct = 1096;

		public const short CaptureOrder = 1093;

		public const short BeCaptureOrder = 1094;

		public const short CaptureOrderIntermediator = 1097;

		public const short OrderProductForOthers = 1098;

		public const short BeOrderProductForOthers = 1099;

		public const short DeliveredOrderProduct = 1100;

		public const short BeDeliveredOrderProduct = 1101;

		public const short AcquisitionDiscard = 1092;

		public const short ShopBuildingBaseDevelopLifeSkill = 1102;

		public const short ShopBuildingBaseDevelopCombatSkill = 1103;

		public const short ShopBuildingPersonalityDevelopLifeSkill = 1104;

		public const short ShopBuildingPersonalityDevelopCombatSkill = 1105;

		public const short ShopBuildingLeaderDevelopLifeSkill = 1106;

		public const short ShopBuildingLeaderDevelopCombatSkill = 1107;

		public const short ShopBuildingLearnLifeSkill = 1108;

		public const short ShopBuildingLearnCombatSkill = 1109;

		public const short JoinTaiwuVillageAfterTaiwuVillageStoneClaimed = 1110;

		public const short TaiwuVillagerFinishedReading = 1111;

		public const short TaiwuVillagerSalaryReceived = 1112;

		public const short ChangeGradeDrop = 1113;

		public const short FarmerCollectMaterial = 1114;

		public const short JoinOrganization = 1115;

		public const short BreakAwayOrganization = 1116;

		public const short ChangeOrganization = 1117;

		public const short VillagerFavorabilityUp = 1118;

		public const short VillagerFavorabilityDown = 1119;

		public const short VillagerFavorabilityUpPerson = 1120;

		public const short VillagerFavorabilityDownPerson = 1121;

		public const short TeamUpProtection = 1122;

		public const short TeamUpRescue = 1123;

		public const short TeamUpMourn = 1124;

		public const short TeamUpVisitFriendOrFamily = 1125;

		public const short TeamUpFindTreasure = 1126;

		public const short TeamUpFindSpecialMaterial = 1127;

		public const short TeamUpTakeRevenge = 1128;

		public const short TeamUpContestForLegendaryBook = 1129;

		public const short TeamUpEscapeFromPrison = 1130;

		public const short TeamUpSeekAsylum = 1131;

		public const short GetInfected = 1132;

		public const short DieByInfected = 1133;

		public const short InheritLegacy = 1134;

		public const short Banquet_1 = 1135;

		public const short Banquet_2 = 1136;

		public const short Banquet_3 = 1137;

		public const short Banquet_4 = 1138;

		public const short Banquet_5 = 1139;

		public const short Banquet_6 = 1140;

		public const short Banquet_7 = 1141;

		public const short Banquet_8 = 1142;

		public const short Banquet_9 = 1143;

		public const short Banquet_10 = 1144;

		public const short SectMainStoryWudangInjured = 1145;

		public const short ExtendDarkAshTime = 1146;

		public const short AdoreInMarriage = 1147;

		public const short SameAreaDistantMarriage = 1148;

		public const short SameStateDistantMarriage = 1149;

		public const short DifferentStateDistantMarriage = 1150;
	}

	public static class DefValue
	{
		public static LifeRecordItem Die => Instance[(short)0];

		public static LifeRecordItem XiangshuPartiallyInfected => Instance[(short)1];

		public static LifeRecordItem XiangshuCompletelyInfected => Instance[(short)2];

		public static LifeRecordItem MotherLoseFetus => Instance[(short)3];

		public static LifeRecordItem FatherLoseFetus => Instance[(short)4];

		public static LifeRecordItem AbandonChild => Instance[(short)5];

		public static LifeRecordItem ChildGetAbandoned => Instance[(short)6];

		public static LifeRecordItem GiveBirthToCricket => Instance[(short)7];

		public static LifeRecordItem GiveBirthToBoy => Instance[(short)8];

		public static LifeRecordItem GiveBirthToGirl => Instance[(short)9];

		public static LifeRecordItem BecomeFatherToNewBornBoy => Instance[(short)10];

		public static LifeRecordItem BecomeFatherToNewBornGirl => Instance[(short)11];

		public static LifeRecordItem BuildGrave => Instance[(short)12];

		public static LifeRecordItem MonkBreakRule => Instance[(short)13];

		public static LifeRecordItem KidnappedCharacterEscaped => Instance[(short)14];

		public static LifeRecordItem EscapeFromKidnapping => Instance[(short)15];

		public static LifeRecordItem ReadBookSucceed => Instance[(short)16];

		public static LifeRecordItem ReadBookFail => Instance[(short)17];

		public static LifeRecordItem BreakoutSucceed => Instance[(short)18];

		public static LifeRecordItem BreakoutFail => Instance[(short)19];

		public static LifeRecordItem LearnCombatSkill => Instance[(short)20];

		public static LifeRecordItem LearnLifeSkill => Instance[(short)21];

		public static LifeRecordItem RepairItem => Instance[(short)22];

		public static LifeRecordItem AddPoisonToItem => Instance[(short)23];

		public static LifeRecordItem LoseOverloadingResource => Instance[(short)24];

		public static LifeRecordItem LoseOverloadingItem => Instance[(short)25];

		public static LifeRecordItem MakeEnemy => Instance[(short)26];

		public static LifeRecordItem SeverEnemy => Instance[(short)27];

		public static LifeRecordItem BeMadeEnemy => Instance[(short)28];

		public static LifeRecordItem SeveredEnemy => Instance[(short)29];

		public static LifeRecordItem Adore => Instance[(short)30];

		public static LifeRecordItem LoveAtFirstSight => Instance[(short)31];

		public static LifeRecordItem ConfessLoveSucceed => Instance[(short)32];

		public static LifeRecordItem ConfessLoveFail => Instance[(short)33];

		public static LifeRecordItem AcceptConfessLove => Instance[(short)34];

		public static LifeRecordItem RefuseConfessLove => Instance[(short)35];

		public static LifeRecordItem BreakupMutually => Instance[(short)36];

		public static LifeRecordItem DumpLover => Instance[(short)37];

		public static LifeRecordItem GetDumppedByLover => Instance[(short)38];

		public static LifeRecordItem ProposeMarriageSucceed => Instance[(short)39];

		public static LifeRecordItem ProposeMarriageFail => Instance[(short)40];

		public static LifeRecordItem RefuseMarriageProposal => Instance[(short)41];

		public static LifeRecordItem BecomeFriend => Instance[(short)42];

		public static LifeRecordItem SeverFriendship => Instance[(short)43];

		public static LifeRecordItem BecomeSwornBrotherOrSister => Instance[(short)44];

		public static LifeRecordItem SeverSwornBrotherhood => Instance[(short)45];

		public static LifeRecordItem GetAdoptedByFather => Instance[(short)46];

		public static LifeRecordItem GetAdoptedByMother => Instance[(short)47];

		public static LifeRecordItem AdoptSon => Instance[(short)48];

		public static LifeRecordItem AdoptDaughter => Instance[(short)49];

		public static LifeRecordItem CreateFaction => Instance[(short)50];

		public static LifeRecordItem JoinFaction => Instance[(short)51];

		public static LifeRecordItem LeaveFaction => Instance[(short)52];

		public static LifeRecordItem FactionRecruitSucceed => Instance[(short)53];

		public static LifeRecordItem FactionRecruitFail => Instance[(short)54];

		public static LifeRecordItem AgreeToJoinFaction => Instance[(short)55];

		public static LifeRecordItem RefuseToJoinFaction => Instance[(short)56];

		public static LifeRecordItem DecideToJoinSect => Instance[(short)57];

		public static LifeRecordItem DecideToFullfillAppointment => Instance[(short)58];

		public static LifeRecordItem DecideToProtect => Instance[(short)59];

		public static LifeRecordItem DecideToRescue => Instance[(short)60];

		public static LifeRecordItem DecideToMourn => Instance[(short)61];

		public static LifeRecordItem DecideToVisit => Instance[(short)62];

		public static LifeRecordItem DecideToFindLostItem => Instance[(short)63];

		public static LifeRecordItem DecideToFindSpecialMaterial => Instance[(short)64];

		public static LifeRecordItem DecideToRevenge => Instance[(short)65];

		public static LifeRecordItem DecideToParticipateAdventure => Instance[(short)66];

		public static LifeRecordItem JoinSectFail => Instance[(short)67];

		public static LifeRecordItem JoinSectSucceed => Instance[(short)68];

		public static LifeRecordItem CanNoLongerFullFillAppointment => Instance[(short)69];

		public static LifeRecordItem WaitForAppointment => Instance[(short)70];

		public static LifeRecordItem FullFillAppointment => Instance[(short)71];

		public static LifeRecordItem FinishProtection => Instance[(short)72];

		public static LifeRecordItem OfferProtection => Instance[(short)73];

		public static LifeRecordItem FinishRescue => Instance[(short)74];

		public static LifeRecordItem FinishMourning => Instance[(short)75];

		public static LifeRecordItem MaintainGrave => Instance[(short)76];

		public static LifeRecordItem UpgradeGrave => Instance[(short)77];

		public static LifeRecordItem FinishVisit => Instance[(short)78];

		public static LifeRecordItem FinishFIndingLostItem => Instance[(short)79];

		public static LifeRecordItem FinishFIndingSpecialMaterial => Instance[(short)80];

		public static LifeRecordItem FindLostItemSucceed => Instance[(short)81];

		public static LifeRecordItem FindLostItemFail => Instance[(short)82];

		public static LifeRecordItem FindSpecialMaterialSucceed => Instance[(short)83];

		public static LifeRecordItem FinishTakingRevenge => Instance[(short)84];

		public static LifeRecordItem MajorVictoryInCombat => Instance[(short)85];

		public static LifeRecordItem MajorFailureInCombat => Instance[(short)86];

		public static LifeRecordItem VictoryInCombat => Instance[(short)87];

		public static LifeRecordItem FailureInCombat => Instance[(short)88];

		public static LifeRecordItem EnemyEscape => Instance[(short)89];

		public static LifeRecordItem LoseAndEscape => Instance[(short)90];

		public static LifeRecordItem KillInPublic => Instance[(short)91];

		public static LifeRecordItem KillInPrivate => Instance[(short)92];

		public static LifeRecordItem KidnapInPublic => Instance[(short)93];

		public static LifeRecordItem KidnapInPrivate => Instance[(short)94];

		public static LifeRecordItem ReleaseLoser => Instance[(short)95];

		public static LifeRecordItem GetKidnappedInPublic => Instance[(short)96];

		public static LifeRecordItem GetKidnappedInPrivate => Instance[(short)97];

		public static LifeRecordItem GetReleasedByWinner => Instance[(short)98];

		public static LifeRecordItem AgreeToProtect => Instance[(short)99];

		public static LifeRecordItem RefuseToProtect => Instance[(short)100];

		public static LifeRecordItem FinishAdventure => Instance[(short)101];

		public static LifeRecordItem RequestHealOuterInjurySucceed => Instance[(short)102];

		public static LifeRecordItem RequestHealInnerInjurySucceed => Instance[(short)103];

		public static LifeRecordItem RequestDetoxPoisonSucceed => Instance[(short)104];

		public static LifeRecordItem RequestHealthSucceed => Instance[(short)105];

		public static LifeRecordItem RequestHealDisorderOfQiSucceed => Instance[(short)106];

		public static LifeRecordItem RequestNeiliSucceed => Instance[(short)107];

		public static LifeRecordItem RequestKillWugSucceed => Instance[(short)108];

		public static LifeRecordItem RequestFoodSucceed => Instance[(short)109];

		public static LifeRecordItem RequestTeaWineSucceed => Instance[(short)110];

		public static LifeRecordItem RequestResourceSucceed => Instance[(short)111];

		public static LifeRecordItem RequestItemSucceed => Instance[(short)112];

		public static LifeRecordItem RequestRepairItemSucceed => Instance[(short)113];

		public static LifeRecordItem RequestAddPoisonToItemSucceed => Instance[(short)114];

		public static LifeRecordItem RequestInstructionOnLifeSkillSucceed => Instance[(short)115];

		public static LifeRecordItem RequestInstructionOnCombatSkillSucceed => Instance[(short)116];

		public static LifeRecordItem RequestInstructionOnLifeSkillFailToLearn => Instance[(short)117];

		public static LifeRecordItem RequestInstructionOnCombatSkillFailToLearn => Instance[(short)118];

		public static LifeRecordItem RequestInstructionOnReadingSucceed => Instance[(short)119];

		public static LifeRecordItem RequestInstructionOnBreakoutSucceed => Instance[(short)120];

		public static LifeRecordItem RequestHealOuterInjuryFail => Instance[(short)121];

		public static LifeRecordItem RequestHealInnerInjuryFail => Instance[(short)122];

		public static LifeRecordItem RequestDetoxPoisonFail => Instance[(short)123];

		public static LifeRecordItem RequestHealthFail => Instance[(short)124];

		public static LifeRecordItem RequestHealDisorderOfQiFail => Instance[(short)125];

		public static LifeRecordItem RequestNeiliFail => Instance[(short)126];

		public static LifeRecordItem RequestKillWugFail => Instance[(short)127];

		public static LifeRecordItem RequestFoodFail => Instance[(short)128];

		public static LifeRecordItem RequestTeaWineFail => Instance[(short)129];

		public static LifeRecordItem RequestResourceFail => Instance[(short)130];

		public static LifeRecordItem RequestItemFail => Instance[(short)131];

		public static LifeRecordItem RequestRepairItemFail => Instance[(short)132];

		public static LifeRecordItem RequestAddPoisonToItemFail => Instance[(short)133];

		public static LifeRecordItem RequestInstructionOnLifeSkillFail => Instance[(short)134];

		public static LifeRecordItem RequestInstructionOnCombatSkillFail => Instance[(short)135];

		public static LifeRecordItem RequestInstructionOnReadingFail => Instance[(short)136];

		public static LifeRecordItem RequestInstructionOnBreakoutFail => Instance[(short)137];

		public static LifeRecordItem AcceptRequestHealOuterInjury => Instance[(short)138];

		public static LifeRecordItem AcceptRequestHealInnerInjury => Instance[(short)139];

		public static LifeRecordItem AcceptRequestDetoxPoison => Instance[(short)140];

		public static LifeRecordItem AcceptRequestHealth => Instance[(short)141];

		public static LifeRecordItem AcceptRequestHealDisorderOfQi => Instance[(short)142];

		public static LifeRecordItem AcceptRequestNeili => Instance[(short)143];

		public static LifeRecordItem AcceptRequestKillWug => Instance[(short)144];

		public static LifeRecordItem AcceptRequestFood => Instance[(short)145];

		public static LifeRecordItem AcceptRequestTeaWine => Instance[(short)146];

		public static LifeRecordItem AcceptRequestResource => Instance[(short)147];

		public static LifeRecordItem AcceptRequestItem => Instance[(short)148];

		public static LifeRecordItem AcceptRequestRepairItem => Instance[(short)149];

		public static LifeRecordItem AcceptRequestAddPoisonToItem => Instance[(short)150];

		public static LifeRecordItem AcceptRequestInstructionOnLifeSkill => Instance[(short)151];

		public static LifeRecordItem AcceptRequestInstructionOnCombatSkill => Instance[(short)152];

		public static LifeRecordItem AcceptRequestInstructionOnLifeSkillButFail => Instance[(short)153];

		public static LifeRecordItem AcceptRequestInstructionOnCombatSkillButFail => Instance[(short)154];

		public static LifeRecordItem AcceptRequestInstructionOnReading => Instance[(short)155];

		public static LifeRecordItem AcceptRequestInstructionOnBreakout => Instance[(short)156];

		public static LifeRecordItem RefuseRequestHealOuterInjury => Instance[(short)157];

		public static LifeRecordItem RefuseRequestHealInnerInjury => Instance[(short)158];

		public static LifeRecordItem RefuseRequestDetoxPoison => Instance[(short)159];

		public static LifeRecordItem RefuseRequestHealth => Instance[(short)160];

		public static LifeRecordItem RefuseRequestHealDisorderOfQi => Instance[(short)161];

		public static LifeRecordItem RefuseRequestNeili => Instance[(short)162];

		public static LifeRecordItem RefuseRequestKillWug => Instance[(short)163];

		public static LifeRecordItem RefuseRequestFood => Instance[(short)164];

		public static LifeRecordItem RefuseRequestTeaWine => Instance[(short)165];

		public static LifeRecordItem RefuseRequestResource => Instance[(short)166];

		public static LifeRecordItem RefuseRequestItem => Instance[(short)167];

		public static LifeRecordItem RefuseRequestRepairItem => Instance[(short)168];

		public static LifeRecordItem RefuseRequestAddPoisonToItem => Instance[(short)169];

		public static LifeRecordItem RefuseRequestInstructionOnLifeSkill => Instance[(short)170];

		public static LifeRecordItem RefuseRequestInstructionOnCombatSkill => Instance[(short)171];

		public static LifeRecordItem RefuseRequestInstructionOnReading => Instance[(short)172];

		public static LifeRecordItem RefuseRequestInstructionOnBreakout => Instance[(short)173];

		public static LifeRecordItem RescueKidnappedCharacterSecretlyFail1 => Instance[(short)174];

		public static LifeRecordItem RescueKidnappedCharacterSecretlyFail2 => Instance[(short)175];

		public static LifeRecordItem RescueKidnappedCharacterSecretlyFail3 => Instance[(short)176];

		public static LifeRecordItem RescueKidnappedCharacterSecretlyFail4 => Instance[(short)177];

		public static LifeRecordItem RescueKidnappedCharacterSecretlySucceed => Instance[(short)178];

		public static LifeRecordItem RescueKidnappedCharacterSecretlySucceedAndEscaped => Instance[(short)179];

		public static LifeRecordItem KidnappedCharacterGetRescuedSecretly => Instance[(short)180];

		public static LifeRecordItem RescueKidnappedCharacterWithWitFail1 => Instance[(short)181];

		public static LifeRecordItem RescueKidnappedCharacterWithWitFail2 => Instance[(short)182];

		public static LifeRecordItem RescueKidnappedCharacterWithWitFail3 => Instance[(short)183];

		public static LifeRecordItem RescueKidnappedCharacterWithWitFail4 => Instance[(short)184];

		public static LifeRecordItem RescueKidnappedCharacterWithWitSucceed => Instance[(short)185];

		public static LifeRecordItem RescueKidnappedCharacterWithWitSucceedAndEscaped => Instance[(short)186];

		public static LifeRecordItem KidnappedCharacterGetRescuedWithWit => Instance[(short)187];

		public static LifeRecordItem RescueKidnappedCharacterWithForceFail1 => Instance[(short)188];

		public static LifeRecordItem RescueKidnappedCharacterWithForceFail2 => Instance[(short)189];

		public static LifeRecordItem RescueKidnappedCharacterWithForceFail3 => Instance[(short)190];

		public static LifeRecordItem RescueKidnappedCharacterWithForceFail4 => Instance[(short)191];

		public static LifeRecordItem RescueKidnappedCharacterWithForceSucceed => Instance[(short)192];

		public static LifeRecordItem RescueKidnappedCharacterWithForceSucceedAndEscaped => Instance[(short)193];

		public static LifeRecordItem KidnappedCharacterGetRescuedWithForce => Instance[(short)194];

		public static LifeRecordItem PoisonEnemyFail1 => Instance[(short)195];

		public static LifeRecordItem PoisonEnemyFail2 => Instance[(short)196];

		public static LifeRecordItem PoisonEnemyFail3 => Instance[(short)197];

		public static LifeRecordItem PoisonEnemyFail4 => Instance[(short)198];

		public static LifeRecordItem PoisonEnemySucceed => Instance[(short)199];

		public static LifeRecordItem PoisonEnemySucceedAndEscaped => Instance[(short)200];

		public static LifeRecordItem GetPoisonedByEnemySucceed => Instance[(short)201];

		public static LifeRecordItem PlotHarmEnemyFail1 => Instance[(short)202];

		public static LifeRecordItem PlotHarmEnemyFail2 => Instance[(short)203];

		public static LifeRecordItem PlotHarmEnemyFail3 => Instance[(short)204];

		public static LifeRecordItem PlotHarmEnemyFail4 => Instance[(short)205];

		public static LifeRecordItem PlotHarmEnemySucceed => Instance[(short)206];

		public static LifeRecordItem PlotHarmEnemySucceedAndEscaped => Instance[(short)207];

		public static LifeRecordItem GetPlottedAgainstSucceed => Instance[(short)208];

		public static LifeRecordItem StealResourceFail1 => Instance[(short)209];

		public static LifeRecordItem StealResourceFail2 => Instance[(short)210];

		public static LifeRecordItem StealResourceFail3 => Instance[(short)211];

		public static LifeRecordItem StealResourceFail4 => Instance[(short)212];

		public static LifeRecordItem StealResourceSucceed => Instance[(short)213];

		public static LifeRecordItem StealResourceSucceedAndEscaped => Instance[(short)214];

		public static LifeRecordItem StealResourceFailAndBeatenUp => Instance[(short)215];

		public static LifeRecordItem ResourceGetStolenSucceed => Instance[(short)216];

		public static LifeRecordItem BeatUpResourceStealer => Instance[(short)217];

		public static LifeRecordItem ScamResourceFail1 => Instance[(short)218];

		public static LifeRecordItem ScamResourceFail2 => Instance[(short)219];

		public static LifeRecordItem ScamResourceFail3 => Instance[(short)220];

		public static LifeRecordItem ScamResourceFail4 => Instance[(short)221];

		public static LifeRecordItem ScamResourceSucceed => Instance[(short)222];

		public static LifeRecordItem ScamResourceSucceedAndEscaped => Instance[(short)223];

		public static LifeRecordItem ScamResourceFailAndBeatenUp => Instance[(short)224];

		public static LifeRecordItem ResourceGetScammedSucceed => Instance[(short)225];

		public static LifeRecordItem BeatUpResourceScammer => Instance[(short)226];

		public static LifeRecordItem RobResourceFail1 => Instance[(short)227];

		public static LifeRecordItem RobResourceFail2 => Instance[(short)228];

		public static LifeRecordItem RobResourceFail3 => Instance[(short)229];

		public static LifeRecordItem RobResourceFail4 => Instance[(short)230];

		public static LifeRecordItem RobResourceSucceed => Instance[(short)231];

		public static LifeRecordItem RobResourceSucceedAndEscaped => Instance[(short)232];

		public static LifeRecordItem RobResourceFailAndBeatenUp => Instance[(short)233];

		public static LifeRecordItem ResourceGetRobbedSucceed => Instance[(short)234];

		public static LifeRecordItem BeatUpResourceRobber => Instance[(short)235];

		public static LifeRecordItem StealItemFail1 => Instance[(short)236];

		public static LifeRecordItem StealItemFail2 => Instance[(short)237];

		public static LifeRecordItem StealItemFail3 => Instance[(short)238];

		public static LifeRecordItem StealItemFail4 => Instance[(short)239];

		public static LifeRecordItem StealItemSucceed => Instance[(short)240];

		public static LifeRecordItem StealItemSucceedAndEscaped => Instance[(short)241];

		public static LifeRecordItem StealItemSucceedAndBeatenUp => Instance[(short)242];

		public static LifeRecordItem ItemGetStolenSucceed => Instance[(short)243];

		public static LifeRecordItem BeatUpItemStealer => Instance[(short)244];

		public static LifeRecordItem ScamItemFail1 => Instance[(short)245];

		public static LifeRecordItem ScamItemFail2 => Instance[(short)246];

		public static LifeRecordItem ScamItemFail3 => Instance[(short)247];

		public static LifeRecordItem ScamItemFail4 => Instance[(short)248];

		public static LifeRecordItem ScamItemSucceed => Instance[(short)249];

		public static LifeRecordItem ScamItemSucceedAndEscaped => Instance[(short)250];

		public static LifeRecordItem ScamItemFailAndBeatenUp => Instance[(short)251];

		public static LifeRecordItem ItemGetScammedSucceed => Instance[(short)252];

		public static LifeRecordItem BeatUpItemScammer => Instance[(short)253];

		public static LifeRecordItem RobItemFail1 => Instance[(short)254];

		public static LifeRecordItem RobItemFail2 => Instance[(short)255];

		public static LifeRecordItem RobItemFail3 => Instance[(short)256];

		public static LifeRecordItem RobItemFail4 => Instance[(short)257];

		public static LifeRecordItem RobItemSucceed => Instance[(short)258];

		public static LifeRecordItem RobItemSucceedAndEscaped => Instance[(short)259];

		public static LifeRecordItem RobItemFailAndBeatenUp => Instance[(short)260];

		public static LifeRecordItem ItemGetRobbedSucceed => Instance[(short)261];

		public static LifeRecordItem BeatUpItemRobber => Instance[(short)262];

		public static LifeRecordItem RobResourceFromGraveSucceed => Instance[(short)263];

		public static LifeRecordItem RobResourceFromGraveFail => Instance[(short)264];

		public static LifeRecordItem RobItemFromGraveSucceed => Instance[(short)265];

		public static LifeRecordItem RobItemFromGraveFail => Instance[(short)266];

		public static LifeRecordItem StealLifeSkillFail1 => Instance[(short)267];

		public static LifeRecordItem StealLifeSkillFail2 => Instance[(short)268];

		public static LifeRecordItem StealLifeSkillFail3 => Instance[(short)269];

		public static LifeRecordItem StealLifeSkillFail4 => Instance[(short)270];

		public static LifeRecordItem StealLifeSkillSucceed => Instance[(short)271];

		public static LifeRecordItem StealLifeSkillSucceedAndEscaped => Instance[(short)272];

		public static LifeRecordItem LifeSkillGetStolenSucceed => Instance[(short)273];

		public static LifeRecordItem ScamLifeSkillFail1 => Instance[(short)274];

		public static LifeRecordItem ScamLifeSkillFail2 => Instance[(short)275];

		public static LifeRecordItem ScamLifeSkillFail3 => Instance[(short)276];

		public static LifeRecordItem ScamLifeSkillFail4 => Instance[(short)277];

		public static LifeRecordItem ScamLifeSkillSucceed => Instance[(short)278];

		public static LifeRecordItem ScamLifeSkillSucceedAndEscaped => Instance[(short)279];

		public static LifeRecordItem LifeSkillGetScammedSucceed => Instance[(short)280];

		public static LifeRecordItem StealCombatSkillFail1 => Instance[(short)281];

		public static LifeRecordItem StealCombatSkillFail2 => Instance[(short)282];

		public static LifeRecordItem StealCombatSkillFail3 => Instance[(short)283];

		public static LifeRecordItem StealCombatSkillFail4 => Instance[(short)284];

		public static LifeRecordItem StealCombatSkillSucceed => Instance[(short)285];

		public static LifeRecordItem StealCombatSkillSucceedAndEscaped => Instance[(short)286];

		public static LifeRecordItem CombatSkillGetStolenSucceed => Instance[(short)287];

		public static LifeRecordItem ScamCombatSkillFail1 => Instance[(short)288];

		public static LifeRecordItem ScamCombatSkillFail2 => Instance[(short)289];

		public static LifeRecordItem ScamCombatSkillFail3 => Instance[(short)290];

		public static LifeRecordItem ScamCombatSkillFail4 => Instance[(short)291];

		public static LifeRecordItem ScamCombatSkillSucceed => Instance[(short)292];

		public static LifeRecordItem ScamCombatSkillSucceedAndEscaped => Instance[(short)293];

		public static LifeRecordItem CombatSkillGetScammedSucceed => Instance[(short)294];

		public static LifeRecordItem LifeSkillBattleWin => Instance[(short)295];

		public static LifeRecordItem LifeSkillBattleLose => Instance[(short)296];

		public static LifeRecordItem ExchangeResource => Instance[(short)297];

		public static LifeRecordItem GiveResource => Instance[(short)298];

		public static LifeRecordItem PurchaseItem => Instance[(short)299];

		public static LifeRecordItem SellItem => Instance[(short)300];

		public static LifeRecordItem GiveItem => Instance[(short)301];

		public static LifeRecordItem GivePoisonousItem => Instance[(short)302];

		public static LifeRecordItem GetResourceAsGift => Instance[(short)303];

		public static LifeRecordItem GetItemAsGift => Instance[(short)304];

		public static LifeRecordItem RefusePoisonousGift => Instance[(short)305];

		public static LifeRecordItem InstructLifeSkill => Instance[(short)306];

		public static LifeRecordItem InstructCombatSkill => Instance[(short)307];

		public static LifeRecordItem LearnLifeSkillWithInstructionSucceed => Instance[(short)308];

		public static LifeRecordItem LearnLifeSkillWithInstructionFail => Instance[(short)309];

		public static LifeRecordItem LearnCombatSkillWithInstructionSucceed => Instance[(short)310];

		public static LifeRecordItem LearnCombatSkillWithInstructionFail => Instance[(short)311];

		public static LifeRecordItem InviteToDrinkSucceed => Instance[(short)312];

		public static LifeRecordItem InviteToDrinkFail => Instance[(short)313];

		public static LifeRecordItem SellSucceed => Instance[(short)314];

		public static LifeRecordItem SellFail => Instance[(short)315];

		public static LifeRecordItem CureSucceed => Instance[(short)316];

		public static LifeRecordItem RepairItemSucceed => Instance[(short)317];

		public static LifeRecordItem BarbSucceed => Instance[(short)318];

		public static LifeRecordItem BarbMistake => Instance[(short)319];

		public static LifeRecordItem BarbFail => Instance[(short)320];

		public static LifeRecordItem AskForMoneySucceed => Instance[(short)321];

		public static LifeRecordItem AskForMoneyFail => Instance[(short)322];

		public static LifeRecordItem EntertainWithMusic => Instance[(short)323];

		public static LifeRecordItem EntertainWithChess => Instance[(short)324];

		public static LifeRecordItem EntertainWithPoem => Instance[(short)325];

		public static LifeRecordItem EntertainWithPainting => Instance[(short)326];

		public static LifeRecordItem AcceptInviteToDrink => Instance[(short)327];

		public static LifeRecordItem RefuseInviteToDrink => Instance[(short)328];

		public static LifeRecordItem AcceptSell => Instance[(short)329];

		public static LifeRecordItem RefuseSell => Instance[(short)330];

		public static LifeRecordItem AcceptCure => Instance[(short)331];

		public static LifeRecordItem AcceptRepairItem => Instance[(short)332];

		public static LifeRecordItem GetBarbSucceed => Instance[(short)333];

		public static LifeRecordItem GetBarbMistake => Instance[(short)334];

		public static LifeRecordItem GetBarbFail => Instance[(short)335];

		public static LifeRecordItem AcceptAskForMoney => Instance[(short)336];

		public static LifeRecordItem RefuseAskForMoney => Instance[(short)337];

		public static LifeRecordItem AcceptEntertainWithMusic => Instance[(short)338];

		public static LifeRecordItem AcceptEntertainWithChess => Instance[(short)339];

		public static LifeRecordItem AcceptEntertainWithPoem => Instance[(short)340];

		public static LifeRecordItem AcceptEntertainWithPainting => Instance[(short)341];

		public static LifeRecordItem MakeItem => Instance[(short)342];

		public static LifeRecordItem TaoismAwakeningSucceed => Instance[(short)343];

		public static LifeRecordItem TaoismAwakeningFail => Instance[(short)344];

		public static LifeRecordItem BuddismAwakeningSucceed => Instance[(short)345];

		public static LifeRecordItem BuddismAwakeningFail => Instance[(short)346];

		public static LifeRecordItem TaoismGetAwakenedSucceed => Instance[(short)347];

		public static LifeRecordItem TaoismGetAwakenedFail => Instance[(short)348];

		public static LifeRecordItem BuddismGetAwakenedSucceed => Instance[(short)349];

		public static LifeRecordItem BuddismGetAwakenedFail => Instance[(short)350];

		public static LifeRecordItem CollectTeaWineSucceed => Instance[(short)351];

		public static LifeRecordItem CollectTeaWineFail => Instance[(short)352];

		public static LifeRecordItem DivinationSucceed => Instance[(short)353];

		public static LifeRecordItem DivinationFail => Instance[(short)354];

		public static LifeRecordItem CricketBattleWin => Instance[(short)355];

		public static LifeRecordItem CricketBattleLose => Instance[(short)356];

		public static LifeRecordItem MakeLoveIllegal => Instance[(short)357];

		public static LifeRecordItem RapeFail => Instance[(short)358];

		public static LifeRecordItem RapeSucceed => Instance[(short)359];

		public static LifeRecordItem ReleaseKidnappedCharacter => Instance[(short)360];

		public static LifeRecordItem GetRapedFail => Instance[(short)361];

		public static LifeRecordItem GetRapedSucceed => Instance[(short)362];

		public static LifeRecordItem GetReleasedByKidnapper => Instance[(short)363];

		public static LifeRecordItem MerchantGetNewProduct => Instance[(short)364];

		public static LifeRecordItem UnexpectedResourceGain => Instance[(short)365];

		public static LifeRecordItem UnexpectedItemGain => Instance[(short)366];

		public static LifeRecordItem UnexpectedSkillBookGain => Instance[(short)367];

		public static LifeRecordItem UnexpectedHealthCure => Instance[(short)368];

		public static LifeRecordItem UnexpectedOuterInjuryCure => Instance[(short)369];

		public static LifeRecordItem UnexpectedInnerInjuryCure => Instance[(short)370];

		public static LifeRecordItem UnexpectedPoisonCure => Instance[(short)371];

		public static LifeRecordItem UnexpectedDisorderOfQiCure => Instance[(short)372];

		public static LifeRecordItem UnexpectedResourceLose => Instance[(short)373];

		public static LifeRecordItem UnexpectedItemLose => Instance[(short)374];

		public static LifeRecordItem UnexpectedSkillBookLose => Instance[(short)375];

		public static LifeRecordItem UnexpectedHealthHarm => Instance[(short)376];

		public static LifeRecordItem UnexpectedOuterInjuryHarm => Instance[(short)377];

		public static LifeRecordItem UnexpectedInnerInjuryHarm => Instance[(short)378];

		public static LifeRecordItem UnexpectedPoisonHarm => Instance[(short)379];

		public static LifeRecordItem UnexpectedDisorderOfQiHarm => Instance[(short)380];

		public static LifeRecordItem KillHereticRandomEnemy => Instance[(short)381];

		public static LifeRecordItem KillRighteousRandomEnemy => Instance[(short)382];

		public static LifeRecordItem DefeatedByHereticRandomEnemy => Instance[(short)383];

		public static LifeRecordItem DefeatedByRighteousRandomEnemy => Instance[(short)384];

		public static LifeRecordItem MonvBad => Instance[(short)385];

		public static LifeRecordItem DayueYaochangBad => Instance[(short)386];

		public static LifeRecordItem JinHuangerBad => Instance[(short)387];

		public static LifeRecordItem YiyihouBad => Instance[(short)388];

		public static LifeRecordItem WeiQiBad => Instance[(short)389];

		public static LifeRecordItem YixiangBad => Instance[(short)390];

		public static LifeRecordItem ShufangBad => Instance[(short)391];

		public static LifeRecordItem JixiBad => Instance[(short)392];

		public static LifeRecordItem MonvGood => Instance[(short)393];

		public static LifeRecordItem DayueYaochangGood => Instance[(short)394];

		public static LifeRecordItem JinHuangerGood => Instance[(short)395];

		public static LifeRecordItem YiyihouGood => Instance[(short)396];

		public static LifeRecordItem WeiQiGood => Instance[(short)397];

		public static LifeRecordItem YixiangGood => Instance[(short)398];

		public static LifeRecordItem XuefengGood => Instance[(short)399];

		public static LifeRecordItem ShufangGood => Instance[(short)400];

		public static LifeRecordItem PregnantWithSamsara0 => Instance[(short)401];

		public static LifeRecordItem PregnantWithSamsara1 => Instance[(short)402];

		public static LifeRecordItem PregnantWithSamsara2 => Instance[(short)403];

		public static LifeRecordItem PregnantWithSamsara3 => Instance[(short)404];

		public static LifeRecordItem PregnantWithSamsara4 => Instance[(short)405];

		public static LifeRecordItem PregnantWithSamsara5 => Instance[(short)406];

		public static LifeRecordItem GainAuthority => Instance[(short)407];

		public static LifeRecordItem SectPunishNormal => Instance[(short)408];

		public static LifeRecordItem SectPunishElope => Instance[(short)409];

		public static LifeRecordItem ExpelVillager => Instance[(short)410];

		public static LifeRecordItem SavedFromInfection => Instance[(short)411];

		public static LifeRecordItem ChangeGrade => Instance[(short)412];

		public static LifeRecordItem ExpelledByTaiwu => Instance[(short)413];

		public static LifeRecordItem InsteadSectPunishElope => Instance[(short)414];

		public static LifeRecordItem AvoidSectPunishElope => Instance[(short)415];

		public static LifeRecordItem JoinJoustForSpouse => Instance[(short)416];

		public static LifeRecordItem GetHusbandByJoustForSpouse => Instance[(short)417];

		public static LifeRecordItem GetWifeByJoustForSpouse => Instance[(short)418];

		public static LifeRecordItem NoHusbandByJoustForSpouse => Instance[(short)419];

		public static LifeRecordItem SectCompetitionBeWinner => Instance[(short)420];

		public static LifeRecordItem SectCompetitionBeParticipant => Instance[(short)421];

		public static LifeRecordItem SectCompetitionBeHost => Instance[(short)422];

		public static LifeRecordItem WulinConferenceBeParticipant => Instance[(short)423];

		public static LifeRecordItem WulinConferenceBeWinner => Instance[(short)424];

		public static LifeRecordItem WulinConferenceBeWinnerButTaiwu => Instance[(short)425];

		public static LifeRecordItem WulinConferenceBeHost => Instance[(short)426];

		public static LifeRecordItem WulinConferenceBeKilledByYufu => Instance[(short)427];

		public static LifeRecordItem WulinConferenceDonation => Instance[(short)428];

		public static LifeRecordItem BeAttackedAndDieByWuYingLing => Instance[(short)429];

		public static LifeRecordItem NaturalDisasterGiveDeath => Instance[(short)430];

		public static LifeRecordItem NaturalDisasterHappen => Instance[(short)431];

		public static LifeRecordItem NaturalDisasterButSurvive => Instance[(short)432];

		public static LifeRecordItem NormalInformationChangeLovingItemSubType => Instance[(short)433];

		public static LifeRecordItem NormalInformationChangeHatingItemSubType => Instance[(short)434];

		public static LifeRecordItem NormalInformationChangeIdealSect => Instance[(short)435];

		public static LifeRecordItem NormalInformationChangeBaseMorality => Instance[(short)436];

		public static LifeRecordItem NormalInformationChangeLifeSkillTypeInterest => Instance[(short)437];

		public static LifeRecordItem RobGraveEncounterSkeleton => Instance[(short)438];

		public static LifeRecordItem RobGraveFailed => Instance[(short)439];

		public static LifeRecordItem SectPunishLevelLowest => Instance[(short)440];

		public static LifeRecordItem PrincipalSectPunishLevelMiddle => Instance[(short)441];

		public static LifeRecordItem PrincipalSectPunishLevelHighest => Instance[(short)442];

		public static LifeRecordItem NonPrincipalSectPunishLevelLowest => Instance[(short)443];

		public static LifeRecordItem NonPrincipalSectPunishLevelHighest => Instance[(short)444];

		public static LifeRecordItem BecomeSwornSiblingByThreatened => Instance[(short)445];

		public static LifeRecordItem MarriedByThreatened => Instance[(short)446];

		public static LifeRecordItem GetAdoptedFatherByThreatened => Instance[(short)447];

		public static LifeRecordItem GetAdoptedMotherByThreatened => Instance[(short)448];

		public static LifeRecordItem GetAdoptedSonByThreatened => Instance[(short)449];

		public static LifeRecordItem GetAdoptedDaughterByThreatened => Instance[(short)450];

		public static LifeRecordItem AddMentorByThreatened => Instance[(short)451];

		public static LifeRecordItem SeverSwornSiblingByThreatened => Instance[(short)452];

		public static LifeRecordItem DivorceByThreatened => Instance[(short)453];

		public static LifeRecordItem SeverMentorByThreatened => Instance[(short)454];

		public static LifeRecordItem SeverAdoptiveFatherByThreatened => Instance[(short)455];

		public static LifeRecordItem SeverAdoptiveMotherByThreatened => Instance[(short)456];

		public static LifeRecordItem SeverAdoptiveSonByThreatened => Instance[(short)457];

		public static LifeRecordItem SeverAdoptiveDaughterByThreatened => Instance[(short)458];

		public static LifeRecordItem GetThreatenedAdoptiveFather => Instance[(short)459];

		public static LifeRecordItem GetThreatenedAdoptiveMother => Instance[(short)460];

		public static LifeRecordItem GetThreatenedAdoptiveSon => Instance[(short)461];

		public static LifeRecordItem GetThreatenedAdoptiveDaughter => Instance[(short)462];

		public static LifeRecordItem ApproveTaiwuByThreatened => Instance[(short)463];

		public static LifeRecordItem FourSeasonsAdventureBeParticipant => Instance[(short)464];

		public static LifeRecordItem FourSeasonsAdventureBeWinner => Instance[(short)465];

		public static LifeRecordItem EndAdored => Instance[(short)466];

		public static LifeRecordItem GetMentor => Instance[(short)467];

		public static LifeRecordItem GetMentee => Instance[(short)468];

		public static LifeRecordItem SeverAdoptiveParent => Instance[(short)469];

		public static LifeRecordItem SeverAdoptiveChild => Instance[(short)470];

		public static LifeRecordItem SeverMentor => Instance[(short)471];

		public static LifeRecordItem SeverMentee => Instance[(short)472];

		public static LifeRecordItem Divorce => Instance[(short)473];

		public static LifeRecordItem ThreatenSucceed => Instance[(short)474];

		public static LifeRecordItem AdmonishSucceed => Instance[(short)475];

		public static LifeRecordItem ChangeBehaviorTypeByAdmonishedGood => Instance[(short)476];

		public static LifeRecordItem ReduceDebtByAdmonished => Instance[(short)477];

		public static LifeRecordItem ReduceDebtByThreatened => Instance[(short)478];

		public static LifeRecordItem ChangeBehaviorTypeByAdmonishedBad => Instance[(short)479];

		public static LifeRecordItem GainLegendaryBook => Instance[(short)480];

		public static LifeRecordItem BoostedByLegendaryBooks => Instance[(short)481];

		public static LifeRecordItem ActCrazy => Instance[(short)482];

		public static LifeRecordItem LegendaryBookShocked => Instance[(short)483];

		public static LifeRecordItem LegendaryBookInsane => Instance[(short)484];

		public static LifeRecordItem LegendaryBookConsumed => Instance[(short)485];

		public static LifeRecordItem DecideToContestForLegendaryBook => Instance[(short)486];

		public static LifeRecordItem FinishContestForLegendaryBook => Instance[(short)487];

		public static LifeRecordItem LegendaryBookChallengeWin => Instance[(short)488];

		public static LifeRecordItem LegendaryBookChallengeLose => Instance[(short)489];

		public static LifeRecordItem AcceptLegendaryBookChallengeWin => Instance[(short)490];

		public static LifeRecordItem AcceptLegendaryBookChallengeLose => Instance[(short)491];

		public static LifeRecordItem AcceptLegendaryBookChallengeEscape => Instance[(short)492];

		public static LifeRecordItem LegendaryBookChallengeEscaped => Instance[(short)493];

		public static LifeRecordItem LegendaryBookChallengeSelfEscaped => Instance[(short)524];

		public static LifeRecordItem AcceptLegendaryBookChallengeEnemyEscaped => Instance[(short)525];

		public static LifeRecordItem RefuseRequestLegendaryBookChallenge => Instance[(short)494];

		public static LifeRecordItem RequestLegendaryBookChallengeFail => Instance[(short)495];

		public static LifeRecordItem AcceptRequestLegendaryBook => Instance[(short)496];

		public static LifeRecordItem RequestLegendaryBookSucceed => Instance[(short)497];

		public static LifeRecordItem RequestLegendaryBookFail => Instance[(short)498];

		public static LifeRecordItem RefuseRequestLegendaryBook => Instance[(short)499];

		public static LifeRecordItem AcceptRequestExchangeLegendaryBook => Instance[(short)500];

		public static LifeRecordItem RequestExchangeLegendaryBookSucceed => Instance[(short)501];

		public static LifeRecordItem RefuseRequestExchangeLegendaryBook => Instance[(short)502];

		public static LifeRecordItem RequestExchangeLegendaryBookFail => Instance[(short)503];

		public static LifeRecordItem GiveLegendaryBookFail => Instance[(short)504];

		public static LifeRecordItem RefuseGiveLegendaryBook => Instance[(short)505];

		public static LifeRecordItem DefeatLegendaryBookInsaneJust => Instance[(short)506];

		public static LifeRecordItem DefeatLegendaryBookInsaneKind => Instance[(short)507];

		public static LifeRecordItem DefeatLegendaryBookInsaneEven => Instance[(short)508];

		public static LifeRecordItem DefeatLegendaryBookInsaneRebel => Instance[(short)509];

		public static LifeRecordItem DefeatLegendaryBookInsaneEgoistic => Instance[(short)510];

		public static LifeRecordItem LegendaryBookInsaneDefeatedJust => Instance[(short)511];

		public static LifeRecordItem LegendaryBookInsaneDefeatedKind => Instance[(short)512];

		public static LifeRecordItem LegendaryBookInsaneDefeatedEven => Instance[(short)513];

		public static LifeRecordItem LegendaryBookInsaneDefeatedRebel => Instance[(short)514];

		public static LifeRecordItem LegendaryBookInsaneDefeatedEgoistic => Instance[(short)515];

		public static LifeRecordItem ShockedInsaneEscaped => Instance[(short)516];

		public static LifeRecordItem ReleaseShockedInsane => Instance[(short)517];

		public static LifeRecordItem UnderAttackEscaped => Instance[(short)518];

		public static LifeRecordItem ReleaseUnderAttack => Instance[(short)519];

		public static LifeRecordItem DefeatConsumed => Instance[(short)520];

		public static LifeRecordItem BeDefetedByConsumed => Instance[(short)521];

		public static LifeRecordItem AcceptRequestExchangeLegendaryBookByExp => Instance[(short)522];

		public static LifeRecordItem RequestExchangeLegendaryBookSucceedByExp => Instance[(short)523];

		public static LifeRecordItem ResignPositionToStudyLegendaryBook => Instance[(short)526];

		public static LifeRecordItem SoundOutLoverMind => Instance[(short)527];

		public static LifeRecordItem SoundOutMind => Instance[(short)528];

		public static LifeRecordItem RedeemMindSucceed => Instance[(short)529];

		public static LifeRecordItem RedeemMindFail => Instance[(short)530];

		public static LifeRecordItem AcceptRedeemMind => Instance[(short)531];

		public static LifeRecordItem RefuseRedeemMind => Instance[(short)532];

		public static LifeRecordItem FirstDateWithLover => Instance[(short)533];

		public static LifeRecordItem FirstDateWithTaiwu => Instance[(short)534];

		public static LifeRecordItem SelectLoverToken => Instance[(short)535];

		public static LifeRecordItem SelectLoverToken2 => Instance[(short)536];

		public static LifeRecordItem DateWithLover => Instance[(short)537];

		public static LifeRecordItem DateWithLover2 => Instance[(short)538];

		public static LifeRecordItem TillDeathDoUsPart => Instance[(short)539];

		public static LifeRecordItem CelebrateBirthday => Instance[(short)540];

		public static LifeRecordItem CelebrateSelfBirthday => Instance[(short)541];

		public static LifeRecordItem CelebrateAnniversary => Instance[(short)542];

		public static LifeRecordItem BeCaughtCheating => Instance[(short)543];

		public static LifeRecordItem CaughtCheating => Instance[(short)544];

		public static LifeRecordItem PregnancyWithWife => Instance[(short)545];

		public static LifeRecordItem PregnancyWithHusband => Instance[(short)546];

		public static LifeRecordItem TeaTasting => Instance[(short)547];

		public static LifeRecordItem TeaTastingLifeSkillBattleWin => Instance[(short)548];

		public static LifeRecordItem TeaTastingLifeSkillBattleLose => Instance[(short)549];

		public static LifeRecordItem TeaTastingDisorderOfQi => Instance[(short)550];

		public static LifeRecordItem WineTasting => Instance[(short)551];

		public static LifeRecordItem WineTastingLifeSkillBattleWin => Instance[(short)552];

		public static LifeRecordItem WineTastingLifeSkillBattleLose => Instance[(short)553];

		public static LifeRecordItem WineTastingDisorderOfQi => Instance[(short)554];

		public static LifeRecordItem FirstNameChanged => Instance[(short)555];

		public static LifeRecordItem LifeSkillModel => Instance[(short)556];

		public static LifeRecordItem CombatSkillModel => Instance[(short)557];

		public static LifeRecordItem PromoteReputation => Instance[(short)558];

		public static LifeRecordItem ReputationPromoted => Instance[(short)559];

		public static LifeRecordItem CapabilityCultivated => Instance[(short)560];

		public static LifeRecordItem BroughtToTaiwuByBeggars => Instance[(short)561];

		public static LifeRecordItem DiscardRevengeForCivilianSkill => Instance[(short)562];

		public static LifeRecordItem CivilianSkillDissolveResentment => Instance[(short)563];

		public static LifeRecordItem PersuadeWithdrawlFromOrganization => Instance[(short)564];

		public static LifeRecordItem WithdrawlFromOrganization => Instance[(short)565];

		public static LifeRecordItem FreeMedicalConsultation => Instance[(short)566];

		public static LifeRecordItem OfferTreasures => Instance[(short)567];

		public static LifeRecordItem ReceiveOfferedTreasures => Instance[(short)568];

		public static LifeRecordItem ForcefulPurchase => Instance[(short)569];

		public static LifeRecordItem ForcefulSale => Instance[(short)570];

		public static LifeRecordItem BegForMoney => Instance[(short)571];

		public static LifeRecordItem AbsurdlyForceToLeave => Instance[(short)572];

		public static LifeRecordItem AbsurdlyForcedToLeave => Instance[(short)573];

		public static LifeRecordItem DiagnoseWithMedicine => Instance[(short)574];

		public static LifeRecordItem DiagnosedWithMedicine => Instance[(short)575];

		public static LifeRecordItem DiagnoseWithNonMedicine => Instance[(short)576];

		public static LifeRecordItem DiagnosedWithWrongMedicine => Instance[(short)577];

		public static LifeRecordItem ExtendLifeSpan => Instance[(short)578];

		public static LifeRecordItem LifeSpanExtended => Instance[(short)579];

		public static LifeRecordItem PersuadeToBecomeMonk => Instance[(short)580];

		public static LifeRecordItem BecomeMonkPersuaded => Instance[(short)581];

		public static LifeRecordItem FailToPersuadeToBecomeMonk => Instance[(short)582];

		public static LifeRecordItem ExpiateDeadSouls => Instance[(short)583];

		public static LifeRecordItem ExociseXiangshuInfectionVictoryInCombat => Instance[(short)584];

		public static LifeRecordItem BecomeExociseXiangshuInfectionVictoryInCombat => Instance[(short)585];

		public static LifeRecordItem ExociseXiangshuInfectionVictoryInCombatDefeated => Instance[(short)604];

		public static LifeRecordItem TribulationSucceeded => Instance[(short)586];

		public static LifeRecordItem TribulationFailed => Instance[(short)587];

		public static LifeRecordItem TribulationCanceled => Instance[(short)588];

		public static LifeRecordItem TribulationContinued => Instance[(short)589];

		public static LifeRecordItem GuidingEvilToGoodSucceed => Instance[(short)590];

		public static LifeRecordItem GuidingEvilGoodSucceed => Instance[(short)591];

		public static LifeRecordItem GuidingEvilToGoodFail => Instance[(short)592];

		public static LifeRecordItem VisitBuddhismTemples => Instance[(short)593];

		public static LifeRecordItem EpiphanyThruVisitTemples => Instance[(short)594];

		public static LifeRecordItem EpiphanyThruVisitTemplesCombatSkill => Instance[(short)595];

		public static LifeRecordItem EpiphanyThruVisitTemplesLifeSkill => Instance[(short)596];

		public static LifeRecordItem EpiphanyThruVisitTemplesExperience => Instance[(short)605];

		public static LifeRecordItem DivineUnexpectedGain => Instance[(short)597];

		public static LifeRecordItem DivineUnexpectedHarm => Instance[(short)598];

		public static LifeRecordItem ExchangeFates => Instance[(short)599];

		public static LifeRecordItem BecomeExchangeFates => Instance[(short)600];

		public static LifeRecordItem ImmortalityGained => Instance[(short)601];

		public static LifeRecordItem ImmortalityLost => Instance[(short)602];

		public static LifeRecordItem ImmortalityRegained => Instance[(short)603];

		public static LifeRecordItem TaiwuReincarnation => Instance[(short)606];

		public static LifeRecordItem TaiwuReincarnationPregnancy => Instance[(short)607];

		public static LifeRecordItem MixPoisonHotRedRotten => Instance[(short)608];

		public static LifeRecordItem MixPoisonHotRottenIllusory => Instance[(short)609];

		public static LifeRecordItem MixPoisonHotRottenGloomy => Instance[(short)610];

		public static LifeRecordItem MixPoisonHotRottenCold => Instance[(short)611];

		public static LifeRecordItem MixPoisonRedRottenIllusory => Instance[(short)612];

		public static LifeRecordItem MixPoisonRedRottenGloomy => Instance[(short)613];

		public static LifeRecordItem MixPoisonRedRottenCold => Instance[(short)614];

		public static LifeRecordItem MixPoisonHotRedIllusory => Instance[(short)615];

		public static LifeRecordItem MixPoisonHotRedGloomy => Instance[(short)616];

		public static LifeRecordItem MixPoisonHotRedCold => Instance[(short)617];

		public static LifeRecordItem MixPoisonGloomyColdIllusory => Instance[(short)618];

		public static LifeRecordItem MixPoisonRottenGloomyCold => Instance[(short)619];

		public static LifeRecordItem MixPoisonHotGloomyCold => Instance[(short)620];

		public static LifeRecordItem MixPoisonRedGloomyCold => Instance[(short)621];

		public static LifeRecordItem MixPoisonRottenColdIllusory => Instance[(short)622];

		public static LifeRecordItem MixPoisonHotColdIllusory => Instance[(short)623];

		public static LifeRecordItem MixPoisonRedColdIllusory => Instance[(short)624];

		public static LifeRecordItem MixPoisonRottenGloomyIllusory => Instance[(short)625];

		public static LifeRecordItem MixPoisonHotGloomyIllusory => Instance[(short)626];

		public static LifeRecordItem MixPoisonRedGloomyIllusory => Instance[(short)627];

		public static LifeRecordItem DiggingXiangshuMinionCombatLost => Instance[(short)628];

		public static LifeRecordItem DiggingXiangshuMinionCombatWon => Instance[(short)629];

		public static LifeRecordItem SectMainStoryXuehouJixiKills => Instance[(short)630];

		public static LifeRecordItem SectMainStoryWudangTreasure => Instance[(short)631];

		public static LifeRecordItem SectMainStoryXuannvJoinOrg => Instance[(short)632];

		public static LifeRecordItem SectMainStoryYuanshanGetAbsorbed => Instance[(short)633];

		public static LifeRecordItem SectMainStoryYuanshanResistSucceed => Instance[(short)634];

		public static LifeRecordItem SectMainStoryYuanshanResistOrdinary => Instance[(short)635];

		public static LifeRecordItem SectMainStoryYuanshanResistFailed => Instance[(short)636];

		public static LifeRecordItem SectMainStoryXuehouZombieKills => Instance[(short)637];

		public static LifeRecordItem SectMainStoryShixiangSkillEnemy => Instance[(short)638];

		public static LifeRecordItem SectMainStoryWuxianMethysis0 => Instance[(short)639];

		public static LifeRecordItem SectMainStoryWuxianPoison => Instance[(short)640];

		public static LifeRecordItem SectMainStoryWuxianAssault => Instance[(short)641];

		public static LifeRecordItem SectMainStoryWuxianMethysis1 => Instance[(short)642];

		public static LifeRecordItem SectMainStoryEmeiInfighting => Instance[(short)643];

		public static LifeRecordItem SectMainStoryJieqingAssassin => Instance[(short)644];

		public static LifeRecordItem WulinConferencePraiseAndGifts => Instance[(short)645];

		public static LifeRecordItem NormalInformationChangeIdealSectNegative => Instance[(short)646];

		public static LifeRecordItem SectMainStoryXuehouJixiRescueTaiwu => Instance[(short)647];

		public static LifeRecordItem SectMainStoryRanshanJoinThreeFactionCompetetion => Instance[(short)648];

		public static LifeRecordItem SectMainStoryRanshanThreeFactionCompetetionWin => Instance[(short)649];

		public static LifeRecordItem SectMainStoryRanshanThreeFactionCompetetionLose => Instance[(short)650];

		public static LifeRecordItem GainExpByStroll => Instance[(short)651];

		public static LifeRecordItem GainExpByReadingOldBook => Instance[(short)652];

		public static LifeRecordItem PunishedAlongsideSpouse => Instance[(short)653];

		public static LifeRecordItem DecideToAdoptFoundling => Instance[(short)654];

		public static LifeRecordItem AdoptFoundlingFail => Instance[(short)655];

		public static LifeRecordItem AdoptFoundlingSucceed => Instance[(short)656];

		public static LifeRecordItem FoundlingGetAdopted => Instance[(short)657];

		public static LifeRecordItem ClaimFoundlingSucceed => Instance[(short)658];

		public static LifeRecordItem FoundlingGetClaimed => Instance[(short)659];

		public static LifeRecordItem SectMainStoryWudangVillagerKilled => Instance[(short)660];

		public static LifeRecordItem SectMainStoryShixiangFallIll => Instance[(short)661];

		public static LifeRecordItem KillAnimal => Instance[(short)662];

		public static LifeRecordItem DefeatedByAnimal => Instance[(short)663];

		public static LifeRecordItem EnterEnemyNest => Instance[(short)664];

		public static LifeRecordItem DieFromEnemyNest => Instance[(short)665];

		public static LifeRecordItem EscapeFromEnemyNest => Instance[(short)666];

		public static LifeRecordItem GetSecretSpreadInVeryHighProbability => Instance[(short)667];

		public static LifeRecordItem GetSecretSpreadInHighProbability => Instance[(short)668];

		public static LifeRecordItem GetSecretSpreadInLowProbability => Instance[(short)669];

		public static LifeRecordItem GetSecretSpreadInVeryLowProbability => Instance[(short)670];

		public static LifeRecordItem SpreadSecretFail => Instance[(short)671];

		public static LifeRecordItem SpreadSecretSuccess => Instance[(short)672];

		public static LifeRecordItem HeardSecretSpreadInVeryHighProbability => Instance[(short)673];

		public static LifeRecordItem HeardSecretSpreadInHighProbability => Instance[(short)674];

		public static LifeRecordItem HeardSecretSpreadInLowProbability => Instance[(short)675];

		public static LifeRecordItem HeardSecretSpreadInVeryLowProbability => Instance[(short)676];

		public static LifeRecordItem RequestKeepSecretFail => Instance[(short)677];

		public static LifeRecordItem RequestKeepSecretSuccess => Instance[(short)678];

		public static LifeRecordItem BeRequestedToKeepSecret => Instance[(short)679];

		public static LifeRecordItem ThreadNeedleMatchFail => Instance[(short)680];

		public static LifeRecordItem ThreadNeedleSeparateFail => Instance[(short)681];

		public static LifeRecordItem ThreadNeedleMatchSuccess => Instance[(short)682];

		public static LifeRecordItem ThreadNeedleSeparateSuccess => Instance[(short)683];

		public static LifeRecordItem ThreadNeedleBeMatched1 => Instance[(short)684];

		public static LifeRecordItem ThreadNeedleBeSeparated1 => Instance[(short)685];

		public static LifeRecordItem ThreadNeedleBeMatched2 => Instance[(short)686];

		public static LifeRecordItem ThreadNeedleBeSeparated2 => Instance[(short)687];

		public static LifeRecordItem SpreadSecretKnown => Instance[(short)688];

		public static LifeRecordItem SectMainStoryXuannvBirthOfMirrorCreatedImposture => Instance[(short)689];

		public static LifeRecordItem EscapeFromEnemyNestBySelf => Instance[(short)690];

		public static LifeRecordItem SaveFromInfection => Instance[(short)691];

		public static LifeRecordItem SaveFromEnemyNest => Instance[(short)692];

		public static LifeRecordItem SaveFromEnemyNestFailed => Instance[(short)695];

		public static LifeRecordItem TameCarrierSucceed => Instance[(short)693];

		public static LifeRecordItem TameCarrierFail => Instance[(short)694];

		public static LifeRecordItem ReleaseCarrier => Instance[(short)696];

		public static LifeRecordItem DLCLoongRidingEffectQiuniuAudience => Instance[(short)697];

		public static LifeRecordItem DLCLoongRidingEffectQiuniu => Instance[(short)698];

		public static LifeRecordItem DLCLoongRidingEffectYazi => Instance[(short)699];

		public static LifeRecordItem DLCLoongRidingEffectChaofeng => Instance[(short)700];

		public static LifeRecordItem DLCLoongRidingEffectPulao => Instance[(short)701];

		public static LifeRecordItem DLCLoongRidingEffectSuanni => Instance[(short)702];

		public static LifeRecordItem DLCLoongRidingEffectBaxia => Instance[(short)703];

		public static LifeRecordItem DLCLoongRidingEffectBian => Instance[(short)704];

		public static LifeRecordItem DLCLoongRidingEffectFuxi => Instance[(short)705];

		public static LifeRecordItem DLCLoongRidingEffectChiwen => Instance[(short)706];

		public static LifeRecordItem DefeatLoong => Instance[(short)707];

		public static LifeRecordItem DefeatedByLoong => Instance[(short)708];

		public static LifeRecordItem DLCLoongRidingEffectYazi2 => Instance[(short)709];

		public static LifeRecordItem DieFromAge => Instance[(short)710];

		public static LifeRecordItem DieFromPoorHealth => Instance[(short)711];

		public static LifeRecordItem KilledInPublic => Instance[(short)713];

		public static LifeRecordItem KilledInPrivate => Instance[(short)712];

		public static LifeRecordItem KilledAfterXiangshuInfected => Instance[(short)714];

		public static LifeRecordItem Assassinated => Instance[(short)715];

		public static LifeRecordItem KilledByXiangshu => Instance[(short)716];

		public static LifeRecordItem PurchaseItem1 => Instance[(short)717];

		public static LifeRecordItem SellItem1 => Instance[(short)718];

		public static LifeRecordItem CleanBodyReincarnationSuccess => Instance[(short)719];

		public static LifeRecordItem CleanBodyReincarnationFail => Instance[(short)720];

		public static LifeRecordItem EvilBodyReincarnationSuccess => Instance[(short)721];

		public static LifeRecordItem EvilBodyReincarnationFail => Instance[(short)722];

		public static LifeRecordItem WugKingForestSpiritBecomeEnemy => Instance[(short)723];

		public static LifeRecordItem SecretMakeEnemy => Instance[(short)724];

		public static LifeRecordItem SecretBeMadeEnemy => Instance[(short)725];

		public static LifeRecordItem CleanBodyDefeatAnimal => Instance[(short)726];

		public static LifeRecordItem EvilBodyDefeatAnimal => Instance[(short)727];

		public static LifeRecordItem CleanBodyDefeatHereticRandomEnemy => Instance[(short)728];

		public static LifeRecordItem EvilBodyDefeatHereticRandomEnemy => Instance[(short)729];

		public static LifeRecordItem CleanBodyDefeatRighteousRandomEnemy => Instance[(short)730];

		public static LifeRecordItem EvilBodyDefeatRighteousRandomEnemy => Instance[(short)731];

		public static LifeRecordItem WuxianParanoiaAdded => Instance[(short)732];

		public static LifeRecordItem WuxianParanoiaAttack => Instance[(short)733];

		public static LifeRecordItem WuxianParanoiaErased => Instance[(short)734];

		public static LifeRecordItem WugKingRedEyeLoseItem => Instance[(short)735];

		public static LifeRecordItem WugForestSpiritReduceFavorability => Instance[(short)736];

		public static LifeRecordItem WugKingForestSpiritBeBecomeEnemy => Instance[(short)737];

		public static LifeRecordItem WugKingBlackBloodChangeDisorderOfQi => Instance[(short)738];

		public static LifeRecordItem WugDevilInsideXiangshuInfection => Instance[(short)739];

		public static LifeRecordItem WugCorpseWormChangeHealth => Instance[(short)740];

		public static LifeRecordItem WugKingIceSilkwormLoseNeili => Instance[(short)741];

		public static LifeRecordItem WugKingGoldenSilkwormEatGrownWug => Instance[(short)742];

		public static LifeRecordItem WugAzureMarrowAddPoison => Instance[(short)743];

		public static LifeRecordItem WugAzureMarrowAddWug => Instance[(short)744];

		public static LifeRecordItem WugAzureMarrowBeAddWug => Instance[(short)745];

		public static LifeRecordItem WuxianParanoiaErased2 => Instance[(short)746];

		public static LifeRecordItem WuxianDecreasedMood => Instance[(short)747];

		public static LifeRecordItem WuxianDecreasedFavorability => Instance[(short)748];

		public static LifeRecordItem WuxianQiDecline => Instance[(short)749];

		public static LifeRecordItem WuxianPoisoning => Instance[(short)750];

		public static LifeRecordItem WuxianLoseItem => Instance[(short)751];

		public static LifeRecordItem WugDevilInsideChangeHappiness => Instance[(short)752];

		public static LifeRecordItem WugRedEyeChangeToGrown => Instance[(short)753];

		public static LifeRecordItem WugForestSpiritChangeToGrown => Instance[(short)754];

		public static LifeRecordItem WugBlackBloodChangeToGrown => Instance[(short)755];

		public static LifeRecordItem WugDevilInsideChangeToGrown => Instance[(short)756];

		public static LifeRecordItem WugCorpseWormChangeToGrown => Instance[(short)757];

		public static LifeRecordItem WugCorpseWormBeChangeToGrown => Instance[(short)758];

		public static LifeRecordItem WugIceSilkwormChangeToGrown => Instance[(short)759];

		public static LifeRecordItem WugGoldenSilkwormChangeToGrown => Instance[(short)760];

		public static LifeRecordItem WugAzureMarrowChangeToGrown => Instance[(short)761];

		public static LifeRecordItem WugAzureMarrowBeChangeToGrown => Instance[(short)762];

		public static LifeRecordItem ManageLearnLifeSkillSuccess => Instance[(short)763];

		public static LifeRecordItem ManageLearnCombatSkillSuccess => Instance[(short)764];

		public static LifeRecordItem ManageLearnLifeSkillFail => Instance[(short)765];

		public static LifeRecordItem ManageLearnCombatSkillFail => Instance[(short)766];

		public static LifeRecordItem ManageLifeSkillAbilityUp => Instance[(short)767];

		public static LifeRecordItem ManageCombatSkillAbilityUp => Instance[(short)768];

		public static LifeRecordItem SmallVillagerXiangshuCompletelyInfected => Instance[(short)769];

		public static LifeRecordItem SmallVillagerSavedFromInfection => Instance[(short)770];

		public static LifeRecordItem SmallVillagerSaveFromInfection => Instance[(short)771];

		public static LifeRecordItem StorageResourceToTreasury => Instance[(short)772];

		public static LifeRecordItem StorageItemToTreasury => Instance[(short)773];

		public static LifeRecordItem TakeResourceFromTreasury => Instance[(short)774];

		public static LifeRecordItem TakeItemFromTreasury => Instance[(short)775];

		public static LifeRecordItem TaiwuStorageResourceToTreasury => Instance[(short)822];

		public static LifeRecordItem TaiwuStorageItemToTreasury => Instance[(short)823];

		public static LifeRecordItem TaiwuTakeResourceFromTreasury => Instance[(short)824];

		public static LifeRecordItem TaiwuTakeItemFromTreasury => Instance[(short)825];

		public static LifeRecordItem DecideToGuardTreasury => Instance[(short)776];

		public static LifeRecordItem FinishGuardingTreasury => Instance[(short)777];

		public static LifeRecordItem IntrudeTreasuryCancelSupportMakeEnemy => Instance[(short)778];

		public static LifeRecordItem IntrudeTreasuryBeCancelSupportMakeEnemy => Instance[(short)779];

		public static LifeRecordItem IntrudeTreasuryCancelSupport => Instance[(short)780];

		public static LifeRecordItem IntrudeTreasuryBeCancelSupport => Instance[(short)781];

		public static LifeRecordItem IntrudeTreasuryMakeEnemyOthers => Instance[(short)782];

		public static LifeRecordItem IntrudeTreasuryBeMakeEnemyOthers => Instance[(short)783];

		public static LifeRecordItem IntrudeTreasuryLostMorale => Instance[(short)784];

		public static LifeRecordItem IntrudeTreasuryBeLostMorale => Instance[(short)785];

		public static LifeRecordItem IntrudeTreasuryBeLostMorale2 => Instance[(short)786];

		public static LifeRecordItem PlunderTreasuryCancelSupportMakeEnemy => Instance[(short)787];

		public static LifeRecordItem PlunderTreasuryBeCancelSupportMakeEnemy => Instance[(short)788];

		public static LifeRecordItem PlunderTreasuryCancelSupport => Instance[(short)789];

		public static LifeRecordItem PlunderTreasuryBeCancelSupport => Instance[(short)790];

		public static LifeRecordItem PlunderTreasuryMakeEnemyOthers => Instance[(short)791];

		public static LifeRecordItem PlunderTreasuryBeMakeEnemyOthers => Instance[(short)792];

		public static LifeRecordItem PlunderTreasuryLostMorale => Instance[(short)793];

		public static LifeRecordItem PlunderTreasuryBeLostMorale => Instance[(short)794];

		public static LifeRecordItem PlunderTreasuryBeLostMorale2 => Instance[(short)795];

		public static LifeRecordItem DonateTreasuryProvideSupport => Instance[(short)796];

		public static LifeRecordItem DonateTreasuryBeProvideSupport => Instance[(short)797];

		public static LifeRecordItem DonateTreasuryGetMorale => Instance[(short)798];

		public static LifeRecordItem DonateTreasuryBeGetMorale => Instance[(short)799];

		public static LifeRecordItem DonateTreasuryGetMorale2 => Instance[(short)800];

		public static LifeRecordItem TreasuryDistributeResource => Instance[(short)826];

		public static LifeRecordItem TreasuryDistributeItem => Instance[(short)801];

		public static LifeRecordItem PoisonEnemyFail12 => Instance[(short)802];

		public static LifeRecordItem PoisonEnemyFail22 => Instance[(short)803];

		public static LifeRecordItem PoisonEnemyFail32 => Instance[(short)804];

		public static LifeRecordItem PoisonEnemyFail42 => Instance[(short)805];

		public static LifeRecordItem PoisonEnemySucceed2 => Instance[(short)806];

		public static LifeRecordItem PoisonEnemySucceedAndEscaped2 => Instance[(short)807];

		public static LifeRecordItem GetPoisonedByEnemySucceed2 => Instance[(short)808];

		public static LifeRecordItem PlotHarmEnemyFail12 => Instance[(short)809];

		public static LifeRecordItem PlotHarmEnemyFail22 => Instance[(short)810];

		public static LifeRecordItem PlotHarmEnemyFail32 => Instance[(short)811];

		public static LifeRecordItem PlotHarmEnemyFail42 => Instance[(short)812];

		public static LifeRecordItem PlotHarmEnemySucceed2 => Instance[(short)813];

		public static LifeRecordItem PlotHarmEnemySucceedAndEscaped2 => Instance[(short)814];

		public static LifeRecordItem GetPlottedAgainstSucceed2 => Instance[(short)815];

		public static LifeRecordItem SectMainStoryBaihuaManiaLow => Instance[(short)816];

		public static LifeRecordItem SectMainStoryBaihuaManiaHigh => Instance[(short)817];

		public static LifeRecordItem SectMainStoryBaihuaManiaAttack => Instance[(short)818];

		public static LifeRecordItem SectMainStoryBaihuaManiaAttacked => Instance[(short)819];

		public static LifeRecordItem SectMainStoryBaihuaManiaCure => Instance[(short)820];

		public static LifeRecordItem SectMainStoryBaihuaManiaCured => Instance[(short)821];

		public static LifeRecordItem GiveUpLegendaryBookSuccessHuaJu => Instance[(short)827];

		public static LifeRecordItem GiveUpLegendaryBookSuccessXuanZhi => Instance[(short)828];

		public static LifeRecordItem GiveUpLegendaryBookSuccessYingJiao => Instance[(short)829];

		public static LifeRecordItem SecretMakeEnemy2 => Instance[(short)830];

		public static LifeRecordItem SecretBeMadeEnemy2 => Instance[(short)831];

		public static LifeRecordItem DecideToHuntFugitive => Instance[(short)832];

		public static LifeRecordItem FinishHuntFugitive => Instance[(short)833];

		public static LifeRecordItem DecideToEscapePunishment => Instance[(short)834];

		public static LifeRecordItem FinishEscapePunishment => Instance[(short)835];

		public static LifeRecordItem DecideToSeekAsylum => Instance[(short)836];

		public static LifeRecordItem FinishSeekAsylum => Instance[(short)837];

		public static LifeRecordItem SeekAsylumSuccess => Instance[(short)838];

		public static LifeRecordItem DecideToEscortPrisoner => Instance[(short)839];

		public static LifeRecordItem EscortPrisonerSucceed => Instance[(short)840];

		public static LifeRecordItem ImprisonedShaoLin => Instance[(short)841];

		public static LifeRecordItem ImprisonedEmei1 => Instance[(short)842];

		public static LifeRecordItem ImprisonedEmei2 => Instance[(short)843];

		public static LifeRecordItem ImprisonedBaihua => Instance[(short)844];

		public static LifeRecordItem ImprisonedWudang => Instance[(short)845];

		public static LifeRecordItem ImprisonedYuanshan => Instance[(short)846];

		public static LifeRecordItem ImprisonedShingXiang => Instance[(short)847];

		public static LifeRecordItem ImprisonedRanShan => Instance[(short)848];

		public static LifeRecordItem ImprisonedXuanNv => Instance[(short)849];

		public static LifeRecordItem ImprisonedZhuJian => Instance[(short)850];

		public static LifeRecordItem ImprisonedKongSang => Instance[(short)851];

		public static LifeRecordItem ImprisonedJinGang => Instance[(short)852];

		public static LifeRecordItem ImprisonedWuXian => Instance[(short)853];

		public static LifeRecordItem ImprisonedJieQing1 => Instance[(short)854];

		public static LifeRecordItem ImprisonedJieQing2 => Instance[(short)855];

		public static LifeRecordItem ImprisonedFuLong => Instance[(short)856];

		public static LifeRecordItem ImprisonedXueHou => Instance[(short)857];

		public static LifeRecordItem IntrudePrisonCancelSupportMakeEnemyNpc => Instance[(short)858];

		public static LifeRecordItem IntrudePrisonCancelSupportMakeEnemyTaiwu => Instance[(short)859];

		public static LifeRecordItem IntrudePrisonCancelSupportNpc => Instance[(short)860];

		public static LifeRecordItem IntrudePrisonCancelSupportTaiwu => Instance[(short)861];

		public static LifeRecordItem IntrudePrisonMakeEnemyOthersNpc => Instance[(short)862];

		public static LifeRecordItem IntrudePrisonMakeEnemyOthersTaiwu => Instance[(short)863];

		public static LifeRecordItem RequestTheReleaseOfTheCriminalNpc => Instance[(short)864];

		public static LifeRecordItem RequestTheReleaseOfTheCriminalTaiwu => Instance[(short)865];

		public static LifeRecordItem ImprisonedXiangshuInfectedSupportIncreaseAndFavorabilityNpc => Instance[(short)866];

		public static LifeRecordItem ImprisonedXiangshuInfectedSupportIncreaseAndFavorabilityTaiwu => Instance[(short)867];

		public static LifeRecordItem ImprisonedXiangshuInfectedIncreaseFavorabilityNpc => Instance[(short)868];

		public static LifeRecordItem ImprisonedXiangshuInfectedIncreaseFavorabilityTaiwu => Instance[(short)869];

		public static LifeRecordItem ImprisonedXiangshuInfectedNpc => Instance[(short)870];

		public static LifeRecordItem ImprisonedXiangshuInfectedTaiwu => Instance[(short)871];

		public static LifeRecordItem RobbedFromPrisonNpc => Instance[(short)872];

		public static LifeRecordItem PrisonBreakIntrudePrisonCancelSupportMakeEnemyNpc => Instance[(short)873];

		public static LifeRecordItem PrisonBreakIntrudePrisonCancelSupportMakeEnemyTaiwu => Instance[(short)874];

		public static LifeRecordItem PrisonBreakIntrudePrisonCancelSupportNpc => Instance[(short)875];

		public static LifeRecordItem PrisonBreakIntrudePrisonCancelSupportTaiwu => Instance[(short)876];

		public static LifeRecordItem PrisonBreakIntrudePrisonMakeEnemyOthersNpc => Instance[(short)877];

		public static LifeRecordItem PrisonBreakIntrudePrisonMakeEnemyOthersTaiwu => Instance[(short)878];

		public static LifeRecordItem ResistArrestIntrudePrisonCancelSupportMakeEnemyNpc => Instance[(short)879];

		public static LifeRecordItem ResistArresPrisonBreakIntrudePrisonCancelSupportMakeEnemyTaiwu => Instance[(short)880];

		public static LifeRecordItem ResistArresPrisonBreakIntrudePrisonCancelSupportNpc => Instance[(short)881];

		public static LifeRecordItem ResistArresPrisonBreakIntrudePrisonCancelSupportTaiwu => Instance[(short)882];

		public static LifeRecordItem ResistArresPrisonBreakIntrudePrisonMakeEnemyOthersNpc => Instance[(short)883];

		public static LifeRecordItem ResistArresPrisonBreakIntrudePrisonMakeEnemyOthersTaiwu => Instance[(short)884];

		public static LifeRecordItem ArrestFailedCaptor => Instance[(short)885];

		public static LifeRecordItem ArrestFailedCriminal => Instance[(short)886];

		public static LifeRecordItem ResistArresEngageInBattleTaiwu => Instance[(short)887];

		public static LifeRecordItem ArrestedSuccessfullyCaptor => Instance[(short)888];

		public static LifeRecordItem ArrestedSuccessfullyCriminal => Instance[(short)889];

		public static LifeRecordItem ReceiveCriminalsCaptor => Instance[(short)890];

		public static LifeRecordItem ReceiveCriminalsTaiwu => Instance[(short)891];

		public static LifeRecordItem ReceiveCriminalsCriminal => Instance[(short)892];

		public static LifeRecordItem BuyHandOverTheCriminalCaptor => Instance[(short)893];

		public static LifeRecordItem BuyHandOverTheCriminalTaiwu => Instance[(short)894];

		public static LifeRecordItem LifeSkillBattleHandOverTheCriminalCaptor => Instance[(short)895];

		public static LifeRecordItem LifeSkillBattleHandOverTheCriminalTaiwu => Instance[(short)896];

		public static LifeRecordItem LifeSkillBattleLoseHandOverTheCriminalCaptor => Instance[(short)897];

		public static LifeRecordItem LifeSkillBattleLoseHandOverTheCriminalTaiwu => Instance[(short)898];

		public static LifeRecordItem VictoryInCombatHandOverTheCriminalCaptor => Instance[(short)899];

		public static LifeRecordItem VictoryInCombatHandOverTheCriminalTaiwu => Instance[(short)900];

		public static LifeRecordItem FailureInCombatHandOverTheCriminalCaptor => Instance[(short)901];

		public static LifeRecordItem FailureInCombatHandOverTheCriminalTaiwu => Instance[(short)902];

		public static LifeRecordItem SectMainStoryFulongFightSucceed => Instance[(short)903];

		public static LifeRecordItem SectMainStoryFulongFightFail => Instance[(short)904];

		public static LifeRecordItem SectMainStoryFulongRobbery => Instance[(short)905];

		public static LifeRecordItem SectMainStoryFulongRobberKilledByTaiwu => Instance[(short)906];

		public static LifeRecordItem SectMainStoryFulongProtect => Instance[(short)907];

		public static LifeRecordItem HonestSectPunishLevel1 => Instance[(short)908];

		public static LifeRecordItem HonestSectPunishLevel2 => Instance[(short)909];

		public static LifeRecordItem HonestSectPunishLevel3 => Instance[(short)910];

		public static LifeRecordItem HonestSectPunishLevel4 => Instance[(short)911];

		public static LifeRecordItem HonestSectPunishLevel5 => Instance[(short)912];

		public static LifeRecordItem HonestSectPunishTogetherWithSpouseLevel5 => Instance[(short)913];

		public static LifeRecordItem ArrestedSectPunishLevel1 => Instance[(short)914];

		public static LifeRecordItem ArrestedSectPunishLevel2 => Instance[(short)915];

		public static LifeRecordItem ArrestedSectPunishLevel3 => Instance[(short)916];

		public static LifeRecordItem ArrestedSectPunishLevel4 => Instance[(short)917];

		public static LifeRecordItem ArrestedSectPunishLevel5 => Instance[(short)918];

		public static LifeRecordItem ArrestedSectPunishTogetherWithSpouseLevel5 => Instance[(short)919];

		public static LifeRecordItem BeImplicatedSectPunishLevel5 => Instance[(short)920];

		public static LifeRecordItem BeReleasedUponCompletionOfASentence => Instance[(short)921];

		public static LifeRecordItem PrisonBreak => Instance[(short)922];

		public static LifeRecordItem SendingToPrison1Taiwu => Instance[(short)923];

		public static LifeRecordItem SendingToPrison2Taiwu => Instance[(short)924];

		public static LifeRecordItem SendingToPrisonCriminal => Instance[(short)925];

		public static LifeRecordItem SentToPrisonTaiwu => Instance[(short)926];

		public static LifeRecordItem SentToPrisonCriminal => Instance[(short)927];

		public static LifeRecordItem CatchCriminalsWinTaiwu => Instance[(short)928];

		public static LifeRecordItem CatchCriminalsWinCriminal => Instance[(short)929];

		public static LifeRecordItem CatchCriminalsFailedTaiwu => Instance[(short)930];

		public static LifeRecordItem CatchCriminalsFailedCriminal => Instance[(short)931];

		public static LifeRecordItem BuyHandOverTheCriminalCaptorByExp => Instance[(short)932];

		public static LifeRecordItem BuyHandOverTheCriminalTaiwuByExp => Instance[(short)933];

		public static LifeRecordItem SendingToPrison1TaiwuByExp => Instance[(short)934];

		public static LifeRecordItem VillagerMigrateResources => Instance[(short)935];

		public static LifeRecordItem VillagerCookingIngredient => Instance[(short)936];

		public static LifeRecordItem VillagerMakingItem => Instance[(short)937];

		public static LifeRecordItem VillagerRepairItem0 => Instance[(short)938];

		public static LifeRecordItem VillagerRepairItem1 => Instance[(short)939];

		public static LifeRecordItem VillagerDisassembleItem0 => Instance[(short)940];

		public static LifeRecordItem VillagerDisassembleItem1 => Instance[(short)941];

		public static LifeRecordItem VillagerRefiningMedicine => Instance[(short)942];

		public static LifeRecordItem VillagerDetoxify0 => Instance[(short)943];

		public static LifeRecordItem VillagerDetoxify1 => Instance[(short)944];

		public static LifeRecordItem VillagerEnvenomedItem => Instance[(short)945];

		public static LifeRecordItem VillagerSoldItem => Instance[(short)946];

		public static LifeRecordItem VillagerBuyItem => Instance[(short)947];

		public static LifeRecordItem VillagerSeverEnemy => Instance[(short)948];

		public static LifeRecordItem VillagerEmotionUp => Instance[(short)949];

		public static LifeRecordItem VillagerMakeFriends => Instance[(short)950];

		public static LifeRecordItem VillagerGetMarried => Instance[(short)951];

		public static LifeRecordItem VillagerBecomeBrothers => Instance[(short)952];

		public static LifeRecordItem VillagerAdopt => Instance[(short)953];

		public static LifeRecordItem VillagerTreatment0 => Instance[(short)954];

		public static LifeRecordItem VillagerTreatment1 => Instance[(short)955];

		public static LifeRecordItem VillagerBeTreatment0 => Instance[(short)956];

		public static LifeRecordItem VillagerBeTreatment1 => Instance[(short)957];

		public static LifeRecordItem XiangshuInfectedPrisonTaiwuVillage => Instance[(short)958];

		public static LifeRecordItem XiangshuInfectedPrisonSettlement => Instance[(short)959];

		public static LifeRecordItem VillagerBeRepairItem1 => Instance[(short)960];

		public static LifeRecordItem TaiwuVillagerTakeItem => Instance[(short)961];

		public static LifeRecordItem TaiwuVillagerStorageItem => Instance[(short)962];

		public static LifeRecordItem TaiwuVillagerStorageResources => Instance[(short)963];

		public static LifeRecordItem TaiwuVillagerTakeResources => Instance[(short)964];

		public static LifeRecordItem LiteratiEntertainingUp => Instance[(short)965];

		public static LifeRecordItem LiteratiEntertainingDown => Instance[(short)966];

		public static LifeRecordItem LiteratiBuildingRelationshipUp => Instance[(short)967];

		public static LifeRecordItem LiteratiBuildingRelationshipDown => Instance[(short)968];

		public static LifeRecordItem LiteratiSpreadingInfluenceUp => Instance[(short)969];

		public static LifeRecordItem LiteratiSpreadingInfluenceDown => Instance[(short)970];

		public static LifeRecordItem SwordTombKeeperBuildingRelationshipUp => Instance[(short)971];

		public static LifeRecordItem SwordTombKeeperBuildingRelationshipDown => Instance[(short)972];

		public static LifeRecordItem SwordTombKeeperSpreadingInfluenceUp => Instance[(short)973];

		public static LifeRecordItem SwordTombKeeperSpreadingInfluenceDown => Instance[(short)974];

		public static LifeRecordItem InquireSwordTomb => Instance[(short)975];

		public static LifeRecordItem GuardingSwordTomb => Instance[(short)976];

		public static LifeRecordItem VillagerPrioritizedActions => Instance[(short)977];

		public static LifeRecordItem VillagerPrioritizedActionsStop => Instance[(short)978];

		public static LifeRecordItem EnvenomedItemOverload => Instance[(short)979];

		public static LifeRecordItem DetoxifyItemOverload => Instance[(short)980];

		public static LifeRecordItem VillagerEnvenomedItemOverload => Instance[(short)981];

		public static LifeRecordItem VillagerDetoxifyItemOverload => Instance[(short)982];

		public static LifeRecordItem VillagerCookingIngredientFailed0 => Instance[(short)983];

		public static LifeRecordItem VillagerCookingIngredientFailed1 => Instance[(short)984];

		public static LifeRecordItem VillagerMakingItemFailed0 => Instance[(short)985];

		public static LifeRecordItem VillagerMakingItemFailed1 => Instance[(short)986];

		public static LifeRecordItem VillagerRepairFailed => Instance[(short)987];

		public static LifeRecordItem VillagerDisassembleItemFailed => Instance[(short)988];

		public static LifeRecordItem VillagerRefiningMedicineFailed0 => Instance[(short)989];

		public static LifeRecordItem VillagerRefiningMedicineFailed1 => Instance[(short)990];

		public static LifeRecordItem VillagerAddPoisonToItemFailed => Instance[(short)991];

		public static LifeRecordItem VillagerDetoxItemFailed => Instance[(short)992];

		public static LifeRecordItem VillagerDistanceFailed0 => Instance[(short)993];

		public static LifeRecordItem VillagerDistanceFailed1 => Instance[(short)994];

		public static LifeRecordItem VillagerDistanceFailed2 => Instance[(short)995];

		public static LifeRecordItem VillagerAttainmentsFailed => Instance[(short)996];

		public static LifeRecordItem TaiwuPunishmentTongyong => Instance[(short)997];

		public static LifeRecordItem TaiwuPunishmentShaolin => Instance[(short)998];

		public static LifeRecordItem TaiwuPunishmentEmei => Instance[(short)999];

		public static LifeRecordItem TaiwuPunishmentBaihua => Instance[(short)1000];

		public static LifeRecordItem TaiwuPunishmentWudang => Instance[(short)1001];

		public static LifeRecordItem TaiwuPunishmentYuanshan => Instance[(short)1002];

		public static LifeRecordItem TaiwuPunishmentShingXiang => Instance[(short)1003];

		public static LifeRecordItem TaiwuPunishmentRanShan => Instance[(short)1004];

		public static LifeRecordItem TaiwuPunishmentXuanNv => Instance[(short)1005];

		public static LifeRecordItem TaiwuPunishmentZhuJian => Instance[(short)1006];

		public static LifeRecordItem TaiwuPunishmentKongSang => Instance[(short)1007];

		public static LifeRecordItem TaiwuPunishmentJinGang => Instance[(short)1008];

		public static LifeRecordItem TaiwuPunishmentWuXian => Instance[(short)1009];

		public static LifeRecordItem TaiwuPunishmentJieQing => Instance[(short)1010];

		public static LifeRecordItem TaiwuPunishmentFuLong => Instance[(short)1011];

		public static LifeRecordItem TaiwuPunishmentXueHou => Instance[(short)1012];

		public static LifeRecordItem SectPunishLevel5Expel => Instance[(short)1013];

		public static LifeRecordItem BeImplicatedSectPunishLevel5New => Instance[(short)1014];

		public static LifeRecordItem BeImplicatedSectPunishLevel5Expel => Instance[(short)1015];

		public static LifeRecordItem ResistArrestIntrudePrisonCancelSupportMakeEnemyNpcGuard => Instance[(short)1016];

		public static LifeRecordItem ResistArresPrisonBreakIntrudePrisonCancelSupportMakeEnemyTaiwuWanted => Instance[(short)1017];

		public static LifeRecordItem ResistArresPrisonBreakIntrudePrisonCancelSupportNpcGuard => Instance[(short)1018];

		public static LifeRecordItem ResistArresPrisonBreakIntrudePrisonCancelSupportTaiwuWanted => Instance[(short)1019];

		public static LifeRecordItem ResistArresPrisonBreakIntrudePrisonMakeEnemyOthersNpcGuard => Instance[(short)1020];

		public static LifeRecordItem ResistArresPrisonBreakIntrudePrisonMakeEnemyOthersTaiwuWanted => Instance[(short)1021];

		public static LifeRecordItem ForgiveForCivilianSkill => Instance[(short)1022];

		public static LifeRecordItem BeggarEatSomeoneFood => Instance[(short)1023];

		public static LifeRecordItem SomeoneFoodEatedByBeggar => Instance[(short)1024];

		public static LifeRecordItem AristocratReleasePrisoner => Instance[(short)1025];

		public static LifeRecordItem PrisonerBeReleaseByAristocrat => Instance[(short)1026];

		public static LifeRecordItem JieQingPunishmentAssassinSetOut => Instance[(short)1027];

		public static LifeRecordItem JieQingPunishmentAssassinSucceed => Instance[(short)1028];

		public static LifeRecordItem JieQingPunishmentAssassinBeSucceed => Instance[(short)1029];

		public static LifeRecordItem JieQingPunishmentAssassinFailed => Instance[(short)1030];

		public static LifeRecordItem JieQingPunishmentAssassinBeFailed => Instance[(short)1031];

		public static LifeRecordItem JieQingPunishmentAssassinGiveUp => Instance[(short)1032];

		public static LifeRecordItem ExociseXiangshuInfectionVictoryInCombatDie => Instance[(short)1033];

		public static LifeRecordItem BecomeExociseXiangshuInfectionVictoryInCombatDie => Instance[(short)1034];

		public static LifeRecordItem ArrestFailedTaiwu => Instance[(short)1035];

		public static LifeRecordItem ArrestedSuccessfullyTaiwu => Instance[(short)1036];

		public static LifeRecordItem LifeSkillBattleLoseAndTheArrestFailedCaptor => Instance[(short)1037];

		public static LifeRecordItem LifeSkillBattleWinAndAvoidArrestTaiwu => Instance[(short)1038];

		public static LifeRecordItem LifeSkillBattleWinAndSuccessfulArrestCaptor => Instance[(short)1039];

		public static LifeRecordItem LifeSkillBattleLoseAndWasArrestedTaiwu => Instance[(short)1040];

		public static LifeRecordItem FailedArrestForBriberyCaptorByAuthority => Instance[(short)1041];

		public static LifeRecordItem BribeSucceededInAvoidingArrestTaiwuByAuthority => Instance[(short)1042];

		public static LifeRecordItem FailedArrestForBriberyCaptorByExp => Instance[(short)1043];

		public static LifeRecordItem BribeSucceededInAvoidingArrestTaiwuByExp => Instance[(short)1044];

		public static LifeRecordItem FailedArrestForBriberyCaptorByMoney => Instance[(short)1045];

		public static LifeRecordItem BribeSucceededInAvoidingArrestTaiwuByMoney => Instance[(short)1046];

		public static LifeRecordItem SubmitToCaptureMeeklyTaiwu => Instance[(short)1047];

		public static LifeRecordItem SubmitToCaptureMeeklyCaptor => Instance[(short)1048];

		public static LifeRecordItem NormalInformationChangeProfession => Instance[(short)1049];

		public static LifeRecordItem FeedTheAnimal => Instance[(short)1050];

		public static LifeRecordItem ProfessionDoctorLifeTransition => Instance[(short)1051];

		public static LifeRecordItem ProfessionDoctorLifeTransitionTaiwu => Instance[(short)1052];

		public static LifeRecordItem CombatSkillKeyPointComprehensionByExp => Instance[(short)1053];

		public static LifeRecordItem CombatSkillKeyPointComprehensionByItems => Instance[(short)1054];

		public static LifeRecordItem CombatSkillKeyPointComprehensionByLoveRelationship => Instance[(short)1055];

		public static LifeRecordItem CombatSkillKeyPointComprehensionByHatredRelationship => Instance[(short)1056];

		public static LifeRecordItem SpiritualDebtKongsangPoisoned => Instance[(short)1057];

		public static LifeRecordItem MartialArtistSkill3NPCItemDropCaseA => Instance[(short)1058];

		public static LifeRecordItem MartialArtistSkill3NPCItemDropCaseB => Instance[(short)1059];

		public static LifeRecordItem SectPunishElopeSucceedJust => Instance[(short)1060];

		public static LifeRecordItem SectPunishElopeSucceedKind => Instance[(short)1061];

		public static LifeRecordItem SectPunishElopeSucceedEven => Instance[(short)1062];

		public static LifeRecordItem SectPunishElopeSucceed => Instance[(short)1063];

		public static LifeRecordItem VillagerGetRefineItem => Instance[(short)1064];

		public static LifeRecordItem VillagerUpgradeRefineItem => Instance[(short)1065];

		public static LifeRecordItem VillagerTreatmentTaiwu => Instance[(short)1066];

		public static LifeRecordItem VillagerReduceXiangshuInfect => Instance[(short)1067];

		public static LifeRecordItem VillagerEarnMoney => Instance[(short)1068];

		public static LifeRecordItem VillagerBeEarnedMoney => Instance[(short)1069];

		public static LifeRecordItem VillagerBeSoldItem => Instance[(short)1070];

		public static LifeRecordItem VillagerBePurchasedItem => Instance[(short)1071];

		public static LifeRecordItem VillagerGetMerchantFavorability => Instance[(short)1072];

		public static LifeRecordItem VillagerGetMerchantFavorabilityTaiwu => Instance[(short)1073];

		public static LifeRecordItem LiteratiBeEntertainedUp => Instance[(short)1074];

		public static LifeRecordItem LiteratiBeEntertainedDown => Instance[(short)1075];

		public static LifeRecordItem LiteratiSpreadingInfluenceCultureUp => Instance[(short)1076];

		public static LifeRecordItem LiteratiSpreadingInfluenceCultureDown => Instance[(short)1077];

		public static LifeRecordItem LiteratiSpreadingInfluenceSafetyUp => Instance[(short)1078];

		public static LifeRecordItem LiteratiSpreadingInfluenceSafetyDown => Instance[(short)1079];

		public static LifeRecordItem LiteratiConnectRelationshipUp => Instance[(short)1080];

		public static LifeRecordItem LiteratiConnectRelationshipDown => Instance[(short)1081];

		public static LifeRecordItem LiteratiConnectRelationshipUpTaiwu => Instance[(short)1082];

		public static LifeRecordItem LiteratiConnectRelationshipDownTaiwu => Instance[(short)1083];

		public static LifeRecordItem LiteratiBeConnectedRelationshipUp => Instance[(short)1084];

		public static LifeRecordItem LiteratiBeConnectedRelationshipDown => Instance[(short)1085];

		public static LifeRecordItem GuardingSwordTombXiangshuInfectUp => Instance[(short)1086];

		public static LifeRecordItem GuardingSwordTombSucceed => Instance[(short)1087];

		public static LifeRecordItem VillagerMakeEnemy => Instance[(short)1088];

		public static LifeRecordItem VillagerConfessLoveSucceed => Instance[(short)1089];

		public static LifeRecordItem OrderProduct => Instance[(short)1090];

		public static LifeRecordItem ReceiveProduct => Instance[(short)1091];

		public static LifeRecordItem BeOrderProduct => Instance[(short)1095];

		public static LifeRecordItem BeReceiveProduct => Instance[(short)1096];

		public static LifeRecordItem CaptureOrder => Instance[(short)1093];

		public static LifeRecordItem BeCaptureOrder => Instance[(short)1094];

		public static LifeRecordItem CaptureOrderIntermediator => Instance[(short)1097];

		public static LifeRecordItem OrderProductForOthers => Instance[(short)1098];

		public static LifeRecordItem BeOrderProductForOthers => Instance[(short)1099];

		public static LifeRecordItem DeliveredOrderProduct => Instance[(short)1100];

		public static LifeRecordItem BeDeliveredOrderProduct => Instance[(short)1101];

		public static LifeRecordItem AcquisitionDiscard => Instance[(short)1092];

		public static LifeRecordItem ShopBuildingBaseDevelopLifeSkill => Instance[(short)1102];

		public static LifeRecordItem ShopBuildingBaseDevelopCombatSkill => Instance[(short)1103];

		public static LifeRecordItem ShopBuildingPersonalityDevelopLifeSkill => Instance[(short)1104];

		public static LifeRecordItem ShopBuildingPersonalityDevelopCombatSkill => Instance[(short)1105];

		public static LifeRecordItem ShopBuildingLeaderDevelopLifeSkill => Instance[(short)1106];

		public static LifeRecordItem ShopBuildingLeaderDevelopCombatSkill => Instance[(short)1107];

		public static LifeRecordItem ShopBuildingLearnLifeSkill => Instance[(short)1108];

		public static LifeRecordItem ShopBuildingLearnCombatSkill => Instance[(short)1109];

		public static LifeRecordItem JoinTaiwuVillageAfterTaiwuVillageStoneClaimed => Instance[(short)1110];

		public static LifeRecordItem TaiwuVillagerFinishedReading => Instance[(short)1111];

		public static LifeRecordItem TaiwuVillagerSalaryReceived => Instance[(short)1112];

		public static LifeRecordItem ChangeGradeDrop => Instance[(short)1113];

		public static LifeRecordItem FarmerCollectMaterial => Instance[(short)1114];

		public static LifeRecordItem JoinOrganization => Instance[(short)1115];

		public static LifeRecordItem BreakAwayOrganization => Instance[(short)1116];

		public static LifeRecordItem ChangeOrganization => Instance[(short)1117];

		public static LifeRecordItem VillagerFavorabilityUp => Instance[(short)1118];

		public static LifeRecordItem VillagerFavorabilityDown => Instance[(short)1119];

		public static LifeRecordItem VillagerFavorabilityUpPerson => Instance[(short)1120];

		public static LifeRecordItem VillagerFavorabilityDownPerson => Instance[(short)1121];

		public static LifeRecordItem TeamUpProtection => Instance[(short)1122];

		public static LifeRecordItem TeamUpRescue => Instance[(short)1123];

		public static LifeRecordItem TeamUpMourn => Instance[(short)1124];

		public static LifeRecordItem TeamUpVisitFriendOrFamily => Instance[(short)1125];

		public static LifeRecordItem TeamUpFindTreasure => Instance[(short)1126];

		public static LifeRecordItem TeamUpFindSpecialMaterial => Instance[(short)1127];

		public static LifeRecordItem TeamUpTakeRevenge => Instance[(short)1128];

		public static LifeRecordItem TeamUpContestForLegendaryBook => Instance[(short)1129];

		public static LifeRecordItem TeamUpEscapeFromPrison => Instance[(short)1130];

		public static LifeRecordItem TeamUpSeekAsylum => Instance[(short)1131];

		public static LifeRecordItem GetInfected => Instance[(short)1132];

		public static LifeRecordItem DieByInfected => Instance[(short)1133];

		public static LifeRecordItem InheritLegacy => Instance[(short)1134];

		public static LifeRecordItem Banquet_1 => Instance[(short)1135];

		public static LifeRecordItem Banquet_2 => Instance[(short)1136];

		public static LifeRecordItem Banquet_3 => Instance[(short)1137];

		public static LifeRecordItem Banquet_4 => Instance[(short)1138];

		public static LifeRecordItem Banquet_5 => Instance[(short)1139];

		public static LifeRecordItem Banquet_6 => Instance[(short)1140];

		public static LifeRecordItem Banquet_7 => Instance[(short)1141];

		public static LifeRecordItem Banquet_8 => Instance[(short)1142];

		public static LifeRecordItem Banquet_9 => Instance[(short)1143];

		public static LifeRecordItem Banquet_10 => Instance[(short)1144];

		public static LifeRecordItem SectMainStoryWudangInjured => Instance[(short)1145];

		public static LifeRecordItem ExtendDarkAshTime => Instance[(short)1146];

		public static LifeRecordItem AdoreInMarriage => Instance[(short)1147];

		public static LifeRecordItem SameAreaDistantMarriage => Instance[(short)1148];

		public static LifeRecordItem SameStateDistantMarriage => Instance[(short)1149];

		public static LifeRecordItem DifferentStateDistantMarriage => Instance[(short)1150];
	}

	public static LifeRecord Instance = new LifeRecord();

	private readonly HashSet<string> RequiredFields = new HashSet<string> { "RelatedIds", "TemplateId", "Name", "Desc" };

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
		_dataArray.Add(new LifeRecordItem(0, 0, 1, new string[6] { "Location", "", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 50, -1, ELifeRecordDisplayType.Great));
		_dataArray.Add(new LifeRecordItem(1, 2, 3, new string[6] { "Location", "", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Absolute, 0, -1, ELifeRecordDisplayType.Great));
		_dataArray.Add(new LifeRecordItem(2, 4, 5, new string[6] { "Location", "", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Absolute, 0, -1, ELifeRecordDisplayType.Great));
		_dataArray.Add(new LifeRecordItem(3, 6, 7, new string[6] { "Location", "", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Absolute, 0, -1, ELifeRecordDisplayType.Relation));
		_dataArray.Add(new LifeRecordItem(4, 6, 8, new string[6] { "Character", "Location", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Absolute, 0, -1, ELifeRecordDisplayType.Relation));
		_dataArray.Add(new LifeRecordItem(5, 9, 10, new string[6] { "Character", "Settlement", "", "", "", "" }, isSourceRecord: true, new List<short> { 6 }, -30000, ELifeRecordScoreType.Absolute, 0, -1, ELifeRecordDisplayType.Relation));
		_dataArray.Add(new LifeRecordItem(6, 11, 12, new string[6] { "", "", "", "", "", "" }, isSourceRecord: false, new List<short> { 5 }, -30000, ELifeRecordScoreType.Absolute, 0, -1, ELifeRecordDisplayType.Relation));
		_dataArray.Add(new LifeRecordItem(7, 13, 14, new string[6] { "Location", "Cricket", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 90, -1, ELifeRecordDisplayType.Relation));
		_dataArray.Add(new LifeRecordItem(8, 15, 16, new string[6] { "Character", "Location", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 90, -1, ELifeRecordDisplayType.Relation));
		_dataArray.Add(new LifeRecordItem(9, 17, 18, new string[6] { "Character", "Location", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 90, -1, ELifeRecordDisplayType.Relation));
		_dataArray.Add(new LifeRecordItem(10, 15, 19, new string[6] { "Character", "Location", "Character", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 90, -1, ELifeRecordDisplayType.Relation));
		_dataArray.Add(new LifeRecordItem(11, 17, 20, new string[6] { "Character", "Location", "Character", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 90, -1, ELifeRecordDisplayType.Relation));
		_dataArray.Add(new LifeRecordItem(12, 21, 22, new string[6] { "Character", "Location", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 50, -1, ELifeRecordDisplayType.Relation));
		_dataArray.Add(new LifeRecordItem(13, 23, 24, new string[6] { "Location", "Item", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 30, -1, ELifeRecordDisplayType.Negative));
		_dataArray.Add(new LifeRecordItem(14, 25, 26, new string[6] { "Character", "Location", "", "", "", "" }, isSourceRecord: true, new List<short> { 15 }, -30000, ELifeRecordScoreType.Normal, 70, -1, ELifeRecordDisplayType.Relation));
		_dataArray.Add(new LifeRecordItem(15, 27, 28, new string[6] { "", "", "", "", "", "" }, isSourceRecord: false, new List<short> { 14 }, -30000, ELifeRecordScoreType.Normal, 30, -1, ELifeRecordDisplayType.Relation));
		_dataArray.Add(new LifeRecordItem(16, 29, 30, new string[6] { "Location", "Item", "Integer", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Calculated, -1, -1, ELifeRecordDisplayType.Study));
		_dataArray.Add(new LifeRecordItem(17, 31, 32, new string[6] { "Location", "Item", "Integer", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Calculated, -1, -1, ELifeRecordDisplayType.Study));
		_dataArray.Add(new LifeRecordItem(18, 33, 34, new string[6] { "Location", "CombatSkill", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Calculated, -1, -1, ELifeRecordDisplayType.Study));
		_dataArray.Add(new LifeRecordItem(19, 35, 36, new string[6] { "Location", "CombatSkill", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Calculated, -1, -1, ELifeRecordDisplayType.Study));
		_dataArray.Add(new LifeRecordItem(20, 37, 38, new string[6] { "Location", "CombatSkill", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Calculated, -1, -1, ELifeRecordDisplayType.Study));
		_dataArray.Add(new LifeRecordItem(21, 39, 38, new string[6] { "Location", "LifeSkill", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Calculated, -1, -1, ELifeRecordDisplayType.Study));
		_dataArray.Add(new LifeRecordItem(22, 40, 41, new string[6] { "Location", "Item", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 60, -1, ELifeRecordDisplayType.Normal));
		_dataArray.Add(new LifeRecordItem(23, 42, 43, new string[6] { "Location", "Item", "Item", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 40, -1, ELifeRecordDisplayType.Normal));
		_dataArray.Add(new LifeRecordItem(24, 44, 45, new string[6] { "Location", "Resource", "Integer", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 40, -1, ELifeRecordDisplayType.Normal));
		_dataArray.Add(new LifeRecordItem(25, 46, 47, new string[6] { "Location", "Item", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 40, -1, ELifeRecordDisplayType.Normal));
		_dataArray.Add(new LifeRecordItem(26, 48, 49, new string[6] { "Character", "Location", "", "", "", "" }, isSourceRecord: true, new List<short> { 28 }, -30000, ELifeRecordScoreType.Normal, 40, -1, ELifeRecordDisplayType.Relation));
		_dataArray.Add(new LifeRecordItem(27, 50, 51, new string[6] { "Character", "Location", "", "", "", "" }, isSourceRecord: true, new List<short> { 29 }, -30000, ELifeRecordScoreType.Normal, 60, -1, ELifeRecordDisplayType.Relation));
		_dataArray.Add(new LifeRecordItem(28, 48, 52, new string[6] { "", "", "", "", "", "" }, isSourceRecord: false, new List<short> { 26 }, -30000, ELifeRecordScoreType.Normal, 40, -1, ELifeRecordDisplayType.Relation));
		_dataArray.Add(new LifeRecordItem(29, 50, 53, new string[6] { "", "", "", "", "", "" }, isSourceRecord: false, new List<short> { 27 }, -30000, ELifeRecordScoreType.Normal, 60, -1, ELifeRecordDisplayType.Relation));
		_dataArray.Add(new LifeRecordItem(30, 54, 55, new string[6] { "Character", "Location", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 60, -1, ELifeRecordDisplayType.Relation));
		_dataArray.Add(new LifeRecordItem(31, 56, 57, new string[6] { "Character", "Location", "", "", "", "" }, isSourceRecord: true, new List<short> { 31 }, -30000, ELifeRecordScoreType.Normal, 70, -1, ELifeRecordDisplayType.Relation));
		_dataArray.Add(new LifeRecordItem(32, 58, 59, new string[6] { "Character", "Location", "", "", "", "" }, isSourceRecord: true, new List<short> { 34 }, -30000, ELifeRecordScoreType.Normal, 80, -1, ELifeRecordDisplayType.Relation));
		_dataArray.Add(new LifeRecordItem(33, 60, 61, new string[6] { "Character", "Location", "", "", "", "" }, isSourceRecord: true, new List<short> { 35 }, -30000, ELifeRecordScoreType.Normal, 40, -1, ELifeRecordDisplayType.Relation));
		_dataArray.Add(new LifeRecordItem(34, 58, 62, new string[6] { "", "", "", "", "", "" }, isSourceRecord: false, new List<short> { 32 }, -30000, ELifeRecordScoreType.Normal, 80, -1, ELifeRecordDisplayType.Relation));
		_dataArray.Add(new LifeRecordItem(35, 60, 63, new string[6] { "", "", "", "", "", "" }, isSourceRecord: false, new List<short> { 33 }, -30000, ELifeRecordScoreType.Normal, 40, -1, ELifeRecordDisplayType.Relation));
		_dataArray.Add(new LifeRecordItem(36, 64, 65, new string[6] { "Character", "Location", "", "", "", "" }, isSourceRecord: true, new List<short> { 36 }, -30000, ELifeRecordScoreType.Normal, 30, 15, ELifeRecordDisplayType.Relation));
		_dataArray.Add(new LifeRecordItem(37, 64, 66, new string[6] { "Character", "Location", "", "", "", "" }, isSourceRecord: true, new List<short> { 38 }, -30000, ELifeRecordScoreType.Normal, 30, -1, ELifeRecordDisplayType.Relation));
		_dataArray.Add(new LifeRecordItem(38, 64, 67, new string[6] { "", "", "", "", "", "" }, isSourceRecord: false, new List<short> { 37 }, -30000, ELifeRecordScoreType.Normal, 30, 17, ELifeRecordDisplayType.Relation));
		_dataArray.Add(new LifeRecordItem(39, 68, 69, new string[6] { "Character", "Location", "", "", "", "" }, isSourceRecord: true, new List<short> { 39 }, -30000, ELifeRecordScoreType.Normal, 90, -1, ELifeRecordDisplayType.Relation));
		_dataArray.Add(new LifeRecordItem(40, 70, 71, new string[6] { "Character", "Location", "", "", "", "" }, isSourceRecord: true, new List<short> { 41 }, -30000, ELifeRecordScoreType.Normal, 10, -1, ELifeRecordDisplayType.Relation));
		_dataArray.Add(new LifeRecordItem(41, 70, 72, new string[6] { "", "", "", "", "", "" }, isSourceRecord: false, new List<short> { 40 }, -30000, ELifeRecordScoreType.Normal, 30, -1, ELifeRecordDisplayType.Relation));
		_dataArray.Add(new LifeRecordItem(42, 73, 74, new string[6] { "Character", "Location", "", "", "", "" }, isSourceRecord: true, new List<short> { 42 }, -30000, ELifeRecordScoreType.Normal, 60, -1, ELifeRecordDisplayType.Relation));
		_dataArray.Add(new LifeRecordItem(43, 75, 76, new string[6] { "Character", "Location", "", "", "", "" }, isSourceRecord: true, new List<short> { 43 }, -30000, ELifeRecordScoreType.Normal, 40, 18, ELifeRecordDisplayType.Relation));
		_dataArray.Add(new LifeRecordItem(44, 77, 78, new string[6] { "Character", "Location", "", "", "", "" }, isSourceRecord: true, new List<short> { 44 }, -30000, ELifeRecordScoreType.Normal, 70, -1, ELifeRecordDisplayType.Relation));
		_dataArray.Add(new LifeRecordItem(45, 79, 80, new string[6] { "Character", "Location", "", "", "", "" }, isSourceRecord: true, new List<short> { 45 }, -30000, ELifeRecordScoreType.Normal, 30, 14, ELifeRecordDisplayType.Relation));
		_dataArray.Add(new LifeRecordItem(46, 81, 82, new string[6] { "Character", "Location", "", "", "", "" }, isSourceRecord: true, new List<short> { 48, 49 }, -30000, ELifeRecordScoreType.Normal, 70, -1, ELifeRecordDisplayType.Relation));
		_dataArray.Add(new LifeRecordItem(47, 83, 84, new string[6] { "Character", "Location", "", "", "", "" }, isSourceRecord: true, new List<short> { 48, 49 }, -30000, ELifeRecordScoreType.Normal, 70, -1, ELifeRecordDisplayType.Relation));
		_dataArray.Add(new LifeRecordItem(48, 85, 86, new string[6] { "Character", "Location", "", "", "", "" }, isSourceRecord: true, new List<short> { 46, 47 }, -30000, ELifeRecordScoreType.Normal, 70, -1, ELifeRecordDisplayType.Relation));
		_dataArray.Add(new LifeRecordItem(49, 87, 88, new string[6] { "Character", "Location", "", "", "", "" }, isSourceRecord: true, new List<short> { 46, 47 }, -30000, ELifeRecordScoreType.Normal, 70, -1, ELifeRecordDisplayType.Relation));
		_dataArray.Add(new LifeRecordItem(50, 89, 90, new string[6] { "Location", "", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 70, -1, ELifeRecordDisplayType.Relation));
		_dataArray.Add(new LifeRecordItem(51, 91, 92, new string[6] { "Character", "Location", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 50, -1, ELifeRecordDisplayType.Relation));
		_dataArray.Add(new LifeRecordItem(52, 93, 94, new string[6] { "Character", "Location", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 50, -1, ELifeRecordDisplayType.Relation));
		_dataArray.Add(new LifeRecordItem(53, 95, 96, new string[6] { "Character", "Location", "", "", "", "" }, isSourceRecord: true, new List<short> { 55 }, -30000, ELifeRecordScoreType.Normal, 60, -1, ELifeRecordDisplayType.Relation));
		_dataArray.Add(new LifeRecordItem(54, 97, 98, new string[6] { "Character", "Location", "", "", "", "" }, isSourceRecord: true, new List<short> { 56 }, -30000, ELifeRecordScoreType.Normal, 40, -1, ELifeRecordDisplayType.Relation));
		_dataArray.Add(new LifeRecordItem(55, 99, 100, new string[6] { "", "", "", "", "", "" }, isSourceRecord: false, new List<short> { 53 }, -30000, ELifeRecordScoreType.Normal, 60, -1, ELifeRecordDisplayType.Relation));
		_dataArray.Add(new LifeRecordItem(56, 101, 102, new string[6] { "", "", "", "", "", "" }, isSourceRecord: false, new List<short> { 54 }, -30000, ELifeRecordScoreType.Normal, 50, -1, ELifeRecordDisplayType.Relation));
		_dataArray.Add(new LifeRecordItem(57, 103, 104, new string[6] { "Location", "Settlement", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 50, -1, ELifeRecordDisplayType.Relation));
		_dataArray.Add(new LifeRecordItem(58, 105, 106, new string[6] { "Location", "", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 50, -1, ELifeRecordDisplayType.Relation));
		_dataArray.Add(new LifeRecordItem(59, 107, 108, new string[6] { "Character", "Location", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 50, -1, ELifeRecordDisplayType.Relation));
	}

	private void CreateItems1()
	{
		_dataArray.Add(new LifeRecordItem(60, 109, 110, new string[6] { "Character", "Location", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 50, -1, ELifeRecordDisplayType.Relation));
		_dataArray.Add(new LifeRecordItem(61, 111, 112, new string[6] { "Character", "Location", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 50, -1, ELifeRecordDisplayType.Normal));
		_dataArray.Add(new LifeRecordItem(62, 113, 114, new string[6] { "Character", "Location", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 50, -1, ELifeRecordDisplayType.Normal));
		_dataArray.Add(new LifeRecordItem(63, 115, 116, new string[6] { "Location", "Location", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 50, -1, ELifeRecordDisplayType.Normal));
		_dataArray.Add(new LifeRecordItem(64, 117, 118, new string[6] { "Location", "Location", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 50, -1, ELifeRecordDisplayType.Normal));
		_dataArray.Add(new LifeRecordItem(65, 119, 120, new string[6] { "Character", "Location", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 50, -1, ELifeRecordDisplayType.Relation));
		_dataArray.Add(new LifeRecordItem(66, 121, 122, new string[6] { "Location", "Adventure", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 50, -1, ELifeRecordDisplayType.Normal));
		_dataArray.Add(new LifeRecordItem(67, 123, 124, new string[6] { "Settlement", "", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 30, -1, ELifeRecordDisplayType.Relation));
		_dataArray.Add(new LifeRecordItem(68, 125, 126, new string[6] { "Location", "Settlement", "OrgGrade", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 70, -1, ELifeRecordDisplayType.Relation));
		_dataArray.Add(new LifeRecordItem(69, 127, 128, new string[6] { "Location", "", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 50, -1, ELifeRecordDisplayType.Relation));
		_dataArray.Add(new LifeRecordItem(70, 129, 130, new string[6] { "Character", "Location", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 50, -1, ELifeRecordDisplayType.Relation));
		_dataArray.Add(new LifeRecordItem(71, 131, 132, new string[6] { "Character", "Location", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 50, -1, ELifeRecordDisplayType.Relation));
		_dataArray.Add(new LifeRecordItem(72, 133, 134, new string[6] { "Character", "Location", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 50, -1, ELifeRecordDisplayType.Relation));
		_dataArray.Add(new LifeRecordItem(73, 135, 136, new string[6] { "Character", "Location", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 50, -1, ELifeRecordDisplayType.Relation));
		_dataArray.Add(new LifeRecordItem(74, 137, 138, new string[6] { "Character", "Location", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 50, -1, ELifeRecordDisplayType.Relation));
		_dataArray.Add(new LifeRecordItem(75, 139, 140, new string[6] { "Character", "Location", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 50, -1, ELifeRecordDisplayType.Normal));
		_dataArray.Add(new LifeRecordItem(76, 141, 142, new string[6] { "Character", "Location", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 50, -1, ELifeRecordDisplayType.Normal));
		_dataArray.Add(new LifeRecordItem(77, 143, 144, new string[6] { "Character", "Location", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 50, -1, ELifeRecordDisplayType.Normal));
		_dataArray.Add(new LifeRecordItem(78, 145, 146, new string[6] { "Character", "Location", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 50, -1, ELifeRecordDisplayType.Normal));
		_dataArray.Add(new LifeRecordItem(79, 147, 148, new string[6] { "Location", "", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 50, -1, ELifeRecordDisplayType.Normal));
		_dataArray.Add(new LifeRecordItem(80, 149, 150, new string[6] { "Location", "", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 50, -1, ELifeRecordDisplayType.Normal));
		_dataArray.Add(new LifeRecordItem(81, 151, 152, new string[6] { "Location", "Item", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Calculated, -1, -1, ELifeRecordDisplayType.Normal));
		_dataArray.Add(new LifeRecordItem(82, 153, 154, new string[6] { "Location", "", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 40, -1, ELifeRecordDisplayType.Normal));
		_dataArray.Add(new LifeRecordItem(83, 155, 156, new string[6] { "Location", "", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Calculated, -1, -1, ELifeRecordDisplayType.Normal));
		_dataArray.Add(new LifeRecordItem(84, 157, 158, new string[6] { "Character", "Location", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 50, -1, ELifeRecordDisplayType.Relation));
		_dataArray.Add(new LifeRecordItem(85, 159, 160, new string[6] { "Character", "Location", "CombatType", "", "", "" }, isSourceRecord: true, new List<short> { 86 }, -30000, ELifeRecordScoreType.Normal, 70, -1, ELifeRecordDisplayType.Combat));
		_dataArray.Add(new LifeRecordItem(86, 161, 162, new string[6] { "Character", "Location", "CombatType", "", "", "" }, isSourceRecord: true, new List<short> { 85 }, -30000, ELifeRecordScoreType.Normal, 30, -1, ELifeRecordDisplayType.Combat));
		_dataArray.Add(new LifeRecordItem(87, 163, 164, new string[6] { "Character", "Location", "CombatType", "", "", "" }, isSourceRecord: true, new List<short> { 88 }, -30000, ELifeRecordScoreType.Normal, 60, -1, ELifeRecordDisplayType.Combat));
		_dataArray.Add(new LifeRecordItem(88, 165, 166, new string[6] { "Character", "Location", "CombatType", "", "", "" }, isSourceRecord: true, new List<short> { 87 }, -30000, ELifeRecordScoreType.Normal, 40, -1, ELifeRecordDisplayType.Combat));
		_dataArray.Add(new LifeRecordItem(89, 167, 168, new string[6] { "Character", "Location", "", "", "", "" }, isSourceRecord: true, new List<short> { 90 }, -30000, ELifeRecordScoreType.Normal, 50, -1, ELifeRecordDisplayType.Combat));
		_dataArray.Add(new LifeRecordItem(90, 169, 170, new string[6] { "Character", "Location", "", "", "", "" }, isSourceRecord: true, new List<short> { 89 }, -30000, ELifeRecordScoreType.Normal, 50, -1, ELifeRecordDisplayType.Combat));
		_dataArray.Add(new LifeRecordItem(91, 171, 172, new string[6] { "Character", "Location", "", "", "", "" }, isSourceRecord: true, new List<short> { 713 }, -30000, ELifeRecordScoreType.Normal, 30, -1, ELifeRecordDisplayType.Crime));
		_dataArray.Add(new LifeRecordItem(92, 173, 174, new string[6] { "Character", "Location", "", "", "", "" }, isSourceRecord: true, new List<short> { 712 }, -30000, ELifeRecordScoreType.Normal, 20, -1, ELifeRecordDisplayType.Crime));
		_dataArray.Add(new LifeRecordItem(93, 175, 176, new string[6] { "Character", "Location", "Item", "", "", "" }, isSourceRecord: true, new List<short> { 96 }, -30000, ELifeRecordScoreType.Normal, 40, -1, ELifeRecordDisplayType.Crime));
		_dataArray.Add(new LifeRecordItem(94, 177, 178, new string[6] { "Character", "Location", "Item", "", "", "" }, isSourceRecord: true, new List<short> { 97 }, -30000, ELifeRecordScoreType.Normal, 30, -1, ELifeRecordDisplayType.Crime));
		_dataArray.Add(new LifeRecordItem(95, 179, 180, new string[6] { "Character", "Location", "", "", "", "" }, isSourceRecord: true, new List<short> { 98 }, -30000, ELifeRecordScoreType.Normal, 50, -1, ELifeRecordDisplayType.Combat));
		_dataArray.Add(new LifeRecordItem(96, 175, 181, new string[6] { "", "", "", "", "", "" }, isSourceRecord: false, new List<short> { 93 }, -30000, ELifeRecordScoreType.Normal, 30, -1, ELifeRecordDisplayType.Great));
		_dataArray.Add(new LifeRecordItem(97, 177, 182, new string[6] { "", "", "", "", "", "" }, isSourceRecord: false, new List<short> { 94 }, -30000, ELifeRecordScoreType.Normal, 20, -1, ELifeRecordDisplayType.Great));
		_dataArray.Add(new LifeRecordItem(98, 183, 184, new string[6] { "", "", "", "", "", "" }, isSourceRecord: false, new List<short> { 95 }, -30000, ELifeRecordScoreType.Normal, 50, -1, ELifeRecordDisplayType.Relation));
		_dataArray.Add(new LifeRecordItem(99, 185, 136, new string[6] { "Character", "Location", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 50, -1, ELifeRecordDisplayType.Relation));
		_dataArray.Add(new LifeRecordItem(100, 186, 187, new string[6] { "Character", "Location", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 50, -1, ELifeRecordDisplayType.Relation));
		_dataArray.Add(new LifeRecordItem(101, 188, 189, new string[6] { "Location", "Adventure", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 50, -1, ELifeRecordDisplayType.Normal));
		_dataArray.Add(new LifeRecordItem(102, 190, 191, new string[6] { "Character", "Location", "Item", "", "", "" }, isSourceRecord: true, new List<short> { 138 }, -30000, ELifeRecordScoreType.Normal, 60, -1, ELifeRecordDisplayType.Normal));
		_dataArray.Add(new LifeRecordItem(103, 192, 193, new string[6] { "Character", "Location", "Item", "", "", "" }, isSourceRecord: true, new List<short> { 139 }, -30000, ELifeRecordScoreType.Normal, 60, -1, ELifeRecordDisplayType.Normal));
		_dataArray.Add(new LifeRecordItem(104, 194, 195, new string[6] { "Character", "Location", "Item", "PoisonType", "", "" }, isSourceRecord: true, new List<short> { 140 }, -30000, ELifeRecordScoreType.Normal, 60, -1, ELifeRecordDisplayType.Normal));
		_dataArray.Add(new LifeRecordItem(105, 196, 197, new string[6] { "Character", "Location", "Item", "", "", "" }, isSourceRecord: true, new List<short> { 141 }, -30000, ELifeRecordScoreType.Normal, 60, -1, ELifeRecordDisplayType.Normal));
		_dataArray.Add(new LifeRecordItem(106, 198, 199, new string[6] { "Character", "Location", "Item", "", "", "" }, isSourceRecord: true, new List<short> { 142 }, -30000, ELifeRecordScoreType.Normal, 60, -1, ELifeRecordDisplayType.Normal));
		_dataArray.Add(new LifeRecordItem(107, 200, 201, new string[6] { "Character", "Location", "Item", "", "", "" }, isSourceRecord: true, new List<short> { 143 }, -30000, ELifeRecordScoreType.Normal, 60, -1, ELifeRecordDisplayType.Normal));
		_dataArray.Add(new LifeRecordItem(108, 202, 203, new string[6] { "Character", "Location", "Item", "Item", "", "" }, isSourceRecord: true, new List<short> { 144 }, -30000, ELifeRecordScoreType.Normal, 60, -1, ELifeRecordDisplayType.Normal));
		_dataArray.Add(new LifeRecordItem(109, 204, 205, new string[6] { "Character", "Location", "Item", "", "", "" }, isSourceRecord: true, new List<short> { 145 }, -30000, ELifeRecordScoreType.Normal, 60, -1, ELifeRecordDisplayType.Normal));
		_dataArray.Add(new LifeRecordItem(110, 206, 207, new string[6] { "Character", "Location", "Item", "", "", "" }, isSourceRecord: true, new List<short> { 146 }, -30000, ELifeRecordScoreType.Normal, 60, -1, ELifeRecordDisplayType.Normal));
		_dataArray.Add(new LifeRecordItem(111, 208, 209, new string[6] { "Character", "Location", "Resource", "Integer", "", "" }, isSourceRecord: true, new List<short> { 147 }, -30000, ELifeRecordScoreType.Normal, 60, -1, ELifeRecordDisplayType.Normal));
		_dataArray.Add(new LifeRecordItem(112, 210, 211, new string[6] { "Character", "Location", "Item", "", "", "" }, isSourceRecord: true, new List<short> { 148 }, -30000, ELifeRecordScoreType.Normal, 60, -1, ELifeRecordDisplayType.Normal));
		_dataArray.Add(new LifeRecordItem(113, 212, 213, new string[6] { "Character", "Location", "Item", "", "", "" }, isSourceRecord: true, new List<short> { 149 }, -30000, ELifeRecordScoreType.Normal, 60, -1, ELifeRecordDisplayType.Normal));
		_dataArray.Add(new LifeRecordItem(114, 214, 215, new string[6] { "Character", "Location", "Item", "", "", "" }, isSourceRecord: true, new List<short> { 150 }, -30000, ELifeRecordScoreType.Normal, 60, -1, ELifeRecordDisplayType.Normal));
		_dataArray.Add(new LifeRecordItem(115, 216, 217, new string[6] { "Character", "Location", "Item", "Integer", "", "" }, isSourceRecord: true, new List<short> { 151 }, -30000, ELifeRecordScoreType.Normal, 60, -1, ELifeRecordDisplayType.Study));
		_dataArray.Add(new LifeRecordItem(116, 218, 217, new string[6] { "Character", "Location", "Item", "Integer", "", "" }, isSourceRecord: true, new List<short> { 152 }, -30000, ELifeRecordScoreType.Normal, 60, -1, ELifeRecordDisplayType.Study));
		_dataArray.Add(new LifeRecordItem(117, 219, 220, new string[6] { "Character", "Location", "Item", "Integer", "", "" }, isSourceRecord: true, new List<short> { 153 }, -30000, ELifeRecordScoreType.Normal, 50, -1, ELifeRecordDisplayType.Study));
		_dataArray.Add(new LifeRecordItem(118, 219, 220, new string[6] { "Character", "Location", "Item", "Integer", "", "" }, isSourceRecord: true, new List<short> { 154 }, -30000, ELifeRecordScoreType.Normal, 50, -1, ELifeRecordDisplayType.Study));
		_dataArray.Add(new LifeRecordItem(119, 221, 222, new string[6] { "Character", "Location", "Item", "Integer", "", "" }, isSourceRecord: true, new List<short> { 155 }, -30000, ELifeRecordScoreType.Normal, 60, -1, ELifeRecordDisplayType.Study));
	}

	private void CreateItems2()
	{
		_dataArray.Add(new LifeRecordItem(120, 223, 224, new string[6] { "Character", "Location", "CombatSkill", "", "", "" }, isSourceRecord: true, new List<short> { 156 }, -30000, ELifeRecordScoreType.Normal, 60, -1, ELifeRecordDisplayType.Study));
		_dataArray.Add(new LifeRecordItem(121, 225, 226, new string[6] { "Character", "Location", "Item", "", "", "" }, isSourceRecord: true, new List<short> { 157 }, -30000, ELifeRecordScoreType.Normal, 40, -1, ELifeRecordDisplayType.Normal));
		_dataArray.Add(new LifeRecordItem(122, 225, 227, new string[6] { "Character", "Location", "Item", "", "", "" }, isSourceRecord: true, new List<short> { 158 }, -30000, ELifeRecordScoreType.Normal, 40, -1, ELifeRecordDisplayType.Normal));
		_dataArray.Add(new LifeRecordItem(123, 225, 228, new string[6] { "Character", "Location", "Item", "PoisonType", "", "" }, isSourceRecord: true, new List<short> { 159 }, -30000, ELifeRecordScoreType.Normal, 40, -1, ELifeRecordDisplayType.Normal));
		_dataArray.Add(new LifeRecordItem(124, 225, 229, new string[6] { "Character", "Location", "Item", "", "", "" }, isSourceRecord: true, new List<short> { 160 }, -30000, ELifeRecordScoreType.Normal, 40, -1, ELifeRecordDisplayType.Normal));
		_dataArray.Add(new LifeRecordItem(125, 225, 230, new string[6] { "Character", "Location", "Item", "", "", "" }, isSourceRecord: true, new List<short> { 161 }, -30000, ELifeRecordScoreType.Normal, 40, -1, ELifeRecordDisplayType.Normal));
		_dataArray.Add(new LifeRecordItem(126, 225, 231, new string[6] { "Character", "Location", "Item", "", "", "" }, isSourceRecord: true, new List<short> { 162 }, -30000, ELifeRecordScoreType.Normal, 40, -1, ELifeRecordDisplayType.Normal));
		_dataArray.Add(new LifeRecordItem(127, 225, 232, new string[6] { "Character", "Location", "Item", "Item", "", "" }, isSourceRecord: true, new List<short> { 163 }, -30000, ELifeRecordScoreType.Normal, 40, -1, ELifeRecordDisplayType.Normal));
		_dataArray.Add(new LifeRecordItem(128, 225, 233, new string[6] { "Character", "Location", "Item", "", "", "" }, isSourceRecord: true, new List<short> { 164 }, -30000, ELifeRecordScoreType.Normal, 40, -1, ELifeRecordDisplayType.Normal));
		_dataArray.Add(new LifeRecordItem(129, 225, 234, new string[6] { "Character", "Location", "Item", "", "", "" }, isSourceRecord: true, new List<short> { 165 }, -30000, ELifeRecordScoreType.Normal, 40, -1, ELifeRecordDisplayType.Normal));
		_dataArray.Add(new LifeRecordItem(130, 225, 235, new string[6] { "Character", "Location", "Resource", "Integer", "", "" }, isSourceRecord: true, new List<short> { 166 }, -30000, ELifeRecordScoreType.Normal, 40, -1, ELifeRecordDisplayType.Normal));
		_dataArray.Add(new LifeRecordItem(131, 225, 236, new string[6] { "Character", "Location", "Item", "", "", "" }, isSourceRecord: true, new List<short> { 167 }, -30000, ELifeRecordScoreType.Normal, 40, -1, ELifeRecordDisplayType.Normal));
		_dataArray.Add(new LifeRecordItem(132, 225, 237, new string[6] { "Character", "Location", "Item", "", "", "" }, isSourceRecord: true, new List<short> { 168 }, -30000, ELifeRecordScoreType.Normal, 40, -1, ELifeRecordDisplayType.Normal));
		_dataArray.Add(new LifeRecordItem(133, 225, 238, new string[6] { "Character", "Location", "Item", "", "", "" }, isSourceRecord: true, new List<short> { 169 }, -30000, ELifeRecordScoreType.Normal, 40, -1, ELifeRecordDisplayType.Normal));
		_dataArray.Add(new LifeRecordItem(134, 225, 239, new string[6] { "Character", "Location", "Item", "Integer", "", "" }, isSourceRecord: true, new List<short> { 170 }, -30000, ELifeRecordScoreType.Normal, 40, -1, ELifeRecordDisplayType.Study));
		_dataArray.Add(new LifeRecordItem(135, 225, 239, new string[6] { "Character", "Location", "Item", "Integer", "", "" }, isSourceRecord: true, new List<short> { 171 }, -30000, ELifeRecordScoreType.Normal, 40, -1, ELifeRecordDisplayType.Study));
		_dataArray.Add(new LifeRecordItem(136, 225, 240, new string[6] { "Character", "Location", "Item", "Integer", "", "" }, isSourceRecord: true, new List<short> { 172 }, -30000, ELifeRecordScoreType.Normal, 40, -1, ELifeRecordDisplayType.Study));
		_dataArray.Add(new LifeRecordItem(137, 225, 241, new string[6] { "Character", "Location", "CombatSkill", "", "", "" }, isSourceRecord: true, new List<short> { 173 }, -30000, ELifeRecordScoreType.Normal, 40, -1, ELifeRecordDisplayType.Study));
		_dataArray.Add(new LifeRecordItem(138, 242, 243, new string[6] { "", "", "", "", "", "" }, isSourceRecord: false, new List<short> { 102 }, -30000, ELifeRecordScoreType.Normal, 50, -1, ELifeRecordDisplayType.Normal));
		_dataArray.Add(new LifeRecordItem(139, 242, 244, new string[6] { "", "", "", "", "", "" }, isSourceRecord: false, new List<short> { 103 }, -30000, ELifeRecordScoreType.Normal, 50, -1, ELifeRecordDisplayType.Normal));
		_dataArray.Add(new LifeRecordItem(140, 242, 245, new string[6] { "", "", "", "", "", "" }, isSourceRecord: false, new List<short> { 104 }, -30000, ELifeRecordScoreType.Normal, 50, -1, ELifeRecordDisplayType.Normal));
		_dataArray.Add(new LifeRecordItem(141, 242, 246, new string[6] { "", "", "", "", "", "" }, isSourceRecord: false, new List<short> { 105 }, -30000, ELifeRecordScoreType.Normal, 50, -1, ELifeRecordDisplayType.Normal));
		_dataArray.Add(new LifeRecordItem(142, 242, 247, new string[6] { "", "", "", "", "", "" }, isSourceRecord: false, new List<short> { 106 }, -30000, ELifeRecordScoreType.Normal, 50, -1, ELifeRecordDisplayType.Normal));
		_dataArray.Add(new LifeRecordItem(143, 242, 248, new string[6] { "", "", "", "", "", "" }, isSourceRecord: false, new List<short> { 107 }, -30000, ELifeRecordScoreType.Normal, 50, -1, ELifeRecordDisplayType.Normal));
		_dataArray.Add(new LifeRecordItem(144, 242, 249, new string[6] { "", "", "", "", "", "" }, isSourceRecord: false, new List<short> { 108 }, -30000, ELifeRecordScoreType.Normal, 50, -1, ELifeRecordDisplayType.Normal));
		_dataArray.Add(new LifeRecordItem(145, 242, 250, new string[6] { "", "", "", "", "", "" }, isSourceRecord: false, new List<short> { 109 }, -30000, ELifeRecordScoreType.Normal, 50, -1, ELifeRecordDisplayType.Normal));
		_dataArray.Add(new LifeRecordItem(146, 242, 251, new string[6] { "", "", "", "", "", "" }, isSourceRecord: false, new List<short> { 110 }, -30000, ELifeRecordScoreType.Normal, 50, -1, ELifeRecordDisplayType.Normal));
		_dataArray.Add(new LifeRecordItem(147, 242, 252, new string[6] { "", "", "", "", "", "" }, isSourceRecord: false, new List<short> { 111 }, -30000, ELifeRecordScoreType.Normal, 50, -1, ELifeRecordDisplayType.Normal));
		_dataArray.Add(new LifeRecordItem(148, 242, 253, new string[6] { "", "", "", "", "", "" }, isSourceRecord: false, new List<short> { 112 }, -30000, ELifeRecordScoreType.Normal, 50, -1, ELifeRecordDisplayType.Normal));
		_dataArray.Add(new LifeRecordItem(149, 242, 254, new string[6] { "", "", "", "", "", "" }, isSourceRecord: false, new List<short> { 113 }, -30000, ELifeRecordScoreType.Normal, 50, -1, ELifeRecordDisplayType.Normal));
		_dataArray.Add(new LifeRecordItem(150, 242, 255, new string[6] { "", "", "", "", "", "" }, isSourceRecord: false, new List<short> { 114 }, -30000, ELifeRecordScoreType.Normal, 50, -1, ELifeRecordDisplayType.Normal));
		_dataArray.Add(new LifeRecordItem(151, 242, 256, new string[6] { "", "", "", "", "", "" }, isSourceRecord: false, new List<short> { 115 }, -30000, ELifeRecordScoreType.Normal, 50, -1, ELifeRecordDisplayType.Study));
		_dataArray.Add(new LifeRecordItem(152, 242, 256, new string[6] { "", "", "", "", "", "" }, isSourceRecord: false, new List<short> { 116 }, -30000, ELifeRecordScoreType.Normal, 50, -1, ELifeRecordDisplayType.Study));
		_dataArray.Add(new LifeRecordItem(153, 242, 257, new string[6] { "", "", "", "", "", "" }, isSourceRecord: false, new List<short> { 117 }, -30000, ELifeRecordScoreType.Normal, 50, -1, ELifeRecordDisplayType.Study));
		_dataArray.Add(new LifeRecordItem(154, 242, 257, new string[6] { "", "", "", "", "", "" }, isSourceRecord: false, new List<short> { 118 }, -30000, ELifeRecordScoreType.Normal, 50, -1, ELifeRecordDisplayType.Study));
		_dataArray.Add(new LifeRecordItem(155, 242, 258, new string[6] { "", "", "", "", "", "" }, isSourceRecord: false, new List<short> { 119 }, -30000, ELifeRecordScoreType.Normal, 50, -1, ELifeRecordDisplayType.Study));
		_dataArray.Add(new LifeRecordItem(156, 242, 259, new string[6] { "", "", "", "", "", "" }, isSourceRecord: false, new List<short> { 120 }, -30000, ELifeRecordScoreType.Normal, 50, -1, ELifeRecordDisplayType.Study));
		_dataArray.Add(new LifeRecordItem(157, 260, 261, new string[6] { "", "", "", "", "", "" }, isSourceRecord: false, new List<short> { 121 }, -30000, ELifeRecordScoreType.Normal, 50, -1, ELifeRecordDisplayType.Normal));
		_dataArray.Add(new LifeRecordItem(158, 260, 262, new string[6] { "", "", "", "", "", "" }, isSourceRecord: false, new List<short> { 122 }, -30000, ELifeRecordScoreType.Normal, 50, -1, ELifeRecordDisplayType.Normal));
		_dataArray.Add(new LifeRecordItem(159, 260, 263, new string[6] { "", "", "", "", "", "" }, isSourceRecord: false, new List<short> { 123 }, -30000, ELifeRecordScoreType.Normal, 50, -1, ELifeRecordDisplayType.Normal));
		_dataArray.Add(new LifeRecordItem(160, 260, 264, new string[6] { "", "", "", "", "", "" }, isSourceRecord: false, new List<short> { 124 }, -30000, ELifeRecordScoreType.Normal, 50, -1, ELifeRecordDisplayType.Normal));
		_dataArray.Add(new LifeRecordItem(161, 260, 265, new string[6] { "", "", "", "", "", "" }, isSourceRecord: false, new List<short> { 125 }, -30000, ELifeRecordScoreType.Normal, 50, -1, ELifeRecordDisplayType.Normal));
		_dataArray.Add(new LifeRecordItem(162, 260, 266, new string[6] { "", "", "", "", "", "" }, isSourceRecord: false, new List<short> { 126 }, -30000, ELifeRecordScoreType.Normal, 50, -1, ELifeRecordDisplayType.Normal));
		_dataArray.Add(new LifeRecordItem(163, 260, 267, new string[6] { "", "", "", "", "", "" }, isSourceRecord: false, new List<short> { 127 }, -30000, ELifeRecordScoreType.Normal, 50, -1, ELifeRecordDisplayType.Normal));
		_dataArray.Add(new LifeRecordItem(164, 260, 268, new string[6] { "", "", "", "", "", "" }, isSourceRecord: false, new List<short> { 128 }, -30000, ELifeRecordScoreType.Normal, 50, -1, ELifeRecordDisplayType.Normal));
		_dataArray.Add(new LifeRecordItem(165, 260, 269, new string[6] { "", "", "", "", "", "" }, isSourceRecord: false, new List<short> { 129 }, -30000, ELifeRecordScoreType.Normal, 50, -1, ELifeRecordDisplayType.Normal));
		_dataArray.Add(new LifeRecordItem(166, 260, 270, new string[6] { "", "", "", "", "", "" }, isSourceRecord: false, new List<short> { 130 }, -30000, ELifeRecordScoreType.Normal, 50, -1, ELifeRecordDisplayType.Normal));
		_dataArray.Add(new LifeRecordItem(167, 260, 271, new string[6] { "", "", "", "", "", "" }, isSourceRecord: false, new List<short> { 131 }, -30000, ELifeRecordScoreType.Normal, 50, -1, ELifeRecordDisplayType.Normal));
		_dataArray.Add(new LifeRecordItem(168, 260, 272, new string[6] { "", "", "", "", "", "" }, isSourceRecord: false, new List<short> { 132 }, -30000, ELifeRecordScoreType.Normal, 50, -1, ELifeRecordDisplayType.Normal));
		_dataArray.Add(new LifeRecordItem(169, 260, 273, new string[6] { "", "", "", "", "", "" }, isSourceRecord: false, new List<short> { 133 }, -30000, ELifeRecordScoreType.Normal, 50, -1, ELifeRecordDisplayType.Normal));
		_dataArray.Add(new LifeRecordItem(170, 260, 274, new string[6] { "", "", "", "", "", "" }, isSourceRecord: false, new List<short> { 134 }, -30000, ELifeRecordScoreType.Normal, 50, -1, ELifeRecordDisplayType.Study));
		_dataArray.Add(new LifeRecordItem(171, 260, 274, new string[6] { "", "", "", "", "", "" }, isSourceRecord: false, new List<short> { 135 }, -30000, ELifeRecordScoreType.Normal, 50, -1, ELifeRecordDisplayType.Study));
		_dataArray.Add(new LifeRecordItem(172, 260, 275, new string[6] { "", "", "", "", "", "" }, isSourceRecord: false, new List<short> { 136 }, -30000, ELifeRecordScoreType.Normal, 50, -1, ELifeRecordDisplayType.Study));
		_dataArray.Add(new LifeRecordItem(173, 260, 276, new string[6] { "", "", "", "", "", "" }, isSourceRecord: false, new List<short> { 137 }, -30000, ELifeRecordScoreType.Normal, 50, -1, ELifeRecordDisplayType.Study));
		_dataArray.Add(new LifeRecordItem(174, 277, 278, new string[6] { "Character", "Location", "Character", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 40, -1, ELifeRecordDisplayType.Relation));
		_dataArray.Add(new LifeRecordItem(175, 277, 279, new string[6] { "Character", "Location", "Character", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 40, -1, ELifeRecordDisplayType.Relation));
		_dataArray.Add(new LifeRecordItem(176, 277, 280, new string[6] { "Character", "Location", "Character", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 40, -1, ELifeRecordDisplayType.Relation));
		_dataArray.Add(new LifeRecordItem(177, 277, 281, new string[6] { "Character", "Location", "Character", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 40, -1, ELifeRecordDisplayType.Relation));
		_dataArray.Add(new LifeRecordItem(178, 277, 282, new string[6] { "Character", "Location", "Character", "", "", "" }, isSourceRecord: true, new List<short> { 180 }, -30000, ELifeRecordScoreType.Normal, 60, -1, ELifeRecordDisplayType.Relation));
		_dataArray.Add(new LifeRecordItem(179, 277, 283, new string[6] { "Character", "Location", "Character", "", "", "" }, isSourceRecord: true, new List<short> { 180 }, -30000, ELifeRecordScoreType.Normal, 60, -1, ELifeRecordDisplayType.Relation));
	}

	private void CreateItems3()
	{
		_dataArray.Add(new LifeRecordItem(180, 277, 284, new string[6] { "", "", "", "", "", "" }, isSourceRecord: false, new List<short> { 178, 179 }, -30000, ELifeRecordScoreType.Normal, 40, -1, ELifeRecordDisplayType.Relation));
		_dataArray.Add(new LifeRecordItem(181, 285, 286, new string[6] { "Character", "Location", "Character", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 40, -1, ELifeRecordDisplayType.Relation));
		_dataArray.Add(new LifeRecordItem(182, 285, 287, new string[6] { "Character", "Location", "Character", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 40, -1, ELifeRecordDisplayType.Relation));
		_dataArray.Add(new LifeRecordItem(183, 285, 288, new string[6] { "Character", "Location", "Character", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 40, -1, ELifeRecordDisplayType.Relation));
		_dataArray.Add(new LifeRecordItem(184, 285, 289, new string[6] { "Character", "Location", "Character", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 40, -1, ELifeRecordDisplayType.Relation));
		_dataArray.Add(new LifeRecordItem(185, 285, 290, new string[6] { "Character", "Location", "Character", "", "", "" }, isSourceRecord: true, new List<short> { 187 }, -30000, ELifeRecordScoreType.Normal, 60, -1, ELifeRecordDisplayType.Relation));
		_dataArray.Add(new LifeRecordItem(186, 285, 291, new string[6] { "Character", "Location", "Character", "", "", "" }, isSourceRecord: true, new List<short> { 187 }, -30000, ELifeRecordScoreType.Normal, 60, -1, ELifeRecordDisplayType.Relation));
		_dataArray.Add(new LifeRecordItem(187, 285, 292, new string[6] { "", "", "", "", "", "" }, isSourceRecord: false, new List<short> { 185, 186 }, -30000, ELifeRecordScoreType.Normal, 40, -1, ELifeRecordDisplayType.Relation));
		_dataArray.Add(new LifeRecordItem(188, 293, 294, new string[6] { "Character", "Location", "Character", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 40, -1, ELifeRecordDisplayType.Relation));
		_dataArray.Add(new LifeRecordItem(189, 293, 295, new string[6] { "Character", "Location", "Character", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 40, -1, ELifeRecordDisplayType.Relation));
		_dataArray.Add(new LifeRecordItem(190, 293, 296, new string[6] { "Character", "Location", "Character", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 40, -1, ELifeRecordDisplayType.Relation));
		_dataArray.Add(new LifeRecordItem(191, 293, 297, new string[6] { "Character", "Location", "Character", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 40, -1, ELifeRecordDisplayType.Relation));
		_dataArray.Add(new LifeRecordItem(192, 293, 298, new string[6] { "Character", "Location", "Character", "", "", "" }, isSourceRecord: true, new List<short> { 194 }, -30000, ELifeRecordScoreType.Normal, 60, -1, ELifeRecordDisplayType.Relation));
		_dataArray.Add(new LifeRecordItem(193, 293, 299, new string[6] { "Character", "Location", "Character", "", "", "" }, isSourceRecord: true, new List<short> { 194 }, -30000, ELifeRecordScoreType.Normal, 60, -1, ELifeRecordDisplayType.Relation));
		_dataArray.Add(new LifeRecordItem(194, 293, 300, new string[6] { "", "", "", "", "", "" }, isSourceRecord: false, new List<short> { 192, 193 }, -30000, ELifeRecordScoreType.Normal, 40, -1, ELifeRecordDisplayType.Relation));
		_dataArray.Add(new LifeRecordItem(195, 301, 302, new string[6] { "Character", "Location", "Item", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 40, -1, ELifeRecordDisplayType.Crime));
		_dataArray.Add(new LifeRecordItem(196, 301, 303, new string[6] { "Character", "Location", "Item", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 40, -1, ELifeRecordDisplayType.Crime));
		_dataArray.Add(new LifeRecordItem(197, 301, 304, new string[6] { "Character", "Location", "Item", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 40, -1, ELifeRecordDisplayType.Crime));
		_dataArray.Add(new LifeRecordItem(198, 301, 305, new string[6] { "Character", "Location", "Item", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 40, -1, ELifeRecordDisplayType.Crime));
		_dataArray.Add(new LifeRecordItem(199, 301, 306, new string[6] { "Character", "Location", "Item", "", "", "" }, isSourceRecord: true, new List<short> { 201 }, -30000, ELifeRecordScoreType.Normal, 60, -1, ELifeRecordDisplayType.Crime));
		_dataArray.Add(new LifeRecordItem(200, 301, 307, new string[6] { "Character", "Location", "Item", "", "", "" }, isSourceRecord: true, new List<short> { 201 }, -30000, ELifeRecordScoreType.Normal, 60, -1, ELifeRecordDisplayType.Crime));
		_dataArray.Add(new LifeRecordItem(201, 301, 308, new string[6] { "", "", "", "", "", "" }, isSourceRecord: false, new List<short> { 199, 200 }, -30000, ELifeRecordScoreType.Normal, 40, -1, ELifeRecordDisplayType.Crime));
		_dataArray.Add(new LifeRecordItem(202, 309, 310, new string[6] { "Character", "Location", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 40, -1, ELifeRecordDisplayType.Crime));
		_dataArray.Add(new LifeRecordItem(203, 309, 311, new string[6] { "Character", "Location", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 40, -1, ELifeRecordDisplayType.Crime));
		_dataArray.Add(new LifeRecordItem(204, 309, 312, new string[6] { "Character", "Location", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 40, -1, ELifeRecordDisplayType.Crime));
		_dataArray.Add(new LifeRecordItem(205, 309, 313, new string[6] { "Character", "Location", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 40, -1, ELifeRecordDisplayType.Crime));
		_dataArray.Add(new LifeRecordItem(206, 309, 314, new string[6] { "Character", "Location", "", "", "", "" }, isSourceRecord: true, new List<short> { 208 }, -30000, ELifeRecordScoreType.Normal, 60, -1, ELifeRecordDisplayType.Crime));
		_dataArray.Add(new LifeRecordItem(207, 309, 315, new string[6] { "Character", "Location", "", "", "", "" }, isSourceRecord: true, new List<short> { 208 }, -30000, ELifeRecordScoreType.Normal, 60, -1, ELifeRecordDisplayType.Crime));
		_dataArray.Add(new LifeRecordItem(208, 309, 316, new string[6] { "", "", "", "", "", "" }, isSourceRecord: false, new List<short> { 206, 207 }, -30000, ELifeRecordScoreType.Normal, 40, -1, ELifeRecordDisplayType.Crime));
		_dataArray.Add(new LifeRecordItem(209, 317, 318, new string[6] { "Character", "Location", "Resource", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 40, -1, ELifeRecordDisplayType.Crime));
		_dataArray.Add(new LifeRecordItem(210, 317, 319, new string[6] { "Character", "Location", "Resource", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 40, -1, ELifeRecordDisplayType.Crime));
		_dataArray.Add(new LifeRecordItem(211, 317, 320, new string[6] { "Character", "Location", "Resource", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 40, -1, ELifeRecordDisplayType.Crime));
		_dataArray.Add(new LifeRecordItem(212, 317, 321, new string[6] { "Character", "Location", "Resource", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 40, -1, ELifeRecordDisplayType.Crime));
		_dataArray.Add(new LifeRecordItem(213, 317, 322, new string[6] { "Character", "Location", "Resource", "Integer", "", "" }, isSourceRecord: true, new List<short> { 216 }, -30000, ELifeRecordScoreType.Calculated, -1, -1, ELifeRecordDisplayType.Crime));
		_dataArray.Add(new LifeRecordItem(214, 317, 323, new string[6] { "Character", "Location", "Resource", "Integer", "", "" }, isSourceRecord: true, new List<short> { 216 }, -30000, ELifeRecordScoreType.Calculated, -1, -1, ELifeRecordDisplayType.Crime));
		_dataArray.Add(new LifeRecordItem(215, 317, 324, new string[6] { "Character", "Location", "Resource", "Integer", "", "" }, isSourceRecord: true, new List<short> { 217 }, -30000, ELifeRecordScoreType.Calculated, -1, -1, ELifeRecordDisplayType.Crime));
		_dataArray.Add(new LifeRecordItem(216, 317, 325, new string[6] { "", "", "", "", "", "" }, isSourceRecord: false, new List<short> { 213, 214 }, -30000, ELifeRecordScoreType.Calculated, -1, -1, ELifeRecordDisplayType.Crime));
		_dataArray.Add(new LifeRecordItem(217, 317, 326, new string[6] { "", "", "", "", "", "" }, isSourceRecord: false, new List<short> { 215 }, -30000, ELifeRecordScoreType.Calculated, -1, -1, ELifeRecordDisplayType.Crime));
		_dataArray.Add(new LifeRecordItem(218, 327, 328, new string[6] { "Character", "Location", "Resource", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 40, -1, ELifeRecordDisplayType.Crime));
		_dataArray.Add(new LifeRecordItem(219, 327, 329, new string[6] { "Character", "Location", "Resource", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 40, -1, ELifeRecordDisplayType.Crime));
		_dataArray.Add(new LifeRecordItem(220, 327, 330, new string[6] { "Character", "Location", "Resource", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 40, -1, ELifeRecordDisplayType.Crime));
		_dataArray.Add(new LifeRecordItem(221, 327, 331, new string[6] { "Character", "Location", "Resource", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 40, -1, ELifeRecordDisplayType.Crime));
		_dataArray.Add(new LifeRecordItem(222, 327, 332, new string[6] { "Character", "Location", "Resource", "Integer", "", "" }, isSourceRecord: true, new List<short> { 225 }, -30000, ELifeRecordScoreType.Calculated, -1, -1, ELifeRecordDisplayType.Crime));
		_dataArray.Add(new LifeRecordItem(223, 327, 333, new string[6] { "Character", "Location", "Resource", "Integer", "", "" }, isSourceRecord: true, new List<short> { 225 }, -30000, ELifeRecordScoreType.Calculated, -1, -1, ELifeRecordDisplayType.Crime));
		_dataArray.Add(new LifeRecordItem(224, 327, 334, new string[6] { "Character", "Location", "Resource", "Integer", "", "" }, isSourceRecord: true, new List<short> { 226 }, -30000, ELifeRecordScoreType.Calculated, -1, -1, ELifeRecordDisplayType.Crime));
		_dataArray.Add(new LifeRecordItem(225, 327, 335, new string[6] { "", "", "", "", "", "" }, isSourceRecord: false, new List<short> { 222, 223 }, -30000, ELifeRecordScoreType.Calculated, -1, -1, ELifeRecordDisplayType.Crime));
		_dataArray.Add(new LifeRecordItem(226, 327, 336, new string[6] { "", "", "", "", "", "" }, isSourceRecord: false, new List<short> { 224 }, -30000, ELifeRecordScoreType.Calculated, -1, -1, ELifeRecordDisplayType.Crime));
		_dataArray.Add(new LifeRecordItem(227, 337, 338, new string[6] { "Character", "Location", "Resource", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 40, -1, ELifeRecordDisplayType.Crime));
		_dataArray.Add(new LifeRecordItem(228, 337, 339, new string[6] { "Character", "Location", "Resource", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 40, -1, ELifeRecordDisplayType.Crime));
		_dataArray.Add(new LifeRecordItem(229, 337, 340, new string[6] { "Character", "Location", "Resource", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 40, -1, ELifeRecordDisplayType.Crime));
		_dataArray.Add(new LifeRecordItem(230, 337, 341, new string[6] { "Character", "Location", "Resource", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 40, -1, ELifeRecordDisplayType.Crime));
		_dataArray.Add(new LifeRecordItem(231, 337, 342, new string[6] { "Character", "Location", "Resource", "Integer", "", "" }, isSourceRecord: true, new List<short> { 234 }, -30000, ELifeRecordScoreType.Calculated, -1, -1, ELifeRecordDisplayType.Crime));
		_dataArray.Add(new LifeRecordItem(232, 337, 343, new string[6] { "Character", "Location", "Resource", "Integer", "", "" }, isSourceRecord: true, new List<short> { 234 }, -30000, ELifeRecordScoreType.Calculated, -1, -1, ELifeRecordDisplayType.Crime));
		_dataArray.Add(new LifeRecordItem(233, 337, 344, new string[6] { "Character", "Location", "Resource", "Integer", "", "" }, isSourceRecord: true, new List<short> { 235 }, -30000, ELifeRecordScoreType.Calculated, -1, -1, ELifeRecordDisplayType.Crime));
		_dataArray.Add(new LifeRecordItem(234, 337, 345, new string[6] { "", "", "", "", "", "" }, isSourceRecord: false, new List<short> { 231, 232 }, -30000, ELifeRecordScoreType.Calculated, -1, -1, ELifeRecordDisplayType.Crime));
		_dataArray.Add(new LifeRecordItem(235, 337, 346, new string[6] { "", "", "", "", "", "" }, isSourceRecord: false, new List<short> { 233 }, -30000, ELifeRecordScoreType.Calculated, -1, -1, ELifeRecordDisplayType.Crime));
		_dataArray.Add(new LifeRecordItem(236, 347, 318, new string[6] { "Character", "Location", "Item", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 40, -1, ELifeRecordDisplayType.Crime));
		_dataArray.Add(new LifeRecordItem(237, 347, 319, new string[6] { "Character", "Location", "Item", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 40, -1, ELifeRecordDisplayType.Crime));
		_dataArray.Add(new LifeRecordItem(238, 347, 320, new string[6] { "Character", "Location", "Item", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 40, -1, ELifeRecordDisplayType.Crime));
		_dataArray.Add(new LifeRecordItem(239, 347, 321, new string[6] { "Character", "Location", "Item", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 40, -1, ELifeRecordDisplayType.Crime));
	}

	private void CreateItems4()
	{
		_dataArray.Add(new LifeRecordItem(240, 347, 322, new string[6] { "Character", "Location", "Item", "", "", "" }, isSourceRecord: true, new List<short> { 243 }, -30000, ELifeRecordScoreType.Calculated, -1, -1, ELifeRecordDisplayType.Crime));
		_dataArray.Add(new LifeRecordItem(241, 347, 323, new string[6] { "Character", "Location", "Item", "", "", "" }, isSourceRecord: true, new List<short> { 243 }, -30000, ELifeRecordScoreType.Calculated, -1, -1, ELifeRecordDisplayType.Crime));
		_dataArray.Add(new LifeRecordItem(242, 347, 324, new string[6] { "Character", "Location", "Item", "", "", "" }, isSourceRecord: true, new List<short> { 244 }, -30000, ELifeRecordScoreType.Calculated, -1, -1, ELifeRecordDisplayType.Crime));
		_dataArray.Add(new LifeRecordItem(243, 347, 284, new string[6] { "", "", "", "", "", "" }, isSourceRecord: false, new List<short> { 240, 241 }, -30000, ELifeRecordScoreType.Calculated, -1, -1, ELifeRecordDisplayType.Crime));
		_dataArray.Add(new LifeRecordItem(244, 347, 348, new string[6] { "", "", "", "", "", "" }, isSourceRecord: false, new List<short> { 242 }, -30000, ELifeRecordScoreType.Calculated, -1, -1, ELifeRecordDisplayType.Crime));
		_dataArray.Add(new LifeRecordItem(245, 349, 328, new string[6] { "Character", "Location", "Item", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 40, -1, ELifeRecordDisplayType.Crime));
		_dataArray.Add(new LifeRecordItem(246, 349, 329, new string[6] { "Character", "Location", "Item", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 40, -1, ELifeRecordDisplayType.Crime));
		_dataArray.Add(new LifeRecordItem(247, 349, 330, new string[6] { "Character", "Location", "Item", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 40, -1, ELifeRecordDisplayType.Crime));
		_dataArray.Add(new LifeRecordItem(248, 349, 331, new string[6] { "Character", "Location", "Item", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 40, -1, ELifeRecordDisplayType.Crime));
		_dataArray.Add(new LifeRecordItem(249, 349, 332, new string[6] { "Character", "Location", "Item", "", "", "" }, isSourceRecord: true, new List<short> { 252 }, -30000, ELifeRecordScoreType.Calculated, -1, -1, ELifeRecordDisplayType.Crime));
		_dataArray.Add(new LifeRecordItem(250, 349, 333, new string[6] { "Character", "Location", "Item", "", "", "" }, isSourceRecord: true, new List<short> { 252 }, -30000, ELifeRecordScoreType.Calculated, -1, -1, ELifeRecordDisplayType.Crime));
		_dataArray.Add(new LifeRecordItem(251, 349, 334, new string[6] { "Character", "Location", "Item", "", "", "" }, isSourceRecord: true, new List<short> { 253 }, -30000, ELifeRecordScoreType.Calculated, -1, -1, ELifeRecordDisplayType.Crime));
		_dataArray.Add(new LifeRecordItem(252, 349, 292, new string[6] { "", "", "", "", "", "" }, isSourceRecord: false, new List<short> { 249, 250 }, -30000, ELifeRecordScoreType.Calculated, -1, -1, ELifeRecordDisplayType.Crime));
		_dataArray.Add(new LifeRecordItem(253, 349, 350, new string[6] { "", "", "", "", "", "" }, isSourceRecord: false, new List<short> { 251 }, -30000, ELifeRecordScoreType.Calculated, -1, -1, ELifeRecordDisplayType.Crime));
		_dataArray.Add(new LifeRecordItem(254, 351, 338, new string[6] { "Character", "Location", "Item", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 40, -1, ELifeRecordDisplayType.Crime));
		_dataArray.Add(new LifeRecordItem(255, 351, 339, new string[6] { "Character", "Location", "Item", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 40, -1, ELifeRecordDisplayType.Crime));
		_dataArray.Add(new LifeRecordItem(256, 351, 340, new string[6] { "Character", "Location", "Item", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 40, -1, ELifeRecordDisplayType.Crime));
		_dataArray.Add(new LifeRecordItem(257, 351, 341, new string[6] { "Character", "Location", "Item", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 40, -1, ELifeRecordDisplayType.Crime));
		_dataArray.Add(new LifeRecordItem(258, 351, 342, new string[6] { "Character", "Location", "Item", "", "", "" }, isSourceRecord: true, new List<short> { 261 }, -30000, ELifeRecordScoreType.Calculated, -1, -1, ELifeRecordDisplayType.Crime));
		_dataArray.Add(new LifeRecordItem(259, 351, 343, new string[6] { "Character", "Location", "Item", "", "", "" }, isSourceRecord: true, new List<short> { 261 }, -30000, ELifeRecordScoreType.Calculated, -1, -1, ELifeRecordDisplayType.Crime));
		_dataArray.Add(new LifeRecordItem(260, 351, 344, new string[6] { "Character", "Location", "Item", "", "", "" }, isSourceRecord: true, new List<short> { 262 }, -30000, ELifeRecordScoreType.Calculated, -1, -1, ELifeRecordDisplayType.Crime));
		_dataArray.Add(new LifeRecordItem(261, 351, 300, new string[6] { "", "", "", "", "", "" }, isSourceRecord: false, new List<short> { 258, 259 }, -30000, ELifeRecordScoreType.Calculated, -1, -1, ELifeRecordDisplayType.Crime));
		_dataArray.Add(new LifeRecordItem(262, 351, 352, new string[6] { "", "", "", "", "", "" }, isSourceRecord: false, new List<short> { 260 }, -30000, ELifeRecordScoreType.Calculated, -1, -1, ELifeRecordDisplayType.Crime));
		_dataArray.Add(new LifeRecordItem(263, 353, 354, new string[6] { "Character", "Location", "Resource", "Integer", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Calculated, -1, -1, ELifeRecordDisplayType.Crime));
		_dataArray.Add(new LifeRecordItem(264, 353, 355, new string[6] { "Character", "Location", "Resource", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 40, -1, ELifeRecordDisplayType.Crime));
		_dataArray.Add(new LifeRecordItem(265, 353, 356, new string[6] { "Character", "Location", "Item", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Calculated, -1, -1, ELifeRecordDisplayType.Crime));
		_dataArray.Add(new LifeRecordItem(266, 353, 355, new string[6] { "Character", "Location", "Item", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 40, -1, ELifeRecordDisplayType.Crime));
		_dataArray.Add(new LifeRecordItem(267, 357, 358, new string[6] { "Character", "Location", "Item", "Integer", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 40, -1, ELifeRecordDisplayType.Study));
		_dataArray.Add(new LifeRecordItem(268, 357, 359, new string[6] { "Character", "Location", "Item", "Integer", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 40, -1, ELifeRecordDisplayType.Study));
		_dataArray.Add(new LifeRecordItem(269, 357, 360, new string[6] { "Character", "Location", "Item", "Integer", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 40, -1, ELifeRecordDisplayType.Study));
		_dataArray.Add(new LifeRecordItem(270, 357, 361, new string[6] { "Character", "Location", "Item", "Integer", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 40, -1, ELifeRecordDisplayType.Study));
		_dataArray.Add(new LifeRecordItem(271, 357, 362, new string[6] { "Character", "Location", "Item", "Integer", "", "" }, isSourceRecord: true, new List<short> { 273 }, -30000, ELifeRecordScoreType.Calculated, -1, -1, ELifeRecordDisplayType.Study));
		_dataArray.Add(new LifeRecordItem(272, 357, 363, new string[6] { "Character", "Location", "Item", "Integer", "", "" }, isSourceRecord: true, new List<short> { 273 }, -30000, ELifeRecordScoreType.Calculated, -1, -1, ELifeRecordDisplayType.Study));
		_dataArray.Add(new LifeRecordItem(273, 357, 364, new string[6] { "", "", "", "", "", "" }, isSourceRecord: false, new List<short> { 271, 272 }, -30000, ELifeRecordScoreType.Calculated, -1, -1, ELifeRecordDisplayType.Study));
		_dataArray.Add(new LifeRecordItem(274, 365, 366, new string[6] { "Character", "Location", "Item", "Integer", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 40, -1, ELifeRecordDisplayType.Study));
		_dataArray.Add(new LifeRecordItem(275, 365, 367, new string[6] { "Character", "Location", "Item", "Integer", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 40, -1, ELifeRecordDisplayType.Study));
		_dataArray.Add(new LifeRecordItem(276, 365, 368, new string[6] { "Character", "Location", "Item", "Integer", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 40, -1, ELifeRecordDisplayType.Study));
		_dataArray.Add(new LifeRecordItem(277, 365, 369, new string[6] { "Character", "Location", "Item", "Integer", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 40, -1, ELifeRecordDisplayType.Study));
		_dataArray.Add(new LifeRecordItem(278, 365, 370, new string[6] { "Character", "Location", "Item", "Integer", "", "" }, isSourceRecord: true, new List<short> { 280 }, -30000, ELifeRecordScoreType.Calculated, -1, -1, ELifeRecordDisplayType.Study));
		_dataArray.Add(new LifeRecordItem(279, 365, 371, new string[6] { "Character", "Location", "Item", "Integer", "", "" }, isSourceRecord: true, new List<short> { 280 }, -30000, ELifeRecordScoreType.Calculated, -1, -1, ELifeRecordDisplayType.Study));
		_dataArray.Add(new LifeRecordItem(280, 365, 372, new string[6] { "", "", "", "", "", "" }, isSourceRecord: false, new List<short> { 278, 279 }, -30000, ELifeRecordScoreType.Calculated, -1, -1, ELifeRecordDisplayType.Study));
		_dataArray.Add(new LifeRecordItem(281, 373, 358, new string[6] { "Character", "Location", "Item", "Integer", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 40, -1, ELifeRecordDisplayType.Study));
		_dataArray.Add(new LifeRecordItem(282, 373, 359, new string[6] { "Character", "Location", "Item", "Integer", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 40, -1, ELifeRecordDisplayType.Study));
		_dataArray.Add(new LifeRecordItem(283, 373, 360, new string[6] { "Character", "Location", "Item", "Integer", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 40, -1, ELifeRecordDisplayType.Study));
		_dataArray.Add(new LifeRecordItem(284, 373, 361, new string[6] { "Character", "Location", "Item", "Integer", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 40, -1, ELifeRecordDisplayType.Study));
		_dataArray.Add(new LifeRecordItem(285, 373, 362, new string[6] { "Character", "Location", "Item", "Integer", "", "" }, isSourceRecord: true, new List<short> { 287 }, -30000, ELifeRecordScoreType.Calculated, -1, -1, ELifeRecordDisplayType.Study));
		_dataArray.Add(new LifeRecordItem(286, 373, 363, new string[6] { "Character", "Location", "Item", "Integer", "", "" }, isSourceRecord: true, new List<short> { 287 }, -30000, ELifeRecordScoreType.Calculated, -1, -1, ELifeRecordDisplayType.Study));
		_dataArray.Add(new LifeRecordItem(287, 373, 364, new string[6] { "", "", "", "", "", "" }, isSourceRecord: false, new List<short> { 285, 286 }, -30000, ELifeRecordScoreType.Calculated, -1, -1, ELifeRecordDisplayType.Study));
		_dataArray.Add(new LifeRecordItem(288, 374, 366, new string[6] { "Character", "Location", "Item", "Integer", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 40, -1, ELifeRecordDisplayType.Study));
		_dataArray.Add(new LifeRecordItem(289, 374, 367, new string[6] { "Character", "Location", "Item", "Integer", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 40, -1, ELifeRecordDisplayType.Study));
		_dataArray.Add(new LifeRecordItem(290, 374, 368, new string[6] { "Character", "Location", "Item", "Integer", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 40, -1, ELifeRecordDisplayType.Study));
		_dataArray.Add(new LifeRecordItem(291, 374, 369, new string[6] { "Character", "Location", "Item", "Integer", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 40, -1, ELifeRecordDisplayType.Study));
		_dataArray.Add(new LifeRecordItem(292, 374, 370, new string[6] { "Character", "Location", "Item", "Integer", "", "" }, isSourceRecord: true, new List<short> { 294 }, -30000, ELifeRecordScoreType.Calculated, -1, -1, ELifeRecordDisplayType.Study));
		_dataArray.Add(new LifeRecordItem(293, 374, 371, new string[6] { "Character", "Location", "Item", "Integer", "", "" }, isSourceRecord: true, new List<short> { 294 }, -30000, ELifeRecordScoreType.Calculated, -1, -1, ELifeRecordDisplayType.Study));
		_dataArray.Add(new LifeRecordItem(294, 374, 372, new string[6] { "", "", "", "", "", "" }, isSourceRecord: false, new List<short> { 292, 293 }, -30000, ELifeRecordScoreType.Calculated, -1, -1, ELifeRecordDisplayType.Study));
		_dataArray.Add(new LifeRecordItem(295, 375, 376, new string[6] { "Character", "Location", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 60, -1, ELifeRecordDisplayType.Normal));
		_dataArray.Add(new LifeRecordItem(296, 377, 378, new string[6] { "Character", "Location", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 40, -1, ELifeRecordDisplayType.Normal));
		_dataArray.Add(new LifeRecordItem(297, 379, 380, new string[6] { "Character", "Location", "Resource", "Integer", "Resource", "Integer" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 50, -1, ELifeRecordDisplayType.Normal));
		_dataArray.Add(new LifeRecordItem(298, 381, 382, new string[6] { "Character", "Location", "Resource", "Integer", "", "" }, isSourceRecord: true, new List<short> { 303 }, -30000, ELifeRecordScoreType.Normal, 50, -1, ELifeRecordDisplayType.Normal));
		_dataArray.Add(new LifeRecordItem(299, 383, 384, new string[6] { "Character", "Location", "Resource", "Integer", "Item", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 50, -1, ELifeRecordDisplayType.Normal));
	}

	private void CreateItems5()
	{
		_dataArray.Add(new LifeRecordItem(300, 385, 386, new string[6] { "Character", "Location", "Resource", "Integer", "Item", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 50, -1, ELifeRecordDisplayType.Normal));
		_dataArray.Add(new LifeRecordItem(301, 387, 388, new string[6] { "Character", "Location", "Item", "", "", "" }, isSourceRecord: true, new List<short> { 304 }, -30000, ELifeRecordScoreType.Normal, 50, -1, ELifeRecordDisplayType.Normal));
		_dataArray.Add(new LifeRecordItem(302, 387, 389, new string[6] { "Character", "Location", "Item", "", "", "" }, isSourceRecord: true, new List<short> { 305 }, -30000, ELifeRecordScoreType.Normal, 40, -1, ELifeRecordDisplayType.Normal));
		_dataArray.Add(new LifeRecordItem(303, 390, 391, new string[6] { "", "", "", "", "", "" }, isSourceRecord: false, new List<short> { 298 }, -30000, ELifeRecordScoreType.Calculated, -1, -1, ELifeRecordDisplayType.Normal));
		_dataArray.Add(new LifeRecordItem(304, 392, 393, new string[6] { "", "", "", "", "", "" }, isSourceRecord: false, new List<short> { 301 }, -30000, ELifeRecordScoreType.Calculated, -1, -1, ELifeRecordDisplayType.Normal));
		_dataArray.Add(new LifeRecordItem(305, 392, 394, new string[6] { "Character", "Location", "Item", "", "", "" }, isSourceRecord: true, new List<short> { 302 }, -30000, ELifeRecordScoreType.Normal, 40, -1, ELifeRecordDisplayType.Normal));
		_dataArray.Add(new LifeRecordItem(306, 395, 396, new string[6] { "", "", "", "", "", "" }, isSourceRecord: false, new List<short> { 308, 309 }, -30000, ELifeRecordScoreType.Normal, 50, -1, ELifeRecordDisplayType.Study));
		_dataArray.Add(new LifeRecordItem(307, 397, 396, new string[6] { "", "", "", "", "", "" }, isSourceRecord: false, new List<short> { 310, 311 }, -30000, ELifeRecordScoreType.Normal, 50, -1, ELifeRecordDisplayType.Study));
		_dataArray.Add(new LifeRecordItem(308, 398, 399, new string[6] { "Character", "Location", "Item", "Integer", "", "" }, isSourceRecord: true, new List<short> { 306 }, -30000, ELifeRecordScoreType.Calculated, -1, -1, ELifeRecordDisplayType.Study));
		_dataArray.Add(new LifeRecordItem(309, 398, 400, new string[6] { "Character", "Location", "Item", "Integer", "", "" }, isSourceRecord: true, new List<short> { 306 }, -30000, ELifeRecordScoreType.Calculated, -1, -1, ELifeRecordDisplayType.Study));
		_dataArray.Add(new LifeRecordItem(310, 398, 399, new string[6] { "Character", "Location", "Item", "Integer", "", "" }, isSourceRecord: true, new List<short> { 307 }, -30000, ELifeRecordScoreType.Calculated, -1, -1, ELifeRecordDisplayType.Study));
		_dataArray.Add(new LifeRecordItem(311, 398, 400, new string[6] { "Character", "Location", "Item", "Integer", "", "" }, isSourceRecord: true, new List<short> { 307 }, -30000, ELifeRecordScoreType.Calculated, -1, -1, ELifeRecordDisplayType.Study));
		_dataArray.Add(new LifeRecordItem(312, 401, 402, new string[6] { "Character", "Location", "Item", "", "", "" }, isSourceRecord: true, new List<short> { 327 }, -30000, ELifeRecordScoreType.Normal, 60, -1, ELifeRecordDisplayType.Normal));
		_dataArray.Add(new LifeRecordItem(313, 401, 403, new string[6] { "Character", "Location", "Item", "", "", "" }, isSourceRecord: true, new List<short> { 328 }, -30000, ELifeRecordScoreType.Normal, 40, -1, ELifeRecordDisplayType.Normal));
		_dataArray.Add(new LifeRecordItem(314, 404, 405, new string[6] { "Character", "Location", "Item", "", "", "" }, isSourceRecord: true, new List<short> { 329 }, -30000, ELifeRecordScoreType.Normal, 60, -1, ELifeRecordDisplayType.Normal));
		_dataArray.Add(new LifeRecordItem(315, 404, 406, new string[6] { "Character", "Location", "Item", "", "", "" }, isSourceRecord: true, new List<short> { 330 }, -30000, ELifeRecordScoreType.Normal, 40, -1, ELifeRecordDisplayType.Normal));
		_dataArray.Add(new LifeRecordItem(316, 407, 408, new string[6] { "Character", "Location", "", "", "", "" }, isSourceRecord: true, new List<short> { 331 }, -30000, ELifeRecordScoreType.Normal, 60, -1, ELifeRecordDisplayType.Normal));
		_dataArray.Add(new LifeRecordItem(317, 409, 410, new string[6] { "Character", "Location", "", "", "", "" }, isSourceRecord: true, new List<short> { 332 }, -30000, ELifeRecordScoreType.Normal, 60, -1, ELifeRecordDisplayType.Normal));
		_dataArray.Add(new LifeRecordItem(318, 411, 412, new string[6] { "Character", "Location", "", "", "", "" }, isSourceRecord: true, new List<short> { 333 }, -30000, ELifeRecordScoreType.Normal, 60, -1, ELifeRecordDisplayType.Normal));
		_dataArray.Add(new LifeRecordItem(319, 411, 413, new string[6] { "Character", "Location", "", "", "", "" }, isSourceRecord: true, new List<short> { 334 }, -30000, ELifeRecordScoreType.Normal, 40, -1, ELifeRecordDisplayType.Normal));
		_dataArray.Add(new LifeRecordItem(320, 411, 414, new string[6] { "Character", "Location", "", "", "", "" }, isSourceRecord: true, new List<short> { 335 }, -30000, ELifeRecordScoreType.Normal, 20, -1, ELifeRecordDisplayType.Normal));
		_dataArray.Add(new LifeRecordItem(321, 415, 416, new string[6] { "Character", "Location", "Resource", "Integer", "", "" }, isSourceRecord: true, new List<short> { 336 }, -30000, ELifeRecordScoreType.Normal, 60, -1, ELifeRecordDisplayType.Normal));
		_dataArray.Add(new LifeRecordItem(322, 415, 417, new string[6] { "Character", "Location", "Resource", "Integer", "", "" }, isSourceRecord: true, new List<short> { 337 }, -30000, ELifeRecordScoreType.Normal, 40, -1, ELifeRecordDisplayType.Normal));
		_dataArray.Add(new LifeRecordItem(323, 418, 419, new string[6] { "Character", "Location", "", "", "", "" }, isSourceRecord: true, new List<short> { 338 }, -30000, ELifeRecordScoreType.Calculated, -1, -1, ELifeRecordDisplayType.Normal));
		_dataArray.Add(new LifeRecordItem(324, 420, 421, new string[6] { "Character", "Location", "", "", "", "" }, isSourceRecord: true, new List<short> { 339 }, -30000, ELifeRecordScoreType.Calculated, -1, -1, ELifeRecordDisplayType.Normal));
		_dataArray.Add(new LifeRecordItem(325, 422, 423, new string[6] { "Character", "Location", "", "", "", "" }, isSourceRecord: true, new List<short> { 340 }, -30000, ELifeRecordScoreType.Calculated, -1, -1, ELifeRecordDisplayType.Normal));
		_dataArray.Add(new LifeRecordItem(326, 424, 425, new string[6] { "Character", "Location", "", "", "", "" }, isSourceRecord: true, new List<short> { 341 }, -30000, ELifeRecordScoreType.Calculated, -1, -1, ELifeRecordDisplayType.Normal));
		_dataArray.Add(new LifeRecordItem(327, 401, 426, new string[6] { "", "", "", "", "", "" }, isSourceRecord: false, new List<short> { 312 }, -30000, ELifeRecordScoreType.Normal, 60, -1, ELifeRecordDisplayType.Normal));
		_dataArray.Add(new LifeRecordItem(328, 401, 427, new string[6] { "", "", "", "", "", "" }, isSourceRecord: false, new List<short> { 313 }, -30000, ELifeRecordScoreType.Normal, 40, -1, ELifeRecordDisplayType.Normal));
		_dataArray.Add(new LifeRecordItem(329, 404, 428, new string[6] { "", "", "", "", "", "" }, isSourceRecord: false, new List<short> { 314 }, -30000, ELifeRecordScoreType.Normal, 60, -1, ELifeRecordDisplayType.Normal));
		_dataArray.Add(new LifeRecordItem(330, 404, 429, new string[6] { "", "", "", "", "", "" }, isSourceRecord: false, new List<short> { 315 }, -30000, ELifeRecordScoreType.Normal, 40, -1, ELifeRecordDisplayType.Normal));
		_dataArray.Add(new LifeRecordItem(331, 407, 430, new string[6] { "", "", "", "", "", "" }, isSourceRecord: false, new List<short> { 316 }, -30000, ELifeRecordScoreType.Normal, 60, -1, ELifeRecordDisplayType.Normal));
		_dataArray.Add(new LifeRecordItem(332, 409, 431, new string[6] { "", "", "", "", "", "" }, isSourceRecord: false, new List<short> { 317 }, -30000, ELifeRecordScoreType.Normal, 60, -1, ELifeRecordDisplayType.Normal));
		_dataArray.Add(new LifeRecordItem(333, 411, 432, new string[6] { "", "", "", "", "", "" }, isSourceRecord: false, new List<short> { 318 }, -30000, ELifeRecordScoreType.Normal, 60, -1, ELifeRecordDisplayType.Normal));
		_dataArray.Add(new LifeRecordItem(334, 411, 433, new string[6] { "", "", "", "", "", "" }, isSourceRecord: false, new List<short> { 319 }, -30000, ELifeRecordScoreType.Normal, 40, -1, ELifeRecordDisplayType.Normal));
		_dataArray.Add(new LifeRecordItem(335, 411, 434, new string[6] { "", "", "", "", "", "" }, isSourceRecord: false, new List<short> { 320 }, -30000, ELifeRecordScoreType.Normal, 20, -1, ELifeRecordDisplayType.Normal));
		_dataArray.Add(new LifeRecordItem(336, 435, 436, new string[6] { "", "", "", "", "", "" }, isSourceRecord: false, new List<short> { 321 }, -30000, ELifeRecordScoreType.Normal, 60, -1, ELifeRecordDisplayType.Normal));
		_dataArray.Add(new LifeRecordItem(337, 435, 437, new string[6] { "", "", "", "", "", "" }, isSourceRecord: false, new List<short> { 322 }, -30000, ELifeRecordScoreType.Normal, 40, -1, ELifeRecordDisplayType.Normal));
		_dataArray.Add(new LifeRecordItem(338, 438, 439, new string[6] { "", "", "", "", "", "" }, isSourceRecord: false, new List<short> { 323 }, -30000, ELifeRecordScoreType.Calculated, -1, -1, ELifeRecordDisplayType.Normal));
		_dataArray.Add(new LifeRecordItem(339, 420, 440, new string[6] { "", "", "", "", "", "" }, isSourceRecord: false, new List<short> { 324 }, -30000, ELifeRecordScoreType.Calculated, -1, -1, ELifeRecordDisplayType.Normal));
		_dataArray.Add(new LifeRecordItem(340, 441, 442, new string[6] { "", "", "", "", "", "" }, isSourceRecord: false, new List<short> { 325 }, -30000, ELifeRecordScoreType.Calculated, -1, -1, ELifeRecordDisplayType.Normal));
		_dataArray.Add(new LifeRecordItem(341, 443, 444, new string[6] { "", "", "", "", "", "" }, isSourceRecord: false, new List<short> { 326 }, -30000, ELifeRecordScoreType.Calculated, -1, -1, ELifeRecordDisplayType.Normal));
		_dataArray.Add(new LifeRecordItem(342, 445, 446, new string[6] { "Location", "Item", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Calculated, -1, -1, ELifeRecordDisplayType.Normal));
		_dataArray.Add(new LifeRecordItem(343, 447, 448, new string[6] { "Character", "Location", "", "", "", "" }, isSourceRecord: true, new List<short> { 347 }, -30000, ELifeRecordScoreType.Normal, 60, -1, ELifeRecordDisplayType.Study));
		_dataArray.Add(new LifeRecordItem(344, 447, 449, new string[6] { "Character", "Location", "", "", "", "" }, isSourceRecord: true, new List<short> { 348 }, -30000, ELifeRecordScoreType.Normal, 40, -1, ELifeRecordDisplayType.Study));
		_dataArray.Add(new LifeRecordItem(345, 450, 451, new string[6] { "Character", "Location", "", "", "", "" }, isSourceRecord: true, new List<short> { 349 }, -30000, ELifeRecordScoreType.Normal, 60, -1, ELifeRecordDisplayType.Study));
		_dataArray.Add(new LifeRecordItem(346, 450, 452, new string[6] { "Character", "Location", "", "", "", "" }, isSourceRecord: true, new List<short> { 350 }, -30000, ELifeRecordScoreType.Normal, 40, -1, ELifeRecordDisplayType.Study));
		_dataArray.Add(new LifeRecordItem(347, 453, 454, new string[6] { "", "", "", "", "", "" }, isSourceRecord: false, new List<short> { 343 }, -30000, ELifeRecordScoreType.Normal, 60, -1, ELifeRecordDisplayType.Study));
		_dataArray.Add(new LifeRecordItem(348, 453, 455, new string[6] { "", "", "", "", "", "" }, isSourceRecord: false, new List<short> { 344 }, -30000, ELifeRecordScoreType.Normal, 40, -1, ELifeRecordDisplayType.Study));
		_dataArray.Add(new LifeRecordItem(349, 456, 457, new string[6] { "", "", "", "", "", "" }, isSourceRecord: false, new List<short> { 345 }, -30000, ELifeRecordScoreType.Normal, 60, -1, ELifeRecordDisplayType.Study));
		_dataArray.Add(new LifeRecordItem(350, 456, 458, new string[6] { "", "", "", "", "", "" }, isSourceRecord: false, new List<short> { 346 }, -30000, ELifeRecordScoreType.Normal, 40, -1, ELifeRecordDisplayType.Study));
		_dataArray.Add(new LifeRecordItem(351, 459, 460, new string[6] { "Location", "Item", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 60, -1, ELifeRecordDisplayType.Normal));
		_dataArray.Add(new LifeRecordItem(352, 459, 461, new string[6] { "Location", "", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 40, -1, ELifeRecordDisplayType.Normal));
		_dataArray.Add(new LifeRecordItem(353, 462, 463, new string[6] { "Character", "Location", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 60, -1, ELifeRecordDisplayType.Normal));
		_dataArray.Add(new LifeRecordItem(354, 462, 464, new string[6] { "Location", "", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 40, -1, ELifeRecordDisplayType.Normal));
		_dataArray.Add(new LifeRecordItem(355, 465, 466, new string[6] { "Character", "Location", "", "", "", "" }, isSourceRecord: true, new List<short> { 356 }, -30000, ELifeRecordScoreType.Normal, 60, -1, ELifeRecordDisplayType.Normal));
		_dataArray.Add(new LifeRecordItem(356, 465, 467, new string[6] { "Character", "Location", "", "", "", "" }, isSourceRecord: true, new List<short> { 355 }, -30000, ELifeRecordScoreType.Normal, 40, -1, ELifeRecordDisplayType.Normal));
		_dataArray.Add(new LifeRecordItem(357, 468, 469, new string[6] { "Character", "Location", "", "", "", "" }, isSourceRecord: true, new List<short> { 357 }, -30000, ELifeRecordScoreType.Normal, 50, -1, ELifeRecordDisplayType.Relation));
		_dataArray.Add(new LifeRecordItem(358, 470, 471, new string[6] { "Character", "Location", "", "", "", "" }, isSourceRecord: true, new List<short> { 361 }, -30000, ELifeRecordScoreType.Normal, 0, -1, ELifeRecordDisplayType.Relation));
		_dataArray.Add(new LifeRecordItem(359, 470, 472, new string[6] { "Character", "Location", "", "", "", "" }, isSourceRecord: true, new List<short> { 362 }, -30000, ELifeRecordScoreType.Normal, 0, -1, ELifeRecordDisplayType.Relation));
	}

	private void CreateItems6()
	{
		_dataArray.Add(new LifeRecordItem(360, 473, 474, new string[6] { "Character", "Location", "", "", "", "" }, isSourceRecord: true, new List<short> { 363 }, -30000, ELifeRecordScoreType.Normal, 70, -1, ELifeRecordDisplayType.Relation));
		_dataArray.Add(new LifeRecordItem(361, 470, 475, new string[6] { "", "", "", "", "", "" }, isSourceRecord: false, new List<short> { 358 }, -30000, ELifeRecordScoreType.Normal, 50, -1, ELifeRecordDisplayType.Relation));
		_dataArray.Add(new LifeRecordItem(362, 470, 476, new string[6] { "", "", "", "", "", "" }, isSourceRecord: false, new List<short> { 359 }, -30000, ELifeRecordScoreType.Absolute, 0, -1, ELifeRecordDisplayType.Relation));
		_dataArray.Add(new LifeRecordItem(363, 473, 477, new string[6] { "", "", "", "", "", "" }, isSourceRecord: false, new List<short> { 360 }, -30000, ELifeRecordScoreType.Normal, 70, -1, ELifeRecordDisplayType.Relation));
		_dataArray.Add(new LifeRecordItem(364, 478, 479, new string[6] { "Location", "", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 50, -1, ELifeRecordDisplayType.Normal));
		_dataArray.Add(new LifeRecordItem(365, 480, 481, new string[6] { "Location", "Resource", "Integer", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 70, -1, ELifeRecordDisplayType.Normal));
		_dataArray.Add(new LifeRecordItem(366, 480, 482, new string[6] { "Location", "Item", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 70, -1, ELifeRecordDisplayType.Normal));
		_dataArray.Add(new LifeRecordItem(367, 480, 483, new string[6] { "Location", "Item", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 70, -1, ELifeRecordDisplayType.Normal));
		_dataArray.Add(new LifeRecordItem(368, 484, 485, new string[6] { "Location", "Integer", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 70, -1, ELifeRecordDisplayType.Normal));
		_dataArray.Add(new LifeRecordItem(369, 484, 486, new string[6] { "Location", "Integer", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 70, -1, ELifeRecordDisplayType.Normal));
		_dataArray.Add(new LifeRecordItem(370, 484, 487, new string[6] { "Location", "Integer", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 70, -1, ELifeRecordDisplayType.Normal));
		_dataArray.Add(new LifeRecordItem(371, 484, 488, new string[6] { "Location", "PoisonType", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 70, -1, ELifeRecordDisplayType.Normal));
		_dataArray.Add(new LifeRecordItem(372, 484, 489, new string[6] { "Location", "", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 70, -1, ELifeRecordDisplayType.Normal));
		_dataArray.Add(new LifeRecordItem(373, 490, 491, new string[6] { "Location", "Resource", "Integer", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 30, -1, ELifeRecordDisplayType.Normal));
		_dataArray.Add(new LifeRecordItem(374, 490, 492, new string[6] { "Location", "Item", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 30, -1, ELifeRecordDisplayType.Normal));
		_dataArray.Add(new LifeRecordItem(375, 490, 493, new string[6] { "Location", "Item", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 30, -1, ELifeRecordDisplayType.Normal));
		_dataArray.Add(new LifeRecordItem(376, 494, 495, new string[6] { "Location", "Integer", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 30, -1, ELifeRecordDisplayType.Normal));
		_dataArray.Add(new LifeRecordItem(377, 494, 496, new string[6] { "Location", "Integer", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 30, -1, ELifeRecordDisplayType.Normal));
		_dataArray.Add(new LifeRecordItem(378, 494, 497, new string[6] { "Location", "Integer", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 30, -1, ELifeRecordDisplayType.Normal));
		_dataArray.Add(new LifeRecordItem(379, 494, 498, new string[6] { "Location", "PoisonType", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 30, -1, ELifeRecordDisplayType.Normal));
		_dataArray.Add(new LifeRecordItem(380, 494, 499, new string[6] { "Location", "", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 30, -1, ELifeRecordDisplayType.Normal));
		_dataArray.Add(new LifeRecordItem(381, 500, 501, new string[6] { "Location", "CharacterTemplate", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 50, -1, ELifeRecordDisplayType.Combat));
		_dataArray.Add(new LifeRecordItem(382, 502, 503, new string[6] { "Location", "CharacterTemplate", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 50, -1, ELifeRecordDisplayType.Combat));
		_dataArray.Add(new LifeRecordItem(383, 504, 505, new string[6] { "Location", "CharacterTemplate", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 40, -1, ELifeRecordDisplayType.Combat));
		_dataArray.Add(new LifeRecordItem(384, 506, 507, new string[6] { "Location", "CharacterTemplate", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 40, -1, ELifeRecordDisplayType.Combat));
		_dataArray.Add(new LifeRecordItem(385, 508, 509, new string[6] { "Location", "", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Absolute, 20, -1, ELifeRecordDisplayType.Negative));
		_dataArray.Add(new LifeRecordItem(386, 510, 511, new string[6] { "Location", "", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Absolute, 20, -1, ELifeRecordDisplayType.Great));
		_dataArray.Add(new LifeRecordItem(387, 512, 513, new string[6] { "Location", "", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Absolute, 20, -1, ELifeRecordDisplayType.Negative));
		_dataArray.Add(new LifeRecordItem(388, 514, 515, new string[6] { "Location", "", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Absolute, 20, -1, ELifeRecordDisplayType.Relation));
		_dataArray.Add(new LifeRecordItem(389, 516, 517, new string[6] { "Location", "", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Absolute, 20, -1, ELifeRecordDisplayType.Negative));
		_dataArray.Add(new LifeRecordItem(390, 518, 519, new string[6] { "Location", "", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Absolute, 20, -1, ELifeRecordDisplayType.Negative));
		_dataArray.Add(new LifeRecordItem(391, 520, 521, new string[6] { "Location", "", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Absolute, 20, -1, ELifeRecordDisplayType.Negative));
		_dataArray.Add(new LifeRecordItem(392, 522, 523, new string[6] { "Location", "", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Absolute, 20, -1, ELifeRecordDisplayType.Great));
		_dataArray.Add(new LifeRecordItem(393, 524, 525, new string[6] { "Location", "", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Absolute, 80, -1, ELifeRecordDisplayType.Normal));
		_dataArray.Add(new LifeRecordItem(394, 526, 527, new string[6] { "Location", "", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Absolute, 80, -1, ELifeRecordDisplayType.Great));
		_dataArray.Add(new LifeRecordItem(395, 528, 529, new string[6] { "Location", "", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Absolute, 80, -1, ELifeRecordDisplayType.Normal));
		_dataArray.Add(new LifeRecordItem(396, 530, 531, new string[6] { "Location", "", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Absolute, 80, -1, ELifeRecordDisplayType.Relation));
		_dataArray.Add(new LifeRecordItem(397, 532, 533, new string[6] { "Location", "", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Absolute, 80, -1, ELifeRecordDisplayType.Normal));
		_dataArray.Add(new LifeRecordItem(398, 534, 535, new string[6] { "Location", "", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Absolute, 80, -1, ELifeRecordDisplayType.Normal));
		_dataArray.Add(new LifeRecordItem(399, 536, 537, new string[6] { "Location", "", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Absolute, 80, -1, ELifeRecordDisplayType.Normal));
		_dataArray.Add(new LifeRecordItem(400, 538, 539, new string[6] { "Location", "", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Absolute, 80, -1, ELifeRecordDisplayType.Normal));
		_dataArray.Add(new LifeRecordItem(401, 540, 541, new string[6] { "Location", "", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Absolute, 0, -1, ELifeRecordDisplayType.Great));
		_dataArray.Add(new LifeRecordItem(402, 542, 543, new string[6] { "Location", "", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Absolute, 20, -1, ELifeRecordDisplayType.Great));
		_dataArray.Add(new LifeRecordItem(403, 544, 545, new string[6] { "Location", "", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Absolute, 40, -1, ELifeRecordDisplayType.Great));
		_dataArray.Add(new LifeRecordItem(404, 546, 547, new string[6] { "Location", "", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Absolute, 50, -1, ELifeRecordDisplayType.Great));
		_dataArray.Add(new LifeRecordItem(405, 548, 549, new string[6] { "Location", "", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Absolute, 70, -1, ELifeRecordDisplayType.Great));
		_dataArray.Add(new LifeRecordItem(406, 550, 551, new string[6] { "Location", "", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Absolute, 90, -1, ELifeRecordDisplayType.Great));
		_dataArray.Add(new LifeRecordItem(407, 552, 553, new string[6] { "Character", "Integer", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 60, -1, ELifeRecordDisplayType.Relation));
		_dataArray.Add(new LifeRecordItem(408, 554, 555, new string[6] { "", "", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 20, -1, ELifeRecordDisplayType.Relation));
		_dataArray.Add(new LifeRecordItem(409, 554, 556, new string[6] { "", "", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 20, -1, ELifeRecordDisplayType.Relation));
		_dataArray.Add(new LifeRecordItem(410, 557, 558, new string[6] { "Character", "Location", "", "", "", "" }, isSourceRecord: true, new List<short> { 413 }, -30000, ELifeRecordScoreType.Normal, 30, -1, ELifeRecordDisplayType.Relation));
		_dataArray.Add(new LifeRecordItem(411, 559, 560, new string[6] { "Character", "Location", "", "", "", "" }, isSourceRecord: true, new List<short> { 691 }, -30000, ELifeRecordScoreType.Absolute, 80, -1, ELifeRecordDisplayType.Relation));
		_dataArray.Add(new LifeRecordItem(412, 561, 562, new string[6] { "Location", "OrgGrade", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 70, -1, ELifeRecordDisplayType.Relation));
		_dataArray.Add(new LifeRecordItem(413, 557, 563, new string[6] { "", "", "", "", "", "" }, isSourceRecord: false, new List<short> { 410 }, -30000, ELifeRecordScoreType.Normal, 30, -1, ELifeRecordDisplayType.Relation));
		_dataArray.Add(new LifeRecordItem(414, 564, 565, new string[6] { "Character", "Location", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 20, 5, ELifeRecordDisplayType.Relation));
		_dataArray.Add(new LifeRecordItem(415, 566, 567, new string[6] { "Character", "", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 80, 6, ELifeRecordDisplayType.Relation));
		_dataArray.Add(new LifeRecordItem(416, 568, 569, new string[6] { "Location", "Location", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 50, -1, ELifeRecordDisplayType.Normal));
		_dataArray.Add(new LifeRecordItem(417, 570, 571, new string[6] { "Character", "Location", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 90, -1, ELifeRecordDisplayType.Relation));
		_dataArray.Add(new LifeRecordItem(418, 572, 573, new string[6] { "Character", "Location", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 90, -1, ELifeRecordDisplayType.Relation));
		_dataArray.Add(new LifeRecordItem(419, 574, 575, new string[6] { "Location", "", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 30, -1, ELifeRecordDisplayType.Normal));
	}

	private void CreateItems7()
	{
		_dataArray.Add(new LifeRecordItem(420, 576, 577, new string[6] { "Location", "Settlement", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 50, -1, ELifeRecordDisplayType.Study));
		_dataArray.Add(new LifeRecordItem(421, 578, 579, new string[6] { "Location", "Settlement", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 50, -1, ELifeRecordDisplayType.Study));
		_dataArray.Add(new LifeRecordItem(422, 580, 581, new string[6] { "Location", "Settlement", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 60, -1, ELifeRecordDisplayType.Study));
		_dataArray.Add(new LifeRecordItem(423, 582, 583, new string[6] { "Location", "Settlement", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 50, -1, ELifeRecordDisplayType.Study));
		_dataArray.Add(new LifeRecordItem(424, 584, 585, new string[6] { "Location", "", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 80, -1, ELifeRecordDisplayType.Study));
		_dataArray.Add(new LifeRecordItem(425, 586, 587, new string[6] { "Location", "", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 80, -1, ELifeRecordDisplayType.Study));
		_dataArray.Add(new LifeRecordItem(426, 588, 589, new string[6] { "Location", "Settlement", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 60, -1, ELifeRecordDisplayType.Study));
		_dataArray.Add(new LifeRecordItem(427, 590, 591, new string[6] { "Location", "", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Absolute, 0, -1, ELifeRecordDisplayType.Study));
		_dataArray.Add(new LifeRecordItem(428, 592, 593, new string[6] { "Location", "Settlement", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 60, -1, ELifeRecordDisplayType.Study));
		_dataArray.Add(new LifeRecordItem(429, 594, 595, new string[6] { "Location", "", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Absolute, 0, -1, ELifeRecordDisplayType.Great));
		_dataArray.Add(new LifeRecordItem(430, 596, 597, new string[6] { "Location", "", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Absolute, 0, -1, ELifeRecordDisplayType.Great));
		_dataArray.Add(new LifeRecordItem(431, 598, 599, new string[6] { "Location", "", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 20, -1, ELifeRecordDisplayType.Great));
		_dataArray.Add(new LifeRecordItem(432, 600, 601, new string[6] { "Location", "", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Absolute, 80, -1, ELifeRecordDisplayType.Great));
		_dataArray.Add(new LifeRecordItem(433, 602, 603, new string[6] { "Location", "Character", "ItemSubType", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 50, -1, ELifeRecordDisplayType.Normal));
		_dataArray.Add(new LifeRecordItem(434, 604, 605, new string[6] { "Location", "Character", "ItemSubType", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 50, -1, ELifeRecordDisplayType.Normal));
		_dataArray.Add(new LifeRecordItem(435, 606, 607, new string[6] { "Location", "Character", "Settlement", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 50, -1, ELifeRecordDisplayType.Normal));
		_dataArray.Add(new LifeRecordItem(436, 608, 609, new string[6] { "Location", "Character", "BehaviorType", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 50, -1, ELifeRecordDisplayType.Normal));
		_dataArray.Add(new LifeRecordItem(437, 610, 611, new string[6] { "Location", "Character", "LifeSkillType", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 50, -1, ELifeRecordDisplayType.Normal));
		_dataArray.Add(new LifeRecordItem(438, 612, 613, new string[6] { "Location", "Character", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 40, -1, ELifeRecordDisplayType.Crime));
		_dataArray.Add(new LifeRecordItem(439, 353, 614, new string[6] { "Character", "Location", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 40, -1, ELifeRecordDisplayType.Crime));
		_dataArray.Add(new LifeRecordItem(440, 615, 616, new string[6] { "PunishmentType", "", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 40, -1, ELifeRecordDisplayType.Great));
		_dataArray.Add(new LifeRecordItem(441, 617, 618, new string[6] { "PunishmentType", "OrgGrade", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 30, -1, ELifeRecordDisplayType.Great));
		_dataArray.Add(new LifeRecordItem(442, 619, 620, new string[6] { "PunishmentType", "", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Absolute, 0, -1, ELifeRecordDisplayType.Great));
		_dataArray.Add(new LifeRecordItem(443, 617, 621, new string[6] { "PunishmentType", "OrgGrade", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 30, -1, ELifeRecordDisplayType.Great));
		_dataArray.Add(new LifeRecordItem(444, 619, 622, new string[6] { "PunishmentType", "", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Absolute, 0, -1, ELifeRecordDisplayType.Great));
		_dataArray.Add(new LifeRecordItem(445, 623, 624, new string[6] { "Character", "Location", "Character", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 40, -1, ELifeRecordDisplayType.Relation));
		_dataArray.Add(new LifeRecordItem(446, 625, 626, new string[6] { "Character", "Location", "Character", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 40, -1, ELifeRecordDisplayType.Relation));
		_dataArray.Add(new LifeRecordItem(447, 627, 628, new string[6] { "Character", "Location", "Character", "", "", "" }, isSourceRecord: true, new List<short> { 461, 462 }, -30000, ELifeRecordScoreType.Normal, 40, -1, ELifeRecordDisplayType.Relation));
		_dataArray.Add(new LifeRecordItem(448, 627, 629, new string[6] { "Character", "Location", "Character", "", "", "" }, isSourceRecord: true, new List<short> { 461, 462 }, -30000, ELifeRecordScoreType.Normal, 40, -1, ELifeRecordDisplayType.Relation));
		_dataArray.Add(new LifeRecordItem(449, 630, 631, new string[6] { "Character", "Location", "Character", "", "", "" }, isSourceRecord: true, new List<short> { 459, 460 }, -30000, ELifeRecordScoreType.Normal, 40, -1, ELifeRecordDisplayType.Relation));
		_dataArray.Add(new LifeRecordItem(450, 630, 632, new string[6] { "Character", "Location", "Character", "", "", "" }, isSourceRecord: true, new List<short> { 459, 460 }, -30000, ELifeRecordScoreType.Normal, 40, -1, ELifeRecordDisplayType.Relation));
		_dataArray.Add(new LifeRecordItem(451, 633, 634, new string[6] { "Character", "Location", "Character", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 40, -1, ELifeRecordDisplayType.Relation));
		_dataArray.Add(new LifeRecordItem(452, 635, 636, new string[6] { "Character", "Location", "Character", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 40, -1, ELifeRecordDisplayType.Relation));
		_dataArray.Add(new LifeRecordItem(453, 637, 638, new string[6] { "Character", "Location", "Character", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 40, -1, ELifeRecordDisplayType.Relation));
		_dataArray.Add(new LifeRecordItem(454, 639, 640, new string[6] { "Character", "Location", "Character", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 40, -1, ELifeRecordDisplayType.Relation));
		_dataArray.Add(new LifeRecordItem(455, 641, 642, new string[6] { "Character", "Location", "Character", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 40, -1, ELifeRecordDisplayType.Relation));
		_dataArray.Add(new LifeRecordItem(456, 641, 643, new string[6] { "Character", "Location", "Character", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 40, -1, ELifeRecordDisplayType.Relation));
		_dataArray.Add(new LifeRecordItem(457, 644, 645, new string[6] { "Character", "Location", "Character", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 40, -1, ELifeRecordDisplayType.Relation));
		_dataArray.Add(new LifeRecordItem(458, 644, 646, new string[6] { "Character", "Location", "Character", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 40, -1, ELifeRecordDisplayType.Relation));
		_dataArray.Add(new LifeRecordItem(459, 81, 82, new string[6] { "", "", "", "", "", "" }, isSourceRecord: false, new List<short> { 449, 450 }, -30000, ELifeRecordScoreType.Normal, 70, -1, ELifeRecordDisplayType.Relation));
		_dataArray.Add(new LifeRecordItem(460, 83, 84, new string[6] { "", "", "", "", "", "" }, isSourceRecord: false, new List<short> { 449, 450 }, -30000, ELifeRecordScoreType.Normal, 70, -1, ELifeRecordDisplayType.Relation));
		_dataArray.Add(new LifeRecordItem(461, 85, 86, new string[6] { "", "", "", "", "", "" }, isSourceRecord: false, new List<short> { 447, 448 }, -30000, ELifeRecordScoreType.Normal, 70, -1, ELifeRecordDisplayType.Relation));
		_dataArray.Add(new LifeRecordItem(462, 87, 88, new string[6] { "", "", "", "", "", "" }, isSourceRecord: false, new List<short> { 447, 448 }, -30000, ELifeRecordScoreType.Normal, 70, -1, ELifeRecordDisplayType.Relation));
		_dataArray.Add(new LifeRecordItem(463, 647, 648, new string[6] { "Character", "Location", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 40, -1, ELifeRecordDisplayType.Relation));
		_dataArray.Add(new LifeRecordItem(464, 121, 649, new string[6] { "Location", "Adventure", "Settlement", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 50, -1, ELifeRecordDisplayType.Normal));
		_dataArray.Add(new LifeRecordItem(465, 650, 651, new string[6] { "Location", "Adventure", "CharacterTitle", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 70, -1, ELifeRecordDisplayType.Study));
		_dataArray.Add(new LifeRecordItem(466, 652, 653, new string[6] { "Character", "Location", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 40, 16, ELifeRecordDisplayType.Relation));
		_dataArray.Add(new LifeRecordItem(467, 654, 655, new string[6] { "Character", "Location", "", "", "", "" }, isSourceRecord: true, new List<short> { 468 }, -30000, ELifeRecordScoreType.Normal, 60, -1, ELifeRecordDisplayType.Relation));
		_dataArray.Add(new LifeRecordItem(468, 656, 657, new string[6] { "Character", "Location", "", "", "", "" }, isSourceRecord: true, new List<short> { 467 }, -30000, ELifeRecordScoreType.Normal, 60, -1, ELifeRecordDisplayType.Relation));
		_dataArray.Add(new LifeRecordItem(469, 658, 659, new string[6] { "Character", "Location", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 40, 12, ELifeRecordDisplayType.Relation));
		_dataArray.Add(new LifeRecordItem(470, 660, 661, new string[6] { "Character", "Location", "", "", "", "" }, isSourceRecord: true, new List<short> { 469 }, -30000, ELifeRecordScoreType.Normal, 40, 11, ELifeRecordDisplayType.Relation));
		_dataArray.Add(new LifeRecordItem(471, 662, 663, new string[6] { "Character", "Location", "", "", "", "" }, isSourceRecord: true, new List<short> { 472 }, -30000, ELifeRecordScoreType.Normal, 40, -1, ELifeRecordDisplayType.Relation));
		_dataArray.Add(new LifeRecordItem(472, 664, 665, new string[6] { "Character", "Location", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 40, -1, ELifeRecordDisplayType.Relation));
		_dataArray.Add(new LifeRecordItem(473, 666, 667, new string[6] { "Character", "Location", "", "", "", "" }, isSourceRecord: true, new List<short> { 473 }, -30000, ELifeRecordScoreType.Normal, 40, 13, ELifeRecordDisplayType.Relation));
		_dataArray.Add(new LifeRecordItem(474, 668, 669, new string[6] { "Character", "Location", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 40, -1, ELifeRecordDisplayType.Relation));
		_dataArray.Add(new LifeRecordItem(475, 670, 671, new string[6] { "Character", "Location", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 60, -1, ELifeRecordDisplayType.Relation));
		_dataArray.Add(new LifeRecordItem(476, 672, 673, new string[6] { "Character", "Location", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 60, -1, ELifeRecordDisplayType.Relation));
		_dataArray.Add(new LifeRecordItem(477, 672, 674, new string[6] { "Character", "Location", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 60, -1, ELifeRecordDisplayType.Relation));
		_dataArray.Add(new LifeRecordItem(478, 675, 676, new string[6] { "Character", "Location", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 40, -1, ELifeRecordDisplayType.Relation));
		_dataArray.Add(new LifeRecordItem(479, 672, 677, new string[6] { "Character", "Location", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 40, -1, ELifeRecordDisplayType.Relation));
	}

	private void CreateItems8()
	{
		_dataArray.Add(new LifeRecordItem(480, 678, 679, new string[6] { "Location", "Item", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 70, -1, ELifeRecordDisplayType.Study));
		_dataArray.Add(new LifeRecordItem(481, 680, 681, new string[6] { "Location", "Item", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 70, -1, ELifeRecordDisplayType.Study));
		_dataArray.Add(new LifeRecordItem(482, 682, 683, new string[6] { "Location", "", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 30, -1, ELifeRecordDisplayType.Great));
		_dataArray.Add(new LifeRecordItem(483, 684, 685, new string[6] { "Location", "Item", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Absolute, 0, -1, ELifeRecordDisplayType.Great));
		_dataArray.Add(new LifeRecordItem(484, 686, 687, new string[6] { "Location", "Item", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Absolute, 0, -1, ELifeRecordDisplayType.Great));
		_dataArray.Add(new LifeRecordItem(485, 4, 688, new string[6] { "Location", "", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Absolute, 0, -1, ELifeRecordDisplayType.Great));
		_dataArray.Add(new LifeRecordItem(486, 689, 690, new string[6] { "Character", "Location", "Item", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 50, -1, ELifeRecordDisplayType.Study));
		_dataArray.Add(new LifeRecordItem(487, 691, 692, new string[6] { "Location", "", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 50, -1, ELifeRecordDisplayType.Study));
		_dataArray.Add(new LifeRecordItem(488, 693, 694, new string[6] { "Character", "Location", "Item", "", "", "" }, isSourceRecord: true, new List<short> { 491 }, -30000, ELifeRecordScoreType.Normal, 60, -1, ELifeRecordDisplayType.Study));
		_dataArray.Add(new LifeRecordItem(489, 693, 695, new string[6] { "Character", "Location", "Item", "", "", "" }, isSourceRecord: true, new List<short> { 490 }, -30000, ELifeRecordScoreType.Normal, 40, -1, ELifeRecordDisplayType.Study));
		_dataArray.Add(new LifeRecordItem(490, 693, 696, new string[6] { "", "", "", "", "", "" }, isSourceRecord: false, new List<short> { 489 }, -30000, ELifeRecordScoreType.Normal, 60, -1, ELifeRecordDisplayType.Study));
		_dataArray.Add(new LifeRecordItem(491, 693, 697, new string[6] { "", "", "", "", "", "" }, isSourceRecord: false, new List<short> { 488 }, -30000, ELifeRecordScoreType.Normal, 40, -1, ELifeRecordDisplayType.Study));
		_dataArray.Add(new LifeRecordItem(492, 693, 698, new string[6] { "Character", "Location", "Item", "", "", "" }, isSourceRecord: true, new List<short> { 493 }, -30000, ELifeRecordScoreType.Normal, 50, -1, ELifeRecordDisplayType.Study));
		_dataArray.Add(new LifeRecordItem(493, 693, 699, new string[6] { "", "", "", "", "", "" }, isSourceRecord: false, new List<short> { 492 }, -30000, ELifeRecordScoreType.Normal, 50, -1, ELifeRecordDisplayType.Study));
		_dataArray.Add(new LifeRecordItem(494, 693, 702, new string[6] { "Character", "Location", "Item", "", "", "" }, isSourceRecord: true, new List<short> { 495 }, -30000, ELifeRecordScoreType.Normal, 50, -1, ELifeRecordDisplayType.Study));
		_dataArray.Add(new LifeRecordItem(495, 693, 703, new string[6] { "", "", "", "", "", "" }, isSourceRecord: false, new List<short> { 494 }, -30000, ELifeRecordScoreType.Normal, 50, -1, ELifeRecordDisplayType.Study));
		_dataArray.Add(new LifeRecordItem(496, 704, 705, new string[6] { "Character", "Location", "Item", "", "", "" }, isSourceRecord: true, new List<short> { 497 }, -30000, ELifeRecordScoreType.Normal, 50, -1, ELifeRecordDisplayType.Study));
		_dataArray.Add(new LifeRecordItem(497, 704, 706, new string[6] { "", "", "", "", "", "" }, isSourceRecord: false, new List<short> { 496 }, -30000, ELifeRecordScoreType.Normal, 60, -1, ELifeRecordDisplayType.Study));
		_dataArray.Add(new LifeRecordItem(498, 704, 707, new string[6] { "Character", "Location", "Item", "", "", "" }, isSourceRecord: true, new List<short> { 499 }, -30000, ELifeRecordScoreType.Normal, 50, -1, ELifeRecordDisplayType.Study));
		_dataArray.Add(new LifeRecordItem(499, 704, 708, new string[6] { "", "", "", "", "", "" }, isSourceRecord: false, new List<short> { 498 }, -30000, ELifeRecordScoreType.Normal, 40, -1, ELifeRecordDisplayType.Study));
		_dataArray.Add(new LifeRecordItem(500, 709, 710, new string[6] { "Character", "Location", "Item", "Resource", "Integer", "" }, isSourceRecord: true, new List<short> { 501 }, -30000, ELifeRecordScoreType.Normal, 50, -1, ELifeRecordDisplayType.Study));
		_dataArray.Add(new LifeRecordItem(501, 709, 711, new string[6] { "", "", "", "", "", "" }, isSourceRecord: false, new List<short> { 500 }, -30000, ELifeRecordScoreType.Normal, 60, -1, ELifeRecordDisplayType.Study));
		_dataArray.Add(new LifeRecordItem(502, 709, 712, new string[6] { "Character", "Location", "Item", "", "", "" }, isSourceRecord: true, new List<short> { 503 }, -30000, ELifeRecordScoreType.Normal, 50, -1, ELifeRecordDisplayType.Study));
		_dataArray.Add(new LifeRecordItem(503, 709, 713, new string[6] { "", "", "", "", "", "" }, isSourceRecord: false, new List<short> { 502 }, -30000, ELifeRecordScoreType.Normal, 40, -1, ELifeRecordDisplayType.Study));
		_dataArray.Add(new LifeRecordItem(504, 714, 715, new string[6] { "Character", "Location", "Item", "", "", "" }, isSourceRecord: true, new List<short> { 505 }, -30000, ELifeRecordScoreType.Normal, 40, -1, ELifeRecordDisplayType.Study));
		_dataArray.Add(new LifeRecordItem(505, 714, 716, new string[6] { "", "", "", "", "", "" }, isSourceRecord: false, new List<short> { 504 }, -30000, ELifeRecordScoreType.Normal, 50, -1, ELifeRecordDisplayType.Study));
		_dataArray.Add(new LifeRecordItem(506, 717, 718, new string[6] { "Character", "Location", "Item", "", "", "" }, isSourceRecord: true, new List<short> { 511 }, -30000, ELifeRecordScoreType.Normal, 70, 3, ELifeRecordDisplayType.Combat));
		_dataArray.Add(new LifeRecordItem(507, 717, 719, new string[6] { "Character", "Location", "", "", "", "" }, isSourceRecord: true, new List<short> { 512 }, -30000, ELifeRecordScoreType.Normal, 70, 7, ELifeRecordDisplayType.Combat));
		_dataArray.Add(new LifeRecordItem(508, 717, 720, new string[6] { "Character", "Location", "", "", "", "" }, isSourceRecord: true, new List<short> { 513 }, -30000, ELifeRecordScoreType.Normal, 70, 8, ELifeRecordDisplayType.Combat));
		_dataArray.Add(new LifeRecordItem(509, 717, 721, new string[6] { "Character", "Location", "", "", "", "" }, isSourceRecord: true, new List<short> { 514 }, -30000, ELifeRecordScoreType.Normal, 70, 10, ELifeRecordDisplayType.Combat));
		_dataArray.Add(new LifeRecordItem(510, 717, 722, new string[6] { "Character", "Location", "Item", "", "", "" }, isSourceRecord: true, new List<short> { 515 }, -30000, ELifeRecordScoreType.Normal, 70, 2, ELifeRecordDisplayType.Combat));
		_dataArray.Add(new LifeRecordItem(511, 717, 723, new string[6] { "", "", "", "", "", "" }, isSourceRecord: false, new List<short> { 506 }, -30000, ELifeRecordScoreType.Normal, 30, -1, ELifeRecordDisplayType.Combat));
		_dataArray.Add(new LifeRecordItem(512, 717, 724, new string[6] { "", "", "", "", "", "" }, isSourceRecord: false, new List<short> { 507 }, -30000, ELifeRecordScoreType.Normal, 30, -1, ELifeRecordDisplayType.Combat));
		_dataArray.Add(new LifeRecordItem(513, 717, 725, new string[6] { "", "", "", "", "", "" }, isSourceRecord: false, new List<short> { 508 }, -30000, ELifeRecordScoreType.Normal, 30, -1, ELifeRecordDisplayType.Combat));
		_dataArray.Add(new LifeRecordItem(514, 717, 726, new string[6] { "", "", "", "", "", "" }, isSourceRecord: false, new List<short> { 509 }, -30000, ELifeRecordScoreType.Normal, 30, -1, ELifeRecordDisplayType.Combat));
		_dataArray.Add(new LifeRecordItem(515, 717, 727, new string[6] { "", "", "", "", "", "" }, isSourceRecord: false, new List<short> { 510 }, -30000, ELifeRecordScoreType.Normal, 30, -1, ELifeRecordDisplayType.Combat));
		_dataArray.Add(new LifeRecordItem(516, 728, 729, new string[6] { "Character", "Location", "", "", "", "" }, isSourceRecord: true, new List<short> { 517 }, -30000, ELifeRecordScoreType.Normal, 50, -1, ELifeRecordDisplayType.Combat));
		_dataArray.Add(new LifeRecordItem(517, 728, 730, new string[6] { "", "", "", "", "", "" }, isSourceRecord: false, new List<short> { 516 }, -30000, ELifeRecordScoreType.Normal, 50, -1, ELifeRecordDisplayType.Combat));
		_dataArray.Add(new LifeRecordItem(518, 728, 731, new string[6] { "Character", "Location", "", "", "", "" }, isSourceRecord: true, new List<short> { 519 }, -30000, ELifeRecordScoreType.Normal, 50, -1, ELifeRecordDisplayType.Combat));
		_dataArray.Add(new LifeRecordItem(519, 728, 732, new string[6] { "", "", "", "", "", "" }, isSourceRecord: false, new List<short> { 518 }, -30000, ELifeRecordScoreType.Normal, 50, -1, ELifeRecordDisplayType.Combat));
		_dataArray.Add(new LifeRecordItem(520, 733, 734, new string[6] { "Character", "Location", "", "", "", "" }, isSourceRecord: true, new List<short> { 521 }, -30000, ELifeRecordScoreType.Normal, 70, 1, ELifeRecordDisplayType.Combat));
		_dataArray.Add(new LifeRecordItem(521, 733, 735, new string[6] { "", "", "", "", "", "" }, isSourceRecord: false, new List<short> { 520 }, -30000, ELifeRecordScoreType.Normal, 30, -1, ELifeRecordDisplayType.Combat));
		_dataArray.Add(new LifeRecordItem(522, 709, 736, new string[6] { "Character", "Location", "Item", "Integer", "", "" }, isSourceRecord: true, new List<short> { 523 }, -30000, ELifeRecordScoreType.Normal, 50, -1, ELifeRecordDisplayType.Great));
		_dataArray.Add(new LifeRecordItem(523, 709, 737, new string[6] { "", "", "", "", "", "" }, isSourceRecord: false, new List<short> { 522 }, -30000, ELifeRecordScoreType.Normal, 60, -1, ELifeRecordDisplayType.Great));
		_dataArray.Add(new LifeRecordItem(524, 693, 700, new string[6] { "Character", "Location", "Item", "", "", "" }, isSourceRecord: true, new List<short> { 525 }, -30000, ELifeRecordScoreType.Normal, 50, -1, ELifeRecordDisplayType.Study));
		_dataArray.Add(new LifeRecordItem(525, 693, 701, new string[6] { "", "", "", "", "", "" }, isSourceRecord: false, new List<short> { 524 }, -30000, ELifeRecordScoreType.Normal, 50, -1, ELifeRecordDisplayType.Study));
		_dataArray.Add(new LifeRecordItem(526, 738, 739, new string[6] { "Location", "Item", "Settlement", "OrgGrade", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 50, -1, ELifeRecordDisplayType.Great));
		_dataArray.Add(new LifeRecordItem(527, 740, 741, new string[6] { "Character", "Location", "", "", "", "" }, isSourceRecord: true, new List<short> { 528 }, -30000, ELifeRecordScoreType.Normal, 49, -1, ELifeRecordDisplayType.Relation));
		_dataArray.Add(new LifeRecordItem(528, 740, 742, new string[6] { "", "", "", "", "", "" }, isSourceRecord: false, new List<short> { 527 }, -30000, ELifeRecordScoreType.Normal, 50, -1, ELifeRecordDisplayType.Relation));
		_dataArray.Add(new LifeRecordItem(529, 743, 744, new string[6] { "Character", "Location", "", "", "", "" }, isSourceRecord: true, new List<short> { 531 }, -30000, ELifeRecordScoreType.Normal, 80, -1, ELifeRecordDisplayType.Relation));
		_dataArray.Add(new LifeRecordItem(530, 743, 745, new string[6] { "Character", "Location", "", "", "", "" }, isSourceRecord: true, new List<short> { 532 }, -30000, ELifeRecordScoreType.Normal, 40, -1, ELifeRecordDisplayType.Relation));
		_dataArray.Add(new LifeRecordItem(531, 743, 746, new string[6] { "", "", "", "", "", "" }, isSourceRecord: false, new List<short> { 529 }, -30000, ELifeRecordScoreType.Normal, 80, -1, ELifeRecordDisplayType.Relation));
		_dataArray.Add(new LifeRecordItem(532, 743, 747, new string[6] { "", "", "", "", "", "" }, isSourceRecord: false, new List<short> { 530 }, -30000, ELifeRecordScoreType.Normal, 40, -1, ELifeRecordDisplayType.Relation));
		_dataArray.Add(new LifeRecordItem(533, 748, 749, new string[6] { "Character", "Location", "", "", "", "" }, isSourceRecord: true, new List<short> { 534 }, -30000, ELifeRecordScoreType.Normal, 70, -1, ELifeRecordDisplayType.Relation));
		_dataArray.Add(new LifeRecordItem(534, 748, 750, new string[6] { "", "", "", "", "", "" }, isSourceRecord: false, new List<short> { 533 }, -30000, ELifeRecordScoreType.Normal, 70, -1, ELifeRecordDisplayType.Relation));
		_dataArray.Add(new LifeRecordItem(535, 751, 752, new string[6] { "Character", "Location", "Item", "", "", "" }, isSourceRecord: true, new List<short> { 536 }, -30000, ELifeRecordScoreType.Normal, 80, -1, ELifeRecordDisplayType.Relation));
		_dataArray.Add(new LifeRecordItem(536, 751, 753, new string[6] { "", "", "", "", "", "" }, isSourceRecord: false, new List<short> { 535 }, -30000, ELifeRecordScoreType.Normal, 80, -1, ELifeRecordDisplayType.Relation));
		_dataArray.Add(new LifeRecordItem(537, 754, 755, new string[6] { "Character", "Location", "", "", "", "" }, isSourceRecord: true, new List<short> { 538 }, -30000, ELifeRecordScoreType.Normal, 60, -1, ELifeRecordDisplayType.Relation));
		_dataArray.Add(new LifeRecordItem(538, 754, 756, new string[6] { "", "", "", "", "", "" }, isSourceRecord: false, new List<short> { 537 }, -30000, ELifeRecordScoreType.Normal, 60, -1, ELifeRecordDisplayType.Relation));
		_dataArray.Add(new LifeRecordItem(539, 757, 758, new string[6] { "Character", "Location", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 90, -1, ELifeRecordDisplayType.Relation));
	}

	private void CreateItems9()
	{
		_dataArray.Add(new LifeRecordItem(540, 759, 760, new string[6] { "Character", "Location", "", "", "", "" }, isSourceRecord: true, new List<short> { 541 }, -30000, ELifeRecordScoreType.Normal, 60, -1, ELifeRecordDisplayType.Relation));
		_dataArray.Add(new LifeRecordItem(541, 759, 761, new string[6] { "", "", "", "", "", "" }, isSourceRecord: false, new List<short> { 540 }, -30000, ELifeRecordScoreType.Normal, 60, -1, ELifeRecordDisplayType.Relation));
		_dataArray.Add(new LifeRecordItem(542, 762, 763, new string[6] { "Character", "Location", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 60, -1, ELifeRecordDisplayType.Relation));
		_dataArray.Add(new LifeRecordItem(543, 764, 765, new string[6] { "Character", "Location", "", "", "", "" }, isSourceRecord: true, new List<short> { 544 }, -30000, ELifeRecordScoreType.Normal, 30, -1, ELifeRecordDisplayType.Relation));
		_dataArray.Add(new LifeRecordItem(544, 764, 766, new string[6] { "", "", "", "", "", "" }, isSourceRecord: false, new List<short> { 543 }, -30000, ELifeRecordScoreType.Normal, 30, -1, ELifeRecordDisplayType.Relation));
		_dataArray.Add(new LifeRecordItem(545, 767, 766, new string[6] { "Character", "Location", "", "", "", "" }, isSourceRecord: true, new List<short> { 546 }, -30000, ELifeRecordScoreType.Normal, 60, -1, ELifeRecordDisplayType.Relation));
		_dataArray.Add(new LifeRecordItem(546, 767, 768, new string[6] { "", "", "", "", "", "" }, isSourceRecord: false, new List<short> { 545 }, -30000, ELifeRecordScoreType.Normal, 60, -1, ELifeRecordDisplayType.Relation));
		_dataArray.Add(new LifeRecordItem(547, 769, 770, new string[6] { "Character", "Location", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 60, -1, ELifeRecordDisplayType.Normal));
		_dataArray.Add(new LifeRecordItem(548, 769, 771, new string[6] { "Character", "Location", "", "", "", "" }, isSourceRecord: true, new List<short> { 549 }, -30000, ELifeRecordScoreType.Normal, 60, -1, ELifeRecordDisplayType.Normal));
		_dataArray.Add(new LifeRecordItem(549, 769, 772, new string[6] { "", "", "", "", "", "" }, isSourceRecord: false, new List<short> { 548 }, -30000, ELifeRecordScoreType.Normal, 40, -1, ELifeRecordDisplayType.Normal));
		_dataArray.Add(new LifeRecordItem(550, 769, 773, new string[6] { "Character", "Location", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 40, -1, ELifeRecordDisplayType.Normal));
		_dataArray.Add(new LifeRecordItem(551, 774, 775, new string[6] { "Character", "Location", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 60, -1, ELifeRecordDisplayType.Normal));
		_dataArray.Add(new LifeRecordItem(552, 774, 776, new string[6] { "Character", "Location", "", "", "", "" }, isSourceRecord: true, new List<short> { 553 }, -30000, ELifeRecordScoreType.Normal, 60, -1, ELifeRecordDisplayType.Normal));
		_dataArray.Add(new LifeRecordItem(553, 774, 777, new string[6] { "", "", "", "", "", "" }, isSourceRecord: false, new List<short> { 552 }, -30000, ELifeRecordScoreType.Normal, 40, -1, ELifeRecordDisplayType.Normal));
		_dataArray.Add(new LifeRecordItem(554, 774, 778, new string[6] { "Character", "Location", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 40, -1, ELifeRecordDisplayType.Normal));
		_dataArray.Add(new LifeRecordItem(555, 779, 780, new string[6] { "Character", "Location", "Character", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 60, -1, ELifeRecordDisplayType.Normal));
		_dataArray.Add(new LifeRecordItem(556, 781, 782, new string[6] { "Location", "LifeSkillType", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 60, -1, ELifeRecordDisplayType.Normal));
		_dataArray.Add(new LifeRecordItem(557, 783, 784, new string[6] { "Location", "CombatSkillType", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 60, -1, ELifeRecordDisplayType.Normal));
		_dataArray.Add(new LifeRecordItem(558, 785, 786, new string[6] { "Character", "Location", "", "", "", "" }, isSourceRecord: true, new List<short> { 559 }, -30000, ELifeRecordScoreType.Normal, 60, -1, ELifeRecordDisplayType.Normal));
		_dataArray.Add(new LifeRecordItem(559, 785, 787, new string[6] { "", "", "", "", "", "" }, isSourceRecord: false, new List<short> { 558 }, -30000, ELifeRecordScoreType.Normal, 60, -1, ELifeRecordDisplayType.Normal));
		_dataArray.Add(new LifeRecordItem(560, 788, 789, new string[6] { "Character", "Location", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 60, -1, ELifeRecordDisplayType.Normal));
		_dataArray.Add(new LifeRecordItem(561, 790, 791, new string[6] { "Character", "Location", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 50, -1, ELifeRecordDisplayType.Normal));
		_dataArray.Add(new LifeRecordItem(562, 792, 793, new string[6] { "Character", "Location", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 60, -1, ELifeRecordDisplayType.Normal));
		_dataArray.Add(new LifeRecordItem(563, 794, 795, new string[6] { "Location", "", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 60, -1, ELifeRecordDisplayType.Normal));
		_dataArray.Add(new LifeRecordItem(564, 796, 797, new string[6] { "Character", "Location", "", "", "", "" }, isSourceRecord: true, new List<short> { 565 }, -30000, ELifeRecordScoreType.Normal, 60, -1, ELifeRecordDisplayType.Normal));
		_dataArray.Add(new LifeRecordItem(565, 796, 798, new string[6] { "", "", "", "", "", "" }, isSourceRecord: false, new List<short> { 564 }, -30000, ELifeRecordScoreType.Normal, 60, -1, ELifeRecordDisplayType.Normal));
		_dataArray.Add(new LifeRecordItem(566, 799, 800, new string[6] { "Location", "", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 60, -1, ELifeRecordDisplayType.Normal));
		_dataArray.Add(new LifeRecordItem(567, 801, 802, new string[6] { "Character", "Location", "", "", "", "" }, isSourceRecord: true, new List<short> { 568 }, -30000, ELifeRecordScoreType.Normal, 50, -1, ELifeRecordDisplayType.Normal));
		_dataArray.Add(new LifeRecordItem(568, 801, 803, new string[6] { "", "", "", "", "", "" }, isSourceRecord: false, new List<short> { 567 }, -30000, ELifeRecordScoreType.Normal, 60, -1, ELifeRecordDisplayType.Normal));
		_dataArray.Add(new LifeRecordItem(569, 804, 805, new string[6] { "Character", "Location", "Integer", "Item", "", "" }, isSourceRecord: true, new List<short> { 570 }, -30000, ELifeRecordScoreType.Normal, 60, -1, ELifeRecordDisplayType.Normal));
		_dataArray.Add(new LifeRecordItem(570, 804, 806, new string[6] { "", "", "", "", "", "" }, isSourceRecord: false, new List<short> { 569 }, -30000, ELifeRecordScoreType.Normal, 60, -1, ELifeRecordDisplayType.Normal));
		_dataArray.Add(new LifeRecordItem(571, 807, 808, new string[6] { "Location", "Integer", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 50, -1, ELifeRecordDisplayType.Normal));
		_dataArray.Add(new LifeRecordItem(572, 809, 810, new string[6] { "Character", "Location", "", "", "", "" }, isSourceRecord: true, new List<short> { 573 }, -30000, ELifeRecordScoreType.Normal, 40, -1, ELifeRecordDisplayType.Normal));
		_dataArray.Add(new LifeRecordItem(573, 809, 811, new string[6] { "", "", "", "", "", "" }, isSourceRecord: false, new List<short> { 572 }, -30000, ELifeRecordScoreType.Normal, 50, -1, ELifeRecordDisplayType.Normal));
		_dataArray.Add(new LifeRecordItem(574, 812, 813, new string[6] { "Character", "Location", "", "", "", "" }, isSourceRecord: true, new List<short> { 575 }, -30000, ELifeRecordScoreType.Normal, 60, -1, ELifeRecordDisplayType.Normal));
		_dataArray.Add(new LifeRecordItem(575, 812, 814, new string[6] { "", "", "", "", "", "" }, isSourceRecord: false, new List<short> { 574 }, -30000, ELifeRecordScoreType.Normal, 60, -1, ELifeRecordDisplayType.Normal));
		_dataArray.Add(new LifeRecordItem(576, 812, 815, new string[6] { "Character", "Location", "", "", "", "" }, isSourceRecord: true, new List<short> { 577 }, -30000, ELifeRecordScoreType.Normal, 50, -1, ELifeRecordDisplayType.Normal));
		_dataArray.Add(new LifeRecordItem(577, 812, 816, new string[6] { "", "", "", "", "", "" }, isSourceRecord: false, new List<short> { 576 }, -30000, ELifeRecordScoreType.Normal, 40, -1, ELifeRecordDisplayType.Normal));
		_dataArray.Add(new LifeRecordItem(578, 817, 818, new string[6] { "Character", "Location", "", "", "", "" }, isSourceRecord: true, new List<short> { 579 }, -30000, ELifeRecordScoreType.Normal, 70, -1, ELifeRecordDisplayType.Normal));
		_dataArray.Add(new LifeRecordItem(579, 817, 819, new string[6] { "", "", "", "", "", "" }, isSourceRecord: false, new List<short> { 578 }, -30000, ELifeRecordScoreType.Normal, 70, -1, ELifeRecordDisplayType.Normal));
		_dataArray.Add(new LifeRecordItem(580, 820, 821, new string[6] { "Character", "Location", "", "", "", "" }, isSourceRecord: true, new List<short> { 581 }, -30000, ELifeRecordScoreType.Normal, 60, -1, ELifeRecordDisplayType.Normal));
		_dataArray.Add(new LifeRecordItem(581, 820, 822, new string[6] { "", "", "", "", "", "" }, isSourceRecord: false, new List<short> { 580 }, -30000, ELifeRecordScoreType.Normal, 60, -1, ELifeRecordDisplayType.Normal));
		_dataArray.Add(new LifeRecordItem(582, 820, 823, new string[6] { "Character", "Location", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 40, -1, ELifeRecordDisplayType.Normal));
		_dataArray.Add(new LifeRecordItem(583, 824, 825, new string[6] { "Location", "", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 60, -1, ELifeRecordDisplayType.Normal));
		_dataArray.Add(new LifeRecordItem(584, 826, 827, new string[6] { "Character", "Location", "", "", "", "" }, isSourceRecord: true, new List<short> { 585 }, -30000, ELifeRecordScoreType.Normal, 60, -1, ELifeRecordDisplayType.Normal));
		_dataArray.Add(new LifeRecordItem(585, 826, 828, new string[6] { "", "", "", "", "", "" }, isSourceRecord: false, new List<short> { 584 }, -30000, ELifeRecordScoreType.Normal, 60, -1, ELifeRecordDisplayType.Normal));
		_dataArray.Add(new LifeRecordItem(586, 830, 831, new string[6] { "Location", "", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Absolute, 90, -1, ELifeRecordDisplayType.Great));
		_dataArray.Add(new LifeRecordItem(587, 830, 832, new string[6] { "Location", "", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Absolute, 0, -1, ELifeRecordDisplayType.Great));
		_dataArray.Add(new LifeRecordItem(588, 830, 833, new string[6] { "Location", "", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Absolute, 30, -1, ELifeRecordDisplayType.Great));
		_dataArray.Add(new LifeRecordItem(589, 830, 834, new string[6] { "Location", "", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Absolute, 70, -1, ELifeRecordDisplayType.Normal));
		_dataArray.Add(new LifeRecordItem(590, 835, 836, new string[6] { "Character", "Location", "", "", "", "" }, isSourceRecord: true, new List<short> { 591 }, -30000, ELifeRecordScoreType.Normal, 60, -1, ELifeRecordDisplayType.Normal));
		_dataArray.Add(new LifeRecordItem(591, 835, 837, new string[6] { "", "", "", "", "", "" }, isSourceRecord: false, new List<short> { 590 }, -30000, ELifeRecordScoreType.Normal, 60, -1, ELifeRecordDisplayType.Normal));
		_dataArray.Add(new LifeRecordItem(592, 835, 838, new string[6] { "Character", "Location", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 40, -1, ELifeRecordDisplayType.Normal));
		_dataArray.Add(new LifeRecordItem(593, 839, 840, new string[6] { "Location", "", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 60, -1, ELifeRecordDisplayType.Normal));
		_dataArray.Add(new LifeRecordItem(594, 841, 842, new string[6] { "Location", "CombatSkill", "LifeSkill", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 60, -1, ELifeRecordDisplayType.Normal));
		_dataArray.Add(new LifeRecordItem(595, 841, 843, new string[6] { "Location", "CombatSkill", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 60, -1, ELifeRecordDisplayType.Normal));
		_dataArray.Add(new LifeRecordItem(596, 841, 843, new string[6] { "Location", "LifeSkill", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 60, -1, ELifeRecordDisplayType.Normal));
		_dataArray.Add(new LifeRecordItem(597, 845, 846, new string[6] { "Character", "Location", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 60, -1, ELifeRecordDisplayType.Normal));
		_dataArray.Add(new LifeRecordItem(598, 845, 847, new string[6] { "Character", "Location", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 40, -1, ELifeRecordDisplayType.Normal));
		_dataArray.Add(new LifeRecordItem(599, 848, 849, new string[6] { "Character", "Location", "", "", "", "" }, isSourceRecord: true, new List<short> { 600 }, -30000, ELifeRecordScoreType.Normal, 50, -1, ELifeRecordDisplayType.Normal));
	}

	private void CreateItems10()
	{
		_dataArray.Add(new LifeRecordItem(600, 848, 850, new string[6] { "", "", "", "", "", "" }, isSourceRecord: false, new List<short> { 599 }, -30000, ELifeRecordScoreType.Normal, 50, -1, ELifeRecordDisplayType.Normal));
		_dataArray.Add(new LifeRecordItem(601, 851, 852, new string[6] { "Character", "Location", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Absolute, 90, -1, ELifeRecordDisplayType.Great));
		_dataArray.Add(new LifeRecordItem(602, 851, 853, new string[6] { "Location", "", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Absolute, 30, -1, ELifeRecordDisplayType.Great));
		_dataArray.Add(new LifeRecordItem(603, 851, 854, new string[6] { "Location", "", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Absolute, 70, -1, ELifeRecordDisplayType.Great));
		_dataArray.Add(new LifeRecordItem(604, 826, 829, new string[6] { "Character", "Location", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 40, -1, ELifeRecordDisplayType.Normal));
		_dataArray.Add(new LifeRecordItem(605, 841, 844, new string[6] { "Location", "Integer", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 60, -1, ELifeRecordDisplayType.Normal));
		_dataArray.Add(new LifeRecordItem(606, 855, 856, new string[6] { "Character", "Location", "Settlement", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Absolute, 90, -1, ELifeRecordDisplayType.Great));
		_dataArray.Add(new LifeRecordItem(607, 855, 857, new string[6] { "Location", "", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Absolute, 90, -1, ELifeRecordDisplayType.Great));
		_dataArray.Add(new LifeRecordItem(608, 858, 859, new string[6] { "Location", "", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 30, -1, ELifeRecordDisplayType.Negative));
		_dataArray.Add(new LifeRecordItem(609, 860, 861, new string[6] { "Location", "", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 30, -1, ELifeRecordDisplayType.Negative));
		_dataArray.Add(new LifeRecordItem(610, 862, 863, new string[6] { "Location", "Item", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 30, -1, ELifeRecordDisplayType.Negative));
		_dataArray.Add(new LifeRecordItem(611, 864, 865, new string[6] { "Location", "", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 30, -1, ELifeRecordDisplayType.Negative));
		_dataArray.Add(new LifeRecordItem(612, 866, 867, new string[6] { "Location", "", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 30, -1, ELifeRecordDisplayType.Negative));
		_dataArray.Add(new LifeRecordItem(613, 868, 869, new string[6] { "Location", "", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 30, -1, ELifeRecordDisplayType.Negative));
		_dataArray.Add(new LifeRecordItem(614, 870, 871, new string[6] { "Location", "", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 30, -1, ELifeRecordDisplayType.Negative));
		_dataArray.Add(new LifeRecordItem(615, 872, 873, new string[6] { "Location", "", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 30, -1, ELifeRecordDisplayType.Negative));
		_dataArray.Add(new LifeRecordItem(616, 874, 875, new string[6] { "Location", "", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 30, -1, ELifeRecordDisplayType.Negative));
		_dataArray.Add(new LifeRecordItem(617, 876, 877, new string[6] { "Location", "", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 30, -1, ELifeRecordDisplayType.Negative));
		_dataArray.Add(new LifeRecordItem(618, 878, 879, new string[6] { "Location", "", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 30, -1, ELifeRecordDisplayType.Negative));
		_dataArray.Add(new LifeRecordItem(619, 880, 881, new string[6] { "Location", "", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 30, -1, ELifeRecordDisplayType.Negative));
		_dataArray.Add(new LifeRecordItem(620, 882, 883, new string[6] { "Location", "", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 30, -1, ELifeRecordDisplayType.Negative));
		_dataArray.Add(new LifeRecordItem(621, 884, 885, new string[6] { "Location", "", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 30, -1, ELifeRecordDisplayType.Negative));
		_dataArray.Add(new LifeRecordItem(622, 886, 887, new string[6] { "Location", "", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 30, -1, ELifeRecordDisplayType.Negative));
		_dataArray.Add(new LifeRecordItem(623, 888, 889, new string[6] { "Location", "", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 30, -1, ELifeRecordDisplayType.Negative));
		_dataArray.Add(new LifeRecordItem(624, 890, 891, new string[6] { "Location", "", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 30, -1, ELifeRecordDisplayType.Negative));
		_dataArray.Add(new LifeRecordItem(625, 892, 893, new string[6] { "Location", "", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 30, -1, ELifeRecordDisplayType.Negative));
		_dataArray.Add(new LifeRecordItem(626, 894, 895, new string[6] { "Location", "", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 30, -1, ELifeRecordDisplayType.Negative));
		_dataArray.Add(new LifeRecordItem(627, 896, 897, new string[6] { "Location", "", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 30, -1, ELifeRecordDisplayType.Negative));
		_dataArray.Add(new LifeRecordItem(628, 898, 899, new string[6] { "Location", "", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Invalid, -1, -1, ELifeRecordDisplayType.Combat));
		_dataArray.Add(new LifeRecordItem(629, 900, 901, new string[6] { "Location", "Item", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Invalid, -1, -1, ELifeRecordDisplayType.Combat));
		_dataArray.Add(new LifeRecordItem(630, 902, 903, new string[6] { "Location", "", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 30, -1, ELifeRecordDisplayType.Negative));
		_dataArray.Add(new LifeRecordItem(631, 904, 905, new string[6] { "Location", "Item", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Absolute, 60, -1, ELifeRecordDisplayType.Normal));
		_dataArray.Add(new LifeRecordItem(632, 906, 907, new string[6] { "Location", "", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Invalid, -1, -1, ELifeRecordDisplayType.Relation));
		_dataArray.Add(new LifeRecordItem(633, 908, 909, new string[6] { "Location", "", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Invalid, -1, -1, ELifeRecordDisplayType.Negative));
		_dataArray.Add(new LifeRecordItem(634, 910, 911, new string[6] { "Location", "", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Invalid, -1, -1, ELifeRecordDisplayType.Combat));
		_dataArray.Add(new LifeRecordItem(635, 910, 912, new string[6] { "Location", "", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Invalid, -1, -1, ELifeRecordDisplayType.Combat));
		_dataArray.Add(new LifeRecordItem(636, 910, 913, new string[6] { "Location", "", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Invalid, -1, -1, ELifeRecordDisplayType.Combat));
		_dataArray.Add(new LifeRecordItem(637, 914, 915, new string[6] { "", "", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 40, -1, ELifeRecordDisplayType.Great));
		_dataArray.Add(new LifeRecordItem(638, 916, 917, new string[6] { "", "", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Absolute, 60, -1, ELifeRecordDisplayType.Combat));
		_dataArray.Add(new LifeRecordItem(639, 918, 919, new string[6] { "Location", "", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Absolute, 30, -1, ELifeRecordDisplayType.Negative));
		_dataArray.Add(new LifeRecordItem(640, 920, 921, new string[6] { "Location", "Character", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Absolute, 40, -1, ELifeRecordDisplayType.Crime));
		_dataArray.Add(new LifeRecordItem(641, 920, 922, new string[6] { "Location", "Character", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Absolute, 40, -1, ELifeRecordDisplayType.Crime));
		_dataArray.Add(new LifeRecordItem(642, 923, 924, new string[6] { "Location", "", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Absolute, 60, -1, ELifeRecordDisplayType.Normal));
		_dataArray.Add(new LifeRecordItem(643, 925, 926, new string[6] { "Location", "", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Invalid, -1, -1, ELifeRecordDisplayType.Crime));
		_dataArray.Add(new LifeRecordItem(644, 927, 928, new string[6] { "Location", "", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Invalid, -1, -1, ELifeRecordDisplayType.Great));
		_dataArray.Add(new LifeRecordItem(645, 929, 930, new string[6] { "Settlement", "OrgGrade", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Invalid, -1, -1, ELifeRecordDisplayType.Study));
		_dataArray.Add(new LifeRecordItem(646, 606, 931, new string[6] { "Location", "Character", "Settlement", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 50, -1, ELifeRecordDisplayType.Normal));
		_dataArray.Add(new LifeRecordItem(647, 932, 933, new string[6] { "Location", "", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 80, -1, ELifeRecordDisplayType.Great));
		_dataArray.Add(new LifeRecordItem(648, 934, 935, new string[6] { "Character", "Location", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Invalid, -1, -1, ELifeRecordDisplayType.Study));
		_dataArray.Add(new LifeRecordItem(649, 936, 937, new string[6] { "Character", "Location", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Invalid, -1, -1, ELifeRecordDisplayType.Combat));
		_dataArray.Add(new LifeRecordItem(650, 938, 939, new string[6] { "Character", "Location", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Invalid, -1, -1, ELifeRecordDisplayType.Combat));
		_dataArray.Add(new LifeRecordItem(651, 940, 941, new string[6] { "Location", "", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 50, -1, ELifeRecordDisplayType.Normal));
		_dataArray.Add(new LifeRecordItem(652, 942, 943, new string[6] { "Location", "Item", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 50, -1, ELifeRecordDisplayType.Normal));
		_dataArray.Add(new LifeRecordItem(653, 944, 945, new string[6] { "Character", "", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Absolute, 0, -1, ELifeRecordDisplayType.Relation));
		_dataArray.Add(new LifeRecordItem(654, 946, 947, new string[6] { "Character", "Location", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 50, -1, ELifeRecordDisplayType.Relation));
		_dataArray.Add(new LifeRecordItem(655, 948, 949, new string[6] { "Character", "Location", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 40, -1, ELifeRecordDisplayType.Relation));
		_dataArray.Add(new LifeRecordItem(656, 950, 951, new string[6] { "Character", "Location", "", "", "", "" }, isSourceRecord: true, new List<short> { 657 }, -30000, ELifeRecordScoreType.Normal, 60, -1, ELifeRecordDisplayType.Relation));
		_dataArray.Add(new LifeRecordItem(657, 952, 953, new string[6] { "", "", "", "", "", "" }, isSourceRecord: false, new List<short> { 656 }, -30000, ELifeRecordScoreType.Absolute, 80, -1, ELifeRecordDisplayType.Relation));
		_dataArray.Add(new LifeRecordItem(658, 954, 955, new string[6] { "Character", "Location", "", "", "", "" }, isSourceRecord: true, new List<short> { 659 }, -30000, ELifeRecordScoreType.Absolute, 80, -1, ELifeRecordDisplayType.Relation));
		_dataArray.Add(new LifeRecordItem(659, 954, 956, new string[6] { "", "", "", "", "", "" }, isSourceRecord: false, new List<short> { 658 }, -30000, ELifeRecordScoreType.Absolute, 80, -1, ELifeRecordDisplayType.Relation));
	}

	private void CreateItems11()
	{
		_dataArray.Add(new LifeRecordItem(660, 957, 958, new string[6] { "Location", "", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 50, -1, ELifeRecordDisplayType.Great));
		_dataArray.Add(new LifeRecordItem(661, 959, 960, new string[6] { "", "", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 40, -1, ELifeRecordDisplayType.Normal));
		_dataArray.Add(new LifeRecordItem(662, 961, 962, new string[6] { "Location", "CharacterTemplate", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 50, -1, ELifeRecordDisplayType.Combat));
		_dataArray.Add(new LifeRecordItem(663, 963, 964, new string[6] { "Location", "CharacterTemplate", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 40, -1, ELifeRecordDisplayType.Combat));
		_dataArray.Add(new LifeRecordItem(664, 965, 966, new string[6] { "Location", "Adventure", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 50, -1, ELifeRecordDisplayType.Great));
		_dataArray.Add(new LifeRecordItem(665, 967, 968, new string[6] { "Location", "Adventure", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 50, -1, ELifeRecordDisplayType.Great));
		_dataArray.Add(new LifeRecordItem(666, 969, 970, new string[6] { "Character", "Location", "Adventure", "", "", "" }, isSourceRecord: true, new List<short> { 692 }, -30000, ELifeRecordScoreType.Normal, 60, -1, ELifeRecordDisplayType.Great));
		_dataArray.Add(new LifeRecordItem(667, 971, 972, new string[6] { "Location", "SecretInformation", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 50, -1, ELifeRecordDisplayType.Relation));
		_dataArray.Add(new LifeRecordItem(668, 971, 973, new string[6] { "Location", "SecretInformation", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 50, -1, ELifeRecordDisplayType.Relation));
		_dataArray.Add(new LifeRecordItem(669, 971, 974, new string[6] { "Location", "SecretInformation", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 50, -1, ELifeRecordDisplayType.Relation));
		_dataArray.Add(new LifeRecordItem(670, 971, 975, new string[6] { "Location", "SecretInformation", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 50, -1, ELifeRecordDisplayType.Relation));
		_dataArray.Add(new LifeRecordItem(671, 976, 977, new string[6] { "Character", "Location", "SecretInformation", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 40, -1, ELifeRecordDisplayType.Relation));
		_dataArray.Add(new LifeRecordItem(672, 978, 979, new string[6] { "Character", "Location", "SecretInformation", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 60, -1, ELifeRecordDisplayType.Relation));
		_dataArray.Add(new LifeRecordItem(673, 980, 981, new string[6] { "Character", "Location", "SecretInformation", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 50, -1, ELifeRecordDisplayType.Relation));
		_dataArray.Add(new LifeRecordItem(674, 980, 982, new string[6] { "Character", "Location", "SecretInformation", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 50, -1, ELifeRecordDisplayType.Relation));
		_dataArray.Add(new LifeRecordItem(675, 980, 983, new string[6] { "Character", "Location", "SecretInformation", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 50, -1, ELifeRecordDisplayType.Relation));
		_dataArray.Add(new LifeRecordItem(676, 980, 984, new string[6] { "Character", "Location", "SecretInformation", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 50, -1, ELifeRecordDisplayType.Relation));
		_dataArray.Add(new LifeRecordItem(677, 985, 986, new string[6] { "Character", "Location", "SecretInformation", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 40, -1, ELifeRecordDisplayType.Relation));
		_dataArray.Add(new LifeRecordItem(678, 987, 988, new string[6] { "Character", "Location", "SecretInformation", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 60, -1, ELifeRecordDisplayType.Relation));
		_dataArray.Add(new LifeRecordItem(679, 989, 990, new string[6] { "Character", "Location", "SecretInformation", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 50, -1, ELifeRecordDisplayType.Relation));
		_dataArray.Add(new LifeRecordItem(680, 991, 992, new string[6] { "Character", "Location", "Character", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 40, -1, ELifeRecordDisplayType.Relation));
		_dataArray.Add(new LifeRecordItem(681, 993, 994, new string[6] { "Character", "Location", "Character", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 40, -1, ELifeRecordDisplayType.Relation));
		_dataArray.Add(new LifeRecordItem(682, 995, 996, new string[6] { "Character", "Location", "Character", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 60, -1, ELifeRecordDisplayType.Relation));
		_dataArray.Add(new LifeRecordItem(683, 997, 998, new string[6] { "Character", "Location", "Character", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 60, -1, ELifeRecordDisplayType.Relation));
		_dataArray.Add(new LifeRecordItem(684, 999, 1000, new string[6] { "Character", "Location", "Character", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 50, -1, ELifeRecordDisplayType.Relation));
		_dataArray.Add(new LifeRecordItem(685, 1001, 1002, new string[6] { "Character", "Location", "Character", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 50, -1, ELifeRecordDisplayType.Relation));
		_dataArray.Add(new LifeRecordItem(686, 999, 1003, new string[6] { "Character", "Location", "Character", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 50, -1, ELifeRecordDisplayType.Relation));
		_dataArray.Add(new LifeRecordItem(687, 1001, 1004, new string[6] { "Character", "Location", "Character", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 50, -1, ELifeRecordDisplayType.Relation));
		_dataArray.Add(new LifeRecordItem(688, 976, 1005, new string[6] { "Character", "Location", "SecretInformation", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 40, -1, ELifeRecordDisplayType.Relation));
		_dataArray.Add(new LifeRecordItem(689, 1006, 1007, new string[6] { "CharacterRealName", "", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 70, -1, ELifeRecordDisplayType.Great));
		_dataArray.Add(new LifeRecordItem(690, 969, 1008, new string[6] { "Location", "Adventure", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 60, -1, ELifeRecordDisplayType.Great));
		_dataArray.Add(new LifeRecordItem(691, 1009, 1010, new string[6] { "", "", "", "", "", "" }, isSourceRecord: false, new List<short> { 411 }, -30000, ELifeRecordScoreType.Normal, 50, 4, ELifeRecordDisplayType.Relation));
		_dataArray.Add(new LifeRecordItem(692, 1011, 1012, new string[6] { "", "", "", "", "", "" }, isSourceRecord: false, new List<short> { 666 }, -30000, ELifeRecordScoreType.Normal, 50, 9, ELifeRecordDisplayType.Relation));
		_dataArray.Add(new LifeRecordItem(693, 1015, 1016, new string[6] { "Item", "Location", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 60, -1, ELifeRecordDisplayType.Normal));
		_dataArray.Add(new LifeRecordItem(694, 1017, 1018, new string[6] { "Item", "Location", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 40, -1, ELifeRecordDisplayType.Normal));
		_dataArray.Add(new LifeRecordItem(695, 1013, 1014, new string[6] { "Character", "Location", "Adventure", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 40, -1, ELifeRecordDisplayType.Relation));
		_dataArray.Add(new LifeRecordItem(696, 1019, 1020, new string[6] { "Item", "Location", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 50, -1, ELifeRecordDisplayType.Normal));
		_dataArray.Add(new LifeRecordItem(697, 1021, 1022, new string[6] { "Character", "Location", "JiaoLoong", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 60, -1, ELifeRecordDisplayType.Normal));
		_dataArray.Add(new LifeRecordItem(698, 1021, 1023, new string[6] { "Location", "JiaoLoong", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 60, -1, ELifeRecordDisplayType.Normal));
		_dataArray.Add(new LifeRecordItem(699, 1024, 1025, new string[6] { "Character", "Location", "JiaoLoong", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 60, -1, ELifeRecordDisplayType.Normal));
		_dataArray.Add(new LifeRecordItem(700, 1026, 1027, new string[6] { "Location", "JiaoLoong", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 60, -1, ELifeRecordDisplayType.Normal));
		_dataArray.Add(new LifeRecordItem(701, 1028, 1029, new string[6] { "JiaoLoong", "Cricket", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 60, -1, ELifeRecordDisplayType.Normal));
		_dataArray.Add(new LifeRecordItem(702, 1030, 1031, new string[6] { "Location", "JiaoLoong", "Item", "Integer", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 60, -1, ELifeRecordDisplayType.Normal));
		_dataArray.Add(new LifeRecordItem(703, 1032, 1033, new string[6] { "Location", "JiaoLoong", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 60, -1, ELifeRecordDisplayType.Normal));
		_dataArray.Add(new LifeRecordItem(704, 1034, 1035, new string[6] { "Location", "JiaoLoong", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 60, -1, ELifeRecordDisplayType.Normal));
		_dataArray.Add(new LifeRecordItem(705, 1036, 1037, new string[6] { "Location", "JiaoLoong", "Item", "Integer", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 60, -1, ELifeRecordDisplayType.Normal));
		_dataArray.Add(new LifeRecordItem(706, 1038, 1039, new string[6] { "Location", "JiaoLoong", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 60, -1, ELifeRecordDisplayType.Normal));
		_dataArray.Add(new LifeRecordItem(707, 1040, 1041, new string[6] { "CharacterTemplate", "Location", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 50, -1, ELifeRecordDisplayType.Normal));
		_dataArray.Add(new LifeRecordItem(708, 1042, 1043, new string[6] { "CharacterTemplate", "Location", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 40, -1, ELifeRecordDisplayType.Normal));
		_dataArray.Add(new LifeRecordItem(709, 1044, 505, new string[6] { "Location", "JiaoLoong", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 40, -1, ELifeRecordDisplayType.Normal));
		_dataArray.Add(new LifeRecordItem(710, 1045, 1046, new string[6] { "Location", "", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 50, -1, ELifeRecordDisplayType.Great));
		_dataArray.Add(new LifeRecordItem(711, 1047, 1048, new string[6] { "Location", "", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 40, -1, ELifeRecordDisplayType.Great));
		_dataArray.Add(new LifeRecordItem(712, 173, 1050, new string[6] { "", "", "", "", "", "" }, isSourceRecord: false, new List<short> { 92 }, -30000, ELifeRecordScoreType.Normal, 30, -1, ELifeRecordDisplayType.Great));
		_dataArray.Add(new LifeRecordItem(713, 171, 1049, new string[6] { "", "", "", "", "", "" }, isSourceRecord: false, new List<short> { 91 }, -30000, ELifeRecordScoreType.Normal, 30, -1, ELifeRecordDisplayType.Great));
		_dataArray.Add(new LifeRecordItem(714, 1051, 1052, new string[6] { "Character", "", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 50, -1, ELifeRecordDisplayType.Great));
		_dataArray.Add(new LifeRecordItem(715, 1053, 1054, new string[6] { "Location", "", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 40, -1, ELifeRecordDisplayType.Great));
		_dataArray.Add(new LifeRecordItem(716, 1055, 1056, new string[6] { "Location", "", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 40, -1, ELifeRecordDisplayType.Great));
		_dataArray.Add(new LifeRecordItem(717, 383, 1057, new string[6] { "Location", "Item", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 50, -1, ELifeRecordDisplayType.Normal));
		_dataArray.Add(new LifeRecordItem(718, 385, 1058, new string[6] { "Location", "Item", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 50, -1, ELifeRecordDisplayType.Normal));
		_dataArray.Add(new LifeRecordItem(719, 1059, 1060, new string[6] { "Character", "Location", "CombatType", "", "", "" }, isSourceRecord: true, new List<short> { 720 }, -30000, ELifeRecordScoreType.Normal, 50, -1, ELifeRecordDisplayType.Combat));
	}

	private void CreateItems12()
	{
		_dataArray.Add(new LifeRecordItem(720, 1061, 1062, new string[6] { "", "", "", "", "", "" }, isSourceRecord: false, new List<short> { 719 }, -30000, ELifeRecordScoreType.Normal, 50, -1, ELifeRecordDisplayType.Combat));
		_dataArray.Add(new LifeRecordItem(721, 1063, 1064, new string[6] { "Character", "Location", "CombatType", "", "", "" }, isSourceRecord: true, new List<short> { 722 }, -30000, ELifeRecordScoreType.Normal, 50, -1, ELifeRecordDisplayType.Combat));
		_dataArray.Add(new LifeRecordItem(722, 1065, 1066, new string[6] { "", "", "", "", "", "" }, isSourceRecord: false, new List<short> { 721 }, -30000, ELifeRecordScoreType.Normal, 50, -1, ELifeRecordDisplayType.Combat));
		_dataArray.Add(new LifeRecordItem(723, 1067, 1068, new string[6] { "Character", "Location", "Item", "", "", "" }, isSourceRecord: true, new List<short> { 737 }, -30000, ELifeRecordScoreType.Normal, 50, -1, ELifeRecordDisplayType.Relation));
		_dataArray.Add(new LifeRecordItem(724, 48, 1069, new string[6] { "Character", "Location", "SecretInformationTemplate", "", "", "" }, isSourceRecord: true, new List<short> { 725 }, -30000, ELifeRecordScoreType.Normal, 50, -1, ELifeRecordDisplayType.Relation));
		_dataArray.Add(new LifeRecordItem(725, 48, 1070, new string[6] { "", "", "", "", "", "" }, isSourceRecord: false, new List<short> { 724 }, -30000, ELifeRecordScoreType.Normal, 50, -1, ELifeRecordDisplayType.Relation));
		_dataArray.Add(new LifeRecordItem(726, 1071, 1072, new string[6] { "Location", "CharacterTemplate", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 50, -1, ELifeRecordDisplayType.Combat));
		_dataArray.Add(new LifeRecordItem(727, 1073, 1074, new string[6] { "Location", "CharacterTemplate", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 50, -1, ELifeRecordDisplayType.Combat));
		_dataArray.Add(new LifeRecordItem(728, 1075, 1076, new string[6] { "Location", "CharacterTemplate", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 50, -1, ELifeRecordDisplayType.Combat));
		_dataArray.Add(new LifeRecordItem(729, 1077, 1078, new string[6] { "Location", "CharacterTemplate", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 50, -1, ELifeRecordDisplayType.Combat));
		_dataArray.Add(new LifeRecordItem(730, 1079, 1080, new string[6] { "Location", "CharacterTemplate", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 50, -1, ELifeRecordDisplayType.Combat));
		_dataArray.Add(new LifeRecordItem(731, 1081, 1082, new string[6] { "Location", "CharacterTemplate", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 50, -1, ELifeRecordDisplayType.Combat));
		_dataArray.Add(new LifeRecordItem(732, 918, 1083, new string[6] { "Location", "", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 40, -1, ELifeRecordDisplayType.Negative));
		_dataArray.Add(new LifeRecordItem(733, 1084, 1085, new string[6] { "Character", "Location", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 40, -1, ELifeRecordDisplayType.Crime));
		_dataArray.Add(new LifeRecordItem(734, 1086, 1087, new string[6] { "Location", "", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 50, -1, ELifeRecordDisplayType.Normal));
		_dataArray.Add(new LifeRecordItem(735, 1088, 1089, new string[6] { "Item", "Location", "Item", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 50, -1, ELifeRecordDisplayType.Normal));
		_dataArray.Add(new LifeRecordItem(736, 1067, 1090, new string[6] { "Item", "Character", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 50, -1, ELifeRecordDisplayType.Relation));
		_dataArray.Add(new LifeRecordItem(737, 1067, 1091, new string[6] { "", "", "", "", "", "" }, isSourceRecord: false, new List<short> { 723 }, -30000, ELifeRecordScoreType.Normal, 50, -1, ELifeRecordDisplayType.Relation));
		_dataArray.Add(new LifeRecordItem(738, 1092, 1093, new string[6] { "Item", "", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 50, -1, ELifeRecordDisplayType.Negative));
		_dataArray.Add(new LifeRecordItem(739, 1094, 1095, new string[6] { "Item", "", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 50, -1, ELifeRecordDisplayType.Negative));
		_dataArray.Add(new LifeRecordItem(740, 1096, 1097, new string[6] { "Item", "", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 50, -1, ELifeRecordDisplayType.Negative));
		_dataArray.Add(new LifeRecordItem(741, 1098, 1099, new string[6] { "Item", "", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 50, -1, ELifeRecordDisplayType.Negative));
		_dataArray.Add(new LifeRecordItem(742, 1100, 1101, new string[6] { "Item", "Item", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 50, -1, ELifeRecordDisplayType.Negative));
		_dataArray.Add(new LifeRecordItem(743, 1102, 1103, new string[6] { "Item", "", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 50, -1, ELifeRecordDisplayType.Negative));
		_dataArray.Add(new LifeRecordItem(744, 1104, 1105, new string[6] { "Character", "Location", "Item", "Item", "", "" }, isSourceRecord: true, new List<short> { 745 }, -30000, ELifeRecordScoreType.Normal, 50, -1, ELifeRecordDisplayType.Relation));
		_dataArray.Add(new LifeRecordItem(745, 1104, 1106, new string[6] { "", "", "", "", "", "" }, isSourceRecord: false, new List<short> { 744 }, -30000, ELifeRecordScoreType.Normal, 50, -1, ELifeRecordDisplayType.Relation));
		_dataArray.Add(new LifeRecordItem(746, 1107, 1108, new string[6] { "", "", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 50, -1, ELifeRecordDisplayType.Normal));
		_dataArray.Add(new LifeRecordItem(747, 1109, 1110, new string[6] { "", "", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 40, -1, ELifeRecordDisplayType.Relation));
		_dataArray.Add(new LifeRecordItem(748, 1111, 1112, new string[6] { "", "", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 40, -1, ELifeRecordDisplayType.Negative));
		_dataArray.Add(new LifeRecordItem(749, 1113, 1114, new string[6] { "", "", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 40, -1, ELifeRecordDisplayType.Negative));
		_dataArray.Add(new LifeRecordItem(750, 1115, 1116, new string[6] { "Location", "", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 40, -1, ELifeRecordDisplayType.Negative));
		_dataArray.Add(new LifeRecordItem(751, 1117, 1118, new string[6] { "Location", "", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 40, -1, ELifeRecordDisplayType.Negative));
		_dataArray.Add(new LifeRecordItem(752, 1094, 1119, new string[6] { "Item", "", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 40, -1, ELifeRecordDisplayType.Negative));
		_dataArray.Add(new LifeRecordItem(753, 1120, 1121, new string[6] { "Item", "Item", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 40, -1, ELifeRecordDisplayType.Negative));
		_dataArray.Add(new LifeRecordItem(754, 1122, 1123, new string[6] { "Item", "Item", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 40, -1, ELifeRecordDisplayType.Negative));
		_dataArray.Add(new LifeRecordItem(755, 1124, 1125, new string[6] { "Item", "Item", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 40, -1, ELifeRecordDisplayType.Negative));
		_dataArray.Add(new LifeRecordItem(756, 1126, 1127, new string[6] { "Item", "Item", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 40, -1, ELifeRecordDisplayType.Negative));
		_dataArray.Add(new LifeRecordItem(757, 1128, 1129, new string[6] { "Character", "Location", "Item", "Item", "", "" }, isSourceRecord: true, new List<short> { 758 }, -30000, ELifeRecordScoreType.Normal, 40, -1, ELifeRecordDisplayType.Negative));
		_dataArray.Add(new LifeRecordItem(758, 1128, 1130, new string[6] { "", "", "", "", "", "" }, isSourceRecord: false, new List<short> { 757 }, -30000, ELifeRecordScoreType.Normal, 40, -1, ELifeRecordDisplayType.Negative));
		_dataArray.Add(new LifeRecordItem(759, 1131, 1132, new string[6] { "Item", "Item", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 40, -1, ELifeRecordDisplayType.Negative));
		_dataArray.Add(new LifeRecordItem(760, 1133, 1134, new string[6] { "Item", "Item", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 40, -1, ELifeRecordDisplayType.Negative));
		_dataArray.Add(new LifeRecordItem(761, 1135, 1136, new string[6] { "Character", "Location", "Item", "Item", "", "" }, isSourceRecord: true, new List<short> { 762 }, -30000, ELifeRecordScoreType.Normal, 40, -1, ELifeRecordDisplayType.Negative));
		_dataArray.Add(new LifeRecordItem(762, 1135, 1137, new string[6] { "", "", "", "", "", "" }, isSourceRecord: false, new List<short> { 761 }, -30000, ELifeRecordScoreType.Normal, 40, -1, ELifeRecordDisplayType.Negative));
		_dataArray.Add(new LifeRecordItem(763, 1138, 1139, new string[6] { "Item", "Integer", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 70, -1, ELifeRecordDisplayType.Study));
		_dataArray.Add(new LifeRecordItem(764, 1140, 1141, new string[6] { "Item", "Integer", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 70, -1, ELifeRecordDisplayType.Study));
		_dataArray.Add(new LifeRecordItem(765, 1142, 1143, new string[6] { "LifeSkillType", "", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 60, -1, ELifeRecordDisplayType.Study));
		_dataArray.Add(new LifeRecordItem(766, 1144, 1145, new string[6] { "CombatSkillType", "", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 60, -1, ELifeRecordDisplayType.Study));
		_dataArray.Add(new LifeRecordItem(767, 1146, 1147, new string[6] { "LifeSkillType", "", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 60, -1, ELifeRecordDisplayType.Study));
		_dataArray.Add(new LifeRecordItem(768, 1146, 1147, new string[6] { "CombatSkillType", "", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 60, -1, ELifeRecordDisplayType.Study));
		_dataArray.Add(new LifeRecordItem(769, 4, 1148, new string[6] { "Location", "", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Absolute, 0, -1, ELifeRecordDisplayType.Great));
		_dataArray.Add(new LifeRecordItem(770, 559, 1149, new string[6] { "Character", "Location", "", "", "", "" }, isSourceRecord: true, new List<short> { 771 }, -30000, ELifeRecordScoreType.Absolute, 80, -1, ELifeRecordDisplayType.Great));
		_dataArray.Add(new LifeRecordItem(771, 1009, 1150, new string[6] { "", "", "", "", "", "" }, isSourceRecord: false, new List<short> { 770 }, -30000, ELifeRecordScoreType.Normal, 50, 4, ELifeRecordDisplayType.Relation));
		_dataArray.Add(new LifeRecordItem(772, 1151, 1152, new string[6] { "Settlement", "Resource", "Integer", "Integer", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 50, -1, ELifeRecordDisplayType.Produce));
		_dataArray.Add(new LifeRecordItem(773, 1153, 1154, new string[6] { "Settlement", "Item", "Integer", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 50, -1, ELifeRecordDisplayType.Produce));
		_dataArray.Add(new LifeRecordItem(774, 1155, 1156, new string[6] { "Settlement", "Resource", "Integer", "Integer", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 50, -1, ELifeRecordDisplayType.Produce));
		_dataArray.Add(new LifeRecordItem(775, 1157, 1158, new string[6] { "Settlement", "Item", "Integer", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 50, -1, ELifeRecordDisplayType.Produce));
		_dataArray.Add(new LifeRecordItem(776, 1163, 1164, new string[6] { "Location", "Settlement", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 50, -1, ELifeRecordDisplayType.Produce));
		_dataArray.Add(new LifeRecordItem(777, 1165, 1166, new string[6] { "Location", "Settlement", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 50, -1, ELifeRecordDisplayType.Produce));
		_dataArray.Add(new LifeRecordItem(778, 1167, 1168, new string[6] { "Character", "Settlement", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 40, -1, ELifeRecordDisplayType.Crime));
		_dataArray.Add(new LifeRecordItem(779, 1167, 1169, new string[6] { "Settlement", "", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 40, -1, ELifeRecordDisplayType.Crime));
	}

	private void CreateItems13()
	{
		_dataArray.Add(new LifeRecordItem(780, 1167, 1170, new string[6] { "Character", "Settlement", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 40, -1, ELifeRecordDisplayType.Crime));
		_dataArray.Add(new LifeRecordItem(781, 1167, 1171, new string[6] { "Settlement", "", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 40, -1, ELifeRecordDisplayType.Crime));
		_dataArray.Add(new LifeRecordItem(782, 1167, 1172, new string[6] { "Character", "Settlement", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 40, -1, ELifeRecordDisplayType.Crime));
		_dataArray.Add(new LifeRecordItem(783, 1167, 1173, new string[6] { "Settlement", "", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 40, -1, ELifeRecordDisplayType.Crime));
		_dataArray.Add(new LifeRecordItem(784, 1167, 1174, new string[6] { "Character", "Settlement", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 40, -1, ELifeRecordDisplayType.Crime));
		_dataArray.Add(new LifeRecordItem(785, 1167, 1175, new string[6] { "Settlement", "Float", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 40, -1, ELifeRecordDisplayType.Crime));
		_dataArray.Add(new LifeRecordItem(786, 1167, 1176, new string[6] { "Settlement", "", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 40, -1, ELifeRecordDisplayType.Crime));
		_dataArray.Add(new LifeRecordItem(787, 1177, 1178, new string[6] { "Character", "Settlement", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 40, -1, ELifeRecordDisplayType.Crime));
		_dataArray.Add(new LifeRecordItem(788, 1177, 1179, new string[6] { "Settlement", "", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 40, -1, ELifeRecordDisplayType.Crime));
		_dataArray.Add(new LifeRecordItem(789, 1177, 1180, new string[6] { "Character", "Settlement", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 40, -1, ELifeRecordDisplayType.Crime));
		_dataArray.Add(new LifeRecordItem(790, 1177, 1181, new string[6] { "Settlement", "", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 40, -1, ELifeRecordDisplayType.Crime));
		_dataArray.Add(new LifeRecordItem(791, 1177, 1182, new string[6] { "Character", "Settlement", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 40, -1, ELifeRecordDisplayType.Crime));
		_dataArray.Add(new LifeRecordItem(792, 1177, 1183, new string[6] { "Settlement", "", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 40, -1, ELifeRecordDisplayType.Crime));
		_dataArray.Add(new LifeRecordItem(793, 1177, 1184, new string[6] { "Character", "Settlement", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 40, -1, ELifeRecordDisplayType.Crime));
		_dataArray.Add(new LifeRecordItem(794, 1177, 1185, new string[6] { "Settlement", "Float", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 40, -1, ELifeRecordDisplayType.Crime));
		_dataArray.Add(new LifeRecordItem(795, 1177, 1186, new string[6] { "Settlement", "", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 40, -1, ELifeRecordDisplayType.Crime));
		_dataArray.Add(new LifeRecordItem(796, 1187, 1188, new string[6] { "Character", "Settlement", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 60, -1, ELifeRecordDisplayType.Relation));
		_dataArray.Add(new LifeRecordItem(797, 1187, 1189, new string[6] { "Settlement", "", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 60, -1, ELifeRecordDisplayType.Relation));
		_dataArray.Add(new LifeRecordItem(798, 1187, 1190, new string[6] { "Character", "Settlement", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 60, -1, ELifeRecordDisplayType.Relation));
		_dataArray.Add(new LifeRecordItem(799, 1187, 1191, new string[6] { "Settlement", "Float", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 60, -1, ELifeRecordDisplayType.Relation));
		_dataArray.Add(new LifeRecordItem(800, 1187, 1192, new string[6] { "Settlement", "", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 60, -1, ELifeRecordDisplayType.Relation));
		_dataArray.Add(new LifeRecordItem(801, 1193, 1195, new string[6] { "Settlement", "Item", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 80, -1, ELifeRecordDisplayType.Produce));
		_dataArray.Add(new LifeRecordItem(802, 301, 1196, new string[6] { "Character", "Location", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 40, -1, ELifeRecordDisplayType.Crime));
		_dataArray.Add(new LifeRecordItem(803, 301, 1197, new string[6] { "Character", "Location", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 40, -1, ELifeRecordDisplayType.Crime));
		_dataArray.Add(new LifeRecordItem(804, 301, 1198, new string[6] { "Character", "Location", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 40, -1, ELifeRecordDisplayType.Crime));
		_dataArray.Add(new LifeRecordItem(805, 301, 1199, new string[6] { "Character", "Location", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 40, -1, ELifeRecordDisplayType.Crime));
		_dataArray.Add(new LifeRecordItem(806, 301, 1200, new string[6] { "Character", "Location", "", "", "", "" }, isSourceRecord: true, new List<short> { 808 }, -30000, ELifeRecordScoreType.Normal, 60, -1, ELifeRecordDisplayType.Crime));
		_dataArray.Add(new LifeRecordItem(807, 301, 1201, new string[6] { "Character", "Location", "", "", "", "" }, isSourceRecord: true, new List<short> { 808 }, -30000, ELifeRecordScoreType.Normal, 60, -1, ELifeRecordDisplayType.Crime));
		_dataArray.Add(new LifeRecordItem(808, 301, 1202, new string[6] { "", "", "", "", "", "" }, isSourceRecord: false, new List<short> { 806, 807 }, -30000, ELifeRecordScoreType.Normal, 40, -1, ELifeRecordDisplayType.Crime));
		_dataArray.Add(new LifeRecordItem(809, 309, 1203, new string[6] { "Character", "Location", "Item", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 40, -1, ELifeRecordDisplayType.Crime));
		_dataArray.Add(new LifeRecordItem(810, 309, 1204, new string[6] { "Character", "Location", "Item", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 40, -1, ELifeRecordDisplayType.Crime));
		_dataArray.Add(new LifeRecordItem(811, 309, 1205, new string[6] { "Character", "Location", "Item", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 40, -1, ELifeRecordDisplayType.Crime));
		_dataArray.Add(new LifeRecordItem(812, 309, 1206, new string[6] { "Character", "Location", "Item", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 40, -1, ELifeRecordDisplayType.Crime));
		_dataArray.Add(new LifeRecordItem(813, 309, 1207, new string[6] { "Character", "Location", "Item", "", "", "" }, isSourceRecord: true, new List<short> { 815 }, -30000, ELifeRecordScoreType.Normal, 60, -1, ELifeRecordDisplayType.Crime));
		_dataArray.Add(new LifeRecordItem(814, 309, 1208, new string[6] { "Character", "Location", "Item", "", "", "" }, isSourceRecord: true, new List<short> { 815 }, -30000, ELifeRecordScoreType.Normal, 60, -1, ELifeRecordDisplayType.Crime));
		_dataArray.Add(new LifeRecordItem(815, 309, 1209, new string[6] { "", "", "", "", "", "" }, isSourceRecord: false, new List<short> { 813, 814 }, -30000, ELifeRecordScoreType.Normal, 40, -1, ELifeRecordDisplayType.Crime));
		_dataArray.Add(new LifeRecordItem(816, 1210, 1211, new string[6] { "Location", "", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 40, -1, ELifeRecordDisplayType.Negative));
		_dataArray.Add(new LifeRecordItem(817, 1210, 1212, new string[6] { "Location", "", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 40, -1, ELifeRecordDisplayType.Negative));
		_dataArray.Add(new LifeRecordItem(818, 1213, 1214, new string[6] { "Character", "Location", "", "", "", "" }, isSourceRecord: true, new List<short> { 819 }, -30000, ELifeRecordScoreType.Normal, 40, -1, ELifeRecordDisplayType.Negative));
		_dataArray.Add(new LifeRecordItem(819, 1213, 1215, new string[6] { "", "", "", "", "", "" }, isSourceRecord: false, new List<short> { 818 }, -30000, ELifeRecordScoreType.Normal, 40, -1, ELifeRecordDisplayType.Negative));
		_dataArray.Add(new LifeRecordItem(820, 1216, 1217, new string[6] { "Character", "Location", "", "", "", "" }, isSourceRecord: true, new List<short> { 821 }, -30000, ELifeRecordScoreType.Normal, 60, -1, ELifeRecordDisplayType.Normal));
		_dataArray.Add(new LifeRecordItem(821, 1218, 1219, new string[6] { "", "", "", "", "", "" }, isSourceRecord: false, new List<short> { 820 }, -30000, ELifeRecordScoreType.Normal, 60, -1, ELifeRecordDisplayType.Normal));
		_dataArray.Add(new LifeRecordItem(822, 1151, 1159, new string[6] { "Settlement", "Resource", "Integer", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 50, -1, ELifeRecordDisplayType.Produce));
		_dataArray.Add(new LifeRecordItem(823, 1153, 1160, new string[6] { "Settlement", "Item", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 50, -1, ELifeRecordDisplayType.Produce));
		_dataArray.Add(new LifeRecordItem(824, 1155, 1161, new string[6] { "Settlement", "Resource", "Integer", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 50, -1, ELifeRecordDisplayType.Produce));
		_dataArray.Add(new LifeRecordItem(825, 1157, 1162, new string[6] { "Settlement", "Item", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 50, -1, ELifeRecordDisplayType.Produce));
		_dataArray.Add(new LifeRecordItem(826, 1193, 1194, new string[6] { "Settlement", "Resource", "Integer", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 80, -1, ELifeRecordDisplayType.Produce));
		_dataArray.Add(new LifeRecordItem(827, 1220, 1221, new string[6] { "Item", "Location", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 50, -1, ELifeRecordDisplayType.Great));
		_dataArray.Add(new LifeRecordItem(828, 1220, 1222, new string[6] { "Item", "Location", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 50, -1, ELifeRecordDisplayType.Great));
		_dataArray.Add(new LifeRecordItem(829, 1220, 1223, new string[6] { "Item", "Location", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 50, -1, ELifeRecordDisplayType.Great));
		_dataArray.Add(new LifeRecordItem(830, 48, 1069, new string[6] { "Character", "Location", "SecretInformation", "", "", "" }, isSourceRecord: true, new List<short> { 831 }, -30000, ELifeRecordScoreType.Normal, 50, -1, ELifeRecordDisplayType.Relation));
		_dataArray.Add(new LifeRecordItem(831, 48, 1070, new string[6] { "", "", "", "", "", "" }, isSourceRecord: false, new List<short> { 830 }, -30000, ELifeRecordScoreType.Normal, 50, -1, ELifeRecordDisplayType.Relation));
		_dataArray.Add(new LifeRecordItem(832, 1224, 1225, new string[6] { "Character", "Location", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 50, -1, ELifeRecordDisplayType.Relation));
		_dataArray.Add(new LifeRecordItem(833, 1226, 1227, new string[6] { "Character", "Location", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 50, -1, ELifeRecordDisplayType.Relation));
		_dataArray.Add(new LifeRecordItem(834, 1228, 1229, new string[6] { "Location", "PunishmentType", "Settlement", "Location", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 40, -1, ELifeRecordDisplayType.Relation));
		_dataArray.Add(new LifeRecordItem(835, 1230, 1231, new string[6] { "Location", "", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 60, -1, ELifeRecordDisplayType.Relation));
		_dataArray.Add(new LifeRecordItem(836, 1232, 1233, new string[6] { "Location", "PunishmentType", "Settlement", "Settlement", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 40, -1, ELifeRecordDisplayType.Relation));
		_dataArray.Add(new LifeRecordItem(837, 1234, 1235, new string[6] { "Location", "Settlement", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 40, -1, ELifeRecordDisplayType.Relation));
		_dataArray.Add(new LifeRecordItem(838, 1236, 1237, new string[6] { "Location", "Settlement", "OrgGrade", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 60, -1, ELifeRecordDisplayType.Relation));
		_dataArray.Add(new LifeRecordItem(839, 1238, 1239, new string[6] { "Character", "Location", "Settlement", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 60, -1, ELifeRecordDisplayType.Relation));
	}

	private void CreateItems14()
	{
		_dataArray.Add(new LifeRecordItem(840, 1240, 1241, new string[6] { "Character", "Location", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 60, -1, ELifeRecordDisplayType.Relation));
		_dataArray.Add(new LifeRecordItem(841, 1242, 1243, new string[6] { "Settlement", "", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 40, -1, ELifeRecordDisplayType.Negative));
		_dataArray.Add(new LifeRecordItem(842, 1242, 1244, new string[6] { "Settlement", "", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 60, -1, ELifeRecordDisplayType.Study));
		_dataArray.Add(new LifeRecordItem(843, 1242, 1245, new string[6] { "Settlement", "", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 40, -1, ELifeRecordDisplayType.Negative));
		_dataArray.Add(new LifeRecordItem(844, 1242, 1246, new string[6] { "Settlement", "", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 40, -1, ELifeRecordDisplayType.Negative));
		_dataArray.Add(new LifeRecordItem(845, 1242, 1247, new string[6] { "Settlement", "", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 40, -1, ELifeRecordDisplayType.Negative));
		_dataArray.Add(new LifeRecordItem(846, 1242, 1248, new string[6] { "Settlement", "", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 40, -1, ELifeRecordDisplayType.Negative));
		_dataArray.Add(new LifeRecordItem(847, 1242, 1249, new string[6] { "Settlement", "", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 40, -1, ELifeRecordDisplayType.Negative));
		_dataArray.Add(new LifeRecordItem(848, 1242, 1250, new string[6] { "Settlement", "", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 40, -1, ELifeRecordDisplayType.Negative));
		_dataArray.Add(new LifeRecordItem(849, 1242, 1251, new string[6] { "Settlement", "", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 40, -1, ELifeRecordDisplayType.Negative));
		_dataArray.Add(new LifeRecordItem(850, 1242, 1252, new string[6] { "Settlement", "", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 40, -1, ELifeRecordDisplayType.Negative));
		_dataArray.Add(new LifeRecordItem(851, 1242, 1253, new string[6] { "Settlement", "Item", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 40, -1, ELifeRecordDisplayType.Negative));
		_dataArray.Add(new LifeRecordItem(852, 1242, 1254, new string[6] { "Settlement", "", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 40, -1, ELifeRecordDisplayType.Negative));
		_dataArray.Add(new LifeRecordItem(853, 1242, 1255, new string[6] { "Settlement", "Item", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 40, -1, ELifeRecordDisplayType.Negative));
		_dataArray.Add(new LifeRecordItem(854, 1242, 1256, new string[6] { "Settlement", "", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 60, -1, ELifeRecordDisplayType.Combat));
		_dataArray.Add(new LifeRecordItem(855, 1242, 1257, new string[6] { "Settlement", "", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 40, -1, ELifeRecordDisplayType.Negative));
		_dataArray.Add(new LifeRecordItem(856, 1242, 1258, new string[6] { "Settlement", "", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 40, -1, ELifeRecordDisplayType.Negative));
		_dataArray.Add(new LifeRecordItem(857, 1242, 1259, new string[6] { "Settlement", "BodyPartType", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 40, -1, ELifeRecordDisplayType.Negative));
		_dataArray.Add(new LifeRecordItem(858, 1260, 1261, new string[6] { "Character", "Settlement", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 40, -1, ELifeRecordDisplayType.Relation));
		_dataArray.Add(new LifeRecordItem(859, 1260, 1262, new string[6] { "Settlement", "", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 40, -1, ELifeRecordDisplayType.Relation));
		_dataArray.Add(new LifeRecordItem(860, 1260, 1263, new string[6] { "Character", "Settlement", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 40, -1, ELifeRecordDisplayType.Relation));
		_dataArray.Add(new LifeRecordItem(861, 1260, 1264, new string[6] { "Settlement", "", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 40, -1, ELifeRecordDisplayType.Relation));
		_dataArray.Add(new LifeRecordItem(862, 1260, 1265, new string[6] { "Character", "Settlement", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 40, -1, ELifeRecordDisplayType.Relation));
		_dataArray.Add(new LifeRecordItem(863, 1260, 1266, new string[6] { "Settlement", "", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 40, -1, ELifeRecordDisplayType.Relation));
		_dataArray.Add(new LifeRecordItem(864, 1267, 1268, new string[6] { "", "", "", "", "", "" }, isSourceRecord: false, new List<short> { 865 }, -30000, ELifeRecordScoreType.Normal, 40, -1, ELifeRecordDisplayType.Relation));
		_dataArray.Add(new LifeRecordItem(865, 1267, 1269, new string[6] { "Character", "Settlement", "", "", "", "" }, isSourceRecord: true, new List<short> { 864 }, -30000, ELifeRecordScoreType.Normal, 40, -1, ELifeRecordDisplayType.Relation));
		_dataArray.Add(new LifeRecordItem(866, 1270, 1271, new string[6] { "Character", "", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 60, -1, ELifeRecordDisplayType.Relation));
		_dataArray.Add(new LifeRecordItem(867, 1270, 1272, new string[6] { "Settlement", "", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 60, -1, ELifeRecordDisplayType.Relation));
		_dataArray.Add(new LifeRecordItem(868, 1270, 1273, new string[6] { "Character", "", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 60, -1, ELifeRecordDisplayType.Relation));
		_dataArray.Add(new LifeRecordItem(869, 1270, 1274, new string[6] { "Settlement", "", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 60, -1, ELifeRecordDisplayType.Relation));
		_dataArray.Add(new LifeRecordItem(870, 1270, 1275, new string[6] { "", "", "", "", "", "" }, isSourceRecord: false, new List<short> { 871 }, -30000, ELifeRecordScoreType.Normal, 50, -1, ELifeRecordDisplayType.Great));
		_dataArray.Add(new LifeRecordItem(871, 1270, 1276, new string[6] { "Character", "Settlement", "", "", "", "" }, isSourceRecord: true, new List<short> { 870 }, -30000, ELifeRecordScoreType.Normal, 50, -1, ELifeRecordDisplayType.Relation));
		_dataArray.Add(new LifeRecordItem(872, 1277, 1278, new string[6] { "Character", "Settlement", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 40, -1, ELifeRecordDisplayType.Relation));
		_dataArray.Add(new LifeRecordItem(873, 1277, 1279, new string[6] { "Character", "Character", "Settlement", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 40, -1, ELifeRecordDisplayType.Relation));
		_dataArray.Add(new LifeRecordItem(874, 1277, 1280, new string[6] { "Character", "Settlement", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 40, -1, ELifeRecordDisplayType.Relation));
		_dataArray.Add(new LifeRecordItem(875, 1277, 1281, new string[6] { "Character", "Character", "Settlement", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 40, -1, ELifeRecordDisplayType.Relation));
		_dataArray.Add(new LifeRecordItem(876, 1277, 1282, new string[6] { "Character", "Settlement", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 40, -1, ELifeRecordDisplayType.Relation));
		_dataArray.Add(new LifeRecordItem(877, 1277, 1283, new string[6] { "Character", "Character", "Settlement", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 40, -1, ELifeRecordDisplayType.Relation));
		_dataArray.Add(new LifeRecordItem(878, 1277, 1284, new string[6] { "Character", "Settlement", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 40, -1, ELifeRecordDisplayType.Relation));
		_dataArray.Add(new LifeRecordItem(879, 1285, 1286, new string[6] { "Character", "Character", "Settlement", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 40, -1, ELifeRecordDisplayType.Relation));
		_dataArray.Add(new LifeRecordItem(880, 1285, 1287, new string[6] { "Character", "Settlement", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 40, -1, ELifeRecordDisplayType.Relation));
		_dataArray.Add(new LifeRecordItem(881, 1285, 1288, new string[6] { "Character", "Character", "Settlement", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 40, -1, ELifeRecordDisplayType.Relation));
		_dataArray.Add(new LifeRecordItem(882, 1285, 1289, new string[6] { "Character", "Settlement", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 40, -1, ELifeRecordDisplayType.Relation));
		_dataArray.Add(new LifeRecordItem(883, 1285, 1290, new string[6] { "Character", "Character", "Settlement", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 40, -1, ELifeRecordDisplayType.Relation));
		_dataArray.Add(new LifeRecordItem(884, 1285, 1291, new string[6] { "Character", "Settlement", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 40, -1, ELifeRecordDisplayType.Relation));
		_dataArray.Add(new LifeRecordItem(885, 1292, 1293, new string[6] { "Character", "Location", "Settlement", "", "", "" }, isSourceRecord: true, new List<short> { 886, 1035 }, -30000, ELifeRecordScoreType.Normal, 40, -1, ELifeRecordDisplayType.Combat));
		_dataArray.Add(new LifeRecordItem(886, 1292, 1294, new string[6] { "", "", "", "", "", "" }, isSourceRecord: false, new List<short> { 885 }, -30000, ELifeRecordScoreType.Normal, 60, -1, ELifeRecordDisplayType.Combat));
		_dataArray.Add(new LifeRecordItem(887, 1292, 1295, new string[6] { "Character", "Character", "Location", "Settlement", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 50, -1, ELifeRecordDisplayType.Combat));
		_dataArray.Add(new LifeRecordItem(888, 1292, 1296, new string[6] { "Character", "Location", "Settlement", "", "", "" }, isSourceRecord: true, new List<short> { 889, 1036 }, -30000, ELifeRecordScoreType.Normal, 40, -1, ELifeRecordDisplayType.Combat));
		_dataArray.Add(new LifeRecordItem(889, 1292, 1297, new string[6] { "", "", "", "", "", "" }, isSourceRecord: false, new List<short> { 888 }, -30000, ELifeRecordScoreType.Normal, 40, -1, ELifeRecordDisplayType.Combat));
		_dataArray.Add(new LifeRecordItem(890, 1298, 1299, new string[6] { "Character", "Character", "Settlement", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 50, -1, ELifeRecordDisplayType.Relation));
		_dataArray.Add(new LifeRecordItem(891, 1298, 1300, new string[6] { "Character", "Character", "Settlement", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 50, -1, ELifeRecordDisplayType.Relation));
		_dataArray.Add(new LifeRecordItem(892, 1298, 1301, new string[6] { "Character", "Character", "Settlement", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 50, -1, ELifeRecordDisplayType.Relation));
		_dataArray.Add(new LifeRecordItem(893, 1302, 1303, new string[6] { "", "", "", "", "", "" }, isSourceRecord: false, new List<short> { 894 }, -30000, ELifeRecordScoreType.Normal, 50, -1, ELifeRecordDisplayType.Relation));
		_dataArray.Add(new LifeRecordItem(894, 1302, 1304, new string[6] { "Character", "Character", "Settlement", "Resource", "Integer", "" }, isSourceRecord: true, new List<short> { 893 }, -30000, ELifeRecordScoreType.Normal, 50, -1, ELifeRecordDisplayType.Relation));
		_dataArray.Add(new LifeRecordItem(895, 1302, 1305, new string[6] { "", "", "", "", "", "" }, isSourceRecord: false, new List<short> { 896 }, -30000, ELifeRecordScoreType.Normal, 50, -1, ELifeRecordDisplayType.Relation));
		_dataArray.Add(new LifeRecordItem(896, 1302, 1306, new string[6] { "Character", "Character", "Settlement", "", "", "" }, isSourceRecord: true, new List<short> { 895 }, -30000, ELifeRecordScoreType.Normal, 50, -1, ELifeRecordDisplayType.Relation));
		_dataArray.Add(new LifeRecordItem(897, 1302, 1307, new string[6] { "", "", "", "", "", "" }, isSourceRecord: false, new List<short> { 898 }, -30000, ELifeRecordScoreType.Normal, 50, -1, ELifeRecordDisplayType.Relation));
		_dataArray.Add(new LifeRecordItem(898, 1302, 1308, new string[6] { "Character", "Character", "Settlement", "", "", "" }, isSourceRecord: true, new List<short> { 897 }, -30000, ELifeRecordScoreType.Normal, 50, -1, ELifeRecordDisplayType.Relation));
		_dataArray.Add(new LifeRecordItem(899, 1302, 1309, new string[6] { "", "", "", "", "", "" }, isSourceRecord: false, new List<short> { 900 }, -30000, ELifeRecordScoreType.Normal, 50, -1, ELifeRecordDisplayType.Combat));
	}

	private void CreateItems15()
	{
		_dataArray.Add(new LifeRecordItem(900, 1302, 1310, new string[6] { "Character", "Character", "Settlement", "", "", "" }, isSourceRecord: true, new List<short> { 899 }, -30000, ELifeRecordScoreType.Normal, 50, -1, ELifeRecordDisplayType.Combat));
		_dataArray.Add(new LifeRecordItem(901, 1302, 1311, new string[6] { "", "", "", "", "", "" }, isSourceRecord: false, new List<short> { 902 }, -30000, ELifeRecordScoreType.Normal, 50, -1, ELifeRecordDisplayType.Combat));
		_dataArray.Add(new LifeRecordItem(902, 1302, 1312, new string[6] { "Character", "Character", "Settlement", "", "", "" }, isSourceRecord: true, new List<short> { 901 }, -30000, ELifeRecordScoreType.Normal, 50, -1, ELifeRecordDisplayType.Combat));
		_dataArray.Add(new LifeRecordItem(903, 1313, 1314, new string[6] { "Location", "", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 60, -1, ELifeRecordDisplayType.Combat));
		_dataArray.Add(new LifeRecordItem(904, 1315, 1316, new string[6] { "Location", "Resource", "Integer", "Resource", "Integer", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 40, -1, ELifeRecordDisplayType.Negative));
		_dataArray.Add(new LifeRecordItem(905, 1315, 1317, new string[6] { "Location", "Resource", "Integer", "Resource", "Integer", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 40, -1, ELifeRecordDisplayType.Negative));
		_dataArray.Add(new LifeRecordItem(906, 1313, 1318, new string[6] { "Location", "", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 60, -1, ELifeRecordDisplayType.Combat));
		_dataArray.Add(new LifeRecordItem(907, 1319, 1320, new string[6] { "Character", "Location", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 60, -1, ELifeRecordDisplayType.Relation));
		_dataArray.Add(new LifeRecordItem(908, 1321, 1322, new string[6] { "PunishmentType", "Settlement", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 40, -1, ELifeRecordDisplayType.Negative));
		_dataArray.Add(new LifeRecordItem(909, 1321, 1323, new string[6] { "PunishmentType", "Settlement", "Integer", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 30, -1, ELifeRecordDisplayType.Negative));
		_dataArray.Add(new LifeRecordItem(910, 1321, 1324, new string[6] { "PunishmentType", "Settlement", "Integer", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 20, -1, ELifeRecordDisplayType.Negative));
		_dataArray.Add(new LifeRecordItem(911, 1321, 1325, new string[6] { "PunishmentType", "Settlement", "Integer", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 10, -1, ELifeRecordDisplayType.Great));
		_dataArray.Add(new LifeRecordItem(912, 1321, 1326, new string[6] { "PunishmentType", "Settlement", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 0, -1, ELifeRecordDisplayType.Great));
		_dataArray.Add(new LifeRecordItem(913, 1321, 1327, new string[6] { "PunishmentType", "Settlement", "Character", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 0, -1, ELifeRecordDisplayType.Great));
		_dataArray.Add(new LifeRecordItem(914, 1321, 1328, new string[6] { "PunishmentType", "Settlement", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 40, -1, ELifeRecordDisplayType.Negative));
		_dataArray.Add(new LifeRecordItem(915, 1321, 1329, new string[6] { "PunishmentType", "Settlement", "Integer", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 30, -1, ELifeRecordDisplayType.Negative));
		_dataArray.Add(new LifeRecordItem(916, 1321, 1330, new string[6] { "PunishmentType", "Settlement", "Integer", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 20, -1, ELifeRecordDisplayType.Negative));
		_dataArray.Add(new LifeRecordItem(917, 1321, 1331, new string[6] { "PunishmentType", "Settlement", "Integer", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 10, -1, ELifeRecordDisplayType.Great));
		_dataArray.Add(new LifeRecordItem(918, 1321, 1332, new string[6] { "PunishmentType", "Settlement", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 0, -1, ELifeRecordDisplayType.Great));
		_dataArray.Add(new LifeRecordItem(919, 1321, 1333, new string[6] { "PunishmentType", "Settlement", "Character", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 0, -1, ELifeRecordDisplayType.Great));
		_dataArray.Add(new LifeRecordItem(920, 1321, 1334, new string[6] { "Character", "Settlement", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 0, -1, ELifeRecordDisplayType.Great));
		_dataArray.Add(new LifeRecordItem(921, 1335, 1336, new string[6] { "Settlement", "", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 70, -1, ELifeRecordDisplayType.Great));
		_dataArray.Add(new LifeRecordItem(922, 1337, 1338, new string[6] { "Settlement", "", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 60, -1, ELifeRecordDisplayType.Great));
		_dataArray.Add(new LifeRecordItem(923, 1339, 1340, new string[6] { "Character", "Settlement", "Resource", "Integer", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 60, -1, ELifeRecordDisplayType.Relation));
		_dataArray.Add(new LifeRecordItem(924, 1339, 1341, new string[6] { "Character", "Settlement", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 60, -1, ELifeRecordDisplayType.Relation));
		_dataArray.Add(new LifeRecordItem(925, 1339, 1342, new string[6] { "Character", "Settlement", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 40, -1, ELifeRecordDisplayType.Relation));
		_dataArray.Add(new LifeRecordItem(926, 1343, 1344, new string[6] { "Character", "Settlement", "", "", "", "" }, isSourceRecord: true, new List<short> { 927 }, -30000, ELifeRecordScoreType.Normal, 50, -1, ELifeRecordDisplayType.Normal));
		_dataArray.Add(new LifeRecordItem(927, 1343, 1345, new string[6] { "", "", "", "", "", "" }, isSourceRecord: false, new List<short> { 926 }, -30000, ELifeRecordScoreType.Normal, 50, -1, ELifeRecordDisplayType.Normal));
		_dataArray.Add(new LifeRecordItem(928, 1346, 1347, new string[6] { "Character", "Location", "Settlement", "", "", "" }, isSourceRecord: true, new List<short> { 929 }, -30000, ELifeRecordScoreType.Normal, 60, -1, ELifeRecordDisplayType.Combat));
		_dataArray.Add(new LifeRecordItem(929, 1346, 1348, new string[6] { "", "", "", "", "", "" }, isSourceRecord: false, new List<short> { 928 }, -30000, ELifeRecordScoreType.Normal, 40, -1, ELifeRecordDisplayType.Combat));
		_dataArray.Add(new LifeRecordItem(930, 1346, 1349, new string[6] { "Character", "Location", "Settlement", "", "", "" }, isSourceRecord: true, new List<short> { 931 }, -30000, ELifeRecordScoreType.Normal, 50, -1, ELifeRecordDisplayType.Combat));
		_dataArray.Add(new LifeRecordItem(931, 1346, 1350, new string[6] { "", "", "", "", "", "" }, isSourceRecord: false, new List<short> { 930 }, -30000, ELifeRecordScoreType.Normal, 50, -1, ELifeRecordDisplayType.Combat));
		_dataArray.Add(new LifeRecordItem(932, 1302, 1351, new string[6] { "", "", "", "", "", "" }, isSourceRecord: false, new List<short> { 933 }, -30000, ELifeRecordScoreType.Normal, 50, -1, ELifeRecordDisplayType.Relation));
		_dataArray.Add(new LifeRecordItem(933, 1302, 1352, new string[6] { "Character", "Character", "Settlement", "Integer", "", "" }, isSourceRecord: true, new List<short> { 932 }, -30000, ELifeRecordScoreType.Normal, 50, -1, ELifeRecordDisplayType.Relation));
		_dataArray.Add(new LifeRecordItem(934, 1339, 1353, new string[6] { "Character", "Settlement", "Integer", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 60, -1, ELifeRecordDisplayType.Relation));
		_dataArray.Add(new LifeRecordItem(935, 1354, 1355, new string[6] { "Location", "Item", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 60, -1, ELifeRecordDisplayType.Produce));
		_dataArray.Add(new LifeRecordItem(936, 1356, 1357, new string[6] { "Location", "Item", "Item", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 50, -1, ELifeRecordDisplayType.Produce));
		_dataArray.Add(new LifeRecordItem(937, 445, 1358, new string[6] { "Location", "Item", "Item", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 50, -1, ELifeRecordDisplayType.Produce));
		_dataArray.Add(new LifeRecordItem(938, 40, 1359, new string[6] { "Location", "Item", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 50, -1, ELifeRecordDisplayType.Produce));
		_dataArray.Add(new LifeRecordItem(939, 40, 1360, new string[6] { "Character", "Location", "Item", "", "", "" }, isSourceRecord: true, new List<short> { 960 }, -30000, ELifeRecordScoreType.Normal, 60, -1, ELifeRecordDisplayType.Produce));
		_dataArray.Add(new LifeRecordItem(940, 1361, 1362, new string[6] { "Location", "Item", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 50, -1, ELifeRecordDisplayType.Produce));
		_dataArray.Add(new LifeRecordItem(941, 1361, 1363, new string[6] { "Location", "Item", "Item", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 60, -1, ELifeRecordDisplayType.Produce));
		_dataArray.Add(new LifeRecordItem(942, 1364, 1365, new string[6] { "Location", "Item", "Item", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 50, -1, ELifeRecordDisplayType.Produce));
		_dataArray.Add(new LifeRecordItem(943, 1366, 1367, new string[6] { "Location", "Item", "Item", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 50, -1, ELifeRecordDisplayType.Produce));
		_dataArray.Add(new LifeRecordItem(944, 1366, 1368, new string[6] { "Location", "Item", "Item", "Item", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 60, -1, ELifeRecordDisplayType.Produce));
		_dataArray.Add(new LifeRecordItem(945, 1369, 1370, new string[6] { "Location", "Item", "Item", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 50, -1, ELifeRecordDisplayType.Produce));
		_dataArray.Add(new LifeRecordItem(946, 1371, 1372, new string[6] { "Character", "Location", "Item", "Resource", "Integer", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 50, -1, ELifeRecordDisplayType.Produce));
		_dataArray.Add(new LifeRecordItem(947, 1373, 1374, new string[6] { "Character", "Location", "Item", "Resource", "Integer", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 50, -1, ELifeRecordDisplayType.Produce));
		_dataArray.Add(new LifeRecordItem(948, 1375, 1376, new string[6] { "Character", "Character", "Location", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 60, -1, ELifeRecordDisplayType.Produce));
		_dataArray.Add(new LifeRecordItem(949, 1377, 1378, new string[6] { "Character", "Character", "Location", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 50, -1, ELifeRecordDisplayType.Produce));
		_dataArray.Add(new LifeRecordItem(950, 1379, 1380, new string[6] { "Character", "Character", "Location", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 50, -1, ELifeRecordDisplayType.Produce));
		_dataArray.Add(new LifeRecordItem(951, 1379, 1381, new string[6] { "Character", "Character", "Location", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 60, -1, ELifeRecordDisplayType.Produce));
		_dataArray.Add(new LifeRecordItem(952, 1379, 1382, new string[6] { "Character", "Character", "Location", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 60, -1, ELifeRecordDisplayType.Produce));
		_dataArray.Add(new LifeRecordItem(953, 1379, 1383, new string[6] { "Character", "Character", "Location", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 60, -1, ELifeRecordDisplayType.Produce));
		_dataArray.Add(new LifeRecordItem(954, 1384, 1385, new string[6] { "Character", "Location", "Integer", "Resource", "", "" }, isSourceRecord: true, new List<short> { 956 }, -30000, ELifeRecordScoreType.Normal, 60, -1, ELifeRecordDisplayType.Produce));
		_dataArray.Add(new LifeRecordItem(955, 1384, 1386, new string[6] { "Character", "Location", "", "", "", "" }, isSourceRecord: true, new List<short> { 957 }, -30000, ELifeRecordScoreType.Normal, 50, -1, ELifeRecordDisplayType.Produce));
		_dataArray.Add(new LifeRecordItem(956, 1384, 1387, new string[6] { "", "", "", "", "", "" }, isSourceRecord: false, new List<short> { 954 }, -30000, ELifeRecordScoreType.Normal, 60, -1, ELifeRecordDisplayType.Produce));
		_dataArray.Add(new LifeRecordItem(957, 1384, 1388, new string[6] { "", "", "", "", "", "" }, isSourceRecord: false, new List<short> { 955 }, -30000, ELifeRecordScoreType.Normal, 50, -1, ELifeRecordDisplayType.Produce));
		_dataArray.Add(new LifeRecordItem(958, 1270, 1389, new string[6] { "Location", "", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 40, -1, ELifeRecordDisplayType.Great));
		_dataArray.Add(new LifeRecordItem(959, 1270, 1390, new string[6] { "Location", "Settlement", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 40, -1, ELifeRecordDisplayType.Great));
	}

	private void CreateItems16()
	{
		_dataArray.Add(new LifeRecordItem(960, 40, 1391, new string[6] { "", "", "", "", "", "" }, isSourceRecord: false, new List<short> { 939 }, -30000, ELifeRecordScoreType.Normal, 60, -1, ELifeRecordDisplayType.Produce));
		_dataArray.Add(new LifeRecordItem(961, 1157, 1392, new string[6] { "Item", "", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 50, -1, ELifeRecordDisplayType.Produce));
		_dataArray.Add(new LifeRecordItem(962, 1153, 1393, new string[6] { "Item", "", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 50, -1, ELifeRecordDisplayType.Produce));
		_dataArray.Add(new LifeRecordItem(963, 1151, 1394, new string[6] { "Integer", "Resource", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 50, -1, ELifeRecordDisplayType.Produce));
		_dataArray.Add(new LifeRecordItem(964, 1155, 1395, new string[6] { "Integer", "Resource", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 50, -1, ELifeRecordDisplayType.Produce));
		_dataArray.Add(new LifeRecordItem(965, 1396, 1397, new string[6] { "Location", "Integer", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 50, -1, ELifeRecordDisplayType.Produce));
		_dataArray.Add(new LifeRecordItem(966, 1396, 1398, new string[6] { "Location", "Integer", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 50, -1, ELifeRecordDisplayType.Produce));
		_dataArray.Add(new LifeRecordItem(967, 1399, 1400, new string[6] { "Location", "Integer", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 50, -1, ELifeRecordDisplayType.Produce));
		_dataArray.Add(new LifeRecordItem(968, 1399, 1401, new string[6] { "Location", "Integer", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 50, -1, ELifeRecordDisplayType.Produce));
		_dataArray.Add(new LifeRecordItem(969, 1402, 1403, new string[6] { "Location", "", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 50, -1, ELifeRecordDisplayType.Produce));
		_dataArray.Add(new LifeRecordItem(970, 1402, 1404, new string[6] { "Location", "", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 50, -1, ELifeRecordDisplayType.Produce));
		_dataArray.Add(new LifeRecordItem(971, 1405, 1406, new string[6] { "Location", "Integer", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 50, -1, ELifeRecordDisplayType.Produce));
		_dataArray.Add(new LifeRecordItem(972, 1405, 1407, new string[6] { "Location", "Integer", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 50, -1, ELifeRecordDisplayType.Produce));
		_dataArray.Add(new LifeRecordItem(973, 1408, 1409, new string[6] { "Location", "", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 50, -1, ELifeRecordDisplayType.Produce));
		_dataArray.Add(new LifeRecordItem(974, 1408, 1410, new string[6] { "Location", "", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 50, -1, ELifeRecordDisplayType.Produce));
		_dataArray.Add(new LifeRecordItem(975, 1411, 1412, new string[6] { "SwordTomb", "", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 60, -1, ELifeRecordDisplayType.Produce));
		_dataArray.Add(new LifeRecordItem(976, 1413, 1414, new string[6] { "CharacterTemplate", "", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 40, -1, ELifeRecordDisplayType.Combat));
		_dataArray.Add(new LifeRecordItem(977, 1415, 1416, new string[6] { "Location", "", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 50, -1, ELifeRecordDisplayType.Produce));
		_dataArray.Add(new LifeRecordItem(978, 1417, 1418, new string[6] { "Location", "", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 50, -1, ELifeRecordDisplayType.Produce));
		_dataArray.Add(new LifeRecordItem(979, 1369, 1419, new string[6] { "Location", "Item", "Item", "Item", "Item", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 60, -1, ELifeRecordDisplayType.Produce));
		_dataArray.Add(new LifeRecordItem(980, 1366, 1420, new string[6] { "Location", "Item", "Item", "Item", "Item", "Item" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 60, -1, ELifeRecordDisplayType.Produce));
		_dataArray.Add(new LifeRecordItem(981, 1369, 1421, new string[6] { "Location", "Item", "Item", "Item", "Item", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 60, -1, ELifeRecordDisplayType.Produce));
		_dataArray.Add(new LifeRecordItem(982, 1366, 1422, new string[6] { "Location", "Item", "Item", "Item", "Item", "Item" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 60, -1, ELifeRecordDisplayType.Produce));
		_dataArray.Add(new LifeRecordItem(983, 1423, 1424, new string[6] { "Location", "Building", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 40, -1, ELifeRecordDisplayType.Produce));
		_dataArray.Add(new LifeRecordItem(984, 1423, 1425, new string[6] { "Location", "", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 40, -1, ELifeRecordDisplayType.Produce));
		_dataArray.Add(new LifeRecordItem(985, 1426, 1427, new string[6] { "Location", "Building", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 40, -1, ELifeRecordDisplayType.Produce));
		_dataArray.Add(new LifeRecordItem(986, 1428, 1429, new string[6] { "Location", "", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 40, -1, ELifeRecordDisplayType.Produce));
		_dataArray.Add(new LifeRecordItem(987, 1430, 1431, new string[6] { "Location", "", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 40, -1, ELifeRecordDisplayType.Produce));
		_dataArray.Add(new LifeRecordItem(988, 1432, 1433, new string[6] { "Location", "", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 40, -1, ELifeRecordDisplayType.Produce));
		_dataArray.Add(new LifeRecordItem(989, 1426, 1434, new string[6] { "Location", "Building", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 40, -1, ELifeRecordDisplayType.Produce));
		_dataArray.Add(new LifeRecordItem(990, 1435, 1436, new string[6] { "Location", "", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 40, -1, ELifeRecordDisplayType.Produce));
		_dataArray.Add(new LifeRecordItem(991, 1437, 1438, new string[6] { "Location", "", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 40, -1, ELifeRecordDisplayType.Produce));
		_dataArray.Add(new LifeRecordItem(992, 1439, 1440, new string[6] { "Location", "", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 40, -1, ELifeRecordDisplayType.Produce));
		_dataArray.Add(new LifeRecordItem(993, 1423, 1441, new string[6] { "Location", "", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 40, -1, ELifeRecordDisplayType.Produce));
		_dataArray.Add(new LifeRecordItem(994, 1426, 1442, new string[6] { "Location", "", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 40, -1, ELifeRecordDisplayType.Produce));
		_dataArray.Add(new LifeRecordItem(995, 1426, 1443, new string[6] { "Location", "", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 40, -1, ELifeRecordDisplayType.Produce));
		_dataArray.Add(new LifeRecordItem(996, 1444, 1445, new string[6] { "Location", "LifeSkillType", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 40, -1, ELifeRecordDisplayType.Produce));
		_dataArray.Add(new LifeRecordItem(997, 1446, 1447, new string[6] { "Settlement", "Integer", "Location", "Integer", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 30, -1, ELifeRecordDisplayType.Negative));
		_dataArray.Add(new LifeRecordItem(998, 1448, 1449, new string[6] { "", "", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 30, -1, ELifeRecordDisplayType.Negative));
		_dataArray.Add(new LifeRecordItem(999, 1450, 1451, new string[6] { "Integer", "", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 30, -1, ELifeRecordDisplayType.Negative));
		_dataArray.Add(new LifeRecordItem(1000, 1452, 1453, new string[6] { "Integer", "", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 30, -1, ELifeRecordDisplayType.Negative));
		_dataArray.Add(new LifeRecordItem(1001, 1454, 1455, new string[6] { "Integer", "", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 30, -1, ELifeRecordDisplayType.Negative));
		_dataArray.Add(new LifeRecordItem(1002, 1456, 1457, new string[6] { "", "", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 30, -1, ELifeRecordDisplayType.Negative));
		_dataArray.Add(new LifeRecordItem(1003, 1458, 1459, new string[6] { "Integer", "", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 30, -1, ELifeRecordDisplayType.Negative));
		_dataArray.Add(new LifeRecordItem(1004, 1460, 1461, new string[6] { "Integer", "", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 30, -1, ELifeRecordDisplayType.Negative));
		_dataArray.Add(new LifeRecordItem(1005, 1462, 1463, new string[6] { "Integer", "", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 30, -1, ELifeRecordDisplayType.Negative));
		_dataArray.Add(new LifeRecordItem(1006, 1464, 1465, new string[6] { "", "", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 30, -1, ELifeRecordDisplayType.Negative));
		_dataArray.Add(new LifeRecordItem(1007, 1466, 1467, new string[6] { "Integer", "", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 30, -1, ELifeRecordDisplayType.Negative));
		_dataArray.Add(new LifeRecordItem(1008, 1468, 1469, new string[6] { "Integer", "", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 30, -1, ELifeRecordDisplayType.Negative));
		_dataArray.Add(new LifeRecordItem(1009, 1470, 1471, new string[6] { "Integer", "", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 30, -1, ELifeRecordDisplayType.Negative));
		_dataArray.Add(new LifeRecordItem(1010, 1472, 1473, new string[6] { "Integer", "", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 30, -1, ELifeRecordDisplayType.Negative));
		_dataArray.Add(new LifeRecordItem(1011, 1474, 1475, new string[6] { "Integer", "", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 30, -1, ELifeRecordDisplayType.Negative));
		_dataArray.Add(new LifeRecordItem(1012, 1476, 1477, new string[6] { "Integer", "", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 30, -1, ELifeRecordDisplayType.Negative));
		_dataArray.Add(new LifeRecordItem(1013, 1321, 1478, new string[6] { "PunishmentType", "Settlement", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 0, -1, ELifeRecordDisplayType.Great));
		_dataArray.Add(new LifeRecordItem(1014, 1321, 1479, new string[6] { "Character", "Settlement", "Integer", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 0, -1, ELifeRecordDisplayType.Great));
		_dataArray.Add(new LifeRecordItem(1015, 1321, 1480, new string[6] { "Character", "Settlement", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 0, -1, ELifeRecordDisplayType.Great));
		_dataArray.Add(new LifeRecordItem(1016, 1481, 1482, new string[6] { "Character", "Settlement", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 40, -1, ELifeRecordDisplayType.Relation));
		_dataArray.Add(new LifeRecordItem(1017, 1481, 1483, new string[6] { "Settlement", "", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 40, -1, ELifeRecordDisplayType.Relation));
		_dataArray.Add(new LifeRecordItem(1018, 1481, 1484, new string[6] { "Character", "Settlement", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 40, -1, ELifeRecordDisplayType.Relation));
		_dataArray.Add(new LifeRecordItem(1019, 1481, 1485, new string[6] { "Settlement", "", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 40, -1, ELifeRecordDisplayType.Relation));
	}

	private void CreateItems17()
	{
		_dataArray.Add(new LifeRecordItem(1020, 1481, 1486, new string[6] { "Character", "Settlement", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 40, -1, ELifeRecordDisplayType.Relation));
		_dataArray.Add(new LifeRecordItem(1021, 1481, 1487, new string[6] { "Settlement", "", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 40, -1, ELifeRecordDisplayType.Relation));
		_dataArray.Add(new LifeRecordItem(1022, 792, 1488, new string[6] { "Character", "Location", "Character", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 60, -1, ELifeRecordDisplayType.Relation));
		_dataArray.Add(new LifeRecordItem(1023, 1489, 1490, new string[6] { "Character", "Location", "Item", "", "", "" }, isSourceRecord: true, new List<short> { 1024 }, -30000, ELifeRecordScoreType.Normal, 60, -1, ELifeRecordDisplayType.Relation));
		_dataArray.Add(new LifeRecordItem(1024, 1489, 1491, new string[6] { "", "", "", "", "", "" }, isSourceRecord: false, new List<short> { 1023 }, -30000, ELifeRecordScoreType.Normal, 40, -1, ELifeRecordDisplayType.Relation));
		_dataArray.Add(new LifeRecordItem(1025, 1492, 1493, new string[6] { "Settlement", "", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 60, -1, ELifeRecordDisplayType.Relation));
		_dataArray.Add(new LifeRecordItem(1026, 1492, 1494, new string[6] { "Character", "Settlement", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 60, -1, ELifeRecordDisplayType.Relation));
		_dataArray.Add(new LifeRecordItem(1027, 1472, 1495, new string[6] { "Character", "", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 50, -1, ELifeRecordDisplayType.Negative));
		_dataArray.Add(new LifeRecordItem(1028, 1472, 1496, new string[6] { "Character", "Location", "", "", "", "" }, isSourceRecord: true, new List<short> { 1029 }, -30000, ELifeRecordScoreType.Normal, 70, -1, ELifeRecordDisplayType.Great));
		_dataArray.Add(new LifeRecordItem(1029, 1472, 1497, new string[6] { "", "", "", "", "", "" }, isSourceRecord: false, new List<short> { 1028 }, -30000, ELifeRecordScoreType.Normal, 0, -1, ELifeRecordDisplayType.Great));
		_dataArray.Add(new LifeRecordItem(1030, 1472, 1498, new string[6] { "Character", "Location", "", "", "", "" }, isSourceRecord: true, new List<short> { 1031 }, -30000, ELifeRecordScoreType.Normal, 30, -1, ELifeRecordDisplayType.Great));
		_dataArray.Add(new LifeRecordItem(1031, 1472, 1499, new string[6] { "", "", "", "", "", "" }, isSourceRecord: false, new List<short> { 1030 }, -30000, ELifeRecordScoreType.Normal, 70, -1, ELifeRecordDisplayType.Great));
		_dataArray.Add(new LifeRecordItem(1032, 1472, 1500, new string[6] { "Character", "", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 40, -1, ELifeRecordDisplayType.Great));
		_dataArray.Add(new LifeRecordItem(1033, 826, 1501, new string[6] { "Character", "", "", "", "", "" }, isSourceRecord: true, new List<short> { 1034 }, -30000, ELifeRecordScoreType.Normal, 60, -1, ELifeRecordDisplayType.Great));
		_dataArray.Add(new LifeRecordItem(1034, 826, 1502, new string[6] { "", "", "", "", "", "" }, isSourceRecord: false, new List<short> { 1033 }, -30000, ELifeRecordScoreType.Normal, 30, -1, ELifeRecordDisplayType.Great));
		_dataArray.Add(new LifeRecordItem(1035, 1292, 1503, new string[6] { "", "", "", "", "", "" }, isSourceRecord: false, new List<short> { 885 }, -30000, ELifeRecordScoreType.Normal, 60, -1, ELifeRecordDisplayType.Combat));
		_dataArray.Add(new LifeRecordItem(1036, 1292, 1504, new string[6] { "", "", "", "", "", "" }, isSourceRecord: false, new List<short> { 888 }, -30000, ELifeRecordScoreType.Normal, 40, -1, ELifeRecordDisplayType.Combat));
		_dataArray.Add(new LifeRecordItem(1037, 1292, 1505, new string[6] { "", "", "", "", "", "" }, isSourceRecord: false, new List<short> { 1038 }, -30000, ELifeRecordScoreType.Normal, 40, -1, ELifeRecordDisplayType.Relation));
		_dataArray.Add(new LifeRecordItem(1038, 1292, 1506, new string[6] { "Character", "", "", "", "", "" }, isSourceRecord: true, new List<short> { 1037 }, -30000, ELifeRecordScoreType.Normal, 60, -1, ELifeRecordDisplayType.Relation));
		_dataArray.Add(new LifeRecordItem(1039, 1292, 1507, new string[6] { "", "", "", "", "", "" }, isSourceRecord: false, new List<short> { 1040 }, -30000, ELifeRecordScoreType.Normal, 60, -1, ELifeRecordDisplayType.Relation));
		_dataArray.Add(new LifeRecordItem(1040, 1292, 1508, new string[6] { "Character", "", "", "", "", "" }, isSourceRecord: true, new List<short> { 1039 }, -30000, ELifeRecordScoreType.Normal, 40, -1, ELifeRecordDisplayType.Relation));
		_dataArray.Add(new LifeRecordItem(1041, 1292, 1509, new string[6] { "", "", "", "", "", "" }, isSourceRecord: false, new List<short> { 1042 }, -30000, ELifeRecordScoreType.Normal, 40, -1, ELifeRecordDisplayType.Relation));
		_dataArray.Add(new LifeRecordItem(1042, 1292, 1510, new string[6] { "Character", "Integer", "", "", "", "" }, isSourceRecord: true, new List<short> { 1041 }, -30000, ELifeRecordScoreType.Normal, 60, -1, ELifeRecordDisplayType.Relation));
		_dataArray.Add(new LifeRecordItem(1043, 1292, 1511, new string[6] { "", "", "", "", "", "" }, isSourceRecord: false, new List<short> { 1044 }, -30000, ELifeRecordScoreType.Normal, 40, -1, ELifeRecordDisplayType.Relation));
		_dataArray.Add(new LifeRecordItem(1044, 1292, 1512, new string[6] { "Character", "Integer", "", "", "", "" }, isSourceRecord: true, new List<short> { 1043 }, -30000, ELifeRecordScoreType.Normal, 60, -1, ELifeRecordDisplayType.Relation));
		_dataArray.Add(new LifeRecordItem(1045, 1292, 1513, new string[6] { "", "", "", "", "", "" }, isSourceRecord: false, new List<short> { 1046 }, -30000, ELifeRecordScoreType.Normal, 40, -1, ELifeRecordDisplayType.Relation));
		_dataArray.Add(new LifeRecordItem(1046, 1292, 1514, new string[6] { "Character", "Integer", "", "", "", "" }, isSourceRecord: true, new List<short> { 1045 }, -30000, ELifeRecordScoreType.Normal, 60, -1, ELifeRecordDisplayType.Relation));
		_dataArray.Add(new LifeRecordItem(1047, 1292, 1515, new string[6] { "Character", "Settlement", "", "", "", "" }, isSourceRecord: true, new List<short> { 1048 }, -30000, ELifeRecordScoreType.Normal, 40, -1, ELifeRecordDisplayType.Relation));
		_dataArray.Add(new LifeRecordItem(1048, 1292, 1516, new string[6] { "", "", "", "", "", "" }, isSourceRecord: false, new List<short> { 1047 }, -30000, ELifeRecordScoreType.Normal, 60, -1, ELifeRecordDisplayType.Relation));
		_dataArray.Add(new LifeRecordItem(1049, 1517, 1518, new string[6] { "Location", "Character", "Profession", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 50, -1, ELifeRecordDisplayType.Normal));
		_dataArray.Add(new LifeRecordItem(1050, 1519, 1520, new string[6] { "Item", "Item", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 50, -1, ELifeRecordDisplayType.Normal));
		_dataArray.Add(new LifeRecordItem(1051, 1521, 1522, new string[6] { "Character", "Location", "", "", "", "" }, isSourceRecord: true, new List<short> { 1052 }, -30000, ELifeRecordScoreType.Normal, 70, -1, ELifeRecordDisplayType.Relation));
		_dataArray.Add(new LifeRecordItem(1052, 1521, 1523, new string[6] { "", "", "", "", "", "" }, isSourceRecord: false, new List<short> { 1051 }, -30000, ELifeRecordScoreType.Normal, 70, -1, ELifeRecordDisplayType.Relation));
		_dataArray.Add(new LifeRecordItem(1053, 1524, 1525, new string[6] { "CombatSkill", "", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 60, -1, ELifeRecordDisplayType.Study));
		_dataArray.Add(new LifeRecordItem(1054, 1524, 1526, new string[6] { "CombatSkill", "Item", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 60, -1, ELifeRecordDisplayType.Study));
		_dataArray.Add(new LifeRecordItem(1055, 1524, 1527, new string[6] { "CombatSkill", "Character", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 60, -1, ELifeRecordDisplayType.Study));
		_dataArray.Add(new LifeRecordItem(1056, 1524, 1528, new string[6] { "CombatSkill", "Character", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 60, -1, ELifeRecordDisplayType.Study));
		_dataArray.Add(new LifeRecordItem(1057, 1529, 1530, new string[6] { "", "", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 40, -1, ELifeRecordDisplayType.Negative));
		_dataArray.Add(new LifeRecordItem(1058, 1531, 1532, new string[6] { "Location", "Character", "Item", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 40, -1, ELifeRecordDisplayType.Negative));
		_dataArray.Add(new LifeRecordItem(1059, 1531, 1533, new string[6] { "Location", "Character", "Item", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 40, -1, ELifeRecordDisplayType.Negative));
		_dataArray.Add(new LifeRecordItem(1060, 1534, 1535, new string[6] { "Character", "Settlement", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 70, -1, ELifeRecordDisplayType.Relation));
		_dataArray.Add(new LifeRecordItem(1061, 1534, 1536, new string[6] { "Character", "Settlement", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 70, -1, ELifeRecordDisplayType.Relation));
		_dataArray.Add(new LifeRecordItem(1062, 1534, 1537, new string[6] { "Character", "Settlement", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 70, -1, ELifeRecordDisplayType.Relation));
		_dataArray.Add(new LifeRecordItem(1063, 1534, 1538, new string[6] { "Character", "Settlement", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 70, -1, ELifeRecordDisplayType.Relation));
		_dataArray.Add(new LifeRecordItem(1064, 1539, 1540, new string[6] { "Item", "", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 60, -1, ELifeRecordDisplayType.Produce));
		_dataArray.Add(new LifeRecordItem(1065, 1541, 1542, new string[6] { "Item", "Item", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 70, -1, ELifeRecordDisplayType.Produce));
		_dataArray.Add(new LifeRecordItem(1066, 1543, 1544, new string[6] { "Character", "Location", "Integer", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 60, -1, ELifeRecordDisplayType.Produce));
		_dataArray.Add(new LifeRecordItem(1067, 1543, 1545, new string[6] { "", "", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 70, -1, ELifeRecordDisplayType.Produce));
		_dataArray.Add(new LifeRecordItem(1068, 1546, 1547, new string[6] { "Character", "Resource", "Integer", "", "", "" }, isSourceRecord: true, new List<short> { 1069 }, -30000, ELifeRecordScoreType.Normal, 60, -1, ELifeRecordDisplayType.Produce));
		_dataArray.Add(new LifeRecordItem(1069, 1546, 1548, new string[6] { "", "", "", "", "", "" }, isSourceRecord: false, new List<short> { 1068 }, -30000, ELifeRecordScoreType.Normal, 40, -1, ELifeRecordDisplayType.Produce));
		_dataArray.Add(new LifeRecordItem(1070, 1549, 1550, new string[6] { "", "", "", "", "", "" }, isSourceRecord: false, new List<short> { 947 }, -30000, ELifeRecordScoreType.Normal, 50, -1, ELifeRecordDisplayType.Produce));
		_dataArray.Add(new LifeRecordItem(1071, 1551, 1552, new string[6] { "", "", "", "", "", "" }, isSourceRecord: false, new List<short> { 946 }, -30000, ELifeRecordScoreType.Normal, 50, -1, ELifeRecordDisplayType.Produce));
		_dataArray.Add(new LifeRecordItem(1072, 1553, 1554, new string[6] { "Character", "Location", "MerchantType", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 60, -1, ELifeRecordDisplayType.Produce));
		_dataArray.Add(new LifeRecordItem(1073, 1553, 1555, new string[6] { "Character", "Location", "MerchantType", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 60, -1, ELifeRecordDisplayType.Produce));
		_dataArray.Add(new LifeRecordItem(1074, 1396, 1556, new string[6] { "Character", "Location", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 60, -1, ELifeRecordDisplayType.Produce));
		_dataArray.Add(new LifeRecordItem(1075, 1396, 1557, new string[6] { "Character", "Location", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 40, -1, ELifeRecordDisplayType.Produce));
		_dataArray.Add(new LifeRecordItem(1076, 1402, 1403, new string[6] { "Settlement", "", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 50, -1, ELifeRecordDisplayType.Produce));
		_dataArray.Add(new LifeRecordItem(1077, 1402, 1404, new string[6] { "Settlement", "", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 50, -1, ELifeRecordDisplayType.Produce));
		_dataArray.Add(new LifeRecordItem(1078, 1402, 1558, new string[6] { "Settlement", "", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 50, -1, ELifeRecordDisplayType.Produce));
		_dataArray.Add(new LifeRecordItem(1079, 1402, 1559, new string[6] { "Settlement", "", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 50, -1, ELifeRecordDisplayType.Produce));
	}

	private void CreateItems18()
	{
		_dataArray.Add(new LifeRecordItem(1080, 1399, 1560, new string[6] { "Settlement", "Integer", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 50, -1, ELifeRecordDisplayType.Produce));
		_dataArray.Add(new LifeRecordItem(1081, 1399, 1561, new string[6] { "Settlement", "Integer", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 50, -1, ELifeRecordDisplayType.Produce));
		_dataArray.Add(new LifeRecordItem(1082, 1399, 1562, new string[6] { "Character", "Settlement", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 50, -1, ELifeRecordDisplayType.Produce));
		_dataArray.Add(new LifeRecordItem(1083, 1399, 1563, new string[6] { "Character", "Settlement", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 50, -1, ELifeRecordDisplayType.Produce));
		_dataArray.Add(new LifeRecordItem(1084, 1399, 1564, new string[6] { "Character", "Settlement", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 50, -1, ELifeRecordDisplayType.Produce));
		_dataArray.Add(new LifeRecordItem(1085, 1399, 1565, new string[6] { "Character", "Settlement", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 50, -1, ELifeRecordDisplayType.Produce));
		_dataArray.Add(new LifeRecordItem(1086, 1413, 1566, new string[6] { "Location", "", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 30, -1, ELifeRecordDisplayType.Negative));
		_dataArray.Add(new LifeRecordItem(1087, 1413, 1567, new string[6] { "CharacterTemplate", "", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 80, -1, ELifeRecordDisplayType.Combat));
		_dataArray.Add(new LifeRecordItem(1088, 1568, 1569, new string[6] { "Character", "Character", "Location", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 40, -1, ELifeRecordDisplayType.Relation));
		_dataArray.Add(new LifeRecordItem(1089, 58, 1570, new string[6] { "Character", "Character", "Location", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 60, -1, ELifeRecordDisplayType.Relation));
		_dataArray.Add(new LifeRecordItem(1090, 1571, 1572, new string[6] { "Character", "Location", "", "", "", "" }, isSourceRecord: true, new List<short> { 1095 }, -30000, ELifeRecordScoreType.Normal, 50, -1, ELifeRecordDisplayType.Produce));
		_dataArray.Add(new LifeRecordItem(1091, 1573, 1574, new string[6] { "Character", "Item", "", "", "", "" }, isSourceRecord: true, new List<short> { 1096 }, -30000, ELifeRecordScoreType.Normal, 60, -1, ELifeRecordDisplayType.Produce));
		_dataArray.Add(new LifeRecordItem(1092, 1589, 1590, new string[6] { "Item", "", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 50, -1, ELifeRecordDisplayType.Produce));
		_dataArray.Add(new LifeRecordItem(1093, 1579, 1580, new string[6] { "Character", "Character", "", "", "", "" }, isSourceRecord: true, new List<short> { 1094 }, -30000, ELifeRecordScoreType.Normal, 50, -1, ELifeRecordDisplayType.Relation));
		_dataArray.Add(new LifeRecordItem(1094, 1579, 1581, new string[6] { "", "", "", "", "", "" }, isSourceRecord: false, new List<short> { 1093 }, -30000, ELifeRecordScoreType.Normal, 50, -1, ELifeRecordDisplayType.Relation));
		_dataArray.Add(new LifeRecordItem(1095, 1575, 1576, new string[6] { "", "", "", "", "", "" }, isSourceRecord: false, new List<short> { 1090 }, -30000, ELifeRecordScoreType.Normal, 50, -1, ELifeRecordDisplayType.Produce));
		_dataArray.Add(new LifeRecordItem(1096, 1577, 1578, new string[6] { "", "", "", "", "", "" }, isSourceRecord: false, new List<short> { 1091 }, -30000, ELifeRecordScoreType.Normal, 60, -1, ELifeRecordDisplayType.Produce));
		_dataArray.Add(new LifeRecordItem(1097, 1579, 1582, new string[6] { "Character", "Character", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 50, -1, ELifeRecordDisplayType.Relation));
		_dataArray.Add(new LifeRecordItem(1098, 1583, 1584, new string[6] { "Character", "Character", "Location", "", "", "" }, isSourceRecord: true, new List<short> { 1099 }, -30000, ELifeRecordScoreType.Normal, 50, -1, ELifeRecordDisplayType.Produce));
		_dataArray.Add(new LifeRecordItem(1099, 1575, 1585, new string[6] { "", "", "", "", "", "" }, isSourceRecord: false, new List<short> { 1098 }, -30000, ELifeRecordScoreType.Normal, 50, -1, ELifeRecordDisplayType.Produce));
		_dataArray.Add(new LifeRecordItem(1100, 1586, 1587, new string[6] { "Character", "Character", "Item", "", "", "" }, isSourceRecord: true, new List<short> { 1101 }, -30000, ELifeRecordScoreType.Normal, 50, -1, ELifeRecordDisplayType.Produce));
		_dataArray.Add(new LifeRecordItem(1101, 1577, 1588, new string[6] { "", "", "", "", "", "" }, isSourceRecord: false, new List<short> { 1100 }, -30000, ELifeRecordScoreType.Normal, 50, -1, ELifeRecordDisplayType.Produce));
		_dataArray.Add(new LifeRecordItem(1102, 1591, 1592, new string[6] { "Building", "LifeSkillType", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 60, -1, ELifeRecordDisplayType.Study));
		_dataArray.Add(new LifeRecordItem(1103, 1591, 1592, new string[6] { "Building", "CombatSkillType", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 60, -1, ELifeRecordDisplayType.Study));
		_dataArray.Add(new LifeRecordItem(1104, 1591, 1593, new string[6] { "Building", "LifeSkillType", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 70, -1, ELifeRecordDisplayType.Study));
		_dataArray.Add(new LifeRecordItem(1105, 1591, 1593, new string[6] { "Building", "CombatSkillType", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 70, -1, ELifeRecordDisplayType.Study));
		_dataArray.Add(new LifeRecordItem(1106, 1591, 1594, new string[6] { "Character", "Building", "LifeSkillType", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 70, -1, ELifeRecordDisplayType.Study));
		_dataArray.Add(new LifeRecordItem(1107, 1591, 1594, new string[6] { "Character", "Building", "CombatSkillType", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 70, -1, ELifeRecordDisplayType.Study));
		_dataArray.Add(new LifeRecordItem(1108, 1595, 1596, new string[6] { "Building", "Item", "Integer", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 60, -1, ELifeRecordDisplayType.Study));
		_dataArray.Add(new LifeRecordItem(1109, 1595, 1597, new string[6] { "Building", "Item", "Integer", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 60, -1, ELifeRecordDisplayType.Relation));
		_dataArray.Add(new LifeRecordItem(1110, 1598, 1599, new string[6] { "Settlement", "", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 60, -1, ELifeRecordDisplayType.Study));
		_dataArray.Add(new LifeRecordItem(1111, 1600, 1601, new string[6] { "Item", "", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 60, -1, ELifeRecordDisplayType.Study));
		_dataArray.Add(new LifeRecordItem(1112, 1602, 1603, new string[6] { "Building", "Integer", "Resource", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 60, -1, ELifeRecordDisplayType.Produce));
		_dataArray.Add(new LifeRecordItem(1113, 1604, 1605, new string[6] { "Location", "OrgGrade", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 40, -1, ELifeRecordDisplayType.Relation));
		_dataArray.Add(new LifeRecordItem(1114, 1606, 1607, new string[6] { "Location", "Item", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 50, -1, ELifeRecordDisplayType.Produce));
		_dataArray.Add(new LifeRecordItem(1115, 1608, 1609, new string[6] { "Settlement", "OrgGrade", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 50, -1, ELifeRecordDisplayType.Relation));
		_dataArray.Add(new LifeRecordItem(1116, 1610, 1611, new string[6] { "Settlement", "", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 50, -1, ELifeRecordDisplayType.Relation));
		_dataArray.Add(new LifeRecordItem(1117, 1612, 1613, new string[6] { "Settlement", "Settlement", "OrgGrade", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 50, -1, ELifeRecordDisplayType.Relation));
		_dataArray.Add(new LifeRecordItem(1118, 1614, 1615, new string[6] { "Character", "Character", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 60, -1, ELifeRecordDisplayType.Relation));
		_dataArray.Add(new LifeRecordItem(1119, 1614, 1616, new string[6] { "Character", "Character", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 40, -1, ELifeRecordDisplayType.Relation));
		_dataArray.Add(new LifeRecordItem(1120, 1614, 1617, new string[6] { "Character", "Character", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 60, -1, ELifeRecordDisplayType.Relation));
		_dataArray.Add(new LifeRecordItem(1121, 1614, 1618, new string[6] { "Character", "Character", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 40, -1, ELifeRecordDisplayType.Relation));
		_dataArray.Add(new LifeRecordItem(1122, 1619, 1620, new string[6] { "Location", "Character", "Character", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 50, -1, ELifeRecordDisplayType.Relation));
		_dataArray.Add(new LifeRecordItem(1123, 1619, 1621, new string[6] { "Location", "Character", "Character", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 50, -1, ELifeRecordDisplayType.Relation));
		_dataArray.Add(new LifeRecordItem(1124, 1619, 1622, new string[6] { "Location", "Character", "Character", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 50, -1, ELifeRecordDisplayType.Relation));
		_dataArray.Add(new LifeRecordItem(1125, 1619, 1623, new string[6] { "Location", "Character", "Character", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 50, -1, ELifeRecordDisplayType.Relation));
		_dataArray.Add(new LifeRecordItem(1126, 1619, 1624, new string[6] { "Location", "Character", "Location", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 50, -1, ELifeRecordDisplayType.Relation));
		_dataArray.Add(new LifeRecordItem(1127, 1619, 1625, new string[6] { "Location", "Character", "Location", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 50, -1, ELifeRecordDisplayType.Relation));
		_dataArray.Add(new LifeRecordItem(1128, 1619, 1626, new string[6] { "Location", "Character", "Character", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 50, -1, ELifeRecordDisplayType.Relation));
		_dataArray.Add(new LifeRecordItem(1129, 1619, 1627, new string[6] { "Location", "Character", "Item", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 50, -1, ELifeRecordDisplayType.Relation));
		_dataArray.Add(new LifeRecordItem(1130, 1619, 1628, new string[6] { "Location", "Character", "Location", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 50, -1, ELifeRecordDisplayType.Relation));
		_dataArray.Add(new LifeRecordItem(1131, 1619, 1629, new string[6] { "Settlement", "Character", "Settlement", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 50, -1, ELifeRecordDisplayType.Relation));
		_dataArray.Add(new LifeRecordItem(1132, 1630, 1631, new string[6] { "Location", "", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 0, -1, ELifeRecordDisplayType.Great));
		_dataArray.Add(new LifeRecordItem(1133, 1630, 1632, new string[6] { "Location", "", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 0, -1, ELifeRecordDisplayType.Great));
		_dataArray.Add(new LifeRecordItem(1134, 1633, 1634, new string[6] { "Character", "Location", "Item", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 50, -1, ELifeRecordDisplayType.Great));
		_dataArray.Add(new LifeRecordItem(1135, 1635, 1636, new string[6] { "Item", "Item", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 60, -1, ELifeRecordDisplayType.Relation));
		_dataArray.Add(new LifeRecordItem(1136, 1635, 1637, new string[6] { "Item", "Item", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 65, -1, ELifeRecordDisplayType.Relation));
		_dataArray.Add(new LifeRecordItem(1137, 1635, 1638, new string[6] { "Item", "Item", "Feast", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 70, -1, ELifeRecordDisplayType.Relation));
		_dataArray.Add(new LifeRecordItem(1138, 1635, 1639, new string[6] { "Item", "Item", "Feast", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 75, -1, ELifeRecordDisplayType.Relation));
		_dataArray.Add(new LifeRecordItem(1139, 1635, 1640, new string[6] { "Item", "Item", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 55, -1, ELifeRecordDisplayType.Relation));
	}

	private void CreateItems19()
	{
		_dataArray.Add(new LifeRecordItem(1140, 1635, 1641, new string[6] { "Item", "Item", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 60, -1, ELifeRecordDisplayType.Relation));
		_dataArray.Add(new LifeRecordItem(1141, 1635, 1642, new string[6] { "Item", "Item", "Feast", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 65, -1, ELifeRecordDisplayType.Relation));
		_dataArray.Add(new LifeRecordItem(1142, 1635, 1643, new string[6] { "Item", "Item", "Feast", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 70, -1, ELifeRecordDisplayType.Relation));
		_dataArray.Add(new LifeRecordItem(1143, 1635, 1644, new string[6] { "", "", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 30, -1, ELifeRecordDisplayType.Relation));
		_dataArray.Add(new LifeRecordItem(1144, 1635, 1645, new string[6] { "", "", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 50, -1, ELifeRecordDisplayType.Relation));
		_dataArray.Add(new LifeRecordItem(1145, 1646, 1647, new string[6] { "Location", "", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 40, -1, ELifeRecordDisplayType.Negative));
		_dataArray.Add(new LifeRecordItem(1146, 1648, 1649, new string[6] { "Location", "", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 70, -1, ELifeRecordDisplayType.Great));
		_dataArray.Add(new LifeRecordItem(1147, 1650, 1651, new string[6] { "Character", "", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 50, -1, ELifeRecordDisplayType.Relation));
		_dataArray.Add(new LifeRecordItem(1148, 1652, 1653, new string[6] { "Settlement", "Character", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 50, -1, ELifeRecordDisplayType.Relation));
		_dataArray.Add(new LifeRecordItem(1149, 1652, 1654, new string[6] { "Settlement", "Character", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 50, -1, ELifeRecordDisplayType.Relation));
		_dataArray.Add(new LifeRecordItem(1150, 1652, 1655, new string[6] { "Settlement", "Character", "", "", "", "" }, isSourceRecord: true, new List<short>(), -30000, ELifeRecordScoreType.Normal, 50, -1, ELifeRecordDisplayType.Relation));
	}

	public override void Init()
	{
		base.Init();
		_dataArray = new List<LifeRecordItem>(1151);
		CreateItems0();
		CreateItems1();
		CreateItems2();
		CreateItems3();
		CreateItems4();
		CreateItems5();
		CreateItems6();
		CreateItems7();
		CreateItems8();
		CreateItems9();
		CreateItems10();
		CreateItems11();
		CreateItems12();
		CreateItems13();
		CreateItems14();
		CreateItems15();
		CreateItems16();
		CreateItems17();
		CreateItems18();
		CreateItems19();
	}
}
