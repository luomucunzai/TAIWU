using System;
using System.Collections.Generic;
using Config.Common;
using Config.ConfigCells;

namespace Config;

[Serializable]
public class CombatState : ConfigData<CombatStateItem, short>
{
	public static class DefKey
	{
		public const short JinZhenFaMaiGongDirect = 0;

		public const short JinZhenFaMaiGongReverse = 1;

		public const short LiHunGongDirect = 2;

		public const short LiHunGongReverse = 3;

		public const short DaJinGangQuanDirectBuff = 4;

		public const short DaJinGangQuanDirectDebuff = 5;

		public const short DaJinGangQuanReverseBuff = 6;

		public const short DaJinGangQuanReverseDebuff = 7;

		public const short ShaoLinYiZhiChanDirect = 8;

		public const short ShaoLinYiZhiChanReverse = 9;

		public const short QingShenShu = 10;

		public const short ZhanYiShiBaDieDirect = 11;

		public const short ZhanYiShiBaDieReverse = 12;

		public const short YiHuaJieMuShouDirectBuff = 13;

		public const short YiHuaJieMuShouDirectDebuff = 14;

		public const short YiHuaJieMuShouReverseBuff = 15;

		public const short YiHuaJieMuShouReverseDebuff = 16;

		public const short EMeiYiZhiChanDirect = 17;

		public const short EMeiYiZhiChanReverse = 18;

		public const short JinDingFeiXian = 19;

		public const short XueHaiNingBingShuDirect = 20;

		public const short XueHaiNingBingShuReverse = 21;

		public const short TaiJiJianFaDirect = 22;

		public const short TaiJiJianFaReverse = 23;

		public const short YunChuangJiuLianDirect = 24;

		public const short YunChuangJiuLianReverse = 25;

		public const short KaiHeJianShuDirect0 = 26;

		public const short KaiHeJianShuDirect1 = 27;

		public const short KaiHeJianShuDirect2 = 28;

		public const short KaiHeJianShuDirect3 = 29;

		public const short KaiHeJianShuReverse0 = 30;

		public const short KaiHeJianShuReverse1 = 31;

		public const short KaiHeJianShuReverse2 = 32;

		public const short KaiHeJianShuReverse3 = 33;

		public const short YuFengFuDirect = 34;

		public const short YuFengFuReverse = 35;

		public const short BuSiGui = 36;

		public const short FengLaiYiDirect = 37;

		public const short FengLaiYiReverse = 38;

		public const short LeiZuBoJianShiDirect = 39;

		public const short LeiZuBoJianShiReverse = 40;

		public const short DaTaiYinYiMingZhi = 41;

		public const short DuanHunYouYinQu = 42;

		public const short ChaiShanQinDieShouDirect = 43;

		public const short ChaiShanQinDieShouReverse = 44;

		public const short WeiLingXianHuaGuZhangDirect = 45;

		public const short WeiLingXianHuaGuZhangReverse = 46;

		public const short KeJinZhenYuXiaoBaShiDirectBuff = 47;

		public const short KeJinZhenYuXiaoBaShiDirectDebuff = 48;

		public const short KeJinZhenYuXiaoBaShiReverseBuff = 49;

		public const short KeJinZhenYuXiaoBaShiReverseDebuff = 50;

		public const short FeiShanDuanHaiDaBaShiDirect = 51;

		public const short FeiShanDuanHaiDaBaShiReverse = 52;

		public const short WuNuShouDirect = 53;

		public const short WuNuShouReverse = 54;

		public const short JinGangHeiShaZhangDirect = 55;

		public const short JinGangHeiShaZhangReverse = 56;

		public const short NaMaiGongDirect = 57;

		public const short NaMaiGongReverse = 58;

		public const short GouLianJianFaDirect = 59;

		public const short GouLianJianFaReverse = 60;

		public const short YuSuoDaoXuanDirect = 61;

		public const short YuSuoDaoXuanReverse = 62;

		public const short TianSheFan = 63;

		public const short XueOuPoShaFaDirect = 64;

		public const short XueOuPoShaFaReverse = 65;

		public const short XueOuPoShaDirect = 66;

		public const short XueOuPoShaReverse = 67;

		public const short TianYuanZong = 68;

		public const short ChiQingShenHuoJinDirectBuff = 69;

		public const short ChiQingShenHuoJinDirectDebuff = 70;

		public const short ChiQingShenHuoJinReverseBuff = 71;

		public const short ChiQingShenHuoJinReverseDebuff = 72;

		public const short XieZiGouHunJiaoDirect = 73;

		public const short XieZiGouHunJiaoReverse = 74;

		public const short FuJunYouYuHit0 = 75;

		public const short FuJunYouYuHit1 = 76;

		public const short FuJunYouYuHit2 = 77;

		public const short FuJunYouYuHit3 = 78;

		public const short FuJunYouYuAvoid0 = 79;

		public const short FuJunYouYuAvoid1 = 80;

		public const short FuJunYouYuAvoid2 = 81;

		public const short FuJunYouYuAvoid3 = 82;

		public const short JieFeng0 = 83;

		public const short JieFeng1 = 84;

		public const short JieFeng2 = 85;

		public const short JieFeng3 = 86;

		public const short JieFeng4 = 87;

		public const short JieFeng5 = 88;

		public const short JieFeng6 = 89;

		public const short JieFeng7 = 90;

		public const short JieFeng8 = 91;

		public const short JieFeng9 = 92;

		public const short ShiFeng = 93;

		public const short ChaiRenBuff0 = 94;

		public const short ChaiRenBuff1 = 95;

		public const short ChaiRenBuff2 = 96;

		public const short ChaiRenBuff3 = 97;

		public const short ChaiRenBuff4 = 98;

		public const short ChaiRenBuff5 = 99;

		public const short ChaiRenBuff6 = 100;

		public const short ChaiRenBuff7 = 101;

		public const short ChaiRenBuff8 = 102;

		public const short ChaiRenBuff9 = 103;

		public const short ChaiRenDebuff0 = 104;

		public const short ChaiRenDebuff1 = 105;

		public const short ChaiRenDebuff2 = 106;

		public const short ChaiRenDebuff3 = 107;

		public const short ChaiRenDebuff4 = 108;

		public const short ChaiRenDebuff5 = 109;

		public const short ChaiRenDebuff6 = 110;

		public const short ChaiRenDebuff7 = 111;

		public const short ChaiRenDebuff8 = 112;

		public const short ChaiRenDebuff9 = 113;

		public const short DuoShenBuff = 114;

		public const short DuoShenDebuff = 115;

		public const short ReduceMindAvoid = 116;

		public const short LegendaryBook0 = 117;

		public const short LegendaryBook1 = 118;

		public const short LegendaryBook2 = 119;

		public const short LegendaryBook3 = 120;

		public const short LegendaryBook4 = 121;

		public const short LegendaryBook5 = 122;

		public const short LegendaryBook6 = 123;

		public const short LegendaryBook7 = 124;

		public const short LegendaryBook8 = 125;

		public const short LegendaryBook9 = 126;

		public const short LegendaryBook10 = 127;

		public const short LegendaryBook11 = 128;

		public const short LegendaryBook12 = 129;

		public const short LegendaryBook13 = 130;

		public const short SavageSkillMountain = 131;

		public const short SavageSkillCanyon = 132;

		public const short SavageSkillHill = 133;

		public const short SavageSkillField = 134;

		public const short SavageSkillWoodland = 135;

		public const short SavageSkillRiverBeach = 136;

		public const short SavageSkillLake = 137;

		public const short SavageSkillJungle = 138;

		public const short SavageSkillCave = 139;

		public const short SavageSkillSwamp = 140;

		public const short SavageSkillTaoYuan = 141;

		public const short WuMingQiDu0 = 142;

		public const short WuMingQiDu1 = 143;

		public const short WuMingQiDu2 = 144;

		public const short WuMingQiDu3 = 145;

		public const short HuaiXueDuanChang = 146;

		public const short JuEShenKu = 147;

		public const short Monkey0 = 148;

		public const short Eagle0 = 149;

		public const short Pig0 = 150;

		public const short Bear0 = 151;

		public const short Bull0 = 152;

		public const short Snake0 = 153;

		public const short Jaguar0 = 154;

		public const short Lion0 = 155;

		public const short Tiger0 = 156;

		public const short Monkey1 = 157;

		public const short Eagle1 = 158;

		public const short Pig1 = 159;

		public const short Bear1 = 160;

		public const short Bull1 = 161;

		public const short Snake1 = 162;

		public const short Jaguar1 = 163;

		public const short Lion1 = 164;

		public const short Tiger1 = 165;

		public const short QiLunGanYingFaDirect = 166;

		public const short QiLunGanYingFaReverse = 167;

		public const short JiaoWhite = 168;

		public const short JiaoBlack = 169;

		public const short JiaoGreen = 170;

		public const short JiaoRed = 171;

		public const short JiaoYellow = 172;

