using System;
using System.Collections.Generic;
using Config.Common;
using Config.ConfigCells;
using Config.ConfigCells.Character;
using GameData.Utilities;

namespace Config;

[Serializable]
public class Organization : ConfigData<OrganizationItem, sbyte>
{
	public static class DefKey
	{
		public const sbyte None = 0;

		public const sbyte Shaolin = 1;

		public const sbyte Emei = 2;

		public const sbyte Baihua = 3;

		public const sbyte Wudang = 4;

		public const sbyte Yuanshan = 5;

		public const sbyte Shixiang = 6;

		public const sbyte Ranshan = 7;

		public const sbyte Xuannv = 8;

		public const sbyte Zhujian = 9;

		public const sbyte Kongsang = 10;

		public const sbyte Jingang = 11;

		public const sbyte Wuxian = 12;

		public const sbyte Jieqing = 13;

		public const sbyte Fulong = 14;

		public const sbyte Xuehou = 15;

		public const sbyte Taiwu = 16;

		public const sbyte Heretic = 17;

		public const sbyte Righteous = 18;

		public const sbyte XiangshuMinion = 19;

		public const sbyte XiangshuInfected = 20;

		public const sbyte Jingcheng = 21;

		public const sbyte Chengdu = 22;

		public const sbyte Guizhou = 23;

		public const sbyte Xiangyang = 24;

		public const sbyte Taiyuan = 25;

		public const sbyte Guangzhou = 26;

		public const sbyte Qingzhou = 27;

		public const sbyte Jiangling = 28;

		public const sbyte Fuzhou = 29;

		public const sbyte Liaoyang = 30;

		public const sbyte Qinzhou = 31;

		public const sbyte Dali = 32;

		public const sbyte Shouchun = 33;

		public const sbyte Hangzhou = 34;

		public const sbyte Yangzhou = 35;

		public const sbyte Village = 36;

		public const sbyte Town = 37;

		public const sbyte WalledTown = 38;

		public const sbyte Beast = 40;
	}

	public static class DefValue
	{
		public static OrganizationItem None => Instance[(sbyte)0];

		public static OrganizationItem Shaolin => Instance[(sbyte)1];

		public static OrganizationItem Emei => Instance[(sbyte)2];

		public static OrganizationItem Baihua => Instance[(sbyte)3];

		public static OrganizationItem Wudang => Instance[(sbyte)4];

		public static OrganizationItem Yuanshan => Instance[(sbyte)5];

		public static OrganizationItem Shixiang => Instance[(sbyte)6];

		public static OrganizationItem Ranshan => Instance[(sbyte)7];

		public static OrganizationItem Xuannv => Instance[(sbyte)8];

		public static OrganizationItem Zhujian => Instance[(sbyte)9];

		public static OrganizationItem Kongsang => Instance[(sbyte)10];

		public static OrganizationItem Jingang => Instance[(sbyte)11];

		public static OrganizationItem Wuxian => Instance[(sbyte)12];

		public static OrganizationItem Jieqing => Instance[(sbyte)13];

		public static OrganizationItem Fulong => Instance[(sbyte)14];

		public static OrganizationItem Xuehou => Instance[(sbyte)15];

		public static OrganizationItem Taiwu => Instance[(sbyte)16];

		public static OrganizationItem Heretic => Instance[(sbyte)17];

		public static OrganizationItem Righteous => Instance[(sbyte)18];

		public static OrganizationItem XiangshuMinion => Instance[(sbyte)19];

		public static OrganizationItem XiangshuInfected => Instance[(sbyte)20];

		public static OrganizationItem Jingcheng => Instance[(sbyte)21];

		public static OrganizationItem Chengdu => Instance[(sbyte)22];

		public static OrganizationItem Guizhou => Instance[(sbyte)23];

		public static OrganizationItem Xiangyang => Instance[(sbyte)24];

		public static OrganizationItem Taiyuan => Instance[(sbyte)25];

		public static OrganizationItem Guangzhou => Instance[(sbyte)26];

		public static OrganizationItem Qingzhou => Instance[(sbyte)27];

		public static OrganizationItem Jiangling => Instance[(sbyte)28];

		public static OrganizationItem Fuzhou => Instance[(sbyte)29];

		public static OrganizationItem Liaoyang => Instance[(sbyte)30];

		public static OrganizationItem Qinzhou => Instance[(sbyte)31];

		public static OrganizationItem Dali => Instance[(sbyte)32];

		public static OrganizationItem Shouchun => Instance[(sbyte)33];

		public static OrganizationItem Hangzhou => Instance[(sbyte)34];

		public static OrganizationItem Yangzhou => Instance[(sbyte)35];

		public static OrganizationItem Village => Instance[(sbyte)36];

		public static OrganizationItem Town => Instance[(sbyte)37];

		public static OrganizationItem WalledTown => Instance[(sbyte)38];

		public static OrganizationItem Beast => Instance[(sbyte)40];
	}

	public static Organization Instance = new Organization();

	private readonly HashSet<string> RequiredFields = new HashSet<string>
	{
		"CharTemplateIds", "RandomEnemyTemplateIds", "MerchantTendency", "LegendaryBookTendency", "AbandonedBabyOrganizations", "PunishmentFeature", "TaiwuPunishementFeature", "LearnLifeSkillTypes", "CombatSkillTypes", "Members",
		"MemberFeature", "PrisonBuilding", "MartialArtistItemBonus", "TaskChains", "TaskReadyWorldState", "StoryGoodEndingsInformation", "StoryBadEndingsInformation", "TaiwuBeHunted", "SkillBreakBonusWeights", "LifeSkillCombatBriberyItemTypeWeight",
		"LifeSkillCombatBriberyItemSubTypeWeight", "VowFixedRewardItems", "VowRandomRewardItems", "TemplateId", "Name", "Desc", "TaiwuVillageSteleDesc", "StoryName", "UnlockStoryLogo", "UnlockStoryDesc",
		"OrganizationExtraDesc", "VowSpecialHint"
	};

	internal override int ToInt(sbyte value)
	{
		return value;
	}

	internal override sbyte ToTemplateId(int value)
	{
		return (sbyte)value;
	}

