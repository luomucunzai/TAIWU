using System;
using System.Collections.Generic;
using Config.Common;

namespace Config;

[Serializable]
public class RandomEnemy : ConfigData<RandomEnemyItem, short>
{
	public static class DefKey
	{
		public const short SickBeggar = 0;

		public const short EvilBeggar = 1;

		public const short BowlBeggar = 2;

		public const short SnakeBeggar = 3;

		public const short BossBeggar = 4;

		public const short PettyThief = 5;

		public const short CommonThief = 6;

		public const short RapistThief = 7;

		public const short FemaleThief = 8;

		public const short BigThief = 9;

		public const short LocalThug = 10;

		public const short MountainBandit = 11;

		public const short EvilBandit = 12;

		public const short FierceBandit = 13;

		public const short BossBandit = 14;

		public const short ShaolinTraitor = 15;

		public const short EmeiTraitor = 16;

		public const short BaihuaTraitor = 17;

		public const short WudangTraitor = 18;

		public const short YuanshanTraitor = 19;

		public const short ShixiangTraitor = 20;

		public const short RanshanTraitor = 21;

		public const short XuannvTraitor = 22;

		public const short ZhujianTraitor = 23;

		public const short KongsangTraitor = 24;

		public const short JingangTraitor = 25;

		public const short WuxianTraitor = 26;

		public const short JieqingTraitor = 27;

		public const short FulongTraitor = 28;

		public const short XuehouTraitor = 29;

		public const short Desperado = 30;

		public const short RapistMonk = 31;

		public const short DevilishTaoist = 32;

		public const short ManEatingGhost = 33;

		public const short CrimeCulprit = 34;

		public const short FoxyGirl = 35;

		public const short DevilishWoman = 36;

		public const short PoisonousWidow = 37;

		public const short HappyReunionFairy = 38;

		public const short FairFaceQueen = 39;

		public const short GhostServant = 40;

		public const short RefineCorpsePerson = 41;

		public const short CorpseClawPerson = 42;

		public const short GhostDoctor = 43;

		public const short LivingDead = 44;

		public const short DevilishMusician = 45;

		public const short MystifyingChessPlayer = 46;

		public const short BloodCalligrapher = 47;

		public const short BadMonkOldMan = 48;

		public const short IgnorantWiseMan = 49;

		public const short ForeignWeirdo = 50;

		public const short PoisonHand = 51;

		public const short EvilBachelor = 52;

		public const short PureOldBeing = 53;

		public const short EvilWayConjointBoss = 54;

		public const short LoseHeartChivalrousMan = 55;

		public const short EvilSpirit = 56;

		public const short LoseSoulPerson = 57;

		public const short BloodyWarGhost = 58;

		public const short RefineHeartMaster = 59;

		public const short MadDemon = 60;

		public const short FirePondMale = 61;

		public const short ColdPondFemale = 62;

		public const short AviciFolk = 63;

		public const short SoumoulouLord = 64;

		public const short DesperatePerson = 65;

		public const short InHumanFolk = 66;

		public const short NoAppearanceOldMonk = 67;

		public const short NoNameOldTaoist = 68;

		public const short WorldWearyOldPerson = 69;

		public const short XiangshuMinion0 = 70;

		public const short XiangshuMinion1 = 71;

		public const short XiangshuMinion2 = 72;

		public const short XiangshuMinion3 = 73;

		public const short XiangshuMinion4 = 74;

		public const short XiangshuMinion5 = 75;

		public const short XiangshuMinion6 = 76;

		public const short XiangshuMinion7 = 77;

		public const short XiangshuMinion8 = 78;

		public const short Righteous0 = 79;

		public const short Righteous1 = 80;

		public const short Righteous2 = 81;

		public const short Righteous3 = 82;

		public const short Righteous4 = 83;

		public const short Righteous5 = 84;

		public const short Righteous6 = 85;

		public const short Righteous7 = 86;

		public const short Righteous8 = 87;

		public const short ShaolinRetiredSenior = 88;

		public const short ShaolinMember6 = 89;

		public const short ShaolinMember5 = 90;

		public const short ShaolinMember4 = 91;

		public const short ShaolinMember3 = 92;

		public const short ShaolinMember2 = 93;

		public const short ShaolinMember1 = 94;

		public const short ShaolinMember0 = 95;

		public const short EmeiRetiredSenior = 96;

		public const short EmeiMember6 = 97;

		public const short EmeiMember5 = 98;

		public const short EmeiMember4 = 99;

		public const short EmeiMember3 = 100;

		public const short EmeiMember2 = 101;

		public const short EmeiMember1 = 102;

		public const short EmeiMember0 = 103;

		public const short BaihuaRetiredSenior = 104;

		public const short BaihuaMember6 = 105;

		public const short BaihuaMember5 = 106;

		public const short BaihuaMember4 = 107;

		public const short BaihuaMember3 = 108;

		public const short BaihuaMember2 = 109;

		public const short BaihuaMember1 = 110;

		public const short BaihuaMember0 = 111;

		public const short WudangRetiredSenior = 112;

		public const short WudangMember6 = 113;

		public const short WudangMember5 = 114;

		public const short WudangMember4 = 115;

		public const short WudangMember3 = 116;

		public const short WudangMember2 = 117;

		public const short WudangMember1 = 118;

		public const short WudangMember0 = 119;

		public const short YuanshanRetiredSenior = 120;

		public const short YuanshanMember6 = 121;

		public const short YuanshanMember5 = 122;

		public const short YuanshanMember4 = 123;

		public const short YuanshanMember3 = 124;

		public const short YuanshanMember2 = 125;

		public const short YuanshanMember1 = 126;

		public const short YuanshanMember0 = 127;

		public const short ShixiangRetiredSenior = 128;

		public const short ShixiangMember6 = 129;

		public const short ShixiangMember5 = 130;

		public const short ShixiangMember4 = 131;

		public const short ShixiangMember3 = 132;

		public const short ShixiangMember2 = 133;

		public const short ShixiangMember1 = 134;

		public const short ShixiangMember0 = 135;

		public const short RanshanRetiredSenior = 136;

		public const short RanshanMember6 = 137;

		public const short RanshanMember5 = 138;

		public const short RanshanMember4 = 139;

		public const short RanshanMember3 = 140;

		public const short RanshanMember2 = 141;

		public const short RanshanMember1 = 142;

		public const short RanshanMember0 = 143;

		public const short XuannvRetiredSenior = 144;

		public const short XuannvMember6 = 145;

		public const short XuannvMember5 = 146;

		public const short XuannvMember4 = 147;

		public const short XuannvMember3 = 148;

		public const short XuannvMember2 = 149;

		public const short XuannvMember1 = 150;

		public const short XuannvMember0 = 151;

		public const short ZhujianRetiredSenior = 152;

		public const short ZhujianMember6 = 153;

		public const short ZhujianMember5 = 154;

		public const short ZhujianMember4 = 155;

		public const short ZhujianMember3 = 156;

		public const short ZhujianMember2 = 157;

		public const short ZhujianMember1 = 158;

		public const short ZhujianMember0 = 159;

		public const short KongsangRetiredSenior = 160;

		public const short KongsangMember6 = 161;

		public const short KongsangMember5 = 162;

		public const short KongsangMember4 = 163;

		public const short KongsangMember3 = 164;

		public const short KongsangMember2 = 165;

		public const short KongsangMember1 = 166;

		public const short KongsangMember0 = 167;

		public const short JingangRetiredSenior = 168;

		public const short JingangMember6 = 169;

		public const short JingangMember5 = 170;

		public const short JingangMember4 = 171;

		public const short JingangMember3 = 172;

		public const short JingangMember2 = 173;

		public const short JingangMember1 = 174;

		public const short JingangMember0 = 175;

		public const short WuxianRetiredSenior = 176;

		public const short WuxianMember6 = 177;

		public const short WuxianMember5 = 178;

		public const short WuxianMember4 = 179;

		public const short WuxianMember3 = 180;

		public const short WuxianMember2 = 181;

		public const short WuxianMember1 = 182;

		public const short WuxianMember0 = 183;

		public const short JieqingRetiredSenior = 184;

		public const short JieqingMember6 = 185;

		public const short JieqingMember5 = 186;

		public const short JieqingMember4 = 187;

		public const short JieqingMember3 = 188;

		public const short JieqingMember2 = 189;

		public const short JieqingMember1 = 190;

		public const short JieqingMember0 = 191;

		public const short FulongRetiredSenior = 192;

		public const short FulongMember6 = 193;

		public const short FulongMember5 = 194;

		public const short FulongMember4 = 195;

		public const short FulongMember3 = 196;

		public const short FulongMember2 = 197;

		public const short FulongMember1 = 198;

		public const short FulongMember0 = 199;

		public const short XuehouRetiredSenior = 200;

		public const short XuehouMember6 = 201;

		public const short XuehouMember5 = 202;

		public const short XuehouMember4 = 203;

		public const short XuehouMember3 = 204;

		public const short XuehouMember2 = 205;

		public const short XuehouMember1 = 206;

		public const short XuehouMember0 = 207;

		public const short MarriageCuiE = 208;
	}

	public static class DefValue
	{
		public static RandomEnemyItem SickBeggar => Instance[(short)0];

		public static RandomEnemyItem EvilBeggar => Instance[(short)1];

		public static RandomEnemyItem BowlBeggar => Instance[(short)2];

		public static RandomEnemyItem SnakeBeggar => Instance[(short)3];

		public static RandomEnemyItem BossBeggar => Instance[(short)4];

		public static RandomEnemyItem PettyThief => Instance[(short)5];

		public static RandomEnemyItem CommonThief => Instance[(short)6];

		public static RandomEnemyItem RapistThief => Instance[(short)7];

		public static RandomEnemyItem FemaleThief => Instance[(short)8];

		public static RandomEnemyItem BigThief => Instance[(short)9];

		public static RandomEnemyItem LocalThug => Instance[(short)10];

		public static RandomEnemyItem MountainBandit => Instance[(short)11];

		public static RandomEnemyItem EvilBandit => Instance[(short)12];

		public static RandomEnemyItem FierceBandit => Instance[(short)13];

		public static RandomEnemyItem BossBandit => Instance[(short)14];

		public static RandomEnemyItem ShaolinTraitor => Instance[(short)15];

		public static RandomEnemyItem EmeiTraitor => Instance[(short)16];

		public static RandomEnemyItem BaihuaTraitor => Instance[(short)17];

		public static RandomEnemyItem WudangTraitor => Instance[(short)18];

		public static RandomEnemyItem YuanshanTraitor => Instance[(short)19];

		public static RandomEnemyItem ShixiangTraitor => Instance[(short)20];

		public static RandomEnemyItem RanshanTraitor => Instance[(short)21];

		public static RandomEnemyItem XuannvTraitor => Instance[(short)22];

		public static RandomEnemyItem ZhujianTraitor => Instance[(short)23];

		public static RandomEnemyItem KongsangTraitor => Instance[(short)24];

		public static RandomEnemyItem JingangTraitor => Instance[(short)25];

		public static RandomEnemyItem WuxianTraitor => Instance[(short)26];

