using System;
using System.Collections.Generic;
using Config.Common;
using Config.ConfigCells;

namespace Config;

[Serializable]
public class TaskInfo : ConfigData<TaskInfoItem, int>
{
	public static class DefKey
	{
		public const int SideQuest_ConstructTaiwuVillage = 22;

		public const int SideQuest_AssignVillagers = 23;

		public const int Kongsang_MissionUnaccepted = 105;

		public const int Kongsang_SearchTarget0 = 106;

		public const int Kongsang_PoisonTest0 = 107;

		public const int Kongsang_ReplySect0 = 108;

		public const int Kongsang_SearchTarget1 = 109;

		public const int Kongsang_PoisonTest1 = 110;

		public const int Kongsang_ReplySect1 = 111;

		public const int Kongsang_SearchTarget2 = 112;

		public const int Kongsang_PoisonTest2 = 113;

		public const int Kongsang_ReplySect3 = 114;

		public const int Kongsang_FindWLiao = 115;

		public const int Kongsang_ReplySect2 = 116;

		public const int Kongsang_WaitForAdventure = 117;

		public const int Kongsang_AdventureAppeared = 118;

		public const int Xuehou_GraveDigging = 119;

		public const int Xuehou_RustBell = 120;

		public const int Xuehou_OldmanMyth = 121;

		public const int Xuehou_Oldman = 122;

		public const int Xuehou_CheckBloodBlock = 123;

		public const int Xuehou_AdventureGrave = 124;

		public const int Xuehou_BringJixiBack = 125;

		public const int Xuehou_StayWithJixi = 126;

		public const int Xuehou_MythInVillage = 127;

		public const int Xuehou_TruthClue1 = 128;

		public const int Xuehou_FalsityClue1 = 129;

		public const int Xuehou_TruthClue2 = 130;

		public const int Xuehou_FalsityClue2 = 131;

		public const int Xuehou_TruthClue3 = 132;

		public const int Xuehou_FalsityClue3 = 133;

		public const int Xuehou_InterrogateJixi = 134;

		public const int Xuehou_PassLegacy = 135;

		public const int Shaolin_MythinShaolin = 136;

		public const int Shaolin_MuddyStatue = 137;

		public const int Shaolin_ReturnStatue = 138;

		public const int Xuannv_QinAndQing = 139;

		public const int Xuannv_WaitForLetters = 140;

		public const int Xuannv_SeekLetterSender = 141;

		public const int Xuannv_SeekXuannv = 142;

		public const int Xuannv_AdventureSoulInMirror = 143;

		public const int Xuannv_RefusedToSearch = 144;

		public const int Xuannv_AcceptedToSearch = 145;

		public const int Xuannv_AdventureIllusionOfMirror = 146;

		public const int Xuannv_SeekLove = 147;

		public const int Xuannv_WaitForReturn = 148;

		public const int Wudang_Prologue = 149;

		public const int Wudang_WaitForSlobbyTaoistMonk = 150;

		public const int Wudang_SloppyTaoistMonkRequest = 151;

		public const int Wudang_SeekSite = 152;

		public const int Wudang_ReturnToSloppyTaoistMonk = 153;

		public const int Wudang_AskWudang = 154;

		public const int Yuanshan_InPrison = 155;

		public const int Yuanshan_MythInPrison = 156;

		public const int Yuanshan_MythInYuanshan = 157;

		public const int Yuanshan_FindHead = 158;

		public const int Yuanshan_Diplomacy = 159;

		public const int Yuanshan_RequestNotAccepted = 160;

		public const int Yuanshan_ReplyToHead = 161;

		public const int Yuanshan_ToBeInDanger = 162;

		public const int Yuanshan_InDanger = 163;

		public const int Yuanshan_DemonDiplomat = 164;

		public const int Yuanshan_DemonAnnihilation1and2 = 165;

		public const int Yuanshan_ReplyToHeadAgain = 166;

		public const int Yuanshan_DemonEscaped = 167;

		public const int Yuanshan_DemonAnnihilation3 = 168;

		public const int Shixiang_Myth = 169;

		public const int Shixiang_PoemAdventure = 170;

		public const int Shixiang_WaitforLetters = 171;

		public const int Shixiang_Arrive = 172;

		public const int Shixiang_Heretics = 173;

		public const int Shixiang_ReplyHead = 174;

		public const int Shixiang_Stay = 175;

		public const int Shixiang_Traitors = 176;

		public const int Shixiang_Barbarians = 177;

		public const int Shixiang_Ending = 178;

		public const int Jingang_Poverty = 179;

		public const int Jingang_ToJingang = 180;

		public const int Jingang_Adventure = 181;

		public const int Jingang_Haunted = 182;

		public const int Jingang_AssistReincarnation = 183;

		public const int Jingang_WaitForReincarnation = 184;

		public const int Jingang_ToKunlun = 185;

		public const int Jingang_SearchMonk = 186;

		public const int Jingang_SecInfoSpreading = 187;

		public const int Jingang_RefuseAndFailing = 188;

		public const int Jingang_MonkEnding = 189;

		public const int Jingang_BackToJingang = 190;

		public const int Jingang_ReturnSutra = 191;

		public const int Jingang_Ending = 192;

		public const int Wuxian_Bath = 193;

		public const int Wuxian_ToBaihua = 194;

		public const int Wuxian_AcceptRequest = 195;

		public const int Wuxian_ToKongsang = 196;

		public const int Wuxian_BackHome = 197;

		public const int Wuxian_WishComeTrue = 198;

		public const int Wuxian_Adventure = 199;

		public const int Emei_HomocideCases = 200;

		public const int Emei_Clues = 201;

		public const int Emei_InvestigateWhiteGibbon = 202;

		public const int Emei_Orthrodox = 203;

		public const int Emei_OrthrodoxAdventure = 204;

		public const int Jieqing_AskHead = 205;

		public const int Jieqing_Abyss = 206;

		public const int Jieqing_DukkhaStones = 207;

		public const int Jieqing_DukkhaOfBirth = 208;

		public const int Jieqing_DukkhaOfAging = 209;

		public const int Jieqing_DukkhaOfSickness = 210;

		public const int Jieqing_DukkhaOfDeath = 211;

		public const int Jieqing_DukkhaOfSeparation = 212;

		public const int Jieqing_DukkhaOfDespisement = 213;

		public const int Jieqing_DukkhaOfCraving = 214;

		public const int Jieqing_AbyssEnd = 215;

		public const int Ranshan_Invitation = 216;

		public const int Ranshan_Prologue = 217;

		public const int Ranshan_Coaching = 218;

		public const int Ranshan_Competition = 219;

		public const int Shaolin_StayAndWait = 220;

		public const int Shaolin_BodhidharmaInDream = 221;

		public const int Shaolin_BrokenStatue = 222;

		public const int Shaolin_Conflict = 223;

		public const int Shaolin_Endeavor = 224;

		public const int Shaolin_WaitForBodhidharma = 225;

		public const int Shaolin_DiggingSutra = 226;

		public const int Shaolin_StudyForBodhidharmaChallenge = 227;

		public const int Shaolin_ObtainedSutra = 228;

		public const int Shaolin_ReadSutra = 229;

		public const int Shaolin_VisitShaolin = 230;

		public const int Shaolin_SutraLibrary = 231;

		public const int Shixiang_Anecdote = 232;

		public const int Shixiang_ArriveSect = 233;

		public const int Shixiang_WaitForBattles = 234;

		public const int Wudang_CultivateHeavenlyTreeMain = 236;

		public const int Wudang_PlantHeavenlyTree = 237;

		public const int Wudang_ProtectHeavenlyTree = 238;

		public const int Wudang_ReadTaoistBook = 239;

		public const int Wudang_GetHeavenlyTreeSeed = 240;

		public const int Wudang_TaoistMonkSacrifices = 241;

		public const int Wudang_WaitForTimePassing = 242;

		public const int Wudang_VisitWudang = 243;

		public const int Wudang_WaitForImmortals = 244;

		public const int Shixiang_AskForJokes = 245;

		public const int Xuannv_Study = 246;

		public const int Xuannv_ReturnToMirror = 247;

		public const int Xuannv_TakeShiToXuannv = 248;

		public const int Xuannv_WaitForMessage = 249;

		public const int Xuannv_AskShadow = 250;

		public const int Xuannv_AskJuner = 251;

		public const int Xuannv_AskXuannvSect = 252;

		public const int Xuannv_FailAndRetake = 253;

		public const int Xuannv_ReadyToStudy = 256;

		public const int Emei_WaitForAdventure = 254;

		public const int Emei_SeekWhiteGibbon = 255;

		public const int PlayerShadowInMirror = 257;

		public const int Emei_WhiteGibbonsDisappeared = 258;

		public const int Emei_ShiHoujiuDisappeared = 259;

		public const int CrossArchive_FollowFuyu = 260;

		public const int CrossArchive_Items = 261;

		public const int CrossArchive_LifeSkills = 262;

		public const int CrossArchive_CombatSkills = 263;

		public const int LoongDLCToVillage = 264;

		public const int LoongDLCCaptureLoong = 265;

		public const int LoongDLCNurtureJiao = 266;

		public const int LoongDLCNurtureJiuzi = 267;

		public const int ChallengeWhiteLoong = 268;

		public const int ChallengeBlackLoong = 269;

		public const int ChallengeBlueLoong = 270;

		public const int ChallengeRedLoong = 271;

		public const int ChallengeYellowLoong = 272;

		public const int NurtureJiao = 273;

		public const int ReproductJiao = 274;

		public const int Wuxian_InWugDanger = 275;

		public const int Wuxian_SeekWuxian = 276;

		public const int Wuxian_TakeBath = 277;

		public const int Wuxian_MakeAWish = 278;

		public const int Wuxian_RefuseRequest = 279;

		public const int Wuxian_BaihuaAdventure = 280;

		public const int Wuxian_SeekBaihua = 281;

		public const int Wuxian_KongsangAdventure = 282;

		public const int Wuxian_SeekKongsang = 283;

		public const int Wuxian_FollowLove = 284;

		public const int Wuxian_FailingWish = 285;

		public const int Jingang_AdventureAppeared = 286;

		public const int Jingang_MonkKilled = 287;

		public const int Jingang_SutraDiscussion = 288;

		public const int Jingang_SutraFeedback = 289;

		public const int Jingang_SutraExplaining = 290;

		public const int Wuxian_Wugged = 291;

		public const int Wuxian_MeetWithRan = 292;

		public const int Ranshan_ToRanshan = 293;

		public const int Ranshan_QinglangDream = 294;

		public const int Ranshan_AfterQinglang = 295;

		public const int Ranshan_LeaveRanshan = 296;

		public const int Ranshan_SanZongBiWu = 297;

		public const int Ranshan_End = 299;

		public const int Ranshan_EnterQinglangge = 298;

		public const int Ranshan_EndInAdvance = 300;

		public const int Baihua_Manic = 301;

		public const int Baihua_AdventureVillageEndemic = 302;

		public const int Baihua_SeekMedCare = 303;

		public const int Baihua_WaitForGurus = 304;

		public const int Baihua_WaitForMelano = 305;

		public const int Baihua_SeekGuru = 306;

		public const int Baihua_WaitForGuruLetter = 307;

		public const int Baihua_SearchInfectedLeuko = 308;

		public const int Baihua_AmbushLeuko = 309;

		public const int Baihua_SearchInfectedMelano = 310;

		public const int Baihua_AmbushMelano = 311;

		public const int Baihua_BringAnimalsBack = 312;

		public const int Baihua_RepairLMRelationship = 313;

		public const int Baihua_LeukoFav = 314;

		public const int Baihua_LeukoClose = 315;

		public const int Baihua_LHelpsM = 316;

		public const int Baihua_MelanoFav = 317;

		public const int Baihua_MelanoClose = 318;

		public const int Baihua_MHelpsL = 324;

		public const int Ranshan_TeachHuaju = 319;

		public const int Ranshan_TeachXuanzhi = 320;

		public const int Ranshan_TeachYingjiao = 321;

		public const int Ranshan_BackToRanshan = 322;

		public const int Ranshan_WaitForBiWu = 323;

		public const int Baihua_PandemicStart = 325;

		public const int Baihua_WaitForAdventure = 326;

		public const int Baihua_AdventureFinale = 327;

		public const int Baihua_Finale = 328;

		public const int Fulong_Diaster = 329;

		public const int Fulong_Sacrifice = 330;

		public const int Fulong_Comet = 331;

		public const int Fulong_ReturnToFulong = 332;

		public const int Fulong_StayFulong = 333;

		public const int Fulong_Mystery = 334;

		public const int Fulong_AskLazuli = 335;

		public const int Fulong_TravelWithLazuli = 336;

		public const int Fulong_BackToFulong = 337;

		public const int Fulong_SeekFeather = 338;

		public const int Fulong_SewFeatherCoat = 339;

		public const int Fulong_FinaleAdventure = 340;

		public const int Fulong_BackHome = 341;

		public const int SideQuest_ChickenMap = 342;

		public const int Fulong_FireFighting = 343;

		public const int Fulong_StayWithLazuli = 344;

		public const int Fulong_TakeLazuliBackToTaiwuVillage = 345;

		public const int Fulong_FireSeeking = 346;

		public const int Zhujian_Poverty = 347;

		public const int Zhujian_Crisis = 348;

		public const int Zhujian_Furnace = 349;

		public const int Zhujian_MistyZhanlu = 350;

		public const int Zhujian_TongshengTalking = 351;

		public const int Zhujian_AccessoryMerchant = 352;

		public const int Zhujian_ConvinceAccessory = 353;

		public const int Zhujian_ReplyAccessoryMerchant = 354;

		public const int Zhujian_BookMerchant = 355;

		public const int Zhujian_ConvinceMerchants = 356;

		public const int Zhujian_ReplyFoodsMerchant = 357;

		public const int Zhujian_MedicineMerchant = 358;

		public const int Zhujian_SeekMaterialMerchant = 359;

		public const int Zhujian_ConstructBranch = 360;

		public const int Zhujian_AdventureBodyCompelete = 361;

		public const int Zhujian_Heir = 362;

		public const int Zhujian_TongshengFav = 363;

		public const int Zhujian_History = 364;

		public const int Zhujian_ToZhanlu = 365;

		public const int Zhujian_MidnightZhujian = 366;

		public const int Zhujian_AdventureFinale = 367;

		public const int Zhujian_TaiyuanHeirtage = 368;

		public const int Zhujian_XiangyangHeritage = 369;

		public const int Zhujian_JianglingHeritage = 370;

		public const int Zhujian_JianglingThief = 371;

		public const int Zhujian_XiangyangThief = 372;

		public const int Zhujian_QinzhouThief = 373;

		public const int Zhujian_End = 374;

		public const int RemakeEmei_Homocide0 = 375;

		public const int RemakeEmei_Homocide1 = 376;

		public const int RemakeEmei_Homocide2 = 377;

		public const int RemakeEmei_Clarify = 378;

		public const int RemakeEmei_AskElder0 = 379;

		public const int RemakeEmei_AskElder1 = 380;

		public const int RemakeEmei_AskElder2 = 381;

		public const int RemakeEmei_MythOfElders = 382;

		public const int RemakeEmei_WaitForGibbon = 383;

		public const int RemakeEmei_History = 384;

		public const int RemakeEmei_MythOfShrine = 385;

		public const int RemakeEmei_Stay = 386;

		public const int RemakeEmei_Strategy = 387;

		public const int RemakeEmei_SeekTruth = 388;

		public const int RemakeEmei_Explain = 389;

		public const int RemakeEmei_MythOfSilhouette = 390;

		public const int RemakeEmei_End = 391;

		public const int RemakeYuanshan_VisitYuanshan = 392;

		public const int RemakeYuanshan_Meditation = 393;

		public const int RemakeYuanshan_Village = 394;

		public const int RemakeYuanshan_TrialVillage = 395;

		public const int RemakeYuanshan_Stockade = 396;

		public const int RemakeYuanshan_SearchNian = 397;

		public const int RemakeYuanshan_TrialStockade = 398;

		public const int RemakeYuanshan_Town = 399;

		public const int RemakeYuanshan_TrialTown = 400;

		public const int RemakeYuanshan_RevisitYuanshan = 401;

		public const int RemakeYuanshan_WaitForEnlightment = 402;

		public const int RemakeYuanshan_Adventure = 403;

		public const int RemakeYuanshan_Finale = 404;

		public const int RemakeYuanshan_EasterEgg = 406;

		public const int PlantTrees = 405;
	}

	public static class DefValue
	{
		public static TaskInfoItem SideQuest_ConstructTaiwuVillage => Instance[22];

		public static TaskInfoItem SideQuest_AssignVillagers => Instance[23];

		public static TaskInfoItem Kongsang_MissionUnaccepted => Instance[105];

		public static TaskInfoItem Kongsang_SearchTarget0 => Instance[106];

		public static TaskInfoItem Kongsang_PoisonTest0 => Instance[107];

		public static TaskInfoItem Kongsang_ReplySect0 => Instance[108];

		public static TaskInfoItem Kongsang_SearchTarget1 => Instance[109];

		public static TaskInfoItem Kongsang_PoisonTest1 => Instance[110];

		public static TaskInfoItem Kongsang_ReplySect1 => Instance[111];

		public static TaskInfoItem Kongsang_SearchTarget2 => Instance[112];

		public static TaskInfoItem Kongsang_PoisonTest2 => Instance[113];

		public static TaskInfoItem Kongsang_ReplySect3 => Instance[114];

		public static TaskInfoItem Kongsang_FindWLiao => Instance[115];

		public static TaskInfoItem Kongsang_ReplySect2 => Instance[116];

		public static TaskInfoItem Kongsang_WaitForAdventure => Instance[117];

		public static TaskInfoItem Kongsang_AdventureAppeared => Instance[118];

		public static TaskInfoItem Xuehou_GraveDigging => Instance[119];

		public static TaskInfoItem Xuehou_RustBell => Instance[120];

		public static TaskInfoItem Xuehou_OldmanMyth => Instance[121];

		public static TaskInfoItem Xuehou_Oldman => Instance[122];

		public static TaskInfoItem Xuehou_CheckBloodBlock => Instance[123];

		public static TaskInfoItem Xuehou_AdventureGrave => Instance[124];

		public static TaskInfoItem Xuehou_BringJixiBack => Instance[125];

		public static TaskInfoItem Xuehou_StayWithJixi => Instance[126];

		public static TaskInfoItem Xuehou_MythInVillage => Instance[127];

		public static TaskInfoItem Xuehou_TruthClue1 => Instance[128];

		public static TaskInfoItem Xuehou_FalsityClue1 => Instance[129];

		public static TaskInfoItem Xuehou_TruthClue2 => Instance[130];

		public static TaskInfoItem Xuehou_FalsityClue2 => Instance[131];

		public static TaskInfoItem Xuehou_TruthClue3 => Instance[132];

		public static TaskInfoItem Xuehou_FalsityClue3 => Instance[133];

		public static TaskInfoItem Xuehou_InterrogateJixi => Instance[134];

		public static TaskInfoItem Xuehou_PassLegacy => Instance[135];

		public static TaskInfoItem Shaolin_MythinShaolin => Instance[136];

		public static TaskInfoItem Shaolin_MuddyStatue => Instance[137];

		public static TaskInfoItem Shaolin_ReturnStatue => Instance[138];

		public static TaskInfoItem Xuannv_QinAndQing => Instance[139];

		public static TaskInfoItem Xuannv_WaitForLetters => Instance[140];

		public static TaskInfoItem Xuannv_SeekLetterSender => Instance[141];

		public static TaskInfoItem Xuannv_SeekXuannv => Instance[142];

		public static TaskInfoItem Xuannv_AdventureSoulInMirror => Instance[143];

		public static TaskInfoItem Xuannv_RefusedToSearch => Instance[144];

		public static TaskInfoItem Xuannv_AcceptedToSearch => Instance[145];

		public static TaskInfoItem Xuannv_AdventureIllusionOfMirror => Instance[146];

		public static TaskInfoItem Xuannv_SeekLove => Instance[147];

		public static TaskInfoItem Xuannv_WaitForReturn => Instance[148];

		public static TaskInfoItem Wudang_Prologue => Instance[149];

		public static TaskInfoItem Wudang_WaitForSlobbyTaoistMonk => Instance[150];

		public static TaskInfoItem Wudang_SloppyTaoistMonkRequest => Instance[151];

		public static TaskInfoItem Wudang_SeekSite => Instance[152];

		public static TaskInfoItem Wudang_ReturnToSloppyTaoistMonk => Instance[153];

		public static TaskInfoItem Wudang_AskWudang => Instance[154];

		public static TaskInfoItem Yuanshan_InPrison => Instance[155];

		public static TaskInfoItem Yuanshan_MythInPrison => Instance[156];

		public static TaskInfoItem Yuanshan_MythInYuanshan => Instance[157];

		public static TaskInfoItem Yuanshan_FindHead => Instance[158];

		public static TaskInfoItem Yuanshan_Diplomacy => Instance[159];

		public static TaskInfoItem Yuanshan_RequestNotAccepted => Instance[160];

		public static TaskInfoItem Yuanshan_ReplyToHead => Instance[161];

		public static TaskInfoItem Yuanshan_ToBeInDanger => Instance[162];

		public static TaskInfoItem Yuanshan_InDanger => Instance[163];

		public static TaskInfoItem Yuanshan_DemonDiplomat => Instance[164];

		public static TaskInfoItem Yuanshan_DemonAnnihilation1and2 => Instance[165];

		public static TaskInfoItem Yuanshan_ReplyToHeadAgain => Instance[166];