		public const short JiaoWB = 173;

		public const short JiaoWG = 174;

		public const short JiaoWR = 175;

		public const short JiaoWY = 176;

		public const short JiaoBG = 177;

		public const short JiaoBR = 178;

		public const short JiaoBY = 179;

		public const short JiaoGR = 180;

		public const short JiaoGY = 181;

		public const short JiaoRY = 182;

		public const short JiaoWBG = 183;

		public const short JiaoWBR = 184;

		public const short JiaoWBY = 185;

		public const short JiaoWGR = 186;

		public const short JiaoWGY = 187;

		public const short JiaoWRY = 188;

		public const short JiaoBGR = 189;

		public const short JiaoBGY = 190;

		public const short JiaoBRY = 191;

		public const short JiaoGRY = 192;

		public const short JiaoWBGR = 193;

		public const short JiaoWBGY = 194;

		public const short JiaoWBRY = 195;

		public const short JiaoWGRY = 196;

		public const short JiaoBGRY = 197;

		public const short JiaoWGRYB = 198;

		public const short Qiuniu = 199;

		public const short Yazi = 200;

		public const short Chaofeng = 201;

		public const short Pulao = 202;

		public const short Suanni = 203;

		public const short Baxia = 204;

		public const short Bian = 205;

		public const short Fuxi = 206;

		public const short Chiwen = 207;

		public const short LiuHeDaoFaDirect = 208;

		public const short LiuHeDaoFaReverse = 209;

		public const short ZhenYuXiangDirect = 210;

		public const short ZhenYuXiangReverse = 211;

		public const short YinFengXieZiShouDirect = 212;

		public const short YinFengXieZiShouReverse = 213;

		public const short HanBingCiGuFaDirect = 214;

		public const short HanBingCiGuFaReverse = 215;

		public const short ZhangXueGongDirect = 216;

		public const short ZhangXueGongReverse = 217;

		public const short HuangQuanZhiDirect = 218;

		public const short HuangQuanZhiReverse = 219;

		public const short DaHuaManTuoLuoZhiDirect = 220;

		public const short DaHuaManTuoLuoZhiReverse = 221;

		public const short QiongHuaTanDirect = 222;

		public const short QiongHuaTanReverse = 223;

		public const short XiongZhongSiQi = 224;

		public const short SiQiDuoHun = 225;

		public const short LegacyPowerShaolin = 226;

		public const short LegacyPowerEmei = 227;

		public const short LegacyPowerBaihua = 228;

		public const short LegacyPowerWudang = 229;

		public const short LegacyPowerYuanshan = 230;

		public const short LegacyPowerShixiang = 231;

		public const short LegacyPowerRanshan = 232;

		public const short LegacyPowerXuannv = 233;

		public const short LegacyPowerZhujian = 234;

		public const short LegacyPowerKongsang = 235;

		public const short LegacyPowerJingang = 236;

		public const short LegacyPowerWuxian = 237;

		public const short LegacyPowerJieqing = 238;

		public const short LegacyPowerFulong = 239;

		public const short LegacyPowerXuehou = 240;

		public const short SoulWitheringBell = 241;

		public const short SoulWitheringBellAfterTransfer = 242;

		public const short HuntingBeasts = 243;
	}

	public static class DefValue
	{
		public static CombatStateItem JinZhenFaMaiGongDirect => Instance[(short)0];

		public static CombatStateItem JinZhenFaMaiGongReverse => Instance[(short)1];

		public static CombatStateItem LiHunGongDirect => Instance[(short)2];

		public static CombatStateItem LiHunGongReverse => Instance[(short)3];

		public static CombatStateItem DaJinGangQuanDirectBuff => Instance[(short)4];

		public static CombatStateItem DaJinGangQuanDirectDebuff => Instance[(short)5];

		public static CombatStateItem DaJinGangQuanReverseBuff => Instance[(short)6];

		public static CombatStateItem DaJinGangQuanReverseDebuff => Instance[(short)7];

		public static CombatStateItem ShaoLinYiZhiChanDirect => Instance[(short)8];

		public static CombatStateItem ShaoLinYiZhiChanReverse => Instance[(short)9];

		public static CombatStateItem QingShenShu => Instance[(short)10];

		public static CombatStateItem ZhanYiShiBaDieDirect => Instance[(short)11];

		public static CombatStateItem ZhanYiShiBaDieReverse => Instance[(short)12];

		public static CombatStateItem YiHuaJieMuShouDirectBuff => Instance[(short)13];

		public static CombatStateItem YiHuaJieMuShouDirectDebuff => Instance[(short)14];

		public static CombatStateItem YiHuaJieMuShouReverseBuff => Instance[(short)15];

		public static CombatStateItem YiHuaJieMuShouReverseDebuff => Instance[(short)16];

		public static CombatStateItem EMeiYiZhiChanDirect => Instance[(short)17];

		public static CombatStateItem EMeiYiZhiChanReverse => Instance[(short)18];

		public static CombatStateItem JinDingFeiXian => Instance[(short)19];

		public static CombatStateItem XueHaiNingBingShuDirect => Instance[(short)20];

		public static CombatStateItem XueHaiNingBingShuReverse => Instance[(short)21];

		public static CombatStateItem TaiJiJianFaDirect => Instance[(short)22];

		public static CombatStateItem TaiJiJianFaReverse => Instance[(short)23];

		public static CombatStateItem YunChuangJiuLianDirect => Instance[(short)24];

		public static CombatStateItem YunChuangJiuLianReverse => Instance[(short)25];

		public static CombatStateItem KaiHeJianShuDirect0 => Instance[(short)26];

		public static CombatStateItem KaiHeJianShuDirect1 => Instance[(short)27];

		public static CombatStateItem KaiHeJianShuDirect2 => Instance[(short)28];

		public static CombatStateItem KaiHeJianShuDirect3 => Instance[(short)29];

		public static CombatStateItem KaiHeJianShuReverse0 => Instance[(short)30];

		public static CombatStateItem KaiHeJianShuReverse1 => Instance[(short)31];

		public static CombatStateItem KaiHeJianShuReverse2 => Instance[(short)32];

		public static CombatStateItem KaiHeJianShuReverse3 => Instance[(short)33];

		public static CombatStateItem YuFengFuDirect => Instance[(short)34];

		public static CombatStateItem YuFengFuReverse => Instance[(short)35];

		public static CombatStateItem BuSiGui => Instance[(short)36];

		public static CombatStateItem FengLaiYiDirect => Instance[(short)37];

		public static CombatStateItem FengLaiYiReverse => Instance[(short)38];

		public static CombatStateItem LeiZuBoJianShiDirect => Instance[(short)39];

		public static CombatStateItem LeiZuBoJianShiReverse => Instance[(short)40];

		public static CombatStateItem DaTaiYinYiMingZhi => Instance[(short)41];

		public static CombatStateItem DuanHunYouYinQu => Instance[(short)42];

		public static CombatStateItem ChaiShanQinDieShouDirect => Instance[(short)43];

		public static CombatStateItem ChaiShanQinDieShouReverse => Instance[(short)44];

		public static CombatStateItem WeiLingXianHuaGuZhangDirect => Instance[(short)45];

		public static CombatStateItem WeiLingXianHuaGuZhangReverse => Instance[(short)46];

		public static CombatStateItem KeJinZhenYuXiaoBaShiDirectBuff => Instance[(short)47];

		public static CombatStateItem KeJinZhenYuXiaoBaShiDirectDebuff => Instance[(short)48];

		public static CombatStateItem KeJinZhenYuXiaoBaShiReverseBuff => Instance[(short)49];

		public static CombatStateItem KeJinZhenYuXiaoBaShiReverseDebuff => Instance[(short)50];

		public static CombatStateItem FeiShanDuanHaiDaBaShiDirect => Instance[(short)51];

		public static CombatStateItem FeiShanDuanHaiDaBaShiReverse => Instance[(short)52];

		public static CombatStateItem WuNuShouDirect => Instance[(short)53];

		public static CombatStateItem WuNuShouReverse => Instance[(short)54];

		public static CombatStateItem JinGangHeiShaZhangDirect => Instance[(short)55];

		public static CombatStateItem JinGangHeiShaZhangReverse => Instance[(short)56];

		public static CombatStateItem NaMaiGongDirect => Instance[(short)57];

		public static CombatStateItem NaMaiGongReverse => Instance[(short)58];

		public static CombatStateItem GouLianJianFaDirect => Instance[(short)59];

		public static CombatStateItem GouLianJianFaReverse => Instance[(short)60];

		public static CombatStateItem YuSuoDaoXuanDirect => Instance[(short)61];

		public static CombatStateItem YuSuoDaoXuanReverse => Instance[(short)62];

		public static CombatStateItem TianSheFan => Instance[(short)63];

		public static CombatStateItem XueOuPoShaFaDirect => Instance[(short)64];

		public static CombatStateItem XueOuPoShaFaReverse => Instance[(short)65];

