using System;
using System.Collections.Generic;
using Config.Common;

namespace Config;

[Serializable]
public class TaiwuVillageStoragesRecord : ConfigData<TaiwuVillageStoragesRecordItem, short>
{
	public static class DefKey
	{
		public const short TakeItem = 0;

		public const short StorageItem = 1;

		public const short StorageResources = 2;

		public const short TakeResources = 3;

		public const short GatherResources = 4;

		public const short MigrateResources = 5;

		public const short CookingIngredient = 6;

		public const short VillagerMakingItem = 7;

		public const short VillagerRepairItem = 8;

		public const short VillagerDisassembleItem0 = 9;

		public const short VillagerDisassembleItem1 = 10;

		public const short VillagerRefiningMedicine = 11;

		public const short VillagerDetoxify0 = 12;

		public const short VillagerDetoxify1 = 13;

		public const short VillagerEnvenomedItem = 14;

		public const short VillagerCure = 15;

		public const short VillagerSoldItem = 16;

		public const short VillagerBuyItem = 17;

		public const short OperatingBuilding = 18;

		public const short ClearRecord = 19;

		public const short EnvenomedItemOverload = 20;

		public const short DetoxifyItemOverload = 21;

		public const short GatherResourcesToTreasury = 22;

		public const short GatherResourcesToStockStorageGoodsShelf = 23;

		public const short GatherResourcesToFoodStorage = 24;

		public const short GatherResourcesToMedicineStorage = 25;

		public const short GatherResourcesToCraftStorage = 26;

		public const short GatherResourcesToCraftStorageToDisassemble = 27;

		public const short LoseOverloadResources = 28;

		public const short LoseOverloadWarehouseItems = 29;

		public const short VillagerGetRefineItem = 30;

		public const short VillagerUpgradeRefineItem = 31;

		public const short VillagerEarnMoney = 32;

		public const short VillagerEnemyDropItem = 33;

		public const short VillagerEnemyDropResources = 34;

		public const short VillagerMakeHarvest = 35;

		public const short OutsiderMakeHarvest = 36;

		public const short VillagerMakeHarvest1 = 37;

		public const short OutsiderMakeHarvest1 = 38;

		public const short VillagerMakeHarvest2 = 41;

		public const short OutsiderMakeHarvest2 = 42;

		public const short VillagerUpgradeRefineItem1 = 39;

		public const short VillagerDonateLegacy = 40;
	}

	public static class DefValue
	{
		public static TaiwuVillageStoragesRecordItem TakeItem => Instance[(short)0];

		public static TaiwuVillageStoragesRecordItem StorageItem => Instance[(short)1];

		public static TaiwuVillageStoragesRecordItem StorageResources => Instance[(short)2];

		public static TaiwuVillageStoragesRecordItem TakeResources => Instance[(short)3];

		public static TaiwuVillageStoragesRecordItem GatherResources => Instance[(short)4];

		public static TaiwuVillageStoragesRecordItem MigrateResources => Instance[(short)5];

		public static TaiwuVillageStoragesRecordItem CookingIngredient => Instance[(short)6];

		public static TaiwuVillageStoragesRecordItem VillagerMakingItem => Instance[(short)7];

		public static TaiwuVillageStoragesRecordItem VillagerRepairItem => Instance[(short)8];

		public static TaiwuVillageStoragesRecordItem VillagerDisassembleItem0 => Instance[(short)9];

		public static TaiwuVillageStoragesRecordItem VillagerDisassembleItem1 => Instance[(short)10];

		public static TaiwuVillageStoragesRecordItem VillagerRefiningMedicine => Instance[(short)11];

		public static TaiwuVillageStoragesRecordItem VillagerDetoxify0 => Instance[(short)12];

		public static TaiwuVillageStoragesRecordItem VillagerDetoxify1 => Instance[(short)13];

		public static TaiwuVillageStoragesRecordItem VillagerEnvenomedItem => Instance[(short)14];

		public static TaiwuVillageStoragesRecordItem VillagerCure => Instance[(short)15];

		public static TaiwuVillageStoragesRecordItem VillagerSoldItem => Instance[(short)16];

		public static TaiwuVillageStoragesRecordItem VillagerBuyItem => Instance[(short)17];

		public static TaiwuVillageStoragesRecordItem OperatingBuilding => Instance[(short)18];

		public static TaiwuVillageStoragesRecordItem ClearRecord => Instance[(short)19];

		public static TaiwuVillageStoragesRecordItem EnvenomedItemOverload => Instance[(short)20];

		public static TaiwuVillageStoragesRecordItem DetoxifyItemOverload => Instance[(short)21];

		public static TaiwuVillageStoragesRecordItem GatherResourcesToTreasury => Instance[(short)22];

