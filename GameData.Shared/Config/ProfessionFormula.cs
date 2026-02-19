using System;
using System.Collections.Generic;
using Config.Common;

namespace Config;

[Serializable]
public class ProfessionFormula : ConfigData<ProfessionFormulaItem, int>
{
	public static class DefKey
	{
		public const int AddSenioritySavage1 = 0;

		public const int AddSenioritySavage2 = 1;

		public const int AddSenioritySavage3 = 2;

		public const int AddSenioritySavage4 = 3;

		public const int AddSenioritySavage5 = 4;

		public const int AddSenioritySavage6 = 5;

		public const int AddSenioritySavage7 = 6;

		public const int AddSenioritySavage8 = 7;

		public const int AddSeniorityHunter1 = 8;

		public const int AddSeniorityHunter2 = 9;

		public const int AddSeniorityHunter3 = 10;

		public const int AddSeniorityHunter4 = 11;

		public const int AddSeniorityHunter5 = 12;

		public const int AddSeniorityHunter6 = 13;

		public const int AddSeniorityHunter7 = 14;

		public const int AddSeniorityHunter8 = 15;

		public const int AddSeniorityCraft1 = 16;

		public const int AddSeniorityCraft2 = 17;

		public const int AddSeniorityCraft3 = 18;

		public const int AddSeniorityCraft4 = 19;

		public const int AddSeniorityCraft5 = 20;

		public const int AddSeniorityCraft61 = 21;

		public const int AddSeniorityCraft62 = 22;

		public const int AddSeniorityMartialArtist1 = 23;

		public const int AddSeniorityMartialArtist2 = 24;

		public const int AddSeniorityMartialArtist3 = 25;

		public const int AddSeniorityMartialArtist4 = 26;

		public const int AddSeniorityMartialArtist5 = 27;

		public const int AddSeniorityMartialArtist6 = 28;

		public const int AddSeniorityMartialArtist7 = 29;

		public const int AddSeniorityMartialArtist8 = 113;

		public const int AddSeniorityLiterati1 = 30;

		public const int AddSeniorityLiterati2 = 31;

		public const int AddSeniorityLiterati3 = 32;

		public const int AddSeniorityLiterati4 = 33;

		public const int AddSeniorityLiterati5 = 34;

		public const int AddSeniorityLiterati6 = 35;

		public const int AddSeniorityTaoistMonk1 = 36;

		public const int AddSeniorityTaoistMonk2 = 37;

		public const int AddSeniorityTaoistMonk3 = 38;

		public const int AddSeniorityTaoistMonk4 = 39;

		public const int AddSeniorityTaoistMonk5 = 40;

		public const int AddSeniorityTaoistMonk6 = 41;

		public const int AddSeniorityBuddhistMonk1 = 42;

		public const int AddSeniorityBuddhistMonk2 = 43;

		public const int AddSeniorityBuddhistMonk3 = 44;

		public const int AddSeniorityBuddhistMonk4 = 45;

		public const int AddSeniorityBuddhistMonk5 = 46;

		public const int AddSeniorityBuddhistMonk6 = 47;

		public const int AddSeniorityWineTaster1 = 48;

		public const int AddSeniorityWineTaster2 = 49;

		public const int AddSeniorityWineTaster3 = 50;

		public const int AddSeniorityWineTaster4 = 51;

		public const int AddSeniorityWineTaster5 = 52;

		public const int AddSeniorityWineTaster6 = 53;

		public const int AddSeniorityWineTaster7 = 54;

		public const int AddSeniorityAristocrat1 = 55;

		public const int AddSeniorityAristocrat2 = 56;

		public const int AddSeniorityAristocrat3 = 57;

		public const int AddSeniorityAristocrat4 = 58;

		public const int AddSeniorityAristocrat5 = 59;

		public const int AddSeniorityBeggar1 = 60;

		public const int AddSeniorityBeggar2 = 61;

		public const int AddSeniorityBeggar3 = 62;

		public const int AddSeniorityBeggar4 = 63;

		public const int AddSeniorityBeggar5 = 64;

		public const int AddSeniorityCivilian1 = 65;

		public const int AddSeniorityCivilian2 = 66;

		public const int AddSeniorityCivilian3 = 67;

		public const int AddSeniorityCivilian4 = 68;

		public const int AddSeniorityCivilian5 = 69;

		public const int AddSeniorityCivilian6 = 70;

		public const int AddSeniorityTraveler1 = 71;

		public const int AddSeniorityTraveler2 = 72;

		public const int AddSeniorityTraveler3 = 73;

		public const int AddSeniorityTraveler4 = 74;

