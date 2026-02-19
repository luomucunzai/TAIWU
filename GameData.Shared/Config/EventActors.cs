using System;
using System.Collections.Generic;
using Config.Common;

namespace Config;

[Serializable]
public class EventActors : ConfigData<EventActorsItem, short>
{
	public static class DefKey
	{
		public const short NestBoy = 0;

		public const short NestGirl = 1;

		public const short NestChild = 2;

		public const short NestWoman = 3;

		public const short LostWife = 4;

		public const short LostChildMine = 5;

		public const short LostHusband = 6;

		public const short LostHusMom = 7;

		public const short LostChildHis = 8;

		public const short PassByMan = 9;

		public const short PassByWoman = 10;

		public const short HelperShaolin = 11;

		public const short HelperEmei = 12;

		public const short HelperBaihua = 13;

		public const short HelperWudang = 14;

		public const short HelperYuanshan = 15;

		public const short HelperShixiang = 16;

		public const short HelperRanshan = 17;

		public const short HelperXuannv = 18;

		public const short HelperZhujian = 19;

		public const short HelperKongsang = 20;

		public const short HelperJingang = 21;

		public const short HelperWuxian = 22;

		public const short HelperJieqing = 23;

		public const short HelperFulong = 24;

		public const short HelperXuehou = 25;

		public const short PoisonPassBy = 26;

		public const short PoisonPrincess = 27;

		public const short PoisonOld = 28;

		public const short PoisonSick = 29;

		public const short PoisonCutter = 30;

		public const short PoisonBoy = 31;

		public const short MarriageServant = 32;

		public const short MarriageOldMan = 33;

		public const short MarriageOldWoman = 34;

		public const short MarriageBrother = 35;

		public const short MarriageSister = 36;

		public const short MarriageEServant = 37;

		public const short MarriageCuiE = 38;

		public const short MarriageHerdsman = 39;

		public const short MarriageGuide = 40;

		public const short MarriageFamilyHolder = 41;

		public const short MarriageWizard = 42;

		public const short MarriageStoler = 43;

		public const short MarriageGoodPerson = 44;

		public const short MarriageBadPerson = 45;

		public const short MarriageWoman = 46;

		public const short MarriageChild = 47;

		public const short MarriageHunterOld = 48;

		public const short MarriageNobleWoman = 49;

		public const short MarriageMoonOld = 50;

		public const short MarriageTeller = 51;

		public const short MarriageMakerLow = 52;

		public const short MarriageMakerMiddle = 53;

		public const short MarriageMakerHigh = 54;

		public const short MarriageCambel = 55;

		public const short MarriageEvenKeeper = 56;

		public const short MarriageJustKeeper = 57;

		public const short MarriageKindKeeper = 58;

		public const short MarriageRebelKeeper = 59;

		public const short MarriageGoisticKeeper = 60;

		public const short MarriageLocal = 61;

		public const short SectGuideShaolin = 62;

		public const short SectGuideEmei = 63;

		public const short SectGuideBaihua = 64;

		public const short SectGuideWudang = 65;

		public const short SectGuideYuanshan = 66;

		public const short SectGuideShixiang = 67;

		public const short SectGuideRanshan = 68;

		public const short SectGuideXuannv = 69;

		public const short SectGuideZhujian = 70;

		public const short SectGuideKongsang = 71;

		public const short SectGuideJingang = 72;

		public const short SectGuideWuxian = 73;

		public const short SectGuideJieqing = 74;

		public const short SectGuideFulong = 75;

		public const short SectGuideXuehou = 76;

		public const short BuildingGuideZhujian = 77;

		public const short MarriageMaid = 78;

		public const short JianghuActor = 79;

		public const short BookShopActor = 80;

		public const short WeaponShopActor = 81;

		public const short AccessoryShopActor = 82;

		public const short ConstructionShopActor = 83;

		public const short MaterialShopActor = 84;

		public const short FoodShopActor = 85;

		public const short CricketConferenceGuide = 86;

		public const short CricketBusinessman = 87;

		public const short CricketOldMan = 88;

		public const short SectSickShaolin = 89;

		public const short SectSickEmei = 90;

		public const short SectSickBaihua = 91;

		public const short SectSickWudang = 92;

		public const short SectSickYuanshan = 93;

		public const short SectSickShixiang = 94;

		public const short SectSickRanshan = 95;

		public const short SectSickXuannv = 96;

		public const short SectSickZhujian = 97;

		public const short SectSickKongsang = 98;

		public const short SectSickJingang = 99;

		public const short SectSickWuxian = 100;

		public const short SectSickJieqing = 101;

		public const short SectSickFulong = 102;

		public const short SectSickXuehou = 103;

		public const short Beggar = 104;

		public const short SectMainStoryKongsang = 105;

		public const short SectMainStoryXuannv = 106;

		public const short SectMainStoryShaolin0 = 107;

		public const short SectMainStoryShaolin1 = 108;

		public const short SectMainStoryXuehou0 = 109;

		public const short SectMainStoryXuehou1 = 110;

		public const short SectMainStoryWudang1 = 111;

		public const short SectMainStoryWudang2 = 112;

		public const short SectMainStoryWudang3 = 113;

		public const short SectMainStoryWudang4 = 114;

		public const short SectMainStoryWudang5 = 115;

		public const short SectMainStoryWudang6 = 116;

		public const short SectMainStoryWudang7 = 117;

		public const short SectMainStoryWudang8 = 118;

		public const short SectMainStoryWudang9 = 119;

		public const short SectMainStoryWudang10 = 120;

		public const short SectMainStoryXuehou2 = 121;

		public const short SectMainStoryXuehou3 = 122;

		public const short SectMainStoryXuehou4 = 123;

		public const short SectMainStoryRanshan = 124;

		public const short SectMainStoryWudang11 = 125;

		public const short SectMainStoryShixiang1 = 126;

		public const short SectMainStoryShixiang2 = 127;

		public const short SectMainStoryKongsangActingHead = 128;

		public const short SectMainStoryShaolin2 = 129;

		public const short SectMainStoryShixiang3 = 130;

		public const short SectMainStoryShixiang4 = 131;

		public const short SectMainStoryShixiang5 = 132;

		public const short SectMainStoryWudangMountainTree = 133;

		public const short SectMainStoryWudangCaveTree = 134;

		public const short SectMainStoryWudangCanyonTree = 135;

		public const short SectMainStoryWudangSwampTree = 136;

		public const short SectMainStoryWudangHillTree = 137;

		public const short SectMainStoryWudangTaoyuanTree = 138;

		public const short SectMainStoryWudangFieldTree = 139;

		public const short SectMainStoryWudangLakeTree = 140;

		public const short SectMainStoryWudangWoodTree = 141;

		public const short SectMainStoryWudangJungleTree = 142;

		public const short SectMainStoryWudangRiverBeachTree = 143;

		public const short SectMainStoryWudangValleyTree = 144;

		public const short SectMainStoryWudangChengzhu = 145;

		public const short SectMainStoryWudangTraveller = 146;

		public const short SectMainStoryWudangDrunkard = 147;

		public const short SectMainStoryWudangLittleTaoistMonk = 148;

		public const short SectMainStoryWudangDuck = 149;

		public const short SectMainStoryXuannv1 = 150;

		public const short SectMainStoryXuannv2 = 151;

		public const short SectMainStoryEmei0 = 152;

		public const short SectMainStoryEmei1 = 153;

		public const short SectMainStoryEmei2 = 154;

		public const short SectMainStoryEmei3 = 155;

		public const short SectMainStoryEmei4 = 156;

		public const short SectMainStoryEmei5 = 157;

		public const short SectMainStoryEmei6 = 158;

		public const short SectMainStoryEmei7 = 159;

		public const short SectMainStoryXuannvShadowOfMirror = 160;

		public const short SectMainStoryEmeiGibbonShadow = 161;

		public const short SectMainStoryEmeiShadows = 162;

		public const short SectMainStoryXuannvGirl = 163;

		public const short CrossArchiveShadow = 164;

		public const short CrossArchiveArchitect = 165;

		public const short DLCLoongKeeperBrother = 166;

		public const short DLCLoongKeeperSister = 167;

		public const short SectMainStoryWuxianGirlInMist = 168;

		public const short SectMainStoryJingangResident = 169;

		public const short SectMainStoryJingangExorcist = 170;

		public const short SectMainStoryJingangGhost = 171;

		public const short SectMainStoryJingangSoul = 172;

		public const short SectMainStoryWuxianGrandma = 173;

		public const short SectMainStoryWuxianLady = 174;

		public const short SectMainStoryWuxianTeenager = 175;

		public const short SectMainStoryWuxianYouth = 176;

		public const short SectMainStoryWuxianPoisonOld = 177;

		public const short SectMainStoryRanshan0 = 178;

		public const short SectMainStoryRanshan1 = 179;

		public const short SectMainStoryRanshan2 = 180;

		public const short SectMainStoryRanshan3 = 181;

		public const short SectMainStoryRanshan4 = 182;

		public const short SectMainStoryRanshan5 = 183;

		public const short SectMainStoryRanshan6 = 184;

		public const short SectMainStoryRanshan7 = 185;

		public const short SectMainStoryRanshan8 = 186;

		public const short SectMainStoryRanshan9 = 187;

		public const short SectMainStoryRanshan10 = 188;

		public const short SectMainStoryBaihuaGuard0 = 189;

		public const short SectMainStoryBaihuaGuard1 = 190;

		public const short SectMainStoryBaihuaShadowFengqing = 191;

		public const short SectMainStoryBaihuaShadowHuanxin = 192;

		public const short SectMainStoryBaihuaShadowXiangshu = 193;

		public const short SectMainStoryBaihuaShadowAnonymMale = 194;

		public const short SectMainStoryBaihuaShadowAnonymFemale = 195;

		public const short SectMainStoryBaihuaFengqing = 196;

		public const short SectMainStoryBaihuaAnonymMaleRelieved = 197;

		public const short SectMainStoryBaihuaAnonymFemaleRelieved = 198;

		public const short SectMainStoryAnonymMale = 199;

		public const short SectMainStoryAnonymFemale = 200;

		public const short SectMainStoryRanshan11 = 201;

		public const short SectMainStoryRanshan12 = 202;

		public const short SectMainStoryRanshan13 = 203;

		public const short SectMainStoryRanshan14 = 204;

		public const short SectMainStoryRanshan15 = 205;

		public const short SectMainStoryRanshan16 = 206;

		public const short SectMainStoryRanshan17 = 207;

		public const short SectMainStoryRanshan18 = 208;

		public const short SectMainStoryRanshan19 = 209;

		public const short SectMainStoryRanshan20 = 210;

		public const short SectMainStoryRanshan21 = 211;

		public const short SectMainStoryRanshan22 = 212;

		public const short SectMainStoryRanshan23 = 213;

		public const short SectMainStoryRanshan24 = 214;

		public const short SectMainStoryRanshan25 = 215;

		public const short SectMainStoryFulongUsher = 216;

		public const short SectMainStoryFulongMaskedWoman = 217;

		public const short SectMainStoryFulongLazuliForm = 218;

		public const short ChickenClever0 = 219;

		public const short ChickenClever1 = 220;

		public const short ChickenClever2 = 221;

		public const short ChickenClever3 = 222;

		public const short ChickenClever4 = 223;

		public const short ChickenClever5 = 224;

		public const short ChickenClever6 = 225;

		public const short ChickenClever7 = 226;

		public const short ChickenClever8 = 227;

		public const short ChickenLucky0 = 228;

		public const short ChickenLucky1 = 229;

		public const short ChickenLucky2 = 230;

		public const short ChickenLucky3 = 231;

		public const short ChickenLucky4 = 232;

		public const short ChickenLucky5 = 233;

		public const short ChickenLucky6 = 234;

		public const short ChickenLucky7 = 235;

		public const short ChickenLucky8 = 236;

		public const short ChickenPerceptive0 = 237;

		public const short ChickenPerceptive1 = 238;

		public const short ChickenPerceptive2 = 239;

		public const short ChickenPerceptive3 = 240;

		public const short ChickenPerceptive4 = 241;

		public const short ChickenPerceptive5 = 242;

		public const short ChickenPerceptive6 = 243;

		public const short ChickenPerceptive7 = 244;

		public const short ChickenPerceptive8 = 245;

		public const short ChickenFirm0 = 246;

		public const short ChickenFirm1 = 247;

		public const short ChickenFirm2 = 248;

		public const short ChickenFirm3 = 249;

		public const short ChickenFirm4 = 250;

		public const short ChickenFirm5 = 251;

		public const short ChickenFirm6 = 252;

		public const short ChickenFirm7 = 253;

		public const short ChickenFirm8 = 254;

		public const short ChickenCalm0 = 255;

		public const short ChickenCalm1 = 256;

		public const short ChickenCalm2 = 257;

		public const short ChickenCalm3 = 258;

