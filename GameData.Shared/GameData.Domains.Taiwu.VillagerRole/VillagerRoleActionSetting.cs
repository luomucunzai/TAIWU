using System;
using System.Collections.Generic;
using Config;
using GameData.Domains.Item;
using GameData.Serializer;

namespace GameData.Domains.Taiwu.VillagerRole;

[SerializableGameData(IsExtensible = true)]
[Obsolete]
public class VillagerRoleActionSetting : ISerializableGameData
{
	private static class FieldIds
	{
		public const ushort AllowUseTaiwuInventoryResource = 0;

		public const ushort ActionStorageDict = 1;

		public const ushort Count = 2;

		public static readonly string[] FieldId2FieldName = new string[2] { "AllowUseTaiwuInventoryResource", "ActionStorageDict" };
	}

	[SerializableGameDataField]
	public bool AllowUseTaiwuInventoryResource;

	[SerializableGameDataField]
	public Dictionary<sbyte, sbyte> ActionStorageDict;

	public void SetActionStorage(VillagerRoleActionType actionType, VillagerRoleStorageType storageType)
	{
		ActionStorageDict[(sbyte)actionType] = (sbyte)storageType;
	}

	public ItemSourceType GetActionStorage(VillagerRoleActionType actionType)
	{
		if (!ActionStorageDict.TryGetValue((sbyte)actionType, out var value))
		{
			return GetItemSourceType(SharedData.StorageDefaultDict[actionType], ItemKey.Invalid);
		}
		return (ItemSourceType)value;
	}

	public static ItemSourceType GetItemSourceType(VillagerRoleStorageType roleStorageType, ItemKey itemKey)
	{
		return roleStorageType switch
		{
			VillagerRoleStorageType.Inventory => ItemSourceType.Inventory, 
			VillagerRoleStorageType.Warehouse => ItemSourceType.Warehouse, 
			VillagerRoleStorageType.Treasury => ItemSourceType.Treasury, 
			VillagerRoleStorageType.Trough => ItemSourceType.Trough, 
			VillagerRoleStorageType.StockStorageWarehouse => ItemSourceType.StockStorageWarehouse, 
			VillagerRoleStorageType.StockStorageGoodsShelf => ItemSourceType.StockStorageGoodsShelf, 
			VillagerRoleStorageType.CraftStorageWarehouse => ItemSourceType.CraftStorageWarehouse, 
			VillagerRoleStorageType.CraftStorageMaterial => ItemSourceType.CraftStorageMaterial, 
			VillagerRoleStorageType.CraftStorageToFix => ItemSourceType.CraftStorageToFix, 
			VillagerRoleStorageType.CraftStorageToDisassemble => ItemSourceType.CraftStorageToDisassemble, 
			VillagerRoleStorageType.MedicineStorageWarehouse => ItemSourceType.MedicineStorageWarehouse, 
			VillagerRoleStorageType.MedicineStorageMaterial => ItemSourceType.MedicineStorageMaterial, 
			VillagerRoleStorageType.MedicineStorageToDetox => ItemSourceType.MedicineStorageToDetox, 
			VillagerRoleStorageType.MedicineStorageToAddPoison => ItemSourceType.MedicineStorageToAddPoison, 
			VillagerRoleStorageType.FoodStorageWarehouse => ItemSourceType.FoodStorageWarehouse, 
			VillagerRoleStorageType.FoodStorageMaterial => ItemSourceType.FoodStorageMaterial, 
			VillagerRoleStorageType.AutoStorageWarehouse => GetAuto(itemKey), 
			VillagerRoleStorageType.AutoStorageMaterial => GetAuto(itemKey), 
			_ => throw new ArgumentOutOfRangeException("roleStorageType", roleStorageType, null), 
		};
		static ItemSourceType GetAuto(ItemKey itemKey2)
		{
			if (itemKey2.ItemType == 5 && Material.Instance[itemKey2.TemplateId].CraftableItemTypes.Count > 0)
			{
				return ItemTemplateHelper.GetResourceType(itemKey2.ItemType, itemKey2.TemplateId) switch
				{
					0 => ItemSourceType.FoodStorageMaterial, 
					1 => ItemSourceType.CraftStorageMaterial, 
					2 => ItemSourceType.CraftStorageMaterial, 
					3 => ItemSourceType.CraftStorageMaterial, 
					4 => ItemSourceType.CraftStorageMaterial, 
					5 => ItemSourceType.MedicineStorageMaterial, 
					_ => throw new ArgumentOutOfRangeException(), 
				};
			}
			return (ItemTemplateHelper.IsMiscResource(itemKey2.ItemType, itemKey2.TemplateId) ? ItemTemplateHelper.GetMiscResourceType(itemKey2.ItemType, itemKey2.TemplateId) : ItemTemplateHelper.GetResourceType(itemKey2.ItemType, itemKey2.TemplateId)) switch
			{
				0 => ItemSourceType.FoodStorageWarehouse, 
				1 => ItemSourceType.CraftStorageWarehouse, 
				2 => ItemSourceType.CraftStorageWarehouse, 
				3 => ItemSourceType.CraftStorageWarehouse, 
				4 => ItemSourceType.CraftStorageWarehouse, 
				5 => ItemSourceType.MedicineStorageWarehouse, 
				6 => ItemSourceType.StockStorageWarehouse, 
				_ => throw new ArgumentOutOfRangeException(), 
			};
		}
	}