		public const int AddSeniorityTraveler5 = 75;

		public const int AddSeniorityTraveler6 = 76;

		public const int AddSeniorityTravelingBuddhistMonk1 = 77;

		public const int AddSeniorityTravelingBuddhistMonk2 = 78;

		public const int AddSeniorityTravelingBuddhistMonk3 = 79;

		public const int AddSeniorityTravelingBuddhistMonk4 = 80;

		public const int AddSeniorityTravelingBuddhistMonk5 = 81;

		public const int AddSeniorityDoctor1 = 82;

		public const int AddSeniorityDoctor2 = 83;

		public const int AddSeniorityDoctor3 = 84;

		public const int AddSeniorityDoctor4 = 85;

		public const int AddSeniorityDoctor5 = 86;

		public const int AddSeniorityDoctor6 = 87;

		public const int AddSeniorityDoctor7 = 88;

		public const int AddSeniorityDoctor8 = 89;

		public const int AddSeniorityTravelingTaoistMonk1 = 90;

		public const int AddSeniorityTravelingTaoistMonk2 = 91;

		public const int AddSeniorityTravelingTaoistMonk3 = 92;

		public const int AddSeniorityTravelingTaoistMonk4 = 93;

		public const int AddSeniorityTravelingTaoistMonk5 = 94;

		public const int AddSeniorityCapitalist1 = 95;

		public const int AddSeniorityCapitalist2 = 96;

		public const int AddSeniorityCapitalist3 = 97;

		public const int AddSeniorityCapitalist4 = 98;

		public const int AddSeniorityTeaTaster1 = 99;

		public const int AddSeniorityTeaTaster2 = 100;

		public const int AddSeniorityTeaTaster3 = 101;

		public const int AddSeniorityTeaTaster4 = 102;

		public const int AddSeniorityTeaTaster5 = 103;

		public const int AddSeniorityTeaTaster6 = 104;

		public const int AddSeniorityTeaTaster7 = 105;

		public const int AddSeniorityDuke1 = 106;

		public const int AddSeniorityDuke2 = 107;

		public const int AddSeniorityDuke3 = 108;

		public const int AddSeniorityDuke4 = 109;

		public const int AddSeniorityDuke5 = 110;

		public const int AddSeniorityDuke6 = 111;

		public const int SkillDukeGetCricketSimulateCount = 112;
	}

	public static class DefValue
	{
		public static ProfessionFormulaItem AddSenioritySavage1 => Instance[0];

		public static ProfessionFormulaItem AddSenioritySavage2 => Instance[1];

		public static ProfessionFormulaItem AddSenioritySavage3 => Instance[2];

		public static ProfessionFormulaItem AddSenioritySavage4 => Instance[3];

		public static ProfessionFormulaItem AddSenioritySavage5 => Instance[4];

		public static ProfessionFormulaItem AddSenioritySavage6 => Instance[5];

		public static ProfessionFormulaItem AddSenioritySavage7 => Instance[6];

		public static ProfessionFormulaItem AddSenioritySavage8 => Instance[7];

		public static ProfessionFormulaItem AddSeniorityHunter1 => Instance[8];

		public static ProfessionFormulaItem AddSeniorityHunter2 => Instance[9];

		public static ProfessionFormulaItem AddSeniorityHunter3 => Instance[10];

		public static ProfessionFormulaItem AddSeniorityHunter4 => Instance[11];

		public static ProfessionFormulaItem AddSeniorityHunter5 => Instance[12];

		public static ProfessionFormulaItem AddSeniorityHunter6 => Instance[13];

		public static ProfessionFormulaItem AddSeniorityHunter7 => Instance[14];

		public static ProfessionFormulaItem AddSeniorityHunter8 => Instance[15];

		public static ProfessionFormulaItem AddSeniorityCraft1 => Instance[16];

		public static ProfessionFormulaItem AddSeniorityCraft2 => Instance[17];

		public static ProfessionFormulaItem AddSeniorityCraft3 => Instance[18];

		public static ProfessionFormulaItem AddSeniorityCraft4 => Instance[19];

		public static ProfessionFormulaItem AddSeniorityCraft5 => Instance[20];

		public static ProfessionFormulaItem AddSeniorityCraft61 => Instance[21];

		public static ProfessionFormulaItem AddSeniorityCraft62 => Instance[22];

		public static ProfessionFormulaItem AddSeniorityMartialArtist1 => Instance[23];

		public static ProfessionFormulaItem AddSeniorityMartialArtist2 => Instance[24];

		public static ProfessionFormulaItem AddSeniorityMartialArtist3 => Instance[25];

