using System;
using System.Collections.Generic;
using Config.Common;
using Config.ConfigCells;
using Config.ConfigCells.Character;

namespace Config;

[Serializable]
public class MapPickups : ConfigData<MapPickupsItem, short>
{
	public static class DefKey
	{
		public const short FoodResource = 0;

		public const short WoodResource = 1;

		public const short StonResource = 2;

		public const short JadeResource = 3;

		public const short SilkResource = 4;

		public const short HerbalResource = 5;

		public const short MoneyResource = 6;

		public const short AuthorityResource = 7;
	}

	public static class DefValue
	{
		public static MapPickupsItem FoodResource => Instance[(short)0];

		public static MapPickupsItem WoodResource => Instance[(short)1];

		public static MapPickupsItem StonResource => Instance[(short)2];

		public static MapPickupsItem JadeResource => Instance[(short)3];

		public static MapPickupsItem SilkResource => Instance[(short)4];

		public static MapPickupsItem HerbalResource => Instance[(short)5];

		public static MapPickupsItem MoneyResource => Instance[(short)6];

		public static MapPickupsItem AuthorityResource => Instance[(short)7];
	}

	public static MapPickups Instance = new MapPickups();

	private readonly HashSet<string> RequiredFields = new HashSet<string>
	{
		"BlockList", "ItemGroup", "ShowConditionInformation", "ShowConditionOrganizationApproving", "InstantNotification", "ExtraBonusAddInstantNotification", "ExtraBonusReplaceInstantNotification", "EventSecondItemRewards", "EventSecondResourceRewards", "EventSecondPropertyRewards",
		"EventSecondItemRewardSelection1", "EventSecondItemRewardSelection2", "TemplateId", "Name", "Icon", "TipsContent", "EventMainContent"
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
		_dataArray.Add(new MapPickupsItem(0, 0, EMapPickupsType.Resource, EMapPickupsType2.Resource, "map_eventicon_0", 1, 2, new byte[15]
		{
			3, 5, 4, 4, 3, 5, 4, 4, 4, 2,
			2, 3, 4, 5, 4
		}, new List<short> { 39, 40, 41, 75, 76, 77, 78, 79, 80, 93 }, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[8] { 50, 100, 200, 400, 800, 1600, 2400, 3200 }, new sbyte[0], default(PresetItemTemplateId), new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new short[6] { 50, 0, 0, 0, 0, 0 }, new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), 179, 244, -1, 2, new int[0], new int[0], new int[0], new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>(), new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(1, 3, EMapPickupsType.Resource, EMapPickupsType2.Resource, "map_eventicon_1", 4, 2, new byte[15]
		{
			2, 5, 4, 4, 3, 4, 5, 4, 4, 4,
			3, 5, 4, 2, 3
		}, new List<short>
		{
			42, 43, 44, 81, 82, 83, 84, 85, 86, 94,
			95, 96
		}, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[8] { 50, 100, 200, 400, 800, 1600, 2400, 3200 }, new sbyte[0], default(PresetItemTemplateId), new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new short[6] { 0, 50, 0, 0, 0, 0 }, new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), 179, 244, -1, 5, new int[0], new int[0], new int[0], new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>(), new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(2, 6, EMapPickupsType.Resource, EMapPickupsType2.Resource, "map_eventicon_2", 7, 2, new byte[15]
		{
			4, 4, 3, 2, 5, 4, 2, 3, 4, 5,
			5, 3, 4, 4, 4
		}, new List<short>
		{
			45, 46, 47, 57, 58, 59, 60, 61, 62, 97,
			98, 99
		}, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[8] { 50, 100, 200, 400, 800, 1600, 2400, 3200 }, new sbyte[0], default(PresetItemTemplateId), new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new short[6] { 0, 0, 50, 0, 0, 0 }, new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), 179, 244, -1, 8, new int[0], new int[0], new int[0], new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>(), new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(3, 9, EMapPickupsType.Resource, EMapPickupsType2.Resource, "map_eventicon_3", 10, 2, new byte[15]
		{
			4, 2, 5, 5, 2, 3, 5, 4, 3, 3,
			4, 4, 4, 4, 4
		}, new List<short>
		{
			54, 55, 56, 87, 88, 89, 90, 91, 92, 106,
			107, 108
		}, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[8] { 50, 100, 200, 400, 800, 1600, 2400, 3200 }, new sbyte[0], default(PresetItemTemplateId), new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new short[6] { 0, 0, 0, 50, 0, 0 }, new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), 179, 244, -1, 11, new int[0], new int[0], new int[0], new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>(), new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(4, 12, EMapPickupsType.Resource, EMapPickupsType2.Resource, "map_eventicon_4", 13, 2, new byte[15]
		{
			3, 4, 5, 4, 2, 2, 4, 5, 4, 3,
			3, 5, 4, 4, 4
		}, new List<short>
		{
			48, 49, 50, 69, 70, 71, 72, 73, 74, 103,
			104, 105
		}, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[8] { 50, 100, 200, 400, 800, 1600, 2400, 3200 }, new sbyte[0], default(PresetItemTemplateId), new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new short[6] { 0, 0, 0, 0, 50, 0 }, new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), 179, 244, -1, 14, new int[0], new int[0], new int[0], new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>(), new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(5, 15, EMapPickupsType.Resource, EMapPickupsType2.Resource, "map_eventicon_5", 16, 2, new byte[15]
		{
			3, 4, 5, 4, 2, 3, 4, 4, 3, 5,
			2, 5, 4, 4, 4
		}, new List<short>
		{
			51, 52, 53, 63, 64, 65, 66, 67, 68, 100,
			101, 102
		}, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[8] { 50, 100, 200, 400, 800, 1600, 2400, 3200 }, new sbyte[0], default(PresetItemTemplateId), new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new short[6] { 0, 0, 0, 0, 0, 50 }, new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), 179, 244, -1, 17, new int[0], new int[0], new int[0], new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>(), new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(6, 18, EMapPickupsType.Resource, EMapPickupsType2.Resource, "map_eventicon_6", 19, 0, new byte[15]
		{
			5, 4, 2, 4, 4, 4, 3, 4, 5, 3,
			3, 2, 4, 4, 5
		}, new List<short>
		{
			39, 40, 41, 42, 43, 44, 45, 46, 47, 54,
			55, 56, 48, 49, 50, 51, 52, 53, 1, 2,
			3, 4, 5, 6, 7, 8, 9, 10, 11, 12,
			13, 14, 15, 34, 35, 36, 19, 20, 21, 22,
			23, 24, 25, 26, 27, 28, 29, 30, 31, 32,
			33
		}, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[8] { 250, 500, 1000, 2000, 4000, 8000, 12000, 16000 }, new sbyte[0], default(PresetItemTemplateId), new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), 179, 244, -1, 20, new int[0], new int[0], new int[0], new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>(), new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(7, 21, EMapPickupsType.Resource, EMapPickupsType2.Resource, "map_eventicon_7", 22, 0, new byte[15]
		{
			5, 4, 3, 4, 4, 4, 4, 4, 4, 3,
			3, 2, 5, 5, 2
		}, new List<short>
		{
			39, 40, 41, 42, 43, 44, 45, 46, 47, 54,
			55, 56, 48, 49, 50, 51, 52, 53, 1, 2,
			3, 4, 5, 6, 7, 8, 9, 10, 11, 12,
			13, 14, 15, 34, 35, 36, 19, 20, 21, 22,
			23, 24, 25, 26, 27, 28, 29, 30, 31, 32,
			33
		}, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[8] { 25, 50, 100, 200, 400, 800, 1200, 1600 }, new sbyte[0], default(PresetItemTemplateId), new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), 179, 244, -1, 23, new int[0], new int[0], new int[0], new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>(), new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(8, 24, EMapPickupsType.Item, EMapPickupsType2.Material, "map_eventicon_0", 25, 1, new byte[15]
		{
			1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
			1, 1, 1, 1, 1
		}, new List<short> { 39, 40, 41, 75, 76, 77, 78, 79, 80, 93 }, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[8] { 0, 1, 2, 3, 4, 4, 5, 5 }, new PresetItemTemplateId("Material", 56), new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new short[6], new bool[12]
		{
			true, false, false, false, true, true, true, false, false, false,
			true, true
		}, -1, new OrganizationApproving(), 180, -1, 247, 26, new int[0], new int[0], new int[0], new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>(), new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(9, 27, EMapPickupsType.Item, EMapPickupsType2.Material, "map_eventicon_0", 25, 1, new byte[15]
		{
			1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
			1, 1, 1, 1, 1
		}, new List<short>
		{
			81, 82, 83, 84, 85, 86, 75, 76, 77, 78,
			79, 80
		}, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[8] { 0, 1, 2, 3, 4, 4, 5, 5 }, new PresetItemTemplateId("Material", 63), new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new short[6], new bool[12]
		{
			true, false, false, false, false, false, false, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), 180, -1, 247, 28, new int[0], new int[0], new int[0], new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>(), new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(10, 29, EMapPickupsType.Item, EMapPickupsType2.Material, "map_eventicon_0", 25, 1, new byte[15]
		{
			1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
			1, 1, 1, 1, 1
		}, new List<short> { 39, 40, 41, 75, 76, 77, 78, 79, 80, 93 }, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[8] { 0, 1, 2, 3, 4, 4, 5, 5 }, new PresetItemTemplateId("Material", 70), new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new short[6], new bool[12]
		{
			false, true, true, true, false, false, false, true, true, true,
			false, false
		}, -1, new OrganizationApproving(), 180, -1, 247, 30, new int[0], new int[0], new int[0], new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>(), new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(11, 31, EMapPickupsType.Item, EMapPickupsType2.Material, "map_eventicon_0", 25, 1, new byte[15]
		{
			1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
			1, 1, 1, 1, 1
		}, new List<short> { 87, 88, 89, 90, 91, 92, 106, 107, 108, 93 }, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[8] { 0, 1, 2, 3, 4, 4, 5, 5 }, new PresetItemTemplateId("Material", 77), new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new short[6], new bool[12]
		{
			false, true, true, true, true, true, true, false, false, false,
			false, false
		}, -1, new OrganizationApproving(), 180, -1, 247, 32, new int[0], new int[0], new int[0], new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>(), new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(12, 33, EMapPickupsType.Item, EMapPickupsType2.Material, "map_eventicon_1", 34, 1, new byte[15]
		{
			1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
			1, 1, 1, 1, 1
		}, new List<short>
		{
			42, 43, 44, 81, 82, 83, 84, 85, 86, 94,
			95, 96
		}, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[8] { 1, 2, 3, 4, 5, 5, 6, 6 }, new PresetItemTemplateId("Material", 0), new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), 181, -1, 247, 35, new int[0], new int[0], new int[0], new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>(), new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(13, 36, EMapPickupsType.Item, EMapPickupsType2.Material, "map_eventicon_1", 34, 1, new byte[15]
		{
			1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
			1, 1, 1, 1, 1
		}, new List<short>
		{
			42, 43, 44, 81, 82, 83, 84, 85, 86, 94,
			95, 96
		}, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[8] { 1, 2, 3, 4, 5, 5, 6, 6 }, new PresetItemTemplateId("Material", 7), new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), 181, -1, 247, 37, new int[0], new int[0], new int[0], new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>(), new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(14, 38, EMapPickupsType.Item, EMapPickupsType2.Material, "map_eventicon_2", 39, 1, new byte[15]
		{
			1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
			1, 1, 1, 1, 1
		}, new List<short>
		{
			45, 46, 47, 57, 58, 59, 60, 61, 62, 97,
			98, 99
		}, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[8] { 1, 2, 3, 4, 5, 5, 6, 6 }, new PresetItemTemplateId("Material", 14), new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), 181, -1, 247, 40, new int[0], new int[0], new int[0], new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>(), new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(15, 41, EMapPickupsType.Item, EMapPickupsType2.Material, "map_eventicon_2", 39, 1, new byte[15]
		{
			1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
			1, 1, 1, 1, 1
		}, new List<short>
		{
			45, 46, 47, 57, 58, 59, 60, 61, 62, 97,
			98, 99
		}, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[8] { 1, 2, 3, 4, 5, 5, 6, 6 }, new PresetItemTemplateId("Material", 21), new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), 181, -1, 247, 42, new int[0], new int[0], new int[0], new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>(), new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(16, 43, EMapPickupsType.Item, EMapPickupsType2.Material, "map_eventicon_3", 44, 1, new byte[15]
		{
			1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
			1, 1, 1, 1, 1
		}, new List<short>
		{
			54, 55, 56, 87, 88, 89, 90, 91, 92, 106,
			107, 108
		}, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[8] { 1, 2, 3, 4, 5, 5, 6, 6 }, new PresetItemTemplateId("Material", 28), new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), 181, -1, 247, 45, new int[0], new int[0], new int[0], new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>(), new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(17, 46, EMapPickupsType.Item, EMapPickupsType2.Material, "map_eventicon_3", 44, 1, new byte[15]
		{
			1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
			1, 1, 1, 1, 1
		}, new List<short>
		{
			54, 55, 56, 87, 88, 89, 90, 91, 92, 106,
			107, 108
		}, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[8] { 1, 2, 3, 4, 5, 5, 6, 6 }, new PresetItemTemplateId("Material", 35), new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), 181, -1, 247, 47, new int[0], new int[0], new int[0], new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>(), new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(18, 48, EMapPickupsType.Item, EMapPickupsType2.Material, "map_eventicon_4", 49, 1, new byte[15]
		{
			1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
			1, 1, 1, 1, 1
		}, new List<short> { 69, 70, 71, 72, 73, 74, 103, 104, 105 }, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[8] { 1, 2, 3, 4, 5, 5, 6, 6 }, new PresetItemTemplateId("Material", 42), new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), 181, -1, 247, 50, new int[0], new int[0], new int[0], new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>(), new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(19, 51, EMapPickupsType.Item, EMapPickupsType2.Material, "map_eventicon_4", 49, 1, new byte[15]
		{
			1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
			1, 1, 1, 1, 1
		}, new List<short> { 48, 49, 50, 69, 70, 71, 103, 104, 105 }, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[8] { 1, 2, 3, 4, 5, 5, 6, 6 }, new PresetItemTemplateId("Material", 49), new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), 181, -1, 247, 52, new int[0], new int[0], new int[0], new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>(), new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(20, 53, EMapPickupsType.Item, EMapPickupsType2.Material, "map_eventicon_5", 54, 0, new byte[15]
		{
			1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
			1, 1, 1, 1, 1
		}, new List<short>
		{
			51, 52, 53, 57, 58, 59, 60, 61, 62, 63,
			64, 65, 66, 67, 68, 69, 70, 71, 72, 73,
			74, 75, 76, 77, 78, 79, 80, 81, 82, 83,
			84, 85, 86, 87, 88, 89, 90, 91, 92
		}, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[3] { 2, 4, 6 }, new PresetItemTemplateId("Material", 140), new sbyte[3] { 1, 3, 5 }, new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), 182, 254, -1, 55, new int[0], new int[0], new int[0], new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>(), new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(21, 56, EMapPickupsType.Item, EMapPickupsType2.Material, "map_eventicon_5", 54, 0, new byte[15]
		{
			1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
			1, 1, 1, 1, 1
		}, new List<short>
		{
			51, 52, 53, 57, 58, 59, 60, 61, 62, 63,
			64, 65, 66, 67, 68, 69, 70, 71, 72, 73,
			74, 75, 76, 77, 78, 79, 80, 81, 82, 83,
			84, 85, 86, 87, 88, 89, 90, 91, 92
		}, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[3] { 2, 4, 6 }, new PresetItemTemplateId("Material", 144), new sbyte[3] { 1, 3, 5 }, new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), 182, 254, -1, 57, new int[0], new int[0], new int[0], new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>(), new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(22, 58, EMapPickupsType.Item, EMapPickupsType2.Material, "map_eventicon_5", 54, 0, new byte[15]
		{
			1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
			1, 1, 1, 1, 1
		}, new List<short>
		{
			51, 52, 53, 57, 58, 59, 60, 61, 62, 63,
			64, 65, 66, 67, 68, 69, 70, 71, 72, 73,
			74, 75, 76, 77, 78, 79, 80, 81, 82, 83,
			84, 85, 86, 87, 88, 89, 90, 91, 92
		}, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[3] { 2, 4, 6 }, new PresetItemTemplateId("Material", 148), new sbyte[3] { 1, 3, 5 }, new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), 182, 254, -1, 59, new int[0], new int[0], new int[0], new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>(), new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(23, 60, EMapPickupsType.Item, EMapPickupsType2.Material, "map_eventicon_5", 54, 0, new byte[15]
		{
			1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
			1, 1, 1, 1, 1
		}, new List<short>
		{
			51, 52, 53, 57, 58, 59, 60, 61, 62, 63,
			64, 65, 66, 67, 68, 69, 70, 71, 72, 73,
			74, 75, 76, 77, 78, 79, 80, 81, 82, 83,
			84, 85, 86, 87, 88, 89, 90, 91, 92
		}, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[3] { 2, 4, 6 }, new PresetItemTemplateId("Material", 152), new sbyte[3] { 1, 3, 5 }, new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), 182, 254, -1, 61, new int[0], new int[0], new int[0], new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>(), new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(24, 62, EMapPickupsType.Item, EMapPickupsType2.Material, "map_eventicon_5", 54, 0, new byte[15]
		{
			1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
			1, 1, 1, 1, 1
		}, new List<short>
		{
			51, 52, 53, 57, 58, 59, 60, 61, 62, 63,
			64, 65, 66, 67, 68, 69, 70, 71, 72, 73,
			74, 75, 76, 77, 78, 79, 80, 81, 82, 83,
			84, 85, 86, 87, 88, 89, 90, 91, 92
		}, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[3] { 2, 4, 6 }, new PresetItemTemplateId("Material", 156), new sbyte[3] { 1, 3, 5 }, new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), 182, 254, -1, 63, new int[0], new int[0], new int[0], new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>(), new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(25, 64, EMapPickupsType.Item, EMapPickupsType2.Material, "map_eventicon_5", 54, 0, new byte[15]
		{
			1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
			1, 1, 1, 1, 1
		}, new List<short>
		{
			51, 52, 53, 57, 58, 59, 60, 61, 62, 63,
			64, 65, 66, 67, 68, 69, 70, 71, 72, 73,
			74, 75, 76, 77, 78, 79, 80, 81, 82, 83,
			84, 85, 86, 87, 88, 89, 90, 91, 92
		}, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[3] { 2, 4, 6 }, new PresetItemTemplateId("Material", 160), new sbyte[3] { 1, 3, 5 }, new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), 182, 254, -1, 65, new int[0], new int[0], new int[0], new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>(), new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(26, 66, EMapPickupsType.Item, EMapPickupsType2.Material, "map_eventicon_5", 54, 0, new byte[15]
		{
			1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
			1, 1, 1, 1, 1
		}, new List<short>
		{
			51, 52, 53, 57, 58, 59, 60, 61, 62, 63,
			64, 65, 66, 67, 68, 69, 70, 71, 72, 73,
			74, 75, 76, 77, 78, 79, 80, 81, 82, 83,
			84, 85, 86, 87, 88, 89, 90, 91, 92
		}, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[3] { 2, 4, 6 }, new PresetItemTemplateId("Material", 164), new sbyte[3] { 1, 3, 5 }, new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), 182, 254, -1, 67, new int[0], new int[0], new int[0], new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>(), new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(27, 68, EMapPickupsType.Item, EMapPickupsType2.Material, "map_eventicon_5", 54, 0, new byte[15]
		{
			1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
			1, 1, 1, 1, 1
		}, new List<short>
		{
			51, 52, 53, 57, 58, 59, 60, 61, 62, 63,
			64, 65, 66, 67, 68, 69, 70, 71, 72, 73,
			74, 75, 76, 77, 78, 79, 80, 81, 82, 83,
			84, 85, 86, 87, 88, 89, 90, 91, 92
		}, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[3] { 2, 4, 6 }, new PresetItemTemplateId("Material", 168), new sbyte[3] { 1, 3, 5 }, new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), 182, 254, -1, 69, new int[0], new int[0], new int[0], new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>(), new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(28, 70, EMapPickupsType.Item, EMapPickupsType2.Material, "map_eventicon_5", 54, 0, new byte[15]
		{
			1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
			1, 1, 1, 1, 1
		}, new List<short>
		{
			51, 52, 53, 57, 58, 59, 60, 61, 62, 63,
			64, 65, 66, 67, 68, 69, 70, 71, 72, 73,
			74, 75, 76, 77, 78, 79, 80, 81, 82, 83,
			84, 85, 86, 87, 88, 89, 90, 91, 92
		}, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[3] { 2, 4, 6 }, new PresetItemTemplateId("Material", 172), new sbyte[3] { 1, 3, 5 }, new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), 182, 254, -1, 71, new int[0], new int[0], new int[0], new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>(), new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(29, 72, EMapPickupsType.Item, EMapPickupsType2.Material, "map_eventicon_5", 54, 0, new byte[15]
		{
			1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
			1, 1, 1, 1, 1
		}, new List<short>
		{
			51, 52, 53, 57, 58, 59, 60, 61, 62, 63,
			64, 65, 66, 67, 68, 69, 70, 71, 72, 73,
			74, 75, 76, 77, 78, 79, 80, 81, 82, 83,
			84, 85, 86, 87, 88, 89, 90, 91, 92
		}, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[3] { 2, 4, 6 }, new PresetItemTemplateId("Material", 176), new sbyte[3] { 1, 3, 5 }, new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), 182, 254, -1, 73, new int[0], new int[0], new int[0], new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>(), new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(30, 74, EMapPickupsType.Item, EMapPickupsType2.Material, "map_eventicon_5", 54, 0, new byte[15]
		{
			1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
			1, 1, 1, 1, 1
		}, new List<short>
		{
			51, 52, 53, 57, 58, 59, 60, 61, 62, 63,
			64, 65, 66, 67, 68, 69, 70, 71, 72, 73,
			74, 75, 76, 77, 78, 79, 80, 81, 82, 83,
			84, 85, 86, 87, 88, 89, 90, 91, 92
		}, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[3] { 2, 4, 6 }, new PresetItemTemplateId("Material", 180), new sbyte[3] { 1, 3, 5 }, new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), 182, 254, -1, 75, new int[0], new int[0], new int[0], new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>(), new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(31, 76, EMapPickupsType.Item, EMapPickupsType2.Material, "map_eventicon_5", 54, 0, new byte[15]
		{
			1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
			1, 1, 1, 1, 1
		}, new List<short>
		{
			51, 52, 53, 57, 58, 59, 60, 61, 62, 63,
			64, 65, 66, 67, 68, 69, 70, 71, 72, 73,
			74, 75, 76, 77, 78, 79, 80, 81, 82, 83,
			84, 85, 86, 87, 88, 89, 90, 91, 92
		}, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[3] { 2, 4, 6 }, new PresetItemTemplateId("Material", 184), new sbyte[3] { 1, 3, 5 }, new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), 182, 254, -1, 77, new int[0], new int[0], new int[0], new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>(), new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(32, 78, EMapPickupsType.Item, EMapPickupsType2.Material, "map_eventicon_5", 54, 0, new byte[15]
		{
			1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
			1, 1, 1, 1, 1
		}, new List<short>
		{
			51, 52, 53, 57, 58, 59, 60, 61, 62, 63,
			64, 65, 66, 67, 68, 69, 70, 71, 72, 73,
			74, 75, 76, 77, 78, 79, 80, 81, 82, 83,
			84, 85, 86, 87, 88, 89, 90, 91, 92
		}, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[3] { 2, 4, 6 }, new PresetItemTemplateId("Material", 188), new sbyte[3] { 1, 3, 5 }, new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), 182, 254, -1, 79, new int[0], new int[0], new int[0], new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>(), new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(33, 80, EMapPickupsType.Item, EMapPickupsType2.Material, "map_eventicon_5", 54, 0, new byte[15]
		{
			1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
			1, 1, 1, 1, 1
		}, new List<short>
		{
			51, 52, 53, 57, 58, 59, 60, 61, 62, 63,
			64, 65, 66, 67, 68, 69, 70, 71, 72, 73,
			74, 75, 76, 77, 78, 79, 80, 81, 82, 83,
			84, 85, 86, 87, 88, 89, 90, 91, 92
		}, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[3] { 2, 4, 6 }, new PresetItemTemplateId("Material", 192), new sbyte[3] { 1, 3, 5 }, new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), 182, 254, -1, 81, new int[0], new int[0], new int[0], new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>(), new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(34, 82, EMapPickupsType.Item, EMapPickupsType2.Material, "map_eventicon_5", 54, 0, new byte[15]
		{
			1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
			1, 1, 1, 1, 1
		}, new List<short>
		{
			51, 52, 53, 57, 58, 59, 60, 61, 62, 63,
			64, 65, 66, 67, 68, 69, 70, 71, 72, 73,
			74, 75, 76, 77, 78, 79, 80, 81, 82, 83,
			84, 85, 86, 87, 88, 89, 90, 91, 92
		}, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[3] { 2, 4, 6 }, new PresetItemTemplateId("Material", 196), new sbyte[3] { 1, 3, 5 }, new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), 182, 254, -1, 83, new int[0], new int[0], new int[0], new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>(), new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(35, 84, EMapPickupsType.Item, EMapPickupsType2.Material, "map_eventicon_5", 54, 0, new byte[15]
		{
			1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
			1, 1, 1, 1, 1
		}, new List<short>
		{
			51, 52, 53, 57, 58, 59, 60, 61, 62, 63,
			64, 65, 66, 67, 68, 69, 70, 71, 72, 73,
			74, 75, 76, 77, 78, 79, 80, 81, 82, 83,
			84, 85, 86, 87, 88, 89, 90, 91, 92
		}, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[3] { 2, 4, 6 }, new PresetItemTemplateId("Material", 200), new sbyte[3] { 1, 3, 5 }, new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), 182, 254, -1, 85, new int[0], new int[0], new int[0], new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>(), new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(36, 86, EMapPickupsType.Item, EMapPickupsType2.Material, "map_eventicon_5", 54, 0, new byte[15]
		{
			1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
			1, 1, 1, 1, 1
		}, new List<short>
		{
			51, 52, 53, 57, 58, 59, 60, 61, 62, 63,
			64, 65, 66, 67, 68, 69, 70, 71, 72, 73,
			74, 75, 76, 77, 78, 79, 80, 81, 82, 83,
			84, 85, 86, 87, 88, 89, 90, 91, 92
		}, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[3] { 2, 4, 6 }, new PresetItemTemplateId("Material", 204), new sbyte[3] { 1, 3, 5 }, new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), 182, 254, -1, 87, new int[0], new int[0], new int[0], new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>(), new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(37, 88, EMapPickupsType.Item, EMapPickupsType2.Material, "map_eventicon_5", 54, 0, new byte[15]
		{
			1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
			1, 1, 1, 1, 1
		}, new List<short>
		{
			51, 52, 53, 57, 58, 59, 60, 61, 62, 63,
			64, 65, 66, 67, 68, 69, 70, 71, 72, 73,
			74, 75, 76, 77, 78, 79, 80, 81, 82, 83,
			84, 85, 86, 87, 88, 89, 90, 91, 92
		}, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[3] { 2, 4, 6 }, new PresetItemTemplateId("Material", 208), new sbyte[3] { 1, 3, 5 }, new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), 182, 254, -1, 89, new int[0], new int[0], new int[0], new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>(), new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(38, 90, EMapPickupsType.Item, EMapPickupsType2.Material, "map_eventicon_5", 54, 0, new byte[15]
		{
			1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
			1, 1, 1, 1, 1
		}, new List<short>
		{
			51, 52, 53, 57, 58, 59, 60, 61, 62, 63,
			64, 65, 66, 67, 68, 69, 70, 71, 72, 73,
			74, 75, 76, 77, 78, 79, 80, 81, 82, 83,
			84, 85, 86, 87, 88, 89, 90, 91, 92
		}, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[3] { 2, 4, 6 }, new PresetItemTemplateId("Material", 212), new sbyte[3] { 1, 3, 5 }, new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), 182, 254, -1, 91, new int[0], new int[0], new int[0], new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>(), new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(39, 92, EMapPickupsType.Item, EMapPickupsType2.Material, "map_eventicon_5", 54, 0, new byte[15]
		{
			1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
			1, 1, 1, 1, 1
		}, new List<short>
		{
			51, 52, 53, 57, 58, 59, 60, 61, 62, 63,
			64, 65, 66, 67, 68, 69, 70, 71, 72, 73,
			74, 75, 76, 77, 78, 79, 80, 81, 82, 83,
			84, 85, 86, 87, 88, 89, 90, 91, 92
		}, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[3] { 2, 4, 6 }, new PresetItemTemplateId("Material", 216), new sbyte[3] { 1, 3, 5 }, new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), 182, 254, -1, 93, new int[0], new int[0], new int[0], new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>(), new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(40, 94, EMapPickupsType.Item, EMapPickupsType2.Material, "map_eventicon_5", 54, 0, new byte[15]
		{
			1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
			1, 1, 1, 1, 1
		}, new List<short>
		{
			51, 52, 53, 57, 58, 59, 60, 61, 62, 63,
			64, 65, 66, 67, 68, 69, 70, 71, 72, 73,
			74, 75, 76, 77, 78, 79, 80, 81, 82, 83,
			84, 85, 86, 87, 88, 89, 90, 91, 92
		}, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[3] { 2, 4, 6 }, new PresetItemTemplateId("Material", 220), new sbyte[3] { 1, 3, 5 }, new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), 182, 254, -1, 95, new int[0], new int[0], new int[0], new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>(), new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(41, 96, EMapPickupsType.Item, EMapPickupsType2.Material, "map_eventicon_5", 54, 0, new byte[15]
		{
			1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
			1, 1, 1, 1, 1
		}, new List<short>
		{
			51, 52, 53, 57, 58, 59, 60, 61, 62, 63,
			64, 65, 66, 67, 68, 69, 70, 71, 72, 73,
			74, 75, 76, 77, 78, 79, 80, 81, 82, 83,
			84, 85, 86, 87, 88, 89, 90, 91, 92
		}, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[3] { 2, 4, 6 }, new PresetItemTemplateId("Material", 224), new sbyte[3] { 1, 3, 5 }, new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), 182, 254, -1, 97, new int[0], new int[0], new int[0], new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>(), new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(42, 98, EMapPickupsType.Item, EMapPickupsType2.Material, "map_eventicon_5", 54, 0, new byte[15]
		{
			1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
			1, 1, 1, 1, 1
		}, new List<short>
		{
			51, 52, 53, 57, 58, 59, 60, 61, 62, 63,
			64, 65, 66, 67, 68, 69, 70, 71, 72, 73,
			74, 75, 76, 77, 78, 79, 80, 81, 82, 83,
			84, 85, 86, 87, 88, 89, 90, 91, 92
		}, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[3] { 2, 4, 6 }, new PresetItemTemplateId("Material", 228), new sbyte[3] { 1, 3, 5 }, new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), 182, 254, -1, 99, new int[0], new int[0], new int[0], new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>(), new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(43, 100, EMapPickupsType.Item, EMapPickupsType2.Material, "map_eventicon_5", 54, 0, new byte[15]
		{
			1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
			1, 1, 1, 1, 1
		}, new List<short>
		{
			51, 52, 53, 57, 58, 59, 60, 61, 62, 63,
			64, 65, 66, 67, 68, 69, 70, 71, 72, 73,
			74, 75, 76, 77, 78, 79, 80, 81, 82, 83,
			84, 85, 86, 87, 88, 89, 90, 91, 92
		}, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[3] { 2, 4, 6 }, new PresetItemTemplateId("Material", 232), new sbyte[3] { 1, 3, 5 }, new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), 182, 254, -1, 101, new int[0], new int[0], new int[0], new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>(), new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(44, 102, EMapPickupsType.Item, EMapPickupsType2.Material, "map_eventicon_5", 103, 0, new byte[15]
		{
			0, 0, 0, 0, 0, 0, 0, 0, 0, 1,
			1, 1, 0, 1, 1
		}, new List<short>
		{
			94, 95, 96, 97, 98, 99, 100, 101, 102, 103,
			104, 105, 106, 107, 108
		}, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[6] { 1, 2, 3, 4, 5, 6 }, new PresetItemTemplateId("Material", 236), new sbyte[6] { 1, 2, 3, 4, 5, 6 }, new short[6], new bool[12]
		{
			false, false, false, false, true, true, true, true, true, true,
			false, false
		}, -1, new OrganizationApproving(), 183, -1, 247, 104, new int[0], new int[0], new int[0], new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>(), new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(45, 105, EMapPickupsType.Item, EMapPickupsType2.Material, "map_eventicon_5", 103, 0, new byte[15]
		{
			0, 0, 1, 0, 0, 0, 0, 1, 0, 1,
			0, 1, 1, 0, 0
		}, new List<short>
		{
			94, 95, 96, 97, 98, 99, 100, 101, 102, 103,
			104, 105, 106, 107, 108
		}, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[6] { 1, 2, 3, 4, 5, 6 }, new PresetItemTemplateId("Material", 243), new sbyte[6] { 1, 2, 3, 4, 5, 6 }, new short[6], new bool[12]
		{
			true, true, true, true, false, false, false, false, false, false,
			true, true
		}, -1, new OrganizationApproving(), 183, -1, 247, 106, new int[0], new int[0], new int[0], new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>(), new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(46, 107, EMapPickupsType.Item, EMapPickupsType2.Material, "map_eventicon_5", 103, 0, new byte[15]
		{
			0, 0, 0, 0, 0, 0, 0, 0, 0, 1,
			1, 1, 0, 1, 1
		}, new List<short>
		{
			94, 95, 96, 97, 98, 99, 100, 101, 102, 103,
			104, 105, 106, 107, 108
		}, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[6] { 1, 2, 3, 4, 5, 6 }, new PresetItemTemplateId("Material", 250), new sbyte[6] { 1, 2, 3, 4, 5, 6 }, new short[6], new bool[12]
		{
			true, true, true, true, false, false, false, false, false, false,
			true, true
		}, -1, new OrganizationApproving(), 183, -1, 247, 108, new int[0], new int[0], new int[0], new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>(), new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(47, 109, EMapPickupsType.Item, EMapPickupsType2.Material, "map_eventicon_5", 103, 0, new byte[15]
		{
			0, 0, 1, 0, 0, 0, 0, 1, 0, 1,
			0, 1, 1, 0, 0
		}, new List<short>
		{
			94, 95, 96, 97, 98, 99, 100, 101, 102, 103,
			104, 105, 106, 107, 108
		}, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[6] { 1, 2, 3, 4, 5, 6 }, new PresetItemTemplateId("Material", 257), new sbyte[6] { 1, 2, 3, 4, 5, 6 }, new short[6], new bool[12]
		{
			false, false, false, false, true, true, true, true, true, true,
			false, false
		}, -1, new OrganizationApproving(), 183, -1, 247, 110, new int[0], new int[0], new int[0], new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>(), new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(48, 111, EMapPickupsType.Item, EMapPickupsType2.Material, "map_eventicon_5", 103, 0, new byte[15]
		{
			0, 0, 0, 0, 0, 0, 0, 0, 0, 1,
			1, 1, 0, 1, 1
		}, new List<short>
		{
			94, 95, 96, 97, 98, 99, 100, 101, 102, 103,
			104, 105, 106, 107, 108
		}, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[6] { 1, 2, 3, 4, 5, 6 }, new PresetItemTemplateId("Material", 264), new sbyte[6] { 1, 2, 3, 4, 5, 6 }, new short[6], new bool[12]
		{
			false, true, true, true, true, true, true, false, false, false,
			false, false
		}, -1, new OrganizationApproving(), 183, -1, 247, 112, new int[0], new int[0], new int[0], new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>(), new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(49, 113, EMapPickupsType.Item, EMapPickupsType2.Material, "map_eventicon_5", 103, 0, new byte[15]
		{
			0, 0, 1, 0, 0, 0, 0, 1, 0, 1,
			0, 1, 1, 0, 0
		}, new List<short>
		{
			94, 95, 96, 97, 98, 99, 100, 101, 102, 103,
			104, 105, 106, 107, 108
		}, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[6] { 1, 2, 3, 4, 5, 6 }, new PresetItemTemplateId("Material", 271), new sbyte[6] { 1, 2, 3, 4, 5, 6 }, new short[6], new bool[12]
		{
			true, false, false, false, false, false, false, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), 183, -1, 247, 114, new int[0], new int[0], new int[0], new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>(), new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(50, 115, EMapPickupsType.Item, EMapPickupsType2.Medicine, "map_eventicon_8", 116, 1, new byte[15]
		{
			0, 0, 0, 0, 0, 0, 0, 0, 0, 1,
			1, 1, 0, 1, 1
		}, new List<short>
		{
			94, 95, 96, 97, 98, 99, 100, 101, 102, 103,
			104, 105, 106, 107, 108
		}, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new PresetItemTemplateId("Medicine", 0), new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), 184, -1, 247, 117, new int[0], new int[0], new int[0], new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>(), new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(51, 118, EMapPickupsType.Item, EMapPickupsType2.Medicine, "map_eventicon_8", 116, 1, new byte[15]
		{
			0, 0, 1, 0, 0, 0, 0, 1, 0, 1,
			0, 1, 1, 0, 0
		}, new List<short>
		{
			94, 95, 96, 97, 98, 99, 100, 101, 102, 103,
			104, 105, 106, 107, 108
		}, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new PresetItemTemplateId("Medicine", 9), new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), 184, -1, 247, 119, new int[0], new int[0], new int[0], new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>(), new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(52, 120, EMapPickupsType.Item, EMapPickupsType2.Medicine, "map_eventicon_8", 116, 1, new byte[15]
		{
			0, 0, 0, 0, 0, 0, 0, 0, 0, 1,
			1, 1, 0, 1, 1
		}, new List<short>
		{
			94, 95, 96, 97, 98, 99, 100, 101, 102, 103,
			104, 105, 106, 107, 108
		}, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new PresetItemTemplateId("Medicine", 27), new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), 184, -1, 247, 121, new int[0], new int[0], new int[0], new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>(), new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(53, 122, EMapPickupsType.Item, EMapPickupsType2.Medicine, "map_eventicon_8", 116, 1, new byte[15]
		{
			0, 0, 1, 0, 0, 0, 0, 1, 0, 1,
			0, 1, 1, 0, 0
		}, new List<short>
		{
			94, 95, 96, 97, 98, 99, 100, 101, 102, 103,
			104, 105, 106, 107, 108
		}, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new PresetItemTemplateId("Medicine", 18), new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), 184, -1, 247, 123, new int[0], new int[0], new int[0], new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>(), new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(54, 124, EMapPickupsType.Item, EMapPickupsType2.Medicine, "map_eventicon_8", 116, 1, new byte[15]
		{
			0, 0, 0, 0, 0, 0, 0, 0, 0, 1,
			1, 1, 0, 1, 1
		}, new List<short>
		{
			94, 95, 96, 97, 98, 99, 100, 101, 102, 103,
			104, 105, 106, 107, 108
		}, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new PresetItemTemplateId("Medicine", 36), new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), 184, -1, 247, 125, new int[0], new int[0], new int[0], new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>(), new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(55, 126, EMapPickupsType.Item, EMapPickupsType2.Medicine, "map_eventicon_8", 116, 1, new byte[15]
		{
			0, 0, 1, 0, 0, 0, 0, 1, 0, 1,
			0, 1, 1, 0, 0
		}, new List<short>
		{
			94, 95, 96, 97, 98, 99, 100, 101, 102, 103,
			104, 105, 106, 107, 108
		}, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new PresetItemTemplateId("Medicine", 45), new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), 184, -1, 247, 127, new int[0], new int[0], new int[0], new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>(), new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(56, 128, EMapPickupsType.Item, EMapPickupsType2.Medicine, "map_eventicon_8", 129, 1, new byte[15]
		{
			1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
			1, 1, 1, 1, 1
		}, new List<short>
		{
			51, 52, 53, 1, 2, 3, 4, 5, 6, 7,
			8, 9, 10, 11, 12, 13, 14, 15, 34, 35,
			36
		}, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[6] { 0, 1, 2, 3, 4, 5 }, new PresetItemTemplateId("Medicine", 54), new sbyte[6] { 0, 1, 2, 3, 4, 5 }, new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), 185, -1, 247, 130, new int[0], new int[0], new int[0], new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>(), new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(57, 131, EMapPickupsType.Item, EMapPickupsType2.Medicine, "map_eventicon_8", 129, 0, new byte[15]
		{
			0, 0, 1, 0, 0, 1, 0, 0, 0, 0,
			0, 0, 0, 0, 0
		}, new List<short>
		{
			51, 52, 53, 1, 2, 3, 4, 5, 6, 7,
			8, 9, 10, 11, 12, 13, 14, 15, 34, 35,
			36
		}, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[5] { 3, 4, 5, 6, 7 }, new PresetItemTemplateId("Medicine", 60), new sbyte[5] { 3, 4, 5, 6, 7 }, new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), 185, -1, 247, 132, new int[0], new int[0], new int[0], new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>(), new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(58, 133, EMapPickupsType.Item, EMapPickupsType2.Medicine, "map_eventicon_8", 129, 1, new byte[15]
		{
			1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
			1, 1, 1, 1, 1
		}, new List<short>
		{
			51, 52, 53, 1, 2, 3, 4, 5, 6, 7,
			8, 9, 10, 11, 12, 13, 14, 15, 34, 35,
			36
		}, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[6] { 0, 1, 2, 3, 4, 5 }, new PresetItemTemplateId("Medicine", 66), new sbyte[6] { 0, 1, 2, 3, 4, 5 }, new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), 185, -1, 247, 134, new int[0], new int[0], new int[0], new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>(), new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(59, 135, EMapPickupsType.Item, EMapPickupsType2.Medicine, "map_eventicon_8", 129, 0, new byte[15]
		{
			0, 0, 1, 0, 0, 0, 1, 0, 0, 0,
			0, 0, 0, 0, 0
		}, new List<short>
		{
			51, 52, 53, 1, 2, 3, 4, 5, 6, 7,
			8, 9, 10, 11, 12, 13, 14, 15, 34, 35,
			36
		}, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[5] { 3, 4, 5, 6, 7 }, new PresetItemTemplateId("Medicine", 72), new sbyte[5] { 3, 4, 5, 6, 7 }, new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), 185, -1, 247, 136, new int[0], new int[0], new int[0], new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>(), new List<PresetItemWithCount>()));
	}

	private void CreateItems1()
	{
		_dataArray.Add(new MapPickupsItem(60, 137, EMapPickupsType.Item, EMapPickupsType2.Medicine, "map_eventicon_8", 129, 1, new byte[15]
		{
			1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
			1, 1, 1, 1, 1
		}, new List<short>
		{
			51, 52, 53, 1, 2, 3, 4, 5, 6, 7,
			8, 9, 10, 11, 12, 13, 14, 15, 34, 35,
			36
		}, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[6] { 0, 1, 2, 3, 4, 5 }, new PresetItemTemplateId("Medicine", 82), new sbyte[6] { 0, 1, 2, 3, 4, 5 }, new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), 185, -1, 247, 138, new int[0], new int[0], new int[0], new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>(), new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(61, 139, EMapPickupsType.Item, EMapPickupsType2.Medicine, "map_eventicon_8", 129, 0, new byte[15]
		{
			0, 0, 0, 1, 0, 0, 0, 0, 0, 1,
			0, 0, 0, 0, 0
		}, new List<short>
		{
			51, 52, 53, 1, 2, 3, 4, 5, 6, 7,
			8, 9, 10, 11, 12, 13, 14, 15, 34, 35,
			36
		}, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[5] { 3, 4, 5, 6, 7 }, new PresetItemTemplateId("Medicine", 88), new sbyte[5] { 3, 4, 5, 6, 7 }, new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), 185, -1, 247, 140, new int[0], new int[0], new int[0], new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>(), new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(62, 141, EMapPickupsType.Item, EMapPickupsType2.Medicine, "map_eventicon_8", 129, 1, new byte[15]
		{
			1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
			1, 1, 1, 1, 1
		}, new List<short>
		{
			51, 52, 53, 1, 2, 3, 4, 5, 6, 7,
			8, 9, 10, 11, 12, 13, 14, 15, 34, 35,
			36
		}, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[6] { 0, 1, 2, 3, 4, 5 }, new PresetItemTemplateId("Medicine", 94), new sbyte[6] { 0, 1, 2, 3, 4, 5 }, new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), 185, -1, 247, 142, new int[0], new int[0], new int[0], new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>(), new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(63, 143, EMapPickupsType.Item, EMapPickupsType2.Medicine, "map_eventicon_8", 129, 0, new byte[15]
		{
			1, 0, 0, 0, 0, 0, 0, 0, 0, 1,
			0, 0, 0, 0, 0
		}, new List<short>
		{
			51, 52, 53, 1, 2, 3, 4, 5, 6, 7,
			8, 9, 10, 11, 12, 13, 14, 15, 34, 35,
			36
		}, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[5] { 3, 4, 5, 6, 7 }, new PresetItemTemplateId("Medicine", 100), new sbyte[5] { 3, 4, 5, 6, 7 }, new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), 185, -1, 247, 144, new int[0], new int[0], new int[0], new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>(), new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(64, 145, EMapPickupsType.Item, EMapPickupsType2.Medicine, "map_eventicon_8", 129, 0, new byte[15]
		{
			1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
			1, 1, 1, 1, 1
		}, new List<short>
		{
			51, 52, 53, 1, 2, 3, 4, 5, 6, 7,
			8, 9, 10, 11, 12, 13, 14, 15, 34, 35,
			36
		}, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[6] { 0, 1, 2, 3, 4, 5 }, new PresetItemTemplateId("Medicine", 130), new sbyte[6] { 0, 1, 2, 3, 4, 5 }, new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), 186, -1, 247, 146, new int[0], new int[0], new int[0], new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>(), new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(65, 147, EMapPickupsType.Item, EMapPickupsType2.Medicine, "map_eventicon_8", 129, 0, new byte[15]
		{
			0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
			1, 0, 0, 0, 0
		}, new List<short>
		{
			51, 52, 53, 1, 2, 3, 4, 5, 6, 7,
			8, 9, 10, 11, 12, 13, 14, 15, 34, 35,
			36
		}, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[5] { 3, 4, 5, 6, 7 }, new PresetItemTemplateId("Medicine", 136), new sbyte[5] { 3, 4, 5, 6, 7 }, new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), 186, -1, 247, 148, new int[0], new int[0], new int[0], new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>(), new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(66, 149, EMapPickupsType.Item, EMapPickupsType2.Medicine, "map_eventicon_8", 129, 0, new byte[15]
		{
			1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
			1, 1, 1, 1, 1
		}, new List<short>
		{
			51, 52, 53, 1, 2, 3, 4, 5, 6, 7,
			8, 9, 10, 11, 12, 13, 14, 15, 34, 35,
			36
		}, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[6] { 0, 1, 2, 3, 4, 5 }, new PresetItemTemplateId("Medicine", 142), new sbyte[6] { 0, 1, 2, 3, 4, 5 }, new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), 186, -1, 247, 150, new int[0], new int[0], new int[0], new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>(), new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(67, 151, EMapPickupsType.Item, EMapPickupsType2.Medicine, "map_eventicon_8", 129, 0, new byte[15]
		{
			0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
			0, 1, 0, 0, 0
		}, new List<short>
		{
			51, 52, 53, 1, 2, 3, 4, 5, 6, 7,
			8, 9, 10, 11, 12, 13, 14, 15, 34, 35,
			36
		}, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[5] { 3, 4, 5, 6, 7 }, new PresetItemTemplateId("Medicine", 148), new sbyte[5] { 3, 4, 5, 6, 7 }, new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), 186, -1, 247, 152, new int[0], new int[0], new int[0], new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>(), new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(68, 153, EMapPickupsType.Item, EMapPickupsType2.Medicine, "map_eventicon_8", 129, 0, new byte[15]
		{
			1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
			1, 1, 1, 1, 1
		}, new List<short>
		{
			51, 52, 53, 1, 2, 3, 4, 5, 6, 7,
			8, 9, 10, 11, 12, 13, 14, 15, 34, 35,
			36
		}, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[6] { 0, 1, 2, 3, 4, 5 }, new PresetItemTemplateId("Medicine", 166), new sbyte[6] { 0, 1, 2, 3, 4, 5 }, new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), 186, -1, 247, 154, new int[0], new int[0], new int[0], new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>(), new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(69, 155, EMapPickupsType.Item, EMapPickupsType2.Medicine, "map_eventicon_8", 129, 0, new byte[15]
		{
			0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
			0, 0, 0, 0, 1
		}, new List<short>
		{
			51, 52, 53, 1, 2, 3, 4, 5, 6, 7,
			8, 9, 10, 11, 12, 13, 14, 15, 34, 35,
			36
		}, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[5] { 3, 4, 5, 6, 7 }, new PresetItemTemplateId("Medicine", 172), new sbyte[5] { 3, 4, 5, 6, 7 }, new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), 186, -1, 247, 156, new int[0], new int[0], new int[0], new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>(), new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(70, 157, EMapPickupsType.Item, EMapPickupsType2.Medicine, "map_eventicon_8", 129, 0, new byte[15]
		{
			1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
			1, 1, 1, 1, 1
		}, new List<short>
		{
			51, 52, 53, 1, 2, 3, 4, 5, 6, 7,
			8, 9, 10, 11, 12, 13, 14, 15, 34, 35,
			36
		}, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[6] { 0, 1, 2, 3, 4, 5 }, new PresetItemTemplateId("Medicine", 154), new sbyte[6] { 0, 1, 2, 3, 4, 5 }, new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), 186, -1, 247, 158, new int[0], new int[0], new int[0], new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>(), new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(71, 159, EMapPickupsType.Item, EMapPickupsType2.Medicine, "map_eventicon_8", 129, 0, new byte[15]
		{
			0, 0, 0, 0, 0, 0, 0, 1, 0, 0,
			0, 0, 0, 0, 0
		}, new List<short>
		{
			51, 52, 53, 1, 2, 3, 4, 5, 6, 7,
			8, 9, 10, 11, 12, 13, 14, 15, 34, 35,
			36
		}, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[5] { 3, 4, 5, 6, 7 }, new PresetItemTemplateId("Medicine", 160), new sbyte[5] { 3, 4, 5, 6, 7 }, new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), 186, -1, 247, 160, new int[0], new int[0], new int[0], new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>(), new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(72, 161, EMapPickupsType.Item, EMapPickupsType2.Medicine, "map_eventicon_8", 129, 0, new byte[15]
		{
			1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
			1, 1, 1, 1, 1
		}, new List<short>
		{
			51, 52, 53, 1, 2, 3, 4, 5, 6, 7,
			8, 9, 10, 11, 12, 13, 14, 15, 34, 35,
			36
		}, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[6] { 0, 1, 2, 3, 4, 5 }, new PresetItemTemplateId("Medicine", 178), new sbyte[6] { 0, 1, 2, 3, 4, 5 }, new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), 186, -1, 247, 162, new int[0], new int[0], new int[0], new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>(), new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(73, 163, EMapPickupsType.Item, EMapPickupsType2.Medicine, "map_eventicon_8", 129, 0, new byte[15]
		{
			0, 0, 0, 0, 0, 0, 0, 0, 0, 1,
			0, 0, 0, 0, 0
		}, new List<short>
		{
			51, 52, 53, 1, 2, 3, 4, 5, 6, 7,
			8, 9, 10, 11, 12, 13, 14, 15, 34, 35,
			36
		}, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[5] { 3, 4, 5, 6, 7 }, new PresetItemTemplateId("Medicine", 184), new sbyte[5] { 3, 4, 5, 6, 7 }, new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), 186, -1, 247, 164, new int[0], new int[0], new int[0], new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>(), new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(74, 165, EMapPickupsType.Item, EMapPickupsType2.Medicine, "map_eventicon_8", 129, 0, new byte[15]
		{
			1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
			1, 1, 1, 1, 1
		}, new List<short>
		{
			51, 52, 53, 1, 2, 3, 4, 5, 6, 7,
			8, 9, 10, 11, 12, 13, 14, 15, 34, 35,
			36
		}, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[6] { 0, 1, 2, 3, 4, 5 }, new PresetItemTemplateId("Medicine", 190), new sbyte[6] { 0, 1, 2, 3, 4, 5 }, new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), 186, -1, 247, 166, new int[0], new int[0], new int[0], new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>(), new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(75, 167, EMapPickupsType.Item, EMapPickupsType2.Medicine, "map_eventicon_8", 129, 0, new byte[15]
		{
			0, 0, 1, 0, 0, 0, 0, 0, 0, 0,
			0, 0, 0, 0, 0
		}, new List<short>
		{
			51, 52, 53, 1, 2, 3, 4, 5, 6, 7,
			8, 9, 10, 11, 12, 13, 14, 15, 34, 35,
			36
		}, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[5] { 3, 4, 5, 6, 7 }, new PresetItemTemplateId("Medicine", 196), new sbyte[5] { 3, 4, 5, 6, 7 }, new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), 186, -1, 247, 168, new int[0], new int[0], new int[0], new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>(), new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(76, 169, EMapPickupsType.Item, EMapPickupsType2.Medicine, "map_eventicon_8", 129, 0, new byte[15]
		{
			1, 0, 0, 0, 1, 1, 0, 0, 0, 0,
			1, 0, 0, 1, 0
		}, new List<short>
		{
			51, 52, 53, 1, 2, 3, 4, 5, 6, 7,
			8, 9, 10, 11, 12, 13, 14, 15, 34, 35,
			36
		}, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[6] { 0, 1, 2, 3, 4, 5 }, new PresetItemTemplateId("Medicine", 274), new sbyte[6] { 0, 1, 2, 3, 4, 5 }, new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), 187, -1, 247, 170, new int[0], new int[0], new int[0], new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>(), new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(77, 171, EMapPickupsType.Item, EMapPickupsType2.Medicine, "map_eventicon_8", 129, 0, new byte[15]
		{
			0, 0, 0, 0, 0, 1, 0, 0, 0, 0,
			0, 0, 0, 0, 0
		}, new List<short>
		{
			51, 52, 53, 1, 2, 3, 4, 5, 6, 7,
			8, 9, 10, 11, 12, 13, 14, 15, 34, 35,
			36
		}, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[5] { 3, 4, 5, 6, 7 }, new PresetItemTemplateId("Medicine", 280), new sbyte[5] { 3, 4, 5, 6, 7 }, new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), 187, -1, 247, 172, new int[0], new int[0], new int[0], new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>(), new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(78, 173, EMapPickupsType.Item, EMapPickupsType2.Medicine, "map_eventicon_8", 129, 0, new byte[15]
		{
			0, 1, 1, 1, 0, 0, 0, 1, 0, 1,
			0, 0, 0, 0, 0
		}, new List<short>
		{
			51, 52, 53, 1, 2, 3, 4, 5, 6, 7,
			8, 9, 10, 11, 12, 13, 14, 15, 34, 35,
			36
		}, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[6] { 0, 1, 2, 3, 4, 5 }, new PresetItemTemplateId("Medicine", 298), new sbyte[6] { 0, 1, 2, 3, 4, 5 }, new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), 187, -1, 247, 174, new int[0], new int[0], new int[0], new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>(), new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(79, 175, EMapPickupsType.Item, EMapPickupsType2.Medicine, "map_eventicon_8", 129, 0, new byte[15]
		{
			0, 0, 0, 0, 0, 0, 0, 0, 0, 1,
			0, 0, 0, 0, 0
		}, new List<short>
		{
			51, 52, 53, 1, 2, 3, 4, 5, 6, 7,
			8, 9, 10, 11, 12, 13, 14, 15, 34, 35,
			36
		}, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[5] { 3, 4, 5, 6, 7 }, new PresetItemTemplateId("Medicine", 304), new sbyte[5] { 3, 4, 5, 6, 7 }, new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), 187, -1, 247, 176, new int[0], new int[0], new int[0], new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>(), new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(80, 177, EMapPickupsType.Item, EMapPickupsType2.Medicine, "map_eventicon_8", 129, 0, new byte[15]
		{
			0, 1, 1, 0, 0, 0, 0, 1, 0, 0,
			0, 0, 1, 0, 1
		}, new List<short>
		{
			51, 52, 53, 1, 2, 3, 4, 5, 6, 7,
			8, 9, 10, 11, 12, 13, 14, 15, 34, 35,
			36
		}, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[6] { 0, 1, 2, 3, 4, 5 }, new PresetItemTemplateId("Medicine", 322), new sbyte[6] { 0, 1, 2, 3, 4, 5 }, new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), 187, -1, 247, 178, new int[0], new int[0], new int[0], new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>(), new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(81, 179, EMapPickupsType.Item, EMapPickupsType2.Medicine, "map_eventicon_8", 129, 0, new byte[15]
		{
			0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
			0, 0, 1, 0, 0
		}, new List<short>
		{
			51, 52, 53, 1, 2, 3, 4, 5, 6, 7,
			8, 9, 10, 11, 12, 13, 14, 15, 34, 35,
			36
		}, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[5] { 3, 4, 5, 6, 7 }, new PresetItemTemplateId("Medicine", 328), new sbyte[5] { 3, 4, 5, 6, 7 }, new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), 187, -1, 247, 180, new int[0], new int[0], new int[0], new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>(), new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(82, 181, EMapPickupsType.Item, EMapPickupsType2.Medicine, "map_eventicon_8", 129, 0, new byte[15]
		{
			0, 0, 0, 0, 0, 1, 0, 0, 1, 0,
			1, 0, 0, 1, 1
		}, new List<short>
		{
			51, 52, 53, 1, 2, 3, 4, 5, 6, 7,
			8, 9, 10, 11, 12, 13, 14, 15, 34, 35,
			36
		}, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[6] { 0, 1, 2, 3, 4, 5 }, new PresetItemTemplateId("Medicine", 334), new sbyte[6] { 0, 1, 2, 3, 4, 5 }, new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), 187, -1, 247, 182, new int[0], new int[0], new int[0], new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>(), new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(83, 183, EMapPickupsType.Item, EMapPickupsType2.Medicine, "map_eventicon_8", 129, 0, new byte[15]
		{
			0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
			0, 0, 0, 1, 0
		}, new List<short>
		{
			51, 52, 53, 1, 2, 3, 4, 5, 6, 7,
			8, 9, 10, 11, 12, 13, 14, 15, 34, 35,
			36
		}, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[5] { 3, 4, 5, 6, 7 }, new PresetItemTemplateId("Medicine", 340), new sbyte[5] { 3, 4, 5, 6, 7 }, new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), 187, -1, 247, 184, new int[0], new int[0], new int[0], new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>(), new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(84, 185, EMapPickupsType.Item, EMapPickupsType2.Medicine, "map_eventicon_8", 129, 0, new byte[15]
		{
			0, 0, 1, 1, 0, 0, 1, 1, 0, 0,
			0, 0, 0, 0, 1
		}, new List<short>
		{
			51, 52, 53, 1, 2, 3, 4, 5, 6, 7,
			8, 9, 10, 11, 12, 13, 14, 15, 34, 35,
			36
		}, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[6] { 0, 1, 2, 3, 4, 5 }, new PresetItemTemplateId("Medicine", 118), new sbyte[6] { 0, 1, 2, 3, 4, 5 }, new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), 187, -1, 247, 186, new int[0], new int[0], new int[0], new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>(), new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(85, 187, EMapPickupsType.Item, EMapPickupsType2.Medicine, "map_eventicon_8", 129, 0, new byte[15]
		{
			0, 0, 0, 0, 0, 0, 1, 0, 0, 0,
			0, 0, 0, 0, 0
		}, new List<short>
		{
			51, 52, 53, 1, 2, 3, 4, 5, 6, 7,
			8, 9, 10, 11, 12, 13, 14, 15, 34, 35,
			36
		}, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[5] { 3, 4, 5, 6, 7 }, new PresetItemTemplateId("Medicine", 124), new sbyte[5] { 3, 4, 5, 6, 7 }, new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), 187, -1, 247, 188, new int[0], new int[0], new int[0], new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>(), new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(86, 189, EMapPickupsType.Item, EMapPickupsType2.Medicine, "map_eventicon_8", 129, 0, new byte[15]
		{
			1, 0, 0, 1, 0, 1, 0, 0, 0, 0,
			1, 0, 0, 1, 0
		}, new List<short>
		{
			51, 52, 53, 1, 2, 3, 4, 5, 6, 7,
			8, 9, 10, 11, 12, 13, 14, 15, 34, 35,
			36
		}, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[6] { 0, 1, 2, 3, 4, 5 }, new PresetItemTemplateId("Medicine", 226), new sbyte[6] { 0, 1, 2, 3, 4, 5 }, new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), 187, -1, 247, 190, new int[0], new int[0], new int[0], new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>(), new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(87, 191, EMapPickupsType.Item, EMapPickupsType2.Medicine, "map_eventicon_8", 129, 0, new byte[15]
		{
			1, 0, 0, 0, 0, 0, 0, 0, 0, 0,
			0, 0, 0, 0, 0
		}, new List<short>
		{
			51, 52, 53, 1, 2, 3, 4, 5, 6, 7,
			8, 9, 10, 11, 12, 13, 14, 15, 34, 35,
			36
		}, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[5] { 3, 4, 5, 6, 7 }, new PresetItemTemplateId("Medicine", 232), new sbyte[5] { 3, 4, 5, 6, 7 }, new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), 187, -1, 247, 192, new int[0], new int[0], new int[0], new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>(), new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(88, 193, EMapPickupsType.Item, EMapPickupsType2.Medicine, "map_eventicon_8", 129, 0, new byte[15]
		{
			0, 1, 0, 1, 0, 0, 1, 1, 0, 1,
			0, 0, 0, 0, 0
		}, new List<short>
		{
			51, 52, 53, 1, 2, 3, 4, 5, 6, 7,
			8, 9, 10, 11, 12, 13, 14, 15, 34, 35,
			36
		}, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[6] { 0, 1, 2, 3, 4, 5 }, new PresetItemTemplateId("Medicine", 238), new sbyte[6] { 0, 1, 2, 3, 4, 5 }, new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), 187, -1, 247, 194, new int[0], new int[0], new int[0], new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>(), new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(89, 195, EMapPickupsType.Item, EMapPickupsType2.Medicine, "map_eventicon_8", 129, 0, new byte[15]
		{
			0, 0, 0, 1, 0, 0, 0, 0, 0, 0,
			0, 0, 0, 0, 0
		}, new List<short>
		{
			51, 52, 53, 1, 2, 3, 4, 5, 6, 7,
			8, 9, 10, 11, 12, 13, 14, 15, 34, 35,
			36
		}, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[5] { 3, 4, 5, 6, 7 }, new PresetItemTemplateId("Medicine", 244), new sbyte[5] { 3, 4, 5, 6, 7 }, new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), 187, -1, 247, 196, new int[0], new int[0], new int[0], new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>(), new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(90, 197, EMapPickupsType.Item, EMapPickupsType2.Medicine, "map_eventicon_8", 129, 0, new byte[15]
		{
			0, 1, 1, 0, 0, 0, 0, 1, 0, 0,
			0, 1, 1, 0, 0
		}, new List<short>
		{
			51, 52, 53, 1, 2, 3, 4, 5, 6, 7,
			8, 9, 10, 11, 12, 13, 14, 15, 34, 35,
			36
		}, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[6] { 0, 1, 2, 3, 4, 5 }, new PresetItemTemplateId("Medicine", 250), new sbyte[6] { 0, 1, 2, 3, 4, 5 }, new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), 187, -1, 247, 198, new int[0], new int[0], new int[0], new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>(), new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(91, 199, EMapPickupsType.Item, EMapPickupsType2.Medicine, "map_eventicon_8", 129, 0, new byte[15]
		{
			0, 1, 0, 0, 0, 0, 0, 0, 0, 0,
			0, 0, 0, 0, 0
		}, new List<short>
		{
			51, 52, 53, 1, 2, 3, 4, 5, 6, 7,
			8, 9, 10, 11, 12, 13, 14, 15, 34, 35,
			36
		}, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[5] { 3, 4, 5, 6, 7 }, new PresetItemTemplateId("Medicine", 256), new sbyte[5] { 3, 4, 5, 6, 7 }, new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), 187, -1, 247, 200, new int[0], new int[0], new int[0], new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>(), new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(92, 201, EMapPickupsType.Item, EMapPickupsType2.Medicine, "map_eventicon_8", 129, 0, new byte[15]
		{
			1, 0, 0, 0, 0, 1, 0, 0, 1, 0,
			1, 0, 0, 1, 0
		}, new List<short>
		{
			51, 52, 53, 1, 2, 3, 4, 5, 6, 7,
			8, 9, 10, 11, 12, 13, 14, 15, 34, 35,
			36
		}, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[6] { 0, 1, 2, 3, 4, 5 }, new PresetItemTemplateId("Medicine", 202), new sbyte[6] { 0, 1, 2, 3, 4, 5 }, new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), 187, -1, 247, 202, new int[0], new int[0], new int[0], new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>(), new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(93, 203, EMapPickupsType.Item, EMapPickupsType2.Medicine, "map_eventicon_8", 129, 0, new byte[15]
		{
			0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
			1, 0, 0, 0, 0
		}, new List<short>
		{
			51, 52, 53, 1, 2, 3, 4, 5, 6, 7,
			8, 9, 10, 11, 12, 13, 14, 15, 34, 35,
			36
		}, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[5] { 3, 4, 5, 6, 7 }, new PresetItemTemplateId("Medicine", 208), new sbyte[5] { 3, 4, 5, 6, 7 }, new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), 187, -1, 247, 204, new int[0], new int[0], new int[0], new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>(), new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(94, 205, EMapPickupsType.Item, EMapPickupsType2.Medicine, "map_eventicon_8", 129, 0, new byte[15]
		{
			0, 0, 1, 1, 0, 0, 1, 1, 1, 0,
			0, 0, 0, 0, 0
		}, new List<short>
		{
			51, 52, 53, 1, 2, 3, 4, 5, 6, 7,
			8, 9, 10, 11, 12, 13, 14, 15, 34, 35,
			36
		}, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[6] { 0, 1, 2, 3, 4, 5 }, new PresetItemTemplateId("Medicine", 214), new sbyte[6] { 0, 1, 2, 3, 4, 5 }, new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), 187, -1, 247, 206, new int[0], new int[0], new int[0], new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>(), new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(95, 207, EMapPickupsType.Item, EMapPickupsType2.Medicine, "map_eventicon_8", 129, 0, new byte[15]
		{
			0, 0, 1, 0, 0, 0, 0, 0, 0, 0,
			0, 0, 0, 0, 0
		}, new List<short>
		{
			51, 52, 53, 1, 2, 3, 4, 5, 6, 7,
			8, 9, 10, 11, 12, 13, 14, 15, 34, 35,
			36
		}, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[5] { 3, 4, 5, 6, 7 }, new PresetItemTemplateId("Medicine", 220), new sbyte[5] { 3, 4, 5, 6, 7 }, new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), 187, -1, 247, 208, new int[0], new int[0], new int[0], new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>(), new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(96, 209, EMapPickupsType.Item, EMapPickupsType2.Medicine, "map_eventicon_8", 129, 0, new byte[15]
		{
			1, 0, 0, 1, 0, 0, 0, 0, 1, 0,
			0, 0, 0, 1, 1
		}, new List<short>
		{
			51, 52, 53, 1, 2, 3, 4, 5, 6, 7,
			8, 9, 10, 11, 12, 13, 14, 15, 34, 35,
			36
		}, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[6] { 0, 1, 2, 3, 4, 5 }, new PresetItemTemplateId("Medicine", 106), new sbyte[6] { 0, 1, 2, 3, 4, 5 }, new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), 187, -1, 247, 210, new int[0], new int[0], new int[0], new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>(), new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(97, 211, EMapPickupsType.Item, EMapPickupsType2.Medicine, "map_eventicon_8", 129, 0, new byte[15]
		{
			0, 0, 0, 0, 0, 0, 0, 0, 1, 0,
			0, 0, 0, 0, 0
		}, new List<short>
		{
			51, 52, 53, 1, 2, 3, 4, 5, 6, 7,
			8, 9, 10, 11, 12, 13, 14, 15, 34, 35,
			36
		}, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[5] { 3, 4, 5, 6, 7 }, new PresetItemTemplateId("Medicine", 112), new sbyte[5] { 3, 4, 5, 6, 7 }, new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), 187, -1, 247, 212, new int[0], new int[0], new int[0], new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>(), new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(98, 213, EMapPickupsType.Item, EMapPickupsType2.Medicine, "map_eventicon_8", 129, 0, new byte[15]
		{
			0, 1, 1, 0, 0, 0, 1, 1, 0, 0,
			0, 0, 1, 0, 0
		}, new List<short>
		{
			51, 52, 53, 1, 2, 3, 4, 5, 6, 7,
			8, 9, 10, 11, 12, 13, 14, 15, 34, 35,
			36
		}, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[6] { 0, 1, 2, 3, 4, 5 }, new PresetItemTemplateId("Medicine", 310), new sbyte[6] { 0, 1, 2, 3, 4, 5 }, new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), 187, -1, 247, 214, new int[0], new int[0], new int[0], new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>(), new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(99, 215, EMapPickupsType.Item, EMapPickupsType2.Medicine, "map_eventicon_8", 129, 0, new byte[15]
		{
			0, 0, 0, 0, 0, 0, 0, 1, 0, 0,
			0, 0, 0, 0, 0
		}, new List<short>
		{
			51, 52, 53, 1, 2, 3, 4, 5, 6, 7,
			8, 9, 10, 11, 12, 13, 14, 15, 34, 35,
			36
		}, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[5] { 3, 4, 5, 6, 7 }, new PresetItemTemplateId("Medicine", 316), new sbyte[5] { 3, 4, 5, 6, 7 }, new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), 187, -1, 247, 216, new int[0], new int[0], new int[0], new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>(), new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(100, 217, EMapPickupsType.Item, EMapPickupsType2.Medicine, "map_eventicon_8", 129, 0, new byte[15]
		{
			0, 0, 1, 1, 0, 0, 1, 1, 0, 0,
			0, 0, 0, 0, 1
		}, new List<short>
		{
			51, 52, 53, 1, 2, 3, 4, 5, 6, 7,
			8, 9, 10, 11, 12, 13, 14, 15, 34, 35,
			36
		}, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[6] { 0, 1, 2, 3, 4, 5 }, new PresetItemTemplateId("Medicine", 262), new sbyte[6] { 0, 1, 2, 3, 4, 5 }, new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), 187, -1, 247, 218, new int[0], new int[0], new int[0], new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>(), new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(101, 219, EMapPickupsType.Item, EMapPickupsType2.Medicine, "map_eventicon_8", 129, 0, new byte[15]
		{
			0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
			0, 0, 0, 0, 1
		}, new List<short>
		{
			51, 52, 53, 1, 2, 3, 4, 5, 6, 7,
			8, 9, 10, 11, 12, 13, 14, 15, 34, 35,
			36
		}, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[5] { 3, 4, 5, 6, 7 }, new PresetItemTemplateId("Medicine", 268), new sbyte[5] { 3, 4, 5, 6, 7 }, new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), 187, -1, 247, 220, new int[0], new int[0], new int[0], new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>(), new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(102, 221, EMapPickupsType.Item, EMapPickupsType2.Medicine, "map_eventicon_8", 129, 0, new byte[15]
		{
			0, 1, 1, 0, 0, 0, 0, 0, 0, 0,
			0, 1, 1, 0, 1
		}, new List<short>
		{
			51, 52, 53, 1, 2, 3, 4, 5, 6, 7,
			8, 9, 10, 11, 12, 13, 14, 15, 34, 35,
			36
		}, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[6] { 0, 1, 2, 3, 4, 5 }, new PresetItemTemplateId("Medicine", 286), new sbyte[6] { 0, 1, 2, 3, 4, 5 }, new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), 187, -1, 247, 222, new int[0], new int[0], new int[0], new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>(), new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(103, 223, EMapPickupsType.Item, EMapPickupsType2.Medicine, "map_eventicon_8", 129, 0, new byte[15]
		{
			0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
			0, 1, 0, 0, 0
		}, new List<short>
		{
			51, 52, 53, 1, 2, 3, 4, 5, 6, 7,
			8, 9, 10, 11, 12, 13, 14, 15, 34, 35,
			36
		}, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[5] { 3, 4, 5, 6, 7 }, new PresetItemTemplateId("Medicine", 292), new sbyte[5] { 3, 4, 5, 6, 7 }, new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), 187, -1, 247, 224, new int[0], new int[0], new int[0], new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>(), new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(104, 225, EMapPickupsType.Item, EMapPickupsType2.Food, "map_eventicon_9", 226, 2, new byte[15]
		{
			1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
			1, 1, 1, 1, 1
		}, new List<short>
		{
			57, 58, 59, 60, 61, 62, 63, 64, 65, 66,
			67, 68, 69, 70, 71, 72, 73, 74, 75, 76,
			77, 78, 79, 80, 81, 82, 83, 84, 85, 86,
			87, 88, 89, 90, 91, 92, 93
		}, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new PresetItemTemplateId("Food", 0), new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new short[6], new bool[12]
		{
			false, true, true, true, true, true, true, true, true, true,
			false, false
		}, -1, new OrganizationApproving(), 188, -1, 247, 227, new int[0], new int[0], new int[0], new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>(), new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(105, 228, EMapPickupsType.Item, EMapPickupsType2.Food, "map_eventicon_9", 226, 1, new byte[15]
		{
			1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
			1, 1, 1, 1, 1
		}, new List<short>
		{
			39, 40, 41, 42, 43, 44, 45, 46, 47, 54,
			55, 56, 48, 49, 50, 51, 52, 53, 1, 2,
			3, 4, 5, 6, 7, 8, 9, 10, 11, 12,
			13, 14, 15, 34, 35, 36
		}, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new PresetItemTemplateId("Food", 9), new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new short[6], new bool[12]
		{
			true, false, false, false, true, true, true, false, false, false,
			true, true
		}, -1, new OrganizationApproving(), 189, -1, 247, 229, new int[0], new int[0], new int[0], new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>(), new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(106, 230, EMapPickupsType.Item, EMapPickupsType2.Food, "map_eventicon_9", 226, 0, new byte[15]
		{
			1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
			1, 1, 1, 1, 1
		}, new List<short>
		{
			39, 40, 41, 42, 43, 44, 45, 46, 47, 54,
			55, 56, 48, 49, 50, 51, 52, 53, 1, 2,
			3, 4, 5, 6, 7, 8, 9, 10, 11, 12,
			13, 14, 15, 34, 35, 36
		}, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[7] { 1, 2, 3, 4, 5, 6, 7 }, new PresetItemTemplateId("Food", 18), new sbyte[7] { 1, 2, 3, 4, 5, 6, 7 }, new short[6], new bool[12]
		{
			true, false, false, false, true, true, true, false, false, false,
			true, true
		}, -1, new OrganizationApproving(), 189, -1, 247, 231, new int[0], new int[0], new int[0], new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>(), new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(107, 232, EMapPickupsType.Item, EMapPickupsType2.Food, "map_eventicon_9", 226, 0, new byte[15]
		{
			1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
			1, 1, 1, 1, 1
		}, new List<short>
		{
			39, 40, 41, 42, 43, 44, 45, 46, 47, 54,
			55, 56, 48, 49, 50, 51, 52, 53, 1, 2,
			3, 4, 5, 6, 7, 8, 9, 10, 11, 12,
			13, 14, 15, 34, 35, 36
		}, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[6] { 2, 3, 4, 5, 6, 7 }, new PresetItemTemplateId("Food", 26), new sbyte[6] { 2, 3, 4, 5, 6, 7 }, new short[6], new bool[12]
		{
			true, false, false, false, true, true, true, false, false, false,
			true, true
		}, -1, new OrganizationApproving(), 189, -1, 247, 233, new int[0], new int[0], new int[0], new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>(), new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(108, 234, EMapPickupsType.Item, EMapPickupsType2.Food, "map_eventicon_9", 226, 0, new byte[15]
		{
			0, 1, 0, 0, 0, 1, 1, 0, 0, 0,
			0, 1, 0, 0, 0
		}, new List<short>
		{
			39, 40, 41, 42, 43, 44, 45, 46, 47, 54,
			55, 56, 48, 49, 50, 51, 52, 53, 1, 2,
			3, 4, 5, 6, 7, 8, 9, 10, 11, 12,
			13, 14, 15, 34, 35, 36
		}, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[5] { 3, 4, 5, 6, 7 }, new PresetItemTemplateId("Food", 33), new sbyte[5] { 3, 4, 5, 6, 7 }, new short[6], new bool[12]
		{
			true, false, false, false, true, true, true, false, false, false,
			true, true
		}, -1, new OrganizationApproving(), 189, -1, 247, 235, new int[0], new int[0], new int[0], new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>(), new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(109, 236, EMapPickupsType.Item, EMapPickupsType2.Food, "map_eventicon_9", 226, 0, new byte[15]
		{
			0, 1, 0, 0, 0, 1, 0, 0, 0, 0,
			0, 1, 0, 0, 0
		}, new List<short>
		{
			39, 40, 41, 42, 43, 44, 45, 46, 47, 54,
			55, 56, 48, 49, 50, 51, 52, 53, 1, 2,
			3, 4, 5, 6, 7, 8, 9, 10, 11, 12,
			13, 14, 15, 34, 35, 36
		}, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[4] { 4, 5, 6, 7 }, new PresetItemTemplateId("Food", 39), new sbyte[4] { 4, 5, 6, 7 }, new short[6], new bool[12]
		{
			true, false, false, false, true, true, true, false, false, false,
			true, true
		}, -1, new OrganizationApproving(), 189, -1, 247, 237, new int[0], new int[0], new int[0], new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>(), new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(110, 238, EMapPickupsType.Item, EMapPickupsType2.Food, "map_eventicon_9", 226, 0, new byte[15]
		{
			0, 0, 0, 0, 0, 1, 0, 0, 0, 0,
			0, 1, 0, 0, 0
		}, new List<short>
		{
			39, 40, 41, 42, 43, 44, 45, 46, 47, 54,
			55, 56, 48, 49, 50, 51, 52, 53, 1, 2,
			3, 4, 5, 6, 7, 8, 9, 10, 11, 12,
			13, 14, 15, 34, 35, 36
		}, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[3] { 5, 6, 7 }, new PresetItemTemplateId("Food", 44), new sbyte[3] { 5, 6, 7 }, new short[6], new bool[12]
		{
			true, false, false, false, true, true, true, false, false, false,
			true, true
		}, -1, new OrganizationApproving(), 189, -1, 247, 239, new int[0], new int[0], new int[0], new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>(), new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(111, 240, EMapPickupsType.Item, EMapPickupsType2.Food, "map_eventicon_9", 226, 0, new byte[15]
		{
			0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
			0, 1, 0, 0, 0
		}, new List<short>
		{
			39, 40, 41, 42, 43, 44, 45, 46, 47, 54,
			55, 56, 48, 49, 50, 51, 52, 53, 1, 2,
			3, 4, 5, 6, 7, 8, 9, 10, 11, 12,
			13, 14, 15, 34, 35, 36
		}, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[2] { 6, 7 }, new PresetItemTemplateId("Food", 48), new sbyte[2] { 6, 7 }, new short[6], new bool[12]
		{
			true, false, false, false, true, true, true, false, false, false,
			true, true
		}, -1, new OrganizationApproving(), 189, -1, 247, 241, new int[0], new int[0], new int[0], new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>(), new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(112, 242, EMapPickupsType.Item, EMapPickupsType2.Food, "map_eventicon_9", 226, 1, new byte[15]
		{
			1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
			1, 1, 1, 1, 1
		}, new List<short>
		{
			39, 40, 41, 42, 43, 44, 45, 46, 47, 54,
			55, 56, 48, 49, 50, 51, 52, 53, 1, 2,
			3, 4, 5, 6, 7, 8, 9, 10, 11, 12,
			13, 14, 15, 34, 35, 36
		}, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new PresetItemTemplateId("Food", 51), new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new short[6], new bool[12]
		{
			true, false, false, false, false, false, false, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), 190, -1, 247, 243, new int[0], new int[0], new int[0], new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>(), new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(113, 244, EMapPickupsType.Item, EMapPickupsType2.Food, "map_eventicon_9", 226, 0, new byte[15]
		{
			1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
			1, 1, 1, 1, 1
		}, new List<short>
		{
			39, 40, 41, 42, 43, 44, 45, 46, 47, 54,
			55, 56, 48, 49, 50, 51, 52, 53, 1, 2,
			3, 4, 5, 6, 7, 8, 9, 10, 11, 12,
			13, 14, 15, 34, 35, 36
		}, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[7] { 1, 2, 3, 4, 5, 6, 7 }, new PresetItemTemplateId("Food", 60), new sbyte[7] { 1, 2, 3, 4, 5, 6, 7 }, new short[6], new bool[12]
		{
			true, false, false, false, false, false, false, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), 190, -1, 247, 245, new int[0], new int[0], new int[0], new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>(), new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(114, 246, EMapPickupsType.Item, EMapPickupsType2.Food, "map_eventicon_9", 226, 0, new byte[15]
		{
			1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
			1, 1, 1, 1, 1
		}, new List<short>
		{
			39, 40, 41, 42, 43, 44, 45, 46, 47, 54,
			55, 56, 48, 49, 50, 51, 52, 53, 1, 2,
			3, 4, 5, 6, 7, 8, 9, 10, 11, 12,
			13, 14, 15, 34, 35, 36
		}, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[6] { 2, 3, 4, 5, 6, 7 }, new PresetItemTemplateId("Food", 68), new sbyte[6] { 2, 3, 4, 5, 6, 7 }, new short[6], new bool[12]
		{
			true, false, false, false, false, false, false, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), 190, -1, 247, 247, new int[0], new int[0], new int[0], new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>(), new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(115, 248, EMapPickupsType.Item, EMapPickupsType2.Food, "map_eventicon_9", 226, 0, new byte[15]
		{
			0, 1, 0, 0, 0, 1, 0, 0, 0, 1,
			0, 0, 0, 1, 0
		}, new List<short>
		{
			39, 40, 41, 42, 43, 44, 45, 46, 47, 54,
			55, 56, 48, 49, 50, 51, 52, 53, 1, 2,
			3, 4, 5, 6, 7, 8, 9, 10, 11, 12,
			13, 14, 15, 34, 35, 36
		}, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[5] { 3, 4, 5, 6, 7 }, new PresetItemTemplateId("Food", 75), new sbyte[5] { 3, 4, 5, 6, 7 }, new short[6], new bool[12]
		{
			true, false, false, false, false, false, false, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), 190, -1, 247, 249, new int[0], new int[0], new int[0], new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>(), new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(116, 250, EMapPickupsType.Item, EMapPickupsType2.Food, "map_eventicon_9", 226, 0, new byte[15]
		{
			0, 1, 0, 0, 0, 1, 0, 0, 0, 1,
			0, 0, 0, 0, 0
		}, new List<short>
		{
			39, 40, 41, 42, 43, 44, 45, 46, 47, 54,
			55, 56, 48, 49, 50, 51, 52, 53, 1, 2,
			3, 4, 5, 6, 7, 8, 9, 10, 11, 12,
			13, 14, 15, 34, 35, 36
		}, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[4] { 4, 5, 6, 7 }, new PresetItemTemplateId("Food", 81), new sbyte[4] { 4, 5, 6, 7 }, new short[6], new bool[12]
		{
			true, false, false, false, false, false, false, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), 190, -1, 247, 251, new int[0], new int[0], new int[0], new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>(), new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(117, 252, EMapPickupsType.Item, EMapPickupsType2.Food, "map_eventicon_9", 226, 0, new byte[15]
		{
			0, 1, 0, 0, 0, 1, 0, 0, 0, 0,
			0, 0, 0, 0, 0
		}, new List<short>
		{
			39, 40, 41, 42, 43, 44, 45, 46, 47, 54,
			55, 56, 48, 49, 50, 51, 52, 53, 1, 2,
			3, 4, 5, 6, 7, 8, 9, 10, 11, 12,
			13, 14, 15, 34, 35, 36
		}, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[3] { 5, 6, 7 }, new PresetItemTemplateId("Food", 86), new sbyte[3] { 5, 6, 7 }, new short[6], new bool[12]
		{
			true, false, false, false, false, false, false, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), 190, -1, 247, 253, new int[0], new int[0], new int[0], new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>(), new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(118, 254, EMapPickupsType.Item, EMapPickupsType2.Food, "map_eventicon_9", 226, 0, new byte[15]
		{
			0, 1, 0, 0, 0, 0, 0, 0, 0, 0,
			0, 0, 0, 0, 0
		}, new List<short>
		{
			39, 40, 41, 42, 43, 44, 45, 46, 47, 54,
			55, 56, 48, 49, 50, 51, 52, 53, 1, 2,
			3, 4, 5, 6, 7, 8, 9, 10, 11, 12,
			13, 14, 15, 34, 35, 36
		}, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[2] { 6, 7 }, new PresetItemTemplateId("Food", 90), new sbyte[2] { 6, 7 }, new short[6], new bool[12]
		{
			true, false, false, false, false, false, false, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), 190, -1, 247, 255, new int[0], new int[0], new int[0], new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>(), new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(119, 256, EMapPickupsType.Item, EMapPickupsType2.Food, "map_eventicon_9", 226, 1, new byte[15]
		{
			1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
			1, 1, 1, 1, 1
		}, new List<short>
		{
			39, 40, 41, 42, 43, 44, 45, 46, 47, 54,
			55, 56, 48, 49, 50, 51, 52, 53, 1, 2,
			3, 4, 5, 6, 7, 8, 9, 10, 11, 12,
			13, 14, 15, 34, 35, 36
		}, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new PresetItemTemplateId("Food", 93), new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new short[6], new bool[12]
		{
			false, true, true, true, false, false, false, true, true, true,
			false, false
		}, -1, new OrganizationApproving(), 191, -1, 247, 257, new int[0], new int[0], new int[0], new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>(), new List<PresetItemWithCount>()));
	}

	private void CreateItems2()
	{
		_dataArray.Add(new MapPickupsItem(120, 258, EMapPickupsType.Item, EMapPickupsType2.Food, "map_eventicon_9", 226, 0, new byte[15]
		{
			1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
			1, 1, 1, 1, 1
		}, new List<short>
		{
			39, 40, 41, 42, 43, 44, 45, 46, 47, 54,
			55, 56, 48, 49, 50, 51, 52, 53, 1, 2,
			3, 4, 5, 6, 7, 8, 9, 10, 11, 12,
			13, 14, 15, 34, 35, 36
		}, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[7] { 1, 2, 3, 4, 5, 6, 7 }, new PresetItemTemplateId("Food", 102), new sbyte[7] { 1, 2, 3, 4, 5, 6, 7 }, new short[6], new bool[12]
		{
			false, true, true, true, false, false, false, true, true, true,
			false, false
		}, -1, new OrganizationApproving(), 191, -1, 247, 259, new int[0], new int[0], new int[0], new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>(), new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(121, 260, EMapPickupsType.Item, EMapPickupsType2.Food, "map_eventicon_9", 226, 0, new byte[15]
		{
			1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
			1, 1, 1, 1, 1
		}, new List<short>
		{
			39, 40, 41, 42, 43, 44, 45, 46, 47, 54,
			55, 56, 48, 49, 50, 51, 52, 53, 1, 2,
			3, 4, 5, 6, 7, 8, 9, 10, 11, 12,
			13, 14, 15, 34, 35, 36
		}, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[6] { 2, 3, 4, 5, 6, 7 }, new PresetItemTemplateId("Food", 110), new sbyte[6] { 2, 3, 4, 5, 6, 7 }, new short[6], new bool[12]
		{
			false, true, true, true, false, false, false, true, true, true,
			false, false
		}, -1, new OrganizationApproving(), 191, -1, 247, 261, new int[0], new int[0], new int[0], new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>(), new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(122, 262, EMapPickupsType.Item, EMapPickupsType2.Food, "map_eventicon_9", 226, 0, new byte[15]
		{
			1, 0, 1, 1, 0, 0, 0, 1, 0, 0,
			0, 0, 0, 0, 0
		}, new List<short>
		{
			39, 40, 41, 42, 43, 44, 45, 46, 47, 54,
			55, 56, 48, 49, 50, 51, 52, 53, 1, 2,
			3, 4, 5, 6, 7, 8, 9, 10, 11, 12,
			13, 14, 15, 34, 35, 36
		}, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[5] { 3, 4, 5, 6, 7 }, new PresetItemTemplateId("Food", 117), new sbyte[5] { 3, 4, 5, 6, 7 }, new short[6], new bool[12]
		{
			false, true, true, true, false, false, false, true, true, true,
			false, false
		}, -1, new OrganizationApproving(), 191, -1, 247, 263, new int[0], new int[0], new int[0], new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>(), new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(123, 264, EMapPickupsType.Item, EMapPickupsType2.Food, "map_eventicon_9", 226, 0, new byte[15]
		{
			1, 0, 1, 0, 0, 0, 0, 1, 0, 0,
			0, 0, 0, 0, 0
		}, new List<short>
		{
			39, 40, 41, 42, 43, 44, 45, 46, 47, 54,
			55, 56, 48, 49, 50, 51, 52, 53, 1, 2,
			3, 4, 5, 6, 7, 8, 9, 10, 11, 12,
			13, 14, 15, 34, 35, 36
		}, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[4] { 4, 5, 6, 7 }, new PresetItemTemplateId("Food", 123), new sbyte[4] { 4, 5, 6, 7 }, new short[6], new bool[12]
		{
			false, true, true, true, false, false, false, true, true, true,
			false, false
		}, -1, new OrganizationApproving(), 191, -1, 247, 265, new int[0], new int[0], new int[0], new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>(), new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(124, 266, EMapPickupsType.Item, EMapPickupsType2.Food, "map_eventicon_9", 226, 0, new byte[15]
		{
			1, 0, 1, 0, 0, 0, 0, 0, 0, 0,
			0, 0, 0, 0, 0
		}, new List<short>
		{
			39, 40, 41, 42, 43, 44, 45, 46, 47, 54,
			55, 56, 48, 49, 50, 51, 52, 53, 1, 2,
			3, 4, 5, 6, 7, 8, 9, 10, 11, 12,
			13, 14, 15, 34, 35, 36
		}, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[3] { 5, 6, 7 }, new PresetItemTemplateId("Food", 128), new sbyte[3] { 5, 6, 7 }, new short[6], new bool[12]
		{
			false, true, true, true, false, false, false, true, true, true,
			false, false
		}, -1, new OrganizationApproving(), 191, -1, 247, 267, new int[0], new int[0], new int[0], new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>(), new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(125, 268, EMapPickupsType.Item, EMapPickupsType2.Food, "map_eventicon_9", 226, 0, new byte[15]
		{
			1, 0, 0, 0, 0, 0, 0, 0, 0, 0,
			0, 0, 0, 0, 0
		}, new List<short>
		{
			39, 40, 41, 42, 43, 44, 45, 46, 47, 54,
			55, 56, 48, 49, 50, 51, 52, 53, 1, 2,
			3, 4, 5, 6, 7, 8, 9, 10, 11, 12,
			13, 14, 15, 34, 35, 36
		}, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[2] { 6, 7 }, new PresetItemTemplateId("Food", 132), new sbyte[2] { 6, 7 }, new short[6], new bool[12]
		{
			false, true, true, true, false, false, false, true, true, true,
			false, false
		}, -1, new OrganizationApproving(), 191, -1, 247, 269, new int[0], new int[0], new int[0], new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>(), new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(126, 270, EMapPickupsType.Item, EMapPickupsType2.Food, "map_eventicon_9", 226, 1, new byte[15]
		{
			1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
			1, 1, 1, 1, 1
		}, new List<short>
		{
			39, 40, 41, 42, 43, 44, 45, 46, 47, 54,
			55, 56, 48, 49, 50, 51, 52, 53, 1, 2,
			3, 4, 5, 6, 7, 8, 9, 10, 11, 12,
			13, 14, 15, 34, 35, 36
		}, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new PresetItemTemplateId("Food", 135), new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new short[6], new bool[12]
		{
			false, true, true, true, true, true, true, false, false, false,
			false, false
		}, -1, new OrganizationApproving(), 192, -1, 247, 271, new int[0], new int[0], new int[0], new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>(), new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(127, 272, EMapPickupsType.Item, EMapPickupsType2.Food, "map_eventicon_9", 226, 0, new byte[15]
		{
			1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
			1, 1, 1, 1, 1
		}, new List<short>
		{
			39, 40, 41, 42, 43, 44, 45, 46, 47, 54,
			55, 56, 48, 49, 50, 51, 52, 53, 1, 2,
			3, 4, 5, 6, 7, 8, 9, 10, 11, 12,
			13, 14, 15, 34, 35, 36
		}, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[7] { 1, 2, 3, 4, 5, 6, 7 }, new PresetItemTemplateId("Food", 144), new sbyte[7] { 1, 2, 3, 4, 5, 6, 7 }, new short[6], new bool[12]
		{
			false, true, true, true, true, true, true, false, false, false,
			false, false
		}, -1, new OrganizationApproving(), 192, -1, 247, 273, new int[0], new int[0], new int[0], new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>(), new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(128, 274, EMapPickupsType.Item, EMapPickupsType2.Food, "map_eventicon_9", 226, 0, new byte[15]
		{
			1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
			1, 1, 1, 1, 1
		}, new List<short>
		{
			39, 40, 41, 42, 43, 44, 45, 46, 47, 54,
			55, 56, 48, 49, 50, 51, 52, 53, 1, 2,
			3, 4, 5, 6, 7, 8, 9, 10, 11, 12,
			13, 14, 15, 34, 35, 36
		}, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[6] { 2, 3, 4, 5, 6, 7 }, new PresetItemTemplateId("Food", 152), new sbyte[6] { 2, 3, 4, 5, 6, 7 }, new short[6], new bool[12]
		{
			false, true, true, true, true, true, true, false, false, false,
			false, false
		}, -1, new OrganizationApproving(), 192, -1, 247, 275, new int[0], new int[0], new int[0], new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>(), new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(129, 276, EMapPickupsType.Item, EMapPickupsType2.Food, "map_eventicon_9", 226, 0, new byte[15]
		{
			0, 0, 0, 1, 0, 0, 0, 0, 0, 0,
			0, 0, 1, 1, 1
		}, new List<short>
		{
			39, 40, 41, 42, 43, 44, 45, 46, 47, 54,
			55, 56, 48, 49, 50, 51, 52, 53, 1, 2,
			3, 4, 5, 6, 7, 8, 9, 10, 11, 12,
			13, 14, 15, 34, 35, 36
		}, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[5] { 3, 4, 5, 6, 7 }, new PresetItemTemplateId("Food", 159), new sbyte[5] { 3, 4, 5, 6, 7 }, new short[6], new bool[12]
		{
			false, true, true, true, true, true, true, false, false, false,
			false, false
		}, -1, new OrganizationApproving(), 192, -1, 247, 277, new int[0], new int[0], new int[0], new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>(), new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(130, 278, EMapPickupsType.Item, EMapPickupsType2.Food, "map_eventicon_9", 226, 0, new byte[15]
		{
			0, 0, 0, 1, 0, 0, 0, 0, 0, 0,
			0, 0, 0, 1, 1
		}, new List<short>
		{
			39, 40, 41, 42, 43, 44, 45, 46, 47, 54,
			55, 56, 48, 49, 50, 51, 52, 53, 1, 2,
			3, 4, 5, 6, 7, 8, 9, 10, 11, 12,
			13, 14, 15, 34, 35, 36
		}, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[4] { 4, 5, 6, 7 }, new PresetItemTemplateId("Food", 165), new sbyte[4] { 4, 5, 6, 7 }, new short[6], new bool[12]
		{
			false, true, true, true, true, true, true, false, false, false,
			false, false
		}, -1, new OrganizationApproving(), 192, -1, 247, 279, new int[0], new int[0], new int[0], new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>(), new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(131, 280, EMapPickupsType.Item, EMapPickupsType2.Food, "map_eventicon_9", 226, 0, new byte[15]
		{
			0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
			0, 0, 0, 1, 1
		}, new List<short>
		{
			39, 40, 41, 42, 43, 44, 45, 46, 47, 54,
			55, 56, 48, 49, 50, 51, 52, 53, 1, 2,
			3, 4, 5, 6, 7, 8, 9, 10, 11, 12,
			13, 14, 15, 34, 35, 36
		}, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[3] { 5, 6, 7 }, new PresetItemTemplateId("Food", 170), new sbyte[3] { 5, 6, 7 }, new short[6], new bool[12]
		{
			false, true, true, true, true, true, true, false, false, false,
			false, false
		}, -1, new OrganizationApproving(), 192, -1, 247, 281, new int[0], new int[0], new int[0], new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>(), new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(132, 282, EMapPickupsType.Item, EMapPickupsType2.Food, "map_eventicon_9", 226, 0, new byte[15]
		{
			0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
			0, 0, 0, 1, 0
		}, new List<short>
		{
			39, 40, 41, 42, 43, 44, 45, 46, 47, 54,
			55, 56, 48, 49, 50, 51, 52, 53, 1, 2,
			3, 4, 5, 6, 7, 8, 9, 10, 11, 12,
			13, 14, 15, 34, 35, 36
		}, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[2] { 6, 7 }, new PresetItemTemplateId("Food", 174), new sbyte[2] { 6, 7 }, new short[6], new bool[12]
		{
			false, true, true, true, true, true, true, false, false, false,
			false, false
		}, -1, new OrganizationApproving(), 192, -1, 247, 283, new int[0], new int[0], new int[0], new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>(), new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(133, 284, EMapPickupsType.Item, EMapPickupsType2.TeaWine, "map_eventicon_10", 285, 1, new byte[15]
		{
			0, 1, 1, 0, 1, 1, 0, 2, 1, 1,
			2, 0, 0, 2, 2
		}, new List<short>
		{
			39, 40, 41, 42, 43, 44, 45, 46, 47, 54,
			55, 56, 48, 49, 50, 51, 52, 53, 1, 2,
			3, 4, 5, 6, 7, 8, 9, 10, 11, 12,
			13, 14, 15, 34, 35, 36
		}, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new PresetItemTemplateId("TeaWine", 0), new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new short[6], new bool[12]
		{
			true, false, false, false, true, true, true, false, false, false,
			true, true
		}, -1, new OrganizationApproving(), 193, -1, 247, 286, new int[0], new int[0], new int[0], new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>(), new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(134, 287, EMapPickupsType.Item, EMapPickupsType2.TeaWine, "map_eventicon_10", 285, 1, new byte[15]
		{
			0, 1, 1, 0, 1, 1, 0, 2, 1, 1,
			2, 0, 0, 2, 2
		}, new List<short>
		{
			39, 40, 41, 42, 43, 44, 45, 46, 47, 54,
			55, 56, 48, 49, 50, 51, 52, 53, 1, 2,
			3, 4, 5, 6, 7, 8, 9, 10, 11, 12,
			13, 14, 15, 34, 35, 36
		}, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new PresetItemTemplateId("TeaWine", 9), new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new short[6], new bool[12]
		{
			true, false, false, false, true, true, true, false, false, false,
			true, true
		}, -1, new OrganizationApproving(), 193, -1, 247, 288, new int[0], new int[0], new int[0], new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>(), new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(135, 289, EMapPickupsType.Item, EMapPickupsType2.TeaWine, "map_eventicon_10", 285, 1, new byte[15]
		{
			2, 1, 1, 1, 0, 0, 1, 2, 1, 0,
			0, 2, 1, 0, 2
		}, new List<short>
		{
			39, 40, 41, 42, 43, 44, 45, 46, 47, 54,
			55, 56, 48, 49, 50, 51, 52, 53, 1, 2,
			3, 4, 5, 6, 7, 8, 9, 10, 11, 12,
			13, 14, 15, 34, 35, 36
		}, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new PresetItemTemplateId("TeaWine", 18), new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new short[6], new bool[12]
		{
			false, true, true, true, false, false, false, true, true, true,
			false, false
		}, -1, new OrganizationApproving(), 194, -1, 247, 290, new int[0], new int[0], new int[0], new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>(), new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(136, 291, EMapPickupsType.Item, EMapPickupsType2.TeaWine, "map_eventicon_10", 285, 1, new byte[15]
		{
			2, 1, 1, 1, 0, 0, 1, 2, 1, 0,
			0, 2, 1, 0, 2
		}, new List<short>
		{
			39, 40, 41, 42, 43, 44, 45, 46, 47, 54,
			55, 56, 48, 49, 50, 51, 52, 53, 1, 2,
			3, 4, 5, 6, 7, 8, 9, 10, 11, 12,
			13, 14, 15, 34, 35, 36
		}, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new PresetItemTemplateId("TeaWine", 27), new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new short[6], new bool[12]
		{
			false, true, true, true, false, false, false, true, true, true,
			false, false
		}, -1, new OrganizationApproving(), 194, -1, 247, 292, new int[0], new int[0], new int[0], new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>(), new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(137, 293, EMapPickupsType.Item, EMapPickupsType2.Tool, "map_eventicon_11", 294, 1, new byte[15]
		{
			0, 1, 0, 0, 1, 1, 0, 0, 0, 0,
			0, 0, 0, 0, 0
		}, new List<short>
		{
			1, 2, 3, 4, 5, 6, 7, 8, 9, 10,
			11, 12, 13, 14, 15, 34, 35, 36
		}, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[7] { 0, 1, 2, 3, 4, 5, 6 }, new PresetItemTemplateId("CraftTool", 36), new sbyte[7] { 0, 1, 2, 3, 4, 5, 6 }, new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), 195, -1, 247, 295, new int[0], new int[0], new int[0], new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>(), new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(138, 296, EMapPickupsType.Item, EMapPickupsType2.Tool, "map_eventicon_11", 294, 1, new byte[15]
		{
			0, 0, 0, 1, 0, 1, 0, 0, 1, 0,
			0, 1, 0, 0, 0
		}, new List<short>
		{
			1, 2, 3, 4, 5, 6, 7, 8, 9, 10,
			11, 12, 13, 14, 15, 34, 35, 36
		}, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[7] { 0, 1, 2, 3, 4, 5, 6 }, new PresetItemTemplateId("CraftTool", 0), new sbyte[7] { 0, 1, 2, 3, 4, 5, 6 }, new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), 195, -1, 247, 297, new int[0], new int[0], new int[0], new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>(), new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(139, 298, EMapPickupsType.Item, EMapPickupsType2.Tool, "map_eventicon_11", 294, 1, new byte[15]
		{
			0, 0, 0, 0, 0, 1, 0, 0, 1, 0,
			0, 0, 0, 1, 0
		}, new List<short>
		{
			1, 2, 3, 4, 5, 6, 7, 8, 9, 10,
			11, 12, 13, 14, 15, 34, 35, 36
		}, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[7] { 0, 1, 2, 3, 4, 5, 6 }, new PresetItemTemplateId("CraftTool", 9), new sbyte[7] { 0, 1, 2, 3, 4, 5, 6 }, new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), 195, -1, 247, 299, new int[0], new int[0], new int[0], new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>(), new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(140, 300, EMapPickupsType.Item, EMapPickupsType2.Tool, "map_eventicon_11", 294, 1, new byte[15]
		{
			0, 0, 0, 0, 0, 0, 1, 0, 1, 1,
			0, 0, 0, 0, 0
		}, new List<short>
		{
			1, 2, 3, 4, 5, 6, 7, 8, 9, 10,
			11, 12, 13, 14, 15, 34, 35, 36
		}, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[7] { 0, 1, 2, 3, 4, 5, 6 }, new PresetItemTemplateId("CraftTool", 18), new sbyte[7] { 0, 1, 2, 3, 4, 5, 6 }, new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), 195, -1, 247, 301, new int[0], new int[0], new int[0], new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>(), new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(141, 302, EMapPickupsType.Item, EMapPickupsType2.Tool, "map_eventicon_11", 294, 1, new byte[15]
		{
			0, 0, 1, 0, 0, 0, 0, 0, 1, 1,
			0, 0, 0, 0, 0
		}, new List<short>
		{
			1, 2, 3, 4, 5, 6, 7, 8, 9, 10,
			11, 12, 13, 14, 15, 34, 35, 36
		}, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[7] { 0, 1, 2, 3, 4, 5, 6 }, new PresetItemTemplateId("CraftTool", 27), new sbyte[7] { 0, 1, 2, 3, 4, 5, 6 }, new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), 195, -1, 247, 303, new int[0], new int[0], new int[0], new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>(), new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(142, 304, EMapPickupsType.Item, EMapPickupsType2.Tool, "map_eventicon_11", 294, 1, new byte[15]
		{
			1, 0, 1, 0, 1, 0, 0, 0, 0, 1,
			0, 1, 1, 0, 1
		}, new List<short>
		{
			1, 2, 3, 4, 5, 6, 7, 8, 9, 10,
			11, 12, 13, 14, 15, 34, 35, 36
		}, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[7] { 0, 1, 2, 3, 4, 5, 6 }, new PresetItemTemplateId("CraftTool", 45), new sbyte[7] { 0, 1, 2, 3, 4, 5, 6 }, new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), 195, -1, 247, 305, new int[0], new int[0], new int[0], new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>(), new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(143, 306, EMapPickupsType.Item, EMapPickupsType2.Accessory, "map_eventicon_12", 307, 0, new byte[15]
		{
			1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
			1, 1, 1, 1, 1
		}, new List<short>
		{
			57, 58, 59, 60, 61, 62, 63, 64, 65, 66,
			67, 68, 69, 70, 71, 72, 73, 74, 75, 76,
			77, 78, 79, 80, 81, 82, 83, 84, 85, 86,
			87, 88, 89, 90, 91, 92, 93, 118, 119, 120,
			121, 122, 123, 124
		}, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new PresetItemTemplateId("Accessory", 90), new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), 196, -1, 247, 308, new int[0], new int[0], new int[0], new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>(), new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(144, 313, EMapPickupsType.Item, EMapPickupsType2.Accessory, "map_eventicon_12", 307, 0, new byte[15]
		{
			1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
			1, 1, 1, 1, 1
		}, new List<short>
		{
			57, 58, 59, 60, 61, 62, 63, 64, 65, 66,
			67, 68, 69, 70, 71, 72, 73, 74, 75, 76,
			77, 78, 79, 80, 81, 82, 83, 84, 85, 86,
			87, 88, 89, 90, 91, 92, 93, 118, 119, 120,
			121, 122, 123, 124
		}, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new PresetItemTemplateId("Accessory", 99), new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), 196, -1, 247, 314, new int[0], new int[0], new int[0], new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>(), new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(145, 315, EMapPickupsType.Item, EMapPickupsType2.Accessory, "map_eventicon_12", 307, 0, new byte[15]
		{
			1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
			1, 1, 1, 1, 1
		}, new List<short>
		{
			57, 58, 59, 60, 61, 62, 63, 64, 65, 66,
			67, 68, 69, 70, 71, 72, 73, 74, 75, 76,
			77, 78, 79, 80, 81, 82, 83, 84, 85, 86,
			87, 88, 89, 90, 91, 92, 93, 118, 119, 120,
			121, 122, 123, 124
		}, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new PresetItemTemplateId("Accessory", 117), new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), 196, -1, 247, 316, new int[0], new int[0], new int[0], new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>(), new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(146, 317, EMapPickupsType.Item, EMapPickupsType2.Accessory, "map_eventicon_12", 307, 0, new byte[15]
		{
			1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
			1, 1, 1, 1, 1
		}, new List<short>
		{
			57, 58, 59, 60, 61, 62, 63, 64, 65, 66,
			67, 68, 69, 70, 71, 72, 73, 74, 75, 76,
			77, 78, 79, 80, 81, 82, 83, 84, 85, 86,
			87, 88, 89, 90, 91, 92, 93, 118, 119, 120,
			121, 122, 123, 124
		}, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new PresetItemTemplateId("Accessory", 180), new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), 196, -1, 247, 318, new int[0], new int[0], new int[0], new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>(), new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(147, 319, EMapPickupsType.Item, EMapPickupsType2.Accessory, "map_eventicon_12", 307, 0, new byte[15]
		{
			1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
			1, 1, 1, 1, 1
		}, new List<short>
		{
			57, 58, 59, 60, 61, 62, 63, 64, 65, 66,
			67, 68, 69, 70, 71, 72, 73, 74, 75, 76,
			77, 78, 79, 80, 81, 82, 83, 84, 85, 86,
			87, 88, 89, 90, 91, 92, 93, 118, 119, 120,
			121, 122, 123, 124
		}, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new PresetItemTemplateId("Accessory", 108), new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), 196, -1, 247, 320, new int[0], new int[0], new int[0], new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>(), new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(148, 321, EMapPickupsType.Item, EMapPickupsType2.Accessory, "map_eventicon_12", 307, 0, new byte[15]
		{
			1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
			1, 1, 1, 1, 1
		}, new List<short>
		{
			57, 58, 59, 60, 61, 62, 63, 64, 65, 66,
			67, 68, 69, 70, 71, 72, 73, 74, 75, 76,
			77, 78, 79, 80, 81, 82, 83, 84, 85, 86,
			87, 88, 89, 90, 91, 92, 93, 118, 119, 120,
			121, 122, 123, 124
		}, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new PresetItemTemplateId("Accessory", 171), new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), 196, -1, 247, 322, new int[0], new int[0], new int[0], new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>(), new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(149, 323, EMapPickupsType.Item, EMapPickupsType2.Accessory, "map_eventicon_12", 307, 0, new byte[15]
		{
			1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
			1, 1, 1, 1, 1
		}, new List<short>
		{
			57, 58, 59, 60, 61, 62, 63, 64, 65, 66,
			67, 68, 69, 70, 71, 72, 73, 74, 75, 76,
			77, 78, 79, 80, 81, 82, 83, 84, 85, 86,
			87, 88, 89, 90, 91, 92, 93, 118, 119, 120,
			121, 122, 123, 124
		}, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new PresetItemTemplateId("Accessory", 162), new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), 196, -1, 247, 324, new int[0], new int[0], new int[0], new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>(), new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(150, 325, EMapPickupsType.Item, EMapPickupsType2.Accessory, "map_eventicon_12", 307, 0, new byte[15]
		{
			1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
			1, 1, 1, 1, 1
		}, new List<short>
		{
			57, 58, 59, 60, 61, 62, 63, 64, 65, 66,
			67, 68, 69, 70, 71, 72, 73, 74, 75, 76,
			77, 78, 79, 80, 81, 82, 83, 84, 85, 86,
			87, 88, 89, 90, 91, 92, 93, 118, 119, 120,
			121, 122, 123, 124
		}, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new PresetItemTemplateId("Accessory", 153), new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), 196, -1, 247, 326, new int[0], new int[0], new int[0], new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>(), new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(151, 327, EMapPickupsType.Item, EMapPickupsType2.Accessory, "map_eventicon_12", 307, 0, new byte[15]
		{
			1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
			1, 1, 1, 1, 1
		}, new List<short>
		{
			57, 58, 59, 60, 61, 62, 63, 64, 65, 66,
			67, 68, 69, 70, 71, 72, 73, 74, 75, 76,
			77, 78, 79, 80, 81, 82, 83, 84, 85, 86,
			87, 88, 89, 90, 91, 92, 93, 118, 119, 120,
			121, 122, 123, 124
		}, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new PresetItemTemplateId("Accessory", 144), new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), 196, -1, 247, 328, new int[0], new int[0], new int[0], new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>(), new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(152, 329, EMapPickupsType.Item, EMapPickupsType2.Accessory, "map_eventicon_12", 307, 0, new byte[15]
		{
			1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
			1, 1, 1, 1, 1
		}, new List<short>
		{
			57, 58, 59, 60, 61, 62, 63, 64, 65, 66,
			67, 68, 69, 70, 71, 72, 73, 74, 75, 76,
			77, 78, 79, 80, 81, 82, 83, 84, 85, 86,
			87, 88, 89, 90, 91, 92, 93, 118, 119, 120,
			121, 122, 123, 124
		}, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new PresetItemTemplateId("Accessory", 135), new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), 196, -1, 247, 330, new int[0], new int[0], new int[0], new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>(), new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(153, 309, EMapPickupsType.Item, EMapPickupsType2.Accessory, "map_eventicon_12", 307, 0, new byte[15]
		{
			1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
			1, 1, 1, 1, 1
		}, new List<short>
		{
			57, 58, 59, 60, 61, 62, 63, 64, 65, 66,
			67, 68, 69, 70, 71, 72, 73, 74, 75, 76,
			77, 78, 79, 80, 81, 82, 83, 84, 85, 86,
			87, 88, 89, 90, 91, 92, 93, 118, 119, 120,
			121, 122, 123, 124
		}, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new PresetItemTemplateId("Accessory", 0), new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), 196, -1, 247, 310, new int[0], new int[0], new int[0], new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>(), new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(154, 311, EMapPickupsType.Item, EMapPickupsType2.Accessory, "map_eventicon_12", 307, 0, new byte[15]
		{
			1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
			1, 1, 1, 1, 1
		}, new List<short>
		{
			57, 58, 59, 60, 61, 62, 63, 64, 65, 66,
			67, 68, 69, 70, 71, 72, 73, 74, 75, 76,
			77, 78, 79, 80, 81, 82, 83, 84, 85, 86,
			87, 88, 89, 90, 91, 92, 93, 118, 119, 120,
			121, 122, 123, 124
		}, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new PresetItemTemplateId("Accessory", 207), new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), 196, -1, 247, 312, new int[0], new int[0], new int[0], new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>(), new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(155, 331, EMapPickupsType.Item, EMapPickupsType2.Accessory, "map_eventicon_12", 307, 0, new byte[15]
		{
			1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
			1, 1, 1, 1, 1
		}, new List<short>
		{
			57, 58, 59, 60, 61, 62, 63, 64, 65, 66,
			67, 68, 69, 70, 71, 72, 73, 74, 75, 76,
			77, 78, 79, 80, 81, 82, 83, 84, 85, 86,
			87, 88, 89, 90, 91, 92, 93, 118, 119, 120,
			121, 122, 123, 124
		}, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new PresetItemTemplateId("Accessory", 72), new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), 196, -1, 247, 332, new int[0], new int[0], new int[0], new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>(), new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(156, 333, EMapPickupsType.Item, EMapPickupsType2.Accessory, "map_eventicon_12", 307, 0, new byte[15]
		{
			1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
			1, 1, 1, 1, 1
		}, new List<short>
		{
			57, 58, 59, 60, 61, 62, 63, 64, 65, 66,
			67, 68, 69, 70, 71, 72, 73, 74, 75, 76,
			77, 78, 79, 80, 81, 82, 83, 84, 85, 86,
			87, 88, 89, 90, 91, 92, 93, 118, 119, 120,
			121, 122, 123, 124
		}, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new PresetItemTemplateId("Accessory", 81), new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), 196, -1, 247, 334, new int[0], new int[0], new int[0], new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>(), new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(157, 335, EMapPickupsType.Item, EMapPickupsType2.Accessory, "map_eventicon_12", 307, 0, new byte[15]
		{
			1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
			1, 1, 1, 1, 1
		}, new List<short>
		{
			57, 58, 59, 60, 61, 62, 63, 64, 65, 66,
			67, 68, 69, 70, 71, 72, 73, 74, 75, 76,
			77, 78, 79, 80, 81, 82, 83, 84, 85, 86,
			87, 88, 89, 90, 91, 92, 93, 118, 119, 120,
			121, 122, 123, 124
		}, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new PresetItemTemplateId("Accessory", 9), new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), 196, -1, 247, 336, new int[0], new int[0], new int[0], new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>(), new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(158, 337, EMapPickupsType.Item, EMapPickupsType2.Accessory, "map_eventicon_12", 307, 0, new byte[15]
		{
			1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
			1, 1, 1, 1, 1
		}, new List<short>
		{
			57, 58, 59, 60, 61, 62, 63, 64, 65, 66,
			67, 68, 69, 70, 71, 72, 73, 74, 75, 76,
			77, 78, 79, 80, 81, 82, 83, 84, 85, 86,
			87, 88, 89, 90, 91, 92, 93, 118, 119, 120,
			121, 122, 123, 124
		}, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new PresetItemTemplateId("Accessory", 126), new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), 196, -1, 247, 338, new int[0], new int[0], new int[0], new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>(), new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(159, 339, EMapPickupsType.Item, EMapPickupsType2.Accessory, "map_eventicon_12", 307, 0, new byte[15]
		{
			1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
			1, 1, 1, 1, 1
		}, new List<short>
		{
			57, 58, 59, 60, 61, 62, 63, 64, 65, 66,
			67, 68, 69, 70, 71, 72, 73, 74, 75, 76,
			77, 78, 79, 80, 81, 82, 83, 84, 85, 86,
			87, 88, 89, 90, 91, 92, 93, 118, 119, 120,
			121, 122, 123, 124
		}, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new PresetItemTemplateId("Accessory", 189), new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), 196, -1, 247, 340, new int[0], new int[0], new int[0], new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>(), new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(160, 341, EMapPickupsType.Item, EMapPickupsType2.Accessory, "map_eventicon_12", 307, 0, new byte[15]
		{
			1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
			1, 1, 1, 1, 1
		}, new List<short>
		{
			57, 58, 59, 60, 61, 62, 63, 64, 65, 66,
			67, 68, 69, 70, 71, 72, 73, 74, 75, 76,
			77, 78, 79, 80, 81, 82, 83, 84, 85, 86,
			87, 88, 89, 90, 91, 92, 93, 118, 119, 120,
			121, 122, 123, 124
		}, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new PresetItemTemplateId("Accessory", 216), new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), 196, -1, 247, 342, new int[0], new int[0], new int[0], new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>(), new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(161, 343, EMapPickupsType.Item, EMapPickupsType2.Accessory, "map_eventicon_12", 307, 0, new byte[15]
		{
			1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
			1, 1, 1, 1, 1
		}, new List<short>
		{
			57, 58, 59, 60, 61, 62, 63, 64, 65, 66,
			67, 68, 69, 70, 71, 72, 73, 74, 75, 76,
			77, 78, 79, 80, 81, 82, 83, 84, 85, 86,
			87, 88, 89, 90, 91, 92, 93, 118, 119, 120,
			121, 122, 123, 124
		}, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new PresetItemTemplateId("Accessory", 198), new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), 196, -1, 247, 344, new int[0], new int[0], new int[0], new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>(), new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(162, 345, EMapPickupsType.Item, EMapPickupsType2.Accessory, "map_eventicon_12", 307, 0, new byte[15]
		{
			0, 0, 0, 0, 0, 0, 0, 0, 0, 1,
			1, 1, 0, 0, 1
		}, new List<short>
		{
			57, 58, 59, 60, 61, 62, 63, 64, 65, 66,
			67, 68, 69, 70, 71, 72, 73, 74, 75, 76,
			77, 78, 79, 80, 81, 82, 83, 84, 85, 86,
			87, 88, 89, 90, 91, 92, 93, 118, 119, 120,
			121, 122, 123, 124
		}, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new PresetItemTemplateId("Accessory", 18), new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), 196, -1, 247, 346, new int[0], new int[0], new int[0], new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>(), new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(163, 347, EMapPickupsType.Item, EMapPickupsType2.Accessory, "map_eventicon_12", 307, 0, new byte[15]
		{
			0, 0, 1, 0, 0, 0, 0, 1, 0, 1,
			0, 1, 1, 0, 0
		}, new List<short>
		{
			57, 58, 59, 60, 61, 62, 63, 64, 65, 66,
			67, 68, 69, 70, 71, 72, 73, 74, 75, 76,
			77, 78, 79, 80, 81, 82, 83, 84, 85, 86,
			87, 88, 89, 90, 91, 92, 93, 118, 119, 120,
			121, 122, 123, 124
		}, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new PresetItemTemplateId("Accessory", 45), new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), 196, -1, 247, 348, new int[0], new int[0], new int[0], new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>(), new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(164, 349, EMapPickupsType.Item, EMapPickupsType2.Accessory, "map_eventicon_12", 307, 0, new byte[15]
		{
			0, 0, 0, 0, 0, 0, 0, 0, 0, 1,
			1, 1, 0, 0, 1
		}, new List<short>
		{
			57, 58, 59, 60, 61, 62, 63, 64, 65, 66,
			67, 68, 69, 70, 71, 72, 73, 74, 75, 76,
			77, 78, 79, 80, 81, 82, 83, 84, 85, 86,
			87, 88, 89, 90, 91, 92, 93, 118, 119, 120,
			121, 122, 123, 124
		}, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new PresetItemTemplateId("Accessory", 54), new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), 196, -1, 247, 350, new int[0], new int[0], new int[0], new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>(), new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(165, 351, EMapPickupsType.Item, EMapPickupsType2.Accessory, "map_eventicon_12", 307, 0, new byte[15]
		{
			0, 0, 1, 0, 0, 0, 0, 1, 0, 1,
			0, 1, 1, 0, 0
		}, new List<short>
		{
			57, 58, 59, 60, 61, 62, 63, 64, 65, 66,
			67, 68, 69, 70, 71, 72, 73, 74, 75, 76,
			77, 78, 79, 80, 81, 82, 83, 84, 85, 86,
			87, 88, 89, 90, 91, 92, 93, 118, 119, 120,
			121, 122, 123, 124
		}, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new PresetItemTemplateId("Accessory", 27), new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), 196, -1, 247, 352, new int[0], new int[0], new int[0], new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>(), new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(166, 353, EMapPickupsType.Item, EMapPickupsType2.Accessory, "map_eventicon_12", 307, 0, new byte[15]
		{
			0, 0, 0, 0, 0, 0, 0, 0, 0, 1,
			1, 1, 0, 0, 1
		}, new List<short>
		{
			57, 58, 59, 60, 61, 62, 63, 64, 65, 66,
			67, 68, 69, 70, 71, 72, 73, 74, 75, 76,
			77, 78, 79, 80, 81, 82, 83, 84, 85, 86,
			87, 88, 89, 90, 91, 92, 93, 118, 119, 120,
			121, 122, 123, 124
		}, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new PresetItemTemplateId("Accessory", 36), new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), 196, -1, 247, 354, new int[0], new int[0], new int[0], new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>(), new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(167, 355, EMapPickupsType.Item, EMapPickupsType2.Accessory, "map_eventicon_12", 307, 0, new byte[15]
		{
			0, 0, 1, 0, 0, 0, 0, 1, 0, 1,
			0, 1, 1, 0, 0
		}, new List<short>
		{
			57, 58, 59, 60, 61, 62, 63, 64, 65, 66,
			67, 68, 69, 70, 71, 72, 73, 74, 75, 76,
			77, 78, 79, 80, 81, 82, 83, 84, 85, 86,
			87, 88, 89, 90, 91, 92, 93, 118, 119, 120,
			121, 122, 123, 124
		}, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new PresetItemTemplateId("Accessory", 63), new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), 196, -1, 247, 356, new int[0], new int[0], new int[0], new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>(), new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(168, 357, EMapPickupsType.Item, EMapPickupsType2.Weapon, "map_eventicon_13", 358, 0, new byte[15]
		{
			0, 0, 0, 0, 0, 0, 0, 0, 0, 1,
			0, 0, 0, 0, 0
		}, new List<short>
		{
			57, 58, 59, 60, 61, 62, 63, 64, 65, 66,
			67, 68, 69, 70, 71, 72, 73, 74, 75, 76,
			77, 78, 79, 80, 81, 82, 83, 84, 85, 86,
			87, 88, 89, 90, 91, 92, 93, 118, 119, 120,
			121, 122, 123, 124
		}, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new PresetItemTemplateId("Weapon", 3), new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), 197, -1, 247, 359, new int[0], new int[0], new int[0], new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>(), new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(169, 360, EMapPickupsType.Item, EMapPickupsType2.Weapon, "map_eventicon_13", 358, 0, new byte[15]
		{
			0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
			0, 0, 0, 0, 1
		}, new List<short>
		{
			57, 58, 59, 60, 61, 62, 63, 64, 65, 66,
			67, 68, 69, 70, 71, 72, 73, 74, 75, 76,
			77, 78, 79, 80, 81, 82, 83, 84, 85, 86,
			87, 88, 89, 90, 91, 92, 93, 118, 119, 120,
			121, 122, 123, 124
		}, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new PresetItemTemplateId("Weapon", 12), new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), 197, -1, 247, 361, new int[0], new int[0], new int[0], new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>(), new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(170, 362, EMapPickupsType.Item, EMapPickupsType2.Weapon, "map_eventicon_13", 358, 0, new byte[15]
		{
			0, 0, 0, 0, 0, 0, 0, 0, 1, 0,
			0, 0, 0, 0, 0
		}, new List<short>
		{
			57, 58, 59, 60, 61, 62, 63, 64, 65, 66,
			67, 68, 69, 70, 71, 72, 73, 74, 75, 76,
			77, 78, 79, 80, 81, 82, 83, 84, 85, 86,
			87, 88, 89, 90, 91, 92, 93, 118, 119, 120,
			121, 122, 123, 124
		}, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new PresetItemTemplateId("Weapon", 21), new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), 198, -1, 247, 363, new int[0], new int[0], new int[0], new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>(), new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(171, 364, EMapPickupsType.Item, EMapPickupsType2.Weapon, "map_eventicon_13", 358, 0, new byte[15]
		{
			0, 0, 0, 0, 0, 0, 0, 0, 1, 0,
			0, 0, 0, 0, 0
		}, new List<short>
		{
			57, 58, 59, 60, 61, 62, 63, 64, 65, 66,
			67, 68, 69, 70, 71, 72, 73, 74, 75, 76,
			77, 78, 79, 80, 81, 82, 83, 84, 85, 86,
			87, 88, 89, 90, 91, 92, 93, 118, 119, 120,
			121, 122, 123, 124
		}, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new PresetItemTemplateId("Weapon", 30), new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), 198, -1, 247, 365, new int[0], new int[0], new int[0], new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>(), new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(172, 366, EMapPickupsType.Item, EMapPickupsType2.Weapon, "map_eventicon_13", 358, 0, new byte[15]
		{
			0, 0, 0, 0, 0, 0, 1, 0, 0, 0,
			0, 0, 0, 0, 0
		}, new List<short>
		{
			57, 58, 59, 60, 61, 62, 63, 64, 65, 66,
			67, 68, 69, 70, 71, 72, 73, 74, 75, 76,
			77, 78, 79, 80, 81, 82, 83, 84, 85, 86,
			87, 88, 89, 90, 91, 92, 93, 118, 119, 120,
			121, 122, 123, 124
		}, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new PresetItemTemplateId("Weapon", 39), new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), 199, -1, 247, 367, new int[0], new int[0], new int[0], new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>(), new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(173, 368, EMapPickupsType.Item, EMapPickupsType2.Weapon, "map_eventicon_13", 358, 0, new byte[15]
		{
			0, 0, 0, 0, 0, 0, 1, 0, 0, 0,
			0, 0, 0, 0, 0
		}, new List<short>
		{
			57, 58, 59, 60, 61, 62, 63, 64, 65, 66,
			67, 68, 69, 70, 71, 72, 73, 74, 75, 76,
			77, 78, 79, 80, 81, 82, 83, 84, 85, 86,
			87, 88, 89, 90, 91, 92, 93, 118, 119, 120,
			121, 122, 123, 124
		}, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new PresetItemTemplateId("Weapon", 48), new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), 199, -1, 247, 369, new int[0], new int[0], new int[0], new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>(), new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(174, 370, EMapPickupsType.Item, EMapPickupsType2.Weapon, "map_eventicon_13", 358, 0, new byte[15]
		{
			0, 0, 1, 0, 0, 0, 0, 0, 0, 0,
			0, 0, 0, 0, 0
		}, new List<short>
		{
			57, 58, 59, 60, 61, 62, 63, 64, 65, 66,
			67, 68, 69, 70, 71, 72, 73, 74, 75, 76,
			77, 78, 79, 80, 81, 82, 83, 84, 85, 86,
			87, 88, 89, 90, 91, 92, 93, 118, 119, 120,
			121, 122, 123, 124
		}, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new PresetItemTemplateId("Weapon", 57), new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), 200, -1, 247, 371, new int[0], new int[0], new int[0], new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>(), new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(175, 372, EMapPickupsType.Item, EMapPickupsType2.Weapon, "map_eventicon_13", 358, 0, new byte[15]
		{
			0, 0, 1, 0, 0, 0, 0, 0, 0, 0,
			0, 0, 0, 0, 0
		}, new List<short>
		{
			57, 58, 59, 60, 61, 62, 63, 64, 65, 66,
			67, 68, 69, 70, 71, 72, 73, 74, 75, 76,
			77, 78, 79, 80, 81, 82, 83, 84, 85, 86,
			87, 88, 89, 90, 91, 92, 93, 118, 119, 120,
			121, 122, 123, 124
		}, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new PresetItemTemplateId("Weapon", 66), new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), 200, -1, 247, 373, new int[0], new int[0], new int[0], new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>(), new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(176, 374, EMapPickupsType.Item, EMapPickupsType2.Weapon, "map_eventicon_13", 358, 0, new byte[15]
		{
			0, 0, 1, 0, 0, 0, 0, 0, 0, 0,
			0, 0, 0, 0, 0
		}, new List<short>
		{
			57, 58, 59, 60, 61, 62, 63, 64, 65, 66,
			67, 68, 69, 70, 71, 72, 73, 74, 75, 76,
			77, 78, 79, 80, 81, 82, 83, 84, 85, 86,
			87, 88, 89, 90, 91, 92, 93, 118, 119, 120,
			121, 122, 123, 124
		}, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new PresetItemTemplateId("Weapon", 75), new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), 200, -1, 247, 375, new int[0], new int[0], new int[0], new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>(), new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(177, 376, EMapPickupsType.Item, EMapPickupsType2.Weapon, "map_eventicon_13", 358, 0, new byte[15]
		{
			0, 0, 1, 0, 0, 0, 0, 0, 0, 0,
			0, 0, 0, 0, 0
		}, new List<short>
		{
			57, 58, 59, 60, 61, 62, 63, 64, 65, 66,
			67, 68, 69, 70, 71, 72, 73, 74, 75, 76,
			77, 78, 79, 80, 81, 82, 83, 84, 85, 86,
			87, 88, 89, 90, 91, 92, 93, 118, 119, 120,
			121, 122, 123, 124
		}, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new PresetItemTemplateId("Weapon", 84), new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), 200, -1, 247, 377, new int[0], new int[0], new int[0], new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>(), new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(178, 378, EMapPickupsType.Item, EMapPickupsType2.Weapon, "map_eventicon_13", 358, 0, new byte[15]
		{
			0, 0, 1, 0, 0, 0, 0, 0, 0, 0,
			0, 0, 0, 0, 0
		}, new List<short>
		{
			57, 58, 59, 60, 61, 62, 63, 64, 65, 66,
			67, 68, 69, 70, 71, 72, 73, 74, 75, 76,
			77, 78, 79, 80, 81, 82, 83, 84, 85, 86,
			87, 88, 89, 90, 91, 92, 93, 118, 119, 120,
			121, 122, 123, 124
		}, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new PresetItemTemplateId("Weapon", 93), new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), 200, -1, 247, 379, new int[0], new int[0], new int[0], new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>(), new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(179, 380, EMapPickupsType.Item, EMapPickupsType2.Weapon, "map_eventicon_13", 358, 0, new byte[15]
		{
			0, 0, 1, 0, 0, 0, 0, 0, 0, 0,
			0, 0, 0, 0, 0
		}, new List<short>
		{
			57, 58, 59, 60, 61, 62, 63, 64, 65, 66,
			67, 68, 69, 70, 71, 72, 73, 74, 75, 76,
			77, 78, 79, 80, 81, 82, 83, 84, 85, 86,
			87, 88, 89, 90, 91, 92, 93, 118, 119, 120,
			121, 122, 123, 124
		}, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new PresetItemTemplateId("Weapon", 102), new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), 200, -1, 247, 381, new int[0], new int[0], new int[0], new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>(), new List<PresetItemWithCount>()));
	}

	private void CreateItems3()
	{
		_dataArray.Add(new MapPickupsItem(180, 382, EMapPickupsType.Item, EMapPickupsType2.Weapon, "map_eventicon_13", 358, 0, new byte[15]
		{
			0, 1, 0, 0, 0, 0, 0, 0, 0, 0,
			0, 0, 0, 0, 0
		}, new List<short>
		{
			57, 58, 59, 60, 61, 62, 63, 64, 65, 66,
			67, 68, 69, 70, 71, 72, 73, 74, 75, 76,
			77, 78, 79, 80, 81, 82, 83, 84, 85, 86,
			87, 88, 89, 90, 91, 92, 93, 118, 119, 120,
			121, 122, 123, 124
		}, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new PresetItemTemplateId("Weapon", 111), new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), 201, -1, 247, 383, new int[0], new int[0], new int[0], new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>(), new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(181, 384, EMapPickupsType.Item, EMapPickupsType2.Weapon, "map_eventicon_13", 358, 0, new byte[15]
		{
			0, 1, 0, 0, 0, 0, 0, 0, 0, 0,
			0, 0, 0, 0, 0
		}, new List<short>
		{
			57, 58, 59, 60, 61, 62, 63, 64, 65, 66,
			67, 68, 69, 70, 71, 72, 73, 74, 75, 76,
			77, 78, 79, 80, 81, 82, 83, 84, 85, 86,
			87, 88, 89, 90, 91, 92, 93, 118, 119, 120,
			121, 122, 123, 124
		}, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new PresetItemTemplateId("Weapon", 120), new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), 201, -1, 247, 385, new int[0], new int[0], new int[0], new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>(), new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(182, 386, EMapPickupsType.Item, EMapPickupsType2.Weapon, "map_eventicon_13", 358, 0, new byte[15]
		{
			0, 1, 0, 0, 0, 0, 0, 0, 0, 0,
			0, 0, 0, 0, 0
		}, new List<short>
		{
			57, 58, 59, 60, 61, 62, 63, 64, 65, 66,
			67, 68, 69, 70, 71, 72, 73, 74, 75, 76,
			77, 78, 79, 80, 81, 82, 83, 84, 85, 86,
			87, 88, 89, 90, 91, 92, 93, 118, 119, 120,
			121, 122, 123, 124
		}, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new PresetItemTemplateId("Weapon", 129), new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), 201, -1, 247, 387, new int[0], new int[0], new int[0], new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>(), new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(183, 388, EMapPickupsType.Item, EMapPickupsType2.Weapon, "map_eventicon_13", 358, 0, new byte[15]
		{
			0, 1, 0, 0, 0, 0, 0, 0, 0, 0,
			0, 0, 0, 0, 0
		}, new List<short>
		{
			57, 58, 59, 60, 61, 62, 63, 64, 65, 66,
			67, 68, 69, 70, 71, 72, 73, 74, 75, 76,
			77, 78, 79, 80, 81, 82, 83, 84, 85, 86,
			87, 88, 89, 90, 91, 92, 93, 118, 119, 120,
			121, 122, 123, 124
		}, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new PresetItemTemplateId("Weapon", 138), new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), 201, -1, 247, 389, new int[0], new int[0], new int[0], new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>(), new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(184, 390, EMapPickupsType.Item, EMapPickupsType2.Weapon, "map_eventicon_13", 358, 0, new byte[15]
		{
			0, 1, 0, 0, 0, 0, 0, 0, 0, 0,
			0, 0, 0, 0, 0
		}, new List<short>
		{
			57, 58, 59, 60, 61, 62, 63, 64, 65, 66,
			67, 68, 69, 70, 71, 72, 73, 74, 75, 76,
			77, 78, 79, 80, 81, 82, 83, 84, 85, 86,
			87, 88, 89, 90, 91, 92, 93, 118, 119, 120,
			121, 122, 123, 124
		}, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new PresetItemTemplateId("Weapon", 147), new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), 201, -1, 247, 391, new int[0], new int[0], new int[0], new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>(), new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(185, 392, EMapPickupsType.Item, EMapPickupsType2.Weapon, "map_eventicon_13", 358, 0, new byte[15]
		{
			0, 1, 0, 0, 0, 0, 0, 0, 0, 0,
			0, 0, 0, 0, 0
		}, new List<short>
		{
			57, 58, 59, 60, 61, 62, 63, 64, 65, 66,
			67, 68, 69, 70, 71, 72, 73, 74, 75, 76,
			77, 78, 79, 80, 81, 82, 83, 84, 85, 86,
			87, 88, 89, 90, 91, 92, 93, 118, 119, 120,
			121, 122, 123, 124
		}, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new PresetItemTemplateId("Weapon", 156), new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), 201, -1, 247, 393, new int[0], new int[0], new int[0], new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>(), new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(186, 394, EMapPickupsType.Item, EMapPickupsType2.Weapon, "map_eventicon_13", 358, 0, new byte[15]
		{
			0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
			0, 0, 1, 1, 0
		}, new List<short>
		{
			57, 58, 59, 60, 61, 62, 63, 64, 65, 66,
			67, 68, 69, 70, 71, 72, 73, 74, 75, 76,
			77, 78, 79, 80, 81, 82, 83, 84, 85, 86,
			87, 88, 89, 90, 91, 92, 93, 118, 119, 120,
			121, 122, 123, 124
		}, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new PresetItemTemplateId("Weapon", 165), new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), 202, -1, 247, 395, new int[0], new int[0], new int[0], new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>(), new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(187, 396, EMapPickupsType.Item, EMapPickupsType2.Weapon, "map_eventicon_13", 358, 0, new byte[15]
		{
			0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
			0, 0, 1, 1, 0
		}, new List<short>
		{
			57, 58, 59, 60, 61, 62, 63, 64, 65, 66,
			67, 68, 69, 70, 71, 72, 73, 74, 75, 76,
			77, 78, 79, 80, 81, 82, 83, 84, 85, 86,
			87, 88, 89, 90, 91, 92, 93, 118, 119, 120,
			121, 122, 123, 124
		}, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new PresetItemTemplateId("Weapon", 174), new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), 202, -1, 247, 397, new int[0], new int[0], new int[0], new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>(), new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(188, 398, EMapPickupsType.Item, EMapPickupsType2.Weapon, "map_eventicon_13", 358, 0, new byte[15]
		{
			0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
			0, 0, 1, 0, 0
		}, new List<short>
		{
			57, 58, 59, 60, 61, 62, 63, 64, 65, 66,
			67, 68, 69, 70, 71, 72, 73, 74, 75, 76,
			77, 78, 79, 80, 81, 82, 83, 84, 85, 86,
			87, 88, 89, 90, 91, 92, 93, 118, 119, 120,
			121, 122, 123, 124
		}, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new PresetItemTemplateId("Weapon", 183), new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), 202, -1, 247, 399, new int[0], new int[0], new int[0], new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>(), new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(189, 400, EMapPickupsType.Item, EMapPickupsType2.Weapon, "map_eventicon_13", 358, 0, new byte[15]
		{
			0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
			0, 0, 1, 0, 0
		}, new List<short>
		{
			57, 58, 59, 60, 61, 62, 63, 64, 65, 66,
			67, 68, 69, 70, 71, 72, 73, 74, 75, 76,
			77, 78, 79, 80, 81, 82, 83, 84, 85, 86,
			87, 88, 89, 90, 91, 92, 93, 118, 119, 120,
			121, 122, 123, 124
		}, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new PresetItemTemplateId("Weapon", 192), new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), 202, -1, 247, 401, new int[0], new int[0], new int[0], new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>(), new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(190, 402, EMapPickupsType.Item, EMapPickupsType2.Weapon, "map_eventicon_13", 358, 0, new byte[15]
		{
			0, 0, 1, 0, 0, 0, 0, 0, 0, 0,
			0, 0, 1, 0, 0
		}, new List<short>
		{
			57, 58, 59, 60, 61, 62, 63, 64, 65, 66,
			67, 68, 69, 70, 71, 72, 73, 74, 75, 76,
			77, 78, 79, 80, 81, 82, 83, 84, 85, 86,
			87, 88, 89, 90, 91, 92, 93, 118, 119, 120,
			121, 122, 123, 124
		}, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new PresetItemTemplateId("Weapon", 201), new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), 202, -1, 247, 403, new int[0], new int[0], new int[0], new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>(), new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(191, 404, EMapPickupsType.Item, EMapPickupsType2.Weapon, "map_eventicon_13", 358, 0, new byte[15]
		{
			0, 0, 1, 0, 0, 0, 0, 0, 0, 0,
			0, 0, 1, 0, 0
		}, new List<short>
		{
			57, 58, 59, 60, 61, 62, 63, 64, 65, 66,
			67, 68, 69, 70, 71, 72, 73, 74, 75, 76,
			77, 78, 79, 80, 81, 82, 83, 84, 85, 86,
			87, 88, 89, 90, 91, 92, 93, 118, 119, 120,
			121, 122, 123, 124
		}, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new PresetItemTemplateId("Weapon", 210), new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), 202, -1, 247, 405, new int[0], new int[0], new int[0], new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>(), new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(192, 406, EMapPickupsType.Item, EMapPickupsType2.Weapon, "map_eventicon_13", 358, 0, new byte[15]
		{
			0, 0, 1, 0, 0, 0, 0, 0, 0, 0,
			0, 0, 0, 0, 0
		}, new List<short>
		{
			57, 58, 59, 60, 61, 62, 63, 64, 65, 66,
			67, 68, 69, 70, 71, 72, 73, 74, 75, 76,
			77, 78, 79, 80, 81, 82, 83, 84, 85, 86,
			87, 88, 89, 90, 91, 92, 93, 118, 119, 120,
			121, 122, 123, 124
		}, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new PresetItemTemplateId("Weapon", 219), new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), 203, -1, 247, 407, new int[0], new int[0], new int[0], new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>(), new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(193, 408, EMapPickupsType.Item, EMapPickupsType2.Weapon, "map_eventicon_13", 358, 0, new byte[15]
		{
			0, 0, 1, 0, 0, 0, 0, 0, 0, 0,
			0, 0, 0, 0, 0
		}, new List<short>
		{
			57, 58, 59, 60, 61, 62, 63, 64, 65, 66,
			67, 68, 69, 70, 71, 72, 73, 74, 75, 76,
			77, 78, 79, 80, 81, 82, 83, 84, 85, 86,
			87, 88, 89, 90, 91, 92, 93, 118, 119, 120,
			121, 122, 123, 124
		}, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new PresetItemTemplateId("Weapon", 228), new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), 203, -1, 247, 409, new int[0], new int[0], new int[0], new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>(), new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(194, 410, EMapPickupsType.Item, EMapPickupsType2.Weapon, "map_eventicon_13", 358, 0, new byte[15]
		{
			0, 0, 1, 0, 0, 0, 0, 0, 0, 0,
			0, 0, 0, 0, 0
		}, new List<short>
		{
			57, 58, 59, 60, 61, 62, 63, 64, 65, 66,
			67, 68, 69, 70, 71, 72, 73, 74, 75, 76,
			77, 78, 79, 80, 81, 82, 83, 84, 85, 86,
			87, 88, 89, 90, 91, 92, 93, 118, 119, 120,
			121, 122, 123, 124
		}, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new PresetItemTemplateId("Weapon", 237), new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), 203, -1, 247, 411, new int[0], new int[0], new int[0], new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>(), new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(195, 412, EMapPickupsType.Item, EMapPickupsType2.Weapon, "map_eventicon_13", 358, 0, new byte[15]
		{
			0, 0, 1, 0, 0, 0, 0, 0, 0, 0,
			0, 0, 0, 0, 0
		}, new List<short>
		{
			57, 58, 59, 60, 61, 62, 63, 64, 65, 66,
			67, 68, 69, 70, 71, 72, 73, 74, 75, 76,
			77, 78, 79, 80, 81, 82, 83, 84, 85, 86,
			87, 88, 89, 90, 91, 92, 93, 118, 119, 120,
			121, 122, 123, 124
		}, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new PresetItemTemplateId("Weapon", 246), new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), 203, -1, 247, 413, new int[0], new int[0], new int[0], new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>(), new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(196, 414, EMapPickupsType.Item, EMapPickupsType2.Weapon, "map_eventicon_13", 358, 0, new byte[15]
		{
			0, 0, 1, 0, 0, 0, 0, 0, 0, 0,
			0, 0, 0, 0, 0
		}, new List<short>
		{
			57, 58, 59, 60, 61, 62, 63, 64, 65, 66,
			67, 68, 69, 70, 71, 72, 73, 74, 75, 76,
			77, 78, 79, 80, 81, 82, 83, 84, 85, 86,
			87, 88, 89, 90, 91, 92, 93, 118, 119, 120,
			121, 122, 123, 124
		}, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new PresetItemTemplateId("Weapon", 255), new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), 203, -1, 247, 415, new int[0], new int[0], new int[0], new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>(), new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(197, 416, EMapPickupsType.Item, EMapPickupsType2.Weapon, "map_eventicon_13", 358, 0, new byte[15]
		{
			0, 0, 1, 0, 0, 0, 0, 0, 0, 0,
			0, 0, 0, 0, 0
		}, new List<short>
		{
			57, 58, 59, 60, 61, 62, 63, 64, 65, 66,
			67, 68, 69, 70, 71, 72, 73, 74, 75, 76,
			77, 78, 79, 80, 81, 82, 83, 84, 85, 86,
			87, 88, 89, 90, 91, 92, 93, 118, 119, 120,
			121, 122, 123, 124
		}, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new PresetItemTemplateId("Weapon", 264), new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), 203, -1, 247, 417, new int[0], new int[0], new int[0], new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>(), new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(198, 418, EMapPickupsType.Item, EMapPickupsType2.Weapon, "map_eventicon_13", 358, 0, new byte[15]
		{
			1, 1, 0, 1, 0, 1, 0, 0, 0, 1,
			1, 1, 0, 1, 1
		}, new List<short>
		{
			57, 58, 59, 60, 61, 62, 63, 64, 65, 66,
			67, 68, 69, 70, 71, 72, 73, 74, 75, 76,
			77, 78, 79, 80, 81, 82, 83, 84, 85, 86,
			87, 88, 89, 90, 91, 92, 93, 118, 119, 120,
			121, 122, 123, 124
		}, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new PresetItemTemplateId("Weapon", 273), new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), 204, -1, 247, 419, new int[0], new int[0], new int[0], new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>(), new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(199, 420, EMapPickupsType.Item, EMapPickupsType2.Weapon, "map_eventicon_13", 358, 0, new byte[15]
		{
			1, 1, 0, 1, 0, 1, 0, 0, 0, 1,
			1, 1, 0, 1, 1
		}, new List<short>
		{
			57, 58, 59, 60, 61, 62, 63, 64, 65, 66,
			67, 68, 69, 70, 71, 72, 73, 74, 75, 76,
			77, 78, 79, 80, 81, 82, 83, 84, 85, 86,
			87, 88, 89, 90, 91, 92, 93, 118, 119, 120,
			121, 122, 123, 124
		}, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new PresetItemTemplateId("Weapon", 282), new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), 204, -1, 247, 421, new int[0], new int[0], new int[0], new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>(), new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(200, 422, EMapPickupsType.Item, EMapPickupsType2.Weapon, "map_eventicon_13", 358, 0, new byte[15]
		{
			1, 1, 0, 1, 0, 1, 0, 0, 0, 1,
			1, 1, 0, 1, 1
		}, new List<short>
		{
			57, 58, 59, 60, 61, 62, 63, 64, 65, 66,
			67, 68, 69, 70, 71, 72, 73, 74, 75, 76,
			77, 78, 79, 80, 81, 82, 83, 84, 85, 86,
			87, 88, 89, 90, 91, 92, 93, 118, 119, 120,
			121, 122, 123, 124
		}, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new PresetItemTemplateId("Weapon", 291), new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), 204, -1, 247, 423, new int[0], new int[0], new int[0], new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>(), new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(201, 424, EMapPickupsType.Item, EMapPickupsType2.Weapon, "map_eventicon_13", 358, 0, new byte[15]
		{
			1, 1, 0, 1, 0, 1, 0, 0, 0, 1,
			1, 1, 0, 1, 1
		}, new List<short>
		{
			57, 58, 59, 60, 61, 62, 63, 64, 65, 66,
			67, 68, 69, 70, 71, 72, 73, 74, 75, 76,
			77, 78, 79, 80, 81, 82, 83, 84, 85, 86,
			87, 88, 89, 90, 91, 92, 93, 118, 119, 120,
			121, 122, 123, 124
		}, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new PresetItemTemplateId("Weapon", 300), new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), 204, -1, 247, 425, new int[0], new int[0], new int[0], new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>(), new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(202, 426, EMapPickupsType.Item, EMapPickupsType2.Weapon, "map_eventicon_13", 358, 0, new byte[15]
		{
			0, 0, 1, 0, 0, 1, 1, 1, 0, 1,
			1, 1, 1, 0, 0
		}, new List<short>
		{
			57, 58, 59, 60, 61, 62, 63, 64, 65, 66,
			67, 68, 69, 70, 71, 72, 73, 74, 75, 76,
			77, 78, 79, 80, 81, 82, 83, 84, 85, 86,
			87, 88, 89, 90, 91, 92, 93, 118, 119, 120,
			121, 122, 123, 124
		}, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new PresetItemTemplateId("Weapon", 309), new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), 204, -1, 247, 427, new int[0], new int[0], new int[0], new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>(), new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(203, 428, EMapPickupsType.Item, EMapPickupsType2.Weapon, "map_eventicon_13", 358, 0, new byte[15]
		{
			0, 0, 1, 0, 0, 1, 1, 1, 0, 1,
			1, 1, 1, 0, 0
		}, new List<short>
		{
			57, 58, 59, 60, 61, 62, 63, 64, 65, 66,
			67, 68, 69, 70, 71, 72, 73, 74, 75, 76,
			77, 78, 79, 80, 81, 82, 83, 84, 85, 86,
			87, 88, 89, 90, 91, 92, 93, 118, 119, 120,
			121, 122, 123, 124
		}, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new PresetItemTemplateId("Weapon", 318), new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), 204, -1, 247, 429, new int[0], new int[0], new int[0], new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>(), new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(204, 430, EMapPickupsType.Item, EMapPickupsType2.Weapon, "map_eventicon_13", 358, 0, new byte[15]
		{
			1, 1, 1, 1, 0, 0, 1, 1, 0, 1,
			0, 1, 1, 0, 0
		}, new List<short>
		{
			57, 58, 59, 60, 61, 62, 63, 64, 65, 66,
			67, 68, 69, 70, 71, 72, 73, 74, 75, 76,
			77, 78, 79, 80, 81, 82, 83, 84, 85, 86,
			87, 88, 89, 90, 91, 92, 93, 118, 119, 120,
			121, 122, 123, 124
		}, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new PresetItemTemplateId("Weapon", 327), new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), 204, -1, 247, 431, new int[0], new int[0], new int[0], new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>(), new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(205, 432, EMapPickupsType.Item, EMapPickupsType2.Weapon, "map_eventicon_13", 358, 0, new byte[15]
		{
			1, 1, 1, 1, 0, 0, 1, 1, 0, 1,
			0, 1, 1, 0, 0
		}, new List<short>
		{
			57, 58, 59, 60, 61, 62, 63, 64, 65, 66,
			67, 68, 69, 70, 71, 72, 73, 74, 75, 76,
			77, 78, 79, 80, 81, 82, 83, 84, 85, 86,
			87, 88, 89, 90, 91, 92, 93, 118, 119, 120,
			121, 122, 123, 124
		}, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new PresetItemTemplateId("Weapon", 336), new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), 204, -1, 247, 433, new int[0], new int[0], new int[0], new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>(), new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(206, 434, EMapPickupsType.Item, EMapPickupsType2.Weapon, "map_eventicon_13", 358, 0, new byte[15]
		{
			1, 1, 1, 1, 0, 0, 1, 1, 0, 1,
			0, 1, 0, 0, 1
		}, new List<short>
		{
			57, 58, 59, 60, 61, 62, 63, 64, 65, 66,
			67, 68, 69, 70, 71, 72, 73, 74, 75, 76,
			77, 78, 79, 80, 81, 82, 83, 84, 85, 86,
			87, 88, 89, 90, 91, 92, 93, 118, 119, 120,
			121, 122, 123, 124
		}, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new PresetItemTemplateId("Weapon", 345), new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), 205, -1, 247, 435, new int[0], new int[0], new int[0], new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>(), new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(207, 436, EMapPickupsType.Item, EMapPickupsType2.Weapon, "map_eventicon_13", 358, 0, new byte[15]
		{
			1, 1, 1, 1, 0, 0, 1, 1, 0, 1,
			0, 1, 0, 0, 1
		}, new List<short>
		{
			57, 58, 59, 60, 61, 62, 63, 64, 65, 66,
			67, 68, 69, 70, 71, 72, 73, 74, 75, 76,
			77, 78, 79, 80, 81, 82, 83, 84, 85, 86,
			87, 88, 89, 90, 91, 92, 93, 118, 119, 120,
			121, 122, 123, 124
		}, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new PresetItemTemplateId("Weapon", 354), new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), 205, -1, 247, 437, new int[0], new int[0], new int[0], new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>(), new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(208, 438, EMapPickupsType.Item, EMapPickupsType2.Weapon, "map_eventicon_13", 358, 0, new byte[15]
		{
			0, 1, 1, 1, 0, 0, 1, 1, 0, 1,
			0, 1, 1, 0, 0
		}, new List<short>
		{
			57, 58, 59, 60, 61, 62, 63, 64, 65, 66,
			67, 68, 69, 70, 71, 72, 73, 74, 75, 76,
			77, 78, 79, 80, 81, 82, 83, 84, 85, 86,
			87, 88, 89, 90, 91, 92, 93, 118, 119, 120,
			121, 122, 123, 124
		}, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new PresetItemTemplateId("Weapon", 363), new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), 205, -1, 247, 439, new int[0], new int[0], new int[0], new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>(), new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(209, 440, EMapPickupsType.Item, EMapPickupsType2.Weapon, "map_eventicon_13", 358, 0, new byte[15]
		{
			0, 1, 1, 1, 0, 0, 1, 1, 0, 1,
			0, 1, 1, 0, 0
		}, new List<short>
		{
			57, 58, 59, 60, 61, 62, 63, 64, 65, 66,
			67, 68, 69, 70, 71, 72, 73, 74, 75, 76,
			77, 78, 79, 80, 81, 82, 83, 84, 85, 86,
			87, 88, 89, 90, 91, 92, 93, 118, 119, 120,
			121, 122, 123, 124
		}, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new PresetItemTemplateId("Weapon", 372), new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), 205, -1, 247, 441, new int[0], new int[0], new int[0], new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>(), new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(210, 442, EMapPickupsType.Item, EMapPickupsType2.Weapon, "map_eventicon_13", 358, 0, new byte[15]
		{
			0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
			1, 0, 0, 0, 0
		}, new List<short>
		{
			57, 58, 59, 60, 61, 62, 63, 64, 65, 66,
			67, 68, 69, 70, 71, 72, 73, 74, 75, 76,
			77, 78, 79, 80, 81, 82, 83, 84, 85, 86,
			87, 88, 89, 90, 91, 92, 93, 118, 119, 120,
			121, 122, 123, 124
		}, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new PresetItemTemplateId("Weapon", 381), new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), 206, -1, 247, 443, new int[0], new int[0], new int[0], new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>(), new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(211, 444, EMapPickupsType.Item, EMapPickupsType2.Weapon, "map_eventicon_13", 358, 0, new byte[15]
		{
			0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
			1, 0, 0, 0, 0
		}, new List<short>
		{
			57, 58, 59, 60, 61, 62, 63, 64, 65, 66,
			67, 68, 69, 70, 71, 72, 73, 74, 75, 76,
			77, 78, 79, 80, 81, 82, 83, 84, 85, 86,
			87, 88, 89, 90, 91, 92, 93, 118, 119, 120,
			121, 122, 123, 124
		}, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new PresetItemTemplateId("Weapon", 390), new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), 206, -1, 247, 445, new int[0], new int[0], new int[0], new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>(), new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(212, 446, EMapPickupsType.Item, EMapPickupsType2.Weapon, "map_eventicon_13", 358, 0, new byte[15]
		{
			0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
			1, 0, 0, 0, 0
		}, new List<short>
		{
			57, 58, 59, 60, 61, 62, 63, 64, 65, 66,
			67, 68, 69, 70, 71, 72, 73, 74, 75, 76,
			77, 78, 79, 80, 81, 82, 83, 84, 85, 86,
			87, 88, 89, 90, 91, 92, 93, 118, 119, 120,
			121, 122, 123, 124
		}, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new PresetItemTemplateId("Weapon", 399), new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), 206, -1, 247, 447, new int[0], new int[0], new int[0], new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>(), new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(213, 448, EMapPickupsType.Item, EMapPickupsType2.Weapon, "map_eventicon_13", 358, 0, new byte[15]
		{
			0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
			1, 0, 0, 0, 0
		}, new List<short>
		{
			57, 58, 59, 60, 61, 62, 63, 64, 65, 66,
			67, 68, 69, 70, 71, 72, 73, 74, 75, 76,
			77, 78, 79, 80, 81, 82, 83, 84, 85, 86,
			87, 88, 89, 90, 91, 92, 93, 118, 119, 120,
			121, 122, 123, 124
		}, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new PresetItemTemplateId("Weapon", 408), new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), 206, -1, 247, 449, new int[0], new int[0], new int[0], new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>(), new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(214, 450, EMapPickupsType.Item, EMapPickupsType2.Weapon, "map_eventicon_13", 358, 0, new byte[15]
		{
			0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
			1, 0, 0, 0, 0
		}, new List<short>
		{
			57, 58, 59, 60, 61, 62, 63, 64, 65, 66,
			67, 68, 69, 70, 71, 72, 73, 74, 75, 76,
			77, 78, 79, 80, 81, 82, 83, 84, 85, 86,
			87, 88, 89, 90, 91, 92, 93, 118, 119, 120,
			121, 122, 123, 124
		}, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new PresetItemTemplateId("Weapon", 417), new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), 206, -1, 247, 451, new int[0], new int[0], new int[0], new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>(), new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(215, 452, EMapPickupsType.Item, EMapPickupsType2.Weapon, "map_eventicon_13", 358, 0, new byte[15]
		{
			0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
			1, 0, 0, 0, 0
		}, new List<short>
		{
			57, 58, 59, 60, 61, 62, 63, 64, 65, 66,
			67, 68, 69, 70, 71, 72, 73, 74, 75, 76,
			77, 78, 79, 80, 81, 82, 83, 84, 85, 86,
			87, 88, 89, 90, 91, 92, 93, 118, 119, 120,
			121, 122, 123, 124
		}, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new PresetItemTemplateId("Weapon", 426), new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), 206, -1, 247, 453, new int[0], new int[0], new int[0], new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>(), new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(216, 454, EMapPickupsType.Item, EMapPickupsType2.Weapon, "map_eventicon_13", 358, 0, new byte[15]
		{
			0, 1, 0, 1, 1, 0, 0, 0, 1, 0,
			0, 1, 1, 0, 0
		}, new List<short>
		{
			57, 58, 59, 60, 61, 62, 63, 64, 65, 66,
			67, 68, 69, 70, 71, 72, 73, 74, 75, 76,
			77, 78, 79, 80, 81, 82, 83, 84, 85, 86,
			87, 88, 89, 90, 91, 92, 93, 118, 119, 120,
			121, 122, 123, 124
		}, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new PresetItemTemplateId("Weapon", 435), new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), 207, -1, 247, 455, new int[0], new int[0], new int[0], new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>(), new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(217, 456, EMapPickupsType.Item, EMapPickupsType2.Weapon, "map_eventicon_13", 358, 0, new byte[15]
		{
			0, 1, 0, 1, 1, 0, 0, 0, 1, 0,
			0, 1, 1, 0, 0
		}, new List<short>
		{
			57, 58, 59, 60, 61, 62, 63, 64, 65, 66,
			67, 68, 69, 70, 71, 72, 73, 74, 75, 76,
			77, 78, 79, 80, 81, 82, 83, 84, 85, 86,
			87, 88, 89, 90, 91, 92, 93, 118, 119, 120,
			121, 122, 123, 124
		}, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new PresetItemTemplateId("Weapon", 444), new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), 207, -1, 247, 457, new int[0], new int[0], new int[0], new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>(), new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(218, 458, EMapPickupsType.Item, EMapPickupsType2.Weapon, "map_eventicon_13", 358, 0, new byte[15]
		{
			0, 1, 0, 1, 1, 0, 0, 0, 1, 0,
			0, 1, 1, 0, 0
		}, new List<short>
		{
			57, 58, 59, 60, 61, 62, 63, 64, 65, 66,
			67, 68, 69, 70, 71, 72, 73, 74, 75, 76,
			77, 78, 79, 80, 81, 82, 83, 84, 85, 86,
			87, 88, 89, 90, 91, 92, 93, 118, 119, 120,
			121, 122, 123, 124
		}, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new PresetItemTemplateId("Weapon", 453), new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), 207, -1, 247, 459, new int[0], new int[0], new int[0], new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>(), new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(219, 460, EMapPickupsType.Item, EMapPickupsType2.Weapon, "map_eventicon_13", 358, 0, new byte[15]
		{
			0, 1, 0, 1, 1, 0, 0, 0, 1, 0,
			0, 1, 1, 0, 0
		}, new List<short>
		{
			57, 58, 59, 60, 61, 62, 63, 64, 65, 66,
			67, 68, 69, 70, 71, 72, 73, 74, 75, 76,
			77, 78, 79, 80, 81, 82, 83, 84, 85, 86,
			87, 88, 89, 90, 91, 92, 93, 118, 119, 120,
			121, 122, 123, 124
		}, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new PresetItemTemplateId("Weapon", 462), new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), 207, -1, 247, 461, new int[0], new int[0], new int[0], new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>(), new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(220, 462, EMapPickupsType.Item, EMapPickupsType2.Weapon, "map_eventicon_13", 358, 0, new byte[15]
		{
			0, 1, 0, 1, 1, 0, 0, 0, 1, 0,
			0, 1, 1, 0, 0
		}, new List<short>
		{
			57, 58, 59, 60, 61, 62, 63, 64, 65, 66,
			67, 68, 69, 70, 71, 72, 73, 74, 75, 76,
			77, 78, 79, 80, 81, 82, 83, 84, 85, 86,
			87, 88, 89, 90, 91, 92, 93, 118, 119, 120,
			121, 122, 123, 124
		}, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new PresetItemTemplateId("Weapon", 471), new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), 207, -1, 247, 463, new int[0], new int[0], new int[0], new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>(), new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(221, 464, EMapPickupsType.Item, EMapPickupsType2.Weapon, "map_eventicon_13", 358, 0, new byte[15]
		{
			0, 1, 0, 1, 1, 0, 0, 0, 1, 0,
			0, 1, 1, 0, 0
		}, new List<short>
		{
			57, 58, 59, 60, 61, 62, 63, 64, 65, 66,
			67, 68, 69, 70, 71, 72, 73, 74, 75, 76,
			77, 78, 79, 80, 81, 82, 83, 84, 85, 86,
			87, 88, 89, 90, 91, 92, 93, 118, 119, 120,
			121, 122, 123, 124
		}, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new PresetItemTemplateId("Weapon", 480), new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), 207, -1, 247, 465, new int[0], new int[0], new int[0], new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>(), new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(222, 466, EMapPickupsType.Item, EMapPickupsType2.Weapon, "map_eventicon_13", 358, 0, new byte[15]
		{
			0, 1, 0, 1, 1, 0, 1, 0, 1, 0,
			0, 1, 0, 0, 0
		}, new List<short>
		{
			57, 58, 59, 60, 61, 62, 63, 64, 65, 66,
			67, 68, 69, 70, 71, 72, 73, 74, 75, 76,
			77, 78, 79, 80, 81, 82, 83, 84, 85, 86,
			87, 88, 89, 90, 91, 92, 93, 118, 119, 120,
			121, 122, 123, 124
		}, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new PresetItemTemplateId("Weapon", 489), new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), 207, -1, 247, 467, new int[0], new int[0], new int[0], new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>(), new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(223, 468, EMapPickupsType.Item, EMapPickupsType2.Weapon, "map_eventicon_13", 358, 0, new byte[15]
		{
			0, 1, 0, 1, 1, 0, 1, 0, 1, 0,
			0, 1, 0, 0, 0
		}, new List<short>
		{
			57, 58, 59, 60, 61, 62, 63, 64, 65, 66,
			67, 68, 69, 70, 71, 72, 73, 74, 75, 76,
			77, 78, 79, 80, 81, 82, 83, 84, 85, 86,
			87, 88, 89, 90, 91, 92, 93, 118, 119, 120,
			121, 122, 123, 124
		}, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new PresetItemTemplateId("Weapon", 498), new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), 207, -1, 247, 469, new int[0], new int[0], new int[0], new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>(), new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(224, 470, EMapPickupsType.Item, EMapPickupsType2.Weapon, "map_eventicon_13", 358, 0, new byte[15]
		{
			0, 0, 0, 1, 0, 0, 1, 0, 1, 0,
			0, 1, 0, 0, 0
		}, new List<short>
		{
			57, 58, 59, 60, 61, 62, 63, 64, 65, 66,
			67, 68, 69, 70, 71, 72, 73, 74, 75, 76,
			77, 78, 79, 80, 81, 82, 83, 84, 85, 86,
			87, 88, 89, 90, 91, 92, 93, 118, 119, 120,
			121, 122, 123, 124
		}, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new PresetItemTemplateId("Weapon", 507), new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), 207, -1, 247, 471, new int[0], new int[0], new int[0], new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>(), new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(225, 472, EMapPickupsType.Item, EMapPickupsType2.Weapon, "map_eventicon_13", 358, 0, new byte[15]
		{
			0, 0, 0, 1, 0, 0, 1, 0, 1, 0,
			0, 1, 0, 0, 0
		}, new List<short>
		{
			57, 58, 59, 60, 61, 62, 63, 64, 65, 66,
			67, 68, 69, 70, 71, 72, 73, 74, 75, 76,
			77, 78, 79, 80, 81, 82, 83, 84, 85, 86,
			87, 88, 89, 90, 91, 92, 93, 118, 119, 120,
			121, 122, 123, 124
		}, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new PresetItemTemplateId("Weapon", 516), new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), 207, -1, 247, 473, new int[0], new int[0], new int[0], new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>(), new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(226, 474, EMapPickupsType.Item, EMapPickupsType2.Weapon, "map_eventicon_13", 358, 0, new byte[15]
		{
			0, 0, 0, 0, 1, 1, 0, 0, 1, 0,
			1, 0, 0, 1, 0
		}, new List<short>
		{
			57, 58, 59, 60, 61, 62, 63, 64, 65, 66,
			67, 68, 69, 70, 71, 72, 73, 74, 75, 76,
			77, 78, 79, 80, 81, 82, 83, 84, 85, 86,
			87, 88, 89, 90, 91, 92, 93, 118, 119, 120,
			121, 122, 123, 124
		}, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new PresetItemTemplateId("Weapon", 525), new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), 208, -1, 247, 475, new int[0], new int[0], new int[0], new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>(), new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(227, 476, EMapPickupsType.Item, EMapPickupsType2.Weapon, "map_eventicon_13", 358, 0, new byte[15]
		{
			0, 0, 0, 0, 1, 1, 0, 0, 1, 0,
			1, 0, 0, 1, 0
		}, new List<short>
		{
			57, 58, 59, 60, 61, 62, 63, 64, 65, 66,
			67, 68, 69, 70, 71, 72, 73, 74, 75, 76,
			77, 78, 79, 80, 81, 82, 83, 84, 85, 86,
			87, 88, 89, 90, 91, 92, 93, 118, 119, 120,
			121, 122, 123, 124
		}, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new PresetItemTemplateId("Weapon", 534), new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), 208, -1, 247, 477, new int[0], new int[0], new int[0], new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>(), new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(228, 478, EMapPickupsType.Item, EMapPickupsType2.Weapon, "map_eventicon_13", 358, 0, new byte[15]
		{
			0, 0, 0, 0, 1, 1, 0, 0, 1, 0,
			1, 0, 0, 1, 0
		}, new List<short>
		{
			57, 58, 59, 60, 61, 62, 63, 64, 65, 66,
			67, 68, 69, 70, 71, 72, 73, 74, 75, 76,
			77, 78, 79, 80, 81, 82, 83, 84, 85, 86,
			87, 88, 89, 90, 91, 92, 93, 118, 119, 120,
			121, 122, 123, 124
		}, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new PresetItemTemplateId("Weapon", 543), new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), 208, -1, 247, 479, new int[0], new int[0], new int[0], new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>(), new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(229, 480, EMapPickupsType.Item, EMapPickupsType2.Weapon, "map_eventicon_13", 358, 0, new byte[15]
		{
			0, 0, 0, 0, 1, 1, 0, 0, 1, 0,
			1, 0, 0, 1, 0
		}, new List<short>
		{
			57, 58, 59, 60, 61, 62, 63, 64, 65, 66,
			67, 68, 69, 70, 71, 72, 73, 74, 75, 76,
			77, 78, 79, 80, 81, 82, 83, 84, 85, 86,
			87, 88, 89, 90, 91, 92, 93, 118, 119, 120,
			121, 122, 123, 124
		}, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new PresetItemTemplateId("Weapon", 552), new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), 208, -1, 247, 481, new int[0], new int[0], new int[0], new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>(), new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(230, 482, EMapPickupsType.Item, EMapPickupsType2.Weapon, "map_eventicon_13", 358, 0, new byte[15]
		{
			0, 0, 0, 0, 1, 1, 0, 0, 1, 0,
			1, 0, 0, 1, 0
		}, new List<short>
		{
			57, 58, 59, 60, 61, 62, 63, 64, 65, 66,
			67, 68, 69, 70, 71, 72, 73, 74, 75, 76,
			77, 78, 79, 80, 81, 82, 83, 84, 85, 86,
			87, 88, 89, 90, 91, 92, 93, 118, 119, 120,
			121, 122, 123, 124
		}, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new PresetItemTemplateId("Weapon", 561), new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), 208, -1, 247, 483, new int[0], new int[0], new int[0], new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>(), new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(231, 484, EMapPickupsType.Item, EMapPickupsType2.Weapon, "map_eventicon_13", 358, 0, new byte[15]
		{
			0, 0, 0, 0, 1, 1, 0, 0, 1, 0,
			1, 0, 0, 1, 0
		}, new List<short>
		{
			57, 58, 59, 60, 61, 62, 63, 64, 65, 66,
			67, 68, 69, 70, 71, 72, 73, 74, 75, 76,
			77, 78, 79, 80, 81, 82, 83, 84, 85, 86,
			87, 88, 89, 90, 91, 92, 93, 118, 119, 120,
			121, 122, 123, 124
		}, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new PresetItemTemplateId("Weapon", 570), new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), 208, -1, 247, 485, new int[0], new int[0], new int[0], new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>(), new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(232, 486, EMapPickupsType.Item, EMapPickupsType2.Weapon, "map_eventicon_13", 358, 0, new byte[15]
		{
			0, 0, 0, 0, 1, 1, 0, 0, 1, 0,
			1, 0, 0, 0, 0
		}, new List<short>
		{
			57, 58, 59, 60, 61, 62, 63, 64, 65, 66,
			67, 68, 69, 70, 71, 72, 73, 74, 75, 76,
			77, 78, 79, 80, 81, 82, 83, 84, 85, 86,
			87, 88, 89, 90, 91, 92, 93, 118, 119, 120,
			121, 122, 123, 124
		}, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new PresetItemTemplateId("Weapon", 579), new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), 208, -1, 247, 487, new int[0], new int[0], new int[0], new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>(), new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(233, 488, EMapPickupsType.Item, EMapPickupsType2.Weapon, "map_eventicon_13", 358, 0, new byte[15]
		{
			0, 0, 0, 0, 1, 1, 0, 0, 1, 0,
			1, 0, 0, 0, 0
		}, new List<short>
		{
			57, 58, 59, 60, 61, 62, 63, 64, 65, 66,
			67, 68, 69, 70, 71, 72, 73, 74, 75, 76,
			77, 78, 79, 80, 81, 82, 83, 84, 85, 86,
			87, 88, 89, 90, 91, 92, 93, 118, 119, 120,
			121, 122, 123, 124
		}, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new PresetItemTemplateId("Weapon", 588), new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), 208, -1, 247, 489, new int[0], new int[0], new int[0], new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>(), new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(234, 490, EMapPickupsType.Item, EMapPickupsType2.Weapon, "map_eventicon_13", 358, 0, new byte[15]
		{
			0, 0, 0, 0, 1, 0, 0, 0, 1, 0,
			0, 0, 0, 0, 0
		}, new List<short>
		{
			57, 58, 59, 60, 61, 62, 63, 64, 65, 66,
			67, 68, 69, 70, 71, 72, 73, 74, 75, 76,
			77, 78, 79, 80, 81, 82, 83, 84, 85, 86,
			87, 88, 89, 90, 91, 92, 93, 118, 119, 120,
			121, 122, 123, 124
		}, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new PresetItemTemplateId("Weapon", 597), new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), 208, -1, 247, 491, new int[0], new int[0], new int[0], new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>(), new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(235, 492, EMapPickupsType.Item, EMapPickupsType2.Weapon, "map_eventicon_13", 358, 0, new byte[15]
		{
			0, 0, 0, 0, 1, 0, 0, 0, 1, 0,
			0, 0, 0, 0, 0
		}, new List<short>
		{
			57, 58, 59, 60, 61, 62, 63, 64, 65, 66,
			67, 68, 69, 70, 71, 72, 73, 74, 75, 76,
			77, 78, 79, 80, 81, 82, 83, 84, 85, 86,
			87, 88, 89, 90, 91, 92, 93, 118, 119, 120,
			121, 122, 123, 124
		}, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new PresetItemTemplateId("Weapon", 606), new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), 208, -1, 247, 493, new int[0], new int[0], new int[0], new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>(), new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(236, 494, EMapPickupsType.Item, EMapPickupsType2.Weapon, "map_eventicon_13", 358, 0, new byte[15]
		{
			1, 0, 0, 0, 0, 0, 0, 0, 1, 0,
			0, 0, 0, 0, 0
		}, new List<short>
		{
			57, 58, 59, 60, 61, 62, 63, 64, 65, 66,
			67, 68, 69, 70, 71, 72, 73, 74, 75, 76,
			77, 78, 79, 80, 81, 82, 83, 84, 85, 86,
			87, 88, 89, 90, 91, 92, 93, 118, 119, 120,
			121, 122, 123, 124
		}, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new PresetItemTemplateId("Weapon", 615), new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), 209, -1, 247, 495, new int[0], new int[0], new int[0], new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>(), new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(237, 496, EMapPickupsType.Item, EMapPickupsType2.Weapon, "map_eventicon_13", 358, 0, new byte[15]
		{
			0, 0, 0, 0, 0, 1, 0, 0, 1, 0,
			0, 0, 0, 0, 0
		}, new List<short>
		{
			57, 58, 59, 60, 61, 62, 63, 64, 65, 66,
			67, 68, 69, 70, 71, 72, 73, 74, 75, 76,
			77, 78, 79, 80, 81, 82, 83, 84, 85, 86,
			87, 88, 89, 90, 91, 92, 93, 118, 119, 120,
			121, 122, 123, 124
		}, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new PresetItemTemplateId("Weapon", 624), new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), 209, -1, 247, 497, new int[0], new int[0], new int[0], new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>(), new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(238, 498, EMapPickupsType.Item, EMapPickupsType2.Weapon, "map_eventicon_13", 358, 0, new byte[15]
		{
			1, 0, 0, 0, 0, 0, 0, 0, 1, 0,
			0, 0, 0, 0, 0
		}, new List<short>
		{
			57, 58, 59, 60, 61, 62, 63, 64, 65, 66,
			67, 68, 69, 70, 71, 72, 73, 74, 75, 76,
			77, 78, 79, 80, 81, 82, 83, 84, 85, 86,
			87, 88, 89, 90, 91, 92, 93, 118, 119, 120,
			121, 122, 123, 124
		}, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new PresetItemTemplateId("Weapon", 633), new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), 209, -1, 247, 499, new int[0], new int[0], new int[0], new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>(), new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(239, 500, EMapPickupsType.Item, EMapPickupsType2.Weapon, "map_eventicon_13", 358, 0, new byte[15]
		{
			0, 0, 0, 0, 0, 1, 0, 0, 1, 0,
			0, 0, 0, 0, 0
		}, new List<short>
		{
			57, 58, 59, 60, 61, 62, 63, 64, 65, 66,
			67, 68, 69, 70, 71, 72, 73, 74, 75, 76,
			77, 78, 79, 80, 81, 82, 83, 84, 85, 86,
			87, 88, 89, 90, 91, 92, 93, 118, 119, 120,
			121, 122, 123, 124
		}, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new PresetItemTemplateId("Weapon", 642), new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), 209, -1, 247, 501, new int[0], new int[0], new int[0], new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>(), new List<PresetItemWithCount>()));
	}

	private void CreateItems4()
	{
		_dataArray.Add(new MapPickupsItem(240, 502, EMapPickupsType.Item, EMapPickupsType2.Weapon, "map_eventicon_13", 358, 0, new byte[15]
		{
			1, 0, 0, 0, 0, 1, 0, 0, 1, 0,
			0, 0, 0, 0, 0
		}, new List<short>
		{
			57, 58, 59, 60, 61, 62, 63, 64, 65, 66,
			67, 68, 69, 70, 71, 72, 73, 74, 75, 76,
			77, 78, 79, 80, 81, 82, 83, 84, 85, 86,
			87, 88, 89, 90, 91, 92, 93, 118, 119, 120,
			121, 122, 123, 124
		}, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new PresetItemTemplateId("Weapon", 651), new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), 209, -1, 247, 503, new int[0], new int[0], new int[0], new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>(), new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(241, 504, EMapPickupsType.Item, EMapPickupsType2.Weapon, "map_eventicon_13", 358, 0, new byte[15]
		{
			1, 0, 0, 0, 0, 1, 0, 0, 1, 0,
			0, 0, 0, 0, 0
		}, new List<short>
		{
			57, 58, 59, 60, 61, 62, 63, 64, 65, 66,
			67, 68, 69, 70, 71, 72, 73, 74, 75, 76,
			77, 78, 79, 80, 81, 82, 83, 84, 85, 86,
			87, 88, 89, 90, 91, 92, 93, 118, 119, 120,
			121, 122, 123, 124
		}, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new PresetItemTemplateId("Weapon", 660), new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), 209, -1, 247, 505, new int[0], new int[0], new int[0], new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>(), new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(242, 506, EMapPickupsType.Item, EMapPickupsType2.Weapon, "map_eventicon_13", 358, 0, new byte[15]
		{
			1, 0, 0, 0, 0, 0, 0, 0, 1, 0,
			0, 0, 0, 0, 0
		}, new List<short>
		{
			57, 58, 59, 60, 61, 62, 63, 64, 65, 66,
			67, 68, 69, 70, 71, 72, 73, 74, 75, 76,
			77, 78, 79, 80, 81, 82, 83, 84, 85, 86,
			87, 88, 89, 90, 91, 92, 93, 118, 119, 120,
			121, 122, 123, 124
		}, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new PresetItemTemplateId("Weapon", 669), new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), 209, -1, 247, 507, new int[0], new int[0], new int[0], new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>(), new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(243, 508, EMapPickupsType.Item, EMapPickupsType2.Weapon, "map_eventicon_13", 358, 0, new byte[15]
		{
			1, 0, 0, 0, 0, 0, 0, 0, 1, 0,
			0, 0, 0, 0, 0
		}, new List<short>
		{
			57, 58, 59, 60, 61, 62, 63, 64, 65, 66,
			67, 68, 69, 70, 71, 72, 73, 74, 75, 76,
			77, 78, 79, 80, 81, 82, 83, 84, 85, 86,
			87, 88, 89, 90, 91, 92, 93, 118, 119, 120,
			121, 122, 123, 124
		}, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new PresetItemTemplateId("Weapon", 678), new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), 209, -1, 247, 509, new int[0], new int[0], new int[0], new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>(), new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(244, 510, EMapPickupsType.Item, EMapPickupsType2.Weapon, "map_eventicon_13", 358, 0, new byte[15]
		{
			1, 0, 0, 0, 0, 0, 0, 0, 1, 0,
			0, 0, 0, 0, 0
		}, new List<short>
		{
			57, 58, 59, 60, 61, 62, 63, 64, 65, 66,
			67, 68, 69, 70, 71, 72, 73, 74, 75, 76,
			77, 78, 79, 80, 81, 82, 83, 84, 85, 86,
			87, 88, 89, 90, 91, 92, 93, 118, 119, 120,
			121, 122, 123, 124
		}, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new PresetItemTemplateId("Weapon", 687), new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), 209, -1, 247, 511, new int[0], new int[0], new int[0], new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>(), new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(245, 512, EMapPickupsType.Item, EMapPickupsType2.Weapon, "map_eventicon_13", 358, 0, new byte[15]
		{
			1, 0, 0, 0, 0, 0, 0, 0, 1, 0,
			0, 0, 0, 0, 0
		}, new List<short>
		{
			57, 58, 59, 60, 61, 62, 63, 64, 65, 66,
			67, 68, 69, 70, 71, 72, 73, 74, 75, 76,
			77, 78, 79, 80, 81, 82, 83, 84, 85, 86,
			87, 88, 89, 90, 91, 92, 93, 118, 119, 120,
			121, 122, 123, 124
		}, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new PresetItemTemplateId("Weapon", 696), new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), 209, -1, 247, 513, new int[0], new int[0], new int[0], new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>(), new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(246, 514, EMapPickupsType.Item, EMapPickupsType2.Weapon, "map_eventicon_13", 358, 0, new byte[15]
		{
			0, 0, 0, 0, 0, 0, 0, 1, 0, 0,
			0, 0, 0, 0, 0
		}, new List<short>
		{
			57, 58, 59, 60, 61, 62, 63, 64, 65, 66,
			67, 68, 69, 70, 71, 72, 73, 74, 75, 76,
			77, 78, 79, 80, 81, 82, 83, 84, 85, 86,
			87, 88, 89, 90, 91, 92, 93, 118, 119, 120,
			121, 122, 123, 124
		}, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new PresetItemTemplateId("Weapon", 705), new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), 210, -1, 247, 515, new int[0], new int[0], new int[0], new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>(), new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(247, 516, EMapPickupsType.Item, EMapPickupsType2.Weapon, "map_eventicon_13", 358, 0, new byte[15]
		{
			0, 0, 0, 0, 0, 0, 0, 1, 0, 0,
			0, 0, 0, 0, 0
		}, new List<short>
		{
			57, 58, 59, 60, 61, 62, 63, 64, 65, 66,
			67, 68, 69, 70, 71, 72, 73, 74, 75, 76,
			77, 78, 79, 80, 81, 82, 83, 84, 85, 86,
			87, 88, 89, 90, 91, 92, 93, 118, 119, 120,
			121, 122, 123, 124
		}, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new PresetItemTemplateId("Weapon", 714), new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), 210, -1, 247, 517, new int[0], new int[0], new int[0], new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>(), new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(248, 518, EMapPickupsType.Item, EMapPickupsType2.Weapon, "map_eventicon_13", 358, 0, new byte[15]
		{
			0, 0, 0, 0, 0, 0, 0, 1, 0, 0,
			0, 0, 0, 0, 0
		}, new List<short>
		{
			57, 58, 59, 60, 61, 62, 63, 64, 65, 66,
			67, 68, 69, 70, 71, 72, 73, 74, 75, 76,
			77, 78, 79, 80, 81, 82, 83, 84, 85, 86,
			87, 88, 89, 90, 91, 92, 93, 118, 119, 120,
			121, 122, 123, 124
		}, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new PresetItemTemplateId("Weapon", 723), new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), 210, -1, 247, 519, new int[0], new int[0], new int[0], new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>(), new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(249, 520, EMapPickupsType.Item, EMapPickupsType2.Weapon, "map_eventicon_13", 358, 0, new byte[15]
		{
			0, 0, 0, 0, 0, 0, 0, 1, 0, 0,
			0, 0, 0, 0, 0
		}, new List<short>
		{
			57, 58, 59, 60, 61, 62, 63, 64, 65, 66,
			67, 68, 69, 70, 71, 72, 73, 74, 75, 76,
			77, 78, 79, 80, 81, 82, 83, 84, 85, 86,
			87, 88, 89, 90, 91, 92, 93, 118, 119, 120,
			121, 122, 123, 124
		}, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new PresetItemTemplateId("Weapon", 732), new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), 210, -1, 247, 521, new int[0], new int[0], new int[0], new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>(), new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(250, 522, EMapPickupsType.Item, EMapPickupsType2.Weapon, "map_eventicon_13", 358, 0, new byte[15]
		{
			0, 0, 0, 0, 0, 0, 0, 1, 0, 0,
			0, 0, 0, 0, 0
		}, new List<short>
		{
			57, 58, 59, 60, 61, 62, 63, 64, 65, 66,
			67, 68, 69, 70, 71, 72, 73, 74, 75, 76,
			77, 78, 79, 80, 81, 82, 83, 84, 85, 86,
			87, 88, 89, 90, 91, 92, 93, 118, 119, 120,
			121, 122, 123, 124
		}, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new PresetItemTemplateId("Weapon", 741), new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), 210, -1, 247, 523, new int[0], new int[0], new int[0], new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>(), new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(251, 524, EMapPickupsType.Item, EMapPickupsType2.Weapon, "map_eventicon_13", 358, 0, new byte[15]
		{
			0, 0, 0, 0, 0, 0, 0, 1, 0, 0,
			0, 0, 0, 0, 0
		}, new List<short>
		{
			57, 58, 59, 60, 61, 62, 63, 64, 65, 66,
			67, 68, 69, 70, 71, 72, 73, 74, 75, 76,
			77, 78, 79, 80, 81, 82, 83, 84, 85, 86,
			87, 88, 89, 90, 91, 92, 93, 118, 119, 120,
			121, 122, 123, 124
		}, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new PresetItemTemplateId("Weapon", 750), new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), 210, -1, 247, 525, new int[0], new int[0], new int[0], new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>(), new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(252, 526, EMapPickupsType.Item, EMapPickupsType2.Weapon, "map_eventicon_13", 358, 0, new byte[15]
		{
			0, 0, 0, 1, 0, 0, 0, 0, 0, 0,
			0, 0, 0, 0, 0
		}, new List<short>
		{
			57, 58, 59, 60, 61, 62, 63, 64, 65, 66,
			67, 68, 69, 70, 71, 72, 73, 74, 75, 76,
			77, 78, 79, 80, 81, 82, 83, 84, 85, 86,
			87, 88, 89, 90, 91, 92, 93, 118, 119, 120,
			121, 122, 123, 124
		}, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new PresetItemTemplateId("Weapon", 759), new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), 211, -1, 247, 527, new int[0], new int[0], new int[0], new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>(), new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(253, 528, EMapPickupsType.Item, EMapPickupsType2.Weapon, "map_eventicon_13", 358, 0, new byte[15]
		{
			0, 0, 0, 1, 0, 0, 0, 0, 0, 0,
			0, 0, 0, 0, 0
		}, new List<short>
		{
			57, 58, 59, 60, 61, 62, 63, 64, 65, 66,
			67, 68, 69, 70, 71, 72, 73, 74, 75, 76,
			77, 78, 79, 80, 81, 82, 83, 84, 85, 86,
			87, 88, 89, 90, 91, 92, 93, 118, 119, 120,
			121, 122, 123, 124
		}, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new PresetItemTemplateId("Weapon", 768), new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), 211, -1, 247, 529, new int[0], new int[0], new int[0], new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>(), new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(254, 530, EMapPickupsType.Item, EMapPickupsType2.Weapon, "map_eventicon_13", 358, 0, new byte[15]
		{
			0, 0, 0, 1, 0, 0, 0, 0, 0, 0,
			0, 0, 0, 0, 0
		}, new List<short>
		{
			57, 58, 59, 60, 61, 62, 63, 64, 65, 66,
			67, 68, 69, 70, 71, 72, 73, 74, 75, 76,
			77, 78, 79, 80, 81, 82, 83, 84, 85, 86,
			87, 88, 89, 90, 91, 92, 93, 118, 119, 120,
			121, 122, 123, 124
		}, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new PresetItemTemplateId("Weapon", 777), new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), 211, -1, 247, 531, new int[0], new int[0], new int[0], new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>(), new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(255, 532, EMapPickupsType.Item, EMapPickupsType2.Weapon, "map_eventicon_13", 358, 0, new byte[15]
		{
			0, 0, 0, 1, 0, 0, 0, 0, 0, 0,
			0, 0, 0, 0, 0
		}, new List<short>
		{
			57, 58, 59, 60, 61, 62, 63, 64, 65, 66,
			67, 68, 69, 70, 71, 72, 73, 74, 75, 76,
			77, 78, 79, 80, 81, 82, 83, 84, 85, 86,
			87, 88, 89, 90, 91, 92, 93, 118, 119, 120,
			121, 122, 123, 124
		}, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new PresetItemTemplateId("Weapon", 786), new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), 211, -1, 247, 533, new int[0], new int[0], new int[0], new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>(), new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(256, 534, EMapPickupsType.Item, EMapPickupsType2.Weapon, "map_eventicon_13", 358, 0, new byte[15]
		{
			0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
			0, 1, 0, 0, 0
		}, new List<short>
		{
			57, 58, 59, 60, 61, 62, 63, 64, 65, 66,
			67, 68, 69, 70, 71, 72, 73, 74, 75, 76,
			77, 78, 79, 80, 81, 82, 83, 84, 85, 86,
			87, 88, 89, 90, 91, 92, 93, 118, 119, 120,
			121, 122, 123, 124
		}, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new PresetItemTemplateId("Weapon", 795), new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), 212, -1, 247, 535, new int[0], new int[0], new int[0], new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>(), new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(257, 536, EMapPickupsType.Item, EMapPickupsType2.Weapon, "map_eventicon_13", 358, 0, new byte[15]
		{
			0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
			0, 1, 0, 0, 0
		}, new List<short>
		{
			57, 58, 59, 60, 61, 62, 63, 64, 65, 66,
			67, 68, 69, 70, 71, 72, 73, 74, 75, 76,
			77, 78, 79, 80, 81, 82, 83, 84, 85, 86,
			87, 88, 89, 90, 91, 92, 93, 118, 119, 120,
			121, 122, 123, 124
		}, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new PresetItemTemplateId("Weapon", 804), new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), 212, -1, 247, 537, new int[0], new int[0], new int[0], new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>(), new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(258, 538, EMapPickupsType.Item, EMapPickupsType2.Weapon, "map_eventicon_13", 358, 0, new byte[15]
		{
			0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
			0, 1, 0, 0, 0
		}, new List<short>
		{
			57, 58, 59, 60, 61, 62, 63, 64, 65, 66,
			67, 68, 69, 70, 71, 72, 73, 74, 75, 76,
			77, 78, 79, 80, 81, 82, 83, 84, 85, 86,
			87, 88, 89, 90, 91, 92, 93, 118, 119, 120,
			121, 122, 123, 124
		}, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new PresetItemTemplateId("Weapon", 813), new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), 212, -1, 247, 539, new int[0], new int[0], new int[0], new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>(), new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(259, 540, EMapPickupsType.Item, EMapPickupsType2.Weapon, "map_eventicon_13", 358, 0, new byte[15]
		{
			0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
			0, 1, 0, 0, 0
		}, new List<short>
		{
			57, 58, 59, 60, 61, 62, 63, 64, 65, 66,
			67, 68, 69, 70, 71, 72, 73, 74, 75, 76,
			77, 78, 79, 80, 81, 82, 83, 84, 85, 86,
			87, 88, 89, 90, 91, 92, 93, 118, 119, 120,
			121, 122, 123, 124
		}, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new PresetItemTemplateId("Weapon", 822), new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), 212, -1, 247, 541, new int[0], new int[0], new int[0], new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>(), new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(260, 542, EMapPickupsType.Item, EMapPickupsType2.Armor, "map_eventicon_14", 543, 0, new byte[15]
		{
			1, 0, 0, 0, 1, 1, 0, 0, 1, 0,
			1, 0, 0, 1, 1
		}, new List<short>
		{
			57, 58, 59, 60, 61, 62, 63, 64, 65, 66,
			67, 68, 69, 70, 71, 72, 73, 74, 75, 76,
			77, 78, 79, 80, 81, 82, 83, 84, 85, 86,
			87, 88, 89, 90, 91, 92, 93, 118, 119, 120,
			121, 122, 123, 124
		}, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new PresetItemTemplateId("Armor", 0), new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), 213, -1, 247, 544, new int[0], new int[0], new int[0], new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>(), new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(261, 545, EMapPickupsType.Item, EMapPickupsType2.Armor, "map_eventicon_14", 543, 0, new byte[15]
		{
			1, 0, 0, 0, 1, 1, 0, 0, 1, 0,
			1, 0, 0, 1, 1
		}, new List<short>
		{
			57, 58, 59, 60, 61, 62, 63, 64, 65, 66,
			67, 68, 69, 70, 71, 72, 73, 74, 75, 76,
			77, 78, 79, 80, 81, 82, 83, 84, 85, 86,
			87, 88, 89, 90, 91, 92, 93, 118, 119, 120,
			121, 122, 123, 124
		}, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new PresetItemTemplateId("Armor", 9), new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), 213, -1, 247, 546, new int[0], new int[0], new int[0], new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>(), new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(262, 547, EMapPickupsType.Item, EMapPickupsType2.Armor, "map_eventicon_14", 543, 0, new byte[15]
		{
			0, 1, 1, 1, 1, 0, 1, 0, 1, 0,
			0, 1, 0, 0, 1
		}, new List<short>
		{
			57, 58, 59, 60, 61, 62, 63, 64, 65, 66,
			67, 68, 69, 70, 71, 72, 73, 74, 75, 76,
			77, 78, 79, 80, 81, 82, 83, 84, 85, 86,
			87, 88, 89, 90, 91, 92, 93, 118, 119, 120,
			121, 122, 123, 124
		}, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new PresetItemTemplateId("Armor", 18), new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), 213, -1, 247, 548, new int[0], new int[0], new int[0], new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>(), new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(263, 549, EMapPickupsType.Item, EMapPickupsType2.Armor, "map_eventicon_14", 543, 0, new byte[15]
		{
			0, 1, 1, 1, 1, 0, 1, 0, 1, 0,
			0, 1, 0, 0, 1
		}, new List<short>
		{
			57, 58, 59, 60, 61, 62, 63, 64, 65, 66,
			67, 68, 69, 70, 71, 72, 73, 74, 75, 76,
			77, 78, 79, 80, 81, 82, 83, 84, 85, 86,
			87, 88, 89, 90, 91, 92, 93, 118, 119, 120,
			121, 122, 123, 124
		}, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new PresetItemTemplateId("Armor", 27), new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), 213, -1, 247, 550, new int[0], new int[0], new int[0], new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>(), new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(264, 551, EMapPickupsType.Item, EMapPickupsType2.Armor, "map_eventicon_14", 543, 0, new byte[15]
		{
			1, 0, 1, 1, 1, 1, 0, 0, 1, 1,
			1, 1, 1, 1, 1
		}, new List<short>
		{
			57, 58, 59, 60, 61, 62, 63, 64, 65, 66,
			67, 68, 69, 70, 71, 72, 73, 74, 75, 76,
			77, 78, 79, 80, 81, 82, 83, 84, 85, 86,
			87, 88, 89, 90, 91, 92, 93, 118, 119, 120,
			121, 122, 123, 124
		}, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new PresetItemTemplateId("Armor", 36), new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), 213, -1, 247, 552, new int[0], new int[0], new int[0], new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>(), new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(265, 553, EMapPickupsType.Item, EMapPickupsType2.Armor, "map_eventicon_14", 543, 0, new byte[15]
		{
			1, 0, 1, 1, 1, 1, 0, 0, 1, 1,
			1, 1, 1, 1, 1
		}, new List<short>
		{
			57, 58, 59, 60, 61, 62, 63, 64, 65, 66,
			67, 68, 69, 70, 71, 72, 73, 74, 75, 76,
			77, 78, 79, 80, 81, 82, 83, 84, 85, 86,
			87, 88, 89, 90, 91, 92, 93, 118, 119, 120,
			121, 122, 123, 124
		}, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new PresetItemTemplateId("Armor", 45), new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), 213, -1, 247, 554, new int[0], new int[0], new int[0], new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>(), new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(266, 555, EMapPickupsType.Item, EMapPickupsType2.Armor, "map_eventicon_14", 543, 0, new byte[15]
		{
			1, 0, 1, 1, 1, 1, 0, 0, 1, 1,
			1, 1, 1, 1, 1
		}, new List<short>
		{
			57, 58, 59, 60, 61, 62, 63, 64, 65, 66,
			67, 68, 69, 70, 71, 72, 73, 74, 75, 76,
			77, 78, 79, 80, 81, 82, 83, 84, 85, 86,
			87, 88, 89, 90, 91, 92, 93, 118, 119, 120,
			121, 122, 123, 124
		}, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new PresetItemTemplateId("Armor", 54), new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), 213, -1, 247, 556, new int[0], new int[0], new int[0], new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>(), new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(267, 557, EMapPickupsType.Item, EMapPickupsType2.Armor, "map_eventicon_14", 543, 0, new byte[15]
		{
			1, 0, 1, 1, 1, 1, 0, 0, 1, 1,
			1, 1, 1, 1, 1
		}, new List<short>
		{
			57, 58, 59, 60, 61, 62, 63, 64, 65, 66,
			67, 68, 69, 70, 71, 72, 73, 74, 75, 76,
			77, 78, 79, 80, 81, 82, 83, 84, 85, 86,
			87, 88, 89, 90, 91, 92, 93, 118, 119, 120,
			121, 122, 123, 124
		}, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new PresetItemTemplateId("Armor", 63), new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), 213, -1, 247, 558, new int[0], new int[0], new int[0], new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>(), new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(268, 559, EMapPickupsType.Item, EMapPickupsType2.Armor, "map_eventicon_14", 543, 0, new byte[15]
		{
			0, 1, 1, 1, 0, 0, 1, 1, 0, 0,
			0, 1, 1, 0, 0
		}, new List<short>
		{
			57, 58, 59, 60, 61, 62, 63, 64, 65, 66,
			67, 68, 69, 70, 71, 72, 73, 74, 75, 76,
			77, 78, 79, 80, 81, 82, 83, 84, 85, 86,
			87, 88, 89, 90, 91, 92, 93, 118, 119, 120,
			121, 122, 123, 124
		}, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new PresetItemTemplateId("Armor", 72), new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), 213, -1, 247, 560, new int[0], new int[0], new int[0], new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>(), new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(269, 561, EMapPickupsType.Item, EMapPickupsType2.Armor, "map_eventicon_14", 543, 0, new byte[15]
		{
			0, 1, 1, 1, 0, 0, 1, 1, 0, 0,
			0, 1, 1, 0, 0
		}, new List<short>
		{
			57, 58, 59, 60, 61, 62, 63, 64, 65, 66,
			67, 68, 69, 70, 71, 72, 73, 74, 75, 76,
			77, 78, 79, 80, 81, 82, 83, 84, 85, 86,
			87, 88, 89, 90, 91, 92, 93, 118, 119, 120,
			121, 122, 123, 124
		}, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new PresetItemTemplateId("Armor", 81), new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), 213, -1, 247, 562, new int[0], new int[0], new int[0], new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>(), new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(270, 563, EMapPickupsType.Item, EMapPickupsType2.Armor, "map_eventicon_14", 543, 0, new byte[15]
		{
			0, 1, 1, 1, 0, 0, 1, 1, 0, 0,
			0, 1, 1, 0, 0
		}, new List<short>
		{
			57, 58, 59, 60, 61, 62, 63, 64, 65, 66,
			67, 68, 69, 70, 71, 72, 73, 74, 75, 76,
			77, 78, 79, 80, 81, 82, 83, 84, 85, 86,
			87, 88, 89, 90, 91, 92, 93, 118, 119, 120,
			121, 122, 123, 124
		}, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new PresetItemTemplateId("Armor", 90), new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), 213, -1, 247, 564, new int[0], new int[0], new int[0], new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>(), new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(271, 565, EMapPickupsType.Item, EMapPickupsType2.Armor, "map_eventicon_14", 543, 0, new byte[15]
		{
			0, 1, 1, 1, 0, 0, 1, 1, 0, 0,
			0, 1, 1, 0, 0
		}, new List<short>
		{
			57, 58, 59, 60, 61, 62, 63, 64, 65, 66,
			67, 68, 69, 70, 71, 72, 73, 74, 75, 76,
			77, 78, 79, 80, 81, 82, 83, 84, 85, 86,
			87, 88, 89, 90, 91, 92, 93, 118, 119, 120,
			121, 122, 123, 124
		}, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new PresetItemTemplateId("Armor", 99), new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), 213, -1, 247, 566, new int[0], new int[0], new int[0], new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>(), new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(272, 567, EMapPickupsType.Item, EMapPickupsType2.Armor, "map_eventicon_14", 543, 0, new byte[15]
		{
			0, 0, 1, 0, 1, 1, 1, 1, 0, 1,
			1, 0, 0, 1, 0
		}, new List<short>
		{
			57, 58, 59, 60, 61, 62, 63, 64, 65, 66,
			67, 68, 69, 70, 71, 72, 73, 74, 75, 76,
			77, 78, 79, 80, 81, 82, 83, 84, 85, 86,
			87, 88, 89, 90, 91, 92, 93, 118, 119, 120,
			121, 122, 123, 124
		}, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new PresetItemTemplateId("Armor", 108), new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), 213, -1, 247, 568, new int[0], new int[0], new int[0], new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>(), new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(273, 569, EMapPickupsType.Item, EMapPickupsType2.Armor, "map_eventicon_14", 543, 0, new byte[15]
		{
			0, 0, 1, 0, 1, 1, 1, 1, 0, 1,
			1, 0, 0, 1, 0
		}, new List<short>
		{
			57, 58, 59, 60, 61, 62, 63, 64, 65, 66,
			67, 68, 69, 70, 71, 72, 73, 74, 75, 76,
			77, 78, 79, 80, 81, 82, 83, 84, 85, 86,
			87, 88, 89, 90, 91, 92, 93, 118, 119, 120,
			121, 122, 123, 124
		}, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new PresetItemTemplateId("Armor", 117), new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), 213, -1, 247, 570, new int[0], new int[0], new int[0], new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>(), new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(274, 571, EMapPickupsType.Item, EMapPickupsType2.Armor, "map_eventicon_14", 543, 0, new byte[15]
		{
			1, 0, 0, 0, 1, 1, 0, 0, 1, 0,
			1, 0, 0, 1, 1
		}, new List<short>
		{
			57, 58, 59, 60, 61, 62, 63, 64, 65, 66,
			67, 68, 69, 70, 71, 72, 73, 74, 75, 76,
			77, 78, 79, 80, 81, 82, 83, 84, 85, 86,
			87, 88, 89, 90, 91, 92, 93, 118, 119, 120,
			121, 122, 123, 124
		}, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new PresetItemTemplateId("Armor", 126), new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), 214, -1, 247, 572, new int[0], new int[0], new int[0], new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>(), new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(275, 573, EMapPickupsType.Item, EMapPickupsType2.Armor, "map_eventicon_14", 543, 0, new byte[15]
		{
			1, 0, 0, 0, 1, 1, 0, 0, 1, 0,
			1, 0, 0, 1, 1
		}, new List<short>
		{
			57, 58, 59, 60, 61, 62, 63, 64, 65, 66,
			67, 68, 69, 70, 71, 72, 73, 74, 75, 76,
			77, 78, 79, 80, 81, 82, 83, 84, 85, 86,
			87, 88, 89, 90, 91, 92, 93, 118, 119, 120,
			121, 122, 123, 124
		}, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new PresetItemTemplateId("Armor", 135), new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), 214, -1, 247, 574, new int[0], new int[0], new int[0], new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>(), new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(276, 575, EMapPickupsType.Item, EMapPickupsType2.Armor, "map_eventicon_14", 543, 0, new byte[15]
		{
			0, 1, 1, 1, 1, 0, 1, 0, 1, 0,
			0, 1, 0, 0, 1
		}, new List<short>
		{
			57, 58, 59, 60, 61, 62, 63, 64, 65, 66,
			67, 68, 69, 70, 71, 72, 73, 74, 75, 76,
			77, 78, 79, 80, 81, 82, 83, 84, 85, 86,
			87, 88, 89, 90, 91, 92, 93, 118, 119, 120,
			121, 122, 123, 124
		}, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new PresetItemTemplateId("Armor", 144), new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), 214, -1, 247, 576, new int[0], new int[0], new int[0], new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>(), new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(277, 577, EMapPickupsType.Item, EMapPickupsType2.Armor, "map_eventicon_14", 543, 0, new byte[15]
		{
			0, 1, 1, 1, 1, 0, 1, 0, 1, 0,
			0, 1, 0, 0, 1
		}, new List<short>
		{
			57, 58, 59, 60, 61, 62, 63, 64, 65, 66,
			67, 68, 69, 70, 71, 72, 73, 74, 75, 76,
			77, 78, 79, 80, 81, 82, 83, 84, 85, 86,
			87, 88, 89, 90, 91, 92, 93, 118, 119, 120,
			121, 122, 123, 124
		}, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new PresetItemTemplateId("Armor", 153), new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), 214, -1, 247, 578, new int[0], new int[0], new int[0], new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>(), new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(278, 579, EMapPickupsType.Item, EMapPickupsType2.Armor, "map_eventicon_14", 543, 0, new byte[15]
		{
			1, 0, 1, 1, 1, 1, 0, 0, 1, 1,
			1, 1, 1, 1, 1
		}, new List<short>
		{
			57, 58, 59, 60, 61, 62, 63, 64, 65, 66,
			67, 68, 69, 70, 71, 72, 73, 74, 75, 76,
			77, 78, 79, 80, 81, 82, 83, 84, 85, 86,
			87, 88, 89, 90, 91, 92, 93, 118, 119, 120,
			121, 122, 123, 124
		}, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new PresetItemTemplateId("Armor", 162), new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), 214, -1, 247, 580, new int[0], new int[0], new int[0], new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>(), new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(279, 581, EMapPickupsType.Item, EMapPickupsType2.Armor, "map_eventicon_14", 543, 0, new byte[15]
		{
			1, 0, 1, 1, 1, 1, 0, 0, 1, 1,
			1, 1, 1, 1, 1
		}, new List<short>
		{
			57, 58, 59, 60, 61, 62, 63, 64, 65, 66,
			67, 68, 69, 70, 71, 72, 73, 74, 75, 76,
			77, 78, 79, 80, 81, 82, 83, 84, 85, 86,
			87, 88, 89, 90, 91, 92, 93, 118, 119, 120,
			121, 122, 123, 124
		}, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new PresetItemTemplateId("Armor", 171), new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), 214, -1, 247, 582, new int[0], new int[0], new int[0], new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>(), new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(280, 583, EMapPickupsType.Item, EMapPickupsType2.Armor, "map_eventicon_14", 543, 0, new byte[15]
		{
			1, 0, 1, 1, 1, 1, 0, 0, 1, 1,
			1, 1, 1, 1, 1
		}, new List<short>
		{
			57, 58, 59, 60, 61, 62, 63, 64, 65, 66,
			67, 68, 69, 70, 71, 72, 73, 74, 75, 76,
			77, 78, 79, 80, 81, 82, 83, 84, 85, 86,
			87, 88, 89, 90, 91, 92, 93, 118, 119, 120,
			121, 122, 123, 124
		}, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new PresetItemTemplateId("Armor", 180), new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), 214, -1, 247, 584, new int[0], new int[0], new int[0], new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>(), new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(281, 585, EMapPickupsType.Item, EMapPickupsType2.Armor, "map_eventicon_14", 543, 0, new byte[15]
		{
			1, 0, 1, 1, 1, 1, 0, 0, 1, 1,
			1, 1, 1, 1, 1
		}, new List<short>
		{
			57, 58, 59, 60, 61, 62, 63, 64, 65, 66,
			67, 68, 69, 70, 71, 72, 73, 74, 75, 76,
			77, 78, 79, 80, 81, 82, 83, 84, 85, 86,
			87, 88, 89, 90, 91, 92, 93, 118, 119, 120,
			121, 122, 123, 124
		}, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new PresetItemTemplateId("Armor", 189), new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), 214, -1, 247, 586, new int[0], new int[0], new int[0], new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>(), new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(282, 587, EMapPickupsType.Item, EMapPickupsType2.Armor, "map_eventicon_14", 543, 0, new byte[15]
		{
			0, 1, 1, 1, 0, 0, 1, 1, 0, 0,
			0, 1, 1, 0, 0
		}, new List<short>
		{
			57, 58, 59, 60, 61, 62, 63, 64, 65, 66,
			67, 68, 69, 70, 71, 72, 73, 74, 75, 76,
			77, 78, 79, 80, 81, 82, 83, 84, 85, 86,
			87, 88, 89, 90, 91, 92, 93, 118, 119, 120,
			121, 122, 123, 124
		}, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new PresetItemTemplateId("Armor", 198), new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), 214, -1, 247, 588, new int[0], new int[0], new int[0], new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>(), new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(283, 589, EMapPickupsType.Item, EMapPickupsType2.Armor, "map_eventicon_14", 543, 0, new byte[15]
		{
			0, 1, 1, 1, 0, 0, 1, 1, 0, 0,
			0, 1, 1, 0, 0
		}, new List<short>
		{
			57, 58, 59, 60, 61, 62, 63, 64, 65, 66,
			67, 68, 69, 70, 71, 72, 73, 74, 75, 76,
			77, 78, 79, 80, 81, 82, 83, 84, 85, 86,
			87, 88, 89, 90, 91, 92, 93, 118, 119, 120,
			121, 122, 123, 124
		}, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new PresetItemTemplateId("Armor", 207), new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), 214, -1, 247, 590, new int[0], new int[0], new int[0], new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>(), new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(284, 591, EMapPickupsType.Item, EMapPickupsType2.Armor, "map_eventicon_14", 543, 0, new byte[15]
		{
			0, 1, 1, 1, 0, 0, 1, 1, 0, 0,
			0, 1, 1, 0, 0
		}, new List<short>
		{
			57, 58, 59, 60, 61, 62, 63, 64, 65, 66,
			67, 68, 69, 70, 71, 72, 73, 74, 75, 76,
			77, 78, 79, 80, 81, 82, 83, 84, 85, 86,
			87, 88, 89, 90, 91, 92, 93, 118, 119, 120,
			121, 122, 123, 124
		}, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new PresetItemTemplateId("Armor", 216), new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), 214, -1, 247, 592, new int[0], new int[0], new int[0], new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>(), new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(285, 593, EMapPickupsType.Item, EMapPickupsType2.Armor, "map_eventicon_14", 543, 0, new byte[15]
		{
			0, 1, 1, 1, 0, 0, 1, 1, 0, 0,
			0, 1, 1, 0, 0
		}, new List<short>
		{
			57, 58, 59, 60, 61, 62, 63, 64, 65, 66,
			67, 68, 69, 70, 71, 72, 73, 74, 75, 76,
			77, 78, 79, 80, 81, 82, 83, 84, 85, 86,
			87, 88, 89, 90, 91, 92, 93, 118, 119, 120,
			121, 122, 123, 124
		}, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new PresetItemTemplateId("Armor", 225), new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), 214, -1, 247, 594, new int[0], new int[0], new int[0], new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>(), new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(286, 595, EMapPickupsType.Item, EMapPickupsType2.Armor, "map_eventicon_14", 543, 0, new byte[15]
		{
			0, 0, 1, 0, 1, 1, 1, 1, 0, 1,
			1, 0, 0, 1, 0
		}, new List<short>
		{
			57, 58, 59, 60, 61, 62, 63, 64, 65, 66,
			67, 68, 69, 70, 71, 72, 73, 74, 75, 76,
			77, 78, 79, 80, 81, 82, 83, 84, 85, 86,
			87, 88, 89, 90, 91, 92, 93, 118, 119, 120,
			121, 122, 123, 124
		}, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new PresetItemTemplateId("Armor", 234), new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), 214, -1, 247, 596, new int[0], new int[0], new int[0], new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>(), new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(287, 597, EMapPickupsType.Item, EMapPickupsType2.Armor, "map_eventicon_14", 543, 0, new byte[15]
		{
			0, 0, 1, 0, 1, 1, 1, 1, 0, 1,
			1, 0, 0, 1, 0
		}, new List<short>
		{
			57, 58, 59, 60, 61, 62, 63, 64, 65, 66,
			67, 68, 69, 70, 71, 72, 73, 74, 75, 76,
			77, 78, 79, 80, 81, 82, 83, 84, 85, 86,
			87, 88, 89, 90, 91, 92, 93, 118, 119, 120,
			121, 122, 123, 124
		}, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new PresetItemTemplateId("Armor", 243), new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), 214, -1, 247, 598, new int[0], new int[0], new int[0], new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>(), new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(288, 599, EMapPickupsType.Item, EMapPickupsType2.Armor, "map_eventicon_14", 543, 0, new byte[15]
		{
			1, 0, 0, 0, 1, 1, 0, 0, 1, 0,
			1, 0, 0, 1, 1
		}, new List<short>
		{
			57, 58, 59, 60, 61, 62, 63, 64, 65, 66,
			67, 68, 69, 70, 71, 72, 73, 74, 75, 76,
			77, 78, 79, 80, 81, 82, 83, 84, 85, 86,
			87, 88, 89, 90, 91, 92, 93, 118, 119, 120,
			121, 122, 123, 124
		}, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new PresetItemTemplateId("Armor", 252), new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), 215, -1, 247, 600, new int[0], new int[0], new int[0], new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>(), new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(289, 601, EMapPickupsType.Item, EMapPickupsType2.Armor, "map_eventicon_14", 543, 0, new byte[15]
		{
			1, 0, 0, 0, 1, 1, 0, 0, 1, 0,
			1, 0, 0, 1, 1
		}, new List<short>
		{
			57, 58, 59, 60, 61, 62, 63, 64, 65, 66,
			67, 68, 69, 70, 71, 72, 73, 74, 75, 76,
			77, 78, 79, 80, 81, 82, 83, 84, 85, 86,
			87, 88, 89, 90, 91, 92, 93, 118, 119, 120,
			121, 122, 123, 124
		}, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new PresetItemTemplateId("Armor", 261), new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), 215, -1, 247, 602, new int[0], new int[0], new int[0], new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>(), new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(290, 603, EMapPickupsType.Item, EMapPickupsType2.Armor, "map_eventicon_14", 543, 0, new byte[15]
		{
			1, 0, 0, 0, 1, 1, 0, 0, 1, 0,
			1, 0, 0, 1, 1
		}, new List<short>
		{
			57, 58, 59, 60, 61, 62, 63, 64, 65, 66,
			67, 68, 69, 70, 71, 72, 73, 74, 75, 76,
			77, 78, 79, 80, 81, 82, 83, 84, 85, 86,
			87, 88, 89, 90, 91, 92, 93, 118, 119, 120,
			121, 122, 123, 124
		}, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new PresetItemTemplateId("Armor", 270), new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), 215, -1, 247, 604, new int[0], new int[0], new int[0], new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>(), new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(291, 605, EMapPickupsType.Item, EMapPickupsType2.Armor, "map_eventicon_14", 543, 0, new byte[15]
		{
			1, 0, 0, 0, 1, 1, 0, 0, 1, 0,
			1, 0, 0, 1, 1
		}, new List<short>
		{
			57, 58, 59, 60, 61, 62, 63, 64, 65, 66,
			67, 68, 69, 70, 71, 72, 73, 74, 75, 76,
			77, 78, 79, 80, 81, 82, 83, 84, 85, 86,
			87, 88, 89, 90, 91, 92, 93, 118, 119, 120,
			121, 122, 123, 124
		}, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new PresetItemTemplateId("Armor", 279), new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), 215, -1, 247, 606, new int[0], new int[0], new int[0], new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>(), new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(292, 607, EMapPickupsType.Item, EMapPickupsType2.Armor, "map_eventicon_14", 543, 0, new byte[15]
		{
			0, 1, 1, 1, 1, 0, 1, 0, 1, 0,
			0, 1, 0, 0, 1
		}, new List<short>
		{
			57, 58, 59, 60, 61, 62, 63, 64, 65, 66,
			67, 68, 69, 70, 71, 72, 73, 74, 75, 76,
			77, 78, 79, 80, 81, 82, 83, 84, 85, 86,
			87, 88, 89, 90, 91, 92, 93, 118, 119, 120,
			121, 122, 123, 124
		}, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new PresetItemTemplateId("Armor", 288), new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), 215, -1, 247, 608, new int[0], new int[0], new int[0], new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>(), new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(293, 609, EMapPickupsType.Item, EMapPickupsType2.Armor, "map_eventicon_14", 543, 0, new byte[15]
		{
			0, 1, 1, 1, 1, 0, 1, 0, 1, 0,
			0, 1, 0, 0, 1
		}, new List<short>
		{
			57, 58, 59, 60, 61, 62, 63, 64, 65, 66,
			67, 68, 69, 70, 71, 72, 73, 74, 75, 76,
			77, 78, 79, 80, 81, 82, 83, 84, 85, 86,
			87, 88, 89, 90, 91, 92, 93, 118, 119, 120,
			121, 122, 123, 124
		}, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new PresetItemTemplateId("Armor", 297), new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), 215, -1, 247, 610, new int[0], new int[0], new int[0], new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>(), new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(294, 611, EMapPickupsType.Item, EMapPickupsType2.Armor, "map_eventicon_14", 543, 0, new byte[15]
		{
			1, 0, 1, 1, 1, 1, 0, 0, 1, 1,
			1, 1, 1, 1, 1
		}, new List<short>
		{
			57, 58, 59, 60, 61, 62, 63, 64, 65, 66,
			67, 68, 69, 70, 71, 72, 73, 74, 75, 76,
			77, 78, 79, 80, 81, 82, 83, 84, 85, 86,
			87, 88, 89, 90, 91, 92, 93, 118, 119, 120,
			121, 122, 123, 124
		}, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new PresetItemTemplateId("Armor", 306), new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), 215, -1, 247, 612, new int[0], new int[0], new int[0], new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>(), new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(295, 613, EMapPickupsType.Item, EMapPickupsType2.Armor, "map_eventicon_14", 543, 0, new byte[15]
		{
			1, 0, 1, 1, 1, 1, 0, 0, 1, 1,
			1, 1, 1, 1, 1
		}, new List<short>
		{
			57, 58, 59, 60, 61, 62, 63, 64, 65, 66,
			67, 68, 69, 70, 71, 72, 73, 74, 75, 76,
			77, 78, 79, 80, 81, 82, 83, 84, 85, 86,
			87, 88, 89, 90, 91, 92, 93, 118, 119, 120,
			121, 122, 123, 124
		}, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new PresetItemTemplateId("Armor", 315), new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), 215, -1, 247, 614, new int[0], new int[0], new int[0], new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>(), new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(296, 615, EMapPickupsType.Item, EMapPickupsType2.Armor, "map_eventicon_14", 543, 0, new byte[15]
		{
			1, 0, 1, 1, 1, 1, 0, 0, 1, 1,
			1, 1, 1, 1, 1
		}, new List<short>
		{
			57, 58, 59, 60, 61, 62, 63, 64, 65, 66,
			67, 68, 69, 70, 71, 72, 73, 74, 75, 76,
			77, 78, 79, 80, 81, 82, 83, 84, 85, 86,
			87, 88, 89, 90, 91, 92, 93, 118, 119, 120,
			121, 122, 123, 124
		}, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new PresetItemTemplateId("Armor", 324), new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), 215, -1, 247, 616, new int[0], new int[0], new int[0], new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>(), new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(297, 617, EMapPickupsType.Item, EMapPickupsType2.Armor, "map_eventicon_14", 543, 0, new byte[15]
		{
			1, 0, 1, 1, 1, 1, 0, 0, 1, 1,
			1, 1, 1, 1, 1
		}, new List<short>
		{
			57, 58, 59, 60, 61, 62, 63, 64, 65, 66,
			67, 68, 69, 70, 71, 72, 73, 74, 75, 76,
			77, 78, 79, 80, 81, 82, 83, 84, 85, 86,
			87, 88, 89, 90, 91, 92, 93, 118, 119, 120,
			121, 122, 123, 124
		}, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new PresetItemTemplateId("Armor", 333), new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), 215, -1, 247, 618, new int[0], new int[0], new int[0], new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>(), new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(298, 619, EMapPickupsType.Item, EMapPickupsType2.Armor, "map_eventicon_14", 543, 0, new byte[15]
		{
			0, 1, 1, 1, 0, 0, 1, 1, 0, 0,
			0, 1, 1, 0, 0
		}, new List<short>
		{
			57, 58, 59, 60, 61, 62, 63, 64, 65, 66,
			67, 68, 69, 70, 71, 72, 73, 74, 75, 76,
			77, 78, 79, 80, 81, 82, 83, 84, 85, 86,
			87, 88, 89, 90, 91, 92, 93, 118, 119, 120,
			121, 122, 123, 124
		}, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new PresetItemTemplateId("Armor", 342), new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), 215, -1, 247, 620, new int[0], new int[0], new int[0], new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>(), new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(299, 621, EMapPickupsType.Item, EMapPickupsType2.Armor, "map_eventicon_14", 543, 0, new byte[15]
		{
			0, 1, 1, 1, 0, 0, 1, 1, 0, 0,
			0, 1, 1, 0, 0
		}, new List<short>
		{
			57, 58, 59, 60, 61, 62, 63, 64, 65, 66,
			67, 68, 69, 70, 71, 72, 73, 74, 75, 76,
			77, 78, 79, 80, 81, 82, 83, 84, 85, 86,
			87, 88, 89, 90, 91, 92, 93, 118, 119, 120,
			121, 122, 123, 124
		}, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new PresetItemTemplateId("Armor", 351), new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), 215, -1, 247, 622, new int[0], new int[0], new int[0], new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>(), new List<PresetItemWithCount>()));
	}

	private void CreateItems5()
	{
		_dataArray.Add(new MapPickupsItem(300, 623, EMapPickupsType.Item, EMapPickupsType2.Armor, "map_eventicon_14", 543, 0, new byte[15]
		{
			0, 1, 1, 1, 0, 0, 1, 1, 0, 0,
			0, 1, 1, 0, 0
		}, new List<short>
		{
			57, 58, 59, 60, 61, 62, 63, 64, 65, 66,
			67, 68, 69, 70, 71, 72, 73, 74, 75, 76,
			77, 78, 79, 80, 81, 82, 83, 84, 85, 86,
			87, 88, 89, 90, 91, 92, 93, 118, 119, 120,
			121, 122, 123, 124
		}, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new PresetItemTemplateId("Armor", 360), new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), 215, -1, 247, 624, new int[0], new int[0], new int[0], new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>(), new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(301, 625, EMapPickupsType.Item, EMapPickupsType2.Armor, "map_eventicon_14", 543, 0, new byte[15]
		{
			0, 1, 1, 1, 0, 0, 1, 1, 0, 0,
			0, 1, 1, 0, 0
		}, new List<short>
		{
			57, 58, 59, 60, 61, 62, 63, 64, 65, 66,
			67, 68, 69, 70, 71, 72, 73, 74, 75, 76,
			77, 78, 79, 80, 81, 82, 83, 84, 85, 86,
			87, 88, 89, 90, 91, 92, 93, 118, 119, 120,
			121, 122, 123, 124
		}, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new PresetItemTemplateId("Armor", 369), new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), 215, -1, 247, 626, new int[0], new int[0], new int[0], new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>(), new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(302, 627, EMapPickupsType.Item, EMapPickupsType2.Armor, "map_eventicon_14", 543, 0, new byte[15]
		{
			0, 0, 1, 0, 1, 1, 1, 1, 0, 1,
			1, 0, 0, 1, 0
		}, new List<short>
		{
			57, 58, 59, 60, 61, 62, 63, 64, 65, 66,
			67, 68, 69, 70, 71, 72, 73, 74, 75, 76,
			77, 78, 79, 80, 81, 82, 83, 84, 85, 86,
			87, 88, 89, 90, 91, 92, 93, 118, 119, 120,
			121, 122, 123, 124
		}, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new PresetItemTemplateId("Armor", 378), new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), 215, -1, 247, 628, new int[0], new int[0], new int[0], new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>(), new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(303, 629, EMapPickupsType.Item, EMapPickupsType2.Armor, "map_eventicon_14", 543, 0, new byte[15]
		{
			0, 0, 1, 0, 1, 1, 1, 1, 0, 1,
			1, 0, 0, 1, 0
		}, new List<short>
		{
			57, 58, 59, 60, 61, 62, 63, 64, 65, 66,
			67, 68, 69, 70, 71, 72, 73, 74, 75, 76,
			77, 78, 79, 80, 81, 82, 83, 84, 85, 86,
			87, 88, 89, 90, 91, 92, 93, 118, 119, 120,
			121, 122, 123, 124
		}, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new PresetItemTemplateId("Armor", 387), new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), 215, -1, 247, 630, new int[0], new int[0], new int[0], new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>(), new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(304, 631, EMapPickupsType.Item, EMapPickupsType2.Armor, "map_eventicon_14", 543, 0, new byte[15]
		{
			1, 0, 0, 0, 1, 1, 0, 0, 1, 0,
			1, 0, 0, 1, 1
		}, new List<short>
		{
			57, 58, 59, 60, 61, 62, 63, 64, 65, 66,
			67, 68, 69, 70, 71, 72, 73, 74, 75, 76,
			77, 78, 79, 80, 81, 82, 83, 84, 85, 86,
			87, 88, 89, 90, 91, 92, 93, 118, 119, 120,
			121, 122, 123, 124
		}, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new PresetItemTemplateId("Armor", 396), new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), 216, -1, 247, 632, new int[0], new int[0], new int[0], new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>(), new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(305, 633, EMapPickupsType.Item, EMapPickupsType2.Armor, "map_eventicon_14", 543, 0, new byte[15]
		{
			1, 0, 0, 0, 1, 1, 0, 0, 1, 0,
			1, 0, 0, 1, 1
		}, new List<short>
		{
			57, 58, 59, 60, 61, 62, 63, 64, 65, 66,
			67, 68, 69, 70, 71, 72, 73, 74, 75, 76,
			77, 78, 79, 80, 81, 82, 83, 84, 85, 86,
			87, 88, 89, 90, 91, 92, 93, 118, 119, 120,
			121, 122, 123, 124
		}, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new PresetItemTemplateId("Armor", 405), new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), 216, -1, 247, 634, new int[0], new int[0], new int[0], new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>(), new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(306, 635, EMapPickupsType.Item, EMapPickupsType2.Armor, "map_eventicon_14", 543, 0, new byte[15]
		{
			0, 1, 1, 1, 1, 0, 1, 0, 1, 0,
			0, 1, 0, 0, 1
		}, new List<short>
		{
			57, 58, 59, 60, 61, 62, 63, 64, 65, 66,
			67, 68, 69, 70, 71, 72, 73, 74, 75, 76,
			77, 78, 79, 80, 81, 82, 83, 84, 85, 86,
			87, 88, 89, 90, 91, 92, 93, 118, 119, 120,
			121, 122, 123, 124
		}, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new PresetItemTemplateId("Armor", 414), new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), 216, -1, 247, 636, new int[0], new int[0], new int[0], new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>(), new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(307, 637, EMapPickupsType.Item, EMapPickupsType2.Armor, "map_eventicon_14", 543, 0, new byte[15]
		{
			0, 1, 1, 1, 1, 0, 1, 0, 1, 0,
			0, 1, 0, 0, 1
		}, new List<short>
		{
			57, 58, 59, 60, 61, 62, 63, 64, 65, 66,
			67, 68, 69, 70, 71, 72, 73, 74, 75, 76,
			77, 78, 79, 80, 81, 82, 83, 84, 85, 86,
			87, 88, 89, 90, 91, 92, 93, 118, 119, 120,
			121, 122, 123, 124
		}, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new PresetItemTemplateId("Armor", 423), new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), 216, -1, 247, 638, new int[0], new int[0], new int[0], new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>(), new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(308, 639, EMapPickupsType.Item, EMapPickupsType2.Armor, "map_eventicon_14", 543, 0, new byte[15]
		{
			1, 0, 1, 1, 1, 1, 0, 0, 1, 1,
			1, 1, 1, 1, 1
		}, new List<short>
		{
			57, 58, 59, 60, 61, 62, 63, 64, 65, 66,
			67, 68, 69, 70, 71, 72, 73, 74, 75, 76,
			77, 78, 79, 80, 81, 82, 83, 84, 85, 86,
			87, 88, 89, 90, 91, 92, 93, 118, 119, 120,
			121, 122, 123, 124
		}, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new PresetItemTemplateId("Armor", 432), new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), 216, -1, 247, 640, new int[0], new int[0], new int[0], new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>(), new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(309, 641, EMapPickupsType.Item, EMapPickupsType2.Armor, "map_eventicon_14", 543, 0, new byte[15]
		{
			1, 0, 1, 1, 1, 1, 0, 0, 1, 1,
			1, 1, 1, 1, 1
		}, new List<short>
		{
			57, 58, 59, 60, 61, 62, 63, 64, 65, 66,
			67, 68, 69, 70, 71, 72, 73, 74, 75, 76,
			77, 78, 79, 80, 81, 82, 83, 84, 85, 86,
			87, 88, 89, 90, 91, 92, 93, 118, 119, 120,
			121, 122, 123, 124
		}, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new PresetItemTemplateId("Armor", 441), new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), 216, -1, 247, 642, new int[0], new int[0], new int[0], new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>(), new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(310, 643, EMapPickupsType.Item, EMapPickupsType2.Armor, "map_eventicon_14", 543, 0, new byte[15]
		{
			1, 0, 1, 1, 1, 1, 0, 0, 1, 1,
			1, 1, 1, 1, 1
		}, new List<short>
		{
			57, 58, 59, 60, 61, 62, 63, 64, 65, 66,
			67, 68, 69, 70, 71, 72, 73, 74, 75, 76,
			77, 78, 79, 80, 81, 82, 83, 84, 85, 86,
			87, 88, 89, 90, 91, 92, 93, 118, 119, 120,
			121, 122, 123, 124
		}, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new PresetItemTemplateId("Armor", 450), new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), 216, -1, 247, 644, new int[0], new int[0], new int[0], new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>(), new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(311, 645, EMapPickupsType.Item, EMapPickupsType2.Armor, "map_eventicon_14", 543, 0, new byte[15]
		{
			1, 0, 1, 1, 1, 1, 0, 0, 1, 1,
			1, 1, 1, 1, 1
		}, new List<short>
		{
			57, 58, 59, 60, 61, 62, 63, 64, 65, 66,
			67, 68, 69, 70, 71, 72, 73, 74, 75, 76,
			77, 78, 79, 80, 81, 82, 83, 84, 85, 86,
			87, 88, 89, 90, 91, 92, 93, 118, 119, 120,
			121, 122, 123, 124
		}, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new PresetItemTemplateId("Armor", 459), new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), 216, -1, 247, 646, new int[0], new int[0], new int[0], new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>(), new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(312, 647, EMapPickupsType.Item, EMapPickupsType2.Armor, "map_eventicon_14", 543, 0, new byte[15]
		{
			0, 1, 1, 1, 0, 0, 1, 1, 0, 0,
			0, 1, 1, 0, 0
		}, new List<short>
		{
			57, 58, 59, 60, 61, 62, 63, 64, 65, 66,
			67, 68, 69, 70, 71, 72, 73, 74, 75, 76,
			77, 78, 79, 80, 81, 82, 83, 84, 85, 86,
			87, 88, 89, 90, 91, 92, 93, 118, 119, 120,
			121, 122, 123, 124
		}, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new PresetItemTemplateId("Armor", 468), new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), 216, -1, 247, 648, new int[0], new int[0], new int[0], new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>(), new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(313, 649, EMapPickupsType.Item, EMapPickupsType2.Armor, "map_eventicon_14", 543, 0, new byte[15]
		{
			0, 1, 1, 1, 0, 0, 1, 1, 0, 0,
			0, 1, 1, 0, 0
		}, new List<short>
		{
			57, 58, 59, 60, 61, 62, 63, 64, 65, 66,
			67, 68, 69, 70, 71, 72, 73, 74, 75, 76,
			77, 78, 79, 80, 81, 82, 83, 84, 85, 86,
			87, 88, 89, 90, 91, 92, 93, 118, 119, 120,
			121, 122, 123, 124
		}, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new PresetItemTemplateId("Armor", 477), new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), 216, -1, 247, 650, new int[0], new int[0], new int[0], new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>(), new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(314, 651, EMapPickupsType.Item, EMapPickupsType2.Armor, "map_eventicon_14", 543, 0, new byte[15]
		{
			0, 1, 1, 1, 0, 0, 1, 1, 0, 0,
			0, 1, 1, 0, 0
		}, new List<short>
		{
			57, 58, 59, 60, 61, 62, 63, 64, 65, 66,
			67, 68, 69, 70, 71, 72, 73, 74, 75, 76,
			77, 78, 79, 80, 81, 82, 83, 84, 85, 86,
			87, 88, 89, 90, 91, 92, 93, 118, 119, 120,
			121, 122, 123, 124
		}, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new PresetItemTemplateId("Armor", 486), new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), 216, -1, 247, 652, new int[0], new int[0], new int[0], new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>(), new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(315, 653, EMapPickupsType.Item, EMapPickupsType2.Armor, "map_eventicon_14", 543, 0, new byte[15]
		{
			0, 1, 1, 1, 0, 0, 1, 1, 0, 0,
			0, 1, 1, 0, 0
		}, new List<short>
		{
			57, 58, 59, 60, 61, 62, 63, 64, 65, 66,
			67, 68, 69, 70, 71, 72, 73, 74, 75, 76,
			77, 78, 79, 80, 81, 82, 83, 84, 85, 86,
			87, 88, 89, 90, 91, 92, 93, 118, 119, 120,
			121, 122, 123, 124
		}, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new PresetItemTemplateId("Armor", 495), new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), 216, -1, 247, 654, new int[0], new int[0], new int[0], new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>(), new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(316, 655, EMapPickupsType.Item, EMapPickupsType2.Armor, "map_eventicon_14", 543, 0, new byte[15]
		{
			0, 0, 1, 0, 1, 1, 1, 1, 0, 1,
			1, 0, 0, 1, 0
		}, new List<short>
		{
			57, 58, 59, 60, 61, 62, 63, 64, 65, 66,
			67, 68, 69, 70, 71, 72, 73, 74, 75, 76,
			77, 78, 79, 80, 81, 82, 83, 84, 85, 86,
			87, 88, 89, 90, 91, 92, 93, 118, 119, 120,
			121, 122, 123, 124
		}, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new PresetItemTemplateId("Armor", 504), new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), 216, -1, 247, 656, new int[0], new int[0], new int[0], new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>(), new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(317, 657, EMapPickupsType.Item, EMapPickupsType2.Armor, "map_eventicon_14", 543, 0, new byte[15]
		{
			0, 0, 1, 0, 1, 1, 1, 1, 0, 1,
			1, 0, 0, 1, 0
		}, new List<short>
		{
			57, 58, 59, 60, 61, 62, 63, 64, 65, 66,
			67, 68, 69, 70, 71, 72, 73, 74, 75, 76,
			77, 78, 79, 80, 81, 82, 83, 84, 85, 86,
			87, 88, 89, 90, 91, 92, 93, 118, 119, 120,
			121, 122, 123, 124
		}, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new PresetItemTemplateId("Armor", 513), new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), 216, -1, 247, 658, new int[0], new int[0], new int[0], new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>(), new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(318, 659, EMapPickupsType.Item, EMapPickupsType2.Carrier, "map_eventicon_15", 660, 1, new byte[15]
		{
			1, 0, 0, 0, 0, 1, 0, 0, 1, 0,
			0, 0, 0, 0, 0
		}, new List<short>
		{
			39, 40, 41, 42, 43, 44, 45, 46, 47, 54,
			55, 56, 48, 49, 50, 51, 52, 53, 1, 2,
			3, 4, 5, 6, 7, 8, 9, 10, 11, 12,
			13, 14, 15, 34, 35, 36
		}, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new PresetItemTemplateId("Carrier", 0), new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), 217, -1, 247, 661, new int[0], new int[0], new int[0], new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>(), new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(319, 662, EMapPickupsType.Item, EMapPickupsType2.Carrier, "map_eventicon_15", 660, 1, new byte[15]
		{
			0, 0, 0, 1, 0, 0, 0, 1, 0, 0,
			0, 0, 0, 0, 1
		}, new List<short>
		{
			39, 40, 41, 42, 43, 44, 45, 46, 47, 54,
			55, 56, 48, 49, 50, 51, 52, 53, 1, 2,
			3, 4, 5, 6, 7, 8, 9, 10, 11, 12,
			13, 14, 15, 34, 35, 36
		}, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new PresetItemTemplateId("Carrier", 9), new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), 217, -1, 247, 663, new int[0], new int[0], new int[0], new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>(), new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(320, 664, EMapPickupsType.Item, EMapPickupsType2.Carrier, "map_eventicon_15", 660, 0, new byte[15]
		{
			0, 0, 1, 0, 0, 0, 0, 0, 0, 1,
			1, 0, 0, 0, 0
		}, new List<short>
		{
			39, 40, 41, 42, 43, 44, 45, 46, 47, 54,
			55, 56, 48, 49, 50, 51, 52, 53, 1, 2,
			3, 4, 5, 6, 7, 8, 9, 10, 11, 12,
			13, 14, 15, 34, 35, 36
		}, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new PresetItemTemplateId("Carrier", 18), new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), 217, -1, 247, 665, new int[0], new int[0], new int[0], new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>(), new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(321, 666, EMapPickupsType.Item, EMapPickupsType2.Carrier, "map_eventicon_15", 660, 0, new byte[15]
		{
			0, 1, 0, 0, 0, 0, 0, 0, 0, 0,
			0, 1, 0, 1, 0
		}, new List<short>
		{
			94, 95, 96, 97, 98, 99, 100, 101, 102, 103,
			104, 105, 106, 107, 108
		}, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[3] { 2, 3, 4 }, new PresetItemTemplateId("Carrier", 27), new sbyte[3] { 2, 4, 6 }, new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), 217, -1, 247, 667, new int[0], new int[0], new int[0], new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>(), new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(322, 668, EMapPickupsType.Growth, EMapPickupsType2.Exp, "map_eventicon_20", 669, 2, new byte[15]
		{
			2, 2, 2, 2, 2, 2, 2, 2, 2, 2,
			2, 2, 2, 2, 2
		}, new List<short>
		{
			39, 40, 41, 42, 43, 44, 45, 46, 47, 54,
			55, 56, 48, 49, 50, 51, 52, 53, 1, 2,
			3, 4, 5, 6, 7, 8, 9, 10, 11, 12,
			13, 14, 15, 34, 35, 36
		}, readEffect: false, loopEffect: false, isExpBonus: true, isDebtBonus: false, new int[8] { 100, 200, 400, 800, 1600, 3200, 6400, 12800 }, new sbyte[0], default(PresetItemTemplateId), new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), 218, 245, -1, 670, new int[0], new int[0], new int[0], new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>(), new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(323, 671, EMapPickupsType.Growth, EMapPickupsType2.Exp, "map_eventicon_20", 669, 2, new byte[15]
		{
			3, 3, 3, 3, 3, 3, 3, 3, 3, 3,
			3, 3, 3, 3, 3
		}, new List<short>
		{
			57, 58, 59, 60, 61, 62, 63, 64, 65, 66,
			67, 68, 69, 70, 71, 72, 73, 74, 75, 76,
			77, 78, 79, 80, 81, 82, 83, 84, 85, 86,
			87, 88, 89, 90, 91, 92, 93
		}, readEffect: false, loopEffect: false, isExpBonus: true, isDebtBonus: false, new int[8] { 150, 300, 600, 1200, 2400, 4800, 9600, 19200 }, new sbyte[0], default(PresetItemTemplateId), new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), 218, 245, -1, 672, new int[0], new int[0], new int[0], new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>(), new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(324, 673, EMapPickupsType.Growth, EMapPickupsType2.Exp, "map_eventicon_20", 669, 0, new byte[15]
		{
			3, 3, 3, 3, 3, 3, 3, 3, 3, 3,
			3, 3, 3, 3, 3
		}, new List<short>
		{
			94, 95, 96, 97, 98, 99, 100, 101, 102, 103,
			104, 105, 106, 107, 108, 118, 119, 120, 121, 122,
			123, 124
		}, readEffect: false, loopEffect: false, isExpBonus: true, isDebtBonus: false, new int[8] { 200, 400, 800, 1600, 3200, 6400, 12800, 25600 }, new sbyte[0], default(PresetItemTemplateId), new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), 218, 245, -1, 674, new int[0], new int[0], new int[0], new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>(), new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(325, 675, EMapPickupsType.Growth, EMapPickupsType2.Read, "map_eventicon_16", 675, 1, new byte[15]
		{
			3, 3, 3, 3, 3, 3, 3, 3, 3, 3,
			3, 3, 3, 3, 3
		}, new List<short>
		{
			57, 58, 59, 60, 61, 62, 63, 64, 65, 66,
			67, 68, 69, 70, 71, 72, 73, 74, 75, 76,
			77, 78, 79, 80, 81, 82, 83, 84, 85, 86,
			87, 88, 89, 90, 91, 92, 93
		}, readEffect: true, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[0], default(PresetItemTemplateId), new sbyte[6] { 0, 1, 2, 3, 4, 5 }, new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), 219, -1, 248, 676, new int[0], new int[0], new int[0], new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>(), new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(326, 677, EMapPickupsType.Growth, EMapPickupsType2.Loop, "map_eventicon_19", 677, 1, new byte[15]
		{
			3, 3, 3, 3, 3, 3, 3, 3, 3, 3,
			3, 3, 3, 3, 3
		}, new List<short>
		{
			94, 95, 96, 97, 98, 99, 100, 101, 102, 103,
			104, 105, 106, 107, 108
		}, readEffect: false, loopEffect: true, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[0], default(PresetItemTemplateId), new sbyte[6] { 0, 1, 2, 3, 4, 5 }, new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), 220, -1, 249, 678, new int[0], new int[0], new int[0], new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>(), new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(327, 679, EMapPickupsType.Growth, EMapPickupsType2.SpiritualDebt, "map_eventicon_17", 680, 0, new byte[15]
		{
			2, 2, 2, 2, 2, 2, 2, 2, 2, 2,
			2, 2, 2, 2, 2
		}, new List<short>
		{
			39, 40, 41, 42, 43, 44, 45, 46, 47, 54,
			55, 56, 48, 49, 50, 51, 52, 53, 1, 2,
			3, 4, 5, 6, 7, 8, 9, 10, 11, 12,
			13, 14, 15, 34, 35, 36
		}, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: true, new int[8] { 25, 50, 75, 125, 175, 225, 300, 375 }, new sbyte[0], default(PresetItemTemplateId), new sbyte[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), 221, 246, -1, 681, new int[0], new int[0], new int[0], new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>(), new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(328, 682, EMapPickupsType.Event, EMapPickupsType2.Invalid, "map_eventicon_18", 683, 0, new byte[15]
		{
			1, 0, 0, 0, 0, 0, 0, 0, 0, 0,
			0, 0, 0, 0, 0
		}, new List<short> { 19 }, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[0], default(PresetItemTemplateId), new sbyte[1], new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), -1, -1, -1, 684, new int[2] { 685, 686 }, new int[2] { 687, 688 }, new int[2] { 689, 689 }, new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>
		{
			new PresetItemWithCount("Accessory", 13, 1)
		}, new List<PresetItemWithCount>
		{
			new PresetItemWithCount("Accessory", 184, 1)
		}));
		_dataArray.Add(new MapPickupsItem(329, 690, EMapPickupsType.Event, EMapPickupsType2.Invalid, "map_eventicon_18", 691, 0, new byte[15]
		{
			1, 0, 0, 0, 0, 0, 0, 0, 0, 0,
			0, 0, 0, 0, 0
		}, new List<short> { 19 }, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[0], default(PresetItemTemplateId), new sbyte[1], new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), -1, -1, -1, 692, new int[2] { 693, 694 }, new int[2] { 695, 696 }, new int[2] { 689, 689 }, new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>
		{
			new PresetItemWithCount("Armor", 418, 1)
		}, new List<PresetItemWithCount>
		{
			new PresetItemWithCount("Armor", 400, 1),
			new PresetItemWithCount("Armor", 508, 1)
		}));
		_dataArray.Add(new MapPickupsItem(330, 697, EMapPickupsType.Event, EMapPickupsType2.Invalid, "map_eventicon_18", 698, 0, new byte[15]
		{
			1, 0, 0, 0, 0, 0, 0, 0, 0, 0,
			0, 0, 0, 0, 0
		}, new List<short> { 19 }, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[0], default(PresetItemTemplateId), new sbyte[1], new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), -1, -1, -1, 699, new int[2] { 700, 701 }, new int[2] { 702, 703 }, new int[2] { 689, 689 }, new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>
		{
			new PresetItemWithCount("Weapon", 286, 1)
		}, new List<PresetItemWithCount>
		{
			new PresetItemWithCount("Weapon", 655, 1),
			new PresetItemWithCount("Weapon", 664, 1),
			new PresetItemWithCount("Weapon", 682, 1),
			new PresetItemWithCount("Weapon", 700, 1)
		}));
		_dataArray.Add(new MapPickupsItem(331, 704, EMapPickupsType.Event, EMapPickupsType2.Invalid, "map_eventicon_18", 705, 0, new byte[15]
		{
			1, 0, 0, 0, 0, 0, 0, 0, 0, 0,
			0, 0, 0, 0, 0
		}, new List<short> { 19 }, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[0], default(PresetItemTemplateId), new sbyte[1], new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(1, 50), -1, -1, -1, 706, new int[2] { 707, 708 }, new int[2] { 709, 710 }, new int[2] { 689, 711 }, new List<PresetItemWithCount>(), new List<ResourceInfo>
		{
			default(ResourceInfo),
			new ResourceInfo(7, 1000)
		}, new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>
		{
			new PresetItemWithCount("Clothing", 18, 1)
		}, new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(332, 712, EMapPickupsType.Event, EMapPickupsType2.Invalid, "map_eventicon_18", 713, 0, new byte[15]
		{
			1, 0, 0, 0, 0, 0, 0, 0, 0, 0,
			0, 0, 0, 0, 0
		}, new List<short> { 19 }, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[0], default(PresetItemTemplateId), new sbyte[1], new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(1, 100), -1, -1, -1, 714, new int[2] { 715, 716 }, new int[2] { 717, 718 }, new int[2] { 689, 711 }, new List<PresetItemWithCount>(), new List<ResourceInfo>
		{
			default(ResourceInfo),
			new ResourceInfo(7, 2000)
		}, new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>
		{
			new PresetItemWithCount("Clothing", 19, 1)
		}, new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(333, 726, EMapPickupsType.Event, EMapPickupsType2.Invalid, "map_eventicon_18", 727, 0, new byte[15]
		{
			1, 0, 0, 0, 0, 0, 0, 0, 0, 0,
			0, 0, 0, 0, 0
		}, new List<short> { 19 }, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[0], default(PresetItemTemplateId), new sbyte[1], new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(1, 300), -1, -1, -1, 728, new int[2] { 729, 730 }, new int[2] { 731, 732 }, new int[2] { 689, 711 }, new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int> { -1, 5000 }, new List<PresetItemWithCount>
		{
			new PresetItemWithCount("SkillBook", 144, 1),
			new PresetItemWithCount("SkillBook", 145, 1),
			new PresetItemWithCount("SkillBook", 146, 1),
			new PresetItemWithCount("SkillBook", 250, 1),
			new PresetItemWithCount("SkillBook", 251, 1),
			new PresetItemWithCount("SkillBook", 252, 1),
			new PresetItemWithCount("SkillBook", 345, 1),
			new PresetItemWithCount("SkillBook", 346, 1),
			new PresetItemWithCount("SkillBook", 347, 1),
			new PresetItemWithCount("SkillBook", 465, 1),
			new PresetItemWithCount("SkillBook", 466, 1),
			new PresetItemWithCount("SkillBook", 467, 1),
			new PresetItemWithCount("SkillBook", 544, 1),
			new PresetItemWithCount("SkillBook", 545, 1),
			new PresetItemWithCount("SkillBook", 546, 1),
			new PresetItemWithCount("SkillBook", 767, 1),
			new PresetItemWithCount("SkillBook", 768, 1),
			new PresetItemWithCount("SkillBook", 769, 1)
		}, new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(334, 733, EMapPickupsType.Event, EMapPickupsType2.Invalid, "map_eventicon_18", 734, 0, new byte[15]
		{
			1, 0, 0, 0, 0, 0, 0, 0, 0, 0,
			0, 0, 0, 0, 0
		}, new List<short> { 19 }, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[0], default(PresetItemTemplateId), new sbyte[1], new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(1, 500), -1, -1, -1, 735, new int[2] { 736, 737 }, new int[2] { 738, 739 }, new int[2] { 689, 711 }, new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int> { -1, 10000 }, new List<PresetItemWithCount>
		{
			new PresetItemWithCount("SkillBook", 147, 1),
			new PresetItemWithCount("SkillBook", 148, 1),
			new PresetItemWithCount("SkillBook", 149, 1),
			new PresetItemWithCount("SkillBook", 253, 1),
			new PresetItemWithCount("SkillBook", 254, 1),
			new PresetItemWithCount("SkillBook", 255, 1),
			new PresetItemWithCount("SkillBook", 348, 1),
			new PresetItemWithCount("SkillBook", 349, 1),
			new PresetItemWithCount("SkillBook", 350, 1),
			new PresetItemWithCount("SkillBook", 468, 1),
			new PresetItemWithCount("SkillBook", 469, 1),
			new PresetItemWithCount("SkillBook", 470, 1),
			new PresetItemWithCount("SkillBook", 547, 1),
			new PresetItemWithCount("SkillBook", 548, 1),
			new PresetItemWithCount("SkillBook", 549, 1),
			new PresetItemWithCount("SkillBook", 770, 1),
			new PresetItemWithCount("SkillBook", 771, 1),
			new PresetItemWithCount("SkillBook", 772, 1)
		}, new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(335, 740, EMapPickupsType.Event, EMapPickupsType2.Invalid, "map_eventicon_18", 741, 0, new byte[15]
		{
			0, 1, 0, 0, 0, 0, 0, 0, 0, 0,
			0, 0, 0, 0, 0
		}, new List<short> { 19 }, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[0], default(PresetItemTemplateId), new sbyte[1], new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(1, 1000), -1, -1, -1, 721, new int[2] { 722, 742 }, new int[2] { 743, 744 }, new int[2] { 689, 711 }, new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int> { -1, 20000 }, new List<PresetItemWithCount>
		{
			new PresetItemWithCount("SkillBook", 150, 1),
			new PresetItemWithCount("SkillBook", 151, 1),
			new PresetItemWithCount("SkillBook", 351, 1),
			new PresetItemWithCount("SkillBook", 352, 1),
			new PresetItemWithCount("SkillBook", 471, 1),
			new PresetItemWithCount("SkillBook", 472, 1),
			new PresetItemWithCount("SkillBook", 773, 1),
			new PresetItemWithCount("SkillBook", 774, 1)
		}, new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(336, 745, EMapPickupsType.Event, EMapPickupsType2.Invalid, "map_eventicon_18", 746, 0, new byte[15]
		{
			0, 1, 0, 0, 0, 0, 0, 0, 0, 0,
			0, 0, 0, 0, 0
		}, new List<short> { 20 }, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[0], default(PresetItemTemplateId), new sbyte[1], new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), -1, -1, -1, 747, new int[2] { 748, 749 }, new int[2] { 750, 751 }, new int[2] { 689, 689 }, new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>
		{
			new PresetItemWithCount("Accessory", 121, 1)
		}, new List<PresetItemWithCount>
		{
			new PresetItemWithCount("Accessory", 211, 1)
		}));
		_dataArray.Add(new MapPickupsItem(337, 752, EMapPickupsType.Event, EMapPickupsType2.Invalid, "map_eventicon_18", 753, 0, new byte[15]
		{
			0, 1, 0, 0, 0, 0, 0, 0, 0, 0,
			0, 0, 0, 0, 0
		}, new List<short> { 20 }, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[0], default(PresetItemTemplateId), new sbyte[1], new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), -1, -1, -1, 754, new int[2] { 755, 756 }, new int[2] { 757, 758 }, new int[2] { 689, 689 }, new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>
		{
			new PresetItemWithCount("Armor", 346, 1),
			new PresetItemWithCount("Armor", 373, 1)
		}, new List<PresetItemWithCount>
		{
			new PresetItemWithCount("Armor", 355, 1),
			new PresetItemWithCount("Armor", 364, 1)
		}));
		_dataArray.Add(new MapPickupsItem(338, 759, EMapPickupsType.Event, EMapPickupsType2.Invalid, "map_eventicon_18", 760, 0, new byte[15]
		{
			0, 1, 0, 0, 0, 0, 0, 0, 0, 0,
			0, 0, 0, 0, 0
		}, new List<short> { 20 }, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[0], default(PresetItemTemplateId), new sbyte[1], new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), -1, -1, -1, 761, new int[2] { 762, 763 }, new int[2] { 764, 765 }, new int[2] { 689, 689 }, new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>
		{
			new PresetItemWithCount("Weapon", 331, 1),
			new PresetItemWithCount("Weapon", 151, 1)
		}, new List<PresetItemWithCount>
		{
			new PresetItemWithCount("Weapon", 484, 1),
			new PresetItemWithCount("Weapon", 466, 1),
			new PresetItemWithCount("Weapon", 439, 1)
		}));
		_dataArray.Add(new MapPickupsItem(339, 766, EMapPickupsType.Event, EMapPickupsType2.Invalid, "map_eventicon_18", 767, 0, new byte[15]
		{
			0, 1, 0, 0, 0, 0, 0, 0, 0, 0,
			0, 0, 0, 0, 0
		}, new List<short> { 20 }, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[0], default(PresetItemTemplateId), new sbyte[1], new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(2, 50), -1, -1, -1, 768, new int[2] { 707, 708 }, new int[2] { 709, 710 }, new int[2] { 689, 711 }, new List<PresetItemWithCount>(), new List<ResourceInfo>
		{
			default(ResourceInfo),
			new ResourceInfo(7, 1000)
		}, new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>
		{
			new PresetItemWithCount("Clothing", 21, 1)
		}, new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(340, 769, EMapPickupsType.Event, EMapPickupsType2.Invalid, "map_eventicon_18", 770, 0, new byte[15]
		{
			0, 1, 0, 0, 0, 0, 0, 0, 0, 0,
			0, 0, 0, 0, 0
		}, new List<short> { 20 }, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[0], default(PresetItemTemplateId), new sbyte[1], new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(2, 100), -1, -1, -1, 771, new int[2] { 772, 773 }, new int[2] { 774, 775 }, new int[2] { 689, 711 }, new List<PresetItemWithCount>(), new List<ResourceInfo>
		{
			default(ResourceInfo),
			new ResourceInfo(7, 2000)
		}, new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>
		{
			new PresetItemWithCount("Clothing", 22, 1)
		}, new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(341, 783, EMapPickupsType.Event, EMapPickupsType2.Invalid, "map_eventicon_18", 784, 0, new byte[15]
		{
			0, 1, 0, 0, 0, 0, 0, 0, 0, 0,
			0, 0, 0, 0, 0
		}, new List<short> { 20 }, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[0], default(PresetItemTemplateId), new sbyte[1], new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(2, 300), -1, -1, -1, 785, new int[2] { 786, 787 }, new int[2] { 788, 789 }, new int[2] { 689, 711 }, new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int> { -1, 5000 }, new List<PresetItemWithCount>
		{
			new PresetItemWithCount("SkillBook", 153, 1),
			new PresetItemWithCount("SkillBook", 154, 1),
			new PresetItemWithCount("SkillBook", 155, 1),
			new PresetItemWithCount("SkillBook", 256, 1),
			new PresetItemWithCount("SkillBook", 257, 1),
			new PresetItemWithCount("SkillBook", 258, 1),
			new PresetItemWithCount("SkillBook", 354, 1),
			new PresetItemWithCount("SkillBook", 355, 1),
			new PresetItemWithCount("SkillBook", 356, 1),
			new PresetItemWithCount("SkillBook", 473, 1),
			new PresetItemWithCount("SkillBook", 474, 1),
			new PresetItemWithCount("SkillBook", 475, 1),
			new PresetItemWithCount("SkillBook", 550, 1),
			new PresetItemWithCount("SkillBook", 551, 1),
			new PresetItemWithCount("SkillBook", 552, 1),
			new PresetItemWithCount("SkillBook", 669, 1),
			new PresetItemWithCount("SkillBook", 670, 1),
			new PresetItemWithCount("SkillBook", 671, 1),
			new PresetItemWithCount("SkillBook", 792, 1),
			new PresetItemWithCount("SkillBook", 793, 1),
			new PresetItemWithCount("SkillBook", 794, 1)
		}, new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(342, 790, EMapPickupsType.Event, EMapPickupsType2.Invalid, "map_eventicon_18", 791, 0, new byte[15]
		{
			0, 1, 0, 0, 0, 0, 0, 0, 0, 0,
			0, 0, 0, 0, 0
		}, new List<short> { 20 }, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[0], default(PresetItemTemplateId), new sbyte[1], new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(2, 500), -1, -1, -1, 792, new int[2] { 793, 794 }, new int[2] { 795, 796 }, new int[2] { 689, 711 }, new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int> { -1, 10000 }, new List<PresetItemWithCount>
		{
			new PresetItemWithCount("SkillBook", 156, 1),
			new PresetItemWithCount("SkillBook", 157, 1),
			new PresetItemWithCount("SkillBook", 158, 1),
			new PresetItemWithCount("SkillBook", 259, 1),
			new PresetItemWithCount("SkillBook", 260, 1),
			new PresetItemWithCount("SkillBook", 261, 1),
			new PresetItemWithCount("SkillBook", 357, 1),
			new PresetItemWithCount("SkillBook", 358, 1),
			new PresetItemWithCount("SkillBook", 359, 1),
			new PresetItemWithCount("SkillBook", 476, 1),
			new PresetItemWithCount("SkillBook", 477, 1),
			new PresetItemWithCount("SkillBook", 478, 1),
			new PresetItemWithCount("SkillBook", 553, 1),
			new PresetItemWithCount("SkillBook", 554, 1),
			new PresetItemWithCount("SkillBook", 555, 1),
			new PresetItemWithCount("SkillBook", 672, 1),
			new PresetItemWithCount("SkillBook", 673, 1),
			new PresetItemWithCount("SkillBook", 674, 1),
			new PresetItemWithCount("SkillBook", 795, 1),
			new PresetItemWithCount("SkillBook", 796, 1),
			new PresetItemWithCount("SkillBook", 797, 1)
		}, new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(343, 797, EMapPickupsType.Event, EMapPickupsType2.Invalid, "map_eventicon_18", 798, 0, new byte[15]
		{
			0, 1, 0, 0, 0, 0, 0, 0, 0, 0,
			0, 0, 0, 0, 0
		}, new List<short> { 20 }, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[0], default(PresetItemTemplateId), new sbyte[1], new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(2, 1000), -1, -1, -1, 799, new int[2] { 800, 801 }, new int[2] { 802, 803 }, new int[2] { 689, 711 }, new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int> { -1, 20000 }, new List<PresetItemWithCount>
		{
			new PresetItemWithCount("SkillBook", 159, 1),
			new PresetItemWithCount("SkillBook", 160, 1),
			new PresetItemWithCount("SkillBook", 262, 1),
			new PresetItemWithCount("SkillBook", 360, 1),
			new PresetItemWithCount("SkillBook", 361, 1),
			new PresetItemWithCount("SkillBook", 479, 1),
			new PresetItemWithCount("SkillBook", 556, 1),
			new PresetItemWithCount("SkillBook", 557, 1),
			new PresetItemWithCount("SkillBook", 675, 1),
			new PresetItemWithCount("SkillBook", 798, 1),
			new PresetItemWithCount("SkillBook", 799, 1)
		}, new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(344, 804, EMapPickupsType.Event, EMapPickupsType2.Invalid, "map_eventicon_18", 805, 0, new byte[15]
		{
			0, 0, 1, 0, 0, 0, 0, 0, 0, 0,
			0, 0, 0, 0, 0
		}, new List<short> { 21 }, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[0], default(PresetItemTemplateId), new sbyte[1], new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), -1, -1, -1, 806, new int[2] { 807, 808 }, new int[2] { 809, 810 }, new int[2] { 689, 689 }, new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>
		{
			new PresetItemWithCount("Accessory", 130, 1)
		}, new List<PresetItemWithCount>
		{
			new PresetItemWithCount("Accessory", 175, 1)
		}));
		_dataArray.Add(new MapPickupsItem(345, 811, EMapPickupsType.Event, EMapPickupsType2.Invalid, "map_eventicon_18", 812, 0, new byte[15]
		{
			0, 0, 1, 0, 0, 0, 0, 0, 0, 0,
			0, 0, 0, 0, 0
		}, new List<short> { 21 }, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[0], default(PresetItemTemplateId), new sbyte[1], new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), -1, -1, -1, 813, new int[2] { 814, 815 }, new int[2] { 816, 817 }, new int[2] { 689, 689 }, new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>
		{
			new PresetItemWithCount("Armor", 472, 1),
			new PresetItemWithCount("Armor", 481, 1)
		}, new List<PresetItemWithCount>
		{
			new PresetItemWithCount("Armor", 490, 1),
			new PresetItemWithCount("Armor", 499, 1)
		}));
		_dataArray.Add(new MapPickupsItem(346, 818, EMapPickupsType.Event, EMapPickupsType2.Invalid, "map_eventicon_18", 819, 0, new byte[15]
		{
			0, 0, 1, 0, 0, 0, 0, 0, 0, 0,
			0, 0, 0, 0, 0
		}, new List<short> { 21 }, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[0], default(PresetItemTemplateId), new sbyte[1], new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), -1, -1, -1, 820, new int[2] { 821, 822 }, new int[2] { 823, 824 }, new int[2] { 689, 689 }, new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>
		{
			new PresetItemWithCount("Weapon", 88, 1),
			new PresetItemWithCount("Weapon", 97, 1),
			new PresetItemWithCount("Weapon", 106, 1)
		}, new List<PresetItemWithCount>
		{
			new PresetItemWithCount("Weapon", 259, 1),
			new PresetItemWithCount("Weapon", 367, 1)
		}));
		_dataArray.Add(new MapPickupsItem(347, 825, EMapPickupsType.Event, EMapPickupsType2.Invalid, "map_eventicon_18", 826, 0, new byte[15]
		{
			0, 0, 1, 0, 0, 0, 0, 0, 0, 0,
			0, 0, 0, 0, 0
		}, new List<short> { 21 }, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[0], default(PresetItemTemplateId), new sbyte[1], new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(3, 50), -1, -1, -1, 827, new int[2] { 707, 708 }, new int[2] { 709, 710 }, new int[2] { 689, 711 }, new List<PresetItemWithCount>(), new List<ResourceInfo>
		{
			default(ResourceInfo),
			new ResourceInfo(7, 1000)
		}, new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>
		{
			new PresetItemWithCount("Clothing", 24, 1)
		}, new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(348, 828, EMapPickupsType.Event, EMapPickupsType2.Invalid, "map_eventicon_18", 829, 0, new byte[15]
		{
			0, 0, 1, 0, 0, 0, 0, 0, 0, 0,
			0, 0, 0, 0, 0
		}, new List<short> { 21 }, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[0], default(PresetItemTemplateId), new sbyte[1], new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(3, 100), -1, -1, -1, 830, new int[2] { 831, 773 }, new int[3] { 832, 833, 834 }, new int[2] { 689, 711 }, new List<PresetItemWithCount>(), new List<ResourceInfo>
		{
			default(ResourceInfo),
			new ResourceInfo(7, 2000)
		}, new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>
		{
			new PresetItemWithCount("Clothing", 25, 1)
		}, new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(349, 842, EMapPickupsType.Event, EMapPickupsType2.Invalid, "map_eventicon_18", 843, 0, new byte[15]
		{
			0, 0, 1, 0, 0, 0, 0, 0, 0, 0,
			0, 0, 0, 0, 0
		}, new List<short> { 21 }, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[0], default(PresetItemTemplateId), new sbyte[1], new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(3, 300), -1, -1, -1, 844, new int[2] { 845, 846 }, new int[2] { 847, 848 }, new int[2] { 689, 711 }, new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int> { -1, 5000 }, new List<PresetItemWithCount>
		{
			new PresetItemWithCount("SkillBook", 162, 1),
			new PresetItemWithCount("SkillBook", 163, 1),
			new PresetItemWithCount("SkillBook", 263, 1),
			new PresetItemWithCount("SkillBook", 264, 1),
			new PresetItemWithCount("SkillBook", 265, 1),
			new PresetItemWithCount("SkillBook", 363, 1),
			new PresetItemWithCount("SkillBook", 364, 1),
			new PresetItemWithCount("SkillBook", 365, 1),
			new PresetItemWithCount("SkillBook", 558, 1),
			new PresetItemWithCount("SkillBook", 559, 1),
			new PresetItemWithCount("SkillBook", 560, 1),
			new PresetItemWithCount("SkillBook", 834, 1),
			new PresetItemWithCount("SkillBook", 835, 1),
			new PresetItemWithCount("SkillBook", 836, 1),
			new PresetItemWithCount("SkillBook", 851, 1),
			new PresetItemWithCount("SkillBook", 852, 1),
			new PresetItemWithCount("SkillBook", 853, 1)
		}, new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(350, 849, EMapPickupsType.Event, EMapPickupsType2.Invalid, "map_eventicon_18", 850, 0, new byte[15]
		{
			0, 0, 1, 0, 0, 0, 0, 0, 0, 0,
			0, 0, 0, 0, 0
		}, new List<short> { 21 }, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[0], default(PresetItemTemplateId), new sbyte[1], new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(3, 500), -1, -1, -1, 851, new int[2] { 852, 853 }, new int[2] { 854, 855 }, new int[2] { 689, 711 }, new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int> { -1, 10000 }, new List<PresetItemWithCount>
		{
			new PresetItemWithCount("SkillBook", 164, 1),
			new PresetItemWithCount("SkillBook", 165, 1),
			new PresetItemWithCount("SkillBook", 266, 1),
			new PresetItemWithCount("SkillBook", 267, 1),
			new PresetItemWithCount("SkillBook", 268, 1),
			new PresetItemWithCount("SkillBook", 366, 1),
			new PresetItemWithCount("SkillBook", 367, 1),
			new PresetItemWithCount("SkillBook", 368, 1),
			new PresetItemWithCount("SkillBook", 561, 1),
			new PresetItemWithCount("SkillBook", 562, 1),
			new PresetItemWithCount("SkillBook", 563, 1),
			new PresetItemWithCount("SkillBook", 837, 1),
			new PresetItemWithCount("SkillBook", 838, 1),
			new PresetItemWithCount("SkillBook", 839, 1),
			new PresetItemWithCount("SkillBook", 854, 1),
			new PresetItemWithCount("SkillBook", 855, 1),
			new PresetItemWithCount("SkillBook", 856, 1)
		}, new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(351, 856, EMapPickupsType.Event, EMapPickupsType2.Invalid, "map_eventicon_18", 857, 0, new byte[15]
		{
			0, 0, 1, 0, 0, 0, 0, 0, 0, 0,
			0, 0, 0, 0, 0
		}, new List<short> { 21 }, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[0], default(PresetItemTemplateId), new sbyte[1], new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(3, 1000), -1, -1, -1, 858, new int[2] { 859, 860 }, new int[2] { 861, 862 }, new int[2] { 689, 711 }, new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int> { -1, 20000 }, new List<PresetItemWithCount>
		{
			new PresetItemWithCount("SkillBook", 269, 1),
			new PresetItemWithCount("SkillBook", 270, 1),
			new PresetItemWithCount("SkillBook", 369, 1),
			new PresetItemWithCount("SkillBook", 370, 1),
			new PresetItemWithCount("SkillBook", 564, 1),
			new PresetItemWithCount("SkillBook", 565, 1),
			new PresetItemWithCount("SkillBook", 840, 1),
			new PresetItemWithCount("SkillBook", 841, 1),
			new PresetItemWithCount("SkillBook", 857, 1)
		}, new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(352, 863, EMapPickupsType.Event, EMapPickupsType2.Invalid, "map_eventicon_18", 864, 0, new byte[15]
		{
			0, 0, 0, 1, 0, 0, 0, 0, 0, 0,
			0, 0, 0, 0, 0
		}, new List<short> { 22 }, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[0], default(PresetItemTemplateId), new sbyte[1], new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), -1, -1, -1, 865, new int[2] { 866, 867 }, new int[2] { 868, 869 }, new int[2] { 689, 689 }, new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>
		{
			new PresetItemWithCount("Accessory", 220, 1)
		}, new List<PresetItemWithCount>
		{
			new PresetItemWithCount("Accessory", 175, 1)
		}));
		_dataArray.Add(new MapPickupsItem(353, 870, EMapPickupsType.Event, EMapPickupsType2.Invalid, "map_eventicon_18", 871, 0, new byte[15]
		{
			0, 0, 0, 1, 0, 0, 0, 0, 0, 0,
			0, 0, 0, 0, 0
		}, new List<short> { 22 }, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[0], default(PresetItemTemplateId), new sbyte[1], new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), -1, -1, -1, 872, new int[2] { 873, 874 }, new int[2] { 875, 876 }, new int[2] { 689, 689 }, new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>
		{
			new PresetItemWithCount("Armor", 40, 1),
			new PresetItemWithCount("Armor", 49, 1)
		}, new List<PresetItemWithCount>
		{
			new PresetItemWithCount("Armor", 58, 1),
			new PresetItemWithCount("Armor", 67, 1)
		}));
		_dataArray.Add(new MapPickupsItem(354, 877, EMapPickupsType.Event, EMapPickupsType2.Invalid, "map_eventicon_18", 878, 0, new byte[15]
		{
			0, 0, 0, 1, 0, 0, 0, 0, 0, 0,
			0, 0, 0, 0, 0
		}, new List<short> { 22 }, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[0], default(PresetItemTemplateId), new sbyte[1], new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), -1, -1, -1, 879, new int[2] { 880, 881 }, new int[2] { 882, 883 }, new int[2] { 689, 689 }, new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>
		{
			new PresetItemWithCount("Weapon", 493, 1),
			new PresetItemWithCount("Weapon", 484, 1),
			new PresetItemWithCount("Weapon", 457, 1),
			new PresetItemWithCount("Weapon", 439, 1)
		}, new List<PresetItemWithCount>
		{
			new PresetItemWithCount("Weapon", 790, 1),
			new PresetItemWithCount("Weapon", 781, 1),
			new PresetItemWithCount("Weapon", 358, 1)
		}));
		_dataArray.Add(new MapPickupsItem(355, 884, EMapPickupsType.Event, EMapPickupsType2.Invalid, "map_eventicon_18", 885, 0, new byte[15]
		{
			0, 0, 0, 1, 0, 0, 0, 0, 0, 0,
			0, 0, 0, 0, 0
		}, new List<short> { 22 }, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[0], default(PresetItemTemplateId), new sbyte[1], new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(4, 50), -1, -1, -1, 886, new int[2] { 707, 708 }, new int[2] { 709, 710 }, new int[2] { 689, 711 }, new List<PresetItemWithCount>(), new List<ResourceInfo>
		{
			default(ResourceInfo),
			new ResourceInfo(7, 1000)
		}, new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>
		{
			new PresetItemWithCount("Clothing", 27, 1)
		}, new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(356, 887, EMapPickupsType.Event, EMapPickupsType2.Invalid, "map_eventicon_18", 888, 0, new byte[15]
		{
			0, 0, 0, 1, 0, 0, 0, 0, 0, 0,
			0, 0, 0, 0, 0
		}, new List<short> { 22 }, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[0], default(PresetItemTemplateId), new sbyte[1], new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(4, 100), -1, -1, -1, 889, new int[2] { 890, 891 }, new int[2] { 892, 893 }, new int[2] { 689, 711 }, new List<PresetItemWithCount>(), new List<ResourceInfo>
		{
			default(ResourceInfo),
			new ResourceInfo(7, 2000)
		}, new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>
		{
			new PresetItemWithCount("Clothing", 28, 1)
		}, new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(357, 901, EMapPickupsType.Event, EMapPickupsType2.Invalid, "map_eventicon_18", 902, 0, new byte[15]
		{
			0, 0, 0, 1, 0, 0, 0, 0, 0, 0,
			0, 0, 0, 0, 0
		}, new List<short> { 22 }, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[0], default(PresetItemTemplateId), new sbyte[1], new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(4, 300), -1, -1, -1, 903, new int[2] { 904, 905 }, new int[2] { 906, 907 }, new int[2] { 689, 711 }, new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int> { -1, 5000 }, new List<PresetItemWithCount>
		{
			new PresetItemWithCount("SkillBook", 166, 1),
			new PresetItemWithCount("SkillBook", 167, 1),
			new PresetItemWithCount("SkillBook", 168, 1),
			new PresetItemWithCount("SkillBook", 271, 1),
			new PresetItemWithCount("SkillBook", 272, 1),
			new PresetItemWithCount("SkillBook", 273, 1),
			new PresetItemWithCount("SkillBook", 372, 1),
			new PresetItemWithCount("SkillBook", 373, 1),
			new PresetItemWithCount("SkillBook", 374, 1),
			new PresetItemWithCount("SkillBook", 480, 1),
			new PresetItemWithCount("SkillBook", 481, 1),
			new PresetItemWithCount("SkillBook", 482, 1),
			new PresetItemWithCount("SkillBook", 676, 1),
			new PresetItemWithCount("SkillBook", 677, 1),
			new PresetItemWithCount("SkillBook", 678, 1),
			new PresetItemWithCount("SkillBook", 817, 1),
			new PresetItemWithCount("SkillBook", 818, 1),
			new PresetItemWithCount("SkillBook", 819, 1)
		}, new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(358, 908, EMapPickupsType.Event, EMapPickupsType2.Invalid, "map_eventicon_18", 909, 0, new byte[15]
		{
			0, 0, 0, 1, 0, 0, 0, 0, 0, 0,
			0, 0, 0, 0, 0
		}, new List<short> { 22 }, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[0], default(PresetItemTemplateId), new sbyte[1], new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(4, 500), -1, -1, -1, 910, new int[2] { 911, 912 }, new int[2] { 913, 914 }, new int[2] { 689, 711 }, new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int> { -1, 10000 }, new List<PresetItemWithCount>
		{
			new PresetItemWithCount("SkillBook", 169, 1),
			new PresetItemWithCount("SkillBook", 170, 1),
			new PresetItemWithCount("SkillBook", 171, 1),
			new PresetItemWithCount("SkillBook", 274, 1),
			new PresetItemWithCount("SkillBook", 275, 1),
			new PresetItemWithCount("SkillBook", 276, 1),
			new PresetItemWithCount("SkillBook", 375, 1),
			new PresetItemWithCount("SkillBook", 376, 1),
			new PresetItemWithCount("SkillBook", 377, 1),
			new PresetItemWithCount("SkillBook", 483, 1),
			new PresetItemWithCount("SkillBook", 484, 1),
			new PresetItemWithCount("SkillBook", 485, 1),
			new PresetItemWithCount("SkillBook", 679, 1),
			new PresetItemWithCount("SkillBook", 680, 1),
			new PresetItemWithCount("SkillBook", 681, 1),
			new PresetItemWithCount("SkillBook", 820, 1),
			new PresetItemWithCount("SkillBook", 821, 1),
			new PresetItemWithCount("SkillBook", 822, 1)
		}, new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(359, 915, EMapPickupsType.Event, EMapPickupsType2.Invalid, "map_eventicon_18", 916, 0, new byte[15]
		{
			0, 0, 0, 1, 0, 0, 0, 0, 0, 0,
			0, 0, 0, 0, 0
		}, new List<short> { 22 }, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[0], default(PresetItemTemplateId), new sbyte[1], new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(4, 1000), -1, -1, -1, 917, new int[2] { 918, 919 }, new int[2] { 920, 921 }, new int[2] { 689, 711 }, new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int> { -1, 20000 }, new List<PresetItemWithCount>
		{
			new PresetItemWithCount("SkillBook", 172, 1),
			new PresetItemWithCount("SkillBook", 173, 1),
			new PresetItemWithCount("SkillBook", 277, 1),
			new PresetItemWithCount("SkillBook", 378, 1),
			new PresetItemWithCount("SkillBook", 379, 1),
			new PresetItemWithCount("SkillBook", 486, 1),
			new PresetItemWithCount("SkillBook", 487, 1),
			new PresetItemWithCount("SkillBook", 682, 1),
			new PresetItemWithCount("SkillBook", 683, 1),
			new PresetItemWithCount("SkillBook", 823, 1),
			new PresetItemWithCount("SkillBook", 824, 1)
		}, new List<PresetItemWithCount>()));
	}

	private void CreateItems6()
	{
		_dataArray.Add(new MapPickupsItem(360, 922, EMapPickupsType.Event, EMapPickupsType2.Invalid, "map_eventicon_18", 923, 0, new byte[15]
		{
			0, 0, 0, 0, 1, 0, 0, 0, 0, 0,
			0, 0, 0, 0, 0
		}, new List<short> { 23 }, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[0], default(PresetItemTemplateId), new sbyte[1], new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), -1, -1, -1, 924, new int[2] { 925, 926 }, new int[2] { 927, 928 }, new int[2] { 689, 689 }, new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>
		{
			new PresetItemWithCount("Accessory", 157, 1)
		}, new List<PresetItemWithCount>
		{
			new PresetItemWithCount("Accessory", 184, 1)
		}));
		_dataArray.Add(new MapPickupsItem(361, 929, EMapPickupsType.Event, EMapPickupsType2.Invalid, "map_eventicon_18", 930, 0, new byte[15]
		{
			0, 0, 0, 0, 1, 0, 0, 0, 0, 0,
			0, 0, 0, 0, 0
		}, new List<short> { 23 }, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[0], default(PresetItemTemplateId), new sbyte[1], new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), -1, -1, -1, 931, new int[2] { 932, 933 }, new int[2] { 934, 935 }, new int[2] { 689, 689 }, new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>
		{
			new PresetItemWithCount("Armor", 238, 1),
			new PresetItemWithCount("Armor", 139, 1),
			new PresetItemWithCount("Armor", 130, 1)
		}, new List<PresetItemWithCount>
		{
			new PresetItemWithCount("Armor", 157, 1),
			new PresetItemWithCount("Armor", 148, 1)
		}));
		_dataArray.Add(new MapPickupsItem(362, 936, EMapPickupsType.Event, EMapPickupsType2.Invalid, "map_eventicon_18", 937, 0, new byte[15]
		{
			0, 0, 0, 0, 1, 0, 0, 0, 0, 0,
			0, 0, 0, 0, 0
		}, new List<short> { 23 }, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[0], default(PresetItemTemplateId), new sbyte[1], new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), -1, -1, -1, 938, new int[2] { 939, 940 }, new int[2] { 941, 942 }, new int[2] { 689, 689 }, new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>
		{
			new PresetItemWithCount("Weapon", 529, 1),
			new PresetItemWithCount("Weapon", 574, 1),
			new PresetItemWithCount("Weapon", 583, 1),
			new PresetItemWithCount("Weapon", 592, 1)
		}, new List<PresetItemWithCount>
		{
			new PresetItemWithCount("Weapon", 457, 1),
			new PresetItemWithCount("Weapon", 475, 1),
			new PresetItemWithCount("Weapon", 502, 1)
		}));
		_dataArray.Add(new MapPickupsItem(363, 943, EMapPickupsType.Event, EMapPickupsType2.Invalid, "map_eventicon_18", 944, 0, new byte[15]
		{
			0, 0, 0, 0, 1, 0, 0, 0, 0, 0,
			0, 0, 0, 0, 0
		}, new List<short> { 23 }, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[0], default(PresetItemTemplateId), new sbyte[1], new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(5, 50), -1, -1, -1, 945, new int[2] { 707, 708 }, new int[2] { 709, 710 }, new int[2] { 689, 711 }, new List<PresetItemWithCount>(), new List<ResourceInfo>
		{
			default(ResourceInfo),
			new ResourceInfo(7, 1000)
		}, new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>
		{
			new PresetItemWithCount("Clothing", 31, 1)
		}, new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(364, 946, EMapPickupsType.Event, EMapPickupsType2.Invalid, "map_eventicon_18", 947, 0, new byte[15]
		{
			0, 0, 0, 0, 1, 0, 0, 0, 0, 0,
			0, 0, 0, 0, 0
		}, new List<short> { 23 }, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[0], default(PresetItemTemplateId), new sbyte[1], new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(5, 100), -1, -1, -1, 948, new int[2] { 949, 950 }, new int[2] { 951, 952 }, new int[2] { 689, 711 }, new List<PresetItemWithCount>(), new List<ResourceInfo>
		{
			default(ResourceInfo),
			new ResourceInfo(7, 2000)
		}, new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>
		{
			new PresetItemWithCount("Clothing", 32, 1)
		}, new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(365, 960, EMapPickupsType.Event, EMapPickupsType2.Invalid, "map_eventicon_18", 961, 0, new byte[15]
		{
			0, 0, 0, 0, 1, 0, 0, 0, 0, 0,
			0, 0, 0, 0, 0
		}, new List<short> { 23 }, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[0], default(PresetItemTemplateId), new sbyte[1], new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(5, 300), -1, -1, -1, 962, new int[2] { 963, 964 }, new int[2] { 965, 966 }, new int[2] { 689, 711 }, new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int> { -1, 5000 }, new List<PresetItemWithCount>
		{
			new PresetItemWithCount("SkillBook", 174, 1),
			new PresetItemWithCount("SkillBook", 175, 1),
			new PresetItemWithCount("SkillBook", 176, 1),
			new PresetItemWithCount("SkillBook", 278, 1),
			new PresetItemWithCount("SkillBook", 279, 1),
			new PresetItemWithCount("SkillBook", 280, 1),
			new PresetItemWithCount("SkillBook", 380, 1),
			new PresetItemWithCount("SkillBook", 381, 1),
			new PresetItemWithCount("SkillBook", 382, 1),
			new PresetItemWithCount("SkillBook", 613, 1),
			new PresetItemWithCount("SkillBook", 614, 1),
			new PresetItemWithCount("SkillBook", 615, 1),
			new PresetItemWithCount("SkillBook", 685, 1),
			new PresetItemWithCount("SkillBook", 686, 1),
			new PresetItemWithCount("SkillBook", 687, 1),
			new PresetItemWithCount("SkillBook", 725, 1),
			new PresetItemWithCount("SkillBook", 726, 1),
			new PresetItemWithCount("SkillBook", 727, 1)
		}, new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(366, 967, EMapPickupsType.Event, EMapPickupsType2.Invalid, "map_eventicon_18", 968, 0, new byte[15]
		{
			0, 0, 0, 0, 1, 0, 0, 0, 0, 0,
			0, 0, 0, 0, 0
		}, new List<short> { 23 }, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[0], default(PresetItemTemplateId), new sbyte[1], new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(5, 500), -1, -1, -1, 969, new int[2] { 970, 971 }, new int[2] { 972, 973 }, new int[2] { 689, 711 }, new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int> { -1, 10000 }, new List<PresetItemWithCount>
		{
			new PresetItemWithCount("SkillBook", 177, 1),
			new PresetItemWithCount("SkillBook", 178, 1),
			new PresetItemWithCount("SkillBook", 281, 1),
			new PresetItemWithCount("SkillBook", 383, 1),
			new PresetItemWithCount("SkillBook", 384, 1),
			new PresetItemWithCount("SkillBook", 385, 1),
			new PresetItemWithCount("SkillBook", 616, 1),
			new PresetItemWithCount("SkillBook", 617, 1),
			new PresetItemWithCount("SkillBook", 618, 1),
			new PresetItemWithCount("SkillBook", 688, 1),
			new PresetItemWithCount("SkillBook", 689, 1),
			new PresetItemWithCount("SkillBook", 690, 1),
			new PresetItemWithCount("SkillBook", 728, 1),
			new PresetItemWithCount("SkillBook", 729, 1),
			new PresetItemWithCount("SkillBook", 730, 1)
		}, new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(367, 974, EMapPickupsType.Event, EMapPickupsType2.Invalid, "map_eventicon_18", 975, 0, new byte[15]
		{
			0, 0, 0, 0, 1, 0, 0, 0, 0, 0,
			0, 0, 0, 0, 0
		}, new List<short> { 23 }, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[0], default(PresetItemTemplateId), new sbyte[1], new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(5, 1000), -1, -1, -1, 976, new int[2] { 977, 978 }, new int[2] { 979, 980 }, new int[2] { 689, 711 }, new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int> { -1, 20000 }, new List<PresetItemWithCount>
		{
			new PresetItemWithCount("SkillBook", 386, 1),
			new PresetItemWithCount("SkillBook", 619, 1),
			new PresetItemWithCount("SkillBook", 620, 1),
			new PresetItemWithCount("SkillBook", 691, 1),
			new PresetItemWithCount("SkillBook", 692, 1),
			new PresetItemWithCount("SkillBook", 731, 1),
			new PresetItemWithCount("SkillBook", 732, 1)
		}, new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(368, 981, EMapPickupsType.Event, EMapPickupsType2.Invalid, "map_eventicon_18", 982, 0, new byte[15]
		{
			0, 0, 0, 0, 0, 1, 0, 0, 0, 0,
			0, 0, 0, 0, 0
		}, new List<short> { 24 }, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[0], default(PresetItemTemplateId), new sbyte[1], new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), -1, -1, -1, 983, new int[2] { 984, 985 }, new int[2] { 986, 987 }, new int[2] { 689, 689 }, new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>
		{
			new PresetItemWithCount("Accessory", 103, 1)
		}, new List<PresetItemWithCount>
		{
			new PresetItemWithCount("Accessory", 157, 1)
		}));
		_dataArray.Add(new MapPickupsItem(369, 988, EMapPickupsType.Event, EMapPickupsType2.Invalid, "map_eventicon_18", 989, 0, new byte[15]
		{
			0, 0, 0, 0, 0, 1, 0, 0, 0, 0,
			0, 0, 0, 0, 0
		}, new List<short> { 24 }, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[0], default(PresetItemTemplateId), new sbyte[1], new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), -1, -1, -1, 990, new int[2] { 991, 992 }, new int[2] { 993, 994 }, new int[2] { 689, 689 }, new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>
		{
			new PresetItemWithCount("Armor", 256, 1),
			new PresetItemWithCount("Armor", 274, 1)
		}, new List<PresetItemWithCount>
		{
			new PresetItemWithCount("Armor", 391, 1),
			new PresetItemWithCount("Armor", 382, 1)
		}));
		_dataArray.Add(new MapPickupsItem(370, 995, EMapPickupsType.Event, EMapPickupsType2.Invalid, "map_eventicon_18", 996, 0, new byte[15]
		{
			0, 0, 0, 0, 0, 1, 0, 0, 0, 0,
			0, 0, 0, 0, 0
		}, new List<short> { 24 }, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[0], default(PresetItemTemplateId), new sbyte[1], new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), -1, -1, -1, 997, new int[2] { 970, 998 }, new int[2] { 999, 1000 }, new int[2] { 689, 689 }, new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>
		{
			new PresetItemWithCount("Weapon", 646, 1),
			new PresetItemWithCount("Weapon", 628, 1),
			new PresetItemWithCount("Weapon", 286, 1)
		}, new List<PresetItemWithCount>
		{
			new PresetItemWithCount("Weapon", 565, 1),
			new PresetItemWithCount("Weapon", 556, 1),
			new PresetItemWithCount("Weapon", 547, 1),
			new PresetItemWithCount("Weapon", 529, 1)
		}));
		_dataArray.Add(new MapPickupsItem(371, 1001, EMapPickupsType.Event, EMapPickupsType2.Invalid, "map_eventicon_18", 1002, 0, new byte[15]
		{
			0, 0, 0, 0, 0, 1, 0, 0, 0, 0,
			0, 0, 0, 0, 0
		}, new List<short> { 24 }, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[0], default(PresetItemTemplateId), new sbyte[1], new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(6, 50), -1, -1, -1, 1003, new int[2] { 707, 708 }, new int[2] { 709, 710 }, new int[2] { 689, 711 }, new List<PresetItemWithCount>(), new List<ResourceInfo>
		{
			default(ResourceInfo),
			new ResourceInfo(7, 1000)
		}, new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>
		{
			new PresetItemWithCount("Clothing", 34, 1)
		}, new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(372, 1004, EMapPickupsType.Event, EMapPickupsType2.Invalid, "map_eventicon_18", 1005, 0, new byte[15]
		{
			0, 0, 0, 0, 0, 1, 0, 0, 0, 0,
			0, 0, 0, 0, 0
		}, new List<short> { 24 }, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[0], default(PresetItemTemplateId), new sbyte[1], new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(6, 100), -1, -1, -1, 1006, new int[2] { 1007, 1008 }, new int[2] { 1009, 1010 }, new int[2] { 689, 711 }, new List<PresetItemWithCount>(), new List<ResourceInfo>
		{
			default(ResourceInfo),
			new ResourceInfo(7, 2000)
		}, new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>
		{
			new PresetItemWithCount("Clothing", 35, 1)
		}, new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(373, 1017, EMapPickupsType.Event, EMapPickupsType2.Invalid, "map_eventicon_18", 1018, 0, new byte[15]
		{
			0, 0, 0, 0, 0, 1, 0, 0, 0, 0,
			0, 0, 0, 0, 0
		}, new List<short> { 24 }, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[0], default(PresetItemTemplateId), new sbyte[1], new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(6, 300), -1, -1, -1, 1019, new int[2] { 1020, 1021 }, new int[2] { 1022, 1023 }, new int[2] { 689, 711 }, new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int> { -1, 5000 }, new List<PresetItemWithCount>
		{
			new PresetItemWithCount("SkillBook", 179, 1),
			new PresetItemWithCount("SkillBook", 180, 1),
			new PresetItemWithCount("SkillBook", 181, 1),
			new PresetItemWithCount("SkillBook", 282, 1),
			new PresetItemWithCount("SkillBook", 283, 1),
			new PresetItemWithCount("SkillBook", 284, 1),
			new PresetItemWithCount("SkillBook", 387, 1),
			new PresetItemWithCount("SkillBook", 388, 1),
			new PresetItemWithCount("SkillBook", 389, 1),
			new PresetItemWithCount("SkillBook", 489, 1),
			new PresetItemWithCount("SkillBook", 490, 1),
			new PresetItemWithCount("SkillBook", 491, 1),
			new PresetItemWithCount("SkillBook", 733, 1),
			new PresetItemWithCount("SkillBook", 734, 1),
			new PresetItemWithCount("SkillBook", 735, 1),
			new PresetItemWithCount("SkillBook", 776, 1),
			new PresetItemWithCount("SkillBook", 777, 1),
			new PresetItemWithCount("SkillBook", 778, 1)
		}, new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(374, 1024, EMapPickupsType.Event, EMapPickupsType2.Invalid, "map_eventicon_18", 1025, 0, new byte[15]
		{
			0, 0, 0, 0, 0, 1, 0, 0, 0, 0,
			0, 0, 0, 0, 0
		}, new List<short> { 24 }, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[0], default(PresetItemTemplateId), new sbyte[1], new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(6, 500), -1, -1, -1, 1026, new int[2] { 1027, 1028 }, new int[2] { 1029, 1030 }, new int[2] { 689, 711 }, new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int> { -1, 10000 }, new List<PresetItemWithCount>
		{
			new PresetItemWithCount("SkillBook", 182, 1),
			new PresetItemWithCount("SkillBook", 285, 1),
			new PresetItemWithCount("SkillBook", 390, 1),
			new PresetItemWithCount("SkillBook", 391, 1),
			new PresetItemWithCount("SkillBook", 392, 1),
			new PresetItemWithCount("SkillBook", 492, 1),
			new PresetItemWithCount("SkillBook", 493, 1),
			new PresetItemWithCount("SkillBook", 494, 1),
			new PresetItemWithCount("SkillBook", 736, 1),
			new PresetItemWithCount("SkillBook", 737, 1),
			new PresetItemWithCount("SkillBook", 738, 1),
			new PresetItemWithCount("SkillBook", 779, 1),
			new PresetItemWithCount("SkillBook", 780, 1),
			new PresetItemWithCount("SkillBook", 781, 1),
			new PresetItemWithCount("SkillBook", 871, 1)
		}, new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(375, 1031, EMapPickupsType.Event, EMapPickupsType2.Invalid, "map_eventicon_18", 1032, 0, new byte[15]
		{
			0, 0, 0, 0, 0, 1, 0, 0, 0, 0,
			0, 0, 0, 0, 0
		}, new List<short> { 24 }, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[0], default(PresetItemTemplateId), new sbyte[1], new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(6, 1000), -1, -1, -1, 1033, new int[2] { 1034, 1035 }, new int[2] { 1036, 1037 }, new int[2] { 689, 711 }, new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int> { -1, 20000 }, new List<PresetItemWithCount>
		{
			new PresetItemWithCount("SkillBook", 495, 1),
			new PresetItemWithCount("SkillBook", 496, 1),
			new PresetItemWithCount("SkillBook", 739, 1),
			new PresetItemWithCount("SkillBook", 740, 1),
			new PresetItemWithCount("SkillBook", 782, 1),
			new PresetItemWithCount("SkillBook", 783, 1)
		}, new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(376, 1038, EMapPickupsType.Event, EMapPickupsType2.Invalid, "map_eventicon_18", 1039, 0, new byte[15]
		{
			0, 0, 0, 0, 0, 0, 1, 0, 0, 0,
			0, 0, 0, 0, 0
		}, new List<short> { 25 }, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[0], default(PresetItemTemplateId), new sbyte[1], new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), -1, -1, -1, 1040, new int[2] { 1041, 1042 }, new int[2] { 1043, 1044 }, new int[2] { 689, 689 }, new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>
		{
			new PresetItemWithCount("Accessory", 148, 1)
		}, new List<PresetItemWithCount>
		{
			new PresetItemWithCount("Accessory", 166, 1)
		}));
		_dataArray.Add(new MapPickupsItem(377, 1045, EMapPickupsType.Event, EMapPickupsType2.Invalid, "map_eventicon_18", 1046, 0, new byte[15]
		{
			0, 0, 0, 0, 0, 0, 1, 0, 0, 0,
			0, 0, 0, 0, 0
		}, new List<short> { 25 }, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[0], default(PresetItemTemplateId), new sbyte[1], new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), -1, -1, -1, 1047, new int[2] { 852, 1048 }, new int[2] { 1049, 1050 }, new int[2] { 689, 689 }, new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>
		{
			new PresetItemWithCount("Armor", 31, 1)
		}, new List<PresetItemWithCount>
		{
			new PresetItemWithCount("Armor", 13, 1),
			new PresetItemWithCount("Armor", 121, 1)
		}));
		_dataArray.Add(new MapPickupsItem(378, 1051, EMapPickupsType.Event, EMapPickupsType2.Invalid, "map_eventicon_18", 1052, 0, new byte[15]
		{
			0, 0, 0, 0, 0, 0, 1, 0, 0, 0,
			0, 0, 0, 0, 0
		}, new List<short> { 25 }, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[0], default(PresetItemTemplateId), new sbyte[1], new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), -1, -1, -1, 1053, new int[2] { 1054, 1055 }, new int[2] { 1056, 1057 }, new int[2] { 689, 689 }, new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>
		{
			new PresetItemWithCount("Weapon", 313, 1),
			new PresetItemWithCount("Weapon", 52, 1)
		}, new List<PresetItemWithCount>
		{
			new PresetItemWithCount("Weapon", 520, 1),
			new PresetItemWithCount("Weapon", 502, 1)
		}));
		_dataArray.Add(new MapPickupsItem(379, 1058, EMapPickupsType.Event, EMapPickupsType2.Invalid, "map_eventicon_18", 1059, 0, new byte[15]
		{
			0, 0, 0, 0, 0, 0, 1, 0, 0, 0,
			0, 0, 0, 0, 0
		}, new List<short> { 25 }, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[0], default(PresetItemTemplateId), new sbyte[1], new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(7, 50), -1, -1, -1, 1060, new int[2] { 707, 708 }, new int[2] { 709, 710 }, new int[2] { 689, 711 }, new List<PresetItemWithCount>(), new List<ResourceInfo>
		{
			default(ResourceInfo),
			new ResourceInfo(7, 1000)
		}, new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>
		{
			new PresetItemWithCount("Clothing", 37, 1)
		}, new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(380, 1061, EMapPickupsType.Event, EMapPickupsType2.Invalid, "map_eventicon_18", 1062, 0, new byte[15]
		{
			0, 0, 0, 0, 0, 0, 1, 0, 0, 0,
			0, 0, 0, 0, 0
		}, new List<short> { 25 }, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[0], default(PresetItemTemplateId), new sbyte[1], new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(7, 100), -1, -1, -1, 1063, new int[2] { 1064, 1065 }, new int[2] { 1066, 1067 }, new int[2] { 689, 711 }, new List<PresetItemWithCount>(), new List<ResourceInfo>
		{
			default(ResourceInfo),
			new ResourceInfo(7, 2000)
		}, new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>
		{
			new PresetItemWithCount("Clothing", 38, 1)
		}, new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(381, 1075, EMapPickupsType.Event, EMapPickupsType2.Invalid, "map_eventicon_18", 1076, 0, new byte[15]
		{
			0, 0, 0, 0, 0, 0, 1, 0, 0, 0,
			0, 0, 0, 0, 0
		}, new List<short> { 25 }, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[0], default(PresetItemTemplateId), new sbyte[1], new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(7, 300), -1, -1, -1, 1077, new int[2] { 1078, 1079 }, new int[2] { 1080, 1081 }, new int[2] { 689, 711 }, new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int> { -1, 5000 }, new List<PresetItemWithCount>
		{
			new PresetItemWithCount("SkillBook", 183, 1),
			new PresetItemWithCount("SkillBook", 184, 1),
			new PresetItemWithCount("SkillBook", 185, 1),
			new PresetItemWithCount("SkillBook", 286, 1),
			new PresetItemWithCount("SkillBook", 287, 1),
			new PresetItemWithCount("SkillBook", 288, 1),
			new PresetItemWithCount("SkillBook", 393, 1),
			new PresetItemWithCount("SkillBook", 394, 1),
			new PresetItemWithCount("SkillBook", 395, 1),
			new PresetItemWithCount("SkillBook", 567, 1),
			new PresetItemWithCount("SkillBook", 568, 1),
			new PresetItemWithCount("SkillBook", 569, 1),
			new PresetItemWithCount("SkillBook", 693, 1),
			new PresetItemWithCount("SkillBook", 694, 1),
			new PresetItemWithCount("SkillBook", 695, 1),
			new PresetItemWithCount("SkillBook", 800, 1),
			new PresetItemWithCount("SkillBook", 801, 1),
			new PresetItemWithCount("SkillBook", 802, 1)
		}, new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(382, 1082, EMapPickupsType.Event, EMapPickupsType2.Invalid, "map_eventicon_18", 1083, 0, new byte[15]
		{
			0, 0, 0, 0, 0, 0, 1, 0, 0, 0,
			0, 0, 0, 0, 0
		}, new List<short> { 25 }, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[0], default(PresetItemTemplateId), new sbyte[1], new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(7, 500), -1, -1, -1, 1084, new int[2] { 1085, 1086 }, new int[2] { 1087, 1088 }, new int[2] { 689, 711 }, new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int> { -1, 10000 }, new List<PresetItemWithCount>
		{
			new PresetItemWithCount("SkillBook", 186, 1),
			new PresetItemWithCount("SkillBook", 187, 1),
			new PresetItemWithCount("SkillBook", 188, 1),
			new PresetItemWithCount("SkillBook", 289, 1),
			new PresetItemWithCount("SkillBook", 290, 1),
			new PresetItemWithCount("SkillBook", 291, 1),
			new PresetItemWithCount("SkillBook", 396, 1),
			new PresetItemWithCount("SkillBook", 397, 1),
			new PresetItemWithCount("SkillBook", 398, 1),
			new PresetItemWithCount("SkillBook", 570, 1),
			new PresetItemWithCount("SkillBook", 571, 1),
			new PresetItemWithCount("SkillBook", 572, 1),
			new PresetItemWithCount("SkillBook", 696, 1),
			new PresetItemWithCount("SkillBook", 697, 1),
			new PresetItemWithCount("SkillBook", 698, 1),
			new PresetItemWithCount("SkillBook", 803, 1),
			new PresetItemWithCount("SkillBook", 804, 1),
			new PresetItemWithCount("SkillBook", 805, 1)
		}, new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(383, 1089, EMapPickupsType.Event, EMapPickupsType2.Invalid, "map_eventicon_18", 1090, 0, new byte[15]
		{
			0, 0, 0, 0, 0, 0, 1, 0, 0, 0,
			0, 0, 0, 0, 0
		}, new List<short> { 25 }, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[0], default(PresetItemTemplateId), new sbyte[1], new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(7, 1000), -1, -1, -1, 1091, new int[2] { 1092, 1093 }, new int[2] { 1094, 1095 }, new int[2] { 689, 711 }, new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int> { -1, 20000 }, new List<PresetItemWithCount>
		{
			new PresetItemWithCount("SkillBook", 189, 1),
			new PresetItemWithCount("SkillBook", 190, 1),
			new PresetItemWithCount("SkillBook", 292, 1),
			new PresetItemWithCount("SkillBook", 399, 1),
			new PresetItemWithCount("SkillBook", 400, 1),
			new PresetItemWithCount("SkillBook", 573, 1),
			new PresetItemWithCount("SkillBook", 699, 1),
			new PresetItemWithCount("SkillBook", 700, 1),
			new PresetItemWithCount("SkillBook", 806, 1),
			new PresetItemWithCount("SkillBook", 807, 1)
		}, new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(384, 1096, EMapPickupsType.Event, EMapPickupsType2.Invalid, "map_eventicon_18", 1097, 0, new byte[15]
		{
			0, 0, 0, 0, 0, 0, 0, 1, 0, 0,
			0, 0, 0, 0, 0
		}, new List<short> { 26 }, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[0], default(PresetItemTemplateId), new sbyte[1], new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), -1, -1, -1, 1098, new int[2] { 1099, 1100 }, new int[2] { 1101, 1102 }, new int[2] { 689, 689 }, new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>
		{
			new PresetItemWithCount("Accessory", 85, 1)
		}, new List<PresetItemWithCount>
		{
			new PresetItemWithCount("Accessory", 175, 1)
		}));
		_dataArray.Add(new MapPickupsItem(385, 1103, EMapPickupsType.Event, EMapPickupsType2.Invalid, "map_eventicon_18", 1104, 0, new byte[15]
		{
			0, 0, 0, 0, 0, 0, 0, 1, 0, 0,
			0, 0, 0, 0, 0
		}, new List<short> { 26 }, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[0], default(PresetItemTemplateId), new sbyte[1], new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), -1, -1, -1, 1105, new int[2] { 1106, 1107 }, new int[2] { 1108, 1109 }, new int[2] { 689, 689 }, new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>
		{
			new PresetItemWithCount("Armor", 76, 1),
			new PresetItemWithCount("Armor", 103, 1)
		}, new List<PresetItemWithCount>
		{
			new PresetItemWithCount("Armor", 94, 1),
			new PresetItemWithCount("Armor", 85, 1)
		}));
		_dataArray.Add(new MapPickupsItem(386, 1110, EMapPickupsType.Event, EMapPickupsType2.Invalid, "map_eventicon_18", 1111, 0, new byte[15]
		{
			0, 0, 0, 0, 0, 0, 0, 1, 0, 0,
			0, 0, 0, 0, 0
		}, new List<short> { 26 }, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[0], default(PresetItemTemplateId), new sbyte[1], new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), -1, -1, -1, 1112, new int[2] { 1113, 1114 }, new int[2] { 1115, 1116 }, new int[2] { 689, 689 }, new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>
		{
			new PresetItemWithCount("Weapon", 340, 1)
		}, new List<PresetItemWithCount>
		{
			new PresetItemWithCount("Weapon", 745, 1)
		}));
		_dataArray.Add(new MapPickupsItem(387, 1117, EMapPickupsType.Event, EMapPickupsType2.Invalid, "map_eventicon_18", 1118, 0, new byte[15]
		{
			0, 0, 0, 0, 0, 0, 0, 1, 0, 0,
			0, 0, 0, 0, 0
		}, new List<short> { 26 }, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[0], default(PresetItemTemplateId), new sbyte[1], new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(8, 50), -1, -1, -1, 1119, new int[2] { 707, 708 }, new int[2] { 709, 710 }, new int[2] { 689, 711 }, new List<PresetItemWithCount>(), new List<ResourceInfo>
		{
			default(ResourceInfo),
			new ResourceInfo(7, 1000)
		}, new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>
		{
			new PresetItemWithCount("Clothing", 40, 1)
		}, new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(388, 1120, EMapPickupsType.Event, EMapPickupsType2.Invalid, "map_eventicon_18", 1121, 0, new byte[15]
		{
			0, 0, 0, 0, 0, 0, 0, 1, 0, 0,
			0, 0, 0, 0, 0
		}, new List<short> { 26 }, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[0], default(PresetItemTemplateId), new sbyte[1], new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(8, 100), -1, -1, -1, 1122, new int[2] { 1123, 1124 }, new int[2] { 1125, 1126 }, new int[2] { 689, 711 }, new List<PresetItemWithCount>(), new List<ResourceInfo>
		{
			default(ResourceInfo),
			new ResourceInfo(7, 2000)
		}, new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>
		{
			new PresetItemWithCount("Clothing", 41, 1)
		}, new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(389, 1133, EMapPickupsType.Event, EMapPickupsType2.Invalid, "map_eventicon_18", 1134, 0, new byte[15]
		{
			0, 0, 0, 0, 0, 0, 0, 1, 0, 0,
			0, 0, 0, 0, 0
		}, new List<short> { 26 }, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[0], default(PresetItemTemplateId), new sbyte[1], new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(8, 300), -1, -1, -1, 1135, new int[2] { 1136, 1137 }, new int[2] { 1138, 1139 }, new int[2] { 689, 711 }, new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int> { -1, 5000 }, new List<PresetItemWithCount>
		{
			new PresetItemWithCount("SkillBook", 191, 1),
			new PresetItemWithCount("SkillBook", 192, 1),
			new PresetItemWithCount("SkillBook", 193, 1),
			new PresetItemWithCount("SkillBook", 293, 1),
			new PresetItemWithCount("SkillBook", 294, 1),
			new PresetItemWithCount("SkillBook", 295, 1),
			new PresetItemWithCount("SkillBook", 401, 1),
			new PresetItemWithCount("SkillBook", 402, 1),
			new PresetItemWithCount("SkillBook", 403, 1),
			new PresetItemWithCount("SkillBook", 498, 1),
			new PresetItemWithCount("SkillBook", 499, 1),
			new PresetItemWithCount("SkillBook", 500, 1),
			new PresetItemWithCount("SkillBook", 574, 1),
			new PresetItemWithCount("SkillBook", 575, 1),
			new PresetItemWithCount("SkillBook", 576, 1),
			new PresetItemWithCount("SkillBook", 858, 1),
			new PresetItemWithCount("SkillBook", 859, 1),
			new PresetItemWithCount("SkillBook", 860, 1)
		}, new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(390, 1140, EMapPickupsType.Event, EMapPickupsType2.Invalid, "map_eventicon_18", 1141, 0, new byte[15]
		{
			0, 0, 0, 0, 0, 0, 0, 1, 0, 0,
			0, 0, 0, 0, 0
		}, new List<short> { 26 }, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[0], default(PresetItemTemplateId), new sbyte[1], new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(8, 500), -1, -1, -1, 1142, new int[2] { 1143, 1144 }, new int[2] { 1145, 1146 }, new int[2] { 689, 711 }, new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int> { -1, 10000 }, new List<PresetItemWithCount>
		{
			new PresetItemWithCount("SkillBook", 194, 1),
			new PresetItemWithCount("SkillBook", 195, 1),
			new PresetItemWithCount("SkillBook", 196, 1),
			new PresetItemWithCount("SkillBook", 296, 1),
			new PresetItemWithCount("SkillBook", 297, 1),
			new PresetItemWithCount("SkillBook", 298, 1),
			new PresetItemWithCount("SkillBook", 404, 1),
			new PresetItemWithCount("SkillBook", 405, 1),
			new PresetItemWithCount("SkillBook", 406, 1),
			new PresetItemWithCount("SkillBook", 501, 1),
			new PresetItemWithCount("SkillBook", 502, 1),
			new PresetItemWithCount("SkillBook", 503, 1),
			new PresetItemWithCount("SkillBook", 577, 1),
			new PresetItemWithCount("SkillBook", 578, 1),
			new PresetItemWithCount("SkillBook", 579, 1),
			new PresetItemWithCount("SkillBook", 861, 1),
			new PresetItemWithCount("SkillBook", 862, 1),
			new PresetItemWithCount("SkillBook", 863, 1)
		}, new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(391, 1147, EMapPickupsType.Event, EMapPickupsType2.Invalid, "map_eventicon_18", 1148, 0, new byte[15]
		{
			0, 0, 0, 0, 0, 0, 0, 1, 0, 0,
			0, 0, 0, 0, 0
		}, new List<short> { 26 }, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[0], default(PresetItemTemplateId), new sbyte[1], new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(8, 1000), -1, -1, -1, 1149, new int[2] { 1150, 1151 }, new int[2] { 1152, 1153 }, new int[2] { 689, 711 }, new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int> { -1, 20000 }, new List<PresetItemWithCount>
		{
			new PresetItemWithCount("SkillBook", 197, 1),
			new PresetItemWithCount("SkillBook", 198, 1),
			new PresetItemWithCount("SkillBook", 299, 1),
			new PresetItemWithCount("SkillBook", 300, 1),
			new PresetItemWithCount("SkillBook", 407, 1),
			new PresetItemWithCount("SkillBook", 504, 1),
			new PresetItemWithCount("SkillBook", 580, 1),
			new PresetItemWithCount("SkillBook", 581, 1),
			new PresetItemWithCount("SkillBook", 864, 1),
			new PresetItemWithCount("SkillBook", 865, 1)
		}, new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(392, 1154, EMapPickupsType.Event, EMapPickupsType2.Invalid, "map_eventicon_18", 1155, 0, new byte[15]
		{
			0, 0, 0, 0, 0, 0, 0, 0, 1, 0,
			0, 0, 0, 0, 0
		}, new List<short> { 27 }, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[0], default(PresetItemTemplateId), new sbyte[1], new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), -1, -1, -1, 1156, new int[2] { 1157, 1158 }, new int[2] { 1159, 1160 }, new int[2] { 689, 689 }, new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>
		{
			new PresetItemWithCount("Accessory", 193, 1)
		}, new List<PresetItemWithCount>
		{
			new PresetItemWithCount("Accessory", 112, 1)
		}));
		_dataArray.Add(new MapPickupsItem(393, 1161, EMapPickupsType.Event, EMapPickupsType2.Invalid, "map_eventicon_18", 1162, 0, new byte[15]
		{
			0, 0, 0, 0, 0, 0, 0, 0, 1, 0,
			0, 0, 0, 0, 0
		}, new List<short> { 27 }, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[0], default(PresetItemTemplateId), new sbyte[1], new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), -1, -1, -1, 1163, new int[2] { 1164, 1165 }, new int[2] { 1166, 1167 }, new int[2] { 689, 689 }, new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>
		{
			new PresetItemWithCount("Armor", 283, 1),
			new PresetItemWithCount("Armor", 265, 1)
		}, new List<PresetItemWithCount>
		{
			new PresetItemWithCount("Armor", 391, 1),
			new PresetItemWithCount("Armor", 292, 1)
		}));
		_dataArray.Add(new MapPickupsItem(394, 1168, EMapPickupsType.Event, EMapPickupsType2.Invalid, "map_eventicon_18", 1169, 0, new byte[15]
		{
			0, 0, 0, 0, 0, 0, 0, 0, 1, 0,
			0, 0, 0, 0, 0
		}, new List<short> { 27 }, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[0], default(PresetItemTemplateId), new sbyte[1], new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), -1, -1, -1, 1170, new int[2] { 1171, 1172 }, new int[2] { 1173, 1174 }, new int[2] { 689, 689 }, new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>
		{
			new PresetItemWithCount("Weapon", 439, 1),
			new PresetItemWithCount("Weapon", 448, 1),
			new PresetItemWithCount("Weapon", 457, 1),
			new PresetItemWithCount("Weapon", 466, 1),
			new PresetItemWithCount("Weapon", 475, 1),
			new PresetItemWithCount("Weapon", 484, 1),
			new PresetItemWithCount("Weapon", 529, 1),
			new PresetItemWithCount("Weapon", 538, 1),
			new PresetItemWithCount("Weapon", 547, 1),
			new PresetItemWithCount("Weapon", 556, 1),
			new PresetItemWithCount("Weapon", 565, 1),
			new PresetItemWithCount("Weapon", 574, 1),
			new PresetItemWithCount("Weapon", 619, 1),
			new PresetItemWithCount("Weapon", 637, 1),
			new PresetItemWithCount("Weapon", 655, 1),
			new PresetItemWithCount("Weapon", 664, 1),
			new PresetItemWithCount("Weapon", 673, 1),
			new PresetItemWithCount("Weapon", 691, 1)
		}, new List<PresetItemWithCount>
		{
			new PresetItemWithCount("Weapon", 34, 1),
			new PresetItemWithCount("Weapon", 25, 1)
		}));
		_dataArray.Add(new MapPickupsItem(395, 1175, EMapPickupsType.Event, EMapPickupsType2.Invalid, "map_eventicon_18", 1176, 0, new byte[15]
		{
			0, 0, 0, 0, 0, 0, 0, 0, 1, 0,
			0, 0, 0, 0, 0
		}, new List<short> { 27 }, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[0], default(PresetItemTemplateId), new sbyte[1], new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(9, 50), -1, -1, -1, 1177, new int[2] { 707, 708 }, new int[1] { 1178 }, new int[2] { 689, 711 }, new List<PresetItemWithCount>(), new List<ResourceInfo>
		{
			default(ResourceInfo),
			new ResourceInfo(7, 1000)
		}, new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>
		{
			new PresetItemWithCount("Clothing", 43, 1)
		}, new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(396, 1179, EMapPickupsType.Event, EMapPickupsType2.Invalid, "map_eventicon_18", 1180, 0, new byte[15]
		{
			0, 0, 0, 0, 0, 0, 0, 0, 1, 0,
			0, 0, 0, 0, 0
		}, new List<short> { 27 }, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[0], default(PresetItemTemplateId), new sbyte[1], new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(9, 100), -1, -1, -1, 1181, new int[2] { 1172, 1182 }, new int[2] { 1183, 1184 }, new int[2] { 689, 711 }, new List<PresetItemWithCount>(), new List<ResourceInfo>
		{
			default(ResourceInfo),
			new ResourceInfo(7, 2000)
		}, new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>
		{
			new PresetItemWithCount("Clothing", 44, 1)
		}, new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(397, 1192, EMapPickupsType.Event, EMapPickupsType2.Invalid, "map_eventicon_18", 1193, 0, new byte[15]
		{
			0, 0, 0, 0, 0, 0, 0, 0, 1, 0,
			0, 0, 0, 0, 0
		}, new List<short> { 27 }, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[0], default(PresetItemTemplateId), new sbyte[1], new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(9, 300), -1, -1, -1, 1194, new int[2] { 1195, 1196 }, new int[2] { 1197, 1198 }, new int[2] { 689, 711 }, new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int> { -1, 5000 }, new List<PresetItemWithCount>
		{
			new PresetItemWithCount("SkillBook", 200, 1),
			new PresetItemWithCount("SkillBook", 201, 1),
			new PresetItemWithCount("SkillBook", 202, 1),
			new PresetItemWithCount("SkillBook", 302, 1),
			new PresetItemWithCount("SkillBook", 303, 1),
			new PresetItemWithCount("SkillBook", 304, 1),
			new PresetItemWithCount("SkillBook", 408, 1),
			new PresetItemWithCount("SkillBook", 409, 1),
			new PresetItemWithCount("SkillBook", 410, 1),
			new PresetItemWithCount("SkillBook", 702, 1),
			new PresetItemWithCount("SkillBook", 703, 1),
			new PresetItemWithCount("SkillBook", 704, 1),
			new PresetItemWithCount("SkillBook", 742, 1),
			new PresetItemWithCount("SkillBook", 743, 1),
			new PresetItemWithCount("SkillBook", 744, 1),
			new PresetItemWithCount("SkillBook", 784, 1),
			new PresetItemWithCount("SkillBook", 785, 1),
			new PresetItemWithCount("SkillBook", 786, 1),
			new PresetItemWithCount("SkillBook", 843, 1),
			new PresetItemWithCount("SkillBook", 844, 1),
			new PresetItemWithCount("SkillBook", 845, 1)
		}, new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(398, 1199, EMapPickupsType.Event, EMapPickupsType2.Invalid, "map_eventicon_18", 1200, 0, new byte[15]
		{
			0, 0, 0, 0, 0, 0, 0, 0, 1, 0,
			0, 0, 0, 0, 0
		}, new List<short> { 27 }, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[0], default(PresetItemTemplateId), new sbyte[1], new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(9, 500), -1, -1, -1, 1201, new int[2] { 1202, 1203 }, new int[2] { 1204, 1205 }, new int[2] { 689, 711 }, new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int> { -1, 10000 }, new List<PresetItemWithCount>
		{
			new PresetItemWithCount("SkillBook", 203, 1),
			new PresetItemWithCount("SkillBook", 204, 1),
			new PresetItemWithCount("SkillBook", 305, 1),
			new PresetItemWithCount("SkillBook", 306, 1),
			new PresetItemWithCount("SkillBook", 307, 1),
			new PresetItemWithCount("SkillBook", 411, 1),
			new PresetItemWithCount("SkillBook", 412, 1),
			new PresetItemWithCount("SkillBook", 413, 1),
			new PresetItemWithCount("SkillBook", 705, 1),
			new PresetItemWithCount("SkillBook", 706, 1),
			new PresetItemWithCount("SkillBook", 707, 1),
			new PresetItemWithCount("SkillBook", 745, 1),
			new PresetItemWithCount("SkillBook", 746, 1),
			new PresetItemWithCount("SkillBook", 747, 1),
			new PresetItemWithCount("SkillBook", 787, 1),
			new PresetItemWithCount("SkillBook", 788, 1),
			new PresetItemWithCount("SkillBook", 789, 1),
			new PresetItemWithCount("SkillBook", 846, 1),
			new PresetItemWithCount("SkillBook", 847, 1),
			new PresetItemWithCount("SkillBook", 848, 1),
			new PresetItemWithCount("SkillBook", 873, 1)
		}, new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(399, 1206, EMapPickupsType.Event, EMapPickupsType2.Invalid, "map_eventicon_18", 1207, 0, new byte[15]
		{
			0, 0, 0, 0, 0, 0, 0, 0, 1, 0,
			0, 0, 0, 0, 0
		}, new List<short> { 27 }, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[0], default(PresetItemTemplateId), new sbyte[1], new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(9, 1000), -1, -1, -1, 1208, new int[2] { 1209, 1210 }, new int[2] { 1211, 1212 }, new int[2] { 689, 711 }, new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int> { -1, 20000 }, new List<PresetItemWithCount>
		{
			new PresetItemWithCount("SkillBook", 308, 1),
			new PresetItemWithCount("SkillBook", 414, 1),
			new PresetItemWithCount("SkillBook", 708, 1),
			new PresetItemWithCount("SkillBook", 709, 1),
			new PresetItemWithCount("SkillBook", 748, 1),
			new PresetItemWithCount("SkillBook", 749, 1),
			new PresetItemWithCount("SkillBook", 790, 1),
			new PresetItemWithCount("SkillBook", 791, 1),
			new PresetItemWithCount("SkillBook", 849, 1),
			new PresetItemWithCount("SkillBook", 850, 1)
		}, new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(400, 1213, EMapPickupsType.Event, EMapPickupsType2.Invalid, "map_eventicon_18", 1214, 0, new byte[15]
		{
			0, 0, 0, 0, 0, 0, 0, 0, 0, 1,
			0, 0, 0, 0, 0
		}, new List<short> { 28 }, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[0], default(PresetItemTemplateId), new sbyte[1], new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), -1, -1, -1, 1215, new int[2] { 1216, 1217 }, new int[2] { 1218, 1219 }, new int[2] { 689, 689 }, new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>
		{
			new PresetItemWithCount("Accessory", 148, 1)
		}, new List<PresetItemWithCount>
		{
			new PresetItemWithCount("Accessory", 112, 1)
		}));
		_dataArray.Add(new MapPickupsItem(401, 1220, EMapPickupsType.Event, EMapPickupsType2.Invalid, "map_eventicon_18", 1221, 0, new byte[15]
		{
			0, 0, 0, 0, 0, 0, 0, 0, 0, 1,
			0, 0, 0, 0, 0
		}, new List<short> { 28 }, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[0], default(PresetItemTemplateId), new sbyte[1], new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), -1, -1, -1, 1222, new int[2] { 1223, 1224 }, new int[2] { 1225, 1226 }, new int[2] { 689, 689 }, new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>
		{
			new PresetItemWithCount("Armor", 184, 1),
			new PresetItemWithCount("Armor", 166, 1)
		}, new List<PresetItemWithCount>
		{
			new PresetItemWithCount("Armor", 193, 1),
			new PresetItemWithCount("Armor", 175, 1)
		}));
		_dataArray.Add(new MapPickupsItem(402, 1227, EMapPickupsType.Event, EMapPickupsType2.Invalid, "map_eventicon_18", 1228, 0, new byte[15]
		{
			0, 0, 0, 0, 0, 0, 0, 0, 0, 1,
			0, 0, 0, 0, 0
		}, new List<short> { 28 }, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[0], default(PresetItemTemplateId), new sbyte[1], new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), -1, -1, -1, 1229, new int[2] { 1230, 1231 }, new int[2] { 1232, 1233 }, new int[2] { 689, 689 }, new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>
		{
			new PresetItemWithCount("Weapon", 7, 1)
		}, new List<PresetItemWithCount>
		{
			new PresetItemWithCount("Weapon", 277, 1)
		}));
		_dataArray.Add(new MapPickupsItem(403, 1234, EMapPickupsType.Event, EMapPickupsType2.Invalid, "map_eventicon_18", 1235, 0, new byte[15]
		{
			0, 0, 0, 0, 0, 0, 0, 0, 0, 1,
			0, 0, 0, 0, 0
		}, new List<short> { 28 }, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[0], default(PresetItemTemplateId), new sbyte[1], new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(10, 50), -1, -1, -1, 1236, new int[2] { 707, 708 }, new int[2] { 709, 710 }, new int[2] { 689, 711 }, new List<PresetItemWithCount>(), new List<ResourceInfo>
		{
			default(ResourceInfo),
			new ResourceInfo(7, 1000)
		}, new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>
		{
			new PresetItemWithCount("Clothing", 46, 1)
		}, new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(404, 1237, EMapPickupsType.Event, EMapPickupsType2.Invalid, "map_eventicon_18", 1238, 0, new byte[15]
		{
			0, 0, 0, 0, 0, 0, 0, 0, 0, 1,
			0, 0, 0, 0, 0
		}, new List<short> { 28 }, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[0], default(PresetItemTemplateId), new sbyte[1], new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(10, 100), -1, -1, -1, 1239, new int[2] { 1240, 1241 }, new int[2] { 1242, 1243 }, new int[2] { 689, 711 }, new List<PresetItemWithCount>(), new List<ResourceInfo>
		{
			default(ResourceInfo),
			new ResourceInfo(7, 2000)
		}, new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>
		{
			new PresetItemWithCount("Clothing", 47, 1)
		}, new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(405, 1250, EMapPickupsType.Event, EMapPickupsType2.Invalid, "map_eventicon_18", 1251, 0, new byte[15]
		{
			0, 0, 0, 0, 0, 0, 0, 0, 0, 1,
			0, 0, 0, 0, 0
		}, new List<short> { 28 }, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[0], default(PresetItemTemplateId), new sbyte[1], new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(10, 300), -1, -1, -1, 1252, new int[2] { 1253, 1254 }, new int[2] { 1255, 1256 }, new int[2] { 689, 711 }, new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int> { -1, 5000 }, new List<PresetItemWithCount>
		{
			new PresetItemWithCount("SkillBook", 205, 1),
			new PresetItemWithCount("SkillBook", 206, 1),
			new PresetItemWithCount("SkillBook", 207, 1),
			new PresetItemWithCount("SkillBook", 309, 1),
			new PresetItemWithCount("SkillBook", 310, 1),
			new PresetItemWithCount("SkillBook", 311, 1),
			new PresetItemWithCount("SkillBook", 415, 1),
			new PresetItemWithCount("SkillBook", 416, 1),
			new PresetItemWithCount("SkillBook", 417, 1),
			new PresetItemWithCount("SkillBook", 505, 1),
			new PresetItemWithCount("SkillBook", 506, 1),
			new PresetItemWithCount("SkillBook", 507, 1),
			new PresetItemWithCount("SkillBook", 582, 1),
			new PresetItemWithCount("SkillBook", 583, 1),
			new PresetItemWithCount("SkillBook", 584, 1),
			new PresetItemWithCount("SkillBook", 622, 1),
			new PresetItemWithCount("SkillBook", 623, 1),
			new PresetItemWithCount("SkillBook", 624, 1),
			new PresetItemWithCount("SkillBook", 638, 1),
			new PresetItemWithCount("SkillBook", 639, 1),
			new PresetItemWithCount("SkillBook", 640, 1)
		}, new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(406, 1257, EMapPickupsType.Event, EMapPickupsType2.Invalid, "map_eventicon_18", 1258, 0, new byte[15]
		{
			0, 0, 0, 0, 0, 0, 0, 0, 0, 1,
			0, 0, 0, 0, 0
		}, new List<short> { 28 }, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[0], default(PresetItemTemplateId), new sbyte[1], new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(10, 500), -1, -1, -1, 1259, new int[2] { 1260, 1261 }, new int[2] { 1262, 1263 }, new int[2] { 689, 711 }, new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int> { -1, 10000 }, new List<PresetItemWithCount>
		{
			new PresetItemWithCount("SkillBook", 208, 1),
			new PresetItemWithCount("SkillBook", 209, 1),
			new PresetItemWithCount("SkillBook", 312, 1),
			new PresetItemWithCount("SkillBook", 313, 1),
			new PresetItemWithCount("SkillBook", 418, 1),
			new PresetItemWithCount("SkillBook", 419, 1),
			new PresetItemWithCount("SkillBook", 420, 1),
			new PresetItemWithCount("SkillBook", 508, 1),
			new PresetItemWithCount("SkillBook", 509, 1),
			new PresetItemWithCount("SkillBook", 510, 1),
			new PresetItemWithCount("SkillBook", 585, 1),
			new PresetItemWithCount("SkillBook", 586, 1),
			new PresetItemWithCount("SkillBook", 587, 1),
			new PresetItemWithCount("SkillBook", 625, 1),
			new PresetItemWithCount("SkillBook", 626, 1),
			new PresetItemWithCount("SkillBook", 627, 1),
			new PresetItemWithCount("SkillBook", 641, 1),
			new PresetItemWithCount("SkillBook", 642, 1),
			new PresetItemWithCount("SkillBook", 643, 1)
		}, new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(407, 1264, EMapPickupsType.Event, EMapPickupsType2.Invalid, "map_eventicon_18", 1265, 0, new byte[15]
		{
			0, 0, 0, 0, 0, 0, 0, 0, 0, 1,
			0, 0, 0, 0, 0
		}, new List<short> { 28 }, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[0], default(PresetItemTemplateId), new sbyte[1], new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(10, 1000), -1, -1, -1, 1266, new int[2] { 1267, 1268 }, new int[2] { 1269, 1270 }, new int[2] { 689, 711 }, new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int> { -1, 20000 }, new List<PresetItemWithCount>
		{
			new PresetItemWithCount("SkillBook", 421, 1),
			new PresetItemWithCount("SkillBook", 422, 1),
			new PresetItemWithCount("SkillBook", 628, 1),
			new PresetItemWithCount("SkillBook", 629, 1),
			new PresetItemWithCount("SkillBook", 644, 1),
			new PresetItemWithCount("SkillBook", 645, 1)
		}, new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(408, 1271, EMapPickupsType.Event, EMapPickupsType2.Invalid, "map_eventicon_18", 1272, 0, new byte[15]
		{
			0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
			1, 0, 0, 0, 0
		}, new List<short> { 29 }, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[0], default(PresetItemTemplateId), new sbyte[1], new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), -1, -1, -1, 1273, new int[2] { 1274, 1275 }, new int[2] { 1276, 1277 }, new int[2] { 689, 689 }, new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>
		{
			new PresetItemWithCount("Accessory", 202, 1)
		}, new List<PresetItemWithCount>
		{
			new PresetItemWithCount("Accessory", 103, 1)
		}));
		_dataArray.Add(new MapPickupsItem(409, 1278, EMapPickupsType.Event, EMapPickupsType2.Invalid, "map_eventicon_18", 1279, 0, new byte[15]
		{
			0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
			1, 0, 0, 0, 0
		}, new List<short> { 29 }, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[0], default(PresetItemTemplateId), new sbyte[1], new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), -1, -1, -1, 1280, new int[2] { 1281, 1282 }, new int[2] { 1283, 1284 }, new int[2] { 689, 689 }, new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>
		{
			new PresetItemWithCount("Armor", 4, 1),
			new PresetItemWithCount("Armor", 112, 1)
		}, new List<PresetItemWithCount>
		{
			new PresetItemWithCount("Armor", 22, 1)
		}));
		_dataArray.Add(new MapPickupsItem(410, 1285, EMapPickupsType.Event, EMapPickupsType2.Invalid, "map_eventicon_18", 1286, 0, new byte[15]
		{
			0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
			1, 0, 0, 0, 0
		}, new List<short> { 29 }, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[0], default(PresetItemTemplateId), new sbyte[1], new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), -1, -1, -1, 1287, new int[2] { 1288, 1289 }, new int[2] { 1290, 1291 }, new int[2] { 689, 689 }, new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>
		{
			new PresetItemWithCount("Weapon", 412, 1),
			new PresetItemWithCount("Weapon", 394, 1),
			new PresetItemWithCount("Weapon", 385, 1),
			new PresetItemWithCount("Weapon", 286, 1)
		}, new List<PresetItemWithCount>
		{
			new PresetItemWithCount("Weapon", 574, 1),
			new PresetItemWithCount("Weapon", 556, 1),
			new PresetItemWithCount("Weapon", 538, 1),
			new PresetItemWithCount("Weapon", 529, 1)
		}));
		_dataArray.Add(new MapPickupsItem(411, 1292, EMapPickupsType.Event, EMapPickupsType2.Invalid, "map_eventicon_18", 1293, 0, new byte[15]
		{
			0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
			1, 0, 0, 0, 0
		}, new List<short> { 29 }, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[0], default(PresetItemTemplateId), new sbyte[1], new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(11, 50), -1, -1, -1, 1294, new int[2] { 707, 708 }, new int[2] { 709, 710 }, new int[2] { 689, 711 }, new List<PresetItemWithCount>(), new List<ResourceInfo>
		{
			default(ResourceInfo),
			new ResourceInfo(7, 1000)
		}, new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>
		{
			new PresetItemWithCount("Clothing", 49, 1)
		}, new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(412, 1295, EMapPickupsType.Event, EMapPickupsType2.Invalid, "map_eventicon_18", 1296, 0, new byte[15]
		{
			0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
			1, 0, 0, 0, 0
		}, new List<short> { 29 }, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[0], default(PresetItemTemplateId), new sbyte[1], new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(11, 100), -1, -1, -1, 1297, new int[2] { 1298, 1299 }, new int[2] { 1300, 1301 }, new int[2] { 689, 711 }, new List<PresetItemWithCount>(), new List<ResourceInfo>
		{
			default(ResourceInfo),
			new ResourceInfo(7, 2000)
		}, new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>
		{
			new PresetItemWithCount("Clothing", 50, 1)
		}, new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(413, 1309, EMapPickupsType.Event, EMapPickupsType2.Invalid, "map_eventicon_18", 1310, 0, new byte[15]
		{
			0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
			1, 0, 0, 0, 0
		}, new List<short> { 29 }, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[0], default(PresetItemTemplateId), new sbyte[1], new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(11, 300), -1, -1, -1, 1311, new int[2] { 1312, 1313 }, new int[2] { 1314, 1315 }, new int[2] { 689, 711 }, new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int> { -1, 5000 }, new List<PresetItemWithCount>
		{
			new PresetItemWithCount("SkillBook", 210, 1),
			new PresetItemWithCount("SkillBook", 211, 1),
			new PresetItemWithCount("SkillBook", 212, 1),
			new PresetItemWithCount("SkillBook", 314, 1),
			new PresetItemWithCount("SkillBook", 315, 1),
			new PresetItemWithCount("SkillBook", 316, 1),
			new PresetItemWithCount("SkillBook", 424, 1),
			new PresetItemWithCount("SkillBook", 425, 1),
			new PresetItemWithCount("SkillBook", 426, 1),
			new PresetItemWithCount("SkillBook", 511, 1),
			new PresetItemWithCount("SkillBook", 512, 1),
			new PresetItemWithCount("SkillBook", 513, 1),
			new PresetItemWithCount("SkillBook", 750, 1),
			new PresetItemWithCount("SkillBook", 751, 1),
			new PresetItemWithCount("SkillBook", 752, 1),
			new PresetItemWithCount("SkillBook", 809, 1),
			new PresetItemWithCount("SkillBook", 810, 1),
			new PresetItemWithCount("SkillBook", 811, 1)
		}, new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(414, 1316, EMapPickupsType.Event, EMapPickupsType2.Invalid, "map_eventicon_18", 1317, 0, new byte[15]
		{
			0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
			1, 0, 0, 0, 0
		}, new List<short> { 29 }, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[0], default(PresetItemTemplateId), new sbyte[1], new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(11, 500), -1, -1, -1, 1318, new int[2] { 1319, 1320 }, new int[1] { 1321 }, new int[2] { 689, 711 }, new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int> { -1, 10000 }, new List<PresetItemWithCount>
		{
			new PresetItemWithCount("SkillBook", 213, 1),
			new PresetItemWithCount("SkillBook", 214, 1),
			new PresetItemWithCount("SkillBook", 215, 1),
			new PresetItemWithCount("SkillBook", 317, 1),
			new PresetItemWithCount("SkillBook", 427, 1),
			new PresetItemWithCount("SkillBook", 428, 1),
			new PresetItemWithCount("SkillBook", 429, 1),
			new PresetItemWithCount("SkillBook", 514, 1),
			new PresetItemWithCount("SkillBook", 515, 1),
			new PresetItemWithCount("SkillBook", 516, 1),
			new PresetItemWithCount("SkillBook", 753, 1),
			new PresetItemWithCount("SkillBook", 754, 1),
			new PresetItemWithCount("SkillBook", 755, 1),
			new PresetItemWithCount("SkillBook", 812, 1),
			new PresetItemWithCount("SkillBook", 813, 1),
			new PresetItemWithCount("SkillBook", 814, 1)
		}, new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(415, 1322, EMapPickupsType.Event, EMapPickupsType2.Invalid, "map_eventicon_18", 1323, 0, new byte[15]
		{
			0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
			1, 0, 0, 0, 0
		}, new List<short> { 29 }, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[0], default(PresetItemTemplateId), new sbyte[1], new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(11, 1000), -1, -1, -1, 1324, new int[2] { 1325, 1326 }, new int[2] { 1327, 1328 }, new int[2] { 689, 711 }, new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int> { -1, 20000 }, new List<PresetItemWithCount>
		{
			new PresetItemWithCount("SkillBook", 216, 1),
			new PresetItemWithCount("SkillBook", 217, 1),
			new PresetItemWithCount("SkillBook", 430, 1),
			new PresetItemWithCount("SkillBook", 431, 1),
			new PresetItemWithCount("SkillBook", 517, 1),
			new PresetItemWithCount("SkillBook", 518, 1),
			new PresetItemWithCount("SkillBook", 756, 1),
			new PresetItemWithCount("SkillBook", 757, 1),
			new PresetItemWithCount("SkillBook", 815, 1),
			new PresetItemWithCount("SkillBook", 816, 1)
		}, new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(416, 1329, EMapPickupsType.Event, EMapPickupsType2.Invalid, "map_eventicon_18", 1330, 0, new byte[15]
		{
			0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
			0, 1, 0, 0, 0
		}, new List<short> { 30 }, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[0], default(PresetItemTemplateId), new sbyte[1], new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), -1, -1, -1, 1331, new int[2] { 1332, 1333 }, new int[2] { 1334, 1335 }, new int[2] { 689, 689 }, new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>
		{
			new PresetItemWithCount("Accessory", 139, 1)
		}, new List<PresetItemWithCount>
		{
			new PresetItemWithCount("Accessory", 121, 1)
		}));
		_dataArray.Add(new MapPickupsItem(417, 1336, EMapPickupsType.Event, EMapPickupsType2.Invalid, "map_eventicon_18", 1337, 0, new byte[15]
		{
			0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
			0, 1, 0, 0, 0
		}, new List<short> { 30 }, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[0], default(PresetItemTemplateId), new sbyte[1], new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), -1, -1, -1, 1338, new int[2] { 1339, 1340 }, new int[2] { 1341, 1342 }, new int[2] { 689, 689 }, new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>
		{
			new PresetItemWithCount("Armor", 517, 1),
			new PresetItemWithCount("Armor", 409, 1)
		}, new List<PresetItemWithCount>
		{
			new PresetItemWithCount("Armor", 427, 1)
		}));
		_dataArray.Add(new MapPickupsItem(418, 1343, EMapPickupsType.Event, EMapPickupsType2.Invalid, "map_eventicon_18", 1344, 0, new byte[15]
		{
			0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
			0, 1, 0, 0, 0
		}, new List<short> { 30 }, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[0], default(PresetItemTemplateId), new sbyte[1], new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), -1, -1, -1, 1345, new int[2] { 793, 1346 }, new int[2] { 1347, 1348 }, new int[2] { 689, 689 }, new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>
		{
			new PresetItemWithCount("Weapon", 817, 1),
			new PresetItemWithCount("Weapon", 799, 1),
			new PresetItemWithCount("Weapon", 376, 1)
		}, new List<PresetItemWithCount>
		{
			new PresetItemWithCount("Weapon", 502, 1),
			new PresetItemWithCount("Weapon", 493, 1),
			new PresetItemWithCount("Weapon", 484, 1)
		}));
		_dataArray.Add(new MapPickupsItem(419, 1349, EMapPickupsType.Event, EMapPickupsType2.Invalid, "map_eventicon_18", 1350, 0, new byte[15]
		{
			0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
			0, 1, 0, 0, 0
		}, new List<short> { 30 }, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[0], default(PresetItemTemplateId), new sbyte[1], new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(12, 50), -1, -1, -1, 1351, new int[2] { 707, 708 }, new int[2] { 709, 710 }, new int[2] { 689, 711 }, new List<PresetItemWithCount>(), new List<ResourceInfo>
		{
			default(ResourceInfo),
			new ResourceInfo(7, 1000)
		}, new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>
		{
			new PresetItemWithCount("Clothing", 52, 1)
		}, new List<PresetItemWithCount>()));
	}

	private void CreateItems7()
	{
		_dataArray.Add(new MapPickupsItem(420, 1352, EMapPickupsType.Event, EMapPickupsType2.Invalid, "map_eventicon_18", 1353, 0, new byte[15]
		{
			0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
			0, 1, 0, 0, 0
		}, new List<short> { 30 }, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[0], default(PresetItemTemplateId), new sbyte[1], new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(12, 100), -1, -1, -1, 1354, new int[2] { 1355, 1356 }, new int[3] { 1357, 1358, 1359 }, new int[2] { 689, 711 }, new List<PresetItemWithCount>(), new List<ResourceInfo>
		{
			default(ResourceInfo),
			new ResourceInfo(7, 2000)
		}, new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>
		{
			new PresetItemWithCount("Clothing", 53, 1)
		}, new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(421, 1367, EMapPickupsType.Event, EMapPickupsType2.Invalid, "map_eventicon_18", 1368, 0, new byte[15]
		{
			0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
			0, 1, 0, 0, 0
		}, new List<short> { 30 }, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[0], default(PresetItemTemplateId), new sbyte[1], new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(12, 300), -1, -1, -1, 1369, new int[2] { 1370, 1371 }, new int[2] { 1372, 1373 }, new int[2] { 689, 711 }, new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int> { -1, 5000 }, new List<PresetItemWithCount>
		{
			new PresetItemWithCount("SkillBook", 219, 1),
			new PresetItemWithCount("SkillBook", 220, 1),
			new PresetItemWithCount("SkillBook", 221, 1),
			new PresetItemWithCount("SkillBook", 318, 1),
			new PresetItemWithCount("SkillBook", 319, 1),
			new PresetItemWithCount("SkillBook", 320, 1),
			new PresetItemWithCount("SkillBook", 432, 1),
			new PresetItemWithCount("SkillBook", 433, 1),
			new PresetItemWithCount("SkillBook", 434, 1),
			new PresetItemWithCount("SkillBook", 519, 1),
			new PresetItemWithCount("SkillBook", 520, 1),
			new PresetItemWithCount("SkillBook", 521, 1),
			new PresetItemWithCount("SkillBook", 588, 1),
			new PresetItemWithCount("SkillBook", 589, 1),
			new PresetItemWithCount("SkillBook", 590, 1),
			new PresetItemWithCount("SkillBook", 710, 1),
			new PresetItemWithCount("SkillBook", 711, 1),
			new PresetItemWithCount("SkillBook", 712, 1),
			new PresetItemWithCount("SkillBook", 825, 1),
			new PresetItemWithCount("SkillBook", 826, 1),
			new PresetItemWithCount("SkillBook", 827, 1)
		}, new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(422, 1374, EMapPickupsType.Event, EMapPickupsType2.Invalid, "map_eventicon_18", 1375, 0, new byte[15]
		{
			0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
			0, 1, 0, 0, 0
		}, new List<short> { 30 }, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[0], default(PresetItemTemplateId), new sbyte[1], new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(12, 500), -1, -1, -1, 1376, new int[2] { 1377, 1378 }, new int[2] { 1379, 1380 }, new int[2] { 689, 711 }, new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int> { -1, 10000 }, new List<PresetItemWithCount>
		{
			new PresetItemWithCount("SkillBook", 222, 1),
			new PresetItemWithCount("SkillBook", 223, 1),
			new PresetItemWithCount("SkillBook", 224, 1),
			new PresetItemWithCount("SkillBook", 321, 1),
			new PresetItemWithCount("SkillBook", 322, 1),
			new PresetItemWithCount("SkillBook", 323, 1),
			new PresetItemWithCount("SkillBook", 435, 1),
			new PresetItemWithCount("SkillBook", 436, 1),
			new PresetItemWithCount("SkillBook", 437, 1),
			new PresetItemWithCount("SkillBook", 522, 1),
			new PresetItemWithCount("SkillBook", 523, 1),
			new PresetItemWithCount("SkillBook", 524, 1),
			new PresetItemWithCount("SkillBook", 591, 1),
			new PresetItemWithCount("SkillBook", 592, 1),
			new PresetItemWithCount("SkillBook", 593, 1),
			new PresetItemWithCount("SkillBook", 713, 1),
			new PresetItemWithCount("SkillBook", 714, 1),
			new PresetItemWithCount("SkillBook", 715, 1),
			new PresetItemWithCount("SkillBook", 828, 1),
			new PresetItemWithCount("SkillBook", 829, 1),
			new PresetItemWithCount("SkillBook", 830, 1)
		}, new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(423, 1381, EMapPickupsType.Event, EMapPickupsType2.Invalid, "map_eventicon_18", 1382, 0, new byte[15]
		{
			0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
			0, 1, 0, 0, 0
		}, new List<short> { 30 }, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[0], default(PresetItemTemplateId), new sbyte[1], new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(12, 1000), -1, -1, -1, 1383, new int[2] { 1216, 1384 }, new int[2] { 1385, 1386 }, new int[2] { 689, 711 }, new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int> { -1, 20000 }, new List<PresetItemWithCount>
		{
			new PresetItemWithCount("SkillBook", 225, 1),
			new PresetItemWithCount("SkillBook", 324, 1),
			new PresetItemWithCount("SkillBook", 438, 1),
			new PresetItemWithCount("SkillBook", 439, 1),
			new PresetItemWithCount("SkillBook", 525, 1),
			new PresetItemWithCount("SkillBook", 526, 1),
			new PresetItemWithCount("SkillBook", 594, 1),
			new PresetItemWithCount("SkillBook", 595, 1),
			new PresetItemWithCount("SkillBook", 716, 1),
			new PresetItemWithCount("SkillBook", 831, 1),
			new PresetItemWithCount("SkillBook", 832, 1)
		}, new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(424, 1387, EMapPickupsType.Event, EMapPickupsType2.Invalid, "map_eventicon_18", 1388, 0, new byte[15]
		{
			0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
			0, 0, 1, 0, 0
		}, new List<short> { 31 }, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[0], default(PresetItemTemplateId), new sbyte[1], new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), -1, -1, -1, 1389, new int[2] { 1390, 1231 }, new int[2] { 1391, 1392 }, new int[2] { 689, 689 }, new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>
		{
			new PresetItemWithCount("Accessory", 139, 1)
		}, new List<PresetItemWithCount>
		{
			new PresetItemWithCount("Accessory", 121, 1)
		}));
		_dataArray.Add(new MapPickupsItem(425, 1393, EMapPickupsType.Event, EMapPickupsType2.Invalid, "map_eventicon_18", 1394, 0, new byte[15]
		{
			0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
			0, 0, 1, 0, 0
		}, new List<short> { 31 }, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[0], default(PresetItemTemplateId), new sbyte[1], new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), -1, -1, -1, 1395, new int[2] { 1396, 1397 }, new int[2] { 1398, 1399 }, new int[2] { 689, 689 }, new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>
		{
			new PresetItemWithCount("Armor", 436, 1),
			new PresetItemWithCount("Armor", 445, 1)
		}, new List<PresetItemWithCount>
		{
			new PresetItemWithCount("Armor", 454, 1),
			new PresetItemWithCount("Armor", 463, 1)
		}));
		_dataArray.Add(new MapPickupsItem(426, 1400, EMapPickupsType.Event, EMapPickupsType2.Invalid, "map_eventicon_18", 1401, 0, new byte[15]
		{
			0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
			0, 0, 1, 0, 0
		}, new List<short> { 31 }, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[0], default(PresetItemTemplateId), new sbyte[1], new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), -1, -1, -1, 1402, new int[2] { 1403, 1404 }, new int[2] { 1405, 1406 }, new int[2] { 689, 689 }, new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>
		{
			new PresetItemWithCount("Armor", 463, 1),
			new PresetItemWithCount("Armor", 436, 1)
		}, new List<PresetItemWithCount>
		{
			new PresetItemWithCount("Armor", 445, 1),
			new PresetItemWithCount("Armor", 454, 1)
		}));
		_dataArray.Add(new MapPickupsItem(427, 1407, EMapPickupsType.Event, EMapPickupsType2.Invalid, "map_eventicon_18", 1408, 0, new byte[15]
		{
			0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
			0, 0, 1, 0, 0
		}, new List<short> { 31 }, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[0], default(PresetItemTemplateId), new sbyte[1], new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(13, 50), -1, -1, -1, 1409, new int[2] { 707, 708 }, new int[2] { 709, 710 }, new int[2] { 689, 711 }, new List<PresetItemWithCount>(), new List<ResourceInfo>
		{
			default(ResourceInfo),
			new ResourceInfo(7, 1000)
		}, new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>
		{
			new PresetItemWithCount("Clothing", 55, 1)
		}, new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(428, 1410, EMapPickupsType.Event, EMapPickupsType2.Invalid, "map_eventicon_18", 1411, 0, new byte[15]
		{
			0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
			0, 0, 1, 0, 0
		}, new List<short> { 31 }, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[0], default(PresetItemTemplateId), new sbyte[1], new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(13, 100), -1, -1, -1, 1412, new int[2] { 1172, 1413 }, new int[2] { 1414, 1415 }, new int[2] { 689, 711 }, new List<PresetItemWithCount>(), new List<ResourceInfo>
		{
			default(ResourceInfo),
			new ResourceInfo(7, 2000)
		}, new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>
		{
			new PresetItemWithCount("Clothing", 56, 1)
		}, new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(429, 1423, EMapPickupsType.Event, EMapPickupsType2.Invalid, "map_eventicon_18", 1424, 0, new byte[15]
		{
			0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
			0, 0, 1, 0, 0
		}, new List<short> { 31 }, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[0], default(PresetItemTemplateId), new sbyte[1], new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(13, 300), -1, -1, -1, 1425, new int[2] { 1426, 1427 }, new int[2] { 1428, 1429 }, new int[2] { 689, 711 }, new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int> { -1, 5000 }, new List<PresetItemWithCount>
		{
			new PresetItemWithCount("SkillBook", 226, 1),
			new PresetItemWithCount("SkillBook", 227, 1),
			new PresetItemWithCount("SkillBook", 228, 1),
			new PresetItemWithCount("SkillBook", 325, 1),
			new PresetItemWithCount("SkillBook", 326, 1),
			new PresetItemWithCount("SkillBook", 327, 1),
			new PresetItemWithCount("SkillBook", 441, 1),
			new PresetItemWithCount("SkillBook", 442, 1),
			new PresetItemWithCount("SkillBook", 443, 1),
			new PresetItemWithCount("SkillBook", 597, 1),
			new PresetItemWithCount("SkillBook", 598, 1),
			new PresetItemWithCount("SkillBook", 599, 1),
			new PresetItemWithCount("SkillBook", 647, 1),
			new PresetItemWithCount("SkillBook", 648, 1),
			new PresetItemWithCount("SkillBook", 649, 1),
			new PresetItemWithCount("SkillBook", 717, 1),
			new PresetItemWithCount("SkillBook", 718, 1),
			new PresetItemWithCount("SkillBook", 719, 1)
		}, new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(430, 1430, EMapPickupsType.Event, EMapPickupsType2.Invalid, "map_eventicon_18", 1431, 0, new byte[15]
		{
			0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
			0, 0, 1, 0, 0
		}, new List<short> { 31 }, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[0], default(PresetItemTemplateId), new sbyte[1], new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(13, 500), -1, -1, -1, 1432, new int[2] { 1433, 1434 }, new int[2] { 1435, 1436 }, new int[2] { 689, 711 }, new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int> { -1, 10000 }, new List<PresetItemWithCount>
		{
			new PresetItemWithCount("SkillBook", 229, 1),
			new PresetItemWithCount("SkillBook", 230, 1),
			new PresetItemWithCount("SkillBook", 231, 1),
			new PresetItemWithCount("SkillBook", 328, 1),
			new PresetItemWithCount("SkillBook", 329, 1),
			new PresetItemWithCount("SkillBook", 330, 1),
			new PresetItemWithCount("SkillBook", 444, 1),
			new PresetItemWithCount("SkillBook", 445, 1),
			new PresetItemWithCount("SkillBook", 446, 1),
			new PresetItemWithCount("SkillBook", 600, 1),
			new PresetItemWithCount("SkillBook", 601, 1),
			new PresetItemWithCount("SkillBook", 602, 1),
			new PresetItemWithCount("SkillBook", 650, 1),
			new PresetItemWithCount("SkillBook", 651, 1),
			new PresetItemWithCount("SkillBook", 652, 1),
			new PresetItemWithCount("SkillBook", 720, 1),
			new PresetItemWithCount("SkillBook", 721, 1),
			new PresetItemWithCount("SkillBook", 722, 1)
		}, new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(431, 1437, EMapPickupsType.Event, EMapPickupsType2.Invalid, "map_eventicon_18", 1438, 0, new byte[15]
		{
			0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
			0, 0, 1, 0, 0
		}, new List<short> { 31 }, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[0], default(PresetItemTemplateId), new sbyte[1], new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(13, 1000), -1, -1, -1, 1439, new int[2] { 1440, 1441 }, new int[2] { 1442, 1443 }, new int[2] { 689, 711 }, new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int> { -1, 20000 }, new List<PresetItemWithCount>
		{
			new PresetItemWithCount("SkillBook", 232, 1),
			new PresetItemWithCount("SkillBook", 331, 1),
			new PresetItemWithCount("SkillBook", 332, 1),
			new PresetItemWithCount("SkillBook", 447, 1),
			new PresetItemWithCount("SkillBook", 603, 1),
			new PresetItemWithCount("SkillBook", 604, 1),
			new PresetItemWithCount("SkillBook", 653, 1),
			new PresetItemWithCount("SkillBook", 654, 1),
			new PresetItemWithCount("SkillBook", 723, 1),
			new PresetItemWithCount("SkillBook", 724, 1)
		}, new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(432, 1444, EMapPickupsType.Event, EMapPickupsType2.Invalid, "map_eventicon_18", 1445, 0, new byte[15]
		{
			0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
			0, 0, 0, 1, 0
		}, new List<short> { 32 }, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[0], default(PresetItemTemplateId), new sbyte[1], new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), -1, -1, -1, 1446, new int[2] { 1447, 1448 }, new int[2] { 1449, 1450 }, new int[2] { 689, 689 }, new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>
		{
			new PresetItemWithCount("Accessory", 4, 1)
		}, new List<PresetItemWithCount>
		{
			new PresetItemWithCount("Accessory", 103, 1)
		}));
		_dataArray.Add(new MapPickupsItem(433, 1451, EMapPickupsType.Event, EMapPickupsType2.Invalid, "map_eventicon_18", 1452, 0, new byte[15]
		{
			0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
			0, 0, 0, 1, 0
		}, new List<short> { 32 }, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[0], default(PresetItemTemplateId), new sbyte[1], new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), -1, -1, -1, 1453, new int[2] { 1454, 1455 }, new int[2] { 1456, 1457 }, new int[2] { 689, 689 }, new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>
		{
			new PresetItemWithCount("Armor", 328, 1),
			new PresetItemWithCount("Armor", 310, 1)
		}, new List<PresetItemWithCount>
		{
			new PresetItemWithCount("Armor", 337, 1),
			new PresetItemWithCount("Armor", 319, 1)
		}));
		_dataArray.Add(new MapPickupsItem(434, 1458, EMapPickupsType.Event, EMapPickupsType2.Invalid, "map_eventicon_18", 1459, 0, new byte[15]
		{
			0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
			0, 0, 0, 1, 0
		}, new List<short> { 32 }, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[0], default(PresetItemTemplateId), new sbyte[1], new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), -1, -1, -1, 1460, new int[2] { 1461, 1462 }, new int[2] { 1463, 1464 }, new int[2] { 689, 689 }, new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>
		{
			new PresetItemWithCount("Weapon", 169, 1),
			new PresetItemWithCount("Weapon", 349, 1)
		}, new List<PresetItemWithCount>
		{
			new PresetItemWithCount("Weapon", 565, 1),
			new PresetItemWithCount("Weapon", 547, 1),
			new PresetItemWithCount("Weapon", 529, 1)
		}));
		_dataArray.Add(new MapPickupsItem(435, 1465, EMapPickupsType.Event, EMapPickupsType2.Invalid, "map_eventicon_18", 1466, 0, new byte[15]
		{
			0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
			0, 0, 0, 1, 0
		}, new List<short> { 32 }, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[0], default(PresetItemTemplateId), new sbyte[1], new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(14, 50), -1, -1, -1, 1467, new int[2] { 707, 708 }, new int[2] { 709, 710 }, new int[2] { 689, 711 }, new List<PresetItemWithCount>(), new List<ResourceInfo>
		{
			default(ResourceInfo),
			new ResourceInfo(7, 1000)
		}, new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>
		{
			new PresetItemWithCount("Clothing", 58, 1)
		}, new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(436, 1468, EMapPickupsType.Event, EMapPickupsType2.Invalid, "map_eventicon_18", 1469, 0, new byte[15]
		{
			0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
			0, 0, 0, 1, 0
		}, new List<short> { 32 }, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[0], default(PresetItemTemplateId), new sbyte[1], new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(14, 100), -1, -1, -1, 1470, new int[2] { 1471, 1472 }, new int[2] { 1473, 1474 }, new int[2] { 689, 711 }, new List<PresetItemWithCount>(), new List<ResourceInfo>
		{
			default(ResourceInfo),
			new ResourceInfo(7, 2000)
		}, new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>
		{
			new PresetItemWithCount("Clothing", 59, 1)
		}, new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(437, 1482, EMapPickupsType.Event, EMapPickupsType2.Invalid, "map_eventicon_18", 1483, 0, new byte[15]
		{
			0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
			0, 0, 0, 1, 0
		}, new List<short> { 32 }, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[0], default(PresetItemTemplateId), new sbyte[1], new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(14, 300), -1, -1, -1, 1484, new int[2] { 1485, 846 }, new int[2] { 1486, 1487 }, new int[2] { 689, 711 }, new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int> { -1, 5000 }, new List<PresetItemWithCount>
		{
			new PresetItemWithCount("SkillBook", 233, 1),
			new PresetItemWithCount("SkillBook", 234, 1),
			new PresetItemWithCount("SkillBook", 235, 1),
			new PresetItemWithCount("SkillBook", 334, 1),
			new PresetItemWithCount("SkillBook", 335, 1),
			new PresetItemWithCount("SkillBook", 336, 1),
			new PresetItemWithCount("SkillBook", 448, 1),
			new PresetItemWithCount("SkillBook", 449, 1),
			new PresetItemWithCount("SkillBook", 450, 1),
			new PresetItemWithCount("SkillBook", 527, 1),
			new PresetItemWithCount("SkillBook", 528, 1),
			new PresetItemWithCount("SkillBook", 529, 1),
			new PresetItemWithCount("SkillBook", 656, 1),
			new PresetItemWithCount("SkillBook", 657, 1),
			new PresetItemWithCount("SkillBook", 658, 1),
			new PresetItemWithCount("SkillBook", 759, 1),
			new PresetItemWithCount("SkillBook", 760, 1),
			new PresetItemWithCount("SkillBook", 761, 1)
		}, new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(438, 1488, EMapPickupsType.Event, EMapPickupsType2.Invalid, "map_eventicon_18", 1489, 0, new byte[15]
		{
			0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
			0, 0, 0, 1, 0
		}, new List<short> { 32 }, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[0], default(PresetItemTemplateId), new sbyte[1], new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(14, 500), -1, -1, -1, 1490, new int[2] { 1491, 1492 }, new int[2] { 1493, 1494 }, new int[2] { 689, 711 }, new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int> { -1, 10000 }, new List<PresetItemWithCount>
		{
			new PresetItemWithCount("SkillBook", 236, 1),
			new PresetItemWithCount("SkillBook", 237, 1),
			new PresetItemWithCount("SkillBook", 238, 1),
			new PresetItemWithCount("SkillBook", 337, 1),
			new PresetItemWithCount("SkillBook", 338, 1),
			new PresetItemWithCount("SkillBook", 339, 1),
			new PresetItemWithCount("SkillBook", 451, 1),
			new PresetItemWithCount("SkillBook", 452, 1),
			new PresetItemWithCount("SkillBook", 453, 1),
			new PresetItemWithCount("SkillBook", 530, 1),
			new PresetItemWithCount("SkillBook", 531, 1),
			new PresetItemWithCount("SkillBook", 532, 1),
			new PresetItemWithCount("SkillBook", 659, 1),
			new PresetItemWithCount("SkillBook", 660, 1),
			new PresetItemWithCount("SkillBook", 661, 1),
			new PresetItemWithCount("SkillBook", 762, 1),
			new PresetItemWithCount("SkillBook", 763, 1),
			new PresetItemWithCount("SkillBook", 764, 1)
		}, new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(439, 1495, EMapPickupsType.Event, EMapPickupsType2.Invalid, "map_eventicon_18", 1496, 0, new byte[15]
		{
			0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
			0, 0, 0, 1, 0
		}, new List<short> { 32 }, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[0], default(PresetItemTemplateId), new sbyte[1], new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(14, 1000), -1, -1, -1, 1497, new int[2] { 1498, 1499 }, new int[2] { 1500, 1501 }, new int[2] { 689, 711 }, new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int> { -1, 20000 }, new List<PresetItemWithCount>
		{
			new PresetItemWithCount("SkillBook", 239, 1),
			new PresetItemWithCount("SkillBook", 240, 1),
			new PresetItemWithCount("SkillBook", 454, 1),
			new PresetItemWithCount("SkillBook", 455, 1),
			new PresetItemWithCount("SkillBook", 533, 1),
			new PresetItemWithCount("SkillBook", 534, 1),
			new PresetItemWithCount("SkillBook", 765, 1),
			new PresetItemWithCount("SkillBook", 766, 1)
		}, new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(440, 1502, EMapPickupsType.Event, EMapPickupsType2.Invalid, "map_eventicon_18", 1503, 0, new byte[15]
		{
			0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
			0, 0, 0, 0, 1
		}, new List<short> { 33 }, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[0], default(PresetItemTemplateId), new sbyte[1], new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), -1, -1, -1, 1504, new int[2] { 1505, 1506 }, new int[2] { 1507, 1508 }, new int[2] { 689, 689 }, new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>
		{
			new PresetItemWithCount("Accessory", 76, 1)
		}, new List<PresetItemWithCount>
		{
			new PresetItemWithCount("Accessory", 166, 1)
		}));
		_dataArray.Add(new MapPickupsItem(441, 1509, EMapPickupsType.Event, EMapPickupsType2.Invalid, "map_eventicon_18", 1510, 0, new byte[15]
		{
			0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
			0, 0, 0, 0, 1
		}, new List<short> { 33 }, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[0], default(PresetItemTemplateId), new sbyte[1], new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), -1, -1, -1, 1511, new int[2] { 846, 1512 }, new int[2] { 1513, 1514 }, new int[2] { 689, 689 }, new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>
		{
			new PresetItemWithCount("Armor", 202, 1),
			new PresetItemWithCount("Armor", 211, 1)
		}, new List<PresetItemWithCount>
		{
			new PresetItemWithCount("Armor", 220, 1),
			new PresetItemWithCount("Armor", 229, 1)
		}));
		_dataArray.Add(new MapPickupsItem(442, 1515, EMapPickupsType.Event, EMapPickupsType2.Invalid, "map_eventicon_18", 1516, 0, new byte[15]
		{
			0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
			0, 0, 0, 0, 1
		}, new List<short> { 33 }, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[0], default(PresetItemTemplateId), new sbyte[1], new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), -1, -1, -1, 1517, new int[2] { 1419, 1518 }, new int[2] { 1519, 1520 }, new int[2] { 689, 689 }, new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>
		{
			new PresetItemWithCount("Weapon", 16, 1)
		}, new List<PresetItemWithCount>
		{
			new PresetItemWithCount("Weapon", 295, 1)
		}));
		_dataArray.Add(new MapPickupsItem(443, 1521, EMapPickupsType.Event, EMapPickupsType2.Invalid, "map_eventicon_18", 1522, 0, new byte[15]
		{
			0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
			0, 0, 0, 0, 1
		}, new List<short> { 33 }, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[0], default(PresetItemTemplateId), new sbyte[1], new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(15, 50), -1, -1, -1, 1523, new int[2] { 707, 708 }, new int[2] { 709, 710 }, new int[2] { 689, 711 }, new List<PresetItemWithCount>(), new List<ResourceInfo>
		{
			default(ResourceInfo),
			new ResourceInfo(7, 1000)
		}, new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>
		{
			new PresetItemWithCount("Clothing", 61, 1)
		}, new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(444, 1524, EMapPickupsType.Event, EMapPickupsType2.Invalid, "map_eventicon_18", 1525, 0, new byte[15]
		{
			0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
			0, 0, 0, 0, 1
		}, new List<short> { 33 }, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[0], default(PresetItemTemplateId), new sbyte[1], new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(15, 100), -1, -1, -1, 1526, new int[2] { 1527, 1528 }, new int[2] { 1529, 1530 }, new int[2] { 689, 711 }, new List<PresetItemWithCount>(), new List<ResourceInfo>
		{
			default(ResourceInfo),
			new ResourceInfo(7, 2000)
		}, new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>
		{
			new PresetItemWithCount("Clothing", 62, 1)
		}, new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(445, 1538, EMapPickupsType.Event, EMapPickupsType2.Invalid, "map_eventicon_18", 1539, 0, new byte[15]
		{
			0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
			0, 0, 0, 0, 1
		}, new List<short> { 33 }, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[0], default(PresetItemTemplateId), new sbyte[1], new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(15, 300), -1, -1, -1, 1540, new int[2] { 1541, 1306 }, new int[2] { 1542, 1543 }, new int[2] { 689, 711 }, new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int> { -1, 5000 }, new List<PresetItemWithCount>
		{
			new PresetItemWithCount("SkillBook", 241, 1),
			new PresetItemWithCount("SkillBook", 242, 1),
			new PresetItemWithCount("SkillBook", 243, 1),
			new PresetItemWithCount("SkillBook", 340, 1),
			new PresetItemWithCount("SkillBook", 341, 1),
			new PresetItemWithCount("SkillBook", 342, 1),
			new PresetItemWithCount("SkillBook", 456, 1),
			new PresetItemWithCount("SkillBook", 457, 1),
			new PresetItemWithCount("SkillBook", 458, 1),
			new PresetItemWithCount("SkillBook", 536, 1),
			new PresetItemWithCount("SkillBook", 537, 1),
			new PresetItemWithCount("SkillBook", 538, 1),
			new PresetItemWithCount("SkillBook", 606, 1),
			new PresetItemWithCount("SkillBook", 607, 1),
			new PresetItemWithCount("SkillBook", 608, 1),
			new PresetItemWithCount("SkillBook", 630, 1),
			new PresetItemWithCount("SkillBook", 631, 1),
			new PresetItemWithCount("SkillBook", 632, 1),
			new PresetItemWithCount("SkillBook", 662, 1),
			new PresetItemWithCount("SkillBook", 663, 1),
			new PresetItemWithCount("SkillBook", 664, 1)
		}, new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(446, 1544, EMapPickupsType.Event, EMapPickupsType2.Invalid, "map_eventicon_18", 1545, 0, new byte[15]
		{
			0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
			0, 0, 0, 0, 1
		}, new List<short> { 33 }, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[0], default(PresetItemTemplateId), new sbyte[1], new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(15, 500), -1, -1, -1, 1546, new int[2] { 1547, 1548 }, new int[2] { 1549, 1550 }, new int[2] { 689, 711 }, new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int> { -1, 10000 }, new List<PresetItemWithCount>
		{
			new PresetItemWithCount("SkillBook", 244, 1),
			new PresetItemWithCount("SkillBook", 245, 1),
			new PresetItemWithCount("SkillBook", 246, 1),
			new PresetItemWithCount("SkillBook", 343, 1),
			new PresetItemWithCount("SkillBook", 344, 1),
			new PresetItemWithCount("SkillBook", 459, 1),
			new PresetItemWithCount("SkillBook", 460, 1),
			new PresetItemWithCount("SkillBook", 461, 1),
			new PresetItemWithCount("SkillBook", 539, 1),
			new PresetItemWithCount("SkillBook", 540, 1),
			new PresetItemWithCount("SkillBook", 541, 1),
			new PresetItemWithCount("SkillBook", 609, 1),
			new PresetItemWithCount("SkillBook", 610, 1),
			new PresetItemWithCount("SkillBook", 611, 1),
			new PresetItemWithCount("SkillBook", 633, 1),
			new PresetItemWithCount("SkillBook", 634, 1),
			new PresetItemWithCount("SkillBook", 635, 1),
			new PresetItemWithCount("SkillBook", 665, 1),
			new PresetItemWithCount("SkillBook", 666, 1),
			new PresetItemWithCount("SkillBook", 667, 1)
		}, new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(447, 1551, EMapPickupsType.Event, EMapPickupsType2.Invalid, "map_eventicon_18", 1552, 0, new byte[15]
		{
			0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
			0, 0, 0, 0, 1
		}, new List<short> { 33 }, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[0], default(PresetItemTemplateId), new sbyte[1], new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(15, 1000), -1, -1, -1, 1553, new int[2] { 1554, 1555 }, new int[2] { 1556, 1557 }, new int[2] { 689, 711 }, new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int> { -1, 20000 }, new List<PresetItemWithCount>
		{
			new PresetItemWithCount("SkillBook", 247, 1),
			new PresetItemWithCount("SkillBook", 248, 1),
			new PresetItemWithCount("SkillBook", 462, 1),
			new PresetItemWithCount("SkillBook", 463, 1),
			new PresetItemWithCount("SkillBook", 542, 1),
			new PresetItemWithCount("SkillBook", 543, 1),
			new PresetItemWithCount("SkillBook", 612, 1),
			new PresetItemWithCount("SkillBook", 636, 1),
			new PresetItemWithCount("SkillBook", 637, 1),
			new PresetItemWithCount("SkillBook", 668, 1)
		}, new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(448, 1558, EMapPickupsType.Event, EMapPickupsType2.Invalid, "map_eventicon_18", 1559, 0, new byte[15]
		{
			1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
			1, 1, 1, 1, 1
		}, new List<short>
		{
			57, 58, 59, 60, 61, 62, 69, 70, 71, 72,
			73, 74, 81, 82, 83, 84, 85, 86, 103, 104,
			105, 106, 107, 108
		}, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[0], default(PresetItemTemplateId), new sbyte[1], new short[6], new bool[12]
		{
			false, true, true, true, false, false, false, false, false, false,
			false, false
		}, -1, new OrganizationApproving(), -1, -1, -1, 1560, new int[2] { 1561, 1562 }, new int[2] { 1563, 1564 }, new int[2] { 1565, 1565 }, new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>
		{
			new PropertyAndValue(37, 5),
			new PropertyAndValue(34, 5)
		}, new List<int>(), new List<int>(), new List<PresetItemWithCount>(), new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(449, 1566, EMapPickupsType.Event, EMapPickupsType2.Invalid, "map_eventicon_18", 1567, 0, new byte[15]
		{
			1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
			1, 1, 1, 1, 1
		}, new List<short>
		{
			39, 40, 41, 42, 43, 44, 45, 46, 47, 54,
			55, 56, 48, 49, 50, 51, 52, 53
		}, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[0], default(PresetItemTemplateId), new sbyte[1], new short[6], new bool[12]
		{
			false, true, true, true, false, false, false, false, false, false,
			false, false
		}, -1, new OrganizationApproving(), -1, -1, -1, 1568, new int[2] { 1569, 1570 }, new int[2] { 1571, 1572 }, new int[2] { 711, 711 }, new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>
		{
			new PropertyAndValue(34, 5),
			new PropertyAndValue(37, 5)
		}, new List<int>(), new List<int>(), new List<PresetItemWithCount>(), new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(450, 1573, EMapPickupsType.Event, EMapPickupsType2.Invalid, "map_eventicon_18", 1574, 0, new byte[15]
		{
			1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
			1, 1, 1, 1, 1
		}, new List<short>
		{
			39, 40, 41, 42, 43, 44, 45, 46, 47, 54,
			55, 56, 48, 49, 50, 51, 52, 53
		}, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[0], default(PresetItemTemplateId), new sbyte[1], new short[6], new bool[12]
		{
			false, true, true, true, false, false, false, false, false, false,
			false, false
		}, -1, new OrganizationApproving(), -1, -1, -1, 1575, new int[2] { 1576, 1577 }, new int[2] { 1578, 1579 }, new int[2] { 711, 711 }, new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>
		{
			new PropertyAndValue(48, 5),
			new PropertyAndValue(49, 5)
		}, new List<int>(), new List<int>(), new List<PresetItemWithCount>(), new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(451, 1580, EMapPickupsType.Event, EMapPickupsType2.Invalid, "map_eventicon_18", 1581, 0, new byte[15]
		{
			1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
			1, 1, 1, 1, 1
		}, new List<short>
		{
			39, 40, 41, 42, 43, 44, 45, 46, 47, 54,
			55, 56, 48, 49, 50, 51, 52, 53
		}, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[0], default(PresetItemTemplateId), new sbyte[1], new short[6], new bool[12]
		{
			false, true, true, true, false, false, false, false, false, false,
			false, false
		}, -1, new OrganizationApproving(), -1, -1, -1, 1582, new int[2] { 1583, 1584 }, new int[2] { 1585, 1586 }, new int[2] { 711, 711 }, new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>
		{
			new PropertyAndValue(38, 5),
			new PropertyAndValue(44, 5)
		}, new List<int>(), new List<int>(), new List<PresetItemWithCount>(), new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(452, 1587, EMapPickupsType.Event, EMapPickupsType2.Invalid, "map_eventicon_18", 1588, 0, new byte[15]
		{
			1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
			1, 1, 1, 1, 1
		}, new List<short>
		{
			57, 58, 59, 69, 70, 71, 72, 73, 74, 106,
			107, 108
		}, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[0], default(PresetItemTemplateId), new sbyte[1], new short[6], new bool[12]
		{
			false, true, true, true, false, false, false, false, false, false,
			false, false
		}, -1, new OrganizationApproving(), -1, -1, -1, 1589, new int[2] { 1590, 1591 }, new int[2] { 1592, 1593 }, new int[2] { 711, 711 }, new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>
		{
			new PropertyAndValue(41, 5),
			new PropertyAndValue(39, 5)
		}, new List<int>(), new List<int>(), new List<PresetItemWithCount>(), new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(453, 1594, EMapPickupsType.Event, EMapPickupsType2.Invalid, "map_eventicon_18", 1595, 0, new byte[15]
		{
			1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
			1, 1, 1, 1, 1
		}, new List<short>
		{
			42, 43, 44, 69, 70, 71, 78, 79, 80, 87,
			88, 89, 90, 91, 92, 93, 103, 104, 105
		}, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[0], default(PresetItemTemplateId), new sbyte[1], new short[6], new bool[12]
		{
			false, false, false, false, true, true, true, false, false, false,
			false, false
		}, -1, new OrganizationApproving(), -1, -1, -1, 1596, new int[2] { 1597, 1598 }, new int[2] { 1599, 1600 }, new int[2] { 689, 689 }, new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>
		{
			new PresetItemWithCount("TeaWine", 21, 9),
			new PresetItemWithCount("TeaWine", 22, 3),
			new PresetItemWithCount("TeaWine", 23, 1)
		}, new List<PresetItemWithCount>
		{
			new PresetItemWithCount("TeaWine", 30, 9),
			new PresetItemWithCount("TeaWine", 31, 3),
			new PresetItemWithCount("TeaWine", 32, 1)
		}));
		_dataArray.Add(new MapPickupsItem(454, 1601, EMapPickupsType.Event, EMapPickupsType2.Invalid, "map_eventicon_18", 1602, 0, new byte[15]
		{
			1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
			1, 1, 1, 1, 1
		}, new List<short>
		{
			57, 58, 59, 60, 61, 62, 69, 70, 71, 72,
			73, 74, 81, 103, 104, 105, 106, 107, 108
		}, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[0], default(PresetItemTemplateId), new sbyte[1], new short[6], new bool[12]
		{
			false, false, false, false, true, true, true, false, false, false,
			false, false
		}, -1, new OrganizationApproving(), -1, -1, -1, 1603, new int[2] { 1604, 1605 }, new int[2] { 1606, 1607 }, new int[2] { 711, 711 }, new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>
		{
			new PropertyAndValue(38, 5),
			new PropertyAndValue(42, 5)
		}, new List<int>(), new List<int>(), new List<PresetItemWithCount>(), new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(455, 1608, EMapPickupsType.Event, EMapPickupsType2.Invalid, "map_eventicon_18", 1609, 0, new byte[15]
		{
			1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
			1, 1, 1, 1, 1
		}, new List<short>
		{
			39, 40, 41, 42, 43, 44, 45, 46, 47, 54,
			55, 56, 48, 49, 50, 51, 52, 53, 57, 58,
			59, 69, 70, 71, 72, 73, 74, 106, 107, 108
		}, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[0], default(PresetItemTemplateId), new sbyte[1], new short[6], new bool[12]
		{
			false, false, false, false, true, true, true, false, false, false,
			false, false
		}, -1, new OrganizationApproving(), -1, -1, -1, 1610, new int[2] { 1611, 1612 }, new int[2] { 1613, 1614 }, new int[2] { 711, 1615 }, new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>
		{
			new PropertyAndValue(36, 5),
			new PropertyAndValue(45, 5)
		}, new List<int>(), new List<int>(), new List<PresetItemWithCount>(), new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(456, 1616, EMapPickupsType.Event, EMapPickupsType2.Invalid, "map_eventicon_18", 1617, 0, new byte[15]
		{
			1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
			1, 1, 1, 1, 1
		}, new List<short>
		{
			1, 2, 3, 4, 5, 6, 7, 8, 9, 10,
			11, 12, 13, 14, 15, 34, 35, 36
		}, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[0], default(PresetItemTemplateId), new sbyte[1], new short[6], new bool[12]
		{
			false, false, false, false, true, true, true, false, false, false,
			false, false
		}, -1, new OrganizationApproving(), -1, -1, -1, 1618, new int[2] { 1619, 1620 }, new int[2] { 1621, 1622 }, new int[2] { 1623, 711 }, new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>
		{
			new PropertyAndValue(43, 5),
			new PropertyAndValue(40, 5)
		}, new List<int>(), new List<int>(), new List<PresetItemWithCount>(), new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(457, 1624, EMapPickupsType.Event, EMapPickupsType2.Invalid, "map_eventicon_18", 1625, 0, new byte[15]
		{
			1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
			1, 1, 1, 1, 1
		}, new List<short> { 97, 98, 99 }, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[0], default(PresetItemTemplateId), new sbyte[1], new short[6], new bool[12]
		{
			false, false, false, false, true, true, true, false, false, false,
			false, false
		}, -1, new OrganizationApproving(), -1, -1, -1, 1626, new int[2] { 1627, 1628 }, new int[2] { 1629, 1630 }, new int[2] { 689, 689 }, new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>
		{
			new PresetItemWithCount("TeaWine", 3, 9),
			new PresetItemWithCount("TeaWine", 4, 3),
			new PresetItemWithCount("TeaWine", 5, 1)
		}, new List<PresetItemWithCount>
		{
			new PresetItemWithCount("TeaWine", 12, 9),
			new PresetItemWithCount("TeaWine", 13, 3),
			new PresetItemWithCount("TeaWine", 14, 1)
		}));
		_dataArray.Add(new MapPickupsItem(458, 1631, EMapPickupsType.Event, EMapPickupsType2.Invalid, "map_eventicon_18", 1632, 0, new byte[15]
		{
			1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
			1, 1, 1, 1, 1
		}, new List<short>
		{
			42, 43, 44, 81, 82, 83, 84, 85, 86, 94,
			95, 96
		}, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[0], default(PresetItemTemplateId), new sbyte[1], new short[6], new bool[12]
		{
			false, false, false, false, false, false, false, true, true, true,
			false, false
		}, -1, new OrganizationApproving(), -1, -1, -1, 1633, new int[2] { 1634, 1635 }, new int[2] { 1636, 1637 }, new int[2] { 711, 711 }, new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>
		{
			new PropertyAndValue(39, 5),
			new PropertyAndValue(35, 5)
		}, new List<int>(), new List<int>(), new List<PresetItemWithCount>(), new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(459, 1638, EMapPickupsType.Event, EMapPickupsType2.Invalid, "map_eventicon_18", 1639, 0, new byte[15]
		{
			1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
			1, 1, 1, 1, 1
		}, new List<short>
		{
			39, 40, 41, 42, 43, 44, 45, 46, 47, 54,
			55, 56, 48, 49, 50, 51, 52, 53
		}, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[0], default(PresetItemTemplateId), new sbyte[1], new short[6], new bool[12]
		{
			false, false, false, false, false, false, false, true, true, true,
			false, false
		}, -1, new OrganizationApproving(), -1, -1, -1, 1640, new int[2] { 1641, 1188 }, new int[2] { 1642, 1643 }, new int[2] { 711, 689 }, new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>
		{
			new PropertyAndValue(111, 150),
			default(PropertyAndValue)
		}, new List<int>(), new List<int>(), new List<PresetItemWithCount>(), new List<PresetItemWithCount>
		{
			new PresetItemWithCount("Misc", 9, 50)
		}));
		_dataArray.Add(new MapPickupsItem(460, 1644, EMapPickupsType.Event, EMapPickupsType2.Invalid, "map_eventicon_18", 1645, 0, new byte[15]
		{
			1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
			1, 1, 1, 1, 1
		}, new List<short>
		{
			75, 76, 77, 81, 82, 83, 84, 85, 86, 94,
			95, 96
		}, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[0], default(PresetItemTemplateId), new sbyte[1], new short[6], new bool[12]
		{
			false, false, false, false, false, false, false, true, true, true,
			false, false
		}, -1, new OrganizationApproving(), -1, -1, -1, 1646, new int[2] { 1647, 1648 }, new int[2] { 1649, 1650 }, new int[2] { 711, 711 }, new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>
		{
			new PropertyAndValue(35, 5),
			new PropertyAndValue(44, 5)
		}, new List<int>(), new List<int>(), new List<PresetItemWithCount>(), new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(461, 1651, EMapPickupsType.Event, EMapPickupsType2.Invalid, "map_eventicon_18", 1652, 0, new byte[15]
		{
			1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
			1, 1, 1, 1, 1
		}, new List<short>
		{
			39, 40, 41, 42, 43, 44, 45, 46, 47, 54,
			55, 56, 48, 49, 50, 51, 52, 53, 34, 35,
			36
		}, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[0], default(PresetItemTemplateId), new sbyte[1], new short[6], new bool[12]
		{
			false, false, false, false, false, false, false, true, true, true,
			false, false
		}, -1, new OrganizationApproving(), -1, -1, -1, 1653, new int[2] { 1654, 1655 }, new int[2] { 1656, 1657 }, new int[2] { 711, 711 }, new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>
		{
			new PropertyAndValue(48, 5),
			new PropertyAndValue(41, 5)
		}, new List<int>(), new List<int>(), new List<PresetItemWithCount>(), new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(462, 1658, EMapPickupsType.Event, EMapPickupsType2.Invalid, "map_eventicon_18", 1659, 0, new byte[15]
		{
			1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
			1, 1, 1, 1, 1
		}, new List<short>
		{
			57, 58, 59, 66, 67, 68, 69, 70, 71, 90,
			91, 92, 106, 107, 108
		}, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[0], default(PresetItemTemplateId), new sbyte[1], new short[6], new bool[12]
		{
			false, false, false, false, false, false, false, true, true, true,
			false, false
		}, -1, new OrganizationApproving(), -1, -1, -1, 1660, new int[2] { 1661, 1662 }, new int[2] { 1663, 1664 }, new int[2] { 711, 711 }, new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>
		{
			new PropertyAndValue(40, 5),
			new PropertyAndValue(43, 5)
		}, new List<int>(), new List<int>(), new List<PresetItemWithCount>(), new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(463, 1665, EMapPickupsType.Event, EMapPickupsType2.Invalid, "map_eventicon_18", 1666, 0, new byte[15]
		{
			1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
			1, 1, 1, 1, 1
		}, new List<short>
		{
			57, 58, 59, 69, 70, 71, 90, 91, 92, 106,
			107, 108
		}, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[0], default(PresetItemTemplateId), new sbyte[1], new short[6], new bool[12]
		{
			true, false, false, false, false, false, false, false, false, false,
			true, true
		}, -1, new OrganizationApproving(), -1, -1, -1, 1667, new int[2] { 1668, 1669 }, new int[2] { 1670, 1671 }, new int[2] { 1623, 1623 }, new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>
		{
			new PropertyAndValue(42, 5),
			new PropertyAndValue(45, 5)
		}, new List<int>(), new List<int>(), new List<PresetItemWithCount>(), new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(464, 1672, EMapPickupsType.Event, EMapPickupsType2.Invalid, "map_eventicon_18", 1673, 0, new byte[15]
		{
			1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
			1, 1, 1, 1, 1
		}, new List<short>
		{
			69, 70, 71, 81, 82, 83, 84, 85, 86, 100,
			101, 102
		}, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[0], default(PresetItemTemplateId), new sbyte[1], new short[6], new bool[12]
		{
			true, false, false, false, false, false, false, false, false, false,
			true, true
		}, -1, new OrganizationApproving(), -1, -1, -1, 1674, new int[2] { 1675, 1676 }, new int[2] { 1677, 1678 }, new int[2] { 689, 689 }, new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>
		{
			new PresetItemWithCount("Material", 140, 9),
			new PresetItemWithCount("Material", 141, 3),
			new PresetItemWithCount("Material", 142, 1),
			new PresetItemWithCount("Material", 144, 9),
			new PresetItemWithCount("Material", 145, 3),
			new PresetItemWithCount("Material", 146, 1),
			new PresetItemWithCount("Material", 148, 9),
			new PresetItemWithCount("Material", 149, 3),
			new PresetItemWithCount("Material", 150, 1),
			new PresetItemWithCount("Material", 152, 9),
			new PresetItemWithCount("Material", 153, 3),
			new PresetItemWithCount("Material", 154, 1),
			new PresetItemWithCount("Material", 176, 9),
			new PresetItemWithCount("Material", 177, 3),
			new PresetItemWithCount("Material", 178, 1),
			new PresetItemWithCount("Material", 180, 9),
			new PresetItemWithCount("Material", 181, 3),
			new PresetItemWithCount("Material", 182, 1),
			new PresetItemWithCount("Material", 184, 9),
			new PresetItemWithCount("Material", 185, 3),
			new PresetItemWithCount("Material", 186, 1),
			new PresetItemWithCount("Material", 188, 9),
			new PresetItemWithCount("Material", 189, 3),
			new PresetItemWithCount("Material", 190, 1),
			new PresetItemWithCount("Material", 208, 9),
			new PresetItemWithCount("Material", 209, 3),
			new PresetItemWithCount("Material", 210, 1),
			new PresetItemWithCount("Material", 212, 9),
			new PresetItemWithCount("Material", 213, 3),
			new PresetItemWithCount("Material", 214, 1),
			new PresetItemWithCount("Material", 220, 9),
			new PresetItemWithCount("Material", 221, 3),
			new PresetItemWithCount("Material", 222, 1),
			new PresetItemWithCount("Material", 232, 9),
			new PresetItemWithCount("Material", 233, 3),
			new PresetItemWithCount("Material", 234, 1)
		}, new List<PresetItemWithCount>
		{
			new PresetItemWithCount("Material", 192, 9),
			new PresetItemWithCount("Material", 193, 3),
			new PresetItemWithCount("Material", 194, 1),
			new PresetItemWithCount("Material", 196, 9),
			new PresetItemWithCount("Material", 197, 3),
			new PresetItemWithCount("Material", 198, 1),
			new PresetItemWithCount("Material", 200, 9),
			new PresetItemWithCount("Material", 201, 3),
			new PresetItemWithCount("Material", 202, 1),
			new PresetItemWithCount("Material", 204, 9),
			new PresetItemWithCount("Material", 205, 3),
			new PresetItemWithCount("Material", 206, 1),
			new PresetItemWithCount("Material", 216, 9),
			new PresetItemWithCount("Material", 217, 3),
			new PresetItemWithCount("Material", 218, 1),
			new PresetItemWithCount("Material", 224, 9),
			new PresetItemWithCount("Material", 225, 3),
			new PresetItemWithCount("Material", 226, 1),
			new PresetItemWithCount("Material", 228, 9),
			new PresetItemWithCount("Material", 229, 3),
			new PresetItemWithCount("Material", 230, 1),
			new PresetItemWithCount("Material", 156, 9),
			new PresetItemWithCount("Material", 157, 3),
			new PresetItemWithCount("Material", 158, 1),
			new PresetItemWithCount("Material", 160, 9),
			new PresetItemWithCount("Material", 161, 3),
			new PresetItemWithCount("Material", 162, 1),
			new PresetItemWithCount("Material", 164, 9),
			new PresetItemWithCount("Material", 165, 3),
			new PresetItemWithCount("Material", 166, 1),
			new PresetItemWithCount("Material", 168, 9),
			new PresetItemWithCount("Material", 169, 3),
			new PresetItemWithCount("Material", 170, 1),
			new PresetItemWithCount("Material", 172, 9),
			new PresetItemWithCount("Material", 173, 3),
			new PresetItemWithCount("Material", 174, 1)
		}));
		_dataArray.Add(new MapPickupsItem(465, 1679, EMapPickupsType.Event, EMapPickupsType2.Invalid, "map_eventicon_18", 1680, 0, new byte[15]
		{
			1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
			1, 1, 1, 1, 1
		}, new List<short>
		{
			1, 2, 3, 4, 5, 6, 7, 8, 9, 10,
			11, 12, 13, 14, 15, 34, 35, 36
		}, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[0], default(PresetItemTemplateId), new sbyte[1], new short[6], new bool[12]
		{
			true, false, false, false, false, false, false, false, false, false,
			true, true
		}, -1, new OrganizationApproving(), -1, -1, -1, 1681, new int[2] { 1682, 1683 }, new int[2] { 1684, 1685 }, new int[2] { 711, 711 }, new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>
		{
			new PropertyAndValue(49, 5),
			new PropertyAndValue(36, 5)
		}, new List<int>(), new List<int>(), new List<PresetItemWithCount>(), new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(466, 1686, EMapPickupsType.Event, EMapPickupsType2.Invalid, "map_eventicon_18", 1687, 0, new byte[15]
		{
			1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
			1, 1, 1, 1, 1
		}, new List<short> { 87, 88, 89, 90, 91, 92, 93, 103, 104, 105 }, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[0], default(PresetItemTemplateId), new sbyte[1], new short[6], new bool[12]
		{
			true, false, false, false, false, false, false, false, false, false,
			true, true
		}, -1, new OrganizationApproving(), -1, -1, -1, 1688, new int[2] { 1654, 1689 }, new int[2] { 1690, 1691 }, new int[2] { 1623, 1623 }, new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>
		{
			new PropertyAndValue(47, 5),
			new PropertyAndValue(46, 5)
		}, new List<int>(), new List<int>(), new List<PresetItemWithCount>(), new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(467, 1692, EMapPickupsType.Event, EMapPickupsType2.Invalid, "map_eventicon_18", 1693, 0, new byte[15]
		{
			1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
			1, 1, 1, 1, 1
		}, new List<short>
		{
			57, 58, 59, 69, 70, 71, 72, 73, 74, 106,
			107, 108
		}, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[0], default(PresetItemTemplateId), new sbyte[1], new short[6], new bool[12]
		{
			true, false, false, false, false, false, false, false, false, false,
			true, true
		}, -1, new OrganizationApproving(), -1, -1, -1, 1694, new int[2] { 1695, 1696 }, new int[2] { 1697, 1698 }, new int[2] { 1699, 711 }, new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>
		{
			new PropertyAndValue(47, 5),
			new PropertyAndValue(46, 5)
		}, new List<int>(), new List<int>(), new List<PresetItemWithCount>(), new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(468, 719, EMapPickupsType.Event, EMapPickupsType2.Invalid, "map_eventicon_18", 720, 0, new byte[15]
		{
			1, 0, 0, 0, 0, 0, 0, 0, 0, 0,
			0, 0, 0, 0, 0
		}, new List<short> { 19 }, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[0], default(PresetItemTemplateId), new sbyte[1], new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(1, 200), -1, -1, -1, 721, new int[2] { 722, 723 }, new int[2] { 724, 725 }, new int[2] { 689, 711 }, new List<PresetItemWithCount>(), new List<ResourceInfo>
		{
			default(ResourceInfo),
			new ResourceInfo(7, 3000)
		}, new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>
		{
			new PresetItemWithCount("Clothing", 20, 1)
		}, new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(469, 776, EMapPickupsType.Event, EMapPickupsType2.Invalid, "map_eventicon_18", 777, 0, new byte[15]
		{
			0, 1, 0, 0, 0, 0, 0, 0, 0, 0,
			0, 0, 0, 0, 0
		}, new List<short> { 20 }, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[0], default(PresetItemTemplateId), new sbyte[1], new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(2, 200), -1, -1, -1, 778, new int[2] { 779, 780 }, new int[2] { 781, 782 }, new int[2] { 689, 711 }, new List<PresetItemWithCount>(), new List<ResourceInfo>
		{
			default(ResourceInfo),
			new ResourceInfo(7, 3000)
		}, new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>
		{
			new PresetItemWithCount("Clothing", 23, 1)
		}, new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(470, 835, EMapPickupsType.Event, EMapPickupsType2.Invalid, "map_eventicon_18", 836, 0, new byte[15]
		{
			0, 0, 1, 0, 0, 0, 0, 0, 0, 0,
			0, 0, 0, 0, 0
		}, new List<short> { 21 }, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[0], default(PresetItemTemplateId), new sbyte[1], new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(3, 200), -1, -1, -1, 837, new int[2] { 838, 839 }, new int[2] { 840, 841 }, new int[2] { 689, 711 }, new List<PresetItemWithCount>(), new List<ResourceInfo>
		{
			default(ResourceInfo),
			new ResourceInfo(7, 3000)
		}, new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>
		{
			new PresetItemWithCount("Clothing", 26, 1)
		}, new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(471, 894, EMapPickupsType.Event, EMapPickupsType2.Invalid, "map_eventicon_18", 895, 0, new byte[15]
		{
			0, 0, 0, 1, 0, 0, 0, 0, 0, 0,
			0, 0, 0, 0, 0
		}, new List<short> { 22 }, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[0], default(PresetItemTemplateId), new sbyte[1], new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(4, 200), -1, -1, -1, 896, new int[2] { 897, 898 }, new int[2] { 899, 900 }, new int[2] { 689, 711 }, new List<PresetItemWithCount>(), new List<ResourceInfo>
		{
			default(ResourceInfo),
			new ResourceInfo(7, 3000)
		}, new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>
		{
			new PresetItemWithCount("Clothing", 29, 1)
		}, new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(472, 953, EMapPickupsType.Event, EMapPickupsType2.Invalid, "map_eventicon_18", 954, 0, new byte[15]
		{
			0, 0, 0, 0, 1, 0, 0, 0, 0, 0,
			0, 0, 0, 0, 0
		}, new List<short> { 23 }, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[0], default(PresetItemTemplateId), new sbyte[1], new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(5, 200), -1, -1, -1, 955, new int[2] { 956, 957 }, new int[2] { 958, 959 }, new int[2] { 689, 711 }, new List<PresetItemWithCount>(), new List<ResourceInfo>
		{
			default(ResourceInfo),
			new ResourceInfo(7, 3000)
		}, new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>
		{
			new PresetItemWithCount("Clothing", 33, 1)
		}, new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(473, 1011, EMapPickupsType.Event, EMapPickupsType2.Invalid, "map_eventicon_18", 1012, 0, new byte[15]
		{
			0, 0, 0, 0, 0, 1, 0, 0, 0, 0,
			0, 0, 0, 0, 0
		}, new List<short> { 24 }, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[0], default(PresetItemTemplateId), new sbyte[1], new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(6, 200), -1, -1, -1, 1013, new int[2] { 793, 1014 }, new int[2] { 1015, 1016 }, new int[2] { 689, 711 }, new List<PresetItemWithCount>(), new List<ResourceInfo>
		{
			default(ResourceInfo),
			new ResourceInfo(7, 3000)
		}, new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>
		{
			new PresetItemWithCount("Clothing", 36, 1)
		}, new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(474, 1068, EMapPickupsType.Event, EMapPickupsType2.Invalid, "map_eventicon_18", 1069, 0, new byte[15]
		{
			0, 0, 0, 0, 0, 0, 1, 0, 0, 0,
			0, 0, 0, 0, 0
		}, new List<short> { 25 }, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[0], default(PresetItemTemplateId), new sbyte[1], new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(7, 200), -1, -1, -1, 1070, new int[2] { 1071, 1072 }, new int[2] { 1073, 1074 }, new int[2] { 689, 711 }, new List<PresetItemWithCount>(), new List<ResourceInfo>
		{
			default(ResourceInfo),
			new ResourceInfo(7, 3000)
		}, new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>
		{
			new PresetItemWithCount("Clothing", 39, 1)
		}, new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(475, 1127, EMapPickupsType.Event, EMapPickupsType2.Invalid, "map_eventicon_18", 1128, 0, new byte[15]
		{
			0, 0, 0, 0, 0, 0, 0, 1, 0, 0,
			0, 0, 0, 0, 0
		}, new List<short> { 26 }, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[0], default(PresetItemTemplateId), new sbyte[1], new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(8, 200), -1, -1, -1, 1129, new int[2] { 793, 1130 }, new int[2] { 1131, 1132 }, new int[2] { 689, 711 }, new List<PresetItemWithCount>(), new List<ResourceInfo>
		{
			default(ResourceInfo),
			new ResourceInfo(7, 3000)
		}, new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>
		{
			new PresetItemWithCount("Clothing", 42, 1)
		}, new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(476, 1185, EMapPickupsType.Event, EMapPickupsType2.Invalid, "map_eventicon_18", 1186, 0, new byte[15]
		{
			0, 0, 0, 0, 0, 0, 0, 0, 1, 0,
			0, 0, 0, 0, 0
		}, new List<short> { 27 }, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[0], default(PresetItemTemplateId), new sbyte[1], new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(9, 200), -1, -1, -1, 1187, new int[2] { 1188, 1189 }, new int[2] { 1190, 1191 }, new int[2] { 689, 711 }, new List<PresetItemWithCount>(), new List<ResourceInfo>
		{
			default(ResourceInfo),
			new ResourceInfo(7, 3000)
		}, new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>
		{
			new PresetItemWithCount("Clothing", 45, 1)
		}, new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(477, 1244, EMapPickupsType.Event, EMapPickupsType2.Invalid, "map_eventicon_18", 1245, 0, new byte[15]
		{
			0, 0, 0, 0, 0, 0, 0, 0, 0, 1,
			0, 0, 0, 0, 0
		}, new List<short> { 28 }, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[0], default(PresetItemTemplateId), new sbyte[1], new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(10, 200), -1, -1, -1, 1246, new int[2] { 793, 1247 }, new int[2] { 1248, 1249 }, new int[2] { 689, 711 }, new List<PresetItemWithCount>(), new List<ResourceInfo>
		{
			default(ResourceInfo),
			new ResourceInfo(7, 3000)
		}, new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>
		{
			new PresetItemWithCount("Clothing", 48, 1)
		}, new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(478, 1302, EMapPickupsType.Event, EMapPickupsType2.Invalid, "map_eventicon_18", 1303, 0, new byte[15]
		{
			0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
			1, 0, 0, 0, 0
		}, new List<short> { 29 }, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[0], default(PresetItemTemplateId), new sbyte[1], new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(11, 200), -1, -1, -1, 1304, new int[2] { 1305, 1306 }, new int[2] { 1307, 1308 }, new int[2] { 689, 711 }, new List<PresetItemWithCount>(), new List<ResourceInfo>
		{
			default(ResourceInfo),
			new ResourceInfo(7, 3000)
		}, new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>
		{
			new PresetItemWithCount("Clothing", 51, 1)
		}, new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(479, 1360, EMapPickupsType.Event, EMapPickupsType2.Invalid, "map_eventicon_18", 1361, 0, new byte[15]
		{
			0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
			0, 1, 0, 0, 0
		}, new List<short> { 30 }, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[0], default(PresetItemTemplateId), new sbyte[1], new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(12, 200), -1, -1, -1, 1362, new int[2] { 1363, 1364 }, new int[2] { 1365, 1366 }, new int[2] { 689, 711 }, new List<PresetItemWithCount>(), new List<ResourceInfo>
		{
			default(ResourceInfo),
			new ResourceInfo(7, 3000)
		}, new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>
		{
			new PresetItemWithCount("Clothing", 54, 1)
		}, new List<PresetItemWithCount>()));
	}

	private void CreateItems8()
	{
		_dataArray.Add(new MapPickupsItem(480, 1416, EMapPickupsType.Event, EMapPickupsType2.Invalid, "map_eventicon_18", 1417, 0, new byte[15]
		{
			0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
			0, 0, 1, 0, 0
		}, new List<short> { 31 }, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[0], default(PresetItemTemplateId), new sbyte[1], new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(13, 200), -1, -1, -1, 1418, new int[2] { 1419, 1420 }, new int[2] { 1421, 1422 }, new int[2] { 689, 711 }, new List<PresetItemWithCount>(), new List<ResourceInfo>
		{
			default(ResourceInfo),
			new ResourceInfo(7, 3000)
		}, new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>
		{
			new PresetItemWithCount("Clothing", 57, 1)
		}, new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(481, 1475, EMapPickupsType.Event, EMapPickupsType2.Invalid, "map_eventicon_18", 1476, 0, new byte[15]
		{
			0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
			0, 0, 0, 1, 0
		}, new List<short> { 32 }, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[0], default(PresetItemTemplateId), new sbyte[1], new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(14, 200), -1, -1, -1, 1477, new int[2] { 1478, 1479 }, new int[2] { 1480, 1481 }, new int[2] { 689, 711 }, new List<PresetItemWithCount>(), new List<ResourceInfo>
		{
			default(ResourceInfo),
			new ResourceInfo(7, 3000)
		}, new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>
		{
			new PresetItemWithCount("Clothing", 60, 1)
		}, new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(482, 1531, EMapPickupsType.Event, EMapPickupsType2.Invalid, "map_eventicon_18", 1532, 0, new byte[15]
		{
			0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
			0, 0, 0, 0, 1
		}, new List<short> { 33 }, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[0], default(PresetItemTemplateId), new sbyte[1], new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(15, 200), -1, -1, -1, 1533, new int[2] { 1534, 1535 }, new int[2] { 1536, 1537 }, new int[2] { 689, 711 }, new List<PresetItemWithCount>(), new List<ResourceInfo>
		{
			default(ResourceInfo),
			new ResourceInfo(7, 3000)
		}, new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>
		{
			new PresetItemWithCount("Clothing", 63, 1)
		}, new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(483, 1700, EMapPickupsType.Event, EMapPickupsType2.Invalid, "map_eventicon_18", 1701, 0, new byte[15]
		{
			1, 0, 0, 0, 0, 0, 0, 0, 0, 0,
			0, 0, 0, 0, 0
		}, new List<short> { 57, 58, 59, 69, 70, 71, 72, 73, 74 }, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[0], default(PresetItemTemplateId), new sbyte[1], new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), -1, -1, -1, 1702, new int[2] { 1703, 1704 }, new int[2] { 1705, 1706 }, new int[2] { 689, 689 }, new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>
		{
			new PresetItemWithCount("Material", 3, 1)
		}, new List<PresetItemWithCount>
		{
			new PresetItemWithCount("Material", 10, 1)
		}));
		_dataArray.Add(new MapPickupsItem(484, 1707, EMapPickupsType.Event, EMapPickupsType2.Invalid, "map_eventicon_18", 1708, 0, new byte[15]
		{
			1, 0, 0, 0, 0, 0, 0, 0, 0, 0,
			0, 0, 0, 0, 0
		}, new List<short> { 87, 88, 89, 90, 91, 92, 93, 103, 104, 105 }, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[0], default(PresetItemTemplateId), new sbyte[1], new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), -1, -1, -1, 1709, new int[2] { 1710, 1711 }, new int[2] { 1712, 1713 }, new int[2] { 689, 689 }, new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>
		{
			new PresetItemWithCount("Material", 60, 1)
		}, new List<PresetItemWithCount>
		{
			new PresetItemWithCount("Material", 81, 1)
		}));
		_dataArray.Add(new MapPickupsItem(485, 1714, EMapPickupsType.Event, EMapPickupsType2.Invalid, "map_eventicon_18", 1715, 0, new byte[15]
		{
			1, 0, 0, 0, 0, 0, 0, 0, 0, 0,
			0, 0, 0, 0, 0
		}, new List<short>
		{
			1, 2, 3, 4, 5, 6, 7, 8, 9, 10,
			11, 12, 13, 14, 15, 34, 35, 36
		}, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[0], default(PresetItemTemplateId), new sbyte[1], new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), -1, -1, -1, 1716, new int[2] { 1172, 1717 }, new int[2] { 1718, 1719 }, new int[2] { 689, 711 }, new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int> { -1, 5000 }, new List<PresetItemWithCount>
		{
			new PresetItemWithCount("SkillBook", 11, 1),
			new PresetItemWithCount("SkillBook", 12, 1),
			new PresetItemWithCount("SkillBook", 13, 1),
			new PresetItemWithCount("SkillBook", 74, 1),
			new PresetItemWithCount("SkillBook", 75, 1),
			new PresetItemWithCount("SkillBook", 76, 1),
			new PresetItemWithCount("SkillBook", 119, 1),
			new PresetItemWithCount("SkillBook", 120, 1),
			new PresetItemWithCount("SkillBook", 121, 1)
		}, new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(486, 1720, EMapPickupsType.Event, EMapPickupsType2.Invalid, "map_eventicon_18", 1721, 0, new byte[15]
		{
			0, 1, 0, 0, 0, 0, 0, 0, 0, 0,
			0, 0, 0, 0, 0
		}, new List<short>
		{
			57, 58, 59, 69, 70, 71, 63, 64, 65, 66,
			67, 68
		}, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[0], default(PresetItemTemplateId), new sbyte[1], new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), -1, -1, -1, 1722, new int[2] { 1723, 1724 }, new int[2] { 1725, 1726 }, new int[2] { 689, 689 }, new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>
		{
			new PresetItemWithCount("Material", 52, 1)
		}, new List<PresetItemWithCount>
		{
			new PresetItemWithCount("Material", 45, 1)
		}));
		_dataArray.Add(new MapPickupsItem(487, 1727, EMapPickupsType.Event, EMapPickupsType2.Invalid, "map_eventicon_18", 1728, 0, new byte[15]
		{
			0, 1, 0, 0, 0, 0, 0, 0, 0, 0,
			0, 0, 0, 0, 0
		}, new List<short>
		{
			1, 2, 3, 4, 5, 6, 7, 8, 9, 10,
			11, 12, 13, 14, 15, 34, 35, 36
		}, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[0], default(PresetItemTemplateId), new sbyte[1], new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), -1, -1, -1, 1729, new int[2] { 1730, 1731 }, new int[2] { 1732, 1733 }, new int[2] { 689, 689 }, new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>
		{
			new PresetItemWithCount("Food", 55, 1),
			new PresetItemWithCount("Food", 63, 1),
			new PresetItemWithCount("Food", 70, 1),
			new PresetItemWithCount("Food", 76, 1),
			new PresetItemWithCount("Food", 81, 1)
		}, new List<PresetItemWithCount>
		{
			new PresetItemWithCount("Food", 139, 1),
			new PresetItemWithCount("Food", 147, 1),
			new PresetItemWithCount("Food", 154, 1),
			new PresetItemWithCount("Food", 160, 1),
			new PresetItemWithCount("Food", 165, 1)
		}));
		_dataArray.Add(new MapPickupsItem(488, 1734, EMapPickupsType.Event, EMapPickupsType2.Invalid, "map_eventicon_18", 1735, 0, new byte[15]
		{
			0, 1, 0, 0, 0, 0, 0, 0, 0, 0,
			0, 0, 0, 0, 0
		}, new List<short> { 63, 64, 65, 90, 91, 92, 106, 107, 108 }, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[0], default(PresetItemTemplateId), new sbyte[1], new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), -1, -1, -1, 1736, new int[2] { 1737, 1738 }, new int[2] { 1739, 1740 }, new int[2] { 689, 711 }, new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int> { -1, 5000 }, new List<PresetItemWithCount>
		{
			new PresetItemWithCount("SkillBook", 110, 1),
			new PresetItemWithCount("SkillBook", 111, 1),
			new PresetItemWithCount("SkillBook", 112, 1),
			new PresetItemWithCount("SkillBook", 119, 1),
			new PresetItemWithCount("SkillBook", 120, 1),
			new PresetItemWithCount("SkillBook", 121, 1),
			new PresetItemWithCount("SkillBook", 128, 1),
			new PresetItemWithCount("SkillBook", 129, 1),
			new PresetItemWithCount("SkillBook", 130, 1)
		}, new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(489, 1741, EMapPickupsType.Event, EMapPickupsType2.Invalid, "map_eventicon_18", 1742, 0, new byte[15]
		{
			0, 0, 1, 0, 0, 0, 0, 0, 0, 0,
			0, 0, 0, 0, 0
		}, new List<short>
		{
			39, 40, 41, 42, 43, 44, 45, 46, 47, 54,
			55, 56, 48, 49, 50, 51, 52, 53
		}, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[0], default(PresetItemTemplateId), new sbyte[1], new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), -1, -1, -1, 1743, new int[2] { 1562, 1744 }, new int[2] { 1745, 1746 }, new int[2] { 689, 689 }, new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>
		{
			new PresetItemWithCount("Material", 52, 1)
		}, new List<PresetItemWithCount>
		{
			new PresetItemWithCount("Material", 45, 1)
		}));
		_dataArray.Add(new MapPickupsItem(490, 1747, EMapPickupsType.Event, EMapPickupsType2.Invalid, "map_eventicon_18", 1748, 0, new byte[15]
		{
			0, 0, 1, 0, 0, 0, 0, 0, 0, 0,
			0, 0, 0, 0, 0
		}, new List<short>
		{
			39, 40, 41, 42, 43, 44, 45, 46, 47, 54,
			55, 56, 48, 49, 50, 51, 52, 53, 1, 2,
			3, 4, 5, 6, 7, 8, 9, 10, 11, 12,
			13, 14, 15, 34, 35, 36, 37
		}, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[0], default(PresetItemTemplateId), new sbyte[1], new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), -1, -1, -1, 1749, new int[2] { 1750, 1751 }, new int[2] { 1752, 1753 }, new int[2] { 689, 689 }, new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>
		{
			new PresetItemWithCount("Material", 74, 1)
		}, new List<PresetItemWithCount>
		{
			new PresetItemWithCount("Material", 81, 1)
		}));
		_dataArray.Add(new MapPickupsItem(491, 1754, EMapPickupsType.Event, EMapPickupsType2.Invalid, "map_eventicon_18", 1755, 0, new byte[15]
		{
			0, 0, 1, 0, 0, 0, 0, 0, 0, 0,
			0, 0, 0, 0, 0
		}, new List<short>
		{
			39, 40, 41, 42, 43, 44, 45, 46, 47, 54,
			55, 56, 48, 49, 50, 51, 52, 53, 1, 2,
			3, 4, 5, 6, 7, 8, 9, 10, 11, 12,
			13, 14, 15, 34, 35, 36, 37
		}, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[0], default(PresetItemTemplateId), new sbyte[1], new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), -1, -1, -1, 1756, new int[2] { 1757, 1758 }, new int[2] { 1759, 1760 }, new int[2] { 689, 711 }, new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int> { -1, 5000 }, new List<PresetItemWithCount>
		{
			new PresetItemWithCount("SkillBook", 2, 1),
			new PresetItemWithCount("SkillBook", 3, 1),
			new PresetItemWithCount("SkillBook", 4, 1),
			new PresetItemWithCount("SkillBook", 29, 1),
			new PresetItemWithCount("SkillBook", 30, 1),
			new PresetItemWithCount("SkillBook", 31, 1),
			new PresetItemWithCount("SkillBook", 74, 1),
			new PresetItemWithCount("SkillBook", 75, 1),
			new PresetItemWithCount("SkillBook", 76, 1)
		}, new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(492, 1761, EMapPickupsType.Event, EMapPickupsType2.Invalid, "map_eventicon_18", 1762, 0, new byte[15]
		{
			0, 0, 0, 1, 0, 0, 0, 0, 0, 0,
			0, 0, 0, 0, 0
		}, new List<short> { 57, 58, 59, 69, 70, 71, 72, 73, 74 }, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[0], default(PresetItemTemplateId), new sbyte[1], new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), -1, -1, -1, 1763, new int[2] { 1764, 1765 }, new int[2] { 1766, 1767 }, new int[2] { 689, 689 }, new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>
		{
			new PresetItemWithCount("Material", 141, 1),
			new PresetItemWithCount("Material", 145, 1),
			new PresetItemWithCount("Material", 149, 1),
			new PresetItemWithCount("Material", 153, 1),
			new PresetItemWithCount("Material", 177, 1),
			new PresetItemWithCount("Material", 181, 1),
			new PresetItemWithCount("Material", 185, 1),
			new PresetItemWithCount("Material", 189, 1),
			new PresetItemWithCount("Material", 209, 1),
			new PresetItemWithCount("Material", 213, 1),
			new PresetItemWithCount("Material", 221, 1),
			new PresetItemWithCount("Material", 233, 1),
			new PresetItemWithCount("Material", 238, 1),
			new PresetItemWithCount("Material", 259, 1),
			new PresetItemWithCount("Material", 266, 1)
		}, new List<PresetItemWithCount>
		{
			new PresetItemWithCount("Material", 157, 1),
			new PresetItemWithCount("Material", 161, 1),
			new PresetItemWithCount("Material", 165, 1),
			new PresetItemWithCount("Material", 169, 1),
			new PresetItemWithCount("Material", 173, 1),
			new PresetItemWithCount("Material", 193, 1),
			new PresetItemWithCount("Material", 197, 1),
			new PresetItemWithCount("Material", 201, 1),
			new PresetItemWithCount("Material", 205, 1),
			new PresetItemWithCount("Material", 217, 1),
			new PresetItemWithCount("Material", 221, 1),
			new PresetItemWithCount("Material", 225, 1),
			new PresetItemWithCount("Material", 229, 1),
			new PresetItemWithCount("Material", 245, 1),
			new PresetItemWithCount("Material", 252, 1),
			new PresetItemWithCount("Material", 273, 1)
		}));
		_dataArray.Add(new MapPickupsItem(493, 1768, EMapPickupsType.Event, EMapPickupsType2.Invalid, "map_eventicon_18", 1769, 0, new byte[15]
		{
			0, 0, 0, 1, 0, 0, 0, 0, 0, 0,
			0, 0, 0, 0, 0
		}, new List<short>
		{
			78, 79, 80, 87, 88, 89, 90, 91, 92, 93,
			100, 101, 102
		}, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[0], default(PresetItemTemplateId), new sbyte[1], new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), -1, -1, -1, 1770, new int[2] { 1771, 1772 }, new int[4] { 1770, 1773, 1774, 1775 }, new int[2] { 689, 689 }, new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>
		{
			new PresetItemWithCount("Material", 81, 1)
		}, new List<PresetItemWithCount>
		{
			new PresetItemWithCount("Food", 4, 1)
		}));
		_dataArray.Add(new MapPickupsItem(494, 1776, EMapPickupsType.Event, EMapPickupsType2.Invalid, "map_eventicon_18", 1777, 0, new byte[15]
		{
			0, 0, 0, 1, 0, 0, 0, 0, 0, 0,
			0, 0, 0, 0, 0
		}, new List<short> { 57, 58, 59, 60, 61, 62, 72, 73, 74 }, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[0], default(PresetItemTemplateId), new sbyte[1], new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), -1, -1, -1, 1778, new int[2] { 1779, 1780 }, new int[2] { 1781, 1782 }, new int[2] { 689, 711 }, new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int> { -1, 5000 }, new List<PresetItemWithCount>
		{
			new PresetItemWithCount("SkillBook", 20, 1),
			new PresetItemWithCount("SkillBook", 21, 1),
			new PresetItemWithCount("SkillBook", 22, 1),
			new PresetItemWithCount("SkillBook", 38, 1),
			new PresetItemWithCount("SkillBook", 39, 1),
			new PresetItemWithCount("SkillBook", 40, 1),
			new PresetItemWithCount("SkillBook", 110, 1),
			new PresetItemWithCount("SkillBook", 111, 1),
			new PresetItemWithCount("SkillBook", 112, 1)
		}, new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(495, 1761, EMapPickupsType.Event, EMapPickupsType2.Invalid, "map_eventicon_18", 1783, 0, new byte[15]
		{
			0, 0, 0, 0, 1, 0, 0, 0, 0, 0,
			0, 0, 0, 0, 0
		}, new List<short>
		{
			63, 64, 65, 90, 91, 92, 106, 107, 108, 87,
			88, 89
		}, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[0], default(PresetItemTemplateId), new sbyte[1], new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), -1, -1, -1, 1784, new int[2] { 1785, 1786 }, new int[2] { 1787, 1788 }, new int[2] { 689, 689 }, new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>
		{
			new PresetItemWithCount("Material", 17, 1)
		}, new List<PresetItemWithCount>
		{
			new PresetItemWithCount("Material", 24, 1)
		}));
		_dataArray.Add(new MapPickupsItem(496, 1768, EMapPickupsType.Event, EMapPickupsType2.Invalid, "map_eventicon_18", 1789, 0, new byte[15]
		{
			0, 0, 0, 0, 1, 0, 0, 0, 0, 0,
			0, 0, 0, 0, 0
		}, new List<short>
		{
			39, 40, 41, 42, 43, 44, 45, 46, 47, 54,
			55, 56, 48, 49, 50, 51, 52, 53
		}, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[0], default(PresetItemTemplateId), new sbyte[1], new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), -1, -1, -1, 1790, new int[2] { 1791, 1792 }, new int[2] { 1793, 1794 }, new int[2] { 689, 689 }, new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>
		{
			new PresetItemWithCount("Material", 60, 1)
		}, new List<PresetItemWithCount>
		{
			new PresetItemWithCount("Material", 74, 1)
		}));
		_dataArray.Add(new MapPickupsItem(497, 1776, EMapPickupsType.Event, EMapPickupsType2.Invalid, "map_eventicon_18", 1795, 0, new byte[15]
		{
			0, 0, 0, 0, 1, 0, 0, 0, 0, 0,
			0, 0, 0, 0, 0
		}, new List<short>
		{
			57, 58, 59, 69, 70, 71, 63, 64, 65, 66,
			67, 68
		}, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[0], default(PresetItemTemplateId), new sbyte[1], new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), -1, -1, -1, 1796, new int[2] { 1797, 1798 }, new int[2] { 1799, 1800 }, new int[2] { 689, 711 }, new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int> { -1, 5000 }, new List<PresetItemWithCount>
		{
			new PresetItemWithCount("SkillBook", 119, 1),
			new PresetItemWithCount("SkillBook", 120, 1),
			new PresetItemWithCount("SkillBook", 121, 1),
			new PresetItemWithCount("SkillBook", 110, 1),
			new PresetItemWithCount("SkillBook", 111, 1),
			new PresetItemWithCount("SkillBook", 112, 1),
			new PresetItemWithCount("SkillBook", 128, 1),
			new PresetItemWithCount("SkillBook", 129, 1),
			new PresetItemWithCount("SkillBook", 130, 1)
		}, new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(498, 1801, EMapPickupsType.Event, EMapPickupsType2.Invalid, "map_eventicon_18", 1802, 0, new byte[15]
		{
			0, 0, 0, 0, 0, 1, 0, 0, 0, 0,
			0, 0, 0, 0, 0
		}, new List<short>
		{
			54, 55, 56, 87, 88, 89, 90, 91, 92, 106,
			107, 108
		}, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[0], default(PresetItemTemplateId), new sbyte[1], new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), -1, -1, -1, 1803, new int[2] { 1804, 1805 }, new int[2] { 1806, 1807 }, new int[2] { 689, 689 }, new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>
		{
			new PresetItemWithCount("Material", 17, 1)
		}, new List<PresetItemWithCount>
		{
			new PresetItemWithCount("Material", 24, 1)
		}));
		_dataArray.Add(new MapPickupsItem(499, 1808, EMapPickupsType.Event, EMapPickupsType2.Invalid, "map_eventicon_18", 1809, 0, new byte[15]
		{
			0, 0, 0, 0, 0, 1, 0, 0, 0, 0,
			0, 0, 0, 0, 0
		}, new List<short>
		{
			78, 79, 80, 87, 88, 89, 90, 91, 92, 57,
			58, 59, 69, 70, 71
		}, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[0], default(PresetItemTemplateId), new sbyte[1], new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), -1, -1, -1, 1810, new int[2] { 1811, 1812 }, new int[2] { 1813, 1814 }, new int[2] { 689, 689 }, new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>
		{
			new PresetItemWithCount("Material", 67, 1)
		}, new List<PresetItemWithCount>
		{
			new PresetItemWithCount("Food", 4, 1)
		}));
		_dataArray.Add(new MapPickupsItem(500, 1815, EMapPickupsType.Event, EMapPickupsType2.Invalid, "map_eventicon_18", 1816, 0, new byte[15]
		{
			0, 0, 0, 0, 0, 1, 0, 0, 0, 0,
			0, 0, 0, 0, 0
		}, new List<short>
		{
			39, 40, 41, 42, 43, 44, 45, 46, 47, 54,
			55, 56, 48, 49, 50, 51, 52, 53, 1, 2,
			3, 4, 5, 6, 7, 8, 9, 10, 11, 12,
			13, 14, 15, 34, 35, 36, 37
		}, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[0], default(PresetItemTemplateId), new sbyte[1], new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), -1, -1, -1, 1817, new int[2] { 1818, 1819 }, new int[2] { 1820, 1821 }, new int[2] { 689, 711 }, new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int> { -1, 5000 }, new List<PresetItemWithCount>
		{
			new PresetItemWithCount("SkillBook", 56, 1),
			new PresetItemWithCount("SkillBook", 57, 1),
			new PresetItemWithCount("SkillBook", 58, 1),
			new PresetItemWithCount("SkillBook", 65, 1),
			new PresetItemWithCount("SkillBook", 66, 1),
			new PresetItemWithCount("SkillBook", 67, 1),
			new PresetItemWithCount("SkillBook", 137, 1),
			new PresetItemWithCount("SkillBook", 138, 1),
			new PresetItemWithCount("SkillBook", 139, 1)
		}, new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(501, 1822, EMapPickupsType.Event, EMapPickupsType2.Invalid, "map_eventicon_18", 1823, 0, new byte[15]
		{
			0, 0, 0, 0, 0, 0, 1, 0, 0, 0,
			0, 0, 0, 0, 0
		}, new List<short>
		{
			39, 40, 41, 42, 43, 44, 45, 46, 47, 54,
			55, 56, 48, 49, 50, 51, 52, 53, 1, 2,
			3, 4, 5, 6, 7, 8, 9, 10, 11, 12,
			13, 14, 15, 34, 35, 36, 37
		}, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[0], default(PresetItemTemplateId), new sbyte[1], new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), -1, -1, -1, 1824, new int[2] { 1825, 1826 }, new int[2] { 1827, 1828 }, new int[2] { 689, 689 }, new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>
		{
			new PresetItemWithCount("Material", 38, 1)
		}, new List<PresetItemWithCount>
		{
			new PresetItemWithCount("Material", 31, 1)
		}));
		_dataArray.Add(new MapPickupsItem(502, 1829, EMapPickupsType.Event, EMapPickupsType2.Invalid, "map_eventicon_18", 1830, 0, new byte[15]
		{
			0, 0, 0, 0, 0, 0, 1, 0, 0, 0,
			0, 0, 0, 0, 0
		}, new List<short>
		{
			39, 40, 41, 42, 43, 44, 45, 46, 47, 54,
			55, 56, 48, 49, 50, 51, 52, 53, 1, 2,
			3, 4, 5, 6, 7, 8, 9, 10, 11, 12,
			13, 14, 15, 34, 35, 36, 37
		}, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[0], default(PresetItemTemplateId), new sbyte[1], new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), -1, -1, -1, 1831, new int[2] { 1832, 1833 }, new int[2] { 1834, 1835 }, new int[2] { 689, 689 }, new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>
		{
			new PresetItemWithCount("Material", 81, 1)
		}, new List<PresetItemWithCount>
		{
			new PresetItemWithCount("Food", 4, 1)
		}));
		_dataArray.Add(new MapPickupsItem(503, 1836, EMapPickupsType.Event, EMapPickupsType2.Invalid, "map_eventicon_18", 1837, 0, new byte[15]
		{
			0, 0, 0, 0, 0, 0, 1, 0, 0, 0,
			0, 0, 0, 0, 0
		}, new List<short> { 57, 58, 59, 69, 70, 71, 72, 73, 74 }, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[0], default(PresetItemTemplateId), new sbyte[1], new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), -1, -1, -1, 1838, new int[2] { 1839, 1840 }, new int[2] { 1841, 1842 }, new int[2] { 689, 711 }, new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int> { -1, 5000 }, new List<PresetItemWithCount>
		{
			new PresetItemWithCount("SkillBook", 20, 1),
			new PresetItemWithCount("SkillBook", 21, 1),
			new PresetItemWithCount("SkillBook", 22, 1),
			new PresetItemWithCount("SkillBook", 38, 1),
			new PresetItemWithCount("SkillBook", 39, 1),
			new PresetItemWithCount("SkillBook", 40, 1),
			new PresetItemWithCount("SkillBook", 110, 1),
			new PresetItemWithCount("SkillBook", 111, 1),
			new PresetItemWithCount("SkillBook", 112, 1)
		}, new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(504, 1843, EMapPickupsType.Event, EMapPickupsType2.Invalid, "map_eventicon_18", 1844, 0, new byte[15]
		{
			0, 0, 0, 0, 0, 0, 0, 1, 0, 0,
			0, 0, 0, 0, 0
		}, new List<short> { 97, 98, 99 }, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[0], default(PresetItemTemplateId), new sbyte[1], new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), -1, -1, -1, 1845, new int[2] { 1846, 1847 }, new int[2] { 1848, 1849 }, new int[2] { 689, 689 }, new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>
		{
			new PresetItemWithCount("Material", 38, 1)
		}, new List<PresetItemWithCount>
		{
			new PresetItemWithCount("Material", 31, 1)
		}));
		_dataArray.Add(new MapPickupsItem(505, 1850, EMapPickupsType.Event, EMapPickupsType2.Invalid, "map_eventicon_18", 1851, 0, new byte[15]
		{
			0, 0, 0, 0, 0, 0, 0, 1, 0, 0,
			0, 0, 0, 0, 0
		}, new List<short>
		{
			39, 40, 41, 42, 43, 44, 45, 46, 47, 54,
			55, 56, 48, 49, 50, 51, 52, 53
		}, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[0], default(PresetItemTemplateId), new sbyte[1], new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), -1, -1, -1, 1852, new int[2] { 1853, 1384 }, new int[2] { 1854, 1855 }, new int[2] { 689, 689 }, new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>
		{
			new PresetItemWithCount("Material", 60, 1)
		}, new List<PresetItemWithCount>
		{
			new PresetItemWithCount("Material", 74, 1)
		}));
		_dataArray.Add(new MapPickupsItem(506, 1856, EMapPickupsType.Event, EMapPickupsType2.Invalid, "map_eventicon_18", 1857, 0, new byte[15]
		{
			0, 0, 0, 0, 0, 0, 0, 1, 0, 0,
			0, 0, 0, 0, 0
		}, new List<short>
		{
			75, 76, 77, 81, 82, 83, 84, 85, 86, 94,
			95, 96
		}, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[0], default(PresetItemTemplateId), new sbyte[1], new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), -1, -1, -1, 1858, new int[2] { 1859, 1860 }, new int[2] { 1861, 1862 }, new int[2] { 689, 711 }, new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int> { -1, 5000 }, new List<PresetItemWithCount>
		{
			new PresetItemWithCount("SkillBook", 2, 1),
			new PresetItemWithCount("SkillBook", 3, 1),
			new PresetItemWithCount("SkillBook", 4, 1),
			new PresetItemWithCount("SkillBook", 29, 1),
			new PresetItemWithCount("SkillBook", 30, 1),
			new PresetItemWithCount("SkillBook", 31, 1),
			new PresetItemWithCount("SkillBook", 47, 1),
			new PresetItemWithCount("SkillBook", 48, 1),
			new PresetItemWithCount("SkillBook", 49, 1)
		}, new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(507, 1863, EMapPickupsType.Event, EMapPickupsType2.Invalid, "map_eventicon_18", 1864, 0, new byte[15]
		{
			0, 0, 0, 0, 0, 0, 0, 0, 1, 0,
			0, 0, 0, 0, 0
		}, new List<short>
		{
			75, 76, 77, 81, 82, 83, 84, 85, 86, 94,
			95, 96
		}, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[0], default(PresetItemTemplateId), new sbyte[1], new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), -1, -1, -1, 1865, new int[2] { 1866, 1867 }, new int[2] { 1868, 1869 }, new int[2] { 689, 689 }, new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>
		{
			new PresetItemWithCount("Material", 17, 1)
		}, new List<PresetItemWithCount>
		{
			new PresetItemWithCount("Material", 24, 1)
		}));
		_dataArray.Add(new MapPickupsItem(508, 1870, EMapPickupsType.Event, EMapPickupsType2.Invalid, "map_eventicon_18", 1871, 0, new byte[15]
		{
			0, 0, 0, 0, 0, 0, 0, 0, 1, 0,
			0, 0, 0, 0, 0
		}, new List<short> { 81, 82, 83, 84, 85, 86, 94, 95, 96 }, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[0], default(PresetItemTemplateId), new sbyte[1], new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), -1, -1, -1, 1872, new int[2] { 1873, 1874 }, new int[2] { 1875, 1876 }, new int[2] { 689, 689 }, new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>
		{
			new PresetItemWithCount("Material", 74, 1)
		}, new List<PresetItemWithCount>
		{
			new PresetItemWithCount("Material", 81, 1)
		}));
		_dataArray.Add(new MapPickupsItem(509, 1877, EMapPickupsType.Event, EMapPickupsType2.Invalid, "map_eventicon_18", 1878, 0, new byte[15]
		{
			0, 0, 0, 0, 0, 0, 0, 0, 1, 0,
			0, 0, 0, 0, 0
		}, new List<short>
		{
			39, 40, 41, 42, 43, 44, 45, 46, 47, 54,
			55, 56, 48, 49, 50, 51, 52, 53, 1, 2,
			3, 4, 5, 6, 7, 8, 9, 10, 11, 12,
			13, 14, 15, 34, 35, 36, 37
		}, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[0], default(PresetItemTemplateId), new sbyte[1], new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), -1, -1, -1, 1879, new int[2] { 1202, 1562 }, new int[2] { 1880, 1881 }, new int[2] { 689, 711 }, new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int> { -1, 5000 }, new List<PresetItemWithCount>
		{
			new PresetItemWithCount("SkillBook", 56, 1),
			new PresetItemWithCount("SkillBook", 57, 1),
			new PresetItemWithCount("SkillBook", 58, 1),
			new PresetItemWithCount("SkillBook", 65, 1),
			new PresetItemWithCount("SkillBook", 66, 1),
			new PresetItemWithCount("SkillBook", 67, 1),
			new PresetItemWithCount("SkillBook", 92, 1),
			new PresetItemWithCount("SkillBook", 93, 1),
			new PresetItemWithCount("SkillBook", 94, 1),
			new PresetItemWithCount("SkillBook", 101, 1),
			new PresetItemWithCount("SkillBook", 102, 1),
			new PresetItemWithCount("SkillBook", 103, 1)
		}, new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(510, 1882, EMapPickupsType.Event, EMapPickupsType2.Invalid, "map_eventicon_18", 1883, 0, new byte[15]
		{
			0, 0, 0, 0, 0, 0, 0, 0, 0, 1,
			0, 0, 0, 0, 0
		}, new List<short> { 81, 82, 83, 84, 85, 86, 94, 95, 96 }, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[0], default(PresetItemTemplateId), new sbyte[1], new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), -1, -1, -1, 1884, new int[2] { 1885, 1886 }, new int[2] { 1887, 1888 }, new int[2] { 689, 689 }, new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>
		{
			new PresetItemWithCount("Material", 141, 1),
			new PresetItemWithCount("Material", 145, 1),
			new PresetItemWithCount("Material", 149, 1),
			new PresetItemWithCount("Material", 153, 1),
			new PresetItemWithCount("Material", 177, 1),
			new PresetItemWithCount("Material", 181, 1),
			new PresetItemWithCount("Material", 185, 1),
			new PresetItemWithCount("Material", 189, 1),
			new PresetItemWithCount("Material", 209, 1),
			new PresetItemWithCount("Material", 213, 1),
			new PresetItemWithCount("Material", 221, 1),
			new PresetItemWithCount("Material", 233, 1),
			new PresetItemWithCount("Material", 238, 1),
			new PresetItemWithCount("Material", 259, 1),
			new PresetItemWithCount("Material", 266, 1)
		}, new List<PresetItemWithCount>
		{
			new PresetItemWithCount("Material", 157, 1),
			new PresetItemWithCount("Material", 161, 1),
			new PresetItemWithCount("Material", 165, 1),
			new PresetItemWithCount("Material", 169, 1),
			new PresetItemWithCount("Material", 173, 1),
			new PresetItemWithCount("Material", 193, 1),
			new PresetItemWithCount("Material", 197, 1),
			new PresetItemWithCount("Material", 201, 1),
			new PresetItemWithCount("Material", 205, 1),
			new PresetItemWithCount("Material", 217, 1),
			new PresetItemWithCount("Material", 221, 1),
			new PresetItemWithCount("Material", 225, 1),
			new PresetItemWithCount("Material", 229, 1),
			new PresetItemWithCount("Material", 245, 1),
			new PresetItemWithCount("Material", 252, 1),
			new PresetItemWithCount("Material", 273, 1)
		}));
		_dataArray.Add(new MapPickupsItem(511, 1889, EMapPickupsType.Event, EMapPickupsType2.Invalid, "map_eventicon_18", 1890, 0, new byte[15]
		{
			0, 0, 0, 0, 0, 0, 0, 0, 0, 1,
			0, 0, 0, 0, 0
		}, new List<short> { 81, 82, 83, 84, 85, 86, 94, 95, 96 }, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[0], default(PresetItemTemplateId), new sbyte[1], new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), -1, -1, -1, 1891, new int[2] { 1892, 1893 }, new int[2] { 1894, 1895 }, new int[2] { 689, 689 }, new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>
		{
			new PresetItemWithCount("Material", 67, 1)
		}, new List<PresetItemWithCount>
		{
			new PresetItemWithCount("Material", 74, 1)
		}));
		_dataArray.Add(new MapPickupsItem(512, 1896, EMapPickupsType.Event, EMapPickupsType2.Invalid, "map_eventicon_18", 1897, 0, new byte[15]
		{
			0, 0, 0, 0, 0, 0, 0, 0, 0, 1,
			0, 0, 0, 0, 0
		}, new List<short>
		{
			57, 58, 59, 69, 70, 71, 72, 73, 74, 106,
			107, 108
		}, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[0], default(PresetItemTemplateId), new sbyte[1], new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), -1, -1, -1, 1898, new int[2] { 1899, 1900 }, new int[2] { 1901, 1902 }, new int[2] { 689, 711 }, new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int> { -1, 5000 }, new List<PresetItemWithCount>
		{
			new PresetItemWithCount("SkillBook", 74, 1),
			new PresetItemWithCount("SkillBook", 75, 1),
			new PresetItemWithCount("SkillBook", 76, 1),
			new PresetItemWithCount("SkillBook", 83, 1),
			new PresetItemWithCount("SkillBook", 84, 1),
			new PresetItemWithCount("SkillBook", 85, 1),
			new PresetItemWithCount("SkillBook", 92, 1),
			new PresetItemWithCount("SkillBook", 93, 1),
			new PresetItemWithCount("SkillBook", 94, 1)
		}, new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(513, 1903, EMapPickupsType.Event, EMapPickupsType2.Invalid, "map_eventicon_18", 1904, 0, new byte[15]
		{
			0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
			1, 0, 0, 0, 0
		}, new List<short>
		{
			39, 40, 41, 42, 43, 44, 45, 46, 47, 54,
			55, 56, 48, 49, 50, 51, 52, 53, 1, 2,
			3, 4, 5, 6, 7, 8, 9, 10, 11, 12,
			13, 14, 15, 34, 35, 36, 37
		}, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[0], default(PresetItemTemplateId), new sbyte[1], new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), -1, -1, -1, 1905, new int[2] { 1906, 1907 }, new int[2] { 1908, 1909 }, new int[2] { 689, 689 }, new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>
		{
			new PresetItemWithCount("Material", 38, 1)
		}, new List<PresetItemWithCount>
		{
			new PresetItemWithCount("Material", 31, 1)
		}));
		_dataArray.Add(new MapPickupsItem(514, 1910, EMapPickupsType.Event, EMapPickupsType2.Invalid, "map_eventicon_18", 1911, 0, new byte[15]
		{
			0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
			1, 0, 0, 0, 0
		}, new List<short> { 103, 104, 105, 75, 76, 77, 78, 79, 80, 93 }, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[0], default(PresetItemTemplateId), new sbyte[1], new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), -1, -1, -1, 1912, new int[2] { 1913, 1914 }, new int[2] { 1915, 1916 }, new int[2] { 689, 689 }, new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>
		{
			new PresetItemWithCount("Material", 67, 1)
		}, new List<PresetItemWithCount>
		{
			new PresetItemWithCount("Food", 4, 1)
		}));
		_dataArray.Add(new MapPickupsItem(515, 1917, EMapPickupsType.Event, EMapPickupsType2.Invalid, "map_eventicon_18", 1918, 0, new byte[15]
		{
			0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
			1, 0, 0, 0, 0
		}, new List<short>
		{
			39, 40, 41, 42, 43, 44, 45, 46, 47, 54,
			55, 56, 48, 49, 50, 51, 52, 53, 1, 2,
			3, 4, 5, 6, 7, 8, 9, 10, 11, 12,
			13, 14, 15, 34, 35, 36, 37
		}, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[0], default(PresetItemTemplateId), new sbyte[1], new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), -1, -1, -1, 1919, new int[2] { 1920, 1921 }, new int[2] { 1922, 1923 }, new int[2] { 689, 711 }, new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int> { -1, 5000 }, new List<PresetItemWithCount>
		{
			new PresetItemWithCount("SkillBook", 119, 1),
			new PresetItemWithCount("SkillBook", 120, 1),
			new PresetItemWithCount("SkillBook", 121, 1),
			new PresetItemWithCount("SkillBook", 101, 1),
			new PresetItemWithCount("SkillBook", 102, 1),
			new PresetItemWithCount("SkillBook", 103, 1),
			new PresetItemWithCount("SkillBook", 137, 1),
			new PresetItemWithCount("SkillBook", 138, 1),
			new PresetItemWithCount("SkillBook", 139, 1)
		}, new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(516, 1924, EMapPickupsType.Event, EMapPickupsType2.Invalid, "map_eventicon_18", 1925, 0, new byte[15]
		{
			0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
			0, 1, 0, 0, 0
		}, new List<short>
		{
			103, 104, 105, 75, 76, 77, 78, 79, 80, 39,
			40, 41, 48, 49, 50, 51, 52, 53
		}, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[0], default(PresetItemTemplateId), new sbyte[1], new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), -1, -1, -1, 1926, new int[2] { 1927, 1928 }, new int[2] { 1929, 1930 }, new int[2] { 689, 689 }, new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>
		{
			new PresetItemWithCount("Material", 3, 1)
		}, new List<PresetItemWithCount>
		{
			new PresetItemWithCount("Material", 10, 1)
		}));
		_dataArray.Add(new MapPickupsItem(517, 1931, EMapPickupsType.Event, EMapPickupsType2.Invalid, "map_eventicon_18", 1932, 0, new byte[15]
		{
			0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
			0, 1, 0, 0, 0
		}, new List<short> { 57, 58, 59, 69, 70, 71, 72, 73, 74 }, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[0], default(PresetItemTemplateId), new sbyte[1], new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), -1, -1, -1, 1933, new int[2] { 1934, 1935 }, new int[2] { 1936, 1937 }, new int[2] { 689, 689 }, new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>
		{
			new PresetItemWithCount("Material", 60, 1)
		}, new List<PresetItemWithCount>
		{
			new PresetItemWithCount("Food", 4, 1)
		}));
		_dataArray.Add(new MapPickupsItem(518, 1938, EMapPickupsType.Event, EMapPickupsType2.Invalid, "map_eventicon_18", 1939, 0, new byte[15]
		{
			0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
			0, 1, 0, 0, 0
		}, new List<short>
		{
			57, 58, 59, 60, 61, 62, 69, 70, 71, 72,
			73, 74, 81, 82, 83, 84, 85, 86, 106, 107,
			108
		}, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[0], default(PresetItemTemplateId), new sbyte[1], new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), -1, -1, -1, 1940, new int[2] { 1941, 1942 }, new int[2] { 1943, 1944 }, new int[2] { 689, 711 }, new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int> { -1, 5000 }, new List<PresetItemWithCount>
		{
			new PresetItemWithCount("SkillBook", 74, 1),
			new PresetItemWithCount("SkillBook", 75, 1),
			new PresetItemWithCount("SkillBook", 76, 1),
			new PresetItemWithCount("SkillBook", 83, 1),
			new PresetItemWithCount("SkillBook", 84, 1),
			new PresetItemWithCount("SkillBook", 85, 1),
			new PresetItemWithCount("SkillBook", 65, 1),
			new PresetItemWithCount("SkillBook", 66, 1),
			new PresetItemWithCount("SkillBook", 67, 1)
		}, new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(519, 1945, EMapPickupsType.Event, EMapPickupsType2.Invalid, "map_eventicon_18", 1946, 0, new byte[15]
		{
			0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
			0, 0, 1, 0, 0
		}, new List<short>
		{
			39, 40, 41, 42, 43, 44, 45, 46, 47, 54,
			55, 56, 48, 49, 50, 51, 52, 53, 1, 2,
			3, 4, 5, 6, 7, 8, 9, 10, 11, 12,
			13, 14, 15, 34, 35, 36, 37
		}, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[0], default(PresetItemTemplateId), new sbyte[1], new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), -1, -1, -1, 1947, new int[2] { 1948, 1949 }, new int[2] { 1950, 1951 }, new int[2] { 689, 689 }, new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>
		{
			new PresetItemWithCount("Material", 52, 1)
		}, new List<PresetItemWithCount>
		{
			new PresetItemWithCount("Material", 45, 1)
		}));
		_dataArray.Add(new MapPickupsItem(520, 1952, EMapPickupsType.Event, EMapPickupsType2.Invalid, "map_eventicon_18", 1953, 0, new byte[15]
		{
			0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
			0, 0, 1, 0, 0
		}, new List<short>
		{
			39, 40, 41, 42, 43, 44, 45, 46, 47, 54,
			55, 56, 48, 49, 50, 51, 52, 53, 1, 2,
			3, 4, 5, 6, 7, 8, 9, 10, 11, 12,
			13, 14, 15, 34, 35, 36, 37
		}, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[0], default(PresetItemTemplateId), new sbyte[1], new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), -1, -1, -1, 1954, new int[2] { 1955, 1956 }, new int[2] { 1957, 1958 }, new int[2] { 689, 689 }, new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>
		{
			new PresetItemWithCount("Material", 67, 1)
		}, new List<PresetItemWithCount>
		{
			new PresetItemWithCount("Material", 74, 1)
		}));
		_dataArray.Add(new MapPickupsItem(521, 1959, EMapPickupsType.Event, EMapPickupsType2.Invalid, "map_eventicon_18", 1960, 0, new byte[15]
		{
			0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
			0, 0, 1, 0, 0
		}, new List<short>
		{
			39, 40, 41, 42, 43, 44, 45, 46, 47, 54,
			55, 56, 48, 49, 50, 51, 52, 53, 1, 2,
			3, 4, 5, 6, 7, 8, 9, 10, 11, 12,
			13, 14, 15, 34, 35, 36, 37
		}, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[0], default(PresetItemTemplateId), new sbyte[1], new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), -1, -1, -1, 1961, new int[2] { 1962, 1963 }, new int[2] { 1964, 1965 }, new int[2] { 689, 711 }, new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int> { -1, 5000 }, new List<PresetItemWithCount>
		{
			new PresetItemWithCount("SkillBook", 11, 1),
			new PresetItemWithCount("SkillBook", 12, 1),
			new PresetItemWithCount("SkillBook", 13, 1),
			new PresetItemWithCount("SkillBook", 38, 1),
			new PresetItemWithCount("SkillBook", 39, 1),
			new PresetItemWithCount("SkillBook", 40, 1),
			new PresetItemWithCount("SkillBook", 83, 1),
			new PresetItemWithCount("SkillBook", 84, 1),
			new PresetItemWithCount("SkillBook", 85, 1)
		}, new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(522, 1966, EMapPickupsType.Event, EMapPickupsType2.Invalid, "map_eventicon_18", 1967, 0, new byte[15]
		{
			0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
			0, 0, 0, 1, 0
		}, new List<short>
		{
			78, 79, 80, 87, 88, 89, 90, 91, 92, 93,
			100, 101, 102
		}, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[0], default(PresetItemTemplateId), new sbyte[1], new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), -1, -1, -1, 1968, new int[2] { 1969, 1970 }, new int[2] { 1971, 1972 }, new int[2] { 689, 689 }, new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>
		{
			new PresetItemWithCount("Material", 3, 1)
		}, new List<PresetItemWithCount>
		{
			new PresetItemWithCount("Material", 10, 1)
		}));
		_dataArray.Add(new MapPickupsItem(523, 1973, EMapPickupsType.Event, EMapPickupsType2.Invalid, "map_eventicon_18", 1974, 0, new byte[15]
		{
			0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
			0, 0, 0, 1, 0
		}, new List<short>
		{
			39, 40, 41, 42, 43, 44, 45, 46, 47, 54,
			55, 56, 48, 49, 50, 51, 52, 53, 1, 2,
			3, 4, 5, 6, 7, 8, 9, 10, 11, 12,
			13, 14, 15, 34, 35, 36, 37
		}, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[0], default(PresetItemTemplateId), new sbyte[1], new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), -1, -1, -1, 1975, new int[2] { 1976, 1977 }, new int[2] { 1978, 1979 }, new int[2] { 689, 689 }, new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>
		{
			new PresetItemWithCount("Material", 74, 1)
		}, new List<PresetItemWithCount>
		{
			new PresetItemWithCount("Material", 81, 1)
		}));
		_dataArray.Add(new MapPickupsItem(524, 1980, EMapPickupsType.Event, EMapPickupsType2.Invalid, "map_eventicon_18", 1981, 0, new byte[15]
		{
			0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
			0, 0, 0, 1, 0
		}, new List<short> { 42, 43, 44, 45, 46, 47 }, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[0], default(PresetItemTemplateId), new sbyte[1], new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), -1, -1, -1, 1982, new int[2] { 1983, 1984 }, new int[2] { 1985, 1986 }, new int[2] { 689, 711 }, new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int> { -1, 5000 }, new List<PresetItemWithCount>
		{
			new PresetItemWithCount("SkillBook", 47, 1),
			new PresetItemWithCount("SkillBook", 48, 1),
			new PresetItemWithCount("SkillBook", 49, 1),
			new PresetItemWithCount("SkillBook", 56, 1),
			new PresetItemWithCount("SkillBook", 57, 1),
			new PresetItemWithCount("SkillBook", 58, 1),
			new PresetItemWithCount("SkillBook", 128, 1),
			new PresetItemWithCount("SkillBook", 129, 1),
			new PresetItemWithCount("SkillBook", 130, 1)
		}, new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(525, 1987, EMapPickupsType.Event, EMapPickupsType2.Invalid, "map_eventicon_18", 1988, 0, new byte[15]
		{
			0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
			0, 0, 0, 0, 1
		}, new List<short> { 57, 58, 59, 69, 70, 71, 72, 73, 74 }, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[0], default(PresetItemTemplateId), new sbyte[1], new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), -1, -1, -1, 1989, new int[2] { 1990, 1991 }, new int[2] { 1992, 1993 }, new int[2] { 689, 689 }, new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>
		{
			new PresetItemWithCount("Material", 239, 1),
			new PresetItemWithCount("Material", 260, 1),
			new PresetItemWithCount("Material", 267, 1)
		}, new List<PresetItemWithCount>
		{
			new PresetItemWithCount("Material", 246, 1),
			new PresetItemWithCount("Material", 253, 1),
			new PresetItemWithCount("Material", 274, 1)
		}));
		_dataArray.Add(new MapPickupsItem(526, 1994, EMapPickupsType.Event, EMapPickupsType2.Invalid, "map_eventicon_18", 1995, 0, new byte[15]
		{
			0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
			0, 0, 0, 0, 1
		}, new List<short>
		{
			103, 104, 105, 75, 76, 77, 78, 79, 80, 39,
			40, 41, 48, 49, 50, 51, 52, 53
		}, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[0], default(PresetItemTemplateId), new sbyte[1], new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), -1, -1, -1, 1996, new int[2] { 1997, 1998 }, new int[2] { 1999, 2000 }, new int[2] { 689, 689 }, new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>
		{
			new PresetItemWithCount("Material", 67, 1)
		}, new List<PresetItemWithCount>
		{
			new PresetItemWithCount("Material", 60, 1)
		}));
		_dataArray.Add(new MapPickupsItem(527, 2001, EMapPickupsType.Event, EMapPickupsType2.Invalid, "map_eventicon_18", 2002, 0, new byte[15]
		{
			0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
			0, 0, 0, 0, 1
		}, new List<short> { 57, 58, 59, 60, 61, 62, 97, 98, 99 }, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[0], default(PresetItemTemplateId), new sbyte[1], new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), -1, -1, -1, 2003, new int[1] { 2004 }, new int[1] { 2005 }, new int[2] { 689, 711 }, new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int> { -1, 5000 }, new List<PresetItemWithCount>
		{
			new PresetItemWithCount("SkillBook", 47, 1),
			new PresetItemWithCount("SkillBook", 48, 1),
			new PresetItemWithCount("SkillBook", 49, 1),
			new PresetItemWithCount("SkillBook", 137, 1),
			new PresetItemWithCount("SkillBook", 138, 1),
			new PresetItemWithCount("SkillBook", 139, 1),
			new PresetItemWithCount("SkillBook", 83, 1),
			new PresetItemWithCount("SkillBook", 84, 1),
			new PresetItemWithCount("SkillBook", 85, 1)
		}, new List<PresetItemWithCount>()));
		_dataArray.Add(new MapPickupsItem(528, 2006, EMapPickupsType.Event, EMapPickupsType2.Invalid, "map_eventicon_18", 2007, 0, new byte[15]
		{
			1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
			1, 1, 1, 1, 1
		}, new List<short>
		{
			39, 40, 41, 42, 43, 44, 45, 46, 47, 54,
			55, 56, 48, 49, 50, 51, 52, 53, 1, 2,
			3, 4, 5, 6, 7, 8, 9, 10, 11, 12,
			13, 14, 15, 34, 35, 36, 37
		}, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[0], default(PresetItemTemplateId), new sbyte[1], new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), -1, -1, -1, 2008, new int[2] { 2009, 2010 }, new int[2] { 2011, 2012 }, new int[2] { 689, 689 }, new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>
		{
			new PresetItemWithCount("Medicine", 69, 3),
			new PresetItemWithCount("Medicine", 72, 3),
			new PresetItemWithCount("Medicine", 57, 3),
			new PresetItemWithCount("Medicine", 60, 3)
		}, new List<PresetItemWithCount>
		{
			new PresetItemWithCount("Medicine", 337, 3),
			new PresetItemWithCount("Medicine", 340, 3),
			new PresetItemWithCount("Medicine", 121, 3),
			new PresetItemWithCount("Medicine", 124, 3)
		}));
		_dataArray.Add(new MapPickupsItem(529, 2013, EMapPickupsType.Event, EMapPickupsType2.Invalid, "map_eventicon_18", 2014, 0, new byte[15]
		{
			1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
			1, 1, 1, 1, 1
		}, new List<short>
		{
			39, 40, 41, 42, 43, 44, 45, 46, 47, 54,
			55, 56, 48, 49, 50, 51, 52, 53, 1, 2,
			3, 4, 5, 6, 7, 8, 9, 10, 11, 12,
			13, 14, 15, 34, 35, 36, 37
		}, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[0], default(PresetItemTemplateId), new sbyte[1], new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), -1, -1, -1, 2015, new int[2] { 2016, 2017 }, new int[2] { 2018, 2019 }, new int[2] { 689, 689 }, new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>
		{
			new PresetItemWithCount("Medicine", 3, 3),
			new PresetItemWithCount("Medicine", 12, 3),
			new PresetItemWithCount("Medicine", 21, 3),
			new PresetItemWithCount("Medicine", 30, 3),
			new PresetItemWithCount("Medicine", 39, 3),
			new PresetItemWithCount("Medicine", 48, 3),
			new PresetItemWithCount("Medicine", 133, 3),
			new PresetItemWithCount("Medicine", 136, 3),
			new PresetItemWithCount("Medicine", 145, 3),
			new PresetItemWithCount("Medicine", 148, 3),
			new PresetItemWithCount("Medicine", 157, 3),
			new PresetItemWithCount("Medicine", 160, 3),
			new PresetItemWithCount("Medicine", 169, 3),
			new PresetItemWithCount("Medicine", 172, 3),
			new PresetItemWithCount("Medicine", 181, 3),
			new PresetItemWithCount("Medicine", 184, 3),
			new PresetItemWithCount("Medicine", 193, 3),
			new PresetItemWithCount("Medicine", 196, 3)
		}, new List<PresetItemWithCount>
		{
			new PresetItemWithCount("Medicine", 205, 3),
			new PresetItemWithCount("Medicine", 208, 3),
			new PresetItemWithCount("Medicine", 217, 3),
			new PresetItemWithCount("Medicine", 220, 3)
		}));
		_dataArray.Add(new MapPickupsItem(530, 2020, EMapPickupsType.Event, EMapPickupsType2.Invalid, "map_eventicon_18", 2021, 0, new byte[15]
		{
			1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
			1, 1, 1, 1, 1
		}, new List<short>
		{
			39, 40, 41, 42, 43, 44, 45, 46, 47, 54,
			55, 56, 48, 49, 50, 51, 52, 53, 1, 2,
			3, 4, 5, 6, 7, 8, 9, 10, 11, 12,
			13, 14, 15, 34, 35, 36, 37
		}, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[0], default(PresetItemTemplateId), new sbyte[1], new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), -1, -1, -1, 2022, new int[2] { 2023, 2024 }, new int[2] { 2025, 2026 }, new int[2] { 689, 689 }, new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>
		{
			new PresetItemWithCount("Medicine", 97, 3),
			new PresetItemWithCount("Medicine", 100, 3),
			new PresetItemWithCount("Medicine", 85, 3),
			new PresetItemWithCount("Medicine", 88, 3)
		}, new List<PresetItemWithCount>
		{
			new PresetItemWithCount("Medicine", 109, 3),
			new PresetItemWithCount("Medicine", 112, 3),
			new PresetItemWithCount("Medicine", 313, 3),
			new PresetItemWithCount("Medicine", 316, 3)
		}));
		_dataArray.Add(new MapPickupsItem(531, 2027, EMapPickupsType.Event, EMapPickupsType2.Invalid, "map_eventicon_18", 2028, 0, new byte[15]
		{
			1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
			1, 1, 1, 1, 1
		}, new List<short>
		{
			39, 40, 41, 42, 43, 44, 45, 46, 47, 54,
			55, 56, 48, 49, 50, 51, 52, 53, 1, 2,
			3, 4, 5, 6, 7, 8, 9, 10, 11, 12,
			13, 14, 15, 34, 35, 36, 37
		}, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[0], default(PresetItemTemplateId), new sbyte[1], new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), -1, -1, -1, 2029, new int[2] { 2030, 2031 }, new int[2] { 2032, 2033 }, new int[2] { 689, 689 }, new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>
		{
			new PresetItemWithCount("Medicine", 277, 3),
			new PresetItemWithCount("Medicine", 280, 3),
			new PresetItemWithCount("Medicine", 301, 3),
			new PresetItemWithCount("Medicine", 304, 3),
			new PresetItemWithCount("Medicine", 325, 3),
			new PresetItemWithCount("Medicine", 328, 3),
			new PresetItemWithCount("Medicine", 229, 3),
			new PresetItemWithCount("Medicine", 232, 3),
			new PresetItemWithCount("Medicine", 241, 3),
			new PresetItemWithCount("Medicine", 244, 3),
			new PresetItemWithCount("Medicine", 253, 3),
			new PresetItemWithCount("Medicine", 256, 3)
		}, new List<PresetItemWithCount>
		{
			new PresetItemWithCount("Medicine", 265, 3),
			new PresetItemWithCount("Medicine", 268, 3),
			new PresetItemWithCount("Medicine", 289, 3),
			new PresetItemWithCount("Medicine", 292, 3)
		}));
		_dataArray.Add(new MapPickupsItem(532, 2034, EMapPickupsType.Event, EMapPickupsType2.Invalid, "map_eventicon_18", 2035, 0, new byte[15]
		{
			1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
			1, 1, 1, 1, 1
		}, new List<short>
		{
			39, 40, 41, 42, 43, 44, 45, 46, 47, 54,
			55, 56, 48, 49, 50, 51, 52, 53, 1, 2,
			3, 4, 5, 6, 7, 8, 9, 10, 11, 12,
			13, 14, 15, 34, 35, 36, 37
		}, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[0], default(PresetItemTemplateId), new sbyte[1], new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), -1, -1, -1, 2036, new int[2] { 2037, 2038 }, new int[2] { 2039, 2040 }, new int[2] { 689, 689 }, new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>
		{
			new PresetItemWithCount("Medicine", 123, 3),
			new PresetItemWithCount("Medicine", 126, 3),
			new PresetItemWithCount("Medicine", 339, 3),
			new PresetItemWithCount("Medicine", 342, 3)
		}, new List<PresetItemWithCount>
		{
			new PresetItemWithCount("Medicine", 71, 3),
			new PresetItemWithCount("Medicine", 74, 3),
			new PresetItemWithCount("Medicine", 59, 3),
			new PresetItemWithCount("Medicine", 62, 3)
		}));
		_dataArray.Add(new MapPickupsItem(533, 2041, EMapPickupsType.Event, EMapPickupsType2.Invalid, "map_eventicon_18", 2042, 0, new byte[15]
		{
			1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
			1, 1, 1, 1, 1
		}, new List<short>
		{
			39, 40, 41, 42, 43, 44, 45, 46, 47, 54,
			55, 56, 48, 49, 50, 51, 52, 53, 1, 2,
			3, 4, 5, 6, 7, 8, 9, 10, 11, 12,
			13, 14, 15, 34, 35, 36, 37
		}, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[0], default(PresetItemTemplateId), new sbyte[1], new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), -1, -1, -1, 2043, new int[2] { 2044, 2045 }, new int[2] { 2046, 2047 }, new int[2] { 689, 689 }, new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>
		{
			new PresetItemWithCount("Medicine", 219, 3),
			new PresetItemWithCount("Medicine", 222, 3),
			new PresetItemWithCount("Medicine", 207, 3),
			new PresetItemWithCount("Medicine", 210, 3)
		}, new List<PresetItemWithCount>
		{
			new PresetItemWithCount("Medicine", 5, 3),
			new PresetItemWithCount("Medicine", 14, 3),
			new PresetItemWithCount("Medicine", 23, 3),
			new PresetItemWithCount("Medicine", 32, 3),
			new PresetItemWithCount("Medicine", 41, 3),
			new PresetItemWithCount("Medicine", 50, 3),
			new PresetItemWithCount("Medicine", 135, 3),
			new PresetItemWithCount("Medicine", 138, 3),
			new PresetItemWithCount("Medicine", 147, 3),
			new PresetItemWithCount("Medicine", 150, 3),
			new PresetItemWithCount("Medicine", 159, 3),
			new PresetItemWithCount("Medicine", 162, 3),
			new PresetItemWithCount("Medicine", 171, 3),
			new PresetItemWithCount("Medicine", 174, 3),
			new PresetItemWithCount("Medicine", 183, 3),
			new PresetItemWithCount("Medicine", 186, 3),
			new PresetItemWithCount("Medicine", 195, 3),
			new PresetItemWithCount("Medicine", 198, 3)
		}));
		_dataArray.Add(new MapPickupsItem(534, 2048, EMapPickupsType.Event, EMapPickupsType2.Invalid, "map_eventicon_18", 2049, 0, new byte[15]
		{
			1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
			1, 1, 1, 1, 1
		}, new List<short>
		{
			39, 40, 41, 42, 43, 44, 45, 46, 47, 54,
			55, 56, 48, 49, 50, 51, 52, 53, 1, 2,
			3, 4, 5, 6, 7, 8, 9, 10, 11, 12,
			13, 14, 15, 34, 35, 36, 37
		}, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[0], default(PresetItemTemplateId), new sbyte[1], new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), -1, -1, -1, 2050, new int[2] { 2051, 2052 }, new int[2] { 2053, 2054 }, new int[2] { 689, 689 }, new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>
		{
			new PresetItemWithCount("Medicine", 111, 3),
			new PresetItemWithCount("Medicine", 114, 3),
			new PresetItemWithCount("Medicine", 315, 3),
			new PresetItemWithCount("Medicine", 318, 3)
		}, new List<PresetItemWithCount>
		{
			new PresetItemWithCount("Medicine", 99, 3),
			new PresetItemWithCount("Medicine", 102, 3),
			new PresetItemWithCount("Medicine", 87, 3),
			new PresetItemWithCount("Medicine", 90, 3)
		}));
		_dataArray.Add(new MapPickupsItem(535, 2055, EMapPickupsType.Event, EMapPickupsType2.Invalid, "map_eventicon_18", 2056, 0, new byte[15]
		{
			1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
			1, 1, 1, 1, 1
		}, new List<short>
		{
			39, 40, 41, 42, 43, 44, 45, 46, 47, 54,
			55, 56, 48, 49, 50, 51, 52, 53, 1, 2,
			3, 4, 5, 6, 7, 8, 9, 10, 11, 12,
			13, 14, 15, 34, 35, 36, 37
		}, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[0], default(PresetItemTemplateId), new sbyte[1], new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), -1, -1, -1, 2057, new int[2] { 2058, 2059 }, new int[2] { 2060, 2061 }, new int[2] { 689, 689 }, new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>
		{
			new PresetItemWithCount("Medicine", 267, 3),
			new PresetItemWithCount("Medicine", 270, 3),
			new PresetItemWithCount("Medicine", 291, 3),
			new PresetItemWithCount("Medicine", 294, 3)
		}, new List<PresetItemWithCount>
		{
			new PresetItemWithCount("Medicine", 279, 3),
			new PresetItemWithCount("Medicine", 282, 3),
			new PresetItemWithCount("Medicine", 303, 3),
			new PresetItemWithCount("Medicine", 306, 3),
			new PresetItemWithCount("Medicine", 327, 3),
			new PresetItemWithCount("Medicine", 330, 3),
			new PresetItemWithCount("Medicine", 231, 3),
			new PresetItemWithCount("Medicine", 234, 3),
			new PresetItemWithCount("Medicine", 243, 3),
			new PresetItemWithCount("Medicine", 246, 3),
			new PresetItemWithCount("Medicine", 255, 3),
			new PresetItemWithCount("Medicine", 258, 3)
		}));
		_dataArray.Add(new MapPickupsItem(536, 2062, EMapPickupsType.Event, EMapPickupsType2.Invalid, "map_eventicon_18", 2063, 0, new byte[15]
		{
			1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
			1, 1, 1, 1, 1
		}, new List<short>
		{
			39, 40, 41, 42, 43, 44, 45, 46, 47, 54,
			55, 56, 48, 49, 50, 51, 52, 53, 1, 2,
			3, 4, 5, 6, 7, 8, 9, 10, 11, 12,
			13, 14, 15, 34, 35, 36, 37
		}, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[0], default(PresetItemTemplateId), new sbyte[1], new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), -1, -1, -1, 2064, new int[2] { 2065, 2066 }, new int[2] { 2067, 2068 }, new int[2] { 689, 689 }, new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>
		{
			new PresetItemWithCount("Medicine", 284, 3),
			new PresetItemWithCount("Medicine", 308, 3),
			new PresetItemWithCount("Medicine", 332, 3),
			new PresetItemWithCount("Medicine", 236, 3),
			new PresetItemWithCount("Medicine", 248, 3),
			new PresetItemWithCount("Medicine", 260, 3)
		}, new List<PresetItemWithCount>
		{
			new PresetItemWithCount("Medicine", 296, 3),
			new PresetItemWithCount("Medicine", 272, 3)
		}));
		_dataArray.Add(new MapPickupsItem(537, 2069, EMapPickupsType.Event, EMapPickupsType2.Invalid, "map_eventicon_18", 2070, 0, new byte[15]
		{
			1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
			1, 1, 1, 1, 1
		}, new List<short>
		{
			39, 40, 41, 42, 43, 44, 45, 46, 47, 54,
			55, 56, 48, 49, 50, 51, 52, 53, 1, 2,
			3, 4, 5, 6, 7, 8, 9, 10, 11, 12,
			13, 14, 15, 34, 35, 36, 37
		}, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[0], default(PresetItemTemplateId), new sbyte[1], new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), -1, -1, -1, 2071, new int[2] { 2072, 2073 }, new int[2] { 2074, 2075 }, new int[2] { 689, 689 }, new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>
		{
			new PresetItemWithCount("Medicine", 104, 3),
			new PresetItemWithCount("Medicine", 92, 3)
		}, new List<PresetItemWithCount>
		{
			new PresetItemWithCount("Medicine", 116, 3),
			new PresetItemWithCount("Medicine", 320, 3)
		}));
		_dataArray.Add(new MapPickupsItem(538, 2076, EMapPickupsType.Event, EMapPickupsType2.Invalid, "map_eventicon_18", 2077, 0, new byte[15]
		{
			1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
			1, 1, 1, 1, 1
		}, new List<short>
		{
			39, 40, 41, 42, 43, 44, 45, 46, 47, 54,
			55, 56, 48, 49, 50, 51, 52, 53, 1, 2,
			3, 4, 5, 6, 7, 8, 9, 10, 11, 12,
			13, 14, 15, 34, 35, 36, 37
		}, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[0], default(PresetItemTemplateId), new sbyte[1], new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), -1, -1, -1, 2078, new int[2] { 2079, 1268 }, new int[2] { 2080, 2081 }, new int[2] { 689, 689 }, new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>
		{
			new PresetItemWithCount("Medicine", 64, 3),
			new PresetItemWithCount("Medicine", 76, 3)
		}, new List<PresetItemWithCount>
		{
			new PresetItemWithCount("Medicine", 344, 3),
			new PresetItemWithCount("Medicine", 128, 3)
		}));
		_dataArray.Add(new MapPickupsItem(539, 2082, EMapPickupsType.Event, EMapPickupsType2.Invalid, "map_eventicon_18", 2083, 0, new byte[15]
		{
			1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
			1, 1, 1, 1, 1
		}, new List<short>
		{
			39, 40, 41, 42, 43, 44, 45, 46, 47, 54,
			55, 56, 48, 49, 50, 51, 52, 53, 1, 2,
			3, 4, 5, 6, 7, 8, 9, 10, 11, 12,
			13, 14, 15, 34, 35, 36, 37
		}, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[0], default(PresetItemTemplateId), new sbyte[1], new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), -1, -1, -1, 2084, new int[2] { 2085, 2086 }, new int[2] { 2087, 2088 }, new int[2] { 689, 689 }, new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>
		{
			new PresetItemWithCount("Medicine", 69, 3),
			new PresetItemWithCount("Medicine", 72, 3),
			new PresetItemWithCount("Medicine", 57, 3),
			new PresetItemWithCount("Medicine", 60, 3)
		}, new List<PresetItemWithCount>
		{
			new PresetItemWithCount("Medicine", 97, 3),
			new PresetItemWithCount("Medicine", 100, 3),
			new PresetItemWithCount("Medicine", 85, 3),
			new PresetItemWithCount("Medicine", 88, 3)
		}));
	}

	private void CreateItems9()
	{
		_dataArray.Add(new MapPickupsItem(540, 2089, EMapPickupsType.Event, EMapPickupsType2.Invalid, "map_eventicon_18", 2090, 0, new byte[15]
		{
			1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
			1, 1, 1, 1, 1
		}, new List<short>
		{
			39, 40, 41, 42, 43, 44, 45, 46, 47, 54,
			55, 56, 48, 49, 50, 51, 52, 53, 1, 2,
			3, 4, 5, 6, 7, 8, 9, 10, 11, 12,
			13, 14, 15, 34, 35, 36, 37
		}, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[0], default(PresetItemTemplateId), new sbyte[1], new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), -1, -1, -1, 2091, new int[2] { 1188, 2092 }, new int[2] { 2093, 2094 }, new int[2] { 689, 689 }, new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>
		{
			new PresetItemWithCount("Medicine", 123, 3),
			new PresetItemWithCount("Medicine", 126, 3),
			new PresetItemWithCount("Medicine", 339, 3),
			new PresetItemWithCount("Medicine", 342, 3)
		}, new List<PresetItemWithCount>
		{
			new PresetItemWithCount("Medicine", 219, 3),
			new PresetItemWithCount("Medicine", 222, 3),
			new PresetItemWithCount("Medicine", 207, 3),
			new PresetItemWithCount("Medicine", 210, 3)
		}));
		_dataArray.Add(new MapPickupsItem(541, 2095, EMapPickupsType.Event, EMapPickupsType2.Invalid, "map_eventicon_18", 2096, 0, new byte[15]
		{
			1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
			1, 1, 1, 1, 1
		}, new List<short>
		{
			39, 40, 41, 42, 43, 44, 45, 46, 47, 54,
			55, 56, 48, 49, 50, 51, 52, 53, 1, 2,
			3, 4, 5, 6, 7, 8, 9, 10, 11, 12,
			13, 14, 15, 34, 35, 36, 37
		}, readEffect: false, loopEffect: false, isExpBonus: false, isDebtBonus: false, new int[0], new sbyte[0], default(PresetItemTemplateId), new sbyte[1], new short[6], new bool[12]
		{
			true, true, true, true, true, true, true, true, true, true,
			true, true
		}, -1, new OrganizationApproving(), -1, -1, -1, 2097, new int[2] { 1419, 2098 }, new int[2] { 2099, 2100 }, new int[2] { 689, 689 }, new List<PresetItemWithCount>(), new List<ResourceInfo>(), new List<PropertyAndValue>(), new List<int>(), new List<int>(), new List<PresetItemWithCount>
		{
			new PresetItemWithCount("Medicine", 284, 3),
			new PresetItemWithCount("Medicine", 308, 3),
			new PresetItemWithCount("Medicine", 332, 3),
			new PresetItemWithCount("Medicine", 236, 3),
			new PresetItemWithCount("Medicine", 248, 3),
			new PresetItemWithCount("Medicine", 260, 3)
		}, new List<PresetItemWithCount>
		{
			new PresetItemWithCount("Medicine", 7, 3),
			new PresetItemWithCount("Medicine", 16, 3),
			new PresetItemWithCount("Medicine", 25, 3),
			new PresetItemWithCount("Medicine", 34, 3),
			new PresetItemWithCount("Medicine", 43, 3),
			new PresetItemWithCount("Medicine", 52, 3),
			new PresetItemWithCount("Medicine", 140, 3),
			new PresetItemWithCount("Medicine", 152, 3),
			new PresetItemWithCount("Medicine", 164, 3),
			new PresetItemWithCount("Medicine", 176, 3),
			new PresetItemWithCount("Medicine", 188, 3),
			new PresetItemWithCount("Medicine", 200, 3)
		}));
	}

	public override void Init()
	{
		base.Init();
		_dataArray = new List<MapPickupsItem>(542);
		CreateItems0();
		CreateItems1();
		CreateItems2();
		CreateItems3();
		CreateItems4();
		CreateItems5();
		CreateItems6();
		CreateItems7();
		CreateItems8();
		CreateItems9();
	}
}