		public static TaskInfoItem Yuanshan_DemonEscaped => Instance[167];

		public static TaskInfoItem Yuanshan_DemonAnnihilation3 => Instance[168];

		public static TaskInfoItem Shixiang_Myth => Instance[169];

		public static TaskInfoItem Shixiang_PoemAdventure => Instance[170];

		public static TaskInfoItem Shixiang_WaitforLetters => Instance[171];

		public static TaskInfoItem Shixiang_Arrive => Instance[172];

		public static TaskInfoItem Shixiang_Heretics => Instance[173];

		public static TaskInfoItem Shixiang_ReplyHead => Instance[174];

		public static TaskInfoItem Shixiang_Stay => Instance[175];

		public static TaskInfoItem Shixiang_Traitors => Instance[176];

		public static TaskInfoItem Shixiang_Barbarians => Instance[177];

		public static TaskInfoItem Shixiang_Ending => Instance[178];

		public static TaskInfoItem Jingang_Poverty => Instance[179];

		public static TaskInfoItem Jingang_ToJingang => Instance[180];

		public static TaskInfoItem Jingang_Adventure => Instance[181];

		public static TaskInfoItem Jingang_Haunted => Instance[182];

		public static TaskInfoItem Jingang_AssistReincarnation => Instance[183];

		public static TaskInfoItem Jingang_WaitForReincarnation => Instance[184];

		public static TaskInfoItem Jingang_ToKunlun => Instance[185];

		public static TaskInfoItem Jingang_SearchMonk => Instance[186];

		public static TaskInfoItem Jingang_SecInfoSpreading => Instance[187];

		public static TaskInfoItem Jingang_RefuseAndFailing => Instance[188];

		public static TaskInfoItem Jingang_MonkEnding => Instance[189];

		public static TaskInfoItem Jingang_BackToJingang => Instance[190];

		public static TaskInfoItem Jingang_ReturnSutra => Instance[191];

		public static TaskInfoItem Jingang_Ending => Instance[192];

		public static TaskInfoItem Wuxian_Bath => Instance[193];

		public static TaskInfoItem Wuxian_ToBaihua => Instance[194];

		public static TaskInfoItem Wuxian_AcceptRequest => Instance[195];

		public static TaskInfoItem Wuxian_ToKongsang => Instance[196];

		public static TaskInfoItem Wuxian_BackHome => Instance[197];

		public static TaskInfoItem Wuxian_WishComeTrue => Instance[198];

		public static TaskInfoItem Wuxian_Adventure => Instance[199];

		public static TaskInfoItem Emei_HomocideCases => Instance[200];

		public static TaskInfoItem Emei_Clues => Instance[201];

		public static TaskInfoItem Emei_InvestigateWhiteGibbon => Instance[202];

		public static TaskInfoItem Emei_Orthrodox => Instance[203];

		public static TaskInfoItem Emei_OrthrodoxAdventure => Instance[204];

		public static TaskInfoItem Jieqing_AskHead => Instance[205];

		public static TaskInfoItem Jieqing_Abyss => Instance[206];

		public static TaskInfoItem Jieqing_DukkhaStones => Instance[207];

		public static TaskInfoItem Jieqing_DukkhaOfBirth => Instance[208];

		public static TaskInfoItem Jieqing_DukkhaOfAging => Instance[209];

		public static TaskInfoItem Jieqing_DukkhaOfSickness => Instance[210];

		public static TaskInfoItem Jieqing_DukkhaOfDeath => Instance[211];

		public static TaskInfoItem Jieqing_DukkhaOfSeparation => Instance[212];

		public static TaskInfoItem Jieqing_DukkhaOfDespisement => Instance[213];

		public static TaskInfoItem Jieqing_DukkhaOfCraving => Instance[214];

		public static TaskInfoItem Jieqing_AbyssEnd => Instance[215];

		public static TaskInfoItem Ranshan_Invitation => Instance[216];

		public static TaskInfoItem Ranshan_Prologue => Instance[217];

		public static TaskInfoItem Ranshan_Coaching => Instance[218];

		public static TaskInfoItem Ranshan_Competition => Instance[219];

		public static TaskInfoItem Shaolin_StayAndWait => Instance[220];

		public static TaskInfoItem Shaolin_BodhidharmaInDream => Instance[221];

		public static TaskInfoItem Shaolin_BrokenStatue => Instance[222];

		public static TaskInfoItem Shaolin_Conflict => Instance[223];

		public static TaskInfoItem Shaolin_Endeavor => Instance[224];

		public static TaskInfoItem Shaolin_WaitForBodhidharma => Instance[225];

		public static TaskInfoItem Shaolin_DiggingSutra => Instance[226];

		public static TaskInfoItem Shaolin_StudyForBodhidharmaChallenge => Instance[227];

		public static TaskInfoItem Shaolin_ObtainedSutra => Instance[228];

		public static TaskInfoItem Shaolin_ReadSutra => Instance[229];

		public static TaskInfoItem Shaolin_VisitShaolin => Instance[230];

		public static TaskInfoItem Shaolin_SutraLibrary => Instance[231];

		public static TaskInfoItem Shixiang_Anecdote => Instance[232];

		public static TaskInfoItem Shixiang_ArriveSect => Instance[233];

		public static TaskInfoItem Shixiang_WaitForBattles => Instance[234];

		public static TaskInfoItem Wudang_CultivateHeavenlyTreeMain => Instance[236];

		public static TaskInfoItem Wudang_PlantHeavenlyTree => Instance[237];

		public static TaskInfoItem Wudang_ProtectHeavenlyTree => Instance[238];

		public static TaskInfoItem Wudang_ReadTaoistBook => Instance[239];

		public static TaskInfoItem Wudang_GetHeavenlyTreeSeed => Instance[240];

		public static TaskInfoItem Wudang_TaoistMonkSacrifices => Instance[241];

		public static TaskInfoItem Wudang_WaitForTimePassing => Instance[242];

		public static TaskInfoItem Wudang_VisitWudang => Instance[243];

		public static TaskInfoItem Wudang_WaitForImmortals => Instance[244];

		public static TaskInfoItem Shixiang_AskForJokes => Instance[245];

		public static TaskInfoItem Xuannv_Study => Instance[246];

		public static TaskInfoItem Xuannv_ReturnToMirror => Instance[247];

		public static TaskInfoItem Xuannv_TakeShiToXuannv => Instance[248];

		public static TaskInfoItem Xuannv_WaitForMessage => Instance[249];

		public static TaskInfoItem Xuannv_AskShadow => Instance[250];

		public static TaskInfoItem Xuannv_AskJuner => Instance[251];

		public static TaskInfoItem Xuannv_AskXuannvSect => Instance[252];

		public static TaskInfoItem Xuannv_FailAndRetake => Instance[253];

		public static TaskInfoItem Xuannv_ReadyToStudy => Instance[256];

		public static TaskInfoItem Emei_WaitForAdventure => Instance[254];

		public static TaskInfoItem Emei_SeekWhiteGibbon => Instance[255];

		public static TaskInfoItem PlayerShadowInMirror => Instance[257];

		public static TaskInfoItem Emei_WhiteGibbonsDisappeared => Instance[258];

		public static TaskInfoItem Emei_ShiHoujiuDisappeared => Instance[259];

		public static TaskInfoItem CrossArchive_FollowFuyu => Instance[260];

		public static TaskInfoItem CrossArchive_Items => Instance[261];

		public static TaskInfoItem CrossArchive_LifeSkills => Instance[262];

		public static TaskInfoItem CrossArchive_CombatSkills => Instance[263];

		public static TaskInfoItem LoongDLCToVillage => Instance[264];

		public static TaskInfoItem LoongDLCCaptureLoong => Instance[265];

		public static TaskInfoItem LoongDLCNurtureJiao => Instance[266];

		public static TaskInfoItem LoongDLCNurtureJiuzi => Instance[267];

		public static TaskInfoItem ChallengeWhiteLoong => Instance[268];

		public static TaskInfoItem ChallengeBlackLoong => Instance[269];

		public static TaskInfoItem ChallengeBlueLoong => Instance[270];

		public static TaskInfoItem ChallengeRedLoong => Instance[271];

		public static TaskInfoItem ChallengeYellowLoong => Instance[272];

		public static TaskInfoItem NurtureJiao => Instance[273];

		public static TaskInfoItem ReproductJiao => Instance[274];

		public static TaskInfoItem Wuxian_InWugDanger => Instance[275];

		public static TaskInfoItem Wuxian_SeekWuxian => Instance[276];

		public static TaskInfoItem Wuxian_TakeBath => Instance[277];

		public static TaskInfoItem Wuxian_MakeAWish => Instance[278];

		public static TaskInfoItem Wuxian_RefuseRequest => Instance[279];

		public static TaskInfoItem Wuxian_BaihuaAdventure => Instance[280];

		public static TaskInfoItem Wuxian_SeekBaihua => Instance[281];

		public static TaskInfoItem Wuxian_KongsangAdventure => Instance[282];

		public static TaskInfoItem Wuxian_SeekKongsang => Instance[283];

		public static TaskInfoItem Wuxian_FollowLove => Instance[284];

		public static TaskInfoItem Wuxian_FailingWish => Instance[285];

		public static TaskInfoItem Jingang_AdventureAppeared => Instance[286];

		public static TaskInfoItem Jingang_MonkKilled => Instance[287];

		public static TaskInfoItem Jingang_SutraDiscussion => Instance[288];

		public static TaskInfoItem Jingang_SutraFeedback => Instance[289];

		public static TaskInfoItem Jingang_SutraExplaining => Instance[290];

		public static TaskInfoItem Wuxian_Wugged => Instance[291];

		public static TaskInfoItem Wuxian_MeetWithRan => Instance[292];

		public static TaskInfoItem Ranshan_ToRanshan => Instance[293];

		public static TaskInfoItem Ranshan_QinglangDream => Instance[294];

		public static TaskInfoItem Ranshan_AfterQinglang => Instance[295];

		public static TaskInfoItem Ranshan_LeaveRanshan => Instance[296];

		public static TaskInfoItem Ranshan_SanZongBiWu => Instance[297];

		public static TaskInfoItem Ranshan_End => Instance[299];

		public static TaskInfoItem Ranshan_EnterQinglangge => Instance[298];

		public static TaskInfoItem Ranshan_EndInAdvance => Instance[300];

		public static TaskInfoItem Baihua_Manic => Instance[301];

		public static TaskInfoItem Baihua_AdventureVillageEndemic => Instance[302];

		public static TaskInfoItem Baihua_SeekMedCare => Instance[303];

		public static TaskInfoItem Baihua_WaitForGurus => Instance[304];

		public static TaskInfoItem Baihua_WaitForMelano => Instance[305];

		public static TaskInfoItem Baihua_SeekGuru => Instance[306];

		public static TaskInfoItem Baihua_WaitForGuruLetter => Instance[307];

		public static TaskInfoItem Baihua_SearchInfectedLeuko => Instance[308];

		public static TaskInfoItem Baihua_AmbushLeuko => Instance[309];

		public static TaskInfoItem Baihua_SearchInfectedMelano => Instance[310];

		public static TaskInfoItem Baihua_AmbushMelano => Instance[311];

		public static TaskInfoItem Baihua_BringAnimalsBack => Instance[312];

		public static TaskInfoItem Baihua_RepairLMRelationship => Instance[313];

		public static TaskInfoItem Baihua_LeukoFav => Instance[314];

		public static TaskInfoItem Baihua_LeukoClose => Instance[315];

		public static TaskInfoItem Baihua_LHelpsM => Instance[316];

		public static TaskInfoItem Baihua_MelanoFav => Instance[317];

		public static TaskInfoItem Baihua_MelanoClose => Instance[318];

		public static TaskInfoItem Baihua_MHelpsL => Instance[324];

		public static TaskInfoItem Ranshan_TeachHuaju => Instance[319];

		public static TaskInfoItem Ranshan_TeachXuanzhi => Instance[320];

		public static TaskInfoItem Ranshan_TeachYingjiao => Instance[321];

		public static TaskInfoItem Ranshan_BackToRanshan => Instance[322];

		public static TaskInfoItem Ranshan_WaitForBiWu => Instance[323];

		public static TaskInfoItem Baihua_PandemicStart => Instance[325];

		public static TaskInfoItem Baihua_WaitForAdventure => Instance[326];

		public static TaskInfoItem Baihua_AdventureFinale => Instance[327];

		public static TaskInfoItem Baihua_Finale => Instance[328];

		public static TaskInfoItem Fulong_Diaster => Instance[329];

		public static TaskInfoItem Fulong_Sacrifice => Instance[330];

		public static TaskInfoItem Fulong_Comet => Instance[331];

		public static TaskInfoItem Fulong_ReturnToFulong => Instance[332];

		public static TaskInfoItem Fulong_StayFulong => Instance[333];

		public static TaskInfoItem Fulong_Mystery => Instance[334];

		public static TaskInfoItem Fulong_AskLazuli => Instance[335];

		public static TaskInfoItem Fulong_TravelWithLazuli => Instance[336];

		public static TaskInfoItem Fulong_BackToFulong => Instance[337];

		public static TaskInfoItem Fulong_SeekFeather => Instance[338];

		public static TaskInfoItem Fulong_SewFeatherCoat => Instance[339];

		public static TaskInfoItem Fulong_FinaleAdventure => Instance[340];

		public static TaskInfoItem Fulong_BackHome => Instance[341];

		public static TaskInfoItem SideQuest_ChickenMap => Instance[342];

		public static TaskInfoItem Fulong_FireFighting => Instance[343];

		public static TaskInfoItem Fulong_StayWithLazuli => Instance[344];

		public static TaskInfoItem Fulong_TakeLazuliBackToTaiwuVillage => Instance[345];

		public static TaskInfoItem Fulong_FireSeeking => Instance[346];

		public static TaskInfoItem Zhujian_Poverty => Instance[347];

		public static TaskInfoItem Zhujian_Crisis => Instance[348];

		public static TaskInfoItem Zhujian_Furnace => Instance[349];

		public static TaskInfoItem Zhujian_MistyZhanlu => Instance[350];

		public static TaskInfoItem Zhujian_TongshengTalking => Instance[351];

		public static TaskInfoItem Zhujian_AccessoryMerchant => Instance[352];

		public static TaskInfoItem Zhujian_ConvinceAccessory => Instance[353];

		public static TaskInfoItem Zhujian_ReplyAccessoryMerchant => Instance[354];

		public static TaskInfoItem Zhujian_BookMerchant => Instance[355];

		public static TaskInfoItem Zhujian_ConvinceMerchants => Instance[356];

		public static TaskInfoItem Zhujian_ReplyFoodsMerchant => Instance[357];

		public static TaskInfoItem Zhujian_MedicineMerchant => Instance[358];

		public static TaskInfoItem Zhujian_SeekMaterialMerchant => Instance[359];

		public static TaskInfoItem Zhujian_ConstructBranch => Instance[360];

		public static TaskInfoItem Zhujian_AdventureBodyCompelete => Instance[361];

		public static TaskInfoItem Zhujian_Heir => Instance[362];

		public static TaskInfoItem Zhujian_TongshengFav => Instance[363];

		public static TaskInfoItem Zhujian_History => Instance[364];

		public static TaskInfoItem Zhujian_ToZhanlu => Instance[365];

		public static TaskInfoItem Zhujian_MidnightZhujian => Instance[366];

		public static TaskInfoItem Zhujian_AdventureFinale => Instance[367];

		public static TaskInfoItem Zhujian_TaiyuanHeirtage => Instance[368];

		public static TaskInfoItem Zhujian_XiangyangHeritage => Instance[369];

		public static TaskInfoItem Zhujian_JianglingHeritage => Instance[370];

		public static TaskInfoItem Zhujian_JianglingThief => Instance[371];

		public static TaskInfoItem Zhujian_XiangyangThief => Instance[372];

		public static TaskInfoItem Zhujian_QinzhouThief => Instance[373];

		public static TaskInfoItem Zhujian_End => Instance[374];

		public static TaskInfoItem RemakeEmei_Homocide0 => Instance[375];

		public static TaskInfoItem RemakeEmei_Homocide1 => Instance[376];

		public static TaskInfoItem RemakeEmei_Homocide2 => Instance[377];

		public static TaskInfoItem RemakeEmei_Clarify => Instance[378];

		public static TaskInfoItem RemakeEmei_AskElder0 => Instance[379];

		public static TaskInfoItem RemakeEmei_AskElder1 => Instance[380];

		public static TaskInfoItem RemakeEmei_AskElder2 => Instance[381];

		public static TaskInfoItem RemakeEmei_MythOfElders => Instance[382];

		public static TaskInfoItem RemakeEmei_WaitForGibbon => Instance[383];

		public static TaskInfoItem RemakeEmei_History => Instance[384];

		public static TaskInfoItem RemakeEmei_MythOfShrine => Instance[385];

		public static TaskInfoItem RemakeEmei_Stay => Instance[386];

		public static TaskInfoItem RemakeEmei_Strategy => Instance[387];

		public static TaskInfoItem RemakeEmei_SeekTruth => Instance[388];

		public static TaskInfoItem RemakeEmei_Explain => Instance[389];

		public static TaskInfoItem RemakeEmei_MythOfSilhouette => Instance[390];

		public static TaskInfoItem RemakeEmei_End => Instance[391];

		public static TaskInfoItem RemakeYuanshan_VisitYuanshan => Instance[392];

		public static TaskInfoItem RemakeYuanshan_Meditation => Instance[393];

		public static TaskInfoItem RemakeYuanshan_Village => Instance[394];

		public static TaskInfoItem RemakeYuanshan_TrialVillage => Instance[395];

		public static TaskInfoItem RemakeYuanshan_Stockade => Instance[396];

		public static TaskInfoItem RemakeYuanshan_SearchNian => Instance[397];

		public static TaskInfoItem RemakeYuanshan_TrialStockade => Instance[398];

		public static TaskInfoItem RemakeYuanshan_Town => Instance[399];

		public static TaskInfoItem RemakeYuanshan_TrialTown => Instance[400];

		public static TaskInfoItem RemakeYuanshan_RevisitYuanshan => Instance[401];

		public static TaskInfoItem RemakeYuanshan_WaitForEnlightment => Instance[402];

		public static TaskInfoItem RemakeYuanshan_Adventure => Instance[403];

		public static TaskInfoItem RemakeYuanshan_Finale => Instance[404];

		public static TaskInfoItem RemakeYuanshan_EasterEgg => Instance[406];

		public static TaskInfoItem PlantTrees => Instance[405];
	}

	public static TaskInfo Instance = new TaskInfo();

	private readonly HashSet<string> RequiredFields = new HashSet<string>
	{
		"RunCondition", "FinishCondition", "BlockCondition", "StartTaskChainsWhenFinish", "CharacterTemplateId", "MonthlyEvents", "TemplateId", "TaskTitle", "TaskOverview", "TaskDescription",
		"TaskBubblesContent", "TaskDescriptionMeet", "EventArgBoxKey", "CombatSkillIdsEventArgBoxKey", "SkillIdsEventArgBoxKey", "FrontEndKey", "StringArrayEventArgBoxKey"
	};

	internal override int ToInt(int value)
	{
		return value;
	}

	internal override int ToTemplateId(int value)
	{
		return value;
	}