		public static TaiwuVillageStoragesRecordItem GatherResourcesToStockStorageGoodsShelf => Instance[(short)23];

		public static TaiwuVillageStoragesRecordItem GatherResourcesToFoodStorage => Instance[(short)24];

		public static TaiwuVillageStoragesRecordItem GatherResourcesToMedicineStorage => Instance[(short)25];

		public static TaiwuVillageStoragesRecordItem GatherResourcesToCraftStorage => Instance[(short)26];

		public static TaiwuVillageStoragesRecordItem GatherResourcesToCraftStorageToDisassemble => Instance[(short)27];

		public static TaiwuVillageStoragesRecordItem LoseOverloadResources => Instance[(short)28];

		public static TaiwuVillageStoragesRecordItem LoseOverloadWarehouseItems => Instance[(short)29];

		public static TaiwuVillageStoragesRecordItem VillagerGetRefineItem => Instance[(short)30];

		public static TaiwuVillageStoragesRecordItem VillagerUpgradeRefineItem => Instance[(short)31];

		public static TaiwuVillageStoragesRecordItem VillagerEarnMoney => Instance[(short)32];

		public static TaiwuVillageStoragesRecordItem VillagerEnemyDropItem => Instance[(short)33];

		public static TaiwuVillageStoragesRecordItem VillagerEnemyDropResources => Instance[(short)34];

		public static TaiwuVillageStoragesRecordItem VillagerMakeHarvest => Instance[(short)35];

		public static TaiwuVillageStoragesRecordItem OutsiderMakeHarvest => Instance[(short)36];

		public static TaiwuVillageStoragesRecordItem VillagerMakeHarvest1 => Instance[(short)37];

		public static TaiwuVillageStoragesRecordItem OutsiderMakeHarvest1 => Instance[(short)38];

		public static TaiwuVillageStoragesRecordItem VillagerMakeHarvest2 => Instance[(short)41];

		public static TaiwuVillageStoragesRecordItem OutsiderMakeHarvest2 => Instance[(short)42];

		public static TaiwuVillageStoragesRecordItem VillagerUpgradeRefineItem1 => Instance[(short)39];

		public static TaiwuVillageStoragesRecordItem VillagerDonateLegacy => Instance[(short)40];
	}

