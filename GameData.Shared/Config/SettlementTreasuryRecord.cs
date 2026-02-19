using System;
using System.Collections.Generic;
using Config.Common;

namespace Config;

[Serializable]
public class SettlementTreasuryRecord : ConfigData<SettlementTreasuryRecordItem, short>
{
	public static class DefKey
	{
		public const short SupplementResource = 0;

		public const short SupplementItem = 1;

		public const short StorageResource = 2;

		public const short StorageItem = 3;

		public const short TakeOutResource = 4;

		public const short TakeOutItem = 5;

		public const short TaiwuStorageResource = 6;

		public const short TaiwuStorageItem = 7;

		public const short TaiwuTakeOutResource = 8;

		public const short TaiwuTakeOutItem = 9;

		public const short DonateSectTreasury = 10;

		public const short DonateTownTreasury = 11;

		public const short IntrudeSectTreasury = 12;

		public const short IntrudeTownTreasury = 13;

		public const short PlunderSectTreasurySuccess = 14;

		public const short PlunderTownTreasurySuccess = 15;

		public const short PlunderSectTreasuryFail = 16;

		public const short PlunderTownTreasuryFail = 17;

		public const short ConfiscateResource = 18;

		public const short ConfiscateItem = 19;

		public const short DistributeItem = 20;

		public const short ClearRecord = 21;

		public const short DistributeResource = 22;

		public const short SectStoryFulongLooting = 23;

		public const short DonateLegacy = 24;
	}

	public static class DefValue
	{
		public static SettlementTreasuryRecordItem SupplementResource => Instance[(short)0];

		public static SettlementTreasuryRecordItem SupplementItem => Instance[(short)1];

		public static SettlementTreasuryRecordItem StorageResource => Instance[(short)2];

		public static SettlementTreasuryRecordItem StorageItem => Instance[(short)3];

		public static SettlementTreasuryRecordItem TakeOutResource => Instance[(short)4];

		public static SettlementTreasuryRecordItem TakeOutItem => Instance[(short)5];

		public static SettlementTreasuryRecordItem TaiwuStorageResource => Instance[(short)6];

		public static SettlementTreasuryRecordItem TaiwuStorageItem => Instance[(short)7];

		public static SettlementTreasuryRecordItem TaiwuTakeOutResource => Instance[(short)8];

		public static SettlementTreasuryRecordItem TaiwuTakeOutItem => Instance[(short)9];

		public static SettlementTreasuryRecordItem DonateSectTreasury => Instance[(short)10];

		public static SettlementTreasuryRecordItem DonateTownTreasury => Instance[(short)11];

		public static SettlementTreasuryRecordItem IntrudeSectTreasury => Instance[(short)12];

		public static SettlementTreasuryRecordItem IntrudeTownTreasury => Instance[(short)13];

		public static SettlementTreasuryRecordItem PlunderSectTreasurySuccess => Instance[(short)14];

		public static SettlementTreasuryRecordItem PlunderTownTreasurySuccess => Instance[(short)15];

		public static SettlementTreasuryRecordItem PlunderSectTreasuryFail => Instance[(short)16];

		public static SettlementTreasuryRecordItem PlunderTownTreasuryFail => Instance[(short)17];

		public static SettlementTreasuryRecordItem ConfiscateResource => Instance[(short)18];

		public static SettlementTreasuryRecordItem ConfiscateItem => Instance[(short)19];

		public static SettlementTreasuryRecordItem DistributeItem => Instance[(short)20];

		public static SettlementTreasuryRecordItem ClearRecord => Instance[(short)21];

		public static SettlementTreasuryRecordItem DistributeResource => Instance[(short)22];

		public static SettlementTreasuryRecordItem SectStoryFulongLooting => Instance[(short)23];

		public static SettlementTreasuryRecordItem DonateLegacy => Instance[(short)24];
	}

	public static SettlementTreasuryRecord Instance = new SettlementTreasuryRecord();