	public static TaiwuVillageStorageType GetTaiwuVillageStorageTypeByItemSourceType(ItemSourceType itemSourceType)
	{
		return itemSourceType switch
		{
			ItemSourceType.Warehouse => TaiwuVillageStorageType.Warehouse, 
			ItemSourceType.Treasury => TaiwuVillageStorageType.Treasury, 
			ItemSourceType.StockStorageGoodsShelf => TaiwuVillageStorageType.Stock, 
			ItemSourceType.CraftStorageMaterial => TaiwuVillageStorageType.Treasury, 
			ItemSourceType.CraftStorageToDisassemble => TaiwuVillageStorageType.Craft, 
			ItemSourceType.MedicineStorageMaterial => TaiwuVillageStorageType.Medicine, 
			ItemSourceType.FoodStorageMaterial => TaiwuVillageStorageType.Food, 
			_ => throw new ArgumentOutOfRangeException(), 
		};
	}

	public VillagerRoleActionSetting()
	{
	}

	public VillagerRoleActionSetting(VillagerRoleActionSetting other)
	{
		AllowUseTaiwuInventoryResource = other.AllowUseTaiwuInventoryResource;
		ActionStorageDict = ((other.ActionStorageDict == null) ? null : new Dictionary<sbyte, sbyte>(other.ActionStorageDict));
	}

	public void Assign(VillagerRoleActionSetting other)
	{
		AllowUseTaiwuInventoryResource = other.AllowUseTaiwuInventoryResource;
		ActionStorageDict = ((other.ActionStorageDict == null) ? null : new Dictionary<sbyte, sbyte>(other.ActionStorageDict));
	}

	public bool IsSerializedSizeFixed()
	{
		return false;
	}

	public int GetSerializedSize()
	{
		int num = 3;
		num += DictionaryOfBasicTypePair.GetSerializedSize<sbyte, sbyte>((IReadOnlyDictionary<sbyte, sbyte>)ActionStorageDict);
		if (num > 4)
		{
			return (num + 3) / 4 * 4;
		}
		return num;
	}

	public unsafe int Serialize(byte* pData)
	{
		*(short*)pData = 2;
		byte* num = pData + 2;
		*num = (AllowUseTaiwuInventoryResource ? ((byte)1) : ((byte)0));
		byte* num2 = num + 1;
		int num3 = (int)(num2 + DictionaryOfBasicTypePair.Serialize<sbyte, sbyte>(num2, ref ActionStorageDict) - pData);
		if (num3 > 4)
		{
			return (num3 + 3) / 4 * 4;
		}
		return num3;
	}

	public unsafe int Deserialize(byte* pData)
	{
		byte* ptr = pData;
		ushort num = *(ushort*)ptr;
		ptr += 2;
		if (num > 0)
		{
			AllowUseTaiwuInventoryResource = *ptr != 0;
			ptr++;
		}
		if (num > 1)
		{
			ptr += DictionaryOfBasicTypePair.Deserialize<sbyte, sbyte>(ptr, ref ActionStorageDict);
		}
		int num2 = (int)(ptr - pData);
		if (num2 > 4)
		{
			return (num2 + 3) / 4 * 4;
		}
		return num2;
	}
}