	private void CreateItems0()
	{
		_dataArray.Add(new TaskInfoItem(0, 0, 1, 2, 3, 4, 180, new List<int>(), new List<int> { 4 }, new List<int>(), isTriggeredTask: false, new List<int>(), 1, 2, 2, new List<short>(), null, null, null, null, null, new AutoTriggerMonthlyEvent[0]));
		_dataArray.Add(new TaskInfoItem(1, 5, 6, 7, 8, 9, 120, new List<int> { 1 }, new List<int>(), new List<int>(), isTriggeredTask: false, new List<int>(), 1, 2, 2, new List<short>(), null, null, null, null, null, new AutoTriggerMonthlyEvent[0]));
		_dataArray.Add(new TaskInfoItem(2, 10, 11, 12, 13, 14, 180, new List<int> { 0 }, new List<int> { 3 }, new List<int>(), isTriggeredTask: false, new List<int>(), 1, 2, 0, new List<short>(), null, null, null, null, null, new AutoTriggerMonthlyEvent[0]));
		_dataArray.Add(new TaskInfoItem(3, 15, 16, 17, 18, 19, 180, new List<int> { 228 }, new List<int>(), new List<int>(), isTriggeredTask: false, new List<int>(), 1, 2, 0, new List<short>(), null, null, null, null, null, new AutoTriggerMonthlyEvent[0]));
		_dataArray.Add(new TaskInfoItem(4, 20, 21, 22, 23, 24, 180, new List<int> { 5 }, new List<int> { 6 }, new List<int>(), isTriggeredTask: false, new List<int>(), 2, 3, 0, new List<short>(), null, null, null, null, null, new AutoTriggerMonthlyEvent[0]));
		_dataArray.Add(new TaskInfoItem(5, 25, 26, 27, 28, 29, 180, new List<int> { 6 }, new List<int> { 223 }, new List<int>(), isTriggeredTask: false, new List<int>(), 2, 5, 0, new List<short>(), null, null, null, null, null, new AutoTriggerMonthlyEvent[0]));
		_dataArray.Add(new TaskInfoItem(6, 30, 31, 32, 33, 34, 180, new List<int> { 229 }, new List<int>(), new List<int>(), isTriggeredTask: false, new List<int>(), 3, 4, 2, new List<short>(), null, null, null, null, null, new AutoTriggerMonthlyEvent[0]));
		_dataArray.Add(new TaskInfoItem(7, 35, 36, 37, 38, 39, 120, new List<int> { 13 }, new List<int> { 225 }, new List<int>(), isTriggeredTask: false, new List<int>(), 4, 5, 2, new List<short>(), null, null, null, null, null, new AutoTriggerMonthlyEvent[0]));
		_dataArray.Add(new TaskInfoItem(8, 40, 41, 42, 43, 44, 120, new List<int> { 27 }, new List<int> { 226 }, new List<int>(), isTriggeredTask: false, new List<int>(), 4, 5, 2, new List<short>(), null, null, null, null, null, new AutoTriggerMonthlyEvent[0]));
		_dataArray.Add(new TaskInfoItem(9, 45, 46, 47, 48, 49, 120, new List<int> { 12, 16 }, new List<int> { 222 }, new List<int>(), isTriggeredTask: false, new List<int>(), 4, 5, 2, new List<short>(), null, null, null, null, null, new AutoTriggerMonthlyEvent[0]));
		_dataArray.Add(new TaskInfoItem(10, 50, 51, 52, 38, 53, 120, new List<int>(), new List<int> { 227 }, new List<int>(), isTriggeredTask: false, new List<int>(), 4, 5, 2, new List<short>(), null, null, null, null, null, new AutoTriggerMonthlyEvent[0]));
		_dataArray.Add(new TaskInfoItem(11, 54, 55, 56, 57, 58, 120, new List<int> { 21, 11 }, new List<int> { 9 }, new List<int>(), isTriggeredTask: false, new List<int>(), 4, 5, 2, new List<short>(), null, null, null, null, null, new AutoTriggerMonthlyEvent[0]));
		_dataArray.Add(new TaskInfoItem(12, 59, 60, 61, 62, 63, 120, new List<int> { 9 }, new List<int> { 224 }, new List<int>(), isTriggeredTask: false, new List<int>(), 4, 5, 2, new List<short>(), null, null, null, null, null, new AutoTriggerMonthlyEvent[0]));
		_dataArray.Add(new TaskInfoItem(13, 64, 65, 66, 67, 68, 180, new List<int> { 8, 10 }, new List<int> { 222 }, new List<int>(), isTriggeredTask: false, new List<int>(), 5, 6, 2, new List<short>(), null, null, null, null, null, new AutoTriggerMonthlyEvent[0]));
		_dataArray.Add(new TaskInfoItem(14, 69, 70, 71, 72, 73, 180, new List<int> { 9 }, new List<int> { 222 }, new List<int>(), isTriggeredTask: false, new List<int>(), 5, 6, 2, new List<short>(), null, null, null, null, null, new AutoTriggerMonthlyEvent[0]));
		_dataArray.Add(new TaskInfoItem(15, 74, 75, 76, 38, 77, 120, new List<int> { 14 }, new List<int> { 222 }, new List<int>(), isTriggeredTask: false, new List<int>(), 4, 5, 2, new List<short>(), null, null, null, null, null, new AutoTriggerMonthlyEvent[0]));
		_dataArray.Add(new TaskInfoItem(16, 78, 79, 80, 38, 81, 120, new List<int> { 15 }, new List<int> { 222 }, new List<int>(), isTriggeredTask: false, new List<int>(), 4, 5, 2, new List<short>(), null, null, null, null, null, new AutoTriggerMonthlyEvent[0]));
		_dataArray.Add(new TaskInfoItem(17, 82, 83, 84, 85, 86, 120, new List<int>(), new List<int> { 8 }, new List<int>(), isTriggeredTask: false, new List<int>(), 5, 6, 2, new List<short>(), null, null, null, null, null, new AutoTriggerMonthlyEvent[0]));
		_dataArray.Add(new TaskInfoItem(18, 87, 70, 71, 88, 89, 180, new List<int> { 20 }, new List<int> { 222 }, new List<int>(), isTriggeredTask: false, new List<int>(), 4, 5, 2, new List<short>(), null, null, null, null, null, new AutoTriggerMonthlyEvent[0]));
		_dataArray.Add(new TaskInfoItem(19, 90, 91, 92, 93, 94, 180, new List<int>(), new List<int>(), new List<int>(), isTriggeredTask: false, new List<int>(), 6, 7, 0, new List<short>(), null, null, null, null, null, new AutoTriggerMonthlyEvent[0]));
		_dataArray.Add(new TaskInfoItem(20, 95, 96, 97, 98, 99, 180, new List<int>(), new List<int> { 253 }, new List<int>(), isTriggeredTask: false, new List<int>(), 7, 8, 0, new List<short>(), null, null, null, null, null, new AutoTriggerMonthlyEvent[0]));
		_dataArray.Add(new TaskInfoItem(21, 100, 101, 102, 103, 104, 180, new List<int> { 26 }, new List<int> { 25 }, new List<int>(), isTriggeredTask: false, new List<int>(), 8, 9, 0, new List<short>(), null, null, null, null, null, new AutoTriggerMonthlyEvent[0]));
		_dataArray.Add(new TaskInfoItem(22, 105, 106, 107, 108, 109, 120, new List<int>(), new List<int>(), new List<int>(), isTriggeredTask: true, new List<int>(), 9, 10, 2, new List<short>(), null, null, null, null, null, new AutoTriggerMonthlyEvent[0]));
		_dataArray.Add(new TaskInfoItem(23, 110, 111, 112, 113, 114, 120, new List<int>(), new List<int>(), new List<int>(), isTriggeredTask: true, new List<int>(), 9, 10, 2, new List<short>(), null, null, null, null, null, new AutoTriggerMonthlyEvent[0]));
		_dataArray.Add(new TaskInfoItem(24, 115, 116, 117, 118, 119, 120, new List<int>(), new List<int> { 30 }, new List<int>(), isTriggeredTask: false, new List<int>(), 9, 10, 0, new List<short>(), null, null, null, null, null, new AutoTriggerMonthlyEvent[0]));
		_dataArray.Add(new TaskInfoItem(25, 120, 121, 122, 123, 124, 180, new List<int> { 30 }, new List<int>(), new List<int>(), isTriggeredTask: false, new List<int>(), 10, 11, 0, new List<short>(), null, null, null, null, null, new AutoTriggerMonthlyEvent[0]));
		_dataArray.Add(new TaskInfoItem(26, 125, 126, 127, 128, 129, 120, new List<int>(), new List<int> { 62 }, new List<int>(), isTriggeredTask: false, new List<int>(), 12, 13, 0, new List<short>(), null, null, null, null, null, new AutoTriggerMonthlyEvent[0]));
		_dataArray.Add(new TaskInfoItem(27, 130, 131, 132, 133, 134, 180, new List<int> { 62, 203 }, new List<int>(), new List<int>(), isTriggeredTask: false, new List<int>(), 12, 13, 0, new List<short>(), null, null, null, null, null, new AutoTriggerMonthlyEvent[0]));
		_dataArray.Add(new TaskInfoItem(28, 135, 126, 136, 137, 138, 180, new List<int> { 62, 202 }, new List<int>(), new List<int>(), isTriggeredTask: false, new List<int>(), 12, 13, 2, new List<short>(), null, null, null, null, null, new AutoTriggerMonthlyEvent[0]));
		_dataArray.Add(new TaskInfoItem(29, 139, 140, 141, 142, 143, 180, new List<int> { 62 }, new List<int>(), new List<int>(), isTriggeredTask: false, new List<int>(), 13, 26, 2, new List<short>(), null, null, null, null, null, new AutoTriggerMonthlyEvent[0]));
		_dataArray.Add(new TaskInfoItem(30, 144, 145, 146, 147, 148, 180, new List<int> { 64 }, new List<int>(), new List<int>(), isTriggeredTask: false, new List<int>(), 14, 16, 0, new List<short>(), null, null, null, null, null, new AutoTriggerMonthlyEvent[0]));
		_dataArray.Add(new TaskInfoItem(31, 149, 150, 151, 152, 153, 180, new List<int>(), new List<int> { 28 }, new List<int>(), isTriggeredTask: false, new List<int>(), 16, 19, 2, new List<short>(), null, null, null, null, null, new AutoTriggerMonthlyEvent[0]));
		_dataArray.Add(new TaskInfoItem(32, 154, 155, 156, 157, 158, 120, new List<int> { 106 }, new List<int>(), new List<int>(), isTriggeredTask: false, new List<int>(), 14, 18, 2, new List<short>(), null, null, null, null, null, new AutoTriggerMonthlyEvent[0]));
		_dataArray.Add(new TaskInfoItem(33, 159, 160, 161, 162, 163, 180, new List<int>(), new List<int> { 96 }, new List<int> { 86 }, isTriggeredTask: false, new List<int>(), 16, 25, 0, new List<short>(), null, null, null, null, null, new AutoTriggerMonthlyEvent[0]));
		_dataArray.Add(new TaskInfoItem(34, 164, 165, 166, 162, 167, 180, new List<int> { 66 }, new List<int> { 87 }, new List<int>(), isTriggeredTask: false, new List<int>(), 16, 25, 0, new List<short>(), null, null, null, null, null, new AutoTriggerMonthlyEvent[0]));
		_dataArray.Add(new TaskInfoItem(35, 168, 169, 170, 162, 171, 180, new List<int> { 67 }, new List<int> { 88 }, new List<int>(), isTriggeredTask: false, new List<int>(), 16, 25, 0, new List<short>(), null, null, null, null, null, new AutoTriggerMonthlyEvent[0]));
		_dataArray.Add(new TaskInfoItem(36, 172, 173, 174, 162, 175, 180, new List<int> { 68 }, new List<int> { 89 }, new List<int>(), isTriggeredTask: false, new List<int>(), 16, 25, 0, new List<short>(), null, null, null, null, null, new AutoTriggerMonthlyEvent[0]));
		_dataArray.Add(new TaskInfoItem(37, 176, 177, 178, 162, 179, 180, new List<int> { 69 }, new List<int> { 90 }, new List<int>(), isTriggeredTask: false, new List<int>(), 16, 25, 0, new List<short>(), null, null, null, null, null, new AutoTriggerMonthlyEvent[0]));
		_dataArray.Add(new TaskInfoItem(38, 180, 181, 182, 162, 183, 180, new List<int> { 70 }, new List<int> { 91 }, new List<int>(), isTriggeredTask: false, new List<int>(), 16, 25, 0, new List<short>(), null, null, null, null, null, new AutoTriggerMonthlyEvent[0]));
		_dataArray.Add(new TaskInfoItem(39, 184, 185, 186, 162, 187, 180, new List<int> { 71 }, new List<int> { 92 }, new List<int>(), isTriggeredTask: false, new List<int>(), 16, 25, 0, new List<short>(), null, null, null, null, null, new AutoTriggerMonthlyEvent[0]));
		_dataArray.Add(new TaskInfoItem(40, 188, 189, 190, 162, 191, 180, new List<int> { 72 }, new List<int> { 93 }, new List<int>(), isTriggeredTask: false, new List<int>(), 16, 25, 0, new List<short>(), null, null, null, null, null, new AutoTriggerMonthlyEvent[0]));
		_dataArray.Add(new TaskInfoItem(41, 192, 193, 194, 162, 195, 180, new List<int> { 73 }, new List<int> { 94 }, new List<int>(), isTriggeredTask: false, new List<int>(), 16, 25, 0, new List<short>(), null, null, null, null, null, new AutoTriggerMonthlyEvent[0]));
		_dataArray.Add(new TaskInfoItem(42, 196, 197, 198, 162, 199, 180, new List<int> { 74 }, new List<int> { 95 }, new List<int>(), isTriggeredTask: false, new List<int>(), 16, 25, 0, new List<short>(), null, null, null, null, null, new AutoTriggerMonthlyEvent[0]));
		_dataArray.Add(new TaskInfoItem(43, 200, 201, 202, 203, 204, 120, new List<int> { 108 }, new List<int>(), new List<int>(), isTriggeredTask: false, new List<int>(), 16, 25, 2, new List<short>(), null, null, null, null, null, new AutoTriggerMonthlyEvent[0]));
		_dataArray.Add(new TaskInfoItem(44, 205, 206, 207, 208, 209, 180, new List<int>(), new List<int>(), new List<int>(), isTriggeredTask: true, new List<int>(), 18, 19, 2, new List<short>(), null, null, null, null, null, new AutoTriggerMonthlyEvent[0]));
		_dataArray.Add(new TaskInfoItem(45, 210, 211, 212, 213, 214, 180, new List<int>(), new List<int>(), new List<int>(), isTriggeredTask: true, new List<int>(), 99, 99, 0, new List<short>(), null, null, null, null, null, new AutoTriggerMonthlyEvent[0]));
		_dataArray.Add(new TaskInfoItem(46, 215, 216, 217, 218, 219, 120, new List<int>(), new List<int>(), new List<int>(), isTriggeredTask: true, new List<int>(), 99, 99, 0, new List<short>(), null, null, null, null, null, new AutoTriggerMonthlyEvent[0]));
		_dataArray.Add(new TaskInfoItem(47, 220, 221, 222, 223, 224, 120, new List<int>(), new List<int>(), new List<int>(), isTriggeredTask: true, new List<int>(), 99, 99, 0, new List<short>(), null, null, null, null, null, new AutoTriggerMonthlyEvent[0]));
		_dataArray.Add(new TaskInfoItem(48, 225, 126, 226, 227, 228, 180, new List<int> { 217 }, new List<int>(), new List<int>(), isTriggeredTask: false, new List<int>(), 19, 23, 0, new List<short>(), null, null, null, null, null, new AutoTriggerMonthlyEvent[0]));
		_dataArray.Add(new TaskInfoItem(49, 234, 235, 236, 237, 238, 180, new List<int> { 215 }, new List<int>(), new List<int>(), isTriggeredTask: false, new List<int>(), 19, 99, 2, new List<short>(), null, null, null, null, null, new AutoTriggerMonthlyEvent[0]));
		_dataArray.Add(new TaskInfoItem(50, 239, 126, 240, 241, 242, 180, new List<int> { 218 }, new List<int> { 204 }, new List<int>(), isTriggeredTask: false, new List<int>(), 20, 23, 0, new List<short>(), null, null, null, null, null, new AutoTriggerMonthlyEvent[0]));
		_dataArray.Add(new TaskInfoItem(51, 243, 244, 245, 246, 247, 120, new List<int> { 115 }, new List<int>(), new List<int>(), isTriggeredTask: false, new List<int>(), 8, 28, 2, new List<short>(), null, null, null, null, null, new AutoTriggerMonthlyEvent[0]));
		_dataArray.Add(new TaskInfoItem(52, 248, 126, 249, 250, 251, 180, new List<int> { 219 }, new List<int>(), new List<int>(), isTriggeredTask: false, new List<int>(), 21, 23, 0, new List<short>(), null, null, null, null, null, new AutoTriggerMonthlyEvent[0]));
		_dataArray.Add(new TaskInfoItem(53, 252, 253, 254, 255, 256, 180, new List<int> { 119, 140 }, new List<int>(), new List<int>(), isTriggeredTask: false, new List<int>(), 21, 23, 2, new List<short>(), null, null, null, null, null, new AutoTriggerMonthlyEvent[0]));
		_dataArray.Add(new TaskInfoItem(54, 257, 258, 259, 260, 261, 180, new List<int> { 130 }, new List<int> { 170 }, new List<int>(), isTriggeredTask: false, new List<int>(), 21, 28, 2, new List<short>(), null, null, null, null, null, new AutoTriggerMonthlyEvent[0]));
		_dataArray.Add(new TaskInfoItem(55, 262, 263, 264, 260, 265, 120, new List<int> { 152 }, new List<int> { 161 }, new List<int>(), isTriggeredTask: false, new List<int>(), 21, 28, 2, new List<short>(), null, null, null, null, null, new AutoTriggerMonthlyEvent[0]));
		_dataArray.Add(new TaskInfoItem(56, 266, 267, 268, 260, 269, 120, new List<int> { 153 }, new List<int> { 162 }, new List<int>(), isTriggeredTask: false, new List<int>(), 21, 28, 2, new List<short>(), null, null, null, null, null, new AutoTriggerMonthlyEvent[0]));
		_dataArray.Add(new TaskInfoItem(57, 270, 271, 272, 260, 273, 120, new List<int> { 154 }, new List<int> { 163 }, new List<int>(), isTriggeredTask: false, new List<int>(), 21, 28, 2, new List<short>(), null, null, null, null, null, new AutoTriggerMonthlyEvent[0]));
		_dataArray.Add(new TaskInfoItem(58, 274, 275, 276, 260, 277, 120, new List<int> { 155 }, new List<int> { 164 }, new List<int>(), isTriggeredTask: false, new List<int>(), 21, 28, 2, new List<short>(), null, null, null, null, null, new AutoTriggerMonthlyEvent[0]));
		_dataArray.Add(new TaskInfoItem(59, 278, 279, 280, 260, 281, 120, new List<int> { 156 }, new List<int> { 165 }, new List<int>(), isTriggeredTask: false, new List<int>(), 21, 28, 2, new List<short>(), null, null, null, null, null, new AutoTriggerMonthlyEvent[0]));
	}