		public const short ChickenCalm4 = 259;

		public const short ChickenCalm5 = 260;

		public const short ChickenCalm6 = 261;

		public const short ChickenCalm7 = 262;

		public const short ChickenCalm8 = 263;

		public const short ChickenEnthusiastic0 = 264;

		public const short ChickenEnthusiastic1 = 265;

		public const short ChickenEnthusiastic2 = 266;

		public const short ChickenEnthusiastic3 = 267;

		public const short ChickenEnthusiastic4 = 268;

		public const short ChickenEnthusiastic5 = 269;

		public const short ChickenEnthusiastic6 = 270;

		public const short ChickenEnthusiastic7 = 271;

		public const short ChickenEnthusiastic8 = 272;

		public const short ChickenBrave0 = 273;

		public const short ChickenBrave1 = 274;

		public const short ChickenBrave2 = 275;

		public const short ChickenBrave3 = 276;

		public const short ChickenBrave4 = 277;

		public const short ChickenBrave5 = 278;

		public const short ChickenBrave6 = 279;

		public const short ChickenBrave7 = 280;

		public const short ChickenBrave8 = 281;

		public const short TaiWuVillager = 282;

		public const short HeadOfTongsheng = 283;

		public const short Ouyezi = 284;

		public const short ShadowOfHeavenlyLord = 285;

		public const short SectMainStoryZhujian1 = 286;

		public const short SectMainStoryZhujian2 = 287;

		public const short SectMainStoryZhujian3 = 288;

		public const short SectMainStoryZhujian4 = 289;

		public const short SectMainStoryZhujian5 = 290;

		public const short SectMainStoryZhujian6 = 291;

		public const short GeneralShaolinMember = 292;

		public const short GeneralEmeiMember = 293;

		public const short GeneraBaihuaMember = 294;

		public const short GeneralWudangMember = 295;

		public const short GeneralYuanshanMember = 296;

		public const short GeneralShixiangMember = 297;

		public const short GeneralRanshanMember = 298;

		public const short GeneralXuannvMember = 299;

		public const short GeneralZhujianMember = 300;

		public const short GeneralKongsangMember = 301;

		public const short GeneralJingangMember = 302;

		public const short GeneralWuxianMember = 303;

		public const short GeneralJieqingMember = 304;

		public const short GeneralFulongMember = 305;

		public const short GeneralXuehouMember = 306;

		public const short RemakeEmeiJefferyi = 307;

		public const short SectMainStoryYuanshan1 = 308;

		public const short SectMainStoryYuanshan2 = 309;

		public const short SectMainStoryYuanshan3 = 310;

		public const short SectMainStoryYuanshan4 = 311;

		public const short SectMainStoryYuanshan5 = 312;

		public const short SectMainStoryYuanshan6 = 313;

		public const short SectMainStoryYuanshan7 = 314;

		public const short SectMainStoryYuanshan8 = 315;

		public const short SectMainStoryYuanshan9 = 316;

		public const short SectMainStoryYuanshan10 = 317;

		public const short SectMainStoryYuanshan11 = 318;

		public const short SectMainStoryYuanshan12 = 319;

		public const short SectMainStoryYuanshan13 = 320;

		public const short SectMainStoryYuanshan14 = 321;

		public const short SectMainStoryYuanshanSeven0 = 322;

		public const short SectMainStoryYuanshanSeven1 = 323;
	}

	public static class DefValue
	{
		public static EventActorsItem NestBoy => Instance[(short)0];

		public static EventActorsItem NestGirl => Instance[(short)1];

		public static EventActorsItem NestChild => Instance[(short)2];

		public static EventActorsItem NestWoman => Instance[(short)3];

		public static EventActorsItem LostWife => Instance[(short)4];

		public static EventActorsItem LostChildMine => Instance[(short)5];

		public static EventActorsItem LostHusband => Instance[(short)6];

		public static EventActorsItem LostHusMom => Instance[(short)7];

		public static EventActorsItem LostChildHis => Instance[(short)8];

		public static EventActorsItem PassByMan => Instance[(short)9];

		public static EventActorsItem PassByWoman => Instance[(short)10];

		public static EventActorsItem HelperShaolin => Instance[(short)11];

		public static EventActorsItem HelperEmei => Instance[(short)12];

		public static EventActorsItem HelperBaihua => Instance[(short)13];

		public static EventActorsItem HelperWudang => Instance[(short)14];

		public static EventActorsItem HelperYuanshan => Instance[(short)15];

		public static EventActorsItem HelperShixiang => Instance[(short)16];

		public static EventActorsItem HelperRanshan => Instance[(short)17];

		public static EventActorsItem HelperXuannv => Instance[(short)18];

		public static EventActorsItem HelperZhujian => Instance[(short)19];

		public static EventActorsItem HelperKongsang => Instance[(short)20];

		public static EventActorsItem HelperJingang => Instance[(short)21];

		public static EventActorsItem HelperWuxian => Instance[(short)22];

		public static EventActorsItem HelperJieqing => Instance[(short)23];

		public static EventActorsItem HelperFulong => Instance[(short)24];

		public static EventActorsItem HelperXuehou => Instance[(short)25];

		public static EventActorsItem PoisonPassBy => Instance[(short)26];

		public static EventActorsItem PoisonPrincess => Instance[(short)27];

		public static EventActorsItem PoisonOld => Instance[(short)28];

		public static EventActorsItem PoisonSick => Instance[(short)29];

		public static EventActorsItem PoisonCutter => Instance[(short)30];

		public static EventActorsItem PoisonBoy => Instance[(short)31];

		public static EventActorsItem MarriageServant => Instance[(short)32];

		public static EventActorsItem MarriageOldMan => Instance[(short)33];

		public static EventActorsItem MarriageOldWoman => Instance[(short)34];

		public static EventActorsItem MarriageBrother => Instance[(short)35];

		public static EventActorsItem MarriageSister => Instance[(short)36];

		public static EventActorsItem MarriageEServant => Instance[(short)37];

		public static EventActorsItem MarriageCuiE => Instance[(short)38];

		public static EventActorsItem MarriageHerdsman => Instance[(short)39];

		public static EventActorsItem MarriageGuide => Instance[(short)40];

		public static EventActorsItem MarriageFamilyHolder => Instance[(short)41];

		public static EventActorsItem MarriageWizard => Instance[(short)42];

		public static EventActorsItem MarriageStoler => Instance[(short)43];

		public static EventActorsItem MarriageGoodPerson => Instance[(short)44];

		public static EventActorsItem MarriageBadPerson => Instance[(short)45];

		public static EventActorsItem MarriageWoman => Instance[(short)46];

		public static EventActorsItem MarriageChild => Instance[(short)47];

		public static EventActorsItem MarriageHunterOld => Instance[(short)48];

		public static EventActorsItem MarriageNobleWoman => Instance[(short)49];

		public static EventActorsItem MarriageMoonOld => Instance[(short)50];

		public static EventActorsItem MarriageTeller => Instance[(short)51];

		public static EventActorsItem MarriageMakerLow => Instance[(short)52];

		public static EventActorsItem MarriageMakerMiddle => Instance[(short)53];

		public static EventActorsItem MarriageMakerHigh => Instance[(short)54];

		public static EventActorsItem MarriageCambel => Instance[(short)55];

		public static EventActorsItem MarriageEvenKeeper => Instance[(short)56];

		public static EventActorsItem MarriageJustKeeper => Instance[(short)57];

		public static EventActorsItem MarriageKindKeeper => Instance[(short)58];

		public static EventActorsItem MarriageRebelKeeper => Instance[(short)59];

		public static EventActorsItem MarriageGoisticKeeper => Instance[(short)60];

		public static EventActorsItem MarriageLocal => Instance[(short)61];

		public static EventActorsItem SectGuideShaolin => Instance[(short)62];

		public static EventActorsItem SectGuideEmei => Instance[(short)63];

		public static EventActorsItem SectGuideBaihua => Instance[(short)64];

		public static EventActorsItem SectGuideWudang => Instance[(short)65];

		public static EventActorsItem SectGuideYuanshan => Instance[(short)66];

		public static EventActorsItem SectGuideShixiang => Instance[(short)67];

		public static EventActorsItem SectGuideRanshan => Instance[(short)68];

		public static EventActorsItem SectGuideXuannv => Instance[(short)69];

		public static EventActorsItem SectGuideZhujian => Instance[(short)70];

		public static EventActorsItem SectGuideKongsang => Instance[(short)71];

		public static EventActorsItem SectGuideJingang => Instance[(short)72];

		public static EventActorsItem SectGuideWuxian => Instance[(short)73];

		public static EventActorsItem SectGuideJieqing => Instance[(short)74];

		public static EventActorsItem SectGuideFulong => Instance[(short)75];

		public static EventActorsItem SectGuideXuehou => Instance[(short)76];

		public static EventActorsItem BuildingGuideZhujian => Instance[(short)77];

		public static EventActorsItem MarriageMaid => Instance[(short)78];

		public static EventActorsItem JianghuActor => Instance[(short)79];

		public static EventActorsItem BookShopActor => Instance[(short)80];

		public static EventActorsItem WeaponShopActor => Instance[(short)81];

		public static EventActorsItem AccessoryShopActor => Instance[(short)82];

		public static EventActorsItem ConstructionShopActor => Instance[(short)83];

		public static EventActorsItem MaterialShopActor => Instance[(short)84];

		public static EventActorsItem FoodShopActor => Instance[(short)85];

		public static EventActorsItem CricketConferenceGuide => Instance[(short)86];

		public static EventActorsItem CricketBusinessman => Instance[(short)87];

		public static EventActorsItem CricketOldMan => Instance[(short)88];

		public static EventActorsItem SectSickShaolin => Instance[(short)89];

		public static EventActorsItem SectSickEmei => Instance[(short)90];

		public static EventActorsItem SectSickBaihua => Instance[(short)91];

		public static EventActorsItem SectSickWudang => Instance[(short)92];

		public static EventActorsItem SectSickYuanshan => Instance[(short)93];

		public static EventActorsItem SectSickShixiang => Instance[(short)94];

		public static EventActorsItem SectSickRanshan => Instance[(short)95];

		public static EventActorsItem SectSickXuannv => Instance[(short)96];

		public static EventActorsItem SectSickZhujian => Instance[(short)97];

		public static EventActorsItem SectSickKongsang => Instance[(short)98];

		public static EventActorsItem SectSickJingang => Instance[(short)99];

		public static EventActorsItem SectSickWuxian => Instance[(short)100];

		public static EventActorsItem SectSickJieqing => Instance[(short)101];

		public static EventActorsItem SectSickFulong => Instance[(short)102];

		public static EventActorsItem SectSickXuehou => Instance[(short)103];

		public static EventActorsItem Beggar => Instance[(short)104];

		public static EventActorsItem SectMainStoryKongsang => Instance[(short)105];

		public static EventActorsItem SectMainStoryXuannv => Instance[(short)106];

		public static EventActorsItem SectMainStoryShaolin0 => Instance[(short)107];

		public static EventActorsItem SectMainStoryShaolin1 => Instance[(short)108];

		public static EventActorsItem SectMainStoryXuehou0 => Instance[(short)109];

		public static EventActorsItem SectMainStoryXuehou1 => Instance[(short)110];

		public static EventActorsItem SectMainStoryWudang1 => Instance[(short)111];

		public static EventActorsItem SectMainStoryWudang2 => Instance[(short)112];

		public static EventActorsItem SectMainStoryWudang3 => Instance[(short)113];

		public static EventActorsItem SectMainStoryWudang4 => Instance[(short)114];

		public static EventActorsItem SectMainStoryWudang5 => Instance[(short)115];

		public static EventActorsItem SectMainStoryWudang6 => Instance[(short)116];

		public static EventActorsItem SectMainStoryWudang7 => Instance[(short)117];

		public static EventActorsItem SectMainStoryWudang8 => Instance[(short)118];

		public static EventActorsItem SectMainStoryWudang9 => Instance[(short)119];

		public static EventActorsItem SectMainStoryWudang10 => Instance[(short)120];

		public static EventActorsItem SectMainStoryXuehou2 => Instance[(short)121];

		public static EventActorsItem SectMainStoryXuehou3 => Instance[(short)122];

		public static EventActorsItem SectMainStoryXuehou4 => Instance[(short)123];

		public static EventActorsItem SectMainStoryRanshan => Instance[(short)124];

		public static EventActorsItem SectMainStoryWudang11 => Instance[(short)125];

		public static EventActorsItem SectMainStoryShixiang1 => Instance[(short)126];

		public static EventActorsItem SectMainStoryShixiang2 => Instance[(short)127];

		public static EventActorsItem SectMainStoryKongsangActingHead => Instance[(short)128];

		public static EventActorsItem SectMainStoryShaolin2 => Instance[(short)129];

		public static EventActorsItem SectMainStoryShixiang3 => Instance[(short)130];

