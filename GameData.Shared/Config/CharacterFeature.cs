using System;
using System.Collections.Generic;
using Config.Common;
using Config.ConfigCells.Character;

namespace Config;

[Serializable]
public class CharacterFeature : ConfigData<CharacterFeatureItem, short>
{
	public static class DefKey
	{
		public const short FoeverLoyal = 164;

		public const short InfertileMale = 168;

		public const short InfertileFemale = 169;

		public const short OneYearOldCatch0 = 171;

		public const short OneYearOldCatch1 = 172;

		public const short OneYearOldCatch2 = 173;

		public const short OneYearOldCatch3 = 174;

		public const short OneYearOldCatch4 = 175;

		public const short OneYearOldCatch5 = 176;

		public const short OneYearOldCatch6 = 177;

		public const short OneYearOldCatch7 = 178;

		public const short OneYearOldCatch8 = 179;

		public const short OneYearOldCatch9 = 180;

		public const short OneYearOldCatch10 = 181;

		public const short OneYearOldCatch11 = 182;

		public const short BirthMonth0 = 183;

		public const short BirthMonth1 = 184;

		public const short BirthMonth2 = 185;

		public const short BirthMonth3 = 186;

		public const short BirthMonth4 = 187;

		public const short BirthMonth5 = 188;

		public const short BirthMonth6 = 189;

		public const short BirthMonth7 = 190;

		public const short BirthMonth8 = 191;

		public const short BirthMonth9 = 192;

		public const short BirthMonth10 = 193;

		public const short BirthMonth11 = 194;

		public const short VirginityTrue = 195;

		public const short VirginityFalse = 196;

		public const short Pregnant = 197;

		public const short ForcedToFollow = 198;

		public const short FulongServant = 199;

		public const short ProtectedByPrayer = 200;

		public const short LifeSkillLearning = 201;

		public const short CombatSkillLearning = 202;

		public const short DreamLover = 203;

		public const short LegendaryBookShocked = 204;

		public const short LegendaryBookInsane = 205;

		public const short XiangshuNotInfected = 216;

		public const short XiangshuPartlyInfected = 217;

		public const short XiangshuCompletelyInfected = 218;

		public const short ShenNvHuanJian = 220;

		public const short ZhenYuFuXie = 221;

		public const short QiHanLingQi = 222;

		public const short QiWenWuCai = 223;

		public const short QingGuoJueShi = 224;

		public const short LongTaiHuaMing = 225;

		public const short RongChenHuaYu = 226;

		public const short BaHongBaZhi = 227;

		public const short FangTianChiLing = 228;

		public const short HaveElixir0 = 234;

		public const short HaveElixir1 = 235;

		public const short HaveElixir2 = 236;

		public const short GreenMotherSpiderPoison0 = 237;

		public const short GreenMotherSpiderPoison1 = 238;

		public const short GreenMotherSpiderPoison2 = 239;

		public const short StarsRegulateBreath0 = 240;

		public const short StarsRegulateBreath1 = 241;

		public const short StarsRegulateBreath2 = 242;

		public const short SupportFromShixiang0 = 243;

		public const short SupportFromShixiang1 = 244;

		public const short SupportFromShixiang2 = 245;

		public const short HeadHurt = 246;

		public const short HeadCrash = 247;

		public const short ChestHurt = 248;

		public const short ChestCrash = 249;

		public const short BellyHurt = 250;

		public const short BellyCrash = 251;

		public const short HandHurt = 252;

		public const short HandCrash = 253;

		public const short LegHurt = 254;

		public const short LegCrash = 255;

		public const short WhiteSnake = 256;

		public const short ReincarnationBonus0 = 257;

		public const short ReincarnationBonus1 = 258;

		public const short ReincarnationBonus2 = 259;

		public const short ReincarnationBonus3 = 260;

		public const short ReincarnationBonus4 = 261;

		public const short ChickenClever0 = 262;

		public const short ChickenClever1 = 263;

		public const short ChickenClever2 = 264;

		public const short ChickenClever3 = 265;

		public const short ChickenClever4 = 266;

		public const short ChickenClever5 = 267;

		public const short ChickenClever6 = 268;

		public const short ChickenClever7 = 269;

		public const short ChickenClever8 = 270;

		public const short ChickenLucky0 = 271;

		public const short ChickenLucky1 = 272;

		public const short ChickenLucky2 = 273;

		public const short ChickenLucky3 = 274;

		public const short ChickenLucky4 = 275;

		public const short ChickenLucky5 = 276;

		public const short ChickenLucky6 = 277;

		public const short ChickenLucky7 = 278;

		public const short ChickenLucky8 = 279;

		public const short ChickenPerceptive0 = 280;

		public const short ChickenPerceptive1 = 281;

		public const short ChickenPerceptive2 = 282;

		public const short ChickenPerceptive3 = 283;

		public const short ChickenPerceptive4 = 284;

		public const short ChickenPerceptive5 = 285;

		public const short ChickenPerceptive6 = 286;

		public const short ChickenPerceptive7 = 287;

		public const short ChickenPerceptive8 = 288;

		public const short ChickenFirm0 = 289;

		public const short ChickenFirm1 = 290;

		public const short ChickenFirm2 = 291;

		public const short ChickenFirm3 = 292;

		public const short ChickenFirm4 = 293;

		public const short ChickenFirm5 = 294;

		public const short ChickenFirm6 = 295;

		public const short ChickenFirm7 = 296;

		public const short ChickenFirm8 = 297;

		public const short ChickenCalm0 = 298;

		public const short ChickenCalm1 = 299;

		public const short ChickenCalm2 = 300;

		public const short ChickenCalm3 = 301;

		public const short ChickenCalm4 = 302;

		public const short ChickenCalm5 = 303;

		public const short ChickenCalm6 = 304;

		public const short ChickenCalm7 = 305;

		public const short ChickenCalm8 = 306;

		public const short ChickenEnthusiastic0 = 307;

		public const short ChickenEnthusiastic1 = 308;

		public const short ChickenEnthusiastic2 = 309;

		public const short ChickenEnthusiastic3 = 310;

		public const short ChickenEnthusiastic4 = 311;

		public const short ChickenEnthusiastic5 = 312;

		public const short ChickenEnthusiastic6 = 313;

		public const short ChickenEnthusiastic7 = 314;

		public const short ChickenEnthusiastic8 = 315;

		public const short ChickenBrave0 = 316;

		public const short ChickenBrave1 = 317;

		public const short ChickenBrave2 = 318;

		public const short ChickenBrave3 = 319;

		public const short ChickenBrave4 = 320;

		public const short ChickenBrave5 = 321;

		public const short ChickenBrave6 = 322;

		public const short ChickenBrave7 = 323;

		public const short ChickenBrave8 = 324;

		public const short Longevity = 335;

		public const short WalkingDead = 337;

		public const short CongenitalMalformation = 339;

		public const short AffectedByLegendaryBookOwner = 340;

		public const short LegendaryBook_1_1 = 341;

		public const short LegendaryBook_2_1 = 342;

		public const short LegendaryBook_3_1 = 343;

		public const short LegendaryBook_4_1 = 344;

		public const short LegendaryBook_5_1 = 345;

		public const short LegendaryBook_6_1 = 346;

		public const short LegendaryBook_7_1 = 347;

		public const short LegendaryBook_8_1 = 348;

		public const short LegendaryBook_9_1 = 349;

		public const short LegendaryBook_10_1 = 350;

		public const short LegendaryBook_11_1 = 351;

		public const short LegendaryBook_12_1 = 352;

		public const short LegendaryBook_13_1 = 353;

		public const short LegendaryBook_14_1 = 354;

		public const short LegendaryBook_1_2 = 355;

		public const short LegendaryBook_2_2 = 356;

		public const short LegendaryBook_3_2 = 357;

		public const short LegendaryBook_4_2 = 358;

		public const short LegendaryBook_5_2 = 359;

		public const short LegendaryBook_6_2 = 360;

		public const short LegendaryBook_7_2 = 361;

		public const short LegendaryBook_8_2 = 362;

		public const short LegendaryBook_9_2 = 363;

		public const short LegendaryBook_10_2 = 364;

		public const short LegendaryBook_11_2 = 365;

		public const short LegendaryBook_12_2 = 366;

		public const short LegendaryBook_13_2 = 367;

		public const short LegendaryBook_14_2 = 368;

		public const short LegendaryBook_1_3 = 369;

		public const short LegendaryBook_2_3 = 370;

		public const short LegendaryBook_3_3 = 371;

		public const short LegendaryBook_4_3 = 372;

		public const short LegendaryBook_5_3 = 373;

		public const short LegendaryBook_6_3 = 374;

		public const short LegendaryBook_7_3 = 375;

		public const short LegendaryBook_8_3 = 376;

		public const short LegendaryBook_9_3 = 377;

		public const short LegendaryBook_10_3 = 378;

		public const short LegendaryBook_11_3 = 379;

		public const short LegendaryBook_12_3 = 380;

		public const short LegendaryBook_13_3 = 381;

		public const short LegendaryBook_14_3 = 382;

		public const short DateWithLover = 383;

		public const short LoveAsStrongAsDeath = 384;

		public const short SeeYouNextLife = 385;

		public const short ThreeLifeLover = 386;

		public const short Disaster_1 = 387;

		public const short Disaster_2 = 388;

		public const short Disaster_3 = 389;

		public const short Disaster_4 = 390;

		public const short Disaster_5 = 391;

		public const short Disaster_6 = 392;

		public const short Disaster_7 = 393;

		public const short ReincarnationBonus5 = 394;

		public const short ReincarnationBonus6 = 395;

		public const short ReincarnationBonus7 = 396;

		public const short ReincarnationBonus8 = 397;

		public const short ReincarnationBonus9 = 398;

		public const short HereticLeader = 403;

		public const short SectLeader = 405;

		public const short Righteous = 406;

		public const short MirrorCreatedImposture = 415;

		public const short MusicBonusFeatureBegin = 416;

		public const short SectMainStoryJingangBeingHaunted = 483;

		public const short PureReincarnation = 484;

		public const short ContaminatedReincarnation = 485;

		public const short SectMainStoryWuxianFailingParanoia = 486;

		public const short ShavedAvatarBegin = 496;

		public const short ShaolinMentee = 499;

		public const short TreasuryGuardBegin = 536;

		public const short TreasuryGuardLow = 537;

		public const short TreasuryGuardMid = 538;

		public const short TreasuryGuardHigh = 539;

		public const short SectMainStoryBaihuaEndemic0 = 540;

		public const short SectMainStoryBaihuaSpecialDebuff0 = 541;

		public const short SectMainStoryBaihuaSpecialDebuff1 = 542;

		public const short SectMainStoryBaihuaEndemic1 = 544;

		public const short SectMainStoryRanshanHuaju = 546;

		public const short SectMainStoryRanshanXuanzhi = 547;

		public const short SectMainStoryRanshanYingjiao = 548;

		public const short PreviousTreasuryGuard = 553;

		public const short DeepValleyCloseFriend = 554;

		public const short JieqingPunish0 = 595;

		public const short JieqingPunish1 = 596;

		public const short JieqingPunish2 = 597;

		public const short JieqingPunish3 = 598;

		public const short JieqingPunish4 = 599;

		public const short BuddhistMonkBonus0 = 605;

		public const short BuddhistMonkBonus1 = 606;

		public const short BuddhistMonkBonus2 = 607;

		public const short BuddhistMonkBonus3 = 608;

		public const short BuddhistMonkBonus4 = 609;

		public const short BuddhistMonkBonus5 = 610;

		public const short BuddhistMonkBonus6 = 611;

		public const short BuddhistMonkBonus7 = 612;

		public const short BuddhistMonkBonus8 = 613;

		public const short BuddhistMonkBonus9 = 614;

		public const short TravelingBuddhistMonkBonus0 = 616;

		public const short TravelingBuddhistMonkBonus1 = 617;

		public const short TravelingBuddhistMonkBonus2 = 618;

		public const short TravelingBuddhistMonkBonus3 = 619;

		public const short TravelingBuddhistMonkBonus4 = 620;

		public const short TravelingBuddhistMonkBonus5 = 621;

		public const short TravelingBuddhistMonkBonus6 = 622;

		public const short TravelingBuddhistMonkBonus7 = 623;

		public const short TravelingBuddhistMonkBonus8 = 624;

		public const short TravelingBuddhistMonkBonus9 = 625;

		public const short TaoistMonkUltimate0 = 627;

		public const short TaoistMonkUltimate1 = 628;

		public const short TaoistMonkUltimate2 = 629;

		public const short SpiritualDebtJingangStrength = 630;

		public const short SpiritualDebtJingangVitality = 631;

		public const short SpiritualDebtJingangDexterity = 632;

		public const short SpiritualDebtJingangEnergy = 633;

		public const short SpiritualDebtJingangIntelligence = 634;

		public const short SpiritualDebtJingangConcentration = 635;

		public const short SpiritualDebtXuehou = 636;

		public const short SpiritualDebtKongsang = 637;

		public const short TaiwuVillageFarm = 734;

		public const short SwordTombKeeperMonv = 741;

		public const short SwordTombKeeperDayueYaochang = 742;

		public const short SwordTombKeeperJiuhan = 743;

		public const short SwordTombKeeperJinHuanger = 744;

		public const short SwordTombKeeperYiYihou = 745;

		public const short SwordTombKeeperWeiQi = 746;

		public const short SwordTombKeeperYixiang = 747;

		public const short SwordTombKeeperXuefeng = 748;

		public const short SwordTombKeeperShuFang = 749;

		public const short ConsummateLevelBroken = 756;

		public const short ConsummateLevelBroken0 = 757;

		public const short DarkAsh = 758;

		public const short ThreeVitalProtect = 759;
	}

	public static class DefValue
	{
		public static CharacterFeatureItem FoeverLoyal => Instance[(short)164];

		public static CharacterFeatureItem InfertileMale => Instance[(short)168];

		public static CharacterFeatureItem InfertileFemale => Instance[(short)169];

		public static CharacterFeatureItem OneYearOldCatch0 => Instance[(short)171];

		public static CharacterFeatureItem OneYearOldCatch1 => Instance[(short)172];

		public static CharacterFeatureItem OneYearOldCatch2 => Instance[(short)173];

		public static CharacterFeatureItem OneYearOldCatch3 => Instance[(short)174];

		public static CharacterFeatureItem OneYearOldCatch4 => Instance[(short)175];

		public static CharacterFeatureItem OneYearOldCatch5 => Instance[(short)176];

		public static CharacterFeatureItem OneYearOldCatch6 => Instance[(short)177];

		public static CharacterFeatureItem OneYearOldCatch7 => Instance[(short)178];

		public static CharacterFeatureItem OneYearOldCatch8 => Instance[(short)179];

		public static CharacterFeatureItem OneYearOldCatch9 => Instance[(short)180];

		public static CharacterFeatureItem OneYearOldCatch10 => Instance[(short)181];

		public static CharacterFeatureItem OneYearOldCatch11 => Instance[(short)182];

		public static CharacterFeatureItem BirthMonth0 => Instance[(short)183];

		public static CharacterFeatureItem BirthMonth1 => Instance[(short)184];

		public static CharacterFeatureItem BirthMonth2 => Instance[(short)185];

		public static CharacterFeatureItem BirthMonth3 => Instance[(short)186];

		public static CharacterFeatureItem BirthMonth4 => Instance[(short)187];

		public static CharacterFeatureItem BirthMonth5 => Instance[(short)188];

		public static CharacterFeatureItem BirthMonth6 => Instance[(short)189];

		public static CharacterFeatureItem BirthMonth7 => Instance[(short)190];

		public static CharacterFeatureItem BirthMonth8 => Instance[(short)191];

		public static CharacterFeatureItem BirthMonth9 => Instance[(short)192];

		public static CharacterFeatureItem BirthMonth10 => Instance[(short)193];

		public static CharacterFeatureItem BirthMonth11 => Instance[(short)194];

		public static CharacterFeatureItem VirginityTrue => Instance[(short)195];

		public static CharacterFeatureItem VirginityFalse => Instance[(short)196];

		public static CharacterFeatureItem Pregnant => Instance[(short)197];

		public static CharacterFeatureItem ForcedToFollow => Instance[(short)198];

		public static CharacterFeatureItem FulongServant => Instance[(short)199];

		public static CharacterFeatureItem ProtectedByPrayer => Instance[(short)200];

		public static CharacterFeatureItem LifeSkillLearning => Instance[(short)201];

		public static CharacterFeatureItem CombatSkillLearning => Instance[(short)202];

		public static CharacterFeatureItem DreamLover => Instance[(short)203];

		public static CharacterFeatureItem LegendaryBookShocked => Instance[(short)204];

		public static CharacterFeatureItem LegendaryBookInsane => Instance[(short)205];

		public static CharacterFeatureItem XiangshuNotInfected => Instance[(short)216];

		public static CharacterFeatureItem XiangshuPartlyInfected => Instance[(short)217];

		public static CharacterFeatureItem XiangshuCompletelyInfected => Instance[(short)218];

		public static CharacterFeatureItem ShenNvHuanJian => Instance[(short)220];

		public static CharacterFeatureItem ZhenYuFuXie => Instance[(short)221];

		public static CharacterFeatureItem QiHanLingQi => Instance[(short)222];

		public static CharacterFeatureItem QiWenWuCai => Instance[(short)223];

		public static CharacterFeatureItem QingGuoJueShi => Instance[(short)224];

		public static CharacterFeatureItem LongTaiHuaMing => Instance[(short)225];

		public static CharacterFeatureItem RongChenHuaYu => Instance[(short)226];

		public static CharacterFeatureItem BaHongBaZhi => Instance[(short)227];

		public static CharacterFeatureItem FangTianChiLing => Instance[(short)228];

		public static CharacterFeatureItem HaveElixir0 => Instance[(short)234];

		public static CharacterFeatureItem HaveElixir1 => Instance[(short)235];

		public static CharacterFeatureItem HaveElixir2 => Instance[(short)236];

		public static CharacterFeatureItem GreenMotherSpiderPoison0 => Instance[(short)237];

		public static CharacterFeatureItem GreenMotherSpiderPoison1 => Instance[(short)238];

		public static CharacterFeatureItem GreenMotherSpiderPoison2 => Instance[(short)239];

		public static CharacterFeatureItem StarsRegulateBreath0 => Instance[(short)240];

		public static CharacterFeatureItem StarsRegulateBreath1 => Instance[(short)241];

		public static CharacterFeatureItem StarsRegulateBreath2 => Instance[(short)242];

		public static CharacterFeatureItem SupportFromShixiang0 => Instance[(short)243];

		public static CharacterFeatureItem SupportFromShixiang1 => Instance[(short)244];

		public static CharacterFeatureItem SupportFromShixiang2 => Instance[(short)245];

		public static CharacterFeatureItem HeadHurt => Instance[(short)246];

		public static CharacterFeatureItem HeadCrash => Instance[(short)247];

		public static CharacterFeatureItem ChestHurt => Instance[(short)248];

		public static CharacterFeatureItem ChestCrash => Instance[(short)249];

		public static CharacterFeatureItem BellyHurt => Instance[(short)250];

		public static CharacterFeatureItem BellyCrash => Instance[(short)251];

		public static CharacterFeatureItem HandHurt => Instance[(short)252];

		public static CharacterFeatureItem HandCrash => Instance[(short)253];

		public static CharacterFeatureItem LegHurt => Instance[(short)254];

		public static CharacterFeatureItem LegCrash => Instance[(short)255];

		public static CharacterFeatureItem WhiteSnake => Instance[(short)256];

		public static CharacterFeatureItem ReincarnationBonus0 => Instance[(short)257];

		public static CharacterFeatureItem ReincarnationBonus1 => Instance[(short)258];

		public static CharacterFeatureItem ReincarnationBonus2 => Instance[(short)259];

		public static CharacterFeatureItem ReincarnationBonus3 => Instance[(short)260];

		public static CharacterFeatureItem ReincarnationBonus4 => Instance[(short)261];

		public static CharacterFeatureItem ChickenClever0 => Instance[(short)262];

		public static CharacterFeatureItem ChickenClever1 => Instance[(short)263];

		public static CharacterFeatureItem ChickenClever2 => Instance[(short)264];

		public static CharacterFeatureItem ChickenClever3 => Instance[(short)265];

		public static CharacterFeatureItem ChickenClever4 => Instance[(short)266];

		public static CharacterFeatureItem ChickenClever5 => Instance[(short)267];

		public static CharacterFeatureItem ChickenClever6 => Instance[(short)268];

		public static CharacterFeatureItem ChickenClever7 => Instance[(short)269];

		public static CharacterFeatureItem ChickenClever8 => Instance[(short)270];

		public static CharacterFeatureItem ChickenLucky0 => Instance[(short)271];

		public static CharacterFeatureItem ChickenLucky1 => Instance[(short)272];

		public static CharacterFeatureItem ChickenLucky2 => Instance[(short)273];

		public static CharacterFeatureItem ChickenLucky3 => Instance[(short)274];

		public static CharacterFeatureItem ChickenLucky4 => Instance[(short)275];

		public static CharacterFeatureItem ChickenLucky5 => Instance[(short)276];

		public static CharacterFeatureItem ChickenLucky6 => Instance[(short)277];

		public static CharacterFeatureItem ChickenLucky7 => Instance[(short)278];

		public static CharacterFeatureItem ChickenLucky8 => Instance[(short)279];

		public static CharacterFeatureItem ChickenPerceptive0 => Instance[(short)280];

		public static CharacterFeatureItem ChickenPerceptive1 => Instance[(short)281];

		public static CharacterFeatureItem ChickenPerceptive2 => Instance[(short)282];

		public static CharacterFeatureItem ChickenPerceptive3 => Instance[(short)283];

		public static CharacterFeatureItem ChickenPerceptive4 => Instance[(short)284];

		public static CharacterFeatureItem ChickenPerceptive5 => Instance[(short)285];

		public static CharacterFeatureItem ChickenPerceptive6 => Instance[(short)286];

		public static CharacterFeatureItem ChickenPerceptive7 => Instance[(short)287];

		public static CharacterFeatureItem ChickenPerceptive8 => Instance[(short)288];

		public static CharacterFeatureItem ChickenFirm0 => Instance[(short)289];

		public static CharacterFeatureItem ChickenFirm1 => Instance[(short)290];

		public static CharacterFeatureItem ChickenFirm2 => Instance[(short)291];

		public static CharacterFeatureItem ChickenFirm3 => Instance[(short)292];

		public static CharacterFeatureItem ChickenFirm4 => Instance[(short)293];

		public static CharacterFeatureItem ChickenFirm5 => Instance[(short)294];

		public static CharacterFeatureItem ChickenFirm6 => Instance[(short)295];

		public static CharacterFeatureItem ChickenFirm7 => Instance[(short)296];

		public static CharacterFeatureItem ChickenFirm8 => Instance[(short)297];

		public static CharacterFeatureItem ChickenCalm0 => Instance[(short)298];

		public static CharacterFeatureItem ChickenCalm1 => Instance[(short)299];

		public static CharacterFeatureItem ChickenCalm2 => Instance[(short)300];

		public static CharacterFeatureItem ChickenCalm3 => Instance[(short)301];

		public static CharacterFeatureItem ChickenCalm4 => Instance[(short)302];

		public static CharacterFeatureItem ChickenCalm5 => Instance[(short)303];

		public static CharacterFeatureItem ChickenCalm6 => Instance[(short)304];

		public static CharacterFeatureItem ChickenCalm7 => Instance[(short)305];

		public static CharacterFeatureItem ChickenCalm8 => Instance[(short)306];

		public static CharacterFeatureItem ChickenEnthusiastic0 => Instance[(short)307];

		public static CharacterFeatureItem ChickenEnthusiastic1 => Instance[(short)308];

		public static CharacterFeatureItem ChickenEnthusiastic2 => Instance[(short)309];

		public static CharacterFeatureItem ChickenEnthusiastic3 => Instance[(short)310];

		public static CharacterFeatureItem ChickenEnthusiastic4 => Instance[(short)311];

		public static CharacterFeatureItem ChickenEnthusiastic5 => Instance[(short)312];

		public static CharacterFeatureItem ChickenEnthusiastic6 => Instance[(short)313];

		public static CharacterFeatureItem ChickenEnthusiastic7 => Instance[(short)314];

		public static CharacterFeatureItem ChickenEnthusiastic8 => Instance[(short)315];

		public static CharacterFeatureItem ChickenBrave0 => Instance[(short)316];

		public static CharacterFeatureItem ChickenBrave1 => Instance[(short)317];

		public static CharacterFeatureItem ChickenBrave2 => Instance[(short)318];

		public static CharacterFeatureItem ChickenBrave3 => Instance[(short)319];

		public static CharacterFeatureItem ChickenBrave4 => Instance[(short)320];

		public static CharacterFeatureItem ChickenBrave5 => Instance[(short)321];

		public static CharacterFeatureItem ChickenBrave6 => Instance[(short)322];

		public static CharacterFeatureItem ChickenBrave7 => Instance[(short)323];

		public static CharacterFeatureItem ChickenBrave8 => Instance[(short)324];

		public static CharacterFeatureItem Longevity => Instance[(short)335];

		public static CharacterFeatureItem WalkingDead => Instance[(short)337];

		public static CharacterFeatureItem CongenitalMalformation => Instance[(short)339];

		public static CharacterFeatureItem AffectedByLegendaryBookOwner => Instance[(short)340];

		public static CharacterFeatureItem LegendaryBook_1_1 => Instance[(short)341];

		public static CharacterFeatureItem LegendaryBook_2_1 => Instance[(short)342];

		public static CharacterFeatureItem LegendaryBook_3_1 => Instance[(short)343];

		public static CharacterFeatureItem LegendaryBook_4_1 => Instance[(short)344];

		public static CharacterFeatureItem LegendaryBook_5_1 => Instance[(short)345];

		public static CharacterFeatureItem LegendaryBook_6_1 => Instance[(short)346];

		public static CharacterFeatureItem LegendaryBook_7_1 => Instance[(short)347];

		public static CharacterFeatureItem LegendaryBook_8_1 => Instance[(short)348];

		public static CharacterFeatureItem LegendaryBook_9_1 => Instance[(short)349];

		public static CharacterFeatureItem LegendaryBook_10_1 => Instance[(short)350];

		public static CharacterFeatureItem LegendaryBook_11_1 => Instance[(short)351];

		public static CharacterFeatureItem LegendaryBook_12_1 => Instance[(short)352];

		public static CharacterFeatureItem LegendaryBook_13_1 => Instance[(short)353];

		public static CharacterFeatureItem LegendaryBook_14_1 => Instance[(short)354];

		public static CharacterFeatureItem LegendaryBook_1_2 => Instance[(short)355];

		public static CharacterFeatureItem LegendaryBook_2_2 => Instance[(short)356];

		public static CharacterFeatureItem LegendaryBook_3_2 => Instance[(short)357];

		public static CharacterFeatureItem LegendaryBook_4_2 => Instance[(short)358];

		public static CharacterFeatureItem LegendaryBook_5_2 => Instance[(short)359];

		public static CharacterFeatureItem LegendaryBook_6_2 => Instance[(short)360];

		public static CharacterFeatureItem LegendaryBook_7_2 => Instance[(short)361];

		public static CharacterFeatureItem LegendaryBook_8_2 => Instance[(short)362];

		public static CharacterFeatureItem LegendaryBook_9_2 => Instance[(short)363];

		public static CharacterFeatureItem LegendaryBook_10_2 => Instance[(short)364];

		public static CharacterFeatureItem LegendaryBook_11_2 => Instance[(short)365];

		public static CharacterFeatureItem LegendaryBook_12_2 => Instance[(short)366];

		public static CharacterFeatureItem LegendaryBook_13_2 => Instance[(short)367];

		public static CharacterFeatureItem LegendaryBook_14_2 => Instance[(short)368];

		public static CharacterFeatureItem LegendaryBook_1_3 => Instance[(short)369];

		public static CharacterFeatureItem LegendaryBook_2_3 => Instance[(short)370];

		public static CharacterFeatureItem LegendaryBook_3_3 => Instance[(short)371];

		public static CharacterFeatureItem LegendaryBook_4_3 => Instance[(short)372];

		public static CharacterFeatureItem LegendaryBook_5_3 => Instance[(short)373];

		public static CharacterFeatureItem LegendaryBook_6_3 => Instance[(short)374];

		public static CharacterFeatureItem LegendaryBook_7_3 => Instance[(short)375];

		public static CharacterFeatureItem LegendaryBook_8_3 => Instance[(short)376];

		public static CharacterFeatureItem LegendaryBook_9_3 => Instance[(short)377];

		public static CharacterFeatureItem LegendaryBook_10_3 => Instance[(short)378];

		public static CharacterFeatureItem LegendaryBook_11_3 => Instance[(short)379];

		public static CharacterFeatureItem LegendaryBook_12_3 => Instance[(short)380];

		public static CharacterFeatureItem LegendaryBook_13_3 => Instance[(short)381];

		public static CharacterFeatureItem LegendaryBook_14_3 => Instance[(short)382];

		public static CharacterFeatureItem DateWithLover => Instance[(short)383];

		public static CharacterFeatureItem LoveAsStrongAsDeath => Instance[(short)384];

		public static CharacterFeatureItem SeeYouNextLife => Instance[(short)385];

		public static CharacterFeatureItem ThreeLifeLover => Instance[(short)386];

		public static CharacterFeatureItem Disaster_1 => Instance[(short)387];

		public static CharacterFeatureItem Disaster_2 => Instance[(short)388];

		public static CharacterFeatureItem Disaster_3 => Instance[(short)389];

		public static CharacterFeatureItem Disaster_4 => Instance[(short)390];

		public static CharacterFeatureItem Disaster_5 => Instance[(short)391];

		public static CharacterFeatureItem Disaster_6 => Instance[(short)392];

		public static CharacterFeatureItem Disaster_7 => Instance[(short)393];

		public static CharacterFeatureItem ReincarnationBonus5 => Instance[(short)394];

		public static CharacterFeatureItem ReincarnationBonus6 => Instance[(short)395];

		public static CharacterFeatureItem ReincarnationBonus7 => Instance[(short)396];

		public static CharacterFeatureItem ReincarnationBonus8 => Instance[(short)397];

		public static CharacterFeatureItem ReincarnationBonus9 => Instance[(short)398];

		public static CharacterFeatureItem HereticLeader => Instance[(short)403];

		public static CharacterFeatureItem SectLeader => Instance[(short)405];

		public static CharacterFeatureItem Righteous => Instance[(short)406];

		public static CharacterFeatureItem MirrorCreatedImposture => Instance[(short)415];

		public static CharacterFeatureItem MusicBonusFeatureBegin => Instance[(short)416];

		public static CharacterFeatureItem SectMainStoryJingangBeingHaunted => Instance[(short)483];

		public static CharacterFeatureItem PureReincarnation => Instance[(short)484];

		public static CharacterFeatureItem ContaminatedReincarnation => Instance[(short)485];

		public static CharacterFeatureItem SectMainStoryWuxianFailingParanoia => Instance[(short)486];

		public static CharacterFeatureItem ShavedAvatarBegin => Instance[(short)496];

		public static CharacterFeatureItem ShaolinMentee => Instance[(short)499];

		public static CharacterFeatureItem TreasuryGuardBegin => Instance[(short)536];

		public static CharacterFeatureItem TreasuryGuardLow => Instance[(short)537];

		public static CharacterFeatureItem TreasuryGuardMid => Instance[(short)538];

		public static CharacterFeatureItem TreasuryGuardHigh => Instance[(short)539];

		public static CharacterFeatureItem SectMainStoryBaihuaEndemic0 => Instance[(short)540];

		public static CharacterFeatureItem SectMainStoryBaihuaSpecialDebuff0 => Instance[(short)541];

		public static CharacterFeatureItem SectMainStoryBaihuaSpecialDebuff1 => Instance[(short)542];

		public static CharacterFeatureItem SectMainStoryBaihuaEndemic1 => Instance[(short)544];

		public static CharacterFeatureItem SectMainStoryRanshanHuaju => Instance[(short)546];

		public static CharacterFeatureItem SectMainStoryRanshanXuanzhi => Instance[(short)547];

		public static CharacterFeatureItem SectMainStoryRanshanYingjiao => Instance[(short)548];

		public static CharacterFeatureItem PreviousTreasuryGuard => Instance[(short)553];

		public static CharacterFeatureItem DeepValleyCloseFriend => Instance[(short)554];

		public static CharacterFeatureItem JieqingPunish0 => Instance[(short)595];

		public static CharacterFeatureItem JieqingPunish1 => Instance[(short)596];

		public static CharacterFeatureItem JieqingPunish2 => Instance[(short)597];

		public static CharacterFeatureItem JieqingPunish3 => Instance[(short)598];

		public static CharacterFeatureItem JieqingPunish4 => Instance[(short)599];

		public static CharacterFeatureItem BuddhistMonkBonus0 => Instance[(short)605];

		public static CharacterFeatureItem BuddhistMonkBonus1 => Instance[(short)606];

		public static CharacterFeatureItem BuddhistMonkBonus2 => Instance[(short)607];

		public static CharacterFeatureItem BuddhistMonkBonus3 => Instance[(short)608];

		public static CharacterFeatureItem BuddhistMonkBonus4 => Instance[(short)609];

		public static CharacterFeatureItem BuddhistMonkBonus5 => Instance[(short)610];

		public static CharacterFeatureItem BuddhistMonkBonus6 => Instance[(short)611];

		public static CharacterFeatureItem BuddhistMonkBonus7 => Instance[(short)612];

		public static CharacterFeatureItem BuddhistMonkBonus8 => Instance[(short)613];

		public static CharacterFeatureItem BuddhistMonkBonus9 => Instance[(short)614];

		public static CharacterFeatureItem TravelingBuddhistMonkBonus0 => Instance[(short)616];

		public static CharacterFeatureItem TravelingBuddhistMonkBonus1 => Instance[(short)617];

		public static CharacterFeatureItem TravelingBuddhistMonkBonus2 => Instance[(short)618];

		public static CharacterFeatureItem TravelingBuddhistMonkBonus3 => Instance[(short)619];

		public static CharacterFeatureItem TravelingBuddhistMonkBonus4 => Instance[(short)620];

		public static CharacterFeatureItem TravelingBuddhistMonkBonus5 => Instance[(short)621];

		public static CharacterFeatureItem TravelingBuddhistMonkBonus6 => Instance[(short)622];

		public static CharacterFeatureItem TravelingBuddhistMonkBonus7 => Instance[(short)623];

		public static CharacterFeatureItem TravelingBuddhistMonkBonus8 => Instance[(short)624];

		public static CharacterFeatureItem TravelingBuddhistMonkBonus9 => Instance[(short)625];

		public static CharacterFeatureItem TaoistMonkUltimate0 => Instance[(short)627];

		public static CharacterFeatureItem TaoistMonkUltimate1 => Instance[(short)628];

		public static CharacterFeatureItem TaoistMonkUltimate2 => Instance[(short)629];

		public static CharacterFeatureItem SpiritualDebtJingangStrength => Instance[(short)630];

		public static CharacterFeatureItem SpiritualDebtJingangVitality => Instance[(short)631];

		public static CharacterFeatureItem SpiritualDebtJingangDexterity => Instance[(short)632];

		public static CharacterFeatureItem SpiritualDebtJingangEnergy => Instance[(short)633];

		public static CharacterFeatureItem SpiritualDebtJingangIntelligence => Instance[(short)634];

		public static CharacterFeatureItem SpiritualDebtJingangConcentration => Instance[(short)635];

		public static CharacterFeatureItem SpiritualDebtXuehou => Instance[(short)636];

		public static CharacterFeatureItem SpiritualDebtKongsang => Instance[(short)637];

		public static CharacterFeatureItem TaiwuVillageFarm => Instance[(short)734];

		public static CharacterFeatureItem SwordTombKeeperMonv => Instance[(short)741];

		public static CharacterFeatureItem SwordTombKeeperDayueYaochang => Instance[(short)742];

		public static CharacterFeatureItem SwordTombKeeperJiuhan => Instance[(short)743];

		public static CharacterFeatureItem SwordTombKeeperJinHuanger => Instance[(short)744];

		public static CharacterFeatureItem SwordTombKeeperYiYihou => Instance[(short)745];

		public static CharacterFeatureItem SwordTombKeeperWeiQi => Instance[(short)746];

		public static CharacterFeatureItem SwordTombKeeperYixiang => Instance[(short)747];

		public static CharacterFeatureItem SwordTombKeeperXuefeng => Instance[(short)748];

		public static CharacterFeatureItem SwordTombKeeperShuFang => Instance[(short)749];

		public static CharacterFeatureItem ConsummateLevelBroken => Instance[(short)756];

		public static CharacterFeatureItem ConsummateLevelBroken0 => Instance[(short)757];

		public static CharacterFeatureItem DarkAsh => Instance[(short)758];

		public static CharacterFeatureItem ThreeVitalProtect => Instance[(short)759];
	}

	public static CharacterFeature Instance = new CharacterFeature();

	private readonly HashSet<string> RequiredFields = new HashSet<string> { "RequiredOrganization", "MutexGroupId", "TemplateId", "Name", "SmallVillageName", "Desc", "SmallVillageDesc", "EffectDesc", "AssociatedSpecialEffect" };

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
		_dataArray.Add(new CharacterFeatureItem(0, 0, 1, hidden: false, ECharacterFeatureType.Good, 2, 3, 4, new FeatureMedals[3]
		{
			new FeatureMedals("pos"),
			new FeatureMedals(),
			new FeatureMedals()
		}, 1, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: true, canBeExchanged: true, mergeable: true, basic: true, inscribable: true, soulTransform: true, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: true, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 0, 1001, 25, 0, -1, 0, 15, 100, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 10, 0, 0, 0, 10, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(1, 5, 6, hidden: false, ECharacterFeatureType.Good, 7, 8, 9, new FeatureMedals[3]
		{
			new FeatureMedals("pos", "pos"),
			new FeatureMedals(),
			new FeatureMedals()
		}, 2, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: true, canBeExchanged: true, mergeable: true, basic: true, inscribable: true, soulTransform: true, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: true, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 0, 1001, 5, 0, -1, 0, 10, 100, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 20, 0, 0, 0, 20, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(2, 10, 11, hidden: false, ECharacterFeatureType.Good, 12, 13, 14, new FeatureMedals[3]
		{
			new FeatureMedals("pos", "pos", "pos"),
			new FeatureMedals(),
			new FeatureMedals()
		}, 3, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: true, canBeExchanged: true, mergeable: true, basic: true, inscribable: true, soulTransform: true, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: true, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 0, 1001, 1, 0, -1, 0, 0, 100, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 30, 0, 0, 0, 30, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(3, 15, 16, hidden: false, ECharacterFeatureType.Bad, 17, 18, 19, new FeatureMedals[3]
		{
			new FeatureMedals("neg"),
			new FeatureMedals(),
			new FeatureMedals()
		}, -1, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: true, canBeExchanged: true, mergeable: true, basic: true, inscribable: true, soulTransform: true, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: true, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 0, 2001, 25, 0, -1, 1, 5, 100, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, -10, 0, 0, 0, -10, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(4, 20, 21, hidden: false, ECharacterFeatureType.Bad, 22, 23, 24, new FeatureMedals[3]
		{
			new FeatureMedals("neg", "neg"),
			new FeatureMedals(),
			new FeatureMedals()
		}, -2, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: true, canBeExchanged: true, mergeable: true, basic: true, inscribable: true, soulTransform: true, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: true, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 0, 2001, 5, 0, -1, 1, 0, 100, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, -20, 0, 0, 0, -20, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(5, 25, 26, hidden: false, ECharacterFeatureType.Bad, 27, 28, 29, new FeatureMedals[3]
		{
			new FeatureMedals("neg", "neg", "neg"),
			new FeatureMedals(),
			new FeatureMedals()
		}, -3, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: true, canBeExchanged: true, mergeable: true, basic: true, inscribable: true, soulTransform: true, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: true, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 0, 2001, 1, 0, -1, 1, 0, 100, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, -30, 0, 0, 0, -30, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(6, 30, 31, hidden: false, ECharacterFeatureType.Good, 32, 33, 34, new FeatureMedals[3]
		{
			new FeatureMedals(),
			new FeatureMedals("pos"),
			new FeatureMedals()
		}, 1, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: true, canBeExchanged: true, mergeable: true, basic: true, inscribable: true, soulTransform: true, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: true, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 6, 1002, 25, 0, -1, 0, 15, 100, null, 0, 0, 0, 0, 5, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 0, 10, 0, 0, 0, 10, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(7, 35, 36, hidden: false, ECharacterFeatureType.Good, 37, 38, 39, new FeatureMedals[3]
		{
			new FeatureMedals(),
			new FeatureMedals("pos", "pos"),
			new FeatureMedals()
		}, 2, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: true, canBeExchanged: true, mergeable: true, basic: true, inscribable: true, soulTransform: true, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: true, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 6, 1002, 5, 0, -1, 0, 10, 100, null, 0, 0, 0, 0, 10, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 0, 20, 0, 0, 0, 20, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(8, 40, 41, hidden: false, ECharacterFeatureType.Good, 42, 43, 44, new FeatureMedals[3]
		{
			new FeatureMedals(),
			new FeatureMedals("pos", "pos", "pos"),
			new FeatureMedals()
		}, 3, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: true, canBeExchanged: true, mergeable: true, basic: true, inscribable: true, soulTransform: true, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: true, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 6, 1002, 1, 0, -1, 0, 0, 100, null, 0, 0, 0, 0, 15, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 0, 30, 0, 0, 0, 30, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(9, 45, 46, hidden: false, ECharacterFeatureType.Bad, 47, 48, 49, new FeatureMedals[3]
		{
			new FeatureMedals(),
			new FeatureMedals("neg"),
			new FeatureMedals()
		}, -1, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: true, canBeExchanged: true, mergeable: true, basic: true, inscribable: true, soulTransform: true, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: true, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 6, 2002, 25, 0, -1, 1, 5, 100, null, 0, 0, 0, 0, -5, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 0, -10, 0, 0, 0, -10, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(10, 50, 51, hidden: false, ECharacterFeatureType.Bad, 52, 53, 54, new FeatureMedals[3]
		{
			new FeatureMedals(),
			new FeatureMedals("neg", "neg"),
			new FeatureMedals()
		}, -2, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: true, canBeExchanged: true, mergeable: true, basic: true, inscribable: true, soulTransform: true, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: true, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 6, 2002, 5, 0, -1, 1, 0, 100, null, 0, 0, 0, 0, -10, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 0, -20, 0, 0, 0, -20, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(11, 55, 56, hidden: false, ECharacterFeatureType.Bad, 57, 58, 59, new FeatureMedals[3]
		{
			new FeatureMedals(),
			new FeatureMedals("neg", "neg", "neg"),
			new FeatureMedals()
		}, -3, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: true, canBeExchanged: true, mergeable: true, basic: true, inscribable: true, soulTransform: true, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: true, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 6, 2002, 1, 0, -1, 1, 0, 100, null, 0, 0, 0, 0, -15, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 0, -30, 0, 0, 0, -30, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(12, 60, 61, hidden: false, ECharacterFeatureType.Good, 62, 63, 64, new FeatureMedals[3]
		{
			new FeatureMedals(),
			new FeatureMedals(),
			new FeatureMedals("pos")
		}, 1, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: true, canBeExchanged: true, mergeable: true, basic: true, inscribable: true, soulTransform: true, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: true, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 12, 1003, 25, 0, -1, 0, 15, 100, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 10, 0, 0, 0, 0, 0, 0, 10, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(13, 65, 66, hidden: false, ECharacterFeatureType.Good, 67, 68, 69, new FeatureMedals[3]
		{
			new FeatureMedals(),
			new FeatureMedals("pos"),
			new FeatureMedals("pos")
		}, 2, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: true, canBeExchanged: true, mergeable: true, basic: true, inscribable: true, soulTransform: true, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: true, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 12, 1003, 5, 0, -1, 0, 10, 100, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 20, 0, 0, 0, 0, 0, 0, 20, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(14, 70, 71, hidden: false, ECharacterFeatureType.Good, 72, 73, 74, new FeatureMedals[3]
		{
			new FeatureMedals(),
			new FeatureMedals("pos"),
			new FeatureMedals("pos", "pos")
		}, 3, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: true, canBeExchanged: true, mergeable: true, basic: true, inscribable: true, soulTransform: true, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: true, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 12, 1003, 1, 0, -1, 0, 0, 100, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 30, 0, 0, 0, 0, 0, 0, 30, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(15, 75, 76, hidden: false, ECharacterFeatureType.Bad, 77, 78, 79, new FeatureMedals[3]
		{
			new FeatureMedals(),
			new FeatureMedals(),
			new FeatureMedals("neg")
		}, -1, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: true, canBeExchanged: true, mergeable: true, basic: true, inscribable: true, soulTransform: true, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: true, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 12, 2003, 25, 0, -1, 1, 5, 100, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, -10, 0, 0, 0, 0, 0, 0, -10, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(16, 80, 81, hidden: false, ECharacterFeatureType.Bad, 82, 83, 84, new FeatureMedals[3]
		{
			new FeatureMedals(),
			new FeatureMedals("neg"),
			new FeatureMedals("neg")
		}, -2, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: true, canBeExchanged: true, mergeable: true, basic: true, inscribable: true, soulTransform: true, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: true, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 12, 2003, 5, 0, -1, 1, 0, 100, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, -20, 0, 0, 0, 0, 0, 0, -20, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(17, 85, 86, hidden: false, ECharacterFeatureType.Bad, 87, 88, 89, new FeatureMedals[3]
		{
			new FeatureMedals(),
			new FeatureMedals("neg"),
			new FeatureMedals("neg", "neg")
		}, -3, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: true, canBeExchanged: true, mergeable: true, basic: true, inscribable: true, soulTransform: true, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: true, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 12, 2003, 1, 0, -1, 1, 0, 100, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, -30, 0, 0, 0, 0, 0, 0, -30, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(18, 90, 91, hidden: false, ECharacterFeatureType.Good, 92, 93, 94, new FeatureMedals[3]
		{
			new FeatureMedals("pos"),
			new FeatureMedals("pos"),
			new FeatureMedals("pos")
		}, 1, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: true, canBeExchanged: true, mergeable: true, basic: true, inscribable: true, soulTransform: true, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: true, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 18, 1004, 25, 0, -1, 0, 15, 100, null, 0, 0, 0, 0, 5, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 0, 0, 0, 10, 0, 0, 0, 10, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(19, 95, 96, hidden: false, ECharacterFeatureType.Good, 97, 98, 99, new FeatureMedals[3]
		{
			new FeatureMedals("pos"),
			new FeatureMedals("pos"),
			new FeatureMedals("pos")
		}, 2, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: true, canBeExchanged: true, mergeable: true, basic: true, inscribable: true, soulTransform: true, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: true, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 18, 1004, 5, 0, -1, 0, 10, 100, null, 0, 0, 0, 0, 10, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 0, 0, 0, 20, 0, 0, 0, 20, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(20, 100, 101, hidden: false, ECharacterFeatureType.Good, 102, 103, 104, new FeatureMedals[3]
		{
			new FeatureMedals("pos"),
			new FeatureMedals("pos"),
			new FeatureMedals("pos")
		}, 3, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: true, canBeExchanged: true, mergeable: true, basic: true, inscribable: true, soulTransform: true, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: true, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 18, 1004, 1, 0, -1, 0, 0, 100, null, 0, 0, 0, 0, 15, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 0, 0, 0, 30, 0, 0, 0, 30, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(21, 105, 106, hidden: false, ECharacterFeatureType.Bad, 107, 108, 109, new FeatureMedals[3]
		{
			new FeatureMedals("neg"),
			new FeatureMedals("neg"),
			new FeatureMedals("neg")
		}, -1, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: true, canBeExchanged: true, mergeable: true, basic: true, inscribable: true, soulTransform: true, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: true, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 18, 2004, 25, 0, -1, 1, 5, 100, null, 0, 0, 0, 0, -5, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 0, 0, 0, -10, 0, 0, 0, -10, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(22, 110, 111, hidden: false, ECharacterFeatureType.Bad, 112, 113, 114, new FeatureMedals[3]
		{
			new FeatureMedals("neg"),
			new FeatureMedals("neg"),
			new FeatureMedals("neg")
		}, -2, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: true, canBeExchanged: true, mergeable: true, basic: true, inscribable: true, soulTransform: true, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: true, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 18, 2004, 5, 0, -1, 1, 0, 100, null, 0, 0, 0, 0, -10, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 0, 0, 0, -20, 0, 0, 0, -20, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(23, 115, 116, hidden: false, ECharacterFeatureType.Bad, 117, 118, 119, new FeatureMedals[3]
		{
			new FeatureMedals("neg"),
			new FeatureMedals("neg"),
			new FeatureMedals("neg")
		}, -3, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: true, canBeExchanged: true, mergeable: true, basic: true, inscribable: true, soulTransform: true, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: true, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 18, 2004, 1, 0, -1, 1, 0, 100, null, 0, 0, 0, 0, -15, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 0, 0, 0, -30, 0, 0, 0, -30, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(24, 120, 121, hidden: false, ECharacterFeatureType.Good, 122, 123, 124, new FeatureMedals[3]
		{
			new FeatureMedals(),
			new FeatureMedals(),
			new FeatureMedals("pos")
		}, 1, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: true, canBeExchanged: true, mergeable: true, basic: true, inscribable: true, soulTransform: true, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: true, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 24, 1005, 25, 0, -1, 0, 15, 100, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 10, 0, 0, 0, 0, 0, 0, 0, 0, 0, 10, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(25, 125, 126, hidden: false, ECharacterFeatureType.Good, 127, 128, 129, new FeatureMedals[3]
		{
			new FeatureMedals(),
			new FeatureMedals(),
			new FeatureMedals("pos", "pos")
		}, 2, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: true, canBeExchanged: true, mergeable: true, basic: true, inscribable: true, soulTransform: true, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: true, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 24, 1005, 5, 0, -1, 0, 10, 100, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 20, 0, 0, 0, 0, 0, 0, 0, 0, 0, 20, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(26, 130, 131, hidden: false, ECharacterFeatureType.Good, 132, 133, 134, new FeatureMedals[3]
		{
			new FeatureMedals(),
			new FeatureMedals(),
			new FeatureMedals("pos", "pos", "pos")
		}, 3, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: true, canBeExchanged: true, mergeable: true, basic: true, inscribable: true, soulTransform: true, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: true, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 24, 1005, 1, 0, -1, 0, 0, 100, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 30, 0, 0, 0, 0, 0, 0, 0, 0, 0, 30, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(27, 135, 136, hidden: false, ECharacterFeatureType.Bad, 137, 138, 139, new FeatureMedals[3]
		{
			new FeatureMedals(),
			new FeatureMedals(),
			new FeatureMedals("neg")
		}, -1, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: true, canBeExchanged: true, mergeable: true, basic: true, inscribable: true, soulTransform: true, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: true, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 24, 2005, 25, 0, -1, 1, 5, 100, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, -10, 0, 0, 0, 0, 0, 0, 0, 0, 0, -10, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(28, 140, 141, hidden: false, ECharacterFeatureType.Bad, 142, 143, 144, new FeatureMedals[3]
		{
			new FeatureMedals(),
			new FeatureMedals(),
			new FeatureMedals("neg", "neg")
		}, -2, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: true, canBeExchanged: true, mergeable: true, basic: true, inscribable: true, soulTransform: true, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: true, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 24, 2005, 5, 0, -1, 1, 0, 100, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, -20, 0, 0, 0, 0, 0, 0, 0, 0, 0, -20, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(29, 145, 146, hidden: false, ECharacterFeatureType.Bad, 147, 148, 149, new FeatureMedals[3]
		{
			new FeatureMedals(),
			new FeatureMedals(),
			new FeatureMedals("neg", "neg", "neg")
		}, -3, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: true, canBeExchanged: true, mergeable: true, basic: true, inscribable: true, soulTransform: true, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: true, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 24, 2005, 1, 0, -1, 1, 0, 100, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, -30, 0, 0, 0, 0, 0, 0, 0, 0, 0, -30, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(30, 150, 151, hidden: false, ECharacterFeatureType.Good, 152, 153, 154, new FeatureMedals[3]
		{
			new FeatureMedals(),
			new FeatureMedals("pos"),
			new FeatureMedals()
		}, 1, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: true, canBeExchanged: true, mergeable: true, basic: true, inscribable: true, soulTransform: true, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: true, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 30, 1006, 25, 0, -1, 0, 15, 100, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 10, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 10, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(31, 155, 156, hidden: false, ECharacterFeatureType.Good, 157, 158, 159, new FeatureMedals[3]
		{
			new FeatureMedals("pos"),
			new FeatureMedals("pos"),
			new FeatureMedals()
		}, 2, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: true, canBeExchanged: true, mergeable: true, basic: true, inscribable: true, soulTransform: true, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: true, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 30, 1006, 5, 0, -1, 0, 10, 100, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 20, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 20, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(32, 160, 161, hidden: false, ECharacterFeatureType.Good, 162, 163, 164, new FeatureMedals[3]
		{
			new FeatureMedals("pos"),
			new FeatureMedals("pos", "pos"),
			new FeatureMedals()
		}, 3, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: true, canBeExchanged: true, mergeable: true, basic: true, inscribable: true, soulTransform: true, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: true, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 30, 1006, 1, 0, -1, 0, 0, 100, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 30, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 30, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(33, 165, 166, hidden: false, ECharacterFeatureType.Bad, 167, 168, 169, new FeatureMedals[3]
		{
			new FeatureMedals(),
			new FeatureMedals("neg"),
			new FeatureMedals()
		}, -1, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: true, canBeExchanged: true, mergeable: true, basic: true, inscribable: true, soulTransform: true, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: true, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 30, 2006, 25, 0, -1, 1, 5, 100, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], -10, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -10, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(34, 170, 171, hidden: false, ECharacterFeatureType.Bad, 172, 173, 174, new FeatureMedals[3]
		{
			new FeatureMedals("neg"),
			new FeatureMedals("neg"),
			new FeatureMedals()
		}, -2, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: true, canBeExchanged: true, mergeable: true, basic: true, inscribable: true, soulTransform: true, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: true, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 30, 2006, 5, 0, -1, 1, 0, 100, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], -20, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -20, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(35, 175, 176, hidden: false, ECharacterFeatureType.Bad, 177, 178, 179, new FeatureMedals[3]
		{
			new FeatureMedals("neg"),
			new FeatureMedals("neg", "neg"),
			new FeatureMedals()
		}, -3, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: true, canBeExchanged: true, mergeable: true, basic: true, inscribable: true, soulTransform: true, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: true, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 30, 2006, 1, 0, -1, 1, 0, 100, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], -30, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -30, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(36, 180, 181, hidden: false, ECharacterFeatureType.Good, 182, 183, 184, new FeatureMedals[3]
		{
			new FeatureMedals("pos"),
			new FeatureMedals("pos"),
			new FeatureMedals("pos")
		}, 1, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: true, canBeExchanged: true, mergeable: true, basic: true, inscribable: true, soulTransform: true, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: true, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 36, 1007, 25, 0, -1, 0, 15, 100, null, 0, 0, 0, 0, 5, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 0, 0, 10, 0, 5, 5, 5, 5, 5, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(37, 185, 186, hidden: false, ECharacterFeatureType.Good, 187, 188, 189, new FeatureMedals[3]
		{
			new FeatureMedals("pos"),
			new FeatureMedals("pos"),
			new FeatureMedals("pos")
		}, 2, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: true, canBeExchanged: true, mergeable: true, basic: true, inscribable: true, soulTransform: true, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: true, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 36, 1007, 5, 0, -1, 0, 10, 100, null, 0, 0, 0, 0, 10, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 0, 0, 20, 0, 10, 10, 10, 10, 10, 10, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(38, 190, 191, hidden: false, ECharacterFeatureType.Good, 192, 193, 194, new FeatureMedals[3]
		{
			new FeatureMedals("pos"),
			new FeatureMedals("pos"),
			new FeatureMedals("pos")
		}, 3, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: true, canBeExchanged: true, mergeable: true, basic: true, inscribable: true, soulTransform: true, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: true, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 36, 1007, 1, 0, -1, 0, 0, 100, null, 0, 0, 0, 0, 15, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 0, 0, 30, 0, 15, 15, 15, 15, 15, 15, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(39, 195, 196, hidden: false, ECharacterFeatureType.Bad, 197, 198, 199, new FeatureMedals[3]
		{
			new FeatureMedals("neg"),
			new FeatureMedals("neg"),
			new FeatureMedals("neg")
		}, -1, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: true, canBeExchanged: true, mergeable: true, basic: true, inscribable: true, soulTransform: true, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: true, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 36, 2007, 25, 0, -1, 1, 5, 100, null, 0, 0, 0, 0, -5, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 0, 0, -10, 0, -5, -5, -5, -5, -5, -5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(40, 200, 201, hidden: false, ECharacterFeatureType.Bad, 202, 203, 204, new FeatureMedals[3]
		{
			new FeatureMedals("neg"),
			new FeatureMedals("neg"),
			new FeatureMedals("neg")
		}, -2, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: true, canBeExchanged: true, mergeable: true, basic: true, inscribable: true, soulTransform: true, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: true, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 36, 2007, 5, 0, -1, 1, 0, 100, null, 0, 0, 0, 0, -10, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 0, 0, -20, 0, -10, -10, -10, -10, -10, -10, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(41, 205, 206, hidden: false, ECharacterFeatureType.Bad, 207, 208, 209, new FeatureMedals[3]
		{
			new FeatureMedals("neg"),
			new FeatureMedals("neg"),
			new FeatureMedals("neg")
		}, -3, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: true, canBeExchanged: true, mergeable: true, basic: true, inscribable: true, soulTransform: true, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: true, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 36, 2007, 1, 0, -1, 1, 0, 100, null, 0, 0, 0, 0, -15, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 0, 0, -30, 0, -15, -15, -15, -15, -15, -15, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(42, 210, 211, hidden: false, ECharacterFeatureType.Good, 212, 213, 214, new FeatureMedals[3]
		{
			new FeatureMedals(),
			new FeatureMedals(),
			new FeatureMedals("pos")
		}, 1, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: true, canBeExchanged: true, mergeable: true, basic: true, inscribable: true, soulTransform: true, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: true, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 42, 1008, 25, 0, -1, 0, 15, 100, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 0, 0, 0, 20, 0, 0, 0, 0, 0, 0, 0, 0, 50, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(43, 215, 216, hidden: false, ECharacterFeatureType.Good, 217, 218, 219, new FeatureMedals[3]
		{
			new FeatureMedals(),
			new FeatureMedals(),
			new FeatureMedals("pos", "pos")
		}, 2, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: true, canBeExchanged: true, mergeable: true, basic: true, inscribable: true, soulTransform: true, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: true, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 42, 1008, 5, 0, -1, 0, 10, 100, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 0, 0, 0, 35, 0, 0, 0, 0, 0, 0, 0, 0, 100, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(44, 220, 221, hidden: false, ECharacterFeatureType.Good, 222, 223, 224, new FeatureMedals[3]
		{
			new FeatureMedals(),
			new FeatureMedals(),
			new FeatureMedals("pos", "pos", "pos")
		}, 3, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: true, canBeExchanged: true, mergeable: true, basic: true, inscribable: true, soulTransform: true, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: true, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 42, 1008, 1, 0, -1, 0, 0, 100, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 0, 0, 0, 50, 0, 0, 0, 0, 0, 0, 0, 0, 150, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(45, 225, 226, hidden: false, ECharacterFeatureType.Bad, 227, 228, 229, new FeatureMedals[3]
		{
			new FeatureMedals(),
			new FeatureMedals(),
			new FeatureMedals("neg")
		}, -1, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: true, canBeExchanged: true, mergeable: true, basic: true, inscribable: true, soulTransform: true, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: true, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 42, 2008, 25, 0, -1, 1, 5, 100, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 0, 0, 0, -20, 0, 0, 0, 0, 0, 0, 0, 0, -50, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(46, 230, 231, hidden: false, ECharacterFeatureType.Bad, 232, 233, 234, new FeatureMedals[3]
		{
			new FeatureMedals(),
			new FeatureMedals(),
			new FeatureMedals("neg", "neg")
		}, -2, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: true, canBeExchanged: true, mergeable: true, basic: true, inscribable: true, soulTransform: true, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: true, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 42, 2008, 5, 0, -1, 1, 0, 100, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 0, 0, 0, -35, 0, 0, 0, 0, 0, 0, 0, 0, -100, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(47, 235, 236, hidden: false, ECharacterFeatureType.Bad, 237, 238, 239, new FeatureMedals[3]
		{
			new FeatureMedals(),
			new FeatureMedals(),
			new FeatureMedals("neg", "neg", "neg")
		}, -3, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: true, canBeExchanged: true, mergeable: true, basic: true, inscribable: true, soulTransform: true, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: true, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 42, 2008, 1, 0, -1, 1, 0, 100, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 0, 0, 0, -50, 0, 0, 0, 0, 0, 0, 0, 0, -150, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(48, 240, 241, hidden: false, ECharacterFeatureType.Good, 242, 243, 244, new FeatureMedals[3]
		{
			new FeatureMedals(),
			new FeatureMedals(),
			new FeatureMedals("pos")
		}, 1, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: true, canBeExchanged: true, mergeable: true, basic: true, inscribable: true, soulTransform: true, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: true, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 48, 1009, 25, 0, -1, 0, 15, 100, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 0, 0, 20, 0, 0, 0, 0, 0, 0, 0, 0, 0, 50, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(49, 245, 246, hidden: false, ECharacterFeatureType.Good, 247, 248, 249, new FeatureMedals[3]
		{
			new FeatureMedals(),
			new FeatureMedals(),
			new FeatureMedals("pos", "pos")
		}, 2, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: true, canBeExchanged: true, mergeable: true, basic: true, inscribable: true, soulTransform: true, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: true, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 48, 1009, 5, 0, -1, 0, 10, 100, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 0, 0, 35, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(50, 250, 251, hidden: false, ECharacterFeatureType.Good, 252, 253, 254, new FeatureMedals[3]
		{
			new FeatureMedals(),
			new FeatureMedals(),
			new FeatureMedals("pos", "pos", "pos")
		}, 3, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: true, canBeExchanged: true, mergeable: true, basic: true, inscribable: true, soulTransform: true, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: true, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 48, 1009, 1, 0, -1, 0, 0, 100, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 0, 0, 50, 0, 0, 0, 0, 0, 0, 0, 0, 0, 150, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(51, 255, 256, hidden: false, ECharacterFeatureType.Bad, 257, 258, 259, new FeatureMedals[3]
		{
			new FeatureMedals(),
			new FeatureMedals(),
			new FeatureMedals("neg")
		}, -1, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: true, canBeExchanged: true, mergeable: true, basic: true, inscribable: true, soulTransform: true, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: true, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 48, 2009, 25, 0, -1, 1, 5, 100, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 0, 0, -20, 0, 0, 0, 0, 0, 0, 0, 0, 0, -50, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(52, 260, 261, hidden: false, ECharacterFeatureType.Bad, 262, 263, 264, new FeatureMedals[3]
		{
			new FeatureMedals(),
			new FeatureMedals(),
			new FeatureMedals("neg", "neg")
		}, -2, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: true, canBeExchanged: true, mergeable: true, basic: true, inscribable: true, soulTransform: true, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: true, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 48, 2009, 5, 0, -1, 1, 0, 100, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 0, 0, -35, 0, 0, 0, 0, 0, 0, 0, 0, 0, -100, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(53, 265, 266, hidden: false, ECharacterFeatureType.Bad, 267, 268, 269, new FeatureMedals[3]
		{
			new FeatureMedals(),
			new FeatureMedals(),
			new FeatureMedals("neg", "neg", "neg")
		}, -3, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: true, canBeExchanged: true, mergeable: true, basic: true, inscribable: true, soulTransform: true, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: true, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 48, 2009, 1, 0, -1, 1, 0, 100, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 0, 0, -50, 0, 0, 0, 0, 0, 0, 0, 0, 0, -150, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(54, 270, 271, hidden: false, ECharacterFeatureType.Good, 272, 273, 274, new FeatureMedals[3]
		{
			new FeatureMedals(),
			new FeatureMedals("pos"),
			new FeatureMedals()
		}, 1, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: true, canBeExchanged: true, mergeable: true, basic: true, inscribable: true, soulTransform: true, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: true, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 54, 1010, 25, 0, -1, 0, 15, 100, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 0, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 10, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(55, 275, 276, hidden: false, ECharacterFeatureType.Good, 277, 278, 279, new FeatureMedals[3]
		{
			new FeatureMedals(),
			new FeatureMedals("pos", "pos"),
			new FeatureMedals()
		}, 2, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: true, canBeExchanged: true, mergeable: true, basic: true, inscribable: true, soulTransform: true, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: true, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 54, 1010, 5, 0, -1, 0, 10, 100, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 0, 10, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 15, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(56, 280, 281, hidden: false, ECharacterFeatureType.Good, 282, 283, 284, new FeatureMedals[3]
		{
			new FeatureMedals(),
			new FeatureMedals("pos", "pos", "pos"),
			new FeatureMedals()
		}, 3, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: true, canBeExchanged: true, mergeable: true, basic: true, inscribable: true, soulTransform: true, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: true, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 54, 1010, 1, 0, -1, 0, 0, 100, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 0, 15, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 20, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(57, 285, 286, hidden: false, ECharacterFeatureType.Bad, 287, 288, 289, new FeatureMedals[3]
		{
			new FeatureMedals(),
			new FeatureMedals("neg"),
			new FeatureMedals()
		}, -1, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: true, canBeExchanged: true, mergeable: true, basic: true, inscribable: true, soulTransform: true, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: true, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 54, 2010, 25, 0, -1, 1, 5, 100, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 0, -5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -10, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(58, 290, 291, hidden: false, ECharacterFeatureType.Bad, 292, 293, 294, new FeatureMedals[3]
		{
			new FeatureMedals(),
			new FeatureMedals("neg", "neg"),
			new FeatureMedals()
		}, -2, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: true, canBeExchanged: true, mergeable: true, basic: true, inscribable: true, soulTransform: true, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: true, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 54, 2010, 5, 0, -1, 1, 0, 100, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 0, -10, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -15, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(59, 295, 296, hidden: false, ECharacterFeatureType.Bad, 297, 298, 299, new FeatureMedals[3]
		{
			new FeatureMedals(),
			new FeatureMedals("neg", "neg", "neg"),
			new FeatureMedals()
		}, -3, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: true, canBeExchanged: true, mergeable: true, basic: true, inscribable: true, soulTransform: true, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: true, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 54, 2010, 1, 0, -1, 1, 0, 100, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 0, -15, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -20, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
	}

	private void CreateItems1()
	{
		_dataArray.Add(new CharacterFeatureItem(60, 300, 301, hidden: false, ECharacterFeatureType.Good, 302, 303, 304, new FeatureMedals[3]
		{
			new FeatureMedals(),
			new FeatureMedals("pos"),
			new FeatureMedals()
		}, 1, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: true, canBeExchanged: true, mergeable: true, basic: true, inscribable: true, soulTransform: true, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: true, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 60, 1011, 25, 0, -1, 0, 15, 100, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 10, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(61, 305, 306, hidden: false, ECharacterFeatureType.Good, 307, 308, 309, new FeatureMedals[3]
		{
			new FeatureMedals(),
			new FeatureMedals("pos"),
			new FeatureMedals()
		}, 2, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: true, canBeExchanged: true, mergeable: true, basic: true, inscribable: true, soulTransform: true, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: true, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 60, 1011, 5, 0, -1, 0, 10, 100, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 10, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 15, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(62, 310, 311, hidden: false, ECharacterFeatureType.Good, 312, 313, 314, new FeatureMedals[3]
		{
			new FeatureMedals(),
			new FeatureMedals("pos"),
			new FeatureMedals()
		}, 3, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: true, canBeExchanged: true, mergeable: true, basic: true, inscribable: true, soulTransform: true, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: true, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 60, 1011, 1, 0, -1, 0, 0, 100, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 15, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 20, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(63, 315, 316, hidden: false, ECharacterFeatureType.Bad, 317, 318, 319, new FeatureMedals[3]
		{
			new FeatureMedals(),
			new FeatureMedals("neg"),
			new FeatureMedals()
		}, -1, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: true, canBeExchanged: true, mergeable: true, basic: true, inscribable: true, soulTransform: true, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: true, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 60, 2011, 25, 0, -1, 1, 5, 100, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], -5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -10, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(64, 320, 321, hidden: false, ECharacterFeatureType.Bad, 322, 323, 324, new FeatureMedals[3]
		{
			new FeatureMedals(),
			new FeatureMedals("neg"),
			new FeatureMedals()
		}, -2, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: true, canBeExchanged: true, mergeable: true, basic: true, inscribable: true, soulTransform: true, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: true, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 60, 2011, 5, 0, -1, 1, 0, 100, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], -10, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -15, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(65, 325, 326, hidden: false, ECharacterFeatureType.Bad, 327, 328, 329, new FeatureMedals[3]
		{
			new FeatureMedals(),
			new FeatureMedals("neg"),
			new FeatureMedals()
		}, -3, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: true, canBeExchanged: true, mergeable: true, basic: true, inscribable: true, soulTransform: true, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: true, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 60, 2011, 1, 0, -1, 1, 0, 100, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], -15, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -20, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(66, 330, 331, hidden: false, ECharacterFeatureType.Good, 332, 333, 334, new FeatureMedals[3]
		{
			new FeatureMedals("pos"),
			new FeatureMedals("pos"),
			new FeatureMedals("pos")
		}, 1, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: true, canBeExchanged: true, mergeable: true, basic: true, inscribable: true, soulTransform: true, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: true, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 66, 1012, 25, 0, -1, 0, 15, 100, null, 0, 10, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 0, 0, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 20, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(67, 335, 336, hidden: false, ECharacterFeatureType.Good, 337, 338, 339, new FeatureMedals[3]
		{
			new FeatureMedals("pos"),
			new FeatureMedals("pos"),
			new FeatureMedals("pos")
		}, 2, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: true, canBeExchanged: true, mergeable: true, basic: true, inscribable: true, soulTransform: true, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: true, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 66, 1012, 5, 0, -1, 0, 10, 100, null, 0, 20, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 0, 0, 10, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 40, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(68, 340, 341, hidden: false, ECharacterFeatureType.Good, 342, 343, 344, new FeatureMedals[3]
		{
			new FeatureMedals("pos"),
			new FeatureMedals("pos"),
			new FeatureMedals("pos")
		}, 3, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: true, canBeExchanged: true, mergeable: true, basic: true, inscribable: true, soulTransform: true, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: true, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 66, 1012, 1, 0, -1, 0, 0, 100, null, 0, 30, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 0, 0, 15, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 60, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(69, 345, 346, hidden: false, ECharacterFeatureType.Bad, 347, 348, 349, new FeatureMedals[3]
		{
			new FeatureMedals("neg"),
			new FeatureMedals("neg"),
			new FeatureMedals("neg")
		}, -1, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: true, canBeExchanged: true, mergeable: true, basic: true, inscribable: true, soulTransform: true, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: true, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 66, 2012, 25, 0, -1, 1, 5, 100, null, 20, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 0, 0, -5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -20, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(70, 350, 351, hidden: false, ECharacterFeatureType.Bad, 352, 353, 354, new FeatureMedals[3]
		{
			new FeatureMedals("neg"),
			new FeatureMedals("neg"),
			new FeatureMedals("neg")
		}, -2, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: true, canBeExchanged: true, mergeable: true, basic: true, inscribable: true, soulTransform: true, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: true, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 66, 2012, 5, 0, -1, 1, 0, 100, null, 40, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 0, 0, -10, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -40, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(71, 355, 356, hidden: false, ECharacterFeatureType.Bad, 357, 358, 359, new FeatureMedals[3]
		{
			new FeatureMedals("neg"),
			new FeatureMedals("neg"),
			new FeatureMedals("neg")
		}, -3, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: true, canBeExchanged: true, mergeable: true, basic: true, inscribable: true, soulTransform: true, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: true, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 66, 2012, 1, 0, -1, 1, 0, 100, null, 60, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 0, 0, -15, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -60, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(72, 360, 361, hidden: false, ECharacterFeatureType.Good, 362, 363, 364, new FeatureMedals[3]
		{
			new FeatureMedals(),
			new FeatureMedals("pos"),
			new FeatureMedals()
		}, 1, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: true, canBeExchanged: true, mergeable: true, basic: true, inscribable: true, soulTransform: true, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: true, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 72, 1013, 25, 0, -1, 0, 15, 100, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 0, 0, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 10, 0, 0, 0, 0, 0, 10, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(73, 365, 366, hidden: false, ECharacterFeatureType.Good, 367, 368, 369, new FeatureMedals[3]
		{
			new FeatureMedals(),
			new FeatureMedals("pos", "pos"),
			new FeatureMedals()
		}, 2, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: true, canBeExchanged: true, mergeable: true, basic: true, inscribable: true, soulTransform: true, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: true, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 72, 1013, 5, 0, -1, 0, 10, 100, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 0, 0, 10, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 15, 0, 0, 0, 0, 0, 15, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(74, 370, 371, hidden: false, ECharacterFeatureType.Good, 372, 373, 374, new FeatureMedals[3]
		{
			new FeatureMedals(),
			new FeatureMedals("pos", "pos", "pos"),
			new FeatureMedals()
		}, 3, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: true, canBeExchanged: true, mergeable: true, basic: true, inscribable: true, soulTransform: true, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: true, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 72, 1013, 1, 0, -1, 0, 0, 100, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 0, 0, 15, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 20, 0, 0, 0, 0, 0, 20, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(75, 375, 376, hidden: false, ECharacterFeatureType.Bad, 377, 378, 379, new FeatureMedals[3]
		{
			new FeatureMedals(),
			new FeatureMedals("neg"),
			new FeatureMedals()
		}, -1, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: true, canBeExchanged: true, mergeable: true, basic: true, inscribable: true, soulTransform: true, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: true, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 72, 2013, 25, 0, -1, 1, 5, 100, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 0, 0, -5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -10, 0, 0, 0, 0, 0, -10, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(76, 380, 381, hidden: false, ECharacterFeatureType.Bad, 382, 383, 384, new FeatureMedals[3]
		{
			new FeatureMedals(),
			new FeatureMedals("neg", "neg"),
			new FeatureMedals()
		}, -2, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: true, canBeExchanged: true, mergeable: true, basic: true, inscribable: true, soulTransform: true, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: true, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 72, 2013, 5, 0, -1, 1, 0, 100, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 0, 0, -10, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -15, 0, 0, 0, 0, 0, -15, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(77, 385, 386, hidden: false, ECharacterFeatureType.Bad, 387, 388, 389, new FeatureMedals[3]
		{
			new FeatureMedals(),
			new FeatureMedals("neg", "neg", "neg"),
			new FeatureMedals()
		}, -3, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: true, canBeExchanged: true, mergeable: true, basic: true, inscribable: true, soulTransform: true, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: true, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 72, 2013, 1, 0, -1, 1, 0, 100, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 0, 0, -15, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -20, 0, 0, 0, 0, 0, -20, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(78, 390, 391, hidden: false, ECharacterFeatureType.Good, 392, 393, 394, new FeatureMedals[3]
		{
			new FeatureMedals(),
			new FeatureMedals("pos"),
			new FeatureMedals()
		}, 1, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: true, canBeExchanged: true, mergeable: true, basic: true, inscribable: true, soulTransform: true, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: true, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 78, 1014, 25, 0, -1, 0, 15, 100, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 20, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(79, 395, 396, hidden: false, ECharacterFeatureType.Good, 397, 398, 399, new FeatureMedals[3]
		{
			new FeatureMedals(),
			new FeatureMedals("pos", "pos"),
			new FeatureMedals()
		}, 2, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: true, canBeExchanged: true, mergeable: true, basic: true, inscribable: true, soulTransform: true, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: true, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 78, 1014, 5, 0, -1, 0, 10, 100, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 10, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 40, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(80, 400, 401, hidden: false, ECharacterFeatureType.Good, 402, 403, 404, new FeatureMedals[3]
		{
			new FeatureMedals(),
			new FeatureMedals("pos", "pos", "pos"),
			new FeatureMedals()
		}, 3, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: true, canBeExchanged: true, mergeable: true, basic: true, inscribable: true, soulTransform: true, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: true, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 78, 1014, 1, 0, -1, 0, 0, 100, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 15, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 60, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(81, 405, 406, hidden: false, ECharacterFeatureType.Bad, 407, 408, 409, new FeatureMedals[3]
		{
			new FeatureMedals(),
			new FeatureMedals("neg"),
			new FeatureMedals()
		}, -1, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: true, canBeExchanged: true, mergeable: true, basic: true, inscribable: true, soulTransform: true, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: true, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 78, 2014, 25, 0, -1, 1, 5, 100, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, -5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -20, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(82, 410, 411, hidden: false, ECharacterFeatureType.Bad, 412, 413, 414, new FeatureMedals[3]
		{
			new FeatureMedals(),
			new FeatureMedals("neg", "neg"),
			new FeatureMedals()
		}, -2, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: true, canBeExchanged: true, mergeable: true, basic: true, inscribable: true, soulTransform: true, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: true, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 78, 2014, 5, 0, -1, 1, 0, 100, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, -10, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -40, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(83, 415, 416, hidden: false, ECharacterFeatureType.Bad, 417, 418, 419, new FeatureMedals[3]
		{
			new FeatureMedals(),
			new FeatureMedals("neg", "neg", "neg"),
			new FeatureMedals()
		}, -3, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: true, canBeExchanged: true, mergeable: true, basic: true, inscribable: true, soulTransform: true, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: true, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 78, 2014, 1, 0, -1, 1, 0, 100, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, -15, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -60, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(84, 420, 421, hidden: false, ECharacterFeatureType.Good, 422, 423, 424, new FeatureMedals[3]
		{
			new FeatureMedals(),
			new FeatureMedals(),
			new FeatureMedals("pos")
		}, 1, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: true, canBeExchanged: true, mergeable: true, basic: true, inscribable: true, soulTransform: true, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: true, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 84, 1015, 25, 0, -1, 0, 15, 100, null, 0, 10, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 0, 0, 0, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 20, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(85, 425, 426, hidden: false, ECharacterFeatureType.Good, 427, 428, 429, new FeatureMedals[3]
		{
			new FeatureMedals(),
			new FeatureMedals(),
			new FeatureMedals("pos")
		}, 2, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: true, canBeExchanged: true, mergeable: true, basic: true, inscribable: true, soulTransform: true, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: true, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 84, 1015, 5, 0, -1, 0, 10, 100, null, 0, 20, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 0, 0, 0, 10, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 40, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(86, 430, 431, hidden: false, ECharacterFeatureType.Good, 432, 433, 434, new FeatureMedals[3]
		{
			new FeatureMedals(),
			new FeatureMedals(),
			new FeatureMedals("pos")
		}, 3, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: true, canBeExchanged: true, mergeable: true, basic: true, inscribable: true, soulTransform: true, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: true, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 84, 1015, 1, 0, -1, 0, 0, 100, null, 0, 30, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 0, 0, 0, 15, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 60, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(87, 435, 436, hidden: false, ECharacterFeatureType.Bad, 437, 438, 439, new FeatureMedals[3]
		{
			new FeatureMedals(),
			new FeatureMedals(),
			new FeatureMedals("neg")
		}, -1, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: true, canBeExchanged: true, mergeable: true, basic: true, inscribable: true, soulTransform: true, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: true, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 84, 2015, 25, 0, -1, 1, 5, 100, null, 20, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 0, 0, 0, -5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -20, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(88, 440, 441, hidden: false, ECharacterFeatureType.Bad, 442, 443, 444, new FeatureMedals[3]
		{
			new FeatureMedals(),
			new FeatureMedals(),
			new FeatureMedals("neg")
		}, -2, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: true, canBeExchanged: true, mergeable: true, basic: true, inscribable: true, soulTransform: true, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: true, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 84, 2015, 5, 0, -1, 1, 0, 100, null, 40, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 0, 0, 0, -10, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -40, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(89, 445, 446, hidden: false, ECharacterFeatureType.Bad, 447, 448, 449, new FeatureMedals[3]
		{
			new FeatureMedals(),
			new FeatureMedals(),
			new FeatureMedals("neg")
		}, -3, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: true, canBeExchanged: true, mergeable: true, basic: true, inscribable: true, soulTransform: true, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: true, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 84, 2015, 1, 0, -1, 1, 0, 100, null, 60, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 0, 0, 0, -15, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -60, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(90, 450, 451, hidden: false, ECharacterFeatureType.Good, 452, 453, 454, new FeatureMedals[3]
		{
			new FeatureMedals(),
			new FeatureMedals("pos"),
			new FeatureMedals()
		}, 1, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: true, canBeExchanged: true, mergeable: true, basic: true, inscribable: true, soulTransform: true, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: true, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 90, 1016, 25, 0, -1, 0, 15, 100, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 0, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 10, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(91, 455, 456, hidden: false, ECharacterFeatureType.Good, 457, 458, 459, new FeatureMedals[3]
		{
			new FeatureMedals(),
			new FeatureMedals("pos"),
			new FeatureMedals("pos")
		}, 2, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: true, canBeExchanged: true, mergeable: true, basic: true, inscribable: true, soulTransform: true, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: true, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 90, 1016, 5, 0, -1, 0, 10, 100, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 0, 10, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 15, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(92, 460, 461, hidden: false, ECharacterFeatureType.Good, 462, 463, 464, new FeatureMedals[3]
		{
			new FeatureMedals(),
			new FeatureMedals("pos", "pos"),
			new FeatureMedals("pos")
		}, 3, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: true, canBeExchanged: true, mergeable: true, basic: true, inscribable: true, soulTransform: true, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: true, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 90, 1016, 1, 0, -1, 0, 0, 100, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 0, 15, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 20, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(93, 465, 466, hidden: false, ECharacterFeatureType.Bad, 467, 468, 469, new FeatureMedals[3]
		{
			new FeatureMedals(),
			new FeatureMedals("neg"),
			new FeatureMedals()
		}, -1, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: true, canBeExchanged: true, mergeable: true, basic: true, inscribable: true, soulTransform: true, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: true, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 90, 2016, 25, 0, -1, 1, 5, 100, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 0, -5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -10, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(94, 470, 471, hidden: false, ECharacterFeatureType.Bad, 472, 473, 474, new FeatureMedals[3]
		{
			new FeatureMedals(),
			new FeatureMedals("neg"),
			new FeatureMedals("neg")
		}, -2, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: true, canBeExchanged: true, mergeable: true, basic: true, inscribable: true, soulTransform: true, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: true, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 90, 2016, 5, 0, -1, 1, 0, 100, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 0, -10, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -15, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(95, 475, 476, hidden: false, ECharacterFeatureType.Bad, 477, 478, 479, new FeatureMedals[3]
		{
			new FeatureMedals(),
			new FeatureMedals("neg", "neg"),
			new FeatureMedals("neg")
		}, -3, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: true, canBeExchanged: true, mergeable: true, basic: true, inscribable: true, soulTransform: true, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: true, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 90, 2016, 1, 0, -1, 1, 0, 100, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 0, -15, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -20, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(96, 480, 481, hidden: false, ECharacterFeatureType.Good, 482, 483, 484, new FeatureMedals[3]
		{
			new FeatureMedals(),
			new FeatureMedals(),
			new FeatureMedals("pos")
		}, 1, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: true, canBeExchanged: true, mergeable: true, basic: true, inscribable: true, soulTransform: true, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: true, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 96, 1017, 25, 0, -1, 0, 15, 100, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 0, 0, 0, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 10, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(97, 485, 486, hidden: false, ECharacterFeatureType.Good, 487, 488, 489, new FeatureMedals[3]
		{
			new FeatureMedals(),
			new FeatureMedals(),
			new FeatureMedals("pos", "pos")
		}, 2, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: true, canBeExchanged: true, mergeable: true, basic: true, inscribable: true, soulTransform: true, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: true, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 96, 1017, 5, 0, -1, 0, 10, 100, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 0, 0, 0, 10, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 15, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(98, 490, 491, hidden: false, ECharacterFeatureType.Good, 492, 493, 494, new FeatureMedals[3]
		{
			new FeatureMedals(),
			new FeatureMedals(),
			new FeatureMedals("pos", "pos", "pos")
		}, 3, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: true, canBeExchanged: true, mergeable: true, basic: true, inscribable: true, soulTransform: true, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: true, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 96, 1017, 1, 0, -1, 0, 0, 100, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 0, 0, 0, 15, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 20, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(99, 495, 496, hidden: false, ECharacterFeatureType.Bad, 497, 498, 499, new FeatureMedals[3]
		{
			new FeatureMedals(),
			new FeatureMedals(),
			new FeatureMedals("neg")
		}, -1, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: true, canBeExchanged: true, mergeable: true, basic: true, inscribable: true, soulTransform: true, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: true, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 96, 2017, 25, 0, -1, 1, 5, 100, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 0, 0, 0, -5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -10, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(100, 500, 501, hidden: false, ECharacterFeatureType.Bad, 502, 503, 504, new FeatureMedals[3]
		{
			new FeatureMedals(),
			new FeatureMedals(),
			new FeatureMedals("neg", "neg")
		}, -2, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: true, canBeExchanged: true, mergeable: true, basic: true, inscribable: true, soulTransform: true, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: true, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 96, 2017, 5, 0, -1, 1, 0, 100, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 0, 0, 0, -10, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -15, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(101, 505, 506, hidden: false, ECharacterFeatureType.Bad, 507, 508, 509, new FeatureMedals[3]
		{
			new FeatureMedals(),
			new FeatureMedals(),
			new FeatureMedals("neg", "neg", "neg")
		}, -3, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: true, canBeExchanged: true, mergeable: true, basic: true, inscribable: true, soulTransform: true, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: true, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 96, 2017, 1, 0, -1, 1, 0, 100, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 0, 0, 0, -15, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -20, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(102, 510, 511, hidden: false, ECharacterFeatureType.Good, 512, 513, 514, new FeatureMedals[3]
		{
			new FeatureMedals(),
			new FeatureMedals(),
			new FeatureMedals("pos")
		}, 1, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: true, canBeExchanged: true, mergeable: true, basic: true, inscribable: true, soulTransform: true, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: true, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 102, 1018, 25, 0, -1, 0, 15, 100, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 10, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(103, 515, 516, hidden: false, ECharacterFeatureType.Good, 517, 518, 519, new FeatureMedals[3]
		{
			new FeatureMedals("pos"),
			new FeatureMedals(),
			new FeatureMedals("pos")
		}, 2, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: true, canBeExchanged: true, mergeable: true, basic: true, inscribable: true, soulTransform: true, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: true, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 102, 1018, 5, 0, -1, 0, 10, 100, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 10, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 15, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(104, 520, 521, hidden: false, ECharacterFeatureType.Good, 522, 523, 524, new FeatureMedals[3]
		{
			new FeatureMedals("pos"),
			new FeatureMedals(),
			new FeatureMedals("pos", "pos")
		}, 3, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: true, canBeExchanged: true, mergeable: true, basic: true, inscribable: true, soulTransform: true, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: true, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 102, 1018, 1, 0, -1, 0, 0, 100, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 15, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 20, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(105, 525, 526, hidden: false, ECharacterFeatureType.Bad, 527, 528, 529, new FeatureMedals[3]
		{
			new FeatureMedals(),
			new FeatureMedals(),
			new FeatureMedals("neg")
		}, -1, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: true, canBeExchanged: true, mergeable: true, basic: true, inscribable: true, soulTransform: true, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: true, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 102, 2018, 25, 0, -1, 1, 5, 100, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, -5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -10, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(106, 530, 531, hidden: false, ECharacterFeatureType.Bad, 532, 533, 534, new FeatureMedals[3]
		{
			new FeatureMedals("neg"),
			new FeatureMedals(),
			new FeatureMedals("neg")
		}, -2, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: true, canBeExchanged: true, mergeable: true, basic: true, inscribable: true, soulTransform: true, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: true, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 102, 2018, 5, 0, -1, 1, 0, 100, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, -10, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -15, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(107, 535, 536, hidden: false, ECharacterFeatureType.Bad, 537, 538, 539, new FeatureMedals[3]
		{
			new FeatureMedals("neg"),
			new FeatureMedals(),
			new FeatureMedals("neg", "neg")
		}, -3, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: true, canBeExchanged: true, mergeable: true, basic: true, inscribable: true, soulTransform: true, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: true, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 102, 2018, 1, 0, -1, 1, 0, 100, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, -15, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -20, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(108, 540, 541, hidden: false, ECharacterFeatureType.Good, 542, 543, 544, new FeatureMedals[3]
		{
			new FeatureMedals("pos"),
			new FeatureMedals(),
			new FeatureMedals()
		}, 1, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: true, canBeExchanged: true, mergeable: true, basic: true, inscribable: true, soulTransform: true, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: true, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 108, 1019, 25, 0, -1, 0, 15, 100, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 10, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(109, 545, 546, hidden: false, ECharacterFeatureType.Good, 547, 548, 549, new FeatureMedals[3]
		{
			new FeatureMedals("pos", "pos"),
			new FeatureMedals(),
			new FeatureMedals()
		}, 2, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: true, canBeExchanged: true, mergeable: true, basic: true, inscribable: true, soulTransform: true, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: true, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 108, 1019, 5, 0, -1, 0, 10, 100, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 10, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 15, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(110, 550, 551, hidden: false, ECharacterFeatureType.Good, 552, 553, 554, new FeatureMedals[3]
		{
			new FeatureMedals("pos", "pos", "pos"),
			new FeatureMedals(),
			new FeatureMedals()
		}, 3, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: true, canBeExchanged: true, mergeable: true, basic: true, inscribable: true, soulTransform: true, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: true, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 108, 1019, 1, 0, -1, 0, 0, 100, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 15, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 20, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(111, 555, 556, hidden: false, ECharacterFeatureType.Bad, 557, 558, 559, new FeatureMedals[3]
		{
			new FeatureMedals("neg"),
			new FeatureMedals(),
			new FeatureMedals()
		}, -1, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: true, canBeExchanged: true, mergeable: true, basic: true, inscribable: true, soulTransform: true, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: true, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 108, 2019, 25, 0, -1, 1, 5, 100, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, -5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -10, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(112, 560, 561, hidden: false, ECharacterFeatureType.Bad, 562, 563, 564, new FeatureMedals[3]
		{
			new FeatureMedals("neg", "neg"),
			new FeatureMedals(),
			new FeatureMedals()
		}, -2, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: true, canBeExchanged: true, mergeable: true, basic: true, inscribable: true, soulTransform: true, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: true, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 108, 2019, 5, 0, -1, 1, 0, 100, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, -10, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -15, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(113, 565, 566, hidden: false, ECharacterFeatureType.Bad, 567, 568, 569, new FeatureMedals[3]
		{
			new FeatureMedals("neg", "neg", "neg"),
			new FeatureMedals(),
			new FeatureMedals()
		}, -3, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: true, canBeExchanged: true, mergeable: true, basic: true, inscribable: true, soulTransform: true, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: true, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 108, 2019, 1, 0, -1, 1, 0, 100, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, -15, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -20, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(114, 570, 571, hidden: false, ECharacterFeatureType.Good, 572, 573, 574, new FeatureMedals[3]
		{
			new FeatureMedals(),
			new FeatureMedals(),
			new FeatureMedals("pos")
		}, 1, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: true, canBeExchanged: true, mergeable: true, basic: true, inscribable: true, soulTransform: true, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: true, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 114, 1020, 25, 0, -1, 0, 15, 100, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 10, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(115, 575, 576, hidden: false, ECharacterFeatureType.Good, 577, 578, 579, new FeatureMedals[3]
		{
			new FeatureMedals(),
			new FeatureMedals(),
			new FeatureMedals("pos", "pos")
		}, 2, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: true, canBeExchanged: true, mergeable: true, basic: true, inscribable: true, soulTransform: true, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: true, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 114, 1020, 5, 0, -1, 0, 10, 100, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 10, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 15, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(116, 580, 581, hidden: false, ECharacterFeatureType.Good, 582, 583, 584, new FeatureMedals[3]
		{
			new FeatureMedals(),
			new FeatureMedals(),
			new FeatureMedals("pos", "pos", "pos")
		}, 3, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: true, canBeExchanged: true, mergeable: true, basic: true, inscribable: true, soulTransform: true, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: true, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 114, 1020, 1, 0, -1, 0, 0, 100, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 15, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 20, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(117, 585, 586, hidden: false, ECharacterFeatureType.Bad, 587, 588, 589, new FeatureMedals[3]
		{
			new FeatureMedals(),
			new FeatureMedals(),
			new FeatureMedals("neg")
		}, -1, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: true, canBeExchanged: true, mergeable: true, basic: true, inscribable: true, soulTransform: true, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: true, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 114, 2020, 25, 0, -1, 1, 5, 100, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], -5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -10, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(118, 590, 591, hidden: false, ECharacterFeatureType.Bad, 592, 593, 594, new FeatureMedals[3]
		{
			new FeatureMedals(),
			new FeatureMedals(),
			new FeatureMedals("neg", "neg")
		}, -2, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: true, canBeExchanged: true, mergeable: true, basic: true, inscribable: true, soulTransform: true, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: true, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 114, 2020, 5, 0, -1, 1, 0, 100, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], -10, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -15, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(119, 595, 596, hidden: false, ECharacterFeatureType.Bad, 597, 598, 599, new FeatureMedals[3]
		{
			new FeatureMedals(),
			new FeatureMedals(),
			new FeatureMedals("neg", "neg", "neg")
		}, -3, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: true, canBeExchanged: true, mergeable: true, basic: true, inscribable: true, soulTransform: true, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: true, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 114, 2020, 1, 0, -1, 1, 0, 100, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], -15, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -20, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
	}

	private void CreateItems2()
	{
		_dataArray.Add(new CharacterFeatureItem(120, 600, 601, hidden: false, ECharacterFeatureType.Good, 602, 603, 604, new FeatureMedals[3]
		{
			new FeatureMedals("pos"),
			new FeatureMedals(),
			new FeatureMedals()
		}, 1, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: true, canBeExchanged: true, mergeable: true, basic: true, inscribable: true, soulTransform: true, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: true, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 120, 1021, 25, 0, -1, 0, 15, 100, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 10, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(121, 605, 606, hidden: false, ECharacterFeatureType.Good, 607, 608, 609, new FeatureMedals[3]
		{
			new FeatureMedals("pos"),
			new FeatureMedals(),
			new FeatureMedals("pos")
		}, 2, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: true, canBeExchanged: true, mergeable: true, basic: true, inscribable: true, soulTransform: true, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: true, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 120, 1021, 5, 0, -1, 0, 10, 100, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 10, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 15, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(122, 610, 611, hidden: false, ECharacterFeatureType.Good, 612, 613, 614, new FeatureMedals[3]
		{
			new FeatureMedals("pos", "pos"),
			new FeatureMedals(),
			new FeatureMedals("pos")
		}, 3, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: true, canBeExchanged: true, mergeable: true, basic: true, inscribable: true, soulTransform: true, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: true, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 120, 1021, 1, 0, -1, 0, 0, 100, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 15, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 20, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(123, 615, 616, hidden: false, ECharacterFeatureType.Bad, 617, 618, 619, new FeatureMedals[3]
		{
			new FeatureMedals("neg"),
			new FeatureMedals(),
			new FeatureMedals()
		}, -1, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: true, canBeExchanged: true, mergeable: true, basic: true, inscribable: true, soulTransform: true, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: true, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 120, 2021, 25, 0, -1, 1, 5, 100, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, -5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -10, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(124, 620, 621, hidden: false, ECharacterFeatureType.Bad, 622, 623, 624, new FeatureMedals[3]
		{
			new FeatureMedals("neg"),
			new FeatureMedals(),
			new FeatureMedals("neg")
		}, -2, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: true, canBeExchanged: true, mergeable: true, basic: true, inscribable: true, soulTransform: true, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: true, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 120, 2021, 5, 0, -1, 1, 0, 100, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, -10, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -15, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(125, 625, 626, hidden: false, ECharacterFeatureType.Bad, 627, 628, 629, new FeatureMedals[3]
		{
			new FeatureMedals("neg", "neg"),
			new FeatureMedals(),
			new FeatureMedals("neg")
		}, -3, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: true, canBeExchanged: true, mergeable: true, basic: true, inscribable: true, soulTransform: true, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: true, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 120, 2021, 1, 0, -1, 1, 0, 100, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, -15, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -20, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(126, 630, 631, hidden: false, ECharacterFeatureType.Good, 632, 633, 634, new FeatureMedals[3]
		{
			new FeatureMedals("pos"),
			new FeatureMedals(),
			new FeatureMedals()
		}, 1, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: true, canBeExchanged: true, mergeable: true, basic: true, inscribable: true, soulTransform: true, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: true, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 126, 1022, 25, 0, -1, 0, 15, 100, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 10, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(127, 635, 636, hidden: false, ECharacterFeatureType.Good, 637, 638, 639, new FeatureMedals[3]
		{
			new FeatureMedals("pos", "pos"),
			new FeatureMedals(),
			new FeatureMedals()
		}, 2, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: true, canBeExchanged: true, mergeable: true, basic: true, inscribable: true, soulTransform: true, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: true, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 126, 1022, 5, 0, -1, 0, 10, 100, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 10, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 15, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(128, 640, 641, hidden: false, ECharacterFeatureType.Good, 642, 643, 644, new FeatureMedals[3]
		{
			new FeatureMedals("pos", "pos", "pos"),
			new FeatureMedals(),
			new FeatureMedals()
		}, 3, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: true, canBeExchanged: true, mergeable: true, basic: true, inscribable: true, soulTransform: true, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: true, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 126, 1022, 1, 0, -1, 0, 0, 100, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 15, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 20, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(129, 645, 646, hidden: false, ECharacterFeatureType.Bad, 647, 648, 649, new FeatureMedals[3]
		{
			new FeatureMedals("neg"),
			new FeatureMedals(),
			new FeatureMedals()
		}, -1, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: true, canBeExchanged: true, mergeable: true, basic: true, inscribable: true, soulTransform: true, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: true, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 126, 2022, 25, 0, -1, 1, 5, 100, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, -5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -10, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(130, 650, 651, hidden: false, ECharacterFeatureType.Bad, 652, 653, 654, new FeatureMedals[3]
		{
			new FeatureMedals("neg", "neg"),
			new FeatureMedals(),
			new FeatureMedals()
		}, -2, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: true, canBeExchanged: true, mergeable: true, basic: true, inscribable: true, soulTransform: true, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: true, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 126, 2022, 5, 0, -1, 1, 0, 100, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, -10, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -15, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(131, 655, 656, hidden: false, ECharacterFeatureType.Bad, 657, 658, 659, new FeatureMedals[3]
		{
			new FeatureMedals("neg", "neg", "neg"),
			new FeatureMedals(),
			new FeatureMedals()
		}, -3, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: true, canBeExchanged: true, mergeable: true, basic: true, inscribable: true, soulTransform: true, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: true, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 126, 2022, 1, 0, -1, 1, 0, 100, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, -15, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -20, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(132, 660, 661, hidden: false, ECharacterFeatureType.Good, 662, 663, 664, new FeatureMedals[3]
		{
			new FeatureMedals("pos"),
			new FeatureMedals(),
			new FeatureMedals()
		}, 1, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: true, canBeExchanged: true, mergeable: true, basic: true, inscribable: true, soulTransform: true, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: true, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 132, 1023, 25, 0, -1, 0, 15, 100, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 10, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(133, 665, 666, hidden: false, ECharacterFeatureType.Good, 667, 668, 669, new FeatureMedals[3]
		{
			new FeatureMedals("pos", "pos"),
			new FeatureMedals(),
			new FeatureMedals()
		}, 2, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: true, canBeExchanged: true, mergeable: true, basic: true, inscribable: true, soulTransform: true, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: true, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 132, 1023, 5, 0, -1, 0, 10, 100, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 10, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 15, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(134, 670, 671, hidden: false, ECharacterFeatureType.Good, 672, 673, 674, new FeatureMedals[3]
		{
			new FeatureMedals("pos", "pos", "pos"),
			new FeatureMedals(),
			new FeatureMedals()
		}, 3, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: true, canBeExchanged: true, mergeable: true, basic: true, inscribable: true, soulTransform: true, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: true, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 132, 1023, 1, 0, -1, 0, 0, 100, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 15, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 20, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(135, 675, 676, hidden: false, ECharacterFeatureType.Bad, 677, 678, 679, new FeatureMedals[3]
		{
			new FeatureMedals("neg"),
			new FeatureMedals(),
			new FeatureMedals()
		}, -1, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: true, canBeExchanged: true, mergeable: true, basic: true, inscribable: true, soulTransform: true, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: true, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 132, 2023, 25, 0, -1, 1, 5, 100, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, -5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -10, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(136, 680, 681, hidden: false, ECharacterFeatureType.Bad, 682, 683, 684, new FeatureMedals[3]
		{
			new FeatureMedals("neg", "neg"),
			new FeatureMedals(),
			new FeatureMedals()
		}, -2, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: true, canBeExchanged: true, mergeable: true, basic: true, inscribable: true, soulTransform: true, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: true, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 132, 2023, 5, 0, -1, 1, 0, 100, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, -10, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -15, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(137, 685, 686, hidden: false, ECharacterFeatureType.Bad, 687, 688, 689, new FeatureMedals[3]
		{
			new FeatureMedals("neg", "neg", "neg"),
			new FeatureMedals(),
			new FeatureMedals()
		}, -3, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: true, canBeExchanged: true, mergeable: true, basic: true, inscribable: true, soulTransform: true, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: true, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 132, 2023, 1, 0, -1, 1, 0, 100, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, -15, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -20, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(138, 690, 691, hidden: false, ECharacterFeatureType.Good, 692, 693, 694, new FeatureMedals[3]
		{
			new FeatureMedals("pos"),
			new FeatureMedals(),
			new FeatureMedals()
		}, 1, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: true, canBeExchanged: true, mergeable: true, basic: true, inscribable: true, soulTransform: true, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: true, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 138, 1024, 25, 0, -1, 0, 15, 100, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 20, 0, 0, 0, 20, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(139, 695, 696, hidden: false, ECharacterFeatureType.Good, 697, 698, 699, new FeatureMedals[3]
		{
			new FeatureMedals("pos"),
			new FeatureMedals(),
			new FeatureMedals()
		}, 2, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: true, canBeExchanged: true, mergeable: true, basic: true, inscribable: true, soulTransform: true, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: true, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 138, 1024, 5, 0, -1, 0, 10, 100, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 10, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 40, 0, 0, 0, 40, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(140, 700, 701, hidden: false, ECharacterFeatureType.Good, 702, 703, 704, new FeatureMedals[3]
		{
			new FeatureMedals("pos"),
			new FeatureMedals(),
			new FeatureMedals()
		}, 3, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: true, canBeExchanged: true, mergeable: true, basic: true, inscribable: true, soulTransform: true, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: true, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 138, 1024, 1, 0, -1, 0, 0, 100, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 15, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 60, 0, 0, 0, 60, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(141, 705, 706, hidden: false, ECharacterFeatureType.Bad, 707, 708, 709, new FeatureMedals[3]
		{
			new FeatureMedals("neg"),
			new FeatureMedals(),
			new FeatureMedals()
		}, -1, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: true, canBeExchanged: true, mergeable: true, basic: true, inscribable: true, soulTransform: true, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: true, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 138, 2024, 25, 0, -1, 1, 5, 100, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], -5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -20, 0, 0, 0, -20, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(142, 710, 711, hidden: false, ECharacterFeatureType.Bad, 712, 713, 714, new FeatureMedals[3]
		{
			new FeatureMedals("neg"),
			new FeatureMedals(),
			new FeatureMedals()
		}, -2, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: true, canBeExchanged: true, mergeable: true, basic: true, inscribable: true, soulTransform: true, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: true, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 138, 2024, 5, 0, -1, 1, 0, 100, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], -10, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -40, 0, 0, 0, -40, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(143, 715, 716, hidden: false, ECharacterFeatureType.Bad, 717, 718, 719, new FeatureMedals[3]
		{
			new FeatureMedals("neg"),
			new FeatureMedals(),
			new FeatureMedals()
		}, -3, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: true, canBeExchanged: true, mergeable: true, basic: true, inscribable: true, soulTransform: true, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: true, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 138, 2024, 1, 0, -1, 1, 0, 100, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], -15, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -60, 0, 0, 0, -60, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(144, 720, 721, hidden: false, ECharacterFeatureType.Good, 722, 723, 724, new FeatureMedals[3]
		{
			new FeatureMedals("pos"),
			new FeatureMedals(),
			new FeatureMedals()
		}, 1, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: true, canBeExchanged: true, mergeable: true, basic: true, inscribable: true, soulTransform: true, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: true, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 144, 1025, 25, 0, -1, 0, 15, 100, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 0, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 20, 0, 0, 20, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(145, 725, 726, hidden: false, ECharacterFeatureType.Good, 727, 728, 729, new FeatureMedals[3]
		{
			new FeatureMedals("pos"),
			new FeatureMedals("pos"),
			new FeatureMedals()
		}, 2, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: true, canBeExchanged: true, mergeable: true, basic: true, inscribable: true, soulTransform: true, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: true, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 144, 1025, 5, 0, -1, 0, 10, 100, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 0, 10, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 40, 0, 0, 40, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(146, 730, 731, hidden: false, ECharacterFeatureType.Good, 732, 733, 734, new FeatureMedals[3]
		{
			new FeatureMedals("pos", "pos"),
			new FeatureMedals("pos"),
			new FeatureMedals()
		}, 3, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: true, canBeExchanged: true, mergeable: true, basic: true, inscribable: true, soulTransform: true, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: true, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 144, 1025, 1, 0, -1, 0, 0, 100, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 0, 15, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 60, 0, 0, 60, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(147, 735, 736, hidden: false, ECharacterFeatureType.Bad, 737, 738, 739, new FeatureMedals[3]
		{
			new FeatureMedals("neg"),
			new FeatureMedals(),
			new FeatureMedals()
		}, -1, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: true, canBeExchanged: true, mergeable: true, basic: true, inscribable: true, soulTransform: true, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: true, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 144, 2025, 25, 0, -1, 1, 5, 100, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 0, -5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -20, 0, 0, -20, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(148, 740, 741, hidden: false, ECharacterFeatureType.Bad, 742, 743, 744, new FeatureMedals[3]
		{
			new FeatureMedals("neg"),
			new FeatureMedals("neg"),
			new FeatureMedals()
		}, -2, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: true, canBeExchanged: true, mergeable: true, basic: true, inscribable: true, soulTransform: true, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: true, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 144, 2025, 5, 0, -1, 1, 0, 100, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 0, -10, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -40, 0, 0, -40, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(149, 745, 746, hidden: false, ECharacterFeatureType.Bad, 747, 748, 749, new FeatureMedals[3]
		{
			new FeatureMedals("neg", "neg"),
			new FeatureMedals("neg"),
			new FeatureMedals()
		}, -3, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: true, canBeExchanged: true, mergeable: true, basic: true, inscribable: true, soulTransform: true, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: true, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 144, 2025, 1, 0, -1, 1, 0, 100, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 0, -15, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -60, 0, 0, -60, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(150, 750, 751, hidden: false, ECharacterFeatureType.Good, 752, 753, 754, new FeatureMedals[3]
		{
			new FeatureMedals(),
			new FeatureMedals("pos"),
			new FeatureMedals()
		}, 1, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: true, canBeExchanged: true, mergeable: true, basic: true, inscribable: true, soulTransform: true, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: true, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 150, 1026, 25, 0, -1, 0, 15, 100, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 25, 25, 25, 25, 25, 25, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(151, 755, 756, hidden: false, ECharacterFeatureType.Good, 757, 758, 759, new FeatureMedals[3]
		{
			new FeatureMedals(),
			new FeatureMedals("pos", "pos"),
			new FeatureMedals()
		}, 2, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: true, canBeExchanged: true, mergeable: true, basic: true, inscribable: true, soulTransform: true, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: true, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 150, 1026, 5, 0, -1, 0, 10, 100, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 10, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 75, 75, 75, 75, 75, 75, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(152, 760, 761, hidden: false, ECharacterFeatureType.Good, 762, 763, 764, new FeatureMedals[3]
		{
			new FeatureMedals(),
			new FeatureMedals("pos", "pos", "pos"),
			new FeatureMedals()
		}, 3, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: true, canBeExchanged: true, mergeable: true, basic: true, inscribable: true, soulTransform: true, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: true, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 150, 1026, 1, 0, -1, 0, 0, 100, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 15, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 125, 125, 125, 125, 125, 125, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(153, 765, 766, hidden: false, ECharacterFeatureType.Bad, 767, 768, 769, new FeatureMedals[3]
		{
			new FeatureMedals(),
			new FeatureMedals("neg"),
			new FeatureMedals()
		}, -1, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: true, canBeExchanged: true, mergeable: true, basic: true, inscribable: true, soulTransform: true, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: true, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 150, 2026, 25, 0, -1, 1, 5, 100, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, -5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -50, -50, -50, -50, -50, -50, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(154, 770, 771, hidden: false, ECharacterFeatureType.Bad, 772, 773, 774, new FeatureMedals[3]
		{
			new FeatureMedals(),
			new FeatureMedals("neg", "neg"),
			new FeatureMedals()
		}, -2, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: true, canBeExchanged: true, mergeable: true, basic: true, inscribable: true, soulTransform: true, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: true, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 150, 2026, 5, 0, -1, 1, 0, 100, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, -10, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -150, -150, -150, -150, -150, -150, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(155, 775, 776, hidden: false, ECharacterFeatureType.Bad, 777, 778, 779, new FeatureMedals[3]
		{
			new FeatureMedals(),
			new FeatureMedals("neg", "neg", "neg"),
			new FeatureMedals()
		}, -3, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: true, canBeExchanged: true, mergeable: true, basic: true, inscribable: true, soulTransform: true, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: true, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 150, 2026, 1, 0, -1, 1, 0, 100, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, -15, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -250, -250, -250, -250, -250, -250, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(156, 780, 781, hidden: false, ECharacterFeatureType.Good, 782, 783, 784, new FeatureMedals[3]
		{
			new FeatureMedals("pos"),
			new FeatureMedals(),
			new FeatureMedals()
		}, 1, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: true, canBeExchanged: true, mergeable: true, basic: true, inscribable: true, soulTransform: true, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: true, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 156, 1027, 25, 0, -1, 0, 15, 100, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 20, 0, 0, 0, 20, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(157, 785, 786, hidden: false, ECharacterFeatureType.Good, 787, 788, 789, new FeatureMedals[3]
		{
			new FeatureMedals("pos", "pos"),
			new FeatureMedals(),
			new FeatureMedals()
		}, 2, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: true, canBeExchanged: true, mergeable: true, basic: true, inscribable: true, soulTransform: true, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: true, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 156, 1027, 5, 0, -1, 0, 10, 100, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 10, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 40, 0, 0, 0, 40, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(158, 790, 791, hidden: false, ECharacterFeatureType.Good, 792, 793, 794, new FeatureMedals[3]
		{
			new FeatureMedals("pos", "pos", "pos"),
			new FeatureMedals(),
			new FeatureMedals()
		}, 3, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: true, canBeExchanged: true, mergeable: true, basic: true, inscribable: true, soulTransform: true, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: true, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 156, 1027, 1, 0, -1, 0, 0, 100, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 15, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 60, 0, 0, 0, 60, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(159, 795, 796, hidden: false, ECharacterFeatureType.Bad, 797, 798, 799, new FeatureMedals[3]
		{
			new FeatureMedals("neg"),
			new FeatureMedals(),
			new FeatureMedals()
		}, -1, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: true, canBeExchanged: true, mergeable: true, basic: true, inscribable: true, soulTransform: true, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: true, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 156, 2027, 25, 0, -1, 1, 5, 100, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, -5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -20, 0, 0, 0, -20, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(160, 800, 801, hidden: false, ECharacterFeatureType.Bad, 802, 803, 804, new FeatureMedals[3]
		{
			new FeatureMedals("neg", "neg"),
			new FeatureMedals(),
			new FeatureMedals()
		}, -2, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: true, canBeExchanged: true, mergeable: true, basic: true, inscribable: true, soulTransform: true, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: true, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 156, 2027, 5, 0, -1, 1, 0, 100, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, -10, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -40, 0, 0, 0, -40, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(161, 805, 806, hidden: false, ECharacterFeatureType.Bad, 807, 808, 809, new FeatureMedals[3]
		{
			new FeatureMedals("neg", "neg", "neg"),
			new FeatureMedals(),
			new FeatureMedals()
		}, -3, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: true, canBeExchanged: true, mergeable: true, basic: true, inscribable: true, soulTransform: true, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: true, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 156, 2027, 1, 0, -1, 1, 0, 100, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, -15, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -60, 0, 0, 0, -60, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(162, 810, 811, hidden: false, ECharacterFeatureType.Good, 812, 813, 814, new FeatureMedals[3]
		{
			new FeatureMedals(),
			new FeatureMedals("pos"),
			new FeatureMedals()
		}, 1, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: true, canBeExchanged: true, mergeable: true, basic: true, inscribable: true, soulTransform: true, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: true, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 162, 1028, 25, 0, -1, 0, 15, 100, null, 0, 0, 0, 0, 0, 0, 90, 80, 500, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(163, 815, 816, hidden: false, ECharacterFeatureType.Good, 817, 818, 819, new FeatureMedals[3]
		{
			new FeatureMedals(),
			new FeatureMedals("pos", "pos"),
			new FeatureMedals()
		}, 2, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: true, canBeExchanged: true, mergeable: true, basic: true, inscribable: true, soulTransform: true, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: true, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 162, 1028, 5, 0, -1, 0, 10, 100, null, 0, 0, 0, 0, 0, 0, 80, 60, 1000, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 10, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(164, 820, 821, hidden: false, ECharacterFeatureType.Good, 822, 823, 824, new FeatureMedals[3]
		{
			new FeatureMedals(),
			new FeatureMedals("pos", "pos", "pos"),
			new FeatureMedals()
		}, 3, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: true, canBeExchanged: true, mergeable: true, basic: true, inscribable: true, soulTransform: true, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: true, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 162, 1028, 1, 0, -1, 0, 0, 100, null, 0, 0, 0, 0, 0, 0, 70, 40, 2000, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 15, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(165, 825, 826, hidden: false, ECharacterFeatureType.Bad, 827, 828, 829, new FeatureMedals[3]
		{
			new FeatureMedals(),
			new FeatureMedals("neg"),
			new FeatureMedals()
		}, -1, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: true, canBeExchanged: true, mergeable: true, basic: true, inscribable: true, soulTransform: true, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: true, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 162, 2028, 25, 0, -1, 1, 5, 100, null, 0, 0, 0, 0, 0, 0, 110, 120, 60, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, -5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(166, 830, 831, hidden: false, ECharacterFeatureType.Bad, 832, 833, 834, new FeatureMedals[3]
		{
			new FeatureMedals(),
			new FeatureMedals("neg", "neg"),
			new FeatureMedals()
		}, -2, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: true, canBeExchanged: true, mergeable: true, basic: true, inscribable: true, soulTransform: true, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: true, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 162, 2028, 5, 0, -1, 1, 0, 100, null, 0, 0, 0, 0, 0, 0, 120, 140, 40, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, -10, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(167, 835, 836, hidden: false, ECharacterFeatureType.Bad, 837, 838, 839, new FeatureMedals[3]
		{
			new FeatureMedals(),
			new FeatureMedals("neg", "neg", "neg"),
			new FeatureMedals()
		}, -3, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: true, canBeExchanged: true, mergeable: true, basic: true, inscribable: true, soulTransform: true, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: true, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 162, 2028, 1, 0, -1, 1, 0, 100, null, 0, 0, 0, 0, 0, 0, 130, 160, 20, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, -15, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(168, 840, 841, hidden: false, ECharacterFeatureType.Special, 842, 843, 844, new FeatureMedals[3]
		{
			new FeatureMedals("neg"),
			new FeatureMedals("dec"),
			new FeatureMedals("pos")
		}, -6, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: true, mergeable: false, basic: true, inscribable: true, soulTransform: true, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: true, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 168, 3001, 1, 0, 1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 30, -30, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1000, 0, -150, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(169, 845, 846, hidden: false, ECharacterFeatureType.Special, 847, 848, 849, new FeatureMedals[3]
		{
			new FeatureMedals("pos"),
			new FeatureMedals("neg"),
			new FeatureMedals("dec")
		}, -6, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: true, mergeable: false, basic: true, inscribable: true, soulTransform: true, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: true, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 168, 3001, 1, 0, 0, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 30, 0, 0, 0, 0, 0, -30, 0, 0, 0, 0, 0, 0, -1000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(170, 850, 851, hidden: false, ECharacterFeatureType.Special, 852, 853, 854, new FeatureMedals[3]
		{
			new FeatureMedals("dec"),
			new FeatureMedals("pos"),
			new FeatureMedals("neg")
		}, -6, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: true, mergeable: false, basic: true, inscribable: true, soulTransform: true, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: true, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 168, 3001, 1, 0, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 30, 0, 0, -30, 0, 0, 0, 0, 0, 0, 0, 0, -50, 0, 150, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(171, 855, 856, hidden: false, ECharacterFeatureType.Special, 857, 858, 859, new FeatureMedals[3]
		{
			new FeatureMedals(),
			new FeatureMedals(),
			new FeatureMedals("inc", "inc")
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: true, soulTransform: true, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 171, 102, 1, 0, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 15, 10, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 12, 12, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(172, 860, 861, hidden: false, ECharacterFeatureType.Special, 862, 863, 864, new FeatureMedals[3]
		{
			new FeatureMedals("inc"),
			new FeatureMedals(),
			new FeatureMedals("inc")
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: true, soulTransform: true, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 171, 102, 1, 0, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 15, 10, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(173, 865, 866, hidden: false, ECharacterFeatureType.Special, 867, 868, 869, new FeatureMedals[3]
		{
			new FeatureMedals("inc", "inc"),
			new FeatureMedals(),
			new FeatureMedals()
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: true, soulTransform: true, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 171, 102, 1, 0, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 10, 15, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 8, 8, 0, 0, 8, 8, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(174, 870, 871, hidden: false, ECharacterFeatureType.Special, 872, 873, 874, new FeatureMedals[3]
		{
			new FeatureMedals("inc"),
			new FeatureMedals("inc"),
			new FeatureMedals()
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: true, soulTransform: true, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 171, 102, 1, 0, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 10, 15, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 12, 12, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(175, 875, 876, hidden: false, ECharacterFeatureType.Special, 877, 878, 879, new FeatureMedals[3]
		{
			new FeatureMedals("inc"),
			new FeatureMedals("inc"),
			new FeatureMedals()
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: true, soulTransform: true, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 171, 102, 1, 0, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 15, 0, 10, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 12, 12, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(176, 880, 881, hidden: false, ECharacterFeatureType.Special, 882, 883, 884, new FeatureMedals[3]
		{
			new FeatureMedals(),
			new FeatureMedals("inc"),
			new FeatureMedals("inc")
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: true, soulTransform: true, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 171, 102, 1, 0, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 0, 10, 0, 15, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 12, 0, 0, 0, 0, 0, 0, 0, 12, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(177, 885, 886, hidden: false, ECharacterFeatureType.Special, 887, 888, 889, new FeatureMedals[3]
		{
			new FeatureMedals(),
			new FeatureMedals(),
			new FeatureMedals("inc", "inc")
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: true, soulTransform: true, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 171, 102, 1, 0, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 10, 0, 0, 0, 0, 15, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 12, 12, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(178, 890, 891, hidden: false, ECharacterFeatureType.Special, 892, 893, 894, new FeatureMedals[3]
		{
			new FeatureMedals(),
			new FeatureMedals("inc"),
			new FeatureMedals("inc")
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: true, soulTransform: true, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 171, 102, 1, 0, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 10, 0, 15, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 18, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(179, 895, 896, hidden: false, ECharacterFeatureType.Special, 897, 898, 899, new FeatureMedals[3]
		{
			new FeatureMedals(),
			new FeatureMedals("inc", "inc"),
			new FeatureMedals()
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: true, soulTransform: true, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 171, 102, 1, 0, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 0, 15, 20, 10, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 75, 75, 75, 75, 75, 75, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
	}

	private void CreateItems3()
	{
		_dataArray.Add(new CharacterFeatureItem(180, 900, 901, hidden: false, ECharacterFeatureType.Special, 902, 903, 904, new FeatureMedals[3]
		{
			new FeatureMedals("inc", "inc"),
			new FeatureMedals(),
			new FeatureMedals()
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: true, soulTransform: true, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 171, 102, 1, 0, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 15, 0, 0, 10, 0, 0, 0, 0, 0, 0, 0, 0, -100, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 120, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(181, 905, 906, hidden: false, ECharacterFeatureType.Special, 907, 908, 909, new FeatureMedals[3]
		{
			new FeatureMedals(),
			new FeatureMedals("inc", "inc"),
			new FeatureMedals()
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: true, soulTransform: true, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 171, 102, 1, 0, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 15, 0, 0, 10, 0, 0, 0, 0, 0, 0, 0, 0, 0, -25, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 18, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(182, 910, 911, hidden: false, ECharacterFeatureType.Special, 912, 913, 914, new FeatureMedals[3]
		{
			new FeatureMedals("inc"),
			new FeatureMedals(),
			new FeatureMedals("inc")
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: true, soulTransform: true, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 171, 102, 1, 0, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 10, 15, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 8, 8, 8, 8, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(183, 915, 916, hidden: false, ECharacterFeatureType.Special, 917, 918, 919, new FeatureMedals[3]
		{
			new FeatureMedals("inc"),
			new FeatureMedals("inc", "inc"),
			new FeatureMedals()
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: true, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 183, 101, 0, 0, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 20, 0, 40, 0, -10, 0, 10, 0, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(184, 920, 921, hidden: false, ECharacterFeatureType.Special, 922, 923, 924, new FeatureMedals[3]
		{
			new FeatureMedals(),
			new FeatureMedals("inc"),
			new FeatureMedals("inc", "inc")
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: true, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 183, 101, 0, 0, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 40, -10, 0, 0, 20, 0, 0, 0, 10, 0, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(185, 925, 926, hidden: false, ECharacterFeatureType.Special, 927, 928, 929, new FeatureMedals[3]
		{
			new FeatureMedals("inc", "inc"),
			new FeatureMedals(),
			new FeatureMedals("inc")
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: true, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 183, 101, 0, 0, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 20, 0, 0, 0, -10, 0, 40, 0, 0, 5, 0, 0, 10, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(186, 930, 931, hidden: false, ECharacterFeatureType.Special, 932, 933, 934, new FeatureMedals[3]
		{
			new FeatureMedals("inc"),
			new FeatureMedals("inc"),
			new FeatureMedals("inc")
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: true, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 183, 101, 0, 0, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], -10, 40, 20, 0, 0, 0, 0, 0, 0, 0, 5, 10, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(187, 935, 936, hidden: false, ECharacterFeatureType.Special, 937, 938, 939, new FeatureMedals[3]
		{
			new FeatureMedals("inc", "inc", "inc"),
			new FeatureMedals(),
			new FeatureMedals()
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: true, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 183, 101, 0, 0, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, -10, 0, 40, 20, 0, 0, 10, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(188, 940, 941, hidden: false, ECharacterFeatureType.Special, 942, 943, 944, new FeatureMedals[3]
		{
			new FeatureMedals("inc"),
			new FeatureMedals("inc"),
			new FeatureMedals("inc")
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: true, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 183, 101, 0, 0, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 40, 20, 0, 0, 0, 0, -10, 0, 0, 10, 0, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(189, 945, 946, hidden: false, ECharacterFeatureType.Special, 947, 948, 949, new FeatureMedals[3]
		{
			new FeatureMedals(),
			new FeatureMedals("inc", "inc", "inc"),
			new FeatureMedals()
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: true, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 183, 101, 0, 0, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], -10, 0, 40, 20, 0, 0, 0, 5, 0, 0, 10, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(190, 950, 951, hidden: false, ECharacterFeatureType.Special, 952, 953, 954, new FeatureMedals[3]
		{
			new FeatureMedals("inc"),
			new FeatureMedals(),
			new FeatureMedals("inc", "inc")
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: true, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 183, 101, 0, 0, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, -10, 0, 20, 0, 40, 0, 5, 0, 0, 0, 10, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(191, 955, 956, hidden: false, ECharacterFeatureType.Special, 957, 958, 959, new FeatureMedals[3]
		{
			new FeatureMedals("inc"),
			new FeatureMedals("inc"),
			new FeatureMedals("inc")
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: true, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 183, 101, 0, 0, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 40, 0, 20, -10, 0, 0, 5, 0, 0, 0, 10, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(192, 960, 961, hidden: false, ECharacterFeatureType.Special, 962, 963, 964, new FeatureMedals[3]
		{
			new FeatureMedals("inc", "inc"),
			new FeatureMedals("inc"),
			new FeatureMedals()
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: true, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 183, 101, 0, 0, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 20, 0, -10, 0, 40, 0, 0, 0, 10, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(193, 965, 966, hidden: false, ECharacterFeatureType.Special, 967, 968, 969, new FeatureMedals[3]
		{
			new FeatureMedals(),
			new FeatureMedals("inc", "inc"),
			new FeatureMedals("inc")
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: true, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 183, 101, 0, 0, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 40, 20, 0, -10, 0, 0, 0, 0, 0, 10, 0, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(194, 970, 971, hidden: false, ECharacterFeatureType.Special, 972, 973, 974, new FeatureMedals[3]
		{
			new FeatureMedals(),
			new FeatureMedals(),
			new FeatureMedals("inc", "inc", "inc")
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: true, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 183, 101, 0, 0, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, -10, 0, 40, 0, 0, 20, 10, 0, 0, 0, 0, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(195, 975, 976, hidden: true, ECharacterFeatureType.Special, 977, 978, 979, new FeatureMedals[3]
		{
			new FeatureMedals(),
			new FeatureMedals(),
			new FeatureMedals()
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: true, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 195, 0, 0, 0, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(196, 980, 981, hidden: true, ECharacterFeatureType.Special, 982, 983, 984, new FeatureMedals[3]
		{
			new FeatureMedals(),
			new FeatureMedals(),
			new FeatureMedals()
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: true, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 195, 0, 0, 0, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(197, 985, 986, hidden: false, ECharacterFeatureType.Special, 987, 988, 989, new FeatureMedals[3]
		{
			new FeatureMedals("dec"),
			new FeatureMedals("dec"),
			new FeatureMedals("dec")
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: false, canDeleteManually: false, canCrossArchive: false, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.Pregnant, -1, 197, 4, 0, 0, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -75, 0, 0, -20, -20, -20, -20, -20, -20, -20, -20, -20, -20, -20, -20, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(198, 990, 991, hidden: false, ECharacterFeatureType.Special, 992, 993, 994, new FeatureMedals[3]
		{
			new FeatureMedals("dec"),
			new FeatureMedals("dec"),
			new FeatureMedals("dec")
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: true, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 198, 108, 0, 0, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 60, 180, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(199, 995, 996, hidden: false, ECharacterFeatureType.Special, 997, 998, 999, new FeatureMedals[3]
		{
			new FeatureMedals(),
			new FeatureMedals("inc", "inc", "inc"),
			new FeatureMedals()
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: true, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 198, 108, 0, 0, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 40, 20, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(200, 1000, 1001, hidden: false, ECharacterFeatureType.Special, 1002, 1003, 1004, new FeatureMedals[3]
		{
			new FeatureMedals("inc"),
			new FeatureMedals("inc"),
			new FeatureMedals("inc")
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: true, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: true, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 200, 108, 0, 0, -1, -1, 0, 100, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 0, 0, 10, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(201, 1005, 1006, hidden: false, ECharacterFeatureType.Special, 1007, 1008, 1009, new FeatureMedals[3]
		{
			new FeatureMedals("inc"),
			new FeatureMedals("inc"),
			new FeatureMedals("inc")
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: true, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 201, 6, 0, 0, -1, -1, 0, 100, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 0, 20, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 30, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(202, 1010, 1011, hidden: false, ECharacterFeatureType.Special, 1012, 1013, 1014, new FeatureMedals[3]
		{
			new FeatureMedals("inc"),
			new FeatureMedals("inc"),
			new FeatureMedals("inc")
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: true, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 202, 6, 0, 0, -1, -1, 0, 100, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 0, 20, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 30, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(203, 1015, 1016, hidden: false, ECharacterFeatureType.Special, 1017, 1018, 1019, new FeatureMedals[3]
		{
			new FeatureMedals("inc"),
			new FeatureMedals("inc"),
			new FeatureMedals("inc")
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: true, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 203, 6, 0, 0, -1, -1, 0, 100, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 600, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(204, 1020, 1021, hidden: false, ECharacterFeatureType.Special, 1022, 1023, 1024, new FeatureMedals[3]
		{
			new FeatureMedals("neg"),
			new FeatureMedals(),
			new FeatureMedals("neg", "neg")
		}, 0, ECharacterFeatureInfectedType.LegendaryBook, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: false, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 204, 4, 0, 0, -1, -1, 0, 0, null, 0, 0, -10, 0, 0, 0, 40, 220, 100, makeConsummateLevelRelated: false, new short[5] { 30, 30, 30, 30, 30 }, new sbyte[5], new sbyte[5], 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 20, 20, 20, 20, 0, 0, 20, 20, 20, 20, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(205, 1025, 1026, hidden: false, ECharacterFeatureType.Special, 1027, 1028, 1029, new FeatureMedals[3]
		{
			new FeatureMedals("neg", "neg"),
			new FeatureMedals(),
			new FeatureMedals("neg")
		}, 0, ECharacterFeatureInfectedType.LegendaryBook, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: false, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.Infected, -1, 204, 4, 0, 0, -1, -1, 0, 0, null, 0, 0, -20, 0, 0, 0, 20, 260, 100, makeConsummateLevelRelated: false, new short[5] { 60, 60, 60, 60, 60 }, new sbyte[5], new sbyte[5], 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 40, 40, 40, 40, 0, 0, 40, 40, 40, 40, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(206, 1030, 1031, hidden: false, ECharacterFeatureType.Special, 1032, 1033, 1034, new FeatureMedals[3]
		{
			new FeatureMedals("neg"),
			new FeatureMedals("neg"),
			new FeatureMedals("neg")
		}, -6, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: true, mergeable: false, basic: false, inscribable: false, soulTransform: true, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 206, 5001, 0, 0, -1, -1, 0, 0, null, 0, 0, 0, 0, -30, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(207, 1035, 1036, hidden: false, ECharacterFeatureType.Special, 1037, 1038, 1039, new FeatureMedals[3]
		{
			new FeatureMedals("neg"),
			new FeatureMedals("neg"),
			new FeatureMedals("neg")
		}, -6, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: true, mergeable: false, basic: false, inscribable: false, soulTransform: true, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 207, 5002, 0, 0, -1, -1, 0, 0, null, 160, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(208, 1040, 1041, hidden: false, ECharacterFeatureType.Special, 1042, 1043, 1044, new FeatureMedals[3]
		{
			new FeatureMedals("neg"),
			new FeatureMedals("neg"),
			new FeatureMedals("neg")
		}, -6, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: true, mergeable: false, basic: false, inscribable: false, soulTransform: true, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 208, 5003, 0, 0, -1, -1, 0, 0, null, 0, 0, 0, 0, -30, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(209, 1045, 1046, hidden: false, ECharacterFeatureType.Special, 1047, 1048, 1049, new FeatureMedals[3]
		{
			new FeatureMedals("neg"),
			new FeatureMedals("neg"),
			new FeatureMedals("neg")
		}, -6, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: true, mergeable: false, basic: false, inscribable: false, soulTransform: true, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 209, 5004, 0, 0, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -500, -500, -500, -500, -500, -500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(210, 1050, 1051, hidden: false, ECharacterFeatureType.Special, 1052, 1053, 1054, new FeatureMedals[3]
		{
			new FeatureMedals(),
			new FeatureMedals("pos", "pos", "pos"),
			new FeatureMedals()
		}, 3, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: true, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 210, 103, 0, 0, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 0, 0, 100, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(211, 1055, 1056, hidden: false, ECharacterFeatureType.Special, 1057, 1058, 1059, new FeatureMedals[3]
		{
			new FeatureMedals(),
			new FeatureMedals(),
			new FeatureMedals("pos", "pos", "pos")
		}, 2, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: true, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 210, 103, 0, 0, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 0, 0, 50, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(212, 1060, 1061, hidden: false, ECharacterFeatureType.Special, 1062, 1063, 1064, new FeatureMedals[3]
		{
			new FeatureMedals("pos", "pos", "pos"),
			new FeatureMedals(),
			new FeatureMedals()
		}, 1, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: true, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 210, 103, 0, 0, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], -30, 0, -30, 30, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(213, 1065, 1066, hidden: false, ECharacterFeatureType.Special, 1067, 1068, 1069, new FeatureMedals[3]
		{
			new FeatureMedals("neg", "neg", "neg"),
			new FeatureMedals(),
			new FeatureMedals()
		}, -3, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: true, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 210, 103, 0, 0, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 0, 0, -100, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(214, 1070, 1071, hidden: false, ECharacterFeatureType.Special, 1072, 1073, 1074, new FeatureMedals[3]
		{
			new FeatureMedals(),
			new FeatureMedals(),
			new FeatureMedals("neg", "neg", "neg")
		}, -2, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: true, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 210, 103, 0, 0, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 0, 0, -50, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(215, 1075, 1076, hidden: false, ECharacterFeatureType.Special, 1077, 1078, 1079, new FeatureMedals[3]
		{
			new FeatureMedals(),
			new FeatureMedals("neg", "neg", "neg"),
			new FeatureMedals()
		}, -1, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: true, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 210, 103, 0, 0, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, -30, 0, -30, 30, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(216, 1080, 1081, hidden: true, ECharacterFeatureType.Special, 1082, 1083, 1084, new FeatureMedals[3]
		{
			new FeatureMedals(),
			new FeatureMedals(),
			new FeatureMedals()
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: true, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 216, 0, 0, 0, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(217, 1085, 1086, hidden: false, ECharacterFeatureType.Special, 1087, 1088, 1089, new FeatureMedals[3]
		{
			new FeatureMedals("neg"),
			new FeatureMedals(),
			new FeatureMedals("neg", "neg")
		}, 0, ECharacterFeatureInfectedType.PartlyInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: false, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 216, 2, 0, 0, -1, -1, 0, 0, null, 0, 0, -10, 0, 0, 0, 60, 180, 100, makeConsummateLevelRelated: false, new short[5] { 20, 20, 20, 20, 20 }, new sbyte[5], new sbyte[5], 0, 0, 0, 35, 0, -35, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 30, 30, 0, 0, 0, 0, 30, 30, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(218, 1090, 1091, hidden: false, ECharacterFeatureType.Special, 1092, 1093, 1094, new FeatureMedals[3]
		{
			new FeatureMedals("neg", "neg"),
			new FeatureMedals(),
			new FeatureMedals("neg")
		}, 0, ECharacterFeatureInfectedType.CompletelyInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: false, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.Infected, -1, 216, 2, 0, 0, -1, -1, 0, 0, null, 0, 0, -20, 0, 0, 0, 40, 220, 100, makeConsummateLevelRelated: false, new short[5] { 40, 40, 40, 40, 40 }, new sbyte[5], new sbyte[5], 0, 0, 0, -100, 0, -100, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 60, 60, 0, 0, 0, 0, 60, 60, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(219, 1095, 1096, hidden: false, ECharacterFeatureType.Special, 1097, 1098, 1099, new FeatureMedals[3]
		{
			new FeatureMedals("neg"),
			new FeatureMedals("neg"),
			new FeatureMedals("neg")
		}, 0, ECharacterFeatureInfectedType.CompletelyInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: true, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 219, 2, 0, 0, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 0, 0, -100, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(220, 1100, 1101, hidden: false, ECharacterFeatureType.Special, 1102, 1103, 1104, new FeatureMedals[3]
		{
			new FeatureMedals(),
			new FeatureMedals("inc", "inc"),
			new FeatureMedals("inc")
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: false, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 220, 6, 0, 0, -1, -1, 0, 0, null, 0, 0, -20, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 100, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(221, 1105, 1106, hidden: false, ECharacterFeatureType.Special, 1107, 1108, 1109, new FeatureMedals[3]
		{
			new FeatureMedals("inc", "inc"),
			new FeatureMedals("inc"),
			new FeatureMedals()
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: false, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 220, 6, 0, 0, -1, -1, 0, 0, null, 0, 0, -20, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 0, 100, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(222, 1110, 1111, hidden: false, ECharacterFeatureType.Special, 1112, 1113, 1114, new FeatureMedals[3]
		{
			new FeatureMedals(),
			new FeatureMedals("inc", "inc", "inc"),
			new FeatureMedals()
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: false, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 220, 6, 0, 0, -1, -1, 0, 0, null, 0, 0, -20, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 100, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(223, 1115, 1116, hidden: false, ECharacterFeatureType.Special, 1117, 1118, 1119, new FeatureMedals[3]
		{
			new FeatureMedals("inc"),
			new FeatureMedals("inc"),
			new FeatureMedals("inc")
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: false, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 220, 6, 0, 0, -1, -1, 0, 0, null, 0, 0, -40, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 0, 0, 100, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(224, 1120, 1121, hidden: false, ECharacterFeatureType.Special, 1122, 1123, 1124, new FeatureMedals[3]
		{
			new FeatureMedals("inc", "inc"),
			new FeatureMedals(),
			new FeatureMedals("inc")
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: false, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 220, 6, 0, 0, -1, -1, 0, 0, null, 0, 0, -20, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 0, 0, 0, 100, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(225, 1125, 1126, hidden: false, ECharacterFeatureType.Special, 1127, 1128, 1129, new FeatureMedals[3]
		{
			new FeatureMedals("inc"),
			new FeatureMedals("inc", "inc"),
			new FeatureMedals()
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: false, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 220, 6, 0, 0, -1, -1, 0, 0, null, 0, 0, -20, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 0, 100, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(226, 1130, 1131, hidden: false, ECharacterFeatureType.Special, 1132, 1133, 1134, new FeatureMedals[3]
		{
			new FeatureMedals(),
			new FeatureMedals(),
			new FeatureMedals("inc", "inc", "inc")
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: false, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 220, 6, 0, 0, -1, -1, 0, 0, null, 0, 0, -40, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 100, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(227, 1135, 1136, hidden: false, ECharacterFeatureType.Special, 1137, 1138, 1139, new FeatureMedals[3]
		{
			new FeatureMedals("inc", "inc", "inc"),
			new FeatureMedals(),
			new FeatureMedals()
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: false, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 220, 6, 0, 0, -1, -1, 0, 0, null, 0, 0, -20, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 100, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(228, 1140, 1141, hidden: false, ECharacterFeatureType.Special, 1142, 1143, 1144, new FeatureMedals[3]
		{
			new FeatureMedals(),
			new FeatureMedals("inc"),
			new FeatureMedals("inc", "inc")
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: false, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 220, 6, 0, 0, -1, -1, 0, 0, null, 0, 0, -60, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 0, 0, 100, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(229, 1145, 1146, hidden: false, ECharacterFeatureType.Special, 1147, 1148, 1149, new FeatureMedals[3]
		{
			new FeatureMedals("neg"),
			new FeatureMedals("neg"),
			new FeatureMedals("neg")
		}, 0, ECharacterFeatureInfectedType.CompletelyInfected, ignoreHealthMark: true, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: false, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.Infected, -1, 219, 2, 0, 0, -1, -1, 0, 0, null, 0, 0, -20, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(230, 1150, 1151, hidden: false, ECharacterFeatureType.Special, 1152, 1153, 1154, new FeatureMedals[3]
		{
			new FeatureMedals(),
			new FeatureMedals("inc", "inc", "inc"),
			new FeatureMedals()
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: false, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 220, 6, 0, 0, -1, -1, 0, 0, null, 0, 0, -40, 0, 0, 0, 10, 10, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, -50, 0, 0, 0, 50, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(231, 1155, 1156, hidden: false, ECharacterFeatureType.Special, 1157, 1158, 1159, new FeatureMedals[3]
		{
			new FeatureMedals("neg"),
			new FeatureMedals("neg"),
			new FeatureMedals("neg")
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: true, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: false, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 220, 6, 0, 0, -1, -1, 0, 0, null, 0, 0, -60, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 0, 0, -50, -50, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(232, 1160, 1161, hidden: false, ECharacterFeatureType.Special, 1162, 1163, 1164, new FeatureMedals[3]
		{
			new FeatureMedals("inc", "inc", "inc"),
			new FeatureMedals(),
			new FeatureMedals()
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: false, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 220, 6, 0, 0, -1, -1, 0, 0, null, 0, 0, -20, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], -50, 0, 0, 50, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(233, 1165, 1166, hidden: false, ECharacterFeatureType.Special, 1167, 1168, 1169, new FeatureMedals[3]
		{
			new FeatureMedals(),
			new FeatureMedals(),
			new FeatureMedals("inc", "inc", "inc")
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: false, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 220, 6, 0, 0, -1, -1, 0, 0, null, 0, 0, -20, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 50, 0, 0, -50, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(234, 1170, 1171, hidden: false, ECharacterFeatureType.Special, 1172, 1173, 1174, new FeatureMedals[3]
		{
			new FeatureMedals(),
			new FeatureMedals(),
			new FeatureMedals("pos")
		}, 1, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: true, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 234, 201, 0, 0, -1, -1, 0, 0, null, 0, 0, 0, 0, 10, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(235, 1170, 1175, hidden: false, ECharacterFeatureType.Special, 1172, 1176, 1177, new FeatureMedals[3]
		{
			new FeatureMedals(),
			new FeatureMedals(),
			new FeatureMedals("pos", "pos")
		}, 2, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: true, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 234, 201, 0, 0, -1, -1, 0, 0, null, 0, 0, 0, 0, 30, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(236, 1170, 1178, hidden: false, ECharacterFeatureType.Special, 1172, 1179, 1180, new FeatureMedals[3]
		{
			new FeatureMedals(),
			new FeatureMedals(),
			new FeatureMedals("pos", "pos", "pos")
		}, 3, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: true, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 234, 201, 0, 0, -1, -1, 0, 0, null, 0, 0, 0, 0, 50, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(237, 1181, 1182, hidden: false, ECharacterFeatureType.Special, 1183, 1184, 1185, new FeatureMedals[3]
		{
			new FeatureMedals(),
			new FeatureMedals("pos"),
			new FeatureMedals()
		}, 1, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: true, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 237, 202, 0, 0, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 25, 25, 25, 25, 25, 25, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(238, 1181, 1186, hidden: false, ECharacterFeatureType.Special, 1183, 1187, 1188, new FeatureMedals[3]
		{
			new FeatureMedals(),
			new FeatureMedals("pos", "pos"),
			new FeatureMedals()
		}, 2, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: true, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 237, 202, 0, 0, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 75, 75, 75, 75, 75, 75, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(239, 1181, 1189, hidden: false, ECharacterFeatureType.Special, 1183, 1190, 1191, new FeatureMedals[3]
		{
			new FeatureMedals(),
			new FeatureMedals("pos", "pos", "pos"),
			new FeatureMedals()
		}, 3, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: true, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 237, 202, 0, 0, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 125, 125, 125, 125, 125, 125, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
	}

	private void CreateItems4()
	{
		_dataArray.Add(new CharacterFeatureItem(240, 1192, 1193, hidden: false, ECharacterFeatureType.Special, 1194, 1195, 1196, new FeatureMedals[3]
		{
			new FeatureMedals(),
			new FeatureMedals("pos"),
			new FeatureMedals()
		}, 1, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: true, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 240, 203, 0, 0, -1, -1, 0, 0, null, 0, 10, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(241, 1192, 1197, hidden: false, ECharacterFeatureType.Special, 1194, 1198, 1199, new FeatureMedals[3]
		{
			new FeatureMedals(),
			new FeatureMedals("pos", "pos"),
			new FeatureMedals()
		}, 2, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: true, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 240, 203, 0, 0, -1, -1, 0, 0, null, 0, 20, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(242, 1192, 1200, hidden: false, ECharacterFeatureType.Special, 1194, 1201, 1202, new FeatureMedals[3]
		{
			new FeatureMedals(),
			new FeatureMedals("pos", "pos", "pos"),
			new FeatureMedals()
		}, 3, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: true, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 240, 203, 0, 0, -1, -1, 0, 0, null, 0, 30, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(243, 1203, 1204, hidden: false, ECharacterFeatureType.Special, 1205, 1206, 1207, new FeatureMedals[3]
		{
			new FeatureMedals(),
			new FeatureMedals(),
			new FeatureMedals("pos")
		}, 1, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: true, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 243, 204, 0, 0, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(244, 1203, 1208, hidden: false, ECharacterFeatureType.Special, 1205, 1209, 1207, new FeatureMedals[3]
		{
			new FeatureMedals(),
			new FeatureMedals(),
			new FeatureMedals("pos", "pos")
		}, 2, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: true, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 243, 204, 0, 0, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(245, 1203, 1210, hidden: false, ECharacterFeatureType.Special, 1205, 1211, 1207, new FeatureMedals[3]
		{
			new FeatureMedals(),
			new FeatureMedals(),
			new FeatureMedals("pos", "pos", "pos")
		}, 3, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: true, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 243, 204, 0, 0, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(246, 1212, 1213, hidden: false, ECharacterFeatureType.Special, 1214, 1215, 1216, new FeatureMedals[3]
		{
			new FeatureMedals(),
			new FeatureMedals(),
			new FeatureMedals("dec")
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: false, canDeleteManually: false, canCrossArchive: false, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 246, 301, 0, 0, -1, -1, 0, 0, null, 60, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(247, 1217, 1218, hidden: false, ECharacterFeatureType.Special, 1214, 1219, 1220, new FeatureMedals[3]
		{
			new FeatureMedals(),
			new FeatureMedals(),
			new FeatureMedals("dec")
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: false, canDeleteManually: false, canCrossArchive: false, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 247, 401, 0, 0, -1, -1, 0, 0, null, 60, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(248, 1221, 1222, hidden: false, ECharacterFeatureType.Special, 1223, 1224, 1225, new FeatureMedals[3]
		{
			new FeatureMedals(),
			new FeatureMedals("dec"),
			new FeatureMedals()
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: false, canDeleteManually: false, canCrossArchive: false, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 248, 302, 0, 0, -1, -1, 0, 0, null, 60, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(249, 1226, 1227, hidden: false, ECharacterFeatureType.Special, 1223, 1228, 1229, new FeatureMedals[3]
		{
			new FeatureMedals(),
			new FeatureMedals("dec"),
			new FeatureMedals()
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: false, canDeleteManually: false, canCrossArchive: false, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 249, 402, 0, 0, -1, -1, 0, 0, null, 60, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(250, 1230, 1231, hidden: false, ECharacterFeatureType.Special, 1232, 1233, 1234, new FeatureMedals[3]
		{
			new FeatureMedals("dec"),
			new FeatureMedals(),
			new FeatureMedals()
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: false, canDeleteManually: false, canCrossArchive: false, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 250, 303, 0, 0, -1, -1, 0, 0, null, 60, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -50, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(251, 1235, 1236, hidden: false, ECharacterFeatureType.Special, 1232, 1237, 1238, new FeatureMedals[3]
		{
			new FeatureMedals("dec"),
			new FeatureMedals(),
			new FeatureMedals()
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: false, canDeleteManually: false, canCrossArchive: false, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 251, 403, 0, 0, -1, -1, 0, 0, null, 60, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -50, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(252, 1239, 1240, hidden: false, ECharacterFeatureType.Special, 1241, 1242, 1243, new FeatureMedals[3]
		{
			new FeatureMedals("dec"),
			new FeatureMedals(),
			new FeatureMedals()
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: false, canDeleteManually: false, canCrossArchive: false, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 252, 304, 0, 0, -1, -1, 0, 0, null, 60, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(253, 1244, 1245, hidden: false, ECharacterFeatureType.Special, 1241, 1246, 1247, new FeatureMedals[3]
		{
			new FeatureMedals("dec"),
			new FeatureMedals(),
			new FeatureMedals()
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: false, canDeleteManually: false, canCrossArchive: false, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 253, 404, 0, 0, -1, -1, 0, 0, null, 60, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(254, 1248, 1249, hidden: false, ECharacterFeatureType.Special, 1250, 1251, 1252, new FeatureMedals[3]
		{
			new FeatureMedals(),
			new FeatureMedals("dec"),
			new FeatureMedals()
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: false, canDeleteManually: false, canCrossArchive: false, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 254, 305, 0, 0, -1, -1, 0, 0, null, 60, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(255, 1253, 1254, hidden: false, ECharacterFeatureType.Special, 1250, 1255, 1256, new FeatureMedals[3]
		{
			new FeatureMedals(),
			new FeatureMedals("dec"),
			new FeatureMedals()
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: false, canDeleteManually: false, canCrossArchive: false, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 255, 405, 0, 0, -1, -1, 0, 0, null, 60, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(256, 1257, 1258, hidden: false, ECharacterFeatureType.Special, 1259, 1260, 1261, new FeatureMedals[3]
		{
			new FeatureMedals("inc"),
			new FeatureMedals("inc"),
			new FeatureMedals("inc")
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: true, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 256, 6, 0, 0, -1, -1, 0, 100, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 100, 100, 100, 100, 100, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(257, 1262, 1263, hidden: false, ECharacterFeatureType.Special, 1264, 1265, 1266, new FeatureMedals[3]
		{
			new FeatureMedals("pos"),
			new FeatureMedals("pos"),
			new FeatureMedals("pos")
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: true, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 257, 103, 0, 0, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 100, 50, 20, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5] { 1, 0, 0, 0, 0 }, 0, 0, 0, 0, 0, 0, 100, 0, 0, 0, 0, 0, 0, 100, 0, 300, 0, 0, 0, 0, 0, 0, 0, 0, 0, 20, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(258, 1267, 1268, hidden: false, ECharacterFeatureType.Special, 1269, 1270, 1266, new FeatureMedals[3]
		{
			new FeatureMedals("pos"),
			new FeatureMedals("pos"),
			new FeatureMedals("pos")
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: true, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 257, 103, 0, 0, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 100, 50, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5] { 0, 1, 0, 0, 0 }, 0, 0, 0, 0, 0, 100, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 20, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 12, 12, 12, 12, 12, 12, 12, 12, 12, 12, 12, 12, 12, 12, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(259, 1271, 1272, hidden: false, ECharacterFeatureType.Special, 1273, 1274, 1266, new FeatureMedals[3]
		{
			new FeatureMedals("pos"),
			new FeatureMedals("pos"),
			new FeatureMedals("pos")
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: true, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 257, 103, 0, 0, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 100, 50, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5] { 0, 0, 1, 0, 0 }, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 20, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 12, 12, 12, 12, 12, 12, 12, 12, 12, 12, 12, 12, 12, 12, 12, 12, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(260, 1275, 1276, hidden: false, ECharacterFeatureType.Special, 1277, 1278, 1266, new FeatureMedals[3]
		{
			new FeatureMedals("pos"),
			new FeatureMedals("pos"),
			new FeatureMedals("pos")
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: true, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 257, 103, 0, 0, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 100, 50, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5] { 0, 0, 0, 1, 0 }, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 20, 20, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 100, 100, 100, 100, 100, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(261, 1279, 1280, hidden: false, ECharacterFeatureType.Special, 1281, 1282, 1266, new FeatureMedals[3]
		{
			new FeatureMedals("pos"),
			new FeatureMedals("pos"),
			new FeatureMedals("pos")
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: true, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 257, 103, 0, 0, -1, -1, 0, 0, null, 0, 0, 0, 0, 100, 0, 100, 50, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5] { 0, 0, 0, 0, 1 }, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 20, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(262, 1283, 1284, hidden: false, ECharacterFeatureType.Temporary, 1285, 1286, 1287, new FeatureMedals[3]
		{
			new FeatureMedals("inc"),
			new FeatureMedals("inc"),
			new FeatureMedals("inc")
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: false, canDeleteManually: false, canCrossArchive: false, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 262, 6002, 0, 12, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 10, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: true, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(263, 1288, 1289, hidden: false, ECharacterFeatureType.Temporary, 1290, 1291, 1292, new FeatureMedals[3]
		{
			new FeatureMedals("inc"),
			new FeatureMedals("inc"),
			new FeatureMedals("inc")
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: false, canDeleteManually: false, canCrossArchive: false, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 262, 6002, 0, 12, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 15, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: true, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(264, 1293, 1294, hidden: false, ECharacterFeatureType.Temporary, 1295, 1296, 1297, new FeatureMedals[3]
		{
			new FeatureMedals("inc"),
			new FeatureMedals("inc"),
			new FeatureMedals("inc")
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: false, canDeleteManually: false, canCrossArchive: false, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 262, 6002, 0, 12, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 20, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: true, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(265, 1298, 1299, hidden: false, ECharacterFeatureType.Temporary, 1300, 1301, 1302, new FeatureMedals[3]
		{
			new FeatureMedals("inc"),
			new FeatureMedals("inc"),
			new FeatureMedals("inc")
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: false, canDeleteManually: false, canCrossArchive: false, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 262, 6002, 0, 12, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 25, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: true, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(266, 1303, 1304, hidden: false, ECharacterFeatureType.Temporary, 1305, 1306, 1307, new FeatureMedals[3]
		{
			new FeatureMedals("inc"),
			new FeatureMedals("inc"),
			new FeatureMedals("inc")
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: false, canDeleteManually: false, canCrossArchive: false, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 262, 6002, 0, 12, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 30, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: true, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(267, 1308, 1309, hidden: false, ECharacterFeatureType.Temporary, 1310, 1311, 1312, new FeatureMedals[3]
		{
			new FeatureMedals("inc"),
			new FeatureMedals("inc"),
			new FeatureMedals("inc")
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: false, canDeleteManually: false, canCrossArchive: false, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 262, 6002, 0, 12, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 35, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: true, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(268, 1313, 1314, hidden: false, ECharacterFeatureType.Temporary, 1315, 1316, 1317, new FeatureMedals[3]
		{
			new FeatureMedals("inc"),
			new FeatureMedals("inc"),
			new FeatureMedals("inc")
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: false, canDeleteManually: false, canCrossArchive: false, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 262, 6002, 0, 12, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 40, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: true, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(269, 1318, 1319, hidden: false, ECharacterFeatureType.Temporary, 1320, 1321, 1322, new FeatureMedals[3]
		{
			new FeatureMedals("inc"),
			new FeatureMedals("inc"),
			new FeatureMedals("inc")
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: false, canDeleteManually: false, canCrossArchive: false, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 262, 6002, 0, 12, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 45, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: true, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(270, 1323, 1324, hidden: false, ECharacterFeatureType.Temporary, 1325, 1326, 1327, new FeatureMedals[3]
		{
			new FeatureMedals("inc"),
			new FeatureMedals("inc"),
			new FeatureMedals("inc")
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: false, canDeleteManually: false, canCrossArchive: false, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 262, 6002, 0, 12, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 50, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: true, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(271, 1328, 1329, hidden: false, ECharacterFeatureType.Temporary, 1330, 1331, 1332, new FeatureMedals[3]
		{
			new FeatureMedals("inc"),
			new FeatureMedals("inc"),
			new FeatureMedals("inc")
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: false, canDeleteManually: false, canCrossArchive: false, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 271, 6003, 0, 12, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 0, 0, 10, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: true, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(272, 1333, 1334, hidden: false, ECharacterFeatureType.Temporary, 1335, 1336, 1337, new FeatureMedals[3]
		{
			new FeatureMedals("inc"),
			new FeatureMedals("inc"),
			new FeatureMedals("inc")
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: false, canDeleteManually: false, canCrossArchive: false, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 271, 6003, 0, 12, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 0, 0, 15, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: true, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(273, 1338, 1339, hidden: false, ECharacterFeatureType.Temporary, 1340, 1341, 1342, new FeatureMedals[3]
		{
			new FeatureMedals("inc"),
			new FeatureMedals("inc"),
			new FeatureMedals("inc")
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: false, canDeleteManually: false, canCrossArchive: false, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 271, 6003, 0, 12, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 0, 0, 20, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: true, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(274, 1343, 1344, hidden: false, ECharacterFeatureType.Temporary, 1345, 1346, 1347, new FeatureMedals[3]
		{
			new FeatureMedals("inc"),
			new FeatureMedals("inc"),
			new FeatureMedals("inc")
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: false, canDeleteManually: false, canCrossArchive: false, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 271, 6003, 0, 12, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 0, 0, 25, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: true, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(275, 1348, 1349, hidden: false, ECharacterFeatureType.Temporary, 1350, 1351, 1352, new FeatureMedals[3]
		{
			new FeatureMedals("inc"),
			new FeatureMedals("inc"),
			new FeatureMedals("inc")
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: false, canDeleteManually: false, canCrossArchive: false, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 271, 6003, 0, 12, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 0, 0, 30, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: true, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(276, 1353, 1354, hidden: false, ECharacterFeatureType.Temporary, 1355, 1356, 1357, new FeatureMedals[3]
		{
			new FeatureMedals("inc"),
			new FeatureMedals("inc"),
			new FeatureMedals("inc")
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: false, canDeleteManually: false, canCrossArchive: false, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 271, 6003, 0, 12, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 0, 0, 35, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: true, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(277, 1358, 1359, hidden: false, ECharacterFeatureType.Temporary, 1360, 1361, 1362, new FeatureMedals[3]
		{
			new FeatureMedals("inc"),
			new FeatureMedals("inc"),
			new FeatureMedals("inc")
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: false, canDeleteManually: false, canCrossArchive: false, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 271, 6003, 0, 12, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 0, 0, 40, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: true, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(278, 1363, 1364, hidden: false, ECharacterFeatureType.Temporary, 1365, 1366, 1367, new FeatureMedals[3]
		{
			new FeatureMedals("inc"),
			new FeatureMedals("inc"),
			new FeatureMedals("inc")
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: false, canDeleteManually: false, canCrossArchive: false, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 271, 6003, 0, 12, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 0, 0, 45, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: true, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(279, 1368, 1369, hidden: false, ECharacterFeatureType.Temporary, 1370, 1371, 1372, new FeatureMedals[3]
		{
			new FeatureMedals("inc"),
			new FeatureMedals("inc"),
			new FeatureMedals("inc")
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: false, canDeleteManually: false, canCrossArchive: false, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 271, 6003, 0, 12, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 0, 0, 50, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: true, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(280, 1373, 1374, hidden: false, ECharacterFeatureType.Temporary, 1375, 1376, 1377, new FeatureMedals[3]
		{
			new FeatureMedals("inc"),
			new FeatureMedals("inc"),
			new FeatureMedals("inc")
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: false, canDeleteManually: false, canCrossArchive: false, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 280, 6004, 0, 12, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 0, 0, 0, 10, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: true, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(281, 1378, 1379, hidden: false, ECharacterFeatureType.Temporary, 1380, 1381, 1382, new FeatureMedals[3]
		{
			new FeatureMedals("inc"),
			new FeatureMedals("inc"),
			new FeatureMedals("inc")
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: false, canDeleteManually: false, canCrossArchive: false, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 280, 6004, 0, 12, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 0, 0, 0, 15, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: true, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(282, 1383, 1384, hidden: false, ECharacterFeatureType.Temporary, 1385, 1386, 1387, new FeatureMedals[3]
		{
			new FeatureMedals("inc"),
			new FeatureMedals("inc"),
			new FeatureMedals("inc")
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: false, canDeleteManually: false, canCrossArchive: false, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 280, 6004, 0, 12, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 0, 0, 0, 20, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: true, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(283, 1388, 1389, hidden: false, ECharacterFeatureType.Temporary, 1390, 1391, 1392, new FeatureMedals[3]
		{
			new FeatureMedals("inc"),
			new FeatureMedals("inc"),
			new FeatureMedals("inc")
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: false, canDeleteManually: false, canCrossArchive: false, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 280, 6004, 0, 12, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 0, 0, 0, 25, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: true, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(284, 1393, 1394, hidden: false, ECharacterFeatureType.Temporary, 1395, 1396, 1397, new FeatureMedals[3]
		{
			new FeatureMedals("inc"),
			new FeatureMedals("inc"),
			new FeatureMedals("inc")
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: false, canDeleteManually: false, canCrossArchive: false, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 280, 6004, 0, 12, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 0, 0, 0, 30, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: true, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(285, 1398, 1399, hidden: false, ECharacterFeatureType.Temporary, 1400, 1401, 1402, new FeatureMedals[3]
		{
			new FeatureMedals("inc"),
			new FeatureMedals("inc"),
			new FeatureMedals("inc")
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: false, canDeleteManually: false, canCrossArchive: false, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 280, 6004, 0, 12, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 0, 0, 0, 35, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: true, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(286, 1403, 1404, hidden: false, ECharacterFeatureType.Temporary, 1405, 1406, 1407, new FeatureMedals[3]
		{
			new FeatureMedals("inc"),
			new FeatureMedals("inc"),
			new FeatureMedals("inc")
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: false, canDeleteManually: false, canCrossArchive: false, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 280, 6004, 0, 12, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 0, 0, 0, 40, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: true, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(287, 1408, 1409, hidden: false, ECharacterFeatureType.Temporary, 1410, 1411, 1412, new FeatureMedals[3]
		{
			new FeatureMedals("inc"),
			new FeatureMedals("inc"),
			new FeatureMedals("inc")
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: false, canDeleteManually: false, canCrossArchive: false, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 280, 6004, 0, 12, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 0, 0, 0, 45, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: true, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(288, 1413, 1414, hidden: false, ECharacterFeatureType.Temporary, 1415, 1416, 1417, new FeatureMedals[3]
		{
			new FeatureMedals("inc"),
			new FeatureMedals("inc"),
			new FeatureMedals("inc")
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: false, canDeleteManually: false, canCrossArchive: false, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 280, 6004, 0, 12, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 0, 0, 0, 50, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: true, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(289, 1418, 1419, hidden: false, ECharacterFeatureType.Temporary, 1420, 1421, 1422, new FeatureMedals[3]
		{
			new FeatureMedals("inc"),
			new FeatureMedals("inc"),
			new FeatureMedals("inc")
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: false, canDeleteManually: false, canCrossArchive: false, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 289, 6005, 0, 12, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 0, 10, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: true, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(290, 1423, 1424, hidden: false, ECharacterFeatureType.Temporary, 1425, 1426, 1427, new FeatureMedals[3]
		{
			new FeatureMedals("inc"),
			new FeatureMedals("inc"),
			new FeatureMedals("inc")
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: false, canDeleteManually: false, canCrossArchive: false, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 289, 6005, 0, 12, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 0, 15, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: true, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(291, 1428, 1429, hidden: false, ECharacterFeatureType.Temporary, 1430, 1431, 1432, new FeatureMedals[3]
		{
			new FeatureMedals("inc"),
			new FeatureMedals("inc"),
			new FeatureMedals("inc")
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: false, canDeleteManually: false, canCrossArchive: false, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 289, 6005, 0, 12, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 0, 20, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: true, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(292, 1433, 1434, hidden: false, ECharacterFeatureType.Temporary, 1435, 1436, 1437, new FeatureMedals[3]
		{
			new FeatureMedals("inc"),
			new FeatureMedals("inc"),
			new FeatureMedals("inc")
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: false, canDeleteManually: false, canCrossArchive: false, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 289, 6005, 0, 12, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 0, 25, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: true, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(293, 1438, 1439, hidden: false, ECharacterFeatureType.Temporary, 1440, 1441, 1442, new FeatureMedals[3]
		{
			new FeatureMedals("inc"),
			new FeatureMedals("inc"),
			new FeatureMedals("inc")
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: false, canDeleteManually: false, canCrossArchive: false, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 289, 6005, 0, 12, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 0, 30, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: true, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(294, 1443, 1444, hidden: false, ECharacterFeatureType.Temporary, 1445, 1446, 1447, new FeatureMedals[3]
		{
			new FeatureMedals("inc"),
			new FeatureMedals("inc"),
			new FeatureMedals("inc")
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: false, canDeleteManually: false, canCrossArchive: false, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 289, 6005, 0, 12, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 0, 35, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: true, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(295, 1448, 1449, hidden: false, ECharacterFeatureType.Temporary, 1450, 1451, 1452, new FeatureMedals[3]
		{
			new FeatureMedals("inc"),
			new FeatureMedals("inc"),
			new FeatureMedals("inc")
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: false, canDeleteManually: false, canCrossArchive: false, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 289, 6005, 0, 12, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 0, 40, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: true, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(296, 1453, 1454, hidden: false, ECharacterFeatureType.Temporary, 1455, 1456, 1457, new FeatureMedals[3]
		{
			new FeatureMedals("inc"),
			new FeatureMedals("inc"),
			new FeatureMedals("inc")
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: false, canDeleteManually: false, canCrossArchive: false, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 289, 6005, 0, 12, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 0, 45, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: true, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(297, 1458, 1459, hidden: false, ECharacterFeatureType.Temporary, 1460, 1461, 1462, new FeatureMedals[3]
		{
			new FeatureMedals("inc"),
			new FeatureMedals("inc"),
			new FeatureMedals("inc")
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: false, canDeleteManually: false, canCrossArchive: false, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 289, 6005, 0, 12, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 0, 50, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: true, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(298, 1463, 1464, hidden: false, ECharacterFeatureType.Temporary, 1465, 1466, 1467, new FeatureMedals[3]
		{
			new FeatureMedals("inc"),
			new FeatureMedals("inc"),
			new FeatureMedals("inc")
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: false, canDeleteManually: false, canCrossArchive: false, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 298, 6006, 0, 12, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 10, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: true, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(299, 1468, 1469, hidden: false, ECharacterFeatureType.Temporary, 1470, 1471, 1472, new FeatureMedals[3]
		{
			new FeatureMedals("inc"),
			new FeatureMedals("inc"),
			new FeatureMedals("inc")
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: false, canDeleteManually: false, canCrossArchive: false, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 298, 6006, 0, 12, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 15, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: true, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
	}

	private void CreateItems5()
	{
		_dataArray.Add(new CharacterFeatureItem(300, 1473, 1474, hidden: false, ECharacterFeatureType.Temporary, 1475, 1476, 1477, new FeatureMedals[3]
		{
			new FeatureMedals("inc"),
			new FeatureMedals("inc"),
			new FeatureMedals("inc")
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: false, canDeleteManually: false, canCrossArchive: false, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 298, 6006, 0, 12, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 20, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: true, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(301, 1478, 1479, hidden: false, ECharacterFeatureType.Temporary, 1480, 1481, 1482, new FeatureMedals[3]
		{
			new FeatureMedals("inc"),
			new FeatureMedals("inc"),
			new FeatureMedals("inc")
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: false, canDeleteManually: false, canCrossArchive: false, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 298, 6006, 0, 12, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 25, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: true, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(302, 1483, 1484, hidden: false, ECharacterFeatureType.Temporary, 1485, 1486, 1487, new FeatureMedals[3]
		{
			new FeatureMedals("inc"),
			new FeatureMedals("inc"),
			new FeatureMedals("inc")
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: false, canDeleteManually: false, canCrossArchive: false, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 298, 6006, 0, 12, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 30, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: true, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(303, 1488, 1489, hidden: false, ECharacterFeatureType.Temporary, 1490, 1491, 1492, new FeatureMedals[3]
		{
			new FeatureMedals("inc"),
			new FeatureMedals("inc"),
			new FeatureMedals("inc")
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: false, canDeleteManually: false, canCrossArchive: false, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 298, 6006, 0, 12, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 35, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: true, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(304, 1493, 1494, hidden: false, ECharacterFeatureType.Temporary, 1495, 1496, 1497, new FeatureMedals[3]
		{
			new FeatureMedals("inc"),
			new FeatureMedals("inc"),
			new FeatureMedals("inc")
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: false, canDeleteManually: false, canCrossArchive: false, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 298, 6006, 0, 12, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 40, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: true, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(305, 1498, 1499, hidden: false, ECharacterFeatureType.Temporary, 1500, 1501, 1502, new FeatureMedals[3]
		{
			new FeatureMedals("inc"),
			new FeatureMedals("inc"),
			new FeatureMedals("inc")
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: false, canDeleteManually: false, canCrossArchive: false, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 298, 6006, 0, 12, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 45, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: true, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(306, 1503, 1504, hidden: false, ECharacterFeatureType.Temporary, 1505, 1506, 1507, new FeatureMedals[3]
		{
			new FeatureMedals("inc"),
			new FeatureMedals("inc"),
			new FeatureMedals("inc")
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: false, canDeleteManually: false, canCrossArchive: false, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 298, 6006, 0, 12, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 50, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: true, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(307, 1508, 1509, hidden: false, ECharacterFeatureType.Temporary, 1510, 1511, 1512, new FeatureMedals[3]
		{
			new FeatureMedals("inc"),
			new FeatureMedals("inc"),
			new FeatureMedals("inc")
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: false, canDeleteManually: false, canCrossArchive: false, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 307, 6007, 0, 12, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 10, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: true, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(308, 1513, 1514, hidden: false, ECharacterFeatureType.Temporary, 1515, 1516, 1517, new FeatureMedals[3]
		{
			new FeatureMedals("inc"),
			new FeatureMedals("inc"),
			new FeatureMedals("inc")
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: false, canDeleteManually: false, canCrossArchive: false, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 307, 6007, 0, 12, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 15, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: true, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(309, 1518, 1519, hidden: false, ECharacterFeatureType.Temporary, 1520, 1521, 1522, new FeatureMedals[3]
		{
			new FeatureMedals("inc"),
			new FeatureMedals("inc"),
			new FeatureMedals("inc")
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: false, canDeleteManually: false, canCrossArchive: false, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 307, 6007, 0, 12, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 20, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: true, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(310, 1523, 1524, hidden: false, ECharacterFeatureType.Temporary, 1525, 1526, 1527, new FeatureMedals[3]
		{
			new FeatureMedals("inc"),
			new FeatureMedals("inc"),
			new FeatureMedals("inc")
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: false, canDeleteManually: false, canCrossArchive: false, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 307, 6007, 0, 12, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 25, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: true, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(311, 1528, 1529, hidden: false, ECharacterFeatureType.Temporary, 1530, 1531, 1532, new FeatureMedals[3]
		{
			new FeatureMedals("inc"),
			new FeatureMedals("inc"),
			new FeatureMedals("inc")
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: false, canDeleteManually: false, canCrossArchive: false, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 307, 6007, 0, 12, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 30, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: true, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(312, 1533, 1534, hidden: false, ECharacterFeatureType.Temporary, 1535, 1536, 1537, new FeatureMedals[3]
		{
			new FeatureMedals("inc"),
			new FeatureMedals("inc"),
			new FeatureMedals("inc")
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: false, canDeleteManually: false, canCrossArchive: false, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 307, 6007, 0, 12, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 35, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: true, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(313, 1538, 1539, hidden: false, ECharacterFeatureType.Temporary, 1540, 1541, 1542, new FeatureMedals[3]
		{
			new FeatureMedals("inc"),
			new FeatureMedals("inc"),
			new FeatureMedals("inc")
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: false, canDeleteManually: false, canCrossArchive: false, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 307, 6007, 0, 12, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 40, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: true, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(314, 1543, 1544, hidden: false, ECharacterFeatureType.Temporary, 1545, 1546, 1547, new FeatureMedals[3]
		{
			new FeatureMedals("inc"),
			new FeatureMedals("inc"),
			new FeatureMedals("inc")
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: false, canDeleteManually: false, canCrossArchive: false, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 307, 6007, 0, 12, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 45, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: true, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(315, 1548, 1549, hidden: false, ECharacterFeatureType.Temporary, 1550, 1551, 1552, new FeatureMedals[3]
		{
			new FeatureMedals("inc"),
			new FeatureMedals("inc"),
			new FeatureMedals("inc")
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: false, canDeleteManually: false, canCrossArchive: false, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 307, 6007, 0, 12, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 50, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: true, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(316, 1553, 1554, hidden: false, ECharacterFeatureType.Temporary, 1555, 1556, 1557, new FeatureMedals[3]
		{
			new FeatureMedals("inc"),
			new FeatureMedals("inc"),
			new FeatureMedals("inc")
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: false, canDeleteManually: false, canCrossArchive: false, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 316, 6008, 0, 12, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 10, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: true, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(317, 1558, 1559, hidden: false, ECharacterFeatureType.Temporary, 1560, 1561, 1562, new FeatureMedals[3]
		{
			new FeatureMedals("inc"),
			new FeatureMedals("inc"),
			new FeatureMedals("inc")
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: false, canDeleteManually: false, canCrossArchive: false, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 316, 6008, 0, 12, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 15, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: true, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(318, 1563, 1564, hidden: false, ECharacterFeatureType.Temporary, 1565, 1566, 1567, new FeatureMedals[3]
		{
			new FeatureMedals("inc"),
			new FeatureMedals("inc"),
			new FeatureMedals("inc")
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: false, canDeleteManually: false, canCrossArchive: false, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 316, 6008, 0, 12, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 20, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: true, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(319, 1568, 1569, hidden: false, ECharacterFeatureType.Temporary, 1570, 1571, 1572, new FeatureMedals[3]
		{
			new FeatureMedals("inc"),
			new FeatureMedals("inc"),
			new FeatureMedals("inc")
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: false, canDeleteManually: false, canCrossArchive: false, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 316, 6008, 0, 12, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 25, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: true, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(320, 1573, 1574, hidden: false, ECharacterFeatureType.Temporary, 1575, 1576, 1577, new FeatureMedals[3]
		{
			new FeatureMedals("inc"),
			new FeatureMedals("inc"),
			new FeatureMedals("inc")
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: false, canDeleteManually: false, canCrossArchive: false, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 316, 6008, 0, 12, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 30, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: true, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(321, 1578, 1579, hidden: false, ECharacterFeatureType.Temporary, 1580, 1581, 1582, new FeatureMedals[3]
		{
			new FeatureMedals("inc"),
			new FeatureMedals("inc"),
			new FeatureMedals("inc")
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: false, canDeleteManually: false, canCrossArchive: false, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 316, 6008, 0, 12, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 35, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: true, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(322, 1583, 1584, hidden: false, ECharacterFeatureType.Temporary, 1585, 1586, 1587, new FeatureMedals[3]
		{
			new FeatureMedals("inc"),
			new FeatureMedals("inc"),
			new FeatureMedals("inc")
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: false, canDeleteManually: false, canCrossArchive: false, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 316, 6008, 0, 12, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 40, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: true, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(323, 1588, 1589, hidden: false, ECharacterFeatureType.Temporary, 1590, 1591, 1592, new FeatureMedals[3]
		{
			new FeatureMedals("inc"),
			new FeatureMedals("inc"),
			new FeatureMedals("inc")
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: false, canDeleteManually: false, canCrossArchive: false, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 316, 6008, 0, 12, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 45, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: true, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(324, 1593, 1594, hidden: false, ECharacterFeatureType.Temporary, 1595, 1596, 1597, new FeatureMedals[3]
		{
			new FeatureMedals("inc"),
			new FeatureMedals("inc"),
			new FeatureMedals("inc")
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: false, canDeleteManually: false, canCrossArchive: false, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 316, 6008, 0, 12, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 50, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: true, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(325, 1598, 1599, hidden: false, ECharacterFeatureType.Special, 1600, 1601, 1602, new FeatureMedals[3]
		{
			new FeatureMedals("neg", "neg", "neg"),
			new FeatureMedals(),
			new FeatureMedals()
		}, 0, ECharacterFeatureInfectedType.CompletelyInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: true, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.Infected, -1, 325, 104, 0, 0, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 0, 0, 0, -10, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(326, 1603, 1604, hidden: false, ECharacterFeatureType.Special, 1605, 1606, 1607, new FeatureMedals[3]
		{
			new FeatureMedals("neg"),
			new FeatureMedals("neg"),
			new FeatureMedals("neg")
		}, 0, ECharacterFeatureInfectedType.CompletelyInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: true, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.Infected, -1, 325, 104, 0, 0, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5] { 10, 10, 10, 10, 10 }, new sbyte[5], new sbyte[5], 0, 0, 0, 0, 0, 0, -15, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(327, 1608, 1609, hidden: false, ECharacterFeatureType.Special, 1610, 1611, 1612, new FeatureMedals[3]
		{
			new FeatureMedals(),
			new FeatureMedals("neg", "neg", "neg"),
			new FeatureMedals()
		}, 0, ECharacterFeatureInfectedType.CompletelyInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: true, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.Infected, -1, 325, 104, 0, 0, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5] { 20, 20, 20, 20, 20 }, new sbyte[5], new sbyte[5], 0, 0, 0, 0, 0, 0, -20, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(328, 1613, 1614, hidden: false, ECharacterFeatureType.Special, 1615, 1616, 1617, new FeatureMedals[3]
		{
			new FeatureMedals(),
			new FeatureMedals(),
			new FeatureMedals("neg", "neg", "neg")
		}, 0, ECharacterFeatureInfectedType.CompletelyInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: true, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.Infected, -1, 325, 104, 0, 0, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5] { 30, 30, 30, 30, 30 }, new sbyte[5], new sbyte[5], 0, 0, 0, 0, 0, 0, -25, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(329, 1618, 1619, hidden: false, ECharacterFeatureType.Special, 1620, 1621, 1622, new FeatureMedals[3]
		{
			new FeatureMedals("neg"),
			new FeatureMedals("neg"),
			new FeatureMedals("neg")
		}, 0, ECharacterFeatureInfectedType.CompletelyInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: true, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.Infected, -1, 325, 104, 0, 0, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5] { 40, 40, 40, 40, 40 }, new sbyte[5], new sbyte[5], 0, 0, 0, 0, 0, 0, -30, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(330, 1623, 1624, hidden: false, ECharacterFeatureType.Special, 1625, 1626, 1627, new FeatureMedals[3]
		{
			new FeatureMedals(),
			new FeatureMedals(),
			new FeatureMedals("neg", "neg", "neg")
		}, 0, ECharacterFeatureInfectedType.CompletelyInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: true, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.Infected, -1, 325, 104, 0, 0, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5] { 50, 50, 50, 50, 50 }, new sbyte[5], new sbyte[5], 0, 0, 0, 0, 0, 0, -35, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(331, 1628, 1629, hidden: false, ECharacterFeatureType.Special, 1630, 1631, 1632, new FeatureMedals[3]
		{
			new FeatureMedals("neg"),
			new FeatureMedals("neg"),
			new FeatureMedals("neg")
		}, 0, ECharacterFeatureInfectedType.CompletelyInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: true, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.Infected, -1, 325, 104, 0, 0, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5] { 60, 60, 60, 60, 60 }, new sbyte[5], new sbyte[5], 0, 0, 0, 0, 0, 0, -40, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(332, 1633, 1634, hidden: false, ECharacterFeatureType.Special, 1635, 1636, 1637, new FeatureMedals[3]
		{
			new FeatureMedals(),
			new FeatureMedals("neg", "neg", "neg"),
			new FeatureMedals()
		}, 0, ECharacterFeatureInfectedType.CompletelyInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: true, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.Infected, -1, 325, 104, 0, 0, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5] { 70, 70, 70, 70, 70 }, new sbyte[5], new sbyte[5], 0, 0, 0, 0, 0, 0, -45, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(333, 1638, 1639, hidden: false, ECharacterFeatureType.Special, 1640, 1641, 1642, new FeatureMedals[3]
		{
			new FeatureMedals("neg", "neg", "neg"),
			new FeatureMedals(),
			new FeatureMedals()
		}, 0, ECharacterFeatureInfectedType.CompletelyInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: true, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.Infected, -1, 325, 104, 0, 0, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5] { 80, 80, 80, 80, 80 }, new sbyte[5], new sbyte[5], 0, 0, 0, 0, 0, 0, -50, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(334, 1643, 1644, hidden: false, ECharacterFeatureType.Temporary, 1645, 1646, 1647, new FeatureMedals[3]
		{
			new FeatureMedals("inc"),
			new FeatureMedals("inc"),
			new FeatureMedals("inc")
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: true, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 334, 6001, 0, 12, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 120, 80, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: true, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(335, 1648, 1649, hidden: false, ECharacterFeatureType.Special, 1650, 1651, 1652, new FeatureMedals[3]
		{
			new FeatureMedals("inc"),
			new FeatureMedals("inc"),
			new FeatureMedals("inc")
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: true, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 335, 6, 0, 0, -1, -1, 0, 100, null, 0, 0, 0, 0, 100, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5] { 0, 1, 1, 1, 1 }, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(336, 1653, 1654, hidden: false, ECharacterFeatureType.Special, 1655, 1656, 1657, new FeatureMedals[3]
		{
			new FeatureMedals("pos"),
			new FeatureMedals("pos"),
			new FeatureMedals("pos")
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: true, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 336, 2, 0, 0, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 0, 0, 100, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(337, 1658, 1659, hidden: false, ECharacterFeatureType.Special, 1660, 1661, 1662, new FeatureMedals[3]
		{
			new FeatureMedals("neg"),
			new FeatureMedals("neg"),
			new FeatureMedals("neg")
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: true, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: true, canDeleteManually: false, canCrossArchive: false, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.WalkingDead, -1, 337, 106, 0, 0, -1, -1, 0, 100, null, 0, 0, 0, 0, 0, 0, 0, 0, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], -100, -100, -100, -100, -100, -100, -100, 0, 0, 0, 0, 0, 0, -1000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 150, 150, 150, 150, 150, 150, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(338, 1663, 1664, hidden: false, ECharacterFeatureType.Special, 1665, 1666, 1667, new FeatureMedals[3]
		{
			new FeatureMedals("inc"),
			new FeatureMedals("inc"),
			new FeatureMedals("inc")
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: true, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: true, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 220, 6, 0, 0, -1, -1, 0, 100, null, 0, 0, 0, 0, 300, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(339, 1668, 1669, hidden: false, ECharacterFeatureType.Special, 1670, 1671, 1672, new FeatureMedals[3]
		{
			new FeatureMedals("neg"),
			new FeatureMedals("neg"),
			new FeatureMedals("neg")
		}, -6, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: true, mergeable: false, basic: false, inscribable: true, soulTransform: true, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 339, 106, 0, 0, -1, -1, 0, 50, null, 160, 0, 0, 0, -30, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 0, 0, -50, -50, 0, 0, 0, 0, 0, 0, -50, 0, -600, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -500, -500, -500, -500, -500, -500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(340, 1673, 1674, hidden: false, ECharacterFeatureType.Special, 1675, 1676, 1677, new FeatureMedals[3]
		{
			new FeatureMedals(),
			new FeatureMedals(),
			new FeatureMedals("inc", "inc", "inc")
		}, 0, ECharacterFeatureInfectedType.LegendaryBook, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: false, canDeleteManually: false, canCrossArchive: false, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 340, 3, 0, 0, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 60, 180, 100, makeConsummateLevelRelated: false, new short[5] { 20, 20, 20, 20, 20 }, new sbyte[5], new sbyte[5], 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(341, 1678, 1679, hidden: false, ECharacterFeatureType.Special, 1680, 1681, 1682, new FeatureMedals[3]
		{
			new FeatureMedals(),
			new FeatureMedals(),
			new FeatureMedals("inc", "inc", "inc")
		}, 0, ECharacterFeatureInfectedType.LegendaryBook, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: false, canDeleteManually: false, canCrossArchive: false, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 341, 5, 0, 0, -1, -1, 0, 0, "LegendaryBook.NpcEffect.Neigong", 0, 0, -20, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5] { 80, 0, 0, 0, 0 }, new sbyte[5], new sbyte[5], 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 50, 50, 0, 0, 0, 0, 50, 50, 120, 120, 0, 0, 0, 0, 0, 0, 120, 120, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 90, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(342, 1683, 1684, hidden: false, ECharacterFeatureType.Special, 1685, 1686, 1687, new FeatureMedals[3]
		{
			new FeatureMedals(),
			new FeatureMedals(),
			new FeatureMedals("inc", "inc", "inc")
		}, 0, ECharacterFeatureInfectedType.LegendaryBook, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: false, canDeleteManually: false, canCrossArchive: false, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 342, 5, 0, 0, -1, -1, 0, 0, "LegendaryBook.NpcEffect.Posing", 0, 0, -20, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5] { 0, 0, 80, 0, 0 }, new sbyte[5], new sbyte[5], 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 50, 50, 50, 50, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 120, 120, 0, 0, 120, 120, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 90, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(343, 1688, 1689, hidden: false, ECharacterFeatureType.Special, 1690, 1691, 1692, new FeatureMedals[3]
		{
			new FeatureMedals(),
			new FeatureMedals(),
			new FeatureMedals("inc", "inc", "inc")
		}, 0, ECharacterFeatureInfectedType.LegendaryBook, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: false, canDeleteManually: false, canCrossArchive: false, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 343, 5, 0, 0, -1, -1, 0, 0, "LegendaryBook.NpcEffect.Stunt", 0, 0, -20, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5] { 0, 0, 0, 80, 80 }, new sbyte[5], new sbyte[5], 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 50, 50, 50, 50, 0, 0, 120, 120, 0, 0, 120, 120, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 90, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(344, 1693, 1694, hidden: false, ECharacterFeatureType.Special, 1695, 1696, 1697, new FeatureMedals[3]
		{
			new FeatureMedals(),
			new FeatureMedals(),
			new FeatureMedals("inc", "inc", "inc")
		}, 0, ECharacterFeatureInfectedType.LegendaryBook, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: false, canDeleteManually: false, canCrossArchive: false, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 344, 5, 0, 0, -1, -1, 0, 0, "LegendaryBook.NpcEffect.FistAndPalm", 0, 0, -20, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5] { 0, 40, 0, 0, 0 }, new sbyte[5], new sbyte[5], 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 0, 0, 0, 0, 0, 180, 180, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 90, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(345, 1698, 1699, hidden: false, ECharacterFeatureType.Special, 1700, 1701, 1702, new FeatureMedals[3]
		{
			new FeatureMedals(),
			new FeatureMedals(),
			new FeatureMedals("inc", "inc", "inc")
		}, 0, ECharacterFeatureInfectedType.LegendaryBook, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: false, canDeleteManually: false, canCrossArchive: false, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 345, 5, 0, 0, -1, -1, 0, 0, "LegendaryBook.NpcEffect.Finger", 0, 0, -20, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5] { 0, 40, 0, 0, 0 }, new sbyte[5], new sbyte[5], 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 180, 180, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 90, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(346, 1703, 1704, hidden: false, ECharacterFeatureType.Special, 1705, 1706, 1707, new FeatureMedals[3]
		{
			new FeatureMedals(),
			new FeatureMedals(),
			new FeatureMedals("inc", "inc", "inc")
		}, 0, ECharacterFeatureInfectedType.LegendaryBook, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: false, canDeleteManually: false, canCrossArchive: false, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 346, 5, 0, 0, -1, -1, 0, 0, "LegendaryBook.NpcEffect.Leg", 0, 0, -20, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5] { 0, 40, 0, 0, 0 }, new sbyte[5], new sbyte[5], 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 50, 50, 50, 0, 0, 0, 0, 0, 180, 180, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 90, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(347, 1708, 1709, hidden: false, ECharacterFeatureType.Special, 1710, 1711, 1712, new FeatureMedals[3]
		{
			new FeatureMedals(),
			new FeatureMedals(),
			new FeatureMedals("inc", "inc", "inc")
		}, 0, ECharacterFeatureInfectedType.LegendaryBook, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: false, canDeleteManually: false, canCrossArchive: false, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 347, 5, 0, 0, -1, -1, 0, 0, "LegendaryBook.NpcEffect.Throw", 0, 0, -20, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5] { 0, 40, 0, 0, 0 }, new sbyte[5], new sbyte[5], 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 180, 0, 0, 0, 180, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 90, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(348, 1713, 1714, hidden: false, ECharacterFeatureType.Special, 1715, 1716, 1717, new FeatureMedals[3]
		{
			new FeatureMedals(),
			new FeatureMedals(),
			new FeatureMedals("inc", "inc", "inc")
		}, 0, ECharacterFeatureInfectedType.LegendaryBook, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: false, canDeleteManually: false, canCrossArchive: false, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 348, 5, 0, 0, -1, -1, 0, 0, "LegendaryBook.NpcEffect.Sword", 0, 0, -20, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5] { 0, 40, 0, 0, 0 }, new sbyte[5], new sbyte[5], 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 50, 50, 50, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 180, 0, 0, 0, 180, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 90, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(349, 1718, 1719, hidden: false, ECharacterFeatureType.Special, 1720, 1721, 1722, new FeatureMedals[3]
		{
			new FeatureMedals(),
			new FeatureMedals(),
			new FeatureMedals("inc", "inc", "inc")
		}, 0, ECharacterFeatureInfectedType.LegendaryBook, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: false, canDeleteManually: false, canCrossArchive: false, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 349, 5, 0, 0, -1, -1, 0, 0, "LegendaryBook.NpcEffect.Blade", 0, 0, -20, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5] { 0, 40, 0, 0, 0 }, new sbyte[5], new sbyte[5], 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 180, 180, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 90, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(350, 1723, 1724, hidden: false, ECharacterFeatureType.Special, 1725, 1726, 1727, new FeatureMedals[3]
		{
			new FeatureMedals(),
			new FeatureMedals(),
			new FeatureMedals("inc", "inc", "inc")
		}, 0, ECharacterFeatureInfectedType.LegendaryBook, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: false, canDeleteManually: false, canCrossArchive: false, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 350, 5, 0, 0, -1, -1, 0, 0, "LegendaryBook.NpcEffect.Polearm", 0, 0, -20, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5] { 0, 40, 0, 0, 0 }, new sbyte[5], new sbyte[5], 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 50, 50, 0, 0, 0, 180, 0, 0, 0, 180, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 90, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(351, 1728, 1729, hidden: false, ECharacterFeatureType.Special, 1730, 1731, 1732, new FeatureMedals[3]
		{
			new FeatureMedals(),
			new FeatureMedals(),
			new FeatureMedals("inc", "inc", "inc")
		}, 0, ECharacterFeatureInfectedType.LegendaryBook, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: false, canDeleteManually: false, canCrossArchive: false, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 351, 5, 0, 0, -1, -1, 0, 0, "LegendaryBook.NpcEffect.Special", 0, 0, -20, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5] { 0, 40, 0, 0, 0 }, new sbyte[5], new sbyte[5], 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 0, 0, 0, 0, 0, 0, 0, 180, 0, 0, 180, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 90, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(352, 1733, 1734, hidden: false, ECharacterFeatureType.Special, 1735, 1736, 1737, new FeatureMedals[3]
		{
			new FeatureMedals(),
			new FeatureMedals(),
			new FeatureMedals("inc", "inc", "inc")
		}, 0, ECharacterFeatureInfectedType.LegendaryBook, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: false, canDeleteManually: false, canCrossArchive: false, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 352, 5, 0, 0, -1, -1, 0, 0, "LegendaryBook.NpcEffect.Whip", 0, 0, -20, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5] { 0, 40, 0, 0, 0 }, new sbyte[5], new sbyte[5], 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 0, 0, 0, 0, 0, 0, 180, 0, 0, 180, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 90, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(353, 1738, 1739, hidden: false, ECharacterFeatureType.Special, 1740, 1741, 1742, new FeatureMedals[3]
		{
			new FeatureMedals(),
			new FeatureMedals(),
			new FeatureMedals("inc", "inc", "inc")
		}, 0, ECharacterFeatureInfectedType.LegendaryBook, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: false, canDeleteManually: false, canCrossArchive: false, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 353, 5, 0, 0, -1, -1, 0, 0, "LegendaryBook.NpcEffect.ControllableShot", 0, 0, -20, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5] { 0, 40, 0, 0, 0 }, new sbyte[5], new sbyte[5], 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 50, 50, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 180, 0, 0, 180, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 90, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(354, 1743, 1744, hidden: false, ECharacterFeatureType.Special, 1745, 1746, 1747, new FeatureMedals[3]
		{
			new FeatureMedals(),
			new FeatureMedals(),
			new FeatureMedals("inc", "inc", "inc")
		}, 0, ECharacterFeatureInfectedType.LegendaryBook, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: false, canDeleteManually: false, canCrossArchive: false, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 354, 5, 0, 0, -1, -1, 0, 0, "LegendaryBook.NpcEffect.CombatMusic", 0, 0, -20, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5] { 0, 40, 0, 0, 0 }, new sbyte[5], new sbyte[5], 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 0, 0, 0, 0, 0, 100, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 180, 180, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 90, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(355, 1678, 1748, hidden: false, ECharacterFeatureType.Temporary, 1680, 1749, 1682, new FeatureMedals[3]
		{
			new FeatureMedals(),
			new FeatureMedals(),
			new FeatureMedals("inc", "inc", "inc")
		}, 0, ECharacterFeatureInfectedType.LegendaryBook, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: false, canDeleteManually: false, canCrossArchive: false, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 341, 5, 0, 6, -1, -1, 0, 0, null, 0, 0, -10, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 20, 20, 0, 0, 0, 0, 20, 20, 60, 60, 0, 0, 0, 0, 0, 0, 60, 60, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 30, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(356, 1683, 1750, hidden: false, ECharacterFeatureType.Temporary, 1685, 1751, 1687, new FeatureMedals[3]
		{
			new FeatureMedals(),
			new FeatureMedals(),
			new FeatureMedals("inc", "inc", "inc")
		}, 0, ECharacterFeatureInfectedType.LegendaryBook, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: false, canDeleteManually: false, canCrossArchive: false, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 342, 5, 0, 6, -1, -1, 0, 0, null, 0, 0, -10, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 20, 20, 20, 20, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 60, 60, 0, 0, 60, 60, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 30, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(357, 1688, 1752, hidden: false, ECharacterFeatureType.Temporary, 1690, 1753, 1692, new FeatureMedals[3]
		{
			new FeatureMedals(),
			new FeatureMedals(),
			new FeatureMedals("inc", "inc", "inc")
		}, 0, ECharacterFeatureInfectedType.LegendaryBook, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: false, canDeleteManually: false, canCrossArchive: false, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 343, 5, 0, 6, -1, -1, 0, 0, null, 0, 0, -10, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 20, 20, 20, 20, 0, 0, 60, 60, 0, 0, 60, 60, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 30, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(358, 1693, 1754, hidden: false, ECharacterFeatureType.Temporary, 1695, 1755, 1697, new FeatureMedals[3]
		{
			new FeatureMedals(),
			new FeatureMedals(),
			new FeatureMedals("inc", "inc", "inc")
		}, 0, ECharacterFeatureInfectedType.LegendaryBook, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: false, canDeleteManually: false, canCrossArchive: false, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 344, 5, 0, 6, -1, -1, 0, 0, null, 0, 0, -10, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 40, 0, 0, 0, 0, 0, 90, 90, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 30, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(359, 1698, 1756, hidden: false, ECharacterFeatureType.Temporary, 1700, 1757, 1702, new FeatureMedals[3]
		{
			new FeatureMedals(),
			new FeatureMedals(),
			new FeatureMedals("inc", "inc", "inc")
		}, 0, ECharacterFeatureInfectedType.LegendaryBook, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: false, canDeleteManually: false, canCrossArchive: false, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 345, 5, 0, 6, -1, -1, 0, 0, null, 0, 0, -10, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 40, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 90, 90, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 30, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
	}

	private void CreateItems6()
	{
		_dataArray.Add(new CharacterFeatureItem(360, 1703, 1758, hidden: false, ECharacterFeatureType.Temporary, 1705, 1759, 1707, new FeatureMedals[3]
		{
			new FeatureMedals(),
			new FeatureMedals(),
			new FeatureMedals("inc", "inc", "inc")
		}, 0, ECharacterFeatureInfectedType.LegendaryBook, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: false, canDeleteManually: false, canCrossArchive: false, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 346, 5, 0, 6, -1, -1, 0, 0, null, 0, 0, -10, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 20, 20, 20, 0, 0, 0, 0, 0, 90, 90, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 30, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(361, 1708, 1760, hidden: false, ECharacterFeatureType.Temporary, 1710, 1761, 1712, new FeatureMedals[3]
		{
			new FeatureMedals(),
			new FeatureMedals(),
			new FeatureMedals("inc", "inc", "inc")
		}, 0, ECharacterFeatureInfectedType.LegendaryBook, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: false, canDeleteManually: false, canCrossArchive: false, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 347, 5, 0, 6, -1, -1, 0, 0, null, 0, 0, -10, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 40, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 90, 0, 0, 0, 90, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 30, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(362, 1713, 1762, hidden: false, ECharacterFeatureType.Temporary, 1715, 1763, 1717, new FeatureMedals[3]
		{
			new FeatureMedals(),
			new FeatureMedals(),
			new FeatureMedals("inc", "inc", "inc")
		}, 0, ECharacterFeatureInfectedType.LegendaryBook, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: false, canDeleteManually: false, canCrossArchive: false, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 348, 5, 0, 6, -1, -1, 0, 0, null, 0, 0, -10, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 20, 20, 20, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 90, 0, 0, 0, 90, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 30, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(363, 1718, 1764, hidden: false, ECharacterFeatureType.Temporary, 1720, 1765, 1722, new FeatureMedals[3]
		{
			new FeatureMedals(),
			new FeatureMedals(),
			new FeatureMedals("inc", "inc", "inc")
		}, 0, ECharacterFeatureInfectedType.LegendaryBook, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: false, canDeleteManually: false, canCrossArchive: false, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 349, 5, 0, 6, -1, -1, 0, 0, null, 0, 0, -10, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 40, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 90, 90, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 30, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(364, 1723, 1766, hidden: false, ECharacterFeatureType.Temporary, 1725, 1767, 1727, new FeatureMedals[3]
		{
			new FeatureMedals(),
			new FeatureMedals(),
			new FeatureMedals("inc", "inc", "inc")
		}, 0, ECharacterFeatureInfectedType.LegendaryBook, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: false, canDeleteManually: false, canCrossArchive: false, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 350, 5, 0, 6, -1, -1, 0, 0, null, 0, 0, -10, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 20, 20, 0, 0, 0, 90, 0, 0, 0, 90, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 30, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(365, 1728, 1768, hidden: false, ECharacterFeatureType.Temporary, 1730, 1769, 1732, new FeatureMedals[3]
		{
			new FeatureMedals(),
			new FeatureMedals(),
			new FeatureMedals("inc", "inc", "inc")
		}, 0, ECharacterFeatureInfectedType.LegendaryBook, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: false, canDeleteManually: false, canCrossArchive: false, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 351, 5, 0, 6, -1, -1, 0, 0, null, 0, 0, -10, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 40, 0, 0, 0, 0, 0, 0, 0, 90, 0, 0, 90, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 30, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(366, 1733, 1770, hidden: false, ECharacterFeatureType.Temporary, 1735, 1771, 1737, new FeatureMedals[3]
		{
			new FeatureMedals(),
			new FeatureMedals(),
			new FeatureMedals("inc", "inc", "inc")
		}, 0, ECharacterFeatureInfectedType.LegendaryBook, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: false, canDeleteManually: false, canCrossArchive: false, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 352, 5, 0, 6, -1, -1, 0, 0, null, 0, 0, -10, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 40, 0, 0, 0, 0, 0, 0, 90, 0, 0, 90, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 30, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(367, 1738, 1772, hidden: false, ECharacterFeatureType.Temporary, 1740, 1773, 1742, new FeatureMedals[3]
		{
			new FeatureMedals(),
			new FeatureMedals(),
			new FeatureMedals("inc", "inc", "inc")
		}, 0, ECharacterFeatureInfectedType.LegendaryBook, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: false, canDeleteManually: false, canCrossArchive: false, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 353, 5, 0, 6, -1, -1, 0, 0, null, 0, 0, -10, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 20, 20, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 90, 0, 0, 90, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 30, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(368, 1743, 1774, hidden: false, ECharacterFeatureType.Temporary, 1745, 1775, 1747, new FeatureMedals[3]
		{
			new FeatureMedals(),
			new FeatureMedals(),
			new FeatureMedals("inc", "inc", "inc")
		}, 0, ECharacterFeatureInfectedType.LegendaryBook, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: false, canDeleteManually: false, canCrossArchive: false, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 354, 5, 0, 6, -1, -1, 0, 0, null, 0, 0, -10, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 40, 0, 0, 0, 0, 0, 40, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 90, 90, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 30, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(369, 1678, 1776, hidden: false, ECharacterFeatureType.Special, 1680, 1777, 1682, new FeatureMedals[3]
		{
			new FeatureMedals(),
			new FeatureMedals(),
			new FeatureMedals("inc", "inc", "inc")
		}, 0, ECharacterFeatureInfectedType.LegendaryBook, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: false, canDeleteManually: false, canCrossArchive: false, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 341, 5, 0, 0, -1, -1, 0, 0, null, 0, 0, -20, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 75, 75, 0, 0, 0, 0, 75, 75, 180, 180, 0, 0, 0, 0, 0, 0, 180, 180, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 90, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(370, 1683, 1778, hidden: false, ECharacterFeatureType.Special, 1685, 1779, 1687, new FeatureMedals[3]
		{
			new FeatureMedals(),
			new FeatureMedals(),
			new FeatureMedals("inc", "inc", "inc")
		}, 0, ECharacterFeatureInfectedType.LegendaryBook, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: false, canDeleteManually: false, canCrossArchive: false, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 342, 5, 0, 0, -1, -1, 0, 0, null, 0, 0, -20, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 75, 75, 75, 75, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 180, 180, 0, 0, 180, 180, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 90, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(371, 1688, 1780, hidden: false, ECharacterFeatureType.Special, 1690, 1781, 1692, new FeatureMedals[3]
		{
			new FeatureMedals(),
			new FeatureMedals(),
			new FeatureMedals("inc", "inc", "inc")
		}, 0, ECharacterFeatureInfectedType.LegendaryBook, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: false, canDeleteManually: false, canCrossArchive: false, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 343, 5, 0, 0, -1, -1, 0, 0, null, 0, 0, -20, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 75, 75, 75, 75, 0, 0, 180, 180, 0, 0, 180, 180, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 90, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(372, 1693, 1782, hidden: false, ECharacterFeatureType.Special, 1695, 1783, 1697, new FeatureMedals[3]
		{
			new FeatureMedals(),
			new FeatureMedals(),
			new FeatureMedals("inc", "inc", "inc")
		}, 0, ECharacterFeatureInfectedType.LegendaryBook, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: false, canDeleteManually: false, canCrossArchive: false, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 344, 5, 0, 0, -1, -1, 0, 0, null, 0, 0, -20, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 150, 0, 0, 0, 0, 0, 270, 270, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 90, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(373, 1698, 1784, hidden: false, ECharacterFeatureType.Special, 1700, 1785, 1702, new FeatureMedals[3]
		{
			new FeatureMedals(),
			new FeatureMedals(),
			new FeatureMedals("inc", "inc", "inc")
		}, 0, ECharacterFeatureInfectedType.LegendaryBook, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: false, canDeleteManually: false, canCrossArchive: false, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 345, 5, 0, 0, -1, -1, 0, 0, null, 0, 0, -20, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 150, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 270, 270, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 90, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(374, 1703, 1786, hidden: false, ECharacterFeatureType.Special, 1705, 1787, 1707, new FeatureMedals[3]
		{
			new FeatureMedals(),
			new FeatureMedals(),
			new FeatureMedals("inc", "inc", "inc")
		}, 0, ECharacterFeatureInfectedType.LegendaryBook, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: false, canDeleteManually: false, canCrossArchive: false, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 346, 5, 0, 0, -1, -1, 0, 0, null, 0, 0, -20, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 75, 75, 75, 0, 0, 0, 0, 0, 270, 270, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 90, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(375, 1708, 1788, hidden: false, ECharacterFeatureType.Special, 1710, 1789, 1712, new FeatureMedals[3]
		{
			new FeatureMedals(),
			new FeatureMedals(),
			new FeatureMedals("inc", "inc", "inc")
		}, 0, ECharacterFeatureInfectedType.LegendaryBook, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: false, canDeleteManually: false, canCrossArchive: false, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 347, 5, 0, 0, -1, -1, 0, 0, null, 0, 0, -20, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 150, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 270, 0, 0, 0, 270, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 90, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(376, 1713, 1790, hidden: false, ECharacterFeatureType.Special, 1715, 1791, 1717, new FeatureMedals[3]
		{
			new FeatureMedals(),
			new FeatureMedals(),
			new FeatureMedals("inc", "inc", "inc")
		}, 0, ECharacterFeatureInfectedType.LegendaryBook, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: false, canDeleteManually: false, canCrossArchive: false, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 348, 5, 0, 0, -1, -1, 0, 0, null, 0, 0, -20, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 75, 75, 75, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 270, 0, 0, 0, 270, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 90, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(377, 1718, 1792, hidden: false, ECharacterFeatureType.Special, 1720, 1793, 1722, new FeatureMedals[3]
		{
			new FeatureMedals(),
			new FeatureMedals(),
			new FeatureMedals("inc", "inc", "inc")
		}, 0, ECharacterFeatureInfectedType.LegendaryBook, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: false, canDeleteManually: false, canCrossArchive: false, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 349, 5, 0, 0, -1, -1, 0, 0, null, 0, 0, -20, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 150, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 270, 270, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 90, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(378, 1723, 1794, hidden: false, ECharacterFeatureType.Special, 1725, 1795, 1727, new FeatureMedals[3]
		{
			new FeatureMedals(),
			new FeatureMedals(),
			new FeatureMedals("inc", "inc", "inc")
		}, 0, ECharacterFeatureInfectedType.LegendaryBook, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: false, canDeleteManually: false, canCrossArchive: false, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 350, 5, 0, 0, -1, -1, 0, 0, null, 0, 0, -20, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 75, 75, 0, 0, 0, 270, 0, 0, 0, 270, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 90, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(379, 1728, 1796, hidden: false, ECharacterFeatureType.Special, 1730, 1797, 1732, new FeatureMedals[3]
		{
			new FeatureMedals(),
			new FeatureMedals(),
			new FeatureMedals("inc", "inc", "inc")
		}, 0, ECharacterFeatureInfectedType.LegendaryBook, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: false, canDeleteManually: false, canCrossArchive: false, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 351, 5, 0, 0, -1, -1, 0, 0, null, 0, 0, -20, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 150, 0, 0, 0, 0, 0, 0, 0, 270, 0, 0, 270, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 90, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(380, 1733, 1798, hidden: false, ECharacterFeatureType.Special, 1735, 1799, 1737, new FeatureMedals[3]
		{
			new FeatureMedals(),
			new FeatureMedals(),
			new FeatureMedals("inc", "inc", "inc")
		}, 0, ECharacterFeatureInfectedType.LegendaryBook, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: false, canDeleteManually: false, canCrossArchive: false, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 352, 5, 0, 0, -1, -1, 0, 0, null, 0, 0, -20, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 150, 0, 0, 0, 0, 0, 0, 270, 0, 0, 270, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 90, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(381, 1738, 1800, hidden: false, ECharacterFeatureType.Special, 1740, 1801, 1742, new FeatureMedals[3]
		{
			new FeatureMedals(),
			new FeatureMedals(),
			new FeatureMedals("inc", "inc", "inc")
		}, 0, ECharacterFeatureInfectedType.LegendaryBook, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: false, canDeleteManually: false, canCrossArchive: false, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 353, 5, 0, 0, -1, -1, 0, 0, null, 0, 0, -20, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 75, 75, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 270, 0, 0, 270, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 90, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(382, 1743, 1802, hidden: false, ECharacterFeatureType.Special, 1745, 1803, 1747, new FeatureMedals[3]
		{
			new FeatureMedals(),
			new FeatureMedals(),
			new FeatureMedals("inc", "inc", "inc")
		}, 0, ECharacterFeatureInfectedType.LegendaryBook, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: false, canDeleteManually: false, canCrossArchive: false, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 354, 5, 0, 0, -1, -1, 0, 0, null, 0, 0, -20, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 150, 0, 0, 0, 0, 0, 150, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 270, 270, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 90, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(383, 1804, 1805, hidden: false, ECharacterFeatureType.Special, 1806, 1807, 1808, new FeatureMedals[3]
		{
			new FeatureMedals("inc"),
			new FeatureMedals("inc"),
			new FeatureMedals("inc")
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: true, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 383, 501, 0, 1, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 100, 10, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(384, 1809, 1810, hidden: false, ECharacterFeatureType.Special, 1811, 1812, 1813, new FeatureMedals[3]
		{
			new FeatureMedals("inc"),
			new FeatureMedals("inc"),
			new FeatureMedals("inc")
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: true, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 384, 502, 0, 0, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(385, 1814, 1815, hidden: false, ECharacterFeatureType.Special, 1816, 1817, 1818, new FeatureMedals[3]
		{
			new FeatureMedals("inc"),
			new FeatureMedals("inc"),
			new FeatureMedals("inc")
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: true, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 385, 503, 0, 0, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 150, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(386, 1819, 1820, hidden: false, ECharacterFeatureType.Special, 1821, 1822, 1823, new FeatureMedals[3]
		{
			new FeatureMedals("inc"),
			new FeatureMedals("inc"),
			new FeatureMedals("inc")
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: true, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 386, 504, 0, 0, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(387, 1824, 1825, hidden: false, ECharacterFeatureType.Special, 1826, 1827, 1828, new FeatureMedals[3]
		{
			new FeatureMedals(),
			new FeatureMedals("dec"),
			new FeatureMedals()
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: false, canDeleteManually: false, canCrossArchive: false, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 387, 601, 0, 0, -1, -1, 0, 0, null, 0, 0, 0, 0, -10, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], -100, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -50, 0, 0, 0, 0, 0, -50, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(388, 1829, 1830, hidden: false, ECharacterFeatureType.Special, 1831, 1832, 1833, new FeatureMedals[3]
		{
			new FeatureMedals("dec"),
			new FeatureMedals(),
			new FeatureMedals()
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: false, canDeleteManually: false, canCrossArchive: false, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 388, 602, 0, 0, -1, -1, 0, 0, null, 0, 0, 0, 0, -10, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, -100, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -50, 0, 0, 0, 0, 0, -50, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(389, 1834, 1835, hidden: false, ECharacterFeatureType.Special, 1836, 1837, 1838, new FeatureMedals[3]
		{
			new FeatureMedals(),
			new FeatureMedals(),
			new FeatureMedals("dec")
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: false, canDeleteManually: false, canCrossArchive: false, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 389, 603, 0, 0, -1, -1, 0, 0, null, 0, 0, 0, 0, -10, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, -100, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -50, 0, 0, 0, 0, 0, -50, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(390, 1839, 1840, hidden: false, ECharacterFeatureType.Special, 1841, 1842, 1843, new FeatureMedals[3]
		{
			new FeatureMedals(),
			new FeatureMedals("dec"),
			new FeatureMedals()
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: false, canDeleteManually: false, canCrossArchive: false, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 390, 604, 0, 0, -1, -1, 0, 0, null, 0, 0, 0, 0, -10, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, -100, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -50, 0, 0, 0, 0, 0, -50, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(391, 1844, 1845, hidden: false, ECharacterFeatureType.Special, 1846, 1847, 1848, new FeatureMedals[3]
		{
			new FeatureMedals("dec"),
			new FeatureMedals(),
			new FeatureMedals()
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: false, canDeleteManually: false, canCrossArchive: false, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 391, 605, 0, 0, -1, -1, 0, 0, null, 0, 0, 0, 0, -10, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 0, -100, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -9999, -9999, -9999, -9999, -9999, -9999, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(392, 1849, 1850, hidden: false, ECharacterFeatureType.Special, 1851, 1852, 1853, new FeatureMedals[3]
		{
			new FeatureMedals(),
			new FeatureMedals(),
			new FeatureMedals("dec")
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: false, canDeleteManually: false, canCrossArchive: false, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 392, 606, 0, 0, -1, -1, 0, 0, null, 0, 0, 0, 0, -10, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 0, 0, 0, -100, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -50, -50, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(393, 1854, 1855, hidden: false, ECharacterFeatureType.Special, 1856, 1857, 1858, new FeatureMedals[3]
		{
			new FeatureMedals("dec"),
			new FeatureMedals("dec"),
			new FeatureMedals("dec")
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: false, canDeleteManually: false, canCrossArchive: false, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 393, 607, 0, 0, -1, -1, 0, 0, null, 0, 0, 0, 0, -10, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 0, 0, -100, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -50, -50, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(394, 1859, 1860, hidden: false, ECharacterFeatureType.Special, 1861, 1862, 1266, new FeatureMedals[3]
		{
			new FeatureMedals("dec"),
			new FeatureMedals("dec"),
			new FeatureMedals("dec")
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: true, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 257, 103, 0, 0, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 100, 200, 2000, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5] { 1, 0, 0, 0, 0 }, 0, 0, 0, 0, 0, 0, -100, 0, 0, 0, 0, 0, 0, 100, 0, 300, 0, 0, 0, 20, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(395, 1863, 1864, hidden: false, ECharacterFeatureType.Special, 1865, 1866, 1266, new FeatureMedals[3]
		{
			new FeatureMedals("dec"),
			new FeatureMedals("dec"),
			new FeatureMedals("dec")
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: true, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 257, 103, 0, 0, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 100, 200, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5] { 0, 1, 0, 0, 0 }, 0, 0, 0, 0, 0, -100, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 20, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 12, 12, 12, 12, 12, 12, 12, 12, 12, 12, 12, 12, 12, 12, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(396, 1867, 1868, hidden: false, ECharacterFeatureType.Special, 1869, 1870, 1266, new FeatureMedals[3]
		{
			new FeatureMedals("dec"),
			new FeatureMedals("dec"),
			new FeatureMedals("dec")
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: true, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 257, 103, 0, 0, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 100, 200, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5] { 0, 0, 1, 0, 0 }, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 20, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 12, 12, 12, 12, 12, 12, 12, 12, 12, 12, 12, 12, 12, 12, 12, 12, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(397, 1871, 1872, hidden: false, ECharacterFeatureType.Special, 1873, 1874, 1266, new FeatureMedals[3]
		{
			new FeatureMedals("dec"),
			new FeatureMedals("dec"),
			new FeatureMedals("dec")
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: true, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 257, 103, 0, 0, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 100, 200, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5] { 0, 0, 0, 1, 0 }, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 20, 20, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 100, 100, 100, 100, 100, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(398, 1875, 1876, hidden: false, ECharacterFeatureType.Special, 1877, 1878, 1266, new FeatureMedals[3]
		{
			new FeatureMedals("dec"),
			new FeatureMedals("dec"),
			new FeatureMedals("dec")
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: true, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 257, 103, 0, 0, -1, -1, 0, 0, null, 0, 0, 0, 0, 100, 0, 100, 200, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5] { 0, 0, 0, 0, 1 }, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 20, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(399, 1879, 1880, hidden: false, ECharacterFeatureType.Special, 1881, 1882, 1883, new FeatureMedals[3]
		{
			new FeatureMedals(),
			new FeatureMedals(),
			new FeatureMedals("neg", "neg", "neg")
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: true, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: false, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 220, 6, 0, 0, -1, -1, 0, 0, null, 0, 0, -20, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 100, 100, -100, -100, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(400, 1884, 1885, hidden: false, ECharacterFeatureType.Special, 1886, 1887, 1888, new FeatureMedals[3]
		{
			new FeatureMedals(),
			new FeatureMedals(),
			new FeatureMedals("inc", "inc", "inc")
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: true, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: false, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 220, 6, 0, 0, -1, -1, 0, 0, null, 0, 0, -40, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 100, 0, -100, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(401, 1889, 1890, hidden: false, ECharacterFeatureType.Special, 1891, 1892, 1893, new FeatureMedals[3]
		{
			new FeatureMedals(),
			new FeatureMedals("inc", "inc", "inc"),
			new FeatureMedals()
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: false, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 220, 6, 0, 0, -1, -1, 0, 0, null, 0, 0, -20, 0, 10000, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(402, 1894, 1895, hidden: false, ECharacterFeatureType.Special, 1896, 1897, 1898, new FeatureMedals[3]
		{
			new FeatureMedals("pos"),
			new FeatureMedals("pos"),
			new FeatureMedals("pos")
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: true, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: false, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 220, 6, 0, 0, -1, -1, 0, 0, null, 0, 0, -40, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 100, 100, 100, 100, 100, 100, 100, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(403, 1899, 1900, hidden: false, ECharacterFeatureType.Special, 1901, 1902, 1903, new FeatureMedals[3]
		{
			new FeatureMedals("inc"),
			new FeatureMedals("inc"),
			new FeatureMedals("inc")
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: false, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, 17, 403, 104, 0, 0, -1, -1, 0, 0, null, 0, 0, -20, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5] { 40, 40, 40, 40, 40 }, new sbyte[5], new sbyte[5], 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 40, 40, 40, 40, 40, 40, 40, 40, 40, 40, 40, 40, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(404, 1904, 1905, hidden: false, ECharacterFeatureType.Special, 1906, 1907, 1908, new FeatureMedals[3]
		{
			new FeatureMedals("pos"),
			new FeatureMedals("pos"),
			new FeatureMedals("pos")
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: true, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: false, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 220, 6, 0, 0, -1, -1, 0, 0, null, 0, 80, -40, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(405, 1909, 1910, hidden: false, ECharacterFeatureType.Special, 1911, 1912, 1913, new FeatureMedals[3]
		{
			new FeatureMedals("inc"),
			new FeatureMedals("inc"),
			new FeatureMedals("inc")
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: false, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 405, 104, 0, 0, -1, -1, 0, 0, null, 0, 0, -20, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5] { 80, 80, 80, 80, 80 }, new sbyte[5], new sbyte[5], 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 40, 40, 40, 40, 40, 40, 40, 40, 40, 40, 40, 40, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(406, 1914, 1915, hidden: false, ECharacterFeatureType.Special, 1916, 1917, 1918, new FeatureMedals[3]
		{
			new FeatureMedals("pos", "pos", "pos"),
			new FeatureMedals(),
			new FeatureMedals()
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: false, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, 18, 406, 104, 0, 0, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 0, 0, 0, 10, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(407, 1919, 1920, hidden: false, ECharacterFeatureType.Special, 1921, 1922, 1923, new FeatureMedals[3]
		{
			new FeatureMedals(),
			new FeatureMedals("pos", "pos", "pos"),
			new FeatureMedals()
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: false, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, 18, 406, 104, 0, 0, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5] { 10, 10, 10, 10, 10 }, new sbyte[5], new sbyte[5], 0, 0, 0, 0, 0, 0, 15, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(408, 1924, 1925, hidden: false, ECharacterFeatureType.Special, 1926, 1927, 1928, new FeatureMedals[3]
		{
			new FeatureMedals(),
			new FeatureMedals(),
			new FeatureMedals("pos", "pos", "pos")
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: false, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, 18, 406, 104, 0, 0, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5] { 20, 20, 20, 20, 20 }, new sbyte[5], new sbyte[5], 0, 0, 0, 0, 0, 0, 20, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(409, 1929, 1930, hidden: false, ECharacterFeatureType.Special, 1931, 1932, 1933, new FeatureMedals[3]
		{
			new FeatureMedals("pos"),
			new FeatureMedals("pos"),
			new FeatureMedals("pos")
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: false, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, 18, 406, 104, 0, 0, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5] { 30, 30, 30, 30, 30 }, new sbyte[5], new sbyte[5], 0, 0, 0, 0, 0, 0, 25, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(410, 1934, 1935, hidden: false, ECharacterFeatureType.Special, 1936, 1937, 1938, new FeatureMedals[3]
		{
			new FeatureMedals("pos"),
			new FeatureMedals("pos"),
			new FeatureMedals("pos")
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: false, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, 18, 406, 104, 0, 0, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5] { 40, 40, 40, 40, 40 }, new sbyte[5], new sbyte[5], 0, 0, 0, 0, 0, 0, 30, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(411, 1939, 1940, hidden: false, ECharacterFeatureType.Special, 1941, 1942, 1943, new FeatureMedals[3]
		{
			new FeatureMedals(),
			new FeatureMedals(),
			new FeatureMedals("pos", "pos", "pos")
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: false, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, 18, 406, 104, 0, 0, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5] { 50, 50, 50, 50, 50 }, new sbyte[5], new sbyte[5], 0, 0, 0, 0, 0, 0, 35, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(412, 1944, 1945, hidden: false, ECharacterFeatureType.Special, 1946, 1947, 1948, new FeatureMedals[3]
		{
			new FeatureMedals("pos", "pos", "pos"),
			new FeatureMedals(),
			new FeatureMedals()
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: false, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, 18, 406, 104, 0, 0, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5] { 60, 60, 60, 60, 60 }, new sbyte[5], new sbyte[5], 0, 0, 0, 0, 0, 0, 40, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(413, 1949, 1950, hidden: false, ECharacterFeatureType.Special, 1951, 1952, 1953, new FeatureMedals[3]
		{
			new FeatureMedals(),
			new FeatureMedals("pos", "pos", "pos"),
			new FeatureMedals()
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: false, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, 18, 406, 104, 0, 0, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5] { 70, 70, 70, 70, 70 }, new sbyte[5], new sbyte[5], 0, 0, 0, 0, 0, 0, 45, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(414, 1954, 1955, hidden: false, ECharacterFeatureType.Special, 1956, 1957, 1958, new FeatureMedals[3]
		{
			new FeatureMedals("pos"),
			new FeatureMedals("pos"),
			new FeatureMedals("pos")
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: false, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, 18, 406, 104, 0, 0, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5] { 80, 80, 80, 80, 80 }, new sbyte[5], new sbyte[5], 0, 0, 0, 0, 0, 0, 50, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(415, 1959, 1960, hidden: false, ECharacterFeatureType.Special, 1961, 1962, 1963, new FeatureMedals[3]
		{
			new FeatureMedals("neg"),
			new FeatureMedals("neg"),
			new FeatureMedals("neg")
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: true, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 415, 6, 0, 0, -1, -1, 0, 50, null, 0, 0, 0, -10, -50, 0, 20, 20, 2000, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], -20, -20, -20, -20, -20, -20, -20, 0, 0, 0, 0, 0, 0, -50, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(416, 1964, 1965, hidden: false, ECharacterFeatureType.Special, 1966, 1967, 1968, new FeatureMedals[3]
		{
			new FeatureMedals("inc"),
			new FeatureMedals("inc"),
			new FeatureMedals("inc")
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: false, canDeleteManually: false, canCrossArchive: false, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 416, 7001, 0, 0, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 0, 0, 30, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(417, 1969, 1970, hidden: false, ECharacterFeatureType.Special, 1971, 1972, 1973, new FeatureMedals[3]
		{
			new FeatureMedals("inc"),
			new FeatureMedals("inc"),
			new FeatureMedals("inc")
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: false, canDeleteManually: false, canCrossArchive: false, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 416, 7001, 0, 0, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 0, 0, 0, 30, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(418, 1974, 1975, hidden: false, ECharacterFeatureType.Special, 1976, 1977, 1978, new FeatureMedals[3]
		{
			new FeatureMedals(),
			new FeatureMedals("inc"),
			new FeatureMedals()
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: false, canDeleteManually: false, canCrossArchive: false, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 416, 7001, 0, 0, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 30, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(419, 1979, 1980, hidden: false, ECharacterFeatureType.Special, 1981, 1982, 1983, new FeatureMedals[3]
		{
			new FeatureMedals(),
			new FeatureMedals(),
			new FeatureMedals("inc")
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: false, canDeleteManually: false, canCrossArchive: false, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 416, 7001, 0, 0, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 30, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
	}

	private void CreateItems7()
	{
		_dataArray.Add(new CharacterFeatureItem(420, 1984, 1985, hidden: false, ECharacterFeatureType.Special, 1986, 1987, 1988, new FeatureMedals[3]
		{
			new FeatureMedals(),
			new FeatureMedals(),
			new FeatureMedals("inc")
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: false, canDeleteManually: false, canCrossArchive: false, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 416, 7001, 0, 0, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 30, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(421, 1989, 1990, hidden: false, ECharacterFeatureType.Special, 1991, 1992, 1993, new FeatureMedals[3]
		{
			new FeatureMedals(),
			new FeatureMedals("inc"),
			new FeatureMedals()
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: false, canDeleteManually: false, canCrossArchive: false, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 416, 7001, 0, 0, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 30, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(422, 1994, 1995, hidden: false, ECharacterFeatureType.Special, 1996, 1997, 1998, new FeatureMedals[3]
		{
			new FeatureMedals(),
			new FeatureMedals("inc"),
			new FeatureMedals()
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: false, canDeleteManually: false, canCrossArchive: false, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 416, 7001, 0, 0, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 0, 30, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(423, 1999, 2000, hidden: false, ECharacterFeatureType.Special, 2001, 2002, 2003, new FeatureMedals[3]
		{
			new FeatureMedals("inc"),
			new FeatureMedals(),
			new FeatureMedals()
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: false, canDeleteManually: false, canCrossArchive: false, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 416, 7001, 0, 0, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 30, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(424, 2004, 2005, hidden: false, ECharacterFeatureType.Special, 2006, 2007, 2008, new FeatureMedals[3]
		{
			new FeatureMedals(),
			new FeatureMedals(),
			new FeatureMedals("inc")
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: false, canDeleteManually: false, canCrossArchive: false, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 416, 7001, 0, 0, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 30, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(425, 2009, 2010, hidden: false, ECharacterFeatureType.Special, 2011, 2012, 2013, new FeatureMedals[3]
		{
			new FeatureMedals(),
			new FeatureMedals("inc"),
			new FeatureMedals()
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: false, canDeleteManually: false, canCrossArchive: false, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 416, 7001, 0, 0, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 30, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(426, 2014, 2015, hidden: false, ECharacterFeatureType.Special, 2016, 2017, 2018, new FeatureMedals[3]
		{
			new FeatureMedals("inc"),
			new FeatureMedals(),
			new FeatureMedals()
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: false, canDeleteManually: false, canCrossArchive: false, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 416, 7001, 0, 0, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 30, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(427, 2019, 2020, hidden: false, ECharacterFeatureType.Special, 2021, 2022, 2023, new FeatureMedals[3]
		{
			new FeatureMedals(),
			new FeatureMedals("inc"),
			new FeatureMedals()
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: false, canDeleteManually: false, canCrossArchive: false, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 416, 7001, 0, 0, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 0, 30, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(428, 2024, 2025, hidden: false, ECharacterFeatureType.Special, 2026, 2027, 2028, new FeatureMedals[3]
		{
			new FeatureMedals("inc"),
			new FeatureMedals(),
			new FeatureMedals()
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: false, canDeleteManually: false, canCrossArchive: false, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 416, 7001, 0, 0, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 0, 30, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(429, 2029, 2030, hidden: false, ECharacterFeatureType.Special, 2031, 2032, 2033, new FeatureMedals[3]
		{
			new FeatureMedals(),
			new FeatureMedals(),
			new FeatureMedals("inc")
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: false, canDeleteManually: false, canCrossArchive: false, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 416, 7001, 0, 0, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 30, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(430, 2034, 2035, hidden: false, ECharacterFeatureType.Special, 2036, 2037, 2038, new FeatureMedals[3]
		{
			new FeatureMedals(),
			new FeatureMedals(),
			new FeatureMedals("inc")
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: false, canDeleteManually: false, canCrossArchive: false, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 416, 7001, 0, 0, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 30, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(431, 2039, 2040, hidden: false, ECharacterFeatureType.Special, 2041, 2042, 2043, new FeatureMedals[3]
		{
			new FeatureMedals("inc"),
			new FeatureMedals(),
			new FeatureMedals()
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: false, canDeleteManually: false, canCrossArchive: false, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 416, 7001, 0, 0, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 30, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(432, 2044, 2045, hidden: false, ECharacterFeatureType.Special, 2046, 2047, 2048, new FeatureMedals[3]
		{
			new FeatureMedals("inc"),
			new FeatureMedals(),
			new FeatureMedals()
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: false, canDeleteManually: false, canCrossArchive: false, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 416, 7001, 0, 0, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 30, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(433, 2049, 2050, hidden: false, ECharacterFeatureType.Special, 2051, 2052, 2053, new FeatureMedals[3]
		{
			new FeatureMedals(),
			new FeatureMedals("inc"),
			new FeatureMedals()
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: false, canDeleteManually: false, canCrossArchive: false, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 416, 7001, 0, 0, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 0, 30, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(434, 2054, 2055, hidden: false, ECharacterFeatureType.Special, 2056, 2057, 2058, new FeatureMedals[3]
		{
			new FeatureMedals(),
			new FeatureMedals(),
			new FeatureMedals("inc")
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: false, canDeleteManually: false, canCrossArchive: false, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 416, 7001, 0, 0, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 30, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(435, 2059, 2060, hidden: false, ECharacterFeatureType.Special, 2061, 2062, 2063, new FeatureMedals[3]
		{
			new FeatureMedals(),
			new FeatureMedals(),
			new FeatureMedals("inc")
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: false, canDeleteManually: false, canCrossArchive: false, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 416, 7001, 0, 0, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 30, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(436, 2064, 2065, hidden: false, ECharacterFeatureType.Special, 2066, 2067, 2068, new FeatureMedals[3]
		{
			new FeatureMedals(),
			new FeatureMedals("inc"),
			new FeatureMedals()
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: false, canDeleteManually: false, canCrossArchive: false, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 416, 7001, 0, 0, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 0, 30, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(437, 2069, 2070, hidden: false, ECharacterFeatureType.Special, 2071, 2072, 2073, new FeatureMedals[3]
		{
			new FeatureMedals("inc"),
			new FeatureMedals(),
			new FeatureMedals()
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: false, canDeleteManually: false, canCrossArchive: false, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 416, 7001, 0, 0, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 30, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(438, 2074, 2075, hidden: false, ECharacterFeatureType.Special, 2076, 2077, 2078, new FeatureMedals[3]
		{
			new FeatureMedals("inc"),
			new FeatureMedals(),
			new FeatureMedals()
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: false, canDeleteManually: false, canCrossArchive: false, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 416, 7001, 0, 0, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 30, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(439, 2079, 2080, hidden: false, ECharacterFeatureType.Special, 2081, 2082, 2083, new FeatureMedals[3]
		{
			new FeatureMedals(),
			new FeatureMedals(),
			new FeatureMedals("inc")
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: false, canDeleteManually: false, canCrossArchive: false, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 416, 7001, 0, 0, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 30, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(440, 2084, 2085, hidden: false, ECharacterFeatureType.Special, 2086, 2087, 2088, new FeatureMedals[3]
		{
			new FeatureMedals(),
			new FeatureMedals("inc"),
			new FeatureMedals()
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: false, canDeleteManually: false, canCrossArchive: false, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 416, 7001, 0, 0, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 30, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(441, 2089, 2090, hidden: false, ECharacterFeatureType.Special, 2091, 2092, 2093, new FeatureMedals[3]
		{
			new FeatureMedals("inc"),
			new FeatureMedals(),
			new FeatureMedals()
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: false, canDeleteManually: false, canCrossArchive: false, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 416, 7001, 0, 0, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 30, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(442, 2094, 2095, hidden: false, ECharacterFeatureType.Special, 2096, 2097, 2098, new FeatureMedals[3]
		{
			new FeatureMedals("inc"),
			new FeatureMedals(),
			new FeatureMedals()
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: false, canDeleteManually: false, canCrossArchive: false, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 416, 7001, 0, 0, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 30, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(443, 2099, 2100, hidden: false, ECharacterFeatureType.Special, 2101, 2102, 2103, new FeatureMedals[3]
		{
			new FeatureMedals("inc"),
			new FeatureMedals(),
			new FeatureMedals()
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: false, canDeleteManually: false, canCrossArchive: false, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 416, 7001, 0, 0, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 30, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(444, 2104, 2105, hidden: false, ECharacterFeatureType.Special, 2106, 2107, 2108, new FeatureMedals[3]
		{
			new FeatureMedals(),
			new FeatureMedals(),
			new FeatureMedals("inc")
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: false, canDeleteManually: false, canCrossArchive: false, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 416, 7001, 0, 0, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 0, 30, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(445, 2109, 2110, hidden: false, ECharacterFeatureType.Special, 2111, 2112, 2113, new FeatureMedals[3]
		{
			new FeatureMedals(),
			new FeatureMedals("inc"),
			new FeatureMedals()
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: false, canDeleteManually: false, canCrossArchive: false, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 416, 7001, 0, 0, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 30, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(446, 2114, 2115, hidden: false, ECharacterFeatureType.Special, 2116, 2117, 2118, new FeatureMedals[3]
		{
			new FeatureMedals(),
			new FeatureMedals("inc"),
			new FeatureMedals()
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: false, canDeleteManually: false, canCrossArchive: false, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 416, 7001, 0, 0, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 30, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(447, 2119, 2120, hidden: false, ECharacterFeatureType.Special, 2121, 2122, 2123, new FeatureMedals[3]
		{
			new FeatureMedals(),
			new FeatureMedals(),
			new FeatureMedals("inc")
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: false, canDeleteManually: false, canCrossArchive: false, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 416, 7001, 0, 0, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 30, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(448, 2124, 2125, hidden: false, ECharacterFeatureType.Special, 2126, 2127, 2128, new FeatureMedals[3]
		{
			new FeatureMedals(),
			new FeatureMedals("inc"),
			new FeatureMedals()
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: false, canDeleteManually: false, canCrossArchive: false, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 416, 7001, 0, 0, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 0, 30, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(449, 2129, 2130, hidden: false, ECharacterFeatureType.Special, 2131, 2132, 2133, new FeatureMedals[3]
		{
			new FeatureMedals(),
			new FeatureMedals(),
			new FeatureMedals("inc")
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: false, canDeleteManually: false, canCrossArchive: false, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 416, 7001, 0, 0, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 30, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(450, 2134, 2135, hidden: false, ECharacterFeatureType.Special, 2136, 2137, 2138, new FeatureMedals[3]
		{
			new FeatureMedals(),
			new FeatureMedals(),
			new FeatureMedals("inc")
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: false, canDeleteManually: false, canCrossArchive: false, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 416, 7001, 0, 0, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 30, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(451, 2139, 2140, hidden: false, ECharacterFeatureType.Special, 2141, 2142, 2143, new FeatureMedals[3]
		{
			new FeatureMedals(),
			new FeatureMedals("inc"),
			new FeatureMedals()
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: false, canDeleteManually: false, canCrossArchive: false, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 416, 7001, 0, 0, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 0, 30, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(452, 2144, 2145, hidden: false, ECharacterFeatureType.Special, 2146, 2147, 2148, new FeatureMedals[3]
		{
			new FeatureMedals("inc"),
			new FeatureMedals(),
			new FeatureMedals()
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: false, canDeleteManually: false, canCrossArchive: false, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 416, 7001, 0, 0, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 30, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(453, 2149, 2150, hidden: false, ECharacterFeatureType.Special, 2151, 2152, 2153, new FeatureMedals[3]
		{
			new FeatureMedals("inc"),
			new FeatureMedals(),
			new FeatureMedals()
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: false, canDeleteManually: false, canCrossArchive: false, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 416, 7001, 0, 0, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 30, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(454, 2154, 2155, hidden: false, ECharacterFeatureType.Special, 2156, 2157, 2158, new FeatureMedals[3]
		{
			new FeatureMedals(),
			new FeatureMedals(),
			new FeatureMedals("inc")
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: false, canDeleteManually: false, canCrossArchive: false, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 416, 7001, 0, 0, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 30, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(455, 2159, 2160, hidden: false, ECharacterFeatureType.Special, 2161, 2162, 2163, new FeatureMedals[3]
		{
			new FeatureMedals(),
			new FeatureMedals("inc"),
			new FeatureMedals()
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: false, canDeleteManually: false, canCrossArchive: false, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 416, 7001, 0, 0, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 30, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(456, 2164, 2165, hidden: false, ECharacterFeatureType.Special, 2166, 2167, 2168, new FeatureMedals[3]
		{
			new FeatureMedals("inc"),
			new FeatureMedals(),
			new FeatureMedals()
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: false, canDeleteManually: false, canCrossArchive: false, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 416, 7001, 0, 0, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 30, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(457, 2169, 2170, hidden: false, ECharacterFeatureType.Special, 2171, 2172, 2173, new FeatureMedals[3]
		{
			new FeatureMedals("inc"),
			new FeatureMedals(),
			new FeatureMedals()
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: false, canDeleteManually: false, canCrossArchive: false, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 416, 7001, 0, 0, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 30, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(458, 2174, 2175, hidden: false, ECharacterFeatureType.Special, 2176, 2177, 2178, new FeatureMedals[3]
		{
			new FeatureMedals("inc"),
			new FeatureMedals(),
			new FeatureMedals()
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: false, canDeleteManually: false, canCrossArchive: false, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 416, 7001, 0, 0, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 30, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(459, 2179, 2180, hidden: false, ECharacterFeatureType.Special, 2181, 2182, 2183, new FeatureMedals[3]
		{
			new FeatureMedals(),
			new FeatureMedals(),
			new FeatureMedals("inc")
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: false, canDeleteManually: false, canCrossArchive: false, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 416, 7001, 0, 0, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 0, 30, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(460, 2184, 2185, hidden: false, ECharacterFeatureType.Special, 2186, 2187, 2188, new FeatureMedals[3]
		{
			new FeatureMedals(),
			new FeatureMedals("inc"),
			new FeatureMedals()
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: false, canDeleteManually: false, canCrossArchive: false, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 416, 7001, 0, 0, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 30, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(461, 2189, 2190, hidden: false, ECharacterFeatureType.Special, 2191, 2192, 2193, new FeatureMedals[3]
		{
			new FeatureMedals(),
			new FeatureMedals("inc"),
			new FeatureMedals()
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: false, canDeleteManually: false, canCrossArchive: false, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 416, 7001, 0, 0, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 30, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(462, 2194, 2195, hidden: false, ECharacterFeatureType.Special, 2196, 2197, 2198, new FeatureMedals[3]
		{
			new FeatureMedals(),
			new FeatureMedals(),
			new FeatureMedals("inc")
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: false, canDeleteManually: false, canCrossArchive: false, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 416, 7001, 0, 0, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 30, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(463, 2199, 2200, hidden: false, ECharacterFeatureType.Special, 2201, 2202, 2203, new FeatureMedals[3]
		{
			new FeatureMedals("pos"),
			new FeatureMedals(),
			new FeatureMedals("pos", "pos")
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: false, canDeleteManually: false, canCrossArchive: false, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 463, 104, 0, 0, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 100, 0, 0, 0, 50, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(464, 2204, 2205, hidden: false, ECharacterFeatureType.Special, 2206, 2207, 2208, new FeatureMedals[3]
		{
			new FeatureMedals("neg", "neg"),
			new FeatureMedals(),
			new FeatureMedals("neg")
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: false, canDeleteManually: false, canCrossArchive: false, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 463, 104, 0, 0, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 100, 0, 50, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(465, 2209, 2210, hidden: false, ECharacterFeatureType.Special, 2211, 2212, 2213, new FeatureMedals[3]
		{
			new FeatureMedals("pos"),
			new FeatureMedals("pos"),
			new FeatureMedals("pos")
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: false, canDeleteManually: false, canCrossArchive: false, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 463, 104, 0, 0, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 100, 0, 0, 0, 0, 50, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(466, 2214, 2215, hidden: false, ECharacterFeatureType.Special, 2216, 2217, 2218, new FeatureMedals[3]
		{
			new FeatureMedals("pos", "pos"),
			new FeatureMedals("pos"),
			new FeatureMedals()
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: false, canDeleteManually: false, canCrossArchive: false, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 463, 104, 0, 0, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 100, 0, 0, 50, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(467, 2219, 2220, hidden: false, ECharacterFeatureType.Special, 2221, 2222, 2223, new FeatureMedals[3]
		{
			new FeatureMedals(),
			new FeatureMedals("pos", "pos"),
			new FeatureMedals("pos")
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: false, canDeleteManually: false, canCrossArchive: false, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 463, 104, 0, 0, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 0, 100, 50, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(468, 2224, 2225, hidden: false, ECharacterFeatureType.Special, 2226, 2227, 2228, new FeatureMedals[3]
		{
			new FeatureMedals("pos"),
			new FeatureMedals(),
			new FeatureMedals("pos", "pos")
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: false, canDeleteManually: false, canCrossArchive: false, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 463, 104, 0, 0, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 35, 0, 0, 0, 25, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(469, 2229, 2230, hidden: false, ECharacterFeatureType.Special, 2231, 2232, 2233, new FeatureMedals[3]
		{
			new FeatureMedals("neg", "neg"),
			new FeatureMedals(),
			new FeatureMedals("neg")
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: false, canDeleteManually: false, canCrossArchive: false, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 463, 104, 0, 0, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 35, 0, 25, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(470, 2234, 2235, hidden: false, ECharacterFeatureType.Special, 2236, 2237, 2238, new FeatureMedals[3]
		{
			new FeatureMedals("pos"),
			new FeatureMedals("pos"),
			new FeatureMedals("pos")
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: false, canDeleteManually: false, canCrossArchive: false, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 463, 104, 0, 0, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 35, 0, 0, 0, 0, 25, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(471, 2239, 2240, hidden: false, ECharacterFeatureType.Special, 2241, 2242, 2243, new FeatureMedals[3]
		{
			new FeatureMedals("pos", "pos"),
			new FeatureMedals("pos"),
			new FeatureMedals()
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: false, canDeleteManually: false, canCrossArchive: false, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 463, 104, 0, 0, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 35, 0, 0, 25, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(472, 2244, 2245, hidden: false, ECharacterFeatureType.Special, 2246, 2247, 2248, new FeatureMedals[3]
		{
			new FeatureMedals(),
			new FeatureMedals("pos", "pos"),
			new FeatureMedals("pos")
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: false, canDeleteManually: false, canCrossArchive: false, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 463, 104, 0, 0, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 0, 35, 25, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(473, 2249, 2250, hidden: false, ECharacterFeatureType.Special, 2251, 2252, 2253, new FeatureMedals[3]
		{
			new FeatureMedals("neg"),
			new FeatureMedals("neg"),
			new FeatureMedals("neg")
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: false, canDeleteManually: false, canCrossArchive: false, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 473, 104, 0, 0, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 0, 0, -50, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(474, 2254, 2255, hidden: false, ECharacterFeatureType.Special, 2256, 2257, 2258, new FeatureMedals[3]
		{
			new FeatureMedals(),
			new FeatureMedals("pos"),
			new FeatureMedals("pos", "pos")
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: false, canDeleteManually: false, canCrossArchive: false, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 474, 104, 0, 0, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 0, 0, 35, 50, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(475, 2259, 2260, hidden: false, ECharacterFeatureType.Special, 2261, 2262, 2263, new FeatureMedals[3]
		{
			new FeatureMedals("neg", "neg", "neg"),
			new FeatureMedals(),
			new FeatureMedals()
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: false, canDeleteManually: false, canCrossArchive: false, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 474, 104, 0, 0, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 50, 0, 35, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(476, 2264, 2265, hidden: false, ECharacterFeatureType.Special, 2266, 2267, 2268, new FeatureMedals[3]
		{
			new FeatureMedals("pos"),
			new FeatureMedals("pos"),
			new FeatureMedals("pos")
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: false, canDeleteManually: false, canCrossArchive: false, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 474, 104, 0, 0, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 50, 0, 0, 35, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(477, 2269, 2270, hidden: false, ECharacterFeatureType.Special, 2271, 2272, 2273, new FeatureMedals[3]
		{
			new FeatureMedals(),
			new FeatureMedals("neg", "neg", "neg"),
			new FeatureMedals()
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: false, canDeleteManually: false, canCrossArchive: false, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 474, 104, 0, 0, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 0, 50, 35, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(478, 2274, 2275, hidden: false, ECharacterFeatureType.Special, 2276, 2277, 2278, new FeatureMedals[3]
		{
			new FeatureMedals("pos", "pos"),
			new FeatureMedals("pos"),
			new FeatureMedals()
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: false, canDeleteManually: false, canCrossArchive: false, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 474, 104, 0, 0, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 50, 0, 0, 0, 0, 35, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(479, 2279, 2280, hidden: false, ECharacterFeatureType.Special, 2281, 2282, 2283, new FeatureMedals[3]
		{
			new FeatureMedals(),
			new FeatureMedals("pos", "pos", "pos"),
			new FeatureMedals()
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: false, canDeleteManually: false, canCrossArchive: false, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 474, 104, 0, 0, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 0, 50, 35, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
	}

	private void CreateItems8()
	{
		_dataArray.Add(new CharacterFeatureItem(480, 2284, 2285, hidden: false, ECharacterFeatureType.Special, 2286, 2287, 2288, new FeatureMedals[3]
		{
			new FeatureMedals("pos", "pos"),
			new FeatureMedals(),
			new FeatureMedals("pos")
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: false, canDeleteManually: false, canCrossArchive: false, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 474, 104, 0, 0, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 50, 0, 35, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(481, 2289, 2290, hidden: false, ECharacterFeatureType.Special, 2291, 2292, 2293, new FeatureMedals[3]
		{
			new FeatureMedals(),
			new FeatureMedals(),
			new FeatureMedals("pos", "pos", "pos")
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: false, canDeleteManually: false, canCrossArchive: false, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 474, 104, 0, 0, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 50, 0, 0, 0, 35, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(482, 2294, 2295, hidden: false, ECharacterFeatureType.Special, 2296, 2297, 2298, new FeatureMedals[3]
		{
			new FeatureMedals(),
			new FeatureMedals(),
			new FeatureMedals("neg", "neg", "neg")
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: false, canDeleteManually: false, canCrossArchive: false, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 474, 104, 0, 0, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 50, 0, 0, 0, 35, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(483, 2299, 2300, hidden: false, ECharacterFeatureType.Special, 2301, 2302, 2303, new FeatureMedals[3]
		{
			new FeatureMedals("neg"),
			new FeatureMedals("neg"),
			new FeatureMedals("neg")
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: false, canDeleteManually: false, canCrossArchive: false, inheritableThroughSamsara: false, inheritableTransferTaiwu: true, ECharacterFeatureDarkAshProtector.None, -1, 483, 4, 0, 0, -1, -1, 0, 0, null, 80, 0, 0, 5, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 0, 0, -20, -20, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(484, 2304, 2305, hidden: false, ECharacterFeatureType.Special, 2306, 2307, 2308, new FeatureMedals[3]
		{
			new FeatureMedals("pos"),
			new FeatureMedals("pos"),
			new FeatureMedals("pos")
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: false, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: true, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 484, 104, 0, 0, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 0, 0, 20, 20, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(485, 2309, 2310, hidden: false, ECharacterFeatureType.Special, 2311, 2312, 2313, new FeatureMedals[3]
		{
			new FeatureMedals("neg"),
			new FeatureMedals("neg"),
			new FeatureMedals("neg")
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: false, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: true, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 484, 104, 0, 0, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 0, 0, -20, -20, 0, 0, 0, 0, 0, 0, 0, -3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(486, 2314, 2315, hidden: false, ECharacterFeatureType.Special, 2316, 2317, 2318, new FeatureMedals[3]
		{
			new FeatureMedals("neg"),
			new FeatureMedals("neg"),
			new FeatureMedals("neg")
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: false, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, 12, 486, 4, 0, 0, -1, -1, 0, 0, null, 0, 0, -20, 0, 0, 0, 0, 200, 100, makeConsummateLevelRelated: false, new short[5] { 40, 40, 40, 40, 40 }, new sbyte[5], new sbyte[5], -50, -50, 50, 50, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 40, 40, 40, 40, 40, 40, 40, 40, 40, 40, 40, 40, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(487, 2319, 2320, hidden: false, ECharacterFeatureType.Special, 2321, 2322, 2323, new FeatureMedals[3]
		{
			new FeatureMedals(),
			new FeatureMedals("inc", "inc", "inc"),
			new FeatureMedals()
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: true, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: false, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 220, 6, 0, 0, -1, -1, 0, 0, null, -80, 160, -40, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 50, -50, -50, 0, 50, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(488, 2324, 2325, hidden: false, ECharacterFeatureType.Special, 2326, 2327, 2328, new FeatureMedals[3]
		{
			new FeatureMedals("inc"),
			new FeatureMedals("inc"),
			new FeatureMedals("inc")
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: false, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 405, 104, 0, 0, -1, -1, 0, 0, null, 0, 0, -20, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5] { 80, 80, 80, 80, 80 }, new sbyte[5], new sbyte[5], 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 40, 40, 40, 40, 40, 40, 40, 40, 40, 40, 40, 40, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(489, 2329, 2330, hidden: false, ECharacterFeatureType.Special, 2331, 2332, 2333, new FeatureMedals[3]
		{
			new FeatureMedals("inc"),
			new FeatureMedals("inc"),
			new FeatureMedals("inc")
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: true, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: false, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 220, 6, 0, 0, -1, -1, 0, 0, null, 0, 0, -40, 100, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, -100, -100, 0, 0, 50, 50, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(490, 2334, 2335, hidden: false, ECharacterFeatureType.Special, 2336, 2337, 2338, new FeatureMedals[3]
		{
			new FeatureMedals("inc"),
			new FeatureMedals("inc"),
			new FeatureMedals("inc")
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: true, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: false, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 220, 6, 0, 0, -1, -1, 0, 0, null, 0, 0, -40, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 100, 0, -100, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(491, 2339, 2340, hidden: false, ECharacterFeatureType.Special, 2341, 2342, 2343, new FeatureMedals[3]
		{
			new FeatureMedals("inc", "inc", "inc"),
			new FeatureMedals(),
			new FeatureMedals()
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: false, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 220, 6, 0, 0, -1, -1, 0, 0, null, 0, 0, -20, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], -50, 0, 0, 50, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(492, 2344, 2345, hidden: false, ECharacterFeatureType.Special, 2346, 2347, 2348, new FeatureMedals[3]
		{
			new FeatureMedals(),
			new FeatureMedals(),
			new FeatureMedals("inc", "inc", "inc")
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: true, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: false, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 220, 6, 0, 0, -1, -1, 0, 0, null, 0, 0, -20, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], -50, 50, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(493, 2349, 2350, hidden: false, ECharacterFeatureType.Special, 2351, 2352, 2353, new FeatureMedals[3]
		{
			new FeatureMedals("inc"),
			new FeatureMedals("inc"),
			new FeatureMedals("inc")
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: true, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: false, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 220, 6, 0, 0, -1, -1, 0, 0, null, 0, 0, -20, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, -50, 50, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(494, 2354, 2355, hidden: false, ECharacterFeatureType.Special, 2356, 2357, 2358, new FeatureMedals[3]
		{
			new FeatureMedals("inc"),
			new FeatureMedals("inc"),
			new FeatureMedals("inc")
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: true, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: false, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 220, 6, 0, 0, -1, -1, 0, 0, null, 0, 0, -20, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 50, -50, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(495, 2359, 2360, hidden: false, ECharacterFeatureType.Special, 2361, 2362, 2363, new FeatureMedals[3]
		{
			new FeatureMedals(),
			new FeatureMedals(),
			new FeatureMedals("inc", "inc", "inc")
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: true, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: false, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 220, 6, 0, 0, -1, -1, 0, 0, null, 0, 0, -20, 10, 0, 0, 20, 20, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], -50, 0, -50, 50, 50, 0, 0, 0, 0, 0, 0, 0, 0, -1000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 150, 150, 150, 150, 150, 150, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(496, 2364, 2365, hidden: false, ECharacterFeatureType.Temporary, 2366, 2367, 2368, new FeatureMedals[3]
		{
			new FeatureMedals(),
			new FeatureMedals(),
			new FeatureMedals("inc")
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: false, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 496, 6, 0, 12, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 110, 90, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(497, 2369, 2370, hidden: false, ECharacterFeatureType.Temporary, 2371, 2372, 2373, new FeatureMedals[3]
		{
			new FeatureMedals(),
			new FeatureMedals("inc"),
			new FeatureMedals()
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: false, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 496, 6, 0, 12, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 105, 95, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 50, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(498, 2374, 2375, hidden: false, ECharacterFeatureType.Temporary, 2376, 2377, 2378, new FeatureMedals[3]
		{
			new FeatureMedals("inc"),
			new FeatureMedals(),
			new FeatureMedals()
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: false, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 496, 6, 0, 12, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 90, 110, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -100, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(499, 2379, 2380, hidden: false, ECharacterFeatureType.Special, 2381, 2382, 2383, new FeatureMedals[3]
		{
			new FeatureMedals(),
			new FeatureMedals("pos"),
			new FeatureMedals()
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: false, canDeleteManually: true, canCrossArchive: true, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 499, 107, 0, 0, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 10, 0, 0, 0, 10, 0, 0, 0, 0, 0, 0, 0, 30, -25, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -6, 12, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9] { 10, 16, 18, 20, 28, 30, 40, 42, 54 }, 20, 10));
		_dataArray.Add(new CharacterFeatureItem(500, 2384, 2385, hidden: false, ECharacterFeatureType.Special, 2386, 2387, 2388, new FeatureMedals[3]
		{
			new FeatureMedals("pos"),
			new FeatureMedals(),
			new FeatureMedals()
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: false, canDeleteManually: true, canCrossArchive: true, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 500, 107, 0, 0, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 10, 0, 10, 0, 0, 0, 0, 0, 30, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 60, -30, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 6, 6, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9] { 8, 14, 16, 18, 26, 28, 38, 40, 52 }, 18, 9));
		_dataArray.Add(new CharacterFeatureItem(501, 2389, 2390, hidden: false, ECharacterFeatureType.Special, 2391, 2392, 2393, new FeatureMedals[3]
		{
			new FeatureMedals(),
			new FeatureMedals(),
			new FeatureMedals("pos")
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: false, canDeleteManually: true, canCrossArchive: true, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 501, 107, 0, 0, -1, -1, 0, 0, null, 0, 0, 0, 0, 30, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 10, 10, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -5, 0, 0, 0, 0, 0, -5, 0, 0, 0, 0, 0, 30, 30, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 12, -6, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9] { 6, 12, 14, 16, 24, 26, 36, 38, 50 }, 16, 8));
		_dataArray.Add(new CharacterFeatureItem(502, 2394, 2395, hidden: false, ECharacterFeatureType.Special, 2396, 2397, 2398, new FeatureMedals[3]
		{
			new FeatureMedals(),
			new FeatureMedals("pos"),
			new FeatureMedals()
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: false, canDeleteManually: true, canCrossArchive: true, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 502, 107, 0, 0, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 10, 0, 10, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -5, -5, -5, 0, 0, 0, 10, 10, 10, 0, 0, 0, 0, 0, 0, 0, 0, 0, -30, -30, 60, 60, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 12, -6, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9] { 8, 14, 16, 18, 26, 28, 38, 40, 52 }, 18, 9));
		_dataArray.Add(new CharacterFeatureItem(503, 2399, 2400, hidden: false, ECharacterFeatureType.Special, 2401, 2402, 2403, new FeatureMedals[3]
		{
			new FeatureMedals("pos"),
			new FeatureMedals(),
			new FeatureMedals()
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: false, canDeleteManually: true, canCrossArchive: true, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 503, 107, 0, 0, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 10, 10, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -5, 0, 0, 0, 0, 0, -5, 0, 10, 10, 0, 0, 0, 0, 0, 0, -30, 60, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9] { 6, 12, 14, 16, 24, 26, 36, 38, 50 }, 16, 8));
		_dataArray.Add(new CharacterFeatureItem(504, 2404, 2405, hidden: false, ECharacterFeatureType.Special, 2406, 2407, 2408, new FeatureMedals[3]
		{
			new FeatureMedals("inc"),
			new FeatureMedals(),
			new FeatureMedals()
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: false, canDeleteManually: true, canCrossArchive: true, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 504, 107, 0, 0, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 10, 10, 0, 0, 30, 0, 0, 0, 0, 0, 0, 0, 0, 0, -5, 0, 0, 10, 10, 0, -5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -6, -6, -6, -6, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(505, 2409, 2410, hidden: false, ECharacterFeatureType.Special, 2411, 2412, 2413, new FeatureMedals[3]
		{
			new FeatureMedals(),
			new FeatureMedals(),
			new FeatureMedals("inc")
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: false, canDeleteManually: true, canCrossArchive: true, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 505, 107, 0, 0, -1, -1, 0, 0, null, 0, 0, -20, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 10, 10, 0, 0, 0, 0, 0, 0, 0, 0, 30, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -150, -150, -150, -150, -150, -150, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(506, 2414, 2415, hidden: false, ECharacterFeatureType.Special, 2416, 2417, 2418, new FeatureMedals[3]
		{
			new FeatureMedals(),
			new FeatureMedals("inc"),
			new FeatureMedals()
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: false, canDeleteManually: true, canCrossArchive: true, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 506, 107, 0, 0, -1, -1, 0, 0, null, 0, 0, 0, 0, -15, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 10, 0, 0, 0, 10, 0, 0, 0, 0, 0, 30, 0, 0, -25, 0, 100, 0, 0, 0, 15, 0, 0, 0, 0, 0, 15, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(507, 2419, 2420, hidden: false, ECharacterFeatureType.Special, 2421, 2422, 2423, new FeatureMedals[3]
		{
			new FeatureMedals("inc"),
			new FeatureMedals(),
			new FeatureMedals()
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: false, canDeleteManually: true, canCrossArchive: true, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 507, 107, 0, 0, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 10, 10, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 12, 12, -6, -6, 12, 12, -6, -6, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(508, 2424, 2425, hidden: false, ECharacterFeatureType.Special, 2426, 2427, 2428, new FeatureMedals[3]
		{
			new FeatureMedals(),
			new FeatureMedals(),
			new FeatureMedals("inc")
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: false, canDeleteManually: true, canCrossArchive: true, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 508, 107, 0, 0, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 10, 10, 0, 0, 0, 0, 0, 0, 30, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -5, -5, 0, 0, 0, 0, 10, 10, -30, -30, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 6, 6, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(509, 2429, 2430, hidden: false, ECharacterFeatureType.Special, 2431, 2432, 2433, new FeatureMedals[3]
		{
			new FeatureMedals(),
			new FeatureMedals("neg"),
			new FeatureMedals()
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: false, canDeleteManually: true, canCrossArchive: true, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 509, 107, 0, 0, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 10, 0, 0, 0, 10, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -5, 0, 0, 0, 0, 0, -5, 0, 0, 60, 60, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9] { -10, -16, -18, -20, -28, -30, -40, -42, -54 }, -20, -10));
		_dataArray.Add(new CharacterFeatureItem(510, 2434, 2435, hidden: false, ECharacterFeatureType.Special, 2436, 2437, 2438, new FeatureMedals[3]
		{
			new FeatureMedals(),
			new FeatureMedals(),
			new FeatureMedals("neg")
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: false, canDeleteManually: true, canCrossArchive: true, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 510, 107, 0, 0, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 10, 10, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -30, -30, 50, 50, 50, 50, 50, 50, 0, 0, 0, 0, 0, 0, 0, 0, -6, 12, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9] { -6, -12, -14, -16, -24, -26, -36, -38, -50 }, -16, -8));
		_dataArray.Add(new CharacterFeatureItem(511, 2439, 2440, hidden: false, ECharacterFeatureType.Special, 2441, 2442, 2443, new FeatureMedals[3]
		{
			new FeatureMedals(),
			new FeatureMedals(),
			new FeatureMedals("neg")
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: false, canDeleteManually: true, canCrossArchive: true, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 511, 107, 0, 0, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 10, 10, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 60, -30, 60, -30, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9] { -6, -12, -14, -16, -24, -26, -36, -38, -50 }, -16, -8));
		_dataArray.Add(new CharacterFeatureItem(512, 2444, 2445, hidden: false, ECharacterFeatureType.Special, 2446, 2447, 2448, new FeatureMedals[3]
		{
			new FeatureMedals("neg"),
			new FeatureMedals(),
			new FeatureMedals()
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: false, canDeleteManually: true, canCrossArchive: true, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 512, 107, 0, 0, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 10, 10, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 10, 10, 10, 0, 0, 0, -5, -5, -5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9] { -8, -14, -16, -18, -26, -28, -38, -40, -52 }, -18, -9));
		_dataArray.Add(new CharacterFeatureItem(513, 2449, 2450, hidden: false, ECharacterFeatureType.Special, 2451, 2452, 2453, new FeatureMedals[3]
		{
			new FeatureMedals(),
			new FeatureMedals("neg"),
			new FeatureMedals()
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: false, canDeleteManually: true, canCrossArchive: true, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 513, 107, 0, 0, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 10, 10, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 10, 10, 0, 0, 0, 0, -5, -5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9] { -8, -14, -16, -18, -26, -28, -38, -40, -52 }, -18, -9));
		_dataArray.Add(new CharacterFeatureItem(514, 2454, 2455, hidden: false, ECharacterFeatureType.Temporary, 2456, 2457, 2458, new FeatureMedals[3]
		{
			new FeatureMedals(),
			new FeatureMedals(),
			new FeatureMedals("inc", "inc", "inc")
		}, 0, ECharacterFeatureInfectedType.LegendaryBook, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: false, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 514, 3, 0, 24, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 40, 220, 100, makeConsummateLevelRelated: false, new short[5] { 20, 20, 20, 20, 20 }, new sbyte[5], new sbyte[5], 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(515, 2459, 2460, hidden: false, ECharacterFeatureType.Special, 2461, 2462, 2463, new FeatureMedals[3]
		{
			new FeatureMedals("inc"),
			new FeatureMedals("inc"),
			new FeatureMedals("inc")
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: false, canDeleteManually: false, canCrossArchive: false, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 515, 6, 0, 0, -1, -1, 0, 0, null, 0, 0, 0, 0, 20, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5] { 10, 10, 10, 10, 10 }, new sbyte[5], 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(516, 2464, 2465, hidden: false, ECharacterFeatureType.Special, 2466, 2467, 2468, new FeatureMedals[3]
		{
			new FeatureMedals("pos"),
			new FeatureMedals(),
			new FeatureMedals()
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: false, canDeleteManually: false, canCrossArchive: false, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 516, 6, 0, 0, -1, -1, 0, 0, null, 0, 0, 0, 0, 15, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5] { 20, -20, 0, 0, 0 }, new sbyte[5], 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(517, 2469, 2470, hidden: false, ECharacterFeatureType.Special, 2471, 2472, 2473, new FeatureMedals[3]
		{
			new FeatureMedals(),
			new FeatureMedals(),
			new FeatureMedals("pos")
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: false, canDeleteManually: false, canCrossArchive: false, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 517, 6, 0, 0, -1, -1, 0, 0, null, 0, 0, 0, 0, 15, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5] { 0, 20, 0, 0, -20 }, new sbyte[5], 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(518, 2474, 2475, hidden: false, ECharacterFeatureType.Special, 2476, 2477, 2478, new FeatureMedals[3]
		{
			new FeatureMedals(),
			new FeatureMedals(),
			new FeatureMedals("pos")
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: false, canDeleteManually: false, canCrossArchive: false, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 518, 6, 0, 0, -1, -1, 0, 0, null, 0, 0, 0, 0, 15, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5] { 0, 0, 20, -20, 0 }, new sbyte[5], 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(519, 2479, 2480, hidden: false, ECharacterFeatureType.Special, 2481, 2482, 2483, new FeatureMedals[3]
		{
			new FeatureMedals("pos"),
			new FeatureMedals(),
			new FeatureMedals()
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: false, canDeleteManually: false, canCrossArchive: false, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 519, 6, 0, 0, -1, -1, 0, 0, null, 0, 0, 0, 0, 15, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5] { -20, 0, 0, 20, 0 }, new sbyte[5], 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(520, 2484, 2485, hidden: false, ECharacterFeatureType.Special, 2486, 2487, 2488, new FeatureMedals[3]
		{
			new FeatureMedals(),
			new FeatureMedals("pos"),
			new FeatureMedals()
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: false, canDeleteManually: false, canCrossArchive: false, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 520, 6, 0, 0, -1, -1, 0, 0, null, 0, 0, 0, 0, 15, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5] { 0, 0, -20, 0, 20 }, new sbyte[5], 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(521, 2489, 2490, hidden: false, ECharacterFeatureType.Special, 2491, 2492, 2493, new FeatureMedals[3]
		{
			new FeatureMedals("dec"),
			new FeatureMedals(),
			new FeatureMedals()
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: false, canDeleteManually: false, canCrossArchive: false, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 521, 6, 0, 0, -1, -1, 0, 0, null, 0, 0, 0, 0, -15, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5] { -20, 0, 0, 0, 0 }, new sbyte[5], 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(522, 2494, 2495, hidden: false, ECharacterFeatureType.Special, 2496, 2497, 2498, new FeatureMedals[3]
		{
			new FeatureMedals(),
			new FeatureMedals(),
			new FeatureMedals("dec")
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: false, canDeleteManually: false, canCrossArchive: false, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 522, 6, 0, 0, -1, -1, 0, 0, null, 0, 0, 0, 0, -15, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5] { 0, -20, 0, 0, 0 }, new sbyte[5], 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(523, 2499, 2500, hidden: false, ECharacterFeatureType.Special, 2501, 2502, 2503, new FeatureMedals[3]
		{
			new FeatureMedals(),
			new FeatureMedals(),
			new FeatureMedals("dec")
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: false, canDeleteManually: false, canCrossArchive: false, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 523, 6, 0, 0, -1, -1, 0, 0, null, 0, 0, 0, 0, -15, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5] { 0, 0, -20, 0, 0 }, new sbyte[5], 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(524, 2504, 2505, hidden: false, ECharacterFeatureType.Special, 2506, 2507, 2508, new FeatureMedals[3]
		{
			new FeatureMedals("dec"),
			new FeatureMedals(),
			new FeatureMedals()
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: false, canDeleteManually: false, canCrossArchive: false, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 524, 6, 0, 0, -1, -1, 0, 0, null, 0, 0, 0, 0, -15, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5] { 0, 0, 0, -20, 0 }, new sbyte[5], 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(525, 2509, 2510, hidden: false, ECharacterFeatureType.Special, 2511, 2512, 2513, new FeatureMedals[3]
		{
			new FeatureMedals(),
			new FeatureMedals("dec"),
			new FeatureMedals()
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: false, canDeleteManually: false, canCrossArchive: false, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 525, 6, 0, 0, -1, -1, 0, 0, null, 0, 0, 0, 0, -15, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5] { 0, 0, 0, 0, -20 }, new sbyte[5], 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(526, 2514, 2515, hidden: false, ECharacterFeatureType.Special, 2516, 2517, 2518, new FeatureMedals[3]
		{
			new FeatureMedals("inc"),
			new FeatureMedals(),
			new FeatureMedals()
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: false, canDeleteManually: false, canCrossArchive: false, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 526, 6, 0, 0, -1, -1, 0, 0, null, 0, 0, 0, 0, 10, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5] { 15, -15, 0, 0, 0 }, new sbyte[5], 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(527, 2519, 2520, hidden: false, ECharacterFeatureType.Special, 2521, 2522, 2523, new FeatureMedals[3]
		{
			new FeatureMedals(),
			new FeatureMedals(),
			new FeatureMedals("inc")
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: false, canDeleteManually: false, canCrossArchive: false, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 527, 6, 0, 0, -1, -1, 0, 0, null, 0, 0, 0, 0, 10, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5] { 0, 15, 0, 0, -15 }, new sbyte[5], 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(528, 2524, 2525, hidden: false, ECharacterFeatureType.Special, 2526, 2527, 2528, new FeatureMedals[3]
		{
			new FeatureMedals(),
			new FeatureMedals(),
			new FeatureMedals("inc")
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: false, canDeleteManually: false, canCrossArchive: false, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 528, 6, 0, 0, -1, -1, 0, 0, null, 0, 0, 0, 0, 10, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5] { 0, 0, 15, -15, 0 }, new sbyte[5], 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(529, 2529, 2530, hidden: false, ECharacterFeatureType.Special, 2531, 2532, 2533, new FeatureMedals[3]
		{
			new FeatureMedals("inc"),
			new FeatureMedals(),
			new FeatureMedals()
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: false, canDeleteManually: false, canCrossArchive: false, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 529, 6, 0, 0, -1, -1, 0, 0, null, 0, 0, 0, 0, 10, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5] { -15, 0, 0, 15, 0 }, new sbyte[5], 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(530, 2534, 2535, hidden: false, ECharacterFeatureType.Special, 2536, 2537, 2538, new FeatureMedals[3]
		{
			new FeatureMedals(),
			new FeatureMedals("inc"),
			new FeatureMedals()
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: false, canDeleteManually: false, canCrossArchive: false, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 530, 6, 0, 0, -1, -1, 0, 0, null, 0, 0, 0, 0, 10, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5] { 0, 0, -15, 0, 15 }, new sbyte[5], 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(531, 2539, 2540, hidden: false, ECharacterFeatureType.Special, 2541, 2542, 2543, new FeatureMedals[3]
		{
			new FeatureMedals("neg"),
			new FeatureMedals(),
			new FeatureMedals()
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: false, canDeleteManually: false, canCrossArchive: false, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 531, 6, 0, 0, -1, -1, 0, 0, null, 0, 0, 0, 0, -10, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5] { -15, 0, 0, 0, 0 }, new sbyte[5], 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(532, 2544, 2545, hidden: false, ECharacterFeatureType.Special, 2546, 2547, 2548, new FeatureMedals[3]
		{
			new FeatureMedals(),
			new FeatureMedals(),
			new FeatureMedals("neg")
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: false, canDeleteManually: false, canCrossArchive: false, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 532, 6, 0, 0, -1, -1, 0, 0, null, 0, 0, 0, 0, -10, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5] { 0, -15, 0, 0, 0 }, new sbyte[5], 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(533, 2549, 2550, hidden: false, ECharacterFeatureType.Special, 2551, 2552, 2553, new FeatureMedals[3]
		{
			new FeatureMedals(),
			new FeatureMedals(),
			new FeatureMedals("neg")
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: false, canDeleteManually: false, canCrossArchive: false, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 533, 6, 0, 0, -1, -1, 0, 0, null, 0, 0, 0, 0, -10, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5] { 0, 0, -15, 0, 0 }, new sbyte[5], 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(534, 2554, 2555, hidden: false, ECharacterFeatureType.Special, 2556, 2557, 2558, new FeatureMedals[3]
		{
			new FeatureMedals("neg"),
			new FeatureMedals(),
			new FeatureMedals()
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: false, canDeleteManually: false, canCrossArchive: false, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 534, 6, 0, 0, -1, -1, 0, 0, null, 0, 0, 0, 0, -10, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5] { 0, 0, 0, -15, 0 }, new sbyte[5], 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(535, 2559, 2560, hidden: false, ECharacterFeatureType.Special, 2561, 2562, 2563, new FeatureMedals[3]
		{
			new FeatureMedals(),
			new FeatureMedals("neg"),
			new FeatureMedals()
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: false, canDeleteManually: false, canCrossArchive: false, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 535, 6, 0, 0, -1, -1, 0, 0, null, 0, 0, 0, 0, -10, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5] { 0, 0, 0, 0, -15 }, new sbyte[5], 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(536, 2564, 2565, hidden: false, ECharacterFeatureType.Special, 2566, 2567, 2568, new FeatureMedals[3]
		{
			new FeatureMedals(),
			new FeatureMedals(),
			new FeatureMedals("inc", "inc", "inc")
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: true, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: false, canDeleteManually: false, canCrossArchive: false, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 536, 4, 0, 0, -1, -1, 0, 0, null, -20, 20, -20, 0, 0, 0, 80, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(537, 2569, 2570, hidden: false, ECharacterFeatureType.Special, 2566, 2571, 2568, new FeatureMedals[3]
		{
			new FeatureMedals(),
			new FeatureMedals(),
			new FeatureMedals("inc", "inc", "inc")
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: true, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: false, canDeleteManually: false, canCrossArchive: false, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 536, 4, 0, 0, -1, -1, 0, 0, null, -40, 40, -40, 0, 0, 0, 60, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(538, 2572, 2573, hidden: false, ECharacterFeatureType.Special, 2566, 2574, 2568, new FeatureMedals[3]
		{
			new FeatureMedals(),
			new FeatureMedals(),
			new FeatureMedals("inc", "inc", "inc")
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: true, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: false, canDeleteManually: false, canCrossArchive: false, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 536, 4, 0, 0, -1, -1, 0, 0, null, -60, 60, -60, 0, 0, 0, 40, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(539, 2575, 2576, hidden: false, ECharacterFeatureType.Special, 2566, 2577, 2568, new FeatureMedals[3]
		{
			new FeatureMedals(),
			new FeatureMedals(),
			new FeatureMedals("inc", "inc", "inc")
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: true, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: false, canDeleteManually: false, canCrossArchive: false, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 536, 4, 0, 0, -1, -1, 0, 0, null, -80, 80, -80, 0, 0, 0, 20, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
	}

	private void CreateItems9()
	{
		_dataArray.Add(new CharacterFeatureItem(540, 2578, 2579, hidden: false, ECharacterFeatureType.Special, 2580, 2581, 2582, new FeatureMedals[3]
		{
			new FeatureMedals("dec"),
			new FeatureMedals("dec"),
			new FeatureMedals("dec")
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: false, canDeleteManually: false, canCrossArchive: false, inheritableThroughSamsara: false, inheritableTransferTaiwu: true, ECharacterFeatureDarkAshProtector.None, -1, 540, 6, 0, 0, -1, -1, 0, 0, null, 20, -20, 20, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(541, 2583, 2584, hidden: false, ECharacterFeatureType.Special, 2585, 2586, 2587, new FeatureMedals[3]
		{
			new FeatureMedals("neg"),
			new FeatureMedals("neg"),
			new FeatureMedals("neg")
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: false, canDeleteManually: false, canCrossArchive: false, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 541, 6, 0, 6, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 40, 220, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], -30, -30, 0, 30, 30, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(542, 2588, 2589, hidden: false, ECharacterFeatureType.Special, 2590, 2591, 2592, new FeatureMedals[3]
		{
			new FeatureMedals("neg"),
			new FeatureMedals("neg"),
			new FeatureMedals("neg")
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: false, canDeleteManually: false, canCrossArchive: false, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 541, 6, 0, 12, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 20, 260, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], -50, -50, 0, 50, 50, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(543, 2593, 2594, hidden: false, ECharacterFeatureType.Special, 2595, 2596, 2597, new FeatureMedals[3]
		{
			new FeatureMedals("inc"),
			new FeatureMedals("inc"),
			new FeatureMedals("inc")
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: true, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: false, canDeleteManually: false, canCrossArchive: false, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 220, 6, 0, 0, -1, -1, 0, 0, null, -60, 60, -20, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 0, 0, 50, 50, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(544, 2598, 2599, hidden: false, ECharacterFeatureType.Special, 2600, 2601, 2602, new FeatureMedals[3]
		{
			new FeatureMedals("dec"),
			new FeatureMedals("dec"),
			new FeatureMedals("dec")
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: false, canDeleteManually: false, canCrossArchive: false, inheritableThroughSamsara: false, inheritableTransferTaiwu: true, ECharacterFeatureDarkAshProtector.None, -1, 540, 6, 0, 0, -1, -1, 0, 0, null, 20, -20, 20, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(545, 2603, 2604, hidden: false, ECharacterFeatureType.Special, 2605, 2606, 2607, new FeatureMedals[3]
		{
			new FeatureMedals("dec"),
			new FeatureMedals("dec"),
			new FeatureMedals("dec")
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: false, canDeleteManually: false, canCrossArchive: false, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 220, 6, 0, 0, -1, -1, 0, 0, null, 80, 80, -40, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 0, 0, -100, -100, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(546, 2608, 2609, hidden: false, ECharacterFeatureType.Special, 2610, 2611, 2612, new FeatureMedals[3]
		{
			new FeatureMedals(),
			new FeatureMedals("inc"),
			new FeatureMedals()
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: true, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: false, canDeleteManually: false, canCrossArchive: false, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 220, 6, 0, 0, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 20, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], -50, 0, 0, 50, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(547, 2613, 2614, hidden: false, ECharacterFeatureType.Special, 2615, 2616, 2617, new FeatureMedals[3]
		{
			new FeatureMedals("inc"),
			new FeatureMedals(),
			new FeatureMedals()
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: true, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: false, canDeleteManually: false, canCrossArchive: false, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 220, 6, 0, 0, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 20, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 50, -50, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(548, 2618, 2619, hidden: false, ECharacterFeatureType.Special, 2620, 2621, 2622, new FeatureMedals[3]
		{
			new FeatureMedals(),
			new FeatureMedals(),
			new FeatureMedals("inc")
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: true, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: false, canDeleteManually: false, canCrossArchive: false, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 220, 6, 0, 0, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 20, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 50, 0, -50, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(549, 2623, 2624, hidden: false, ECharacterFeatureType.Special, 2625, 2626, 2627, new FeatureMedals[3]
		{
			new FeatureMedals(),
			new FeatureMedals(),
			new FeatureMedals("inc", "inc", "inc")
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: true, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: false, canDeleteManually: false, canCrossArchive: false, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 220, 6, 0, 0, -1, -1, 0, 0, null, -80, 80, -20, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 50, 50, -100, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(550, 2628, 2629, hidden: false, ECharacterFeatureType.Special, 2630, 2631, 2632, new FeatureMedals[3]
		{
			new FeatureMedals(),
			new FeatureMedals(),
			new FeatureMedals("inc", "inc", "inc")
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: true, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: false, canDeleteManually: false, canCrossArchive: false, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 220, 6, 0, 0, -1, -1, 0, 0, null, -80, 80, -40, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 50, 50, -100, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(551, 2633, 2634, hidden: false, ECharacterFeatureType.Special, 2635, 2636, 2637, new FeatureMedals[3]
		{
			new FeatureMedals("inc"),
			new FeatureMedals(),
			new FeatureMedals("inc", "inc")
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: true, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: false, canDeleteManually: false, canCrossArchive: false, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 220, 6, 0, 0, -1, -1, 0, 0, null, -60, 60, -20, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 50, 50, 0, 50, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(552, 2638, 2639, hidden: false, ECharacterFeatureType.Special, 2640, 2641, 2642, new FeatureMedals[3]
		{
			new FeatureMedals(),
			new FeatureMedals("inc"),
			new FeatureMedals("inc", "inc")
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: true, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: false, canDeleteManually: false, canCrossArchive: false, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 220, 6, 0, 0, -1, -1, 0, 0, null, -60, 60, -20, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 50, 0, 0, 0, 50, 50, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(553, 2643, 2644, hidden: true, ECharacterFeatureType.Temporary, 2645, 2646, 2647, new FeatureMedals[3]
		{
			new FeatureMedals(),
			new FeatureMedals(),
			new FeatureMedals("inc", "inc", "inc")
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: false, canDeleteManually: false, canCrossArchive: false, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 553, 0, 0, 24, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(554, 2648, 2649, hidden: true, ECharacterFeatureType.Special, 2650, 2651, 2652, new FeatureMedals[3]
		{
			new FeatureMedals(),
			new FeatureMedals(),
			new FeatureMedals()
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: false, canDeleteManually: false, canCrossArchive: false, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 554, 0, 0, 0, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(555, 2653, 2654, hidden: false, ECharacterFeatureType.Special, 2655, 2656, 2657, new FeatureMedals[3]
		{
			new FeatureMedals("dec"),
			new FeatureMedals("dec"),
			new FeatureMedals("dec")
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: true, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: false, canDeleteManually: false, canCrossArchive: false, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 555, 4, 0, 0, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 0, 0, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(556, 2658, 2659, hidden: false, ECharacterFeatureType.Special, 2660, 2661, 2662, new FeatureMedals[3]
		{
			new FeatureMedals("dec"),
			new FeatureMedals("dec"),
			new FeatureMedals("dec")
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: true, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: false, canDeleteManually: false, canCrossArchive: false, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 220, 6, 0, 0, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], -20, -20, -20, -20, -20, -20, -20, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(557, 2663, 2664, hidden: false, ECharacterFeatureType.Special, 2665, 2666, 2667, new FeatureMedals[3]
		{
			new FeatureMedals("inc"),
			new FeatureMedals("inc"),
			new FeatureMedals("inc")
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: true, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: false, canDeleteManually: false, canCrossArchive: false, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 220, 6, 0, 0, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 20, 20, 20, 20, 20, 20, 20, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(558, 2668, 2669, hidden: false, ECharacterFeatureType.Special, 2670, 2671, 2672, new FeatureMedals[3]
		{
			new FeatureMedals("inc"),
			new FeatureMedals("inc"),
			new FeatureMedals("inc")
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: true, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: false, canDeleteManually: false, canCrossArchive: false, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 220, 6, 0, 0, -1, -1, 0, 0, null, 0, 0, -40, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 50, 50, 50, 50, 50, 50, 50, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(559, 2673, 2674, hidden: false, ECharacterFeatureType.Special, 2675, 2676, 2677, new FeatureMedals[3]
		{
			new FeatureMedals("inc"),
			new FeatureMedals("inc"),
			new FeatureMedals("inc")
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: true, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: false, canDeleteManually: false, canCrossArchive: false, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 220, 6, 0, 0, -1, -1, 0, 0, null, 0, 0, -20, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 0, 0, -50, -50, 90, 90, 90, 90, 90, 90, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(560, 2678, 2679, hidden: false, ECharacterFeatureType.Temporary, 2680, 2681, 2682, new FeatureMedals[3]
		{
			new FeatureMedals(),
			new FeatureMedals(),
			new FeatureMedals("dec")
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: true, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 560, 0, 0, 4, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(561, 2678, 2683, hidden: false, ECharacterFeatureType.Temporary, 2680, 2684, 2682, new FeatureMedals[3]
		{
			new FeatureMedals(),
			new FeatureMedals(),
			new FeatureMedals("dec")
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: true, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 561, 0, 0, 8, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(562, 2678, 2685, hidden: false, ECharacterFeatureType.Temporary, 2680, 2686, 2682, new FeatureMedals[3]
		{
			new FeatureMedals(),
			new FeatureMedals(),
			new FeatureMedals("dec")
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: true, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 562, 0, 0, 16, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(563, 2678, 2687, hidden: false, ECharacterFeatureType.Temporary, 2680, 2688, 2682, new FeatureMedals[3]
		{
			new FeatureMedals(),
			new FeatureMedals(),
			new FeatureMedals("dec")
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: true, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 563, 0, 0, 32, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(564, 2678, 2689, hidden: false, ECharacterFeatureType.Temporary, 2680, 2690, 2682, new FeatureMedals[3]
		{
			new FeatureMedals(),
			new FeatureMedals(),
			new FeatureMedals("dec")
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: true, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 564, 0, 0, 64, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(565, 2691, 2692, hidden: false, ECharacterFeatureType.Temporary, 2693, 2694, 2695, new FeatureMedals[3]
		{
			new FeatureMedals("dec"),
			new FeatureMedals(),
			new FeatureMedals()
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: true, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 565, 0, 0, 3, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 10, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(566, 2696, 2697, hidden: false, ECharacterFeatureType.Temporary, 2693, 2698, 2699, new FeatureMedals[3]
		{
			new FeatureMedals("dec"),
			new FeatureMedals(),
			new FeatureMedals()
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: true, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 566, 0, 0, 6, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 20, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(567, 2700, 2701, hidden: false, ECharacterFeatureType.Temporary, 2693, 2702, 2703, new FeatureMedals[3]
		{
			new FeatureMedals("dec"),
			new FeatureMedals(),
			new FeatureMedals()
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: true, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 567, 0, 0, 12, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 40, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(568, 2704, 2705, hidden: false, ECharacterFeatureType.Temporary, 2693, 2706, 2707, new FeatureMedals[3]
		{
			new FeatureMedals("dec"),
			new FeatureMedals(),
			new FeatureMedals()
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: true, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 568, 0, 0, 24, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 70, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(569, 2708, 2709, hidden: false, ECharacterFeatureType.Temporary, 2693, 2710, 2711, new FeatureMedals[3]
		{
			new FeatureMedals("dec"),
			new FeatureMedals(),
			new FeatureMedals()
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: true, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 569, 0, 0, 48, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 110, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(570, 2712, 2713, hidden: false, ECharacterFeatureType.Temporary, 2714, 2715, 2716, new FeatureMedals[3]
		{
			new FeatureMedals(),
			new FeatureMedals("dec"),
			new FeatureMedals()
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: true, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 570, 0, 0, 3, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 100, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(571, 2717, 2718, hidden: false, ECharacterFeatureType.Temporary, 2714, 2719, 2720, new FeatureMedals[3]
		{
			new FeatureMedals(),
			new FeatureMedals("dec"),
			new FeatureMedals()
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: true, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 571, 0, 0, 6, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 200, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(572, 2721, 2722, hidden: false, ECharacterFeatureType.Temporary, 2714, 2723, 2724, new FeatureMedals[3]
		{
			new FeatureMedals(),
			new FeatureMedals("dec"),
			new FeatureMedals()
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: true, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 572, 0, 0, 12, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 400, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(573, 2725, 2726, hidden: false, ECharacterFeatureType.Temporary, 2714, 2727, 2728, new FeatureMedals[3]
		{
			new FeatureMedals(),
			new FeatureMedals("dec"),
			new FeatureMedals()
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: true, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 573, 0, 0, 24, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 700, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(574, 2729, 2730, hidden: false, ECharacterFeatureType.Temporary, 2714, 2731, 2732, new FeatureMedals[3]
		{
			new FeatureMedals(),
			new FeatureMedals("dec"),
			new FeatureMedals()
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: true, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 574, 0, 0, 48, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 1100, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(575, 2733, 2734, hidden: false, ECharacterFeatureType.Temporary, 2735, 2736, 2737, new FeatureMedals[3]
		{
			new FeatureMedals(),
			new FeatureMedals(),
			new FeatureMedals("dec")
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: true, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 575, 0, 0, 3, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -5, -5, -5, -5, -5, -5, -5, -5, -5, -5, -5, -5, -5, -5, -5, -5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(576, 2738, 2739, hidden: false, ECharacterFeatureType.Temporary, 2735, 2740, 2741, new FeatureMedals[3]
		{
			new FeatureMedals(),
			new FeatureMedals(),
			new FeatureMedals("dec")
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: true, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 576, 0, 0, 6, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -15, -15, -15, -15, -15, -15, -15, -15, -15, -15, -15, -15, -15, -15, -15, -15, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(577, 2742, 2743, hidden: false, ECharacterFeatureType.Temporary, 2735, 2744, 2745, new FeatureMedals[3]
		{
			new FeatureMedals(),
			new FeatureMedals(),
			new FeatureMedals("dec")
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: true, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 577, 0, 0, 12, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -30, -30, -30, -30, -30, -30, -30, -30, -30, -30, -30, -30, -30, -30, -30, -30, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(578, 2746, 2747, hidden: false, ECharacterFeatureType.Temporary, 2735, 2748, 2749, new FeatureMedals[3]
		{
			new FeatureMedals(),
			new FeatureMedals(),
			new FeatureMedals("dec")
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: true, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 578, 0, 0, 24, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -50, -50, -50, -50, -50, -50, -50, -50, -50, -50, -50, -50, -50, -50, -50, -50, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(579, 2750, 2751, hidden: false, ECharacterFeatureType.Temporary, 2735, 2752, 2753, new FeatureMedals[3]
		{
			new FeatureMedals(),
			new FeatureMedals(),
			new FeatureMedals("dec")
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: true, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 579, 0, 0, 48, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -75, -75, -75, -75, -75, -75, -75, -75, -75, -75, -75, -75, -75, -75, -75, -75, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(580, 2754, 2755, hidden: false, ECharacterFeatureType.Temporary, 2756, 2757, 2758, new FeatureMedals[3]
		{
			new FeatureMedals("dec"),
			new FeatureMedals(),
			new FeatureMedals()
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: true, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 580, 0, 0, 3, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: true, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(581, 2754, 2759, hidden: false, ECharacterFeatureType.Temporary, 2756, 2760, 2758, new FeatureMedals[3]
		{
			new FeatureMedals("dec"),
			new FeatureMedals(),
			new FeatureMedals()
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: true, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 581, 0, 0, 6, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: true, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(582, 2754, 2761, hidden: false, ECharacterFeatureType.Temporary, 2756, 2762, 2758, new FeatureMedals[3]
		{
			new FeatureMedals("dec"),
			new FeatureMedals(),
			new FeatureMedals()
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: true, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 582, 0, 0, 12, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: true, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(583, 2754, 2763, hidden: false, ECharacterFeatureType.Temporary, 2756, 2764, 2758, new FeatureMedals[3]
		{
			new FeatureMedals("dec"),
			new FeatureMedals(),
			new FeatureMedals()
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: true, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 583, 0, 0, 24, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: true, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(584, 2754, 2765, hidden: false, ECharacterFeatureType.Temporary, 2756, 2766, 2758, new FeatureMedals[3]
		{
			new FeatureMedals("dec"),
			new FeatureMedals(),
			new FeatureMedals()
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: true, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 584, 0, 0, 48, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: true, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(585, 2767, 2768, hidden: false, ECharacterFeatureType.Temporary, 2769, 2770, 2771, new FeatureMedals[3]
		{
			new FeatureMedals(),
			new FeatureMedals(),
			new FeatureMedals("dec")
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: true, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 585, 0, 0, 3, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], -5, -5, -5, -5, -5, -5, -5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(586, 2772, 2773, hidden: false, ECharacterFeatureType.Temporary, 2769, 2774, 2775, new FeatureMedals[3]
		{
			new FeatureMedals(),
			new FeatureMedals(),
			new FeatureMedals("dec")
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: true, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 586, 0, 0, 6, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], -15, -15, -15, -15, -15, -15, -15, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(587, 2776, 2777, hidden: false, ECharacterFeatureType.Temporary, 2769, 2778, 2779, new FeatureMedals[3]
		{
			new FeatureMedals(),
			new FeatureMedals(),
			new FeatureMedals("dec")
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: true, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 587, 0, 0, 12, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], -30, -30, -30, -30, -30, -30, -30, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(588, 2780, 2781, hidden: false, ECharacterFeatureType.Temporary, 2769, 2782, 2783, new FeatureMedals[3]
		{
			new FeatureMedals(),
			new FeatureMedals(),
			new FeatureMedals("dec")
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: true, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 588, 0, 0, 24, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], -50, -50, -50, -50, -50, -50, -50, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(589, 2784, 2785, hidden: false, ECharacterFeatureType.Temporary, 2769, 2786, 2787, new FeatureMedals[3]
		{
			new FeatureMedals(),
			new FeatureMedals(),
			new FeatureMedals("dec")
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: true, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 589, 0, 0, 48, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], -75, -75, -75, -75, -75, -75, -75, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(590, 2788, 2789, hidden: false, ECharacterFeatureType.Temporary, 2790, 2791, 2792, new FeatureMedals[3]
		{
			new FeatureMedals(),
			new FeatureMedals("dec"),
			new FeatureMedals()
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: true, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 590, 0, 0, 3, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 0, 0, 0, 0, -5, -5, -5, -5, -5, -5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(591, 2793, 2794, hidden: false, ECharacterFeatureType.Temporary, 2790, 2795, 2796, new FeatureMedals[3]
		{
			new FeatureMedals(),
			new FeatureMedals("dec"),
			new FeatureMedals()
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: true, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 591, 0, 0, 6, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 0, 0, 0, 0, -15, -15, -15, -15, -15, -15, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(592, 2797, 2798, hidden: false, ECharacterFeatureType.Temporary, 2790, 2799, 2800, new FeatureMedals[3]
		{
			new FeatureMedals(),
			new FeatureMedals("dec"),
			new FeatureMedals()
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: true, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 592, 0, 0, 12, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 0, 0, 0, 0, -30, -30, -30, -30, -30, -30, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(593, 2801, 2802, hidden: false, ECharacterFeatureType.Temporary, 2790, 2803, 2804, new FeatureMedals[3]
		{
			new FeatureMedals(),
			new FeatureMedals("dec"),
			new FeatureMedals()
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: true, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 593, 0, 0, 24, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 0, 0, 0, 0, -50, -50, -50, -50, -50, -50, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(594, 2805, 2806, hidden: false, ECharacterFeatureType.Temporary, 2790, 2807, 2808, new FeatureMedals[3]
		{
			new FeatureMedals(),
			new FeatureMedals("dec"),
			new FeatureMedals()
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: true, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 594, 0, 0, 48, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 0, 0, 0, 0, -75, -75, -75, -75, -75, -75, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(595, 2809, 2810, hidden: false, ECharacterFeatureType.Temporary, 2811, 2812, 2813, new FeatureMedals[3]
		{
			new FeatureMedals(),
			new FeatureMedals("dec"),
			new FeatureMedals()
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: true, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 595, 0, 0, 2, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(596, 2814, 2815, hidden: false, ECharacterFeatureType.Temporary, 2811, 2816, 2813, new FeatureMedals[3]
		{
			new FeatureMedals(),
			new FeatureMedals("dec"),
			new FeatureMedals()
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: true, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 596, 0, 0, 3, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(597, 2817, 2818, hidden: false, ECharacterFeatureType.Temporary, 2811, 2819, 2813, new FeatureMedals[3]
		{
			new FeatureMedals(),
			new FeatureMedals("dec"),
			new FeatureMedals()
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: true, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 597, 0, 0, 6, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(598, 2820, 2821, hidden: false, ECharacterFeatureType.Temporary, 2811, 2822, 2813, new FeatureMedals[3]
		{
			new FeatureMedals(),
			new FeatureMedals("dec"),
			new FeatureMedals()
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: true, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 598, 0, 0, 9, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(599, 2823, 2824, hidden: false, ECharacterFeatureType.Temporary, 2811, 2825, 2813, new FeatureMedals[3]
		{
			new FeatureMedals(),
			new FeatureMedals("dec"),
			new FeatureMedals()
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: true, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 599, 0, 0, 16, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
	}

	private void CreateItems10()
	{
		_dataArray.Add(new CharacterFeatureItem(600, 2826, 2827, hidden: false, ECharacterFeatureType.Temporary, 2828, 2829, 2830, new FeatureMedals[3]
		{
			new FeatureMedals("dec"),
			new FeatureMedals(),
			new FeatureMedals()
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: true, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 600, 0, 0, 3, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 25, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(601, 2831, 2832, hidden: false, ECharacterFeatureType.Temporary, 2828, 2833, 2830, new FeatureMedals[3]
		{
			new FeatureMedals("dec"),
			new FeatureMedals(),
			new FeatureMedals()
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: true, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 601, 0, 0, 6, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 25, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(602, 2834, 2835, hidden: false, ECharacterFeatureType.Temporary, 2828, 2836, 2830, new FeatureMedals[3]
		{
			new FeatureMedals("dec"),
			new FeatureMedals(),
			new FeatureMedals()
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: true, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 602, 0, 0, 12, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 25, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(603, 2837, 2838, hidden: false, ECharacterFeatureType.Temporary, 2828, 2839, 2830, new FeatureMedals[3]
		{
			new FeatureMedals("dec"),
			new FeatureMedals(),
			new FeatureMedals()
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: true, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 603, 0, 0, 24, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 25, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(604, 2840, 2841, hidden: false, ECharacterFeatureType.Temporary, 2828, 2842, 2830, new FeatureMedals[3]
		{
			new FeatureMedals("dec"),
			new FeatureMedals(),
			new FeatureMedals()
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: true, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 604, 0, 0, 48, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 25, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(605, 2843, 2844, hidden: false, ECharacterFeatureType.Special, 2845, 2846, 2847, new FeatureMedals[3]
		{
			new FeatureMedals("pos"),
			new FeatureMedals("pos"),
			new FeatureMedals("pos")
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: true, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 605, 103, 0, 0, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 100, 75, 10, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5] { 1, 0, 0, 0, 0 }, 0, 0, 0, 0, 0, 0, 50, 0, 0, 0, 0, 0, 0, 50, 0, 150, 0, 0, 0, 0, 0, 0, 0, 0, 0, 10, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(606, 2848, 2849, hidden: false, ECharacterFeatureType.Special, 2850, 2851, 2847, new FeatureMedals[3]
		{
			new FeatureMedals("pos"),
			new FeatureMedals("pos"),
			new FeatureMedals("pos")
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: true, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 605, 103, 0, 0, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 100, 75, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5] { 0, 1, 0, 0, 0 }, 0, 0, 0, 0, 0, 50, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 10, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(607, 2852, 2853, hidden: false, ECharacterFeatureType.Special, 2854, 2855, 2847, new FeatureMedals[3]
		{
			new FeatureMedals("pos"),
			new FeatureMedals("pos"),
			new FeatureMedals("pos")
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: true, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 605, 103, 0, 0, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 100, 75, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5] { 0, 0, 1, 0, 0 }, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 10, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(608, 2856, 2857, hidden: false, ECharacterFeatureType.Special, 2858, 2859, 2847, new FeatureMedals[3]
		{
			new FeatureMedals("pos"),
			new FeatureMedals("pos"),
			new FeatureMedals("pos")
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: true, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 605, 103, 0, 0, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 100, 75, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5] { 0, 0, 0, 1, 0 }, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 10, 10, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 50, 50, 50, 50, 50, 50, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(609, 2860, 2861, hidden: false, ECharacterFeatureType.Special, 2862, 2863, 2847, new FeatureMedals[3]
		{
			new FeatureMedals("pos"),
			new FeatureMedals("pos"),
			new FeatureMedals("pos")
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: true, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 605, 103, 0, 0, -1, -1, 0, 0, null, 0, 0, 0, 0, 50, 0, 100, 75, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5] { 0, 0, 0, 0, 1 }, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 10, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(610, 2864, 2865, hidden: false, ECharacterFeatureType.Special, 2866, 2867, 2847, new FeatureMedals[3]
		{
			new FeatureMedals("dec"),
			new FeatureMedals("dec"),
			new FeatureMedals("dec")
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: true, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 605, 103, 0, 0, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 100, 150, 1000, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5] { 1, 0, 0, 0, 0 }, 0, 0, 0, 0, 0, 0, -50, 0, 0, 0, 0, 0, 0, 50, 0, 150, 0, 0, 0, 10, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(611, 2868, 2869, hidden: false, ECharacterFeatureType.Special, 2870, 2871, 2847, new FeatureMedals[3]
		{
			new FeatureMedals("dec"),
			new FeatureMedals("dec"),
			new FeatureMedals("dec")
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: true, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 605, 103, 0, 0, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 100, 150, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5] { 0, 1, 0, 0, 0 }, 0, 0, 0, 0, 0, -50, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 10, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(612, 2872, 2873, hidden: false, ECharacterFeatureType.Special, 2874, 2875, 2847, new FeatureMedals[3]
		{
			new FeatureMedals("dec"),
			new FeatureMedals("dec"),
			new FeatureMedals("dec")
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: true, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 605, 103, 0, 0, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 100, 150, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5] { 0, 0, 1, 0, 0 }, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 10, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(613, 2876, 2877, hidden: false, ECharacterFeatureType.Special, 2878, 2879, 2847, new FeatureMedals[3]
		{
			new FeatureMedals("dec"),
			new FeatureMedals("dec"),
			new FeatureMedals("dec")
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: true, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 605, 103, 0, 0, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 100, 150, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5] { 0, 0, 0, 1, 0 }, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 10, 10, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 50, 50, 50, 50, 50, 50, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(614, 2880, 2881, hidden: false, ECharacterFeatureType.Special, 2882, 2883, 2847, new FeatureMedals[3]
		{
			new FeatureMedals("dec"),
			new FeatureMedals("dec"),
			new FeatureMedals("dec")
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: true, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 605, 103, 0, 0, -1, -1, 0, 0, null, 0, 0, 0, 0, 50, 0, 100, 150, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5] { 0, 0, 0, 0, 1 }, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 10, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(615, 2884, 2885, hidden: false, ECharacterFeatureType.Special, 2886, 2887, 2888, new FeatureMedals[3]
		{
			new FeatureMedals("inc"),
			new FeatureMedals("inc"),
			new FeatureMedals("inc")
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: true, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: false, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 615, 6, 0, 0, -1, -1, 0, 0, null, -80, 40, 0, 0, 0, 0, 50, 50, 2000, makeConsummateLevelRelated: true, new short[5], new sbyte[5], new sbyte[5], 50, -50, -50, 0, 50, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 300, 300, 300, 300, 300, 300, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(616, 2889, 2890, hidden: false, ECharacterFeatureType.Special, 2891, 2892, 2893, new FeatureMedals[3]
		{
			new FeatureMedals("pos"),
			new FeatureMedals("pos"),
			new FeatureMedals("pos")
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: false, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 616, 103, 0, 0, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 100, 75, 10, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5] { 1, 0, 0, 0, 0 }, 0, 0, 0, 0, 0, 0, 50, 0, 0, 0, 0, 0, 0, 50, 0, 150, 0, 0, 0, 0, 0, 0, 0, 0, 0, 10, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(617, 2894, 2895, hidden: false, ECharacterFeatureType.Special, 2896, 2897, 2893, new FeatureMedals[3]
		{
			new FeatureMedals("pos"),
			new FeatureMedals("pos"),
			new FeatureMedals("pos")
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: false, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 616, 103, 0, 0, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 100, 75, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5] { 0, 1, 0, 0, 0 }, 0, 0, 0, 0, 0, 50, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 10, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(618, 2898, 2899, hidden: false, ECharacterFeatureType.Special, 2900, 2901, 2893, new FeatureMedals[3]
		{
			new FeatureMedals("pos"),
			new FeatureMedals("pos"),
			new FeatureMedals("pos")
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: false, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 616, 103, 0, 0, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 100, 75, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5] { 0, 0, 1, 0, 0 }, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 10, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(619, 2902, 2903, hidden: false, ECharacterFeatureType.Special, 2904, 2905, 2893, new FeatureMedals[3]
		{
			new FeatureMedals("pos"),
			new FeatureMedals("pos"),
			new FeatureMedals("pos")
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: false, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 616, 103, 0, 0, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 100, 75, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5] { 0, 0, 0, 1, 0 }, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 10, 10, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 50, 50, 50, 50, 50, 50, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(620, 2906, 2907, hidden: false, ECharacterFeatureType.Special, 2908, 2909, 2893, new FeatureMedals[3]
		{
			new FeatureMedals("pos"),
			new FeatureMedals("pos"),
			new FeatureMedals("pos")
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: false, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 616, 103, 0, 0, -1, -1, 0, 0, null, 0, 0, 0, 0, 50, 0, 100, 75, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5] { 0, 0, 0, 0, 1 }, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 10, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(621, 2910, 2911, hidden: false, ECharacterFeatureType.Special, 2912, 2913, 2893, new FeatureMedals[3]
		{
			new FeatureMedals("dec"),
			new FeatureMedals("dec"),
			new FeatureMedals("dec")
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: false, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 616, 103, 0, 0, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 100, 150, 1000, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5] { 1, 0, 0, 0, 0 }, 0, 0, 0, 0, 0, 0, -50, 0, 0, 0, 0, 0, 0, 50, 0, 150, 0, 0, 0, 10, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(622, 2914, 2915, hidden: false, ECharacterFeatureType.Special, 2916, 2917, 2893, new FeatureMedals[3]
		{
			new FeatureMedals("dec"),
			new FeatureMedals("dec"),
			new FeatureMedals("dec")
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: false, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 616, 103, 0, 0, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 100, 150, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5] { 0, 1, 0, 0, 0 }, 0, 0, 0, 0, 0, -50, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 10, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(623, 2918, 2919, hidden: false, ECharacterFeatureType.Special, 2920, 2921, 2893, new FeatureMedals[3]
		{
			new FeatureMedals("dec"),
			new FeatureMedals("dec"),
			new FeatureMedals("dec")
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: false, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 616, 103, 0, 0, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 100, 150, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5] { 0, 0, 1, 0, 0 }, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 10, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(624, 2922, 2923, hidden: false, ECharacterFeatureType.Special, 2924, 2925, 2893, new FeatureMedals[3]
		{
			new FeatureMedals("dec"),
			new FeatureMedals("dec"),
			new FeatureMedals("dec")
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: false, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 616, 103, 0, 0, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 100, 150, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5] { 0, 0, 0, 1, 0 }, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 10, 10, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 50, 50, 50, 50, 50, 50, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(625, 2926, 2927, hidden: false, ECharacterFeatureType.Special, 2928, 2929, 2893, new FeatureMedals[3]
		{
			new FeatureMedals("dec"),
			new FeatureMedals("dec"),
			new FeatureMedals("dec")
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: false, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 616, 103, 0, 0, -1, -1, 0, 0, null, 0, 0, 0, 0, 50, 0, 100, 150, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5] { 0, 0, 0, 0, 1 }, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 10, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(626, 2930, 2931, hidden: false, ECharacterFeatureType.Special, 2932, 2933, 2934, new FeatureMedals[3]
		{
			new FeatureMedals(),
			new FeatureMedals(),
			new FeatureMedals("inc")
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: true, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: false, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 220, 6, 0, 0, -1, -1, 0, 0, null, -40, 20, -40, 0, 0, 0, 0, 0, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 100, 100, 100, 100, 100, -100, -100, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(627, 2935, 2936, hidden: false, ECharacterFeatureType.Special, 2937, 2938, 2939, new FeatureMedals[3]
		{
			new FeatureMedals("dec"),
			new FeatureMedals("dec"),
			new FeatureMedals("dec")
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: true, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 627, 601, 0, 0, -1, -1, 0, 0, null, 0, 0, 0, 0, -20, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], -25, -25, -25, -25, -25, -25, -25, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -75, -75, -75, -75, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(628, 2940, 2941, hidden: false, ECharacterFeatureType.Special, 2942, 2943, 2944, new FeatureMedals[3]
		{
			new FeatureMedals("dec"),
			new FeatureMedals("dec"),
			new FeatureMedals("dec")
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: true, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 628, 602, 0, 0, -1, -1, 0, 0, null, 0, 0, 0, 0, -20, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], -25, -25, -25, -25, -25, -25, -25, 0, 0, 0, 0, 0, 0, 0, 0, 0, -75, -75, -75, -75, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(629, 2945, 2946, hidden: false, ECharacterFeatureType.Special, 2947, 2948, 2949, new FeatureMedals[3]
		{
			new FeatureMedals("dec"),
			new FeatureMedals("dec"),
			new FeatureMedals("dec")
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: true, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 629, 603, 0, 0, -1, -1, 0, 0, null, 0, 0, 0, 0, -20, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], -25, -25, -25, -25, -25, -25, -25, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -75, -75, 0, 0, 0, 0, -75, -75, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(630, 2950, 2951, hidden: false, ECharacterFeatureType.Temporary, 2952, 2953, 2954, new FeatureMedals[3]
		{
			new FeatureMedals("pos"),
			new FeatureMedals(),
			new FeatureMedals()
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: true, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 630, 0, 0, 12, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 0, 0, 0, 0, 30, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(631, 2955, 2956, hidden: false, ECharacterFeatureType.Temporary, 2957, 2958, 2959, new FeatureMedals[3]
		{
			new FeatureMedals("pos"),
			new FeatureMedals(),
			new FeatureMedals()
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: true, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 631, 0, 0, 12, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 0, 0, 0, 0, 0, 30, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(632, 2960, 2961, hidden: false, ECharacterFeatureType.Temporary, 2962, 2963, 2964, new FeatureMedals[3]
		{
			new FeatureMedals(),
			new FeatureMedals("pos"),
			new FeatureMedals()
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: true, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 632, 0, 0, 12, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 0, 0, 0, 0, 0, 0, 30, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(633, 2965, 2966, hidden: false, ECharacterFeatureType.Temporary, 2967, 2968, 2969, new FeatureMedals[3]
		{
			new FeatureMedals(),
			new FeatureMedals("pos"),
			new FeatureMedals()
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: true, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 633, 0, 0, 12, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 30, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(634, 2970, 2971, hidden: false, ECharacterFeatureType.Temporary, 2972, 2973, 2974, new FeatureMedals[3]
		{
			new FeatureMedals(),
			new FeatureMedals(),
			new FeatureMedals("pos")
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: true, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 634, 0, 0, 12, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 30, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(635, 2975, 2976, hidden: false, ECharacterFeatureType.Temporary, 2977, 2978, 2979, new FeatureMedals[3]
		{
			new FeatureMedals(),
			new FeatureMedals(),
			new FeatureMedals("pos")
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: true, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 635, 0, 0, 12, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 30, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(636, 2980, 2981, hidden: false, ECharacterFeatureType.Temporary, 2982, 2983, 2984, new FeatureMedals[3]
		{
			new FeatureMedals("neg"),
			new FeatureMedals("neg"),
			new FeatureMedals("neg")
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: true, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 636, 0, 0, 12, -1, -1, 0, 0, null, 0, 0, 0, 10, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5] { 20, 20, 20, 20, 20 }, new sbyte[5], new sbyte[5], 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(637, 2985, 2986, hidden: false, ECharacterFeatureType.Temporary, 2987, 2988, 2989, new FeatureMedals[3]
		{
			new FeatureMedals(),
			new FeatureMedals("pos"),
			new FeatureMedals()
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: true, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 637, 0, 0, 36, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(638, 1100, 2990, hidden: true, ECharacterFeatureType.Special, 2991, 2992, 2993, new FeatureMedals[3]
		{
			new FeatureMedals(),
			new FeatureMedals(),
			new FeatureMedals()
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: true, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: false, inheritableTransferTaiwu: true, ECharacterFeatureDarkAshProtector.None, -1, 638, 0, 0, 0, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 200, 200, 200, 200, 200, 200, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(639, 1105, 2994, hidden: true, ECharacterFeatureType.Special, 2995, 2996, 2997, new FeatureMedals[3]
		{
			new FeatureMedals(),
			new FeatureMedals(),
			new FeatureMedals()
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: true, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: false, inheritableTransferTaiwu: true, ECharacterFeatureDarkAshProtector.None, -1, 639, 0, 0, 0, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 10, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(640, 1110, 2998, hidden: true, ECharacterFeatureType.Special, 2999, 3000, 3001, new FeatureMedals[3]
		{
			new FeatureMedals(),
			new FeatureMedals(),
			new FeatureMedals()
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: true, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: false, inheritableTransferTaiwu: true, ECharacterFeatureDarkAshProtector.None, -1, 640, 0, 0, 0, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 10, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(641, 1115, 3002, hidden: true, ECharacterFeatureType.Special, 3003, 3004, 3005, new FeatureMedals[3]
		{
			new FeatureMedals(),
			new FeatureMedals(),
			new FeatureMedals()
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: true, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: false, inheritableTransferTaiwu: true, ECharacterFeatureDarkAshProtector.None, -1, 641, 0, 0, 0, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 10, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(642, 1120, 3006, hidden: true, ECharacterFeatureType.Special, 3007, 3008, 3009, new FeatureMedals[3]
		{
			new FeatureMedals(),
			new FeatureMedals(),
			new FeatureMedals()
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: true, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: false, inheritableTransferTaiwu: true, ECharacterFeatureDarkAshProtector.None, -1, 642, 0, 0, 0, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 10, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 10, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(643, 1125, 3010, hidden: true, ECharacterFeatureType.Special, 3011, 3012, 3013, new FeatureMedals[3]
		{
			new FeatureMedals(),
			new FeatureMedals(),
			new FeatureMedals()
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: true, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: false, inheritableTransferTaiwu: true, ECharacterFeatureDarkAshProtector.None, -1, 643, 0, 0, 0, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 0, 0, 0, 0, 0, 10, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(644, 1130, 3014, hidden: true, ECharacterFeatureType.Special, 3015, 3016, 3017, new FeatureMedals[3]
		{
			new FeatureMedals(),
			new FeatureMedals(),
			new FeatureMedals()
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: true, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: false, inheritableTransferTaiwu: true, ECharacterFeatureDarkAshProtector.None, -1, 644, 0, 0, 0, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 0, 0, 0, 0, 0, 0, 10, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(645, 1135, 3018, hidden: true, ECharacterFeatureType.Special, 3019, 3020, 3021, new FeatureMedals[3]
		{
			new FeatureMedals(),
			new FeatureMedals(),
			new FeatureMedals()
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: true, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: false, inheritableTransferTaiwu: true, ECharacterFeatureDarkAshProtector.None, -1, 645, 0, 0, 0, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 0, 0, 0, 0, 10, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(646, 1140, 3022, hidden: true, ECharacterFeatureType.Special, 3023, 3024, 3025, new FeatureMedals[3]
		{
			new FeatureMedals(),
			new FeatureMedals(),
			new FeatureMedals()
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: true, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: false, inheritableTransferTaiwu: true, ECharacterFeatureDarkAshProtector.None, -1, 646, 0, 0, 0, -1, -1, 0, 0, null, 0, 0, 0, 0, 10, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(647, 1100, 3026, hidden: true, ECharacterFeatureType.Special, 3027, 3028, 3029, new FeatureMedals[3]
		{
			new FeatureMedals(),
			new FeatureMedals(),
			new FeatureMedals()
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: true, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: false, inheritableTransferTaiwu: true, ECharacterFeatureDarkAshProtector.None, -1, 638, 0, 0, 0, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 400, 400, 400, 400, 400, 400, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(648, 1105, 3030, hidden: true, ECharacterFeatureType.Special, 3031, 3032, 3033, new FeatureMedals[3]
		{
			new FeatureMedals(),
			new FeatureMedals(),
			new FeatureMedals()
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: true, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: false, inheritableTransferTaiwu: true, ECharacterFeatureDarkAshProtector.None, -1, 639, 0, 0, 0, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 15, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(649, 1110, 3034, hidden: true, ECharacterFeatureType.Special, 3035, 3036, 3037, new FeatureMedals[3]
		{
			new FeatureMedals(),
			new FeatureMedals(),
			new FeatureMedals()
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: true, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: false, inheritableTransferTaiwu: true, ECharacterFeatureDarkAshProtector.None, -1, 640, 0, 0, 0, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 15, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(650, 1115, 3038, hidden: true, ECharacterFeatureType.Special, 3039, 3040, 3041, new FeatureMedals[3]
		{
			new FeatureMedals(),
			new FeatureMedals(),
			new FeatureMedals()
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: true, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: false, inheritableTransferTaiwu: true, ECharacterFeatureDarkAshProtector.None, -1, 641, 0, 0, 0, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 15, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(651, 1120, 3042, hidden: true, ECharacterFeatureType.Special, 3043, 3044, 3045, new FeatureMedals[3]
		{
			new FeatureMedals(),
			new FeatureMedals(),
			new FeatureMedals()
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: true, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: false, inheritableTransferTaiwu: true, ECharacterFeatureDarkAshProtector.None, -1, 642, 0, 0, 0, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 15, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 15, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(652, 1125, 3046, hidden: true, ECharacterFeatureType.Special, 3047, 3048, 3049, new FeatureMedals[3]
		{
			new FeatureMedals(),
			new FeatureMedals(),
			new FeatureMedals()
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: true, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: false, inheritableTransferTaiwu: true, ECharacterFeatureDarkAshProtector.None, -1, 643, 0, 0, 0, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 0, 0, 0, 0, 0, 15, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(653, 1130, 3050, hidden: true, ECharacterFeatureType.Special, 3051, 3052, 3053, new FeatureMedals[3]
		{
			new FeatureMedals(),
			new FeatureMedals(),
			new FeatureMedals()
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: true, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: false, inheritableTransferTaiwu: true, ECharacterFeatureDarkAshProtector.None, -1, 644, 0, 0, 0, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 0, 0, 0, 0, 0, 0, 15, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(654, 1135, 3054, hidden: true, ECharacterFeatureType.Special, 3055, 3056, 3057, new FeatureMedals[3]
		{
			new FeatureMedals(),
			new FeatureMedals(),
			new FeatureMedals()
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: true, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: false, inheritableTransferTaiwu: true, ECharacterFeatureDarkAshProtector.None, -1, 645, 0, 0, 0, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 0, 0, 0, 0, 15, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(655, 1140, 3058, hidden: true, ECharacterFeatureType.Special, 3059, 3060, 3061, new FeatureMedals[3]
		{
			new FeatureMedals(),
			new FeatureMedals(),
			new FeatureMedals()
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: true, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: false, inheritableTransferTaiwu: true, ECharacterFeatureDarkAshProtector.None, -1, 646, 0, 0, 0, -1, -1, 0, 0, null, 0, 0, 0, 0, 15, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(656, 1100, 3062, hidden: true, ECharacterFeatureType.Special, 3063, 3064, 3065, new FeatureMedals[3]
		{
			new FeatureMedals(),
			new FeatureMedals(),
			new FeatureMedals()
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: true, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: false, inheritableTransferTaiwu: true, ECharacterFeatureDarkAshProtector.None, -1, 638, 0, 0, 0, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 600, 600, 600, 600, 600, 600, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(657, 1105, 3066, hidden: true, ECharacterFeatureType.Special, 3067, 3068, 3069, new FeatureMedals[3]
		{
			new FeatureMedals(),
			new FeatureMedals(),
			new FeatureMedals()
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: true, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: false, inheritableTransferTaiwu: true, ECharacterFeatureDarkAshProtector.None, -1, 639, 0, 0, 0, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 20, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(658, 1110, 3070, hidden: true, ECharacterFeatureType.Special, 3071, 3072, 3073, new FeatureMedals[3]
		{
			new FeatureMedals(),
			new FeatureMedals(),
			new FeatureMedals()
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: true, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: false, inheritableTransferTaiwu: true, ECharacterFeatureDarkAshProtector.None, -1, 640, 0, 0, 0, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 20, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(659, 1115, 3074, hidden: true, ECharacterFeatureType.Special, 3075, 3076, 3077, new FeatureMedals[3]
		{
			new FeatureMedals(),
			new FeatureMedals(),
			new FeatureMedals()
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: true, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: false, inheritableTransferTaiwu: true, ECharacterFeatureDarkAshProtector.None, -1, 641, 0, 0, 0, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 20, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
	}

	private void CreateItems11()
	{
		_dataArray.Add(new CharacterFeatureItem(660, 1120, 3078, hidden: true, ECharacterFeatureType.Special, 3079, 3080, 3081, new FeatureMedals[3]
		{
			new FeatureMedals(),
			new FeatureMedals(),
			new FeatureMedals()
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: true, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: false, inheritableTransferTaiwu: true, ECharacterFeatureDarkAshProtector.None, -1, 642, 0, 0, 0, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 20, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 20, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(661, 1125, 3082, hidden: true, ECharacterFeatureType.Special, 3083, 3084, 3085, new FeatureMedals[3]
		{
			new FeatureMedals(),
			new FeatureMedals(),
			new FeatureMedals()
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: true, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: false, inheritableTransferTaiwu: true, ECharacterFeatureDarkAshProtector.None, -1, 643, 0, 0, 0, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 0, 0, 0, 0, 0, 20, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(662, 1130, 3086, hidden: true, ECharacterFeatureType.Special, 3087, 3088, 3089, new FeatureMedals[3]
		{
			new FeatureMedals(),
			new FeatureMedals(),
			new FeatureMedals()
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: true, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: false, inheritableTransferTaiwu: true, ECharacterFeatureDarkAshProtector.None, -1, 644, 0, 0, 0, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 0, 0, 0, 0, 0, 0, 20, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(663, 1135, 3090, hidden: true, ECharacterFeatureType.Special, 3091, 3092, 3093, new FeatureMedals[3]
		{
			new FeatureMedals(),
			new FeatureMedals(),
			new FeatureMedals()
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: true, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: false, inheritableTransferTaiwu: true, ECharacterFeatureDarkAshProtector.None, -1, 645, 0, 0, 0, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 0, 0, 0, 0, 20, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(664, 1140, 3094, hidden: true, ECharacterFeatureType.Special, 3095, 3096, 3097, new FeatureMedals[3]
		{
			new FeatureMedals(),
			new FeatureMedals(),
			new FeatureMedals()
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: true, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: false, inheritableTransferTaiwu: true, ECharacterFeatureDarkAshProtector.None, -1, 646, 0, 0, 0, -1, -1, 0, 0, null, 0, 0, 0, 0, 20, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(665, 1100, 3098, hidden: true, ECharacterFeatureType.Special, 3099, 3100, 3101, new FeatureMedals[3]
		{
			new FeatureMedals(),
			new FeatureMedals(),
			new FeatureMedals()
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: true, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: false, inheritableTransferTaiwu: true, ECharacterFeatureDarkAshProtector.None, -1, 638, 0, 0, 0, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 800, 800, 800, 800, 800, 800, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(666, 1105, 3102, hidden: true, ECharacterFeatureType.Special, 3103, 3104, 3105, new FeatureMedals[3]
		{
			new FeatureMedals(),
			new FeatureMedals(),
			new FeatureMedals()
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: true, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: false, inheritableTransferTaiwu: true, ECharacterFeatureDarkAshProtector.None, -1, 639, 0, 0, 0, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 30, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(667, 1110, 3106, hidden: true, ECharacterFeatureType.Special, 3107, 3108, 3109, new FeatureMedals[3]
		{
			new FeatureMedals(),
			new FeatureMedals(),
			new FeatureMedals()
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: true, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: false, inheritableTransferTaiwu: true, ECharacterFeatureDarkAshProtector.None, -1, 640, 0, 0, 0, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 30, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(668, 1115, 3110, hidden: true, ECharacterFeatureType.Special, 3111, 3112, 3113, new FeatureMedals[3]
		{
			new FeatureMedals(),
			new FeatureMedals(),
			new FeatureMedals()
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: true, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: false, inheritableTransferTaiwu: true, ECharacterFeatureDarkAshProtector.None, -1, 641, 0, 0, 0, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 30, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(669, 1120, 3114, hidden: true, ECharacterFeatureType.Special, 3115, 3116, 3117, new FeatureMedals[3]
		{
			new FeatureMedals(),
			new FeatureMedals(),
			new FeatureMedals()
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: true, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: false, inheritableTransferTaiwu: true, ECharacterFeatureDarkAshProtector.None, -1, 642, 0, 0, 0, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 30, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 30, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(670, 1125, 3118, hidden: true, ECharacterFeatureType.Special, 3119, 3120, 3121, new FeatureMedals[3]
		{
			new FeatureMedals(),
			new FeatureMedals(),
			new FeatureMedals()
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: true, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: false, inheritableTransferTaiwu: true, ECharacterFeatureDarkAshProtector.None, -1, 643, 0, 0, 0, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 0, 0, 0, 0, 0, 30, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(671, 1130, 3122, hidden: true, ECharacterFeatureType.Special, 3123, 3124, 3125, new FeatureMedals[3]
		{
			new FeatureMedals(),
			new FeatureMedals(),
			new FeatureMedals()
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: true, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: false, inheritableTransferTaiwu: true, ECharacterFeatureDarkAshProtector.None, -1, 644, 0, 0, 0, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 0, 0, 0, 0, 0, 0, 30, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(672, 1135, 3126, hidden: true, ECharacterFeatureType.Special, 3127, 3128, 3129, new FeatureMedals[3]
		{
			new FeatureMedals(),
			new FeatureMedals(),
			new FeatureMedals()
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: true, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: false, inheritableTransferTaiwu: true, ECharacterFeatureDarkAshProtector.None, -1, 645, 0, 0, 0, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 0, 0, 0, 0, 30, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(673, 1140, 3130, hidden: true, ECharacterFeatureType.Special, 3131, 3132, 3133, new FeatureMedals[3]
		{
			new FeatureMedals(),
			new FeatureMedals(),
			new FeatureMedals()
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: true, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: false, inheritableTransferTaiwu: true, ECharacterFeatureDarkAshProtector.None, -1, 646, 0, 0, 0, -1, -1, 0, 0, null, 0, 0, 0, 0, 30, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(674, 3134, 3135, hidden: false, ECharacterFeatureType.Special, 3136, 3137, 3138, new FeatureMedals[3]
		{
			new FeatureMedals("inc"),
			new FeatureMedals("inc"),
			new FeatureMedals("inc")
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: false, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 674, 6, 0, 0, -1, -1, 0, 0, null, -60, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5] { 0, 0, 0, 20, 0 }, new sbyte[5], 0, 0, 0, 0, 0, 0, -50, 0, 0, 0, 30, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(675, 3139, 3140, hidden: false, ECharacterFeatureType.Special, 3141, 3142, 3143, new FeatureMedals[3]
		{
			new FeatureMedals("inc"),
			new FeatureMedals("inc"),
			new FeatureMedals("inc")
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: false, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 675, 6, 0, 0, -1, -1, 0, 0, null, 0, 0, -30, 0, -50, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 0, 0, 0, -50, 0, 30, 0, 0, 0, 0, -100, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(676, 3144, 3145, hidden: false, ECharacterFeatureType.Special, 3146, 3147, 3148, new FeatureMedals[3]
		{
			new FeatureMedals("inc"),
			new FeatureMedals("inc"),
			new FeatureMedals("inc")
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: false, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 676, 6, 0, 0, -1, -1, 0, 0, null, 0, 0, -80, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 0, 0, 0, -50, 0, 0, 0, 0, 0, 30, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(677, 3149, 3150, hidden: false, ECharacterFeatureType.Special, 3151, 3152, 3153, new FeatureMedals[3]
		{
			new FeatureMedals("inc"),
			new FeatureMedals("inc"),
			new FeatureMedals("inc")
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: false, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 677, 6, 0, 0, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 0, 0, 0, -50, 30, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(678, 3154, 3155, hidden: false, ECharacterFeatureType.Special, 3156, 3157, 3158, new FeatureMedals[3]
		{
			new FeatureMedals("inc"),
			new FeatureMedals("inc"),
			new FeatureMedals("inc")
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: false, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 678, 6, 0, 0, -1, -1, 0, 0, null, 0, 0, -50, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 0, 0, 0, -50, 0, 0, 0, 0, 30, 0, 0, 0, 200, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(679, 3159, 3160, hidden: false, ECharacterFeatureType.Special, 3161, 3162, 3163, new FeatureMedals[3]
		{
			new FeatureMedals("inc"),
			new FeatureMedals("inc"),
			new FeatureMedals("inc")
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: false, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 679, 6, 0, 0, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5] { 0, 80, 0, 0, 0 }, new sbyte[5], new sbyte[5], 0, 50, 0, 0, 0, 0, -50, 0, 30, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(680, 3164, 3165, hidden: false, ECharacterFeatureType.Special, 3166, 3167, 3168, new FeatureMedals[3]
		{
			new FeatureMedals("inc"),
			new FeatureMedals("inc"),
			new FeatureMedals("inc")
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: false, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 680, 6, 0, 0, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 0, 0, 0, -50, 0, 0, 30, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(681, 3169, 3170, hidden: false, ECharacterFeatureType.Special, 3171, 3172, 3173, new FeatureMedals[3]
		{
			new FeatureMedals("inc"),
			new FeatureMedals("inc"),
			new FeatureMedals("inc")
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: false, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 681, 6, 0, 0, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 0, 0, 100, -50, 0, 0, 0, 0, 0, 0, 0, 0, 150, 30, 30, 30, 30, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(682, 3174, 3175, hidden: false, ECharacterFeatureType.Special, 3176, 3177, 3178, new FeatureMedals[3]
		{
			new FeatureMedals("inc"),
			new FeatureMedals("inc"),
			new FeatureMedals("inc")
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: false, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 682, 6, 0, 0, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 0, 0, -100, 50, 0, 0, 0, 0, 0, 0, 0, 0, 150, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(683, 3179, 3180, hidden: false, ECharacterFeatureType.Special, 3181, 3182, 3183, new FeatureMedals[3]
		{
			new FeatureMedals("inc"),
			new FeatureMedals("inc"),
			new FeatureMedals("inc")
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: false, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 683, 6, 0, 0, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5] { 0, 0, 0, 80, 0 }, new sbyte[5], new sbyte[5], 0, 0, 0, 0, 0, 0, -50, 0, 0, 0, 30, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(684, 3184, 3185, hidden: false, ECharacterFeatureType.Special, 3186, 3187, 3188, new FeatureMedals[3]
		{
			new FeatureMedals("inc"),
			new FeatureMedals("inc"),
			new FeatureMedals("inc")
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: false, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 684, 6, 0, 0, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 0, 0, 0, -50, 0, 0, 0, 0, 15, 15, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(685, 3189, 3190, hidden: false, ECharacterFeatureType.Special, 3191, 3192, 3193, new FeatureMedals[3]
		{
			new FeatureMedals("inc"),
			new FeatureMedals("inc"),
			new FeatureMedals("inc")
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: false, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 685, 6, 0, 0, -1, -1, 0, 0, null, -80, 0, -50, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 0, 0, 0, -50, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(686, 3194, 3195, hidden: false, ECharacterFeatureType.Special, 3196, 3197, 3198, new FeatureMedals[3]
		{
			new FeatureMedals("inc"),
			new FeatureMedals("inc"),
			new FeatureMedals("inc")
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: false, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 686, 6, 0, 0, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 0, 0, 0, -50, 0, 0, 30, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(687, 3199, 3200, hidden: false, ECharacterFeatureType.Special, 3201, 3202, 3203, new FeatureMedals[3]
		{
			new FeatureMedals("inc"),
			new FeatureMedals("inc"),
			new FeatureMedals("inc")
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: false, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 687, 6, 0, 0, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 0, 0, 0, -50, 15, 15, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(688, 3204, 3205, hidden: false, ECharacterFeatureType.Special, 3206, 3207, 3208, new FeatureMedals[3]
		{
			new FeatureMedals("inc"),
			new FeatureMedals("inc"),
			new FeatureMedals("inc")
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: false, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 688, 6, 0, 0, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 0, 0, 0, -50, 0, 0, 15, 15, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(689, 3209, 3210, hidden: false, ECharacterFeatureType.Special, 3211, 3212, 3213, new FeatureMedals[3]
		{
			new FeatureMedals("inc"),
			new FeatureMedals("inc"),
			new FeatureMedals("inc")
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: false, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 689, 6, 0, 0, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 0, 0, 0, -50, 0, 0, 0, 0, 0, 30, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(690, 3214, 3215, hidden: false, ECharacterFeatureType.Special, 3216, 3217, 3218, new FeatureMedals[3]
		{
			new FeatureMedals("inc"),
			new FeatureMedals("inc"),
			new FeatureMedals("inc")
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: false, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 690, 6, 0, 0, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 0, 0, 0, -50, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(691, 3219, 3220, hidden: false, ECharacterFeatureType.Special, 3221, 3222, 3223, new FeatureMedals[3]
		{
			new FeatureMedals("inc"),
			new FeatureMedals("inc"),
			new FeatureMedals("inc")
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: false, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 691, 6, 0, 0, -1, -1, 0, 0, null, 0, 80, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 50, 0, 0, 0, 0, -50, 0, 0, 0, 0, 30, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(692, 3224, 3225, hidden: false, ECharacterFeatureType.Temporary, 3226, 3227, 3228, new FeatureMedals[3]
		{
			new FeatureMedals("inc"),
			new FeatureMedals("inc"),
			new FeatureMedals("inc")
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: false, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 692, 6, 0, 12, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 6, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(693, 3229, 3230, hidden: false, ECharacterFeatureType.Temporary, 3231, 3232, 3233, new FeatureMedals[3]
		{
			new FeatureMedals("inc"),
			new FeatureMedals("inc"),
			new FeatureMedals("inc")
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: false, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 692, 6, 0, 12, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 12, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(694, 3234, 3235, hidden: false, ECharacterFeatureType.Temporary, 3236, 3237, 3238, new FeatureMedals[3]
		{
			new FeatureMedals("inc"),
			new FeatureMedals("inc"),
			new FeatureMedals("inc")
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: false, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 692, 6, 0, 12, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 18, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(695, 3239, 3240, hidden: false, ECharacterFeatureType.Temporary, 3241, 3242, 3243, new FeatureMedals[3]
		{
			new FeatureMedals("inc"),
			new FeatureMedals("inc"),
			new FeatureMedals("inc")
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: false, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 695, 6, 0, 12, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 6, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(696, 3244, 3245, hidden: false, ECharacterFeatureType.Temporary, 3246, 3247, 3248, new FeatureMedals[3]
		{
			new FeatureMedals("inc"),
			new FeatureMedals("inc"),
			new FeatureMedals("inc")
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: false, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 695, 6, 0, 12, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 12, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(697, 3249, 3250, hidden: false, ECharacterFeatureType.Temporary, 3251, 3252, 3253, new FeatureMedals[3]
		{
			new FeatureMedals("inc"),
			new FeatureMedals("inc"),
			new FeatureMedals("inc")
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: false, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 695, 6, 0, 12, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 18, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(698, 3254, 3255, hidden: false, ECharacterFeatureType.Temporary, 3256, 3257, 3258, new FeatureMedals[3]
		{
			new FeatureMedals("inc"),
			new FeatureMedals("inc"),
			new FeatureMedals("inc")
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: false, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 698, 6, 0, 12, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 6, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(699, 3259, 3260, hidden: false, ECharacterFeatureType.Temporary, 3261, 3262, 3263, new FeatureMedals[3]
		{
			new FeatureMedals("inc"),
			new FeatureMedals("inc"),
			new FeatureMedals("inc")
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: false, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 698, 6, 0, 12, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 12, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(700, 3264, 3265, hidden: false, ECharacterFeatureType.Temporary, 3266, 3267, 3268, new FeatureMedals[3]
		{
			new FeatureMedals("inc"),
			new FeatureMedals("inc"),
			new FeatureMedals("inc")
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: false, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 698, 6, 0, 12, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 18, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(701, 3269, 3270, hidden: false, ECharacterFeatureType.Temporary, 3271, 3272, 3273, new FeatureMedals[3]
		{
			new FeatureMedals("inc"),
			new FeatureMedals("inc"),
			new FeatureMedals("inc")
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: false, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 701, 6, 0, 12, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 6, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(702, 3274, 3275, hidden: false, ECharacterFeatureType.Temporary, 3276, 3277, 3278, new FeatureMedals[3]
		{
			new FeatureMedals("inc"),
			new FeatureMedals("inc"),
			new FeatureMedals("inc")
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: false, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 701, 6, 0, 12, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 12, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(703, 3279, 3280, hidden: false, ECharacterFeatureType.Temporary, 3281, 3282, 3283, new FeatureMedals[3]
		{
			new FeatureMedals("inc"),
			new FeatureMedals("inc"),
			new FeatureMedals("inc")
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: false, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 701, 6, 0, 12, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 18, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(704, 3284, 3285, hidden: false, ECharacterFeatureType.Temporary, 3286, 3287, 3288, new FeatureMedals[3]
		{
			new FeatureMedals("inc"),
			new FeatureMedals("inc"),
			new FeatureMedals("inc")
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: false, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 704, 6, 0, 12, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 6, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(705, 3289, 3290, hidden: false, ECharacterFeatureType.Temporary, 3291, 3292, 3293, new FeatureMedals[3]
		{
			new FeatureMedals("inc"),
			new FeatureMedals("inc"),
			new FeatureMedals("inc")
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: false, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 704, 6, 0, 12, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 12, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(706, 3294, 3295, hidden: false, ECharacterFeatureType.Temporary, 3296, 3297, 3298, new FeatureMedals[3]
		{
			new FeatureMedals("inc"),
			new FeatureMedals("inc"),
			new FeatureMedals("inc")
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: false, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 704, 6, 0, 12, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 18, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(707, 3299, 3300, hidden: false, ECharacterFeatureType.Temporary, 3301, 3302, 3303, new FeatureMedals[3]
		{
			new FeatureMedals("inc"),
			new FeatureMedals("inc"),
			new FeatureMedals("inc")
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: false, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 707, 6, 0, 12, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 6, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(708, 3304, 3305, hidden: false, ECharacterFeatureType.Temporary, 3306, 3307, 3308, new FeatureMedals[3]
		{
			new FeatureMedals("inc"),
			new FeatureMedals("inc"),
			new FeatureMedals("inc")
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: false, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 707, 6, 0, 12, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 12, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(709, 3309, 3310, hidden: false, ECharacterFeatureType.Temporary, 3311, 3312, 3313, new FeatureMedals[3]
		{
			new FeatureMedals("inc"),
			new FeatureMedals("inc"),
			new FeatureMedals("inc")
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: false, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 707, 6, 0, 12, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 18, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(710, 3314, 3315, hidden: false, ECharacterFeatureType.Temporary, 3316, 3317, 3318, new FeatureMedals[3]
		{
			new FeatureMedals("inc"),
			new FeatureMedals("inc"),
			new FeatureMedals("inc")
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: false, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 710, 6, 0, 12, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 6, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(711, 3319, 3320, hidden: false, ECharacterFeatureType.Temporary, 3321, 3322, 3323, new FeatureMedals[3]
		{
			new FeatureMedals("inc"),
			new FeatureMedals("inc"),
			new FeatureMedals("inc")
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: false, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 710, 6, 0, 12, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 12, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(712, 3324, 3325, hidden: false, ECharacterFeatureType.Temporary, 3326, 3327, 3328, new FeatureMedals[3]
		{
			new FeatureMedals("inc"),
			new FeatureMedals("inc"),
			new FeatureMedals("inc")
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: false, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 710, 6, 0, 12, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 18, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(713, 3329, 3330, hidden: false, ECharacterFeatureType.Temporary, 3331, 3332, 3333, new FeatureMedals[3]
		{
			new FeatureMedals("inc"),
			new FeatureMedals("inc"),
			new FeatureMedals("inc")
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: false, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 713, 6, 0, 12, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 6, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(714, 3334, 3335, hidden: false, ECharacterFeatureType.Temporary, 3336, 3337, 3338, new FeatureMedals[3]
		{
			new FeatureMedals("inc"),
			new FeatureMedals("inc"),
			new FeatureMedals("inc")
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: false, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 713, 6, 0, 12, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 12, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(715, 3339, 3340, hidden: false, ECharacterFeatureType.Temporary, 3341, 3342, 3343, new FeatureMedals[3]
		{
			new FeatureMedals("inc"),
			new FeatureMedals("inc"),
			new FeatureMedals("inc")
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: false, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 713, 6, 0, 12, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 18, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(716, 3344, 3345, hidden: false, ECharacterFeatureType.Temporary, 3346, 3347, 3348, new FeatureMedals[3]
		{
			new FeatureMedals("inc"),
			new FeatureMedals("inc"),
			new FeatureMedals("inc")
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: false, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 716, 6, 0, 12, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 6, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(717, 3349, 3350, hidden: false, ECharacterFeatureType.Temporary, 3351, 3352, 3353, new FeatureMedals[3]
		{
			new FeatureMedals("inc"),
			new FeatureMedals("inc"),
			new FeatureMedals("inc")
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: false, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 716, 6, 0, 12, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 12, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(718, 3354, 3355, hidden: false, ECharacterFeatureType.Temporary, 3356, 3357, 3358, new FeatureMedals[3]
		{
			new FeatureMedals("inc"),
			new FeatureMedals("inc"),
			new FeatureMedals("inc")
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: false, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 716, 6, 0, 12, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 18, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(719, 3359, 3360, hidden: false, ECharacterFeatureType.Temporary, 3361, 3362, 3363, new FeatureMedals[3]
		{
			new FeatureMedals("inc"),
			new FeatureMedals("inc"),
			new FeatureMedals("inc")
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: false, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 719, 6, 0, 12, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 6, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
	}

	private void CreateItems12()
	{
		_dataArray.Add(new CharacterFeatureItem(720, 3364, 3365, hidden: false, ECharacterFeatureType.Temporary, 3366, 3367, 3368, new FeatureMedals[3]
		{
			new FeatureMedals("inc"),
			new FeatureMedals("inc"),
			new FeatureMedals("inc")
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: false, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 719, 6, 0, 12, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 12, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(721, 3369, 3370, hidden: false, ECharacterFeatureType.Temporary, 3371, 3372, 3373, new FeatureMedals[3]
		{
			new FeatureMedals("inc"),
			new FeatureMedals("inc"),
			new FeatureMedals("inc")
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: false, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 719, 6, 0, 12, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 18, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(722, 3374, 3375, hidden: false, ECharacterFeatureType.Temporary, 3376, 3377, 3378, new FeatureMedals[3]
		{
			new FeatureMedals("inc"),
			new FeatureMedals("inc"),
			new FeatureMedals("inc")
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: false, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 722, 6, 0, 12, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 6, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(723, 3379, 3380, hidden: false, ECharacterFeatureType.Temporary, 3381, 3382, 3383, new FeatureMedals[3]
		{
			new FeatureMedals("inc"),
			new FeatureMedals("inc"),
			new FeatureMedals("inc")
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: false, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 722, 6, 0, 12, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 12, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(724, 3384, 3385, hidden: false, ECharacterFeatureType.Temporary, 3386, 3387, 3388, new FeatureMedals[3]
		{
			new FeatureMedals("inc"),
			new FeatureMedals("inc"),
			new FeatureMedals("inc")
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: false, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 722, 6, 0, 12, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 18, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(725, 3389, 3390, hidden: false, ECharacterFeatureType.Temporary, 3391, 3392, 3393, new FeatureMedals[3]
		{
			new FeatureMedals("inc"),
			new FeatureMedals("inc"),
			new FeatureMedals("inc")
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: false, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 725, 6, 0, 12, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 6, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(726, 3394, 3395, hidden: false, ECharacterFeatureType.Temporary, 3396, 3397, 3398, new FeatureMedals[3]
		{
			new FeatureMedals("inc"),
			new FeatureMedals("inc"),
			new FeatureMedals("inc")
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: false, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 725, 6, 0, 12, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 12, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(727, 3399, 3400, hidden: false, ECharacterFeatureType.Temporary, 3401, 3402, 3403, new FeatureMedals[3]
		{
			new FeatureMedals("inc"),
			new FeatureMedals("inc"),
			new FeatureMedals("inc")
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: false, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 725, 6, 0, 12, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 18, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(728, 3404, 3405, hidden: false, ECharacterFeatureType.Temporary, 3406, 3407, 3408, new FeatureMedals[3]
		{
			new FeatureMedals("inc"),
			new FeatureMedals("inc"),
			new FeatureMedals("inc")
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: false, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 728, 6, 0, 12, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 6, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(729, 3409, 3410, hidden: false, ECharacterFeatureType.Temporary, 3411, 3412, 3413, new FeatureMedals[3]
		{
			new FeatureMedals("inc"),
			new FeatureMedals("inc"),
			new FeatureMedals("inc")
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: false, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 728, 6, 0, 12, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 12, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(730, 3414, 3415, hidden: false, ECharacterFeatureType.Temporary, 3416, 3417, 3418, new FeatureMedals[3]
		{
			new FeatureMedals("inc"),
			new FeatureMedals("inc"),
			new FeatureMedals("inc")
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: false, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 728, 6, 0, 12, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 18, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(731, 3419, 3420, hidden: false, ECharacterFeatureType.Temporary, 3421, 3422, 3423, new FeatureMedals[3]
		{
			new FeatureMedals("inc"),
			new FeatureMedals("inc"),
			new FeatureMedals("inc")
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: false, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 731, 6, 0, 12, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 6, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(732, 3424, 3425, hidden: false, ECharacterFeatureType.Temporary, 3426, 3427, 3428, new FeatureMedals[3]
		{
			new FeatureMedals("inc"),
			new FeatureMedals("inc"),
			new FeatureMedals("inc")
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: false, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 731, 6, 0, 12, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 12, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(733, 3429, 3430, hidden: false, ECharacterFeatureType.Temporary, 3431, 3432, 3433, new FeatureMedals[3]
		{
			new FeatureMedals("inc"),
			new FeatureMedals("inc"),
			new FeatureMedals("inc")
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: false, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 731, 6, 0, 12, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 18, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(734, 3434, 3435, hidden: false, ECharacterFeatureType.Special, 3436, 3437, 3438, new FeatureMedals[3]
		{
			new FeatureMedals("inc"),
			new FeatureMedals(),
			new FeatureMedals()
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: true, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 734, 6, 0, 0, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 0, 0, 0, 20, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 12, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(735, 3439, 3440, hidden: false, ECharacterFeatureType.Special, 3441, 3442, 3443, new FeatureMedals[3]
		{
			new FeatureMedals(),
			new FeatureMedals("inc"),
			new FeatureMedals()
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: true, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 734, 6, 0, 0, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 0, 20, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 6, 6, 0, 0, 6, 6, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(736, 3444, 3445, hidden: false, ECharacterFeatureType.Special, 3446, 3447, 3448, new FeatureMedals[3]
		{
			new FeatureMedals(),
			new FeatureMedals("inc"),
			new FeatureMedals()
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: true, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 734, 6, 0, 0, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 20, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 8, 8, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(737, 3449, 3450, hidden: false, ECharacterFeatureType.Special, 3451, 3452, 3453, new FeatureMedals[3]
		{
			new FeatureMedals(),
			new FeatureMedals(),
			new FeatureMedals("inc")
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: true, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 734, 6, 0, 0, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 20, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 12, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(738, 3454, 3455, hidden: false, ECharacterFeatureType.Special, 3456, 3457, 3458, new FeatureMedals[3]
		{
			new FeatureMedals(),
			new FeatureMedals(),
			new FeatureMedals("inc")
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: true, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 734, 6, 0, 0, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 20, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 6, 6, 6, 6, 0, 6, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(739, 3459, 3460, hidden: false, ECharacterFeatureType.Special, 3461, 3462, 3463, new FeatureMedals[3]
		{
			new FeatureMedals("inc"),
			new FeatureMedals(),
			new FeatureMedals()
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: true, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 734, 6, 0, 0, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 20, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 6, 6, 0, 0, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(740, 3464, 3465, hidden: false, ECharacterFeatureType.Special, 3466, 3467, 3468, new FeatureMedals[3]
		{
			new FeatureMedals(),
			new FeatureMedals(),
			new FeatureMedals("inc")
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: true, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 734, 6, 0, 0, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 0, 0, 20, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 12, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(741, 3469, 3470, hidden: false, ECharacterFeatureType.Special, 3471, 3472, 2984, new FeatureMedals[3]
		{
			new FeatureMedals(),
			new FeatureMedals("inc", "inc"),
			new FeatureMedals("inc")
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: false, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 741, 8, 0, 60, -1, -1, 0, 0, null, 0, 0, 0, 5, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 20, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 20, 0, 0, 0, 0, 0, 20, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(742, 3473, 3474, hidden: false, ECharacterFeatureType.Special, 3475, 3476, 2984, new FeatureMedals[3]
		{
			new FeatureMedals("inc", "inc"),
			new FeatureMedals("inc"),
			new FeatureMedals()
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: false, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 742, 8, 0, 60, -1, -1, 0, 0, null, 0, 0, 0, 5, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 0, 20, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 20, 0, 0, 0, 0, 0, 20, 0, 0, 0, 0, 0, 0, 0, 0, 100, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(743, 3477, 3478, hidden: false, ECharacterFeatureType.Special, 3479, 3480, 2984, new FeatureMedals[3]
		{
			new FeatureMedals(),
			new FeatureMedals("inc", "inc", "inc"),
			new FeatureMedals()
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: false, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 743, 8, 0, 60, -1, -1, 0, 0, null, 0, 0, 0, 5, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 20, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 10, 0, 10, 0, 0, 0, 10, 0, 10, 0, 0, 0, 0, 0, 100, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(744, 3481, 3482, hidden: false, ECharacterFeatureType.Special, 3483, 3484, 2984, new FeatureMedals[3]
		{
			new FeatureMedals("inc"),
			new FeatureMedals("inc"),
			new FeatureMedals("inc")
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: false, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 744, 8, 0, 60, -1, -1, 0, 0, null, 0, 0, 0, 5, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 0, 0, 20, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 10, 0, 10, 0, 0, 0, 10, 0, 10, 0, 0, 0, 0, 0, 100, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(745, 3485, 3486, hidden: false, ECharacterFeatureType.Special, 3487, 3488, 2984, new FeatureMedals[3]
		{
			new FeatureMedals("inc", "inc"),
			new FeatureMedals(),
			new FeatureMedals("inc")
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: false, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 745, 8, 0, 60, -1, -1, 0, 0, null, 0, 0, 0, 5, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 0, 0, 0, 20, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 10, 0, 10, 0, 0, 0, 10, 0, 10, 0, 0, 0, 0, 0, 0, 100, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(746, 3489, 3490, hidden: false, ECharacterFeatureType.Special, 3491, 3492, 2984, new FeatureMedals[3]
		{
			new FeatureMedals("inc"),
			new FeatureMedals("inc", "inc"),
			new FeatureMedals()
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: false, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 746, 8, 0, 60, -1, -1, 0, 0, null, 0, 0, 0, 5, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 0, 20, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 10, 10, 0, 0, 0, 0, 10, 10, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(747, 3493, 3494, hidden: false, ECharacterFeatureType.Special, 3495, 3496, 2984, new FeatureMedals[3]
		{
			new FeatureMedals(),
			new FeatureMedals(),
			new FeatureMedals("inc", "inc", "inc")
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: false, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 747, 8, 0, 60, -1, -1, 0, 0, null, 0, 0, 0, 5, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 20, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 20, 0, 0, 0, 0, 0, 20, 0, 0, 0, 0, 50, 50, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(748, 3497, 3498, hidden: false, ECharacterFeatureType.Special, 3499, 3500, 2984, new FeatureMedals[3]
		{
			new FeatureMedals("inc", "inc", "inc"),
			new FeatureMedals(),
			new FeatureMedals()
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: false, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 748, 8, 0, 60, -1, -1, 0, 0, null, 0, 0, 0, 5, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 20, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 10, 0, 0, 0, 0, 0, 10, 0, 0, 0, 0, 0, 0, 0, 0, 100, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(749, 3501, 3502, hidden: false, ECharacterFeatureType.Special, 3503, 3504, 2984, new FeatureMedals[3]
		{
			new FeatureMedals(),
			new FeatureMedals("inc"),
			new FeatureMedals("inc", "inc")
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: false, canDeleteManually: false, canCrossArchive: true, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 749, 8, 0, 60, -1, -1, 0, 0, null, 0, 0, 0, 5, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 0, 0, 20, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 20, 0, 0, 0, 0, 0, 20, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(750, 3505, 3506, hidden: false, ECharacterFeatureType.Special, 3507, 3508, 3509, new FeatureMedals[3]
		{
			new FeatureMedals("neg"),
			new FeatureMedals("neg"),
			new FeatureMedals("neg")
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: true, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: false, canDeleteManually: false, canCrossArchive: false, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 220, 6, 0, 0, -1, -1, 0, 0, null, 0, 0, -40, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 0, 0, -50, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(751, 3510, 3511, hidden: false, ECharacterFeatureType.Special, 3512, 3513, 3514, new FeatureMedals[3]
		{
			new FeatureMedals("neg"),
			new FeatureMedals("neg"),
			new FeatureMedals("neg")
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: true, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: false, canDeleteManually: false, canCrossArchive: false, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 220, 6, 0, 0, -1, -1, 0, 0, null, 0, 0, -40, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 0, 0, 0, -50, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(752, 3515, 3516, hidden: false, ECharacterFeatureType.Special, 3517, 3518, 3519, new FeatureMedals[3]
		{
			new FeatureMedals("neg"),
			new FeatureMedals("neg"),
			new FeatureMedals("neg")
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: true, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: false, canDeleteManually: false, canCrossArchive: false, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 220, 6, 0, 0, -1, -1, 0, 0, null, 0, 0, -40, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 0, 0, -25, -25, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(753, 3520, 3521, hidden: false, ECharacterFeatureType.Special, 3522, 3523, 3524, new FeatureMedals[3]
		{
			new FeatureMedals("pos"),
			new FeatureMedals("pos"),
			new FeatureMedals("pos")
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: true, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: false, canDeleteManually: false, canCrossArchive: false, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 220, 6, 0, 0, -1, -1, 0, 0, null, 0, 0, -40, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 0, 0, 50, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(754, 3525, 3526, hidden: false, ECharacterFeatureType.Special, 3527, 3528, 3529, new FeatureMedals[3]
		{
			new FeatureMedals("pos"),
			new FeatureMedals("pos"),
			new FeatureMedals("pos")
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: true, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: false, canDeleteManually: false, canCrossArchive: false, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 220, 6, 0, 0, -1, -1, 0, 0, null, 0, 0, -40, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 0, 0, 0, 50, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(755, 3530, 3531, hidden: false, ECharacterFeatureType.Special, 3532, 3533, 3534, new FeatureMedals[3]
		{
			new FeatureMedals("pos"),
			new FeatureMedals("pos"),
			new FeatureMedals("pos")
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: true, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: false, canDeleteManually: false, canCrossArchive: false, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 220, 6, 0, 0, -1, -1, 0, 0, null, 0, 0, -40, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 0, 0, 25, 25, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(756, 3535, 3536, hidden: false, ECharacterFeatureType.Special, 3537, 3538, 3539, new FeatureMedals[3]
		{
			new FeatureMedals("inc"),
			new FeatureMedals("inc"),
			new FeatureMedals("inc")
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: false, canDeleteManually: false, canCrossArchive: false, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 756, 0, 0, 0, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(757, 3535, 3540, hidden: false, ECharacterFeatureType.Special, 3541, 3542, 3543, new FeatureMedals[3]
		{
			new FeatureMedals("inc"),
			new FeatureMedals("inc"),
			new FeatureMedals("inc")
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: false, canDeleteManually: false, canCrossArchive: false, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.None, -1, 756, 0, 0, 0, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(758, 3544, 3545, hidden: false, ECharacterFeatureType.Temporary, 3546, 3547, 3548, new FeatureMedals[3]
		{
			new FeatureMedals("dec"),
			new FeatureMedals("dec"),
			new FeatureMedals("dec")
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: false, canDeleteManually: false, canCrossArchive: false, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.DarkAsh, -1, 758, 2, 0, 0, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
		_dataArray.Add(new CharacterFeatureItem(759, 3549, 3550, hidden: false, ECharacterFeatureType.Special, 3551, 3552, 3553, new FeatureMedals[3]
		{
			new FeatureMedals("inc"),
			new FeatureMedals("inc"),
			new FeatureMedals("inc")
		}, 0, ECharacterFeatureInfectedType.NotInfected, ignoreHealthMark: false, isTreasuryGuard: false, canBeModified: false, canBeExchanged: false, mergeable: false, basic: false, inscribable: false, soulTransform: false, canDeleteManually: false, canCrossArchive: false, inheritableThroughSamsara: false, inheritableTransferTaiwu: false, ECharacterFeatureDarkAshProtector.ThreeVital, -1, 759, 6, 0, 0, -1, -1, 0, 0, null, 0, 0, 0, 0, 0, 0, 100, 100, 100, makeConsummateLevelRelated: false, new short[5], new sbyte[5], new sbyte[5], 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, isChickenFeature: false, 0, 0, 0, loseConsummateBonus: false, 0, 0, new sbyte[9], 0, 0));
	}

	public override void Init()
	{
		base.Init();
		_dataArray = new List<CharacterFeatureItem>(760);
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
	}

	public static int GetCharacterPropertyBonus(int key, ECharacterPropertyReferencedType property)
	{
		return Instance._dataArray[key].GetCharacterPropertyBonusInt(property);
	}

	public static int GetCharacterPropertyBonus(short[] keys, ECharacterPropertyReferencedType property)
	{
		int num = 0;
		int i = 0;
		for (int num2 = keys.Length; i < num2; i++)
		{
			num += Instance._dataArray[keys[i]].GetCharacterPropertyBonusInt(property);
		}
		return num;
	}

	public static int GetCharacterPropertyBonus(List<short> keys, ECharacterPropertyReferencedType property)
	{
		int num = 0;
		int i = 0;
		for (int count = keys.Count; i < count; i++)
		{
			num += Instance._dataArray[keys[i]].GetCharacterPropertyBonusInt(property);
		}
		return num;
	}

	public static int GetCharacterPropertyBonus(int[] keys, ECharacterPropertyReferencedType property)
	{
		int num = 0;
		int i = 0;
		for (int num2 = keys.Length; i < num2; i++)
		{
			num += Instance._dataArray[keys[i]].GetCharacterPropertyBonusInt(property);
		}
		return num;
	}

	public static int GetCharacterPropertyBonus(List<int> keys, ECharacterPropertyReferencedType property)
	{
		int num = 0;
		int i = 0;
		for (int count = keys.Count; i < count; i++)
		{
			num += Instance._dataArray[keys[i]].GetCharacterPropertyBonusInt(property);
		}
		return num;
	}
}