	private void CreateItems1()
	{
		_dataArray.Add(new TaskInfoItem(60, 282, 283, 284, 260, 285, 120, new List<int> { 157 }, new List<int> { 166 }, new List<int>(), isTriggeredTask: false, new List<int>(), 21, 28, 2, new List<short>(), null, null, null, null, null, new AutoTriggerMonthlyEvent[0]));
		_dataArray.Add(new TaskInfoItem(61, 286, 287, 288, 260, 289, 120, new List<int> { 158 }, new List<int> { 167 }, new List<int>(), isTriggeredTask: false, new List<int>(), 21, 28, 2, new List<short>(), null, null, null, null, null, new AutoTriggerMonthlyEvent[0]));
		_dataArray.Add(new TaskInfoItem(62, 290, 291, 292, 260, 293, 120, new List<int> { 159 }, new List<int> { 168 }, new List<int>(), isTriggeredTask: false, new List<int>(), 21, 28, 2, new List<short>(), null, null, null, null, null, new AutoTriggerMonthlyEvent[0]));
		_dataArray.Add(new TaskInfoItem(63, 294, 295, 296, 260, 297, 120, new List<int> { 160 }, new List<int> { 169 }, new List<int>(), isTriggeredTask: false, new List<int>(), 21, 28, 2, new List<short>(), null, null, null, null, null, new AutoTriggerMonthlyEvent[0]));
		_dataArray.Add(new TaskInfoItem(64, 298, 126, 299, 300, 301, 180, new List<int>(), new List<int> { 221 }, new List<int>(), isTriggeredTask: false, new List<int>(), 22, 23, 0, new List<short>(), null, null, null, null, null, new AutoTriggerMonthlyEvent[0]));
		_dataArray.Add(new TaskInfoItem(65, 302, 303, 304, 305, 306, 180, new List<int> { 171, 173 }, new List<int>(), new List<int>(), isTriggeredTask: false, new List<int>(), 8, 28, 2, new List<short>(), null, null, null, null, null, new AutoTriggerMonthlyEvent[0]));
		_dataArray.Add(new TaskInfoItem(66, 307, 308, 309, 310, 311, 120, new List<int> { 117, 151 }, new List<int> { 214 }, new List<int>(), isTriggeredTask: false, new List<int>(), 22, 28, 2, new List<short>(), null, null, null, null, null, new AutoTriggerMonthlyEvent[0]));
		_dataArray.Add(new TaskInfoItem(67, 312, 313, 314, 315, 316, 180, new List<int> { 192 }, new List<int> { 65 }, new List<int>(), isTriggeredTask: false, new List<int>(), 22, 23, 2, new List<short>(), null, null, null, null, null, new AutoTriggerMonthlyEvent[0]));
		_dataArray.Add(new TaskInfoItem(68, 317, 318, 319, 320, 321, 180, new List<int>(), new List<int> { 247 }, new List<int> { 76 }, isTriggeredTask: false, new List<int>(), 22, 23, 0, new List<short>(), null, null, null, null, null, new AutoTriggerMonthlyEvent[0]));
		_dataArray.Add(new TaskInfoItem(69, 322, 323, 324, 325, 326, 180, new List<int> { 247 }, new List<int> { 75 }, new List<int>(), isTriggeredTask: false, new List<int>(), 22, 23, 0, new List<short>(), null, null, null, null, null, new AutoTriggerMonthlyEvent[0]));
		_dataArray.Add(new TaskInfoItem(70, 327, 328, 329, 330, 331, 180, new List<int> { 75 }, new List<int> { 176 }, new List<int>(), isTriggeredTask: false, new List<int>(), 23, 24, 0, new List<short>(), null, null, null, null, null, new AutoTriggerMonthlyEvent[0]));
		_dataArray.Add(new TaskInfoItem(71, 332, 333, 334, 335, 336, 120, new List<int> { 176, 250, 251 }, new List<int>(), new List<int>(), isTriggeredTask: false, new List<int>(), 23, 24, 0, new List<short>(), null, null, null, null, null, new AutoTriggerMonthlyEvent[0]));
		_dataArray.Add(new TaskInfoItem(72, 337, 338, 339, 340, 341, 180, new List<int> { 176, 177 }, new List<int> { 176, 75 }, new List<int>(), isTriggeredTask: false, new List<int>(), 23, 24, 0, new List<short>(), null, null, null, null, null, new AutoTriggerMonthlyEvent[0]));
		_dataArray.Add(new TaskInfoItem(73, 342, 343, 344, 345, 346, 120, new List<int>(), new List<int> { 179 }, new List<int>(), isTriggeredTask: false, new List<int>(), 99, 99, 0, new List<short>(), null, null, null, null, null, new AutoTriggerMonthlyEvent[0]));
		_dataArray.Add(new TaskInfoItem(74, 347, 348, 349, 350, 351, 180, new List<int>(), new List<int> { 178 }, new List<int>(), isTriggeredTask: false, new List<int>(), 25, 26, 0, new List<short>(), null, null, null, null, null, new AutoTriggerMonthlyEvent[0]));
		_dataArray.Add(new TaskInfoItem(75, 352, 353, 354, 355, 356, 180, new List<int> { 178 }, new List<int> { 180 }, new List<int>(), isTriggeredTask: false, new List<int>(), 25, 26, 0, new List<short>(), null, null, null, null, null, new AutoTriggerMonthlyEvent[0]));
		_dataArray.Add(new TaskInfoItem(76, 357, 126, 358, 359, 360, 180, new List<int> { 180 }, new List<int>(), new List<int>(), isTriggeredTask: false, new List<int>(), 25, 26, 0, new List<short>(), null, null, null, null, null, new AutoTriggerMonthlyEvent[0]));
		_dataArray.Add(new TaskInfoItem(77, 361, 362, 363, 364, 365, 180, new List<int>(), new List<int> { 182 }, new List<int>(), isTriggeredTask: false, new List<int>(), 26, 27, 0, new List<short>(), null, null, null, null, null, new AutoTriggerMonthlyEvent[0]));
		_dataArray.Add(new TaskInfoItem(78, 366, 367, 368, 369, 370, 180, new List<int> { 182 }, new List<int>(), new List<int>(), isTriggeredTask: false, new List<int>(), 28, 29, 0, new List<short>(), null, null, null, null, null, new AutoTriggerMonthlyEvent[0]));
		_dataArray.Add(new TaskInfoItem(79, 164, 371, 372, 373, 374, 180, new List<int> { 183 }, new List<int>(), new List<int>(), isTriggeredTask: false, new List<int>(), 28, 29, 0, new List<short>(), null, null, null, null, null, new AutoTriggerMonthlyEvent[0]));
		_dataArray.Add(new TaskInfoItem(80, 168, 375, 376, 377, 378, 180, new List<int> { 184 }, new List<int>(), new List<int>(), isTriggeredTask: false, new List<int>(), 28, 29, 0, new List<short>(), null, null, null, null, null, new AutoTriggerMonthlyEvent[0]));
		_dataArray.Add(new TaskInfoItem(81, 172, 379, 380, 381, 382, 180, new List<int> { 185 }, new List<int>(), new List<int>(), isTriggeredTask: false, new List<int>(), 28, 29, 0, new List<short>(), null, null, null, null, null, new AutoTriggerMonthlyEvent[0]));
		_dataArray.Add(new TaskInfoItem(82, 176, 383, 384, 385, 386, 180, new List<int> { 186 }, new List<int>(), new List<int>(), isTriggeredTask: false, new List<int>(), 28, 29, 0, new List<short>(), null, null, null, null, null, new AutoTriggerMonthlyEvent[0]));
		_dataArray.Add(new TaskInfoItem(83, 180, 387, 388, 389, 390, 180, new List<int> { 187 }, new List<int>(), new List<int>(), isTriggeredTask: false, new List<int>(), 28, 29, 0, new List<short>(), null, null, null, null, null, new AutoTriggerMonthlyEvent[0]));
		_dataArray.Add(new TaskInfoItem(84, 184, 391, 392, 393, 394, 180, new List<int> { 188 }, new List<int>(), new List<int>(), isTriggeredTask: false, new List<int>(), 28, 29, 0, new List<short>(), null, null, null, null, null, new AutoTriggerMonthlyEvent[0]));
		_dataArray.Add(new TaskInfoItem(85, 188, 395, 396, 397, 398, 180, new List<int> { 189 }, new List<int>(), new List<int>(), isTriggeredTask: false, new List<int>(), 28, 29, 0, new List<short>(), null, null, null, null, null, new AutoTriggerMonthlyEvent[0]));
		_dataArray.Add(new TaskInfoItem(86, 192, 399, 400, 401, 402, 180, new List<int> { 190 }, new List<int>(), new List<int>(), isTriggeredTask: false, new List<int>(), 28, 29, 0, new List<short>(), null, null, null, null, null, new AutoTriggerMonthlyEvent[0]));
		_dataArray.Add(new TaskInfoItem(87, 196, 403, 404, 405, 406, 180, new List<int> { 191 }, new List<int>(), new List<int>(), isTriggeredTask: false, new List<int>(), 28, 29, 0, new List<short>(), null, null, null, null, null, new AutoTriggerMonthlyEvent[0]));
		_dataArray.Add(new TaskInfoItem(88, 229, 230, 231, 232, 233, 180, new List<int> { 110 }, new List<int> { 215 }, new List<int>(), isTriggeredTask: false, new List<int>(), 19, 28, 2, new List<short>(), null, null, null, null, null, new AutoTriggerMonthlyEvent[0]));
		_dataArray.Add(new TaskInfoItem(89, 407, 408, 409, 410, 411, 180, new List<int>(), new List<int>(), new List<int>(), isTriggeredTask: false, new List<int>(), 9, 28, 2, new List<short>(), null, null, null, null, null, new AutoTriggerMonthlyEvent[0]));
		_dataArray.Add(new TaskInfoItem(90, 412, 413, 414, 410, 415, 180, new List<int>(), new List<int> { 231 }, new List<int>(), isTriggeredTask: false, new List<int>(), 9, 28, 2, new List<short>(), null, null, null, null, null, new AutoTriggerMonthlyEvent[0]));
		_dataArray.Add(new TaskInfoItem(91, 416, 417, 418, 410, 419, 180, new List<int>(), new List<int> { 232 }, new List<int>(), isTriggeredTask: false, new List<int>(), 9, 28, 2, new List<short>(), null, null, null, null, null, new AutoTriggerMonthlyEvent[0]));
		_dataArray.Add(new TaskInfoItem(92, 420, 421, 422, 410, 423, 180, new List<int>(), new List<int> { 233 }, new List<int>(), isTriggeredTask: false, new List<int>(), 9, 28, 2, new List<short>(), null, null, null, null, null, new AutoTriggerMonthlyEvent[0]));
		_dataArray.Add(new TaskInfoItem(93, 424, 425, 426, 410, 427, 180, new List<int>(), new List<int> { 234 }, new List<int>(), isTriggeredTask: false, new List<int>(), 9, 28, 2, new List<short>(), null, null, null, null, null, new AutoTriggerMonthlyEvent[0]));
		_dataArray.Add(new TaskInfoItem(94, 428, 429, 430, 410, 431, 180, new List<int>(), new List<int> { 235 }, new List<int>(), isTriggeredTask: false, new List<int>(), 9, 28, 2, new List<short>(), null, null, null, null, null, new AutoTriggerMonthlyEvent[0]));
		_dataArray.Add(new TaskInfoItem(95, 432, 433, 434, 410, 435, 180, new List<int>(), new List<int> { 236 }, new List<int>(), isTriggeredTask: false, new List<int>(), 9, 28, 2, new List<short>(), null, null, null, null, null, new AutoTriggerMonthlyEvent[0]));
		_dataArray.Add(new TaskInfoItem(96, 436, 437, 438, 410, 439, 180, new List<int>(), new List<int> { 237 }, new List<int>(), isTriggeredTask: false, new List<int>(), 9, 28, 2, new List<short>(), null, null, null, null, null, new AutoTriggerMonthlyEvent[0]));
		_dataArray.Add(new TaskInfoItem(97, 440, 441, 442, 410, 443, 180, new List<int>(), new List<int> { 238 }, new List<int>(), isTriggeredTask: false, new List<int>(), 9, 28, 2, new List<short>(), null, null, null, null, null, new AutoTriggerMonthlyEvent[0]));
		_dataArray.Add(new TaskInfoItem(98, 444, 445, 446, 410, 447, 180, new List<int>(), new List<int> { 239 }, new List<int>(), isTriggeredTask: false, new List<int>(), 9, 28, 2, new List<short>(), null, null, null, null, null, new AutoTriggerMonthlyEvent[0]));
		_dataArray.Add(new TaskInfoItem(99, 448, 449, 450, 410, 451, 180, new List<int>(), new List<int> { 240 }, new List<int>(), isTriggeredTask: false, new List<int>(), 9, 28, 2, new List<short>(), null, null, null, null, null, new AutoTriggerMonthlyEvent[0]));
		_dataArray.Add(new TaskInfoItem(100, 452, 453, 454, 410, 455, 180, new List<int>(), new List<int> { 241 }, new List<int>(), isTriggeredTask: false, new List<int>(), 9, 28, 2, new List<short>(), null, null, null, null, null, new AutoTriggerMonthlyEvent[0]));
		_dataArray.Add(new TaskInfoItem(101, 456, 457, 458, 410, 459, 180, new List<int>(), new List<int> { 242 }, new List<int>(), isTriggeredTask: false, new List<int>(), 9, 28, 2, new List<short>(), null, null, null, null, null, new AutoTriggerMonthlyEvent[0]));
		_dataArray.Add(new TaskInfoItem(102, 460, 461, 462, 410, 463, 180, new List<int>(), new List<int> { 243 }, new List<int>(), isTriggeredTask: false, new List<int>(), 9, 28, 2, new List<short>(), null, null, null, null, null, new AutoTriggerMonthlyEvent[0]));
		_dataArray.Add(new TaskInfoItem(103, 464, 465, 466, 410, 467, 180, new List<int>(), new List<int> { 244 }, new List<int>(), isTriggeredTask: false, new List<int>(), 9, 28, 2, new List<short>(), null, null, null, null, null, new AutoTriggerMonthlyEvent[0]));
		_dataArray.Add(new TaskInfoItem(104, 468, 469, 470, 410, 471, 180, new List<int>(), new List<int> { 245 }, new List<int>(), isTriggeredTask: false, new List<int>(), 9, 28, 2, new List<short>(), null, null, null, null, null, new AutoTriggerMonthlyEvent[0]));
		_dataArray.Add(new TaskInfoItem(105, 472, 473, 474, 475, 476, 120, new List<int>(), new List<int>(), new List<int>(), isTriggeredTask: true, new List<int>(), 0, 99, 1, new List<short>(), null, null, null, null, null, new AutoTriggerMonthlyEvent[0]));
		_dataArray.Add(new TaskInfoItem(106, 477, 478, 479, 480, 481, 120, new List<int>(), new List<int>(), new List<int>(), isTriggeredTask: true, new List<int>(), 0, 99, 1, new List<short>(), null, null, null, null, null, new AutoTriggerMonthlyEvent[0]));
		_dataArray.Add(new TaskInfoItem(107, 482, 483, 484, 485, 486, 120, new List<int>(), new List<int>(), new List<int>(), isTriggeredTask: true, new List<int>(), 0, 99, 1, new List<short> { 668 }, null, null, null, null, null, new AutoTriggerMonthlyEvent[0]));
		_dataArray.Add(new TaskInfoItem(108, 487, 488, 489, 490, 491, 120, new List<int>(), new List<int>(), new List<int>(), isTriggeredTask: true, new List<int>(), 0, 99, 1, new List<short>(), null, null, null, null, null, new AutoTriggerMonthlyEvent[0]));
		_dataArray.Add(new TaskInfoItem(109, 477, 492, 493, 494, 495, 120, new List<int>(), new List<int>(), new List<int>(), isTriggeredTask: true, new List<int>(), 0, 99, 1, new List<short>(), null, null, null, null, null, new AutoTriggerMonthlyEvent[0]));
		_dataArray.Add(new TaskInfoItem(110, 482, 483, 484, 485, 496, 120, new List<int>(), new List<int>(), new List<int>(), isTriggeredTask: true, new List<int>(), 0, 99, 1, new List<short> { 668 }, null, null, null, null, null, new AutoTriggerMonthlyEvent[0]));
		_dataArray.Add(new TaskInfoItem(111, 487, 488, 497, 498, 499, 120, new List<int>(), new List<int>(), new List<int>(), isTriggeredTask: true, new List<int>(), 0, 99, 1, new List<short>(), null, null, null, null, null, new AutoTriggerMonthlyEvent[0]));
		_dataArray.Add(new TaskInfoItem(112, 477, 492, 493, 494, 500, 120, new List<int>(), new List<int>(), new List<int>(), isTriggeredTask: true, new List<int>(), 0, 99, 1, new List<short>(), null, null, null, null, null, new AutoTriggerMonthlyEvent[0]));
		_dataArray.Add(new TaskInfoItem(113, 482, 501, 484, 485, 502, 120, new List<int>(), new List<int>(), new List<int>(), isTriggeredTask: true, new List<int>(), 0, 99, 1, new List<short> { 668 }, null, null, null, null, null, new AutoTriggerMonthlyEvent[0]));
		_dataArray.Add(new TaskInfoItem(114, 487, 488, 503, 504, 505, 120, new List<int>(), new List<int>(), new List<int>(), isTriggeredTask: true, new List<int>(), 0, 99, 1, new List<short>(), null, null, null, null, null, new AutoTriggerMonthlyEvent[0]));
		_dataArray.Add(new TaskInfoItem(115, 506, 507, 508, 509, 510, 120, new List<int>(), new List<int>(), new List<int>(), isTriggeredTask: true, new List<int>(), 0, 99, 1, new List<short>(), null, null, null, null, null, new AutoTriggerMonthlyEvent[0]));
		_dataArray.Add(new TaskInfoItem(116, 511, 512, 513, 514, 515, 120, new List<int>(), new List<int>(), new List<int>(), isTriggeredTask: true, new List<int>(), 0, 99, 1, new List<short>(), null, null, null, null, null, new AutoTriggerMonthlyEvent[0]));
		_dataArray.Add(new TaskInfoItem(117, 516, 517, 518, 519, 520, 120, new List<int>(), new List<int>(), new List<int>(), isTriggeredTask: true, new List<int>(), 0, 99, 1, new List<short>(), null, null, null, null, null, new AutoTriggerMonthlyEvent[0]));
		_dataArray.Add(new TaskInfoItem(118, 521, 522, 523, 524, 525, 120, new List<int>(), new List<int>(), new List<int>(), isTriggeredTask: true, new List<int>(), 0, 99, 1, new List<short>(), null, null, null, null, null, new AutoTriggerMonthlyEvent[0]));
		_dataArray.Add(new TaskInfoItem(119, 526, 527, 528, 529, 530, 120, new List<int>(), new List<int>(), new List<int>(), isTriggeredTask: true, new List<int>(), 0, 99, 1, new List<short>(), null, null, null, null, null, new AutoTriggerMonthlyEvent[0]));
	}

