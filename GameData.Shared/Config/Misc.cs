using System;
using System.Collections.Generic;
using Config.Common;
using GameData.Domains.Extra;

namespace Config;

[Serializable]
public class Misc : ConfigData<MiscItem, short>
{
	public static class DefKey
	{
		public const short BloodDew0 = 0;

		public const short BloodDew1 = 1;

		public const short BloodDew2 = 2;

		public const short BloodDew3 = 3;

		public const short BloodDew4 = 4;

		public const short BloodDew5 = 5;

		public const short BloodDew6 = 6;

		public const short BloodDew7 = 7;

		public const short BloodDew8 = 8;

		public const short SweepNet = 9;

		public const short Teleogryllus = 10;

		public const short LongHornedGrasshopper = 11;

		public const short Grasshopper = 12;

		public const short DrumwingedKatydid = 13;

		public const short GoldenPipa = 14;

		public const short BambooSandfly = 15;

		public const short SmallYellowSandfly = 16;

		public const short PearGreenSandfly = 17;

		public const short FlowerSandfly = 18;

		public const short BlackSandfly = 19;

		public const short BrayingSandfly = 20;

		public const short BigYellowSandfly = 21;

		public const short GrassYellowSandfly = 22;

		public const short KitchenSandfly = 23;

		public const short MirrorSandfly = 24;

		public const short TripleTail = 25;

		public const short GoldenSandfly = 26;

		public const short Katydid = 27;

		public const short WesternPresentMonv0 = 28;

		public const short WesternPresentMonv1 = 29;

		public const short WesternPresentMonv2 = 30;

		public const short WesternPresentMonv3 = 31;

		public const short WesternPresentMonv4 = 32;

		public const short WesternPresentDayueYaochang0 = 33;

		public const short WesternPresentDayueYaochang1 = 34;

		public const short WesternPresentDayueYaochang2 = 35;

		public const short WesternPresentDayueYaochang3 = 36;

		public const short WesternPresentDayueYaochang4 = 37;

		public const short WesternPresentJiuhan0 = 38;

		public const short WesternPresentJiuhan1 = 39;

		public const short WesternPresentJiuhan2 = 40;

		public const short WesternPresentJiuhan3 = 41;

		public const short WesternPresentJiuhan4 = 42;

		public const short WesternPresentJinHuanger0 = 43;

		public const short WesternPresentJinHuanger1 = 44;

		public const short WesternPresentJinHuanger2 = 45;

		public const short WesternPresentJinHuanger3 = 46;

		public const short WesternPresentJinHuanger4 = 47;

		public const short WesternPresentYiYihou0 = 48;

		public const short WesternPresentYiYihou1 = 49;

		public const short WesternPresentYiYihou2 = 50;

		public const short WesternPresentYiYihou3 = 51;

		public const short WesternPresentYiYihou4 = 52;

		public const short WesternPresentWeiQi0 = 53;

		public const short WesternPresentWeiQi1 = 54;

		public const short WesternPresentWeiQi2 = 55;

		public const short WesternPresentWeiQi3 = 56;

		public const short WesternPresentWeiQi4 = 57;

		public const short WesternPresentYixiang0 = 58;

		public const short WesternPresentYixiang1 = 59;

		public const short WesternPresentYixiang2 = 60;

		public const short WesternPresentYixiang3 = 61;

		public const short WesternPresentYixiang4 = 62;

		public const short WesternPresentXuefeng0 = 63;

		public const short WesternPresentXuefeng1 = 64;

		public const short WesternPresentXuefeng2 = 65;

		public const short WesternPresentXuefeng3 = 66;

		public const short WesternPresentXuefeng4 = 67;

		public const short WesternPresentShuFang0 = 68;

		public const short WesternPresentShuFang1 = 69;

		public const short WesternPresentShuFang2 = 70;

		public const short WesternPresentShuFang3 = 71;

		public const short WesternPresentShuFang4 = 72;

		public const short Rope0 = 73;

		public const short Rope1 = 74;

		public const short Rope2 = 75;

		public const short Rope3 = 76;

		public const short Rope4 = 77;

		public const short Rope5 = 78;

		public const short Rope6 = 79;

		public const short Rope7 = 80;

		public const short Rope8 = 81;

		public const short Jar0 = 82;

		public const short Jar1 = 83;

		public const short Jar2 = 84;

		public const short Jar3 = 85;

		public const short Jar4 = 86;

		public const short Jar5 = 87;

		public const short Jar6 = 88;

		public const short Jar7 = 89;

		public const short Jar8 = 90;

		public const short TestingNeedle = 91;

		public const short MaterialKeyConstruct0 = 92;

		public const short MaterialKeyConstruct1 = 93;

		public const short MaterialKeyConstruct2 = 94;

		public const short MaterialKeyConstruct3 = 95;

		public const short MaterialKeyConstruct4 = 96;

		public const short MaterialKeyConstruct5 = 97;

		public const short MaterialKeyConstruct6 = 98;

		public const short MaterialKeyConstruct7 = 99;

		public const short MaterialKeyConstruct8 = 100;

		public const short MaterialKeyConstruct9 = 101;

		public const short CombatSkillKeyConstruct0 = 102;

		public const short CombatSkillKeyConstruct1 = 103;

		public const short CombatSkillKeyConstruct2 = 104;

		public const short CombatSkillKeyConstruct3 = 105;

		public const short CombatSkillKeyConstruct4 = 106;

		public const short CombatSkillKeyConstruct5 = 107;

		public const short CombatSkillKeyConstruct6 = 108;

		public const short CombatSkillKeyConstruct7 = 109;

		public const short CombatSkillKeyConstruct8 = 110;

		public const short CombatSkillKeyConstruct9 = 111;

		public const short CombatSkillKeyConstruct10 = 112;

		public const short CombatSkillKeyConstruct11 = 113;

		public const short CombatSkillKeyConstruct12 = 114;

		public const short CombatSkillKeyConstruct13 = 115;

		public const short CombatSkillKeyConstruct14 = 116;

		public const short CombatSkillKeyConstruct15 = 117;

		public const short MusicKeyConstruct0 = 118;

		public const short MusicKeyConstruct1 = 119;

		public const short MusicKeyConstruct2 = 120;

		public const short MusicKeyConstruct3 = 121;

		public const short ChessKeyConstruct0 = 122;

		public const short ChessKeyConstruct1 = 123;

		public const short ChessKeyConstruct2 = 124;

		public const short ChessKeyConstruct3 = 125;

		public const short PoemKeyConstruct0 = 126;

		public const short PoemKeyConstruct1 = 127;

		public const short PoemKeyConstruct2 = 128;

		public const short PoemKeyConstruct3 = 129;

		public const short PaintingKeyConstruct0 = 130;

		public const short PaintingKeyConstruct1 = 131;

		public const short PaintingKeyConstruct2 = 132;

		public const short PaintingKeyConstruct3 = 133;

		public const short MathKeyConstruct0 = 134;

		public const short MathKeyConstruct1 = 135;

		public const short MathKeyConstruct2 = 136;

		public const short MathKeyConstruct3 = 137;

		public const short AppraisalKeyConstruct0 = 138;

		public const short AppraisalKeyConstruct1 = 139;

		public const short AppraisalKeyConstruct2 = 140;

		public const short AppraisalKeyConstruct3 = 141;

		public const short AppraisalKeyConstruct4 = 142;

		public const short ForgingKeyConstruct0 = 143;

		public const short ForgingKeyConstruct1 = 144;

		public const short ForgingKeyConstruct2 = 145;

		public const short ForgingKeyConstruct3 = 146;

		public const short ForgingKeyConstruct4 = 147;

		public const short ForgingKeyConstruct5 = 148;

		public const short WoodworkingKeyConstruct0 = 149;

		public const short WoodworkingKeyConstruct1 = 150;

		public const short WoodworkingKeyConstruct2 = 151;

		public const short WoodworkingKeyConstruct3 = 152;

		public const short WoodworkingKeyConstruct4 = 153;

		public const short WoodworkingKeyConstruct5 = 154;

		public const short MedicineKeyConstruct0 = 155;

		public const short MedicineKeyConstruct1 = 156;

		public const short MedicineKeyConstruct2 = 157;

		public const short MedicineKeyConstruct3 = 158;

		public const short MedicineKeyConstruct4 = 159;

		public const short MedicineKeyConstruct5 = 160;

		public const short ToxicologyKeyConstruct0 = 161;

		public const short ToxicologyKeyConstruct1 = 162;

		public const short ToxicologyKeyConstruct2 = 163;

		public const short ToxicologyKeyConstruct3 = 164;

		public const short ToxicologyKeyConstruct4 = 165;

		public const short ToxicologyKeyConstruct5 = 166;

		public const short WeavingKeyConstruct0 = 167;

		public const short WeavingKeyConstruct1 = 168;

		public const short WeavingKeyConstruct2 = 169;

		public const short WeavingKeyConstruct3 = 170;

		public const short WeavingKeyConstruct4 = 171;

		public const short WeavingKeyConstruct5 = 172;

		public const short JadeKeyConstruct0 = 173;

		public const short JadeKeyConstruct1 = 174;

		public const short JadeKeyConstruct2 = 175;

		public const short JadeKeyConstruct3 = 176;

		public const short JadeKeyConstruct4 = 177;

		public const short JadeKeyConstruct5 = 178;

		public const short TaoismKeyConstruct0 = 179;

		public const short TaoismKeyConstruct1 = 180;

		public const short TaoismKeyConstruct2 = 181;

		public const short TaoismKeyConstruct3 = 182;

		public const short BuddhismKeyConstruct0 = 183;

		public const short BuddhismKeyConstruct1 = 184;

		public const short BuddhismKeyConstruct2 = 185;

		public const short BuddhismKeyConstruct3 = 186;

		public const short CookingKeyConstruct0 = 187;

		public const short CookingKeyConstruct1 = 188;

		public const short CookingKeyConstruct2 = 189;

		public const short CookingKeyConstruct3 = 190;

		public const short CookingKeyConstruct4 = 191;

		public const short CookingKeyConstruct5 = 192;

		public const short EclecticKeyConstruct0 = 193;

		public const short EclecticKeyConstruct1 = 194;

		public const short EclecticKeyConstruct2 = 195;

		public const short EclecticKeyConstruct3 = 196;

		public const short TaiwuGenealogy = 197;

		public const short WesternRegionsMap = 198;

		public const short WheelOfKarma = 199;

		public const short SwordFragmentsStart = 200;

		public const short SwordFragments1 = 201;

		public const short SwordFragments2 = 202;

		public const short SwordFragments3 = 203;

		public const short SwordFragments4 = 204;

		public const short SwordFragments5 = 205;

		public const short SwordFragments6 = 206;

		public const short SwordFragments7 = 207;

		public const short SwordFragments8 = 208;

		public const short SwordFragmentsEnd = 209;

		public const short FuyuSwordGrip = 210;

		public const short LegacyBookNeigong = 211;

		public const short LegacyBookPosing = 212;

		public const short LegacyBookStunt = 213;

		public const short LegacyBookFistAndPalm = 214;

		public const short LegacyBookFinger = 215;

		public const short LegacyBookLeg = 216;

		public const short LegacyBookThrow = 217;

		public const short LegacyBookSword = 218;

		public const short LegacyBookBlade = 219;

		public const short LegacyBookPolearm = 220;

		public const short LegacyBookSpecial = 221;

		public const short LegacyBookWhip = 222;

		public const short LegacyBookControllableShot = 223;

		public const short LegacyBookCombatMusic = 224;

		public const short WuYing = 225;

		public const short HuanxinBloodyWutong = 226;

		public const short TwelveBambooThorn = 227;

		public const short RemainsOfXiangong = 228;

		public const short NiceLittleBoat = 229;

		public const short FuyuSwordFurnace = 230;

		public const short SevenCutBlackBamboo = 231;

		public const short LuggageOfHuanxin = 232;

		public const short SevenColorIronPlate = 233;

		public const short TianJieFuLu = 234;

		public const short LifeSkillClassic = 235;

		public const short CombatSkillClassic = 236;

		public const short BasketOnBack = 237;

		public const short HuntingBow = 238;

		public const short Hammer = 239;

		public const short BrokenBowl = 240;

		public const short GrainSac = 241;

		public const short CrippledShoes = 242;

		public const short AntiqueJadeBat = 243;

		public const short AntiqueJadeFox = 244;

		public const short AntiqueJadeButterfly = 245;

		public const short AntiqueBell = 246;

		public const short MuddyStatue = 247;

		public const short SectMainStoryItemWudang0 = 248;

		public const short SectMainStoryItemWudang1 = 249;

		public const short SectMainStoryItemWudang2 = 250;

		public const short SectMainStoryItemWudang3 = 251;

		public const short SectMainStoryItemWudang4 = 252;

		public const short SectMainStoryItemWudang5 = 253;

		public const short SectMainStoryItemWudang6 = 254;

		public const short SectMainStoryItemWudang7 = 255;

		public const short SectMainStoryItemWudang8 = 256;

		public const short SectMainStoryItemWudang9 = 257;

		public const short SectMainStoryItemWudang10 = 263;

		public const short SectMainStoryItemJingangSutraFragments = 258;

		public const short SectMainStoryItemJingangSutraAuthentic = 259;

		public const short SectMainStoryItemJingangSutraFake = 260;

		public const short SectMainStoryItemJingangSutraEastern = 261;

		public const short SectMainStoryItemJingangEerieAmulet = 262;

		public const short MuddyStatue2 = 264;

		public const short MuddyStatue3 = 265;

		public const short JieqingAssassinationToken = 266;

		public const short EmeiWhiteGibbonToken = 267;

		public const short RanshanJadeAmulet = 268;

		public const short RanshanJadeSword = 269;

		public const short RanshanJadeRing = 270;

		public const short BrokenMuddyStatue = 271;

		public const short SectMainStoryItemWudangMountainSeed = 272;

		public const short SectMainStoryItemWudangCaveSeed = 273;

		public const short SectMainStoryItemWudangCanyonSeed = 274;

		public const short SectMainStoryItemWudangSwampSeed = 275;

		public const short SectMainStoryItemWudangHillSeed = 276;

		public const short SectMainStoryItemWudangTaoyuanSeed = 277;

		public const short SectMainStoryItemWudangFieldSeed = 278;

		public const short SectMainStoryItemWudangLakeSeed = 279;

		public const short SectMainStoryItemWudangWoodlandSeed = 280;

		public const short SectMainStoryItemWudangJungleSeed = 281;

		public const short SectMainStoryItemWudangRiverBeachSeed = 282;

		public const short SectMainStoryItemWudangValleySeed = 283;

		public const short SectMainStoryItemXuannvNotes = 284;

		public const short DLCGoldenWeb = 285;

		public const short DLCLoongScale = 286;

		public const short DLCJiaoEggMale = 287;

		public const short DLCJiaoEggFemale = 288;

		public const short JiaoWhite = 289;

		public const short JiaoBlack = 290;

		public const short JiaoGreen = 291;

		public const short JiaoRed = 292;

		public const short JiaoYellow = 293;

		public const short JiaoWB = 294;

		public const short JiaoWG = 295;

		public const short JiaoWR = 296;

		public const short JiaoWY = 297;

		public const short JiaoBG = 298;

		public const short JiaoBR = 299;

		public const short JiaoBY = 300;

		public const short JiaoGR = 301;

		public const short JiaoGY = 302;

		public const short JiaoRY = 303;

		public const short JiaoWBG = 304;

		public const short JiaoWBR = 305;

		public const short JiaoWBY = 306;

		public const short JiaoWGR = 307;

		public const short JiaoWGY = 308;

		public const short JiaoWRY = 309;

		public const short JiaoBGR = 310;

		public const short JiaoBGY = 311;

		public const short JiaoBRY = 312;

		public const short JiaoGRY = 313;

		public const short JiaoWBGR = 314;

		public const short JiaoWBGY = 315;

		public const short JiaoWBRY = 316;

		public const short JiaoWGRY = 317;

		public const short JiaoBGRY = 318;

		public const short JiaoWGRYB = 319;

		public const short SectMainStoryItemWuxianWugFairy = 320;

		public const short ResourceFood = 321;

		public const short ResourceWood = 322;

		public const short ResourceMetal = 323;

		public const short ResourceJade = 324;

		public const short ResourceFabric = 325;

		public const short ResourceHerb = 326;

		public const short ResourceMoney = 327;

		public const short ResourceAuthority = 328;

		public const short FuyuSwordFragment = 329;

		public const short SectMainStoryFulongChickenFeather = 330;

		public const short SectMainStoryFulongFeatherCoat = 331;

		public const short SectMainStoryFulongChickenMap = 332;

		public const short NormalResourceKeyConstruct1 = 333;

		public const short NormalResourceKeyConstruct2 = 334;

		public const short NormalResourceKeyConstruct3 = 335;

		public const short NormalResourceKeyConstruct4 = 336;

		public const short NormalResourceKeyConstruct5 = 337;

		public const short NormalResourceKeyConstruct6 = 338;

		public const short NormalResourceKeyConstruct7 = 342;

		public const short NormalResourceKeyConstruct8 = 339;

		public const short NormalResourceKeyConstruct9 = 340;

		public const short NormalResourceKeyConstruct10 = 341;

		public const short BoardSword = 343;

		public const short BrushInk = 344;

		public const short TridentBell = 345;

		public const short BuddhismRosary = 346;

		public const short GoldenCup = 347;

		public const short JadePendent = 348;

		public const short IronBowl = 349;

		public const short NineNeedles = 350;

		public const short DivinationSticks = 351;

		public const short Abacus = 352;

		public const short TeaPot = 353;

		public const short RoyalSash = 354;

		public const short SectMainStoryItemZhujianTongshengHead = 355;

		public const short SectMainStoryItemZhujianTongshengLeftArm = 356;

		public const short SectMainStoryItemZhujianTongshengRightArm = 357;

		public const short SectMainStoryItemZhujianTongshengLeftLeg = 358;

		public const short SectMainStoryItemZhujianTongshengRightLeg = 359;

		public const short SectMainStoryItemZhujianTongshengTorso = 360;

		public const short SectMainStoryItemZhujianWeaponPrototype = 361;

		public const short SectMainStoryItemZhujianWeaponPrototype1 = 362;

		public const short SectMainStoryItemZhujianWeaponPrototype2 = 363;

		public const short RareResourceKeyConstruct1 = 364;

		public const short RareResourceKeyConstruct2 = 365;

		public const short RareResourceKeyConstruct3 = 366;

		public const short RareResourceKeyConstruct4 = 367;

		public const short RareResourceKeyConstruct5 = 368;

		public const short RareResourceKeyConstruct6 = 369;

		public const short RareResourceKeyConstruct7 = 370;

		public const short RareResourceKeyConstruct8 = 371;

		public const short RareResourceKeyConstruct9 = 372;

		public const short RareResourceKeyConstruct10 = 373;

		public const short SectMainStoryItemYuanshanRosary = 374;

		public const short ShenJianSpiritWords = 375;

		public const short HuanSheSpiritWords = 376;

		public const short FuCangSpiritWords = 377;

		public const short YinShenSpiritWords = 378;

		public const short QuLiuWuSpiritWords = 379;

		public const short ShenJianMysteryWords = 380;

		public const short HuanSheMysteryWords = 381;

		public const short FuCangMysteryWords = 382;

		public const short YinShenMysteryWords = 383;

		public const short QuLiuWuMysteryWords = 384;

		public const short ThanksLetter0 = 385;

		public const short ThanksLetter1 = 386;

		public const short ThanksLetter2 = 387;

		public const short ThanksLetter3 = 388;

		public const short ThanksLetter4 = 389;

		public const short ThanksLetter5 = 390;

		public const short ThanksLetter6 = 391;

		public const short ThanksLetter7 = 392;

		public const short ThanksLetter8 = 393;

		public const short SectMainStoryItemWudangMountainSeedNormal = 394;

		public const short SectMainStoryItemWudangCaveSeedNormal = 395;

		public const short SectMainStoryItemWudangCanyonSeedNormal = 396;

		public const short SectMainStoryItemWudangSwampSeedNormal = 397;

		public const short SectMainStoryItemWudangHillSeedNormal = 398;

		public const short SectMainStoryItemWudangTaoyuanSeedNormal = 399;

		public const short SectMainStoryItemWudangFieldSeedNormal = 400;

		public const short SectMainStoryItemWudangLakeSeedNormal = 401;

		public const short SectMainStoryItemWudangWoodlandSeedNormal = 402;

		public const short SectMainStoryItemWudangJungleSeedNormal = 403;

		public const short SectMainStoryItemWudangRiverBeachSeedNormal = 404;

		public const short SectMainStoryItemWudangValleySeedNormal = 405;

		public const short HomingPigeon = 406;

		public const short SectMainStoryItemJieQingStars = 407;
	}

	public static class DefValue
	{
		public static MiscItem BloodDew0 => Instance[(short)0];

		public static MiscItem BloodDew1 => Instance[(short)1];

		public static MiscItem BloodDew2 => Instance[(short)2];

		public static MiscItem BloodDew3 => Instance[(short)3];

		public static MiscItem BloodDew4 => Instance[(short)4];

		public static MiscItem BloodDew5 => Instance[(short)5];

		public static MiscItem BloodDew6 => Instance[(short)6];

		public static MiscItem BloodDew7 => Instance[(short)7];

		public static MiscItem BloodDew8 => Instance[(short)8];

		public static MiscItem SweepNet => Instance[(short)9];

		public static MiscItem Teleogryllus => Instance[(short)10];

		public static MiscItem LongHornedGrasshopper => Instance[(short)11];

		public static MiscItem Grasshopper => Instance[(short)12];

		public static MiscItem DrumwingedKatydid => Instance[(short)13];

		public static MiscItem GoldenPipa => Instance[(short)14];

		public static MiscItem BambooSandfly => Instance[(short)15];

		public static MiscItem SmallYellowSandfly => Instance[(short)16];

		public static MiscItem PearGreenSandfly => Instance[(short)17];

		public static MiscItem FlowerSandfly => Instance[(short)18];

		public static MiscItem BlackSandfly => Instance[(short)19];

		public static MiscItem BrayingSandfly => Instance[(short)20];

		public static MiscItem BigYellowSandfly => Instance[(short)21];

		public static MiscItem GrassYellowSandfly => Instance[(short)22];

		public static MiscItem KitchenSandfly => Instance[(short)23];

		public static MiscItem MirrorSandfly => Instance[(short)24];

		public static MiscItem TripleTail => Instance[(short)25];

		public static MiscItem GoldenSandfly => Instance[(short)26];

		public static MiscItem Katydid => Instance[(short)27];

		public static MiscItem WesternPresentMonv0 => Instance[(short)28];

		public static MiscItem WesternPresentMonv1 => Instance[(short)29];

		public static MiscItem WesternPresentMonv2 => Instance[(short)30];

		public static MiscItem WesternPresentMonv3 => Instance[(short)31];

		public static MiscItem WesternPresentMonv4 => Instance[(short)32];

		public static MiscItem WesternPresentDayueYaochang0 => Instance[(short)33];

		public static MiscItem WesternPresentDayueYaochang1 => Instance[(short)34];

		public static MiscItem WesternPresentDayueYaochang2 => Instance[(short)35];

		public static MiscItem WesternPresentDayueYaochang3 => Instance[(short)36];

		public static MiscItem WesternPresentDayueYaochang4 => Instance[(short)37];

		public static MiscItem WesternPresentJiuhan0 => Instance[(short)38];

		public static MiscItem WesternPresentJiuhan1 => Instance[(short)39];

		public static MiscItem WesternPresentJiuhan2 => Instance[(short)40];

		public static MiscItem WesternPresentJiuhan3 => Instance[(short)41];

		public static MiscItem WesternPresentJiuhan4 => Instance[(short)42];

		public static MiscItem WesternPresentJinHuanger0 => Instance[(short)43];

		public static MiscItem WesternPresentJinHuanger1 => Instance[(short)44];

		public static MiscItem WesternPresentJinHuanger2 => Instance[(short)45];

		public static MiscItem WesternPresentJinHuanger3 => Instance[(short)46];

		public static MiscItem WesternPresentJinHuanger4 => Instance[(short)47];

		public static MiscItem WesternPresentYiYihou0 => Instance[(short)48];

		public static MiscItem WesternPresentYiYihou1 => Instance[(short)49];

		public static MiscItem WesternPresentYiYihou2 => Instance[(short)50];

		public static MiscItem WesternPresentYiYihou3 => Instance[(short)51];

		public static MiscItem WesternPresentYiYihou4 => Instance[(short)52];

		public static MiscItem WesternPresentWeiQi0 => Instance[(short)53];

		public static MiscItem WesternPresentWeiQi1 => Instance[(short)54];

		public static MiscItem WesternPresentWeiQi2 => Instance[(short)55];

		public static MiscItem WesternPresentWeiQi3 => Instance[(short)56];

		public static MiscItem WesternPresentWeiQi4 => Instance[(short)57];

		public static MiscItem WesternPresentYixiang0 => Instance[(short)58];

		public static MiscItem WesternPresentYixiang1 => Instance[(short)59];

		public static MiscItem WesternPresentYixiang2 => Instance[(short)60];

		public static MiscItem WesternPresentYixiang3 => Instance[(short)61];

		public static MiscItem WesternPresentYixiang4 => Instance[(short)62];

		public static MiscItem WesternPresentXuefeng0 => Instance[(short)63];

		public static MiscItem WesternPresentXuefeng1 => Instance[(short)64];

		public static MiscItem WesternPresentXuefeng2 => Instance[(short)65];

		public static MiscItem WesternPresentXuefeng3 => Instance[(short)66];

		public static MiscItem WesternPresentXuefeng4 => Instance[(short)67];

		public static MiscItem WesternPresentShuFang0 => Instance[(short)68];

		public static MiscItem WesternPresentShuFang1 => Instance[(short)69];

		public static MiscItem WesternPresentShuFang2 => Instance[(short)70];

		public static MiscItem WesternPresentShuFang3 => Instance[(short)71];

		public static MiscItem WesternPresentShuFang4 => Instance[(short)72];

		public static MiscItem Rope0 => Instance[(short)73];

		public static MiscItem Rope1 => Instance[(short)74];

		public static MiscItem Rope2 => Instance[(short)75];

		public static MiscItem Rope3 => Instance[(short)76];

		public static MiscItem Rope4 => Instance[(short)77];

		public static MiscItem Rope5 => Instance[(short)78];

		public static MiscItem Rope6 => Instance[(short)79];

		public static MiscItem Rope7 => Instance[(short)80];

		public static MiscItem Rope8 => Instance[(short)81];

		public static MiscItem Jar0 => Instance[(short)82];

		public static MiscItem Jar1 => Instance[(short)83];

		public static MiscItem Jar2 => Instance[(short)84];

		public static MiscItem Jar3 => Instance[(short)85];

		public static MiscItem Jar4 => Instance[(short)86];

		public static MiscItem Jar5 => Instance[(short)87];

		public static MiscItem Jar6 => Instance[(short)88];

		public static MiscItem Jar7 => Instance[(short)89];

		public static MiscItem Jar8 => Instance[(short)90];

		public static MiscItem TestingNeedle => Instance[(short)91];

		public static MiscItem MaterialKeyConstruct0 => Instance[(short)92];

		public static MiscItem MaterialKeyConstruct1 => Instance[(short)93];

		public static MiscItem MaterialKeyConstruct2 => Instance[(short)94];

		public static MiscItem MaterialKeyConstruct3 => Instance[(short)95];

		public static MiscItem MaterialKeyConstruct4 => Instance[(short)96];

		public static MiscItem MaterialKeyConstruct5 => Instance[(short)97];

		public static MiscItem MaterialKeyConstruct6 => Instance[(short)98];

		public static MiscItem MaterialKeyConstruct7 => Instance[(short)99];

		public static MiscItem MaterialKeyConstruct8 => Instance[(short)100];

		public static MiscItem MaterialKeyConstruct9 => Instance[(short)101];

		public static MiscItem CombatSkillKeyConstruct0 => Instance[(short)102];

		public static MiscItem CombatSkillKeyConstruct1 => Instance[(short)103];

		public static MiscItem CombatSkillKeyConstruct2 => Instance[(short)104];

		public static MiscItem CombatSkillKeyConstruct3 => Instance[(short)105];

		public static MiscItem CombatSkillKeyConstruct4 => Instance[(short)106];

		public static MiscItem CombatSkillKeyConstruct5 => Instance[(short)107];

		public static MiscItem CombatSkillKeyConstruct6 => Instance[(short)108];

		public static MiscItem CombatSkillKeyConstruct7 => Instance[(short)109];

		public static MiscItem CombatSkillKeyConstruct8 => Instance[(short)110];

		public static MiscItem CombatSkillKeyConstruct9 => Instance[(short)111];

		public static MiscItem CombatSkillKeyConstruct10 => Instance[(short)112];

		public static MiscItem CombatSkillKeyConstruct11 => Instance[(short)113];

		public static MiscItem CombatSkillKeyConstruct12 => Instance[(short)114];

		public static MiscItem CombatSkillKeyConstruct13 => Instance[(short)115];

		public static MiscItem CombatSkillKeyConstruct14 => Instance[(short)116];

		public static MiscItem CombatSkillKeyConstruct15 => Instance[(short)117];

		public static MiscItem MusicKeyConstruct0 => Instance[(short)118];

		public static MiscItem MusicKeyConstruct1 => Instance[(short)119];

		public static MiscItem MusicKeyConstruct2 => Instance[(short)120];

		public static MiscItem MusicKeyConstruct3 => Instance[(short)121];

		public static MiscItem ChessKeyConstruct0 => Instance[(short)122];

		public static MiscItem ChessKeyConstruct1 => Instance[(short)123];

		public static MiscItem ChessKeyConstruct2 => Instance[(short)124];

		public static MiscItem ChessKeyConstruct3 => Instance[(short)125];

		public static MiscItem PoemKeyConstruct0 => Instance[(short)126];

		public static MiscItem PoemKeyConstruct1 => Instance[(short)127];

		public static MiscItem PoemKeyConstruct2 => Instance[(short)128];

		public static MiscItem PoemKeyConstruct3 => Instance[(short)129];

		public static MiscItem PaintingKeyConstruct0 => Instance[(short)130];

		public static MiscItem PaintingKeyConstruct1 => Instance[(short)131];

		public static MiscItem PaintingKeyConstruct2 => Instance[(short)132];

		public static MiscItem PaintingKeyConstruct3 => Instance[(short)133];

		public static MiscItem MathKeyConstruct0 => Instance[(short)134];

		public static MiscItem MathKeyConstruct1 => Instance[(short)135];

		public static MiscItem MathKeyConstruct2 => Instance[(short)136];

		public static MiscItem MathKeyConstruct3 => Instance[(short)137];

		public static MiscItem AppraisalKeyConstruct0 => Instance[(short)138];

		public static MiscItem AppraisalKeyConstruct1 => Instance[(short)139];

		public static MiscItem AppraisalKeyConstruct2 => Instance[(short)140];

		public static MiscItem AppraisalKeyConstruct3 => Instance[(short)141];

		public static MiscItem AppraisalKeyConstruct4 => Instance[(short)142];

		public static MiscItem ForgingKeyConstruct0 => Instance[(short)143];

		public static MiscItem ForgingKeyConstruct1 => Instance[(short)144];

		public static MiscItem ForgingKeyConstruct2 => Instance[(short)145];

		public static MiscItem ForgingKeyConstruct3 => Instance[(short)146];

		public static MiscItem ForgingKeyConstruct4 => Instance[(short)147];

		public static MiscItem ForgingKeyConstruct5 => Instance[(short)148];

		public static MiscItem WoodworkingKeyConstruct0 => Instance[(short)149];

		public static MiscItem WoodworkingKeyConstruct1 => Instance[(short)150];

		public static MiscItem WoodworkingKeyConstruct2 => Instance[(short)151];

		public static MiscItem WoodworkingKeyConstruct3 => Instance[(short)152];

		public static MiscItem WoodworkingKeyConstruct4 => Instance[(short)153];

		public static MiscItem WoodworkingKeyConstruct5 => Instance[(short)154];

		public static MiscItem MedicineKeyConstruct0 => Instance[(short)155];

		public static MiscItem MedicineKeyConstruct1 => Instance[(short)156];

		public static MiscItem MedicineKeyConstruct2 => Instance[(short)157];

		public static MiscItem MedicineKeyConstruct3 => Instance[(short)158];

		public static MiscItem MedicineKeyConstruct4 => Instance[(short)159];

		public static MiscItem MedicineKeyConstruct5 => Instance[(short)160];

		public static MiscItem ToxicologyKeyConstruct0 => Instance[(short)161];

		public static MiscItem ToxicologyKeyConstruct1 => Instance[(short)162];

		public static MiscItem ToxicologyKeyConstruct2 => Instance[(short)163];

		public static MiscItem ToxicologyKeyConstruct3 => Instance[(short)164];

		public static MiscItem ToxicologyKeyConstruct4 => Instance[(short)165];

		public static MiscItem ToxicologyKeyConstruct5 => Instance[(short)166];

		public static MiscItem WeavingKeyConstruct0 => Instance[(short)167];

		public static MiscItem WeavingKeyConstruct1 => Instance[(short)168];

		public static MiscItem WeavingKeyConstruct2 => Instance[(short)169];

		public static MiscItem WeavingKeyConstruct3 => Instance[(short)170];

		public static MiscItem WeavingKeyConstruct4 => Instance[(short)171];

		public static MiscItem WeavingKeyConstruct5 => Instance[(short)172];

		public static MiscItem JadeKeyConstruct0 => Instance[(short)173];

		public static MiscItem JadeKeyConstruct1 => Instance[(short)174];

		public static MiscItem JadeKeyConstruct2 => Instance[(short)175];

		public static MiscItem JadeKeyConstruct3 => Instance[(short)176];

		public static MiscItem JadeKeyConstruct4 => Instance[(short)177];

		public static MiscItem JadeKeyConstruct5 => Instance[(short)178];

		public static MiscItem TaoismKeyConstruct0 => Instance[(short)179];

		public static MiscItem TaoismKeyConstruct1 => Instance[(short)180];

		public static MiscItem TaoismKeyConstruct2 => Instance[(short)181];

		public static MiscItem TaoismKeyConstruct3 => Instance[(short)182];

		public static MiscItem BuddhismKeyConstruct0 => Instance[(short)183];

		public static MiscItem BuddhismKeyConstruct1 => Instance[(short)184];

		public static MiscItem BuddhismKeyConstruct2 => Instance[(short)185];

		public static MiscItem BuddhismKeyConstruct3 => Instance[(short)186];

		public static MiscItem CookingKeyConstruct0 => Instance[(short)187];

		public static MiscItem CookingKeyConstruct1 => Instance[(short)188];

		public static MiscItem CookingKeyConstruct2 => Instance[(short)189];

		public static MiscItem CookingKeyConstruct3 => Instance[(short)190];

		public static MiscItem CookingKeyConstruct4 => Instance[(short)191];

		public static MiscItem CookingKeyConstruct5 => Instance[(short)192];

		public static MiscItem EclecticKeyConstruct0 => Instance[(short)193];

		public static MiscItem EclecticKeyConstruct1 => Instance[(short)194];

		public static MiscItem EclecticKeyConstruct2 => Instance[(short)195];

		public static MiscItem EclecticKeyConstruct3 => Instance[(short)196];

		public static MiscItem TaiwuGenealogy => Instance[(short)197];

		public static MiscItem WesternRegionsMap => Instance[(short)198];

		public static MiscItem WheelOfKarma => Instance[(short)199];

		public static MiscItem SwordFragmentsStart => Instance[(short)200];

		public static MiscItem SwordFragments1 => Instance[(short)201];

		public static MiscItem SwordFragments2 => Instance[(short)202];

		public static MiscItem SwordFragments3 => Instance[(short)203];

		public static MiscItem SwordFragments4 => Instance[(short)204];

		public static MiscItem SwordFragments5 => Instance[(short)205];

		public static MiscItem SwordFragments6 => Instance[(short)206];

		public static MiscItem SwordFragments7 => Instance[(short)207];

		public static MiscItem SwordFragments8 => Instance[(short)208];

		public static MiscItem SwordFragmentsEnd => Instance[(short)209];

		public static MiscItem FuyuSwordGrip => Instance[(short)210];

		public static MiscItem LegacyBookNeigong => Instance[(short)211];

		public static MiscItem LegacyBookPosing => Instance[(short)212];

		public static MiscItem LegacyBookStunt => Instance[(short)213];

		public static MiscItem LegacyBookFistAndPalm => Instance[(short)214];

		public static MiscItem LegacyBookFinger => Instance[(short)215];

		public static MiscItem LegacyBookLeg => Instance[(short)216];

		public static MiscItem LegacyBookThrow => Instance[(short)217];

		public static MiscItem LegacyBookSword => Instance[(short)218];

		public static MiscItem LegacyBookBlade => Instance[(short)219];

		public static MiscItem LegacyBookPolearm => Instance[(short)220];

		public static MiscItem LegacyBookSpecial => Instance[(short)221];

		public static MiscItem LegacyBookWhip => Instance[(short)222];

		public static MiscItem LegacyBookControllableShot => Instance[(short)223];

		public static MiscItem LegacyBookCombatMusic => Instance[(short)224];

		public static MiscItem WuYing => Instance[(short)225];

		public static MiscItem HuanxinBloodyWutong => Instance[(short)226];

		public static MiscItem TwelveBambooThorn => Instance[(short)227];

		public static MiscItem RemainsOfXiangong => Instance[(short)228];

		public static MiscItem NiceLittleBoat => Instance[(short)229];

		public static MiscItem FuyuSwordFurnace => Instance[(short)230];

		public static MiscItem SevenCutBlackBamboo => Instance[(short)231];

		public static MiscItem LuggageOfHuanxin => Instance[(short)232];

		public static MiscItem SevenColorIronPlate => Instance[(short)233];

		public static MiscItem TianJieFuLu => Instance[(short)234];

		public static MiscItem LifeSkillClassic => Instance[(short)235];

		public static MiscItem CombatSkillClassic => Instance[(short)236];

		public static MiscItem BasketOnBack => Instance[(short)237];

		public static MiscItem HuntingBow => Instance[(short)238];

		public static MiscItem Hammer => Instance[(short)239];

		public static MiscItem BrokenBowl => Instance[(short)240];

		public static MiscItem GrainSac => Instance[(short)241];

		public static MiscItem CrippledShoes => Instance[(short)242];

		public static MiscItem AntiqueJadeBat => Instance[(short)243];

		public static MiscItem AntiqueJadeFox => Instance[(short)244];

		public static MiscItem AntiqueJadeButterfly => Instance[(short)245];

		public static MiscItem AntiqueBell => Instance[(short)246];

		public static MiscItem MuddyStatue => Instance[(short)247];

		public static MiscItem SectMainStoryItemWudang0 => Instance[(short)248];

		public static MiscItem SectMainStoryItemWudang1 => Instance[(short)249];

		public static MiscItem SectMainStoryItemWudang2 => Instance[(short)250];

		public static MiscItem SectMainStoryItemWudang3 => Instance[(short)251];

		public static MiscItem SectMainStoryItemWudang4 => Instance[(short)252];

		public static MiscItem SectMainStoryItemWudang5 => Instance[(short)253];

		public static MiscItem SectMainStoryItemWudang6 => Instance[(short)254];

		public static MiscItem SectMainStoryItemWudang7 => Instance[(short)255];

		public static MiscItem SectMainStoryItemWudang8 => Instance[(short)256];

		public static MiscItem SectMainStoryItemWudang9 => Instance[(short)257];

		public static MiscItem SectMainStoryItemWudang10 => Instance[(short)263];

		public static MiscItem SectMainStoryItemJingangSutraFragments => Instance[(short)258];

		public static MiscItem SectMainStoryItemJingangSutraAuthentic => Instance[(short)259];

		public static MiscItem SectMainStoryItemJingangSutraFake => Instance[(short)260];

		public static MiscItem SectMainStoryItemJingangSutraEastern => Instance[(short)261];

		public static MiscItem SectMainStoryItemJingangEerieAmulet => Instance[(short)262];

		public static MiscItem MuddyStatue2 => Instance[(short)264];

		public static MiscItem MuddyStatue3 => Instance[(short)265];

		public static MiscItem JieqingAssassinationToken => Instance[(short)266];

		public static MiscItem EmeiWhiteGibbonToken => Instance[(short)267];

		public static MiscItem RanshanJadeAmulet => Instance[(short)268];

		public static MiscItem RanshanJadeSword => Instance[(short)269];

		public static MiscItem RanshanJadeRing => Instance[(short)270];

		public static MiscItem BrokenMuddyStatue => Instance[(short)271];

		public static MiscItem SectMainStoryItemWudangMountainSeed => Instance[(short)272];

		public static MiscItem SectMainStoryItemWudangCaveSeed => Instance[(short)273];

		public static MiscItem SectMainStoryItemWudangCanyonSeed => Instance[(short)274];

		public static MiscItem SectMainStoryItemWudangSwampSeed => Instance[(short)275];

		public static MiscItem SectMainStoryItemWudangHillSeed => Instance[(short)276];

		public static MiscItem SectMainStoryItemWudangTaoyuanSeed => Instance[(short)277];

		public static MiscItem SectMainStoryItemWudangFieldSeed => Instance[(short)278];

		public static MiscItem SectMainStoryItemWudangLakeSeed => Instance[(short)279];

		public static MiscItem SectMainStoryItemWudangWoodlandSeed => Instance[(short)280];

		public static MiscItem SectMainStoryItemWudangJungleSeed => Instance[(short)281];

		public static MiscItem SectMainStoryItemWudangRiverBeachSeed => Instance[(short)282];

		public static MiscItem SectMainStoryItemWudangValleySeed => Instance[(short)283];

		public static MiscItem SectMainStoryItemXuannvNotes => Instance[(short)284];

		public static MiscItem DLCGoldenWeb => Instance[(short)285];

		public static MiscItem DLCLoongScale => Instance[(short)286];

		public static MiscItem DLCJiaoEggMale => Instance[(short)287];

		public static MiscItem DLCJiaoEggFemale => Instance[(short)288];

		public static MiscItem JiaoWhite => Instance[(short)289];

		public static MiscItem JiaoBlack => Instance[(short)290];

		public static MiscItem JiaoGreen => Instance[(short)291];

		public static MiscItem JiaoRed => Instance[(short)292];

		public static MiscItem JiaoYellow => Instance[(short)293];

		public static MiscItem JiaoWB => Instance[(short)294];

		public static MiscItem JiaoWG => Instance[(short)295];

		public static MiscItem JiaoWR => Instance[(short)296];

		public static MiscItem JiaoWY => Instance[(short)297];

		public static MiscItem JiaoBG => Instance[(short)298];

		public static MiscItem JiaoBR => Instance[(short)299];

		public static MiscItem JiaoBY => Instance[(short)300];

		public static MiscItem JiaoGR => Instance[(short)301];

		public static MiscItem JiaoGY => Instance[(short)302];

		public static MiscItem JiaoRY => Instance[(short)303];

		public static MiscItem JiaoWBG => Instance[(short)304];

		public static MiscItem JiaoWBR => Instance[(short)305];

		public static MiscItem JiaoWBY => Instance[(short)306];

		public static MiscItem JiaoWGR => Instance[(short)307];

		public static MiscItem JiaoWGY => Instance[(short)308];

		public static MiscItem JiaoWRY => Instance[(short)309];

		public static MiscItem JiaoBGR => Instance[(short)310];

		public static MiscItem JiaoBGY => Instance[(short)311];

		public static MiscItem JiaoBRY => Instance[(short)312];

		public static MiscItem JiaoGRY => Instance[(short)313];

		public static MiscItem JiaoWBGR => Instance[(short)314];

		public static MiscItem JiaoWBGY => Instance[(short)315];

		public static MiscItem JiaoWBRY => Instance[(short)316];

		public static MiscItem JiaoWGRY => Instance[(short)317];

		public static MiscItem JiaoBGRY => Instance[(short)318];

		public static MiscItem JiaoWGRYB => Instance[(short)319];

		public static MiscItem SectMainStoryItemWuxianWugFairy => Instance[(short)320];

		public static MiscItem ResourceFood => Instance[(short)321];

		public static MiscItem ResourceWood => Instance[(short)322];

		public static MiscItem ResourceMetal => Instance[(short)323];

		public static MiscItem ResourceJade => Instance[(short)324];

		public static MiscItem ResourceFabric => Instance[(short)325];

		public static MiscItem ResourceHerb => Instance[(short)326];

		public static MiscItem ResourceMoney => Instance[(short)327];

		public static MiscItem ResourceAuthority => Instance[(short)328];

		public static MiscItem FuyuSwordFragment => Instance[(short)329];

		public static MiscItem SectMainStoryFulongChickenFeather => Instance[(short)330];

		public static MiscItem SectMainStoryFulongFeatherCoat => Instance[(short)331];

		public static MiscItem SectMainStoryFulongChickenMap => Instance[(short)332];

		public static MiscItem NormalResourceKeyConstruct1 => Instance[(short)333];

		public static MiscItem NormalResourceKeyConstruct2 => Instance[(short)334];

		public static MiscItem NormalResourceKeyConstruct3 => Instance[(short)335];

		public static MiscItem NormalResourceKeyConstruct4 => Instance[(short)336];

		public static MiscItem NormalResourceKeyConstruct5 => Instance[(short)337];

		public static MiscItem NormalResourceKeyConstruct6 => Instance[(short)338];

		public static MiscItem NormalResourceKeyConstruct7 => Instance[(short)342];

		public static MiscItem NormalResourceKeyConstruct8 => Instance[(short)339];

		public static MiscItem NormalResourceKeyConstruct9 => Instance[(short)340];

		public static MiscItem NormalResourceKeyConstruct10 => Instance[(short)341];

		public static MiscItem BoardSword => Instance[(short)343];

		public static MiscItem BrushInk => Instance[(short)344];

		public static MiscItem TridentBell => Instance[(short)345];

		public static MiscItem BuddhismRosary => Instance[(short)346];

		public static MiscItem GoldenCup => Instance[(short)347];

		public static MiscItem JadePendent => Instance[(short)348];

		public static MiscItem IronBowl => Instance[(short)349];

		public static MiscItem NineNeedles => Instance[(short)350];

		public static MiscItem DivinationSticks => Instance[(short)351];

		public static MiscItem Abacus => Instance[(short)352];

		public static MiscItem TeaPot => Instance[(short)353];

		public static MiscItem RoyalSash => Instance[(short)354];

		public static MiscItem SectMainStoryItemZhujianTongshengHead => Instance[(short)355];

		public static MiscItem SectMainStoryItemZhujianTongshengLeftArm => Instance[(short)356];

		public static MiscItem SectMainStoryItemZhujianTongshengRightArm => Instance[(short)357];

		public static MiscItem SectMainStoryItemZhujianTongshengLeftLeg => Instance[(short)358];

		public static MiscItem SectMainStoryItemZhujianTongshengRightLeg => Instance[(short)359];

		public static MiscItem SectMainStoryItemZhujianTongshengTorso => Instance[(short)360];

		public static MiscItem SectMainStoryItemZhujianWeaponPrototype => Instance[(short)361];

		public static MiscItem SectMainStoryItemZhujianWeaponPrototype1 => Instance[(short)362];

		public static MiscItem SectMainStoryItemZhujianWeaponPrototype2 => Instance[(short)363];

		public static MiscItem RareResourceKeyConstruct1 => Instance[(short)364];

		public static MiscItem RareResourceKeyConstruct2 => Instance[(short)365];

		public static MiscItem RareResourceKeyConstruct3 => Instance[(short)366];

		public static MiscItem RareResourceKeyConstruct4 => Instance[(short)367];

		public static MiscItem RareResourceKeyConstruct5 => Instance[(short)368];

		public static MiscItem RareResourceKeyConstruct6 => Instance[(short)369];

		public static MiscItem RareResourceKeyConstruct7 => Instance[(short)370];

		public static MiscItem RareResourceKeyConstruct8 => Instance[(short)371];

		public static MiscItem RareResourceKeyConstruct9 => Instance[(short)372];

		public static MiscItem RareResourceKeyConstruct10 => Instance[(short)373];

		public static MiscItem SectMainStoryItemYuanshanRosary => Instance[(short)374];

		public static MiscItem ShenJianSpiritWords => Instance[(short)375];

		public static MiscItem HuanSheSpiritWords => Instance[(short)376];

		public static MiscItem FuCangSpiritWords => Instance[(short)377];

		public static MiscItem YinShenSpiritWords => Instance[(short)378];

		public static MiscItem QuLiuWuSpiritWords => Instance[(short)379];

		public static MiscItem ShenJianMysteryWords => Instance[(short)380];

		public static MiscItem HuanSheMysteryWords => Instance[(short)381];

		public static MiscItem FuCangMysteryWords => Instance[(short)382];

		public static MiscItem YinShenMysteryWords => Instance[(short)383];

		public static MiscItem QuLiuWuMysteryWords => Instance[(short)384];

		public static MiscItem ThanksLetter0 => Instance[(short)385];

		public static MiscItem ThanksLetter1 => Instance[(short)386];

		public static MiscItem ThanksLetter2 => Instance[(short)387];

		public static MiscItem ThanksLetter3 => Instance[(short)388];

		public static MiscItem ThanksLetter4 => Instance[(short)389];

		public static MiscItem ThanksLetter5 => Instance[(short)390];

		public static MiscItem ThanksLetter6 => Instance[(short)391];

		public static MiscItem ThanksLetter7 => Instance[(short)392];

		public static MiscItem ThanksLetter8 => Instance[(short)393];

		public static MiscItem SectMainStoryItemWudangMountainSeedNormal => Instance[(short)394];

		public static MiscItem SectMainStoryItemWudangCaveSeedNormal => Instance[(short)395];

		public static MiscItem SectMainStoryItemWudangCanyonSeedNormal => Instance[(short)396];

		public static MiscItem SectMainStoryItemWudangSwampSeedNormal => Instance[(short)397];

		public static MiscItem SectMainStoryItemWudangHillSeedNormal => Instance[(short)398];

		public static MiscItem SectMainStoryItemWudangTaoyuanSeedNormal => Instance[(short)399];

		public static MiscItem SectMainStoryItemWudangFieldSeedNormal => Instance[(short)400];

		public static MiscItem SectMainStoryItemWudangLakeSeedNormal => Instance[(short)401];

		public static MiscItem SectMainStoryItemWudangWoodlandSeedNormal => Instance[(short)402];

		public static MiscItem SectMainStoryItemWudangJungleSeedNormal => Instance[(short)403];

		public static MiscItem SectMainStoryItemWudangRiverBeachSeedNormal => Instance[(short)404];

		public static MiscItem SectMainStoryItemWudangValleySeedNormal => Instance[(short)405];

		public static MiscItem HomingPigeon => Instance[(short)406];

		public static MiscItem SectMainStoryItemJieQingStars => Instance[(short)407];
	}

	public static Misc Instance = new Misc();

	private readonly HashSet<string> RequiredFields = new HashSet<string>
	{
		"ItemSubType", "GroupId", "ResourceType", "MakeItemSubType", "BreakBonusEffect", "RequireCombatConfig", "StateBuryAmount", "CombatUseEffect", "CombatPrepareUseEffect", "TemplateId",
		"Name", "Grade", "Icon", "Desc", "GainExp"
	};

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
		_dataArray.Add(new MiscItem(0, 0, 12, 1200, 0, 0, "icon_Misc_jiupinxielou", 1, transferable: true, stackable: true, wagerable: false, refinable: false, poisonable: false, repairable: false, inheritable: true, consumable: true, 0, 10, 50, 100, 0, 0, 600, 3, allowRandomCreate: true, 50, isSpecial: false, -1, -1, 1, 40, 25, 0, 0, 1, canUseOnPrepareCombat: true, allowUseInPlayAndTest: false, -1, 60, new List<short>(), new List<int>(), EMiscGenerateType.Invalid, new List<TreasureStateInfo>(), EMiscFilterType.Item, -1, -1, -1, 0));
		_dataArray.Add(new MiscItem(1, 0, 12, 1200, 1, 0, "icon_Misc_jiupinxielou", 2, transferable: true, stackable: true, wagerable: false, refinable: false, poisonable: false, repairable: false, inheritable: true, consumable: true, 0, 10, 100, 200, 0, 0, 1200, 4, allowRandomCreate: true, 50, isSpecial: false, -1, -1, 1, 40, 50, 0, 0, 1, canUseOnPrepareCombat: true, allowUseInPlayAndTest: false, -1, 60, new List<short>(), new List<int>(), EMiscGenerateType.Invalid, new List<TreasureStateInfo>(), EMiscFilterType.Item, -1, -1, -1, 0));
		_dataArray.Add(new MiscItem(2, 0, 12, 1200, 2, 0, "icon_Misc_jiupinxielou", 3, transferable: true, stackable: true, wagerable: false, refinable: false, poisonable: false, repairable: false, inheritable: true, consumable: true, 0, 10, 300, 600, 0, 0, 1800, 5, allowRandomCreate: true, 50, isSpecial: false, -1, -1, 1, 40, 100, 0, 0, 1, canUseOnPrepareCombat: true, allowUseInPlayAndTest: false, -1, 60, new List<short>(), new List<int>(), EMiscGenerateType.Invalid, new List<TreasureStateInfo>(), EMiscFilterType.Item, -1, -1, -1, 0));
		_dataArray.Add(new MiscItem(3, 0, 12, 1200, 3, 0, "icon_Misc_liupinxielou", 4, transferable: true, stackable: true, wagerable: false, refinable: false, poisonable: false, repairable: false, inheritable: true, consumable: true, 0, 10, 750, 1500, 0, 0, 3000, 6, allowRandomCreate: true, 50, isSpecial: false, -1, -1, 1, 40, 200, 0, 0, 1, canUseOnPrepareCombat: true, allowUseInPlayAndTest: false, -1, 60, new List<short>(), new List<int>(), EMiscGenerateType.Invalid, new List<TreasureStateInfo>(), EMiscFilterType.Item, -1, -1, -1, 0));
		_dataArray.Add(new MiscItem(4, 0, 12, 1200, 4, 0, "icon_Misc_liupinxielou", 5, transferable: true, stackable: true, wagerable: false, refinable: false, poisonable: false, repairable: false, inheritable: true, consumable: true, 0, 10, 1550, 3100, 1, 0, 4200, 7, allowRandomCreate: true, 50, isSpecial: false, -1, -1, 1, 40, 400, 0, 0, 1, canUseOnPrepareCombat: true, allowUseInPlayAndTest: false, -1, 60, new List<short>(), new List<int>(), EMiscGenerateType.Invalid, new List<TreasureStateInfo>(), EMiscFilterType.Item, -1, -1, -1, 0));
		_dataArray.Add(new MiscItem(5, 0, 12, 1200, 5, 0, "icon_Misc_liupinxielou", 6, transferable: true, stackable: true, wagerable: false, refinable: false, poisonable: false, repairable: false, inheritable: true, consumable: true, 0, 10, 2800, 5600, 2, 0, 5400, 7, allowRandomCreate: true, 50, isSpecial: false, -1, -1, 1, 40, 800, 0, 0, 1, canUseOnPrepareCombat: true, allowUseInPlayAndTest: false, -1, 60, new List<short>(), new List<int>(), EMiscGenerateType.Invalid, new List<TreasureStateInfo>(), EMiscFilterType.Item, -1, -1, -1, 0));
		_dataArray.Add(new MiscItem(6, 0, 12, 1200, 6, 0, "icon_Misc_sanpinxielou", 7, transferable: true, stackable: true, wagerable: false, refinable: false, poisonable: false, repairable: false, inheritable: true, consumable: true, 0, 10, 4600, 9200, 3, 0, 7200, 8, allowRandomCreate: true, 50, isSpecial: false, -1, -1, 1, 40, 1600, 0, 0, 1, canUseOnPrepareCombat: true, allowUseInPlayAndTest: false, -1, 60, new List<short>(), new List<int>(), EMiscGenerateType.Invalid, new List<TreasureStateInfo>(), EMiscFilterType.Item, -1, -1, -1, 0));
		_dataArray.Add(new MiscItem(7, 0, 12, 1200, 7, 0, "icon_Misc_erpinxielou", 8, transferable: true, stackable: true, wagerable: false, refinable: false, poisonable: false, repairable: false, inheritable: true, consumable: true, 0, 10, 7050, 14100, 4, 0, 9000, 8, allowRandomCreate: true, 50, isSpecial: false, -1, -1, 1, 40, 3200, 0, 0, 1, canUseOnPrepareCombat: true, allowUseInPlayAndTest: false, -1, 60, new List<short>(), new List<int>(), EMiscGenerateType.Invalid, new List<TreasureStateInfo>(), EMiscFilterType.Item, -1, -1, -1, 0));
		_dataArray.Add(new MiscItem(8, 0, 12, 1200, 8, 0, "icon_Misc_yipinxielou", 9, transferable: true, stackable: true, wagerable: false, refinable: false, poisonable: false, repairable: false, inheritable: true, consumable: true, 0, 10, 10250, 20500, 5, 0, 10800, 8, allowRandomCreate: true, 50, isSpecial: false, -1, -1, 1, 40, 6400, 0, 0, 1, canUseOnPrepareCombat: true, allowUseInPlayAndTest: false, -1, 60, new List<short>(), new List<int>(), EMiscGenerateType.Invalid, new List<TreasureStateInfo>(), EMiscFilterType.Item, -1, -1, -1, 0));
		_dataArray.Add(new MiscItem(9, 10, 12, 1200, 0, -1, "icon_Misc_buchongwang", 11, transferable: true, stackable: true, wagerable: false, refinable: false, poisonable: false, repairable: false, inheritable: true, consumable: false, 0, 10, 50, 100, 0, 0, 600, 3, allowRandomCreate: true, 50, isSpecial: false, -1, -1, 12, -1, 0, 0, 0, -1, canUseOnPrepareCombat: false, allowUseInPlayAndTest: false, -1, 60, new List<short>(), new List<int>(), EMiscGenerateType.Invalid, new List<TreasureStateInfo>(), EMiscFilterType.Item, -1, -1, -1, 0));
		_dataArray.Add(new MiscItem(10, 12, 12, 1204, 0, -1, "icon_Misc_youhulu", 13, transferable: true, stackable: true, wagerable: false, refinable: false, poisonable: false, repairable: false, inheritable: true, consumable: false, 0, 0, 50, 50, 0, 0, 1800, 8, allowRandomCreate: true, 50, isSpecial: true, -1, -1, 12, -1, 0, 0, 0, -1, canUseOnPrepareCombat: false, allowUseInPlayAndTest: false, -1, 60, new List<short>(), new List<int>(), EMiscGenerateType.Invalid, new List<TreasureStateInfo>(), EMiscFilterType.Misc, -1, -1, -1, 0));
		_dataArray.Add(new MiscItem(11, 14, 12, 1204, 0, -1, "icon_Misc_youhulu", 15, transferable: true, stackable: true, wagerable: false, refinable: false, poisonable: false, repairable: false, inheritable: true, consumable: false, 0, 0, 50, 50, 0, 0, 1800, 8, allowRandomCreate: true, 50, isSpecial: true, -1, -1, 12, -1, 0, 0, 0, -1, canUseOnPrepareCombat: false, allowUseInPlayAndTest: false, -1, 60, new List<short>(), new List<int>(), EMiscGenerateType.Invalid, new List<TreasureStateInfo>(), EMiscFilterType.Misc, -1, -1, -1, 0));
		_dataArray.Add(new MiscItem(12, 16, 12, 1204, 0, -1, "icon_Misc_youhulu", 17, transferable: true, stackable: true, wagerable: false, refinable: false, poisonable: false, repairable: false, inheritable: true, consumable: false, 0, 0, 50, 50, 0, 0, 1800, 8, allowRandomCreate: true, 50, isSpecial: true, -1, -1, 12, -1, 0, 0, 0, -1, canUseOnPrepareCombat: false, allowUseInPlayAndTest: false, -1, 60, new List<short>(), new List<int>(), EMiscGenerateType.Invalid, new List<TreasureStateInfo>(), EMiscFilterType.Misc, -1, -1, -1, 0));
		_dataArray.Add(new MiscItem(13, 18, 12, 1204, 0, -1, "icon_Misc_guchimingzhong", 19, transferable: true, stackable: true, wagerable: false, refinable: false, poisonable: false, repairable: false, inheritable: true, consumable: false, 0, 0, 50, 50, 0, 0, 1800, 8, allowRandomCreate: true, 50, isSpecial: true, -1, -1, 12, -1, 0, 0, 0, -1, canUseOnPrepareCombat: false, allowUseInPlayAndTest: false, -1, 60, new List<short>(), new List<int>(), EMiscGenerateType.Invalid, new List<TreasureStateInfo>(), EMiscFilterType.Misc, -1, -1, -1, 0));
		_dataArray.Add(new MiscItem(14, 20, 12, 1204, 0, -1, "icon_Misc_guchimingzhong", 21, transferable: true, stackable: true, wagerable: false, refinable: false, poisonable: false, repairable: false, inheritable: true, consumable: false, 0, 0, 50, 50, 0, 0, 1800, 8, allowRandomCreate: true, 50, isSpecial: true, -1, -1, 12, -1, 0, 0, 0, -1, canUseOnPrepareCombat: false, allowUseInPlayAndTest: false, -1, 60, new List<short>(), new List<int>(), EMiscGenerateType.Invalid, new List<TreasureStateInfo>(), EMiscFilterType.Misc, -1, -1, -1, 0));
		_dataArray.Add(new MiscItem(15, 22, 12, 1204, 0, -1, "icon_Misc_guchimingzhong", 23, transferable: true, stackable: true, wagerable: false, refinable: false, poisonable: false, repairable: false, inheritable: true, consumable: false, 0, 0, 50, 50, 0, 0, 1800, 8, allowRandomCreate: true, 50, isSpecial: true, -1, -1, 12, -1, 0, 0, 0, -1, canUseOnPrepareCombat: false, allowUseInPlayAndTest: false, -1, 60, new List<short>(), new List<int>(), EMiscGenerateType.Invalid, new List<TreasureStateInfo>(), EMiscFilterType.Misc, -1, -1, -1, 0));
		_dataArray.Add(new MiscItem(16, 24, 12, 1204, 0, -1, "icon_Misc_xiaohuangling", 25, transferable: true, stackable: true, wagerable: false, refinable: false, poisonable: false, repairable: false, inheritable: true, consumable: false, 0, 0, 50, 50, 0, 0, 1800, 8, allowRandomCreate: true, 50, isSpecial: true, -1, -1, 12, -1, 0, 0, 0, -1, canUseOnPrepareCombat: false, allowUseInPlayAndTest: false, -1, 60, new List<short>(), new List<int>(), EMiscGenerateType.Invalid, new List<TreasureStateInfo>(), EMiscFilterType.Misc, -1, -1, -1, 0));
		_dataArray.Add(new MiscItem(17, 26, 12, 1204, 0, -1, "icon_Misc_xiaohuangling", 27, transferable: true, stackable: true, wagerable: false, refinable: false, poisonable: false, repairable: false, inheritable: true, consumable: false, 0, 0, 50, 50, 0, 0, 1800, 8, allowRandomCreate: true, 50, isSpecial: true, -1, -1, 12, -1, 0, 0, 0, -1, canUseOnPrepareCombat: false, allowUseInPlayAndTest: false, -1, 60, new List<short>(), new List<int>(), EMiscGenerateType.Invalid, new List<TreasureStateInfo>(), EMiscFilterType.Misc, -1, -1, -1, 0));
		_dataArray.Add(new MiscItem(18, 28, 12, 1204, 0, -1, "icon_Misc_xiaohuangling", 29, transferable: true, stackable: true, wagerable: false, refinable: false, poisonable: false, repairable: false, inheritable: true, consumable: false, 0, 0, 50, 50, 0, 0, 1800, 8, allowRandomCreate: true, 50, isSpecial: true, -1, -1, 12, -1, 0, 0, 0, -1, canUseOnPrepareCombat: false, allowUseInPlayAndTest: false, -1, 60, new List<short>(), new List<int>(), EMiscGenerateType.Invalid, new List<TreasureStateInfo>(), EMiscFilterType.Misc, -1, -1, -1, 0));
		_dataArray.Add(new MiscItem(19, 30, 12, 1204, 0, -1, "icon_Misc_moling", 31, transferable: true, stackable: true, wagerable: false, refinable: false, poisonable: false, repairable: false, inheritable: true, consumable: false, 0, 0, 50, 50, 0, 0, 1800, 8, allowRandomCreate: true, 50, isSpecial: true, -1, -1, 12, -1, 0, 0, 0, -1, canUseOnPrepareCombat: false, allowUseInPlayAndTest: false, -1, 60, new List<short>(), new List<int>(), EMiscGenerateType.Invalid, new List<TreasureStateInfo>(), EMiscFilterType.Misc, -1, -1, -1, 0));
		_dataArray.Add(new MiscItem(20, 32, 12, 1204, 0, -1, "icon_Misc_moling", 33, transferable: true, stackable: true, wagerable: false, refinable: false, poisonable: false, repairable: false, inheritable: true, consumable: false, 0, 0, 50, 50, 0, 0, 1800, 8, allowRandomCreate: true, 50, isSpecial: true, -1, -1, 12, -1, 0, 0, 0, -1, canUseOnPrepareCombat: false, allowUseInPlayAndTest: false, -1, 60, new List<short>(), new List<int>(), EMiscGenerateType.Invalid, new List<TreasureStateInfo>(), EMiscFilterType.Misc, -1, -1, -1, 0));
		_dataArray.Add(new MiscItem(21, 34, 12, 1204, 0, -1, "icon_Misc_moling", 35, transferable: true, stackable: true, wagerable: false, refinable: false, poisonable: false, repairable: false, inheritable: true, consumable: false, 0, 0, 50, 50, 0, 0, 1800, 8, allowRandomCreate: true, 50, isSpecial: true, -1, -1, 12, -1, 0, 0, 0, -1, canUseOnPrepareCombat: false, allowUseInPlayAndTest: false, -1, 60, new List<short>(), new List<int>(), EMiscGenerateType.Invalid, new List<TreasureStateInfo>(), EMiscFilterType.Misc, -1, -1, -1, 0));
		_dataArray.Add(new MiscItem(22, 36, 12, 1204, 0, -1, "icon_Misc_caohuangling", 37, transferable: true, stackable: true, wagerable: false, refinable: false, poisonable: false, repairable: false, inheritable: true, consumable: false, 0, 0, 50, 50, 0, 0, 1800, 8, allowRandomCreate: true, 50, isSpecial: true, -1, -1, 12, -1, 0, 0, 0, -1, canUseOnPrepareCombat: false, allowUseInPlayAndTest: false, -1, 60, new List<short>(), new List<int>(), EMiscGenerateType.Invalid, new List<TreasureStateInfo>(), EMiscFilterType.Misc, -1, -1, -1, 0));
		_dataArray.Add(new MiscItem(23, 38, 12, 1204, 0, -1, "icon_Misc_caohuangling", 39, transferable: true, stackable: true, wagerable: false, refinable: false, poisonable: false, repairable: false, inheritable: true, consumable: false, 0, 0, 50, 50, 0, 0, 1800, 8, allowRandomCreate: true, 50, isSpecial: true, -1, -1, 12, -1, 0, 0, 0, -1, canUseOnPrepareCombat: false, allowUseInPlayAndTest: false, -1, 60, new List<short>(), new List<int>(), EMiscGenerateType.Invalid, new List<TreasureStateInfo>(), EMiscFilterType.Misc, -1, -1, -1, 0));
		_dataArray.Add(new MiscItem(24, 40, 12, 1204, 0, -1, "icon_Misc_caohuangling", 41, transferable: true, stackable: true, wagerable: false, refinable: false, poisonable: false, repairable: false, inheritable: true, consumable: false, 0, 0, 50, 50, 0, 0, 1800, 8, allowRandomCreate: true, 50, isSpecial: true, -1, -1, 12, -1, 0, 0, 0, -1, canUseOnPrepareCombat: false, allowUseInPlayAndTest: false, -1, 60, new List<short>(), new List<int>(), EMiscGenerateType.Invalid, new List<TreasureStateInfo>(), EMiscFilterType.Misc, -1, -1, -1, 0));
		_dataArray.Add(new MiscItem(25, 42, 12, 1204, 0, -1, "icon_Misc_sanwei", 43, transferable: true, stackable: true, wagerable: false, refinable: false, poisonable: false, repairable: false, inheritable: true, consumable: false, 0, 0, 50, 50, 0, 0, 1800, 8, allowRandomCreate: true, 50, isSpecial: true, -1, -1, 12, -1, 0, 0, 0, -1, canUseOnPrepareCombat: false, allowUseInPlayAndTest: false, -1, 60, new List<short>(), new List<int>(), EMiscGenerateType.Invalid, new List<TreasureStateInfo>(), EMiscFilterType.Misc, -1, -1, -1, 0));
		_dataArray.Add(new MiscItem(26, 44, 12, 1204, 0, -1, "icon_Misc_sanwei", 45, transferable: true, stackable: true, wagerable: false, refinable: false, poisonable: false, repairable: false, inheritable: true, consumable: false, 0, 0, 50, 50, 0, 0, 1800, 8, allowRandomCreate: true, 50, isSpecial: true, -1, -1, 12, -1, 0, 0, 0, -1, canUseOnPrepareCombat: false, allowUseInPlayAndTest: false, -1, 60, new List<short>(), new List<int>(), EMiscGenerateType.Invalid, new List<TreasureStateInfo>(), EMiscFilterType.Misc, -1, -1, -1, 0));
		_dataArray.Add(new MiscItem(27, 46, 12, 1204, 0, -1, "icon_Misc_sanwei", 47, transferable: true, stackable: true, wagerable: false, refinable: false, poisonable: false, repairable: false, inheritable: true, consumable: false, 0, 0, 50, 50, 0, 0, 1800, 8, allowRandomCreate: true, 50, isSpecial: true, -1, -1, 12, -1, 0, 0, 0, -1, canUseOnPrepareCombat: false, allowUseInPlayAndTest: false, -1, 60, new List<short>(), new List<int>(), EMiscGenerateType.Invalid, new List<TreasureStateInfo>(), EMiscFilterType.Misc, -1, -1, -1, 0));
		_dataArray.Add(new MiscItem(28, 48, 12, 1203, 4, 28, "icon_Misc_guandiao", 49, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, consumable: false, 0, 30, 3100, 9300, 2, 10, 4200, 7, allowRandomCreate: true, 0, isSpecial: true, -1, -1, 1200, -1, 0, 0, 0, -1, canUseOnPrepareCombat: false, allowUseInPlayAndTest: false, -1, 60, new List<short>(), new List<int>(), EMiscGenerateType.Invalid, new List<TreasureStateInfo>(), EMiscFilterType.WesternPresent, -1, -1, -1, 0));
		_dataArray.Add(new MiscItem(29, 50, 12, 1203, 5, 28, "icon_Misc_baige", 51, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, consumable: false, 0, 20, 5600, 16800, 3, 12, 5400, 7, allowRandomCreate: true, 0, isSpecial: true, -1, -1, 1200, -1, 0, 0, 0, -1, canUseOnPrepareCombat: false, allowUseInPlayAndTest: false, -1, 60, new List<short>(), new List<int>(), EMiscGenerateType.Invalid, new List<TreasureStateInfo>(), EMiscFilterType.WesternPresent, -1, -1, -1, 0));
		_dataArray.Add(new MiscItem(30, 52, 12, 1203, 6, 28, "icon_Misc_jiaoxiaoyumao", 53, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, consumable: false, 0, 10, 9200, 27600, 4, 14, 7200, 8, allowRandomCreate: true, 0, isSpecial: true, -1, -1, 1200, -1, 0, 0, 0, -1, canUseOnPrepareCombat: false, allowUseInPlayAndTest: false, -1, 60, new List<short>(), new List<int>(), EMiscGenerateType.Invalid, new List<TreasureStateInfo>(), EMiscFilterType.WesternPresent, -1, -1, -1, 0));
		_dataArray.Add(new MiscItem(31, 54, 12, 1203, 7, 28, "icon_Misc_miniewamaotouying", 55, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, consumable: false, 0, 30, 14100, 42300, 5, 16, 9000, 8, allowRandomCreate: true, 0, isSpecial: true, -1, -1, 1200, -1, 0, 0, 0, -1, canUseOnPrepareCombat: false, allowUseInPlayAndTest: false, -1, 60, new List<short>(), new List<int>(), EMiscGenerateType.Invalid, new List<TreasureStateInfo>(), EMiscFilterType.WesternPresent, -1, -1, -1, 0));
		_dataArray.Add(new MiscItem(32, 56, 12, 1203, 8, 28, "icon_Misc_taiyangshenyumao", 57, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, consumable: false, 0, 10, 20500, 61500, 6, 18, 10800, 8, allowRandomCreate: true, 0, isSpecial: true, -1, -1, 1200, -1, 0, 0, 0, -1, canUseOnPrepareCombat: false, allowUseInPlayAndTest: false, -1, 60, new List<short>(), new List<int>(), EMiscGenerateType.Invalid, new List<TreasureStateInfo>(), EMiscFilterType.WesternPresent, -1, -1, -1, 0));
		_dataArray.Add(new MiscItem(33, 58, 12, 1203, 4, 33, "icon_Misc_tuoling", 59, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, consumable: false, 0, 10, 3100, 9300, 2, 10, 4200, 7, allowRandomCreate: true, 0, isSpecial: true, -1, -1, 1200, -1, 0, 0, 0, -1, canUseOnPrepareCombat: false, allowUseInPlayAndTest: false, -1, 60, new List<short>(), new List<int>(), EMiscGenerateType.Invalid, new List<TreasureStateInfo>(), EMiscFilterType.WesternPresent, -1, -1, -1, 0));
		_dataArray.Add(new MiscItem(34, 60, 12, 1203, 5, 33, "icon_Misc_maodunmingdijian", 61, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, consumable: false, 0, 10, 5600, 16800, 3, 12, 5400, 7, allowRandomCreate: true, 0, isSpecial: true, -1, -1, 1200, -1, 0, 0, 0, -1, canUseOnPrepareCombat: false, allowUseInPlayAndTest: false, -1, 60, new List<short>(), new List<int>(), EMiscGenerateType.Invalid, new List<TreasureStateInfo>(), EMiscFilterType.WesternPresent, -1, -1, -1, 0));
		_dataArray.Add(new MiscItem(35, 62, 12, 1203, 6, 33, "icon_Misc_delitiebang", 63, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, consumable: false, 0, 600, 9200, 27600, 4, 14, 7200, 8, allowRandomCreate: true, 0, isSpecial: true, -1, -1, 1200, -1, 0, 0, 0, -1, canUseOnPrepareCombat: false, allowUseInPlayAndTest: false, -1, 60, new List<short>(), new List<int>(), EMiscGenerateType.Invalid, new List<TreasureStateInfo>(), EMiscFilterType.WesternPresent, -1, -1, -1, 0));
		_dataArray.Add(new MiscItem(36, 64, 12, 1203, 7, 33, "icon_Misc_yiluodetangdao", 65, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, consumable: false, 0, 250, 14100, 42300, 5, 16, 9000, 8, allowRandomCreate: true, 0, isSpecial: true, -1, -1, 1200, -1, 0, 0, 0, -1, canUseOnPrepareCombat: false, allowUseInPlayAndTest: false, -1, 60, new List<short>(), new List<int>(), EMiscGenerateType.Invalid, new List<TreasureStateInfo>(), EMiscFilterType.WesternPresent, -1, -1, -1, 0));
		_dataArray.Add(new MiscItem(37, 66, 12, 1203, 8, 33, "icon_Misc_huangjinkaijia", 67, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, consumable: false, 0, 1000, 20500, 61500, 6, 18, 10800, 8, allowRandomCreate: true, 0, isSpecial: true, -1, -1, 1200, -1, 0, 0, 0, -1, canUseOnPrepareCombat: false, allowUseInPlayAndTest: false, -1, 60, new List<short>(), new List<int>(), EMiscGenerateType.Invalid, new List<TreasureStateInfo>(), EMiscFilterType.WesternPresent, -1, -1, -1, 0));
		_dataArray.Add(new MiscItem(38, 68, 12, 1203, 4, 38, "icon_Misc_weisidɑzhennvxiang", 69, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, consumable: false, 0, 70, 3100, 9300, 2, 10, 4200, 7, allowRandomCreate: true, 0, isSpecial: true, -1, -1, 1200, -1, 0, 0, 0, -1, canUseOnPrepareCombat: false, allowUseInPlayAndTest: false, -1, 60, new List<short>(), new List<int>(), EMiscGenerateType.Invalid, new List<TreasureStateInfo>(), EMiscFilterType.WesternPresent, -1, -1, -1, 0));
		_dataArray.Add(new MiscItem(39, 70, 12, 1203, 5, 38, "icon_Misc_changshengjun", 71, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, consumable: false, 0, 90, 5600, 16800, 3, 12, 5400, 7, allowRandomCreate: true, 0, isSpecial: true, -1, -1, 1200, -1, 0, 0, 0, -1, canUseOnPrepareCombat: false, allowUseInPlayAndTest: false, -1, 60, new List<short>(), new List<int>(), EMiscGenerateType.Invalid, new List<TreasureStateInfo>(), EMiscFilterType.WesternPresent, -1, -1, -1, 0));
		_dataArray.Add(new MiscItem(40, 72, 12, 1203, 6, 38, "icon_Misc_dashiwangdiaoxiang", 73, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, consumable: false, 0, 120, 9200, 27600, 4, 14, 7200, 8, allowRandomCreate: true, 0, isSpecial: true, -1, -1, 1200, -1, 0, 0, 0, -1, canUseOnPrepareCombat: false, allowUseInPlayAndTest: false, -1, 60, new List<short>(), new List<int>(), EMiscGenerateType.Invalid, new List<TreasureStateInfo>(), EMiscFilterType.WesternPresent, -1, -1, -1, 0));
		_dataArray.Add(new MiscItem(41, 74, 12, 1203, 7, 38, "icon_Misc_lingweng", 75, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, consumable: false, 0, 100, 14100, 42300, 5, 16, 9000, 8, allowRandomCreate: true, 0, isSpecial: true, -1, -1, 1200, -1, 0, 0, 0, -1, canUseOnPrepareCombat: false, allowUseInPlayAndTest: false, -1, 60, new List<short>(), new List<int>(), EMiscGenerateType.Invalid, new List<TreasureStateInfo>(), EMiscFilterType.WesternPresent, -1, -1, -1, 0));
		_dataArray.Add(new MiscItem(42, 76, 12, 1203, 8, 38, "icon_Misc_weinasinvshenxiang", 77, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, consumable: false, 0, 800, 20500, 61500, 6, 18, 10800, 8, allowRandomCreate: true, 0, isSpecial: true, -1, -1, 1200, -1, 0, 0, 0, -1, canUseOnPrepareCombat: false, allowUseInPlayAndTest: false, -1, 60, new List<short>(), new List<int>(), EMiscGenerateType.Invalid, new List<TreasureStateInfo>(), EMiscFilterType.WesternPresent, -1, -1, -1, 0));
		_dataArray.Add(new MiscItem(43, 78, 12, 1203, 4, 43, "icon_Misc_putaojiu", 79, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, consumable: false, 0, 100, 3100, 9300, 2, 10, 4200, 7, allowRandomCreate: true, 0, isSpecial: true, -1, -1, 1200, -1, 0, 0, 0, -1, canUseOnPrepareCombat: false, allowUseInPlayAndTest: false, -1, 60, new List<short>(), new List<int>(), EMiscGenerateType.Invalid, new List<TreasureStateInfo>(), EMiscFilterType.WesternPresent, -1, -1, -1, 0));
		_dataArray.Add(new MiscItem(44, 80, 12, 1203, 5, 43, "icon_Misc_liuliyeguangbei", 81, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, consumable: false, 0, 20, 5600, 16800, 3, 12, 5400, 7, allowRandomCreate: true, 0, isSpecial: true, -1, -1, 1200, -1, 0, 0, 0, -1, canUseOnPrepareCombat: false, allowUseInPlayAndTest: false, -1, 60, new List<short>(), new List<int>(), EMiscGenerateType.Invalid, new List<TreasureStateInfo>(), EMiscFilterType.WesternPresent, -1, -1, -1, 0));
		_dataArray.Add(new MiscItem(45, 82, 12, 1203, 6, 43, "icon_Misc_longgaojiu", 83, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, consumable: false, 0, 100, 9200, 27600, 4, 14, 7200, 8, allowRandomCreate: true, 0, isSpecial: true, -1, -1, 1200, -1, 0, 0, 0, -1, canUseOnPrepareCombat: false, allowUseInPlayAndTest: false, -1, 60, new List<short>(), new List<int>(), EMiscGenerateType.Invalid, new List<TreasureStateInfo>(), EMiscFilterType.WesternPresent, -1, -1, -1, 0));
		_dataArray.Add(new MiscItem(46, 84, 12, 1203, 7, 43, "icon_Misc_yaokunjiu", 85, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, consumable: false, 0, 100, 14100, 42300, 5, 16, 9000, 8, allowRandomCreate: true, 0, isSpecial: true, -1, -1, 1200, -1, 0, 0, 0, -1, canUseOnPrepareCombat: false, allowUseInPlayAndTest: false, -1, 60, new List<short>(), new List<int>(), EMiscGenerateType.Invalid, new List<TreasureStateInfo>(), EMiscFilterType.WesternPresent, -1, -1, -1, 0));
		_dataArray.Add(new MiscItem(47, 86, 12, 1203, 8, 43, "icon_Misc_shoushoumanaobei", 87, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, consumable: false, 0, 20, 20500, 61500, 6, 18, 10800, 8, allowRandomCreate: true, 0, isSpecial: true, -1, -1, 1200, -1, 0, 0, 0, -1, canUseOnPrepareCombat: false, allowUseInPlayAndTest: false, -1, 60, new List<short>(), new List<int>(), EMiscGenerateType.Invalid, new List<TreasureStateInfo>(), EMiscFilterType.WesternPresent, -1, -1, -1, 0));
		_dataArray.Add(new MiscItem(48, 88, 12, 1203, 4, 48, "icon_Misc_suoluohuachong", 89, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, consumable: false, 0, 10, 3100, 9300, 2, 10, 4200, 7, allowRandomCreate: true, 0, isSpecial: true, -1, -1, 1200, -1, 0, 0, 0, -1, canUseOnPrepareCombat: false, allowUseInPlayAndTest: false, -1, 60, new List<short>(), new List<int>(), EMiscGenerateType.Invalid, new List<TreasureStateInfo>(), EMiscFilterType.WesternPresent, -1, -1, -1, 0));
		_dataArray.Add(new MiscItem(49, 90, 12, 1203, 5, 48, "icon_Misc_guiyehuaguan", 91, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, consumable: false, 0, 10, 5600, 16800, 3, 12, 5400, 7, allowRandomCreate: true, 0, isSpecial: true, -1, -1, 1200, -1, 0, 0, 0, -1, canUseOnPrepareCombat: false, allowUseInPlayAndTest: false, -1, 60, new List<short>(), new List<int>(), EMiscGenerateType.Invalid, new List<TreasureStateInfo>(), EMiscFilterType.WesternPresent, -1, -1, -1, 0));
		_dataArray.Add(new MiscItem(50, 92, 12, 1203, 6, 48, "icon_Misc_luweidi", 93, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, consumable: false, 0, 10, 9200, 27600, 4, 14, 7200, 8, allowRandomCreate: true, 0, isSpecial: true, -1, -1, 1200, -1, 0, 0, 0, -1, canUseOnPrepareCombat: false, allowUseInPlayAndTest: false, -1, 60, new List<short>(), new List<int>(), EMiscGenerateType.Invalid, new List<TreasureStateInfo>(), EMiscFilterType.WesternPresent, -1, -1, -1, 0));
		_dataArray.Add(new MiscItem(51, 94, 12, 1203, 7, 48, "icon_Misc_suopalizuosirong", 95, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, consumable: false, 0, 30, 14100, 42300, 5, 16, 9000, 8, allowRandomCreate: true, 0, isSpecial: true, -1, -1, 1200, -1, 0, 0, 0, -1, canUseOnPrepareCombat: false, allowUseInPlayAndTest: false, -1, 60, new List<short>(), new List<int>(), EMiscGenerateType.Invalid, new List<TreasureStateInfo>(), EMiscFilterType.WesternPresent, -1, -1, -1, 0));
		_dataArray.Add(new MiscItem(52, 96, 12, 1203, 8, 48, "icon_Misc_huohuanbu", 97, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, consumable: false, 0, 30, 20500, 61500, 6, 18, 10800, 8, allowRandomCreate: true, 0, isSpecial: true, -1, -1, 1200, -1, 0, 0, 0, -1, canUseOnPrepareCombat: false, allowUseInPlayAndTest: false, -1, 60, new List<short>(), new List<int>(), EMiscGenerateType.Invalid, new List<TreasureStateInfo>(), EMiscFilterType.WesternPresent, -1, -1, -1, 0));
		_dataArray.Add(new MiscItem(53, 98, 12, 1203, 4, 53, "icon_Misc_foya", 99, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, consumable: false, 0, 10, 3100, 9300, 2, 10, 4200, 7, allowRandomCreate: true, 0, isSpecial: true, -1, -1, 1200, -1, 0, 0, 0, -1, canUseOnPrepareCombat: false, allowUseInPlayAndTest: false, -1, 60, new List<short>(), new List<int>(), EMiscGenerateType.Invalid, new List<TreasureStateInfo>(), EMiscFilterType.WesternPresent, -1, -1, -1, 0));
		_dataArray.Add(new MiscItem(54, 100, 12, 1203, 5, 53, "icon_Misc_baihuojiaohuozhong", 101, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, consumable: false, 0, 10, 5600, 16800, 3, 12, 5400, 7, allowRandomCreate: true, 0, isSpecial: true, -1, -1, 1200, -1, 0, 0, 0, -1, canUseOnPrepareCombat: false, allowUseInPlayAndTest: false, -1, 60, new List<short>(), new List<int>(), EMiscGenerateType.Invalid, new List<TreasureStateInfo>(), EMiscFilterType.WesternPresent, -1, -1, -1, 0));
		_dataArray.Add(new MiscItem(55, 102, 12, 1203, 6, 53, "icon_Misc_xidaduokuxingxiang", 103, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, consumable: false, 0, 100, 9200, 27600, 4, 14, 7200, 8, allowRandomCreate: true, 0, isSpecial: true, -1, -1, 1200, -1, 0, 0, 0, -1, canUseOnPrepareCombat: false, allowUseInPlayAndTest: false, -1, 60, new List<short>(), new List<int>(), EMiscGenerateType.Invalid, new List<TreasureStateInfo>(), EMiscFilterType.WesternPresent, -1, -1, -1, 0));
		_dataArray.Add(new MiscItem(56, 104, 12, 1203, 7, 53, "icon_Misc_dazhujiaodewangguan", 105, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, consumable: false, 0, 40, 14100, 42300, 5, 16, 9000, 8, allowRandomCreate: true, 0, isSpecial: true, -1, -1, 1200, -1, 0, 0, 0, -1, canUseOnPrepareCombat: false, allowUseInPlayAndTest: false, -1, 60, new List<short>(), new List<int>(), EMiscGenerateType.Invalid, new List<TreasureStateInfo>(), EMiscFilterType.WesternPresent, -1, -1, -1, 0));
		_dataArray.Add(new MiscItem(57, 106, 12, 1203, 8, 53, "icon_Misc_jinshenguhui", 107, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, consumable: false, 0, 10, 20500, 61500, 6, 18, 10800, 8, allowRandomCreate: true, 0, isSpecial: true, -1, -1, 1200, -1, 0, 0, 0, -1, canUseOnPrepareCombat: false, allowUseInPlayAndTest: false, -1, 60, new List<short>(), new List<int>(), EMiscGenerateType.Invalid, new List<TreasureStateInfo>(), EMiscFilterType.WesternPresent, -1, -1, -1, 0));
		_dataArray.Add(new MiscItem(58, 108, 12, 1203, 4, 58, "icon_Misc_baoshi", 109, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, consumable: false, 0, 30, 3100, 9300, 2, 10, 4200, 7, allowRandomCreate: true, 0, isSpecial: true, -1, -1, 1200, -1, 0, 0, 0, -1, canUseOnPrepareCombat: false, allowUseInPlayAndTest: false, -1, 60, new List<short>(), new List<int>(), EMiscGenerateType.Invalid, new List<TreasureStateInfo>(), EMiscFilterType.WesternPresent, -1, -1, -1, 0));
		_dataArray.Add(new MiscItem(59, 110, 12, 1203, 5, 58, "icon_Misc_jingcunzhu", 111, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, consumable: false, 0, 10, 5600, 16800, 3, 12, 5400, 7, allowRandomCreate: true, 0, isSpecial: true, -1, -1, 1200, -1, 0, 0, 0, -1, canUseOnPrepareCombat: false, allowUseInPlayAndTest: false, -1, 60, new List<short>(), new List<int>(), EMiscGenerateType.Invalid, new List<TreasureStateInfo>(), EMiscFilterType.WesternPresent, -1, -1, -1, 0));
	}

	private void CreateItems1()
	{
		_dataArray.Add(new MiscItem(60, 112, 12, 1203, 6, 58, "icon_Misc_yutianyu", 113, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, consumable: false, 0, 30, 9200, 27600, 4, 14, 7200, 8, allowRandomCreate: true, 0, isSpecial: true, -1, -1, 1200, -1, 0, 0, 0, -1, canUseOnPrepareCombat: false, allowUseInPlayAndTest: false, -1, 60, new List<short>(), new List<int>(), EMiscGenerateType.Invalid, new List<TreasureStateInfo>(), EMiscFilterType.WesternPresent, -1, -1, -1, 0));
		_dataArray.Add(new MiscItem(61, 114, 12, 1203, 7, 58, "icon_Misc_yangsuizhu", 115, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, consumable: false, 0, 10, 14100, 42300, 5, 16, 9000, 8, allowRandomCreate: true, 0, isSpecial: true, -1, -1, 1200, -1, 0, 0, 0, -1, canUseOnPrepareCombat: false, allowUseInPlayAndTest: false, -1, 60, new List<short>(), new List<int>(), EMiscGenerateType.Invalid, new List<TreasureStateInfo>(), EMiscFilterType.WesternPresent, -1, -1, -1, 0));
		_dataArray.Add(new MiscItem(62, 116, 12, 1203, 8, 58, "icon_Misc_zuanshi", 117, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, consumable: false, 0, 20, 20500, 61500, 6, 18, 10800, 8, allowRandomCreate: true, 0, isSpecial: true, -1, -1, 1200, -1, 0, 0, 0, -1, canUseOnPrepareCombat: false, allowUseInPlayAndTest: false, -1, 60, new List<short>(), new List<int>(), EMiscGenerateType.Invalid, new List<TreasureStateInfo>(), EMiscFilterType.WesternPresent, -1, -1, -1, 0));
		_dataArray.Add(new MiscItem(63, 118, 12, 1203, 4, 63, "icon_Misc_miandai", 119, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, consumable: false, 0, 10, 3100, 9300, 2, 10, 4200, 7, allowRandomCreate: true, 0, isSpecial: true, -1, -1, 1200, -1, 0, 0, 0, -1, canUseOnPrepareCombat: false, allowUseInPlayAndTest: false, -1, 60, new List<short>(), new List<int>(), EMiscGenerateType.Invalid, new List<TreasureStateInfo>(), EMiscFilterType.WesternPresent, -1, -1, -1, 0));
		_dataArray.Add(new MiscItem(64, 120, 12, 1203, 5, 63, "icon_Misc_bosiliujinyinhu", 121, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, consumable: false, 0, 40, 5600, 16800, 3, 12, 5400, 7, allowRandomCreate: true, 0, isSpecial: true, -1, -1, 1200, -1, 0, 0, 0, -1, canUseOnPrepareCombat: false, allowUseInPlayAndTest: false, -1, 60, new List<short>(), new List<int>(), EMiscGenerateType.Invalid, new List<TreasureStateInfo>(), EMiscFilterType.WesternPresent, -1, -1, -1, 0));
		_dataArray.Add(new MiscItem(65, 122, 12, 1203, 6, 63, "icon_Misc_sichouhuangfu", 123, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, consumable: false, 0, 40, 9200, 27600, 4, 14, 7200, 8, allowRandomCreate: true, 0, isSpecial: true, -1, -1, 1200, -1, 0, 0, 0, -1, canUseOnPrepareCombat: false, allowUseInPlayAndTest: false, -1, 60, new List<short>(), new List<int>(), EMiscGenerateType.Invalid, new List<TreasureStateInfo>(), EMiscFilterType.WesternPresent, -1, -1, -1, 0));
		_dataArray.Add(new MiscItem(66, 124, 12, 1203, 7, 63, "icon_Misc_jinyuxianghushenfu", 125, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, consumable: false, 0, 10, 14100, 42300, 5, 16, 9000, 8, allowRandomCreate: true, 0, isSpecial: true, -1, -1, 1200, -1, 0, 0, 0, -1, canUseOnPrepareCombat: false, allowUseInPlayAndTest: false, -1, 60, new List<short>(), new List<int>(), EMiscGenerateType.Invalid, new List<TreasureStateInfo>(), EMiscFilterType.WesternPresent, -1, -1, -1, 0));
		_dataArray.Add(new MiscItem(67, 126, 12, 1203, 8, 63, "icon_Misc_guanghuiwangguan", 127, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, consumable: false, 0, 40, 20500, 61500, 6, 18, 10800, 8, allowRandomCreate: true, 0, isSpecial: true, -1, -1, 1200, -1, 0, 0, 0, -1, canUseOnPrepareCombat: false, allowUseInPlayAndTest: false, -1, 60, new List<short>(), new List<int>(), EMiscGenerateType.Invalid, new List<TreasureStateInfo>(), EMiscFilterType.WesternPresent, -1, -1, -1, 0));
		_dataArray.Add(new MiscItem(68, 128, 12, 1203, 4, 68, "icon_Misc_dunhuangxingtu", 129, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, consumable: false, 0, 10, 3100, 9300, 2, 10, 4200, 7, allowRandomCreate: true, 0, isSpecial: true, -1, -1, 1200, -1, 0, 0, 0, -1, canUseOnPrepareCombat: false, allowUseInPlayAndTest: false, -1, 60, new List<short>(), new List<int>(), EMiscGenerateType.Invalid, new List<TreasureStateInfo>(), EMiscFilterType.WesternPresent, -1, -1, -1, 0));
		_dataArray.Add(new MiscItem(69, 130, 12, 1203, 5, 68, "icon_Misc_yangpishengjing", 131, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, consumable: false, 0, 20, 5600, 16800, 3, 12, 5400, 7, allowRandomCreate: true, 0, isSpecial: true, -1, -1, 1200, -1, 0, 0, 0, -1, canUseOnPrepareCombat: false, allowUseInPlayAndTest: false, -1, 60, new List<short>(), new List<int>(), EMiscGenerateType.Invalid, new List<TreasureStateInfo>(), EMiscFilterType.WesternPresent, -1, -1, -1, 0));
		_dataArray.Add(new MiscItem(70, 132, 12, 1203, 6, 68, "icon_Misc_manufadian", 133, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, consumable: false, 0, 40, 9200, 27600, 4, 14, 7200, 8, allowRandomCreate: true, 0, isSpecial: true, -1, -1, 1200, -1, 0, 0, 0, -1, canUseOnPrepareCombat: false, allowUseInPlayAndTest: false, -1, 60, new List<short>(), new List<int>(), EMiscGenerateType.Invalid, new List<TreasureStateInfo>(), EMiscFilterType.WesternPresent, -1, -1, -1, 0));
		_dataArray.Add(new MiscItem(71, 134, 12, 1203, 7, 68, "icon_Misc_hemashishi", 135, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, consumable: false, 0, 30, 14100, 42300, 5, 16, 9000, 8, allowRandomCreate: true, 0, isSpecial: true, -1, -1, 1200, -1, 0, 0, 0, -1, canUseOnPrepareCombat: false, allowUseInPlayAndTest: false, -1, 60, new List<short>(), new List<int>(), EMiscGenerateType.Invalid, new List<TreasureStateInfo>(), EMiscFilterType.WesternPresent, -1, -1, -1, 0));
		_dataArray.Add(new MiscItem(72, 136, 12, 1203, 8, 68, "icon_Misc_sihaiwenshu", 137, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, consumable: false, 0, 40, 20500, 61500, 6, 18, 10800, 8, allowRandomCreate: true, 0, isSpecial: true, -1, -1, 1200, -1, 0, 0, 0, -1, canUseOnPrepareCombat: false, allowUseInPlayAndTest: false, -1, 60, new List<short>(), new List<int>(), EMiscGenerateType.Invalid, new List<TreasureStateInfo>(), EMiscFilterType.WesternPresent, -1, -1, -1, 0));
		_dataArray.Add(new MiscItem(73, 138, 12, 1206, 0, 73, "icon_Misc_changsheng", 139, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, consumable: true, 0, 30, 150, 300, 0, 1, 600, 3, allowRandomCreate: true, 45, isSpecial: false, 4, 181, 36, -1, 0, 0, 20, 1, canUseOnPrepareCombat: false, allowUseInPlayAndTest: false, 80, 60, new List<short>(), new List<int>(), EMiscGenerateType.Invalid, new List<TreasureStateInfo>(), EMiscFilterType.Item, 6, 4, -1, 0));
		_dataArray.Add(new MiscItem(74, 140, 12, 1206, 1, 73, "icon_Misc_funiusuo", 141, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, consumable: true, 0, 50, 300, 600, 0, 2, 1200, 4, allowRandomCreate: true, 40, isSpecial: false, 4, 181, 36, -1, 0, 0, 25, 1, canUseOnPrepareCombat: false, allowUseInPlayAndTest: false, 80, 60, new List<short>(), new List<int>(), EMiscGenerateType.Invalid, new List<TreasureStateInfo>(), EMiscFilterType.Item, 6, 4, -1, 0));
		_dataArray.Add(new MiscItem(75, 142, 12, 1206, 2, 73, "icon_Misc_wucaisheng", 143, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, consumable: true, 0, 40, 900, 1800, 1, 3, 1800, 5, allowRandomCreate: true, 35, isSpecial: false, 4, 181, 36, -1, 0, 0, 30, 1, canUseOnPrepareCombat: false, allowUseInPlayAndTest: false, 80, 60, new List<short>(), new List<int>(), EMiscGenerateType.Invalid, new List<TreasureStateInfo>(), EMiscFilterType.Item, 6, 4, -1, 0));
		_dataArray.Add(new MiscItem(76, 144, 12, 1206, 3, 73, "icon_Misc_bairensuo", 145, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, consumable: true, 0, 60, 2250, 4500, 2, 4, 3000, 6, allowRandomCreate: true, 30, isSpecial: false, 4, 181, 36, -1, 0, 0, 35, 1, canUseOnPrepareCombat: false, allowUseInPlayAndTest: false, 80, 60, new List<short>(), new List<int>(), EMiscGenerateType.Invalid, new List<TreasureStateInfo>(), EMiscFilterType.Item, 6, 4, -1, 0));
		_dataArray.Add(new MiscItem(77, 146, 12, 1206, 4, 73, "icon_Misc_zhuxiachangsuo", 147, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, consumable: true, 0, 40, 4650, 9300, 3, 5, 4200, 7, allowRandomCreate: true, 25, isSpecial: false, 4, 181, 36, -1, 0, 0, 40, 1, canUseOnPrepareCombat: false, allowUseInPlayAndTest: false, 80, 60, new List<short>(), new List<int>(), EMiscGenerateType.Invalid, new List<TreasureStateInfo>(), EMiscFilterType.Item, 6, 4, -1, 0));
		_dataArray.Add(new MiscItem(78, 148, 12, 1206, 5, 73, "icon_Misc_qianjiaosheng", 149, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, consumable: true, 0, 60, 8400, 16800, 4, 6, 5400, 7, allowRandomCreate: true, 20, isSpecial: false, 4, 181, 36, -1, 0, 0, 45, 1, canUseOnPrepareCombat: false, allowUseInPlayAndTest: false, 80, 60, new List<short>(), new List<int>(), EMiscGenerateType.Invalid, new List<TreasureStateInfo>(), EMiscFilterType.Item, 6, 4, -1, 0));
		_dataArray.Add(new MiscItem(79, 150, 12, 1206, 6, 73, "icon_Misc_wanjiechanyunsuo", 151, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, consumable: true, 0, 50, 13800, 27600, 5, 7, 7200, 8, allowRandomCreate: true, 15, isSpecial: false, 4, 181, 36, -1, 0, 0, 50, 1, canUseOnPrepareCombat: false, allowUseInPlayAndTest: false, 80, 60, new List<short>(), new List<int>(), EMiscGenerateType.Invalid, new List<TreasureStateInfo>(), EMiscFilterType.Item, 6, 4, -1, 0));
		_dataArray.Add(new MiscItem(80, 152, 12, 1206, 7, 73, "icon_Misc_wuzhongsuo", 153, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, consumable: true, 0, 60, 21150, 42300, 6, 8, 9000, 8, allowRandomCreate: true, 10, isSpecial: false, 4, 181, 36, -1, 0, 0, 60, 1, canUseOnPrepareCombat: false, allowUseInPlayAndTest: false, 80, 60, new List<short>(), new List<int>(), EMiscGenerateType.Invalid, new List<TreasureStateInfo>(), EMiscFilterType.Item, 6, 4, -1, 0));
		_dataArray.Add(new MiscItem(81, 154, 12, 1206, 8, 73, "icon_Misc_kunxiansheng", 155, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, consumable: true, 0, 40, 30750, 61500, 6, 9, 10800, 8, allowRandomCreate: true, 5, isSpecial: false, 4, 181, 36, -1, 0, 0, 70, 1, canUseOnPrepareCombat: false, allowUseInPlayAndTest: false, 80, 60, new List<short>(), new List<int>(), EMiscGenerateType.Invalid, new List<TreasureStateInfo>(), EMiscFilterType.Item, 6, 4, -1, 0));
		_dataArray.Add(new MiscItem(82, 156, 12, 1201, 0, 82, "icon_Misc_dancuzhiguan", 157, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, consumable: false, 0, 80, 100, 300, 0, 2, 600, 3, allowRandomCreate: true, 45, isSpecial: false, -1, -1, 1200, -1, 0, 0, 0, -1, canUseOnPrepareCombat: false, allowUseInPlayAndTest: false, -1, 60, new List<short>(), new List<int>(), EMiscGenerateType.Invalid, new List<TreasureStateInfo>(), EMiscFilterType.Misc, -1, -1, -1, 0));
		_dataArray.Add(new MiscItem(83, 158, 12, 1201, 1, 82, "icon_Misc_cizhicuzhiguan", 159, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, consumable: false, 0, 60, 200, 600, 0, 4, 1200, 4, allowRandomCreate: true, 40, isSpecial: false, -1, -1, 1200, -1, 0, 20, 0, -1, canUseOnPrepareCombat: false, allowUseInPlayAndTest: false, -1, 60, new List<short>(), new List<int>(), EMiscGenerateType.Invalid, new List<TreasureStateInfo>(), EMiscFilterType.Misc, -1, -1, -1, 0));
		_dataArray.Add(new MiscItem(84, 160, 12, 1201, 2, 82, "icon_Misc_zishacuzhiguan", 161, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, consumable: false, 0, 70, 600, 1800, 0, 6, 1800, 5, allowRandomCreate: true, 35, isSpecial: false, -1, -1, 1200, -1, 0, 25, 0, -1, canUseOnPrepareCombat: false, allowUseInPlayAndTest: false, -1, 60, new List<short>(), new List<int>(), EMiscGenerateType.Invalid, new List<TreasureStateInfo>(), EMiscFilterType.Misc, -1, -1, -1, 0));
		_dataArray.Add(new MiscItem(85, 162, 12, 1201, 3, 82, "icon_Misc_bainicuzhiguan", 163, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, consumable: false, 0, 70, 1500, 4500, 1, 8, 3000, 6, allowRandomCreate: true, 30, isSpecial: false, -1, -1, 1200, -1, 0, 35, 0, -1, canUseOnPrepareCombat: false, allowUseInPlayAndTest: false, -1, 60, new List<short>(), new List<int>(), EMiscGenerateType.Invalid, new List<TreasureStateInfo>(), EMiscFilterType.Misc, -1, -1, -1, 0));
		_dataArray.Add(new MiscItem(86, 164, 12, 1201, 4, 82, "icon_Misc_laoqingzhuancuzhiguan", 165, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, consumable: false, 0, 90, 3100, 9300, 2, 10, 4200, 7, allowRandomCreate: true, 25, isSpecial: false, -1, -1, 1200, -1, 0, 45, 0, -1, canUseOnPrepareCombat: false, allowUseInPlayAndTest: false, -1, 60, new List<short>(), new List<int>(), EMiscGenerateType.Invalid, new List<TreasureStateInfo>(), EMiscFilterType.Misc, -1, -1, -1, 0));
		_dataArray.Add(new MiscItem(87, 166, 12, 1201, 5, 82, "icon_Misc_moyucuzhiguan", 167, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, consumable: false, 0, 80, 5600, 16800, 3, 12, 5400, 7, allowRandomCreate: true, 20, isSpecial: false, -1, -1, 1200, -1, 0, 55, 0, -1, canUseOnPrepareCombat: false, allowUseInPlayAndTest: false, -1, 60, new List<short>(), new List<int>(), EMiscGenerateType.Invalid, new List<TreasureStateInfo>(), EMiscFilterType.Misc, -1, -1, -1, 0));
		_dataArray.Add(new MiscItem(88, 168, 12, 1201, 6, 82, "icon_Misc_yunisajincuzhiguan", 169, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, consumable: false, 0, 70, 9200, 27600, 4, 14, 7200, 8, allowRandomCreate: true, 15, isSpecial: false, -1, -1, 1200, -1, 0, 70, 0, -1, canUseOnPrepareCombat: false, allowUseInPlayAndTest: false, -1, 60, new List<short>(), new List<int>(), EMiscGenerateType.Invalid, new List<TreasureStateInfo>(), EMiscFilterType.Misc, -1, -1, -1, 0));
		_dataArray.Add(new MiscItem(89, 170, 12, 1201, 7, 82, "icon_Misc_longxinglushacuzhiguan", 171, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, consumable: false, 0, 60, 14100, 42300, 5, 16, 9000, 8, allowRandomCreate: true, 10, isSpecial: false, -1, -1, 1200, -1, 0, 85, 0, -1, canUseOnPrepareCombat: false, allowUseInPlayAndTest: false, -1, 60, new List<short>(), new List<int>(), EMiscGenerateType.Invalid, new List<TreasureStateInfo>(), EMiscFilterType.Misc, -1, -1, -1, 0));
		_dataArray.Add(new MiscItem(90, 172, 12, 1201, 8, 82, "icon_Misc_bainiandengnicuzhiguan", 173, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, consumable: false, 0, 80, 20500, 61500, 6, 18, 10800, 8, allowRandomCreate: true, 5, isSpecial: false, -1, -1, 1200, -1, 0, 100, 0, -1, canUseOnPrepareCombat: false, allowUseInPlayAndTest: false, -1, 60, new List<short>(), new List<int>(), EMiscGenerateType.Invalid, new List<TreasureStateInfo>(), EMiscFilterType.Misc, -1, -1, -1, 0));
		_dataArray.Add(new MiscItem(91, 174, 12, 1200, 0, -1, "icon_Misc_yanduyinzhen", 175, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, consumable: false, 0, 10, 100, 300, 0, 1, 600, 3, allowRandomCreate: true, 45, isSpecial: false, -1, -1, 36, -1, 0, 0, 0, -1, canUseOnPrepareCombat: false, allowUseInPlayAndTest: false, -1, 60, new List<short>(), new List<int>(), EMiscGenerateType.Invalid, new List<TreasureStateInfo>(), EMiscFilterType.Item, -1, -1, -1, 0));
		_dataArray.Add(new MiscItem(92, 176, 12, 1205, 3, 92, "icon_Misc_jinggangsuokou", 177, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, consumable: false, 0, 900, 1500, 15000, 1, 4, 1500, 6, allowRandomCreate: true, 30, isSpecial: false, -1, -1, 120, -1, 0, 0, 0, -1, canUseOnPrepareCombat: false, allowUseInPlayAndTest: false, -1, 60, new List<short>(), new List<int> { 2, 3, 4 }, EMiscGenerateType.Any, new List<TreasureStateInfo>
		{
			new TreasureStateInfo(9, 3),
			new TreasureStateInfo(6, 2),
			new TreasureStateInfo(14, 2),
			new TreasureStateInfo(15, 1),
			new TreasureStateInfo(3, 1),
			new TreasureStateInfo(7, 1)
		}, EMiscFilterType.KeyItem, -1, -1, -1, 0));
		_dataArray.Add(new MiscItem(93, 178, 12, 1205, 3, 92, "icon_Misc_heihuoyao", 179, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, consumable: false, 0, 1200, 1500, 15000, 1, 4, 1500, 6, allowRandomCreate: true, 30, isSpecial: false, -1, -1, 120, -1, 0, 0, 0, -1, canUseOnPrepareCombat: false, allowUseInPlayAndTest: false, -1, 60, new List<short>(), new List<int> { 2, 3, 4 }, EMiscGenerateType.Any, new List<TreasureStateInfo>
		{
			new TreasureStateInfo(9, 3),
			new TreasureStateInfo(6, 2),
			new TreasureStateInfo(14, 2),
			new TreasureStateInfo(15, 1),
			new TreasureStateInfo(3, 1),
			new TreasureStateInfo(7, 1)
		}, EMiscFilterType.KeyItem, -1, -1, -1, 0));
		_dataArray.Add(new MiscItem(94, 180, 12, 1205, 3, 92, "icon_Misc_bainianshaxin", 181, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, consumable: false, 0, 1200, 1500, 15000, 1, 4, 1500, 6, allowRandomCreate: true, 30, isSpecial: false, -1, -1, 120, -1, 0, 0, 0, -1, canUseOnPrepareCombat: false, allowUseInPlayAndTest: false, -1, 60, new List<short>(), new List<int> { 2, 3, 4 }, EMiscGenerateType.Any, new List<TreasureStateInfo>
		{
			new TreasureStateInfo(9, 3),
			new TreasureStateInfo(6, 2),
			new TreasureStateInfo(12, 2),
			new TreasureStateInfo(14, 1),
			new TreasureStateInfo(3, 1),
			new TreasureStateInfo(7, 1)
		}, EMiscFilterType.KeyItem, -1, -1, -1, 0));
		_dataArray.Add(new MiscItem(95, 182, 12, 1205, 3, 92, "icon_Misc_jinganglun", 183, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, consumable: false, 0, 900, 1500, 15000, 1, 4, 1500, 6, allowRandomCreate: true, 30, isSpecial: false, -1, -1, 120, -1, 0, 0, 0, -1, canUseOnPrepareCombat: false, allowUseInPlayAndTest: false, -1, 60, new List<short>(), new List<int> { 2, 3, 4 }, EMiscGenerateType.Any, new List<TreasureStateInfo>
		{
			new TreasureStateInfo(9, 3),
			new TreasureStateInfo(6, 2),
			new TreasureStateInfo(12, 2),
			new TreasureStateInfo(14, 1),
			new TreasureStateInfo(3, 1),
			new TreasureStateInfo(7, 1)
		}, EMiscFilterType.KeyItem, -1, -1, -1, 0));
		_dataArray.Add(new MiscItem(96, 184, 12, 1205, 3, 92, "icon_Misc_yutaoguan", 185, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, consumable: false, 0, 900, 1500, 15000, 1, 4, 1500, 6, allowRandomCreate: true, 30, isSpecial: false, -1, -1, 120, -1, 0, 0, 0, -1, canUseOnPrepareCombat: false, allowUseInPlayAndTest: false, -1, 60, new List<short>(), new List<int> { 2, 3, 4 }, EMiscGenerateType.Any, new List<TreasureStateInfo>
		{
			new TreasureStateInfo(3, 3),
			new TreasureStateInfo(10, 3),
			new TreasureStateInfo(1, 1),
			new TreasureStateInfo(12, 1),
			new TreasureStateInfo(15, 1),
			new TreasureStateInfo(5, 1)
		}, EMiscFilterType.KeyItem, -1, -1, -1, 0));
		_dataArray.Add(new MiscItem(97, 186, 12, 1205, 3, 92, "icon_Misc_sishelong", 187, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, consumable: false, 0, 600, 1500, 15000, 1, 4, 1500, 6, allowRandomCreate: true, 30, isSpecial: false, -1, -1, 120, -1, 0, 0, 0, -1, canUseOnPrepareCombat: false, allowUseInPlayAndTest: false, -1, 60, new List<short>(), new List<int> { 2, 3, 4 }, EMiscGenerateType.Any, new List<TreasureStateInfo>
		{
			new TreasureStateInfo(12, 3),
			new TreasureStateInfo(10, 3),
			new TreasureStateInfo(13, 1),
			new TreasureStateInfo(3, 1),
			new TreasureStateInfo(15, 1),
			new TreasureStateInfo(5, 1)
		}, EMiscFilterType.KeyItem, -1, -1, -1, 0));
		_dataArray.Add(new MiscItem(98, 188, 12, 1205, 3, 92, "icon_Misc_yinxianbaipeng", 189, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, consumable: false, 0, 900, 1500, 15000, 1, 4, 1500, 6, allowRandomCreate: true, 30, isSpecial: false, -1, -1, 120, -1, 0, 0, 0, -1, canUseOnPrepareCombat: false, allowUseInPlayAndTest: false, -1, 60, new List<short>(), new List<int> { 2, 3, 4 }, EMiscGenerateType.Any, new List<TreasureStateInfo>
		{
			new TreasureStateInfo(3, 3),
			new TreasureStateInfo(9, 2),
			new TreasureStateInfo(10, 2),
			new TreasureStateInfo(2, 1),
			new TreasureStateInfo(8, 1),
			new TreasureStateInfo(11, 1)
		}, EMiscFilterType.KeyItem, -1, -1, -1, 0));
		_dataArray.Add(new MiscItem(99, 190, 12, 1205, 3, 92, "icon_Misc_fengshuiqidan", 191, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, consumable: false, 0, 600, 1500, 15000, 1, 4, 1500, 6, allowRandomCreate: true, 30, isSpecial: false, -1, -1, 120, -1, 0, 0, 0, -1, canUseOnPrepareCombat: false, allowUseInPlayAndTest: false, -1, 60, new List<short>(), new List<int> { 2, 3, 4 }, EMiscGenerateType.Any, new List<TreasureStateInfo>
		{
			new TreasureStateInfo(3, 3),
			new TreasureStateInfo(9, 2),
			new TreasureStateInfo(10, 2),
			new TreasureStateInfo(12, 1),
			new TreasureStateInfo(14, 1),
			new TreasureStateInfo(4, 1)
		}, EMiscFilterType.KeyItem, -1, -1, -1, 0));
		_dataArray.Add(new MiscItem(100, 192, 12, 1205, 3, 92, "icon_Misc_zhenyuanbixieshou", 193, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, consumable: false, 0, 900, 1500, 15000, 1, 4, 1500, 6, allowRandomCreate: true, 30, isSpecial: false, -1, -1, 120, -1, 0, 0, 0, -1, canUseOnPrepareCombat: false, allowUseInPlayAndTest: false, -1, 60, new List<short>(), new List<int> { 2, 3, 4 }, EMiscGenerateType.Any, new List<TreasureStateInfo>
		{
			new TreasureStateInfo(7, 3),
			new TreasureStateInfo(9, 2),
			new TreasureStateInfo(10, 2),
			new TreasureStateInfo(13, 1),
			new TreasureStateInfo(15, 1),
			new TreasureStateInfo(1, 1)
		}, EMiscFilterType.KeyItem, -1, -1, -1, 0));
		_dataArray.Add(new MiscItem(101, 194, 12, 1205, 3, 92, "icon_Misc_wudanju", 195, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, consumable: false, 0, 1200, 1500, 15000, 1, 4, 1500, 6, allowRandomCreate: true, 30, isSpecial: false, -1, -1, 120, -1, 0, 0, 0, -1, canUseOnPrepareCombat: false, allowUseInPlayAndTest: false, -1, 60, new List<short>(), new List<int> { 2, 3, 4 }, EMiscGenerateType.Any, new List<TreasureStateInfo>
		{
			new TreasureStateInfo(7, 3),
			new TreasureStateInfo(9, 2),
			new TreasureStateInfo(10, 2),
			new TreasureStateInfo(8, 1),
			new TreasureStateInfo(6, 1),
			new TreasureStateInfo(5, 1)
		}, EMiscFilterType.KeyItem, -1, -1, -1, 0));
		_dataArray.Add(new MiscItem(102, 196, 12, 1205, 5, 102, "icon_Misc_jueyindan", 197, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, consumable: false, 0, 200, 5600, 56000, 3, 6, 2700, 7, allowRandomCreate: true, 20, isSpecial: false, -1, -1, 120, -1, 0, 0, 0, -1, canUseOnPrepareCombat: false, allowUseInPlayAndTest: false, -1, 60, new List<short>(), new List<int> { 4, 5, 6 }, EMiscGenerateType.Any, new List<TreasureStateInfo>
		{
			new TreasureStateInfo(1, 1),
			new TreasureStateInfo(2, 1),
			new TreasureStateInfo(8, 1),
			new TreasureStateInfo(11, 1),
			new TreasureStateInfo(15, 1)
		}, EMiscFilterType.KeyItem, -1, -1, -1, 0));
		_dataArray.Add(new MiscItem(103, 198, 12, 1205, 5, 102, "icon_Misc_qilinzao", 199, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, consumable: false, 0, 300, 5600, 56000, 3, 6, 2700, 7, allowRandomCreate: true, 20, isSpecial: false, -1, -1, 120, -1, 0, 0, 0, -1, canUseOnPrepareCombat: false, allowUseInPlayAndTest: false, -1, 60, new List<short>(), new List<int> { 4, 5, 6 }, EMiscGenerateType.Any, new List<TreasureStateInfo>
		{
			new TreasureStateInfo(4, 2),
			new TreasureStateInfo(13, 2),
			new TreasureStateInfo(3, 1)
		}, EMiscFilterType.KeyItem, -1, -1, -1, 0));
		_dataArray.Add(new MiscItem(104, 200, 12, 1205, 5, 102, "icon_Misc_fengkongbaiji", 201, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, consumable: false, 0, 250, 5600, 56000, 3, 6, 2700, 7, allowRandomCreate: true, 20, isSpecial: false, -1, -1, 120, -1, 0, 0, 0, -1, canUseOnPrepareCombat: false, allowUseInPlayAndTest: false, -1, 60, new List<short>(), new List<int> { 4, 5, 6 }, EMiscGenerateType.Any, new List<TreasureStateInfo>
		{
			new TreasureStateInfo(1, 1),
			new TreasureStateInfo(2, 1),
			new TreasureStateInfo(3, 1),
			new TreasureStateInfo(10, 1),
			new TreasureStateInfo(12, 1),
			new TreasureStateInfo(15, 1)
		}, EMiscFilterType.KeyItem, -1, -1, -1, 0));
		_dataArray.Add(new MiscItem(105, 202, 12, 1205, 5, 102, "icon_Misc_sihulong", 203, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, consumable: false, 0, 1200, 5600, 56000, 3, 6, 2700, 7, allowRandomCreate: true, 20, isSpecial: false, -1, -1, 120, -1, 0, 0, 0, -1, canUseOnPrepareCombat: false, allowUseInPlayAndTest: false, -1, 60, new List<short>(), new List<int> { 4, 5, 6 }, EMiscGenerateType.Any, new List<TreasureStateInfo>
		{
			new TreasureStateInfo(4, 1),
			new TreasureStateInfo(6, 1),
			new TreasureStateInfo(14, 1),
			new TreasureStateInfo(1, 1),
			new TreasureStateInfo(11, 1),
			new TreasureStateInfo(12, 1),
			new TreasureStateInfo(15, 1)
		}, EMiscFilterType.KeyItem, -1, -1, -1, 0));
		_dataArray.Add(new MiscItem(106, 204, 12, 1205, 5, 102, "icon_Misc_changshengjian", 205, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, consumable: false, 0, 100, 5600, 56000, 3, 6, 2700, 7, allowRandomCreate: true, 20, isSpecial: false, -1, -1, 120, -1, 0, 0, 0, -1, canUseOnPrepareCombat: false, allowUseInPlayAndTest: false, -1, 60, new List<short>(), new List<int> { 4, 5, 6 }, EMiscGenerateType.Any, new List<TreasureStateInfo>
		{
			new TreasureStateInfo(2, 1),
			new TreasureStateInfo(3, 1),
			new TreasureStateInfo(8, 1),
			new TreasureStateInfo(12, 1),
			new TreasureStateInfo(13, 1)
		}, EMiscFilterType.KeyItem, -1, -1, -1, 0));
		_dataArray.Add(new MiscItem(107, 206, 12, 1205, 5, 102, "icon_Misc_longgujingou", 207, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, consumable: false, 0, 600, 5600, 56000, 3, 6, 2700, 7, allowRandomCreate: true, 20, isSpecial: false, -1, -1, 120, -1, 0, 0, 0, -1, canUseOnPrepareCombat: false, allowUseInPlayAndTest: false, -1, 60, new List<short>(), new List<int> { 4, 5, 6 }, EMiscGenerateType.Any, new List<TreasureStateInfo>
		{
			new TreasureStateInfo(5, 2),
			new TreasureStateInfo(10, 1),
			new TreasureStateInfo(15, 1)
		}, EMiscFilterType.KeyItem, -1, -1, -1, 0));
		_dataArray.Add(new MiscItem(108, 208, 12, 1205, 5, 102, "icon_Misc_jueguangbi", 209, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, consumable: false, 0, 900, 5600, 56000, 3, 6, 2700, 7, allowRandomCreate: true, 20, isSpecial: false, -1, -1, 120, -1, 0, 0, 0, -1, canUseOnPrepareCombat: false, allowUseInPlayAndTest: false, -1, 60, new List<short>(), new List<int> { 4, 5, 6 }, EMiscGenerateType.Any, new List<TreasureStateInfo>
		{
			new TreasureStateInfo(10, 2),
			new TreasureStateInfo(13, 2),
			new TreasureStateInfo(14, 1),
			new TreasureStateInfo(15, 1)
		}, EMiscFilterType.KeyItem, -1, -1, -1, 0));
		_dataArray.Add(new MiscItem(109, 210, 12, 1205, 5, 102, "icon_Misc_zangjiandan", 211, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, consumable: false, 0, 600, 5600, 56000, 3, 6, 2700, 7, allowRandomCreate: true, 20, isSpecial: false, -1, -1, 120, -1, 0, 0, 0, -1, canUseOnPrepareCombat: false, allowUseInPlayAndTest: false, -1, 60, new List<short>(), new List<int> { 4, 5, 6 }, EMiscGenerateType.Any, new List<TreasureStateInfo>
		{
			new TreasureStateInfo(4, 1),
			new TreasureStateInfo(7, 1),
			new TreasureStateInfo(5, 1),
			new TreasureStateInfo(9, 1),
			new TreasureStateInfo(13, 1)
		}, EMiscFilterType.KeyItem, -1, -1, -1, 0));
		_dataArray.Add(new MiscItem(110, 212, 12, 1205, 5, 102, "icon_Misc_zhendaodan", 213, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, consumable: false, 0, 900, 5600, 56000, 3, 6, 2700, 7, allowRandomCreate: true, 20, isSpecial: false, -1, -1, 120, -1, 0, 0, 0, -1, canUseOnPrepareCombat: false, allowUseInPlayAndTest: false, -1, 60, new List<short>(), new List<int> { 4, 5, 6 }, EMiscGenerateType.Any, new List<TreasureStateInfo>
		{
			new TreasureStateInfo(6, 1),
			new TreasureStateInfo(11, 1),
			new TreasureStateInfo(5, 1),
			new TreasureStateInfo(9, 1),
			new TreasureStateInfo(14, 1)
		}, EMiscFilterType.KeyItem, -1, -1, -1, 0));
		_dataArray.Add(new MiscItem(111, 214, 12, 1205, 5, 102, "icon_Misc_pozhentieju", 215, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, consumable: false, 0, 1500, 5600, 56000, 3, 6, 2700, 7, allowRandomCreate: true, 20, isSpecial: false, -1, -1, 120, -1, 0, 0, 0, -1, canUseOnPrepareCombat: false, allowUseInPlayAndTest: false, -1, 60, new List<short>(), new List<int> { 4, 5, 6 }, EMiscGenerateType.Any, new List<TreasureStateInfo>
		{
			new TreasureStateInfo(1, 2),
			new TreasureStateInfo(6, 1),
			new TreasureStateInfo(9, 1)
		}, EMiscFilterType.KeyItem, -1, -1, -1, 0));
		_dataArray.Add(new MiscItem(112, 216, 12, 1205, 5, 102, "icon_Misc_zhenyandan", 217, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, consumable: false, 0, 500, 5600, 56000, 3, 6, 2700, 7, allowRandomCreate: true, 20, isSpecial: false, -1, -1, 120, -1, 0, 0, 0, -1, canUseOnPrepareCombat: false, allowUseInPlayAndTest: false, -1, 60, new List<short>(), new List<int> { 4, 5, 6 }, EMiscGenerateType.Any, new List<TreasureStateInfo>
		{
			new TreasureStateInfo(2, 1),
			new TreasureStateInfo(7, 2),
			new TreasureStateInfo(11, 1)
		}, EMiscFilterType.KeyItem, -1, -1, -1, 0));
		_dataArray.Add(new MiscItem(113, 218, 12, 1205, 5, 102, "icon_Misc_baizhuanruansuo", 219, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, consumable: false, 0, 200, 5600, 56000, 3, 6, 2700, 7, allowRandomCreate: true, 20, isSpecial: false, -1, -1, 120, -1, 0, 0, 0, -1, canUseOnPrepareCombat: false, allowUseInPlayAndTest: false, -1, 60, new List<short>(), new List<int> { 4, 5, 6 }, EMiscGenerateType.Any, new List<TreasureStateInfo>
		{
			new TreasureStateInfo(4, 2),
			new TreasureStateInfo(12, 2)
		}, EMiscFilterType.KeyItem, -1, -1, -1, 0));
		_dataArray.Add(new MiscItem(114, 220, 12, 1205, 5, 102, "icon_Misc_zhuiguangbaozhu", 221, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, consumable: false, 0, 100, 5600, 56000, 3, 6, 2700, 7, allowRandomCreate: true, 20, isSpecial: false, -1, -1, 120, -1, 0, 0, 0, -1, canUseOnPrepareCombat: false, allowUseInPlayAndTest: false, -1, 60, new List<short>(), new List<int> { 4, 5, 6 }, EMiscGenerateType.Any, new List<TreasureStateInfo>
		{
			new TreasureStateInfo(3, 2),
			new TreasureStateInfo(9, 2)
		}, EMiscFilterType.KeyItem, -1, -1, -1, 0));
		_dataArray.Add(new MiscItem(115, 222, 12, 1205, 5, 102, "icon_Misc_qisexinxian", 223, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, consumable: false, 0, 150, 5600, 56000, 3, 6, 2700, 7, allowRandomCreate: true, 20, isSpecial: false, -1, -1, 120, -1, 0, 0, 0, -1, canUseOnPrepareCombat: false, allowUseInPlayAndTest: false, -1, 60, new List<short>(), new List<int> { 4, 5, 6 }, EMiscGenerateType.Any, new List<TreasureStateInfo>
		{
			new TreasureStateInfo(8, 3),
			new TreasureStateInfo(3, 1)
		}, EMiscFilterType.KeyItem, -1, -1, -1, 0));
		_dataArray.Add(new MiscItem(116, 224, 12, 1205, 1, 102, "icon_Misc_lianshenjiushi", 225, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, consumable: false, 0, 500, 200, 2000, 0, 2, 600, 4, allowRandomCreate: true, 40, isSpecial: false, -1, -1, 120, -1, 0, 0, 0, -1, canUseOnPrepareCombat: false, allowUseInPlayAndTest: false, -1, 60, new List<short>(), new List<int> { 1, 2 }, EMiscGenerateType.Any, new List<TreasureStateInfo>
		{
			new TreasureStateInfo(1, 1),
			new TreasureStateInfo(2, 1),
			new TreasureStateInfo(4, 1),
			new TreasureStateInfo(9, 1)
		}, EMiscFilterType.KeyItem, -1, -1, -1, 0));
		_dataArray.Add(new MiscItem(117, 226, 12, 1205, 7, 102, "icon_Misc_hanyutai", 227, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, consumable: false, 0, 1500, 14100, 141000, 5, 8, 4500, 8, allowRandomCreate: true, 10, isSpecial: false, -1, -1, 120, -1, 0, 0, 0, -1, canUseOnPrepareCombat: false, allowUseInPlayAndTest: false, -1, 60, new List<short>(), new List<int> { 6 }, EMiscGenerateType.Any, new List<TreasureStateInfo>
		{
			new TreasureStateInfo(11, 1),
			new TreasureStateInfo(15, 1),
			new TreasureStateInfo(12, 1),
			new TreasureStateInfo(10, 1)
		}, EMiscFilterType.KeyItem, -1, -1, -1, 0));
		_dataArray.Add(new MiscItem(118, 228, 12, 1205, 0, 118, "icon_Misc_shilehuiyintai", 229, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, consumable: false, 0, 600, 100, 1000, 0, 1, 300, 3, allowRandomCreate: true, 45, isSpecial: false, -1, -1, 120, -1, 0, 0, 0, -1, canUseOnPrepareCombat: false, allowUseInPlayAndTest: false, -1, 60, new List<short>(), new List<int> { 1 }, EMiscGenerateType.Any, new List<TreasureStateInfo>
		{
			new TreasureStateInfo(8, 2),
			new TreasureStateInfo(3, 1)
		}, EMiscFilterType.KeyItem, -1, -1, -1, 0));
		_dataArray.Add(new MiscItem(119, 230, 12, 1205, 2, 118, "icon_Misc_chaoxinshi", 231, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, consumable: false, 0, 1200, 600, 6000, 0, 3, 900, 5, allowRandomCreate: true, 35, isSpecial: false, -1, -1, 120, -1, 0, 0, 0, -1, canUseOnPrepareCombat: false, allowUseInPlayAndTest: false, -1, 60, new List<short>(), new List<int> { 1, 2, 3 }, EMiscGenerateType.Any, new List<TreasureStateInfo>
		{
			new TreasureStateInfo(8, 2),
			new TreasureStateInfo(3, 1),
			new TreasureStateInfo(6, 1)
		}, EMiscFilterType.KeyItem, -1, -1, -1, 0));
	}

	private void CreateItems2()
	{
		_dataArray.Add(new MiscItem(120, 232, 12, 1205, 4, 118, "icon_Misc_shiwaixiangen", 233, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, consumable: false, 0, 100, 3100, 31000, 2, 5, 2100, 7, allowRandomCreate: true, 25, isSpecial: false, -1, -1, 120, -1, 0, 0, 0, -1, canUseOnPrepareCombat: false, allowUseInPlayAndTest: false, -1, 60, new List<short>(), new List<int> { 3, 4, 5 }, EMiscGenerateType.Any, new List<TreasureStateInfo>
		{
			new TreasureStateInfo(8, 2),
			new TreasureStateInfo(3, 2),
			new TreasureStateInfo(6, 1)
		}, EMiscFilterType.KeyItem, -1, -1, -1, 0));
		_dataArray.Add(new MiscItem(121, 234, 12, 1205, 6, 118, "icon_Misc_qicaiwutongjia", 235, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, consumable: false, 0, 500, 9200, 92000, 4, 7, 3600, 8, allowRandomCreate: true, 15, isSpecial: false, -1, -1, 120, -1, 0, 0, 0, -1, canUseOnPrepareCombat: false, allowUseInPlayAndTest: false, -1, 60, new List<short>(), new List<int> { 5, 6 }, EMiscGenerateType.Any, new List<TreasureStateInfo>
		{
			new TreasureStateInfo(8, 1),
			new TreasureStateInfo(3, 1)
		}, EMiscFilterType.KeyItem, -1, -1, -1, 0));
		_dataArray.Add(new MiscItem(122, 236, 12, 1205, 0, 122, "icon_Misc_junzishipan", 237, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, consumable: false, 0, 600, 100, 1000, 0, 1, 300, 3, allowRandomCreate: true, 45, isSpecial: false, -1, -1, 120, -1, 0, 0, 0, -1, canUseOnPrepareCombat: false, allowUseInPlayAndTest: false, -1, 60, new List<short>(), new List<int> { 1 }, EMiscGenerateType.Any, new List<TreasureStateInfo>
		{
			new TreasureStateInfo(13, 2),
			new TreasureStateInfo(1, 1)
		}, EMiscFilterType.KeyItem, -1, -1, -1, 0));
		_dataArray.Add(new MiscItem(123, 238, 12, 1205, 2, 122, "icon_Misc_baoshiqizi", 239, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, consumable: false, 0, 100, 600, 6000, 0, 3, 900, 5, allowRandomCreate: true, 35, isSpecial: false, -1, -1, 120, -1, 0, 0, 0, -1, canUseOnPrepareCombat: false, allowUseInPlayAndTest: false, -1, 60, new List<short>(), new List<int> { 1, 2, 3 }, EMiscGenerateType.Any, new List<TreasureStateInfo>
		{
			new TreasureStateInfo(13, 2),
			new TreasureStateInfo(1, 1),
			new TreasureStateInfo(6, 1)
		}, EMiscFilterType.KeyItem, -1, -1, -1, 0));
		_dataArray.Add(new MiscItem(124, 240, 12, 1205, 4, 122, "icon_Misc_kongmingjing", 241, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, consumable: false, 0, 200, 3100, 31000, 2, 5, 2100, 7, allowRandomCreate: true, 25, isSpecial: false, -1, -1, 120, -1, 0, 0, 0, -1, canUseOnPrepareCombat: false, allowUseInPlayAndTest: false, -1, 60, new List<short>(), new List<int> { 3, 4, 5 }, EMiscGenerateType.Any, new List<TreasureStateInfo>
		{
			new TreasureStateInfo(13, 2),
			new TreasureStateInfo(1, 2),
			new TreasureStateInfo(6, 1)
		}, EMiscFilterType.KeyItem, -1, -1, -1, 0));
		_dataArray.Add(new MiscItem(125, 242, 12, 1205, 6, 122, "icon_Misc_fanglüetuiyan", 243, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, consumable: false, 0, 900, 9200, 92000, 4, 7, 3600, 8, allowRandomCreate: true, 15, isSpecial: false, -1, -1, 120, -1, 0, 0, 0, -1, canUseOnPrepareCombat: false, allowUseInPlayAndTest: false, -1, 60, new List<short>(), new List<int> { 5, 6 }, EMiscGenerateType.Any, new List<TreasureStateInfo>
		{
			new TreasureStateInfo(13, 1),
			new TreasureStateInfo(1, 1)
		}, EMiscFilterType.KeyItem, -1, -1, -1, 0));
		_dataArray.Add(new MiscItem(126, 244, 12, 1205, 0, 126, "icon_Misc_mingjiabeike", 245, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, consumable: false, 0, 900, 100, 1000, 0, 1, 300, 3, allowRandomCreate: true, 45, isSpecial: false, -1, -1, 120, -1, 0, 0, 0, -1, canUseOnPrepareCombat: false, allowUseInPlayAndTest: false, -1, 60, new List<short>(), new List<int> { 1 }, EMiscGenerateType.Any, new List<TreasureStateInfo>
		{
			new TreasureStateInfo(7, 2),
			new TreasureStateInfo(4, 1)
		}, EMiscFilterType.KeyItem, -1, -1, -1, 0));
		_dataArray.Add(new MiscItem(127, 246, 12, 1205, 2, 126, "icon_Misc_shenguituobeixiang", 247, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, consumable: false, 0, 1200, 600, 6000, 0, 3, 900, 5, allowRandomCreate: true, 35, isSpecial: false, -1, -1, 120, -1, 0, 0, 0, -1, canUseOnPrepareCombat: false, allowUseInPlayAndTest: false, -1, 60, new List<short>(), new List<int> { 1, 2, 3 }, EMiscGenerateType.Any, new List<TreasureStateInfo>
		{
			new TreasureStateInfo(7, 2),
			new TreasureStateInfo(4, 1),
			new TreasureStateInfo(6, 1)
		}, EMiscFilterType.KeyItem, -1, -1, -1, 0));
		_dataArray.Add(new MiscItem(128, 248, 12, 1205, 4, 126, "icon_Misc_zhaoyeyueguangdan", 249, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, consumable: false, 0, 50, 3100, 31000, 2, 5, 2100, 7, allowRandomCreate: true, 25, isSpecial: false, -1, -1, 120, -1, 0, 0, 0, -1, canUseOnPrepareCombat: false, allowUseInPlayAndTest: false, -1, 60, new List<short>(), new List<int> { 3, 4, 5 }, EMiscGenerateType.Any, new List<TreasureStateInfo>
		{
			new TreasureStateInfo(7, 2),
			new TreasureStateInfo(4, 2),
			new TreasureStateInfo(6, 1)
		}, EMiscFilterType.KeyItem, -1, -1, -1, 0));
		_dataArray.Add(new MiscItem(129, 250, 12, 1205, 6, 126, "icon_Misc_qianshuwanxianggui", 251, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, consumable: false, 0, 1500, 9200, 92000, 4, 7, 3600, 8, allowRandomCreate: true, 15, isSpecial: false, -1, -1, 120, -1, 0, 0, 0, -1, canUseOnPrepareCombat: false, allowUseInPlayAndTest: false, -1, 60, new List<short>(), new List<int> { 5, 6 }, EMiscGenerateType.Any, new List<TreasureStateInfo>
		{
			new TreasureStateInfo(7, 1),
			new TreasureStateInfo(4, 1)
		}, EMiscFilterType.KeyItem, -1, -1, -1, 0));
		_dataArray.Add(new MiscItem(130, 252, 12, 1205, 0, 130, "icon_Misc_jingseqiangcai", 253, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, consumable: false, 0, 300, 100, 1000, 0, 1, 300, 3, allowRandomCreate: true, 45, isSpecial: false, -1, -1, 120, -1, 0, 0, 0, -1, canUseOnPrepareCombat: false, allowUseInPlayAndTest: false, -1, 60, new List<short>(), new List<int> { 1 }, EMiscGenerateType.Any, new List<TreasureStateInfo>
		{
			new TreasureStateInfo(3, 2),
			new TreasureStateInfo(8, 1)
		}, EMiscFilterType.KeyItem, -1, -1, -1, 0));
		_dataArray.Add(new MiscItem(131, 254, 12, 1205, 2, 130, "icon_Misc_baiseyanbo", 255, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, consumable: false, 0, 500, 600, 6000, 0, 3, 900, 5, allowRandomCreate: true, 35, isSpecial: false, -1, -1, 120, -1, 0, 0, 0, -1, canUseOnPrepareCombat: false, allowUseInPlayAndTest: false, -1, 60, new List<short>(), new List<int> { 1, 2, 3 }, EMiscGenerateType.Any, new List<TreasureStateInfo>
		{
			new TreasureStateInfo(3, 2),
			new TreasureStateInfo(8, 1),
			new TreasureStateInfo(6, 1)
		}, EMiscFilterType.KeyItem, -1, -1, -1, 0));
		_dataArray.Add(new MiscItem(132, 256, 12, 1205, 4, 130, "icon_Misc_baicaixiangmushi", 257, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, consumable: false, 0, 600, 3100, 31000, 2, 5, 2100, 7, allowRandomCreate: true, 25, isSpecial: false, -1, -1, 120, -1, 0, 0, 0, -1, canUseOnPrepareCombat: false, allowUseInPlayAndTest: false, -1, 60, new List<short>(), new List<int> { 3, 4, 5 }, EMiscGenerateType.Any, new List<TreasureStateInfo>
		{
			new TreasureStateInfo(3, 2),
			new TreasureStateInfo(8, 2),
			new TreasureStateInfo(6, 1)
		}, EMiscFilterType.KeyItem, -1, -1, -1, 0));
		_dataArray.Add(new MiscItem(133, 258, 12, 1205, 6, 130, "icon_Misc_yujinghuatai", 259, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, consumable: false, 0, 900, 9200, 92000, 4, 7, 3600, 8, allowRandomCreate: true, 15, isSpecial: false, -1, -1, 120, -1, 0, 0, 0, -1, canUseOnPrepareCombat: false, allowUseInPlayAndTest: false, -1, 60, new List<short>(), new List<int> { 5, 6 }, EMiscGenerateType.Any, new List<TreasureStateInfo>
		{
			new TreasureStateInfo(3, 1),
			new TreasureStateInfo(8, 1)
		}, EMiscFilterType.KeyItem, -1, -1, -1, 0));
		_dataArray.Add(new MiscItem(134, 260, 12, 1205, 0, 134, "icon_Misc_riguixingyi", 261, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, consumable: false, 0, 300, 100, 1000, 0, 1, 300, 3, allowRandomCreate: true, 45, isSpecial: false, -1, -1, 120, -1, 0, 0, 0, -1, canUseOnPrepareCombat: false, allowUseInPlayAndTest: false, -1, 60, new List<short>(), new List<int> { 1 }, EMiscGenerateType.Any, new List<TreasureStateInfo>
		{
			new TreasureStateInfo(13, 1),
			new TreasureStateInfo(7, 1),
			new TreasureStateInfo(4, 1)
		}, EMiscFilterType.KeyItem, -1, -1, -1, 0));
		_dataArray.Add(new MiscItem(135, 262, 12, 1205, 2, 134, "icon_Misc_wangxingzhu", 263, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, consumable: false, 0, 200, 600, 6000, 0, 3, 900, 5, allowRandomCreate: true, 35, isSpecial: false, -1, -1, 120, -1, 0, 0, 0, -1, canUseOnPrepareCombat: false, allowUseInPlayAndTest: false, -1, 60, new List<short>(), new List<int> { 1, 2, 3 }, EMiscGenerateType.Any, new List<TreasureStateInfo>
		{
			new TreasureStateInfo(13, 1),
			new TreasureStateInfo(7, 1),
			new TreasureStateInfo(4, 1),
			new TreasureStateInfo(2, 1)
		}, EMiscFilterType.KeyItem, -1, -1, -1, 0));
		_dataArray.Add(new MiscItem(136, 264, 12, 1205, 4, 134, "icon_Misc_kunlunshanshenxiang", 265, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, consumable: false, 0, 500, 3100, 31000, 2, 5, 2100, 7, allowRandomCreate: true, 25, isSpecial: false, -1, -1, 120, -1, 0, 0, 0, -1, canUseOnPrepareCombat: false, allowUseInPlayAndTest: false, -1, 60, new List<short>(), new List<int> { 3, 4, 5 }, EMiscGenerateType.Any, new List<TreasureStateInfo>
		{
			new TreasureStateInfo(13, 1),
			new TreasureStateInfo(7, 1),
			new TreasureStateInfo(4, 1),
			new TreasureStateInfo(2, 1),
			new TreasureStateInfo(3, 1)
		}, EMiscFilterType.KeyItem, -1, -1, -1, 0));
		_dataArray.Add(new MiscItem(137, 266, 12, 1205, 6, 134, "icon_Misc_shengmieliangxingtu", 267, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, consumable: false, 0, 10, 9200, 92000, 4, 7, 3600, 8, allowRandomCreate: true, 15, isSpecial: false, -1, -1, 120, -1, 0, 0, 0, -1, canUseOnPrepareCombat: false, allowUseInPlayAndTest: false, -1, 60, new List<short>(), new List<int> { 5, 6 }, EMiscGenerateType.Any, new List<TreasureStateInfo>
		{
			new TreasureStateInfo(13, 1),
			new TreasureStateInfo(7, 1)
		}, EMiscFilterType.KeyItem, -1, -1, -1, 0));
		_dataArray.Add(new MiscItem(138, 268, 12, 1205, 0, 138, "icon_Misc_shengxiangnijinlu", 269, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, consumable: false, 0, 500, 100, 1000, 0, 1, 300, 3, allowRandomCreate: true, 45, isSpecial: false, -1, -1, 120, -1, 0, 0, 0, -1, canUseOnPrepareCombat: false, allowUseInPlayAndTest: false, -1, 60, new List<short>(), new List<int> { 1 }, EMiscGenerateType.Any, new List<TreasureStateInfo>
		{
			new TreasureStateInfo(14, 1),
			new TreasureStateInfo(15, 1),
			new TreasureStateInfo(8, 1)
		}, EMiscFilterType.KeyItem, -1, -1, -1, 0));
		_dataArray.Add(new MiscItem(139, 270, 12, 1205, 2, 138, "icon_Misc_yiyuzhenwan", 271, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, consumable: false, 0, 300, 600, 6000, 0, 3, 900, 5, allowRandomCreate: true, 35, isSpecial: false, -1, -1, 120, -1, 0, 0, 0, -1, canUseOnPrepareCombat: false, allowUseInPlayAndTest: false, -1, 60, new List<short>(), new List<int> { 1, 2, 3 }, EMiscGenerateType.Any, new List<TreasureStateInfo>
		{
			new TreasureStateInfo(14, 1),
			new TreasureStateInfo(15, 1),
			new TreasureStateInfo(8, 1),
			new TreasureStateInfo(11, 1)
		}, EMiscFilterType.KeyItem, -1, -1, -1, 0));
		_dataArray.Add(new MiscItem(140, 272, 12, 1205, 4, 138, "icon_Misc_shiwaishanshuidan", 273, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, consumable: false, 0, 900, 3100, 31000, 2, 5, 2100, 7, allowRandomCreate: true, 25, isSpecial: false, -1, -1, 120, -1, 0, 0, 0, -1, canUseOnPrepareCombat: false, allowUseInPlayAndTest: false, -1, 60, new List<short>(), new List<int> { 3, 4, 5 }, EMiscGenerateType.Any, new List<TreasureStateInfo>
		{
			new TreasureStateInfo(14, 2),
			new TreasureStateInfo(15, 1),
			new TreasureStateInfo(8, 1),
			new TreasureStateInfo(11, 1)
		}, EMiscFilterType.KeyItem, -1, -1, -1, 0));
		_dataArray.Add(new MiscItem(141, 274, 12, 1205, 6, 138, "icon_Misc_yunlongxuanqingchi", 275, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, consumable: false, 0, 900, 9200, 92000, 4, 7, 3600, 8, allowRandomCreate: true, 15, isSpecial: false, -1, -1, 120, -1, 0, 0, 0, -1, canUseOnPrepareCombat: false, allowUseInPlayAndTest: false, -1, 60, new List<short>(), new List<int> { 5, 6 }, EMiscGenerateType.Any, new List<TreasureStateInfo>
		{
			new TreasureStateInfo(14, 2),
			new TreasureStateInfo(15, 1),
			new TreasureStateInfo(8, 1),
			new TreasureStateInfo(11, 1)
		}, EMiscFilterType.KeyItem, -1, -1, -1, 0));
		_dataArray.Add(new MiscItem(142, 276, 12, 1205, 6, 138, "icon_Misc_jinyuchenglupan", 277, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, consumable: false, 0, 600, 9200, 92000, 4, 7, 3600, 8, allowRandomCreate: true, 15, isSpecial: false, -1, -1, 120, -1, 0, 0, 0, -1, canUseOnPrepareCombat: false, allowUseInPlayAndTest: false, -1, 60, new List<short>(), new List<int> { 5, 6 }, EMiscGenerateType.Any, new List<TreasureStateInfo>
		{
			new TreasureStateInfo(14, 2),
			new TreasureStateInfo(15, 1),
			new TreasureStateInfo(8, 1),
			new TreasureStateInfo(11, 1)
		}, EMiscFilterType.KeyItem, -1, -1, -1, 0));
		_dataArray.Add(new MiscItem(143, 278, 12, 1205, 0, 143, "icon_Misc_hongluduandatai", 279, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, consumable: false, 0, 1500, 100, 1000, 0, 1, 300, 3, allowRandomCreate: true, 45, isSpecial: false, -1, -1, 120, -1, 0, 0, 0, -1, canUseOnPrepareCombat: false, allowUseInPlayAndTest: false, -1, 60, new List<short>(), new List<int> { 1 }, EMiscGenerateType.Any, new List<TreasureStateInfo>
		{
			new TreasureStateInfo(9, 1),
			new TreasureStateInfo(6, 1),
			new TreasureStateInfo(14, 1)
		}, EMiscFilterType.KeyItem, -1, -1, -1, 0));
		_dataArray.Add(new MiscItem(144, 280, 12, 1205, 2, 143, "icon_Misc_tiantieli", 281, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, consumable: false, 0, 600, 600, 6000, 0, 3, 900, 5, allowRandomCreate: true, 35, isSpecial: false, -1, -1, 120, -1, 0, 0, 0, -1, canUseOnPrepareCombat: false, allowUseInPlayAndTest: false, -1, 60, new List<short>(), new List<int> { 1, 2, 3 }, EMiscGenerateType.Any, new List<TreasureStateInfo>
		{
			new TreasureStateInfo(9, 2),
			new TreasureStateInfo(6, 1),
			new TreasureStateInfo(14, 1)
		}, EMiscFilterType.KeyItem, -1, -1, -1, 0));
		_dataArray.Add(new MiscItem(145, 282, 12, 1205, 4, 143, "icon_Misc_huanjinsha", 283, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, consumable: false, 0, 500, 3100, 31000, 2, 5, 2100, 7, allowRandomCreate: true, 25, isSpecial: false, -1, -1, 120, -1, 0, 0, 0, -1, canUseOnPrepareCombat: false, allowUseInPlayAndTest: false, -1, 60, new List<short>(), new List<int> { 3, 4, 5 }, EMiscGenerateType.Any, new List<TreasureStateInfo>
		{
			new TreasureStateInfo(9, 1),
			new TreasureStateInfo(6, 1),
			new TreasureStateInfo(14, 1),
			new TreasureStateInfo(7, 1),
			new TreasureStateInfo(3, 1)
		}, EMiscFilterType.KeyItem, -1, -1, -1, 0));
		_dataArray.Add(new MiscItem(146, 284, 12, 1205, 4, 143, "icon_Misc_huahuochenjinchi", 285, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, consumable: false, 0, 600, 3100, 31000, 2, 5, 2100, 7, allowRandomCreate: true, 25, isSpecial: false, -1, -1, 120, -1, 0, 0, 0, -1, canUseOnPrepareCombat: false, allowUseInPlayAndTest: false, -1, 60, new List<short>(), new List<int> { 3, 4, 5 }, EMiscGenerateType.Any, new List<TreasureStateInfo>
		{
			new TreasureStateInfo(9, 1),
			new TreasureStateInfo(6, 1),
			new TreasureStateInfo(14, 1),
			new TreasureStateInfo(7, 1),
			new TreasureStateInfo(3, 1)
		}, EMiscFilterType.KeyItem, -1, -1, -1, 0));
		_dataArray.Add(new MiscItem(147, 286, 12, 1205, 6, 143, "icon_Misc_longwangshuifu", 287, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, consumable: false, 0, 100, 9200, 92000, 4, 7, 3600, 8, allowRandomCreate: true, 15, isSpecial: false, -1, -1, 120, -1, 0, 0, 0, -1, canUseOnPrepareCombat: false, allowUseInPlayAndTest: false, -1, 60, new List<short>(), new List<int> { 5, 6 }, EMiscGenerateType.Any, new List<TreasureStateInfo>
		{
			new TreasureStateInfo(9, 1),
			new TreasureStateInfo(14, 1)
		}, EMiscFilterType.KeyItem, -1, -1, -1, 0));
		_dataArray.Add(new MiscItem(148, 288, 12, 1205, 7, 143, "icon_Misc_shenhuoluxin", 289, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, consumable: false, 0, 200, 14100, 141000, 5, 8, 4500, 8, allowRandomCreate: true, 10, isSpecial: false, -1, -1, 120, -1, 0, 0, 0, -1, canUseOnPrepareCombat: false, allowUseInPlayAndTest: false, -1, 60, new List<short>(), new List<int> { 6 }, EMiscGenerateType.Any, new List<TreasureStateInfo>
		{
			new TreasureStateInfo(9, 1),
			new TreasureStateInfo(7, 1)
		}, EMiscFilterType.KeyItem, -1, -1, -1, 0));
		_dataArray.Add(new MiscItem(149, 290, 12, 1205, 0, 149, "icon_Misc_zhimubaibao", 291, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, consumable: false, 0, 300, 100, 1000, 0, 1, 300, 3, allowRandomCreate: true, 45, isSpecial: false, -1, -1, 120, -1, 0, 0, 0, -1, canUseOnPrepareCombat: false, allowUseInPlayAndTest: false, -1, 60, new List<short>(), new List<int> { 1 }, EMiscGenerateType.Any, new List<TreasureStateInfo>
		{
			new TreasureStateInfo(9, 1),
			new TreasureStateInfo(6, 1),
			new TreasureStateInfo(12, 1)
		}, EMiscFilterType.KeyItem, -1, -1, -1, 0));
		_dataArray.Add(new MiscItem(150, 292, 12, 1205, 2, 149, "icon_Misc_touseqiongzhi", 293, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, consumable: false, 0, 200, 600, 6000, 0, 3, 900, 5, allowRandomCreate: true, 35, isSpecial: false, -1, -1, 120, -1, 0, 0, 0, -1, canUseOnPrepareCombat: false, allowUseInPlayAndTest: false, -1, 60, new List<short>(), new List<int> { 1, 2, 3 }, EMiscGenerateType.Any, new List<TreasureStateInfo>
		{
			new TreasureStateInfo(9, 1),
			new TreasureStateInfo(6, 1),
			new TreasureStateInfo(12, 1)
		}, EMiscFilterType.KeyItem, -1, -1, -1, 0));
		_dataArray.Add(new MiscItem(151, 294, 12, 1205, 4, 149, "icon_Misc_bairenju", 295, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, consumable: false, 0, 600, 3100, 31000, 2, 5, 2100, 7, allowRandomCreate: true, 25, isSpecial: false, -1, -1, 120, -1, 0, 0, 0, -1, canUseOnPrepareCombat: false, allowUseInPlayAndTest: false, -1, 60, new List<short>(), new List<int> { 3, 4, 5 }, EMiscGenerateType.Any, new List<TreasureStateInfo>
		{
			new TreasureStateInfo(9, 1),
			new TreasureStateInfo(6, 1),
			new TreasureStateInfo(12, 1),
			new TreasureStateInfo(15, 1),
			new TreasureStateInfo(7, 1),
			new TreasureStateInfo(3, 1)
		}, EMiscFilterType.KeyItem, -1, -1, -1, 0));
		_dataArray.Add(new MiscItem(152, 296, 12, 1205, 4, 149, "icon_Misc_panshanshuilong", 297, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, consumable: false, 0, 900, 3100, 31000, 2, 5, 2100, 7, allowRandomCreate: true, 25, isSpecial: false, -1, -1, 120, -1, 0, 0, 0, -1, canUseOnPrepareCombat: false, allowUseInPlayAndTest: false, -1, 60, new List<short>(), new List<int> { 3, 4, 5 }, EMiscGenerateType.Any, new List<TreasureStateInfo>
		{
			new TreasureStateInfo(9, 1),
			new TreasureStateInfo(6, 1),
			new TreasureStateInfo(12, 1),
			new TreasureStateInfo(15, 1),
			new TreasureStateInfo(7, 1),
			new TreasureStateInfo(3, 1)
		}, EMiscFilterType.KeyItem, -1, -1, -1, 0));
		_dataArray.Add(new MiscItem(153, 298, 12, 1205, 6, 149, "icon_Misc_yingulengyanmei", 299, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, consumable: false, 0, 500, 9200, 92000, 4, 7, 3600, 8, allowRandomCreate: true, 15, isSpecial: false, -1, -1, 120, -1, 0, 0, 0, -1, canUseOnPrepareCombat: false, allowUseInPlayAndTest: false, -1, 60, new List<short>(), new List<int> { 5, 6 }, EMiscGenerateType.Any, new List<TreasureStateInfo>
		{
			new TreasureStateInfo(9, 1),
			new TreasureStateInfo(6, 1)
		}, EMiscFilterType.KeyItem, -1, -1, -1, 0));
		_dataArray.Add(new MiscItem(154, 300, 12, 1205, 7, 149, "icon_Misc_shenmugen", 301, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, consumable: false, 0, 100, 14100, 141000, 5, 8, 4500, 8, allowRandomCreate: true, 10, isSpecial: false, -1, -1, 120, -1, 0, 0, 0, -1, canUseOnPrepareCombat: false, allowUseInPlayAndTest: false, -1, 60, new List<short>(), new List<int> { 6 }, EMiscGenerateType.Any, new List<TreasureStateInfo>
		{
			new TreasureStateInfo(9, 1),
			new TreasureStateInfo(12, 1)
		}, EMiscFilterType.KeyItem, -1, -1, -1, 0));
		_dataArray.Add(new MiscItem(155, 302, 12, 1205, 0, 155, "icon_Misc_hongmuyaogui", 303, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, consumable: false, 0, 1500, 100, 1000, 0, 1, 300, 3, allowRandomCreate: true, 45, isSpecial: false, -1, -1, 120, -1, 0, 0, 0, -1, canUseOnPrepareCombat: false, allowUseInPlayAndTest: false, -1, 60, new List<short>(), new List<int> { 1 }, EMiscGenerateType.Any, new List<TreasureStateInfo>
		{
			new TreasureStateInfo(10, 1),
			new TreasureStateInfo(3, 1),
			new TreasureStateInfo(12, 1),
			new TreasureStateInfo(1, 1),
			new TreasureStateInfo(15, 1)
		}, EMiscFilterType.KeyItem, -1, -1, -1, 0));
		_dataArray.Add(new MiscItem(156, 304, 12, 1205, 2, 155, "icon_Misc_hanyubingguan", 305, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, consumable: false, 0, 1500, 600, 6000, 0, 3, 900, 5, allowRandomCreate: true, 35, isSpecial: false, -1, -1, 120, -1, 0, 0, 0, -1, canUseOnPrepareCombat: false, allowUseInPlayAndTest: false, -1, 60, new List<short>(), new List<int> { 1, 2, 3 }, EMiscGenerateType.Any, new List<TreasureStateInfo>
		{
			new TreasureStateInfo(10, 1),
			new TreasureStateInfo(3, 1),
			new TreasureStateInfo(12, 1),
			new TreasureStateInfo(1, 1),
			new TreasureStateInfo(15, 1)
		}, EMiscFilterType.KeyItem, -1, -1, -1, 0));
		_dataArray.Add(new MiscItem(157, 306, 12, 1205, 4, 155, "icon_Misc_sishidingguangzhu", 307, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, consumable: false, 0, 10, 3100, 31000, 2, 5, 2100, 7, allowRandomCreate: true, 25, isSpecial: false, -1, -1, 120, -1, 0, 0, 0, -1, canUseOnPrepareCombat: false, allowUseInPlayAndTest: false, -1, 60, new List<short>(), new List<int> { 3, 4, 5 }, EMiscGenerateType.Any, new List<TreasureStateInfo>
		{
			new TreasureStateInfo(10, 1),
			new TreasureStateInfo(3, 1),
			new TreasureStateInfo(12, 1),
			new TreasureStateInfo(1, 1),
			new TreasureStateInfo(15, 1)
		}, EMiscFilterType.KeyItem, -1, -1, -1, 0));
		_dataArray.Add(new MiscItem(158, 308, 12, 1205, 4, 155, "icon_Misc_tongyuanbaiqiaodan", 309, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, consumable: false, 0, 500, 3100, 31000, 2, 5, 2100, 7, allowRandomCreate: true, 25, isSpecial: false, -1, -1, 120, -1, 0, 0, 0, -1, canUseOnPrepareCombat: false, allowUseInPlayAndTest: false, -1, 60, new List<short>(), new List<int> { 3, 4, 5 }, EMiscGenerateType.Any, new List<TreasureStateInfo>
		{
			new TreasureStateInfo(10, 1),
			new TreasureStateInfo(3, 1),
			new TreasureStateInfo(12, 1),
			new TreasureStateInfo(1, 1),
			new TreasureStateInfo(15, 1)
		}, EMiscFilterType.KeyItem, -1, -1, -1, 0));
		_dataArray.Add(new MiscItem(159, 310, 12, 1205, 6, 155, "icon_Misc_shuihuowuyantan", 311, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, consumable: false, 0, 500, 9200, 92000, 4, 7, 3600, 8, allowRandomCreate: true, 15, isSpecial: false, -1, -1, 120, -1, 0, 0, 0, -1, canUseOnPrepareCombat: false, allowUseInPlayAndTest: false, -1, 60, new List<short>(), new List<int> { 5, 6 }, EMiscGenerateType.Any, new List<TreasureStateInfo>
		{
			new TreasureStateInfo(10, 1),
			new TreasureStateInfo(3, 1)
		}, EMiscFilterType.KeyItem, -1, -1, -1, 0));
		_dataArray.Add(new MiscItem(160, 312, 12, 1205, 7, 155, "icon_Misc_shennongxie", 313, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, consumable: false, 0, 20, 14100, 141000, 5, 8, 4500, 8, allowRandomCreate: true, 10, isSpecial: false, -1, -1, 120, -1, 0, 0, 0, -1, canUseOnPrepareCombat: false, allowUseInPlayAndTest: false, -1, 60, new List<short>(), new List<int> { 6 }, EMiscGenerateType.Any, new List<TreasureStateInfo>
		{
			new TreasureStateInfo(10, 1),
			new TreasureStateInfo(3, 1)
		}, EMiscFilterType.KeyItem, -1, -1, -1, 0));
		_dataArray.Add(new MiscItem(161, 314, 12, 1205, 0, 161, "icon_Misc_xinglujia", 315, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, consumable: false, 0, 900, 100, 1000, 0, 1, 300, 3, allowRandomCreate: true, 45, isSpecial: false, -1, -1, 120, -1, 0, 0, 0, -1, canUseOnPrepareCombat: false, allowUseInPlayAndTest: false, -1, 60, new List<short>(), new List<int> { 1 }, EMiscGenerateType.Any, new List<TreasureStateInfo>
		{
			new TreasureStateInfo(10, 1),
			new TreasureStateInfo(3, 1),
			new TreasureStateInfo(12, 1),
			new TreasureStateInfo(13, 1),
			new TreasureStateInfo(15, 1)
		}, EMiscFilterType.KeyItem, -1, -1, -1, 0));
		_dataArray.Add(new MiscItem(162, 316, 12, 1205, 2, 161, "icon_Misc_birenxiang", 317, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, consumable: false, 0, 100, 600, 6000, 0, 3, 900, 5, allowRandomCreate: true, 35, isSpecial: false, -1, -1, 120, -1, 0, 0, 0, -1, canUseOnPrepareCombat: false, allowUseInPlayAndTest: false, -1, 60, new List<short>(), new List<int> { 1, 2, 3 }, EMiscGenerateType.Any, new List<TreasureStateInfo>
		{
			new TreasureStateInfo(10, 1),
			new TreasureStateInfo(3, 1),
			new TreasureStateInfo(12, 1),
			new TreasureStateInfo(13, 1),
			new TreasureStateInfo(15, 1)
		}, EMiscFilterType.KeyItem, -1, -1, -1, 0));
		_dataArray.Add(new MiscItem(163, 318, 12, 1205, 4, 161, "icon_Misc_tunguihuazhanglu", 319, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, consumable: false, 0, 200, 3100, 31000, 2, 5, 2100, 7, allowRandomCreate: true, 25, isSpecial: false, -1, -1, 120, -1, 0, 0, 0, -1, canUseOnPrepareCombat: false, allowUseInPlayAndTest: false, -1, 60, new List<short>(), new List<int> { 3, 4, 5 }, EMiscGenerateType.Any, new List<TreasureStateInfo>
		{
			new TreasureStateInfo(10, 1),
			new TreasureStateInfo(3, 1),
			new TreasureStateInfo(12, 1),
			new TreasureStateInfo(13, 1),
			new TreasureStateInfo(15, 1)
		}, EMiscFilterType.KeyItem, -1, -1, -1, 0));
		_dataArray.Add(new MiscItem(164, 320, 12, 1205, 4, 161, "icon_Misc_sirenlong", 321, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, consumable: false, 0, 600, 3100, 31000, 2, 5, 2100, 7, allowRandomCreate: true, 25, isSpecial: false, -1, -1, 120, -1, 0, 0, 0, -1, canUseOnPrepareCombat: false, allowUseInPlayAndTest: false, -1, 60, new List<short>(), new List<int> { 3, 4, 5 }, EMiscGenerateType.Any, new List<TreasureStateInfo>
		{
			new TreasureStateInfo(10, 1),
			new TreasureStateInfo(3, 1),
			new TreasureStateInfo(12, 1),
			new TreasureStateInfo(13, 1),
			new TreasureStateInfo(15, 1)
		}, EMiscFilterType.KeyItem, -1, -1, -1, 0));
		_dataArray.Add(new MiscItem(165, 322, 12, 1205, 6, 161, "icon_Misc_huaxiedan", 323, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, consumable: false, 0, 300, 9200, 92000, 4, 7, 3600, 8, allowRandomCreate: true, 15, isSpecial: false, -1, -1, 120, -1, 0, 0, 0, -1, canUseOnPrepareCombat: false, allowUseInPlayAndTest: false, -1, 60, new List<short>(), new List<int> { 5, 6 }, EMiscGenerateType.Any, new List<TreasureStateInfo>
		{
			new TreasureStateInfo(10, 1),
			new TreasureStateInfo(12, 1)
		}, EMiscFilterType.KeyItem, -1, -1, -1, 0));
		_dataArray.Add(new MiscItem(166, 324, 12, 1205, 7, 161, "icon_Misc_qixiangshenlongmu", 325, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, consumable: false, 0, 500, 14100, 141000, 5, 8, 4500, 8, allowRandomCreate: true, 10, isSpecial: false, -1, -1, 120, -1, 0, 0, 0, -1, canUseOnPrepareCombat: false, allowUseInPlayAndTest: false, -1, 60, new List<short>(), new List<int> { 6 }, EMiscGenerateType.Any, new List<TreasureStateInfo>
		{
			new TreasureStateInfo(10, 1),
			new TreasureStateInfo(12, 1)
		}, EMiscFilterType.KeyItem, -1, -1, -1, 0));
		_dataArray.Add(new MiscItem(167, 326, 12, 1205, 0, 167, "icon_Misc_zhijiranliao", 327, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, consumable: false, 0, 600, 100, 1000, 0, 1, 300, 3, allowRandomCreate: true, 45, isSpecial: false, -1, -1, 120, -1, 0, 0, 0, -1, canUseOnPrepareCombat: false, allowUseInPlayAndTest: false, -1, 60, new List<short>(), new List<int> { 1 }, EMiscGenerateType.Any, new List<TreasureStateInfo>
		{
			new TreasureStateInfo(3, 1),
			new TreasureStateInfo(9, 1),
			new TreasureStateInfo(10, 1)
		}, EMiscFilterType.KeyItem, -1, -1, -1, 0));
		_dataArray.Add(new MiscItem(168, 328, 12, 1205, 2, 167, "icon_Misc_xuanshexigu", 329, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, consumable: false, 0, 100, 600, 6000, 0, 3, 900, 5, allowRandomCreate: true, 35, isSpecial: false, -1, -1, 120, -1, 0, 0, 0, -1, canUseOnPrepareCombat: false, allowUseInPlayAndTest: false, -1, 60, new List<short>(), new List<int> { 1, 2, 3 }, EMiscGenerateType.Any, new List<TreasureStateInfo>
		{
			new TreasureStateInfo(3, 2),
			new TreasureStateInfo(9, 1),
			new TreasureStateInfo(10, 1)
		}, EMiscFilterType.KeyItem, -1, -1, -1, 0));
		_dataArray.Add(new MiscItem(169, 330, 12, 1205, 4, 167, "icon_Misc_baihuazhongzi", 331, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, consumable: false, 0, 100, 3100, 31000, 2, 5, 2100, 7, allowRandomCreate: true, 25, isSpecial: false, -1, -1, 120, -1, 0, 0, 0, -1, canUseOnPrepareCombat: false, allowUseInPlayAndTest: false, -1, 60, new List<short>(), new List<int> { 3, 4, 5 }, EMiscGenerateType.Any, new List<TreasureStateInfo>
		{
			new TreasureStateInfo(3, 2),
			new TreasureStateInfo(9, 2),
			new TreasureStateInfo(10, 1)
		}, EMiscFilterType.KeyItem, -1, -1, -1, 0));
		_dataArray.Add(new MiscItem(170, 332, 12, 1205, 4, 167, "icon_Misc_qizhentailuan", 333, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, consumable: false, 0, 200, 3100, 31000, 2, 5, 2100, 7, allowRandomCreate: true, 25, isSpecial: false, -1, -1, 120, -1, 0, 0, 0, -1, canUseOnPrepareCombat: false, allowUseInPlayAndTest: false, -1, 60, new List<short>(), new List<int> { 3, 4, 5 }, EMiscGenerateType.Any, new List<TreasureStateInfo>
		{
			new TreasureStateInfo(3, 2),
			new TreasureStateInfo(9, 2),
			new TreasureStateInfo(10, 1)
		}, EMiscFilterType.KeyItem, -1, -1, -1, 0));
		_dataArray.Add(new MiscItem(171, 334, 12, 1205, 6, 167, "icon_Misc_shiersecaican", 335, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, consumable: false, 0, 10, 9200, 92000, 4, 7, 3600, 8, allowRandomCreate: true, 15, isSpecial: false, -1, -1, 120, -1, 0, 0, 0, -1, canUseOnPrepareCombat: false, allowUseInPlayAndTest: false, -1, 60, new List<short>(), new List<int> { 5, 6 }, EMiscGenerateType.Any, new List<TreasureStateInfo>
		{
			new TreasureStateInfo(3, 1),
			new TreasureStateInfo(10, 1)
		}, EMiscFilterType.KeyItem, -1, -1, -1, 0));
		_dataArray.Add(new MiscItem(172, 336, 12, 1205, 7, 167, "icon_Misc_shencaijingci", 337, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, consumable: false, 0, 100, 14100, 141000, 5, 8, 4500, 8, allowRandomCreate: true, 10, isSpecial: false, -1, -1, 120, -1, 0, 0, 0, -1, canUseOnPrepareCombat: false, allowUseInPlayAndTest: false, -1, 60, new List<short>(), new List<int> { 6 }, EMiscGenerateType.Any, new List<TreasureStateInfo>
		{
			new TreasureStateInfo(3, 1),
			new TreasureStateInfo(9, 1)
		}, EMiscFilterType.KeyItem, -1, -1, -1, 0));
		_dataArray.Add(new MiscItem(173, 338, 12, 1205, 0, 173, "icon_Misc_sanshanmaoshi", 339, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, consumable: false, 0, 900, 100, 1000, 0, 1, 300, 3, allowRandomCreate: true, 45, isSpecial: false, -1, -1, 120, -1, 0, 0, 0, -1, canUseOnPrepareCombat: false, allowUseInPlayAndTest: false, -1, 60, new List<short>(), new List<int> { 1 }, EMiscGenerateType.Any, new List<TreasureStateInfo>
		{
			new TreasureStateInfo(7, 1),
			new TreasureStateInfo(9, 1),
			new TreasureStateInfo(10, 1)
		}, EMiscFilterType.KeyItem, -1, -1, -1, 0));
		_dataArray.Add(new MiscItem(174, 340, 12, 1205, 2, 173, "icon_Misc_huochigongjing", 341, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, consumable: false, 0, 600, 600, 6000, 0, 3, 900, 5, allowRandomCreate: true, 35, isSpecial: false, -1, -1, 120, -1, 0, 0, 0, -1, canUseOnPrepareCombat: false, allowUseInPlayAndTest: false, -1, 60, new List<short>(), new List<int> { 1, 2, 3 }, EMiscGenerateType.Any, new List<TreasureStateInfo>
		{
			new TreasureStateInfo(7, 2),
			new TreasureStateInfo(9, 1),
			new TreasureStateInfo(10, 1)
		}, EMiscFilterType.KeyItem, -1, -1, -1, 0));
		_dataArray.Add(new MiscItem(175, 342, 12, 1205, 4, 173, "icon_Misc_huanbaosha", 343, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, consumable: false, 0, 500, 3100, 31000, 2, 5, 2100, 7, allowRandomCreate: true, 25, isSpecial: false, -1, -1, 120, -1, 0, 0, 0, -1, canUseOnPrepareCombat: false, allowUseInPlayAndTest: false, -1, 60, new List<short>(), new List<int> { 3, 4, 5 }, EMiscGenerateType.Any, new List<TreasureStateInfo>
		{
			new TreasureStateInfo(7, 2),
			new TreasureStateInfo(9, 2),
			new TreasureStateInfo(10, 1)
		}, EMiscFilterType.KeyItem, -1, -1, -1, 0));
		_dataArray.Add(new MiscItem(176, 344, 12, 1205, 4, 173, "icon_Misc_jingangju", 345, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, consumable: false, 0, 600, 3100, 31000, 2, 5, 2100, 7, allowRandomCreate: true, 25, isSpecial: false, -1, -1, 120, -1, 0, 0, 0, -1, canUseOnPrepareCombat: false, allowUseInPlayAndTest: false, -1, 60, new List<short>(), new List<int> { 3, 4, 5 }, EMiscGenerateType.Any, new List<TreasureStateInfo>
		{
			new TreasureStateInfo(7, 2),
			new TreasureStateInfo(9, 2),
			new TreasureStateInfo(10, 1)
		}, EMiscFilterType.KeyItem, -1, -1, -1, 0));
		_dataArray.Add(new MiscItem(177, 346, 12, 1205, 6, 173, "icon_Misc_linglongbabao", 347, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, consumable: false, 0, 50, 9200, 92000, 4, 7, 3600, 8, allowRandomCreate: true, 15, isSpecial: false, -1, -1, 120, -1, 0, 0, 0, -1, canUseOnPrepareCombat: false, allowUseInPlayAndTest: false, -1, 60, new List<short>(), new List<int> { 5, 6 }, EMiscGenerateType.Any, new List<TreasureStateInfo>
		{
			new TreasureStateInfo(7, 1),
			new TreasureStateInfo(10, 1)
		}, EMiscFilterType.KeyItem, -1, -1, -1, 0));
		_dataArray.Add(new MiscItem(178, 348, 12, 1205, 7, 173, "icon_Misc_qiannianyumu", 349, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, consumable: false, 0, 500, 14100, 141000, 5, 8, 4500, 8, allowRandomCreate: true, 10, isSpecial: false, -1, -1, 120, -1, 0, 0, 0, -1, canUseOnPrepareCombat: false, allowUseInPlayAndTest: false, -1, 60, new List<short>(), new List<int> { 6 }, EMiscGenerateType.Any, new List<TreasureStateInfo>
		{
			new TreasureStateInfo(7, 1),
			new TreasureStateInfo(9, 1)
		}, EMiscFilterType.KeyItem, -1, -1, -1, 0));
		_dataArray.Add(new MiscItem(179, 350, 12, 1205, 0, 179, "icon_Misc_xianghuobeiji", 351, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, consumable: false, 0, 900, 100, 1000, 0, 1, 300, 3, allowRandomCreate: true, 45, isSpecial: false, -1, -1, 120, -1, 0, 0, 0, -1, canUseOnPrepareCombat: false, allowUseInPlayAndTest: false, -1, 60, new List<short>(), new List<int> { 1 }, EMiscGenerateType.Any, new List<TreasureStateInfo>
		{
			new TreasureStateInfo(4, 2),
			new TreasureStateInfo(7, 2)
		}, EMiscFilterType.KeyItem, -1, -1, -1, 0));
	}

	private void CreateItems3()
	{
		_dataArray.Add(new MiscItem(180, 352, 12, 1205, 2, 179, "icon_Misc_xianshidaocang", 353, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, consumable: false, 0, 500, 600, 6000, 0, 3, 900, 5, allowRandomCreate: true, 35, isSpecial: false, -1, -1, 120, -1, 0, 0, 0, -1, canUseOnPrepareCombat: false, allowUseInPlayAndTest: false, -1, 60, new List<short>(), new List<int> { 1, 2, 3 }, EMiscGenerateType.Any, new List<TreasureStateInfo>
		{
			new TreasureStateInfo(4, 1),
			new TreasureStateInfo(7, 1),
			new TreasureStateInfo(2, 1),
			new TreasureStateInfo(5, 1)
		}, EMiscFilterType.KeyItem, -1, -1, -1, 0));
		_dataArray.Add(new MiscItem(181, 354, 12, 1205, 4, 179, "icon_Misc_xianshanyunqi", 355, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, consumable: false, 0, 10, 3100, 31000, 2, 5, 2100, 7, allowRandomCreate: true, 25, isSpecial: false, -1, -1, 120, -1, 0, 0, 0, -1, canUseOnPrepareCombat: false, allowUseInPlayAndTest: false, -1, 60, new List<short>(), new List<int> { 3, 4, 5 }, EMiscGenerateType.Any, new List<TreasureStateInfo>
		{
			new TreasureStateInfo(4, 2),
			new TreasureStateInfo(7, 2),
			new TreasureStateInfo(2, 1),
			new TreasureStateInfo(5, 1)
		}, EMiscFilterType.KeyItem, -1, -1, -1, 0));
		_dataArray.Add(new MiscItem(182, 356, 12, 1205, 6, 179, "icon_Misc_zijinliandanlu", 357, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, consumable: false, 0, 600, 9200, 92000, 4, 7, 3600, 8, allowRandomCreate: true, 15, isSpecial: false, -1, -1, 120, -1, 0, 0, 0, -1, canUseOnPrepareCombat: false, allowUseInPlayAndTest: false, -1, 60, new List<short>(), new List<int> { 5, 6 }, EMiscGenerateType.Any, new List<TreasureStateInfo>
		{
			new TreasureStateInfo(4, 1),
			new TreasureStateInfo(7, 1)
		}, EMiscFilterType.KeyItem, -1, -1, -1, 0));
		_dataArray.Add(new MiscItem(183, 358, 12, 1205, 0, 183, "icon_Misc_gaosengsheli", 359, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, consumable: false, 0, 20, 100, 1000, 0, 1, 300, 3, allowRandomCreate: true, 45, isSpecial: false, -1, -1, 120, -1, 0, 0, 0, -1, canUseOnPrepareCombat: false, allowUseInPlayAndTest: false, -1, 60, new List<short>(), new List<int> { 1 }, EMiscGenerateType.Any, new List<TreasureStateInfo>
		{
			new TreasureStateInfo(1, 2),
			new TreasureStateInfo(11, 2)
		}, EMiscFilterType.KeyItem, -1, -1, -1, 0));
		_dataArray.Add(new MiscItem(184, 360, 12, 1205, 2, 183, "icon_Misc_zhixinfoyao", 361, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, consumable: false, 0, 50, 600, 6000, 0, 3, 900, 5, allowRandomCreate: true, 35, isSpecial: false, -1, -1, 120, -1, 0, 0, 0, -1, canUseOnPrepareCombat: false, allowUseInPlayAndTest: false, -1, 60, new List<short>(), new List<int> { 1, 2, 3 }, EMiscGenerateType.Any, new List<TreasureStateInfo>
		{
			new TreasureStateInfo(1, 1),
			new TreasureStateInfo(11, 1),
			new TreasureStateInfo(2, 1),
			new TreasureStateInfo(5, 1),
			new TreasureStateInfo(15, 1)
		}, EMiscFilterType.KeyItem, -1, -1, -1, 0));
		_dataArray.Add(new MiscItem(185, 362, 12, 1205, 4, 183, "icon_Misc_kurongshuangshu", 363, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, consumable: false, 0, 600, 3100, 31000, 2, 5, 2100, 7, allowRandomCreate: true, 25, isSpecial: false, -1, -1, 120, -1, 0, 0, 0, -1, canUseOnPrepareCombat: false, allowUseInPlayAndTest: false, -1, 60, new List<short>(), new List<int> { 3, 4, 5 }, EMiscGenerateType.Any, new List<TreasureStateInfo>
		{
			new TreasureStateInfo(1, 1),
			new TreasureStateInfo(11, 1),
			new TreasureStateInfo(2, 1),
			new TreasureStateInfo(5, 1),
			new TreasureStateInfo(15, 1)
		}, EMiscFilterType.KeyItem, -1, -1, -1, 0));
		_dataArray.Add(new MiscItem(186, 364, 12, 1205, 6, 183, "icon_Misc_sanshiwubuzhenjing", 365, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, consumable: false, 0, 500, 9200, 92000, 4, 7, 3600, 8, allowRandomCreate: true, 15, isSpecial: false, -1, -1, 120, -1, 0, 0, 0, -1, canUseOnPrepareCombat: false, allowUseInPlayAndTest: false, -1, 60, new List<short>(), new List<int> { 5, 6 }, EMiscGenerateType.Any, new List<TreasureStateInfo>
		{
			new TreasureStateInfo(1, 1),
			new TreasureStateInfo(11, 1)
		}, EMiscFilterType.KeyItem, -1, -1, -1, 0));
		_dataArray.Add(new MiscItem(187, 366, 12, 1205, 0, 187, "icon_Misc_baiqianwanzhan", 367, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, consumable: false, 0, 600, 100, 1000, 0, 1, 300, 3, allowRandomCreate: true, 45, isSpecial: false, -1, -1, 120, -1, 0, 0, 0, -1, canUseOnPrepareCombat: false, allowUseInPlayAndTest: false, -1, 60, new List<short>(), new List<int> { 1 }, EMiscGenerateType.Any, new List<TreasureStateInfo>
		{
			new TreasureStateInfo(14, 2),
			new TreasureStateInfo(6, 1),
			new TreasureStateInfo(2, 1)
		}, EMiscFilterType.KeyItem, -1, -1, -1, 0));
		_dataArray.Add(new MiscItem(188, 368, 12, 1205, 2, 187, "icon_Misc_tiantiexuanding", 369, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, consumable: false, 0, 900, 600, 6000, 0, 3, 900, 5, allowRandomCreate: true, 35, isSpecial: false, -1, -1, 120, -1, 0, 0, 0, -1, canUseOnPrepareCombat: false, allowUseInPlayAndTest: false, -1, 60, new List<short>(), new List<int> { 1, 2, 3 }, EMiscGenerateType.Any, new List<TreasureStateInfo>
		{
			new TreasureStateInfo(14, 1),
			new TreasureStateInfo(6, 1),
			new TreasureStateInfo(2, 1),
			new TreasureStateInfo(5, 1)
		}, EMiscFilterType.KeyItem, -1, -1, -1, 0));
		_dataArray.Add(new MiscItem(189, 370, 12, 1205, 4, 187, "icon_Misc_sijiliufeng", 371, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, consumable: false, 0, 500, 3100, 31000, 2, 5, 2100, 7, allowRandomCreate: true, 25, isSpecial: false, -1, -1, 120, -1, 0, 0, 0, -1, canUseOnPrepareCombat: false, allowUseInPlayAndTest: false, -1, 60, new List<short>(), new List<int> { 3, 4, 5 }, EMiscGenerateType.Any, new List<TreasureStateInfo>
		{
			new TreasureStateInfo(14, 2),
			new TreasureStateInfo(6, 1),
			new TreasureStateInfo(2, 1),
			new TreasureStateInfo(5, 1)
		}, EMiscFilterType.KeyItem, -1, -1, -1, 0));
		_dataArray.Add(new MiscItem(190, 372, 12, 1205, 4, 187, "icon_Misc_tianchenghuayu", 373, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, consumable: false, 0, 500, 3100, 31000, 2, 5, 2100, 7, allowRandomCreate: true, 25, isSpecial: false, -1, -1, 120, -1, 0, 0, 0, -1, canUseOnPrepareCombat: false, allowUseInPlayAndTest: false, -1, 60, new List<short>(), new List<int> { 3, 4, 5 }, EMiscGenerateType.Any, new List<TreasureStateInfo>
		{
			new TreasureStateInfo(14, 2),
			new TreasureStateInfo(6, 1),
			new TreasureStateInfo(2, 1),
			new TreasureStateInfo(5, 1)
		}, EMiscFilterType.KeyItem, -1, -1, -1, 0));
		_dataArray.Add(new MiscItem(191, 374, 12, 1205, 6, 187, "icon_Misc_shuangerwanguohu", 375, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, consumable: false, 0, 300, 9200, 92000, 4, 7, 3600, 8, allowRandomCreate: true, 15, isSpecial: false, -1, -1, 120, -1, 0, 0, 0, -1, canUseOnPrepareCombat: false, allowUseInPlayAndTest: false, -1, 60, new List<short>(), new List<int> { 5, 6 }, EMiscGenerateType.Any, new List<TreasureStateInfo>
		{
			new TreasureStateInfo(14, 1),
			new TreasureStateInfo(6, 1),
			new TreasureStateInfo(2, 1)
		}, EMiscFilterType.KeyItem, -1, -1, -1, 0));
		_dataArray.Add(new MiscItem(192, 376, 12, 1205, 7, 187, "icon_Misc_shuijingbingzhuan", 377, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, consumable: false, 0, 600, 14100, 141000, 5, 8, 4500, 8, allowRandomCreate: true, 10, isSpecial: false, -1, -1, 120, -1, 0, 0, 0, -1, canUseOnPrepareCombat: false, allowUseInPlayAndTest: false, -1, 60, new List<short>(), new List<int> { 6 }, EMiscGenerateType.Any, new List<TreasureStateInfo>
		{
			new TreasureStateInfo(14, 1),
			new TreasureStateInfo(6, 1),
			new TreasureStateInfo(2, 1)
		}, EMiscFilterType.KeyItem, -1, -1, -1, 0));
		_dataArray.Add(new MiscItem(193, 378, 12, 1205, 0, 193, "icon_Misc_jinzhuangyushi", 379, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, consumable: false, 0, 500, 100, 1000, 0, 1, 300, 3, allowRandomCreate: true, 45, isSpecial: false, -1, -1, 120, -1, 0, 0, 0, -1, canUseOnPrepareCombat: false, allowUseInPlayAndTest: false, -1, 60, new List<short>(), new List<int> { 1 }, EMiscGenerateType.Any, new List<TreasureStateInfo>
		{
			new TreasureStateInfo(15, 1),
			new TreasureStateInfo(7, 1),
			new TreasureStateInfo(11, 1)
		}, EMiscFilterType.KeyItem, -1, -1, -1, 0));
		_dataArray.Add(new MiscItem(194, 380, 12, 1205, 2, 193, "icon_Misc_xibanxingtou", 381, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, consumable: false, 0, 900, 600, 6000, 0, 3, 900, 5, allowRandomCreate: true, 35, isSpecial: false, -1, -1, 120, -1, 0, 0, 0, -1, canUseOnPrepareCombat: false, allowUseInPlayAndTest: false, -1, 60, new List<short>(), new List<int> { 1, 2, 3 }, EMiscGenerateType.Any, new List<TreasureStateInfo>
		{
			new TreasureStateInfo(15, 2),
			new TreasureStateInfo(7, 1),
			new TreasureStateInfo(11, 1)
		}, EMiscFilterType.KeyItem, -1, -1, -1, 0));
		_dataArray.Add(new MiscItem(195, 382, 12, 1205, 4, 193, "icon_Misc_mingyuanshanshui", 383, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, consumable: false, 0, 1500, 3100, 31000, 2, 5, 2100, 7, allowRandomCreate: true, 25, isSpecial: false, -1, -1, 120, -1, 0, 0, 0, -1, canUseOnPrepareCombat: false, allowUseInPlayAndTest: false, -1, 60, new List<short>(), new List<int> { 3, 4, 5 }, EMiscGenerateType.Any, new List<TreasureStateInfo>
		{
			new TreasureStateInfo(15, 1),
			new TreasureStateInfo(7, 1),
			new TreasureStateInfo(11, 1),
			new TreasureStateInfo(6, 1)
		}, EMiscFilterType.KeyItem, -1, -1, -1, 0));
		_dataArray.Add(new MiscItem(196, 384, 12, 1205, 6, 193, "icon_Misc_qihuayidan", 385, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, consumable: false, 0, 600, 9200, 92000, 4, 7, 3600, 8, allowRandomCreate: true, 15, isSpecial: false, -1, -1, 120, -1, 0, 0, 0, -1, canUseOnPrepareCombat: false, allowUseInPlayAndTest: false, -1, 60, new List<short>(), new List<int> { 5, 6 }, EMiscGenerateType.Any, new List<TreasureStateInfo>
		{
			new TreasureStateInfo(15, 1),
			new TreasureStateInfo(11, 1)
		}, EMiscFilterType.KeyItem, -1, -1, -1, 0));
		_dataArray.Add(new MiscItem(197, 386, 12, 1205, 8, -1, "icon_Misc_taiwuzupu", 387, transferable: false, stackable: true, wagerable: false, refinable: false, poisonable: false, repairable: false, inheritable: false, consumable: false, 0, 0, 0, 0, 0, 0, 0, 0, allowRandomCreate: true, 0, isSpecial: true, -1, -1, -1, -1, 0, 0, 0, -1, canUseOnPrepareCombat: false, allowUseInPlayAndTest: false, -1, 60, new List<short>(), new List<int>(), EMiscGenerateType.Invalid, new List<TreasureStateInfo>(), EMiscFilterType.KeyItem, -1, -1, -1, 0));
		_dataArray.Add(new MiscItem(198, 388, 12, 1205, 8, -1, "icon_Misc_xiyuditu", 389, transferable: false, stackable: true, wagerable: false, refinable: false, poisonable: false, repairable: false, inheritable: true, consumable: false, 0, 0, 0, 0, 0, 0, 0, 0, allowRandomCreate: true, 0, isSpecial: true, -1, -1, -1, -1, 0, 0, 0, -1, canUseOnPrepareCombat: false, allowUseInPlayAndTest: false, -1, 60, new List<short>(), new List<int>(), EMiscGenerateType.Invalid, new List<TreasureStateInfo>(), EMiscFilterType.KeyItem, -1, -1, -1, 0));
		_dataArray.Add(new MiscItem(199, 390, 12, 1205, 8, -1, "icon_Misc_liudaozhuanlun", 391, transferable: false, stackable: true, wagerable: false, refinable: false, poisonable: false, repairable: false, inheritable: true, consumable: false, 0, 0, 0, 0, 0, 0, 0, 0, allowRandomCreate: true, 0, isSpecial: true, -1, -1, -1, -1, 0, 0, 0, -1, canUseOnPrepareCombat: false, allowUseInPlayAndTest: false, -1, 60, new List<short>(), new List<int>(), EMiscGenerateType.Invalid, new List<TreasureStateInfo>(), EMiscFilterType.KeyItem, -1, -1, -1, 0));
		_dataArray.Add(new MiscItem(200, 392, 12, 1200, 8, 200, "icon_Misc_monvyisuipian", 393, transferable: false, stackable: false, wagerable: false, refinable: false, poisonable: false, repairable: false, inheritable: false, consumable: false, -99, 10, 0, 0, 0, 0, 0, 0, allowRandomCreate: true, 100, isSpecial: true, -1, -1, -1, -1, 0, 0, 0, 0, canUseOnPrepareCombat: false, allowUseInPlayAndTest: false, -1, 60, new List<short>(), new List<int>(), EMiscGenerateType.Invalid, new List<TreasureStateInfo>(), EMiscFilterType.Item, -1, -1, -1, 0));
		_dataArray.Add(new MiscItem(201, 394, 12, 1200, 8, 200, "icon_Misc_fuxietiesuipian", 395, transferable: false, stackable: false, wagerable: false, refinable: false, poisonable: false, repairable: false, inheritable: false, consumable: false, -99, 80, 0, 0, 0, 0, 0, 0, allowRandomCreate: true, 100, isSpecial: true, -1, -1, -1, -1, 0, 0, 0, 0, canUseOnPrepareCombat: false, allowUseInPlayAndTest: false, -1, 60, new List<short>(), new List<int>(), EMiscGenerateType.Invalid, new List<TreasureStateInfo>(), EMiscFilterType.Item, -1, -1, -1, 0));
		_dataArray.Add(new MiscItem(202, 396, 12, 1200, 8, 200, "icon_Misc_daxuanningsuipian", 397, transferable: false, stackable: false, wagerable: false, refinable: false, poisonable: false, repairable: false, inheritable: false, consumable: false, -99, 40, 0, 0, 0, 0, 0, 0, allowRandomCreate: true, 100, isSpecial: true, -1, -1, -1, -1, 0, 0, 0, 0, canUseOnPrepareCombat: false, allowUseInPlayAndTest: false, -1, 60, new List<short>(), new List<int>(), EMiscGenerateType.Invalid, new List<TreasureStateInfo>(), EMiscFilterType.Item, -1, -1, -1, 0));
		_dataArray.Add(new MiscItem(203, 398, 12, 1200, 8, 200, "icon_Misc_fenghuangjiansuipian", 399, transferable: false, stackable: false, wagerable: false, refinable: false, poisonable: false, repairable: false, inheritable: false, consumable: false, -99, 50, 0, 0, 0, 0, 0, 0, allowRandomCreate: true, 100, isSpecial: true, -1, -1, -1, -1, 0, 0, 0, 0, canUseOnPrepareCombat: false, allowUseInPlayAndTest: false, -1, 60, new List<short>(), new List<int>(), EMiscGenerateType.Invalid, new List<TreasureStateInfo>(), EMiscFilterType.Item, -1, -1, -1, 0));
		_dataArray.Add(new MiscItem(204, 400, 12, 1200, 8, 200, "icon_Misc_fenshenliansuipian", 401, transferable: false, stackable: false, wagerable: false, refinable: false, poisonable: false, repairable: false, inheritable: false, consumable: false, -99, 20, 0, 0, 0, 0, 0, 0, allowRandomCreate: true, 100, isSpecial: true, -1, -1, -1, -1, 0, 0, 0, 0, canUseOnPrepareCombat: false, allowUseInPlayAndTest: false, -1, 60, new List<short>(), new List<int>(), EMiscGenerateType.Invalid, new List<TreasureStateInfo>(), EMiscFilterType.Item, -1, -1, -1, 0));
		_dataArray.Add(new MiscItem(205, 402, 12, 1200, 8, 200, "icon_Misc_jielongbosuipian", 403, transferable: false, stackable: false, wagerable: false, refinable: false, poisonable: false, repairable: false, inheritable: false, consumable: false, -99, 60, 0, 0, 0, 0, 0, 0, allowRandomCreate: true, 100, isSpecial: true, -1, -1, -1, -1, 0, 0, 0, 0, canUseOnPrepareCombat: false, allowUseInPlayAndTest: false, -1, 60, new List<short>(), new List<int>(), EMiscGenerateType.Invalid, new List<TreasureStateInfo>(), EMiscFilterType.Item, -1, -1, -1, 0));
		_dataArray.Add(new MiscItem(206, 404, 12, 1200, 8, 200, "icon_Misc_rongchenyinsuipian", 405, transferable: false, stackable: false, wagerable: false, refinable: false, poisonable: false, repairable: false, inheritable: false, consumable: false, -99, 10, 0, 0, 0, 0, 0, 0, allowRandomCreate: true, 100, isSpecial: true, -1, -1, -1, -1, 0, 0, 0, 0, canUseOnPrepareCombat: false, allowUseInPlayAndTest: false, -1, 60, new List<short>(), new List<int>(), EMiscGenerateType.Invalid, new List<TreasureStateInfo>(), EMiscFilterType.Item, -1, -1, -1, 0));
		_dataArray.Add(new MiscItem(207, 406, 12, 1200, 8, 200, "icon_Misc_qiumomusuipian", 407, transferable: false, stackable: false, wagerable: false, refinable: false, poisonable: false, repairable: false, inheritable: false, consumable: false, -99, 70, 0, 0, 0, 0, 0, 0, allowRandomCreate: true, 100, isSpecial: true, -1, -1, -1, -1, 0, 0, 0, 0, canUseOnPrepareCombat: false, allowUseInPlayAndTest: false, -1, 60, new List<short>(), new List<int>(), EMiscGenerateType.Invalid, new List<TreasureStateInfo>(), EMiscFilterType.Item, -1, -1, -1, 0));
		_dataArray.Add(new MiscItem(208, 408, 12, 1200, 8, 200, "icon_Misc_guishenxiasuipian", 409, transferable: false, stackable: false, wagerable: false, refinable: false, poisonable: false, repairable: false, inheritable: false, consumable: false, -99, 10, 0, 0, 0, 0, 0, 0, allowRandomCreate: true, 100, isSpecial: true, -1, -1, -1, -1, 0, 0, 0, 0, canUseOnPrepareCombat: false, allowUseInPlayAndTest: false, -1, 60, new List<short>(), new List<int>(), EMiscGenerateType.Invalid, new List<TreasureStateInfo>(), EMiscFilterType.Item, -1, -1, -1, 0));
		_dataArray.Add(new MiscItem(209, 410, 12, 1200, 8, 200, "icon_Misc_fuyujiansuipian", 411, transferable: false, stackable: false, wagerable: false, refinable: false, poisonable: false, repairable: false, inheritable: false, consumable: false, -99, 30, 0, 0, 0, 0, 0, 0, allowRandomCreate: true, 100, isSpecial: true, -1, -1, -1, -1, 0, 0, 0, 0, canUseOnPrepareCombat: false, allowUseInPlayAndTest: false, -1, 60, new List<short>(), new List<int>(), EMiscGenerateType.Invalid, new List<TreasureStateInfo>(), EMiscFilterType.Item, -1, -1, -1, 0));
		_dataArray.Add(new MiscItem(210, 412, 12, 1200, 0, 200, "icon_Misc_fuyujianbing", 413, transferable: false, stackable: false, wagerable: false, refinable: false, poisonable: false, repairable: false, inheritable: false, consumable: false, -99, 0, 0, 0, 0, 0, 0, 0, allowRandomCreate: true, 0, isSpecial: true, -1, -1, -1, -1, 0, 0, 0, -1, canUseOnPrepareCombat: false, allowUseInPlayAndTest: false, -1, 60, new List<short>(), new List<int>(), EMiscGenerateType.Invalid, new List<TreasureStateInfo>(), EMiscFilterType.Item, -1, -1, -1, 0));
		_dataArray.Add(new MiscItem(211, 414, 12, 1202, 8, -1, "icon_Misc_hunxinmozijue", 415, transferable: false, stackable: false, wagerable: false, refinable: false, poisonable: false, repairable: false, inheritable: true, consumable: false, -99, 0, 0, 0, 0, 36, 21600, 8, allowRandomCreate: true, 0, isSpecial: true, -1, -1, -1, -1, 0, 0, 0, -1, canUseOnPrepareCombat: false, allowUseInPlayAndTest: false, -1, 60, new List<short>(), new List<int>(), EMiscGenerateType.Invalid, new List<TreasureStateInfo>(), EMiscFilterType.LegendaryBook, -1, -1, -1, 0));
		_dataArray.Add(new MiscItem(212, 416, 12, 1202, 8, -1, "icon_Misc_baiyixinghuaji", 417, transferable: false, stackable: false, wagerable: false, refinable: false, poisonable: false, repairable: false, inheritable: true, consumable: false, -99, 0, 0, 0, 0, 36, 21600, 8, allowRandomCreate: true, 0, isSpecial: true, -1, -1, -1, -1, 0, 0, 0, -1, canUseOnPrepareCombat: false, allowUseInPlayAndTest: false, -1, 60, new List<short>(), new List<int>(), EMiscGenerateType.Invalid, new List<TreasureStateInfo>(), EMiscFilterType.LegendaryBook, -1, -1, -1, 0));
		_dataArray.Add(new MiscItem(213, 418, 12, 1202, 8, -1, "icon_Misc_daquanqianfa", 419, transferable: false, stackable: false, wagerable: false, refinable: false, poisonable: false, repairable: false, inheritable: true, consumable: false, -99, 0, 0, 0, 0, 36, 21600, 8, allowRandomCreate: true, 0, isSpecial: true, -1, -1, -1, -1, 0, 0, 0, -1, canUseOnPrepareCombat: false, allowUseInPlayAndTest: false, -1, 60, new List<short>(), new List<int>(), EMiscGenerateType.Invalid, new List<TreasureStateInfo>(), EMiscFilterType.LegendaryBook, -1, -1, -1, 0));
		_dataArray.Add(new MiscItem(214, 420, 12, 1202, 8, -1, "icon_Misc_xianglongyanhua", 421, transferable: false, stackable: false, wagerable: false, refinable: false, poisonable: false, repairable: false, inheritable: true, consumable: false, -99, 0, 0, 0, 0, 36, 21600, 8, allowRandomCreate: true, 0, isSpecial: true, -1, -1, -1, -1, 0, 0, 0, -1, canUseOnPrepareCombat: false, allowUseInPlayAndTest: false, -1, 60, new List<short>(), new List<int>(), EMiscGenerateType.Invalid, new List<TreasureStateInfo>(), EMiscFilterType.LegendaryBook, -1, -1, -1, 0));
		_dataArray.Add(new MiscItem(215, 422, 12, 1202, 8, -1, "icon_Misc_xinguancanjian", 423, transferable: false, stackable: false, wagerable: false, refinable: false, poisonable: false, repairable: false, inheritable: true, consumable: false, -99, 0, 0, 0, 0, 36, 21600, 8, allowRandomCreate: true, 0, isSpecial: true, -1, -1, -1, -1, 0, 0, 0, -1, canUseOnPrepareCombat: false, allowUseInPlayAndTest: false, -1, 60, new List<short>(), new List<int>(), EMiscGenerateType.Invalid, new List<TreasureStateInfo>(), EMiscFilterType.LegendaryBook, -1, -1, -1, 0));
		_dataArray.Add(new MiscItem(216, 424, 12, 1202, 8, -1, "icon_Misc_bashanzhibaojing", 425, transferable: false, stackable: false, wagerable: false, refinable: false, poisonable: false, repairable: false, inheritable: true, consumable: false, -99, 0, 0, 0, 0, 36, 21600, 8, allowRandomCreate: true, 0, isSpecial: true, -1, -1, -1, -1, 0, 0, 0, -1, canUseOnPrepareCombat: false, allowUseInPlayAndTest: false, -1, 60, new List<short>(), new List<int>(), EMiscGenerateType.Invalid, new List<TreasureStateInfo>(), EMiscFilterType.LegendaryBook, -1, -1, -1, 0));
		_dataArray.Add(new MiscItem(217, 426, 12, 1202, 8, -1, "icon_Misc_huayingqishu", 427, transferable: false, stackable: false, wagerable: false, refinable: false, poisonable: false, repairable: false, inheritable: true, consumable: false, -99, 0, 0, 0, 0, 36, 21600, 8, allowRandomCreate: true, 0, isSpecial: true, -1, -1, -1, -1, 0, 0, 0, -1, canUseOnPrepareCombat: false, allowUseInPlayAndTest: false, -1, 60, new List<short>(), new List<int>(), EMiscGenerateType.Invalid, new List<TreasureStateInfo>(), EMiscFilterType.LegendaryBook, -1, -1, -1, 0));
		_dataArray.Add(new MiscItem(218, 428, 12, 1202, 8, -1, "icon_Misc_wumingjiandian", 429, transferable: false, stackable: false, wagerable: false, refinable: false, poisonable: false, repairable: false, inheritable: true, consumable: false, -99, 0, 0, 0, 0, 36, 21600, 8, allowRandomCreate: true, 0, isSpecial: true, -1, -1, -1, -1, 0, 0, 0, -1, canUseOnPrepareCombat: false, allowUseInPlayAndTest: false, -1, 60, new List<short>(), new List<int>(), EMiscGenerateType.Invalid, new List<TreasureStateInfo>(), EMiscFilterType.LegendaryBook, -1, -1, -1, 0));
		_dataArray.Add(new MiscItem(219, 430, 12, 1202, 8, -1, "icon_Misc_shishamoluolu", 431, transferable: false, stackable: false, wagerable: false, refinable: false, poisonable: false, repairable: false, inheritable: true, consumable: false, -99, 0, 0, 0, 0, 36, 21600, 8, allowRandomCreate: true, 0, isSpecial: true, -1, -1, -1, -1, 0, 0, 0, -1, canUseOnPrepareCombat: false, allowUseInPlayAndTest: false, -1, 60, new List<short>(), new List<int>(), EMiscGenerateType.Invalid, new List<TreasureStateInfo>(), EMiscFilterType.LegendaryBook, -1, -1, -1, 0));
		_dataArray.Add(new MiscItem(220, 432, 12, 1202, 8, -1, "icon_Misc_yihuakaitian", 433, transferable: false, stackable: false, wagerable: false, refinable: false, poisonable: false, repairable: false, inheritable: true, consumable: false, -99, 0, 0, 0, 0, 36, 21600, 8, allowRandomCreate: true, 0, isSpecial: true, -1, -1, -1, -1, 0, 0, 0, -1, canUseOnPrepareCombat: false, allowUseInPlayAndTest: false, -1, 60, new List<short>(), new List<int>(), EMiscGenerateType.Invalid, new List<TreasureStateInfo>(), EMiscFilterType.LegendaryBook, -1, -1, -1, 0));
		_dataArray.Add(new MiscItem(221, 434, 12, 1202, 8, -1, "icon_Misc_wuxianxuanyuanshu", 435, transferable: false, stackable: false, wagerable: false, refinable: false, poisonable: false, repairable: false, inheritable: true, consumable: false, -99, 0, 0, 0, 0, 36, 21600, 8, allowRandomCreate: true, 0, isSpecial: true, -1, -1, -1, -1, 0, 0, 0, -1, canUseOnPrepareCombat: false, allowUseInPlayAndTest: false, -1, 60, new List<short>(), new List<int>(), EMiscGenerateType.Invalid, new List<TreasureStateInfo>(), EMiscFilterType.LegendaryBook, -1, -1, -1, 0));
		_dataArray.Add(new MiscItem(222, 436, 12, 1202, 8, -1, "icon_Misc_jiusizhencang", 437, transferable: false, stackable: false, wagerable: false, refinable: false, poisonable: false, repairable: false, inheritable: true, consumable: false, -99, 0, 0, 0, 0, 36, 21600, 8, allowRandomCreate: true, 0, isSpecial: true, -1, -1, -1, -1, 0, 0, 0, -1, canUseOnPrepareCombat: false, allowUseInPlayAndTest: false, -1, 60, new List<short>(), new List<int>(), EMiscGenerateType.Invalid, new List<TreasureStateInfo>(), EMiscFilterType.LegendaryBook, -1, -1, -1, 0));
		_dataArray.Add(new MiscItem(223, 438, 12, 1202, 8, -1, "icon_Misc_tiantongshenshu", 439, transferable: false, stackable: false, wagerable: false, refinable: false, poisonable: false, repairable: false, inheritable: true, consumable: false, -99, 0, 0, 0, 0, 36, 21600, 8, allowRandomCreate: true, 0, isSpecial: true, -1, -1, -1, -1, 0, 0, 0, -1, canUseOnPrepareCombat: false, allowUseInPlayAndTest: false, -1, 60, new List<short>(), new List<int>(), EMiscGenerateType.Invalid, new List<TreasureStateInfo>(), EMiscFilterType.LegendaryBook, -1, -1, -1, 0));
		_dataArray.Add(new MiscItem(224, 440, 12, 1202, 8, -1, "icon_Misc_shennvjueyin", 441, transferable: false, stackable: false, wagerable: false, refinable: false, poisonable: false, repairable: false, inheritable: true, consumable: false, -99, 0, 0, 0, 0, 36, 21600, 8, allowRandomCreate: true, 0, isSpecial: true, -1, -1, -1, -1, 0, 0, 0, -1, canUseOnPrepareCombat: false, allowUseInPlayAndTest: false, -1, 60, new List<short>(), new List<int>(), EMiscGenerateType.Invalid, new List<TreasureStateInfo>(), EMiscFilterType.LegendaryBook, -1, -1, -1, 0));
		_dataArray.Add(new MiscItem(225, 442, 12, 1207, 8, -1, "icon_Misc_moyingling", 443, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, consumable: false, -7, 10, 30750, 61500, 6, 18, 10800, 8, allowRandomCreate: true, 0, isSpecial: false, -1, -1, -1, -1, 0, 0, 0, -1, canUseOnPrepareCombat: false, allowUseInPlayAndTest: false, -1, 60, new List<short>(), new List<int>(), EMiscGenerateType.Invalid, new List<TreasureStateInfo>(), EMiscFilterType.Collection, -1, -1, -1, 0));
		_dataArray.Add(new MiscItem(226, 444, 12, 1200, 0, -1, "icon_Misc_huanxinxuewutong", 445, transferable: false, stackable: false, wagerable: false, refinable: false, poisonable: false, repairable: false, inheritable: false, consumable: false, -99, 0, 0, 0, 0, 0, 0, 0, allowRandomCreate: true, 0, isSpecial: true, 1, -1, -1, -1, 0, 0, 0, -1, canUseOnPrepareCombat: false, allowUseInPlayAndTest: false, -1, 60, new List<short>(), new List<int>(), EMiscGenerateType.Invalid, new List<TreasureStateInfo>(), EMiscFilterType.Collection, -1, -1, -1, 0));
		_dataArray.Add(new MiscItem(227, 446, 12, 1200, 0, -1, "icon_Misc_shierzhiqingzhuci", 447, transferable: false, stackable: false, wagerable: false, refinable: false, poisonable: false, repairable: false, inheritable: false, consumable: false, -99, 0, 0, 0, 0, 0, 0, 0, allowRandomCreate: true, 0, isSpecial: true, 1, -1, -1, -1, 0, 0, 0, -1, canUseOnPrepareCombat: false, allowUseInPlayAndTest: false, -1, 60, new List<short>(), new List<int>(), EMiscGenerateType.Invalid, new List<TreasureStateInfo>(), EMiscFilterType.Collection, -1, -1, -1, 0));
		_dataArray.Add(new MiscItem(228, 448, 12, 1200, 0, -1, "icon_Misc_xuxiangongdecanhai", 449, transferable: false, stackable: false, wagerable: false, refinable: false, poisonable: false, repairable: false, inheritable: false, consumable: false, -99, 0, 0, 0, 0, 0, 0, 0, allowRandomCreate: true, 0, isSpecial: true, -1, -1, -1, -1, 0, 0, 0, -1, canUseOnPrepareCombat: false, allowUseInPlayAndTest: false, -1, 60, new List<short>(), new List<int>(), EMiscGenerateType.Invalid, new List<TreasureStateInfo>(), EMiscFilterType.Collection, -1, -1, -1, 0));
		_dataArray.Add(new MiscItem(229, 450, 12, 1200, 0, -1, "icon_Misc_laokaodexiaochuan", 451, transferable: false, stackable: false, wagerable: false, refinable: false, poisonable: false, repairable: false, inheritable: false, consumable: false, -99, 0, 0, 0, 0, 0, 0, 0, allowRandomCreate: true, 0, isSpecial: true, -1, -1, -1, -1, 0, 0, 0, -1, canUseOnPrepareCombat: false, allowUseInPlayAndTest: false, -1, 60, new List<short>(), new List<int>(), EMiscGenerateType.Invalid, new List<TreasureStateInfo>(), EMiscFilterType.Collection, -1, -1, -1, 0));
		_dataArray.Add(new MiscItem(230, 452, 12, 1200, 8, -1, "icon_Misc_zijinliandanlu", 453, transferable: false, stackable: false, wagerable: false, refinable: false, poisonable: false, repairable: false, inheritable: false, consumable: false, -99, 0, 0, 0, 0, 0, 0, 0, allowRandomCreate: true, 0, isSpecial: true, -1, -1, -1, -1, 0, 0, 0, -1, canUseOnPrepareCombat: false, allowUseInPlayAndTest: false, -1, 60, new List<short>(), new List<int>(), EMiscGenerateType.Invalid, new List<TreasureStateInfo>(), EMiscFilterType.Collection, -1, -1, -1, 0));
		_dataArray.Add(new MiscItem(231, 454, 12, 1200, 8, -1, "icon_Misc_qijiexuanzhu", 455, transferable: false, stackable: false, wagerable: false, refinable: false, poisonable: false, repairable: false, inheritable: false, consumable: false, -99, 0, 0, 0, 0, 0, 0, 0, allowRandomCreate: true, 0, isSpecial: true, 1, -1, -1, -1, 0, 0, 0, -1, canUseOnPrepareCombat: false, allowUseInPlayAndTest: false, -1, 60, new List<short>(), new List<int>(), EMiscGenerateType.Invalid, new List<TreasureStateInfo>(), EMiscFilterType.Collection, -1, -1, -1, 0));
		_dataArray.Add(new MiscItem(232, 456, 12, 1200, 0, -1, "icon_Misc_huanxindexingli", 457, transferable: false, stackable: false, wagerable: false, refinable: false, poisonable: false, repairable: false, inheritable: false, consumable: false, -99, 7000, 0, 0, 0, 0, 0, 0, allowRandomCreate: true, 0, isSpecial: true, -1, -1, -1, -1, 0, 0, 0, -1, canUseOnPrepareCombat: false, allowUseInPlayAndTest: false, -1, 60, new List<short>(), new List<int>(), EMiscGenerateType.Invalid, new List<TreasureStateInfo>(), EMiscFilterType.Collection, -1, -1, -1, 0));
		_dataArray.Add(new MiscItem(233, 458, 12, 1205, 0, -1, "icon_Misc_qisetiepan", 459, transferable: false, stackable: true, wagerable: false, refinable: false, poisonable: false, repairable: false, inheritable: false, consumable: false, 0, 10, 0, 0, 0, 0, 0, 0, allowRandomCreate: true, 0, isSpecial: true, -1, -1, -1, -1, 0, 0, 0, -1, canUseOnPrepareCombat: false, allowUseInPlayAndTest: false, -1, 60, new List<short>(), new List<int>(), EMiscGenerateType.Invalid, new List<TreasureStateInfo>(), EMiscFilterType.Collection, -1, -1, -1, 0));
		_dataArray.Add(new MiscItem(234, 460, 12, 1200, 8, -1, "icon_Misc_tianjiefulu", 461, transferable: false, stackable: true, wagerable: false, refinable: false, poisonable: false, repairable: false, inheritable: true, consumable: false, 0, 0, 0, 0, 0, 0, 0, 0, allowRandomCreate: true, 0, isSpecial: true, -1, -1, 1, -1, 0, 0, 0, -1, canUseOnPrepareCombat: true, allowUseInPlayAndTest: false, -1, 60, new List<short>(), new List<int>(), EMiscGenerateType.Invalid, new List<TreasureStateInfo>(), EMiscFilterType.Item, -1, -1, -1, 0));
		_dataArray.Add(new MiscItem(235, 462, 12, 1205, 8, 235, "icon_Misc_yifajingdian", 463, transferable: false, stackable: true, wagerable: false, refinable: false, poisonable: false, repairable: false, inheritable: true, consumable: false, 0, 0, 0, 0, 0, 0, 0, 0, allowRandomCreate: true, 0, isSpecial: true, -1, -1, -1, -1, 0, 0, 0, -1, canUseOnPrepareCombat: false, allowUseInPlayAndTest: false, -1, 60, new List<short>(), new List<int>(), EMiscGenerateType.Invalid, new List<TreasureStateInfo>(), EMiscFilterType.KeyItem, -1, -1, -1, 0));
		_dataArray.Add(new MiscItem(236, 464, 12, 1205, 8, 235, "icon_Misc_wulunjingdian", 465, transferable: false, stackable: true, wagerable: false, refinable: false, poisonable: false, repairable: false, inheritable: true, consumable: false, 0, 0, 0, 0, 0, 0, 0, 0, allowRandomCreate: true, 0, isSpecial: true, -1, -1, -1, -1, 0, 0, 0, -1, canUseOnPrepareCombat: false, allowUseInPlayAndTest: false, -1, 60, new List<short>(), new List<int>(), EMiscGenerateType.Invalid, new List<TreasureStateInfo>(), EMiscFilterType.KeyItem, -1, -1, -1, 0));
		_dataArray.Add(new MiscItem(237, 466, 12, 1200, 0, -1, "icon_Misc_beilou", 467, transferable: false, stackable: true, wagerable: false, refinable: false, poisonable: false, repairable: false, inheritable: false, consumable: false, 0, 0, 0, 0, 0, 0, 0, 0, allowRandomCreate: true, 0, isSpecial: true, -1, -1, -1, -1, 0, 0, 0, -1, canUseOnPrepareCombat: false, allowUseInPlayAndTest: false, -1, 60, new List<short>(), new List<int>(), EMiscGenerateType.Invalid, new List<TreasureStateInfo>(), EMiscFilterType.Collection, -1, -1, -1, 0));
		_dataArray.Add(new MiscItem(238, 468, 12, 1200, 1, -1, "icon_Misc_liegong", 469, transferable: false, stackable: true, wagerable: false, refinable: false, poisonable: false, repairable: false, inheritable: false, consumable: false, 0, 0, 0, 0, 0, 0, 0, 0, allowRandomCreate: true, 0, isSpecial: true, -1, -1, -1, -1, 0, 0, 0, -1, canUseOnPrepareCombat: false, allowUseInPlayAndTest: false, -1, 60, new List<short>(), new List<int>(), EMiscGenerateType.Invalid, new List<TreasureStateInfo>(), EMiscFilterType.Collection, -1, -1, -1, 0));
		_dataArray.Add(new MiscItem(239, 470, 12, 1200, 2, -1, "icon_Misc_tiechui", 471, transferable: false, stackable: true, wagerable: false, refinable: false, poisonable: false, repairable: false, inheritable: false, consumable: false, 0, 0, 0, 0, 0, 0, 0, 0, allowRandomCreate: true, 0, isSpecial: true, -1, -1, -1, -1, 0, 0, 0, -1, canUseOnPrepareCombat: false, allowUseInPlayAndTest: false, -1, 60, new List<short>(), new List<int>(), EMiscGenerateType.Invalid, new List<TreasureStateInfo>(), EMiscFilterType.Collection, -1, -1, -1, 0));
	}

	private void CreateItems4()
	{
		_dataArray.Add(new MiscItem(240, 472, 12, 1200, 0, -1, "icon_Misc_powan", 473, transferable: false, stackable: true, wagerable: false, refinable: false, poisonable: false, repairable: false, inheritable: false, consumable: false, 0, 0, 0, 0, 0, 0, 0, 0, allowRandomCreate: true, 0, isSpecial: true, -1, -1, -1, -1, 0, 0, 0, -1, canUseOnPrepareCombat: false, allowUseInPlayAndTest: false, -1, 60, new List<short>(), new List<int>(), EMiscGenerateType.Invalid, new List<TreasureStateInfo>(), EMiscFilterType.Collection, -1, -1, -1, 0));
		_dataArray.Add(new MiscItem(241, 474, 12, 1200, 1, -1, "icon_Misc_gunang", 475, transferable: false, stackable: true, wagerable: false, refinable: false, poisonable: false, repairable: false, inheritable: false, consumable: false, 0, 0, 0, 0, 0, 0, 0, 0, allowRandomCreate: true, 0, isSpecial: true, -1, -1, -1, -1, 0, 0, 0, -1, canUseOnPrepareCombat: false, allowUseInPlayAndTest: false, -1, 60, new List<short>(), new List<int>(), EMiscGenerateType.Invalid, new List<TreasureStateInfo>(), EMiscFilterType.Collection, -1, -1, -1, 0));
		_dataArray.Add(new MiscItem(242, 476, 12, 1200, 2, -1, "icon_Misc_canlv", 477, transferable: false, stackable: true, wagerable: false, refinable: false, poisonable: false, repairable: false, inheritable: false, consumable: false, 0, 0, 0, 0, 0, 0, 0, 0, allowRandomCreate: true, 0, isSpecial: true, -1, -1, -1, -1, 0, 0, 0, -1, canUseOnPrepareCombat: false, allowUseInPlayAndTest: false, -1, 60, new List<short>(), new List<int>(), EMiscGenerateType.Invalid, new List<TreasureStateInfo>(), EMiscFilterType.Collection, -1, -1, -1, 0));
		_dataArray.Add(new MiscItem(243, 478, 12, 1200, 3, -1, "icon_Misc_gulaodeyubianfu", 479, transferable: false, stackable: true, wagerable: false, refinable: false, poisonable: false, repairable: false, inheritable: false, consumable: false, 0, 0, 0, 0, 0, 0, 2000, 0, allowRandomCreate: true, 0, isSpecial: true, -1, -1, -1, -1, 0, 0, 0, -1, canUseOnPrepareCombat: false, allowUseInPlayAndTest: false, -1, 60, new List<short>(), new List<int>(), EMiscGenerateType.Invalid, new List<TreasureStateInfo>(), EMiscFilterType.Collection, -1, -1, -1, 0));
		_dataArray.Add(new MiscItem(244, 480, 12, 1200, 3, -1, "icon_Misc_gulaodeyuhuli", 481, transferable: false, stackable: true, wagerable: false, refinable: false, poisonable: false, repairable: false, inheritable: false, consumable: false, 0, 0, 0, 0, 0, 0, 4000, 0, allowRandomCreate: true, 0, isSpecial: true, -1, -1, -1, -1, 0, 0, 0, -1, canUseOnPrepareCombat: false, allowUseInPlayAndTest: false, -1, 60, new List<short>(), new List<int>(), EMiscGenerateType.Invalid, new List<TreasureStateInfo>(), EMiscFilterType.Collection, -1, -1, -1, 0));
		_dataArray.Add(new MiscItem(245, 482, 12, 1200, 3, -1, "icon_Misc_gulaodeyuhudie", 483, transferable: false, stackable: true, wagerable: false, refinable: false, poisonable: false, repairable: false, inheritable: false, consumable: false, 0, 0, 0, 0, 0, 0, 6000, 0, allowRandomCreate: true, 0, isSpecial: true, -1, -1, -1, -1, 0, 0, 0, -1, canUseOnPrepareCombat: false, allowUseInPlayAndTest: false, -1, 60, new List<short>(), new List<int>(), EMiscGenerateType.Invalid, new List<TreasureStateInfo>(), EMiscFilterType.Collection, -1, -1, -1, 0));
		_dataArray.Add(new MiscItem(246, 484, 12, 1200, 0, -1, "icon_Misc_pojiudelingdang", 485, transferable: false, stackable: true, wagerable: false, refinable: false, poisonable: false, repairable: false, inheritable: false, consumable: false, 0, 0, 0, 0, 0, 0, 0, 0, allowRandomCreate: true, 0, isSpecial: true, -1, -1, -1, -1, 0, 0, 0, -1, canUseOnPrepareCombat: false, allowUseInPlayAndTest: false, -1, 60, new List<short>(), new List<int>(), EMiscGenerateType.Invalid, new List<TreasureStateInfo>(), EMiscFilterType.Collection, -1, -1, -1, 0));
		_dataArray.Add(new MiscItem(247, 486, 12, 1200, 0, -1, "icon_Misc_angzangdediaoxiang", 487, transferable: false, stackable: true, wagerable: false, refinable: false, poisonable: false, repairable: false, inheritable: false, consumable: false, 0, 0, 0, 0, 0, 0, 0, 0, allowRandomCreate: true, 0, isSpecial: true, -1, -1, -1, -1, 0, 0, 0, -1, canUseOnPrepareCombat: false, allowUseInPlayAndTest: false, -1, 60, new List<short>(), new List<int>(), EMiscGenerateType.Invalid, new List<TreasureStateInfo>(), EMiscFilterType.Collection, -1, -1, -1, 0));
		_dataArray.Add(new MiscItem(248, 488, 12, 1200, 8, -1, "icon_Misc_liujinhuoling", 489, transferable: false, stackable: true, wagerable: false, refinable: false, poisonable: false, repairable: false, inheritable: false, consumable: false, 0, 0, 0, 0, 0, 0, 0, 0, allowRandomCreate: true, 0, isSpecial: true, -1, -1, -1, -1, 0, 0, 0, -1, canUseOnPrepareCombat: false, allowUseInPlayAndTest: false, -1, 60, new List<short>(), new List<int>(), EMiscGenerateType.Invalid, new List<TreasureStateInfo>(), EMiscFilterType.Collection, -1, -1, -1, 0));
		_dataArray.Add(new MiscItem(249, 490, 12, 1200, 8, -1, "icon_Misc_qingtongbaojuan", 491, transferable: false, stackable: true, wagerable: false, refinable: false, poisonable: false, repairable: false, inheritable: false, consumable: false, 0, 0, 0, 0, 0, 0, 0, 0, allowRandomCreate: true, 0, isSpecial: true, -1, -1, -1, -1, 0, 0, 0, -1, canUseOnPrepareCombat: false, allowUseInPlayAndTest: false, -1, 60, new List<short>(), new List<int>(), EMiscGenerateType.Invalid, new List<TreasureStateInfo>(), EMiscFilterType.Collection, -1, -1, -1, 0));
		_dataArray.Add(new MiscItem(250, 492, 12, 1200, 8, -1, "icon_Misc_wangfangpingfabian", 493, transferable: false, stackable: true, wagerable: false, refinable: false, poisonable: false, repairable: false, inheritable: false, consumable: false, 0, 0, 0, 0, 0, 0, 0, 0, allowRandomCreate: true, 0, isSpecial: true, -1, -1, -1, -1, 0, 0, 0, -1, canUseOnPrepareCombat: false, allowUseInPlayAndTest: false, -1, 60, new List<short>(), new List<int>(), EMiscGenerateType.Invalid, new List<TreasureStateInfo>(), EMiscFilterType.Collection, -1, -1, -1, 0));
		_dataArray.Add(new MiscItem(251, 494, 12, 1200, 8, -1, "icon_Misc_taishangyuyiwen", 495, transferable: false, stackable: true, wagerable: false, refinable: false, poisonable: false, repairable: false, inheritable: false, consumable: false, 0, 0, 0, 0, 0, 0, 0, 0, allowRandomCreate: true, 0, isSpecial: true, -1, -1, -1, -1, 0, 0, 0, -1, canUseOnPrepareCombat: false, allowUseInPlayAndTest: false, -1, 60, new List<short>(), new List<int>(), EMiscGenerateType.Invalid, new List<TreasureStateInfo>(), EMiscFilterType.Collection, -1, -1, -1, 0));
		_dataArray.Add(new MiscItem(252, 496, 12, 1200, 8, -1, "icon_Misc_huojing", 497, transferable: false, stackable: true, wagerable: false, refinable: false, poisonable: false, repairable: false, inheritable: false, consumable: false, 0, 0, 0, 0, 0, 0, 0, 0, allowRandomCreate: true, 0, isSpecial: true, -1, -1, -1, -1, 0, 0, 0, -1, canUseOnPrepareCombat: false, allowUseInPlayAndTest: false, -1, 60, new List<short>(), new List<int>(), EMiscGenerateType.Invalid, new List<TreasureStateInfo>(), EMiscFilterType.Collection, -1, -1, -1, 0));
		_dataArray.Add(new MiscItem(253, 498, 12, 1200, 8, -1, "icon_Misc_xuanzhoujinzhi", 499, transferable: false, stackable: true, wagerable: false, refinable: false, poisonable: false, repairable: false, inheritable: false, consumable: false, 0, 0, 0, 0, 0, 0, 0, 0, allowRandomCreate: true, 0, isSpecial: true, -1, -1, -1, -1, 0, 0, 0, -1, canUseOnPrepareCombat: false, allowUseInPlayAndTest: false, -1, 60, new List<short>(), new List<int>(), EMiscGenerateType.Invalid, new List<TreasureStateInfo>(), EMiscFilterType.Collection, -1, -1, -1, 0));
		_dataArray.Add(new MiscItem(254, 500, 12, 1200, 8, -1, "icon_Misc_nanhuozhu", 501, transferable: false, stackable: true, wagerable: false, refinable: false, poisonable: false, repairable: false, inheritable: false, consumable: false, 0, 0, 0, 0, 0, 0, 0, 0, allowRandomCreate: true, 0, isSpecial: true, -1, -1, -1, -1, 0, 0, 0, -1, canUseOnPrepareCombat: false, allowUseInPlayAndTest: false, -1, 60, new List<short>(), new List<int>(), EMiscGenerateType.Invalid, new List<TreasureStateInfo>(), EMiscFilterType.Collection, -1, -1, -1, 0));
		_dataArray.Add(new MiscItem(255, 502, 12, 1200, 8, -1, "icon_Misc_jiuchibanfu", 503, transferable: false, stackable: true, wagerable: false, refinable: false, poisonable: false, repairable: false, inheritable: false, consumable: false, 0, 0, 0, 0, 0, 0, 0, 0, allowRandomCreate: true, 0, isSpecial: true, -1, -1, -1, -1, 0, 0, 0, -1, canUseOnPrepareCombat: false, allowUseInPlayAndTest: false, -1, 60, new List<short>(), new List<int>(), EMiscGenerateType.Invalid, new List<TreasureStateInfo>(), EMiscFilterType.Collection, -1, -1, -1, 0));
		_dataArray.Add(new MiscItem(256, 504, 12, 1200, 8, -1, "icon_Misc_ziyuhuban", 505, transferable: false, stackable: true, wagerable: false, refinable: false, poisonable: false, repairable: false, inheritable: false, consumable: false, 0, 0, 0, 0, 0, 0, 0, 0, allowRandomCreate: true, 0, isSpecial: true, -1, -1, -1, -1, 0, 0, 0, -1, canUseOnPrepareCombat: false, allowUseInPlayAndTest: false, -1, 60, new List<short>(), new List<int>(), EMiscGenerateType.Invalid, new List<TreasureStateInfo>(), EMiscFilterType.Collection, -1, -1, -1, 0));
		_dataArray.Add(new MiscItem(257, 506, 12, 1200, 8, -1, "icon_Misc_xuandanzhishu", 507, transferable: false, stackable: true, wagerable: false, refinable: false, poisonable: false, repairable: false, inheritable: false, consumable: false, 0, 0, 0, 0, 0, 0, 0, 0, allowRandomCreate: true, 0, isSpecial: true, -1, -1, -1, -1, 0, 0, 0, -1, canUseOnPrepareCombat: false, allowUseInPlayAndTest: false, -1, 60, new List<short>(), new List<int>(), EMiscGenerateType.Invalid, new List<TreasureStateInfo>(), EMiscFilterType.Collection, -1, -1, -1, 0));
		_dataArray.Add(new MiscItem(258, 510, 12, 1200, 0, -1, "icon_Misc_xiyucanjing", 511, transferable: false, stackable: true, wagerable: false, refinable: false, poisonable: false, repairable: false, inheritable: false, consumable: false, 0, 0, 0, 0, 0, 0, 0, 0, allowRandomCreate: true, 0, isSpecial: true, -1, -1, -1, -1, 0, 0, 0, -1, canUseOnPrepareCombat: false, allowUseInPlayAndTest: false, -1, 60, new List<short>(), new List<int>(), EMiscGenerateType.Invalid, new List<TreasureStateInfo>(), EMiscFilterType.Collection, -1, -1, -1, 0));
		_dataArray.Add(new MiscItem(259, 512, 12, 1200, 7, -1, "icon_Misc_xiyuzhenjing", 513, transferable: false, stackable: true, wagerable: false, refinable: false, poisonable: false, repairable: false, inheritable: false, consumable: false, 0, 0, 0, 0, 0, 0, 0, 0, allowRandomCreate: true, 0, isSpecial: true, -1, -1, -1, -1, 0, 0, 0, -1, canUseOnPrepareCombat: false, allowUseInPlayAndTest: false, -1, 60, new List<short>(), new List<int>(), EMiscGenerateType.Invalid, new List<TreasureStateInfo>(), EMiscFilterType.Collection, -1, -1, -1, 0));
		_dataArray.Add(new MiscItem(260, 514, 12, 1200, 0, -1, "icon_Misc_cuozijiajing", 515, transferable: false, stackable: true, wagerable: false, refinable: false, poisonable: false, repairable: false, inheritable: false, consumable: false, 0, 0, 0, 0, 0, 0, 0, 0, allowRandomCreate: true, 0, isSpecial: true, -1, -1, -1, -1, 0, 0, 0, -1, canUseOnPrepareCombat: false, allowUseInPlayAndTest: false, -1, 60, new List<short>(), new List<int>(), EMiscGenerateType.Invalid, new List<TreasureStateInfo>(), EMiscFilterType.Collection, -1, -1, -1, 0));
		_dataArray.Add(new MiscItem(261, 516, 12, 1200, 7, -1, "icon_Misc_dongchuanzhenjing", 517, transferable: false, stackable: true, wagerable: false, refinable: false, poisonable: false, repairable: false, inheritable: false, consumable: false, 0, 0, 0, 0, 0, 0, 0, 0, allowRandomCreate: true, 0, isSpecial: true, -1, -1, -1, -1, 0, 0, 0, -1, canUseOnPrepareCombat: false, allowUseInPlayAndTest: false, -1, 60, new List<short>(), new List<int>(), EMiscGenerateType.Invalid, new List<TreasureStateInfo>(), EMiscFilterType.Collection, -1, -1, -1, 0));
		_dataArray.Add(new MiscItem(262, 518, 12, 1200, 0, -1, "icon_Misc_guguaidefopai", 519, transferable: false, stackable: true, wagerable: false, refinable: false, poisonable: false, repairable: false, inheritable: false, consumable: false, 0, 0, 0, 0, 0, 0, 0, 0, allowRandomCreate: true, 0, isSpecial: true, -1, -1, -1, -1, 0, 0, 0, -1, canUseOnPrepareCombat: false, allowUseInPlayAndTest: false, -1, 60, new List<short>(), new List<int>(), EMiscGenerateType.Invalid, new List<TreasureStateInfo>(), EMiscFilterType.Collection, -1, -1, -1, 0));
		_dataArray.Add(new MiscItem(263, 508, 12, 1200, 0, -1, "icon_Misc_latadaozhangsuoyiliudebaiyu", 509, transferable: false, stackable: true, wagerable: false, refinable: false, poisonable: false, repairable: false, inheritable: false, consumable: false, 0, 0, 0, 0, 0, 0, 0, 0, allowRandomCreate: true, 0, isSpecial: true, -1, -1, -1, -1, 0, 0, 0, -1, canUseOnPrepareCombat: false, allowUseInPlayAndTest: false, -1, 60, new List<short>(), new List<int>(), EMiscGenerateType.Invalid, new List<TreasureStateInfo>(), EMiscFilterType.Collection, -1, -1, -1, 0));
		_dataArray.Add(new MiscItem(264, 486, 12, 1200, 0, -1, "icon_Misc_angzangdediaoxiang", 520, transferable: false, stackable: true, wagerable: false, refinable: false, poisonable: false, repairable: false, inheritable: false, consumable: false, 0, 0, 0, 0, 0, 0, 0, 0, allowRandomCreate: true, 0, isSpecial: true, -1, -1, -1, -1, 0, 0, 0, -1, canUseOnPrepareCombat: false, allowUseInPlayAndTest: false, -1, 60, new List<short>(), new List<int>(), EMiscGenerateType.Invalid, new List<TreasureStateInfo>(), EMiscFilterType.Collection, -1, -1, -1, 0));
		_dataArray.Add(new MiscItem(265, 486, 12, 1200, 0, -1, "icon_Misc_angzangdediaoxiang2", 521, transferable: false, stackable: true, wagerable: false, refinable: false, poisonable: false, repairable: false, inheritable: false, consumable: false, 0, 0, 0, 0, 0, 0, 0, 0, allowRandomCreate: true, 0, isSpecial: true, -1, -1, -1, -1, 0, 0, 0, -1, canUseOnPrepareCombat: false, allowUseInPlayAndTest: false, -1, 60, new List<short>(), new List<int>(), EMiscGenerateType.Invalid, new List<TreasureStateInfo>(), EMiscFilterType.Collection, -1, -1, -1, 0));
		_dataArray.Add(new MiscItem(266, 522, 12, 1200, 8, -1, null, 523, transferable: false, stackable: true, wagerable: false, refinable: false, poisonable: false, repairable: false, inheritable: false, consumable: false, 0, 0, 0, 0, 0, 0, 0, 0, allowRandomCreate: true, 0, isSpecial: true, -1, -1, -1, -1, 0, 0, 0, -1, canUseOnPrepareCombat: false, allowUseInPlayAndTest: false, -1, 60, new List<short>(), new List<int>(), EMiscGenerateType.Invalid, new List<TreasureStateInfo>(), EMiscFilterType.Collection, -1, -1, -1, 0));
		_dataArray.Add(new MiscItem(267, 524, 12, 1200, 8, -1, "icon_Misc_zushimiling", 525, transferable: false, stackable: true, wagerable: false, refinable: false, poisonable: false, repairable: false, inheritable: false, consumable: false, 0, 0, 0, 0, 0, 0, 0, 0, allowRandomCreate: true, 0, isSpecial: true, -1, -1, -1, -1, 0, 0, 0, -1, canUseOnPrepareCombat: false, allowUseInPlayAndTest: false, -1, 60, new List<short>(), new List<int>(), EMiscGenerateType.Invalid, new List<TreasureStateInfo>(), EMiscFilterType.Collection, -1, -1, -1, 0));
		_dataArray.Add(new MiscItem(268, 526, 12, 1200, 0, -1, "icon_Misc_kailiepuyu", 527, transferable: false, stackable: true, wagerable: false, refinable: false, poisonable: false, repairable: false, inheritable: false, consumable: false, 0, 0, 0, 0, 0, 0, 0, 0, allowRandomCreate: true, 0, isSpecial: true, -1, -1, -1, -1, 0, 0, 0, -1, canUseOnPrepareCombat: false, allowUseInPlayAndTest: false, -1, 60, new List<short>(), new List<int>(), EMiscGenerateType.Invalid, new List<TreasureStateInfo>(), EMiscFilterType.Collection, -1, -1, -1, 0));
		_dataArray.Add(new MiscItem(269, 528, 12, 1200, 0, -1, "icon_Misc_duanbingmujian", 529, transferable: false, stackable: true, wagerable: false, refinable: false, poisonable: false, repairable: false, inheritable: false, consumable: false, 0, 0, 0, 0, 0, 0, 0, 0, allowRandomCreate: true, 0, isSpecial: true, -1, -1, -1, -1, 0, 0, 0, -1, canUseOnPrepareCombat: false, allowUseInPlayAndTest: false, -1, 60, new List<short>(), new List<int>(), EMiscGenerateType.Invalid, new List<TreasureStateInfo>(), EMiscFilterType.Collection, -1, -1, -1, 0));
		_dataArray.Add(new MiscItem(270, 530, 12, 1200, 0, -1, "icon_Misc_yinyanghetao", 531, transferable: false, stackable: true, wagerable: false, refinable: false, poisonable: false, repairable: false, inheritable: false, consumable: false, 0, 0, 0, 0, 0, 0, 0, 0, allowRandomCreate: true, 0, isSpecial: true, -1, -1, -1, -1, 0, 0, 0, -1, canUseOnPrepareCombat: false, allowUseInPlayAndTest: false, -1, 60, new List<short>(), new List<int>(), EMiscGenerateType.Invalid, new List<TreasureStateInfo>(), EMiscFilterType.Collection, -1, -1, -1, 0));
		_dataArray.Add(new MiscItem(271, 532, 12, 1200, 0, -1, "icon_Misc_angzangdediaoxiang3", 533, transferable: false, stackable: true, wagerable: false, refinable: false, poisonable: false, repairable: false, inheritable: false, consumable: false, 0, 0, 0, 0, 0, 0, 0, 0, allowRandomCreate: true, 0, isSpecial: true, -1, -1, -1, -1, 0, 0, 0, -1, canUseOnPrepareCombat: false, allowUseInPlayAndTest: false, -1, 60, new List<short>(), new List<int>(), EMiscGenerateType.Invalid, new List<TreasureStateInfo>(), EMiscFilterType.Collection, -1, -1, -1, 0));
		_dataArray.Add(new MiscItem(272, 534, 12, 1200, 5, 272, "icon_Misc_shenmuzhong", 535, transferable: false, stackable: true, wagerable: false, refinable: false, poisonable: false, repairable: false, inheritable: false, consumable: false, 0, 0, 0, 0, 0, 0, 0, 0, allowRandomCreate: true, 0, isSpecial: true, -1, -1, -1, -1, 0, 0, 0, -1, canUseOnPrepareCombat: false, allowUseInPlayAndTest: false, -1, 60, new List<short>(), new List<int>(), EMiscGenerateType.Invalid, new List<TreasureStateInfo>(), EMiscFilterType.Item, -1, -1, -1, 0));
		_dataArray.Add(new MiscItem(273, 536, 12, 1200, 5, 272, "icon_Misc_shenmuzhong", 535, transferable: false, stackable: true, wagerable: false, refinable: false, poisonable: false, repairable: false, inheritable: false, consumable: false, 0, 0, 0, 0, 0, 0, 0, 0, allowRandomCreate: true, 0, isSpecial: true, -1, -1, -1, -1, 0, 0, 0, -1, canUseOnPrepareCombat: false, allowUseInPlayAndTest: false, -1, 60, new List<short>(), new List<int>(), EMiscGenerateType.Invalid, new List<TreasureStateInfo>(), EMiscFilterType.Item, -1, -1, -1, 0));
		_dataArray.Add(new MiscItem(274, 537, 12, 1200, 5, 272, "icon_Misc_shenmuzhong", 535, transferable: false, stackable: true, wagerable: false, refinable: false, poisonable: false, repairable: false, inheritable: false, consumable: false, 0, 0, 0, 0, 0, 0, 0, 0, allowRandomCreate: true, 0, isSpecial: true, -1, -1, -1, -1, 0, 0, 0, -1, canUseOnPrepareCombat: false, allowUseInPlayAndTest: false, -1, 60, new List<short>(), new List<int>(), EMiscGenerateType.Invalid, new List<TreasureStateInfo>(), EMiscFilterType.Item, -1, -1, -1, 0));
		_dataArray.Add(new MiscItem(275, 538, 12, 1200, 5, 272, "icon_Misc_shenmuzhong", 535, transferable: false, stackable: true, wagerable: false, refinable: false, poisonable: false, repairable: false, inheritable: false, consumable: false, 0, 0, 0, 0, 0, 0, 0, 0, allowRandomCreate: true, 0, isSpecial: true, -1, -1, -1, -1, 0, 0, 0, -1, canUseOnPrepareCombat: false, allowUseInPlayAndTest: false, -1, 60, new List<short>(), new List<int>(), EMiscGenerateType.Invalid, new List<TreasureStateInfo>(), EMiscFilterType.Item, -1, -1, -1, 0));
		_dataArray.Add(new MiscItem(276, 539, 12, 1200, 5, 272, "icon_Misc_shenmuzhong", 535, transferable: false, stackable: true, wagerable: false, refinable: false, poisonable: false, repairable: false, inheritable: false, consumable: false, 0, 0, 0, 0, 0, 0, 0, 0, allowRandomCreate: true, 0, isSpecial: true, -1, -1, -1, -1, 0, 0, 0, -1, canUseOnPrepareCombat: false, allowUseInPlayAndTest: false, -1, 60, new List<short>(), new List<int>(), EMiscGenerateType.Invalid, new List<TreasureStateInfo>(), EMiscFilterType.Item, -1, -1, -1, 0));
		_dataArray.Add(new MiscItem(277, 540, 12, 1200, 5, 272, "icon_Misc_shenmuzhong", 535, transferable: false, stackable: true, wagerable: false, refinable: false, poisonable: false, repairable: false, inheritable: false, consumable: false, 0, 0, 0, 0, 0, 0, 0, 0, allowRandomCreate: true, 0, isSpecial: true, -1, -1, -1, -1, 0, 0, 0, -1, canUseOnPrepareCombat: false, allowUseInPlayAndTest: false, -1, 60, new List<short>(), new List<int>(), EMiscGenerateType.Invalid, new List<TreasureStateInfo>(), EMiscFilterType.Item, -1, -1, -1, 0));
		_dataArray.Add(new MiscItem(278, 541, 12, 1200, 5, 272, "icon_Misc_shenmuzhong", 535, transferable: false, stackable: true, wagerable: false, refinable: false, poisonable: false, repairable: false, inheritable: false, consumable: false, 0, 0, 0, 0, 0, 0, 0, 0, allowRandomCreate: true, 0, isSpecial: true, -1, -1, -1, -1, 0, 0, 0, -1, canUseOnPrepareCombat: false, allowUseInPlayAndTest: false, -1, 60, new List<short>(), new List<int>(), EMiscGenerateType.Invalid, new List<TreasureStateInfo>(), EMiscFilterType.Item, -1, -1, -1, 0));
		_dataArray.Add(new MiscItem(279, 542, 12, 1200, 5, 272, "icon_Misc_shenmuzhong", 535, transferable: false, stackable: true, wagerable: false, refinable: false, poisonable: false, repairable: false, inheritable: false, consumable: false, 0, 0, 0, 0, 0, 0, 0, 0, allowRandomCreate: true, 0, isSpecial: true, -1, -1, -1, -1, 0, 0, 0, -1, canUseOnPrepareCombat: false, allowUseInPlayAndTest: false, -1, 60, new List<short>(), new List<int>(), EMiscGenerateType.Invalid, new List<TreasureStateInfo>(), EMiscFilterType.Item, -1, -1, -1, 0));
		_dataArray.Add(new MiscItem(280, 543, 12, 1200, 5, 272, "icon_Misc_shenmuzhong", 535, transferable: false, stackable: true, wagerable: false, refinable: false, poisonable: false, repairable: false, inheritable: false, consumable: false, 0, 0, 0, 0, 0, 0, 0, 0, allowRandomCreate: true, 0, isSpecial: true, -1, -1, -1, -1, 0, 0, 0, -1, canUseOnPrepareCombat: false, allowUseInPlayAndTest: false, -1, 60, new List<short>(), new List<int>(), EMiscGenerateType.Invalid, new List<TreasureStateInfo>(), EMiscFilterType.Item, -1, -1, -1, 0));
		_dataArray.Add(new MiscItem(281, 544, 12, 1200, 5, 272, "icon_Misc_shenmuzhong", 535, transferable: false, stackable: true, wagerable: false, refinable: false, poisonable: false, repairable: false, inheritable: false, consumable: false, 0, 0, 0, 0, 0, 0, 0, 0, allowRandomCreate: true, 0, isSpecial: true, -1, -1, -1, -1, 0, 0, 0, -1, canUseOnPrepareCombat: false, allowUseInPlayAndTest: false, -1, 60, new List<short>(), new List<int>(), EMiscGenerateType.Invalid, new List<TreasureStateInfo>(), EMiscFilterType.Item, -1, -1, -1, 0));
		_dataArray.Add(new MiscItem(282, 545, 12, 1200, 5, 272, "icon_Misc_shenmuzhong", 535, transferable: false, stackable: true, wagerable: false, refinable: false, poisonable: false, repairable: false, inheritable: false, consumable: false, 0, 0, 0, 0, 0, 0, 0, 0, allowRandomCreate: true, 0, isSpecial: true, -1, -1, -1, -1, 0, 0, 0, -1, canUseOnPrepareCombat: false, allowUseInPlayAndTest: false, -1, 60, new List<short>(), new List<int>(), EMiscGenerateType.Invalid, new List<TreasureStateInfo>(), EMiscFilterType.Item, -1, -1, -1, 0));
		_dataArray.Add(new MiscItem(283, 546, 12, 1200, 5, 272, "icon_Misc_shenmuzhong", 535, transferable: false, stackable: true, wagerable: false, refinable: false, poisonable: false, repairable: false, inheritable: false, consumable: false, 0, 0, 0, 0, 0, 0, 0, 0, allowRandomCreate: true, 0, isSpecial: true, -1, -1, -1, -1, 0, 0, 0, -1, canUseOnPrepareCombat: false, allowUseInPlayAndTest: false, -1, 60, new List<short>(), new List<int>(), EMiscGenerateType.Invalid, new List<TreasureStateInfo>(), EMiscFilterType.Item, -1, -1, -1, 0));
		_dataArray.Add(new MiscItem(284, 547, 12, 1200, 5, -1, "icon_Misc_guluanjingshuiyao", 548, transferable: false, stackable: true, wagerable: false, refinable: false, poisonable: false, repairable: false, inheritable: false, consumable: false, 0, 0, 0, 0, 0, 0, 0, 0, allowRandomCreate: true, 0, isSpecial: true, -1, -1, -1, -1, 0, 0, 0, -1, canUseOnPrepareCombat: false, allowUseInPlayAndTest: false, -1, 60, new List<short>(), new List<int>(), EMiscGenerateType.Invalid, new List<TreasureStateInfo>(), EMiscFilterType.Item, -1, -1, -1, 0));
		_dataArray.Add(new MiscItem(285, 549, 12, 1200, 8, -1, "icon_Misc_yilvjinsi", 550, transferable: false, stackable: false, wagerable: false, refinable: false, poisonable: false, repairable: false, inheritable: false, consumable: false, -99, 0, 0, 0, 0, 0, 0, 0, allowRandomCreate: true, 0, isSpecial: true, -1, -1, -1, -1, 0, 0, 0, 0, canUseOnPrepareCombat: false, allowUseInPlayAndTest: false, 80, 60, new List<short> { 182, 183, 184, 185, 186 }, new List<int>(), EMiscGenerateType.Invalid, new List<TreasureStateInfo>(), EMiscFilterType.Item, 6, 4, -1, 0));
		_dataArray.Add(new MiscItem(286, 551, 12, 1200, 8, -1, "icon_Misc_longlin", 552, transferable: false, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, consumable: false, 0, 0, 0, 0, 0, 0, 0, 0, allowRandomCreate: true, 100, isSpecial: true, -1, -1, -1, -1, 0, 0, 0, -1, canUseOnPrepareCombat: false, allowUseInPlayAndTest: false, -1, 60, new List<short>(), new List<int>(), EMiscGenerateType.Invalid, new List<TreasureStateInfo>(), EMiscFilterType.Collection, -1, -1, -1, 0));
		_dataArray.Add(new MiscItem(287, 553, 12, 1200, 4, -1, null, 554, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, consumable: false, 0, 0, 5, 10, 0, 0, 50, 8, allowRandomCreate: true, 0, isSpecial: false, -1, -1, 36, -1, 0, 0, 0, -1, canUseOnPrepareCombat: false, allowUseInPlayAndTest: false, -1, 60, new List<short>(), new List<int>(), EMiscGenerateType.Invalid, new List<TreasureStateInfo>(), EMiscFilterType.Invalid, -1, -1, -1, 0));
		_dataArray.Add(new MiscItem(288, 555, 12, 1200, 4, -1, null, 556, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, consumable: false, 0, 0, 5, 10, 0, 0, 50, 8, allowRandomCreate: true, 0, isSpecial: false, -1, -1, 36, -1, 0, 0, 0, -1, canUseOnPrepareCombat: false, allowUseInPlayAndTest: false, -1, 60, new List<short>(), new List<int>(), EMiscGenerateType.Invalid, new List<TreasureStateInfo>(), EMiscFilterType.Invalid, -1, -1, -1, 0));
		_dataArray.Add(new MiscItem(289, 557, 12, 1200, 4, -1, null, 558, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, consumable: false, 0, 0, 5, 10, 0, 0, 50, 8, allowRandomCreate: true, 0, isSpecial: false, -1, -1, 36, -1, 0, 0, 0, -1, canUseOnPrepareCombat: false, allowUseInPlayAndTest: false, -1, 60, new List<short>(), new List<int>(), EMiscGenerateType.Invalid, new List<TreasureStateInfo>(), EMiscFilterType.Invalid, -1, -1, -1, 0));
		_dataArray.Add(new MiscItem(290, 559, 12, 1200, 4, -1, null, 560, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, consumable: false, 0, 0, 5, 10, 0, 0, 50, 8, allowRandomCreate: true, 0, isSpecial: false, -1, -1, 36, -1, 0, 0, 0, -1, canUseOnPrepareCombat: false, allowUseInPlayAndTest: false, -1, 60, new List<short>(), new List<int>(), EMiscGenerateType.Invalid, new List<TreasureStateInfo>(), EMiscFilterType.Invalid, -1, -1, -1, 0));
		_dataArray.Add(new MiscItem(291, 561, 12, 1200, 4, -1, null, 562, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, consumable: false, 0, 0, 5, 10, 0, 0, 50, 8, allowRandomCreate: true, 0, isSpecial: false, -1, -1, 36, -1, 0, 0, 0, -1, canUseOnPrepareCombat: false, allowUseInPlayAndTest: false, -1, 60, new List<short>(), new List<int>(), EMiscGenerateType.Invalid, new List<TreasureStateInfo>(), EMiscFilterType.Invalid, -1, -1, -1, 0));
		_dataArray.Add(new MiscItem(292, 563, 12, 1200, 4, -1, null, 564, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, consumable: false, 0, 0, 5, 10, 0, 0, 50, 8, allowRandomCreate: true, 0, isSpecial: false, -1, -1, 36, -1, 0, 0, 0, -1, canUseOnPrepareCombat: false, allowUseInPlayAndTest: false, -1, 60, new List<short>(), new List<int>(), EMiscGenerateType.Invalid, new List<TreasureStateInfo>(), EMiscFilterType.Invalid, -1, -1, -1, 0));
		_dataArray.Add(new MiscItem(293, 565, 12, 1200, 4, -1, null, 566, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, consumable: false, 0, 0, 5, 10, 0, 0, 50, 8, allowRandomCreate: true, 0, isSpecial: false, -1, -1, 36, -1, 0, 0, 0, -1, canUseOnPrepareCombat: false, allowUseInPlayAndTest: false, -1, 60, new List<short>(), new List<int>(), EMiscGenerateType.Invalid, new List<TreasureStateInfo>(), EMiscFilterType.Invalid, -1, -1, -1, 0));
		_dataArray.Add(new MiscItem(294, 567, 12, 1200, 5, -1, null, 568, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, consumable: false, 0, 0, 5, 10, 0, 0, 50, 8, allowRandomCreate: true, 0, isSpecial: false, -1, -1, 36, -1, 0, 0, 0, -1, canUseOnPrepareCombat: false, allowUseInPlayAndTest: false, -1, 60, new List<short>(), new List<int>(), EMiscGenerateType.Invalid, new List<TreasureStateInfo>(), EMiscFilterType.Invalid, -1, -1, -1, 0));
		_dataArray.Add(new MiscItem(295, 569, 12, 1200, 5, -1, null, 570, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, consumable: false, 0, 0, 5, 10, 0, 0, 50, 8, allowRandomCreate: true, 0, isSpecial: false, -1, -1, 36, -1, 0, 0, 0, -1, canUseOnPrepareCombat: false, allowUseInPlayAndTest: false, -1, 60, new List<short>(), new List<int>(), EMiscGenerateType.Invalid, new List<TreasureStateInfo>(), EMiscFilterType.Invalid, -1, -1, -1, 0));
		_dataArray.Add(new MiscItem(296, 571, 12, 1200, 5, -1, null, 572, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, consumable: false, 0, 0, 5, 10, 0, 0, 50, 8, allowRandomCreate: true, 0, isSpecial: false, -1, -1, 36, -1, 0, 0, 0, -1, canUseOnPrepareCombat: false, allowUseInPlayAndTest: false, -1, 60, new List<short>(), new List<int>(), EMiscGenerateType.Invalid, new List<TreasureStateInfo>(), EMiscFilterType.Invalid, -1, -1, -1, 0));
		_dataArray.Add(new MiscItem(297, 573, 12, 1200, 5, -1, null, 574, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, consumable: false, 0, 0, 5, 10, 0, 0, 50, 8, allowRandomCreate: true, 0, isSpecial: false, -1, -1, 36, -1, 0, 0, 0, -1, canUseOnPrepareCombat: false, allowUseInPlayAndTest: false, -1, 60, new List<short>(), new List<int>(), EMiscGenerateType.Invalid, new List<TreasureStateInfo>(), EMiscFilterType.Invalid, -1, -1, -1, 0));
		_dataArray.Add(new MiscItem(298, 575, 12, 1200, 5, -1, null, 576, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, consumable: false, 0, 0, 5, 10, 0, 0, 50, 8, allowRandomCreate: true, 0, isSpecial: false, -1, -1, 36, -1, 0, 0, 0, -1, canUseOnPrepareCombat: false, allowUseInPlayAndTest: false, -1, 60, new List<short>(), new List<int>(), EMiscGenerateType.Invalid, new List<TreasureStateInfo>(), EMiscFilterType.Invalid, -1, -1, -1, 0));
		_dataArray.Add(new MiscItem(299, 577, 12, 1200, 5, -1, null, 578, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, consumable: false, 0, 0, 5, 10, 0, 0, 50, 8, allowRandomCreate: true, 0, isSpecial: false, -1, -1, 36, -1, 0, 0, 0, -1, canUseOnPrepareCombat: false, allowUseInPlayAndTest: false, -1, 60, new List<short>(), new List<int>(), EMiscGenerateType.Invalid, new List<TreasureStateInfo>(), EMiscFilterType.Invalid, -1, -1, -1, 0));
	}

	private void CreateItems5()
	{
		_dataArray.Add(new MiscItem(300, 579, 12, 1200, 5, -1, null, 580, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, consumable: false, 0, 0, 5, 10, 0, 0, 50, 8, allowRandomCreate: true, 0, isSpecial: false, -1, -1, 36, -1, 0, 0, 0, -1, canUseOnPrepareCombat: false, allowUseInPlayAndTest: false, -1, 60, new List<short>(), new List<int>(), EMiscGenerateType.Invalid, new List<TreasureStateInfo>(), EMiscFilterType.Invalid, -1, -1, -1, 0));
		_dataArray.Add(new MiscItem(301, 581, 12, 1200, 5, -1, null, 582, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, consumable: false, 0, 0, 5, 10, 0, 0, 50, 8, allowRandomCreate: true, 0, isSpecial: false, -1, -1, 36, -1, 0, 0, 0, -1, canUseOnPrepareCombat: false, allowUseInPlayAndTest: false, -1, 60, new List<short>(), new List<int>(), EMiscGenerateType.Invalid, new List<TreasureStateInfo>(), EMiscFilterType.Invalid, -1, -1, -1, 0));
		_dataArray.Add(new MiscItem(302, 583, 12, 1200, 5, -1, null, 584, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, consumable: false, 0, 0, 5, 10, 0, 0, 50, 8, allowRandomCreate: true, 0, isSpecial: false, -1, -1, 36, -1, 0, 0, 0, -1, canUseOnPrepareCombat: false, allowUseInPlayAndTest: false, -1, 60, new List<short>(), new List<int>(), EMiscGenerateType.Invalid, new List<TreasureStateInfo>(), EMiscFilterType.Invalid, -1, -1, -1, 0));
		_dataArray.Add(new MiscItem(303, 585, 12, 1200, 5, -1, null, 586, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, consumable: false, 0, 0, 5, 10, 0, 0, 50, 8, allowRandomCreate: true, 0, isSpecial: false, -1, -1, 36, -1, 0, 0, 0, -1, canUseOnPrepareCombat: false, allowUseInPlayAndTest: false, -1, 60, new List<short>(), new List<int>(), EMiscGenerateType.Invalid, new List<TreasureStateInfo>(), EMiscFilterType.Invalid, -1, -1, -1, 0));
		_dataArray.Add(new MiscItem(304, 587, 12, 1200, 6, -1, null, 588, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, consumable: false, 0, 0, 5, 10, 0, 0, 50, 8, allowRandomCreate: true, 0, isSpecial: false, -1, -1, 36, -1, 0, 0, 0, -1, canUseOnPrepareCombat: false, allowUseInPlayAndTest: false, -1, 60, new List<short>(), new List<int>(), EMiscGenerateType.Invalid, new List<TreasureStateInfo>(), EMiscFilterType.Invalid, -1, -1, -1, 0));
		_dataArray.Add(new MiscItem(305, 589, 12, 1200, 6, -1, null, 590, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, consumable: false, 0, 0, 5, 10, 0, 0, 50, 8, allowRandomCreate: true, 0, isSpecial: false, -1, -1, 36, -1, 0, 0, 0, -1, canUseOnPrepareCombat: false, allowUseInPlayAndTest: false, -1, 60, new List<short>(), new List<int>(), EMiscGenerateType.Invalid, new List<TreasureStateInfo>(), EMiscFilterType.Invalid, -1, -1, -1, 0));
		_dataArray.Add(new MiscItem(306, 591, 12, 1200, 6, -1, null, 592, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, consumable: false, 0, 0, 5, 10, 0, 0, 50, 8, allowRandomCreate: true, 0, isSpecial: false, -1, -1, 36, -1, 0, 0, 0, -1, canUseOnPrepareCombat: false, allowUseInPlayAndTest: false, -1, 60, new List<short>(), new List<int>(), EMiscGenerateType.Invalid, new List<TreasureStateInfo>(), EMiscFilterType.Invalid, -1, -1, -1, 0));
		_dataArray.Add(new MiscItem(307, 593, 12, 1200, 6, -1, null, 594, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, consumable: false, 0, 0, 5, 10, 0, 0, 50, 8, allowRandomCreate: true, 0, isSpecial: false, -1, -1, 36, -1, 0, 0, 0, -1, canUseOnPrepareCombat: false, allowUseInPlayAndTest: false, -1, 60, new List<short>(), new List<int>(), EMiscGenerateType.Invalid, new List<TreasureStateInfo>(), EMiscFilterType.Invalid, -1, -1, -1, 0));
		_dataArray.Add(new MiscItem(308, 595, 12, 1200, 6, -1, null, 596, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, consumable: false, 0, 0, 5, 10, 0, 0, 50, 8, allowRandomCreate: true, 0, isSpecial: false, -1, -1, 36, -1, 0, 0, 0, -1, canUseOnPrepareCombat: false, allowUseInPlayAndTest: false, -1, 60, new List<short>(), new List<int>(), EMiscGenerateType.Invalid, new List<TreasureStateInfo>(), EMiscFilterType.Invalid, -1, -1, -1, 0));
		_dataArray.Add(new MiscItem(309, 597, 12, 1200, 6, -1, null, 598, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, consumable: false, 0, 0, 5, 10, 0, 0, 50, 8, allowRandomCreate: true, 0, isSpecial: false, -1, -1, 36, -1, 0, 0, 0, -1, canUseOnPrepareCombat: false, allowUseInPlayAndTest: false, -1, 60, new List<short>(), new List<int>(), EMiscGenerateType.Invalid, new List<TreasureStateInfo>(), EMiscFilterType.Invalid, -1, -1, -1, 0));
		_dataArray.Add(new MiscItem(310, 599, 12, 1200, 6, -1, null, 600, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, consumable: false, 0, 0, 5, 10, 0, 0, 50, 8, allowRandomCreate: true, 0, isSpecial: false, -1, -1, 36, -1, 0, 0, 0, -1, canUseOnPrepareCombat: false, allowUseInPlayAndTest: false, -1, 60, new List<short>(), new List<int>(), EMiscGenerateType.Invalid, new List<TreasureStateInfo>(), EMiscFilterType.Invalid, -1, -1, -1, 0));
		_dataArray.Add(new MiscItem(311, 601, 12, 1200, 6, -1, null, 602, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, consumable: false, 0, 0, 5, 10, 0, 0, 50, 8, allowRandomCreate: true, 0, isSpecial: false, -1, -1, 36, -1, 0, 0, 0, -1, canUseOnPrepareCombat: false, allowUseInPlayAndTest: false, -1, 60, new List<short>(), new List<int>(), EMiscGenerateType.Invalid, new List<TreasureStateInfo>(), EMiscFilterType.Invalid, -1, -1, -1, 0));
		_dataArray.Add(new MiscItem(312, 603, 12, 1200, 6, -1, null, 604, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, consumable: false, 0, 0, 5, 10, 0, 0, 50, 8, allowRandomCreate: true, 0, isSpecial: false, -1, -1, 36, -1, 0, 0, 0, -1, canUseOnPrepareCombat: false, allowUseInPlayAndTest: false, -1, 60, new List<short>(), new List<int>(), EMiscGenerateType.Invalid, new List<TreasureStateInfo>(), EMiscFilterType.Invalid, -1, -1, -1, 0));
		_dataArray.Add(new MiscItem(313, 605, 12, 1200, 6, -1, null, 606, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, consumable: false, 0, 0, 5, 10, 0, 0, 50, 8, allowRandomCreate: true, 0, isSpecial: false, -1, -1, 36, -1, 0, 0, 0, -1, canUseOnPrepareCombat: false, allowUseInPlayAndTest: false, -1, 60, new List<short>(), new List<int>(), EMiscGenerateType.Invalid, new List<TreasureStateInfo>(), EMiscFilterType.Invalid, -1, -1, -1, 0));
		_dataArray.Add(new MiscItem(314, 607, 12, 1200, 7, -1, null, 608, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, consumable: false, 0, 0, 5, 10, 0, 0, 50, 8, allowRandomCreate: true, 0, isSpecial: false, -1, -1, 36, -1, 0, 0, 0, -1, canUseOnPrepareCombat: false, allowUseInPlayAndTest: false, -1, 60, new List<short>(), new List<int>(), EMiscGenerateType.Invalid, new List<TreasureStateInfo>(), EMiscFilterType.Invalid, -1, -1, -1, 0));
		_dataArray.Add(new MiscItem(315, 609, 12, 1200, 7, -1, null, 610, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, consumable: false, 0, 0, 5, 10, 0, 0, 50, 8, allowRandomCreate: true, 0, isSpecial: false, -1, -1, 36, -1, 0, 0, 0, -1, canUseOnPrepareCombat: false, allowUseInPlayAndTest: false, -1, 60, new List<short>(), new List<int>(), EMiscGenerateType.Invalid, new List<TreasureStateInfo>(), EMiscFilterType.Invalid, -1, -1, -1, 0));
		_dataArray.Add(new MiscItem(316, 611, 12, 1200, 7, -1, null, 612, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, consumable: false, 0, 0, 5, 10, 0, 0, 50, 8, allowRandomCreate: true, 0, isSpecial: false, -1, -1, 36, -1, 0, 0, 0, -1, canUseOnPrepareCombat: false, allowUseInPlayAndTest: false, -1, 60, new List<short>(), new List<int>(), EMiscGenerateType.Invalid, new List<TreasureStateInfo>(), EMiscFilterType.Invalid, -1, -1, -1, 0));
		_dataArray.Add(new MiscItem(317, 613, 12, 1200, 7, -1, null, 614, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, consumable: false, 0, 0, 5, 10, 0, 0, 50, 8, allowRandomCreate: true, 0, isSpecial: false, -1, -1, 36, -1, 0, 0, 0, -1, canUseOnPrepareCombat: false, allowUseInPlayAndTest: false, -1, 60, new List<short>(), new List<int>(), EMiscGenerateType.Invalid, new List<TreasureStateInfo>(), EMiscFilterType.Invalid, -1, -1, -1, 0));
		_dataArray.Add(new MiscItem(318, 615, 12, 1200, 7, -1, null, 616, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, consumable: false, 0, 0, 5, 10, 0, 0, 50, 8, allowRandomCreate: true, 0, isSpecial: false, -1, -1, 36, -1, 0, 0, 0, -1, canUseOnPrepareCombat: false, allowUseInPlayAndTest: false, -1, 60, new List<short>(), new List<int>(), EMiscGenerateType.Invalid, new List<TreasureStateInfo>(), EMiscFilterType.Invalid, -1, -1, -1, 0));
		_dataArray.Add(new MiscItem(319, 617, 12, 1200, 8, -1, null, 618, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, consumable: false, 0, 0, 5, 10, 0, 0, 50, 8, allowRandomCreate: true, 0, isSpecial: false, -1, -1, 36, -1, 0, 0, 0, -1, canUseOnPrepareCombat: false, allowUseInPlayAndTest: false, -1, 60, new List<short>(), new List<int>(), EMiscGenerateType.Invalid, new List<TreasureStateInfo>(), EMiscFilterType.Invalid, -1, -1, -1, 0));
		_dataArray.Add(new MiscItem(320, 619, 12, 1200, 8, -1, "icon_Misc_guxian", 620, transferable: false, stackable: false, wagerable: false, refinable: false, poisonable: false, repairable: false, inheritable: false, consumable: false, 0, 0, 0, 0, 0, 0, 0, 0, allowRandomCreate: true, 0, isSpecial: true, -1, -1, -1, -1, 0, 0, 0, -1, canUseOnPrepareCombat: false, allowUseInPlayAndTest: false, -1, 60, new List<short>(), new List<int>(), EMiscGenerateType.Invalid, new List<TreasureStateInfo>(), EMiscFilterType.Item, -1, -1, -1, 0));
		_dataArray.Add(new MiscItem(321, 621, 12, 1200, 0, 321, null, 622, transferable: false, stackable: false, wagerable: false, refinable: false, poisonable: false, repairable: false, inheritable: true, consumable: false, 0, 0, 5, 5, 0, 0, 50, 8, allowRandomCreate: true, 0, isSpecial: false, -1, -1, 36, -1, 0, 0, 0, -1, canUseOnPrepareCombat: false, allowUseInPlayAndTest: false, -1, 60, new List<short>(), new List<int>(), EMiscGenerateType.Invalid, new List<TreasureStateInfo>(), EMiscFilterType.Resource, -1, -1, -1, 0));
		_dataArray.Add(new MiscItem(322, 623, 12, 1200, 0, 321, null, 624, transferable: false, stackable: false, wagerable: false, refinable: false, poisonable: false, repairable: false, inheritable: true, consumable: false, 0, 0, 5, 5, 0, 0, 50, 8, allowRandomCreate: true, 0, isSpecial: false, -1, -1, 36, -1, 0, 0, 0, -1, canUseOnPrepareCombat: false, allowUseInPlayAndTest: false, -1, 60, new List<short>(), new List<int>(), EMiscGenerateType.Invalid, new List<TreasureStateInfo>(), EMiscFilterType.Resource, -1, -1, -1, 0));
		_dataArray.Add(new MiscItem(323, 625, 12, 1200, 0, 321, null, 626, transferable: false, stackable: false, wagerable: false, refinable: false, poisonable: false, repairable: false, inheritable: true, consumable: false, 0, 0, 5, 5, 0, 0, 50, 8, allowRandomCreate: true, 0, isSpecial: false, -1, -1, 36, -1, 0, 0, 0, -1, canUseOnPrepareCombat: false, allowUseInPlayAndTest: false, -1, 60, new List<short>(), new List<int>(), EMiscGenerateType.Invalid, new List<TreasureStateInfo>(), EMiscFilterType.Resource, -1, -1, -1, 0));
		_dataArray.Add(new MiscItem(324, 627, 12, 1200, 0, 321, null, 628, transferable: false, stackable: false, wagerable: false, refinable: false, poisonable: false, repairable: false, inheritable: true, consumable: false, 0, 0, 5, 5, 0, 0, 50, 8, allowRandomCreate: true, 0, isSpecial: false, -1, -1, 36, -1, 0, 0, 0, -1, canUseOnPrepareCombat: false, allowUseInPlayAndTest: false, -1, 60, new List<short>(), new List<int>(), EMiscGenerateType.Invalid, new List<TreasureStateInfo>(), EMiscFilterType.Resource, -1, -1, -1, 0));
		_dataArray.Add(new MiscItem(325, 629, 12, 1200, 0, 321, null, 630, transferable: false, stackable: false, wagerable: false, refinable: false, poisonable: false, repairable: false, inheritable: true, consumable: false, 0, 0, 5, 5, 0, 0, 50, 8, allowRandomCreate: true, 0, isSpecial: false, -1, -1, 36, -1, 0, 0, 0, -1, canUseOnPrepareCombat: false, allowUseInPlayAndTest: false, -1, 60, new List<short>(), new List<int>(), EMiscGenerateType.Invalid, new List<TreasureStateInfo>(), EMiscFilterType.Resource, -1, -1, -1, 0));
		_dataArray.Add(new MiscItem(326, 631, 12, 1200, 0, 321, null, 632, transferable: false, stackable: false, wagerable: false, refinable: false, poisonable: false, repairable: false, inheritable: true, consumable: false, 0, 0, 5, 5, 0, 0, 50, 8, allowRandomCreate: true, 0, isSpecial: false, -1, -1, 36, -1, 0, 0, 0, -1, canUseOnPrepareCombat: false, allowUseInPlayAndTest: false, -1, 60, new List<short>(), new List<int>(), EMiscGenerateType.Invalid, new List<TreasureStateInfo>(), EMiscFilterType.Resource, -1, -1, -1, 0));
		_dataArray.Add(new MiscItem(327, 633, 12, 1200, 0, 321, null, 634, transferable: false, stackable: false, wagerable: false, refinable: false, poisonable: false, repairable: false, inheritable: true, consumable: false, 0, 0, 5, 1, 0, 0, 50, 8, allowRandomCreate: true, 0, isSpecial: true, -1, -1, 36, -1, 0, 0, 0, -1, canUseOnPrepareCombat: false, allowUseInPlayAndTest: false, -1, 60, new List<short>(), new List<int>(), EMiscGenerateType.Invalid, new List<TreasureStateInfo>(), EMiscFilterType.Resource, -1, -1, -1, 0));
		_dataArray.Add(new MiscItem(328, 635, 12, 1200, 0, 321, null, 636, transferable: false, stackable: false, wagerable: false, refinable: false, poisonable: false, repairable: false, inheritable: true, consumable: false, 0, 0, 5, 10, 0, 0, 50, 8, allowRandomCreate: true, 0, isSpecial: true, -1, -1, 36, -1, 0, 0, 0, -1, canUseOnPrepareCombat: false, allowUseInPlayAndTest: false, -1, 60, new List<short>(), new List<int>(), EMiscGenerateType.Invalid, new List<TreasureStateInfo>(), EMiscFilterType.Resource, -1, -1, -1, 0));
		_dataArray.Add(new MiscItem(329, 637, 12, 1200, 0, -1, "icon_Misc_fuyujiancanpian", 638, transferable: false, stackable: true, wagerable: false, refinable: false, poisonable: false, repairable: false, inheritable: false, consumable: false, 0, 0, 0, 0, 0, 0, 0, 0, allowRandomCreate: true, 0, isSpecial: true, -1, -1, -1, -1, 0, 0, 0, -1, canUseOnPrepareCombat: false, allowUseInPlayAndTest: false, -1, 60, new List<short>(), new List<int>(), EMiscGenerateType.Invalid, new List<TreasureStateInfo>(), EMiscFilterType.Collection, -1, -1, -1, 0));
		_dataArray.Add(new MiscItem(330, 639, 12, 1200, 6, -1, "icon_Misc_yuanjiyu", 640, transferable: false, stackable: true, wagerable: false, refinable: false, poisonable: false, repairable: false, inheritable: false, consumable: false, 0, 0, 0, 0, 0, 0, 0, 0, allowRandomCreate: true, 0, isSpecial: true, -1, -1, -1, -1, 0, 0, 0, -1, canUseOnPrepareCombat: false, allowUseInPlayAndTest: false, -1, 60, new List<short>(), new List<int>(), EMiscGenerateType.Invalid, new List<TreasureStateInfo>(), EMiscFilterType.Collection, -1, -1, -1, 0));
		_dataArray.Add(new MiscItem(331, 641, 12, 1200, 8, -1, "icon_Misc_wucaiyuyi", 642, transferable: false, stackable: false, wagerable: false, refinable: false, poisonable: false, repairable: false, inheritable: false, consumable: false, 0, 0, 5, 10, 0, 0, 50, 8, allowRandomCreate: true, 0, isSpecial: true, 4, 276, -1, -1, 0, 0, 0, -1, canUseOnPrepareCombat: false, allowUseInPlayAndTest: false, -1, 60, new List<short>(), new List<int>(), EMiscGenerateType.Invalid, new List<TreasureStateInfo>(), EMiscFilterType.Collection, -1, -1, -1, 0));
		_dataArray.Add(new MiscItem(332, 643, 12, 1200, 0, -1, "icon_Misc_shenjitu", 644, transferable: false, stackable: false, wagerable: false, refinable: false, poisonable: false, repairable: false, inheritable: false, consumable: false, 0, 0, 5, 10, 0, 0, 0, 0, allowRandomCreate: true, 0, isSpecial: true, -1, -1, -1, -1, 0, 0, 0, -1, canUseOnPrepareCombat: false, allowUseInPlayAndTest: false, -1, 60, new List<short>(), new List<int>(), EMiscGenerateType.Invalid, new List<TreasureStateInfo>(), EMiscFilterType.Item, -1, -1, -1, 0));
		_dataArray.Add(new MiscItem(333, 645, 12, 1205, 1, -1, "icon_Misc_shuijing", 646, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, consumable: false, 0, 50, 200, 2000, 1, 2, 600, 4, allowRandomCreate: true, 0, isSpecial: true, 0, -1, 36, -1, 0, 0, 0, -1, canUseOnPrepareCombat: false, allowUseInPlayAndTest: false, -1, 60, new List<short>(), new List<int> { 1, 2, 3 }, EMiscGenerateType.V78R31, new List<TreasureStateInfo>
		{
			new TreasureStateInfo(1, 15),
			new TreasureStateInfo(2, 15),
			new TreasureStateInfo(3, 15),
			new TreasureStateInfo(4, 15),
			new TreasureStateInfo(5, 15),
			new TreasureStateInfo(6, 15),
			new TreasureStateInfo(7, 15),
			new TreasureStateInfo(8, 15),
			new TreasureStateInfo(9, 15),
			new TreasureStateInfo(10, 15),
			new TreasureStateInfo(11, 15),
			new TreasureStateInfo(12, 15),
			new TreasureStateInfo(13, 15),
			new TreasureStateInfo(14, 15),
			new TreasureStateInfo(15, 15)
		}, EMiscFilterType.KeyItem, -1, -1, -1, 200));
		_dataArray.Add(new MiscItem(334, 647, 12, 1205, 1, -1, "icon_Misc_huoshi", 648, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, consumable: false, 0, 600, 200, 2000, 1, 2, 600, 4, allowRandomCreate: true, 0, isSpecial: true, 2, -1, 36, -1, 0, 0, 0, -1, canUseOnPrepareCombat: false, allowUseInPlayAndTest: false, -1, 60, new List<short>(), new List<int> { 1, 2, 3 }, EMiscGenerateType.V78R31, new List<TreasureStateInfo>
		{
			new TreasureStateInfo(1, 15),
			new TreasureStateInfo(2, 15),
			new TreasureStateInfo(3, 15),
			new TreasureStateInfo(4, 15),
			new TreasureStateInfo(5, 15),
			new TreasureStateInfo(6, 15),
			new TreasureStateInfo(7, 15),
			new TreasureStateInfo(8, 15),
			new TreasureStateInfo(9, 15),
			new TreasureStateInfo(10, 15),
			new TreasureStateInfo(11, 15),
			new TreasureStateInfo(12, 15),
			new TreasureStateInfo(13, 15),
			new TreasureStateInfo(14, 15),
			new TreasureStateInfo(15, 15)
		}, EMiscFilterType.KeyItem, -1, -1, -1, 200));
		_dataArray.Add(new MiscItem(335, 649, 12, 1205, 1, -1, "icon_Misc_qingqingmiaomu", 650, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, consumable: false, 0, 600, 200, 2000, 1, 2, 600, 4, allowRandomCreate: true, 0, isSpecial: true, 1, -1, 36, -1, 0, 0, 0, -1, canUseOnPrepareCombat: false, allowUseInPlayAndTest: false, -1, 60, new List<short>(), new List<int> { 1, 2, 3 }, EMiscGenerateType.V78R31, new List<TreasureStateInfo>
		{
			new TreasureStateInfo(1, 15),
			new TreasureStateInfo(2, 15),
			new TreasureStateInfo(3, 15),
			new TreasureStateInfo(4, 15),
			new TreasureStateInfo(5, 15),
			new TreasureStateInfo(6, 15),
			new TreasureStateInfo(7, 15),
			new TreasureStateInfo(8, 15),
			new TreasureStateInfo(9, 15),
			new TreasureStateInfo(10, 15),
			new TreasureStateInfo(11, 15),
			new TreasureStateInfo(12, 15),
			new TreasureStateInfo(13, 15),
			new TreasureStateInfo(14, 15),
			new TreasureStateInfo(15, 15)
		}, EMiscFilterType.KeyItem, -1, -1, -1, 200));
		_dataArray.Add(new MiscItem(336, 651, 12, 1205, 1, -1, "icon_Misc_guyan", 652, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, consumable: false, 0, 600, 200, 2000, 1, 2, 600, 4, allowRandomCreate: true, 0, isSpecial: true, 2, -1, 36, -1, 0, 0, 0, -1, canUseOnPrepareCombat: false, allowUseInPlayAndTest: false, -1, 60, new List<short>(), new List<int> { 1, 2, 3 }, EMiscGenerateType.V78R31, new List<TreasureStateInfo>
		{
			new TreasureStateInfo(1, 15),
			new TreasureStateInfo(2, 15),
			new TreasureStateInfo(3, 15),
			new TreasureStateInfo(4, 15),
			new TreasureStateInfo(5, 15),
			new TreasureStateInfo(6, 15),
			new TreasureStateInfo(7, 15),
			new TreasureStateInfo(8, 15),
			new TreasureStateInfo(9, 15),
			new TreasureStateInfo(10, 15),
			new TreasureStateInfo(11, 15),
			new TreasureStateInfo(12, 15),
			new TreasureStateInfo(13, 15),
			new TreasureStateInfo(14, 15),
			new TreasureStateInfo(15, 15)
		}, EMiscFilterType.KeyItem, -1, -1, -1, 200));
		_dataArray.Add(new MiscItem(337, 653, 12, 1205, 1, -1, "icon_Misc_baicaoyaozi", 654, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, consumable: false, 0, 100, 200, 2000, 1, 2, 600, 4, allowRandomCreate: true, 0, isSpecial: true, 5, -1, 36, -1, 0, 0, 0, -1, canUseOnPrepareCombat: false, allowUseInPlayAndTest: false, -1, 60, new List<short>(), new List<int> { 1, 2, 3 }, EMiscGenerateType.V78R31, new List<TreasureStateInfo>
		{
			new TreasureStateInfo(1, 15),
			new TreasureStateInfo(2, 15),
			new TreasureStateInfo(3, 15),
			new TreasureStateInfo(4, 15),
			new TreasureStateInfo(5, 15),
			new TreasureStateInfo(6, 15),
			new TreasureStateInfo(7, 15),
			new TreasureStateInfo(8, 15),
			new TreasureStateInfo(9, 15),
			new TreasureStateInfo(10, 15),
			new TreasureStateInfo(11, 15),
			new TreasureStateInfo(12, 15),
			new TreasureStateInfo(13, 15),
			new TreasureStateInfo(14, 15),
			new TreasureStateInfo(15, 15)
		}, EMiscFilterType.KeyItem, -1, -1, -1, 200));
		_dataArray.Add(new MiscItem(338, 655, 12, 1205, 1, -1, "icon_Misc_funi", 656, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, consumable: false, 0, 500, 200, 2000, 1, 2, 600, 4, allowRandomCreate: true, 0, isSpecial: true, 5, -1, 36, -1, 0, 0, 0, -1, canUseOnPrepareCombat: false, allowUseInPlayAndTest: false, -1, 60, new List<short>(), new List<int> { 1, 2, 3 }, EMiscGenerateType.V78R31, new List<TreasureStateInfo>
		{
			new TreasureStateInfo(1, 15),
			new TreasureStateInfo(2, 15),
			new TreasureStateInfo(3, 15),
			new TreasureStateInfo(4, 15),
			new TreasureStateInfo(5, 15),
			new TreasureStateInfo(6, 15),
			new TreasureStateInfo(7, 15),
			new TreasureStateInfo(8, 15),
			new TreasureStateInfo(9, 15),
			new TreasureStateInfo(10, 15),
			new TreasureStateInfo(11, 15),
			new TreasureStateInfo(12, 15),
			new TreasureStateInfo(13, 15),
			new TreasureStateInfo(14, 15),
			new TreasureStateInfo(15, 15)
		}, EMiscFilterType.KeyItem, -1, -1, -1, 200));
		_dataArray.Add(new MiscItem(339, 659, 12, 1205, 1, -1, "icon_Misc_puyu", 660, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, consumable: false, 0, 500, 200, 2000, 1, 2, 600, 4, allowRandomCreate: true, 0, isSpecial: true, 3, -1, 36, -1, 0, 0, 0, -1, canUseOnPrepareCombat: false, allowUseInPlayAndTest: false, -1, 60, new List<short>(), new List<int> { 1, 2, 3 }, EMiscGenerateType.V78R31, new List<TreasureStateInfo>
		{
			new TreasureStateInfo(1, 15),
			new TreasureStateInfo(2, 15),
			new TreasureStateInfo(3, 15),
			new TreasureStateInfo(4, 15),
			new TreasureStateInfo(5, 15),
			new TreasureStateInfo(6, 15),
			new TreasureStateInfo(7, 15),
			new TreasureStateInfo(8, 15),
			new TreasureStateInfo(9, 15),
			new TreasureStateInfo(10, 15),
			new TreasureStateInfo(11, 15),
			new TreasureStateInfo(12, 15),
			new TreasureStateInfo(13, 15),
			new TreasureStateInfo(14, 15),
			new TreasureStateInfo(15, 15)
		}, EMiscFilterType.KeyItem, -1, -1, -1, 200));
		_dataArray.Add(new MiscItem(340, 661, 12, 1205, 1, -1, "icon_Misc_wotu", 662, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, consumable: false, 0, 600, 200, 2000, 1, 2, 600, 4, allowRandomCreate: true, 0, isSpecial: true, 1, -1, 36, -1, 0, 0, 0, -1, canUseOnPrepareCombat: false, allowUseInPlayAndTest: false, -1, 60, new List<short>(), new List<int> { 1, 2, 3 }, EMiscGenerateType.V78R31, new List<TreasureStateInfo>
		{
			new TreasureStateInfo(1, 15),
			new TreasureStateInfo(2, 15),
			new TreasureStateInfo(3, 15),
			new TreasureStateInfo(4, 15),
			new TreasureStateInfo(5, 15),
			new TreasureStateInfo(6, 15),
			new TreasureStateInfo(7, 15),
			new TreasureStateInfo(8, 15),
			new TreasureStateInfo(9, 15),
			new TreasureStateInfo(10, 15),
			new TreasureStateInfo(11, 15),
			new TreasureStateInfo(12, 15),
			new TreasureStateInfo(13, 15),
			new TreasureStateInfo(14, 15),
			new TreasureStateInfo(15, 15)
		}, EMiscFilterType.KeyItem, -1, -1, -1, 200));
		_dataArray.Add(new MiscItem(341, 663, 12, 1205, 1, -1, "icon_Misc_youshou", 664, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, consumable: false, 0, 600, 200, 2000, 1, 2, 600, 4, allowRandomCreate: true, 0, isSpecial: true, 0, -1, 36, -1, 0, 0, 0, -1, canUseOnPrepareCombat: false, allowUseInPlayAndTest: false, -1, 60, new List<short>(), new List<int> { 1, 2, 3 }, EMiscGenerateType.V78R31, new List<TreasureStateInfo>
		{
			new TreasureStateInfo(1, 15),
			new TreasureStateInfo(2, 15),
			new TreasureStateInfo(3, 15),
			new TreasureStateInfo(4, 15),
			new TreasureStateInfo(5, 15),
			new TreasureStateInfo(6, 15),
			new TreasureStateInfo(7, 15),
			new TreasureStateInfo(8, 15),
			new TreasureStateInfo(9, 15),
			new TreasureStateInfo(10, 15),
			new TreasureStateInfo(11, 15),
			new TreasureStateInfo(12, 15),
			new TreasureStateInfo(13, 15),
			new TreasureStateInfo(14, 15),
			new TreasureStateInfo(15, 15)
		}, EMiscFilterType.KeyItem, -1, -1, -1, 200));
		_dataArray.Add(new MiscItem(342, 657, 12, 1205, 1, -1, "icon_Misc_baihuazhongzi", 658, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, consumable: false, 0, 100, 200, 2000, 1, 2, 600, 4, allowRandomCreate: true, 0, isSpecial: true, 4, -1, 36, -1, 0, 0, 0, -1, canUseOnPrepareCombat: false, allowUseInPlayAndTest: false, -1, 60, new List<short>(), new List<int> { 1, 2, 3 }, EMiscGenerateType.V78R31, new List<TreasureStateInfo>
		{
			new TreasureStateInfo(1, 15),
			new TreasureStateInfo(2, 15),
			new TreasureStateInfo(3, 15),
			new TreasureStateInfo(4, 15),
			new TreasureStateInfo(5, 15),
			new TreasureStateInfo(6, 15),
			new TreasureStateInfo(7, 15),
			new TreasureStateInfo(8, 15),
			new TreasureStateInfo(9, 15),
			new TreasureStateInfo(10, 15),
			new TreasureStateInfo(11, 15),
			new TreasureStateInfo(12, 15),
			new TreasureStateInfo(13, 15),
			new TreasureStateInfo(14, 15),
			new TreasureStateInfo(15, 15)
		}, EMiscFilterType.KeyItem, -1, -1, -1, 200));
		_dataArray.Add(new MiscItem(343, 665, 12, 1200, 3, -1, "icon_Misc_yaodao", 666, transferable: false, stackable: true, wagerable: false, refinable: false, poisonable: false, repairable: false, inheritable: false, consumable: false, 0, 0, 0, 0, 0, 0, 0, 0, allowRandomCreate: true, 0, isSpecial: true, -1, -1, -1, -1, 0, 0, 0, -1, canUseOnPrepareCombat: false, allowUseInPlayAndTest: false, -1, 60, new List<short>(), new List<int>(), EMiscGenerateType.Invalid, new List<TreasureStateInfo>(), EMiscFilterType.Collection, -1, -1, -1, 0));
		_dataArray.Add(new MiscItem(344, 667, 12, 1200, 4, -1, "icon_Misc_bimo", 668, transferable: false, stackable: true, wagerable: false, refinable: false, poisonable: false, repairable: false, inheritable: false, consumable: false, 0, 0, 0, 0, 0, 0, 0, 0, allowRandomCreate: true, 0, isSpecial: true, -1, -1, -1, -1, 0, 0, 0, -1, canUseOnPrepareCombat: false, allowUseInPlayAndTest: false, -1, 60, new List<short>(), new List<int>(), EMiscGenerateType.Invalid, new List<TreasureStateInfo>(), EMiscFilterType.Collection, -1, -1, -1, 0));
		_dataArray.Add(new MiscItem(345, 669, 12, 1200, 5, -1, "icon_Misc_sanqingling", 670, transferable: false, stackable: true, wagerable: false, refinable: false, poisonable: false, repairable: false, inheritable: false, consumable: false, 0, 0, 0, 0, 0, 0, 0, 0, allowRandomCreate: true, 0, isSpecial: true, -1, -1, -1, -1, 0, 0, 0, -1, canUseOnPrepareCombat: false, allowUseInPlayAndTest: false, -1, 60, new List<short>(), new List<int>(), EMiscGenerateType.Invalid, new List<TreasureStateInfo>(), EMiscFilterType.Collection, -1, -1, -1, 0));
		_dataArray.Add(new MiscItem(346, 671, 12, 1200, 6, -1, "icon_Misc_fozhu", 672, transferable: false, stackable: true, wagerable: false, refinable: false, poisonable: false, repairable: false, inheritable: false, consumable: false, 0, 0, 0, 0, 0, 0, 0, 0, allowRandomCreate: true, 0, isSpecial: true, -1, -1, -1, -1, 0, 0, 0, -1, canUseOnPrepareCombat: false, allowUseInPlayAndTest: false, -1, 60, new List<short>(), new List<int>(), EMiscGenerateType.Invalid, new List<TreasureStateInfo>(), EMiscFilterType.Collection, -1, -1, -1, 0));
		_dataArray.Add(new MiscItem(347, 673, 12, 1200, 7, -1, "icon_Misc_jinzun", 674, transferable: false, stackable: true, wagerable: false, refinable: false, poisonable: false, repairable: false, inheritable: false, consumable: false, 0, 0, 0, 0, 0, 0, 0, 0, allowRandomCreate: true, 0, isSpecial: true, -1, -1, -1, -1, 0, 0, 0, -1, canUseOnPrepareCombat: false, allowUseInPlayAndTest: false, -1, 60, new List<short>(), new List<int>(), EMiscGenerateType.Invalid, new List<TreasureStateInfo>(), EMiscFilterType.Collection, -1, -1, -1, 0));
		_dataArray.Add(new MiscItem(348, 675, 12, 1200, 8, -1, "icon_Misc_yupei", 676, transferable: false, stackable: true, wagerable: false, refinable: false, poisonable: false, repairable: false, inheritable: false, consumable: false, 0, 0, 0, 0, 0, 0, 0, 0, allowRandomCreate: true, 0, isSpecial: true, -1, -1, -1, -1, 0, 0, 0, -1, canUseOnPrepareCombat: false, allowUseInPlayAndTest: false, -1, 60, new List<short>(), new List<int>(), EMiscGenerateType.Invalid, new List<TreasureStateInfo>(), EMiscFilterType.Collection, -1, -1, -1, 0));
		_dataArray.Add(new MiscItem(349, 677, 12, 1200, 3, -1, "icon_Misc_tiebo", 678, transferable: false, stackable: true, wagerable: false, refinable: false, poisonable: false, repairable: false, inheritable: false, consumable: false, 0, 0, 0, 0, 0, 0, 0, 0, allowRandomCreate: true, 0, isSpecial: true, -1, -1, -1, -1, 0, 0, 0, -1, canUseOnPrepareCombat: false, allowUseInPlayAndTest: false, -1, 60, new List<short>(), new List<int>(), EMiscGenerateType.Invalid, new List<TreasureStateInfo>(), EMiscFilterType.Collection, -1, -1, -1, 0));
		_dataArray.Add(new MiscItem(350, 679, 12, 1200, 4, -1, "icon_Misc_jiuzhen", 680, transferable: false, stackable: true, wagerable: false, refinable: false, poisonable: false, repairable: false, inheritable: false, consumable: false, 0, 0, 0, 0, 0, 0, 0, 0, allowRandomCreate: true, 0, isSpecial: true, -1, -1, -1, -1, 0, 0, 0, -1, canUseOnPrepareCombat: false, allowUseInPlayAndTest: false, -1, 60, new List<short>(), new List<int>(), EMiscGenerateType.Invalid, new List<TreasureStateInfo>(), EMiscFilterType.Collection, -1, -1, -1, 0));
		_dataArray.Add(new MiscItem(351, 681, 12, 1200, 5, -1, "icon_Misc_qiantong", 682, transferable: false, stackable: true, wagerable: false, refinable: false, poisonable: false, repairable: false, inheritable: false, consumable: false, 0, 0, 0, 0, 0, 0, 0, 0, allowRandomCreate: true, 0, isSpecial: true, -1, -1, -1, -1, 0, 0, 0, -1, canUseOnPrepareCombat: false, allowUseInPlayAndTest: false, -1, 60, new List<short>(), new List<int>(), EMiscGenerateType.Invalid, new List<TreasureStateInfo>(), EMiscFilterType.Collection, -1, -1, -1, 0));
		_dataArray.Add(new MiscItem(352, 683, 12, 1200, 6, -1, "icon_Misc_suanpan", 684, transferable: false, stackable: true, wagerable: false, refinable: false, poisonable: false, repairable: false, inheritable: false, consumable: false, 0, 0, 0, 0, 0, 0, 0, 0, allowRandomCreate: true, 0, isSpecial: true, -1, -1, -1, -1, 0, 0, 0, -1, canUseOnPrepareCombat: false, allowUseInPlayAndTest: false, -1, 60, new List<short>(), new List<int>(), EMiscGenerateType.Invalid, new List<TreasureStateInfo>(), EMiscFilterType.Collection, -1, -1, -1, 0));
		_dataArray.Add(new MiscItem(353, 685, 12, 1200, 7, -1, "icon_Misc_chahu", 686, transferable: false, stackable: true, wagerable: false, refinable: false, poisonable: false, repairable: false, inheritable: false, consumable: false, 0, 0, 0, 0, 0, 0, 0, 0, allowRandomCreate: true, 0, isSpecial: true, -1, -1, -1, -1, 0, 0, 0, -1, canUseOnPrepareCombat: false, allowUseInPlayAndTest: false, -1, 60, new List<short>(), new List<int>(), EMiscGenerateType.Invalid, new List<TreasureStateInfo>(), EMiscFilterType.Collection, -1, -1, -1, 0));
		_dataArray.Add(new MiscItem(354, 687, 12, 1200, 8, -1, "icon_Misc_yinshou", 688, transferable: false, stackable: true, wagerable: false, refinable: false, poisonable: false, repairable: false, inheritable: false, consumable: false, 0, 0, 0, 0, 0, 0, 0, 0, allowRandomCreate: true, 0, isSpecial: true, -1, -1, -1, -1, 0, 0, 0, -1, canUseOnPrepareCombat: false, allowUseInPlayAndTest: false, -1, 60, new List<short>(), new List<int>(), EMiscGenerateType.Invalid, new List<TreasureStateInfo>(), EMiscFilterType.Collection, -1, -1, -1, 0));
		_dataArray.Add(new MiscItem(355, 689, 12, 1200, 0, -1, "icon_Misc_tou", 690, transferable: false, stackable: false, wagerable: false, refinable: false, poisonable: false, repairable: false, inheritable: false, consumable: false, 0, 0, 0, 0, 0, 0, 0, 0, allowRandomCreate: true, 0, isSpecial: true, -1, -1, -1, -1, 0, 0, 0, -1, canUseOnPrepareCombat: false, allowUseInPlayAndTest: false, -1, 60, new List<short>(), new List<int>(), EMiscGenerateType.Invalid, new List<TreasureStateInfo>(), EMiscFilterType.Collection, -1, -1, -1, 0));
		_dataArray.Add(new MiscItem(356, 691, 12, 1200, 0, -1, "icon_Misc_zuobi", 692, transferable: false, stackable: false, wagerable: false, refinable: false, poisonable: false, repairable: false, inheritable: false, consumable: false, 0, 0, 0, 0, 0, 0, 0, 0, allowRandomCreate: true, 0, isSpecial: true, -1, -1, -1, -1, 0, 0, 0, -1, canUseOnPrepareCombat: false, allowUseInPlayAndTest: false, -1, 60, new List<short>(), new List<int>(), EMiscGenerateType.Invalid, new List<TreasureStateInfo>(), EMiscFilterType.Collection, -1, -1, -1, 0));
		_dataArray.Add(new MiscItem(357, 693, 12, 1200, 0, -1, "icon_Misc_youbi", 694, transferable: false, stackable: false, wagerable: false, refinable: false, poisonable: false, repairable: false, inheritable: false, consumable: false, 0, 0, 0, 0, 0, 0, 0, 0, allowRandomCreate: true, 0, isSpecial: true, -1, -1, -1, -1, 0, 0, 0, -1, canUseOnPrepareCombat: false, allowUseInPlayAndTest: false, -1, 60, new List<short>(), new List<int>(), EMiscGenerateType.Invalid, new List<TreasureStateInfo>(), EMiscFilterType.Collection, -1, -1, -1, 0));
		_dataArray.Add(new MiscItem(358, 695, 12, 1200, 0, -1, "icon_Misc_zuotui", 696, transferable: false, stackable: false, wagerable: false, refinable: false, poisonable: false, repairable: false, inheritable: false, consumable: false, 0, 0, 0, 0, 0, 0, 0, 0, allowRandomCreate: true, 0, isSpecial: true, -1, -1, -1, -1, 0, 0, 0, -1, canUseOnPrepareCombat: false, allowUseInPlayAndTest: false, -1, 60, new List<short>(), new List<int>(), EMiscGenerateType.Invalid, new List<TreasureStateInfo>(), EMiscFilterType.Collection, -1, -1, -1, 0));
		_dataArray.Add(new MiscItem(359, 697, 12, 1200, 0, -1, "icon_Misc_youtui", 698, transferable: false, stackable: false, wagerable: false, refinable: false, poisonable: false, repairable: false, inheritable: false, consumable: false, 0, 0, 0, 0, 0, 0, 0, 0, allowRandomCreate: true, 0, isSpecial: true, -1, -1, -1, -1, 0, 0, 0, -1, canUseOnPrepareCombat: false, allowUseInPlayAndTest: false, -1, 60, new List<short>(), new List<int>(), EMiscGenerateType.Invalid, new List<TreasureStateInfo>(), EMiscFilterType.Collection, -1, -1, -1, 0));
	}

	private void CreateItems6()
	{
		_dataArray.Add(new MiscItem(360, 699, 12, 1200, 0, -1, "icon_Misc_qugan", 700, transferable: false, stackable: false, wagerable: false, refinable: false, poisonable: false, repairable: false, inheritable: false, consumable: false, 0, 0, 0, 0, 0, 0, 0, 0, allowRandomCreate: true, 0, isSpecial: true, -1, -1, -1, -1, 0, 0, 0, -1, canUseOnPrepareCombat: false, allowUseInPlayAndTest: false, -1, 60, new List<short>(), new List<int>(), EMiscGenerateType.Invalid, new List<TreasureStateInfo>(), EMiscFilterType.Collection, -1, -1, -1, 0));
		_dataArray.Add(new MiscItem(361, 701, 12, 1200, 0, -1, "icon_Misc_weichengbingren", 702, transferable: false, stackable: false, wagerable: false, refinable: false, poisonable: false, repairable: false, inheritable: false, consumable: false, 0, 0, 0, 0, 0, 0, 0, 0, allowRandomCreate: true, 0, isSpecial: true, -1, -1, -1, -1, 0, 0, 0, -1, canUseOnPrepareCombat: false, allowUseInPlayAndTest: false, -1, 60, new List<short>(), new List<int>(), EMiscGenerateType.Invalid, new List<TreasureStateInfo>(), EMiscFilterType.Collection, -1, -1, -1, 0));
		_dataArray.Add(new MiscItem(362, 703, 12, 1200, 0, -1, "icon_Misc_weichengbingren", 702, transferable: false, stackable: false, wagerable: false, refinable: false, poisonable: false, repairable: false, inheritable: false, consumable: false, 0, 0, 0, 0, 0, 0, 0, 0, allowRandomCreate: true, 0, isSpecial: true, -1, -1, -1, -1, 0, 0, 0, -1, canUseOnPrepareCombat: false, allowUseInPlayAndTest: false, -1, 60, new List<short>(), new List<int>(), EMiscGenerateType.Invalid, new List<TreasureStateInfo>(), EMiscFilterType.Collection, -1, -1, -1, 0));
		_dataArray.Add(new MiscItem(363, 704, 12, 1200, 0, -1, "icon_Misc_weichengbingren", 702, transferable: false, stackable: false, wagerable: false, refinable: false, poisonable: false, repairable: false, inheritable: false, consumable: false, 0, 0, 0, 0, 0, 0, 0, 0, allowRandomCreate: true, 0, isSpecial: true, -1, -1, -1, -1, 0, 0, 0, -1, canUseOnPrepareCombat: false, allowUseInPlayAndTest: false, -1, 60, new List<short>(), new List<int>(), EMiscGenerateType.Invalid, new List<TreasureStateInfo>(), EMiscFilterType.Collection, -1, -1, -1, 0));
		_dataArray.Add(new MiscItem(364, 705, 12, 1205, 6, -1, "icon_Misc_jinwuhui", 706, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, consumable: false, 0, 50, 9200, 92000, 5, 7, 3600, 8, allowRandomCreate: true, 0, isSpecial: true, 2, -1, 36, -1, 0, 0, 0, -1, canUseOnPrepareCombat: false, allowUseInPlayAndTest: false, -1, 60, new List<short>(), new List<int> { 4, 5, 6 }, EMiscGenerateType.V78R31, new List<TreasureStateInfo>
		{
			new TreasureStateInfo(1, 6),
			new TreasureStateInfo(2, 6),
			new TreasureStateInfo(3, 6),
			new TreasureStateInfo(4, 6),
			new TreasureStateInfo(5, 6),
			new TreasureStateInfo(6, 6),
			new TreasureStateInfo(7, 6),
			new TreasureStateInfo(8, 6),
			new TreasureStateInfo(9, 6),
			new TreasureStateInfo(10, 6),
			new TreasureStateInfo(11, 6),
			new TreasureStateInfo(12, 6),
			new TreasureStateInfo(13, 6),
			new TreasureStateInfo(14, 6),
			new TreasureStateInfo(15, 6)
		}, EMiscFilterType.KeyItem, -1, -1, -1, 1500));
		_dataArray.Add(new MiscItem(365, 707, 12, 1205, 6, -1, "icon_Misc_yingxingshi", 708, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, consumable: false, 0, 600, 9200, 92000, 5, 7, 3600, 8, allowRandomCreate: true, 0, isSpecial: true, 2, -1, 36, -1, 0, 0, 0, -1, canUseOnPrepareCombat: false, allowUseInPlayAndTest: false, -1, 60, new List<short>(), new List<int> { 4, 5, 6 }, EMiscGenerateType.V78R31, new List<TreasureStateInfo>
		{
			new TreasureStateInfo(1, 6),
			new TreasureStateInfo(2, 6),
			new TreasureStateInfo(3, 6),
			new TreasureStateInfo(4, 6),
			new TreasureStateInfo(5, 6),
			new TreasureStateInfo(6, 6),
			new TreasureStateInfo(7, 6),
			new TreasureStateInfo(8, 6),
			new TreasureStateInfo(9, 6),
			new TreasureStateInfo(10, 6),
			new TreasureStateInfo(11, 6),
			new TreasureStateInfo(12, 6),
			new TreasureStateInfo(13, 6),
			new TreasureStateInfo(14, 6),
			new TreasureStateInfo(15, 6)
		}, EMiscFilterType.KeyItem, -1, -1, -1, 1500));
		_dataArray.Add(new MiscItem(366, 709, 12, 1205, 6, -1, "icon_Misc_miyuqizhang", 710, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, consumable: false, 0, 100, 9200, 92000, 5, 7, 3600, 8, allowRandomCreate: true, 0, isSpecial: true, 1, -1, 36, -1, 0, 0, 0, -1, canUseOnPrepareCombat: false, allowUseInPlayAndTest: false, -1, 60, new List<short>(), new List<int> { 4, 5, 6 }, EMiscGenerateType.V78R31, new List<TreasureStateInfo>
		{
			new TreasureStateInfo(1, 6),
			new TreasureStateInfo(2, 6),
			new TreasureStateInfo(3, 6),
			new TreasureStateInfo(4, 6),
			new TreasureStateInfo(5, 6),
			new TreasureStateInfo(6, 6),
			new TreasureStateInfo(7, 6),
			new TreasureStateInfo(8, 6),
			new TreasureStateInfo(9, 6),
			new TreasureStateInfo(10, 6),
			new TreasureStateInfo(11, 6),
			new TreasureStateInfo(12, 6),
			new TreasureStateInfo(13, 6),
			new TreasureStateInfo(14, 6),
			new TreasureStateInfo(15, 6)
		}, EMiscFilterType.KeyItem, -1, -1, -1, 1500));
		_dataArray.Add(new MiscItem(367, 711, 12, 1205, 6, -1, "icon_Misc_taishilongzhi", 712, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, consumable: false, 0, 100, 9200, 92000, 5, 7, 3600, 8, allowRandomCreate: true, 0, isSpecial: true, 1, -1, 36, -1, 0, 0, 0, -1, canUseOnPrepareCombat: false, allowUseInPlayAndTest: false, -1, 60, new List<short>(), new List<int> { 4, 5, 6 }, EMiscGenerateType.V78R31, new List<TreasureStateInfo>
		{
			new TreasureStateInfo(1, 6),
			new TreasureStateInfo(2, 6),
			new TreasureStateInfo(3, 6),
			new TreasureStateInfo(4, 6),
			new TreasureStateInfo(5, 6),
			new TreasureStateInfo(6, 6),
			new TreasureStateInfo(7, 6),
			new TreasureStateInfo(8, 6),
			new TreasureStateInfo(9, 6),
			new TreasureStateInfo(10, 6),
			new TreasureStateInfo(11, 6),
			new TreasureStateInfo(12, 6),
			new TreasureStateInfo(13, 6),
			new TreasureStateInfo(14, 6),
			new TreasureStateInfo(15, 6)
		}, EMiscFilterType.KeyItem, -1, -1, -1, 1500));
		_dataArray.Add(new MiscItem(368, 713, 12, 1205, 6, -1, "icon_Misc_changchunjiuqiaoshi", 714, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, consumable: false, 0, 600, 9200, 92000, 5, 7, 3600, 8, allowRandomCreate: true, 0, isSpecial: true, 5, -1, 36, -1, 0, 0, 0, -1, canUseOnPrepareCombat: false, allowUseInPlayAndTest: false, -1, 60, new List<short>(), new List<int> { 4, 5, 6 }, EMiscGenerateType.V78R31, new List<TreasureStateInfo>
		{
			new TreasureStateInfo(1, 6),
			new TreasureStateInfo(2, 6),
			new TreasureStateInfo(3, 6),
			new TreasureStateInfo(4, 6),
			new TreasureStateInfo(5, 6),
			new TreasureStateInfo(6, 6),
			new TreasureStateInfo(7, 6),
			new TreasureStateInfo(8, 6),
			new TreasureStateInfo(9, 6),
			new TreasureStateInfo(10, 6),
			new TreasureStateInfo(11, 6),
			new TreasureStateInfo(12, 6),
			new TreasureStateInfo(13, 6),
			new TreasureStateInfo(14, 6),
			new TreasureStateInfo(15, 6)
		}, EMiscFilterType.KeyItem, -1, -1, -1, 1500));
		_dataArray.Add(new MiscItem(369, 715, 12, 1205, 6, -1, "icon_Misc_xuansheshi", 716, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, consumable: false, 0, 500, 9200, 92000, 5, 7, 3600, 8, allowRandomCreate: true, 0, isSpecial: true, 5, -1, 36, -1, 0, 0, 0, -1, canUseOnPrepareCombat: false, allowUseInPlayAndTest: false, -1, 60, new List<short>(), new List<int> { 4, 5, 6 }, EMiscGenerateType.V78R31, new List<TreasureStateInfo>
		{
			new TreasureStateInfo(1, 6),
			new TreasureStateInfo(2, 6),
			new TreasureStateInfo(3, 6),
			new TreasureStateInfo(4, 6),
			new TreasureStateInfo(5, 6),
			new TreasureStateInfo(6, 6),
			new TreasureStateInfo(7, 6),
			new TreasureStateInfo(8, 6),
			new TreasureStateInfo(9, 6),
			new TreasureStateInfo(10, 6),
			new TreasureStateInfo(11, 6),
			new TreasureStateInfo(12, 6),
			new TreasureStateInfo(13, 6),
			new TreasureStateInfo(14, 6),
			new TreasureStateInfo(15, 6)
		}, EMiscFilterType.KeyItem, -1, -1, -1, 1500));
		_dataArray.Add(new MiscItem(370, 717, 12, 1205, 6, -1, "icon_Misc_qixiangfengwang", 718, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, consumable: false, 0, 100, 9200, 92000, 5, 7, 3600, 8, allowRandomCreate: true, 0, isSpecial: true, 4, -1, 36, -1, 0, 0, 0, -1, canUseOnPrepareCombat: false, allowUseInPlayAndTest: false, -1, 60, new List<short>(), new List<int> { 4, 5, 6 }, EMiscGenerateType.V78R31, new List<TreasureStateInfo>
		{
			new TreasureStateInfo(1, 6),
			new TreasureStateInfo(2, 6),
			new TreasureStateInfo(3, 6),
			new TreasureStateInfo(4, 6),
			new TreasureStateInfo(5, 6),
			new TreasureStateInfo(6, 6),
			new TreasureStateInfo(7, 6),
			new TreasureStateInfo(8, 6),
			new TreasureStateInfo(9, 6),
			new TreasureStateInfo(10, 6),
			new TreasureStateInfo(11, 6),
			new TreasureStateInfo(12, 6),
			new TreasureStateInfo(13, 6),
			new TreasureStateInfo(14, 6),
			new TreasureStateInfo(15, 6)
		}, EMiscFilterType.KeyItem, -1, -1, -1, 1500));
		_dataArray.Add(new MiscItem(371, 719, 12, 1205, 6, -1, "icon_Misc_shengshaeryuanqi", 720, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, consumable: false, 0, 100, 9200, 92000, 5, 7, 3600, 8, allowRandomCreate: true, 0, isSpecial: true, 4, -1, 36, -1, 0, 0, 0, -1, canUseOnPrepareCombat: false, allowUseInPlayAndTest: false, -1, 60, new List<short>(), new List<int> { 4, 5, 6 }, EMiscGenerateType.V78R31, new List<TreasureStateInfo>
		{
			new TreasureStateInfo(1, 6),
			new TreasureStateInfo(2, 6),
			new TreasureStateInfo(3, 6),
			new TreasureStateInfo(4, 6),
			new TreasureStateInfo(5, 6),
			new TreasureStateInfo(6, 6),
			new TreasureStateInfo(7, 6),
			new TreasureStateInfo(8, 6),
			new TreasureStateInfo(9, 6),
			new TreasureStateInfo(10, 6),
			new TreasureStateInfo(11, 6),
			new TreasureStateInfo(12, 6),
			new TreasureStateInfo(13, 6),
			new TreasureStateInfo(14, 6),
			new TreasureStateInfo(15, 6)
		}, EMiscFilterType.KeyItem, -1, -1, -1, 1500));
		_dataArray.Add(new MiscItem(372, 721, 12, 1205, 6, -1, "icon_Misc_dilingxue", 722, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, consumable: false, 0, 500, 9200, 92000, 5, 7, 3600, 8, allowRandomCreate: true, 0, isSpecial: true, 3, -1, 36, -1, 0, 0, 0, -1, canUseOnPrepareCombat: false, allowUseInPlayAndTest: false, -1, 60, new List<short>(), new List<int> { 4, 5, 6 }, EMiscGenerateType.V78R31, new List<TreasureStateInfo>
		{
			new TreasureStateInfo(1, 6),
			new TreasureStateInfo(2, 6),
			new TreasureStateInfo(3, 6),
			new TreasureStateInfo(4, 6),
			new TreasureStateInfo(5, 6),
			new TreasureStateInfo(6, 6),
			new TreasureStateInfo(7, 6),
			new TreasureStateInfo(8, 6),
			new TreasureStateInfo(9, 6),
			new TreasureStateInfo(10, 6),
			new TreasureStateInfo(11, 6),
			new TreasureStateInfo(12, 6),
			new TreasureStateInfo(13, 6),
			new TreasureStateInfo(14, 6),
			new TreasureStateInfo(15, 6)
		}, EMiscFilterType.KeyItem, -1, -1, -1, 1500));
		_dataArray.Add(new MiscItem(373, 723, 12, 1205, 6, -1, "icon_Misc_xudiyinsha", 724, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, consumable: false, 0, 100, 9200, 92000, 5, 7, 3600, 8, allowRandomCreate: true, 0, isSpecial: true, 3, -1, 36, -1, 0, 0, 0, -1, canUseOnPrepareCombat: false, allowUseInPlayAndTest: false, -1, 60, new List<short>(), new List<int> { 4, 5, 6 }, EMiscGenerateType.V78R31, new List<TreasureStateInfo>
		{
			new TreasureStateInfo(1, 6),
			new TreasureStateInfo(2, 6),
			new TreasureStateInfo(3, 6),
			new TreasureStateInfo(4, 6),
			new TreasureStateInfo(5, 6),
			new TreasureStateInfo(6, 6),
			new TreasureStateInfo(7, 6),
			new TreasureStateInfo(8, 6),
			new TreasureStateInfo(9, 6),
			new TreasureStateInfo(10, 6),
			new TreasureStateInfo(11, 6),
			new TreasureStateInfo(12, 6),
			new TreasureStateInfo(13, 6),
			new TreasureStateInfo(14, 6),
			new TreasureStateInfo(15, 6)
		}, EMiscFilterType.KeyItem, -1, -1, -1, 1500));
		_dataArray.Add(new MiscItem(374, 725, 12, 1200, 8, -1, "icon_Misc_huanianzhu", 726, transferable: false, stackable: false, wagerable: false, refinable: false, poisonable: false, repairable: false, inheritable: false, consumable: false, 0, 0, 0, 0, 0, 0, 0, 0, allowRandomCreate: true, 0, isSpecial: true, -1, -1, -1, -1, 0, 0, 0, -1, canUseOnPrepareCombat: false, allowUseInPlayAndTest: false, -1, 60, new List<short>(), new List<int>(), EMiscGenerateType.Invalid, new List<TreasureStateInfo>(), EMiscFilterType.Item, -1, -1, -1, 0));
		_dataArray.Add(new MiscItem(375, 727, 12, 1200, 8, -1, "icon_Misc_lingwen", 728, transferable: false, stackable: true, wagerable: false, refinable: false, poisonable: false, repairable: false, inheritable: false, consumable: true, 0, 0, 5, 10, 0, 0, 0, 0, allowRandomCreate: true, 0, isSpecial: true, -1, -1, -1, -1, 0, 0, 0, 0, canUseOnPrepareCombat: false, allowUseInPlayAndTest: true, -1, 60, new List<short>(), new List<int>(), EMiscGenerateType.Invalid, new List<TreasureStateInfo>(), EMiscFilterType.Invalid, 12, 11, -1, 0));
		_dataArray.Add(new MiscItem(376, 729, 12, 1200, 8, -1, "icon_Misc_lingwen", 730, transferable: false, stackable: true, wagerable: false, refinable: false, poisonable: false, repairable: false, inheritable: false, consumable: true, 0, 0, 5, 10, 0, 0, 0, 0, allowRandomCreate: true, 0, isSpecial: true, -1, -1, -1, -1, 0, 0, 0, 0, canUseOnPrepareCombat: false, allowUseInPlayAndTest: true, -1, 0, new List<short>(), new List<int>(), EMiscGenerateType.Invalid, new List<TreasureStateInfo>(), EMiscFilterType.Invalid, 12, 11, -1, 0));
		_dataArray.Add(new MiscItem(377, 731, 12, 1200, 8, -1, "icon_Misc_lingwen", 732, transferable: false, stackable: true, wagerable: false, refinable: false, poisonable: false, repairable: false, inheritable: false, consumable: true, 0, 0, 5, 10, 0, 0, 0, 0, allowRandomCreate: true, 0, isSpecial: true, -1, -1, -1, -1, 0, 0, 0, 0, canUseOnPrepareCombat: false, allowUseInPlayAndTest: true, -1, 0, new List<short>(), new List<int>(), EMiscGenerateType.Invalid, new List<TreasureStateInfo>(), EMiscFilterType.Invalid, 12, 11, -1, 0));
		_dataArray.Add(new MiscItem(378, 733, 12, 1200, 8, -1, "icon_Misc_lingwen", 734, transferable: false, stackable: true, wagerable: false, refinable: false, poisonable: false, repairable: false, inheritable: false, consumable: true, 0, 0, 5, 10, 0, 0, 0, 0, allowRandomCreate: true, 0, isSpecial: true, -1, -1, -1, -1, 0, 0, 0, 0, canUseOnPrepareCombat: false, allowUseInPlayAndTest: true, -1, 60, new List<short>(), new List<int>(), EMiscGenerateType.Invalid, new List<TreasureStateInfo>(), EMiscFilterType.Invalid, 12, 11, -1, 0));
		_dataArray.Add(new MiscItem(379, 735, 12, 1200, 8, -1, "icon_Misc_lingwen", 736, transferable: false, stackable: true, wagerable: false, refinable: false, poisonable: false, repairable: false, inheritable: false, consumable: true, 0, 0, 5, 10, 0, 0, 0, 0, allowRandomCreate: true, 0, isSpecial: true, -1, -1, -1, -1, 0, 0, 0, 0, canUseOnPrepareCombat: false, allowUseInPlayAndTest: true, -1, 60, new List<short>(), new List<int>(), EMiscGenerateType.Invalid, new List<TreasureStateInfo>(), EMiscFilterType.Invalid, 12, 11, -1, 0));
		_dataArray.Add(new MiscItem(380, 737, 12, 1200, 8, -1, "icon_Misc_yaowen", 738, transferable: false, stackable: true, wagerable: false, refinable: false, poisonable: false, repairable: false, inheritable: false, consumable: true, 0, 0, 5, 10, 0, 0, 0, 0, allowRandomCreate: true, 0, isSpecial: true, -1, -1, -1, -1, 0, 0, 0, 0, canUseOnPrepareCombat: false, allowUseInPlayAndTest: true, -1, 60, new List<short>(), new List<int>(), EMiscGenerateType.Invalid, new List<TreasureStateInfo>(), EMiscFilterType.Invalid, 13, 11, -1, 0));
		_dataArray.Add(new MiscItem(381, 739, 12, 1200, 8, -1, "icon_Misc_yaowen", 740, transferable: false, stackable: true, wagerable: false, refinable: false, poisonable: false, repairable: false, inheritable: false, consumable: true, 0, 0, 5, 10, 0, 0, 0, 0, allowRandomCreate: true, 0, isSpecial: true, -1, -1, -1, -1, 0, 0, 0, 0, canUseOnPrepareCombat: false, allowUseInPlayAndTest: true, -1, 0, new List<short>(), new List<int>(), EMiscGenerateType.Invalid, new List<TreasureStateInfo>(), EMiscFilterType.Invalid, 13, 11, -1, 0));
		_dataArray.Add(new MiscItem(382, 741, 12, 1200, 8, -1, "icon_Misc_yaowen", 742, transferable: false, stackable: true, wagerable: false, refinable: false, poisonable: false, repairable: false, inheritable: false, consumable: true, 0, 0, 5, 10, 0, 0, 0, 0, allowRandomCreate: true, 0, isSpecial: true, -1, -1, -1, -1, 0, 0, 0, 0, canUseOnPrepareCombat: false, allowUseInPlayAndTest: true, -1, 0, new List<short>(), new List<int>(), EMiscGenerateType.Invalid, new List<TreasureStateInfo>(), EMiscFilterType.Invalid, 13, 11, -1, 0));
		_dataArray.Add(new MiscItem(383, 743, 12, 1200, 8, -1, "icon_Misc_yaowen", 744, transferable: false, stackable: true, wagerable: false, refinable: false, poisonable: false, repairable: false, inheritable: false, consumable: true, 0, 0, 5, 10, 0, 0, 0, 0, allowRandomCreate: true, 0, isSpecial: true, -1, -1, -1, -1, 0, 0, 0, 0, canUseOnPrepareCombat: false, allowUseInPlayAndTest: true, -1, 60, new List<short>(), new List<int>(), EMiscGenerateType.Invalid, new List<TreasureStateInfo>(), EMiscFilterType.Invalid, 13, 11, -1, 0));
		_dataArray.Add(new MiscItem(384, 745, 12, 1200, 8, -1, "icon_Misc_yaowen", 746, transferable: false, stackable: true, wagerable: false, refinable: false, poisonable: false, repairable: false, inheritable: false, consumable: true, 0, 0, 5, 10, 0, 0, 0, 0, allowRandomCreate: true, 0, isSpecial: true, -1, -1, -1, -1, 0, 0, 0, 0, canUseOnPrepareCombat: false, allowUseInPlayAndTest: true, -1, 60, new List<short>(), new List<int>(), EMiscGenerateType.Invalid, new List<TreasureStateInfo>(), EMiscFilterType.Invalid, 13, 11, -1, 0));
		_dataArray.Add(new MiscItem(385, 747, 12, 1200, 0, 385, "icon_Misc_ganxiexin", 748, transferable: false, stackable: true, wagerable: false, refinable: false, poisonable: false, repairable: false, inheritable: true, consumable: true, 0, 0, 5, 10, 0, 0, 50, 8, allowRandomCreate: true, 0, isSpecial: false, -1, -1, 1, -1, 0, 0, 0, -1, canUseOnPrepareCombat: false, allowUseInPlayAndTest: false, -1, 60, new List<short>(), new List<int>(), EMiscGenerateType.Invalid, new List<TreasureStateInfo>(), EMiscFilterType.Item, -1, -1, 300, 0));
		_dataArray.Add(new MiscItem(386, 747, 12, 1200, 1, 385, "icon_Misc_ganxiexin", 748, transferable: false, stackable: true, wagerable: false, refinable: false, poisonable: false, repairable: false, inheritable: true, consumable: true, 0, 0, 5, 10, 0, 0, 50, 8, allowRandomCreate: true, 0, isSpecial: false, -1, -1, 1, -1, 0, 0, 0, -1, canUseOnPrepareCombat: false, allowUseInPlayAndTest: false, -1, 60, new List<short>(), new List<int>(), EMiscGenerateType.Invalid, new List<TreasureStateInfo>(), EMiscFilterType.Item, -1, -1, 600, 0));
		_dataArray.Add(new MiscItem(387, 747, 12, 1200, 2, 385, "icon_Misc_ganxiexin", 748, transferable: false, stackable: true, wagerable: false, refinable: false, poisonable: false, repairable: false, inheritable: true, consumable: true, 0, 0, 5, 10, 0, 0, 50, 8, allowRandomCreate: true, 0, isSpecial: false, -1, -1, 1, -1, 0, 0, 0, -1, canUseOnPrepareCombat: false, allowUseInPlayAndTest: false, -1, 60, new List<short>(), new List<int>(), EMiscGenerateType.Invalid, new List<TreasureStateInfo>(), EMiscFilterType.Item, -1, -1, 900, 0));
		_dataArray.Add(new MiscItem(388, 747, 12, 1200, 3, 385, "icon_Misc_ganxiexin", 748, transferable: false, stackable: true, wagerable: false, refinable: false, poisonable: false, repairable: false, inheritable: true, consumable: true, 0, 0, 5, 10, 0, 0, 50, 8, allowRandomCreate: true, 0, isSpecial: false, -1, -1, 1, -1, 0, 0, 0, -1, canUseOnPrepareCombat: false, allowUseInPlayAndTest: false, -1, 60, new List<short>(), new List<int>(), EMiscGenerateType.Invalid, new List<TreasureStateInfo>(), EMiscFilterType.Item, -1, -1, 1200, 0));
		_dataArray.Add(new MiscItem(389, 747, 12, 1200, 4, 385, "icon_Misc_ganxiexin", 748, transferable: false, stackable: true, wagerable: false, refinable: false, poisonable: false, repairable: false, inheritable: true, consumable: true, 0, 0, 5, 10, 0, 0, 50, 8, allowRandomCreate: true, 0, isSpecial: false, -1, -1, 1, -1, 0, 0, 0, -1, canUseOnPrepareCombat: false, allowUseInPlayAndTest: false, -1, 60, new List<short>(), new List<int>(), EMiscGenerateType.Invalid, new List<TreasureStateInfo>(), EMiscFilterType.Item, -1, -1, 1500, 0));
		_dataArray.Add(new MiscItem(390, 747, 12, 1200, 5, 385, "icon_Misc_ganxiexin", 748, transferable: false, stackable: true, wagerable: false, refinable: false, poisonable: false, repairable: false, inheritable: true, consumable: true, 0, 0, 5, 10, 0, 0, 50, 8, allowRandomCreate: true, 0, isSpecial: false, -1, -1, 1, -1, 0, 0, 0, -1, canUseOnPrepareCombat: false, allowUseInPlayAndTest: false, -1, 60, new List<short>(), new List<int>(), EMiscGenerateType.Invalid, new List<TreasureStateInfo>(), EMiscFilterType.Item, -1, -1, 1800, 0));
		_dataArray.Add(new MiscItem(391, 747, 12, 1200, 6, 385, "icon_Misc_ganxiexin", 748, transferable: false, stackable: true, wagerable: false, refinable: false, poisonable: false, repairable: false, inheritable: true, consumable: true, 0, 0, 5, 10, 0, 0, 50, 8, allowRandomCreate: true, 0, isSpecial: false, -1, -1, 1, -1, 0, 0, 0, -1, canUseOnPrepareCombat: false, allowUseInPlayAndTest: false, -1, 60, new List<short>(), new List<int>(), EMiscGenerateType.Invalid, new List<TreasureStateInfo>(), EMiscFilterType.Item, -1, -1, 2100, 0));
		_dataArray.Add(new MiscItem(392, 747, 12, 1200, 7, 385, "icon_Misc_ganxiexin", 748, transferable: false, stackable: true, wagerable: false, refinable: false, poisonable: false, repairable: false, inheritable: true, consumable: true, 0, 0, 5, 10, 0, 0, 50, 8, allowRandomCreate: true, 0, isSpecial: false, -1, -1, 1, -1, 0, 0, 0, -1, canUseOnPrepareCombat: false, allowUseInPlayAndTest: false, -1, 60, new List<short>(), new List<int>(), EMiscGenerateType.Invalid, new List<TreasureStateInfo>(), EMiscFilterType.Item, -1, -1, 2400, 0));
		_dataArray.Add(new MiscItem(393, 747, 12, 1200, 8, 385, "icon_Misc_ganxiexin", 748, transferable: false, stackable: true, wagerable: false, refinable: false, poisonable: false, repairable: false, inheritable: true, consumable: true, 0, 0, 5, 10, 0, 0, 50, 8, allowRandomCreate: true, 0, isSpecial: false, -1, -1, 1, -1, 0, 0, 0, -1, canUseOnPrepareCombat: false, allowUseInPlayAndTest: false, -1, 60, new List<short>(), new List<int>(), EMiscGenerateType.Invalid, new List<TreasureStateInfo>(), EMiscFilterType.Item, -1, -1, 2700, 0));
		_dataArray.Add(new MiscItem(394, 749, 12, 1200, 5, 394, "icon_Misc_shenmuzhong", 750, transferable: false, stackable: true, wagerable: false, refinable: false, poisonable: false, repairable: false, inheritable: false, consumable: false, 0, 0, 0, 0, 0, 0, 0, 0, allowRandomCreate: true, 0, isSpecial: true, -1, -1, -1, -1, 0, 0, 0, -1, canUseOnPrepareCombat: false, allowUseInPlayAndTest: false, -1, 60, new List<short>(), new List<int>(), EMiscGenerateType.Invalid, new List<TreasureStateInfo>(), EMiscFilterType.Item, -1, -1, -1, 0));
		_dataArray.Add(new MiscItem(395, 751, 12, 1200, 5, 394, "icon_Misc_shenmuzhong", 750, transferable: false, stackable: true, wagerable: false, refinable: false, poisonable: false, repairable: false, inheritable: false, consumable: false, 0, 0, 0, 0, 0, 0, 0, 0, allowRandomCreate: true, 0, isSpecial: true, -1, -1, -1, -1, 0, 0, 0, -1, canUseOnPrepareCombat: false, allowUseInPlayAndTest: false, -1, 60, new List<short>(), new List<int>(), EMiscGenerateType.Invalid, new List<TreasureStateInfo>(), EMiscFilterType.Item, -1, -1, -1, 0));
		_dataArray.Add(new MiscItem(396, 752, 12, 1200, 5, 394, "icon_Misc_shenmuzhong", 750, transferable: false, stackable: true, wagerable: false, refinable: false, poisonable: false, repairable: false, inheritable: false, consumable: false, 0, 0, 0, 0, 0, 0, 0, 0, allowRandomCreate: true, 0, isSpecial: true, -1, -1, -1, -1, 0, 0, 0, -1, canUseOnPrepareCombat: false, allowUseInPlayAndTest: false, -1, 60, new List<short>(), new List<int>(), EMiscGenerateType.Invalid, new List<TreasureStateInfo>(), EMiscFilterType.Item, -1, -1, -1, 0));
		_dataArray.Add(new MiscItem(397, 753, 12, 1200, 5, 394, "icon_Misc_shenmuzhong", 750, transferable: false, stackable: true, wagerable: false, refinable: false, poisonable: false, repairable: false, inheritable: false, consumable: false, 0, 0, 0, 0, 0, 0, 0, 0, allowRandomCreate: true, 0, isSpecial: true, -1, -1, -1, -1, 0, 0, 0, -1, canUseOnPrepareCombat: false, allowUseInPlayAndTest: false, -1, 60, new List<short>(), new List<int>(), EMiscGenerateType.Invalid, new List<TreasureStateInfo>(), EMiscFilterType.Item, -1, -1, -1, 0));
		_dataArray.Add(new MiscItem(398, 754, 12, 1200, 5, 394, "icon_Misc_shenmuzhong", 750, transferable: false, stackable: true, wagerable: false, refinable: false, poisonable: false, repairable: false, inheritable: false, consumable: false, 0, 0, 0, 0, 0, 0, 0, 0, allowRandomCreate: true, 0, isSpecial: true, -1, -1, -1, -1, 0, 0, 0, -1, canUseOnPrepareCombat: false, allowUseInPlayAndTest: false, -1, 60, new List<short>(), new List<int>(), EMiscGenerateType.Invalid, new List<TreasureStateInfo>(), EMiscFilterType.Item, -1, -1, -1, 0));
		_dataArray.Add(new MiscItem(399, 755, 12, 1200, 5, 394, "icon_Misc_shenmuzhong", 750, transferable: false, stackable: true, wagerable: false, refinable: false, poisonable: false, repairable: false, inheritable: false, consumable: false, 0, 0, 0, 0, 0, 0, 0, 0, allowRandomCreate: true, 0, isSpecial: true, -1, -1, -1, -1, 0, 0, 0, -1, canUseOnPrepareCombat: false, allowUseInPlayAndTest: false, -1, 60, new List<short>(), new List<int>(), EMiscGenerateType.Invalid, new List<TreasureStateInfo>(), EMiscFilterType.Item, -1, -1, -1, 0));
		_dataArray.Add(new MiscItem(400, 756, 12, 1200, 5, 394, "icon_Misc_shenmuzhong", 750, transferable: false, stackable: true, wagerable: false, refinable: false, poisonable: false, repairable: false, inheritable: false, consumable: false, 0, 0, 0, 0, 0, 0, 0, 0, allowRandomCreate: true, 0, isSpecial: true, -1, -1, -1, -1, 0, 0, 0, -1, canUseOnPrepareCombat: false, allowUseInPlayAndTest: false, -1, 60, new List<short>(), new List<int>(), EMiscGenerateType.Invalid, new List<TreasureStateInfo>(), EMiscFilterType.Item, -1, -1, -1, 0));
		_dataArray.Add(new MiscItem(401, 757, 12, 1200, 5, 394, "icon_Misc_shenmuzhong", 750, transferable: false, stackable: true, wagerable: false, refinable: false, poisonable: false, repairable: false, inheritable: false, consumable: false, 0, 0, 0, 0, 0, 0, 0, 0, allowRandomCreate: true, 0, isSpecial: true, -1, -1, -1, -1, 0, 0, 0, -1, canUseOnPrepareCombat: false, allowUseInPlayAndTest: false, -1, 60, new List<short>(), new List<int>(), EMiscGenerateType.Invalid, new List<TreasureStateInfo>(), EMiscFilterType.Item, -1, -1, -1, 0));
		_dataArray.Add(new MiscItem(402, 758, 12, 1200, 5, 394, "icon_Misc_shenmuzhong", 750, transferable: false, stackable: true, wagerable: false, refinable: false, poisonable: false, repairable: false, inheritable: false, consumable: false, 0, 0, 0, 0, 0, 0, 0, 0, allowRandomCreate: true, 0, isSpecial: true, -1, -1, -1, -1, 0, 0, 0, -1, canUseOnPrepareCombat: false, allowUseInPlayAndTest: false, -1, 60, new List<short>(), new List<int>(), EMiscGenerateType.Invalid, new List<TreasureStateInfo>(), EMiscFilterType.Item, -1, -1, -1, 0));
		_dataArray.Add(new MiscItem(403, 759, 12, 1200, 5, 394, "icon_Misc_shenmuzhong", 750, transferable: false, stackable: true, wagerable: false, refinable: false, poisonable: false, repairable: false, inheritable: false, consumable: false, 0, 0, 0, 0, 0, 0, 0, 0, allowRandomCreate: true, 0, isSpecial: true, -1, -1, -1, -1, 0, 0, 0, -1, canUseOnPrepareCombat: false, allowUseInPlayAndTest: false, -1, 60, new List<short>(), new List<int>(), EMiscGenerateType.Invalid, new List<TreasureStateInfo>(), EMiscFilterType.Item, -1, -1, -1, 0));
		_dataArray.Add(new MiscItem(404, 760, 12, 1200, 5, 394, "icon_Misc_shenmuzhong", 750, transferable: false, stackable: true, wagerable: false, refinable: false, poisonable: false, repairable: false, inheritable: false, consumable: false, 0, 0, 0, 0, 0, 0, 0, 0, allowRandomCreate: true, 0, isSpecial: true, -1, -1, -1, -1, 0, 0, 0, -1, canUseOnPrepareCombat: false, allowUseInPlayAndTest: false, -1, 60, new List<short>(), new List<int>(), EMiscGenerateType.Invalid, new List<TreasureStateInfo>(), EMiscFilterType.Item, -1, -1, -1, 0));
		_dataArray.Add(new MiscItem(405, 761, 12, 1200, 5, 394, "icon_Misc_shenmuzhong", 750, transferable: false, stackable: true, wagerable: false, refinable: false, poisonable: false, repairable: false, inheritable: false, consumable: false, 0, 0, 0, 0, 0, 0, 0, 0, allowRandomCreate: true, 0, isSpecial: true, -1, -1, -1, -1, 0, 0, 0, -1, canUseOnPrepareCombat: false, allowUseInPlayAndTest: false, -1, 60, new List<short>(), new List<int>(), EMiscGenerateType.Invalid, new List<TreasureStateInfo>(), EMiscFilterType.Item, -1, -1, -1, 0));
		_dataArray.Add(new MiscItem(406, 762, 12, 1200, 0, -1, null, 763, transferable: true, stackable: true, wagerable: false, refinable: false, poisonable: false, repairable: false, inheritable: true, consumable: false, 0, 10, 50, 100, 0, 0, 600, 3, allowRandomCreate: true, 50, isSpecial: false, -1, -1, 12, -1, 0, 0, 0, -1, canUseOnPrepareCombat: false, allowUseInPlayAndTest: false, -1, 60, new List<short>(), new List<int>(), EMiscGenerateType.Invalid, new List<TreasureStateInfo>(), EMiscFilterType.Item, -1, -1, -1, 0));
		_dataArray.Add(new MiscItem(407, 764, 12, 1200, 8, -1, "icon_Misc_qiwenxingdou", 765, transferable: false, stackable: false, wagerable: false, refinable: false, poisonable: false, repairable: false, inheritable: false, consumable: false, 0, 0, 0, 0, 0, 0, 0, 0, allowRandomCreate: true, 0, isSpecial: true, -1, -1, -1, -1, 0, 0, 0, -1, canUseOnPrepareCombat: false, allowUseInPlayAndTest: false, -1, 60, new List<short>(), new List<int>(), EMiscGenerateType.Invalid, new List<TreasureStateInfo>(), EMiscFilterType.Item, -1, -1, -1, 0));
		_dataArray.Add(new MiscItem(408, 766, 12, 1200, 8, -1, "icon_Misc_shangxingguimian", 767, transferable: false, stackable: false, wagerable: false, refinable: false, poisonable: false, repairable: false, inheritable: false, consumable: false, 0, 0, 0, 0, 0, 0, 0, 0, allowRandomCreate: true, 0, isSpecial: true, -1, -1, -1, -1, 0, 0, 0, -1, canUseOnPrepareCombat: false, allowUseInPlayAndTest: false, -1, 60, new List<short>(), new List<int>(), EMiscGenerateType.Invalid, new List<TreasureStateInfo>(), EMiscFilterType.Item, -1, -1, -1, 0));
	}

	public override void Init()
	{
		base.Init();
		_dataArray = new List<MiscItem>(409);
		CreateItems0();
		CreateItems1();
		CreateItems2();
		CreateItems3();
		CreateItems4();
		CreateItems5();
		CreateItems6();
	}
}
