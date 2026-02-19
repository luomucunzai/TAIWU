using System;
using System.Collections.Generic;
using Config.Common;

namespace Config;

[Serializable]
public class ProfessionSkill : ConfigData<ProfessionSkillItem, int>
{
	public static class DefKey
	{
		public const int SavageSkill0 = 0;

		public const int SavageSkill1 = 1;

		public const int SavageSkill2 = 2;

		public const int SavageSkill3 = 60;

		public const int HunterSkill0 = 3;

		public const int HunterSkill1 = 4;

		public const int HunterSkill2 = 5;

		public const int HunterSkill3 = 61;

		public const int CraftSkill0 = 6;

		public const int CraftSkill1 = 7;

		public const int CraftSkill2 = 8;

		public const int CraftSkill3 = 62;

		public const int MartialArtistSkill0 = 9;

		public const int MartialArtistSkill1 = 10;

		public const int MartialArtistSkill2 = 11;

		public const int MartialArtistSkill3 = 63;

		public const int LiteratiSkill0 = 12;

		public const int LiteratiSkill1 = 13;

		public const int LiteratiSkill2 = 14;

		public const int LiteratiSkill3 = 64;

		public const int TaoistMonkSkill0 = 15;

		public const int TaoistMonkSkill1 = 16;

		public const int TaoistMonkSkill2 = 17;

		public const int TaoistMonkSkill3 = 18;

		public const int BuddhistMonkSkill0 = 19;

		public const int BuddhistMonkSkill1 = 20;

		public const int BuddhistMonkSkill2 = 21;

		public const int BuddhistMonkSkill3 = 22;

		public const int WineTasterSkill0 = 23;

		public const int WineTasterSkill1 = 24;

		public const int WineTasterSkill2 = 25;

		public const int WineTasterSkill3 = 65;

		public const int AristocratSkill0 = 26;

		public const int AristocratSkill1 = 27;

		public const int AristocratSkill2 = 28;

		public const int AristocratSkill3 = 66;

		public const int BeggarSkill0 = 29;

		public const int BeggarSkill1 = 30;

		public const int BeggarSkill2 = 31;

		public const int BeggarSkill3 = 67;

		public const int CivilianSkill0 = 32;

		public const int CivilianSkill1 = 33;

		public const int CivilianSkill2 = 34;

		public const int CivilianSkill3 = 59;

		public const int TravelerSkill0 = 35;

		public const int TravelerSkill1 = 36;

		public const int TravelerSkill2 = 37;

		public const int TravelerSkill3 = 68;

		public const int TravelingBuddhistMonkSkill0 = 38;

		public const int TravelingBuddhistMonkSkill1 = 39;

		public const int TravelingBuddhistMonkSkill2 = 40;

		public const int TravelingBuddhistMonkSkill3 = 41;

		public const int DoctorSkill0 = 42;

		public const int DoctorSkill1 = 43;

		public const int DoctorSkill2 = 44;

		public const int DoctorSkill3 = 69;

		public const int TravelingTaoistMonkSkill0 = 45;

		public const int TravelingTaoistMonkSkill1 = 46;

		public const int TravelingTaoistMonkSkill2 = 47;

		public const int TravelingTaoistMonkSkill3 = 48;

		public const int CapitalistSkill0 = 49;

		public const int CapitalistSkill1 = 50;

		public const int CapitalistSkill2 = 51;

		public const int CapitalistSkill3 = 71;

		public const int TeaTasterSkill0 = 52;

		public const int TeaTasterSkill1 = 53;

		public const int TeaTasterSkill2 = 54;

		public const int TeaTasterSkill3 = 58;

		public const int DukeSkill0 = 55;

		public const int DukeSkill1 = 56;

		public const int DukeSkill2 = 57;

		public const int DukeSkill3 = 70;
	}

	public static class DefValue
	{
		public static ProfessionSkillItem SavageSkill0 => Instance[0];

		public static ProfessionSkillItem SavageSkill1 => Instance[1];

		public static ProfessionSkillItem SavageSkill2 => Instance[2];

		public static ProfessionSkillItem SavageSkill3 => Instance[60];

		public static ProfessionSkillItem HunterSkill0 => Instance[3];

		public static ProfessionSkillItem HunterSkill1 => Instance[4];

		public static ProfessionSkillItem HunterSkill2 => Instance[5];

		public static ProfessionSkillItem HunterSkill3 => Instance[61];

		public static ProfessionSkillItem CraftSkill0 => Instance[6];

		public static ProfessionSkillItem CraftSkill1 => Instance[7];

		public static ProfessionSkillItem CraftSkill2 => Instance[8];

		public static ProfessionSkillItem CraftSkill3 => Instance[62];

		public static ProfessionSkillItem MartialArtistSkill0 => Instance[9];

		public static ProfessionSkillItem MartialArtistSkill1 => Instance[10];

		public static ProfessionSkillItem MartialArtistSkill2 => Instance[11];

		public static ProfessionSkillItem MartialArtistSkill3 => Instance[63];

		public static ProfessionSkillItem LiteratiSkill0 => Instance[12];