		public static ProfessionFormulaItem AddSeniorityMartialArtist4 => Instance[26];

		public static ProfessionFormulaItem AddSeniorityMartialArtist5 => Instance[27];

		public static ProfessionFormulaItem AddSeniorityMartialArtist6 => Instance[28];

		public static ProfessionFormulaItem AddSeniorityMartialArtist7 => Instance[29];

		public static ProfessionFormulaItem AddSeniorityMartialArtist8 => Instance[113];

		public static ProfessionFormulaItem AddSeniorityLiterati1 => Instance[30];

		public static ProfessionFormulaItem AddSeniorityLiterati2 => Instance[31];

		public static ProfessionFormulaItem AddSeniorityLiterati3 => Instance[32];

		public static ProfessionFormulaItem AddSeniorityLiterati4 => Instance[33];

		public static ProfessionFormulaItem AddSeniorityLiterati5 => Instance[34];

		public static ProfessionFormulaItem AddSeniorityLiterati6 => Instance[35];

		public static ProfessionFormulaItem AddSeniorityTaoistMonk1 => Instance[36];

		public static ProfessionFormulaItem AddSeniorityTaoistMonk2 => Instance[37];

		public static ProfessionFormulaItem AddSeniorityTaoistMonk3 => Instance[38];

		public static ProfessionFormulaItem AddSeniorityTaoistMonk4 => Instance[39];

		public static ProfessionFormulaItem AddSeniorityTaoistMonk5 => Instance[40];

		public static ProfessionFormulaItem AddSeniorityTaoistMonk6 => Instance[41];

		public static ProfessionFormulaItem AddSeniorityBuddhistMonk1 => Instance[42];

		public static ProfessionFormulaItem AddSeniorityBuddhistMonk2 => Instance[43];

		public static ProfessionFormulaItem AddSeniorityBuddhistMonk3 => Instance[44];

		public static ProfessionFormulaItem AddSeniorityBuddhistMonk4 => Instance[45];

		public static ProfessionFormulaItem AddSeniorityBuddhistMonk5 => Instance[46];

		public static ProfessionFormulaItem AddSeniorityBuddhistMonk6 => Instance[47];

		public static ProfessionFormulaItem AddSeniorityWineTaster1 => Instance[48];

		public static ProfessionFormulaItem AddSeniorityWineTaster2 => Instance[49];

		public static ProfessionFormulaItem AddSeniorityWineTaster3 => Instance[50];

		public static ProfessionFormulaItem AddSeniorityWineTaster4 => Instance[51];

		public static ProfessionFormulaItem AddSeniorityWineTaster5 => Instance[52];

		public static ProfessionFormulaItem AddSeniorityWineTaster6 => Instance[53];

		public static ProfessionFormulaItem AddSeniorityWineTaster7 => Instance[54];

		public static ProfessionFormulaItem AddSeniorityAristocrat1 => Instance[55];

		public static ProfessionFormulaItem AddSeniorityAristocrat2 => Instance[56];

		public static ProfessionFormulaItem AddSeniorityAristocrat3 => Instance[57];

		public static ProfessionFormulaItem AddSeniorityAristocrat4 => Instance[58];

		public static ProfessionFormulaItem AddSeniorityAristocrat5 => Instance[59];

		public static ProfessionFormulaItem AddSeniorityBeggar1 => Instance[60];

		public static ProfessionFormulaItem AddSeniorityBeggar2 => Instance[61];

		public static ProfessionFormulaItem AddSeniorityBeggar3 => Instance[62];

		public static ProfessionFormulaItem AddSeniorityBeggar4 => Instance[63];

		public static ProfessionFormulaItem AddSeniorityBeggar5 => Instance[64];

		public static ProfessionFormulaItem AddSeniorityCivilian1 => Instance[65];

		public static ProfessionFormulaItem AddSeniorityCivilian2 => Instance[66];

		public static ProfessionFormulaItem AddSeniorityCivilian3 => Instance[67];

		public static ProfessionFormulaItem AddSeniorityCivilian4 => Instance[68];

		public static ProfessionFormulaItem AddSeniorityCivilian5 => Instance[69];

		public static ProfessionFormulaItem AddSeniorityCivilian6 => Instance[70];

		public static ProfessionFormulaItem AddSeniorityTraveler1 => Instance[71];

		public static ProfessionFormulaItem AddSeniorityTraveler2 => Instance[72];

		public static ProfessionFormulaItem AddSeniorityTraveler3 => Instance[73];

		public static ProfessionFormulaItem AddSeniorityTraveler4 => Instance[74];

		public static ProfessionFormulaItem AddSeniorityTraveler5 => Instance[75];