		public static EventActorsItem SectMainStoryShixiang4 => Instance[(short)131];

		public static EventActorsItem SectMainStoryShixiang5 => Instance[(short)132];

		public static EventActorsItem SectMainStoryWudangMountainTree => Instance[(short)133];

		public static EventActorsItem SectMainStoryWudangCaveTree => Instance[(short)134];

		public static EventActorsItem SectMainStoryWudangCanyonTree => Instance[(short)135];

		public static EventActorsItem SectMainStoryWudangSwampTree => Instance[(short)136];

		public static EventActorsItem SectMainStoryWudangHillTree => Instance[(short)137];

		public static EventActorsItem SectMainStoryWudangTaoyuanTree => Instance[(short)138];

		public static EventActorsItem SectMainStoryWudangFieldTree => Instance[(short)139];

		public static EventActorsItem SectMainStoryWudangLakeTree => Instance[(short)140];

		public static EventActorsItem SectMainStoryWudangWoodTree => Instance[(short)141];

		public static EventActorsItem SectMainStoryWudangJungleTree => Instance[(short)142];

		public static EventActorsItem SectMainStoryWudangRiverBeachTree => Instance[(short)143];

		public static EventActorsItem SectMainStoryWudangValleyTree => Instance[(short)144];

		public static EventActorsItem SectMainStoryWudangChengzhu => Instance[(short)145];

		public static EventActorsItem SectMainStoryWudangTraveller => Instance[(short)146];

		public static EventActorsItem SectMainStoryWudangDrunkard => Instance[(short)147];

		public static EventActorsItem SectMainStoryWudangLittleTaoistMonk => Instance[(short)148];

		public static EventActorsItem SectMainStoryWudangDuck => Instance[(short)149];

		public static EventActorsItem SectMainStoryXuannv1 => Instance[(short)150];

		public static EventActorsItem SectMainStoryXuannv2 => Instance[(short)151];

		public static EventActorsItem SectMainStoryEmei0 => Instance[(short)152];

		public static EventActorsItem SectMainStoryEmei1 => Instance[(short)153];

		public static EventActorsItem SectMainStoryEmei2 => Instance[(short)154];

		public static EventActorsItem SectMainStoryEmei3 => Instance[(short)155];

		public static EventActorsItem SectMainStoryEmei4 => Instance[(short)156];

		public static EventActorsItem SectMainStoryEmei5 => Instance[(short)157];

		public static EventActorsItem SectMainStoryEmei6 => Instance[(short)158];

		public static EventActorsItem SectMainStoryEmei7 => Instance[(short)159];

		public static EventActorsItem SectMainStoryXuannvShadowOfMirror => Instance[(short)160];

		public static EventActorsItem SectMainStoryEmeiGibbonShadow => Instance[(short)161];

		public static EventActorsItem SectMainStoryEmeiShadows => Instance[(short)162];

		public static EventActorsItem SectMainStoryXuannvGirl => Instance[(short)163];

		public static EventActorsItem CrossArchiveShadow => Instance[(short)164];

		public static EventActorsItem CrossArchiveArchitect => Instance[(short)165];

		public static EventActorsItem DLCLoongKeeperBrother => Instance[(short)166];

		public static EventActorsItem DLCLoongKeeperSister => Instance[(short)167];

		public static EventActorsItem SectMainStoryWuxianGirlInMist => Instance[(short)168];

		public static EventActorsItem SectMainStoryJingangResident => Instance[(short)169];

		public static EventActorsItem SectMainStoryJingangExorcist => Instance[(short)170];

		public static EventActorsItem SectMainStoryJingangGhost => Instance[(short)171];

		public static EventActorsItem SectMainStoryJingangSoul => Instance[(short)172];

		public static EventActorsItem SectMainStoryWuxianGrandma => Instance[(short)173];

		public static EventActorsItem SectMainStoryWuxianLady => Instance[(short)174];

		public static EventActorsItem SectMainStoryWuxianTeenager => Instance[(short)175];

		public static EventActorsItem SectMainStoryWuxianYouth => Instance[(short)176];

		public static EventActorsItem SectMainStoryWuxianPoisonOld => Instance[(short)177];

		public static EventActorsItem SectMainStoryRanshan0 => Instance[(short)178];

		public static EventActorsItem SectMainStoryRanshan1 => Instance[(short)179];

		public static EventActorsItem SectMainStoryRanshan2 => Instance[(short)180];

		public static EventActorsItem SectMainStoryRanshan3 => Instance[(short)181];

		public static EventActorsItem SectMainStoryRanshan4 => Instance[(short)182];

		public static EventActorsItem SectMainStoryRanshan5 => Instance[(short)183];

		public static EventActorsItem SectMainStoryRanshan6 => Instance[(short)184];

		public static EventActorsItem SectMainStoryRanshan7 => Instance[(short)185];

		public static EventActorsItem SectMainStoryRanshan8 => Instance[(short)186];

		public static EventActorsItem SectMainStoryRanshan9 => Instance[(short)187];

		public static EventActorsItem SectMainStoryRanshan10 => Instance[(short)188];

		public static EventActorsItem SectMainStoryBaihuaGuard0 => Instance[(short)189];

		public static EventActorsItem SectMainStoryBaihuaGuard1 => Instance[(short)190];

		public static EventActorsItem SectMainStoryBaihuaShadowFengqing => Instance[(short)191];

		public static EventActorsItem SectMainStoryBaihuaShadowHuanxin => Instance[(short)192];

		public static EventActorsItem SectMainStoryBaihuaShadowXiangshu => Instance[(short)193];

		public static EventActorsItem SectMainStoryBaihuaShadowAnonymMale => Instance[(short)194];

		public static EventActorsItem SectMainStoryBaihuaShadowAnonymFemale => Instance[(short)195];

		public static EventActorsItem SectMainStoryBaihuaFengqing => Instance[(short)196];

		public static EventActorsItem SectMainStoryBaihuaAnonymMaleRelieved => Instance[(short)197];

		public static EventActorsItem SectMainStoryBaihuaAnonymFemaleRelieved => Instance[(short)198];

		public static EventActorsItem SectMainStoryAnonymMale => Instance[(short)199];

		public static EventActorsItem SectMainStoryAnonymFemale => Instance[(short)200];

		public static EventActorsItem SectMainStoryRanshan11 => Instance[(short)201];

		public static EventActorsItem SectMainStoryRanshan12 => Instance[(short)202];

		public static EventActorsItem SectMainStoryRanshan13 => Instance[(short)203];

		public static EventActorsItem SectMainStoryRanshan14 => Instance[(short)204];

		public static EventActorsItem SectMainStoryRanshan15 => Instance[(short)205];

		public static EventActorsItem SectMainStoryRanshan16 => Instance[(short)206];

		public static EventActorsItem SectMainStoryRanshan17 => Instance[(short)207];

		public static EventActorsItem SectMainStoryRanshan18 => Instance[(short)208];

		public static EventActorsItem SectMainStoryRanshan19 => Instance[(short)209];

		public static EventActorsItem SectMainStoryRanshan20 => Instance[(short)210];

		public static EventActorsItem SectMainStoryRanshan21 => Instance[(short)211];

		public static EventActorsItem SectMainStoryRanshan22 => Instance[(short)212];

		public static EventActorsItem SectMainStoryRanshan23 => Instance[(short)213];

		public static EventActorsItem SectMainStoryRanshan24 => Instance[(short)214];

		public static EventActorsItem SectMainStoryRanshan25 => Instance[(short)215];

		public static EventActorsItem SectMainStoryFulongUsher => Instance[(short)216];

		public static EventActorsItem SectMainStoryFulongMaskedWoman => Instance[(short)217];

		public static EventActorsItem SectMainStoryFulongLazuliForm => Instance[(short)218];

		public static EventActorsItem ChickenClever0 => Instance[(short)219];

		public static EventActorsItem ChickenClever1 => Instance[(short)220];

		public static EventActorsItem ChickenClever2 => Instance[(short)221];

		public static EventActorsItem ChickenClever3 => Instance[(short)222];

		public static EventActorsItem ChickenClever4 => Instance[(short)223];

		public static EventActorsItem ChickenClever5 => Instance[(short)224];

		public static EventActorsItem ChickenClever6 => Instance[(short)225];

		public static EventActorsItem ChickenClever7 => Instance[(short)226];

		public static EventActorsItem ChickenClever8 => Instance[(short)227];

		public static EventActorsItem ChickenLucky0 => Instance[(short)228];

		public static EventActorsItem ChickenLucky1 => Instance[(short)229];

		public static EventActorsItem ChickenLucky2 => Instance[(short)230];

		public static EventActorsItem ChickenLucky3 => Instance[(short)231];

		public static EventActorsItem ChickenLucky4 => Instance[(short)232];

		public static EventActorsItem ChickenLucky5 => Instance[(short)233];

		public static EventActorsItem ChickenLucky6 => Instance[(short)234];

		public static EventActorsItem ChickenLucky7 => Instance[(short)235];

		public static EventActorsItem ChickenLucky8 => Instance[(short)236];

		public static EventActorsItem ChickenPerceptive0 => Instance[(short)237];

		public static EventActorsItem ChickenPerceptive1 => Instance[(short)238];

		public static EventActorsItem ChickenPerceptive2 => Instance[(short)239];

		public static EventActorsItem ChickenPerceptive3 => Instance[(short)240];

		public static EventActorsItem ChickenPerceptive4 => Instance[(short)241];

		public static EventActorsItem ChickenPerceptive5 => Instance[(short)242];

		public static EventActorsItem ChickenPerceptive6 => Instance[(short)243];

		public static EventActorsItem ChickenPerceptive7 => Instance[(short)244];

		public static EventActorsItem ChickenPerceptive8 => Instance[(short)245];

		public static EventActorsItem ChickenFirm0 => Instance[(short)246];

		public static EventActorsItem ChickenFirm1 => Instance[(short)247];

		public static EventActorsItem ChickenFirm2 => Instance[(short)248];

		public static EventActorsItem ChickenFirm3 => Instance[(short)249];

		public static EventActorsItem ChickenFirm4 => Instance[(short)250];

		public static EventActorsItem ChickenFirm5 => Instance[(short)251];

		public static EventActorsItem ChickenFirm6 => Instance[(short)252];

		public static EventActorsItem ChickenFirm7 => Instance[(short)253];

		public static EventActorsItem ChickenFirm8 => Instance[(short)254];

		public static EventActorsItem ChickenCalm0 => Instance[(short)255];

		public static EventActorsItem ChickenCalm1 => Instance[(short)256];

		public static EventActorsItem ChickenCalm2 => Instance[(short)257];

		public static EventActorsItem ChickenCalm3 => Instance[(short)258];

		public static EventActorsItem ChickenCalm4 => Instance[(short)259];

		public static EventActorsItem ChickenCalm5 => Instance[(short)260];

		public static EventActorsItem ChickenCalm6 => Instance[(short)261];

		public static EventActorsItem ChickenCalm7 => Instance[(short)262];

		public static EventActorsItem ChickenCalm8 => Instance[(short)263];

		public static EventActorsItem ChickenEnthusiastic0 => Instance[(short)264];

		public static EventActorsItem ChickenEnthusiastic1 => Instance[(short)265];

		public static EventActorsItem ChickenEnthusiastic2 => Instance[(short)266];

		public static EventActorsItem ChickenEnthusiastic3 => Instance[(short)267];

		public static EventActorsItem ChickenEnthusiastic4 => Instance[(short)268];

		public static EventActorsItem ChickenEnthusiastic5 => Instance[(short)269];

		public static EventActorsItem ChickenEnthusiastic6 => Instance[(short)270];

		public static EventActorsItem ChickenEnthusiastic7 => Instance[(short)271];

		public static EventActorsItem ChickenEnthusiastic8 => Instance[(short)272];

		public static EventActorsItem ChickenBrave0 => Instance[(short)273];

		public static EventActorsItem ChickenBrave1 => Instance[(short)274];

		public static EventActorsItem ChickenBrave2 => Instance[(short)275];

		public static EventActorsItem ChickenBrave3 => Instance[(short)276];

		public static EventActorsItem ChickenBrave4 => Instance[(short)277];

		public static EventActorsItem ChickenBrave5 => Instance[(short)278];

		public static EventActorsItem ChickenBrave6 => Instance[(short)279];

		public static EventActorsItem ChickenBrave7 => Instance[(short)280];

		public static EventActorsItem ChickenBrave8 => Instance[(short)281];

		public static EventActorsItem TaiWuVillager => Instance[(short)282];

		public static EventActorsItem HeadOfTongsheng => Instance[(short)283];

		public static EventActorsItem Ouyezi => Instance[(short)284];

		public static EventActorsItem ShadowOfHeavenlyLord => Instance[(short)285];

		public static EventActorsItem SectMainStoryZhujian1 => Instance[(short)286];

		public static EventActorsItem SectMainStoryZhujian2 => Instance[(short)287];