		public static CombatStateItem XueOuPoShaDirect => Instance[(short)66];

		public static CombatStateItem XueOuPoShaReverse => Instance[(short)67];

		public static CombatStateItem TianYuanZong => Instance[(short)68];

		public static CombatStateItem ChiQingShenHuoJinDirectBuff => Instance[(short)69];

		public static CombatStateItem ChiQingShenHuoJinDirectDebuff => Instance[(short)70];

		public static CombatStateItem ChiQingShenHuoJinReverseBuff => Instance[(short)71];

		public static CombatStateItem ChiQingShenHuoJinReverseDebuff => Instance[(short)72];

		public static CombatStateItem XieZiGouHunJiaoDirect => Instance[(short)73];

		public static CombatStateItem XieZiGouHunJiaoReverse => Instance[(short)74];

		public static CombatStateItem FuJunYouYuHit0 => Instance[(short)75];

		public static CombatStateItem FuJunYouYuHit1 => Instance[(short)76];

		public static CombatStateItem FuJunYouYuHit2 => Instance[(short)77];

		public static CombatStateItem FuJunYouYuHit3 => Instance[(short)78];

		public static CombatStateItem FuJunYouYuAvoid0 => Instance[(short)79];

		public static CombatStateItem FuJunYouYuAvoid1 => Instance[(short)80];

		public static CombatStateItem FuJunYouYuAvoid2 => Instance[(short)81];

		public static CombatStateItem FuJunYouYuAvoid3 => Instance[(short)82];

		public static CombatStateItem JieFeng0 => Instance[(short)83];

		public static CombatStateItem JieFeng1 => Instance[(short)84];

		public static CombatStateItem JieFeng2 => Instance[(short)85];

		public static CombatStateItem JieFeng3 => Instance[(short)86];

		public static CombatStateItem JieFeng4 => Instance[(short)87];

		public static CombatStateItem JieFeng5 => Instance[(short)88];

		public static CombatStateItem JieFeng6 => Instance[(short)89];

		public static CombatStateItem JieFeng7 => Instance[(short)90];

		public static CombatStateItem JieFeng8 => Instance[(short)91];

		public static CombatStateItem JieFeng9 => Instance[(short)92];

		public static CombatStateItem ShiFeng => Instance[(short)93];

		public static CombatStateItem ChaiRenBuff0 => Instance[(short)94];

		public static CombatStateItem ChaiRenBuff1 => Instance[(short)95];

		public static CombatStateItem ChaiRenBuff2 => Instance[(short)96];

		public static CombatStateItem ChaiRenBuff3 => Instance[(short)97];

		public static CombatStateItem ChaiRenBuff4 => Instance[(short)98];

		public static CombatStateItem ChaiRenBuff5 => Instance[(short)99];

		public static CombatStateItem ChaiRenBuff6 => Instance[(short)100];

		public static CombatStateItem ChaiRenBuff7 => Instance[(short)101];

		public static CombatStateItem ChaiRenBuff8 => Instance[(short)102];

		public static CombatStateItem ChaiRenBuff9 => Instance[(short)103];

		public static CombatStateItem ChaiRenDebuff0 => Instance[(short)104];

		public static CombatStateItem ChaiRenDebuff1 => Instance[(short)105];

		public static CombatStateItem ChaiRenDebuff2 => Instance[(short)106];

		public static CombatStateItem ChaiRenDebuff3 => Instance[(short)107];

		public static CombatStateItem ChaiRenDebuff4 => Instance[(short)108];

		public static CombatStateItem ChaiRenDebuff5 => Instance[(short)109];

		public static CombatStateItem ChaiRenDebuff6 => Instance[(short)110];

		public static CombatStateItem ChaiRenDebuff7 => Instance[(short)111];

		public static CombatStateItem ChaiRenDebuff8 => Instance[(short)112];

		public static CombatStateItem ChaiRenDebuff9 => Instance[(short)113];

		public static CombatStateItem DuoShenBuff => Instance[(short)114];

		public static CombatStateItem DuoShenDebuff => Instance[(short)115];

		public static CombatStateItem ReduceMindAvoid => Instance[(short)116];

		public static CombatStateItem LegendaryBook0 => Instance[(short)117];

		public static CombatStateItem LegendaryBook1 => Instance[(short)118];

		public static CombatStateItem LegendaryBook2 => Instance[(short)119];

		public static CombatStateItem LegendaryBook3 => Instance[(short)120];

		public static CombatStateItem LegendaryBook4 => Instance[(short)121];

		public static CombatStateItem LegendaryBook5 => Instance[(short)122];

		public static CombatStateItem LegendaryBook6 => Instance[(short)123];

		public static CombatStateItem LegendaryBook7 => Instance[(short)124];

		public static CombatStateItem LegendaryBook8 => Instance[(short)125];

		public static CombatStateItem LegendaryBook9 => Instance[(short)126];

		public static CombatStateItem LegendaryBook10 => Instance[(short)127];

		public static CombatStateItem LegendaryBook11 => Instance[(short)128];

		public static CombatStateItem LegendaryBook12 => Instance[(short)129];

		public static CombatStateItem LegendaryBook13 => Instance[(short)130];

		public static CombatStateItem SavageSkillMountain => Instance[(short)131];

		public static CombatStateItem SavageSkillCanyon => Instance[(short)132];

		public static CombatStateItem SavageSkillHill => Instance[(short)133];

		public static CombatStateItem SavageSkillField => Instance[(short)134];

		public static CombatStateItem SavageSkillWoodland => Instance[(short)135];

		public static CombatStateItem SavageSkillRiverBeach => Instance[(short)136];

		public static CombatStateItem SavageSkillLake => Instance[(short)137];

		public static CombatStateItem SavageSkillJungle => Instance[(short)138];

		public static CombatStateItem SavageSkillCave => Instance[(short)139];

		public static CombatStateItem SavageSkillSwamp => Instance[(short)140];

		public static CombatStateItem SavageSkillTaoYuan => Instance[(short)141];

		public static CombatStateItem WuMingQiDu0 => Instance[(short)142];

		public static CombatStateItem WuMingQiDu1 => Instance[(short)143];

		public static CombatStateItem WuMingQiDu2 => Instance[(short)144];

		public static CombatStateItem WuMingQiDu3 => Instance[(short)145];

		public static CombatStateItem HuaiXueDuanChang => Instance[(short)146];

		public static CombatStateItem JuEShenKu => Instance[(short)147];

		public static CombatStateItem Monkey0 => Instance[(short)148];

		public static CombatStateItem Eagle0 => Instance[(short)149];

		public static CombatStateItem Pig0 => Instance[(short)150];

		public static CombatStateItem Bear0 => Instance[(short)151];

		public static CombatStateItem Bull0 => Instance[(short)152];

		public static CombatStateItem Snake0 => Instance[(short)153];

		public static CombatStateItem Jaguar0 => Instance[(short)154];

		public static CombatStateItem Lion0 => Instance[(short)155];

		public static CombatStateItem Tiger0 => Instance[(short)156];

		public static CombatStateItem Monkey1 => Instance[(short)157];

		public static CombatStateItem Eagle1 => Instance[(short)158];

		public static CombatStateItem Pig1 => Instance[(short)159];

		public static CombatStateItem Bear1 => Instance[(short)160];

		public static CombatStateItem Bull1 => Instance[(short)161];

		public static CombatStateItem Snake1 => Instance[(short)162];

		public static CombatStateItem Jaguar1 => Instance[(short)163];

		public static CombatStateItem Lion1 => Instance[(short)164];

		public static CombatStateItem Tiger1 => Instance[(short)165];

		public static CombatStateItem QiLunGanYingFaDirect => Instance[(short)166];

		public static CombatStateItem QiLunGanYingFaReverse => Instance[(short)167];

		public static CombatStateItem JiaoWhite => Instance[(short)168];

		public static CombatStateItem JiaoBlack => Instance[(short)169];

		public static CombatStateItem JiaoGreen => Instance[(short)170];

		public static CombatStateItem JiaoRed => Instance[(short)171];

		public static CombatStateItem JiaoYellow => Instance[(short)172];

		public static CombatStateItem JiaoWB => Instance[(short)173];

		public static CombatStateItem JiaoWG => Instance[(short)174];

		public static CombatStateItem JiaoWR => Instance[(short)175];

		public static CombatStateItem JiaoWY => Instance[(short)176];

		public static CombatStateItem JiaoBG => Instance[(short)177];

		public static CombatStateItem JiaoBR => Instance[(short)178];

		public static CombatStateItem JiaoBY => Instance[(short)179];

		public static CombatStateItem JiaoGR => Instance[(short)180];

		public static CombatStateItem JiaoGY => Instance[(short)181];

		public static CombatStateItem JiaoRY => Instance[(short)182];

		public static CombatStateItem JiaoWBG => Instance[(short)183];

		public static CombatStateItem JiaoWBR => Instance[(short)184];