	private void CreateItems0()
	{
		_dataArray.Add(new OrganizationItem(0, 0, 1, 2, 0, 0, 0, new short[2] { -1, -1 }, new short[9] { -1, -1, -1, -1, -1, -1, -1, -1, -1 }, EOrganizationSettlementType.Invalid, isSect: false, isCivilian: true, -1, 0, short.MinValue, -1, 30, hereditary: true, allowPoisoning: true, noMeatEating: false, noDrinking: false, -1, 1, -1, new List<sbyte>(), -1, null, new List<sbyte>(), new List<sbyte>(), -1, -1, new sbyte[15], new short[9], 0, -1, 3, 4, 5, 6, -1, null, 7, null, -1, null, null, -1, new ShortPair[0], new List<LifeSkillCombatBriberyItemTypeWeight>(), new List<LifeSkillCombatBriberyItemSubTypeWeight>(), new List<PresetInventoryItem>(), new List<PresetInventoryItem>(), 0, -1));
		_dataArray.Add(new OrganizationItem(1, 8, 9, 10, 125, 150, 700, new short[2] { 0, 1 }, new short[9] { 323, 322, 321, 320, 319, 318, 317, 766, 316 }, EOrganizationSettlementType.Sect, isSect: true, isCivilian: false, 0, 1, 250, 1, 20, hereditary: false, allowPoisoning: false, noMeatEating: true, noDrinking: true, 1, 1, -1, new List<sbyte> { 2, 3, 4, 5, 16 }, 206, null, new List<sbyte> { 1, 8, 13 }, new List<sbyte> { 0, 1, 2, 3, 4, 9 }, 0, 18, new sbyte[15]
		{
			0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
			0, 0, 0, -1, 0
		}, new short[9] { 54, 53, 52, 51, 50, 49, 48, 47, 46 }, 1, 499, 11, 12, 13, 14, 303, new short[7] { 300, 273, 354, 678, 651, 660, 696 }, 15, new int[1] { 30 }, 22, new List<short> { 59 }, new List<short> { 60 }, 0, new ShortPair[3]
		{
			new ShortPair(14, 100),
			new ShortPair(1, 25),
			new ShortPair(37, 1)
		}, new List<LifeSkillCombatBriberyItemTypeWeight>
		{
			new LifeSkillCombatBriberyItemTypeWeight(0, 10),
			new LifeSkillCombatBriberyItemTypeWeight(1, 20),
			new LifeSkillCombatBriberyItemTypeWeight(2, 10),
			new LifeSkillCombatBriberyItemTypeWeight(4, 10),
			new LifeSkillCombatBriberyItemTypeWeight(7, 0)
		}, new List<LifeSkillCombatBriberyItemSubTypeWeight>
		{
			new LifeSkillCombatBriberyItemSubTypeWeight(1000, 40),
			new LifeSkillCombatBriberyItemSubTypeWeight(800, 10),
			new LifeSkillCombatBriberyItemSubTypeWeight(801, 0),
			new LifeSkillCombatBriberyItemSubTypeWeight(505, 0),
			new LifeSkillCombatBriberyItemSubTypeWeight(506, 0),
			new LifeSkillCombatBriberyItemSubTypeWeight(501, 0),
			new LifeSkillCombatBriberyItemSubTypeWeight(502, 0),
			new LifeSkillCombatBriberyItemSubTypeWeight(503, 0),
			new LifeSkillCombatBriberyItemSubTypeWeight(504, 0)
		}, new List<PresetInventoryItem>
		{
			new PresetInventoryItem("TeaWine", 20, 3, 100),
			new PresetInventoryItem("TeaWine", 29, 3, 100)
		}, new List<PresetInventoryItem>(), 0, 90));
		_dataArray.Add(new OrganizationItem(2, 16, 17, 18, 150, -18, 650, new short[2] { 2, 3 }, new short[9] { 331, 330, 329, 328, 327, 326, 325, 767, 324 }, EOrganizationSettlementType.Sect, isSect: true, isCivilian: false, 1, 1, 250, -1, 30, hereditary: true, allowPoisoning: true, noMeatEating: false, noDrinking: false, 0, 1, 10, new List<sbyte>(), -1, new List<short> { 560, 561, 562, 563, 564 }, new List<sbyte> { 12, 13, 14 }, new List<sbyte> { 0, 1, 2, 3, 4, 7, 10 }, 1, 9, new sbyte[15]
		{
			0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
			-1, 0, 0, 0, 0
		}, new short[9] { 63, 62, 61, 60, 59, 58, 57, 56, 55 }, 4, 500, 19, 20, 21, 22, 304, new short[10] { 354, 345, 372, 273, 327, 363, 435, 462, 480, 147 }, 23, new int[1] { 37 }, 23, new List<short> { 73 }, new List<short> { 72 }, 1, new ShortPair[8]
		{
			new ShortPair(14, 50),
			new ShortPair(15, 50),
			new ShortPair(12, 25),
			new ShortPair(24, 25),
			new ShortPair(23, 25),
			new ShortPair(35, 25),
			new ShortPair(36, 25),
			new ShortPair(37, 1)
		}, new List<LifeSkillCombatBriberyItemTypeWeight>
		{
			new LifeSkillCombatBriberyItemTypeWeight(0, 5),
			new LifeSkillCombatBriberyItemTypeWeight(1, 5),
			new LifeSkillCombatBriberyItemTypeWeight(2, 20),
			new LifeSkillCombatBriberyItemTypeWeight(4, 10),
			new LifeSkillCombatBriberyItemTypeWeight(7, 10)
		}, new List<LifeSkillCombatBriberyItemSubTypeWeight>
		{
			new LifeSkillCombatBriberyItemSubTypeWeight(1000, 40),
			new LifeSkillCombatBriberyItemSubTypeWeight(800, 10),
			new LifeSkillCombatBriberyItemSubTypeWeight(801, 0),
			new LifeSkillCombatBriberyItemSubTypeWeight(505, 0),
			new LifeSkillCombatBriberyItemSubTypeWeight(506, 0),
			new LifeSkillCombatBriberyItemSubTypeWeight(501, 0),
			new LifeSkillCombatBriberyItemSubTypeWeight(502, 0),
			new LifeSkillCombatBriberyItemSubTypeWeight(503, 0),
			new LifeSkillCombatBriberyItemSubTypeWeight(504, 0)
		}, new List<PresetInventoryItem>(), new List<PresetInventoryItem>
		{
			new PresetInventoryItem("Misc", 333, 1, 100),
			new PresetInventoryItem("Misc", 334, 1, 100),
			new PresetInventoryItem("Misc", 335, 1, 100),
			new PresetInventoryItem("Misc", 336, 1, 100),
			new PresetInventoryItem("Misc", 337, 1, 100),
			new PresetInventoryItem("Misc", 338, 1, 100),
			new PresetInventoryItem("Misc", 342, 1, 100),
			new PresetInventoryItem("Misc", 339, 1, 100),
			new PresetInventoryItem("Misc", 340, 1, 100),
			new PresetInventoryItem("Misc", 341, 1, 100)
		}, 3, 90));
		_dataArray.Add(new OrganizationItem(3, 24, 25, 26, -18, -18, 250, new short[2] { 4, 5 }, new short[9] { 339, 338, 337, 336, 335, 334, 333, 768, 332 }, EOrganizationSettlementType.Sect, isSect: true, isCivilian: false, -1, 1, 250, -1, 40, hereditary: false, allowPoisoning: true, noMeatEating: false, noDrinking: false, 4, 1, 4, new List<sbyte>(), -1, new List<short> { 565, 566, 567, 568, 569 }, new List<sbyte> { 0, 3, 8, 9, 10 }, new List<sbyte> { 0, 1, 2, 4, 12, 13 }, 2, 12, new sbyte[15]
		{
			0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
			0, 0, 0, 0, -1
		}, new short[9] { 72, 71, 70, 69, 68, 67, 66, 65, 64 }, 3, 501, 27, 28, 29, 30, 305, new short[7] { 363, 372, 93, 102, 84, 102, 255 }, 31, new int[3] { 48, 50, 51 }, 24, new List<short> { 107 }, new List<short> { 108 }, 2, new ShortPair[8]
		{
			new ShortPair(4, 100),
			new ShortPair(3, 25),
			new ShortPair(0, 25),
			new ShortPair(42, 25),
			new ShortPair(44, 25),
			new ShortPair(46, 25),
			new ShortPair(32, 25),
			new ShortPair(37, 1)
		}, new List<LifeSkillCombatBriberyItemTypeWeight>
		{
			new LifeSkillCombatBriberyItemTypeWeight(0, 5),
			new LifeSkillCombatBriberyItemTypeWeight(1, 0),
			new LifeSkillCombatBriberyItemTypeWeight(2, 0),
			new LifeSkillCombatBriberyItemTypeWeight(4, 0),
			new LifeSkillCombatBriberyItemTypeWeight(7, 0)
		}, new List<LifeSkillCombatBriberyItemSubTypeWeight>
		{
			new LifeSkillCombatBriberyItemSubTypeWeight(1000, 25),
			new LifeSkillCombatBriberyItemSubTypeWeight(800, 40),
			new LifeSkillCombatBriberyItemSubTypeWeight(801, 0),
			new LifeSkillCombatBriberyItemSubTypeWeight(505, 20),
			new LifeSkillCombatBriberyItemSubTypeWeight(506, 0),
			new LifeSkillCombatBriberyItemSubTypeWeight(501, 0),
			new LifeSkillCombatBriberyItemSubTypeWeight(502, 0),
			new LifeSkillCombatBriberyItemSubTypeWeight(503, 0),
			new LifeSkillCombatBriberyItemSubTypeWeight(504, 10)
		}, new List<PresetInventoryItem>
		{
			new PresetInventoryItem("Medicine", 60, 3, 100),
			new PresetInventoryItem("Medicine", 72, 3, 100)
		}, new List<PresetInventoryItem>(), 0, 70));
		_dataArray.Add(new OrganizationItem(4, 32, 33, 34, 150, 125, 600, new short[2] { 6, 7 }, new short[9] { 347, 346, 345, 344, 343, 342, 341, 769, 340 }, EOrganizationSettlementType.Sect, isSect: true, isCivilian: false, 2, 1, 0, -1, 30, hereditary: false, allowPoisoning: false, noMeatEating: false, noDrinking: false, 2, 1, 7, new List<sbyte>(), -1, new List<short> { 570, 571, 572, 573, 574 }, new List<sbyte> { 2, 4, 12 }, new List<sbyte> { 0, 1, 2, 3, 7, 11 }, 3, 15, new sbyte[15]
		{
			0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
			0, 0, -1, 0, 0
		}, new short[9] { 81, 80, 79, 78, 77, 76, 75, 74, 73 }, 1, 502, 35, 36, 37, 38, 306, new short[8] { 273, 354, 435, 480, 453, 489, 786, 777 }, 39, new int[2] { 32, 41 }, 25, new List<short> { 61 }, new List<short> { 62 }, 3, new ShortPair[6]
		{
			new ShortPair(15, 100),
			new ShortPair(2, 25),
			new ShortPair(19, 25),
			new ShortPair(26, 25),
			new ShortPair(30, 25),
			new ShortPair(37, 1)
		}, new List<LifeSkillCombatBriberyItemTypeWeight>
		{
			new LifeSkillCombatBriberyItemTypeWeight(0, 10),
			new LifeSkillCombatBriberyItemTypeWeight(1, 10),
			new LifeSkillCombatBriberyItemTypeWeight(2, 20),
			new LifeSkillCombatBriberyItemTypeWeight(4, 0),
			new LifeSkillCombatBriberyItemTypeWeight(7, 0)
		}, new List<LifeSkillCombatBriberyItemSubTypeWeight>
		{
			new LifeSkillCombatBriberyItemSubTypeWeight(1000, 40),
			new LifeSkillCombatBriberyItemSubTypeWeight(800, 20),
			new LifeSkillCombatBriberyItemSubTypeWeight(801, 0),
			new LifeSkillCombatBriberyItemSubTypeWeight(505, 0),
			new LifeSkillCombatBriberyItemSubTypeWeight(506, 0),
			new LifeSkillCombatBriberyItemSubTypeWeight(501, 0),
			new LifeSkillCombatBriberyItemSubTypeWeight(502, 0),
			new LifeSkillCombatBriberyItemSubTypeWeight(503, 0),
			new LifeSkillCombatBriberyItemSubTypeWeight(504, 0)
		}, new List<PresetInventoryItem>
		{
			new PresetInventoryItem("Medicine", 88, 3, 100)
		}, new List<PresetInventoryItem>(), 0, 90));
		_dataArray.Add(new OrganizationItem(5, 40, 41, 42, -18, 150, 800, new short[2] { 8, 9 }, new short[9] { 355, 354, 353, 352, 351, 350, 349, 770, 348 }, EOrganizationSettlementType.Sect, isSect: true, isCivilian: false, -1, 1, 500, -1, 20, hereditary: false, allowPoisoning: false, noMeatEating: true, noDrinking: false, 0, 1, 5, new List<sbyte>(), -1, null, new List<sbyte> { 8, 9, 12, 13, 14 }, new List<sbyte> { 0, 1, 2, 5, 7, 8 }, 4, 18, new sbyte[15]
		{
			0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
			0, -1, 0, 0, 0
		}, new short[9] { 90, 89, 88, 87, 86, 85, 84, 83, 82 }, 0, 503, 43, 44, 45, 46, 307, new short[7] { 453, 471, 498, 570, 588, 525, 579 }, 47, new int[1] { 33 }, 26, new List<short> { 65 }, new List<short> { 66 }, 4, new ShortPair[3]
		{
			new ShortPair(14, 50),
			new ShortPair(15, 50),
			new ShortPair(37, 1)
		}, new List<LifeSkillCombatBriberyItemTypeWeight>
		{
			new LifeSkillCombatBriberyItemTypeWeight(0, 20),
			new LifeSkillCombatBriberyItemTypeWeight(1, 20),
			new LifeSkillCombatBriberyItemTypeWeight(2, 20),
			new LifeSkillCombatBriberyItemTypeWeight(4, 20),
			new LifeSkillCombatBriberyItemTypeWeight(7, 0)
		}, new List<LifeSkillCombatBriberyItemSubTypeWeight>
		{
			new LifeSkillCombatBriberyItemSubTypeWeight(1000, 20),
			new LifeSkillCombatBriberyItemSubTypeWeight(800, 0),
			new LifeSkillCombatBriberyItemSubTypeWeight(801, 0),
			new LifeSkillCombatBriberyItemSubTypeWeight(505, 0),
			new LifeSkillCombatBriberyItemSubTypeWeight(506, 0),
			new LifeSkillCombatBriberyItemSubTypeWeight(501, 0),
			new LifeSkillCombatBriberyItemSubTypeWeight(502, 0),
			new LifeSkillCombatBriberyItemSubTypeWeight(503, 0),
			new LifeSkillCombatBriberyItemSubTypeWeight(504, 0)
		}, new List<PresetInventoryItem>
		{
			new PresetInventoryItem("Medicine", 292, 3, 100)
		}, new List<PresetInventoryItem>(), 0, 90));
		_dataArray.Add(new OrganizationItem(6, 48, 49, 50, -18, -18, 750, new short[2] { 10, 11 }, new short[9] { 363, 362, 361, 360, 359, 358, 357, 771, 356 }, EOrganizationSettlementType.Sect, isSect: true, isCivilian: false, -1, 0, -250, -1, 40, hereditary: true, allowPoisoning: false, noMeatEating: false, noDrinking: false, 3, 1, 9, new List<sbyte>(), -1, new List<short> { 575, 576, 577, 578, 579 }, new List<sbyte> { 6, 7, 14, 15 }, new List<sbyte> { 0, 1, 2, 3, 8, 9 }, 0, 9, new sbyte[15]
		{
			0, 0, 0, 0, 0, 0, -1, 0, 0, 0,
			0, 0, 0, 0, 0
		}, new short[9] { 99, 98, 97, 96, 95, 94, 93, 92, 91 }, 0, 504, 51, 52, 53, 54, 308, new short[7] { 282, 525, 543, 552, 561, 624, 642 }, 55, new int[1] { 34 }, 27, new List<short> { 67 }, new List<short> { 68 }, 5, new ShortPair[3]
		{
			new ShortPair(22, 50),
			new ShortPair(29, 25),
			new ShortPair(37, 1)
		}, new List<LifeSkillCombatBriberyItemTypeWeight>
		{
			new LifeSkillCombatBriberyItemTypeWeight(0, 20),
			new LifeSkillCombatBriberyItemTypeWeight(1, 20),
			new LifeSkillCombatBriberyItemTypeWeight(2, 15),
			new LifeSkillCombatBriberyItemTypeWeight(4, 20),
			new LifeSkillCombatBriberyItemTypeWeight(7, 20)
		}, new List<LifeSkillCombatBriberyItemSubTypeWeight>
		{
			new LifeSkillCombatBriberyItemSubTypeWeight(1000, 0),
			new LifeSkillCombatBriberyItemSubTypeWeight(800, 5),
			new LifeSkillCombatBriberyItemSubTypeWeight(801, 0),
			new LifeSkillCombatBriberyItemSubTypeWeight(505, 0),
			new LifeSkillCombatBriberyItemSubTypeWeight(506, 0),
			new LifeSkillCombatBriberyItemSubTypeWeight(501, 0),
			new LifeSkillCombatBriberyItemSubTypeWeight(502, 0),
			new LifeSkillCombatBriberyItemSubTypeWeight(503, 0),
			new LifeSkillCombatBriberyItemSubTypeWeight(504, 0)
		}, new List<PresetInventoryItem>
		{
			new PresetInventoryItem("TeaWine", 2, 3, 100),
			new PresetInventoryItem("TeaWine", 11, 3, 100)
		}, new List<PresetInventoryItem>(), 0, 90));
		_dataArray.Add(new OrganizationItem(7, 56, 57, 58, 150, -18, 100, new short[2] { 12, 13 }, new short[9] { 371, 370, 369, 368, 367, 366, 365, 772, 364 }, EOrganizationSettlementType.Sect, isSect: true, isCivilian: false, 3, 0, 0, -1, 30, hereditary: false, allowPoisoning: true, noMeatEating: true, noDrinking: false, 1, 1, 1, new List<sbyte>(), -1, new List<short> { 580, 581, 582, 583, 584 }, new List<sbyte> { 2, 4, 11, 12, 15 }, new List<sbyte> { 0, 1, 2, 4, 7, 10 }, 1, 15, new sbyte[15]
		{
			0, 0, 0, 0, 0, 0, 0, 0, 0, -1,
			0, 0, 0, 0, 0
		}, new short[9] { 108, 107, 106, 105, 104, 103, 102, 101, 100 }, 2, 505, 59, 60, 61, 62, 309, new short[5] { 309, 336, 498, 516, 48 }, 63, new int[2] { 40, 49 }, 28, new List<short> { 76 }, new List<short> { 77 }, 6, new ShortPair[7]
		{
			new ShortPair(15, 100),
			new ShortPair(2, 25),
			new ShortPair(10, 25),
			new ShortPair(13, 25),
			new ShortPair(20, 25),
			new ShortPair(31, 25),
			new ShortPair(37, 1)
		}, new List<LifeSkillCombatBriberyItemTypeWeight>
		{
			new LifeSkillCombatBriberyItemTypeWeight(0, 5),
			new LifeSkillCombatBriberyItemTypeWeight(1, 0),
			new LifeSkillCombatBriberyItemTypeWeight(2, 20),
			new LifeSkillCombatBriberyItemTypeWeight(4, 0),
			new LifeSkillCombatBriberyItemTypeWeight(7, 0)
		}, new List<LifeSkillCombatBriberyItemSubTypeWeight>
		{
			new LifeSkillCombatBriberyItemSubTypeWeight(1000, 50),
			new LifeSkillCombatBriberyItemSubTypeWeight(800, 25),
			new LifeSkillCombatBriberyItemSubTypeWeight(801, 0),
			new LifeSkillCombatBriberyItemSubTypeWeight(505, 0),
			new LifeSkillCombatBriberyItemSubTypeWeight(506, 0),
			new LifeSkillCombatBriberyItemSubTypeWeight(501, 0),
			new LifeSkillCombatBriberyItemSubTypeWeight(502, 0),
			new LifeSkillCombatBriberyItemSubTypeWeight(503, 0),
			new LifeSkillCombatBriberyItemSubTypeWeight(504, 0)
		}, new List<PresetInventoryItem>
		{
			new PresetInventoryItem("Medicine", 112, 3, 100)
		}, new List<PresetInventoryItem>(), 0, 70));
		_dataArray.Add(new OrganizationItem(8, 64, 65, 66, 150, -18, 400, new short[2] { 14, 15 }, new short[9] { 379, 378, 377, 376, 375, 374, 373, 773, 372 }, EOrganizationSettlementType.Sect, isSect: true, isCivilian: false, -1, 0, 500, 0, 10, hereditary: false, allowPoisoning: true, noMeatEating: false, noDrinking: false, 6, 1, 13, new List<sbyte> { 6, 7, 9, 10, 16 }, 207, new List<short> { 585, 586, 587, 588, 589 }, new List<sbyte> { 0, 3, 5, 12 }, new List<sbyte> { 0, 1, 2, 3, 4, 13 }, 2, 18, new sbyte[15]
		{
			0, 0, 0, 0, 0, 0, 0, 0, -1, 0,
			0, 0, 0, 0, 0
		}, new short[9] { 117, 116, 115, 114, 113, 112, 111, 110, 109 }, 1, 506, 67, 68, 69, 70, 310, new short[6] { 336, 327, 345, 318, 309, 741 }, 71, new int[1] { 31 }, 29, new List<short> { 63 }, new List<short> { 64 }, 7, new ShortPair[6]
		{
			new ShortPair(0, 100),
			new ShortPair(3, 25),
			new ShortPair(11, 25),
			new ShortPair(25, 25),
			new ShortPair(33, 25),
			new ShortPair(37, 1)
		}, new List<LifeSkillCombatBriberyItemTypeWeight>
		{
			new LifeSkillCombatBriberyItemTypeWeight(0, 5),
			new LifeSkillCombatBriberyItemTypeWeight(1, 0),
			new LifeSkillCombatBriberyItemTypeWeight(2, 30),
			new LifeSkillCombatBriberyItemTypeWeight(4, 0),
			new LifeSkillCombatBriberyItemTypeWeight(7, 0)
		}, new List<LifeSkillCombatBriberyItemSubTypeWeight>
		{
			new LifeSkillCombatBriberyItemSubTypeWeight(1000, 40),
			new LifeSkillCombatBriberyItemSubTypeWeight(800, 25),
			new LifeSkillCombatBriberyItemSubTypeWeight(801, 0),
			new LifeSkillCombatBriberyItemSubTypeWeight(505, 0),
			new LifeSkillCombatBriberyItemSubTypeWeight(506, 0),
			new LifeSkillCombatBriberyItemSubTypeWeight(501, 0),
			new LifeSkillCombatBriberyItemSubTypeWeight(502, 0),
			new LifeSkillCombatBriberyItemSubTypeWeight(503, 0),
			new LifeSkillCombatBriberyItemSubTypeWeight(504, 0)
		}, new List<PresetInventoryItem>
		{
			new PresetInventoryItem("Medicine", 268, 3, 100)
		}, new List<PresetInventoryItem>(), 0, 70));
		_dataArray.Add(new OrganizationItem(9, 72, 73, 74, -18, -18, 300, new short[2] { 16, 17 }, new short[9] { 387, 386, 385, 384, 383, 382, 381, 774, 380 }, EOrganizationSettlementType.Sect, isSect: true, isCivilian: false, -1, 0, 0, -1, 30, hereditary: true, allowPoisoning: false, noMeatEating: false, noDrinking: false, 5, 1, 12, new List<sbyte>(), -1, null, new List<sbyte> { 6, 7, 10, 11 }, new List<sbyte> { 0, 1, 2, 7, 8, 9, 12 }, 3, 12, new sbyte[15]
		{
			0, 0, 0, 0, 0, -1, 0, 0, 0, 0,
			0, 0, 0, 0, 0
		}, new short[9] { 126, 125, 124, 123, 122, 121, 120, 119, 118 }, 2, 507, 75, 76, 77, 78, 311, new short[20]
		{
			462, 435, 471, 444, 480, 453, 534, 561, 543, 570,
			552, 525, 615, 669, 633, 687, 651, 660, 30, 21
		}, 79, new int[2] { 54, 55 }, 30, new List<short> { 120 }, new List<short> { 121 }, 8, new ShortPair[9]
		{
			new ShortPair(6, 100),
			new ShortPair(7, 100),
			new ShortPair(8, 50),
			new ShortPair(9, 50),
			new ShortPair(29, 25),
			new ShortPair(30, 25),
			new ShortPair(31, 25),
			new ShortPair(32, 25),
			new ShortPair(37, 1)
		}, new List<LifeSkillCombatBriberyItemTypeWeight>
		{
			new LifeSkillCombatBriberyItemTypeWeight(0, 20),
			new LifeSkillCombatBriberyItemTypeWeight(1, 20),
			new LifeSkillCombatBriberyItemTypeWeight(2, 5),
			new LifeSkillCombatBriberyItemTypeWeight(4, 5),
			new LifeSkillCombatBriberyItemTypeWeight(7, 0)
		}, new List<LifeSkillCombatBriberyItemSubTypeWeight>
		{
			new LifeSkillCombatBriberyItemSubTypeWeight(1000, 10),
			new LifeSkillCombatBriberyItemSubTypeWeight(800, 0),
			new LifeSkillCombatBriberyItemSubTypeWeight(801, 0),
			new LifeSkillCombatBriberyItemSubTypeWeight(505, 0),
			new LifeSkillCombatBriberyItemSubTypeWeight(506, 0),
			new LifeSkillCombatBriberyItemSubTypeWeight(501, 10),
			new LifeSkillCombatBriberyItemSubTypeWeight(502, 10),
			new LifeSkillCombatBriberyItemSubTypeWeight(503, 10),
			new LifeSkillCombatBriberyItemSubTypeWeight(504, 10)
		}, new List<PresetInventoryItem>
		{
			new PresetInventoryItem("Material", 0, 3, 100),
			new PresetInventoryItem("Material", 7, 3, 100),
			new PresetInventoryItem("Material", 14, 3, 100),
			new PresetInventoryItem("Material", 21, 3, 100),
			new PresetInventoryItem("Material", 28, 3, 100),
			new PresetInventoryItem("Material", 35, 3, 100),
			new PresetInventoryItem("Material", 42, 3, 100),
			new PresetInventoryItem("Material", 49, 3, 100)
		}, new List<PresetInventoryItem>(), 0, 70));
		_dataArray.Add(new OrganizationItem(10, 80, 81, 82, -18, 125, 500, new short[2] { 18, 19 }, new short[9] { 395, 394, 393, 392, 391, 390, 389, 775, 388 }, EOrganizationSettlementType.Sect, isSect: true, isCivilian: false, -1, 0, -500, -1, 20, hereditary: false, allowPoisoning: true, noMeatEating: false, noDrinking: true, 4, 1, 2, new List<sbyte>(), -1, null, new List<sbyte> { 8, 9, 10, 11 }, new List<sbyte> { 0, 1, 2, 3, 4, 5, 6 }, 4, 18, new sbyte[15]
		{
			0, 0, 0, 0, 0, 0, 0, -1, 0, 0,
			0, 0, 0, 0, 0
		}, new short[9] { 135, 134, 133, 132, 131, 130, 129, 128, 127 }, 3, 508, 83, 84, 85, 86, 312, new short[5] { 273, 336, 318, 372, 3 }, 87, new int[1] { 27 }, 31, new List<short> { 55 }, new List<short> { 56 }, 9, new ShortPair[11]
		{
			new ShortPair(4, 100),
			new ShortPair(5, 100),
			new ShortPair(18, 25),
			new ShortPair(41, 25),
			new ShortPair(42, 25),
			new ShortPair(43, 25),
			new ShortPair(44, 25),
			new ShortPair(45, 25),
			new ShortPair(46, 25),
			new ShortPair(32, 25),
			new ShortPair(37, 1)
		}, new List<LifeSkillCombatBriberyItemTypeWeight>
		{
			new LifeSkillCombatBriberyItemTypeWeight(0, 5),
			new LifeSkillCombatBriberyItemTypeWeight(1, 0),
			new LifeSkillCombatBriberyItemTypeWeight(2, 0),
			new LifeSkillCombatBriberyItemTypeWeight(4, 0),
			new LifeSkillCombatBriberyItemTypeWeight(7, 0)
		}, new List<LifeSkillCombatBriberyItemSubTypeWeight>
		{
			new LifeSkillCombatBriberyItemSubTypeWeight(1000, 5),
			new LifeSkillCombatBriberyItemSubTypeWeight(800, 25),
			new LifeSkillCombatBriberyItemSubTypeWeight(801, 25),
			new LifeSkillCombatBriberyItemSubTypeWeight(505, 20),
			new LifeSkillCombatBriberyItemSubTypeWeight(506, 20),
			new LifeSkillCombatBriberyItemSubTypeWeight(501, 0),
			new LifeSkillCombatBriberyItemSubTypeWeight(502, 0),
			new LifeSkillCombatBriberyItemSubTypeWeight(503, 0),
			new LifeSkillCombatBriberyItemSubTypeWeight(504, 0)
		}, new List<PresetInventoryItem>
		{
			new PresetInventoryItem("Medicine", 100, 3, 100)
		}, new List<PresetInventoryItem>(), 0, 70));
		_dataArray.Add(new OrganizationItem(11, 88, 89, 90, -18, -18, 750, new short[2] { 30, 31 }, new short[9] { 403, 402, 401, 400, 399, 398, 397, 776, 396 }, EOrganizationSettlementType.Sect, isSect: true, isCivilian: false, -1, -1, -500, -1, 10, hereditary: false, allowPoisoning: true, noMeatEating: false, noDrinking: false, 3, 1, 8, new List<sbyte> { 12, 13, 14, 15, 16 }, 208, new List<short> { 590, 591, 592, 593, 594 }, new List<sbyte> { 5, 13, 15 }, new List<sbyte> { 0, 1, 2, 3, 8, 10 }, 0, 6, new sbyte[15]
		{
			0, -1, 0, 0, 0, 0, 0, 0, 0, 0,
			0, 0, 0, 0, 0
		}, new short[9] { 144, 143, 142, 141, 140, 139, 138, 137, 136 }, 4, 509, 91, 92, 93, 94, 313, new short[9] { 282, 300, 534, 525, 552, 570, 381, 390, 408 }, 95, new int[1] { 35 }, 32, new List<short> { 78 }, new List<short> { 79 }, 10, new ShortPair[4]
		{
			new ShortPair(14, 50),
			new ShortPair(16, 25),
			new ShortPair(17, 25),
			new ShortPair(37, 1)
		}, new List<LifeSkillCombatBriberyItemTypeWeight>
		{
			new LifeSkillCombatBriberyItemTypeWeight(0, 10),
			new LifeSkillCombatBriberyItemTypeWeight(1, 10),
			new LifeSkillCombatBriberyItemTypeWeight(2, 40),
			new LifeSkillCombatBriberyItemTypeWeight(4, 10),
			new LifeSkillCombatBriberyItemTypeWeight(7, 20)
		}, new List<LifeSkillCombatBriberyItemSubTypeWeight>
		{
			new LifeSkillCombatBriberyItemSubTypeWeight(1000, 10),
			new LifeSkillCombatBriberyItemSubTypeWeight(800, 0),
			new LifeSkillCombatBriberyItemSubTypeWeight(801, 0),
			new LifeSkillCombatBriberyItemSubTypeWeight(505, 0),
			new LifeSkillCombatBriberyItemSubTypeWeight(506, 0),
			new LifeSkillCombatBriberyItemSubTypeWeight(501, 0),
			new LifeSkillCombatBriberyItemSubTypeWeight(502, 0),
			new LifeSkillCombatBriberyItemSubTypeWeight(503, 0),
			new LifeSkillCombatBriberyItemSubTypeWeight(504, 0)
		}, new List<PresetInventoryItem>(), new List<PresetInventoryItem>
		{
			new PresetInventoryItem("Misc", 28, 1, 100),
			new PresetInventoryItem("Misc", 33, 1, 100),
			new PresetInventoryItem("Misc", 38, 1, 100),
			new PresetInventoryItem("Misc", 43, 1, 100),
			new PresetInventoryItem("Misc", 48, 1, 100),
			new PresetInventoryItem("Misc", 53, 1, 100),
			new PresetInventoryItem("Misc", 58, 1, 100),
			new PresetInventoryItem("Misc", 63, 1, 100),
			new PresetInventoryItem("Misc", 68, 1, 100)
		}, 3, 80));
		_dataArray.Add(new OrganizationItem(12, 96, 97, 98, -18, -18, 550, new short[2] { 22, 23 }, new short[9] { 411, 410, 409, 408, 407, 406, 405, 777, 404 }, EOrganizationSettlementType.Sect, isSect: true, isCivilian: false, -1, -1, -250, -1, 20, hereditary: true, allowPoisoning: true, noMeatEating: false, noDrinking: false, 2, 1, 11, new List<sbyte>
		{
			1, 2, 3, 4, 5, 6, 7, 8, 9, 10,
			11, 13, 14, 15, 16
		}, 209, null, new List<sbyte> { 7, 8, 9 }, new List<sbyte> { 0, 1, 2, 3, 4, 7, 11 }, 1, 15, new sbyte[15]
		{
			0, 0, 0, 0, -1, 0, 0, 0, 0, 0,
			0, 0, 0, 0, 0
		}, new short[9] { 153, 152, 151, 150, 149, 148, 147, 146, 145 }, 3, 510, 99, 100, 101, 102, 314, new short[11]
		{
			354, 273, 345, 372, 336, 327, 480, 498, 489, 795,
			813
		}, 103, new int[1] { 36 }, 33, new List<short> { 69 }, new List<short> { 70, 71 }, 11, new ShortPair[9]
		{
			new ShortPair(5, 100),
			new ShortPair(41, 25),
			new ShortPair(42, 25),
			new ShortPair(43, 25),
			new ShortPair(44, 25),
			new ShortPair(45, 25),
			new ShortPair(46, 25),
			new ShortPair(30, 25),
			new ShortPair(37, 1)
		}, new List<LifeSkillCombatBriberyItemTypeWeight>
		{
			new LifeSkillCombatBriberyItemTypeWeight(0, 10),
			new LifeSkillCombatBriberyItemTypeWeight(1, 0),
			new LifeSkillCombatBriberyItemTypeWeight(2, 0),
			new LifeSkillCombatBriberyItemTypeWeight(4, 0),
			new LifeSkillCombatBriberyItemTypeWeight(7, 0)
		}, new List<LifeSkillCombatBriberyItemSubTypeWeight>
		{
			new LifeSkillCombatBriberyItemSubTypeWeight(1000, 20),
			new LifeSkillCombatBriberyItemSubTypeWeight(800, 0),
			new LifeSkillCombatBriberyItemSubTypeWeight(801, 25),
			new LifeSkillCombatBriberyItemSubTypeWeight(505, 0),
			new LifeSkillCombatBriberyItemSubTypeWeight(506, 35),
			new LifeSkillCombatBriberyItemSubTypeWeight(501, 10),
			new LifeSkillCombatBriberyItemSubTypeWeight(502, 0),
			new LifeSkillCombatBriberyItemSubTypeWeight(503, 0),
			new LifeSkillCombatBriberyItemSubTypeWeight(504, 0)
		}, new List<PresetInventoryItem>
		{
			new PresetInventoryItem("Medicine", 2, 1, 100),
			new PresetInventoryItem("Medicine", 11, 1, 100),
			new PresetInventoryItem("Medicine", 20, 1, 100),
			new PresetInventoryItem("Medicine", 29, 1, 100),
			new PresetInventoryItem("Medicine", 38, 1, 100),
			new PresetInventoryItem("Medicine", 47, 1, 100)
		}, new List<PresetInventoryItem>(), 0, 80));
		_dataArray.Add(new OrganizationItem(13, 104, 105, 106, 125, -18, 350, new short[2] { 24, 25 }, new short[9] { 419, 418, 417, 416, 415, 414, 413, 778, 412 }, EOrganizationSettlementType.Sect, isSect: true, isCivilian: false, -1, -1, -250, -1, 10, hereditary: false, allowPoisoning: true, noMeatEating: true, noDrinking: false, 6, 1, 6, new List<sbyte>(), -1, new List<short> { 595, 596, 597, 598, 599 }, new List<sbyte> { 1, 4, 9 }, new List<sbyte> { 0, 1, 2, 4, 6, 7 }, 2, 9, new sbyte[15]
		{
			0, 0, 0, -1, 0, 0, 0, 0, 0, 0,
			0, 0, 0, 0, 0
		}, new short[9] { 162, 161, 160, 159, 158, 157, 156, 155, 154 }, 0, 511, 107, 108, 109, 110, 315, new short[8] { 318, 309, 363, 183, 192, 201, 462, 435 }, 111, null, -1, null, null, 12, new ShortPair[6]
		{
			new ShortPair(10, 50),
			new ShortPair(1, 50),
			new ShortPair(27, 25),
			new ShortPair(28, 25),
			new ShortPair(34, 25),
			new ShortPair(37, 1)
		}, new List<LifeSkillCombatBriberyItemTypeWeight>
		{
			new LifeSkillCombatBriberyItemTypeWeight(0, 5),
			new LifeSkillCombatBriberyItemTypeWeight(1, 0),
			new LifeSkillCombatBriberyItemTypeWeight(2, 20),
			new LifeSkillCombatBriberyItemTypeWeight(4, 0),
			new LifeSkillCombatBriberyItemTypeWeight(7, 0)
		}, new List<LifeSkillCombatBriberyItemSubTypeWeight>
		{
			new LifeSkillCombatBriberyItemSubTypeWeight(1000, 25),
			new LifeSkillCombatBriberyItemSubTypeWeight(800, 10),
			new LifeSkillCombatBriberyItemSubTypeWeight(801, 20),
			new LifeSkillCombatBriberyItemSubTypeWeight(505, 0),
			new LifeSkillCombatBriberyItemSubTypeWeight(506, 20),
			new LifeSkillCombatBriberyItemSubTypeWeight(501, 0),
			new LifeSkillCombatBriberyItemSubTypeWeight(502, 0),
			new LifeSkillCombatBriberyItemSubTypeWeight(503, 0),
			new LifeSkillCombatBriberyItemSubTypeWeight(504, 0)
		}, new List<PresetInventoryItem>
		{
			new PresetInventoryItem("Medicine", 316, 3, 100)
		}, new List<PresetInventoryItem>(), 0, 80));
		_dataArray.Add(new OrganizationItem(14, 112, 113, 114, -18, -18, 500, new short[2] { 26, 27 }, new short[9] { 427, 426, 425, 424, 423, 422, 421, 779, 420 }, EOrganizationSettlementType.Sect, isSect: true, isCivilian: false, -1, -1, 500, -1, 30, hereditary: true, allowPoisoning: false, noMeatEating: false, noDrinking: false, 5, 1, 3, new List<sbyte>(), -1, new List<short> { 600, 601, 602, 603, 604 }, new List<sbyte> { 5, 6, 14 }, new List<sbyte> { 0, 1, 2, 3, 6, 8 }, 3, 12, new sbyte[15]
		{
			-1, 0, 0, 0, 0, 0, 0, 0, 0, 0,
			0, 0, 0, 0, 0
		}, new short[9] { 171, 170, 169, 168, 167, 166, 165, 164, 163 }, 2, 512, 115, 116, 117, 118, 316, new short[7] { 282, 273, 300, 165, 525, 543, 561 }, 119, new int[1] { 52 }, 35, new List<short> { 109 }, new List<short> { 110 }, 13, new ShortPair[7]
		{
			new ShortPair(11, 50),
			new ShortPair(12, 50),
			new ShortPair(21, 25),
			new ShortPair(38, 25),
			new ShortPair(39, 25),
			new ShortPair(29, 25),
			new ShortPair(37, 1)
		}, new List<LifeSkillCombatBriberyItemTypeWeight>
		{
			new LifeSkillCombatBriberyItemTypeWeight(0, 20),
			new LifeSkillCombatBriberyItemTypeWeight(1, 20),
			new LifeSkillCombatBriberyItemTypeWeight(2, 10),
			new LifeSkillCombatBriberyItemTypeWeight(4, 0),
			new LifeSkillCombatBriberyItemTypeWeight(7, 40)
		}, new List<LifeSkillCombatBriberyItemSubTypeWeight>
		{
			new LifeSkillCombatBriberyItemSubTypeWeight(1000, 10),
			new LifeSkillCombatBriberyItemSubTypeWeight(800, 0),
			new LifeSkillCombatBriberyItemSubTypeWeight(801, 0),
			new LifeSkillCombatBriberyItemSubTypeWeight(505, 0),
			new LifeSkillCombatBriberyItemSubTypeWeight(506, 0),
			new LifeSkillCombatBriberyItemSubTypeWeight(501, 0),
			new LifeSkillCombatBriberyItemSubTypeWeight(502, 0),
			new LifeSkillCombatBriberyItemSubTypeWeight(503, 0),
			new LifeSkillCombatBriberyItemSubTypeWeight(504, 0)
		}, new List<PresetInventoryItem>
		{
			new PresetInventoryItem("Material", 57, 3, 100),
			new PresetInventoryItem("Material", 64, 3, 100),
			new PresetInventoryItem("Material", 71, 3, 100),
			new PresetInventoryItem("Material", 78, 3, 100)
		}, new List<PresetInventoryItem>(), 0, 80));
		_dataArray.Add(new OrganizationItem(15, 120, 121, 122, -18, -18, 750, new short[2] { 28, 29 }, new short[9] { 435, 434, 433, 432, 431, 430, 429, 780, 428 }, EOrganizationSettlementType.Sect, isSect: true, isCivilian: false, -1, -1, -500, -1, 10, hereditary: true, allowPoisoning: true, noMeatEating: false, noDrinking: false, 0, 1, 0, new List<sbyte>(), -1, null, new List<sbyte> { 5, 8, 9, 13, 15 }, new List<sbyte> { 0, 1, 2, 3, 4, 5, 6 }, 4, 6, new sbyte[15]
		{
			0, 0, -1, 0, 0, 0, 0, 0, 0, 0,
			0, 0, 0, 0, 0
		}, new short[9] { 180, 179, 178, 177, 176, 175, 174, 173, 172 }, 4, 513, 123, 124, 125, 126, 317, new short[6] { 345, 273, 354, 291, 345, 12 }, 127, new int[2] { 28, 29 }, 36, new List<short> { 57 }, new List<short> { 58 }, 14, new ShortPair[6]
		{
			new ShortPair(40, 100),
			new ShortPair(13, 25),
			new ShortPair(41, 25),
			new ShortPair(43, 25),
			new ShortPair(45, 25),
			new ShortPair(37, 1)
		}, new List<LifeSkillCombatBriberyItemTypeWeight>
		{
			new LifeSkillCombatBriberyItemTypeWeight(0, 5),
			new LifeSkillCombatBriberyItemTypeWeight(1, 5),
			new LifeSkillCombatBriberyItemTypeWeight(2, 20),
			new LifeSkillCombatBriberyItemTypeWeight(4, 5),
			new LifeSkillCombatBriberyItemTypeWeight(7, 5)
		}, new List<LifeSkillCombatBriberyItemSubTypeWeight>
		{
			new LifeSkillCombatBriberyItemSubTypeWeight(1000, 15),
			new LifeSkillCombatBriberyItemSubTypeWeight(800, 5),
			new LifeSkillCombatBriberyItemSubTypeWeight(801, 20),
			new LifeSkillCombatBriberyItemSubTypeWeight(505, 0),
			new LifeSkillCombatBriberyItemSubTypeWeight(506, 20),
			new LifeSkillCombatBriberyItemSubTypeWeight(501, 0),
			new LifeSkillCombatBriberyItemSubTypeWeight(502, 0),
			new LifeSkillCombatBriberyItemSubTypeWeight(503, 0),
			new LifeSkillCombatBriberyItemSubTypeWeight(504, 0)
		}, new List<PresetInventoryItem>
		{
			new PresetInventoryItem("Misc", 2, 9, 100)
		}, new List<PresetInventoryItem>(), 0, 100));
		_dataArray.Add(new OrganizationItem(16, 128, 129, 130, 25, 100, 100, new short[2] { -1, -1 }, new short[9] { -1, -1, -1, -1, -1, -1, -1, -1, -1 }, EOrganizationSettlementType.TaiwuVillage, isSect: false, isCivilian: false, -1, 1, short.MinValue, -1, 20, hereditary: true, allowPoisoning: true, noMeatEating: false, noDrinking: false, -1, 1, -1, new List<sbyte>(), -1, null, new List<sbyte>(), new List<sbyte>(), -1, 12, new sbyte[15], new short[9] { 18, 17, 16, 15, 14, 13, 12, 11, 10 }, 0, -1, 131, 132, 133, 134, -1, null, 135, null, -1, null, null, -1, new ShortPair[0], new List<LifeSkillCombatBriberyItemTypeWeight>(), new List<LifeSkillCombatBriberyItemSubTypeWeight>(), new List<PresetInventoryItem>(), new List<PresetInventoryItem>(), 0, -1));
		_dataArray.Add(new OrganizationItem(17, 136, 137, 138, 0, 0, 0, new short[2] { -1, -1 }, new short[9] { -1, -1, -1, -1, -1, -1, -1, -1, -1 }, EOrganizationSettlementType.Invalid, isSect: false, isCivilian: false, -1, -1, short.MinValue, -1, 0, hereditary: true, allowPoisoning: true, noMeatEating: false, noDrinking: false, -1, 1, -1, new List<sbyte>(), -1, null, new List<sbyte>(), new List<sbyte>(), -1, -1, new sbyte[15], new short[9] { 27, 26, 25, 24, 23, 22, 21, 20, 19 }, 0, -1, 139, 140, 141, 142, -1, null, 143, null, -1, null, null, -1, new ShortPair[0], new List<LifeSkillCombatBriberyItemTypeWeight>(), new List<LifeSkillCombatBriberyItemSubTypeWeight>(), new List<PresetInventoryItem>(), new List<PresetInventoryItem>(), 0, -1));
		_dataArray.Add(new OrganizationItem(18, 144, 145, 146, 0, 0, 0, new short[2] { -1, -1 }, new short[9] { -1, -1, -1, -1, -1, -1, -1, -1, -1 }, EOrganizationSettlementType.Invalid, isSect: false, isCivilian: false, -1, 1, short.MinValue, -1, 0, hereditary: true, allowPoisoning: true, noMeatEating: false, noDrinking: false, -1, 1, -1, new List<sbyte>(), -1, null, new List<sbyte>(), new List<sbyte>(), -1, -1, new sbyte[15], new short[9] { 36, 35, 34, 33, 32, 31, 30, 29, 28 }, 0, -1, 147, 148, 149, 150, -1, null, 151, null, -1, null, null, -1, new ShortPair[0], new List<LifeSkillCombatBriberyItemTypeWeight>(), new List<LifeSkillCombatBriberyItemSubTypeWeight>(), new List<PresetInventoryItem>(), new List<PresetInventoryItem>(), 0, -1));
		_dataArray.Add(new OrganizationItem(19, 152, 153, 154, 0, 0, 0, new short[2] { -1, -1 }, new short[9] { -1, -1, -1, -1, -1, -1, -1, -1, -1 }, EOrganizationSettlementType.Invalid, isSect: false, isCivilian: false, -1, -1, short.MinValue, -1, 0, hereditary: true, allowPoisoning: true, noMeatEating: false, noDrinking: false, -1, 1, -1, new List<sbyte>(), -1, null, new List<sbyte>(), new List<sbyte>(), -1, -1, new sbyte[15], new short[9] { 45, 44, 43, 42, 41, 40, 39, 38, 37 }, 0, -1, 155, 156, 157, 158, -1, null, 159, null, -1, null, null, -1, new ShortPair[0], new List<LifeSkillCombatBriberyItemTypeWeight>(), new List<LifeSkillCombatBriberyItemSubTypeWeight>(), new List<PresetInventoryItem>(), new List<PresetInventoryItem>(), 0, -1));
		_dataArray.Add(new OrganizationItem(20, 152, 160, 161, 0, 0, 0, new short[2] { -1, -1 }, new short[9] { -1, -1, -1, -1, -1, -1, -1, -1, -1 }, EOrganizationSettlementType.Invalid, isSect: false, isCivilian: false, -1, -1, short.MinValue, -1, 0, hereditary: true, allowPoisoning: true, noMeatEating: false, noDrinking: false, -1, 1, -1, new List<sbyte>(), -1, null, new List<sbyte>(), new List<sbyte>(), -1, -1, new sbyte[15], new short[9] { 9, 8, 7, 6, 5, 4, 3, 2, 1 }, 0, -1, 162, 163, 164, 165, -1, null, 166, null, -1, null, null, -1, new ShortPair[0], new List<LifeSkillCombatBriberyItemTypeWeight>(), new List<LifeSkillCombatBriberyItemSubTypeWeight>(), new List<PresetInventoryItem>(), new List<PresetInventoryItem>(), 0, -1));
		_dataArray.Add(new OrganizationItem(21, 167, 168, 169, 150, 125, 550000, new short[2] { 0, 1 }, new short[9] { -1, -1, -1, -1, -1, -1, -1, -1, -1 }, EOrganizationSettlementType.City, isSect: false, isCivilian: true, -1, 0, short.MinValue, -1, 30, hereditary: true, allowPoisoning: true, noMeatEating: false, noDrinking: false, -1, 4, -1, new List<sbyte>(), -1, null, new List<sbyte>(), new List<sbyte>(), -1, 12, new sbyte[15], new short[9] { 189, 188, 187, 186, 185, 184, 183, 182, 181 }, 0, -1, 170, 171, 172, 173, -1, null, 174, null, -1, null, null, -1, new ShortPair[0], new List<LifeSkillCombatBriberyItemTypeWeight>(), new List<LifeSkillCombatBriberyItemSubTypeWeight>(), new List<PresetInventoryItem>(), new List<PresetInventoryItem>(), 0, 30));
		_dataArray.Add(new OrganizationItem(22, 175, 176, 177, 150, 125, 120000, new short[2] { 2, 3 }, new short[9] { -1, -1, -1, -1, -1, -1, -1, -1, -1 }, EOrganizationSettlementType.City, isSect: false, isCivilian: true, -1, 0, short.MinValue, -1, 30, hereditary: true, allowPoisoning: true, noMeatEating: false, noDrinking: false, -1, 4, -1, new List<sbyte>(), -1, null, new List<sbyte>(), new List<sbyte>(), -1, 12, new sbyte[15], new short[9] { 189, 188, 187, 186, 185, 184, 183, 182, 181 }, 0, -1, 178, 179, 180, 181, -1, null, 182, null, -1, null, null, -1, new ShortPair[0], new List<LifeSkillCombatBriberyItemTypeWeight>(), new List<LifeSkillCombatBriberyItemSubTypeWeight>(), new List<PresetInventoryItem>(), new List<PresetInventoryItem>(), 0, 30));
		_dataArray.Add(new OrganizationItem(23, 183, 184, 185, 75, 150, 52000, new short[2] { 4, 5 }, new short[9] { -1, -1, -1, -1, -1, -1, -1, -1, -1 }, EOrganizationSettlementType.City, isSect: false, isCivilian: true, -1, 0, short.MinValue, -1, 30, hereditary: true, allowPoisoning: true, noMeatEating: false, noDrinking: false, -1, 4, -1, new List<sbyte>(), -1, null, new List<sbyte>(), new List<sbyte>(), -1, 12, new sbyte[15], new short[9] { 189, 188, 187, 186, 185, 184, 183, 182, 181 }, 0, -1, 186, 187, 188, 189, -1, null, 190, null, -1, null, null, -1, new ShortPair[0], new List<LifeSkillCombatBriberyItemTypeWeight>(), new List<LifeSkillCombatBriberyItemSubTypeWeight>(), new List<PresetInventoryItem>(), new List<PresetInventoryItem>(), 0, 30));
		_dataArray.Add(new OrganizationItem(24, 191, 192, 193, 125, 175, 120000, new short[2] { 6, 7 }, new short[9] { -1, -1, -1, -1, -1, -1, -1, -1, -1 }, EOrganizationSettlementType.City, isSect: false, isCivilian: true, -1, 0, short.MinValue, -1, 30, hereditary: true, allowPoisoning: true, noMeatEating: false, noDrinking: false, -1, 4, -1, new List<sbyte>(), -1, null, new List<sbyte>(), new List<sbyte>(), -1, 12, new sbyte[15], new short[9] { 189, 188, 187, 186, 185, 184, 183, 182, 181 }, 0, -1, 194, 195, 196, 197, -1, null, 198, null, -1, null, null, -1, new ShortPair[0], new List<LifeSkillCombatBriberyItemTypeWeight>(), new List<LifeSkillCombatBriberyItemSubTypeWeight>(), new List<PresetInventoryItem>(), new List<PresetInventoryItem>(), 0, 30));
		_dataArray.Add(new OrganizationItem(25, 199, 200, 201, 100, 175, 58000, new short[2] { 8, 9 }, new short[9] { -1, -1, -1, -1, -1, -1, -1, -1, -1 }, EOrganizationSettlementType.City, isSect: false, isCivilian: true, -1, 0, short.MinValue, -1, 30, hereditary: true, allowPoisoning: true, noMeatEating: false, noDrinking: false, -1, 4, -1, new List<sbyte>(), -1, null, new List<sbyte>(), new List<sbyte>(), -1, 12, new sbyte[15], new short[9] { 189, 188, 187, 186, 185, 184, 183, 182, 181 }, 0, -1, 202, 203, 204, 205, -1, null, 206, null, -1, null, null, -1, new ShortPair[0], new List<LifeSkillCombatBriberyItemTypeWeight>(), new List<LifeSkillCombatBriberyItemSubTypeWeight>(), new List<PresetInventoryItem>(), new List<PresetInventoryItem>(), 0, 30));
		_dataArray.Add(new OrganizationItem(26, 207, 208, 209, 125, 75, 78000, new short[2] { 10, 11 }, new short[9] { -1, -1, -1, -1, -1, -1, -1, -1, -1 }, EOrganizationSettlementType.City, isSect: false, isCivilian: true, -1, 0, short.MinValue, -1, 30, hereditary: true, allowPoisoning: true, noMeatEating: false, noDrinking: false, -1, 4, -1, new List<sbyte>(), -1, null, new List<sbyte>(), new List<sbyte>(), -1, 12, new sbyte[15], new short[9] { 189, 188, 187, 186, 185, 184, 183, 182, 181 }, 0, -1, 210, 211, 212, 213, -1, null, 214, null, -1, null, null, -1, new ShortPair[0], new List<LifeSkillCombatBriberyItemTypeWeight>(), new List<LifeSkillCombatBriberyItemSubTypeWeight>(), new List<PresetInventoryItem>(), new List<PresetInventoryItem>(), 0, 30));
		_dataArray.Add(new OrganizationItem(27, 215, 216, 217, 100, 100, 54000, new short[2] { 12, 13 }, new short[9] { -1, -1, -1, -1, -1, -1, -1, -1, -1 }, EOrganizationSettlementType.City, isSect: false, isCivilian: true, -1, 0, short.MinValue, -1, 30, hereditary: true, allowPoisoning: true, noMeatEating: false, noDrinking: false, -1, 4, -1, new List<sbyte>(), -1, null, new List<sbyte>(), new List<sbyte>(), -1, 12, new sbyte[15], new short[9] { 189, 188, 187, 186, 185, 184, 183, 182, 181 }, 0, -1, 218, 219, 220, 221, -1, null, 222, null, -1, null, null, -1, new ShortPair[0], new List<LifeSkillCombatBriberyItemTypeWeight>(), new List<LifeSkillCombatBriberyItemSubTypeWeight>(), new List<PresetInventoryItem>(), new List<PresetInventoryItem>(), 0, 30));
		_dataArray.Add(new OrganizationItem(28, 223, 224, 225, 175, 125, 75000, new short[2] { 14, 15 }, new short[9] { -1, -1, -1, -1, -1, -1, -1, -1, -1 }, EOrganizationSettlementType.City, isSect: false, isCivilian: true, -1, 0, short.MinValue, -1, 30, hereditary: true, allowPoisoning: true, noMeatEating: false, noDrinking: false, -1, 4, -1, new List<sbyte>(), -1, null, new List<sbyte>(), new List<sbyte>(), -1, 12, new sbyte[15], new short[9] { 189, 188, 187, 186, 185, 184, 183, 182, 181 }, 0, -1, 226, 227, 228, 229, -1, null, 230, null, -1, null, null, -1, new ShortPair[0], new List<LifeSkillCombatBriberyItemTypeWeight>(), new List<LifeSkillCombatBriberyItemSubTypeWeight>(), new List<PresetInventoryItem>(), new List<PresetInventoryItem>(), 0, 30));
		_dataArray.Add(new OrganizationItem(29, 231, 232, 233, 125, 150, 140000, new short[2] { 16, 17 }, new short[9] { -1, -1, -1, -1, -1, -1, -1, -1, -1 }, EOrganizationSettlementType.City, isSect: false, isCivilian: true, -1, 0, short.MinValue, -1, 30, hereditary: true, allowPoisoning: true, noMeatEating: false, noDrinking: false, -1, 4, -1, new List<sbyte>(), -1, null, new List<sbyte>(), new List<sbyte>(), -1, 12, new sbyte[15], new short[9] { 189, 188, 187, 186, 185, 184, 183, 182, 181 }, 0, -1, 234, 235, 236, 237, -1, null, 238, null, -1, null, null, -1, new ShortPair[0], new List<LifeSkillCombatBriberyItemTypeWeight>(), new List<LifeSkillCombatBriberyItemSubTypeWeight>(), new List<PresetInventoryItem>(), new List<PresetInventoryItem>(), 0, 30));
		_dataArray.Add(new OrganizationItem(30, 239, 240, 241, 75, 175, 56000, new short[2] { 18, 19 }, new short[9] { -1, -1, -1, -1, -1, -1, -1, -1, -1 }, EOrganizationSettlementType.City, isSect: false, isCivilian: true, -1, 0, short.MinValue, -1, 30, hereditary: true, allowPoisoning: true, noMeatEating: false, noDrinking: false, -1, 4, -1, new List<sbyte>(), -1, null, new List<sbyte>(), new List<sbyte>(), -1, 12, new sbyte[15], new short[9] { 189, 188, 187, 186, 185, 184, 183, 182, 181 }, 0, -1, 242, 243, 244, 245, -1, null, 246, null, -1, null, null, -1, new ShortPair[0], new List<LifeSkillCombatBriberyItemTypeWeight>(), new List<LifeSkillCombatBriberyItemSubTypeWeight>(), new List<PresetInventoryItem>(), new List<PresetInventoryItem>(), 0, 30));
		_dataArray.Add(new OrganizationItem(31, 247, 248, 249, 75, 75, 32000, new short[2] { 20, 21 }, new short[9] { -1, -1, -1, -1, -1, -1, -1, -1, -1 }, EOrganizationSettlementType.City, isSect: false, isCivilian: true, -1, 0, short.MinValue, -1, 30, hereditary: true, allowPoisoning: true, noMeatEating: false, noDrinking: false, -1, 4, -1, new List<sbyte>(), -1, null, new List<sbyte>(), new List<sbyte>(), -1, 12, new sbyte[15], new short[9] { 189, 188, 187, 186, 185, 184, 183, 182, 181 }, 0, -1, 250, 251, 252, 253, -1, null, 254, null, -1, null, null, -1, new ShortPair[0], new List<LifeSkillCombatBriberyItemTypeWeight>(), new List<LifeSkillCombatBriberyItemSubTypeWeight>(), new List<PresetInventoryItem>(), new List<PresetInventoryItem>(), 0, 30));
		_dataArray.Add(new OrganizationItem(32, 255, 256, 257, 100, 150, 68000, new short[2] { 22, 23 }, new short[9] { -1, -1, -1, -1, -1, -1, -1, -1, -1 }, EOrganizationSettlementType.City, isSect: false, isCivilian: true, -1, 0, short.MinValue, -1, 30, hereditary: true, allowPoisoning: true, noMeatEating: false, noDrinking: false, -1, 4, -1, new List<sbyte>(), -1, null, new List<sbyte>(), new List<sbyte>(), -1, 12, new sbyte[15], new short[9] { 189, 188, 187, 186, 185, 184, 183, 182, 181 }, 0, -1, 258, 259, 260, 261, -1, null, 262, null, -1, null, null, -1, new ShortPair[0], new List<LifeSkillCombatBriberyItemTypeWeight>(), new List<LifeSkillCombatBriberyItemSubTypeWeight>(), new List<PresetInventoryItem>(), new List<PresetInventoryItem>(), 0, 30));
		_dataArray.Add(new OrganizationItem(33, 263, 264, 265, 150, 100, 85000, new short[2] { 24, 25 }, new short[9] { -1, -1, -1, -1, -1, -1, -1, -1, -1 }, EOrganizationSettlementType.City, isSect: false, isCivilian: true, -1, 0, short.MinValue, -1, 30, hereditary: true, allowPoisoning: true, noMeatEating: false, noDrinking: false, -1, 4, -1, new List<sbyte>(), -1, null, new List<sbyte>(), new List<sbyte>(), -1, 12, new sbyte[15], new short[9] { 189, 188, 187, 186, 185, 184, 183, 182, 181 }, 0, -1, 266, 267, 268, 269, -1, null, 270, null, -1, null, null, -1, new ShortPair[0], new List<LifeSkillCombatBriberyItemTypeWeight>(), new List<LifeSkillCombatBriberyItemSubTypeWeight>(), new List<PresetInventoryItem>(), new List<PresetInventoryItem>(), 0, 30));
		_dataArray.Add(new OrganizationItem(34, 271, 272, 273, 175, 100, 160000, new short[2] { 26, 27 }, new short[9] { -1, -1, -1, -1, -1, -1, -1, -1, -1 }, EOrganizationSettlementType.City, isSect: false, isCivilian: true, -1, 0, short.MinValue, -1, 30, hereditary: true, allowPoisoning: true, noMeatEating: false, noDrinking: false, -1, 4, -1, new List<sbyte>(), -1, null, new List<sbyte>(), new List<sbyte>(), -1, 12, new sbyte[15], new short[9] { 189, 188, 187, 186, 185, 184, 183, 182, 181 }, 0, -1, 274, 275, 276, 277, -1, null, 278, null, -1, null, null, -1, new ShortPair[0], new List<LifeSkillCombatBriberyItemTypeWeight>(), new List<LifeSkillCombatBriberyItemSubTypeWeight>(), new List<PresetInventoryItem>(), new List<PresetInventoryItem>(), 0, 30));
		_dataArray.Add(new OrganizationItem(35, 279, 280, 281, 175, 75, 110000, new short[2] { 28, 29 }, new short[9] { -1, -1, -1, -1, -1, -1, -1, -1, -1 }, EOrganizationSettlementType.City, isSect: false, isCivilian: true, -1, 0, short.MinValue, -1, 30, hereditary: true, allowPoisoning: true, noMeatEating: false, noDrinking: false, -1, 4, -1, new List<sbyte>(), -1, null, new List<sbyte>(), new List<sbyte>(), -1, 12, new sbyte[15], new short[9] { 189, 188, 187, 186, 185, 184, 183, 182, 181 }, 0, -1, 282, 283, 284, 285, -1, null, 286, null, -1, null, null, -1, new ShortPair[0], new List<LifeSkillCombatBriberyItemTypeWeight>(), new List<LifeSkillCombatBriberyItemSubTypeWeight>(), new List<PresetInventoryItem>(), new List<PresetInventoryItem>(), 0, 30));
		_dataArray.Add(new OrganizationItem(36, 287, 288, 289, -10, -10, 50, new short[2] { -1, -1 }, new short[9] { -1, -1, -1, -1, -1, -1, -1, -1, -1 }, EOrganizationSettlementType.Village, isSect: false, isCivilian: true, -1, 0, short.MinValue, -1, 30, hereditary: true, allowPoisoning: true, noMeatEating: false, noDrinking: false, -1, 1, -1, new List<sbyte>(), -1, null, new List<sbyte>(), new List<sbyte>(), -1, 12, new sbyte[15], new short[9] { 198, 197, 196, 195, 194, 193, 192, 191, 190 }, 0, -1, 290, 291, 292, 293, -1, null, 294, null, -1, null, null, -1, new ShortPair[0], new List<LifeSkillCombatBriberyItemTypeWeight>(), new List<LifeSkillCombatBriberyItemSubTypeWeight>(), new List<PresetInventoryItem>(), new List<PresetInventoryItem>(), 0, 20));
		_dataArray.Add(new OrganizationItem(37, 295, 296, 297, -18, -18, 1000, new short[2] { -1, -1 }, new short[9] { -1, -1, -1, -1, -1, -1, -1, -1, -1 }, EOrganizationSettlementType.Town, isSect: false, isCivilian: true, -1, 0, short.MinValue, -1, 30, hereditary: true, allowPoisoning: true, noMeatEating: false, noDrinking: false, -1, 3, -1, new List<sbyte>(), -1, null, new List<sbyte>(), new List<sbyte>(), -1, 12, new sbyte[15], new short[9] { 207, 206, 205, 204, 203, 202, 201, 200, 199 }, 0, -1, 298, 299, 300, 301, -1, null, 302, null, -1, null, null, -1, new ShortPair[0], new List<LifeSkillCombatBriberyItemTypeWeight>(), new List<LifeSkillCombatBriberyItemSubTypeWeight>(), new List<PresetInventoryItem>(), new List<PresetInventoryItem>(), 0, 25));
		_dataArray.Add(new OrganizationItem(38, 303, 304, 305, -14, -14, 300, new short[2] { -1, -1 }, new short[9] { -1, -1, -1, -1, -1, -1, -1, -1, -1 }, EOrganizationSettlementType.WalledTown, isSect: false, isCivilian: true, -1, 0, short.MinValue, -1, 30, hereditary: true, allowPoisoning: true, noMeatEating: false, noDrinking: false, -1, 2, -1, new List<sbyte>(), -1, null, new List<sbyte>(), new List<sbyte>(), -1, 12, new sbyte[15], new short[9] { 216, 215, 214, 213, 212, 211, 210, 209, 208 }, 0, -1, 306, 307, 308, 309, -1, null, 310, null, -1, null, null, -1, new ShortPair[0], new List<LifeSkillCombatBriberyItemTypeWeight>(), new List<LifeSkillCombatBriberyItemSubTypeWeight>(), new List<PresetInventoryItem>(), new List<PresetInventoryItem>(), 0, 25));
		_dataArray.Add(new OrganizationItem(39, 311, 312, 313, 0, 0, 0, new short[2] { -1, -1 }, new short[9] { -1, -1, -1, -1, -1, -1, -1, -1, -1 }, EOrganizationSettlementType.Invalid, isSect: false, isCivilian: false, -1, -1, short.MinValue, -1, 0, hereditary: true, allowPoisoning: true, noMeatEating: false, noDrinking: false, -1, 1, -1, new List<sbyte>(), -1, null, new List<sbyte>(), new List<sbyte>(), -1, -1, new sbyte[15], new short[9] { 225, 224, 223, 222, 221, 220, 219, 218, 217 }, 0, -1, 314, 315, 316, 317, -1, null, 318, null, -1, null, null, -1, new ShortPair[0], new List<LifeSkillCombatBriberyItemTypeWeight>(), new List<LifeSkillCombatBriberyItemSubTypeWeight>(), new List<PresetInventoryItem>(), new List<PresetInventoryItem>(), 0, -1));
		_dataArray.Add(new OrganizationItem(40, 319, 320, 321, 0, 0, 0, new short[2] { -1, -1 }, new short[9] { -1, -1, -1, -1, -1, -1, -1, -1, -1 }, EOrganizationSettlementType.Invalid, isSect: false, isCivilian: false, -1, -1, short.MinValue, -1, 0, hereditary: true, allowPoisoning: true, noMeatEating: false, noDrinking: false, -1, 1, -1, new List<sbyte>(), -1, null, new List<sbyte>(), new List<sbyte>(), -1, -1, new sbyte[15], new short[9] { 234, 233, 232, 231, 230, 229, 228, 227, 226 }, 0, -1, 322, 323, 324, 325, -1, null, 326, null, -1, null, null, -1, new ShortPair[0], new List<LifeSkillCombatBriberyItemTypeWeight>(), new List<LifeSkillCombatBriberyItemSubTypeWeight>(), new List<PresetInventoryItem>(), new List<PresetInventoryItem>(), 0, -1));
		_dataArray.Add(new OrganizationItem(41, 327, 328, 329, 0, 0, 0, new short[2] { -1, -1 }, new short[9] { -1, -1, -1, -1, -1, -1, -1, -1, -1 }, EOrganizationSettlementType.Invalid, isSect: false, isCivilian: false, -1, -1, short.MinValue, -1, 0, hereditary: true, allowPoisoning: true, noMeatEating: false, noDrinking: false, -1, 1, -1, new List<sbyte>(), -1, null, new List<sbyte>(), new List<sbyte>(), -1, -1, new sbyte[15], new short[9] { 243, 242, 241, 240, 239, 238, 237, 236, 235 }, 0, -1, 330, 331, 332, 333, -1, null, 334, null, -1, null, null, -1, new ShortPair[0], new List<LifeSkillCombatBriberyItemTypeWeight>(), new List<LifeSkillCombatBriberyItemSubTypeWeight>(), new List<PresetInventoryItem>(), new List<PresetInventoryItem>(), 0, -1));
	}

	public override void Init()
	{
		base.Init();
		_dataArray = new List<OrganizationItem>(42);
		CreateItems0();
	}
}