		public static EventActorsItem SectMainStoryZhujian3 => Instance[(short)288];

		public static EventActorsItem SectMainStoryZhujian4 => Instance[(short)289];

		public static EventActorsItem SectMainStoryZhujian5 => Instance[(short)290];

		public static EventActorsItem SectMainStoryZhujian6 => Instance[(short)291];

		public static EventActorsItem GeneralShaolinMember => Instance[(short)292];

		public static EventActorsItem GeneralEmeiMember => Instance[(short)293];

		public static EventActorsItem GeneraBaihuaMember => Instance[(short)294];

		public static EventActorsItem GeneralWudangMember => Instance[(short)295];

		public static EventActorsItem GeneralYuanshanMember => Instance[(short)296];

		public static EventActorsItem GeneralShixiangMember => Instance[(short)297];

		public static EventActorsItem GeneralRanshanMember => Instance[(short)298];

		public static EventActorsItem GeneralXuannvMember => Instance[(short)299];

		public static EventActorsItem GeneralZhujianMember => Instance[(short)300];

		public static EventActorsItem GeneralKongsangMember => Instance[(short)301];

		public static EventActorsItem GeneralJingangMember => Instance[(short)302];

		public static EventActorsItem GeneralWuxianMember => Instance[(short)303];

		public static EventActorsItem GeneralJieqingMember => Instance[(short)304];

		public static EventActorsItem GeneralFulongMember => Instance[(short)305];

		public static EventActorsItem GeneralXuehouMember => Instance[(short)306];

		public static EventActorsItem RemakeEmeiJefferyi => Instance[(short)307];

		public static EventActorsItem SectMainStoryYuanshan1 => Instance[(short)308];

		public static EventActorsItem SectMainStoryYuanshan2 => Instance[(short)309];

		public static EventActorsItem SectMainStoryYuanshan3 => Instance[(short)310];

		public static EventActorsItem SectMainStoryYuanshan4 => Instance[(short)311];

		public static EventActorsItem SectMainStoryYuanshan5 => Instance[(short)312];

		public static EventActorsItem SectMainStoryYuanshan6 => Instance[(short)313];

		public static EventActorsItem SectMainStoryYuanshan7 => Instance[(short)314];

		public static EventActorsItem SectMainStoryYuanshan8 => Instance[(short)315];

		public static EventActorsItem SectMainStoryYuanshan9 => Instance[(short)316];

		public static EventActorsItem SectMainStoryYuanshan10 => Instance[(short)317];

		public static EventActorsItem SectMainStoryYuanshan11 => Instance[(short)318];

		public static EventActorsItem SectMainStoryYuanshan12 => Instance[(short)319];

		public static EventActorsItem SectMainStoryYuanshan13 => Instance[(short)320];

		public static EventActorsItem SectMainStoryYuanshan14 => Instance[(short)321];

		public static EventActorsItem SectMainStoryYuanshanSeven0 => Instance[(short)322];

		public static EventActorsItem SectMainStoryYuanshanSeven1 => Instance[(short)323];
	}

	public static EventActors Instance = new EventActors();

