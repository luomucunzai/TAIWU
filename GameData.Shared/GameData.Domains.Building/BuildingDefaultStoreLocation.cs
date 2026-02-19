using System;
using System.Collections.Generic;
using GameData.Domains.Taiwu;
using GameData.Serializer;

namespace GameData.Domains.Building;

[SerializableGameData(IsExtensible = true)]
public class BuildingDefaultStoreLocation : ISerializableGameData
{
	private static class FieldIds
	{
		public const ushort Data = 0;

		public const ushort Count = 1;

		public static readonly string[] FieldId2FieldName = new string[1] { "Data" };
	}

	public const int StoreToInverntory = 0;

	public const int StoreToWarehouse = 1;

	public const int StoreToTreasury = 2;

	public const int StoreToGoodsShelf = 3;

	[SerializableGameDataField]
	private Dictionary<int, int> _data = new Dictionary<int, int>();

	public ItemSourceType GetMakeType(int fromType)
	{
		return ConvertDataToItemSource(GetMakeData(fromType));
	}

	public int GetMakeData(int data)
	{
		if (data >= 0 || data < -16)
		{
			if (!_data.TryGetValue(data, out data))
			{
				return 1;
			}
			return data;
		}
		if (!_data.TryGetValue(data, out data))
		{
			return 0;
		}
		return data;
	}

	public int SetMakeData(int type, int data)
	{
		return _data[type] = data;
	}

	public static ItemSourceType ConvertDataToItemSource(int data)
	{
		return data switch
		{
			0 => ItemSourceType.Inventory, 
			1 => ItemSourceType.Warehouse, 
			2 => ItemSourceType.Treasury, 
			3 => ItemSourceType.StockStorageGoodsShelf, 
			_ => throw new NotImplementedException($"not supported: {data}"), 
		};
	}

	public BuildingDefaultStoreLocation()
	{
	}

	public BuildingDefaultStoreLocation(BuildingDefaultStoreLocation other)
	{
		_data = ((other._data == null) ? null : new Dictionary<int, int>(other._data));
	}

	public void Assign(BuildingDefaultStoreLocation other)
	{
		_data = ((other._data == null) ? null : new Dictionary<int, int>(other._data));
	}

	public bool IsSerializedSizeFixed()
	{
		return false;
	}

	public int GetSerializedSize()
	{
		int num = 2;
		num += DictionaryOfBasicTypePair.GetSerializedSize<int, int>((IReadOnlyDictionary<int, int>)_data);
		if (num > 4)
		{
			return (num + 3) / 4 * 4;
		}
		return num;
	}

	public unsafe int Serialize(byte* pData)
	{
		*(short*)pData = 1;
		byte* num = pData + 2;
		int num2 = (int)(num + DictionaryOfBasicTypePair.Serialize<int, int>(num, ref _data) - pData);
		if (num2 > 4)
		{
			return (num2 + 3) / 4 * 4;
		}
		return num2;
	}

	public unsafe int Deserialize(byte* pData)
	{
		byte* ptr = pData;
		ushort num = *(ushort*)ptr;
		ptr += 2;
		if (num > 0)
		{
			ptr += DictionaryOfBasicTypePair.Deserialize<int, int>(ptr, ref _data);
		}
		int num2 = (int)(ptr - pData);
		if (num2 > 4)
		{
			return (num2 + 3) / 4 * 4;
		}
		return num2;
	}
}
