using System;

namespace GameData.Domains.Taiwu;

public enum TaiwuVillageStorageType : sbyte
{
	Inventory = -1,
	Warehouse = -2,
	Treasury = 0,
	Stock = 1,
	[Obsolete]
	Craft = 2,
	[Obsolete]
	Medicine = 3,
	[Obsolete]
	Food = 4,
	Count = 5
}