		public static RandomEnemyItem JieqingTraitor => Instance[(short)27];

		public static RandomEnemyItem FulongTraitor => Instance[(short)28];

		public static RandomEnemyItem XuehouTraitor => Instance[(short)29];

		public static RandomEnemyItem Desperado => Instance[(short)30];

		public static RandomEnemyItem RapistMonk => Instance[(short)31];

		public static RandomEnemyItem DevilishTaoist => Instance[(short)32];

		public static RandomEnemyItem ManEatingGhost => Instance[(short)33];

		public static RandomEnemyItem CrimeCulprit => Instance[(short)34];

		public static RandomEnemyItem FoxyGirl => Instance[(short)35];

		public static RandomEnemyItem DevilishWoman => Instance[(short)36];

		public static RandomEnemyItem PoisonousWidow => Instance[(short)37];

		public static RandomEnemyItem HappyReunionFairy => Instance[(short)38];

		public static RandomEnemyItem FairFaceQueen => Instance[(short)39];

		public static RandomEnemyItem GhostServant => Instance[(short)40];

		public static RandomEnemyItem RefineCorpsePerson => Instance[(short)41];

		public static RandomEnemyItem CorpseClawPerson => Instance[(short)42];

		public static RandomEnemyItem GhostDoctor => Instance[(short)43];

		public static RandomEnemyItem LivingDead => Instance[(short)44];

		public static RandomEnemyItem DevilishMusician => Instance[(short)45];

		public static RandomEnemyItem MystifyingChessPlayer => Instance[(short)46];

		public static RandomEnemyItem BloodCalligrapher => Instance[(short)47];

		public static RandomEnemyItem BadMonkOldMan => Instance[(short)48];

		public static RandomEnemyItem IgnorantWiseMan => Instance[(short)49];

		public static RandomEnemyItem ForeignWeirdo => Instance[(short)50];

		public static RandomEnemyItem PoisonHand => Instance[(short)51];

		public static RandomEnemyItem EvilBachelor => Instance[(short)52];

		public static RandomEnemyItem PureOldBeing => Instance[(short)53];

		public static RandomEnemyItem EvilWayConjointBoss => Instance[(short)54];

		public static RandomEnemyItem LoseHeartChivalrousMan => Instance[(short)55];

		public static RandomEnemyItem EvilSpirit => Instance[(short)56];

		public static RandomEnemyItem LoseSoulPerson => Instance[(short)57];

		public static RandomEnemyItem BloodyWarGhost => Instance[(short)58];

		public static RandomEnemyItem RefineHeartMaster => Instance[(short)59];

		public static RandomEnemyItem MadDemon => Instance[(short)60];

		public static RandomEnemyItem FirePondMale => Instance[(short)61];

		public static RandomEnemyItem ColdPondFemale => Instance[(short)62];

		public static RandomEnemyItem AviciFolk => Instance[(short)63];

		public static RandomEnemyItem SoumoulouLord => Instance[(short)64];

		public static RandomEnemyItem DesperatePerson => Instance[(short)65];

		public static RandomEnemyItem InHumanFolk => Instance[(short)66];

		public static RandomEnemyItem NoAppearanceOldMonk => Instance[(short)67];

		public static RandomEnemyItem NoNameOldTaoist => Instance[(short)68];

		public static RandomEnemyItem WorldWearyOldPerson => Instance[(short)69];

		public static RandomEnemyItem XiangshuMinion0 => Instance[(short)70];

		public static RandomEnemyItem XiangshuMinion1 => Instance[(short)71];

		public static RandomEnemyItem XiangshuMinion2 => Instance[(short)72];

		public static RandomEnemyItem XiangshuMinion3 => Instance[(short)73];

		public static RandomEnemyItem XiangshuMinion4 => Instance[(short)74];

		public static RandomEnemyItem XiangshuMinion5 => Instance[(short)75];

		public static RandomEnemyItem XiangshuMinion6 => Instance[(short)76];

		public static RandomEnemyItem XiangshuMinion7 => Instance[(short)77];

		public static RandomEnemyItem XiangshuMinion8 => Instance[(short)78];

		public static RandomEnemyItem Righteous0 => Instance[(short)79];

		public static RandomEnemyItem Righteous1 => Instance[(short)80];

		public static RandomEnemyItem Righteous2 => Instance[(short)81];

		public static RandomEnemyItem Righteous3 => Instance[(short)82];

		public static RandomEnemyItem Righteous4 => Instance[(short)83];

		public static RandomEnemyItem Righteous5 => Instance[(short)84];

		public static RandomEnemyItem Righteous6 => Instance[(short)85];

		public static RandomEnemyItem Righteous7 => Instance[(short)86];

		public static RandomEnemyItem Righteous8 => Instance[(short)87];

		public static RandomEnemyItem ShaolinRetiredSenior => Instance[(short)88];

		public static RandomEnemyItem ShaolinMember6 => Instance[(short)89];

		public static RandomEnemyItem ShaolinMember5 => Instance[(short)90];

		public static RandomEnemyItem ShaolinMember4 => Instance[(short)91];

		public static RandomEnemyItem ShaolinMember3 => Instance[(short)92];

		public static RandomEnemyItem ShaolinMember2 => Instance[(short)93];

		public static RandomEnemyItem ShaolinMember1 => Instance[(short)94];

		public static RandomEnemyItem ShaolinMember0 => Instance[(short)95];

		public static RandomEnemyItem EmeiRetiredSenior => Instance[(short)96];

		public static RandomEnemyItem EmeiMember6 => Instance[(short)97];

		public static RandomEnemyItem EmeiMember5 => Instance[(short)98];

		public static RandomEnemyItem EmeiMember4 => Instance[(short)99];

		public static RandomEnemyItem EmeiMember3 => Instance[(short)100];

		public static RandomEnemyItem EmeiMember2 => Instance[(short)101];

		public static RandomEnemyItem EmeiMember1 => Instance[(short)102];

		public static RandomEnemyItem EmeiMember0 => Instance[(short)103];

		public static RandomEnemyItem BaihuaRetiredSenior => Instance[(short)104];

		public static RandomEnemyItem BaihuaMember6 => Instance[(short)105];

		public static RandomEnemyItem BaihuaMember5 => Instance[(short)106];

		public static RandomEnemyItem BaihuaMember4 => Instance[(short)107];

		public static RandomEnemyItem BaihuaMember3 => Instance[(short)108];

		public static RandomEnemyItem BaihuaMember2 => Instance[(short)109];

		public static RandomEnemyItem BaihuaMember1 => Instance[(short)110];

		public static RandomEnemyItem BaihuaMember0 => Instance[(short)111];

		public static RandomEnemyItem WudangRetiredSenior => Instance[(short)112];

		public static RandomEnemyItem WudangMember6 => Instance[(short)113];

		public static RandomEnemyItem WudangMember5 => Instance[(short)114];

		public static RandomEnemyItem WudangMember4 => Instance[(short)115];

		public static RandomEnemyItem WudangMember3 => Instance[(short)116];

		public static RandomEnemyItem WudangMember2 => Instance[(short)117];

		public static RandomEnemyItem WudangMember1 => Instance[(short)118];

		public static RandomEnemyItem WudangMember0 => Instance[(short)119];

		public static RandomEnemyItem YuanshanRetiredSenior => Instance[(short)120];

		public static RandomEnemyItem YuanshanMember6 => Instance[(short)121];

		public static RandomEnemyItem YuanshanMember5 => Instance[(short)122];

		public static RandomEnemyItem YuanshanMember4 => Instance[(short)123];

		public static RandomEnemyItem YuanshanMember3 => Instance[(short)124];

		public static RandomEnemyItem YuanshanMember2 => Instance[(short)125];

		public static RandomEnemyItem YuanshanMember1 => Instance[(short)126];

		public static RandomEnemyItem YuanshanMember0 => Instance[(short)127];

		public static RandomEnemyItem ShixiangRetiredSenior => Instance[(short)128];

		public static RandomEnemyItem ShixiangMember6 => Instance[(short)129];

		public static RandomEnemyItem ShixiangMember5 => Instance[(short)130];

		public static RandomEnemyItem ShixiangMember4 => Instance[(short)131];

		public static RandomEnemyItem ShixiangMember3 => Instance[(short)132];

		public static RandomEnemyItem ShixiangMember2 => Instance[(short)133];

		public static RandomEnemyItem ShixiangMember1 => Instance[(short)134];

		public static RandomEnemyItem ShixiangMember0 => Instance[(short)135];

		public static RandomEnemyItem RanshanRetiredSenior => Instance[(short)136];

		public static RandomEnemyItem RanshanMember6 => Instance[(short)137];

		public static RandomEnemyItem RanshanMember5 => Instance[(short)138];

		public static RandomEnemyItem RanshanMember4 => Instance[(short)139];

		public static RandomEnemyItem RanshanMember3 => Instance[(short)140];

		public static RandomEnemyItem RanshanMember2 => Instance[(short)141];

		public static RandomEnemyItem RanshanMember1 => Instance[(short)142];

		public static RandomEnemyItem RanshanMember0 => Instance[(short)143];

		public static RandomEnemyItem XuannvRetiredSenior => Instance[(short)144];

		public static RandomEnemyItem XuannvMember6 => Instance[(short)145];

		public static RandomEnemyItem XuannvMember5 => Instance[(short)146];

		public static RandomEnemyItem XuannvMember4 => Instance[(short)147];

		public static RandomEnemyItem XuannvMember3 => Instance[(short)148];

		public static RandomEnemyItem XuannvMember2 => Instance[(short)149];

		public static RandomEnemyItem XuannvMember1 => Instance[(short)150];

		public static RandomEnemyItem XuannvMember0 => Instance[(short)151];

		public static RandomEnemyItem ZhujianRetiredSenior => Instance[(short)152];

		public static RandomEnemyItem ZhujianMember6 => Instance[(short)153];

		public static RandomEnemyItem ZhujianMember5 => Instance[(short)154];

		public static RandomEnemyItem ZhujianMember4 => Instance[(short)155];

		public static RandomEnemyItem ZhujianMember3 => Instance[(short)156];

		public static RandomEnemyItem ZhujianMember2 => Instance[(short)157];

		public static RandomEnemyItem ZhujianMember1 => Instance[(short)158];

		public static RandomEnemyItem ZhujianMember0 => Instance[(short)159];

		public static RandomEnemyItem KongsangRetiredSenior => Instance[(short)160];

		public static RandomEnemyItem KongsangMember6 => Instance[(short)161];

		public static RandomEnemyItem KongsangMember5 => Instance[(short)162];

		public static RandomEnemyItem KongsangMember4 => Instance[(short)163];

		public static RandomEnemyItem KongsangMember3 => Instance[(short)164];

		public static RandomEnemyItem KongsangMember2 => Instance[(short)165];

		public static RandomEnemyItem KongsangMember1 => Instance[(short)166];

		public static RandomEnemyItem KongsangMember0 => Instance[(short)167];

		public static RandomEnemyItem JingangRetiredSenior => Instance[(short)168];