	private void CreateItems2()
	{
		_dataArray.Add(new TaskInfoItem(120, 531, 532, 533, 534, 535, 120, new List<int>(), new List<int>(), new List<int>(), isTriggeredTask: true, new List<int>(), 0, 99, 1, new List<short>(), null, null, null, null, null, new AutoTriggerMonthlyEvent[0]));
		_dataArray.Add(new TaskInfoItem(121, 536, 537, 538, 539, 540, 120, new List<int>(), new List<int>(), new List<int>(), isTriggeredTask: true, new List<int>(), 0, 99, 1, new List<short>(), null, null, null, null, null, new AutoTriggerMonthlyEvent[0]));
		_dataArray.Add(new TaskInfoItem(122, 541, 542, 543, 544, 545, 120, new List<int>(), new List<int>(), new List<int>(), isTriggeredTask: true, new List<int>(), 0, 99, 1, new List<short>(), null, null, null, null, null, new AutoTriggerMonthlyEvent[0]));
		_dataArray.Add(new TaskInfoItem(123, 546, 547, 548, 549, 550, 120, new List<int>(), new List<int>(), new List<int>(), isTriggeredTask: true, new List<int>(), 0, 99, 1, new List<short>(), null, null, null, null, null, new AutoTriggerMonthlyEvent[0]));
		_dataArray.Add(new TaskInfoItem(124, 551, 552, 553, 554, 555, 120, new List<int>(), new List<int>(), new List<int>(), isTriggeredTask: true, new List<int>(), 0, 99, 1, new List<short>(), null, null, null, null, null, new AutoTriggerMonthlyEvent[0]));
		_dataArray.Add(new TaskInfoItem(125, 556, 557, 558, 559, 560, 120, new List<int>(), new List<int>(), new List<int>(), isTriggeredTask: true, new List<int>(), 0, 99, 1, new List<short>(), null, null, null, null, null, new AutoTriggerMonthlyEvent[0]));
		_dataArray.Add(new TaskInfoItem(126, 561, 562, 563, 564, 565, 120, new List<int>(), new List<int>(), new List<int>(), isTriggeredTask: true, new List<int>(), 0, 99, 1, new List<short>(), null, null, null, null, null, new AutoTriggerMonthlyEvent[0]));
		_dataArray.Add(new TaskInfoItem(127, 566, 567, 568, 569, 570, 120, new List<int>(), new List<int>(), new List<int>(), isTriggeredTask: true, new List<int>(), 0, 99, 1, new List<short>(), null, null, null, null, null, new AutoTriggerMonthlyEvent[0]));
		_dataArray.Add(new TaskInfoItem(128, 571, 572, 573, 574, 575, 120, new List<int>(), new List<int>(), new List<int>(), isTriggeredTask: true, new List<int>(), 0, 99, 1, new List<short>(), null, null, null, null, null, new AutoTriggerMonthlyEvent[0]));
		_dataArray.Add(new TaskInfoItem(129, 576, 577, 578, 579, 580, 120, new List<int>(), new List<int>(), new List<int>(), isTriggeredTask: true, new List<int>(), 0, 99, 1, new List<short>(), null, null, null, null, null, new AutoTriggerMonthlyEvent[0]));
		_dataArray.Add(new TaskInfoItem(130, 581, 582, 583, 584, 585, 120, new List<int>(), new List<int>(), new List<int>(), isTriggeredTask: true, new List<int>(), 0, 99, 1, new List<short>(), null, null, null, null, null, new AutoTriggerMonthlyEvent[0]));
		_dataArray.Add(new TaskInfoItem(131, 586, 587, 588, 589, 590, 120, new List<int>(), new List<int>(), new List<int>(), isTriggeredTask: true, new List<int>(), 0, 99, 1, new List<short>(), null, null, null, null, null, new AutoTriggerMonthlyEvent[0]));
		_dataArray.Add(new TaskInfoItem(132, 591, 592, 593, 594, 595, 120, new List<int>(), new List<int>(), new List<int>(), isTriggeredTask: true, new List<int>(), 0, 99, 1, new List<short>(), null, null, null, null, null, new AutoTriggerMonthlyEvent[0]));
		_dataArray.Add(new TaskInfoItem(133, 596, 597, 598, 599, 600, 120, new List<int>(), new List<int>(), new List<int>(), isTriggeredTask: true, new List<int>(), 0, 99, 1, new List<short>(), null, null, null, null, null, new AutoTriggerMonthlyEvent[0]));
		_dataArray.Add(new TaskInfoItem(134, 601, 602, 603, 604, 605, 120, new List<int>(), new List<int>(), new List<int>(), isTriggeredTask: true, new List<int>(), 0, 99, 1, new List<short>(), null, null, null, null, null, new AutoTriggerMonthlyEvent[0]));
		_dataArray.Add(new TaskInfoItem(135, 606, 607, 608, 609, 610, 120, new List<int>(), new List<int>(), new List<int>(), isTriggeredTask: true, new List<int>(), 0, 99, 1, new List<short> { 537, 538, 539 }, null, null, null, null, null, new AutoTriggerMonthlyEvent[0]));
		_dataArray.Add(new TaskInfoItem(136, 611, 612, 613, 614, 615, 120, new List<int>(), new List<int>(), new List<int>(), isTriggeredTask: true, new List<int>(), 0, 99, 1, new List<short>(), null, null, null, null, null, new AutoTriggerMonthlyEvent[0]));
		_dataArray.Add(new TaskInfoItem(137, 616, 617, 618, 619, 620, 120, new List<int>(), new List<int>(), new List<int>(), isTriggeredTask: true, new List<int>(), 0, 99, 1, new List<short>(), null, null, null, null, null, new AutoTriggerMonthlyEvent[0]));
		_dataArray.Add(new TaskInfoItem(138, 621, 622, 623, 624, 625, 120, new List<int>(), new List<int>(), new List<int>(), isTriggeredTask: true, new List<int>(), 0, 99, 1, new List<short>(), null, null, null, null, null, new AutoTriggerMonthlyEvent[0]));
		_dataArray.Add(new TaskInfoItem(139, 626, 627, 628, 629, 630, 120, new List<int>(), new List<int>(), new List<int>(), isTriggeredTask: true, new List<int>(), 0, 99, 1, new List<short>(), null, null, null, null, null, new AutoTriggerMonthlyEvent[0]));
		_dataArray.Add(new TaskInfoItem(140, 631, 632, 633, 634, 635, 120, new List<int>(), new List<int>(), new List<int>(), isTriggeredTask: true, new List<int>(), 0, 99, 1, new List<short>(), null, null, null, null, null, new AutoTriggerMonthlyEvent[0]));
		_dataArray.Add(new TaskInfoItem(141, 636, 637, 638, 639, 640, 120, new List<int>(), new List<int>(), new List<int>(), isTriggeredTask: true, new List<int>(), 0, 99, 1, new List<short>(), null, null, null, null, null, new AutoTriggerMonthlyEvent[0]));
		_dataArray.Add(new TaskInfoItem(142, 641, 642, 643, 644, 645, 120, new List<int>(), new List<int>(), new List<int>(), isTriggeredTask: true, new List<int>(), 0, 99, 1, new List<short>(), null, null, null, null, null, new AutoTriggerMonthlyEvent[0]));
		_dataArray.Add(new TaskInfoItem(143, 646, 647, 648, 649, 650, 120, new List<int>(), new List<int>(), new List<int>(), isTriggeredTask: true, new List<int>(), 0, 99, 1, new List<short>(), null, null, null, null, null, new AutoTriggerMonthlyEvent[0]));
		_dataArray.Add(new TaskInfoItem(144, 651, 652, 653, 654, 655, 120, new List<int>(), new List<int>(), new List<int>(), isTriggeredTask: true, new List<int>(), 0, 99, 1, new List<short>(), null, null, null, null, null, new AutoTriggerMonthlyEvent[0]));
		_dataArray.Add(new TaskInfoItem(145, 656, 652, 657, 658, 659, 120, new List<int>(), new List<int>(), new List<int>(), isTriggeredTask: true, new List<int>(), 0, 99, 1, new List<short>(), null, null, null, null, null, new AutoTriggerMonthlyEvent[0]));
		_dataArray.Add(new TaskInfoItem(146, 660, 661, 662, 663, 664, 120, new List<int>(), new List<int>(), new List<int>(), isTriggeredTask: true, new List<int>(), 0, 99, 1, new List<short>(), null, null, null, null, null, new AutoTriggerMonthlyEvent[0]));
		_dataArray.Add(new TaskInfoItem(147, 665, 666, 667, 668, 669, 120, new List<int>(), new List<int>(), new List<int>(), isTriggeredTask: true, new List<int>(), 0, 99, 1, new List<short>(), "ConchShip_PresetKey_Xuannv_LoverReincarnateLocation", null, null, null, null, new AutoTriggerMonthlyEvent[0]));
		_dataArray.Add(new TaskInfoItem(148, 670, 671, 672, 673, 674, 120, new List<int>(), new List<int>(), new List<int>(), isTriggeredTask: true, new List<int>(), 0, 99, 1, new List<short>(), null, null, null, null, new string[1] { "ConchShip_PresetKey_Xuannv_WaitLoverNameKey" }, new AutoTriggerMonthlyEvent[0]));
		_dataArray.Add(new TaskInfoItem(149, 675, 676, 677, 678, 679, 120, new List<int>(), new List<int>(), new List<int>(), isTriggeredTask: true, new List<int>(), 0, 99, 1, new List<short>(), null, null, null, null, null, new AutoTriggerMonthlyEvent[0]));
		_dataArray.Add(new TaskInfoItem(150, 680, 681, 682, 683, 684, 120, new List<int>(), new List<int>(), new List<int>(), isTriggeredTask: true, new List<int>(), 0, 99, 1, new List<short>(), null, null, null, null, null, new AutoTriggerMonthlyEvent[0]));
		_dataArray.Add(new TaskInfoItem(151, 685, 686, 687, 688, 689, 120, new List<int>(), new List<int>(), new List<int>(), isTriggeredTask: true, new List<int>(), 0, 99, 1, new List<short>(), null, null, null, null, null, new AutoTriggerMonthlyEvent[0]));
		_dataArray.Add(new TaskInfoItem(152, 690, 691, 692, 693, 694, 120, new List<int>(), new List<int>(), new List<int>(), isTriggeredTask: true, new List<int>(), 0, 99, 1, new List<short>(), null, null, null, "HeavenlyCaveLocations", null, new AutoTriggerMonthlyEvent[0]));
		_dataArray.Add(new TaskInfoItem(153, 695, 696, 697, 698, 699, 120, new List<int>(), new List<int>(), new List<int>(), isTriggeredTask: true, new List<int>(), 0, 99, 1, new List<short>(), null, null, null, null, null, new AutoTriggerMonthlyEvent[0]));
		_dataArray.Add(new TaskInfoItem(154, 700, 701, 702, 703, 704, 120, new List<int>(), new List<int>(), new List<int>(), isTriggeredTask: true, new List<int>(), 0, 99, 1, new List<short>(), null, null, null, null, null, new AutoTriggerMonthlyEvent[0]));
		_dataArray.Add(new TaskInfoItem(155, 705, 706, 707, 708, 709, 120, new List<int>(), new List<int>(), new List<int>(), isTriggeredTask: true, new List<int>(), 0, 99, 1, new List<short>(), null, null, null, null, null, new AutoTriggerMonthlyEvent[0]));
		_dataArray.Add(new TaskInfoItem(156, 710, 711, 712, 713, 714, 120, new List<int>(), new List<int>(), new List<int>(), isTriggeredTask: true, new List<int>(), 0, 99, 1, new List<short>(), null, null, null, null, null, new AutoTriggerMonthlyEvent[0]));
		_dataArray.Add(new TaskInfoItem(157, 715, 716, 717, 718, 719, 120, new List<int>(), new List<int>(), new List<int>(), isTriggeredTask: true, new List<int>(), 0, 99, 1, new List<short>(), null, null, null, null, null, new AutoTriggerMonthlyEvent[0]));
		_dataArray.Add(new TaskInfoItem(158, 720, 721, 722, 723, 724, 120, new List<int>(), new List<int>(), new List<int>(), isTriggeredTask: true, new List<int>(), 0, 99, 1, new List<short>(), null, null, null, null, null, new AutoTriggerMonthlyEvent[0]));
		_dataArray.Add(new TaskInfoItem(159, 725, 726, 727, 728, 729, 120, new List<int>(), new List<int>(), new List<int>(), isTriggeredTask: true, new List<int>(), 0, 99, 1, new List<short>(), null, null, null, null, null, new AutoTriggerMonthlyEvent[0]));
		_dataArray.Add(new TaskInfoItem(160, 725, 730, 731, 732, 733, 120, new List<int>(), new List<int>(), new List<int>(), isTriggeredTask: true, new List<int>(), 0, 99, 1, new List<short>(), null, null, null, null, null, new AutoTriggerMonthlyEvent[0]));
		_dataArray.Add(new TaskInfoItem(161, 720, 734, 735, 736, 737, 120, new List<int>(), new List<int>(), new List<int>(), isTriggeredTask: true, new List<int>(), 0, 99, 1, new List<short>(), null, null, null, null, null, new AutoTriggerMonthlyEvent[0]));
		_dataArray.Add(new TaskInfoItem(162, 738, 739, 740, 741, 742, 120, new List<int>(), new List<int>(), new List<int>(), isTriggeredTask: true, new List<int>(), 0, 99, 1, new List<short>(), null, null, null, null, null, new AutoTriggerMonthlyEvent[0]));
		_dataArray.Add(new TaskInfoItem(163, 743, 744, 745, 746, 747, 120, new List<int>(), new List<int>(), new List<int>(), isTriggeredTask: true, new List<int>(), 0, 99, 1, new List<short>(), null, null, null, null, null, new AutoTriggerMonthlyEvent[0]));
		_dataArray.Add(new TaskInfoItem(164, 748, 749, 750, 751, 752, 120, new List<int>(), new List<int>(), new List<int>(), isTriggeredTask: true, new List<int>(), 0, 99, 1, new List<short>(), null, null, null, null, null, new AutoTriggerMonthlyEvent[0]));
		_dataArray.Add(new TaskInfoItem(165, 753, 754, 755, 756, 757, 120, new List<int>(), new List<int>(), new List<int>(), isTriggeredTask: true, new List<int>(), 0, 99, 1, new List<short>(), null, null, null, null, null, new AutoTriggerMonthlyEvent[0]));
		_dataArray.Add(new TaskInfoItem(166, 748, 758, 759, 760, 761, 120, new List<int>(), new List<int>(), new List<int>(), isTriggeredTask: true, new List<int>(), 0, 99, 1, new List<short>(), null, null, null, null, null, new AutoTriggerMonthlyEvent[0]));
		_dataArray.Add(new TaskInfoItem(167, 762, 763, 764, 765, 766, 120, new List<int>(), new List<int>(), new List<int>(), isTriggeredTask: true, new List<int>(), 0, 99, 1, new List<short>(), null, null, null, null, null, new AutoTriggerMonthlyEvent[0]));
		_dataArray.Add(new TaskInfoItem(168, 767, 768, 769, 770, 771, 120, new List<int>(), new List<int>(), new List<int>(), isTriggeredTask: true, new List<int>(), 0, 99, 1, new List<short>(), null, null, null, null, null, new AutoTriggerMonthlyEvent[0]));
		_dataArray.Add(new TaskInfoItem(169, 772, 773, 774, 775, 776, 120, new List<int>(), new List<int>(), new List<int>(), isTriggeredTask: true, new List<int>(), 0, 99, 1, new List<short>(), null, null, null, null, null, new AutoTriggerMonthlyEvent[0]));
		_dataArray.Add(new TaskInfoItem(170, 777, 778, 779, 780, 781, 120, new List<int>(), new List<int>(), new List<int> { 249 }, isTriggeredTask: true, new List<int>(), 0, 99, 1, new List<short>(), null, null, null, null, null, new AutoTriggerMonthlyEvent[0]));
		_dataArray.Add(new TaskInfoItem(171, 782, 783, 784, 785, 786, 120, new List<int>(), new List<int>(), new List<int>(), isTriggeredTask: true, new List<int>(), 0, 99, 1, new List<short>(), null, null, null, null, null, new AutoTriggerMonthlyEvent[0]));
		_dataArray.Add(new TaskInfoItem(172, 787, 788, 789, 790, 791, 120, new List<int>(), new List<int>(), new List<int>(), isTriggeredTask: true, new List<int>(), 0, 99, 1, new List<short>(), null, null, null, null, null, new AutoTriggerMonthlyEvent[0]));
		_dataArray.Add(new TaskInfoItem(173, 792, 793, 794, 795, 796, 120, new List<int>(), new List<int>(), new List<int>(), isTriggeredTask: true, new List<int>(), 0, 99, 1, new List<short>(), null, null, null, null, null, new AutoTriggerMonthlyEvent[0]));
		_dataArray.Add(new TaskInfoItem(174, 797, 798, 799, 800, 801, 120, new List<int>(), new List<int>(), new List<int>(), isTriggeredTask: true, new List<int>(), 0, 99, 1, new List<short>(), null, null, null, null, null, new AutoTriggerMonthlyEvent[0]));
		_dataArray.Add(new TaskInfoItem(175, 802, 803, 804, 805, 806, 120, new List<int>(), new List<int>(), new List<int>(), isTriggeredTask: true, new List<int>(), 0, 99, 1, new List<short>(), null, null, null, null, null, new AutoTriggerMonthlyEvent[0]));
		_dataArray.Add(new TaskInfoItem(176, 807, 808, 809, 810, 811, 120, new List<int>(), new List<int>(), new List<int>(), isTriggeredTask: true, new List<int>(), 0, 99, 1, new List<short>(), null, null, null, null, null, new AutoTriggerMonthlyEvent[0]));
		_dataArray.Add(new TaskInfoItem(177, 812, 813, 814, 815, 816, 120, new List<int>(), new List<int>(), new List<int>(), isTriggeredTask: true, new List<int>(), 0, 99, 1, new List<short>(), null, null, null, null, null, new AutoTriggerMonthlyEvent[0]));
		_dataArray.Add(new TaskInfoItem(178, 817, 793, 818, 819, 820, 120, new List<int>(), new List<int>(), new List<int>(), isTriggeredTask: true, new List<int>(), 0, 99, 1, new List<short>(), null, null, null, null, null, new AutoTriggerMonthlyEvent[0]));
		_dataArray.Add(new TaskInfoItem(179, 821, 822, 823, 824, 825, 120, new List<int>(), new List<int>(), new List<int>(), isTriggeredTask: true, new List<int>(), 0, 99, 1, new List<short>(), null, null, null, null, null, new AutoTriggerMonthlyEvent[0]));
	}

	private void CreateItems3()
	{
		_dataArray.Add(new TaskInfoItem(180, 826, 827, 828, 829, 830, 120, new List<int>(), new List<int>(), new List<int>(), isTriggeredTask: true, new List<int>(), 0, 99, 1, new List<short>(), null, null, null, null, null, new AutoTriggerMonthlyEvent[0]));
		_dataArray.Add(new TaskInfoItem(181, 831, 832, 833, 834, 835, 120, new List<int>(), new List<int>(), new List<int>(), isTriggeredTask: true, new List<int>(), 0, 99, 1, new List<short>(), "ConchShip_PresetKey_JingangAdventureNearestSettlementId", null, null, null, null, new AutoTriggerMonthlyEvent[0]));
		_dataArray.Add(new TaskInfoItem(182, 836, 837, 838, 839, 840, 120, new List<int>(), new List<int>(), new List<int>(), isTriggeredTask: true, new List<int>(), 0, 99, 1, new List<short>(), null, null, null, null, null, new AutoTriggerMonthlyEvent[0]));
		_dataArray.Add(new TaskInfoItem(183, 243, 841, 842, 843, 844, 120, new List<int>(), new List<int>(), new List<int>(), isTriggeredTask: true, new List<int>(), 0, 99, 1, new List<short>(), null, null, null, null, null, new AutoTriggerMonthlyEvent[0]));
		_dataArray.Add(new TaskInfoItem(184, 845, 846, 847, 848, 849, 120, new List<int>(), new List<int>(), new List<int>(), isTriggeredTask: true, new List<int>(), 0, 99, 1, new List<short>(), null, null, null, null, null, new AutoTriggerMonthlyEvent[0]));
		_dataArray.Add(new TaskInfoItem(185, 850, 851, 852, 853, 854, 120, new List<int>(), new List<int>(), new List<int>(), isTriggeredTask: true, new List<int>(), 0, 99, 1, new List<short>(), null, null, null, null, null, new AutoTriggerMonthlyEvent[0]));
		_dataArray.Add(new TaskInfoItem(186, 855, 856, 857, 858, 859, 120, new List<int>(), new List<int>(), new List<int>(), isTriggeredTask: true, new List<int>(), 0, 99, 1, new List<short>(), null, null, null, null, null, new AutoTriggerMonthlyEvent[0]));
		_dataArray.Add(new TaskInfoItem(187, 860, 861, 862, 863, 864, 120, new List<int>(), new List<int>(), new List<int>(), isTriggeredTask: true, new List<int>(), 0, 99, 1, new List<short>(), null, null, null, null, null, new AutoTriggerMonthlyEvent[0]));
		_dataArray.Add(new TaskInfoItem(188, 865, 866, 867, 868, 869, 120, new List<int>(), new List<int>(), new List<int>(), isTriggeredTask: true, new List<int>(), 0, 99, 1, new List<short>(), null, null, null, null, null, new AutoTriggerMonthlyEvent[0]));
		_dataArray.Add(new TaskInfoItem(189, 870, 871, 872, 873, 874, 120, new List<int>(), new List<int>(), new List<int>(), isTriggeredTask: true, new List<int>(), 0, 99, 1, new List<short>(), null, null, null, null, null, new AutoTriggerMonthlyEvent[0]));
		_dataArray.Add(new TaskInfoItem(190, 875, 827, 876, 877, 878, 120, new List<int>(), new List<int>(), new List<int>(), isTriggeredTask: true, new List<int>(), 0, 99, 1, new List<short>(), null, null, null, null, null, new AutoTriggerMonthlyEvent[0]));
		_dataArray.Add(new TaskInfoItem(191, 879, 871, 880, 881, 882, 120, new List<int>(), new List<int>(), new List<int>(), isTriggeredTask: true, new List<int>(), 0, 99, 1, new List<short>(), null, null, null, null, null, new AutoTriggerMonthlyEvent[0]));
		_dataArray.Add(new TaskInfoItem(192, 883, 884, 885, 886, 887, 120, new List<int>(), new List<int>(), new List<int>(), isTriggeredTask: true, new List<int>(), 0, 99, 1, new List<short>(), null, null, null, null, null, new AutoTriggerMonthlyEvent[0]));
		_dataArray.Add(new TaskInfoItem(193, 888, 889, 890, 891, 892, 120, new List<int>(), new List<int>(), new List<int>(), isTriggeredTask: true, new List<int>(), 0, 99, 1, new List<short>(), null, null, null, null, null, new AutoTriggerMonthlyEvent[0]));
		_dataArray.Add(new TaskInfoItem(194, 893, 894, 895, 896, 897, 120, new List<int>(), new List<int>(), new List<int>(), isTriggeredTask: true, new List<int>(), 0, 99, 1, new List<short>(), null, null, null, null, null, new AutoTriggerMonthlyEvent[0]));
		_dataArray.Add(new TaskInfoItem(195, 898, 899, 900, 901, 902, 120, new List<int>(), new List<int>(), new List<int>(), isTriggeredTask: true, new List<int>(), 0, 99, 1, new List<short>(), null, null, null, null, null, new AutoTriggerMonthlyEvent[0]));
		_dataArray.Add(new TaskInfoItem(196, 903, 904, 905, 906, 907, 120, new List<int>(), new List<int>(), new List<int>(), isTriggeredTask: true, new List<int>(), 0, 99, 1, new List<short>(), null, null, null, null, null, new AutoTriggerMonthlyEvent[0]));
		_dataArray.Add(new TaskInfoItem(197, 908, 909, 910, 911, 912, 120, new List<int>(), new List<int>(), new List<int>(), isTriggeredTask: true, new List<int>(), 0, 99, 1, new List<short>(), null, null, null, null, null, new AutoTriggerMonthlyEvent[0]));
		_dataArray.Add(new TaskInfoItem(198, 913, 914, 915, 916, 917, 120, new List<int>(), new List<int>(), new List<int>(), isTriggeredTask: true, new List<int>(), 0, 99, 1, new List<short>(), null, null, null, null, null, new AutoTriggerMonthlyEvent[0]));
		_dataArray.Add(new TaskInfoItem(199, 918, 919, 920, 921, 922, 120, new List<int>(), new List<int>(), new List<int>(), isTriggeredTask: true, new List<int>(), 0, 99, 1, new List<short>(), null, null, null, null, null, new AutoTriggerMonthlyEvent[0]));
		_dataArray.Add(new TaskInfoItem(200, 923, 924, 925, 926, 927, 120, new List<int>(), new List<int>(), new List<int>(), isTriggeredTask: true, new List<int>(), 0, 99, 1, new List<short>(), null, null, null, null, null, new AutoTriggerMonthlyEvent[0]));
		_dataArray.Add(new TaskInfoItem(201, 928, 929, 930, 931, 932, 120, new List<int>(), new List<int>(), new List<int>(), isTriggeredTask: true, new List<int>(), 0, 99, 1, new List<short>(), null, null, null, null, null, new AutoTriggerMonthlyEvent[0]));
		_dataArray.Add(new TaskInfoItem(202, 933, 934, 935, 936, 937, 120, new List<int>(), new List<int>(), new List<int>(), isTriggeredTask: true, new List<int>(), 0, 99, 1, new List<short>(), null, null, null, null, null, new AutoTriggerMonthlyEvent[0]));
		_dataArray.Add(new TaskInfoItem(203, 938, 939, 940, 941, 942, 120, new List<int>(), new List<int>(), new List<int>(), isTriggeredTask: true, new List<int>(), 0, 99, 1, new List<short>(), null, null, null, null, null, new AutoTriggerMonthlyEvent[0]));
		_dataArray.Add(new TaskInfoItem(204, 943, 944, 945, 946, 947, 120, new List<int>(), new List<int>(), new List<int>(), isTriggeredTask: true, new List<int>(), 0, 99, 1, new List<short>(), null, null, null, null, null, new AutoTriggerMonthlyEvent[0]));
		_dataArray.Add(new TaskInfoItem(205, 948, 949, 950, 951, 952, 120, new List<int>(), new List<int>(), new List<int>(), isTriggeredTask: true, new List<int>(), 0, 99, 1, new List<short>(), null, null, null, null, null, new AutoTriggerMonthlyEvent[0]));
		_dataArray.Add(new TaskInfoItem(206, 953, 954, 955, 956, 957, 120, new List<int>(), new List<int>(), new List<int>(), isTriggeredTask: true, new List<int>(), 0, 99, 1, new List<short>(), null, null, null, null, null, new AutoTriggerMonthlyEvent[0]));
		_dataArray.Add(new TaskInfoItem(207, 958, 959, 960, 961, 962, 120, new List<int>(), new List<int>(), new List<int>(), isTriggeredTask: true, new List<int>(), 0, 99, 1, new List<short>(), null, null, null, null, null, new AutoTriggerMonthlyEvent[0]));
		_dataArray.Add(new TaskInfoItem(208, 963, 964, 965, 966, 967, 120, new List<int>(), new List<int>(), new List<int>(), isTriggeredTask: true, new List<int>(), 0, 99, 1, new List<short>(), null, null, null, null, null, new AutoTriggerMonthlyEvent[0]));
		_dataArray.Add(new TaskInfoItem(209, 968, 969, 970, 971, 972, 120, new List<int>(), new List<int>(), new List<int>(), isTriggeredTask: true, new List<int>(), 0, 99, 1, new List<short>(), null, null, null, null, null, new AutoTriggerMonthlyEvent[0]));
		_dataArray.Add(new TaskInfoItem(210, 973, 974, 975, 976, 977, 120, new List<int>(), new List<int>(), new List<int>(), isTriggeredTask: true, new List<int>(), 0, 99, 1, new List<short>(), null, null, null, null, null, new AutoTriggerMonthlyEvent[0]));
		_dataArray.Add(new TaskInfoItem(211, 978, 979, 980, 981, 982, 120, new List<int>(), new List<int>(), new List<int>(), isTriggeredTask: true, new List<int>(), 0, 99, 1, new List<short>(), null, null, null, null, null, new AutoTriggerMonthlyEvent[0]));
		_dataArray.Add(new TaskInfoItem(212, 983, 984, 985, 986, 987, 120, new List<int>(), new List<int>(), new List<int>(), isTriggeredTask: true, new List<int>(), 0, 99, 1, new List<short>(), null, null, null, null, null, new AutoTriggerMonthlyEvent[0]));
		_dataArray.Add(new TaskInfoItem(213, 988, 989, 990, 991, 992, 120, new List<int>(), new List<int>(), new List<int>(), isTriggeredTask: true, new List<int>(), 0, 99, 1, new List<short>(), null, null, null, null, null, new AutoTriggerMonthlyEvent[0]));
		_dataArray.Add(new TaskInfoItem(214, 993, 994, 995, 996, 997, 120, new List<int>(), new List<int>(), new List<int>(), isTriggeredTask: true, new List<int>(), 0, 99, 1, new List<short>(), null, null, null, null, null, new AutoTriggerMonthlyEvent[0]));
		_dataArray.Add(new TaskInfoItem(215, 998, 999, 1000, 1001, 1002, 120, new List<int>(), new List<int>(), new List<int>(), isTriggeredTask: true, new List<int>(), 0, 99, 1, new List<short>(), null, null, null, null, null, new AutoTriggerMonthlyEvent[0]));
		_dataArray.Add(new TaskInfoItem(216, 1003, 1004, 1005, 1006, 1007, 120, new List<int>(), new List<int>(), new List<int>(), isTriggeredTask: true, new List<int>(), 0, 99, 1, new List<short>(), null, null, null, null, null, new AutoTriggerMonthlyEvent[0]));
		_dataArray.Add(new TaskInfoItem(217, 1008, 1009, 1010, 1011, 1012, 120, new List<int>(), new List<int>(), new List<int>(), isTriggeredTask: true, new List<int>(), 0, 99, 1, new List<short>(), null, null, null, null, null, new AutoTriggerMonthlyEvent[0]));
		_dataArray.Add(new TaskInfoItem(218, 1013, 1013, 1014, 1015, 1016, 120, new List<int>(), new List<int>(), new List<int>(), isTriggeredTask: true, new List<int>(), 0, 99, 1, new List<short>(), null, null, null, null, null, new AutoTriggerMonthlyEvent[0]));
		_dataArray.Add(new TaskInfoItem(219, 1017, 1018, 1019, 1020, 1021, 120, new List<int>(), new List<int>(), new List<int>(), isTriggeredTask: true, new List<int>(), 0, 99, 1, new List<short>(), null, null, null, null, null, new AutoTriggerMonthlyEvent[0]));
		_dataArray.Add(new TaskInfoItem(220, 1022, 1023, 1024, 1025, 1026, 120, new List<int>(), new List<int>(), new List<int>(), isTriggeredTask: true, new List<int>(), 0, 99, 1, new List<short>(), null, null, null, null, null, new AutoTriggerMonthlyEvent[0]));
		_dataArray.Add(new TaskInfoItem(221, 1027, 1028, 1029, 1030, 1031, 120, new List<int>(), new List<int>(), new List<int>(), isTriggeredTask: true, new List<int>(), 0, 99, 1, new List<short>(), null, null, null, null, null, new AutoTriggerMonthlyEvent[0]));
		_dataArray.Add(new TaskInfoItem(222, 1032, 1033, 1034, 1035, 1036, 120, new List<int>(), new List<int>(), new List<int>(), isTriggeredTask: true, new List<int>(), 0, 99, 1, new List<short>(), null, null, null, null, null, new AutoTriggerMonthlyEvent[0]));
		_dataArray.Add(new TaskInfoItem(223, 1037, 871, 1038, 1039, 1040, 120, new List<int>(), new List<int>(), new List<int>(), isTriggeredTask: true, new List<int>(), 0, 99, 1, new List<short>(), null, null, null, null, null, new AutoTriggerMonthlyEvent[0]));
		_dataArray.Add(new TaskInfoItem(224, 1041, 1042, 1043, 1044, 1045, 120, new List<int>(), new List<int>(), new List<int>(), isTriggeredTask: true, new List<int>(), 0, 99, 1, new List<short>(), null, new string[3] { "ConchShip_PresetKey_Shaolin_LearningSkill0", "ConchShip_PresetKey_Shaolin_LearningSkill1", "ConchShip_PresetKey_Shaolin_LearningSkill2" }, null, null, null, new AutoTriggerMonthlyEvent[0]));
		_dataArray.Add(new TaskInfoItem(225, 1046, 1047, 1048, 1049, 1050, 120, new List<int>(), new List<int>(), new List<int>(), isTriggeredTask: true, new List<int>(), 0, 99, 1, new List<short>(), null, null, null, null, null, new AutoTriggerMonthlyEvent[0]));
		_dataArray.Add(new TaskInfoItem(226, 1051, 1052, 1053, 1054, 1055, 120, new List<int>(), new List<int>(), new List<int>(), isTriggeredTask: true, new List<int>(), 0, 99, 1, new List<short>(), null, null, null, null, null, new AutoTriggerMonthlyEvent[0]));
		_dataArray.Add(new TaskInfoItem(227, 1056, 1057, 1058, 1059, 1060, 120, new List<int>(), new List<int>(), new List<int>(), isTriggeredTask: true, new List<int>(), 0, 99, 1, new List<short>(), "ConchShip_PresetKey_StudyForBodhidharmaChallenge", null, null, null, null, new AutoTriggerMonthlyEvent[0]));
		_dataArray.Add(new TaskInfoItem(228, 1061, 1062, 1063, 1064, 1065, 120, new List<int>(), new List<int>(), new List<int>(), isTriggeredTask: true, new List<int>(), 0, 99, 1, new List<short>(), null, null, null, null, null, new AutoTriggerMonthlyEvent[0]));
		_dataArray.Add(new TaskInfoItem(229, 1066, 1067, 1068, 1069, 1070, 120, new List<int>(), new List<int>(), new List<int>(), isTriggeredTask: true, new List<int>(), 0, 99, 1, new List<short>(), null, null, new string[1] { "ConchShip_PresetKey_ShaolinReadingMaxGradeSutra" }, null, null, new AutoTriggerMonthlyEvent[0]));
		_dataArray.Add(new TaskInfoItem(230, 1071, 1072, 1073, 1074, 1075, 120, new List<int>(), new List<int>(), new List<int>(), isTriggeredTask: true, new List<int>(), 0, 99, 1, new List<short>(), null, null, null, null, null, new AutoTriggerMonthlyEvent[0]));
		_dataArray.Add(new TaskInfoItem(231, 1076, 1077, 1078, 1079, 1080, 120, new List<int>(), new List<int>(), new List<int>(), isTriggeredTask: true, new List<int>(), 0, 99, 1, new List<short>(), null, null, null, null, null, new AutoTriggerMonthlyEvent[0]));
		_dataArray.Add(new TaskInfoItem(232, 1081, 1082, 1083, 1084, 1085, 120, new List<int>(), new List<int>(), new List<int>(), isTriggeredTask: true, new List<int>(), 0, 99, 1, new List<short>(), null, null, null, null, null, new AutoTriggerMonthlyEvent[0]));
		_dataArray.Add(new TaskInfoItem(233, 1086, 793, 1087, 1088, 1089, 120, new List<int>(), new List<int>(), new List<int>(), isTriggeredTask: true, new List<int>(), 0, 99, 1, new List<short>(), null, null, null, null, null, new AutoTriggerMonthlyEvent[0]));
		_dataArray.Add(new TaskInfoItem(234, 812, 1090, 1091, 1092, 1093, 120, new List<int>(), new List<int>(), new List<int>(), isTriggeredTask: true, new List<int>(), 0, 99, 1, new List<short>(), null, null, null, null, null, new AutoTriggerMonthlyEvent[0]));
		_dataArray.Add(new TaskInfoItem(235, 1094, 126, 1095, 1096, 1097, 240, new List<int> { 25, 248 }, new List<int> { 25, 29 }, new List<int>(), isTriggeredTask: false, new List<int>(), 8, 9, 0, new List<short>(), null, null, null, null, null, new AutoTriggerMonthlyEvent[0]));
		_dataArray.Add(new TaskInfoItem(236, 1098, 1099, 1100, 1101, 1102, 120, new List<int>(), new List<int>(), new List<int>(), isTriggeredTask: true, new List<int>(), 0, 99, 1, new List<short>(), null, null, null, null, null, new AutoTriggerMonthlyEvent[0]));
		_dataArray.Add(new TaskInfoItem(237, 1103, 1104, 1105, 1106, 1107, 120, new List<int>(), new List<int>(), new List<int>(), isTriggeredTask: true, new List<int>(), 0, 99, 1, new List<short>(), null, null, null, null, null, new AutoTriggerMonthlyEvent[0]));
		_dataArray.Add(new TaskInfoItem(238, 1108, 1109, 1110, 1111, 1112, 120, new List<int>(), new List<int>(), new List<int>(), isTriggeredTask: true, new List<int>(), 0, 99, 1, new List<short>(), null, null, null, null, null, new AutoTriggerMonthlyEvent[0]));
		_dataArray.Add(new TaskInfoItem(239, 1113, 1114, 1115, 1116, 1117, 120, new List<int>(), new List<int>(), new List<int>(), isTriggeredTask: true, new List<int>(), 0, 99, 1, new List<short>(), null, null, null, null, null, new AutoTriggerMonthlyEvent[0]));
	}

