using System;
using System.Collections.Generic;
using Config.Common;

namespace Config;

[Serializable]
public class MapBlock : ConfigData<MapBlockItem, short>
{
	public static class DefKey
	{
		public const short Taiwucun = 0;

		public const short Jingcheng = 1;

		public const short Chengdu = 2;

		public const short Guizhou = 3;

		public const short Xiangyang = 4;

		public const short Taiyuan = 5;

		public const short Guangzhou = 6;

		public const short Qingzhou = 7;

		public const short Jiangling = 8;

		public const short Fuzhou = 9;

		public const short Liaoyang = 10;

		public const short Qinzhou = 11;

		public const short Dali = 12;

		public const short Shouchun = 13;

		public const short Hangzhou = 14;

		public const short Yangzhou = 15;

		public const short SecretVilliage = 16;

		public const short BambooHouse1 = 17;

		public const short BambooHouse2 = 18;

		public const short Shaolin = 19;

		public const short Emei = 20;

		public const short Baihua = 21;

		public const short Wudang = 22;

		public const short Yuanshan = 23;

		public const short Shixiang = 24;

		public const short Ranshan = 25;

		public const short Xuannv = 26;

		public const short Zhujian = 27;

		public const short Kongsang = 28;

		public const short Jingang = 29;

		public const short Wuxian = 30;

		public const short Jieqing = 31;

		public const short Fulong = 32;

		public const short Xuehou = 33;

		public const short Village = 34;

		public const short Town = 35;

		public const short Stockade = 36;

		public const short Station = 37;

		public const short BrokenStation = 38;

		public const short Farmland1 = 39;

		public const short Gardens1 = 42;

		public const short StoneForest1 = 45;

		public const short MulberryField1 = 48;

		public const short HerbalGarden1 = 51;

		public const short JadeMountain1 = 54;

		public const short TianXian3 = 68;

		public const short HeGu1 = 90;

		public const short HeGu3 = 92;

		public const short TaoYuan1 = 103;

		public const short Ruin1 = 118;

		public const short Ruin2 = 119;

		public const short Ruin3 = 120;

		public const short Ruin4 = 121;

		public const short Ruin5 = 122;

		public const short Ruin6 = 123;

		public const short Abyss = 124;

		public const short Block = 125;

		public const short None = 126;

		public const short SwordTombMonv = 128;

		public const short SwordTombDayueYaochang = 129;

		public const short SwordTombJiuhan = 130;

		public const short SwordTombJinHuanger = 131;

		public const short SwordTombYiYihou = 132;

		public const short SwordTombWeiQi = 133;

		public const short SwordTombYixiang = 134;

		public const short SwordTombXuefeng = 135;

		public const short SwordTombShuFang = 136;

		public const short LoongWhiteBlock = 137;

		public const short LoongBlackBlock = 138;

		public const short LoongGreenBlock = 139;

		public const short LoongRedBlock = 140;

		public const short LoongYellowBlock = 141;
	}

	public static class DefValue
	{
		public static MapBlockItem Taiwucun => Instance[(short)0];

		public static MapBlockItem Jingcheng => Instance[(short)1];

		public static MapBlockItem Chengdu => Instance[(short)2];

		public static MapBlockItem Guizhou => Instance[(short)3];

		public static MapBlockItem Xiangyang => Instance[(short)4];

		public static MapBlockItem Taiyuan => Instance[(short)5];

		public static MapBlockItem Guangzhou => Instance[(short)6];

		public static MapBlockItem Qingzhou => Instance[(short)7];

		public static MapBlockItem Jiangling => Instance[(short)8];

		public static MapBlockItem Fuzhou => Instance[(short)9];

		public static MapBlockItem Liaoyang => Instance[(short)10];

		public static MapBlockItem Qinzhou => Instance[(short)11];

		public static MapBlockItem Dali => Instance[(short)12];

		public static MapBlockItem Shouchun => Instance[(short)13];

		public static MapBlockItem Hangzhou => Instance[(short)14];

		public static MapBlockItem Yangzhou => Instance[(short)15];

		public static MapBlockItem SecretVilliage => Instance[(short)16];

		public static MapBlockItem BambooHouse1 => Instance[(short)17];

		public static MapBlockItem BambooHouse2 => Instance[(short)18];

		public static MapBlockItem Shaolin => Instance[(short)19];

		public static MapBlockItem Emei => Instance[(short)20];

		public static MapBlockItem Baihua => Instance[(short)21];

		public static MapBlockItem Wudang => Instance[(short)22];

		public static MapBlockItem Yuanshan => Instance[(short)23];

		public static MapBlockItem Shixiang => Instance[(short)24];

		public static MapBlockItem Ranshan => Instance[(short)25];

		public static MapBlockItem Xuannv => Instance[(short)26];

		public static MapBlockItem Zhujian => Instance[(short)27];

		public static MapBlockItem Kongsang => Instance[(short)28];

		public static MapBlockItem Jingang => Instance[(short)29];

		public static MapBlockItem Wuxian => Instance[(short)30];

		public static MapBlockItem Jieqing => Instance[(short)31];

		public static MapBlockItem Fulong => Instance[(short)32];

		public static MapBlockItem Xuehou => Instance[(short)33];

		public static MapBlockItem Village => Instance[(short)34];

		public static MapBlockItem Town => Instance[(short)35];

		public static MapBlockItem Stockade => Instance[(short)36];

		public static MapBlockItem Station => Instance[(short)37];

		public static MapBlockItem BrokenStation => Instance[(short)38];

		public static MapBlockItem Farmland1 => Instance[(short)39];

		public static MapBlockItem Gardens1 => Instance[(short)42];

		public static MapBlockItem StoneForest1 => Instance[(short)45];

		public static MapBlockItem MulberryField1 => Instance[(short)48];

		public static MapBlockItem HerbalGarden1 => Instance[(short)51];

		public static MapBlockItem JadeMountain1 => Instance[(short)54];

		public static MapBlockItem TianXian3 => Instance[(short)68];

		public static MapBlockItem HeGu1 => Instance[(short)90];

		public static MapBlockItem HeGu3 => Instance[(short)92];

		public static MapBlockItem TaoYuan1 => Instance[(short)103];

		public static MapBlockItem Ruin1 => Instance[(short)118];

		public static MapBlockItem Ruin2 => Instance[(short)119];

		public static MapBlockItem Ruin3 => Instance[(short)120];

		public static MapBlockItem Ruin4 => Instance[(short)121];

		public static MapBlockItem Ruin5 => Instance[(short)122];

		public static MapBlockItem Ruin6 => Instance[(short)123];

		public static MapBlockItem Abyss => Instance[(short)124];

		public static MapBlockItem Block => Instance[(short)125];

		public static MapBlockItem None => Instance[(short)126];

		public static MapBlockItem SwordTombMonv => Instance[(short)128];

		public static MapBlockItem SwordTombDayueYaochang => Instance[(short)129];

		public static MapBlockItem SwordTombJiuhan => Instance[(short)130];

		public static MapBlockItem SwordTombJinHuanger => Instance[(short)131];

		public static MapBlockItem SwordTombYiYihou => Instance[(short)132];

		public static MapBlockItem SwordTombWeiQi => Instance[(short)133];

		public static MapBlockItem SwordTombYixiang => Instance[(short)134];

		public static MapBlockItem SwordTombXuefeng => Instance[(short)135];

		public static MapBlockItem SwordTombShuFang => Instance[(short)136];

		public static MapBlockItem LoongWhiteBlock => Instance[(short)137];

		public static MapBlockItem LoongBlackBlock => Instance[(short)138];

		public static MapBlockItem LoongGreenBlock => Instance[(short)139];

		public static MapBlockItem LoongRedBlock => Instance[(short)140];

		public static MapBlockItem LoongYellowBlock => Instance[(short)141];
	}

	public static MapBlock Instance = new MapBlock();

