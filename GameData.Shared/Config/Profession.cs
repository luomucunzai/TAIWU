using System;
using System.Collections.Generic;
using Config.Common;

namespace Config;

[Serializable]
public class Profession : ConfigData<ProfessionItem, int>
{
	public static class DefKey
	{
		public const int Savage = 0;

		public const int Hunter = 1;

		public const int Craft = 2;

		public const int MartialArtist = 3;

		public const int Literati = 4;

		public const int TaoistMonk = 5;

		public const int BuddhistMonk = 6;

		public const int WineTaster = 7;

		public const int Aristocrat = 8;

		public const int Beggar = 9;

		public const int Civilian = 10;

		public const int Traveler = 11;

		public const int TravelingBuddhistMonk = 12;

		public const int Doctor = 13;

		public const int TravelingTaoistMonk = 14;

		public const int Capitalist = 15;

		public const int TeaTaster = 16;

		public const int Duke = 17;
	}

	public static class DefValue
	{
		public static ProfessionItem Savage => Instance[0];

		public static ProfessionItem Hunter => Instance[1];

		public static ProfessionItem Craft => Instance[2];

		public static ProfessionItem MartialArtist => Instance[3];

		public static ProfessionItem Literati => Instance[4];

		public static ProfessionItem TaoistMonk => Instance[5];

		public static ProfessionItem BuddhistMonk => Instance[6];

		public static ProfessionItem WineTaster => Instance[7];

		public static ProfessionItem Aristocrat => Instance[8];

		public static ProfessionItem Beggar => Instance[9];

		public static ProfessionItem Civilian => Instance[10];

		public static ProfessionItem Traveler => Instance[11];

		public static ProfessionItem TravelingBuddhistMonk => Instance[12];

		public static ProfessionItem Doctor => Instance[13];

		public static ProfessionItem TravelingTaoistMonk => Instance[14];

		public static ProfessionItem Capitalist => Instance[15];

		public static ProfessionItem TeaTaster => Instance[16];

		public static ProfessionItem Duke => Instance[17];
	}

	public static Profession Instance = new Profession();