	private readonly HashSet<string> RequiredFields = new HashSet<string> { "TemplateId", "Name", "Desc" };

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
		_dataArray.Add(new SettlementTreasuryRecordItem(0, 0, 1, new string[6] { "", "", "", "", "", "" }));
		_dataArray.Add(new SettlementTreasuryRecordItem(1, 0, 2, new string[6] { "", "", "", "", "", "" }));
		_dataArray.Add(new SettlementTreasuryRecordItem(2, 3, 4, new string[6] { "Character", "Resource", "Integer", "Integer", "", "" }));
		_dataArray.Add(new SettlementTreasuryRecordItem(3, 5, 6, new string[6] { "Character", "Item", "Integer", "", "", "" }));
		_dataArray.Add(new SettlementTreasuryRecordItem(4, 7, 8, new string[6] { "Character", "Resource", "Integer", "Integer", "", "" }));
		_dataArray.Add(new SettlementTreasuryRecordItem(5, 9, 10, new string[6] { "Character", "Item", "Integer", "", "", "" }));
		_dataArray.Add(new SettlementTreasuryRecordItem(6, 3, 11, new string[6] { "Character", "Resource", "Integer", "", "", "" }));
		_dataArray.Add(new SettlementTreasuryRecordItem(7, 5, 12, new string[6] { "Character", "Item", "", "", "", "" }));
		_dataArray.Add(new SettlementTreasuryRecordItem(8, 7, 13, new string[6] { "Character", "Resource", "Integer", "", "", "" }));
		_dataArray.Add(new SettlementTreasuryRecordItem(9, 9, 14, new string[6] { "Character", "Item", "", "", "", "" }));
		_dataArray.Add(new SettlementTreasuryRecordItem(10, 15, 16, new string[6] { "Character", "", "", "", "", "" }));
		_dataArray.Add(new SettlementTreasuryRecordItem(11, 15, 16, new string[6] { "Character", "", "", "", "", "" }));
		_dataArray.Add(new SettlementTreasuryRecordItem(12, 17, 18, new string[6] { "Character", "", "", "", "", "" }));
		_dataArray.Add(new SettlementTreasuryRecordItem(13, 17, 18, new string[6] { "Character", "", "", "", "", "" }));
		_dataArray.Add(new SettlementTreasuryRecordItem(14, 19, 20, new string[6] { "Character", "", "", "", "", "" }));
		_dataArray.Add(new SettlementTreasuryRecordItem(15, 19, 20, new string[6] { "Character", "", "", "", "", "" }));
		_dataArray.Add(new SettlementTreasuryRecordItem(16, 19, 21, new string[6] { "Character", "", "", "", "", "" }));
		_dataArray.Add(new SettlementTreasuryRecordItem(17, 19, 21, new string[6] { "Character", "", "", "", "", "" }));
		_dataArray.Add(new SettlementTreasuryRecordItem(18, 22, 23, new string[6] { "Character", "Resource", "Integer", "", "", "" }));
		_dataArray.Add(new SettlementTreasuryRecordItem(19, 22, 24, new string[6] { "Character", "Item", "", "", "", "" }));
		_dataArray.Add(new SettlementTreasuryRecordItem(20, 25, 26, new string[6] { "Character", "Item", "", "", "", "" }));
		_dataArray.Add(new SettlementTreasuryRecordItem(21, 27, 28, new string[6] { "", "", "", "", "", "" }));
		_dataArray.Add(new SettlementTreasuryRecordItem(22, 29, 30, new string[6] { "Character", "Resource", "Integer", "", "", "" }));
		_dataArray.Add(new SettlementTreasuryRecordItem(23, 31, 32, new string[6] { "Resource", "Integer", "Resource", "Integer", "", "" }));
		_dataArray.Add(new SettlementTreasuryRecordItem(24, 33, 34, new string[6] { "Character", "Item", "", "", "", "" }));
	}

	public override void Init()
	{
		base.Init();
		_dataArray = new List<SettlementTreasuryRecordItem>(25);
		CreateItems0();
	}
}