		public static ProfessionFormulaItem AddSeniorityTraveler6 => Instance[76];

		public static ProfessionFormulaItem AddSeniorityTravelingBuddhistMonk1 => Instance[77];

		public static ProfessionFormulaItem AddSeniorityTravelingBuddhistMonk2 => Instance[78];

		public static ProfessionFormulaItem AddSeniorityTravelingBuddhistMonk3 => Instance[79];

		public static ProfessionFormulaItem AddSeniorityTravelingBuddhistMonk4 => Instance[80];

		public static ProfessionFormulaItem AddSeniorityTravelingBuddhistMonk5 => Instance[81];

		public static ProfessionFormulaItem AddSeniorityDoctor1 => Instance[82];

		public static ProfessionFormulaItem AddSeniorityDoctor2 => Instance[83];

		public static ProfessionFormulaItem AddSeniorityDoctor3 => Instance[84];

		public static ProfessionFormulaItem AddSeniorityDoctor4 => Instance[85];

		public static ProfessionFormulaItem AddSeniorityDoctor5 => Instance[86];

		public static ProfessionFormulaItem AddSeniorityDoctor6 => Instance[87];

		public static ProfessionFormulaItem AddSeniorityDoctor7 => Instance[88];

		public static ProfessionFormulaItem AddSeniorityDoctor8 => Instance[89];

		public static ProfessionFormulaItem AddSeniorityTravelingTaoistMonk1 => Instance[90];

		public static ProfessionFormulaItem AddSeniorityTravelingTaoistMonk2 => Instance[91];

		public static ProfessionFormulaItem AddSeniorityTravelingTaoistMonk3 => Instance[92];

		public static ProfessionFormulaItem AddSeniorityTravelingTaoistMonk4 => Instance[93];

		public static ProfessionFormulaItem AddSeniorityTravelingTaoistMonk5 => Instance[94];

		public static ProfessionFormulaItem AddSeniorityCapitalist1 => Instance[95];

		public static ProfessionFormulaItem AddSeniorityCapitalist2 => Instance[96];

		public static ProfessionFormulaItem AddSeniorityCapitalist3 => Instance[97];

		public static ProfessionFormulaItem AddSeniorityCapitalist4 => Instance[98];

		public static ProfessionFormulaItem AddSeniorityTeaTaster1 => Instance[99];

		public static ProfessionFormulaItem AddSeniorityTeaTaster2 => Instance[100];

		public static ProfessionFormulaItem AddSeniorityTeaTaster3 => Instance[101];

		public static ProfessionFormulaItem AddSeniorityTeaTaster4 => Instance[102];

		public static ProfessionFormulaItem AddSeniorityTeaTaster5 => Instance[103];

		public static ProfessionFormulaItem AddSeniorityTeaTaster6 => Instance[104];

		public static ProfessionFormulaItem AddSeniorityTeaTaster7 => Instance[105];

		public static ProfessionFormulaItem AddSeniorityDuke1 => Instance[106];

		public static ProfessionFormulaItem AddSeniorityDuke2 => Instance[107];

		public static ProfessionFormulaItem AddSeniorityDuke3 => Instance[108];

		public static ProfessionFormulaItem AddSeniorityDuke4 => Instance[109];

		public static ProfessionFormulaItem AddSeniorityDuke5 => Instance[110];

		public static ProfessionFormulaItem AddSeniorityDuke6 => Instance[111];

		public static ProfessionFormulaItem SkillDukeGetCricketSimulateCount => Instance[112];
	}

	public static ProfessionFormula Instance = new ProfessionFormula();