	private readonly HashSet<string> RequiredFields = new HashSet<string>
	{
		"SplitOrMergeBlockId", "MainResourceType", "ResourceCollectionType", "LandFormType", "CenterBuilding", "PresetBuildingList", "RandomBuildingList", "InformationTemplateId", "CombatScene", "CombatState",
		"TemplateId", "Name", "AdventureEditorName", "AdventureBlockImage", "Desc", "VillagerWorkCorrectImage", "Bgm", "FixedBuildingImage", "EventBack"
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
		_dataArray.Add(new MapBlockItem(0, EMapBlockType.Town, EMapBlockSubType.TaiwuCun, 0, 0, 1, 2, 1, 2, 3, 1, 1, freeStepIgnorePathCost: true, showTips: true, new string[1] { "sp_maptop_city" }, new string[1] { "" }, null, ignoreDestroyed: false, taiwuEventChangedBlock: true, -1, new List<sbyte>(), new short[6], 0, -1, new int[0], "city_taiwucun", new string[1] { "Ambience_map_7" }, 16, 0, 44, 1, null, new List<short> { 48, 46 }, new List<short>(), -1, new List<(short, short)> { (1, 100) }, "tex_mapblockevent_town_taiwucun_1", 41, -1, canGenerate: true));
		_dataArray.Add(new MapBlockItem(1, EMapBlockType.City, EMapBlockSubType.Jingcheng, 3, 3, 4, 5, 3, 3, 5, 1, 1, freeStepIgnorePathCost: true, showTips: true, new string[1] { "MapCityJingcheng" }, new string[1] { "City_JingCheng/eff_city_jingcheng" }, "Villagerwork_MapCityJingcheng", ignoreDestroyed: false, taiwuEventChangedBlock: false, -1, new List<sbyte>(), new short[6], 0, -1, new int[9] { 6, 7, 8, 9, 10, 11, 12, 13, 14 }, "city_jincheng", new string[9] { "Ambience_map_14", " Ambience_map_city03", " Ambience_map_17", " Ambience_map_city03", " Ambience_map_city03", " Ambience_map_city03", " Ambience_map_city03", " Ambience_map_city03", " Ambience_map_city03" }, 16, 0, 224, 10, null, new List<short>
		{
			48, 284, 277, 46, 46, 46, 47, 47, 47, 47,
			47, 47, 47, 47, 47
		}, new List<short> { 47, 47, 47, 47, 47, 47 }, 0, new List<(short, short)> { (1, 100) }, "tex_city_jincheng", 9, -1, canGenerate: true));
		_dataArray.Add(new MapBlockItem(2, EMapBlockType.City, EMapBlockSubType.Chengdu, 15, 15, 16, 17, 3, 3, 5, 1, 1, freeStepIgnorePathCost: true, showTips: true, new string[1] { "MapCityChengdu" }, new string[1] { "City_ChengDu/eff_city_chengdu" }, "Villagerwork_MapCityChengdu", ignoreDestroyed: false, taiwuEventChangedBlock: false, -1, new List<sbyte>(), new short[6], 0, -1, new int[9] { 18, 19, 20, 21, 22, 23, 24, 25, 26 }, "city_chengdu", new string[9] { "Ambience_map_city03", " Ambience_map_city03", " Ambience_map_4", " Ambience_map_city01", " Ambience_map_14", " Ambience_map_4", " Ambience_map_4", " Ambience_map_14", " Ambience_map_17" }, 16, 1, 225, 10, null, new List<short>
		{
			48, 284, 275, 46, 46, 46, 46, 46, 46, 47,
			47, 47, 47, 47, 47
		}, new List<short> { 46, 46, 46, 47, 47, 47 }, 1, new List<(short, short)> { (1, 100) }, "tex_city_chengdu", 9, -1, canGenerate: true));
		_dataArray.Add(new MapBlockItem(3, EMapBlockType.City, EMapBlockSubType.Guizhou, 27, 27, 1, 28, 3, 3, 5, 1, 1, freeStepIgnorePathCost: true, showTips: true, new string[1] { "MapCityGuizhou" }, new string[1] { "City_GuiZhou/eff_city_guizhou" }, "Villagerwork_MapCityGuizhou", ignoreDestroyed: false, taiwuEventChangedBlock: false, -1, new List<sbyte>(), new short[6], 0, -1, new int[9] { 29, 30, 31, 32, 33, 34, 35, 36, 37 }, "city_guizhou", new string[9] { "Ambience_map_4", " Ambience_map_14", " Ambience_map_17", " Ambience_map_17", " Ambience_map_17", " Ambience_map_17", " Ambience_map_17", " Ambience_map_17", " Ambience_map_14" }, 16, 2, 226, 10, null, new List<short>
		{
			48, 284, 278, 46, 46, 46, 46, 46, 46, 46,
			46, 46, 47, 47, 47
		}, new List<short> { 46, 46, 46, 47, 47, 47 }, 2, new List<(short, short)> { (1, 100) }, "tex_city_guizhou", 9, -1, canGenerate: true));
		_dataArray.Add(new MapBlockItem(4, EMapBlockType.City, EMapBlockSubType.Xiangyang, 38, 38, 4, 39, 3, 3, 5, 1, 1, freeStepIgnorePathCost: true, showTips: true, new string[1] { "MapCityXiangyang" }, new string[1] { "City_XiangYang/eff_city_xiangyang" }, "Villagerwork_MapCityXiangyang", ignoreDestroyed: false, taiwuEventChangedBlock: false, -1, new List<sbyte>(), new short[6], 0, -1, new int[9] { 40, 41, 42, 43, 44, 45, 46, 47, 48 }, "city_xiangyang", new string[9] { "Ambience_map_city01", " Ambience_map_city03", " Ambience_map_14", " Ambience_map_city02", " Ambience_map_city03", " Ambience_map_17", " Ambience_map_17", " Ambience_map_14", " Ambience_map_14" }, 16, 3, 227, 10, null, new List<short>
		{
			48, 284, 275, 46, 46, 46, 46, 46, 46, 47,
			47, 47, 47, 47, 47
		}, new List<short> { 46, 46, 46, 47, 47, 47 }, 3, new List<(short, short)> { (1, 100) }, "tex_city_xiangyang", 9, -1, canGenerate: true));
		_dataArray.Add(new MapBlockItem(5, EMapBlockType.City, EMapBlockSubType.Taiyuan, 49, 49, 16, 50, 3, 3, 5, 1, 1, freeStepIgnorePathCost: true, showTips: true, new string[1] { "MapCityTaiyuan" }, new string[1] { "City_TaiYuan/eff_city_taiyuan" }, "Villagerwork_MapCityTaiyuan", ignoreDestroyed: false, taiwuEventChangedBlock: false, -1, new List<sbyte>(), new short[6], 0, -1, new int[9] { 51, 52, 53, 54, 55, 56, 57, 58, 59 }, "city_taiyuan", new string[9] { "Ambience_map_city01", " Ambience_map_14", " Ambience_map_14", " Ambience_map_14", " Ambience_map_17", " Ambience_map_baihuagu_2", " Ambience_map_14", " Ambience_map_17", " Ambience_map_14" }, 16, 1, 228, 10, null, new List<short>
		{
			48, 284, 274, 46, 46, 46, 46, 46, 46, 46,
			46, 46, 47, 47, 47
		}, new List<short> { 46, 46, 46, 47, 47, 47 }, 4, new List<(short, short)> { (1, 100) }, "tex_city_taiyuan", 9, -1, canGenerate: true));
		_dataArray.Add(new MapBlockItem(6, EMapBlockType.City, EMapBlockSubType.Guangzhou, 60, 60, 1, 61, 3, 3, 5, 1, 1, freeStepIgnorePathCost: true, showTips: true, new string[1] { "MapCityGuangzhou" }, new string[1] { "City_GuangZhou/eff_city_guangzhou" }, "Villagerwork_MapCityGuangzhou", ignoreDestroyed: false, taiwuEventChangedBlock: false, -1, new List<sbyte>(), new short[6], 0, -1, new int[9] { 62, 63, 64, 65, 66, 67, 68, 69, 70 }, "city_guangzhou", new string[9] { "Ambience_map_city01", " Ambience_map_city01", " Ambience_map_12", " Ambience_map_17", " Ambience_map_city02", " Ambience_map_12", " Ambience_map_14", " Ambience_map_17", " Ambience_map_12" }, 16, 4, 229, 10, null, new List<short>
		{
			48, 284, 274, 46, 46, 46, 46, 46, 46, 47,
			47, 47, 47, 47, 47
		}, new List<short> { 46, 46, 46, 47, 47, 47 }, 5, new List<(short, short)> { (1, 100) }, "tex_city_guangzhou", 9, -1, canGenerate: true));
		_dataArray.Add(new MapBlockItem(7, EMapBlockType.City, EMapBlockSubType.Qingzhou, 71, 71, 4, 72, 3, 3, 5, 1, 1, freeStepIgnorePathCost: true, showTips: true, new string[1] { "MapCityQingzhou" }, new string[1] { "City_QingZhou/eff_city_qingzhou" }, "Villagerwork_MapCityQingzhou", ignoreDestroyed: false, taiwuEventChangedBlock: false, -1, new List<sbyte>(), new short[6], 0, -1, new int[9] { 73, 74, 75, 76, 77, 78, 79, 80, 81 }, "city_qingzhou", new string[9] { "Ambience_map_8", " Ambience_map_baihuagu_2", " Ambience_map_16", " Ambience_map_14", " Ambience_map_17", " Ambience_map_14", " Ambience_map_14", " Ambience_map_12", " Ambience_map_14" }, 16, 0, 230, 10, null, new List<short>
		{
			48, 284, 279, 46, 46, 46, 46, 46, 46, 47,
			47, 47, 47, 47, 47
		}, new List<short> { 46, 46, 46, 47, 47, 47 }, 6, new List<(short, short)> { (1, 100) }, "tex_city_qingzhou", 9, -1, canGenerate: true));
		_dataArray.Add(new MapBlockItem(8, EMapBlockType.City, EMapBlockSubType.Jiangling, 82, 82, 16, 83, 3, 3, 5, 1, 1, freeStepIgnorePathCost: true, showTips: true, new string[1] { "MapCityJiangling" }, new string[1] { "City_JiangLing/eff_city_jiangling" }, "Villagerwork_MapCityJiangling", ignoreDestroyed: false, taiwuEventChangedBlock: false, -1, new List<sbyte>(), new short[6], 0, -1, new int[9] { 84, 85, 86, 87, 88, 89, 90, 91, 92 }, "city_jiangling", new string[9] { "Ambience_map_14", " Ambience_map_city02", " Ambience_map_city01", " Ambience_map_14", " Ambience_map_xuannv_1", " Ambience_map_yuanshan_2", " Ambience_map_14", " Ambience_map_12", " Ambience_map_14" }, 16, 3, 231, 10, null, new List<short>
		{
			48, 284, 280, 46, 46, 46, 47, 47, 47, 47,
			47, 47, 47, 47, 47, 47, 47, 47
		}, new List<short> { 47, 47, 47, 47, 47, 47 }, 7, new List<(short, short)> { (1, 100) }, "tex_city_jiangling", 9, -1, canGenerate: true));
		_dataArray.Add(new MapBlockItem(9, EMapBlockType.City, EMapBlockSubType.Fuzhou, 93, 93, 4, 94, 3, 3, 5, 1, 1, freeStepIgnorePathCost: true, showTips: true, new string[1] { "MapCityFuzhou" }, new string[1] { "City_FuZhou/eff_city_fuzhou" }, "Villagerwork_MapCityFuzhou", ignoreDestroyed: false, taiwuEventChangedBlock: false, -1, new List<sbyte>(), new short[6], 0, -1, new int[9] { 95, 96, 97, 98, 99, 100, 101, 102, 103 }, "city_fuzhou", new string[9] { "Ambience_map_14", " Ambience_map_city01", " Ambience_map_17", " Ambience_map_17", " Ambience_map_1", " Ambience_map_17", " Ambience_map_1", " Ambience_map_17", " Ambience_map_14" }, 16, 4, 232, 10, null, new List<short>
		{
			48, 284, 277, 46, 46, 46, 46, 46, 46, 47,
			47, 47, 47, 47, 47
		}, new List<short> { 46, 46, 46, 47, 47, 47 }, 8, new List<(short, short)> { (1, 100) }, "tex_city_fuzhou", 9, -1, canGenerate: true));
		_dataArray.Add(new MapBlockItem(10, EMapBlockType.City, EMapBlockSubType.Liaoyang, 104, 104, 16, 105, 3, 3, 5, 1, 1, freeStepIgnorePathCost: true, showTips: true, new string[1] { "MapCityLiaoyang" }, new string[1] { "City_LiaoYang/eff_city_liaoyang" }, "Villagerwork_MapCityLiaoyang", ignoreDestroyed: false, taiwuEventChangedBlock: false, -1, new List<sbyte>(), new short[6], 0, -1, new int[9] { 106, 107, 108, 109, 110, 111, 112, 113, 114 }, "city_liaoyang", new string[9] { "Ambience_map_14", " Ambience_map_city03", " Ambience_map_14", " Ambience_map_14", " Ambience_map_city03", " Ambience_map_14", " Ambience_map_14", " Ambience_map_14", " Ambience_map_1" }, 16, 5, 233, 10, null, new List<short>
		{
			48, 284, 278, 46, 46, 46, 46, 46, 46, 47,
			47, 47, 47, 47, 47
		}, new List<short> { 46, 46, 46, 47, 47, 47 }, 9, new List<(short, short)> { (1, 100) }, "tex_city_liaoyang", 9, -1, canGenerate: true));
		_dataArray.Add(new MapBlockItem(11, EMapBlockType.City, EMapBlockSubType.Qinzhou, 115, 115, 1, 116, 3, 3, 5, 1, 1, freeStepIgnorePathCost: true, showTips: true, new string[1] { "MapCityQinzhou" }, new string[1] { "City_QinZhou/eff_city_qinzhou" }, "Villagerwork_MapCityQinzhou", ignoreDestroyed: false, taiwuEventChangedBlock: false, -1, new List<sbyte>(), new short[6], 0, -1, new int[9] { 117, 118, 119, 120, 121, 122, 123, 124, 125 }, "city_qinzhou", new string[9] { "Ambience_map_city05", " Ambience_map_jingangzong_2", " Ambience_map_15", " Ambience_map_city05", " Ambience_map_15", " Ambience_map_15", " Ambience_map_15", " Ambience_map_15", " Ambience_map_15" }, 16, 1, 234, 10, null, new List<short>
		{
			48, 284, 46, 46, 46, 46, 46, 46, 46, 46,
			46, 46, 46, 46
		}, new List<short> { 46, 46, 46, 46, 46, 46 }, 10, new List<(short, short)> { (1, 100) }, "tex_city_qinzhou", 9, -1, canGenerate: true));
		_dataArray.Add(new MapBlockItem(12, EMapBlockType.City, EMapBlockSubType.Dali, 126, 126, 1, 127, 3, 3, 5, 1, 1, freeStepIgnorePathCost: true, showTips: true, new string[1] { "MapCityDali" }, new string[1] { "City_DaLi/eff_city_dali" }, "Villagerwork_MapCityDali", ignoreDestroyed: false, taiwuEventChangedBlock: false, -1, new List<sbyte>(), new short[6], 0, -1, new int[9] { 128, 129, 130, 131, 132, 133, 134, 135, 136 }, "city_dali", new string[9] { "Ambience_map_16", " Ambience_map_yuanshan_2", " Ambience_map_14", " Ambience_map_14", " Ambience_map_14", " Ambience_map_14", " Ambience_map_14", " Ambience_map_17", " Ambience_map_12" }, 16, 2, 235, 10, null, new List<short>
		{
			48, 284, 279, 46, 46, 46, 46, 46, 46, 47,
			47, 47, 47, 47, 47
		}, new List<short> { 46, 46, 46, 47, 47, 47 }, 11, new List<(short, short)> { (1, 100) }, "tex_city_dali", 9, -1, canGenerate: true));
		_dataArray.Add(new MapBlockItem(13, EMapBlockType.City, EMapBlockSubType.Shouchun, 137, 137, 4, 138, 3, 3, 5, 1, 1, freeStepIgnorePathCost: true, showTips: true, new string[1] { "MapCityShouchun" }, new string[1] { "City_ShouChun/eff_city_shouchun" }, "Villagerwork_MapCityShouchun", ignoreDestroyed: false, taiwuEventChangedBlock: false, -1, new List<sbyte>(), new short[6], 0, -1, new int[9] { 139, 140, 141, 142, 143, 144, 145, 146, 147 }, "city_shouchun", new string[9] { "Ambience_map_14", " Ambience_map_city03", " Ambience_map_14", " Ambience_map_14", " Ambience_map_city04", " Ambience_map_17", " Ambience_map_14", " Ambience_map_17", " Ambience_map_1" }, 16, 0, 236, 10, null, new List<short>
		{
			48, 284, 276, 46, 46, 46, 46, 46, 46, 47,
			47, 47, 47, 47, 47
		}, new List<short> { 46, 46, 46, 47, 47, 47 }, 12, new List<(short, short)> { (1, 100) }, "tex_city_shouchun", 9, -1, canGenerate: true));
		_dataArray.Add(new MapBlockItem(14, EMapBlockType.City, EMapBlockSubType.Hangzhou, 148, 148, 16, 149, 3, 3, 5, 1, 1, freeStepIgnorePathCost: true, showTips: true, new string[1] { "MapCityHangzhou" }, new string[1] { "City_HangZhou/eff_city_hangzhou" }, "Villagerwork_MapCityHangzhou", ignoreDestroyed: false, taiwuEventChangedBlock: false, -1, new List<sbyte>(), new short[6], 0, -1, new int[9] { 150, 151, 152, 153, 154, 155, 156, 157, 158 }, "city_hangzhou", new string[9] { "Ambience_map_14", " Ambience_map_city03", " Ambience_map_city04", " Ambience_map_17", " Ambience_map_city04", " Ambience_map_city04", " Ambience_map_14", " Ambience_map_12", " Ambience_map_14" }, 16, 4, 237, 10, null, new List<short>
		{
			48, 284, 280, 46, 46, 46, 47, 47, 47, 47,
			47, 47, 47, 47, 47
		}, new List<short> { 47, 47, 47, 47, 47, 47 }, 13, new List<(short, short)> { (1, 100) }, "tex_city_hangzhou", 9, -1, canGenerate: true));
		_dataArray.Add(new MapBlockItem(15, EMapBlockType.City, EMapBlockSubType.Yangzhou, 159, 159, 1, 160, 3, 3, 5, 1, 1, freeStepIgnorePathCost: true, showTips: true, new string[1] { "MapCityYangzhou" }, new string[1] { "City_YangZhou/eff_city_yangzhou" }, "Villagerwork_MapCityYangzhou", ignoreDestroyed: false, taiwuEventChangedBlock: false, -1, new List<sbyte>(), new short[6], 0, -1, new int[9] { 161, 162, 163, 164, 165, 166, 167, 168, 169 }, "city_yangzhou", new string[9] { "Ambience_map_city03", " Ambience_map_city04", " Ambience_map_17", " Ambience_map_city02", " Ambience_map_city04", " Ambience_map_17", " Ambience_map_14", " Ambience_map_14", " Ambience_map_1" }, 16, 3, 238, 10, null, new List<short>
		{
			48, 284, 276, 46, 46, 46, 47, 47, 47, 47,
			47, 47, 47, 47, 47
		}, new List<short> { 47, 47, 47, 47, 47, 47 }, 14, new List<(short, short)> { (1, 100) }, "tex_city_yangzhou", 9, -1, canGenerate: true));
		_dataArray.Add(new MapBlockItem(16, EMapBlockType.Town, EMapBlockSubType.Village, 170, 170, 4, 171, 1, 3, 4, 1, 1, freeStepIgnorePathCost: true, showTips: true, new string[1] { "SecretVillage" }, new string[1] { "" }, null, ignoreDestroyed: false, taiwuEventChangedBlock: true, -1, new List<sbyte>(), new short[6], 0, -1, new int[0], null, new string[1] { "Ambience_map_1" }, 10, 0, 254, 5, null, new List<short> { 48, 46 }, new List<short>(), -1, new List<(short, short)> { (1, 100) }, "tex_mapblockevent_town_village", 11, -1, canGenerate: true));
		_dataArray.Add(new MapBlockItem(17, EMapBlockType.Town, EMapBlockSubType.Zhulu, 172, 173, 16, 174, 1, 0, 1, 1, 1, freeStepIgnorePathCost: true, showTips: true, new string[1] { "Bamboohouse_1_1" }, new string[1] { "" }, null, ignoreDestroyed: false, taiwuEventChangedBlock: false, -1, new List<sbyte>(), new short[6], 0, -1, new int[0], null, new string[1] { "Ambience_map_5" }, 6, 2, 257, 1, null, new List<short> { 48 }, new List<short>(), -1, new List<(short, short)> { (1, 100) }, "tex_mapblockevent_bamboohouse_0", 11, -1, canGenerate: true));
		_dataArray.Add(new MapBlockItem(18, EMapBlockType.Town, EMapBlockSubType.Zhulu, 172, 175, 1, 176, 1, 0, 1, 1, 1, freeStepIgnorePathCost: true, showTips: true, new string[1] { "Bamboohouse_1_1" }, new string[1] { "" }, null, ignoreDestroyed: false, taiwuEventChangedBlock: false, -1, new List<sbyte>(), new short[6], 0, -1, new int[0], null, new string[1] { "Ambience_map_5" }, 6, 2, 258, 1, null, new List<short> { 48 }, new List<short>(), -1, new List<(short, short)> { (1, 100) }, "tex_mapblockevent_bamboohouse_1", 11, -1, canGenerate: true));
		_dataArray.Add(new MapBlockItem(19, EMapBlockType.Sect, EMapBlockSubType.ShaolinPai, 177, 177, 4, 178, 2, 3, 5, 1, 1, freeStepIgnorePathCost: true, showTips: true, new string[1] { "MapGangShaolin" }, new string[1] { "Organization_shaolin/eff_organization_shaolin" }, "Villagerwork_MapGangShaolin", ignoreDestroyed: false, taiwuEventChangedBlock: false, -1, new List<sbyte>(), new short[6], 0, -1, new int[4] { 179, 180, 181, 182 }, "sect_shaolinpai_zaoke", new string[4] { "Ambience_map_4", " Ambience_map_14", " Ambience_map_14", " Ambience_map_shaolin_1" }, 12, 1, 239, 8, "少林派", new List<short> { 48, 288, 303, 259, 46, 46, 46 }, new List<short> { 46, 46, 46 }, 15, new List<(short, short)> { (1, 100) }, "tex_sect_shaolinpai", 0, -1, canGenerate: true));
		_dataArray.Add(new MapBlockItem(20, EMapBlockType.Sect, EMapBlockSubType.EmeiPai, 183, 183, 16, 184, 2, 3, 5, 1, 1, freeStepIgnorePathCost: true, showTips: true, new string[1] { "MapGangEmei" }, new string[1] { "Organization_emei/eff_organization_emei" }, "Villagerwork_MapGangEmei", ignoreDestroyed: false, taiwuEventChangedBlock: false, -1, new List<sbyte>(), new short[6], 0, -1, new int[4] { 185, 186, 187, 188 }, "sect_emeipai", new string[4] { "Ambience_map_emeishan_1", " Ambience_map_4", " Ambience_map_6", " Ambience_map_5" }, 16, 1, 240, 8, "峨眉派", new List<short> { 48, 289, 304, 260, 46, 46, 47, 47 }, new List<short> { 46, 46, 46, 47, 47, 47 }, 16, new List<(short, short)> { (1, 100) }, "tex_sect_emeipai", 0, -1, canGenerate: true));
		_dataArray.Add(new MapBlockItem(21, EMapBlockType.Sect, EMapBlockSubType.BaihuaGu, 189, 189, 4, 190, 2, 3, 5, 1, 1, freeStepIgnorePathCost: true, showTips: true, new string[1] { "MapGangBaihua" }, new string[1] { "Organization_baihua/eff_organization_baihua" }, "Villagerwork_MapGangBaihua", ignoreDestroyed: false, taiwuEventChangedBlock: false, -1, new List<sbyte>(), new short[6], 0, -1, new int[4] { 191, 192, 193, 194 }, "sect_baihuagu", new string[4] { "Ambience_map_1", " Ambience_map_baihuagu_1", " Ambience_map_baihuagu_2", " Ambience_map_1" }, 10, 3, 241, 8, "百花谷", new List<short> { 48, 290, 305, 261, 46, 46 }, new List<short> { 46, 46 }, 17, new List<(short, short)> { (1, 100) }, "tex_sect_baihuagu", 0, -1, canGenerate: true));
		_dataArray.Add(new MapBlockItem(22, EMapBlockType.Sect, EMapBlockSubType.WudangPai, 195, 195, 16, 196, 2, 3, 5, 1, 1, freeStepIgnorePathCost: true, showTips: true, new string[1] { "MapGangWudang" }, new string[1] { "Organization_wudang/eff_organization_wudang" }, "Villagerwork_MapGangWudang", ignoreDestroyed: false, taiwuEventChangedBlock: false, -1, new List<sbyte>(), new short[6], 0, -1, new int[4] { 197, 198, 199, 200 }, "sect_wudangpai_tiangang", new string[4] { "Ambience_map_6", " Ambience_map_13", " Ambience_map_4", " Ambience_map_wudang_1" }, 12, 1, 242, 8, "武当派", new List<short> { 48, 291, 306, 262, 46, 46, 47, 47 }, new List<short> { 46, 47 }, 18, new List<(short, short)> { (1, 100) }, "tex_sect_wudangpai", 0, -1, canGenerate: true));
		_dataArray.Add(new MapBlockItem(23, EMapBlockType.Sect, EMapBlockSubType.YuanshanPai, 201, 201, 1, 202, 2, 3, 5, 1, 1, freeStepIgnorePathCost: true, showTips: true, new string[1] { "MapGangYuanshan" }, new string[1] { "Organization_yuanshan/eff_organization_yuanshan" }, "Villagerwork_MapGangYuanshan", ignoreDestroyed: false, taiwuEventChangedBlock: false, -1, new List<sbyte>(), new short[6], 0, -1, new int[4] { 203, 204, 205, 206 }, "sect_yuanshanpai_daxiaoyuanshan", new string[4] { "Ambience_map_yuanshan_1", " Ambience_map_14", " Ambience_map_yuanshan_2", " Ambience_map_yuanshan_3" }, 16, 1, 243, 8, "元山派", new List<short> { 48, 292, 307, 263, 46, 46, 46, 46, 46 }, new List<short> { 46, 46, 46, 46, 46 }, 19, new List<(short, short)> { (1, 100) }, "tex_sect_yuanshanpai", 0, -1, canGenerate: true));
		_dataArray.Add(new MapBlockItem(24, EMapBlockType.Sect, EMapBlockSubType.ShixiangMen, 207, 207, 1, 208, 2, 3, 5, 1, 1, freeStepIgnorePathCost: true, showTips: true, new string[1] { "MapGangShixiang" }, new string[1] { "Organization_shixiang/eff_organization_shixiang" }, "Villagerwork_MapGangShixiang", ignoreDestroyed: false, taiwuEventChangedBlock: false, -1, new List<sbyte>(), new short[6], 0, -1, new int[4] { 209, 210, 211, 212 }, "sect_shixiangmen_xiongshijiuwu", new string[4] { "Ambience_map_13", " Ambience_map_13", " Ambience_map_shixiangmen_2", " Ambience_map_shixiangmen_1" }, 12, 4, 244, 8, "狮相门", new List<short> { 48, 293, 308, 264, 46, 46, 47, 47 }, new List<short> { 46, 47 }, 20, new List<(short, short)> { (1, 100) }, "tex_sect_shixiangmen", 0, -1, canGenerate: true));
		_dataArray.Add(new MapBlockItem(25, EMapBlockType.Sect, EMapBlockSubType.RanshanPai, 213, 213, 4, 214, 2, 3, 5, 1, 1, freeStepIgnorePathCost: true, showTips: true, new string[1] { "MapGangRanshan" }, new string[1] { "Organization_ranshan/eff_organization_ranshan" }, "Villagerwork_MapGangRanshan", ignoreDestroyed: false, taiwuEventChangedBlock: false, -1, new List<sbyte>(), new short[6], 0, -1, new int[4] { 215, 216, 217, 218 }, "sect_ranshanpai", new string[4] { "Ambience_map_14", " Ambience_map_ranshan_1", " Ambience_map_14", " Ambience_map_14" }, 10, 2, 245, 8, "然山派", new List<short> { 48, 294, 309, 265, 46 }, new List<short> { 46 }, 21, new List<(short, short)> { (1, 100) }, "tex_sect_ranshanpai", 0, -1, canGenerate: true));
		_dataArray.Add(new MapBlockItem(26, EMapBlockType.Sect, EMapBlockSubType.XuannvPai, 219, 219, 16, 220, 2, 3, 5, 1, 1, freeStepIgnorePathCost: true, showTips: true, new string[1] { "MapGangXuannv" }, new string[1] { "Organization_xuannv/eff_organization_xuannv" }, "Villagerwork_MapGangXuannv", ignoreDestroyed: false, taiwuEventChangedBlock: false, -1, new List<sbyte>(), new short[6], 0, -1, new int[4] { 221, 222, 223, 224 }, "sect_xuannvpai_xuannvfeng", new string[4] { "Ambience_map_xuannv_3", " Ambience_map_xuannv_2", " Ambience_map_xuannv_1", " Ambience_map_xuannv_1" }, 12, 5, 246, 8, "璇女派", new List<short> { 48, 295, 310, 266, 46, 46, 47, 47 }, new List<short> { 46, 47 }, 22, new List<(short, short)> { (1, 100) }, "tex_sect_xuannvpai", 0, -1, canGenerate: true));
		_dataArray.Add(new MapBlockItem(27, EMapBlockType.Sect, EMapBlockSubType.ZhujianShanzhuang, 225, 225, 4, 226, 2, 3, 5, 1, 1, freeStepIgnorePathCost: true, showTips: true, new string[1] { "MapGangZhujian" }, new string[1] { "Organization_zhujian/eff_organization_zhujian" }, "Villagerwork_MapGangZhujian", ignoreDestroyed: false, taiwuEventChangedBlock: false, -1, new List<sbyte>(), new short[6], 0, -1, new int[4] { 227, 228, 229, 230 }, "sect_zhujianshanzhuang_tiehao", new string[4] { "Ambience_map_zhujian_2", " Ambience_map_zhujian_1", " Ambience_map_13", " Ambience_map_zhujian_1" }, 12, 1, 247, 8, "铸剑山庄", new List<short> { 48, 296, 311, 267, 46, 46, 47, 47 }, new List<short> { 46, 47 }, 23, new List<(short, short)> { (1, 100) }, "tex_sect_zhujianshanzhuang", 0, -1, canGenerate: true));
		_dataArray.Add(new MapBlockItem(28, EMapBlockType.Sect, EMapBlockSubType.KongsangPai, 231, 231, 16, 232, 2, 3, 5, 1, 1, freeStepIgnorePathCost: true, showTips: true, new string[1] { "MapGangKongsang" }, new string[1] { "Organization_kongsang/eff_organization_kongsang" }, "Villagerwork_MapGangKongsang", ignoreDestroyed: false, taiwuEventChangedBlock: false, -1, new List<sbyte>(), new short[6], 0, -1, new int[4] { 233, 234, 235, 236 }, "sect_kongsangpai", new string[4] { "Ambience_map_14", " Ambience_map_15", " Ambience_map_15", " Ambience_map_15" }, 12, 5, 248, 8, "空桑派", new List<short> { 48, 297, 312, 268, 46, 46 }, new List<short> { 46, 46 }, 24, new List<(short, short)> { (1, 100) }, "tex_sect_kongsangpai", 0, -1, canGenerate: true));
		_dataArray.Add(new MapBlockItem(29, EMapBlockType.Sect, EMapBlockSubType.JingangZong, 237, 237, 1, 238, 2, 3, 5, 1, 1, freeStepIgnorePathCost: true, showTips: true, new string[1] { "MapGangJingang" }, new string[1] { "Organization_jingang/eff_organization_jingang" }, "Villagerwork_MapGangJingang", ignoreDestroyed: false, taiwuEventChangedBlock: false, -1, new List<sbyte>(), new short[6], 0, -1, new int[4] { 239, 240, 241, 242 }, "sect_jingangzong", new string[4] { "Ambience_map_jingangzong_1", " Ambience_map_15", " Ambience_map_jingangzong_2", " Ambience_map_jingangzong_1" }, 16, 1, 249, 8, "金刚宗", new List<short> { 48, 298, 313, 269, 46, 46, 47, 47 }, new List<short> { 46, 46, 46, 47, 47, 47 }, 25, new List<(short, short)> { (1, 100) }, "tex_sect_jingangzong", 0, -1, canGenerate: true));
		_dataArray.Add(new MapBlockItem(30, EMapBlockType.Sect, EMapBlockSubType.WuxianJiao, 243, 243, 4, 244, 2, 3, 5, 1, 1, freeStepIgnorePathCost: true, showTips: true, new string[1] { "MapGangWuxian" }, new string[1] { "Organization_wuxian/eff_organization_wuxian" }, "Villagerwork_MapGangWuxian", ignoreDestroyed: false, taiwuEventChangedBlock: false, -1, new List<sbyte>(), new short[6], 0, -1, new int[4] { 245, 246, 247, 248 }, "sect_wuxianjiao_wuxian", new string[4] { "Ambience_map_wuxian_3", " Ambience_map_wuxian_2", " Ambience_map_wuxian_4", " Ambience_map_wuxian_1" }, 12, 2, 250, 8, "五仙教", new List<short> { 48, 299, 314, 270, 46, 46 }, new List<short> { 46, 46 }, 26, new List<(short, short)> { (1, 100) }, "tex_sect_wuxianjiao", 0, -1, canGenerate: true));
		_dataArray.Add(new MapBlockItem(31, EMapBlockType.Sect, EMapBlockSubType.JieqingMen, 249, 249, 16, 250, 2, 3, 5, 1, 1, freeStepIgnorePathCost: true, showTips: true, new string[1] { "MapGangJieqing" }, new string[1] { "Organization_jieqing/eff_organization_jieqing" }, "Villagerwork_MapGangJieqing", ignoreDestroyed: false, taiwuEventChangedBlock: false, -1, new List<sbyte>(), new short[6], 0, -1, new int[4] { 251, 252, 253, 254 }, "sect_jieqingya", new string[4] { "Ambience_map_4", " Ambience_map_4", " Ambience_map_4", " Ambience_map_4" }, 10, 2, 251, 8, "界青门", new List<short> { 48, 300, 315, 271, 46 }, new List<short> { 46 }, 27, new List<(short, short)> { (1, 100) }, "tex_sect_jieqingya", 0, -1, canGenerate: true));
		_dataArray.Add(new MapBlockItem(32, EMapBlockType.Sect, EMapBlockSubType.FulongTan, 255, 255, 4, 256, 2, 3, 5, 1, 1, freeStepIgnorePathCost: true, showTips: true, new string[1] { "MapGangFulong" }, new string[1] { "Organization_fulong/eff_organization_fulong" }, "Villagerwork_MapGangFulong", ignoreDestroyed: false, taiwuEventChangedBlock: false, -1, new List<sbyte>(), new short[6], 0, -1, new int[4] { 257, 258, 259, 260 }, "sect_fulongtan", new string[4] { "Ambience_map_fulongtan_1", " Ambience_map_fulongtan_1", " Ambience_map_fulongtan_1", " Ambience_map_fulongtan_2" }, 16, 4, 252, 8, "伏龙坛", new List<short> { 48, 301, 316, 272, 46, 46, 46, 47, 47, 47 }, new List<short> { 46, 46, 46, 47, 47, 47 }, 28, new List<(short, short)> { (1, 100) }, "tex_sect_fulongtan", 0, -1, canGenerate: true));
		_dataArray.Add(new MapBlockItem(33, EMapBlockType.Sect, EMapBlockSubType.XuehouJiao, 261, 261, 16, 262, 2, 3, 5, 1, 1, freeStepIgnorePathCost: true, showTips: true, new string[1] { "MapGangXuehou" }, new string[1] { "Organization_xuehou/eff_organization_xuehou" }, "Villagerwork_MapGangXuehou", ignoreDestroyed: false, taiwuEventChangedBlock: false, -1, new List<sbyte>(), new short[6], 0, -1, new int[4] { 263, 264, 265, 266 }, "sect_xuehoujiao_xueji", new string[4] { "Ambience_map_xuehou_2", " Ambience_map_xuehou_1", " Ambience_map_10", " Ambience_map_xuehou_2" }, 12, 3, 253, 8, "血犼教", new List<short> { 48, 302, 317, 273, 46, 47 }, new List<short> { 47, 47 }, 29, new List<(short, short)> { (1, 100) }, "tex_sect_xuehoujiao", 0, -1, canGenerate: true));
		_dataArray.Add(new MapBlockItem(34, EMapBlockType.Town, EMapBlockSubType.Village, 267, 267, 1, 268, 1, 3, 4, 1, 1, freeStepIgnorePathCost: true, showTips: true, new string[1] { "Cunzhuang_1" }, new string[1] { "" }, null, ignoreDestroyed: false, taiwuEventChangedBlock: false, -1, new List<sbyte>(), new short[6], 0, -1, new int[0], null, new string[1] { "Ambience_map_1" }, 10, -1, 254, 5, null, new List<short> { 48, 286, 46, 46, 49 }, new List<short> { 46, 46 }, -1, new List<(short, short)> { (1, 100) }, "tex_mapblockevent_town_village", 11, -1, canGenerate: true));
		_dataArray.Add(new MapBlockItem(35, EMapBlockType.Town, EMapBlockSubType.Town, 269, 269, 4, 270, 1, 3, 4, 1, 1, freeStepIgnorePathCost: true, showTips: true, new string[1] { "Shizhen_1" }, new string[1] { "" }, null, ignoreDestroyed: false, taiwuEventChangedBlock: false, -1, new List<sbyte>(), new short[6], 0, -1, new int[0], null, new string[1] { "Ambience_map_1" }, 12, -1, 255, 5, null, new List<short> { 48, 285, 46, 46, 46, 47, 47, 47 }, new List<short> { 85, 92, 99, 106, 113, 213 }, -1, new List<(short, short)> { (1, 100) }, "tex_mapblockevent_town_town", 20, -1, canGenerate: true));
		_dataArray.Add(new MapBlockItem(36, EMapBlockType.Town, EMapBlockSubType.WalledTown, 271, 271, 16, 272, 1, 3, 4, 1, 1, freeStepIgnorePathCost: true, showTips: true, new string[1] { "Guanzhai_1" }, new string[1] { "" }, null, ignoreDestroyed: false, taiwuEventChangedBlock: false, -1, new List<sbyte>(), new short[6], 0, -1, new int[0], null, new string[1] { "Ambience_map_11" }, 12, -1, 256, 5, null, new List<short> { 48, 287, 52, 46, 46, 46, 46, 46 }, new List<short> { 129, 139, 179, 169, 120, 203 }, -1, new List<(short, short)> { (1, 100) }, "tex_mapblockevent_town_walledtown", 10, -1, canGenerate: true));
		_dataArray.Add(new MapBlockItem(37, EMapBlockType.Station, EMapBlockSubType.Station, 273, 273, 4, 274, 1, 0, 3, 1, 1, freeStepIgnorePathCost: true, showTips: true, new string[1] { "Travefixed_1_1" }, new string[1] { "Station_1/eff_Station1" }, null, ignoreDestroyed: false, taiwuEventChangedBlock: false, -1, new List<sbyte>(), new short[6], 0, -1, new int[0], null, new string[1] { "Ambience_map_1" }, -1, -1, -1, -1, null, new List<short>(), new List<short>(), -1, new List<(short, short)> { (1, 100) }, "tex_station_travefixed", 11, -1, canGenerate: true));
		_dataArray.Add(new MapBlockItem(38, EMapBlockType.Station, EMapBlockSubType.Station, 275, 275, 16, 276, 1, 0, 1, 1, 1, freeStepIgnorePathCost: true, showTips: true, new string[1] { "Traveruined_1_1" }, new string[1] { "Station_2/eff_Station2" }, null, ignoreDestroyed: false, taiwuEventChangedBlock: false, -1, new List<sbyte>(), new short[6], 0, -1, new int[0], null, new string[1] { "" }, -1, -1, -1, -1, null, new List<short>(), new List<short>(), -1, new List<(short, short)> { (1, 100) }, "tex_station_traveruined", 11, -1, canGenerate: true));
		_dataArray.Add(new MapBlockItem(39, EMapBlockType.Developed, EMapBlockSubType.Farmland, 277, 278, 1, 279, 1, 0, 1, 1, 1, freeStepIgnorePathCost: true, showTips: true, new string[3] { "Farmland_1_1", "Farmland_2_1", "Farmland_3_1" }, new string[1] { "" }, null, ignoreDestroyed: false, taiwuEventChangedBlock: false, -1, new List<sbyte> { 0 }, new short[6] { 150, -10, -10, -10, -10, -10 }, 1, 1000, new int[0], null, new string[1] { "Ambience_map_1" }, -1, -1, -1, -1, null, new List<short>(), new List<short>(), -1, new List<(short, short)> { (1, 100) }, "tex_mapblockevent_developedl_farmland", 5, -1, canGenerate: true));
		_dataArray.Add(new MapBlockItem(40, EMapBlockType.Developed, EMapBlockSubType.Farmland, 277, 280, 4, 279, 1, 0, 1, 1, 1, freeStepIgnorePathCost: true, showTips: true, new string[3] { "Farmland_1_2", "Farmland_2_2", "Farmland_3_2" }, new string[1] { "" }, null, ignoreDestroyed: false, taiwuEventChangedBlock: false, -1, new List<sbyte> { 0 }, new short[6] { 150, -10, -10, -10, -10, -10 }, 1, 1000, new int[0], null, new string[1] { "Ambience_map_1" }, -1, -1, -1, -1, null, new List<short>(), new List<short>(), -1, new List<(short, short)> { (1, 100) }, "tex_mapblockevent_developedl_farmland", 5, -1, canGenerate: true));
		_dataArray.Add(new MapBlockItem(41, EMapBlockType.Developed, EMapBlockSubType.Farmland, 277, 281, 16, 279, 1, 0, 1, 1, 1, freeStepIgnorePathCost: true, showTips: true, new string[3] { "Farmland_1_3", "Farmland_2_3", "Farmland_3_3" }, new string[1] { "" }, null, ignoreDestroyed: false, taiwuEventChangedBlock: false, -1, new List<sbyte> { 0 }, new short[6] { 150, -10, -10, -10, -10, -10 }, 1, 1000, new int[0], null, new string[1] { "Ambience_map_1" }, -1, -1, -1, -1, null, new List<short>(), new List<short>(), -1, new List<(short, short)> { (1, 100) }, "tex_mapblockevent_developedl_farmland", 5, -1, canGenerate: true));
		_dataArray.Add(new MapBlockItem(42, EMapBlockType.Developed, EMapBlockSubType.Gardens, 282, 283, 1, 284, 1, 0, 1, 1, 1, freeStepIgnorePathCost: true, showTips: true, new string[3] { "Garden_1_1", "Garden_2_1", "Garden_3_1" }, new string[1] { "" }, null, ignoreDestroyed: false, taiwuEventChangedBlock: false, -1, new List<sbyte> { 1 }, new short[6] { -10, 150, -10, -10, -10, -10 }, 2, 1000, new int[0], null, new string[1] { "Ambience_map_1" }, -1, -1, -1, -1, null, new List<short>(), new List<short>(), -1, new List<(short, short)> { (1, 100) }, "tex_mapblockevent_developedl_gardens", 3, -1, canGenerate: true));
		_dataArray.Add(new MapBlockItem(43, EMapBlockType.Developed, EMapBlockSubType.Gardens, 282, 285, 4, 284, 1, 0, 1, 1, 1, freeStepIgnorePathCost: true, showTips: true, new string[3] { "Garden_1_2", "Garden_2_2", "Garden_3_2" }, new string[1] { "" }, null, ignoreDestroyed: false, taiwuEventChangedBlock: false, -1, new List<sbyte> { 1 }, new short[6] { -10, 150, -10, -10, -10, -10 }, 2, 1000, new int[0], null, new string[1] { "Ambience_map_1" }, -1, -1, -1, -1, null, new List<short>(), new List<short>(), -1, new List<(short, short)> { (1, 100) }, "tex_mapblockevent_developedl_gardens", 3, -1, canGenerate: true));
		_dataArray.Add(new MapBlockItem(44, EMapBlockType.Developed, EMapBlockSubType.Gardens, 282, 286, 16, 284, 1, 0, 1, 1, 1, freeStepIgnorePathCost: true, showTips: true, new string[3] { "Garden_1_3", "Garden_2_3", "Garden_3_3" }, new string[1] { "" }, null, ignoreDestroyed: false, taiwuEventChangedBlock: false, -1, new List<sbyte> { 1 }, new short[6] { -10, 150, -10, -10, -10, -10 }, 2, 1000, new int[0], null, new string[1] { "Ambience_map_1" }, -1, -1, -1, -1, null, new List<short>(), new List<short>(), -1, new List<(short, short)> { (1, 100) }, "tex_mapblockevent_developedl_gardens", 3, -1, canGenerate: true));
		_dataArray.Add(new MapBlockItem(45, EMapBlockType.Developed, EMapBlockSubType.StoneForest, 287, 288, 1, 289, 1, 0, 1, 2, 2, freeStepIgnorePathCost: true, showTips: true, new string[3] { "Stone_1_1", "Stone_2_1", "Stone_3_1" }, new string[1] { "" }, null, ignoreDestroyed: false, taiwuEventChangedBlock: false, -1, new List<sbyte> { 2 }, new short[6] { -10, -10, 150, -10, -10, -10 }, 3, 1000, new int[0], null, new string[1] { "Ambience_map_4" }, -1, -1, -1, -1, null, new List<short>(), new List<short>(), -1, new List<(short, short)> { (1, 100) }, "tex_mapblockevent_developedl_stoneforest", 37, -1, canGenerate: true));
		_dataArray.Add(new MapBlockItem(46, EMapBlockType.Developed, EMapBlockSubType.StoneForest, 287, 290, 4, 289, 1, 0, 1, 2, 2, freeStepIgnorePathCost: true, showTips: true, new string[3] { "Stone_1_2", "Stone_2_2", "Stone_3_2" }, new string[1] { "" }, null, ignoreDestroyed: false, taiwuEventChangedBlock: false, -1, new List<sbyte> { 2 }, new short[6] { -10, -10, 150, -10, -10, -10 }, 3, 1000, new int[0], null, new string[1] { "Ambience_map_4" }, -1, -1, -1, -1, null, new List<short>(), new List<short>(), -1, new List<(short, short)> { (1, 100) }, "tex_mapblockevent_developedl_stoneforest", 37, -1, canGenerate: true));
		_dataArray.Add(new MapBlockItem(47, EMapBlockType.Developed, EMapBlockSubType.StoneForest, 287, 291, 16, 289, 1, 0, 1, 2, 2, freeStepIgnorePathCost: true, showTips: true, new string[3] { "Stone_1_3", "Stone_2_3", "Stone_3_3" }, new string[1] { "" }, null, ignoreDestroyed: false, taiwuEventChangedBlock: false, -1, new List<sbyte> { 2 }, new short[6] { -10, -10, 150, -10, -10, -10 }, 3, 1000, new int[0], null, new string[1] { "Ambience_map_4" }, -1, -1, -1, -1, null, new List<short>(), new List<short>(), -1, new List<(short, short)> { (1, 100) }, "tex_mapblockevent_developedl_stoneforest", 37, -1, canGenerate: true));
		_dataArray.Add(new MapBlockItem(48, EMapBlockType.Developed, EMapBlockSubType.MulberryField, 292, 293, 4, 294, 1, 0, 1, 1, 1, freeStepIgnorePathCost: true, showTips: true, new string[3] { "Sang_1_1", "Sang_2_1", "Sang_3_1" }, new string[1] { "" }, null, ignoreDestroyed: false, taiwuEventChangedBlock: false, -1, new List<sbyte> { 4 }, new short[6] { -10, -10, -10, -10, 150, -10 }, 5, 1000, new int[0], null, new string[1] { "Ambience_map_2" }, -1, -1, -1, -1, null, new List<short>(), new List<short>(), -1, new List<(short, short)> { (1, 100) }, "tex_mapblockevent_developedl_herbalgarden", 5, -1, canGenerate: true));
		_dataArray.Add(new MapBlockItem(49, EMapBlockType.Developed, EMapBlockSubType.MulberryField, 292, 295, 16, 294, 1, 0, 1, 1, 1, freeStepIgnorePathCost: true, showTips: true, new string[3] { "Sang_1_2", "Sang_2_2", "Sang_3_2" }, new string[1] { "" }, null, ignoreDestroyed: false, taiwuEventChangedBlock: false, -1, new List<sbyte> { 4 }, new short[6] { -10, -10, -10, -10, 150, -10 }, 5, 1000, new int[0], null, new string[1] { "Ambience_map_2" }, -1, -1, -1, -1, null, new List<short>(), new List<short>(), -1, new List<(short, short)> { (1, 100) }, "tex_mapblockevent_developedl_herbalgarden", 5, -1, canGenerate: true));
		_dataArray.Add(new MapBlockItem(50, EMapBlockType.Developed, EMapBlockSubType.MulberryField, 292, 296, 1, 294, 1, 0, 1, 1, 1, freeStepIgnorePathCost: true, showTips: true, new string[3] { "Sang_1_3", "Sang_2_3", "Sang_3_3" }, new string[1] { "" }, null, ignoreDestroyed: false, taiwuEventChangedBlock: false, -1, new List<sbyte> { 4 }, new short[6] { -10, -10, -10, -10, 150, -10 }, 5, 1000, new int[0], null, new string[1] { "Ambience_map_2" }, -1, -1, -1, -1, null, new List<short>(), new List<short>(), -1, new List<(short, short)> { (1, 100) }, "tex_mapblockevent_developedl_herbalgarden", 5, -1, canGenerate: true));
		_dataArray.Add(new MapBlockItem(51, EMapBlockType.Developed, EMapBlockSubType.HerbalGarden, 297, 298, 4, 299, 1, 0, 1, 1, 1, freeStepIgnorePathCost: true, showTips: true, new string[3] { "Herb_1_1", "Herb_2_1", "Herb_3_1" }, new string[1] { "" }, null, ignoreDestroyed: false, taiwuEventChangedBlock: false, -1, new List<sbyte> { 5 }, new short[6] { -10, -10, -10, -10, -10, 150 }, 6, 1000, new int[0], null, new string[1] { "Ambience_map_1" }, -1, -1, -1, -1, null, new List<short>(), new List<short>(), -1, new List<(short, short)> { (1, 100) }, "tex_mapblockevent_developedl_mulberryfield", 3, -1, canGenerate: true));
		_dataArray.Add(new MapBlockItem(52, EMapBlockType.Developed, EMapBlockSubType.HerbalGarden, 297, 300, 16, 299, 1, 0, 1, 1, 1, freeStepIgnorePathCost: true, showTips: true, new string[3] { "Herb_1_2", "Herb_2_2", "Herb_3_2" }, new string[1] { "" }, null, ignoreDestroyed: false, taiwuEventChangedBlock: false, -1, new List<sbyte> { 5 }, new short[6] { -10, -10, -10, -10, -10, 150 }, 6, 1000, new int[0], null, new string[1] { "Ambience_map_1" }, -1, -1, -1, -1, null, new List<short>(), new List<short>(), -1, new List<(short, short)> { (1, 100) }, "tex_mapblockevent_developedl_mulberryfield", 3, -1, canGenerate: true));
		_dataArray.Add(new MapBlockItem(53, EMapBlockType.Developed, EMapBlockSubType.HerbalGarden, 297, 301, 4, 299, 1, 0, 1, 1, 1, freeStepIgnorePathCost: true, showTips: true, new string[3] { "Herb_1_3", "Herb_2_3", "Herb_3_3" }, new string[1] { "" }, null, ignoreDestroyed: false, taiwuEventChangedBlock: false, -1, new List<sbyte> { 5 }, new short[6] { -10, -10, -10, -10, -10, 150 }, 6, 1000, new int[0], null, new string[1] { "Ambience_map_1" }, -1, -1, -1, -1, null, new List<short>(), new List<short>(), -1, new List<(short, short)> { (1, 100) }, "tex_mapblockevent_developedl_mulberryfield", 3, -1, canGenerate: true));
		_dataArray.Add(new MapBlockItem(54, EMapBlockType.Developed, EMapBlockSubType.JadeMountain, 302, 303, 16, 304, 1, 0, 1, 2, 2, freeStepIgnorePathCost: true, showTips: true, new string[3] { "Jade_1_1", "Jade_2_1", "Jade_3_1" }, new string[1] { "" }, null, ignoreDestroyed: false, taiwuEventChangedBlock: false, -1, new List<sbyte> { 3 }, new short[6] { -10, -10, -10, 150, -10, -10 }, 4, 1000, new int[0], null, new string[1] { "Ambience_map_3" }, -1, -1, -1, -1, null, new List<short>(), new List<short>(), -1, new List<(short, short)> { (1, 100) }, "tex_mapblockevent_developedl_jademountain", 37, -1, canGenerate: true));
		_dataArray.Add(new MapBlockItem(55, EMapBlockType.Developed, EMapBlockSubType.JadeMountain, 302, 305, 1, 304, 1, 0, 1, 2, 2, freeStepIgnorePathCost: true, showTips: true, new string[3] { "Jade_1_2", "Jade_2_2", "Jade_3_2" }, new string[1] { "" }, null, ignoreDestroyed: false, taiwuEventChangedBlock: false, -1, new List<sbyte> { 3 }, new short[6] { -10, -10, -10, 150, -10, -10 }, 4, 1000, new int[0], null, new string[1] { "Ambience_map_3" }, -1, -1, -1, -1, null, new List<short>(), new List<short>(), -1, new List<(short, short)> { (1, 100) }, "tex_mapblockevent_developedl_jademountain", 37, -1, canGenerate: true));
		_dataArray.Add(new MapBlockItem(56, EMapBlockType.Developed, EMapBlockSubType.JadeMountain, 302, 306, 4, 304, 1, 0, 1, 2, 2, freeStepIgnorePathCost: true, showTips: true, new string[3] { "Jade_1_3", "Jade_2_3", "Jade_3_3" }, new string[1] { "" }, null, ignoreDestroyed: false, taiwuEventChangedBlock: false, -1, new List<sbyte> { 3 }, new short[6] { -10, -10, -10, 150, -10, -10 }, 4, 1000, new int[0], null, new string[1] { "Ambience_map_3" }, -1, -1, -1, -1, null, new List<short>(), new List<short>(), -1, new List<(short, short)> { (1, 100) }, "tex_mapblockevent_developedl_jademountain", 37, -1, canGenerate: true));
		_dataArray.Add(new MapBlockItem(57, EMapBlockType.Normal, EMapBlockSubType.Mountain, 307, 308, 16, 309, 1, 0, 2, 3, 3, freeStepIgnorePathCost: true, showTips: true, new string[3] { "Mountain_1_1", "Mountain_2_1", "Mountain_3_1" }, new string[1] { "" }, null, ignoreDestroyed: false, taiwuEventChangedBlock: false, 60, new List<sbyte> { 2 }, new short[6] { -18, -18, 125, -10, -18, -18 }, 9, 300, new int[0], null, new string[1] { "Ambience_map_4" }, -1, -1, -1, -1, null, new List<short>(), new List<short>(), -1, new List<(short, short)> { (1, 100) }, "tex_mapblockevent_normal_mountain", 8, 131, canGenerate: true));
		_dataArray.Add(new MapBlockItem(58, EMapBlockType.Normal, EMapBlockSubType.Mountain, 307, 310, 4, 309, 1, 0, 2, 3, 3, freeStepIgnorePathCost: true, showTips: true, new string[3] { "Mountain_1_2", "Mountain_2_2", "Mountain_3_2" }, new string[1] { "" }, null, ignoreDestroyed: false, taiwuEventChangedBlock: false, 61, new List<sbyte> { 2 }, new short[6] { -18, -18, 125, -10, -18, -18 }, 9, 300, new int[0], null, new string[1] { "Ambience_map_4" }, -1, -1, -1, -1, null, new List<short>(), new List<short>(), -1, new List<(short, short)> { (1, 100) }, "tex_mapblockevent_normal_mountain", 8, 131, canGenerate: true));
		_dataArray.Add(new MapBlockItem(59, EMapBlockType.Normal, EMapBlockSubType.Mountain, 307, 311, 16, 309, 1, 0, 2, 3, 3, freeStepIgnorePathCost: true, showTips: true, new string[3] { "Mountain_1_3", "Mountain_2_3", "Mountain_3_3" }, new string[1] { "" }, null, ignoreDestroyed: false, taiwuEventChangedBlock: false, 62, new List<sbyte> { 2 }, new short[6] { -18, -18, 125, -10, -18, -18 }, 9, 300, new int[0], null, new string[1] { "Ambience_map_4" }, -1, -1, -1, -1, null, new List<short>(), new List<short>(), -1, new List<(short, short)> { (1, 100) }, "tex_mapblockevent_normal_mountain", 8, 131, canGenerate: true));
	}