		public static ProfessionSkillItem LiteratiSkill1 => Instance[13];

		public static ProfessionSkillItem LiteratiSkill2 => Instance[14];

		public static ProfessionSkillItem LiteratiSkill3 => Instance[64];

		public static ProfessionSkillItem TaoistMonkSkill0 => Instance[15];

		public static ProfessionSkillItem TaoistMonkSkill1 => Instance[16];

		public static ProfessionSkillItem TaoistMonkSkill2 => Instance[17];

		public static ProfessionSkillItem TaoistMonkSkill3 => Instance[18];

		public static ProfessionSkillItem BuddhistMonkSkill0 => Instance[19];

		public static ProfessionSkillItem BuddhistMonkSkill1 => Instance[20];

		public static ProfessionSkillItem BuddhistMonkSkill2 => Instance[21];

		public static ProfessionSkillItem BuddhistMonkSkill3 => Instance[22];

		public static ProfessionSkillItem WineTasterSkill0 => Instance[23];

		public static ProfessionSkillItem WineTasterSkill1 => Instance[24];

		public static ProfessionSkillItem WineTasterSkill2 => Instance[25];

		public static ProfessionSkillItem WineTasterSkill3 => Instance[65];

		public static ProfessionSkillItem AristocratSkill0 => Instance[26];

		public static ProfessionSkillItem AristocratSkill1 => Instance[27];

		public static ProfessionSkillItem AristocratSkill2 => Instance[28];

		public static ProfessionSkillItem AristocratSkill3 => Instance[66];

		public static ProfessionSkillItem BeggarSkill0 => Instance[29];

		public static ProfessionSkillItem BeggarSkill1 => Instance[30];

		public static ProfessionSkillItem BeggarSkill2 => Instance[31];

		public static ProfessionSkillItem BeggarSkill3 => Instance[67];

		public static ProfessionSkillItem CivilianSkill0 => Instance[32];

		public static ProfessionSkillItem CivilianSkill1 => Instance[33];

		public static ProfessionSkillItem CivilianSkill2 => Instance[34];

		public static ProfessionSkillItem CivilianSkill3 => Instance[59];

		public static ProfessionSkillItem TravelerSkill0 => Instance[35];

		public static ProfessionSkillItem TravelerSkill1 => Instance[36];

		public static ProfessionSkillItem TravelerSkill2 => Instance[37];

		public static ProfessionSkillItem TravelerSkill3 => Instance[68];

		public static ProfessionSkillItem TravelingBuddhistMonkSkill0 => Instance[38];

		public static ProfessionSkillItem TravelingBuddhistMonkSkill1 => Instance[39];

		public static ProfessionSkillItem TravelingBuddhistMonkSkill2 => Instance[40];

		public static ProfessionSkillItem TravelingBuddhistMonkSkill3 => Instance[41];

		public static ProfessionSkillItem DoctorSkill0 => Instance[42];

		public static ProfessionSkillItem DoctorSkill1 => Instance[43];

		public static ProfessionSkillItem DoctorSkill2 => Instance[44];

		public static ProfessionSkillItem DoctorSkill3 => Instance[69];

		public static ProfessionSkillItem TravelingTaoistMonkSkill0 => Instance[45];

		public static ProfessionSkillItem TravelingTaoistMonkSkill1 => Instance[46];

		public static ProfessionSkillItem TravelingTaoistMonkSkill2 => Instance[47];

		public static ProfessionSkillItem TravelingTaoistMonkSkill3 => Instance[48];

		public static ProfessionSkillItem CapitalistSkill0 => Instance[49];

		public static ProfessionSkillItem CapitalistSkill1 => Instance[50];

		public static ProfessionSkillItem CapitalistSkill2 => Instance[51];

		public static ProfessionSkillItem CapitalistSkill3 => Instance[71];

		public static ProfessionSkillItem TeaTasterSkill0 => Instance[52];

		public static ProfessionSkillItem TeaTasterSkill1 => Instance[53];

		public static ProfessionSkillItem TeaTasterSkill2 => Instance[54];

		public static ProfessionSkillItem TeaTasterSkill3 => Instance[58];

		public static ProfessionSkillItem DukeSkill0 => Instance[55];

		public static ProfessionSkillItem DukeSkill1 => Instance[56];

		public static ProfessionSkillItem DukeSkill2 => Instance[57];

		public static ProfessionSkillItem DukeSkill3 => Instance[70];
	}

	public static ProfessionSkill Instance = new ProfessionSkill();