		public static RandomEnemyItem JingangMember6 => Instance[(short)169];

		public static RandomEnemyItem JingangMember5 => Instance[(short)170];

		public static RandomEnemyItem JingangMember4 => Instance[(short)171];

		public static RandomEnemyItem JingangMember3 => Instance[(short)172];

		public static RandomEnemyItem JingangMember2 => Instance[(short)173];

		public static RandomEnemyItem JingangMember1 => Instance[(short)174];

		public static RandomEnemyItem JingangMember0 => Instance[(short)175];

		public static RandomEnemyItem WuxianRetiredSenior => Instance[(short)176];

		public static RandomEnemyItem WuxianMember6 => Instance[(short)177];

		public static RandomEnemyItem WuxianMember5 => Instance[(short)178];

		public static RandomEnemyItem WuxianMember4 => Instance[(short)179];

		public static RandomEnemyItem WuxianMember3 => Instance[(short)180];

		public static RandomEnemyItem WuxianMember2 => Instance[(short)181];

		public static RandomEnemyItem WuxianMember1 => Instance[(short)182];

		public static RandomEnemyItem WuxianMember0 => Instance[(short)183];

		public static RandomEnemyItem JieqingRetiredSenior => Instance[(short)184];

		public static RandomEnemyItem JieqingMember6 => Instance[(short)185];

		public static RandomEnemyItem JieqingMember5 => Instance[(short)186];

		public static RandomEnemyItem JieqingMember4 => Instance[(short)187];

		public static RandomEnemyItem JieqingMember3 => Instance[(short)188];

		public static RandomEnemyItem JieqingMember2 => Instance[(short)189];

		public static RandomEnemyItem JieqingMember1 => Instance[(short)190];

		public static RandomEnemyItem JieqingMember0 => Instance[(short)191];

		public static RandomEnemyItem FulongRetiredSenior => Instance[(short)192];

		public static RandomEnemyItem FulongMember6 => Instance[(short)193];

		public static RandomEnemyItem FulongMember5 => Instance[(short)194];

		public static RandomEnemyItem FulongMember4 => Instance[(short)195];

		public static RandomEnemyItem FulongMember3 => Instance[(short)196];

		public static RandomEnemyItem FulongMember2 => Instance[(short)197];

		public static RandomEnemyItem FulongMember1 => Instance[(short)198];

		public static RandomEnemyItem FulongMember0 => Instance[(short)199];

		public static RandomEnemyItem XuehouRetiredSenior => Instance[(short)200];

		public static RandomEnemyItem XuehouMember6 => Instance[(short)201];

		public static RandomEnemyItem XuehouMember5 => Instance[(short)202];

		public static RandomEnemyItem XuehouMember4 => Instance[(short)203];

		public static RandomEnemyItem XuehouMember3 => Instance[(short)204];

		public static RandomEnemyItem XuehouMember2 => Instance[(short)205];

		public static RandomEnemyItem XuehouMember1 => Instance[(short)206];

		public static RandomEnemyItem XuehouMember0 => Instance[(short)207];

		public static RandomEnemyItem MarriageCuiE => Instance[(short)208];
	}

	public static RandomEnemy Instance = new RandomEnemy();