	private readonly HashSet<string> RequiredFields = new HashSet<string> { "Clothing", "TemplateId", "Name", "Texture" };

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
		_dataArray.Add(new EventActorsItem(0, 0, null, 1, new byte[2] { 8, 12 }, new short[2] { 0, 900 }, -1, isMonk: false, -1));
		_dataArray.Add(new EventActorsItem(1, 1, null, 0, new byte[2] { 8, 12 }, new short[2] { 0, 900 }, -1, isMonk: false, -1));
		_dataArray.Add(new EventActorsItem(2, 2, null, -1, new byte[2] { 5, 8 }, new short[2] { 0, 900 }, -1, isMonk: false, -1));
		_dataArray.Add(new EventActorsItem(3, 3, null, 0, new byte[2] { 18, 25 }, new short[2] { 500, 900 }, -1, isMonk: false, -1));
		_dataArray.Add(new EventActorsItem(4, 4, null, 0, new byte[2] { 18, 18 }, new short[2] { 800, 900 }, 4, isMonk: false, -1));
		_dataArray.Add(new EventActorsItem(5, 5, null, -1, new byte[2] { 5, 5 }, new short[2] { 800, 900 }, 4, isMonk: false, -1));
		_dataArray.Add(new EventActorsItem(6, 6, null, 1, new byte[2] { 20, 20 }, new short[2] { 800, 900 }, 4, isMonk: false, -1));
		_dataArray.Add(new EventActorsItem(7, 7, null, 0, new byte[2] { 50, 70 }, new short[2] { 0, 900 }, 15, isMonk: false, -1));
		_dataArray.Add(new EventActorsItem(8, 8, null, -1, new byte[2] { 5, 5 }, new short[2] { 800, 900 }, 4, isMonk: false, -1));
		_dataArray.Add(new EventActorsItem(9, 9, null, 1, new byte[2] { 16, 30 }, new short[2] { 0, 900 }, -1, isMonk: false, -1));
		_dataArray.Add(new EventActorsItem(10, 9, null, 0, new byte[2] { 16, 30 }, new short[2] { 0, 900 }, -1, isMonk: false, -1));
		_dataArray.Add(new EventActorsItem(11, 10, null, 1, new byte[2] { 20, 40 }, new short[2] { 0, 900 }, 19, isMonk: true, -1));
		_dataArray.Add(new EventActorsItem(12, 11, null, -1, new byte[2] { 20, 40 }, new short[2] { 0, 900 }, 22, isMonk: false, -1));
		_dataArray.Add(new EventActorsItem(13, 12, null, -1, new byte[2] { 20, 40 }, new short[2] { 0, 900 }, 25, isMonk: false, -1));
		_dataArray.Add(new EventActorsItem(14, 13, null, -1, new byte[2] { 20, 40 }, new short[2] { 0, 900 }, 28, isMonk: false, -1));
		_dataArray.Add(new EventActorsItem(15, 14, null, -1, new byte[2] { 20, 40 }, new short[2] { 0, 900 }, 32, isMonk: false, -1));
		_dataArray.Add(new EventActorsItem(16, 15, null, -1, new byte[2] { 20, 40 }, new short[2] { 0, 900 }, 35, isMonk: false, -1));
		_dataArray.Add(new EventActorsItem(17, 16, null, -1, new byte[2] { 20, 40 }, new short[2] { 0, 900 }, 38, isMonk: false, -1));
		_dataArray.Add(new EventActorsItem(18, 17, null, 0, new byte[2] { 20, 40 }, new short[2] { 0, 900 }, 41, isMonk: false, -1));
		_dataArray.Add(new EventActorsItem(19, 18, null, -1, new byte[2] { 20, 40 }, new short[2] { 0, 900 }, 44, isMonk: false, -1));
		_dataArray.Add(new EventActorsItem(20, 19, null, -1, new byte[2] { 20, 40 }, new short[2] { 0, 900 }, 47, isMonk: false, -1));
		_dataArray.Add(new EventActorsItem(21, 20, null, -1, new byte[2] { 20, 40 }, new short[2] { 0, 900 }, 50, isMonk: true, -1));
		_dataArray.Add(new EventActorsItem(22, 21, null, -1, new byte[2] { 20, 40 }, new short[2] { 0, 900 }, 53, isMonk: false, -1));
		_dataArray.Add(new EventActorsItem(23, 22, null, -1, new byte[2] { 20, 40 }, new short[2] { 0, 900 }, 56, isMonk: false, -1));
		_dataArray.Add(new EventActorsItem(24, 23, null, -1, new byte[2] { 20, 40 }, new short[2] { 0, 900 }, 59, isMonk: false, -1));
		_dataArray.Add(new EventActorsItem(25, 24, null, -1, new byte[2] { 20, 40 }, new short[2] { 0, 900 }, 62, isMonk: false, -1));
		_dataArray.Add(new EventActorsItem(26, 25, null, -1, new byte[2] { 18, 60 }, new short[2] { 0, 900 }, -1, isMonk: false, -1));
		_dataArray.Add(new EventActorsItem(27, 26, null, 0, new byte[2] { 20, 20 }, new short[2] { 600, 900 }, 17, isMonk: false, -1));
		_dataArray.Add(new EventActorsItem(28, 27, null, 1, new byte[2] { 60, 90 }, new short[2] { 0, 900 }, -1, isMonk: false, -1));
		_dataArray.Add(new EventActorsItem(29, 28, null, -1, new byte[2] { 18, 60 }, new short[2] { 0, 900 }, -1, isMonk: false, -1));
		_dataArray.Add(new EventActorsItem(30, 29, null, 1, new byte[2] { 30, 40 }, new short[2] { 0, 900 }, 1, isMonk: false, -1));
		_dataArray.Add(new EventActorsItem(31, 30, null, 1, new byte[2] { 16, 16 }, new short[2] { 0, 900 }, 11, isMonk: false, -1));
		_dataArray.Add(new EventActorsItem(32, 31, null, 1, new byte[2] { 16, 30 }, new short[2] { 0, 900 }, 2, isMonk: false, -1));
		_dataArray.Add(new EventActorsItem(33, 32, null, 1, new byte[2] { 50, 90 }, new short[2] { 0, 900 }, 16, isMonk: false, -1));
		_dataArray.Add(new EventActorsItem(34, 32, null, 0, new byte[2] { 50, 90 }, new short[2] { 0, 900 }, 16, isMonk: false, -1));
		_dataArray.Add(new EventActorsItem(35, 33, null, 1, new byte[2] { 10, 15 }, new short[2] { 0, 900 }, 16, isMonk: false, -1));
		_dataArray.Add(new EventActorsItem(36, 34, null, 0, new byte[2] { 10, 15 }, new short[2] { 0, 900 }, 16, isMonk: false, -1));
		_dataArray.Add(new EventActorsItem(37, 35, null, -1, new byte[2] { 18, 60 }, new short[2] { 0, 900 }, 2, isMonk: false, -1));
		_dataArray.Add(new EventActorsItem(38, 36, null, 0, new byte[2] { 18, 18 }, new short[2] { 500, 900 }, 4, isMonk: false, -1));
		_dataArray.Add(new EventActorsItem(39, 37, null, 1, new byte[2] { 16, 40 }, new short[2] { 0, 900 }, 1, isMonk: false, -1));
		_dataArray.Add(new EventActorsItem(40, 38, null, -1, new byte[2] { 16, 30 }, new short[2] { 0, 900 }, -1, isMonk: false, -1));
		_dataArray.Add(new EventActorsItem(41, 39, null, 1, new byte[2] { 60, 90 }, new short[2] { 0, 900 }, 16, isMonk: false, -1));
		_dataArray.Add(new EventActorsItem(42, 40, null, -1, new byte[2] { 30, 50 }, new short[2] { 0, 900 }, 5, isMonk: false, -1));
		_dataArray.Add(new EventActorsItem(43, 41, null, -1, new byte[2] { 18, 60 }, new short[2] { 0, 900 }, 10, isMonk: false, -1));
		_dataArray.Add(new EventActorsItem(44, 42, null, -1, new byte[2] { 30, 50 }, new short[2] { 0, 900 }, -1, isMonk: false, -1));
		_dataArray.Add(new EventActorsItem(45, 43, null, -1, new byte[2] { 20, 40 }, new short[2] { 0, 900 }, -1, isMonk: false, -1));
		_dataArray.Add(new EventActorsItem(46, 44, null, 0, new byte[2] { 30, 50 }, new short[2] { 0, 900 }, 15, isMonk: false, -1));
		_dataArray.Add(new EventActorsItem(47, 45, null, -1, new byte[2] { 6, 12 }, new short[2] { 0, 900 }, 15, isMonk: false, -1));
		_dataArray.Add(new EventActorsItem(48, 32, null, -1, new byte[2] { 40, 60 }, new short[2] { 0, 900 }, 7, isMonk: false, -1));
		_dataArray.Add(new EventActorsItem(49, 46, null, 0, new byte[2] { 35, 40 }, new short[2] { 600, 900 }, 16, isMonk: false, -1));
		_dataArray.Add(new EventActorsItem(50, 27, null, 1, new byte[2] { 90, 120 }, new short[2] { 0, 900 }, 9, isMonk: false, -1));
		_dataArray.Add(new EventActorsItem(51, 47, null, 1, new byte[2] { 30, 60 }, new short[2] { 0, 900 }, 14, isMonk: false, -1));
		_dataArray.Add(new EventActorsItem(52, 48, null, 0, new byte[2] { 20, 30 }, new short[2] { 0, 900 }, 10, isMonk: false, -1));
		_dataArray.Add(new EventActorsItem(53, 48, null, 0, new byte[2] { 30, 40 }, new short[2] { 0, 900 }, 15, isMonk: false, -1));
		_dataArray.Add(new EventActorsItem(54, 48, null, 0, new byte[2] { 40, 50 }, new short[2] { 0, 900 }, 16, isMonk: false, -1));
		_dataArray.Add(new EventActorsItem(55, 49, null, -1, new byte[2] { 18, 60 }, new short[2] { 0, 900 }, 15, isMonk: false, -1));
		_dataArray.Add(new EventActorsItem(56, 50, null, -1, new byte[2] { 18, 60 }, new short[2] { 0, 900 }, 2, isMonk: false, -1));
		_dataArray.Add(new EventActorsItem(57, 51, null, -1, new byte[2] { 18, 60 }, new short[2] { 0, 900 }, 2, isMonk: false, -1));
		_dataArray.Add(new EventActorsItem(58, 51, null, -1, new byte[2] { 18, 60 }, new short[2] { 0, 900 }, 2, isMonk: false, -1));
		_dataArray.Add(new EventActorsItem(59, 51, null, -1, new byte[2] { 18, 60 }, new short[2] { 0, 900 }, 2, isMonk: false, -1));
	}

	private void CreateItems1()
	{
		_dataArray.Add(new EventActorsItem(60, 51, null, -1, new byte[2] { 18, 60 }, new short[2] { 0, 900 }, 2, isMonk: false, -1));
		_dataArray.Add(new EventActorsItem(61, 52, null, -1, new byte[2] { 50, 60 }, new short[2] { 0, 900 }, 0, isMonk: false, -1));
		_dataArray.Add(new EventActorsItem(62, 53, null, 1, new byte[2] { 20, 40 }, new short[2] { 0, 900 }, 18, isMonk: true, -1));
		_dataArray.Add(new EventActorsItem(63, 54, null, -1, new byte[2] { 20, 40 }, new short[2] { 0, 900 }, 21, isMonk: false, -1));
		_dataArray.Add(new EventActorsItem(64, 54, null, -1, new byte[2] { 20, 40 }, new short[2] { 0, 900 }, 24, isMonk: false, -1));
		_dataArray.Add(new EventActorsItem(65, 55, null, -1, new byte[2] { 20, 40 }, new short[2] { 0, 900 }, 27, isMonk: false, -1));
		_dataArray.Add(new EventActorsItem(66, 54, null, -1, new byte[2] { 20, 40 }, new short[2] { 0, 900 }, 31, isMonk: false, -1));
		_dataArray.Add(new EventActorsItem(67, 54, null, -1, new byte[2] { 20, 40 }, new short[2] { 0, 900 }, 34, isMonk: false, -1));
		_dataArray.Add(new EventActorsItem(68, 54, null, -1, new byte[2] { 20, 40 }, new short[2] { 0, 900 }, 37, isMonk: false, -1));
		_dataArray.Add(new EventActorsItem(69, 54, null, 0, new byte[2] { 20, 40 }, new short[2] { 0, 900 }, 40, isMonk: false, -1));
		_dataArray.Add(new EventActorsItem(70, 54, null, -1, new byte[2] { 20, 40 }, new short[2] { 0, 900 }, 43, isMonk: false, -1));
		_dataArray.Add(new EventActorsItem(71, 54, null, -1, new byte[2] { 20, 40 }, new short[2] { 0, 900 }, 46, isMonk: false, -1));
		_dataArray.Add(new EventActorsItem(72, 54, null, -1, new byte[2] { 20, 40 }, new short[2] { 0, 900 }, 49, isMonk: true, -1));
		_dataArray.Add(new EventActorsItem(73, 54, null, -1, new byte[2] { 20, 40 }, new short[2] { 0, 900 }, 52, isMonk: false, -1));
		_dataArray.Add(new EventActorsItem(74, 54, null, -1, new byte[2] { 20, 40 }, new short[2] { 0, 900 }, 55, isMonk: false, -1));
		_dataArray.Add(new EventActorsItem(75, 54, null, -1, new byte[2] { 20, 40 }, new short[2] { 0, 900 }, 58, isMonk: false, -1));
		_dataArray.Add(new EventActorsItem(76, 54, null, -1, new byte[2] { 20, 40 }, new short[2] { 0, 900 }, 61, isMonk: false, -1));
		_dataArray.Add(new EventActorsItem(77, 56, null, -1, new byte[2] { 20, 40 }, new short[2] { 0, 900 }, 43, isMonk: false, -1));
		_dataArray.Add(new EventActorsItem(78, 57, null, 0, new byte[2] { 16, 30 }, new short[2] { 0, 900 }, 10, isMonk: false, -1));
		_dataArray.Add(new EventActorsItem(79, 58, null, -1, new byte[2] { 30, 40 }, new short[2] { 0, 900 }, 3, isMonk: false, -1));
		_dataArray.Add(new EventActorsItem(80, 59, null, -1, new byte[2] { 30, 40 }, new short[2] { 0, 900 }, 4, isMonk: false, -1));
		_dataArray.Add(new EventActorsItem(81, 59, null, -1, new byte[2] { 30, 40 }, new short[2] { 0, 900 }, 7, isMonk: false, -1));
		_dataArray.Add(new EventActorsItem(82, 59, null, -1, new byte[2] { 30, 40 }, new short[2] { 0, 900 }, 15, isMonk: false, -1));
		_dataArray.Add(new EventActorsItem(83, 59, null, -1, new byte[2] { 30, 40 }, new short[2] { 0, 900 }, 13, isMonk: false, -1));
		_dataArray.Add(new EventActorsItem(84, 59, null, -1, new byte[2] { 30, 40 }, new short[2] { 0, 900 }, 11, isMonk: false, -1));
		_dataArray.Add(new EventActorsItem(85, 59, null, -1, new byte[2] { 30, 40 }, new short[2] { 0, 900 }, 10, isMonk: false, -1));
		_dataArray.Add(new EventActorsItem(86, 60, null, -1, new byte[2] { 20, 40 }, new short[2] { 0, 900 }, 2, isMonk: false, -1));
		_dataArray.Add(new EventActorsItem(87, 61, null, -1, new byte[2] { 30, 50 }, new short[2] { 0, 900 }, 16, isMonk: false, -1));
		_dataArray.Add(new EventActorsItem(88, 27, null, -1, new byte[2] { 40, 60 }, new short[2] { 0, 900 }, 8, isMonk: false, -1));
		_dataArray.Add(new EventActorsItem(89, 62, null, -1, new byte[2] { 20, 40 }, new short[2] { 0, 900 }, 19, isMonk: false, -1));
		_dataArray.Add(new EventActorsItem(90, 63, null, -1, new byte[2] { 20, 40 }, new short[2] { 0, 900 }, 22, isMonk: false, -1));
		_dataArray.Add(new EventActorsItem(91, 64, null, -1, new byte[2] { 20, 40 }, new short[2] { 0, 900 }, 25, isMonk: false, -1));
		_dataArray.Add(new EventActorsItem(92, 65, null, -1, new byte[2] { 20, 40 }, new short[2] { 0, 900 }, 28, isMonk: false, -1));
		_dataArray.Add(new EventActorsItem(93, 66, null, -1, new byte[2] { 20, 40 }, new short[2] { 0, 900 }, 32, isMonk: false, -1));
		_dataArray.Add(new EventActorsItem(94, 67, null, -1, new byte[2] { 20, 40 }, new short[2] { 0, 900 }, 35, isMonk: false, -1));
		_dataArray.Add(new EventActorsItem(95, 68, null, -1, new byte[2] { 20, 40 }, new short[2] { 0, 900 }, 38, isMonk: false, -1));
		_dataArray.Add(new EventActorsItem(96, 69, null, -1, new byte[2] { 20, 40 }, new short[2] { 0, 900 }, 41, isMonk: false, -1));
		_dataArray.Add(new EventActorsItem(97, 70, null, -1, new byte[2] { 20, 40 }, new short[2] { 0, 900 }, 44, isMonk: false, -1));
		_dataArray.Add(new EventActorsItem(98, 71, null, -1, new byte[2] { 20, 40 }, new short[2] { 0, 900 }, 47, isMonk: false, -1));
		_dataArray.Add(new EventActorsItem(99, 72, null, -1, new byte[2] { 20, 40 }, new short[2] { 0, 900 }, 50, isMonk: false, -1));
		_dataArray.Add(new EventActorsItem(100, 73, null, -1, new byte[2] { 20, 40 }, new short[2] { 0, 900 }, 53, isMonk: false, -1));
		_dataArray.Add(new EventActorsItem(101, 74, null, -1, new byte[2] { 20, 40 }, new short[2] { 0, 900 }, 56, isMonk: false, -1));
		_dataArray.Add(new EventActorsItem(102, 75, null, -1, new byte[2] { 20, 40 }, new short[2] { 0, 900 }, 59, isMonk: false, -1));
		_dataArray.Add(new EventActorsItem(103, 76, null, -1, new byte[2] { 20, 40 }, new short[2] { 0, 900 }, 62, isMonk: false, -1));
		_dataArray.Add(new EventActorsItem(104, 77, null, -1, new byte[2] { 20, 40 }, new short[2] { 0, 900 }, 9, isMonk: false, -1));
		_dataArray.Add(new EventActorsItem(105, 78, null, -1, new byte[2] { 20, 40 }, new short[2] { 0, 900 }, 48, isMonk: false, -1));
		_dataArray.Add(new EventActorsItem(106, 79, null, 0, new byte[2] { 20, 40 }, new short[2] { 0, 900 }, 41, isMonk: false, -1));
		_dataArray.Add(new EventActorsItem(107, 80, null, 1, new byte[2] { 55, 60 }, new short[2] { 0, 900 }, 20, isMonk: true, 2));
		_dataArray.Add(new EventActorsItem(108, 81, null, 1, new byte[2] { 18, 19 }, new short[2] { 0, 900 }, 18, isMonk: true, 0));
		_dataArray.Add(new EventActorsItem(109, 82, null, 1, new byte[2] { 20, 40 }, new short[2] { 0, 900 }, 16, isMonk: false, -1));
		_dataArray.Add(new EventActorsItem(110, 83, "NpcFace_shanzhuyouzai", -1, new byte[2] { 18, 60 }, new short[2] { 0, 900 }, -1, isMonk: false, -1));
		_dataArray.Add(new EventActorsItem(111, 29, null, 1, new byte[2] { 30, 40 }, new short[2] { 0, 900 }, 1, isMonk: false, -1));
		_dataArray.Add(new EventActorsItem(112, 27, null, 1, new byte[2] { 90, 120 }, new short[2] { 0, 900 }, 1, isMonk: false, -1));
		_dataArray.Add(new EventActorsItem(113, 84, null, 0, new byte[2] { 18, 20 }, new short[2] { 600, 900 }, 16, isMonk: false, -1));
		_dataArray.Add(new EventActorsItem(114, 85, null, 1, new byte[2] { 20, 25 }, new short[2] { 0, 900 }, 52, isMonk: false, -1));
		_dataArray.Add(new EventActorsItem(115, 45, null, -1, new byte[2] { 6, 12 }, new short[2] { 0, 900 }, 15, isMonk: false, -1));
		_dataArray.Add(new EventActorsItem(116, 86, null, 1, new byte[2] { 90, 120 }, new short[2] { 0, 900 }, 0, isMonk: false, -1));
		_dataArray.Add(new EventActorsItem(117, 87, null, 1, new byte[2] { 90, 120 }, new short[2] { 0, 900 }, 14, isMonk: false, -1));
		_dataArray.Add(new EventActorsItem(118, 44, null, 0, new byte[2] { 30, 40 }, new short[2] { 0, 900 }, 15, isMonk: false, -1));
		_dataArray.Add(new EventActorsItem(119, 88, null, 1, new byte[2] { 60, 90 }, new short[2] { 600, 900 }, 14, isMonk: false, -1));
	}

	private void CreateItems2()
	{
		_dataArray.Add(new EventActorsItem(120, 87, null, 1, new byte[2] { 90, 120 }, new short[2] { 0, 900 }, 5, isMonk: false, -1));
		_dataArray.Add(new EventActorsItem(121, 89, null, 1, new byte[2] { 20, 30 }, new short[2] { 0, 900 }, 10, isMonk: false, -1));
		_dataArray.Add(new EventActorsItem(122, 89, null, 0, new byte[2] { 30, 40 }, new short[2] { 0, 900 }, 10, isMonk: false, -1));
		_dataArray.Add(new EventActorsItem(123, 89, null, -1, new byte[2] { 20, 40 }, new short[2] { 0, 900 }, 10, isMonk: false, -1));
		_dataArray.Add(new EventActorsItem(124, 90, null, -1, new byte[2] { 18, 60 }, new short[2] { 0, 900 }, -1, isMonk: false, -1));
		_dataArray.Add(new EventActorsItem(125, 91, null, 1, new byte[2] { 18, 25 }, new short[2] { 0, 900 }, 14, isMonk: false, -1));
		_dataArray.Add(new EventActorsItem(126, 92, null, 1, new byte[2] { 30, 40 }, new short[2] { 0, 900 }, 10, isMonk: false, -1));
		_dataArray.Add(new EventActorsItem(127, 44, null, 0, new byte[2] { 50, 55 }, new short[2] { 0, 900 }, 10, isMonk: false, -1));
		_dataArray.Add(new EventActorsItem(128, 93, null, -1, new byte[2] { 30, 40 }, new short[2] { 0, 900 }, 48, isMonk: false, -1));
		_dataArray.Add(new EventActorsItem(129, 94, null, 1, new byte[2] { 18, 40 }, new short[2] { 0, 900 }, 19, isMonk: true, -1));
		_dataArray.Add(new EventActorsItem(130, 95, "NpcFace_shixiangshiren", -1, new byte[2] { 18, 60 }, new short[2] { 0, 900 }, -1, isMonk: false, -1));
		_dataArray.Add(new EventActorsItem(131, 96, "NpcFace_feishitangdizi", -1, new byte[2] { 18, 60 }, new short[2] { 0, 900 }, -1, isMonk: false, -1));
		_dataArray.Add(new EventActorsItem(132, 97, "NpcFace_yizugaoshou", -1, new byte[2] { 18, 60 }, new short[2] { 0, 900 }, -1, isMonk: false, -1));
		_dataArray.Add(new EventActorsItem(133, 98, null, -1, new byte[2] { 18, 60 }, new short[2] { 0, 900 }, -1, isMonk: false, -1));
		_dataArray.Add(new EventActorsItem(134, 99, null, -1, new byte[2] { 18, 60 }, new short[2] { 0, 900 }, -1, isMonk: false, -1));
		_dataArray.Add(new EventActorsItem(135, 100, null, -1, new byte[2] { 18, 60 }, new short[2] { 0, 900 }, -1, isMonk: false, -1));
		_dataArray.Add(new EventActorsItem(136, 101, null, -1, new byte[2] { 18, 60 }, new short[2] { 0, 900 }, -1, isMonk: false, -1));
		_dataArray.Add(new EventActorsItem(137, 102, null, -1, new byte[2] { 18, 60 }, new short[2] { 0, 900 }, -1, isMonk: false, -1));
		_dataArray.Add(new EventActorsItem(138, 103, null, -1, new byte[2] { 18, 60 }, new short[2] { 0, 900 }, -1, isMonk: false, -1));
		_dataArray.Add(new EventActorsItem(139, 104, null, -1, new byte[2] { 18, 60 }, new short[2] { 0, 900 }, -1, isMonk: false, -1));
		_dataArray.Add(new EventActorsItem(140, 105, null, -1, new byte[2] { 18, 60 }, new short[2] { 0, 900 }, -1, isMonk: false, -1));
		_dataArray.Add(new EventActorsItem(141, 106, null, -1, new byte[2] { 18, 60 }, new short[2] { 0, 900 }, -1, isMonk: false, -1));
		_dataArray.Add(new EventActorsItem(142, 107, null, -1, new byte[2] { 18, 60 }, new short[2] { 0, 900 }, -1, isMonk: false, -1));
		_dataArray.Add(new EventActorsItem(143, 108, null, -1, new byte[2] { 18, 60 }, new short[2] { 0, 900 }, -1, isMonk: false, -1));
		_dataArray.Add(new EventActorsItem(144, 109, null, -1, new byte[2] { 18, 60 }, new short[2] { 0, 900 }, -1, isMonk: false, -1));
		_dataArray.Add(new EventActorsItem(145, 110, null, -1, new byte[2] { 30, 50 }, new short[2] { 0, 900 }, 17, isMonk: false, -1));
		_dataArray.Add(new EventActorsItem(146, 111, null, 1, new byte[2] { 20, 40 }, new short[2] { 0, 900 }, 11, isMonk: false, -1));
		_dataArray.Add(new EventActorsItem(147, 112, null, 1, new byte[2] { 20, 40 }, new short[2] { 0, 900 }, 0, isMonk: false, -1));
		_dataArray.Add(new EventActorsItem(148, 113, null, 1, new byte[2] { 16, 20 }, new short[2] { 500, 599 }, 27, isMonk: false, 0));
		_dataArray.Add(new EventActorsItem(149, 114, "NpcFace_wudangya", -1, new byte[2] { 18, 60 }, new short[2] { 0, 900 }, -1, isMonk: false, -1));
		_dataArray.Add(new EventActorsItem(150, 115, null, -1, new byte[2] { 40, 60 }, new short[2] { 0, 900 }, 4, isMonk: false, -1));
		_dataArray.Add(new EventActorsItem(151, 51, null, -1, new byte[2] { 20, 30 }, new short[2] { 0, 900 }, 10, isMonk: false, -1));
		_dataArray.Add(new EventActorsItem(152, 116, null, 1, new byte[2] { 18, 60 }, new short[2] { 0, 900 }, 22, isMonk: false, -1));
		_dataArray.Add(new EventActorsItem(153, 117, null, 1, new byte[2] { 18, 60 }, new short[2] { 0, 900 }, 22, isMonk: false, -1));
		_dataArray.Add(new EventActorsItem(154, 118, null, -1, new byte[2] { 18, 60 }, new short[2] { 0, 900 }, 22, isMonk: false, 0));
		_dataArray.Add(new EventActorsItem(155, 119, null, -1, new byte[2] { 18, 60 }, new short[2] { 0, 900 }, 22, isMonk: false, 2));
		_dataArray.Add(new EventActorsItem(156, 120, null, -1, new byte[2] { 18, 60 }, new short[2] { 0, 900 }, 22, isMonk: false, -1));
		_dataArray.Add(new EventActorsItem(157, 121, null, -1, new byte[2] { 18, 60 }, new short[2] { 0, 900 }, 22, isMonk: false, -1));
		_dataArray.Add(new EventActorsItem(158, 122, null, 0, new byte[2] { 18, 60 }, new short[2] { 0, 900 }, 22, isMonk: false, -1));
		_dataArray.Add(new EventActorsItem(159, 123, null, 1, new byte[2] { 18, 60 }, new short[2] { 0, 900 }, 22, isMonk: false, -1));
		_dataArray.Add(new EventActorsItem(160, 124, "NpcFace_tiannvxuying", -1, new byte[2] { 18, 60 }, new short[2] { 0, 900 }, -1, isMonk: false, -1));
		_dataArray.Add(new EventActorsItem(161, 125, "NpcFace_emeibaiyuanheiying", -1, new byte[2] { 18, 60 }, new short[2] { 0, 900 }, -1, isMonk: false, -1));
		_dataArray.Add(new EventActorsItem(162, 126, "NpcFace_emeizhanglaojiabanheiyiren", -1, new byte[2] { 18, 60 }, new short[2] { 0, 900 }, -1, isMonk: false, -1));
		_dataArray.Add(new EventActorsItem(163, 127, null, 0, new byte[2] { 5, 6 }, new short[2] { 0, 900 }, 15, isMonk: false, -1));
		_dataArray.Add(new EventActorsItem(164, 128, "NpcFace_menghuijudaheiying", -1, new byte[2] { 18, 60 }, new short[2] { 0, 900 }, -1, isMonk: false, -1));
		_dataArray.Add(new EventActorsItem(165, 129, null, -1, new byte[2] { 20, 40 }, new short[2] { 0, 900 }, 2, isMonk: false, -1));
		_dataArray.Add(new EventActorsItem(166, 130, "NpcFace_huanlongshigege", -1, new byte[2] { 18, 60 }, new short[2] { 0, 900 }, -1, isMonk: false, -1));
		_dataArray.Add(new EventActorsItem(167, 131, "NpcFace_huanlongshimeimei", -1, new byte[2] { 18, 60 }, new short[2] { 0, 900 }, -1, isMonk: false, -1));
		_dataArray.Add(new EventActorsItem(168, 132, "NpcFace_wuzhongshaonv", -1, new byte[2] { 18, 60 }, new short[2] { 0, 900 }, -1, isMonk: false, -1));
		_dataArray.Add(new EventActorsItem(169, 133, null, 1, new byte[2] { 60, 70 }, new short[2] { 0, 900 }, 10, isMonk: false, 0));
		_dataArray.Add(new EventActorsItem(170, 134, null, 1, new byte[2] { 40, 60 }, new short[2] { 0, 900 }, 10, isMonk: false, -1));
		_dataArray.Add(new EventActorsItem(171, 135, "NpcFace_jingangguiyingsengren", -1, new byte[2] { 18, 60 }, new short[2] { 0, 900 }, -1, isMonk: false, -1));
		_dataArray.Add(new EventActorsItem(172, 136, "NpcFace_gaosenghunling", -1, new byte[2] { 18, 60 }, new short[2] { 0, 900 }, -1, isMonk: false, -1));
		_dataArray.Add(new EventActorsItem(173, 137, null, 0, new byte[2] { 60, 70 }, new short[2] { 400, 500 }, 52, isMonk: false, 1));
		_dataArray.Add(new EventActorsItem(174, 138, null, 0, new byte[2] { 30, 40 }, new short[2] { 500, 600 }, 52, isMonk: false, 0));
		_dataArray.Add(new EventActorsItem(175, 84, null, 0, new byte[2] { 20, 30 }, new short[2] { 400, 600 }, 52, isMonk: false, 1));
		_dataArray.Add(new EventActorsItem(176, 139, null, 1, new byte[2] { 20, 30 }, new short[2] { 0, 900 }, 52, isMonk: false, -1));
		_dataArray.Add(new EventActorsItem(177, 27, null, 1, new byte[2] { 60, 90 }, new short[2] { 0, 900 }, 52, isMonk: false, -1));
		_dataArray.Add(new EventActorsItem(178, 88, null, 1, new byte[2] { 18, 25 }, new short[2] { 600, 900 }, 37, isMonk: false, -1));
		_dataArray.Add(new EventActorsItem(179, 45, null, 0, new byte[2] { 6, 12 }, new short[2] { 0, 900 }, 4, isMonk: false, -1));
	}

	private void CreateItems3()
	{
		_dataArray.Add(new EventActorsItem(180, 45, null, 1, new byte[2] { 6, 12 }, new short[2] { 0, 900 }, 4, isMonk: false, -1));
		_dataArray.Add(new EventActorsItem(181, 45, null, -1, new byte[2] { 6, 12 }, new short[2] { 0, 900 }, 4, isMonk: false, -1));
		_dataArray.Add(new EventActorsItem(182, 16, null, -1, new byte[2] { 20, 40 }, new short[2] { 600, 900 }, 38, isMonk: false, -1));
		_dataArray.Add(new EventActorsItem(183, 54, null, -1, new byte[2] { 20, 40 }, new short[2] { 0, 900 }, 37, isMonk: false, -1));
		_dataArray.Add(new EventActorsItem(184, 140, null, -1, new byte[2] { 20, 40 }, new short[2] { 0, 900 }, 37, isMonk: false, -1));
		_dataArray.Add(new EventActorsItem(185, 140, null, -1, new byte[2] { 20, 40 }, new short[2] { 0, 900 }, 38, isMonk: false, -1));
		_dataArray.Add(new EventActorsItem(186, 141, null, -1, new byte[2] { 6, 12 }, new short[2] { 0, 900 }, 15, isMonk: false, -1));
		_dataArray.Add(new EventActorsItem(187, 140, null, 1, new byte[2] { 20, 40 }, new short[2] { 0, 900 }, 37, isMonk: false, -1));
		_dataArray.Add(new EventActorsItem(188, 142, null, 1, new byte[2] { 30, 40 }, new short[2] { 0, 900 }, 1, isMonk: false, -1));
		_dataArray.Add(new EventActorsItem(189, 143, null, 1, new byte[2] { 30, 40 }, new short[2] { 0, 900 }, 1, isMonk: false, -1));
		_dataArray.Add(new EventActorsItem(190, 143, null, 1, new byte[2] { 30, 40 }, new short[2] { 0, 900 }, 11, isMonk: false, -1));
		_dataArray.Add(new EventActorsItem(191, 128, "NpcFace_baihuazhuxianfengqingjianying", -1, new byte[2] { 18, 60 }, new short[2] { 0, 900 }, -1, isMonk: false, -1));
		_dataArray.Add(new EventActorsItem(192, 144, "NpcFace_baihuazhuxianhuanxinjianying", -1, new byte[2] { 18, 60 }, new short[2] { 0, 900 }, -1, isMonk: false, -1));
		_dataArray.Add(new EventActorsItem(193, 145, "NpcFace_xiangshu2", -1, new byte[2] { 18, 60 }, new short[2] { 0, 900 }, -1, isMonk: false, -1));
		_dataArray.Add(new EventActorsItem(194, 146, "NpcFace_wumingzhirennanjianying", -1, new byte[2] { 18, 60 }, new short[2] { 0, 900 }, -1, isMonk: false, -1));
		_dataArray.Add(new EventActorsItem(195, 146, "NpcFace_wumingzhirennvjianying", -1, new byte[2] { 18, 60 }, new short[2] { 0, 900 }, -1, isMonk: false, -1));
		_dataArray.Add(new EventActorsItem(196, 147, "NpcFace_baihuazhuxianfengqing", -1, new byte[2] { 18, 60 }, new short[2] { 0, 900 }, -1, isMonk: false, -1));
		_dataArray.Add(new EventActorsItem(197, 146, "NpcFace_wumingzhirennanjietuo", -1, new byte[2] { 18, 60 }, new short[2] { 0, 900 }, -1, isMonk: false, -1));
		_dataArray.Add(new EventActorsItem(198, 146, "NpcFace_wumingzhirennvjietuo", -1, new byte[2] { 18, 60 }, new short[2] { 0, 900 }, -1, isMonk: false, -1));
		_dataArray.Add(new EventActorsItem(199, 146, "NpcFace_wumingzhirennan", -1, new byte[2] { 18, 60 }, new short[2] { 0, 900 }, -1, isMonk: false, -1));
		_dataArray.Add(new EventActorsItem(200, 146, "NpcFace_wumingzhirennv", -1, new byte[2] { 18, 60 }, new short[2] { 0, 900 }, -1, isMonk: false, -1));
		_dataArray.Add(new EventActorsItem(201, 148, null, 1, new byte[2] { 30, 40 }, new short[2] { 0, 900 }, 10, isMonk: false, -1));
		_dataArray.Add(new EventActorsItem(202, 149, null, 0, new byte[2] { 20, 25 }, new short[2] { 0, 900 }, 10, isMonk: false, -1));
		_dataArray.Add(new EventActorsItem(203, 150, null, 1, new byte[2] { 16, 20 }, new short[2] { 0, 900 }, 10, isMonk: false, -1));
		_dataArray.Add(new EventActorsItem(204, 151, null, 1, new byte[2] { 30, 40 }, new short[2] { 0, 900 }, 15, isMonk: false, -1));
		_dataArray.Add(new EventActorsItem(205, 152, null, 1, new byte[2] { 30, 40 }, new short[2] { 0, 900 }, 11, isMonk: false, -1));
		_dataArray.Add(new EventActorsItem(206, 153, "NpcFace_guiguziwu", -1, new byte[2] { 18, 60 }, new short[2] { 0, 900 }, -1, isMonk: false, -1));
		_dataArray.Add(new EventActorsItem(207, 154, null, 1, new byte[2] { 60, 70 }, new short[2] { 0, 900 }, 4, isMonk: false, 0));
		_dataArray.Add(new EventActorsItem(208, 155, null, 0, new byte[2] { 16, 20 }, new short[2] { 600, 900 }, 11, isMonk: false, 1));
		_dataArray.Add(new EventActorsItem(209, 84, null, 0, new byte[2] { 16, 20 }, new short[2] { 600, 900 }, 16, isMonk: false, -1));
		_dataArray.Add(new EventActorsItem(210, 156, null, 1, new byte[2] { 16, 20 }, new short[2] { 600, 900 }, 10, isMonk: false, -1));
		_dataArray.Add(new EventActorsItem(211, 44, null, 0, new byte[2] { 20, 25 }, new short[2] { 500, 900 }, 13, isMonk: false, -1));
		_dataArray.Add(new EventActorsItem(212, 157, null, 1, new byte[2] { 20, 25 }, new short[2] { 500, 900 }, 13, isMonk: false, -1));
		_dataArray.Add(new EventActorsItem(213, 84, null, 0, new byte[2] { 16, 20 }, new short[2] { 800, 900 }, 16, isMonk: false, -1));
		_dataArray.Add(new EventActorsItem(214, 85, null, 1, new byte[2] { 16, 20 }, new short[2] { 800, 900 }, 16, isMonk: false, -1));
		_dataArray.Add(new EventActorsItem(215, 158, null, 0, new byte[2] { 16, 20 }, new short[2] { 0, 100 }, 16, isMonk: false, -1));
		_dataArray.Add(new EventActorsItem(216, 159, null, -1, new byte[2] { 20, 40 }, new short[2] { 0, 900 }, 59, isMonk: false, -1));
		_dataArray.Add(new EventActorsItem(217, 160, "NpcFace_mengmianshenminvzi", -1, new byte[2] { 18, 60 }, new short[2] { 0, 900 }, -1, isMonk: false, -1));
		_dataArray.Add(new EventActorsItem(218, 161, "NpcFace_liulishouxing", -1, new byte[2] { 18, 60 }, new short[2] { 0, 900 }, -1, isMonk: false, -1));
		_dataArray.Add(new EventActorsItem(219, 162, "NpcFace_chicken_clever0", -1, new byte[2] { 18, 60 }, new short[2] { 0, 900 }, -1, isMonk: false, -1));
		_dataArray.Add(new EventActorsItem(220, 163, "NpcFace_chicken_clever1", -1, new byte[2] { 18, 60 }, new short[2] { 0, 900 }, -1, isMonk: false, -1));
		_dataArray.Add(new EventActorsItem(221, 164, "NpcFace_chicken_clever2", -1, new byte[2] { 18, 60 }, new short[2] { 0, 900 }, -1, isMonk: false, -1));
		_dataArray.Add(new EventActorsItem(222, 165, "NpcFace_chicken_clever3", -1, new byte[2] { 18, 60 }, new short[2] { 0, 900 }, -1, isMonk: false, -1));
		_dataArray.Add(new EventActorsItem(223, 166, "NpcFace_chicken_clever4", -1, new byte[2] { 18, 60 }, new short[2] { 0, 900 }, -1, isMonk: false, -1));
		_dataArray.Add(new EventActorsItem(224, 167, "NpcFace_chicken_clever5", -1, new byte[2] { 18, 60 }, new short[2] { 0, 900 }, -1, isMonk: false, -1));
		_dataArray.Add(new EventActorsItem(225, 168, "NpcFace_chicken_clever6", -1, new byte[2] { 18, 60 }, new short[2] { 0, 900 }, -1, isMonk: false, -1));
		_dataArray.Add(new EventActorsItem(226, 169, "NpcFace_chicken_clever7", -1, new byte[2] { 18, 60 }, new short[2] { 0, 900 }, -1, isMonk: false, -1));
		_dataArray.Add(new EventActorsItem(227, 170, "NpcFace_chicken_clever8", -1, new byte[2] { 18, 60 }, new short[2] { 0, 900 }, -1, isMonk: false, -1));
		_dataArray.Add(new EventActorsItem(228, 171, "NpcFace_chicken_lucky0", -1, new byte[2] { 18, 60 }, new short[2] { 0, 900 }, -1, isMonk: false, -1));
		_dataArray.Add(new EventActorsItem(229, 172, "NpcFace_chicken_lucky1", -1, new byte[2] { 18, 60 }, new short[2] { 0, 900 }, -1, isMonk: false, -1));
		_dataArray.Add(new EventActorsItem(230, 173, "NpcFace_chicken_lucky2", -1, new byte[2] { 18, 60 }, new short[2] { 0, 900 }, -1, isMonk: false, -1));
		_dataArray.Add(new EventActorsItem(231, 174, "NpcFace_chicken_lucky3", -1, new byte[2] { 18, 60 }, new short[2] { 0, 900 }, -1, isMonk: false, -1));
		_dataArray.Add(new EventActorsItem(232, 175, "NpcFace_chicken_lucky4", -1, new byte[2] { 18, 60 }, new short[2] { 0, 900 }, -1, isMonk: false, -1));
		_dataArray.Add(new EventActorsItem(233, 176, "NpcFace_chicken_lucky5", -1, new byte[2] { 18, 60 }, new short[2] { 0, 900 }, -1, isMonk: false, -1));
		_dataArray.Add(new EventActorsItem(234, 177, "NpcFace_chicken_lucky6", -1, new byte[2] { 18, 60 }, new short[2] { 0, 900 }, -1, isMonk: false, -1));
		_dataArray.Add(new EventActorsItem(235, 178, "NpcFace_chicken_lucky7", -1, new byte[2] { 18, 60 }, new short[2] { 0, 900 }, -1, isMonk: false, -1));
		_dataArray.Add(new EventActorsItem(236, 179, "NpcFace_chicken_lucky8", -1, new byte[2] { 18, 60 }, new short[2] { 0, 900 }, -1, isMonk: false, -1));
		_dataArray.Add(new EventActorsItem(237, 180, "NpcFace_chicken_perceptive0", -1, new byte[2] { 18, 60 }, new short[2] { 0, 900 }, -1, isMonk: false, -1));
		_dataArray.Add(new EventActorsItem(238, 181, "NpcFace_chicken_perceptive1", -1, new byte[2] { 18, 60 }, new short[2] { 0, 900 }, -1, isMonk: false, -1));
		_dataArray.Add(new EventActorsItem(239, 182, "NpcFace_chicken_perceptive2", -1, new byte[2] { 18, 60 }, new short[2] { 0, 900 }, -1, isMonk: false, -1));
	}

	private void CreateItems4()
	{
		_dataArray.Add(new EventActorsItem(240, 183, "NpcFace_chicken_perceptive3", -1, new byte[2] { 18, 60 }, new short[2] { 0, 900 }, -1, isMonk: false, -1));
		_dataArray.Add(new EventActorsItem(241, 184, "NpcFace_chicken_perceptive4", -1, new byte[2] { 18, 60 }, new short[2] { 0, 900 }, -1, isMonk: false, -1));
		_dataArray.Add(new EventActorsItem(242, 185, "NpcFace_chicken_perceptive5", -1, new byte[2] { 18, 60 }, new short[2] { 0, 900 }, -1, isMonk: false, -1));
		_dataArray.Add(new EventActorsItem(243, 186, "NpcFace_chicken_perceptive6", -1, new byte[2] { 18, 60 }, new short[2] { 0, 900 }, -1, isMonk: false, -1));
		_dataArray.Add(new EventActorsItem(244, 187, "NpcFace_chicken_perceptive7", -1, new byte[2] { 18, 60 }, new short[2] { 0, 900 }, -1, isMonk: false, -1));
		_dataArray.Add(new EventActorsItem(245, 188, "NpcFace_chicken_perceptive8", -1, new byte[2] { 18, 60 }, new short[2] { 0, 900 }, -1, isMonk: false, -1));
		_dataArray.Add(new EventActorsItem(246, 189, "NpcFace_chicken_firm0", -1, new byte[2] { 18, 60 }, new short[2] { 0, 900 }, -1, isMonk: false, -1));
		_dataArray.Add(new EventActorsItem(247, 190, "NpcFace_chicken_firm1", -1, new byte[2] { 18, 60 }, new short[2] { 0, 900 }, -1, isMonk: false, -1));
		_dataArray.Add(new EventActorsItem(248, 191, "NpcFace_chicken_firm2", -1, new byte[2] { 18, 60 }, new short[2] { 0, 900 }, -1, isMonk: false, -1));
		_dataArray.Add(new EventActorsItem(249, 192, "NpcFace_chicken_firm3", -1, new byte[2] { 18, 60 }, new short[2] { 0, 900 }, -1, isMonk: false, -1));
		_dataArray.Add(new EventActorsItem(250, 193, "NpcFace_chicken_firm4", -1, new byte[2] { 18, 60 }, new short[2] { 0, 900 }, -1, isMonk: false, -1));
		_dataArray.Add(new EventActorsItem(251, 194, "NpcFace_chicken_firm5", -1, new byte[2] { 18, 60 }, new short[2] { 0, 900 }, -1, isMonk: false, -1));
		_dataArray.Add(new EventActorsItem(252, 195, "NpcFace_chicken_firm6", -1, new byte[2] { 18, 60 }, new short[2] { 0, 900 }, -1, isMonk: false, -1));
		_dataArray.Add(new EventActorsItem(253, 196, "NpcFace_chicken_firm7", -1, new byte[2] { 18, 60 }, new short[2] { 0, 900 }, -1, isMonk: false, -1));
		_dataArray.Add(new EventActorsItem(254, 197, "NpcFace_chicken_firm8", -1, new byte[2] { 18, 60 }, new short[2] { 0, 900 }, -1, isMonk: false, -1));
		_dataArray.Add(new EventActorsItem(255, 198, "NpcFace_chicken_calm0", -1, new byte[2] { 18, 60 }, new short[2] { 0, 900 }, -1, isMonk: false, -1));
		_dataArray.Add(new EventActorsItem(256, 199, "NpcFace_chicken_calm1", -1, new byte[2] { 18, 60 }, new short[2] { 0, 900 }, -1, isMonk: false, -1));
		_dataArray.Add(new EventActorsItem(257, 200, "NpcFace_chicken_calm2", -1, new byte[2] { 18, 60 }, new short[2] { 0, 900 }, -1, isMonk: false, -1));
		_dataArray.Add(new EventActorsItem(258, 201, "NpcFace_chicken_calm3", -1, new byte[2] { 18, 60 }, new short[2] { 0, 900 }, -1, isMonk: false, -1));
		_dataArray.Add(new EventActorsItem(259, 202, "NpcFace_chicken_calm4", -1, new byte[2] { 18, 60 }, new short[2] { 0, 900 }, -1, isMonk: false, -1));
		_dataArray.Add(new EventActorsItem(260, 203, "NpcFace_chicken_calm5", -1, new byte[2] { 18, 60 }, new short[2] { 0, 900 }, -1, isMonk: false, -1));
		_dataArray.Add(new EventActorsItem(261, 204, "NpcFace_chicken_calm6", -1, new byte[2] { 18, 60 }, new short[2] { 0, 900 }, -1, isMonk: false, -1));
		_dataArray.Add(new EventActorsItem(262, 205, "NpcFace_chicken_calm7", -1, new byte[2] { 18, 60 }, new short[2] { 0, 900 }, -1, isMonk: false, -1));
		_dataArray.Add(new EventActorsItem(263, 206, "NpcFace_chicken_calm8", -1, new byte[2] { 18, 60 }, new short[2] { 0, 900 }, -1, isMonk: false, -1));
		_dataArray.Add(new EventActorsItem(264, 207, "NpcFace_chicken_enthusiastic0", -1, new byte[2] { 18, 60 }, new short[2] { 0, 900 }, -1, isMonk: false, -1));
		_dataArray.Add(new EventActorsItem(265, 208, "NpcFace_chicken_enthusiastic1", -1, new byte[2] { 18, 60 }, new short[2] { 0, 900 }, -1, isMonk: false, -1));
		_dataArray.Add(new EventActorsItem(266, 209, "NpcFace_chicken_enthusiastic2", -1, new byte[2] { 18, 60 }, new short[2] { 0, 900 }, -1, isMonk: false, -1));
		_dataArray.Add(new EventActorsItem(267, 210, "NpcFace_chicken_enthusiastic3", -1, new byte[2] { 18, 60 }, new short[2] { 0, 900 }, -1, isMonk: false, -1));
		_dataArray.Add(new EventActorsItem(268, 211, "NpcFace_chicken_enthusiastic4", -1, new byte[2] { 18, 60 }, new short[2] { 0, 900 }, -1, isMonk: false, -1));
		_dataArray.Add(new EventActorsItem(269, 212, "NpcFace_chicken_enthusiastic5", -1, new byte[2] { 18, 60 }, new short[2] { 0, 900 }, -1, isMonk: false, -1));
		_dataArray.Add(new EventActorsItem(270, 213, "NpcFace_chicken_enthusiastic6", -1, new byte[2] { 18, 60 }, new short[2] { 0, 900 }, -1, isMonk: false, -1));
		_dataArray.Add(new EventActorsItem(271, 214, "NpcFace_chicken_enthusiastic7", -1, new byte[2] { 18, 60 }, new short[2] { 0, 900 }, -1, isMonk: false, -1));
		_dataArray.Add(new EventActorsItem(272, 215, "NpcFace_chicken_enthusiastic8", -1, new byte[2] { 18, 60 }, new short[2] { 0, 900 }, -1, isMonk: false, -1));
		_dataArray.Add(new EventActorsItem(273, 216, "NpcFace_chicken_brave0", -1, new byte[2] { 18, 60 }, new short[2] { 0, 900 }, -1, isMonk: false, -1));
		_dataArray.Add(new EventActorsItem(274, 217, "NpcFace_chicken_brave1", -1, new byte[2] { 18, 60 }, new short[2] { 0, 900 }, -1, isMonk: false, -1));
		_dataArray.Add(new EventActorsItem(275, 218, "NpcFace_chicken_brave2", -1, new byte[2] { 18, 60 }, new short[2] { 0, 900 }, -1, isMonk: false, -1));
		_dataArray.Add(new EventActorsItem(276, 219, "NpcFace_chicken_brave3", -1, new byte[2] { 18, 60 }, new short[2] { 0, 900 }, -1, isMonk: false, -1));
		_dataArray.Add(new EventActorsItem(277, 220, "NpcFace_chicken_brave4", -1, new byte[2] { 18, 60 }, new short[2] { 0, 900 }, -1, isMonk: false, -1));
		_dataArray.Add(new EventActorsItem(278, 221, "NpcFace_chicken_brave5", -1, new byte[2] { 18, 60 }, new short[2] { 0, 900 }, -1, isMonk: false, -1));
		_dataArray.Add(new EventActorsItem(279, 222, "NpcFace_chicken_brave6", -1, new byte[2] { 18, 60 }, new short[2] { 0, 900 }, -1, isMonk: false, -1));
		_dataArray.Add(new EventActorsItem(280, 223, "NpcFace_chicken_brave7", -1, new byte[2] { 18, 60 }, new short[2] { 0, 900 }, -1, isMonk: false, -1));
		_dataArray.Add(new EventActorsItem(281, 224, "NpcFace_chicken_brave8", -1, new byte[2] { 18, 60 }, new short[2] { 0, 900 }, -1, isMonk: false, -1));
		_dataArray.Add(new EventActorsItem(282, 225, null, -1, new byte[2] { 20, 60 }, new short[2] { 0, 900 }, 10, isMonk: false, -1));
		_dataArray.Add(new EventActorsItem(283, 226, "NpcFace_tongshengtou", -1, new byte[2] { 18, 60 }, new short[2] { 0, 900 }, -1, isMonk: false, -1));
		_dataArray.Add(new EventActorsItem(284, 227, "NpcFace_ouyanzi", -1, new byte[2] { 18, 60 }, new short[2] { 0, 900 }, -1, isMonk: false, -1));
		_dataArray.Add(new EventActorsItem(285, 228, "NpcFace_tiandiheiying", -1, new byte[2] { 18, 60 }, new short[2] { 0, 900 }, -1, isMonk: false, -1));
		_dataArray.Add(new EventActorsItem(286, 229, null, 1, new byte[2] { 18, 25 }, new short[2] { 500, 900 }, 43, isMonk: false, -1));
		_dataArray.Add(new EventActorsItem(287, 229, null, 0, new byte[2] { 18, 25 }, new short[2] { 500, 900 }, 43, isMonk: false, -1));
		_dataArray.Add(new EventActorsItem(288, 230, null, 1, new byte[2] { 70, 80 }, new short[2] { 0, 900 }, 44, isMonk: false, -1));
		_dataArray.Add(new EventActorsItem(289, 231, null, 1, new byte[2] { 70, 80 }, new short[2] { 0, 900 }, 43, isMonk: false, -1));
		_dataArray.Add(new EventActorsItem(290, 232, null, -1, new byte[2] { 70, 80 }, new short[2] { 0, 900 }, 43, isMonk: false, -1));
		_dataArray.Add(new EventActorsItem(291, 233, null, 1, new byte[2] { 18, 60 }, new short[2] { 500, 900 }, 44, isMonk: false, -1));
		_dataArray.Add(new EventActorsItem(292, 10, null, 1, new byte[2] { 20, 40 }, new short[2] { 0, 900 }, 19, isMonk: true, -1));
		_dataArray.Add(new EventActorsItem(293, 11, null, -1, new byte[2] { 20, 40 }, new short[2] { 0, 900 }, 22, isMonk: false, -1));
		_dataArray.Add(new EventActorsItem(294, 12, null, -1, new byte[2] { 20, 40 }, new short[2] { 0, 900 }, 25, isMonk: false, -1));
		_dataArray.Add(new EventActorsItem(295, 13, null, -1, new byte[2] { 20, 40 }, new short[2] { 0, 900 }, 28, isMonk: false, -1));
		_dataArray.Add(new EventActorsItem(296, 14, null, -1, new byte[2] { 20, 40 }, new short[2] { 0, 900 }, 32, isMonk: false, -1));
		_dataArray.Add(new EventActorsItem(297, 15, null, -1, new byte[2] { 20, 40 }, new short[2] { 0, 900 }, 35, isMonk: false, -1));
		_dataArray.Add(new EventActorsItem(298, 16, null, -1, new byte[2] { 20, 40 }, new short[2] { 0, 900 }, 38, isMonk: false, -1));
		_dataArray.Add(new EventActorsItem(299, 17, null, 0, new byte[2] { 20, 40 }, new short[2] { 0, 900 }, 41, isMonk: false, -1));
	}

	private void CreateItems5()
	{
		_dataArray.Add(new EventActorsItem(300, 18, null, -1, new byte[2] { 20, 40 }, new short[2] { 0, 900 }, 44, isMonk: false, -1));
		_dataArray.Add(new EventActorsItem(301, 19, null, -1, new byte[2] { 20, 40 }, new short[2] { 0, 900 }, 47, isMonk: false, -1));
		_dataArray.Add(new EventActorsItem(302, 20, null, -1, new byte[2] { 20, 40 }, new short[2] { 0, 900 }, 50, isMonk: true, -1));
		_dataArray.Add(new EventActorsItem(303, 21, null, -1, new byte[2] { 20, 40 }, new short[2] { 0, 900 }, 53, isMonk: false, -1));
		_dataArray.Add(new EventActorsItem(304, 22, null, -1, new byte[2] { 20, 40 }, new short[2] { 0, 900 }, 56, isMonk: false, -1));
		_dataArray.Add(new EventActorsItem(305, 23, null, -1, new byte[2] { 20, 40 }, new short[2] { 0, 900 }, 59, isMonk: false, -1));
		_dataArray.Add(new EventActorsItem(306, 24, null, -1, new byte[2] { 20, 40 }, new short[2] { 0, 900 }, 62, isMonk: false, -1));
		_dataArray.Add(new EventActorsItem(307, 234, "NpcFace_chicken_clever0", -1, new byte[2] { 18, 60 }, new short[2] { 0, 900 }, -1, isMonk: false, -1));
		_dataArray.Add(new EventActorsItem(308, 9, null, 1, new byte[2] { 30, 60 }, new short[2] { 0, 900 }, 10, isMonk: false, -1));
		_dataArray.Add(new EventActorsItem(309, 9, null, 0, new byte[2] { 30, 60 }, new short[2] { 0, 900 }, 10, isMonk: false, -1));
		_dataArray.Add(new EventActorsItem(310, 9, null, -1, new byte[2] { 30, 60 }, new short[2] { 0, 900 }, 10, isMonk: false, -1));
		_dataArray.Add(new EventActorsItem(311, 14, null, -1, new byte[2] { 18, 25 }, new short[2] { 0, 900 }, 32, isMonk: false, -1));
		_dataArray.Add(new EventActorsItem(312, 14, null, 1, new byte[2] { 18, 25 }, new short[2] { 0, 900 }, 32, isMonk: false, -1));
		_dataArray.Add(new EventActorsItem(313, 14, null, 0, new byte[2] { 18, 25 }, new short[2] { 0, 900 }, 32, isMonk: false, -1));
		_dataArray.Add(new EventActorsItem(314, 84, null, 0, new byte[2] { 18, 18 }, new short[2] { 0, 900 }, -1, isMonk: false, -1));
		_dataArray.Add(new EventActorsItem(315, 157, null, 1, new byte[2] { 30, 40 }, new short[2] { 0, 900 }, 10, isMonk: false, -1));
		_dataArray.Add(new EventActorsItem(316, 235, null, -1, new byte[2] { 18, 25 }, new short[2] { 0, 900 }, 11, isMonk: false, -1));
		_dataArray.Add(new EventActorsItem(317, 236, null, 0, new byte[2] { 18, 25 }, new short[2] { 0, 900 }, 33, isMonk: false, -1));
		_dataArray.Add(new EventActorsItem(318, 237, null, -1, new byte[2] { 40, 60 }, new short[2] { 0, 900 }, 4, isMonk: false, -1));
		_dataArray.Add(new EventActorsItem(319, 238, null, -1, new byte[2] { 50, 70 }, new short[2] { 0, 900 }, 33, isMonk: false, -1));
		_dataArray.Add(new EventActorsItem(320, 239, null, 1, new byte[2] { 30, 60 }, new short[2] { 0, 900 }, 10, isMonk: false, -1));
		_dataArray.Add(new EventActorsItem(321, 240, null, 1, new byte[2] { 60, 70 }, new short[2] { 0, 900 }, 33, isMonk: false, -1));
		_dataArray.Add(new EventActorsItem(322, 128, "NpcFace_diqidaitaiwunan", -1, new byte[2] { 18, 60 }, new short[2] { 0, 900 }, -1, isMonk: false, -1));
		_dataArray.Add(new EventActorsItem(323, 128, "NpcFace_diqidaitaiwunv", -1, new byte[2] { 18, 60 }, new short[2] { 0, 900 }, -1, isMonk: false, -1));
	}

	public override void Init()
	{
		base.Init();
		_dataArray = new List<EventActorsItem>(324);
		CreateItems0();
		CreateItems1();
		CreateItems2();
		CreateItems3();
		CreateItems4();
		CreateItems5();
	}
}