	private void CreateItems4()
	{
		_dataArray.Add(new TaskInfoItem(240, 1118, 1119, 1120, 1121, 1122, 120, new List<int>(), new List<int>(), new List<int>(), isTriggeredTask: true, new List<int>(), 0, 99, 1, new List<short>(), null, null, null, null, null, new AutoTriggerMonthlyEvent[0]));
		_dataArray.Add(new TaskInfoItem(241, 1123, 1124, 1125, 1126, 1127, 120, new List<int>(), new List<int>(), new List<int>(), isTriggeredTask: true, new List<int>(), 0, 99, 1, new List<short>(), null, null, null, null, null, new AutoTriggerMonthlyEvent[0]));
		_dataArray.Add(new TaskInfoItem(242, 1128, 1129, 1130, 1131, 1132, 120, new List<int>(), new List<int>(), new List<int>(), isTriggeredTask: true, new List<int>(), 0, 99, 1, new List<short>(), null, null, null, null, null, new AutoTriggerMonthlyEvent[0]));
		_dataArray.Add(new TaskInfoItem(243, 1133, 1134, 1135, 1136, 1137, 120, new List<int>(), new List<int>(), new List<int>(), isTriggeredTask: true, new List<int>(), 0, 99, 1, new List<short>(), null, null, null, null, null, new AutoTriggerMonthlyEvent[0]));
		_dataArray.Add(new TaskInfoItem(244, 1138, 1139, 1140, 1141, 1142, 120, new List<int>(), new List<int>(), new List<int>(), isTriggeredTask: true, new List<int>(), 0, 99, 1, new List<short>(), null, null, null, null, null, new AutoTriggerMonthlyEvent[0]));
		_dataArray.Add(new TaskInfoItem(245, 1143, 1144, 1145, 1146, 1147, 120, new List<int>(), new List<int>(), new List<int>(), isTriggeredTask: true, new List<int>(), 0, 99, 1, new List<short>(), null, null, null, null, null, new AutoTriggerMonthlyEvent[0]));
		_dataArray.Add(new TaskInfoItem(246, 1148, 1149, 1150, 1151, 1152, 120, new List<int>(), new List<int>(), new List<int>(), isTriggeredTask: true, new List<int>(), 0, 99, 1, new List<short>(), null, new string[3] { "ConchShip_PresetKey_XuannvPartThree_LearningSkillId_0", "ConchShip_PresetKey_XuannvPartThree_LearningSkillId_1", "ConchShip_PresetKey_XuannvPartThree_LearningSkillId_2" }, null, null, null, new AutoTriggerMonthlyEvent[0]));
		_dataArray.Add(new TaskInfoItem(247, 1153, 1154, 1155, 1156, 1157, 120, new List<int>(), new List<int>(), new List<int>(), isTriggeredTask: true, new List<int>(), 0, 99, 1, new List<short>(), null, null, null, null, null, new AutoTriggerMonthlyEvent[0]));
		_dataArray.Add(new TaskInfoItem(248, 1158, 1159, 1160, 1161, 1162, 120, new List<int>(), new List<int>(), new List<int>(), isTriggeredTask: true, new List<int>(), 0, 99, 1, new List<short>(), null, null, null, null, null, new AutoTriggerMonthlyEvent[0]));
		_dataArray.Add(new TaskInfoItem(249, 641, 642, 643, 1163, 1164, 120, new List<int>(), new List<int>(), new List<int>(), isTriggeredTask: true, new List<int>(), 0, 99, 1, new List<short>(), null, null, null, null, null, new AutoTriggerMonthlyEvent[0]));
		_dataArray.Add(new TaskInfoItem(250, 1165, 1166, 1167, 1168, 1169, 120, new List<int>(), new List<int>(), new List<int>(), isTriggeredTask: true, new List<int>(), 0, 99, 1, new List<short>(), null, null, null, null, null, new AutoTriggerMonthlyEvent[0]));
		_dataArray.Add(new TaskInfoItem(251, 1170, 652, 1171, 1172, 1173, 120, new List<int>(), new List<int>(), new List<int>(), isTriggeredTask: true, new List<int>(), 0, 99, 1, new List<short>(), null, null, null, null, null, new AutoTriggerMonthlyEvent[0]));
		_dataArray.Add(new TaskInfoItem(252, 1174, 1175, 1176, 1177, 1178, 120, new List<int>(), new List<int>(), new List<int>(), isTriggeredTask: true, new List<int>(), 0, 99, 1, new List<short>(), null, null, null, null, null, new AutoTriggerMonthlyEvent[0]));
		_dataArray.Add(new TaskInfoItem(253, 1179, 1180, 1181, 1182, 1183, 120, new List<int>(), new List<int>(), new List<int>(), isTriggeredTask: true, new List<int>(), 0, 99, 1, new List<short>(), null, null, null, null, null, new AutoTriggerMonthlyEvent[0]));
		_dataArray.Add(new TaskInfoItem(254, 1188, 1189, 1190, 1191, 1192, 120, new List<int>(), new List<int>(), new List<int>(), isTriggeredTask: true, new List<int>(), 0, 99, 1, new List<short>(), null, null, null, null, null, new AutoTriggerMonthlyEvent[0]));
		_dataArray.Add(new TaskInfoItem(255, 1193, 1194, 1195, 1196, 1197, 120, new List<int>(), new List<int>(), new List<int>(), isTriggeredTask: true, new List<int>(), 0, 99, 1, new List<short>(), null, null, null, null, null, new AutoTriggerMonthlyEvent[0]));
		_dataArray.Add(new TaskInfoItem(256, 656, 1184, 1185, 1186, 1187, 120, new List<int>(), new List<int>(), new List<int>(), isTriggeredTask: true, new List<int>(), 0, 99, 1, new List<short>(), null, null, null, null, null, new AutoTriggerMonthlyEvent[0]));
		_dataArray.Add(new TaskInfoItem(257, 1198, 1199, 1200, 1201, 1202, 120, new List<int>(), new List<int>(), new List<int>(), isTriggeredTask: true, new List<int>(), 0, 99, 2, new List<short>(), null, null, null, null, null, new AutoTriggerMonthlyEvent[0]));
		_dataArray.Add(new TaskInfoItem(258, 1203, 1204, 1205, 1206, 1207, 120, new List<int>(), new List<int>(), new List<int>(), isTriggeredTask: true, new List<int>(), 0, 99, 1, new List<short>(), null, null, null, null, null, new AutoTriggerMonthlyEvent[0]));
		_dataArray.Add(new TaskInfoItem(259, 1208, 1209, 1210, 1211, 1212, 120, new List<int>(), new List<int>(), new List<int>(), isTriggeredTask: true, new List<int>(), 0, 99, 1, new List<short>(), null, null, null, null, null, new AutoTriggerMonthlyEvent[0]));
		_dataArray.Add(new TaskInfoItem(260, 1213, 1214, 1215, 1216, 1217, 120, new List<int>(), new List<int>(), new List<int>(), isTriggeredTask: true, new List<int>(), 0, 99, 0, new List<short>(), null, null, null, null, null, new AutoTriggerMonthlyEvent[0]));
		_dataArray.Add(new TaskInfoItem(261, 1218, 1219, 1220, 1221, 1222, 120, new List<int>(), new List<int>(), new List<int>(), isTriggeredTask: true, new List<int>(), 0, 99, 2, new List<short>(), null, null, null, null, null, new AutoTriggerMonthlyEvent[0]));
		_dataArray.Add(new TaskInfoItem(262, 1223, 1224, 1225, 1221, 1226, 120, new List<int>(), new List<int>(), new List<int>(), isTriggeredTask: true, new List<int>(), 0, 99, 2, new List<short>(), null, null, null, null, null, new AutoTriggerMonthlyEvent[0]));
		_dataArray.Add(new TaskInfoItem(263, 1227, 1228, 1229, 1221, 1230, 120, new List<int>(), new List<int>(), new List<int>(), isTriggeredTask: true, new List<int>(), 0, 99, 2, new List<short>(), null, null, null, null, null, new AutoTriggerMonthlyEvent[0]));
		_dataArray.Add(new TaskInfoItem(264, 1231, 1232, 1233, 1234, 1235, 120, new List<int>(), new List<int>(), new List<int>(), isTriggeredTask: true, new List<int>(), 0, 99, 2, new List<short>(), null, null, null, null, null, new AutoTriggerMonthlyEvent[0]));
		_dataArray.Add(new TaskInfoItem(265, 1236, 1237, 1238, 1239, 1240, 120, new List<int>(), new List<int>(), new List<int>(), isTriggeredTask: true, new List<int>(), 0, 99, 2, new List<short>(), null, null, null, null, null, new AutoTriggerMonthlyEvent[0]));
		_dataArray.Add(new TaskInfoItem(266, 1241, 1242, 1243, 1244, 1245, 120, new List<int>(), new List<int>(), new List<int>(), isTriggeredTask: true, new List<int>(), 0, 99, 2, new List<short>(), null, null, null, null, null, new AutoTriggerMonthlyEvent[0]));
		_dataArray.Add(new TaskInfoItem(267, 1246, 871, 1247, 1248, 1249, 120, new List<int>(), new List<int>(), new List<int>(), isTriggeredTask: true, new List<int>(), 0, 99, 2, new List<short>(), null, null, null, null, null, new AutoTriggerMonthlyEvent[0]));
		_dataArray.Add(new TaskInfoItem(268, 1250, 1251, 1252, 1253, 1254, 120, new List<int>(), new List<int>(), new List<int>(), isTriggeredTask: true, new List<int>(), 0, 99, 2, new List<short>(), null, null, null, null, null, new AutoTriggerMonthlyEvent[0]));
		_dataArray.Add(new TaskInfoItem(269, 1255, 1256, 1257, 1258, 1259, 120, new List<int>(), new List<int>(), new List<int>(), isTriggeredTask: true, new List<int>(), 0, 99, 2, new List<short>(), null, null, null, null, null, new AutoTriggerMonthlyEvent[0]));
		_dataArray.Add(new TaskInfoItem(270, 1260, 1261, 1262, 1263, 1264, 120, new List<int>(), new List<int>(), new List<int>(), isTriggeredTask: true, new List<int>(), 0, 99, 2, new List<short>(), null, null, null, null, null, new AutoTriggerMonthlyEvent[0]));
		_dataArray.Add(new TaskInfoItem(271, 1265, 1266, 1267, 1268, 1269, 120, new List<int>(), new List<int>(), new List<int>(), isTriggeredTask: true, new List<int>(), 0, 99, 2, new List<short>(), null, null, null, null, null, new AutoTriggerMonthlyEvent[0]));
		_dataArray.Add(new TaskInfoItem(272, 1270, 1271, 1272, 1273, 1274, 120, new List<int>(), new List<int>(), new List<int>(), isTriggeredTask: true, new List<int>(), 0, 99, 2, new List<short>(), null, null, null, null, null, new AutoTriggerMonthlyEvent[0]));
		_dataArray.Add(new TaskInfoItem(273, 1275, 1276, 1277, 1278, 1279, 120, new List<int>(), new List<int>(), new List<int>(), isTriggeredTask: true, new List<int>(), 0, 99, 2, new List<short>(), null, null, null, null, null, new AutoTriggerMonthlyEvent[0]));
		_dataArray.Add(new TaskInfoItem(274, 1280, 1281, 1282, 1283, 1284, 120, new List<int>(), new List<int>(), new List<int>(), isTriggeredTask: true, new List<int>(), 0, 99, 2, new List<short>(), null, null, null, null, null, new AutoTriggerMonthlyEvent[0]));
		_dataArray.Add(new TaskInfoItem(275, 1285, 1286, 1287, 1288, 1289, 120, new List<int>(), new List<int>(), new List<int>(), isTriggeredTask: true, new List<int>(), 0, 99, 1, new List<short>(), null, null, null, null, null, new AutoTriggerMonthlyEvent[0]));
		_dataArray.Add(new TaskInfoItem(276, 1290, 1291, 1292, 1293, 1294, 120, new List<int>(), new List<int>(), new List<int>(), isTriggeredTask: true, new List<int>(), 0, 99, 1, new List<short>(), null, null, null, null, null, new AutoTriggerMonthlyEvent[0]));
		_dataArray.Add(new TaskInfoItem(277, 888, 1295, 890, 1296, 1297, 120, new List<int>(), new List<int>(), new List<int>(), isTriggeredTask: true, new List<int>(), 0, 99, 1, new List<short>(), null, null, null, null, null, new AutoTriggerMonthlyEvent[0]));
		_dataArray.Add(new TaskInfoItem(278, 1298, 627, 1299, 1300, 1301, 120, new List<int>(), new List<int>(), new List<int>(), isTriggeredTask: true, new List<int>(), 0, 99, 1, new List<short>(), null, null, null, null, null, new AutoTriggerMonthlyEvent[0]));
		_dataArray.Add(new TaskInfoItem(279, 1302, 1303, 1304, 1305, 1306, 120, new List<int>(), new List<int>(), new List<int>(), isTriggeredTask: true, new List<int>(), 0, 99, 1, new List<short>(), null, null, null, null, null, new AutoTriggerMonthlyEvent[0]));
		_dataArray.Add(new TaskInfoItem(280, 1307, 1308, 1309, 1310, 1311, 120, new List<int>(), new List<int>(), new List<int>(), isTriggeredTask: true, new List<int>(), 0, 99, 1, new List<short>(), null, null, null, null, null, new AutoTriggerMonthlyEvent[0]));
		_dataArray.Add(new TaskInfoItem(281, 1312, 1313, 1314, 1315, 1316, 120, new List<int>(), new List<int>(), new List<int>(), isTriggeredTask: true, new List<int>(), 0, 99, 1, new List<short>(), null, null, null, null, null, new AutoTriggerMonthlyEvent[0]));
		_dataArray.Add(new TaskInfoItem(282, 1317, 1318, 1319, 1320, 1321, 120, new List<int>(), new List<int>(), new List<int>(), isTriggeredTask: true, new List<int>(), 0, 99, 1, new List<short>(), null, null, null, null, null, new AutoTriggerMonthlyEvent[0]));
		_dataArray.Add(new TaskInfoItem(283, 1322, 1323, 1324, 1325, 1326, 120, new List<int>(), new List<int>(), new List<int>(), isTriggeredTask: true, new List<int>(), 0, 99, 1, new List<short>(), null, null, null, null, null, new AutoTriggerMonthlyEvent[0]));
		_dataArray.Add(new TaskInfoItem(284, 1327, 919, 1328, 1329, 1330, 120, new List<int>(), new List<int>(), new List<int>(), isTriggeredTask: true, new List<int>(), 0, 99, 1, new List<short>(), null, null, null, null, null, new AutoTriggerMonthlyEvent[0]));
		_dataArray.Add(new TaskInfoItem(285, 1331, 1303, 1332, 1333, 1334, 120, new List<int>(), new List<int>(), new List<int>(), isTriggeredTask: true, new List<int>(), 0, 99, 1, new List<short>(), null, null, null, null, null, new AutoTriggerMonthlyEvent[0]));
		_dataArray.Add(new TaskInfoItem(286, 1335, 871, 1336, 1337, 1338, 120, new List<int>(), new List<int>(), new List<int>(), isTriggeredTask: true, new List<int>(), 0, 99, 1, new List<short>(), null, null, null, null, null, new AutoTriggerMonthlyEvent[0]));
		_dataArray.Add(new TaskInfoItem(287, 1339, 1340, 1341, 1342, 1343, 120, new List<int>(), new List<int>(), new List<int>(), isTriggeredTask: true, new List<int>(), 0, 99, 1, new List<short>(), null, null, null, null, null, new AutoTriggerMonthlyEvent[0]));
		_dataArray.Add(new TaskInfoItem(288, 1344, 1345, 1346, 1347, 1348, 120, new List<int>(), new List<int>(), new List<int>(), isTriggeredTask: true, new List<int>(), 0, 99, 1, new List<short>(), null, null, null, null, null, new AutoTriggerMonthlyEvent[0]));
		_dataArray.Add(new TaskInfoItem(289, 1349, 1350, 1351, 1352, 1353, 120, new List<int>(), new List<int>(), new List<int>(), isTriggeredTask: true, new List<int>(), 0, 99, 1, new List<short>(), null, null, null, null, null, new AutoTriggerMonthlyEvent[0]));
		_dataArray.Add(new TaskInfoItem(290, 1354, 1355, 1356, 1357, 1358, 120, new List<int>(), new List<int>(), new List<int>(), isTriggeredTask: true, new List<int>(), 0, 99, 1, new List<short>(), null, null, null, null, null, new AutoTriggerMonthlyEvent[0]));
		_dataArray.Add(new TaskInfoItem(291, 1359, 1340, 1360, 1361, 1362, 120, new List<int>(), new List<int>(), new List<int>(), isTriggeredTask: true, new List<int>(), 0, 99, 1, new List<short>(), null, null, null, null, null, new AutoTriggerMonthlyEvent[0]));
		_dataArray.Add(new TaskInfoItem(292, 1363, 1364, 1365, 1366, 1367, 120, new List<int>(), new List<int>(), new List<int>(), isTriggeredTask: true, new List<int>(), 0, 99, 1, new List<short>(), null, null, null, null, null, new AutoTriggerMonthlyEvent[0]));
		_dataArray.Add(new TaskInfoItem(293, 1368, 1369, 1370, 1371, 1372, 120, new List<int>(), new List<int>(), new List<int>(), isTriggeredTask: true, new List<int>(), 0, 99, 1, new List<short>(), null, null, null, null, null, new AutoTriggerMonthlyEvent[0]));
		_dataArray.Add(new TaskInfoItem(294, 1373, 1374, 1375, 1376, 1377, 120, new List<int>(), new List<int>(), new List<int>(), isTriggeredTask: true, new List<int>(), 0, 99, 1, new List<short>(), null, null, null, null, null, new AutoTriggerMonthlyEvent[0]));
		_dataArray.Add(new TaskInfoItem(295, 1378, 1379, 1380, 1381, 1382, 120, new List<int>(), new List<int>(), new List<int>(), isTriggeredTask: true, new List<int>(), 0, 99, 1, new List<short>(), null, null, null, null, null, new AutoTriggerMonthlyEvent[0]));
		_dataArray.Add(new TaskInfoItem(296, 1383, 1384, 1385, 1386, 1387, 120, new List<int>(), new List<int>(), new List<int>(), isTriggeredTask: true, new List<int>(), 0, 99, 1, new List<short>(), "ConchShip_PresetKey_SanZongBiWuCountDown", null, null, null, null, new AutoTriggerMonthlyEvent[0]));
		_dataArray.Add(new TaskInfoItem(297, 1017, 1388, 1389, 1390, 1391, 120, new List<int>(), new List<int>(), new List<int>(), isTriggeredTask: true, new List<int>(), 0, 99, 1, new List<short>(), null, null, null, null, null, new AutoTriggerMonthlyEvent[0]));
		_dataArray.Add(new TaskInfoItem(298, 1397, 1398, 1399, 1400, 1401, 120, new List<int>(), new List<int>(), new List<int>(), isTriggeredTask: true, new List<int>(), 0, 99, 1, new List<short>(), null, null, null, null, null, new AutoTriggerMonthlyEvent[0]));
		_dataArray.Add(new TaskInfoItem(299, 1392, 1393, 1394, 1395, 1396, 120, new List<int>(), new List<int>(), new List<int>(), isTriggeredTask: true, new List<int>(), 0, 99, 1, new List<short>(), null, null, null, null, null, new AutoTriggerMonthlyEvent[0]));
	}