	private readonly HashSet<string> RequiredFields = new HashSet<string> { "TemplateId" };

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
		_dataArray.Add(new ProfessionFormulaItem(0, EProfessionFormulaType.SeniorityGainFormula1, new int[1] { 20 }, 3000));
		_dataArray.Add(new ProfessionFormulaItem(1, EProfessionFormulaType.SeniorityGainFormula1, new int[1] { 1 }, 28200));
		_dataArray.Add(new ProfessionFormulaItem(2, EProfessionFormulaType.SeniorityGainFormula9, new int[1] { 28200 }, 28200));
		_dataArray.Add(new ProfessionFormulaItem(3, EProfessionFormulaType.SeniorityGainFormula9, new int[1] { 123000 }, 123000));
		_dataArray.Add(new ProfessionFormulaItem(4, EProfessionFormulaType.SeniorityGainFormula6, new int[1] { 2 }, 14100));
		_dataArray.Add(new ProfessionFormulaItem(5, EProfessionFormulaType.SeniorityGainFormula1, new int[1] { 1 }, 61500));
		_dataArray.Add(new ProfessionFormulaItem(6, EProfessionFormulaType.SeniorityGainFormula12, new int[2] { 250, 100 }, 70500));
		_dataArray.Add(new ProfessionFormulaItem(7, EProfessionFormulaType.SeniorityGainFormula1, new int[1], 24000));
		_dataArray.Add(new ProfessionFormulaItem(8, EProfessionFormulaType.SeniorityGainFormula3, new int[3] { 20, 4, 2 }, 6480));
		_dataArray.Add(new ProfessionFormulaItem(9, EProfessionFormulaType.SeniorityGainFormula3, new int[3] { 50, 4, 2 }, 16200));
		_dataArray.Add(new ProfessionFormulaItem(10, EProfessionFormulaType.SeniorityGainFormula3, new int[3] { 100, 4, 2 }, 32400));
		_dataArray.Add(new ProfessionFormulaItem(11, EProfessionFormulaType.SeniorityGainFormula4, new int[3] { 10, 2, 2 }, 10000));
		_dataArray.Add(new ProfessionFormulaItem(12, EProfessionFormulaType.SeniorityGainFormula4, new int[3] { 5, 2, 2 }, 25000));
		_dataArray.Add(new ProfessionFormulaItem(13, EProfessionFormulaType.SeniorityGainFormula3, new int[3] { 100, 2, 2 }, 10000));
		_dataArray.Add(new ProfessionFormulaItem(14, EProfessionFormulaType.SeniorityGainFormula9, new int[1] { 162000 }, 162000));
		_dataArray.Add(new ProfessionFormulaItem(15, EProfessionFormulaType.SeniorityGainFormula9, new int[1] { 810000 }, 810000));
		_dataArray.Add(new ProfessionFormulaItem(16, EProfessionFormulaType.SeniorityGainFormula3, new int[3] { 100, 2, 3 }, 100000));
		_dataArray.Add(new ProfessionFormulaItem(17, EProfessionFormulaType.SeniorityGainFormula3, new int[3] { 20, 2, 3 }, 20000));
		_dataArray.Add(new ProfessionFormulaItem(18, EProfessionFormulaType.SeniorityGainFormula5, new int[4] { 10, 2, 3, 100 }, 10000));
		_dataArray.Add(new ProfessionFormulaItem(19, EProfessionFormulaType.SeniorityGainFormula3, new int[3] { 5, 2, 3 }, 5000));
		_dataArray.Add(new ProfessionFormulaItem(20, EProfessionFormulaType.SeniorityGainFormula1, new int[1] { 3 }, 4800));
		_dataArray.Add(new ProfessionFormulaItem(21, EProfessionFormulaType.SeniorityGainFormula12, new int[2] { 25, 100 }, 7050));
		_dataArray.Add(new ProfessionFormulaItem(22, EProfessionFormulaType.SeniorityGainFormula12, new int[2] { 25, 100 }, 4600));
		_dataArray.Add(new ProfessionFormulaItem(23, EProfessionFormulaType.SeniorityGainFormula1, new int[1] { 5 }, 7125));
		_dataArray.Add(new ProfessionFormulaItem(24, EProfessionFormulaType.SeniorityGainFormula1, new int[1] { 5 }, 14250));
		_dataArray.Add(new ProfessionFormulaItem(25, EProfessionFormulaType.SeniorityGainFormula1, new int[1] { 5 }, 14250));
		_dataArray.Add(new ProfessionFormulaItem(26, EProfessionFormulaType.SeniorityGainFormula3, new int[3] { 1000, 1, 2 }, 100000));
		_dataArray.Add(new ProfessionFormulaItem(27, EProfessionFormulaType.SeniorityGainFormula4, new int[3] { 100, 2, 2 }, 50000));
		_dataArray.Add(new ProfessionFormulaItem(28, EProfessionFormulaType.SeniorityGainFormula4, new int[3] { 3, 2, 2 }, 30000));
		_dataArray.Add(new ProfessionFormulaItem(29, EProfessionFormulaType.SeniorityGainFormula3, new int[3] { 500, 0, 2 }, 32000));
		_dataArray.Add(new ProfessionFormulaItem(30, EProfessionFormulaType.SeniorityGainFormula7, new int[2] { 3, 2 }, 22500));
		_dataArray.Add(new ProfessionFormulaItem(31, EProfessionFormulaType.SeniorityGainFormula3, new int[3] { 100, 2, 2 }, 10000));
		_dataArray.Add(new ProfessionFormulaItem(32, EProfessionFormulaType.SeniorityGainFormula2, new int[1] { 1 }, 9000));
		_dataArray.Add(new ProfessionFormulaItem(33, EProfessionFormulaType.SeniorityGainFormula2, new int[1] { 1 }, 9000));
		_dataArray.Add(new ProfessionFormulaItem(34, EProfessionFormulaType.SeniorityGainFormula2, new int[1] { 10 }, 90000));
		_dataArray.Add(new ProfessionFormulaItem(35, EProfessionFormulaType.SeniorityGainFormula4, new int[3] { 3, 2, 2 }, 30000));
		_dataArray.Add(new ProfessionFormulaItem(36, EProfessionFormulaType.SeniorityGainFormula3, new int[3] { 50, 2, 3 }, 50000));
		_dataArray.Add(new ProfessionFormulaItem(37, EProfessionFormulaType.SeniorityGainFormula3, new int[3] { 5, 2, 3 }, 5000));
		_dataArray.Add(new ProfessionFormulaItem(38, EProfessionFormulaType.SeniorityGainFormula3, new int[3] { 50, 2, 3 }, 50000));
		_dataArray.Add(new ProfessionFormulaItem(39, EProfessionFormulaType.SeniorityGainFormula3, new int[3] { 50, 2, 3 }, 50000));
		_dataArray.Add(new ProfessionFormulaItem(40, EProfessionFormulaType.SeniorityGainFormula1, new int[1] { 5 }, 14250));
		_dataArray.Add(new ProfessionFormulaItem(41, EProfessionFormulaType.SeniorityGainFormula3, new int[3] { 100, 2, 2 }, 10000));
		_dataArray.Add(new ProfessionFormulaItem(42, EProfessionFormulaType.SeniorityGainFormula3, new int[3] { 50, 2, 3 }, 50000));
		_dataArray.Add(new ProfessionFormulaItem(43, EProfessionFormulaType.SeniorityGainFormula3, new int[3] { 5, 2, 3 }, 5000));
		_dataArray.Add(new ProfessionFormulaItem(44, EProfessionFormulaType.SeniorityGainFormula3, new int[3] { 100, 2, 2 }, 40000));
		_dataArray.Add(new ProfessionFormulaItem(45, EProfessionFormulaType.SeniorityGainFormula3, new int[3] { 20, 2, 2 }, 8000));
		_dataArray.Add(new ProfessionFormulaItem(46, EProfessionFormulaType.SeniorityGainFormula1, new int[1] { 10 }, 36000));
		_dataArray.Add(new ProfessionFormulaItem(47, EProfessionFormulaType.SeniorityGainFormula3, new int[3] { 100, 2, 2 }, 10000));
		_dataArray.Add(new ProfessionFormulaItem(48, EProfessionFormulaType.SeniorityGainFormula6, new int[1] { 2 }, 20500));
		_dataArray.Add(new ProfessionFormulaItem(49, EProfessionFormulaType.SeniorityGainFormula6, new int[1] { 10 }, 4100));
		_dataArray.Add(new ProfessionFormulaItem(50, EProfessionFormulaType.SeniorityGainFormula3, new int[3] { 300, 2, 2 }, 30000));
		_dataArray.Add(new ProfessionFormulaItem(51, EProfessionFormulaType.SeniorityGainFormula6, new int[1] { 2 }, 30750));
		_dataArray.Add(new ProfessionFormulaItem(52, EProfessionFormulaType.SeniorityGainFormula3, new int[3] { 200, 2, 2 }, 20000));
		_dataArray.Add(new ProfessionFormulaItem(53, EProfessionFormulaType.SeniorityGainFormula3, new int[3] { 100, 2, 2 }, 10000));
		_dataArray.Add(new ProfessionFormulaItem(54, EProfessionFormulaType.SeniorityGainFormula6, new int[1] { 4 }, 10250));
		_dataArray.Add(new ProfessionFormulaItem(55, EProfessionFormulaType.SeniorityGainFormula1, new int[1] { 50 }, 10000));
		_dataArray.Add(new ProfessionFormulaItem(56, EProfessionFormulaType.SeniorityGainFormula2, new int[1] { 50 }, 10000));
		_dataArray.Add(new ProfessionFormulaItem(57, EProfessionFormulaType.SeniorityGainFormula1, new int[1] { 1000 }, 200000));
		_dataArray.Add(new ProfessionFormulaItem(58, EProfessionFormulaType.SeniorityGainFormula2, new int[1] { 10 }, 240000));
		_dataArray.Add(new ProfessionFormulaItem(59, EProfessionFormulaType.SeniorityGainFormula4, new int[3] { 50, 2, 2 }, 25000));
	}

	private void CreateItems1()
	{
		_dataArray.Add(new ProfessionFormulaItem(60, EProfessionFormulaType.SeniorityGainFormula1, new int[1] { 10 }, 10240));
		_dataArray.Add(new ProfessionFormulaItem(61, EProfessionFormulaType.SeniorityGainFormula3, new int[3] { 50, 2, 2 }, 5000));
		_dataArray.Add(new ProfessionFormulaItem(62, EProfessionFormulaType.SeniorityGainFormula6, new int[1] { 2 }, 10250));
		_dataArray.Add(new ProfessionFormulaItem(63, EProfessionFormulaType.SeniorityGainFormula8, new int[3] { 500, 4, 2 }, 8000));
		_dataArray.Add(new ProfessionFormulaItem(64, EProfessionFormulaType.SeniorityGainFormula3, new int[3] { 500, 0, 2 }, 32000));
		_dataArray.Add(new ProfessionFormulaItem(65, EProfessionFormulaType.SeniorityGainFormula3, new int[3] { 50, 2, 3 }, 50000));
		_dataArray.Add(new ProfessionFormulaItem(66, EProfessionFormulaType.SeniorityGainFormula1, new int[1] { 10 }, 36000));
		_dataArray.Add(new ProfessionFormulaItem(67, EProfessionFormulaType.SeniorityGainFormula3, new int[3] { 50, 4, 2 }, 20000));
		_dataArray.Add(new ProfessionFormulaItem(68, EProfessionFormulaType.SeniorityGainFormula3, new int[3] { 50, 4, 2 }, 20000));
		_dataArray.Add(new ProfessionFormulaItem(69, EProfessionFormulaType.SeniorityGainFormula3, new int[3] { 50, 4, 2 }, 20000));
		_dataArray.Add(new ProfessionFormulaItem(70, EProfessionFormulaType.SeniorityGainFormula3, new int[3] { 500, 0, 2 }, 32000));
		_dataArray.Add(new ProfessionFormulaItem(71, EProfessionFormulaType.SeniorityGainFormula1, new int[1] { 10 }, -1));
		_dataArray.Add(new ProfessionFormulaItem(72, EProfessionFormulaType.SeniorityGainFormula1, new int[1] { 2 }, -1));
		_dataArray.Add(new ProfessionFormulaItem(73, EProfessionFormulaType.SeniorityGainFormula1, new int[1] { 10 }, -1));
		_dataArray.Add(new ProfessionFormulaItem(74, EProfessionFormulaType.SeniorityGainFormula9, new int[1] { 30000 }, 30000));
		_dataArray.Add(new ProfessionFormulaItem(75, EProfessionFormulaType.SeniorityGainFormula9, new int[1] { 5000 }, 5000));
		_dataArray.Add(new ProfessionFormulaItem(76, EProfessionFormulaType.SeniorityGainFormula9, new int[1] { 5000 }, 5000));
		_dataArray.Add(new ProfessionFormulaItem(77, EProfessionFormulaType.SeniorityGainFormula3, new int[3] { 100, 2, 2 }, 10000));
		_dataArray.Add(new ProfessionFormulaItem(78, EProfessionFormulaType.SeniorityGainFormula3, new int[3] { 200, 2, 2 }, 20000));
		_dataArray.Add(new ProfessionFormulaItem(79, EProfessionFormulaType.SeniorityGainFormula7, new int[2] { 3, 2 }, 22500));
		_dataArray.Add(new ProfessionFormulaItem(80, EProfessionFormulaType.SeniorityGainFormula9, new int[1] { 50000 }, 50000));
		_dataArray.Add(new ProfessionFormulaItem(81, EProfessionFormulaType.SeniorityGainFormula7, new int[2] { 3, 2 }, 22500));
		_dataArray.Add(new ProfessionFormulaItem(82, EProfessionFormulaType.SeniorityGainFormula3, new int[3] { 30, 2, 3 }, 30000));
		_dataArray.Add(new ProfessionFormulaItem(83, EProfessionFormulaType.SeniorityGainFormula3, new int[3] { 20, 2, 3 }, 20000));
		_dataArray.Add(new ProfessionFormulaItem(84, EProfessionFormulaType.SeniorityGainFormula3, new int[3] { 10, 2, 3 }, 10000));
		_dataArray.Add(new ProfessionFormulaItem(85, EProfessionFormulaType.SeniorityGainFormula10, new int[6] { 250, 500, 1000, 2000, 4000, 8000 }, -1));
		_dataArray.Add(new ProfessionFormulaItem(86, EProfessionFormulaType.SeniorityGainFormula10, new int[3] { 1000, 2000, 16000 }, -1));
		_dataArray.Add(new ProfessionFormulaItem(87, EProfessionFormulaType.SeniorityGainFormula0, new int[0], -1));
		_dataArray.Add(new ProfessionFormulaItem(88, EProfessionFormulaType.SeniorityGainFormula1, new int[1] { 100 }, -1));
		_dataArray.Add(new ProfessionFormulaItem(89, EProfessionFormulaType.SeniorityGainFormula6, new int[1] { 4 }, 7050));
		_dataArray.Add(new ProfessionFormulaItem(90, EProfessionFormulaType.SeniorityGainFormula3, new int[3] { 150, 1, 2 }, 30000));
		_dataArray.Add(new ProfessionFormulaItem(91, EProfessionFormulaType.SeniorityGainFormula1, new int[1] { 10 }, 10000));
		_dataArray.Add(new ProfessionFormulaItem(92, EProfessionFormulaType.SeniorityGainFormula3, new int[3] { 50, 2, 2 }, 20000));
		_dataArray.Add(new ProfessionFormulaItem(93, EProfessionFormulaType.SeniorityGainFormula1, new int[1] { 100 }, -1));
		_dataArray.Add(new ProfessionFormulaItem(94, EProfessionFormulaType.SeniorityGainFormula7, new int[2] { 3, 2 }, 22500));
		_dataArray.Add(new ProfessionFormulaItem(95, EProfessionFormulaType.SeniorityGainFormula6, new int[1] { 4 }, 15375));
		_dataArray.Add(new ProfessionFormulaItem(96, EProfessionFormulaType.SeniorityGainFormula6, new int[1] { 4 }, 15375));
		_dataArray.Add(new ProfessionFormulaItem(97, EProfessionFormulaType.SeniorityGainFormula6, new int[1] { 2 }, 30750));
		_dataArray.Add(new ProfessionFormulaItem(98, EProfessionFormulaType.SeniorityGainFormula11, new int[3] { 10000, 100, 100 }, -1));
		_dataArray.Add(new ProfessionFormulaItem(99, EProfessionFormulaType.SeniorityGainFormula6, new int[1] { 2 }, 20500));
		_dataArray.Add(new ProfessionFormulaItem(100, EProfessionFormulaType.SeniorityGainFormula6, new int[1] { 10 }, 4100));
		_dataArray.Add(new ProfessionFormulaItem(101, EProfessionFormulaType.SeniorityGainFormula3, new int[3] { 300, 2, 2 }, 30000));
		_dataArray.Add(new ProfessionFormulaItem(102, EProfessionFormulaType.SeniorityGainFormula6, new int[1] { 2 }, 30750));
		_dataArray.Add(new ProfessionFormulaItem(103, EProfessionFormulaType.SeniorityGainFormula3, new int[3] { 200, 2, 2 }, -1));
		_dataArray.Add(new ProfessionFormulaItem(104, EProfessionFormulaType.SeniorityGainFormula3, new int[3] { 100, 2, 2 }, 10000));
		_dataArray.Add(new ProfessionFormulaItem(105, EProfessionFormulaType.SeniorityGainFormula6, new int[1] { 4 }, 10250));
		_dataArray.Add(new ProfessionFormulaItem(106, EProfessionFormulaType.SeniorityGainFormula6, new int[1] { 2 }, 30750));
		_dataArray.Add(new ProfessionFormulaItem(107, EProfessionFormulaType.SeniorityGainFormula3, new int[3] { 500, 2, 2 }, -1));
		_dataArray.Add(new ProfessionFormulaItem(108, EProfessionFormulaType.SeniorityGainFormula9, new int[1] { 10250 }, -1));
		_dataArray.Add(new ProfessionFormulaItem(109, EProfessionFormulaType.SeniorityGainFormula9, new int[1] { 153750 }, -1));
		_dataArray.Add(new ProfessionFormulaItem(110, EProfessionFormulaType.SeniorityGainFormula6, new int[1] { 2 }, 30750));
		_dataArray.Add(new ProfessionFormulaItem(111, EProfessionFormulaType.SeniorityGainFormula9, new int[1] { 307500 }, -1));
		_dataArray.Add(new ProfessionFormulaItem(112, EProfessionFormulaType.SeniorityGainFormula9, new int[1] { 18 }, -1));
		_dataArray.Add(new ProfessionFormulaItem(113, EProfessionFormulaType.SeniorityGainFormula1, new int[1] { 1000 }, -1));
	}

	public override void Init()
	{
		base.Init();
		_dataArray = new List<ProfessionFormulaItem>(114);
		CreateItems0();
		CreateItems1();
	}
}
