using System.Collections.Generic;
using GameData.Domains.Item;
using GameData.Domains.Map;
using GameData.Serializer;

namespace GameData.Domains.Character.SortFilter;

[SerializableGameData(NotForArchive = true)]
public class CharacterSortFilterSettings : ISerializableGameData
{
	public sbyte FilterType;

	public sbyte FilterSubType;

	public int FilterSubId;

	public int TargetCharId;

	public Location TargetLocation;

	public ItemKey VillagerNeededItem;

	public readonly List<(int type, bool isDescending)> SortOrder;

	public CharacterSortFilterSettings()
	{
		FilterType = -1;
		FilterSubType = -1;
		TargetCharId = -1;
		TargetLocation = Location.Invalid;
		FilterSubId = -1;
		SortOrder = new List<(int, bool)>();
	}

	public bool IsSerializedSizeFixed()
	{
		return false;
	}

	public int GetSerializedSize()
	{
		int num = 12 + TargetLocation.GetSerializedSize() + SortOrder.Count * 5 + VillagerNeededItem.GetSerializedSize();
		if (num > 4)
		{
			return (num + 3) / 4 * 4;
		}
		return num;
	}

	public unsafe int Serialize(byte* pData)
	{
		byte* ptr = pData;
		*ptr = (byte)FilterType;
		ptr++;
		*ptr = (byte)FilterSubType;
		ptr++;
		*(int*)ptr = FilterSubId;
		ptr += 4;
		*(int*)ptr = TargetCharId;
		ptr += 4;
		ptr += TargetLocation.Serialize(ptr);
		ptr += VillagerNeededItem.Serialize(ptr);
		*(ushort*)ptr = (ushort)SortOrder.Count;
		ptr += 2;
		foreach (var item in SortOrder)
		{
			*(int*)ptr = item.type;
			ptr += 4;
			*ptr = (item.isDescending ? ((byte)1) : ((byte)0));
			ptr++;
		}
		int num = (int)(ptr - pData);
		if (num > 4)
		{
			return (num + 3) / 4 * 4;
		}
		return num;
	}

	public unsafe int Deserialize(byte* pData)
	{
		byte* ptr = pData;
		FilterType = (sbyte)(*ptr);
		ptr++;
		FilterSubType = (sbyte)(*ptr);
		ptr++;
		FilterSubId = *(int*)ptr;
		ptr += 4;
		TargetCharId = *(int*)ptr;
		ptr += 4;
		ptr += TargetLocation.Deserialize(ptr);
		ptr += VillagerNeededItem.Deserialize(ptr);
		ushort num = *(ushort*)ptr;
		ptr += 2;
		SortOrder.Clear();
		for (int i = 0; i < num; i++)
		{
			int item = *(int*)ptr;
			ptr += 4;
			bool item2 = *ptr != 0;
			ptr++;
			SortOrder.Add((item, item2));
		}
		int num2 = (int)(ptr - pData);
		if (num2 > 4)
		{
			return (num2 + 3) / 4 * 4;
		}
		return num2;
	}
}