	private readonly HashSet<string> RequiredFields = new HashSet<string>
	{
		"Profession", "CharacterProperty", "ResourcesCost", "TemplateId", "Name", "Icon", "Desc", "FunctionalDesc", "Level", "UnlockSeniority",
		"SkillUnlockDesc", "SkillUnlockExplain"
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
		_dataArray.Add(new ProfessionSkillItem(0, 0, instant: false, 0, "profession_skillicon_0_0", 1, 2, 1, 0, EProfessionSkillTriggerType.Passive, ignoreCanExecuteSkill: false, EProfessionSkillType.Passive, 1, 0, costTimeWhenFinished: false, 5, 20, 10, 0, new List<ResourceInfo>(), 0, 3, 4));
		_dataArray.Add(new ProfessionSkillItem(1, 5, instant: true, 0, "profession_skillicon_0_1", 6, 7, 2, -1, EProfessionSkillTriggerType.Active, ignoreCanExecuteSkill: false, EProfessionSkillType.Active, 3, 10, costTimeWhenFinished: false, 20, 20, 10, 500, new List<ResourceInfo>(), 0, 8, 9));
		_dataArray.Add(new ProfessionSkillItem(2, 10, instant: false, 0, "profession_skillicon_0_2", 11, 12, 3, -1, EProfessionSkillTriggerType.Passive, ignoreCanExecuteSkill: false, EProfessionSkillType.Passive, 1, 0, costTimeWhenFinished: false, 50, 0, 0, 0, new List<ResourceInfo>(), 0, 13, 14));
		_dataArray.Add(new ProfessionSkillItem(3, 20, instant: false, 1, "profession_skillicon_1_1", 21, 22, 1, -1, EProfessionSkillTriggerType.Passive, ignoreCanExecuteSkill: false, EProfessionSkillType.Passive, 1, 0, costTimeWhenFinished: false, 15, 40, 20, 0, new List<ResourceInfo>(), 0, 23, 24));
		_dataArray.Add(new ProfessionSkillItem(4, 25, instant: false, 1, "profession_skillicon_1_2", 26, 27, 2, -1, EProfessionSkillTriggerType.Passive, ignoreCanExecuteSkill: false, EProfessionSkillType.Passive, 1, 0, costTimeWhenFinished: false, 30, 0, 0, 0, new List<ResourceInfo>(), 0, 28, 29));
		_dataArray.Add(new ProfessionSkillItem(5, 30, instant: true, 1, "profession_skillicon_1_0", 31, 32, 3, -1, EProfessionSkillTriggerType.Active, ignoreCanExecuteSkill: false, EProfessionSkillType.Active, 1, 10, costTimeWhenFinished: false, 75, 20, 10, 0, new List<ResourceInfo>
		{
			new ResourceInfo(0, 100)
		}, 0, 33, 34));
		_dataArray.Add(new ProfessionSkillItem(6, 40, instant: false, 2, "profession_skillicon_2_0", 41, 42, 1, -1, EProfessionSkillTriggerType.Passive, ignoreCanExecuteSkill: false, EProfessionSkillType.Passive, 1, 0, costTimeWhenFinished: false, 15, 0, 0, 0, new List<ResourceInfo>(), 0, 43, 44));
		_dataArray.Add(new ProfessionSkillItem(7, 45, instant: false, 2, "profession_skillicon_2_1", 46, 47, 2, -1, EProfessionSkillTriggerType.Passive, ignoreCanExecuteSkill: false, EProfessionSkillType.Passive, 1, 0, costTimeWhenFinished: false, 30, 60, 30, 0, new List<ResourceInfo>(), 0, 48, 49));
		_dataArray.Add(new ProfessionSkillItem(8, 50, instant: false, 2, "profession_skillicon_2_2", 51, 52, 3, -1, EProfessionSkillTriggerType.Active, ignoreCanExecuteSkill: false, EProfessionSkillType.Active, 3, 5, costTimeWhenFinished: false, 75, 0, 0, 0, new List<ResourceInfo>
		{
			new ResourceInfo(1, 2500),
			new ResourceInfo(2, 2500),
			new ResourceInfo(3, 2500),
			new ResourceInfo(4, 2500)
		}, 0, 53, 54));
		_dataArray.Add(new ProfessionSkillItem(9, 60, instant: false, 3, "profession_skillicon_3_0", 61, 62, 1, -1, EProfessionSkillTriggerType.Passive, ignoreCanExecuteSkill: false, EProfessionSkillType.Passive, 1, 0, costTimeWhenFinished: false, 5, 0, 0, 0, new List<ResourceInfo>(), 0, 63, 64));
		_dataArray.Add(new ProfessionSkillItem(10, 65, instant: false, 3, "profession_skillicon_3_1", 66, 67, 2, -1, EProfessionSkillTriggerType.Passive, ignoreCanExecuteSkill: false, EProfessionSkillType.Passive, 1, 0, costTimeWhenFinished: false, 20, 0, 0, 0, new List<ResourceInfo>(), 0, 68, 69));
		_dataArray.Add(new ProfessionSkillItem(11, 70, instant: false, 3, "profession_skillicon_3_2", 71, 72, 3, -1, EProfessionSkillTriggerType.Passive, ignoreCanExecuteSkill: false, EProfessionSkillType.Passive, 1, 0, costTimeWhenFinished: false, 50, 0, 0, 0, new List<ResourceInfo>(), 0, 73, 74));
		_dataArray.Add(new ProfessionSkillItem(12, 80, instant: false, 4, "profession_skillicon_4_0", 81, 82, 1, -1, EProfessionSkillTriggerType.Interactive, ignoreCanExecuteSkill: false, EProfessionSkillType.Interactive, 1, 5, costTimeWhenFinished: false, 5, 20, 10, 500, new List<ResourceInfo>(), 0, 83, 84));
		_dataArray.Add(new ProfessionSkillItem(13, 85, instant: false, 4, "profession_skillicon_4_1", 86, 87, 2, -1, EProfessionSkillTriggerType.Passive, ignoreCanExecuteSkill: false, EProfessionSkillType.Passive, 1, 0, costTimeWhenFinished: false, 20, 40, 20, 0, new List<ResourceInfo>(), 0, 88, 89));
		_dataArray.Add(new ProfessionSkillItem(14, 90, instant: false, 4, "profession_skillicon_4_2", 91, 92, 3, -1, EProfessionSkillTriggerType.Active, ignoreCanExecuteSkill: false, EProfessionSkillType.Active, 3, 5, costTimeWhenFinished: false, 50, 0, 0, 0, new List<ResourceInfo>(), 0, 93, 94));
		_dataArray.Add(new ProfessionSkillItem(15, 100, instant: false, 5, "profession_skillicon_5_0", 101, 102, 1, 4, EProfessionSkillTriggerType.Passive, ignoreCanExecuteSkill: false, EProfessionSkillType.Passive, 1, 0, costTimeWhenFinished: false, 5, 20, 10, 0, new List<ResourceInfo>(), 0, 103, 104));
		_dataArray.Add(new ProfessionSkillItem(16, 105, instant: false, 5, "profession_skillicon_5_1", 106, 107, 2, -1, EProfessionSkillTriggerType.Interactive, ignoreCanExecuteSkill: false, EProfessionSkillType.Interactive, 3, 10, costTimeWhenFinished: false, 20, 40, 20, 5000, new List<ResourceInfo>(), 0, 108, 109));
		_dataArray.Add(new ProfessionSkillItem(17, 110, instant: false, 5, "profession_skillicon_5_2", 111, 112, 3, -1, EProfessionSkillTriggerType.Passive, ignoreCanExecuteSkill: false, EProfessionSkillType.Passive, 1, 0, costTimeWhenFinished: false, 50, 0, 0, 0, new List<ResourceInfo>(), 0, 113, 114));
		_dataArray.Add(new ProfessionSkillItem(18, 115, instant: true, 5, "profession_skillicon_5_3", 116, 117, 4, -1, EProfessionSkillTriggerType.Active, ignoreCanExecuteSkill: false, EProfessionSkillType.Active, 3, 20, costTimeWhenFinished: false, 100, 0, 0, 0, new List<ResourceInfo>(), 0, 118, 119));
		_dataArray.Add(new ProfessionSkillItem(19, 120, instant: false, 6, "profession_skillicon_6_0", 121, 122, 1, 2, EProfessionSkillTriggerType.Passive, ignoreCanExecuteSkill: false, EProfessionSkillType.Passive, 1, 0, costTimeWhenFinished: false, 5, 20, 10, 0, new List<ResourceInfo>(), 0, 123, 124));
		_dataArray.Add(new ProfessionSkillItem(20, 125, instant: false, 6, "profession_skillicon_6_1", 126, 127, 2, -1, EProfessionSkillTriggerType.Interactive, ignoreCanExecuteSkill: false, EProfessionSkillType.Interactive, 1, 10, costTimeWhenFinished: false, 20, 20, 10, 5000, new List<ResourceInfo>
		{
			new ResourceInfo(7, 1000)
		}, 0, 128, 129));
		_dataArray.Add(new ProfessionSkillItem(21, 130, instant: false, 6, "profession_skillicon_6_2", 131, 132, 3, -1, EProfessionSkillTriggerType.Active, ignoreCanExecuteSkill: false, EProfessionSkillType.Interactive, 3, 15, costTimeWhenFinished: false, 50, 0, 0, 15000, new List<ResourceInfo>(), 0, 133, 134));
		_dataArray.Add(new ProfessionSkillItem(22, 135, instant: false, 6, "profession_skillicon_6_3", 136, 137, 4, -1, EProfessionSkillTriggerType.Passive, ignoreCanExecuteSkill: false, EProfessionSkillType.Active, 1, 0, costTimeWhenFinished: false, 100, 0, 0, 0, new List<ResourceInfo>(), 0, 138, 139));
		_dataArray.Add(new ProfessionSkillItem(23, 140, instant: false, 7, "profession_skillicon_7_0", 141, 142, 1, -1, EProfessionSkillTriggerType.Interactive, ignoreCanExecuteSkill: false, EProfessionSkillType.Interactive, 1, 5, costTimeWhenFinished: false, 5, 0, 0, 0, new List<ResourceInfo>
		{
			new ResourceInfo(7, 100)
		}, 0, 143, 144));
		_dataArray.Add(new ProfessionSkillItem(24, 145, instant: false, 7, "profession_skillicon_7_1", 146, 147, 2, -1, EProfessionSkillTriggerType.Passive, ignoreCanExecuteSkill: false, EProfessionSkillType.Passive, 1, 0, costTimeWhenFinished: false, 20, 0, 0, 0, new List<ResourceInfo>(), 0, 148, 149));
		_dataArray.Add(new ProfessionSkillItem(25, 150, instant: false, 7, "profession_skillicon_7_2", 151, 152, 3, -1, EProfessionSkillTriggerType.Passive, ignoreCanExecuteSkill: false, EProfessionSkillType.Passive, 1, 0, costTimeWhenFinished: false, 50, 0, 0, 0, new List<ResourceInfo>(), 0, 153, 154));
		_dataArray.Add(new ProfessionSkillItem(26, 160, instant: false, 8, "profession_skillicon_8_0", 161, 162, 1, -1, EProfessionSkillTriggerType.Interactive, ignoreCanExecuteSkill: false, EProfessionSkillType.Interactive, 1, 10, costTimeWhenFinished: false, 15, 20, 10, 0, new List<ResourceInfo>
		{
			new ResourceInfo(7, 100)
		}, 0, 163, 164));
		_dataArray.Add(new ProfessionSkillItem(27, 165, instant: false, 8, "profession_skillicon_8_1", 166, 167, 2, -1, EProfessionSkillTriggerType.Active, ignoreCanExecuteSkill: false, EProfessionSkillType.Interactive, 3, 10, costTimeWhenFinished: false, 30, 20, 10, 5000, new List<ResourceInfo>(), 0, 168, 169));
		_dataArray.Add(new ProfessionSkillItem(28, 170, instant: false, 8, "profession_skillicon_8_2", 171, 172, 3, -1, EProfessionSkillTriggerType.Passive, ignoreCanExecuteSkill: false, EProfessionSkillType.Passive, 1, 0, costTimeWhenFinished: false, 75, 0, 0, 0, new List<ResourceInfo>(), 0, 173, 174));
		_dataArray.Add(new ProfessionSkillItem(29, 180, instant: false, 9, "profession_skillicon_9_0", 181, 182, 1, -1, EProfessionSkillTriggerType.Active, ignoreCanExecuteSkill: false, EProfessionSkillType.Interactive, 1, 10, costTimeWhenFinished: false, 5, 20, 10, 0, new List<ResourceInfo>
		{
			new ResourceInfo(7, 10)
		}, 0, 183, 184));
		_dataArray.Add(new ProfessionSkillItem(30, 185, instant: true, 9, "profession_skillicon_9_1", 186, 187, 2, -1, EProfessionSkillTriggerType.Active, ignoreCanExecuteSkill: false, EProfessionSkillType.Active, 1, 5, costTimeWhenFinished: false, 20, 20, 10, 0, new List<ResourceInfo>
		{
			new ResourceInfo(7, 100)
		}, 0, 188, 189));
		_dataArray.Add(new ProfessionSkillItem(31, 190, instant: false, 9, "profession_skillicon_9_2", 191, 192, 3, -1, EProfessionSkillTriggerType.Active, ignoreCanExecuteSkill: false, EProfessionSkillType.Interactive, 3, 10, costTimeWhenFinished: false, 50, 0, 0, 0, new List<ResourceInfo>
		{
			new ResourceInfo(7, 1000)
		}, 0, 193, 194));
		_dataArray.Add(new ProfessionSkillItem(32, 200, instant: false, 10, "profession_skillicon_10_0", 201, 202, 1, -1, EProfessionSkillTriggerType.Passive, ignoreCanExecuteSkill: false, EProfessionSkillType.Passive, 1, 0, costTimeWhenFinished: false, 5, 0, 0, 0, new List<ResourceInfo>(), 0, 203, 204));
		_dataArray.Add(new ProfessionSkillItem(33, 205, instant: true, 10, "profession_skillicon_10_1", 206, 207, 2, -1, EProfessionSkillTriggerType.Active, ignoreCanExecuteSkill: false, EProfessionSkillType.Active, 12, 10, costTimeWhenFinished: false, 20, 720, 360, 2500, new List<ResourceInfo>
		{
			new ResourceInfo(7, 1000)
		}, 0, 208, 209));
		_dataArray.Add(new ProfessionSkillItem(34, 210, instant: false, 10, "profession_skillicon_10_2", 211, 212, 3, -1, EProfessionSkillTriggerType.Interactive, ignoreCanExecuteSkill: false, EProfessionSkillType.Interactive, 36, 10, costTimeWhenFinished: false, 50, 0, 0, 25000, new List<ResourceInfo>
		{
			new ResourceInfo(7, 5000)
		}, 0, 213, 214));
		_dataArray.Add(new ProfessionSkillItem(35, 220, instant: true, 11, "profession_skillicon_11_0", 221, 222, 1, 3, EProfessionSkillTriggerType.Passive, ignoreCanExecuteSkill: false, EProfessionSkillType.Passive, 1, 0, costTimeWhenFinished: false, 5, 20, 10, 0, new List<ResourceInfo>(), 0, 223, 224));
		_dataArray.Add(new ProfessionSkillItem(36, 225, instant: true, 11, "profession_skillicon_11_1", 226, 227, 2, -1, EProfessionSkillTriggerType.Passive, ignoreCanExecuteSkill: false, EProfessionSkillType.Passive, 1, 0, costTimeWhenFinished: false, 20, 20, 10, 0, new List<ResourceInfo>(), 0, 228, 229));
		_dataArray.Add(new ProfessionSkillItem(37, 230, instant: false, 11, "profession_skillicon_11_2", 231, 232, 3, -1, EProfessionSkillTriggerType.Active, ignoreCanExecuteSkill: false, EProfessionSkillType.Active, 1, 0, costTimeWhenFinished: false, 50, 0, 0, 2500, new List<ResourceInfo>(), 0, 233, 234));
		_dataArray.Add(new ProfessionSkillItem(38, 240, instant: true, 12, "profession_skillicon_12_0", 241, 242, 1, 5, EProfessionSkillTriggerType.Passive, ignoreCanExecuteSkill: false, EProfessionSkillType.Passive, 1, 0, costTimeWhenFinished: false, 5, 20, 10, 0, new List<ResourceInfo>(), 0, 243, 244));
		_dataArray.Add(new ProfessionSkillItem(39, 245, instant: false, 12, "profession_skillicon_12_1", 246, 247, 2, -1, EProfessionSkillTriggerType.Interactive, ignoreCanExecuteSkill: false, EProfessionSkillType.Interactive, 1, 5, costTimeWhenFinished: false, 20, 20, 10, 500, new List<ResourceInfo>(), 0, 248, 249));
		_dataArray.Add(new ProfessionSkillItem(40, 250, instant: false, 12, "profession_skillicon_12_2", 251, 252, 3, -1, EProfessionSkillTriggerType.Active, ignoreCanExecuteSkill: false, EProfessionSkillType.Interactive, 24, 0, costTimeWhenFinished: false, 50, 0, 0, 0, new List<ResourceInfo>(), 0, 253, 254));
		_dataArray.Add(new ProfessionSkillItem(41, 255, instant: false, 12, "profession_skillicon_12_3", 256, 257, 4, -1, EProfessionSkillTriggerType.Passive, ignoreCanExecuteSkill: false, EProfessionSkillType.Passive, 1, 0, costTimeWhenFinished: false, 100, 0, 0, 0, new List<ResourceInfo>(), 0, 258, 259));
		_dataArray.Add(new ProfessionSkillItem(42, 260, instant: false, 13, "profession_skillicon_13_0", 261, 262, 1, -1, EProfessionSkillTriggerType.Interactive, ignoreCanExecuteSkill: false, EProfessionSkillType.Interactive, 0, 5, costTimeWhenFinished: false, 5, 20, 10, 0, new List<ResourceInfo>
		{
			new ResourceInfo(5, 100)
		}, 0, 263, 264));
		_dataArray.Add(new ProfessionSkillItem(43, 265, instant: true, 13, "profession_skillicon_13_1", 266, 267, 2, -1, EProfessionSkillTriggerType.Active, ignoreCanExecuteSkill: false, EProfessionSkillType.Active, 3, 10, costTimeWhenFinished: false, 30, 60, 30, 0, new List<ResourceInfo>
		{
			new ResourceInfo(5, 2500)
		}, 0, 268, 269));
		_dataArray.Add(new ProfessionSkillItem(44, 270, instant: false, 13, "profession_skillicon_13_2", 271, 272, 3, -1, EProfessionSkillTriggerType.Passive, ignoreCanExecuteSkill: false, EProfessionSkillType.Passive, 1, 0, costTimeWhenFinished: false, 50, 0, 0, 0, new List<ResourceInfo>(), 0, 273, 274));
		_dataArray.Add(new ProfessionSkillItem(45, 280, instant: true, 14, "profession_skillicon_14_0", 281, 282, 1, 1, EProfessionSkillTriggerType.Passive, ignoreCanExecuteSkill: false, EProfessionSkillType.Passive, 1, 0, costTimeWhenFinished: false, 5, 20, 10, 0, new List<ResourceInfo>(), 0, 283, 284));
		_dataArray.Add(new ProfessionSkillItem(46, 285, instant: false, 14, "profession_skillicon_14_1", 286, 287, 2, -1, EProfessionSkillTriggerType.Interactive, ignoreCanExecuteSkill: false, EProfessionSkillType.Interactive, 3, 5, costTimeWhenFinished: false, 20, 20, 10, 2500, new List<ResourceInfo>(), 0, 288, 289));
		_dataArray.Add(new ProfessionSkillItem(47, 290, instant: false, 14, "profession_skillicon_14_2", 291, 292, 3, -1, EProfessionSkillTriggerType.Interactive, ignoreCanExecuteSkill: false, EProfessionSkillType.Active, 12, 10, costTimeWhenFinished: false, 50, 0, 0, 50000, new List<ResourceInfo>(), 0, 293, 294));
		_dataArray.Add(new ProfessionSkillItem(48, 295, instant: false, 14, "profession_skillicon_14_3", 296, 297, 4, -1, EProfessionSkillTriggerType.Passive, ignoreCanExecuteSkill: false, EProfessionSkillType.Passive, 1, 0, costTimeWhenFinished: false, 100, 0, 0, 0, new List<ResourceInfo>(), 0, 298, 299));
		_dataArray.Add(new ProfessionSkillItem(49, 300, instant: false, 15, "profession_skillicon_15_0", 301, 302, 1, -1, EProfessionSkillTriggerType.Interactive, ignoreCanExecuteSkill: false, EProfessionSkillType.Interactive, 1, 5, costTimeWhenFinished: false, 15, 20, 10, 0, new List<ResourceInfo>(), 14000, 303, 304));
		_dataArray.Add(new ProfessionSkillItem(50, 305, instant: false, 15, "profession_skillicon_15_1", 306, 307, 2, -1, EProfessionSkillTriggerType.Active, ignoreCanExecuteSkill: false, EProfessionSkillType.Interactive, 1, 5, costTimeWhenFinished: false, 30, 20, 10, 0, new List<ResourceInfo>
		{
			new ResourceInfo(7, 500)
		}, 0, 308, 309));
		_dataArray.Add(new ProfessionSkillItem(51, 310, instant: false, 15, "profession_skillicon_15_2", 311, 312, 3, -1, EProfessionSkillTriggerType.Active, ignoreCanExecuteSkill: false, EProfessionSkillType.Active, 0, 0, costTimeWhenFinished: false, 75, 0, 0, 0, new List<ResourceInfo>(), 0, 313, 314));
		_dataArray.Add(new ProfessionSkillItem(52, 320, instant: false, 16, "profession_skillicon_16_0", 321, 322, 1, -1, EProfessionSkillTriggerType.Interactive, ignoreCanExecuteSkill: false, EProfessionSkillType.Interactive, 1, 5, costTimeWhenFinished: false, 5, 0, 0, 0, new List<ResourceInfo>
		{
			new ResourceInfo(7, 100)
		}, 0, 323, 324));
		_dataArray.Add(new ProfessionSkillItem(53, 325, instant: false, 16, "profession_skillicon_16_1", 326, 327, 2, -1, EProfessionSkillTriggerType.Passive, ignoreCanExecuteSkill: false, EProfessionSkillType.Passive, 1, 0, costTimeWhenFinished: false, 20, 0, 0, 0, new List<ResourceInfo>(), 0, 328, 329));
		_dataArray.Add(new ProfessionSkillItem(54, 330, instant: false, 16, "profession_skillicon_16_2", 331, 332, 3, -1, EProfessionSkillTriggerType.Passive, ignoreCanExecuteSkill: false, EProfessionSkillType.Passive, 1, 0, costTimeWhenFinished: false, 50, 0, 0, 0, new List<ResourceInfo>(), 0, 333, 334));
		_dataArray.Add(new ProfessionSkillItem(55, 340, instant: false, 17, "profession_skillicon_17_0", 341, 342, 1, -1, EProfessionSkillTriggerType.Active, ignoreCanExecuteSkill: false, EProfessionSkillType.Interactive, 1, 5, costTimeWhenFinished: false, 10, 20, 10, 1000, new List<ResourceInfo>(), 0, 343, 344));
		_dataArray.Add(new ProfessionSkillItem(56, 345, instant: false, 17, "profession_skillicon_17_1", 346, 347, 2, -1, EProfessionSkillTriggerType.Interactive, ignoreCanExecuteSkill: false, EProfessionSkillType.Interactive, 1, 10, costTimeWhenFinished: false, 30, 120, 60, 0, new List<ResourceInfo>
		{
			new ResourceInfo(7, 2500)
		}, 0, 348, 349));
		_dataArray.Add(new ProfessionSkillItem(57, 350, instant: false, 17, "profession_skillicon_17_2", 351, 352, 3, -1, EProfessionSkillTriggerType.Active, ignoreCanExecuteSkill: false, EProfessionSkillType.Active, 6, 10, costTimeWhenFinished: false, 75, 0, 0, 25000, new List<ResourceInfo>(), 0, 353, 354));
		_dataArray.Add(new ProfessionSkillItem(58, 335, instant: false, 16, "profession_skillicon_16_3", 336, 337, 4, -1, EProfessionSkillTriggerType.Active, ignoreCanExecuteSkill: false, EProfessionSkillType.Active, 12, 20, costTimeWhenFinished: false, 100, 0, 0, 25000, new List<ResourceInfo>
		{
			new ResourceInfo(7, 5000)
		}, 0, 338, 339));
		_dataArray.Add(new ProfessionSkillItem(59, 215, instant: false, 10, "profession_skillicon_10_3", 216, 217, 4, -1, EProfessionSkillTriggerType.Passive, ignoreCanExecuteSkill: false, EProfessionSkillType.Passive, 1, 0, costTimeWhenFinished: false, 100, 0, 0, 0, new List<ResourceInfo>(), 0, 218, 219));
	}