	private void CreateItems1()
	{
		_dataArray.Add(new MapBlockItem(60, EMapBlockType.Normal, EMapBlockSubType.BigMountain, 312, 313, 1, 314, 2, 0, 2, 3, 3, freeStepIgnorePathCost: true, showTips: true, new string[1] { "Mountain_big_1" }, new string[1] { "Block_Mountain/eff_block_mountain" }, "Villagerwork_Mountain_big", ignoreDestroyed: false, taiwuEventChangedBlock: false, 57, new List<sbyte> { 2 }, new short[6] { -18, -18, 125, -10, -18, -18 }, 9, 300, new int[0], null, new string[1] { "Ambience_map_4" }, -1, -1, -1, -1, null, new List<short>(), new List<short>(), -1, new List<(short, short)> { (1, 100) }, "tex_mapblockevent_normal_mountain", 8, 131, canGenerate: true));
		_dataArray.Add(new MapBlockItem(61, EMapBlockType.Normal, EMapBlockSubType.BigMountain, 312, 315, 4, 314, 2, 0, 2, 3, 3, freeStepIgnorePathCost: true, showTips: true, new string[1] { "Mountain_big_2" }, new string[1] { "Block_Mountain/eff_block_mountain" }, "Villagerwork_Mountain_big", ignoreDestroyed: false, taiwuEventChangedBlock: false, 58, new List<sbyte> { 2 }, new short[6] { -18, -18, 125, -10, -18, -18 }, 9, 300, new int[0], null, new string[1] { "Ambience_map_4" }, -1, -1, -1, -1, null, new List<short>(), new List<short>(), -1, new List<(short, short)> { (1, 100) }, "tex_mapblockevent_normal_mountain", 8, 131, canGenerate: true));
		_dataArray.Add(new MapBlockItem(62, EMapBlockType.Normal, EMapBlockSubType.BigMountain, 312, 316, 16, 314, 2, 0, 2, 3, 3, freeStepIgnorePathCost: true, showTips: true, new string[1] { "Mountain_big_3" }, new string[1] { "Block_Mountain/eff_block_mountain" }, "Villagerwork_Mountain_big", ignoreDestroyed: false, taiwuEventChangedBlock: false, 59, new List<sbyte> { 2 }, new short[6] { -18, -18, 125, -10, -18, -18 }, 9, 300, new int[0], null, new string[1] { "Ambience_map_4" }, -1, -1, -1, -1, null, new List<short>(), new List<short>(), -1, new List<(short, short)> { (1, 100) }, "tex_mapblockevent_normal_mountain", 8, 131, canGenerate: true));
		_dataArray.Add(new MapBlockItem(63, EMapBlockType.Normal, EMapBlockSubType.Canyon, 317, 318, 1, 319, 1, 0, 1, 2, 2, freeStepIgnorePathCost: true, showTips: true, new string[3] { "Valley_1_1", "Valley_2_1", "Valley_3_1" }, new string[1] { "" }, null, ignoreDestroyed: false, taiwuEventChangedBlock: false, 66, new List<sbyte> { 5 }, new short[6] { -18, -18, -18, -18, -18, 125 }, 12, 300, new int[0], null, new string[1] { "Ambience_map_4" }, -1, -1, -1, -1, null, new List<short>(), new List<short>(), -1, new List<(short, short)> { (1, 100) }, "tex_mapblockevent_normal_canyon", 22, 132, canGenerate: true));
		_dataArray.Add(new MapBlockItem(64, EMapBlockType.Normal, EMapBlockSubType.Canyon, 317, 320, 4, 319, 1, 0, 1, 2, 2, freeStepIgnorePathCost: true, showTips: true, new string[3] { "Valley_1_2", "Valley_2_2", "Valley_3_2" }, new string[1] { "" }, null, ignoreDestroyed: false, taiwuEventChangedBlock: false, 67, new List<sbyte> { 5 }, new short[6] { -18, -18, -18, -18, -18, 125 }, 12, 300, new int[0], null, new string[1] { "Ambience_map_4" }, -1, -1, -1, -1, null, new List<short>(), new List<short>(), -1, new List<(short, short)> { (1, 100) }, "tex_mapblockevent_normal_canyon", 22, 132, canGenerate: true));
		_dataArray.Add(new MapBlockItem(65, EMapBlockType.Normal, EMapBlockSubType.Canyon, 317, 321, 16, 319, 1, 0, 1, 2, 2, freeStepIgnorePathCost: true, showTips: true, new string[3] { "Valley_1_3", "Valley_2_3", "Valley_3_3" }, new string[1] { "" }, null, ignoreDestroyed: false, taiwuEventChangedBlock: false, 68, new List<sbyte> { 5 }, new short[6] { -18, -18, -18, -18, -18, 125 }, 12, 300, new int[0], null, new string[1] { "Ambience_map_4" }, -1, -1, -1, -1, null, new List<short>(), new List<short>(), -1, new List<(short, short)> { (1, 100) }, "tex_mapblockevent_normal_canyon", 22, 132, canGenerate: true));
		_dataArray.Add(new MapBlockItem(66, EMapBlockType.Normal, EMapBlockSubType.BigCanyon, 322, 323, 1, 324, 2, 0, 2, 2, 2, freeStepIgnorePathCost: true, showTips: true, new string[1] { "Valley_big_1" }, new string[1] { "Block_Valley/eff_block_valley" }, "Villagerwork_Valley_big", ignoreDestroyed: false, taiwuEventChangedBlock: false, 63, new List<sbyte> { 5 }, new short[6] { -18, -18, -18, -18, -18, 125 }, 12, 300, new int[0], null, new string[1] { "Ambience_map_4" }, -1, -1, -1, -1, null, new List<short>(), new List<short>(), -1, new List<(short, short)> { (1, 100) }, "tex_mapblockevent_normal_canyon", 22, 132, canGenerate: true));
		_dataArray.Add(new MapBlockItem(67, EMapBlockType.Normal, EMapBlockSubType.BigCanyon, 322, 325, 4, 324, 2, 0, 2, 2, 2, freeStepIgnorePathCost: true, showTips: true, new string[1] { "Valley_big_1" }, new string[1] { "Block_Valley/eff_block_valley" }, "Villagerwork_Valley_big", ignoreDestroyed: false, taiwuEventChangedBlock: false, 64, new List<sbyte> { 5 }, new short[6] { -18, -18, -18, -18, -18, 125 }, 12, 300, new int[0], null, new string[1] { "Ambience_map_4" }, -1, -1, -1, -1, null, new List<short>(), new List<short>(), -1, new List<(short, short)> { (1, 100) }, "tex_mapblockevent_normal_canyon", 22, 132, canGenerate: true));
		_dataArray.Add(new MapBlockItem(68, EMapBlockType.Normal, EMapBlockSubType.BigCanyon, 322, 326, 16, 324, 2, 0, 2, 2, 2, freeStepIgnorePathCost: true, showTips: true, new string[1] { "Valley_big_1" }, new string[1] { "Block_Valley/eff_block_valley" }, "Villagerwork_Valley_big", ignoreDestroyed: false, taiwuEventChangedBlock: false, 65, new List<sbyte> { 5 }, new short[6] { -18, -18, -18, -18, -18, 125 }, 12, 300, new int[0], null, new string[1] { "Ambience_map_4" }, -1, -1, -1, -1, null, new List<short>(), new List<short>(), -1, new List<(short, short)> { (1, 100) }, "tex_mapblockevent_normal_canyon", 22, 132, canGenerate: true));
		_dataArray.Add(new MapBlockItem(69, EMapBlockType.Normal, EMapBlockSubType.Hill, 327, 328, 4, 329, 1, 0, 1, 2, 2, freeStepIgnorePathCost: true, showTips: true, new string[3] { "Hill_1_1", "Hill_2_1", "Hill_3_1" }, new string[1] { "" }, null, ignoreDestroyed: false, taiwuEventChangedBlock: false, 72, new List<sbyte> { 4 }, new short[6] { -18, -18, -18, -18, 125, -18 }, 11, 300, new int[0], null, new string[1] { "Ambience_map_3" }, -1, -1, -1, -1, null, new List<short>(), new List<short>(), -1, new List<(short, short)> { (1, 100) }, "tex_mapblockevent_normal_hill", 1, 133, canGenerate: true));
		_dataArray.Add(new MapBlockItem(70, EMapBlockType.Normal, EMapBlockSubType.Hill, 327, 330, 16, 329, 1, 0, 1, 2, 2, freeStepIgnorePathCost: true, showTips: true, new string[3] { "Hill_1_2", "Hill_2_2", "Hill_3_2" }, new string[1] { "" }, null, ignoreDestroyed: false, taiwuEventChangedBlock: false, 73, new List<sbyte> { 4 }, new short[6] { -18, -18, -18, -18, 125, -18 }, 11, 300, new int[0], null, new string[1] { "Ambience_map_3" }, -1, -1, -1, -1, null, new List<short>(), new List<short>(), -1, new List<(short, short)> { (1, 100) }, "tex_mapblockevent_normal_hill", 1, 133, canGenerate: true));
		_dataArray.Add(new MapBlockItem(71, EMapBlockType.Normal, EMapBlockSubType.Hill, 327, 331, 1, 329, 1, 0, 1, 2, 2, freeStepIgnorePathCost: true, showTips: true, new string[3] { "Hill_1_3", "Hill_2_3", "Hill_3_3" }, new string[1] { "" }, null, ignoreDestroyed: false, taiwuEventChangedBlock: false, 74, new List<sbyte> { 4 }, new short[6] { -18, -18, -18, -18, 125, -18 }, 11, 300, new int[0], null, new string[1] { "Ambience_map_3" }, -1, -1, -1, -1, null, new List<short>(), new List<short>(), -1, new List<(short, short)> { (1, 100) }, "tex_mapblockevent_normal_hill", 1, 133, canGenerate: true));
		_dataArray.Add(new MapBlockItem(72, EMapBlockType.Normal, EMapBlockSubType.BigHill, 332, 333, 4, 334, 2, 0, 2, 2, 2, freeStepIgnorePathCost: true, showTips: true, new string[1] { "Hill_big_1" }, new string[1] { "Block_Hill/eff_block_hill" }, "Villagerwork_Hill_big", ignoreDestroyed: false, taiwuEventChangedBlock: false, 69, new List<sbyte> { 4 }, new short[6] { -18, -18, -18, -18, 125, -18 }, 11, 300, new int[0], null, new string[1] { "Ambience_map_3" }, -1, -1, -1, -1, null, new List<short>(), new List<short>(), -1, new List<(short, short)> { (1, 100) }, "tex_mapblockevent_normal_hill", 1, 133, canGenerate: true));
		_dataArray.Add(new MapBlockItem(73, EMapBlockType.Normal, EMapBlockSubType.BigHill, 332, 335, 16, 334, 2, 0, 2, 2, 2, freeStepIgnorePathCost: true, showTips: true, new string[1] { "Hill_big_1" }, new string[1] { "Block_Hill/eff_block_hill" }, "Villagerwork_Hill_big", ignoreDestroyed: false, taiwuEventChangedBlock: false, 70, new List<sbyte> { 4 }, new short[6] { -18, -18, -18, -18, 125, -18 }, 11, 300, new int[0], null, new string[1] { "Ambience_map_3" }, -1, -1, -1, -1, null, new List<short>(), new List<short>(), -1, new List<(short, short)> { (1, 100) }, "tex_mapblockevent_normal_hill", 1, 133, canGenerate: true));
		_dataArray.Add(new MapBlockItem(74, EMapBlockType.Normal, EMapBlockSubType.BigHill, 332, 336, 4, 334, 2, 0, 2, 2, 2, freeStepIgnorePathCost: true, showTips: true, new string[1] { "Hill_big_1" }, new string[1] { "Block_Hill/eff_block_hill" }, "Villagerwork_Hill_big", ignoreDestroyed: false, taiwuEventChangedBlock: false, 71, new List<sbyte> { 4 }, new short[6] { -18, -18, -18, -18, 125, -18 }, 11, 300, new int[0], null, new string[1] { "Ambience_map_3" }, -1, -1, -1, -1, null, new List<short>(), new List<short>(), -1, new List<(short, short)> { (1, 100) }, "tex_mapblockevent_normal_hill", 1, 133, canGenerate: true));
		_dataArray.Add(new MapBlockItem(75, EMapBlockType.Normal, EMapBlockSubType.Field, 337, 338, 16, 339, 1, 0, 1, 1, 1, freeStepIgnorePathCost: true, showTips: true, new string[3] { "Filed_1_1", "Filed_2_1", "Filed_3_1" }, new string[1] { "" }, null, ignoreDestroyed: false, taiwuEventChangedBlock: false, 78, new List<sbyte> { 0 }, new short[6] { 125, -18, -18, -18, -18, -18 }, 7, 300, new int[0], null, new string[1] { "Ambience_map_1" }, -1, -1, -1, -1, null, new List<short>(), new List<short>(), -1, new List<(short, short)> { (1, 100) }, "tex_mapblockevent_normal_field", 18, 134, canGenerate: true));
		_dataArray.Add(new MapBlockItem(76, EMapBlockType.Normal, EMapBlockSubType.Field, 337, 340, 1, 339, 1, 0, 1, 1, 1, freeStepIgnorePathCost: true, showTips: true, new string[3] { "Filed_1_2", "Filed_2_2", "Filed_3_2" }, new string[1] { "" }, null, ignoreDestroyed: false, taiwuEventChangedBlock: false, 79, new List<sbyte> { 0 }, new short[6] { 125, -18, -18, -18, -18, -18 }, 7, 300, new int[0], null, new string[1] { "Ambience_map_1" }, -1, -1, -1, -1, null, new List<short>(), new List<short>(), -1, new List<(short, short)> { (1, 100) }, "tex_mapblockevent_normal_field", 18, 134, canGenerate: true));
		_dataArray.Add(new MapBlockItem(77, EMapBlockType.Normal, EMapBlockSubType.Field, 337, 341, 4, 339, 1, 0, 1, 1, 1, freeStepIgnorePathCost: true, showTips: true, new string[3] { "Filed_1_3", "Filed_2_3", "Filed_3_3" }, new string[1] { "" }, null, ignoreDestroyed: false, taiwuEventChangedBlock: false, 80, new List<sbyte> { 0 }, new short[6] { 125, -18, -18, -18, -18, -18 }, 7, 300, new int[0], null, new string[1] { "Ambience_map_1" }, -1, -1, -1, -1, null, new List<short>(), new List<short>(), -1, new List<(short, short)> { (1, 100) }, "tex_mapblockevent_normal_field", 18, 134, canGenerate: true));
		_dataArray.Add(new MapBlockItem(78, EMapBlockType.Normal, EMapBlockSubType.BigField, 342, 343, 16, 344, 2, 0, 2, 1, 1, freeStepIgnorePathCost: true, showTips: true, new string[1] { "Filed_big_1" }, new string[1] { "Block_Filed/eff_block_filed" }, "Villagerwork_Filed_big", ignoreDestroyed: false, taiwuEventChangedBlock: false, 75, new List<sbyte> { 0 }, new short[6] { 125, -18, -18, -18, -18, -18 }, 7, 300, new int[0], null, new string[1] { "Ambience_map_1" }, -1, -1, -1, -1, null, new List<short>(), new List<short>(), -1, new List<(short, short)> { (1, 100) }, "tex_mapblockevent_normal_field", 18, 134, canGenerate: true));
		_dataArray.Add(new MapBlockItem(79, EMapBlockType.Normal, EMapBlockSubType.BigField, 342, 345, 4, 344, 2, 0, 2, 1, 1, freeStepIgnorePathCost: true, showTips: true, new string[1] { "Filed_big_2" }, new string[1] { "Block_Filed/eff_block_filed" }, "Villagerwork_Filed_big", ignoreDestroyed: false, taiwuEventChangedBlock: false, 76, new List<sbyte> { 0 }, new short[6] { 125, -18, -18, -18, -18, -18 }, 7, 300, new int[0], null, new string[1] { "Ambience_map_1" }, -1, -1, -1, -1, null, new List<short>(), new List<short>(), -1, new List<(short, short)> { (1, 100) }, "tex_mapblockevent_normal_field", 18, 134, canGenerate: true));
		_dataArray.Add(new MapBlockItem(80, EMapBlockType.Normal, EMapBlockSubType.BigField, 342, 346, 16, 344, 2, 0, 2, 1, 1, freeStepIgnorePathCost: true, showTips: true, new string[1] { "Filed_big_3" }, new string[1] { "Block_Filed/eff_block_filed" }, "Villagerwork_Filed_big", ignoreDestroyed: false, taiwuEventChangedBlock: false, 77, new List<sbyte> { 0 }, new short[6] { 125, -18, -18, -18, -18, -18 }, 7, 300, new int[0], null, new string[1] { "Ambience_map_1" }, -1, -1, -1, -1, null, new List<short>(), new List<short>(), -1, new List<(short, short)> { (1, 100) }, "tex_mapblockevent_normal_field", 18, 134, canGenerate: true));
		_dataArray.Add(new MapBlockItem(81, EMapBlockType.Normal, EMapBlockSubType.Woodland, 347, 348, 1, 349, 1, 0, 1, 2, 2, freeStepIgnorePathCost: true, showTips: true, new string[3] { "Wood_1_1", "Wood_2_1", "Wood_3_1" }, new string[1] { "" }, null, ignoreDestroyed: false, taiwuEventChangedBlock: false, 84, new List<sbyte> { 1 }, new short[6] { -18, 125, -18, -18, -18, -18 }, 8, 300, new int[0], null, new string[1] { "Ambience_map_6" }, -1, -1, -1, -1, null, new List<short>(), new List<short>(), -1, new List<(short, short)> { (1, 100) }, "tex_mapblockevent_normal_woodland", 16, 135, canGenerate: true));
		_dataArray.Add(new MapBlockItem(82, EMapBlockType.Normal, EMapBlockSubType.Woodland, 347, 350, 4, 349, 1, 0, 1, 2, 2, freeStepIgnorePathCost: true, showTips: true, new string[3] { "Wood_1_2", "Wood_2_2", "Wood_3_2" }, new string[1] { "" }, null, ignoreDestroyed: false, taiwuEventChangedBlock: false, 85, new List<sbyte> { 1 }, new short[6] { -18, 125, -18, -18, -18, -18 }, 8, 300, new int[0], null, new string[1] { "Ambience_map_6" }, -1, -1, -1, -1, null, new List<short>(), new List<short>(), -1, new List<(short, short)> { (1, 100) }, "tex_mapblockevent_normal_woodland", 16, 135, canGenerate: true));
		_dataArray.Add(new MapBlockItem(83, EMapBlockType.Normal, EMapBlockSubType.Woodland, 347, 351, 16, 349, 1, 0, 1, 2, 2, freeStepIgnorePathCost: true, showTips: true, new string[3] { "Wood_1_3", "Wood_2_3", "Wood_3_3" }, new string[1] { "" }, null, ignoreDestroyed: false, taiwuEventChangedBlock: false, 86, new List<sbyte> { 1 }, new short[6] { -18, 125, -18, -18, -18, -18 }, 8, 300, new int[0], null, new string[1] { "Ambience_map_6" }, -1, -1, -1, -1, null, new List<short>(), new List<short>(), -1, new List<(short, short)> { (1, 100) }, "tex_mapblockevent_normal_woodland", 16, 135, canGenerate: true));
		_dataArray.Add(new MapBlockItem(84, EMapBlockType.Normal, EMapBlockSubType.BigWoodland, 352, 353, 1, 354, 2, 0, 2, 2, 2, freeStepIgnorePathCost: true, showTips: true, new string[1] { "Wood_big_1" }, new string[1] { "Block_Wood/eff_block_wood" }, "Villagerwork_Wood_big", ignoreDestroyed: false, taiwuEventChangedBlock: false, 81, new List<sbyte> { 1 }, new short[6] { -18, 125, -18, -18, -18, -18 }, 8, 300, new int[0], null, new string[1] { "Ambience_map_6" }, -1, -1, -1, -1, null, new List<short>(), new List<short>(), -1, new List<(short, short)> { (1, 100) }, "tex_mapblockevent_normal_woodland", 16, 135, canGenerate: true));
		_dataArray.Add(new MapBlockItem(85, EMapBlockType.Normal, EMapBlockSubType.BigWoodland, 352, 355, 4, 354, 2, 0, 2, 2, 2, freeStepIgnorePathCost: true, showTips: true, new string[1] { "Wood_big_1" }, new string[1] { "Block_Wood/eff_block_wood" }, "Villagerwork_Wood_big", ignoreDestroyed: false, taiwuEventChangedBlock: false, 82, new List<sbyte> { 1 }, new short[6] { -18, 125, -18, -18, -18, -18 }, 8, 300, new int[0], null, new string[1] { "Ambience_map_6" }, -1, -1, -1, -1, null, new List<short>(), new List<short>(), -1, new List<(short, short)> { (1, 100) }, "tex_mapblockevent_normal_woodland", 16, 135, canGenerate: true));
		_dataArray.Add(new MapBlockItem(86, EMapBlockType.Normal, EMapBlockSubType.BigWoodland, 352, 356, 16, 354, 2, 0, 2, 2, 2, freeStepIgnorePathCost: true, showTips: true, new string[1] { "Wood_big_1" }, new string[1] { "Block_Wood/eff_block_wood" }, "Villagerwork_Wood_big", ignoreDestroyed: false, taiwuEventChangedBlock: false, 83, new List<sbyte> { 1 }, new short[6] { -18, 125, -18, -18, -18, -18 }, 8, 300, new int[0], null, new string[1] { "Ambience_map_6" }, -1, -1, -1, -1, null, new List<short>(), new List<short>(), -1, new List<(short, short)> { (1, 100) }, "tex_mapblockevent_normal_woodland", 16, 135, canGenerate: true));
		_dataArray.Add(new MapBlockItem(87, EMapBlockType.Normal, EMapBlockSubType.RiverBeach, 357, 358, 1, 359, 1, 0, 1, 1, 1, freeStepIgnorePathCost: true, showTips: true, new string[3] { "River_1_1", "River_2_1", "River_3_1" }, new string[1] { "" }, null, ignoreDestroyed: false, taiwuEventChangedBlock: false, 90, new List<sbyte> { 3 }, new short[6] { -18, -18, -18, 125, -18, -18 }, 10, 300, new int[0], null, new string[1] { "Ambience_map_5" }, -1, -1, -1, -1, null, new List<short>(), new List<short>(), -1, new List<(short, short)> { (1, 100) }, "tex_mapblockevent_normal_riverbeach", 38, 136, canGenerate: true));
		_dataArray.Add(new MapBlockItem(88, EMapBlockType.Normal, EMapBlockSubType.RiverBeach, 357, 360, 4, 359, 1, 0, 1, 1, 1, freeStepIgnorePathCost: true, showTips: true, new string[3] { "River_1_2", "River_2_2", "River_3_2" }, new string[1] { "" }, null, ignoreDestroyed: false, taiwuEventChangedBlock: false, 91, new List<sbyte> { 3 }, new short[6] { -18, -18, -18, 125, -18, -18 }, 10, 300, new int[0], null, new string[1] { "Ambience_map_5" }, -1, -1, -1, -1, null, new List<short>(), new List<short>(), -1, new List<(short, short)> { (1, 100) }, "tex_mapblockevent_normal_riverbeach", 38, 136, canGenerate: true));
		_dataArray.Add(new MapBlockItem(89, EMapBlockType.Normal, EMapBlockSubType.RiverBeach, 357, 361, 16, 359, 1, 0, 1, 1, 1, freeStepIgnorePathCost: true, showTips: true, new string[3] { "River_1_3", "River_2_3", "River_3_3" }, new string[1] { "" }, null, ignoreDestroyed: false, taiwuEventChangedBlock: false, 92, new List<sbyte> { 3 }, new short[6] { -18, -18, -18, 125, -18, -18 }, 10, 300, new int[0], null, new string[1] { "Ambience_map_5" }, -1, -1, -1, -1, null, new List<short>(), new List<short>(), -1, new List<(short, short)> { (1, 100) }, "tex_mapblockevent_normal_riverbeach", 38, 136, canGenerate: true));
		_dataArray.Add(new MapBlockItem(90, EMapBlockType.Normal, EMapBlockSubType.BigRiverBeach, 362, 363, 4, 364, 2, 0, 2, 1, 1, freeStepIgnorePathCost: true, showTips: true, new string[1] { "River_big_1" }, new string[1] { "Block_River/eff_block_river" }, "Villagerwork_River_big", ignoreDestroyed: false, taiwuEventChangedBlock: false, 87, new List<sbyte> { 3 }, new short[6] { -18, -18, -18, 125, -18, -18 }, 10, 300, new int[0], null, new string[1] { "Ambience_map_5" }, -1, -1, -1, -1, null, new List<short>(), new List<short>(), -1, new List<(short, short)> { (1, 100) }, "tex_mapblockevent_normal_riverbeach", 38, 136, canGenerate: true));
		_dataArray.Add(new MapBlockItem(91, EMapBlockType.Normal, EMapBlockSubType.BigRiverBeach, 362, 365, 16, 364, 2, 0, 2, 1, 1, freeStepIgnorePathCost: true, showTips: true, new string[1] { "River_big_1" }, new string[1] { "Block_River/eff_block_river" }, "Villagerwork_River_big", ignoreDestroyed: false, taiwuEventChangedBlock: false, 88, new List<sbyte> { 3 }, new short[6] { -18, -18, -18, 125, -18, -18 }, 10, 300, new int[0], null, new string[1] { "Ambience_map_5" }, -1, -1, -1, -1, null, new List<short>(), new List<short>(), -1, new List<(short, short)> { (1, 100) }, "tex_mapblockevent_normal_riverbeach", 38, 136, canGenerate: true));
		_dataArray.Add(new MapBlockItem(92, EMapBlockType.Normal, EMapBlockSubType.BigRiverBeach, 362, 366, 1, 364, 2, 0, 2, 1, 1, freeStepIgnorePathCost: true, showTips: true, new string[1] { "River_big_1" }, new string[1] { "Block_River/eff_block_river" }, "Villagerwork_River_big", ignoreDestroyed: false, taiwuEventChangedBlock: false, 89, new List<sbyte> { 3 }, new short[6] { -18, -18, -18, 125, -18, -18 }, 10, 300, new int[0], null, new string[1] { "Ambience_map_5" }, -1, -1, -1, -1, null, new List<short>(), new List<short>(), -1, new List<(short, short)> { (1, 100) }, "tex_mapblockevent_normal_riverbeach", 38, 136, canGenerate: true));
		_dataArray.Add(new MapBlockItem(93, EMapBlockType.Wild, EMapBlockSubType.Lake, 367, 367, 4, 368, 1, 0, 1, 1, 1, freeStepIgnorePathCost: true, showTips: true, new string[1] { "Lake_1" }, new string[3] { "Block_Water/eff_block_water", "Block_Water/eff_block_water_1", "Block_Water/eff_block_water_2" }, "Villagerwork_Lake", ignoreDestroyed: false, taiwuEventChangedBlock: false, -1, new List<sbyte> { 0 }, new short[6] { 100, -14, -14, -14, -14, -14 }, 13, 300, new int[0], null, new string[1] { "Ambience_map_12" }, -1, -1, -1, -1, null, new List<short>(), new List<short>(), -1, new List<(short, short)> { (1, 100) }, "tex_mapblockevent_wild_lake", 13, 137, canGenerate: true));
		_dataArray.Add(new MapBlockItem(94, EMapBlockType.Wild, EMapBlockSubType.Jungle, 369, 370, 16, 371, 1, 0, 1, 3, 3, freeStepIgnorePathCost: true, showTips: true, new string[3] { "Forest_1_1", "Forest_2_1", "Forest_3_1" }, new string[1] { "" }, null, ignoreDestroyed: false, taiwuEventChangedBlock: false, -1, new List<sbyte> { 1 }, new short[6] { -14, 100, -14, -14, -14, -14 }, 14, 300, new int[0], null, new string[1] { "Ambience_map_6" }, -1, -1, -1, -1, null, new List<short>(), new List<short>(), -1, new List<(short, short)> { (1, 100) }, "tex_mapblockevent_wild_jungle", 16, 138, canGenerate: true));
		_dataArray.Add(new MapBlockItem(95, EMapBlockType.Wild, EMapBlockSubType.Jungle, 369, 372, 4, 371, 1, 0, 1, 3, 3, freeStepIgnorePathCost: true, showTips: true, new string[3] { "Forest_1_2", "Forest_2_2", "Forest_3_2" }, new string[1] { "" }, null, ignoreDestroyed: false, taiwuEventChangedBlock: false, -1, new List<sbyte> { 1 }, new short[6] { -14, 100, -14, -14, -14, -14 }, 14, 300, new int[0], null, new string[1] { "Ambience_map_6" }, -1, -1, -1, -1, null, new List<short>(), new List<short>(), -1, new List<(short, short)> { (1, 100) }, "tex_mapblockevent_wild_jungle", 16, 138, canGenerate: true));
		_dataArray.Add(new MapBlockItem(96, EMapBlockType.Wild, EMapBlockSubType.Jungle, 369, 373, 16, 371, 1, 0, 1, 3, 3, freeStepIgnorePathCost: true, showTips: true, new string[3] { "Forest_1_3", "Forest_2_3", "Forest_3_3" }, new string[1] { "" }, null, ignoreDestroyed: false, taiwuEventChangedBlock: false, -1, new List<sbyte> { 1 }, new short[6] { -14, 100, -14, -14, -14, -14 }, 14, 300, new int[0], null, new string[1] { "Ambience_map_6" }, -1, -1, -1, -1, null, new List<short>(), new List<short>(), -1, new List<(short, short)> { (1, 100) }, "tex_mapblockevent_wild_jungle", 16, 138, canGenerate: true));
		_dataArray.Add(new MapBlockItem(97, EMapBlockType.Wild, EMapBlockSubType.Cave, 374, 375, 1, 376, 1, 0, 1, 2, 2, freeStepIgnorePathCost: true, showTips: true, new string[3] { "Cave_1_1", "Cave_2_1", "Cave_3_1" }, new string[1] { "" }, null, ignoreDestroyed: false, taiwuEventChangedBlock: false, -1, new List<sbyte> { 2 }, new short[6] { -14, -14, 100, -14, -14, -14 }, 15, 300, new int[0], null, new string[1] { "Ambience_map_4" }, -1, -1, -1, -1, null, new List<short>(), new List<short>(), -1, new List<(short, short)> { (1, 100) }, "tex_mapblockevent_wild_cave", 2, 139, canGenerate: true));
		_dataArray.Add(new MapBlockItem(98, EMapBlockType.Wild, EMapBlockSubType.Cave, 374, 377, 4, 376, 1, 0, 1, 2, 2, freeStepIgnorePathCost: true, showTips: true, new string[3] { "Cave_1_2", "Cave_2_2", "Cave_3_2" }, new string[1] { "" }, null, ignoreDestroyed: false, taiwuEventChangedBlock: false, -1, new List<sbyte> { 2 }, new short[6] { -14, -14, 100, -14, -14, -14 }, 15, 300, new int[0], null, new string[1] { "Ambience_map_4" }, -1, -1, -1, -1, null, new List<short>(), new List<short>(), -1, new List<(short, short)> { (1, 100) }, "tex_mapblockevent_wild_cave", 2, 139, canGenerate: true));
		_dataArray.Add(new MapBlockItem(99, EMapBlockType.Wild, EMapBlockSubType.Cave, 374, 378, 16, 376, 1, 0, 1, 2, 2, freeStepIgnorePathCost: true, showTips: true, new string[3] { "Cave_1_3", "Cave_2_3", "Cave_3_3" }, new string[1] { "" }, null, ignoreDestroyed: false, taiwuEventChangedBlock: false, -1, new List<sbyte> { 2 }, new short[6] { -14, -14, 100, -14, -14, -14 }, 15, 300, new int[0], null, new string[1] { "Ambience_map_4" }, -1, -1, -1, -1, null, new List<short>(), new List<short>(), -1, new List<(short, short)> { (1, 100) }, "tex_mapblockevent_wild_cave", 2, 139, canGenerate: true));
		_dataArray.Add(new MapBlockItem(100, EMapBlockType.Wild, EMapBlockSubType.Swamp, 379, 380, 4, 381, 1, 0, 1, 3, 3, freeStepIgnorePathCost: true, showTips: true, new string[3] { "Swamp_1_1", "Swamp_2_1", "Swamp_3_1" }, new string[1] { "" }, null, ignoreDestroyed: false, taiwuEventChangedBlock: false, -1, new List<sbyte> { 5 }, new short[6] { -14, -14, -14, -14, -14, 100 }, 18, 300, new int[0], null, new string[1] { "Ambience_map_9" }, -1, -1, -1, -1, null, new List<short>(), new List<short>(), -1, new List<(short, short)> { (1, 100) }, "tex_mapblockevent_wild_swamp", 12, 140, canGenerate: true));
		_dataArray.Add(new MapBlockItem(101, EMapBlockType.Wild, EMapBlockSubType.Swamp, 379, 382, 16, 381, 1, 0, 1, 3, 3, freeStepIgnorePathCost: true, showTips: true, new string[3] { "Swamp_1_2", "Swamp_2_2", "Swamp_3_2" }, new string[1] { "" }, null, ignoreDestroyed: false, taiwuEventChangedBlock: false, -1, new List<sbyte> { 5 }, new short[6] { -14, -14, -14, -14, -14, 100 }, 18, 300, new int[0], null, new string[1] { "Ambience_map_9" }, -1, -1, -1, -1, null, new List<short>(), new List<short>(), -1, new List<(short, short)> { (1, 100) }, "tex_mapblockevent_wild_swamp", 12, 140, canGenerate: true));
		_dataArray.Add(new MapBlockItem(102, EMapBlockType.Wild, EMapBlockSubType.Swamp, 379, 383, 1, 381, 1, 0, 1, 3, 3, freeStepIgnorePathCost: true, showTips: true, new string[3] { "Swamp_1_3", "Swamp_2_3", "Swamp_3_3" }, new string[1] { "" }, null, ignoreDestroyed: false, taiwuEventChangedBlock: false, -1, new List<sbyte> { 5 }, new short[6] { -14, -14, -14, -14, -14, 100 }, 18, 300, new int[0], null, new string[1] { "Ambience_map_9" }, -1, -1, -1, -1, null, new List<short>(), new List<short>(), -1, new List<(short, short)> { (1, 100) }, "tex_mapblockevent_wild_swamp", 12, 140, canGenerate: true));
		_dataArray.Add(new MapBlockItem(103, EMapBlockType.Wild, EMapBlockSubType.TaoYuan, 384, 385, 4, 386, 1, 0, 1, 1, 1, freeStepIgnorePathCost: true, showTips: true, new string[3] { "Tao_1_1", "Tao_2_1", "Tao_3_1" }, new string[1] { "" }, null, ignoreDestroyed: false, taiwuEventChangedBlock: false, -1, new List<sbyte> { 4 }, new short[6] { -14, -14, -14, -14, 100, -14 }, 17, 300, new int[0], null, new string[1] { "Ambience_map_3" }, -1, -1, -1, -1, null, new List<short>(), new List<short>(), -1, new List<(short, short)> { (1, 100) }, "tex_mapblockevent_wild_taoyuan", 2, 141, canGenerate: true));
		_dataArray.Add(new MapBlockItem(104, EMapBlockType.Wild, EMapBlockSubType.TaoYuan, 384, 387, 16, 386, 1, 0, 1, 1, 1, freeStepIgnorePathCost: true, showTips: true, new string[3] { "Tao_1_2", "Tao_2_2", "Tao_3_2" }, new string[1] { "" }, null, ignoreDestroyed: false, taiwuEventChangedBlock: false, -1, new List<sbyte> { 4 }, new short[6] { -14, -14, -14, -14, 100, -14 }, 17, 300, new int[0], null, new string[1] { "Ambience_map_3" }, -1, -1, -1, -1, null, new List<short>(), new List<short>(), -1, new List<(short, short)> { (1, 100) }, "tex_mapblockevent_wild_taoyuan", 2, 141, canGenerate: true));
		_dataArray.Add(new MapBlockItem(105, EMapBlockType.Wild, EMapBlockSubType.TaoYuan, 384, 388, 1, 386, 1, 0, 1, 1, 1, freeStepIgnorePathCost: true, showTips: true, new string[3] { "Tao_1_3", "Tao_2_3", "Tao_3_3" }, new string[1] { "" }, null, ignoreDestroyed: false, taiwuEventChangedBlock: false, -1, new List<sbyte> { 4 }, new short[6] { -14, -14, -14, -14, 100, -14 }, 17, 300, new int[0], null, new string[1] { "Ambience_map_3" }, -1, -1, -1, -1, null, new List<short>(), new List<short>(), -1, new List<(short, short)> { (1, 100) }, "tex_mapblockevent_wild_taoyuan", 2, 141, canGenerate: true));
		_dataArray.Add(new MapBlockItem(106, EMapBlockType.Wild, EMapBlockSubType.Valley, 389, 390, 4, 391, 1, 0, 1, 2, 2, freeStepIgnorePathCost: true, showTips: true, new string[3] { "Stream_1_1", "Stream_2_1", "Stream_3_1" }, new string[1] { "" }, null, ignoreDestroyed: false, taiwuEventChangedBlock: false, -1, new List<sbyte> { 3 }, new short[6] { -14, -14, -14, 100, -14, -14 }, 16, 300, new int[0], null, new string[1] { "Ambience_map_8" }, -1, -1, -1, -1, null, new List<short>(), new List<short>(), -1, new List<(short, short)> { (1, 100) }, "tex_mapblockevent_wild_valley", 36, 136, canGenerate: true));
		_dataArray.Add(new MapBlockItem(107, EMapBlockType.Wild, EMapBlockSubType.Valley, 389, 392, 16, 391, 1, 0, 1, 2, 2, freeStepIgnorePathCost: true, showTips: true, new string[3] { "Stream_1_2", "Stream_2_2", "Stream_3_2" }, new string[1] { "" }, null, ignoreDestroyed: false, taiwuEventChangedBlock: false, -1, new List<sbyte> { 3 }, new short[6] { -14, -14, -14, 100, -14, -14 }, 16, 300, new int[0], null, new string[1] { "Ambience_map_8" }, -1, -1, -1, -1, null, new List<short>(), new List<short>(), -1, new List<(short, short)> { (1, 100) }, "tex_mapblockevent_wild_valley", 36, 136, canGenerate: true));
		_dataArray.Add(new MapBlockItem(108, EMapBlockType.Wild, EMapBlockSubType.Valley, 389, 393, 1, 391, 1, 0, 1, 2, 2, freeStepIgnorePathCost: true, showTips: true, new string[3] { "Stream_1_3", "Stream_2_3", "Stream_3_3" }, new string[1] { "" }, null, ignoreDestroyed: false, taiwuEventChangedBlock: false, -1, new List<sbyte> { 3 }, new short[6] { -14, -14, -14, 100, -14, -14 }, 16, 300, new int[0], null, new string[1] { "Ambience_map_8" }, -1, -1, -1, -1, null, new List<short>(), new List<short>(), -1, new List<(short, short)> { (1, 100) }, "tex_mapblockevent_wild_valley", 36, 136, canGenerate: true));
		_dataArray.Add(new MapBlockItem(109, EMapBlockType.Bad, EMapBlockSubType.Wild, 394, 395, 4, 396, 1, 0, 1, 1, 1, freeStepIgnorePathCost: true, showTips: true, new string[1] { "Wildland_1_1" }, new string[1] { "" }, null, ignoreDestroyed: false, taiwuEventChangedBlock: false, -1, new List<sbyte>(), new short[6], -1, 300, new int[0], null, new string[1] { "Ambience_map_10" }, -1, -1, -1, -1, null, new List<short>(), new List<short>(), -1, new List<(short, short)> { (1, 100) }, "tex_mapblockevent_bad_wild_0", 0, -1, canGenerate: true));
		_dataArray.Add(new MapBlockItem(110, EMapBlockType.Bad, EMapBlockSubType.Wild, 394, 397, 16, 396, 1, 0, 1, 1, 1, freeStepIgnorePathCost: true, showTips: true, new string[1] { "Wildland_1_2" }, new string[1] { "" }, null, ignoreDestroyed: false, taiwuEventChangedBlock: false, -1, new List<sbyte>(), new short[6], -1, 300, new int[0], null, new string[1] { "Ambience_map_10" }, -1, -1, -1, -1, null, new List<short>(), new List<short>(), -1, new List<(short, short)> { (1, 100) }, "tex_mapblockevent_bad_wild_0", 0, -1, canGenerate: true));
		_dataArray.Add(new MapBlockItem(111, EMapBlockType.Bad, EMapBlockSubType.Wild, 394, 398, 4, 396, 1, 0, 1, 1, 1, freeStepIgnorePathCost: true, showTips: true, new string[1] { "Wildland_1_3" }, new string[1] { "" }, null, ignoreDestroyed: false, taiwuEventChangedBlock: false, -1, new List<sbyte>(), new short[6], -1, 300, new int[0], null, new string[1] { "Ambience_map_10" }, -1, -1, -1, -1, null, new List<short>(), new List<short>(), -1, new List<(short, short)> { (1, 100) }, "tex_mapblockevent_bad_wild_0", 0, -1, canGenerate: true));
		_dataArray.Add(new MapBlockItem(112, EMapBlockType.Bad, EMapBlockSubType.Wild, 394, 399, 16, 396, 1, 0, 1, 1, 1, freeStepIgnorePathCost: true, showTips: true, new string[1] { "Wildland_2_1" }, new string[1] { "" }, null, ignoreDestroyed: false, taiwuEventChangedBlock: false, -1, new List<sbyte>(), new short[6], -1, 300, new int[0], null, new string[1] { "Ambience_map_10" }, -1, -1, -1, -1, null, new List<short>(), new List<short>(), -1, new List<(short, short)> { (1, 100) }, "tex_mapblockevent_bad_wild_1", 0, -1, canGenerate: true));
		_dataArray.Add(new MapBlockItem(113, EMapBlockType.Bad, EMapBlockSubType.Wild, 394, 400, 1, 396, 1, 0, 1, 1, 1, freeStepIgnorePathCost: true, showTips: true, new string[1] { "Wildland_2_2" }, new string[1] { "" }, null, ignoreDestroyed: false, taiwuEventChangedBlock: false, -1, new List<sbyte>(), new short[6], -1, 300, new int[0], null, new string[1] { "Ambience_map_10" }, -1, -1, -1, -1, null, new List<short>(), new List<short>(), -1, new List<(short, short)> { (1, 100) }, "tex_mapblockevent_bad_wild_1", 0, -1, canGenerate: true));
		_dataArray.Add(new MapBlockItem(114, EMapBlockType.Bad, EMapBlockSubType.Wild, 394, 401, 4, 396, 1, 0, 1, 1, 1, freeStepIgnorePathCost: true, showTips: true, new string[1] { "Wildland_2_3" }, new string[1] { "" }, null, ignoreDestroyed: false, taiwuEventChangedBlock: false, -1, new List<sbyte>(), new short[6], -1, 300, new int[0], null, new string[1] { "Ambience_map_10" }, -1, -1, -1, -1, null, new List<short>(), new List<short>(), -1, new List<(short, short)> { (1, 100) }, "tex_mapblockevent_bad_wild_1", 0, -1, canGenerate: true));
		_dataArray.Add(new MapBlockItem(115, EMapBlockType.Bad, EMapBlockSubType.Wild, 394, 402, 16, 396, 1, 0, 1, 1, 1, freeStepIgnorePathCost: true, showTips: true, new string[1] { "Wildland_3_1" }, new string[1] { "" }, null, ignoreDestroyed: false, taiwuEventChangedBlock: false, -1, new List<sbyte>(), new short[6], -1, 300, new int[0], null, new string[1] { "Ambience_map_10" }, -1, -1, -1, -1, null, new List<short>(), new List<short>(), -1, new List<(short, short)> { (1, 100) }, "tex_mapblockevent_bad_wild_2", 0, -1, canGenerate: true));
		_dataArray.Add(new MapBlockItem(116, EMapBlockType.Bad, EMapBlockSubType.Wild, 394, 403, 4, 396, 1, 0, 1, 1, 1, freeStepIgnorePathCost: true, showTips: true, new string[1] { "Wildland_3_2" }, new string[1] { "" }, null, ignoreDestroyed: false, taiwuEventChangedBlock: false, -1, new List<sbyte>(), new short[6], -1, 300, new int[0], null, new string[1] { "Ambience_map_10" }, -1, -1, -1, -1, null, new List<short>(), new List<short>(), -1, new List<(short, short)> { (1, 100) }, "tex_mapblockevent_bad_wild_2", 0, -1, canGenerate: true));
		_dataArray.Add(new MapBlockItem(117, EMapBlockType.Bad, EMapBlockSubType.Wild, 394, 404, 16, 396, 1, 0, 1, 1, 1, freeStepIgnorePathCost: true, showTips: true, new string[1] { "Wildland_3_3" }, new string[1] { "" }, null, ignoreDestroyed: false, taiwuEventChangedBlock: false, -1, new List<sbyte>(), new short[6], -1, 300, new int[0], null, new string[1] { "Ambience_map_10" }, -1, -1, -1, -1, null, new List<short>(), new List<short>(), -1, new List<(short, short)> { (1, 100) }, "tex_mapblockevent_bad_wild_2", 0, -1, canGenerate: true));
		_dataArray.Add(new MapBlockItem(118, EMapBlockType.Bad, EMapBlockSubType.Ruin, 405, 406, 1, 407, 1, 0, 1, 2, 90, freeStepIgnorePathCost: true, showTips: true, new string[1] { "Ruinland_1" }, new string[1] { "" }, null, ignoreDestroyed: false, taiwuEventChangedBlock: false, -1, new List<sbyte>(), new short[6], -1, 300, new int[0], null, new string[1] { "Ambience_map_4" }, -1, -1, -1, -1, null, new List<short>(), new List<short>(), -1, new List<(short, short)> { (1, 100) }, "tex_mapblockevent_bad_ruin_0", 32, -1, canGenerate: true));
		_dataArray.Add(new MapBlockItem(119, EMapBlockType.Bad, EMapBlockSubType.Ruin, 405, 408, 4, 407, 1, 0, 1, 2, 90, freeStepIgnorePathCost: true, showTips: true, new string[1] { "Ruinland_2" }, new string[1] { "" }, null, ignoreDestroyed: false, taiwuEventChangedBlock: false, -1, new List<sbyte>(), new short[6], -1, 300, new int[0], null, new string[1] { "Ambience_map_4" }, -1, -1, -1, -1, null, new List<short>(), new List<short>(), -1, new List<(short, short)> { (1, 100) }, "tex_mapblockevent_bad_ruin_0", 32, -1, canGenerate: true));
	}

