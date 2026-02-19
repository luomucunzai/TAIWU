using System;
using System.Collections.Generic;
using GameData.Domains.Taiwu;
using GameData.Serializer;

namespace GameData.Domains.Building;

[SerializableGameData(IsExtensible = true)]
public class BuildingResourceOutputSetting : ISerializableGameData
{
	private static class FieldIds
	{
		public const ushort ResourceStorage = 0;

		public const ushort ItemStorage = 1;

		public const ushort Count = 2;

		public static readonly string[] FieldId2FieldName = new string[2] { "ResourceStorage", "ItemStorage" };
	}

	[SerializableGameDataField]
	public Dictionary<sbyte, sbyte> ResourceStorage;

	[SerializableGameDataField]
	public Dictionary<sbyte, sbyte> ItemStorage;

	public void Init()
	{
		InitResourceStorage();
		InitItemStorage();
	}

	public void InitResourceStorage()
	{
		if (ResourceStorage == null)
		{
			ResourceStorage = new Dictionary<sbyte, sbyte>();
		}
		ResourceStorage.Clear();
		for (sbyte b = 0; b <= 6; b++)
		{
			ResourceStorage[b] = -1;
		}
	}

	public void InitItemStorage()
	{
		if (ItemStorage == null)
		{
			ItemStorage = new Dictionary<sbyte, sbyte>();
		}
		ItemStorage.Clear();
		for (sbyte b = -1; b < 6; b++)
		{
			ItemStorage[b] = -2;
		}
	}

	public static ItemSourceType GetItemSourceTypeByStorageType(TaiwuVillageStorageType storageType, bool isMaterialItem, bool isMoney)
	{
		return storageType switch
		{
			TaiwuVillageStorageType.Inventory => ItemSourceType.Inventory, 
			TaiwuVillageStorageType.Warehouse => ItemSourceType.Warehouse, 
			TaiwuVillageStorageType.Treasury => ItemSourceType.Treasury, 
			TaiwuVillageStorageType.Stock => ItemSourceType.StockStorageGoodsShelf, 
			_ => throw new ArgumentOutOfRangeException("storageType", storageType, null), 
		};
	}

	public BuildingResourceOutputSetting()
	{
	}

	public BuildingResourceOutputSetting(BuildingResourceOutputSetting other)
	{
		ResourceStorage = ((other.ResourceStorage == null) ? null : new Dictionary<sbyte, sbyte>(other.ResourceStorage));
		ItemStorage = ((other.ItemStorage == null) ? null : new Dictionary<sbyte, sbyte>(other.ItemStorage));
	}

	public void Assign(BuildingResourceOutputSetting other)
	{
		ResourceStorage = ((other.ResourceStorage == null) ? null : new Dictionary<sbyte, sbyte>(other.ResourceStorage));
		ItemStorage = ((other.ItemStorage == null) ? null : new Dictionary<sbyte, sbyte>(other.ItemStorage));
	}

	public bool IsSerializedSizeFixed()
	{
		return false;
	}

	public int GetSerializedSize()
	{
		int num = 2;
		num += DictionaryOfBasicTypePair.GetSerializedSize<sbyte, sbyte>((IReadOnlyDictionary<sbyte, sbyte>)ResourceStorage);
		num += DictionaryOfBasicTypePair.GetSerializedSize<sbyte, sbyte>((IReadOnlyDictionary<sbyte, sbyte>)ItemStorage);
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
		byte* num2 = num + DictionaryOfBasicTypePair.Serialize<sbyte, sbyte>(num, ref ResourceStorage);
		int num3 = (int)(num2 + DictionaryOfBasicTypePair.Serialize<sbyte, sbyte>(num2, ref ItemStorage) - pData);
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
			ptr += DictionaryOfBasicTypePair.Deserialize<sbyte, sbyte>(ptr, ref ResourceStorage);
		}
		if (num > 1)
		{
			ptr += DictionaryOfBasicTypePair.Deserialize<sbyte, sbyte>(ptr, ref ItemStorage);
		}
		int num2 = (int)(ptr - pData);
		if (num2 > 4)
		{
			return (num2 + 3) / 4 * 4;
		}
		return num2;
	}
}
