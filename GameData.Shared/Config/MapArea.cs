using System;
using System.Collections.Generic;
using Config.Common;
using Config.ConfigCells;
using GameData.Utilities;

namespace Config;

[Serializable]
public class MapArea : ConfigData<MapAreaItem, short>
{
	public static class DefKey
	{
		public const short Born = 0;

		public const short MainCityBegin = 1;

		public const short Chengdu = 2;

		public const short Guizhou = 3;

		public const short Xiangyang = 4;

		public const short taiyuan = 5;

		public const short Guangzhou = 6;

		public const short qingzhou = 7;

		public const short Jiangling = 8;

		public const short fuzhou = 9;

		public const short liaoyang = 10;

		public const short qinzhou = 11;

		public const short dali = 12;

		public const short shouchun = 13;

		public const short hangzhou = 14;

		public const short MainCityEnd = 15;

		public const short WhiteDeerLake = 18;

		public const short LotusMountain = 21;

		public const short XuannvPeak = 23;

		public const short KongsangMountain = 25;

		public const short KunlunMountain = 26;

		public const short Blackwater = 27;

		public const short Chimingdao = 29;

		public const short ThirdAreaBegin = 31;

		public const short Lishui = 47;

		public const short MaoerMountain = 49;

		public const short Nanhai = 66;

		public const short Huizhou = 67;

		public const short LuofuMountain = 68;

		public const short MeizhouCity = 69;

		public const short Chaozhou = 70;

		public const short Nanling = 71;

		public const short WhiteCloudMountain = 72;

		public const short Xiangshui = 83;

		public const short DongtingLake = 84;

		public const short Guide = 136;

		public const short SecretVilliage = 137;
	}

	public static class DefValue
	{
		public static MapAreaItem Born => Instance[(short)0];

		public static MapAreaItem MainCityBegin => Instance[(short)1];

		public static MapAreaItem Chengdu => Instance[(short)2];

		public static MapAreaItem Guizhou => Instance[(short)3];

		public static MapAreaItem Xiangyang => Instance[(short)4];

		public static MapAreaItem taiyuan => Instance[(short)5];

		public static MapAreaItem Guangzhou => Instance[(short)6];

		public static MapAreaItem qingzhou => Instance[(short)7];

		public static MapAreaItem Jiangling => Instance[(short)8];

		public static MapAreaItem fuzhou => Instance[(short)9];

		public static MapAreaItem liaoyang => Instance[(short)10];

		public static MapAreaItem qinzhou => Instance[(short)11];

		public static MapAreaItem dali => Instance[(short)12];

		public static MapAreaItem shouchun => Instance[(short)13];

		public static MapAreaItem hangzhou => Instance[(short)14];

		public static MapAreaItem MainCityEnd => Instance[(short)15];

		public static MapAreaItem WhiteDeerLake => Instance[(short)18];

		public static MapAreaItem LotusMountain => Instance[(short)21];

		public static MapAreaItem XuannvPeak => Instance[(short)23];

		public static MapAreaItem KongsangMountain => Instance[(short)25];

		public static MapAreaItem KunlunMountain => Instance[(short)26];

		public static MapAreaItem Blackwater => Instance[(short)27];

		public static MapAreaItem Chimingdao => Instance[(short)29];

		public static MapAreaItem ThirdAreaBegin => Instance[(short)31];

		public static MapAreaItem Lishui => Instance[(short)47];

		public static MapAreaItem MaoerMountain => Instance[(short)49];

		public static MapAreaItem Nanhai => Instance[(short)66];

		public static MapAreaItem Huizhou => Instance[(short)67];

		public static MapAreaItem LuofuMountain => Instance[(short)68];

		public static MapAreaItem MeizhouCity => Instance[(short)69];

		public static MapAreaItem Chaozhou => Instance[(short)70];

		public static MapAreaItem Nanling => Instance[(short)71];

		public static MapAreaItem WhiteCloudMountain => Instance[(short)72];

		public static MapAreaItem Xiangshui => Instance[(short)83];

		public static MapAreaItem DongtingLake => Instance[(short)84];

		public static MapAreaItem Guide => Instance[(short)136];

		public static MapAreaItem SecretVilliage => Instance[(short)137];
	}

	public static MapArea Instance = new MapArea();

	private readonly HashSet<string> RequiredFields = new HashSet<string>
	{
		"StateID", "NeighborAreas", "EnemyNests", "SettlementBlockCore", "OrganizationId", "CenterBlock", "DevelopedBlockCore", "NormalBlockCore", "WildBlockCore", "BigBaseBlockCore",
		"SeriesBlockCore", "EncircleBlockCore", "SceneryBlockCore", "TemplateId", "Name", "Desc", "BigMapIcon", "BlockAtlas", "CustomBlockConfig", "TempleName",
		"TempleDesc", "CaveName", "CaveDesc"
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
		List<MapAreaItem> dataArray = _dataArray;
		sbyte[] worldMapPos = new sbyte[2] { -1, -1 };
		AreaTravelRoute[] neighborAreas = new AreaTravelRoute[0];
		string[] miniMapIcons = new string[4] { "largemap_part_2_icon_hei_2", "largemap_part_2_icon_jin_2", "largemap_part_2_icon_hei_2", "largemap_part_2_icon_hui_2" };
		short[] miniMapPos = new short[2] { 180, 180 };
		List<EnemyNestCreationInfo[]> list = new List<EnemyNestCreationInfo[]>();
		List<EnemyNestCreationInfo[]> enemyNests = list;
		List<IntPair> list2 = new List<IntPair>();
		List<IntPair> enemyNestCreationDateRanges = list2;
		short[] settlementBlockCore = new short[1] { 17 };
		sbyte[] organizationId = new sbyte[1];
		List<short[]> developedBlockCore = new List<short[]>
		{
			new short[2] { 39, 33 },
			new short[2] { 40, 33 },
			new short[2] { 41, 33 },
			new short[2] { 42, 33 },
			new short[2] { 43, 33 },
			new short[2] { 44, 33 },
			new short[2] { 45, 33 },
			new short[2] { 46, 33 },
			new short[2] { 47, 33 },
			new short[2] { 48, 33 },
			new short[2] { 49, 33 },
			new short[2] { 50, 33 },
			new short[2] { 51, 33 },
			new short[2] { 52, 33 },
			new short[2] { 53, 33 },
			new short[2] { 54, 33 },
			new short[2] { 55, 33 },
			new short[2] { 56, 33 },
			new short[2] { 109, 10 },
			new short[2] { 110, 10 },
			new short[2] { 111, 10 }
		};
		List<short[]> normalBlockCore = new List<short[]>
		{
			new short[2] { 57, 33 },
			new short[2] { 58, 33 },
			new short[2] { 59, 33 },
			new short[2] { 63, 33 },
			new short[2] { 64, 33 },
			new short[2] { 65, 33 },
			new short[2] { 69, 33 },
			new short[2] { 70, 33 },
			new short[2] { 71, 33 },
			new short[2] { 75, 33 },
			new short[2] { 76, 33 },
			new short[2] { 77, 33 },
			new short[2] { 81, 33 },
			new short[2] { 82, 33 },
			new short[2] { 83, 33 },
			new short[2] { 87, 33 },
			new short[2] { 88, 33 },
			new short[2] { 89, 33 },
			new short[2] { 112, 30 },
			new short[2] { 113, 30 },
			new short[2] { 114, 30 },
			new short[2] { 124, 30 }
		};
		List<short[]> wildBlockCore = new List<short[]>
		{
			new short[2] { 93, 33 },
			new short[2] { 94, 33 },
			new short[2] { 95, 33 },
			new short[2] { 96, 33 },
			new short[2] { 97, 33 },
			new short[2] { 98, 33 },
			new short[2] { 99, 33 },
			new short[2] { 100, 33 },
			new short[2] { 101, 33 },
			new short[2] { 102, 33 },
			new short[2] { 103, 33 },
			new short[2] { 104, 33 },
			new short[2] { 105, 33 },
			new short[2] { 106, 33 },
			new short[2] { 107, 33 },
			new short[2] { 108, 33 },
			new short[2] { 115, 40 },
			new short[2] { 116, 40 },
			new short[2] { 117, 40 },
			new short[2] { 124, 30 }
		};
		List<short[]> bigBaseBlockCore = new List<short[]>
		{
			new short[2] { 60, 1 },
			new short[2] { 78, 1 },
			new short[2] { 61, 1 },
			new short[2] { 79, 1 },
			new short[2] { 62, 1 },
			new short[2] { 80, 1 }
		};
		List<short[]> seriesBlockCore = new List<short[]> { new short[4] { 45, 1, 8, 16 } };
		List<short[]> list3 = new List<short[]>();
		List<short[]> encircleBlockCore = list3;
		short[] sceneryBlockCore = new short[0];
		List<short> list4 = new List<short>();
		List<short> lovingItemSubTypes = list4;
		list4 = new List<short>();
		dataArray.Add(new MapAreaItem(0, 0, 0, 1, 20, worldMapPos, neighborAreas, "largemap_part_1_changgui_0", miniMapIcons, null, miniMapPos, 0, 0, 5, enemyNests, enemyNestCreationDateRanges, settlementBlockCore, organizationId, 17, -1, null, developedBlockCore, normalBlockCore, wildBlockCore, bigBaseBlockCore, seriesBlockCore, encircleBlockCore, sceneryBlockCore, 2, 3, null, null, EMapAreaAreaDirection.South, lovingItemSubTypes, list4, showDarkAshStatus: false));
		List<MapAreaItem> dataArray2 = _dataArray;
		sbyte[] worldMapPos2 = new sbyte[2] { 15, 16 };
		AreaTravelRoute[] neighborAreas2 = new AreaTravelRoute[5]
		{
			new AreaTravelRoute(32, 4, null),
			new AreaTravelRoute(34, 12, new int[2] { 14, 16 }),
			new AreaTravelRoute(37, 10, new int[2] { 15, 15 }),
			new AreaTravelRoute(117, 30, new int[6] { 15, 15, 16, 15, 16, 14 }),
			new AreaTravelRoute(132, 30, new int[6] { 15, 15, 16, 15, 17, 15 })
		};
		string[] miniMapIcons2 = new string[4] { "largemap_part_2_icon_hei_5", "largemap_part_2_icon_jin_5", "largemap_part_2_icon_lan_5", "largemap_part_2_icon_hui_5" };
		short[] miniMapPos2 = new short[2] { 210, 120 };
		List<EnemyNestCreationInfo[]> enemyNests2 = new List<EnemyNestCreationInfo[]>
		{
			new EnemyNestCreationInfo[2]
			{
				new EnemyNestCreationInfo(4, 4),
				new EnemyNestCreationInfo(5, 4)
			},
			new EnemyNestCreationInfo[2]
			{
				new EnemyNestCreationInfo(6, 5),
				new EnemyNestCreationInfo(7, 5)
			},
			new EnemyNestCreationInfo[2]
			{
				new EnemyNestCreationInfo(8, 12),
				new EnemyNestCreationInfo(9, 12)
			},
			new EnemyNestCreationInfo[3]
			{
				new EnemyNestCreationInfo(10, 7),
				new EnemyNestCreationInfo(10, 7),
				new EnemyNestCreationInfo(11, 7)
			},
			new EnemyNestCreationInfo[1]
			{
				new EnemyNestCreationInfo(14, 4)
			}
		};
		List<IntPair> enemyNestCreationDateRanges2 = new List<IntPair>
		{
			new IntPair(0, 12),
			new IntPair(12, 24),
			new IntPair(24, 36),
			new IntPair(36, 48),
			new IntPair(0, 0)
		};
		short[] settlementBlockCore2 = new short[3] { 1, 35, 36 };
		sbyte[] organizationId2 = new sbyte[3] { 21, 37, 38 };
		List<short[]> developedBlockCore2 = new List<short[]>
		{
			new short[2] { 39, 33 },
			new short[2] { 40, 33 },
			new short[2] { 41, 33 },
			new short[2] { 42, 33 },
			new short[2] { 43, 33 },
			new short[2] { 44, 33 },
			new short[2] { 45, 33 },
			new short[2] { 46, 33 },
			new short[2] { 47, 33 },
			new short[2] { 48, 33 },
			new short[2] { 49, 33 },
			new short[2] { 50, 33 },
			new short[2] { 51, 33 },
			new short[2] { 52, 33 },
			new short[2] { 53, 33 },
			new short[2] { 54, 33 },
			new short[2] { 55, 33 },
			new short[2] { 56, 33 },
			new short[2] { 109, 10 },
			new short[2] { 110, 10 },
			new short[2] { 111, 10 }
		};
		List<short[]> normalBlockCore2 = new List<short[]>
		{
			new short[2] { 57, 33 },
			new short[2] { 58, 33 },
			new short[2] { 59, 33 },
			new short[2] { 63, 33 },
			new short[2] { 64, 33 },
			new short[2] { 65, 33 },
			new short[2] { 69, 33 },
			new short[2] { 70, 33 },
			new short[2] { 71, 33 },
			new short[2] { 75, 33 },
			new short[2] { 76, 33 },
			new short[2] { 77, 33 },
			new short[2] { 81, 33 },
			new short[2] { 82, 33 },
			new short[2] { 83, 33 },
			new short[2] { 87, 33 },
			new short[2] { 88, 33 },
			new short[2] { 89, 33 },
			new short[2] { 112, 30 },
			new short[2] { 113, 30 },
			new short[2] { 114, 30 },
			new short[2] { 124, 30 }
		};
		List<short[]> wildBlockCore2 = new List<short[]>
		{
			new short[2] { 93, 33 },
			new short[2] { 94, 33 },
			new short[2] { 95, 33 },
			new short[2] { 96, 33 },
			new short[2] { 97, 33 },
			new short[2] { 98, 33 },
			new short[2] { 99, 33 },
			new short[2] { 100, 33 },
			new short[2] { 101, 33 },
			new short[2] { 102, 33 },
			new short[2] { 103, 33 },
			new short[2] { 104, 33 },
			new short[2] { 105, 33 },
			new short[2] { 106, 33 },
			new short[2] { 107, 33 },
			new short[2] { 108, 33 },
			new short[2] { 115, 40 },
			new short[2] { 116, 40 },
			new short[2] { 117, 40 },
			new short[2] { 124, 30 }
		};
		List<short[]> bigBaseBlockCore2 = new List<short[]>
		{
			new short[2] { 72, 1 },
			new short[2] { 73, 1 },
			new short[2] { 60, 1 },
			new short[2] { 61, 1 },
			new short[2] { 62, 1 },
			new short[2] { 78, 1 },
			new short[2] { 79, 1 },
			new short[2] { 84, 1 },
			new short[2] { 66, 1 },
			new short[2] { 90, 1 }
		};
		list3 = new List<short[]>();
		List<short[]> seriesBlockCore2 = list3;
		list3 = new List<short[]>();
		List<short[]> encircleBlockCore2 = list3;
		short[] sceneryBlockCore2 = new short[0];
		int[] caveName = new int[1] { 8 };
		int[] caveDesc = new int[1] { 9 };
		list4 = new List<short>();
		List<short> lovingItemSubTypes2 = list4;
		list4 = new List<short>();
		dataArray2.Add(new MapAreaItem(1, 1, 4, 5, 30, worldMapPos2, neighborAreas2, "largemap_part_1_chengzhen_8", miniMapIcons2, "MapCityJingcheng", miniMapPos2, 0, 2, 9, enemyNests2, enemyNestCreationDateRanges2, settlementBlockCore2, organizationId2, -1, 0, null, developedBlockCore2, normalBlockCore2, wildBlockCore2, bigBaseBlockCore2, seriesBlockCore2, encircleBlockCore2, sceneryBlockCore2, 6, 7, caveName, caveDesc, EMapAreaAreaDirection.South, lovingItemSubTypes2, list4, showDarkAshStatus: true));
		List<MapAreaItem> dataArray3 = _dataArray;
		sbyte[] worldMapPos3 = new sbyte[2] { 4, 10 };
		AreaTravelRoute[] neighborAreas3 = new AreaTravelRoute[4]
		{
			new AreaTravelRoute(38, 4, null),
			new AreaTravelRoute(40, 4, null),
			new AreaTravelRoute(41, 4, null),
			new AreaTravelRoute(44, 4, null)
		};
		string[] miniMapIcons3 = new string[4] { "largemap_part_2_icon_hei_12", "largemap_part_2_icon_jin_12", "largemap_part_2_icon_lan_12", "largemap_part_2_icon_hui_12" };
		short[] miniMapPos3 = new short[2] { 130, 210 };
		List<EnemyNestCreationInfo[]> enemyNests3 = new List<EnemyNestCreationInfo[]>
		{
			new EnemyNestCreationInfo[2]
			{
				new EnemyNestCreationInfo(4, 4),
				new EnemyNestCreationInfo(5, 4)
			},
			new EnemyNestCreationInfo[2]
			{
				new EnemyNestCreationInfo(6, 5),
				new EnemyNestCreationInfo(7, 5)
			},
			new EnemyNestCreationInfo[2]
			{
				new EnemyNestCreationInfo(8, 12),
				new EnemyNestCreationInfo(9, 12)
			},
			new EnemyNestCreationInfo[3]
			{
				new EnemyNestCreationInfo(10, 7),
				new EnemyNestCreationInfo(10, 7),
				new EnemyNestCreationInfo(11, 7)
			},
			new EnemyNestCreationInfo[1]
			{
				new EnemyNestCreationInfo(14, 4)
			}
		};
		List<IntPair> enemyNestCreationDateRanges3 = new List<IntPair>
		{
			new IntPair(0, 12),
			new IntPair(12, 24),
			new IntPair(24, 36),
			new IntPair(36, 48),
			new IntPair(0, 0)
		};
		short[] settlementBlockCore3 = new short[3] { 2, 35, 36 };
		sbyte[] organizationId3 = new sbyte[3] { 22, 37, 38 };
		List<short[]> developedBlockCore3 = new List<short[]>
		{
			new short[2] { 39, 33 },
			new short[2] { 40, 33 },
			new short[2] { 41, 33 },
			new short[2] { 42, 33 },
			new short[2] { 43, 33 },
			new short[2] { 44, 33 },
			new short[2] { 45, 33 },
			new short[2] { 46, 33 },
			new short[2] { 47, 33 },
			new short[2] { 48, 33 },
			new short[2] { 49, 33 },
			new short[2] { 50, 33 },
			new short[2] { 51, 33 },
			new short[2] { 52, 33 },
			new short[2] { 53, 33 },
			new short[2] { 54, 33 },
			new short[2] { 55, 33 },
			new short[2] { 56, 33 },
			new short[2] { 109, 10 },
			new short[2] { 110, 10 },
			new short[2] { 111, 10 }
		};
		List<short[]> normalBlockCore3 = new List<short[]>
		{
			new short[2] { 57, 33 },
			new short[2] { 58, 33 },
			new short[2] { 59, 33 },
			new short[2] { 63, 33 },
			new short[2] { 64, 33 },
			new short[2] { 65, 33 },
			new short[2] { 69, 33 },
			new short[2] { 70, 33 },
			new short[2] { 71, 33 },
			new short[2] { 75, 33 },
			new short[2] { 76, 33 },
			new short[2] { 77, 33 },
			new short[2] { 81, 33 },
			new short[2] { 82, 33 },
			new short[2] { 83, 33 },
			new short[2] { 87, 33 },
			new short[2] { 88, 33 },
			new short[2] { 89, 33 },
			new short[2] { 112, 30 },
			new short[2] { 113, 30 },
			new short[2] { 114, 30 },
			new short[2] { 124, 30 }
		};
		List<short[]> wildBlockCore3 = new List<short[]>
		{
			new short[2] { 93, 33 },
			new short[2] { 94, 33 },
			new short[2] { 95, 33 },
			new short[2] { 96, 33 },
			new short[2] { 97, 33 },
			new short[2] { 98, 33 },
			new short[2] { 99, 33 },
			new short[2] { 100, 33 },
			new short[2] { 101, 33 },
			new short[2] { 102, 33 },
			new short[2] { 103, 33 },
			new short[2] { 104, 33 },
			new short[2] { 105, 33 },
			new short[2] { 106, 33 },
			new short[2] { 107, 33 },
			new short[2] { 108, 33 },
			new short[2] { 115, 40 },
			new short[2] { 116, 40 },
			new short[2] { 117, 40 },
			new short[2] { 124, 30 }
		};
		List<short[]> bigBaseBlockCore3 = new List<short[]>
		{
			new short[2] { 72, 1 },
			new short[2] { 74, 1 },
			new short[2] { 60, 1 },
			new short[2] { 78, 1 },
			new short[2] { 79, 1 },
			new short[2] { 80, 1 },
			new short[2] { 79, 1 },
			new short[2] { 80, 1 },
			new short[2] { 84, 1 },
			new short[2] { 66, 1 },
			new short[2] { 90, 1 },
			new short[2] { 91, 1 }
		};
		list3 = new List<short[]>();
		List<short[]> seriesBlockCore3 = list3;
		list3 = new List<short[]>();
		List<short[]> encircleBlockCore3 = list3;
		short[] sceneryBlockCore3 = new short[0];
		int[] caveName2 = new int[1] { 14 };
		int[] caveDesc2 = new int[1] { 15 };
		list4 = new List<short>();
		List<short> lovingItemSubTypes3 = list4;
		list4 = new List<short>();
		dataArray3.Add(new MapAreaItem(2, 2, 10, 11, 30, worldMapPos3, neighborAreas3, "largemap_part_1_chengzhen_0", miniMapIcons3, "MapCityChengdu", miniMapPos3, 0, 2, 9, enemyNests3, enemyNestCreationDateRanges3, settlementBlockCore3, organizationId3, -1, 0, null, developedBlockCore3, normalBlockCore3, wildBlockCore3, bigBaseBlockCore3, seriesBlockCore3, encircleBlockCore3, sceneryBlockCore3, 12, 13, caveName2, caveDesc2, EMapAreaAreaDirection.South, lovingItemSubTypes3, list4, showDarkAshStatus: true));
		List<MapAreaItem> dataArray4 = _dataArray;
		sbyte[] worldMapPos4 = new sbyte[2] { 10, 3 };
		AreaTravelRoute[] neighborAreas4 = new AreaTravelRoute[5]
		{
			new AreaTravelRoute(18, 4, null),
			new AreaTravelRoute(46, 20, new int[4] { 9, 3, 8, 3 }),
			new AreaTravelRoute(47, 10, new int[2] { 10, 2 }),
			new AreaTravelRoute(50, 10, new int[2] { 10, 2 }),
			new AreaTravelRoute(51, 4, null)
		};
		string[] miniMapIcons4 = new string[4] { "largemap_part_2_icon_hei_16", "largemap_part_2_icon_jin_16", "largemap_part_2_icon_lan_16", "largemap_part_2_icon_hui_16" };
		short[] miniMapPos4 = new short[2] { 220, 210 };
		List<EnemyNestCreationInfo[]> enemyNests4 = new List<EnemyNestCreationInfo[]>
		{
			new EnemyNestCreationInfo[2]
			{
				new EnemyNestCreationInfo(4, 4),
				new EnemyNestCreationInfo(5, 4)
			},
			new EnemyNestCreationInfo[2]
			{
				new EnemyNestCreationInfo(6, 5),
				new EnemyNestCreationInfo(7, 5)
			},
			new EnemyNestCreationInfo[2]
			{
				new EnemyNestCreationInfo(8, 12),
				new EnemyNestCreationInfo(9, 12)
			},
			new EnemyNestCreationInfo[3]
			{
				new EnemyNestCreationInfo(10, 7),
				new EnemyNestCreationInfo(10, 7),
				new EnemyNestCreationInfo(11, 7)
			},
			new EnemyNestCreationInfo[1]
			{
				new EnemyNestCreationInfo(14, 4)
			}
		};
		List<IntPair> enemyNestCreationDateRanges4 = new List<IntPair>
		{
			new IntPair(0, 12),
			new IntPair(12, 24),
			new IntPair(24, 36),
			new IntPair(36, 48),
			new IntPair(0, 0)
		};
		short[] settlementBlockCore4 = new short[3] { 3, 35, 36 };
		sbyte[] organizationId4 = new sbyte[3] { 23, 37, 38 };
		List<short[]> developedBlockCore4 = new List<short[]>
		{
			new short[2] { 39, 33 },
			new short[2] { 40, 33 },
			new short[2] { 41, 33 },
			new short[2] { 42, 33 },
			new short[2] { 43, 33 },
			new short[2] { 44, 33 },
			new short[2] { 45, 33 },
			new short[2] { 46, 33 },
			new short[2] { 47, 33 },
			new short[2] { 48, 33 },
			new short[2] { 49, 33 },
			new short[2] { 50, 33 },
			new short[2] { 51, 33 },
			new short[2] { 52, 33 },
			new short[2] { 53, 33 },
			new short[2] { 54, 33 },
			new short[2] { 55, 33 },
			new short[2] { 56, 33 },
			new short[2] { 109, 10 },
			new short[2] { 110, 10 },
			new short[2] { 111, 10 }
		};
		List<short[]> normalBlockCore4 = new List<short[]>
		{
			new short[2] { 57, 33 },
			new short[2] { 58, 33 },
			new short[2] { 59, 33 },
			new short[2] { 63, 33 },
			new short[2] { 64, 33 },
			new short[2] { 65, 33 },
			new short[2] { 69, 33 },
			new short[2] { 70, 33 },
			new short[2] { 71, 33 },
			new short[2] { 75, 33 },
			new short[2] { 76, 33 },
			new short[2] { 77, 33 },
			new short[2] { 81, 33 },
			new short[2] { 82, 33 },
			new short[2] { 83, 33 },
			new short[2] { 87, 33 },
			new short[2] { 88, 33 },
			new short[2] { 89, 33 },
			new short[2] { 112, 30 },
			new short[2] { 113, 30 },
			new short[2] { 114, 30 },
			new short[2] { 124, 30 }
		};
		List<short[]> wildBlockCore4 = new List<short[]>
		{
			new short[2] { 93, 33 },
			new short[2] { 94, 33 },
			new short[2] { 95, 33 },
			new short[2] { 96, 33 },
			new short[2] { 97, 33 },
			new short[2] { 98, 33 },
			new short[2] { 99, 33 },
			new short[2] { 100, 33 },
			new short[2] { 101, 33 },
			new short[2] { 102, 33 },
			new short[2] { 103, 33 },
			new short[2] { 104, 33 },
			new short[2] { 105, 33 },
			new short[2] { 106, 33 },
			new short[2] { 107, 33 },
			new short[2] { 108, 33 },
			new short[2] { 115, 40 },
			new short[2] { 116, 40 },
			new short[2] { 117, 40 },
			new short[2] { 124, 30 }
		};
		List<short[]> bigBaseBlockCore4 = new List<short[]>
		{
			new short[2] { 73, 1 },
			new short[2] { 74, 1 },
			new short[2] { 73, 1 },
			new short[2] { 60, 1 },
			new short[2] { 61, 1 },
			new short[2] { 62, 1 },
			new short[2] { 78, 1 },
			new short[2] { 84, 1 },
			new short[2] { 66, 1 },
			new short[2] { 67, 1 },
			new short[2] { 90, 1 }
		};
		list3 = new List<short[]>();
		List<short[]> seriesBlockCore4 = list3;
		list3 = new List<short[]>();
		List<short[]> encircleBlockCore4 = list3;
		short[] sceneryBlockCore4 = new short[0];
		list4 = new List<short>();
		List<short> lovingItemSubTypes4 = list4;
		list4 = new List<short>();
		dataArray4.Add(new MapAreaItem(3, 3, 16, 17, 30, worldMapPos4, neighborAreas4, "largemap_part_1_chengzhen_4", miniMapIcons4, "MapCityGuizhou", miniMapPos4, 0, 2, 9, enemyNests4, enemyNestCreationDateRanges4, settlementBlockCore4, organizationId4, -1, 0, null, developedBlockCore4, normalBlockCore4, wildBlockCore4, bigBaseBlockCore4, seriesBlockCore4, encircleBlockCore4, sceneryBlockCore4, 18, 19, null, null, EMapAreaAreaDirection.South, lovingItemSubTypes4, list4, showDarkAshStatus: true));
		List<MapAreaItem> dataArray5 = _dataArray;
		sbyte[] worldMapPos5 = new sbyte[2] { 12, 12 };
		AreaTravelRoute[] neighborAreas5 = new AreaTravelRoute[4]
		{
			new AreaTravelRoute(8, 8, new int[2] { 12, 11 }),
			new AreaTravelRoute(19, 4, null),
			new AreaTravelRoute(31, 10, new int[2] { 12, 13 }),
			new AreaTravelRoute(55, 10, new int[2] { 13, 12 })
		};
		string[] miniMapIcons5 = new string[4] { "largemap_part_2_icon_hei_8", "largemap_part_2_icon_jin_8", "largemap_part_2_icon_lan_8", "largemap_part_2_icon_hui_8" };
		short[] miniMapPos5 = new short[2] { 180, 200 };
		List<EnemyNestCreationInfo[]> enemyNests5 = new List<EnemyNestCreationInfo[]>
		{
			new EnemyNestCreationInfo[2]
			{
				new EnemyNestCreationInfo(4, 4),
				new EnemyNestCreationInfo(5, 4)
			},
			new EnemyNestCreationInfo[2]
			{
				new EnemyNestCreationInfo(6, 5),
				new EnemyNestCreationInfo(7, 5)
			},
			new EnemyNestCreationInfo[2]
			{
				new EnemyNestCreationInfo(8, 12),
				new EnemyNestCreationInfo(9, 12)
			},
			new EnemyNestCreationInfo[3]
			{
				new EnemyNestCreationInfo(10, 7),
				new EnemyNestCreationInfo(10, 7),
				new EnemyNestCreationInfo(11, 7)
			},
			new EnemyNestCreationInfo[1]
			{
				new EnemyNestCreationInfo(14, 4)
			}
		};
		List<IntPair> enemyNestCreationDateRanges5 = new List<IntPair>
		{
			new IntPair(0, 12),
			new IntPair(12, 24),
			new IntPair(24, 36),
			new IntPair(36, 48),
			new IntPair(0, 0)
		};
		short[] settlementBlockCore5 = new short[3] { 4, 35, 36 };
		sbyte[] organizationId5 = new sbyte[3] { 24, 37, 38 };
		List<short[]> developedBlockCore5 = new List<short[]>
		{
			new short[2] { 39, 33 },
			new short[2] { 40, 33 },
			new short[2] { 41, 33 },
			new short[2] { 42, 33 },
			new short[2] { 43, 33 },
			new short[2] { 44, 33 },
			new short[2] { 45, 33 },
			new short[2] { 46, 33 },
			new short[2] { 47, 33 },
			new short[2] { 48, 33 },
			new short[2] { 49, 33 },
			new short[2] { 50, 33 },
			new short[2] { 51, 33 },
			new short[2] { 52, 33 },
			new short[2] { 53, 33 },
			new short[2] { 54, 33 },
			new short[2] { 55, 33 },
			new short[2] { 56, 33 },
			new short[2] { 109, 10 },
			new short[2] { 110, 10 },
			new short[2] { 111, 10 }
		};
		List<short[]> normalBlockCore5 = new List<short[]>
		{
			new short[2] { 57, 33 },
			new short[2] { 58, 33 },
			new short[2] { 59, 33 },
			new short[2] { 63, 33 },
			new short[2] { 64, 33 },
			new short[2] { 65, 33 },
			new short[2] { 69, 33 },
			new short[2] { 70, 33 },
			new short[2] { 71, 33 },
			new short[2] { 75, 33 },
			new short[2] { 76, 33 },
			new short[2] { 77, 33 },
			new short[2] { 81, 33 },
			new short[2] { 82, 33 },
			new short[2] { 83, 33 },
			new short[2] { 87, 33 },
			new short[2] { 88, 33 },
			new short[2] { 89, 33 },
			new short[2] { 112, 30 },
			new short[2] { 113, 30 },
			new short[2] { 114, 30 },
			new short[2] { 124, 30 }
		};
		List<short[]> wildBlockCore5 = new List<short[]>
		{
			new short[2] { 93, 33 },
			new short[2] { 94, 33 },
			new short[2] { 95, 33 },
			new short[2] { 96, 33 },
			new short[2] { 97, 33 },
			new short[2] { 98, 33 },
			new short[2] { 99, 33 },
			new short[2] { 100, 33 },
			new short[2] { 101, 33 },
			new short[2] { 102, 33 },
			new short[2] { 103, 33 },
			new short[2] { 104, 33 },
			new short[2] { 105, 33 },
			new short[2] { 106, 33 },
			new short[2] { 107, 33 },
			new short[2] { 108, 33 },
			new short[2] { 115, 40 },
			new short[2] { 116, 40 },
			new short[2] { 117, 40 },
			new short[2] { 124, 30 }
		};
		List<short[]> bigBaseBlockCore5 = new List<short[]>
		{
			new short[2] { 72, 1 },
			new short[2] { 60, 1 },
			new short[2] { 78, 1 },
			new short[2] { 79, 1 },
			new short[2] { 84, 1 },
			new short[2] { 85, 1 },
			new short[2] { 66, 1 },
			new short[2] { 90, 1 },
			new short[2] { 91, 1 },
			new short[2] { 92, 1 }
		};
		list3 = new List<short[]>();
		List<short[]> seriesBlockCore5 = list3;
		list3 = new List<short[]>();
		List<short[]> encircleBlockCore5 = list3;
		short[] sceneryBlockCore5 = new short[0];
		list4 = new List<short>();
		List<short> lovingItemSubTypes5 = list4;
		list4 = new List<short>();
		dataArray5.Add(new MapAreaItem(4, 4, 20, 21, 30, worldMapPos5, neighborAreas5, "largemap_part_1_chengzhen_13", miniMapIcons5, "MapCityXiangyang", miniMapPos5, 0, 2, 9, enemyNests5, enemyNestCreationDateRanges5, settlementBlockCore5, organizationId5, -1, 0, null, developedBlockCore5, normalBlockCore5, wildBlockCore5, bigBaseBlockCore5, seriesBlockCore5, encircleBlockCore5, sceneryBlockCore5, 22, 23, null, null, EMapAreaAreaDirection.South, lovingItemSubTypes5, list4, showDarkAshStatus: true));
		List<MapAreaItem> dataArray6 = _dataArray;
		sbyte[] worldMapPos6 = new sbyte[2] { 12, 18 };
		AreaTravelRoute[] neighborAreas6 = new AreaTravelRoute[4]
		{
			new AreaTravelRoute(20, 4, null),
			new AreaTravelRoute(34, 22, new int[4] { 12, 17, 12, 16 }),
			new AreaTravelRoute(35, 22, new int[4] { 12, 17, 12, 16 }),
			new AreaTravelRoute(62, 4, null)
		};
		string[] miniMapIcons6 = new string[4] { "largemap_part_2_icon_hei_6", "largemap_part_2_icon_jin_6", "largemap_part_2_icon_lan_6", "largemap_part_2_icon_hui_6" };
		short[] miniMapPos6 = new short[2] { 200, 190 };
		List<EnemyNestCreationInfo[]> enemyNests6 = new List<EnemyNestCreationInfo[]>
		{
			new EnemyNestCreationInfo[2]
			{
				new EnemyNestCreationInfo(4, 4),
				new EnemyNestCreationInfo(5, 4)
			},
			new EnemyNestCreationInfo[2]
			{
				new EnemyNestCreationInfo(6, 5),
				new EnemyNestCreationInfo(7, 5)
			},
			new EnemyNestCreationInfo[2]
			{
				new EnemyNestCreationInfo(8, 12),
				new EnemyNestCreationInfo(9, 12)
			},
			new EnemyNestCreationInfo[3]
			{
				new EnemyNestCreationInfo(10, 7),
				new EnemyNestCreationInfo(10, 7),
				new EnemyNestCreationInfo(11, 7)
			},
			new EnemyNestCreationInfo[1]
			{
				new EnemyNestCreationInfo(14, 4)
			}
		};
		List<IntPair> enemyNestCreationDateRanges6 = new List<IntPair>
		{
			new IntPair(0, 12),
			new IntPair(12, 24),
			new IntPair(24, 36),
			new IntPair(36, 48),
			new IntPair(0, 0)
		};
		short[] settlementBlockCore6 = new short[3] { 5, 35, 36 };
		sbyte[] organizationId6 = new sbyte[3] { 25, 37, 38 };
		List<short[]> developedBlockCore6 = new List<short[]>
		{
			new short[2] { 39, 33 },
			new short[2] { 40, 33 },
			new short[2] { 41, 33 },
			new short[2] { 42, 33 },
			new short[2] { 43, 33 },
			new short[2] { 44, 33 },
			new short[2] { 45, 33 },
			new short[2] { 46, 33 },
			new short[2] { 47, 33 },
			new short[2] { 48, 33 },
			new short[2] { 49, 33 },
			new short[2] { 50, 33 },
			new short[2] { 51, 33 },
			new short[2] { 52, 33 },
			new short[2] { 53, 33 },
			new short[2] { 54, 33 },
			new short[2] { 55, 33 },
			new short[2] { 56, 33 },
			new short[2] { 109, 10 },
			new short[2] { 110, 10 },
			new short[2] { 111, 10 }
		};
		List<short[]> normalBlockCore6 = new List<short[]>
		{
			new short[2] { 57, 33 },
			new short[2] { 58, 33 },
			new short[2] { 59, 33 },
			new short[2] { 63, 33 },
			new short[2] { 64, 33 },
			new short[2] { 65, 33 },
			new short[2] { 69, 33 },
			new short[2] { 70, 33 },
			new short[2] { 71, 33 },
			new short[2] { 75, 33 },
			new short[2] { 76, 33 },
			new short[2] { 77, 33 },
			new short[2] { 81, 33 },
			new short[2] { 82, 33 },
			new short[2] { 83, 33 },
			new short[2] { 87, 33 },
			new short[2] { 88, 33 },
			new short[2] { 89, 33 },
			new short[2] { 112, 30 },
			new short[2] { 113, 30 },
			new short[2] { 114, 30 },
			new short[2] { 124, 30 }
		};
		List<short[]> wildBlockCore6 = new List<short[]>
		{
			new short[2] { 93, 33 },
			new short[2] { 94, 33 },
			new short[2] { 95, 33 },
			new short[2] { 96, 33 },
			new short[2] { 97, 33 },
			new short[2] { 98, 33 },
			new short[2] { 99, 33 },
			new short[2] { 100, 33 },
			new short[2] { 101, 33 },
			new short[2] { 102, 33 },
			new short[2] { 103, 33 },
			new short[2] { 104, 33 },
			new short[2] { 105, 33 },
			new short[2] { 106, 33 },
			new short[2] { 107, 33 },
			new short[2] { 108, 33 },
			new short[2] { 115, 40 },
			new short[2] { 116, 40 },
			new short[2] { 117, 40 },
			new short[2] { 124, 30 }
		};
		List<short[]> bigBaseBlockCore6 = new List<short[]>
		{
			new short[2] { 72, 1 },
			new short[2] { 73, 1 },
			new short[2] { 74, 1 },
			new short[2] { 60, 1 },
			new short[2] { 61, 1 },
			new short[2] { 79, 1 },
			new short[2] { 80, 1 },
			new short[2] { 84, 1 },
			new short[2] { 66, 1 },
			new short[2] { 90, 1 }
		};
		list3 = new List<short[]>();
		List<short[]> seriesBlockCore6 = list3;
		list3 = new List<short[]>();
		List<short[]> encircleBlockCore6 = list3;
		short[] sceneryBlockCore6 = new short[0];
		list4 = new List<short>();
		List<short> lovingItemSubTypes6 = list4;
		list4 = new List<short>();
		dataArray6.Add(new MapAreaItem(5, 5, 24, 25, 30, worldMapPos6, neighborAreas6, "largemap_part_1_chengzhen_12", miniMapIcons6, "MapCityTaiyuan", miniMapPos6, 0, 2, 9, enemyNests6, enemyNestCreationDateRanges6, settlementBlockCore6, organizationId6, -1, 0, null, developedBlockCore6, normalBlockCore6, wildBlockCore6, bigBaseBlockCore6, seriesBlockCore6, encircleBlockCore6, sceneryBlockCore6, 26, 27, null, null, EMapAreaAreaDirection.South, lovingItemSubTypes6, list4, showDarkAshStatus: true));
		List<MapAreaItem> dataArray7 = _dataArray;
		sbyte[] worldMapPos7 = new sbyte[2] { 15, 1 };
		AreaTravelRoute[] neighborAreas7 = new AreaTravelRoute[4]
		{
			new AreaTravelRoute(21, 4, null),
			new AreaTravelRoute(66, 4, null),
			new AreaTravelRoute(67, 4, null),
			new AreaTravelRoute(68, 4, null)
		};
		string[] miniMapIcons7 = new string[4] { "largemap_part_2_icon_hei_17", "largemap_part_2_icon_jin_17", "largemap_part_2_icon_lan_17", "largemap_part_2_icon_hui_17" };
		short[] miniMapPos7 = new short[2] { 180, 160 };
		List<EnemyNestCreationInfo[]> enemyNests7 = new List<EnemyNestCreationInfo[]>
		{
			new EnemyNestCreationInfo[2]
			{
				new EnemyNestCreationInfo(4, 4),
				new EnemyNestCreationInfo(5, 4)
			},
			new EnemyNestCreationInfo[2]
			{
				new EnemyNestCreationInfo(6, 5),
				new EnemyNestCreationInfo(7, 5)
			},
			new EnemyNestCreationInfo[2]
			{
				new EnemyNestCreationInfo(8, 12),
				new EnemyNestCreationInfo(9, 12)
			},
			new EnemyNestCreationInfo[3]
			{
				new EnemyNestCreationInfo(10, 7),
				new EnemyNestCreationInfo(10, 7),
				new EnemyNestCreationInfo(11, 7)
			},
			new EnemyNestCreationInfo[1]
			{
				new EnemyNestCreationInfo(14, 4)
			}
		};
		List<IntPair> enemyNestCreationDateRanges7 = new List<IntPair>
		{
			new IntPair(0, 12),
			new IntPair(12, 24),
			new IntPair(24, 36),
			new IntPair(36, 48),
			new IntPair(0, 0)
		};
		short[] settlementBlockCore7 = new short[3] { 6, 35, 36 };
		sbyte[] organizationId7 = new sbyte[3] { 26, 37, 38 };
		List<short[]> developedBlockCore7 = new List<short[]>
		{
			new short[2] { 39, 33 },
			new short[2] { 40, 33 },
			new short[2] { 41, 33 },
			new short[2] { 42, 33 },
			new short[2] { 43, 33 },
			new short[2] { 44, 33 },
			new short[2] { 45, 33 },
			new short[2] { 46, 33 },
			new short[2] { 47, 33 },
			new short[2] { 48, 33 },
			new short[2] { 49, 33 },
			new short[2] { 50, 33 },
			new short[2] { 51, 33 },
			new short[2] { 52, 33 },
			new short[2] { 53, 33 },
			new short[2] { 54, 33 },
			new short[2] { 55, 33 },
			new short[2] { 56, 33 },
			new short[2] { 109, 10 },
			new short[2] { 110, 10 },
			new short[2] { 111, 10 }
		};
		List<short[]> normalBlockCore7 = new List<short[]>
		{
			new short[2] { 57, 33 },
			new short[2] { 58, 33 },
			new short[2] { 59, 33 },
			new short[2] { 63, 33 },
			new short[2] { 64, 33 },
			new short[2] { 65, 33 },
			new short[2] { 69, 33 },
			new short[2] { 70, 33 },
			new short[2] { 71, 33 },
			new short[2] { 75, 33 },
			new short[2] { 76, 33 },
			new short[2] { 77, 33 },
			new short[2] { 81, 33 },
			new short[2] { 82, 33 },
			new short[2] { 83, 33 },
			new short[2] { 87, 33 },
			new short[2] { 88, 33 },
			new short[2] { 89, 33 },
			new short[2] { 112, 30 },
			new short[2] { 113, 30 },
			new short[2] { 114, 30 },
			new short[2] { 124, 30 }
		};
		List<short[]> wildBlockCore7 = new List<short[]>
		{
			new short[2] { 93, 33 },
			new short[2] { 94, 33 },
			new short[2] { 95, 33 },
			new short[2] { 96, 33 },
			new short[2] { 97, 33 },
			new short[2] { 98, 33 },
			new short[2] { 99, 33 },
			new short[2] { 100, 33 },
			new short[2] { 101, 33 },
			new short[2] { 102, 33 },
			new short[2] { 103, 33 },
			new short[2] { 104, 33 },
			new short[2] { 105, 33 },
			new short[2] { 106, 33 },
			new short[2] { 107, 33 },
			new short[2] { 108, 33 },
			new short[2] { 115, 40 },
			new short[2] { 116, 40 },
			new short[2] { 117, 40 },
			new short[2] { 124, 30 }
		};
		List<short[]> bigBaseBlockCore7 = new List<short[]>
		{
			new short[2] { 72, 1 },
			new short[2] { 60, 1 },
			new short[2] { 61, 1 },
			new short[2] { 78, 1 },
			new short[2] { 79, 1 },
			new short[2] { 80, 1 },
			new short[2] { 84, 1 },
			new short[2] { 66, 1 },
			new short[2] { 90, 1 },
			new short[2] { 91, 1 }
		};
		list3 = new List<short[]>();
		List<short[]> seriesBlockCore7 = list3;
		list3 = new List<short[]>();
		List<short[]> encircleBlockCore7 = list3;
		short[] sceneryBlockCore7 = new short[0];
		int[] caveName3 = new int[1] { 32 };
		int[] caveDesc3 = new int[1] { 33 };
		list4 = new List<short>();
		List<short> lovingItemSubTypes7 = list4;
		list4 = new List<short>();
		dataArray7.Add(new MapAreaItem(6, 6, 28, 29, 30, worldMapPos7, neighborAreas7, "largemap_part_1_chengzhen_3", miniMapIcons7, "MapCityGuangzhou", miniMapPos7, 0, 2, 9, enemyNests7, enemyNestCreationDateRanges7, settlementBlockCore7, organizationId7, -1, 0, null, developedBlockCore7, normalBlockCore7, wildBlockCore7, bigBaseBlockCore7, seriesBlockCore7, encircleBlockCore7, sceneryBlockCore7, 30, 31, caveName3, caveDesc3, EMapAreaAreaDirection.South, lovingItemSubTypes7, list4, showDarkAshStatus: true));
		List<MapAreaItem> dataArray8 = _dataArray;
		sbyte[] worldMapPos8 = new sbyte[2] { 19, 19 };
		AreaTravelRoute[] neighborAreas8 = new AreaTravelRoute[2]
		{
			new AreaTravelRoute(22, 4, null),
			new AreaTravelRoute(76, 4, null)
		};
		string[] miniMapIcons8 = new string[4] { "largemap_part_2_icon_hei_4", "largemap_part_2_icon_jin_4", "largemap_part_2_icon_lan_4", "largemap_part_2_icon_hui_4" };
		short[] miniMapPos8 = new short[2] { 210, 200 };
		List<EnemyNestCreationInfo[]> enemyNests8 = new List<EnemyNestCreationInfo[]>
		{
			new EnemyNestCreationInfo[2]
			{
				new EnemyNestCreationInfo(4, 4),
				new EnemyNestCreationInfo(5, 4)
			},
			new EnemyNestCreationInfo[2]
			{
				new EnemyNestCreationInfo(6, 5),
				new EnemyNestCreationInfo(7, 5)
			},
			new EnemyNestCreationInfo[2]
			{
				new EnemyNestCreationInfo(8, 12),
				new EnemyNestCreationInfo(9, 12)
			},
			new EnemyNestCreationInfo[3]
			{
				new EnemyNestCreationInfo(10, 7),
				new EnemyNestCreationInfo(10, 7),
				new EnemyNestCreationInfo(11, 7)
			},
			new EnemyNestCreationInfo[1]
			{
				new EnemyNestCreationInfo(14, 4)
			}
		};
		List<IntPair> enemyNestCreationDateRanges8 = new List<IntPair>
		{
			new IntPair(0, 12),
			new IntPair(12, 24),
			new IntPair(24, 36),
			new IntPair(36, 48),
			new IntPair(0, 0)
		};
		short[] settlementBlockCore8 = new short[3] { 7, 35, 36 };
		sbyte[] organizationId8 = new sbyte[3] { 27, 37, 38 };
		List<short[]> developedBlockCore8 = new List<short[]>
		{
			new short[2] { 39, 33 },
			new short[2] { 40, 33 },
			new short[2] { 41, 33 },
			new short[2] { 42, 33 },
			new short[2] { 43, 33 },
			new short[2] { 44, 33 },
			new short[2] { 45, 33 },
			new short[2] { 46, 33 },
			new short[2] { 47, 33 },
			new short[2] { 48, 33 },
			new short[2] { 49, 33 },
			new short[2] { 50, 33 },
			new short[2] { 51, 33 },
			new short[2] { 52, 33 },
			new short[2] { 53, 33 },
			new short[2] { 54, 33 },
			new short[2] { 55, 33 },
			new short[2] { 56, 33 },
			new short[2] { 109, 10 },
			new short[2] { 110, 10 },
			new short[2] { 111, 10 }
		};
		List<short[]> normalBlockCore8 = new List<short[]>
		{
			new short[2] { 57, 33 },
			new short[2] { 58, 33 },
			new short[2] { 59, 33 },
			new short[2] { 63, 33 },
			new short[2] { 64, 33 },
			new short[2] { 65, 33 },
			new short[2] { 69, 33 },
			new short[2] { 70, 33 },
			new short[2] { 71, 33 },
			new short[2] { 75, 33 },
			new short[2] { 76, 33 },
			new short[2] { 77, 33 },
			new short[2] { 81, 33 },
			new short[2] { 82, 33 },
			new short[2] { 83, 33 },
			new short[2] { 87, 33 },
			new short[2] { 88, 33 },
			new short[2] { 89, 33 },
			new short[2] { 112, 30 },
			new short[2] { 113, 30 },
			new short[2] { 114, 30 },
			new short[2] { 124, 30 }
		};
		List<short[]> wildBlockCore8 = new List<short[]>
		{
			new short[2] { 93, 33 },
			new short[2] { 94, 33 },
			new short[2] { 95, 33 },
			new short[2] { 96, 33 },
			new short[2] { 97, 33 },
			new short[2] { 98, 33 },
			new short[2] { 99, 33 },
			new short[2] { 100, 33 },
			new short[2] { 101, 33 },
			new short[2] { 102, 33 },
			new short[2] { 103, 33 },
			new short[2] { 104, 33 },
			new short[2] { 105, 33 },
			new short[2] { 106, 33 },
			new short[2] { 107, 33 },
			new short[2] { 108, 33 },
			new short[2] { 115, 40 },
			new short[2] { 116, 40 },
			new short[2] { 117, 40 },
			new short[2] { 124, 30 }
		};
		List<short[]> bigBaseBlockCore8 = new List<short[]>
		{
			new short[2] { 72, 1 },
			new short[2] { 73, 1 },
			new short[2] { 74, 1 },
			new short[2] { 60, 1 },
			new short[2] { 61, 1 },
			new short[2] { 78, 1 },
			new short[2] { 79, 1 },
			new short[2] { 84, 1 },
			new short[2] { 66, 1 },
			new short[2] { 90, 1 }
		};
		list3 = new List<short[]>();
		List<short[]> seriesBlockCore8 = list3;
		list3 = new List<short[]>();
		List<short[]> encircleBlockCore8 = list3;
		short[] sceneryBlockCore8 = new short[0];
		list4 = new List<short>();
		List<short> lovingItemSubTypes8 = list4;
		list4 = new List<short>();
		dataArray8.Add(new MapAreaItem(7, 7, 34, 35, 30, worldMapPos8, neighborAreas8, "largemap_part_1_chengzhen_11", miniMapIcons8, "MapCityQingzhou", miniMapPos8, 0, 2, 9, enemyNests8, enemyNestCreationDateRanges8, settlementBlockCore8, organizationId8, -1, 0, null, developedBlockCore8, normalBlockCore8, wildBlockCore8, bigBaseBlockCore8, seriesBlockCore8, encircleBlockCore8, sceneryBlockCore8, 36, 37, null, null, EMapAreaAreaDirection.South, lovingItemSubTypes8, list4, showDarkAshStatus: true));
		List<MapAreaItem> dataArray9 = _dataArray;
		sbyte[] worldMapPos9 = new sbyte[2] { 12, 10 };
		AreaTravelRoute[] neighborAreas9 = new AreaTravelRoute[2]
		{
			new AreaTravelRoute(23, 4, null),
			new AreaTravelRoute(84, 10, new int[2] { 12, 9 })
		};
		string[] miniMapIcons9 = new string[4] { "largemap_part_2_icon_hei_13", "largemap_part_2_icon_jin_13", "largemap_part_2_icon_lan_13", "largemap_part_2_icon_hui_13" };
		short[] miniMapPos9 = new short[2] { 200, 270 };
		List<EnemyNestCreationInfo[]> enemyNests9 = new List<EnemyNestCreationInfo[]>
		{
			new EnemyNestCreationInfo[2]
			{
				new EnemyNestCreationInfo(4, 4),
				new EnemyNestCreationInfo(5, 4)
			},
			new EnemyNestCreationInfo[2]
			{
				new EnemyNestCreationInfo(6, 5),
				new EnemyNestCreationInfo(7, 5)
			},
			new EnemyNestCreationInfo[2]
			{
				new EnemyNestCreationInfo(8, 12),
				new EnemyNestCreationInfo(9, 12)
			},
			new EnemyNestCreationInfo[3]
			{
				new EnemyNestCreationInfo(10, 7),
				new EnemyNestCreationInfo(10, 7),
				new EnemyNestCreationInfo(11, 7)
			},
			new EnemyNestCreationInfo[1]
			{
				new EnemyNestCreationInfo(14, 4)
			}
		};
		List<IntPair> enemyNestCreationDateRanges9 = new List<IntPair>
		{
			new IntPair(0, 12),
			new IntPair(12, 24),
			new IntPair(24, 36),
			new IntPair(36, 48),
			new IntPair(0, 0)
		};
		short[] settlementBlockCore9 = new short[3] { 8, 35, 36 };
		sbyte[] organizationId9 = new sbyte[3] { 28, 37, 38 };
		List<short[]> developedBlockCore9 = new List<short[]>
		{
			new short[2] { 39, 33 },
			new short[2] { 40, 33 },
			new short[2] { 41, 33 },
			new short[2] { 42, 33 },
			new short[2] { 43, 33 },
			new short[2] { 44, 33 },
			new short[2] { 45, 33 },
			new short[2] { 46, 33 },
			new short[2] { 47, 33 },
			new short[2] { 48, 33 },
			new short[2] { 49, 33 },
			new short[2] { 50, 33 },
			new short[2] { 51, 33 },
			new short[2] { 52, 33 },
			new short[2] { 53, 33 },
			new short[2] { 54, 33 },
			new short[2] { 55, 33 },
			new short[2] { 56, 33 },
			new short[2] { 109, 10 },
			new short[2] { 110, 10 },
			new short[2] { 111, 10 }
		};
		List<short[]> normalBlockCore9 = new List<short[]>
		{
			new short[2] { 57, 33 },
			new short[2] { 58, 33 },
			new short[2] { 59, 33 },
			new short[2] { 63, 33 },
			new short[2] { 64, 33 },
			new short[2] { 65, 33 },
			new short[2] { 69, 33 },
			new short[2] { 70, 33 },
			new short[2] { 71, 33 },
			new short[2] { 75, 33 },
			new short[2] { 76, 33 },
			new short[2] { 77, 33 },
			new short[2] { 81, 33 },
			new short[2] { 82, 33 },
			new short[2] { 83, 33 },
			new short[2] { 87, 33 },
			new short[2] { 88, 33 },
			new short[2] { 89, 33 },
			new short[2] { 112, 30 },
			new short[2] { 113, 30 },
			new short[2] { 114, 30 },
			new short[2] { 124, 30 }
		};
		List<short[]> wildBlockCore9 = new List<short[]>
		{
			new short[2] { 93, 33 },
			new short[2] { 94, 33 },
			new short[2] { 95, 33 },
			new short[2] { 96, 33 },
			new short[2] { 97, 33 },
			new short[2] { 98, 33 },
			new short[2] { 99, 33 },
			new short[2] { 100, 33 },
			new short[2] { 101, 33 },
			new short[2] { 102, 33 },
			new short[2] { 103, 33 },
			new short[2] { 104, 33 },
			new short[2] { 105, 33 },
			new short[2] { 106, 33 },
			new short[2] { 107, 33 },
			new short[2] { 108, 33 },
			new short[2] { 115, 40 },
			new short[2] { 116, 40 },
			new short[2] { 117, 40 },
			new short[2] { 124, 30 }
		};
		List<short[]> bigBaseBlockCore9 = new List<short[]>
		{
			new short[2] { 72, 1 },
			new short[2] { 60, 1 },
			new short[2] { 61, 1 },
			new short[2] { 78, 1 },
			new short[2] { 79, 1 },
			new short[2] { 80, 1 },
			new short[2] { 84, 1 },
			new short[2] { 66, 1 },
			new short[2] { 91, 1 },
			new short[2] { 92, 1 }
		};
		list3 = new List<short[]>();
		List<short[]> seriesBlockCore9 = list3;
		list3 = new List<short[]>();
		List<short[]> encircleBlockCore9 = list3;
		short[] sceneryBlockCore9 = new short[0];
		list4 = new List<short>();
		List<short> lovingItemSubTypes9 = list4;
		list4 = new List<short>();
		dataArray9.Add(new MapAreaItem(8, 8, 38, 39, 30, worldMapPos9, neighborAreas9, "largemap_part_1_chengzhen_7", miniMapIcons9, "MapCityJiangling", miniMapPos9, 0, 2, 9, enemyNests9, enemyNestCreationDateRanges9, settlementBlockCore9, organizationId9, -1, 0, null, developedBlockCore9, normalBlockCore9, wildBlockCore9, bigBaseBlockCore9, seriesBlockCore9, encircleBlockCore9, sceneryBlockCore9, 40, 41, null, null, EMapAreaAreaDirection.South, lovingItemSubTypes9, list4, showDarkAshStatus: true));
		List<MapAreaItem> dataArray10 = _dataArray;
		sbyte[] worldMapPos10 = new sbyte[2] { 21, 5 };
		AreaTravelRoute[] neighborAreas10 = new AreaTravelRoute[5]
		{
			new AreaTravelRoute(24, 10, new int[2] { 20, 5 }),
			new AreaTravelRoute(87, 10, new int[2] { 20, 5 }),
			new AreaTravelRoute(89, 4, null),
			new AreaTravelRoute(91, 22, new int[4] { 21, 4, 22, 4 }),
			new AreaTravelRoute(92, 12, new int[2] { 21, 4 })
		};
		string[] miniMapIcons10 = new string[4] { "largemap_part_2_icon_hei_14", "largemap_part_2_icon_jin_14", "largemap_part_2_icon_lan_14", "largemap_part_2_icon_hui_14" };
		short[] miniMapPos10 = new short[2] { 170, 190 };
		List<EnemyNestCreationInfo[]> enemyNests10 = new List<EnemyNestCreationInfo[]>
		{
			new EnemyNestCreationInfo[2]
			{
				new EnemyNestCreationInfo(4, 4),
				new EnemyNestCreationInfo(5, 4)
			},
			new EnemyNestCreationInfo[2]
			{
				new EnemyNestCreationInfo(6, 5),
				new EnemyNestCreationInfo(7, 5)
			},
			new EnemyNestCreationInfo[2]
			{
				new EnemyNestCreationInfo(8, 12),
				new EnemyNestCreationInfo(9, 12)
			},
			new EnemyNestCreationInfo[3]
			{
				new EnemyNestCreationInfo(10, 7),
				new EnemyNestCreationInfo(10, 7),
				new EnemyNestCreationInfo(11, 7)
			},
			new EnemyNestCreationInfo[1]
			{
				new EnemyNestCreationInfo(14, 4)
			}
		};
		List<IntPair> enemyNestCreationDateRanges10 = new List<IntPair>
		{
			new IntPair(0, 12),
			new IntPair(12, 24),
			new IntPair(24, 36),
			new IntPair(36, 48),
			new IntPair(0, 0)
		};
		short[] settlementBlockCore10 = new short[3] { 9, 35, 36 };
		sbyte[] organizationId10 = new sbyte[3] { 29, 37, 38 };
		List<short[]> developedBlockCore10 = new List<short[]>
		{
			new short[2] { 39, 33 },
			new short[2] { 40, 33 },
			new short[2] { 41, 33 },
			new short[2] { 42, 33 },
			new short[2] { 43, 33 },
			new short[2] { 44, 33 },
			new short[2] { 45, 33 },
			new short[2] { 46, 33 },
			new short[2] { 47, 33 },
			new short[2] { 48, 33 },
			new short[2] { 49, 33 },
			new short[2] { 50, 33 },
			new short[2] { 51, 33 },
			new short[2] { 52, 33 },
			new short[2] { 53, 33 },
			new short[2] { 54, 33 },
			new short[2] { 55, 33 },
			new short[2] { 56, 33 },
			new short[2] { 109, 10 },
			new short[2] { 110, 10 },
			new short[2] { 111, 10 }
		};
		List<short[]> normalBlockCore10 = new List<short[]>
		{
			new short[2] { 57, 33 },
			new short[2] { 58, 33 },
			new short[2] { 59, 33 },
			new short[2] { 63, 33 },
			new short[2] { 64, 33 },
			new short[2] { 65, 33 },
			new short[2] { 69, 33 },
			new short[2] { 70, 33 },
			new short[2] { 71, 33 },
			new short[2] { 75, 33 },
			new short[2] { 76, 33 },
			new short[2] { 77, 33 },
			new short[2] { 81, 33 },
			new short[2] { 82, 33 },
			new short[2] { 83, 33 },
			new short[2] { 87, 33 },
			new short[2] { 88, 33 },
			new short[2] { 89, 33 },
			new short[2] { 112, 30 },
			new short[2] { 113, 30 },
			new short[2] { 114, 30 },
			new short[2] { 124, 30 }
		};
		List<short[]> wildBlockCore10 = new List<short[]>
		{
			new short[2] { 93, 33 },
			new short[2] { 94, 33 },
			new short[2] { 95, 33 },
			new short[2] { 96, 33 },
			new short[2] { 97, 33 },
			new short[2] { 98, 33 },
			new short[2] { 99, 33 },
			new short[2] { 100, 33 },
			new short[2] { 101, 33 },
			new short[2] { 102, 33 },
			new short[2] { 103, 33 },
			new short[2] { 104, 33 },
			new short[2] { 105, 33 },
			new short[2] { 106, 33 },
			new short[2] { 107, 33 },
			new short[2] { 108, 33 },
			new short[2] { 115, 40 },
			new short[2] { 116, 40 },
			new short[2] { 117, 40 },
			new short[2] { 124, 30 }
		};
		List<short[]> bigBaseBlockCore10 = new List<short[]>
		{
			new short[2] { 72, 1 },
			new short[2] { 73, 1 },
			new short[2] { 60, 1 },
			new short[2] { 78, 1 },
			new short[2] { 79, 1 },
			new short[2] { 80, 1 },
			new short[2] { 84, 1 },
			new short[2] { 66, 1 },
			new short[2] { 90, 1 },
			new short[2] { 92, 1 }
		};
		list3 = new List<short[]>();
		List<short[]> seriesBlockCore10 = list3;
		list3 = new List<short[]>();
		List<short[]> encircleBlockCore10 = list3;
		short[] sceneryBlockCore10 = new short[0];
		list4 = new List<short>();
		List<short> lovingItemSubTypes10 = list4;
		list4 = new List<short>();
		dataArray10.Add(new MapAreaItem(9, 9, 42, 43, 30, worldMapPos10, neighborAreas10, "largemap_part_1_chengzhen_2", miniMapIcons10, "MapCityFuzhou", miniMapPos10, 0, 2, 9, enemyNests10, enemyNestCreationDateRanges10, settlementBlockCore10, organizationId10, -1, 0, null, developedBlockCore10, normalBlockCore10, wildBlockCore10, bigBaseBlockCore10, seriesBlockCore10, encircleBlockCore10, sceneryBlockCore10, 44, 45, null, null, EMapAreaAreaDirection.South, lovingItemSubTypes10, list4, showDarkAshStatus: true));
		List<MapAreaItem> dataArray11 = _dataArray;
		sbyte[] worldMapPos11 = new sbyte[2] { 20, 23 };
		AreaTravelRoute[] neighborAreas11 = new AreaTravelRoute[3]
		{
			new AreaTravelRoute(73, 40, new int[8] { 20, 22, 20, 21, 19, 21, 18, 21 }),
			new AreaTravelRoute(96, 4, null),
			new AreaTravelRoute(97, 20, new int[4] { 21, 23, 22, 23 })
		};
		string[] miniMapIcons11 = new string[4] { "largemap_part_2_icon_hei_3", "largemap_part_2_icon_jin_3", "largemap_part_2_icon_lan_3", "largemap_part_2_icon_hui_3" };
		short[] miniMapPos11 = new short[2] { 140, 160 };
		List<EnemyNestCreationInfo[]> enemyNests11 = new List<EnemyNestCreationInfo[]>
		{
			new EnemyNestCreationInfo[2]
			{
				new EnemyNestCreationInfo(4, 4),
				new EnemyNestCreationInfo(5, 4)
			},
			new EnemyNestCreationInfo[2]
			{
				new EnemyNestCreationInfo(6, 5),
				new EnemyNestCreationInfo(7, 5)
			},
			new EnemyNestCreationInfo[2]
			{
				new EnemyNestCreationInfo(8, 12),
				new EnemyNestCreationInfo(9, 12)
			},
			new EnemyNestCreationInfo[3]
			{
				new EnemyNestCreationInfo(10, 7),
				new EnemyNestCreationInfo(10, 7),
				new EnemyNestCreationInfo(11, 7)
			},
			new EnemyNestCreationInfo[1]
			{
				new EnemyNestCreationInfo(14, 4)
			}
		};
		List<IntPair> enemyNestCreationDateRanges11 = new List<IntPair>
		{
			new IntPair(0, 12),
			new IntPair(12, 24),
			new IntPair(24, 36),
			new IntPair(36, 48),
			new IntPair(0, 0)
		};
		short[] settlementBlockCore11 = new short[3] { 10, 35, 36 };
		sbyte[] organizationId11 = new sbyte[3] { 30, 37, 38 };
		List<short[]> developedBlockCore11 = new List<short[]>
		{
			new short[2] { 39, 33 },
			new short[2] { 40, 33 },
			new short[2] { 41, 33 },
			new short[2] { 42, 33 },
			new short[2] { 43, 33 },
			new short[2] { 44, 33 },
			new short[2] { 45, 33 },
			new short[2] { 46, 33 },
			new short[2] { 47, 33 },
			new short[2] { 48, 33 },
			new short[2] { 49, 33 },
			new short[2] { 50, 33 },
			new short[2] { 51, 33 },
			new short[2] { 52, 33 },
			new short[2] { 53, 33 },
			new short[2] { 54, 33 },
			new short[2] { 55, 33 },
			new short[2] { 56, 33 },
			new short[2] { 109, 10 },
			new short[2] { 110, 10 },
			new short[2] { 111, 10 }
		};
		List<short[]> normalBlockCore11 = new List<short[]>
		{
			new short[2] { 57, 33 },
			new short[2] { 58, 33 },
			new short[2] { 59, 33 },
			new short[2] { 63, 33 },
			new short[2] { 64, 33 },
			new short[2] { 65, 33 },
			new short[2] { 69, 33 },
			new short[2] { 70, 33 },
			new short[2] { 71, 33 },
			new short[2] { 75, 33 },
			new short[2] { 76, 33 },
			new short[2] { 77, 33 },
			new short[2] { 81, 33 },
			new short[2] { 82, 33 },
			new short[2] { 83, 33 },
			new short[2] { 87, 33 },
			new short[2] { 88, 33 },
			new short[2] { 89, 33 },
			new short[2] { 112, 30 },
			new short[2] { 113, 30 },
			new short[2] { 114, 30 },
			new short[2] { 124, 30 }
		};
		List<short[]> wildBlockCore11 = new List<short[]>
		{
			new short[2] { 93, 33 },
			new short[2] { 94, 33 },
			new short[2] { 95, 33 },
			new short[2] { 96, 33 },
			new short[2] { 97, 33 },
			new short[2] { 98, 33 },
			new short[2] { 99, 33 },
			new short[2] { 100, 33 },
			new short[2] { 101, 33 },
			new short[2] { 102, 33 },
			new short[2] { 103, 33 },
			new short[2] { 104, 33 },
			new short[2] { 105, 33 },
			new short[2] { 106, 33 },
			new short[2] { 107, 33 },
			new short[2] { 108, 33 },
			new short[2] { 115, 40 },
			new short[2] { 116, 40 },
			new short[2] { 117, 40 },
			new short[2] { 124, 30 }
		};
		List<short[]> bigBaseBlockCore11 = new List<short[]>
		{
			new short[2] { 73, 1 },
			new short[2] { 74, 1 },
			new short[2] { 61, 1 },
			new short[2] { 62, 1 },
			new short[2] { 78, 1 },
			new short[2] { 84, 1 },
			new short[2] { 85, 1 },
			new short[2] { 86, 1 },
			new short[2] { 66, 1 },
			new short[2] { 90, 1 }
		};
		list3 = new List<short[]>();
		List<short[]> seriesBlockCore11 = list3;
		list3 = new List<short[]>();
		List<short[]> encircleBlockCore11 = list3;
		short[] sceneryBlockCore11 = new short[0];
		list4 = new List<short>();
		List<short> lovingItemSubTypes11 = list4;
		list4 = new List<short>();
		dataArray11.Add(new MapAreaItem(10, 10, 46, 47, 30, worldMapPos11, neighborAreas11, "largemap_part_1_chengzhen_9", miniMapIcons11, "MapCityLiaoyang", miniMapPos11, 0, 2, 9, enemyNests11, enemyNestCreationDateRanges11, settlementBlockCore11, organizationId11, -1, 0, null, developedBlockCore11, normalBlockCore11, wildBlockCore11, bigBaseBlockCore11, seriesBlockCore11, encircleBlockCore11, sceneryBlockCore11, 48, 49, null, null, EMapAreaAreaDirection.South, lovingItemSubTypes11, list4, showDarkAshStatus: true));
		List<MapAreaItem> dataArray12 = _dataArray;
		sbyte[] worldMapPos12 = new sbyte[2] { 6, 14 };
		AreaTravelRoute[] neighborAreas12 = new AreaTravelRoute[3]
		{
			new AreaTravelRoute(42, 10, new int[2] { 6, 13 }),
			new AreaTravelRoute(105, 4, null),
			new AreaTravelRoute(107, 4, null)
		};
		string[] miniMapIcons12 = new string[4] { "largemap_part_2_icon_hei_7", "largemap_part_2_icon_jin_7", "largemap_part_2_icon_lan_7", "largemap_part_2_icon_hui_7" };
		short[] miniMapPos12 = new short[2] { 240, 150 };
		List<EnemyNestCreationInfo[]> enemyNests12 = new List<EnemyNestCreationInfo[]>
		{
			new EnemyNestCreationInfo[2]
			{
				new EnemyNestCreationInfo(4, 4),
				new EnemyNestCreationInfo(5, 4)
			},
			new EnemyNestCreationInfo[2]
			{
				new EnemyNestCreationInfo(6, 5),
				new EnemyNestCreationInfo(7, 5)
			},
			new EnemyNestCreationInfo[2]
			{
				new EnemyNestCreationInfo(8, 12),
				new EnemyNestCreationInfo(9, 12)
			},
			new EnemyNestCreationInfo[3]
			{
				new EnemyNestCreationInfo(10, 7),
				new EnemyNestCreationInfo(10, 7),
				new EnemyNestCreationInfo(11, 7)
			},
			new EnemyNestCreationInfo[1]
			{
				new EnemyNestCreationInfo(14, 4)
			}
		};
		List<IntPair> enemyNestCreationDateRanges12 = new List<IntPair>
		{
			new IntPair(0, 12),
			new IntPair(12, 24),
			new IntPair(24, 36),
			new IntPair(36, 48),
			new IntPair(0, 0)
		};
		short[] settlementBlockCore12 = new short[3] { 11, 35, 36 };
		sbyte[] organizationId12 = new sbyte[3] { 31, 37, 38 };
		List<short[]> developedBlockCore12 = new List<short[]>
		{
			new short[2] { 39, 33 },
			new short[2] { 40, 33 },
			new short[2] { 41, 33 },
			new short[2] { 42, 33 },
			new short[2] { 43, 33 },
			new short[2] { 44, 33 },
			new short[2] { 45, 33 },
			new short[2] { 46, 33 },
			new short[2] { 47, 33 },
			new short[2] { 48, 33 },
			new short[2] { 49, 33 },
			new short[2] { 50, 33 },
			new short[2] { 51, 33 },
			new short[2] { 52, 33 },
			new short[2] { 53, 33 },
			new short[2] { 54, 33 },
			new short[2] { 55, 33 },
			new short[2] { 56, 33 },
			new short[2] { 109, 10 },
			new short[2] { 110, 10 },
			new short[2] { 111, 10 }
		};
		List<short[]> normalBlockCore12 = new List<short[]>
		{
			new short[2] { 57, 33 },
			new short[2] { 58, 33 },
			new short[2] { 59, 33 },
			new short[2] { 63, 33 },
			new short[2] { 64, 33 },
			new short[2] { 65, 33 },
			new short[2] { 69, 33 },
			new short[2] { 70, 33 },
			new short[2] { 71, 33 },
			new short[2] { 75, 33 },
			new short[2] { 76, 33 },
			new short[2] { 77, 33 },
			new short[2] { 81, 33 },
			new short[2] { 82, 33 },
			new short[2] { 83, 33 },
			new short[2] { 87, 33 },
			new short[2] { 88, 33 },
			new short[2] { 89, 33 },
			new short[2] { 112, 30 },
			new short[2] { 113, 30 },
			new short[2] { 114, 30 },
			new short[2] { 124, 30 }
		};
		List<short[]> wildBlockCore12 = new List<short[]>
		{
			new short[2] { 93, 33 },
			new short[2] { 94, 33 },
			new short[2] { 95, 33 },
			new short[2] { 96, 33 },
			new short[2] { 97, 33 },
			new short[2] { 98, 33 },
			new short[2] { 99, 33 },
			new short[2] { 100, 33 },
			new short[2] { 101, 33 },
			new short[2] { 102, 33 },
			new short[2] { 103, 33 },
			new short[2] { 104, 33 },
			new short[2] { 105, 33 },
			new short[2] { 106, 33 },
			new short[2] { 107, 33 },
			new short[2] { 108, 33 },
			new short[2] { 115, 40 },
			new short[2] { 116, 40 },
			new short[2] { 117, 40 },
			new short[2] { 124, 30 }
		};
		List<short[]> bigBaseBlockCore12 = new List<short[]>
		{
			new short[2] { 72, 1 },
			new short[2] { 74, 1 },
			new short[2] { 60, 1 },
			new short[2] { 61, 1 },
			new short[2] { 62, 1 },
			new short[2] { 78, 1 },
			new short[2] { 85, 1 },
			new short[2] { 86, 1 },
			new short[2] { 66, 1 },
			new short[2] { 90, 1 }
		};
		list3 = new List<short[]>();
		List<short[]> seriesBlockCore12 = list3;
		list3 = new List<short[]>();
		List<short[]> encircleBlockCore12 = list3;
		short[] sceneryBlockCore12 = new short[0];
		int[] caveName4 = new int[1] { 54 };
		int[] caveDesc4 = new int[1] { 55 };
		list4 = new List<short>();
		List<short> lovingItemSubTypes12 = list4;
		list4 = new List<short>();
		dataArray12.Add(new MapAreaItem(11, 11, 50, 51, 30, worldMapPos12, neighborAreas12, "largemap_part_1_chengzhen_10", miniMapIcons12, "MapCityQinzhou", miniMapPos12, 0, 2, 9, enemyNests12, enemyNestCreationDateRanges12, settlementBlockCore12, organizationId12, -1, 0, null, developedBlockCore12, normalBlockCore12, wildBlockCore12, bigBaseBlockCore12, seriesBlockCore12, encircleBlockCore12, sceneryBlockCore12, 52, 53, caveName4, caveDesc4, EMapAreaAreaDirection.South, lovingItemSubTypes12, list4, showDarkAshStatus: true));
		List<MapAreaItem> dataArray13 = _dataArray;
		sbyte[] worldMapPos13 = new sbyte[2] { 1, 4 };
		AreaTravelRoute[] neighborAreas13 = new AreaTravelRoute[2]
		{
			new AreaTravelRoute(111, 4, null),
			new AreaTravelRoute(114, 20, new int[4] { 2, 4, 3, 4 })
		};
		string[] miniMapIcons13 = new string[4] { "largemap_part_2_icon_hei_15", "largemap_part_2_icon_jin_15", "largemap_part_2_icon_lan_15", "largemap_part_2_icon_hui_15" };
		short[] miniMapPos13 = new short[2] { 140, 220 };
		List<EnemyNestCreationInfo[]> enemyNests13 = new List<EnemyNestCreationInfo[]>
		{
			new EnemyNestCreationInfo[2]
			{
				new EnemyNestCreationInfo(4, 4),
				new EnemyNestCreationInfo(5, 4)
			},
			new EnemyNestCreationInfo[2]
			{
				new EnemyNestCreationInfo(6, 5),
				new EnemyNestCreationInfo(7, 5)
			},
			new EnemyNestCreationInfo[2]
			{
				new EnemyNestCreationInfo(8, 12),
				new EnemyNestCreationInfo(9, 12)
			},
			new EnemyNestCreationInfo[3]
			{
				new EnemyNestCreationInfo(10, 7),
				new EnemyNestCreationInfo(10, 7),
				new EnemyNestCreationInfo(11, 7)
			},
			new EnemyNestCreationInfo[1]
			{
				new EnemyNestCreationInfo(14, 4)
			}
		};
		List<IntPair> enemyNestCreationDateRanges13 = new List<IntPair>
		{
			new IntPair(0, 12),
			new IntPair(12, 24),
			new IntPair(24, 36),
			new IntPair(36, 48),
			new IntPair(0, 0)
		};
		short[] settlementBlockCore13 = new short[3] { 12, 35, 36 };
		sbyte[] organizationId13 = new sbyte[3] { 32, 37, 38 };
		List<short[]> developedBlockCore13 = new List<short[]>
		{
			new short[2] { 39, 33 },
			new short[2] { 40, 33 },
			new short[2] { 41, 33 },
			new short[2] { 42, 33 },
			new short[2] { 43, 33 },
			new short[2] { 44, 33 },
			new short[2] { 45, 33 },
			new short[2] { 46, 33 },
			new short[2] { 47, 33 },
			new short[2] { 48, 33 },
			new short[2] { 49, 33 },
			new short[2] { 50, 33 },
			new short[2] { 51, 33 },
			new short[2] { 52, 33 },
			new short[2] { 53, 33 },
			new short[2] { 54, 33 },
			new short[2] { 55, 33 },
			new short[2] { 56, 33 },
			new short[2] { 109, 10 },
			new short[2] { 110, 10 },
			new short[2] { 111, 10 }
		};
		List<short[]> normalBlockCore13 = new List<short[]>
		{
			new short[2] { 57, 33 },
			new short[2] { 58, 33 },
			new short[2] { 59, 33 },
			new short[2] { 63, 33 },
			new short[2] { 64, 33 },
			new short[2] { 65, 33 },
			new short[2] { 69, 33 },
			new short[2] { 70, 33 },
			new short[2] { 71, 33 },
			new short[2] { 75, 33 },
			new short[2] { 76, 33 },
			new short[2] { 77, 33 },
			new short[2] { 81, 33 },
			new short[2] { 82, 33 },
			new short[2] { 83, 33 },
			new short[2] { 87, 33 },
			new short[2] { 88, 33 },
			new short[2] { 89, 33 },
			new short[2] { 112, 30 },
			new short[2] { 113, 30 },
			new short[2] { 114, 30 },
			new short[2] { 124, 30 }
		};
		List<short[]> wildBlockCore13 = new List<short[]>
		{
			new short[2] { 93, 33 },
			new short[2] { 94, 33 },
			new short[2] { 95, 33 },
			new short[2] { 96, 33 },
			new short[2] { 97, 33 },
			new short[2] { 98, 33 },
			new short[2] { 99, 33 },
			new short[2] { 100, 33 },
			new short[2] { 101, 33 },
			new short[2] { 102, 33 },
			new short[2] { 103, 33 },
			new short[2] { 104, 33 },
			new short[2] { 105, 33 },
			new short[2] { 106, 33 },
			new short[2] { 107, 33 },
			new short[2] { 108, 33 },
			new short[2] { 115, 40 },
			new short[2] { 116, 40 },
			new short[2] { 117, 40 },
			new short[2] { 124, 30 }
		};
		List<short[]> bigBaseBlockCore13 = new List<short[]>
		{
			new short[2] { 73, 1 },
			new short[2] { 74, 1 },
			new short[2] { 60, 1 },
			new short[2] { 61, 1 },
			new short[2] { 62, 1 },
			new short[2] { 78, 1 },
			new short[2] { 84, 1 },
			new short[2] { 67, 1 },
			new short[2] { 68, 1 },
			new short[2] { 90, 1 }
		};
		list3 = new List<short[]>();
		List<short[]> seriesBlockCore13 = list3;
		list3 = new List<short[]>();
		List<short[]> encircleBlockCore13 = list3;
		short[] sceneryBlockCore13 = new short[0];
		list4 = new List<short>();
		List<short> lovingItemSubTypes13 = list4;
		list4 = new List<short>();
		dataArray13.Add(new MapAreaItem(12, 12, 56, 57, 30, worldMapPos13, neighborAreas13, "largemap_part_1_chengzhen_1", miniMapIcons13, "MapCityDali", miniMapPos13, 0, 2, 9, enemyNests13, enemyNestCreationDateRanges13, settlementBlockCore13, organizationId13, -1, 0, null, developedBlockCore13, normalBlockCore13, wildBlockCore13, bigBaseBlockCore13, seriesBlockCore13, encircleBlockCore13, sceneryBlockCore13, 58, 59, null, null, EMapAreaAreaDirection.South, lovingItemSubTypes13, list4, showDarkAshStatus: true));
		List<MapAreaItem> dataArray14 = _dataArray;
		sbyte[] worldMapPos14 = new sbyte[2] { 17, 13 };
		AreaTravelRoute[] neighborAreas14 = new AreaTravelRoute[3]
		{
			new AreaTravelRoute(115, 4, null),
			new AreaTravelRoute(117, 4, null),
			new AreaTravelRoute(130, 10, new int[2] { 18, 13 })
		};
		string[] miniMapIcons14 = new string[4] { "largemap_part_2_icon_hei_9", "largemap_part_2_icon_jin_9", "largemap_part_2_icon_lan_9", "largemap_part_2_icon_hui_9" };
		short[] miniMapPos14 = new short[2] { 180, 260 };
		List<EnemyNestCreationInfo[]> enemyNests14 = new List<EnemyNestCreationInfo[]>
		{
			new EnemyNestCreationInfo[2]
			{
				new EnemyNestCreationInfo(4, 4),
				new EnemyNestCreationInfo(5, 4)
			},
			new EnemyNestCreationInfo[2]
			{
				new EnemyNestCreationInfo(6, 5),
				new EnemyNestCreationInfo(7, 5)
			},
			new EnemyNestCreationInfo[2]
			{
				new EnemyNestCreationInfo(8, 12),
				new EnemyNestCreationInfo(9, 12)
			},
			new EnemyNestCreationInfo[3]
			{
				new EnemyNestCreationInfo(10, 7),
				new EnemyNestCreationInfo(10, 7),
				new EnemyNestCreationInfo(11, 7)
			},
			new EnemyNestCreationInfo[1]
			{
				new EnemyNestCreationInfo(14, 4)
			}
		};
		List<IntPair> enemyNestCreationDateRanges14 = new List<IntPair>
		{
			new IntPair(0, 12),
			new IntPair(12, 24),
			new IntPair(24, 36),
			new IntPair(36, 48),
			new IntPair(0, 0)
		};
		short[] settlementBlockCore14 = new short[3] { 13, 35, 36 };
		sbyte[] organizationId14 = new sbyte[3] { 33, 37, 38 };
		List<short[]> developedBlockCore14 = new List<short[]>
		{
			new short[2] { 39, 33 },
			new short[2] { 40, 33 },
			new short[2] { 41, 33 },
			new short[2] { 42, 33 },
			new short[2] { 43, 33 },
			new short[2] { 44, 33 },
			new short[2] { 45, 33 },
			new short[2] { 46, 33 },
			new short[2] { 47, 33 },
			new short[2] { 48, 33 },
			new short[2] { 49, 33 },
			new short[2] { 50, 33 },
			new short[2] { 51, 33 },
			new short[2] { 52, 33 },
			new short[2] { 53, 33 },
			new short[2] { 54, 33 },
			new short[2] { 55, 33 },
			new short[2] { 56, 33 },
			new short[2] { 109, 10 },
			new short[2] { 110, 10 },
			new short[2] { 111, 10 }
		};
		List<short[]> normalBlockCore14 = new List<short[]>
		{
			new short[2] { 57, 33 },
			new short[2] { 58, 33 },
			new short[2] { 59, 33 },
			new short[2] { 63, 33 },
			new short[2] { 64, 33 },
			new short[2] { 65, 33 },
			new short[2] { 69, 33 },
			new short[2] { 70, 33 },
			new short[2] { 71, 33 },
			new short[2] { 75, 33 },
			new short[2] { 76, 33 },
			new short[2] { 77, 33 },
			new short[2] { 81, 33 },
			new short[2] { 82, 33 },
			new short[2] { 83, 33 },
			new short[2] { 87, 33 },
			new short[2] { 88, 33 },
			new short[2] { 89, 33 },
			new short[2] { 112, 30 },
			new short[2] { 113, 30 },
			new short[2] { 114, 30 },
			new short[2] { 124, 30 }
		};
		List<short[]> wildBlockCore14 = new List<short[]>
		{
			new short[2] { 93, 33 },
			new short[2] { 94, 33 },
			new short[2] { 95, 33 },
			new short[2] { 96, 33 },
			new short[2] { 97, 33 },
			new short[2] { 98, 33 },
			new short[2] { 99, 33 },
			new short[2] { 100, 33 },
			new short[2] { 101, 33 },
			new short[2] { 102, 33 },
			new short[2] { 103, 33 },
			new short[2] { 104, 33 },
			new short[2] { 105, 33 },
			new short[2] { 106, 33 },
			new short[2] { 107, 33 },
			new short[2] { 108, 33 },
			new short[2] { 115, 40 },
			new short[2] { 116, 40 },
			new short[2] { 117, 40 },
			new short[2] { 124, 30 }
		};
		List<short[]> bigBaseBlockCore14 = new List<short[]>
		{
			new short[2] { 72, 1 },
			new short[2] { 60, 1 },
			new short[2] { 78, 1 },
			new short[2] { 84, 1 },
			new short[2] { 85, 1 },
			new short[2] { 86, 1 },
			new short[2] { 67, 1 },
			new short[2] { 68, 1 },
			new short[2] { 91, 1 },
			new short[2] { 92, 1 }
		};
		list3 = new List<short[]>();
		List<short[]> seriesBlockCore14 = list3;
		list3 = new List<short[]>();
		List<short[]> encircleBlockCore14 = list3;
		short[] sceneryBlockCore14 = new short[0];
		int[] caveName5 = new int[1] { 64 };
		int[] caveDesc5 = new int[1] { 65 };
		list4 = new List<short>();
		List<short> lovingItemSubTypes14 = list4;
		list4 = new List<short>();
		dataArray14.Add(new MapAreaItem(13, 13, 60, 61, 30, worldMapPos14, neighborAreas14, "largemap_part_1_chengzhen_6", miniMapIcons14, "MapCityShouchun", miniMapPos14, 0, 2, 9, enemyNests14, enemyNestCreationDateRanges14, settlementBlockCore14, organizationId14, -1, 0, null, developedBlockCore14, normalBlockCore14, wildBlockCore14, bigBaseBlockCore14, seriesBlockCore14, encircleBlockCore14, sceneryBlockCore14, 62, 63, caveName5, caveDesc5, EMapAreaAreaDirection.South, lovingItemSubTypes14, list4, showDarkAshStatus: true));
		List<MapAreaItem> dataArray15 = _dataArray;
		sbyte[] worldMapPos15 = new sbyte[2] { 21, 10 };
		AreaTravelRoute[] neighborAreas15 = new AreaTravelRoute[5]
		{
			new AreaTravelRoute(122, 4, null),
			new AreaTravelRoute(124, 10, new int[2] { 21, 9 }),
			new AreaTravelRoute(125, 4, null),
			new AreaTravelRoute(126, 10, new int[2] { 22, 10 }),
			new AreaTravelRoute(128, 10, new int[2] { 21, 9 })
		};
		string[] miniMapIcons15 = new string[4] { "largemap_part_2_icon_hei_10", "largemap_part_2_icon_jin_10", "largemap_part_2_icon_lan_10", "largemap_part_2_icon_hui_10" };
		short[] miniMapPos15 = new short[2] { 130, 180 };
		List<EnemyNestCreationInfo[]> enemyNests15 = new List<EnemyNestCreationInfo[]>
		{
			new EnemyNestCreationInfo[2]
			{
				new EnemyNestCreationInfo(4, 4),
				new EnemyNestCreationInfo(5, 4)
			},
			new EnemyNestCreationInfo[2]
			{
				new EnemyNestCreationInfo(6, 5),
				new EnemyNestCreationInfo(7, 5)
			},
			new EnemyNestCreationInfo[2]
			{
				new EnemyNestCreationInfo(8, 12),
				new EnemyNestCreationInfo(9, 12)
			},
			new EnemyNestCreationInfo[3]
			{
				new EnemyNestCreationInfo(10, 7),
				new EnemyNestCreationInfo(10, 7),
				new EnemyNestCreationInfo(11, 7)
			},
			new EnemyNestCreationInfo[1]
			{
				new EnemyNestCreationInfo(14, 4)
			}
		};
		List<IntPair> enemyNestCreationDateRanges15 = new List<IntPair>
		{
			new IntPair(0, 12),
			new IntPair(12, 24),
			new IntPair(24, 36),
			new IntPair(36, 48),
			new IntPair(0, 0)
		};
		short[] settlementBlockCore15 = new short[3] { 14, 35, 36 };
		sbyte[] organizationId15 = new sbyte[3] { 34, 37, 38 };
		List<short[]> developedBlockCore15 = new List<short[]>
		{
			new short[2] { 39, 33 },
			new short[2] { 40, 33 },
			new short[2] { 41, 33 },
			new short[2] { 42, 33 },
			new short[2] { 43, 33 },
			new short[2] { 44, 33 },
			new short[2] { 45, 33 },
			new short[2] { 46, 33 },
			new short[2] { 47, 33 },
			new short[2] { 48, 33 },
			new short[2] { 49, 33 },
			new short[2] { 50, 33 },
			new short[2] { 51, 33 },
			new short[2] { 52, 33 },
			new short[2] { 53, 33 },
			new short[2] { 54, 33 },
			new short[2] { 55, 33 },
			new short[2] { 56, 33 },
			new short[2] { 109, 10 },
			new short[2] { 110, 10 },
			new short[2] { 111, 10 }
		};
		List<short[]> normalBlockCore15 = new List<short[]>
		{
			new short[2] { 57, 33 },
			new short[2] { 58, 33 },
			new short[2] { 59, 33 },
			new short[2] { 63, 33 },
			new short[2] { 64, 33 },
			new short[2] { 65, 33 },
			new short[2] { 69, 33 },
			new short[2] { 70, 33 },
			new short[2] { 71, 33 },
			new short[2] { 75, 33 },
			new short[2] { 76, 33 },
			new short[2] { 77, 33 },
			new short[2] { 81, 33 },
			new short[2] { 82, 33 },
			new short[2] { 83, 33 },
			new short[2] { 87, 33 },
			new short[2] { 88, 33 },
			new short[2] { 89, 33 },
			new short[2] { 112, 30 },
			new short[2] { 113, 30 },
			new short[2] { 114, 30 },
			new short[2] { 124, 30 }
		};
		List<short[]> wildBlockCore15 = new List<short[]>
		{
			new short[2] { 93, 33 },
			new short[2] { 94, 33 },
			new short[2] { 95, 33 },
			new short[2] { 96, 33 },
			new short[2] { 97, 33 },
			new short[2] { 98, 33 },
			new short[2] { 99, 33 },
			new short[2] { 100, 33 },
			new short[2] { 101, 33 },
			new short[2] { 102, 33 },
			new short[2] { 103, 33 },
			new short[2] { 104, 33 },
			new short[2] { 105, 33 },
			new short[2] { 106, 33 },
			new short[2] { 107, 33 },
			new short[2] { 108, 33 },
			new short[2] { 115, 40 },
			new short[2] { 116, 40 },
			new short[2] { 117, 40 },
			new short[2] { 124, 30 }
		};
		List<short[]> bigBaseBlockCore15 = new List<short[]>
		{
			new short[2] { 72, 1 },
			new short[2] { 60, 1 },
			new short[2] { 78, 1 },
			new short[2] { 85, 1 },
			new short[2] { 86, 1 },
			new short[2] { 66, 1 },
			new short[2] { 68, 1 },
			new short[2] { 91, 1 },
			new short[2] { 92, 1 }
		};
		list3 = new List<short[]>();
		List<short[]> seriesBlockCore15 = list3;
		list3 = new List<short[]>();
		List<short[]> encircleBlockCore15 = list3;
		short[] sceneryBlockCore15 = new short[0];
		int[] caveName6 = new int[5] { 70, 71, 72, 73, 74 };
		int[] caveDesc6 = new int[5] { 75, 76, 77, 78, 79 };
		list4 = new List<short>();
		List<short> lovingItemSubTypes15 = list4;
		list4 = new List<short>();
		dataArray15.Add(new MapAreaItem(14, 14, 66, 67, 30, worldMapPos15, neighborAreas15, "largemap_part_1_chengzhen_5", miniMapIcons15, "MapCityHangzhou", miniMapPos15, 0, 2, 9, enemyNests15, enemyNestCreationDateRanges15, settlementBlockCore15, organizationId15, -1, 0, null, developedBlockCore15, normalBlockCore15, wildBlockCore15, bigBaseBlockCore15, seriesBlockCore15, encircleBlockCore15, sceneryBlockCore15, 68, 69, caveName6, caveDesc6, EMapAreaAreaDirection.South, lovingItemSubTypes15, list4, showDarkAshStatus: true));
		List<MapAreaItem> dataArray16 = _dataArray;
		sbyte[] worldMapPos16 = new sbyte[2] { 20, 13 };
		AreaTravelRoute[] neighborAreas16 = new AreaTravelRoute[3]
		{
			new AreaTravelRoute(129, 22, new int[4] { 21, 13, 22, 13 }),
			new AreaTravelRoute(130, 4, null),
			new AreaTravelRoute(133, 4, null)
		};
		string[] miniMapIcons16 = new string[4] { "largemap_part_2_icon_hei_11", "largemap_part_2_icon_jin_11", "largemap_part_2_icon_lan_11", "largemap_part_2_icon_hui_11" };
		short[] miniMapPos16 = new short[2] { 200, 130 };
		List<EnemyNestCreationInfo[]> enemyNests16 = new List<EnemyNestCreationInfo[]>
		{
			new EnemyNestCreationInfo[2]
			{
				new EnemyNestCreationInfo(4, 4),
				new EnemyNestCreationInfo(5, 4)
			},
			new EnemyNestCreationInfo[2]
			{
				new EnemyNestCreationInfo(6, 5),
				new EnemyNestCreationInfo(7, 5)
			},
			new EnemyNestCreationInfo[2]
			{
				new EnemyNestCreationInfo(8, 12),
				new EnemyNestCreationInfo(9, 12)
			},
			new EnemyNestCreationInfo[3]
			{
				new EnemyNestCreationInfo(10, 7),
				new EnemyNestCreationInfo(10, 7),
				new EnemyNestCreationInfo(11, 7)
			},
			new EnemyNestCreationInfo[1]
			{
				new EnemyNestCreationInfo(14, 4)
			}
		};
		List<IntPair> enemyNestCreationDateRanges16 = new List<IntPair>
		{
			new IntPair(0, 12),
			new IntPair(12, 24),
			new IntPair(24, 36),
			new IntPair(36, 48),
			new IntPair(0, 0)
		};
		short[] settlementBlockCore16 = new short[3] { 15, 35, 36 };
		sbyte[] organizationId16 = new sbyte[3] { 35, 37, 38 };
		List<short[]> developedBlockCore16 = new List<short[]>
		{
			new short[2] { 39, 33 },
			new short[2] { 40, 33 },
			new short[2] { 41, 33 },
			new short[2] { 42, 33 },
			new short[2] { 43, 33 },
			new short[2] { 44, 33 },
			new short[2] { 45, 33 },
			new short[2] { 46, 33 },
			new short[2] { 47, 33 },
			new short[2] { 48, 33 },
			new short[2] { 49, 33 },
			new short[2] { 50, 33 },
			new short[2] { 51, 33 },
			new short[2] { 52, 33 },
			new short[2] { 53, 33 },
			new short[2] { 54, 33 },
			new short[2] { 55, 33 },
			new short[2] { 56, 33 },
			new short[2] { 109, 10 },
			new short[2] { 110, 10 },
			new short[2] { 111, 10 }
		};
		List<short[]> normalBlockCore16 = new List<short[]>
		{
			new short[2] { 57, 33 },
			new short[2] { 58, 33 },
			new short[2] { 59, 33 },
			new short[2] { 63, 33 },
			new short[2] { 64, 33 },
			new short[2] { 65, 33 },
			new short[2] { 69, 33 },
			new short[2] { 70, 33 },
			new short[2] { 71, 33 },
			new short[2] { 75, 33 },
			new short[2] { 76, 33 },
			new short[2] { 77, 33 },
			new short[2] { 81, 33 },
			new short[2] { 82, 33 },
			new short[2] { 83, 33 },
			new short[2] { 87, 33 },
			new short[2] { 88, 33 },
			new short[2] { 89, 33 },
			new short[2] { 112, 30 },
			new short[2] { 113, 30 },
			new short[2] { 114, 30 },
			new short[2] { 124, 30 }
		};
		List<short[]> wildBlockCore16 = new List<short[]>
		{
			new short[2] { 93, 33 },
			new short[2] { 94, 33 },
			new short[2] { 95, 33 },
			new short[2] { 96, 33 },
			new short[2] { 97, 33 },
			new short[2] { 98, 33 },
			new short[2] { 99, 33 },
			new short[2] { 100, 33 },
			new short[2] { 101, 33 },
			new short[2] { 102, 33 },
			new short[2] { 103, 33 },
			new short[2] { 104, 33 },
			new short[2] { 105, 33 },
			new short[2] { 106, 33 },
			new short[2] { 107, 33 },
			new short[2] { 108, 33 },
			new short[2] { 115, 40 },
			new short[2] { 116, 40 },
			new short[2] { 117, 40 },
			new short[2] { 124, 30 }
		};
		List<short[]> bigBaseBlockCore16 = new List<short[]>
		{
			new short[2] { 72, 1 },
			new short[2] { 60, 1 },
			new short[2] { 78, 1 },
			new short[2] { 84, 1 },
			new short[2] { 86, 1 },
			new short[2] { 66, 1 },
			new short[2] { 67, 1 },
			new short[2] { 68, 1 },
			new short[2] { 91, 1 },
			new short[2] { 92, 1 }
		};
		list3 = new List<short[]>();
		List<short[]> seriesBlockCore16 = list3;
		list3 = new List<short[]>();
		List<short[]> encircleBlockCore16 = list3;
		short[] sceneryBlockCore16 = new short[0];
		list4 = new List<short>();
		List<short> lovingItemSubTypes16 = list4;
		list4 = new List<short>();
		dataArray16.Add(new MapAreaItem(15, 15, 80, 81, 30, worldMapPos16, neighborAreas16, "largemap_part_1_chengzhen_14", miniMapIcons16, "MapCityYangzhou", miniMapPos16, 0, 2, 9, enemyNests16, enemyNestCreationDateRanges16, settlementBlockCore16, organizationId16, -1, 0, null, developedBlockCore16, normalBlockCore16, wildBlockCore16, bigBaseBlockCore16, seriesBlockCore16, encircleBlockCore16, sceneryBlockCore16, 82, 83, null, null, EMapAreaAreaDirection.South, lovingItemSubTypes16, list4, showDarkAshStatus: true));
		List<MapAreaItem> dataArray17 = _dataArray;
		sbyte[] worldMapPos17 = new sbyte[2] { 13, 14 };
		AreaTravelRoute[] neighborAreas17 = new AreaTravelRoute[2]
		{
			new AreaTravelRoute(31, 6, null),
			new AreaTravelRoute(37, 12, new int[2] { 13, 15 })
		};
		string[] miniMapIcons17 = new string[4] { "largemap_part_2_icon_hei_20", "largemap_part_2_icon_jin_20", "largemap_part_2_icon_lan_20", "largemap_part_2_icon_hui_20" };
		short[] miniMapPos17 = new short[2] { 150, 60 };
		List<EnemyNestCreationInfo[]> enemyNests17 = new List<EnemyNestCreationInfo[]>
		{
			new EnemyNestCreationInfo[2]
			{
				new EnemyNestCreationInfo(2, 3),
				new EnemyNestCreationInfo(3, 3)
			},
			new EnemyNestCreationInfo[2]
			{
				new EnemyNestCreationInfo(4, 4),
				new EnemyNestCreationInfo(5, 4)
			},
			new EnemyNestCreationInfo[2]
			{
				new EnemyNestCreationInfo(6, 5),
				new EnemyNestCreationInfo(7, 5)
			},
			new EnemyNestCreationInfo[8]
			{
				new EnemyNestCreationInfo(8, 12),
				new EnemyNestCreationInfo(9, 12),
				new EnemyNestCreationInfo(8, 12),
				new EnemyNestCreationInfo(9, 12),
				new EnemyNestCreationInfo(8, 12),
				new EnemyNestCreationInfo(9, 12),
				new EnemyNestCreationInfo(10, 7),
				new EnemyNestCreationInfo(11, 7)
			},
			new EnemyNestCreationInfo[1]
			{
				new EnemyNestCreationInfo(13, 4)
			}
		};
		List<IntPair> enemyNestCreationDateRanges17 = new List<IntPair>
		{
			new IntPair(0, 6),
			new IntPair(6, 12),
			new IntPair(12, 24),
			new IntPair(24, 36),
			new IntPair(0, 0)
		};
		short[] settlementBlockCore17 = new short[3] { 19, 35, 36 };
		sbyte[] organizationId17 = new sbyte[3] { 1, 37, 38 };
		List<short[]> developedBlockCore17 = new List<short[]>
		{
			new short[2] { 39, 33 },
			new short[2] { 40, 33 },
			new short[2] { 41, 33 },
			new short[2] { 42, 33 },
			new short[2] { 43, 33 },
			new short[2] { 44, 33 },
			new short[2] { 45, 33 },
			new short[2] { 46, 33 },
			new short[2] { 47, 33 },
			new short[2] { 48, 33 },
			new short[2] { 49, 33 },
			new short[2] { 50, 33 },
			new short[2] { 51, 33 },
			new short[2] { 52, 33 },
			new short[2] { 53, 33 },
			new short[2] { 54, 33 },
			new short[2] { 55, 33 },
			new short[2] { 56, 33 },
			new short[2] { 109, 10 },
			new short[2] { 110, 10 },
			new short[2] { 111, 10 }
		};
		List<short[]> normalBlockCore17 = new List<short[]>
		{
			new short[2] { 57, 33 },
			new short[2] { 58, 33 },
			new short[2] { 59, 33 },
			new short[2] { 63, 33 },
			new short[2] { 64, 33 },
			new short[2] { 65, 33 },
			new short[2] { 69, 33 },
			new short[2] { 70, 33 },
			new short[2] { 71, 33 },
			new short[2] { 75, 33 },
			new short[2] { 76, 33 },
			new short[2] { 77, 33 },
			new short[2] { 81, 33 },
			new short[2] { 82, 33 },
			new short[2] { 83, 33 },
			new short[2] { 87, 33 },
			new short[2] { 88, 33 },
			new short[2] { 89, 33 },
			new short[2] { 112, 30 },
			new short[2] { 113, 30 },
			new short[2] { 114, 30 },
			new short[2] { 124, 30 }
		};
		List<short[]> wildBlockCore17 = new List<short[]>
		{
			new short[2] { 93, 33 },
			new short[2] { 94, 33 },
			new short[2] { 95, 33 },
			new short[2] { 96, 33 },
			new short[2] { 97, 33 },
			new short[2] { 98, 33 },
			new short[2] { 99, 33 },
			new short[2] { 100, 33 },
			new short[2] { 101, 33 },
			new short[2] { 102, 33 },
			new short[2] { 103, 33 },
			new short[2] { 104, 33 },
			new short[2] { 105, 33 },
			new short[2] { 106, 33 },
			new short[2] { 107, 33 },
			new short[2] { 108, 33 },
			new short[2] { 115, 40 },
			new short[2] { 116, 40 },
			new short[2] { 117, 40 },
			new short[2] { 124, 30 }
		};
		List<short[]> bigBaseBlockCore17 = new List<short[]>
		{
			new short[2] { 72, 1 },
			new short[2] { 60, 1 },
			new short[2] { 61, 1 },
			new short[2] { 62, 1 },
			new short[2] { 79, 1 },
			new short[2] { 80, 1 },
			new short[2] { 84, 1 },
			new short[2] { 67, 1 },
			new short[2] { 68, 1 },
			new short[2] { 90, 1 }
		};
		list3 = new List<short[]>();
		List<short[]> seriesBlockCore17 = list3;
		list3 = new List<short[]>();
		List<short[]> encircleBlockCore17 = list3;
		short[] sceneryBlockCore17 = new short[0];
		List<short> lovingItemSubTypes17 = new List<short> { 900 };
		list4 = new List<short>();
		dataArray17.Add(new MapAreaItem(16, 1, 84, 85, 30, worldMapPos17, neighborAreas17, "largemap_part_1_menpai_7", miniMapIcons17, "MapGangShaolin", miniMapPos17, 0, 1, 7, enemyNests17, enemyNestCreationDateRanges17, settlementBlockCore17, organizationId17, -1, 1, null, developedBlockCore17, normalBlockCore17, wildBlockCore17, bigBaseBlockCore17, seriesBlockCore17, encircleBlockCore17, sceneryBlockCore17, 86, 87, null, null, EMapAreaAreaDirection.South, lovingItemSubTypes17, list4, showDarkAshStatus: true));
		List<MapAreaItem> dataArray18 = _dataArray;
		sbyte[] worldMapPos18 = new sbyte[2] { 3, 8 };
		AreaTravelRoute[] neighborAreas18 = new AreaTravelRoute[2]
		{
			new AreaTravelRoute(44, 14, new int[2] { 4, 8 }),
			new AreaTravelRoute(114, 44, new int[8] { 4, 8, 4, 7, 4, 6, 4, 5 })
		};
		string[] miniMapIcons18 = new string[4] { "largemap_part_2_icon_hei_27", "largemap_part_2_icon_jin_27", "largemap_part_2_icon_lan_27", "largemap_part_2_icon_hui_27" };
		short[] miniMapPos18 = new short[2] { 90, 130 };
		List<EnemyNestCreationInfo[]> enemyNests18 = new List<EnemyNestCreationInfo[]>
		{
			new EnemyNestCreationInfo[2]
			{
				new EnemyNestCreationInfo(2, 3),
				new EnemyNestCreationInfo(3, 3)
			},
			new EnemyNestCreationInfo[2]
			{
				new EnemyNestCreationInfo(4, 4),
				new EnemyNestCreationInfo(5, 4)
			},
			new EnemyNestCreationInfo[2]
			{
				new EnemyNestCreationInfo(6, 5),
				new EnemyNestCreationInfo(7, 5)
			},
			new EnemyNestCreationInfo[8]
			{
				new EnemyNestCreationInfo(8, 12),
				new EnemyNestCreationInfo(9, 12),
				new EnemyNestCreationInfo(8, 12),
				new EnemyNestCreationInfo(9, 12),
				new EnemyNestCreationInfo(8, 12),
				new EnemyNestCreationInfo(9, 12),
				new EnemyNestCreationInfo(10, 7),
				new EnemyNestCreationInfo(11, 7)
			},
			new EnemyNestCreationInfo[1]
			{
				new EnemyNestCreationInfo(13, 4)
			}
		};
		List<IntPair> enemyNestCreationDateRanges18 = new List<IntPair>
		{
			new IntPair(0, 6),
			new IntPair(6, 12),
			new IntPair(12, 24),
			new IntPair(24, 36),
			new IntPair(0, 0)
		};
		short[] settlementBlockCore18 = new short[3] { 20, 34, 36 };
		sbyte[] organizationId18 = new sbyte[3] { 2, 36, 38 };
		List<short[]> developedBlockCore18 = new List<short[]>
		{
			new short[2] { 39, 33 },
			new short[2] { 40, 33 },
			new short[2] { 41, 33 },
			new short[2] { 42, 33 },
			new short[2] { 43, 33 },
			new short[2] { 44, 33 },
			new short[2] { 45, 33 },
			new short[2] { 46, 33 },
			new short[2] { 47, 33 },
			new short[2] { 48, 33 },
			new short[2] { 49, 33 },
			new short[2] { 50, 33 },
			new short[2] { 51, 33 },
			new short[2] { 52, 33 },
			new short[2] { 53, 33 },
			new short[2] { 54, 33 },
			new short[2] { 55, 33 },
			new short[2] { 56, 33 },
			new short[2] { 109, 10 },
			new short[2] { 110, 10 },
			new short[2] { 111, 10 }
		};
		List<short[]> normalBlockCore18 = new List<short[]>
		{
			new short[2] { 57, 33 },
			new short[2] { 58, 33 },
			new short[2] { 59, 33 },
			new short[2] { 63, 33 },
			new short[2] { 64, 33 },
			new short[2] { 65, 33 },
			new short[2] { 69, 33 },
			new short[2] { 70, 33 },
			new short[2] { 71, 33 },
			new short[2] { 75, 33 },
			new short[2] { 76, 33 },
			new short[2] { 77, 33 },
			new short[2] { 81, 33 },
			new short[2] { 82, 33 },
			new short[2] { 83, 33 },
			new short[2] { 87, 33 },
			new short[2] { 88, 33 },
			new short[2] { 89, 33 },
			new short[2] { 112, 30 },
			new short[2] { 113, 30 },
			new short[2] { 114, 30 },
			new short[2] { 124, 30 }
		};
		List<short[]> wildBlockCore18 = new List<short[]>
		{
			new short[2] { 93, 33 },
			new short[2] { 94, 33 },
			new short[2] { 95, 33 },
			new short[2] { 96, 33 },
			new short[2] { 97, 33 },
			new short[2] { 98, 33 },
			new short[2] { 99, 33 },
			new short[2] { 100, 33 },
			new short[2] { 101, 33 },
			new short[2] { 102, 33 },
			new short[2] { 103, 33 },
			new short[2] { 104, 33 },
			new short[2] { 105, 33 },
			new short[2] { 106, 33 },
			new short[2] { 107, 33 },
			new short[2] { 108, 33 },
			new short[2] { 115, 40 },
			new short[2] { 116, 40 },
			new short[2] { 117, 40 },
			new short[2] { 124, 30 }
		};
		List<short[]> bigBaseBlockCore18 = new List<short[]>
		{
			new short[2] { 72, 1 },
			new short[2] { 60, 1 },
			new short[2] { 61, 1 },
			new short[2] { 62, 1 },
			new short[2] { 78, 1 },
			new short[2] { 84, 1 },
			new short[2] { 86, 1 },
			new short[2] { 67, 1 },
			new short[2] { 68, 1 },
			new short[2] { 90, 1 }
		};
		list3 = new List<short[]>();
		List<short[]> seriesBlockCore18 = list3;
		list3 = new List<short[]>();
		List<short[]> encircleBlockCore18 = list3;
		short[] sceneryBlockCore18 = new short[0];
		List<short> lovingItemSubTypes18 = new List<short> { 1 };
		list4 = new List<short>();
		dataArray18.Add(new MapAreaItem(17, 2, 88, 89, 30, worldMapPos18, neighborAreas18, "largemap_part_1_menpai_1", miniMapIcons18, "MapGangEmei", miniMapPos18, 0, 1, 7, enemyNests18, enemyNestCreationDateRanges18, settlementBlockCore18, organizationId18, -1, 2, null, developedBlockCore18, normalBlockCore18, wildBlockCore18, bigBaseBlockCore18, seriesBlockCore18, encircleBlockCore18, sceneryBlockCore18, 90, 91, null, null, EMapAreaAreaDirection.South, lovingItemSubTypes18, list4, showDarkAshStatus: true));
		List<MapAreaItem> dataArray19 = _dataArray;
		sbyte[] worldMapPos19 = new sbyte[2] { 11, 3 };
		AreaTravelRoute[] neighborAreas19 = new AreaTravelRoute[0];
		string[] miniMapIcons19 = new string[4] { "largemap_part_2_icon_hei_31", "largemap_part_2_icon_jin_31", "largemap_part_2_icon_lan_31", "largemap_part_2_icon_hui_31" };
		short[] miniMapPos19 = new short[2] { 260, 210 };
		List<EnemyNestCreationInfo[]> enemyNests19 = new List<EnemyNestCreationInfo[]>
		{
			new EnemyNestCreationInfo[2]
			{
				new EnemyNestCreationInfo(2, 3),
				new EnemyNestCreationInfo(3, 3)
			},
			new EnemyNestCreationInfo[2]
			{
				new EnemyNestCreationInfo(4, 4),
				new EnemyNestCreationInfo(5, 4)
			},
			new EnemyNestCreationInfo[2]
			{
				new EnemyNestCreationInfo(6, 5),
				new EnemyNestCreationInfo(7, 5)
			},
			new EnemyNestCreationInfo[8]
			{
				new EnemyNestCreationInfo(8, 12),
				new EnemyNestCreationInfo(9, 12),
				new EnemyNestCreationInfo(8, 12),
				new EnemyNestCreationInfo(9, 12),
				new EnemyNestCreationInfo(8, 12),
				new EnemyNestCreationInfo(9, 12),
				new EnemyNestCreationInfo(10, 7),
				new EnemyNestCreationInfo(11, 7)
			},
			new EnemyNestCreationInfo[1]
			{
				new EnemyNestCreationInfo(13, 4)
			}
		};
		List<IntPair> enemyNestCreationDateRanges19 = new List<IntPair>
		{
			new IntPair(0, 6),
			new IntPair(6, 12),
			new IntPair(12, 24),
			new IntPair(24, 36),
			new IntPair(0, 0)
		};
		short[] settlementBlockCore19 = new short[3] { 21, 34, 36 };
		sbyte[] organizationId19 = new sbyte[3] { 3, 36, 38 };
		List<short[]> developedBlockCore19 = new List<short[]>
		{
			new short[2] { 39, 33 },
			new short[2] { 40, 33 },
			new short[2] { 41, 33 },
			new short[2] { 42, 33 },
			new short[2] { 43, 33 },
			new short[2] { 44, 33 },
			new short[2] { 45, 33 },
			new short[2] { 46, 33 },
			new short[2] { 47, 33 },
			new short[2] { 48, 33 },
			new short[2] { 49, 33 },
			new short[2] { 50, 33 },
			new short[2] { 51, 33 },
			new short[2] { 52, 33 },
			new short[2] { 53, 33 },
			new short[2] { 54, 33 },
			new short[2] { 55, 33 },
			new short[2] { 56, 33 },
			new short[2] { 109, 10 },
			new short[2] { 110, 10 },
			new short[2] { 111, 10 }
		};
		List<short[]> normalBlockCore19 = new List<short[]>
		{
			new short[2] { 57, 33 },
			new short[2] { 58, 33 },
			new short[2] { 59, 33 },
			new short[2] { 63, 33 },
			new short[2] { 64, 33 },
			new short[2] { 65, 33 },
			new short[2] { 69, 33 },
			new short[2] { 70, 33 },
			new short[2] { 71, 33 },
			new short[2] { 75, 33 },
			new short[2] { 76, 33 },
			new short[2] { 77, 33 },
			new short[2] { 81, 33 },
			new short[2] { 82, 33 },
			new short[2] { 83, 33 },
			new short[2] { 87, 33 },
			new short[2] { 88, 33 },
			new short[2] { 89, 33 },
			new short[2] { 112, 30 },
			new short[2] { 113, 30 },
			new short[2] { 114, 30 },
			new short[2] { 124, 30 }
		};
		List<short[]> wildBlockCore19 = new List<short[]>
		{
			new short[2] { 93, 33 },
			new short[2] { 94, 33 },
			new short[2] { 95, 33 },
			new short[2] { 96, 33 },
			new short[2] { 97, 33 },
			new short[2] { 98, 33 },
			new short[2] { 99, 33 },
			new short[2] { 100, 33 },
			new short[2] { 101, 33 },
			new short[2] { 102, 33 },
			new short[2] { 103, 33 },
			new short[2] { 104, 33 },
			new short[2] { 105, 33 },
			new short[2] { 106, 33 },
			new short[2] { 107, 33 },
			new short[2] { 108, 33 },
			new short[2] { 115, 40 },
			new short[2] { 116, 40 },
			new short[2] { 117, 40 },
			new short[2] { 124, 30 }
		};
		List<short[]> bigBaseBlockCore19 = new List<short[]>
		{
			new short[2] { 72, 1 },
			new short[2] { 74, 1 },
			new short[2] { 60, 1 },
			new short[2] { 61, 1 },
			new short[2] { 62, 1 },
			new short[2] { 78, 1 },
			new short[2] { 84, 1 },
			new short[2] { 66, 1 },
			new short[2] { 91, 1 },
			new short[2] { 92, 1 }
		};
		list3 = new List<short[]>();
		List<short[]> seriesBlockCore19 = list3;
		list3 = new List<short[]>();
		List<short[]> encircleBlockCore19 = list3;
		short[] sceneryBlockCore19 = new short[0];
		List<short> lovingItemSubTypes19 = new List<short> { 0, 3 };
		list4 = new List<short>();
		dataArray19.Add(new MapAreaItem(18, 3, 92, 93, 30, worldMapPos19, neighborAreas19, "largemap_part_1_menpai_0", miniMapIcons19, "MapGangBaihua", miniMapPos19, 0, 1, 7, enemyNests19, enemyNestCreationDateRanges19, settlementBlockCore19, organizationId19, -1, 2, null, developedBlockCore19, normalBlockCore19, wildBlockCore19, bigBaseBlockCore19, seriesBlockCore19, encircleBlockCore19, sceneryBlockCore19, 94, 95, null, null, EMapAreaAreaDirection.South, lovingItemSubTypes19, list4, showDarkAshStatus: true));
		List<MapAreaItem> dataArray20 = _dataArray;
		sbyte[] worldMapPos20 = new sbyte[2] { 11, 12 };
		AreaTravelRoute[] neighborAreas20 = new AreaTravelRoute[2]
		{
			new AreaTravelRoute(53, 6, null),
			new AreaTravelRoute(56, 14, new int[2] { 11, 13 })
		};
		string[] miniMapIcons20 = new string[4] { "largemap_part_2_icon_hei_23", "largemap_part_2_icon_jin_23", "largemap_part_2_icon_lan_23", "largemap_part_2_icon_hui_23" };
		short[] miniMapPos20 = new short[2] { 140, 200 };
		List<EnemyNestCreationInfo[]> enemyNests20 = new List<EnemyNestCreationInfo[]>
		{
			new EnemyNestCreationInfo[2]
			{
				new EnemyNestCreationInfo(2, 3),
				new EnemyNestCreationInfo(3, 3)
			},
			new EnemyNestCreationInfo[2]
			{
				new EnemyNestCreationInfo(4, 4),
				new EnemyNestCreationInfo(5, 4)
			},
			new EnemyNestCreationInfo[2]
			{
				new EnemyNestCreationInfo(6, 5),
				new EnemyNestCreationInfo(7, 5)
			},
			new EnemyNestCreationInfo[8]
			{
				new EnemyNestCreationInfo(8, 12),
				new EnemyNestCreationInfo(9, 12),
				new EnemyNestCreationInfo(8, 12),
				new EnemyNestCreationInfo(9, 12),
				new EnemyNestCreationInfo(8, 12),
				new EnemyNestCreationInfo(9, 12),
				new EnemyNestCreationInfo(10, 7),
				new EnemyNestCreationInfo(11, 7)
			},
			new EnemyNestCreationInfo[1]
			{
				new EnemyNestCreationInfo(13, 4)
			}
		};
		List<IntPair> enemyNestCreationDateRanges20 = new List<IntPair>
		{
			new IntPair(0, 6),
			new IntPair(6, 12),
			new IntPair(12, 24),
			new IntPair(24, 36),
			new IntPair(0, 0)
		};
		short[] settlementBlockCore20 = new short[3] { 22, 34, 35 };
		sbyte[] organizationId20 = new sbyte[3] { 4, 36, 37 };
		List<short[]> developedBlockCore20 = new List<short[]>
		{
			new short[2] { 39, 33 },
			new short[2] { 40, 33 },
			new short[2] { 41, 33 },
			new short[2] { 42, 33 },
			new short[2] { 43, 33 },
			new short[2] { 44, 33 },
			new short[2] { 45, 33 },
			new short[2] { 46, 33 },
			new short[2] { 47, 33 },
			new short[2] { 48, 33 },
			new short[2] { 49, 33 },
			new short[2] { 50, 33 },
			new short[2] { 51, 33 },
			new short[2] { 52, 33 },
			new short[2] { 53, 33 },
			new short[2] { 54, 33 },
			new short[2] { 55, 33 },
			new short[2] { 56, 33 },
			new short[2] { 109, 10 },
			new short[2] { 110, 10 },
			new short[2] { 111, 10 }
		};
		List<short[]> normalBlockCore20 = new List<short[]>
		{
			new short[2] { 57, 33 },
			new short[2] { 58, 33 },
			new short[2] { 59, 33 },
			new short[2] { 63, 33 },
			new short[2] { 64, 33 },
			new short[2] { 65, 33 },
			new short[2] { 69, 33 },
			new short[2] { 70, 33 },
			new short[2] { 71, 33 },
			new short[2] { 75, 33 },
			new short[2] { 76, 33 },
			new short[2] { 77, 33 },
			new short[2] { 81, 33 },
			new short[2] { 82, 33 },
			new short[2] { 83, 33 },
			new short[2] { 87, 33 },
			new short[2] { 88, 33 },
			new short[2] { 89, 33 },
			new short[2] { 112, 30 },
			new short[2] { 113, 30 },
			new short[2] { 114, 30 },
			new short[2] { 124, 30 }
		};
		List<short[]> wildBlockCore20 = new List<short[]>
		{
			new short[2] { 93, 33 },
			new short[2] { 94, 33 },
			new short[2] { 95, 33 },
			new short[2] { 96, 33 },
			new short[2] { 97, 33 },
			new short[2] { 98, 33 },
			new short[2] { 99, 33 },
			new short[2] { 100, 33 },
			new short[2] { 101, 33 },
			new short[2] { 102, 33 },
			new short[2] { 103, 33 },
			new short[2] { 104, 33 },
			new short[2] { 105, 33 },
			new short[2] { 106, 33 },
			new short[2] { 107, 33 },
			new short[2] { 108, 33 },
			new short[2] { 115, 40 },
			new short[2] { 116, 40 },
			new short[2] { 117, 40 },
			new short[2] { 124, 30 }
		};
		List<short[]> bigBaseBlockCore20 = new List<short[]>
		{
			new short[2] { 72, 1 },
			new short[2] { 60, 1 },
			new short[2] { 78, 1 },
			new short[2] { 79, 1 },
			new short[2] { 80, 1 },
			new short[2] { 85, 1 },
			new short[2] { 86, 1 },
			new short[2] { 66, 1 },
			new short[2] { 91, 1 },
			new short[2] { 92, 1 }
		};
		list3 = new List<short[]>();
		List<short[]> seriesBlockCore20 = list3;
		list3 = new List<short[]>();
		List<short[]> encircleBlockCore20 = list3;
		short[] sceneryBlockCore20 = new short[0];
		List<short> lovingItemSubTypes20 = new List<short> { 6 };
		list4 = new List<short>();
		dataArray20.Add(new MapAreaItem(19, 4, 96, 97, 30, worldMapPos20, neighborAreas20, "largemap_part_1_menpai_10", miniMapIcons20, "MapGangWudang", miniMapPos20, 0, 1, 7, enemyNests20, enemyNestCreationDateRanges20, settlementBlockCore20, organizationId20, -1, 2, null, developedBlockCore20, normalBlockCore20, wildBlockCore20, bigBaseBlockCore20, seriesBlockCore20, encircleBlockCore20, sceneryBlockCore20, 98, 99, null, null, EMapAreaAreaDirection.South, lovingItemSubTypes20, list4, showDarkAshStatus: true));
		List<MapAreaItem> dataArray21 = _dataArray;
		sbyte[] worldMapPos21 = new sbyte[2] { 11, 18 };
		AreaTravelRoute[] neighborAreas21 = new AreaTravelRoute[2]
		{
			new AreaTravelRoute(64, 6, null),
			new AreaTravelRoute(65, 14, new int[2] { 10, 18 })
		};
		string[] miniMapIcons21 = new string[4] { "largemap_part_2_icon_hei_21", "largemap_part_2_icon_jin_21", "largemap_part_2_icon_lan_21", "largemap_part_2_icon_hui_21" };
		short[] miniMapPos21 = new short[2] { 160, 190 };
		List<EnemyNestCreationInfo[]> enemyNests21 = new List<EnemyNestCreationInfo[]>
		{
			new EnemyNestCreationInfo[2]
			{
				new EnemyNestCreationInfo(2, 3),
				new EnemyNestCreationInfo(3, 3)
			},
			new EnemyNestCreationInfo[2]
			{
				new EnemyNestCreationInfo(4, 4),
				new EnemyNestCreationInfo(5, 4)
			},
			new EnemyNestCreationInfo[2]
			{
				new EnemyNestCreationInfo(6, 5),
				new EnemyNestCreationInfo(7, 5)
			},
			new EnemyNestCreationInfo[8]
			{
				new EnemyNestCreationInfo(8, 12),
				new EnemyNestCreationInfo(9, 12),
				new EnemyNestCreationInfo(8, 12),
				new EnemyNestCreationInfo(9, 12),
				new EnemyNestCreationInfo(8, 12),
				new EnemyNestCreationInfo(9, 12),
				new EnemyNestCreationInfo(10, 7),
				new EnemyNestCreationInfo(11, 7)
			},
			new EnemyNestCreationInfo[1]
			{
				new EnemyNestCreationInfo(13, 4)
			}
		};
		List<IntPair> enemyNestCreationDateRanges21 = new List<IntPair>
		{
			new IntPair(0, 6),
			new IntPair(6, 12),
			new IntPair(12, 24),
			new IntPair(24, 36),
			new IntPair(0, 0)
		};
		short[] settlementBlockCore21 = new short[3] { 23, 34, 36 };
		sbyte[] organizationId21 = new sbyte[3] { 5, 36, 38 };
		List<short[]> developedBlockCore21 = new List<short[]>
		{
			new short[2] { 39, 33 },
			new short[2] { 40, 33 },
			new short[2] { 41, 33 },
			new short[2] { 42, 33 },
			new short[2] { 43, 33 },
			new short[2] { 44, 33 },
			new short[2] { 45, 33 },
			new short[2] { 46, 33 },
			new short[2] { 47, 33 },
			new short[2] { 48, 33 },
			new short[2] { 49, 33 },
			new short[2] { 50, 33 },
			new short[2] { 51, 33 },
			new short[2] { 52, 33 },
			new short[2] { 53, 33 },
			new short[2] { 54, 33 },
			new short[2] { 55, 33 },
			new short[2] { 56, 33 },
			new short[2] { 109, 10 },
			new short[2] { 110, 10 },
			new short[2] { 111, 10 }
		};
		List<short[]> normalBlockCore21 = new List<short[]>
		{
			new short[2] { 57, 33 },
			new short[2] { 58, 33 },
			new short[2] { 59, 33 },
			new short[2] { 63, 33 },
			new short[2] { 64, 33 },
			new short[2] { 65, 33 },
			new short[2] { 69, 33 },
			new short[2] { 70, 33 },
			new short[2] { 71, 33 },
			new short[2] { 75, 33 },
			new short[2] { 76, 33 },
			new short[2] { 77, 33 },
			new short[2] { 81, 33 },
			new short[2] { 82, 33 },
			new short[2] { 83, 33 },
			new short[2] { 87, 33 },
			new short[2] { 88, 33 },
			new short[2] { 89, 33 },
			new short[2] { 112, 30 },
			new short[2] { 113, 30 },
			new short[2] { 114, 30 },
			new short[2] { 124, 30 }
		};
		List<short[]> wildBlockCore21 = new List<short[]>
		{
			new short[2] { 93, 33 },
			new short[2] { 94, 33 },
			new short[2] { 95, 33 },
			new short[2] { 96, 33 },
			new short[2] { 97, 33 },
			new short[2] { 98, 33 },
			new short[2] { 99, 33 },
			new short[2] { 100, 33 },
			new short[2] { 101, 33 },
			new short[2] { 102, 33 },
			new short[2] { 103, 33 },
			new short[2] { 104, 33 },
			new short[2] { 105, 33 },
			new short[2] { 106, 33 },
			new short[2] { 107, 33 },
			new short[2] { 108, 33 },
			new short[2] { 115, 40 },
			new short[2] { 116, 40 },
			new short[2] { 117, 40 },
			new short[2] { 124, 30 }
		};
		List<short[]> bigBaseBlockCore21 = new List<short[]>
		{
			new short[2] { 72, 1 },
			new short[2] { 73, 1 },
			new short[2] { 74, 1 },
			new short[2] { 61, 1 },
			new short[2] { 62, 1 },
			new short[2] { 79, 1 },
			new short[2] { 80, 1 },
			new short[2] { 84, 1 },
			new short[2] { 66, 1 },
			new short[2] { 90, 1 }
		};
		list3 = new List<short[]>();
		List<short[]> seriesBlockCore21 = list3;
		list3 = new List<short[]>();
		List<short[]> encircleBlockCore21 = list3;
		short[] sceneryBlockCore21 = new short[0];
		List<short> lovingItemSubTypes21 = new List<short> { 8, 9 };
		list4 = new List<short>();
		dataArray21.Add(new MapAreaItem(20, 5, 100, 101, 30, worldMapPos21, neighborAreas21, "largemap_part_1_menpai_13", miniMapIcons21, "MapGangYuanshan", miniMapPos21, 0, 1, 7, enemyNests21, enemyNestCreationDateRanges21, settlementBlockCore21, organizationId21, -1, 2, null, developedBlockCore21, normalBlockCore21, wildBlockCore21, bigBaseBlockCore21, seriesBlockCore21, encircleBlockCore21, sceneryBlockCore21, 102, 103, null, null, EMapAreaAreaDirection.South, lovingItemSubTypes21, list4, showDarkAshStatus: true));
		List<MapAreaItem> dataArray22 = _dataArray;
		sbyte[] worldMapPos22 = new sbyte[2] { 14, 1 };
		AreaTravelRoute[] neighborAreas22 = new AreaTravelRoute[0];
		string[] miniMapIcons22 = new string[4] { "largemap_part_2_icon_hei_32", "largemap_part_2_icon_jin_32", "largemap_part_2_icon_lan_32", "largemap_part_2_icon_hui_32" };
		short[] miniMapPos22 = new short[2] { 140, 160 };
		List<EnemyNestCreationInfo[]> enemyNests22 = new List<EnemyNestCreationInfo[]>
		{
			new EnemyNestCreationInfo[2]
			{
				new EnemyNestCreationInfo(2, 3),
				new EnemyNestCreationInfo(3, 3)
			},
			new EnemyNestCreationInfo[2]
			{
				new EnemyNestCreationInfo(4, 4),
				new EnemyNestCreationInfo(5, 4)
			},
			new EnemyNestCreationInfo[2]
			{
				new EnemyNestCreationInfo(6, 5),
				new EnemyNestCreationInfo(7, 5)
			},
			new EnemyNestCreationInfo[8]
			{
				new EnemyNestCreationInfo(8, 12),
				new EnemyNestCreationInfo(9, 12),
				new EnemyNestCreationInfo(8, 12),
				new EnemyNestCreationInfo(9, 12),
				new EnemyNestCreationInfo(8, 12),
				new EnemyNestCreationInfo(9, 12),
				new EnemyNestCreationInfo(10, 7),
				new EnemyNestCreationInfo(11, 7)
			},
			new EnemyNestCreationInfo[1]
			{
				new EnemyNestCreationInfo(13, 4)
			}
		};
		List<IntPair> enemyNestCreationDateRanges22 = new List<IntPair>
		{
			new IntPair(0, 6),
			new IntPair(6, 12),
			new IntPair(12, 24),
			new IntPair(24, 36),
			new IntPair(0, 0)
		};
		short[] settlementBlockCore22 = new short[3] { 24, 35, 36 };
		sbyte[] organizationId22 = new sbyte[3] { 6, 37, 38 };
		List<short[]> developedBlockCore22 = new List<short[]>
		{
			new short[2] { 39, 33 },
			new short[2] { 40, 33 },
			new short[2] { 41, 33 },
			new short[2] { 42, 33 },
			new short[2] { 43, 33 },
			new short[2] { 44, 33 },
			new short[2] { 45, 33 },
			new short[2] { 46, 33 },
			new short[2] { 47, 33 },
			new short[2] { 48, 33 },
			new short[2] { 49, 33 },
			new short[2] { 50, 33 },
			new short[2] { 51, 33 },
			new short[2] { 52, 33 },
			new short[2] { 53, 33 },
			new short[2] { 54, 33 },
			new short[2] { 55, 33 },
			new short[2] { 56, 33 },
			new short[2] { 109, 10 },
			new short[2] { 110, 10 },
			new short[2] { 111, 10 }
		};
		List<short[]> normalBlockCore22 = new List<short[]>
		{
			new short[2] { 57, 33 },
			new short[2] { 58, 33 },
			new short[2] { 59, 33 },
			new short[2] { 63, 33 },
			new short[2] { 64, 33 },
			new short[2] { 65, 33 },
			new short[2] { 69, 33 },
			new short[2] { 70, 33 },
			new short[2] { 71, 33 },
			new short[2] { 75, 33 },
			new short[2] { 76, 33 },
			new short[2] { 77, 33 },
			new short[2] { 81, 33 },
			new short[2] { 82, 33 },
			new short[2] { 83, 33 },
			new short[2] { 87, 33 },
			new short[2] { 88, 33 },
			new short[2] { 89, 33 },
			new short[2] { 112, 30 },
			new short[2] { 113, 30 },
			new short[2] { 114, 30 },
			new short[2] { 124, 30 }
		};
		List<short[]> wildBlockCore22 = new List<short[]>
		{
			new short[2] { 93, 33 },
			new short[2] { 94, 33 },
			new short[2] { 95, 33 },
			new short[2] { 96, 33 },
			new short[2] { 97, 33 },
			new short[2] { 98, 33 },
			new short[2] { 99, 33 },
			new short[2] { 100, 33 },
			new short[2] { 101, 33 },
			new short[2] { 102, 33 },
			new short[2] { 103, 33 },
			new short[2] { 104, 33 },
			new short[2] { 105, 33 },
			new short[2] { 106, 33 },
			new short[2] { 107, 33 },
			new short[2] { 108, 33 },
			new short[2] { 115, 40 },
			new short[2] { 116, 40 },
			new short[2] { 117, 40 },
			new short[2] { 124, 30 }
		};
		List<short[]> bigBaseBlockCore22 = new List<short[]>
		{
			new short[2] { 72, 1 },
			new short[2] { 61, 1 },
			new short[2] { 62, 1 },
			new short[2] { 79, 1 },
			new short[2] { 80, 1 },
			new short[2] { 84, 1 },
			new short[2] { 66, 1 },
			new short[2] { 90, 1 },
			new short[2] { 91, 1 },
			new short[2] { 92, 1 }
		};
		list3 = new List<short[]>();
		List<short[]> seriesBlockCore22 = list3;
		list3 = new List<short[]>();
		List<short[]> encircleBlockCore22 = list3;
		short[] sceneryBlockCore22 = new short[0];
		List<short> lovingItemSubTypes22 = new List<short> { 10 };
		list4 = new List<short>();
		dataArray22.Add(new MapAreaItem(21, 6, 104, 105, 30, worldMapPos22, neighborAreas22, "largemap_part_1_menpai_8", miniMapIcons22, "MapGangShixiang", miniMapPos22, 0, 1, 7, enemyNests22, enemyNestCreationDateRanges22, settlementBlockCore22, organizationId22, -1, 1, null, developedBlockCore22, normalBlockCore22, wildBlockCore22, bigBaseBlockCore22, seriesBlockCore22, encircleBlockCore22, sceneryBlockCore22, 106, 107, null, null, EMapAreaAreaDirection.South, lovingItemSubTypes22, list4, showDarkAshStatus: true));
		List<MapAreaItem> dataArray23 = _dataArray;
		sbyte[] worldMapPos23 = new sbyte[2] { 20, 19 };
		AreaTravelRoute[] neighborAreas23 = new AreaTravelRoute[1]
		{
			new AreaTravelRoute(77, 12, new int[2] { 20, 20 })
		};
		string[] miniMapIcons23 = new string[4] { "largemap_part_2_icon_hei_19", "largemap_part_2_icon_jin_19", "largemap_part_2_icon_lan_19", "largemap_part_2_icon_hui_19" };
		short[] miniMapPos23 = new short[2] { 250, 200 };
		List<EnemyNestCreationInfo[]> enemyNests23 = new List<EnemyNestCreationInfo[]>
		{
			new EnemyNestCreationInfo[2]
			{
				new EnemyNestCreationInfo(2, 3),
				new EnemyNestCreationInfo(3, 3)
			},
			new EnemyNestCreationInfo[2]
			{
				new EnemyNestCreationInfo(4, 4),
				new EnemyNestCreationInfo(5, 4)
			},
			new EnemyNestCreationInfo[2]
			{
				new EnemyNestCreationInfo(6, 5),
				new EnemyNestCreationInfo(7, 5)
			},
			new EnemyNestCreationInfo[8]
			{
				new EnemyNestCreationInfo(8, 12),
				new EnemyNestCreationInfo(9, 12),
				new EnemyNestCreationInfo(8, 12),
				new EnemyNestCreationInfo(9, 12),
				new EnemyNestCreationInfo(8, 12),
				new EnemyNestCreationInfo(9, 12),
				new EnemyNestCreationInfo(10, 7),
				new EnemyNestCreationInfo(11, 7)
			},
			new EnemyNestCreationInfo[1]
			{
				new EnemyNestCreationInfo(13, 4)
			}
		};
		List<IntPair> enemyNestCreationDateRanges23 = new List<IntPair>
		{
			new IntPair(0, 6),
			new IntPair(6, 12),
			new IntPair(12, 24),
			new IntPair(24, 36),
			new IntPair(0, 0)
		};
		short[] settlementBlockCore23 = new short[3] { 25, 34, 36 };
		sbyte[] organizationId23 = new sbyte[3] { 7, 36, 38 };
		List<short[]> developedBlockCore23 = new List<short[]>
		{
			new short[2] { 39, 33 },
			new short[2] { 40, 33 },
			new short[2] { 41, 33 },
			new short[2] { 42, 33 },
			new short[2] { 43, 33 },
			new short[2] { 44, 33 },
			new short[2] { 45, 33 },
			new short[2] { 46, 33 },
			new short[2] { 47, 33 },
			new short[2] { 48, 33 },
			new short[2] { 49, 33 },
			new short[2] { 50, 33 },
			new short[2] { 51, 33 },
			new short[2] { 52, 33 },
			new short[2] { 53, 33 },
			new short[2] { 54, 33 },
			new short[2] { 55, 33 },
			new short[2] { 56, 33 },
			new short[2] { 109, 10 },
			new short[2] { 110, 10 },
			new short[2] { 111, 10 }
		};
		List<short[]> normalBlockCore23 = new List<short[]>
		{
			new short[2] { 57, 33 },
			new short[2] { 58, 33 },
			new short[2] { 59, 33 },
			new short[2] { 63, 33 },
			new short[2] { 64, 33 },
			new short[2] { 65, 33 },
			new short[2] { 69, 33 },
			new short[2] { 70, 33 },
			new short[2] { 71, 33 },
			new short[2] { 75, 33 },
			new short[2] { 76, 33 },
			new short[2] { 77, 33 },
			new short[2] { 81, 33 },
			new short[2] { 82, 33 },
			new short[2] { 83, 33 },
			new short[2] { 87, 33 },
			new short[2] { 88, 33 },
			new short[2] { 89, 33 },
			new short[2] { 112, 30 },
			new short[2] { 113, 30 },
			new short[2] { 114, 30 },
			new short[2] { 124, 30 }
		};
		List<short[]> wildBlockCore23 = new List<short[]>
		{
			new short[2] { 93, 33 },
			new short[2] { 94, 33 },
			new short[2] { 95, 33 },
			new short[2] { 96, 33 },
			new short[2] { 97, 33 },
			new short[2] { 98, 33 },
			new short[2] { 99, 33 },
			new short[2] { 100, 33 },
			new short[2] { 101, 33 },
			new short[2] { 102, 33 },
			new short[2] { 103, 33 },
			new short[2] { 104, 33 },
			new short[2] { 105, 33 },
			new short[2] { 106, 33 },
			new short[2] { 107, 33 },
			new short[2] { 108, 33 },
			new short[2] { 115, 40 },
			new short[2] { 116, 40 },
			new short[2] { 117, 40 },
			new short[2] { 124, 30 }
		};
		List<short[]> bigBaseBlockCore23 = new List<short[]>
		{
			new short[2] { 72, 1 },
			new short[2] { 73, 1 },
			new short[2] { 74, 1 },
			new short[2] { 61, 1 },
			new short[2] { 62, 1 },
			new short[2] { 79, 1 },
			new short[2] { 80, 1 },
			new short[2] { 84, 1 },
			new short[2] { 66, 1 },
			new short[2] { 90, 1 }
		};
		list3 = new List<short[]>();
		List<short[]> seriesBlockCore23 = list3;
		list3 = new List<short[]>();
		List<short[]> encircleBlockCore23 = list3;
		short[] sceneryBlockCore23 = new short[0];
		List<short> lovingItemSubTypes23 = new List<short> { 13 };
		list4 = new List<short>();
		dataArray23.Add(new MapAreaItem(22, 7, 108, 109, 30, worldMapPos23, neighborAreas23, "largemap_part_1_menpai_6", miniMapIcons23, "MapGangRanshan", miniMapPos23, 0, 1, 7, enemyNests23, enemyNestCreationDateRanges23, settlementBlockCore23, organizationId23, -1, 2, null, developedBlockCore23, normalBlockCore23, wildBlockCore23, bigBaseBlockCore23, seriesBlockCore23, encircleBlockCore23, sceneryBlockCore23, 110, 111, null, null, EMapAreaAreaDirection.South, lovingItemSubTypes23, list4, showDarkAshStatus: true));
		List<MapAreaItem> dataArray24 = _dataArray;
		sbyte[] worldMapPos24 = new sbyte[2] { 11, 10 };
		AreaTravelRoute[] neighborAreas24 = new AreaTravelRoute[1]
		{
			new AreaTravelRoute(80, 6, null)
		};
		string[] miniMapIcons24 = new string[4] { "largemap_part_2_icon_hei_28", "largemap_part_2_icon_jin_28", "largemap_part_2_icon_lan_28", "largemap_part_2_icon_hui_28" };
		short[] miniMapPos24 = new short[2] { 160, 270 };
		List<EnemyNestCreationInfo[]> enemyNests24 = new List<EnemyNestCreationInfo[]>
		{
			new EnemyNestCreationInfo[2]
			{
				new EnemyNestCreationInfo(2, 3),
				new EnemyNestCreationInfo(3, 3)
			},
			new EnemyNestCreationInfo[2]
			{
				new EnemyNestCreationInfo(4, 4),
				new EnemyNestCreationInfo(5, 4)
			},
			new EnemyNestCreationInfo[2]
			{
				new EnemyNestCreationInfo(6, 5),
				new EnemyNestCreationInfo(7, 5)
			},
			new EnemyNestCreationInfo[8]
			{
				new EnemyNestCreationInfo(8, 12),
				new EnemyNestCreationInfo(9, 12),
				new EnemyNestCreationInfo(8, 12),
				new EnemyNestCreationInfo(9, 12),
				new EnemyNestCreationInfo(8, 12),
				new EnemyNestCreationInfo(9, 12),
				new EnemyNestCreationInfo(10, 7),
				new EnemyNestCreationInfo(11, 7)
			},
			new EnemyNestCreationInfo[1]
			{
				new EnemyNestCreationInfo(13, 4)
			}
		};
		List<IntPair> enemyNestCreationDateRanges24 = new List<IntPair>
		{
			new IntPair(0, 6),
			new IntPair(6, 12),
			new IntPair(12, 24),
			new IntPair(24, 36),
			new IntPair(0, 0)
		};
		short[] settlementBlockCore24 = new short[3] { 26, 35, 36 };
		sbyte[] organizationId24 = new sbyte[3] { 8, 37, 38 };
		List<short[]> developedBlockCore24 = new List<short[]>
		{
			new short[2] { 39, 33 },
			new short[2] { 40, 33 },
			new short[2] { 41, 33 },
			new short[2] { 42, 33 },
			new short[2] { 43, 33 },
			new short[2] { 44, 33 },
			new short[2] { 45, 33 },
			new short[2] { 46, 33 },
			new short[2] { 47, 33 },
			new short[2] { 48, 33 },
			new short[2] { 49, 33 },
			new short[2] { 50, 33 },
			new short[2] { 51, 33 },
			new short[2] { 52, 33 },
			new short[2] { 53, 33 },
			new short[2] { 54, 33 },
			new short[2] { 55, 33 },
			new short[2] { 56, 33 },
			new short[2] { 109, 10 },
			new short[2] { 110, 10 },
			new short[2] { 111, 10 }
		};
		List<short[]> normalBlockCore24 = new List<short[]>
		{
			new short[2] { 57, 33 },
			new short[2] { 58, 33 },
			new short[2] { 59, 33 },
			new short[2] { 63, 33 },
			new short[2] { 64, 33 },
			new short[2] { 65, 33 },
			new short[2] { 69, 33 },
			new short[2] { 70, 33 },
			new short[2] { 71, 33 },
			new short[2] { 75, 33 },
			new short[2] { 76, 33 },
			new short[2] { 77, 33 },
			new short[2] { 81, 33 },
			new short[2] { 82, 33 },
			new short[2] { 83, 33 },
			new short[2] { 87, 33 },
			new short[2] { 88, 33 },
			new short[2] { 89, 33 },
			new short[2] { 112, 30 },
			new short[2] { 113, 30 },
			new short[2] { 114, 30 },
			new short[2] { 124, 30 }
		};
		List<short[]> wildBlockCore24 = new List<short[]>
		{
			new short[2] { 93, 33 },
			new short[2] { 94, 33 },
			new short[2] { 95, 33 },
			new short[2] { 96, 33 },
			new short[2] { 97, 33 },
			new short[2] { 98, 33 },
			new short[2] { 99, 33 },
			new short[2] { 100, 33 },
			new short[2] { 101, 33 },
			new short[2] { 102, 33 },
			new short[2] { 103, 33 },
			new short[2] { 104, 33 },
			new short[2] { 105, 33 },
			new short[2] { 106, 33 },
			new short[2] { 107, 33 },
			new short[2] { 108, 33 },
			new short[2] { 115, 40 },
			new short[2] { 116, 40 },
			new short[2] { 117, 40 },
			new short[2] { 124, 30 }
		};
		List<short[]> bigBaseBlockCore24 = new List<short[]>
		{
			new short[2] { 72, 1 },
			new short[2] { 61, 1 },
			new short[2] { 62, 1 },
			new short[2] { 78, 1 },
			new short[2] { 79, 1 },
			new short[2] { 80, 1 },
			new short[2] { 85, 1 },
			new short[2] { 86, 1 },
			new short[2] { 66, 1 },
			new short[2] { 90, 1 }
		};
		list3 = new List<short[]>();
		List<short[]> seriesBlockCore24 = list3;
		list3 = new List<short[]>();
		List<short[]> encircleBlockCore24 = list3;
		short[] sceneryBlockCore24 = new short[0];
		List<short> lovingItemSubTypes24 = new List<short> { 11, 600 };
		list4 = new List<short>();
		dataArray24.Add(new MapAreaItem(23, 8, 112, 113, 30, worldMapPos24, neighborAreas24, "largemap_part_1_menpai_11", miniMapIcons24, "MapGangXuannv", miniMapPos24, 0, 1, 7, enemyNests24, enemyNestCreationDateRanges24, settlementBlockCore24, organizationId24, -1, 1, null, developedBlockCore24, normalBlockCore24, wildBlockCore24, bigBaseBlockCore24, seriesBlockCore24, encircleBlockCore24, sceneryBlockCore24, 114, 115, null, null, EMapAreaAreaDirection.South, lovingItemSubTypes24, list4, showDarkAshStatus: true));
		List<MapAreaItem> dataArray25 = _dataArray;
		sbyte[] worldMapPos25 = new sbyte[2] { 20, 6 };
		AreaTravelRoute[] neighborAreas25 = new AreaTravelRoute[4]
		{
			new AreaTravelRoute(87, 12, new int[2] { 20, 5 }),
			new AreaTravelRoute(89, 8, null),
			new AreaTravelRoute(90, 6, null),
			new AreaTravelRoute(128, 22, new int[4] { 20, 7, 20, 8 })
		};
		string[] miniMapIcons25 = new string[4] { "largemap_part_2_icon_hei_29", "largemap_part_2_icon_jin_29", "largemap_part_2_icon_lan_29", "largemap_part_2_icon_hui_29" };
		short[] miniMapPos25 = new short[2] { 130, 230 };
		List<EnemyNestCreationInfo[]> enemyNests25 = new List<EnemyNestCreationInfo[]>
		{
			new EnemyNestCreationInfo[2]
			{
				new EnemyNestCreationInfo(2, 3),
				new EnemyNestCreationInfo(3, 3)
			},
			new EnemyNestCreationInfo[2]
			{
				new EnemyNestCreationInfo(4, 4),
				new EnemyNestCreationInfo(5, 4)
			},
			new EnemyNestCreationInfo[2]
			{
				new EnemyNestCreationInfo(6, 5),
				new EnemyNestCreationInfo(7, 5)
			},
			new EnemyNestCreationInfo[8]
			{
				new EnemyNestCreationInfo(8, 12),
				new EnemyNestCreationInfo(9, 12),
				new EnemyNestCreationInfo(8, 12),
				new EnemyNestCreationInfo(9, 12),
				new EnemyNestCreationInfo(8, 12),
				new EnemyNestCreationInfo(9, 12),
				new EnemyNestCreationInfo(10, 7),
				new EnemyNestCreationInfo(11, 7)
			},
			new EnemyNestCreationInfo[1]
			{
				new EnemyNestCreationInfo(13, 4)
			}
		};
		List<IntPair> enemyNestCreationDateRanges25 = new List<IntPair>
		{
			new IntPair(0, 6),
			new IntPair(6, 12),
			new IntPair(12, 24),
			new IntPair(24, 36),
			new IntPair(0, 0)
		};
		short[] settlementBlockCore25 = new short[3] { 27, 34, 35 };
		sbyte[] organizationId25 = new sbyte[3] { 9, 36, 37 };
		List<short[]> developedBlockCore25 = new List<short[]>
		{
			new short[2] { 39, 33 },
			new short[2] { 40, 33 },
			new short[2] { 41, 33 },
			new short[2] { 42, 33 },
			new short[2] { 43, 33 },
			new short[2] { 44, 33 },
			new short[2] { 45, 33 },
			new short[2] { 46, 33 },
			new short[2] { 47, 33 },
			new short[2] { 48, 33 },
			new short[2] { 49, 33 },
			new short[2] { 50, 33 },
			new short[2] { 51, 33 },
			new short[2] { 52, 33 },
			new short[2] { 53, 33 },
			new short[2] { 54, 33 },
			new short[2] { 55, 33 },
			new short[2] { 56, 33 },
			new short[2] { 109, 10 },
			new short[2] { 110, 10 },
			new short[2] { 111, 10 }
		};
		List<short[]> normalBlockCore25 = new List<short[]>
		{
			new short[2] { 57, 33 },
			new short[2] { 58, 33 },
			new short[2] { 59, 33 },
			new short[2] { 63, 33 },
			new short[2] { 64, 33 },
			new short[2] { 65, 33 },
			new short[2] { 69, 33 },
			new short[2] { 70, 33 },
			new short[2] { 71, 33 },
			new short[2] { 75, 33 },
			new short[2] { 76, 33 },
			new short[2] { 77, 33 },
			new short[2] { 81, 33 },
			new short[2] { 82, 33 },
			new short[2] { 83, 33 },
			new short[2] { 87, 33 },
			new short[2] { 88, 33 },
			new short[2] { 89, 33 },
			new short[2] { 112, 30 },
			new short[2] { 113, 30 },
			new short[2] { 114, 30 },
			new short[2] { 124, 30 }
		};
		List<short[]> wildBlockCore25 = new List<short[]>
		{
			new short[2] { 93, 33 },
			new short[2] { 94, 33 },
			new short[2] { 95, 33 },
			new short[2] { 96, 33 },
			new short[2] { 97, 33 },
			new short[2] { 98, 33 },
			new short[2] { 99, 33 },
			new short[2] { 100, 33 },
			new short[2] { 101, 33 },
			new short[2] { 102, 33 },
			new short[2] { 103, 33 },
			new short[2] { 104, 33 },
			new short[2] { 105, 33 },
			new short[2] { 106, 33 },
			new short[2] { 107, 33 },
			new short[2] { 108, 33 },
			new short[2] { 115, 40 },
			new short[2] { 116, 40 },
			new short[2] { 117, 40 },
			new short[2] { 124, 30 }
		};
		List<short[]> bigBaseBlockCore25 = new List<short[]>
		{
			new short[2] { 72, 1 },
			new short[2] { 60, 1 },
			new short[2] { 61, 1 },
			new short[2] { 62, 1 },
			new short[2] { 79, 1 },
			new short[2] { 80, 1 },
			new short[2] { 85, 1 },
			new short[2] { 86, 1 },
			new short[2] { 66, 1 },
			new short[2] { 90, 1 }
		};
		list3 = new List<short[]>();
		List<short[]> seriesBlockCore25 = list3;
		list3 = new List<short[]>();
		List<short[]> encircleBlockCore25 = list3;
		short[] sceneryBlockCore25 = new short[0];
		List<short> lovingItemSubTypes25 = new List<short> { 12, 30 };
		list4 = new List<short>();
		dataArray25.Add(new MapAreaItem(24, 9, 116, 117, 30, worldMapPos25, neighborAreas25, "largemap_part_1_menpai_14", miniMapIcons25, "MapGangZhujian", miniMapPos25, 0, 1, 7, enemyNests25, enemyNestCreationDateRanges25, settlementBlockCore25, organizationId25, -1, 2, null, developedBlockCore25, normalBlockCore25, wildBlockCore25, bigBaseBlockCore25, seriesBlockCore25, encircleBlockCore25, sceneryBlockCore25, 118, 119, null, null, EMapAreaAreaDirection.South, lovingItemSubTypes25, list4, showDarkAshStatus: true));
		List<MapAreaItem> dataArray26 = _dataArray;
		sbyte[] worldMapPos26 = new sbyte[2] { 22, 24 };
		AreaTravelRoute[] neighborAreas26 = new AreaTravelRoute[1]
		{
			new AreaTravelRoute(94, 8, null)
		};
		string[] miniMapIcons26 = new string[4] { "largemap_part_2_icon_hei_18", "largemap_part_2_icon_jin_18", "largemap_part_2_icon_lan_18", "largemap_part_2_icon_hui_18" };
		short[] miniMapPos26 = new short[2] { 180, 200 };
		List<EnemyNestCreationInfo[]> enemyNests26 = new List<EnemyNestCreationInfo[]>
		{
			new EnemyNestCreationInfo[2]
			{
				new EnemyNestCreationInfo(2, 3),
				new EnemyNestCreationInfo(3, 3)
			},
			new EnemyNestCreationInfo[2]
			{
				new EnemyNestCreationInfo(4, 4),
				new EnemyNestCreationInfo(5, 4)
			},
			new EnemyNestCreationInfo[2]
			{
				new EnemyNestCreationInfo(6, 5),
				new EnemyNestCreationInfo(7, 5)
			},
			new EnemyNestCreationInfo[8]
			{
				new EnemyNestCreationInfo(8, 12),
				new EnemyNestCreationInfo(9, 12),
				new EnemyNestCreationInfo(8, 12),
				new EnemyNestCreationInfo(9, 12),
				new EnemyNestCreationInfo(8, 12),
				new EnemyNestCreationInfo(9, 12),
				new EnemyNestCreationInfo(10, 7),
				new EnemyNestCreationInfo(11, 7)
			},
			new EnemyNestCreationInfo[1]
			{
				new EnemyNestCreationInfo(13, 4)
			}
		};
		List<IntPair> enemyNestCreationDateRanges26 = new List<IntPair>
		{
			new IntPair(0, 6),
			new IntPair(6, 12),
			new IntPair(12, 24),
			new IntPair(24, 36),
			new IntPair(0, 0)
		};
		short[] settlementBlockCore26 = new short[3] { 28, 34, 36 };
		sbyte[] organizationId26 = new sbyte[3] { 10, 36, 38 };
		List<short[]> developedBlockCore26 = new List<short[]>
		{
			new short[2] { 39, 33 },
			new short[2] { 40, 33 },
			new short[2] { 41, 33 },
			new short[2] { 42, 33 },
			new short[2] { 43, 33 },
			new short[2] { 44, 33 },
			new short[2] { 45, 33 },
			new short[2] { 46, 33 },
			new short[2] { 47, 33 },
			new short[2] { 48, 33 },
			new short[2] { 49, 33 },
			new short[2] { 50, 33 },
			new short[2] { 51, 33 },
			new short[2] { 52, 33 },
			new short[2] { 53, 33 },
			new short[2] { 54, 33 },
			new short[2] { 55, 33 },
			new short[2] { 56, 33 },
			new short[2] { 109, 10 },
			new short[2] { 110, 10 },
			new short[2] { 111, 10 }
		};
		List<short[]> normalBlockCore26 = new List<short[]>
		{
			new short[2] { 57, 33 },
			new short[2] { 58, 33 },
			new short[2] { 59, 33 },
			new short[2] { 63, 33 },
			new short[2] { 64, 33 },
			new short[2] { 65, 33 },
			new short[2] { 69, 33 },
			new short[2] { 70, 33 },
			new short[2] { 71, 33 },
			new short[2] { 75, 33 },
			new short[2] { 76, 33 },
			new short[2] { 77, 33 },
			new short[2] { 81, 33 },
			new short[2] { 82, 33 },
			new short[2] { 83, 33 },
			new short[2] { 87, 33 },
			new short[2] { 88, 33 },
			new short[2] { 89, 33 },
			new short[2] { 112, 30 },
			new short[2] { 113, 30 },
			new short[2] { 114, 30 },
			new short[2] { 124, 30 }
		};
		List<short[]> wildBlockCore26 = new List<short[]>
		{
			new short[2] { 93, 33 },
			new short[2] { 94, 33 },
			new short[2] { 95, 33 },
			new short[2] { 96, 33 },
			new short[2] { 97, 33 },
			new short[2] { 98, 33 },
			new short[2] { 99, 33 },
			new short[2] { 100, 33 },
			new short[2] { 101, 33 },
			new short[2] { 102, 33 },
			new short[2] { 103, 33 },
			new short[2] { 104, 33 },
			new short[2] { 105, 33 },
			new short[2] { 106, 33 },
			new short[2] { 107, 33 },
			new short[2] { 108, 33 },
			new short[2] { 115, 40 },
			new short[2] { 116, 40 },
			new short[2] { 117, 40 },
			new short[2] { 124, 30 }
		};
		List<short[]> bigBaseBlockCore26 = new List<short[]>
		{
			new short[2] { 72, 1 },
			new short[2] { 61, 1 },
			new short[2] { 62, 1 },
			new short[2] { 78, 1 },
			new short[2] { 85, 1 },
			new short[2] { 86, 1 },
			new short[2] { 66, 1 },
			new short[2] { 67, 1 },
			new short[2] { 68, 1 },
			new short[2] { 90, 1 }
		};
		list3 = new List<short[]>();
		List<short[]> seriesBlockCore26 = list3;
		list3 = new List<short[]>();
		List<short[]> encircleBlockCore26 = list3;
		short[] sceneryBlockCore26 = new short[0];
		List<short> lovingItemSubTypes26 = new List<short> { 14 };
		list4 = new List<short>();
		dataArray26.Add(new MapAreaItem(25, 10, 120, 121, 30, worldMapPos26, neighborAreas26, "largemap_part_1_menpai_5", miniMapIcons26, "MapGangKongsang", miniMapPos26, 0, 1, 7, enemyNests26, enemyNestCreationDateRanges26, settlementBlockCore26, organizationId26, -1, 2, null, developedBlockCore26, normalBlockCore26, wildBlockCore26, bigBaseBlockCore26, seriesBlockCore26, encircleBlockCore26, sceneryBlockCore26, 122, 123, null, null, EMapAreaAreaDirection.South, lovingItemSubTypes26, list4, showDarkAshStatus: true));
		List<MapAreaItem> dataArray27 = _dataArray;
		sbyte[] worldMapPos27 = new sbyte[2] { 0, 15 };
		AreaTravelRoute[] neighborAreas27 = new AreaTravelRoute[4]
		{
			new AreaTravelRoute(101, 46, new int[8] { 1, 15, 2, 15, 3, 15, 3, 16 }),
			new AreaTravelRoute(102, 46, new int[8] { 1, 15, 1, 16, 1, 17, 1, 18 }),
			new AreaTravelRoute(103, 36, new int[6] { 1, 15, 2, 15, 3, 15 }),
			new AreaTravelRoute(106, 46, new int[8] { 1, 15, 2, 15, 3, 15, 3, 14 })
		};
		string[] miniMapIcons27 = new string[4] { "largemap_part_2_icon_hei_22", "largemap_part_2_icon_jin_22", "largemap_part_2_icon_lan_22", "largemap_part_2_icon_hui_22" };
		short[] miniMapPos27 = new short[2] { 80, 180 };
		List<EnemyNestCreationInfo[]> enemyNests27 = new List<EnemyNestCreationInfo[]>
		{
			new EnemyNestCreationInfo[2]
			{
				new EnemyNestCreationInfo(2, 3),
				new EnemyNestCreationInfo(3, 3)
			},
			new EnemyNestCreationInfo[2]
			{
				new EnemyNestCreationInfo(4, 4),
				new EnemyNestCreationInfo(5, 4)
			},
			new EnemyNestCreationInfo[2]
			{
				new EnemyNestCreationInfo(6, 5),
				new EnemyNestCreationInfo(7, 5)
			},
			new EnemyNestCreationInfo[8]
			{
				new EnemyNestCreationInfo(8, 12),
				new EnemyNestCreationInfo(9, 12),
				new EnemyNestCreationInfo(8, 12),
				new EnemyNestCreationInfo(9, 12),
				new EnemyNestCreationInfo(8, 12),
				new EnemyNestCreationInfo(9, 12),
				new EnemyNestCreationInfo(10, 7),
				new EnemyNestCreationInfo(11, 7)
			},
			new EnemyNestCreationInfo[1]
			{
				new EnemyNestCreationInfo(13, 4)
			}
		};
		List<IntPair> enemyNestCreationDateRanges27 = new List<IntPair>
		{
			new IntPair(0, 6),
			new IntPair(6, 12),
			new IntPair(12, 24),
			new IntPair(24, 36),
			new IntPair(0, 0)
		};
		short[] settlementBlockCore27 = new short[3] { 29, 34, 36 };
		sbyte[] organizationId27 = new sbyte[3] { 11, 36, 38 };
		List<short[]> developedBlockCore27 = new List<short[]>
		{
			new short[2] { 39, 33 },
			new short[2] { 40, 33 },
			new short[2] { 41, 33 },
			new short[2] { 42, 33 },
			new short[2] { 43, 33 },
			new short[2] { 44, 33 },
			new short[2] { 45, 33 },
			new short[2] { 46, 33 },
			new short[2] { 47, 33 },
			new short[2] { 48, 33 },
			new short[2] { 49, 33 },
			new short[2] { 50, 33 },
			new short[2] { 51, 33 },
			new short[2] { 52, 33 },
			new short[2] { 53, 33 },
			new short[2] { 54, 33 },
			new short[2] { 55, 33 },
			new short[2] { 56, 33 },
			new short[2] { 109, 10 },
			new short[2] { 110, 10 },
			new short[2] { 111, 10 }
		};
		List<short[]> normalBlockCore27 = new List<short[]>
		{
			new short[2] { 57, 33 },
			new short[2] { 58, 33 },
			new short[2] { 59, 33 },
			new short[2] { 63, 33 },
			new short[2] { 64, 33 },
			new short[2] { 65, 33 },
			new short[2] { 69, 33 },
			new short[2] { 70, 33 },
			new short[2] { 71, 33 },
			new short[2] { 75, 33 },
			new short[2] { 76, 33 },
			new short[2] { 77, 33 },
			new short[2] { 81, 33 },
			new short[2] { 82, 33 },
			new short[2] { 83, 33 },
			new short[2] { 87, 33 },
			new short[2] { 88, 33 },
			new short[2] { 89, 33 },
			new short[2] { 112, 30 },
			new short[2] { 113, 30 },
			new short[2] { 114, 30 },
			new short[2] { 124, 30 }
		};
		List<short[]> wildBlockCore27 = new List<short[]>
		{
			new short[2] { 93, 33 },
			new short[2] { 94, 33 },
			new short[2] { 95, 33 },
			new short[2] { 96, 33 },
			new short[2] { 97, 33 },
			new short[2] { 98, 33 },
			new short[2] { 99, 33 },
			new short[2] { 100, 33 },
			new short[2] { 101, 33 },
			new short[2] { 102, 33 },
			new short[2] { 103, 33 },
			new short[2] { 104, 33 },
			new short[2] { 105, 33 },
			new short[2] { 106, 33 },
			new short[2] { 107, 33 },
			new short[2] { 108, 33 },
			new short[2] { 115, 40 },
			new short[2] { 116, 40 },
			new short[2] { 117, 40 },
			new short[2] { 124, 30 }
		};
		List<short[]> bigBaseBlockCore27 = new List<short[]>
		{
			new short[2] { 73, 1 },
			new short[2] { 74, 1 },
			new short[2] { 60, 1 },
			new short[2] { 78, 1 },
			new short[2] { 79, 1 },
			new short[2] { 80, 1 },
			new short[2] { 84, 1 },
			new short[2] { 66, 1 },
			new short[2] { 91, 1 },
			new short[2] { 92, 1 }
		};
		list3 = new List<short[]>();
		List<short[]> seriesBlockCore27 = list3;
		list3 = new List<short[]>();
		List<short[]> encircleBlockCore27 = list3;
		short[] sceneryBlockCore27 = new short[0];
		List<short> lovingItemSubTypes27 = new List<short> { 5 };
		list4 = new List<short>();
		dataArray27.Add(new MapAreaItem(26, 11, 124, 125, 30, worldMapPos27, neighborAreas27, "largemap_part_1_menpai_4", miniMapIcons27, "MapGangJingang", miniMapPos27, 0, 1, 7, enemyNests27, enemyNestCreationDateRanges27, settlementBlockCore27, organizationId27, -1, 2, null, developedBlockCore27, normalBlockCore27, wildBlockCore27, bigBaseBlockCore27, seriesBlockCore27, encircleBlockCore27, sceneryBlockCore27, 126, 127, null, null, EMapAreaAreaDirection.South, lovingItemSubTypes27, list4, showDarkAshStatus: true));
		List<MapAreaItem> dataArray28 = _dataArray;
		sbyte[] worldMapPos28 = new sbyte[2] { 4, 3 };
		AreaTravelRoute[] neighborAreas28 = new AreaTravelRoute[3]
		{
			new AreaTravelRoute(46, 22, new int[4] { 5, 3, 6, 3 }),
			new AreaTravelRoute(109, 6, null),
			new AreaTravelRoute(114, 6, null)
		};
		string[] miniMapIcons28 = new string[4] { "largemap_part_2_icon_hei_30", "largemap_part_2_icon_jin_30", "largemap_part_2_icon_lan_30", "largemap_part_2_icon_hui_30" };
		short[] miniMapPos28 = new short[2] { 260, 180 };
		List<EnemyNestCreationInfo[]> enemyNests28 = new List<EnemyNestCreationInfo[]>
		{
			new EnemyNestCreationInfo[2]
			{
				new EnemyNestCreationInfo(2, 3),
				new EnemyNestCreationInfo(3, 3)
			},
			new EnemyNestCreationInfo[2]
			{
				new EnemyNestCreationInfo(4, 4),
				new EnemyNestCreationInfo(5, 4)
			},
			new EnemyNestCreationInfo[2]
			{
				new EnemyNestCreationInfo(6, 5),
				new EnemyNestCreationInfo(7, 5)
			},
			new EnemyNestCreationInfo[8]
			{
				new EnemyNestCreationInfo(8, 12),
				new EnemyNestCreationInfo(9, 12),
				new EnemyNestCreationInfo(8, 12),
				new EnemyNestCreationInfo(9, 12),
				new EnemyNestCreationInfo(8, 12),
				new EnemyNestCreationInfo(9, 12),
				new EnemyNestCreationInfo(10, 7),
				new EnemyNestCreationInfo(11, 7)
			},
			new EnemyNestCreationInfo[1]
			{
				new EnemyNestCreationInfo(13, 4)
			}
		};
		List<IntPair> enemyNestCreationDateRanges28 = new List<IntPair>
		{
			new IntPair(0, 6),
			new IntPair(6, 12),
			new IntPair(12, 24),
			new IntPair(24, 36),
			new IntPair(0, 0)
		};
		short[] settlementBlockCore28 = new short[3] { 30, 34, 36 };
		sbyte[] organizationId28 = new sbyte[3] { 12, 36, 38 };
		List<short[]> developedBlockCore28 = new List<short[]>
		{
			new short[2] { 39, 33 },
			new short[2] { 40, 33 },
			new short[2] { 41, 33 },
			new short[2] { 42, 33 },
			new short[2] { 43, 33 },
			new short[2] { 44, 33 },
			new short[2] { 45, 33 },
			new short[2] { 46, 33 },
			new short[2] { 47, 33 },
			new short[2] { 48, 33 },
			new short[2] { 49, 33 },
			new short[2] { 50, 33 },
			new short[2] { 51, 33 },
			new short[2] { 52, 33 },
			new short[2] { 53, 33 },
			new short[2] { 54, 33 },
			new short[2] { 55, 33 },
			new short[2] { 56, 33 },
			new short[2] { 109, 10 },
			new short[2] { 110, 10 },
			new short[2] { 111, 10 }
		};
		List<short[]> normalBlockCore28 = new List<short[]>
		{
			new short[2] { 57, 33 },
			new short[2] { 58, 33 },
			new short[2] { 59, 33 },
			new short[2] { 63, 33 },
			new short[2] { 64, 33 },
			new short[2] { 65, 33 },
			new short[2] { 69, 33 },
			new short[2] { 70, 33 },
			new short[2] { 71, 33 },
			new short[2] { 75, 33 },
			new short[2] { 76, 33 },
			new short[2] { 77, 33 },
			new short[2] { 81, 33 },
			new short[2] { 82, 33 },
			new short[2] { 83, 33 },
			new short[2] { 87, 33 },
			new short[2] { 88, 33 },
			new short[2] { 89, 33 },
			new short[2] { 112, 30 },
			new short[2] { 113, 30 },
			new short[2] { 114, 30 },
			new short[2] { 124, 30 }
		};
		List<short[]> wildBlockCore28 = new List<short[]>
		{
			new short[2] { 93, 33 },
			new short[2] { 94, 33 },
			new short[2] { 95, 33 },
			new short[2] { 96, 33 },
			new short[2] { 97, 33 },
			new short[2] { 98, 33 },
			new short[2] { 99, 33 },
			new short[2] { 100, 33 },
			new short[2] { 101, 33 },
			new short[2] { 102, 33 },
			new short[2] { 103, 33 },
			new short[2] { 104, 33 },
			new short[2] { 105, 33 },
			new short[2] { 106, 33 },
			new short[2] { 107, 33 },
			new short[2] { 108, 33 },
			new short[2] { 115, 40 },
			new short[2] { 116, 40 },
			new short[2] { 117, 40 },
			new short[2] { 124, 30 }
		};
		List<short[]> bigBaseBlockCore28 = new List<short[]>
		{
			new short[2] { 73, 1 },
			new short[2] { 74, 1 },
			new short[2] { 61, 1 },
			new short[2] { 62, 1 },
			new short[2] { 78, 1 },
			new short[2] { 84, 1 },
			new short[2] { 66, 1 },
			new short[2] { 67, 1 },
			new short[2] { 68, 1 },
			new short[2] { 90, 1 }
		};
		list3 = new List<short[]>();
		List<short[]> seriesBlockCore28 = list3;
		list3 = new List<short[]>();
		List<short[]> encircleBlockCore28 = list3;
		short[] sceneryBlockCore28 = new short[0];
		List<short> lovingItemSubTypes28 = new List<short> { 7 };
		list4 = new List<short>();
		dataArray28.Add(new MapAreaItem(27, 12, 128, 129, 30, worldMapPos28, neighborAreas28, "largemap_part_1_menpai_9", miniMapIcons28, "MapGangWuxian", miniMapPos28, 0, 1, 7, enemyNests28, enemyNestCreationDateRanges28, settlementBlockCore28, organizationId28, -1, 2, null, developedBlockCore28, normalBlockCore28, wildBlockCore28, bigBaseBlockCore28, seriesBlockCore28, encircleBlockCore28, sceneryBlockCore28, 130, 131, null, null, EMapAreaAreaDirection.South, lovingItemSubTypes28, list4, showDarkAshStatus: true));
		List<MapAreaItem> dataArray29 = _dataArray;
		sbyte[] worldMapPos29 = new sbyte[2] { 17, 8 };
		AreaTravelRoute[] neighborAreas29 = new AreaTravelRoute[5]
		{
			new AreaTravelRoute(57, 24, new int[4] { 16, 8, 15, 8 }),
			new AreaTravelRoute(69, 44, new int[8] { 17, 7, 17, 6, 17, 5, 17, 4 }),
			new AreaTravelRoute(82, 44, new int[8] { 17, 7, 17, 6, 16, 6, 15, 6 }),
			new AreaTravelRoute(90, 34, new int[6] { 17, 7, 17, 6, 18, 6 }),
			new AreaTravelRoute(118, 8, null)
		};
		string[] miniMapIcons29 = new string[4] { "largemap_part_2_icon_hei_24", "largemap_part_2_icon_jin_24", "largemap_part_2_icon_lan_24", "largemap_part_2_icon_hui_24" };
		short[] miniMapPos29 = new short[2] { 180, 100 };
		List<EnemyNestCreationInfo[]> enemyNests29 = new List<EnemyNestCreationInfo[]>
		{
			new EnemyNestCreationInfo[2]
			{
				new EnemyNestCreationInfo(2, 3),
				new EnemyNestCreationInfo(3, 3)
			},
			new EnemyNestCreationInfo[2]
			{
				new EnemyNestCreationInfo(4, 4),
				new EnemyNestCreationInfo(5, 4)
			},
			new EnemyNestCreationInfo[2]
			{
				new EnemyNestCreationInfo(6, 5),
				new EnemyNestCreationInfo(7, 5)
			},
			new EnemyNestCreationInfo[8]
			{
				new EnemyNestCreationInfo(8, 12),
				new EnemyNestCreationInfo(9, 12),
				new EnemyNestCreationInfo(8, 12),
				new EnemyNestCreationInfo(9, 12),
				new EnemyNestCreationInfo(8, 12),
				new EnemyNestCreationInfo(9, 12),
				new EnemyNestCreationInfo(10, 7),
				new EnemyNestCreationInfo(11, 7)
			},
			new EnemyNestCreationInfo[1]
			{
				new EnemyNestCreationInfo(13, 4)
			}
		};
		List<IntPair> enemyNestCreationDateRanges29 = new List<IntPair>
		{
			new IntPair(0, 6),
			new IntPair(6, 12),
			new IntPair(12, 24),
			new IntPair(24, 36),
			new IntPair(0, 0)
		};
		short[] settlementBlockCore29 = new short[3] { 31, 34, 36 };
		sbyte[] organizationId29 = new sbyte[3] { 13, 36, 38 };
		List<short[]> developedBlockCore29 = new List<short[]>
		{
			new short[2] { 39, 33 },
			new short[2] { 40, 33 },
			new short[2] { 41, 33 },
			new short[2] { 42, 33 },
			new short[2] { 43, 33 },
			new short[2] { 44, 33 },
			new short[2] { 45, 33 },
			new short[2] { 46, 33 },
			new short[2] { 47, 33 },
			new short[2] { 48, 33 },
			new short[2] { 49, 33 },
			new short[2] { 50, 33 },
			new short[2] { 51, 33 },
			new short[2] { 52, 33 },
			new short[2] { 53, 33 },
			new short[2] { 54, 33 },
			new short[2] { 55, 33 },
			new short[2] { 56, 33 },
			new short[2] { 109, 10 },
			new short[2] { 110, 10 },
			new short[2] { 111, 10 }
		};
		List<short[]> normalBlockCore29 = new List<short[]>
		{
			new short[2] { 57, 33 },
			new short[2] { 58, 33 },
			new short[2] { 59, 33 },
			new short[2] { 63, 33 },
			new short[2] { 64, 33 },
			new short[2] { 65, 33 },
			new short[2] { 69, 33 },
			new short[2] { 70, 33 },
			new short[2] { 71, 33 },
			new short[2] { 75, 33 },
			new short[2] { 76, 33 },
			new short[2] { 77, 33 },
			new short[2] { 81, 33 },
			new short[2] { 82, 33 },
			new short[2] { 83, 33 },
			new short[2] { 87, 33 },
			new short[2] { 88, 33 },
			new short[2] { 89, 33 },
			new short[2] { 112, 30 },
			new short[2] { 113, 30 },
			new short[2] { 114, 30 },
			new short[2] { 124, 30 }
		};
		List<short[]> wildBlockCore29 = new List<short[]>
		{
			new short[2] { 93, 33 },
			new short[2] { 94, 33 },
			new short[2] { 95, 33 },
			new short[2] { 96, 33 },
			new short[2] { 97, 33 },
			new short[2] { 98, 33 },
			new short[2] { 99, 33 },
			new short[2] { 100, 33 },
			new short[2] { 101, 33 },
			new short[2] { 102, 33 },
			new short[2] { 103, 33 },
			new short[2] { 104, 33 },
			new short[2] { 105, 33 },
			new short[2] { 106, 33 },
			new short[2] { 107, 33 },
			new short[2] { 108, 33 },
			new short[2] { 115, 40 },
			new short[2] { 116, 40 },
			new short[2] { 117, 40 },
			new short[2] { 124, 30 }
		};
		List<short[]> bigBaseBlockCore29 = new List<short[]>
		{
			new short[2] { 72, 1 },
			new short[2] { 60, 1 },
			new short[2] { 78, 1 },
			new short[2] { 85, 1 },
			new short[2] { 86, 1 },
			new short[2] { 66, 1 },
			new short[2] { 67, 1 },
			new short[2] { 68, 1 },
			new short[2] { 91, 1 },
			new short[2] { 92, 1 }
		};
		list3 = new List<short[]>();
		List<short[]> seriesBlockCore29 = list3;
		list3 = new List<short[]>();
		List<short[]> encircleBlockCore29 = list3;
		short[] sceneryBlockCore29 = new short[0];
		List<short> lovingItemSubTypes29 = new List<short> { 2 };
		list4 = new List<short>();
		dataArray29.Add(new MapAreaItem(28, 13, 132, 133, 30, worldMapPos29, neighborAreas29, "largemap_part_1_menpai_3", miniMapIcons29, "MapGangJieqing", miniMapPos29, 0, 1, 7, enemyNests29, enemyNestCreationDateRanges29, settlementBlockCore29, organizationId29, -1, 2, null, developedBlockCore29, normalBlockCore29, wildBlockCore29, bigBaseBlockCore29, seriesBlockCore29, encircleBlockCore29, sceneryBlockCore29, 134, 135, null, null, EMapAreaAreaDirection.South, lovingItemSubTypes29, list4, showDarkAshStatus: true));
		List<MapAreaItem> dataArray30 = _dataArray;
		sbyte[] worldMapPos30 = new sbyte[2] { 24, 11 };
		AreaTravelRoute[] neighborAreas30 = new AreaTravelRoute[1]
		{
			new AreaTravelRoute(122, 24, new int[4] { 23, 11, 22, 11 })
		};
		string[] miniMapIcons30 = new string[4] { "largemap_part_2_icon_hei_25", "largemap_part_2_icon_jin_25", "largemap_part_2_icon_lan_25", "largemap_part_2_icon_hui_25" };
		short[] miniMapPos30 = new short[2] { 250, 220 };
		List<EnemyNestCreationInfo[]> enemyNests30 = new List<EnemyNestCreationInfo[]>
		{
			new EnemyNestCreationInfo[2]
			{
				new EnemyNestCreationInfo(2, 3),
				new EnemyNestCreationInfo(3, 3)
			},
			new EnemyNestCreationInfo[2]
			{
				new EnemyNestCreationInfo(4, 4),
				new EnemyNestCreationInfo(5, 4)
			},
			new EnemyNestCreationInfo[2]
			{
				new EnemyNestCreationInfo(6, 5),
				new EnemyNestCreationInfo(7, 5)
			},
			new EnemyNestCreationInfo[8]
			{
				new EnemyNestCreationInfo(8, 12),
				new EnemyNestCreationInfo(9, 12),
				new EnemyNestCreationInfo(8, 12),
				new EnemyNestCreationInfo(9, 12),
				new EnemyNestCreationInfo(8, 12),
				new EnemyNestCreationInfo(9, 12),
				new EnemyNestCreationInfo(10, 7),
				new EnemyNestCreationInfo(11, 7)
			},
			new EnemyNestCreationInfo[1]
			{
				new EnemyNestCreationInfo(13, 4)
			}
		};
		List<IntPair> enemyNestCreationDateRanges30 = new List<IntPair>
		{
			new IntPair(0, 6),
			new IntPair(6, 12),
			new IntPair(12, 24),
			new IntPair(24, 36),
			new IntPair(0, 0)
		};
		short[] settlementBlockCore30 = new short[3] { 32, 34, 36 };
		sbyte[] organizationId30 = new sbyte[3] { 14, 36, 38 };
		List<short[]> developedBlockCore30 = new List<short[]>
		{
			new short[2] { 39, 33 },
			new short[2] { 40, 33 },
			new short[2] { 41, 33 },
			new short[2] { 42, 33 },
			new short[2] { 43, 33 },
			new short[2] { 44, 33 },
			new short[2] { 45, 33 },
			new short[2] { 46, 33 },
			new short[2] { 47, 33 },
			new short[2] { 48, 33 },
			new short[2] { 49, 33 },
			new short[2] { 50, 33 },
			new short[2] { 51, 33 },
			new short[2] { 52, 33 },
			new short[2] { 53, 33 },
			new short[2] { 54, 33 },
			new short[2] { 55, 33 },
			new short[2] { 56, 33 },
			new short[2] { 109, 10 },
			new short[2] { 110, 10 },
			new short[2] { 111, 10 }
		};
		List<short[]> normalBlockCore30 = new List<short[]>
		{
			new short[2] { 57, 33 },
			new short[2] { 58, 33 },
			new short[2] { 59, 33 },
			new short[2] { 63, 33 },
			new short[2] { 64, 33 },
			new short[2] { 65, 33 },
			new short[2] { 69, 33 },
			new short[2] { 70, 33 },
			new short[2] { 71, 33 },
			new short[2] { 75, 33 },
			new short[2] { 76, 33 },
			new short[2] { 77, 33 },
			new short[2] { 81, 33 },
			new short[2] { 82, 33 },
			new short[2] { 83, 33 },
			new short[2] { 87, 33 },
			new short[2] { 88, 33 },
			new short[2] { 89, 33 },
			new short[2] { 112, 30 },
			new short[2] { 113, 30 },
			new short[2] { 114, 30 },
			new short[2] { 124, 30 }
		};
		List<short[]> wildBlockCore30 = new List<short[]>
		{
			new short[2] { 93, 33 },
			new short[2] { 94, 33 },
			new short[2] { 95, 33 },
			new short[2] { 96, 33 },
			new short[2] { 97, 33 },
			new short[2] { 98, 33 },
			new short[2] { 99, 33 },
			new short[2] { 100, 33 },
			new short[2] { 101, 33 },
			new short[2] { 102, 33 },
			new short[2] { 103, 33 },
			new short[2] { 104, 33 },
			new short[2] { 105, 33 },
			new short[2] { 106, 33 },
			new short[2] { 107, 33 },
			new short[2] { 108, 33 },
			new short[2] { 115, 40 },
			new short[2] { 116, 40 },
			new short[2] { 117, 40 },
			new short[2] { 124, 30 }
		};
		List<short[]> bigBaseBlockCore30 = new List<short[]>
		{
			new short[2] { 72, 1 },
			new short[2] { 60, 1 },
			new short[2] { 78, 1 },
			new short[2] { 84, 1 },
			new short[2] { 85, 1 },
			new short[2] { 86, 1 },
			new short[2] { 67, 1 },
			new short[2] { 68, 1 },
			new short[2] { 91, 1 },
			new short[2] { 92, 1 }
		};
		list3 = new List<short[]>();
		List<short[]> seriesBlockCore30 = list3;
		list3 = new List<short[]>();
		List<short[]> encircleBlockCore30 = list3;
		short[] sceneryBlockCore30 = new short[0];
		List<short> lovingItemSubTypes30 = new List<short> { 4, 901 };
		list4 = new List<short>();
		dataArray30.Add(new MapAreaItem(29, 14, 136, 137, 30, worldMapPos30, neighborAreas30, "largemap_part_1_menpai_2", miniMapIcons30, "MapGangFulong", miniMapPos30, 0, 1, 7, enemyNests30, enemyNestCreationDateRanges30, settlementBlockCore30, organizationId30, -1, 2, null, developedBlockCore30, normalBlockCore30, wildBlockCore30, bigBaseBlockCore30, seriesBlockCore30, encircleBlockCore30, sceneryBlockCore30, 138, 139, null, null, EMapAreaAreaDirection.South, lovingItemSubTypes30, list4, showDarkAshStatus: true));
		List<MapAreaItem> dataArray31 = _dataArray;
		sbyte[] worldMapPos31 = new sbyte[2] { 18, 14 };
		AreaTravelRoute[] neighborAreas31 = new AreaTravelRoute[1]
		{
			new AreaTravelRoute(134, 6, null)
		};
		string[] miniMapIcons31 = new string[4] { "largemap_part_2_icon_hei_26", "largemap_part_2_icon_jin_26", "largemap_part_2_icon_lan_26", "largemap_part_2_icon_hui_26" };
		short[] miniMapPos31 = new short[2] { 120, 170 };
		List<EnemyNestCreationInfo[]> enemyNests31 = new List<EnemyNestCreationInfo[]>
		{
			new EnemyNestCreationInfo[2]
			{
				new EnemyNestCreationInfo(2, 3),
				new EnemyNestCreationInfo(3, 3)
			},
			new EnemyNestCreationInfo[2]
			{
				new EnemyNestCreationInfo(4, 4),
				new EnemyNestCreationInfo(5, 4)
			},
			new EnemyNestCreationInfo[2]
			{
				new EnemyNestCreationInfo(6, 5),
				new EnemyNestCreationInfo(7, 5)
			},
			new EnemyNestCreationInfo[8]
			{
				new EnemyNestCreationInfo(8, 12),
				new EnemyNestCreationInfo(9, 12),
				new EnemyNestCreationInfo(8, 12),
				new EnemyNestCreationInfo(9, 12),
				new EnemyNestCreationInfo(8, 12),
				new EnemyNestCreationInfo(9, 12),
				new EnemyNestCreationInfo(10, 7),
				new EnemyNestCreationInfo(11, 7)
			},
			new EnemyNestCreationInfo[1]
			{
				new EnemyNestCreationInfo(13, 4)
			}
		};
		List<IntPair> enemyNestCreationDateRanges31 = new List<IntPair>
		{
			new IntPair(0, 6),
			new IntPair(6, 12),
			new IntPair(12, 24),
			new IntPair(24, 36),
			new IntPair(0, 0)
		};
		short[] settlementBlockCore31 = new short[3] { 33, 34, 36 };
		sbyte[] organizationId31 = new sbyte[3] { 15, 36, 38 };
		List<short[]> developedBlockCore31 = new List<short[]>
		{
			new short[2] { 39, 33 },
			new short[2] { 40, 33 },
			new short[2] { 41, 33 },
			new short[2] { 42, 33 },
			new short[2] { 43, 33 },
			new short[2] { 44, 33 },
			new short[2] { 45, 33 },
			new short[2] { 46, 33 },
			new short[2] { 47, 33 },
			new short[2] { 48, 33 },
			new short[2] { 49, 33 },
			new short[2] { 50, 33 },
			new short[2] { 51, 33 },
			new short[2] { 52, 33 },
			new short[2] { 53, 33 },
			new short[2] { 54, 33 },
			new short[2] { 55, 33 },
			new short[2] { 56, 33 },
			new short[2] { 109, 10 },
			new short[2] { 110, 10 },
			new short[2] { 111, 10 }
		};
		List<short[]> normalBlockCore31 = new List<short[]>
		{
			new short[2] { 57, 33 },
			new short[2] { 58, 33 },
			new short[2] { 59, 33 },
			new short[2] { 63, 33 },
			new short[2] { 64, 33 },
			new short[2] { 65, 33 },
			new short[2] { 69, 33 },
			new short[2] { 70, 33 },
			new short[2] { 71, 33 },
			new short[2] { 75, 33 },
			new short[2] { 76, 33 },
			new short[2] { 77, 33 },
			new short[2] { 81, 33 },
			new short[2] { 82, 33 },
			new short[2] { 83, 33 },
			new short[2] { 87, 33 },
			new short[2] { 88, 33 },
			new short[2] { 89, 33 },
			new short[2] { 112, 30 },
			new short[2] { 113, 30 },
			new short[2] { 114, 30 },
			new short[2] { 124, 30 }
		};
		List<short[]> wildBlockCore31 = new List<short[]>
		{
			new short[2] { 93, 33 },
			new short[2] { 94, 33 },
			new short[2] { 95, 33 },
			new short[2] { 96, 33 },
			new short[2] { 97, 33 },
			new short[2] { 98, 33 },
			new short[2] { 99, 33 },
			new short[2] { 100, 33 },
			new short[2] { 101, 33 },
			new short[2] { 102, 33 },
			new short[2] { 103, 33 },
			new short[2] { 104, 33 },
			new short[2] { 105, 33 },
			new short[2] { 106, 33 },
			new short[2] { 107, 33 },
			new short[2] { 108, 33 },
			new short[2] { 115, 40 },
			new short[2] { 116, 40 },
			new short[2] { 117, 40 },
			new short[2] { 124, 30 }
		};
		List<short[]> bigBaseBlockCore31 = new List<short[]>
		{
			new short[2] { 72, 1 },
			new short[2] { 60, 1 },
			new short[2] { 78, 1 },
			new short[2] { 85, 1 },
			new short[2] { 86, 1 },
			new short[2] { 67, 1 },
			new short[2] { 68, 1 },
			new short[2] { 90, 1 },
			new short[2] { 91, 1 },
			new short[2] { 92, 1 }
		};
		list3 = new List<short[]>();
		List<short[]> seriesBlockCore31 = list3;
		list3 = new List<short[]>();
		List<short[]> encircleBlockCore31 = list3;
		short[] sceneryBlockCore31 = new short[0];
		List<short> lovingItemSubTypes31 = new List<short> { 15 };
		list4 = new List<short>();
		dataArray31.Add(new MapAreaItem(30, 15, 140, 141, 30, worldMapPos31, neighborAreas31, "largemap_part_1_menpai_12", miniMapIcons31, "MapGangXuehou", miniMapPos31, 0, 1, 7, enemyNests31, enemyNestCreationDateRanges31, settlementBlockCore31, organizationId31, -1, 2, null, developedBlockCore31, normalBlockCore31, wildBlockCore31, bigBaseBlockCore31, seriesBlockCore31, encircleBlockCore31, sceneryBlockCore31, 142, 143, null, null, EMapAreaAreaDirection.South, lovingItemSubTypes31, list4, showDarkAshStatus: true));
		List<MapAreaItem> dataArray32 = _dataArray;
		sbyte[] worldMapPos32 = new sbyte[2] { 12, 14 };
		AreaTravelRoute[] neighborAreas32 = new AreaTravelRoute[3]
		{
			new AreaTravelRoute(35, 6, null),
			new AreaTravelRoute(56, 24, new int[4] { 11, 14, 10, 14 }),
			new AreaTravelRoute(63, 32, new int[6] { 11, 14, 10, 14, 10, 15 })
		};
		string[] miniMapIcons32 = new string[4] { "largemap_part_2_icon_hei_2", "largemap_part_2_icon_jin_2", "largemap_part_2_icon_lan_2", "largemap_part_2_icon_hui_2" };
		short[] miniMapPos32 = new short[2] { 110, 60 };
		List<EnemyNestCreationInfo[]> enemyNests32 = new List<EnemyNestCreationInfo[]>
		{
			new EnemyNestCreationInfo[2]
			{
				new EnemyNestCreationInfo(0, 2),
				new EnemyNestCreationInfo(1, 2)
			},
			new EnemyNestCreationInfo[2]
			{
				new EnemyNestCreationInfo(1, 2),
				new EnemyNestCreationInfo(2, 3)
			},
			new EnemyNestCreationInfo[4]
			{
				new EnemyNestCreationInfo(2, 3),
				new EnemyNestCreationInfo(3, 3),
				new EnemyNestCreationInfo(4, 4),
				new EnemyNestCreationInfo(5, 4)
			},
			new EnemyNestCreationInfo[1]
			{
				new EnemyNestCreationInfo(12, 2)
			}
		};
		List<IntPair> enemyNestCreationDateRanges32 = new List<IntPair>
		{
			new IntPair(0, 6),
			new IntPair(6, 12),
			new IntPair(12, 24),
			new IntPair(0, 0)
		};
		short[] settlementBlockCore32 = new short[1] { 35 };
		sbyte[] organizationId32 = new sbyte[1] { 37 };
		List<short[]> developedBlockCore32 = new List<short[]>
		{
			new short[2] { 39, 33 },
			new short[2] { 40, 33 },
			new short[2] { 41, 33 },
			new short[2] { 42, 33 },
			new short[2] { 43, 33 },
			new short[2] { 44, 33 },
			new short[2] { 45, 33 },
			new short[2] { 46, 33 },
			new short[2] { 47, 33 },
			new short[2] { 48, 33 },
			new short[2] { 49, 33 },
			new short[2] { 50, 33 },
			new short[2] { 51, 33 },
			new short[2] { 52, 33 },
			new short[2] { 53, 33 },
			new short[2] { 54, 33 },
			new short[2] { 55, 33 },
			new short[2] { 56, 33 },
			new short[2] { 109, 10 },
			new short[2] { 110, 10 },
			new short[2] { 111, 10 }
		};
		List<short[]> normalBlockCore32 = new List<short[]>
		{
			new short[2] { 57, 33 },
			new short[2] { 58, 33 },
			new short[2] { 59, 33 },
			new short[2] { 63, 33 },
			new short[2] { 64, 33 },
			new short[2] { 65, 33 },
			new short[2] { 69, 33 },
			new short[2] { 70, 33 },
			new short[2] { 71, 33 },
			new short[2] { 75, 33 },
			new short[2] { 76, 33 },
			new short[2] { 77, 33 },
			new short[2] { 81, 33 },
			new short[2] { 82, 33 },
			new short[2] { 83, 33 },
			new short[2] { 87, 33 },
			new short[2] { 88, 33 },
			new short[2] { 89, 33 },
			new short[2] { 112, 30 },
			new short[2] { 113, 30 },
			new short[2] { 114, 30 },
			new short[2] { 124, 30 }
		};
		List<short[]> wildBlockCore32 = new List<short[]>
		{
			new short[2] { 93, 33 },
			new short[2] { 94, 33 },
			new short[2] { 95, 33 },
			new short[2] { 96, 33 },
			new short[2] { 97, 33 },
			new short[2] { 98, 33 },
			new short[2] { 99, 33 },
			new short[2] { 100, 33 },
			new short[2] { 101, 33 },
			new short[2] { 102, 33 },
			new short[2] { 103, 33 },
			new short[2] { 104, 33 },
			new short[2] { 105, 33 },
			new short[2] { 106, 33 },
			new short[2] { 107, 33 },
			new short[2] { 108, 33 },
			new short[2] { 115, 40 },
			new short[2] { 116, 40 },
			new short[2] { 117, 40 },
			new short[2] { 124, 30 }
		};
		List<short[]> bigBaseBlockCore32 = new List<short[]>
		{
			new short[2] { 60, 1 },
			new short[2] { 79, 1 },
			new short[2] { 80, 1 },
			new short[2] { 84, 1 },
			new short[2] { 66, 1 }
		};
		list3 = new List<short[]>();
		List<short[]> seriesBlockCore32 = list3;
		list3 = new List<short[]>();
		List<short[]> encircleBlockCore32 = list3;
		short[] sceneryBlockCore32 = new short[0];
		list4 = new List<short>();
		List<short> lovingItemSubTypes32 = list4;
		list4 = new List<short>();
		dataArray32.Add(new MapAreaItem(31, 1, 144, 145, 20, worldMapPos32, neighborAreas32, "largemap_part_1_changgui_2", miniMapIcons32, null, miniMapPos32, 0, 0, 5, enemyNests32, enemyNestCreationDateRanges32, settlementBlockCore32, organizationId32, -1, 0, null, developedBlockCore32, normalBlockCore32, wildBlockCore32, bigBaseBlockCore32, seriesBlockCore32, encircleBlockCore32, sceneryBlockCore32, 146, 147, null, null, EMapAreaAreaDirection.South, lovingItemSubTypes32, list4, showDarkAshStatus: true));
		List<MapAreaItem> dataArray33 = _dataArray;
		sbyte[] worldMapPos33 = new sbyte[2] { 15, 17 };
		AreaTravelRoute[] neighborAreas33 = new AreaTravelRoute[1]
		{
			new AreaTravelRoute(36, 22, new int[4] { 15, 18, 15, 19 })
		};
		string[] miniMapIcons33 = new string[4] { "largemap_part_2_icon_hei_2", "largemap_part_2_icon_jin_2", "largemap_part_2_icon_lan_2", "largemap_part_2_icon_hui_2" };
		short[] miniMapPos33 = new short[2] { 210, 170 };
		List<EnemyNestCreationInfo[]> enemyNests33 = new List<EnemyNestCreationInfo[]>
		{
			new EnemyNestCreationInfo[2]
			{
				new EnemyNestCreationInfo(0, 2),
				new EnemyNestCreationInfo(1, 2)
			},
			new EnemyNestCreationInfo[2]
			{
				new EnemyNestCreationInfo(1, 2),
				new EnemyNestCreationInfo(2, 3)
			},
			new EnemyNestCreationInfo[4]
			{
				new EnemyNestCreationInfo(2, 3),
				new EnemyNestCreationInfo(3, 3),
				new EnemyNestCreationInfo(4, 4),
				new EnemyNestCreationInfo(5, 4)
			},
			new EnemyNestCreationInfo[1]
			{
				new EnemyNestCreationInfo(12, 2)
			}
		};
		List<IntPair> enemyNestCreationDateRanges33 = new List<IntPair>
		{
			new IntPair(0, 6),
			new IntPair(6, 12),
			new IntPair(12, 24),
			new IntPair(0, 0)
		};
		short[] settlementBlockCore33 = new short[1] { 36 };
		sbyte[] organizationId33 = new sbyte[1] { 38 };
		List<short[]> developedBlockCore33 = new List<short[]>
		{
			new short[2] { 39, 33 },
			new short[2] { 40, 33 },
			new short[2] { 41, 33 },
			new short[2] { 42, 33 },
			new short[2] { 43, 33 },
			new short[2] { 44, 33 },
			new short[2] { 45, 33 },
			new short[2] { 46, 33 },
			new short[2] { 47, 33 },
			new short[2] { 48, 33 },
			new short[2] { 49, 33 },
			new short[2] { 50, 33 },
			new short[2] { 51, 33 },
			new short[2] { 52, 33 },
			new short[2] { 53, 33 },
			new short[2] { 54, 33 },
			new short[2] { 55, 33 },
			new short[2] { 56, 33 },
			new short[2] { 109, 10 },
			new short[2] { 110, 10 },
			new short[2] { 111, 10 }
		};
		List<short[]> normalBlockCore33 = new List<short[]>
		{
			new short[2] { 57, 33 },
			new short[2] { 58, 33 },
			new short[2] { 59, 33 },
			new short[2] { 63, 33 },
			new short[2] { 64, 33 },
			new short[2] { 65, 33 },
			new short[2] { 69, 33 },
			new short[2] { 70, 33 },
			new short[2] { 71, 33 },
			new short[2] { 75, 33 },
			new short[2] { 76, 33 },
			new short[2] { 77, 33 },
			new short[2] { 81, 33 },
			new short[2] { 82, 33 },
			new short[2] { 83, 33 },
			new short[2] { 87, 33 },
			new short[2] { 88, 33 },
			new short[2] { 89, 33 },
			new short[2] { 112, 30 },
			new short[2] { 113, 30 },
			new short[2] { 114, 30 },
			new short[2] { 124, 30 }
		};
		List<short[]> wildBlockCore33 = new List<short[]>
		{
			new short[2] { 93, 33 },
			new short[2] { 94, 33 },
			new short[2] { 95, 33 },
			new short[2] { 96, 33 },
			new short[2] { 97, 33 },
			new short[2] { 98, 33 },
			new short[2] { 99, 33 },
			new short[2] { 100, 33 },
			new short[2] { 101, 33 },
			new short[2] { 102, 33 },
			new short[2] { 103, 33 },
			new short[2] { 104, 33 },
			new short[2] { 105, 33 },
			new short[2] { 106, 33 },
			new short[2] { 107, 33 },
			new short[2] { 108, 33 },
			new short[2] { 115, 40 },
			new short[2] { 116, 40 },
			new short[2] { 117, 40 },
			new short[2] { 124, 30 }
		};
		List<short[]> bigBaseBlockCore33 = new List<short[]>
		{
			new short[2] { 60, 1 },
			new short[2] { 79, 1 },
			new short[2] { 80, 1 },
			new short[2] { 84, 1 },
			new short[2] { 66, 1 }
		};
		list3 = new List<short[]>();
		List<short[]> seriesBlockCore33 = list3;
		list3 = new List<short[]>();
		List<short[]> encircleBlockCore33 = list3;
		short[] sceneryBlockCore33 = new short[0];
		list4 = new List<short>();
		List<short> lovingItemSubTypes33 = list4;
		list4 = new List<short>();
		dataArray33.Add(new MapAreaItem(32, 1, 148, 149, 20, worldMapPos33, neighborAreas33, "largemap_part_1_changgui_2", miniMapIcons33, null, miniMapPos33, 0, 0, 5, enemyNests33, enemyNestCreationDateRanges33, settlementBlockCore33, organizationId33, -1, 0, null, developedBlockCore33, normalBlockCore33, wildBlockCore33, bigBaseBlockCore33, seriesBlockCore33, encircleBlockCore33, sceneryBlockCore33, 150, 151, null, null, EMapAreaAreaDirection.South, lovingItemSubTypes33, list4, showDarkAshStatus: true));
		List<MapAreaItem> dataArray34 = _dataArray;
		sbyte[] worldMapPos34 = new sbyte[2] { 14, 20 };
		AreaTravelRoute[] neighborAreas34 = new AreaTravelRoute[2]
		{
			new AreaTravelRoute(36, 6, null),
			new AreaTravelRoute(61, 12, new int[2] { 13, 20 })
		};
		string[] miniMapIcons34 = new string[4] { "largemap_part_2_icon_hei_0", "largemap_part_2_icon_jin_0", "largemap_part_2_icon_lan_0", "largemap_part_2_icon_hui_0" };
		short[] miniMapPos34 = new short[2] { 180, 260 };
		List<EnemyNestCreationInfo[]> enemyNests34 = new List<EnemyNestCreationInfo[]>
		{
			new EnemyNestCreationInfo[2]
			{
				new EnemyNestCreationInfo(0, 2),
				new EnemyNestCreationInfo(1, 2)
			},
			new EnemyNestCreationInfo[2]
			{
				new EnemyNestCreationInfo(1, 2),
				new EnemyNestCreationInfo(2, 3)
			},
			new EnemyNestCreationInfo[4]
			{
				new EnemyNestCreationInfo(2, 3),
				new EnemyNestCreationInfo(3, 3),
				new EnemyNestCreationInfo(4, 4),
				new EnemyNestCreationInfo(5, 4)
			},
			new EnemyNestCreationInfo[1]
			{
				new EnemyNestCreationInfo(12, 2)
			}
		};
		List<IntPair> enemyNestCreationDateRanges34 = new List<IntPair>
		{
			new IntPair(0, 6),
			new IntPair(6, 12),
			new IntPair(12, 24),
			new IntPair(0, 0)
		};
		short[] settlementBlockCore34 = new short[1] { 35 };
		sbyte[] organizationId34 = new sbyte[1] { 37 };
		List<short[]> developedBlockCore34 = new List<short[]>
		{
			new short[2] { 39, 33 },
			new short[2] { 40, 33 },
			new short[2] { 41, 33 },
			new short[2] { 42, 33 },
			new short[2] { 43, 33 },
			new short[2] { 44, 33 },
			new short[2] { 45, 33 },
			new short[2] { 46, 33 },
			new short[2] { 47, 33 },
			new short[2] { 48, 33 },
			new short[2] { 49, 33 },
			new short[2] { 50, 33 },
			new short[2] { 51, 33 },
			new short[2] { 52, 33 },
			new short[2] { 53, 33 },
			new short[2] { 54, 33 },
			new short[2] { 55, 33 },
			new short[2] { 56, 33 },
			new short[2] { 109, 10 },
			new short[2] { 110, 10 },
			new short[2] { 111, 10 }
		};
		List<short[]> normalBlockCore34 = new List<short[]>
		{
			new short[2] { 57, 33 },
			new short[2] { 58, 33 },
			new short[2] { 59, 33 },
			new short[2] { 63, 33 },
			new short[2] { 64, 33 },
			new short[2] { 65, 33 },
			new short[2] { 69, 33 },
			new short[2] { 70, 33 },
			new short[2] { 71, 33 },
			new short[2] { 75, 33 },
			new short[2] { 76, 33 },
			new short[2] { 77, 33 },
			new short[2] { 81, 33 },
			new short[2] { 82, 33 },
			new short[2] { 83, 33 },
			new short[2] { 87, 33 },
			new short[2] { 88, 33 },
			new short[2] { 89, 33 },
			new short[2] { 112, 30 },
			new short[2] { 113, 30 },
			new short[2] { 114, 30 },
			new short[2] { 124, 30 }
		};
		List<short[]> wildBlockCore34 = new List<short[]>
		{
			new short[2] { 93, 33 },
			new short[2] { 94, 33 },
			new short[2] { 95, 33 },
			new short[2] { 96, 33 },
			new short[2] { 97, 33 },
			new short[2] { 98, 33 },
			new short[2] { 99, 33 },
			new short[2] { 100, 33 },
			new short[2] { 101, 33 },
			new short[2] { 102, 33 },
			new short[2] { 103, 33 },
			new short[2] { 104, 33 },
			new short[2] { 105, 33 },
			new short[2] { 106, 33 },
			new short[2] { 107, 33 },
			new short[2] { 108, 33 },
			new short[2] { 115, 40 },
			new short[2] { 116, 40 },
			new short[2] { 117, 40 },
			new short[2] { 124, 30 }
		};
		List<short[]> bigBaseBlockCore34 = new List<short[]>
		{
			new short[2] { 60, 1 },
			new short[2] { 79, 1 },
			new short[2] { 80, 1 },
			new short[2] { 84, 1 },
			new short[2] { 66, 1 }
		};
		list3 = new List<short[]>();
		List<short[]> seriesBlockCore34 = list3;
		list3 = new List<short[]>();
		List<short[]> encircleBlockCore34 = list3;
		short[] sceneryBlockCore34 = new short[0];
		list4 = new List<short>();
		List<short> lovingItemSubTypes34 = list4;
		list4 = new List<short>();
		dataArray34.Add(new MapAreaItem(33, 1, 152, 153, 20, worldMapPos34, neighborAreas34, "largemap_part_1_changgui_0", miniMapIcons34, null, miniMapPos34, 0, 0, 5, enemyNests34, enemyNestCreationDateRanges34, settlementBlockCore34, organizationId34, -1, 0, null, developedBlockCore34, normalBlockCore34, wildBlockCore34, bigBaseBlockCore34, seriesBlockCore34, encircleBlockCore34, sceneryBlockCore34, 154, 155, null, null, EMapAreaAreaDirection.South, lovingItemSubTypes34, list4, showDarkAshStatus: true));
		List<MapAreaItem> dataArray35 = _dataArray;
		sbyte[] worldMapPos35 = new sbyte[2] { 13, 16 };
		AreaTravelRoute[] neighborAreas35 = new AreaTravelRoute[1]
		{
			new AreaTravelRoute(35, 16, new int[2] { 12, 16 })
		};
		string[] miniMapIcons35 = new string[4] { "largemap_part_2_icon_hei_0", "largemap_part_2_icon_jin_0", "largemap_part_2_icon_lan_0", "largemap_part_2_icon_hui_0" };
		short[] miniMapPos35 = new short[2] { 140, 140 };
		List<EnemyNestCreationInfo[]> enemyNests35 = new List<EnemyNestCreationInfo[]>
		{
			new EnemyNestCreationInfo[2]
			{
				new EnemyNestCreationInfo(0, 2),
				new EnemyNestCreationInfo(1, 2)
			},
			new EnemyNestCreationInfo[2]
			{
				new EnemyNestCreationInfo(1, 2),
				new EnemyNestCreationInfo(2, 3)
			},
			new EnemyNestCreationInfo[4]
			{
				new EnemyNestCreationInfo(2, 3),
				new EnemyNestCreationInfo(3, 3),
				new EnemyNestCreationInfo(4, 4),
				new EnemyNestCreationInfo(5, 4)
			},
			new EnemyNestCreationInfo[1]
			{
				new EnemyNestCreationInfo(12, 2)
			}
		};
		List<IntPair> enemyNestCreationDateRanges35 = new List<IntPair>
		{
			new IntPair(0, 6),
			new IntPair(6, 12),
			new IntPair(12, 24),
			new IntPair(0, 0)
		};
		short[] settlementBlockCore35 = new short[1] { 36 };
		sbyte[] organizationId35 = new sbyte[1] { 38 };
		List<short[]> developedBlockCore35 = new List<short[]>
		{
			new short[2] { 39, 33 },
			new short[2] { 40, 33 },
			new short[2] { 41, 33 },
			new short[2] { 42, 33 },
			new short[2] { 43, 33 },
			new short[2] { 44, 33 },
			new short[2] { 45, 33 },
			new short[2] { 46, 33 },
			new short[2] { 47, 33 },
			new short[2] { 48, 33 },
			new short[2] { 49, 33 },
			new short[2] { 50, 33 },
			new short[2] { 51, 33 },
			new short[2] { 52, 33 },
			new short[2] { 53, 33 },
			new short[2] { 54, 33 },
			new short[2] { 55, 33 },
			new short[2] { 56, 33 },
			new short[2] { 109, 10 },
			new short[2] { 110, 10 },
			new short[2] { 111, 10 }
		};
		List<short[]> normalBlockCore35 = new List<short[]>
		{
			new short[2] { 57, 33 },
			new short[2] { 58, 33 },
			new short[2] { 59, 33 },
			new short[2] { 63, 33 },
			new short[2] { 64, 33 },
			new short[2] { 65, 33 },
			new short[2] { 69, 33 },
			new short[2] { 70, 33 },
			new short[2] { 71, 33 },
			new short[2] { 75, 33 },
			new short[2] { 76, 33 },
			new short[2] { 77, 33 },
			new short[2] { 81, 33 },
			new short[2] { 82, 33 },
			new short[2] { 83, 33 },
			new short[2] { 87, 33 },
			new short[2] { 88, 33 },
			new short[2] { 89, 33 },
			new short[2] { 112, 30 },
			new short[2] { 113, 30 },
			new short[2] { 114, 30 },
			new short[2] { 124, 30 }
		};
		List<short[]> wildBlockCore35 = new List<short[]>
		{
			new short[2] { 93, 33 },
			new short[2] { 94, 33 },
			new short[2] { 95, 33 },
			new short[2] { 96, 33 },
			new short[2] { 97, 33 },
			new short[2] { 98, 33 },
			new short[2] { 99, 33 },
			new short[2] { 100, 33 },
			new short[2] { 101, 33 },
			new short[2] { 102, 33 },
			new short[2] { 103, 33 },
			new short[2] { 104, 33 },
			new short[2] { 105, 33 },
			new short[2] { 106, 33 },
			new short[2] { 107, 33 },
			new short[2] { 108, 33 },
			new short[2] { 115, 40 },
			new short[2] { 116, 40 },
			new short[2] { 117, 40 },
			new short[2] { 124, 30 }
		};
		List<short[]> bigBaseBlockCore35 = new List<short[]>
		{
			new short[2] { 60, 1 },
			new short[2] { 79, 1 },
			new short[2] { 80, 1 },
			new short[2] { 84, 1 },
			new short[2] { 66, 1 }
		};
		list3 = new List<short[]>();
		List<short[]> seriesBlockCore35 = list3;
		list3 = new List<short[]>();
		List<short[]> encircleBlockCore35 = list3;
		short[] sceneryBlockCore35 = new short[0];
		list4 = new List<short>();
		List<short> lovingItemSubTypes35 = list4;
		list4 = new List<short>();
		dataArray35.Add(new MapAreaItem(34, 1, 156, 157, 20, worldMapPos35, neighborAreas35, "largemap_part_1_changgui_0", miniMapIcons35, null, miniMapPos35, 0, 0, 5, enemyNests35, enemyNestCreationDateRanges35, settlementBlockCore35, organizationId35, -1, 0, null, developedBlockCore35, normalBlockCore35, wildBlockCore35, bigBaseBlockCore35, seriesBlockCore35, encircleBlockCore35, sceneryBlockCore35, 158, 159, null, null, EMapAreaAreaDirection.South, lovingItemSubTypes35, list4, showDarkAshStatus: true));
		List<MapAreaItem> dataArray36 = _dataArray;
		sbyte[] worldMapPos36 = new sbyte[2] { 12, 15 };
		AreaTravelRoute[] neighborAreas36 = new AreaTravelRoute[0];
		string[] miniMapIcons36 = new string[4] { "largemap_part_2_icon_hei_0", "largemap_part_2_icon_jin_0", "largemap_part_2_icon_lan_0", "largemap_part_2_icon_hui_0" };
		short[] miniMapPos36 = new short[2] { 110, 100 };
		List<EnemyNestCreationInfo[]> enemyNests36 = new List<EnemyNestCreationInfo[]>
		{
			new EnemyNestCreationInfo[2]
			{
				new EnemyNestCreationInfo(0, 2),
				new EnemyNestCreationInfo(1, 2)
			},
			new EnemyNestCreationInfo[2]
			{
				new EnemyNestCreationInfo(1, 2),
				new EnemyNestCreationInfo(2, 3)
			},
			new EnemyNestCreationInfo[4]
			{
				new EnemyNestCreationInfo(2, 3),
				new EnemyNestCreationInfo(3, 3),
				new EnemyNestCreationInfo(4, 4),
				new EnemyNestCreationInfo(5, 4)
			},
			new EnemyNestCreationInfo[1]
			{
				new EnemyNestCreationInfo(12, 2)
			}
		};
		List<IntPair> enemyNestCreationDateRanges36 = new List<IntPair>
		{
			new IntPair(0, 6),
			new IntPair(6, 12),
			new IntPair(12, 24),
			new IntPair(0, 0)
		};
		short[] settlementBlockCore36 = new short[1] { 36 };
		sbyte[] organizationId36 = new sbyte[1] { 38 };
		List<short[]> developedBlockCore36 = new List<short[]>
		{
			new short[2] { 39, 33 },
			new short[2] { 40, 33 },
			new short[2] { 41, 33 },
			new short[2] { 42, 33 },
			new short[2] { 43, 33 },
			new short[2] { 44, 33 },
			new short[2] { 45, 33 },
			new short[2] { 46, 33 },
			new short[2] { 47, 33 },
			new short[2] { 48, 33 },
			new short[2] { 49, 33 },
			new short[2] { 50, 33 },
			new short[2] { 51, 33 },
			new short[2] { 52, 33 },
			new short[2] { 53, 33 },
			new short[2] { 54, 33 },
			new short[2] { 55, 33 },
			new short[2] { 56, 33 },
			new short[2] { 109, 10 },
			new short[2] { 110, 10 },
			new short[2] { 111, 10 }
		};
		List<short[]> normalBlockCore36 = new List<short[]>
		{
			new short[2] { 57, 33 },
			new short[2] { 58, 33 },
			new short[2] { 59, 33 },
			new short[2] { 63, 33 },
			new short[2] { 64, 33 },
			new short[2] { 65, 33 },
			new short[2] { 69, 33 },
			new short[2] { 70, 33 },
			new short[2] { 71, 33 },
			new short[2] { 75, 33 },
			new short[2] { 76, 33 },
			new short[2] { 77, 33 },
			new short[2] { 81, 33 },
			new short[2] { 82, 33 },
			new short[2] { 83, 33 },
			new short[2] { 87, 33 },
			new short[2] { 88, 33 },
			new short[2] { 89, 33 },
			new short[2] { 112, 30 },
			new short[2] { 113, 30 },
			new short[2] { 114, 30 },
			new short[2] { 124, 30 }
		};
		List<short[]> wildBlockCore36 = new List<short[]>
		{
			new short[2] { 93, 33 },
			new short[2] { 94, 33 },
			new short[2] { 95, 33 },
			new short[2] { 96, 33 },
			new short[2] { 97, 33 },
			new short[2] { 98, 33 },
			new short[2] { 99, 33 },
			new short[2] { 100, 33 },
			new short[2] { 101, 33 },
			new short[2] { 102, 33 },
			new short[2] { 103, 33 },
			new short[2] { 104, 33 },
			new short[2] { 105, 33 },
			new short[2] { 106, 33 },
			new short[2] { 107, 33 },
			new short[2] { 108, 33 },
			new short[2] { 115, 40 },
			new short[2] { 116, 40 },
			new short[2] { 117, 40 },
			new short[2] { 124, 30 }
		};
		List<short[]> bigBaseBlockCore36 = new List<short[]>
		{
			new short[2] { 60, 1 },
			new short[2] { 79, 1 },
			new short[2] { 80, 1 },
			new short[2] { 84, 1 },
			new short[2] { 66, 1 }
		};
		list3 = new List<short[]>();
		List<short[]> seriesBlockCore36 = list3;
		list3 = new List<short[]>();
		List<short[]> encircleBlockCore36 = list3;
		short[] sceneryBlockCore36 = new short[0];
		list4 = new List<short>();
		List<short> lovingItemSubTypes36 = list4;
		list4 = new List<short>();
		dataArray36.Add(new MapAreaItem(35, 1, 160, 161, 20, worldMapPos36, neighborAreas36, "largemap_part_1_changgui_0", miniMapIcons36, null, miniMapPos36, 0, 0, 5, enemyNests36, enemyNestCreationDateRanges36, settlementBlockCore36, organizationId36, -1, 0, null, developedBlockCore36, normalBlockCore36, wildBlockCore36, bigBaseBlockCore36, seriesBlockCore36, encircleBlockCore36, sceneryBlockCore36, 162, 163, null, null, EMapAreaAreaDirection.South, lovingItemSubTypes36, list4, showDarkAshStatus: true));
		List<MapAreaItem> dataArray37 = _dataArray;
		sbyte[] worldMapPos37 = new sbyte[2] { 15, 20 };
		AreaTravelRoute[] neighborAreas37 = new AreaTravelRoute[2]
		{
			new AreaTravelRoute(75, 6, null),
			new AreaTravelRoute(99, 52, new int[8] { 15, 21, 15, 22, 15, 23, 16, 23 })
		};
		string[] miniMapIcons37 = new string[4] { "largemap_part_2_icon_hei_2", "largemap_part_2_icon_jin_2", "largemap_part_2_icon_lan_2", "largemap_part_2_icon_hui_2" };
		short[] miniMapPos37 = new short[2] { 220, 260 };
		List<EnemyNestCreationInfo[]> enemyNests37 = new List<EnemyNestCreationInfo[]>
		{
			new EnemyNestCreationInfo[2]
			{
				new EnemyNestCreationInfo(0, 2),
				new EnemyNestCreationInfo(1, 2)
			},
			new EnemyNestCreationInfo[2]
			{
				new EnemyNestCreationInfo(1, 2),
				new EnemyNestCreationInfo(2, 3)
			},
			new EnemyNestCreationInfo[4]
			{
				new EnemyNestCreationInfo(2, 3),
				new EnemyNestCreationInfo(3, 3),
				new EnemyNestCreationInfo(4, 4),
				new EnemyNestCreationInfo(5, 4)
			},
			new EnemyNestCreationInfo[1]
			{
				new EnemyNestCreationInfo(12, 2)
			}
		};
		List<IntPair> enemyNestCreationDateRanges37 = new List<IntPair>
		{
			new IntPair(0, 6),
			new IntPair(6, 12),
			new IntPair(12, 24),
			new IntPair(0, 0)
		};
		short[] settlementBlockCore37 = new short[1] { 35 };
		sbyte[] organizationId37 = new sbyte[1] { 37 };
		List<short[]> developedBlockCore37 = new List<short[]>
		{
			new short[2] { 39, 33 },
			new short[2] { 40, 33 },
			new short[2] { 41, 33 },
			new short[2] { 42, 33 },
			new short[2] { 43, 33 },
			new short[2] { 44, 33 },
			new short[2] { 45, 33 },
			new short[2] { 46, 33 },
			new short[2] { 47, 33 },
			new short[2] { 48, 33 },
			new short[2] { 49, 33 },
			new short[2] { 50, 33 },
			new short[2] { 51, 33 },
			new short[2] { 52, 33 },
			new short[2] { 53, 33 },
			new short[2] { 54, 33 },
			new short[2] { 55, 33 },
			new short[2] { 56, 33 },
			new short[2] { 109, 10 },
			new short[2] { 110, 10 },
			new short[2] { 111, 10 }
		};
		List<short[]> normalBlockCore37 = new List<short[]>
		{
			new short[2] { 57, 33 },
			new short[2] { 58, 33 },
			new short[2] { 59, 33 },
			new short[2] { 63, 33 },
			new short[2] { 64, 33 },
			new short[2] { 65, 33 },
			new short[2] { 69, 33 },
			new short[2] { 70, 33 },
			new short[2] { 71, 33 },
			new short[2] { 75, 33 },
			new short[2] { 76, 33 },
			new short[2] { 77, 33 },
			new short[2] { 81, 33 },
			new short[2] { 82, 33 },
			new short[2] { 83, 33 },
			new short[2] { 87, 33 },
			new short[2] { 88, 33 },
			new short[2] { 89, 33 },
			new short[2] { 112, 30 },
			new short[2] { 113, 30 },
			new short[2] { 114, 30 },
			new short[2] { 124, 30 }
		};
		List<short[]> wildBlockCore37 = new List<short[]>
		{
			new short[2] { 93, 33 },
			new short[2] { 94, 33 },
			new short[2] { 95, 33 },
			new short[2] { 96, 33 },
			new short[2] { 97, 33 },
			new short[2] { 98, 33 },
			new short[2] { 99, 33 },
			new short[2] { 100, 33 },
			new short[2] { 101, 33 },
			new short[2] { 102, 33 },
			new short[2] { 103, 33 },
			new short[2] { 104, 33 },
			new short[2] { 105, 33 },
			new short[2] { 106, 33 },
			new short[2] { 107, 33 },
			new short[2] { 108, 33 },
			new short[2] { 115, 40 },
			new short[2] { 116, 40 },
			new short[2] { 117, 40 },
			new short[2] { 124, 30 }
		};
		List<short[]> bigBaseBlockCore37 = new List<short[]>
		{
			new short[2] { 60, 1 },
			new short[2] { 79, 1 },
			new short[2] { 80, 1 },
			new short[2] { 84, 1 },
			new short[2] { 66, 1 }
		};
		list3 = new List<short[]>();
		List<short[]> seriesBlockCore37 = list3;
		list3 = new List<short[]>();
		List<short[]> encircleBlockCore37 = list3;
		short[] sceneryBlockCore37 = new short[0];
		list4 = new List<short>();
		List<short> lovingItemSubTypes37 = list4;
		list4 = new List<short>();
		dataArray37.Add(new MapAreaItem(36, 1, 164, 165, 20, worldMapPos37, neighborAreas37, "largemap_part_1_changgui_2", miniMapIcons37, null, miniMapPos37, 0, 0, 5, enemyNests37, enemyNestCreationDateRanges37, settlementBlockCore37, organizationId37, -1, 0, null, developedBlockCore37, normalBlockCore37, wildBlockCore37, bigBaseBlockCore37, seriesBlockCore37, encircleBlockCore37, sceneryBlockCore37, 166, 167, null, null, EMapAreaAreaDirection.South, lovingItemSubTypes37, list4, showDarkAshStatus: true));
		List<MapAreaItem> dataArray38 = _dataArray;
		sbyte[] worldMapPos38 = new sbyte[2] { 14, 15 };
		AreaTravelRoute[] neighborAreas38 = new AreaTravelRoute[2]
		{
			new AreaTravelRoute(117, 32, new int[6] { 15, 15, 16, 15, 16, 14 }),
			new AreaTravelRoute(132, 32, new int[6] { 15, 15, 16, 15, 17, 15 })
		};
		string[] miniMapIcons38 = new string[4] { "largemap_part_2_icon_hei_1", "largemap_part_2_icon_jin_1", "largemap_part_2_icon_lan_1", "largemap_part_2_icon_hui_1" };
		short[] miniMapPos38 = new short[2] { 170, 100 };
		List<EnemyNestCreationInfo[]> enemyNests38 = new List<EnemyNestCreationInfo[]>
		{
			new EnemyNestCreationInfo[2]
			{
				new EnemyNestCreationInfo(0, 2),
				new EnemyNestCreationInfo(1, 2)
			},
			new EnemyNestCreationInfo[2]
			{
				new EnemyNestCreationInfo(1, 2),
				new EnemyNestCreationInfo(2, 3)
			},
			new EnemyNestCreationInfo[4]
			{
				new EnemyNestCreationInfo(2, 3),
				new EnemyNestCreationInfo(3, 3),
				new EnemyNestCreationInfo(4, 4),
				new EnemyNestCreationInfo(5, 4)
			},
			new EnemyNestCreationInfo[1]
			{
				new EnemyNestCreationInfo(12, 2)
			}
		};
		List<IntPair> enemyNestCreationDateRanges38 = new List<IntPair>
		{
			new IntPair(0, 6),
			new IntPair(6, 12),
			new IntPair(12, 24),
			new IntPair(0, 0)
		};
		short[] settlementBlockCore38 = new short[1] { 34 };
		sbyte[] organizationId38 = new sbyte[1] { 36 };
		List<short[]> developedBlockCore38 = new List<short[]>
		{
			new short[2] { 39, 33 },
			new short[2] { 40, 33 },
			new short[2] { 41, 33 },
			new short[2] { 42, 33 },
			new short[2] { 43, 33 },
			new short[2] { 44, 33 },
			new short[2] { 45, 33 },
			new short[2] { 46, 33 },
			new short[2] { 47, 33 },
			new short[2] { 48, 33 },
			new short[2] { 49, 33 },
			new short[2] { 50, 33 },
			new short[2] { 51, 33 },
			new short[2] { 52, 33 },
			new short[2] { 53, 33 },
			new short[2] { 54, 33 },
			new short[2] { 55, 33 },
			new short[2] { 56, 33 },
			new short[2] { 109, 10 },
			new short[2] { 110, 10 },
			new short[2] { 111, 10 }
		};
		List<short[]> normalBlockCore38 = new List<short[]>
		{
			new short[2] { 57, 33 },
			new short[2] { 58, 33 },
			new short[2] { 59, 33 },
			new short[2] { 63, 33 },
			new short[2] { 64, 33 },
			new short[2] { 65, 33 },
			new short[2] { 69, 33 },
			new short[2] { 70, 33 },
			new short[2] { 71, 33 },
			new short[2] { 75, 33 },
			new short[2] { 76, 33 },
			new short[2] { 77, 33 },
			new short[2] { 81, 33 },
			new short[2] { 82, 33 },
			new short[2] { 83, 33 },
			new short[2] { 87, 33 },
			new short[2] { 88, 33 },
			new short[2] { 89, 33 },
			new short[2] { 112, 30 },
			new short[2] { 113, 30 },
			new short[2] { 114, 30 },
			new short[2] { 124, 30 }
		};
		List<short[]> wildBlockCore38 = new List<short[]>
		{
			new short[2] { 93, 33 },
			new short[2] { 94, 33 },
			new short[2] { 95, 33 },
			new short[2] { 96, 33 },
			new short[2] { 97, 33 },
			new short[2] { 98, 33 },
			new short[2] { 99, 33 },
			new short[2] { 100, 33 },
			new short[2] { 101, 33 },
			new short[2] { 102, 33 },
			new short[2] { 103, 33 },
			new short[2] { 104, 33 },
			new short[2] { 105, 33 },
			new short[2] { 106, 33 },
			new short[2] { 107, 33 },
			new short[2] { 108, 33 },
			new short[2] { 115, 40 },
			new short[2] { 116, 40 },
			new short[2] { 117, 40 },
			new short[2] { 124, 30 }
		};
		List<short[]> bigBaseBlockCore38 = new List<short[]>
		{
			new short[2] { 60, 1 },
			new short[2] { 79, 1 },
			new short[2] { 80, 1 },
			new short[2] { 84, 1 },
			new short[2] { 66, 1 }
		};
		list3 = new List<short[]>();
		List<short[]> seriesBlockCore38 = list3;
		list3 = new List<short[]>();
		List<short[]> encircleBlockCore38 = list3;
		short[] sceneryBlockCore38 = new short[0];
		list4 = new List<short>();
		List<short> lovingItemSubTypes38 = list4;
		list4 = new List<short>();
		dataArray38.Add(new MapAreaItem(37, 1, 168, 169, 20, worldMapPos38, neighborAreas38, "largemap_part_1_changgui_1", miniMapIcons38, null, miniMapPos38, 0, 0, 5, enemyNests38, enemyNestCreationDateRanges38, settlementBlockCore38, organizationId38, -1, 0, null, developedBlockCore38, normalBlockCore38, wildBlockCore38, bigBaseBlockCore38, seriesBlockCore38, encircleBlockCore38, sceneryBlockCore38, 170, 171, null, null, EMapAreaAreaDirection.South, lovingItemSubTypes38, list4, showDarkAshStatus: true));
		List<MapAreaItem> dataArray39 = _dataArray;
		sbyte[] worldMapPos39 = new sbyte[2] { 5, 10 };
		AreaTravelRoute[] neighborAreas39 = new AreaTravelRoute[1]
		{
			new AreaTravelRoute(43, 32, new int[6] { 6, 10, 7, 10, 7, 9 })
		};
		string[] miniMapIcons39 = new string[4] { "largemap_part_2_icon_hei_2", "largemap_part_2_icon_jin_2", "largemap_part_2_icon_lan_2", "largemap_part_2_icon_hui_2" };
		short[] miniMapPos39 = new short[2] { 170, 210 };
		List<EnemyNestCreationInfo[]> enemyNests39 = new List<EnemyNestCreationInfo[]>
		{
			new EnemyNestCreationInfo[2]
			{
				new EnemyNestCreationInfo(0, 2),
				new EnemyNestCreationInfo(1, 2)
			},
			new EnemyNestCreationInfo[2]
			{
				new EnemyNestCreationInfo(1, 2),
				new EnemyNestCreationInfo(2, 3)
			},
			new EnemyNestCreationInfo[4]
			{
				new EnemyNestCreationInfo(2, 3),
				new EnemyNestCreationInfo(3, 3),
				new EnemyNestCreationInfo(4, 4),
				new EnemyNestCreationInfo(5, 4)
			},
			new EnemyNestCreationInfo[1]
			{
				new EnemyNestCreationInfo(12, 2)
			}
		};
		List<IntPair> enemyNestCreationDateRanges39 = new List<IntPair>
		{
			new IntPair(0, 6),
			new IntPair(6, 12),
			new IntPair(12, 24),
			new IntPair(0, 0)
		};
		short[] settlementBlockCore39 = new short[1] { 35 };
		sbyte[] organizationId39 = new sbyte[1] { 37 };
		List<short[]> developedBlockCore39 = new List<short[]>
		{
			new short[2] { 39, 33 },
			new short[2] { 40, 33 },
			new short[2] { 41, 33 },
			new short[2] { 42, 33 },
			new short[2] { 43, 33 },
			new short[2] { 44, 33 },
			new short[2] { 45, 33 },
			new short[2] { 46, 33 },
			new short[2] { 47, 33 },
			new short[2] { 48, 33 },
			new short[2] { 49, 33 },
			new short[2] { 50, 33 },
			new short[2] { 51, 33 },
			new short[2] { 52, 33 },
			new short[2] { 53, 33 },
			new short[2] { 54, 33 },
			new short[2] { 55, 33 },
			new short[2] { 56, 33 },
			new short[2] { 109, 10 },
			new short[2] { 110, 10 },
			new short[2] { 111, 10 }
		};
		List<short[]> normalBlockCore39 = new List<short[]>
		{
			new short[2] { 57, 33 },
			new short[2] { 58, 33 },
			new short[2] { 59, 33 },
			new short[2] { 63, 33 },
			new short[2] { 64, 33 },
			new short[2] { 65, 33 },
			new short[2] { 69, 33 },
			new short[2] { 70, 33 },
			new short[2] { 71, 33 },
			new short[2] { 75, 33 },
			new short[2] { 76, 33 },
			new short[2] { 77, 33 },
			new short[2] { 81, 33 },
			new short[2] { 82, 33 },
			new short[2] { 83, 33 },
			new short[2] { 87, 33 },
			new short[2] { 88, 33 },
			new short[2] { 89, 33 },
			new short[2] { 112, 30 },
			new short[2] { 113, 30 },
			new short[2] { 114, 30 },
			new short[2] { 124, 30 }
		};
		List<short[]> wildBlockCore39 = new List<short[]>
		{
			new short[2] { 93, 33 },
			new short[2] { 94, 33 },
			new short[2] { 95, 33 },
			new short[2] { 96, 33 },
			new short[2] { 97, 33 },
			new short[2] { 98, 33 },
			new short[2] { 99, 33 },
			new short[2] { 100, 33 },
			new short[2] { 101, 33 },
			new short[2] { 102, 33 },
			new short[2] { 103, 33 },
			new short[2] { 104, 33 },
			new short[2] { 105, 33 },
			new short[2] { 106, 33 },
			new short[2] { 107, 33 },
			new short[2] { 108, 33 },
			new short[2] { 115, 40 },
			new short[2] { 116, 40 },
			new short[2] { 117, 40 },
			new short[2] { 124, 30 }
		};
		List<short[]> bigBaseBlockCore39 = new List<short[]>
		{
			new short[2] { 72, 1 },
			new short[2] { 60, 1 },
			new short[2] { 79, 1 },
			new short[2] { 80, 1 },
			new short[2] { 90, 1 }
		};
		list3 = new List<short[]>();
		List<short[]> seriesBlockCore39 = list3;
		list3 = new List<short[]>();
		List<short[]> encircleBlockCore39 = list3;
		short[] sceneryBlockCore39 = new short[0];
		list4 = new List<short>();
		List<short> lovingItemSubTypes39 = list4;
		list4 = new List<short>();
		dataArray39.Add(new MapAreaItem(38, 2, 172, 173, 20, worldMapPos39, neighborAreas39, "largemap_part_1_changgui_2", miniMapIcons39, null, miniMapPos39, 0, 0, 5, enemyNests39, enemyNestCreationDateRanges39, settlementBlockCore39, organizationId39, -1, 0, null, developedBlockCore39, normalBlockCore39, wildBlockCore39, bigBaseBlockCore39, seriesBlockCore39, encircleBlockCore39, sceneryBlockCore39, 174, 175, null, null, EMapAreaAreaDirection.South, lovingItemSubTypes39, list4, showDarkAshStatus: true));
		List<MapAreaItem> dataArray40 = _dataArray;
		sbyte[] worldMapPos40 = new sbyte[2] { 3, 11 };
		AreaTravelRoute[] neighborAreas40 = new AreaTravelRoute[1]
		{
			new AreaTravelRoute(41, 8, null)
		};
		string[] miniMapIcons40 = new string[4] { "largemap_part_2_icon_hei_0", "largemap_part_2_icon_jin_0", "largemap_part_2_icon_lan_0", "largemap_part_2_icon_hui_0" };
		short[] miniMapPos40 = new short[2] { 90, 210 };
		List<EnemyNestCreationInfo[]> enemyNests40 = new List<EnemyNestCreationInfo[]>
		{
			new EnemyNestCreationInfo[2]
			{
				new EnemyNestCreationInfo(0, 2),
				new EnemyNestCreationInfo(1, 2)
			},
			new EnemyNestCreationInfo[2]
			{
				new EnemyNestCreationInfo(1, 2),
				new EnemyNestCreationInfo(2, 3)
			},
			new EnemyNestCreationInfo[4]
			{
				new EnemyNestCreationInfo(2, 3),
				new EnemyNestCreationInfo(3, 3),
				new EnemyNestCreationInfo(4, 4),
				new EnemyNestCreationInfo(5, 4)
			},
			new EnemyNestCreationInfo[1]
			{
				new EnemyNestCreationInfo(12, 2)
			}
		};
		List<IntPair> enemyNestCreationDateRanges40 = new List<IntPair>
		{
			new IntPair(0, 6),
			new IntPair(6, 12),
			new IntPair(12, 24),
			new IntPair(0, 0)
		};
		short[] settlementBlockCore40 = new short[1] { 34 };
		sbyte[] organizationId40 = new sbyte[1] { 36 };
		List<short[]> developedBlockCore40 = new List<short[]>
		{
			new short[2] { 39, 33 },
			new short[2] { 40, 33 },
			new short[2] { 41, 33 },
			new short[2] { 42, 33 },
			new short[2] { 43, 33 },
			new short[2] { 44, 33 },
			new short[2] { 45, 33 },
			new short[2] { 46, 33 },
			new short[2] { 47, 33 },
			new short[2] { 48, 33 },
			new short[2] { 49, 33 },
			new short[2] { 50, 33 },
			new short[2] { 51, 33 },
			new short[2] { 52, 33 },
			new short[2] { 53, 33 },
			new short[2] { 54, 33 },
			new short[2] { 55, 33 },
			new short[2] { 56, 33 },
			new short[2] { 109, 10 },
			new short[2] { 110, 10 },
			new short[2] { 111, 10 }
		};
		List<short[]> normalBlockCore40 = new List<short[]>
		{
			new short[2] { 57, 33 },
			new short[2] { 58, 33 },
			new short[2] { 59, 33 },
			new short[2] { 63, 33 },
			new short[2] { 64, 33 },
			new short[2] { 65, 33 },
			new short[2] { 69, 33 },
			new short[2] { 70, 33 },
			new short[2] { 71, 33 },
			new short[2] { 75, 33 },
			new short[2] { 76, 33 },
			new short[2] { 77, 33 },
			new short[2] { 81, 33 },
			new short[2] { 82, 33 },
			new short[2] { 83, 33 },
			new short[2] { 87, 33 },
			new short[2] { 88, 33 },
			new short[2] { 89, 33 },
			new short[2] { 112, 30 },
			new short[2] { 113, 30 },
			new short[2] { 114, 30 },
			new short[2] { 124, 30 }
		};
		List<short[]> wildBlockCore40 = new List<short[]>
		{
			new short[2] { 93, 33 },
			new short[2] { 94, 33 },
			new short[2] { 95, 33 },
			new short[2] { 96, 33 },
			new short[2] { 97, 33 },
			new short[2] { 98, 33 },
			new short[2] { 99, 33 },
			new short[2] { 100, 33 },
			new short[2] { 101, 33 },
			new short[2] { 102, 33 },
			new short[2] { 103, 33 },
			new short[2] { 104, 33 },
			new short[2] { 105, 33 },
			new short[2] { 106, 33 },
			new short[2] { 107, 33 },
			new short[2] { 108, 33 },
			new short[2] { 115, 40 },
			new short[2] { 116, 40 },
			new short[2] { 117, 40 },
			new short[2] { 124, 30 }
		};
		List<short[]> bigBaseBlockCore40 = new List<short[]>
		{
			new short[2] { 72, 1 },
			new short[2] { 60, 1 },
			new short[2] { 79, 1 },
			new short[2] { 80, 1 },
			new short[2] { 90, 1 }
		};
		list3 = new List<short[]>();
		List<short[]> seriesBlockCore40 = list3;
		list3 = new List<short[]>();
		List<short[]> encircleBlockCore40 = list3;
		short[] sceneryBlockCore40 = new short[0];
		list4 = new List<short>();
		List<short> lovingItemSubTypes40 = list4;
		list4 = new List<short>();
		dataArray40.Add(new MapAreaItem(39, 2, 176, 177, 20, worldMapPos40, neighborAreas40, "largemap_part_1_changgui_0", miniMapIcons40, null, miniMapPos40, 0, 0, 5, enemyNests40, enemyNestCreationDateRanges40, settlementBlockCore40, organizationId40, -1, 0, null, developedBlockCore40, normalBlockCore40, wildBlockCore40, bigBaseBlockCore40, seriesBlockCore40, encircleBlockCore40, sceneryBlockCore40, 178, 179, null, null, EMapAreaAreaDirection.South, lovingItemSubTypes40, list4, showDarkAshStatus: true));
		List<MapAreaItem> dataArray41 = _dataArray;
		sbyte[] worldMapPos41 = new sbyte[2] { 3, 10 };
		AreaTravelRoute[] neighborAreas41 = new AreaTravelRoute[0];
		string[] miniMapIcons41 = new string[4] { "largemap_part_2_icon_hei_0", "largemap_part_2_icon_jin_0", "largemap_part_2_icon_lan_0", "largemap_part_2_icon_hui_0" };
		short[] miniMapPos41 = new short[2] { 90, 170 };
		List<EnemyNestCreationInfo[]> enemyNests41 = new List<EnemyNestCreationInfo[]>
		{
			new EnemyNestCreationInfo[2]
			{
				new EnemyNestCreationInfo(0, 2),
				new EnemyNestCreationInfo(1, 2)
			},
			new EnemyNestCreationInfo[2]
			{
				new EnemyNestCreationInfo(1, 2),
				new EnemyNestCreationInfo(2, 3)
			},
			new EnemyNestCreationInfo[4]
			{
				new EnemyNestCreationInfo(2, 3),
				new EnemyNestCreationInfo(3, 3),
				new EnemyNestCreationInfo(4, 4),
				new EnemyNestCreationInfo(5, 4)
			},
			new EnemyNestCreationInfo[1]
			{
				new EnemyNestCreationInfo(12, 2)
			}
		};
		List<IntPair> enemyNestCreationDateRanges41 = new List<IntPair>
		{
			new IntPair(0, 6),
			new IntPair(6, 12),
			new IntPair(12, 24),
			new IntPair(0, 0)
		};
		short[] settlementBlockCore41 = new short[1] { 36 };
		sbyte[] organizationId41 = new sbyte[1] { 38 };
		List<short[]> developedBlockCore41 = new List<short[]>
		{
			new short[2] { 39, 33 },
			new short[2] { 40, 33 },
			new short[2] { 41, 33 },
			new short[2] { 42, 33 },
			new short[2] { 43, 33 },
			new short[2] { 44, 33 },
			new short[2] { 45, 33 },
			new short[2] { 46, 33 },
			new short[2] { 47, 33 },
			new short[2] { 48, 33 },
			new short[2] { 49, 33 },
			new short[2] { 50, 33 },
			new short[2] { 51, 33 },
			new short[2] { 52, 33 },
			new short[2] { 53, 33 },
			new short[2] { 54, 33 },
			new short[2] { 55, 33 },
			new short[2] { 56, 33 },
			new short[2] { 109, 10 },
			new short[2] { 110, 10 },
			new short[2] { 111, 10 }
		};
		List<short[]> normalBlockCore41 = new List<short[]>
		{
			new short[2] { 57, 33 },
			new short[2] { 58, 33 },
			new short[2] { 59, 33 },
			new short[2] { 63, 33 },
			new short[2] { 64, 33 },
			new short[2] { 65, 33 },
			new short[2] { 69, 33 },
			new short[2] { 70, 33 },
			new short[2] { 71, 33 },
			new short[2] { 75, 33 },
			new short[2] { 76, 33 },
			new short[2] { 77, 33 },
			new short[2] { 81, 33 },
			new short[2] { 82, 33 },
			new short[2] { 83, 33 },
			new short[2] { 87, 33 },
			new short[2] { 88, 33 },
			new short[2] { 89, 33 },
			new short[2] { 112, 30 },
			new short[2] { 113, 30 },
			new short[2] { 114, 30 },
			new short[2] { 124, 30 }
		};
		List<short[]> wildBlockCore41 = new List<short[]>
		{
			new short[2] { 93, 33 },
			new short[2] { 94, 33 },
			new short[2] { 95, 33 },
			new short[2] { 96, 33 },
			new short[2] { 97, 33 },
			new short[2] { 98, 33 },
			new short[2] { 99, 33 },
			new short[2] { 100, 33 },
			new short[2] { 101, 33 },
			new short[2] { 102, 33 },
			new short[2] { 103, 33 },
			new short[2] { 104, 33 },
			new short[2] { 105, 33 },
			new short[2] { 106, 33 },
			new short[2] { 107, 33 },
			new short[2] { 108, 33 },
			new short[2] { 115, 40 },
			new short[2] { 116, 40 },
			new short[2] { 117, 40 },
			new short[2] { 124, 30 }
		};
		List<short[]> bigBaseBlockCore41 = new List<short[]>
		{
			new short[2] { 72, 1 },
			new short[2] { 60, 1 },
			new short[2] { 79, 1 },
			new short[2] { 80, 1 },
			new short[2] { 90, 1 }
		};
		list3 = new List<short[]>();
		List<short[]> seriesBlockCore41 = list3;
		list3 = new List<short[]>();
		List<short[]> encircleBlockCore41 = list3;
		short[] sceneryBlockCore41 = new short[0];
		list4 = new List<short>();
		List<short> lovingItemSubTypes41 = list4;
		list4 = new List<short>();
		dataArray41.Add(new MapAreaItem(40, 2, 180, 181, 20, worldMapPos41, neighborAreas41, "largemap_part_1_changgui_0", miniMapIcons41, null, miniMapPos41, 0, 0, 5, enemyNests41, enemyNestCreationDateRanges41, settlementBlockCore41, organizationId41, -1, 0, null, developedBlockCore41, normalBlockCore41, wildBlockCore41, bigBaseBlockCore41, seriesBlockCore41, encircleBlockCore41, sceneryBlockCore41, 182, 183, null, null, EMapAreaAreaDirection.South, lovingItemSubTypes41, list4, showDarkAshStatus: true));
		List<MapAreaItem> dataArray42 = _dataArray;
		sbyte[] worldMapPos42 = new sbyte[2] { 4, 11 };
		AreaTravelRoute[] neighborAreas42 = new AreaTravelRoute[1]
		{
			new AreaTravelRoute(42, 22, new int[4] { 5, 11, 6, 11 })
		};
		string[] miniMapIcons42 = new string[4] { "largemap_part_2_icon_hei_0", "largemap_part_2_icon_jin_0", "largemap_part_2_icon_lan_0", "largemap_part_2_icon_hui_0" };
		short[] miniMapPos42 = new short[2] { 130, 250 };
		List<EnemyNestCreationInfo[]> enemyNests42 = new List<EnemyNestCreationInfo[]>
		{
			new EnemyNestCreationInfo[2]
			{
				new EnemyNestCreationInfo(0, 2),
				new EnemyNestCreationInfo(1, 2)
			},
			new EnemyNestCreationInfo[2]
			{
				new EnemyNestCreationInfo(1, 2),
				new EnemyNestCreationInfo(2, 3)
			},
			new EnemyNestCreationInfo[4]
			{
				new EnemyNestCreationInfo(2, 3),
				new EnemyNestCreationInfo(3, 3),
				new EnemyNestCreationInfo(4, 4),
				new EnemyNestCreationInfo(5, 4)
			},
			new EnemyNestCreationInfo[1]
			{
				new EnemyNestCreationInfo(12, 2)
			}
		};
		List<IntPair> enemyNestCreationDateRanges42 = new List<IntPair>
		{
			new IntPair(0, 6),
			new IntPair(6, 12),
			new IntPair(12, 24),
			new IntPair(0, 0)
		};
		short[] settlementBlockCore42 = new short[1] { 36 };
		sbyte[] organizationId42 = new sbyte[1] { 38 };
		List<short[]> developedBlockCore42 = new List<short[]>
		{
			new short[2] { 39, 33 },
			new short[2] { 40, 33 },
			new short[2] { 41, 33 },
			new short[2] { 42, 33 },
			new short[2] { 43, 33 },
			new short[2] { 44, 33 },
			new short[2] { 45, 33 },
			new short[2] { 46, 33 },
			new short[2] { 47, 33 },
			new short[2] { 48, 33 },
			new short[2] { 49, 33 },
			new short[2] { 50, 33 },
			new short[2] { 51, 33 },
			new short[2] { 52, 33 },
			new short[2] { 53, 33 },
			new short[2] { 54, 33 },
			new short[2] { 55, 33 },
			new short[2] { 56, 33 },
			new short[2] { 109, 10 },
			new short[2] { 110, 10 },
			new short[2] { 111, 10 }
		};
		List<short[]> normalBlockCore42 = new List<short[]>
		{
			new short[2] { 57, 33 },
			new short[2] { 58, 33 },
			new short[2] { 59, 33 },
			new short[2] { 63, 33 },
			new short[2] { 64, 33 },
			new short[2] { 65, 33 },
			new short[2] { 69, 33 },
			new short[2] { 70, 33 },
			new short[2] { 71, 33 },
			new short[2] { 75, 33 },
			new short[2] { 76, 33 },
			new short[2] { 77, 33 },
			new short[2] { 81, 33 },
			new short[2] { 82, 33 },
			new short[2] { 83, 33 },
			new short[2] { 87, 33 },
			new short[2] { 88, 33 },
			new short[2] { 89, 33 },
			new short[2] { 112, 30 },
			new short[2] { 113, 30 },
			new short[2] { 114, 30 },
			new short[2] { 124, 30 }
		};
		List<short[]> wildBlockCore42 = new List<short[]>
		{
			new short[2] { 93, 33 },
			new short[2] { 94, 33 },
			new short[2] { 95, 33 },
			new short[2] { 96, 33 },
			new short[2] { 97, 33 },
			new short[2] { 98, 33 },
			new short[2] { 99, 33 },
			new short[2] { 100, 33 },
			new short[2] { 101, 33 },
			new short[2] { 102, 33 },
			new short[2] { 103, 33 },
			new short[2] { 104, 33 },
			new short[2] { 105, 33 },
			new short[2] { 106, 33 },
			new short[2] { 107, 33 },
			new short[2] { 108, 33 },
			new short[2] { 115, 40 },
			new short[2] { 116, 40 },
			new short[2] { 117, 40 },
			new short[2] { 124, 30 }
		};
		List<short[]> bigBaseBlockCore42 = new List<short[]>
		{
			new short[2] { 72, 1 },
			new short[2] { 60, 1 },
			new short[2] { 79, 1 },
			new short[2] { 80, 1 },
			new short[2] { 90, 1 }
		};
		list3 = new List<short[]>();
		List<short[]> seriesBlockCore42 = list3;
		list3 = new List<short[]>();
		List<short[]> encircleBlockCore42 = list3;
		short[] sceneryBlockCore42 = new short[0];
		list4 = new List<short>();
		List<short> lovingItemSubTypes42 = list4;
		list4 = new List<short>();
		dataArray42.Add(new MapAreaItem(41, 2, 184, 185, 20, worldMapPos42, neighborAreas42, "largemap_part_1_changgui_0", miniMapIcons42, null, miniMapPos42, 0, 0, 5, enemyNests42, enemyNestCreationDateRanges42, settlementBlockCore42, organizationId42, -1, 0, null, developedBlockCore42, normalBlockCore42, wildBlockCore42, bigBaseBlockCore42, seriesBlockCore42, encircleBlockCore42, sceneryBlockCore42, 186, 187, null, null, EMapAreaAreaDirection.South, lovingItemSubTypes42, list4, showDarkAshStatus: true));
		List<MapAreaItem> dataArray43 = _dataArray;
		sbyte[] worldMapPos43 = new sbyte[2] { 6, 12 };
		AreaTravelRoute[] neighborAreas43 = new AreaTravelRoute[1]
		{
			new AreaTravelRoute(53, 32, new int[6] { 7, 12, 8, 12, 9, 12 })
		};
		string[] miniMapIcons43 = new string[4] { "largemap_part_2_icon_hei_1", "largemap_part_2_icon_jin_1", "largemap_part_2_icon_lan_1", "largemap_part_2_icon_hui_1" };
		short[] miniMapPos43 = new short[2] { 210, 250 };
		List<EnemyNestCreationInfo[]> enemyNests43 = new List<EnemyNestCreationInfo[]>
		{
			new EnemyNestCreationInfo[2]
			{
				new EnemyNestCreationInfo(0, 2),
				new EnemyNestCreationInfo(1, 2)
			},
			new EnemyNestCreationInfo[2]
			{
				new EnemyNestCreationInfo(1, 2),
				new EnemyNestCreationInfo(2, 3)
			},
			new EnemyNestCreationInfo[4]
			{
				new EnemyNestCreationInfo(2, 3),
				new EnemyNestCreationInfo(3, 3),
				new EnemyNestCreationInfo(4, 4),
				new EnemyNestCreationInfo(5, 4)
			},
			new EnemyNestCreationInfo[1]
			{
				new EnemyNestCreationInfo(12, 2)
			}
		};
		List<IntPair> enemyNestCreationDateRanges43 = new List<IntPair>
		{
			new IntPair(0, 6),
			new IntPair(6, 12),
			new IntPair(12, 24),
			new IntPair(0, 0)
		};
		short[] settlementBlockCore43 = new short[1] { 35 };
		sbyte[] organizationId43 = new sbyte[1] { 37 };
		List<short[]> developedBlockCore43 = new List<short[]>
		{
			new short[2] { 39, 33 },
			new short[2] { 40, 33 },
			new short[2] { 41, 33 },
			new short[2] { 42, 33 },
			new short[2] { 43, 33 },
			new short[2] { 44, 33 },
			new short[2] { 45, 33 },
			new short[2] { 46, 33 },
			new short[2] { 47, 33 },
			new short[2] { 48, 33 },
			new short[2] { 49, 33 },
			new short[2] { 50, 33 },
			new short[2] { 51, 33 },
			new short[2] { 52, 33 },
			new short[2] { 53, 33 },
			new short[2] { 54, 33 },
			new short[2] { 55, 33 },
			new short[2] { 56, 33 },
			new short[2] { 109, 10 },
			new short[2] { 110, 10 },
			new short[2] { 111, 10 }
		};
		List<short[]> normalBlockCore43 = new List<short[]>
		{
			new short[2] { 57, 33 },
			new short[2] { 58, 33 },
			new short[2] { 59, 33 },
			new short[2] { 63, 33 },
			new short[2] { 64, 33 },
			new short[2] { 65, 33 },
			new short[2] { 69, 33 },
			new short[2] { 70, 33 },
			new short[2] { 71, 33 },
			new short[2] { 75, 33 },
			new short[2] { 76, 33 },
			new short[2] { 77, 33 },
			new short[2] { 81, 33 },
			new short[2] { 82, 33 },
			new short[2] { 83, 33 },
			new short[2] { 87, 33 },
			new short[2] { 88, 33 },
			new short[2] { 89, 33 },
			new short[2] { 112, 30 },
			new short[2] { 113, 30 },
			new short[2] { 114, 30 },
			new short[2] { 124, 30 }
		};
		List<short[]> wildBlockCore43 = new List<short[]>
		{
			new short[2] { 93, 33 },
			new short[2] { 94, 33 },
			new short[2] { 95, 33 },
			new short[2] { 96, 33 },
			new short[2] { 97, 33 },
			new short[2] { 98, 33 },
			new short[2] { 99, 33 },
			new short[2] { 100, 33 },
			new short[2] { 101, 33 },
			new short[2] { 102, 33 },
			new short[2] { 103, 33 },
			new short[2] { 104, 33 },
			new short[2] { 105, 33 },
			new short[2] { 106, 33 },
			new short[2] { 107, 33 },
			new short[2] { 108, 33 },
			new short[2] { 115, 40 },
			new short[2] { 116, 40 },
			new short[2] { 117, 40 },
			new short[2] { 124, 30 }
		};
		List<short[]> bigBaseBlockCore43 = new List<short[]>
		{
			new short[2] { 72, 1 },
			new short[2] { 60, 1 },
			new short[2] { 79, 1 },
			new short[2] { 80, 1 },
			new short[2] { 90, 1 }
		};
		list3 = new List<short[]>();
		List<short[]> seriesBlockCore43 = list3;
		list3 = new List<short[]>();
		List<short[]> encircleBlockCore43 = list3;
		short[] sceneryBlockCore43 = new short[0];
		list4 = new List<short>();
		List<short> lovingItemSubTypes43 = list4;
		list4 = new List<short>();
		dataArray43.Add(new MapAreaItem(42, 2, 188, 189, 20, worldMapPos43, neighborAreas43, "largemap_part_1_changgui_1", miniMapIcons43, null, miniMapPos43, 0, 0, 5, enemyNests43, enemyNestCreationDateRanges43, settlementBlockCore43, organizationId43, -1, 0, null, developedBlockCore43, normalBlockCore43, wildBlockCore43, bigBaseBlockCore43, seriesBlockCore43, encircleBlockCore43, sceneryBlockCore43, 190, 191, null, null, EMapAreaAreaDirection.South, lovingItemSubTypes43, list4, showDarkAshStatus: true));
		List<MapAreaItem> dataArray44 = _dataArray;
		sbyte[] worldMapPos44 = new sbyte[2] { 7, 8 };
		AreaTravelRoute[] neighborAreas44 = new AreaTravelRoute[2]
		{
			new AreaTravelRoute(46, 42, new int[8] { 7, 7, 7, 6, 7, 5, 7, 4 }),
			new AreaTravelRoute(86, 22, new int[4] { 8, 8, 9, 8 })
		};
		string[] miniMapIcons44 = new string[4] { "largemap_part_2_icon_hei_1", "largemap_part_2_icon_jin_1", "largemap_part_2_icon_lan_1", "largemap_part_2_icon_hui_1" };
		short[] miniMapPos44 = new short[2] { 250, 130 };
		List<EnemyNestCreationInfo[]> enemyNests44 = new List<EnemyNestCreationInfo[]>
		{
			new EnemyNestCreationInfo[2]
			{
				new EnemyNestCreationInfo(0, 2),
				new EnemyNestCreationInfo(1, 2)
			},
			new EnemyNestCreationInfo[2]
			{
				new EnemyNestCreationInfo(1, 2),
				new EnemyNestCreationInfo(2, 3)
			},
			new EnemyNestCreationInfo[4]
			{
				new EnemyNestCreationInfo(2, 3),
				new EnemyNestCreationInfo(3, 3),
				new EnemyNestCreationInfo(4, 4),
				new EnemyNestCreationInfo(5, 4)
			},
			new EnemyNestCreationInfo[1]
			{
				new EnemyNestCreationInfo(12, 2)
			}
		};
		List<IntPair> enemyNestCreationDateRanges44 = new List<IntPair>
		{
			new IntPair(0, 6),
			new IntPair(6, 12),
			new IntPair(12, 24),
			new IntPair(0, 0)
		};
		short[] settlementBlockCore44 = new short[1] { 34 };
		sbyte[] organizationId44 = new sbyte[1] { 36 };
		List<short[]> developedBlockCore44 = new List<short[]>
		{
			new short[2] { 39, 33 },
			new short[2] { 40, 33 },
			new short[2] { 41, 33 },
			new short[2] { 42, 33 },
			new short[2] { 43, 33 },
			new short[2] { 44, 33 },
			new short[2] { 45, 33 },
			new short[2] { 46, 33 },
			new short[2] { 47, 33 },
			new short[2] { 48, 33 },
			new short[2] { 49, 33 },
			new short[2] { 50, 33 },
			new short[2] { 51, 33 },
			new short[2] { 52, 33 },
			new short[2] { 53, 33 },
			new short[2] { 54, 33 },
			new short[2] { 55, 33 },
			new short[2] { 56, 33 },
			new short[2] { 109, 10 },
			new short[2] { 110, 10 },
			new short[2] { 111, 10 }
		};
		List<short[]> normalBlockCore44 = new List<short[]>
		{
			new short[2] { 57, 33 },
			new short[2] { 58, 33 },
			new short[2] { 59, 33 },
			new short[2] { 63, 33 },
			new short[2] { 64, 33 },
			new short[2] { 65, 33 },
			new short[2] { 69, 33 },
			new short[2] { 70, 33 },
			new short[2] { 71, 33 },
			new short[2] { 75, 33 },
			new short[2] { 76, 33 },
			new short[2] { 77, 33 },
			new short[2] { 81, 33 },
			new short[2] { 82, 33 },
			new short[2] { 83, 33 },
			new short[2] { 87, 33 },
			new short[2] { 88, 33 },
			new short[2] { 89, 33 },
			new short[2] { 112, 30 },
			new short[2] { 113, 30 },
			new short[2] { 114, 30 },
			new short[2] { 124, 30 }
		};
		List<short[]> wildBlockCore44 = new List<short[]>
		{
			new short[2] { 93, 33 },
			new short[2] { 94, 33 },
			new short[2] { 95, 33 },
			new short[2] { 96, 33 },
			new short[2] { 97, 33 },
			new short[2] { 98, 33 },
			new short[2] { 99, 33 },
			new short[2] { 100, 33 },
			new short[2] { 101, 33 },
			new short[2] { 102, 33 },
			new short[2] { 103, 33 },
			new short[2] { 104, 33 },
			new short[2] { 105, 33 },
			new short[2] { 106, 33 },
			new short[2] { 107, 33 },
			new short[2] { 108, 33 },
			new short[2] { 115, 40 },
			new short[2] { 116, 40 },
			new short[2] { 117, 40 },
			new short[2] { 124, 30 }
		};
		List<short[]> bigBaseBlockCore44 = new List<short[]>
		{
			new short[2] { 72, 1 },
			new short[2] { 60, 1 },
			new short[2] { 79, 1 },
			new short[2] { 80, 1 },
			new short[2] { 90, 1 }
		};
		list3 = new List<short[]>();
		List<short[]> seriesBlockCore44 = list3;
		list3 = new List<short[]>();
		List<short[]> encircleBlockCore44 = list3;
		short[] sceneryBlockCore44 = new short[0];
		list4 = new List<short>();
		List<short> lovingItemSubTypes44 = list4;
		list4 = new List<short>();
		dataArray44.Add(new MapAreaItem(43, 2, 192, 193, 20, worldMapPos44, neighborAreas44, "largemap_part_1_changgui_1", miniMapIcons44, null, miniMapPos44, 0, 0, 5, enemyNests44, enemyNestCreationDateRanges44, settlementBlockCore44, organizationId44, -1, 0, null, developedBlockCore44, normalBlockCore44, wildBlockCore44, bigBaseBlockCore44, seriesBlockCore44, encircleBlockCore44, sceneryBlockCore44, 194, 195, null, null, EMapAreaAreaDirection.South, lovingItemSubTypes44, list4, showDarkAshStatus: true));
		List<MapAreaItem> dataArray45 = _dataArray;
		sbyte[] worldMapPos45 = new sbyte[2] { 4, 9 };
		AreaTravelRoute[] neighborAreas45 = new AreaTravelRoute[1]
		{
			new AreaTravelRoute(114, 42, new int[8] { 4, 8, 4, 7, 4, 6, 4, 5 })
		};
		string[] miniMapIcons45 = new string[4] { "largemap_part_2_icon_hei_1", "largemap_part_2_icon_jin_1", "largemap_part_2_icon_lan_1", "largemap_part_2_icon_hui_1" };
		short[] miniMapPos45 = new short[2] { 130, 170 };
		List<EnemyNestCreationInfo[]> enemyNests45 = new List<EnemyNestCreationInfo[]>
		{
			new EnemyNestCreationInfo[2]
			{
				new EnemyNestCreationInfo(0, 2),
				new EnemyNestCreationInfo(1, 2)
			},
			new EnemyNestCreationInfo[2]
			{
				new EnemyNestCreationInfo(1, 2),
				new EnemyNestCreationInfo(2, 3)
			},
			new EnemyNestCreationInfo[4]
			{
				new EnemyNestCreationInfo(2, 3),
				new EnemyNestCreationInfo(3, 3),
				new EnemyNestCreationInfo(4, 4),
				new EnemyNestCreationInfo(5, 4)
			},
			new EnemyNestCreationInfo[1]
			{
				new EnemyNestCreationInfo(12, 2)
			}
		};
		List<IntPair> enemyNestCreationDateRanges45 = new List<IntPair>
		{
			new IntPair(0, 6),
			new IntPair(6, 12),
			new IntPair(12, 24),
			new IntPair(0, 0)
		};
		short[] settlementBlockCore45 = new short[1] { 34 };
		sbyte[] organizationId45 = new sbyte[1] { 36 };
		List<short[]> developedBlockCore45 = new List<short[]>
		{
			new short[2] { 39, 33 },
			new short[2] { 40, 33 },
			new short[2] { 41, 33 },
			new short[2] { 42, 33 },
			new short[2] { 43, 33 },
			new short[2] { 44, 33 },
			new short[2] { 45, 33 },
			new short[2] { 46, 33 },
			new short[2] { 47, 33 },
			new short[2] { 48, 33 },
			new short[2] { 49, 33 },
			new short[2] { 50, 33 },
			new short[2] { 51, 33 },
			new short[2] { 52, 33 },
			new short[2] { 53, 33 },
			new short[2] { 54, 33 },
			new short[2] { 55, 33 },
			new short[2] { 56, 33 },
			new short[2] { 109, 10 },
			new short[2] { 110, 10 },
			new short[2] { 111, 10 }
		};
		List<short[]> normalBlockCore45 = new List<short[]>
		{
			new short[2] { 57, 33 },
			new short[2] { 58, 33 },
			new short[2] { 59, 33 },
			new short[2] { 63, 33 },
			new short[2] { 64, 33 },
			new short[2] { 65, 33 },
			new short[2] { 69, 33 },
			new short[2] { 70, 33 },
			new short[2] { 71, 33 },
			new short[2] { 75, 33 },
			new short[2] { 76, 33 },
			new short[2] { 77, 33 },
			new short[2] { 81, 33 },
			new short[2] { 82, 33 },
			new short[2] { 83, 33 },
			new short[2] { 87, 33 },
			new short[2] { 88, 33 },
			new short[2] { 89, 33 },
			new short[2] { 112, 30 },
			new short[2] { 113, 30 },
			new short[2] { 114, 30 },
			new short[2] { 124, 30 }
		};
		List<short[]> wildBlockCore45 = new List<short[]>
		{
			new short[2] { 93, 33 },
			new short[2] { 94, 33 },
			new short[2] { 95, 33 },
			new short[2] { 96, 33 },
			new short[2] { 97, 33 },
			new short[2] { 98, 33 },
			new short[2] { 99, 33 },
			new short[2] { 100, 33 },
			new short[2] { 101, 33 },
			new short[2] { 102, 33 },
			new short[2] { 103, 33 },
			new short[2] { 104, 33 },
			new short[2] { 105, 33 },
			new short[2] { 106, 33 },
			new short[2] { 107, 33 },
			new short[2] { 108, 33 },
			new short[2] { 115, 40 },
			new short[2] { 116, 40 },
			new short[2] { 117, 40 },
			new short[2] { 124, 30 }
		};
		List<short[]> bigBaseBlockCore45 = new List<short[]>
		{
			new short[2] { 72, 1 },
			new short[2] { 60, 1 },
			new short[2] { 79, 1 },
			new short[2] { 80, 1 },
			new short[2] { 90, 1 }
		};
		list3 = new List<short[]>();
		List<short[]> seriesBlockCore45 = list3;
		list3 = new List<short[]>();
		List<short[]> encircleBlockCore45 = list3;
		short[] sceneryBlockCore45 = new short[0];
		list4 = new List<short>();
		List<short> lovingItemSubTypes45 = list4;
		list4 = new List<short>();
		dataArray45.Add(new MapAreaItem(44, 2, 196, 197, 20, worldMapPos45, neighborAreas45, "largemap_part_1_changgui_1", miniMapIcons45, null, miniMapPos45, 0, 0, 5, enemyNests45, enemyNestCreationDateRanges45, settlementBlockCore45, organizationId45, -1, 0, null, developedBlockCore45, normalBlockCore45, wildBlockCore45, bigBaseBlockCore45, seriesBlockCore45, encircleBlockCore45, sceneryBlockCore45, 198, 199, null, null, EMapAreaAreaDirection.South, lovingItemSubTypes45, list4, showDarkAshStatus: true));
		List<MapAreaItem> dataArray46 = _dataArray;
		sbyte[] worldMapPos46 = new sbyte[2] { 9, 1 };
		AreaTravelRoute[] neighborAreas46 = new AreaTravelRoute[2]
		{
			new AreaTravelRoute(48, 14, new int[2] { 9, 0 }),
			new AreaTravelRoute(50, 6, null)
		};
		string[] miniMapIcons46 = new string[4] { "largemap_part_2_icon_hei_2", "largemap_part_2_icon_jin_2", "largemap_part_2_icon_lan_2", "largemap_part_2_icon_hui_2" };
		short[] miniMapPos46 = new short[2] { 180, 130 };
		List<EnemyNestCreationInfo[]> enemyNests46 = new List<EnemyNestCreationInfo[]>
		{
			new EnemyNestCreationInfo[2]
			{
				new EnemyNestCreationInfo(0, 2),
				new EnemyNestCreationInfo(1, 2)
			},
			new EnemyNestCreationInfo[2]
			{
				new EnemyNestCreationInfo(1, 2),
				new EnemyNestCreationInfo(2, 3)
			},
			new EnemyNestCreationInfo[4]
			{
				new EnemyNestCreationInfo(2, 3),
				new EnemyNestCreationInfo(3, 3),
				new EnemyNestCreationInfo(4, 4),
				new EnemyNestCreationInfo(5, 4)
			},
			new EnemyNestCreationInfo[1]
			{
				new EnemyNestCreationInfo(12, 2)
			}
		};
		List<IntPair> enemyNestCreationDateRanges46 = new List<IntPair>
		{
			new IntPair(0, 6),
			new IntPair(6, 12),
			new IntPair(12, 24),
			new IntPair(0, 0)
		};
		short[] settlementBlockCore46 = new short[1] { 36 };
		sbyte[] organizationId46 = new sbyte[1] { 38 };
		List<short[]> developedBlockCore46 = new List<short[]>
		{
			new short[2] { 39, 33 },
			new short[2] { 40, 33 },
			new short[2] { 41, 33 },
			new short[2] { 42, 33 },
			new short[2] { 43, 33 },
			new short[2] { 44, 33 },
			new short[2] { 45, 33 },
			new short[2] { 46, 33 },
			new short[2] { 47, 33 },
			new short[2] { 48, 33 },
			new short[2] { 49, 33 },
			new short[2] { 50, 33 },
			new short[2] { 51, 33 },
			new short[2] { 52, 33 },
			new short[2] { 53, 33 },
			new short[2] { 54, 33 },
			new short[2] { 55, 33 },
			new short[2] { 56, 33 },
			new short[2] { 109, 10 },
			new short[2] { 110, 10 },
			new short[2] { 111, 10 }
		};
		List<short[]> normalBlockCore46 = new List<short[]>
		{
			new short[2] { 57, 33 },
			new short[2] { 58, 33 },
			new short[2] { 59, 33 },
			new short[2] { 63, 33 },
			new short[2] { 64, 33 },
			new short[2] { 65, 33 },
			new short[2] { 69, 33 },
			new short[2] { 70, 33 },
			new short[2] { 71, 33 },
			new short[2] { 75, 33 },
			new short[2] { 76, 33 },
			new short[2] { 77, 33 },
			new short[2] { 81, 33 },
			new short[2] { 82, 33 },
			new short[2] { 83, 33 },
			new short[2] { 87, 33 },
			new short[2] { 88, 33 },
			new short[2] { 89, 33 },
			new short[2] { 112, 30 },
			new short[2] { 113, 30 },
			new short[2] { 114, 30 },
			new short[2] { 124, 30 }
		};
		List<short[]> wildBlockCore46 = new List<short[]>
		{
			new short[2] { 93, 33 },
			new short[2] { 94, 33 },
			new short[2] { 95, 33 },
			new short[2] { 96, 33 },
			new short[2] { 97, 33 },
			new short[2] { 98, 33 },
			new short[2] { 99, 33 },
			new short[2] { 100, 33 },
			new short[2] { 101, 33 },
			new short[2] { 102, 33 },
			new short[2] { 103, 33 },
			new short[2] { 104, 33 },
			new short[2] { 105, 33 },
			new short[2] { 106, 33 },
			new short[2] { 107, 33 },
			new short[2] { 108, 33 },
			new short[2] { 115, 40 },
			new short[2] { 116, 40 },
			new short[2] { 117, 40 },
			new short[2] { 124, 30 }
		};
		List<short[]> bigBaseBlockCore46 = new List<short[]>
		{
			new short[2] { 73, 1 },
			new short[2] { 74, 1 },
			new short[2] { 78, 1 },
			new short[2] { 84, 1 },
			new short[2] { 90, 1 }
		};
		list3 = new List<short[]>();
		List<short[]> seriesBlockCore46 = list3;
		list3 = new List<short[]>();
		List<short[]> encircleBlockCore46 = list3;
		short[] sceneryBlockCore46 = new short[0];
		list4 = new List<short>();
		List<short> lovingItemSubTypes46 = list4;
		list4 = new List<short>();
		dataArray46.Add(new MapAreaItem(45, 3, 200, 201, 20, worldMapPos46, neighborAreas46, "largemap_part_1_changgui_2", miniMapIcons46, null, miniMapPos46, 0, 0, 5, enemyNests46, enemyNestCreationDateRanges46, settlementBlockCore46, organizationId46, -1, 0, null, developedBlockCore46, normalBlockCore46, wildBlockCore46, bigBaseBlockCore46, seriesBlockCore46, encircleBlockCore46, sceneryBlockCore46, 202, 203, null, null, EMapAreaAreaDirection.South, lovingItemSubTypes46, list4, showDarkAshStatus: true));
		List<MapAreaItem> dataArray47 = _dataArray;
		sbyte[] worldMapPos47 = new sbyte[2] { 7, 3 };
		AreaTravelRoute[] neighborAreas47 = new AreaTravelRoute[0];
		string[] miniMapIcons47 = new string[4] { "largemap_part_2_icon_hei_1", "largemap_part_2_icon_jin_1", "largemap_part_2_icon_lan_1", "largemap_part_2_icon_hui_1" };
		short[] miniMapPos47 = new short[2] { 100, 210 };
		List<EnemyNestCreationInfo[]> enemyNests47 = new List<EnemyNestCreationInfo[]>
		{
			new EnemyNestCreationInfo[2]
			{
				new EnemyNestCreationInfo(0, 2),
				new EnemyNestCreationInfo(1, 2)
			},
			new EnemyNestCreationInfo[2]
			{
				new EnemyNestCreationInfo(1, 2),
				new EnemyNestCreationInfo(2, 3)
			},
			new EnemyNestCreationInfo[4]
			{
				new EnemyNestCreationInfo(2, 3),
				new EnemyNestCreationInfo(3, 3),
				new EnemyNestCreationInfo(4, 4),
				new EnemyNestCreationInfo(5, 4)
			},
			new EnemyNestCreationInfo[1]
			{
				new EnemyNestCreationInfo(12, 2)
			}
		};
		List<IntPair> enemyNestCreationDateRanges47 = new List<IntPair>
		{
			new IntPair(0, 6),
			new IntPair(6, 12),
			new IntPair(12, 24),
			new IntPair(0, 0)
		};
		short[] settlementBlockCore47 = new short[1] { 35 };
		sbyte[] organizationId47 = new sbyte[1] { 37 };
		List<short[]> developedBlockCore47 = new List<short[]>
		{
			new short[2] { 39, 33 },
			new short[2] { 40, 33 },
			new short[2] { 41, 33 },
			new short[2] { 42, 33 },
			new short[2] { 43, 33 },
			new short[2] { 44, 33 },
			new short[2] { 45, 33 },
			new short[2] { 46, 33 },
			new short[2] { 47, 33 },
			new short[2] { 48, 33 },
			new short[2] { 49, 33 },
			new short[2] { 50, 33 },
			new short[2] { 51, 33 },
			new short[2] { 52, 33 },
			new short[2] { 53, 33 },
			new short[2] { 54, 33 },
			new short[2] { 55, 33 },
			new short[2] { 56, 33 },
			new short[2] { 109, 10 },
			new short[2] { 110, 10 },
			new short[2] { 111, 10 }
		};
		List<short[]> normalBlockCore47 = new List<short[]>
		{
			new short[2] { 57, 33 },
			new short[2] { 58, 33 },
			new short[2] { 59, 33 },
			new short[2] { 63, 33 },
			new short[2] { 64, 33 },
			new short[2] { 65, 33 },
			new short[2] { 69, 33 },
			new short[2] { 70, 33 },
			new short[2] { 71, 33 },
			new short[2] { 75, 33 },
			new short[2] { 76, 33 },
			new short[2] { 77, 33 },
			new short[2] { 81, 33 },
			new short[2] { 82, 33 },
			new short[2] { 83, 33 },
			new short[2] { 87, 33 },
			new short[2] { 88, 33 },
			new short[2] { 89, 33 },
			new short[2] { 112, 30 },
			new short[2] { 113, 30 },
			new short[2] { 114, 30 },
			new short[2] { 124, 30 }
		};
		List<short[]> wildBlockCore47 = new List<short[]>
		{
			new short[2] { 93, 33 },
			new short[2] { 94, 33 },
			new short[2] { 95, 33 },
			new short[2] { 96, 33 },
			new short[2] { 97, 33 },
			new short[2] { 98, 33 },
			new short[2] { 99, 33 },
			new short[2] { 100, 33 },
			new short[2] { 101, 33 },
			new short[2] { 102, 33 },
			new short[2] { 103, 33 },
			new short[2] { 104, 33 },
			new short[2] { 105, 33 },
			new short[2] { 106, 33 },
			new short[2] { 107, 33 },
			new short[2] { 108, 33 },
			new short[2] { 115, 40 },
			new short[2] { 116, 40 },
			new short[2] { 117, 40 },
			new short[2] { 124, 30 }
		};
		List<short[]> bigBaseBlockCore47 = new List<short[]>
		{
			new short[2] { 73, 1 },
			new short[2] { 74, 1 },
			new short[2] { 78, 1 },
			new short[2] { 84, 1 },
			new short[2] { 90, 1 }
		};
		list3 = new List<short[]>();
		List<short[]> seriesBlockCore47 = list3;
		list3 = new List<short[]>();
		List<short[]> encircleBlockCore47 = list3;
		short[] sceneryBlockCore47 = new short[0];
		list4 = new List<short>();
		List<short> lovingItemSubTypes47 = list4;
		list4 = new List<short>();
		dataArray47.Add(new MapAreaItem(46, 3, 204, 205, 20, worldMapPos47, neighborAreas47, "largemap_part_1_changgui_1", miniMapIcons47, null, miniMapPos47, 0, 0, 5, enemyNests47, enemyNestCreationDateRanges47, settlementBlockCore47, organizationId47, -1, 0, null, developedBlockCore47, normalBlockCore47, wildBlockCore47, bigBaseBlockCore47, seriesBlockCore47, encircleBlockCore47, sceneryBlockCore47, 206, 207, null, null, EMapAreaAreaDirection.South, lovingItemSubTypes47, list4, showDarkAshStatus: true));
		List<MapAreaItem> dataArray48 = _dataArray;
		sbyte[] worldMapPos48 = new sbyte[2] { 11, 2 };
		AreaTravelRoute[] neighborAreas48 = new AreaTravelRoute[2]
		{
			new AreaTravelRoute(50, 12, new int[2] { 10, 2 }),
			new AreaTravelRoute(72, 22, new int[4] { 12, 2, 13, 2 })
		};
		string[] miniMapIcons48 = new string[4] { "largemap_part_2_icon_hei_1", "largemap_part_2_icon_jin_1", "largemap_part_2_icon_lan_1", "largemap_part_2_icon_hui_1" };
		short[] miniMapPos48 = new short[2] { 260, 170 };
		List<EnemyNestCreationInfo[]> enemyNests48 = new List<EnemyNestCreationInfo[]>
		{
			new EnemyNestCreationInfo[2]
			{
				new EnemyNestCreationInfo(0, 2),
				new EnemyNestCreationInfo(1, 2)
			},
			new EnemyNestCreationInfo[2]
			{
				new EnemyNestCreationInfo(1, 2),
				new EnemyNestCreationInfo(2, 3)
			},
			new EnemyNestCreationInfo[4]
			{
				new EnemyNestCreationInfo(2, 3),
				new EnemyNestCreationInfo(3, 3),
				new EnemyNestCreationInfo(4, 4),
				new EnemyNestCreationInfo(5, 4)
			},
			new EnemyNestCreationInfo[1]
			{
				new EnemyNestCreationInfo(12, 2)
			}
		};
		List<IntPair> enemyNestCreationDateRanges48 = new List<IntPair>
		{
			new IntPair(0, 6),
			new IntPair(6, 12),
			new IntPair(12, 24),
			new IntPair(0, 0)
		};
		short[] settlementBlockCore48 = new short[1] { 34 };
		sbyte[] organizationId48 = new sbyte[1] { 36 };
		List<short[]> developedBlockCore48 = new List<short[]>
		{
			new short[2] { 39, 33 },
			new short[2] { 40, 33 },
			new short[2] { 41, 33 },
			new short[2] { 42, 33 },
			new short[2] { 43, 33 },
			new short[2] { 44, 33 },
			new short[2] { 45, 33 },
			new short[2] { 46, 33 },
			new short[2] { 47, 33 },
			new short[2] { 48, 33 },
			new short[2] { 49, 33 },
			new short[2] { 50, 33 },
			new short[2] { 51, 33 },
			new short[2] { 52, 33 },
			new short[2] { 53, 33 },
			new short[2] { 54, 33 },
			new short[2] { 55, 33 },
			new short[2] { 56, 33 },
			new short[2] { 109, 10 },
			new short[2] { 110, 10 },
			new short[2] { 111, 10 }
		};
		List<short[]> normalBlockCore48 = new List<short[]>
		{
			new short[2] { 57, 33 },
			new short[2] { 58, 33 },
			new short[2] { 59, 33 },
			new short[2] { 63, 33 },
			new short[2] { 64, 33 },
			new short[2] { 65, 33 },
			new short[2] { 69, 33 },
			new short[2] { 70, 33 },
			new short[2] { 71, 33 },
			new short[2] { 75, 33 },
			new short[2] { 76, 33 },
			new short[2] { 77, 33 },
			new short[2] { 81, 33 },
			new short[2] { 82, 33 },
			new short[2] { 83, 33 },
			new short[2] { 87, 33 },
			new short[2] { 88, 33 },
			new short[2] { 89, 33 },
			new short[2] { 112, 30 },
			new short[2] { 113, 30 },
			new short[2] { 114, 30 },
			new short[2] { 124, 30 }
		};
		List<short[]> wildBlockCore48 = new List<short[]>
		{
			new short[2] { 93, 33 },
			new short[2] { 94, 33 },
			new short[2] { 95, 33 },
			new short[2] { 96, 33 },
			new short[2] { 97, 33 },
			new short[2] { 98, 33 },
			new short[2] { 99, 33 },
			new short[2] { 100, 33 },
			new short[2] { 101, 33 },
			new short[2] { 102, 33 },
			new short[2] { 103, 33 },
			new short[2] { 104, 33 },
			new short[2] { 105, 33 },
			new short[2] { 106, 33 },
			new short[2] { 107, 33 },
			new short[2] { 108, 33 },
			new short[2] { 115, 40 },
			new short[2] { 116, 40 },
			new short[2] { 117, 40 },
			new short[2] { 124, 30 }
		};
		List<short[]> bigBaseBlockCore48 = new List<short[]>
		{
			new short[2] { 73, 1 },
			new short[2] { 74, 1 },
			new short[2] { 78, 1 },
			new short[2] { 84, 1 },
			new short[2] { 90, 1 }
		};
		list3 = new List<short[]>();
		List<short[]> seriesBlockCore48 = list3;
		list3 = new List<short[]>();
		List<short[]> encircleBlockCore48 = list3;
		short[] sceneryBlockCore48 = new short[0];
		list4 = new List<short>();
		List<short> lovingItemSubTypes48 = list4;
		list4 = new List<short>();
		dataArray48.Add(new MapAreaItem(47, 3, 208, 209, 20, worldMapPos48, neighborAreas48, "largemap_part_1_changgui_1", miniMapIcons48, null, miniMapPos48, 0, 0, 5, enemyNests48, enemyNestCreationDateRanges48, settlementBlockCore48, organizationId48, -1, 0, null, developedBlockCore48, normalBlockCore48, wildBlockCore48, bigBaseBlockCore48, seriesBlockCore48, encircleBlockCore48, sceneryBlockCore48, 210, 211, null, null, EMapAreaAreaDirection.South, lovingItemSubTypes48, list4, showDarkAshStatus: true));
		List<MapAreaItem> dataArray49 = _dataArray;
		sbyte[] worldMapPos49 = new sbyte[2] { 10, 0 };
		AreaTravelRoute[] neighborAreas49 = new AreaTravelRoute[0];
		string[] miniMapIcons49 = new string[4] { "largemap_part_2_icon_hei_2", "largemap_part_2_icon_jin_2", "largemap_part_2_icon_lan_2", "largemap_part_2_icon_hui_2" };
		short[] miniMapPos49 = new short[2] { 220, 90 };
		List<EnemyNestCreationInfo[]> enemyNests49 = new List<EnemyNestCreationInfo[]>
		{
			new EnemyNestCreationInfo[2]
			{
				new EnemyNestCreationInfo(0, 2),
				new EnemyNestCreationInfo(1, 2)
			},
			new EnemyNestCreationInfo[2]
			{
				new EnemyNestCreationInfo(1, 2),
				new EnemyNestCreationInfo(2, 3)
			},
			new EnemyNestCreationInfo[4]
			{
				new EnemyNestCreationInfo(2, 3),
				new EnemyNestCreationInfo(3, 3),
				new EnemyNestCreationInfo(4, 4),
				new EnemyNestCreationInfo(5, 4)
			},
			new EnemyNestCreationInfo[1]
			{
				new EnemyNestCreationInfo(12, 2)
			}
		};
		List<IntPair> enemyNestCreationDateRanges49 = new List<IntPair>
		{
			new IntPair(0, 6),
			new IntPair(6, 12),
			new IntPair(12, 24),
			new IntPair(0, 0)
		};
		short[] settlementBlockCore49 = new short[1] { 34 };
		sbyte[] organizationId49 = new sbyte[1] { 36 };
		List<short[]> developedBlockCore49 = new List<short[]>
		{
			new short[2] { 39, 33 },
			new short[2] { 40, 33 },
			new short[2] { 41, 33 },
			new short[2] { 42, 33 },
			new short[2] { 43, 33 },
			new short[2] { 44, 33 },
			new short[2] { 45, 33 },
			new short[2] { 46, 33 },
			new short[2] { 47, 33 },
			new short[2] { 48, 33 },
			new short[2] { 49, 33 },
			new short[2] { 50, 33 },
			new short[2] { 51, 33 },
			new short[2] { 52, 33 },
			new short[2] { 53, 33 },
			new short[2] { 54, 33 },
			new short[2] { 55, 33 },
			new short[2] { 56, 33 },
			new short[2] { 109, 10 },
			new short[2] { 110, 10 },
			new short[2] { 111, 10 }
		};
		List<short[]> normalBlockCore49 = new List<short[]>
		{
			new short[2] { 57, 33 },
			new short[2] { 58, 33 },
			new short[2] { 59, 33 },
			new short[2] { 63, 33 },
			new short[2] { 64, 33 },
			new short[2] { 65, 33 },
			new short[2] { 69, 33 },
			new short[2] { 70, 33 },
			new short[2] { 71, 33 },
			new short[2] { 75, 33 },
			new short[2] { 76, 33 },
			new short[2] { 77, 33 },
			new short[2] { 81, 33 },
			new short[2] { 82, 33 },
			new short[2] { 83, 33 },
			new short[2] { 87, 33 },
			new short[2] { 88, 33 },
			new short[2] { 89, 33 },
			new short[2] { 112, 30 },
			new short[2] { 113, 30 },
			new short[2] { 114, 30 },
			new short[2] { 124, 30 }
		};
		List<short[]> wildBlockCore49 = new List<short[]>
		{
			new short[2] { 93, 33 },
			new short[2] { 94, 33 },
			new short[2] { 95, 33 },
			new short[2] { 96, 33 },
			new short[2] { 97, 33 },
			new short[2] { 98, 33 },
			new short[2] { 99, 33 },
			new short[2] { 100, 33 },
			new short[2] { 101, 33 },
			new short[2] { 102, 33 },
			new short[2] { 103, 33 },
			new short[2] { 104, 33 },
			new short[2] { 105, 33 },
			new short[2] { 106, 33 },
			new short[2] { 107, 33 },
			new short[2] { 108, 33 },
			new short[2] { 115, 40 },
			new short[2] { 116, 40 },
			new short[2] { 117, 40 },
			new short[2] { 124, 30 }
		};
		List<short[]> bigBaseBlockCore49 = new List<short[]>
		{
			new short[2] { 73, 1 },
			new short[2] { 74, 1 },
			new short[2] { 78, 1 },
			new short[2] { 84, 1 },
			new short[2] { 90, 1 }
		};
		list3 = new List<short[]>();
		List<short[]> seriesBlockCore49 = list3;
		list3 = new List<short[]>();
		List<short[]> encircleBlockCore49 = list3;
		short[] sceneryBlockCore49 = new short[0];
		list4 = new List<short>();
		List<short> lovingItemSubTypes49 = list4;
		list4 = new List<short>();
		dataArray49.Add(new MapAreaItem(48, 3, 212, 213, 20, worldMapPos49, neighborAreas49, "largemap_part_1_changgui_2", miniMapIcons49, null, miniMapPos49, 0, 0, 5, enemyNests49, enemyNestCreationDateRanges49, settlementBlockCore49, organizationId49, -1, 0, null, developedBlockCore49, normalBlockCore49, wildBlockCore49, bigBaseBlockCore49, seriesBlockCore49, encircleBlockCore49, sceneryBlockCore49, 214, 215, null, null, EMapAreaAreaDirection.South, lovingItemSubTypes49, list4, showDarkAshStatus: true));
		List<MapAreaItem> dataArray50 = _dataArray;
		sbyte[] worldMapPos50 = new sbyte[2] { 11, 4 };
		AreaTravelRoute[] neighborAreas50 = new AreaTravelRoute[2]
		{
			new AreaTravelRoute(51, 6, null),
			new AreaTravelRoute(85, 22, new int[4] { 11, 5, 12, 5 })
		};
		string[] miniMapIcons50 = new string[4] { "largemap_part_2_icon_hei_0", "largemap_part_2_icon_jin_0", "largemap_part_2_icon_lan_0", "largemap_part_2_icon_hui_0" };
		short[] miniMapPos50 = new short[2] { 260, 250 };
		List<EnemyNestCreationInfo[]> enemyNests50 = new List<EnemyNestCreationInfo[]>
		{
			new EnemyNestCreationInfo[2]
			{
				new EnemyNestCreationInfo(0, 2),
				new EnemyNestCreationInfo(1, 2)
			},
			new EnemyNestCreationInfo[2]
			{
				new EnemyNestCreationInfo(1, 2),
				new EnemyNestCreationInfo(2, 3)
			},
			new EnemyNestCreationInfo[4]
			{
				new EnemyNestCreationInfo(2, 3),
				new EnemyNestCreationInfo(3, 3),
				new EnemyNestCreationInfo(4, 4),
				new EnemyNestCreationInfo(5, 4)
			},
			new EnemyNestCreationInfo[1]
			{
				new EnemyNestCreationInfo(12, 2)
			}
		};
		List<IntPair> enemyNestCreationDateRanges50 = new List<IntPair>
		{
			new IntPair(0, 6),
			new IntPair(6, 12),
			new IntPair(12, 24),
			new IntPair(0, 0)
		};
		short[] settlementBlockCore50 = new short[1] { 36 };
		sbyte[] organizationId50 = new sbyte[1] { 38 };
		List<short[]> developedBlockCore50 = new List<short[]>
		{
			new short[2] { 39, 33 },
			new short[2] { 40, 33 },
			new short[2] { 41, 33 },
			new short[2] { 42, 33 },
			new short[2] { 43, 33 },
			new short[2] { 44, 33 },
			new short[2] { 45, 33 },
			new short[2] { 46, 33 },
			new short[2] { 47, 33 },
			new short[2] { 48, 33 },
			new short[2] { 49, 33 },
			new short[2] { 50, 33 },
			new short[2] { 51, 33 },
			new short[2] { 52, 33 },
			new short[2] { 53, 33 },
			new short[2] { 54, 33 },
			new short[2] { 55, 33 },
			new short[2] { 56, 33 },
			new short[2] { 109, 10 },
			new short[2] { 110, 10 },
			new short[2] { 111, 10 }
		};
		List<short[]> normalBlockCore50 = new List<short[]>
		{
			new short[2] { 57, 33 },
			new short[2] { 58, 33 },
			new short[2] { 59, 33 },
			new short[2] { 63, 33 },
			new short[2] { 64, 33 },
			new short[2] { 65, 33 },
			new short[2] { 69, 33 },
			new short[2] { 70, 33 },
			new short[2] { 71, 33 },
			new short[2] { 75, 33 },
			new short[2] { 76, 33 },
			new short[2] { 77, 33 },
			new short[2] { 81, 33 },
			new short[2] { 82, 33 },
			new short[2] { 83, 33 },
			new short[2] { 87, 33 },
			new short[2] { 88, 33 },
			new short[2] { 89, 33 },
			new short[2] { 112, 30 },
			new short[2] { 113, 30 },
			new short[2] { 114, 30 },
			new short[2] { 124, 30 }
		};
		List<short[]> wildBlockCore50 = new List<short[]>
		{
			new short[2] { 93, 33 },
			new short[2] { 94, 33 },
			new short[2] { 95, 33 },
			new short[2] { 96, 33 },
			new short[2] { 97, 33 },
			new short[2] { 98, 33 },
			new short[2] { 99, 33 },
			new short[2] { 100, 33 },
			new short[2] { 101, 33 },
			new short[2] { 102, 33 },
			new short[2] { 103, 33 },
			new short[2] { 104, 33 },
			new short[2] { 105, 33 },
			new short[2] { 106, 33 },
			new short[2] { 107, 33 },
			new short[2] { 108, 33 },
			new short[2] { 115, 40 },
			new short[2] { 116, 40 },
			new short[2] { 117, 40 },
			new short[2] { 124, 30 }
		};
		List<short[]> bigBaseBlockCore50 = new List<short[]>
		{
			new short[2] { 73, 1 },
			new short[2] { 74, 1 },
			new short[2] { 78, 1 },
			new short[2] { 84, 1 },
			new short[2] { 90, 1 }
		};
		list3 = new List<short[]>();
		List<short[]> seriesBlockCore50 = list3;
		list3 = new List<short[]>();
		List<short[]> encircleBlockCore50 = list3;
		short[] sceneryBlockCore50 = new short[0];
		list4 = new List<short>();
		List<short> lovingItemSubTypes50 = list4;
		list4 = new List<short>();
		dataArray50.Add(new MapAreaItem(49, 3, 216, 217, 20, worldMapPos50, neighborAreas50, "largemap_part_1_changgui_0", miniMapIcons50, null, miniMapPos50, 0, 0, 5, enemyNests50, enemyNestCreationDateRanges50, settlementBlockCore50, organizationId50, -1, 0, null, developedBlockCore50, normalBlockCore50, wildBlockCore50, bigBaseBlockCore50, seriesBlockCore50, encircleBlockCore50, sceneryBlockCore50, 218, 219, null, null, EMapAreaAreaDirection.South, lovingItemSubTypes50, list4, showDarkAshStatus: true));
		List<MapAreaItem> dataArray51 = _dataArray;
		sbyte[] worldMapPos51 = new sbyte[2] { 9, 2 };
		AreaTravelRoute[] neighborAreas51 = new AreaTravelRoute[0];
		string[] miniMapIcons51 = new string[4] { "largemap_part_2_icon_hei_0", "largemap_part_2_icon_jin_0", "largemap_part_2_icon_lan_0", "largemap_part_2_icon_hui_0" };
		short[] miniMapPos51 = new short[2] { 180, 170 };
		List<EnemyNestCreationInfo[]> enemyNests51 = new List<EnemyNestCreationInfo[]>
		{
			new EnemyNestCreationInfo[2]
			{
				new EnemyNestCreationInfo(0, 2),
				new EnemyNestCreationInfo(1, 2)
			},
			new EnemyNestCreationInfo[2]
			{
				new EnemyNestCreationInfo(1, 2),
				new EnemyNestCreationInfo(2, 3)
			},
			new EnemyNestCreationInfo[4]
			{
				new EnemyNestCreationInfo(2, 3),
				new EnemyNestCreationInfo(3, 3),
				new EnemyNestCreationInfo(4, 4),
				new EnemyNestCreationInfo(5, 4)
			},
			new EnemyNestCreationInfo[1]
			{
				new EnemyNestCreationInfo(12, 2)
			}
		};
		List<IntPair> enemyNestCreationDateRanges51 = new List<IntPair>
		{
			new IntPair(0, 6),
			new IntPair(6, 12),
			new IntPair(12, 24),
			new IntPair(0, 0)
		};
		short[] settlementBlockCore51 = new short[1] { 34 };
		sbyte[] organizationId51 = new sbyte[1] { 36 };
		List<short[]> developedBlockCore51 = new List<short[]>
		{
			new short[2] { 39, 33 },
			new short[2] { 40, 33 },
			new short[2] { 41, 33 },
			new short[2] { 42, 33 },
			new short[2] { 43, 33 },
			new short[2] { 44, 33 },
			new short[2] { 45, 33 },
			new short[2] { 46, 33 },
			new short[2] { 47, 33 },
			new short[2] { 48, 33 },
			new short[2] { 49, 33 },
			new short[2] { 50, 33 },
			new short[2] { 51, 33 },
			new short[2] { 52, 33 },
			new short[2] { 53, 33 },
			new short[2] { 54, 33 },
			new short[2] { 55, 33 },
			new short[2] { 56, 33 },
			new short[2] { 109, 10 },
			new short[2] { 110, 10 },
			new short[2] { 111, 10 }
		};
		List<short[]> normalBlockCore51 = new List<short[]>
		{
			new short[2] { 57, 33 },
			new short[2] { 58, 33 },
			new short[2] { 59, 33 },
			new short[2] { 63, 33 },
			new short[2] { 64, 33 },
			new short[2] { 65, 33 },
			new short[2] { 69, 33 },
			new short[2] { 70, 33 },
			new short[2] { 71, 33 },
			new short[2] { 75, 33 },
			new short[2] { 76, 33 },
			new short[2] { 77, 33 },
			new short[2] { 81, 33 },
			new short[2] { 82, 33 },
			new short[2] { 83, 33 },
			new short[2] { 87, 33 },
			new short[2] { 88, 33 },
			new short[2] { 89, 33 },
			new short[2] { 112, 30 },
			new short[2] { 113, 30 },
			new short[2] { 114, 30 },
			new short[2] { 124, 30 }
		};
		List<short[]> wildBlockCore51 = new List<short[]>
		{
			new short[2] { 93, 33 },
			new short[2] { 94, 33 },
			new short[2] { 95, 33 },
			new short[2] { 96, 33 },
			new short[2] { 97, 33 },
			new short[2] { 98, 33 },
			new short[2] { 99, 33 },
			new short[2] { 100, 33 },
			new short[2] { 101, 33 },
			new short[2] { 102, 33 },
			new short[2] { 103, 33 },
			new short[2] { 104, 33 },
			new short[2] { 105, 33 },
			new short[2] { 106, 33 },
			new short[2] { 107, 33 },
			new short[2] { 108, 33 },
			new short[2] { 115, 40 },
			new short[2] { 116, 40 },
			new short[2] { 117, 40 },
			new short[2] { 124, 30 }
		};
		List<short[]> bigBaseBlockCore51 = new List<short[]>
		{
			new short[2] { 73, 1 },
			new short[2] { 74, 1 },
			new short[2] { 78, 1 },
			new short[2] { 84, 1 },
			new short[2] { 90, 1 }
		};
		list3 = new List<short[]>();
		List<short[]> seriesBlockCore51 = list3;
		list3 = new List<short[]>();
		List<short[]> encircleBlockCore51 = list3;
		short[] sceneryBlockCore51 = new short[0];
		list4 = new List<short>();
		List<short> lovingItemSubTypes51 = list4;
		list4 = new List<short>();
		dataArray51.Add(new MapAreaItem(50, 3, 220, 221, 20, worldMapPos51, neighborAreas51, "largemap_part_1_changgui_0", miniMapIcons51, null, miniMapPos51, 0, 0, 5, enemyNests51, enemyNestCreationDateRanges51, settlementBlockCore51, organizationId51, -1, 0, null, developedBlockCore51, normalBlockCore51, wildBlockCore51, bigBaseBlockCore51, seriesBlockCore51, encircleBlockCore51, sceneryBlockCore51, 222, 223, null, null, EMapAreaAreaDirection.South, lovingItemSubTypes51, list4, showDarkAshStatus: true));
		List<MapAreaItem> dataArray52 = _dataArray;
		sbyte[] worldMapPos52 = new sbyte[2] { 10, 4 };
		AreaTravelRoute[] neighborAreas52 = new AreaTravelRoute[0];
		string[] miniMapIcons52 = new string[4] { "largemap_part_2_icon_hei_0", "largemap_part_2_icon_jin_0", "largemap_part_2_icon_lan_0", "largemap_part_2_icon_hui_0" };
		short[] miniMapPos52 = new short[2] { 220, 250 };
		List<EnemyNestCreationInfo[]> enemyNests52 = new List<EnemyNestCreationInfo[]>
		{
			new EnemyNestCreationInfo[2]
			{
				new EnemyNestCreationInfo(0, 2),
				new EnemyNestCreationInfo(1, 2)
			},
			new EnemyNestCreationInfo[2]
			{
				new EnemyNestCreationInfo(1, 2),
				new EnemyNestCreationInfo(2, 3)
			},
			new EnemyNestCreationInfo[4]
			{
				new EnemyNestCreationInfo(2, 3),
				new EnemyNestCreationInfo(3, 3),
				new EnemyNestCreationInfo(4, 4),
				new EnemyNestCreationInfo(5, 4)
			},
			new EnemyNestCreationInfo[1]
			{
				new EnemyNestCreationInfo(12, 2)
			}
		};
		List<IntPair> enemyNestCreationDateRanges52 = new List<IntPair>
		{
			new IntPair(0, 6),
			new IntPair(6, 12),
			new IntPair(12, 24),
			new IntPair(0, 0)
		};
		short[] settlementBlockCore52 = new short[1] { 34 };
		sbyte[] organizationId52 = new sbyte[1] { 36 };
		List<short[]> developedBlockCore52 = new List<short[]>
		{
			new short[2] { 39, 33 },
			new short[2] { 40, 33 },
			new short[2] { 41, 33 },
			new short[2] { 42, 33 },
			new short[2] { 43, 33 },
			new short[2] { 44, 33 },
			new short[2] { 45, 33 },
			new short[2] { 46, 33 },
			new short[2] { 47, 33 },
			new short[2] { 48, 33 },
			new short[2] { 49, 33 },
			new short[2] { 50, 33 },
			new short[2] { 51, 33 },
			new short[2] { 52, 33 },
			new short[2] { 53, 33 },
			new short[2] { 54, 33 },
			new short[2] { 55, 33 },
			new short[2] { 56, 33 },
			new short[2] { 109, 10 },
			new short[2] { 110, 10 },
			new short[2] { 111, 10 }
		};
		List<short[]> normalBlockCore52 = new List<short[]>
		{
			new short[2] { 57, 33 },
			new short[2] { 58, 33 },
			new short[2] { 59, 33 },
			new short[2] { 63, 33 },
			new short[2] { 64, 33 },
			new short[2] { 65, 33 },
			new short[2] { 69, 33 },
			new short[2] { 70, 33 },
			new short[2] { 71, 33 },
			new short[2] { 75, 33 },
			new short[2] { 76, 33 },
			new short[2] { 77, 33 },
			new short[2] { 81, 33 },
			new short[2] { 82, 33 },
			new short[2] { 83, 33 },
			new short[2] { 87, 33 },
			new short[2] { 88, 33 },
			new short[2] { 89, 33 },
			new short[2] { 112, 30 },
			new short[2] { 113, 30 },
			new short[2] { 114, 30 },
			new short[2] { 124, 30 }
		};
		List<short[]> wildBlockCore52 = new List<short[]>
		{
			new short[2] { 93, 33 },
			new short[2] { 94, 33 },
			new short[2] { 95, 33 },
			new short[2] { 96, 33 },
			new short[2] { 97, 33 },
			new short[2] { 98, 33 },
			new short[2] { 99, 33 },
			new short[2] { 100, 33 },
			new short[2] { 101, 33 },
			new short[2] { 102, 33 },
			new short[2] { 103, 33 },
			new short[2] { 104, 33 },
			new short[2] { 105, 33 },
			new short[2] { 106, 33 },
			new short[2] { 107, 33 },
			new short[2] { 108, 33 },
			new short[2] { 115, 40 },
			new short[2] { 116, 40 },
			new short[2] { 117, 40 },
			new short[2] { 124, 30 }
		};
		List<short[]> bigBaseBlockCore52 = new List<short[]>
		{
			new short[2] { 73, 1 },
			new short[2] { 74, 1 },
			new short[2] { 78, 1 },
			new short[2] { 84, 1 },
			new short[2] { 90, 1 }
		};
		list3 = new List<short[]>();
		List<short[]> seriesBlockCore52 = list3;
		list3 = new List<short[]>();
		List<short[]> encircleBlockCore52 = list3;
		short[] sceneryBlockCore52 = new short[0];
		list4 = new List<short>();
		List<short> lovingItemSubTypes52 = list4;
		list4 = new List<short>();
		dataArray52.Add(new MapAreaItem(51, 3, 224, 225, 20, worldMapPos52, neighborAreas52, "largemap_part_1_changgui_0", miniMapIcons52, null, miniMapPos52, 0, 0, 5, enemyNests52, enemyNestCreationDateRanges52, settlementBlockCore52, organizationId52, -1, 0, null, developedBlockCore52, normalBlockCore52, wildBlockCore52, bigBaseBlockCore52, seriesBlockCore52, encircleBlockCore52, sceneryBlockCore52, 226, 227, null, null, EMapAreaAreaDirection.South, lovingItemSubTypes52, list4, showDarkAshStatus: true));
		List<MapAreaItem> dataArray53 = _dataArray;
		sbyte[] worldMapPos53 = new sbyte[2] { 9, 13 };
		AreaTravelRoute[] neighborAreas53 = new AreaTravelRoute[2]
		{
			new AreaTravelRoute(56, 6, null),
			new AreaTravelRoute(104, 12, new int[2] { 8, 13 })
		};
		string[] miniMapIcons53 = new string[4] { "largemap_part_2_icon_hei_2", "largemap_part_2_icon_jin_2", "largemap_part_2_icon_lan_2", "largemap_part_2_icon_hui_2" };
		short[] miniMapPos53 = new short[2] { 60, 240 };
		List<EnemyNestCreationInfo[]> enemyNests53 = new List<EnemyNestCreationInfo[]>
		{
			new EnemyNestCreationInfo[2]
			{
				new EnemyNestCreationInfo(0, 2),
				new EnemyNestCreationInfo(1, 2)
			},
			new EnemyNestCreationInfo[2]
			{
				new EnemyNestCreationInfo(1, 2),
				new EnemyNestCreationInfo(2, 3)
			},
			new EnemyNestCreationInfo[4]
			{
				new EnemyNestCreationInfo(2, 3),
				new EnemyNestCreationInfo(3, 3),
				new EnemyNestCreationInfo(4, 4),
				new EnemyNestCreationInfo(5, 4)
			},
			new EnemyNestCreationInfo[1]
			{
				new EnemyNestCreationInfo(12, 2)
			}
		};
		List<IntPair> enemyNestCreationDateRanges53 = new List<IntPair>
		{
			new IntPair(0, 6),
			new IntPair(6, 12),
			new IntPair(12, 24),
			new IntPair(0, 0)
		};
		short[] settlementBlockCore53 = new short[1] { 35 };
		sbyte[] organizationId53 = new sbyte[1] { 37 };
		List<short[]> developedBlockCore53 = new List<short[]>
		{
			new short[2] { 39, 33 },
			new short[2] { 40, 33 },
			new short[2] { 41, 33 },
			new short[2] { 42, 33 },
			new short[2] { 43, 33 },
			new short[2] { 44, 33 },
			new short[2] { 45, 33 },
			new short[2] { 46, 33 },
			new short[2] { 47, 33 },
			new short[2] { 48, 33 },
			new short[2] { 49, 33 },
			new short[2] { 50, 33 },
			new short[2] { 51, 33 },
			new short[2] { 52, 33 },
			new short[2] { 53, 33 },
			new short[2] { 54, 33 },
			new short[2] { 55, 33 },
			new short[2] { 56, 33 },
			new short[2] { 109, 10 },
			new short[2] { 110, 10 },
			new short[2] { 111, 10 }
		};
		List<short[]> normalBlockCore53 = new List<short[]>
		{
			new short[2] { 57, 33 },
			new short[2] { 58, 33 },
			new short[2] { 59, 33 },
			new short[2] { 63, 33 },
			new short[2] { 64, 33 },
			new short[2] { 65, 33 },
			new short[2] { 69, 33 },
			new short[2] { 70, 33 },
			new short[2] { 71, 33 },
			new short[2] { 75, 33 },
			new short[2] { 76, 33 },
			new short[2] { 77, 33 },
			new short[2] { 81, 33 },
			new short[2] { 82, 33 },
			new short[2] { 83, 33 },
			new short[2] { 87, 33 },
			new short[2] { 88, 33 },
			new short[2] { 89, 33 },
			new short[2] { 112, 30 },
			new short[2] { 113, 30 },
			new short[2] { 114, 30 },
			new short[2] { 124, 30 }
		};
		List<short[]> wildBlockCore53 = new List<short[]>
		{
			new short[2] { 93, 33 },
			new short[2] { 94, 33 },
			new short[2] { 95, 33 },
			new short[2] { 96, 33 },
			new short[2] { 97, 33 },
			new short[2] { 98, 33 },
			new short[2] { 99, 33 },
			new short[2] { 100, 33 },
			new short[2] { 101, 33 },
			new short[2] { 102, 33 },
			new short[2] { 103, 33 },
			new short[2] { 104, 33 },
			new short[2] { 105, 33 },
			new short[2] { 106, 33 },
			new short[2] { 107, 33 },
			new short[2] { 108, 33 },
			new short[2] { 115, 40 },
			new short[2] { 116, 40 },
			new short[2] { 117, 40 },
			new short[2] { 124, 30 }
		};
		List<short[]> bigBaseBlockCore53 = new List<short[]>
		{
			new short[2] { 72, 1 },
			new short[2] { 78, 1 },
			new short[2] { 84, 1 },
			new short[2] { 91, 1 },
			new short[2] { 92, 1 }
		};
		list3 = new List<short[]>();
		List<short[]> seriesBlockCore53 = list3;
		list3 = new List<short[]>();
		List<short[]> encircleBlockCore53 = list3;
		short[] sceneryBlockCore53 = new short[0];
		list4 = new List<short>();
		List<short> lovingItemSubTypes53 = list4;
		list4 = new List<short>();
		dataArray53.Add(new MapAreaItem(52, 4, 228, 229, 20, worldMapPos53, neighborAreas53, "largemap_part_1_changgui_2", miniMapIcons53, null, miniMapPos53, 0, 0, 5, enemyNests53, enemyNestCreationDateRanges53, settlementBlockCore53, organizationId53, -1, 0, null, developedBlockCore53, normalBlockCore53, wildBlockCore53, bigBaseBlockCore53, seriesBlockCore53, encircleBlockCore53, sceneryBlockCore53, 230, 231, null, null, EMapAreaAreaDirection.South, lovingItemSubTypes53, list4, showDarkAshStatus: true));
		List<MapAreaItem> dataArray54 = _dataArray;
		sbyte[] worldMapPos54 = new sbyte[2] { 10, 12 };
		AreaTravelRoute[] neighborAreas54 = new AreaTravelRoute[0];
		string[] miniMapIcons54 = new string[4] { "largemap_part_2_icon_hei_2", "largemap_part_2_icon_jin_2", "largemap_part_2_icon_lan_2", "largemap_part_2_icon_hui_2" };
		short[] miniMapPos54 = new short[2] { 100, 200 };
		List<EnemyNestCreationInfo[]> enemyNests54 = new List<EnemyNestCreationInfo[]>
		{
			new EnemyNestCreationInfo[2]
			{
				new EnemyNestCreationInfo(0, 2),
				new EnemyNestCreationInfo(1, 2)
			},
			new EnemyNestCreationInfo[2]
			{
				new EnemyNestCreationInfo(1, 2),
				new EnemyNestCreationInfo(2, 3)
			},
			new EnemyNestCreationInfo[4]
			{
				new EnemyNestCreationInfo(2, 3),
				new EnemyNestCreationInfo(3, 3),
				new EnemyNestCreationInfo(4, 4),
				new EnemyNestCreationInfo(5, 4)
			},
			new EnemyNestCreationInfo[1]
			{
				new EnemyNestCreationInfo(12, 2)
			}
		};
		List<IntPair> enemyNestCreationDateRanges54 = new List<IntPair>
		{
			new IntPair(0, 6),
			new IntPair(6, 12),
			new IntPair(12, 24),
			new IntPair(0, 0)
		};
		short[] settlementBlockCore54 = new short[1] { 34 };
		sbyte[] organizationId54 = new sbyte[1] { 36 };
		List<short[]> developedBlockCore54 = new List<short[]>
		{
			new short[2] { 39, 33 },
			new short[2] { 40, 33 },
			new short[2] { 41, 33 },
			new short[2] { 42, 33 },
			new short[2] { 43, 33 },
			new short[2] { 44, 33 },
			new short[2] { 45, 33 },
			new short[2] { 46, 33 },
			new short[2] { 47, 33 },
			new short[2] { 48, 33 },
			new short[2] { 49, 33 },
			new short[2] { 50, 33 },
			new short[2] { 51, 33 },
			new short[2] { 52, 33 },
			new short[2] { 53, 33 },
			new short[2] { 54, 33 },
			new short[2] { 55, 33 },
			new short[2] { 56, 33 },
			new short[2] { 109, 10 },
			new short[2] { 110, 10 },
			new short[2] { 111, 10 }
		};
		List<short[]> normalBlockCore54 = new List<short[]>
		{
			new short[2] { 57, 33 },
			new short[2] { 58, 33 },
			new short[2] { 59, 33 },
			new short[2] { 63, 33 },
			new short[2] { 64, 33 },
			new short[2] { 65, 33 },
			new short[2] { 69, 33 },
			new short[2] { 70, 33 },
			new short[2] { 71, 33 },
			new short[2] { 75, 33 },
			new short[2] { 76, 33 },
			new short[2] { 77, 33 },
			new short[2] { 81, 33 },
			new short[2] { 82, 33 },
			new short[2] { 83, 33 },
			new short[2] { 87, 33 },
			new short[2] { 88, 33 },
			new short[2] { 89, 33 },
			new short[2] { 112, 30 },
			new short[2] { 113, 30 },
			new short[2] { 114, 30 },
			new short[2] { 124, 30 }
		};
		List<short[]> wildBlockCore54 = new List<short[]>
		{
			new short[2] { 93, 33 },
			new short[2] { 94, 33 },
			new short[2] { 95, 33 },
			new short[2] { 96, 33 },
			new short[2] { 97, 33 },
			new short[2] { 98, 33 },
			new short[2] { 99, 33 },
			new short[2] { 100, 33 },
			new short[2] { 101, 33 },
			new short[2] { 102, 33 },
			new short[2] { 103, 33 },
			new short[2] { 104, 33 },
			new short[2] { 105, 33 },
			new short[2] { 106, 33 },
			new short[2] { 107, 33 },
			new short[2] { 108, 33 },
			new short[2] { 115, 40 },
			new short[2] { 116, 40 },
			new short[2] { 117, 40 },
			new short[2] { 124, 30 }
		};
		List<short[]> bigBaseBlockCore54 = new List<short[]>
		{
			new short[2] { 72, 1 },
			new short[2] { 78, 1 },
			new short[2] { 84, 1 },
			new short[2] { 91, 1 },
			new short[2] { 92, 1 }
		};
		list3 = new List<short[]>();
		List<short[]> seriesBlockCore54 = list3;
		list3 = new List<short[]>();
		List<short[]> encircleBlockCore54 = list3;
		short[] sceneryBlockCore54 = new short[0];
		list4 = new List<short>();
		List<short> lovingItemSubTypes54 = list4;
		list4 = new List<short>();
		dataArray54.Add(new MapAreaItem(53, 4, 232, 233, 20, worldMapPos54, neighborAreas54, "largemap_part_1_changgui_2", miniMapIcons54, null, miniMapPos54, 0, 0, 5, enemyNests54, enemyNestCreationDateRanges54, settlementBlockCore54, organizationId54, -1, 0, null, developedBlockCore54, normalBlockCore54, wildBlockCore54, bigBaseBlockCore54, seriesBlockCore54, encircleBlockCore54, sceneryBlockCore54, 234, 235, null, null, EMapAreaAreaDirection.South, lovingItemSubTypes54, list4, showDarkAshStatus: true));
		List<MapAreaItem> dataArray55 = _dataArray;
		sbyte[] worldMapPos55 = new sbyte[2] { 15, 10 };
		AreaTravelRoute[] neighborAreas55 = new AreaTravelRoute[2]
		{
			new AreaTravelRoute(57, 6, null),
			new AreaTravelRoute(58, 12, new int[2] { 14, 10 })
		};
		string[] miniMapIcons55 = new string[4] { "largemap_part_2_icon_hei_2", "largemap_part_2_icon_jin_2", "largemap_part_2_icon_lan_2", "largemap_part_2_icon_hui_2" };
		short[] miniMapPos55 = new short[2] { 300, 120 };
		List<EnemyNestCreationInfo[]> enemyNests55 = new List<EnemyNestCreationInfo[]>
		{
			new EnemyNestCreationInfo[2]
			{
				new EnemyNestCreationInfo(0, 2),
				new EnemyNestCreationInfo(1, 2)
			},
			new EnemyNestCreationInfo[2]
			{
				new EnemyNestCreationInfo(1, 2),
				new EnemyNestCreationInfo(2, 3)
			},
			new EnemyNestCreationInfo[4]
			{
				new EnemyNestCreationInfo(2, 3),
				new EnemyNestCreationInfo(3, 3),
				new EnemyNestCreationInfo(4, 4),
				new EnemyNestCreationInfo(5, 4)
			},
			new EnemyNestCreationInfo[1]
			{
				new EnemyNestCreationInfo(12, 2)
			}
		};
		List<IntPair> enemyNestCreationDateRanges55 = new List<IntPair>
		{
			new IntPair(0, 6),
			new IntPair(6, 12),
			new IntPair(12, 24),
			new IntPair(0, 0)
		};
		short[] settlementBlockCore55 = new short[1] { 35 };
		sbyte[] organizationId55 = new sbyte[1] { 37 };
		List<short[]> developedBlockCore55 = new List<short[]>
		{
			new short[2] { 39, 33 },
			new short[2] { 40, 33 },
			new short[2] { 41, 33 },
			new short[2] { 42, 33 },
			new short[2] { 43, 33 },
			new short[2] { 44, 33 },
			new short[2] { 45, 33 },
			new short[2] { 46, 33 },
			new short[2] { 47, 33 },
			new short[2] { 48, 33 },
			new short[2] { 49, 33 },
			new short[2] { 50, 33 },
			new short[2] { 51, 33 },
			new short[2] { 52, 33 },
			new short[2] { 53, 33 },
			new short[2] { 54, 33 },
			new short[2] { 55, 33 },
			new short[2] { 56, 33 },
			new short[2] { 109, 10 },
			new short[2] { 110, 10 },
			new short[2] { 111, 10 }
		};
		List<short[]> normalBlockCore55 = new List<short[]>
		{
			new short[2] { 57, 33 },
			new short[2] { 58, 33 },
			new short[2] { 59, 33 },
			new short[2] { 63, 33 },
			new short[2] { 64, 33 },
			new short[2] { 65, 33 },
			new short[2] { 69, 33 },
			new short[2] { 70, 33 },
			new short[2] { 71, 33 },
			new short[2] { 75, 33 },
			new short[2] { 76, 33 },
			new short[2] { 77, 33 },
			new short[2] { 81, 33 },
			new short[2] { 82, 33 },
			new short[2] { 83, 33 },
			new short[2] { 87, 33 },
			new short[2] { 88, 33 },
			new short[2] { 89, 33 },
			new short[2] { 112, 30 },
			new short[2] { 113, 30 },
			new short[2] { 114, 30 },
			new short[2] { 124, 30 }
		};
		List<short[]> wildBlockCore55 = new List<short[]>
		{
			new short[2] { 93, 33 },
			new short[2] { 94, 33 },
			new short[2] { 95, 33 },
			new short[2] { 96, 33 },
			new short[2] { 97, 33 },
			new short[2] { 98, 33 },
			new short[2] { 99, 33 },
			new short[2] { 100, 33 },
			new short[2] { 101, 33 },
			new short[2] { 102, 33 },
			new short[2] { 103, 33 },
			new short[2] { 104, 33 },
			new short[2] { 105, 33 },
			new short[2] { 106, 33 },
			new short[2] { 107, 33 },
			new short[2] { 108, 33 },
			new short[2] { 115, 40 },
			new short[2] { 116, 40 },
			new short[2] { 117, 40 },
			new short[2] { 124, 30 }
		};
		List<short[]> bigBaseBlockCore55 = new List<short[]>
		{
			new short[2] { 72, 1 },
			new short[2] { 78, 1 },
			new short[2] { 84, 1 },
			new short[2] { 91, 1 },
			new short[2] { 92, 1 }
		};
		list3 = new List<short[]>();
		List<short[]> seriesBlockCore55 = list3;
		list3 = new List<short[]>();
		List<short[]> encircleBlockCore55 = list3;
		short[] sceneryBlockCore55 = new short[0];
		list4 = new List<short>();
		List<short> lovingItemSubTypes55 = list4;
		list4 = new List<short>();
		dataArray55.Add(new MapAreaItem(54, 4, 236, 237, 20, worldMapPos55, neighborAreas55, "largemap_part_1_changgui_2", miniMapIcons55, null, miniMapPos55, 0, 0, 5, enemyNests55, enemyNestCreationDateRanges55, settlementBlockCore55, organizationId55, -1, 0, null, developedBlockCore55, normalBlockCore55, wildBlockCore55, bigBaseBlockCore55, seriesBlockCore55, encircleBlockCore55, sceneryBlockCore55, 238, 239, null, null, EMapAreaAreaDirection.South, lovingItemSubTypes55, list4, showDarkAshStatus: true));
		List<MapAreaItem> dataArray56 = _dataArray;
		sbyte[] worldMapPos56 = new sbyte[2] { 13, 11 };
		AreaTravelRoute[] neighborAreas56 = new AreaTravelRoute[1]
		{
			new AreaTravelRoute(58, 6, null)
		};
		string[] miniMapIcons56 = new string[4] { "largemap_part_2_icon_hei_1", "largemap_part_2_icon_jin_1", "largemap_part_2_icon_lan_1", "largemap_part_2_icon_hui_1" };
		short[] miniMapPos56 = new short[2] { 220, 160 };
		List<EnemyNestCreationInfo[]> enemyNests56 = new List<EnemyNestCreationInfo[]>
		{
			new EnemyNestCreationInfo[2]
			{
				new EnemyNestCreationInfo(0, 2),
				new EnemyNestCreationInfo(1, 2)
			},
			new EnemyNestCreationInfo[2]
			{
				new EnemyNestCreationInfo(1, 2),
				new EnemyNestCreationInfo(2, 3)
			},
			new EnemyNestCreationInfo[4]
			{
				new EnemyNestCreationInfo(2, 3),
				new EnemyNestCreationInfo(3, 3),
				new EnemyNestCreationInfo(4, 4),
				new EnemyNestCreationInfo(5, 4)
			},
			new EnemyNestCreationInfo[1]
			{
				new EnemyNestCreationInfo(12, 2)
			}
		};
		List<IntPair> enemyNestCreationDateRanges56 = new List<IntPair>
		{
			new IntPair(0, 6),
			new IntPair(6, 12),
			new IntPair(12, 24),
			new IntPair(0, 0)
		};
		short[] settlementBlockCore56 = new short[1] { 35 };
		sbyte[] organizationId56 = new sbyte[1] { 37 };
		List<short[]> developedBlockCore56 = new List<short[]>
		{
			new short[2] { 39, 33 },
			new short[2] { 40, 33 },
			new short[2] { 41, 33 },
			new short[2] { 42, 33 },
			new short[2] { 43, 33 },
			new short[2] { 44, 33 },
			new short[2] { 45, 33 },
			new short[2] { 46, 33 },
			new short[2] { 47, 33 },
			new short[2] { 48, 33 },
			new short[2] { 49, 33 },
			new short[2] { 50, 33 },
			new short[2] { 51, 33 },
			new short[2] { 52, 33 },
			new short[2] { 53, 33 },
			new short[2] { 54, 33 },
			new short[2] { 55, 33 },
			new short[2] { 56, 33 },
			new short[2] { 109, 10 },
			new short[2] { 110, 10 },
			new short[2] { 111, 10 }
		};
		List<short[]> normalBlockCore56 = new List<short[]>
		{
			new short[2] { 57, 33 },
			new short[2] { 58, 33 },
			new short[2] { 59, 33 },
			new short[2] { 63, 33 },
			new short[2] { 64, 33 },
			new short[2] { 65, 33 },
			new short[2] { 69, 33 },
			new short[2] { 70, 33 },
			new short[2] { 71, 33 },
			new short[2] { 75, 33 },
			new short[2] { 76, 33 },
			new short[2] { 77, 33 },
			new short[2] { 81, 33 },
			new short[2] { 82, 33 },
			new short[2] { 83, 33 },
			new short[2] { 87, 33 },
			new short[2] { 88, 33 },
			new short[2] { 89, 33 },
			new short[2] { 112, 30 },
			new short[2] { 113, 30 },
			new short[2] { 114, 30 },
			new short[2] { 124, 30 }
		};
		List<short[]> wildBlockCore56 = new List<short[]>
		{
			new short[2] { 93, 33 },
			new short[2] { 94, 33 },
			new short[2] { 95, 33 },
			new short[2] { 96, 33 },
			new short[2] { 97, 33 },
			new short[2] { 98, 33 },
			new short[2] { 99, 33 },
			new short[2] { 100, 33 },
			new short[2] { 101, 33 },
			new short[2] { 102, 33 },
			new short[2] { 103, 33 },
			new short[2] { 104, 33 },
			new short[2] { 105, 33 },
			new short[2] { 106, 33 },
			new short[2] { 107, 33 },
			new short[2] { 108, 33 },
			new short[2] { 115, 40 },
			new short[2] { 116, 40 },
			new short[2] { 117, 40 },
			new short[2] { 124, 30 }
		};
		List<short[]> bigBaseBlockCore56 = new List<short[]>
		{
			new short[2] { 72, 1 },
			new short[2] { 78, 1 },
			new short[2] { 84, 1 },
			new short[2] { 91, 1 },
			new short[2] { 92, 1 }
		};
		list3 = new List<short[]>();
		List<short[]> seriesBlockCore56 = list3;
		list3 = new List<short[]>();
		List<short[]> encircleBlockCore56 = list3;
		short[] sceneryBlockCore56 = new short[0];
		list4 = new List<short>();
		List<short> lovingItemSubTypes56 = list4;
		list4 = new List<short>();
		dataArray56.Add(new MapAreaItem(55, 4, 240, 241, 20, worldMapPos56, neighborAreas56, "largemap_part_1_changgui_1", miniMapIcons56, null, miniMapPos56, 0, 0, 5, enemyNests56, enemyNestCreationDateRanges56, settlementBlockCore56, organizationId56, -1, 0, null, developedBlockCore56, normalBlockCore56, wildBlockCore56, bigBaseBlockCore56, seriesBlockCore56, encircleBlockCore56, sceneryBlockCore56, 242, 243, null, null, EMapAreaAreaDirection.South, lovingItemSubTypes56, list4, showDarkAshStatus: true));
		List<MapAreaItem> dataArray57 = _dataArray;
		sbyte[] worldMapPos57 = new sbyte[2] { 10, 13 };
		AreaTravelRoute[] neighborAreas57 = new AreaTravelRoute[1]
		{
			new AreaTravelRoute(63, 24, new int[4] { 10, 14, 10, 15 })
		};
		string[] miniMapIcons57 = new string[4] { "largemap_part_2_icon_hei_0", "largemap_part_2_icon_jin_0", "largemap_part_2_icon_lan_0", "largemap_part_2_icon_hui_0" };
		short[] miniMapPos57 = new short[2] { 100, 240 };
		List<EnemyNestCreationInfo[]> enemyNests57 = new List<EnemyNestCreationInfo[]>
		{
			new EnemyNestCreationInfo[2]
			{
				new EnemyNestCreationInfo(0, 2),
				new EnemyNestCreationInfo(1, 2)
			},
			new EnemyNestCreationInfo[2]
			{
				new EnemyNestCreationInfo(1, 2),
				new EnemyNestCreationInfo(2, 3)
			},
			new EnemyNestCreationInfo[4]
			{
				new EnemyNestCreationInfo(2, 3),
				new EnemyNestCreationInfo(3, 3),
				new EnemyNestCreationInfo(4, 4),
				new EnemyNestCreationInfo(5, 4)
			},
			new EnemyNestCreationInfo[1]
			{
				new EnemyNestCreationInfo(12, 2)
			}
		};
		List<IntPair> enemyNestCreationDateRanges57 = new List<IntPair>
		{
			new IntPair(0, 6),
			new IntPair(6, 12),
			new IntPair(12, 24),
			new IntPair(0, 0)
		};
		short[] settlementBlockCore57 = new short[1] { 36 };
		sbyte[] organizationId57 = new sbyte[1] { 38 };
		List<short[]> developedBlockCore57 = new List<short[]>
		{
			new short[2] { 39, 33 },
			new short[2] { 40, 33 },
			new short[2] { 41, 33 },
			new short[2] { 42, 33 },
			new short[2] { 43, 33 },
			new short[2] { 44, 33 },
			new short[2] { 45, 33 },
			new short[2] { 46, 33 },
			new short[2] { 47, 33 },
			new short[2] { 48, 33 },
			new short[2] { 49, 33 },
			new short[2] { 50, 33 },
			new short[2] { 51, 33 },
			new short[2] { 52, 33 },
			new short[2] { 53, 33 },
			new short[2] { 54, 33 },
			new short[2] { 55, 33 },
			new short[2] { 56, 33 },
			new short[2] { 109, 10 },
			new short[2] { 110, 10 },
			new short[2] { 111, 10 }
		};
		List<short[]> normalBlockCore57 = new List<short[]>
		{
			new short[2] { 57, 33 },
			new short[2] { 58, 33 },
			new short[2] { 59, 33 },
			new short[2] { 63, 33 },
			new short[2] { 64, 33 },
			new short[2] { 65, 33 },
			new short[2] { 69, 33 },
			new short[2] { 70, 33 },
			new short[2] { 71, 33 },
			new short[2] { 75, 33 },
			new short[2] { 76, 33 },
			new short[2] { 77, 33 },
			new short[2] { 81, 33 },
			new short[2] { 82, 33 },
			new short[2] { 83, 33 },
			new short[2] { 87, 33 },
			new short[2] { 88, 33 },
			new short[2] { 89, 33 },
			new short[2] { 112, 30 },
			new short[2] { 113, 30 },
			new short[2] { 114, 30 },
			new short[2] { 124, 30 }
		};
		List<short[]> wildBlockCore57 = new List<short[]>
		{
			new short[2] { 93, 33 },
			new short[2] { 94, 33 },
			new short[2] { 95, 33 },
			new short[2] { 96, 33 },
			new short[2] { 97, 33 },
			new short[2] { 98, 33 },
			new short[2] { 99, 33 },
			new short[2] { 100, 33 },
			new short[2] { 101, 33 },
			new short[2] { 102, 33 },
			new short[2] { 103, 33 },
			new short[2] { 104, 33 },
			new short[2] { 105, 33 },
			new short[2] { 106, 33 },
			new short[2] { 107, 33 },
			new short[2] { 108, 33 },
			new short[2] { 115, 40 },
			new short[2] { 116, 40 },
			new short[2] { 117, 40 },
			new short[2] { 124, 30 }
		};
		List<short[]> bigBaseBlockCore57 = new List<short[]>
		{
			new short[2] { 72, 1 },
			new short[2] { 78, 1 },
			new short[2] { 84, 1 },
			new short[2] { 91, 1 },
			new short[2] { 92, 1 }
		};
		list3 = new List<short[]>();
		List<short[]> seriesBlockCore57 = list3;
		list3 = new List<short[]>();
		List<short[]> encircleBlockCore57 = list3;
		short[] sceneryBlockCore57 = new short[0];
		list4 = new List<short>();
		List<short> lovingItemSubTypes57 = list4;
		list4 = new List<short>();
		dataArray57.Add(new MapAreaItem(56, 4, 244, 245, 20, worldMapPos57, neighborAreas57, "largemap_part_1_changgui_0", miniMapIcons57, null, miniMapPos57, 0, 0, 5, enemyNests57, enemyNestCreationDateRanges57, settlementBlockCore57, organizationId57, -1, 0, null, developedBlockCore57, normalBlockCore57, wildBlockCore57, bigBaseBlockCore57, seriesBlockCore57, encircleBlockCore57, sceneryBlockCore57, 246, 247, null, null, EMapAreaAreaDirection.South, lovingItemSubTypes57, list4, showDarkAshStatus: true));
		List<MapAreaItem> dataArray58 = _dataArray;
		sbyte[] worldMapPos58 = new sbyte[2] { 15, 9 };
		AreaTravelRoute[] neighborAreas58 = new AreaTravelRoute[1]
		{
			new AreaTravelRoute(84, 14, new int[2] { 14, 9 })
		};
		string[] miniMapIcons58 = new string[4] { "largemap_part_2_icon_hei_0", "largemap_part_2_icon_jin_0", "largemap_part_2_icon_lan_0", "largemap_part_2_icon_hui_0" };
		short[] miniMapPos58 = new short[2] { 300, 80 };
		List<EnemyNestCreationInfo[]> enemyNests58 = new List<EnemyNestCreationInfo[]>
		{
			new EnemyNestCreationInfo[2]
			{
				new EnemyNestCreationInfo(0, 2),
				new EnemyNestCreationInfo(1, 2)
			},
			new EnemyNestCreationInfo[2]
			{
				new EnemyNestCreationInfo(1, 2),
				new EnemyNestCreationInfo(2, 3)
			},
			new EnemyNestCreationInfo[4]
			{
				new EnemyNestCreationInfo(2, 3),
				new EnemyNestCreationInfo(3, 3),
				new EnemyNestCreationInfo(4, 4),
				new EnemyNestCreationInfo(5, 4)
			},
			new EnemyNestCreationInfo[1]
			{
				new EnemyNestCreationInfo(12, 2)
			}
		};
		List<IntPair> enemyNestCreationDateRanges58 = new List<IntPair>
		{
			new IntPair(0, 6),
			new IntPair(6, 12),
			new IntPair(12, 24),
			new IntPair(0, 0)
		};
		short[] settlementBlockCore58 = new short[1] { 34 };
		sbyte[] organizationId58 = new sbyte[1] { 36 };
		List<short[]> developedBlockCore58 = new List<short[]>
		{
			new short[2] { 39, 33 },
			new short[2] { 40, 33 },
			new short[2] { 41, 33 },
			new short[2] { 42, 33 },
			new short[2] { 43, 33 },
			new short[2] { 44, 33 },
			new short[2] { 45, 33 },
			new short[2] { 46, 33 },
			new short[2] { 47, 33 },
			new short[2] { 48, 33 },
			new short[2] { 49, 33 },
			new short[2] { 50, 33 },
			new short[2] { 51, 33 },
			new short[2] { 52, 33 },
			new short[2] { 53, 33 },
			new short[2] { 54, 33 },
			new short[2] { 55, 33 },
			new short[2] { 56, 33 },
			new short[2] { 109, 10 },
			new short[2] { 110, 10 },
			new short[2] { 111, 10 }
		};
		List<short[]> normalBlockCore58 = new List<short[]>
		{
			new short[2] { 57, 33 },
			new short[2] { 58, 33 },
			new short[2] { 59, 33 },
			new short[2] { 63, 33 },
			new short[2] { 64, 33 },
			new short[2] { 65, 33 },
			new short[2] { 69, 33 },
			new short[2] { 70, 33 },
			new short[2] { 71, 33 },
			new short[2] { 75, 33 },
			new short[2] { 76, 33 },
			new short[2] { 77, 33 },
			new short[2] { 81, 33 },
			new short[2] { 82, 33 },
			new short[2] { 83, 33 },
			new short[2] { 87, 33 },
			new short[2] { 88, 33 },
			new short[2] { 89, 33 },
			new short[2] { 112, 30 },
			new short[2] { 113, 30 },
			new short[2] { 114, 30 },
			new short[2] { 124, 30 }
		};
		List<short[]> wildBlockCore58 = new List<short[]>
		{
			new short[2] { 93, 33 },
			new short[2] { 94, 33 },
			new short[2] { 95, 33 },
			new short[2] { 96, 33 },
			new short[2] { 97, 33 },
			new short[2] { 98, 33 },
			new short[2] { 99, 33 },
			new short[2] { 100, 33 },
			new short[2] { 101, 33 },
			new short[2] { 102, 33 },
			new short[2] { 103, 33 },
			new short[2] { 104, 33 },
			new short[2] { 105, 33 },
			new short[2] { 106, 33 },
			new short[2] { 107, 33 },
			new short[2] { 108, 33 },
			new short[2] { 115, 40 },
			new short[2] { 116, 40 },
			new short[2] { 117, 40 },
			new short[2] { 124, 30 }
		};
		List<short[]> bigBaseBlockCore58 = new List<short[]>
		{
			new short[2] { 72, 1 },
			new short[2] { 78, 1 },
			new short[2] { 84, 1 },
			new short[2] { 91, 1 },
			new short[2] { 92, 1 }
		};
		list3 = new List<short[]>();
		List<short[]> seriesBlockCore58 = list3;
		list3 = new List<short[]>();
		List<short[]> encircleBlockCore58 = list3;
		short[] sceneryBlockCore58 = new short[0];
		list4 = new List<short>();
		List<short> lovingItemSubTypes58 = list4;
		list4 = new List<short>();
		dataArray58.Add(new MapAreaItem(57, 4, 248, 249, 20, worldMapPos58, neighborAreas58, "largemap_part_1_changgui_0", miniMapIcons58, null, miniMapPos58, 0, 0, 5, enemyNests58, enemyNestCreationDateRanges58, settlementBlockCore58, organizationId58, -1, 0, null, developedBlockCore58, normalBlockCore58, wildBlockCore58, bigBaseBlockCore58, seriesBlockCore58, encircleBlockCore58, sceneryBlockCore58, 250, 251, null, null, EMapAreaAreaDirection.South, lovingItemSubTypes58, list4, showDarkAshStatus: true));
		List<MapAreaItem> dataArray59 = _dataArray;
		sbyte[] worldMapPos59 = new sbyte[2] { 14, 11 };
		AreaTravelRoute[] neighborAreas59 = new AreaTravelRoute[0];
		string[] miniMapIcons59 = new string[4] { "largemap_part_2_icon_hei_0", "largemap_part_2_icon_jin_0", "largemap_part_2_icon_lan_0", "largemap_part_2_icon_hui_0" };
		short[] miniMapPos59 = new short[2] { 260, 160 };
		List<EnemyNestCreationInfo[]> enemyNests59 = new List<EnemyNestCreationInfo[]>
		{
			new EnemyNestCreationInfo[2]
			{
				new EnemyNestCreationInfo(0, 2),
				new EnemyNestCreationInfo(1, 2)
			},
			new EnemyNestCreationInfo[2]
			{
				new EnemyNestCreationInfo(1, 2),
				new EnemyNestCreationInfo(2, 3)
			},
			new EnemyNestCreationInfo[4]
			{
				new EnemyNestCreationInfo(2, 3),
				new EnemyNestCreationInfo(3, 3),
				new EnemyNestCreationInfo(4, 4),
				new EnemyNestCreationInfo(5, 4)
			},
			new EnemyNestCreationInfo[1]
			{
				new EnemyNestCreationInfo(12, 2)
			}
		};
		List<IntPair> enemyNestCreationDateRanges59 = new List<IntPair>
		{
			new IntPair(0, 6),
			new IntPair(6, 12),
			new IntPair(12, 24),
			new IntPair(0, 0)
		};
		short[] settlementBlockCore59 = new short[1] { 34 };
		sbyte[] organizationId59 = new sbyte[1] { 36 };
		List<short[]> developedBlockCore59 = new List<short[]>
		{
			new short[2] { 39, 33 },
			new short[2] { 40, 33 },
			new short[2] { 41, 33 },
			new short[2] { 42, 33 },
			new short[2] { 43, 33 },
			new short[2] { 44, 33 },
			new short[2] { 45, 33 },
			new short[2] { 46, 33 },
			new short[2] { 47, 33 },
			new short[2] { 48, 33 },
			new short[2] { 49, 33 },
			new short[2] { 50, 33 },
			new short[2] { 51, 33 },
			new short[2] { 52, 33 },
			new short[2] { 53, 33 },
			new short[2] { 54, 33 },
			new short[2] { 55, 33 },
			new short[2] { 56, 33 },
			new short[2] { 109, 10 },
			new short[2] { 110, 10 },
			new short[2] { 111, 10 }
		};
		List<short[]> normalBlockCore59 = new List<short[]>
		{
			new short[2] { 57, 33 },
			new short[2] { 58, 33 },
			new short[2] { 59, 33 },
			new short[2] { 63, 33 },
			new short[2] { 64, 33 },
			new short[2] { 65, 33 },
			new short[2] { 69, 33 },
			new short[2] { 70, 33 },
			new short[2] { 71, 33 },
			new short[2] { 75, 33 },
			new short[2] { 76, 33 },
			new short[2] { 77, 33 },
			new short[2] { 81, 33 },
			new short[2] { 82, 33 },
			new short[2] { 83, 33 },
			new short[2] { 87, 33 },
			new short[2] { 88, 33 },
			new short[2] { 89, 33 },
			new short[2] { 112, 30 },
			new short[2] { 113, 30 },
			new short[2] { 114, 30 },
			new short[2] { 124, 30 }
		};
		List<short[]> wildBlockCore59 = new List<short[]>
		{
			new short[2] { 93, 33 },
			new short[2] { 94, 33 },
			new short[2] { 95, 33 },
			new short[2] { 96, 33 },
			new short[2] { 97, 33 },
			new short[2] { 98, 33 },
			new short[2] { 99, 33 },
			new short[2] { 100, 33 },
			new short[2] { 101, 33 },
			new short[2] { 102, 33 },
			new short[2] { 103, 33 },
			new short[2] { 104, 33 },
			new short[2] { 105, 33 },
			new short[2] { 106, 33 },
			new short[2] { 107, 33 },
			new short[2] { 108, 33 },
			new short[2] { 115, 40 },
			new short[2] { 116, 40 },
			new short[2] { 117, 40 },
			new short[2] { 124, 30 }
		};
		List<short[]> bigBaseBlockCore59 = new List<short[]>
		{
			new short[2] { 72, 1 },
			new short[2] { 78, 1 },
			new short[2] { 84, 1 },
			new short[2] { 91, 1 },
			new short[2] { 92, 1 }
		};
		list3 = new List<short[]>();
		List<short[]> seriesBlockCore59 = list3;
		list3 = new List<short[]>();
		List<short[]> encircleBlockCore59 = list3;
		short[] sceneryBlockCore59 = new short[0];
		list4 = new List<short>();
		List<short> lovingItemSubTypes59 = list4;
		list4 = new List<short>();
		dataArray59.Add(new MapAreaItem(58, 4, 252, 253, 20, worldMapPos59, neighborAreas59, "largemap_part_1_changgui_0", miniMapIcons59, null, miniMapPos59, 0, 0, 5, enemyNests59, enemyNestCreationDateRanges59, settlementBlockCore59, organizationId59, -1, 0, null, developedBlockCore59, normalBlockCore59, wildBlockCore59, bigBaseBlockCore59, seriesBlockCore59, encircleBlockCore59, sceneryBlockCore59, 254, 255, null, null, EMapAreaAreaDirection.South, lovingItemSubTypes59, list4, showDarkAshStatus: true));
		List<MapAreaItem> dataArray60 = _dataArray;
		sbyte[] worldMapPos60 = new sbyte[2] { 12, 20 };
		AreaTravelRoute[] neighborAreas60 = new AreaTravelRoute[1]
		{
			new AreaTravelRoute(60, 6, null)
		};
		string[] miniMapIcons60 = new string[4] { "largemap_part_2_icon_hei_2", "largemap_part_2_icon_jin_2", "largemap_part_2_icon_lan_2", "largemap_part_2_icon_hui_2" };
		short[] miniMapPos60 = new short[2] { 200, 270 };
		List<EnemyNestCreationInfo[]> enemyNests60 = new List<EnemyNestCreationInfo[]>
		{
			new EnemyNestCreationInfo[2]
			{
				new EnemyNestCreationInfo(0, 2),
				new EnemyNestCreationInfo(1, 2)
			},
			new EnemyNestCreationInfo[2]
			{
				new EnemyNestCreationInfo(1, 2),
				new EnemyNestCreationInfo(2, 3)
			},
			new EnemyNestCreationInfo[4]
			{
				new EnemyNestCreationInfo(2, 3),
				new EnemyNestCreationInfo(3, 3),
				new EnemyNestCreationInfo(4, 4),
				new EnemyNestCreationInfo(5, 4)
			},
			new EnemyNestCreationInfo[1]
			{
				new EnemyNestCreationInfo(12, 2)
			}
		};
		List<IntPair> enemyNestCreationDateRanges60 = new List<IntPair>
		{
			new IntPair(0, 6),
			new IntPair(6, 12),
			new IntPair(12, 24),
			new IntPair(0, 0)
		};
		short[] settlementBlockCore60 = new short[1] { 35 };
		sbyte[] organizationId60 = new sbyte[1] { 37 };
		List<short[]> developedBlockCore60 = new List<short[]>
		{
			new short[2] { 39, 33 },
			new short[2] { 40, 33 },
			new short[2] { 41, 33 },
			new short[2] { 42, 33 },
			new short[2] { 43, 33 },
			new short[2] { 44, 33 },
			new short[2] { 45, 33 },
			new short[2] { 46, 33 },
			new short[2] { 47, 33 },
			new short[2] { 48, 33 },
			new short[2] { 49, 33 },
			new short[2] { 50, 33 },
			new short[2] { 51, 33 },
			new short[2] { 52, 33 },
			new short[2] { 53, 33 },
			new short[2] { 54, 33 },
			new short[2] { 55, 33 },
			new short[2] { 56, 33 },
			new short[2] { 109, 10 },
			new short[2] { 110, 10 },
			new short[2] { 111, 10 }
		};
		List<short[]> normalBlockCore60 = new List<short[]>
		{
			new short[2] { 57, 33 },
			new short[2] { 58, 33 },
			new short[2] { 59, 33 },
			new short[2] { 63, 33 },
			new short[2] { 64, 33 },
			new short[2] { 65, 33 },
			new short[2] { 69, 33 },
			new short[2] { 70, 33 },
			new short[2] { 71, 33 },
			new short[2] { 75, 33 },
			new short[2] { 76, 33 },
			new short[2] { 77, 33 },
			new short[2] { 81, 33 },
			new short[2] { 82, 33 },
			new short[2] { 83, 33 },
			new short[2] { 87, 33 },
			new short[2] { 88, 33 },
			new short[2] { 89, 33 },
			new short[2] { 112, 30 },
			new short[2] { 113, 30 },
			new short[2] { 114, 30 },
			new short[2] { 124, 30 }
		};
		List<short[]> wildBlockCore60 = new List<short[]>
		{
			new short[2] { 93, 33 },
			new short[2] { 94, 33 },
			new short[2] { 95, 33 },
			new short[2] { 96, 33 },
			new short[2] { 97, 33 },
			new short[2] { 98, 33 },
			new short[2] { 99, 33 },
			new short[2] { 100, 33 },
			new short[2] { 101, 33 },
			new short[2] { 102, 33 },
			new short[2] { 103, 33 },
			new short[2] { 104, 33 },
			new short[2] { 105, 33 },
			new short[2] { 106, 33 },
			new short[2] { 107, 33 },
			new short[2] { 108, 33 },
			new short[2] { 115, 40 },
			new short[2] { 116, 40 },
			new short[2] { 117, 40 },
			new short[2] { 124, 30 }
		};
		List<short[]> bigBaseBlockCore60 = new List<short[]>
		{
			new short[2] { 73, 1 },
			new short[2] { 74, 1 },
			new short[2] { 60, 1 },
			new short[2] { 84, 1 },
			new short[2] { 66, 1 }
		};
		list3 = new List<short[]>();
		List<short[]> seriesBlockCore60 = list3;
		list3 = new List<short[]>();
		List<short[]> encircleBlockCore60 = list3;
		short[] sceneryBlockCore60 = new short[0];
		list4 = new List<short>();
		List<short> lovingItemSubTypes60 = list4;
		list4 = new List<short>();
		dataArray60.Add(new MapAreaItem(59, 5, 256, 257, 20, worldMapPos60, neighborAreas60, "largemap_part_1_changgui_2", miniMapIcons60, null, miniMapPos60, 0, 0, 5, enemyNests60, enemyNestCreationDateRanges60, settlementBlockCore60, organizationId60, -1, 0, null, developedBlockCore60, normalBlockCore60, wildBlockCore60, bigBaseBlockCore60, seriesBlockCore60, encircleBlockCore60, sceneryBlockCore60, 258, 259, null, null, EMapAreaAreaDirection.South, lovingItemSubTypes60, list4, showDarkAshStatus: true));
	}

	private void CreateItems1()
	{
		List<MapAreaItem> dataArray = _dataArray;
		sbyte[] worldMapPos = new sbyte[2] { 12, 19 };
		AreaTravelRoute[] neighborAreas = new AreaTravelRoute[1]
		{
			new AreaTravelRoute(61, 6, null)
		};
		string[] miniMapIcons = new string[4] { "largemap_part_2_icon_hei_0", "largemap_part_2_icon_jin_0", "largemap_part_2_icon_lan_0", "largemap_part_2_icon_hui_0" };
		short[] miniMapPos = new short[2] { 200, 230 };
		List<EnemyNestCreationInfo[]> enemyNests = new List<EnemyNestCreationInfo[]>
		{
			new EnemyNestCreationInfo[2]
			{
				new EnemyNestCreationInfo(0, 2),
				new EnemyNestCreationInfo(1, 2)
			},
			new EnemyNestCreationInfo[2]
			{
				new EnemyNestCreationInfo(1, 2),
				new EnemyNestCreationInfo(2, 3)
			},
			new EnemyNestCreationInfo[4]
			{
				new EnemyNestCreationInfo(2, 3),
				new EnemyNestCreationInfo(3, 3),
				new EnemyNestCreationInfo(4, 4),
				new EnemyNestCreationInfo(5, 4)
			},
			new EnemyNestCreationInfo[1]
			{
				new EnemyNestCreationInfo(12, 2)
			}
		};
		List<IntPair> enemyNestCreationDateRanges = new List<IntPair>
		{
			new IntPair(0, 6),
			new IntPair(6, 12),
			new IntPair(12, 24),
			new IntPair(0, 0)
		};
		short[] settlementBlockCore = new short[1] { 34 };
		sbyte[] organizationId = new sbyte[1] { 36 };
		List<short[]> developedBlockCore = new List<short[]>
		{
			new short[2] { 39, 33 },
			new short[2] { 40, 33 },
			new short[2] { 41, 33 },
			new short[2] { 42, 33 },
			new short[2] { 43, 33 },
			new short[2] { 44, 33 },
			new short[2] { 45, 33 },
			new short[2] { 46, 33 },
			new short[2] { 47, 33 },
			new short[2] { 48, 33 },
			new short[2] { 49, 33 },
			new short[2] { 50, 33 },
			new short[2] { 51, 33 },
			new short[2] { 52, 33 },
			new short[2] { 53, 33 },
			new short[2] { 54, 33 },
			new short[2] { 55, 33 },
			new short[2] { 56, 33 },
			new short[2] { 109, 10 },
			new short[2] { 110, 10 },
			new short[2] { 111, 10 }
		};
		List<short[]> normalBlockCore = new List<short[]>
		{
			new short[2] { 57, 33 },
			new short[2] { 58, 33 },
			new short[2] { 59, 33 },
			new short[2] { 63, 33 },
			new short[2] { 64, 33 },
			new short[2] { 65, 33 },
			new short[2] { 69, 33 },
			new short[2] { 70, 33 },
			new short[2] { 71, 33 },
			new short[2] { 75, 33 },
			new short[2] { 76, 33 },
			new short[2] { 77, 33 },
			new short[2] { 81, 33 },
			new short[2] { 82, 33 },
			new short[2] { 83, 33 },
			new short[2] { 87, 33 },
			new short[2] { 88, 33 },
			new short[2] { 89, 33 },
			new short[2] { 112, 30 },
			new short[2] { 113, 30 },
			new short[2] { 114, 30 },
			new short[2] { 124, 30 }
		};
		List<short[]> wildBlockCore = new List<short[]>
		{
			new short[2] { 93, 33 },
			new short[2] { 94, 33 },
			new short[2] { 95, 33 },
			new short[2] { 96, 33 },
			new short[2] { 97, 33 },
			new short[2] { 98, 33 },
			new short[2] { 99, 33 },
			new short[2] { 100, 33 },
			new short[2] { 101, 33 },
			new short[2] { 102, 33 },
			new short[2] { 103, 33 },
			new short[2] { 104, 33 },
			new short[2] { 105, 33 },
			new short[2] { 106, 33 },
			new short[2] { 107, 33 },
			new short[2] { 108, 33 },
			new short[2] { 115, 40 },
			new short[2] { 116, 40 },
			new short[2] { 117, 40 },
			new short[2] { 124, 30 }
		};
		List<short[]> bigBaseBlockCore = new List<short[]>
		{
			new short[2] { 73, 1 },
			new short[2] { 74, 1 },
			new short[2] { 60, 1 },
			new short[2] { 84, 1 },
			new short[2] { 66, 1 }
		};
		List<short[]> list = new List<short[]>();
		List<short[]> seriesBlockCore = list;
		list = new List<short[]>();
		List<short[]> encircleBlockCore = list;
		short[] sceneryBlockCore = new short[0];
		List<short> list2 = new List<short>();
		List<short> lovingItemSubTypes = list2;
		list2 = new List<short>();
		dataArray.Add(new MapAreaItem(60, 5, 260, 261, 20, worldMapPos, neighborAreas, "largemap_part_1_changgui_0", miniMapIcons, null, miniMapPos, 0, 0, 5, enemyNests, enemyNestCreationDateRanges, settlementBlockCore, organizationId, -1, 0, null, developedBlockCore, normalBlockCore, wildBlockCore, bigBaseBlockCore, seriesBlockCore, encircleBlockCore, sceneryBlockCore, 262, 263, null, null, EMapAreaAreaDirection.South, lovingItemSubTypes, list2, showDarkAshStatus: true));
		List<MapAreaItem> dataArray2 = _dataArray;
		sbyte[] worldMapPos2 = new sbyte[2] { 13, 19 };
		AreaTravelRoute[] neighborAreas2 = new AreaTravelRoute[1]
		{
			new AreaTravelRoute(62, 6, null)
		};
		string[] miniMapIcons2 = new string[4] { "largemap_part_2_icon_hei_0", "largemap_part_2_icon_jin_0", "largemap_part_2_icon_lan_0", "largemap_part_2_icon_hui_0" };
		short[] miniMapPos2 = new short[2] { 240, 230 };
		List<EnemyNestCreationInfo[]> enemyNests2 = new List<EnemyNestCreationInfo[]>
		{
			new EnemyNestCreationInfo[2]
			{
				new EnemyNestCreationInfo(0, 2),
				new EnemyNestCreationInfo(1, 2)
			},
			new EnemyNestCreationInfo[2]
			{
				new EnemyNestCreationInfo(1, 2),
				new EnemyNestCreationInfo(2, 3)
			},
			new EnemyNestCreationInfo[4]
			{
				new EnemyNestCreationInfo(2, 3),
				new EnemyNestCreationInfo(3, 3),
				new EnemyNestCreationInfo(4, 4),
				new EnemyNestCreationInfo(5, 4)
			},
			new EnemyNestCreationInfo[1]
			{
				new EnemyNestCreationInfo(12, 2)
			}
		};
		List<IntPair> enemyNestCreationDateRanges2 = new List<IntPair>
		{
			new IntPair(0, 6),
			new IntPair(6, 12),
			new IntPair(12, 24),
			new IntPair(0, 0)
		};
		short[] settlementBlockCore2 = new short[1] { 35 };
		sbyte[] organizationId2 = new sbyte[1] { 37 };
		List<short[]> developedBlockCore2 = new List<short[]>
		{
			new short[2] { 39, 33 },
			new short[2] { 40, 33 },
			new short[2] { 41, 33 },
			new short[2] { 42, 33 },
			new short[2] { 43, 33 },
			new short[2] { 44, 33 },
			new short[2] { 45, 33 },
			new short[2] { 46, 33 },
			new short[2] { 47, 33 },
			new short[2] { 48, 33 },
			new short[2] { 49, 33 },
			new short[2] { 50, 33 },
			new short[2] { 51, 33 },
			new short[2] { 52, 33 },
			new short[2] { 53, 33 },
			new short[2] { 54, 33 },
			new short[2] { 55, 33 },
			new short[2] { 56, 33 },
			new short[2] { 109, 10 },
			new short[2] { 110, 10 },
			new short[2] { 111, 10 }
		};
		List<short[]> normalBlockCore2 = new List<short[]>
		{
			new short[2] { 57, 33 },
			new short[2] { 58, 33 },
			new short[2] { 59, 33 },
			new short[2] { 63, 33 },
			new short[2] { 64, 33 },
			new short[2] { 65, 33 },
			new short[2] { 69, 33 },
			new short[2] { 70, 33 },
			new short[2] { 71, 33 },
			new short[2] { 75, 33 },
			new short[2] { 76, 33 },
			new short[2] { 77, 33 },
			new short[2] { 81, 33 },
			new short[2] { 82, 33 },
			new short[2] { 83, 33 },
			new short[2] { 87, 33 },
			new short[2] { 88, 33 },
			new short[2] { 89, 33 },
			new short[2] { 112, 30 },
			new short[2] { 113, 30 },
			new short[2] { 114, 30 },
			new short[2] { 124, 30 }
		};
		List<short[]> wildBlockCore2 = new List<short[]>
		{
			new short[2] { 93, 33 },
			new short[2] { 94, 33 },
			new short[2] { 95, 33 },
			new short[2] { 96, 33 },
			new short[2] { 97, 33 },
			new short[2] { 98, 33 },
			new short[2] { 99, 33 },
			new short[2] { 100, 33 },
			new short[2] { 101, 33 },
			new short[2] { 102, 33 },
			new short[2] { 103, 33 },
			new short[2] { 104, 33 },
			new short[2] { 105, 33 },
			new short[2] { 106, 33 },
			new short[2] { 107, 33 },
			new short[2] { 108, 33 },
			new short[2] { 115, 40 },
			new short[2] { 116, 40 },
			new short[2] { 117, 40 },
			new short[2] { 124, 30 }
		};
		List<short[]> bigBaseBlockCore2 = new List<short[]>
		{
			new short[2] { 73, 1 },
			new short[2] { 74, 1 },
			new short[2] { 60, 1 },
			new short[2] { 84, 1 },
			new short[2] { 66, 1 }
		};
		list = new List<short[]>();
		List<short[]> seriesBlockCore2 = list;
		list = new List<short[]>();
		List<short[]> encircleBlockCore2 = list;
		short[] sceneryBlockCore2 = new short[0];
		list2 = new List<short>();
		List<short> lovingItemSubTypes2 = list2;
		list2 = new List<short>();
		dataArray2.Add(new MapAreaItem(61, 5, 264, 265, 20, worldMapPos2, neighborAreas2, "largemap_part_1_changgui_0", miniMapIcons2, null, miniMapPos2, 0, 0, 5, enemyNests2, enemyNestCreationDateRanges2, settlementBlockCore2, organizationId2, -1, 0, null, developedBlockCore2, normalBlockCore2, wildBlockCore2, bigBaseBlockCore2, seriesBlockCore2, encircleBlockCore2, sceneryBlockCore2, 266, 267, null, null, EMapAreaAreaDirection.South, lovingItemSubTypes2, list2, showDarkAshStatus: true));
		List<MapAreaItem> dataArray3 = _dataArray;
		sbyte[] worldMapPos3 = new sbyte[2] { 13, 18 };
		AreaTravelRoute[] neighborAreas3 = new AreaTravelRoute[0];
		string[] miniMapIcons3 = new string[4] { "largemap_part_2_icon_hei_2", "largemap_part_2_icon_jin_2", "largemap_part_2_icon_lan_2", "largemap_part_2_icon_hui_2" };
		short[] miniMapPos3 = new short[2] { 240, 190 };
		List<EnemyNestCreationInfo[]> enemyNests3 = new List<EnemyNestCreationInfo[]>
		{
			new EnemyNestCreationInfo[2]
			{
				new EnemyNestCreationInfo(0, 2),
				new EnemyNestCreationInfo(1, 2)
			},
			new EnemyNestCreationInfo[2]
			{
				new EnemyNestCreationInfo(1, 2),
				new EnemyNestCreationInfo(2, 3)
			},
			new EnemyNestCreationInfo[4]
			{
				new EnemyNestCreationInfo(2, 3),
				new EnemyNestCreationInfo(3, 3),
				new EnemyNestCreationInfo(4, 4),
				new EnemyNestCreationInfo(5, 4)
			},
			new EnemyNestCreationInfo[1]
			{
				new EnemyNestCreationInfo(12, 2)
			}
		};
		List<IntPair> enemyNestCreationDateRanges3 = new List<IntPair>
		{
			new IntPair(0, 6),
			new IntPair(6, 12),
			new IntPair(12, 24),
			new IntPair(0, 0)
		};
		short[] settlementBlockCore3 = new short[1] { 36 };
		sbyte[] organizationId3 = new sbyte[1] { 38 };
		List<short[]> developedBlockCore3 = new List<short[]>
		{
			new short[2] { 39, 33 },
			new short[2] { 40, 33 },
			new short[2] { 41, 33 },
			new short[2] { 42, 33 },
			new short[2] { 43, 33 },
			new short[2] { 44, 33 },
			new short[2] { 45, 33 },
			new short[2] { 46, 33 },
			new short[2] { 47, 33 },
			new short[2] { 48, 33 },
			new short[2] { 49, 33 },
			new short[2] { 50, 33 },
			new short[2] { 51, 33 },
			new short[2] { 52, 33 },
			new short[2] { 53, 33 },
			new short[2] { 54, 33 },
			new short[2] { 55, 33 },
			new short[2] { 56, 33 },
			new short[2] { 109, 10 },
			new short[2] { 110, 10 },
			new short[2] { 111, 10 }
		};
		List<short[]> normalBlockCore3 = new List<short[]>
		{
			new short[2] { 57, 33 },
			new short[2] { 58, 33 },
			new short[2] { 59, 33 },
			new short[2] { 63, 33 },
			new short[2] { 64, 33 },
			new short[2] { 65, 33 },
			new short[2] { 69, 33 },
			new short[2] { 70, 33 },
			new short[2] { 71, 33 },
			new short[2] { 75, 33 },
			new short[2] { 76, 33 },
			new short[2] { 77, 33 },
			new short[2] { 81, 33 },
			new short[2] { 82, 33 },
			new short[2] { 83, 33 },
			new short[2] { 87, 33 },
			new short[2] { 88, 33 },
			new short[2] { 89, 33 },
			new short[2] { 112, 30 },
			new short[2] { 113, 30 },
			new short[2] { 114, 30 },
			new short[2] { 124, 30 }
		};
		List<short[]> wildBlockCore3 = new List<short[]>
		{
			new short[2] { 93, 33 },
			new short[2] { 94, 33 },
			new short[2] { 95, 33 },
			new short[2] { 96, 33 },
			new short[2] { 97, 33 },
			new short[2] { 98, 33 },
			new short[2] { 99, 33 },
			new short[2] { 100, 33 },
			new short[2] { 101, 33 },
			new short[2] { 102, 33 },
			new short[2] { 103, 33 },
			new short[2] { 104, 33 },
			new short[2] { 105, 33 },
			new short[2] { 106, 33 },
			new short[2] { 107, 33 },
			new short[2] { 108, 33 },
			new short[2] { 115, 40 },
			new short[2] { 116, 40 },
			new short[2] { 117, 40 },
			new short[2] { 124, 30 }
		};
		List<short[]> bigBaseBlockCore3 = new List<short[]>
		{
			new short[2] { 73, 1 },
			new short[2] { 74, 1 },
			new short[2] { 60, 1 },
			new short[2] { 84, 1 },
			new short[2] { 66, 1 }
		};
		list = new List<short[]>();
		List<short[]> seriesBlockCore3 = list;
		list = new List<short[]>();
		List<short[]> encircleBlockCore3 = list;
		short[] sceneryBlockCore3 = new short[0];
		list2 = new List<short>();
		List<short> lovingItemSubTypes3 = list2;
		list2 = new List<short>();
		dataArray3.Add(new MapAreaItem(62, 5, 268, 269, 20, worldMapPos3, neighborAreas3, "largemap_part_1_changgui_2", miniMapIcons3, null, miniMapPos3, 0, 0, 5, enemyNests3, enemyNestCreationDateRanges3, settlementBlockCore3, organizationId3, -1, 0, null, developedBlockCore3, normalBlockCore3, wildBlockCore3, bigBaseBlockCore3, seriesBlockCore3, encircleBlockCore3, sceneryBlockCore3, 270, 271, null, null, EMapAreaAreaDirection.South, lovingItemSubTypes3, list2, showDarkAshStatus: true));
		List<MapAreaItem> dataArray4 = _dataArray;
		sbyte[] worldMapPos4 = new sbyte[2] { 10, 16 };
		AreaTravelRoute[] neighborAreas4 = new AreaTravelRoute[2]
		{
			new AreaTravelRoute(64, 12, new int[2] { 10, 17 }),
			new AreaTravelRoute(107, 42, new int[8] { 9, 16, 8, 16, 7, 16, 7, 15 })
		};
		string[] miniMapIcons4 = new string[4] { "largemap_part_2_icon_hei_2", "largemap_part_2_icon_jin_2", "largemap_part_2_icon_lan_2", "largemap_part_2_icon_hui_2" };
		short[] miniMapPos4 = new short[2] { 120, 110 };
		List<EnemyNestCreationInfo[]> enemyNests4 = new List<EnemyNestCreationInfo[]>
		{
			new EnemyNestCreationInfo[2]
			{
				new EnemyNestCreationInfo(0, 2),
				new EnemyNestCreationInfo(1, 2)
			},
			new EnemyNestCreationInfo[2]
			{
				new EnemyNestCreationInfo(1, 2),
				new EnemyNestCreationInfo(2, 3)
			},
			new EnemyNestCreationInfo[4]
			{
				new EnemyNestCreationInfo(2, 3),
				new EnemyNestCreationInfo(3, 3),
				new EnemyNestCreationInfo(4, 4),
				new EnemyNestCreationInfo(5, 4)
			},
			new EnemyNestCreationInfo[1]
			{
				new EnemyNestCreationInfo(12, 2)
			}
		};
		List<IntPair> enemyNestCreationDateRanges4 = new List<IntPair>
		{
			new IntPair(0, 6),
			new IntPair(6, 12),
			new IntPair(12, 24),
			new IntPair(0, 0)
		};
		short[] settlementBlockCore4 = new short[1] { 35 };
		sbyte[] organizationId4 = new sbyte[1] { 37 };
		List<short[]> developedBlockCore4 = new List<short[]>
		{
			new short[2] { 39, 33 },
			new short[2] { 40, 33 },
			new short[2] { 41, 33 },
			new short[2] { 42, 33 },
			new short[2] { 43, 33 },
			new short[2] { 44, 33 },
			new short[2] { 45, 33 },
			new short[2] { 46, 33 },
			new short[2] { 47, 33 },
			new short[2] { 48, 33 },
			new short[2] { 49, 33 },
			new short[2] { 50, 33 },
			new short[2] { 51, 33 },
			new short[2] { 52, 33 },
			new short[2] { 53, 33 },
			new short[2] { 54, 33 },
			new short[2] { 55, 33 },
			new short[2] { 56, 33 },
			new short[2] { 109, 10 },
			new short[2] { 110, 10 },
			new short[2] { 111, 10 }
		};
		List<short[]> normalBlockCore4 = new List<short[]>
		{
			new short[2] { 57, 33 },
			new short[2] { 58, 33 },
			new short[2] { 59, 33 },
			new short[2] { 63, 33 },
			new short[2] { 64, 33 },
			new short[2] { 65, 33 },
			new short[2] { 69, 33 },
			new short[2] { 70, 33 },
			new short[2] { 71, 33 },
			new short[2] { 75, 33 },
			new short[2] { 76, 33 },
			new short[2] { 77, 33 },
			new short[2] { 81, 33 },
			new short[2] { 82, 33 },
			new short[2] { 83, 33 },
			new short[2] { 87, 33 },
			new short[2] { 88, 33 },
			new short[2] { 89, 33 },
			new short[2] { 112, 30 },
			new short[2] { 113, 30 },
			new short[2] { 114, 30 },
			new short[2] { 124, 30 }
		};
		List<short[]> wildBlockCore4 = new List<short[]>
		{
			new short[2] { 93, 33 },
			new short[2] { 94, 33 },
			new short[2] { 95, 33 },
			new short[2] { 96, 33 },
			new short[2] { 97, 33 },
			new short[2] { 98, 33 },
			new short[2] { 99, 33 },
			new short[2] { 100, 33 },
			new short[2] { 101, 33 },
			new short[2] { 102, 33 },
			new short[2] { 103, 33 },
			new short[2] { 104, 33 },
			new short[2] { 105, 33 },
			new short[2] { 106, 33 },
			new short[2] { 107, 33 },
			new short[2] { 108, 33 },
			new short[2] { 115, 40 },
			new short[2] { 116, 40 },
			new short[2] { 117, 40 },
			new short[2] { 124, 30 }
		};
		List<short[]> bigBaseBlockCore4 = new List<short[]>
		{
			new short[2] { 73, 1 },
			new short[2] { 74, 1 },
			new short[2] { 60, 1 },
			new short[2] { 84, 1 },
			new short[2] { 66, 1 }
		};
		list = new List<short[]>();
		List<short[]> seriesBlockCore4 = list;
		list = new List<short[]>();
		List<short[]> encircleBlockCore4 = list;
		short[] sceneryBlockCore4 = new short[0];
		list2 = new List<short>();
		List<short> lovingItemSubTypes4 = list2;
		list2 = new List<short>();
		dataArray4.Add(new MapAreaItem(63, 5, 272, 273, 20, worldMapPos4, neighborAreas4, "largemap_part_1_changgui_2", miniMapIcons4, null, miniMapPos4, 0, 0, 5, enemyNests4, enemyNestCreationDateRanges4, settlementBlockCore4, organizationId4, -1, 0, null, developedBlockCore4, normalBlockCore4, wildBlockCore4, bigBaseBlockCore4, seriesBlockCore4, encircleBlockCore4, sceneryBlockCore4, 274, 275, null, null, EMapAreaAreaDirection.South, lovingItemSubTypes4, list2, showDarkAshStatus: true));
		List<MapAreaItem> dataArray5 = _dataArray;
		sbyte[] worldMapPos5 = new sbyte[2] { 11, 17 };
		AreaTravelRoute[] neighborAreas5 = new AreaTravelRoute[0];
		string[] miniMapIcons5 = new string[4] { "largemap_part_2_icon_hei_1", "largemap_part_2_icon_jin_1", "largemap_part_2_icon_lan_1", "largemap_part_2_icon_hui_1" };
		short[] miniMapPos5 = new short[2] { 160, 150 };
		List<EnemyNestCreationInfo[]> enemyNests5 = new List<EnemyNestCreationInfo[]>
		{
			new EnemyNestCreationInfo[2]
			{
				new EnemyNestCreationInfo(0, 2),
				new EnemyNestCreationInfo(1, 2)
			},
			new EnemyNestCreationInfo[2]
			{
				new EnemyNestCreationInfo(1, 2),
				new EnemyNestCreationInfo(2, 3)
			},
			new EnemyNestCreationInfo[4]
			{
				new EnemyNestCreationInfo(2, 3),
				new EnemyNestCreationInfo(3, 3),
				new EnemyNestCreationInfo(4, 4),
				new EnemyNestCreationInfo(5, 4)
			},
			new EnemyNestCreationInfo[1]
			{
				new EnemyNestCreationInfo(12, 2)
			}
		};
		List<IntPair> enemyNestCreationDateRanges5 = new List<IntPair>
		{
			new IntPair(0, 6),
			new IntPair(6, 12),
			new IntPair(12, 24),
			new IntPair(0, 0)
		};
		short[] settlementBlockCore5 = new short[1] { 34 };
		sbyte[] organizationId5 = new sbyte[1] { 36 };
		List<short[]> developedBlockCore5 = new List<short[]>
		{
			new short[2] { 39, 33 },
			new short[2] { 40, 33 },
			new short[2] { 41, 33 },
			new short[2] { 42, 33 },
			new short[2] { 43, 33 },
			new short[2] { 44, 33 },
			new short[2] { 45, 33 },
			new short[2] { 46, 33 },
			new short[2] { 47, 33 },
			new short[2] { 48, 33 },
			new short[2] { 49, 33 },
			new short[2] { 50, 33 },
			new short[2] { 51, 33 },
			new short[2] { 52, 33 },
			new short[2] { 53, 33 },
			new short[2] { 54, 33 },
			new short[2] { 55, 33 },
			new short[2] { 56, 33 },
			new short[2] { 109, 10 },
			new short[2] { 110, 10 },
			new short[2] { 111, 10 }
		};
		List<short[]> normalBlockCore5 = new List<short[]>
		{
			new short[2] { 57, 33 },
			new short[2] { 58, 33 },
			new short[2] { 59, 33 },
			new short[2] { 63, 33 },
			new short[2] { 64, 33 },
			new short[2] { 65, 33 },
			new short[2] { 69, 33 },
			new short[2] { 70, 33 },
			new short[2] { 71, 33 },
			new short[2] { 75, 33 },
			new short[2] { 76, 33 },
			new short[2] { 77, 33 },
			new short[2] { 81, 33 },
			new short[2] { 82, 33 },
			new short[2] { 83, 33 },
			new short[2] { 87, 33 },
			new short[2] { 88, 33 },
			new short[2] { 89, 33 },
			new short[2] { 112, 30 },
			new short[2] { 113, 30 },
			new short[2] { 114, 30 },
			new short[2] { 124, 30 }
		};
		List<short[]> wildBlockCore5 = new List<short[]>
		{
			new short[2] { 93, 33 },
			new short[2] { 94, 33 },
			new short[2] { 95, 33 },
			new short[2] { 96, 33 },
			new short[2] { 97, 33 },
			new short[2] { 98, 33 },
			new short[2] { 99, 33 },
			new short[2] { 100, 33 },
			new short[2] { 101, 33 },
			new short[2] { 102, 33 },
			new short[2] { 103, 33 },
			new short[2] { 104, 33 },
			new short[2] { 105, 33 },
			new short[2] { 106, 33 },
			new short[2] { 107, 33 },
			new short[2] { 108, 33 },
			new short[2] { 115, 40 },
			new short[2] { 116, 40 },
			new short[2] { 117, 40 },
			new short[2] { 124, 30 }
		};
		List<short[]> bigBaseBlockCore5 = new List<short[]>
		{
			new short[2] { 73, 1 },
			new short[2] { 74, 1 },
			new short[2] { 60, 1 },
			new short[2] { 84, 1 },
			new short[2] { 66, 1 }
		};
		list = new List<short[]>();
		List<short[]> seriesBlockCore5 = list;
		list = new List<short[]>();
		List<short[]> encircleBlockCore5 = list;
		short[] sceneryBlockCore5 = new short[0];
		list2 = new List<short>();
		List<short> lovingItemSubTypes5 = list2;
		list2 = new List<short>();
		dataArray5.Add(new MapAreaItem(64, 5, 276, 277, 20, worldMapPos5, neighborAreas5, "largemap_part_1_changgui_1", miniMapIcons5, null, miniMapPos5, 0, 0, 5, enemyNests5, enemyNestCreationDateRanges5, settlementBlockCore5, organizationId5, -1, 0, null, developedBlockCore5, normalBlockCore5, wildBlockCore5, bigBaseBlockCore5, seriesBlockCore5, encircleBlockCore5, sceneryBlockCore5, 278, 279, null, null, EMapAreaAreaDirection.South, lovingItemSubTypes5, list2, showDarkAshStatus: true));
		List<MapAreaItem> dataArray6 = _dataArray;
		sbyte[] worldMapPos6 = new sbyte[2] { 9, 18 };
		AreaTravelRoute[] neighborAreas6 = new AreaTravelRoute[0];
		string[] miniMapIcons6 = new string[4] { "largemap_part_2_icon_hei_0", "largemap_part_2_icon_jin_0", "largemap_part_2_icon_lan_0", "largemap_part_2_icon_hui_0" };
		short[] miniMapPos6 = new short[2] { 80, 190 };
		List<EnemyNestCreationInfo[]> enemyNests6 = new List<EnemyNestCreationInfo[]>
		{
			new EnemyNestCreationInfo[2]
			{
				new EnemyNestCreationInfo(0, 2),
				new EnemyNestCreationInfo(1, 2)
			},
			new EnemyNestCreationInfo[2]
			{
				new EnemyNestCreationInfo(1, 2),
				new EnemyNestCreationInfo(2, 3)
			},
			new EnemyNestCreationInfo[4]
			{
				new EnemyNestCreationInfo(2, 3),
				new EnemyNestCreationInfo(3, 3),
				new EnemyNestCreationInfo(4, 4),
				new EnemyNestCreationInfo(5, 4)
			},
			new EnemyNestCreationInfo[1]
			{
				new EnemyNestCreationInfo(12, 2)
			}
		};
		List<IntPair> enemyNestCreationDateRanges6 = new List<IntPair>
		{
			new IntPair(0, 6),
			new IntPair(6, 12),
			new IntPair(12, 24),
			new IntPair(0, 0)
		};
		short[] settlementBlockCore6 = new short[1] { 34 };
		sbyte[] organizationId6 = new sbyte[1] { 36 };
		List<short[]> developedBlockCore6 = new List<short[]>
		{
			new short[2] { 39, 33 },
			new short[2] { 40, 33 },
			new short[2] { 41, 33 },
			new short[2] { 42, 33 },
			new short[2] { 43, 33 },
			new short[2] { 44, 33 },
			new short[2] { 45, 33 },
			new short[2] { 46, 33 },
			new short[2] { 47, 33 },
			new short[2] { 48, 33 },
			new short[2] { 49, 33 },
			new short[2] { 50, 33 },
			new short[2] { 51, 33 },
			new short[2] { 52, 33 },
			new short[2] { 53, 33 },
			new short[2] { 54, 33 },
			new short[2] { 55, 33 },
			new short[2] { 56, 33 },
			new short[2] { 109, 10 },
			new short[2] { 110, 10 },
			new short[2] { 111, 10 }
		};
		List<short[]> normalBlockCore6 = new List<short[]>
		{
			new short[2] { 57, 33 },
			new short[2] { 58, 33 },
			new short[2] { 59, 33 },
			new short[2] { 63, 33 },
			new short[2] { 64, 33 },
			new short[2] { 65, 33 },
			new short[2] { 69, 33 },
			new short[2] { 70, 33 },
			new short[2] { 71, 33 },
			new short[2] { 75, 33 },
			new short[2] { 76, 33 },
			new short[2] { 77, 33 },
			new short[2] { 81, 33 },
			new short[2] { 82, 33 },
			new short[2] { 83, 33 },
			new short[2] { 87, 33 },
			new short[2] { 88, 33 },
			new short[2] { 89, 33 },
			new short[2] { 112, 30 },
			new short[2] { 113, 30 },
			new short[2] { 114, 30 },
			new short[2] { 124, 30 }
		};
		List<short[]> wildBlockCore6 = new List<short[]>
		{
			new short[2] { 93, 33 },
			new short[2] { 94, 33 },
			new short[2] { 95, 33 },
			new short[2] { 96, 33 },
			new short[2] { 97, 33 },
			new short[2] { 98, 33 },
			new short[2] { 99, 33 },
			new short[2] { 100, 33 },
			new short[2] { 101, 33 },
			new short[2] { 102, 33 },
			new short[2] { 103, 33 },
			new short[2] { 104, 33 },
			new short[2] { 105, 33 },
			new short[2] { 106, 33 },
			new short[2] { 107, 33 },
			new short[2] { 108, 33 },
			new short[2] { 115, 40 },
			new short[2] { 116, 40 },
			new short[2] { 117, 40 },
			new short[2] { 124, 30 }
		};
		List<short[]> bigBaseBlockCore6 = new List<short[]>
		{
			new short[2] { 73, 1 },
			new short[2] { 74, 1 },
			new short[2] { 60, 1 },
			new short[2] { 84, 1 },
			new short[2] { 66, 1 }
		};
		list = new List<short[]>();
		List<short[]> seriesBlockCore6 = list;
		list = new List<short[]>();
		List<short[]> encircleBlockCore6 = list;
		short[] sceneryBlockCore6 = new short[0];
		list2 = new List<short>();
		List<short> lovingItemSubTypes6 = list2;
		list2 = new List<short>();
		dataArray6.Add(new MapAreaItem(65, 5, 280, 281, 20, worldMapPos6, neighborAreas6, "largemap_part_1_changgui_0", miniMapIcons6, null, miniMapPos6, 0, 0, 5, enemyNests6, enemyNestCreationDateRanges6, settlementBlockCore6, organizationId6, -1, 0, null, developedBlockCore6, normalBlockCore6, wildBlockCore6, bigBaseBlockCore6, seriesBlockCore6, encircleBlockCore6, sceneryBlockCore6, 282, 283, null, null, EMapAreaAreaDirection.South, lovingItemSubTypes6, list2, showDarkAshStatus: true));
		List<MapAreaItem> dataArray7 = _dataArray;
		sbyte[] worldMapPos7 = new sbyte[2] { 15, 0 };
		AreaTravelRoute[] neighborAreas7 = new AreaTravelRoute[0];
		string[] miniMapIcons7 = new string[4] { "largemap_part_2_icon_hei_2", "largemap_part_2_icon_jin_2", "largemap_part_2_icon_lan_2", "largemap_part_2_icon_hui_2" };
		short[] miniMapPos7 = new short[2] { 180, 120 };
		List<EnemyNestCreationInfo[]> enemyNests7 = new List<EnemyNestCreationInfo[]>
		{
			new EnemyNestCreationInfo[2]
			{
				new EnemyNestCreationInfo(0, 2),
				new EnemyNestCreationInfo(1, 2)
			},
			new EnemyNestCreationInfo[2]
			{
				new EnemyNestCreationInfo(1, 2),
				new EnemyNestCreationInfo(2, 3)
			},
			new EnemyNestCreationInfo[4]
			{
				new EnemyNestCreationInfo(2, 3),
				new EnemyNestCreationInfo(3, 3),
				new EnemyNestCreationInfo(4, 4),
				new EnemyNestCreationInfo(5, 4)
			},
			new EnemyNestCreationInfo[1]
			{
				new EnemyNestCreationInfo(12, 2)
			}
		};
		List<IntPair> enemyNestCreationDateRanges7 = new List<IntPair>
		{
			new IntPair(0, 6),
			new IntPair(6, 12),
			new IntPair(12, 24),
			new IntPair(0, 0)
		};
		short[] settlementBlockCore7 = new short[1] { 35 };
		sbyte[] organizationId7 = new sbyte[1] { 37 };
		List<short[]> developedBlockCore7 = new List<short[]>
		{
			new short[2] { 39, 33 },
			new short[2] { 40, 33 },
			new short[2] { 41, 33 },
			new short[2] { 42, 33 },
			new short[2] { 43, 33 },
			new short[2] { 44, 33 },
			new short[2] { 45, 33 },
			new short[2] { 46, 33 },
			new short[2] { 47, 33 },
			new short[2] { 48, 33 },
			new short[2] { 49, 33 },
			new short[2] { 50, 33 },
			new short[2] { 51, 33 },
			new short[2] { 52, 33 },
			new short[2] { 53, 33 },
			new short[2] { 54, 33 },
			new short[2] { 55, 33 },
			new short[2] { 56, 33 },
			new short[2] { 109, 10 },
			new short[2] { 110, 10 },
			new short[2] { 111, 10 }
		};
		List<short[]> normalBlockCore7 = new List<short[]>
		{
			new short[2] { 57, 33 },
			new short[2] { 58, 33 },
			new short[2] { 59, 33 },
			new short[2] { 63, 33 },
			new short[2] { 64, 33 },
			new short[2] { 65, 33 },
			new short[2] { 69, 33 },
			new short[2] { 70, 33 },
			new short[2] { 71, 33 },
			new short[2] { 75, 33 },
			new short[2] { 76, 33 },
			new short[2] { 77, 33 },
			new short[2] { 81, 33 },
			new short[2] { 82, 33 },
			new short[2] { 83, 33 },
			new short[2] { 87, 33 },
			new short[2] { 88, 33 },
			new short[2] { 89, 33 },
			new short[2] { 112, 30 },
			new short[2] { 113, 30 },
			new short[2] { 114, 30 },
			new short[2] { 124, 30 }
		};
		List<short[]> wildBlockCore7 = new List<short[]>
		{
			new short[2] { 93, 33 },
			new short[2] { 94, 33 },
			new short[2] { 95, 33 },
			new short[2] { 96, 33 },
			new short[2] { 97, 33 },
			new short[2] { 98, 33 },
			new short[2] { 99, 33 },
			new short[2] { 100, 33 },
			new short[2] { 101, 33 },
			new short[2] { 102, 33 },
			new short[2] { 103, 33 },
			new short[2] { 104, 33 },
			new short[2] { 105, 33 },
			new short[2] { 106, 33 },
			new short[2] { 107, 33 },
			new short[2] { 108, 33 },
			new short[2] { 115, 40 },
			new short[2] { 116, 40 },
			new short[2] { 117, 40 },
			new short[2] { 124, 30 }
		};
		List<short[]> bigBaseBlockCore7 = new List<short[]>
		{
			new short[2] { 60, 1 },
			new short[2] { 78, 1 },
			new short[2] { 86, 1 },
			new short[2] { 91, 1 },
			new short[2] { 92, 1 }
		};
		list = new List<short[]>();
		List<short[]> seriesBlockCore7 = list;
		list = new List<short[]>();
		List<short[]> encircleBlockCore7 = list;
		short[] sceneryBlockCore7 = new short[0];
		list2 = new List<short>();
		List<short> lovingItemSubTypes7 = list2;
		list2 = new List<short>();
		dataArray7.Add(new MapAreaItem(66, 6, 284, 285, 20, worldMapPos7, neighborAreas7, "largemap_part_1_changgui_2", miniMapIcons7, null, miniMapPos7, 0, 0, 5, enemyNests7, enemyNestCreationDateRanges7, settlementBlockCore7, organizationId7, -1, 0, null, developedBlockCore7, normalBlockCore7, wildBlockCore7, bigBaseBlockCore7, seriesBlockCore7, encircleBlockCore7, sceneryBlockCore7, 286, 287, null, null, EMapAreaAreaDirection.South, lovingItemSubTypes7, list2, showDarkAshStatus: true));
		List<MapAreaItem> dataArray8 = _dataArray;
		sbyte[] worldMapPos8 = new sbyte[2] { 16, 1 };
		AreaTravelRoute[] neighborAreas8 = new AreaTravelRoute[1]
		{
			new AreaTravelRoute(70, 12, new int[2] { 16, 2 })
		};
		string[] miniMapIcons8 = new string[4] { "largemap_part_2_icon_hei_2", "largemap_part_2_icon_jin_2", "largemap_part_2_icon_lan_2", "largemap_part_2_icon_hui_2" };
		short[] miniMapPos8 = new short[2] { 220, 160 };
		List<EnemyNestCreationInfo[]> enemyNests8 = new List<EnemyNestCreationInfo[]>
		{
			new EnemyNestCreationInfo[2]
			{
				new EnemyNestCreationInfo(0, 2),
				new EnemyNestCreationInfo(1, 2)
			},
			new EnemyNestCreationInfo[2]
			{
				new EnemyNestCreationInfo(1, 2),
				new EnemyNestCreationInfo(2, 3)
			},
			new EnemyNestCreationInfo[4]
			{
				new EnemyNestCreationInfo(2, 3),
				new EnemyNestCreationInfo(3, 3),
				new EnemyNestCreationInfo(4, 4),
				new EnemyNestCreationInfo(5, 4)
			},
			new EnemyNestCreationInfo[1]
			{
				new EnemyNestCreationInfo(12, 2)
			}
		};
		List<IntPair> enemyNestCreationDateRanges8 = new List<IntPair>
		{
			new IntPair(0, 6),
			new IntPair(6, 12),
			new IntPair(12, 24),
			new IntPair(0, 0)
		};
		short[] settlementBlockCore8 = new short[1] { 35 };
		sbyte[] organizationId8 = new sbyte[1] { 37 };
		List<short[]> developedBlockCore8 = new List<short[]>
		{
			new short[2] { 39, 33 },
			new short[2] { 40, 33 },
			new short[2] { 41, 33 },
			new short[2] { 42, 33 },
			new short[2] { 43, 33 },
			new short[2] { 44, 33 },
			new short[2] { 45, 33 },
			new short[2] { 46, 33 },
			new short[2] { 47, 33 },
			new short[2] { 48, 33 },
			new short[2] { 49, 33 },
			new short[2] { 50, 33 },
			new short[2] { 51, 33 },
			new short[2] { 52, 33 },
			new short[2] { 53, 33 },
			new short[2] { 54, 33 },
			new short[2] { 55, 33 },
			new short[2] { 56, 33 },
			new short[2] { 109, 10 },
			new short[2] { 110, 10 },
			new short[2] { 111, 10 }
		};
		List<short[]> normalBlockCore8 = new List<short[]>
		{
			new short[2] { 57, 33 },
			new short[2] { 58, 33 },
			new short[2] { 59, 33 },
			new short[2] { 63, 33 },
			new short[2] { 64, 33 },
			new short[2] { 65, 33 },
			new short[2] { 69, 33 },
			new short[2] { 70, 33 },
			new short[2] { 71, 33 },
			new short[2] { 75, 33 },
			new short[2] { 76, 33 },
			new short[2] { 77, 33 },
			new short[2] { 81, 33 },
			new short[2] { 82, 33 },
			new short[2] { 83, 33 },
			new short[2] { 87, 33 },
			new short[2] { 88, 33 },
			new short[2] { 89, 33 },
			new short[2] { 112, 30 },
			new short[2] { 113, 30 },
			new short[2] { 114, 30 },
			new short[2] { 124, 30 }
		};
		List<short[]> wildBlockCore8 = new List<short[]>
		{
			new short[2] { 93, 33 },
			new short[2] { 94, 33 },
			new short[2] { 95, 33 },
			new short[2] { 96, 33 },
			new short[2] { 97, 33 },
			new short[2] { 98, 33 },
			new short[2] { 99, 33 },
			new short[2] { 100, 33 },
			new short[2] { 101, 33 },
			new short[2] { 102, 33 },
			new short[2] { 103, 33 },
			new short[2] { 104, 33 },
			new short[2] { 105, 33 },
			new short[2] { 106, 33 },
			new short[2] { 107, 33 },
			new short[2] { 108, 33 },
			new short[2] { 115, 40 },
			new short[2] { 116, 40 },
			new short[2] { 117, 40 },
			new short[2] { 124, 30 }
		};
		List<short[]> bigBaseBlockCore8 = new List<short[]>
		{
			new short[2] { 60, 1 },
			new short[2] { 78, 1 },
			new short[2] { 86, 1 },
			new short[2] { 91, 1 },
			new short[2] { 92, 1 }
		};
		list = new List<short[]>();
		List<short[]> seriesBlockCore8 = list;
		list = new List<short[]>();
		List<short[]> encircleBlockCore8 = list;
		short[] sceneryBlockCore8 = new short[0];
		list2 = new List<short>();
		List<short> lovingItemSubTypes8 = list2;
		list2 = new List<short>();
		dataArray8.Add(new MapAreaItem(67, 6, 288, 289, 20, worldMapPos8, neighborAreas8, "largemap_part_1_changgui_2", miniMapIcons8, null, miniMapPos8, 0, 0, 5, enemyNests8, enemyNestCreationDateRanges8, settlementBlockCore8, organizationId8, -1, 0, null, developedBlockCore8, normalBlockCore8, wildBlockCore8, bigBaseBlockCore8, seriesBlockCore8, encircleBlockCore8, sceneryBlockCore8, 290, 291, null, null, EMapAreaAreaDirection.South, lovingItemSubTypes8, list2, showDarkAshStatus: true));
		List<MapAreaItem> dataArray9 = _dataArray;
		sbyte[] worldMapPos9 = new sbyte[2] { 15, 2 };
		AreaTravelRoute[] neighborAreas9 = new AreaTravelRoute[1]
		{
			new AreaTravelRoute(72, 6, null)
		};
		string[] miniMapIcons9 = new string[4] { "largemap_part_2_icon_hei_0", "largemap_part_2_icon_jin_0", "largemap_part_2_icon_lan_0", "largemap_part_2_icon_hui_0" };
		short[] miniMapPos9 = new short[2] { 180, 200 };
		List<EnemyNestCreationInfo[]> enemyNests9 = new List<EnemyNestCreationInfo[]>
		{
			new EnemyNestCreationInfo[2]
			{
				new EnemyNestCreationInfo(0, 2),
				new EnemyNestCreationInfo(1, 2)
			},
			new EnemyNestCreationInfo[2]
			{
				new EnemyNestCreationInfo(1, 2),
				new EnemyNestCreationInfo(2, 3)
			},
			new EnemyNestCreationInfo[4]
			{
				new EnemyNestCreationInfo(2, 3),
				new EnemyNestCreationInfo(3, 3),
				new EnemyNestCreationInfo(4, 4),
				new EnemyNestCreationInfo(5, 4)
			},
			new EnemyNestCreationInfo[1]
			{
				new EnemyNestCreationInfo(12, 2)
			}
		};
		List<IntPair> enemyNestCreationDateRanges9 = new List<IntPair>
		{
			new IntPair(0, 6),
			new IntPair(6, 12),
			new IntPair(12, 24),
			new IntPair(0, 0)
		};
		short[] settlementBlockCore9 = new short[1] { 34 };
		sbyte[] organizationId9 = new sbyte[1] { 36 };
		List<short[]> developedBlockCore9 = new List<short[]>
		{
			new short[2] { 39, 33 },
			new short[2] { 40, 33 },
			new short[2] { 41, 33 },
			new short[2] { 42, 33 },
			new short[2] { 43, 33 },
			new short[2] { 44, 33 },
			new short[2] { 45, 33 },
			new short[2] { 46, 33 },
			new short[2] { 47, 33 },
			new short[2] { 48, 33 },
			new short[2] { 49, 33 },
			new short[2] { 50, 33 },
			new short[2] { 51, 33 },
			new short[2] { 52, 33 },
			new short[2] { 53, 33 },
			new short[2] { 54, 33 },
			new short[2] { 55, 33 },
			new short[2] { 56, 33 },
			new short[2] { 109, 10 },
			new short[2] { 110, 10 },
			new short[2] { 111, 10 }
		};
		List<short[]> normalBlockCore9 = new List<short[]>
		{
			new short[2] { 57, 33 },
			new short[2] { 58, 33 },
			new short[2] { 59, 33 },
			new short[2] { 63, 33 },
			new short[2] { 64, 33 },
			new short[2] { 65, 33 },
			new short[2] { 69, 33 },
			new short[2] { 70, 33 },
			new short[2] { 71, 33 },
			new short[2] { 75, 33 },
			new short[2] { 76, 33 },
			new short[2] { 77, 33 },
			new short[2] { 81, 33 },
			new short[2] { 82, 33 },
			new short[2] { 83, 33 },
			new short[2] { 87, 33 },
			new short[2] { 88, 33 },
			new short[2] { 89, 33 },
			new short[2] { 112, 30 },
			new short[2] { 113, 30 },
			new short[2] { 114, 30 },
			new short[2] { 124, 30 }
		};
		List<short[]> wildBlockCore9 = new List<short[]>
		{
			new short[2] { 93, 33 },
			new short[2] { 94, 33 },
			new short[2] { 95, 33 },
			new short[2] { 96, 33 },
			new short[2] { 97, 33 },
			new short[2] { 98, 33 },
			new short[2] { 99, 33 },
			new short[2] { 100, 33 },
			new short[2] { 101, 33 },
			new short[2] { 102, 33 },
			new short[2] { 103, 33 },
			new short[2] { 104, 33 },
			new short[2] { 105, 33 },
			new short[2] { 106, 33 },
			new short[2] { 107, 33 },
			new short[2] { 108, 33 },
			new short[2] { 115, 40 },
			new short[2] { 116, 40 },
			new short[2] { 117, 40 },
			new short[2] { 124, 30 }
		};
		List<short[]> bigBaseBlockCore9 = new List<short[]>
		{
			new short[2] { 60, 1 },
			new short[2] { 78, 1 },
			new short[2] { 86, 1 },
			new short[2] { 91, 1 },
			new short[2] { 92, 1 }
		};
		list = new List<short[]>();
		List<short[]> seriesBlockCore9 = list;
		list = new List<short[]>();
		List<short[]> encircleBlockCore9 = list;
		short[] sceneryBlockCore9 = new short[0];
		list2 = new List<short>();
		List<short> lovingItemSubTypes9 = list2;
		list2 = new List<short>();
		dataArray9.Add(new MapAreaItem(68, 6, 292, 293, 20, worldMapPos9, neighborAreas9, "largemap_part_1_changgui_0", miniMapIcons9, null, miniMapPos9, 0, 0, 5, enemyNests9, enemyNestCreationDateRanges9, settlementBlockCore9, organizationId9, -1, 0, null, developedBlockCore9, normalBlockCore9, wildBlockCore9, bigBaseBlockCore9, seriesBlockCore9, encircleBlockCore9, sceneryBlockCore9, 294, 295, null, null, EMapAreaAreaDirection.South, lovingItemSubTypes9, list2, showDarkAshStatus: true));
		List<MapAreaItem> dataArray10 = _dataArray;
		sbyte[] worldMapPos10 = new sbyte[2] { 17, 3 };
		AreaTravelRoute[] neighborAreas10 = new AreaTravelRoute[4]
		{
			new AreaTravelRoute(70, 6, null),
			new AreaTravelRoute(82, 52, new int[10] { 17, 4, 17, 5, 17, 6, 16, 6, 15, 6 }),
			new AreaTravelRoute(88, 12, new int[2] { 18, 3 }),
			new AreaTravelRoute(90, 42, new int[8] { 17, 4, 17, 5, 17, 6, 18, 6 })
		};
		string[] miniMapIcons10 = new string[4] { "largemap_part_2_icon_hei_2", "largemap_part_2_icon_jin_2", "largemap_part_2_icon_lan_2", "largemap_part_2_icon_hui_2" };
		short[] miniMapPos10 = new short[2] { 260, 240 };
		List<EnemyNestCreationInfo[]> enemyNests10 = new List<EnemyNestCreationInfo[]>
		{
			new EnemyNestCreationInfo[2]
			{
				new EnemyNestCreationInfo(0, 2),
				new EnemyNestCreationInfo(1, 2)
			},
			new EnemyNestCreationInfo[2]
			{
				new EnemyNestCreationInfo(1, 2),
				new EnemyNestCreationInfo(2, 3)
			},
			new EnemyNestCreationInfo[4]
			{
				new EnemyNestCreationInfo(2, 3),
				new EnemyNestCreationInfo(3, 3),
				new EnemyNestCreationInfo(4, 4),
				new EnemyNestCreationInfo(5, 4)
			},
			new EnemyNestCreationInfo[1]
			{
				new EnemyNestCreationInfo(12, 2)
			}
		};
		List<IntPair> enemyNestCreationDateRanges10 = new List<IntPair>
		{
			new IntPair(0, 6),
			new IntPair(6, 12),
			new IntPair(12, 24),
			new IntPair(0, 0)
		};
		short[] settlementBlockCore10 = new short[1] { 35 };
		sbyte[] organizationId10 = new sbyte[1] { 37 };
		List<short[]> developedBlockCore10 = new List<short[]>
		{
			new short[2] { 39, 33 },
			new short[2] { 40, 33 },
			new short[2] { 41, 33 },
			new short[2] { 42, 33 },
			new short[2] { 43, 33 },
			new short[2] { 44, 33 },
			new short[2] { 45, 33 },
			new short[2] { 46, 33 },
			new short[2] { 47, 33 },
			new short[2] { 48, 33 },
			new short[2] { 49, 33 },
			new short[2] { 50, 33 },
			new short[2] { 51, 33 },
			new short[2] { 52, 33 },
			new short[2] { 53, 33 },
			new short[2] { 54, 33 },
			new short[2] { 55, 33 },
			new short[2] { 56, 33 },
			new short[2] { 109, 10 },
			new short[2] { 110, 10 },
			new short[2] { 111, 10 }
		};
		List<short[]> normalBlockCore10 = new List<short[]>
		{
			new short[2] { 57, 33 },
			new short[2] { 58, 33 },
			new short[2] { 59, 33 },
			new short[2] { 63, 33 },
			new short[2] { 64, 33 },
			new short[2] { 65, 33 },
			new short[2] { 69, 33 },
			new short[2] { 70, 33 },
			new short[2] { 71, 33 },
			new short[2] { 75, 33 },
			new short[2] { 76, 33 },
			new short[2] { 77, 33 },
			new short[2] { 81, 33 },
			new short[2] { 82, 33 },
			new short[2] { 83, 33 },
			new short[2] { 87, 33 },
			new short[2] { 88, 33 },
			new short[2] { 89, 33 },
			new short[2] { 112, 30 },
			new short[2] { 113, 30 },
			new short[2] { 114, 30 },
			new short[2] { 124, 30 }
		};
		List<short[]> wildBlockCore10 = new List<short[]>
		{
			new short[2] { 93, 33 },
			new short[2] { 94, 33 },
			new short[2] { 95, 33 },
			new short[2] { 96, 33 },
			new short[2] { 97, 33 },
			new short[2] { 98, 33 },
			new short[2] { 99, 33 },
			new short[2] { 100, 33 },
			new short[2] { 101, 33 },
			new short[2] { 102, 33 },
			new short[2] { 103, 33 },
			new short[2] { 104, 33 },
			new short[2] { 105, 33 },
			new short[2] { 106, 33 },
			new short[2] { 107, 33 },
			new short[2] { 108, 33 },
			new short[2] { 115, 40 },
			new short[2] { 116, 40 },
			new short[2] { 117, 40 },
			new short[2] { 124, 30 }
		};
		List<short[]> bigBaseBlockCore10 = new List<short[]>
		{
			new short[2] { 60, 1 },
			new short[2] { 78, 1 },
			new short[2] { 86, 1 },
			new short[2] { 91, 1 },
			new short[2] { 92, 1 }
		};
		list = new List<short[]>();
		List<short[]> seriesBlockCore10 = list;
		list = new List<short[]>();
		List<short[]> encircleBlockCore10 = list;
		short[] sceneryBlockCore10 = new short[0];
		list2 = new List<short>();
		List<short> lovingItemSubTypes10 = list2;
		list2 = new List<short>();
		dataArray10.Add(new MapAreaItem(69, 6, 296, 297, 20, worldMapPos10, neighborAreas10, "largemap_part_1_changgui_2", miniMapIcons10, null, miniMapPos10, 0, 0, 5, enemyNests10, enemyNestCreationDateRanges10, settlementBlockCore10, organizationId10, -1, 0, null, developedBlockCore10, normalBlockCore10, wildBlockCore10, bigBaseBlockCore10, seriesBlockCore10, encircleBlockCore10, sceneryBlockCore10, 298, 299, null, null, EMapAreaAreaDirection.South, lovingItemSubTypes10, list2, showDarkAshStatus: true));
		List<MapAreaItem> dataArray11 = _dataArray;
		sbyte[] worldMapPos11 = new sbyte[2] { 17, 2 };
		AreaTravelRoute[] neighborAreas11 = new AreaTravelRoute[0];
		string[] miniMapIcons11 = new string[4] { "largemap_part_2_icon_hei_2", "largemap_part_2_icon_jin_2", "largemap_part_2_icon_lan_2", "largemap_part_2_icon_hui_2" };
		short[] miniMapPos11 = new short[2] { 260, 200 };
		List<EnemyNestCreationInfo[]> enemyNests11 = new List<EnemyNestCreationInfo[]>
		{
			new EnemyNestCreationInfo[2]
			{
				new EnemyNestCreationInfo(0, 2),
				new EnemyNestCreationInfo(1, 2)
			},
			new EnemyNestCreationInfo[2]
			{
				new EnemyNestCreationInfo(1, 2),
				new EnemyNestCreationInfo(2, 3)
			},
			new EnemyNestCreationInfo[4]
			{
				new EnemyNestCreationInfo(2, 3),
				new EnemyNestCreationInfo(3, 3),
				new EnemyNestCreationInfo(4, 4),
				new EnemyNestCreationInfo(5, 4)
			},
			new EnemyNestCreationInfo[1]
			{
				new EnemyNestCreationInfo(12, 2)
			}
		};
		List<IntPair> enemyNestCreationDateRanges11 = new List<IntPair>
		{
			new IntPair(0, 6),
			new IntPair(6, 12),
			new IntPair(12, 24),
			new IntPair(0, 0)
		};
		short[] settlementBlockCore11 = new short[1] { 34 };
		sbyte[] organizationId11 = new sbyte[1] { 36 };
		List<short[]> developedBlockCore11 = new List<short[]>
		{
			new short[2] { 39, 33 },
			new short[2] { 40, 33 },
			new short[2] { 41, 33 },
			new short[2] { 42, 33 },
			new short[2] { 43, 33 },
			new short[2] { 44, 33 },
			new short[2] { 45, 33 },
			new short[2] { 46, 33 },
			new short[2] { 47, 33 },
			new short[2] { 48, 33 },
			new short[2] { 49, 33 },
			new short[2] { 50, 33 },
			new short[2] { 51, 33 },
			new short[2] { 52, 33 },
			new short[2] { 53, 33 },
			new short[2] { 54, 33 },
			new short[2] { 55, 33 },
			new short[2] { 56, 33 },
			new short[2] { 109, 10 },
			new short[2] { 110, 10 },
			new short[2] { 111, 10 }
		};
		List<short[]> normalBlockCore11 = new List<short[]>
		{
			new short[2] { 57, 33 },
			new short[2] { 58, 33 },
			new short[2] { 59, 33 },
			new short[2] { 63, 33 },
			new short[2] { 64, 33 },
			new short[2] { 65, 33 },
			new short[2] { 69, 33 },
			new short[2] { 70, 33 },
			new short[2] { 71, 33 },
			new short[2] { 75, 33 },
			new short[2] { 76, 33 },
			new short[2] { 77, 33 },
			new short[2] { 81, 33 },
			new short[2] { 82, 33 },
			new short[2] { 83, 33 },
			new short[2] { 87, 33 },
			new short[2] { 88, 33 },
			new short[2] { 89, 33 },
			new short[2] { 112, 30 },
			new short[2] { 113, 30 },
			new short[2] { 114, 30 },
			new short[2] { 124, 30 }
		};
		List<short[]> wildBlockCore11 = new List<short[]>
		{
			new short[2] { 93, 33 },
			new short[2] { 94, 33 },
			new short[2] { 95, 33 },
			new short[2] { 96, 33 },
			new short[2] { 97, 33 },
			new short[2] { 98, 33 },
			new short[2] { 99, 33 },
			new short[2] { 100, 33 },
			new short[2] { 101, 33 },
			new short[2] { 102, 33 },
			new short[2] { 103, 33 },
			new short[2] { 104, 33 },
			new short[2] { 105, 33 },
			new short[2] { 106, 33 },
			new short[2] { 107, 33 },
			new short[2] { 108, 33 },
			new short[2] { 115, 40 },
			new short[2] { 116, 40 },
			new short[2] { 117, 40 },
			new short[2] { 124, 30 }
		};
		List<short[]> bigBaseBlockCore11 = new List<short[]>
		{
			new short[2] { 60, 1 },
			new short[2] { 78, 1 },
			new short[2] { 86, 1 },
			new short[2] { 91, 1 },
			new short[2] { 92, 1 }
		};
		list = new List<short[]>();
		List<short[]> seriesBlockCore11 = list;
		list = new List<short[]>();
		List<short[]> encircleBlockCore11 = list;
		short[] sceneryBlockCore11 = new short[0];
		list2 = new List<short>();
		List<short> lovingItemSubTypes11 = list2;
		list2 = new List<short>();
		dataArray11.Add(new MapAreaItem(70, 6, 300, 301, 20, worldMapPos11, neighborAreas11, "largemap_part_1_changgui_2", miniMapIcons11, null, miniMapPos11, 0, 0, 5, enemyNests11, enemyNestCreationDateRanges11, settlementBlockCore11, organizationId11, -1, 0, null, developedBlockCore11, normalBlockCore11, wildBlockCore11, bigBaseBlockCore11, seriesBlockCore11, encircleBlockCore11, sceneryBlockCore11, 302, 303, null, null, EMapAreaAreaDirection.South, lovingItemSubTypes11, list2, showDarkAshStatus: true));
		List<MapAreaItem> dataArray12 = _dataArray;
		sbyte[] worldMapPos12 = new sbyte[2] { 14, 3 };
		AreaTravelRoute[] neighborAreas12 = new AreaTravelRoute[2]
		{
			new AreaTravelRoute(72, 6, null),
			new AreaTravelRoute(85, 24, new int[4] { 14, 4, 13, 4 })
		};
		string[] miniMapIcons12 = new string[4] { "largemap_part_2_icon_hei_0", "largemap_part_2_icon_jin_0", "largemap_part_2_icon_lan_0", "largemap_part_2_icon_hui_0" };
		short[] miniMapPos12 = new short[2] { 140, 240 };
		List<EnemyNestCreationInfo[]> enemyNests12 = new List<EnemyNestCreationInfo[]>
		{
			new EnemyNestCreationInfo[2]
			{
				new EnemyNestCreationInfo(0, 2),
				new EnemyNestCreationInfo(1, 2)
			},
			new EnemyNestCreationInfo[2]
			{
				new EnemyNestCreationInfo(1, 2),
				new EnemyNestCreationInfo(2, 3)
			},
			new EnemyNestCreationInfo[4]
			{
				new EnemyNestCreationInfo(2, 3),
				new EnemyNestCreationInfo(3, 3),
				new EnemyNestCreationInfo(4, 4),
				new EnemyNestCreationInfo(5, 4)
			},
			new EnemyNestCreationInfo[1]
			{
				new EnemyNestCreationInfo(12, 2)
			}
		};
		List<IntPair> enemyNestCreationDateRanges12 = new List<IntPair>
		{
			new IntPair(0, 6),
			new IntPair(6, 12),
			new IntPair(12, 24),
			new IntPair(0, 0)
		};
		short[] settlementBlockCore12 = new short[1] { 36 };
		sbyte[] organizationId12 = new sbyte[1] { 38 };
		List<short[]> developedBlockCore12 = new List<short[]>
		{
			new short[2] { 39, 33 },
			new short[2] { 40, 33 },
			new short[2] { 41, 33 },
			new short[2] { 42, 33 },
			new short[2] { 43, 33 },
			new short[2] { 44, 33 },
			new short[2] { 45, 33 },
			new short[2] { 46, 33 },
			new short[2] { 47, 33 },
			new short[2] { 48, 33 },
			new short[2] { 49, 33 },
			new short[2] { 50, 33 },
			new short[2] { 51, 33 },
			new short[2] { 52, 33 },
			new short[2] { 53, 33 },
			new short[2] { 54, 33 },
			new short[2] { 55, 33 },
			new short[2] { 56, 33 },
			new short[2] { 109, 10 },
			new short[2] { 110, 10 },
			new short[2] { 111, 10 }
		};
		List<short[]> normalBlockCore12 = new List<short[]>
		{
			new short[2] { 57, 33 },
			new short[2] { 58, 33 },
			new short[2] { 59, 33 },
			new short[2] { 63, 33 },
			new short[2] { 64, 33 },
			new short[2] { 65, 33 },
			new short[2] { 69, 33 },
			new short[2] { 70, 33 },
			new short[2] { 71, 33 },
			new short[2] { 75, 33 },
			new short[2] { 76, 33 },
			new short[2] { 77, 33 },
			new short[2] { 81, 33 },
			new short[2] { 82, 33 },
			new short[2] { 83, 33 },
			new short[2] { 87, 33 },
			new short[2] { 88, 33 },
			new short[2] { 89, 33 },
			new short[2] { 112, 30 },
			new short[2] { 113, 30 },
			new short[2] { 114, 30 },
			new short[2] { 124, 30 }
		};
		List<short[]> wildBlockCore12 = new List<short[]>
		{
			new short[2] { 93, 33 },
			new short[2] { 94, 33 },
			new short[2] { 95, 33 },
			new short[2] { 96, 33 },
			new short[2] { 97, 33 },
			new short[2] { 98, 33 },
			new short[2] { 99, 33 },
			new short[2] { 100, 33 },
			new short[2] { 101, 33 },
			new short[2] { 102, 33 },
			new short[2] { 103, 33 },
			new short[2] { 104, 33 },
			new short[2] { 105, 33 },
			new short[2] { 106, 33 },
			new short[2] { 107, 33 },
			new short[2] { 108, 33 },
			new short[2] { 115, 40 },
			new short[2] { 116, 40 },
			new short[2] { 117, 40 },
			new short[2] { 124, 30 }
		};
		List<short[]> bigBaseBlockCore12 = new List<short[]>
		{
			new short[2] { 60, 1 },
			new short[2] { 78, 1 },
			new short[2] { 86, 1 },
			new short[2] { 91, 1 },
			new short[2] { 92, 1 }
		};
		list = new List<short[]>();
		List<short[]> seriesBlockCore12 = list;
		list = new List<short[]>();
		List<short[]> encircleBlockCore12 = list;
		short[] sceneryBlockCore12 = new short[0];
		list2 = new List<short>();
		List<short> lovingItemSubTypes12 = list2;
		list2 = new List<short>();
		dataArray12.Add(new MapAreaItem(71, 6, 304, 305, 20, worldMapPos12, neighborAreas12, "largemap_part_1_changgui_0", miniMapIcons12, null, miniMapPos12, 0, 0, 5, enemyNests12, enemyNestCreationDateRanges12, settlementBlockCore12, organizationId12, -1, 0, null, developedBlockCore12, normalBlockCore12, wildBlockCore12, bigBaseBlockCore12, seriesBlockCore12, encircleBlockCore12, sceneryBlockCore12, 306, 307, null, null, EMapAreaAreaDirection.South, lovingItemSubTypes12, list2, showDarkAshStatus: true));
		List<MapAreaItem> dataArray13 = _dataArray;
		sbyte[] worldMapPos13 = new sbyte[2] { 14, 2 };
		AreaTravelRoute[] neighborAreas13 = new AreaTravelRoute[0];
		string[] miniMapIcons13 = new string[4] { "largemap_part_2_icon_hei_0", "largemap_part_2_icon_jin_0", "largemap_part_2_icon_lan_0", "largemap_part_2_icon_hui_0" };
		short[] miniMapPos13 = new short[2] { 140, 200 };
		List<EnemyNestCreationInfo[]> enemyNests13 = new List<EnemyNestCreationInfo[]>
		{
			new EnemyNestCreationInfo[2]
			{
				new EnemyNestCreationInfo(0, 2),
				new EnemyNestCreationInfo(1, 2)
			},
			new EnemyNestCreationInfo[2]
			{
				new EnemyNestCreationInfo(1, 2),
				new EnemyNestCreationInfo(2, 3)
			},
			new EnemyNestCreationInfo[4]
			{
				new EnemyNestCreationInfo(2, 3),
				new EnemyNestCreationInfo(3, 3),
				new EnemyNestCreationInfo(4, 4),
				new EnemyNestCreationInfo(5, 4)
			},
			new EnemyNestCreationInfo[1]
			{
				new EnemyNestCreationInfo(12, 2)
			}
		};
		List<IntPair> enemyNestCreationDateRanges13 = new List<IntPair>
		{
			new IntPair(0, 6),
			new IntPair(6, 12),
			new IntPair(12, 24),
			new IntPair(0, 0)
		};
		short[] settlementBlockCore13 = new short[1] { 34 };
		sbyte[] organizationId13 = new sbyte[1] { 36 };
		List<short[]> developedBlockCore13 = new List<short[]>
		{
			new short[2] { 39, 33 },
			new short[2] { 40, 33 },
			new short[2] { 41, 33 },
			new short[2] { 42, 33 },
			new short[2] { 43, 33 },
			new short[2] { 44, 33 },
			new short[2] { 45, 33 },
			new short[2] { 46, 33 },
			new short[2] { 47, 33 },
			new short[2] { 48, 33 },
			new short[2] { 49, 33 },
			new short[2] { 50, 33 },
			new short[2] { 51, 33 },
			new short[2] { 52, 33 },
			new short[2] { 53, 33 },
			new short[2] { 54, 33 },
			new short[2] { 55, 33 },
			new short[2] { 56, 33 },
			new short[2] { 109, 10 },
			new short[2] { 110, 10 },
			new short[2] { 111, 10 }
		};
		List<short[]> normalBlockCore13 = new List<short[]>
		{
			new short[2] { 57, 33 },
			new short[2] { 58, 33 },
			new short[2] { 59, 33 },
			new short[2] { 63, 33 },
			new short[2] { 64, 33 },
			new short[2] { 65, 33 },
			new short[2] { 69, 33 },
			new short[2] { 70, 33 },
			new short[2] { 71, 33 },
			new short[2] { 75, 33 },
			new short[2] { 76, 33 },
			new short[2] { 77, 33 },
			new short[2] { 81, 33 },
			new short[2] { 82, 33 },
			new short[2] { 83, 33 },
			new short[2] { 87, 33 },
			new short[2] { 88, 33 },
			new short[2] { 89, 33 },
			new short[2] { 112, 30 },
			new short[2] { 113, 30 },
			new short[2] { 114, 30 },
			new short[2] { 124, 30 }
		};
		List<short[]> wildBlockCore13 = new List<short[]>
		{
			new short[2] { 93, 33 },
			new short[2] { 94, 33 },
			new short[2] { 95, 33 },
			new short[2] { 96, 33 },
			new short[2] { 97, 33 },
			new short[2] { 98, 33 },
			new short[2] { 99, 33 },
			new short[2] { 100, 33 },
			new short[2] { 101, 33 },
			new short[2] { 102, 33 },
			new short[2] { 103, 33 },
			new short[2] { 104, 33 },
			new short[2] { 105, 33 },
			new short[2] { 106, 33 },
			new short[2] { 107, 33 },
			new short[2] { 108, 33 },
			new short[2] { 115, 40 },
			new short[2] { 116, 40 },
			new short[2] { 117, 40 },
			new short[2] { 124, 30 }
		};
		List<short[]> bigBaseBlockCore13 = new List<short[]>
		{
			new short[2] { 60, 1 },
			new short[2] { 78, 1 },
			new short[2] { 86, 1 },
			new short[2] { 91, 1 },
			new short[2] { 92, 1 }
		};
		list = new List<short[]>();
		List<short[]> seriesBlockCore13 = list;
		list = new List<short[]>();
		List<short[]> encircleBlockCore13 = list;
		short[] sceneryBlockCore13 = new short[0];
		list2 = new List<short>();
		List<short> lovingItemSubTypes13 = list2;
		list2 = new List<short>();
		dataArray13.Add(new MapAreaItem(72, 6, 308, 309, 20, worldMapPos13, neighborAreas13, "largemap_part_1_changgui_0", miniMapIcons13, null, miniMapPos13, 0, 0, 5, enemyNests13, enemyNestCreationDateRanges13, settlementBlockCore13, organizationId13, -1, 0, null, developedBlockCore13, normalBlockCore13, wildBlockCore13, bigBaseBlockCore13, seriesBlockCore13, encircleBlockCore13, sceneryBlockCore13, 310, 311, null, null, EMapAreaAreaDirection.South, lovingItemSubTypes13, list2, showDarkAshStatus: true));
		List<MapAreaItem> dataArray14 = _dataArray;
		sbyte[] worldMapPos14 = new sbyte[2] { 18, 20 };
		AreaTravelRoute[] neighborAreas14 = new AreaTravelRoute[2]
		{
			new AreaTravelRoute(75, 12, new int[2] { 17, 20 }),
			new AreaTravelRoute(76, 6, null)
		};
		string[] miniMapIcons14 = new string[4] { "largemap_part_2_icon_hei_1", "largemap_part_2_icon_jin_1", "largemap_part_2_icon_lan_1", "largemap_part_2_icon_hui_1" };
		short[] miniMapPos14 = new short[2] { 170, 240 };
		List<EnemyNestCreationInfo[]> enemyNests14 = new List<EnemyNestCreationInfo[]>
		{
			new EnemyNestCreationInfo[2]
			{
				new EnemyNestCreationInfo(0, 2),
				new EnemyNestCreationInfo(1, 2)
			},
			new EnemyNestCreationInfo[2]
			{
				new EnemyNestCreationInfo(1, 2),
				new EnemyNestCreationInfo(2, 3)
			},
			new EnemyNestCreationInfo[4]
			{
				new EnemyNestCreationInfo(2, 3),
				new EnemyNestCreationInfo(3, 3),
				new EnemyNestCreationInfo(4, 4),
				new EnemyNestCreationInfo(5, 4)
			},
			new EnemyNestCreationInfo[1]
			{
				new EnemyNestCreationInfo(12, 2)
			}
		};
		List<IntPair> enemyNestCreationDateRanges14 = new List<IntPair>
		{
			new IntPair(0, 6),
			new IntPair(6, 12),
			new IntPair(12, 24),
			new IntPair(0, 0)
		};
		short[] settlementBlockCore14 = new short[1] { 35 };
		sbyte[] organizationId14 = new sbyte[1] { 37 };
		List<short[]> developedBlockCore14 = new List<short[]>
		{
			new short[2] { 39, 33 },
			new short[2] { 40, 33 },
			new short[2] { 41, 33 },
			new short[2] { 42, 33 },
			new short[2] { 43, 33 },
			new short[2] { 44, 33 },
			new short[2] { 45, 33 },
			new short[2] { 46, 33 },
			new short[2] { 47, 33 },
			new short[2] { 48, 33 },
			new short[2] { 49, 33 },
			new short[2] { 50, 33 },
			new short[2] { 51, 33 },
			new short[2] { 52, 33 },
			new short[2] { 53, 33 },
			new short[2] { 54, 33 },
			new short[2] { 55, 33 },
			new short[2] { 56, 33 },
			new short[2] { 109, 10 },
			new short[2] { 110, 10 },
			new short[2] { 111, 10 }
		};
		List<short[]> normalBlockCore14 = new List<short[]>
		{
			new short[2] { 57, 33 },
			new short[2] { 58, 33 },
			new short[2] { 59, 33 },
			new short[2] { 63, 33 },
			new short[2] { 64, 33 },
			new short[2] { 65, 33 },
			new short[2] { 69, 33 },
			new short[2] { 70, 33 },
			new short[2] { 71, 33 },
			new short[2] { 75, 33 },
			new short[2] { 76, 33 },
			new short[2] { 77, 33 },
			new short[2] { 81, 33 },
			new short[2] { 82, 33 },
			new short[2] { 83, 33 },
			new short[2] { 87, 33 },
			new short[2] { 88, 33 },
			new short[2] { 89, 33 },
			new short[2] { 112, 30 },
			new short[2] { 113, 30 },
			new short[2] { 114, 30 },
			new short[2] { 124, 30 }
		};
		List<short[]> wildBlockCore14 = new List<short[]>
		{
			new short[2] { 93, 33 },
			new short[2] { 94, 33 },
			new short[2] { 95, 33 },
			new short[2] { 96, 33 },
			new short[2] { 97, 33 },
			new short[2] { 98, 33 },
			new short[2] { 99, 33 },
			new short[2] { 100, 33 },
			new short[2] { 101, 33 },
			new short[2] { 102, 33 },
			new short[2] { 103, 33 },
			new short[2] { 104, 33 },
			new short[2] { 105, 33 },
			new short[2] { 106, 33 },
			new short[2] { 107, 33 },
			new short[2] { 108, 33 },
			new short[2] { 115, 40 },
			new short[2] { 116, 40 },
			new short[2] { 117, 40 },
			new short[2] { 124, 30 }
		};
		List<short[]> bigBaseBlockCore14 = new List<short[]>
		{
			new short[2] { 73, 1 },
			new short[2] { 74, 1 },
			new short[2] { 78, 1 },
			new short[2] { 84, 1 },
			new short[2] { 90, 1 }
		};
		list = new List<short[]>();
		List<short[]> seriesBlockCore14 = list;
		list = new List<short[]>();
		List<short[]> encircleBlockCore14 = list;
		short[] sceneryBlockCore14 = new short[0];
		list2 = new List<short>();
		List<short> lovingItemSubTypes14 = list2;
		list2 = new List<short>();
		dataArray14.Add(new MapAreaItem(73, 7, 312, 313, 20, worldMapPos14, neighborAreas14, "largemap_part_1_changgui_1", miniMapIcons14, null, miniMapPos14, 0, 0, 5, enemyNests14, enemyNestCreationDateRanges14, settlementBlockCore14, organizationId14, -1, 0, null, developedBlockCore14, normalBlockCore14, wildBlockCore14, bigBaseBlockCore14, seriesBlockCore14, encircleBlockCore14, sceneryBlockCore14, 314, 315, null, null, EMapAreaAreaDirection.South, lovingItemSubTypes14, list2, showDarkAshStatus: true));
		List<MapAreaItem> dataArray15 = _dataArray;
		sbyte[] worldMapPos15 = new sbyte[2] { 18, 18 };
		AreaTravelRoute[] neighborAreas15 = new AreaTravelRoute[3]
		{
			new AreaTravelRoute(76, 6, null),
			new AreaTravelRoute(78, 14, new int[2] { 18, 17 }),
			new AreaTravelRoute(132, 24, new int[4] { 18, 17, 18, 16 })
		};
		string[] miniMapIcons15 = new string[4] { "largemap_part_2_icon_hei_0", "largemap_part_2_icon_jin_0", "largemap_part_2_icon_lan_0", "largemap_part_2_icon_hui_0" };
		short[] miniMapPos15 = new short[2] { 170, 160 };
		List<EnemyNestCreationInfo[]> enemyNests15 = new List<EnemyNestCreationInfo[]>
		{
			new EnemyNestCreationInfo[2]
			{
				new EnemyNestCreationInfo(0, 2),
				new EnemyNestCreationInfo(1, 2)
			},
			new EnemyNestCreationInfo[2]
			{
				new EnemyNestCreationInfo(1, 2),
				new EnemyNestCreationInfo(2, 3)
			},
			new EnemyNestCreationInfo[4]
			{
				new EnemyNestCreationInfo(2, 3),
				new EnemyNestCreationInfo(3, 3),
				new EnemyNestCreationInfo(4, 4),
				new EnemyNestCreationInfo(5, 4)
			},
			new EnemyNestCreationInfo[1]
			{
				new EnemyNestCreationInfo(12, 2)
			}
		};
		List<IntPair> enemyNestCreationDateRanges15 = new List<IntPair>
		{
			new IntPair(0, 6),
			new IntPair(6, 12),
			new IntPair(12, 24),
			new IntPair(0, 0)
		};
		short[] settlementBlockCore15 = new short[1] { 36 };
		sbyte[] organizationId15 = new sbyte[1] { 38 };
		List<short[]> developedBlockCore15 = new List<short[]>
		{
			new short[2] { 39, 33 },
			new short[2] { 40, 33 },
			new short[2] { 41, 33 },
			new short[2] { 42, 33 },
			new short[2] { 43, 33 },
			new short[2] { 44, 33 },
			new short[2] { 45, 33 },
			new short[2] { 46, 33 },
			new short[2] { 47, 33 },
			new short[2] { 48, 33 },
			new short[2] { 49, 33 },
			new short[2] { 50, 33 },
			new short[2] { 51, 33 },
			new short[2] { 52, 33 },
			new short[2] { 53, 33 },
			new short[2] { 54, 33 },
			new short[2] { 55, 33 },
			new short[2] { 56, 33 },
			new short[2] { 109, 10 },
			new short[2] { 110, 10 },
			new short[2] { 111, 10 }
		};
		List<short[]> normalBlockCore15 = new List<short[]>
		{
			new short[2] { 57, 33 },
			new short[2] { 58, 33 },
			new short[2] { 59, 33 },
			new short[2] { 63, 33 },
			new short[2] { 64, 33 },
			new short[2] { 65, 33 },
			new short[2] { 69, 33 },
			new short[2] { 70, 33 },
			new short[2] { 71, 33 },
			new short[2] { 75, 33 },
			new short[2] { 76, 33 },
			new short[2] { 77, 33 },
			new short[2] { 81, 33 },
			new short[2] { 82, 33 },
			new short[2] { 83, 33 },
			new short[2] { 87, 33 },
			new short[2] { 88, 33 },
			new short[2] { 89, 33 },
			new short[2] { 112, 30 },
			new short[2] { 113, 30 },
			new short[2] { 114, 30 },
			new short[2] { 124, 30 }
		};
		List<short[]> wildBlockCore15 = new List<short[]>
		{
			new short[2] { 93, 33 },
			new short[2] { 94, 33 },
			new short[2] { 95, 33 },
			new short[2] { 96, 33 },
			new short[2] { 97, 33 },
			new short[2] { 98, 33 },
			new short[2] { 99, 33 },
			new short[2] { 100, 33 },
			new short[2] { 101, 33 },
			new short[2] { 102, 33 },
			new short[2] { 103, 33 },
			new short[2] { 104, 33 },
			new short[2] { 105, 33 },
			new short[2] { 106, 33 },
			new short[2] { 107, 33 },
			new short[2] { 108, 33 },
			new short[2] { 115, 40 },
			new short[2] { 116, 40 },
			new short[2] { 117, 40 },
			new short[2] { 124, 30 }
		};
		List<short[]> bigBaseBlockCore15 = new List<short[]>
		{
			new short[2] { 73, 1 },
			new short[2] { 74, 1 },
			new short[2] { 78, 1 },
			new short[2] { 84, 1 },
			new short[2] { 90, 1 }
		};
		list = new List<short[]>();
		List<short[]> seriesBlockCore15 = list;
		list = new List<short[]>();
		List<short[]> encircleBlockCore15 = list;
		short[] sceneryBlockCore15 = new short[0];
		list2 = new List<short>();
		List<short> lovingItemSubTypes15 = list2;
		list2 = new List<short>();
		dataArray15.Add(new MapAreaItem(74, 7, 316, 317, 20, worldMapPos15, neighborAreas15, "largemap_part_1_changgui_0", miniMapIcons15, null, miniMapPos15, 0, 0, 5, enemyNests15, enemyNestCreationDateRanges15, settlementBlockCore15, organizationId15, -1, 0, null, developedBlockCore15, normalBlockCore15, wildBlockCore15, bigBaseBlockCore15, seriesBlockCore15, encircleBlockCore15, sceneryBlockCore15, 318, 319, null, null, EMapAreaAreaDirection.South, lovingItemSubTypes15, list2, showDarkAshStatus: true));
		List<MapAreaItem> dataArray16 = _dataArray;
		sbyte[] worldMapPos16 = new sbyte[2] { 16, 20 };
		AreaTravelRoute[] neighborAreas16 = new AreaTravelRoute[0];
		string[] miniMapIcons16 = new string[4] { "largemap_part_2_icon_hei_2", "largemap_part_2_icon_jin_2", "largemap_part_2_icon_lan_2", "largemap_part_2_icon_hui_2" };
		short[] miniMapPos16 = new short[2] { 90, 200 };
		List<EnemyNestCreationInfo[]> enemyNests16 = new List<EnemyNestCreationInfo[]>
		{
			new EnemyNestCreationInfo[2]
			{
				new EnemyNestCreationInfo(0, 2),
				new EnemyNestCreationInfo(1, 2)
			},
			new EnemyNestCreationInfo[2]
			{
				new EnemyNestCreationInfo(1, 2),
				new EnemyNestCreationInfo(2, 3)
			},
			new EnemyNestCreationInfo[4]
			{
				new EnemyNestCreationInfo(2, 3),
				new EnemyNestCreationInfo(3, 3),
				new EnemyNestCreationInfo(4, 4),
				new EnemyNestCreationInfo(5, 4)
			},
			new EnemyNestCreationInfo[1]
			{
				new EnemyNestCreationInfo(12, 2)
			}
		};
		List<IntPair> enemyNestCreationDateRanges16 = new List<IntPair>
		{
			new IntPair(0, 6),
			new IntPair(6, 12),
			new IntPair(12, 24),
			new IntPair(0, 0)
		};
		short[] settlementBlockCore16 = new short[1] { 35 };
		sbyte[] organizationId16 = new sbyte[1] { 37 };
		List<short[]> developedBlockCore16 = new List<short[]>
		{
			new short[2] { 39, 33 },
			new short[2] { 40, 33 },
			new short[2] { 41, 33 },
			new short[2] { 42, 33 },
			new short[2] { 43, 33 },
			new short[2] { 44, 33 },
			new short[2] { 45, 33 },
			new short[2] { 46, 33 },
			new short[2] { 47, 33 },
			new short[2] { 48, 33 },
			new short[2] { 49, 33 },
			new short[2] { 50, 33 },
			new short[2] { 51, 33 },
			new short[2] { 52, 33 },
			new short[2] { 53, 33 },
			new short[2] { 54, 33 },
			new short[2] { 55, 33 },
			new short[2] { 56, 33 },
			new short[2] { 109, 10 },
			new short[2] { 110, 10 },
			new short[2] { 111, 10 }
		};
		List<short[]> normalBlockCore16 = new List<short[]>
		{
			new short[2] { 57, 33 },
			new short[2] { 58, 33 },
			new short[2] { 59, 33 },
			new short[2] { 63, 33 },
			new short[2] { 64, 33 },
			new short[2] { 65, 33 },
			new short[2] { 69, 33 },
			new short[2] { 70, 33 },
			new short[2] { 71, 33 },
			new short[2] { 75, 33 },
			new short[2] { 76, 33 },
			new short[2] { 77, 33 },
			new short[2] { 81, 33 },
			new short[2] { 82, 33 },
			new short[2] { 83, 33 },
			new short[2] { 87, 33 },
			new short[2] { 88, 33 },
			new short[2] { 89, 33 },
			new short[2] { 112, 30 },
			new short[2] { 113, 30 },
			new short[2] { 114, 30 },
			new short[2] { 124, 30 }
		};
		List<short[]> wildBlockCore16 = new List<short[]>
		{
			new short[2] { 93, 33 },
			new short[2] { 94, 33 },
			new short[2] { 95, 33 },
			new short[2] { 96, 33 },
			new short[2] { 97, 33 },
			new short[2] { 98, 33 },
			new short[2] { 99, 33 },
			new short[2] { 100, 33 },
			new short[2] { 101, 33 },
			new short[2] { 102, 33 },
			new short[2] { 103, 33 },
			new short[2] { 104, 33 },
			new short[2] { 105, 33 },
			new short[2] { 106, 33 },
			new short[2] { 107, 33 },
			new short[2] { 108, 33 },
			new short[2] { 115, 40 },
			new short[2] { 116, 40 },
			new short[2] { 117, 40 },
			new short[2] { 124, 30 }
		};
		List<short[]> bigBaseBlockCore16 = new List<short[]>
		{
			new short[2] { 73, 1 },
			new short[2] { 74, 1 },
			new short[2] { 78, 1 },
			new short[2] { 84, 1 },
			new short[2] { 90, 1 }
		};
		list = new List<short[]>();
		List<short[]> seriesBlockCore16 = list;
		list = new List<short[]>();
		List<short[]> encircleBlockCore16 = list;
		short[] sceneryBlockCore16 = new short[0];
		list2 = new List<short>();
		List<short> lovingItemSubTypes16 = list2;
		list2 = new List<short>();
		dataArray16.Add(new MapAreaItem(75, 7, 320, 321, 20, worldMapPos16, neighborAreas16, "largemap_part_1_changgui_2", miniMapIcons16, null, miniMapPos16, 0, 0, 5, enemyNests16, enemyNestCreationDateRanges16, settlementBlockCore16, organizationId16, -1, 0, null, developedBlockCore16, normalBlockCore16, wildBlockCore16, bigBaseBlockCore16, seriesBlockCore16, encircleBlockCore16, sceneryBlockCore16, 322, 323, null, null, EMapAreaAreaDirection.South, lovingItemSubTypes16, list2, showDarkAshStatus: true));
		List<MapAreaItem> dataArray17 = _dataArray;
		sbyte[] worldMapPos17 = new sbyte[2] { 18, 19 };
		AreaTravelRoute[] neighborAreas17 = new AreaTravelRoute[0];
		string[] miniMapIcons17 = new string[4] { "largemap_part_2_icon_hei_1", "largemap_part_2_icon_jin_1", "largemap_part_2_icon_lan_1", "largemap_part_2_icon_hui_1" };
		short[] miniMapPos17 = new short[2] { 170, 200 };
		List<EnemyNestCreationInfo[]> enemyNests17 = new List<EnemyNestCreationInfo[]>
		{
			new EnemyNestCreationInfo[2]
			{
				new EnemyNestCreationInfo(0, 2),
				new EnemyNestCreationInfo(1, 2)
			},
			new EnemyNestCreationInfo[2]
			{
				new EnemyNestCreationInfo(1, 2),
				new EnemyNestCreationInfo(2, 3)
			},
			new EnemyNestCreationInfo[4]
			{
				new EnemyNestCreationInfo(2, 3),
				new EnemyNestCreationInfo(3, 3),
				new EnemyNestCreationInfo(4, 4),
				new EnemyNestCreationInfo(5, 4)
			},
			new EnemyNestCreationInfo[1]
			{
				new EnemyNestCreationInfo(12, 2)
			}
		};
		List<IntPair> enemyNestCreationDateRanges17 = new List<IntPair>
		{
			new IntPair(0, 6),
			new IntPair(6, 12),
			new IntPair(12, 24),
			new IntPair(0, 0)
		};
		short[] settlementBlockCore17 = new short[1] { 34 };
		sbyte[] organizationId17 = new sbyte[1] { 36 };
		List<short[]> developedBlockCore17 = new List<short[]>
		{
			new short[2] { 39, 33 },
			new short[2] { 40, 33 },
			new short[2] { 41, 33 },
			new short[2] { 42, 33 },
			new short[2] { 43, 33 },
			new short[2] { 44, 33 },
			new short[2] { 45, 33 },
			new short[2] { 46, 33 },
			new short[2] { 47, 33 },
			new short[2] { 48, 33 },
			new short[2] { 49, 33 },
			new short[2] { 50, 33 },
			new short[2] { 51, 33 },
			new short[2] { 52, 33 },
			new short[2] { 53, 33 },
			new short[2] { 54, 33 },
			new short[2] { 55, 33 },
			new short[2] { 56, 33 },
			new short[2] { 109, 10 },
			new short[2] { 110, 10 },
			new short[2] { 111, 10 }
		};
		List<short[]> normalBlockCore17 = new List<short[]>
		{
			new short[2] { 57, 33 },
			new short[2] { 58, 33 },
			new short[2] { 59, 33 },
			new short[2] { 63, 33 },
			new short[2] { 64, 33 },
			new short[2] { 65, 33 },
			new short[2] { 69, 33 },
			new short[2] { 70, 33 },
			new short[2] { 71, 33 },
			new short[2] { 75, 33 },
			new short[2] { 76, 33 },
			new short[2] { 77, 33 },
			new short[2] { 81, 33 },
			new short[2] { 82, 33 },
			new short[2] { 83, 33 },
			new short[2] { 87, 33 },
			new short[2] { 88, 33 },
			new short[2] { 89, 33 },
			new short[2] { 112, 30 },
			new short[2] { 113, 30 },
			new short[2] { 114, 30 },
			new short[2] { 124, 30 }
		};
		List<short[]> wildBlockCore17 = new List<short[]>
		{
			new short[2] { 93, 33 },
			new short[2] { 94, 33 },
			new short[2] { 95, 33 },
			new short[2] { 96, 33 },
			new short[2] { 97, 33 },
			new short[2] { 98, 33 },
			new short[2] { 99, 33 },
			new short[2] { 100, 33 },
			new short[2] { 101, 33 },
			new short[2] { 102, 33 },
			new short[2] { 103, 33 },
			new short[2] { 104, 33 },
			new short[2] { 105, 33 },
			new short[2] { 106, 33 },
			new short[2] { 107, 33 },
			new short[2] { 108, 33 },
			new short[2] { 115, 40 },
			new short[2] { 116, 40 },
			new short[2] { 117, 40 },
			new short[2] { 124, 30 }
		};
		List<short[]> bigBaseBlockCore17 = new List<short[]>
		{
			new short[2] { 73, 1 },
			new short[2] { 74, 1 },
			new short[2] { 78, 1 },
			new short[2] { 84, 1 },
			new short[2] { 90, 1 }
		};
		list = new List<short[]>();
		List<short[]> seriesBlockCore17 = list;
		list = new List<short[]>();
		List<short[]> encircleBlockCore17 = list;
		short[] sceneryBlockCore17 = new short[0];
		list2 = new List<short>();
		List<short> lovingItemSubTypes17 = list2;
		list2 = new List<short>();
		dataArray17.Add(new MapAreaItem(76, 7, 324, 325, 20, worldMapPos17, neighborAreas17, "largemap_part_1_changgui_1", miniMapIcons17, null, miniMapPos17, 0, 0, 5, enemyNests17, enemyNestCreationDateRanges17, settlementBlockCore17, organizationId17, -1, 0, null, developedBlockCore17, normalBlockCore17, wildBlockCore17, bigBaseBlockCore17, seriesBlockCore17, encircleBlockCore17, sceneryBlockCore17, 326, 327, null, null, EMapAreaAreaDirection.South, lovingItemSubTypes17, list2, showDarkAshStatus: true));
		List<MapAreaItem> dataArray18 = _dataArray;
		sbyte[] worldMapPos18 = new sbyte[2] { 21, 20 };
		AreaTravelRoute[] neighborAreas18 = new AreaTravelRoute[1]
		{
			new AreaTravelRoute(79, 24, new int[4] { 22, 20, 22, 19 })
		};
		string[] miniMapIcons18 = new string[4] { "largemap_part_2_icon_hei_2", "largemap_part_2_icon_jin_2", "largemap_part_2_icon_lan_2", "largemap_part_2_icon_hui_2" };
		short[] miniMapPos18 = new short[2] { 290, 240 };
		List<EnemyNestCreationInfo[]> enemyNests18 = new List<EnemyNestCreationInfo[]>
		{
			new EnemyNestCreationInfo[2]
			{
				new EnemyNestCreationInfo(0, 2),
				new EnemyNestCreationInfo(1, 2)
			},
			new EnemyNestCreationInfo[2]
			{
				new EnemyNestCreationInfo(1, 2),
				new EnemyNestCreationInfo(2, 3)
			},
			new EnemyNestCreationInfo[4]
			{
				new EnemyNestCreationInfo(2, 3),
				new EnemyNestCreationInfo(3, 3),
				new EnemyNestCreationInfo(4, 4),
				new EnemyNestCreationInfo(5, 4)
			},
			new EnemyNestCreationInfo[1]
			{
				new EnemyNestCreationInfo(12, 2)
			}
		};
		List<IntPair> enemyNestCreationDateRanges18 = new List<IntPair>
		{
			new IntPair(0, 6),
			new IntPair(6, 12),
			new IntPair(12, 24),
			new IntPair(0, 0)
		};
		short[] settlementBlockCore18 = new short[1] { 34 };
		sbyte[] organizationId18 = new sbyte[1] { 36 };
		List<short[]> developedBlockCore18 = new List<short[]>
		{
			new short[2] { 39, 33 },
			new short[2] { 40, 33 },
			new short[2] { 41, 33 },
			new short[2] { 42, 33 },
			new short[2] { 43, 33 },
			new short[2] { 44, 33 },
			new short[2] { 45, 33 },
			new short[2] { 46, 33 },
			new short[2] { 47, 33 },
			new short[2] { 48, 33 },
			new short[2] { 49, 33 },
			new short[2] { 50, 33 },
			new short[2] { 51, 33 },
			new short[2] { 52, 33 },
			new short[2] { 53, 33 },
			new short[2] { 54, 33 },
			new short[2] { 55, 33 },
			new short[2] { 56, 33 },
			new short[2] { 109, 10 },
			new short[2] { 110, 10 },
			new short[2] { 111, 10 }
		};
		List<short[]> normalBlockCore18 = new List<short[]>
		{
			new short[2] { 57, 33 },
			new short[2] { 58, 33 },
			new short[2] { 59, 33 },
			new short[2] { 63, 33 },
			new short[2] { 64, 33 },
			new short[2] { 65, 33 },
			new short[2] { 69, 33 },
			new short[2] { 70, 33 },
			new short[2] { 71, 33 },
			new short[2] { 75, 33 },
			new short[2] { 76, 33 },
			new short[2] { 77, 33 },
			new short[2] { 81, 33 },
			new short[2] { 82, 33 },
			new short[2] { 83, 33 },
			new short[2] { 87, 33 },
			new short[2] { 88, 33 },
			new short[2] { 89, 33 },
			new short[2] { 112, 30 },
			new short[2] { 113, 30 },
			new short[2] { 114, 30 },
			new short[2] { 124, 30 }
		};
		List<short[]> wildBlockCore18 = new List<short[]>
		{
			new short[2] { 93, 33 },
			new short[2] { 94, 33 },
			new short[2] { 95, 33 },
			new short[2] { 96, 33 },
			new short[2] { 97, 33 },
			new short[2] { 98, 33 },
			new short[2] { 99, 33 },
			new short[2] { 100, 33 },
			new short[2] { 101, 33 },
			new short[2] { 102, 33 },
			new short[2] { 103, 33 },
			new short[2] { 104, 33 },
			new short[2] { 105, 33 },
			new short[2] { 106, 33 },
			new short[2] { 107, 33 },
			new short[2] { 108, 33 },
			new short[2] { 115, 40 },
			new short[2] { 116, 40 },
			new short[2] { 117, 40 },
			new short[2] { 124, 30 }
		};
		List<short[]> bigBaseBlockCore18 = new List<short[]>
		{
			new short[2] { 73, 1 },
			new short[2] { 74, 1 },
			new short[2] { 78, 1 },
			new short[2] { 84, 1 },
			new short[2] { 90, 1 }
		};
		list = new List<short[]>();
		List<short[]> seriesBlockCore18 = list;
		list = new List<short[]>();
		List<short[]> encircleBlockCore18 = list;
		short[] sceneryBlockCore18 = new short[0];
		list2 = new List<short>();
		List<short> lovingItemSubTypes18 = list2;
		list2 = new List<short>();
		dataArray18.Add(new MapAreaItem(77, 7, 328, 329, 20, worldMapPos18, neighborAreas18, "largemap_part_1_changgui_2", miniMapIcons18, null, miniMapPos18, 0, 0, 5, enemyNests18, enemyNestCreationDateRanges18, settlementBlockCore18, organizationId18, -1, 0, null, developedBlockCore18, normalBlockCore18, wildBlockCore18, bigBaseBlockCore18, seriesBlockCore18, encircleBlockCore18, sceneryBlockCore18, 330, 331, null, null, EMapAreaAreaDirection.South, lovingItemSubTypes18, list2, showDarkAshStatus: true));
		List<MapAreaItem> dataArray19 = _dataArray;
		sbyte[] worldMapPos19 = new sbyte[2] { 17, 17 };
		AreaTravelRoute[] neighborAreas19 = new AreaTravelRoute[1]
		{
			new AreaTravelRoute(132, 22, new int[4] { 18, 17, 18, 16 })
		};
		string[] miniMapIcons19 = new string[4] { "largemap_part_2_icon_hei_0", "largemap_part_2_icon_jin_0", "largemap_part_2_icon_lan_0", "largemap_part_2_icon_hui_0" };
		short[] miniMapPos19 = new short[2] { 130, 120 };
		List<EnemyNestCreationInfo[]> enemyNests19 = new List<EnemyNestCreationInfo[]>
		{
			new EnemyNestCreationInfo[2]
			{
				new EnemyNestCreationInfo(0, 2),
				new EnemyNestCreationInfo(1, 2)
			},
			new EnemyNestCreationInfo[2]
			{
				new EnemyNestCreationInfo(1, 2),
				new EnemyNestCreationInfo(2, 3)
			},
			new EnemyNestCreationInfo[4]
			{
				new EnemyNestCreationInfo(2, 3),
				new EnemyNestCreationInfo(3, 3),
				new EnemyNestCreationInfo(4, 4),
				new EnemyNestCreationInfo(5, 4)
			},
			new EnemyNestCreationInfo[1]
			{
				new EnemyNestCreationInfo(12, 2)
			}
		};
		List<IntPair> enemyNestCreationDateRanges19 = new List<IntPair>
		{
			new IntPair(0, 6),
			new IntPair(6, 12),
			new IntPair(12, 24),
			new IntPair(0, 0)
		};
		short[] settlementBlockCore19 = new short[1] { 34 };
		sbyte[] organizationId19 = new sbyte[1] { 36 };
		List<short[]> developedBlockCore19 = new List<short[]>
		{
			new short[2] { 39, 33 },
			new short[2] { 40, 33 },
			new short[2] { 41, 33 },
			new short[2] { 42, 33 },
			new short[2] { 43, 33 },
			new short[2] { 44, 33 },
			new short[2] { 45, 33 },
			new short[2] { 46, 33 },
			new short[2] { 47, 33 },
			new short[2] { 48, 33 },
			new short[2] { 49, 33 },
			new short[2] { 50, 33 },
			new short[2] { 51, 33 },
			new short[2] { 52, 33 },
			new short[2] { 53, 33 },
			new short[2] { 54, 33 },
			new short[2] { 55, 33 },
			new short[2] { 56, 33 },
			new short[2] { 109, 10 },
			new short[2] { 110, 10 },
			new short[2] { 111, 10 }
		};
		List<short[]> normalBlockCore19 = new List<short[]>
		{
			new short[2] { 57, 33 },
			new short[2] { 58, 33 },
			new short[2] { 59, 33 },
			new short[2] { 63, 33 },
			new short[2] { 64, 33 },
			new short[2] { 65, 33 },
			new short[2] { 69, 33 },
			new short[2] { 70, 33 },
			new short[2] { 71, 33 },
			new short[2] { 75, 33 },
			new short[2] { 76, 33 },
			new short[2] { 77, 33 },
			new short[2] { 81, 33 },
			new short[2] { 82, 33 },
			new short[2] { 83, 33 },
			new short[2] { 87, 33 },
			new short[2] { 88, 33 },
			new short[2] { 89, 33 },
			new short[2] { 112, 30 },
			new short[2] { 113, 30 },
			new short[2] { 114, 30 },
			new short[2] { 124, 30 }
		};
		List<short[]> wildBlockCore19 = new List<short[]>
		{
			new short[2] { 93, 33 },
			new short[2] { 94, 33 },
			new short[2] { 95, 33 },
			new short[2] { 96, 33 },
			new short[2] { 97, 33 },
			new short[2] { 98, 33 },
			new short[2] { 99, 33 },
			new short[2] { 100, 33 },
			new short[2] { 101, 33 },
			new short[2] { 102, 33 },
			new short[2] { 103, 33 },
			new short[2] { 104, 33 },
			new short[2] { 105, 33 },
			new short[2] { 106, 33 },
			new short[2] { 107, 33 },
			new short[2] { 108, 33 },
			new short[2] { 115, 40 },
			new short[2] { 116, 40 },
			new short[2] { 117, 40 },
			new short[2] { 124, 30 }
		};
		List<short[]> bigBaseBlockCore19 = new List<short[]>
		{
			new short[2] { 73, 1 },
			new short[2] { 74, 1 },
			new short[2] { 78, 1 },
			new short[2] { 84, 1 },
			new short[2] { 90, 1 }
		};
		list = new List<short[]>();
		List<short[]> seriesBlockCore19 = list;
		list = new List<short[]>();
		List<short[]> encircleBlockCore19 = list;
		short[] sceneryBlockCore19 = new short[0];
		list2 = new List<short>();
		List<short> lovingItemSubTypes19 = list2;
		list2 = new List<short>();
		dataArray19.Add(new MapAreaItem(78, 7, 332, 333, 20, worldMapPos19, neighborAreas19, "largemap_part_1_changgui_0", miniMapIcons19, null, miniMapPos19, 0, 0, 5, enemyNests19, enemyNestCreationDateRanges19, settlementBlockCore19, organizationId19, -1, 0, null, developedBlockCore19, normalBlockCore19, wildBlockCore19, bigBaseBlockCore19, seriesBlockCore19, encircleBlockCore19, sceneryBlockCore19, 334, 335, null, null, EMapAreaAreaDirection.South, lovingItemSubTypes19, list2, showDarkAshStatus: true));
		List<MapAreaItem> dataArray20 = _dataArray;
		sbyte[] worldMapPos20 = new sbyte[2] { 22, 18 };
		AreaTravelRoute[] neighborAreas20 = new AreaTravelRoute[0];
		string[] miniMapIcons20 = new string[4] { "largemap_part_2_icon_hei_0", "largemap_part_2_icon_jin_0", "largemap_part_2_icon_lan_0", "largemap_part_2_icon_hui_0" };
		short[] miniMapPos20 = new short[2] { 330, 160 };
		List<EnemyNestCreationInfo[]> enemyNests20 = new List<EnemyNestCreationInfo[]>
		{
			new EnemyNestCreationInfo[2]
			{
				new EnemyNestCreationInfo(0, 2),
				new EnemyNestCreationInfo(1, 2)
			},
			new EnemyNestCreationInfo[2]
			{
				new EnemyNestCreationInfo(1, 2),
				new EnemyNestCreationInfo(2, 3)
			},
			new EnemyNestCreationInfo[4]
			{
				new EnemyNestCreationInfo(2, 3),
				new EnemyNestCreationInfo(3, 3),
				new EnemyNestCreationInfo(4, 4),
				new EnemyNestCreationInfo(5, 4)
			},
			new EnemyNestCreationInfo[1]
			{
				new EnemyNestCreationInfo(12, 2)
			}
		};
		List<IntPair> enemyNestCreationDateRanges20 = new List<IntPair>
		{
			new IntPair(0, 6),
			new IntPair(6, 12),
			new IntPair(12, 24),
			new IntPair(0, 0)
		};
		short[] settlementBlockCore20 = new short[1] { 34 };
		sbyte[] organizationId20 = new sbyte[1] { 36 };
		List<short[]> developedBlockCore20 = new List<short[]>
		{
			new short[2] { 39, 33 },
			new short[2] { 40, 33 },
			new short[2] { 41, 33 },
			new short[2] { 42, 33 },
			new short[2] { 43, 33 },
			new short[2] { 44, 33 },
			new short[2] { 45, 33 },
			new short[2] { 46, 33 },
			new short[2] { 47, 33 },
			new short[2] { 48, 33 },
			new short[2] { 49, 33 },
			new short[2] { 50, 33 },
			new short[2] { 51, 33 },
			new short[2] { 52, 33 },
			new short[2] { 53, 33 },
			new short[2] { 54, 33 },
			new short[2] { 55, 33 },
			new short[2] { 56, 33 },
			new short[2] { 109, 10 },
			new short[2] { 110, 10 },
			new short[2] { 111, 10 }
		};
		List<short[]> normalBlockCore20 = new List<short[]>
		{
			new short[2] { 57, 33 },
			new short[2] { 58, 33 },
			new short[2] { 59, 33 },
			new short[2] { 63, 33 },
			new short[2] { 64, 33 },
			new short[2] { 65, 33 },
			new short[2] { 69, 33 },
			new short[2] { 70, 33 },
			new short[2] { 71, 33 },
			new short[2] { 75, 33 },
			new short[2] { 76, 33 },
			new short[2] { 77, 33 },
			new short[2] { 81, 33 },
			new short[2] { 82, 33 },
			new short[2] { 83, 33 },
			new short[2] { 87, 33 },
			new short[2] { 88, 33 },
			new short[2] { 89, 33 },
			new short[2] { 112, 30 },
			new short[2] { 113, 30 },
			new short[2] { 114, 30 },
			new short[2] { 124, 30 }
		};
		List<short[]> wildBlockCore20 = new List<short[]>
		{
			new short[2] { 93, 33 },
			new short[2] { 94, 33 },
			new short[2] { 95, 33 },
			new short[2] { 96, 33 },
			new short[2] { 97, 33 },
			new short[2] { 98, 33 },
			new short[2] { 99, 33 },
			new short[2] { 100, 33 },
			new short[2] { 101, 33 },
			new short[2] { 102, 33 },
			new short[2] { 103, 33 },
			new short[2] { 104, 33 },
			new short[2] { 105, 33 },
			new short[2] { 106, 33 },
			new short[2] { 107, 33 },
			new short[2] { 108, 33 },
			new short[2] { 115, 40 },
			new short[2] { 116, 40 },
			new short[2] { 117, 40 },
			new short[2] { 124, 30 }
		};
		List<short[]> bigBaseBlockCore20 = new List<short[]>
		{
			new short[2] { 73, 1 },
			new short[2] { 74, 1 },
			new short[2] { 78, 1 },
			new short[2] { 84, 1 },
			new short[2] { 90, 1 }
		};
		list = new List<short[]>();
		List<short[]> seriesBlockCore20 = list;
		list = new List<short[]>();
		List<short[]> encircleBlockCore20 = list;
		short[] sceneryBlockCore20 = new short[0];
		list2 = new List<short>();
		List<short> lovingItemSubTypes20 = list2;
		list2 = new List<short>();
		dataArray20.Add(new MapAreaItem(79, 7, 336, 337, 20, worldMapPos20, neighborAreas20, "largemap_part_1_changgui_0", miniMapIcons20, null, miniMapPos20, 0, 0, 5, enemyNests20, enemyNestCreationDateRanges20, settlementBlockCore20, organizationId20, -1, 0, null, developedBlockCore20, normalBlockCore20, wildBlockCore20, bigBaseBlockCore20, seriesBlockCore20, encircleBlockCore20, sceneryBlockCore20, 338, 339, null, null, EMapAreaAreaDirection.South, lovingItemSubTypes20, list2, showDarkAshStatus: true));
		List<MapAreaItem> dataArray21 = _dataArray;
		sbyte[] worldMapPos21 = new sbyte[2] { 10, 10 };
		AreaTravelRoute[] neighborAreas21 = new AreaTravelRoute[0];
		string[] miniMapIcons21 = new string[4] { "largemap_part_2_icon_hei_0", "largemap_part_2_icon_jin_0", "largemap_part_2_icon_lan_0", "largemap_part_2_icon_hui_0" };
		short[] miniMapPos21 = new short[2] { 120, 270 };
		List<EnemyNestCreationInfo[]> enemyNests21 = new List<EnemyNestCreationInfo[]>
		{
			new EnemyNestCreationInfo[2]
			{
				new EnemyNestCreationInfo(0, 2),
				new EnemyNestCreationInfo(1, 2)
			},
			new EnemyNestCreationInfo[2]
			{
				new EnemyNestCreationInfo(1, 2),
				new EnemyNestCreationInfo(2, 3)
			},
			new EnemyNestCreationInfo[4]
			{
				new EnemyNestCreationInfo(2, 3),
				new EnemyNestCreationInfo(3, 3),
				new EnemyNestCreationInfo(4, 4),
				new EnemyNestCreationInfo(5, 4)
			},
			new EnemyNestCreationInfo[1]
			{
				new EnemyNestCreationInfo(12, 2)
			}
		};
		List<IntPair> enemyNestCreationDateRanges21 = new List<IntPair>
		{
			new IntPair(0, 6),
			new IntPair(6, 12),
			new IntPair(12, 24),
			new IntPair(0, 0)
		};
		short[] settlementBlockCore21 = new short[1] { 36 };
		sbyte[] organizationId21 = new sbyte[1] { 38 };
		List<short[]> developedBlockCore21 = new List<short[]>
		{
			new short[2] { 39, 33 },
			new short[2] { 40, 33 },
			new short[2] { 41, 33 },
			new short[2] { 42, 33 },
			new short[2] { 43, 33 },
			new short[2] { 44, 33 },
			new short[2] { 45, 33 },
			new short[2] { 46, 33 },
			new short[2] { 47, 33 },
			new short[2] { 48, 33 },
			new short[2] { 49, 33 },
			new short[2] { 50, 33 },
			new short[2] { 51, 33 },
			new short[2] { 52, 33 },
			new short[2] { 53, 33 },
			new short[2] { 54, 33 },
			new short[2] { 55, 33 },
			new short[2] { 56, 33 },
			new short[2] { 109, 10 },
			new short[2] { 110, 10 },
			new short[2] { 111, 10 }
		};
		List<short[]> normalBlockCore21 = new List<short[]>
		{
			new short[2] { 57, 33 },
			new short[2] { 58, 33 },
			new short[2] { 59, 33 },
			new short[2] { 63, 33 },
			new short[2] { 64, 33 },
			new short[2] { 65, 33 },
			new short[2] { 69, 33 },
			new short[2] { 70, 33 },
			new short[2] { 71, 33 },
			new short[2] { 75, 33 },
			new short[2] { 76, 33 },
			new short[2] { 77, 33 },
			new short[2] { 81, 33 },
			new short[2] { 82, 33 },
			new short[2] { 83, 33 },
			new short[2] { 87, 33 },
			new short[2] { 88, 33 },
			new short[2] { 89, 33 },
			new short[2] { 112, 30 },
			new short[2] { 113, 30 },
			new short[2] { 114, 30 },
			new short[2] { 124, 30 }
		};
		List<short[]> wildBlockCore21 = new List<short[]>
		{
			new short[2] { 93, 33 },
			new short[2] { 94, 33 },
			new short[2] { 95, 33 },
			new short[2] { 96, 33 },
			new short[2] { 97, 33 },
			new short[2] { 98, 33 },
			new short[2] { 99, 33 },
			new short[2] { 100, 33 },
			new short[2] { 101, 33 },
			new short[2] { 102, 33 },
			new short[2] { 103, 33 },
			new short[2] { 104, 33 },
			new short[2] { 105, 33 },
			new short[2] { 106, 33 },
			new short[2] { 107, 33 },
			new short[2] { 108, 33 },
			new short[2] { 115, 40 },
			new short[2] { 116, 40 },
			new short[2] { 117, 40 },
			new short[2] { 124, 30 }
		};
		List<short[]> bigBaseBlockCore21 = new List<short[]>
		{
			new short[2] { 60, 1 },
			new short[2] { 79, 1 },
			new short[2] { 80, 1 },
			new short[2] { 84, 1 },
			new short[2] { 90, 1 }
		};
		list = new List<short[]>();
		List<short[]> seriesBlockCore21 = list;
		list = new List<short[]>();
		List<short[]> encircleBlockCore21 = list;
		short[] sceneryBlockCore21 = new short[0];
		list2 = new List<short>();
		List<short> lovingItemSubTypes21 = list2;
		list2 = new List<short>();
		dataArray21.Add(new MapAreaItem(80, 8, 340, 341, 20, worldMapPos21, neighborAreas21, "largemap_part_1_changgui_0", miniMapIcons21, null, miniMapPos21, 0, 0, 5, enemyNests21, enemyNestCreationDateRanges21, settlementBlockCore21, organizationId21, -1, 0, null, developedBlockCore21, normalBlockCore21, wildBlockCore21, bigBaseBlockCore21, seriesBlockCore21, encircleBlockCore21, sceneryBlockCore21, 342, 343, null, null, EMapAreaAreaDirection.South, lovingItemSubTypes21, list2, showDarkAshStatus: true));
		List<MapAreaItem> dataArray22 = _dataArray;
		sbyte[] worldMapPos22 = new sbyte[2] { 13, 7 };
		AreaTravelRoute[] neighborAreas22 = new AreaTravelRoute[3]
		{
			new AreaTravelRoute(82, 12, new int[2] { 13, 6 }),
			new AreaTravelRoute(83, 6, null),
			new AreaTravelRoute(85, 12, new int[2] { 13, 6 })
		};
		string[] miniMapIcons22 = new string[4] { "largemap_part_2_icon_hei_2", "largemap_part_2_icon_jin_2", "largemap_part_2_icon_lan_2", "largemap_part_2_icon_hui_2" };
		short[] miniMapPos22 = new short[2] { 240, 150 };
		List<EnemyNestCreationInfo[]> enemyNests22 = new List<EnemyNestCreationInfo[]>
		{
			new EnemyNestCreationInfo[2]
			{
				new EnemyNestCreationInfo(0, 2),
				new EnemyNestCreationInfo(1, 2)
			},
			new EnemyNestCreationInfo[2]
			{
				new EnemyNestCreationInfo(1, 2),
				new EnemyNestCreationInfo(2, 3)
			},
			new EnemyNestCreationInfo[4]
			{
				new EnemyNestCreationInfo(2, 3),
				new EnemyNestCreationInfo(3, 3),
				new EnemyNestCreationInfo(4, 4),
				new EnemyNestCreationInfo(5, 4)
			},
			new EnemyNestCreationInfo[1]
			{
				new EnemyNestCreationInfo(12, 2)
			}
		};
		List<IntPair> enemyNestCreationDateRanges22 = new List<IntPair>
		{
			new IntPair(0, 6),
			new IntPair(6, 12),
			new IntPair(12, 24),
			new IntPair(0, 0)
		};
		short[] settlementBlockCore22 = new short[1] { 35 };
		sbyte[] organizationId22 = new sbyte[1] { 37 };
		List<short[]> developedBlockCore22 = new List<short[]>
		{
			new short[2] { 39, 33 },
			new short[2] { 40, 33 },
			new short[2] { 41, 33 },
			new short[2] { 42, 33 },
			new short[2] { 43, 33 },
			new short[2] { 44, 33 },
			new short[2] { 45, 33 },
			new short[2] { 46, 33 },
			new short[2] { 47, 33 },
			new short[2] { 48, 33 },
			new short[2] { 49, 33 },
			new short[2] { 50, 33 },
			new short[2] { 51, 33 },
			new short[2] { 52, 33 },
			new short[2] { 53, 33 },
			new short[2] { 54, 33 },
			new short[2] { 55, 33 },
			new short[2] { 56, 33 },
			new short[2] { 109, 10 },
			new short[2] { 110, 10 },
			new short[2] { 111, 10 }
		};
		List<short[]> normalBlockCore22 = new List<short[]>
		{
			new short[2] { 57, 33 },
			new short[2] { 58, 33 },
			new short[2] { 59, 33 },
			new short[2] { 63, 33 },
			new short[2] { 64, 33 },
			new short[2] { 65, 33 },
			new short[2] { 69, 33 },
			new short[2] { 70, 33 },
			new short[2] { 71, 33 },
			new short[2] { 75, 33 },
			new short[2] { 76, 33 },
			new short[2] { 77, 33 },
			new short[2] { 81, 33 },
			new short[2] { 82, 33 },
			new short[2] { 83, 33 },
			new short[2] { 87, 33 },
			new short[2] { 88, 33 },
			new short[2] { 89, 33 },
			new short[2] { 112, 30 },
			new short[2] { 113, 30 },
			new short[2] { 114, 30 },
			new short[2] { 124, 30 }
		};
		List<short[]> wildBlockCore22 = new List<short[]>
		{
			new short[2] { 93, 33 },
			new short[2] { 94, 33 },
			new short[2] { 95, 33 },
			new short[2] { 96, 33 },
			new short[2] { 97, 33 },
			new short[2] { 98, 33 },
			new short[2] { 99, 33 },
			new short[2] { 100, 33 },
			new short[2] { 101, 33 },
			new short[2] { 102, 33 },
			new short[2] { 103, 33 },
			new short[2] { 104, 33 },
			new short[2] { 105, 33 },
			new short[2] { 106, 33 },
			new short[2] { 107, 33 },
			new short[2] { 108, 33 },
			new short[2] { 115, 40 },
			new short[2] { 116, 40 },
			new short[2] { 117, 40 },
			new short[2] { 124, 30 }
		};
		List<short[]> bigBaseBlockCore22 = new List<short[]>
		{
			new short[2] { 60, 1 },
			new short[2] { 79, 1 },
			new short[2] { 80, 1 },
			new short[2] { 84, 1 },
			new short[2] { 90, 1 }
		};
		list = new List<short[]>();
		List<short[]> seriesBlockCore22 = list;
		list = new List<short[]>();
		List<short[]> encircleBlockCore22 = list;
		short[] sceneryBlockCore22 = new short[0];
		list2 = new List<short>();
		List<short> lovingItemSubTypes22 = list2;
		list2 = new List<short>();
		dataArray22.Add(new MapAreaItem(81, 8, 344, 345, 20, worldMapPos22, neighborAreas22, "largemap_part_1_changgui_2", miniMapIcons22, null, miniMapPos22, 0, 0, 5, enemyNests22, enemyNestCreationDateRanges22, settlementBlockCore22, organizationId22, -1, 0, null, developedBlockCore22, normalBlockCore22, wildBlockCore22, bigBaseBlockCore22, seriesBlockCore22, encircleBlockCore22, sceneryBlockCore22, 346, 347, null, null, EMapAreaAreaDirection.South, lovingItemSubTypes22, list2, showDarkAshStatus: true));
		List<MapAreaItem> dataArray23 = _dataArray;
		sbyte[] worldMapPos23 = new sbyte[2] { 14, 6 };
		AreaTravelRoute[] neighborAreas23 = new AreaTravelRoute[2]
		{
			new AreaTravelRoute(85, 12, new int[2] { 13, 6 }),
			new AreaTravelRoute(90, 42, new int[8] { 15, 6, 16, 6, 17, 6, 18, 6 })
		};
		string[] miniMapIcons23 = new string[4] { "largemap_part_2_icon_hei_0", "largemap_part_2_icon_jin_0", "largemap_part_2_icon_lan_0", "largemap_part_2_icon_hui_0" };
		short[] miniMapPos23 = new short[2] { 280, 110 };
		List<EnemyNestCreationInfo[]> enemyNests23 = new List<EnemyNestCreationInfo[]>
		{
			new EnemyNestCreationInfo[2]
			{
				new EnemyNestCreationInfo(0, 2),
				new EnemyNestCreationInfo(1, 2)
			},
			new EnemyNestCreationInfo[2]
			{
				new EnemyNestCreationInfo(1, 2),
				new EnemyNestCreationInfo(2, 3)
			},
			new EnemyNestCreationInfo[4]
			{
				new EnemyNestCreationInfo(2, 3),
				new EnemyNestCreationInfo(3, 3),
				new EnemyNestCreationInfo(4, 4),
				new EnemyNestCreationInfo(5, 4)
			},
			new EnemyNestCreationInfo[1]
			{
				new EnemyNestCreationInfo(12, 2)
			}
		};
		List<IntPair> enemyNestCreationDateRanges23 = new List<IntPair>
		{
			new IntPair(0, 6),
			new IntPair(6, 12),
			new IntPair(12, 24),
			new IntPair(0, 0)
		};
		short[] settlementBlockCore23 = new short[1] { 34 };
		sbyte[] organizationId23 = new sbyte[1] { 36 };
		List<short[]> developedBlockCore23 = new List<short[]>
		{
			new short[2] { 39, 33 },
			new short[2] { 40, 33 },
			new short[2] { 41, 33 },
			new short[2] { 42, 33 },
			new short[2] { 43, 33 },
			new short[2] { 44, 33 },
			new short[2] { 45, 33 },
			new short[2] { 46, 33 },
			new short[2] { 47, 33 },
			new short[2] { 48, 33 },
			new short[2] { 49, 33 },
			new short[2] { 50, 33 },
			new short[2] { 51, 33 },
			new short[2] { 52, 33 },
			new short[2] { 53, 33 },
			new short[2] { 54, 33 },
			new short[2] { 55, 33 },
			new short[2] { 56, 33 },
			new short[2] { 109, 10 },
			new short[2] { 110, 10 },
			new short[2] { 111, 10 }
		};
		List<short[]> normalBlockCore23 = new List<short[]>
		{
			new short[2] { 57, 33 },
			new short[2] { 58, 33 },
			new short[2] { 59, 33 },
			new short[2] { 63, 33 },
			new short[2] { 64, 33 },
			new short[2] { 65, 33 },
			new short[2] { 69, 33 },
			new short[2] { 70, 33 },
			new short[2] { 71, 33 },
			new short[2] { 75, 33 },
			new short[2] { 76, 33 },
			new short[2] { 77, 33 },
			new short[2] { 81, 33 },
			new short[2] { 82, 33 },
			new short[2] { 83, 33 },
			new short[2] { 87, 33 },
			new short[2] { 88, 33 },
			new short[2] { 89, 33 },
			new short[2] { 112, 30 },
			new short[2] { 113, 30 },
			new short[2] { 114, 30 },
			new short[2] { 124, 30 }
		};
		List<short[]> wildBlockCore23 = new List<short[]>
		{
			new short[2] { 93, 33 },
			new short[2] { 94, 33 },
			new short[2] { 95, 33 },
			new short[2] { 96, 33 },
			new short[2] { 97, 33 },
			new short[2] { 98, 33 },
			new short[2] { 99, 33 },
			new short[2] { 100, 33 },
			new short[2] { 101, 33 },
			new short[2] { 102, 33 },
			new short[2] { 103, 33 },
			new short[2] { 104, 33 },
			new short[2] { 105, 33 },
			new short[2] { 106, 33 },
			new short[2] { 107, 33 },
			new short[2] { 108, 33 },
			new short[2] { 115, 40 },
			new short[2] { 116, 40 },
			new short[2] { 117, 40 },
			new short[2] { 124, 30 }
		};
		List<short[]> bigBaseBlockCore23 = new List<short[]>
		{
			new short[2] { 60, 1 },
			new short[2] { 79, 1 },
			new short[2] { 80, 1 },
			new short[2] { 84, 1 },
			new short[2] { 90, 1 }
		};
		list = new List<short[]>();
		List<short[]> seriesBlockCore23 = list;
		list = new List<short[]>();
		List<short[]> encircleBlockCore23 = list;
		short[] sceneryBlockCore23 = new short[0];
		list2 = new List<short>();
		List<short> lovingItemSubTypes23 = list2;
		list2 = new List<short>();
		dataArray23.Add(new MapAreaItem(82, 8, 348, 349, 20, worldMapPos23, neighborAreas23, "largemap_part_1_changgui_0", miniMapIcons23, null, miniMapPos23, 0, 0, 5, enemyNests23, enemyNestCreationDateRanges23, settlementBlockCore23, organizationId23, -1, 0, null, developedBlockCore23, normalBlockCore23, wildBlockCore23, bigBaseBlockCore23, seriesBlockCore23, encircleBlockCore23, sceneryBlockCore23, 350, 351, null, null, EMapAreaAreaDirection.South, lovingItemSubTypes23, list2, showDarkAshStatus: true));
		List<MapAreaItem> dataArray24 = _dataArray;
		sbyte[] worldMapPos24 = new sbyte[2] { 13, 8 };
		AreaTravelRoute[] neighborAreas24 = new AreaTravelRoute[2]
		{
			new AreaTravelRoute(84, 6, null),
			new AreaTravelRoute(86, 22, new int[4] { 12, 8, 11, 8 })
		};
		string[] miniMapIcons24 = new string[4] { "largemap_part_2_icon_hei_1", "largemap_part_2_icon_jin_1", "largemap_part_2_icon_lan_1", "largemap_part_2_icon_hui_1" };
		short[] miniMapPos24 = new short[2] { 240, 190 };
		List<EnemyNestCreationInfo[]> enemyNests24 = new List<EnemyNestCreationInfo[]>
		{
			new EnemyNestCreationInfo[2]
			{
				new EnemyNestCreationInfo(0, 2),
				new EnemyNestCreationInfo(1, 2)
			},
			new EnemyNestCreationInfo[2]
			{
				new EnemyNestCreationInfo(1, 2),
				new EnemyNestCreationInfo(2, 3)
			},
			new EnemyNestCreationInfo[4]
			{
				new EnemyNestCreationInfo(2, 3),
				new EnemyNestCreationInfo(3, 3),
				new EnemyNestCreationInfo(4, 4),
				new EnemyNestCreationInfo(5, 4)
			},
			new EnemyNestCreationInfo[1]
			{
				new EnemyNestCreationInfo(12, 2)
			}
		};
		List<IntPair> enemyNestCreationDateRanges24 = new List<IntPair>
		{
			new IntPair(0, 6),
			new IntPair(6, 12),
			new IntPair(12, 24),
			new IntPair(0, 0)
		};
		short[] settlementBlockCore24 = new short[1] { 35 };
		sbyte[] organizationId24 = new sbyte[1] { 37 };
		List<short[]> developedBlockCore24 = new List<short[]>
		{
			new short[2] { 39, 33 },
			new short[2] { 40, 33 },
			new short[2] { 41, 33 },
			new short[2] { 42, 33 },
			new short[2] { 43, 33 },
			new short[2] { 44, 33 },
			new short[2] { 45, 33 },
			new short[2] { 46, 33 },
			new short[2] { 47, 33 },
			new short[2] { 48, 33 },
			new short[2] { 49, 33 },
			new short[2] { 50, 33 },
			new short[2] { 51, 33 },
			new short[2] { 52, 33 },
			new short[2] { 53, 33 },
			new short[2] { 54, 33 },
			new short[2] { 55, 33 },
			new short[2] { 56, 33 },
			new short[2] { 109, 10 },
			new short[2] { 110, 10 },
			new short[2] { 111, 10 }
		};
		List<short[]> normalBlockCore24 = new List<short[]>
		{
			new short[2] { 57, 33 },
			new short[2] { 58, 33 },
			new short[2] { 59, 33 },
			new short[2] { 63, 33 },
			new short[2] { 64, 33 },
			new short[2] { 65, 33 },
			new short[2] { 69, 33 },
			new short[2] { 70, 33 },
			new short[2] { 71, 33 },
			new short[2] { 75, 33 },
			new short[2] { 76, 33 },
			new short[2] { 77, 33 },
			new short[2] { 81, 33 },
			new short[2] { 82, 33 },
			new short[2] { 83, 33 },
			new short[2] { 87, 33 },
			new short[2] { 88, 33 },
			new short[2] { 89, 33 },
			new short[2] { 112, 30 },
			new short[2] { 113, 30 },
			new short[2] { 114, 30 },
			new short[2] { 124, 30 }
		};
		List<short[]> wildBlockCore24 = new List<short[]>
		{
			new short[2] { 93, 33 },
			new short[2] { 94, 33 },
			new short[2] { 95, 33 },
			new short[2] { 96, 33 },
			new short[2] { 97, 33 },
			new short[2] { 98, 33 },
			new short[2] { 99, 33 },
			new short[2] { 100, 33 },
			new short[2] { 101, 33 },
			new short[2] { 102, 33 },
			new short[2] { 103, 33 },
			new short[2] { 104, 33 },
			new short[2] { 105, 33 },
			new short[2] { 106, 33 },
			new short[2] { 107, 33 },
			new short[2] { 108, 33 },
			new short[2] { 115, 40 },
			new short[2] { 116, 40 },
			new short[2] { 117, 40 },
			new short[2] { 124, 30 }
		};
		List<short[]> bigBaseBlockCore24 = new List<short[]>
		{
			new short[2] { 60, 1 },
			new short[2] { 79, 1 },
			new short[2] { 80, 1 },
			new short[2] { 84, 1 },
			new short[2] { 90, 1 }
		};
		list = new List<short[]>();
		List<short[]> seriesBlockCore24 = list;
		list = new List<short[]>();
		List<short[]> encircleBlockCore24 = list;
		short[] sceneryBlockCore24 = new short[0];
		list2 = new List<short>();
		List<short> lovingItemSubTypes24 = list2;
		list2 = new List<short>();
		dataArray24.Add(new MapAreaItem(83, 8, 352, 353, 20, worldMapPos24, neighborAreas24, "largemap_part_1_changgui_1", miniMapIcons24, null, miniMapPos24, 0, 0, 5, enemyNests24, enemyNestCreationDateRanges24, settlementBlockCore24, organizationId24, -1, 0, null, developedBlockCore24, normalBlockCore24, wildBlockCore24, bigBaseBlockCore24, seriesBlockCore24, encircleBlockCore24, sceneryBlockCore24, 354, 355, null, null, EMapAreaAreaDirection.South, lovingItemSubTypes24, list2, showDarkAshStatus: true));
		List<MapAreaItem> dataArray25 = _dataArray;
		sbyte[] worldMapPos25 = new sbyte[2] { 13, 9 };
		AreaTravelRoute[] neighborAreas25 = new AreaTravelRoute[0];
		string[] miniMapIcons25 = new string[4] { "largemap_part_2_icon_hei_1", "largemap_part_2_icon_jin_1", "largemap_part_2_icon_lan_1", "largemap_part_2_icon_hui_1" };
		short[] miniMapPos25 = new short[2] { 240, 230 };
		List<EnemyNestCreationInfo[]> enemyNests25 = new List<EnemyNestCreationInfo[]>
		{
			new EnemyNestCreationInfo[2]
			{
				new EnemyNestCreationInfo(0, 2),
				new EnemyNestCreationInfo(1, 2)
			},
			new EnemyNestCreationInfo[2]
			{
				new EnemyNestCreationInfo(1, 2),
				new EnemyNestCreationInfo(2, 3)
			},
			new EnemyNestCreationInfo[4]
			{
				new EnemyNestCreationInfo(2, 3),
				new EnemyNestCreationInfo(3, 3),
				new EnemyNestCreationInfo(4, 4),
				new EnemyNestCreationInfo(5, 4)
			},
			new EnemyNestCreationInfo[1]
			{
				new EnemyNestCreationInfo(12, 2)
			}
		};
		List<IntPair> enemyNestCreationDateRanges25 = new List<IntPair>
		{
			new IntPair(0, 6),
			new IntPair(6, 12),
			new IntPair(12, 24),
			new IntPair(0, 0)
		};
		short[] settlementBlockCore25 = new short[1] { 35 };
		sbyte[] organizationId25 = new sbyte[1] { 37 };
		List<short[]> developedBlockCore25 = new List<short[]>
		{
			new short[2] { 39, 33 },
			new short[2] { 40, 33 },
			new short[2] { 41, 33 },
			new short[2] { 42, 33 },
			new short[2] { 43, 33 },
			new short[2] { 44, 33 },
			new short[2] { 45, 33 },
			new short[2] { 46, 33 },
			new short[2] { 47, 33 },
			new short[2] { 48, 33 },
			new short[2] { 49, 33 },
			new short[2] { 50, 33 },
			new short[2] { 51, 33 },
			new short[2] { 52, 33 },
			new short[2] { 53, 33 },
			new short[2] { 54, 33 },
			new short[2] { 55, 33 },
			new short[2] { 56, 33 },
			new short[2] { 109, 10 },
			new short[2] { 110, 10 },
			new short[2] { 111, 10 }
		};
		List<short[]> normalBlockCore25 = new List<short[]>
		{
			new short[2] { 57, 33 },
			new short[2] { 58, 33 },
			new short[2] { 59, 33 },
			new short[2] { 63, 33 },
			new short[2] { 64, 33 },
			new short[2] { 65, 33 },
			new short[2] { 69, 33 },
			new short[2] { 70, 33 },
			new short[2] { 71, 33 },
			new short[2] { 75, 33 },
			new short[2] { 76, 33 },
			new short[2] { 77, 33 },
			new short[2] { 81, 33 },
			new short[2] { 82, 33 },
			new short[2] { 83, 33 },
			new short[2] { 87, 33 },
			new short[2] { 88, 33 },
			new short[2] { 89, 33 },
			new short[2] { 112, 30 },
			new short[2] { 113, 30 },
			new short[2] { 114, 30 },
			new short[2] { 124, 30 }
		};
		List<short[]> wildBlockCore25 = new List<short[]>
		{
			new short[2] { 93, 33 },
			new short[2] { 94, 33 },
			new short[2] { 95, 33 },
			new short[2] { 96, 33 },
			new short[2] { 97, 33 },
			new short[2] { 98, 33 },
			new short[2] { 99, 33 },
			new short[2] { 100, 33 },
			new short[2] { 101, 33 },
			new short[2] { 102, 33 },
			new short[2] { 103, 33 },
			new short[2] { 104, 33 },
			new short[2] { 105, 33 },
			new short[2] { 106, 33 },
			new short[2] { 107, 33 },
			new short[2] { 108, 33 },
			new short[2] { 115, 40 },
			new short[2] { 116, 40 },
			new short[2] { 117, 40 },
			new short[2] { 124, 30 }
		};
		List<short[]> bigBaseBlockCore25 = new List<short[]>
		{
			new short[2] { 60, 1 },
			new short[2] { 79, 1 },
			new short[2] { 80, 1 },
			new short[2] { 84, 1 },
			new short[2] { 90, 1 }
		};
		list = new List<short[]>();
		List<short[]> seriesBlockCore25 = list;
		list = new List<short[]>();
		List<short[]> encircleBlockCore25 = list;
		short[] sceneryBlockCore25 = new short[0];
		list2 = new List<short>();
		List<short> lovingItemSubTypes25 = list2;
		list2 = new List<short>();
		dataArray25.Add(new MapAreaItem(84, 8, 356, 357, 20, worldMapPos25, neighborAreas25, "largemap_part_1_changgui_1", miniMapIcons25, null, miniMapPos25, 0, 0, 5, enemyNests25, enemyNestCreationDateRanges25, settlementBlockCore25, organizationId25, -1, 0, null, developedBlockCore25, normalBlockCore25, wildBlockCore25, bigBaseBlockCore25, seriesBlockCore25, encircleBlockCore25, sceneryBlockCore25, 358, 359, null, null, EMapAreaAreaDirection.South, lovingItemSubTypes25, list2, showDarkAshStatus: true));
		List<MapAreaItem> dataArray26 = _dataArray;
		sbyte[] worldMapPos26 = new sbyte[2] { 13, 5 };
		AreaTravelRoute[] neighborAreas26 = new AreaTravelRoute[0];
		string[] miniMapIcons26 = new string[4] { "largemap_part_2_icon_hei_0", "largemap_part_2_icon_jin_0", "largemap_part_2_icon_lan_0", "largemap_part_2_icon_hui_0" };
		short[] miniMapPos26 = new short[2] { 240, 70 };
		List<EnemyNestCreationInfo[]> enemyNests26 = new List<EnemyNestCreationInfo[]>
		{
			new EnemyNestCreationInfo[2]
			{
				new EnemyNestCreationInfo(0, 2),
				new EnemyNestCreationInfo(1, 2)
			},
			new EnemyNestCreationInfo[2]
			{
				new EnemyNestCreationInfo(1, 2),
				new EnemyNestCreationInfo(2, 3)
			},
			new EnemyNestCreationInfo[4]
			{
				new EnemyNestCreationInfo(2, 3),
				new EnemyNestCreationInfo(3, 3),
				new EnemyNestCreationInfo(4, 4),
				new EnemyNestCreationInfo(5, 4)
			},
			new EnemyNestCreationInfo[1]
			{
				new EnemyNestCreationInfo(12, 2)
			}
		};
		List<IntPair> enemyNestCreationDateRanges26 = new List<IntPair>
		{
			new IntPair(0, 6),
			new IntPair(6, 12),
			new IntPair(12, 24),
			new IntPair(0, 0)
		};
		short[] settlementBlockCore26 = new short[1] { 36 };
		sbyte[] organizationId26 = new sbyte[1] { 38 };
		List<short[]> developedBlockCore26 = new List<short[]>
		{
			new short[2] { 39, 33 },
			new short[2] { 40, 33 },
			new short[2] { 41, 33 },
			new short[2] { 42, 33 },
			new short[2] { 43, 33 },
			new short[2] { 44, 33 },
			new short[2] { 45, 33 },
			new short[2] { 46, 33 },
			new short[2] { 47, 33 },
			new short[2] { 48, 33 },
			new short[2] { 49, 33 },
			new short[2] { 50, 33 },
			new short[2] { 51, 33 },
			new short[2] { 52, 33 },
			new short[2] { 53, 33 },
			new short[2] { 54, 33 },
			new short[2] { 55, 33 },
			new short[2] { 56, 33 },
			new short[2] { 109, 10 },
			new short[2] { 110, 10 },
			new short[2] { 111, 10 }
		};
		List<short[]> normalBlockCore26 = new List<short[]>
		{
			new short[2] { 57, 33 },
			new short[2] { 58, 33 },
			new short[2] { 59, 33 },
			new short[2] { 63, 33 },
			new short[2] { 64, 33 },
			new short[2] { 65, 33 },
			new short[2] { 69, 33 },
			new short[2] { 70, 33 },
			new short[2] { 71, 33 },
			new short[2] { 75, 33 },
			new short[2] { 76, 33 },
			new short[2] { 77, 33 },
			new short[2] { 81, 33 },
			new short[2] { 82, 33 },
			new short[2] { 83, 33 },
			new short[2] { 87, 33 },
			new short[2] { 88, 33 },
			new short[2] { 89, 33 },
			new short[2] { 112, 30 },
			new short[2] { 113, 30 },
			new short[2] { 114, 30 },
			new short[2] { 124, 30 }
		};
		List<short[]> wildBlockCore26 = new List<short[]>
		{
			new short[2] { 93, 33 },
			new short[2] { 94, 33 },
			new short[2] { 95, 33 },
			new short[2] { 96, 33 },
			new short[2] { 97, 33 },
			new short[2] { 98, 33 },
			new short[2] { 99, 33 },
			new short[2] { 100, 33 },
			new short[2] { 101, 33 },
			new short[2] { 102, 33 },
			new short[2] { 103, 33 },
			new short[2] { 104, 33 },
			new short[2] { 105, 33 },
			new short[2] { 106, 33 },
			new short[2] { 107, 33 },
			new short[2] { 108, 33 },
			new short[2] { 115, 40 },
			new short[2] { 116, 40 },
			new short[2] { 117, 40 },
			new short[2] { 124, 30 }
		};
		List<short[]> bigBaseBlockCore26 = new List<short[]>
		{
			new short[2] { 60, 1 },
			new short[2] { 79, 1 },
			new short[2] { 80, 1 },
			new short[2] { 84, 1 },
			new short[2] { 90, 1 }
		};
		list = new List<short[]>();
		List<short[]> seriesBlockCore26 = list;
		list = new List<short[]>();
		List<short[]> encircleBlockCore26 = list;
		short[] sceneryBlockCore26 = new short[0];
		list2 = new List<short>();
		List<short> lovingItemSubTypes26 = list2;
		list2 = new List<short>();
		dataArray26.Add(new MapAreaItem(85, 8, 360, 361, 20, worldMapPos26, neighborAreas26, "largemap_part_1_changgui_0", miniMapIcons26, null, miniMapPos26, 0, 0, 5, enemyNests26, enemyNestCreationDateRanges26, settlementBlockCore26, organizationId26, -1, 0, null, developedBlockCore26, normalBlockCore26, wildBlockCore26, bigBaseBlockCore26, seriesBlockCore26, encircleBlockCore26, sceneryBlockCore26, 362, 363, null, null, EMapAreaAreaDirection.South, lovingItemSubTypes26, list2, showDarkAshStatus: true));
		List<MapAreaItem> dataArray27 = _dataArray;
		sbyte[] worldMapPos27 = new sbyte[2] { 10, 8 };
		AreaTravelRoute[] neighborAreas27 = new AreaTravelRoute[0];
		string[] miniMapIcons27 = new string[4] { "largemap_part_2_icon_hei_0", "largemap_part_2_icon_jin_0", "largemap_part_2_icon_lan_0", "largemap_part_2_icon_hui_0" };
		short[] miniMapPos27 = new short[2] { 120, 190 };
		List<EnemyNestCreationInfo[]> enemyNests27 = new List<EnemyNestCreationInfo[]>
		{
			new EnemyNestCreationInfo[2]
			{
				new EnemyNestCreationInfo(0, 2),
				new EnemyNestCreationInfo(1, 2)
			},
			new EnemyNestCreationInfo[2]
			{
				new EnemyNestCreationInfo(1, 2),
				new EnemyNestCreationInfo(2, 3)
			},
			new EnemyNestCreationInfo[4]
			{
				new EnemyNestCreationInfo(2, 3),
				new EnemyNestCreationInfo(3, 3),
				new EnemyNestCreationInfo(4, 4),
				new EnemyNestCreationInfo(5, 4)
			},
			new EnemyNestCreationInfo[1]
			{
				new EnemyNestCreationInfo(12, 2)
			}
		};
		List<IntPair> enemyNestCreationDateRanges27 = new List<IntPair>
		{
			new IntPair(0, 6),
			new IntPair(6, 12),
			new IntPair(12, 24),
			new IntPair(0, 0)
		};
		short[] settlementBlockCore27 = new short[1] { 34 };
		sbyte[] organizationId27 = new sbyte[1] { 36 };
		List<short[]> developedBlockCore27 = new List<short[]>
		{
			new short[2] { 39, 33 },
			new short[2] { 40, 33 },
			new short[2] { 41, 33 },
			new short[2] { 42, 33 },
			new short[2] { 43, 33 },
			new short[2] { 44, 33 },
			new short[2] { 45, 33 },
			new short[2] { 46, 33 },
			new short[2] { 47, 33 },
			new short[2] { 48, 33 },
			new short[2] { 49, 33 },
			new short[2] { 50, 33 },
			new short[2] { 51, 33 },
			new short[2] { 52, 33 },
			new short[2] { 53, 33 },
			new short[2] { 54, 33 },
			new short[2] { 55, 33 },
			new short[2] { 56, 33 },
			new short[2] { 109, 10 },
			new short[2] { 110, 10 },
			new short[2] { 111, 10 }
		};
		List<short[]> normalBlockCore27 = new List<short[]>
		{
			new short[2] { 57, 33 },
			new short[2] { 58, 33 },
			new short[2] { 59, 33 },
			new short[2] { 63, 33 },
			new short[2] { 64, 33 },
			new short[2] { 65, 33 },
			new short[2] { 69, 33 },
			new short[2] { 70, 33 },
			new short[2] { 71, 33 },
			new short[2] { 75, 33 },
			new short[2] { 76, 33 },
			new short[2] { 77, 33 },
			new short[2] { 81, 33 },
			new short[2] { 82, 33 },
			new short[2] { 83, 33 },
			new short[2] { 87, 33 },
			new short[2] { 88, 33 },
			new short[2] { 89, 33 },
			new short[2] { 112, 30 },
			new short[2] { 113, 30 },
			new short[2] { 114, 30 },
			new short[2] { 124, 30 }
		};
		List<short[]> wildBlockCore27 = new List<short[]>
		{
			new short[2] { 93, 33 },
			new short[2] { 94, 33 },
			new short[2] { 95, 33 },
			new short[2] { 96, 33 },
			new short[2] { 97, 33 },
			new short[2] { 98, 33 },
			new short[2] { 99, 33 },
			new short[2] { 100, 33 },
			new short[2] { 101, 33 },
			new short[2] { 102, 33 },
			new short[2] { 103, 33 },
			new short[2] { 104, 33 },
			new short[2] { 105, 33 },
			new short[2] { 106, 33 },
			new short[2] { 107, 33 },
			new short[2] { 108, 33 },
			new short[2] { 115, 40 },
			new short[2] { 116, 40 },
			new short[2] { 117, 40 },
			new short[2] { 124, 30 }
		};
		List<short[]> bigBaseBlockCore27 = new List<short[]>
		{
			new short[2] { 60, 1 },
			new short[2] { 79, 1 },
			new short[2] { 80, 1 },
			new short[2] { 84, 1 },
			new short[2] { 90, 1 }
		};
		list = new List<short[]>();
		List<short[]> seriesBlockCore27 = list;
		list = new List<short[]>();
		List<short[]> encircleBlockCore27 = list;
		short[] sceneryBlockCore27 = new short[0];
		list2 = new List<short>();
		List<short> lovingItemSubTypes27 = list2;
		list2 = new List<short>();
		dataArray27.Add(new MapAreaItem(86, 8, 364, 365, 20, worldMapPos27, neighborAreas27, "largemap_part_1_changgui_0", miniMapIcons27, null, miniMapPos27, 0, 0, 5, enemyNests27, enemyNestCreationDateRanges27, settlementBlockCore27, organizationId27, -1, 0, null, developedBlockCore27, normalBlockCore27, wildBlockCore27, bigBaseBlockCore27, seriesBlockCore27, encircleBlockCore27, sceneryBlockCore27, 366, 367, null, null, EMapAreaAreaDirection.South, lovingItemSubTypes27, list2, showDarkAshStatus: true));
		List<MapAreaItem> dataArray28 = _dataArray;
		sbyte[] worldMapPos28 = new sbyte[2] { 20, 4 };
		AreaTravelRoute[] neighborAreas28 = new AreaTravelRoute[1]
		{
			new AreaTravelRoute(88, 12, new int[2] { 19, 4 })
		};
		string[] miniMapIcons28 = new string[4] { "largemap_part_2_icon_hei_2", "largemap_part_2_icon_jin_2", "largemap_part_2_icon_lan_2", "largemap_part_2_icon_hui_2" };
		short[] miniMapPos28 = new short[2] { 130, 150 };
		List<EnemyNestCreationInfo[]> enemyNests28 = new List<EnemyNestCreationInfo[]>
		{
			new EnemyNestCreationInfo[2]
			{
				new EnemyNestCreationInfo(0, 2),
				new EnemyNestCreationInfo(1, 2)
			},
			new EnemyNestCreationInfo[2]
			{
				new EnemyNestCreationInfo(1, 2),
				new EnemyNestCreationInfo(2, 3)
			},
			new EnemyNestCreationInfo[4]
			{
				new EnemyNestCreationInfo(2, 3),
				new EnemyNestCreationInfo(3, 3),
				new EnemyNestCreationInfo(4, 4),
				new EnemyNestCreationInfo(5, 4)
			},
			new EnemyNestCreationInfo[1]
			{
				new EnemyNestCreationInfo(12, 2)
			}
		};
		List<IntPair> enemyNestCreationDateRanges28 = new List<IntPair>
		{
			new IntPair(0, 6),
			new IntPair(6, 12),
			new IntPair(12, 24),
			new IntPair(0, 0)
		};
		short[] settlementBlockCore28 = new short[1] { 35 };
		sbyte[] organizationId28 = new sbyte[1] { 37 };
		List<short[]> developedBlockCore28 = new List<short[]>
		{
			new short[2] { 39, 33 },
			new short[2] { 40, 33 },
			new short[2] { 41, 33 },
			new short[2] { 42, 33 },
			new short[2] { 43, 33 },
			new short[2] { 44, 33 },
			new short[2] { 45, 33 },
			new short[2] { 46, 33 },
			new short[2] { 47, 33 },
			new short[2] { 48, 33 },
			new short[2] { 49, 33 },
			new short[2] { 50, 33 },
			new short[2] { 51, 33 },
			new short[2] { 52, 33 },
			new short[2] { 53, 33 },
			new short[2] { 54, 33 },
			new short[2] { 55, 33 },
			new short[2] { 56, 33 },
			new short[2] { 109, 10 },
			new short[2] { 110, 10 },
			new short[2] { 111, 10 }
		};
		List<short[]> normalBlockCore28 = new List<short[]>
		{
			new short[2] { 57, 33 },
			new short[2] { 58, 33 },
			new short[2] { 59, 33 },
			new short[2] { 63, 33 },
			new short[2] { 64, 33 },
			new short[2] { 65, 33 },
			new short[2] { 69, 33 },
			new short[2] { 70, 33 },
			new short[2] { 71, 33 },
			new short[2] { 75, 33 },
			new short[2] { 76, 33 },
			new short[2] { 77, 33 },
			new short[2] { 81, 33 },
			new short[2] { 82, 33 },
			new short[2] { 83, 33 },
			new short[2] { 87, 33 },
			new short[2] { 88, 33 },
			new short[2] { 89, 33 },
			new short[2] { 112, 30 },
			new short[2] { 113, 30 },
			new short[2] { 114, 30 },
			new short[2] { 124, 30 }
		};
		List<short[]> wildBlockCore28 = new List<short[]>
		{
			new short[2] { 93, 33 },
			new short[2] { 94, 33 },
			new short[2] { 95, 33 },
			new short[2] { 96, 33 },
			new short[2] { 97, 33 },
			new short[2] { 98, 33 },
			new short[2] { 99, 33 },
			new short[2] { 100, 33 },
			new short[2] { 101, 33 },
			new short[2] { 102, 33 },
			new short[2] { 103, 33 },
			new short[2] { 104, 33 },
			new short[2] { 105, 33 },
			new short[2] { 106, 33 },
			new short[2] { 107, 33 },
			new short[2] { 108, 33 },
			new short[2] { 115, 40 },
			new short[2] { 116, 40 },
			new short[2] { 117, 40 },
			new short[2] { 124, 30 }
		};
		List<short[]> bigBaseBlockCore28 = new List<short[]>
		{
			new short[2] { 60, 1 },
			new short[2] { 78, 1 },
			new short[2] { 84, 1 },
			new short[2] { 91, 1 },
			new short[2] { 92, 1 }
		};
		list = new List<short[]>();
		List<short[]> seriesBlockCore28 = list;
		list = new List<short[]>();
		List<short[]> encircleBlockCore28 = list;
		short[] sceneryBlockCore28 = new short[0];
		list2 = new List<short>();
		List<short> lovingItemSubTypes28 = list2;
		list2 = new List<short>();
		dataArray28.Add(new MapAreaItem(87, 9, 368, 369, 20, worldMapPos28, neighborAreas28, "largemap_part_1_changgui_2", miniMapIcons28, null, miniMapPos28, 0, 0, 5, enemyNests28, enemyNestCreationDateRanges28, settlementBlockCore28, organizationId28, -1, 0, null, developedBlockCore28, normalBlockCore28, wildBlockCore28, bigBaseBlockCore28, seriesBlockCore28, encircleBlockCore28, sceneryBlockCore28, 370, 371, null, null, EMapAreaAreaDirection.South, lovingItemSubTypes28, list2, showDarkAshStatus: true));
		List<MapAreaItem> dataArray29 = _dataArray;
		sbyte[] worldMapPos29 = new sbyte[2] { 19, 3 };
		AreaTravelRoute[] neighborAreas29 = new AreaTravelRoute[0];
		string[] miniMapIcons29 = new string[4] { "largemap_part_2_icon_hei_2", "largemap_part_2_icon_jin_2", "largemap_part_2_icon_lan_2", "largemap_part_2_icon_hui_2" };
		short[] miniMapPos29 = new short[2] { 90, 110 };
		List<EnemyNestCreationInfo[]> enemyNests29 = new List<EnemyNestCreationInfo[]>
		{
			new EnemyNestCreationInfo[2]
			{
				new EnemyNestCreationInfo(0, 2),
				new EnemyNestCreationInfo(1, 2)
			},
			new EnemyNestCreationInfo[2]
			{
				new EnemyNestCreationInfo(1, 2),
				new EnemyNestCreationInfo(2, 3)
			},
			new EnemyNestCreationInfo[4]
			{
				new EnemyNestCreationInfo(2, 3),
				new EnemyNestCreationInfo(3, 3),
				new EnemyNestCreationInfo(4, 4),
				new EnemyNestCreationInfo(5, 4)
			},
			new EnemyNestCreationInfo[1]
			{
				new EnemyNestCreationInfo(12, 2)
			}
		};
		List<IntPair> enemyNestCreationDateRanges29 = new List<IntPair>
		{
			new IntPair(0, 6),
			new IntPair(6, 12),
			new IntPair(12, 24),
			new IntPair(0, 0)
		};
		short[] settlementBlockCore29 = new short[1] { 34 };
		sbyte[] organizationId29 = new sbyte[1] { 36 };
		List<short[]> developedBlockCore29 = new List<short[]>
		{
			new short[2] { 39, 33 },
			new short[2] { 40, 33 },
			new short[2] { 41, 33 },
			new short[2] { 42, 33 },
			new short[2] { 43, 33 },
			new short[2] { 44, 33 },
			new short[2] { 45, 33 },
			new short[2] { 46, 33 },
			new short[2] { 47, 33 },
			new short[2] { 48, 33 },
			new short[2] { 49, 33 },
			new short[2] { 50, 33 },
			new short[2] { 51, 33 },
			new short[2] { 52, 33 },
			new short[2] { 53, 33 },
			new short[2] { 54, 33 },
			new short[2] { 55, 33 },
			new short[2] { 56, 33 },
			new short[2] { 109, 10 },
			new short[2] { 110, 10 },
			new short[2] { 111, 10 }
		};
		List<short[]> normalBlockCore29 = new List<short[]>
		{
			new short[2] { 57, 33 },
			new short[2] { 58, 33 },
			new short[2] { 59, 33 },
			new short[2] { 63, 33 },
			new short[2] { 64, 33 },
			new short[2] { 65, 33 },
			new short[2] { 69, 33 },
			new short[2] { 70, 33 },
			new short[2] { 71, 33 },
			new short[2] { 75, 33 },
			new short[2] { 76, 33 },
			new short[2] { 77, 33 },
			new short[2] { 81, 33 },
			new short[2] { 82, 33 },
			new short[2] { 83, 33 },
			new short[2] { 87, 33 },
			new short[2] { 88, 33 },
			new short[2] { 89, 33 },
			new short[2] { 112, 30 },
			new short[2] { 113, 30 },
			new short[2] { 114, 30 },
			new short[2] { 124, 30 }
		};
		List<short[]> wildBlockCore29 = new List<short[]>
		{
			new short[2] { 93, 33 },
			new short[2] { 94, 33 },
			new short[2] { 95, 33 },
			new short[2] { 96, 33 },
			new short[2] { 97, 33 },
			new short[2] { 98, 33 },
			new short[2] { 99, 33 },
			new short[2] { 100, 33 },
			new short[2] { 101, 33 },
			new short[2] { 102, 33 },
			new short[2] { 103, 33 },
			new short[2] { 104, 33 },
			new short[2] { 105, 33 },
			new short[2] { 106, 33 },
			new short[2] { 107, 33 },
			new short[2] { 108, 33 },
			new short[2] { 115, 40 },
			new short[2] { 116, 40 },
			new short[2] { 117, 40 },
			new short[2] { 124, 30 }
		};
		List<short[]> bigBaseBlockCore29 = new List<short[]>
		{
			new short[2] { 60, 1 },
			new short[2] { 78, 1 },
			new short[2] { 84, 1 },
			new short[2] { 91, 1 },
			new short[2] { 92, 1 }
		};
		list = new List<short[]>();
		List<short[]> seriesBlockCore29 = list;
		list = new List<short[]>();
		List<short[]> encircleBlockCore29 = list;
		short[] sceneryBlockCore29 = new short[0];
		list2 = new List<short>();
		List<short> lovingItemSubTypes29 = list2;
		list2 = new List<short>();
		dataArray29.Add(new MapAreaItem(88, 9, 372, 373, 20, worldMapPos29, neighborAreas29, "largemap_part_1_changgui_2", miniMapIcons29, null, miniMapPos29, 0, 0, 5, enemyNests29, enemyNestCreationDateRanges29, settlementBlockCore29, organizationId29, -1, 0, null, developedBlockCore29, normalBlockCore29, wildBlockCore29, bigBaseBlockCore29, seriesBlockCore29, encircleBlockCore29, sceneryBlockCore29, 374, 375, null, null, EMapAreaAreaDirection.South, lovingItemSubTypes29, list2, showDarkAshStatus: true));
		List<MapAreaItem> dataArray30 = _dataArray;
		sbyte[] worldMapPos30 = new sbyte[2] { 21, 6 };
		AreaTravelRoute[] neighborAreas30 = new AreaTravelRoute[1]
		{
			new AreaTravelRoute(93, 6, null)
		};
		string[] miniMapIcons30 = new string[4] { "largemap_part_2_icon_hei_2", "largemap_part_2_icon_jin_2", "largemap_part_2_icon_lan_2", "largemap_part_2_icon_hui_2" };
		short[] miniMapPos30 = new short[2] { 170, 230 };
		List<EnemyNestCreationInfo[]> enemyNests30 = new List<EnemyNestCreationInfo[]>
		{
			new EnemyNestCreationInfo[2]
			{
				new EnemyNestCreationInfo(0, 2),
				new EnemyNestCreationInfo(1, 2)
			},
			new EnemyNestCreationInfo[2]
			{
				new EnemyNestCreationInfo(1, 2),
				new EnemyNestCreationInfo(2, 3)
			},
			new EnemyNestCreationInfo[4]
			{
				new EnemyNestCreationInfo(2, 3),
				new EnemyNestCreationInfo(3, 3),
				new EnemyNestCreationInfo(4, 4),
				new EnemyNestCreationInfo(5, 4)
			},
			new EnemyNestCreationInfo[1]
			{
				new EnemyNestCreationInfo(12, 2)
			}
		};
		List<IntPair> enemyNestCreationDateRanges30 = new List<IntPair>
		{
			new IntPair(0, 6),
			new IntPair(6, 12),
			new IntPair(12, 24),
			new IntPair(0, 0)
		};
		short[] settlementBlockCore30 = new short[1] { 35 };
		sbyte[] organizationId30 = new sbyte[1] { 37 };
		List<short[]> developedBlockCore30 = new List<short[]>
		{
			new short[2] { 39, 33 },
			new short[2] { 40, 33 },
			new short[2] { 41, 33 },
			new short[2] { 42, 33 },
			new short[2] { 43, 33 },
			new short[2] { 44, 33 },
			new short[2] { 45, 33 },
			new short[2] { 46, 33 },
			new short[2] { 47, 33 },
			new short[2] { 48, 33 },
			new short[2] { 49, 33 },
			new short[2] { 50, 33 },
			new short[2] { 51, 33 },
			new short[2] { 52, 33 },
			new short[2] { 53, 33 },
			new short[2] { 54, 33 },
			new short[2] { 55, 33 },
			new short[2] { 56, 33 },
			new short[2] { 109, 10 },
			new short[2] { 110, 10 },
			new short[2] { 111, 10 }
		};
		List<short[]> normalBlockCore30 = new List<short[]>
		{
			new short[2] { 57, 33 },
			new short[2] { 58, 33 },
			new short[2] { 59, 33 },
			new short[2] { 63, 33 },
			new short[2] { 64, 33 },
			new short[2] { 65, 33 },
			new short[2] { 69, 33 },
			new short[2] { 70, 33 },
			new short[2] { 71, 33 },
			new short[2] { 75, 33 },
			new short[2] { 76, 33 },
			new short[2] { 77, 33 },
			new short[2] { 81, 33 },
			new short[2] { 82, 33 },
			new short[2] { 83, 33 },
			new short[2] { 87, 33 },
			new short[2] { 88, 33 },
			new short[2] { 89, 33 },
			new short[2] { 112, 30 },
			new short[2] { 113, 30 },
			new short[2] { 114, 30 },
			new short[2] { 124, 30 }
		};
		List<short[]> wildBlockCore30 = new List<short[]>
		{
			new short[2] { 93, 33 },
			new short[2] { 94, 33 },
			new short[2] { 95, 33 },
			new short[2] { 96, 33 },
			new short[2] { 97, 33 },
			new short[2] { 98, 33 },
			new short[2] { 99, 33 },
			new short[2] { 100, 33 },
			new short[2] { 101, 33 },
			new short[2] { 102, 33 },
			new short[2] { 103, 33 },
			new short[2] { 104, 33 },
			new short[2] { 105, 33 },
			new short[2] { 106, 33 },
			new short[2] { 107, 33 },
			new short[2] { 108, 33 },
			new short[2] { 115, 40 },
			new short[2] { 116, 40 },
			new short[2] { 117, 40 },
			new short[2] { 124, 30 }
		};
		List<short[]> bigBaseBlockCore30 = new List<short[]>
		{
			new short[2] { 60, 1 },
			new short[2] { 78, 1 },
			new short[2] { 84, 1 },
			new short[2] { 91, 1 },
			new short[2] { 92, 1 }
		};
		list = new List<short[]>();
		List<short[]> seriesBlockCore30 = list;
		list = new List<short[]>();
		List<short[]> encircleBlockCore30 = list;
		short[] sceneryBlockCore30 = new short[0];
		list2 = new List<short>();
		List<short> lovingItemSubTypes30 = list2;
		list2 = new List<short>();
		dataArray30.Add(new MapAreaItem(89, 9, 376, 377, 20, worldMapPos30, neighborAreas30, "largemap_part_1_changgui_2", miniMapIcons30, null, miniMapPos30, 0, 0, 5, enemyNests30, enemyNestCreationDateRanges30, settlementBlockCore30, organizationId30, -1, 0, null, developedBlockCore30, normalBlockCore30, wildBlockCore30, bigBaseBlockCore30, seriesBlockCore30, encircleBlockCore30, sceneryBlockCore30, 378, 379, null, null, EMapAreaAreaDirection.South, lovingItemSubTypes30, list2, showDarkAshStatus: true));
		List<MapAreaItem> dataArray31 = _dataArray;
		sbyte[] worldMapPos31 = new sbyte[2] { 19, 6 };
		AreaTravelRoute[] neighborAreas31 = new AreaTravelRoute[0];
		string[] miniMapIcons31 = new string[4] { "largemap_part_2_icon_hei_0", "largemap_part_2_icon_jin_0", "largemap_part_2_icon_lan_0", "largemap_part_2_icon_hui_0" };
		short[] miniMapPos31 = new short[2] { 90, 230 };
		List<EnemyNestCreationInfo[]> enemyNests31 = new List<EnemyNestCreationInfo[]>
		{
			new EnemyNestCreationInfo[2]
			{
				new EnemyNestCreationInfo(0, 2),
				new EnemyNestCreationInfo(1, 2)
			},
			new EnemyNestCreationInfo[2]
			{
				new EnemyNestCreationInfo(1, 2),
				new EnemyNestCreationInfo(2, 3)
			},
			new EnemyNestCreationInfo[4]
			{
				new EnemyNestCreationInfo(2, 3),
				new EnemyNestCreationInfo(3, 3),
				new EnemyNestCreationInfo(4, 4),
				new EnemyNestCreationInfo(5, 4)
			},
			new EnemyNestCreationInfo[1]
			{
				new EnemyNestCreationInfo(12, 2)
			}
		};
		List<IntPair> enemyNestCreationDateRanges31 = new List<IntPair>
		{
			new IntPair(0, 6),
			new IntPair(6, 12),
			new IntPair(12, 24),
			new IntPair(0, 0)
		};
		short[] settlementBlockCore31 = new short[1] { 34 };
		sbyte[] organizationId31 = new sbyte[1] { 36 };
		List<short[]> developedBlockCore31 = new List<short[]>
		{
			new short[2] { 39, 33 },
			new short[2] { 40, 33 },
			new short[2] { 41, 33 },
			new short[2] { 42, 33 },
			new short[2] { 43, 33 },
			new short[2] { 44, 33 },
			new short[2] { 45, 33 },
			new short[2] { 46, 33 },
			new short[2] { 47, 33 },
			new short[2] { 48, 33 },
			new short[2] { 49, 33 },
			new short[2] { 50, 33 },
			new short[2] { 51, 33 },
			new short[2] { 52, 33 },
			new short[2] { 53, 33 },
			new short[2] { 54, 33 },
			new short[2] { 55, 33 },
			new short[2] { 56, 33 },
			new short[2] { 109, 10 },
			new short[2] { 110, 10 },
			new short[2] { 111, 10 }
		};
		List<short[]> normalBlockCore31 = new List<short[]>
		{
			new short[2] { 57, 33 },
			new short[2] { 58, 33 },
			new short[2] { 59, 33 },
			new short[2] { 63, 33 },
			new short[2] { 64, 33 },
			new short[2] { 65, 33 },
			new short[2] { 69, 33 },
			new short[2] { 70, 33 },
			new short[2] { 71, 33 },
			new short[2] { 75, 33 },
			new short[2] { 76, 33 },
			new short[2] { 77, 33 },
			new short[2] { 81, 33 },
			new short[2] { 82, 33 },
			new short[2] { 83, 33 },
			new short[2] { 87, 33 },
			new short[2] { 88, 33 },
			new short[2] { 89, 33 },
			new short[2] { 112, 30 },
			new short[2] { 113, 30 },
			new short[2] { 114, 30 },
			new short[2] { 124, 30 }
		};
		List<short[]> wildBlockCore31 = new List<short[]>
		{
			new short[2] { 93, 33 },
			new short[2] { 94, 33 },
			new short[2] { 95, 33 },
			new short[2] { 96, 33 },
			new short[2] { 97, 33 },
			new short[2] { 98, 33 },
			new short[2] { 99, 33 },
			new short[2] { 100, 33 },
			new short[2] { 101, 33 },
			new short[2] { 102, 33 },
			new short[2] { 103, 33 },
			new short[2] { 104, 33 },
			new short[2] { 105, 33 },
			new short[2] { 106, 33 },
			new short[2] { 107, 33 },
			new short[2] { 108, 33 },
			new short[2] { 115, 40 },
			new short[2] { 116, 40 },
			new short[2] { 117, 40 },
			new short[2] { 124, 30 }
		};
		List<short[]> bigBaseBlockCore31 = new List<short[]>
		{
			new short[2] { 60, 1 },
			new short[2] { 78, 1 },
			new short[2] { 84, 1 },
			new short[2] { 91, 1 },
			new short[2] { 92, 1 }
		};
		list = new List<short[]>();
		List<short[]> seriesBlockCore31 = list;
		list = new List<short[]>();
		List<short[]> encircleBlockCore31 = list;
		short[] sceneryBlockCore31 = new short[0];
		list2 = new List<short>();
		List<short> lovingItemSubTypes31 = list2;
		list2 = new List<short>();
		dataArray31.Add(new MapAreaItem(90, 9, 380, 381, 20, worldMapPos31, neighborAreas31, "largemap_part_1_changgui_0", miniMapIcons31, null, miniMapPos31, 0, 0, 5, enemyNests31, enemyNestCreationDateRanges31, settlementBlockCore31, organizationId31, -1, 0, null, developedBlockCore31, normalBlockCore31, wildBlockCore31, bigBaseBlockCore31, seriesBlockCore31, encircleBlockCore31, sceneryBlockCore31, 382, 383, null, null, EMapAreaAreaDirection.South, lovingItemSubTypes31, list2, showDarkAshStatus: true));
		List<MapAreaItem> dataArray32 = _dataArray;
		sbyte[] worldMapPos32 = new sbyte[2] { 23, 4 };
		AreaTravelRoute[] neighborAreas32 = new AreaTravelRoute[1]
		{
			new AreaTravelRoute(92, 26, new int[4] { 22, 4, 21, 4 })
		};
		string[] miniMapIcons32 = new string[4] { "largemap_part_2_icon_hei_2", "largemap_part_2_icon_jin_2", "largemap_part_2_icon_lan_2", "largemap_part_2_icon_hui_2" };
		short[] miniMapPos32 = new short[2] { 250, 150 };
		List<EnemyNestCreationInfo[]> enemyNests32 = new List<EnemyNestCreationInfo[]>
		{
			new EnemyNestCreationInfo[2]
			{
				new EnemyNestCreationInfo(0, 2),
				new EnemyNestCreationInfo(1, 2)
			},
			new EnemyNestCreationInfo[2]
			{
				new EnemyNestCreationInfo(1, 2),
				new EnemyNestCreationInfo(2, 3)
			},
			new EnemyNestCreationInfo[4]
			{
				new EnemyNestCreationInfo(2, 3),
				new EnemyNestCreationInfo(3, 3),
				new EnemyNestCreationInfo(4, 4),
				new EnemyNestCreationInfo(5, 4)
			},
			new EnemyNestCreationInfo[1]
			{
				new EnemyNestCreationInfo(12, 2)
			}
		};
		List<IntPair> enemyNestCreationDateRanges32 = new List<IntPair>
		{
			new IntPair(0, 6),
			new IntPair(6, 12),
			new IntPair(12, 24),
			new IntPair(0, 0)
		};
		short[] settlementBlockCore32 = new short[1] { 35 };
		sbyte[] organizationId32 = new sbyte[1] { 37 };
		List<short[]> developedBlockCore32 = new List<short[]>
		{
			new short[2] { 39, 33 },
			new short[2] { 40, 33 },
			new short[2] { 41, 33 },
			new short[2] { 42, 33 },
			new short[2] { 43, 33 },
			new short[2] { 44, 33 },
			new short[2] { 45, 33 },
			new short[2] { 46, 33 },
			new short[2] { 47, 33 },
			new short[2] { 48, 33 },
			new short[2] { 49, 33 },
			new short[2] { 50, 33 },
			new short[2] { 51, 33 },
			new short[2] { 52, 33 },
			new short[2] { 53, 33 },
			new short[2] { 54, 33 },
			new short[2] { 55, 33 },
			new short[2] { 56, 33 },
			new short[2] { 109, 10 },
			new short[2] { 110, 10 },
			new short[2] { 111, 10 }
		};
		List<short[]> normalBlockCore32 = new List<short[]>
		{
			new short[2] { 57, 33 },
			new short[2] { 58, 33 },
			new short[2] { 59, 33 },
			new short[2] { 63, 33 },
			new short[2] { 64, 33 },
			new short[2] { 65, 33 },
			new short[2] { 69, 33 },
			new short[2] { 70, 33 },
			new short[2] { 71, 33 },
			new short[2] { 75, 33 },
			new short[2] { 76, 33 },
			new short[2] { 77, 33 },
			new short[2] { 81, 33 },
			new short[2] { 82, 33 },
			new short[2] { 83, 33 },
			new short[2] { 87, 33 },
			new short[2] { 88, 33 },
			new short[2] { 89, 33 },
			new short[2] { 112, 30 },
			new short[2] { 113, 30 },
			new short[2] { 114, 30 },
			new short[2] { 124, 30 }
		};
		List<short[]> wildBlockCore32 = new List<short[]>
		{
			new short[2] { 93, 33 },
			new short[2] { 94, 33 },
			new short[2] { 95, 33 },
			new short[2] { 96, 33 },
			new short[2] { 97, 33 },
			new short[2] { 98, 33 },
			new short[2] { 99, 33 },
			new short[2] { 100, 33 },
			new short[2] { 101, 33 },
			new short[2] { 102, 33 },
			new short[2] { 103, 33 },
			new short[2] { 104, 33 },
			new short[2] { 105, 33 },
			new short[2] { 106, 33 },
			new short[2] { 107, 33 },
			new short[2] { 108, 33 },
			new short[2] { 115, 40 },
			new short[2] { 116, 40 },
			new short[2] { 117, 40 },
			new short[2] { 124, 30 }
		};
		List<short[]> bigBaseBlockCore32 = new List<short[]>
		{
			new short[2] { 60, 1 },
			new short[2] { 78, 1 },
			new short[2] { 84, 1 },
			new short[2] { 91, 1 },
			new short[2] { 92, 1 }
		};
		list = new List<short[]>();
		List<short[]> seriesBlockCore32 = list;
		list = new List<short[]>();
		List<short[]> encircleBlockCore32 = list;
		short[] sceneryBlockCore32 = new short[0];
		list2 = new List<short>();
		List<short> lovingItemSubTypes32 = list2;
		list2 = new List<short>();
		dataArray32.Add(new MapAreaItem(91, 9, 384, 385, 20, worldMapPos32, neighborAreas32, "largemap_part_1_changgui_2", miniMapIcons32, null, miniMapPos32, 0, 0, 5, enemyNests32, enemyNestCreationDateRanges32, settlementBlockCore32, organizationId32, -1, 0, null, developedBlockCore32, normalBlockCore32, wildBlockCore32, bigBaseBlockCore32, seriesBlockCore32, encircleBlockCore32, sceneryBlockCore32, 386, 387, null, null, EMapAreaAreaDirection.South, lovingItemSubTypes32, list2, showDarkAshStatus: true));
		List<MapAreaItem> dataArray33 = _dataArray;
		sbyte[] worldMapPos33 = new sbyte[2] { 21, 3 };
		AreaTravelRoute[] neighborAreas33 = new AreaTravelRoute[0];
		string[] miniMapIcons33 = new string[4] { "largemap_part_2_icon_hei_1", "largemap_part_2_icon_jin_1", "largemap_part_2_icon_lan_1", "largemap_part_2_icon_hui_1" };
		short[] miniMapPos33 = new short[2] { 170, 110 };
		List<EnemyNestCreationInfo[]> enemyNests33 = new List<EnemyNestCreationInfo[]>
		{
			new EnemyNestCreationInfo[2]
			{
				new EnemyNestCreationInfo(0, 2),
				new EnemyNestCreationInfo(1, 2)
			},
			new EnemyNestCreationInfo[2]
			{
				new EnemyNestCreationInfo(1, 2),
				new EnemyNestCreationInfo(2, 3)
			},
			new EnemyNestCreationInfo[4]
			{
				new EnemyNestCreationInfo(2, 3),
				new EnemyNestCreationInfo(3, 3),
				new EnemyNestCreationInfo(4, 4),
				new EnemyNestCreationInfo(5, 4)
			},
			new EnemyNestCreationInfo[1]
			{
				new EnemyNestCreationInfo(12, 2)
			}
		};
		List<IntPair> enemyNestCreationDateRanges33 = new List<IntPair>
		{
			new IntPair(0, 6),
			new IntPair(6, 12),
			new IntPair(12, 24),
			new IntPair(0, 0)
		};
		short[] settlementBlockCore33 = new short[1] { 34 };
		sbyte[] organizationId33 = new sbyte[1] { 36 };
		List<short[]> developedBlockCore33 = new List<short[]>
		{
			new short[2] { 39, 33 },
			new short[2] { 40, 33 },
			new short[2] { 41, 33 },
			new short[2] { 42, 33 },
			new short[2] { 43, 33 },
			new short[2] { 44, 33 },
			new short[2] { 45, 33 },
			new short[2] { 46, 33 },
			new short[2] { 47, 33 },
			new short[2] { 48, 33 },
			new short[2] { 49, 33 },
			new short[2] { 50, 33 },
			new short[2] { 51, 33 },
			new short[2] { 52, 33 },
			new short[2] { 53, 33 },
			new short[2] { 54, 33 },
			new short[2] { 55, 33 },
			new short[2] { 56, 33 },
			new short[2] { 109, 10 },
			new short[2] { 110, 10 },
			new short[2] { 111, 10 }
		};
		List<short[]> normalBlockCore33 = new List<short[]>
		{
			new short[2] { 57, 33 },
			new short[2] { 58, 33 },
			new short[2] { 59, 33 },
			new short[2] { 63, 33 },
			new short[2] { 64, 33 },
			new short[2] { 65, 33 },
			new short[2] { 69, 33 },
			new short[2] { 70, 33 },
			new short[2] { 71, 33 },
			new short[2] { 75, 33 },
			new short[2] { 76, 33 },
			new short[2] { 77, 33 },
			new short[2] { 81, 33 },
			new short[2] { 82, 33 },
			new short[2] { 83, 33 },
			new short[2] { 87, 33 },
			new short[2] { 88, 33 },
			new short[2] { 89, 33 },
			new short[2] { 112, 30 },
			new short[2] { 113, 30 },
			new short[2] { 114, 30 },
			new short[2] { 124, 30 }
		};
		List<short[]> wildBlockCore33 = new List<short[]>
		{
			new short[2] { 93, 33 },
			new short[2] { 94, 33 },
			new short[2] { 95, 33 },
			new short[2] { 96, 33 },
			new short[2] { 97, 33 },
			new short[2] { 98, 33 },
			new short[2] { 99, 33 },
			new short[2] { 100, 33 },
			new short[2] { 101, 33 },
			new short[2] { 102, 33 },
			new short[2] { 103, 33 },
			new short[2] { 104, 33 },
			new short[2] { 105, 33 },
			new short[2] { 106, 33 },
			new short[2] { 107, 33 },
			new short[2] { 108, 33 },
			new short[2] { 115, 40 },
			new short[2] { 116, 40 },
			new short[2] { 117, 40 },
			new short[2] { 124, 30 }
		};
		List<short[]> bigBaseBlockCore33 = new List<short[]>
		{
			new short[2] { 60, 1 },
			new short[2] { 78, 1 },
			new short[2] { 84, 1 },
			new short[2] { 91, 1 },
			new short[2] { 92, 1 }
		};
		list = new List<short[]>();
		List<short[]> seriesBlockCore33 = list;
		list = new List<short[]>();
		List<short[]> encircleBlockCore33 = list;
		short[] sceneryBlockCore33 = new short[0];
		list2 = new List<short>();
		List<short> lovingItemSubTypes33 = list2;
		list2 = new List<short>();
		dataArray33.Add(new MapAreaItem(92, 9, 388, 389, 20, worldMapPos33, neighborAreas33, "largemap_part_1_changgui_1", miniMapIcons33, null, miniMapPos33, 0, 0, 5, enemyNests33, enemyNestCreationDateRanges33, settlementBlockCore33, organizationId33, -1, 0, null, developedBlockCore33, normalBlockCore33, wildBlockCore33, bigBaseBlockCore33, seriesBlockCore33, encircleBlockCore33, sceneryBlockCore33, 390, 391, null, null, EMapAreaAreaDirection.South, lovingItemSubTypes33, list2, showDarkAshStatus: true));
		List<MapAreaItem> dataArray34 = _dataArray;
		sbyte[] worldMapPos34 = new sbyte[2] { 21, 7 };
		AreaTravelRoute[] neighborAreas34 = new AreaTravelRoute[0];
		string[] miniMapIcons34 = new string[4] { "largemap_part_2_icon_hei_0", "largemap_part_2_icon_jin_0", "largemap_part_2_icon_lan_0", "largemap_part_2_icon_hui_0" };
		short[] miniMapPos34 = new short[2] { 170, 270 };
		List<EnemyNestCreationInfo[]> enemyNests34 = new List<EnemyNestCreationInfo[]>
		{
			new EnemyNestCreationInfo[2]
			{
				new EnemyNestCreationInfo(0, 2),
				new EnemyNestCreationInfo(1, 2)
			},
			new EnemyNestCreationInfo[2]
			{
				new EnemyNestCreationInfo(1, 2),
				new EnemyNestCreationInfo(2, 3)
			},
			new EnemyNestCreationInfo[4]
			{
				new EnemyNestCreationInfo(2, 3),
				new EnemyNestCreationInfo(3, 3),
				new EnemyNestCreationInfo(4, 4),
				new EnemyNestCreationInfo(5, 4)
			},
			new EnemyNestCreationInfo[1]
			{
				new EnemyNestCreationInfo(12, 2)
			}
		};
		List<IntPair> enemyNestCreationDateRanges34 = new List<IntPair>
		{
			new IntPair(0, 6),
			new IntPair(6, 12),
			new IntPair(12, 24),
			new IntPair(0, 0)
		};
		short[] settlementBlockCore34 = new short[1] { 34 };
		sbyte[] organizationId34 = new sbyte[1] { 36 };
		List<short[]> developedBlockCore34 = new List<short[]>
		{
			new short[2] { 39, 33 },
			new short[2] { 40, 33 },
			new short[2] { 41, 33 },
			new short[2] { 42, 33 },
			new short[2] { 43, 33 },
			new short[2] { 44, 33 },
			new short[2] { 45, 33 },
			new short[2] { 46, 33 },
			new short[2] { 47, 33 },
			new short[2] { 48, 33 },
			new short[2] { 49, 33 },
			new short[2] { 50, 33 },
			new short[2] { 51, 33 },
			new short[2] { 52, 33 },
			new short[2] { 53, 33 },
			new short[2] { 54, 33 },
			new short[2] { 55, 33 },
			new short[2] { 56, 33 },
			new short[2] { 109, 10 },
			new short[2] { 110, 10 },
			new short[2] { 111, 10 }
		};
		List<short[]> normalBlockCore34 = new List<short[]>
		{
			new short[2] { 57, 33 },
			new short[2] { 58, 33 },
			new short[2] { 59, 33 },
			new short[2] { 63, 33 },
			new short[2] { 64, 33 },
			new short[2] { 65, 33 },
			new short[2] { 69, 33 },
			new short[2] { 70, 33 },
			new short[2] { 71, 33 },
			new short[2] { 75, 33 },
			new short[2] { 76, 33 },
			new short[2] { 77, 33 },
			new short[2] { 81, 33 },
			new short[2] { 82, 33 },
			new short[2] { 83, 33 },
			new short[2] { 87, 33 },
			new short[2] { 88, 33 },
			new short[2] { 89, 33 },
			new short[2] { 112, 30 },
			new short[2] { 113, 30 },
			new short[2] { 114, 30 },
			new short[2] { 124, 30 }
		};
		List<short[]> wildBlockCore34 = new List<short[]>
		{
			new short[2] { 93, 33 },
			new short[2] { 94, 33 },
			new short[2] { 95, 33 },
			new short[2] { 96, 33 },
			new short[2] { 97, 33 },
			new short[2] { 98, 33 },
			new short[2] { 99, 33 },
			new short[2] { 100, 33 },
			new short[2] { 101, 33 },
			new short[2] { 102, 33 },
			new short[2] { 103, 33 },
			new short[2] { 104, 33 },
			new short[2] { 105, 33 },
			new short[2] { 106, 33 },
			new short[2] { 107, 33 },
			new short[2] { 108, 33 },
			new short[2] { 115, 40 },
			new short[2] { 116, 40 },
			new short[2] { 117, 40 },
			new short[2] { 124, 30 }
		};
		List<short[]> bigBaseBlockCore34 = new List<short[]>
		{
			new short[2] { 60, 1 },
			new short[2] { 78, 1 },
			new short[2] { 84, 1 },
			new short[2] { 91, 1 },
			new short[2] { 92, 1 }
		};
		list = new List<short[]>();
		List<short[]> seriesBlockCore34 = list;
		list = new List<short[]>();
		List<short[]> encircleBlockCore34 = list;
		short[] sceneryBlockCore34 = new short[0];
		list2 = new List<short>();
		List<short> lovingItemSubTypes34 = list2;
		list2 = new List<short>();
		dataArray34.Add(new MapAreaItem(93, 9, 392, 393, 20, worldMapPos34, neighborAreas34, "largemap_part_1_changgui_0", miniMapIcons34, null, miniMapPos34, 0, 0, 5, enemyNests34, enemyNestCreationDateRanges34, settlementBlockCore34, organizationId34, -1, 0, null, developedBlockCore34, normalBlockCore34, wildBlockCore34, bigBaseBlockCore34, seriesBlockCore34, encircleBlockCore34, sceneryBlockCore34, 394, 395, null, null, EMapAreaAreaDirection.South, lovingItemSubTypes34, list2, showDarkAshStatus: true));
		List<MapAreaItem> dataArray35 = _dataArray;
		sbyte[] worldMapPos35 = new sbyte[2] { 23, 24 };
		AreaTravelRoute[] neighborAreas35 = new AreaTravelRoute[2]
		{
			new AreaTravelRoute(97, 6, null),
			new AreaTravelRoute(100, 16, new int[2] { 23, 25 })
		};
		string[] miniMapIcons35 = new string[4] { "largemap_part_2_icon_hei_0", "largemap_part_2_icon_jin_0", "largemap_part_2_icon_lan_0", "largemap_part_2_icon_hui_0" };
		short[] miniMapPos35 = new short[2] { 220, 200 };
		List<EnemyNestCreationInfo[]> enemyNests35 = new List<EnemyNestCreationInfo[]>
		{
			new EnemyNestCreationInfo[2]
			{
				new EnemyNestCreationInfo(0, 2),
				new EnemyNestCreationInfo(1, 2)
			},
			new EnemyNestCreationInfo[2]
			{
				new EnemyNestCreationInfo(1, 2),
				new EnemyNestCreationInfo(2, 3)
			},
			new EnemyNestCreationInfo[4]
			{
				new EnemyNestCreationInfo(2, 3),
				new EnemyNestCreationInfo(3, 3),
				new EnemyNestCreationInfo(4, 4),
				new EnemyNestCreationInfo(5, 4)
			},
			new EnemyNestCreationInfo[1]
			{
				new EnemyNestCreationInfo(12, 2)
			}
		};
		List<IntPair> enemyNestCreationDateRanges35 = new List<IntPair>
		{
			new IntPair(0, 6),
			new IntPair(6, 12),
			new IntPair(12, 24),
			new IntPair(0, 0)
		};
		short[] settlementBlockCore35 = new short[1] { 36 };
		sbyte[] organizationId35 = new sbyte[1] { 38 };
		List<short[]> developedBlockCore35 = new List<short[]>
		{
			new short[2] { 39, 33 },
			new short[2] { 40, 33 },
			new short[2] { 41, 33 },
			new short[2] { 42, 33 },
			new short[2] { 43, 33 },
			new short[2] { 44, 33 },
			new short[2] { 45, 33 },
			new short[2] { 46, 33 },
			new short[2] { 47, 33 },
			new short[2] { 48, 33 },
			new short[2] { 49, 33 },
			new short[2] { 50, 33 },
			new short[2] { 51, 33 },
			new short[2] { 52, 33 },
			new short[2] { 53, 33 },
			new short[2] { 54, 33 },
			new short[2] { 55, 33 },
			new short[2] { 56, 33 },
			new short[2] { 109, 10 },
			new short[2] { 110, 10 },
			new short[2] { 111, 10 }
		};
		List<short[]> normalBlockCore35 = new List<short[]>
		{
			new short[2] { 57, 33 },
			new short[2] { 58, 33 },
			new short[2] { 59, 33 },
			new short[2] { 63, 33 },
			new short[2] { 64, 33 },
			new short[2] { 65, 33 },
			new short[2] { 69, 33 },
			new short[2] { 70, 33 },
			new short[2] { 71, 33 },
			new short[2] { 75, 33 },
			new short[2] { 76, 33 },
			new short[2] { 77, 33 },
			new short[2] { 81, 33 },
			new short[2] { 82, 33 },
			new short[2] { 83, 33 },
			new short[2] { 87, 33 },
			new short[2] { 88, 33 },
			new short[2] { 89, 33 },
			new short[2] { 112, 30 },
			new short[2] { 113, 30 },
			new short[2] { 114, 30 },
			new short[2] { 124, 30 }
		};
		List<short[]> wildBlockCore35 = new List<short[]>
		{
			new short[2] { 93, 33 },
			new short[2] { 94, 33 },
			new short[2] { 95, 33 },
			new short[2] { 96, 33 },
			new short[2] { 97, 33 },
			new short[2] { 98, 33 },
			new short[2] { 99, 33 },
			new short[2] { 100, 33 },
			new short[2] { 101, 33 },
			new short[2] { 102, 33 },
			new short[2] { 103, 33 },
			new short[2] { 104, 33 },
			new short[2] { 105, 33 },
			new short[2] { 106, 33 },
			new short[2] { 107, 33 },
			new short[2] { 108, 33 },
			new short[2] { 115, 40 },
			new short[2] { 116, 40 },
			new short[2] { 117, 40 },
			new short[2] { 124, 30 }
		};
		List<short[]> bigBaseBlockCore35 = new List<short[]>
		{
			new short[2] { 73, 1 },
			new short[2] { 74, 1 },
			new short[2] { 60, 1 },
			new short[2] { 78, 1 },
			new short[2] { 90, 1 }
		};
		list = new List<short[]>();
		List<short[]> seriesBlockCore35 = list;
		list = new List<short[]>();
		List<short[]> encircleBlockCore35 = list;
		short[] sceneryBlockCore35 = new short[0];
		list2 = new List<short>();
		List<short> lovingItemSubTypes35 = list2;
		list2 = new List<short>();
		dataArray35.Add(new MapAreaItem(94, 10, 396, 397, 20, worldMapPos35, neighborAreas35, "largemap_part_1_changgui_0", miniMapIcons35, null, miniMapPos35, 0, 0, 5, enemyNests35, enemyNestCreationDateRanges35, settlementBlockCore35, organizationId35, -1, 0, null, developedBlockCore35, normalBlockCore35, wildBlockCore35, bigBaseBlockCore35, seriesBlockCore35, encircleBlockCore35, sceneryBlockCore35, 398, 399, null, null, EMapAreaAreaDirection.South, lovingItemSubTypes35, list2, showDarkAshStatus: true));
		List<MapAreaItem> dataArray36 = _dataArray;
		sbyte[] worldMapPos36 = new sbyte[2] { 21, 25 };
		AreaTravelRoute[] neighborAreas36 = new AreaTravelRoute[1]
		{
			new AreaTravelRoute(96, 16, new int[2] { 21, 24 })
		};
		string[] miniMapIcons36 = new string[4] { "largemap_part_2_icon_hei_0", "largemap_part_2_icon_jin_0", "largemap_part_2_icon_lan_0", "largemap_part_2_icon_hui_0" };
		short[] miniMapPos36 = new short[2] { 160, 240 };
		List<EnemyNestCreationInfo[]> enemyNests36 = new List<EnemyNestCreationInfo[]>
		{
			new EnemyNestCreationInfo[2]
			{
				new EnemyNestCreationInfo(0, 2),
				new EnemyNestCreationInfo(1, 2)
			},
			new EnemyNestCreationInfo[2]
			{
				new EnemyNestCreationInfo(1, 2),
				new EnemyNestCreationInfo(2, 3)
			},
			new EnemyNestCreationInfo[4]
			{
				new EnemyNestCreationInfo(2, 3),
				new EnemyNestCreationInfo(3, 3),
				new EnemyNestCreationInfo(4, 4),
				new EnemyNestCreationInfo(5, 4)
			},
			new EnemyNestCreationInfo[1]
			{
				new EnemyNestCreationInfo(12, 2)
			}
		};
		List<IntPair> enemyNestCreationDateRanges36 = new List<IntPair>
		{
			new IntPair(0, 6),
			new IntPair(6, 12),
			new IntPair(12, 24),
			new IntPair(0, 0)
		};
		short[] settlementBlockCore36 = new short[1] { 34 };
		sbyte[] organizationId36 = new sbyte[1] { 36 };
		List<short[]> developedBlockCore36 = new List<short[]>
		{
			new short[2] { 39, 33 },
			new short[2] { 40, 33 },
			new short[2] { 41, 33 },
			new short[2] { 42, 33 },
			new short[2] { 43, 33 },
			new short[2] { 44, 33 },
			new short[2] { 45, 33 },
			new short[2] { 46, 33 },
			new short[2] { 47, 33 },
			new short[2] { 48, 33 },
			new short[2] { 49, 33 },
			new short[2] { 50, 33 },
			new short[2] { 51, 33 },
			new short[2] { 52, 33 },
			new short[2] { 53, 33 },
			new short[2] { 54, 33 },
			new short[2] { 55, 33 },
			new short[2] { 56, 33 },
			new short[2] { 109, 10 },
			new short[2] { 110, 10 },
			new short[2] { 111, 10 }
		};
		List<short[]> normalBlockCore36 = new List<short[]>
		{
			new short[2] { 57, 33 },
			new short[2] { 58, 33 },
			new short[2] { 59, 33 },
			new short[2] { 63, 33 },
			new short[2] { 64, 33 },
			new short[2] { 65, 33 },
			new short[2] { 69, 33 },
			new short[2] { 70, 33 },
			new short[2] { 71, 33 },
			new short[2] { 75, 33 },
			new short[2] { 76, 33 },
			new short[2] { 77, 33 },
			new short[2] { 81, 33 },
			new short[2] { 82, 33 },
			new short[2] { 83, 33 },
			new short[2] { 87, 33 },
			new short[2] { 88, 33 },
			new short[2] { 89, 33 },
			new short[2] { 112, 30 },
			new short[2] { 113, 30 },
			new short[2] { 114, 30 },
			new short[2] { 124, 30 }
		};
		List<short[]> wildBlockCore36 = new List<short[]>
		{
			new short[2] { 93, 33 },
			new short[2] { 94, 33 },
			new short[2] { 95, 33 },
			new short[2] { 96, 33 },
			new short[2] { 97, 33 },
			new short[2] { 98, 33 },
			new short[2] { 99, 33 },
			new short[2] { 100, 33 },
			new short[2] { 101, 33 },
			new short[2] { 102, 33 },
			new short[2] { 103, 33 },
			new short[2] { 104, 33 },
			new short[2] { 105, 33 },
			new short[2] { 106, 33 },
			new short[2] { 107, 33 },
			new short[2] { 108, 33 },
			new short[2] { 115, 40 },
			new short[2] { 116, 40 },
			new short[2] { 117, 40 },
			new short[2] { 124, 30 }
		};
		List<short[]> bigBaseBlockCore36 = new List<short[]>
		{
			new short[2] { 73, 1 },
			new short[2] { 74, 1 },
			new short[2] { 60, 1 },
			new short[2] { 78, 1 },
			new short[2] { 90, 1 }
		};
		list = new List<short[]>();
		List<short[]> seriesBlockCore36 = list;
		list = new List<short[]>();
		List<short[]> encircleBlockCore36 = list;
		short[] sceneryBlockCore36 = new short[0];
		list2 = new List<short>();
		List<short> lovingItemSubTypes36 = list2;
		list2 = new List<short>();
		dataArray36.Add(new MapAreaItem(95, 10, 400, 401, 20, worldMapPos36, neighborAreas36, "largemap_part_1_changgui_0", miniMapIcons36, null, miniMapPos36, 0, 0, 5, enemyNests36, enemyNestCreationDateRanges36, settlementBlockCore36, organizationId36, -1, 0, null, developedBlockCore36, normalBlockCore36, wildBlockCore36, bigBaseBlockCore36, seriesBlockCore36, encircleBlockCore36, sceneryBlockCore36, 402, 403, null, null, EMapAreaAreaDirection.South, lovingItemSubTypes36, list2, showDarkAshStatus: true));
		List<MapAreaItem> dataArray37 = _dataArray;
		sbyte[] worldMapPos37 = new sbyte[2] { 20, 24 };
		AreaTravelRoute[] neighborAreas37 = new AreaTravelRoute[1]
		{
			new AreaTravelRoute(99, 24, new int[6] { 19, 24, 18, 24, 17, 24 })
		};
		string[] miniMapIcons37 = new string[4] { "largemap_part_2_icon_hei_1", "largemap_part_2_icon_jin_1", "largemap_part_2_icon_lan_1", "largemap_part_2_icon_hui_1" };
		short[] miniMapPos37 = new short[2] { 140, 200 };
		List<EnemyNestCreationInfo[]> enemyNests37 = new List<EnemyNestCreationInfo[]>
		{
			new EnemyNestCreationInfo[2]
			{
				new EnemyNestCreationInfo(0, 2),
				new EnemyNestCreationInfo(1, 2)
			},
			new EnemyNestCreationInfo[2]
			{
				new EnemyNestCreationInfo(1, 2),
				new EnemyNestCreationInfo(2, 3)
			},
			new EnemyNestCreationInfo[4]
			{
				new EnemyNestCreationInfo(2, 3),
				new EnemyNestCreationInfo(3, 3),
				new EnemyNestCreationInfo(4, 4),
				new EnemyNestCreationInfo(5, 4)
			},
			new EnemyNestCreationInfo[1]
			{
				new EnemyNestCreationInfo(12, 2)
			}
		};
		List<IntPair> enemyNestCreationDateRanges37 = new List<IntPair>
		{
			new IntPair(0, 6),
			new IntPair(6, 12),
			new IntPair(12, 24),
			new IntPair(0, 0)
		};
		short[] settlementBlockCore37 = new short[1] { 35 };
		sbyte[] organizationId37 = new sbyte[1] { 37 };
		List<short[]> developedBlockCore37 = new List<short[]>
		{
			new short[2] { 39, 33 },
			new short[2] { 40, 33 },
			new short[2] { 41, 33 },
			new short[2] { 42, 33 },
			new short[2] { 43, 33 },
			new short[2] { 44, 33 },
			new short[2] { 45, 33 },
			new short[2] { 46, 33 },
			new short[2] { 47, 33 },
			new short[2] { 48, 33 },
			new short[2] { 49, 33 },
			new short[2] { 50, 33 },
			new short[2] { 51, 33 },
			new short[2] { 52, 33 },
			new short[2] { 53, 33 },
			new short[2] { 54, 33 },
			new short[2] { 55, 33 },
			new short[2] { 56, 33 },
			new short[2] { 109, 10 },
			new short[2] { 110, 10 },
			new short[2] { 111, 10 }
		};
		List<short[]> normalBlockCore37 = new List<short[]>
		{
			new short[2] { 57, 33 },
			new short[2] { 58, 33 },
			new short[2] { 59, 33 },
			new short[2] { 63, 33 },
			new short[2] { 64, 33 },
			new short[2] { 65, 33 },
			new short[2] { 69, 33 },
			new short[2] { 70, 33 },
			new short[2] { 71, 33 },
			new short[2] { 75, 33 },
			new short[2] { 76, 33 },
			new short[2] { 77, 33 },
			new short[2] { 81, 33 },
			new short[2] { 82, 33 },
			new short[2] { 83, 33 },
			new short[2] { 87, 33 },
			new short[2] { 88, 33 },
			new short[2] { 89, 33 },
			new short[2] { 112, 30 },
			new short[2] { 113, 30 },
			new short[2] { 114, 30 },
			new short[2] { 124, 30 }
		};
		List<short[]> wildBlockCore37 = new List<short[]>
		{
			new short[2] { 93, 33 },
			new short[2] { 94, 33 },
			new short[2] { 95, 33 },
			new short[2] { 96, 33 },
			new short[2] { 97, 33 },
			new short[2] { 98, 33 },
			new short[2] { 99, 33 },
			new short[2] { 100, 33 },
			new short[2] { 101, 33 },
			new short[2] { 102, 33 },
			new short[2] { 103, 33 },
			new short[2] { 104, 33 },
			new short[2] { 105, 33 },
			new short[2] { 106, 33 },
			new short[2] { 107, 33 },
			new short[2] { 108, 33 },
			new short[2] { 115, 40 },
			new short[2] { 116, 40 },
			new short[2] { 117, 40 },
			new short[2] { 124, 30 }
		};
		List<short[]> bigBaseBlockCore37 = new List<short[]>
		{
			new short[2] { 73, 1 },
			new short[2] { 74, 1 },
			new short[2] { 60, 1 },
			new short[2] { 78, 1 },
			new short[2] { 90, 1 }
		};
		list = new List<short[]>();
		List<short[]> seriesBlockCore37 = list;
		list = new List<short[]>();
		List<short[]> encircleBlockCore37 = list;
		short[] sceneryBlockCore37 = new short[0];
		list2 = new List<short>();
		List<short> lovingItemSubTypes37 = list2;
		list2 = new List<short>();
		dataArray37.Add(new MapAreaItem(96, 10, 404, 405, 20, worldMapPos37, neighborAreas37, "largemap_part_1_changgui_1", miniMapIcons37, null, miniMapPos37, 0, 0, 5, enemyNests37, enemyNestCreationDateRanges37, settlementBlockCore37, organizationId37, -1, 0, null, developedBlockCore37, normalBlockCore37, wildBlockCore37, bigBaseBlockCore37, seriesBlockCore37, encircleBlockCore37, sceneryBlockCore37, 406, 407, null, null, EMapAreaAreaDirection.South, lovingItemSubTypes37, list2, showDarkAshStatus: true));
		List<MapAreaItem> dataArray38 = _dataArray;
		sbyte[] worldMapPos38 = new sbyte[2] { 23, 23 };
		AreaTravelRoute[] neighborAreas38 = new AreaTravelRoute[1]
		{
			new AreaTravelRoute(98, 22, new int[4] { 24, 23, 24, 22 })
		};
		string[] miniMapIcons38 = new string[4] { "largemap_part_2_icon_hei_1", "largemap_part_2_icon_jin_1", "largemap_part_2_icon_lan_1", "largemap_part_2_icon_hui_1" };
		short[] miniMapPos38 = new short[2] { 220, 160 };
		List<EnemyNestCreationInfo[]> enemyNests38 = new List<EnemyNestCreationInfo[]>
		{
			new EnemyNestCreationInfo[2]
			{
				new EnemyNestCreationInfo(0, 2),
				new EnemyNestCreationInfo(1, 2)
			},
			new EnemyNestCreationInfo[2]
			{
				new EnemyNestCreationInfo(1, 2),
				new EnemyNestCreationInfo(2, 3)
			},
			new EnemyNestCreationInfo[4]
			{
				new EnemyNestCreationInfo(2, 3),
				new EnemyNestCreationInfo(3, 3),
				new EnemyNestCreationInfo(4, 4),
				new EnemyNestCreationInfo(5, 4)
			},
			new EnemyNestCreationInfo[1]
			{
				new EnemyNestCreationInfo(12, 2)
			}
		};
		List<IntPair> enemyNestCreationDateRanges38 = new List<IntPair>
		{
			new IntPair(0, 6),
			new IntPair(6, 12),
			new IntPair(12, 24),
			new IntPair(0, 0)
		};
		short[] settlementBlockCore38 = new short[1] { 34 };
		sbyte[] organizationId38 = new sbyte[1] { 36 };
		List<short[]> developedBlockCore38 = new List<short[]>
		{
			new short[2] { 39, 33 },
			new short[2] { 40, 33 },
			new short[2] { 41, 33 },
			new short[2] { 42, 33 },
			new short[2] { 43, 33 },
			new short[2] { 44, 33 },
			new short[2] { 45, 33 },
			new short[2] { 46, 33 },
			new short[2] { 47, 33 },
			new short[2] { 48, 33 },
			new short[2] { 49, 33 },
			new short[2] { 50, 33 },
			new short[2] { 51, 33 },
			new short[2] { 52, 33 },
			new short[2] { 53, 33 },
			new short[2] { 54, 33 },
			new short[2] { 55, 33 },
			new short[2] { 56, 33 },
			new short[2] { 109, 10 },
			new short[2] { 110, 10 },
			new short[2] { 111, 10 }
		};
		List<short[]> normalBlockCore38 = new List<short[]>
		{
			new short[2] { 57, 33 },
			new short[2] { 58, 33 },
			new short[2] { 59, 33 },
			new short[2] { 63, 33 },
			new short[2] { 64, 33 },
			new short[2] { 65, 33 },
			new short[2] { 69, 33 },
			new short[2] { 70, 33 },
			new short[2] { 71, 33 },
			new short[2] { 75, 33 },
			new short[2] { 76, 33 },
			new short[2] { 77, 33 },
			new short[2] { 81, 33 },
			new short[2] { 82, 33 },
			new short[2] { 83, 33 },
			new short[2] { 87, 33 },
			new short[2] { 88, 33 },
			new short[2] { 89, 33 },
			new short[2] { 112, 30 },
			new short[2] { 113, 30 },
			new short[2] { 114, 30 },
			new short[2] { 124, 30 }
		};
		List<short[]> wildBlockCore38 = new List<short[]>
		{
			new short[2] { 93, 33 },
			new short[2] { 94, 33 },
			new short[2] { 95, 33 },
			new short[2] { 96, 33 },
			new short[2] { 97, 33 },
			new short[2] { 98, 33 },
			new short[2] { 99, 33 },
			new short[2] { 100, 33 },
			new short[2] { 101, 33 },
			new short[2] { 102, 33 },
			new short[2] { 103, 33 },
			new short[2] { 104, 33 },
			new short[2] { 105, 33 },
			new short[2] { 106, 33 },
			new short[2] { 107, 33 },
			new short[2] { 108, 33 },
			new short[2] { 115, 40 },
			new short[2] { 116, 40 },
			new short[2] { 117, 40 },
			new short[2] { 124, 30 }
		};
		List<short[]> bigBaseBlockCore38 = new List<short[]>
		{
			new short[2] { 73, 1 },
			new short[2] { 74, 1 },
			new short[2] { 60, 1 },
			new short[2] { 78, 1 },
			new short[2] { 90, 1 }
		};
		list = new List<short[]>();
		List<short[]> seriesBlockCore38 = list;
		list = new List<short[]>();
		List<short[]> encircleBlockCore38 = list;
		short[] sceneryBlockCore38 = new short[0];
		list2 = new List<short>();
		List<short> lovingItemSubTypes38 = list2;
		list2 = new List<short>();
		dataArray38.Add(new MapAreaItem(97, 10, 408, 409, 20, worldMapPos38, neighborAreas38, "largemap_part_1_changgui_1", miniMapIcons38, null, miniMapPos38, 0, 0, 5, enemyNests38, enemyNestCreationDateRanges38, settlementBlockCore38, organizationId38, -1, 0, null, developedBlockCore38, normalBlockCore38, wildBlockCore38, bigBaseBlockCore38, seriesBlockCore38, encircleBlockCore38, sceneryBlockCore38, 410, 411, null, null, EMapAreaAreaDirection.South, lovingItemSubTypes38, list2, showDarkAshStatus: true));
		List<MapAreaItem> dataArray39 = _dataArray;
		sbyte[] worldMapPos39 = new sbyte[2] { 24, 21 };
		AreaTravelRoute[] neighborAreas39 = new AreaTravelRoute[0];
		string[] miniMapIcons39 = new string[4] { "largemap_part_2_icon_hei_2", "largemap_part_2_icon_jin_2", "largemap_part_2_icon_lan_2", "largemap_part_2_icon_hui_2" };
		short[] miniMapPos39 = new short[2] { 250, 110 };
		List<EnemyNestCreationInfo[]> enemyNests39 = new List<EnemyNestCreationInfo[]>
		{
			new EnemyNestCreationInfo[2]
			{
				new EnemyNestCreationInfo(0, 2),
				new EnemyNestCreationInfo(1, 2)
			},
			new EnemyNestCreationInfo[2]
			{
				new EnemyNestCreationInfo(1, 2),
				new EnemyNestCreationInfo(2, 3)
			},
			new EnemyNestCreationInfo[4]
			{
				new EnemyNestCreationInfo(2, 3),
				new EnemyNestCreationInfo(3, 3),
				new EnemyNestCreationInfo(4, 4),
				new EnemyNestCreationInfo(5, 4)
			},
			new EnemyNestCreationInfo[1]
			{
				new EnemyNestCreationInfo(12, 2)
			}
		};
		List<IntPair> enemyNestCreationDateRanges39 = new List<IntPair>
		{
			new IntPair(0, 6),
			new IntPair(6, 12),
			new IntPair(12, 24),
			new IntPair(0, 0)
		};
		short[] settlementBlockCore39 = new short[1] { 35 };
		sbyte[] organizationId39 = new sbyte[1] { 37 };
		List<short[]> developedBlockCore39 = new List<short[]>
		{
			new short[2] { 39, 33 },
			new short[2] { 40, 33 },
			new short[2] { 41, 33 },
			new short[2] { 42, 33 },
			new short[2] { 43, 33 },
			new short[2] { 44, 33 },
			new short[2] { 45, 33 },
			new short[2] { 46, 33 },
			new short[2] { 47, 33 },
			new short[2] { 48, 33 },
			new short[2] { 49, 33 },
			new short[2] { 50, 33 },
			new short[2] { 51, 33 },
			new short[2] { 52, 33 },
			new short[2] { 53, 33 },
			new short[2] { 54, 33 },
			new short[2] { 55, 33 },
			new short[2] { 56, 33 },
			new short[2] { 109, 10 },
			new short[2] { 110, 10 },
			new short[2] { 111, 10 }
		};
		List<short[]> normalBlockCore39 = new List<short[]>
		{
			new short[2] { 57, 33 },
			new short[2] { 58, 33 },
			new short[2] { 59, 33 },
			new short[2] { 63, 33 },
			new short[2] { 64, 33 },
			new short[2] { 65, 33 },
			new short[2] { 69, 33 },
			new short[2] { 70, 33 },
			new short[2] { 71, 33 },
			new short[2] { 75, 33 },
			new short[2] { 76, 33 },
			new short[2] { 77, 33 },
			new short[2] { 81, 33 },
			new short[2] { 82, 33 },
			new short[2] { 83, 33 },
			new short[2] { 87, 33 },
			new short[2] { 88, 33 },
			new short[2] { 89, 33 },
			new short[2] { 112, 30 },
			new short[2] { 113, 30 },
			new short[2] { 114, 30 },
			new short[2] { 124, 30 }
		};
		List<short[]> wildBlockCore39 = new List<short[]>
		{
			new short[2] { 93, 33 },
			new short[2] { 94, 33 },
			new short[2] { 95, 33 },
			new short[2] { 96, 33 },
			new short[2] { 97, 33 },
			new short[2] { 98, 33 },
			new short[2] { 99, 33 },
			new short[2] { 100, 33 },
			new short[2] { 101, 33 },
			new short[2] { 102, 33 },
			new short[2] { 103, 33 },
			new short[2] { 104, 33 },
			new short[2] { 105, 33 },
			new short[2] { 106, 33 },
			new short[2] { 107, 33 },
			new short[2] { 108, 33 },
			new short[2] { 115, 40 },
			new short[2] { 116, 40 },
			new short[2] { 117, 40 },
			new short[2] { 124, 30 }
		};
		List<short[]> bigBaseBlockCore39 = new List<short[]>
		{
			new short[2] { 73, 1 },
			new short[2] { 74, 1 },
			new short[2] { 60, 1 },
			new short[2] { 78, 1 },
			new short[2] { 90, 1 }
		};
		list = new List<short[]>();
		List<short[]> seriesBlockCore39 = list;
		list = new List<short[]>();
		List<short[]> encircleBlockCore39 = list;
		short[] sceneryBlockCore39 = new short[0];
		list2 = new List<short>();
		List<short> lovingItemSubTypes39 = list2;
		list2 = new List<short>();
		dataArray39.Add(new MapAreaItem(98, 10, 412, 413, 20, worldMapPos39, neighborAreas39, "largemap_part_1_changgui_2", miniMapIcons39, null, miniMapPos39, 0, 0, 5, enemyNests39, enemyNestCreationDateRanges39, settlementBlockCore39, organizationId39, -1, 0, null, developedBlockCore39, normalBlockCore39, wildBlockCore39, bigBaseBlockCore39, seriesBlockCore39, encircleBlockCore39, sceneryBlockCore39, 414, 415, null, null, EMapAreaAreaDirection.South, lovingItemSubTypes39, list2, showDarkAshStatus: true));
		List<MapAreaItem> dataArray40 = _dataArray;
		sbyte[] worldMapPos40 = new sbyte[2] { 17, 23 };
		AreaTravelRoute[] neighborAreas40 = new AreaTravelRoute[0];
		string[] miniMapIcons40 = new string[4] { "largemap_part_2_icon_hei_0", "largemap_part_2_icon_jin_0", "largemap_part_2_icon_lan_0", "largemap_part_2_icon_hui_0" };
		short[] miniMapPos40 = new short[2] { 80, 200 };
		List<EnemyNestCreationInfo[]> enemyNests40 = new List<EnemyNestCreationInfo[]>
		{
			new EnemyNestCreationInfo[2]
			{
				new EnemyNestCreationInfo(0, 2),
				new EnemyNestCreationInfo(1, 2)
			},
			new EnemyNestCreationInfo[2]
			{
				new EnemyNestCreationInfo(1, 2),
				new EnemyNestCreationInfo(2, 3)
			},
			new EnemyNestCreationInfo[4]
			{
				new EnemyNestCreationInfo(2, 3),
				new EnemyNestCreationInfo(3, 3),
				new EnemyNestCreationInfo(4, 4),
				new EnemyNestCreationInfo(5, 4)
			},
			new EnemyNestCreationInfo[1]
			{
				new EnemyNestCreationInfo(12, 2)
			}
		};
		List<IntPair> enemyNestCreationDateRanges40 = new List<IntPair>
		{
			new IntPair(0, 6),
			new IntPair(6, 12),
			new IntPair(12, 24),
			new IntPair(0, 0)
		};
		short[] settlementBlockCore40 = new short[1] { 36 };
		sbyte[] organizationId40 = new sbyte[1] { 38 };
		List<short[]> developedBlockCore40 = new List<short[]>
		{
			new short[2] { 39, 33 },
			new short[2] { 40, 33 },
			new short[2] { 41, 33 },
			new short[2] { 42, 33 },
			new short[2] { 43, 33 },
			new short[2] { 44, 33 },
			new short[2] { 45, 33 },
			new short[2] { 46, 33 },
			new short[2] { 47, 33 },
			new short[2] { 48, 33 },
			new short[2] { 49, 33 },
			new short[2] { 50, 33 },
			new short[2] { 51, 33 },
			new short[2] { 52, 33 },
			new short[2] { 53, 33 },
			new short[2] { 54, 33 },
			new short[2] { 55, 33 },
			new short[2] { 56, 33 },
			new short[2] { 109, 10 },
			new short[2] { 110, 10 },
			new short[2] { 111, 10 }
		};
		List<short[]> normalBlockCore40 = new List<short[]>
		{
			new short[2] { 57, 33 },
			new short[2] { 58, 33 },
			new short[2] { 59, 33 },
			new short[2] { 63, 33 },
			new short[2] { 64, 33 },
			new short[2] { 65, 33 },
			new short[2] { 69, 33 },
			new short[2] { 70, 33 },
			new short[2] { 71, 33 },
			new short[2] { 75, 33 },
			new short[2] { 76, 33 },
			new short[2] { 77, 33 },
			new short[2] { 81, 33 },
			new short[2] { 82, 33 },
			new short[2] { 83, 33 },
			new short[2] { 87, 33 },
			new short[2] { 88, 33 },
			new short[2] { 89, 33 },
			new short[2] { 112, 30 },
			new short[2] { 113, 30 },
			new short[2] { 114, 30 },
			new short[2] { 124, 30 }
		};
		List<short[]> wildBlockCore40 = new List<short[]>
		{
			new short[2] { 93, 33 },
			new short[2] { 94, 33 },
			new short[2] { 95, 33 },
			new short[2] { 96, 33 },
			new short[2] { 97, 33 },
			new short[2] { 98, 33 },
			new short[2] { 99, 33 },
			new short[2] { 100, 33 },
			new short[2] { 101, 33 },
			new short[2] { 102, 33 },
			new short[2] { 103, 33 },
			new short[2] { 104, 33 },
			new short[2] { 105, 33 },
			new short[2] { 106, 33 },
			new short[2] { 107, 33 },
			new short[2] { 108, 33 },
			new short[2] { 115, 40 },
			new short[2] { 116, 40 },
			new short[2] { 117, 40 },
			new short[2] { 124, 30 }
		};
		List<short[]> bigBaseBlockCore40 = new List<short[]>
		{
			new short[2] { 73, 1 },
			new short[2] { 74, 1 },
			new short[2] { 60, 1 },
			new short[2] { 78, 1 },
			new short[2] { 90, 1 }
		};
		list = new List<short[]>();
		List<short[]> seriesBlockCore40 = list;
		list = new List<short[]>();
		List<short[]> encircleBlockCore40 = list;
		short[] sceneryBlockCore40 = new short[0];
		list2 = new List<short>();
		List<short> lovingItemSubTypes40 = list2;
		list2 = new List<short>();
		dataArray40.Add(new MapAreaItem(99, 10, 416, 417, 20, worldMapPos40, neighborAreas40, "largemap_part_1_changgui_0", miniMapIcons40, null, miniMapPos40, 0, 0, 5, enemyNests40, enemyNestCreationDateRanges40, settlementBlockCore40, organizationId40, -1, 0, null, developedBlockCore40, normalBlockCore40, wildBlockCore40, bigBaseBlockCore40, seriesBlockCore40, encircleBlockCore40, sceneryBlockCore40, 418, 419, null, null, EMapAreaAreaDirection.South, lovingItemSubTypes40, list2, showDarkAshStatus: true));
		List<MapAreaItem> dataArray41 = _dataArray;
		sbyte[] worldMapPos41 = new sbyte[2] { 24, 25 };
		AreaTravelRoute[] neighborAreas41 = new AreaTravelRoute[0];
		string[] miniMapIcons41 = new string[4] { "largemap_part_2_icon_hei_1", "largemap_part_2_icon_jin_1", "largemap_part_2_icon_lan_1", "largemap_part_2_icon_hui_1" };
		short[] miniMapPos41 = new short[2] { 240, 240 };
		List<EnemyNestCreationInfo[]> enemyNests41 = new List<EnemyNestCreationInfo[]>
		{
			new EnemyNestCreationInfo[2]
			{
				new EnemyNestCreationInfo(0, 2),
				new EnemyNestCreationInfo(1, 2)
			},
			new EnemyNestCreationInfo[2]
			{
				new EnemyNestCreationInfo(1, 2),
				new EnemyNestCreationInfo(2, 3)
			},
			new EnemyNestCreationInfo[4]
			{
				new EnemyNestCreationInfo(2, 3),
				new EnemyNestCreationInfo(3, 3),
				new EnemyNestCreationInfo(4, 4),
				new EnemyNestCreationInfo(5, 4)
			},
			new EnemyNestCreationInfo[1]
			{
				new EnemyNestCreationInfo(12, 2)
			}
		};
		List<IntPair> enemyNestCreationDateRanges41 = new List<IntPair>
		{
			new IntPair(0, 6),
			new IntPair(6, 12),
			new IntPair(12, 24),
			new IntPair(0, 0)
		};
		short[] settlementBlockCore41 = new short[1] { 34 };
		sbyte[] organizationId41 = new sbyte[1] { 36 };
		List<short[]> developedBlockCore41 = new List<short[]>
		{
			new short[2] { 39, 33 },
			new short[2] { 40, 33 },
			new short[2] { 41, 33 },
			new short[2] { 42, 33 },
			new short[2] { 43, 33 },
			new short[2] { 44, 33 },
			new short[2] { 45, 33 },
			new short[2] { 46, 33 },
			new short[2] { 47, 33 },
			new short[2] { 48, 33 },
			new short[2] { 49, 33 },
			new short[2] { 50, 33 },
			new short[2] { 51, 33 },
			new short[2] { 52, 33 },
			new short[2] { 53, 33 },
			new short[2] { 54, 33 },
			new short[2] { 55, 33 },
			new short[2] { 56, 33 },
			new short[2] { 109, 10 },
			new short[2] { 110, 10 },
			new short[2] { 111, 10 }
		};
		List<short[]> normalBlockCore41 = new List<short[]>
		{
			new short[2] { 57, 33 },
			new short[2] { 58, 33 },
			new short[2] { 59, 33 },
			new short[2] { 63, 33 },
			new short[2] { 64, 33 },
			new short[2] { 65, 33 },
			new short[2] { 69, 33 },
			new short[2] { 70, 33 },
			new short[2] { 71, 33 },
			new short[2] { 75, 33 },
			new short[2] { 76, 33 },
			new short[2] { 77, 33 },
			new short[2] { 81, 33 },
			new short[2] { 82, 33 },
			new short[2] { 83, 33 },
			new short[2] { 87, 33 },
			new short[2] { 88, 33 },
			new short[2] { 89, 33 },
			new short[2] { 112, 30 },
			new short[2] { 113, 30 },
			new short[2] { 114, 30 },
			new short[2] { 124, 30 }
		};
		List<short[]> wildBlockCore41 = new List<short[]>
		{
			new short[2] { 93, 33 },
			new short[2] { 94, 33 },
			new short[2] { 95, 33 },
			new short[2] { 96, 33 },
			new short[2] { 97, 33 },
			new short[2] { 98, 33 },
			new short[2] { 99, 33 },
			new short[2] { 100, 33 },
			new short[2] { 101, 33 },
			new short[2] { 102, 33 },
			new short[2] { 103, 33 },
			new short[2] { 104, 33 },
			new short[2] { 105, 33 },
			new short[2] { 106, 33 },
			new short[2] { 107, 33 },
			new short[2] { 108, 33 },
			new short[2] { 115, 40 },
			new short[2] { 116, 40 },
			new short[2] { 117, 40 },
			new short[2] { 124, 30 }
		};
		List<short[]> bigBaseBlockCore41 = new List<short[]>
		{
			new short[2] { 73, 1 },
			new short[2] { 74, 1 },
			new short[2] { 60, 1 },
			new short[2] { 78, 1 },
			new short[2] { 90, 1 }
		};
		list = new List<short[]>();
		List<short[]> seriesBlockCore41 = list;
		list = new List<short[]>();
		List<short[]> encircleBlockCore41 = list;
		short[] sceneryBlockCore41 = new short[0];
		list2 = new List<short>();
		List<short> lovingItemSubTypes41 = list2;
		list2 = new List<short>();
		dataArray41.Add(new MapAreaItem(100, 10, 420, 421, 20, worldMapPos41, neighborAreas41, "largemap_part_1_changgui_1", miniMapIcons41, null, miniMapPos41, 0, 0, 5, enemyNests41, enemyNestCreationDateRanges41, settlementBlockCore41, organizationId41, -1, 0, null, developedBlockCore41, normalBlockCore41, wildBlockCore41, bigBaseBlockCore41, seriesBlockCore41, encircleBlockCore41, sceneryBlockCore41, 422, 423, null, null, EMapAreaAreaDirection.South, lovingItemSubTypes41, list2, showDarkAshStatus: true));
		List<MapAreaItem> dataArray42 = _dataArray;
		sbyte[] worldMapPos42 = new sbyte[2] { 3, 17 };
		AreaTravelRoute[] neighborAreas42 = new AreaTravelRoute[3]
		{
			new AreaTravelRoute(102, 76, new int[14]
			{
				3, 16, 3, 15, 2, 15, 1, 15, 1, 16,
				1, 17, 1, 18
			}),
			new AreaTravelRoute(103, 26, new int[4] { 3, 16, 3, 15 }),
			new AreaTravelRoute(106, 36, new int[6] { 3, 16, 3, 15, 3, 14 })
		};
		string[] miniMapIcons42 = new string[4] { "largemap_part_2_icon_hei_2", "largemap_part_2_icon_jin_2", "largemap_part_2_icon_lan_2", "largemap_part_2_icon_hui_2" };
		short[] miniMapPos42 = new short[2] { 160, 230 };
		List<EnemyNestCreationInfo[]> enemyNests42 = new List<EnemyNestCreationInfo[]>
		{
			new EnemyNestCreationInfo[2]
			{
				new EnemyNestCreationInfo(0, 2),
				new EnemyNestCreationInfo(1, 2)
			},
			new EnemyNestCreationInfo[2]
			{
				new EnemyNestCreationInfo(1, 2),
				new EnemyNestCreationInfo(2, 3)
			},
			new EnemyNestCreationInfo[4]
			{
				new EnemyNestCreationInfo(2, 3),
				new EnemyNestCreationInfo(3, 3),
				new EnemyNestCreationInfo(4, 4),
				new EnemyNestCreationInfo(5, 4)
			},
			new EnemyNestCreationInfo[1]
			{
				new EnemyNestCreationInfo(12, 2)
			}
		};
		List<IntPair> enemyNestCreationDateRanges42 = new List<IntPair>
		{
			new IntPair(0, 6),
			new IntPair(6, 12),
			new IntPair(12, 24),
			new IntPair(0, 0)
		};
		short[] settlementBlockCore42 = new short[1] { 35 };
		sbyte[] organizationId42 = new sbyte[1] { 37 };
		List<short[]> developedBlockCore42 = new List<short[]>
		{
			new short[2] { 39, 33 },
			new short[2] { 40, 33 },
			new short[2] { 41, 33 },
			new short[2] { 42, 33 },
			new short[2] { 43, 33 },
			new short[2] { 44, 33 },
			new short[2] { 45, 33 },
			new short[2] { 46, 33 },
			new short[2] { 47, 33 },
			new short[2] { 48, 33 },
			new short[2] { 49, 33 },
			new short[2] { 50, 33 },
			new short[2] { 51, 33 },
			new short[2] { 52, 33 },
			new short[2] { 53, 33 },
			new short[2] { 54, 33 },
			new short[2] { 55, 33 },
			new short[2] { 56, 33 },
			new short[2] { 109, 10 },
			new short[2] { 110, 10 },
			new short[2] { 111, 10 }
		};
		List<short[]> normalBlockCore42 = new List<short[]>
		{
			new short[2] { 57, 33 },
			new short[2] { 58, 33 },
			new short[2] { 59, 33 },
			new short[2] { 63, 33 },
			new short[2] { 64, 33 },
			new short[2] { 65, 33 },
			new short[2] { 69, 33 },
			new short[2] { 70, 33 },
			new short[2] { 71, 33 },
			new short[2] { 75, 33 },
			new short[2] { 76, 33 },
			new short[2] { 77, 33 },
			new short[2] { 81, 33 },
			new short[2] { 82, 33 },
			new short[2] { 83, 33 },
			new short[2] { 87, 33 },
			new short[2] { 88, 33 },
			new short[2] { 89, 33 },
			new short[2] { 112, 30 },
			new short[2] { 113, 30 },
			new short[2] { 114, 30 },
			new short[2] { 124, 30 }
		};
		List<short[]> wildBlockCore42 = new List<short[]>
		{
			new short[2] { 93, 33 },
			new short[2] { 94, 33 },
			new short[2] { 95, 33 },
			new short[2] { 96, 33 },
			new short[2] { 97, 33 },
			new short[2] { 98, 33 },
			new short[2] { 99, 33 },
			new short[2] { 100, 33 },
			new short[2] { 101, 33 },
			new short[2] { 102, 33 },
			new short[2] { 103, 33 },
			new short[2] { 104, 33 },
			new short[2] { 105, 33 },
			new short[2] { 106, 33 },
			new short[2] { 107, 33 },
			new short[2] { 108, 33 },
			new short[2] { 115, 40 },
			new short[2] { 116, 40 },
			new short[2] { 117, 40 },
			new short[2] { 124, 30 }
		};
		List<short[]> bigBaseBlockCore42 = new List<short[]>
		{
			new short[2] { 72, 1 },
			new short[2] { 61, 1 },
			new short[2] { 62, 1 },
			new short[2] { 84, 1 },
			new short[2] { 66, 1 }
		};
		list = new List<short[]>();
		List<short[]> seriesBlockCore42 = list;
		list = new List<short[]>();
		List<short[]> encircleBlockCore42 = list;
		short[] sceneryBlockCore42 = new short[0];
		list2 = new List<short>();
		List<short> lovingItemSubTypes42 = list2;
		list2 = new List<short>();
		dataArray42.Add(new MapAreaItem(101, 11, 424, 425, 20, worldMapPos42, neighborAreas42, "largemap_part_1_changgui_2", miniMapIcons42, null, miniMapPos42, 0, 0, 5, enemyNests42, enemyNestCreationDateRanges42, settlementBlockCore42, organizationId42, -1, 0, null, developedBlockCore42, normalBlockCore42, wildBlockCore42, bigBaseBlockCore42, seriesBlockCore42, encircleBlockCore42, sceneryBlockCore42, 426, 427, null, null, EMapAreaAreaDirection.South, lovingItemSubTypes42, list2, showDarkAshStatus: true));
		List<MapAreaItem> dataArray43 = _dataArray;
		sbyte[] worldMapPos43 = new sbyte[2] { 1, 19 };
		AreaTravelRoute[] neighborAreas43 = new AreaTravelRoute[2]
		{
			new AreaTravelRoute(103, 66, new int[12]
			{
				1, 18, 1, 17, 1, 16, 1, 15, 2, 15,
				3, 15
			}),
			new AreaTravelRoute(106, 76, new int[14]
			{
				1, 18, 1, 17, 1, 16, 1, 15, 2, 15,
				3, 15, 3, 14
			})
		};
		string[] miniMapIcons43 = new string[4] { "largemap_part_2_icon_hei_0", "largemap_part_2_icon_jin_0", "largemap_part_2_icon_lan_0", "largemap_part_2_icon_hui_0" };
		short[] miniMapPos43 = new short[2] { 120, 270 };
		List<EnemyNestCreationInfo[]> enemyNests43 = new List<EnemyNestCreationInfo[]>
		{
			new EnemyNestCreationInfo[2]
			{
				new EnemyNestCreationInfo(0, 2),
				new EnemyNestCreationInfo(1, 2)
			},
			new EnemyNestCreationInfo[2]
			{
				new EnemyNestCreationInfo(1, 2),
				new EnemyNestCreationInfo(2, 3)
			},
			new EnemyNestCreationInfo[4]
			{
				new EnemyNestCreationInfo(2, 3),
				new EnemyNestCreationInfo(3, 3),
				new EnemyNestCreationInfo(4, 4),
				new EnemyNestCreationInfo(5, 4)
			},
			new EnemyNestCreationInfo[1]
			{
				new EnemyNestCreationInfo(12, 2)
			}
		};
		List<IntPair> enemyNestCreationDateRanges43 = new List<IntPair>
		{
			new IntPair(0, 6),
			new IntPair(6, 12),
			new IntPair(12, 24),
			new IntPair(0, 0)
		};
		short[] settlementBlockCore43 = new short[1] { 34 };
		sbyte[] organizationId43 = new sbyte[1] { 36 };
		List<short[]> developedBlockCore43 = new List<short[]>
		{
			new short[2] { 39, 33 },
			new short[2] { 40, 33 },
			new short[2] { 41, 33 },
			new short[2] { 42, 33 },
			new short[2] { 43, 33 },
			new short[2] { 44, 33 },
			new short[2] { 45, 33 },
			new short[2] { 46, 33 },
			new short[2] { 47, 33 },
			new short[2] { 48, 33 },
			new short[2] { 49, 33 },
			new short[2] { 50, 33 },
			new short[2] { 51, 33 },
			new short[2] { 52, 33 },
			new short[2] { 53, 33 },
			new short[2] { 54, 33 },
			new short[2] { 55, 33 },
			new short[2] { 56, 33 },
			new short[2] { 109, 10 },
			new short[2] { 110, 10 },
			new short[2] { 111, 10 }
		};
		List<short[]> normalBlockCore43 = new List<short[]>
		{
			new short[2] { 57, 33 },
			new short[2] { 58, 33 },
			new short[2] { 59, 33 },
			new short[2] { 63, 33 },
			new short[2] { 64, 33 },
			new short[2] { 65, 33 },
			new short[2] { 69, 33 },
			new short[2] { 70, 33 },
			new short[2] { 71, 33 },
			new short[2] { 75, 33 },
			new short[2] { 76, 33 },
			new short[2] { 77, 33 },
			new short[2] { 81, 33 },
			new short[2] { 82, 33 },
			new short[2] { 83, 33 },
			new short[2] { 87, 33 },
			new short[2] { 88, 33 },
			new short[2] { 89, 33 },
			new short[2] { 112, 30 },
			new short[2] { 113, 30 },
			new short[2] { 114, 30 },
			new short[2] { 124, 30 }
		};
		List<short[]> wildBlockCore43 = new List<short[]>
		{
			new short[2] { 93, 33 },
			new short[2] { 94, 33 },
			new short[2] { 95, 33 },
			new short[2] { 96, 33 },
			new short[2] { 97, 33 },
			new short[2] { 98, 33 },
			new short[2] { 99, 33 },
			new short[2] { 100, 33 },
			new short[2] { 101, 33 },
			new short[2] { 102, 33 },
			new short[2] { 103, 33 },
			new short[2] { 104, 33 },
			new short[2] { 105, 33 },
			new short[2] { 106, 33 },
			new short[2] { 107, 33 },
			new short[2] { 108, 33 },
			new short[2] { 115, 40 },
			new short[2] { 116, 40 },
			new short[2] { 117, 40 },
			new short[2] { 124, 30 }
		};
		List<short[]> bigBaseBlockCore43 = new List<short[]>
		{
			new short[2] { 72, 1 },
			new short[2] { 61, 1 },
			new short[2] { 62, 1 },
			new short[2] { 84, 1 },
			new short[2] { 66, 1 }
		};
		list = new List<short[]>();
		List<short[]> seriesBlockCore43 = list;
		list = new List<short[]>();
		List<short[]> encircleBlockCore43 = list;
		short[] sceneryBlockCore43 = new short[0];
		list2 = new List<short>();
		List<short> lovingItemSubTypes43 = list2;
		list2 = new List<short>();
		dataArray43.Add(new MapAreaItem(102, 11, 428, 429, 20, worldMapPos43, neighborAreas43, "largemap_part_1_changgui_0", miniMapIcons43, null, miniMapPos43, 0, 0, 5, enemyNests43, enemyNestCreationDateRanges43, settlementBlockCore43, organizationId43, -1, 0, null, developedBlockCore43, normalBlockCore43, wildBlockCore43, bigBaseBlockCore43, seriesBlockCore43, encircleBlockCore43, sceneryBlockCore43, 430, 431, null, null, EMapAreaAreaDirection.South, lovingItemSubTypes43, list2, showDarkAshStatus: true));
		List<MapAreaItem> dataArray44 = _dataArray;
		sbyte[] worldMapPos44 = new sbyte[2] { 4, 15 };
		AreaTravelRoute[] neighborAreas44 = new AreaTravelRoute[2]
		{
			new AreaTravelRoute(105, 16, new int[2] { 5, 15 }),
			new AreaTravelRoute(106, 26, new int[4] { 3, 15, 3, 14 })
		};
		string[] miniMapIcons44 = new string[4] { "largemap_part_2_icon_hei_2", "largemap_part_2_icon_jin_2", "largemap_part_2_icon_lan_2", "largemap_part_2_icon_hui_2" };
		short[] miniMapPos44 = new short[2] { 190, 150 };
		List<EnemyNestCreationInfo[]> enemyNests44 = new List<EnemyNestCreationInfo[]>
		{
			new EnemyNestCreationInfo[2]
			{
				new EnemyNestCreationInfo(0, 2),
				new EnemyNestCreationInfo(1, 2)
			},
			new EnemyNestCreationInfo[2]
			{
				new EnemyNestCreationInfo(1, 2),
				new EnemyNestCreationInfo(2, 3)
			},
			new EnemyNestCreationInfo[4]
			{
				new EnemyNestCreationInfo(2, 3),
				new EnemyNestCreationInfo(3, 3),
				new EnemyNestCreationInfo(4, 4),
				new EnemyNestCreationInfo(5, 4)
			},
			new EnemyNestCreationInfo[1]
			{
				new EnemyNestCreationInfo(12, 2)
			}
		};
		List<IntPair> enemyNestCreationDateRanges44 = new List<IntPair>
		{
			new IntPair(0, 6),
			new IntPair(6, 12),
			new IntPair(12, 24),
			new IntPair(0, 0)
		};
		short[] settlementBlockCore44 = new short[1] { 35 };
		sbyte[] organizationId44 = new sbyte[1] { 37 };
		List<short[]> developedBlockCore44 = new List<short[]>
		{
			new short[2] { 39, 33 },
			new short[2] { 40, 33 },
			new short[2] { 41, 33 },
			new short[2] { 42, 33 },
			new short[2] { 43, 33 },
			new short[2] { 44, 33 },
			new short[2] { 45, 33 },
			new short[2] { 46, 33 },
			new short[2] { 47, 33 },
			new short[2] { 48, 33 },
			new short[2] { 49, 33 },
			new short[2] { 50, 33 },
			new short[2] { 51, 33 },
			new short[2] { 52, 33 },
			new short[2] { 53, 33 },
			new short[2] { 54, 33 },
			new short[2] { 55, 33 },
			new short[2] { 56, 33 },
			new short[2] { 109, 10 },
			new short[2] { 110, 10 },
			new short[2] { 111, 10 }
		};
		List<short[]> normalBlockCore44 = new List<short[]>
		{
			new short[2] { 57, 33 },
			new short[2] { 58, 33 },
			new short[2] { 59, 33 },
			new short[2] { 63, 33 },
			new short[2] { 64, 33 },
			new short[2] { 65, 33 },
			new short[2] { 69, 33 },
			new short[2] { 70, 33 },
			new short[2] { 71, 33 },
			new short[2] { 75, 33 },
			new short[2] { 76, 33 },
			new short[2] { 77, 33 },
			new short[2] { 81, 33 },
			new short[2] { 82, 33 },
			new short[2] { 83, 33 },
			new short[2] { 87, 33 },
			new short[2] { 88, 33 },
			new short[2] { 89, 33 },
			new short[2] { 112, 30 },
			new short[2] { 113, 30 },
			new short[2] { 114, 30 },
			new short[2] { 124, 30 }
		};
		List<short[]> wildBlockCore44 = new List<short[]>
		{
			new short[2] { 93, 33 },
			new short[2] { 94, 33 },
			new short[2] { 95, 33 },
			new short[2] { 96, 33 },
			new short[2] { 97, 33 },
			new short[2] { 98, 33 },
			new short[2] { 99, 33 },
			new short[2] { 100, 33 },
			new short[2] { 101, 33 },
			new short[2] { 102, 33 },
			new short[2] { 103, 33 },
			new short[2] { 104, 33 },
			new short[2] { 105, 33 },
			new short[2] { 106, 33 },
			new short[2] { 107, 33 },
			new short[2] { 108, 33 },
			new short[2] { 115, 40 },
			new short[2] { 116, 40 },
			new short[2] { 117, 40 },
			new short[2] { 124, 30 }
		};
		List<short[]> bigBaseBlockCore44 = new List<short[]>
		{
			new short[2] { 72, 1 },
			new short[2] { 61, 1 },
			new short[2] { 62, 1 },
			new short[2] { 84, 1 },
			new short[2] { 66, 1 }
		};
		list = new List<short[]>();
		List<short[]> seriesBlockCore44 = list;
		list = new List<short[]>();
		List<short[]> encircleBlockCore44 = list;
		short[] sceneryBlockCore44 = new short[0];
		list2 = new List<short>();
		List<short> lovingItemSubTypes44 = list2;
		list2 = new List<short>();
		dataArray44.Add(new MapAreaItem(103, 11, 432, 433, 20, worldMapPos44, neighborAreas44, "largemap_part_1_changgui_2", miniMapIcons44, null, miniMapPos44, 0, 0, 5, enemyNests44, enemyNestCreationDateRanges44, settlementBlockCore44, organizationId44, -1, 0, null, developedBlockCore44, normalBlockCore44, wildBlockCore44, bigBaseBlockCore44, seriesBlockCore44, encircleBlockCore44, sceneryBlockCore44, 434, 435, null, null, EMapAreaAreaDirection.South, lovingItemSubTypes44, list2, showDarkAshStatus: true));
		List<MapAreaItem> dataArray45 = _dataArray;
		sbyte[] worldMapPos45 = new sbyte[2] { 7, 13 };
		AreaTravelRoute[] neighborAreas45 = new AreaTravelRoute[1]
		{
			new AreaTravelRoute(107, 6, null)
		};
		string[] miniMapIcons45 = new string[4] { "largemap_part_2_icon_hei_2", "largemap_part_2_icon_jin_2", "largemap_part_2_icon_lan_2", "largemap_part_2_icon_hui_2" };
		short[] miniMapPos45 = new short[2] { 280, 110 };
		List<EnemyNestCreationInfo[]> enemyNests45 = new List<EnemyNestCreationInfo[]>
		{
			new EnemyNestCreationInfo[2]
			{
				new EnemyNestCreationInfo(0, 2),
				new EnemyNestCreationInfo(1, 2)
			},
			new EnemyNestCreationInfo[2]
			{
				new EnemyNestCreationInfo(1, 2),
				new EnemyNestCreationInfo(2, 3)
			},
			new EnemyNestCreationInfo[4]
			{
				new EnemyNestCreationInfo(2, 3),
				new EnemyNestCreationInfo(3, 3),
				new EnemyNestCreationInfo(4, 4),
				new EnemyNestCreationInfo(5, 4)
			},
			new EnemyNestCreationInfo[1]
			{
				new EnemyNestCreationInfo(12, 2)
			}
		};
		List<IntPair> enemyNestCreationDateRanges45 = new List<IntPair>
		{
			new IntPair(0, 6),
			new IntPair(6, 12),
			new IntPair(12, 24),
			new IntPair(0, 0)
		};
		short[] settlementBlockCore45 = new short[1] { 35 };
		sbyte[] organizationId45 = new sbyte[1] { 37 };
		List<short[]> developedBlockCore45 = new List<short[]>
		{
			new short[2] { 39, 33 },
			new short[2] { 40, 33 },
			new short[2] { 41, 33 },
			new short[2] { 42, 33 },
			new short[2] { 43, 33 },
			new short[2] { 44, 33 },
			new short[2] { 45, 33 },
			new short[2] { 46, 33 },
			new short[2] { 47, 33 },
			new short[2] { 48, 33 },
			new short[2] { 49, 33 },
			new short[2] { 50, 33 },
			new short[2] { 51, 33 },
			new short[2] { 52, 33 },
			new short[2] { 53, 33 },
			new short[2] { 54, 33 },
			new short[2] { 55, 33 },
			new short[2] { 56, 33 },
			new short[2] { 109, 10 },
			new short[2] { 110, 10 },
			new short[2] { 111, 10 }
		};
		List<short[]> normalBlockCore45 = new List<short[]>
		{
			new short[2] { 57, 33 },
			new short[2] { 58, 33 },
			new short[2] { 59, 33 },
			new short[2] { 63, 33 },
			new short[2] { 64, 33 },
			new short[2] { 65, 33 },
			new short[2] { 69, 33 },
			new short[2] { 70, 33 },
			new short[2] { 71, 33 },
			new short[2] { 75, 33 },
			new short[2] { 76, 33 },
			new short[2] { 77, 33 },
			new short[2] { 81, 33 },
			new short[2] { 82, 33 },
			new short[2] { 83, 33 },
			new short[2] { 87, 33 },
			new short[2] { 88, 33 },
			new short[2] { 89, 33 },
			new short[2] { 112, 30 },
			new short[2] { 113, 30 },
			new short[2] { 114, 30 },
			new short[2] { 124, 30 }
		};
		List<short[]> wildBlockCore45 = new List<short[]>
		{
			new short[2] { 93, 33 },
			new short[2] { 94, 33 },
			new short[2] { 95, 33 },
			new short[2] { 96, 33 },
			new short[2] { 97, 33 },
			new short[2] { 98, 33 },
			new short[2] { 99, 33 },
			new short[2] { 100, 33 },
			new short[2] { 101, 33 },
			new short[2] { 102, 33 },
			new short[2] { 103, 33 },
			new short[2] { 104, 33 },
			new short[2] { 105, 33 },
			new short[2] { 106, 33 },
			new short[2] { 107, 33 },
			new short[2] { 108, 33 },
			new short[2] { 115, 40 },
			new short[2] { 116, 40 },
			new short[2] { 117, 40 },
			new short[2] { 124, 30 }
		};
		List<short[]> bigBaseBlockCore45 = new List<short[]>
		{
			new short[2] { 72, 1 },
			new short[2] { 61, 1 },
			new short[2] { 62, 1 },
			new short[2] { 84, 1 },
			new short[2] { 66, 1 }
		};
		list = new List<short[]>();
		List<short[]> seriesBlockCore45 = list;
		list = new List<short[]>();
		List<short[]> encircleBlockCore45 = list;
		short[] sceneryBlockCore45 = new short[0];
		list2 = new List<short>();
		List<short> lovingItemSubTypes45 = list2;
		list2 = new List<short>();
		dataArray45.Add(new MapAreaItem(104, 11, 436, 437, 20, worldMapPos45, neighborAreas45, "largemap_part_1_changgui_2", miniMapIcons45, null, miniMapPos45, 0, 0, 5, enemyNests45, enemyNestCreationDateRanges45, settlementBlockCore45, organizationId45, -1, 0, null, developedBlockCore45, normalBlockCore45, wildBlockCore45, bigBaseBlockCore45, seriesBlockCore45, encircleBlockCore45, sceneryBlockCore45, 438, 439, null, null, EMapAreaAreaDirection.South, lovingItemSubTypes45, list2, showDarkAshStatus: true));
		List<MapAreaItem> dataArray46 = _dataArray;
		sbyte[] worldMapPos46 = new sbyte[2] { 6, 15 };
		AreaTravelRoute[] neighborAreas46 = new AreaTravelRoute[0];
		string[] miniMapIcons46 = new string[4] { "largemap_part_2_icon_hei_0", "largemap_part_2_icon_jin_0", "largemap_part_2_icon_lan_0", "largemap_part_2_icon_hui_0" };
		short[] miniMapPos46 = new short[2] { 240, 180 };
		List<EnemyNestCreationInfo[]> enemyNests46 = new List<EnemyNestCreationInfo[]>
		{
			new EnemyNestCreationInfo[2]
			{
				new EnemyNestCreationInfo(0, 2),
				new EnemyNestCreationInfo(1, 2)
			},
			new EnemyNestCreationInfo[2]
			{
				new EnemyNestCreationInfo(1, 2),
				new EnemyNestCreationInfo(2, 3)
			},
			new EnemyNestCreationInfo[4]
			{
				new EnemyNestCreationInfo(2, 3),
				new EnemyNestCreationInfo(3, 3),
				new EnemyNestCreationInfo(4, 4),
				new EnemyNestCreationInfo(5, 4)
			},
			new EnemyNestCreationInfo[1]
			{
				new EnemyNestCreationInfo(12, 2)
			}
		};
		List<IntPair> enemyNestCreationDateRanges46 = new List<IntPair>
		{
			new IntPair(0, 6),
			new IntPair(6, 12),
			new IntPair(12, 24),
			new IntPair(0, 0)
		};
		short[] settlementBlockCore46 = new short[1] { 34 };
		sbyte[] organizationId46 = new sbyte[1] { 36 };
		List<short[]> developedBlockCore46 = new List<short[]>
		{
			new short[2] { 39, 33 },
			new short[2] { 40, 33 },
			new short[2] { 41, 33 },
			new short[2] { 42, 33 },
			new short[2] { 43, 33 },
			new short[2] { 44, 33 },
			new short[2] { 45, 33 },
			new short[2] { 46, 33 },
			new short[2] { 47, 33 },
			new short[2] { 48, 33 },
			new short[2] { 49, 33 },
			new short[2] { 50, 33 },
			new short[2] { 51, 33 },
			new short[2] { 52, 33 },
			new short[2] { 53, 33 },
			new short[2] { 54, 33 },
			new short[2] { 55, 33 },
			new short[2] { 56, 33 },
			new short[2] { 109, 10 },
			new short[2] { 110, 10 },
			new short[2] { 111, 10 }
		};
		List<short[]> normalBlockCore46 = new List<short[]>
		{
			new short[2] { 57, 33 },
			new short[2] { 58, 33 },
			new short[2] { 59, 33 },
			new short[2] { 63, 33 },
			new short[2] { 64, 33 },
			new short[2] { 65, 33 },
			new short[2] { 69, 33 },
			new short[2] { 70, 33 },
			new short[2] { 71, 33 },
			new short[2] { 75, 33 },
			new short[2] { 76, 33 },
			new short[2] { 77, 33 },
			new short[2] { 81, 33 },
			new short[2] { 82, 33 },
			new short[2] { 83, 33 },
			new short[2] { 87, 33 },
			new short[2] { 88, 33 },
			new short[2] { 89, 33 },
			new short[2] { 112, 30 },
			new short[2] { 113, 30 },
			new short[2] { 114, 30 },
			new short[2] { 124, 30 }
		};
		List<short[]> wildBlockCore46 = new List<short[]>
		{
			new short[2] { 93, 33 },
			new short[2] { 94, 33 },
			new short[2] { 95, 33 },
			new short[2] { 96, 33 },
			new short[2] { 97, 33 },
			new short[2] { 98, 33 },
			new short[2] { 99, 33 },
			new short[2] { 100, 33 },
			new short[2] { 101, 33 },
			new short[2] { 102, 33 },
			new short[2] { 103, 33 },
			new short[2] { 104, 33 },
			new short[2] { 105, 33 },
			new short[2] { 106, 33 },
			new short[2] { 107, 33 },
			new short[2] { 108, 33 },
			new short[2] { 115, 40 },
			new short[2] { 116, 40 },
			new short[2] { 117, 40 },
			new short[2] { 124, 30 }
		};
		List<short[]> bigBaseBlockCore46 = new List<short[]>
		{
			new short[2] { 72, 1 },
			new short[2] { 61, 1 },
			new short[2] { 62, 1 },
			new short[2] { 84, 1 },
			new short[2] { 66, 1 }
		};
		list = new List<short[]>();
		List<short[]> seriesBlockCore46 = list;
		list = new List<short[]>();
		List<short[]> encircleBlockCore46 = list;
		short[] sceneryBlockCore46 = new short[0];
		list2 = new List<short>();
		List<short> lovingItemSubTypes46 = list2;
		list2 = new List<short>();
		dataArray46.Add(new MapAreaItem(105, 11, 440, 441, 20, worldMapPos46, neighborAreas46, "largemap_part_1_changgui_0", miniMapIcons46, null, miniMapPos46, 0, 0, 5, enemyNests46, enemyNestCreationDateRanges46, settlementBlockCore46, organizationId46, -1, 0, null, developedBlockCore46, normalBlockCore46, wildBlockCore46, bigBaseBlockCore46, seriesBlockCore46, encircleBlockCore46, sceneryBlockCore46, 442, 443, null, null, EMapAreaAreaDirection.South, lovingItemSubTypes46, list2, showDarkAshStatus: true));
		List<MapAreaItem> dataArray47 = _dataArray;
		sbyte[] worldMapPos47 = new sbyte[2] { 3, 13 };
		AreaTravelRoute[] neighborAreas47 = new AreaTravelRoute[0];
		string[] miniMapIcons47 = new string[4] { "largemap_part_2_icon_hei_0", "largemap_part_2_icon_jin_0", "largemap_part_2_icon_lan_0", "largemap_part_2_icon_hui_0" };
		short[] miniMapPos47 = new short[2] { 180, 110 };
		List<EnemyNestCreationInfo[]> enemyNests47 = new List<EnemyNestCreationInfo[]>
		{
			new EnemyNestCreationInfo[2]
			{
				new EnemyNestCreationInfo(0, 2),
				new EnemyNestCreationInfo(1, 2)
			},
			new EnemyNestCreationInfo[2]
			{
				new EnemyNestCreationInfo(1, 2),
				new EnemyNestCreationInfo(2, 3)
			},
			new EnemyNestCreationInfo[4]
			{
				new EnemyNestCreationInfo(2, 3),
				new EnemyNestCreationInfo(3, 3),
				new EnemyNestCreationInfo(4, 4),
				new EnemyNestCreationInfo(5, 4)
			},
			new EnemyNestCreationInfo[1]
			{
				new EnemyNestCreationInfo(12, 2)
			}
		};
		List<IntPair> enemyNestCreationDateRanges47 = new List<IntPair>
		{
			new IntPair(0, 6),
			new IntPair(6, 12),
			new IntPair(12, 24),
			new IntPair(0, 0)
		};
		short[] settlementBlockCore47 = new short[1] { 34 };
		sbyte[] organizationId47 = new sbyte[1] { 36 };
		List<short[]> developedBlockCore47 = new List<short[]>
		{
			new short[2] { 39, 33 },
			new short[2] { 40, 33 },
			new short[2] { 41, 33 },
			new short[2] { 42, 33 },
			new short[2] { 43, 33 },
			new short[2] { 44, 33 },
			new short[2] { 45, 33 },
			new short[2] { 46, 33 },
			new short[2] { 47, 33 },
			new short[2] { 48, 33 },
			new short[2] { 49, 33 },
			new short[2] { 50, 33 },
			new short[2] { 51, 33 },
			new short[2] { 52, 33 },
			new short[2] { 53, 33 },
			new short[2] { 54, 33 },
			new short[2] { 55, 33 },
			new short[2] { 56, 33 },
			new short[2] { 109, 10 },
			new short[2] { 110, 10 },
			new short[2] { 111, 10 }
		};
		List<short[]> normalBlockCore47 = new List<short[]>
		{
			new short[2] { 57, 33 },
			new short[2] { 58, 33 },
			new short[2] { 59, 33 },
			new short[2] { 63, 33 },
			new short[2] { 64, 33 },
			new short[2] { 65, 33 },
			new short[2] { 69, 33 },
			new short[2] { 70, 33 },
			new short[2] { 71, 33 },
			new short[2] { 75, 33 },
			new short[2] { 76, 33 },
			new short[2] { 77, 33 },
			new short[2] { 81, 33 },
			new short[2] { 82, 33 },
			new short[2] { 83, 33 },
			new short[2] { 87, 33 },
			new short[2] { 88, 33 },
			new short[2] { 89, 33 },
			new short[2] { 112, 30 },
			new short[2] { 113, 30 },
			new short[2] { 114, 30 },
			new short[2] { 124, 30 }
		};
		List<short[]> wildBlockCore47 = new List<short[]>
		{
			new short[2] { 93, 33 },
			new short[2] { 94, 33 },
			new short[2] { 95, 33 },
			new short[2] { 96, 33 },
			new short[2] { 97, 33 },
			new short[2] { 98, 33 },
			new short[2] { 99, 33 },
			new short[2] { 100, 33 },
			new short[2] { 101, 33 },
			new short[2] { 102, 33 },
			new short[2] { 103, 33 },
			new short[2] { 104, 33 },
			new short[2] { 105, 33 },
			new short[2] { 106, 33 },
			new short[2] { 107, 33 },
			new short[2] { 108, 33 },
			new short[2] { 115, 40 },
			new short[2] { 116, 40 },
			new short[2] { 117, 40 },
			new short[2] { 124, 30 }
		};
		List<short[]> bigBaseBlockCore47 = new List<short[]>
		{
			new short[2] { 72, 1 },
			new short[2] { 61, 1 },
			new short[2] { 62, 1 },
			new short[2] { 84, 1 },
			new short[2] { 66, 1 }
		};
		list = new List<short[]>();
		List<short[]> seriesBlockCore47 = list;
		list = new List<short[]>();
		List<short[]> encircleBlockCore47 = list;
		short[] sceneryBlockCore47 = new short[0];
		list2 = new List<short>();
		List<short> lovingItemSubTypes47 = list2;
		list2 = new List<short>();
		dataArray47.Add(new MapAreaItem(106, 11, 444, 445, 20, worldMapPos47, neighborAreas47, "largemap_part_1_changgui_0", miniMapIcons47, null, miniMapPos47, 0, 0, 5, enemyNests47, enemyNestCreationDateRanges47, settlementBlockCore47, organizationId47, -1, 0, null, developedBlockCore47, normalBlockCore47, wildBlockCore47, bigBaseBlockCore47, seriesBlockCore47, encircleBlockCore47, sceneryBlockCore47, 446, 447, null, null, EMapAreaAreaDirection.South, lovingItemSubTypes47, list2, showDarkAshStatus: true));
		List<MapAreaItem> dataArray48 = _dataArray;
		sbyte[] worldMapPos48 = new sbyte[2] { 7, 14 };
		AreaTravelRoute[] neighborAreas48 = new AreaTravelRoute[0];
		string[] miniMapIcons48 = new string[4] { "largemap_part_2_icon_hei_2", "largemap_part_2_icon_jin_2", "largemap_part_2_icon_lan_2", "largemap_part_2_icon_hui_2" };
		short[] miniMapPos48 = new short[2] { 280, 150 };
		List<EnemyNestCreationInfo[]> enemyNests48 = new List<EnemyNestCreationInfo[]>
		{
			new EnemyNestCreationInfo[2]
			{
				new EnemyNestCreationInfo(0, 2),
				new EnemyNestCreationInfo(1, 2)
			},
			new EnemyNestCreationInfo[2]
			{
				new EnemyNestCreationInfo(1, 2),
				new EnemyNestCreationInfo(2, 3)
			},
			new EnemyNestCreationInfo[4]
			{
				new EnemyNestCreationInfo(2, 3),
				new EnemyNestCreationInfo(3, 3),
				new EnemyNestCreationInfo(4, 4),
				new EnemyNestCreationInfo(5, 4)
			},
			new EnemyNestCreationInfo[1]
			{
				new EnemyNestCreationInfo(12, 2)
			}
		};
		List<IntPair> enemyNestCreationDateRanges48 = new List<IntPair>
		{
			new IntPair(0, 6),
			new IntPair(6, 12),
			new IntPair(12, 24),
			new IntPair(0, 0)
		};
		short[] settlementBlockCore48 = new short[1] { 34 };
		sbyte[] organizationId48 = new sbyte[1] { 36 };
		List<short[]> developedBlockCore48 = new List<short[]>
		{
			new short[2] { 39, 33 },
			new short[2] { 40, 33 },
			new short[2] { 41, 33 },
			new short[2] { 42, 33 },
			new short[2] { 43, 33 },
			new short[2] { 44, 33 },
			new short[2] { 45, 33 },
			new short[2] { 46, 33 },
			new short[2] { 47, 33 },
			new short[2] { 48, 33 },
			new short[2] { 49, 33 },
			new short[2] { 50, 33 },
			new short[2] { 51, 33 },
			new short[2] { 52, 33 },
			new short[2] { 53, 33 },
			new short[2] { 54, 33 },
			new short[2] { 55, 33 },
			new short[2] { 56, 33 },
			new short[2] { 109, 10 },
			new short[2] { 110, 10 },
			new short[2] { 111, 10 }
		};
		List<short[]> normalBlockCore48 = new List<short[]>
		{
			new short[2] { 57, 33 },
			new short[2] { 58, 33 },
			new short[2] { 59, 33 },
			new short[2] { 63, 33 },
			new short[2] { 64, 33 },
			new short[2] { 65, 33 },
			new short[2] { 69, 33 },
			new short[2] { 70, 33 },
			new short[2] { 71, 33 },
			new short[2] { 75, 33 },
			new short[2] { 76, 33 },
			new short[2] { 77, 33 },
			new short[2] { 81, 33 },
			new short[2] { 82, 33 },
			new short[2] { 83, 33 },
			new short[2] { 87, 33 },
			new short[2] { 88, 33 },
			new short[2] { 89, 33 },
			new short[2] { 112, 30 },
			new short[2] { 113, 30 },
			new short[2] { 114, 30 },
			new short[2] { 124, 30 }
		};
		List<short[]> wildBlockCore48 = new List<short[]>
		{
			new short[2] { 93, 33 },
			new short[2] { 94, 33 },
			new short[2] { 95, 33 },
			new short[2] { 96, 33 },
			new short[2] { 97, 33 },
			new short[2] { 98, 33 },
			new short[2] { 99, 33 },
			new short[2] { 100, 33 },
			new short[2] { 101, 33 },
			new short[2] { 102, 33 },
			new short[2] { 103, 33 },
			new short[2] { 104, 33 },
			new short[2] { 105, 33 },
			new short[2] { 106, 33 },
			new short[2] { 107, 33 },
			new short[2] { 108, 33 },
			new short[2] { 115, 40 },
			new short[2] { 116, 40 },
			new short[2] { 117, 40 },
			new short[2] { 124, 30 }
		};
		List<short[]> bigBaseBlockCore48 = new List<short[]>
		{
			new short[2] { 72, 1 },
			new short[2] { 61, 1 },
			new short[2] { 62, 1 },
			new short[2] { 84, 1 },
			new short[2] { 66, 1 }
		};
		list = new List<short[]>();
		List<short[]> seriesBlockCore48 = list;
		list = new List<short[]>();
		List<short[]> encircleBlockCore48 = list;
		short[] sceneryBlockCore48 = new short[0];
		list2 = new List<short>();
		List<short> lovingItemSubTypes48 = list2;
		list2 = new List<short>();
		dataArray48.Add(new MapAreaItem(107, 11, 448, 449, 20, worldMapPos48, neighborAreas48, "largemap_part_1_changgui_2", miniMapIcons48, null, miniMapPos48, 0, 0, 5, enemyNests48, enemyNestCreationDateRanges48, settlementBlockCore48, organizationId48, -1, 0, null, developedBlockCore48, normalBlockCore48, wildBlockCore48, bigBaseBlockCore48, seriesBlockCore48, encircleBlockCore48, sceneryBlockCore48, 450, 451, null, null, EMapAreaAreaDirection.South, lovingItemSubTypes48, list2, showDarkAshStatus: true));
		List<MapAreaItem> dataArray49 = _dataArray;
		sbyte[] worldMapPos49 = new sbyte[2] { 4, 1 };
		AreaTravelRoute[] neighborAreas49 = new AreaTravelRoute[2]
		{
			new AreaTravelRoute(109, 6, null),
			new AreaTravelRoute(112, 16, new int[2] { 3, 1 })
		};
		string[] miniMapIcons49 = new string[4] { "largemap_part_2_icon_hei_1", "largemap_part_2_icon_jin_1", "largemap_part_2_icon_lan_1", "largemap_part_2_icon_hui_1" };
		short[] miniMapPos49 = new short[2] { 260, 100 };
		List<EnemyNestCreationInfo[]> enemyNests49 = new List<EnemyNestCreationInfo[]>
		{
			new EnemyNestCreationInfo[2]
			{
				new EnemyNestCreationInfo(0, 2),
				new EnemyNestCreationInfo(1, 2)
			},
			new EnemyNestCreationInfo[2]
			{
				new EnemyNestCreationInfo(1, 2),
				new EnemyNestCreationInfo(2, 3)
			},
			new EnemyNestCreationInfo[4]
			{
				new EnemyNestCreationInfo(2, 3),
				new EnemyNestCreationInfo(3, 3),
				new EnemyNestCreationInfo(4, 4),
				new EnemyNestCreationInfo(5, 4)
			},
			new EnemyNestCreationInfo[1]
			{
				new EnemyNestCreationInfo(12, 2)
			}
		};
		List<IntPair> enemyNestCreationDateRanges49 = new List<IntPair>
		{
			new IntPair(0, 6),
			new IntPair(6, 12),
			new IntPair(12, 24),
			new IntPair(0, 0)
		};
		short[] settlementBlockCore49 = new short[1] { 36 };
		sbyte[] organizationId49 = new sbyte[1] { 38 };
		List<short[]> developedBlockCore49 = new List<short[]>
		{
			new short[2] { 39, 33 },
			new short[2] { 40, 33 },
			new short[2] { 41, 33 },
			new short[2] { 42, 33 },
			new short[2] { 43, 33 },
			new short[2] { 44, 33 },
			new short[2] { 45, 33 },
			new short[2] { 46, 33 },
			new short[2] { 47, 33 },
			new short[2] { 48, 33 },
			new short[2] { 49, 33 },
			new short[2] { 50, 33 },
			new short[2] { 51, 33 },
			new short[2] { 52, 33 },
			new short[2] { 53, 33 },
			new short[2] { 54, 33 },
			new short[2] { 55, 33 },
			new short[2] { 56, 33 },
			new short[2] { 109, 10 },
			new short[2] { 110, 10 },
			new short[2] { 111, 10 }
		};
		List<short[]> normalBlockCore49 = new List<short[]>
		{
			new short[2] { 57, 33 },
			new short[2] { 58, 33 },
			new short[2] { 59, 33 },
			new short[2] { 63, 33 },
			new short[2] { 64, 33 },
			new short[2] { 65, 33 },
			new short[2] { 69, 33 },
			new short[2] { 70, 33 },
			new short[2] { 71, 33 },
			new short[2] { 75, 33 },
			new short[2] { 76, 33 },
			new short[2] { 77, 33 },
			new short[2] { 81, 33 },
			new short[2] { 82, 33 },
			new short[2] { 83, 33 },
			new short[2] { 87, 33 },
			new short[2] { 88, 33 },
			new short[2] { 89, 33 },
			new short[2] { 112, 30 },
			new short[2] { 113, 30 },
			new short[2] { 114, 30 },
			new short[2] { 124, 30 }
		};
		List<short[]> wildBlockCore49 = new List<short[]>
		{
			new short[2] { 93, 33 },
			new short[2] { 94, 33 },
			new short[2] { 95, 33 },
			new short[2] { 96, 33 },
			new short[2] { 97, 33 },
			new short[2] { 98, 33 },
			new short[2] { 99, 33 },
			new short[2] { 100, 33 },
			new short[2] { 101, 33 },
			new short[2] { 102, 33 },
			new short[2] { 103, 33 },
			new short[2] { 104, 33 },
			new short[2] { 105, 33 },
			new short[2] { 106, 33 },
			new short[2] { 107, 33 },
			new short[2] { 108, 33 },
			new short[2] { 115, 40 },
			new short[2] { 116, 40 },
			new short[2] { 117, 40 },
			new short[2] { 124, 30 }
		};
		List<short[]> bigBaseBlockCore49 = new List<short[]>
		{
			new short[2] { 72, 1 },
			new short[2] { 60, 1 },
			new short[2] { 84, 1 },
			new short[2] { 67, 1 },
			new short[2] { 68, 1 }
		};
		list = new List<short[]>();
		List<short[]> seriesBlockCore49 = list;
		list = new List<short[]>();
		List<short[]> encircleBlockCore49 = list;
		short[] sceneryBlockCore49 = new short[0];
		list2 = new List<short>();
		List<short> lovingItemSubTypes49 = list2;
		list2 = new List<short>();
		dataArray49.Add(new MapAreaItem(108, 12, 452, 453, 20, worldMapPos49, neighborAreas49, "largemap_part_1_changgui_1", miniMapIcons49, null, miniMapPos49, 0, 0, 5, enemyNests49, enemyNestCreationDateRanges49, settlementBlockCore49, organizationId49, -1, 0, null, developedBlockCore49, normalBlockCore49, wildBlockCore49, bigBaseBlockCore49, seriesBlockCore49, encircleBlockCore49, sceneryBlockCore49, 454, 455, null, null, EMapAreaAreaDirection.South, lovingItemSubTypes49, list2, showDarkAshStatus: true));
		List<MapAreaItem> dataArray50 = _dataArray;
		sbyte[] worldMapPos50 = new sbyte[2] { 4, 2 };
		AreaTravelRoute[] neighborAreas50 = new AreaTravelRoute[0];
		string[] miniMapIcons50 = new string[4] { "largemap_part_2_icon_hei_2", "largemap_part_2_icon_jin_2", "largemap_part_2_icon_lan_2", "largemap_part_2_icon_hui_2" };
		short[] miniMapPos50 = new short[2] { 260, 140 };
		List<EnemyNestCreationInfo[]> enemyNests50 = new List<EnemyNestCreationInfo[]>
		{
			new EnemyNestCreationInfo[2]
			{
				new EnemyNestCreationInfo(0, 2),
				new EnemyNestCreationInfo(1, 2)
			},
			new EnemyNestCreationInfo[2]
			{
				new EnemyNestCreationInfo(1, 2),
				new EnemyNestCreationInfo(2, 3)
			},
			new EnemyNestCreationInfo[4]
			{
				new EnemyNestCreationInfo(2, 3),
				new EnemyNestCreationInfo(3, 3),
				new EnemyNestCreationInfo(4, 4),
				new EnemyNestCreationInfo(5, 4)
			},
			new EnemyNestCreationInfo[1]
			{
				new EnemyNestCreationInfo(12, 2)
			}
		};
		List<IntPair> enemyNestCreationDateRanges50 = new List<IntPair>
		{
			new IntPair(0, 6),
			new IntPair(6, 12),
			new IntPair(12, 24),
			new IntPair(0, 0)
		};
		short[] settlementBlockCore50 = new short[1] { 35 };
		sbyte[] organizationId50 = new sbyte[1] { 37 };
		List<short[]> developedBlockCore50 = new List<short[]>
		{
			new short[2] { 39, 33 },
			new short[2] { 40, 33 },
			new short[2] { 41, 33 },
			new short[2] { 42, 33 },
			new short[2] { 43, 33 },
			new short[2] { 44, 33 },
			new short[2] { 45, 33 },
			new short[2] { 46, 33 },
			new short[2] { 47, 33 },
			new short[2] { 48, 33 },
			new short[2] { 49, 33 },
			new short[2] { 50, 33 },
			new short[2] { 51, 33 },
			new short[2] { 52, 33 },
			new short[2] { 53, 33 },
			new short[2] { 54, 33 },
			new short[2] { 55, 33 },
			new short[2] { 56, 33 },
			new short[2] { 109, 10 },
			new short[2] { 110, 10 },
			new short[2] { 111, 10 }
		};
		List<short[]> normalBlockCore50 = new List<short[]>
		{
			new short[2] { 57, 33 },
			new short[2] { 58, 33 },
			new short[2] { 59, 33 },
			new short[2] { 63, 33 },
			new short[2] { 64, 33 },
			new short[2] { 65, 33 },
			new short[2] { 69, 33 },
			new short[2] { 70, 33 },
			new short[2] { 71, 33 },
			new short[2] { 75, 33 },
			new short[2] { 76, 33 },
			new short[2] { 77, 33 },
			new short[2] { 81, 33 },
			new short[2] { 82, 33 },
			new short[2] { 83, 33 },
			new short[2] { 87, 33 },
			new short[2] { 88, 33 },
			new short[2] { 89, 33 },
			new short[2] { 112, 30 },
			new short[2] { 113, 30 },
			new short[2] { 114, 30 },
			new short[2] { 124, 30 }
		};
		List<short[]> wildBlockCore50 = new List<short[]>
		{
			new short[2] { 93, 33 },
			new short[2] { 94, 33 },
			new short[2] { 95, 33 },
			new short[2] { 96, 33 },
			new short[2] { 97, 33 },
			new short[2] { 98, 33 },
			new short[2] { 99, 33 },
			new short[2] { 100, 33 },
			new short[2] { 101, 33 },
			new short[2] { 102, 33 },
			new short[2] { 103, 33 },
			new short[2] { 104, 33 },
			new short[2] { 105, 33 },
			new short[2] { 106, 33 },
			new short[2] { 107, 33 },
			new short[2] { 108, 33 },
			new short[2] { 115, 40 },
			new short[2] { 116, 40 },
			new short[2] { 117, 40 },
			new short[2] { 124, 30 }
		};
		List<short[]> bigBaseBlockCore50 = new List<short[]>
		{
			new short[2] { 72, 1 },
			new short[2] { 60, 1 },
			new short[2] { 84, 1 },
			new short[2] { 67, 1 },
			new short[2] { 68, 1 }
		};
		list = new List<short[]>();
		List<short[]> seriesBlockCore50 = list;
		list = new List<short[]>();
		List<short[]> encircleBlockCore50 = list;
		short[] sceneryBlockCore50 = new short[0];
		list2 = new List<short>();
		List<short> lovingItemSubTypes50 = list2;
		list2 = new List<short>();
		dataArray50.Add(new MapAreaItem(109, 12, 456, 457, 20, worldMapPos50, neighborAreas50, "largemap_part_1_changgui_2", miniMapIcons50, null, miniMapPos50, 0, 0, 5, enemyNests50, enemyNestCreationDateRanges50, settlementBlockCore50, organizationId50, -1, 0, null, developedBlockCore50, normalBlockCore50, wildBlockCore50, bigBaseBlockCore50, seriesBlockCore50, encircleBlockCore50, sceneryBlockCore50, 458, 459, null, null, EMapAreaAreaDirection.South, lovingItemSubTypes50, list2, showDarkAshStatus: true));
		List<MapAreaItem> dataArray51 = _dataArray;
		sbyte[] worldMapPos51 = new sbyte[2] { 1, 1 };
		AreaTravelRoute[] neighborAreas51 = new AreaTravelRoute[2]
		{
			new AreaTravelRoute(112, 8, null),
			new AreaTravelRoute(113, 16, new int[2] { 0, 1 })
		};
		string[] miniMapIcons51 = new string[4] { "largemap_part_2_icon_hei_1", "largemap_part_2_icon_jin_1", "largemap_part_2_icon_lan_1", "largemap_part_2_icon_hui_1" };
		short[] miniMapPos51 = new short[2] { 140, 100 };
		List<EnemyNestCreationInfo[]> enemyNests51 = new List<EnemyNestCreationInfo[]>
		{
			new EnemyNestCreationInfo[2]
			{
				new EnemyNestCreationInfo(0, 2),
				new EnemyNestCreationInfo(1, 2)
			},
			new EnemyNestCreationInfo[2]
			{
				new EnemyNestCreationInfo(1, 2),
				new EnemyNestCreationInfo(2, 3)
			},
			new EnemyNestCreationInfo[4]
			{
				new EnemyNestCreationInfo(2, 3),
				new EnemyNestCreationInfo(3, 3),
				new EnemyNestCreationInfo(4, 4),
				new EnemyNestCreationInfo(5, 4)
			},
			new EnemyNestCreationInfo[1]
			{
				new EnemyNestCreationInfo(12, 2)
			}
		};
		List<IntPair> enemyNestCreationDateRanges51 = new List<IntPair>
		{
			new IntPair(0, 6),
			new IntPair(6, 12),
			new IntPair(12, 24),
			new IntPair(0, 0)
		};
		short[] settlementBlockCore51 = new short[1] { 34 };
		sbyte[] organizationId51 = new sbyte[1] { 36 };
		List<short[]> developedBlockCore51 = new List<short[]>
		{
			new short[2] { 39, 33 },
			new short[2] { 40, 33 },
			new short[2] { 41, 33 },
			new short[2] { 42, 33 },
			new short[2] { 43, 33 },
			new short[2] { 44, 33 },
			new short[2] { 45, 33 },
			new short[2] { 46, 33 },
			new short[2] { 47, 33 },
			new short[2] { 48, 33 },
			new short[2] { 49, 33 },
			new short[2] { 50, 33 },
			new short[2] { 51, 33 },
			new short[2] { 52, 33 },
			new short[2] { 53, 33 },
			new short[2] { 54, 33 },
			new short[2] { 55, 33 },
			new short[2] { 56, 33 },
			new short[2] { 109, 10 },
			new short[2] { 110, 10 },
			new short[2] { 111, 10 }
		};
		List<short[]> normalBlockCore51 = new List<short[]>
		{
			new short[2] { 57, 33 },
			new short[2] { 58, 33 },
			new short[2] { 59, 33 },
			new short[2] { 63, 33 },
			new short[2] { 64, 33 },
			new short[2] { 65, 33 },
			new short[2] { 69, 33 },
			new short[2] { 70, 33 },
			new short[2] { 71, 33 },
			new short[2] { 75, 33 },
			new short[2] { 76, 33 },
			new short[2] { 77, 33 },
			new short[2] { 81, 33 },
			new short[2] { 82, 33 },
			new short[2] { 83, 33 },
			new short[2] { 87, 33 },
			new short[2] { 88, 33 },
			new short[2] { 89, 33 },
			new short[2] { 112, 30 },
			new short[2] { 113, 30 },
			new short[2] { 114, 30 },
			new short[2] { 124, 30 }
		};
		List<short[]> wildBlockCore51 = new List<short[]>
		{
			new short[2] { 93, 33 },
			new short[2] { 94, 33 },
			new short[2] { 95, 33 },
			new short[2] { 96, 33 },
			new short[2] { 97, 33 },
			new short[2] { 98, 33 },
			new short[2] { 99, 33 },
			new short[2] { 100, 33 },
			new short[2] { 101, 33 },
			new short[2] { 102, 33 },
			new short[2] { 103, 33 },
			new short[2] { 104, 33 },
			new short[2] { 105, 33 },
			new short[2] { 106, 33 },
			new short[2] { 107, 33 },
			new short[2] { 108, 33 },
			new short[2] { 115, 40 },
			new short[2] { 116, 40 },
			new short[2] { 117, 40 },
			new short[2] { 124, 30 }
		};
		List<short[]> bigBaseBlockCore51 = new List<short[]>
		{
			new short[2] { 72, 1 },
			new short[2] { 60, 1 },
			new short[2] { 84, 1 },
			new short[2] { 67, 1 },
			new short[2] { 68, 1 }
		};
		list = new List<short[]>();
		List<short[]> seriesBlockCore51 = list;
		list = new List<short[]>();
		List<short[]> encircleBlockCore51 = list;
		short[] sceneryBlockCore51 = new short[0];
		list2 = new List<short>();
		List<short> lovingItemSubTypes51 = list2;
		list2 = new List<short>();
		dataArray51.Add(new MapAreaItem(110, 12, 460, 461, 20, worldMapPos51, neighborAreas51, "largemap_part_1_changgui_1", miniMapIcons51, null, miniMapPos51, 0, 0, 5, enemyNests51, enemyNestCreationDateRanges51, settlementBlockCore51, organizationId51, -1, 0, null, developedBlockCore51, normalBlockCore51, wildBlockCore51, bigBaseBlockCore51, seriesBlockCore51, encircleBlockCore51, sceneryBlockCore51, 462, 463, null, null, EMapAreaAreaDirection.South, lovingItemSubTypes51, list2, showDarkAshStatus: true));
		List<MapAreaItem> dataArray52 = _dataArray;
		sbyte[] worldMapPos52 = new sbyte[2] { 1, 3 };
		AreaTravelRoute[] neighborAreas52 = new AreaTravelRoute[1]
		{
			new AreaTravelRoute(113, 14, new int[2] { 0, 3 })
		};
		string[] miniMapIcons52 = new string[4] { "largemap_part_2_icon_hei_1", "largemap_part_2_icon_jin_1", "largemap_part_2_icon_lan_1", "largemap_part_2_icon_hui_1" };
		short[] miniMapPos52 = new short[2] { 140, 180 };
		List<EnemyNestCreationInfo[]> enemyNests52 = new List<EnemyNestCreationInfo[]>
		{
			new EnemyNestCreationInfo[2]
			{
				new EnemyNestCreationInfo(0, 2),
				new EnemyNestCreationInfo(1, 2)
			},
			new EnemyNestCreationInfo[2]
			{
				new EnemyNestCreationInfo(1, 2),
				new EnemyNestCreationInfo(2, 3)
			},
			new EnemyNestCreationInfo[4]
			{
				new EnemyNestCreationInfo(2, 3),
				new EnemyNestCreationInfo(3, 3),
				new EnemyNestCreationInfo(4, 4),
				new EnemyNestCreationInfo(5, 4)
			},
			new EnemyNestCreationInfo[1]
			{
				new EnemyNestCreationInfo(12, 2)
			}
		};
		List<IntPair> enemyNestCreationDateRanges52 = new List<IntPair>
		{
			new IntPair(0, 6),
			new IntPair(6, 12),
			new IntPair(12, 24),
			new IntPair(0, 0)
		};
		short[] settlementBlockCore52 = new short[1] { 34 };
		sbyte[] organizationId52 = new sbyte[1] { 36 };
		List<short[]> developedBlockCore52 = new List<short[]>
		{
			new short[2] { 39, 33 },
			new short[2] { 40, 33 },
			new short[2] { 41, 33 },
			new short[2] { 42, 33 },
			new short[2] { 43, 33 },
			new short[2] { 44, 33 },
			new short[2] { 45, 33 },
			new short[2] { 46, 33 },
			new short[2] { 47, 33 },
			new short[2] { 48, 33 },
			new short[2] { 49, 33 },
			new short[2] { 50, 33 },
			new short[2] { 51, 33 },
			new short[2] { 52, 33 },
			new short[2] { 53, 33 },
			new short[2] { 54, 33 },
			new short[2] { 55, 33 },
			new short[2] { 56, 33 },
			new short[2] { 109, 10 },
			new short[2] { 110, 10 },
			new short[2] { 111, 10 }
		};
		List<short[]> normalBlockCore52 = new List<short[]>
		{
			new short[2] { 57, 33 },
			new short[2] { 58, 33 },
			new short[2] { 59, 33 },
			new short[2] { 63, 33 },
			new short[2] { 64, 33 },
			new short[2] { 65, 33 },
			new short[2] { 69, 33 },
			new short[2] { 70, 33 },
			new short[2] { 71, 33 },
			new short[2] { 75, 33 },
			new short[2] { 76, 33 },
			new short[2] { 77, 33 },
			new short[2] { 81, 33 },
			new short[2] { 82, 33 },
			new short[2] { 83, 33 },
			new short[2] { 87, 33 },
			new short[2] { 88, 33 },
			new short[2] { 89, 33 },
			new short[2] { 112, 30 },
			new short[2] { 113, 30 },
			new short[2] { 114, 30 },
			new short[2] { 124, 30 }
		};
		List<short[]> wildBlockCore52 = new List<short[]>
		{
			new short[2] { 93, 33 },
			new short[2] { 94, 33 },
			new short[2] { 95, 33 },
			new short[2] { 96, 33 },
			new short[2] { 97, 33 },
			new short[2] { 98, 33 },
			new short[2] { 99, 33 },
			new short[2] { 100, 33 },
			new short[2] { 101, 33 },
			new short[2] { 102, 33 },
			new short[2] { 103, 33 },
			new short[2] { 104, 33 },
			new short[2] { 105, 33 },
			new short[2] { 106, 33 },
			new short[2] { 107, 33 },
			new short[2] { 108, 33 },
			new short[2] { 115, 40 },
			new short[2] { 116, 40 },
			new short[2] { 117, 40 },
			new short[2] { 124, 30 }
		};
		List<short[]> bigBaseBlockCore52 = new List<short[]>
		{
			new short[2] { 72, 1 },
			new short[2] { 60, 1 },
			new short[2] { 84, 1 },
			new short[2] { 67, 1 },
			new short[2] { 68, 1 }
		};
		list = new List<short[]>();
		List<short[]> seriesBlockCore52 = list;
		list = new List<short[]>();
		List<short[]> encircleBlockCore52 = list;
		short[] sceneryBlockCore52 = new short[0];
		list2 = new List<short>();
		List<short> lovingItemSubTypes52 = list2;
		list2 = new List<short>();
		dataArray52.Add(new MapAreaItem(111, 12, 464, 465, 20, worldMapPos52, neighborAreas52, "largemap_part_1_changgui_1", miniMapIcons52, null, miniMapPos52, 0, 0, 5, enemyNests52, enemyNestCreationDateRanges52, settlementBlockCore52, organizationId52, -1, 0, null, developedBlockCore52, normalBlockCore52, wildBlockCore52, bigBaseBlockCore52, seriesBlockCore52, encircleBlockCore52, sceneryBlockCore52, 466, 467, null, null, EMapAreaAreaDirection.South, lovingItemSubTypes52, list2, showDarkAshStatus: true));
		List<MapAreaItem> dataArray53 = _dataArray;
		sbyte[] worldMapPos53 = new sbyte[2] { 2, 1 };
		AreaTravelRoute[] neighborAreas53 = new AreaTravelRoute[0];
		string[] miniMapIcons53 = new string[4] { "largemap_part_2_icon_hei_0", "largemap_part_2_icon_jin_0", "largemap_part_2_icon_lan_0", "largemap_part_2_icon_hui_0" };
		short[] miniMapPos53 = new short[2] { 180, 100 };
		List<EnemyNestCreationInfo[]> enemyNests53 = new List<EnemyNestCreationInfo[]>
		{
			new EnemyNestCreationInfo[2]
			{
				new EnemyNestCreationInfo(0, 2),
				new EnemyNestCreationInfo(1, 2)
			},
			new EnemyNestCreationInfo[2]
			{
				new EnemyNestCreationInfo(1, 2),
				new EnemyNestCreationInfo(2, 3)
			},
			new EnemyNestCreationInfo[4]
			{
				new EnemyNestCreationInfo(2, 3),
				new EnemyNestCreationInfo(3, 3),
				new EnemyNestCreationInfo(4, 4),
				new EnemyNestCreationInfo(5, 4)
			},
			new EnemyNestCreationInfo[1]
			{
				new EnemyNestCreationInfo(12, 2)
			}
		};
		List<IntPair> enemyNestCreationDateRanges53 = new List<IntPair>
		{
			new IntPair(0, 6),
			new IntPair(6, 12),
			new IntPair(12, 24),
			new IntPair(0, 0)
		};
		short[] settlementBlockCore53 = new short[1] { 36 };
		sbyte[] organizationId53 = new sbyte[1] { 38 };
		List<short[]> developedBlockCore53 = new List<short[]>
		{
			new short[2] { 39, 33 },
			new short[2] { 40, 33 },
			new short[2] { 41, 33 },
			new short[2] { 42, 33 },
			new short[2] { 43, 33 },
			new short[2] { 44, 33 },
			new short[2] { 45, 33 },
			new short[2] { 46, 33 },
			new short[2] { 47, 33 },
			new short[2] { 48, 33 },
			new short[2] { 49, 33 },
			new short[2] { 50, 33 },
			new short[2] { 51, 33 },
			new short[2] { 52, 33 },
			new short[2] { 53, 33 },
			new short[2] { 54, 33 },
			new short[2] { 55, 33 },
			new short[2] { 56, 33 },
			new short[2] { 109, 10 },
			new short[2] { 110, 10 },
			new short[2] { 111, 10 }
		};
		List<short[]> normalBlockCore53 = new List<short[]>
		{
			new short[2] { 57, 33 },
			new short[2] { 58, 33 },
			new short[2] { 59, 33 },
			new short[2] { 63, 33 },
			new short[2] { 64, 33 },
			new short[2] { 65, 33 },
			new short[2] { 69, 33 },
			new short[2] { 70, 33 },
			new short[2] { 71, 33 },
			new short[2] { 75, 33 },
			new short[2] { 76, 33 },
			new short[2] { 77, 33 },
			new short[2] { 81, 33 },
			new short[2] { 82, 33 },
			new short[2] { 83, 33 },
			new short[2] { 87, 33 },
			new short[2] { 88, 33 },
			new short[2] { 89, 33 },
			new short[2] { 112, 30 },
			new short[2] { 113, 30 },
			new short[2] { 114, 30 },
			new short[2] { 124, 30 }
		};
		List<short[]> wildBlockCore53 = new List<short[]>
		{
			new short[2] { 93, 33 },
			new short[2] { 94, 33 },
			new short[2] { 95, 33 },
			new short[2] { 96, 33 },
			new short[2] { 97, 33 },
			new short[2] { 98, 33 },
			new short[2] { 99, 33 },
			new short[2] { 100, 33 },
			new short[2] { 101, 33 },
			new short[2] { 102, 33 },
			new short[2] { 103, 33 },
			new short[2] { 104, 33 },
			new short[2] { 105, 33 },
			new short[2] { 106, 33 },
			new short[2] { 107, 33 },
			new short[2] { 108, 33 },
			new short[2] { 115, 40 },
			new short[2] { 116, 40 },
			new short[2] { 117, 40 },
			new short[2] { 124, 30 }
		};
		List<short[]> bigBaseBlockCore53 = new List<short[]>
		{
			new short[2] { 72, 1 },
			new short[2] { 60, 1 },
			new short[2] { 84, 1 },
			new short[2] { 67, 1 },
			new short[2] { 68, 1 }
		};
		list = new List<short[]>();
		List<short[]> seriesBlockCore53 = list;
		list = new List<short[]>();
		List<short[]> encircleBlockCore53 = list;
		short[] sceneryBlockCore53 = new short[0];
		list2 = new List<short>();
		List<short> lovingItemSubTypes53 = list2;
		list2 = new List<short>();
		dataArray53.Add(new MapAreaItem(112, 12, 468, 469, 20, worldMapPos53, neighborAreas53, "largemap_part_1_changgui_0", miniMapIcons53, null, miniMapPos53, 0, 0, 5, enemyNests53, enemyNestCreationDateRanges53, settlementBlockCore53, organizationId53, -1, 0, null, developedBlockCore53, normalBlockCore53, wildBlockCore53, bigBaseBlockCore53, seriesBlockCore53, encircleBlockCore53, sceneryBlockCore53, 470, 471, null, null, EMapAreaAreaDirection.South, lovingItemSubTypes53, list2, showDarkAshStatus: true));
		List<MapAreaItem> dataArray54 = _dataArray;
		sbyte[] worldMapPos54 = new sbyte[2] { 0, 2 };
		AreaTravelRoute[] neighborAreas54 = new AreaTravelRoute[0];
		string[] miniMapIcons54 = new string[4] { "largemap_part_2_icon_hei_1", "largemap_part_2_icon_jin_1", "largemap_part_2_icon_lan_1", "largemap_part_2_icon_hui_1" };
		short[] miniMapPos54 = new short[2] { 100, 140 };
		List<EnemyNestCreationInfo[]> enemyNests54 = new List<EnemyNestCreationInfo[]>
		{
			new EnemyNestCreationInfo[2]
			{
				new EnemyNestCreationInfo(0, 2),
				new EnemyNestCreationInfo(1, 2)
			},
			new EnemyNestCreationInfo[2]
			{
				new EnemyNestCreationInfo(1, 2),
				new EnemyNestCreationInfo(2, 3)
			},
			new EnemyNestCreationInfo[4]
			{
				new EnemyNestCreationInfo(2, 3),
				new EnemyNestCreationInfo(3, 3),
				new EnemyNestCreationInfo(4, 4),
				new EnemyNestCreationInfo(5, 4)
			},
			new EnemyNestCreationInfo[1]
			{
				new EnemyNestCreationInfo(12, 2)
			}
		};
		List<IntPair> enemyNestCreationDateRanges54 = new List<IntPair>
		{
			new IntPair(0, 6),
			new IntPair(6, 12),
			new IntPair(12, 24),
			new IntPair(0, 0)
		};
		short[] settlementBlockCore54 = new short[1] { 34 };
		sbyte[] organizationId54 = new sbyte[1] { 36 };
		List<short[]> developedBlockCore54 = new List<short[]>
		{
			new short[2] { 39, 33 },
			new short[2] { 40, 33 },
			new short[2] { 41, 33 },
			new short[2] { 42, 33 },
			new short[2] { 43, 33 },
			new short[2] { 44, 33 },
			new short[2] { 45, 33 },
			new short[2] { 46, 33 },
			new short[2] { 47, 33 },
			new short[2] { 48, 33 },
			new short[2] { 49, 33 },
			new short[2] { 50, 33 },
			new short[2] { 51, 33 },
			new short[2] { 52, 33 },
			new short[2] { 53, 33 },
			new short[2] { 54, 33 },
			new short[2] { 55, 33 },
			new short[2] { 56, 33 },
			new short[2] { 109, 10 },
			new short[2] { 110, 10 },
			new short[2] { 111, 10 }
		};
		List<short[]> normalBlockCore54 = new List<short[]>
		{
			new short[2] { 57, 33 },
			new short[2] { 58, 33 },
			new short[2] { 59, 33 },
			new short[2] { 63, 33 },
			new short[2] { 64, 33 },
			new short[2] { 65, 33 },
			new short[2] { 69, 33 },
			new short[2] { 70, 33 },
			new short[2] { 71, 33 },
			new short[2] { 75, 33 },
			new short[2] { 76, 33 },
			new short[2] { 77, 33 },
			new short[2] { 81, 33 },
			new short[2] { 82, 33 },
			new short[2] { 83, 33 },
			new short[2] { 87, 33 },
			new short[2] { 88, 33 },
			new short[2] { 89, 33 },
			new short[2] { 112, 30 },
			new short[2] { 113, 30 },
			new short[2] { 114, 30 },
			new short[2] { 124, 30 }
		};
		List<short[]> wildBlockCore54 = new List<short[]>
		{
			new short[2] { 93, 33 },
			new short[2] { 94, 33 },
			new short[2] { 95, 33 },
			new short[2] { 96, 33 },
			new short[2] { 97, 33 },
			new short[2] { 98, 33 },
			new short[2] { 99, 33 },
			new short[2] { 100, 33 },
			new short[2] { 101, 33 },
			new short[2] { 102, 33 },
			new short[2] { 103, 33 },
			new short[2] { 104, 33 },
			new short[2] { 105, 33 },
			new short[2] { 106, 33 },
			new short[2] { 107, 33 },
			new short[2] { 108, 33 },
			new short[2] { 115, 40 },
			new short[2] { 116, 40 },
			new short[2] { 117, 40 },
			new short[2] { 124, 30 }
		};
		List<short[]> bigBaseBlockCore54 = new List<short[]>
		{
			new short[2] { 72, 1 },
			new short[2] { 60, 1 },
			new short[2] { 84, 1 },
			new short[2] { 67, 1 },
			new short[2] { 68, 1 }
		};
		list = new List<short[]>();
		List<short[]> seriesBlockCore54 = list;
		list = new List<short[]>();
		List<short[]> encircleBlockCore54 = list;
		short[] sceneryBlockCore54 = new short[0];
		list2 = new List<short>();
		List<short> lovingItemSubTypes54 = list2;
		list2 = new List<short>();
		dataArray54.Add(new MapAreaItem(113, 12, 472, 473, 20, worldMapPos54, neighborAreas54, "largemap_part_1_changgui_1", miniMapIcons54, null, miniMapPos54, 0, 0, 5, enemyNests54, enemyNestCreationDateRanges54, settlementBlockCore54, organizationId54, -1, 0, null, developedBlockCore54, normalBlockCore54, wildBlockCore54, bigBaseBlockCore54, seriesBlockCore54, encircleBlockCore54, sceneryBlockCore54, 474, 475, null, null, EMapAreaAreaDirection.South, lovingItemSubTypes54, list2, showDarkAshStatus: true));
		List<MapAreaItem> dataArray55 = _dataArray;
		sbyte[] worldMapPos55 = new sbyte[2] { 4, 4 };
		AreaTravelRoute[] neighborAreas55 = new AreaTravelRoute[0];
		string[] miniMapIcons55 = new string[4] { "largemap_part_2_icon_hei_2", "largemap_part_2_icon_jin_2", "largemap_part_2_icon_lan_2", "largemap_part_2_icon_hui_2" };
		short[] miniMapPos55 = new short[2] { 260, 220 };
		List<EnemyNestCreationInfo[]> enemyNests55 = new List<EnemyNestCreationInfo[]>
		{
			new EnemyNestCreationInfo[2]
			{
				new EnemyNestCreationInfo(0, 2),
				new EnemyNestCreationInfo(1, 2)
			},
			new EnemyNestCreationInfo[2]
			{
				new EnemyNestCreationInfo(1, 2),
				new EnemyNestCreationInfo(2, 3)
			},
			new EnemyNestCreationInfo[4]
			{
				new EnemyNestCreationInfo(2, 3),
				new EnemyNestCreationInfo(3, 3),
				new EnemyNestCreationInfo(4, 4),
				new EnemyNestCreationInfo(5, 4)
			},
			new EnemyNestCreationInfo[1]
			{
				new EnemyNestCreationInfo(12, 2)
			}
		};
		List<IntPair> enemyNestCreationDateRanges55 = new List<IntPair>
		{
			new IntPair(0, 6),
			new IntPair(6, 12),
			new IntPair(12, 24),
			new IntPair(0, 0)
		};
		short[] settlementBlockCore55 = new short[1] { 34 };
		sbyte[] organizationId55 = new sbyte[1] { 36 };
		List<short[]> developedBlockCore55 = new List<short[]>
		{
			new short[2] { 39, 33 },
			new short[2] { 40, 33 },
			new short[2] { 41, 33 },
			new short[2] { 42, 33 },
			new short[2] { 43, 33 },
			new short[2] { 44, 33 },
			new short[2] { 45, 33 },
			new short[2] { 46, 33 },
			new short[2] { 47, 33 },
			new short[2] { 48, 33 },
			new short[2] { 49, 33 },
			new short[2] { 50, 33 },
			new short[2] { 51, 33 },
			new short[2] { 52, 33 },
			new short[2] { 53, 33 },
			new short[2] { 54, 33 },
			new short[2] { 55, 33 },
			new short[2] { 56, 33 },
			new short[2] { 109, 10 },
			new short[2] { 110, 10 },
			new short[2] { 111, 10 }
		};
		List<short[]> normalBlockCore55 = new List<short[]>
		{
			new short[2] { 57, 33 },
			new short[2] { 58, 33 },
			new short[2] { 59, 33 },
			new short[2] { 63, 33 },
			new short[2] { 64, 33 },
			new short[2] { 65, 33 },
			new short[2] { 69, 33 },
			new short[2] { 70, 33 },
			new short[2] { 71, 33 },
			new short[2] { 75, 33 },
			new short[2] { 76, 33 },
			new short[2] { 77, 33 },
			new short[2] { 81, 33 },
			new short[2] { 82, 33 },
			new short[2] { 83, 33 },
			new short[2] { 87, 33 },
			new short[2] { 88, 33 },
			new short[2] { 89, 33 },
			new short[2] { 112, 30 },
			new short[2] { 113, 30 },
			new short[2] { 114, 30 },
			new short[2] { 124, 30 }
		};
		List<short[]> wildBlockCore55 = new List<short[]>
		{
			new short[2] { 93, 33 },
			new short[2] { 94, 33 },
			new short[2] { 95, 33 },
			new short[2] { 96, 33 },
			new short[2] { 97, 33 },
			new short[2] { 98, 33 },
			new short[2] { 99, 33 },
			new short[2] { 100, 33 },
			new short[2] { 101, 33 },
			new short[2] { 102, 33 },
			new short[2] { 103, 33 },
			new short[2] { 104, 33 },
			new short[2] { 105, 33 },
			new short[2] { 106, 33 },
			new short[2] { 107, 33 },
			new short[2] { 108, 33 },
			new short[2] { 115, 40 },
			new short[2] { 116, 40 },
			new short[2] { 117, 40 },
			new short[2] { 124, 30 }
		};
		List<short[]> bigBaseBlockCore55 = new List<short[]>
		{
			new short[2] { 72, 1 },
			new short[2] { 60, 1 },
			new short[2] { 84, 1 },
			new short[2] { 67, 1 },
			new short[2] { 68, 1 }
		};
		list = new List<short[]>();
		List<short[]> seriesBlockCore55 = list;
		list = new List<short[]>();
		List<short[]> encircleBlockCore55 = list;
		short[] sceneryBlockCore55 = new short[0];
		list2 = new List<short>();
		List<short> lovingItemSubTypes55 = list2;
		list2 = new List<short>();
		dataArray55.Add(new MapAreaItem(114, 12, 476, 477, 20, worldMapPos55, neighborAreas55, "largemap_part_1_changgui_2", miniMapIcons55, null, miniMapPos55, 0, 0, 5, enemyNests55, enemyNestCreationDateRanges55, settlementBlockCore55, organizationId55, -1, 0, null, developedBlockCore55, normalBlockCore55, wildBlockCore55, bigBaseBlockCore55, seriesBlockCore55, encircleBlockCore55, sceneryBlockCore55, 478, 479, null, null, EMapAreaAreaDirection.South, lovingItemSubTypes55, list2, showDarkAshStatus: true));
		List<MapAreaItem> dataArray56 = _dataArray;
		sbyte[] worldMapPos56 = new sbyte[2] { 17, 12 };
		AreaTravelRoute[] neighborAreas56 = new AreaTravelRoute[2]
		{
			new AreaTravelRoute(120, 6, null),
			new AreaTravelRoute(121, 6, null)
		};
		string[] miniMapIcons56 = new string[4] { "largemap_part_2_icon_hei_2", "largemap_part_2_icon_jin_2", "largemap_part_2_icon_lan_2", "largemap_part_2_icon_hui_2" };
		short[] miniMapPos56 = new short[2] { 180, 220 };
		List<EnemyNestCreationInfo[]> enemyNests56 = new List<EnemyNestCreationInfo[]>
		{
			new EnemyNestCreationInfo[2]
			{
				new EnemyNestCreationInfo(0, 2),
				new EnemyNestCreationInfo(1, 2)
			},
			new EnemyNestCreationInfo[2]
			{
				new EnemyNestCreationInfo(1, 2),
				new EnemyNestCreationInfo(2, 3)
			},
			new EnemyNestCreationInfo[4]
			{
				new EnemyNestCreationInfo(2, 3),
				new EnemyNestCreationInfo(3, 3),
				new EnemyNestCreationInfo(4, 4),
				new EnemyNestCreationInfo(5, 4)
			},
			new EnemyNestCreationInfo[1]
			{
				new EnemyNestCreationInfo(12, 2)
			}
		};
		List<IntPair> enemyNestCreationDateRanges56 = new List<IntPair>
		{
			new IntPair(0, 6),
			new IntPair(6, 12),
			new IntPair(12, 24),
			new IntPair(0, 0)
		};
		short[] settlementBlockCore56 = new short[1] { 35 };
		sbyte[] organizationId56 = new sbyte[1] { 37 };
		List<short[]> developedBlockCore56 = new List<short[]>
		{
			new short[2] { 39, 33 },
			new short[2] { 40, 33 },
			new short[2] { 41, 33 },
			new short[2] { 42, 33 },
			new short[2] { 43, 33 },
			new short[2] { 44, 33 },
			new short[2] { 45, 33 },
			new short[2] { 46, 33 },
			new short[2] { 47, 33 },
			new short[2] { 48, 33 },
			new short[2] { 49, 33 },
			new short[2] { 50, 33 },
			new short[2] { 51, 33 },
			new short[2] { 52, 33 },
			new short[2] { 53, 33 },
			new short[2] { 54, 33 },
			new short[2] { 55, 33 },
			new short[2] { 56, 33 },
			new short[2] { 109, 10 },
			new short[2] { 110, 10 },
			new short[2] { 111, 10 }
		};
		List<short[]> normalBlockCore56 = new List<short[]>
		{
			new short[2] { 57, 33 },
			new short[2] { 58, 33 },
			new short[2] { 59, 33 },
			new short[2] { 63, 33 },
			new short[2] { 64, 33 },
			new short[2] { 65, 33 },
			new short[2] { 69, 33 },
			new short[2] { 70, 33 },
			new short[2] { 71, 33 },
			new short[2] { 75, 33 },
			new short[2] { 76, 33 },
			new short[2] { 77, 33 },
			new short[2] { 81, 33 },
			new short[2] { 82, 33 },
			new short[2] { 83, 33 },
			new short[2] { 87, 33 },
			new short[2] { 88, 33 },
			new short[2] { 89, 33 },
			new short[2] { 112, 30 },
			new short[2] { 113, 30 },
			new short[2] { 114, 30 },
			new short[2] { 124, 30 }
		};
		List<short[]> wildBlockCore56 = new List<short[]>
		{
			new short[2] { 93, 33 },
			new short[2] { 94, 33 },
			new short[2] { 95, 33 },
			new short[2] { 96, 33 },
			new short[2] { 97, 33 },
			new short[2] { 98, 33 },
			new short[2] { 99, 33 },
			new short[2] { 100, 33 },
			new short[2] { 101, 33 },
			new short[2] { 102, 33 },
			new short[2] { 103, 33 },
			new short[2] { 104, 33 },
			new short[2] { 105, 33 },
			new short[2] { 106, 33 },
			new short[2] { 107, 33 },
			new short[2] { 108, 33 },
			new short[2] { 115, 40 },
			new short[2] { 116, 40 },
			new short[2] { 117, 40 },
			new short[2] { 124, 30 }
		};
		List<short[]> bigBaseBlockCore56 = new List<short[]>
		{
			new short[2] { 78, 1 },
			new short[2] { 84, 1 },
			new short[2] { 66, 1 },
			new short[2] { 91, 1 },
			new short[2] { 92, 1 }
		};
		list = new List<short[]>();
		List<short[]> seriesBlockCore56 = list;
		list = new List<short[]>();
		List<short[]> encircleBlockCore56 = list;
		short[] sceneryBlockCore56 = new short[0];
		list2 = new List<short>();
		List<short> lovingItemSubTypes56 = list2;
		list2 = new List<short>();
		dataArray56.Add(new MapAreaItem(115, 13, 480, 481, 20, worldMapPos56, neighborAreas56, "largemap_part_1_changgui_2", miniMapIcons56, null, miniMapPos56, 0, 0, 5, enemyNests56, enemyNestCreationDateRanges56, settlementBlockCore56, organizationId56, -1, 0, null, developedBlockCore56, normalBlockCore56, wildBlockCore56, bigBaseBlockCore56, seriesBlockCore56, encircleBlockCore56, sceneryBlockCore56, 482, 483, null, null, EMapAreaAreaDirection.South, lovingItemSubTypes56, list2, showDarkAshStatus: true));
		List<MapAreaItem> dataArray57 = _dataArray;
		sbyte[] worldMapPos57 = new sbyte[2] { 18, 10 };
		AreaTravelRoute[] neighborAreas57 = new AreaTravelRoute[2]
		{
			new AreaTravelRoute(118, 14, new int[2] { 17, 10 }),
			new AreaTravelRoute(120, 12, new int[2] { 17, 10 })
		};
		string[] miniMapIcons57 = new string[4] { "largemap_part_2_icon_hei_0", "largemap_part_2_icon_jin_0", "largemap_part_2_icon_lan_0", "largemap_part_2_icon_hui_0" };
		short[] miniMapPos57 = new short[2] { 220, 180 };
		List<EnemyNestCreationInfo[]> enemyNests57 = new List<EnemyNestCreationInfo[]>
		{
			new EnemyNestCreationInfo[2]
			{
				new EnemyNestCreationInfo(0, 2),
				new EnemyNestCreationInfo(1, 2)
			},
			new EnemyNestCreationInfo[2]
			{
				new EnemyNestCreationInfo(1, 2),
				new EnemyNestCreationInfo(2, 3)
			},
			new EnemyNestCreationInfo[4]
			{
				new EnemyNestCreationInfo(2, 3),
				new EnemyNestCreationInfo(3, 3),
				new EnemyNestCreationInfo(4, 4),
				new EnemyNestCreationInfo(5, 4)
			},
			new EnemyNestCreationInfo[1]
			{
				new EnemyNestCreationInfo(12, 2)
			}
		};
		List<IntPair> enemyNestCreationDateRanges57 = new List<IntPair>
		{
			new IntPair(0, 6),
			new IntPair(6, 12),
			new IntPair(12, 24),
			new IntPair(0, 0)
		};
		short[] settlementBlockCore57 = new short[1] { 36 };
		sbyte[] organizationId57 = new sbyte[1] { 38 };
		List<short[]> developedBlockCore57 = new List<short[]>
		{
			new short[2] { 39, 33 },
			new short[2] { 40, 33 },
			new short[2] { 41, 33 },
			new short[2] { 42, 33 },
			new short[2] { 43, 33 },
			new short[2] { 44, 33 },
			new short[2] { 45, 33 },
			new short[2] { 46, 33 },
			new short[2] { 47, 33 },
			new short[2] { 48, 33 },
			new short[2] { 49, 33 },
			new short[2] { 50, 33 },
			new short[2] { 51, 33 },
			new short[2] { 52, 33 },
			new short[2] { 53, 33 },
			new short[2] { 54, 33 },
			new short[2] { 55, 33 },
			new short[2] { 56, 33 },
			new short[2] { 109, 10 },
			new short[2] { 110, 10 },
			new short[2] { 111, 10 }
		};
		List<short[]> normalBlockCore57 = new List<short[]>
		{
			new short[2] { 57, 33 },
			new short[2] { 58, 33 },
			new short[2] { 59, 33 },
			new short[2] { 63, 33 },
			new short[2] { 64, 33 },
			new short[2] { 65, 33 },
			new short[2] { 69, 33 },
			new short[2] { 70, 33 },
			new short[2] { 71, 33 },
			new short[2] { 75, 33 },
			new short[2] { 76, 33 },
			new short[2] { 77, 33 },
			new short[2] { 81, 33 },
			new short[2] { 82, 33 },
			new short[2] { 83, 33 },
			new short[2] { 87, 33 },
			new short[2] { 88, 33 },
			new short[2] { 89, 33 },
			new short[2] { 112, 30 },
			new short[2] { 113, 30 },
			new short[2] { 114, 30 },
			new short[2] { 124, 30 }
		};
		List<short[]> wildBlockCore57 = new List<short[]>
		{
			new short[2] { 93, 33 },
			new short[2] { 94, 33 },
			new short[2] { 95, 33 },
			new short[2] { 96, 33 },
			new short[2] { 97, 33 },
			new short[2] { 98, 33 },
			new short[2] { 99, 33 },
			new short[2] { 100, 33 },
			new short[2] { 101, 33 },
			new short[2] { 102, 33 },
			new short[2] { 103, 33 },
			new short[2] { 104, 33 },
			new short[2] { 105, 33 },
			new short[2] { 106, 33 },
			new short[2] { 107, 33 },
			new short[2] { 108, 33 },
			new short[2] { 115, 40 },
			new short[2] { 116, 40 },
			new short[2] { 117, 40 },
			new short[2] { 124, 30 }
		};
		List<short[]> bigBaseBlockCore57 = new List<short[]>
		{
			new short[2] { 78, 1 },
			new short[2] { 84, 1 },
			new short[2] { 66, 1 },
			new short[2] { 91, 1 },
			new short[2] { 92, 1 }
		};
		list = new List<short[]>();
		List<short[]> seriesBlockCore57 = list;
		list = new List<short[]>();
		List<short[]> encircleBlockCore57 = list;
		short[] sceneryBlockCore57 = new short[0];
		list2 = new List<short>();
		List<short> lovingItemSubTypes57 = list2;
		list2 = new List<short>();
		dataArray57.Add(new MapAreaItem(116, 13, 484, 485, 20, worldMapPos57, neighborAreas57, "largemap_part_1_changgui_0", miniMapIcons57, null, miniMapPos57, 0, 0, 5, enemyNests57, enemyNestCreationDateRanges57, settlementBlockCore57, organizationId57, -1, 0, null, developedBlockCore57, normalBlockCore57, wildBlockCore57, bigBaseBlockCore57, seriesBlockCore57, encircleBlockCore57, sceneryBlockCore57, 486, 487, null, null, EMapAreaAreaDirection.South, lovingItemSubTypes57, list2, showDarkAshStatus: true));
		List<MapAreaItem> dataArray58 = _dataArray;
		sbyte[] worldMapPos58 = new sbyte[2] { 16, 13 };
		AreaTravelRoute[] neighborAreas58 = new AreaTravelRoute[1]
		{
			new AreaTravelRoute(132, 32, new int[6] { 16, 14, 16, 15, 17, 15 })
		};
		string[] miniMapIcons58 = new string[4] { "largemap_part_2_icon_hei_1", "largemap_part_2_icon_jin_1", "largemap_part_2_icon_lan_1", "largemap_part_2_icon_hui_1" };
		short[] miniMapPos58 = new short[2] { 140, 260 };
		List<EnemyNestCreationInfo[]> enemyNests58 = new List<EnemyNestCreationInfo[]>
		{
			new EnemyNestCreationInfo[2]
			{
				new EnemyNestCreationInfo(0, 2),
				new EnemyNestCreationInfo(1, 2)
			},
			new EnemyNestCreationInfo[2]
			{
				new EnemyNestCreationInfo(1, 2),
				new EnemyNestCreationInfo(2, 3)
			},
			new EnemyNestCreationInfo[4]
			{
				new EnemyNestCreationInfo(2, 3),
				new EnemyNestCreationInfo(3, 3),
				new EnemyNestCreationInfo(4, 4),
				new EnemyNestCreationInfo(5, 4)
			},
			new EnemyNestCreationInfo[1]
			{
				new EnemyNestCreationInfo(12, 2)
			}
		};
		List<IntPair> enemyNestCreationDateRanges58 = new List<IntPair>
		{
			new IntPair(0, 6),
			new IntPair(6, 12),
			new IntPair(12, 24),
			new IntPair(0, 0)
		};
		short[] settlementBlockCore58 = new short[1] { 35 };
		sbyte[] organizationId58 = new sbyte[1] { 37 };
		List<short[]> developedBlockCore58 = new List<short[]>
		{
			new short[2] { 39, 33 },
			new short[2] { 40, 33 },
			new short[2] { 41, 33 },
			new short[2] { 42, 33 },
			new short[2] { 43, 33 },
			new short[2] { 44, 33 },
			new short[2] { 45, 33 },
			new short[2] { 46, 33 },
			new short[2] { 47, 33 },
			new short[2] { 48, 33 },
			new short[2] { 49, 33 },
			new short[2] { 50, 33 },
			new short[2] { 51, 33 },
			new short[2] { 52, 33 },
			new short[2] { 53, 33 },
			new short[2] { 54, 33 },
			new short[2] { 55, 33 },
			new short[2] { 56, 33 },
			new short[2] { 109, 10 },
			new short[2] { 110, 10 },
			new short[2] { 111, 10 }
		};
		List<short[]> normalBlockCore58 = new List<short[]>
		{
			new short[2] { 57, 33 },
			new short[2] { 58, 33 },
			new short[2] { 59, 33 },
			new short[2] { 63, 33 },
			new short[2] { 64, 33 },
			new short[2] { 65, 33 },
			new short[2] { 69, 33 },
			new short[2] { 70, 33 },
			new short[2] { 71, 33 },
			new short[2] { 75, 33 },
			new short[2] { 76, 33 },
			new short[2] { 77, 33 },
			new short[2] { 81, 33 },
			new short[2] { 82, 33 },
			new short[2] { 83, 33 },
			new short[2] { 87, 33 },
			new short[2] { 88, 33 },
			new short[2] { 89, 33 },
			new short[2] { 112, 30 },
			new short[2] { 113, 30 },
			new short[2] { 114, 30 },
			new short[2] { 124, 30 }
		};
		List<short[]> wildBlockCore58 = new List<short[]>
		{
			new short[2] { 93, 33 },
			new short[2] { 94, 33 },
			new short[2] { 95, 33 },
			new short[2] { 96, 33 },
			new short[2] { 97, 33 },
			new short[2] { 98, 33 },
			new short[2] { 99, 33 },
			new short[2] { 100, 33 },
			new short[2] { 101, 33 },
			new short[2] { 102, 33 },
			new short[2] { 103, 33 },
			new short[2] { 104, 33 },
			new short[2] { 105, 33 },
			new short[2] { 106, 33 },
			new short[2] { 107, 33 },
			new short[2] { 108, 33 },
			new short[2] { 115, 40 },
			new short[2] { 116, 40 },
			new short[2] { 117, 40 },
			new short[2] { 124, 30 }
		};
		List<short[]> bigBaseBlockCore58 = new List<short[]>
		{
			new short[2] { 78, 1 },
			new short[2] { 84, 1 },
			new short[2] { 66, 1 },
			new short[2] { 91, 1 },
			new short[2] { 92, 1 }
		};
		list = new List<short[]>();
		List<short[]> seriesBlockCore58 = list;
		list = new List<short[]>();
		List<short[]> encircleBlockCore58 = list;
		short[] sceneryBlockCore58 = new short[0];
		list2 = new List<short>();
		List<short> lovingItemSubTypes58 = list2;
		list2 = new List<short>();
		dataArray58.Add(new MapAreaItem(117, 13, 488, 489, 20, worldMapPos58, neighborAreas58, "largemap_part_1_changgui_1", miniMapIcons58, null, miniMapPos58, 0, 0, 5, enemyNests58, enemyNestCreationDateRanges58, settlementBlockCore58, organizationId58, -1, 0, null, developedBlockCore58, normalBlockCore58, wildBlockCore58, bigBaseBlockCore58, seriesBlockCore58, encircleBlockCore58, sceneryBlockCore58, 490, 491, null, null, EMapAreaAreaDirection.South, lovingItemSubTypes58, list2, showDarkAshStatus: true));
		List<MapAreaItem> dataArray59 = _dataArray;
		sbyte[] worldMapPos59 = new sbyte[2] { 17, 9 };
		AreaTravelRoute[] neighborAreas59 = new AreaTravelRoute[2]
		{
			new AreaTravelRoute(120, 14, new int[2] { 17, 10 }),
			new AreaTravelRoute(127, 16, new int[2] { 18, 9 })
		};
		string[] miniMapIcons59 = new string[4] { "largemap_part_2_icon_hei_0", "largemap_part_2_icon_jin_0", "largemap_part_2_icon_lan_0", "largemap_part_2_icon_hui_0" };
		short[] miniMapPos59 = new short[2] { 180, 140 };
		List<EnemyNestCreationInfo[]> enemyNests59 = new List<EnemyNestCreationInfo[]>
		{
			new EnemyNestCreationInfo[2]
			{
				new EnemyNestCreationInfo(0, 2),
				new EnemyNestCreationInfo(1, 2)
			},
			new EnemyNestCreationInfo[2]
			{
				new EnemyNestCreationInfo(1, 2),
				new EnemyNestCreationInfo(2, 3)
			},
			new EnemyNestCreationInfo[4]
			{
				new EnemyNestCreationInfo(2, 3),
				new EnemyNestCreationInfo(3, 3),
				new EnemyNestCreationInfo(4, 4),
				new EnemyNestCreationInfo(5, 4)
			},
			new EnemyNestCreationInfo[1]
			{
				new EnemyNestCreationInfo(12, 2)
			}
		};
		List<IntPair> enemyNestCreationDateRanges59 = new List<IntPair>
		{
			new IntPair(0, 6),
			new IntPair(6, 12),
			new IntPair(12, 24),
			new IntPair(0, 0)
		};
		short[] settlementBlockCore59 = new short[1] { 35 };
		sbyte[] organizationId59 = new sbyte[1] { 37 };
		List<short[]> developedBlockCore59 = new List<short[]>
		{
			new short[2] { 39, 33 },
			new short[2] { 40, 33 },
			new short[2] { 41, 33 },
			new short[2] { 42, 33 },
			new short[2] { 43, 33 },
			new short[2] { 44, 33 },
			new short[2] { 45, 33 },
			new short[2] { 46, 33 },
			new short[2] { 47, 33 },
			new short[2] { 48, 33 },
			new short[2] { 49, 33 },
			new short[2] { 50, 33 },
			new short[2] { 51, 33 },
			new short[2] { 52, 33 },
			new short[2] { 53, 33 },
			new short[2] { 54, 33 },
			new short[2] { 55, 33 },
			new short[2] { 56, 33 },
			new short[2] { 109, 10 },
			new short[2] { 110, 10 },
			new short[2] { 111, 10 }
		};
		List<short[]> normalBlockCore59 = new List<short[]>
		{
			new short[2] { 57, 33 },
			new short[2] { 58, 33 },
			new short[2] { 59, 33 },
			new short[2] { 63, 33 },
			new short[2] { 64, 33 },
			new short[2] { 65, 33 },
			new short[2] { 69, 33 },
			new short[2] { 70, 33 },
			new short[2] { 71, 33 },
			new short[2] { 75, 33 },
			new short[2] { 76, 33 },
			new short[2] { 77, 33 },
			new short[2] { 81, 33 },
			new short[2] { 82, 33 },
			new short[2] { 83, 33 },
			new short[2] { 87, 33 },
			new short[2] { 88, 33 },
			new short[2] { 89, 33 },
			new short[2] { 112, 30 },
			new short[2] { 113, 30 },
			new short[2] { 114, 30 },
			new short[2] { 124, 30 }
		};
		List<short[]> wildBlockCore59 = new List<short[]>
		{
			new short[2] { 93, 33 },
			new short[2] { 94, 33 },
			new short[2] { 95, 33 },
			new short[2] { 96, 33 },
			new short[2] { 97, 33 },
			new short[2] { 98, 33 },
			new short[2] { 99, 33 },
			new short[2] { 100, 33 },
			new short[2] { 101, 33 },
			new short[2] { 102, 33 },
			new short[2] { 103, 33 },
			new short[2] { 104, 33 },
			new short[2] { 105, 33 },
			new short[2] { 106, 33 },
			new short[2] { 107, 33 },
			new short[2] { 108, 33 },
			new short[2] { 115, 40 },
			new short[2] { 116, 40 },
			new short[2] { 117, 40 },
			new short[2] { 124, 30 }
		};
		List<short[]> bigBaseBlockCore59 = new List<short[]>
		{
			new short[2] { 78, 1 },
			new short[2] { 84, 1 },
			new short[2] { 66, 1 },
			new short[2] { 91, 1 },
			new short[2] { 92, 1 }
		};
		list = new List<short[]>();
		List<short[]> seriesBlockCore59 = list;
		list = new List<short[]>();
		List<short[]> encircleBlockCore59 = list;
		short[] sceneryBlockCore59 = new short[0];
		list2 = new List<short>();
		List<short> lovingItemSubTypes59 = list2;
		list2 = new List<short>();
		dataArray59.Add(new MapAreaItem(118, 13, 492, 493, 20, worldMapPos59, neighborAreas59, "largemap_part_1_changgui_0", miniMapIcons59, null, miniMapPos59, 0, 0, 5, enemyNests59, enemyNestCreationDateRanges59, settlementBlockCore59, organizationId59, -1, 0, null, developedBlockCore59, normalBlockCore59, wildBlockCore59, bigBaseBlockCore59, seriesBlockCore59, encircleBlockCore59, sceneryBlockCore59, 494, 495, null, null, EMapAreaAreaDirection.South, lovingItemSubTypes59, list2, showDarkAshStatus: true));
		List<MapAreaItem> dataArray60 = _dataArray;
		sbyte[] worldMapPos60 = new sbyte[2] { 18, 11 };
		AreaTravelRoute[] neighborAreas60 = new AreaTravelRoute[1]
		{
			new AreaTravelRoute(120, 6, null)
		};
		string[] miniMapIcons60 = new string[4] { "largemap_part_2_icon_hei_0", "largemap_part_2_icon_jin_0", "largemap_part_2_icon_lan_0", "largemap_part_2_icon_hui_0" };
		short[] miniMapPos60 = new short[2] { 220, 220 };
		List<EnemyNestCreationInfo[]> enemyNests60 = new List<EnemyNestCreationInfo[]>
		{
			new EnemyNestCreationInfo[2]
			{
				new EnemyNestCreationInfo(0, 2),
				new EnemyNestCreationInfo(1, 2)
			},
			new EnemyNestCreationInfo[2]
			{
				new EnemyNestCreationInfo(1, 2),
				new EnemyNestCreationInfo(2, 3)
			},
			new EnemyNestCreationInfo[4]
			{
				new EnemyNestCreationInfo(2, 3),
				new EnemyNestCreationInfo(3, 3),
				new EnemyNestCreationInfo(4, 4),
				new EnemyNestCreationInfo(5, 4)
			},
			new EnemyNestCreationInfo[1]
			{
				new EnemyNestCreationInfo(12, 2)
			}
		};
		List<IntPair> enemyNestCreationDateRanges60 = new List<IntPair>
		{
			new IntPair(0, 6),
			new IntPair(6, 12),
			new IntPair(12, 24),
			new IntPair(0, 0)
		};
		short[] settlementBlockCore60 = new short[1] { 36 };
		sbyte[] organizationId60 = new sbyte[1] { 38 };
		List<short[]> developedBlockCore60 = new List<short[]>
		{
			new short[2] { 39, 33 },
			new short[2] { 40, 33 },
			new short[2] { 41, 33 },
			new short[2] { 42, 33 },
			new short[2] { 43, 33 },
			new short[2] { 44, 33 },
			new short[2] { 45, 33 },
			new short[2] { 46, 33 },
			new short[2] { 47, 33 },
			new short[2] { 48, 33 },
			new short[2] { 49, 33 },
			new short[2] { 50, 33 },
			new short[2] { 51, 33 },
			new short[2] { 52, 33 },
			new short[2] { 53, 33 },
			new short[2] { 54, 33 },
			new short[2] { 55, 33 },
			new short[2] { 56, 33 },
			new short[2] { 109, 10 },
			new short[2] { 110, 10 },
			new short[2] { 111, 10 }
		};
		List<short[]> normalBlockCore60 = new List<short[]>
		{
			new short[2] { 57, 33 },
			new short[2] { 58, 33 },
			new short[2] { 59, 33 },
			new short[2] { 63, 33 },
			new short[2] { 64, 33 },
			new short[2] { 65, 33 },
			new short[2] { 69, 33 },
			new short[2] { 70, 33 },
			new short[2] { 71, 33 },
			new short[2] { 75, 33 },
			new short[2] { 76, 33 },
			new short[2] { 77, 33 },
			new short[2] { 81, 33 },
			new short[2] { 82, 33 },
			new short[2] { 83, 33 },
			new short[2] { 87, 33 },
			new short[2] { 88, 33 },
			new short[2] { 89, 33 },
			new short[2] { 112, 30 },
			new short[2] { 113, 30 },
			new short[2] { 114, 30 },
			new short[2] { 124, 30 }
		};
		List<short[]> wildBlockCore60 = new List<short[]>
		{
			new short[2] { 93, 33 },
			new short[2] { 94, 33 },
			new short[2] { 95, 33 },
			new short[2] { 96, 33 },
			new short[2] { 97, 33 },
			new short[2] { 98, 33 },
			new short[2] { 99, 33 },
			new short[2] { 100, 33 },
			new short[2] { 101, 33 },
			new short[2] { 102, 33 },
			new short[2] { 103, 33 },
			new short[2] { 104, 33 },
			new short[2] { 105, 33 },
			new short[2] { 106, 33 },
			new short[2] { 107, 33 },
			new short[2] { 108, 33 },
			new short[2] { 115, 40 },
			new short[2] { 116, 40 },
			new short[2] { 117, 40 },
			new short[2] { 124, 30 }
		};
		List<short[]> bigBaseBlockCore60 = new List<short[]>
		{
			new short[2] { 78, 1 },
			new short[2] { 84, 1 },
			new short[2] { 66, 1 },
			new short[2] { 91, 1 },
			new short[2] { 92, 1 }
		};
		list = new List<short[]>();
		List<short[]> seriesBlockCore60 = list;
		list = new List<short[]>();
		List<short[]> encircleBlockCore60 = list;
		short[] sceneryBlockCore60 = new short[0];
		list2 = new List<short>();
		List<short> lovingItemSubTypes60 = list2;
		list2 = new List<short>();
		dataArray60.Add(new MapAreaItem(119, 13, 496, 497, 20, worldMapPos60, neighborAreas60, "largemap_part_1_changgui_0", miniMapIcons60, null, miniMapPos60, 0, 0, 5, enemyNests60, enemyNestCreationDateRanges60, settlementBlockCore60, organizationId60, -1, 0, null, developedBlockCore60, normalBlockCore60, wildBlockCore60, bigBaseBlockCore60, seriesBlockCore60, encircleBlockCore60, sceneryBlockCore60, 498, 499, null, null, EMapAreaAreaDirection.South, lovingItemSubTypes60, list2, showDarkAshStatus: true));
	}

	private void CreateItems2()
	{
		_dataArray.Add(new MapAreaItem(120, 13, 500, 501, 20, new sbyte[2] { 17, 11 }, new AreaTravelRoute[0], "largemap_part_1_changgui_0", new string[4] { "largemap_part_2_icon_hei_0", "largemap_part_2_icon_jin_0", "largemap_part_2_icon_lan_0", "largemap_part_2_icon_hui_0" }, null, new short[2] { 180, 180 }, 0, 0, 5, new List<EnemyNestCreationInfo[]>
		{
			new EnemyNestCreationInfo[2]
			{
				new EnemyNestCreationInfo(0, 2),
				new EnemyNestCreationInfo(1, 2)
			},
			new EnemyNestCreationInfo[2]
			{
				new EnemyNestCreationInfo(1, 2),
				new EnemyNestCreationInfo(2, 3)
			},
			new EnemyNestCreationInfo[4]
			{
				new EnemyNestCreationInfo(2, 3),
				new EnemyNestCreationInfo(3, 3),
				new EnemyNestCreationInfo(4, 4),
				new EnemyNestCreationInfo(5, 4)
			},
			new EnemyNestCreationInfo[1]
			{
				new EnemyNestCreationInfo(12, 2)
			}
		}, new List<IntPair>
		{
			new IntPair(0, 6),
			new IntPair(6, 12),
			new IntPair(12, 24),
			new IntPair(0, 0)
		}, new short[1] { 34 }, new sbyte[1] { 36 }, -1, 0, null, new List<short[]>
		{
			new short[2] { 39, 33 },
			new short[2] { 40, 33 },
			new short[2] { 41, 33 },
			new short[2] { 42, 33 },
			new short[2] { 43, 33 },
			new short[2] { 44, 33 },
			new short[2] { 45, 33 },
			new short[2] { 46, 33 },
			new short[2] { 47, 33 },
			new short[2] { 48, 33 },
			new short[2] { 49, 33 },
			new short[2] { 50, 33 },
			new short[2] { 51, 33 },
			new short[2] { 52, 33 },
			new short[2] { 53, 33 },
			new short[2] { 54, 33 },
			new short[2] { 55, 33 },
			new short[2] { 56, 33 },
			new short[2] { 109, 10 },
			new short[2] { 110, 10 },
			new short[2] { 111, 10 }
		}, new List<short[]>
		{
			new short[2] { 57, 33 },
			new short[2] { 58, 33 },
			new short[2] { 59, 33 },
			new short[2] { 63, 33 },
			new short[2] { 64, 33 },
			new short[2] { 65, 33 },
			new short[2] { 69, 33 },
			new short[2] { 70, 33 },
			new short[2] { 71, 33 },
			new short[2] { 75, 33 },
			new short[2] { 76, 33 },
			new short[2] { 77, 33 },
			new short[2] { 81, 33 },
			new short[2] { 82, 33 },
			new short[2] { 83, 33 },
			new short[2] { 87, 33 },
			new short[2] { 88, 33 },
			new short[2] { 89, 33 },
			new short[2] { 112, 30 },
			new short[2] { 113, 30 },
			new short[2] { 114, 30 },
			new short[2] { 124, 30 }
		}, new List<short[]>
		{
			new short[2] { 93, 33 },
			new short[2] { 94, 33 },
			new short[2] { 95, 33 },
			new short[2] { 96, 33 },
			new short[2] { 97, 33 },
			new short[2] { 98, 33 },
			new short[2] { 99, 33 },
			new short[2] { 100, 33 },
			new short[2] { 101, 33 },
			new short[2] { 102, 33 },
			new short[2] { 103, 33 },
			new short[2] { 104, 33 },
			new short[2] { 105, 33 },
			new short[2] { 106, 33 },
			new short[2] { 107, 33 },
			new short[2] { 108, 33 },
			new short[2] { 115, 40 },
			new short[2] { 116, 40 },
			new short[2] { 117, 40 },
			new short[2] { 124, 30 }
		}, new List<short[]>
		{
			new short[2] { 78, 1 },
			new short[2] { 84, 1 },
			new short[2] { 66, 1 },
			new short[2] { 91, 1 },
			new short[2] { 92, 1 }
		}, new List<short[]>(), new List<short[]>(), new short[0], 502, 503, null, null, EMapAreaAreaDirection.South, new List<short>(), new List<short>(), showDarkAshStatus: true));
		_dataArray.Add(new MapAreaItem(121, 13, 504, 505, 20, new sbyte[2] { 18, 12 }, new AreaTravelRoute[1]
		{
			new AreaTravelRoute(123, 22, new int[4] { 19, 12, 19, 11 })
		}, "largemap_part_1_changgui_1", new string[4] { "largemap_part_2_icon_hei_1", "largemap_part_2_icon_jin_1", "largemap_part_2_icon_lan_1", "largemap_part_2_icon_hui_1" }, null, new short[2] { 220, 260 }, 0, 0, 5, new List<EnemyNestCreationInfo[]>
		{
			new EnemyNestCreationInfo[2]
			{
				new EnemyNestCreationInfo(0, 2),
				new EnemyNestCreationInfo(1, 2)
			},
			new EnemyNestCreationInfo[2]
			{
				new EnemyNestCreationInfo(1, 2),
				new EnemyNestCreationInfo(2, 3)
			},
			new EnemyNestCreationInfo[4]
			{
				new EnemyNestCreationInfo(2, 3),
				new EnemyNestCreationInfo(3, 3),
				new EnemyNestCreationInfo(4, 4),
				new EnemyNestCreationInfo(5, 4)
			},
			new EnemyNestCreationInfo[1]
			{
				new EnemyNestCreationInfo(12, 2)
			}
		}, new List<IntPair>
		{
			new IntPair(0, 6),
			new IntPair(6, 12),
			new IntPair(12, 24),
			new IntPair(0, 0)
		}, new short[1] { 34 }, new sbyte[1] { 36 }, -1, 0, null, new List<short[]>
		{
			new short[2] { 39, 33 },
			new short[2] { 40, 33 },
			new short[2] { 41, 33 },
			new short[2] { 42, 33 },
			new short[2] { 43, 33 },
			new short[2] { 44, 33 },
			new short[2] { 45, 33 },
			new short[2] { 46, 33 },
			new short[2] { 47, 33 },
			new short[2] { 48, 33 },
			new short[2] { 49, 33 },
			new short[2] { 50, 33 },
			new short[2] { 51, 33 },
			new short[2] { 52, 33 },
			new short[2] { 53, 33 },
			new short[2] { 54, 33 },
			new short[2] { 55, 33 },
			new short[2] { 56, 33 },
			new short[2] { 109, 10 },
			new short[2] { 110, 10 },
			new short[2] { 111, 10 }
		}, new List<short[]>
		{
			new short[2] { 57, 33 },
			new short[2] { 58, 33 },
			new short[2] { 59, 33 },
			new short[2] { 63, 33 },
			new short[2] { 64, 33 },
			new short[2] { 65, 33 },
			new short[2] { 69, 33 },
			new short[2] { 70, 33 },
			new short[2] { 71, 33 },
			new short[2] { 75, 33 },
			new short[2] { 76, 33 },
			new short[2] { 77, 33 },
			new short[2] { 81, 33 },
			new short[2] { 82, 33 },
			new short[2] { 83, 33 },
			new short[2] { 87, 33 },
			new short[2] { 88, 33 },
			new short[2] { 89, 33 },
			new short[2] { 112, 30 },
			new short[2] { 113, 30 },
			new short[2] { 114, 30 },
			new short[2] { 124, 30 }
		}, new List<short[]>
		{
			new short[2] { 93, 33 },
			new short[2] { 94, 33 },
			new short[2] { 95, 33 },
			new short[2] { 96, 33 },
			new short[2] { 97, 33 },
			new short[2] { 98, 33 },
			new short[2] { 99, 33 },
			new short[2] { 100, 33 },
			new short[2] { 101, 33 },
			new short[2] { 102, 33 },
			new short[2] { 103, 33 },
			new short[2] { 104, 33 },
			new short[2] { 105, 33 },
			new short[2] { 106, 33 },
			new short[2] { 107, 33 },
			new short[2] { 108, 33 },
			new short[2] { 115, 40 },
			new short[2] { 116, 40 },
			new short[2] { 117, 40 },
			new short[2] { 124, 30 }
		}, new List<short[]>
		{
			new short[2] { 78, 1 },
			new short[2] { 84, 1 },
			new short[2] { 66, 1 },
			new short[2] { 91, 1 },
			new short[2] { 92, 1 }
		}, new List<short[]>(), new List<short[]>(), new short[0], 506, 507, null, null, EMapAreaAreaDirection.South, new List<short>(), new List<short>(), showDarkAshStatus: true));
		_dataArray.Add(new MapAreaItem(122, 14, 508, 509, 20, new sbyte[2] { 21, 11 }, new AreaTravelRoute[1]
		{
			new AreaTravelRoute(123, 6, null)
		}, "largemap_part_1_changgui_2", new string[4] { "largemap_part_2_icon_hei_2", "largemap_part_2_icon_jin_2", "largemap_part_2_icon_lan_2", "largemap_part_2_icon_hui_2" }, null, new short[2] { 130, 220 }, 0, 0, 5, new List<EnemyNestCreationInfo[]>
		{
			new EnemyNestCreationInfo[2]
			{
				new EnemyNestCreationInfo(0, 2),
				new EnemyNestCreationInfo(1, 2)
			},
			new EnemyNestCreationInfo[2]
			{
				new EnemyNestCreationInfo(1, 2),
				new EnemyNestCreationInfo(2, 3)
			},
			new EnemyNestCreationInfo[4]
			{
				new EnemyNestCreationInfo(2, 3),
				new EnemyNestCreationInfo(3, 3),
				new EnemyNestCreationInfo(4, 4),
				new EnemyNestCreationInfo(5, 4)
			},
			new EnemyNestCreationInfo[1]
			{
				new EnemyNestCreationInfo(12, 2)
			}
		}, new List<IntPair>
		{
			new IntPair(0, 6),
			new IntPair(6, 12),
			new IntPair(12, 24),
			new IntPair(0, 0)
		}, new short[1] { 35 }, new sbyte[1] { 37 }, -1, 0, null, new List<short[]>
		{
			new short[2] { 39, 33 },
			new short[2] { 40, 33 },
			new short[2] { 41, 33 },
			new short[2] { 42, 33 },
			new short[2] { 43, 33 },
			new short[2] { 44, 33 },
			new short[2] { 45, 33 },
			new short[2] { 46, 33 },
			new short[2] { 47, 33 },
			new short[2] { 48, 33 },
			new short[2] { 49, 33 },
			new short[2] { 50, 33 },
			new short[2] { 51, 33 },
			new short[2] { 52, 33 },
			new short[2] { 53, 33 },
			new short[2] { 54, 33 },
			new short[2] { 55, 33 },
			new short[2] { 56, 33 },
			new short[2] { 109, 10 },
			new short[2] { 110, 10 },
			new short[2] { 111, 10 }
		}, new List<short[]>
		{
			new short[2] { 57, 33 },
			new short[2] { 58, 33 },
			new short[2] { 59, 33 },
			new short[2] { 63, 33 },
			new short[2] { 64, 33 },
			new short[2] { 65, 33 },
			new short[2] { 69, 33 },
			new short[2] { 70, 33 },
			new short[2] { 71, 33 },
			new short[2] { 75, 33 },
			new short[2] { 76, 33 },
			new short[2] { 77, 33 },
			new short[2] { 81, 33 },
			new short[2] { 82, 33 },
			new short[2] { 83, 33 },
			new short[2] { 87, 33 },
			new short[2] { 88, 33 },
			new short[2] { 89, 33 },
			new short[2] { 112, 30 },
			new short[2] { 113, 30 },
			new short[2] { 114, 30 },
			new short[2] { 124, 30 }
		}, new List<short[]>
		{
			new short[2] { 93, 33 },
			new short[2] { 94, 33 },
			new short[2] { 95, 33 },
			new short[2] { 96, 33 },
			new short[2] { 97, 33 },
			new short[2] { 98, 33 },
			new short[2] { 99, 33 },
			new short[2] { 100, 33 },
			new short[2] { 101, 33 },
			new short[2] { 102, 33 },
			new short[2] { 103, 33 },
			new short[2] { 104, 33 },
			new short[2] { 105, 33 },
			new short[2] { 106, 33 },
			new short[2] { 107, 33 },
			new short[2] { 108, 33 },
			new short[2] { 115, 40 },
			new short[2] { 116, 40 },
			new short[2] { 117, 40 },
			new short[2] { 124, 30 }
		}, new List<short[]>
		{
			new short[2] { 60, 1 },
			new short[2] { 84, 1 },
			new short[2] { 67, 1 },
			new short[2] { 68, 1 },
			new short[2] { 90, 1 }
		}, new List<short[]>(), new List<short[]>(), new short[0], 510, 511, null, null, EMapAreaAreaDirection.South, new List<short>(), new List<short>(), showDarkAshStatus: true));
		_dataArray.Add(new MapAreaItem(123, 14, 512, 513, 20, new sbyte[2] { 20, 11 }, new AreaTravelRoute[1]
		{
			new AreaTravelRoute(133, 6, null)
		}, "largemap_part_1_changgui_1", new string[4] { "largemap_part_2_icon_hei_1", "largemap_part_2_icon_jin_1", "largemap_part_2_icon_lan_1", "largemap_part_2_icon_hui_1" }, null, new short[2] { 90, 220 }, 0, 0, 5, new List<EnemyNestCreationInfo[]>
		{
			new EnemyNestCreationInfo[2]
			{
				new EnemyNestCreationInfo(0, 2),
				new EnemyNestCreationInfo(1, 2)
			},
			new EnemyNestCreationInfo[2]
			{
				new EnemyNestCreationInfo(1, 2),
				new EnemyNestCreationInfo(2, 3)
			},
			new EnemyNestCreationInfo[4]
			{
				new EnemyNestCreationInfo(2, 3),
				new EnemyNestCreationInfo(3, 3),
				new EnemyNestCreationInfo(4, 4),
				new EnemyNestCreationInfo(5, 4)
			},
			new EnemyNestCreationInfo[1]
			{
				new EnemyNestCreationInfo(12, 2)
			}
		}, new List<IntPair>
		{
			new IntPair(0, 6),
			new IntPair(6, 12),
			new IntPair(12, 24),
			new IntPair(0, 0)
		}, new short[1] { 35 }, new sbyte[1] { 37 }, -1, 0, null, new List<short[]>
		{
			new short[2] { 39, 33 },
			new short[2] { 40, 33 },
			new short[2] { 41, 33 },
			new short[2] { 42, 33 },
			new short[2] { 43, 33 },
			new short[2] { 44, 33 },
			new short[2] { 45, 33 },
			new short[2] { 46, 33 },
			new short[2] { 47, 33 },
			new short[2] { 48, 33 },
			new short[2] { 49, 33 },
			new short[2] { 50, 33 },
			new short[2] { 51, 33 },
			new short[2] { 52, 33 },
			new short[2] { 53, 33 },
			new short[2] { 54, 33 },
			new short[2] { 55, 33 },
			new short[2] { 56, 33 },
			new short[2] { 109, 10 },
			new short[2] { 110, 10 },
			new short[2] { 111, 10 }
		}, new List<short[]>
		{
			new short[2] { 57, 33 },
			new short[2] { 58, 33 },
			new short[2] { 59, 33 },
			new short[2] { 63, 33 },
			new short[2] { 64, 33 },
			new short[2] { 65, 33 },
			new short[2] { 69, 33 },
			new short[2] { 70, 33 },
			new short[2] { 71, 33 },
			new short[2] { 75, 33 },
			new short[2] { 76, 33 },
			new short[2] { 77, 33 },
			new short[2] { 81, 33 },
			new short[2] { 82, 33 },
			new short[2] { 83, 33 },
			new short[2] { 87, 33 },
			new short[2] { 88, 33 },
			new short[2] { 89, 33 },
			new short[2] { 112, 30 },
			new short[2] { 113, 30 },
			new short[2] { 114, 30 },
			new short[2] { 124, 30 }
		}, new List<short[]>
		{
			new short[2] { 93, 33 },
			new short[2] { 94, 33 },
			new short[2] { 95, 33 },
			new short[2] { 96, 33 },
			new short[2] { 97, 33 },
			new short[2] { 98, 33 },
			new short[2] { 99, 33 },
			new short[2] { 100, 33 },
			new short[2] { 101, 33 },
			new short[2] { 102, 33 },
			new short[2] { 103, 33 },
			new short[2] { 104, 33 },
			new short[2] { 105, 33 },
			new short[2] { 106, 33 },
			new short[2] { 107, 33 },
			new short[2] { 108, 33 },
			new short[2] { 115, 40 },
			new short[2] { 116, 40 },
			new short[2] { 117, 40 },
			new short[2] { 124, 30 }
		}, new List<short[]>
		{
			new short[2] { 60, 1 },
			new short[2] { 84, 1 },
			new short[2] { 67, 1 },
			new short[2] { 68, 1 },
			new short[2] { 90, 1 }
		}, new List<short[]>(), new List<short[]>(), new short[0], 514, 515, null, null, EMapAreaAreaDirection.South, new List<short>(), new List<short>(), showDarkAshStatus: true));
		_dataArray.Add(new MapAreaItem(124, 14, 516, 517, 20, new sbyte[2] { 22, 9 }, new AreaTravelRoute[1]
		{
			new AreaTravelRoute(128, 12, new int[2] { 21, 9 })
		}, "largemap_part_1_changgui_2", new string[4] { "largemap_part_2_icon_hei_2", "largemap_part_2_icon_jin_2", "largemap_part_2_icon_lan_2", "largemap_part_2_icon_hui_2" }, null, new short[2] { 170, 140 }, 0, 0, 5, new List<EnemyNestCreationInfo[]>
		{
			new EnemyNestCreationInfo[2]
			{
				new EnemyNestCreationInfo(0, 2),
				new EnemyNestCreationInfo(1, 2)
			},
			new EnemyNestCreationInfo[2]
			{
				new EnemyNestCreationInfo(1, 2),
				new EnemyNestCreationInfo(2, 3)
			},
			new EnemyNestCreationInfo[4]
			{
				new EnemyNestCreationInfo(2, 3),
				new EnemyNestCreationInfo(3, 3),
				new EnemyNestCreationInfo(4, 4),
				new EnemyNestCreationInfo(5, 4)
			},
			new EnemyNestCreationInfo[1]
			{
				new EnemyNestCreationInfo(12, 2)
			}
		}, new List<IntPair>
		{
			new IntPair(0, 6),
			new IntPair(6, 12),
			new IntPair(12, 24),
			new IntPair(0, 0)
		}, new short[1] { 34 }, new sbyte[1] { 36 }, -1, 0, null, new List<short[]>
		{
			new short[2] { 39, 33 },
			new short[2] { 40, 33 },
			new short[2] { 41, 33 },
			new short[2] { 42, 33 },
			new short[2] { 43, 33 },
			new short[2] { 44, 33 },
			new short[2] { 45, 33 },
			new short[2] { 46, 33 },
			new short[2] { 47, 33 },
			new short[2] { 48, 33 },
			new short[2] { 49, 33 },
			new short[2] { 50, 33 },
			new short[2] { 51, 33 },
			new short[2] { 52, 33 },
			new short[2] { 53, 33 },
			new short[2] { 54, 33 },
			new short[2] { 55, 33 },
			new short[2] { 56, 33 },
			new short[2] { 109, 10 },
			new short[2] { 110, 10 },
			new short[2] { 111, 10 }
		}, new List<short[]>
		{
			new short[2] { 57, 33 },
			new short[2] { 58, 33 },
			new short[2] { 59, 33 },
			new short[2] { 63, 33 },
			new short[2] { 64, 33 },
			new short[2] { 65, 33 },
			new short[2] { 69, 33 },
			new short[2] { 70, 33 },
			new short[2] { 71, 33 },
			new short[2] { 75, 33 },
			new short[2] { 76, 33 },
			new short[2] { 77, 33 },
			new short[2] { 81, 33 },
			new short[2] { 82, 33 },
			new short[2] { 83, 33 },
			new short[2] { 87, 33 },
			new short[2] { 88, 33 },
			new short[2] { 89, 33 },
			new short[2] { 112, 30 },
			new short[2] { 113, 30 },
			new short[2] { 114, 30 },
			new short[2] { 124, 30 }
		}, new List<short[]>
		{
			new short[2] { 93, 33 },
			new short[2] { 94, 33 },
			new short[2] { 95, 33 },
			new short[2] { 96, 33 },
			new short[2] { 97, 33 },
			new short[2] { 98, 33 },
			new short[2] { 99, 33 },
			new short[2] { 100, 33 },
			new short[2] { 101, 33 },
			new short[2] { 102, 33 },
			new short[2] { 103, 33 },
			new short[2] { 104, 33 },
			new short[2] { 105, 33 },
			new short[2] { 106, 33 },
			new short[2] { 107, 33 },
			new short[2] { 108, 33 },
			new short[2] { 115, 40 },
			new short[2] { 116, 40 },
			new short[2] { 117, 40 },
			new short[2] { 124, 30 }
		}, new List<short[]>
		{
			new short[2] { 60, 1 },
			new short[2] { 84, 1 },
			new short[2] { 67, 1 },
			new short[2] { 68, 1 },
			new short[2] { 90, 1 }
		}, new List<short[]>(), new List<short[]>(), new short[0], 518, 519, null, null, EMapAreaAreaDirection.South, new List<short>(), new List<short>(), showDarkAshStatus: true));
		_dataArray.Add(new MapAreaItem(125, 14, 520, 521, 20, new sbyte[2] { 20, 10 }, new AreaTravelRoute[0], "largemap_part_1_changgui_1", new string[4] { "largemap_part_2_icon_hei_1", "largemap_part_2_icon_jin_1", "largemap_part_2_icon_lan_1", "largemap_part_2_icon_hui_1" }, null, new short[2] { 90, 180 }, 0, 0, 5, new List<EnemyNestCreationInfo[]>
		{
			new EnemyNestCreationInfo[2]
			{
				new EnemyNestCreationInfo(0, 2),
				new EnemyNestCreationInfo(1, 2)
			},
			new EnemyNestCreationInfo[2]
			{
				new EnemyNestCreationInfo(1, 2),
				new EnemyNestCreationInfo(2, 3)
			},
			new EnemyNestCreationInfo[4]
			{
				new EnemyNestCreationInfo(2, 3),
				new EnemyNestCreationInfo(3, 3),
				new EnemyNestCreationInfo(4, 4),
				new EnemyNestCreationInfo(5, 4)
			},
			new EnemyNestCreationInfo[1]
			{
				new EnemyNestCreationInfo(12, 2)
			}
		}, new List<IntPair>
		{
			new IntPair(0, 6),
			new IntPair(6, 12),
			new IntPair(12, 24),
			new IntPair(0, 0)
		}, new short[1] { 35 }, new sbyte[1] { 37 }, -1, 0, null, new List<short[]>
		{
			new short[2] { 39, 33 },
			new short[2] { 40, 33 },
			new short[2] { 41, 33 },
			new short[2] { 42, 33 },
			new short[2] { 43, 33 },
			new short[2] { 44, 33 },
			new short[2] { 45, 33 },
			new short[2] { 46, 33 },
			new short[2] { 47, 33 },
			new short[2] { 48, 33 },
			new short[2] { 49, 33 },
			new short[2] { 50, 33 },
			new short[2] { 51, 33 },
			new short[2] { 52, 33 },
			new short[2] { 53, 33 },
			new short[2] { 54, 33 },
			new short[2] { 55, 33 },
			new short[2] { 56, 33 },
			new short[2] { 109, 10 },
			new short[2] { 110, 10 },
			new short[2] { 111, 10 }
		}, new List<short[]>
		{
			new short[2] { 57, 33 },
			new short[2] { 58, 33 },
			new short[2] { 59, 33 },
			new short[2] { 63, 33 },
			new short[2] { 64, 33 },
			new short[2] { 65, 33 },
			new short[2] { 69, 33 },
			new short[2] { 70, 33 },
			new short[2] { 71, 33 },
			new short[2] { 75, 33 },
			new short[2] { 76, 33 },
			new short[2] { 77, 33 },
			new short[2] { 81, 33 },
			new short[2] { 82, 33 },
			new short[2] { 83, 33 },
			new short[2] { 87, 33 },
			new short[2] { 88, 33 },
			new short[2] { 89, 33 },
			new short[2] { 112, 30 },
			new short[2] { 113, 30 },
			new short[2] { 114, 30 },
			new short[2] { 124, 30 }
		}, new List<short[]>
		{
			new short[2] { 93, 33 },
			new short[2] { 94, 33 },
			new short[2] { 95, 33 },
			new short[2] { 96, 33 },
			new short[2] { 97, 33 },
			new short[2] { 98, 33 },
			new short[2] { 99, 33 },
			new short[2] { 100, 33 },
			new short[2] { 101, 33 },
			new short[2] { 102, 33 },
			new short[2] { 103, 33 },
			new short[2] { 104, 33 },
			new short[2] { 105, 33 },
			new short[2] { 106, 33 },
			new short[2] { 107, 33 },
			new short[2] { 108, 33 },
			new short[2] { 115, 40 },
			new short[2] { 116, 40 },
			new short[2] { 117, 40 },
			new short[2] { 124, 30 }
		}, new List<short[]>
		{
			new short[2] { 60, 1 },
			new short[2] { 84, 1 },
			new short[2] { 67, 1 },
			new short[2] { 68, 1 },
			new short[2] { 90, 1 }
		}, new List<short[]>(), new List<short[]>(), new short[0], 522, 523, null, null, EMapAreaAreaDirection.South, new List<short>(), new List<short>(), showDarkAshStatus: true));
		_dataArray.Add(new MapAreaItem(126, 14, 524, 525, 20, new sbyte[2] { 23, 10 }, new AreaTravelRoute[0], "largemap_part_1_changgui_0", new string[4] { "largemap_part_2_icon_hei_0", "largemap_part_2_icon_jin_0", "largemap_part_2_icon_lan_0", "largemap_part_2_icon_hui_0" }, null, new short[2] { 210, 180 }, 0, 0, 5, new List<EnemyNestCreationInfo[]>
		{
			new EnemyNestCreationInfo[2]
			{
				new EnemyNestCreationInfo(0, 2),
				new EnemyNestCreationInfo(1, 2)
			},
			new EnemyNestCreationInfo[2]
			{
				new EnemyNestCreationInfo(1, 2),
				new EnemyNestCreationInfo(2, 3)
			},
			new EnemyNestCreationInfo[4]
			{
				new EnemyNestCreationInfo(2, 3),
				new EnemyNestCreationInfo(3, 3),
				new EnemyNestCreationInfo(4, 4),
				new EnemyNestCreationInfo(5, 4)
			},
			new EnemyNestCreationInfo[1]
			{
				new EnemyNestCreationInfo(12, 2)
			}
		}, new List<IntPair>
		{
			new IntPair(0, 6),
			new IntPair(6, 12),
			new IntPair(12, 24),
			new IntPair(0, 0)
		}, new short[1] { 35 }, new sbyte[1] { 37 }, -1, 0, null, new List<short[]>
		{
			new short[2] { 39, 33 },
			new short[2] { 40, 33 },
			new short[2] { 41, 33 },
			new short[2] { 42, 33 },
			new short[2] { 43, 33 },
			new short[2] { 44, 33 },
			new short[2] { 45, 33 },
			new short[2] { 46, 33 },
			new short[2] { 47, 33 },
			new short[2] { 48, 33 },
			new short[2] { 49, 33 },
			new short[2] { 50, 33 },
			new short[2] { 51, 33 },
			new short[2] { 52, 33 },
			new short[2] { 53, 33 },
			new short[2] { 54, 33 },
			new short[2] { 55, 33 },
			new short[2] { 56, 33 },
			new short[2] { 109, 10 },
			new short[2] { 110, 10 },
			new short[2] { 111, 10 }
		}, new List<short[]>
		{
			new short[2] { 57, 33 },
			new short[2] { 58, 33 },
			new short[2] { 59, 33 },
			new short[2] { 63, 33 },
			new short[2] { 64, 33 },
			new short[2] { 65, 33 },
			new short[2] { 69, 33 },
			new short[2] { 70, 33 },
			new short[2] { 71, 33 },
			new short[2] { 75, 33 },
			new short[2] { 76, 33 },
			new short[2] { 77, 33 },
			new short[2] { 81, 33 },
			new short[2] { 82, 33 },
			new short[2] { 83, 33 },
			new short[2] { 87, 33 },
			new short[2] { 88, 33 },
			new short[2] { 89, 33 },
			new short[2] { 112, 30 },
			new short[2] { 113, 30 },
			new short[2] { 114, 30 },
			new short[2] { 124, 30 }
		}, new List<short[]>
		{
			new short[2] { 93, 33 },
			new short[2] { 94, 33 },
			new short[2] { 95, 33 },
			new short[2] { 96, 33 },
			new short[2] { 97, 33 },
			new short[2] { 98, 33 },
			new short[2] { 99, 33 },
			new short[2] { 100, 33 },
			new short[2] { 101, 33 },
			new short[2] { 102, 33 },
			new short[2] { 103, 33 },
			new short[2] { 104, 33 },
			new short[2] { 105, 33 },
			new short[2] { 106, 33 },
			new short[2] { 107, 33 },
			new short[2] { 108, 33 },
			new short[2] { 115, 40 },
			new short[2] { 116, 40 },
			new short[2] { 117, 40 },
			new short[2] { 124, 30 }
		}, new List<short[]>
		{
			new short[2] { 60, 1 },
			new short[2] { 84, 1 },
			new short[2] { 67, 1 },
			new short[2] { 68, 1 },
			new short[2] { 90, 1 }
		}, new List<short[]>(), new List<short[]>(), new short[0], 526, 527, null, null, EMapAreaAreaDirection.South, new List<short>(), new List<short>(), showDarkAshStatus: true));
		_dataArray.Add(new MapAreaItem(127, 14, 528, 529, 20, new sbyte[2] { 19, 9 }, new AreaTravelRoute[1]
		{
			new AreaTravelRoute(128, 6, null)
		}, "largemap_part_1_changgui_1", new string[4] { "largemap_part_2_icon_hei_1", "largemap_part_2_icon_jin_1", "largemap_part_2_icon_lan_1", "largemap_part_2_icon_hui_1" }, null, new short[2] { 50, 140 }, 0, 0, 5, new List<EnemyNestCreationInfo[]>
		{
			new EnemyNestCreationInfo[2]
			{
				new EnemyNestCreationInfo(0, 2),
				new EnemyNestCreationInfo(1, 2)
			},
			new EnemyNestCreationInfo[2]
			{
				new EnemyNestCreationInfo(1, 2),
				new EnemyNestCreationInfo(2, 3)
			},
			new EnemyNestCreationInfo[4]
			{
				new EnemyNestCreationInfo(2, 3),
				new EnemyNestCreationInfo(3, 3),
				new EnemyNestCreationInfo(4, 4),
				new EnemyNestCreationInfo(5, 4)
			},
			new EnemyNestCreationInfo[1]
			{
				new EnemyNestCreationInfo(12, 2)
			}
		}, new List<IntPair>
		{
			new IntPair(0, 6),
			new IntPair(6, 12),
			new IntPair(12, 24),
			new IntPair(0, 0)
		}, new short[1] { 34 }, new sbyte[1] { 36 }, -1, 0, null, new List<short[]>
		{
			new short[2] { 39, 33 },
			new short[2] { 40, 33 },
			new short[2] { 41, 33 },
			new short[2] { 42, 33 },
			new short[2] { 43, 33 },
			new short[2] { 44, 33 },
			new short[2] { 45, 33 },
			new short[2] { 46, 33 },
			new short[2] { 47, 33 },
			new short[2] { 48, 33 },
			new short[2] { 49, 33 },
			new short[2] { 50, 33 },
			new short[2] { 51, 33 },
			new short[2] { 52, 33 },
			new short[2] { 53, 33 },
			new short[2] { 54, 33 },
			new short[2] { 55, 33 },
			new short[2] { 56, 33 },
			new short[2] { 109, 10 },
			new short[2] { 110, 10 },
			new short[2] { 111, 10 }
		}, new List<short[]>
		{
			new short[2] { 57, 33 },
			new short[2] { 58, 33 },
			new short[2] { 59, 33 },
			new short[2] { 63, 33 },
			new short[2] { 64, 33 },
			new short[2] { 65, 33 },
			new short[2] { 69, 33 },
			new short[2] { 70, 33 },
			new short[2] { 71, 33 },
			new short[2] { 75, 33 },
			new short[2] { 76, 33 },
			new short[2] { 77, 33 },
			new short[2] { 81, 33 },
			new short[2] { 82, 33 },
			new short[2] { 83, 33 },
			new short[2] { 87, 33 },
			new short[2] { 88, 33 },
			new short[2] { 89, 33 },
			new short[2] { 112, 30 },
			new short[2] { 113, 30 },
			new short[2] { 114, 30 },
			new short[2] { 124, 30 }
		}, new List<short[]>
		{
			new short[2] { 93, 33 },
			new short[2] { 94, 33 },
			new short[2] { 95, 33 },
			new short[2] { 96, 33 },
			new short[2] { 97, 33 },
			new short[2] { 98, 33 },
			new short[2] { 99, 33 },
			new short[2] { 100, 33 },
			new short[2] { 101, 33 },
			new short[2] { 102, 33 },
			new short[2] { 103, 33 },
			new short[2] { 104, 33 },
			new short[2] { 105, 33 },
			new short[2] { 106, 33 },
			new short[2] { 107, 33 },
			new short[2] { 108, 33 },
			new short[2] { 115, 40 },
			new short[2] { 116, 40 },
			new short[2] { 117, 40 },
			new short[2] { 124, 30 }
		}, new List<short[]>
		{
			new short[2] { 60, 1 },
			new short[2] { 84, 1 },
			new short[2] { 67, 1 },
			new short[2] { 68, 1 },
			new short[2] { 90, 1 }
		}, new List<short[]>(), new List<short[]>(), new short[0], 530, 531, null, null, EMapAreaAreaDirection.South, new List<short>(), new List<short>(), showDarkAshStatus: true));
		_dataArray.Add(new MapAreaItem(128, 14, 532, 533, 20, new sbyte[2] { 20, 9 }, new AreaTravelRoute[0], "largemap_part_1_changgui_0", new string[4] { "largemap_part_2_icon_hei_0", "largemap_part_2_icon_jin_0", "largemap_part_2_icon_lan_0", "largemap_part_2_icon_hui_0" }, null, new short[2] { 90, 140 }, 0, 0, 5, new List<EnemyNestCreationInfo[]>
		{
			new EnemyNestCreationInfo[2]
			{
				new EnemyNestCreationInfo(0, 2),
				new EnemyNestCreationInfo(1, 2)
			},
			new EnemyNestCreationInfo[2]
			{
				new EnemyNestCreationInfo(1, 2),
				new EnemyNestCreationInfo(2, 3)
			},
			new EnemyNestCreationInfo[4]
			{
				new EnemyNestCreationInfo(2, 3),
				new EnemyNestCreationInfo(3, 3),
				new EnemyNestCreationInfo(4, 4),
				new EnemyNestCreationInfo(5, 4)
			},
			new EnemyNestCreationInfo[1]
			{
				new EnemyNestCreationInfo(12, 2)
			}
		}, new List<IntPair>
		{
			new IntPair(0, 6),
			new IntPair(6, 12),
			new IntPair(12, 24),
			new IntPair(0, 0)
		}, new short[1] { 34 }, new sbyte[1] { 36 }, -1, 0, null, new List<short[]>
		{
			new short[2] { 39, 33 },
			new short[2] { 40, 33 },
			new short[2] { 41, 33 },
			new short[2] { 42, 33 },
			new short[2] { 43, 33 },
			new short[2] { 44, 33 },
			new short[2] { 45, 33 },
			new short[2] { 46, 33 },
			new short[2] { 47, 33 },
			new short[2] { 48, 33 },
			new short[2] { 49, 33 },
			new short[2] { 50, 33 },
			new short[2] { 51, 33 },
			new short[2] { 52, 33 },
			new short[2] { 53, 33 },
			new short[2] { 54, 33 },
			new short[2] { 55, 33 },
			new short[2] { 56, 33 },
			new short[2] { 109, 10 },
			new short[2] { 110, 10 },
			new short[2] { 111, 10 }
		}, new List<short[]>
		{
			new short[2] { 57, 33 },
			new short[2] { 58, 33 },
			new short[2] { 59, 33 },
			new short[2] { 63, 33 },
			new short[2] { 64, 33 },
			new short[2] { 65, 33 },
			new short[2] { 69, 33 },
			new short[2] { 70, 33 },
			new short[2] { 71, 33 },
			new short[2] { 75, 33 },
			new short[2] { 76, 33 },
			new short[2] { 77, 33 },
			new short[2] { 81, 33 },
			new short[2] { 82, 33 },
			new short[2] { 83, 33 },
			new short[2] { 87, 33 },
			new short[2] { 88, 33 },
			new short[2] { 89, 33 },
			new short[2] { 112, 30 },
			new short[2] { 113, 30 },
			new short[2] { 114, 30 },
			new short[2] { 124, 30 }
		}, new List<short[]>
		{
			new short[2] { 93, 33 },
			new short[2] { 94, 33 },
			new short[2] { 95, 33 },
			new short[2] { 96, 33 },
			new short[2] { 97, 33 },
			new short[2] { 98, 33 },
			new short[2] { 99, 33 },
			new short[2] { 100, 33 },
			new short[2] { 101, 33 },
			new short[2] { 102, 33 },
			new short[2] { 103, 33 },
			new short[2] { 104, 33 },
			new short[2] { 105, 33 },
			new short[2] { 106, 33 },
			new short[2] { 107, 33 },
			new short[2] { 108, 33 },
			new short[2] { 115, 40 },
			new short[2] { 116, 40 },
			new short[2] { 117, 40 },
			new short[2] { 124, 30 }
		}, new List<short[]>
		{
			new short[2] { 60, 1 },
			new short[2] { 84, 1 },
			new short[2] { 67, 1 },
			new short[2] { 68, 1 },
			new short[2] { 90, 1 }
		}, new List<short[]>(), new List<short[]>(), new short[0], 534, 535, null, null, EMapAreaAreaDirection.South, new List<short>(), new List<short>(), showDarkAshStatus: true));
		_dataArray.Add(new MapAreaItem(129, 15, 536, 537, 20, new sbyte[2] { 23, 13 }, new AreaTravelRoute[0], "largemap_part_1_changgui_1", new string[4] { "largemap_part_2_icon_hei_1", "largemap_part_2_icon_jin_1", "largemap_part_2_icon_lan_1", "largemap_part_2_icon_hui_1" }, null, new short[2] { 320, 130 }, 0, 0, 5, new List<EnemyNestCreationInfo[]>
		{
			new EnemyNestCreationInfo[2]
			{
				new EnemyNestCreationInfo(0, 2),
				new EnemyNestCreationInfo(1, 2)
			},
			new EnemyNestCreationInfo[2]
			{
				new EnemyNestCreationInfo(1, 2),
				new EnemyNestCreationInfo(2, 3)
			},
			new EnemyNestCreationInfo[4]
			{
				new EnemyNestCreationInfo(2, 3),
				new EnemyNestCreationInfo(3, 3),
				new EnemyNestCreationInfo(4, 4),
				new EnemyNestCreationInfo(5, 4)
			},
			new EnemyNestCreationInfo[1]
			{
				new EnemyNestCreationInfo(12, 2)
			}
		}, new List<IntPair>
		{
			new IntPair(0, 6),
			new IntPair(6, 12),
			new IntPair(12, 24),
			new IntPair(0, 0)
		}, new short[1] { 35 }, new sbyte[1] { 37 }, -1, 0, null, new List<short[]>
		{
			new short[2] { 39, 33 },
			new short[2] { 40, 33 },
			new short[2] { 41, 33 },
			new short[2] { 42, 33 },
			new short[2] { 43, 33 },
			new short[2] { 44, 33 },
			new short[2] { 45, 33 },
			new short[2] { 46, 33 },
			new short[2] { 47, 33 },
			new short[2] { 48, 33 },
			new short[2] { 49, 33 },
			new short[2] { 50, 33 },
			new short[2] { 51, 33 },
			new short[2] { 52, 33 },
			new short[2] { 53, 33 },
			new short[2] { 54, 33 },
			new short[2] { 55, 33 },
			new short[2] { 56, 33 },
			new short[2] { 109, 10 },
			new short[2] { 110, 10 },
			new short[2] { 111, 10 }
		}, new List<short[]>
		{
			new short[2] { 57, 33 },
			new short[2] { 58, 33 },
			new short[2] { 59, 33 },
			new short[2] { 63, 33 },
			new short[2] { 64, 33 },
			new short[2] { 65, 33 },
			new short[2] { 69, 33 },
			new short[2] { 70, 33 },
			new short[2] { 71, 33 },
			new short[2] { 75, 33 },
			new short[2] { 76, 33 },
			new short[2] { 77, 33 },
			new short[2] { 81, 33 },
			new short[2] { 82, 33 },
			new short[2] { 83, 33 },
			new short[2] { 87, 33 },
			new short[2] { 88, 33 },
			new short[2] { 89, 33 },
			new short[2] { 112, 30 },
			new short[2] { 113, 30 },
			new short[2] { 114, 30 },
			new short[2] { 124, 30 }
		}, new List<short[]>
		{
			new short[2] { 93, 33 },
			new short[2] { 94, 33 },
			new short[2] { 95, 33 },
			new short[2] { 96, 33 },
			new short[2] { 97, 33 },
			new short[2] { 98, 33 },
			new short[2] { 99, 33 },
			new short[2] { 100, 33 },
			new short[2] { 101, 33 },
			new short[2] { 102, 33 },
			new short[2] { 103, 33 },
			new short[2] { 104, 33 },
			new short[2] { 105, 33 },
			new short[2] { 106, 33 },
			new short[2] { 107, 33 },
			new short[2] { 108, 33 },
			new short[2] { 115, 40 },
			new short[2] { 116, 40 },
			new short[2] { 117, 40 },
			new short[2] { 124, 30 }
		}, new List<short[]>
		{
			new short[2] { 72, 1 },
			new short[2] { 85, 1 },
			new short[2] { 86, 1 },
			new short[2] { 66, 1 },
			new short[2] { 90, 1 }
		}, new List<short[]>(), new List<short[]>(), new short[0], 538, 539, null, null, EMapAreaAreaDirection.South, new List<short>(), new List<short>(), showDarkAshStatus: true));
		_dataArray.Add(new MapAreaItem(130, 15, 540, 541, 20, new sbyte[2] { 19, 13 }, new AreaTravelRoute[1]
		{
			new AreaTravelRoute(134, 6, null)
		}, "largemap_part_1_changgui_2", new string[4] { "largemap_part_2_icon_hei_2", "largemap_part_2_icon_jin_2", "largemap_part_2_icon_lan_2", "largemap_part_2_icon_hui_2" }, null, new short[2] { 160, 130 }, 0, 0, 5, new List<EnemyNestCreationInfo[]>
		{
			new EnemyNestCreationInfo[2]
			{
				new EnemyNestCreationInfo(0, 2),
				new EnemyNestCreationInfo(1, 2)
			},
			new EnemyNestCreationInfo[2]
			{
				new EnemyNestCreationInfo(1, 2),
				new EnemyNestCreationInfo(2, 3)
			},
			new EnemyNestCreationInfo[4]
			{
				new EnemyNestCreationInfo(2, 3),
				new EnemyNestCreationInfo(3, 3),
				new EnemyNestCreationInfo(4, 4),
				new EnemyNestCreationInfo(5, 4)
			},
			new EnemyNestCreationInfo[1]
			{
				new EnemyNestCreationInfo(12, 2)
			}
		}, new List<IntPair>
		{
			new IntPair(0, 6),
			new IntPair(6, 12),
			new IntPair(12, 24),
			new IntPair(0, 0)
		}, new short[1] { 35 }, new sbyte[1] { 37 }, -1, 0, null, new List<short[]>
		{
			new short[2] { 39, 33 },
			new short[2] { 40, 33 },
			new short[2] { 41, 33 },
			new short[2] { 42, 33 },
			new short[2] { 43, 33 },
			new short[2] { 44, 33 },
			new short[2] { 45, 33 },
			new short[2] { 46, 33 },
			new short[2] { 47, 33 },
			new short[2] { 48, 33 },
			new short[2] { 49, 33 },
			new short[2] { 50, 33 },
			new short[2] { 51, 33 },
			new short[2] { 52, 33 },
			new short[2] { 53, 33 },
			new short[2] { 54, 33 },
			new short[2] { 55, 33 },
			new short[2] { 56, 33 },
			new short[2] { 109, 10 },
			new short[2] { 110, 10 },
			new short[2] { 111, 10 }
		}, new List<short[]>
		{
			new short[2] { 57, 33 },
			new short[2] { 58, 33 },
			new short[2] { 59, 33 },
			new short[2] { 63, 33 },
			new short[2] { 64, 33 },
			new short[2] { 65, 33 },
			new short[2] { 69, 33 },
			new short[2] { 70, 33 },
			new short[2] { 71, 33 },
			new short[2] { 75, 33 },
			new short[2] { 76, 33 },
			new short[2] { 77, 33 },
			new short[2] { 81, 33 },
			new short[2] { 82, 33 },
			new short[2] { 83, 33 },
			new short[2] { 87, 33 },
			new short[2] { 88, 33 },
			new short[2] { 89, 33 },
			new short[2] { 112, 30 },
			new short[2] { 113, 30 },
			new short[2] { 114, 30 },
			new short[2] { 124, 30 }
		}, new List<short[]>
		{
			new short[2] { 93, 33 },
			new short[2] { 94, 33 },
			new short[2] { 95, 33 },
			new short[2] { 96, 33 },
			new short[2] { 97, 33 },
			new short[2] { 98, 33 },
			new short[2] { 99, 33 },
			new short[2] { 100, 33 },
			new short[2] { 101, 33 },
			new short[2] { 102, 33 },
			new short[2] { 103, 33 },
			new short[2] { 104, 33 },
			new short[2] { 105, 33 },
			new short[2] { 106, 33 },
			new short[2] { 107, 33 },
			new short[2] { 108, 33 },
			new short[2] { 115, 40 },
			new short[2] { 116, 40 },
			new short[2] { 117, 40 },
			new short[2] { 124, 30 }
		}, new List<short[]>
		{
			new short[2] { 72, 1 },
			new short[2] { 85, 1 },
			new short[2] { 86, 1 },
			new short[2] { 66, 1 },
			new short[2] { 90, 1 }
		}, new List<short[]>(), new List<short[]>(), new short[0], 542, 543, null, null, EMapAreaAreaDirection.South, new List<short>(), new List<short>(), showDarkAshStatus: true));
		_dataArray.Add(new MapAreaItem(131, 15, 544, 545, 20, new sbyte[2] { 19, 15 }, new AreaTravelRoute[3]
		{
			new AreaTravelRoute(132, 6, null),
			new AreaTravelRoute(134, 6, null),
			new AreaTravelRoute(135, 6, null)
		}, "largemap_part_1_changgui_2", new string[4] { "largemap_part_2_icon_hei_2", "largemap_part_2_icon_jin_2", "largemap_part_2_icon_lan_2", "largemap_part_2_icon_hui_2" }, null, new short[2] { 160, 210 }, 0, 0, 5, new List<EnemyNestCreationInfo[]>
		{
			new EnemyNestCreationInfo[2]
			{
				new EnemyNestCreationInfo(0, 2),
				new EnemyNestCreationInfo(1, 2)
			},
			new EnemyNestCreationInfo[2]
			{
				new EnemyNestCreationInfo(1, 2),
				new EnemyNestCreationInfo(2, 3)
			},
			new EnemyNestCreationInfo[4]
			{
				new EnemyNestCreationInfo(2, 3),
				new EnemyNestCreationInfo(3, 3),
				new EnemyNestCreationInfo(4, 4),
				new EnemyNestCreationInfo(5, 4)
			},
			new EnemyNestCreationInfo[1]
			{
				new EnemyNestCreationInfo(12, 2)
			}
		}, new List<IntPair>
		{
			new IntPair(0, 6),
			new IntPair(6, 12),
			new IntPair(12, 24),
			new IntPair(0, 0)
		}, new short[1] { 35 }, new sbyte[1] { 37 }, -1, 0, null, new List<short[]>
		{
			new short[2] { 39, 33 },
			new short[2] { 40, 33 },
			new short[2] { 41, 33 },
			new short[2] { 42, 33 },
			new short[2] { 43, 33 },
			new short[2] { 44, 33 },
			new short[2] { 45, 33 },
			new short[2] { 46, 33 },
			new short[2] { 47, 33 },
			new short[2] { 48, 33 },
			new short[2] { 49, 33 },
			new short[2] { 50, 33 },
			new short[2] { 51, 33 },
			new short[2] { 52, 33 },
			new short[2] { 53, 33 },
			new short[2] { 54, 33 },
			new short[2] { 55, 33 },
			new short[2] { 56, 33 },
			new short[2] { 109, 10 },
			new short[2] { 110, 10 },
			new short[2] { 111, 10 }
		}, new List<short[]>
		{
			new short[2] { 57, 33 },
			new short[2] { 58, 33 },
			new short[2] { 59, 33 },
			new short[2] { 63, 33 },
			new short[2] { 64, 33 },
			new short[2] { 65, 33 },
			new short[2] { 69, 33 },
			new short[2] { 70, 33 },
			new short[2] { 71, 33 },
			new short[2] { 75, 33 },
			new short[2] { 76, 33 },
			new short[2] { 77, 33 },
			new short[2] { 81, 33 },
			new short[2] { 82, 33 },
			new short[2] { 83, 33 },
			new short[2] { 87, 33 },
			new short[2] { 88, 33 },
			new short[2] { 89, 33 },
			new short[2] { 112, 30 },
			new short[2] { 113, 30 },
			new short[2] { 114, 30 },
			new short[2] { 124, 30 }
		}, new List<short[]>
		{
			new short[2] { 93, 33 },
			new short[2] { 94, 33 },
			new short[2] { 95, 33 },
			new short[2] { 96, 33 },
			new short[2] { 97, 33 },
			new short[2] { 98, 33 },
			new short[2] { 99, 33 },
			new short[2] { 100, 33 },
			new short[2] { 101, 33 },
			new short[2] { 102, 33 },
			new short[2] { 103, 33 },
			new short[2] { 104, 33 },
			new short[2] { 105, 33 },
			new short[2] { 106, 33 },
			new short[2] { 107, 33 },
			new short[2] { 108, 33 },
			new short[2] { 115, 40 },
			new short[2] { 116, 40 },
			new short[2] { 117, 40 },
			new short[2] { 124, 30 }
		}, new List<short[]>
		{
			new short[2] { 72, 1 },
			new short[2] { 85, 1 },
			new short[2] { 86, 1 },
			new short[2] { 66, 1 },
			new short[2] { 90, 1 }
		}, new List<short[]>(), new List<short[]>(), new short[0], 546, 547, null, null, EMapAreaAreaDirection.South, new List<short>(), new List<short>(), showDarkAshStatus: true));
		_dataArray.Add(new MapAreaItem(132, 15, 548, 549, 20, new sbyte[2] { 18, 15 }, new AreaTravelRoute[0], "largemap_part_1_changgui_2", new string[4] { "largemap_part_2_icon_hei_2", "largemap_part_2_icon_jin_2", "largemap_part_2_icon_lan_2", "largemap_part_2_icon_hui_2" }, null, new short[2] { 120, 210 }, 0, 0, 5, new List<EnemyNestCreationInfo[]>
		{
			new EnemyNestCreationInfo[2]
			{
				new EnemyNestCreationInfo(0, 2),
				new EnemyNestCreationInfo(1, 2)
			},
			new EnemyNestCreationInfo[2]
			{
				new EnemyNestCreationInfo(1, 2),
				new EnemyNestCreationInfo(2, 3)
			},
			new EnemyNestCreationInfo[4]
			{
				new EnemyNestCreationInfo(2, 3),
				new EnemyNestCreationInfo(3, 3),
				new EnemyNestCreationInfo(4, 4),
				new EnemyNestCreationInfo(5, 4)
			},
			new EnemyNestCreationInfo[1]
			{
				new EnemyNestCreationInfo(12, 2)
			}
		}, new List<IntPair>
		{
			new IntPair(0, 6),
			new IntPair(6, 12),
			new IntPair(12, 24),
			new IntPair(0, 0)
		}, new short[1] { 35 }, new sbyte[1] { 37 }, -1, 0, null, new List<short[]>
		{
			new short[2] { 39, 33 },
			new short[2] { 40, 33 },
			new short[2] { 41, 33 },
			new short[2] { 42, 33 },
			new short[2] { 43, 33 },
			new short[2] { 44, 33 },
			new short[2] { 45, 33 },
			new short[2] { 46, 33 },
			new short[2] { 47, 33 },
			new short[2] { 48, 33 },
			new short[2] { 49, 33 },
			new short[2] { 50, 33 },
			new short[2] { 51, 33 },
			new short[2] { 52, 33 },
			new short[2] { 53, 33 },
			new short[2] { 54, 33 },
			new short[2] { 55, 33 },
			new short[2] { 56, 33 },
			new short[2] { 109, 10 },
			new short[2] { 110, 10 },
			new short[2] { 111, 10 }
		}, new List<short[]>
		{
			new short[2] { 57, 33 },
			new short[2] { 58, 33 },
			new short[2] { 59, 33 },
			new short[2] { 63, 33 },
			new short[2] { 64, 33 },
			new short[2] { 65, 33 },
			new short[2] { 69, 33 },
			new short[2] { 70, 33 },
			new short[2] { 71, 33 },
			new short[2] { 75, 33 },
			new short[2] { 76, 33 },
			new short[2] { 77, 33 },
			new short[2] { 81, 33 },
			new short[2] { 82, 33 },
			new short[2] { 83, 33 },
			new short[2] { 87, 33 },
			new short[2] { 88, 33 },
			new short[2] { 89, 33 },
			new short[2] { 112, 30 },
			new short[2] { 113, 30 },
			new short[2] { 114, 30 },
			new short[2] { 124, 30 }
		}, new List<short[]>
		{
			new short[2] { 93, 33 },
			new short[2] { 94, 33 },
			new short[2] { 95, 33 },
			new short[2] { 96, 33 },
			new short[2] { 97, 33 },
			new short[2] { 98, 33 },
			new short[2] { 99, 33 },
			new short[2] { 100, 33 },
			new short[2] { 101, 33 },
			new short[2] { 102, 33 },
			new short[2] { 103, 33 },
			new short[2] { 104, 33 },
			new short[2] { 105, 33 },
			new short[2] { 106, 33 },
			new short[2] { 107, 33 },
			new short[2] { 108, 33 },
			new short[2] { 115, 40 },
			new short[2] { 116, 40 },
			new short[2] { 117, 40 },
			new short[2] { 124, 30 }
		}, new List<short[]>
		{
			new short[2] { 72, 1 },
			new short[2] { 85, 1 },
			new short[2] { 86, 1 },
			new short[2] { 66, 1 },
			new short[2] { 90, 1 }
		}, new List<short[]>(), new List<short[]>(), new short[0], 550, 551, null, null, EMapAreaAreaDirection.South, new List<short>(), new List<short>(), showDarkAshStatus: true));
		_dataArray.Add(new MapAreaItem(133, 15, 552, 553, 20, new sbyte[2] { 20, 12 }, new AreaTravelRoute[0], "largemap_part_1_changgui_0", new string[4] { "largemap_part_2_icon_hei_0", "largemap_part_2_icon_jin_0", "largemap_part_2_icon_lan_0", "largemap_part_2_icon_hui_0" }, null, new short[2] { 200, 90 }, 0, 0, 5, new List<EnemyNestCreationInfo[]>
		{
			new EnemyNestCreationInfo[2]
			{
				new EnemyNestCreationInfo(0, 2),
				new EnemyNestCreationInfo(1, 2)
			},
			new EnemyNestCreationInfo[2]
			{
				new EnemyNestCreationInfo(1, 2),
				new EnemyNestCreationInfo(2, 3)
			},
			new EnemyNestCreationInfo[4]
			{
				new EnemyNestCreationInfo(2, 3),
				new EnemyNestCreationInfo(3, 3),
				new EnemyNestCreationInfo(4, 4),
				new EnemyNestCreationInfo(5, 4)
			},
			new EnemyNestCreationInfo[1]
			{
				new EnemyNestCreationInfo(12, 2)
			}
		}, new List<IntPair>
		{
			new IntPair(0, 6),
			new IntPair(6, 12),
			new IntPair(12, 24),
			new IntPair(0, 0)
		}, new short[1] { 34 }, new sbyte[1] { 36 }, -1, 0, null, new List<short[]>
		{
			new short[2] { 39, 33 },
			new short[2] { 40, 33 },
			new short[2] { 41, 33 },
			new short[2] { 42, 33 },
			new short[2] { 43, 33 },
			new short[2] { 44, 33 },
			new short[2] { 45, 33 },
			new short[2] { 46, 33 },
			new short[2] { 47, 33 },
			new short[2] { 48, 33 },
			new short[2] { 49, 33 },
			new short[2] { 50, 33 },
			new short[2] { 51, 33 },
			new short[2] { 52, 33 },
			new short[2] { 53, 33 },
			new short[2] { 54, 33 },
			new short[2] { 55, 33 },
			new short[2] { 56, 33 },
			new short[2] { 109, 10 },
			new short[2] { 110, 10 },
			new short[2] { 111, 10 }
		}, new List<short[]>
		{
			new short[2] { 57, 33 },
			new short[2] { 58, 33 },
			new short[2] { 59, 33 },
			new short[2] { 63, 33 },
			new short[2] { 64, 33 },
			new short[2] { 65, 33 },
			new short[2] { 69, 33 },
			new short[2] { 70, 33 },
			new short[2] { 71, 33 },
			new short[2] { 75, 33 },
			new short[2] { 76, 33 },
			new short[2] { 77, 33 },
			new short[2] { 81, 33 },
			new short[2] { 82, 33 },
			new short[2] { 83, 33 },
			new short[2] { 87, 33 },
			new short[2] { 88, 33 },
			new short[2] { 89, 33 },
			new short[2] { 112, 30 },
			new short[2] { 113, 30 },
			new short[2] { 114, 30 },
			new short[2] { 124, 30 }
		}, new List<short[]>
		{
			new short[2] { 93, 33 },
			new short[2] { 94, 33 },
			new short[2] { 95, 33 },
			new short[2] { 96, 33 },
			new short[2] { 97, 33 },
			new short[2] { 98, 33 },
			new short[2] { 99, 33 },
			new short[2] { 100, 33 },
			new short[2] { 101, 33 },
			new short[2] { 102, 33 },
			new short[2] { 103, 33 },
			new short[2] { 104, 33 },
			new short[2] { 105, 33 },
			new short[2] { 106, 33 },
			new short[2] { 107, 33 },
			new short[2] { 108, 33 },
			new short[2] { 115, 40 },
			new short[2] { 116, 40 },
			new short[2] { 117, 40 },
			new short[2] { 124, 30 }
		}, new List<short[]>
		{
			new short[2] { 72, 1 },
			new short[2] { 85, 1 },
			new short[2] { 86, 1 },
			new short[2] { 66, 1 },
			new short[2] { 90, 1 }
		}, new List<short[]>(), new List<short[]>(), new short[0], 554, 555, null, null, EMapAreaAreaDirection.South, new List<short>(), new List<short>(), showDarkAshStatus: true));
		_dataArray.Add(new MapAreaItem(134, 15, 556, 557, 20, new sbyte[2] { 19, 14 }, new AreaTravelRoute[0], "largemap_part_1_changgui_1", new string[4] { "largemap_part_2_icon_hei_1", "largemap_part_2_icon_jin_1", "largemap_part_2_icon_lan_1", "largemap_part_2_icon_hui_1" }, null, new short[2] { 160, 170 }, 0, 0, 5, new List<EnemyNestCreationInfo[]>
		{
			new EnemyNestCreationInfo[2]
			{
				new EnemyNestCreationInfo(0, 2),
				new EnemyNestCreationInfo(1, 2)
			},
			new EnemyNestCreationInfo[2]
			{
				new EnemyNestCreationInfo(1, 2),
				new EnemyNestCreationInfo(2, 3)
			},
			new EnemyNestCreationInfo[4]
			{
				new EnemyNestCreationInfo(2, 3),
				new EnemyNestCreationInfo(3, 3),
				new EnemyNestCreationInfo(4, 4),
				new EnemyNestCreationInfo(5, 4)
			},
			new EnemyNestCreationInfo[1]
			{
				new EnemyNestCreationInfo(12, 2)
			}
		}, new List<IntPair>
		{
			new IntPair(0, 6),
			new IntPair(6, 12),
			new IntPair(12, 24),
			new IntPair(0, 0)
		}, new short[1] { 35 }, new sbyte[1] { 37 }, -1, 0, null, new List<short[]>
		{
			new short[2] { 39, 33 },
			new short[2] { 40, 33 },
			new short[2] { 41, 33 },
			new short[2] { 42, 33 },
			new short[2] { 43, 33 },
			new short[2] { 44, 33 },
			new short[2] { 45, 33 },
			new short[2] { 46, 33 },
			new short[2] { 47, 33 },
			new short[2] { 48, 33 },
			new short[2] { 49, 33 },
			new short[2] { 50, 33 },
			new short[2] { 51, 33 },
			new short[2] { 52, 33 },
			new short[2] { 53, 33 },
			new short[2] { 54, 33 },
			new short[2] { 55, 33 },
			new short[2] { 56, 33 },
			new short[2] { 109, 10 },
			new short[2] { 110, 10 },
			new short[2] { 111, 10 }
		}, new List<short[]>
		{
			new short[2] { 57, 33 },
			new short[2] { 58, 33 },
			new short[2] { 59, 33 },
			new short[2] { 63, 33 },
			new short[2] { 64, 33 },
			new short[2] { 65, 33 },
			new short[2] { 69, 33 },
			new short[2] { 70, 33 },
			new short[2] { 71, 33 },
			new short[2] { 75, 33 },
			new short[2] { 76, 33 },
			new short[2] { 77, 33 },
			new short[2] { 81, 33 },
			new short[2] { 82, 33 },
			new short[2] { 83, 33 },
			new short[2] { 87, 33 },
			new short[2] { 88, 33 },
			new short[2] { 89, 33 },
			new short[2] { 112, 30 },
			new short[2] { 113, 30 },
			new short[2] { 114, 30 },
			new short[2] { 124, 30 }
		}, new List<short[]>
		{
			new short[2] { 93, 33 },
			new short[2] { 94, 33 },
			new short[2] { 95, 33 },
			new short[2] { 96, 33 },
			new short[2] { 97, 33 },
			new short[2] { 98, 33 },
			new short[2] { 99, 33 },
			new short[2] { 100, 33 },
			new short[2] { 101, 33 },
			new short[2] { 102, 33 },
			new short[2] { 103, 33 },
			new short[2] { 104, 33 },
			new short[2] { 105, 33 },
			new short[2] { 106, 33 },
			new short[2] { 107, 33 },
			new short[2] { 108, 33 },
			new short[2] { 115, 40 },
			new short[2] { 116, 40 },
			new short[2] { 117, 40 },
			new short[2] { 124, 30 }
		}, new List<short[]>
		{
			new short[2] { 72, 1 },
			new short[2] { 85, 1 },
			new short[2] { 86, 1 },
			new short[2] { 66, 1 },
			new short[2] { 90, 1 }
		}, new List<short[]>(), new List<short[]>(), new short[0], 558, 559, null, null, EMapAreaAreaDirection.South, new List<short>(), new List<short>(), showDarkAshStatus: true));
		_dataArray.Add(new MapAreaItem(135, 15, 560, 561, 20, new sbyte[2] { 19, 16 }, new AreaTravelRoute[0], "largemap_part_1_changgui_0", new string[4] { "largemap_part_2_icon_hei_0", "largemap_part_2_icon_jin_0", "largemap_part_2_icon_lan_0", "largemap_part_2_icon_hui_0" }, null, new short[2] { 160, 250 }, 0, 0, 5, new List<EnemyNestCreationInfo[]>
		{
			new EnemyNestCreationInfo[2]
			{
				new EnemyNestCreationInfo(0, 2),
				new EnemyNestCreationInfo(1, 2)
			},
			new EnemyNestCreationInfo[2]
			{
				new EnemyNestCreationInfo(1, 2),
				new EnemyNestCreationInfo(2, 3)
			},
			new EnemyNestCreationInfo[4]
			{
				new EnemyNestCreationInfo(2, 3),
				new EnemyNestCreationInfo(3, 3),
				new EnemyNestCreationInfo(4, 4),
				new EnemyNestCreationInfo(5, 4)
			},
			new EnemyNestCreationInfo[1]
			{
				new EnemyNestCreationInfo(12, 2)
			}
		}, new List<IntPair>
		{
			new IntPair(0, 6),
			new IntPair(6, 12),
			new IntPair(12, 24),
			new IntPair(0, 0)
		}, new short[1] { 34 }, new sbyte[1] { 36 }, -1, 0, null, new List<short[]>
		{
			new short[2] { 39, 33 },
			new short[2] { 40, 33 },
			new short[2] { 41, 33 },
			new short[2] { 42, 33 },
			new short[2] { 43, 33 },
			new short[2] { 44, 33 },
			new short[2] { 45, 33 },
			new short[2] { 46, 33 },
			new short[2] { 47, 33 },
			new short[2] { 48, 33 },
			new short[2] { 49, 33 },
			new short[2] { 50, 33 },
			new short[2] { 51, 33 },
			new short[2] { 52, 33 },
			new short[2] { 53, 33 },
			new short[2] { 54, 33 },
			new short[2] { 55, 33 },
			new short[2] { 56, 33 },
			new short[2] { 109, 10 },
			new short[2] { 110, 10 },
			new short[2] { 111, 10 }
		}, new List<short[]>
		{
			new short[2] { 57, 33 },
			new short[2] { 58, 33 },
			new short[2] { 59, 33 },
			new short[2] { 63, 33 },
			new short[2] { 64, 33 },
			new short[2] { 65, 33 },
			new short[2] { 69, 33 },
			new short[2] { 70, 33 },
			new short[2] { 71, 33 },
			new short[2] { 75, 33 },
			new short[2] { 76, 33 },
			new short[2] { 77, 33 },
			new short[2] { 81, 33 },
			new short[2] { 82, 33 },
			new short[2] { 83, 33 },
			new short[2] { 87, 33 },
			new short[2] { 88, 33 },
			new short[2] { 89, 33 },
			new short[2] { 112, 30 },
			new short[2] { 113, 30 },
			new short[2] { 114, 30 },
			new short[2] { 124, 30 }
		}, new List<short[]>
		{
			new short[2] { 93, 33 },
			new short[2] { 94, 33 },
			new short[2] { 95, 33 },
			new short[2] { 96, 33 },
			new short[2] { 97, 33 },
			new short[2] { 98, 33 },
			new short[2] { 99, 33 },
			new short[2] { 100, 33 },
			new short[2] { 101, 33 },
			new short[2] { 102, 33 },
			new short[2] { 103, 33 },
			new short[2] { 104, 33 },
			new short[2] { 105, 33 },
			new short[2] { 106, 33 },
			new short[2] { 107, 33 },
			new short[2] { 108, 33 },
			new short[2] { 115, 40 },
			new short[2] { 116, 40 },
			new short[2] { 117, 40 },
			new short[2] { 124, 30 }
		}, new List<short[]>
		{
			new short[2] { 72, 1 },
			new short[2] { 85, 1 },
			new short[2] { 86, 1 },
			new short[2] { 66, 1 },
			new short[2] { 90, 1 }
		}, new List<short[]>(), new List<short[]>(), new short[0], 562, 563, null, null, EMapAreaAreaDirection.South, new List<short>(), new List<short>(), showDarkAshStatus: true));
		_dataArray.Add(new MapAreaItem(136, 0, 0, 1, 20, new sbyte[2] { -1, -1 }, new AreaTravelRoute[0], "{largemap_part_1_changgui_0}", new string[4] { "", "", "", "" }, null, new short[2] { -1, -1 }, 0, 0, 5, new List<EnemyNestCreationInfo[]>(), new List<IntPair>(), new short[1] { 17 }, new sbyte[1], 17, -1, "Born2", new List<short[]>
		{
			new short[2] { 39, 33 },
			new short[2] { 40, 33 },
			new short[2] { 41, 33 },
			new short[2] { 42, 33 },
			new short[2] { 43, 33 },
			new short[2] { 44, 33 },
			new short[2] { 45, 33 },
			new short[2] { 46, 33 },
			new short[2] { 47, 33 },
			new short[2] { 48, 33 },
			new short[2] { 49, 33 },
			new short[2] { 50, 33 },
			new short[2] { 51, 33 },
			new short[2] { 52, 33 },
			new short[2] { 53, 33 },
			new short[2] { 54, 33 },
			new short[2] { 55, 33 },
			new short[2] { 56, 33 },
			new short[2] { 109, 10 },
			new short[2] { 110, 10 },
			new short[2] { 111, 10 }
		}, new List<short[]>
		{
			new short[2] { 57, 33 },
			new short[2] { 58, 33 },
			new short[2] { 59, 33 },
			new short[2] { 63, 33 },
			new short[2] { 64, 33 },
			new short[2] { 65, 33 },
			new short[2] { 69, 33 },
			new short[2] { 70, 33 },
			new short[2] { 71, 33 },
			new short[2] { 75, 33 },
			new short[2] { 76, 33 },
			new short[2] { 77, 33 },
			new short[2] { 81, 33 },
			new short[2] { 82, 33 },
			new short[2] { 83, 33 },
			new short[2] { 87, 33 },
			new short[2] { 88, 33 },
			new short[2] { 89, 33 },
			new short[2] { 112, 30 },
			new short[2] { 113, 30 },
			new short[2] { 114, 30 },
			new short[2] { 124, 30 }
		}, new List<short[]>
		{
			new short[2] { 93, 33 },
			new short[2] { 94, 33 },
			new short[2] { 95, 33 },
			new short[2] { 96, 33 },
			new short[2] { 97, 33 },
			new short[2] { 98, 33 },
			new short[2] { 99, 33 },
			new short[2] { 100, 33 },
			new short[2] { 101, 33 },
			new short[2] { 102, 33 },
			new short[2] { 103, 33 },
			new short[2] { 104, 33 },
			new short[2] { 105, 33 },
			new short[2] { 106, 33 },
			new short[2] { 107, 33 },
			new short[2] { 108, 33 },
			new short[2] { 115, 40 },
			new short[2] { 116, 40 },
			new short[2] { 117, 40 },
			new short[2] { 124, 30 }
		}, new List<short[]>
		{
			new short[2] { 60, 1 },
			new short[2] { 78, 1 },
			new short[2] { 61, 1 },
			new short[2] { 79, 1 },
			new short[2] { 62, 1 },
			new short[2] { 80, 1 }
		}, new List<short[]>(), new List<short[]>(), new short[0], 564, 565, null, null, EMapAreaAreaDirection.South, new List<short>(), new List<short>(), showDarkAshStatus: false));
		_dataArray.Add(new MapAreaItem(137, 0, 566, 567, 20, new sbyte[2] { -1, -1 }, new AreaTravelRoute[0], "{largemap_part_1_changgui_0}", new string[4] { "", "", "", "" }, null, new short[2] { -1, -1 }, 0, 0, 5, new List<EnemyNestCreationInfo[]>(), new List<IntPair>(), new short[1] { 16 }, new sbyte[1], 16, -1, null, new List<short[]>
		{
			new short[2] { 39, 33 },
			new short[2] { 40, 33 },
			new short[2] { 41, 33 },
			new short[2] { 42, 33 },
			new short[2] { 43, 33 },
			new short[2] { 44, 33 },
			new short[2] { 45, 33 },
			new short[2] { 46, 33 },
			new short[2] { 47, 33 },
			new short[2] { 48, 33 },
			new short[2] { 49, 33 },
			new short[2] { 50, 33 },
			new short[2] { 51, 33 },
			new short[2] { 52, 33 },
			new short[2] { 53, 33 },
			new short[2] { 54, 33 },
			new short[2] { 55, 33 },
			new short[2] { 56, 33 },
			new short[2] { 109, 10 },
			new short[2] { 110, 10 },
			new short[2] { 111, 10 }
		}, new List<short[]>
		{
			new short[2] { 57, 33 },
			new short[2] { 58, 33 },
			new short[2] { 59, 33 },
			new short[2] { 63, 33 },
			new short[2] { 64, 33 },
			new short[2] { 65, 33 },
			new short[2] { 69, 33 },
			new short[2] { 70, 33 },
			new short[2] { 71, 33 },
			new short[2] { 75, 33 },
			new short[2] { 76, 33 },
			new short[2] { 77, 33 },
			new short[2] { 81, 33 },
			new short[2] { 82, 33 },
			new short[2] { 83, 33 },
			new short[2] { 87, 33 },
			new short[2] { 88, 33 },
			new short[2] { 89, 33 },
			new short[2] { 112, 30 },
			new short[2] { 113, 30 },
			new short[2] { 114, 30 },
			new short[2] { 124, 30 }
		}, new List<short[]>
		{
			new short[2] { 93, 33 },
			new short[2] { 94, 33 },
			new short[2] { 95, 33 },
			new short[2] { 96, 33 },
			new short[2] { 97, 33 },
			new short[2] { 98, 33 },
			new short[2] { 99, 33 },
			new short[2] { 100, 33 },
			new short[2] { 101, 33 },
			new short[2] { 102, 33 },
			new short[2] { 103, 33 },
			new short[2] { 104, 33 },
			new short[2] { 105, 33 },
			new short[2] { 106, 33 },
			new short[2] { 107, 33 },
			new short[2] { 108, 33 },
			new short[2] { 115, 40 },
			new short[2] { 116, 40 },
			new short[2] { 117, 40 },
			new short[2] { 124, 30 }
		}, new List<short[]>
		{
			new short[2] { 60, 1 },
			new short[2] { 78, 1 },
			new short[2] { 61, 1 },
			new short[2] { 79, 1 },
			new short[2] { 62, 1 },
			new short[2] { 80, 1 }
		}, new List<short[]>(), new List<short[]>(), new short[0], 568, 569, null, null, EMapAreaAreaDirection.South, new List<short>(), new List<short>(), showDarkAshStatus: false));
	}

	public override void Init()
	{
		base.Init();
		_dataArray = new List<MapAreaItem>(138);
		CreateItems0();
		CreateItems1();
		CreateItems2();
	}
}