	private void CreateItems5()
	{
		_dataArray.Add(new TaskInfoItem(300, 1402, 1403, 1394, 1395, 1404, 120, new List<int>(), new List<int>(), new List<int>(), isTriggeredTask: true, new List<int>(), 0, 99, 1, new List<short>(), null, null, null, null, null, new AutoTriggerMonthlyEvent[0]));
		_dataArray.Add(new TaskInfoItem(301, 1405, 1406, 1407, 1408, 1409, 120, new List<int>(), new List<int>(), new List<int>(), isTriggeredTask: true, new List<int>(), 0, 99, 1, new List<short>(), null, null, null, null, null, new AutoTriggerMonthlyEvent[0]));
		_dataArray.Add(new TaskInfoItem(302, 1410, 1411, 1412, 1413, 1414, 120, new List<int>(), new List<int>(), new List<int>(), isTriggeredTask: true, new List<int>(), 0, 99, 1, new List<short>(), null, null, null, null, null, new AutoTriggerMonthlyEvent[0]));
		_dataArray.Add(new TaskInfoItem(303, 1415, 1416, 1417, 1418, 1419, 120, new List<int>(), new List<int>(), new List<int>(), isTriggeredTask: true, new List<int>(), 0, 99, 1, new List<short>(), null, null, null, null, null, new AutoTriggerMonthlyEvent[0]));
		_dataArray.Add(new TaskInfoItem(304, 1420, 1421, 1422, 1423, 1424, 120, new List<int>(), new List<int>(), new List<int>(), isTriggeredTask: true, new List<int>(), 0, 99, 1, new List<short>(), null, null, null, null, null, new AutoTriggerMonthlyEvent[0]));
		_dataArray.Add(new TaskInfoItem(305, 1425, 1426, 1427, 1428, 1429, 120, new List<int>(), new List<int>(), new List<int>(), isTriggeredTask: true, new List<int>(), 0, 99, 1, new List<short>(), null, null, null, null, null, new AutoTriggerMonthlyEvent[0]));
		_dataArray.Add(new TaskInfoItem(306, 1430, 666, 1431, 1432, 1433, 120, new List<int>(), new List<int>(), new List<int>(), isTriggeredTask: true, new List<int>(), 0, 99, 1, new List<short>(), "ConchShip_PresetKey_BaihuaVillageSettlementIdSelection", null, null, null, null, new AutoTriggerMonthlyEvent[0]));
		_dataArray.Add(new TaskInfoItem(307, 1434, 1435, 1436, 1437, 1438, 120, new List<int>(), new List<int>(), new List<int>(), isTriggeredTask: true, new List<int>(), 0, 99, 1, new List<short>(), null, null, null, null, null, new AutoTriggerMonthlyEvent[0]));
		_dataArray.Add(new TaskInfoItem(308, 1439, 1440, 1441, 1442, 1443, 120, new List<int>(), new List<int>(), new List<int>(), isTriggeredTask: true, new List<int>(), 0, 99, 1, new List<short>(), "ConchShip_PresetKey_BaihuaLeukoKillsMonthEventSettlementId", null, null, null, null, new AutoTriggerMonthlyEvent[0]));
		_dataArray.Add(new TaskInfoItem(309, 1444, 1445, 1446, 1447, 1448, 120, new List<int>(), new List<int>(), new List<int>(), isTriggeredTask: true, new List<int>(), 0, 99, 1, new List<short>(), "ConchShip_PresetKey_BaihuaLeukoKillsMonthEventSettlementId", null, null, null, null, new AutoTriggerMonthlyEvent[0]));
		_dataArray.Add(new TaskInfoItem(310, 1449, 1450, 1451, 1452, 1453, 120, new List<int>(), new List<int>(), new List<int>(), isTriggeredTask: true, new List<int>(), 0, 99, 1, new List<short>(), "ConchShip_PresetKey_BaihuaMelanoKillsMonthEventSettlementId", null, null, null, null, new AutoTriggerMonthlyEvent[0]));
		_dataArray.Add(new TaskInfoItem(311, 1444, 1454, 1455, 1456, 1457, 120, new List<int>(), new List<int>(), new List<int>(), isTriggeredTask: true, new List<int>(), 0, 99, 1, new List<short>(), "ConchShip_PresetKey_BaihuaMelanoKillsMonthEventSettlementId", null, null, null, null, new AutoTriggerMonthlyEvent[0]));
		_dataArray.Add(new TaskInfoItem(312, 1458, 1459, 1459, 1460, 1461, 120, new List<int>(), new List<int>(), new List<int>(), isTriggeredTask: true, new List<int>(), 0, 99, 1, new List<short>(), null, null, null, null, null, new AutoTriggerMonthlyEvent[0]));
		_dataArray.Add(new TaskInfoItem(313, 1462, 1463, 1464, 1465, 1466, 120, new List<int>(), new List<int>(), new List<int>(), isTriggeredTask: true, new List<int>(), 0, 99, 1, new List<short>(), null, null, null, null, null, new AutoTriggerMonthlyEvent[0]));
		_dataArray.Add(new TaskInfoItem(314, 1467, 1468, 1469, 1470, 1471, 120, new List<int>(), new List<int>(), new List<int>(), isTriggeredTask: true, new List<int>(), 0, 99, 1, new List<short>(), null, null, null, null, null, new AutoTriggerMonthlyEvent[0]));
		_dataArray.Add(new TaskInfoItem(315, 1472, 1473, 1474, 1475, 1476, 120, new List<int>(), new List<int>(), new List<int>(), isTriggeredTask: true, new List<int>(), 0, 99, 1, new List<short>(), null, null, null, null, null, new AutoTriggerMonthlyEvent[0]));
		_dataArray.Add(new TaskInfoItem(316, 1477, 1478, 1479, 1480, 1481, 120, new List<int>(), new List<int>(), new List<int>(), isTriggeredTask: true, new List<int>(), 0, 99, 1, new List<short>(), null, null, null, null, null, new AutoTriggerMonthlyEvent[0]));
		_dataArray.Add(new TaskInfoItem(317, 1482, 1483, 1484, 1485, 1486, 120, new List<int>(), new List<int>(), new List<int>(), isTriggeredTask: true, new List<int>(), 0, 99, 1, new List<short>(), null, null, null, null, null, new AutoTriggerMonthlyEvent[0]));
		_dataArray.Add(new TaskInfoItem(318, 1487, 1488, 1489, 1490, 1491, 120, new List<int>(), new List<int>(), new List<int>(), isTriggeredTask: true, new List<int>(), 0, 99, 1, new List<short>(), null, null, null, null, null, new AutoTriggerMonthlyEvent[0]));
		_dataArray.Add(new TaskInfoItem(319, 1497, 1498, 1499, 1500, 1501, 120, new List<int>(), new List<int>(), new List<int>(), isTriggeredTask: true, new List<int>(), 0, 99, 1, new List<short>(), null, null, null, null, null, new AutoTriggerMonthlyEvent[0]));
		_dataArray.Add(new TaskInfoItem(320, 1502, 1503, 1504, 1505, 1506, 120, new List<int>(), new List<int>(), new List<int>(), isTriggeredTask: true, new List<int>(), 0, 99, 1, new List<short>(), null, null, null, null, null, new AutoTriggerMonthlyEvent[0]));
		_dataArray.Add(new TaskInfoItem(321, 1507, 1508, 1509, 1510, 1511, 120, new List<int>(), new List<int>(), new List<int>(), isTriggeredTask: true, new List<int>(), 0, 99, 1, new List<short>(), null, null, null, null, null, new AutoTriggerMonthlyEvent[0]));
		_dataArray.Add(new TaskInfoItem(322, 1512, 1513, 1514, 1515, 1516, 120, new List<int>(), new List<int>(), new List<int>(), isTriggeredTask: true, new List<int>(), 0, 99, 1, new List<short>(), null, null, null, null, null, new AutoTriggerMonthlyEvent[0]));
		_dataArray.Add(new TaskInfoItem(323, 1517, 1518, 1519, 1520, 1521, 120, new List<int>(), new List<int>(), new List<int>(), isTriggeredTask: true, new List<int>(), 0, 99, 1, new List<short>(), null, null, null, null, null, new AutoTriggerMonthlyEvent[0]));
		_dataArray.Add(new TaskInfoItem(324, 1492, 1493, 1494, 1495, 1496, 120, new List<int>(), new List<int>(), new List<int>(), isTriggeredTask: true, new List<int>(), 0, 99, 1, new List<short>(), null, null, null, null, null, new AutoTriggerMonthlyEvent[0]));
		_dataArray.Add(new TaskInfoItem(325, 1522, 1523, 1524, 1525, 1526, 120, new List<int>(), new List<int>(), new List<int>(), isTriggeredTask: true, new List<int>(), 0, 99, 1, new List<short>(), null, null, null, null, null, new AutoTriggerMonthlyEvent[0]));
		_dataArray.Add(new TaskInfoItem(326, 1527, 1528, 1529, 1530, 1531, 120, new List<int>(), new List<int>(), new List<int>(), isTriggeredTask: true, new List<int>(), 0, 99, 1, new List<short>(), null, null, null, null, null, new AutoTriggerMonthlyEvent[0]));
		_dataArray.Add(new TaskInfoItem(327, 1532, 1533, 1534, 1535, 1536, 120, new List<int>(), new List<int>(), new List<int>(), isTriggeredTask: true, new List<int>(), 0, 99, 1, new List<short>(), null, null, null, null, null, new AutoTriggerMonthlyEvent[0]));
		_dataArray.Add(new TaskInfoItem(328, 1537, 1538, 1539, 1540, 1541, 120, new List<int>(), new List<int>(), new List<int>(), isTriggeredTask: true, new List<int>(), 0, 99, 1, new List<short>(), null, null, null, null, null, new AutoTriggerMonthlyEvent[0]));
		_dataArray.Add(new TaskInfoItem(329, 1542, 1543, 1544, 1545, 1546, 120, new List<int>(), new List<int>(), new List<int>(), isTriggeredTask: true, new List<int>(), 0, 99, 1, new List<short>(), null, null, null, null, null, new AutoTriggerMonthlyEvent[0]));
		_dataArray.Add(new TaskInfoItem(330, 1547, 1548, 1549, 1550, 1551, 120, new List<int>(), new List<int>(), new List<int>(), isTriggeredTask: true, new List<int>(), 0, 99, 1, new List<short>(), "ConchShip_PresetKey_FulongAdventureOneCountDown", null, null, null, null, new AutoTriggerMonthlyEvent[0]));
		_dataArray.Add(new TaskInfoItem(331, 1552, 1553, 1554, 1555, 1556, 120, new List<int>(), new List<int>(), new List<int>(), isTriggeredTask: true, new List<int>(), 0, 99, 1, new List<short>(), null, null, null, null, null, new AutoTriggerMonthlyEvent[0]));
		_dataArray.Add(new TaskInfoItem(332, 1557, 1558, 1559, 1560, 1561, 120, new List<int>(), new List<int>(), new List<int>(), isTriggeredTask: true, new List<int>(), 0, 99, 1, new List<short>(), null, null, null, null, null, new AutoTriggerMonthlyEvent[0]));
		_dataArray.Add(new TaskInfoItem(333, 1562, 1563, 1564, 1565, 1566, 120, new List<int>(), new List<int>(), new List<int>(), isTriggeredTask: true, new List<int>(), 0, 99, 1, new List<short>(), null, null, null, null, null, new AutoTriggerMonthlyEvent[0]));
		_dataArray.Add(new TaskInfoItem(334, 1567, 1568, 1569, 1570, 1571, 120, new List<int>(), new List<int>(), new List<int>(), isTriggeredTask: true, new List<int>(), 0, 99, 1, new List<short>(), null, null, null, null, null, new AutoTriggerMonthlyEvent[0]));
		_dataArray.Add(new TaskInfoItem(335, 1572, 1573, 1574, 1575, 1576, 120, new List<int>(), new List<int>(), new List<int>(), isTriggeredTask: true, new List<int>(), 0, 99, 1, new List<short>(), null, null, null, null, null, new AutoTriggerMonthlyEvent[0]));
		_dataArray.Add(new TaskInfoItem(336, 1577, 1578, 1579, 1580, 1581, 120, new List<int>(), new List<int>(), new List<int>(), isTriggeredTask: true, new List<int>(), 0, 99, 1, new List<short>(), null, null, null, null, null, new AutoTriggerMonthlyEvent[0]));
		_dataArray.Add(new TaskInfoItem(337, 1582, 1583, 1584, 1585, 1586, 120, new List<int>(), new List<int>(), new List<int>(), isTriggeredTask: true, new List<int>(), 0, 99, 1, new List<short>(), null, null, null, null, null, new AutoTriggerMonthlyEvent[0]));
		_dataArray.Add(new TaskInfoItem(338, 1587, 1588, 1589, 1590, 1591, 120, new List<int>(), new List<int>(), new List<int>(), isTriggeredTask: true, new List<int>(), 0, 99, 1, new List<short>(), null, null, null, null, new string[1] { "ConchShip_PresetKey_FulongChickenFeatherLackCount" }, new AutoTriggerMonthlyEvent[0]));
		_dataArray.Add(new TaskInfoItem(339, 1592, 1593, 1594, 1595, 1596, 120, new List<int>(), new List<int>(), new List<int>(), isTriggeredTask: true, new List<int>(), 0, 99, 1, new List<short>(), null, null, null, null, null, new AutoTriggerMonthlyEvent[0]));
		_dataArray.Add(new TaskInfoItem(340, 1597, 1598, 1599, 1600, 1601, 120, new List<int>(), new List<int>(), new List<int>(), isTriggeredTask: true, new List<int>(), 0, 99, 1, new List<short>(), "ConchShip_PresetKey_FulongAdventureThreeCountDown", null, null, null, null, new AutoTriggerMonthlyEvent[0]));
		_dataArray.Add(new TaskInfoItem(341, 1602, 1603, 1604, 1605, 1606, 120, new List<int>(), new List<int>(), new List<int>(), isTriggeredTask: true, new List<int>(), 0, 99, 1, new List<short>(), null, null, null, null, null, new AutoTriggerMonthlyEvent[0]));
		_dataArray.Add(new TaskInfoItem(342, 1607, 1608, 1609, 1610, 1611, 120, new List<int>(), new List<int>(), new List<int>(), isTriggeredTask: true, new List<int>(), 0, 99, 2, new List<short>(), null, null, null, null, null, new AutoTriggerMonthlyEvent[0]));
		_dataArray.Add(new TaskInfoItem(343, 1612, 1613, 1614, 1615, 1616, 120, new List<int>(), new List<int>(), new List<int>(), isTriggeredTask: true, new List<int>(), 0, 99, 1, new List<short>(), null, null, null, null, null, new AutoTriggerMonthlyEvent[0]));
		_dataArray.Add(new TaskInfoItem(344, 1617, 1618, 1619, 1620, 1621, 120, new List<int>(), new List<int>(), new List<int>(), isTriggeredTask: true, new List<int>(), 0, 99, 1, new List<short>(), null, null, null, null, null, new AutoTriggerMonthlyEvent[0]));
		_dataArray.Add(new TaskInfoItem(345, 1622, 1623, 1624, 1625, 1626, 120, new List<int>(), new List<int>(), new List<int>(), isTriggeredTask: true, new List<int>(), 0, 99, 1, new List<short>(), null, null, null, null, null, new AutoTriggerMonthlyEvent[0]));
		_dataArray.Add(new TaskInfoItem(346, 1627, 1628, 1629, 1630, 1631, 120, new List<int>(), new List<int>(), new List<int>(), isTriggeredTask: true, new List<int>(), 0, 99, 1, new List<short>(), null, null, null, null, null, new AutoTriggerMonthlyEvent[0]));
		_dataArray.Add(new TaskInfoItem(347, 1632, 1633, 1634, 1635, 1636, 120, new List<int>(), new List<int>(), new List<int>(), isTriggeredTask: true, new List<int>(), 0, 99, 1, new List<short>(), null, null, null, null, null, new AutoTriggerMonthlyEvent[0]));
		_dataArray.Add(new TaskInfoItem(348, 1637, 666, 1638, 1639, 1640, 120, new List<int>(), new List<int>(), new List<int>(), isTriggeredTask: true, new List<int>(), 0, 99, 1, new List<short>(), "ConchShip_PresetKey_ZhanluLocationSecond", null, null, null, null, new AutoTriggerMonthlyEvent[0]));
		_dataArray.Add(new TaskInfoItem(349, 1641, 1642, 1643, 1644, 1645, 120, new List<int>(), new List<int>(), new List<int>(), isTriggeredTask: true, new List<int>(), 0, 99, 1, new List<short>(), null, null, null, null, null, new AutoTriggerMonthlyEvent[0]));
		_dataArray.Add(new TaskInfoItem(350, 1646, 1647, 1648, 1649, 1650, 120, new List<int>(), new List<int>(), new List<int>(), isTriggeredTask: true, new List<int>(), 0, 99, 1, new List<short>(), null, null, null, null, null, new AutoTriggerMonthlyEvent[0]));
		_dataArray.Add(new TaskInfoItem(351, 1651, 627, 1652, 1653, 1654, 120, new List<int>(), new List<int>(), new List<int>(), isTriggeredTask: true, new List<int>(), 0, 99, 1, new List<short>(), null, null, null, null, null, new AutoTriggerMonthlyEvent[0]));
		_dataArray.Add(new TaskInfoItem(352, 1655, 1656, 1657, 1658, 1659, 120, new List<int>(), new List<int>(), new List<int>(), isTriggeredTask: true, new List<int>(), 0, 99, 1, new List<short>(), null, null, null, null, null, new AutoTriggerMonthlyEvent[0]));
		_dataArray.Add(new TaskInfoItem(353, 1660, 1661, 1662, 1663, 1664, 120, new List<int>(), new List<int>(), new List<int>(), isTriggeredTask: true, new List<int>(), 0, 99, 1, new List<short>(), null, null, null, null, null, new AutoTriggerMonthlyEvent[0]));
		_dataArray.Add(new TaskInfoItem(354, 1665, 1666, 1667, 1668, 1669, 120, new List<int>(), new List<int>(), new List<int>(), isTriggeredTask: true, new List<int>(), 0, 99, 1, new List<short>(), null, null, null, null, null, new AutoTriggerMonthlyEvent[0]));
		_dataArray.Add(new TaskInfoItem(355, 1670, 1671, 1672, 1673, 1674, 120, new List<int>(), new List<int>(), new List<int>(), isTriggeredTask: true, new List<int>(), 0, 99, 1, new List<short>(), null, null, null, null, null, new AutoTriggerMonthlyEvent[0]));
		_dataArray.Add(new TaskInfoItem(356, 1660, 1675, 1676, 1663, 1677, 120, new List<int>(), new List<int>(), new List<int>(), isTriggeredTask: true, new List<int>(), 0, 99, 1, new List<short>(), null, null, null, null, null, new AutoTriggerMonthlyEvent[0]));
		_dataArray.Add(new TaskInfoItem(357, 1678, 1679, 1680, 1681, 1682, 120, new List<int>(), new List<int>(), new List<int>(), isTriggeredTask: true, new List<int>(), 0, 99, 1, new List<short>(), null, null, null, null, null, new AutoTriggerMonthlyEvent[0]));
		_dataArray.Add(new TaskInfoItem(358, 1683, 627, 1684, 1685, 1686, 120, new List<int>(), new List<int>(), new List<int>(), isTriggeredTask: true, new List<int>(), 0, 99, 1, new List<short>(), null, null, null, null, null, new AutoTriggerMonthlyEvent[0]));
		_dataArray.Add(new TaskInfoItem(359, 1687, 1688, 1689, 1690, 1691, 120, new List<int>(), new List<int>(), new List<int>(), isTriggeredTask: true, new List<int>(), 0, 99, 1, new List<short>(), null, null, null, null, null, new AutoTriggerMonthlyEvent[0]));
	}