		public static CombatStateItem JiaoWBY => Instance[(short)185];

		public static CombatStateItem JiaoWGR => Instance[(short)186];

		public static CombatStateItem JiaoWGY => Instance[(short)187];

		public static CombatStateItem JiaoWRY => Instance[(short)188];

		public static CombatStateItem JiaoBGR => Instance[(short)189];

		public static CombatStateItem JiaoBGY => Instance[(short)190];

		public static CombatStateItem JiaoBRY => Instance[(short)191];

		public static CombatStateItem JiaoGRY => Instance[(short)192];

		public static CombatStateItem JiaoWBGR => Instance[(short)193];

		public static CombatStateItem JiaoWBGY => Instance[(short)194];

		public static CombatStateItem JiaoWBRY => Instance[(short)195];

		public static CombatStateItem JiaoWGRY => Instance[(short)196];

		public static CombatStateItem JiaoBGRY => Instance[(short)197];

		public static CombatStateItem JiaoWGRYB => Instance[(short)198];

		public static CombatStateItem Qiuniu => Instance[(short)199];

		public static CombatStateItem Yazi => Instance[(short)200];

		public static CombatStateItem Chaofeng => Instance[(short)201];

		public static CombatStateItem Pulao => Instance[(short)202];

		public static CombatStateItem Suanni => Instance[(short)203];

		public static CombatStateItem Baxia => Instance[(short)204];

		public static CombatStateItem Bian => Instance[(short)205];

		public static CombatStateItem Fuxi => Instance[(short)206];

		public static CombatStateItem Chiwen => Instance[(short)207];

		public static CombatStateItem LiuHeDaoFaDirect => Instance[(short)208];

		public static CombatStateItem LiuHeDaoFaReverse => Instance[(short)209];

		public static CombatStateItem ZhenYuXiangDirect => Instance[(short)210];

		public static CombatStateItem ZhenYuXiangReverse => Instance[(short)211];

		public static CombatStateItem YinFengXieZiShouDirect => Instance[(short)212];

		public static CombatStateItem YinFengXieZiShouReverse => Instance[(short)213];

		public static CombatStateItem HanBingCiGuFaDirect => Instance[(short)214];

		public static CombatStateItem HanBingCiGuFaReverse => Instance[(short)215];

		public static CombatStateItem ZhangXueGongDirect => Instance[(short)216];

		public static CombatStateItem ZhangXueGongReverse => Instance[(short)217];

		public static CombatStateItem HuangQuanZhiDirect => Instance[(short)218];

		public static CombatStateItem HuangQuanZhiReverse => Instance[(short)219];

		public static CombatStateItem DaHuaManTuoLuoZhiDirect => Instance[(short)220];

		public static CombatStateItem DaHuaManTuoLuoZhiReverse => Instance[(short)221];

		public static CombatStateItem QiongHuaTanDirect => Instance[(short)222];

		public static CombatStateItem QiongHuaTanReverse => Instance[(short)223];

		public static CombatStateItem XiongZhongSiQi => Instance[(short)224];

		public static CombatStateItem SiQiDuoHun => Instance[(short)225];

		public static CombatStateItem LegacyPowerShaolin => Instance[(short)226];

		public static CombatStateItem LegacyPowerEmei => Instance[(short)227];

		public static CombatStateItem LegacyPowerBaihua => Instance[(short)228];

		public static CombatStateItem LegacyPowerWudang => Instance[(short)229];

		public static CombatStateItem LegacyPowerYuanshan => Instance[(short)230];

		public static CombatStateItem LegacyPowerShixiang => Instance[(short)231];

		public static CombatStateItem LegacyPowerRanshan => Instance[(short)232];

		public static CombatStateItem LegacyPowerXuannv => Instance[(short)233];

		public static CombatStateItem LegacyPowerZhujian => Instance[(short)234];

		public static CombatStateItem LegacyPowerKongsang => Instance[(short)235];

		public static CombatStateItem LegacyPowerJingang => Instance[(short)236];

		public static CombatStateItem LegacyPowerWuxian => Instance[(short)237];

		public static CombatStateItem LegacyPowerJieqing => Instance[(short)238];

		public static CombatStateItem LegacyPowerFulong => Instance[(short)239];

		public static CombatStateItem LegacyPowerXuehou => Instance[(short)240];

		public static CombatStateItem SoulWitheringBell => Instance[(short)241];

		public static CombatStateItem SoulWitheringBellAfterTransfer => Instance[(short)242];

		public static CombatStateItem HuntingBeasts => Instance[(short)243];
	}

	public static CombatState Instance = new CombatState();