	private void CreateItems1()
	{
		_dataArray.Add(new ProfessionSkillItem(60, 15, instant: false, 0, "profession_skillicon_0_3", 16, 17, 4, -1, EProfessionSkillTriggerType.Active, ignoreCanExecuteSkill: false, EProfessionSkillType.Active, 3, 0, costTimeWhenFinished: false, 100, 0, 0, 5000, new List<ResourceInfo>(), 0, 18, 19));
		_dataArray.Add(new ProfessionSkillItem(61, 35, instant: false, 1, "profession_skillicon_1_3", 36, 37, 4, -1, EProfessionSkillTriggerType.Active, ignoreCanExecuteSkill: false, EProfessionSkillType.Interactive, 6, 10, costTimeWhenFinished: false, 100, 0, 0, 25000, new List<ResourceInfo>
		{
			new ResourceInfo(0, 10000)
		}, 0, 38, 39));
		_dataArray.Add(new ProfessionSkillItem(62, 55, instant: false, 2, "profession_skillicon_2_3", 56, 57, 4, -1, EProfessionSkillTriggerType.Passive, ignoreCanExecuteSkill: false, EProfessionSkillType.Passive, 1, 0, costTimeWhenFinished: false, 100, 0, 0, 0, new List<ResourceInfo>(), 0, 58, 59));
		_dataArray.Add(new ProfessionSkillItem(63, 75, instant: true, 3, "profession_skillicon_3_3", 76, 77, 4, -1, EProfessionSkillTriggerType.Active, ignoreCanExecuteSkill: false, EProfessionSkillType.Active, 3, 10, costTimeWhenFinished: false, 100, 0, 0, 0, new List<ResourceInfo>
		{
			new ResourceInfo(7, 2500)
		}, 0, 78, 79));
		_dataArray.Add(new ProfessionSkillItem(64, 95, instant: false, 4, "profession_skillicon_4_3", 96, 97, 4, -1, EProfessionSkillTriggerType.Active, ignoreCanExecuteSkill: false, EProfessionSkillType.Active, 6, 10, costTimeWhenFinished: false, 100, 0, 0, 25000, new List<ResourceInfo>(), 0, 98, 99));
		_dataArray.Add(new ProfessionSkillItem(65, 155, instant: false, 7, "profession_skillicon_7_3", 156, 157, 4, -1, EProfessionSkillTriggerType.Active, ignoreCanExecuteSkill: false, EProfessionSkillType.Active, 12, 20, costTimeWhenFinished: false, 100, 0, 0, 25000, new List<ResourceInfo>
		{
			new ResourceInfo(7, 5000)
		}, 0, 158, 159));
		_dataArray.Add(new ProfessionSkillItem(66, 175, instant: true, 8, "profession_skillicon_8_3", 176, 177, 4, -1, EProfessionSkillTriggerType.Active, ignoreCanExecuteSkill: false, EProfessionSkillType.Active, 6, 20, costTimeWhenFinished: false, 100, 0, 0, 0, new List<ResourceInfo>
		{
			new ResourceInfo(7, 5000)
		}, 0, 178, 179));
		_dataArray.Add(new ProfessionSkillItem(67, 195, instant: false, 9, "profession_skillicon_9_3", 196, 197, 4, -1, EProfessionSkillTriggerType.Active, ignoreCanExecuteSkill: false, EProfessionSkillType.Active, 0, 1, costTimeWhenFinished: false, 100, 0, 0, 0, new List<ResourceInfo>
		{
			new ResourceInfo(7, 500)
		}, 0, 198, 199));
		_dataArray.Add(new ProfessionSkillItem(68, 235, instant: false, 11, "profession_skillicon_11_3", 236, 237, 4, -1, EProfessionSkillTriggerType.Active, ignoreCanExecuteSkill: true, EProfessionSkillType.Active, 0, 0, costTimeWhenFinished: false, 100, 0, 0, 0, new List<ResourceInfo>
		{
			new ResourceInfo(0, 10000),
			new ResourceInfo(1, 10000),
			new ResourceInfo(2, 10000),
			new ResourceInfo(3, 10000),
			new ResourceInfo(4, 10000),
			new ResourceInfo(5, 10000)
		}, 0, 238, 239));
		_dataArray.Add(new ProfessionSkillItem(69, 275, instant: false, 13, "profession_skillicon_13_3", 276, 277, 4, -1, EProfessionSkillTriggerType.Interactive, ignoreCanExecuteSkill: false, EProfessionSkillType.Interactive, 1, 10, costTimeWhenFinished: false, 100, 0, 0, 0, new List<ResourceInfo>(), 0, 278, 279));
		_dataArray.Add(new ProfessionSkillItem(70, 355, instant: true, 17, "profession_skillicon_17_3", 356, 357, 4, -1, EProfessionSkillTriggerType.Active, ignoreCanExecuteSkill: false, EProfessionSkillType.Active, 12, 5, costTimeWhenFinished: false, 100, 0, 0, 0, new List<ResourceInfo>
		{
			new ResourceInfo(7, 5000)
		}, 0, 358, 359));
		_dataArray.Add(new ProfessionSkillItem(71, 315, instant: false, 15, "profession_skillicon_15_3", 316, 317, 4, -1, EProfessionSkillTriggerType.Passive, ignoreCanExecuteSkill: false, EProfessionSkillType.Passive, 1, 0, costTimeWhenFinished: false, 100, 0, 0, 0, new List<ResourceInfo>(), 0, 318, 319));
	}

	public override void Init()
	{
		base.Init();
		_dataArray = new List<ProfessionSkillItem>(72);
		CreateItems0();
		CreateItems1();
	}
}