	private readonly HashSet<string> RequiredFields = new HashSet<string> { "SectIds", "PoisonsToAdd", "TemplateId", "PracticeRandomRange" };

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
		_dataArray.Add(new RandomEnemyItem(0, new List<short> { 5, 14, 15 }, 1, 0, -40, -40, 2, 4, 1, new short[6] { 0, 9, 18, 27, 36, 45 }, (30, 100), new(int, int)[2]
		{
			(1, 2),
			(1, 2)
		}));
		_dataArray.Add(new RandomEnemyItem(1, new List<short> { 5, 14, 15 }, 1, 0, -40, -40, 2, 4, 1, new short[6] { 0, 9, 18, 27, 36, 45 }, (30, 100), new(int, int)[2]
		{
			(1, 2),
			(1, 2)
		}));
		_dataArray.Add(new RandomEnemyItem(2, new List<short> { 5, 14, 15 }, 1, 0, -40, -40, 2, 4, 1, new short[6] { 0, 9, 18, 27, 36, 45 }, (30, 100), new(int, int)[2]
		{
			(1, 2),
			(1, 2)
		}));
		_dataArray.Add(new RandomEnemyItem(3, new List<short> { 5, 14, 15 }, 1, 0, -40, -40, 2, 4, 1, new short[6] { 0, 9, 18, 27, 36, 45 }, (30, 100), new(int, int)[2]
		{
			(1, 2),
			(1, 2)
		}));
		_dataArray.Add(new RandomEnemyItem(4, new List<short> { 5, 14, 15 }, 2, 1, -40, -40, 10, 12, 1, new short[6] { 0, 9, 18, 27, 36, 45 }, (100, 100), new(int, int)[2]
		{
			(3, 5),
			(3, 5)
		}));
		_dataArray.Add(new RandomEnemyItem(5, new List<short> { 2, 8, 13 }, 1, 0, -20, -20, 4, 0, 0, new short[0], (35, 110), new(int, int)[2]
		{
			(1, 3),
			(1, 3)
		}));
		_dataArray.Add(new RandomEnemyItem(6, new List<short> { 2, 8, 13 }, 1, 0, -20, -20, 4, 0, 0, new short[0], (35, 110), new(int, int)[2]
		{
			(1, 3),
			(1, 3)
		}));
		_dataArray.Add(new RandomEnemyItem(7, new List<short> { 2, 8, 13 }, 1, 1, -20, -20, 4, 0, 0, new short[0], (35, 110), new(int, int)[2]
		{
			(1, 3),
			(1, 3)
		}));
		_dataArray.Add(new RandomEnemyItem(8, new List<short> { 2, 8, 13 }, 1, 1, -20, -20, 4, 0, 0, new short[0], (35, 110), new(int, int)[2]
		{
			(1, 3),
			(1, 3)
		}));
		_dataArray.Add(new RandomEnemyItem(9, new List<short> { 2, 8, 13 }, 2, 2, -20, -20, 20, 0, 0, new short[0], (100, 100), new(int, int)[2]
		{
			(3, 5),
			(3, 5)
		}));
		_dataArray.Add(new RandomEnemyItem(10, new List<short> { 1, 6, 11 }, 1, 1, 0, 0, 6, 0, 0, new short[0], (40, 120), new(int, int)[2]
		{
			(1, 4),
			(1, 4)
		}));
		_dataArray.Add(new RandomEnemyItem(11, new List<short> { 1, 6, 11 }, 1, 1, 0, 0, 6, 0, 0, new short[0], (40, 120), new(int, int)[2]
		{
			(1, 4),
			(1, 4)
		}));
		_dataArray.Add(new RandomEnemyItem(12, new List<short> { 1, 6, 11 }, 1, 1, 0, 0, 6, 0, 0, new short[0], (40, 120), new(int, int)[2]
		{
			(1, 4),
			(1, 4)
		}));
		_dataArray.Add(new RandomEnemyItem(13, new List<short> { 1, 6, 11 }, 1, 2, 0, 0, 6, 0, 0, new short[0], (40, 120), new(int, int)[2]
		{
			(1, 4),
			(1, 4)
		}));
		_dataArray.Add(new RandomEnemyItem(14, new List<short> { 1, 6, 11 }, 2, 3, 0, 0, 30, 0, 0, new short[0], (100, 100), new(int, int)[2]
		{
			(3, 5),
			(3, 5)
		}));
		_dataArray.Add(new RandomEnemyItem(15, new List<short> { 1 }, 1, 3, 0, 0, 8, 0, 0, new short[0], (45, 130), new(int, int)[2]
		{
			(2, 4),
			(2, 4)
		}));
		_dataArray.Add(new RandomEnemyItem(16, new List<short> { 2 }, 1, 3, 0, 0, 8, 0, 0, new short[0], (45, 130), new(int, int)[2]
		{
			(2, 4),
			(2, 4)
		}));
		_dataArray.Add(new RandomEnemyItem(17, new List<short> { 3 }, 1, 3, 0, 0, 8, 20, 2, new short[3] { 11, 20, 47 }, (45, 130), new(int, int)[2]
		{
			(2, 4),
			(2, 4)
		}));
		_dataArray.Add(new RandomEnemyItem(18, new List<short> { 4 }, 1, 3, 0, 0, 8, 0, 0, new short[0], (45, 130), new(int, int)[2]
		{
			(2, 4),
			(2, 4)
		}));
		_dataArray.Add(new RandomEnemyItem(19, new List<short> { 5 }, 1, 3, 0, 0, 8, 0, 0, new short[0], (45, 130), new(int, int)[2]
		{
			(2, 4),
			(2, 4)
		}));
		_dataArray.Add(new RandomEnemyItem(20, new List<short> { 6 }, 1, 3, 0, 0, 8, 0, 0, new short[0], (45, 130), new(int, int)[2]
		{
			(2, 4),
			(2, 4)
		}));
		_dataArray.Add(new RandomEnemyItem(21, new List<short> { 7 }, 1, 3, 0, 0, 8, 0, 0, new short[0], (45, 130), new(int, int)[2]
		{
			(2, 4),
			(2, 4)
		}));
		_dataArray.Add(new RandomEnemyItem(22, new List<short> { 8 }, 1, 3, 0, 0, 8, 0, 0, new short[0], (45, 130), new(int, int)[2]
		{
			(2, 4),
			(2, 4)
		}));
		_dataArray.Add(new RandomEnemyItem(23, new List<short> { 9 }, 1, 3, 0, 0, 8, 0, 0, new short[0], (45, 130), new(int, int)[2]
		{
			(2, 4),
			(2, 4)
		}));
		_dataArray.Add(new RandomEnemyItem(24, new List<short> { 10 }, 1, 3, 0, 0, 8, 28, 2, new short[6] { 2, 11, 20, 29, 38, 47 }, (45, 130), new(int, int)[2]
		{
			(2, 4),
			(2, 4)
		}));
		_dataArray.Add(new RandomEnemyItem(25, new List<short> { 11 }, 1, 3, 0, 0, 8, 12, 1, new short[3] { 1, 28, 37 }, (45, 130), new(int, int)[2]
		{
			(2, 4),
			(2, 4)
		}));
		_dataArray.Add(new RandomEnemyItem(26, new List<short> { 12 }, 1, 3, 0, 0, 8, 36, 2, new short[6] { 2, 11, 20, 29, 38, 47 }, (45, 130), new(int, int)[2]
		{
			(2, 4),
			(2, 4)
		}));
		_dataArray.Add(new RandomEnemyItem(27, new List<short> { 13 }, 1, 3, 0, 0, 8, 12, 1, new short[3] { 10, 19, 46 }, (45, 130), new(int, int)[2]
		{
			(2, 4),
			(2, 4)
		}));
		_dataArray.Add(new RandomEnemyItem(28, new List<short> { 14 }, 1, 3, 0, 0, 8, 0, 0, new short[0], (45, 130), new(int, int)[2]
		{
			(2, 4),
			(2, 4)
		}));
		_dataArray.Add(new RandomEnemyItem(29, new List<short> { 15 }, 1, 3, 0, 0, 8, 12, 1, new short[3] { 1, 28, 37 }, (45, 130), new(int, int)[2]
		{
			(2, 4),
			(2, 4)
		}));
		_dataArray.Add(new RandomEnemyItem(30, new List<short> { 3, 8, 13 }, 1, 2, -20, 30, 10, 0, 0, new short[0], (50, 140), new(int, int)[2]
		{
			(2, 5),
			(2, 5)
		}));
		_dataArray.Add(new RandomEnemyItem(31, new List<short> { 1, 6, 11 }, 1, 2, -20, 30, 10, 0, 0, new short[0], (50, 140), new(int, int)[2]
		{
			(2, 5),
			(2, 5)
		}));
		_dataArray.Add(new RandomEnemyItem(32, new List<short> { 2, 7, 12 }, 1, 2, -20, 30, 10, 0, 0, new short[0], (50, 140), new(int, int)[2]
		{
			(2, 5),
			(2, 5)
		}));
		_dataArray.Add(new RandomEnemyItem(33, new List<short> { 5, 10, 15 }, 1, 3, -20, 30, 10, 0, 0, new short[0], (50, 140), new(int, int)[2]
		{
			(2, 5),
			(2, 5)
		}));
		_dataArray.Add(new RandomEnemyItem(34, new List<short> { 4, 9, 14 }, 2, 4, -20, 30, 50, 0, 0, new short[0], (100, 100), new(int, int)[2]
		{
			(3, 5),
			(3, 5)
		}));
		_dataArray.Add(new RandomEnemyItem(35, new List<short> { 2, 3, 7, 8, 12, 13 }, 2, 2, 30, -20, 12, 8, 2, new short[3] { 11, 20, 47 }, (55, 150), new(int, int)[2]
		{
			(2, 6),
			(2, 6)
		}));
		_dataArray.Add(new RandomEnemyItem(36, new List<short> { 2, 3, 7, 8, 12, 13 }, 2, 2, 30, -20, 12, 8, 2, new short[3] { 11, 20, 47 }, (55, 150), new(int, int)[2]
		{
			(2, 6),
			(2, 6)
		}));
		_dataArray.Add(new RandomEnemyItem(37, new List<short> { 2, 3, 7, 8, 12, 13 }, 2, 3, 30, -20, 12, 8, 2, new short[3] { 11, 20, 47 }, (55, 150), new(int, int)[2]
		{
			(2, 6),
			(2, 6)
		}));
		_dataArray.Add(new RandomEnemyItem(38, new List<short> { 2, 3, 7, 8, 12, 13 }, 2, 3, 30, -20, 12, 8, 2, new short[3] { 11, 20, 47 }, (55, 150), new(int, int)[2]
		{
			(2, 6),
			(2, 6)
		}));
		_dataArray.Add(new RandomEnemyItem(39, new List<short> { 2, 3, 7, 8, 12, 13 }, 3, 4, 30, -20, 60, 24, 3, new short[3] { 11, 20, 47 }, (100, 100), new(int, int)[2]
		{
			(3, 5),
			(3, 5)
		}));
		_dataArray.Add(new RandomEnemyItem(40, new List<short> { 4, 5, 9, 10, 14, 15 }, 2, 3, 0, 0, 14, 12, 2, new short[6] { 3, 12, 21, 30, 39, 48 }, (60, 160), new(int, int)[2]
		{
			(2, 7),
			(2, 7)
		}));
		_dataArray.Add(new RandomEnemyItem(41, new List<short> { 4, 5, 9, 10, 14, 15 }, 2, 3, 0, 0, 14, 12, 2, new short[6] { 3, 12, 21, 30, 39, 48 }, (60, 160), new(int, int)[2]
		{
			(2, 7),
			(2, 7)
		}));
		_dataArray.Add(new RandomEnemyItem(42, new List<short> { 4, 5, 9, 10, 14, 15 }, 2, 3, 0, 0, 14, 12, 2, new short[6] { 3, 12, 21, 30, 39, 48 }, (60, 160), new(int, int)[2]
		{
			(2, 7),
			(2, 7)
		}));
		_dataArray.Add(new RandomEnemyItem(43, new List<short> { 4, 5, 9, 10, 14, 15 }, 2, 4, 0, 0, 14, 12, 2, new short[6] { 3, 12, 21, 30, 39, 48 }, (60, 160), new(int, int)[2]
		{
			(2, 7),
			(2, 7)
		}));
		_dataArray.Add(new RandomEnemyItem(44, new List<short> { 4, 5, 9, 10, 14, 15 }, 3, 5, 0, 0, 70, 36, 3, new short[6] { 3, 12, 21, 30, 39, 48 }, (100, 100), new(int, int)[2]
		{
			(3, 5),
			(3, 5)
		}));
		_dataArray.Add(new RandomEnemyItem(45, new List<short> { 3, 8, 13 }, 2, 3, 40, 0, 16, 0, 0, new short[0], (65, 170), new(int, int)[2]
		{
			(2, 8),
			(2, 8)
		}));
		_dataArray.Add(new RandomEnemyItem(46, new List<short> { 4, 14, 9 }, 2, 3, 40, 0, 16, 0, 0, new short[0], (65, 170), new(int, int)[2]
		{
			(2, 8),
			(2, 8)
		}));
		_dataArray.Add(new RandomEnemyItem(47, new List<short> { 5, 10, 15 }, 2, 4, 40, 0, 16, 0, 0, new short[0], (65, 170), new(int, int)[2]
		{
			(2, 8),
			(2, 8)
		}));
		_dataArray.Add(new RandomEnemyItem(48, new List<short> { 1, 6, 11 }, 2, 4, 40, 0, 16, 0, 0, new short[0], (65, 170), new(int, int)[2]
		{
			(2, 8),
			(2, 8)
		}));
		_dataArray.Add(new RandomEnemyItem(49, new List<short> { 2, 7, 12 }, 2, 5, 40, 0, 80, 0, 0, new short[0], (100, 100), new(int, int)[2]
		{
			(3, 5),
			(3, 5)
		}));
		_dataArray.Add(new RandomEnemyItem(50, new List<short> { 3, 10, 12 }, 2, 4, 20, 20, 18, 24, 3, new short[6] { 4, 13, 22, 31, 40, 49 }, (70, 180), new(int, int)[2]
		{
			(2, 9),
			(2, 9)
		}));
		_dataArray.Add(new RandomEnemyItem(51, new List<short> { 3, 10, 12 }, 2, 4, 20, 20, 18, 24, 3, new short[6] { 4, 13, 22, 31, 40, 49 }, (70, 180), new(int, int)[2]
		{
			(2, 9),
			(2, 9)
		}));
		_dataArray.Add(new RandomEnemyItem(52, new List<short> { 3, 10, 12 }, 2, 4, 20, 20, 18, 24, 3, new short[6] { 4, 13, 22, 31, 40, 49 }, (70, 180), new(int, int)[2]
		{
			(2, 9),
			(2, 9)
		}));
		_dataArray.Add(new RandomEnemyItem(53, new List<short> { 3, 10, 12 }, 2, 5, 20, 20, 18, 24, 3, new short[6] { 4, 13, 22, 31, 40, 49 }, (70, 180), new(int, int)[2]
		{
			(2, 9),
			(2, 9)
		}));
		_dataArray.Add(new RandomEnemyItem(54, new List<short> { 3, 10, 12 }, 3, 6, 20, 20, 90, 72, 3, new short[6] { 4, 13, 22, 31, 40, 49 }, (100, 100), new(int, int)[2]
		{
			(3, 5),
			(3, 5)
		}));
		_dataArray.Add(new RandomEnemyItem(55, new List<short> { 1, 2, 3, 4, 5 }, 2, 4, 0, 40, 20, 0, 0, new short[0], (75, 190), new(int, int)[2]
		{
			(2, 10),
			(2, 10)
		}));
		_dataArray.Add(new RandomEnemyItem(56, new List<short> { 1, 2, 3, 4, 5 }, 2, 4, 0, 40, 20, 0, 0, new short[0], (75, 190), new(int, int)[2]
		{
			(2, 10),
			(2, 10)
		}));
		_dataArray.Add(new RandomEnemyItem(57, new List<short> { 1, 2, 3, 4, 5 }, 2, 5, 0, 40, 20, 0, 0, new short[0], (75, 190), new(int, int)[2]
		{
			(2, 10),
			(2, 10)
		}));
		_dataArray.Add(new RandomEnemyItem(58, new List<short> { 1, 2, 3, 4, 5 }, 2, 5, 0, 40, 20, 0, 0, new short[0], (75, 190), new(int, int)[2]
		{
			(2, 10),
			(2, 10)
		}));
		_dataArray.Add(new RandomEnemyItem(59, new List<short> { 11, 12, 13, 14, 15 }, 3, 6, 0, 40, 100, 0, 0, new short[0], (100, 100), new(int, int)[2]
		{
			(3, 5),
			(3, 5)
		}));
	}

	private void CreateItems1()
	{
		_dataArray.Add(new RandomEnemyItem(60, new List<short> { 1, 3, 6, 8, 11, 13 }, 3, 5, 30, 30, 22, 16, 3, new short[6] { 5, 14, 23, 32, 41, 50 }, (80, 200), new(int, int)[2]
		{
			(3, 5),
			(3, 5)
		}));
		_dataArray.Add(new RandomEnemyItem(61, new List<short> { 4, 5, 9, 10, 14, 15 }, 3, 5, 30, 30, 22, 16, 3, new short[6] { 5, 14, 23, 32, 41, 50 }, (80, 200), new(int, int)[2]
		{
			(3, 5),
			(3, 5)
		}));
		_dataArray.Add(new RandomEnemyItem(62, new List<short> { 2, 3, 7, 8, 12, 13 }, 3, 5, 30, 30, 22, 16, 3, new short[6] { 5, 14, 23, 32, 41, 50 }, (80, 200), new(int, int)[2]
		{
			(3, 5),
			(3, 5)
		}));
		_dataArray.Add(new RandomEnemyItem(63, new List<short> { 2, 4, 7, 9, 12, 14 }, 3, 6, 30, 30, 22, 16, 3, new short[6] { 5, 14, 23, 32, 41, 50 }, (80, 200), new(int, int)[2]
		{
			(3, 5),
			(3, 5)
		}));
		_dataArray.Add(new RandomEnemyItem(64, new List<short> { 1, 5, 6, 10, 11, 15 }, 4, 7, 30, 30, 110, 48, 3, new short[6] { 5, 14, 23, 32, 41, 50 }, (100, 100), new(int, int)[2]
		{
			(3, 5),
			(3, 5)
		}));
		_dataArray.Add(new RandomEnemyItem(65, new List<short>
		{
			1, 2, 3, 4, 5, 6, 7, 8, 9, 10,
			11, 12, 13, 14, 15
		}, 3, 5, 30, 30, 24, 0, 0, new short[0], (85, 210), new(int, int)[2]
		{
			(3, 5),
			(3, 5)
		}));
		_dataArray.Add(new RandomEnemyItem(66, new List<short>
		{
			1, 2, 3, 4, 5, 6, 7, 8, 9, 10,
			11, 12, 13, 14, 15
		}, 3, 5, 30, 30, 24, 0, 0, new short[0], (85, 210), new(int, int)[2]
		{
			(3, 5),
			(3, 5)
		}));
		_dataArray.Add(new RandomEnemyItem(67, new List<short>
		{
			1, 2, 3, 4, 5, 6, 7, 8, 9, 10,
			11, 12, 13, 14, 15
		}, 3, 6, 30, 30, 24, 0, 0, new short[0], (85, 210), new(int, int)[2]
		{
			(3, 5),
			(3, 5)
		}));
		_dataArray.Add(new RandomEnemyItem(68, new List<short>
		{
			1, 2, 3, 4, 5, 6, 7, 8, 9, 10,
			11, 12, 13, 14, 15
		}, 3, 6, 30, 30, 24, 0, 0, new short[0], (85, 210), new(int, int)[2]
		{
			(3, 5),
			(3, 5)
		}));
		_dataArray.Add(new RandomEnemyItem(69, new List<short>
		{
			1, 2, 3, 4, 5, 6, 7, 8, 9, 10,
			11, 12, 13, 14, 15
		}, 4, 7, 30, 30, 120, 0, 0, new short[0], (100, 100), new(int, int)[2]
		{
			(3, 5),
			(3, 5)
		}));
		_dataArray.Add(new RandomEnemyItem(70, new List<short> { 6, 7, 8, 9, 10, 11, 12, 13, 14, 15 }, 1, 1, -20, -20, 0, 4, 1, new short[6] { 0, 9, 18, 27, 36, 45 }, (60, 140), new(int, int)[2]
		{
			(0, 0),
			(2, 6)
		}));
		_dataArray.Add(new RandomEnemyItem(71, new List<short> { 6, 7, 8, 9, 10, 11, 12, 13, 14, 15 }, 1, 1, -10, -10, 0, 8, 1, new short[6] { 0, 9, 18, 27, 36, 45 }, (65, 150), new(int, int)[2]
		{
			(0, 0),
			(2, 6)
		}));
		_dataArray.Add(new RandomEnemyItem(72, new List<short> { 6, 7, 8, 9, 10, 11, 12, 13, 14, 15 }, 1, 2, 0, 0, 0, 12, 1, new short[6] { 1, 10, 19, 28, 37, 46 }, (70, 160), new(int, int)[2]
		{
			(0, 0),
			(3, 9)
		}));
		_dataArray.Add(new RandomEnemyItem(73, new List<short> { 6, 7, 8, 9, 10, 11, 12, 13, 14, 15 }, 2, 3, 10, 10, 0, 16, 2, new short[6] { 2, 11, 20, 29, 38, 47 }, (75, 170), new(int, int)[2]
		{
			(0, 0),
			(3, 9)
		}));
		_dataArray.Add(new RandomEnemyItem(74, new List<short> { 6, 7, 8, 9, 10, 11, 12, 13, 14, 15 }, 2, 4, 20, 20, 0, 20, 2, new short[6] { 2, 11, 20, 29, 38, 47 }, (80, 180), new(int, int)[2]
		{
			(0, 0),
			(4, 12)
		}));
		_dataArray.Add(new RandomEnemyItem(75, new List<short> { 6, 7, 8, 9, 10, 11, 12, 13, 14, 15 }, 3, 5, 30, 30, 0, 24, 2, new short[6] { 3, 12, 21, 30, 39, 48 }, (85, 190), new(int, int)[2]
		{
			(0, 0),
			(4, 12)
		}));
		_dataArray.Add(new RandomEnemyItem(76, new List<short> { 6, 7, 8, 9, 10, 11, 12, 13, 14, 15 }, 3, 6, 40, 40, 0, 28, 3, new short[6] { 4, 13, 22, 31, 40, 49 }, (90, 200), new(int, int)[2]
		{
			(0, 0),
			(5, 5)
		}));
		_dataArray.Add(new RandomEnemyItem(77, new List<short> { 6, 7, 8, 9, 10, 11, 12, 13, 14, 15 }, 4, 6, 50, 50, 0, 32, 3, new short[6] { 5, 14, 23, 32, 41, 50 }, (95, 210), new(int, int)[2]
		{
			(0, 0),
			(5, 5)
		}));
		_dataArray.Add(new RandomEnemyItem(78, new List<short> { 6, 7, 8, 9, 10, 11, 12, 13, 14, 15 }, 4, 7, 60, 60, 0, 36, 3, new short[6] { 6, 15, 24, 33, 42, 51 }, (100, 100), new(int, int)[2]
		{
			(0, 0),
			(5, 5)
		}));
		_dataArray.Add(new RandomEnemyItem(79, new List<short> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 }, 1, 1, 0, 0, -20, 0, 0, new short[0], (60, 140), new(int, int)[2]
		{
			(2, 6),
			(0, 0)
		}));
		_dataArray.Add(new RandomEnemyItem(80, new List<short> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 }, 1, 1, 5, 5, -30, 0, 0, new short[0], (65, 150), new(int, int)[2]
		{
			(2, 6),
			(0, 0)
		}));
		_dataArray.Add(new RandomEnemyItem(81, new List<short> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 }, 1, 2, 10, 10, -40, 0, 0, new short[0], (70, 160), new(int, int)[2]
		{
			(3, 9),
			(0, 0)
		}));
		_dataArray.Add(new RandomEnemyItem(82, new List<short> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 }, 2, 3, 15, 15, -50, 0, 0, new short[0], (75, 170), new(int, int)[2]
		{
			(3, 9),
			(0, 0)
		}));
		_dataArray.Add(new RandomEnemyItem(83, new List<short> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 }, 2, 4, 20, 20, -60, 0, 0, new short[0], (80, 180), new(int, int)[2]
		{
			(4, 12),
			(0, 0)
		}));
		_dataArray.Add(new RandomEnemyItem(84, new List<short> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 }, 3, 5, 25, 25, -70, 0, 0, new short[0], (85, 190), new(int, int)[2]
		{
			(4, 12),
			(0, 0)
		}));
		_dataArray.Add(new RandomEnemyItem(85, new List<short> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 }, 3, 6, 30, 30, -80, 0, 0, new short[0], (90, 200), new(int, int)[2]
		{
			(5, 5),
			(0, 0)
		}));
		_dataArray.Add(new RandomEnemyItem(86, new List<short> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 }, 4, 6, 35, 35, -90, 0, 0, new short[0], (95, 210), new(int, int)[2]
		{
			(5, 5),
			(0, 0)
		}));
		_dataArray.Add(new RandomEnemyItem(87, new List<short> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 }, 4, 7, 40, 40, -100, 0, 0, new short[0], (100, 100), new(int, int)[2]
		{
			(5, 5),
			(0, 0)
		}));
		_dataArray.Add(new RandomEnemyItem(88, new List<short> { 1 }, 1, 7, 40, 40, 0, 0, 0, new short[0], (100, 100), new(int, int)[2]
		{
			(5, 5),
			(5, 5)
		}));
		_dataArray.Add(new RandomEnemyItem(89, new List<short> { 1 }, 1, 6, 0, 0, 0, 0, 0, new short[0], (75, 190), new(int, int)[2]
		{
			(5, 5),
			(4, 8)
		}));
		_dataArray.Add(new RandomEnemyItem(90, new List<short> { 1 }, 1, 5, 0, 0, 0, 0, 0, new short[0], (70, 180), new(int, int)[2]
		{
			(5, 5),
			(4, 8)
		}));
		_dataArray.Add(new RandomEnemyItem(91, new List<short> { 1 }, 1, 4, 0, 0, 0, 0, 0, new short[0], (65, 170), new(int, int)[2]
		{
			(4, 8),
			(3, 6)
		}));
		_dataArray.Add(new RandomEnemyItem(92, new List<short> { 1 }, 1, 3, 0, 0, 0, 0, 0, new short[0], (60, 160), new(int, int)[2]
		{
			(4, 8),
			(3, 6)
		}));
		_dataArray.Add(new RandomEnemyItem(93, new List<short> { 1 }, 1, 2, 0, 0, 0, 0, 0, new short[0], (55, 150), new(int, int)[2]
		{
			(3, 6),
			(2, 4)
		}));
		_dataArray.Add(new RandomEnemyItem(94, new List<short> { 1 }, 1, 1, 0, 0, 0, 0, 0, new short[0], (50, 140), new(int, int)[2]
		{
			(3, 6),
			(2, 4)
		}));
		_dataArray.Add(new RandomEnemyItem(95, new List<short> { 1 }, 1, 0, 0, 0, 0, 0, 0, new short[0], (45, 130), new(int, int)[2]
		{
			(2, 4),
			(1, 2)
		}));
		_dataArray.Add(new RandomEnemyItem(96, new List<short> { 2 }, 1, 7, 40, 40, 0, 0, 0, new short[0], (100, 100), new(int, int)[2]
		{
			(5, 5),
			(5, 5)
		}));
		_dataArray.Add(new RandomEnemyItem(97, new List<short> { 2 }, 1, 6, 0, 0, 0, 0, 0, new short[0], (75, 190), new(int, int)[2]
		{
			(5, 5),
			(4, 8)
		}));
		_dataArray.Add(new RandomEnemyItem(98, new List<short> { 2 }, 1, 5, 0, 0, 0, 0, 0, new short[0], (70, 180), new(int, int)[2]
		{
			(5, 5),
			(4, 8)
		}));
		_dataArray.Add(new RandomEnemyItem(99, new List<short> { 2 }, 1, 4, 0, 0, 0, 0, 0, new short[0], (65, 170), new(int, int)[2]
		{
			(4, 8),
			(3, 6)
		}));
		_dataArray.Add(new RandomEnemyItem(100, new List<short> { 2 }, 1, 3, 0, 0, 0, 0, 0, new short[0], (60, 160), new(int, int)[2]
		{
			(4, 8),
			(3, 6)
		}));
		_dataArray.Add(new RandomEnemyItem(101, new List<short> { 2 }, 1, 2, 0, 0, 0, 0, 0, new short[0], (55, 150), new(int, int)[2]
		{
			(3, 6),
			(2, 4)
		}));
		_dataArray.Add(new RandomEnemyItem(102, new List<short> { 2 }, 1, 1, 0, 0, 0, 0, 0, new short[0], (50, 140), new(int, int)[2]
		{
			(3, 6),
			(2, 4)
		}));
		_dataArray.Add(new RandomEnemyItem(103, new List<short> { 2 }, 1, 0, 0, 0, 0, 0, 0, new short[0], (45, 130), new(int, int)[2]
		{
			(2, 4),
			(1, 2)
		}));
		_dataArray.Add(new RandomEnemyItem(104, new List<short> { 3 }, 1, 7, 40, 40, 0, 72, 3, new short[3] { 15, 24, 51 }, (100, 100), new(int, int)[2]
		{
			(5, 5),
			(5, 5)
		}));
		_dataArray.Add(new RandomEnemyItem(105, new List<short> { 3 }, 1, 6, 0, 0, 0, 36, 3, new short[3] { 14, 23, 50 }, (75, 190), new(int, int)[2]
		{
			(5, 5),
			(4, 8)
		}));
		_dataArray.Add(new RandomEnemyItem(106, new List<short> { 3 }, 1, 5, 0, 0, 0, 32, 3, new short[3] { 13, 22, 49 }, (70, 180), new(int, int)[2]
		{
			(5, 5),
			(4, 8)
		}));
		_dataArray.Add(new RandomEnemyItem(107, new List<short> { 3 }, 1, 4, 0, 0, 0, 28, 2, new short[3] { 12, 21, 48 }, (65, 170), new(int, int)[2]
		{
			(4, 8),
			(3, 6)
		}));
		_dataArray.Add(new RandomEnemyItem(108, new List<short> { 3 }, 1, 3, 0, 0, 0, 24, 2, new short[3] { 11, 20, 47 }, (60, 160), new(int, int)[2]
		{
			(4, 8),
			(3, 6)
		}));
		_dataArray.Add(new RandomEnemyItem(109, new List<short> { 3 }, 1, 2, 0, 0, 0, 20, 2, new short[3] { 10, 19, 46 }, (55, 150), new(int, int)[2]
		{
			(3, 6),
			(2, 4)
		}));
		_dataArray.Add(new RandomEnemyItem(110, new List<short> { 3 }, 1, 1, 0, 0, 0, 16, 1, new short[3] { 9, 18, 45 }, (50, 140), new(int, int)[2]
		{
			(3, 6),
			(2, 4)
		}));
		_dataArray.Add(new RandomEnemyItem(111, new List<short> { 3 }, 1, 0, 0, 0, 0, 12, 1, new short[3] { 9, 18, 45 }, (45, 130), new(int, int)[2]
		{
			(2, 4),
			(1, 2)
		}));
		_dataArray.Add(new RandomEnemyItem(112, new List<short> { 4 }, 1, 7, 40, 40, 0, 0, 0, new short[0], (100, 100), new(int, int)[2]
		{
			(5, 5),
			(5, 5)
		}));
		_dataArray.Add(new RandomEnemyItem(113, new List<short> { 4 }, 1, 6, 0, 0, 0, 0, 0, new short[0], (75, 190), new(int, int)[2]
		{
			(5, 5),
			(4, 8)
		}));
		_dataArray.Add(new RandomEnemyItem(114, new List<short> { 4 }, 1, 5, 0, 0, 0, 0, 0, new short[0], (70, 180), new(int, int)[2]
		{
			(5, 5),
			(4, 8)
		}));
		_dataArray.Add(new RandomEnemyItem(115, new List<short> { 4 }, 1, 4, 0, 0, 0, 0, 0, new short[0], (65, 170), new(int, int)[2]
		{
			(4, 8),
			(3, 6)
		}));
		_dataArray.Add(new RandomEnemyItem(116, new List<short> { 4 }, 1, 3, 0, 0, 0, 0, 0, new short[0], (60, 160), new(int, int)[2]
		{
			(4, 8),
			(3, 6)
		}));
		_dataArray.Add(new RandomEnemyItem(117, new List<short> { 4 }, 1, 2, 0, 0, 0, 0, 0, new short[0], (55, 150), new(int, int)[2]
		{
			(3, 6),
			(2, 4)
		}));
		_dataArray.Add(new RandomEnemyItem(118, new List<short> { 4 }, 1, 1, 0, 0, 0, 0, 0, new short[0], (50, 140), new(int, int)[2]
		{
			(3, 6),
			(2, 4)
		}));
		_dataArray.Add(new RandomEnemyItem(119, new List<short> { 4 }, 1, 0, 0, 0, 0, 0, 0, new short[0], (45, 130), new(int, int)[2]
		{
			(2, 4),
			(1, 2)
		}));
	}

	private void CreateItems2()
	{
		_dataArray.Add(new RandomEnemyItem(120, new List<short> { 5 }, 1, 7, 40, 40, 0, 0, 0, new short[0], (100, 100), new(int, int)[2]
		{
			(5, 5),
			(5, 5)
		}));
		_dataArray.Add(new RandomEnemyItem(121, new List<short> { 5 }, 1, 6, 0, 0, 0, 0, 0, new short[0], (75, 190), new(int, int)[2]
		{
			(5, 5),
			(4, 8)
		}));
		_dataArray.Add(new RandomEnemyItem(122, new List<short> { 5 }, 1, 5, 0, 0, 0, 0, 0, new short[0], (70, 180), new(int, int)[2]
		{
			(5, 5),
			(4, 8)
		}));
		_dataArray.Add(new RandomEnemyItem(123, new List<short> { 5 }, 1, 4, 0, 0, 0, 0, 0, new short[0], (65, 170), new(int, int)[2]
		{
			(4, 8),
			(3, 6)
		}));
		_dataArray.Add(new RandomEnemyItem(124, new List<short> { 5 }, 1, 3, 0, 0, 0, 0, 0, new short[0], (60, 160), new(int, int)[2]
		{
			(4, 8),
			(3, 6)
		}));
		_dataArray.Add(new RandomEnemyItem(125, new List<short> { 5 }, 1, 2, 0, 0, 0, 0, 0, new short[0], (55, 150), new(int, int)[2]
		{
			(3, 6),
			(2, 4)
		}));
		_dataArray.Add(new RandomEnemyItem(126, new List<short> { 5 }, 1, 1, 0, 0, 0, 0, 0, new short[0], (50, 140), new(int, int)[2]
		{
			(3, 6),
			(2, 4)
		}));
		_dataArray.Add(new RandomEnemyItem(127, new List<short> { 5 }, 1, 0, 0, 0, 0, 0, 0, new short[0], (45, 130), new(int, int)[2]
		{
			(2, 4),
			(1, 2)
		}));
		_dataArray.Add(new RandomEnemyItem(128, new List<short> { 6 }, 1, 7, 40, 40, 0, 0, 0, new short[0], (100, 100), new(int, int)[2]
		{
			(5, 5),
			(5, 5)
		}));
		_dataArray.Add(new RandomEnemyItem(129, new List<short> { 6 }, 1, 6, 0, 0, 0, 0, 0, new short[0], (75, 190), new(int, int)[2]
		{
			(4, 12),
			(4, 12)
		}));
		_dataArray.Add(new RandomEnemyItem(130, new List<short> { 6 }, 1, 5, 0, 0, 0, 0, 0, new short[0], (70, 180), new(int, int)[2]
		{
			(4, 12),
			(4, 12)
		}));
		_dataArray.Add(new RandomEnemyItem(131, new List<short> { 6 }, 1, 4, 0, 0, 0, 0, 0, new short[0], (65, 170), new(int, int)[2]
		{
			(3, 9),
			(3, 9)
		}));
		_dataArray.Add(new RandomEnemyItem(132, new List<short> { 6 }, 1, 3, 0, 0, 0, 0, 0, new short[0], (60, 160), new(int, int)[2]
		{
			(3, 9),
			(3, 9)
		}));
		_dataArray.Add(new RandomEnemyItem(133, new List<short> { 6 }, 1, 2, 0, 0, 0, 0, 0, new short[0], (55, 150), new(int, int)[2]
		{
			(2, 6),
			(2, 6)
		}));
		_dataArray.Add(new RandomEnemyItem(134, new List<short> { 6 }, 1, 1, 0, 0, 0, 0, 0, new short[0], (50, 140), new(int, int)[2]
		{
			(2, 6),
			(2, 6)
		}));
		_dataArray.Add(new RandomEnemyItem(135, new List<short> { 6 }, 1, 0, 0, 0, 0, 0, 0, new short[0], (45, 130), new(int, int)[2]
		{
			(1, 3),
			(1, 3)
		}));
		_dataArray.Add(new RandomEnemyItem(136, new List<short> { 7 }, 1, 7, 40, 40, 0, 0, 0, new short[0], (100, 100), new(int, int)[2]
		{
			(5, 5),
			(5, 5)
		}));
		_dataArray.Add(new RandomEnemyItem(137, new List<short> { 7 }, 1, 6, 0, 0, 0, 0, 0, new short[0], (75, 190), new(int, int)[2]
		{
			(4, 12),
			(4, 12)
		}));
		_dataArray.Add(new RandomEnemyItem(138, new List<short> { 7 }, 1, 5, 0, 0, 0, 0, 0, new short[0], (70, 180), new(int, int)[2]
		{
			(4, 12),
			(4, 12)
		}));
		_dataArray.Add(new RandomEnemyItem(139, new List<short> { 7 }, 1, 4, 0, 0, 0, 0, 0, new short[0], (65, 170), new(int, int)[2]
		{
			(3, 9),
			(3, 9)
		}));
		_dataArray.Add(new RandomEnemyItem(140, new List<short> { 7 }, 1, 3, 0, 0, 0, 0, 0, new short[0], (60, 160), new(int, int)[2]
		{
			(3, 9),
			(3, 9)
		}));
		_dataArray.Add(new RandomEnemyItem(141, new List<short> { 7 }, 1, 2, 0, 0, 0, 0, 0, new short[0], (55, 150), new(int, int)[2]
		{
			(2, 6),
			(2, 6)
		}));
		_dataArray.Add(new RandomEnemyItem(142, new List<short> { 7 }, 1, 1, 0, 0, 0, 0, 0, new short[0], (50, 140), new(int, int)[2]
		{
			(2, 6),
			(2, 6)
		}));
		_dataArray.Add(new RandomEnemyItem(143, new List<short> { 7 }, 1, 0, 0, 0, 0, 0, 0, new short[0], (45, 130), new(int, int)[2]
		{
			(1, 3),
			(1, 3)
		}));
		_dataArray.Add(new RandomEnemyItem(144, new List<short> { 8 }, 1, 7, 40, 40, 0, 0, 0, new short[0], (100, 100), new(int, int)[2]
		{
			(5, 5),
			(5, 5)
		}));
		_dataArray.Add(new RandomEnemyItem(145, new List<short> { 8 }, 1, 6, 0, 0, 0, 0, 0, new short[0], (75, 190), new(int, int)[2]
		{
			(4, 12),
			(4, 12)
		}));
		_dataArray.Add(new RandomEnemyItem(146, new List<short> { 8 }, 1, 5, 0, 0, 0, 0, 0, new short[0], (70, 180), new(int, int)[2]
		{
			(4, 12),
			(4, 12)
		}));
		_dataArray.Add(new RandomEnemyItem(147, new List<short> { 8 }, 1, 4, 0, 0, 0, 0, 0, new short[0], (65, 170), new(int, int)[2]
		{
			(3, 9),
			(3, 9)
		}));
		_dataArray.Add(new RandomEnemyItem(148, new List<short> { 8 }, 1, 3, 0, 0, 0, 0, 0, new short[0], (60, 160), new(int, int)[2]
		{
			(3, 9),
			(3, 9)
		}));
		_dataArray.Add(new RandomEnemyItem(149, new List<short> { 8 }, 1, 2, 0, 0, 0, 0, 0, new short[0], (55, 150), new(int, int)[2]
		{
			(2, 6),
			(2, 6)
		}));
		_dataArray.Add(new RandomEnemyItem(150, new List<short> { 8 }, 1, 1, 0, 0, 0, 0, 0, new short[0], (50, 140), new(int, int)[2]
		{
			(2, 6),
			(2, 6)
		}));
		_dataArray.Add(new RandomEnemyItem(151, new List<short> { 8 }, 1, 0, 0, 0, 0, 0, 0, new short[0], (45, 130), new(int, int)[2]
		{
			(1, 3),
			(1, 3)
		}));
		_dataArray.Add(new RandomEnemyItem(152, new List<short> { 9 }, 1, 7, 40, 40, 0, 0, 0, new short[0], (100, 100), new(int, int)[2]
		{
			(5, 5),
			(5, 5)
		}));
		_dataArray.Add(new RandomEnemyItem(153, new List<short> { 9 }, 1, 6, 0, 0, 0, 0, 0, new short[0], (75, 190), new(int, int)[2]
		{
			(4, 12),
			(4, 12)
		}));
		_dataArray.Add(new RandomEnemyItem(154, new List<short> { 9 }, 1, 5, 0, 0, 0, 0, 0, new short[0], (70, 180), new(int, int)[2]
		{
			(4, 12),
			(4, 12)
		}));
		_dataArray.Add(new RandomEnemyItem(155, new List<short> { 9 }, 1, 4, 0, 0, 0, 0, 0, new short[0], (65, 170), new(int, int)[2]
		{
			(3, 9),
			(3, 9)
		}));
		_dataArray.Add(new RandomEnemyItem(156, new List<short> { 9 }, 1, 3, 0, 0, 0, 0, 0, new short[0], (60, 160), new(int, int)[2]
		{
			(3, 9),
			(3, 9)
		}));
		_dataArray.Add(new RandomEnemyItem(157, new List<short> { 9 }, 1, 2, 0, 0, 0, 0, 0, new short[0], (55, 150), new(int, int)[2]
		{
			(2, 6),
			(2, 6)
		}));
		_dataArray.Add(new RandomEnemyItem(158, new List<short> { 9 }, 1, 1, 0, 0, 0, 0, 0, new short[0], (50, 140), new(int, int)[2]
		{
			(2, 6),
			(2, 6)
		}));
		_dataArray.Add(new RandomEnemyItem(159, new List<short> { 9 }, 1, 0, 0, 0, 0, 0, 0, new short[0], (45, 130), new(int, int)[2]
		{
			(1, 3),
			(1, 3)
		}));
		_dataArray.Add(new RandomEnemyItem(160, new List<short> { 10 }, 1, 7, 40, 40, 0, 88, 3, new short[6] { 6, 15, 24, 33, 42, 51 }, (100, 100), new(int, int)[2]
		{
			(5, 5),
			(5, 5)
		}));
		_dataArray.Add(new RandomEnemyItem(161, new List<short> { 10 }, 1, 6, 0, 0, 0, 44, 3, new short[6] { 5, 14, 23, 32, 41, 50 }, (75, 190), new(int, int)[2]
		{
			(4, 12),
			(4, 12)
		}));
		_dataArray.Add(new RandomEnemyItem(162, new List<short> { 10 }, 1, 5, 0, 0, 0, 40, 3, new short[6] { 4, 13, 22, 31, 40, 49 }, (70, 180), new(int, int)[2]
		{
			(4, 12),
			(4, 12)
		}));
		_dataArray.Add(new RandomEnemyItem(163, new List<short> { 10 }, 1, 4, 0, 0, 0, 36, 2, new short[6] { 3, 12, 21, 30, 39, 48 }, (65, 170), new(int, int)[2]
		{
			(3, 9),
			(3, 9)
		}));
		_dataArray.Add(new RandomEnemyItem(164, new List<short> { 10 }, 1, 3, 0, 0, 0, 32, 2, new short[6] { 2, 11, 20, 29, 38, 47 }, (60, 160), new(int, int)[2]
		{
			(3, 9),
			(3, 9)
		}));
		_dataArray.Add(new RandomEnemyItem(165, new List<short> { 10 }, 1, 2, 0, 0, 0, 28, 2, new short[6] { 1, 10, 19, 28, 37, 46 }, (55, 150), new(int, int)[2]
		{
			(2, 6),
			(2, 6)
		}));
		_dataArray.Add(new RandomEnemyItem(166, new List<short> { 10 }, 1, 1, 0, 0, 0, 24, 1, new short[6] { 0, 9, 18, 27, 36, 45 }, (50, 140), new(int, int)[2]
		{
			(2, 6),
			(2, 6)
		}));
		_dataArray.Add(new RandomEnemyItem(167, new List<short> { 10 }, 1, 0, 0, 0, 0, 20, 1, new short[6] { 0, 9, 18, 27, 36, 45 }, (45, 130), new(int, int)[2]
		{
			(1, 3),
			(1, 3)
		}));
		_dataArray.Add(new RandomEnemyItem(168, new List<short> { 11 }, 1, 7, 40, 40, 0, 56, 3, new short[3] { 6, 33, 42 }, (100, 100), new(int, int)[2]
		{
			(5, 5),
			(5, 5)
		}));
		_dataArray.Add(new RandomEnemyItem(169, new List<short> { 11 }, 1, 6, 0, 0, 0, 28, 3, new short[3] { 5, 32, 41 }, (75, 190), new(int, int)[2]
		{
			(4, 8),
			(5, 5)
		}));
		_dataArray.Add(new RandomEnemyItem(170, new List<short> { 11 }, 1, 5, 0, 0, 0, 24, 3, new short[3] { 4, 31, 40 }, (70, 180), new(int, int)[2]
		{
			(4, 8),
			(5, 5)
		}));
		_dataArray.Add(new RandomEnemyItem(171, new List<short> { 11 }, 1, 4, 0, 0, 0, 20, 2, new short[3] { 3, 30, 39 }, (65, 170), new(int, int)[2]
		{
			(3, 6),
			(4, 8)
		}));
		_dataArray.Add(new RandomEnemyItem(172, new List<short> { 11 }, 1, 3, 0, 0, 0, 16, 2, new short[3] { 2, 29, 38 }, (60, 160), new(int, int)[2]
		{
			(3, 6),
			(4, 8)
		}));
		_dataArray.Add(new RandomEnemyItem(173, new List<short> { 11 }, 1, 2, 0, 0, 0, 12, 2, new short[3] { 1, 28, 37 }, (55, 150), new(int, int)[2]
		{
			(2, 4),
			(3, 6)
		}));
		_dataArray.Add(new RandomEnemyItem(174, new List<short> { 11 }, 1, 1, 0, 0, 0, 8, 1, new short[3] { 0, 27, 36 }, (50, 140), new(int, int)[2]
		{
			(2, 4),
			(3, 6)
		}));
		_dataArray.Add(new RandomEnemyItem(175, new List<short> { 11 }, 1, 0, 0, 0, 0, 4, 1, new short[3] { 0, 27, 36 }, (45, 130), new(int, int)[2]
		{
			(1, 2),
			(2, 4)
		}));
		_dataArray.Add(new RandomEnemyItem(176, new List<short> { 12 }, 1, 7, 40, 40, 0, 88, 3, new short[6] { 6, 15, 24, 33, 42, 51 }, (100, 100), new(int, int)[2]
		{
			(5, 5),
			(5, 5)
		}));
		_dataArray.Add(new RandomEnemyItem(177, new List<short> { 12 }, 1, 6, 0, 0, 0, 44, 3, new short[6] { 5, 14, 23, 32, 41, 50 }, (75, 190), new(int, int)[2]
		{
			(4, 8),
			(5, 5)
		}));
		_dataArray.Add(new RandomEnemyItem(178, new List<short> { 12 }, 1, 5, 0, 0, 0, 40, 3, new short[6] { 4, 13, 22, 31, 40, 49 }, (70, 180), new(int, int)[2]
		{
			(4, 8),
			(5, 5)
		}));
		_dataArray.Add(new RandomEnemyItem(179, new List<short> { 12 }, 1, 4, 0, 0, 0, 36, 2, new short[6] { 3, 12, 21, 30, 39, 48 }, (65, 170), new(int, int)[2]
		{
			(3, 6),
			(4, 8)
		}));
	}

	private void CreateItems3()
	{
		_dataArray.Add(new RandomEnemyItem(180, new List<short> { 12 }, 1, 3, 0, 0, 0, 32, 2, new short[6] { 2, 11, 20, 29, 38, 47 }, (60, 160), new(int, int)[2]
		{
			(3, 6),
			(4, 8)
		}));
		_dataArray.Add(new RandomEnemyItem(181, new List<short> { 12 }, 1, 2, 0, 0, 0, 28, 2, new short[6] { 1, 10, 19, 28, 37, 46 }, (55, 150), new(int, int)[2]
		{
			(2, 4),
			(3, 6)
		}));
		_dataArray.Add(new RandomEnemyItem(182, new List<short> { 12 }, 1, 1, 0, 0, 0, 24, 1, new short[6] { 0, 9, 18, 27, 36, 45 }, (50, 140), new(int, int)[2]
		{
			(2, 4),
			(3, 6)
		}));
		_dataArray.Add(new RandomEnemyItem(183, new List<short> { 12 }, 1, 0, 0, 0, 0, 20, 1, new short[6] { 0, 9, 18, 27, 36, 45 }, (45, 130), new(int, int)[2]
		{
			(1, 2),
			(2, 4)
		}));
		_dataArray.Add(new RandomEnemyItem(184, new List<short> { 13 }, 1, 7, 40, 40, 0, 56, 3, new short[3] { 15, 24, 51 }, (100, 100), new(int, int)[2]
		{
			(5, 5),
			(5, 5)
		}));
		_dataArray.Add(new RandomEnemyItem(185, new List<short> { 13 }, 1, 6, 0, 0, 0, 28, 3, new short[3] { 14, 23, 50 }, (75, 190), new(int, int)[2]
		{
			(4, 8),
			(5, 5)
		}));
		_dataArray.Add(new RandomEnemyItem(186, new List<short> { 13 }, 1, 5, 0, 0, 0, 24, 3, new short[3] { 13, 22, 49 }, (70, 180), new(int, int)[2]
		{
			(4, 8),
			(5, 5)
		}));
		_dataArray.Add(new RandomEnemyItem(187, new List<short> { 13 }, 1, 4, 0, 0, 0, 20, 2, new short[3] { 12, 21, 48 }, (65, 170), new(int, int)[2]
		{
			(3, 6),
			(4, 8)
		}));
		_dataArray.Add(new RandomEnemyItem(188, new List<short> { 13 }, 1, 3, 0, 0, 0, 16, 2, new short[3] { 11, 20, 47 }, (60, 160), new(int, int)[2]
		{
			(3, 6),
			(4, 8)
		}));
		_dataArray.Add(new RandomEnemyItem(189, new List<short> { 13 }, 1, 2, 0, 0, 0, 12, 2, new short[3] { 10, 19, 46 }, (55, 150), new(int, int)[2]
		{
			(2, 4),
			(3, 6)
		}));
		_dataArray.Add(new RandomEnemyItem(190, new List<short> { 13 }, 1, 1, 0, 0, 0, 8, 1, new short[3] { 9, 18, 45 }, (50, 140), new(int, int)[2]
		{
			(2, 4),
			(3, 6)
		}));
		_dataArray.Add(new RandomEnemyItem(191, new List<short> { 13 }, 1, 0, 0, 0, 0, 4, 1, new short[3] { 9, 18, 45 }, (45, 130), new(int, int)[2]
		{
			(1, 2),
			(2, 4)
		}));
		_dataArray.Add(new RandomEnemyItem(192, new List<short> { 14 }, 1, 7, 40, 40, 0, 0, 0, new short[0], (100, 100), new(int, int)[2]
		{
			(5, 5),
			(5, 5)
		}));
		_dataArray.Add(new RandomEnemyItem(193, new List<short> { 14 }, 1, 6, 0, 0, 0, 0, 0, new short[0], (75, 190), new(int, int)[2]
		{
			(4, 8),
			(5, 5)
		}));
		_dataArray.Add(new RandomEnemyItem(194, new List<short> { 14 }, 1, 5, 0, 0, 0, 0, 0, new short[0], (70, 180), new(int, int)[2]
		{
			(4, 8),
			(5, 5)
		}));
		_dataArray.Add(new RandomEnemyItem(195, new List<short> { 14 }, 1, 4, 0, 0, 0, 0, 0, new short[0], (65, 170), new(int, int)[2]
		{
			(3, 6),
			(4, 8)
		}));
		_dataArray.Add(new RandomEnemyItem(196, new List<short> { 14 }, 1, 3, 0, 0, 0, 0, 0, new short[0], (60, 160), new(int, int)[2]
		{
			(3, 6),
			(4, 8)
		}));
		_dataArray.Add(new RandomEnemyItem(197, new List<short> { 14 }, 1, 2, 0, 0, 0, 0, 0, new short[0], (55, 150), new(int, int)[2]
		{
			(2, 4),
			(3, 6)
		}));
		_dataArray.Add(new RandomEnemyItem(198, new List<short> { 14 }, 1, 1, 0, 0, 0, 0, 0, new short[0], (50, 140), new(int, int)[2]
		{
			(2, 4),
			(3, 6)
		}));
		_dataArray.Add(new RandomEnemyItem(199, new List<short> { 14 }, 1, 0, 0, 0, 0, 0, 0, new short[0], (45, 130), new(int, int)[2]
		{
			(1, 2),
			(2, 4)
		}));
		_dataArray.Add(new RandomEnemyItem(200, new List<short> { 15 }, 1, 7, 40, 40, 0, 56, 3, new short[3] { 6, 33, 42 }, (100, 100), new(int, int)[2]
		{
			(5, 5),
			(5, 5)
		}));
		_dataArray.Add(new RandomEnemyItem(201, new List<short> { 15 }, 1, 6, 0, 0, 0, 28, 3, new short[3] { 5, 32, 41 }, (75, 190), new(int, int)[2]
		{
			(4, 8),
			(5, 5)
		}));
		_dataArray.Add(new RandomEnemyItem(202, new List<short> { 15 }, 1, 5, 0, 0, 0, 24, 3, new short[3] { 4, 31, 40 }, (70, 180), new(int, int)[2]
		{
			(4, 8),
			(5, 5)
		}));
		_dataArray.Add(new RandomEnemyItem(203, new List<short> { 15 }, 1, 4, 0, 0, 0, 20, 2, new short[3] { 3, 30, 39 }, (65, 170), new(int, int)[2]
		{
			(3, 6),
			(4, 8)
		}));
		_dataArray.Add(new RandomEnemyItem(204, new List<short> { 15 }, 1, 3, 0, 0, 0, 16, 2, new short[3] { 2, 29, 38 }, (60, 160), new(int, int)[2]
		{
			(3, 6),
			(4, 8)
		}));
		_dataArray.Add(new RandomEnemyItem(205, new List<short> { 15 }, 1, 2, 0, 0, 0, 12, 2, new short[3] { 1, 28, 37 }, (55, 150), new(int, int)[2]
		{
			(2, 4),
			(3, 6)
		}));
		_dataArray.Add(new RandomEnemyItem(206, new List<short> { 15 }, 1, 1, 0, 0, 0, 8, 1, new short[3] { 0, 27, 36 }, (50, 140), new(int, int)[2]
		{
			(2, 4),
			(3, 6)
		}));
		_dataArray.Add(new RandomEnemyItem(207, new List<short> { 15 }, 1, 0, 0, 0, 0, 4, 1, new short[3] { 0, 27, 36 }, (45, 130), new(int, int)[2]
		{
			(1, 2),
			(2, 4)
		}));
		_dataArray.Add(new RandomEnemyItem(208, new List<short> { 2 }, 1, 4, 20, 20, 0, 0, 0, new short[0], (100, 100), new(int, int)[2]
		{
			(5, 5),
			(5, 5)
		}));
		_dataArray.Add(new RandomEnemyItem(209, new List<short> { 1 }, 1, 6, 20, 20, 0, 0, 0, new short[0], (80, 200), new(int, int)[2]
		{
			(5, 5),
			(4, 8)
		}));
		_dataArray.Add(new RandomEnemyItem(210, new List<short> { 2 }, 1, 6, 20, 20, 0, 0, 0, new short[0], (80, 200), new(int, int)[2]
		{
			(5, 5),
			(4, 8)
		}));
		_dataArray.Add(new RandomEnemyItem(211, new List<short> { 3 }, 1, 6, 20, 20, 0, 54, 3, new short[3] { 14, 23, 50 }, (80, 200), new(int, int)[2]
		{
			(5, 5),
			(4, 8)
		}));
		_dataArray.Add(new RandomEnemyItem(212, new List<short> { 4 }, 1, 6, 20, 20, 0, 0, 0, new short[0], (80, 200), new(int, int)[2]
		{
			(5, 5),
			(4, 8)
		}));
		_dataArray.Add(new RandomEnemyItem(213, new List<short> { 5 }, 1, 6, 20, 20, 0, 0, 0, new short[0], (80, 200), new(int, int)[2]
		{
			(5, 5),
			(4, 8)
		}));
		_dataArray.Add(new RandomEnemyItem(214, new List<short> { 6 }, 1, 6, 20, 20, 0, 0, 0, new short[0], (80, 200), new(int, int)[2]
		{
			(4, 8),
			(4, 8)
		}));
		_dataArray.Add(new RandomEnemyItem(215, new List<short> { 7 }, 1, 6, 20, 20, 0, 0, 0, new short[0], (80, 200), new(int, int)[2]
		{
			(4, 8),
			(4, 8)
		}));
		_dataArray.Add(new RandomEnemyItem(216, new List<short> { 8 }, 1, 6, 20, 20, 0, 0, 0, new short[0], (80, 200), new(int, int)[2]
		{
			(4, 8),
			(4, 8)
		}));
		_dataArray.Add(new RandomEnemyItem(217, new List<short> { 9 }, 1, 6, 20, 20, 0, 0, 0, new short[0], (80, 200), new(int, int)[2]
		{
			(4, 8),
			(4, 8)
		}));
		_dataArray.Add(new RandomEnemyItem(218, new List<short> { 10 }, 1, 6, 20, 20, 0, 66, 3, new short[6] { 5, 14, 23, 32, 41, 50 }, (80, 200), new(int, int)[2]
		{
			(4, 8),
			(4, 8)
		}));
		_dataArray.Add(new RandomEnemyItem(219, new List<short> { 11 }, 1, 6, 20, 20, 0, 42, 3, new short[3] { 5, 32, 41 }, (80, 200), new(int, int)[2]
		{
			(4, 8),
			(5, 5)
		}));
		_dataArray.Add(new RandomEnemyItem(220, new List<short> { 12 }, 1, 6, 20, 20, 0, 66, 3, new short[6] { 5, 14, 23, 32, 41, 50 }, (80, 200), new(int, int)[2]
		{
			(4, 8),
			(5, 5)
		}));
		_dataArray.Add(new RandomEnemyItem(221, new List<short> { 13 }, 1, 6, 20, 20, 0, 42, 3, new short[3] { 14, 23, 50 }, (80, 200), new(int, int)[2]
		{
			(4, 8),
			(5, 5)
		}));
		_dataArray.Add(new RandomEnemyItem(222, new List<short> { 14 }, 1, 6, 20, 20, 0, 0, 0, new short[0], (80, 200), new(int, int)[2]
		{
			(4, 8),
			(5, 5)
		}));
		_dataArray.Add(new RandomEnemyItem(223, new List<short> { 15 }, 1, 6, 20, 20, 0, 42, 3, new short[3] { 5, 32, 41 }, (80, 200), new(int, int)[2]
		{
			(4, 8),
			(5, 5)
		}));
		_dataArray.Add(new RandomEnemyItem(224, new List<short>
		{
			1, 2, 3, 4, 5, 6, 7, 8, 9, 10,
			11, 12, 13, 14, 15
		}, 3, 2, 40, 40, 0, 36, 2, new short[6] { 3, 12, 21, 30, 39, 48 }, (60, 160), new(int, int)[2]
		{
			(3, 6),
			(4, 8)
		}));
		_dataArray.Add(new RandomEnemyItem(225, new List<short>
		{
			1, 2, 3, 4, 5, 6, 7, 8, 9, 10,
			11, 12, 13, 14, 15
		}, 3, 3, 40, 40, 0, 40, 3, new short[6] { 4, 13, 22, 31, 40, 49 }, (65, 170), new(int, int)[2]
		{
			(3, 6),
			(4, 8)
		}));
		_dataArray.Add(new RandomEnemyItem(226, new List<short>
		{
			1, 2, 3, 4, 5, 6, 7, 8, 9, 10,
			11, 12, 13, 14, 15
		}, 3, 4, 40, 40, 0, 44, 3, new short[6] { 5, 14, 23, 32, 41, 50 }, (70, 180), new(int, int)[2]
		{
			(4, 8),
			(4, 8)
		}));
		_dataArray.Add(new RandomEnemyItem(227, new List<short>
		{
			1, 2, 3, 4, 5, 6, 7, 8, 9, 10,
			11, 12, 13, 14, 15
		}, 3, 5, 40, 40, 0, 66, 3, new short[6] { 5, 14, 23, 32, 41, 50 }, (75, 190), new(int, int)[2]
		{
			(4, 8),
			(4, 8)
		}));
		_dataArray.Add(new RandomEnemyItem(228, new List<short>
		{
			1, 2, 3, 4, 5, 6, 7, 8, 9, 10,
			11, 12, 13, 14, 15
		}, 3, 6, 40, 40, 0, 88, 3, new short[6] { 6, 15, 24, 33, 42, 51 }, (80, 200), new(int, int)[2]
		{
			(4, 8),
			(4, 8)
		}));
	}

	public override void Init()
	{
		base.Init();
		_dataArray = new List<RandomEnemyItem>(229);
		CreateItems0();
		CreateItems1();
		CreateItems2();
		CreateItems3();
	}
}