	private readonly HashSet<string> RequiredFields = new HashSet<string> { "PropertyList", "ReverseState", "TemplateId", "Name", "TipsDesc", "Desc" };

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
		_dataArray.Add(new CombatStateItem(0, 0, new List<CombatStateProperty>
		{
			new CombatStateProperty(6, 3, 1),
			new CombatStateProperty(7, 3, 1),
			new CombatStateProperty(8, 3, 1),
			new CombatStateProperty(9, 3, 1)
		}, -1, 1, 2));
		_dataArray.Add(new CombatStateItem(1, 0, new List<CombatStateProperty>
		{
			new CombatStateProperty(0, 3, 1),
			new CombatStateProperty(1, 3, 1),
			new CombatStateProperty(2, 3, 1),
			new CombatStateProperty(3, 3, 1)
		}, -1, 3, 4));
		_dataArray.Add(new CombatStateItem(2, 5, new List<CombatStateProperty>
		{
			new CombatStateProperty(6, 3, 1),
			new CombatStateProperty(7, 3, 1),
			new CombatStateProperty(8, 3, 1),
			new CombatStateProperty(9, 3, 1)
		}, -1, 6, 7));
		_dataArray.Add(new CombatStateItem(3, 5, new List<CombatStateProperty>
		{
			new CombatStateProperty(0, 3, 1),
			new CombatStateProperty(1, 3, 1),
			new CombatStateProperty(2, 3, 1),
			new CombatStateProperty(3, 3, 1)
		}, -1, 8, 9));
		_dataArray.Add(new CombatStateItem(4, 10, new List<CombatStateProperty>
		{
			new CombatStateProperty(4, 3, 1)
		}, -1, 11, 12));
		_dataArray.Add(new CombatStateItem(5, 10, new List<CombatStateProperty>
		{
			new CombatStateProperty(5, -3, 1)
		}, -1, 13, 14));
		_dataArray.Add(new CombatStateItem(6, 10, new List<CombatStateProperty>
		{
			new CombatStateProperty(5, 3, 1)
		}, -1, 15, 16));
		_dataArray.Add(new CombatStateItem(7, 10, new List<CombatStateProperty>
		{
			new CombatStateProperty(4, -3, 1)
		}, -1, 17, 18));
		_dataArray.Add(new CombatStateItem(8, 19, new List<CombatStateProperty>
		{
			new CombatStateProperty(4, 3, 1)
		}, -1, 20, 21));
		_dataArray.Add(new CombatStateItem(9, 19, new List<CombatStateProperty>
		{
			new CombatStateProperty(10, -3, 1)
		}, -1, 22, 23));
		_dataArray.Add(new CombatStateItem(10, 24, new List<CombatStateProperty>
		{
			new CombatStateProperty(32, 7, 1),
			new CombatStateProperty(13, 20, 0),
			new CombatStateProperty(12, 20, 0)
		}, -1, 25, 26));
		_dataArray.Add(new CombatStateItem(11, 27, new List<CombatStateProperty>
		{
			new CombatStateProperty(6, 3, 1),
			new CombatStateProperty(7, 3, 1),
			new CombatStateProperty(8, 3, 1)
		}, -1, 28, 29));
		_dataArray.Add(new CombatStateItem(12, 27, new List<CombatStateProperty>
		{
			new CombatStateProperty(6, -3, 1),
			new CombatStateProperty(7, -3, 1),
			new CombatStateProperty(8, -3, 1)
		}, -1, 30, 31));
		_dataArray.Add(new CombatStateItem(13, 32, new List<CombatStateProperty>
		{
			new CombatStateProperty(11, 3, 1)
		}, -1, 33, 34));
		_dataArray.Add(new CombatStateItem(14, 32, new List<CombatStateProperty>
		{
			new CombatStateProperty(10, -3, 1)
		}, -1, 35, 36));
		_dataArray.Add(new CombatStateItem(15, 32, new List<CombatStateProperty>
		{
			new CombatStateProperty(10, 3, 1)
		}, -1, 37, 38));
		_dataArray.Add(new CombatStateItem(16, 32, new List<CombatStateProperty>
		{
			new CombatStateProperty(11, -3, 1)
		}, -1, 39, 40));
		_dataArray.Add(new CombatStateItem(17, 41, new List<CombatStateProperty>
		{
			new CombatStateProperty(5, 3, 1)
		}, -1, 42, 43));
		_dataArray.Add(new CombatStateItem(18, 41, new List<CombatStateProperty>
		{
			new CombatStateProperty(11, -3, 1)
		}, -1, 44, 45));
		_dataArray.Add(new CombatStateItem(19, 46, new List<CombatStateProperty>
		{
			new CombatStateProperty(0, 3, 1),
			new CombatStateProperty(1, 3, 1),
			new CombatStateProperty(2, 3, 1)
		}, -1, 47, 48));
		_dataArray.Add(new CombatStateItem(20, 49, new List<CombatStateProperty>
		{
			new CombatStateProperty(10, 300, 0)
		}, -1, 50, 51));
		_dataArray.Add(new CombatStateItem(21, 49, new List<CombatStateProperty>
		{
			new CombatStateProperty(11, 300, 0)
		}, -1, 52, 53));
		_dataArray.Add(new CombatStateItem(22, 54, new List<CombatStateProperty>
		{
			new CombatStateProperty(4, -300, 0),
			new CombatStateProperty(5, -300, 0)
		}, -1, 55, 56));
		_dataArray.Add(new CombatStateItem(23, 54, new List<CombatStateProperty>
		{
			new CombatStateProperty(10, -300, 0),
			new CombatStateProperty(11, -300, 0)
		}, -1, 57, 58));
		_dataArray.Add(new CombatStateItem(24, 59, new List<CombatStateProperty>
		{
			new CombatStateProperty(15, 30, 0)
		}, -1, 60, 61));
		_dataArray.Add(new CombatStateItem(25, 59, new List<CombatStateProperty>
		{
			new CombatStateProperty(17, 30, 0)
		}, -1, 62, 63));
		_dataArray.Add(new CombatStateItem(26, 64, new List<CombatStateProperty>
		{
			new CombatStateProperty(0, -3, 1)
		}, -1, 65, 66));
		_dataArray.Add(new CombatStateItem(27, 64, new List<CombatStateProperty>
		{
			new CombatStateProperty(1, -3, 1)
		}, -1, 67, 68));
		_dataArray.Add(new CombatStateItem(28, 64, new List<CombatStateProperty>
		{
			new CombatStateProperty(2, -3, 1)
		}, -1, 69, 70));
		_dataArray.Add(new CombatStateItem(29, 64, new List<CombatStateProperty>
		{
			new CombatStateProperty(3, -3, 1)
		}, -1, 71, 72));
		_dataArray.Add(new CombatStateItem(30, 64, new List<CombatStateProperty>
		{
			new CombatStateProperty(6, -3, 1)
		}, -1, 73, 74));
		_dataArray.Add(new CombatStateItem(31, 64, new List<CombatStateProperty>
		{
			new CombatStateProperty(7, -3, 1)
		}, -1, 75, 76));
		_dataArray.Add(new CombatStateItem(32, 64, new List<CombatStateProperty>
		{
			new CombatStateProperty(8, -3, 1)
		}, -1, 77, 78));
		_dataArray.Add(new CombatStateItem(33, 64, new List<CombatStateProperty>
		{
			new CombatStateProperty(9, -3, 1)
		}, -1, 79, 80));
		_dataArray.Add(new CombatStateItem(34, 81, new List<CombatStateProperty>
		{
			new CombatStateProperty(16, 30, 0)
		}, -1, 82, 83));
		_dataArray.Add(new CombatStateItem(35, 81, new List<CombatStateProperty>
		{
			new CombatStateProperty(16, -30, 0)
		}, -1, 84, 85));
		_dataArray.Add(new CombatStateItem(36, 86, new List<CombatStateProperty>
		{
			new CombatStateProperty(30, 15, 0)
		}, -1, 87, 88));
		_dataArray.Add(new CombatStateItem(37, 89, new List<CombatStateProperty>
		{
			new CombatStateProperty(3, 3, 1),
			new CombatStateProperty(9, 3, 1)
		}, -1, 90, 91));
		_dataArray.Add(new CombatStateItem(38, 89, new List<CombatStateProperty>
		{
			new CombatStateProperty(3, -3, 1),
			new CombatStateProperty(9, -3, 1)
		}, -1, 92, 93));
		_dataArray.Add(new CombatStateItem(39, 94, new List<CombatStateProperty>
		{
			new CombatStateProperty(10, -3, 1)
		}, -1, 95, 96));
		_dataArray.Add(new CombatStateItem(40, 94, new List<CombatStateProperty>
		{
			new CombatStateProperty(11, -3, 1)
		}, -1, 97, 98));
		_dataArray.Add(new CombatStateItem(41, 99, new List<CombatStateProperty>
		{
			new CombatStateProperty(31, -15, 1)
		}, -1, 100, 101));
		_dataArray.Add(new CombatStateItem(42, 102, new List<CombatStateProperty>
		{
			new CombatStateProperty(30, -15, 0)
		}, -1, 103, 104));
		_dataArray.Add(new CombatStateItem(43, 105, new List<CombatStateProperty>
		{
			new CombatStateProperty(14, -30, 0)
		}, -1, 106, 107));
		_dataArray.Add(new CombatStateItem(44, 105, new List<CombatStateProperty>
		{
			new CombatStateProperty(19, -30, 0)
		}, -1, 108, 109));
		_dataArray.Add(new CombatStateItem(45, 110, new List<CombatStateProperty>
		{
			new CombatStateProperty(15, -30, 0)
		}, -1, 111, 112));
		_dataArray.Add(new CombatStateItem(46, 110, new List<CombatStateProperty>
		{
			new CombatStateProperty(17, -30, 0)
		}, -1, 113, 114));
		_dataArray.Add(new CombatStateItem(47, 115, new List<CombatStateProperty>
		{
			new CombatStateProperty(4, 3, 1)
		}, -1, 116, 117));
		_dataArray.Add(new CombatStateItem(48, 115, new List<CombatStateProperty>
		{
			new CombatStateProperty(10, -3, 1)
		}, -1, 118, 119));
		_dataArray.Add(new CombatStateItem(49, 115, new List<CombatStateProperty>
		{
			new CombatStateProperty(5, 3, 1)
		}, -1, 120, 121));
		_dataArray.Add(new CombatStateItem(50, 115, new List<CombatStateProperty>
		{
			new CombatStateProperty(11, -3, 1)
		}, -1, 122, 123));
		_dataArray.Add(new CombatStateItem(51, 124, new List<CombatStateProperty>
		{
			new CombatStateProperty(15, -10, 1)
		}, -1, 125, 126));
		_dataArray.Add(new CombatStateItem(52, 124, new List<CombatStateProperty>
		{
			new CombatStateProperty(17, -10, 1)
		}, -1, 127, 128));
		_dataArray.Add(new CombatStateItem(53, 129, new List<CombatStateProperty>
		{
			new CombatStateProperty(4, 3, 1)
		}, -1, 130, 131));
		_dataArray.Add(new CombatStateItem(54, 129, new List<CombatStateProperty>
		{
			new CombatStateProperty(5, 3, 1)
		}, -1, 132, 133));
		_dataArray.Add(new CombatStateItem(55, 134, new List<CombatStateProperty>
		{
			new CombatStateProperty(28, 3, 1)
		}, -1, 135, 136));
		_dataArray.Add(new CombatStateItem(56, 134, new List<CombatStateProperty>
		{
			new CombatStateProperty(29, 3, 1)
		}, -1, 137, 138));
		_dataArray.Add(new CombatStateItem(57, 139, new List<CombatStateProperty>
		{
			new CombatStateProperty(12, -5, 1)
		}, -1, 140, 141));
		_dataArray.Add(new CombatStateItem(58, 139, new List<CombatStateProperty>
		{
			new CombatStateProperty(13, -5, 1)
		}, -1, 142, 143));
		_dataArray.Add(new CombatStateItem(59, 144, new List<CombatStateProperty>
		{
			new CombatStateProperty(19, -5, 1)
		}, -1, 145, 146));
	}

	private void CreateItems1()
	{
		_dataArray.Add(new CombatStateItem(60, 144, new List<CombatStateProperty>
		{
			new CombatStateProperty(14, -5, 1)
		}, -1, 147, 148));
		_dataArray.Add(new CombatStateItem(61, 149, new List<CombatStateProperty>
		{
			new CombatStateProperty(13, 30, 0),
			new CombatStateProperty(12, 30, 0)
		}, -1, 150, 151));
		_dataArray.Add(new CombatStateItem(62, 149, new List<CombatStateProperty>
		{
			new CombatStateProperty(13, -30, 0),
			new CombatStateProperty(12, -30, 0)
		}, -1, 152, 153));
		_dataArray.Add(new CombatStateItem(63, 154, new List<CombatStateProperty>
		{
			new CombatStateProperty(22, -60, 0),
			new CombatStateProperty(23, -60, 0),
			new CombatStateProperty(24, -60, 0),
			new CombatStateProperty(25, -60, 0),
			new CombatStateProperty(26, -60, 0),
			new CombatStateProperty(27, -60, 0)
		}, -1, 155, 156));
		_dataArray.Add(new CombatStateItem(64, 157, new List<CombatStateProperty>
		{
			new CombatStateProperty(4, -3, 1)
		}, -1, 158, 159));
		_dataArray.Add(new CombatStateItem(65, 157, new List<CombatStateProperty>
		{
			new CombatStateProperty(5, -3, 1)
		}, -1, 160, 161));
		_dataArray.Add(new CombatStateItem(66, 162, new List<CombatStateProperty>(), -1, 163, 164));
		_dataArray.Add(new CombatStateItem(67, 162, new List<CombatStateProperty>(), -1, 165, 166));
		_dataArray.Add(new CombatStateItem(68, 167, new List<CombatStateProperty>
		{
			new CombatStateProperty(4, 3, 1),
			new CombatStateProperty(5, 3, 1)
		}, -1, 168, 169));
		_dataArray.Add(new CombatStateItem(69, 170, new List<CombatStateProperty>
		{
			new CombatStateProperty(4, 6, 1)
		}, -1, 171, 172));
		_dataArray.Add(new CombatStateItem(70, 170, new List<CombatStateProperty>
		{
			new CombatStateProperty(10, -3, 1)
		}, -1, 173, 174));
		_dataArray.Add(new CombatStateItem(71, 170, new List<CombatStateProperty>
		{
			new CombatStateProperty(5, 6, 1)
		}, -1, 175, 176));
		_dataArray.Add(new CombatStateItem(72, 170, new List<CombatStateProperty>
		{
			new CombatStateProperty(11, -3, 1)
		}, -1, 177, 178));
		_dataArray.Add(new CombatStateItem(73, 179, new List<CombatStateProperty>
		{
			new CombatStateProperty(14, 30, 0)
		}, -1, 180, 181));
		_dataArray.Add(new CombatStateItem(74, 179, new List<CombatStateProperty>
		{
			new CombatStateProperty(19, 30, 0)
		}, -1, 182, 183));
		_dataArray.Add(new CombatStateItem(75, 184, new List<CombatStateProperty>
		{
			new CombatStateProperty(0, 600, 0)
		}, -1, 185, 186));
		_dataArray.Add(new CombatStateItem(76, 184, new List<CombatStateProperty>
		{
			new CombatStateProperty(1, 600, 0)
		}, -1, 187, 188));
		_dataArray.Add(new CombatStateItem(77, 184, new List<CombatStateProperty>
		{
			new CombatStateProperty(2, 600, 0)
		}, -1, 189, 190));
		_dataArray.Add(new CombatStateItem(78, 184, new List<CombatStateProperty>
		{
			new CombatStateProperty(3, 600, 0)
		}, -1, 191, 192));
		_dataArray.Add(new CombatStateItem(79, 184, new List<CombatStateProperty>
		{
			new CombatStateProperty(6, 600, 0)
		}, -1, 193, 194));
		_dataArray.Add(new CombatStateItem(80, 184, new List<CombatStateProperty>
		{
			new CombatStateProperty(7, 600, 0)
		}, -1, 195, 196));
		_dataArray.Add(new CombatStateItem(81, 184, new List<CombatStateProperty>
		{
			new CombatStateProperty(8, 600, 0)
		}, -1, 197, 198));
		_dataArray.Add(new CombatStateItem(82, 184, new List<CombatStateProperty>
		{
			new CombatStateProperty(9, 600, 0)
		}, -1, 199, 200));
		_dataArray.Add(new CombatStateItem(83, 201, new List<CombatStateProperty>
		{
			new CombatStateProperty(12, 60, 0)
		}, -1, 202, 203));
		_dataArray.Add(new CombatStateItem(84, 201, new List<CombatStateProperty>
		{
			new CombatStateProperty(13, 60, 0)
		}, -1, 204, 205));
		_dataArray.Add(new CombatStateItem(85, 201, new List<CombatStateProperty>
		{
			new CombatStateProperty(14, 60, 0)
		}, -1, 206, 207));
		_dataArray.Add(new CombatStateItem(86, 201, new List<CombatStateProperty>
		{
			new CombatStateProperty(15, 60, 0)
		}, -1, 208, 209));
		_dataArray.Add(new CombatStateItem(87, 201, new List<CombatStateProperty>
		{
			new CombatStateProperty(16, 60, 0)
		}, -1, 210, 211));
		_dataArray.Add(new CombatStateItem(88, 201, new List<CombatStateProperty>
		{
			new CombatStateProperty(17, 60, 0)
		}, -1, 212, 213));
		_dataArray.Add(new CombatStateItem(89, 201, new List<CombatStateProperty>
		{
			new CombatStateProperty(18, 60, 0)
		}, -1, 214, 215));
		_dataArray.Add(new CombatStateItem(90, 201, new List<CombatStateProperty>
		{
			new CombatStateProperty(19, 60, 0)
		}, -1, 216, 217));
		_dataArray.Add(new CombatStateItem(91, 201, new List<CombatStateProperty>
		{
			new CombatStateProperty(20, 60, 0)
		}, -1, 218, 219));
		_dataArray.Add(new CombatStateItem(92, 201, new List<CombatStateProperty>
		{
			new CombatStateProperty(21, 60, 0)
		}, -1, 220, 221));
		_dataArray.Add(new CombatStateItem(93, 222, new List<CombatStateProperty>
		{
			new CombatStateProperty(4, 16, 1),
			new CombatStateProperty(5, 16, 1)
		}, -1, 223, 224));
		_dataArray.Add(new CombatStateItem(94, 225, new List<CombatStateProperty>
		{
			new CombatStateProperty(12, 60, 0)
		}, -1, 226, 227));
		_dataArray.Add(new CombatStateItem(95, 225, new List<CombatStateProperty>
		{
			new CombatStateProperty(13, 60, 0)
		}, -1, 228, 229));
		_dataArray.Add(new CombatStateItem(96, 225, new List<CombatStateProperty>
		{
			new CombatStateProperty(14, 60, 0)
		}, -1, 230, 231));
		_dataArray.Add(new CombatStateItem(97, 225, new List<CombatStateProperty>
		{
			new CombatStateProperty(15, 60, 0)
		}, -1, 232, 233));
		_dataArray.Add(new CombatStateItem(98, 225, new List<CombatStateProperty>
		{
			new CombatStateProperty(16, 60, 0)
		}, -1, 234, 235));
		_dataArray.Add(new CombatStateItem(99, 225, new List<CombatStateProperty>
		{
			new CombatStateProperty(17, 60, 0)
		}, -1, 236, 237));
		_dataArray.Add(new CombatStateItem(100, 225, new List<CombatStateProperty>
		{
			new CombatStateProperty(18, 60, 0)
		}, -1, 238, 239));
		_dataArray.Add(new CombatStateItem(101, 225, new List<CombatStateProperty>
		{
			new CombatStateProperty(19, 60, 0)
		}, -1, 240, 241));
		_dataArray.Add(new CombatStateItem(102, 225, new List<CombatStateProperty>
		{
			new CombatStateProperty(20, 60, 0)
		}, -1, 242, 243));
		_dataArray.Add(new CombatStateItem(103, 225, new List<CombatStateProperty>
		{
			new CombatStateProperty(21, 60, 0)
		}, -1, 244, 245));
		_dataArray.Add(new CombatStateItem(104, 225, new List<CombatStateProperty>
		{
			new CombatStateProperty(12, -60, 0)
		}, -1, 246, 247));
		_dataArray.Add(new CombatStateItem(105, 225, new List<CombatStateProperty>
		{
			new CombatStateProperty(13, -60, 0)
		}, -1, 248, 249));
		_dataArray.Add(new CombatStateItem(106, 225, new List<CombatStateProperty>
		{
			new CombatStateProperty(14, -60, 0)
		}, -1, 250, 251));
		_dataArray.Add(new CombatStateItem(107, 225, new List<CombatStateProperty>
		{
			new CombatStateProperty(15, -60, 0)
		}, -1, 252, 253));
		_dataArray.Add(new CombatStateItem(108, 225, new List<CombatStateProperty>
		{
			new CombatStateProperty(16, -60, 0)
		}, -1, 254, 255));
		_dataArray.Add(new CombatStateItem(109, 225, new List<CombatStateProperty>
		{
			new CombatStateProperty(17, -60, 0)
		}, -1, 256, 257));
		_dataArray.Add(new CombatStateItem(110, 225, new List<CombatStateProperty>
		{
			new CombatStateProperty(18, -60, 0)
		}, -1, 258, 259));
		_dataArray.Add(new CombatStateItem(111, 225, new List<CombatStateProperty>
		{
			new CombatStateProperty(19, -60, 0)
		}, -1, 260, 261));
		_dataArray.Add(new CombatStateItem(112, 225, new List<CombatStateProperty>
		{
			new CombatStateProperty(20, -60, 0)
		}, -1, 262, 263));
		_dataArray.Add(new CombatStateItem(113, 225, new List<CombatStateProperty>
		{
			new CombatStateProperty(21, -60, 0)
		}, -1, 264, 265));
		_dataArray.Add(new CombatStateItem(114, 266, new List<CombatStateProperty>
		{
			new CombatStateProperty(4, 16, 1),
			new CombatStateProperty(5, 16, 1)
		}, -1, 267, 268));
		_dataArray.Add(new CombatStateItem(115, 266, new List<CombatStateProperty>
		{
			new CombatStateProperty(4, -16, 1),
			new CombatStateProperty(5, -16, 1)
		}, -1, 269, 270));
		_dataArray.Add(new CombatStateItem(116, 271, new List<CombatStateProperty>
		{
			new CombatStateProperty(9, -6, 1)
		}, -1, 272, 273));
		_dataArray.Add(new CombatStateItem(117, 274, new List<CombatStateProperty>(), -1, 275, 276));
		_dataArray.Add(new CombatStateItem(118, 277, new List<CombatStateProperty>(), -1, 278, 279));
		_dataArray.Add(new CombatStateItem(119, 280, new List<CombatStateProperty>(), -1, 281, 282));
	}

	private void CreateItems2()
	{
		_dataArray.Add(new CombatStateItem(120, 283, new List<CombatStateProperty>(), -1, 284, 285));
		_dataArray.Add(new CombatStateItem(121, 286, new List<CombatStateProperty>(), -1, 287, 288));
		_dataArray.Add(new CombatStateItem(122, 289, new List<CombatStateProperty>(), -1, 290, 291));
		_dataArray.Add(new CombatStateItem(123, 292, new List<CombatStateProperty>(), -1, 293, 294));
		_dataArray.Add(new CombatStateItem(124, 295, new List<CombatStateProperty>(), -1, 296, 297));
		_dataArray.Add(new CombatStateItem(125, 298, new List<CombatStateProperty>(), -1, 299, 300));
		_dataArray.Add(new CombatStateItem(126, 301, new List<CombatStateProperty>(), -1, 302, 303));
		_dataArray.Add(new CombatStateItem(127, 304, new List<CombatStateProperty>(), -1, 305, 306));
		_dataArray.Add(new CombatStateItem(128, 307, new List<CombatStateProperty>(), -1, 308, 309));
		_dataArray.Add(new CombatStateItem(129, 310, new List<CombatStateProperty>(), -1, 311, 312));
		_dataArray.Add(new CombatStateItem(130, 313, new List<CombatStateProperty>(), -1, 314, 315));
		_dataArray.Add(new CombatStateItem(131, 316, new List<CombatStateProperty>(), -1, 317, 318));
		_dataArray.Add(new CombatStateItem(132, 319, new List<CombatStateProperty>(), -1, 320, 321));
		_dataArray.Add(new CombatStateItem(133, 322, new List<CombatStateProperty>(), -1, 323, 324));
		_dataArray.Add(new CombatStateItem(134, 325, new List<CombatStateProperty>(), -1, 326, 327));
		_dataArray.Add(new CombatStateItem(135, 328, new List<CombatStateProperty>(), -1, 329, 330));
		_dataArray.Add(new CombatStateItem(136, 331, new List<CombatStateProperty>(), -1, 332, 333));
		_dataArray.Add(new CombatStateItem(137, 334, new List<CombatStateProperty>(), -1, 335, 336));
		_dataArray.Add(new CombatStateItem(138, 337, new List<CombatStateProperty>(), -1, 338, 339));
		_dataArray.Add(new CombatStateItem(139, 340, new List<CombatStateProperty>(), -1, 341, 342));
		_dataArray.Add(new CombatStateItem(140, 343, new List<CombatStateProperty>(), -1, 344, 345));
		_dataArray.Add(new CombatStateItem(141, 346, new List<CombatStateProperty>(), -1, 347, 348));
		_dataArray.Add(new CombatStateItem(142, 349, new List<CombatStateProperty>(), -1, 350, 351));
		_dataArray.Add(new CombatStateItem(143, 349, new List<CombatStateProperty>(), -1, 350, 352));
		_dataArray.Add(new CombatStateItem(144, 349, new List<CombatStateProperty>(), -1, 350, 353));
		_dataArray.Add(new CombatStateItem(145, 349, new List<CombatStateProperty>(), -1, 354, 355));
		_dataArray.Add(new CombatStateItem(146, 356, new List<CombatStateProperty>
		{
			new CombatStateProperty(6, -6, 1),
			new CombatStateProperty(7, -6, 1),
			new CombatStateProperty(8, -6, 1),
			new CombatStateProperty(9, -6, 1)
		}, -1, 357, 358));
		_dataArray.Add(new CombatStateItem(147, 359, new List<CombatStateProperty>
		{
			new CombatStateProperty(0, -6, 1),
			new CombatStateProperty(1, -6, 1),
			new CombatStateProperty(2, -6, 1),
			new CombatStateProperty(3, -6, 1)
		}, -1, 360, 361));
		_dataArray.Add(new CombatStateItem(148, 362, new List<CombatStateProperty>(), -1, 363, 364));
		_dataArray.Add(new CombatStateItem(149, 365, new List<CombatStateProperty>(), -1, 366, 367));
		_dataArray.Add(new CombatStateItem(150, 368, new List<CombatStateProperty>(), -1, 369, 370));
		_dataArray.Add(new CombatStateItem(151, 371, new List<CombatStateProperty>(), -1, 372, 373));
		_dataArray.Add(new CombatStateItem(152, 374, new List<CombatStateProperty>(), -1, 375, 376));
		_dataArray.Add(new CombatStateItem(153, 377, new List<CombatStateProperty>(), -1, 378, 379));
		_dataArray.Add(new CombatStateItem(154, 380, new List<CombatStateProperty>(), -1, 381, 382));
		_dataArray.Add(new CombatStateItem(155, 383, new List<CombatStateProperty>(), -1, 384, 385));
		_dataArray.Add(new CombatStateItem(156, 386, new List<CombatStateProperty>(), -1, 387, 388));
		_dataArray.Add(new CombatStateItem(157, 389, new List<CombatStateProperty>(), -1, 390, 391));
		_dataArray.Add(new CombatStateItem(158, 392, new List<CombatStateProperty>(), -1, 393, 394));
		_dataArray.Add(new CombatStateItem(159, 395, new List<CombatStateProperty>(), -1, 396, 397));
		_dataArray.Add(new CombatStateItem(160, 398, new List<CombatStateProperty>(), -1, 399, 400));
		_dataArray.Add(new CombatStateItem(161, 401, new List<CombatStateProperty>(), -1, 402, 403));
		_dataArray.Add(new CombatStateItem(162, 404, new List<CombatStateProperty>(), -1, 405, 406));
		_dataArray.Add(new CombatStateItem(163, 407, new List<CombatStateProperty>(), -1, 408, 409));
		_dataArray.Add(new CombatStateItem(164, 410, new List<CombatStateProperty>(), -1, 411, 412));
		_dataArray.Add(new CombatStateItem(165, 413, new List<CombatStateProperty>(), -1, 414, 415));
		_dataArray.Add(new CombatStateItem(166, 416, new List<CombatStateProperty>(), 167, 417, 418));
		_dataArray.Add(new CombatStateItem(167, 416, new List<CombatStateProperty>(), 166, 419, 420));
		_dataArray.Add(new CombatStateItem(168, 421, new List<CombatStateProperty>(), -1, 422, 423));
		_dataArray.Add(new CombatStateItem(169, 424, new List<CombatStateProperty>(), -1, 425, 426));
		_dataArray.Add(new CombatStateItem(170, 427, new List<CombatStateProperty>(), -1, 428, 429));
		_dataArray.Add(new CombatStateItem(171, 430, new List<CombatStateProperty>(), -1, 431, 432));
		_dataArray.Add(new CombatStateItem(172, 433, new List<CombatStateProperty>(), -1, 434, 435));
		_dataArray.Add(new CombatStateItem(173, 436, new List<CombatStateProperty>(), -1, 437, 438));
		_dataArray.Add(new CombatStateItem(174, 439, new List<CombatStateProperty>(), -1, 440, 441));
		_dataArray.Add(new CombatStateItem(175, 442, new List<CombatStateProperty>(), -1, 443, 444));
		_dataArray.Add(new CombatStateItem(176, 445, new List<CombatStateProperty>(), -1, 446, 447));
		_dataArray.Add(new CombatStateItem(177, 448, new List<CombatStateProperty>(), -1, 449, 450));
		_dataArray.Add(new CombatStateItem(178, 451, new List<CombatStateProperty>(), -1, 452, 453));
		_dataArray.Add(new CombatStateItem(179, 454, new List<CombatStateProperty>(), -1, 455, 456));
	}

	private void CreateItems3()
	{
		_dataArray.Add(new CombatStateItem(180, 457, new List<CombatStateProperty>(), -1, 458, 459));
		_dataArray.Add(new CombatStateItem(181, 460, new List<CombatStateProperty>(), -1, 461, 462));
		_dataArray.Add(new CombatStateItem(182, 463, new List<CombatStateProperty>(), -1, 464, 465));
		_dataArray.Add(new CombatStateItem(183, 466, new List<CombatStateProperty>(), -1, 467, 468));
		_dataArray.Add(new CombatStateItem(184, 469, new List<CombatStateProperty>(), -1, 470, 471));
		_dataArray.Add(new CombatStateItem(185, 472, new List<CombatStateProperty>(), -1, 473, 474));
		_dataArray.Add(new CombatStateItem(186, 475, new List<CombatStateProperty>(), -1, 476, 477));
		_dataArray.Add(new CombatStateItem(187, 478, new List<CombatStateProperty>(), -1, 479, 480));
		_dataArray.Add(new CombatStateItem(188, 481, new List<CombatStateProperty>(), -1, 482, 483));
		_dataArray.Add(new CombatStateItem(189, 484, new List<CombatStateProperty>(), -1, 485, 486));
		_dataArray.Add(new CombatStateItem(190, 487, new List<CombatStateProperty>(), -1, 488, 489));
		_dataArray.Add(new CombatStateItem(191, 490, new List<CombatStateProperty>(), -1, 491, 492));
		_dataArray.Add(new CombatStateItem(192, 493, new List<CombatStateProperty>(), -1, 494, 495));
		_dataArray.Add(new CombatStateItem(193, 496, new List<CombatStateProperty>(), -1, 497, 498));
		_dataArray.Add(new CombatStateItem(194, 499, new List<CombatStateProperty>(), -1, 500, 501));
		_dataArray.Add(new CombatStateItem(195, 502, new List<CombatStateProperty>(), -1, 503, 504));
		_dataArray.Add(new CombatStateItem(196, 505, new List<CombatStateProperty>(), -1, 506, 507));
		_dataArray.Add(new CombatStateItem(197, 508, new List<CombatStateProperty>(), -1, 509, 510));
		_dataArray.Add(new CombatStateItem(198, 511, new List<CombatStateProperty>(), -1, 512, 513));
		_dataArray.Add(new CombatStateItem(199, 514, new List<CombatStateProperty>(), -1, 515, 516));
		_dataArray.Add(new CombatStateItem(200, 517, new List<CombatStateProperty>(), -1, 518, 519));
		_dataArray.Add(new CombatStateItem(201, 520, new List<CombatStateProperty>(), -1, 521, 522));
		_dataArray.Add(new CombatStateItem(202, 523, new List<CombatStateProperty>(), -1, 524, 525));
		_dataArray.Add(new CombatStateItem(203, 526, new List<CombatStateProperty>(), -1, 527, 528));
		_dataArray.Add(new CombatStateItem(204, 529, new List<CombatStateProperty>(), -1, 530, 531));
		_dataArray.Add(new CombatStateItem(205, 532, new List<CombatStateProperty>(), -1, 533, 534));
		_dataArray.Add(new CombatStateItem(206, 535, new List<CombatStateProperty>(), -1, 536, 537));
		_dataArray.Add(new CombatStateItem(207, 538, new List<CombatStateProperty>(), -1, 539, 540));
		_dataArray.Add(new CombatStateItem(208, 541, new List<CombatStateProperty>
		{
			new CombatStateProperty(34, 2, 0)
		}, -1, 542, 543));
		_dataArray.Add(new CombatStateItem(209, 541, new List<CombatStateProperty>
		{
			new CombatStateProperty(33, 2, 0)
		}, -1, 544, 545));
		_dataArray.Add(new CombatStateItem(210, 546, new List<CombatStateProperty>
		{
			new CombatStateProperty(22, -60, 0)
		}, -1, 547, 548));
		_dataArray.Add(new CombatStateItem(211, 546, new List<CombatStateProperty>
		{
			new CombatStateProperty(22, 30, 0)
		}, -1, 549, 550));
		_dataArray.Add(new CombatStateItem(212, 551, new List<CombatStateProperty>
		{
			new CombatStateProperty(23, -60, 0)
		}, -1, 552, 553));
		_dataArray.Add(new CombatStateItem(213, 551, new List<CombatStateProperty>
		{
			new CombatStateProperty(23, 30, 0)
		}, -1, 554, 555));
		_dataArray.Add(new CombatStateItem(214, 556, new List<CombatStateProperty>
		{
			new CombatStateProperty(24, -60, 0)
		}, -1, 557, 558));
		_dataArray.Add(new CombatStateItem(215, 556, new List<CombatStateProperty>
		{
			new CombatStateProperty(24, 30, 0)
		}, -1, 559, 560));
		_dataArray.Add(new CombatStateItem(216, 561, new List<CombatStateProperty>
		{
			new CombatStateProperty(25, -60, 0)
		}, -1, 562, 563));
		_dataArray.Add(new CombatStateItem(217, 561, new List<CombatStateProperty>
		{
			new CombatStateProperty(25, 30, 0)
		}, -1, 564, 565));
		_dataArray.Add(new CombatStateItem(218, 566, new List<CombatStateProperty>
		{
			new CombatStateProperty(26, -60, 0)
		}, -1, 567, 568));
		_dataArray.Add(new CombatStateItem(219, 566, new List<CombatStateProperty>
		{
			new CombatStateProperty(26, 30, 0)
		}, -1, 569, 570));
		_dataArray.Add(new CombatStateItem(220, 571, new List<CombatStateProperty>
		{
			new CombatStateProperty(27, -60, 0)
		}, -1, 572, 573));
		_dataArray.Add(new CombatStateItem(221, 571, new List<CombatStateProperty>
		{
			new CombatStateProperty(27, 30, 0)
		}, -1, 574, 575));
		_dataArray.Add(new CombatStateItem(222, 576, new List<CombatStateProperty>
		{
			new CombatStateProperty(35, 10, 0)
		}, -1, 577, 578));
		_dataArray.Add(new CombatStateItem(223, 576, new List<CombatStateProperty>
		{
			new CombatStateProperty(35, -10, 0)
		}, -1, 579, 580));
		_dataArray.Add(new CombatStateItem(224, 581, new List<CombatStateProperty>(), -1, 582, 583));
		_dataArray.Add(new CombatStateItem(225, 584, new List<CombatStateProperty>(), -1, 585, 586));
		_dataArray.Add(new CombatStateItem(226, 587, new List<CombatStateProperty>(), -1, 588, 589));
		_dataArray.Add(new CombatStateItem(227, 590, new List<CombatStateProperty>(), -1, 591, 592));
		_dataArray.Add(new CombatStateItem(228, 593, new List<CombatStateProperty>(), -1, 594, 595));
		_dataArray.Add(new CombatStateItem(229, 596, new List<CombatStateProperty>(), -1, 597, 598));
		_dataArray.Add(new CombatStateItem(230, 599, new List<CombatStateProperty>(), -1, 600, 601));
		_dataArray.Add(new CombatStateItem(231, 602, new List<CombatStateProperty>(), -1, 603, 604));
		_dataArray.Add(new CombatStateItem(232, 605, new List<CombatStateProperty>(), -1, 606, 607));
		_dataArray.Add(new CombatStateItem(233, 608, new List<CombatStateProperty>(), -1, 609, 610));
		_dataArray.Add(new CombatStateItem(234, 611, new List<CombatStateProperty>(), -1, 612, 613));
		_dataArray.Add(new CombatStateItem(235, 614, new List<CombatStateProperty>(), -1, 615, 616));
		_dataArray.Add(new CombatStateItem(236, 617, new List<CombatStateProperty>(), -1, 618, 619));
		_dataArray.Add(new CombatStateItem(237, 620, new List<CombatStateProperty>(), -1, 621, 622));
		_dataArray.Add(new CombatStateItem(238, 623, new List<CombatStateProperty>(), -1, 624, 625));
		_dataArray.Add(new CombatStateItem(239, 626, new List<CombatStateProperty>(), -1, 627, 628));
	}

	private void CreateItems4()
	{
		_dataArray.Add(new CombatStateItem(240, 629, new List<CombatStateProperty>(), -1, 630, 631));
		_dataArray.Add(new CombatStateItem(241, 632, new List<CombatStateProperty>(), -1, 633, 634));
		_dataArray.Add(new CombatStateItem(242, 632, new List<CombatStateProperty>(), -1, 635, 636));
		_dataArray.Add(new CombatStateItem(243, 637, new List<CombatStateProperty>(), -1, 638, 639));
		_dataArray.Add(new CombatStateItem(244, 640, new List<CombatStateProperty>(), -1, 641, 642));
		_dataArray.Add(new CombatStateItem(245, 640, new List<CombatStateProperty>(), -1, 643, 644));
	}

	public override void Init()
	{
		base.Init();
		_dataArray = new List<CombatStateItem>(246);
		CreateItems0();
		CreateItems1();
		CreateItems2();
		CreateItems3();
		CreateItems4();
	}
}