	private readonly HashSet<string> RequiredFields = new HashSet<string>
	{
		"ProfessionSkills", "ExtraProfessionSkill", "BonusLifeSkills", "BonusCombatSkills", "BonusClothing", "ConflictingProfessions", "CompatibleProfessions", "TemplateId", "Name", "Desc",
		"TextureBig", "Texture", "TextureSmall", "NameSprite", "ProfessionSeniorityPerMonth", "DemandTeachingText", "DemandTeachingFinishText"
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
		_dataArray.Add(new ProfessionItem(0, 0, 1, "Profession_0_0", "Profession_1_0", "Profession_2_0", "bottom_profession_mingcheng_0", new int[3] { 0, 1, 2 }, 60, new List<sbyte> { 12 }, new List<sbyte>(), 0, new List<int> { 8, 17, 7, 16 }, new List<int> { 1, 9, 10 }, forbidWine: false, forbidMeat: false, forbidSex: false, reinitOnCrossArchive: true, new int[7] { 2, 3, 4, 5, 6, 7, 8 }, new uint[7], new int[9] { 22800, 22940, 23144, 23400, 23550, 23800, 24160, 24200, 24480 }, 9, 10));
		_dataArray.Add(new ProfessionItem(1, 11, 12, "Profession_0_1", "Profession_1_1", "Profession_2_1", "bottom_profession_mingcheng_1", new int[3] { 3, 4, 5 }, 61, new List<sbyte> { 15 }, new List<sbyte>(), 1, new List<int> { 8, 17 }, new List<int> { 0, 9, 10 }, forbidWine: false, forbidMeat: false, forbidSex: false, reinitOnCrossArchive: true, new int[8] { 13, 14, 15, 16, 17, 18, 19, 20 }, new uint[8] { 0u, 0u, 0u, 0u, 0u, 0u, 2764950u, 2764950u }, new int[9] { 15600, 15688, 16280, 16800, 17400, 18800, 20160, 21200, 22080 }, 21, 22));
		_dataArray.Add(new ProfessionItem(2, 23, 24, "Profession_0_2", "Profession_1_2", "Profession_2_2", "bottom_profession_mingcheng_2", new int[3] { 6, 7, 8 }, 62, new List<sbyte> { 11, 6, 10, 7 }, new List<sbyte>(), 2, new List<int>(), new List<int>(), forbidWine: false, forbidMeat: false, forbidSex: false, reinitOnCrossArchive: true, new int[6] { 25, 26, 27, 28, 29, 30 }, new uint[6], new int[9] { 12000, 12580, 13200, 13800, 15000, 16000, 17600, 18400, 20160 }, 31, 32));
		_dataArray.Add(new ProfessionItem(3, 33, 34, "Profession_0_3", "Profession_1_3", "Profession_2_3", "bottom_profession_mingcheng_3", new int[3] { 9, 10, 11 }, 63, new List<sbyte>(), new List<sbyte>
		{
			0, 1, 2, 3, 4, 5, 6, 7, 8, 9,
			10, 11, 12, 13
		}, 3, new List<int> { 4 }, new List<int>(), forbidWine: false, forbidMeat: false, forbidSex: false, reinitOnCrossArchive: true, new int[8] { 35, 36, 37, 38, 39, 40, 41, 42 }, new uint[8], new int[9] { 6900, 7622, 8360, 9600, 12000, 13200, 14720, 16000, 18000 }, 43, 44));
		_dataArray.Add(new ProfessionItem(4, 45, 46, "Profession_0_4", "Profession_1_4", "Profession_2_4", "bottom_profession_mingcheng_4", new int[3] { 12, 13, 14 }, 64, new List<sbyte> { 0, 1, 2, 3 }, new List<sbyte>(), 4, new List<int> { 3 }, new List<int>(), forbidWine: false, forbidMeat: false, forbidSex: false, reinitOnCrossArchive: true, new int[6] { 47, 48, 49, 50, 51, 52 }, new uint[6], new int[9] { 6600, 7400, 8096, 9240, 11250, 13000, 14400, 15600, 17760 }, 53, 54));
		_dataArray.Add(new ProfessionItem(5, 55, 56, "Profession_0_5", "Profession_1_5", "Profession_2_5", "bottom_profession_mingcheng_5", new int[3] { 15, 16, 17 }, 18, new List<sbyte> { 12 }, new List<sbyte>(), 5, new List<int> { 6, 12 }, new List<int> { 14 }, forbidWine: false, forbidMeat: true, forbidSex: true, reinitOnCrossArchive: false, new int[6] { 57, 58, 59, 60, 61, 62 }, new uint[6], new int[9] { 3300, 3626, 4048, 4680, 5400, 7000, 9600, 11200, 14400 }, 63, 64));
		_dataArray.Add(new ProfessionItem(6, 65, 66, "Profession_0_6", "Profession_1_6", "Profession_2_6", "bottom_profession_mingcheng_6", new int[3] { 19, 20, 21 }, 22, new List<sbyte> { 13 }, new List<sbyte>(), 6, new List<int> { 5, 14 }, new List<int> { 12 }, forbidWine: true, forbidMeat: true, forbidSex: true, reinitOnCrossArchive: false, new int[6] { 67, 68, 69, 70, 71, 72 }, new uint[6], new int[9] { 3420, 3626, 3960, 4680, 5400, 7000, 9920, 11600, 14880 }, 73, 74));
		_dataArray.Add(new ProfessionItem(7, 75, 76, "Profession_0_7", "Profession_1_7", "Profession_2_7", "bottom_profession_mingcheng_7", new int[3] { 23, 24, 25 }, 65, new List<sbyte> { 5 }, new List<sbyte>(), 7, new List<int> { 9, 10, 16 }, new List<int> { 8 }, forbidWine: false, forbidMeat: false, forbidSex: false, reinitOnCrossArchive: true, new int[6] { 77, 78, 79, 80, 81, 82 }, new uint[6], new int[9] { 4200, 4588, 5192, 6000, 7050, 8400, 11200, 13200, 16800 }, 83, 84));
		_dataArray.Add(new ProfessionItem(8, 85, 86, "Profession_0_8", "Profession_1_8", "Profession_2_8", "bottom_profession_mingcheng_8", new int[3] { 26, 27, 28 }, 66, new List<sbyte> { 5 }, new List<sbyte>(), 8, new List<int> { 0, 1, 9, 10 }, new List<int> { 7, 17 }, forbidWine: false, forbidMeat: false, forbidSex: false, reinitOnCrossArchive: true, new int[5] { 87, 88, 89, 90, 91 }, new uint[5], new int[9] { 3180, 3478, 3872, 4440, 5100, 6800, 9280, 10800, 13920 }, 92, 93));
		_dataArray.Add(new ProfessionItem(9, 94, 95, "Profession_0_9", "Profession_1_9", "Profession_2_9", "bottom_profession_mingcheng_9", new int[3] { 29, 30, 31 }, 67, new List<sbyte> { 15 }, new List<sbyte>(), 9, new List<int> { 8, 17, 7, 16 }, new List<int> { 0, 1, 10 }, forbidWine: false, forbidMeat: false, forbidSex: false, reinitOnCrossArchive: true, new int[5] { 96, 97, 98, 99, 100 }, new uint[5], new int[9] { 23400, 23458, 23760, 24000, 24300, 24400, 24640, 24800, 24960 }, 101, 102));
		_dataArray.Add(new ProfessionItem(10, 103, 104, "Profession_0_10", "Profession_1_10", "Profession_2_10", "bottom_profession_mingcheng_10", new int[3] { 32, 33, 34 }, 59, new List<sbyte> { 14 }, new List<sbyte>(), 10, new List<int> { 8, 17 }, new List<int> { 0, 1, 9 }, forbidWine: false, forbidMeat: false, forbidSex: false, reinitOnCrossArchive: true, new int[6] { 105, 106, 107, 108, 109, 110 }, new uint[6], new int[9] { 10800, 11840, 12496, 12960, 14100, 15000, 16960, 18000, 19680 }, 111, 112));
		_dataArray.Add(new ProfessionItem(11, 113, 114, "Profession_0_11", "Profession_1_11", "Profession_2_11", "bottom_profession_mingcheng_11", new int[3] { 35, 36, 37 }, 68, new List<sbyte> { 15 }, new List<sbyte>(), 11, new List<int>(), new List<int>(), forbidWine: false, forbidMeat: false, forbidSex: false, reinitOnCrossArchive: true, new int[6] { 115, 116, 117, 118, 119, 120 }, new uint[6], new int[9] { 17400, 17760, 18040, 18600, 19500, 22000, 22400, 23040, 24000 }, 121, 122));
		_dataArray.Add(new ProfessionItem(12, 123, 124, "Profession_0_12", "Profession_1_12", "Profession_2_12", "bottom_profession_mingcheng_12", new int[3] { 38, 39, 40 }, 41, new List<sbyte> { 13 }, new List<sbyte>(), 12, new List<int> { 5, 14 }, new List<int> { 6 }, forbidWine: true, forbidMeat: true, forbidSex: true, reinitOnCrossArchive: false, new int[5] { 125, 126, 127, 128, 129 }, new uint[5], new int[9] { 4500, 4958, 5544, 6360, 7500, 8900, 11680, 13800, 17040 }, 130, 131));
		_dataArray.Add(new ProfessionItem(13, 132, 133, "Profession_0_13", "Profession_1_13", "Profession_2_13", "bottom_profession_mingcheng_13", new int[3] { 42, 43, 44 }, 69, new List<sbyte> { 9, 8 }, new List<sbyte>(), 13, new List<int>(), new List<int>(), forbidWine: false, forbidMeat: false, forbidSex: false, reinitOnCrossArchive: true, new int[7] { 134, 135, 136, 137, 138, 139, 140 }, new uint[7], new int[9] { 9000, 9990, 10560, 12000, 13500, 15000, 16000, 16800, 18240 }, 141, 142));
		_dataArray.Add(new ProfessionItem(14, 143, 144, "Profession_0_14", "Profession_1_14", "Profession_2_14", "bottom_profession_mingcheng_14", new int[3] { 45, 46, 47 }, 48, new List<sbyte> { 4 }, new List<sbyte>(), 14, new List<int> { 6, 12 }, new List<int> { 5 }, forbidWine: false, forbidMeat: true, forbidSex: true, reinitOnCrossArchive: false, new int[5] { 145, 146, 147, 148, 149 }, new uint[5], new int[9] { 4440, 4884, 5456, 6240, 7350, 8800, 11520, 13600, 16800 }, 150, 151));
		_dataArray.Add(new ProfessionItem(15, 152, 153, "Profession_0_15", "Profession_1_15", "Profession_2_15", "bottom_profession_mingcheng_15", new int[3] { 49, 50, 51 }, 71, new List<sbyte> { 15 }, new List<sbyte>(), 15, new List<int>(), new List<int>(), forbidWine: false, forbidMeat: false, forbidSex: false, reinitOnCrossArchive: true, new int[4] { 154, 155, 156, 157 }, new uint[4], new int[9] { 3600, 3848, 4224, 4920, 5700, 7200, 10240, 12000, 15360 }, 158, 159));
		_dataArray.Add(new ProfessionItem(16, 160, 161, "Profession_0_16", "Profession_1_16", "Profession_2_16", "bottom_profession_mingcheng_16", new int[3] { 52, 53, 54 }, 58, new List<sbyte> { 5 }, new List<sbyte>(), 16, new List<int> { 0, 1, 7 }, new List<int> { 17 }, forbidWine: false, forbidMeat: false, forbidSex: false, reinitOnCrossArchive: true, new int[6] { 162, 163, 164, 165, 166, 167 }, new uint[6], new int[9] { 4080, 4440, 4928, 5760, 6750, 8400, 10880, 12800, 16320 }, 168, 169));
		_dataArray.Add(new ProfessionItem(17, 170, 171, "Profession_0_17", "Profession_1_17", "Profession_2_17", "bottom_profession_mingcheng_17", new int[3] { 55, 56, 57 }, 70, new List<sbyte> { 5 }, new List<sbyte>(), 17, new List<int> { 0, 1, 9, 10 }, new List<int> { 16, 8 }, forbidWine: false, forbidMeat: false, forbidSex: false, reinitOnCrossArchive: true, new int[6] { 172, 173, 174, 175, 176, 177 }, new uint[6], new int[9] { 3840, 4144, 4576, 5400, 6300, 7800, 10240, 12400, 15840 }, 178, 179));
	}

	public override void Init()
	{
		base.Init();
		_dataArray = new List<ProfessionItem>(18);
		CreateItems0();
	}
}
