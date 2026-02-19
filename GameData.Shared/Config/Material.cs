using System;
using System.Collections.Generic;
using Config.Common;
using Config.ConfigCells.Character;
using GameData.Domains.Item;

namespace Config;

[Serializable]
public class Material : ConfigData<MaterialItem, short>
{
	public static class DefKey
	{
		public const short WoodOuter1 = 0;

		public const short WoodOuter2 = 1;

		public const short WoodOuter3 = 2;

		public const short WoodOuter4 = 3;

		public const short WoodOuter5 = 4;

		public const short WoodOuter6 = 5;

		public const short WoodOuter7 = 6;

		public const short WoodInner1 = 7;

		public const short WoodInner2 = 8;

		public const short WoodInner3 = 9;

		public const short WoodInner4 = 10;

		public const short WoodInner5 = 11;

		public const short WoodInner6 = 12;

		public const short WoodInner7 = 13;

		public const short MetalOuter1 = 14;

		public const short MetalOuter2 = 15;

		public const short MetalOuter3 = 16;

		public const short MetalOuter4 = 17;

		public const short MetalOuter5 = 18;

		public const short MetalOuter6 = 19;

		public const short MetalOuter7 = 20;

		public const short MetalInner1 = 21;

		public const short MetalInner2 = 22;

		public const short MetalInner3 = 23;

		public const short MetalInner4 = 24;

		public const short MetalInner5 = 25;

		public const short MetalInner6 = 26;

		public const short MetalInner7 = 27;

		public const short JadeOuter1 = 28;

		public const short JadeOuter2 = 29;

		public const short JadeOuter3 = 30;

		public const short JadeOuter4 = 31;

		public const short JadeOuter5 = 32;

		public const short JadeOuter6 = 33;

		public const short JadeOuter7 = 34;

		public const short JadeInner1 = 35;

		public const short JadeInner2 = 36;

		public const short JadeInner3 = 37;

		public const short JadeInner4 = 38;

		public const short JadeInner5 = 39;

		public const short JadeInner6 = 40;

		public const short JadeInner7 = 41;

		public const short FabricOuter1 = 42;

		public const short FabricOuter2 = 43;

		public const short FabricOuter3 = 44;

		public const short FabricOuter4 = 45;

		public const short FabricOuter5 = 46;

		public const short FabricOuter6 = 47;

		public const short FabricOuter7 = 48;

		public const short FabricInner1 = 49;

		public const short FabricInner2 = 50;

		public const short FabricInner3 = 51;

		public const short FabricInner4 = 52;

		public const short FabricInner5 = 53;

		public const short FabricInner6 = 54;

		public const short FabricInner7 = 55;

		public const short CookingBird0 = 56;

		public const short CookingBird1 = 57;

		public const short CookingBird2 = 58;

		public const short CookingBird3 = 59;

		public const short CookingBird4 = 60;

		public const short CookingBird5 = 61;

		public const short CookingBird6 = 62;

		public const short CookingBeast0 = 63;

		public const short CookingBeast1 = 64;

		public const short CookingBeast2 = 65;

		public const short CookingBeast3 = 66;

		public const short CookingBeast4 = 67;

		public const short CookingBeast5 = 68;

		public const short CookingBeast6 = 69;

		public const short CookingVegetarian0 = 70;

		public const short CookingVegetarian1 = 71;

		public const short CookingVegetarian2 = 72;

		public const short CookingVegetarian3 = 73;

		public const short CookingVegetarian4 = 74;

		public const short CookingVegetarian5 = 75;

		public const short CookingVegetarian6 = 76;

		public const short CookingFish0 = 77;

		public const short CookingFish1 = 78;

		public const short CookingFish2 = 79;

		public const short CookingFish3 = 80;

		public const short CookingFish4 = 81;

		public const short CookingFish5 = 82;

		public const short CookingFish6 = 83;

		public const short MedicineOuterInjury1 = 140;

		public const short MedicineOuterInjury3 = 141;

		public const short MedicineOuterInjury5 = 142;

		public const short MedicineOuterInjury7 = 143;

		public const short MedicinePoisonRed1 = 144;

		public const short MedicinePoisonRed3 = 145;

		public const short MedicinePoisonRed5 = 146;

		public const short MedicinePoisonRed7 = 147;

		public const short MedicinePenetrateResistOfOuter1 = 148;

		public const short MedicinePenetrateResistOfOuter3 = 149;

		public const short MedicinePenetrateResistOfOuter5 = 150;

		public const short MedicinePenetrateResistOfOuter7 = 151;

		public const short MedicineStrength1 = 152;

		public const short MedicineStrength3 = 153;

		public const short MedicineStrength5 = 154;

		public const short MedicineStrength7 = 155;

		public const short MedicineInnerInjury1 = 156;

		public const short MedicineInnerInjury3 = 157;

		public const short MedicineInnerInjury5 = 158;

		public const short MedicineInnerInjury7 = 159;

		public const short MedicinePoisonGloomy1 = 160;

		public const short MedicinePoisonGloomy3 = 161;

		public const short MedicinePoisonGloomy5 = 162;

		public const short MedicinePoisonGloomy7 = 163;

		public const short MedicinePenetrateResistOfInner1 = 164;

		public const short MedicinePenetrateResistOfInner3 = 165;

		public const short MedicinePenetrateResistOfInner5 = 166;

		public const short MedicinePenetrateResistOfInner7 = 167;

		public const short MedicineRecoverOfBreath1 = 168;

		public const short MedicineRecoverOfBreath3 = 169;

		public const short MedicineRecoverOfBreath5 = 170;

		public const short MedicineRecoverOfBreath7 = 171;

		public const short MedicineDisorderOfQi1 = 172;

		public const short MedicineDisorderOfQi3 = 173;

		public const short MedicineDisorderOfQi5 = 174;

		public const short MedicineDisorderOfQi7 = 175;

		public const short MedicinePoisonCold1 = 176;

		public const short MedicinePoisonCold3 = 177;

		public const short MedicinePoisonCold5 = 178;

		public const short MedicinePoisonCold7 = 179;

		public const short MedicineAvoidRateStr1 = 180;

		public const short MedicineAvoidRateStr3 = 181;

		public const short MedicineAvoidRateStr5 = 182;

		public const short MedicineAvoidRateStr7 = 183;

		public const short MedicineHitRateTechnique1 = 184;

		public const short MedicineHitRateTechnique3 = 185;

		public const short MedicineHitRateTechnique5 = 186;

		public const short MedicineHitRateTechnique7 = 187;

		public const short MedicineHealth1 = 188;

		public const short MedicineHealth3 = 189;

		public const short MedicineHealth5 = 190;

		public const short MedicineHealth7 = 191;

		public const short MedicinePoisonIllusory1 = 192;

		public const short MedicinePoisonIllusory3 = 193;

		public const short MedicinePoisonIllusory5 = 194;

		public const short MedicinePoisonIllusory7 = 195;

		public const short MedicineAvoidRateTech1 = 196;

		public const short MedicineAvoidRateTech3 = 197;

		public const short MedicineAvoidRateTech5 = 198;

		public const short MedicineAvoidRateTech7 = 199;

		public const short MedicineRecoverOfStance1 = 200;

		public const short MedicineRecoverOfStance3 = 201;

		public const short MedicineRecoverOfStance5 = 202;

		public const short MedicineRecoverOfStance7 = 203;

		public const short MedicineRecoverOtherA1 = 204;

		public const short MedicineRecoverOtherA3 = 205;

		public const short MedicineRecoverOtherA5 = 206;

		public const short MedicineRecoverOtherA7 = 207;

		public const short MedicinePoisonRotten1 = 208;

		public const short MedicinePoisonRotten3 = 209;

		public const short MedicinePoisonRotten5 = 210;

		public const short MedicinePoisonRotten7 = 211;

		public const short MedicineAvoidRateSpeed1 = 212;

		public const short MedicineAvoidRateSpeed3 = 213;

		public const short MedicineAvoidRateSpeed5 = 214;

		public const short MedicineAvoidRateSpeed7 = 215;

		public const short MedicineHitRateSpeed1 = 216;

		public const short MedicineHitRateSpeed3 = 217;

		public const short MedicineHitRateSpeed5 = 218;

		public const short MedicineHitRateSpeed7 = 219;

		public const short MedicineRecoverAttackA1 = 220;

		public const short MedicineRecoverAttackA3 = 221;

		public const short MedicineRecoverAttackA5 = 222;

		public const short MedicineRecoverAttackA7 = 223;

		public const short MedicinePoisonHot1 = 224;

		public const short MedicinePoisonHot3 = 225;

		public const short MedicinePoisonHot5 = 226;

		public const short MedicinePoisonHot7 = 227;

		public const short MedicineRecoverOtherB1 = 228;

		public const short MedicineRecoverOtherB3 = 229;

		public const short MedicineRecoverOtherB5 = 230;

		public const short MedicineRecoverOtherB7 = 231;

		public const short MedicineRecoverAttackB1 = 232;

		public const short MedicineRecoverAttackB3 = 233;

		public const short MedicineRecoverAttackB5 = 234;

		public const short MedicineRecoverAttackB7 = 235;

		public const short PoisonHot1 = 236;

		public const short PoisonHot2 = 237;

		public const short PoisonHot3 = 238;

		public const short PoisonHot4 = 239;

		public const short PoisonHot5 = 240;

		public const short PoisonHot6 = 241;

		public const short PoisonHot7 = 242;

		public const short PoisonGloomy1 = 243;

		public const short PoisonGloomy2 = 244;

		public const short PoisonGloomy3 = 245;

		public const short PoisonGloomy4 = 246;

		public const short PoisonGloomy5 = 247;

		public const short PoisonGloomy6 = 248;

		public const short PoisonGloomy7 = 249;

		public const short PoisonCold1 = 250;

		public const short PoisonCold2 = 251;

		public const short PoisonCold3 = 252;

		public const short PoisonCold4 = 253;

		public const short PoisonCold5 = 254;

		public const short PoisonCold6 = 255;

		public const short PoisonCold7 = 256;

		public const short PoisonRed1 = 257;

		public const short PoisonRed2 = 258;

		public const short PoisonRed3 = 259;

		public const short PoisonRed4 = 260;

		public const short PoisonRed5 = 261;

		public const short PoisonRed6 = 262;

		public const short PoisonRed7 = 263;

		public const short PoisonRotten1 = 264;

		public const short PoisonRotten2 = 265;

		public const short PoisonRotten3 = 266;

		public const short PoisonRotten4 = 267;

		public const short PoisonRotten5 = 268;

		public const short PoisonRotten6 = 269;

		public const short PoisonRotten7 = 270;

		public const short PoisonIllusory1 = 271;

		public const short PoisonIllusory2 = 272;

		public const short PoisonIllusory3 = 273;

		public const short PoisonIllusory4 = 274;

		public const short PoisonIllusory5 = 275;

		public const short PoisonIllusory6 = 276;

		public const short PoisonIllusory7 = 277;

		public const short GreenBambooPiece = 279;

		public const short JiaoWhiteEgg = 280;

		public const short JiaoBlackEgg = 281;

		public const short JiaoGreenEgg = 282;

		public const short JiaoRedEgg = 283;

		public const short JiaoYellowEgg = 284;

		public const short JiaoWBEgg = 285;

		public const short JiaoWGEgg = 286;

		public const short JiaoWREgg = 287;

		public const short JiaoWYEgg = 288;

		public const short JiaoBGEgg = 289;

		public const short JiaoBREgg = 290;

		public const short JiaoBYEgg = 291;

		public const short JiaoGREgg = 292;

		public const short JiaoGYEgg = 293;

		public const short JiaoRYEgg = 294;

		public const short JiaoWBGEgg = 295;

		public const short JiaoWBREgg = 296;

		public const short JiaoWBYEgg = 297;

		public const short JiaoWGREgg = 298;

		public const short JiaoWGYEgg = 299;

		public const short JiaoWRYEgg = 300;

		public const short JiaoBGREgg = 301;

		public const short JiaoBGYEgg = 302;

		public const short JiaoBRYEgg = 303;

		public const short JiaoGRYEgg = 304;

		public const short JiaoWBGREgg = 305;

		public const short JiaoWBGYEgg = 306;

		public const short JiaoWBRYEgg = 307;

		public const short JiaoWGRYEgg = 308;

		public const short JiaoBGRYEgg = 309;

		public const short JiaoWGRYBEgg = 310;

		public const short JiaoWhite = 311;

		public const short JiaoBlack = 312;

		public const short JiaoGreen = 313;

		public const short JiaoRed = 314;

		public const short JiaoYellow = 315;

		public const short JiaoWB = 316;

		public const short JiaoWG = 317;

		public const short JiaoWR = 318;

		public const short JiaoWY = 319;

		public const short JiaoBG = 320;

		public const short JiaoBR = 321;

		public const short JiaoBY = 322;

		public const short JiaoGR = 323;

		public const short JiaoGY = 324;

		public const short JiaoRY = 325;

		public const short JiaoWBG = 326;

		public const short JiaoWBR = 327;

		public const short JiaoWBY = 328;

		public const short JiaoWGR = 329;

		public const short JiaoWGY = 330;

		public const short JiaoWRY = 331;

		public const short JiaoBGR = 332;

		public const short JiaoBGY = 333;

		public const short JiaoBRY = 334;

		public const short JiaoGRY = 335;

		public const short JiaoWBGR = 336;

		public const short JiaoWBGY = 337;

		public const short JiaoWBRY = 338;

		public const short JiaoWGRY = 339;

		public const short JiaoBGRY = 340;

		public const short JiaoWGRYB = 341;

		public const short SectStoryFulongFeathers = 342;
	}

	public static class DefValue
	{
		public static MaterialItem WoodOuter1 => Instance[(short)0];

		public static MaterialItem WoodOuter2 => Instance[(short)1];

		public static MaterialItem WoodOuter3 => Instance[(short)2];

		public static MaterialItem WoodOuter4 => Instance[(short)3];

		public static MaterialItem WoodOuter5 => Instance[(short)4];

		public static MaterialItem WoodOuter6 => Instance[(short)5];

		public static MaterialItem WoodOuter7 => Instance[(short)6];

		public static MaterialItem WoodInner1 => Instance[(short)7];

		public static MaterialItem WoodInner2 => Instance[(short)8];

		public static MaterialItem WoodInner3 => Instance[(short)9];

		public static MaterialItem WoodInner4 => Instance[(short)10];

		public static MaterialItem WoodInner5 => Instance[(short)11];

		public static MaterialItem WoodInner6 => Instance[(short)12];

		public static MaterialItem WoodInner7 => Instance[(short)13];

		public static MaterialItem MetalOuter1 => Instance[(short)14];

		public static MaterialItem MetalOuter2 => Instance[(short)15];

		public static MaterialItem MetalOuter3 => Instance[(short)16];

		public static MaterialItem MetalOuter4 => Instance[(short)17];

		public static MaterialItem MetalOuter5 => Instance[(short)18];

		public static MaterialItem MetalOuter6 => Instance[(short)19];

		public static MaterialItem MetalOuter7 => Instance[(short)20];

		public static MaterialItem MetalInner1 => Instance[(short)21];

		public static MaterialItem MetalInner2 => Instance[(short)22];

		public static MaterialItem MetalInner3 => Instance[(short)23];

		public static MaterialItem MetalInner4 => Instance[(short)24];

		public static MaterialItem MetalInner5 => Instance[(short)25];

		public static MaterialItem MetalInner6 => Instance[(short)26];

		public static MaterialItem MetalInner7 => Instance[(short)27];

		public static MaterialItem JadeOuter1 => Instance[(short)28];

		public static MaterialItem JadeOuter2 => Instance[(short)29];

		public static MaterialItem JadeOuter3 => Instance[(short)30];

		public static MaterialItem JadeOuter4 => Instance[(short)31];

		public static MaterialItem JadeOuter5 => Instance[(short)32];

		public static MaterialItem JadeOuter6 => Instance[(short)33];

		public static MaterialItem JadeOuter7 => Instance[(short)34];

		public static MaterialItem JadeInner1 => Instance[(short)35];

		public static MaterialItem JadeInner2 => Instance[(short)36];

		public static MaterialItem JadeInner3 => Instance[(short)37];

		public static MaterialItem JadeInner4 => Instance[(short)38];

		public static MaterialItem JadeInner5 => Instance[(short)39];

		public static MaterialItem JadeInner6 => Instance[(short)40];

		public static MaterialItem JadeInner7 => Instance[(short)41];

		public static MaterialItem FabricOuter1 => Instance[(short)42];

		public static MaterialItem FabricOuter2 => Instance[(short)43];

		public static MaterialItem FabricOuter3 => Instance[(short)44];

		public static MaterialItem FabricOuter4 => Instance[(short)45];

		public static MaterialItem FabricOuter5 => Instance[(short)46];

		public static MaterialItem FabricOuter6 => Instance[(short)47];

		public static MaterialItem FabricOuter7 => Instance[(short)48];

		public static MaterialItem FabricInner1 => Instance[(short)49];

		public static MaterialItem FabricInner2 => Instance[(short)50];

		public static MaterialItem FabricInner3 => Instance[(short)51];

		public static MaterialItem FabricInner4 => Instance[(short)52];

		public static MaterialItem FabricInner5 => Instance[(short)53];

		public static MaterialItem FabricInner6 => Instance[(short)54];

		public static MaterialItem FabricInner7 => Instance[(short)55];

		public static MaterialItem CookingBird0 => Instance[(short)56];

		public static MaterialItem CookingBird1 => Instance[(short)57];

		public static MaterialItem CookingBird2 => Instance[(short)58];

		public static MaterialItem CookingBird3 => Instance[(short)59];

		public static MaterialItem CookingBird4 => Instance[(short)60];

		public static MaterialItem CookingBird5 => Instance[(short)61];

		public static MaterialItem CookingBird6 => Instance[(short)62];

		public static MaterialItem CookingBeast0 => Instance[(short)63];

		public static MaterialItem CookingBeast1 => Instance[(short)64];

		public static MaterialItem CookingBeast2 => Instance[(short)65];

		public static MaterialItem CookingBeast3 => Instance[(short)66];

		public static MaterialItem CookingBeast4 => Instance[(short)67];

		public static MaterialItem CookingBeast5 => Instance[(short)68];

		public static MaterialItem CookingBeast6 => Instance[(short)69];

		public static MaterialItem CookingVegetarian0 => Instance[(short)70];

		public static MaterialItem CookingVegetarian1 => Instance[(short)71];

		public static MaterialItem CookingVegetarian2 => Instance[(short)72];

		public static MaterialItem CookingVegetarian3 => Instance[(short)73];

		public static MaterialItem CookingVegetarian4 => Instance[(short)74];

		public static MaterialItem CookingVegetarian5 => Instance[(short)75];

		public static MaterialItem CookingVegetarian6 => Instance[(short)76];

		public static MaterialItem CookingFish0 => Instance[(short)77];

		public static MaterialItem CookingFish1 => Instance[(short)78];

		public static MaterialItem CookingFish2 => Instance[(short)79];

		public static MaterialItem CookingFish3 => Instance[(short)80];

		public static MaterialItem CookingFish4 => Instance[(short)81];

		public static MaterialItem CookingFish5 => Instance[(short)82];

		public static MaterialItem CookingFish6 => Instance[(short)83];

		public static MaterialItem MedicineOuterInjury1 => Instance[(short)140];

		public static MaterialItem MedicineOuterInjury3 => Instance[(short)141];

		public static MaterialItem MedicineOuterInjury5 => Instance[(short)142];

		public static MaterialItem MedicineOuterInjury7 => Instance[(short)143];

		public static MaterialItem MedicinePoisonRed1 => Instance[(short)144];

		public static MaterialItem MedicinePoisonRed3 => Instance[(short)145];

		public static MaterialItem MedicinePoisonRed5 => Instance[(short)146];

		public static MaterialItem MedicinePoisonRed7 => Instance[(short)147];

		public static MaterialItem MedicinePenetrateResistOfOuter1 => Instance[(short)148];

		public static MaterialItem MedicinePenetrateResistOfOuter3 => Instance[(short)149];

		public static MaterialItem MedicinePenetrateResistOfOuter5 => Instance[(short)150];

		public static MaterialItem MedicinePenetrateResistOfOuter7 => Instance[(short)151];

		public static MaterialItem MedicineStrength1 => Instance[(short)152];

		public static MaterialItem MedicineStrength3 => Instance[(short)153];

		public static MaterialItem MedicineStrength5 => Instance[(short)154];

		public static MaterialItem MedicineStrength7 => Instance[(short)155];

		public static MaterialItem MedicineInnerInjury1 => Instance[(short)156];

		public static MaterialItem MedicineInnerInjury3 => Instance[(short)157];

		public static MaterialItem MedicineInnerInjury5 => Instance[(short)158];

		public static MaterialItem MedicineInnerInjury7 => Instance[(short)159];

		public static MaterialItem MedicinePoisonGloomy1 => Instance[(short)160];

		public static MaterialItem MedicinePoisonGloomy3 => Instance[(short)161];

		public static MaterialItem MedicinePoisonGloomy5 => Instance[(short)162];

		public static MaterialItem MedicinePoisonGloomy7 => Instance[(short)163];

		public static MaterialItem MedicinePenetrateResistOfInner1 => Instance[(short)164];

		public static MaterialItem MedicinePenetrateResistOfInner3 => Instance[(short)165];

		public static MaterialItem MedicinePenetrateResistOfInner5 => Instance[(short)166];

		public static MaterialItem MedicinePenetrateResistOfInner7 => Instance[(short)167];

		public static MaterialItem MedicineRecoverOfBreath1 => Instance[(short)168];

		public static MaterialItem MedicineRecoverOfBreath3 => Instance[(short)169];

		public static MaterialItem MedicineRecoverOfBreath5 => Instance[(short)170];

		public static MaterialItem MedicineRecoverOfBreath7 => Instance[(short)171];

		public static MaterialItem MedicineDisorderOfQi1 => Instance[(short)172];

		public static MaterialItem MedicineDisorderOfQi3 => Instance[(short)173];

		public static MaterialItem MedicineDisorderOfQi5 => Instance[(short)174];

		public static MaterialItem MedicineDisorderOfQi7 => Instance[(short)175];

		public static MaterialItem MedicinePoisonCold1 => Instance[(short)176];

		public static MaterialItem MedicinePoisonCold3 => Instance[(short)177];

		public static MaterialItem MedicinePoisonCold5 => Instance[(short)178];

		public static MaterialItem MedicinePoisonCold7 => Instance[(short)179];

		public static MaterialItem MedicineAvoidRateStr1 => Instance[(short)180];

		public static MaterialItem MedicineAvoidRateStr3 => Instance[(short)181];

		public static MaterialItem MedicineAvoidRateStr5 => Instance[(short)182];

		public static MaterialItem MedicineAvoidRateStr7 => Instance[(short)183];

		public static MaterialItem MedicineHitRateTechnique1 => Instance[(short)184];

		public static MaterialItem MedicineHitRateTechnique3 => Instance[(short)185];

		public static MaterialItem MedicineHitRateTechnique5 => Instance[(short)186];

		public static MaterialItem MedicineHitRateTechnique7 => Instance[(short)187];

		public static MaterialItem MedicineHealth1 => Instance[(short)188];

		public static MaterialItem MedicineHealth3 => Instance[(short)189];

		public static MaterialItem MedicineHealth5 => Instance[(short)190];

		public static MaterialItem MedicineHealth7 => Instance[(short)191];

		public static MaterialItem MedicinePoisonIllusory1 => Instance[(short)192];

		public static MaterialItem MedicinePoisonIllusory3 => Instance[(short)193];

		public static MaterialItem MedicinePoisonIllusory5 => Instance[(short)194];

		public static MaterialItem MedicinePoisonIllusory7 => Instance[(short)195];

		public static MaterialItem MedicineAvoidRateTech1 => Instance[(short)196];

		public static MaterialItem MedicineAvoidRateTech3 => Instance[(short)197];

		public static MaterialItem MedicineAvoidRateTech5 => Instance[(short)198];

		public static MaterialItem MedicineAvoidRateTech7 => Instance[(short)199];

		public static MaterialItem MedicineRecoverOfStance1 => Instance[(short)200];

		public static MaterialItem MedicineRecoverOfStance3 => Instance[(short)201];

		public static MaterialItem MedicineRecoverOfStance5 => Instance[(short)202];

		public static MaterialItem MedicineRecoverOfStance7 => Instance[(short)203];

		public static MaterialItem MedicineRecoverOtherA1 => Instance[(short)204];

		public static MaterialItem MedicineRecoverOtherA3 => Instance[(short)205];

		public static MaterialItem MedicineRecoverOtherA5 => Instance[(short)206];

		public static MaterialItem MedicineRecoverOtherA7 => Instance[(short)207];

		public static MaterialItem MedicinePoisonRotten1 => Instance[(short)208];

		public static MaterialItem MedicinePoisonRotten3 => Instance[(short)209];

		public static MaterialItem MedicinePoisonRotten5 => Instance[(short)210];

		public static MaterialItem MedicinePoisonRotten7 => Instance[(short)211];

		public static MaterialItem MedicineAvoidRateSpeed1 => Instance[(short)212];

		public static MaterialItem MedicineAvoidRateSpeed3 => Instance[(short)213];

		public static MaterialItem MedicineAvoidRateSpeed5 => Instance[(short)214];

		public static MaterialItem MedicineAvoidRateSpeed7 => Instance[(short)215];

		public static MaterialItem MedicineHitRateSpeed1 => Instance[(short)216];

		public static MaterialItem MedicineHitRateSpeed3 => Instance[(short)217];

		public static MaterialItem MedicineHitRateSpeed5 => Instance[(short)218];

		public static MaterialItem MedicineHitRateSpeed7 => Instance[(short)219];

		public static MaterialItem MedicineRecoverAttackA1 => Instance[(short)220];

		public static MaterialItem MedicineRecoverAttackA3 => Instance[(short)221];

		public static MaterialItem MedicineRecoverAttackA5 => Instance[(short)222];

		public static MaterialItem MedicineRecoverAttackA7 => Instance[(short)223];

		public static MaterialItem MedicinePoisonHot1 => Instance[(short)224];

		public static MaterialItem MedicinePoisonHot3 => Instance[(short)225];

		public static MaterialItem MedicinePoisonHot5 => Instance[(short)226];

		public static MaterialItem MedicinePoisonHot7 => Instance[(short)227];

		public static MaterialItem MedicineRecoverOtherB1 => Instance[(short)228];

		public static MaterialItem MedicineRecoverOtherB3 => Instance[(short)229];

		public static MaterialItem MedicineRecoverOtherB5 => Instance[(short)230];

		public static MaterialItem MedicineRecoverOtherB7 => Instance[(short)231];

		public static MaterialItem MedicineRecoverAttackB1 => Instance[(short)232];

		public static MaterialItem MedicineRecoverAttackB3 => Instance[(short)233];

		public static MaterialItem MedicineRecoverAttackB5 => Instance[(short)234];

		public static MaterialItem MedicineRecoverAttackB7 => Instance[(short)235];

		public static MaterialItem PoisonHot1 => Instance[(short)236];

		public static MaterialItem PoisonHot2 => Instance[(short)237];

		public static MaterialItem PoisonHot3 => Instance[(short)238];

		public static MaterialItem PoisonHot4 => Instance[(short)239];

		public static MaterialItem PoisonHot5 => Instance[(short)240];

		public static MaterialItem PoisonHot6 => Instance[(short)241];

		public static MaterialItem PoisonHot7 => Instance[(short)242];

		public static MaterialItem PoisonGloomy1 => Instance[(short)243];

		public static MaterialItem PoisonGloomy2 => Instance[(short)244];

		public static MaterialItem PoisonGloomy3 => Instance[(short)245];

		public static MaterialItem PoisonGloomy4 => Instance[(short)246];

		public static MaterialItem PoisonGloomy5 => Instance[(short)247];

		public static MaterialItem PoisonGloomy6 => Instance[(short)248];

		public static MaterialItem PoisonGloomy7 => Instance[(short)249];

		public static MaterialItem PoisonCold1 => Instance[(short)250];

		public static MaterialItem PoisonCold2 => Instance[(short)251];

		public static MaterialItem PoisonCold3 => Instance[(short)252];

		public static MaterialItem PoisonCold4 => Instance[(short)253];

		public static MaterialItem PoisonCold5 => Instance[(short)254];

		public static MaterialItem PoisonCold6 => Instance[(short)255];

		public static MaterialItem PoisonCold7 => Instance[(short)256];

		public static MaterialItem PoisonRed1 => Instance[(short)257];

		public static MaterialItem PoisonRed2 => Instance[(short)258];

		public static MaterialItem PoisonRed3 => Instance[(short)259];

		public static MaterialItem PoisonRed4 => Instance[(short)260];

		public static MaterialItem PoisonRed5 => Instance[(short)261];

		public static MaterialItem PoisonRed6 => Instance[(short)262];

		public static MaterialItem PoisonRed7 => Instance[(short)263];

		public static MaterialItem PoisonRotten1 => Instance[(short)264];

		public static MaterialItem PoisonRotten2 => Instance[(short)265];

		public static MaterialItem PoisonRotten3 => Instance[(short)266];

		public static MaterialItem PoisonRotten4 => Instance[(short)267];

		public static MaterialItem PoisonRotten5 => Instance[(short)268];

		public static MaterialItem PoisonRotten6 => Instance[(short)269];

		public static MaterialItem PoisonRotten7 => Instance[(short)270];

		public static MaterialItem PoisonIllusory1 => Instance[(short)271];

		public static MaterialItem PoisonIllusory2 => Instance[(short)272];

		public static MaterialItem PoisonIllusory3 => Instance[(short)273];

		public static MaterialItem PoisonIllusory4 => Instance[(short)274];

		public static MaterialItem PoisonIllusory5 => Instance[(short)275];

		public static MaterialItem PoisonIllusory6 => Instance[(short)276];

		public static MaterialItem PoisonIllusory7 => Instance[(short)277];

		public static MaterialItem GreenBambooPiece => Instance[(short)279];

		public static MaterialItem JiaoWhiteEgg => Instance[(short)280];

		public static MaterialItem JiaoBlackEgg => Instance[(short)281];

		public static MaterialItem JiaoGreenEgg => Instance[(short)282];

		public static MaterialItem JiaoRedEgg => Instance[(short)283];

		public static MaterialItem JiaoYellowEgg => Instance[(short)284];

		public static MaterialItem JiaoWBEgg => Instance[(short)285];

		public static MaterialItem JiaoWGEgg => Instance[(short)286];

		public static MaterialItem JiaoWREgg => Instance[(short)287];

		public static MaterialItem JiaoWYEgg => Instance[(short)288];

		public static MaterialItem JiaoBGEgg => Instance[(short)289];

		public static MaterialItem JiaoBREgg => Instance[(short)290];

		public static MaterialItem JiaoBYEgg => Instance[(short)291];

		public static MaterialItem JiaoGREgg => Instance[(short)292];

		public static MaterialItem JiaoGYEgg => Instance[(short)293];

		public static MaterialItem JiaoRYEgg => Instance[(short)294];

		public static MaterialItem JiaoWBGEgg => Instance[(short)295];

		public static MaterialItem JiaoWBREgg => Instance[(short)296];

		public static MaterialItem JiaoWBYEgg => Instance[(short)297];

		public static MaterialItem JiaoWGREgg => Instance[(short)298];

		public static MaterialItem JiaoWGYEgg => Instance[(short)299];

		public static MaterialItem JiaoWRYEgg => Instance[(short)300];

		public static MaterialItem JiaoBGREgg => Instance[(short)301];

		public static MaterialItem JiaoBGYEgg => Instance[(short)302];

		public static MaterialItem JiaoBRYEgg => Instance[(short)303];

		public static MaterialItem JiaoGRYEgg => Instance[(short)304];

		public static MaterialItem JiaoWBGREgg => Instance[(short)305];

		public static MaterialItem JiaoWBGYEgg => Instance[(short)306];

		public static MaterialItem JiaoWBRYEgg => Instance[(short)307];

		public static MaterialItem JiaoWGRYEgg => Instance[(short)308];

		public static MaterialItem JiaoBGRYEgg => Instance[(short)309];

		public static MaterialItem JiaoWGRYBEgg => Instance[(short)310];

		public static MaterialItem JiaoWhite => Instance[(short)311];

		public static MaterialItem JiaoBlack => Instance[(short)312];

		public static MaterialItem JiaoGreen => Instance[(short)313];

		public static MaterialItem JiaoRed => Instance[(short)314];

		public static MaterialItem JiaoYellow => Instance[(short)315];

		public static MaterialItem JiaoWB => Instance[(short)316];

		public static MaterialItem JiaoWG => Instance[(short)317];

		public static MaterialItem JiaoWR => Instance[(short)318];

		public static MaterialItem JiaoWY => Instance[(short)319];

		public static MaterialItem JiaoBG => Instance[(short)320];

		public static MaterialItem JiaoBR => Instance[(short)321];

		public static MaterialItem JiaoBY => Instance[(short)322];

		public static MaterialItem JiaoGR => Instance[(short)323];

		public static MaterialItem JiaoGY => Instance[(short)324];

		public static MaterialItem JiaoRY => Instance[(short)325];

		public static MaterialItem JiaoWBG => Instance[(short)326];

		public static MaterialItem JiaoWBR => Instance[(short)327];

		public static MaterialItem JiaoWBY => Instance[(short)328];

		public static MaterialItem JiaoWGR => Instance[(short)329];

		public static MaterialItem JiaoWGY => Instance[(short)330];

		public static MaterialItem JiaoWRY => Instance[(short)331];

		public static MaterialItem JiaoBGR => Instance[(short)332];

		public static MaterialItem JiaoBGY => Instance[(short)333];

		public static MaterialItem JiaoBRY => Instance[(short)334];

		public static MaterialItem JiaoGRY => Instance[(short)335];

		public static MaterialItem JiaoWBGR => Instance[(short)336];

		public static MaterialItem JiaoWBGY => Instance[(short)337];

		public static MaterialItem JiaoWBRY => Instance[(short)338];

		public static MaterialItem JiaoWGRY => Instance[(short)339];

		public static MaterialItem JiaoBGRY => Instance[(short)340];

		public static MaterialItem JiaoWGRYB => Instance[(short)341];

		public static MaterialItem SectStoryFulongFeathers => Instance[(short)342];
	}

	public static Material Instance = new Material();

	private readonly HashSet<string> RequiredFields = new HashSet<string>
	{
		"ItemSubType", "GroupId", "ResourceType", "BreakBonusEffect", "RefiningEffect", "RequiredLifeSkillType", "CraftableItemTypes", "DisassembleResultItemList", "PrimaryEffectType", "PrimaryEffectSubType",
		"SecondaryEffectType", "SecondaryEffectSubType", "TemplateId", "Name", "Grade", "Icon", "Desc", "BaseWeight", "ResourceAmount", "RequiredAttainment"
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
		_dataArray.Add(new MaterialItem(0, 0, 5, 501, 1, 0, "icon_Material_baiyumu", 1, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 0, 60, 200, 300, 0, 2, 600, 3, allowRandomCreate: true, 40, isSpecial: false, 1, 36, EMaterialProperty.Invalid, 30, -1, 50, 7, 30, 5, new List<short>
		{
			5, 9, 15, 21, 27, 33, 41, 47, 53, 59,
			65, 70, 74, 79, 87, 95, 103, 111, 120
		}, new PoisonsAndLevels(default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short)), new List<PresetInventoryItem>(), 0, 1, 0, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, primaryRecoverAllInjuries: false, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, secondaryRecoverAllInjuries: false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 60, EMaterialFilterType.Invalid, EMaterialFilterHardness.Wooden));
		_dataArray.Add(new MaterialItem(1, 2, 5, 501, 2, 0, "icon_Material_baiyumu", 3, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 0, 120, 600, 900, 0, 3, 1200, 4, allowRandomCreate: true, 35, isSpecial: false, 1, 36, EMaterialProperty.Invalid, 30, -1, 100, 7, 60, 10, new List<short>
		{
			5, 9, 15, 21, 27, 33, 41, 47, 53, 59,
			65, 70, 74, 79, 87, 95, 103, 111, 120
		}, new PoisonsAndLevels(default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short)), new List<PresetInventoryItem>(), 0, 1, 0, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, primaryRecoverAllInjuries: false, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, secondaryRecoverAllInjuries: false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 60, EMaterialFilterType.Invalid, EMaterialFilterHardness.Wooden));
		_dataArray.Add(new MaterialItem(2, 4, 5, 501, 3, 0, "icon_Material_qifengwutong", 5, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 0, 80, 1500, 2250, 1, 4, 1800, 5, allowRandomCreate: true, 30, isSpecial: false, 1, 36, EMaterialProperty.Invalid, 30, -1, 200, 7, 100, 20, new List<short>
		{
			5, 9, 15, 21, 27, 33, 41, 47, 53, 59,
			65, 70, 74, 79, 87, 95, 103, 111, 120
		}, new PoisonsAndLevels(default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short)), new List<PresetInventoryItem>(), 0, 1, 0, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, primaryRecoverAllInjuries: false, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, secondaryRecoverAllInjuries: false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 60, EMaterialFilterType.Invalid, EMaterialFilterHardness.Wooden));
		_dataArray.Add(new MaterialItem(3, 6, 5, 501, 4, 0, "icon_Material_qifengwutong", 7, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 0, 100, 3100, 4650, 2, 5, 3000, 6, allowRandomCreate: true, 25, isSpecial: false, 1, 36, EMaterialProperty.Invalid, 30, -1, 400, 7, 150, 40, new List<short>
		{
			5, 9, 15, 21, 27, 33, 41, 47, 53, 59,
			65, 70, 74, 79, 87, 95, 103, 111, 120
		}, new PoisonsAndLevels(default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short)), new List<PresetInventoryItem>(), 0, 1, 0, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, primaryRecoverAllInjuries: false, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, secondaryRecoverAllInjuries: false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 60, EMaterialFilterType.Invalid, EMaterialFilterHardness.Wooden));
		_dataArray.Add(new MaterialItem(4, 8, 5, 501, 5, 0, "icon_Material_qifengwutong", 9, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 0, 120, 5600, 8400, 3, 6, 4200, 7, allowRandomCreate: true, 20, isSpecial: false, 1, 36, EMaterialProperty.Invalid, 30, -1, 600, 7, 210, 80, new List<short>
		{
			5, 9, 15, 21, 27, 33, 41, 47, 53, 59,
			65, 70, 74, 79, 87, 95, 103, 111, 120
		}, new PoisonsAndLevels(default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short)), new List<PresetInventoryItem>(), 0, 1, 0, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, primaryRecoverAllInjuries: false, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, secondaryRecoverAllInjuries: false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 60, EMaterialFilterType.Invalid, EMaterialFilterHardness.Wooden));
		_dataArray.Add(new MaterialItem(5, 10, 5, 501, 6, 0, "icon_Material_qianjiehuanghuali", 11, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 0, 80, 9200, 13800, 4, 7, 5400, 7, allowRandomCreate: true, 15, isSpecial: false, 1, 36, EMaterialProperty.Invalid, 30, -1, 800, 7, 280, 160, new List<short>
		{
			5, 9, 15, 21, 27, 33, 41, 47, 53, 59,
			65, 70, 74, 79, 87, 95, 103, 111, 120
		}, new PoisonsAndLevels(default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short)), new List<PresetInventoryItem>(), 0, 1, 0, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, primaryRecoverAllInjuries: false, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, secondaryRecoverAllInjuries: false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 60, EMaterialFilterType.Invalid, EMaterialFilterHardness.Wooden));
		_dataArray.Add(new MaterialItem(6, 12, 5, 501, 7, 0, "icon_Material_wujinzitan", 13, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 0, 100, 14100, 21150, 5, 8, 7200, 8, allowRandomCreate: true, 10, isSpecial: false, 1, 36, EMaterialProperty.Invalid, 30, -1, 1000, 7, 360, 320, new List<short>
		{
			5, 9, 15, 21, 27, 33, 41, 47, 53, 59,
			65, 70, 74, 79, 87, 95, 103, 111, 120
		}, new PoisonsAndLevels(default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short)), new List<PresetInventoryItem>(), 0, 1, 0, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, primaryRecoverAllInjuries: false, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, secondaryRecoverAllInjuries: false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 60, EMaterialFilterType.Invalid, EMaterialFilterHardness.Wooden));
		_dataArray.Add(new MaterialItem(7, 14, 5, 501, 1, 7, "icon_Material_mengzongzhu", 15, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 0, 50, 200, 300, 0, 2, 600, 3, allowRandomCreate: true, 40, isSpecial: false, 1, 36, EMaterialProperty.Invalid, 30, -1, 50, 7, 30, 5, new List<short>
		{
			12, 18, 24, 30, 37, 44, 50, 56, 62, 68,
			72, 76, 83, 91, 99, 107, 115, 121
		}, new PoisonsAndLevels(default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short)), new List<PresetInventoryItem>(), 0, 1, 0, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, primaryRecoverAllInjuries: false, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, secondaryRecoverAllInjuries: false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 60, EMaterialFilterType.Invalid, EMaterialFilterHardness.Rattan));
		_dataArray.Add(new MaterialItem(8, 16, 5, 501, 2, 7, "icon_Material_mengzongzhu", 17, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 0, 40, 600, 900, 0, 3, 1200, 4, allowRandomCreate: true, 35, isSpecial: false, 1, 36, EMaterialProperty.Invalid, 30, -1, 100, 7, 60, 10, new List<short>
		{
			12, 18, 24, 30, 37, 44, 50, 56, 62, 68,
			72, 76, 83, 91, 99, 107, 115, 121
		}, new PoisonsAndLevels(default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short)), new List<PresetInventoryItem>(), 0, 1, 0, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, primaryRecoverAllInjuries: false, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, secondaryRecoverAllInjuries: false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 60, EMaterialFilterType.Invalid, EMaterialFilterHardness.Rattan));
		_dataArray.Add(new MaterialItem(9, 18, 5, 501, 3, 7, "icon_Material_qingputizhi", 19, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 0, 30, 1500, 2250, 1, 4, 1800, 5, allowRandomCreate: true, 30, isSpecial: false, 1, 36, EMaterialProperty.Invalid, 30, -1, 200, 7, 100, 20, new List<short>
		{
			12, 18, 24, 30, 37, 44, 50, 56, 62, 68,
			72, 76, 83, 91, 99, 107, 115, 121
		}, new PoisonsAndLevels(default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short)), new List<PresetInventoryItem>(), 0, 1, 0, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, primaryRecoverAllInjuries: false, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, secondaryRecoverAllInjuries: false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 60, EMaterialFilterType.Invalid, EMaterialFilterHardness.Rattan));
		_dataArray.Add(new MaterialItem(10, 20, 5, 501, 4, 7, "icon_Material_qingputizhi", 21, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 0, 100, 3100, 4650, 2, 5, 3000, 6, allowRandomCreate: true, 25, isSpecial: false, 1, 36, EMaterialProperty.Invalid, 30, -1, 400, 7, 150, 40, new List<short>
		{
			12, 18, 24, 30, 37, 44, 50, 56, 62, 68,
			72, 76, 83, 91, 99, 107, 115, 121
		}, new PoisonsAndLevels(default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short)), new List<PresetInventoryItem>(), 0, 1, 0, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, primaryRecoverAllInjuries: false, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, secondaryRecoverAllInjuries: false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 60, EMaterialFilterType.Invalid, EMaterialFilterHardness.Rattan));
		_dataArray.Add(new MaterialItem(11, 22, 5, 501, 5, 7, "icon_Material_qingputizhi", 23, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 0, 50, 5600, 8400, 3, 6, 4200, 7, allowRandomCreate: true, 20, isSpecial: false, 1, 36, EMaterialProperty.Invalid, 30, -1, 600, 7, 210, 80, new List<short>
		{
			12, 18, 24, 30, 37, 44, 50, 56, 62, 68,
			72, 76, 83, 91, 99, 107, 115, 121
		}, new PoisonsAndLevels(default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short)), new List<PresetInventoryItem>(), 0, 1, 0, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, primaryRecoverAllInjuries: false, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, secondaryRecoverAllInjuries: false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 60, EMaterialFilterType.Invalid, EMaterialFilterHardness.Rattan));
		_dataArray.Add(new MaterialItem(12, 24, 5, 501, 6, 7, "icon_Material_huotaomu", 25, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 0, 60, 9200, 13800, 4, 7, 5400, 7, allowRandomCreate: true, 15, isSpecial: false, 1, 36, EMaterialProperty.Invalid, 30, -1, 800, 7, 280, 160, new List<short>
		{
			12, 18, 24, 30, 37, 44, 50, 56, 62, 68,
			72, 76, 83, 91, 99, 107, 115, 121
		}, new PoisonsAndLevels(default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short)), new List<PresetInventoryItem>(), 0, 1, 0, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, primaryRecoverAllInjuries: false, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, secondaryRecoverAllInjuries: false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 60, EMaterialFilterType.Invalid, EMaterialFilterHardness.Rattan));
		_dataArray.Add(new MaterialItem(13, 26, 5, 501, 7, 7, "icon_Material_jiuquzizhu", 27, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 0, 50, 14100, 21150, 5, 8, 7200, 8, allowRandomCreate: true, 10, isSpecial: false, 1, 36, EMaterialProperty.Invalid, 30, -1, 1000, 7, 360, 320, new List<short>
		{
			12, 18, 24, 30, 37, 44, 50, 56, 62, 68,
			72, 76, 83, 91, 99, 107, 115, 121
		}, new PoisonsAndLevels(default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short)), new List<PresetInventoryItem>(), 0, 1, 0, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, primaryRecoverAllInjuries: false, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, secondaryRecoverAllInjuries: false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 60, EMaterialFilterType.Invalid, EMaterialFilterHardness.Rattan));
		_dataArray.Add(new MaterialItem(14, 28, 5, 502, 1, 14, "icon_Material_bintie", 29, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 0, 100, 200, 300, 0, 2, 600, 3, allowRandomCreate: true, 40, isSpecial: false, 2, 36, EMaterialProperty.Invalid, 29, -1, 50, 6, 30, 5, new List<short>
		{
			4, 8, 14, 20, 26, 32, 40, 46, 52, 58,
			64, 78, 86, 94, 102, 110
		}, new PoisonsAndLevels(default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short)), new List<PresetInventoryItem>(), 0, 1, 0, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, primaryRecoverAllInjuries: false, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, secondaryRecoverAllInjuries: false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 60, EMaterialFilterType.Invalid, EMaterialFilterHardness.Icon));
		_dataArray.Add(new MaterialItem(15, 30, 5, 502, 2, 14, "icon_Material_bintie", 31, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 0, 150, 600, 900, 0, 3, 1200, 4, allowRandomCreate: true, 35, isSpecial: false, 2, 36, EMaterialProperty.Invalid, 29, -1, 100, 6, 60, 10, new List<short>
		{
			4, 8, 14, 20, 26, 32, 40, 46, 52, 58,
			64, 78, 86, 94, 102, 110
		}, new PoisonsAndLevels(default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short)), new List<PresetInventoryItem>(), 0, 1, 0, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, primaryRecoverAllInjuries: false, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, secondaryRecoverAllInjuries: false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 60, EMaterialFilterType.Invalid, EMaterialFilterHardness.Icon));
		_dataArray.Add(new MaterialItem(16, 32, 5, 502, 3, 14, "icon_Material_baiyuejingtie", 33, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 0, 180, 1500, 2250, 1, 4, 1800, 5, allowRandomCreate: true, 30, isSpecial: false, 2, 36, EMaterialProperty.Invalid, 29, -1, 200, 6, 100, 20, new List<short>
		{
			4, 8, 14, 20, 26, 32, 40, 46, 52, 58,
			64, 78, 86, 94, 102, 110
		}, new PoisonsAndLevels(default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short)), new List<PresetInventoryItem>(), 0, 1, 0, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, primaryRecoverAllInjuries: false, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, secondaryRecoverAllInjuries: false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 60, EMaterialFilterType.Invalid, EMaterialFilterHardness.Icon));
		_dataArray.Add(new MaterialItem(17, 34, 5, 502, 4, 14, "icon_Material_baiyuejingtie", 35, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 0, 120, 3100, 4650, 2, 5, 3000, 6, allowRandomCreate: true, 25, isSpecial: false, 2, 36, EMaterialProperty.Invalid, 29, -1, 400, 6, 150, 40, new List<short>
		{
			4, 8, 14, 20, 26, 32, 40, 46, 52, 58,
			64, 78, 86, 94, 102, 110
		}, new PoisonsAndLevels(default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short)), new List<PresetInventoryItem>(), 0, 1, 0, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, primaryRecoverAllInjuries: false, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, secondaryRecoverAllInjuries: false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 60, EMaterialFilterType.Invalid, EMaterialFilterHardness.Icon));
		_dataArray.Add(new MaterialItem(18, 36, 5, 502, 5, 14, "icon_Material_baiyuejingtie", 37, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 0, 200, 5600, 8400, 3, 6, 4200, 7, allowRandomCreate: true, 20, isSpecial: false, 2, 36, EMaterialProperty.Invalid, 29, -1, 600, 6, 210, 80, new List<short>
		{
			4, 8, 14, 20, 26, 32, 40, 46, 52, 58,
			64, 78, 86, 94, 102, 110
		}, new PoisonsAndLevels(default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short)), new List<PresetInventoryItem>(), 0, 1, 0, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, primaryRecoverAllInjuries: false, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, secondaryRecoverAllInjuries: false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 60, EMaterialFilterType.Invalid, EMaterialFilterHardness.Icon));
		_dataArray.Add(new MaterialItem(19, 38, 5, 502, 6, 14, "icon_Material_wuseshentie", 39, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 0, 280, 9200, 13800, 4, 7, 5400, 7, allowRandomCreate: true, 15, isSpecial: false, 2, 36, EMaterialProperty.Invalid, 29, -1, 800, 6, 280, 160, new List<short>
		{
			4, 8, 14, 20, 26, 32, 40, 46, 52, 58,
			64, 78, 86, 94, 102, 110
		}, new PoisonsAndLevels(default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short)), new List<PresetInventoryItem>(), 0, 1, 0, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, primaryRecoverAllInjuries: false, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, secondaryRecoverAllInjuries: false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 60, EMaterialFilterType.Invalid, EMaterialFilterHardness.Icon));
		_dataArray.Add(new MaterialItem(20, 40, 5, 502, 7, 14, "icon_Material_xuantie", 41, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 0, 360, 14100, 21150, 5, 8, 7200, 8, allowRandomCreate: true, 10, isSpecial: false, 2, 36, EMaterialProperty.Invalid, 29, -1, 1000, 6, 360, 320, new List<short>
		{
			4, 8, 14, 20, 26, 32, 40, 46, 52, 58,
			64, 78, 86, 94, 102, 110
		}, new PoisonsAndLevels(default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short)), new List<PresetInventoryItem>(), 0, 1, 0, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, primaryRecoverAllInjuries: false, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, secondaryRecoverAllInjuries: false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 60, EMaterialFilterType.Invalid, EMaterialFilterHardness.Icon));
		_dataArray.Add(new MaterialItem(21, 42, 5, 502, 1, 21, "icon_Material_yuantong", 43, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 0, 80, 200, 300, 0, 2, 600, 3, allowRandomCreate: true, 40, isSpecial: false, 2, 36, EMaterialProperty.Invalid, 29, -1, 50, 6, 30, 5, new List<short>
		{
			11, 17, 23, 29, 36, 43, 49, 55, 61, 67,
			82, 90, 98, 106, 114
		}, new PoisonsAndLevels(default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short)), new List<PresetInventoryItem>(), 0, 1, 0, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, primaryRecoverAllInjuries: false, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, secondaryRecoverAllInjuries: false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 60, EMaterialFilterType.Invalid, EMaterialFilterHardness.GoldSilver));
		_dataArray.Add(new MaterialItem(22, 44, 5, 502, 2, 21, "icon_Material_yuantong", 45, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 0, 60, 600, 900, 0, 3, 1200, 4, allowRandomCreate: true, 35, isSpecial: false, 2, 36, EMaterialProperty.Invalid, 29, -1, 100, 6, 60, 10, new List<short>
		{
			11, 17, 23, 29, 36, 43, 49, 55, 61, 67,
			82, 90, 98, 106, 114
		}, new PoisonsAndLevels(default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short)), new List<PresetInventoryItem>(), 0, 1, 0, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, primaryRecoverAllInjuries: false, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, secondaryRecoverAllInjuries: false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 60, EMaterialFilterType.Invalid, EMaterialFilterHardness.GoldSilver));
		_dataArray.Add(new MaterialItem(23, 46, 5, 502, 3, 21, "icon_Material_zijin", 47, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 0, 120, 1500, 2250, 1, 4, 1800, 5, allowRandomCreate: true, 30, isSpecial: false, 2, 36, EMaterialProperty.Invalid, 29, -1, 200, 6, 100, 20, new List<short>
		{
			11, 17, 23, 29, 36, 43, 49, 55, 61, 67,
			82, 90, 98, 106, 114
		}, new PoisonsAndLevels(default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short)), new List<PresetInventoryItem>(), 0, 1, 0, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, primaryRecoverAllInjuries: false, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, secondaryRecoverAllInjuries: false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 60, EMaterialFilterType.Invalid, EMaterialFilterHardness.GoldSilver));
		_dataArray.Add(new MaterialItem(24, 48, 5, 502, 4, 21, "icon_Material_zijin", 49, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 0, 140, 3100, 4650, 2, 5, 3000, 6, allowRandomCreate: true, 25, isSpecial: false, 2, 36, EMaterialProperty.Invalid, 29, -1, 400, 6, 150, 40, new List<short>
		{
			11, 17, 23, 29, 36, 43, 49, 55, 61, 67,
			82, 90, 98, 106, 114
		}, new PoisonsAndLevels(default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short)), new List<PresetInventoryItem>(), 0, 1, 0, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, primaryRecoverAllInjuries: false, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, secondaryRecoverAllInjuries: false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 60, EMaterialFilterType.Invalid, EMaterialFilterHardness.GoldSilver));
		_dataArray.Add(new MaterialItem(25, 50, 5, 502, 5, 21, "icon_Material_zijin", 51, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 0, 180, 5600, 8400, 3, 6, 4200, 7, allowRandomCreate: true, 20, isSpecial: false, 2, 36, EMaterialProperty.Invalid, 29, -1, 600, 6, 210, 80, new List<short>
		{
			11, 17, 23, 29, 36, 43, 49, 55, 61, 67,
			82, 90, 98, 106, 114
		}, new PoisonsAndLevels(default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short)), new List<PresetInventoryItem>(), 0, 1, 0, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, primaryRecoverAllInjuries: false, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, secondaryRecoverAllInjuries: false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 60, EMaterialFilterType.Invalid, EMaterialFilterHardness.GoldSilver));
		_dataArray.Add(new MaterialItem(26, 52, 5, 502, 6, 21, "icon_Material_shiercaixiayin", 53, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 0, 50, 9200, 13800, 4, 7, 5400, 7, allowRandomCreate: true, 15, isSpecial: false, 2, 36, EMaterialProperty.Invalid, 29, -1, 800, 6, 280, 160, new List<short>
		{
			11, 17, 23, 29, 36, 43, 49, 55, 61, 67,
			82, 90, 98, 106, 114
		}, new PoisonsAndLevels(default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short)), new List<PresetInventoryItem>(), 0, 1, 0, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, primaryRecoverAllInjuries: false, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, secondaryRecoverAllInjuries: false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 60, EMaterialFilterType.Invalid, EMaterialFilterHardness.GoldSilver));
		_dataArray.Add(new MaterialItem(27, 54, 5, 502, 7, 21, "icon_Material_chankejingjin", 55, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 0, 40, 14100, 21150, 5, 8, 7200, 8, allowRandomCreate: true, 10, isSpecial: false, 2, 36, EMaterialProperty.Invalid, 29, -1, 1000, 6, 360, 320, new List<short>
		{
			11, 17, 23, 29, 36, 43, 49, 55, 61, 67,
			82, 90, 98, 106, 114
		}, new PoisonsAndLevels(default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short)), new List<PresetInventoryItem>(), 0, 1, 0, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, primaryRecoverAllInjuries: false, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, secondaryRecoverAllInjuries: false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 60, EMaterialFilterType.Invalid, EMaterialFilterHardness.GoldSilver));
		_dataArray.Add(new MaterialItem(28, 56, 5, 503, 1, 28, "icon_Material_heimanao", 57, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 0, 20, 200, 300, 0, 2, 600, 3, allowRandomCreate: true, 40, isSpecial: false, 3, 36, EMaterialProperty.Invalid, 31, -1, 50, 11, 30, 5, new List<short>
		{
			10, 16, 22, 28, 34, 42, 48, 54, 60, 66,
			80, 88, 96, 104, 112
		}, new PoisonsAndLevels(default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short)), new List<PresetInventoryItem>(), 0, 1, 0, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, primaryRecoverAllInjuries: false, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, secondaryRecoverAllInjuries: false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 60, EMaterialFilterType.Invalid, EMaterialFilterHardness.Stone));
		_dataArray.Add(new MaterialItem(29, 58, 5, 503, 2, 28, "icon_Material_heimanao", 59, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 0, 20, 600, 900, 0, 3, 1200, 4, allowRandomCreate: true, 35, isSpecial: false, 3, 36, EMaterialProperty.Invalid, 31, -1, 100, 11, 60, 10, new List<short>
		{
			10, 16, 22, 28, 34, 42, 48, 54, 60, 66,
			80, 88, 96, 104, 112
		}, new PoisonsAndLevels(default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short)), new List<PresetInventoryItem>(), 0, 1, 0, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, primaryRecoverAllInjuries: false, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, secondaryRecoverAllInjuries: false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 60, EMaterialFilterType.Invalid, EMaterialFilterHardness.Stone));
		_dataArray.Add(new MaterialItem(30, 60, 5, 503, 3, 28, "icon_Material_qingjindan", 61, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 0, 40, 1500, 2250, 1, 4, 1800, 5, allowRandomCreate: true, 30, isSpecial: false, 3, 36, EMaterialProperty.Invalid, 31, -1, 200, 11, 100, 20, new List<short>
		{
			10, 16, 22, 28, 34, 42, 48, 54, 60, 66,
			80, 88, 96, 104, 112
		}, new PoisonsAndLevels(default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short)), new List<PresetInventoryItem>(), 0, 1, 0, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, primaryRecoverAllInjuries: false, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, secondaryRecoverAllInjuries: false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 60, EMaterialFilterType.Invalid, EMaterialFilterHardness.Stone));
		_dataArray.Add(new MaterialItem(31, 62, 5, 503, 4, 28, "icon_Material_qingjindan", 63, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 0, 20, 3100, 4650, 2, 5, 3000, 6, allowRandomCreate: true, 25, isSpecial: false, 3, 36, EMaterialProperty.Invalid, 31, -1, 400, 11, 150, 40, new List<short>
		{
			10, 16, 22, 28, 34, 42, 48, 54, 60, 66,
			80, 88, 96, 104, 112
		}, new PoisonsAndLevels(default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short)), new List<PresetInventoryItem>(), 0, 1, 0, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, primaryRecoverAllInjuries: false, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, secondaryRecoverAllInjuries: false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 60, EMaterialFilterType.Invalid, EMaterialFilterHardness.Stone));
		_dataArray.Add(new MaterialItem(32, 64, 5, 503, 5, 28, "icon_Material_qingjindan", 65, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 0, 90, 5600, 8400, 3, 6, 4200, 7, allowRandomCreate: true, 20, isSpecial: false, 3, 36, EMaterialProperty.Invalid, 31, -1, 600, 11, 210, 80, new List<short>
		{
			10, 16, 22, 28, 34, 42, 48, 54, 60, 66,
			80, 88, 96, 104, 112
		}, new PoisonsAndLevels(default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short)), new List<PresetInventoryItem>(), 0, 1, 0, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, primaryRecoverAllInjuries: false, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, secondaryRecoverAllInjuries: false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 60, EMaterialFilterType.Invalid, EMaterialFilterHardness.Stone));
		_dataArray.Add(new MaterialItem(33, 66, 5, 503, 6, 28, "icon_Material_qingxiaoshendan", 67, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 0, 60, 9200, 13800, 4, 7, 5400, 7, allowRandomCreate: true, 15, isSpecial: false, 3, 36, EMaterialProperty.Invalid, 31, -1, 800, 11, 280, 160, new List<short>
		{
			10, 16, 22, 28, 34, 42, 48, 54, 60, 66,
			80, 88, 96, 104, 112
		}, new PoisonsAndLevels(default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short)), new List<PresetInventoryItem>(), 0, 1, 0, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, primaryRecoverAllInjuries: false, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, secondaryRecoverAllInjuries: false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 60, EMaterialFilterType.Invalid, EMaterialFilterHardness.Stone));
		_dataArray.Add(new MaterialItem(34, 68, 5, 503, 7, 28, "icon_Material_shenzhaodan", 69, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 0, 80, 14100, 21150, 5, 8, 7200, 8, allowRandomCreate: true, 10, isSpecial: false, 3, 36, EMaterialProperty.Invalid, 31, -1, 1000, 11, 360, 320, new List<short>
		{
			10, 16, 22, 28, 34, 42, 48, 54, 60, 66,
			80, 88, 96, 104, 112
		}, new PoisonsAndLevels(default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short)), new List<PresetInventoryItem>(), 0, 1, 0, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, primaryRecoverAllInjuries: false, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, secondaryRecoverAllInjuries: false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 60, EMaterialFilterType.Invalid, EMaterialFilterHardness.Stone));
		_dataArray.Add(new MaterialItem(35, 70, 5, 503, 1, 35, "icon_Material_shuiyu", 71, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 0, 30, 200, 300, 0, 2, 600, 3, allowRandomCreate: true, 40, isSpecial: false, 3, 36, EMaterialProperty.Invalid, 31, -1, 50, 11, 30, 5, new List<short>
		{
			6, 13, 19, 25, 31, 38, 45, 51, 57, 63,
			69, 84, 92, 100, 108, 116
		}, new PoisonsAndLevels(default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short)), new List<PresetInventoryItem>(), 0, 1, 0, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, primaryRecoverAllInjuries: false, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, secondaryRecoverAllInjuries: false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 60, EMaterialFilterType.Invalid, EMaterialFilterHardness.Jade));
		_dataArray.Add(new MaterialItem(36, 72, 5, 503, 2, 35, "icon_Material_shuiyu", 73, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 0, 30, 600, 900, 0, 3, 1200, 4, allowRandomCreate: true, 35, isSpecial: false, 3, 36, EMaterialProperty.Invalid, 31, -1, 100, 11, 60, 10, new List<short>
		{
			6, 13, 19, 25, 31, 38, 45, 51, 57, 63,
			69, 84, 92, 100, 108, 116
		}, new PoisonsAndLevels(default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short)), new List<PresetInventoryItem>(), 0, 1, 0, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, primaryRecoverAllInjuries: false, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, secondaryRecoverAllInjuries: false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 60, EMaterialFilterType.Invalid, EMaterialFilterHardness.Jade));
		_dataArray.Add(new MaterialItem(37, 74, 5, 503, 3, 35, "icon_Material_yangzhibaiyu", 75, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 0, 40, 1500, 2250, 1, 4, 1800, 5, allowRandomCreate: true, 30, isSpecial: false, 3, 36, EMaterialProperty.Invalid, 31, -1, 200, 11, 100, 20, new List<short>
		{
			6, 13, 19, 25, 31, 38, 45, 51, 57, 63,
			69, 84, 92, 100, 108, 116
		}, new PoisonsAndLevels(default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short)), new List<PresetInventoryItem>(), 0, 1, 0, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, primaryRecoverAllInjuries: false, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, secondaryRecoverAllInjuries: false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 60, EMaterialFilterType.Invalid, EMaterialFilterHardness.Jade));
		_dataArray.Add(new MaterialItem(38, 76, 5, 503, 4, 35, "icon_Material_yangzhibaiyu", 77, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 0, 60, 3100, 4650, 2, 5, 3000, 6, allowRandomCreate: true, 25, isSpecial: false, 3, 36, EMaterialProperty.Invalid, 31, -1, 400, 11, 150, 40, new List<short>
		{
			6, 13, 19, 25, 31, 38, 45, 51, 57, 63,
			69, 84, 92, 100, 108, 116
		}, new PoisonsAndLevels(default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short)), new List<PresetInventoryItem>(), 0, 1, 0, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, primaryRecoverAllInjuries: false, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, secondaryRecoverAllInjuries: false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 60, EMaterialFilterType.Invalid, EMaterialFilterHardness.Jade));
		_dataArray.Add(new MaterialItem(39, 78, 5, 503, 5, 35, "icon_Material_yangzhibaiyu", 79, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 0, 80, 5600, 8400, 3, 6, 4200, 7, allowRandomCreate: true, 20, isSpecial: false, 3, 36, EMaterialProperty.Invalid, 31, -1, 600, 11, 210, 80, new List<short>
		{
			6, 13, 19, 25, 31, 38, 45, 51, 57, 63,
			69, 84, 92, 100, 108, 116
		}, new PoisonsAndLevels(default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short)), new List<PresetInventoryItem>(), 0, 1, 0, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, primaryRecoverAllInjuries: false, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, secondaryRecoverAllInjuries: false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 60, EMaterialFilterType.Invalid, EMaterialFilterHardness.Jade));
		_dataArray.Add(new MaterialItem(40, 80, 5, 503, 6, 35, "icon_Material_hanyu", 81, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 0, 60, 9200, 13800, 4, 7, 5400, 7, allowRandomCreate: true, 15, isSpecial: false, 3, 36, EMaterialProperty.Invalid, 31, -1, 800, 11, 280, 160, new List<short>
		{
			6, 13, 19, 25, 31, 38, 45, 51, 57, 63,
			69, 84, 92, 100, 108, 116
		}, new PoisonsAndLevels(default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short)), new List<PresetInventoryItem>(), 0, 1, 0, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, primaryRecoverAllInjuries: false, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, secondaryRecoverAllInjuries: false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 60, EMaterialFilterType.Invalid, EMaterialFilterHardness.Jade));
		_dataArray.Add(new MaterialItem(41, 82, 5, 503, 7, 35, "icon_Material_kunlunhuoyu", 83, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 0, 90, 14100, 21150, 5, 8, 7200, 8, allowRandomCreate: true, 10, isSpecial: false, 3, 36, EMaterialProperty.Invalid, 31, -1, 1000, 11, 360, 320, new List<short>
		{
			6, 13, 19, 25, 31, 38, 45, 51, 57, 63,
			69, 84, 92, 100, 108, 116
		}, new PoisonsAndLevels(default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short)), new List<PresetInventoryItem>(), 0, 1, 0, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, primaryRecoverAllInjuries: false, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, secondaryRecoverAllInjuries: false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 60, EMaterialFilterType.Invalid, EMaterialFilterHardness.Jade));
		_dataArray.Add(new MaterialItem(42, 84, 5, 504, 1, 42, "icon_Material_huyi", 85, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 0, 50, 200, 300, 0, 2, 600, 3, allowRandomCreate: true, 40, isSpecial: false, 4, 36, EMaterialProperty.Invalid, 32, -1, 50, 10, 30, 5, new List<short>
		{
			35, 71, 75, 81, 89, 97, 105, 113, 118, 122,
			123
		}, new PoisonsAndLevels(default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short)), new List<PresetInventoryItem>(), 0, 1, 0, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, primaryRecoverAllInjuries: false, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, secondaryRecoverAllInjuries: false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 60, EMaterialFilterType.Invalid, EMaterialFilterHardness.Fur));
		_dataArray.Add(new MaterialItem(43, 86, 5, 504, 2, 42, "icon_Material_huyi", 87, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 0, 40, 600, 900, 0, 3, 1200, 4, allowRandomCreate: true, 35, isSpecial: false, 4, 36, EMaterialProperty.Invalid, 32, -1, 100, 10, 60, 10, new List<short>
		{
			35, 71, 75, 81, 89, 97, 105, 113, 118, 122,
			123
		}, new PoisonsAndLevels(default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short)), new List<PresetInventoryItem>(), 0, 1, 0, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, primaryRecoverAllInjuries: false, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, secondaryRecoverAllInjuries: false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 60, EMaterialFilterType.Invalid, EMaterialFilterHardness.Fur));
		_dataArray.Add(new MaterialItem(44, 88, 5, 504, 3, 42, "icon_Material_baimanglin", 89, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 0, 20, 1500, 2250, 1, 4, 1800, 5, allowRandomCreate: true, 30, isSpecial: false, 4, 36, EMaterialProperty.Invalid, 32, -1, 200, 10, 100, 20, new List<short>
		{
			35, 71, 75, 81, 89, 97, 105, 113, 118, 122,
			123
		}, new PoisonsAndLevels(default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short)), new List<PresetInventoryItem>(), 0, 1, 0, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, primaryRecoverAllInjuries: false, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, secondaryRecoverAllInjuries: false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 60, EMaterialFilterType.Invalid, EMaterialFilterHardness.Fur));
		_dataArray.Add(new MaterialItem(45, 90, 5, 504, 4, 42, "icon_Material_baimanglin", 91, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 0, 40, 3100, 4650, 2, 5, 3000, 6, allowRandomCreate: true, 25, isSpecial: false, 4, 36, EMaterialProperty.Invalid, 32, -1, 400, 10, 150, 40, new List<short>
		{
			35, 71, 75, 81, 89, 97, 105, 113, 118, 122,
			123
		}, new PoisonsAndLevels(default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short)), new List<PresetInventoryItem>(), 0, 1, 0, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, primaryRecoverAllInjuries: false, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, secondaryRecoverAllInjuries: false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 60, EMaterialFilterType.Invalid, EMaterialFilterHardness.Fur));
		_dataArray.Add(new MaterialItem(46, 92, 5, 504, 5, 42, "icon_Material_baimanglin", 93, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 0, 20, 5600, 8400, 3, 6, 4200, 7, allowRandomCreate: true, 20, isSpecial: false, 4, 36, EMaterialProperty.Invalid, 32, -1, 600, 10, 210, 80, new List<short>
		{
			35, 71, 75, 81, 89, 97, 105, 113, 118, 122,
			123
		}, new PoisonsAndLevels(default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short)), new List<PresetInventoryItem>(), 0, 1, 0, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, primaryRecoverAllInjuries: false, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, secondaryRecoverAllInjuries: false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 60, EMaterialFilterType.Invalid, EMaterialFilterHardness.Fur));
		_dataArray.Add(new MaterialItem(47, 94, 5, 504, 6, 42, "icon_Material_longbeijinjin", 95, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 0, 60, 9200, 13800, 4, 7, 5400, 7, allowRandomCreate: true, 15, isSpecial: false, 4, 36, EMaterialProperty.Invalid, 32, -1, 800, 10, 280, 160, new List<short>
		{
			35, 71, 75, 81, 89, 97, 105, 113, 118, 122,
			123
		}, new PoisonsAndLevels(default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short)), new List<PresetInventoryItem>(), 0, 1, 0, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, primaryRecoverAllInjuries: false, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, secondaryRecoverAllInjuries: false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 60, EMaterialFilterType.Invalid, EMaterialFilterHardness.Fur));
		_dataArray.Add(new MaterialItem(48, 96, 5, 504, 7, 42, "icon_Material_jinlvchanyi", 97, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 0, 20, 14100, 21150, 5, 8, 7200, 8, allowRandomCreate: true, 10, isSpecial: false, 4, 36, EMaterialProperty.Invalid, 32, -1, 1000, 10, 360, 320, new List<short>
		{
			35, 71, 75, 81, 89, 97, 105, 113, 118, 122,
			123
		}, new PoisonsAndLevels(default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short)), new List<PresetInventoryItem>(), 0, 1, 0, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, primaryRecoverAllInjuries: false, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, secondaryRecoverAllInjuries: false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 60, EMaterialFilterType.Invalid, EMaterialFilterHardness.Fur));
		_dataArray.Add(new MaterialItem(49, 98, 5, 504, 1, 49, "icon_Material_xiuwenhuangma", 99, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 0, 30, 200, 300, 0, 2, 600, 3, allowRandomCreate: true, 40, isSpecial: false, 4, 36, EMaterialProperty.Invalid, 32, -1, 50, 10, 30, 5, new List<short>
		{
			7, 39, 73, 77, 85, 93, 101, 109, 117, 119,
			122, 123
		}, new PoisonsAndLevels(default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short)), new List<PresetInventoryItem>(), 0, 1, 0, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, primaryRecoverAllInjuries: false, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, secondaryRecoverAllInjuries: false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 60, EMaterialFilterType.Invalid, EMaterialFilterHardness.Woven));
		_dataArray.Add(new MaterialItem(50, 100, 5, 504, 2, 49, "icon_Material_xiuwenhuangma", 101, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 0, 20, 600, 900, 0, 3, 1200, 4, allowRandomCreate: true, 35, isSpecial: false, 4, 36, EMaterialProperty.Invalid, 32, -1, 100, 10, 60, 10, new List<short>
		{
			7, 39, 73, 77, 85, 93, 101, 109, 117, 119,
			122, 123
		}, new PoisonsAndLevels(default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short)), new List<PresetInventoryItem>(), 0, 1, 0, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, primaryRecoverAllInjuries: false, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, secondaryRecoverAllInjuries: false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 60, EMaterialFilterType.Invalid, EMaterialFilterHardness.Woven));
		_dataArray.Add(new MaterialItem(51, 102, 5, 504, 3, 49, "icon_Material_baihuabaicaosi", 103, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 0, 20, 1500, 2250, 1, 4, 1800, 5, allowRandomCreate: true, 30, isSpecial: false, 4, 36, EMaterialProperty.Invalid, 32, -1, 200, 10, 100, 20, new List<short>
		{
			7, 39, 73, 77, 85, 93, 101, 109, 117, 119,
			122, 123
		}, new PoisonsAndLevels(default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short)), new List<PresetInventoryItem>(), 0, 1, 0, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, primaryRecoverAllInjuries: false, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, secondaryRecoverAllInjuries: false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 60, EMaterialFilterType.Invalid, EMaterialFilterHardness.Woven));
		_dataArray.Add(new MaterialItem(52, 104, 5, 504, 4, 49, "icon_Material_baihuabaicaosi", 105, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 0, 30, 3100, 4650, 2, 5, 3000, 6, allowRandomCreate: true, 25, isSpecial: false, 4, 36, EMaterialProperty.Invalid, 32, -1, 400, 10, 150, 40, new List<short>
		{
			7, 39, 73, 77, 85, 93, 101, 109, 117, 119,
			122, 123
		}, new PoisonsAndLevels(default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short)), new List<PresetInventoryItem>(), 0, 1, 0, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, primaryRecoverAllInjuries: false, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, secondaryRecoverAllInjuries: false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 60, EMaterialFilterType.Invalid, EMaterialFilterHardness.Woven));
		_dataArray.Add(new MaterialItem(53, 106, 5, 504, 5, 49, "icon_Material_baihuabaicaosi", 107, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 0, 20, 5600, 8400, 3, 6, 4200, 7, allowRandomCreate: true, 20, isSpecial: false, 4, 36, EMaterialProperty.Invalid, 32, -1, 600, 10, 210, 80, new List<short>
		{
			7, 39, 73, 77, 85, 93, 101, 109, 117, 119,
			122, 123
		}, new PoisonsAndLevels(default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short)), new List<PresetInventoryItem>(), 0, 1, 0, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, primaryRecoverAllInjuries: false, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, secondaryRecoverAllInjuries: false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 60, EMaterialFilterType.Invalid, EMaterialFilterHardness.Woven));
		_dataArray.Add(new MaterialItem(54, 108, 5, 504, 6, 49, "icon_Material_xielousi", 109, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 0, 30, 9200, 13800, 4, 7, 5400, 7, allowRandomCreate: true, 15, isSpecial: false, 4, 36, EMaterialProperty.Invalid, 32, -1, 800, 10, 280, 160, new List<short>
		{
			7, 39, 73, 77, 85, 93, 101, 109, 117, 119,
			122, 123
		}, new PoisonsAndLevels(default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short)), new List<PresetInventoryItem>(), 0, 1, 0, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, primaryRecoverAllInjuries: false, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, secondaryRecoverAllInjuries: false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 60, EMaterialFilterType.Invalid, EMaterialFilterHardness.Woven));
		_dataArray.Add(new MaterialItem(55, 110, 5, 504, 7, 49, "icon_Material_tiancansi", 111, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 0, 20, 14100, 21150, 5, 8, 7200, 8, allowRandomCreate: true, 10, isSpecial: false, 4, 36, EMaterialProperty.Invalid, 32, -1, 1000, 10, 360, 320, new List<short>
		{
			7, 39, 73, 77, 85, 93, 101, 109, 117, 119,
			122, 123
		}, new PoisonsAndLevels(default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short)), new List<PresetInventoryItem>(), 0, 1, 0, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, primaryRecoverAllInjuries: false, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, secondaryRecoverAllInjuries: false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 60, EMaterialFilterType.Invalid, EMaterialFilterHardness.Woven));
		_dataArray.Add(new MaterialItem(56, 112, 5, 500, 0, 56, "icon_Material_jidan", 113, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 0, 10, 25, 50, 0, 1, 600, 3, allowRandomCreate: true, 40, isSpecial: false, 0, 12, EMaterialProperty.Invalid, -1, -1, 25, 14, 10, 5, new List<short> { 124 }, new PoisonsAndLevels(default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short)), new List<PresetInventoryItem>(), 0, 1, 0, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, primaryRecoverAllInjuries: false, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, secondaryRecoverAllInjuries: false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 60, EMaterialFilterType.Bird, EMaterialFilterHardness.Invalid));
		_dataArray.Add(new MaterialItem(57, 114, 5, 500, 1, 56, "icon_Material_jidan", 115, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 0, 30, 50, 100, 0, 2, 1200, 4, allowRandomCreate: true, 35, isSpecial: false, 0, 12, EMaterialProperty.Invalid, -1, -1, 50, 14, 30, 10, new List<short> { 125 }, new PoisonsAndLevels(default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short)), new List<PresetInventoryItem>(), 0, 1, 0, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, primaryRecoverAllInjuries: false, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, secondaryRecoverAllInjuries: false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 60, EMaterialFilterType.Bird, EMaterialFilterHardness.Invalid));
		_dataArray.Add(new MaterialItem(58, 116, 5, 500, 2, 56, "icon_Material_jidan", 117, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 0, 50, 150, 300, 0, 3, 1800, 5, allowRandomCreate: true, 30, isSpecial: false, 0, 12, EMaterialProperty.Invalid, -1, -1, 100, 14, 60, 20, new List<short> { 126 }, new PoisonsAndLevels(default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short)), new List<PresetInventoryItem>(), 0, 1, 0, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, primaryRecoverAllInjuries: false, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, secondaryRecoverAllInjuries: false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 60, EMaterialFilterType.Bird, EMaterialFilterHardness.Invalid));
		_dataArray.Add(new MaterialItem(59, 118, 5, 500, 3, 56, "icon_Material_yane", 119, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 0, 60, 375, 750, 0, 4, 3000, 6, allowRandomCreate: true, 25, isSpecial: false, 0, 12, EMaterialProperty.Invalid, -1, -1, 200, 14, 100, 40, new List<short> { 127 }, new PoisonsAndLevels(default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short)), new List<PresetInventoryItem>(), 0, 1, 0, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, primaryRecoverAllInjuries: false, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, secondaryRecoverAllInjuries: false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 60, EMaterialFilterType.Bird, EMaterialFilterHardness.Invalid));
	}

	private void CreateItems1()
	{
		_dataArray.Add(new MaterialItem(60, 120, 5, 500, 4, 56, "icon_Material_yane", 121, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 0, 40, 775, 1550, 1, 5, 4200, 7, allowRandomCreate: true, 20, isSpecial: false, 0, 12, EMaterialProperty.Invalid, -1, -1, 400, 14, 150, 80, new List<short> { 128 }, new PoisonsAndLevels(default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short)), new List<PresetInventoryItem>(), 0, 1, 0, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, primaryRecoverAllInjuries: false, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, secondaryRecoverAllInjuries: false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 60, EMaterialFilterType.Bird, EMaterialFilterHardness.Invalid));
		_dataArray.Add(new MaterialItem(61, 122, 5, 500, 5, 56, "icon_Material_yane", 123, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 0, 10, 1400, 2800, 2, 6, 5400, 7, allowRandomCreate: true, 15, isSpecial: false, 0, 12, EMaterialProperty.Invalid, -1, -1, 600, 14, 210, 160, new List<short> { 129 }, new PoisonsAndLevels(default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short)), new List<PresetInventoryItem>(), 0, 1, 0, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, primaryRecoverAllInjuries: false, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, secondaryRecoverAllInjuries: false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 60, EMaterialFilterType.Bird, EMaterialFilterHardness.Invalid));
		_dataArray.Add(new MaterialItem(62, 124, 5, 500, 6, 56, "icon_Material_lankongqiao", 125, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 0, 80, 2300, 4600, 3, 7, 7200, 8, allowRandomCreate: true, 10, isSpecial: false, 0, 12, EMaterialProperty.Invalid, -1, -1, 800, 14, 280, 320, new List<short> { 130 }, new PoisonsAndLevels(default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short)), new List<PresetInventoryItem>(), 0, 1, 0, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, primaryRecoverAllInjuries: false, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, secondaryRecoverAllInjuries: false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 60, EMaterialFilterType.Bird, EMaterialFilterHardness.Invalid));
		_dataArray.Add(new MaterialItem(63, 126, 5, 500, 0, 63, "icon_Material_yetu", 127, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 0, 30, 25, 50, 0, 1, 600, 3, allowRandomCreate: true, 40, isSpecial: false, 0, 12, EMaterialProperty.Invalid, -1, -1, 25, 14, 10, 5, new List<short> { 131 }, new PoisonsAndLevels(default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short)), new List<PresetInventoryItem>(), 0, 1, 0, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, primaryRecoverAllInjuries: false, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, secondaryRecoverAllInjuries: false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 60, EMaterialFilterType.Beast, EMaterialFilterHardness.Invalid));
		_dataArray.Add(new MaterialItem(64, 128, 5, 500, 1, 63, "icon_Material_yetu", 129, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 0, 60, 50, 100, 0, 2, 1200, 4, allowRandomCreate: true, 35, isSpecial: false, 0, 12, EMaterialProperty.Invalid, -1, -1, 50, 14, 30, 10, new List<short> { 132 }, new PoisonsAndLevels(default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short)), new List<PresetInventoryItem>(), 0, 1, 0, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, primaryRecoverAllInjuries: false, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, secondaryRecoverAllInjuries: false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 60, EMaterialFilterType.Beast, EMaterialFilterHardness.Invalid));
		_dataArray.Add(new MaterialItem(65, 130, 5, 500, 2, 63, "icon_Material_yetu", 131, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 0, 50, 150, 300, 0, 3, 1800, 5, allowRandomCreate: true, 30, isSpecial: false, 0, 12, EMaterialProperty.Invalid, -1, -1, 100, 14, 60, 20, new List<short> { 133 }, new PoisonsAndLevels(default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short)), new List<PresetInventoryItem>(), 0, 1, 0, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, primaryRecoverAllInjuries: false, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, secondaryRecoverAllInjuries: false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 60, EMaterialFilterType.Beast, EMaterialFilterHardness.Invalid));
		_dataArray.Add(new MaterialItem(66, 132, 5, 500, 3, 63, "icon_Material_baihuajinshe", 133, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 0, 20, 375, 750, 0, 4, 3000, 6, allowRandomCreate: true, 25, isSpecial: false, 0, 12, EMaterialProperty.Invalid, -1, -1, 200, 14, 100, 40, new List<short> { 134 }, new PoisonsAndLevels(default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short)), new List<PresetInventoryItem>(), 0, 1, 0, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, primaryRecoverAllInjuries: false, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, secondaryRecoverAllInjuries: false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 60, EMaterialFilterType.Beast, EMaterialFilterHardness.Invalid));
		_dataArray.Add(new MaterialItem(67, 134, 5, 500, 4, 63, "icon_Material_baihuajinshe", 135, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 0, 60, 775, 1550, 1, 5, 4200, 7, allowRandomCreate: true, 20, isSpecial: false, 0, 12, EMaterialProperty.Invalid, -1, -1, 400, 14, 150, 80, new List<short> { 135 }, new PoisonsAndLevels(default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short)), new List<PresetInventoryItem>(), 0, 1, 0, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, primaryRecoverAllInjuries: false, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, secondaryRecoverAllInjuries: false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 60, EMaterialFilterType.Beast, EMaterialFilterHardness.Invalid));
		_dataArray.Add(new MaterialItem(68, 136, 5, 500, 5, 63, "icon_Material_baihuajinshe", 137, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 0, 120, 1400, 2800, 2, 6, 5400, 7, allowRandomCreate: true, 15, isSpecial: false, 0, 12, EMaterialProperty.Invalid, -1, -1, 600, 14, 210, 160, new List<short> { 136 }, new PoisonsAndLevels(default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short)), new List<PresetInventoryItem>(), 0, 1, 0, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, primaryRecoverAllInjuries: false, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, secondaryRecoverAllInjuries: false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 60, EMaterialFilterType.Beast, EMaterialFilterHardness.Invalid));
		_dataArray.Add(new MaterialItem(69, 138, 5, 500, 6, 63, "icon_Material_heixiong", 139, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 0, 100, 2300, 4600, 3, 7, 7200, 8, allowRandomCreate: true, 10, isSpecial: false, 0, 12, EMaterialProperty.Invalid, -1, -1, 800, 14, 280, 320, new List<short> { 137 }, new PoisonsAndLevels(default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short)), new List<PresetInventoryItem>(), 0, 1, 0, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, primaryRecoverAllInjuries: false, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, secondaryRecoverAllInjuries: false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 60, EMaterialFilterType.Beast, EMaterialFilterHardness.Invalid));
		_dataArray.Add(new MaterialItem(70, 140, 5, 500, 0, 70, "icon_Material_xiaomai", 141, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 0, 10, 25, 50, 0, 1, 600, 3, allowRandomCreate: true, 40, isSpecial: false, 0, 12, EMaterialProperty.Invalid, -1, -1, 25, 14, 10, 5, new List<short> { 138 }, new PoisonsAndLevels(default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short)), new List<PresetInventoryItem>(), 0, 1, 0, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, primaryRecoverAllInjuries: false, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, secondaryRecoverAllInjuries: false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 60, EMaterialFilterType.Vegetarian, EMaterialFilterHardness.Invalid));
		_dataArray.Add(new MaterialItem(71, 142, 5, 500, 1, 70, "icon_Material_xiaomai", 143, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 0, 20, 50, 100, 0, 2, 1200, 4, allowRandomCreate: true, 35, isSpecial: false, 0, 12, EMaterialProperty.Invalid, -1, -1, 50, 14, 30, 10, new List<short> { 139 }, new PoisonsAndLevels(default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short)), new List<PresetInventoryItem>(), 0, 1, 0, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, primaryRecoverAllInjuries: false, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, secondaryRecoverAllInjuries: false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 60, EMaterialFilterType.Vegetarian, EMaterialFilterHardness.Invalid));
		_dataArray.Add(new MaterialItem(72, 144, 5, 500, 2, 70, "icon_Material_xiaomai", 145, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 0, 10, 150, 300, 0, 3, 1800, 5, allowRandomCreate: true, 30, isSpecial: false, 0, 12, EMaterialProperty.Invalid, -1, -1, 100, 14, 60, 20, new List<short> { 140 }, new PoisonsAndLevels(default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short)), new List<PresetInventoryItem>(), 0, 1, 0, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, primaryRecoverAllInjuries: false, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, secondaryRecoverAllInjuries: false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 60, EMaterialFilterType.Vegetarian, EMaterialFilterHardness.Invalid));
		_dataArray.Add(new MaterialItem(73, 146, 5, 500, 3, 70, "icon_Material_yulusun", 147, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 0, 20, 375, 750, 0, 4, 3000, 6, allowRandomCreate: true, 25, isSpecial: false, 0, 12, EMaterialProperty.Invalid, -1, -1, 200, 14, 100, 40, new List<short> { 141 }, new PoisonsAndLevels(default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short)), new List<PresetInventoryItem>(), 0, 1, 0, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, primaryRecoverAllInjuries: false, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, secondaryRecoverAllInjuries: false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 60, EMaterialFilterType.Vegetarian, EMaterialFilterHardness.Invalid));
		_dataArray.Add(new MaterialItem(74, 148, 5, 500, 4, 70, "icon_Material_yulusun", 149, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 0, 10, 775, 1550, 1, 5, 4200, 7, allowRandomCreate: true, 20, isSpecial: false, 0, 12, EMaterialProperty.Invalid, -1, -1, 400, 14, 150, 80, new List<short> { 142 }, new PoisonsAndLevels(default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short)), new List<PresetInventoryItem>(), 0, 1, 0, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, primaryRecoverAllInjuries: false, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, secondaryRecoverAllInjuries: false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 60, EMaterialFilterType.Vegetarian, EMaterialFilterHardness.Invalid));
		_dataArray.Add(new MaterialItem(75, 150, 5, 500, 5, 70, "icon_Material_yulusun", 151, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 0, 10, 1400, 2800, 2, 6, 5400, 7, allowRandomCreate: true, 15, isSpecial: false, 0, 12, EMaterialProperty.Invalid, -1, -1, 600, 14, 210, 160, new List<short> { 143 }, new PoisonsAndLevels(default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short)), new List<PresetInventoryItem>(), 0, 1, 0, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, primaryRecoverAllInjuries: false, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, secondaryRecoverAllInjuries: false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 60, EMaterialFilterType.Vegetarian, EMaterialFilterHardness.Invalid));
		_dataArray.Add(new MaterialItem(76, 152, 5, 500, 6, 70, "icon_Material_houtougu", 153, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 0, 20, 2300, 4600, 3, 7, 7200, 8, allowRandomCreate: true, 10, isSpecial: false, 0, 12, EMaterialProperty.Invalid, -1, -1, 800, 14, 280, 320, new List<short> { 144 }, new PoisonsAndLevels(default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short)), new List<PresetInventoryItem>(), 0, 1, 0, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, primaryRecoverAllInjuries: false, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, secondaryRecoverAllInjuries: false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 60, EMaterialFilterType.Vegetarian, EMaterialFilterHardness.Invalid));
		_dataArray.Add(new MaterialItem(77, 154, 5, 500, 0, 77, "icon_Material_caoyu", 155, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 0, 30, 25, 50, 0, 1, 600, 3, allowRandomCreate: true, 40, isSpecial: false, 0, 12, EMaterialProperty.Invalid, -1, -1, 25, 14, 10, 5, new List<short> { 145 }, new PoisonsAndLevels(default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short)), new List<PresetInventoryItem>(), 0, 1, 0, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, primaryRecoverAllInjuries: false, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, secondaryRecoverAllInjuries: false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 60, EMaterialFilterType.Fish, EMaterialFilterHardness.Invalid));
		_dataArray.Add(new MaterialItem(78, 156, 5, 500, 1, 77, "icon_Material_caoyu", 157, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 0, 20, 50, 100, 0, 2, 1200, 4, allowRandomCreate: true, 35, isSpecial: false, 0, 12, EMaterialProperty.Invalid, -1, -1, 50, 14, 30, 10, new List<short> { 146 }, new PoisonsAndLevels(default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short)), new List<PresetInventoryItem>(), 0, 1, 0, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, primaryRecoverAllInjuries: false, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, secondaryRecoverAllInjuries: false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 60, EMaterialFilterType.Fish, EMaterialFilterHardness.Invalid));
		_dataArray.Add(new MaterialItem(79, 158, 5, 500, 2, 77, "icon_Material_caoyu", 159, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 0, 50, 150, 300, 0, 3, 1800, 5, allowRandomCreate: true, 30, isSpecial: false, 0, 12, EMaterialProperty.Invalid, -1, -1, 100, 14, 60, 20, new List<short> { 147 }, new PoisonsAndLevels(default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short)), new List<PresetInventoryItem>(), 0, 1, 0, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, primaryRecoverAllInjuries: false, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, secondaryRecoverAllInjuries: false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 60, EMaterialFilterType.Fish, EMaterialFilterHardness.Invalid));
		_dataArray.Add(new MaterialItem(80, 160, 5, 500, 3, 77, "icon_Material_chixie", 161, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 0, 20, 375, 750, 0, 4, 3000, 6, allowRandomCreate: true, 25, isSpecial: false, 0, 12, EMaterialProperty.Invalid, -1, -1, 200, 14, 100, 40, new List<short> { 148 }, new PoisonsAndLevels(default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short)), new List<PresetInventoryItem>(), 0, 1, 0, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, primaryRecoverAllInjuries: false, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, secondaryRecoverAllInjuries: false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 60, EMaterialFilterType.Fish, EMaterialFilterHardness.Invalid));
		_dataArray.Add(new MaterialItem(81, 162, 5, 500, 4, 77, "icon_Material_chixie", 163, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 0, 40, 775, 1550, 1, 5, 4200, 7, allowRandomCreate: true, 20, isSpecial: false, 0, 12, EMaterialProperty.Invalid, -1, -1, 400, 14, 150, 80, new List<short> { 149 }, new PoisonsAndLevels(default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short)), new List<PresetInventoryItem>(), 0, 1, 0, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, primaryRecoverAllInjuries: false, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, secondaryRecoverAllInjuries: false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 60, EMaterialFilterType.Fish, EMaterialFilterHardness.Invalid));
		_dataArray.Add(new MaterialItem(82, 164, 5, 500, 5, 77, "icon_Material_chixie", 165, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 0, 30, 1400, 2800, 2, 6, 5400, 7, allowRandomCreate: true, 15, isSpecial: false, 0, 12, EMaterialProperty.Invalid, -1, -1, 600, 14, 210, 160, new List<short> { 150 }, new PoisonsAndLevels(default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short)), new List<PresetInventoryItem>(), 0, 1, 0, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, primaryRecoverAllInjuries: false, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, secondaryRecoverAllInjuries: false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 60, EMaterialFilterType.Fish, EMaterialFilterHardness.Invalid));
		_dataArray.Add(new MaterialItem(83, 166, 5, 500, 6, 77, "icon_Material_xunhuangyu", 167, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 0, 40, 2300, 4600, 3, 7, 7200, 8, allowRandomCreate: true, 10, isSpecial: false, 0, 12, EMaterialProperty.Invalid, -1, -1, 800, 14, 280, 320, new List<short> { 151 }, new PoisonsAndLevels(default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short)), new List<PresetInventoryItem>(), 0, 1, 0, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, primaryRecoverAllInjuries: false, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, secondaryRecoverAllInjuries: false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 60, EMaterialFilterType.Fish, EMaterialFilterHardness.Invalid));
		_dataArray.Add(new MaterialItem(84, 168, 5, 501, 0, 84, "icon_Material_baiyumudiaohua", 169, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 0, 20, 100, 150, 0, 1, 600, 3, allowRandomCreate: true, 40, isSpecial: false, 1, 36, EMaterialProperty.Invalid, -1, 0, 0, 7, 30, 25, new List<short>(), new PoisonsAndLevels(default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short)), new List<PresetInventoryItem>(), 0, 1, 0, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, primaryRecoverAllInjuries: false, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, secondaryRecoverAllInjuries: false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 60, EMaterialFilterType.Invalid, EMaterialFilterHardness.Wooden));
		_dataArray.Add(new MaterialItem(85, 170, 5, 501, 1, 84, "icon_Material_baiyumudiaohua", 171, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 0, 30, 200, 300, 0, 2, 1200, 4, allowRandomCreate: true, 35, isSpecial: false, 1, 36, EMaterialProperty.Invalid, -1, 0, 0, 7, 60, 50, new List<short>(), new PoisonsAndLevels(default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short)), new List<PresetInventoryItem>(), 0, 1, 0, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, primaryRecoverAllInjuries: false, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, secondaryRecoverAllInjuries: false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 60, EMaterialFilterType.Invalid, EMaterialFilterHardness.Wooden));
		_dataArray.Add(new MaterialItem(86, 172, 5, 501, 2, 84, "icon_Material_baiyumudiaohua", 173, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 0, 20, 600, 900, 0, 3, 1800, 5, allowRandomCreate: true, 30, isSpecial: false, 1, 36, EMaterialProperty.Invalid, -1, 0, 0, 7, 100, 100, new List<short>(), new PoisonsAndLevels(default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short)), new List<PresetInventoryItem>(), 0, 1, 0, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, primaryRecoverAllInjuries: false, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, secondaryRecoverAllInjuries: false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 60, EMaterialFilterType.Invalid, EMaterialFilterHardness.Wooden));
		_dataArray.Add(new MaterialItem(87, 174, 5, 501, 3, 84, "icon_Material_baotaxiejudiaohua", 175, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 0, 20, 1500, 2250, 1, 4, 3000, 6, allowRandomCreate: true, 25, isSpecial: false, 1, 36, EMaterialProperty.Invalid, -1, 0, 0, 7, 150, 200, new List<short>(), new PoisonsAndLevels(default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short)), new List<PresetInventoryItem>(), 0, 1, 0, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, primaryRecoverAllInjuries: false, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, secondaryRecoverAllInjuries: false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 60, EMaterialFilterType.Invalid, EMaterialFilterHardness.Wooden));
		_dataArray.Add(new MaterialItem(88, 176, 5, 501, 4, 84, "icon_Material_baotaxiejudiaohua", 177, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 0, 30, 3100, 4650, 2, 5, 4200, 7, allowRandomCreate: true, 20, isSpecial: false, 1, 36, EMaterialProperty.Invalid, -1, 0, 0, 7, 210, 400, new List<short>(), new PoisonsAndLevels(default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short)), new List<PresetInventoryItem>(), 0, 1, 0, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, primaryRecoverAllInjuries: false, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, secondaryRecoverAllInjuries: false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 60, EMaterialFilterType.Invalid, EMaterialFilterHardness.Wooden));
		_dataArray.Add(new MaterialItem(89, 178, 5, 501, 5, 84, "icon_Material_baotaxiejudiaohua", 179, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 0, 20, 5600, 8400, 3, 6, 5400, 7, allowRandomCreate: true, 15, isSpecial: false, 1, 36, EMaterialProperty.Invalid, -1, 0, 0, 7, 280, 800, new List<short>(), new PoisonsAndLevels(default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short)), new List<PresetInventoryItem>(), 0, 1, 0, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, primaryRecoverAllInjuries: false, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, secondaryRecoverAllInjuries: false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 60, EMaterialFilterType.Invalid, EMaterialFilterHardness.Wooden));
		_dataArray.Add(new MaterialItem(90, 180, 5, 501, 6, 84, "icon_Material_wujinzitandiaohua", 181, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 0, 20, 9200, 13800, 4, 7, 7200, 8, allowRandomCreate: true, 10, isSpecial: false, 1, 36, EMaterialProperty.Invalid, -1, 0, 0, 7, 360, 1600, new List<short>(), new PoisonsAndLevels(default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short)), new List<PresetInventoryItem>(), 0, 1, 0, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, primaryRecoverAllInjuries: false, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, secondaryRecoverAllInjuries: false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 60, EMaterialFilterType.Invalid, EMaterialFilterHardness.Wooden));
		_dataArray.Add(new MaterialItem(91, 182, 5, 501, 0, 91, "icon_Material_mengzongzhubianzhi", 183, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 0, 10, 100, 150, 0, 1, 600, 3, allowRandomCreate: true, 40, isSpecial: false, 1, 36, EMaterialProperty.Invalid, -1, 1, 0, 7, 30, 25, new List<short>(), new PoisonsAndLevels(default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short)), new List<PresetInventoryItem>(), 0, 1, 0, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, primaryRecoverAllInjuries: false, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, secondaryRecoverAllInjuries: false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 60, EMaterialFilterType.Invalid, EMaterialFilterHardness.Rattan));
		_dataArray.Add(new MaterialItem(92, 184, 5, 501, 1, 91, "icon_Material_mengzongzhubianzhi", 185, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 0, 10, 200, 300, 0, 2, 1200, 4, allowRandomCreate: true, 35, isSpecial: false, 1, 36, EMaterialProperty.Invalid, -1, 1, 0, 7, 60, 50, new List<short>(), new PoisonsAndLevels(default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short)), new List<PresetInventoryItem>(), 0, 1, 0, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, primaryRecoverAllInjuries: false, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, secondaryRecoverAllInjuries: false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 60, EMaterialFilterType.Invalid, EMaterialFilterHardness.Rattan));
		_dataArray.Add(new MaterialItem(93, 186, 5, 501, 2, 91, "icon_Material_mengzongzhubianzhi", 187, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 0, 10, 600, 900, 0, 3, 1800, 5, allowRandomCreate: true, 30, isSpecial: false, 1, 36, EMaterialProperty.Invalid, -1, 1, 0, 7, 100, 100, new List<short>(), new PoisonsAndLevels(default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short)), new List<PresetInventoryItem>(), 0, 1, 0, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, primaryRecoverAllInjuries: false, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, secondaryRecoverAllInjuries: false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 60, EMaterialFilterType.Invalid, EMaterialFilterHardness.Rattan));
		_dataArray.Add(new MaterialItem(94, 188, 5, 501, 3, 91, "icon_Material_longpangenbianzhi", 189, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 0, 20, 1500, 2250, 1, 4, 3000, 6, allowRandomCreate: true, 25, isSpecial: false, 1, 36, EMaterialProperty.Invalid, -1, 1, 0, 7, 150, 200, new List<short>(), new PoisonsAndLevels(default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short)), new List<PresetInventoryItem>(), 0, 1, 0, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, primaryRecoverAllInjuries: false, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, secondaryRecoverAllInjuries: false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 60, EMaterialFilterType.Invalid, EMaterialFilterHardness.Rattan));
		_dataArray.Add(new MaterialItem(95, 190, 5, 501, 4, 91, "icon_Material_longpangenbianzhi", 191, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 0, 10, 3100, 4650, 2, 5, 4200, 7, allowRandomCreate: true, 20, isSpecial: false, 1, 36, EMaterialProperty.Invalid, -1, 1, 0, 7, 210, 400, new List<short>(), new PoisonsAndLevels(default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short)), new List<PresetInventoryItem>(), 0, 1, 0, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, primaryRecoverAllInjuries: false, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, secondaryRecoverAllInjuries: false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 60, EMaterialFilterType.Invalid, EMaterialFilterHardness.Rattan));
		_dataArray.Add(new MaterialItem(96, 192, 5, 501, 5, 91, "icon_Material_longpangenbianzhi", 193, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 0, 10, 5600, 8400, 3, 6, 5400, 7, allowRandomCreate: true, 15, isSpecial: false, 1, 36, EMaterialProperty.Invalid, -1, 1, 0, 7, 280, 800, new List<short>(), new PoisonsAndLevels(default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short)), new List<PresetInventoryItem>(), 0, 1, 0, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, primaryRecoverAllInjuries: false, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, secondaryRecoverAllInjuries: false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 60, EMaterialFilterType.Invalid, EMaterialFilterHardness.Rattan));
		_dataArray.Add(new MaterialItem(97, 194, 5, 501, 6, 91, "icon_Material_jiuquzizhubianzhi", 195, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 0, 10, 9200, 13800, 4, 7, 7200, 8, allowRandomCreate: true, 10, isSpecial: false, 1, 36, EMaterialProperty.Invalid, -1, 1, 0, 7, 360, 1600, new List<short>(), new PoisonsAndLevels(default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short)), new List<PresetInventoryItem>(), 0, 1, 0, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, primaryRecoverAllInjuries: false, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, secondaryRecoverAllInjuries: false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 60, EMaterialFilterType.Invalid, EMaterialFilterHardness.Rattan));
		_dataArray.Add(new MaterialItem(98, 196, 5, 502, 0, 98, "icon_Material_bintietiehua", 197, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 0, 30, 100, 150, 0, 1, 600, 3, allowRandomCreate: true, 40, isSpecial: false, 2, 36, EMaterialProperty.Invalid, -1, 2, 0, 6, 30, 25, new List<short>(), new PoisonsAndLevels(default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short)), new List<PresetInventoryItem>(), 0, 1, 0, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, primaryRecoverAllInjuries: false, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, secondaryRecoverAllInjuries: false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 60, EMaterialFilterType.Invalid, EMaterialFilterHardness.Icon));
		_dataArray.Add(new MaterialItem(99, 198, 5, 502, 1, 98, "icon_Material_bintietiehua", 199, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 0, 40, 200, 300, 0, 2, 1200, 4, allowRandomCreate: true, 35, isSpecial: false, 2, 36, EMaterialProperty.Invalid, -1, 2, 0, 6, 60, 50, new List<short>(), new PoisonsAndLevels(default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short)), new List<PresetInventoryItem>(), 0, 1, 0, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, primaryRecoverAllInjuries: false, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, secondaryRecoverAllInjuries: false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 60, EMaterialFilterType.Invalid, EMaterialFilterHardness.Icon));
		_dataArray.Add(new MaterialItem(100, 200, 5, 502, 2, 98, "icon_Material_bintietiehua", 201, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 0, 50, 600, 900, 0, 3, 1800, 5, allowRandomCreate: true, 30, isSpecial: false, 2, 36, EMaterialProperty.Invalid, -1, 2, 0, 6, 100, 100, new List<short>(), new PoisonsAndLevels(default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short)), new List<PresetInventoryItem>(), 0, 1, 0, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, primaryRecoverAllInjuries: false, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, secondaryRecoverAllInjuries: false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 60, EMaterialFilterType.Invalid, EMaterialFilterHardness.Icon));
		_dataArray.Add(new MaterialItem(101, 202, 5, 502, 3, 98, "icon_Material_wujintiehua", 203, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 0, 40, 1500, 2250, 1, 4, 3000, 6, allowRandomCreate: true, 25, isSpecial: false, 2, 36, EMaterialProperty.Invalid, -1, 2, 0, 6, 150, 200, new List<short>(), new PoisonsAndLevels(default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short)), new List<PresetInventoryItem>(), 0, 1, 0, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, primaryRecoverAllInjuries: false, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, secondaryRecoverAllInjuries: false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 60, EMaterialFilterType.Invalid, EMaterialFilterHardness.Icon));
		_dataArray.Add(new MaterialItem(102, 204, 5, 502, 4, 98, "icon_Material_wujintiehua", 205, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 0, 60, 3100, 4650, 2, 5, 4200, 7, allowRandomCreate: true, 20, isSpecial: false, 2, 36, EMaterialProperty.Invalid, -1, 2, 0, 6, 210, 400, new List<short>(), new PoisonsAndLevels(default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short)), new List<PresetInventoryItem>(), 0, 1, 0, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, primaryRecoverAllInjuries: false, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, secondaryRecoverAllInjuries: false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 60, EMaterialFilterType.Invalid, EMaterialFilterHardness.Icon));
		_dataArray.Add(new MaterialItem(103, 206, 5, 502, 5, 98, "icon_Material_wujintiehua", 207, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 0, 70, 5600, 8400, 3, 6, 5400, 7, allowRandomCreate: true, 15, isSpecial: false, 2, 36, EMaterialProperty.Invalid, -1, 2, 0, 6, 280, 800, new List<short>(), new PoisonsAndLevels(default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short)), new List<PresetInventoryItem>(), 0, 1, 0, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, primaryRecoverAllInjuries: false, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, secondaryRecoverAllInjuries: false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 60, EMaterialFilterType.Invalid, EMaterialFilterHardness.Icon));
		_dataArray.Add(new MaterialItem(104, 208, 5, 502, 6, 98, "icon_Material_xuantietiehua", 209, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 0, 100, 9200, 13800, 4, 7, 7200, 8, allowRandomCreate: true, 10, isSpecial: false, 2, 36, EMaterialProperty.Invalid, -1, 2, 0, 6, 360, 1600, new List<short>(), new PoisonsAndLevels(default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short)), new List<PresetInventoryItem>(), 0, 1, 0, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, primaryRecoverAllInjuries: false, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, secondaryRecoverAllInjuries: false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 60, EMaterialFilterType.Invalid, EMaterialFilterHardness.Icon));
		_dataArray.Add(new MaterialItem(105, 210, 5, 502, 0, 105, "icon_Material_yuantonghuasi", 211, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 0, 10, 100, 150, 0, 1, 600, 3, allowRandomCreate: true, 40, isSpecial: false, 2, 36, EMaterialProperty.Invalid, -1, 3, 0, 6, 30, 25, new List<short>(), new PoisonsAndLevels(default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short)), new List<PresetInventoryItem>(), 0, 1, 0, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, primaryRecoverAllInjuries: false, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, secondaryRecoverAllInjuries: false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 60, EMaterialFilterType.Invalid, EMaterialFilterHardness.GoldSilver));
		_dataArray.Add(new MaterialItem(106, 212, 5, 502, 1, 105, "icon_Material_yuantonghuasi", 213, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 0, 10, 200, 300, 0, 2, 1200, 4, allowRandomCreate: true, 35, isSpecial: false, 2, 36, EMaterialProperty.Invalid, -1, 3, 0, 6, 60, 50, new List<short>(), new PoisonsAndLevels(default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short)), new List<PresetInventoryItem>(), 0, 1, 0, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, primaryRecoverAllInjuries: false, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, secondaryRecoverAllInjuries: false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 60, EMaterialFilterType.Invalid, EMaterialFilterHardness.GoldSilver));
		_dataArray.Add(new MaterialItem(107, 214, 5, 502, 2, 105, "icon_Material_yuantonghuasi", 215, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 0, 20, 600, 900, 0, 3, 1800, 5, allowRandomCreate: true, 30, isSpecial: false, 2, 36, EMaterialProperty.Invalid, -1, 3, 0, 6, 100, 100, new List<short>(), new PoisonsAndLevels(default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short)), new List<PresetInventoryItem>(), 0, 1, 0, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, primaryRecoverAllInjuries: false, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, secondaryRecoverAllInjuries: false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 60, EMaterialFilterType.Invalid, EMaterialFilterHardness.GoldSilver));
		_dataArray.Add(new MaterialItem(108, 216, 5, 502, 3, 105, "icon_Material_ruyibaotonghuasi", 217, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 0, 30, 1500, 2250, 1, 4, 3000, 6, allowRandomCreate: true, 25, isSpecial: false, 2, 36, EMaterialProperty.Invalid, -1, 3, 0, 6, 150, 200, new List<short>(), new PoisonsAndLevels(default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short)), new List<PresetInventoryItem>(), 0, 1, 0, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, primaryRecoverAllInjuries: false, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, secondaryRecoverAllInjuries: false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 60, EMaterialFilterType.Invalid, EMaterialFilterHardness.GoldSilver));
		_dataArray.Add(new MaterialItem(109, 218, 5, 502, 4, 105, "icon_Material_ruyibaotonghuasi", 219, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 0, 40, 3100, 4650, 2, 5, 4200, 7, allowRandomCreate: true, 20, isSpecial: false, 2, 36, EMaterialProperty.Invalid, -1, 3, 0, 6, 210, 400, new List<short>(), new PoisonsAndLevels(default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short)), new List<PresetInventoryItem>(), 0, 1, 0, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, primaryRecoverAllInjuries: false, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, secondaryRecoverAllInjuries: false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 60, EMaterialFilterType.Invalid, EMaterialFilterHardness.GoldSilver));
		_dataArray.Add(new MaterialItem(110, 220, 5, 502, 5, 105, "icon_Material_ruyibaotonghuasi", 221, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 0, 10, 5600, 8400, 3, 6, 5400, 7, allowRandomCreate: true, 15, isSpecial: false, 2, 36, EMaterialProperty.Invalid, -1, 3, 0, 6, 280, 800, new List<short>(), new PoisonsAndLevels(default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short)), new List<PresetInventoryItem>(), 0, 1, 0, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, primaryRecoverAllInjuries: false, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, secondaryRecoverAllInjuries: false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 60, EMaterialFilterType.Invalid, EMaterialFilterHardness.GoldSilver));
		_dataArray.Add(new MaterialItem(111, 222, 5, 502, 6, 105, "icon_Material_chankejingjinhuasi", 223, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 0, 10, 9200, 13800, 4, 7, 7200, 8, allowRandomCreate: true, 10, isSpecial: false, 2, 36, EMaterialProperty.Invalid, -1, 3, 0, 6, 360, 1600, new List<short>(), new PoisonsAndLevels(default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short)), new List<PresetInventoryItem>(), 0, 1, 0, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, primaryRecoverAllInjuries: false, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, secondaryRecoverAllInjuries: false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 60, EMaterialFilterType.Invalid, EMaterialFilterHardness.GoldSilver));
		_dataArray.Add(new MaterialItem(112, 224, 5, 503, 0, 112, "icon_Material_heimanaoqianshi", 225, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 0, 10, 100, 150, 0, 1, 600, 3, allowRandomCreate: true, 40, isSpecial: false, 3, 36, EMaterialProperty.Invalid, -1, 4, 0, 11, 30, 25, new List<short>(), new PoisonsAndLevels(default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short)), new List<PresetInventoryItem>(), 0, 1, 0, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, primaryRecoverAllInjuries: false, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, secondaryRecoverAllInjuries: false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 60, EMaterialFilterType.Invalid, EMaterialFilterHardness.Stone));
		_dataArray.Add(new MaterialItem(113, 226, 5, 503, 1, 112, "icon_Material_heimanaoqianshi", 227, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 0, 10, 200, 300, 0, 2, 1200, 4, allowRandomCreate: true, 35, isSpecial: false, 3, 36, EMaterialProperty.Invalid, -1, 4, 0, 11, 60, 50, new List<short>(), new PoisonsAndLevels(default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short)), new List<PresetInventoryItem>(), 0, 1, 0, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, primaryRecoverAllInjuries: false, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, secondaryRecoverAllInjuries: false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 60, EMaterialFilterType.Invalid, EMaterialFilterHardness.Stone));
		_dataArray.Add(new MaterialItem(114, 228, 5, 503, 2, 112, "icon_Material_heimanaoqianshi", 229, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 0, 10, 600, 900, 0, 3, 1800, 5, allowRandomCreate: true, 30, isSpecial: false, 3, 36, EMaterialProperty.Invalid, -1, 4, 0, 11, 100, 100, new List<short>(), new PoisonsAndLevels(default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short)), new List<PresetInventoryItem>(), 0, 1, 0, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, primaryRecoverAllInjuries: false, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, secondaryRecoverAllInjuries: false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 60, EMaterialFilterType.Invalid, EMaterialFilterHardness.Stone));
		_dataArray.Add(new MaterialItem(115, 230, 5, 503, 3, 112, "icon_Material_guiwenmaoyanqianshi", 231, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 0, 10, 1500, 2250, 1, 4, 3000, 6, allowRandomCreate: true, 25, isSpecial: false, 3, 36, EMaterialProperty.Invalid, -1, 4, 0, 11, 150, 200, new List<short>(), new PoisonsAndLevels(default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short)), new List<PresetInventoryItem>(), 0, 1, 0, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, primaryRecoverAllInjuries: false, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, secondaryRecoverAllInjuries: false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 60, EMaterialFilterType.Invalid, EMaterialFilterHardness.Stone));
		_dataArray.Add(new MaterialItem(116, 232, 5, 503, 4, 112, "icon_Material_guiwenmaoyanqianshi", 233, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 0, 20, 3100, 4650, 2, 5, 4200, 7, allowRandomCreate: true, 20, isSpecial: false, 3, 36, EMaterialProperty.Invalid, -1, 4, 0, 11, 210, 400, new List<short>(), new PoisonsAndLevels(default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short)), new List<PresetInventoryItem>(), 0, 1, 0, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, primaryRecoverAllInjuries: false, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, secondaryRecoverAllInjuries: false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 60, EMaterialFilterType.Invalid, EMaterialFilterHardness.Stone));
		_dataArray.Add(new MaterialItem(117, 234, 5, 503, 5, 112, "icon_Material_guiwenmaoyanqianshi", 235, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 0, 10, 5600, 8400, 3, 6, 5400, 7, allowRandomCreate: true, 15, isSpecial: false, 3, 36, EMaterialProperty.Invalid, -1, 4, 0, 11, 280, 800, new List<short>(), new PoisonsAndLevels(default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short)), new List<PresetInventoryItem>(), 0, 1, 0, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, primaryRecoverAllInjuries: false, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, secondaryRecoverAllInjuries: false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 60, EMaterialFilterType.Invalid, EMaterialFilterHardness.Stone));
		_dataArray.Add(new MaterialItem(118, 236, 5, 503, 6, 112, "icon_Material_shenzhaodanqianshi", 237, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 0, 20, 9200, 13800, 4, 7, 7200, 8, allowRandomCreate: true, 10, isSpecial: false, 3, 36, EMaterialProperty.Invalid, -1, 4, 0, 11, 360, 1600, new List<short>(), new PoisonsAndLevels(default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short)), new List<PresetInventoryItem>(), 0, 1, 0, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, primaryRecoverAllInjuries: false, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, secondaryRecoverAllInjuries: false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 60, EMaterialFilterType.Invalid, EMaterialFilterHardness.Stone));
		_dataArray.Add(new MaterialItem(119, 238, 5, 503, 0, 119, "icon_Material_shuiyuzhuizhu", 239, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 0, 10, 100, 150, 0, 1, 600, 3, allowRandomCreate: true, 40, isSpecial: false, 3, 36, EMaterialProperty.Invalid, -1, 5, 0, 11, 30, 25, new List<short>(), new PoisonsAndLevels(default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short)), new List<PresetInventoryItem>(), 0, 1, 0, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, primaryRecoverAllInjuries: false, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, secondaryRecoverAllInjuries: false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 60, EMaterialFilterType.Invalid, EMaterialFilterHardness.Jade));
	}

	private void CreateItems2()
	{
		_dataArray.Add(new MaterialItem(120, 240, 5, 503, 1, 119, "icon_Material_shuiyuzhuizhu", 241, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 0, 10, 200, 300, 0, 2, 1200, 4, allowRandomCreate: true, 35, isSpecial: false, 3, 36, EMaterialProperty.Invalid, -1, 5, 0, 11, 60, 50, new List<short>(), new PoisonsAndLevels(default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short)), new List<PresetInventoryItem>(), 0, 1, 0, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, primaryRecoverAllInjuries: false, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, secondaryRecoverAllInjuries: false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 60, EMaterialFilterType.Invalid, EMaterialFilterHardness.Jade));
		_dataArray.Add(new MaterialItem(121, 242, 5, 503, 2, 119, "icon_Material_shuiyuzhuizhu", 243, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 0, 10, 600, 900, 0, 3, 1800, 5, allowRandomCreate: true, 30, isSpecial: false, 3, 36, EMaterialProperty.Invalid, -1, 5, 0, 11, 100, 100, new List<short>(), new PoisonsAndLevels(default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short)), new List<PresetInventoryItem>(), 0, 1, 0, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, primaryRecoverAllInjuries: false, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, secondaryRecoverAllInjuries: false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 60, EMaterialFilterType.Invalid, EMaterialFilterHardness.Jade));
		_dataArray.Add(new MaterialItem(122, 244, 5, 503, 3, 119, "icon_Material_wuseliulizhuizhu", 245, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 0, 10, 1500, 2250, 1, 4, 3000, 6, allowRandomCreate: true, 25, isSpecial: false, 3, 36, EMaterialProperty.Invalid, -1, 5, 0, 11, 150, 200, new List<short>(), new PoisonsAndLevels(default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short)), new List<PresetInventoryItem>(), 0, 1, 0, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, primaryRecoverAllInjuries: false, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, secondaryRecoverAllInjuries: false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 60, EMaterialFilterType.Invalid, EMaterialFilterHardness.Jade));
		_dataArray.Add(new MaterialItem(123, 246, 5, 503, 4, 119, "icon_Material_wuseliulizhuizhu", 247, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 0, 20, 3100, 4650, 2, 5, 4200, 7, allowRandomCreate: true, 20, isSpecial: false, 3, 36, EMaterialProperty.Invalid, -1, 5, 0, 11, 210, 400, new List<short>(), new PoisonsAndLevels(default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short)), new List<PresetInventoryItem>(), 0, 1, 0, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, primaryRecoverAllInjuries: false, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, secondaryRecoverAllInjuries: false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 60, EMaterialFilterType.Invalid, EMaterialFilterHardness.Jade));
		_dataArray.Add(new MaterialItem(124, 248, 5, 503, 5, 119, "icon_Material_wuseliulizhuizhu", 249, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 0, 10, 5600, 8400, 3, 6, 5400, 7, allowRandomCreate: true, 15, isSpecial: false, 3, 36, EMaterialProperty.Invalid, -1, 5, 0, 11, 280, 800, new List<short>(), new PoisonsAndLevels(default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short)), new List<PresetInventoryItem>(), 0, 1, 0, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, primaryRecoverAllInjuries: false, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, secondaryRecoverAllInjuries: false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 60, EMaterialFilterType.Invalid, EMaterialFilterHardness.Jade));
		_dataArray.Add(new MaterialItem(125, 250, 5, 503, 6, 119, "icon_Material_kunlunhuoyuzhuizhu", 251, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 0, 20, 9200, 13800, 4, 7, 7200, 8, allowRandomCreate: true, 10, isSpecial: false, 3, 36, EMaterialProperty.Invalid, -1, 5, 0, 11, 360, 1600, new List<short>(), new PoisonsAndLevels(default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short)), new List<PresetInventoryItem>(), 0, 1, 0, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, primaryRecoverAllInjuries: false, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, secondaryRecoverAllInjuries: false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 60, EMaterialFilterType.Invalid, EMaterialFilterHardness.Jade));
		_dataArray.Add(new MaterialItem(126, 252, 5, 504, 0, 126, "icon_Material_huyibu", 253, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 0, 10, 100, 150, 0, 1, 600, 3, allowRandomCreate: true, 40, isSpecial: false, 4, 36, EMaterialProperty.Invalid, -1, 6, 0, 10, 30, 25, new List<short>(), new PoisonsAndLevels(default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short)), new List<PresetInventoryItem>(), 0, 1, 0, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, primaryRecoverAllInjuries: false, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, secondaryRecoverAllInjuries: false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 60, EMaterialFilterType.Invalid, EMaterialFilterHardness.Fur));
		_dataArray.Add(new MaterialItem(127, 254, 5, 504, 1, 126, "icon_Material_huyibu", 255, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 0, 10, 200, 300, 0, 2, 1200, 4, allowRandomCreate: true, 35, isSpecial: false, 4, 36, EMaterialProperty.Invalid, -1, 6, 0, 10, 60, 50, new List<short>(), new PoisonsAndLevels(default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short)), new List<PresetInventoryItem>(), 0, 1, 0, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, primaryRecoverAllInjuries: false, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, secondaryRecoverAllInjuries: false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 60, EMaterialFilterType.Invalid, EMaterialFilterHardness.Fur));
		_dataArray.Add(new MaterialItem(128, 256, 5, 504, 2, 126, "icon_Material_huyibu", 257, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 0, 10, 600, 900, 0, 3, 1800, 5, allowRandomCreate: true, 30, isSpecial: false, 4, 36, EMaterialProperty.Invalid, -1, 6, 0, 10, 100, 100, new List<short>(), new PoisonsAndLevels(default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short)), new List<PresetInventoryItem>(), 0, 1, 0, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, primaryRecoverAllInjuries: false, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, secondaryRecoverAllInjuries: false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 60, EMaterialFilterType.Invalid, EMaterialFilterHardness.Fur));
		_dataArray.Add(new MaterialItem(129, 258, 5, 504, 3, 126, "icon_Material_huxianyibu", 259, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 0, 10, 1500, 2250, 1, 4, 3000, 6, allowRandomCreate: true, 25, isSpecial: false, 4, 36, EMaterialProperty.Invalid, -1, 6, 0, 10, 150, 200, new List<short>(), new PoisonsAndLevels(default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short)), new List<PresetInventoryItem>(), 0, 1, 0, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, primaryRecoverAllInjuries: false, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, secondaryRecoverAllInjuries: false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 60, EMaterialFilterType.Invalid, EMaterialFilterHardness.Fur));
		_dataArray.Add(new MaterialItem(130, 260, 5, 504, 4, 126, "icon_Material_huxianyibu", 261, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 0, 10, 3100, 4650, 2, 5, 4200, 7, allowRandomCreate: true, 20, isSpecial: false, 4, 36, EMaterialProperty.Invalid, -1, 6, 0, 10, 210, 400, new List<short>(), new PoisonsAndLevels(default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short)), new List<PresetInventoryItem>(), 0, 1, 0, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, primaryRecoverAllInjuries: false, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, secondaryRecoverAllInjuries: false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 60, EMaterialFilterType.Invalid, EMaterialFilterHardness.Fur));
		_dataArray.Add(new MaterialItem(131, 262, 5, 504, 5, 126, "icon_Material_huxianyibu", 263, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 0, 20, 5600, 8400, 3, 6, 5400, 7, allowRandomCreate: true, 15, isSpecial: false, 4, 36, EMaterialProperty.Invalid, -1, 6, 0, 10, 280, 800, new List<short>(), new PoisonsAndLevels(default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short)), new List<PresetInventoryItem>(), 0, 1, 0, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, primaryRecoverAllInjuries: false, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, secondaryRecoverAllInjuries: false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 60, EMaterialFilterType.Invalid, EMaterialFilterHardness.Fur));
		_dataArray.Add(new MaterialItem(132, 264, 5, 504, 6, 126, "icon_Material_jinlvchanyibu", 265, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 0, 10, 9200, 13800, 4, 7, 7200, 8, allowRandomCreate: true, 10, isSpecial: false, 4, 36, EMaterialProperty.Invalid, -1, 6, 0, 10, 360, 1600, new List<short>(), new PoisonsAndLevels(default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short)), new List<PresetInventoryItem>(), 0, 1, 0, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, primaryRecoverAllInjuries: false, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, secondaryRecoverAllInjuries: false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 60, EMaterialFilterType.Invalid, EMaterialFilterHardness.Fur));
		_dataArray.Add(new MaterialItem(133, 266, 5, 504, 0, 133, "icon_Material_xiuwenhuangmabu", 267, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 0, 10, 100, 150, 0, 1, 600, 3, allowRandomCreate: true, 40, isSpecial: false, 4, 36, EMaterialProperty.Invalid, -1, 7, 0, 10, 30, 25, new List<short>(), new PoisonsAndLevels(default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short)), new List<PresetInventoryItem>(), 0, 1, 0, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, primaryRecoverAllInjuries: false, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, secondaryRecoverAllInjuries: false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 60, EMaterialFilterType.Invalid, EMaterialFilterHardness.Woven));
		_dataArray.Add(new MaterialItem(134, 268, 5, 504, 1, 133, "icon_Material_xiuwenhuangmabu", 269, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 0, 10, 200, 300, 0, 2, 1200, 4, allowRandomCreate: true, 35, isSpecial: false, 4, 36, EMaterialProperty.Invalid, -1, 7, 0, 10, 60, 50, new List<short>(), new PoisonsAndLevels(default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short)), new List<PresetInventoryItem>(), 0, 1, 0, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, primaryRecoverAllInjuries: false, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, secondaryRecoverAllInjuries: false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 60, EMaterialFilterType.Invalid, EMaterialFilterHardness.Woven));
		_dataArray.Add(new MaterialItem(135, 270, 5, 504, 2, 133, "icon_Material_xiuwenhuangmabu", 271, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 0, 10, 600, 900, 0, 3, 1800, 5, allowRandomCreate: true, 30, isSpecial: false, 4, 36, EMaterialProperty.Invalid, -1, 7, 0, 10, 100, 100, new List<short>(), new PoisonsAndLevels(default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short)), new List<PresetInventoryItem>(), 0, 1, 0, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, primaryRecoverAllInjuries: false, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, secondaryRecoverAllInjuries: false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 60, EMaterialFilterType.Invalid, EMaterialFilterHardness.Woven));
		_dataArray.Add(new MaterialItem(136, 272, 5, 504, 3, 133, "icon_Material_xuanjinruansibu", 273, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 0, 10, 1500, 2250, 1, 4, 3000, 6, allowRandomCreate: true, 25, isSpecial: false, 4, 36, EMaterialProperty.Invalid, -1, 7, 0, 10, 150, 200, new List<short>(), new PoisonsAndLevels(default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short)), new List<PresetInventoryItem>(), 0, 1, 0, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, primaryRecoverAllInjuries: false, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, secondaryRecoverAllInjuries: false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 60, EMaterialFilterType.Invalid, EMaterialFilterHardness.Woven));
		_dataArray.Add(new MaterialItem(137, 274, 5, 504, 4, 133, "icon_Material_xuanjinruansibu", 275, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 0, 10, 3100, 4650, 2, 5, 4200, 7, allowRandomCreate: true, 20, isSpecial: false, 4, 36, EMaterialProperty.Invalid, -1, 7, 0, 10, 210, 400, new List<short>(), new PoisonsAndLevels(default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short)), new List<PresetInventoryItem>(), 0, 1, 0, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, primaryRecoverAllInjuries: false, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, secondaryRecoverAllInjuries: false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 60, EMaterialFilterType.Invalid, EMaterialFilterHardness.Woven));
		_dataArray.Add(new MaterialItem(138, 276, 5, 504, 5, 133, "icon_Material_xuanjinruansibu", 277, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 0, 10, 5600, 8400, 3, 6, 5400, 7, allowRandomCreate: true, 15, isSpecial: false, 4, 36, EMaterialProperty.Invalid, -1, 7, 0, 10, 280, 800, new List<short>(), new PoisonsAndLevels(default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short)), new List<PresetInventoryItem>(), 0, 1, 0, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, primaryRecoverAllInjuries: false, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, secondaryRecoverAllInjuries: false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 60, EMaterialFilterType.Invalid, EMaterialFilterHardness.Woven));
		_dataArray.Add(new MaterialItem(139, 278, 5, 504, 6, 133, "icon_Material_tiancansibu", 279, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 0, 10, 9200, 13800, 4, 7, 7200, 8, allowRandomCreate: true, 10, isSpecial: false, 4, 36, EMaterialProperty.Invalid, -1, 7, 0, 10, 360, 1600, new List<short>(), new PoisonsAndLevels(default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short)), new List<PresetInventoryItem>(), 0, 1, 0, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, primaryRecoverAllInjuries: false, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, secondaryRecoverAllInjuries: false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 60, EMaterialFilterType.Invalid, EMaterialFilterHardness.Woven));
		_dataArray.Add(new MaterialItem(140, 280, 5, 505, 1, 140, "icon_Material_jiegucao", 281, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 0, 10, 100, 100, 0, 2, 1200, 4, allowRandomCreate: true, 40, isSpecial: false, 5, 12, EMaterialProperty.Yang, -1, -1, 25, 8, 30, 5, new List<short> { 158, 2 }, new PoisonsAndLevels(default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short)), new List<PresetInventoryItem>(), 0, 1, 0, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, primaryRecoverAllInjuries: false, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, secondaryRecoverAllInjuries: false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 60, EMaterialFilterType.Invalid, EMaterialFilterHardness.Invalid));
		_dataArray.Add(new MaterialItem(141, 282, 5, 505, 3, 140, "icon_Material_fudeyanhusuo", 283, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 0, 10, 750, 750, 0, 4, 3000, 5, allowRandomCreate: true, 30, isSpecial: false, 5, 12, EMaterialProperty.Yang, -1, -1, 100, 8, 100, 20, new List<short> { 158, 2 }, new PoisonsAndLevels(default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short)), new List<PresetInventoryItem>(), 0, 1, 0, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, primaryRecoverAllInjuries: false, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, secondaryRecoverAllInjuries: false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 60, EMaterialFilterType.Invalid, EMaterialFilterHardness.Invalid));
		_dataArray.Add(new MaterialItem(142, 284, 5, 505, 5, 140, "icon_Material_fudeyanhusuo", 285, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 0, 10, 2800, 2800, 2, 6, 5400, 7, allowRandomCreate: true, 20, isSpecial: false, 5, 12, EMaterialProperty.Yang, -1, -1, 300, 8, 210, 80, new List<short> { 158, 2 }, new PoisonsAndLevels(default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short)), new List<PresetInventoryItem>(), 0, 1, 0, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, primaryRecoverAllInjuries: false, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, secondaryRecoverAllInjuries: false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 60, EMaterialFilterType.Invalid, EMaterialFilterHardness.Invalid));
		_dataArray.Add(new MaterialItem(143, 286, 5, 505, 7, 140, "icon_Material_qiannianhuolingzhi", 287, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 0, 10, 7050, 7050, 4, 8, 9000, 8, allowRandomCreate: true, 10, isSpecial: false, 5, 12, EMaterialProperty.Yang, -1, -1, 500, 8, 360, 320, new List<short> { 158, 2 }, new PoisonsAndLevels(default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short)), new List<PresetInventoryItem>(), 0, 1, 6, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, primaryRecoverAllInjuries: false, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, secondaryRecoverAllInjuries: false, 100, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 60, EMaterialFilterType.Invalid, EMaterialFilterHardness.Invalid));
		_dataArray.Add(new MaterialItem(144, 288, 5, 505, 1, 144, "icon_Material_zizhucao", 289, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 0, 10, 100, 100, 0, 2, 1200, 4, allowRandomCreate: true, 40, isSpecial: false, 5, 12, EMaterialProperty.Yang, -1, -1, 25, 8, 30, 5, new List<short> { 159, 2 }, new PoisonsAndLevels(default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short)), new List<PresetInventoryItem>(), 0, 1, 0, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, primaryRecoverAllInjuries: false, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, secondaryRecoverAllInjuries: false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 60, EMaterialFilterType.Invalid, EMaterialFilterHardness.Invalid));
		_dataArray.Add(new MaterialItem(145, 290, 5, 505, 3, 144, "icon_Material_xueshanjiuniucao", 291, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 0, 10, 750, 750, 0, 4, 3000, 5, allowRandomCreate: true, 30, isSpecial: false, 5, 12, EMaterialProperty.Yang, -1, -1, 100, 8, 100, 20, new List<short> { 159, 2 }, new PoisonsAndLevels(default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short)), new List<PresetInventoryItem>(), 0, 1, 0, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, primaryRecoverAllInjuries: false, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, secondaryRecoverAllInjuries: false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 60, EMaterialFilterType.Invalid, EMaterialFilterHardness.Invalid));
		_dataArray.Add(new MaterialItem(146, 292, 5, 505, 5, 144, "icon_Material_xueshanjiuniucao", 293, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 0, 10, 2800, 2800, 2, 6, 5400, 7, allowRandomCreate: true, 20, isSpecial: false, 5, 12, EMaterialProperty.Yang, -1, -1, 300, 8, 210, 80, new List<short> { 159, 2 }, new PoisonsAndLevels(default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short)), new List<PresetInventoryItem>(), 0, 1, 0, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, primaryRecoverAllInjuries: false, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, secondaryRecoverAllInjuries: false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 60, EMaterialFilterType.Invalid, EMaterialFilterHardness.Invalid));
		_dataArray.Add(new MaterialItem(147, 294, 5, 505, 7, 144, "icon_Material_yufolou", 295, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 0, 10, 7050, 7050, 4, 8, 9000, 8, allowRandomCreate: true, 10, isSpecial: false, 5, 12, EMaterialProperty.Yang, -1, -1, 500, 8, 360, 320, new List<short> { 159, 2 }, new PoisonsAndLevels(default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short)), new List<PresetInventoryItem>(), 0, 1, 4, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, primaryRecoverAllInjuries: false, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, secondaryRecoverAllInjuries: false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 80, 0, 0, 2, 60, EMaterialFilterType.Invalid, EMaterialFilterHardness.Invalid));
		_dataArray.Add(new MaterialItem(148, 296, 5, 505, 1, 148, "icon_Material_qiannianjian", 297, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 0, 10, 100, 100, 0, 2, 1200, 4, allowRandomCreate: true, 40, isSpecial: false, 5, 12, EMaterialProperty.Yang, -1, -1, 25, 8, 30, 5, new List<short> { 160, 2 }, new PoisonsAndLevels(default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short)), new List<PresetInventoryItem>(), 0, 1, 0, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, primaryRecoverAllInjuries: false, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, secondaryRecoverAllInjuries: false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 60, EMaterialFilterType.Invalid, EMaterialFilterHardness.Invalid));
		_dataArray.Add(new MaterialItem(149, 298, 5, 505, 3, 148, "icon_Material_zihuasheshecao", 299, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 0, 10, 750, 750, 0, 4, 3000, 5, allowRandomCreate: true, 30, isSpecial: false, 5, 12, EMaterialProperty.Yang, -1, -1, 100, 8, 100, 20, new List<short> { 160, 2 }, new PoisonsAndLevels(default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short)), new List<PresetInventoryItem>(), 0, 1, 0, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, primaryRecoverAllInjuries: false, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, secondaryRecoverAllInjuries: false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 60, EMaterialFilterType.Invalid, EMaterialFilterHardness.Invalid));
		_dataArray.Add(new MaterialItem(150, 300, 5, 505, 5, 148, "icon_Material_zihuasheshecao", 301, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 0, 10, 2800, 2800, 2, 6, 5400, 7, allowRandomCreate: true, 20, isSpecial: false, 5, 12, EMaterialProperty.Yang, -1, -1, 300, 8, 210, 80, new List<short> { 160, 2 }, new PoisonsAndLevels(default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short)), new List<PresetInventoryItem>(), 0, 1, 0, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, primaryRecoverAllInjuries: false, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, secondaryRecoverAllInjuries: false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 60, EMaterialFilterType.Invalid, EMaterialFilterHardness.Invalid));
		_dataArray.Add(new MaterialItem(151, 302, 5, 505, 7, 148, "icon_Material_nvwadan", 303, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 0, 10, 7050, 7050, 4, 8, 9000, 8, allowRandomCreate: true, 10, isSpecial: false, 5, 12, EMaterialProperty.Yang, -1, -1, 500, 8, 360, 320, new List<short> { 160, 2 }, new PoisonsAndLevels(default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short)), new List<PresetInventoryItem>(), 0, 1, 2, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, primaryRecoverAllInjuries: false, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, secondaryRecoverAllInjuries: false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 200, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 40, 0, 0, 2, 60, EMaterialFilterType.Invalid, EMaterialFilterHardness.Invalid));
		_dataArray.Add(new MaterialItem(152, 304, 5, 505, 1, 152, "icon_Material_hugu", 305, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 0, 20, 100, 100, 0, 2, 1200, 4, allowRandomCreate: true, 40, isSpecial: false, 5, 12, EMaterialProperty.Yang, -1, -1, 25, 8, 30, 5, new List<short> { 161, 2 }, new PoisonsAndLevels(default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short)), new List<PresetInventoryItem>(), 0, 1, 0, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, primaryRecoverAllInjuries: false, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, secondaryRecoverAllInjuries: false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 60, EMaterialFilterType.Invalid, EMaterialFilterHardness.Invalid));
		_dataArray.Add(new MaterialItem(153, 306, 5, 505, 3, 152, "icon_Material_sulongcao", 307, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 0, 10, 750, 750, 0, 4, 3000, 5, allowRandomCreate: true, 30, isSpecial: false, 5, 12, EMaterialProperty.Yang, -1, -1, 100, 8, 100, 20, new List<short> { 161, 2 }, new PoisonsAndLevels(default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short)), new List<PresetInventoryItem>(), 0, 1, 0, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, primaryRecoverAllInjuries: false, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, secondaryRecoverAllInjuries: false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 60, EMaterialFilterType.Invalid, EMaterialFilterHardness.Invalid));
		_dataArray.Add(new MaterialItem(154, 308, 5, 505, 5, 152, "icon_Material_sulongcao", 309, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 0, 20, 2800, 2800, 2, 6, 5400, 7, allowRandomCreate: true, 20, isSpecial: false, 5, 12, EMaterialProperty.Yang, -1, -1, 300, 8, 210, 80, new List<short> { 161, 2 }, new PoisonsAndLevels(default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short)), new List<PresetInventoryItem>(), 0, 1, 0, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, primaryRecoverAllInjuries: false, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, secondaryRecoverAllInjuries: false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 60, EMaterialFilterType.Invalid, EMaterialFilterHardness.Invalid));
		_dataArray.Add(new MaterialItem(155, 310, 5, 505, 7, 152, "icon_Material_chifuxiegui", 311, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 0, 20, 7050, 7050, 4, 8, 9000, 8, allowRandomCreate: true, 10, isSpecial: false, 5, 12, EMaterialProperty.Yang, -1, -1, 500, 8, 360, 320, new List<short> { 161, 2 }, new PoisonsAndLevels(default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short)), new List<PresetInventoryItem>(), 0, 1, 2, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, primaryRecoverAllInjuries: false, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, secondaryRecoverAllInjuries: false, 200, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 60, EMaterialFilterType.Invalid, EMaterialFilterHardness.Invalid));
		_dataArray.Add(new MaterialItem(156, 312, 5, 505, 1, 156, "icon_Material_renshen", 313, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 0, 10, 100, 100, 0, 2, 1200, 4, allowRandomCreate: true, 40, isSpecial: false, 5, 12, EMaterialProperty.Yin, -1, -1, 25, 8, 30, 5, new List<short> { 162, 2 }, new PoisonsAndLevels(default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short)), new List<PresetInventoryItem>(), 0, 1, 0, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, primaryRecoverAllInjuries: false, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, secondaryRecoverAllInjuries: false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 60, EMaterialFilterType.Invalid, EMaterialFilterHardness.Invalid));
		_dataArray.Add(new MaterialItem(157, 314, 5, 505, 3, 156, "icon_Material_ziqingjiangxiang", 315, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 0, 10, 750, 750, 0, 4, 3000, 5, allowRandomCreate: true, 30, isSpecial: false, 5, 12, EMaterialProperty.Yin, -1, -1, 100, 8, 100, 20, new List<short> { 162, 2 }, new PoisonsAndLevels(default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short)), new List<PresetInventoryItem>(), 0, 1, 0, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, primaryRecoverAllInjuries: false, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, secondaryRecoverAllInjuries: false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 60, EMaterialFilterType.Invalid, EMaterialFilterHardness.Invalid));
		_dataArray.Add(new MaterialItem(158, 316, 5, 505, 5, 156, "icon_Material_ziqingjiangxiang", 317, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 0, 10, 2800, 2800, 2, 6, 5400, 7, allowRandomCreate: true, 20, isSpecial: false, 5, 12, EMaterialProperty.Yin, -1, -1, 300, 8, 210, 80, new List<short> { 162, 2 }, new PoisonsAndLevels(default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short)), new List<PresetInventoryItem>(), 0, 1, 0, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, primaryRecoverAllInjuries: false, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, secondaryRecoverAllInjuries: false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 60, EMaterialFilterType.Invalid, EMaterialFilterHardness.Invalid));
		_dataArray.Add(new MaterialItem(159, 318, 5, 505, 7, 156, "icon_Material_qiannianxuecan", 319, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 0, 10, 7050, 7050, 4, 8, 9000, 8, allowRandomCreate: true, 10, isSpecial: false, 5, 12, EMaterialProperty.Yin, -1, -1, 500, 8, 360, 320, new List<short> { 162, 2 }, new PoisonsAndLevels(default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short)), new List<PresetInventoryItem>(), 0, 1, 6, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, primaryRecoverAllInjuries: false, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, secondaryRecoverAllInjuries: false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 50, 50, 0, 0, 0, 0, 0, 0, 0, 0, 2, 60, EMaterialFilterType.Invalid, EMaterialFilterHardness.Invalid));
		_dataArray.Add(new MaterialItem(160, 320, 5, 505, 1, 160, "icon_Material_qingshedan", 321, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 0, 10, 100, 100, 0, 2, 1200, 4, allowRandomCreate: true, 40, isSpecial: false, 5, 12, EMaterialProperty.Yin, -1, -1, 25, 8, 30, 5, new List<short> { 163, 2 }, new PoisonsAndLevels(default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short)), new List<PresetInventoryItem>(), 0, 1, 0, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, primaryRecoverAllInjuries: false, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, secondaryRecoverAllInjuries: false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 60, EMaterialFilterType.Invalid, EMaterialFilterHardness.Invalid));
		_dataArray.Add(new MaterialItem(161, 322, 5, 505, 3, 160, "icon_Material_jinbanwuyao", 323, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 0, 10, 750, 750, 0, 4, 3000, 5, allowRandomCreate: true, 30, isSpecial: false, 5, 12, EMaterialProperty.Yin, -1, -1, 100, 8, 100, 20, new List<short> { 163, 2 }, new PoisonsAndLevels(default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short)), new List<PresetInventoryItem>(), 0, 1, 0, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, primaryRecoverAllInjuries: false, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, secondaryRecoverAllInjuries: false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 60, EMaterialFilterType.Invalid, EMaterialFilterHardness.Invalid));
		_dataArray.Add(new MaterialItem(162, 324, 5, 505, 5, 160, "icon_Material_jinbanwuyao", 325, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 0, 10, 2800, 2800, 2, 6, 5400, 7, allowRandomCreate: true, 20, isSpecial: false, 5, 12, EMaterialProperty.Yin, -1, -1, 300, 8, 210, 80, new List<short> { 163, 2 }, new PoisonsAndLevels(default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short)), new List<PresetInventoryItem>(), 0, 1, 0, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, primaryRecoverAllInjuries: false, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, secondaryRecoverAllInjuries: false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 60, EMaterialFilterType.Invalid, EMaterialFilterHardness.Invalid));
		_dataArray.Add(new MaterialItem(163, 326, 5, 505, 7, 160, "icon_Material_ziyuwangcan", 327, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 0, 10, 7050, 7050, 4, 8, 9000, 8, allowRandomCreate: true, 10, isSpecial: false, 5, 12, EMaterialProperty.Yin, -1, -1, 500, 8, 360, 320, new List<short> { 163, 2 }, new PoisonsAndLevels(default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short)), new List<PresetInventoryItem>(), 0, 1, 4, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, primaryRecoverAllInjuries: false, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, secondaryRecoverAllInjuries: false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 80, 0, 0, 0, 0, 2, 60, EMaterialFilterType.Invalid, EMaterialFilterHardness.Invalid));
		_dataArray.Add(new MaterialItem(164, 328, 5, 505, 1, 164, "icon_Material_suhexiang", 329, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 0, 10, 100, 100, 0, 2, 1200, 4, allowRandomCreate: true, 40, isSpecial: false, 5, 12, EMaterialProperty.Yin, -1, -1, 25, 8, 30, 5, new List<short> { 164, 2 }, new PoisonsAndLevels(default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short)), new List<PresetInventoryItem>(), 0, 1, 0, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, primaryRecoverAllInjuries: false, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, secondaryRecoverAllInjuries: false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 60, EMaterialFilterType.Invalid, EMaterialFilterHardness.Invalid));
		_dataArray.Add(new MaterialItem(165, 330, 5, 505, 3, 164, "icon_Material_yewudan", 331, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 0, 10, 750, 750, 0, 4, 3000, 5, allowRandomCreate: true, 30, isSpecial: false, 5, 12, EMaterialProperty.Yin, -1, -1, 100, 8, 100, 20, new List<short> { 164, 2 }, new PoisonsAndLevels(default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short)), new List<PresetInventoryItem>(), 0, 1, 0, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, primaryRecoverAllInjuries: false, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, secondaryRecoverAllInjuries: false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 60, EMaterialFilterType.Invalid, EMaterialFilterHardness.Invalid));
		_dataArray.Add(new MaterialItem(166, 332, 5, 505, 5, 164, "icon_Material_yewudan", 333, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 0, 10, 2800, 2800, 2, 6, 5400, 7, allowRandomCreate: true, 20, isSpecial: false, 5, 12, EMaterialProperty.Yin, -1, -1, 300, 8, 210, 80, new List<short> { 164, 2 }, new PoisonsAndLevels(default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short)), new List<PresetInventoryItem>(), 0, 1, 0, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, primaryRecoverAllInjuries: false, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, secondaryRecoverAllInjuries: false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 60, EMaterialFilterType.Invalid, EMaterialFilterHardness.Invalid));
		_dataArray.Add(new MaterialItem(167, 334, 5, 505, 7, 164, "icon_Material_yaochilan", 335, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 0, 10, 7050, 7050, 4, 8, 9000, 8, allowRandomCreate: true, 10, isSpecial: false, 5, 12, EMaterialProperty.Yin, -1, -1, 500, 8, 360, 320, new List<short> { 164, 2 }, new PoisonsAndLevels(default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short)), new List<PresetInventoryItem>(), 0, 1, 2, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, primaryRecoverAllInjuries: false, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, secondaryRecoverAllInjuries: false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 200, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 40, 0, 0, 0, 0, 2, 60, EMaterialFilterType.Invalid, EMaterialFilterHardness.Invalid));
		_dataArray.Add(new MaterialItem(168, 336, 5, 505, 1, 168, "icon_Material_suxinhua", 337, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 0, 10, 100, 100, 0, 2, 1200, 4, allowRandomCreate: true, 40, isSpecial: false, 5, 12, EMaterialProperty.Yin, -1, -1, 25, 8, 30, 5, new List<short> { 165, 2 }, new PoisonsAndLevels(default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short)), new List<PresetInventoryItem>(), 0, 1, 0, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, primaryRecoverAllInjuries: false, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, secondaryRecoverAllInjuries: false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 60, EMaterialFilterType.Invalid, EMaterialFilterHardness.Invalid));
		_dataArray.Add(new MaterialItem(169, 338, 5, 505, 3, 168, "icon_Material_anhunxiang", 339, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 0, 10, 750, 750, 0, 4, 3000, 5, allowRandomCreate: true, 30, isSpecial: false, 5, 12, EMaterialProperty.Yin, -1, -1, 100, 8, 100, 20, new List<short> { 165, 2 }, new PoisonsAndLevels(default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short)), new List<PresetInventoryItem>(), 0, 1, 0, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, primaryRecoverAllInjuries: false, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, secondaryRecoverAllInjuries: false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 60, EMaterialFilterType.Invalid, EMaterialFilterHardness.Invalid));
		_dataArray.Add(new MaterialItem(170, 340, 5, 505, 5, 168, "icon_Material_anhunxiang", 341, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 0, 10, 2800, 2800, 2, 6, 5400, 7, allowRandomCreate: true, 20, isSpecial: false, 5, 12, EMaterialProperty.Yin, -1, -1, 300, 8, 210, 80, new List<short> { 165, 2 }, new PoisonsAndLevels(default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short)), new List<PresetInventoryItem>(), 0, 1, 0, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, primaryRecoverAllInjuries: false, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, secondaryRecoverAllInjuries: false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 60, EMaterialFilterType.Invalid, EMaterialFilterHardness.Invalid));
		_dataArray.Add(new MaterialItem(171, 342, 5, 505, 7, 168, "icon_Material_tianxiangqiongyushidan", 343, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 0, 10, 7050, 7050, 4, 8, 9000, 8, allowRandomCreate: true, 10, isSpecial: false, 5, 12, EMaterialProperty.Yin, -1, -1, 500, 8, 360, 320, new List<short> { 165, 2 }, new PoisonsAndLevels(default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short)), new List<PresetInventoryItem>(), 0, 1, 2, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, primaryRecoverAllInjuries: false, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, secondaryRecoverAllInjuries: false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 0, 0, 0, 0, 0, 0, 100, 100, 0, 0, 0, 0, 0, 0, 0, 0, 2, 60, EMaterialFilterType.Invalid, EMaterialFilterHardness.Invalid));
		_dataArray.Add(new MaterialItem(172, 344, 5, 505, 1, 172, "icon_Material_gongfengmi", 345, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 0, 10, 100, 100, 0, 2, 1200, 4, allowRandomCreate: true, 40, isSpecial: false, 5, 12, EMaterialProperty.Yin, -1, -1, 25, 8, 30, 5, new List<short> { 166, 2 }, new PoisonsAndLevels(default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short)), new List<PresetInventoryItem>(), 0, 1, 0, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, primaryRecoverAllInjuries: false, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, secondaryRecoverAllInjuries: false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 60, EMaterialFilterType.Invalid, EMaterialFilterHardness.Invalid));
		_dataArray.Add(new MaterialItem(173, 346, 5, 505, 3, 172, "icon_Material_yuluxie", 347, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 0, 10, 750, 750, 0, 4, 3000, 5, allowRandomCreate: true, 30, isSpecial: false, 5, 12, EMaterialProperty.Yin, -1, -1, 100, 8, 100, 20, new List<short> { 166, 2 }, new PoisonsAndLevels(default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short)), new List<PresetInventoryItem>(), 0, 1, 0, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, primaryRecoverAllInjuries: false, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, secondaryRecoverAllInjuries: false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 60, EMaterialFilterType.Invalid, EMaterialFilterHardness.Invalid));
		_dataArray.Add(new MaterialItem(174, 348, 5, 505, 5, 172, "icon_Material_yuluxie", 349, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 0, 30, 2800, 2800, 2, 6, 5400, 7, allowRandomCreate: true, 20, isSpecial: false, 5, 12, EMaterialProperty.Yin, -1, -1, 300, 8, 210, 80, new List<short> { 166, 2 }, new PoisonsAndLevels(default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short)), new List<PresetInventoryItem>(), 0, 1, 0, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, primaryRecoverAllInjuries: false, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, secondaryRecoverAllInjuries: false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 60, EMaterialFilterType.Invalid, EMaterialFilterHardness.Invalid));
		_dataArray.Add(new MaterialItem(175, 350, 5, 505, 7, 172, "icon_Material_tianshanxuelian", 351, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 0, 10, 7050, 7050, 4, 8, 9000, 8, allowRandomCreate: true, 10, isSpecial: false, 5, 12, EMaterialProperty.Yin, -1, -1, 500, 8, 360, 320, new List<short> { 166, 2 }, new PoisonsAndLevels(default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short)), new List<PresetInventoryItem>(), 0, 1, 6, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, primaryRecoverAllInjuries: false, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, secondaryRecoverAllInjuries: false, 0, 100, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 60, EMaterialFilterType.Invalid, EMaterialFilterHardness.Invalid));
		_dataArray.Add(new MaterialItem(176, 352, 5, 505, 1, 176, "icon_Material_wushegu", 353, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 0, 10, 100, 100, 0, 2, 1200, 4, allowRandomCreate: true, 40, isSpecial: false, 5, 12, EMaterialProperty.Yang, -1, -1, 25, 8, 30, 5, new List<short> { 167, 2 }, new PoisonsAndLevels(default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short)), new List<PresetInventoryItem>(), 0, 1, 0, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, primaryRecoverAllInjuries: false, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, secondaryRecoverAllInjuries: false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 60, EMaterialFilterType.Invalid, EMaterialFilterHardness.Invalid));
		_dataArray.Add(new MaterialItem(177, 354, 5, 505, 3, 176, "icon_Material_hongluodingxiang", 355, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 0, 10, 750, 750, 0, 4, 3000, 5, allowRandomCreate: true, 30, isSpecial: false, 5, 12, EMaterialProperty.Yang, -1, -1, 100, 8, 100, 20, new List<short> { 167, 2 }, new PoisonsAndLevels(default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short)), new List<PresetInventoryItem>(), 0, 1, 0, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, primaryRecoverAllInjuries: false, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, secondaryRecoverAllInjuries: false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 60, EMaterialFilterType.Invalid, EMaterialFilterHardness.Invalid));
		_dataArray.Add(new MaterialItem(178, 356, 5, 505, 5, 176, "icon_Material_hongluodingxiang", 357, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 0, 10, 2800, 2800, 2, 6, 5400, 7, allowRandomCreate: true, 20, isSpecial: false, 5, 12, EMaterialProperty.Yang, -1, -1, 300, 8, 210, 80, new List<short> { 167, 2 }, new PoisonsAndLevels(default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short)), new List<PresetInventoryItem>(), 0, 1, 0, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, primaryRecoverAllInjuries: false, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, secondaryRecoverAllInjuries: false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 60, EMaterialFilterType.Invalid, EMaterialFilterHardness.Invalid));
		_dataArray.Add(new MaterialItem(179, 358, 5, 505, 7, 176, "icon_Material_longgexielou", 359, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 0, 10, 7050, 7050, 4, 8, 9000, 8, allowRandomCreate: true, 10, isSpecial: false, 5, 12, EMaterialProperty.Yang, -1, -1, 500, 8, 360, 320, new List<short> { 167, 2 }, new PoisonsAndLevels(default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short)), new List<PresetInventoryItem>(), 0, 1, 4, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, primaryRecoverAllInjuries: false, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, secondaryRecoverAllInjuries: false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 80, 0, 0, 0, 2, 60, EMaterialFilterType.Invalid, EMaterialFilterHardness.Invalid));
	}

	private void CreateItems3()
	{
		_dataArray.Add(new MaterialItem(180, 360, 5, 505, 1, 180, "icon_Material_changshengbaihe", 361, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 0, 10, 100, 100, 0, 2, 1200, 4, allowRandomCreate: true, 40, isSpecial: false, 5, 12, EMaterialProperty.Yang, -1, -1, 25, 8, 30, 5, new List<short> { 168, 2 }, new PoisonsAndLevels(default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short)), new List<PresetInventoryItem>(), 0, 1, 0, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, primaryRecoverAllInjuries: false, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, secondaryRecoverAllInjuries: false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 60, EMaterialFilterType.Invalid, EMaterialFilterHardness.Invalid));
		_dataArray.Add(new MaterialItem(181, 362, 5, 505, 3, 180, "icon_Material_qixianglingzhi", 363, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 0, 10, 750, 750, 0, 4, 3000, 5, allowRandomCreate: true, 30, isSpecial: false, 5, 12, EMaterialProperty.Yang, -1, -1, 100, 8, 100, 20, new List<short> { 168, 2 }, new PoisonsAndLevels(default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short)), new List<PresetInventoryItem>(), 0, 1, 0, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, primaryRecoverAllInjuries: false, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, secondaryRecoverAllInjuries: false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 60, EMaterialFilterType.Invalid, EMaterialFilterHardness.Invalid));
		_dataArray.Add(new MaterialItem(182, 364, 5, 505, 5, 180, "icon_Material_qixianglingzhi", 365, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 0, 20, 2800, 2800, 2, 6, 5400, 7, allowRandomCreate: true, 20, isSpecial: false, 5, 12, EMaterialProperty.Yang, -1, -1, 300, 8, 210, 80, new List<short> { 168, 2 }, new PoisonsAndLevels(default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short)), new List<PresetInventoryItem>(), 0, 1, 0, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, primaryRecoverAllInjuries: false, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, secondaryRecoverAllInjuries: false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 60, EMaterialFilterType.Invalid, EMaterialFilterHardness.Invalid));
		_dataArray.Add(new MaterialItem(183, 366, 5, 505, 7, 180, "icon_Material_jiuseyuputi", 367, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 0, 10, 7050, 7050, 4, 8, 9000, 8, allowRandomCreate: true, 10, isSpecial: false, 5, 12, EMaterialProperty.Yang, -1, -1, 500, 8, 360, 320, new List<short> { 168, 2 }, new PoisonsAndLevels(default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short)), new List<PresetInventoryItem>(), 0, 1, 2, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, primaryRecoverAllInjuries: false, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, secondaryRecoverAllInjuries: false, 0, 0, 0, 0, 0, 0, 200, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 40, 0, 0, 0, 2, 60, EMaterialFilterType.Invalid, EMaterialFilterHardness.Invalid));
		_dataArray.Add(new MaterialItem(184, 368, 5, 505, 1, 184, "icon_Material_mianhuangqi", 369, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 0, 10, 100, 100, 0, 2, 1200, 4, allowRandomCreate: true, 40, isSpecial: false, 5, 12, EMaterialProperty.Yang, -1, -1, 25, 8, 30, 5, new List<short> { 169, 2 }, new PoisonsAndLevels(default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short)), new List<PresetInventoryItem>(), 0, 1, 0, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, primaryRecoverAllInjuries: false, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, secondaryRecoverAllInjuries: false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 60, EMaterialFilterType.Invalid, EMaterialFilterHardness.Invalid));
		_dataArray.Add(new MaterialItem(185, 370, 5, 505, 3, 184, "icon_Material_feicuizhi", 371, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 0, 10, 750, 750, 0, 4, 3000, 5, allowRandomCreate: true, 30, isSpecial: false, 5, 12, EMaterialProperty.Yang, -1, -1, 100, 8, 100, 20, new List<short> { 169, 2 }, new PoisonsAndLevels(default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short)), new List<PresetInventoryItem>(), 0, 1, 0, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, primaryRecoverAllInjuries: false, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, secondaryRecoverAllInjuries: false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 60, EMaterialFilterType.Invalid, EMaterialFilterHardness.Invalid));
		_dataArray.Add(new MaterialItem(186, 372, 5, 505, 5, 184, "icon_Material_feicuizhi", 373, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 0, 10, 2800, 2800, 2, 6, 5400, 7, allowRandomCreate: true, 20, isSpecial: false, 5, 12, EMaterialProperty.Yang, -1, -1, 300, 8, 210, 80, new List<short> { 169, 2 }, new PoisonsAndLevels(default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short)), new List<PresetInventoryItem>(), 0, 1, 0, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, primaryRecoverAllInjuries: false, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, secondaryRecoverAllInjuries: false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 60, EMaterialFilterType.Invalid, EMaterialFilterHardness.Invalid));
		_dataArray.Add(new MaterialItem(187, 374, 5, 505, 7, 184, "icon_Material_longxianshiru", 375, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 0, 10, 7050, 7050, 4, 8, 9000, 8, allowRandomCreate: true, 10, isSpecial: false, 5, 12, EMaterialProperty.Yang, -1, -1, 500, 8, 360, 320, new List<short> { 169, 2 }, new PoisonsAndLevels(default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short)), new List<PresetInventoryItem>(), 0, 1, 2, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, primaryRecoverAllInjuries: false, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, secondaryRecoverAllInjuries: false, 0, 200, 0, 0, 0, 0, 100, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 60, EMaterialFilterType.Invalid, EMaterialFilterHardness.Invalid));
		_dataArray.Add(new MaterialItem(188, 376, 5, 505, 1, 188, "icon_Material_xuege", 377, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 0, 10, 100, 100, 0, 2, 1200, 4, allowRandomCreate: true, 40, isSpecial: false, 5, 12, EMaterialProperty.Yang, -1, -1, 25, 8, 30, 5, new List<short> { 170, 2 }, new PoisonsAndLevels(default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short)), new List<PresetInventoryItem>(), 0, 1, 0, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, primaryRecoverAllInjuries: false, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, secondaryRecoverAllInjuries: false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 60, EMaterialFilterType.Invalid, EMaterialFilterHardness.Invalid));
		_dataArray.Add(new MaterialItem(189, 378, 5, 505, 3, 188, "icon_Material_lingzhicao", 379, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 0, 10, 750, 750, 0, 4, 3000, 5, allowRandomCreate: true, 30, isSpecial: false, 5, 12, EMaterialProperty.Yang, -1, -1, 100, 8, 100, 20, new List<short> { 170, 2 }, new PoisonsAndLevels(default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short)), new List<PresetInventoryItem>(), 0, 1, 0, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, primaryRecoverAllInjuries: false, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, secondaryRecoverAllInjuries: false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 60, EMaterialFilterType.Invalid, EMaterialFilterHardness.Invalid));
		_dataArray.Add(new MaterialItem(190, 380, 5, 505, 5, 188, "icon_Material_lingzhicao", 381, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 0, 10, 2800, 2800, 2, 6, 5400, 7, allowRandomCreate: true, 20, isSpecial: false, 5, 12, EMaterialProperty.Yang, -1, -1, 300, 8, 210, 80, new List<short> { 170, 2 }, new PoisonsAndLevels(default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short)), new List<PresetInventoryItem>(), 0, 1, 0, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, primaryRecoverAllInjuries: false, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, secondaryRecoverAllInjuries: false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 60, EMaterialFilterType.Invalid, EMaterialFilterHardness.Invalid));
		_dataArray.Add(new MaterialItem(191, 382, 5, 505, 7, 188, "icon_Material_renxingheshouwu", 383, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 0, 20, 7050, 7050, 4, 8, 9000, 8, allowRandomCreate: true, 10, isSpecial: false, 5, 12, EMaterialProperty.Yang, -1, -1, 500, 8, 360, 320, new List<short> { 170, 2 }, new PoisonsAndLevels(default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short)), new List<PresetInventoryItem>(), 0, 1, 6, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, primaryRecoverAllInjuries: false, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, secondaryRecoverAllInjuries: false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 50, 50, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 60, EMaterialFilterType.Invalid, EMaterialFilterHardness.Invalid));
		_dataArray.Add(new MaterialItem(192, 384, 5, 505, 1, 192, "icon_Material_zhenzhumu", 385, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 0, 10, 100, 100, 0, 2, 1200, 4, allowRandomCreate: true, 40, isSpecial: false, 5, 12, EMaterialProperty.Yin, -1, -1, 25, 8, 30, 5, new List<short> { 171, 2 }, new PoisonsAndLevels(default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short)), new List<PresetInventoryItem>(), 0, 1, 0, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, primaryRecoverAllInjuries: false, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, secondaryRecoverAllInjuries: false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 60, EMaterialFilterType.Invalid, EMaterialFilterHardness.Invalid));
		_dataArray.Add(new MaterialItem(193, 386, 5, 505, 3, 192, "icon_Material_zhuxinfushen", 387, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 0, 10, 750, 750, 0, 4, 3000, 5, allowRandomCreate: true, 30, isSpecial: false, 5, 12, EMaterialProperty.Yin, -1, -1, 100, 8, 100, 20, new List<short> { 171, 2 }, new PoisonsAndLevels(default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short)), new List<PresetInventoryItem>(), 0, 1, 0, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, primaryRecoverAllInjuries: false, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, secondaryRecoverAllInjuries: false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 60, EMaterialFilterType.Invalid, EMaterialFilterHardness.Invalid));
		_dataArray.Add(new MaterialItem(194, 388, 5, 505, 5, 192, "icon_Material_zhuxinfushen", 389, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 0, 10, 2800, 2800, 2, 6, 5400, 7, allowRandomCreate: true, 20, isSpecial: false, 5, 12, EMaterialProperty.Yin, -1, -1, 300, 8, 210, 80, new List<short> { 171, 2 }, new PoisonsAndLevels(default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short)), new List<PresetInventoryItem>(), 0, 1, 0, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, primaryRecoverAllInjuries: false, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, secondaryRecoverAllInjuries: false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 60, EMaterialFilterType.Invalid, EMaterialFilterHardness.Invalid));
		_dataArray.Add(new MaterialItem(195, 390, 5, 505, 7, 192, "icon_Material_motianma", 391, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 0, 10, 7050, 7050, 4, 8, 9000, 8, allowRandomCreate: true, 10, isSpecial: false, 5, 12, EMaterialProperty.Yin, -1, -1, 500, 8, 360, 320, new List<short> { 171, 2 }, new PoisonsAndLevels(default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short)), new List<PresetInventoryItem>(), 0, 1, 4, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, primaryRecoverAllInjuries: false, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, secondaryRecoverAllInjuries: false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 80, 2, 60, EMaterialFilterType.Invalid, EMaterialFilterHardness.Invalid));
		_dataArray.Add(new MaterialItem(196, 392, 5, 505, 1, 196, "icon_Material_mantianxiang", 393, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 0, 10, 100, 100, 0, 2, 1200, 4, allowRandomCreate: true, 40, isSpecial: false, 5, 12, EMaterialProperty.Yin, -1, -1, 25, 8, 30, 5, new List<short> { 172, 2 }, new PoisonsAndLevels(default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short)), new List<PresetInventoryItem>(), 0, 1, 0, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, primaryRecoverAllInjuries: false, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, secondaryRecoverAllInjuries: false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 60, EMaterialFilterType.Invalid, EMaterialFilterHardness.Invalid));
		_dataArray.Add(new MaterialItem(197, 394, 5, 505, 3, 196, "icon_Material_xinghunhua", 395, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 0, 10, 750, 750, 0, 4, 3000, 5, allowRandomCreate: true, 30, isSpecial: false, 5, 12, EMaterialProperty.Yin, -1, -1, 100, 8, 100, 20, new List<short> { 172, 2 }, new PoisonsAndLevels(default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short)), new List<PresetInventoryItem>(), 0, 1, 0, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, primaryRecoverAllInjuries: false, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, secondaryRecoverAllInjuries: false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 60, EMaterialFilterType.Invalid, EMaterialFilterHardness.Invalid));
		_dataArray.Add(new MaterialItem(198, 396, 5, 505, 5, 196, "icon_Material_xinghunhua", 397, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 0, 10, 2800, 2800, 2, 6, 5400, 7, allowRandomCreate: true, 20, isSpecial: false, 5, 12, EMaterialProperty.Yin, -1, -1, 300, 8, 210, 80, new List<short> { 172, 2 }, new PoisonsAndLevels(default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short)), new List<PresetInventoryItem>(), 0, 1, 0, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, primaryRecoverAllInjuries: false, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, secondaryRecoverAllInjuries: false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 60, EMaterialFilterType.Invalid, EMaterialFilterHardness.Invalid));
		_dataArray.Add(new MaterialItem(199, 398, 5, 505, 7, 196, "icon_Material_linglongshanhu", 399, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 0, 10, 7050, 7050, 4, 8, 9000, 8, allowRandomCreate: true, 10, isSpecial: false, 5, 12, EMaterialProperty.Yin, -1, -1, 500, 8, 360, 320, new List<short> { 172, 2 }, new PoisonsAndLevels(default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short)), new List<PresetInventoryItem>(), 0, 1, 2, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, primaryRecoverAllInjuries: false, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, secondaryRecoverAllInjuries: false, 0, 0, 0, 0, 0, 0, 0, 200, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 40, 2, 60, EMaterialFilterType.Invalid, EMaterialFilterHardness.Invalid));
		_dataArray.Add(new MaterialItem(200, 400, 5, 505, 1, 200, "icon_Material_yexianjiang", 401, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 0, 10, 100, 100, 0, 2, 1200, 4, allowRandomCreate: true, 40, isSpecial: false, 5, 12, EMaterialProperty.Yin, -1, -1, 25, 8, 30, 5, new List<short> { 173, 2 }, new PoisonsAndLevels(default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short)), new List<PresetInventoryItem>(), 0, 1, 0, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, primaryRecoverAllInjuries: false, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, secondaryRecoverAllInjuries: false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 60, EMaterialFilterType.Invalid, EMaterialFilterHardness.Invalid));
		_dataArray.Add(new MaterialItem(201, 402, 5, 505, 3, 200, "icon_Material_lurong", 403, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 0, 10, 750, 750, 0, 4, 3000, 5, allowRandomCreate: true, 30, isSpecial: false, 5, 12, EMaterialProperty.Yin, -1, -1, 100, 8, 100, 20, new List<short> { 173, 2 }, new PoisonsAndLevels(default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short)), new List<PresetInventoryItem>(), 0, 1, 0, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, primaryRecoverAllInjuries: false, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, secondaryRecoverAllInjuries: false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 60, EMaterialFilterType.Invalid, EMaterialFilterHardness.Invalid));
		_dataArray.Add(new MaterialItem(202, 404, 5, 505, 5, 200, "icon_Material_lurong", 405, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 0, 10, 2800, 2800, 2, 6, 5400, 7, allowRandomCreate: true, 20, isSpecial: false, 5, 12, EMaterialProperty.Yin, -1, -1, 300, 8, 210, 80, new List<short> { 173, 2 }, new PoisonsAndLevels(default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short)), new List<PresetInventoryItem>(), 0, 1, 0, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, primaryRecoverAllInjuries: false, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, secondaryRecoverAllInjuries: false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 60, EMaterialFilterType.Invalid, EMaterialFilterHardness.Invalid));
		_dataArray.Add(new MaterialItem(203, 406, 5, 505, 7, 200, "icon_Material_hupodoukou", 407, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 0, 10, 7050, 7050, 4, 8, 9000, 8, allowRandomCreate: true, 10, isSpecial: false, 5, 12, EMaterialProperty.Yin, -1, -1, 500, 8, 360, 320, new List<short> { 173, 2 }, new PoisonsAndLevels(default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short)), new List<PresetInventoryItem>(), 0, 1, 2, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, primaryRecoverAllInjuries: false, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, secondaryRecoverAllInjuries: false, 0, 0, 0, 0, 0, 0, 0, 100, 0, 0, 0, 0, 0, 0, 100, 100, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 60, EMaterialFilterType.Invalid, EMaterialFilterHardness.Invalid));
		_dataArray.Add(new MaterialItem(204, 408, 5, 505, 1, 204, "icon_Material_zhuguo", 409, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 0, 10, 100, 100, 0, 2, 1200, 4, allowRandomCreate: true, 40, isSpecial: false, 5, 12, EMaterialProperty.Yin, -1, -1, 25, 8, 30, 5, new List<short> { 174, 2 }, new PoisonsAndLevels(default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short)), new List<PresetInventoryItem>(), 0, 1, 0, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, primaryRecoverAllInjuries: false, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, secondaryRecoverAllInjuries: false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 60, EMaterialFilterType.Invalid, EMaterialFilterHardness.Invalid));
		_dataArray.Add(new MaterialItem(205, 410, 5, 505, 3, 204, "icon_Material_youtaicao", 411, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 0, 10, 750, 750, 0, 4, 3000, 5, allowRandomCreate: true, 30, isSpecial: false, 5, 12, EMaterialProperty.Yin, -1, -1, 100, 8, 100, 20, new List<short> { 174, 2 }, new PoisonsAndLevels(default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short)), new List<PresetInventoryItem>(), 0, 1, 0, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, primaryRecoverAllInjuries: false, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, secondaryRecoverAllInjuries: false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 60, EMaterialFilterType.Invalid, EMaterialFilterHardness.Invalid));
		_dataArray.Add(new MaterialItem(206, 412, 5, 505, 5, 204, "icon_Material_youtaicao", 413, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 0, 10, 2800, 2800, 2, 6, 5400, 7, allowRandomCreate: true, 20, isSpecial: false, 5, 12, EMaterialProperty.Yin, -1, -1, 300, 8, 210, 80, new List<short> { 174, 2 }, new PoisonsAndLevels(default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short)), new List<PresetInventoryItem>(), 0, 1, 0, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, primaryRecoverAllInjuries: false, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, secondaryRecoverAllInjuries: false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 60, EMaterialFilterType.Invalid, EMaterialFilterHardness.Invalid));
		_dataArray.Add(new MaterialItem(207, 414, 5, 505, 7, 204, "icon_Material_shuminghua", 415, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 0, 10, 7050, 7050, 4, 8, 9000, 8, allowRandomCreate: true, 10, isSpecial: false, 5, 12, EMaterialProperty.Yin, -1, -1, 500, 8, 360, 320, new List<short> { 174, 2 }, new PoisonsAndLevels(default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short)), new List<PresetInventoryItem>(), 0, 1, 2, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, primaryRecoverAllInjuries: false, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, secondaryRecoverAllInjuries: false, 0, 0, 100, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 100, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 60, EMaterialFilterType.Invalid, EMaterialFilterHardness.Invalid));
		_dataArray.Add(new MaterialItem(208, 416, 5, 505, 1, 208, "icon_Material_qiandan", 417, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 0, 10, 100, 100, 0, 2, 1200, 4, allowRandomCreate: true, 40, isSpecial: false, 5, 12, EMaterialProperty.Yang, -1, -1, 25, 8, 30, 5, new List<short> { 175, 2 }, new PoisonsAndLevels(default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short)), new List<PresetInventoryItem>(), 0, 1, 0, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, primaryRecoverAllInjuries: false, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, secondaryRecoverAllInjuries: false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 60, EMaterialFilterType.Invalid, EMaterialFilterHardness.Invalid));
		_dataArray.Add(new MaterialItem(209, 418, 5, 505, 3, 208, "icon_Material_baichongguijian", 419, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 0, 10, 750, 750, 0, 4, 3000, 5, allowRandomCreate: true, 30, isSpecial: false, 5, 12, EMaterialProperty.Yang, -1, -1, 100, 8, 100, 20, new List<short> { 175, 2 }, new PoisonsAndLevels(default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short)), new List<PresetInventoryItem>(), 0, 1, 0, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, primaryRecoverAllInjuries: false, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, secondaryRecoverAllInjuries: false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 60, EMaterialFilterType.Invalid, EMaterialFilterHardness.Invalid));
		_dataArray.Add(new MaterialItem(210, 420, 5, 505, 5, 208, "icon_Material_baichongguijian", 421, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 0, 10, 2800, 2800, 2, 6, 5400, 7, allowRandomCreate: true, 20, isSpecial: false, 5, 12, EMaterialProperty.Yang, -1, -1, 300, 8, 210, 80, new List<short> { 175, 2 }, new PoisonsAndLevels(default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short)), new List<PresetInventoryItem>(), 0, 1, 0, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, primaryRecoverAllInjuries: false, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, secondaryRecoverAllInjuries: false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 60, EMaterialFilterType.Invalid, EMaterialFilterHardness.Invalid));
		_dataArray.Add(new MaterialItem(211, 422, 5, 505, 7, 208, "icon_Material_wubeiyinchan", 423, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 0, 10, 7050, 7050, 4, 8, 9000, 8, allowRandomCreate: true, 10, isSpecial: false, 5, 12, EMaterialProperty.Yang, -1, -1, 500, 8, 360, 320, new List<short> { 175, 2 }, new PoisonsAndLevels(default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short)), new List<PresetInventoryItem>(), 0, 1, 2, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, primaryRecoverAllInjuries: false, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, secondaryRecoverAllInjuries: false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 50, 50, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 80, 0, 2, 60, EMaterialFilterType.Invalid, EMaterialFilterHardness.Invalid));
		_dataArray.Add(new MaterialItem(212, 424, 5, 505, 1, 212, "icon_Material_zuifurong", 425, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 0, 10, 100, 100, 0, 2, 1200, 4, allowRandomCreate: true, 40, isSpecial: false, 5, 12, EMaterialProperty.Yang, -1, -1, 25, 8, 30, 5, new List<short> { 176, 2 }, new PoisonsAndLevels(default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short)), new List<PresetInventoryItem>(), 0, 1, 0, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, primaryRecoverAllInjuries: false, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, secondaryRecoverAllInjuries: false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 60, EMaterialFilterType.Invalid, EMaterialFilterHardness.Invalid));
		_dataArray.Add(new MaterialItem(213, 426, 5, 505, 3, 212, "icon_Material_tianzhufozuo", 427, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 0, 10, 750, 750, 0, 4, 3000, 5, allowRandomCreate: true, 30, isSpecial: false, 5, 12, EMaterialProperty.Yang, -1, -1, 100, 8, 100, 20, new List<short> { 176, 2 }, new PoisonsAndLevels(default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short)), new List<PresetInventoryItem>(), 0, 1, 0, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, primaryRecoverAllInjuries: false, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, secondaryRecoverAllInjuries: false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 60, EMaterialFilterType.Invalid, EMaterialFilterHardness.Invalid));
		_dataArray.Add(new MaterialItem(214, 428, 5, 505, 5, 212, "icon_Material_tianzhufozuo", 429, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 0, 10, 2800, 2800, 2, 6, 5400, 7, allowRandomCreate: true, 20, isSpecial: false, 5, 12, EMaterialProperty.Yang, -1, -1, 300, 8, 210, 80, new List<short> { 176, 2 }, new PoisonsAndLevels(default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short)), new List<PresetInventoryItem>(), 0, 1, 0, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, primaryRecoverAllInjuries: false, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, secondaryRecoverAllInjuries: false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 60, EMaterialFilterType.Invalid, EMaterialFilterHardness.Invalid));
		_dataArray.Add(new MaterialItem(215, 430, 5, 505, 7, 212, "icon_Material_jincan", 431, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 0, 10, 7050, 7050, 4, 8, 9000, 8, allowRandomCreate: true, 10, isSpecial: false, 5, 12, EMaterialProperty.Yang, -1, -1, 500, 8, 360, 320, new List<short> { 176, 2 }, new PoisonsAndLevels(default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short)), new List<PresetInventoryItem>(), 0, 1, 2, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, primaryRecoverAllInjuries: false, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, secondaryRecoverAllInjuries: false, 0, 0, 0, 0, 0, 0, 0, 0, 200, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 40, 0, 2, 60, EMaterialFilterType.Invalid, EMaterialFilterHardness.Invalid));
		_dataArray.Add(new MaterialItem(216, 432, 5, 505, 1, 216, "icon_Material_suiyincishi", 433, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 0, 10, 100, 100, 0, 2, 1200, 4, allowRandomCreate: true, 40, isSpecial: false, 5, 12, EMaterialProperty.Yin, -1, -1, 25, 8, 30, 5, new List<short> { 177, 2 }, new PoisonsAndLevels(default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short)), new List<PresetInventoryItem>(), 0, 1, 0, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, primaryRecoverAllInjuries: false, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, secondaryRecoverAllInjuries: false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 60, EMaterialFilterType.Invalid, EMaterialFilterHardness.Invalid));
		_dataArray.Add(new MaterialItem(217, 434, 5, 505, 3, 216, "icon_Material_kongqingshi", 435, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 0, 10, 750, 750, 0, 4, 3000, 5, allowRandomCreate: true, 30, isSpecial: false, 5, 12, EMaterialProperty.Yin, -1, -1, 100, 8, 100, 20, new List<short> { 177, 2 }, new PoisonsAndLevels(default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short)), new List<PresetInventoryItem>(), 0, 1, 0, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, primaryRecoverAllInjuries: false, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, secondaryRecoverAllInjuries: false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 60, EMaterialFilterType.Invalid, EMaterialFilterHardness.Invalid));
		_dataArray.Add(new MaterialItem(218, 436, 5, 505, 5, 216, "icon_Material_kongqingshi", 437, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 0, 10, 2800, 2800, 2, 6, 5400, 7, allowRandomCreate: true, 20, isSpecial: false, 5, 12, EMaterialProperty.Yin, -1, -1, 300, 8, 210, 80, new List<short> { 177, 2 }, new PoisonsAndLevels(default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short)), new List<PresetInventoryItem>(), 0, 1, 0, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, primaryRecoverAllInjuries: false, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, secondaryRecoverAllInjuries: false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 60, EMaterialFilterType.Invalid, EMaterialFilterHardness.Invalid));
		_dataArray.Add(new MaterialItem(219, 438, 5, 505, 7, 216, "icon_Material_chiyuhanchan", 439, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 0, 10, 7050, 7050, 4, 8, 9000, 8, allowRandomCreate: true, 10, isSpecial: false, 5, 12, EMaterialProperty.Yin, -1, -1, 500, 8, 360, 320, new List<short> { 177, 2 }, new PoisonsAndLevels(default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short)), new List<PresetInventoryItem>(), 0, 1, 2, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, primaryRecoverAllInjuries: false, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, secondaryRecoverAllInjuries: false, 0, 0, 200, 0, 0, 0, 0, 0, 100, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 60, EMaterialFilterType.Invalid, EMaterialFilterHardness.Invalid));
		_dataArray.Add(new MaterialItem(220, 440, 5, 505, 1, 220, "icon_Material_jiujiechangpu", 441, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 0, 10, 100, 100, 0, 2, 1200, 4, allowRandomCreate: true, 40, isSpecial: false, 5, 12, EMaterialProperty.Yang, -1, -1, 25, 8, 30, 5, new List<short> { 178, 2 }, new PoisonsAndLevels(default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short)), new List<PresetInventoryItem>(), 0, 1, 0, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, primaryRecoverAllInjuries: false, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, secondaryRecoverAllInjuries: false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 60, EMaterialFilterType.Invalid, EMaterialFilterHardness.Invalid));
		_dataArray.Add(new MaterialItem(221, 442, 5, 505, 3, 220, "icon_Material_yinxianchongcao", 443, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 0, 10, 750, 750, 0, 4, 3000, 5, allowRandomCreate: true, 30, isSpecial: false, 5, 12, EMaterialProperty.Yang, -1, -1, 100, 8, 100, 20, new List<short> { 178, 2 }, new PoisonsAndLevels(default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short)), new List<PresetInventoryItem>(), 0, 1, 0, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, primaryRecoverAllInjuries: false, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, secondaryRecoverAllInjuries: false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 60, EMaterialFilterType.Invalid, EMaterialFilterHardness.Invalid));
		_dataArray.Add(new MaterialItem(222, 444, 5, 505, 5, 220, "icon_Material_yinxianchongcao", 445, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 0, 20, 2800, 2800, 2, 6, 5400, 7, allowRandomCreate: true, 20, isSpecial: false, 5, 12, EMaterialProperty.Yang, -1, -1, 300, 8, 210, 80, new List<short> { 178, 2 }, new PoisonsAndLevels(default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short)), new List<PresetInventoryItem>(), 0, 1, 0, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, primaryRecoverAllInjuries: false, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, secondaryRecoverAllInjuries: false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 60, EMaterialFilterType.Invalid, EMaterialFilterHardness.Invalid));
		_dataArray.Add(new MaterialItem(223, 446, 5, 505, 7, 220, "icon_Material_shelizi", 447, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 0, 10, 7050, 7050, 4, 8, 9000, 8, allowRandomCreate: true, 10, isSpecial: false, 5, 12, EMaterialProperty.Yang, -1, -1, 500, 8, 360, 320, new List<short> { 178, 2 }, new PoisonsAndLevels(default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short)), new List<PresetInventoryItem>(), 0, 1, 2, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, primaryRecoverAllInjuries: false, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, secondaryRecoverAllInjuries: false, 0, 0, 0, 0, 100, 200, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 60, EMaterialFilterType.Invalid, EMaterialFilterHardness.Invalid));
		_dataArray.Add(new MaterialItem(224, 448, 5, 505, 1, 224, "icon_Material_xihuang", 449, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 0, 10, 100, 100, 0, 2, 1200, 4, allowRandomCreate: true, 40, isSpecial: false, 5, 12, EMaterialProperty.Yin, -1, -1, 25, 8, 30, 5, new List<short> { 179, 2 }, new PoisonsAndLevels(default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short)), new List<PresetInventoryItem>(), 0, 1, 0, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, primaryRecoverAllInjuries: false, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, secondaryRecoverAllInjuries: false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 60, EMaterialFilterType.Invalid, EMaterialFilterHardness.Invalid));
		_dataArray.Add(new MaterialItem(225, 450, 5, 505, 3, 224, "icon_Material_heixiongdan", 451, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 0, 10, 750, 750, 0, 4, 3000, 5, allowRandomCreate: true, 30, isSpecial: false, 5, 12, EMaterialProperty.Yin, -1, -1, 100, 8, 100, 20, new List<short> { 179, 2 }, new PoisonsAndLevels(default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short)), new List<PresetInventoryItem>(), 0, 1, 0, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, primaryRecoverAllInjuries: false, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, secondaryRecoverAllInjuries: false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 60, EMaterialFilterType.Invalid, EMaterialFilterHardness.Invalid));
		_dataArray.Add(new MaterialItem(226, 452, 5, 505, 5, 224, "icon_Material_heixiongdan", 453, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 0, 10, 2800, 2800, 2, 6, 5400, 7, allowRandomCreate: true, 20, isSpecial: false, 5, 12, EMaterialProperty.Yin, -1, -1, 300, 8, 210, 80, new List<short> { 179, 2 }, new PoisonsAndLevels(default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short)), new List<PresetInventoryItem>(), 0, 1, 0, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, primaryRecoverAllInjuries: false, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, secondaryRecoverAllInjuries: false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 60, EMaterialFilterType.Invalid, EMaterialFilterHardness.Invalid));
		_dataArray.Add(new MaterialItem(227, 454, 5, 505, 7, 224, "icon_Material_tianshetui", 455, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 0, 10, 7050, 7050, 4, 8, 9000, 8, allowRandomCreate: true, 10, isSpecial: false, 5, 12, EMaterialProperty.Yin, -1, -1, 500, 8, 360, 320, new List<short> { 179, 2 }, new PoisonsAndLevels(default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short)), new List<PresetInventoryItem>(), 0, 1, 2, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, primaryRecoverAllInjuries: false, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, secondaryRecoverAllInjuries: false, 0, 0, 0, 0, 0, 100, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 80, 0, 0, 0, 0, 0, 2, 60, EMaterialFilterType.Invalid, EMaterialFilterHardness.Invalid));
		_dataArray.Add(new MaterialItem(228, 456, 5, 505, 1, 228, "icon_Material_shexiang", 457, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 0, 10, 100, 100, 0, 2, 1200, 4, allowRandomCreate: true, 40, isSpecial: false, 5, 12, EMaterialProperty.Yin, -1, -1, 25, 8, 30, 5, new List<short> { 180, 2 }, new PoisonsAndLevels(default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short)), new List<PresetInventoryItem>(), 0, 1, 0, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, primaryRecoverAllInjuries: false, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, secondaryRecoverAllInjuries: false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 60, EMaterialFilterType.Invalid, EMaterialFilterHardness.Invalid));
		_dataArray.Add(new MaterialItem(229, 458, 5, 505, 3, 228, "icon_Material_jinxiangfu", 459, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 0, 10, 750, 750, 0, 4, 3000, 5, allowRandomCreate: true, 30, isSpecial: false, 5, 12, EMaterialProperty.Yin, -1, -1, 100, 8, 100, 20, new List<short> { 180, 2 }, new PoisonsAndLevels(default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short)), new List<PresetInventoryItem>(), 0, 1, 0, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, primaryRecoverAllInjuries: false, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, secondaryRecoverAllInjuries: false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 60, EMaterialFilterType.Invalid, EMaterialFilterHardness.Invalid));
		_dataArray.Add(new MaterialItem(230, 460, 5, 505, 5, 228, "icon_Material_jinxiangfu", 461, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 0, 10, 2800, 2800, 2, 6, 5400, 7, allowRandomCreate: true, 20, isSpecial: false, 5, 12, EMaterialProperty.Yin, -1, -1, 300, 8, 210, 80, new List<short> { 180, 2 }, new PoisonsAndLevels(default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short)), new List<PresetInventoryItem>(), 0, 1, 0, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, primaryRecoverAllInjuries: false, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, secondaryRecoverAllInjuries: false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 60, EMaterialFilterType.Invalid, EMaterialFilterHardness.Invalid));
		_dataArray.Add(new MaterialItem(231, 462, 5, 505, 7, 228, "icon_Material_jinmupantao", 463, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 0, 10, 7050, 7050, 4, 8, 9000, 8, allowRandomCreate: true, 10, isSpecial: false, 5, 12, EMaterialProperty.Yin, -1, -1, 500, 8, 360, 320, new List<short> { 180, 2 }, new PoisonsAndLevels(default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short)), new List<PresetInventoryItem>(), 0, 1, 2, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, primaryRecoverAllInjuries: false, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, secondaryRecoverAllInjuries: false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 100, 0, 0, 0, 0, 40, 0, 0, 0, 0, 0, 2, 60, EMaterialFilterType.Invalid, EMaterialFilterHardness.Invalid));
		_dataArray.Add(new MaterialItem(232, 464, 5, 505, 1, 232, "icon_Material_jiguichong", 465, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 0, 10, 100, 100, 0, 2, 1200, 4, allowRandomCreate: true, 40, isSpecial: false, 5, 12, EMaterialProperty.Yang, -1, -1, 25, 8, 30, 5, new List<short> { 181, 2 }, new PoisonsAndLevels(default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short)), new List<PresetInventoryItem>(), 0, 1, 0, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, primaryRecoverAllInjuries: false, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, secondaryRecoverAllInjuries: false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 60, EMaterialFilterType.Invalid, EMaterialFilterHardness.Invalid));
		_dataArray.Add(new MaterialItem(233, 466, 5, 505, 3, 232, "icon_Material_wutongxieshe", 467, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 0, 10, 750, 750, 0, 4, 3000, 5, allowRandomCreate: true, 30, isSpecial: false, 5, 12, EMaterialProperty.Yang, -1, -1, 100, 8, 100, 20, new List<short> { 181, 2 }, new PoisonsAndLevels(default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short)), new List<PresetInventoryItem>(), 0, 1, 0, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, primaryRecoverAllInjuries: false, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, secondaryRecoverAllInjuries: false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 60, EMaterialFilterType.Invalid, EMaterialFilterHardness.Invalid));
		_dataArray.Add(new MaterialItem(234, 468, 5, 505, 5, 232, "icon_Material_wutongxieshe", 469, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 0, 10, 2800, 2800, 2, 6, 5400, 7, allowRandomCreate: true, 20, isSpecial: false, 5, 12, EMaterialProperty.Yang, -1, -1, 300, 8, 210, 80, new List<short> { 181, 2 }, new PoisonsAndLevels(default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short)), new List<PresetInventoryItem>(), 0, 1, 0, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, primaryRecoverAllInjuries: false, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, secondaryRecoverAllInjuries: false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 60, EMaterialFilterType.Invalid, EMaterialFilterHardness.Invalid));
		_dataArray.Add(new MaterialItem(235, 470, 5, 505, 7, 232, "icon_Material_bamangxuandan", 471, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 0, 30, 7050, 7050, 4, 8, 9000, 8, allowRandomCreate: true, 10, isSpecial: false, 5, 12, EMaterialProperty.Yang, -1, -1, 500, 8, 360, 320, new List<short> { 181, 2 }, new PoisonsAndLevels(default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short)), new List<PresetInventoryItem>(), 0, 1, 2, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, primaryRecoverAllInjuries: false, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, secondaryRecoverAllInjuries: false, 0, 0, 0, 0, 200, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 50, 50, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 60, EMaterialFilterType.Invalid, EMaterialFilterHardness.Invalid));
		_dataArray.Add(new MaterialItem(236, 472, 5, 506, 1, 236, "icon_Material_zhenyu", 473, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 0, 10, 100, 200, 0, 2, 600, 3, allowRandomCreate: true, 40, isSpecial: false, 5, 12, EMaterialProperty.Yang, -1, -1, 25, 9, 30, 5, new List<short> { 152, 3 }, new PoisonsAndLevels(5, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0), new List<PresetInventoryItem>(), 0, 1, 0, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, primaryRecoverAllInjuries: false, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, secondaryRecoverAllInjuries: false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 60, EMaterialFilterType.Poison, EMaterialFilterHardness.Invalid));
		_dataArray.Add(new MaterialItem(237, 474, 5, 506, 2, 236, "icon_Material_zhenyu", 475, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 0, 10, 300, 600, 0, 3, 1200, 4, allowRandomCreate: true, 35, isSpecial: false, 5, 12, EMaterialProperty.Yang, -1, -1, 50, 9, 60, 10, new List<short> { 152, 3 }, new PoisonsAndLevels(10, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0), new List<PresetInventoryItem>(), 0, 1, 0, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, primaryRecoverAllInjuries: false, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, secondaryRecoverAllInjuries: false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 60, EMaterialFilterType.Poison, EMaterialFilterHardness.Invalid));
		_dataArray.Add(new MaterialItem(238, 476, 5, 506, 3, 236, "icon_Material_qianjicao", 477, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 0, 10, 750, 1500, 0, 4, 1800, 5, allowRandomCreate: true, 30, isSpecial: false, 5, 12, EMaterialProperty.Yang, -1, -1, 100, 9, 100, 20, new List<short> { 152, 3 }, new PoisonsAndLevels(15, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0), new List<PresetInventoryItem>(), 0, 1, 0, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, primaryRecoverAllInjuries: false, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, secondaryRecoverAllInjuries: false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 60, EMaterialFilterType.Poison, EMaterialFilterHardness.Invalid));
		_dataArray.Add(new MaterialItem(239, 478, 5, 506, 4, 236, "icon_Material_qianjicao", 479, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 0, 10, 1550, 3100, 1, 5, 3000, 6, allowRandomCreate: true, 25, isSpecial: false, 5, 12, EMaterialProperty.Yang, -1, -1, 200, 9, 150, 40, new List<short> { 152, 3 }, new PoisonsAndLevels(15, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0), new List<PresetInventoryItem>(), 0, 1, 0, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, primaryRecoverAllInjuries: false, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, secondaryRecoverAllInjuries: false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 60, EMaterialFilterType.Poison, EMaterialFilterHardness.Invalid));
	}

	private void CreateItems4()
	{
		_dataArray.Add(new MaterialItem(240, 480, 5, 506, 5, 236, "icon_Material_qianjicao", 481, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 0, 10, 2800, 5600, 2, 6, 4200, 7, allowRandomCreate: true, 20, isSpecial: false, 5, 12, EMaterialProperty.Yang, -1, -1, 300, 9, 210, 80, new List<short> { 152, 3 }, new PoisonsAndLevels(20, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0), new List<PresetInventoryItem>(), 0, 1, 0, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, primaryRecoverAllInjuries: false, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, secondaryRecoverAllInjuries: false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 60, EMaterialFilterType.Poison, EMaterialFilterHardness.Invalid));
		_dataArray.Add(new MaterialItem(241, 482, 5, 506, 6, 236, "icon_Material_jinshe", 483, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 0, 10, 4600, 9200, 3, 7, 5400, 7, allowRandomCreate: true, 15, isSpecial: false, 5, 12, EMaterialProperty.Yang, -1, -1, 400, 9, 280, 160, new List<short> { 152, 3 }, new PoisonsAndLevels(25, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0), new List<PresetInventoryItem>(), 0, 1, 0, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, primaryRecoverAllInjuries: false, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, secondaryRecoverAllInjuries: false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 60, EMaterialFilterType.Poison, EMaterialFilterHardness.Invalid));
		_dataArray.Add(new MaterialItem(242, 484, 5, 506, 7, 236, "icon_Material_duanchangcao", 485, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 0, 10, 7050, 14100, 4, 8, 7200, 8, allowRandomCreate: true, 10, isSpecial: false, 5, 12, EMaterialProperty.Yang, -1, -1, 500, 9, 360, 320, new List<short> { 152, 3 }, new PoisonsAndLevels(25, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0), new List<PresetInventoryItem>(), 0, 1, 0, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, primaryRecoverAllInjuries: false, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, secondaryRecoverAllInjuries: false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 60, EMaterialFilterType.Poison, EMaterialFilterHardness.Invalid));
		_dataArray.Add(new MaterialItem(243, 486, 5, 506, 1, 243, "icon_Material_caowutou", 487, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 0, 10, 100, 200, 0, 2, 600, 3, allowRandomCreate: true, 40, isSpecial: false, 5, 12, EMaterialProperty.Yin, -1, -1, 25, 9, 30, 5, new List<short> { 153, 3 }, new PoisonsAndLevels(0, 0, 5, 1, 0, 0, 0, 0, 0, 0, 0, 0), new List<PresetInventoryItem>(), 0, 1, 0, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, primaryRecoverAllInjuries: false, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, secondaryRecoverAllInjuries: false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 60, EMaterialFilterType.Poison, EMaterialFilterHardness.Invalid));
		_dataArray.Add(new MaterialItem(244, 488, 5, 506, 2, 243, "icon_Material_caowutou", 489, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 0, 10, 300, 600, 0, 3, 1200, 4, allowRandomCreate: true, 35, isSpecial: false, 5, 12, EMaterialProperty.Yin, -1, -1, 50, 9, 60, 10, new List<short> { 153, 3 }, new PoisonsAndLevels(0, 0, 10, 1, 0, 0, 0, 0, 0, 0, 0, 0), new List<PresetInventoryItem>(), 0, 1, 0, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, primaryRecoverAllInjuries: false, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, secondaryRecoverAllInjuries: false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 60, EMaterialFilterType.Poison, EMaterialFilterHardness.Invalid));
		_dataArray.Add(new MaterialItem(245, 490, 5, 506, 3, 243, "icon_Material_ziwufeng", 491, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 0, 10, 750, 1500, 0, 4, 1800, 5, allowRandomCreate: true, 30, isSpecial: false, 5, 12, EMaterialProperty.Yin, -1, -1, 100, 9, 100, 20, new List<short> { 153, 3 }, new PoisonsAndLevels(0, 0, 15, 1, 0, 0, 0, 0, 0, 0, 0, 0), new List<PresetInventoryItem>(), 0, 1, 0, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, primaryRecoverAllInjuries: false, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, secondaryRecoverAllInjuries: false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 60, EMaterialFilterType.Poison, EMaterialFilterHardness.Invalid));
		_dataArray.Add(new MaterialItem(246, 492, 5, 506, 4, 243, "icon_Material_ziwufeng", 493, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 0, 10, 1550, 3100, 1, 5, 3000, 6, allowRandomCreate: true, 25, isSpecial: false, 5, 12, EMaterialProperty.Yin, -1, -1, 200, 9, 150, 40, new List<short> { 153, 3 }, new PoisonsAndLevels(0, 0, 15, 2, 0, 0, 0, 0, 0, 0, 0, 0), new List<PresetInventoryItem>(), 0, 1, 0, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, primaryRecoverAllInjuries: false, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, secondaryRecoverAllInjuries: false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 60, EMaterialFilterType.Poison, EMaterialFilterHardness.Invalid));
		_dataArray.Add(new MaterialItem(247, 494, 5, 506, 5, 243, "icon_Material_ziwufeng", 495, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 0, 10, 2800, 5600, 2, 6, 4200, 7, allowRandomCreate: true, 20, isSpecial: false, 5, 12, EMaterialProperty.Yin, -1, -1, 300, 9, 210, 80, new List<short> { 153, 3 }, new PoisonsAndLevels(0, 0, 20, 2, 0, 0, 0, 0, 0, 0, 0, 0), new List<PresetInventoryItem>(), 0, 1, 0, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, primaryRecoverAllInjuries: false, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, secondaryRecoverAllInjuries: false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 60, EMaterialFilterType.Poison, EMaterialFilterHardness.Invalid));
		_dataArray.Add(new MaterialItem(248, 496, 5, 506, 6, 243, "icon_Material_qicaiyushaniang", 497, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 0, 10, 4600, 9200, 3, 7, 5400, 7, allowRandomCreate: true, 15, isSpecial: false, 5, 12, EMaterialProperty.Yin, -1, -1, 400, 9, 280, 160, new List<short> { 153, 3 }, new PoisonsAndLevels(0, 0, 25, 2, 0, 0, 0, 0, 0, 0, 0, 0), new List<PresetInventoryItem>(), 0, 1, 0, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, primaryRecoverAllInjuries: false, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, secondaryRecoverAllInjuries: false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 60, EMaterialFilterType.Poison, EMaterialFilterHardness.Invalid));
		_dataArray.Add(new MaterialItem(249, 498, 5, 506, 7, 243, "icon_Material_xieqiaohua", 499, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 0, 10, 7050, 14100, 4, 8, 7200, 8, allowRandomCreate: true, 10, isSpecial: false, 5, 12, EMaterialProperty.Yin, -1, -1, 500, 9, 360, 320, new List<short> { 153, 3 }, new PoisonsAndLevels(0, 0, 25, 3, 0, 0, 0, 0, 0, 0, 0, 0), new List<PresetInventoryItem>(), 0, 1, 0, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, primaryRecoverAllInjuries: false, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, secondaryRecoverAllInjuries: false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 60, EMaterialFilterType.Poison, EMaterialFilterHardness.Invalid));
		_dataArray.Add(new MaterialItem(250, 500, 5, 506, 1, 250, "icon_Material_lieshuangcao", 501, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 0, 10, 100, 200, 0, 2, 600, 3, allowRandomCreate: true, 40, isSpecial: false, 5, 12, EMaterialProperty.Yin, -1, -1, 25, 9, 30, 5, new List<short> { 154, 3 }, new PoisonsAndLevels(0, 0, 0, 0, 5, 1, 0, 0, 0, 0, 0, 0), new List<PresetInventoryItem>(), 0, 1, 0, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, primaryRecoverAllInjuries: false, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, secondaryRecoverAllInjuries: false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 60, EMaterialFilterType.Poison, EMaterialFilterHardness.Invalid));
		_dataArray.Add(new MaterialItem(251, 502, 5, 506, 2, 250, "icon_Material_lieshuangcao", 503, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 0, 10, 300, 600, 0, 3, 1200, 4, allowRandomCreate: true, 35, isSpecial: false, 5, 12, EMaterialProperty.Yin, -1, -1, 50, 9, 60, 10, new List<short> { 154, 3 }, new PoisonsAndLevels(0, 0, 0, 0, 10, 1, 0, 0, 0, 0, 0, 0), new List<PresetInventoryItem>(), 0, 1, 0, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, primaryRecoverAllInjuries: false, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, secondaryRecoverAllInjuries: false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 60, EMaterialFilterType.Poison, EMaterialFilterHardness.Invalid));
		_dataArray.Add(new MaterialItem(252, 504, 5, 506, 3, 250, "icon_Material_xuanyindan", 505, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 0, 10, 750, 1500, 0, 4, 1800, 5, allowRandomCreate: true, 30, isSpecial: false, 5, 12, EMaterialProperty.Yin, -1, -1, 100, 9, 100, 20, new List<short> { 154, 3 }, new PoisonsAndLevels(0, 0, 0, 0, 15, 1, 0, 0, 0, 0, 0, 0), new List<PresetInventoryItem>(), 0, 1, 0, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, primaryRecoverAllInjuries: false, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, secondaryRecoverAllInjuries: false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 60, EMaterialFilterType.Poison, EMaterialFilterHardness.Invalid));
		_dataArray.Add(new MaterialItem(253, 506, 5, 506, 4, 250, "icon_Material_xuanyindan", 507, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 0, 10, 1550, 3100, 1, 5, 3000, 6, allowRandomCreate: true, 25, isSpecial: false, 5, 12, EMaterialProperty.Yin, -1, -1, 200, 9, 150, 40, new List<short> { 154, 3 }, new PoisonsAndLevels(0, 0, 0, 0, 15, 2, 0, 0, 0, 0, 0, 0), new List<PresetInventoryItem>(), 0, 1, 0, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, primaryRecoverAllInjuries: false, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, secondaryRecoverAllInjuries: false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 60, EMaterialFilterType.Poison, EMaterialFilterHardness.Invalid));
		_dataArray.Add(new MaterialItem(254, 508, 5, 506, 5, 250, "icon_Material_xuanyindan", 509, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 0, 10, 2800, 5600, 2, 6, 4200, 7, allowRandomCreate: true, 20, isSpecial: false, 5, 12, EMaterialProperty.Yin, -1, -1, 300, 9, 210, 80, new List<short> { 154, 3 }, new PoisonsAndLevels(0, 0, 0, 0, 20, 2, 0, 0, 0, 0, 0, 0), new List<PresetInventoryItem>(), 0, 1, 0, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, primaryRecoverAllInjuries: false, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, secondaryRecoverAllInjuries: false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 60, EMaterialFilterType.Poison, EMaterialFilterHardness.Invalid));
		_dataArray.Add(new MaterialItem(255, 510, 5, 506, 6, 250, "icon_Material_qingjiaodan", 511, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 0, 10, 4600, 9200, 3, 7, 5400, 7, allowRandomCreate: true, 15, isSpecial: false, 5, 12, EMaterialProperty.Yin, -1, -1, 400, 9, 280, 160, new List<short> { 154, 3 }, new PoisonsAndLevels(0, 0, 0, 0, 25, 2, 0, 0, 0, 0, 0, 0), new List<PresetInventoryItem>(), 0, 1, 0, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, primaryRecoverAllInjuries: false, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, secondaryRecoverAllInjuries: false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 60, EMaterialFilterType.Poison, EMaterialFilterHardness.Invalid));
		_dataArray.Add(new MaterialItem(256, 512, 5, 506, 7, 250, "icon_Material_qiannianbingcan", 513, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 0, 10, 7050, 14100, 4, 8, 7200, 8, allowRandomCreate: true, 10, isSpecial: false, 5, 12, EMaterialProperty.Yin, -1, -1, 500, 9, 360, 320, new List<short> { 154, 3 }, new PoisonsAndLevels(0, 0, 0, 0, 25, 3, 0, 0, 0, 0, 0, 0), new List<PresetInventoryItem>(), 0, 1, 0, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, primaryRecoverAllInjuries: false, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, secondaryRecoverAllInjuries: false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 60, EMaterialFilterType.Poison, EMaterialFilterHardness.Invalid));
		_dataArray.Add(new MaterialItem(257, 514, 5, 506, 1, 257, "icon_Material_gongxinshi", 515, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 0, 10, 100, 200, 0, 2, 600, 3, allowRandomCreate: true, 40, isSpecial: false, 5, 12, EMaterialProperty.Yang, -1, -1, 25, 9, 30, 5, new List<short> { 155, 3 }, new PoisonsAndLevels(0, 0, 0, 0, 0, 0, 5, 1, 0, 0, 0, 0), new List<PresetInventoryItem>(), 0, 1, 0, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, primaryRecoverAllInjuries: false, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, secondaryRecoverAllInjuries: false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 60, EMaterialFilterType.Poison, EMaterialFilterHardness.Invalid));
		_dataArray.Add(new MaterialItem(258, 516, 5, 506, 2, 257, "icon_Material_gongxinshi", 517, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 0, 10, 300, 600, 0, 3, 1200, 4, allowRandomCreate: true, 35, isSpecial: false, 5, 12, EMaterialProperty.Yang, -1, -1, 50, 9, 60, 10, new List<short> { 155, 3 }, new PoisonsAndLevels(0, 0, 0, 0, 0, 0, 10, 1, 0, 0, 0, 0), new List<PresetInventoryItem>(), 0, 1, 0, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, primaryRecoverAllInjuries: false, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, secondaryRecoverAllInjuries: false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 60, EMaterialFilterType.Poison, EMaterialFilterHardness.Invalid));
		_dataArray.Add(new MaterialItem(259, 518, 5, 506, 3, 257, "icon_Material_yipinhong", 519, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 0, 10, 750, 1500, 0, 4, 1800, 5, allowRandomCreate: true, 30, isSpecial: false, 5, 12, EMaterialProperty.Yang, -1, -1, 100, 9, 100, 20, new List<short> { 155, 3 }, new PoisonsAndLevels(0, 0, 0, 0, 0, 0, 15, 1, 0, 0, 0, 0), new List<PresetInventoryItem>(), 0, 1, 0, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, primaryRecoverAllInjuries: false, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, secondaryRecoverAllInjuries: false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 60, EMaterialFilterType.Poison, EMaterialFilterHardness.Invalid));
		_dataArray.Add(new MaterialItem(260, 520, 5, 506, 4, 257, "icon_Material_yipinhong", 521, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 0, 10, 1550, 3100, 1, 5, 3000, 6, allowRandomCreate: true, 25, isSpecial: false, 5, 12, EMaterialProperty.Yang, -1, -1, 200, 9, 150, 40, new List<short> { 155, 3 }, new PoisonsAndLevels(0, 0, 0, 0, 0, 0, 15, 2, 0, 0, 0, 0), new List<PresetInventoryItem>(), 0, 1, 0, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, primaryRecoverAllInjuries: false, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, secondaryRecoverAllInjuries: false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 60, EMaterialFilterType.Poison, EMaterialFilterHardness.Invalid));
		_dataArray.Add(new MaterialItem(261, 522, 5, 506, 5, 257, "icon_Material_yipinhong", 523, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 0, 10, 2800, 5600, 2, 6, 4200, 7, allowRandomCreate: true, 20, isSpecial: false, 5, 12, EMaterialProperty.Yang, -1, -1, 300, 9, 210, 80, new List<short> { 155, 3 }, new PoisonsAndLevels(0, 0, 0, 0, 0, 0, 20, 2, 0, 0, 0, 0), new List<PresetInventoryItem>(), 0, 1, 0, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, primaryRecoverAllInjuries: false, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, secondaryRecoverAllInjuries: false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 60, EMaterialFilterType.Poison, EMaterialFilterHardness.Invalid));
		_dataArray.Add(new MaterialItem(262, 524, 5, 506, 6, 257, "icon_Material_fenghuangmu", 525, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 0, 20, 4600, 9200, 3, 7, 5400, 7, allowRandomCreate: true, 15, isSpecial: false, 5, 12, EMaterialProperty.Yang, -1, -1, 400, 9, 280, 160, new List<short> { 155, 3 }, new PoisonsAndLevels(0, 0, 0, 0, 0, 0, 25, 2, 0, 0, 0, 0), new List<PresetInventoryItem>(), 0, 1, 0, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, primaryRecoverAllInjuries: false, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, secondaryRecoverAllInjuries: false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 60, EMaterialFilterType.Poison, EMaterialFilterHardness.Invalid));
		_dataArray.Add(new MaterialItem(263, 526, 5, 506, 7, 257, "icon_Material_xiechan", 527, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 0, 10, 7050, 14100, 4, 8, 7200, 8, allowRandomCreate: true, 10, isSpecial: false, 5, 12, EMaterialProperty.Yang, -1, -1, 500, 9, 360, 320, new List<short> { 155, 3 }, new PoisonsAndLevels(0, 0, 0, 0, 0, 0, 25, 3, 0, 0, 0, 0), new List<PresetInventoryItem>(), 0, 1, 0, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, primaryRecoverAllInjuries: false, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, secondaryRecoverAllInjuries: false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 60, EMaterialFilterType.Poison, EMaterialFilterHardness.Invalid));
		_dataArray.Add(new MaterialItem(264, 528, 5, 506, 1, 264, "icon_Material_fushichong", 529, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 0, 10, 100, 200, 0, 2, 600, 3, allowRandomCreate: true, 40, isSpecial: false, 5, 12, EMaterialProperty.Yang, -1, -1, 25, 9, 30, 5, new List<short> { 156, 3 }, new PoisonsAndLevels(0, 0, 0, 0, 0, 0, 0, 0, 5, 1, 0, 0), new List<PresetInventoryItem>(), 0, 1, 0, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, primaryRecoverAllInjuries: false, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, secondaryRecoverAllInjuries: false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 60, EMaterialFilterType.Poison, EMaterialFilterHardness.Invalid));
		_dataArray.Add(new MaterialItem(265, 530, 5, 506, 2, 264, "icon_Material_fushichong", 531, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 0, 10, 300, 600, 0, 3, 1200, 4, allowRandomCreate: true, 35, isSpecial: false, 5, 12, EMaterialProperty.Yang, -1, -1, 50, 9, 60, 10, new List<short> { 156, 3 }, new PoisonsAndLevels(0, 0, 0, 0, 0, 0, 0, 0, 10, 1, 0, 0), new List<PresetInventoryItem>(), 0, 1, 0, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, primaryRecoverAllInjuries: false, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, secondaryRecoverAllInjuries: false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 60, EMaterialFilterType.Poison, EMaterialFilterHardness.Invalid));
		_dataArray.Add(new MaterialItem(266, 532, 5, 506, 3, 264, "icon_Material_sanwencao", 533, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 0, 10, 750, 1500, 0, 4, 1800, 5, allowRandomCreate: true, 30, isSpecial: false, 5, 12, EMaterialProperty.Yang, -1, -1, 100, 9, 100, 20, new List<short> { 156, 3 }, new PoisonsAndLevels(0, 0, 0, 0, 0, 0, 0, 0, 15, 1, 0, 0), new List<PresetInventoryItem>(), 0, 1, 0, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, primaryRecoverAllInjuries: false, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, secondaryRecoverAllInjuries: false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 60, EMaterialFilterType.Poison, EMaterialFilterHardness.Invalid));
		_dataArray.Add(new MaterialItem(267, 534, 5, 506, 4, 264, "icon_Material_sanwencao", 535, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 0, 10, 1550, 3100, 1, 5, 3000, 6, allowRandomCreate: true, 25, isSpecial: false, 5, 12, EMaterialProperty.Yang, -1, -1, 200, 9, 150, 40, new List<short> { 156, 3 }, new PoisonsAndLevels(0, 0, 0, 0, 0, 0, 0, 0, 15, 2, 0, 0), new List<PresetInventoryItem>(), 0, 1, 0, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, primaryRecoverAllInjuries: false, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, secondaryRecoverAllInjuries: false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 60, EMaterialFilterType.Poison, EMaterialFilterHardness.Invalid));
		_dataArray.Add(new MaterialItem(268, 536, 5, 506, 5, 264, "icon_Material_sanwencao", 537, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 0, 10, 2800, 5600, 2, 6, 4200, 7, allowRandomCreate: true, 20, isSpecial: false, 5, 12, EMaterialProperty.Yang, -1, -1, 300, 9, 210, 80, new List<short> { 156, 3 }, new PoisonsAndLevels(0, 0, 0, 0, 0, 0, 0, 0, 20, 2, 0, 0), new List<PresetInventoryItem>(), 0, 1, 0, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, primaryRecoverAllInjuries: false, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, secondaryRecoverAllInjuries: false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 60, EMaterialFilterType.Poison, EMaterialFilterHardness.Invalid));
		_dataArray.Add(new MaterialItem(269, 538, 5, 506, 6, 264, "icon_Material_heishuimingshegu", 539, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 0, 20, 4600, 9200, 3, 7, 5400, 7, allowRandomCreate: true, 15, isSpecial: false, 5, 12, EMaterialProperty.Yang, -1, -1, 400, 9, 280, 160, new List<short> { 156, 3 }, new PoisonsAndLevels(0, 0, 0, 0, 0, 0, 0, 0, 25, 2, 0, 0), new List<PresetInventoryItem>(), 0, 1, 0, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, primaryRecoverAllInjuries: false, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, secondaryRecoverAllInjuries: false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 60, EMaterialFilterType.Poison, EMaterialFilterHardness.Invalid));
		_dataArray.Add(new MaterialItem(270, 540, 5, 506, 7, 264, "icon_Material_qiannianqingzhu", 541, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 0, 10, 7050, 14100, 4, 8, 7200, 8, allowRandomCreate: true, 10, isSpecial: false, 5, 12, EMaterialProperty.Yang, -1, -1, 500, 9, 360, 320, new List<short> { 156, 3 }, new PoisonsAndLevels(0, 0, 0, 0, 0, 0, 0, 0, 25, 3, 0, 0), new List<PresetInventoryItem>(), 0, 1, 0, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, primaryRecoverAllInjuries: false, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, secondaryRecoverAllInjuries: false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 60, EMaterialFilterType.Poison, EMaterialFilterHardness.Invalid));
		_dataArray.Add(new MaterialItem(271, 542, 5, 506, 1, 271, "icon_Material_jiazhutao", 543, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 0, 10, 100, 200, 0, 2, 600, 3, allowRandomCreate: true, 40, isSpecial: false, 5, 12, EMaterialProperty.Yin, -1, -1, 25, 9, 30, 5, new List<short> { 157, 3 }, new PoisonsAndLevels(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 5, 1), new List<PresetInventoryItem>(), 0, 1, 0, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, primaryRecoverAllInjuries: false, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, secondaryRecoverAllInjuries: false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 60, EMaterialFilterType.Poison, EMaterialFilterHardness.Invalid));
		_dataArray.Add(new MaterialItem(272, 544, 5, 506, 2, 271, "icon_Material_jiazhutao", 545, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 0, 10, 300, 600, 0, 3, 1200, 4, allowRandomCreate: true, 35, isSpecial: false, 5, 12, EMaterialProperty.Yin, -1, -1, 50, 9, 60, 10, new List<short> { 157, 3 }, new PoisonsAndLevels(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 10, 1), new List<PresetInventoryItem>(), 0, 1, 0, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, primaryRecoverAllInjuries: false, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, secondaryRecoverAllInjuries: false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 60, EMaterialFilterType.Poison, EMaterialFilterHardness.Invalid));
		_dataArray.Add(new MaterialItem(273, 546, 5, 506, 3, 271, "icon_Material_fuhunsi", 547, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 0, 10, 750, 1500, 0, 4, 1800, 5, allowRandomCreate: true, 30, isSpecial: false, 5, 12, EMaterialProperty.Yin, -1, -1, 100, 9, 100, 20, new List<short> { 157, 3 }, new PoisonsAndLevels(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 15, 1), new List<PresetInventoryItem>(), 0, 1, 0, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, primaryRecoverAllInjuries: false, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, secondaryRecoverAllInjuries: false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 60, EMaterialFilterType.Poison, EMaterialFilterHardness.Invalid));
		_dataArray.Add(new MaterialItem(274, 548, 5, 506, 4, 271, "icon_Material_fuhunsi", 549, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 0, 10, 1550, 3100, 1, 5, 3000, 6, allowRandomCreate: true, 25, isSpecial: false, 5, 12, EMaterialProperty.Yin, -1, -1, 200, 9, 150, 40, new List<short> { 157, 3 }, new PoisonsAndLevels(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 15, 2), new List<PresetInventoryItem>(), 0, 1, 0, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, primaryRecoverAllInjuries: false, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, secondaryRecoverAllInjuries: false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 60, EMaterialFilterType.Poison, EMaterialFilterHardness.Invalid));
		_dataArray.Add(new MaterialItem(275, 550, 5, 506, 5, 271, "icon_Material_fuhunsi", 551, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 0, 10, 2800, 5600, 2, 6, 4200, 7, allowRandomCreate: true, 20, isSpecial: false, 5, 12, EMaterialProperty.Yin, -1, -1, 300, 9, 210, 80, new List<short> { 157, 3 }, new PoisonsAndLevels(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 20, 2), new List<PresetInventoryItem>(), 0, 1, 0, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, primaryRecoverAllInjuries: false, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, secondaryRecoverAllInjuries: false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 60, EMaterialFilterType.Poison, EMaterialFilterHardness.Invalid));
		_dataArray.Add(new MaterialItem(276, 552, 5, 506, 6, 271, "icon_Material_momeilan", 553, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 0, 10, 4600, 9200, 3, 7, 5400, 7, allowRandomCreate: true, 15, isSpecial: false, 5, 12, EMaterialProperty.Yin, -1, -1, 400, 9, 280, 160, new List<short> { 157, 3 }, new PoisonsAndLevels(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 25, 2), new List<PresetInventoryItem>(), 0, 1, 0, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, primaryRecoverAllInjuries: false, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, secondaryRecoverAllInjuries: false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 60, EMaterialFilterType.Poison, EMaterialFilterHardness.Invalid));
		_dataArray.Add(new MaterialItem(277, 554, 5, 506, 7, 271, "icon_Material_renmianmantuoluo", 555, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 0, 10, 7050, 14100, 4, 8, 7200, 8, allowRandomCreate: true, 10, isSpecial: false, 5, 12, EMaterialProperty.Yin, -1, -1, 500, 9, 360, 320, new List<short> { 157, 3 }, new PoisonsAndLevels(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 25, 3), new List<PresetInventoryItem>(), 0, 1, 0, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, primaryRecoverAllInjuries: false, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, secondaryRecoverAllInjuries: false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 60, EMaterialFilterType.Poison, EMaterialFilterHardness.Invalid));
		_dataArray.Add(new MaterialItem(278, 556, 5, 505, 8, -1, "icon_Material_dizhaoyufuhua", 557, transferable: false, stackable: true, wagerable: false, refinable: false, poisonable: false, repairable: false, inheritable: true, 0, 0, 0, 0, 0, 0, 0, 0, allowRandomCreate: true, 0, isSpecial: true, 5, -1, EMaterialProperty.Invalid, -1, -1, 0, 8, 450, 30000, new List<short> { 1 }, new PoisonsAndLevels(default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short)), new List<PresetInventoryItem>(), 0, 1, 0, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, primaryRecoverAllInjuries: false, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, secondaryRecoverAllInjuries: false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 60, EMaterialFilterType.Invalid, EMaterialFilterHardness.Invalid));
		_dataArray.Add(new MaterialItem(279, 558, 5, 501, 0, -1, "icon_Material_qingzhupian", 559, transferable: true, stackable: true, wagerable: false, refinable: false, poisonable: false, repairable: false, inheritable: true, 0, 0, 0, 0, 0, 0, 0, 0, allowRandomCreate: true, 0, isSpecial: true, 1, -1, EMaterialProperty.Invalid, -1, -1, 0, 7, 0, 10, new List<short> { 0 }, new PoisonsAndLevels(default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short)), new List<PresetInventoryItem>(), 0, 1, 0, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, primaryRecoverAllInjuries: false, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, secondaryRecoverAllInjuries: false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 60, EMaterialFilterType.Invalid, EMaterialFilterHardness.Invalid));
		_dataArray.Add(new MaterialItem(280, 560, 5, 506, 4, -1, "icon_Material_JiaoWhiteEgg", 561, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 0, 100, 10, 15, 0, 0, 50, 8, allowRandomCreate: false, 50, isSpecial: true, 5, 36, EMaterialProperty.Invalid, -1, -1, 200, 9, 150, 0, new List<short>(), new PoisonsAndLevels(15, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0), new List<PresetInventoryItem>
		{
			new PresetInventoryItem("Material", 236, 50, 100),
			new PresetInventoryItem("Material", 237, 40, 100),
			new PresetInventoryItem("Material", 238, 30, 100),
			new PresetInventoryItem("Material", 239, 20, 100),
			new PresetInventoryItem("Material", 240, 10, 100),
			new PresetInventoryItem("Material", 241, 5, 100),
			new PresetInventoryItem("Material", 242, 1, 100)
		}, 1, 1, 0, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, primaryRecoverAllInjuries: false, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, secondaryRecoverAllInjuries: false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 60, EMaterialFilterType.JiaoEgg, EMaterialFilterHardness.Invalid));
		_dataArray.Add(new MaterialItem(281, 562, 5, 506, 4, -1, "icon_Material_JiaoBlackEgg", 563, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 0, 100, 10, 15, 0, 0, 50, 8, allowRandomCreate: false, 50, isSpecial: true, 5, 36, EMaterialProperty.Invalid, -1, -1, 200, 9, 150, 0, new List<short>(), new PoisonsAndLevels(0, 0, 0, 0, 15, 2, 0, 0, 0, 0, 0, 0), new List<PresetInventoryItem>
		{
			new PresetInventoryItem("Material", 243, 50, 100),
			new PresetInventoryItem("Material", 244, 40, 100),
			new PresetInventoryItem("Material", 245, 30, 100),
			new PresetInventoryItem("Material", 246, 20, 100),
			new PresetInventoryItem("Material", 247, 10, 100),
			new PresetInventoryItem("Material", 248, 5, 100),
			new PresetInventoryItem("Material", 249, 1, 100)
		}, 1, 1, 0, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, primaryRecoverAllInjuries: false, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, secondaryRecoverAllInjuries: false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 60, EMaterialFilterType.JiaoEgg, EMaterialFilterHardness.Invalid));
		_dataArray.Add(new MaterialItem(282, 564, 5, 506, 4, -1, "icon_Material_JiaoGreenEgg", 565, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 0, 100, 10, 15, 0, 0, 50, 8, allowRandomCreate: false, 50, isSpecial: true, 5, 36, EMaterialProperty.Invalid, -1, -1, 200, 9, 150, 0, new List<short>(), new PoisonsAndLevels(0, 0, 15, 2, 0, 0, 0, 0, 0, 0, 0, 0), new List<PresetInventoryItem>
		{
			new PresetInventoryItem("Material", 250, 50, 100),
			new PresetInventoryItem("Material", 251, 40, 100),
			new PresetInventoryItem("Material", 252, 30, 100),
			new PresetInventoryItem("Material", 253, 20, 100),
			new PresetInventoryItem("Material", 254, 10, 100),
			new PresetInventoryItem("Material", 255, 5, 100),
			new PresetInventoryItem("Material", 256, 1, 100)
		}, 1, 1, 0, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, primaryRecoverAllInjuries: false, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, secondaryRecoverAllInjuries: false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 60, EMaterialFilterType.JiaoEgg, EMaterialFilterHardness.Invalid));
		_dataArray.Add(new MaterialItem(283, 566, 5, 506, 4, -1, "icon_Material_JiaoRedEgg", 567, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 0, 100, 10, 15, 0, 0, 50, 8, allowRandomCreate: false, 50, isSpecial: true, 5, 36, EMaterialProperty.Invalid, -1, -1, 200, 9, 150, 0, new List<short>(), new PoisonsAndLevels(0, 0, 0, 0, 0, 0, 15, 2, 0, 0, 0, 0), new List<PresetInventoryItem>
		{
			new PresetInventoryItem("Material", 257, 50, 100),
			new PresetInventoryItem("Material", 258, 40, 100),
			new PresetInventoryItem("Material", 259, 30, 100),
			new PresetInventoryItem("Material", 260, 20, 100),
			new PresetInventoryItem("Material", 261, 10, 100),
			new PresetInventoryItem("Material", 262, 5, 100),
			new PresetInventoryItem("Material", 263, 1, 100)
		}, 1, 1, 0, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, primaryRecoverAllInjuries: false, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, secondaryRecoverAllInjuries: false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 60, EMaterialFilterType.JiaoEgg, EMaterialFilterHardness.Invalid));
		_dataArray.Add(new MaterialItem(284, 568, 5, 506, 4, -1, "icon_Material_JiaoYellowEgg", 569, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 0, 100, 10, 15, 0, 0, 50, 8, allowRandomCreate: false, 50, isSpecial: true, 5, 36, EMaterialProperty.Invalid, -1, -1, 200, 9, 150, 0, new List<short>(), new PoisonsAndLevels(0, 0, 0, 0, 0, 0, 0, 0, 15, 2, 0, 0), new List<PresetInventoryItem>
		{
			new PresetInventoryItem("Material", 264, 50, 100),
			new PresetInventoryItem("Material", 265, 40, 100),
			new PresetInventoryItem("Material", 266, 30, 100),
			new PresetInventoryItem("Material", 267, 20, 100),
			new PresetInventoryItem("Material", 268, 10, 100),
			new PresetInventoryItem("Material", 269, 5, 100),
			new PresetInventoryItem("Material", 270, 1, 100)
		}, 1, 1, 0, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, primaryRecoverAllInjuries: false, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, secondaryRecoverAllInjuries: false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 60, EMaterialFilterType.JiaoEgg, EMaterialFilterHardness.Invalid));
		_dataArray.Add(new MaterialItem(285, 570, 5, 506, 5, -1, "icon_Material_JiaoWBEgg", 571, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 0, 100, 10, 15, 0, 0, 50, 8, allowRandomCreate: false, 40, isSpecial: true, 5, 36, EMaterialProperty.Invalid, -1, -1, 300, 9, 210, 0, new List<short>(), new PoisonsAndLevels(20, 2, 0, 0, 20, 2, 0, 0, 0, 0, 0, 0), new List<PresetInventoryItem>
		{
			new PresetInventoryItem("Material", 237, 40, 100),
			new PresetInventoryItem("Material", 238, 30, 100),
			new PresetInventoryItem("Material", 239, 20, 100),
			new PresetInventoryItem("Material", 240, 10, 100),
			new PresetInventoryItem("Material", 241, 5, 100),
			new PresetInventoryItem("Material", 242, 1, 100),
			new PresetInventoryItem("Material", 251, 40, 100),
			new PresetInventoryItem("Material", 252, 30, 100),
			new PresetInventoryItem("Material", 253, 20, 100),
			new PresetInventoryItem("Material", 254, 10, 100),
			new PresetInventoryItem("Material", 255, 5, 100),
			new PresetInventoryItem("Material", 256, 1, 100)
		}, 1, 1, 0, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, primaryRecoverAllInjuries: false, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, secondaryRecoverAllInjuries: false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 60, EMaterialFilterType.JiaoEgg, EMaterialFilterHardness.Invalid));
		_dataArray.Add(new MaterialItem(286, 572, 5, 506, 5, -1, "icon_Material_JiaoWGEgg", 573, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 0, 100, 10, 15, 0, 0, 50, 8, allowRandomCreate: false, 40, isSpecial: true, 5, 36, EMaterialProperty.Invalid, -1, -1, 300, 9, 210, 0, new List<short>(), new PoisonsAndLevels(20, 2, 20, 2, 0, 0, 0, 0, 0, 0, 0, 0), new List<PresetInventoryItem>
		{
			new PresetInventoryItem("Material", 237, 40, 100),
			new PresetInventoryItem("Material", 238, 30, 100),
			new PresetInventoryItem("Material", 239, 20, 100),
			new PresetInventoryItem("Material", 240, 10, 100),
			new PresetInventoryItem("Material", 241, 5, 100),
			new PresetInventoryItem("Material", 242, 1, 100),
			new PresetInventoryItem("Material", 244, 40, 100),
			new PresetInventoryItem("Material", 245, 30, 100),
			new PresetInventoryItem("Material", 246, 20, 100),
			new PresetInventoryItem("Material", 247, 10, 100),
			new PresetInventoryItem("Material", 248, 5, 100),
			new PresetInventoryItem("Material", 249, 1, 100)
		}, 1, 1, 0, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, primaryRecoverAllInjuries: false, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, secondaryRecoverAllInjuries: false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 60, EMaterialFilterType.JiaoEgg, EMaterialFilterHardness.Invalid));
		_dataArray.Add(new MaterialItem(287, 574, 5, 506, 5, -1, "icon_Material_JiaoWREgg", 575, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 0, 100, 10, 15, 0, 0, 50, 8, allowRandomCreate: false, 40, isSpecial: true, 5, 36, EMaterialProperty.Invalid, -1, -1, 300, 9, 210, 0, new List<short>(), new PoisonsAndLevels(20, 2, 0, 0, 0, 0, 20, 2, 0, 0, 0, 0), new List<PresetInventoryItem>
		{
			new PresetInventoryItem("Material", 237, 40, 100),
			new PresetInventoryItem("Material", 238, 30, 100),
			new PresetInventoryItem("Material", 239, 20, 100),
			new PresetInventoryItem("Material", 240, 10, 100),
			new PresetInventoryItem("Material", 241, 5, 100),
			new PresetInventoryItem("Material", 242, 1, 100),
			new PresetInventoryItem("Material", 258, 40, 100),
			new PresetInventoryItem("Material", 259, 30, 100),
			new PresetInventoryItem("Material", 260, 20, 100),
			new PresetInventoryItem("Material", 261, 10, 100),
			new PresetInventoryItem("Material", 262, 5, 100),
			new PresetInventoryItem("Material", 263, 1, 100)
		}, 1, 1, 0, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, primaryRecoverAllInjuries: false, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, secondaryRecoverAllInjuries: false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 60, EMaterialFilterType.JiaoEgg, EMaterialFilterHardness.Invalid));
		_dataArray.Add(new MaterialItem(288, 576, 5, 506, 5, -1, "icon_Material_JiaoWYEgg", 577, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 0, 100, 10, 15, 0, 0, 50, 8, allowRandomCreate: false, 40, isSpecial: true, 5, 36, EMaterialProperty.Invalid, -1, -1, 300, 9, 210, 0, new List<short>(), new PoisonsAndLevels(20, 2, 0, 0, 0, 0, 0, 0, 20, 2, 0, 0), new List<PresetInventoryItem>
		{
			new PresetInventoryItem("Material", 237, 40, 100),
			new PresetInventoryItem("Material", 238, 30, 100),
			new PresetInventoryItem("Material", 239, 20, 100),
			new PresetInventoryItem("Material", 240, 10, 100),
			new PresetInventoryItem("Material", 241, 5, 100),
			new PresetInventoryItem("Material", 242, 1, 100),
			new PresetInventoryItem("Material", 265, 40, 100),
			new PresetInventoryItem("Material", 266, 30, 100),
			new PresetInventoryItem("Material", 267, 20, 100),
			new PresetInventoryItem("Material", 268, 10, 100),
			new PresetInventoryItem("Material", 269, 5, 100),
			new PresetInventoryItem("Material", 270, 1, 100)
		}, 1, 1, 0, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, primaryRecoverAllInjuries: false, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, secondaryRecoverAllInjuries: false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 60, EMaterialFilterType.JiaoEgg, EMaterialFilterHardness.Invalid));
		_dataArray.Add(new MaterialItem(289, 578, 5, 506, 5, -1, "icon_Material_JiaoBGEgg", 579, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 0, 100, 10, 15, 0, 0, 50, 8, allowRandomCreate: false, 40, isSpecial: true, 5, 36, EMaterialProperty.Invalid, -1, -1, 300, 9, 210, 0, new List<short>(), new PoisonsAndLevels(0, 0, 20, 2, 20, 2, 0, 0, 0, 0, 0, 0), new List<PresetInventoryItem>
		{
			new PresetInventoryItem("Material", 251, 40, 100),
			new PresetInventoryItem("Material", 252, 30, 100),
			new PresetInventoryItem("Material", 253, 20, 100),
			new PresetInventoryItem("Material", 254, 10, 100),
			new PresetInventoryItem("Material", 255, 5, 100),
			new PresetInventoryItem("Material", 256, 1, 100),
			new PresetInventoryItem("Material", 244, 40, 100),
			new PresetInventoryItem("Material", 245, 30, 100),
			new PresetInventoryItem("Material", 246, 20, 100),
			new PresetInventoryItem("Material", 247, 10, 100),
			new PresetInventoryItem("Material", 248, 5, 100),
			new PresetInventoryItem("Material", 249, 1, 100)
		}, 1, 1, 0, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, primaryRecoverAllInjuries: false, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, secondaryRecoverAllInjuries: false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 60, EMaterialFilterType.JiaoEgg, EMaterialFilterHardness.Invalid));
		_dataArray.Add(new MaterialItem(290, 580, 5, 506, 5, -1, "icon_Material_JiaoBREgg", 581, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 0, 100, 10, 15, 0, 0, 50, 8, allowRandomCreate: false, 40, isSpecial: true, 5, 36, EMaterialProperty.Invalid, -1, -1, 300, 9, 210, 0, new List<short>(), new PoisonsAndLevels(0, 0, 0, 0, 20, 2, 20, 2, 0, 0, 0, 0), new List<PresetInventoryItem>
		{
			new PresetInventoryItem("Material", 251, 40, 100),
			new PresetInventoryItem("Material", 252, 30, 100),
			new PresetInventoryItem("Material", 253, 20, 100),
			new PresetInventoryItem("Material", 254, 10, 100),
			new PresetInventoryItem("Material", 255, 5, 100),
			new PresetInventoryItem("Material", 256, 1, 100),
			new PresetInventoryItem("Material", 258, 40, 100),
			new PresetInventoryItem("Material", 259, 30, 100),
			new PresetInventoryItem("Material", 260, 20, 100),
			new PresetInventoryItem("Material", 261, 10, 100),
			new PresetInventoryItem("Material", 262, 5, 100),
			new PresetInventoryItem("Material", 263, 1, 100)
		}, 1, 1, 0, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, primaryRecoverAllInjuries: false, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, secondaryRecoverAllInjuries: false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 60, EMaterialFilterType.JiaoEgg, EMaterialFilterHardness.Invalid));
		_dataArray.Add(new MaterialItem(291, 582, 5, 506, 5, -1, "icon_Material_JiaoBYEgg", 583, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 0, 100, 10, 15, 0, 0, 50, 8, allowRandomCreate: false, 40, isSpecial: true, 5, 36, EMaterialProperty.Invalid, -1, -1, 300, 9, 210, 0, new List<short>(), new PoisonsAndLevels(0, 0, 0, 0, 20, 2, 0, 0, 20, 2, 0, 0), new List<PresetInventoryItem>
		{
			new PresetInventoryItem("Material", 251, 40, 100),
			new PresetInventoryItem("Material", 252, 30, 100),
			new PresetInventoryItem("Material", 253, 20, 100),
			new PresetInventoryItem("Material", 254, 10, 100),
			new PresetInventoryItem("Material", 255, 5, 100),
			new PresetInventoryItem("Material", 256, 1, 100),
			new PresetInventoryItem("Material", 265, 40, 100),
			new PresetInventoryItem("Material", 266, 30, 100),
			new PresetInventoryItem("Material", 267, 20, 100),
			new PresetInventoryItem("Material", 268, 10, 100),
			new PresetInventoryItem("Material", 269, 5, 100),
			new PresetInventoryItem("Material", 270, 1, 100)
		}, 1, 1, 0, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, primaryRecoverAllInjuries: false, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, secondaryRecoverAllInjuries: false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 60, EMaterialFilterType.JiaoEgg, EMaterialFilterHardness.Invalid));
		_dataArray.Add(new MaterialItem(292, 584, 5, 506, 5, -1, "icon_Material_JiaoGREgg", 585, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 0, 100, 10, 15, 0, 0, 50, 8, allowRandomCreate: false, 40, isSpecial: true, 5, 36, EMaterialProperty.Invalid, -1, -1, 300, 9, 210, 0, new List<short>(), new PoisonsAndLevels(0, 0, 20, 2, 0, 0, 20, 2, 0, 0, 0, 0), new List<PresetInventoryItem>
		{
			new PresetInventoryItem("Material", 244, 40, 100),
			new PresetInventoryItem("Material", 245, 30, 100),
			new PresetInventoryItem("Material", 246, 20, 100),
			new PresetInventoryItem("Material", 247, 10, 100),
			new PresetInventoryItem("Material", 248, 5, 100),
			new PresetInventoryItem("Material", 249, 1, 100),
			new PresetInventoryItem("Material", 258, 40, 100),
			new PresetInventoryItem("Material", 259, 30, 100),
			new PresetInventoryItem("Material", 260, 20, 100),
			new PresetInventoryItem("Material", 261, 10, 100),
			new PresetInventoryItem("Material", 262, 5, 100),
			new PresetInventoryItem("Material", 263, 1, 100)
		}, 1, 1, 0, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, primaryRecoverAllInjuries: false, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, secondaryRecoverAllInjuries: false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 60, EMaterialFilterType.JiaoEgg, EMaterialFilterHardness.Invalid));
		_dataArray.Add(new MaterialItem(293, 586, 5, 506, 5, -1, "icon_Material_JiaoGYEgg", 587, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 0, 100, 10, 15, 0, 0, 50, 8, allowRandomCreate: false, 40, isSpecial: true, 5, 36, EMaterialProperty.Invalid, -1, -1, 300, 9, 210, 0, new List<short>(), new PoisonsAndLevels(0, 0, 20, 2, 0, 0, 0, 0, 20, 2, 0, 0), new List<PresetInventoryItem>
		{
			new PresetInventoryItem("Material", 244, 40, 100),
			new PresetInventoryItem("Material", 245, 30, 100),
			new PresetInventoryItem("Material", 246, 20, 100),
			new PresetInventoryItem("Material", 247, 10, 100),
			new PresetInventoryItem("Material", 248, 5, 100),
			new PresetInventoryItem("Material", 249, 1, 100),
			new PresetInventoryItem("Material", 265, 40, 100),
			new PresetInventoryItem("Material", 266, 30, 100),
			new PresetInventoryItem("Material", 267, 20, 100),
			new PresetInventoryItem("Material", 268, 10, 100),
			new PresetInventoryItem("Material", 269, 5, 100),
			new PresetInventoryItem("Material", 270, 1, 100)
		}, 1, 1, 0, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, primaryRecoverAllInjuries: false, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, secondaryRecoverAllInjuries: false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 60, EMaterialFilterType.JiaoEgg, EMaterialFilterHardness.Invalid));
		_dataArray.Add(new MaterialItem(294, 588, 5, 506, 5, -1, "icon_Material_JiaoRYEgg", 589, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 0, 100, 10, 15, 0, 0, 50, 8, allowRandomCreate: false, 40, isSpecial: true, 5, 36, EMaterialProperty.Invalid, -1, -1, 300, 9, 210, 0, new List<short>(), new PoisonsAndLevels(0, 0, 0, 0, 0, 0, 20, 2, 20, 2, 0, 0), new List<PresetInventoryItem>
		{
			new PresetInventoryItem("Material", 258, 40, 100),
			new PresetInventoryItem("Material", 259, 30, 100),
			new PresetInventoryItem("Material", 260, 20, 100),
			new PresetInventoryItem("Material", 261, 10, 100),
			new PresetInventoryItem("Material", 262, 5, 100),
			new PresetInventoryItem("Material", 263, 1, 100),
			new PresetInventoryItem("Material", 265, 40, 100),
			new PresetInventoryItem("Material", 266, 30, 100),
			new PresetInventoryItem("Material", 267, 20, 100),
			new PresetInventoryItem("Material", 268, 10, 100),
			new PresetInventoryItem("Material", 269, 5, 100),
			new PresetInventoryItem("Material", 270, 1, 100)
		}, 1, 1, 0, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, primaryRecoverAllInjuries: false, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, secondaryRecoverAllInjuries: false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 60, EMaterialFilterType.JiaoEgg, EMaterialFilterHardness.Invalid));
		_dataArray.Add(new MaterialItem(295, 590, 5, 506, 6, -1, "icon_Material_JiaoWBGEgg", 591, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 0, 100, 10, 15, 0, 0, 50, 8, allowRandomCreate: false, 30, isSpecial: true, 5, 36, EMaterialProperty.Invalid, -1, -1, 400, 9, 280, 0, new List<short>(), new PoisonsAndLevels(25, 2, 25, 2, 25, 2, 0, 0, 0, 0, 0, 0), new List<PresetInventoryItem>
		{
			new PresetInventoryItem("Material", 238, 30, 100),
			new PresetInventoryItem("Material", 239, 20, 100),
			new PresetInventoryItem("Material", 240, 10, 100),
			new PresetInventoryItem("Material", 241, 5, 100),
			new PresetInventoryItem("Material", 242, 1, 100),
			new PresetInventoryItem("Material", 252, 30, 100),
			new PresetInventoryItem("Material", 253, 20, 100),
			new PresetInventoryItem("Material", 254, 10, 100),
			new PresetInventoryItem("Material", 255, 5, 100),
			new PresetInventoryItem("Material", 256, 1, 100),
			new PresetInventoryItem("Material", 245, 30, 100),
			new PresetInventoryItem("Material", 246, 20, 100),
			new PresetInventoryItem("Material", 247, 10, 100),
			new PresetInventoryItem("Material", 248, 5, 100),
			new PresetInventoryItem("Material", 249, 1, 100)
		}, 1, 1, 0, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, primaryRecoverAllInjuries: false, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, secondaryRecoverAllInjuries: false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 60, EMaterialFilterType.JiaoEgg, EMaterialFilterHardness.Invalid));
		_dataArray.Add(new MaterialItem(296, 592, 5, 506, 6, -1, "icon_Material_JiaoWBREgg", 593, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 0, 100, 10, 15, 0, 0, 50, 8, allowRandomCreate: false, 30, isSpecial: true, 5, 36, EMaterialProperty.Invalid, -1, -1, 400, 9, 280, 0, new List<short>(), new PoisonsAndLevels(25, 2, 0, 0, 25, 2, 25, 2, 0, 0, 0, 0), new List<PresetInventoryItem>
		{
			new PresetInventoryItem("Material", 238, 30, 100),
			new PresetInventoryItem("Material", 239, 20, 100),
			new PresetInventoryItem("Material", 240, 10, 100),
			new PresetInventoryItem("Material", 241, 5, 100),
			new PresetInventoryItem("Material", 242, 1, 100),
			new PresetInventoryItem("Material", 252, 30, 100),
			new PresetInventoryItem("Material", 253, 20, 100),
			new PresetInventoryItem("Material", 254, 10, 100),
			new PresetInventoryItem("Material", 255, 5, 100),
			new PresetInventoryItem("Material", 256, 1, 100),
			new PresetInventoryItem("Material", 259, 30, 100),
			new PresetInventoryItem("Material", 260, 20, 100),
			new PresetInventoryItem("Material", 261, 10, 100),
			new PresetInventoryItem("Material", 262, 5, 100),
			new PresetInventoryItem("Material", 263, 1, 100)
		}, 1, 1, 0, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, primaryRecoverAllInjuries: false, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, secondaryRecoverAllInjuries: false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 60, EMaterialFilterType.JiaoEgg, EMaterialFilterHardness.Invalid));
		_dataArray.Add(new MaterialItem(297, 594, 5, 506, 6, -1, "icon_Material_JiaoWBYEgg", 595, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 0, 100, 10, 15, 0, 0, 50, 8, allowRandomCreate: false, 30, isSpecial: true, 5, 36, EMaterialProperty.Invalid, -1, -1, 400, 9, 280, 0, new List<short>(), new PoisonsAndLevels(25, 2, 0, 0, 25, 2, 0, 0, 25, 2, 0, 0), new List<PresetInventoryItem>
		{
			new PresetInventoryItem("Material", 238, 30, 100),
			new PresetInventoryItem("Material", 239, 20, 100),
			new PresetInventoryItem("Material", 240, 10, 100),
			new PresetInventoryItem("Material", 241, 5, 100),
			new PresetInventoryItem("Material", 242, 1, 100),
			new PresetInventoryItem("Material", 252, 30, 100),
			new PresetInventoryItem("Material", 253, 20, 100),
			new PresetInventoryItem("Material", 254, 10, 100),
			new PresetInventoryItem("Material", 255, 5, 100),
			new PresetInventoryItem("Material", 256, 1, 100),
			new PresetInventoryItem("Material", 266, 30, 100),
			new PresetInventoryItem("Material", 267, 20, 100),
			new PresetInventoryItem("Material", 268, 10, 100),
			new PresetInventoryItem("Material", 269, 5, 100),
			new PresetInventoryItem("Material", 270, 1, 100)
		}, 1, 1, 0, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, primaryRecoverAllInjuries: false, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, secondaryRecoverAllInjuries: false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 60, EMaterialFilterType.JiaoEgg, EMaterialFilterHardness.Invalid));
		_dataArray.Add(new MaterialItem(298, 596, 5, 506, 6, -1, "icon_Material_JiaoWGREgg", 597, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 0, 100, 10, 15, 0, 0, 50, 8, allowRandomCreate: false, 30, isSpecial: true, 5, 36, EMaterialProperty.Invalid, -1, -1, 400, 9, 280, 0, new List<short>(), new PoisonsAndLevels(25, 2, 25, 2, 0, 0, 25, 2, 0, 0, 0, 0), new List<PresetInventoryItem>
		{
			new PresetInventoryItem("Material", 238, 30, 100),
			new PresetInventoryItem("Material", 239, 20, 100),
			new PresetInventoryItem("Material", 240, 10, 100),
			new PresetInventoryItem("Material", 241, 5, 100),
			new PresetInventoryItem("Material", 242, 1, 100),
			new PresetInventoryItem("Material", 245, 30, 100),
			new PresetInventoryItem("Material", 246, 20, 100),
			new PresetInventoryItem("Material", 247, 10, 100),
			new PresetInventoryItem("Material", 248, 5, 100),
			new PresetInventoryItem("Material", 249, 1, 100),
			new PresetInventoryItem("Material", 259, 30, 100),
			new PresetInventoryItem("Material", 260, 20, 100),
			new PresetInventoryItem("Material", 261, 10, 100),
			new PresetInventoryItem("Material", 262, 5, 100),
			new PresetInventoryItem("Material", 263, 1, 100)
		}, 1, 1, 0, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, primaryRecoverAllInjuries: false, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, secondaryRecoverAllInjuries: false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 60, EMaterialFilterType.JiaoEgg, EMaterialFilterHardness.Invalid));
		_dataArray.Add(new MaterialItem(299, 598, 5, 506, 6, -1, "icon_Material_JiaoWGYEgg", 599, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 0, 100, 10, 15, 0, 0, 50, 8, allowRandomCreate: false, 30, isSpecial: true, 5, 36, EMaterialProperty.Invalid, -1, -1, 400, 9, 280, 0, new List<short>(), new PoisonsAndLevels(25, 2, 25, 2, 0, 0, 0, 0, 25, 2, 0, 0), new List<PresetInventoryItem>
		{
			new PresetInventoryItem("Material", 238, 30, 100),
			new PresetInventoryItem("Material", 239, 20, 100),
			new PresetInventoryItem("Material", 240, 10, 100),
			new PresetInventoryItem("Material", 241, 5, 100),
			new PresetInventoryItem("Material", 242, 1, 100),
			new PresetInventoryItem("Material", 245, 30, 100),
			new PresetInventoryItem("Material", 246, 20, 100),
			new PresetInventoryItem("Material", 247, 10, 100),
			new PresetInventoryItem("Material", 248, 5, 100),
			new PresetInventoryItem("Material", 249, 1, 100),
			new PresetInventoryItem("Material", 266, 30, 100),
			new PresetInventoryItem("Material", 267, 20, 100),
			new PresetInventoryItem("Material", 268, 10, 100),
			new PresetInventoryItem("Material", 269, 5, 100),
			new PresetInventoryItem("Material", 270, 1, 100)
		}, 1, 1, 0, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, primaryRecoverAllInjuries: false, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, secondaryRecoverAllInjuries: false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 60, EMaterialFilterType.JiaoEgg, EMaterialFilterHardness.Invalid));
	}

	private void CreateItems5()
	{
		_dataArray.Add(new MaterialItem(300, 600, 5, 506, 6, -1, "icon_Material_JiaoWRYEgg", 601, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 0, 100, 10, 15, 0, 0, 50, 8, allowRandomCreate: false, 30, isSpecial: true, 5, 36, EMaterialProperty.Invalid, -1, -1, 400, 9, 280, 0, new List<short>(), new PoisonsAndLevels(25, 2, 0, 0, 0, 0, 25, 2, 25, 2, 0, 0), new List<PresetInventoryItem>
		{
			new PresetInventoryItem("Material", 238, 30, 100),
			new PresetInventoryItem("Material", 239, 20, 100),
			new PresetInventoryItem("Material", 240, 10, 100),
			new PresetInventoryItem("Material", 241, 5, 100),
			new PresetInventoryItem("Material", 242, 1, 100),
			new PresetInventoryItem("Material", 259, 30, 100),
			new PresetInventoryItem("Material", 260, 20, 100),
			new PresetInventoryItem("Material", 261, 10, 100),
			new PresetInventoryItem("Material", 262, 5, 100),
			new PresetInventoryItem("Material", 263, 1, 100),
			new PresetInventoryItem("Material", 266, 30, 100),
			new PresetInventoryItem("Material", 267, 20, 100),
			new PresetInventoryItem("Material", 268, 10, 100),
			new PresetInventoryItem("Material", 269, 5, 100),
			new PresetInventoryItem("Material", 270, 1, 100)
		}, 1, 1, 0, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, primaryRecoverAllInjuries: false, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, secondaryRecoverAllInjuries: false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 60, EMaterialFilterType.JiaoEgg, EMaterialFilterHardness.Invalid));
		_dataArray.Add(new MaterialItem(301, 602, 5, 506, 6, -1, "icon_Material_JiaoBGREgg", 603, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 0, 100, 10, 15, 0, 0, 50, 8, allowRandomCreate: false, 30, isSpecial: true, 5, 36, EMaterialProperty.Invalid, -1, -1, 400, 9, 280, 0, new List<short>(), new PoisonsAndLevels(0, 0, 25, 2, 25, 2, 25, 2, 0, 0, 0, 0), new List<PresetInventoryItem>
		{
			new PresetInventoryItem("Material", 252, 30, 100),
			new PresetInventoryItem("Material", 253, 20, 100),
			new PresetInventoryItem("Material", 254, 10, 100),
			new PresetInventoryItem("Material", 255, 5, 100),
			new PresetInventoryItem("Material", 256, 1, 100),
			new PresetInventoryItem("Material", 245, 30, 100),
			new PresetInventoryItem("Material", 246, 20, 100),
			new PresetInventoryItem("Material", 247, 10, 100),
			new PresetInventoryItem("Material", 248, 5, 100),
			new PresetInventoryItem("Material", 249, 1, 100),
			new PresetInventoryItem("Material", 259, 30, 100),
			new PresetInventoryItem("Material", 260, 20, 100),
			new PresetInventoryItem("Material", 261, 10, 100),
			new PresetInventoryItem("Material", 262, 5, 100),
			new PresetInventoryItem("Material", 263, 1, 100)
		}, 1, 1, 0, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, primaryRecoverAllInjuries: false, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, secondaryRecoverAllInjuries: false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 60, EMaterialFilterType.JiaoEgg, EMaterialFilterHardness.Invalid));
		_dataArray.Add(new MaterialItem(302, 604, 5, 506, 6, -1, "icon_Material_JiaoBGYEgg", 605, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 0, 100, 10, 15, 0, 0, 50, 8, allowRandomCreate: false, 30, isSpecial: true, 5, 36, EMaterialProperty.Invalid, -1, -1, 400, 9, 280, 0, new List<short>(), new PoisonsAndLevels(0, 0, 25, 2, 25, 2, 0, 0, 25, 2, 0, 0), new List<PresetInventoryItem>
		{
			new PresetInventoryItem("Material", 252, 30, 100),
			new PresetInventoryItem("Material", 253, 20, 100),
			new PresetInventoryItem("Material", 254, 10, 100),
			new PresetInventoryItem("Material", 255, 5, 100),
			new PresetInventoryItem("Material", 256, 1, 100),
			new PresetInventoryItem("Material", 245, 30, 100),
			new PresetInventoryItem("Material", 246, 20, 100),
			new PresetInventoryItem("Material", 247, 10, 100),
			new PresetInventoryItem("Material", 248, 5, 100),
			new PresetInventoryItem("Material", 249, 1, 100),
			new PresetInventoryItem("Material", 266, 30, 100),
			new PresetInventoryItem("Material", 267, 20, 100),
			new PresetInventoryItem("Material", 268, 10, 100),
			new PresetInventoryItem("Material", 269, 5, 100),
			new PresetInventoryItem("Material", 270, 1, 100)
		}, 1, 1, 0, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, primaryRecoverAllInjuries: false, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, secondaryRecoverAllInjuries: false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 60, EMaterialFilterType.JiaoEgg, EMaterialFilterHardness.Invalid));
		_dataArray.Add(new MaterialItem(303, 606, 5, 506, 6, -1, "icon_Material_JiaoBRYEgg", 607, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 0, 100, 10, 15, 0, 0, 50, 8, allowRandomCreate: false, 30, isSpecial: true, 5, 36, EMaterialProperty.Invalid, -1, -1, 400, 9, 280, 0, new List<short>(), new PoisonsAndLevels(0, 0, 0, 0, 25, 2, 25, 2, 25, 2, 0, 0), new List<PresetInventoryItem>
		{
			new PresetInventoryItem("Material", 252, 30, 100),
			new PresetInventoryItem("Material", 253, 20, 100),
			new PresetInventoryItem("Material", 254, 10, 100),
			new PresetInventoryItem("Material", 255, 5, 100),
			new PresetInventoryItem("Material", 256, 1, 100),
			new PresetInventoryItem("Material", 259, 30, 100),
			new PresetInventoryItem("Material", 260, 20, 100),
			new PresetInventoryItem("Material", 261, 10, 100),
			new PresetInventoryItem("Material", 262, 5, 100),
			new PresetInventoryItem("Material", 263, 1, 100),
			new PresetInventoryItem("Material", 266, 30, 100),
			new PresetInventoryItem("Material", 267, 20, 100),
			new PresetInventoryItem("Material", 268, 10, 100),
			new PresetInventoryItem("Material", 269, 5, 100),
			new PresetInventoryItem("Material", 270, 1, 100)
		}, 1, 1, 0, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, primaryRecoverAllInjuries: false, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, secondaryRecoverAllInjuries: false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 60, EMaterialFilterType.JiaoEgg, EMaterialFilterHardness.Invalid));
		_dataArray.Add(new MaterialItem(304, 608, 5, 506, 6, -1, "icon_Material_JiaoGRYEgg", 609, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 0, 100, 10, 15, 0, 0, 50, 8, allowRandomCreate: false, 30, isSpecial: true, 5, 36, EMaterialProperty.Invalid, -1, -1, 400, 9, 280, 0, new List<short>(), new PoisonsAndLevels(0, 0, 25, 2, 0, 0, 25, 2, 25, 2, 0, 0), new List<PresetInventoryItem>
		{
			new PresetInventoryItem("Material", 245, 30, 100),
			new PresetInventoryItem("Material", 246, 20, 100),
			new PresetInventoryItem("Material", 247, 10, 100),
			new PresetInventoryItem("Material", 248, 5, 100),
			new PresetInventoryItem("Material", 249, 1, 100),
			new PresetInventoryItem("Material", 259, 30, 100),
			new PresetInventoryItem("Material", 260, 20, 100),
			new PresetInventoryItem("Material", 261, 10, 100),
			new PresetInventoryItem("Material", 262, 5, 100),
			new PresetInventoryItem("Material", 263, 1, 100),
			new PresetInventoryItem("Material", 266, 30, 100),
			new PresetInventoryItem("Material", 267, 20, 100),
			new PresetInventoryItem("Material", 268, 10, 100),
			new PresetInventoryItem("Material", 269, 5, 100),
			new PresetInventoryItem("Material", 270, 1, 100)
		}, 1, 1, 0, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, primaryRecoverAllInjuries: false, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, secondaryRecoverAllInjuries: false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 60, EMaterialFilterType.JiaoEgg, EMaterialFilterHardness.Invalid));
		_dataArray.Add(new MaterialItem(305, 610, 5, 506, 7, -1, "icon_Material_JiaoWBGREgg", 611, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 0, 100, 10, 15, 0, 0, 50, 8, allowRandomCreate: false, 20, isSpecial: true, 5, 36, EMaterialProperty.Invalid, -1, -1, 500, 9, 360, 0, new List<short>(), new PoisonsAndLevels(25, 3, 25, 3, 25, 3, 25, 3, 0, 0, 0, 0), new List<PresetInventoryItem>
		{
			new PresetInventoryItem("Material", 239, 20, 100),
			new PresetInventoryItem("Material", 240, 10, 100),
			new PresetInventoryItem("Material", 241, 5, 100),
			new PresetInventoryItem("Material", 242, 1, 100),
			new PresetInventoryItem("Material", 253, 20, 100),
			new PresetInventoryItem("Material", 254, 10, 100),
			new PresetInventoryItem("Material", 255, 5, 100),
			new PresetInventoryItem("Material", 256, 1, 100),
			new PresetInventoryItem("Material", 246, 20, 100),
			new PresetInventoryItem("Material", 247, 10, 100),
			new PresetInventoryItem("Material", 248, 5, 100),
			new PresetInventoryItem("Material", 249, 1, 100),
			new PresetInventoryItem("Material", 260, 20, 100),
			new PresetInventoryItem("Material", 261, 10, 100),
			new PresetInventoryItem("Material", 262, 5, 100),
			new PresetInventoryItem("Material", 263, 1, 100)
		}, 1, 1, 0, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, primaryRecoverAllInjuries: false, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, secondaryRecoverAllInjuries: false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 60, EMaterialFilterType.JiaoEgg, EMaterialFilterHardness.Invalid));
		_dataArray.Add(new MaterialItem(306, 612, 5, 506, 7, -1, "icon_Material_JiaoWBGYEgg", 613, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 0, 100, 10, 15, 0, 0, 50, 8, allowRandomCreate: false, 20, isSpecial: true, 5, 36, EMaterialProperty.Invalid, -1, -1, 500, 9, 360, 0, new List<short>(), new PoisonsAndLevels(25, 3, 25, 3, 25, 3, 0, 0, 25, 3, 0, 0), new List<PresetInventoryItem>
		{
			new PresetInventoryItem("Material", 239, 20, 100),
			new PresetInventoryItem("Material", 240, 10, 100),
			new PresetInventoryItem("Material", 241, 5, 100),
			new PresetInventoryItem("Material", 242, 1, 100),
			new PresetInventoryItem("Material", 253, 20, 100),
			new PresetInventoryItem("Material", 254, 10, 100),
			new PresetInventoryItem("Material", 255, 5, 100),
			new PresetInventoryItem("Material", 256, 1, 100),
			new PresetInventoryItem("Material", 246, 20, 100),
			new PresetInventoryItem("Material", 247, 10, 100),
			new PresetInventoryItem("Material", 248, 5, 100),
			new PresetInventoryItem("Material", 249, 1, 100),
			new PresetInventoryItem("Material", 267, 20, 100),
			new PresetInventoryItem("Material", 268, 10, 100),
			new PresetInventoryItem("Material", 269, 5, 100),
			new PresetInventoryItem("Material", 270, 1, 100)
		}, 1, 1, 0, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, primaryRecoverAllInjuries: false, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, secondaryRecoverAllInjuries: false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 60, EMaterialFilterType.JiaoEgg, EMaterialFilterHardness.Invalid));
		_dataArray.Add(new MaterialItem(307, 614, 5, 506, 7, -1, "icon_Material_JiaoWBRYEgg", 615, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 0, 100, 10, 15, 0, 0, 50, 8, allowRandomCreate: false, 20, isSpecial: true, 5, 36, EMaterialProperty.Invalid, -1, -1, 500, 9, 360, 0, new List<short>(), new PoisonsAndLevels(25, 3, 0, 0, 25, 3, 25, 3, 25, 3, 0, 0), new List<PresetInventoryItem>
		{
			new PresetInventoryItem("Material", 239, 20, 100),
			new PresetInventoryItem("Material", 240, 10, 100),
			new PresetInventoryItem("Material", 241, 5, 100),
			new PresetInventoryItem("Material", 242, 1, 100),
			new PresetInventoryItem("Material", 253, 20, 100),
			new PresetInventoryItem("Material", 254, 10, 100),
			new PresetInventoryItem("Material", 255, 5, 100),
			new PresetInventoryItem("Material", 256, 1, 100),
			new PresetInventoryItem("Material", 260, 20, 100),
			new PresetInventoryItem("Material", 261, 10, 100),
			new PresetInventoryItem("Material", 262, 5, 100),
			new PresetInventoryItem("Material", 263, 1, 100),
			new PresetInventoryItem("Material", 267, 20, 100),
			new PresetInventoryItem("Material", 268, 10, 100),
			new PresetInventoryItem("Material", 269, 5, 100),
			new PresetInventoryItem("Material", 270, 1, 100)
		}, 1, 1, 0, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, primaryRecoverAllInjuries: false, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, secondaryRecoverAllInjuries: false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 60, EMaterialFilterType.JiaoEgg, EMaterialFilterHardness.Invalid));
		_dataArray.Add(new MaterialItem(308, 616, 5, 506, 7, -1, "icon_Material_JiaoWGRYEgg", 617, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 0, 100, 10, 15, 0, 0, 50, 8, allowRandomCreate: false, 20, isSpecial: true, 5, 36, EMaterialProperty.Invalid, -1, -1, 500, 9, 360, 0, new List<short>(), new PoisonsAndLevels(25, 3, 25, 3, 0, 0, 25, 3, 25, 3, 0, 0), new List<PresetInventoryItem>
		{
			new PresetInventoryItem("Material", 239, 20, 100),
			new PresetInventoryItem("Material", 240, 10, 100),
			new PresetInventoryItem("Material", 241, 5, 100),
			new PresetInventoryItem("Material", 242, 1, 100),
			new PresetInventoryItem("Material", 246, 20, 100),
			new PresetInventoryItem("Material", 247, 10, 100),
			new PresetInventoryItem("Material", 248, 5, 100),
			new PresetInventoryItem("Material", 249, 1, 100),
			new PresetInventoryItem("Material", 260, 20, 100),
			new PresetInventoryItem("Material", 261, 10, 100),
			new PresetInventoryItem("Material", 262, 5, 100),
			new PresetInventoryItem("Material", 263, 1, 100),
			new PresetInventoryItem("Material", 267, 20, 100),
			new PresetInventoryItem("Material", 268, 10, 100),
			new PresetInventoryItem("Material", 269, 5, 100),
			new PresetInventoryItem("Material", 270, 1, 100)
		}, 1, 1, 0, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, primaryRecoverAllInjuries: false, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, secondaryRecoverAllInjuries: false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 60, EMaterialFilterType.JiaoEgg, EMaterialFilterHardness.Invalid));
		_dataArray.Add(new MaterialItem(309, 618, 5, 506, 7, -1, "icon_Material_JiaoBGRYEgg", 619, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 0, 100, 10, 15, 0, 0, 50, 8, allowRandomCreate: false, 20, isSpecial: true, 5, 36, EMaterialProperty.Invalid, -1, -1, 500, 9, 360, 0, new List<short>(), new PoisonsAndLevels(0, 0, 25, 3, 25, 3, 25, 3, 25, 3, 0, 0), new List<PresetInventoryItem>
		{
			new PresetInventoryItem("Material", 253, 20, 100),
			new PresetInventoryItem("Material", 254, 10, 100),
			new PresetInventoryItem("Material", 255, 5, 100),
			new PresetInventoryItem("Material", 256, 1, 100),
			new PresetInventoryItem("Material", 246, 20, 100),
			new PresetInventoryItem("Material", 247, 10, 100),
			new PresetInventoryItem("Material", 248, 5, 100),
			new PresetInventoryItem("Material", 249, 1, 100),
			new PresetInventoryItem("Material", 260, 20, 100),
			new PresetInventoryItem("Material", 261, 10, 100),
			new PresetInventoryItem("Material", 262, 5, 100),
			new PresetInventoryItem("Material", 263, 1, 100),
			new PresetInventoryItem("Material", 267, 20, 100),
			new PresetInventoryItem("Material", 268, 10, 100),
			new PresetInventoryItem("Material", 269, 5, 100),
			new PresetInventoryItem("Material", 270, 1, 100)
		}, 1, 1, 0, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, primaryRecoverAllInjuries: false, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, secondaryRecoverAllInjuries: false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 60, EMaterialFilterType.JiaoEgg, EMaterialFilterHardness.Invalid));
		_dataArray.Add(new MaterialItem(310, 620, 5, 506, 8, -1, "icon_Material_JiaoWGRYBEgg", 621, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 0, 100, 10, 15, 0, 0, 50, 8, allowRandomCreate: false, 10, isSpecial: true, 5, 36, EMaterialProperty.Invalid, -1, -1, 600, 9, 450, 0, new List<short>(), new PoisonsAndLevels(25, 3, 25, 3, 25, 3, 25, 3, 25, 3, 0, 0), new List<PresetInventoryItem>
		{
			new PresetInventoryItem("Material", 240, 10, 100),
			new PresetInventoryItem("Material", 241, 5, 100),
			new PresetInventoryItem("Material", 242, 1, 100),
			new PresetInventoryItem("Material", 247, 10, 100),
			new PresetInventoryItem("Material", 248, 5, 100),
			new PresetInventoryItem("Material", 249, 1, 100),
			new PresetInventoryItem("Material", 254, 10, 100),
			new PresetInventoryItem("Material", 255, 5, 100),
			new PresetInventoryItem("Material", 256, 1, 100),
			new PresetInventoryItem("Material", 261, 10, 100),
			new PresetInventoryItem("Material", 262, 5, 100),
			new PresetInventoryItem("Material", 263, 1, 100),
			new PresetInventoryItem("Material", 268, 10, 100),
			new PresetInventoryItem("Material", 269, 5, 100),
			new PresetInventoryItem("Material", 270, 1, 100)
		}, 1, 1, 0, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, primaryRecoverAllInjuries: false, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, secondaryRecoverAllInjuries: false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 60, EMaterialFilterType.JiaoEgg, EMaterialFilterHardness.Invalid));
		_dataArray.Add(new MaterialItem(311, 622, 5, 506, 4, -1, "icon_Material_baiyoujiao", 623, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 0, 0, 10, 15, 0, 0, 50, 8, allowRandomCreate: false, 50, isSpecial: true, 5, 36, EMaterialProperty.Invalid, -1, -1, 400, 9, 150, 0, new List<short>(), new PoisonsAndLevels(30, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0), new List<PresetInventoryItem>
		{
			new PresetInventoryItem("Material", 236, 50, 100),
			new PresetInventoryItem("Material", 237, 40, 100),
			new PresetInventoryItem("Material", 238, 30, 100),
			new PresetInventoryItem("Material", 239, 20, 100),
			new PresetInventoryItem("Material", 240, 10, 100),
			new PresetInventoryItem("Material", 241, 5, 100),
			new PresetInventoryItem("Material", 242, 1, 100)
		}, 3, 1, 0, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, primaryRecoverAllInjuries: false, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, secondaryRecoverAllInjuries: false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 60, EMaterialFilterType.Jiao, EMaterialFilterHardness.Invalid));
		_dataArray.Add(new MaterialItem(312, 624, 5, 506, 4, -1, "icon_Material_baiyoujiao", 625, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 0, 0, 10, 15, 0, 0, 50, 8, allowRandomCreate: false, 50, isSpecial: true, 5, 36, EMaterialProperty.Invalid, -1, -1, 400, 9, 150, 0, new List<short>(), new PoisonsAndLevels(0, 0, 0, 0, 30, 2, 0, 0, 0, 0, 0, 0), new List<PresetInventoryItem>
		{
			new PresetInventoryItem("Material", 243, 50, 100),
			new PresetInventoryItem("Material", 244, 40, 100),
			new PresetInventoryItem("Material", 245, 30, 100),
			new PresetInventoryItem("Material", 246, 20, 100),
			new PresetInventoryItem("Material", 247, 10, 100),
			new PresetInventoryItem("Material", 248, 5, 100),
			new PresetInventoryItem("Material", 249, 1, 100)
		}, 3, 1, 0, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, primaryRecoverAllInjuries: false, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, secondaryRecoverAllInjuries: false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 60, EMaterialFilterType.Jiao, EMaterialFilterHardness.Invalid));
		_dataArray.Add(new MaterialItem(313, 626, 5, 506, 4, -1, "icon_Material_baiyoujiao", 627, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 0, 0, 10, 15, 0, 0, 50, 8, allowRandomCreate: false, 50, isSpecial: true, 5, 36, EMaterialProperty.Invalid, -1, -1, 400, 9, 150, 0, new List<short>(), new PoisonsAndLevels(0, 0, 30, 2, 0, 0, 0, 0, 0, 0, 0, 0), new List<PresetInventoryItem>
		{
			new PresetInventoryItem("Material", 250, 50, 100),
			new PresetInventoryItem("Material", 251, 40, 100),
			new PresetInventoryItem("Material", 252, 30, 100),
			new PresetInventoryItem("Material", 253, 20, 100),
			new PresetInventoryItem("Material", 254, 10, 100),
			new PresetInventoryItem("Material", 255, 5, 100),
			new PresetInventoryItem("Material", 256, 1, 100)
		}, 3, 1, 0, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, primaryRecoverAllInjuries: false, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, secondaryRecoverAllInjuries: false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 60, EMaterialFilterType.Jiao, EMaterialFilterHardness.Invalid));
		_dataArray.Add(new MaterialItem(314, 628, 5, 506, 4, -1, "icon_Material_baiyoujiao", 629, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 0, 0, 10, 15, 0, 0, 50, 8, allowRandomCreate: false, 50, isSpecial: true, 5, 36, EMaterialProperty.Invalid, -1, -1, 400, 9, 150, 0, new List<short>(), new PoisonsAndLevels(0, 0, 0, 0, 0, 0, 30, 2, 0, 0, 0, 0), new List<PresetInventoryItem>
		{
			new PresetInventoryItem("Material", 257, 50, 100),
			new PresetInventoryItem("Material", 258, 40, 100),
			new PresetInventoryItem("Material", 259, 30, 100),
			new PresetInventoryItem("Material", 260, 20, 100),
			new PresetInventoryItem("Material", 261, 10, 100),
			new PresetInventoryItem("Material", 262, 5, 100),
			new PresetInventoryItem("Material", 263, 1, 100)
		}, 3, 1, 0, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, primaryRecoverAllInjuries: false, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, secondaryRecoverAllInjuries: false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 60, EMaterialFilterType.Jiao, EMaterialFilterHardness.Invalid));
		_dataArray.Add(new MaterialItem(315, 630, 5, 506, 4, -1, "icon_Material_baiyoujiao", 631, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 0, 0, 10, 15, 0, 0, 50, 8, allowRandomCreate: false, 50, isSpecial: true, 5, 36, EMaterialProperty.Invalid, -1, -1, 400, 9, 150, 0, new List<short>(), new PoisonsAndLevels(0, 0, 0, 0, 0, 0, 0, 0, 30, 2, 0, 0), new List<PresetInventoryItem>
		{
			new PresetInventoryItem("Material", 264, 50, 100),
			new PresetInventoryItem("Material", 265, 40, 100),
			new PresetInventoryItem("Material", 266, 30, 100),
			new PresetInventoryItem("Material", 267, 20, 100),
			new PresetInventoryItem("Material", 268, 10, 100),
			new PresetInventoryItem("Material", 269, 5, 100),
			new PresetInventoryItem("Material", 270, 1, 100)
		}, 3, 1, 0, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, primaryRecoverAllInjuries: false, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, secondaryRecoverAllInjuries: false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 60, EMaterialFilterType.Jiao, EMaterialFilterHardness.Invalid));
		_dataArray.Add(new MaterialItem(316, 632, 5, 506, 5, -1, "icon_Material_baiheiyoujiao", 633, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 0, 0, 10, 15, 0, 0, 50, 8, allowRandomCreate: false, 40, isSpecial: true, 5, 36, EMaterialProperty.Invalid, -1, -1, 600, 9, 210, 0, new List<short>(), new PoisonsAndLevels(40, 2, 0, 0, 40, 2, 0, 0, 0, 0, 0, 0), new List<PresetInventoryItem>
		{
			new PresetInventoryItem("Material", 237, 40, 100),
			new PresetInventoryItem("Material", 238, 30, 100),
			new PresetInventoryItem("Material", 239, 20, 100),
			new PresetInventoryItem("Material", 240, 10, 100),
			new PresetInventoryItem("Material", 241, 5, 100),
			new PresetInventoryItem("Material", 242, 1, 100),
			new PresetInventoryItem("Material", 251, 40, 100),
			new PresetInventoryItem("Material", 252, 30, 100),
			new PresetInventoryItem("Material", 253, 20, 100),
			new PresetInventoryItem("Material", 254, 10, 100),
			new PresetInventoryItem("Material", 255, 5, 100),
			new PresetInventoryItem("Material", 256, 1, 100)
		}, 3, 1, 0, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, primaryRecoverAllInjuries: false, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, secondaryRecoverAllInjuries: false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 60, EMaterialFilterType.Jiao, EMaterialFilterHardness.Invalid));
		_dataArray.Add(new MaterialItem(317, 634, 5, 506, 5, -1, "icon_Material_baiheiyoujiao", 635, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 0, 0, 10, 15, 0, 0, 50, 8, allowRandomCreate: false, 40, isSpecial: true, 5, 36, EMaterialProperty.Invalid, -1, -1, 600, 9, 210, 0, new List<short>(), new PoisonsAndLevels(40, 2, 40, 2, 0, 0, 0, 0, 0, 0, 0, 0), new List<PresetInventoryItem>
		{
			new PresetInventoryItem("Material", 237, 40, 100),
			new PresetInventoryItem("Material", 238, 30, 100),
			new PresetInventoryItem("Material", 239, 20, 100),
			new PresetInventoryItem("Material", 240, 10, 100),
			new PresetInventoryItem("Material", 241, 5, 100),
			new PresetInventoryItem("Material", 242, 1, 100),
			new PresetInventoryItem("Material", 244, 40, 100),
			new PresetInventoryItem("Material", 245, 30, 100),
			new PresetInventoryItem("Material", 246, 20, 100),
			new PresetInventoryItem("Material", 247, 10, 100),
			new PresetInventoryItem("Material", 248, 5, 100),
			new PresetInventoryItem("Material", 249, 1, 100)
		}, 3, 1, 0, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, primaryRecoverAllInjuries: false, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, secondaryRecoverAllInjuries: false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 60, EMaterialFilterType.Jiao, EMaterialFilterHardness.Invalid));
		_dataArray.Add(new MaterialItem(318, 636, 5, 506, 5, -1, "icon_Material_baiheiyoujiao", 637, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 0, 0, 10, 15, 0, 0, 50, 8, allowRandomCreate: false, 40, isSpecial: true, 5, 36, EMaterialProperty.Invalid, -1, -1, 600, 9, 210, 0, new List<short>(), new PoisonsAndLevels(40, 2, 0, 0, 0, 0, 40, 2, 0, 0, 0, 0), new List<PresetInventoryItem>
		{
			new PresetInventoryItem("Material", 237, 40, 100),
			new PresetInventoryItem("Material", 238, 30, 100),
			new PresetInventoryItem("Material", 239, 20, 100),
			new PresetInventoryItem("Material", 240, 10, 100),
			new PresetInventoryItem("Material", 241, 5, 100),
			new PresetInventoryItem("Material", 242, 1, 100),
			new PresetInventoryItem("Material", 258, 40, 100),
			new PresetInventoryItem("Material", 259, 30, 100),
			new PresetInventoryItem("Material", 260, 20, 100),
			new PresetInventoryItem("Material", 261, 10, 100),
			new PresetInventoryItem("Material", 262, 5, 100),
			new PresetInventoryItem("Material", 263, 1, 100)
		}, 3, 1, 0, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, primaryRecoverAllInjuries: false, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, secondaryRecoverAllInjuries: false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 60, EMaterialFilterType.Jiao, EMaterialFilterHardness.Invalid));
		_dataArray.Add(new MaterialItem(319, 638, 5, 506, 5, -1, "icon_Material_baiheiyoujiao", 639, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 0, 0, 10, 15, 0, 0, 50, 8, allowRandomCreate: false, 40, isSpecial: true, 5, 36, EMaterialProperty.Invalid, -1, -1, 600, 9, 210, 0, new List<short>(), new PoisonsAndLevels(40, 2, 0, 0, 0, 0, 0, 0, 40, 2, 0, 0), new List<PresetInventoryItem>
		{
			new PresetInventoryItem("Material", 237, 40, 100),
			new PresetInventoryItem("Material", 238, 30, 100),
			new PresetInventoryItem("Material", 239, 20, 100),
			new PresetInventoryItem("Material", 240, 10, 100),
			new PresetInventoryItem("Material", 241, 5, 100),
			new PresetInventoryItem("Material", 242, 1, 100),
			new PresetInventoryItem("Material", 265, 40, 100),
			new PresetInventoryItem("Material", 266, 30, 100),
			new PresetInventoryItem("Material", 267, 20, 100),
			new PresetInventoryItem("Material", 268, 10, 100),
			new PresetInventoryItem("Material", 269, 5, 100),
			new PresetInventoryItem("Material", 270, 1, 100)
		}, 3, 1, 0, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, primaryRecoverAllInjuries: false, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, secondaryRecoverAllInjuries: false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 60, EMaterialFilterType.Jiao, EMaterialFilterHardness.Invalid));
		_dataArray.Add(new MaterialItem(320, 640, 5, 506, 5, -1, "icon_Material_baiheiyoujiao", 641, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 0, 0, 10, 15, 0, 0, 50, 8, allowRandomCreate: false, 40, isSpecial: true, 5, 36, EMaterialProperty.Invalid, -1, -1, 600, 9, 210, 0, new List<short>(), new PoisonsAndLevels(0, 0, 40, 2, 40, 2, 0, 0, 0, 0, 0, 0), new List<PresetInventoryItem>
		{
			new PresetInventoryItem("Material", 251, 40, 100),
			new PresetInventoryItem("Material", 252, 30, 100),
			new PresetInventoryItem("Material", 253, 20, 100),
			new PresetInventoryItem("Material", 254, 10, 100),
			new PresetInventoryItem("Material", 255, 5, 100),
			new PresetInventoryItem("Material", 256, 1, 100),
			new PresetInventoryItem("Material", 244, 40, 100),
			new PresetInventoryItem("Material", 245, 30, 100),
			new PresetInventoryItem("Material", 246, 20, 100),
			new PresetInventoryItem("Material", 247, 10, 100),
			new PresetInventoryItem("Material", 248, 5, 100),
			new PresetInventoryItem("Material", 249, 1, 100)
		}, 3, 1, 0, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, primaryRecoverAllInjuries: false, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, secondaryRecoverAllInjuries: false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 60, EMaterialFilterType.Jiao, EMaterialFilterHardness.Invalid));
		_dataArray.Add(new MaterialItem(321, 642, 5, 506, 5, -1, "icon_Material_baiheiyoujiao", 643, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 0, 0, 10, 15, 0, 0, 50, 8, allowRandomCreate: false, 40, isSpecial: true, 5, 36, EMaterialProperty.Invalid, -1, -1, 600, 9, 210, 0, new List<short>(), new PoisonsAndLevels(0, 0, 0, 0, 40, 2, 40, 2, 0, 0, 0, 0), new List<PresetInventoryItem>
		{
			new PresetInventoryItem("Material", 251, 40, 100),
			new PresetInventoryItem("Material", 252, 30, 100),
			new PresetInventoryItem("Material", 253, 20, 100),
			new PresetInventoryItem("Material", 254, 10, 100),
			new PresetInventoryItem("Material", 255, 5, 100),
			new PresetInventoryItem("Material", 256, 1, 100),
			new PresetInventoryItem("Material", 258, 40, 100),
			new PresetInventoryItem("Material", 259, 30, 100),
			new PresetInventoryItem("Material", 260, 20, 100),
			new PresetInventoryItem("Material", 261, 10, 100),
			new PresetInventoryItem("Material", 262, 5, 100),
			new PresetInventoryItem("Material", 263, 1, 100)
		}, 3, 1, 0, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, primaryRecoverAllInjuries: false, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, secondaryRecoverAllInjuries: false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 60, EMaterialFilterType.Jiao, EMaterialFilterHardness.Invalid));
		_dataArray.Add(new MaterialItem(322, 644, 5, 506, 5, -1, "icon_Material_baiheiyoujiao", 645, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 0, 0, 10, 15, 0, 0, 50, 8, allowRandomCreate: false, 40, isSpecial: true, 5, 36, EMaterialProperty.Invalid, -1, -1, 600, 9, 210, 0, new List<short>(), new PoisonsAndLevels(0, 0, 0, 0, 40, 2, 0, 0, 40, 2, 0, 0), new List<PresetInventoryItem>
		{
			new PresetInventoryItem("Material", 251, 40, 100),
			new PresetInventoryItem("Material", 252, 30, 100),
			new PresetInventoryItem("Material", 253, 20, 100),
			new PresetInventoryItem("Material", 254, 10, 100),
			new PresetInventoryItem("Material", 255, 5, 100),
			new PresetInventoryItem("Material", 256, 1, 100),
			new PresetInventoryItem("Material", 265, 40, 100),
			new PresetInventoryItem("Material", 266, 30, 100),
			new PresetInventoryItem("Material", 267, 20, 100),
			new PresetInventoryItem("Material", 268, 10, 100),
			new PresetInventoryItem("Material", 269, 5, 100),
			new PresetInventoryItem("Material", 270, 1, 100)
		}, 3, 1, 0, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, primaryRecoverAllInjuries: false, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, secondaryRecoverAllInjuries: false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 60, EMaterialFilterType.Jiao, EMaterialFilterHardness.Invalid));
		_dataArray.Add(new MaterialItem(323, 646, 5, 506, 5, -1, "icon_Material_baiheiyoujiao", 647, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 0, 0, 10, 15, 0, 0, 50, 8, allowRandomCreate: false, 40, isSpecial: true, 5, 36, EMaterialProperty.Invalid, -1, -1, 600, 9, 210, 0, new List<short>(), new PoisonsAndLevels(0, 0, 40, 2, 0, 0, 40, 2, 0, 0, 0, 0), new List<PresetInventoryItem>
		{
			new PresetInventoryItem("Material", 244, 40, 100),
			new PresetInventoryItem("Material", 245, 30, 100),
			new PresetInventoryItem("Material", 246, 20, 100),
			new PresetInventoryItem("Material", 247, 10, 100),
			new PresetInventoryItem("Material", 248, 5, 100),
			new PresetInventoryItem("Material", 249, 1, 100),
			new PresetInventoryItem("Material", 258, 40, 100),
			new PresetInventoryItem("Material", 259, 30, 100),
			new PresetInventoryItem("Material", 260, 20, 100),
			new PresetInventoryItem("Material", 261, 10, 100),
			new PresetInventoryItem("Material", 262, 5, 100),
			new PresetInventoryItem("Material", 263, 1, 100)
		}, 3, 1, 0, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, primaryRecoverAllInjuries: false, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, secondaryRecoverAllInjuries: false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 60, EMaterialFilterType.Jiao, EMaterialFilterHardness.Invalid));
		_dataArray.Add(new MaterialItem(324, 648, 5, 506, 5, -1, "icon_Material_baiheiyoujiao", 649, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 0, 0, 10, 15, 0, 0, 50, 8, allowRandomCreate: false, 40, isSpecial: true, 5, 36, EMaterialProperty.Invalid, -1, -1, 600, 9, 210, 0, new List<short>(), new PoisonsAndLevels(0, 0, 40, 2, 0, 0, 0, 0, 40, 2, 0, 0), new List<PresetInventoryItem>
		{
			new PresetInventoryItem("Material", 244, 40, 100),
			new PresetInventoryItem("Material", 245, 30, 100),
			new PresetInventoryItem("Material", 246, 20, 100),
			new PresetInventoryItem("Material", 247, 10, 100),
			new PresetInventoryItem("Material", 248, 5, 100),
			new PresetInventoryItem("Material", 249, 1, 100),
			new PresetInventoryItem("Material", 265, 40, 100),
			new PresetInventoryItem("Material", 266, 30, 100),
			new PresetInventoryItem("Material", 267, 20, 100),
			new PresetInventoryItem("Material", 268, 10, 100),
			new PresetInventoryItem("Material", 269, 5, 100),
			new PresetInventoryItem("Material", 270, 1, 100)
		}, 3, 1, 0, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, primaryRecoverAllInjuries: false, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, secondaryRecoverAllInjuries: false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 60, EMaterialFilterType.Jiao, EMaterialFilterHardness.Invalid));
		_dataArray.Add(new MaterialItem(325, 650, 5, 506, 5, -1, "icon_Material_baiheiyoujiao", 651, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 0, 0, 10, 15, 0, 0, 50, 8, allowRandomCreate: false, 40, isSpecial: true, 5, 36, EMaterialProperty.Invalid, -1, -1, 600, 9, 210, 0, new List<short>(), new PoisonsAndLevels(0, 0, 0, 0, 0, 0, 40, 2, 40, 2, 0, 0), new List<PresetInventoryItem>
		{
			new PresetInventoryItem("Material", 258, 40, 100),
			new PresetInventoryItem("Material", 259, 30, 100),
			new PresetInventoryItem("Material", 260, 20, 100),
			new PresetInventoryItem("Material", 261, 10, 100),
			new PresetInventoryItem("Material", 262, 5, 100),
			new PresetInventoryItem("Material", 263, 1, 100),
			new PresetInventoryItem("Material", 265, 40, 100),
			new PresetInventoryItem("Material", 266, 30, 100),
			new PresetInventoryItem("Material", 267, 20, 100),
			new PresetInventoryItem("Material", 268, 10, 100),
			new PresetInventoryItem("Material", 269, 5, 100),
			new PresetInventoryItem("Material", 270, 1, 100)
		}, 3, 1, 0, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, primaryRecoverAllInjuries: false, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, secondaryRecoverAllInjuries: false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 60, EMaterialFilterType.Jiao, EMaterialFilterHardness.Invalid));
		_dataArray.Add(new MaterialItem(326, 652, 5, 506, 6, -1, "icon_Material_baiheiqingyoujiao", 653, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 0, 0, 10, 15, 0, 0, 50, 8, allowRandomCreate: false, 30, isSpecial: true, 5, 36, EMaterialProperty.Invalid, -1, -1, 800, 9, 280, 0, new List<short>(), new PoisonsAndLevels(50, 2, 50, 2, 50, 2, 0, 0, 0, 0, 0, 0), new List<PresetInventoryItem>
		{
			new PresetInventoryItem("Material", 238, 30, 100),
			new PresetInventoryItem("Material", 239, 20, 100),
			new PresetInventoryItem("Material", 240, 10, 100),
			new PresetInventoryItem("Material", 241, 5, 100),
			new PresetInventoryItem("Material", 242, 1, 100),
			new PresetInventoryItem("Material", 252, 30, 100),
			new PresetInventoryItem("Material", 253, 20, 100),
			new PresetInventoryItem("Material", 254, 10, 100),
			new PresetInventoryItem("Material", 255, 5, 100),
			new PresetInventoryItem("Material", 256, 1, 100),
			new PresetInventoryItem("Material", 245, 30, 100),
			new PresetInventoryItem("Material", 246, 20, 100),
			new PresetInventoryItem("Material", 247, 10, 100),
			new PresetInventoryItem("Material", 248, 5, 100),
			new PresetInventoryItem("Material", 249, 1, 100)
		}, 3, 1, 0, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, primaryRecoverAllInjuries: false, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, secondaryRecoverAllInjuries: false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 60, EMaterialFilterType.Jiao, EMaterialFilterHardness.Invalid));
		_dataArray.Add(new MaterialItem(327, 654, 5, 506, 6, -1, "icon_Material_baiheiqingyoujiao", 655, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 0, 0, 10, 15, 0, 0, 50, 8, allowRandomCreate: false, 30, isSpecial: true, 5, 36, EMaterialProperty.Invalid, -1, -1, 800, 9, 280, 0, new List<short>(), new PoisonsAndLevels(50, 2, 0, 0, 50, 2, 50, 2, 0, 0, 0, 0), new List<PresetInventoryItem>
		{
			new PresetInventoryItem("Material", 238, 30, 100),
			new PresetInventoryItem("Material", 239, 20, 100),
			new PresetInventoryItem("Material", 240, 10, 100),
			new PresetInventoryItem("Material", 241, 5, 100),
			new PresetInventoryItem("Material", 242, 1, 100),
			new PresetInventoryItem("Material", 252, 30, 100),
			new PresetInventoryItem("Material", 253, 20, 100),
			new PresetInventoryItem("Material", 254, 10, 100),
			new PresetInventoryItem("Material", 255, 5, 100),
			new PresetInventoryItem("Material", 256, 1, 100),
			new PresetInventoryItem("Material", 259, 30, 100),
			new PresetInventoryItem("Material", 260, 20, 100),
			new PresetInventoryItem("Material", 261, 10, 100),
			new PresetInventoryItem("Material", 262, 5, 100),
			new PresetInventoryItem("Material", 263, 1, 100)
		}, 3, 1, 0, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, primaryRecoverAllInjuries: false, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, secondaryRecoverAllInjuries: false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 60, EMaterialFilterType.Jiao, EMaterialFilterHardness.Invalid));
		_dataArray.Add(new MaterialItem(328, 656, 5, 506, 6, -1, "icon_Material_baiheiqingyoujiao", 657, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 0, 0, 10, 15, 0, 0, 50, 8, allowRandomCreate: false, 30, isSpecial: true, 5, 36, EMaterialProperty.Invalid, -1, -1, 800, 9, 280, 0, new List<short>(), new PoisonsAndLevels(50, 2, 0, 0, 50, 2, 0, 0, 50, 2, 0, 0), new List<PresetInventoryItem>
		{
			new PresetInventoryItem("Material", 238, 30, 100),
			new PresetInventoryItem("Material", 239, 20, 100),
			new PresetInventoryItem("Material", 240, 10, 100),
			new PresetInventoryItem("Material", 241, 5, 100),
			new PresetInventoryItem("Material", 242, 1, 100),
			new PresetInventoryItem("Material", 252, 30, 100),
			new PresetInventoryItem("Material", 253, 20, 100),
			new PresetInventoryItem("Material", 254, 10, 100),
			new PresetInventoryItem("Material", 255, 5, 100),
			new PresetInventoryItem("Material", 256, 1, 100),
			new PresetInventoryItem("Material", 266, 30, 100),
			new PresetInventoryItem("Material", 267, 20, 100),
			new PresetInventoryItem("Material", 268, 10, 100),
			new PresetInventoryItem("Material", 269, 5, 100),
			new PresetInventoryItem("Material", 270, 1, 100)
		}, 3, 1, 0, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, primaryRecoverAllInjuries: false, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, secondaryRecoverAllInjuries: false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 60, EMaterialFilterType.Jiao, EMaterialFilterHardness.Invalid));
		_dataArray.Add(new MaterialItem(329, 658, 5, 506, 6, -1, "icon_Material_baiheiqingyoujiao", 659, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 0, 0, 10, 15, 0, 0, 50, 8, allowRandomCreate: false, 30, isSpecial: true, 5, 36, EMaterialProperty.Invalid, -1, -1, 800, 9, 280, 0, new List<short>(), new PoisonsAndLevels(50, 2, 50, 2, 0, 0, 50, 2, 0, 0, 0, 0), new List<PresetInventoryItem>
		{
			new PresetInventoryItem("Material", 238, 30, 100),
			new PresetInventoryItem("Material", 239, 20, 100),
			new PresetInventoryItem("Material", 240, 10, 100),
			new PresetInventoryItem("Material", 241, 5, 100),
			new PresetInventoryItem("Material", 242, 1, 100),
			new PresetInventoryItem("Material", 245, 30, 100),
			new PresetInventoryItem("Material", 246, 20, 100),
			new PresetInventoryItem("Material", 247, 10, 100),
			new PresetInventoryItem("Material", 248, 5, 100),
			new PresetInventoryItem("Material", 249, 1, 100),
			new PresetInventoryItem("Material", 259, 30, 100),
			new PresetInventoryItem("Material", 260, 20, 100),
			new PresetInventoryItem("Material", 261, 10, 100),
			new PresetInventoryItem("Material", 262, 5, 100),
			new PresetInventoryItem("Material", 263, 1, 100)
		}, 3, 1, 0, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, primaryRecoverAllInjuries: false, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, secondaryRecoverAllInjuries: false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 60, EMaterialFilterType.Jiao, EMaterialFilterHardness.Invalid));
		_dataArray.Add(new MaterialItem(330, 660, 5, 506, 6, -1, "icon_Material_baiheiqingyoujiao", 661, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 0, 0, 10, 15, 0, 0, 50, 8, allowRandomCreate: false, 30, isSpecial: true, 5, 36, EMaterialProperty.Invalid, -1, -1, 800, 9, 280, 0, new List<short>(), new PoisonsAndLevels(50, 2, 50, 2, 0, 0, 0, 0, 50, 2, 0, 0), new List<PresetInventoryItem>
		{
			new PresetInventoryItem("Material", 238, 30, 100),
			new PresetInventoryItem("Material", 239, 20, 100),
			new PresetInventoryItem("Material", 240, 10, 100),
			new PresetInventoryItem("Material", 241, 5, 100),
			new PresetInventoryItem("Material", 242, 1, 100),
			new PresetInventoryItem("Material", 245, 30, 100),
			new PresetInventoryItem("Material", 246, 20, 100),
			new PresetInventoryItem("Material", 247, 10, 100),
			new PresetInventoryItem("Material", 248, 5, 100),
			new PresetInventoryItem("Material", 249, 1, 100),
			new PresetInventoryItem("Material", 266, 30, 100),
			new PresetInventoryItem("Material", 267, 20, 100),
			new PresetInventoryItem("Material", 268, 10, 100),
			new PresetInventoryItem("Material", 269, 5, 100),
			new PresetInventoryItem("Material", 270, 1, 100)
		}, 3, 1, 0, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, primaryRecoverAllInjuries: false, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, secondaryRecoverAllInjuries: false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 60, EMaterialFilterType.Jiao, EMaterialFilterHardness.Invalid));
		_dataArray.Add(new MaterialItem(331, 662, 5, 506, 6, -1, "icon_Material_baiheiqingyoujiao", 663, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 0, 0, 10, 15, 0, 0, 50, 8, allowRandomCreate: false, 30, isSpecial: true, 5, 36, EMaterialProperty.Invalid, -1, -1, 800, 9, 280, 0, new List<short>(), new PoisonsAndLevels(50, 2, 0, 0, 0, 0, 50, 2, 50, 2, 0, 0), new List<PresetInventoryItem>
		{
			new PresetInventoryItem("Material", 238, 30, 100),
			new PresetInventoryItem("Material", 239, 20, 100),
			new PresetInventoryItem("Material", 240, 10, 100),
			new PresetInventoryItem("Material", 241, 5, 100),
			new PresetInventoryItem("Material", 242, 1, 100),
			new PresetInventoryItem("Material", 259, 30, 100),
			new PresetInventoryItem("Material", 260, 20, 100),
			new PresetInventoryItem("Material", 261, 10, 100),
			new PresetInventoryItem("Material", 262, 5, 100),
			new PresetInventoryItem("Material", 263, 1, 100),
			new PresetInventoryItem("Material", 266, 30, 100),
			new PresetInventoryItem("Material", 267, 20, 100),
			new PresetInventoryItem("Material", 268, 10, 100),
			new PresetInventoryItem("Material", 269, 5, 100),
			new PresetInventoryItem("Material", 270, 1, 100)
		}, 3, 1, 0, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, primaryRecoverAllInjuries: false, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, secondaryRecoverAllInjuries: false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 60, EMaterialFilterType.Jiao, EMaterialFilterHardness.Invalid));
		_dataArray.Add(new MaterialItem(332, 664, 5, 506, 6, -1, "icon_Material_baiheiqingyoujiao", 665, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 0, 0, 10, 15, 0, 0, 50, 8, allowRandomCreate: false, 30, isSpecial: true, 5, 36, EMaterialProperty.Invalid, -1, -1, 800, 9, 280, 0, new List<short>(), new PoisonsAndLevels(0, 0, 50, 2, 50, 2, 50, 2, 0, 0, 0, 0), new List<PresetInventoryItem>
		{
			new PresetInventoryItem("Material", 252, 30, 100),
			new PresetInventoryItem("Material", 253, 20, 100),
			new PresetInventoryItem("Material", 254, 10, 100),
			new PresetInventoryItem("Material", 255, 5, 100),
			new PresetInventoryItem("Material", 256, 1, 100),
			new PresetInventoryItem("Material", 245, 30, 100),
			new PresetInventoryItem("Material", 246, 20, 100),
			new PresetInventoryItem("Material", 247, 10, 100),
			new PresetInventoryItem("Material", 248, 5, 100),
			new PresetInventoryItem("Material", 249, 1, 100),
			new PresetInventoryItem("Material", 259, 30, 100),
			new PresetInventoryItem("Material", 260, 20, 100),
			new PresetInventoryItem("Material", 261, 10, 100),
			new PresetInventoryItem("Material", 262, 5, 100),
			new PresetInventoryItem("Material", 263, 1, 100)
		}, 3, 1, 0, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, primaryRecoverAllInjuries: false, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, secondaryRecoverAllInjuries: false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 60, EMaterialFilterType.Jiao, EMaterialFilterHardness.Invalid));
		_dataArray.Add(new MaterialItem(333, 666, 5, 506, 6, -1, "icon_Material_baiheiqingyoujiao", 667, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 0, 0, 10, 15, 0, 0, 50, 8, allowRandomCreate: false, 30, isSpecial: true, 5, 36, EMaterialProperty.Invalid, -1, -1, 800, 9, 280, 0, new List<short>(), new PoisonsAndLevels(0, 0, 50, 2, 50, 2, 0, 0, 50, 2, 0, 0), new List<PresetInventoryItem>
		{
			new PresetInventoryItem("Material", 252, 30, 100),
			new PresetInventoryItem("Material", 253, 20, 100),
			new PresetInventoryItem("Material", 254, 10, 100),
			new PresetInventoryItem("Material", 255, 5, 100),
			new PresetInventoryItem("Material", 256, 1, 100),
			new PresetInventoryItem("Material", 245, 30, 100),
			new PresetInventoryItem("Material", 246, 20, 100),
			new PresetInventoryItem("Material", 247, 10, 100),
			new PresetInventoryItem("Material", 248, 5, 100),
			new PresetInventoryItem("Material", 249, 1, 100),
			new PresetInventoryItem("Material", 266, 30, 100),
			new PresetInventoryItem("Material", 267, 20, 100),
			new PresetInventoryItem("Material", 268, 10, 100),
			new PresetInventoryItem("Material", 269, 5, 100),
			new PresetInventoryItem("Material", 270, 1, 100)
		}, 3, 1, 0, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, primaryRecoverAllInjuries: false, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, secondaryRecoverAllInjuries: false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 60, EMaterialFilterType.Jiao, EMaterialFilterHardness.Invalid));
		_dataArray.Add(new MaterialItem(334, 668, 5, 506, 6, -1, "icon_Material_baiheiqingyoujiao", 669, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 0, 0, 10, 15, 0, 0, 50, 8, allowRandomCreate: false, 30, isSpecial: true, 5, 36, EMaterialProperty.Invalid, -1, -1, 800, 9, 280, 0, new List<short>(), new PoisonsAndLevels(0, 0, 0, 0, 50, 2, 50, 2, 50, 2, 0, 0), new List<PresetInventoryItem>
		{
			new PresetInventoryItem("Material", 252, 30, 100),
			new PresetInventoryItem("Material", 253, 20, 100),
			new PresetInventoryItem("Material", 254, 10, 100),
			new PresetInventoryItem("Material", 255, 5, 100),
			new PresetInventoryItem("Material", 256, 1, 100),
			new PresetInventoryItem("Material", 259, 30, 100),
			new PresetInventoryItem("Material", 260, 20, 100),
			new PresetInventoryItem("Material", 261, 10, 100),
			new PresetInventoryItem("Material", 262, 5, 100),
			new PresetInventoryItem("Material", 263, 1, 100),
			new PresetInventoryItem("Material", 266, 30, 100),
			new PresetInventoryItem("Material", 267, 20, 100),
			new PresetInventoryItem("Material", 268, 10, 100),
			new PresetInventoryItem("Material", 269, 5, 100),
			new PresetInventoryItem("Material", 270, 1, 100)
		}, 3, 1, 0, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, primaryRecoverAllInjuries: false, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, secondaryRecoverAllInjuries: false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 60, EMaterialFilterType.Jiao, EMaterialFilterHardness.Invalid));
		_dataArray.Add(new MaterialItem(335, 670, 5, 506, 6, -1, "icon_Material_baiheiqingyoujiao", 671, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 0, 0, 10, 15, 0, 0, 50, 8, allowRandomCreate: false, 30, isSpecial: true, 5, 36, EMaterialProperty.Invalid, -1, -1, 800, 9, 280, 0, new List<short>(), new PoisonsAndLevels(0, 0, 50, 2, 0, 0, 50, 2, 50, 2, 0, 0), new List<PresetInventoryItem>
		{
			new PresetInventoryItem("Material", 245, 30, 100),
			new PresetInventoryItem("Material", 246, 20, 100),
			new PresetInventoryItem("Material", 247, 10, 100),
			new PresetInventoryItem("Material", 248, 5, 100),
			new PresetInventoryItem("Material", 249, 1, 100),
			new PresetInventoryItem("Material", 259, 30, 100),
			new PresetInventoryItem("Material", 260, 20, 100),
			new PresetInventoryItem("Material", 261, 10, 100),
			new PresetInventoryItem("Material", 262, 5, 100),
			new PresetInventoryItem("Material", 263, 1, 100),
			new PresetInventoryItem("Material", 266, 30, 100),
			new PresetInventoryItem("Material", 267, 20, 100),
			new PresetInventoryItem("Material", 268, 10, 100),
			new PresetInventoryItem("Material", 269, 5, 100),
			new PresetInventoryItem("Material", 270, 1, 100)
		}, 3, 1, 0, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, primaryRecoverAllInjuries: false, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, secondaryRecoverAllInjuries: false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 60, EMaterialFilterType.Jiao, EMaterialFilterHardness.Invalid));
		_dataArray.Add(new MaterialItem(336, 672, 5, 506, 7, -1, "icon_Material_baiheiqingchiyoujiao", 673, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 0, 0, 10, 15, 0, 0, 50, 8, allowRandomCreate: false, 20, isSpecial: true, 5, 36, EMaterialProperty.Invalid, -1, -1, 1000, 9, 360, 0, new List<short>(), new PoisonsAndLevels(50, 3, 50, 3, 50, 3, 50, 3, 0, 0, 0, 0), new List<PresetInventoryItem>
		{
			new PresetInventoryItem("Material", 239, 20, 100),
			new PresetInventoryItem("Material", 240, 10, 100),
			new PresetInventoryItem("Material", 241, 5, 100),
			new PresetInventoryItem("Material", 242, 1, 100),
			new PresetInventoryItem("Material", 253, 20, 100),
			new PresetInventoryItem("Material", 254, 10, 100),
			new PresetInventoryItem("Material", 255, 5, 100),
			new PresetInventoryItem("Material", 256, 1, 100),
			new PresetInventoryItem("Material", 246, 20, 100),
			new PresetInventoryItem("Material", 247, 10, 100),
			new PresetInventoryItem("Material", 248, 5, 100),
			new PresetInventoryItem("Material", 249, 1, 100),
			new PresetInventoryItem("Material", 260, 20, 100),
			new PresetInventoryItem("Material", 261, 10, 100),
			new PresetInventoryItem("Material", 262, 5, 100),
			new PresetInventoryItem("Material", 263, 1, 100)
		}, 3, 1, 0, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, primaryRecoverAllInjuries: false, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, secondaryRecoverAllInjuries: false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 60, EMaterialFilterType.Jiao, EMaterialFilterHardness.Invalid));
		_dataArray.Add(new MaterialItem(337, 674, 5, 506, 7, -1, "icon_Material_baiheiqingchiyoujiao", 675, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 0, 0, 10, 15, 0, 0, 50, 8, allowRandomCreate: false, 20, isSpecial: true, 5, 36, EMaterialProperty.Invalid, -1, -1, 1000, 9, 360, 0, new List<short>(), new PoisonsAndLevels(50, 3, 50, 3, 50, 3, 0, 0, 50, 3, 0, 0), new List<PresetInventoryItem>
		{
			new PresetInventoryItem("Material", 239, 20, 100),
			new PresetInventoryItem("Material", 240, 10, 100),
			new PresetInventoryItem("Material", 241, 5, 100),
			new PresetInventoryItem("Material", 242, 1, 100),
			new PresetInventoryItem("Material", 253, 20, 100),
			new PresetInventoryItem("Material", 254, 10, 100),
			new PresetInventoryItem("Material", 255, 5, 100),
			new PresetInventoryItem("Material", 256, 1, 100),
			new PresetInventoryItem("Material", 246, 20, 100),
			new PresetInventoryItem("Material", 247, 10, 100),
			new PresetInventoryItem("Material", 248, 5, 100),
			new PresetInventoryItem("Material", 249, 1, 100),
			new PresetInventoryItem("Material", 267, 20, 100),
			new PresetInventoryItem("Material", 268, 10, 100),
			new PresetInventoryItem("Material", 269, 5, 100),
			new PresetInventoryItem("Material", 270, 1, 100)
		}, 3, 1, 0, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, primaryRecoverAllInjuries: false, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, secondaryRecoverAllInjuries: false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 60, EMaterialFilterType.Jiao, EMaterialFilterHardness.Invalid));
		_dataArray.Add(new MaterialItem(338, 676, 5, 506, 7, -1, "icon_Material_baiheiqingchiyoujiao", 677, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 0, 0, 10, 15, 0, 0, 50, 8, allowRandomCreate: false, 20, isSpecial: true, 5, 36, EMaterialProperty.Invalid, -1, -1, 1000, 9, 360, 0, new List<short>(), new PoisonsAndLevels(50, 3, 0, 0, 50, 3, 50, 3, 50, 3, 0, 0), new List<PresetInventoryItem>
		{
			new PresetInventoryItem("Material", 239, 20, 100),
			new PresetInventoryItem("Material", 240, 10, 100),
			new PresetInventoryItem("Material", 241, 5, 100),
			new PresetInventoryItem("Material", 242, 1, 100),
			new PresetInventoryItem("Material", 253, 20, 100),
			new PresetInventoryItem("Material", 254, 10, 100),
			new PresetInventoryItem("Material", 255, 5, 100),
			new PresetInventoryItem("Material", 256, 1, 100),
			new PresetInventoryItem("Material", 260, 20, 100),
			new PresetInventoryItem("Material", 261, 10, 100),
			new PresetInventoryItem("Material", 262, 5, 100),
			new PresetInventoryItem("Material", 263, 1, 100),
			new PresetInventoryItem("Material", 267, 20, 100),
			new PresetInventoryItem("Material", 268, 10, 100),
			new PresetInventoryItem("Material", 269, 5, 100),
			new PresetInventoryItem("Material", 270, 1, 100)
		}, 3, 1, 0, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, primaryRecoverAllInjuries: false, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, secondaryRecoverAllInjuries: false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 60, EMaterialFilterType.Jiao, EMaterialFilterHardness.Invalid));
		_dataArray.Add(new MaterialItem(339, 678, 5, 506, 7, -1, "icon_Material_baiheiqingchiyoujiao", 679, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 0, 0, 10, 15, 0, 0, 50, 8, allowRandomCreate: false, 20, isSpecial: true, 5, 36, EMaterialProperty.Invalid, -1, -1, 1000, 9, 360, 0, new List<short>(), new PoisonsAndLevels(50, 3, 50, 3, 0, 0, 50, 3, 50, 3, 0, 0), new List<PresetInventoryItem>
		{
			new PresetInventoryItem("Material", 239, 20, 100),
			new PresetInventoryItem("Material", 240, 10, 100),
			new PresetInventoryItem("Material", 241, 5, 100),
			new PresetInventoryItem("Material", 242, 1, 100),
			new PresetInventoryItem("Material", 246, 20, 100),
			new PresetInventoryItem("Material", 247, 10, 100),
			new PresetInventoryItem("Material", 248, 5, 100),
			new PresetInventoryItem("Material", 249, 1, 100),
			new PresetInventoryItem("Material", 260, 20, 100),
			new PresetInventoryItem("Material", 261, 10, 100),
			new PresetInventoryItem("Material", 262, 5, 100),
			new PresetInventoryItem("Material", 263, 1, 100),
			new PresetInventoryItem("Material", 267, 20, 100),
			new PresetInventoryItem("Material", 268, 10, 100),
			new PresetInventoryItem("Material", 269, 5, 100),
			new PresetInventoryItem("Material", 270, 1, 100)
		}, 3, 1, 0, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, primaryRecoverAllInjuries: false, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, secondaryRecoverAllInjuries: false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 60, EMaterialFilterType.Jiao, EMaterialFilterHardness.Invalid));
		_dataArray.Add(new MaterialItem(340, 680, 5, 506, 7, -1, "icon_Material_baiheiqingchiyoujiao", 681, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 0, 0, 10, 15, 0, 0, 50, 8, allowRandomCreate: false, 20, isSpecial: true, 5, 36, EMaterialProperty.Invalid, -1, -1, 1000, 9, 360, 0, new List<short>(), new PoisonsAndLevels(0, 0, 50, 3, 50, 3, 50, 3, 50, 3, 0, 0), new List<PresetInventoryItem>
		{
			new PresetInventoryItem("Material", 253, 20, 100),
			new PresetInventoryItem("Material", 254, 10, 100),
			new PresetInventoryItem("Material", 255, 5, 100),
			new PresetInventoryItem("Material", 256, 1, 100),
			new PresetInventoryItem("Material", 246, 20, 100),
			new PresetInventoryItem("Material", 247, 10, 100),
			new PresetInventoryItem("Material", 248, 5, 100),
			new PresetInventoryItem("Material", 249, 1, 100),
			new PresetInventoryItem("Material", 260, 20, 100),
			new PresetInventoryItem("Material", 261, 10, 100),
			new PresetInventoryItem("Material", 262, 5, 100),
			new PresetInventoryItem("Material", 263, 1, 100),
			new PresetInventoryItem("Material", 267, 20, 100),
			new PresetInventoryItem("Material", 268, 10, 100),
			new PresetInventoryItem("Material", 269, 5, 100),
			new PresetInventoryItem("Material", 270, 1, 100)
		}, 3, 1, 0, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, primaryRecoverAllInjuries: false, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, secondaryRecoverAllInjuries: false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 60, EMaterialFilterType.Jiao, EMaterialFilterHardness.Invalid));
		_dataArray.Add(new MaterialItem(341, 682, 5, 506, 8, -1, "icon_Material_baiqingchihuangheiyoujiao", 683, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 0, 0, 10, 15, 0, 0, 50, 8, allowRandomCreate: false, 10, isSpecial: true, 5, 36, EMaterialProperty.Invalid, -1, -1, 1200, 9, 450, 0, new List<short>(), new PoisonsAndLevels(50, 3, 50, 3, 50, 3, 50, 3, 50, 3, 0, 0), new List<PresetInventoryItem>
		{
			new PresetInventoryItem("Material", 240, 10, 100),
			new PresetInventoryItem("Material", 241, 5, 100),
			new PresetInventoryItem("Material", 242, 1, 100),
			new PresetInventoryItem("Material", 247, 10, 100),
			new PresetInventoryItem("Material", 248, 5, 100),
			new PresetInventoryItem("Material", 249, 1, 100),
			new PresetInventoryItem("Material", 254, 10, 100),
			new PresetInventoryItem("Material", 255, 5, 100),
			new PresetInventoryItem("Material", 256, 1, 100),
			new PresetInventoryItem("Material", 261, 10, 100),
			new PresetInventoryItem("Material", 262, 5, 100),
			new PresetInventoryItem("Material", 263, 1, 100),
			new PresetInventoryItem("Material", 268, 10, 100),
			new PresetInventoryItem("Material", 269, 5, 100),
			new PresetInventoryItem("Material", 270, 1, 100)
		}, 3, 1, 0, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, primaryRecoverAllInjuries: false, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, secondaryRecoverAllInjuries: false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 60, EMaterialFilterType.Jiao, EMaterialFilterHardness.Invalid));
		_dataArray.Add(new MaterialItem(342, 684, 5, 504, 7, -1, "icon_Material_bainiaocaiyu", 685, transferable: false, stackable: false, wagerable: false, refinable: false, poisonable: false, repairable: false, inheritable: true, -1, 0, 10, 15, 0, 0, 50, 8, allowRandomCreate: false, 0, isSpecial: true, 4, -1, EMaterialProperty.Invalid, -1, -1, 0, 10, 30, 30, new List<short> { 182 }, new PoisonsAndLevels(default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short)), new List<PresetInventoryItem>(), 0, 1, 0, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, primaryRecoverAllInjuries: false, EMedicineEffectType.Invalid, EMedicineEffectSubType.Invalid, 0, 0, 0, secondaryRecoverAllInjuries: false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 60, EMaterialFilterType.Invalid, EMaterialFilterHardness.Invalid));
	}

	public override void Init()
	{
		base.Init();
		_dataArray = new List<MaterialItem>(343);
		CreateItems0();
		CreateItems1();
		CreateItems2();
		CreateItems3();
		CreateItems4();
		CreateItems5();
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
