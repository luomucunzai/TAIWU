using System;
using System.Collections.Generic;
using Config.Common;

namespace Config;

[Serializable]
public class MerchantType : ConfigData<MerchantTypeItem, sbyte>
{
	public static class DefKey
	{
		public const sbyte Foods = 0;

		public const sbyte Books = 1;

		public const sbyte Materials = 2;

		public const sbyte Equipments = 3;

		public const sbyte Medicines = 4;

		public const sbyte Constructions = 5;

		public const sbyte Accessories = 6;

		public const sbyte FruitShop = 7;

		public const sbyte EMeiShop = 8;
	}

	public static class DefValue
	{
		public static MerchantTypeItem Foods => Instance[(sbyte)0];

		public static MerchantTypeItem Books => Instance[(sbyte)1];

		public static MerchantTypeItem Materials => Instance[(sbyte)2];

		public static MerchantTypeItem Equipments => Instance[(sbyte)3];

		public static MerchantTypeItem Medicines => Instance[(sbyte)4];

		public static MerchantTypeItem Constructions => Instance[(sbyte)5];

		public static MerchantTypeItem Accessories => Instance[(sbyte)6];

		public static MerchantTypeItem FruitShop => Instance[(sbyte)7];

		public static MerchantTypeItem EMeiShop => Instance[(sbyte)8];
	}

	public static MerchantType Instance = new MerchantType();

	private readonly HashSet<string> RequiredFields = new HashSet<string>
	{
		"HeadArea", "BranchArea", "TemplateId", "Name", "HeadLevel", "BranchLevel", "CaravanAvatar", "Icon", "Prologue", "IntroduceDialog",
		"FavorDialog1", "FavorDialog2", "FavorDialog3", "SpringSeasonDialog", "SummerSeasonDialog", "AutumnSeasonDialog", "WinterSeasonDialog", "SpringMarketsAdventureSeasonDialog", "EventContent", "EventDialogContent",
		"TaiwuVillagerMerchantChangingTypeContent", "RefreshDesc"
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
		_dataArray.Add(new MerchantTypeItem(0, 0, 6, 6, 5, 5, EMerchantTypeCityAttributeType.Safety, "NpcFace_funiubang", "sp_icon_shanghui_0", 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14));
		_dataArray.Add(new MerchantTypeItem(1, 15, 4, 6, 2, 5, EMerchantTypeCityAttributeType.Culture, "NpcFace_wenshanshuhaige", "sp_icon_shanghui_1", 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29));
		_dataArray.Add(new MerchantTypeItem(2, 30, 15, 6, 13, 5, EMerchantTypeCityAttributeType.Safety, "NpcFace_wuhushanghui", "sp_icon_shanghui_2", 31, 32, 33, 34, 35, 36, 37, 38, 39, 40, 41, 42, 43, 44));
		_dataArray.Add(new MerchantTypeItem(3, 45, 9, 6, 1, 5, EMerchantTypeCityAttributeType.Safety, "NpcFace_dawukuishanghao", "sp_icon_shanghui_3", 46, 47, 48, 49, 50, 51, 52, 53, 54, 55, 56, 57, 58, 59));
		_dataArray.Add(new MerchantTypeItem(4, 60, 3, 6, 10, 5, EMerchantTypeCityAttributeType.Safety, "NpcFace_huichuntang", "sp_icon_shanghui_4", 61, 62, 63, 64, 65, 66, 67, 68, 69, 70, 71, 72, 73, 74));
		_dataArray.Add(new MerchantTypeItem(5, 75, 7, 6, 12, 5, EMerchantTypeCityAttributeType.Culture, "NpcFace_gongshufang", "sp_icon_shanghui_5", 76, 77, 78, 79, 80, 81, 82, 83, 84, 85, 86, 87, 88, 89));
		_dataArray.Add(new MerchantTypeItem(6, 90, 8, 6, 14, 5, EMerchantTypeCityAttributeType.Culture, "NpcFace_qihuozhai", "sp_icon_shanghui_6", 91, 92, 93, 94, 95, 96, 97, 98, 99, 100, 101, 102, 103, 104));
		_dataArray.Add(new MerchantTypeItem(7, 105, -1, -1, -1, -1, EMerchantTypeCityAttributeType.Invalid, null, null, 106, 106, 107, 108, 109, 110, 111, 112, 113, 114, 115, 116, 117, 118));
		_dataArray.Add(new MerchantTypeItem(8, 119, -1, -1, -1, -1, EMerchantTypeCityAttributeType.Invalid, null, null, 120, 120, 121, 122, 123, 124, 125, 126, 127, 128, 129, 130, 131, 132));
	}

	public override void Init()
	{
		base.Init();
		_dataArray = new List<MerchantTypeItem>(9);
		CreateItems0();
	}
}