	private void CreateItems2()
	{
		_dataArray.Add(new MapBlockItem(120, EMapBlockType.Bad, EMapBlockSubType.Ruin, 405, 409, 16, 407, 1, 0, 1, 2, 90, freeStepIgnorePathCost: true, showTips: true, new string[1] { "Ruinland_3" }, new string[1] { "" }, null, ignoreDestroyed: false, taiwuEventChangedBlock: false, -1, new List<sbyte>(), new short[6], -1, 300, new int[0], null, new string[1] { "Ambience_map_4" }, -1, -1, -1, -1, null, new List<short>(), new List<short>(), -1, new List<(short, short)> { (1, 100) }, "tex_mapblockevent_bad_ruin_0", 32, -1, canGenerate: true));
		_dataArray.Add(new MapBlockItem(121, EMapBlockType.Bad, EMapBlockSubType.Ruin, 405, 410, 4, 407, 1, 0, 1, 2, 90, freeStepIgnorePathCost: true, showTips: true, new string[1] { "Ruinland_4" }, new string[1] { "" }, null, ignoreDestroyed: false, taiwuEventChangedBlock: false, -1, new List<sbyte>(), new short[6], -1, 300, new int[0], null, new string[1] { "Ambience_map_4" }, -1, -1, -1, -1, null, new List<short>(), new List<short>(), -1, new List<(short, short)> { (1, 100) }, "tex_mapblockevent_bad_ruin_1", 32, -1, canGenerate: true));
		_dataArray.Add(new MapBlockItem(122, EMapBlockType.Bad, EMapBlockSubType.Ruin, 405, 411, 16, 407, 1, 0, 1, 2, 90, freeStepIgnorePathCost: true, showTips: true, new string[1] { "Ruinland_5" }, new string[1] { "" }, null, ignoreDestroyed: false, taiwuEventChangedBlock: false, -1, new List<sbyte>(), new short[6], -1, 300, new int[0], null, new string[1] { "Ambience_map_4" }, -1, -1, -1, -1, null, new List<short>(), new List<short>(), -1, new List<(short, short)> { (1, 100) }, "tex_mapblockevent_bad_ruin_1", 32, -1, canGenerate: true));
		_dataArray.Add(new MapBlockItem(123, EMapBlockType.Bad, EMapBlockSubType.Ruin, 405, 412, 1, 407, 1, 0, 1, 2, 90, freeStepIgnorePathCost: true, showTips: true, new string[1] { "Ruinland_6" }, new string[1] { "" }, null, ignoreDestroyed: false, taiwuEventChangedBlock: false, -1, new List<sbyte>(), new short[6], -1, 300, new int[0], null, new string[1] { "Ambience_map_4" }, -1, -1, -1, -1, null, new List<short>(), new List<short>(), -1, new List<(short, short)> { (1, 100) }, "tex_mapblockevent_bad_ruin_1", 32, -1, canGenerate: true));
		_dataArray.Add(new MapBlockItem(124, EMapBlockType.Bad, EMapBlockSubType.DarkPool, 413, 413, 4, 414, 1, 0, 1, 4, 99, freeStepIgnorePathCost: false, showTips: true, new string[1] { "Darkland_1_1" }, new string[1] { "" }, null, ignoreDestroyed: false, taiwuEventChangedBlock: false, -1, new List<sbyte>(), new short[6], -1, -1, new int[0], null, new string[1] { "Ambience_map_4" }, -1, -1, -1, -1, null, new List<short>(), new List<short>(), -1, new List<(short, short)> { (1, 100) }, "tex_mapblockevent_bad_darkpool", 14, -1, canGenerate: true));
		_dataArray.Add(new MapBlockItem(125, EMapBlockType.Invalid, EMapBlockSubType.Block, 415, 416, 16, 417, 1, 0, 1, -1, -1, freeStepIgnorePathCost: false, showTips: false, new string[1] { "sp_maptop_5011" }, new string[1] { "" }, null, ignoreDestroyed: false, taiwuEventChangedBlock: false, -1, new List<sbyte>(), new short[6], -1, -1, new int[0], null, new string[1] { "" }, -1, -1, -1, -1, null, new List<short>(), new List<short>(), -1, new List<(short, short)> { (1, 100) }, "tex_mapblockevent_wild_valley", 0, -1, canGenerate: true));
		_dataArray.Add(new MapBlockItem(126, EMapBlockType.Invalid, EMapBlockSubType.None, 418, 419, 1, 420, 1, 0, 1, -1, -1, freeStepIgnorePathCost: false, showTips: false, new string[1] { "" }, new string[1] { "" }, null, ignoreDestroyed: false, taiwuEventChangedBlock: false, -1, new List<sbyte>(), new short[6], -1, -1, new int[0], null, new string[1] { "" }, -1, -1, -1, -1, null, new List<short>(), new List<short>(), -1, new List<(short, short)> { (1, 100) }, "tex_mapblockevent_wild_valley", 0, -1, canGenerate: true));
		_dataArray.Add(new MapBlockItem(127, EMapBlockType.scenery, EMapBlockSubType.Scenery_1, 421, 422, 4, 423, 2, 0, 2, 1, 1, freeStepIgnorePathCost: true, showTips: true, new string[1] { "sp_maptop_scenery_400" }, new string[1] { "" }, null, ignoreDestroyed: false, taiwuEventChangedBlock: false, -1, new List<sbyte>(), new short[6] { 150, 150, 150, 150, 150, 150 }, 9, -1, new int[4] { 421, 424, 425, 426 }, null, new string[1] { "" }, -1, -1, -1, -1, null, new List<short>(), new List<short>(), -1, new List<(short, short)> { (1, 100) }, "tex_mapblockevent_normal_mountain", 0, -1, canGenerate: true));
		_dataArray.Add(new MapBlockItem(128, EMapBlockType.Bad, EMapBlockSubType.SwordTomb, 427, 428, 16, 429, 2, 0, 2, 1, 1, freeStepIgnorePathCost: true, showTips: true, new string[1] { "MapSwordTombMonvyi" }, new string[1] { "SwordTomb_monv/eff_Swordtomb_monv" }, "Villagerwork_MapSwordTombMonvyi", ignoreDestroyed: false, taiwuEventChangedBlock: false, -1, new List<sbyte>(), new short[6], -1, -1, new int[4] { 427, 427, 427, 427 }, null, new string[4] { "Ambience_map_12", "Ambience_map_12", "Ambience_map_12", "Ambience_map_12" }, -1, -1, -1, -1, null, new List<short>(), new List<short>(), -1, new List<(short, short)> { (1, 100) }, "tex_swordtomb_0", 0, -1, canGenerate: true));
		_dataArray.Add(new MapBlockItem(129, EMapBlockType.Bad, EMapBlockSubType.SwordTomb, 430, 431, 1, 432, 2, 0, 2, 1, 1, freeStepIgnorePathCost: true, showTips: true, new string[1] { "MapSwordTombFuxietie" }, new string[1] { "SwordTomb_dayue/eff_Swordtomb_dayue" }, "Villagerwork_MapSwordTombFuxietie", ignoreDestroyed: false, taiwuEventChangedBlock: false, -1, new List<sbyte>(), new short[6], -1, -1, new int[4] { 430, 430, 430, 430 }, null, new string[4] { "Ambience_map_boss_5", "Ambience_map_boss_5", "Ambience_map_boss_5", "Ambience_map_boss_5" }, -1, -1, -1, -1, null, new List<short>(), new List<short>(), -1, new List<(short, short)> { (1, 100) }, "tex_swordtomb_0", 0, -1, canGenerate: true));
		_dataArray.Add(new MapBlockItem(130, EMapBlockType.Bad, EMapBlockSubType.SwordTomb, 433, 434, 4, 435, 2, 0, 2, 1, 1, freeStepIgnorePathCost: true, showTips: true, new string[1] { "MapSwordTombDaxuanning" }, new string[1] { "SwordTomb_jiuhan/eff_Swordtomb_jiuhan" }, "Villagerwork_MapSwordTombDaxuanning", ignoreDestroyed: false, taiwuEventChangedBlock: false, -1, new List<sbyte>(), new short[6], -1, -1, new int[4] { 433, 433, 433, 433 }, null, new string[4] { "Ambience_map_15", "Ambience_map_15", "Ambience_map_15", "Ambience_map_15" }, -1, -1, -1, -1, null, new List<short>(), new List<short>(), -1, new List<(short, short)> { (1, 100) }, "tex_swordtomb_0", 0, -1, canGenerate: true));
		_dataArray.Add(new MapBlockItem(131, EMapBlockType.Bad, EMapBlockSubType.SwordTomb, 436, 437, 16, 438, 2, 0, 2, 1, 1, freeStepIgnorePathCost: true, showTips: true, new string[1] { "MapSwordTombfenghuangjian" }, new string[1] { "SwordTomb_fenghuangjian/eff_Swordtomb_fenghuangjian" }, "Villagerwork_MapSwordTombfenghuangjian", ignoreDestroyed: false, taiwuEventChangedBlock: false, -1, new List<sbyte>(), new short[6], -1, -1, new int[4] { 436, 436, 436, 436 }, null, new string[4] { "Ambience_map_boss_3", "Ambience_map_boss_3", "Ambience_map_boss_3", "Ambience_map_boss_3" }, -1, -1, -1, -1, null, new List<short>(), new List<short>(), -1, new List<(short, short)> { (1, 100) }, "tex_swordtomb_0", 0, -1, canGenerate: true));
		_dataArray.Add(new MapBlockItem(132, EMapBlockType.Bad, EMapBlockSubType.SwordTomb, 439, 440, 4, 441, 2, 0, 2, 1, 1, freeStepIgnorePathCost: true, showTips: true, new string[1] { "MapSwordTombFenshenlian" }, new string[1] { "SwordTomb_fenshenlian/eff_Swordtomb_fenshenlian" }, "Villagerwork_MapSwordTombFenshenlian", ignoreDestroyed: false, taiwuEventChangedBlock: false, -1, new List<sbyte>(), new short[6], -1, -1, new int[4] { 439, 439, 439, 439 }, null, new string[4] { "Ambience_map_boss_2", "Ambience_map_boss_2", "Ambience_map_boss_2", "Ambience_map_boss_2" }, -1, -1, -1, -1, null, new List<short>(), new List<short>(), -1, new List<(short, short)> { (1, 100) }, "tex_swordtomb_0", 0, -1, canGenerate: true));
		_dataArray.Add(new MapBlockItem(133, EMapBlockType.Bad, EMapBlockSubType.SwordTomb, 442, 443, 16, 444, 2, 0, 2, 1, 1, freeStepIgnorePathCost: true, showTips: true, new string[1] { "MapSwordTombjielongpo" }, new string[1] { "SwordTomb_xielongpo/eff_Swordtomb_xielongpo" }, "Villagerwork_MapSwordTombjielongpo", ignoreDestroyed: false, taiwuEventChangedBlock: false, -1, new List<sbyte>(), new short[6], -1, -1, new int[4] { 442, 442, 442, 442 }, null, new string[4] { "Ambience_map_10", "Ambience_map_10", "Ambience_map_10", "Ambience_map_10" }, -1, -1, -1, -1, null, new List<short>(), new List<short>(), -1, new List<(short, short)> { (1, 100) }, "tex_swordtomb_0", 0, -1, canGenerate: true));
		_dataArray.Add(new MapBlockItem(134, EMapBlockType.Bad, EMapBlockSubType.SwordTomb, 445, 446, 1, 447, 2, 0, 2, 1, 1, freeStepIgnorePathCost: true, showTips: true, new string[1] { "MapSwordTombLuochenyin" }, new string[1] { "SwordTomb_yixiang/eff_Swordtomb_yixiang" }, "Villagerwork_MapSwordTombLuochenyin", ignoreDestroyed: false, taiwuEventChangedBlock: false, -1, new List<sbyte>(), new short[6], -1, -1, new int[4] { 445, 445, 445, 445 }, null, new string[4] { "Ambience_map_boss_6", "Ambience_map_boss_6", "Ambience_map_boss_6", "Ambience_map_boss_6" }, -1, -1, -1, -1, null, new List<short>(), new List<short>(), -1, new List<(short, short)> { (1, 100) }, "tex_swordtomb_0", 0, -1, canGenerate: true));
		_dataArray.Add(new MapBlockItem(135, EMapBlockType.Bad, EMapBlockSubType.SwordTomb, 448, 449, 4, 450, 2, 0, 2, 1, 1, freeStepIgnorePathCost: true, showTips: true, new string[1] { "MapSwordTombQiumomu" }, new string[1] { "SwordTomb_xuefeng/eff_Swordtomb_xuefeng" }, "Villagerwork_MapSwordTombQiumomu", ignoreDestroyed: false, taiwuEventChangedBlock: false, -1, new List<sbyte>(), new short[6], -1, -1, new int[4] { 448, 448, 448, 448 }, null, new string[4] { "Ambience_map_boss_4", "Ambience_map_boss_4", "Ambience_map_boss_4", "Ambience_map_boss_4" }, -1, -1, -1, -1, null, new List<short>(), new List<short>(), -1, new List<(short, short)> { (1, 100) }, "tex_swordtomb_0", 0, -1, canGenerate: true));
		_dataArray.Add(new MapBlockItem(136, EMapBlockType.Bad, EMapBlockSubType.SwordTomb, 451, 452, 16, 453, 2, 0, 2, 1, 1, freeStepIgnorePathCost: true, showTips: true, new string[1] { "MapSwordTombGuishenxia" }, new string[1] { "SwordTomb_guishenxia/eff_Swordtomb_guishenxia" }, "Villagerwork_MapSwordTombGuishenxia", ignoreDestroyed: false, taiwuEventChangedBlock: false, -1, new List<sbyte>(), new short[6], -1, -1, new int[4] { 451, 451, 451, 451 }, null, new string[4] { "Ambience_map_boss_1", "Ambience_map_boss_1", "Ambience_map_boss_1", "Ambience_map_boss_1" }, -1, -1, -1, -1, null, new List<short>(), new List<short>(), -1, new List<(short, short)> { (1, 100) }, "tex_swordtomb_0", 0, -1, canGenerate: true));
		_dataArray.Add(new MapBlockItem(137, EMapBlockType.Wild, EMapBlockSubType.DLCLoong, 454, 454, 4, 455, 1, 0, 1, 5, 5, freeStepIgnorePathCost: true, showTips: true, new string[1] { "Fiveloong_0" }, new string[2] { "Justiselong_jin/eff_wulong_jin_xiao1", "Justiselong_jin/eff_wulong_jin_xiao2" }, null, ignoreDestroyed: true, taiwuEventChangedBlock: false, -1, new List<sbyte> { 2 }, new short[6] { 0, 0, 300, 90, 90, 150 }, 19, 0, new int[0], null, new string[2] { "Ambience_map_dragon_jin_big", "Ambience_map_dragon_jin_small" }, -1, -1, -1, -1, null, new List<short>(), new List<short>(), -1, new List<(short, short)> { (1, 100) }, "tex_fiveloong_4_0", 0, -1, canGenerate: false));
		_dataArray.Add(new MapBlockItem(138, EMapBlockType.Wild, EMapBlockSubType.DLCLoong, 456, 456, 16, 457, 1, 0, 1, 5, 5, freeStepIgnorePathCost: true, showTips: true, new string[1] { "Fiveloong_2" }, new string[1] { "Justiselong_shui/eff_wulong_shui_xiao1" }, "Villagerwork_Fiveloong_2", ignoreDestroyed: true, taiwuEventChangedBlock: false, -1, new List<sbyte> { 3 }, new short[6] { 0, 90, 90, 300, 0, 150 }, 20, 0, new int[0], null, new string[2] { "Ambience_map_dragon_shui_big", "Ambience_map_dragon_shui_small" }, -1, -1, -1, -1, null, new List<short>(), new List<short>(), -1, new List<(short, short)> { (1, 100) }, "tex_fiveloong_4_1", 0, -1, canGenerate: false));
		_dataArray.Add(new MapBlockItem(139, EMapBlockType.Wild, EMapBlockSubType.DLCLoong, 458, 458, 1, 459, 1, 0, 1, 5, 5, freeStepIgnorePathCost: true, showTips: true, new string[1] { "Fiveloong_4" }, new string[2] { "Justiselong_feng/eff_wulong_mu_xiao1", "Justiselong_feng/eff_wulong_mu_xiao2" }, null, ignoreDestroyed: true, taiwuEventChangedBlock: false, -1, new List<sbyte> { 1 }, new short[6] { 90, 300, 0, 90, 0, 150 }, 21, 0, new int[0], null, new string[2] { "Ambience_map_dragon_mu_big", "Ambience_map_dragon_mu_small" }, -1, -1, -1, -1, null, new List<short>(), new List<short>(), -1, new List<(short, short)> { (1, 100) }, "tex_fiveloong_4_3", 0, -1, canGenerate: false));
		_dataArray.Add(new MapBlockItem(140, EMapBlockType.Wild, EMapBlockSubType.DLCLoong, 460, 460, 4, 461, 1, 0, 1, 5, 5, freeStepIgnorePathCost: true, showTips: true, new string[1] { "Fiveloong_3" }, new string[1] { "Justiselong_huo/eff_wulong_huo_xiao1" }, null, ignoreDestroyed: true, taiwuEventChangedBlock: false, -1, new List<sbyte> { 0 }, new short[6] { 300, 90, 0, 0, 90, 150 }, 22, 0, new int[0], null, new string[2] { "Ambience_map_dragon_huo_big", "Ambience_map_dragon_huo_small" }, -1, -1, -1, -1, null, new List<short>(), new List<short>(), -1, new List<(short, short)> { (1, 100) }, "tex_fiveloong_4_2", 0, -1, canGenerate: false));
		_dataArray.Add(new MapBlockItem(141, EMapBlockType.Wild, EMapBlockSubType.DLCLoong, 462, 462, 16, 463, 1, 0, 1, 5, 5, freeStepIgnorePathCost: true, showTips: true, new string[1] { "Fiveloong_1" }, new string[1] { "Justiselong_sha/eff_wulong_tu_xiao1" }, null, ignoreDestroyed: true, taiwuEventChangedBlock: false, -1, new List<sbyte> { 4 }, new short[6] { 90, 0, 90, 0, 300, 150 }, 23, 0, new int[0], null, new string[2] { "Ambience_map_dragon_tu_big", "Ambience_map_dragon_tu_small" }, -1, -1, -1, -1, null, new List<short>(), new List<short>(), -1, new List<(short, short)> { (1, 100) }, "tex_fiveloong_4_4", 0, -1, canGenerate: false));
	}

	public override void Init()
	{
		base.Init();
		_dataArray = new List<MapBlockItem>(142);
		CreateItems0();
		CreateItems1();
		CreateItems2();
	}
}
