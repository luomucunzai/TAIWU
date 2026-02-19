using System;
using GameData.Serializer;

namespace GameData.Domains.Item;

public struct ItemKeyAndDate : ISerializableGameData, IEquatable<ItemKeyAndDate>, IComparable<ItemKeyAndDate>
{
	public int Date;

	public ItemKey ItemKey;

	public ItemKeyAndDate(int date, ItemKey itemKey)
	{
		Date = date;
		ItemKey = itemKey;
	}

	public bool IsSerializedSizeFixed()
	{
		return true;
	}

	public unsafe int GetSerializedSize()
	{
		return 4 + sizeof(ItemKey);
	}

	public unsafe int Serialize(byte* pData)
	{
		*(int*)pData = Date;
		*(ItemKey*)(pData + 4) = ItemKey;
		return 4 + sizeof(ItemKey);
	}

	public unsafe int Deserialize(byte* pData)
	{
		Date = *(int*)pData;
		ItemKey = *(ItemKey*)(pData + 4);
		return 4 + sizeof(ItemKey);
	}

	public bool Equals(ItemKeyAndDate other)
	{
		if (Date == other.Date)
		{
			return ItemKey.Equals(other.ItemKey);
		}
		return false;
	}

	public override bool Equals(object obj)
	{
		if (obj is ItemKeyAndDate other)
		{
			return Equals(other);
		}
		return false;
	}

	public override int GetHashCode()
	{
		return (Date * 397) ^ ItemKey.GetHashCode();
	}

	public int CompareTo(ItemKeyAndDate other)
	{
		int num = Date - other.Date;
		if (num != 0)
		{
			return num;
		}
		return ItemKey.Id - other.ItemKey.Id;
	}
}