	private void CreateItems6()
	{
		_dataArray.Add(new TaskInfoItem(360, 1660, 1692, 1693, 1694, 1695, 120, new List<int>(), new List<int>(), new List<int>(), isTriggeredTask: true, new List<int>(), 0, 99, 1, new List<short>(), null, null, null, null, null, new AutoTriggerMonthlyEvent[0]));
		_dataArray.Add(new TaskInfoItem(361, 1696, 1697, 1698, 1699, 1700, 120, new List<int>(), new List<int>(), new List<int>(), isTriggeredTask: true, new List<int>(), 0, 99, 1, new List<short>(), null, null, null, null, null, new AutoTriggerMonthlyEvent[0]));
		_dataArray.Add(new TaskInfoItem(362, 1701, 1702, 1703, 1704, 1705, 120, new List<int>(), new List<int>(), new List<int>(), isTriggeredTask: true, new List<int>(), 0, 99, 1, new List<short>(), null, null, null, null, null, new AutoTriggerMonthlyEvent[0]));
		_dataArray.Add(new TaskInfoItem(363, 1706, 1707, 1708, 1709, 1710, 120, new List<int>(), new List<int>(), new List<int>(), isTriggeredTask: true, new List<int>(), 0, 99, 1, new List<short>(), null, null, null, null, null, new AutoTriggerMonthlyEvent[0]));
		_dataArray.Add(new TaskInfoItem(364, 1711, 1712, 1713, 1714, 1715, 120, new List<int>(), new List<int>(), new List<int>(), isTriggeredTask: true, new List<int>(), 0, 99, 1, new List<short>(), null, null, null, null, null, new AutoTriggerMonthlyEvent[0]));
		_dataArray.Add(new TaskInfoItem(365, 1716, 1717, 1718, 1719, 1720, 120, new List<int>(), new List<int>(), new List<int>(), isTriggeredTask: true, new List<int>(), 0, 99, 1, new List<short>(), null, null, null, null, null, new AutoTriggerMonthlyEvent[0]));
		_dataArray.Add(new TaskInfoItem(366, 1721, 1722, 1723, 1724, 1725, 120, new List<int>(), new List<int>(), new List<int>(), isTriggeredTask: true, new List<int>(), 0, 99, 1, new List<short>(), null, null, null, null, null, new AutoTriggerMonthlyEvent[0]));
		_dataArray.Add(new TaskInfoItem(367, 1726, 1727, 1728, 1729, 1730, 120, new List<int>(), new List<int>(), new List<int>(), isTriggeredTask: true, new List<int>(), 0, 99, 1, new List<short>(), null, null, null, null, null, new AutoTriggerMonthlyEvent[0]));
		_dataArray.Add(new TaskInfoItem(368, 1731, 1732, 1733, 1734, 1735, 120, new List<int>(), new List<int>(), new List<int>(), isTriggeredTask: true, new List<int>(), 0, 99, 1, new List<short>(), null, null, null, null, null, new AutoTriggerMonthlyEvent[0]));
		_dataArray.Add(new TaskInfoItem(369, 1736, 1737, 1738, 1739, 1740, 120, new List<int>(), new List<int>(), new List<int>(), isTriggeredTask: true, new List<int>(), 0, 99, 1, new List<short>(), null, null, null, null, null, new AutoTriggerMonthlyEvent[0]));
		_dataArray.Add(new TaskInfoItem(370, 1741, 1742, 1743, 1744, 1745, 120, new List<int>(), new List<int>(), new List<int>(), isTriggeredTask: true, new List<int>(), 0, 99, 1, new List<short>(), null, null, null, null, null, new AutoTriggerMonthlyEvent[0]));
		_dataArray.Add(new TaskInfoItem(371, 1746, 1747, 1748, 1749, 1750, 120, new List<int>(), new List<int>(), new List<int>(), isTriggeredTask: true, new List<int>(), 0, 99, 1, new List<short>(), null, null, null, null, null, new AutoTriggerMonthlyEvent[0]));
		_dataArray.Add(new TaskInfoItem(372, 1751, 1752, 1753, 1749, 1754, 120, new List<int>(), new List<int>(), new List<int>(), isTriggeredTask: true, new List<int>(), 0, 99, 1, new List<short>(), null, null, null, null, null, new AutoTriggerMonthlyEvent[0]));
		_dataArray.Add(new TaskInfoItem(373, 1755, 1756, 1757, 1749, 1758, 120, new List<int>(), new List<int>(), new List<int>(), isTriggeredTask: true, new List<int>(), 0, 99, 1, new List<short>(), null, null, null, null, null, new AutoTriggerMonthlyEvent[0]));
		_dataArray.Add(new TaskInfoItem(374, 1759, 1760, 1761, 1762, 1763, 120, new List<int>(), new List<int>(), new List<int>(), isTriggeredTask: true, new List<int>(), 0, 99, 1, new List<short>(), null, null, null, null, null, new AutoTriggerMonthlyEvent[0]));
		_dataArray.Add(new TaskInfoItem(375, 923, 1764, 923, 1765, 1766, 120, new List<int>(), new List<int>(), new List<int>(), isTriggeredTask: true, new List<int>(), 0, 99, 1, new List<short>(), null, null, null, null, null, new AutoTriggerMonthlyEvent[0]));
		_dataArray.Add(new TaskInfoItem(376, 1767, 1764, 1767, 1768, 1769, 120, new List<int>(), new List<int>(), new List<int>(), isTriggeredTask: true, new List<int>(), 0, 99, 1, new List<short>(), null, null, null, null, null, new AutoTriggerMonthlyEvent[0]));
		_dataArray.Add(new TaskInfoItem(377, 1770, 1764, 1770, 1771, 1772, 120, new List<int>(), new List<int>(), new List<int>(), isTriggeredTask: true, new List<int>(), 0, 99, 1, new List<short>(), null, null, null, null, null, new AutoTriggerMonthlyEvent[0]));
		_dataArray.Add(new TaskInfoItem(378, 1773, 1774, 1773, 1775, 1776, 120, new List<int>(), new List<int>(), new List<int>(), isTriggeredTask: true, new List<int>(), 0, 99, 1, new List<short>(), null, null, null, null, null, new AutoTriggerMonthlyEvent[0]));
		_dataArray.Add(new TaskInfoItem(379, 1777, 1777, 1777, 1778, 1779, 120, new List<int>(), new List<int>(), new List<int>(), isTriggeredTask: true, new List<int>(), 0, 99, 1, new List<short>(), null, null, null, null, null, new AutoTriggerMonthlyEvent[0]));
		_dataArray.Add(new TaskInfoItem(380, 1780, 1780, 1780, 1781, 1782, 120, new List<int>(), new List<int>(), new List<int>(), isTriggeredTask: true, new List<int>(), 0, 99, 1, new List<short>(), null, null, null, null, null, new AutoTriggerMonthlyEvent[0]));
		_dataArray.Add(new TaskInfoItem(381, 1783, 1783, 1783, 1784, 1785, 120, new List<int>(), new List<int>(), new List<int>(), isTriggeredTask: true, new List<int>(), 0, 99, 1, new List<short>(), null, null, null, null, null, new AutoTriggerMonthlyEvent[0]));
		_dataArray.Add(new TaskInfoItem(382, 1786, 1787, 1786, 1788, 1789, 120, new List<int>(), new List<int>(), new List<int>(), isTriggeredTask: true, new List<int>(), 0, 99, 1, new List<short>(), null, null, null, null, null, new AutoTriggerMonthlyEvent[0]));
		_dataArray.Add(new TaskInfoItem(383, 1790, 1790, 1790, 1791, 1792, 120, new List<int>(), new List<int>(), new List<int>(), isTriggeredTask: true, new List<int>(), 0, 99, 1, new List<short>(), null, null, null, null, null, new AutoTriggerMonthlyEvent[1]
		{
			new AutoTriggerMonthlyEvent(372)
		}));
		_dataArray.Add(new TaskInfoItem(384, 1793, 1794, 1793, 1795, 1796, 120, new List<int>(), new List<int>(), new List<int>(), isTriggeredTask: true, new List<int>(), 0, 99, 1, new List<short>(), null, null, null, null, null, new AutoTriggerMonthlyEvent[0]));
		_dataArray.Add(new TaskInfoItem(385, 1797, 1798, 1797, 1799, 1800, 120, new List<int>(), new List<int>(), new List<int>(), isTriggeredTask: true, new List<int>(), 0, 99, 1, new List<short>(), null, null, null, null, null, new AutoTriggerMonthlyEvent[0]));
		_dataArray.Add(new TaskInfoItem(386, 782, 1801, 782, 1802, 1803, 120, new List<int>(), new List<int>(), new List<int>(), isTriggeredTask: true, new List<int>(), 0, 99, 1, new List<short>(), null, null, null, null, null, new AutoTriggerMonthlyEvent[1]
		{
			new AutoTriggerMonthlyEvent(373)
		}));
		_dataArray.Add(new TaskInfoItem(387, 1804, 1801, 1804, 1805, 1806, 120, new List<int>(), new List<int>(), new List<int>(), isTriggeredTask: true, new List<int>(), 0, 99, 1, new List<short>(), null, null, null, null, null, new AutoTriggerMonthlyEvent[2]
		{
			new AutoTriggerMonthlyEvent(374),
			new AutoTriggerMonthlyEvent(375)
		}));
		_dataArray.Add(new TaskInfoItem(388, 1807, 1794, 1807, 1808, 1809, 120, new List<int>(), new List<int>(), new List<int>(), isTriggeredTask: true, new List<int>(), 0, 99, 1, new List<short>(), null, null, null, null, null, new AutoTriggerMonthlyEvent[0]));
		_dataArray.Add(new TaskInfoItem(389, 1810, 1774, 1810, 1811, 1812, 120, new List<int>(), new List<int>(), new List<int>(), isTriggeredTask: true, new List<int>(), 0, 99, 1, new List<short>(), null, null, null, null, null, new AutoTriggerMonthlyEvent[0]));
		_dataArray.Add(new TaskInfoItem(390, 1813, 1801, 1813, 1814, 1815, 120, new List<int>(), new List<int>(), new List<int>(), isTriggeredTask: true, new List<int>(), 0, 99, 1, new List<short>(), null, null, null, null, null, new AutoTriggerMonthlyEvent[1]
		{
			new AutoTriggerMonthlyEvent(376)
		}));
		_dataArray.Add(new TaskInfoItem(391, 1816, 1801, 1816, 1817, 1818, 120, new List<int>(), new List<int>(), new List<int>(), isTriggeredTask: true, new List<int>(), 0, 99, 1, new List<short>(), null, null, null, null, null, new AutoTriggerMonthlyEvent[2]
		{
			new AutoTriggerMonthlyEvent(377),
			new AutoTriggerMonthlyEvent(378)
		}));
		_dataArray.Add(new TaskInfoItem(392, 1819, 1820, 1821, 1822, 1823, 120, new List<int>(), new List<int>(), new List<int>(), isTriggeredTask: true, new List<int>(), 0, 99, 1, new List<short>(), null, null, null, null, null, new AutoTriggerMonthlyEvent[0]));
		_dataArray.Add(new TaskInfoItem(393, 1824, 1825, 1826, 1827, 1828, 120, new List<int>(), new List<int>(), new List<int>(), isTriggeredTask: true, new List<int>(), 0, 99, 1, new List<short>(), null, null, null, null, null, new AutoTriggerMonthlyEvent[0]));
		_dataArray.Add(new TaskInfoItem(394, 1829, 666, 1830, 1831, 1832, 120, new List<int>(), new List<int>(), new List<int>(), isTriggeredTask: true, new List<int>(), 0, 99, 1, new List<short>(), "VillageLocation", null, null, null, null, new AutoTriggerMonthlyEvent[0]));
		_dataArray.Add(new TaskInfoItem(395, 1833, 1834, 1835, 1836, 1837, 120, new List<int>(), new List<int>(), new List<int>(), isTriggeredTask: true, new List<int>(), 0, 99, 1, new List<short>(), "VillageLocation", null, null, null, null, new AutoTriggerMonthlyEvent[0]));
		_dataArray.Add(new TaskInfoItem(396, 1838, 666, 1830, 1839, 1840, 120, new List<int>(), new List<int>(), new List<int>(), isTriggeredTask: true, new List<int>(), 0, 99, 1, new List<short>(), "StockadeLocation", null, null, null, null, new AutoTriggerMonthlyEvent[0]));
		_dataArray.Add(new TaskInfoItem(397, 1841, 1842, 1843, 1844, 1845, 120, new List<int>(), new List<int>(), new List<int>(), isTriggeredTask: true, new List<int>(), 0, 99, 1, new List<short>(), "StockadeLocation", null, null, null, null, new AutoTriggerMonthlyEvent[0]));
		_dataArray.Add(new TaskInfoItem(398, 1833, 1846, 1835, 1836, 1847, 120, new List<int>(), new List<int>(), new List<int>(), isTriggeredTask: true, new List<int>(), 0, 99, 1, new List<short>(), "StockadeLocation", null, null, null, null, new AutoTriggerMonthlyEvent[0]));
		_dataArray.Add(new TaskInfoItem(399, 1848, 666, 1830, 1849, 1850, 120, new List<int>(), new List<int>(), new List<int>(), isTriggeredTask: true, new List<int>(), 0, 99, 1, new List<short>(), "TownLocation", null, null, null, null, new AutoTriggerMonthlyEvent[0]));
		_dataArray.Add(new TaskInfoItem(400, 1833, 1851, 1835, 1836, 1852, 120, new List<int>(), new List<int>(), new List<int>(), isTriggeredTask: true, new List<int>(), 0, 99, 1, new List<short>(), "TownLocation", null, null, null, null, new AutoTriggerMonthlyEvent[0]));
		_dataArray.Add(new TaskInfoItem(401, 1853, 1854, 1855, 1856, 1857, 120, new List<int>(), new List<int>(), new List<int>(), isTriggeredTask: true, new List<int>(), 0, 99, 1, new List<short>(), null, null, null, null, null, new AutoTriggerMonthlyEvent[0]));
		_dataArray.Add(new TaskInfoItem(402, 1858, 1859, 1860, 1861, 1862, 120, new List<int>(), new List<int>(), new List<int>(), isTriggeredTask: true, new List<int>(), 0, 99, 1, new List<short>(), null, null, null, null, null, new AutoTriggerMonthlyEvent[1]
		{
			new AutoTriggerMonthlyEvent(379, "RoleTaiwu")
		}));
		_dataArray.Add(new TaskInfoItem(403, 1863, 1864, 1865, 1866, 1867, 120, new List<int>(), new List<int>(), new List<int>(), isTriggeredTask: true, new List<int>(), 0, 99, 1, new List<short>(), null, null, null, null, null, new AutoTriggerMonthlyEvent[0]));
		_dataArray.Add(new TaskInfoItem(404, 1816, 1868, 1869, 1870, 1871, 120, new List<int>(), new List<int>(), new List<int>(), isTriggeredTask: true, new List<int>(), 0, 99, 1, new List<short>(), null, null, null, null, null, new AutoTriggerMonthlyEvent[1]
		{
			new AutoTriggerMonthlyEvent(380)
		}));
		_dataArray.Add(new TaskInfoItem(405, 1875, 1876, 1877, 1878, 1879, 120, new List<int>(), new List<int>(), new List<int>(), isTriggeredTask: true, new List<int>(), 0, 99, 2, new List<short>(), null, null, null, null, null, new AutoTriggerMonthlyEvent[0]));
		_dataArray.Add(new TaskInfoItem(406, 1872, 627, 1873, 1870, 1874, 120, new List<int>(), new List<int>(), new List<int>(), isTriggeredTask: true, new List<int>(), 0, 99, 1, new List<short>(), null, null, null, null, null, new AutoTriggerMonthlyEvent[1]
		{
			new AutoTriggerMonthlyEvent(383)
		}));
	}

	public override void Init()
	{
		base.Init();
		_dataArray = new List<TaskInfoItem>(407);
		CreateItems0();
		CreateItems1();
		CreateItems2();
		CreateItems3();
		CreateItems4();
		CreateItems5();
		CreateItems6();
	}
}
