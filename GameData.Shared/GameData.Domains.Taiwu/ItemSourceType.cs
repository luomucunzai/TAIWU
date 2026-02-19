using System;

namespace GameData.Domains.Taiwu;

public enum ItemSourceType
{
	Invalid = -1,
	Equipment,
	Inventory,
	Warehouse,
	Treasury,
	Trough,
	[Obsolete]
	StockStorageWarehouse,
	StockStorageGoodsShelf,
	[Obsolete]
	CraftStorageWarehouse,
	[Obsolete]
	CraftStorageMaterial,
	[Obsolete]
	CraftStorageToFix,
	[Obsolete]
	CraftStorageToDisassemble,
	[Obsolete]
	MedicineStorageWarehouse,
	[Obsolete]
	MedicineStorageMaterial,
	[Obsolete]
	MedicineStorageToDetox,
	[Obsolete]
	MedicineStorageToAddPoison,
	[Obsolete]
	FoodStorageWarehouse,
	[Obsolete]
	FoodStorageMaterial,
	EquipmentPlan,
	JiaoPool,
	SettlementTreasury,
	OldTreasury,
	Resources
}