	public static TaiwuVillageStoragesRecord Instance = new TaiwuVillageStoragesRecord();

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
		_dataArray.Add(new TaiwuVillageStoragesRecordItem(0, 0, 1, new string[6] { "Character", "Item", "", "", "", "" }));
		_dataArray.Add(new TaiwuVillageStoragesRecordItem(1, 2, 3, new string[6] { "Character", "Item", "", "", "", "" }));
		_dataArray.Add(new TaiwuVillageStoragesRecordItem(2, 4, 5, new string[6] { "Character", "Integer", "Resource", "", "", "" }));
		_dataArray.Add(new TaiwuVillageStoragesRecordItem(3, 6, 7, new string[6] { "Character", "Integer", "Resource", "", "", "" }));
		_dataArray.Add(new TaiwuVillageStoragesRecordItem(4, 8, 9, new string[6] { "Character", "Item", "", "", "", "" }));
		_dataArray.Add(new TaiwuVillageStoragesRecordItem(5, 10, 11, new string[6] { "Character", "Item", "", "", "", "" }));
		_dataArray.Add(new TaiwuVillageStoragesRecordItem(6, 12, 13, new string[6] { "Character", "Item", "Item", "", "", "" }));
		_dataArray.Add(new TaiwuVillageStoragesRecordItem(7, 14, 15, new string[6] { "Character", "Item", "Item", "", "", "" }));
		_dataArray.Add(new TaiwuVillageStoragesRecordItem(8, 16, 17, new string[6] { "Character", "Item", "", "", "", "" }));
		_dataArray.Add(new TaiwuVillageStoragesRecordItem(9, 18, 19, new string[6] { "Character", "Item", "", "", "", "" }));
		_dataArray.Add(new TaiwuVillageStoragesRecordItem(10, 18, 20, new string[6] { "Character", "Item", "Item", "", "", "" }));
		_dataArray.Add(new TaiwuVillageStoragesRecordItem(11, 21, 22, new string[6] { "Character", "Item", "Item", "", "", "" }));
		_dataArray.Add(new TaiwuVillageStoragesRecordItem(12, 23, 24, new string[6] { "Character", "Item", "Item", "", "", "" }));
		_dataArray.Add(new TaiwuVillageStoragesRecordItem(13, 25, 26, new string[6] { "Character", "Item", "Item", "Item", "", "" }));
		_dataArray.Add(new TaiwuVillageStoragesRecordItem(14, 27, 28, new string[6] { "Character", "Item", "Item", "", "", "" }));
		_dataArray.Add(new TaiwuVillageStoragesRecordItem(15, 29, 30, new string[6] { "Character", "Location", "Integer", "Resource", "", "" }));
		_dataArray.Add(new TaiwuVillageStoragesRecordItem(16, 31, 32, new string[6] { "Character", "Item", "Integer", "Resource", "", "" }));
		_dataArray.Add(new TaiwuVillageStoragesRecordItem(17, 33, 34, new string[6] { "Character", "Integer", "Resource", "Item", "", "" }));
		_dataArray.Add(new TaiwuVillageStoragesRecordItem(18, 35, 36, new string[6] { "Item", "Building", "", "", "", "" }));
		_dataArray.Add(new TaiwuVillageStoragesRecordItem(19, 37, 38, new string[6] { "", "", "", "", "", "" }));
		_dataArray.Add(new TaiwuVillageStoragesRecordItem(20, 27, 39, new string[6] { "Character", "Item", "Item", "Item", "Item", "" }));
		_dataArray.Add(new TaiwuVillageStoragesRecordItem(21, 25, 40, new string[6] { "Character", "Item", "Item", "Item", "Item", "Item" }));
		_dataArray.Add(new TaiwuVillageStoragesRecordItem(22, 8, 41, new string[6] { "Character", "Item", "", "", "", "" }));
		_dataArray.Add(new TaiwuVillageStoragesRecordItem(23, 8, 42, new string[6] { "Character", "Item", "", "", "", "" }));
		_dataArray.Add(new TaiwuVillageStoragesRecordItem(24, 8, 43, new string[6] { "Character", "Item", "", "", "", "" }));
		_dataArray.Add(new TaiwuVillageStoragesRecordItem(25, 8, 44, new string[6] { "Character", "Item", "", "", "", "" }));
		_dataArray.Add(new TaiwuVillageStoragesRecordItem(26, 8, 45, new string[6] { "Character", "Item", "", "", "", "" }));
		_dataArray.Add(new TaiwuVillageStoragesRecordItem(27, 8, 46, new string[6] { "Character", "Item", "", "", "", "" }));
		_dataArray.Add(new TaiwuVillageStoragesRecordItem(28, 47, 48, new string[6] { "Resource", "Integer", "", "", "", "" }));
		_dataArray.Add(new TaiwuVillageStoragesRecordItem(29, 49, 50, new string[6] { "Item", "", "", "", "", "" }));
		_dataArray.Add(new TaiwuVillageStoragesRecordItem(30, 51, 52, new string[6] { "Character", "Item", "", "", "", "" }));
		_dataArray.Add(new TaiwuVillageStoragesRecordItem(31, 53, 54, new string[6] { "Character", "Item", "", "", "", "" }));
		_dataArray.Add(new TaiwuVillageStoragesRecordItem(32, 55, 56, new string[6] { "Character", "Character", "Resource", "Integer", "", "" }));
		_dataArray.Add(new TaiwuVillageStoragesRecordItem(33, 57, 58, new string[6] { "Character", "Item", "", "", "", "" }));
		_dataArray.Add(new TaiwuVillageStoragesRecordItem(34, 59, 60, new string[6] { "Character", "Resource", "Integer", "", "", "" }));
		_dataArray.Add(new TaiwuVillageStoragesRecordItem(35, 61, 62, new string[6] { "Building", "Item", "", "", "", "" }));
		_dataArray.Add(new TaiwuVillageStoragesRecordItem(36, 61, 63, new string[6] { "Character", "Settlement", "Item", "", "", "" }));
		_dataArray.Add(new TaiwuVillageStoragesRecordItem(37, 61, 64, new string[6] { "Building", "Item", "", "", "", "" }));
		_dataArray.Add(new TaiwuVillageStoragesRecordItem(38, 61, 65, new string[6] { "Character", "Settlement", "Item", "", "", "" }));
		_dataArray.Add(new TaiwuVillageStoragesRecordItem(39, 53, 68, new string[6] { "Character", "Item", "Item", "", "", "" }));
		_dataArray.Add(new TaiwuVillageStoragesRecordItem(40, 69, 70, new string[6] { "Character", "Item", "", "", "", "" }));
		_dataArray.Add(new TaiwuVillageStoragesRecordItem(41, 61, 66, new string[6] { "Building", "Item", "", "", "", "" }));
		_dataArray.Add(new TaiwuVillageStoragesRecordItem(42, 61, 67, new string[6] { "Character", "Settlement", "Item", "", "", "" }));
	}

	public override void Init()
	{
		base.Init();
		_dataArray = new List<TaiwuVillageStoragesRecordItem>(43);
		CreateItems0();
	}
}
