using GameData.Serializer;

namespace GameData.Domains.Item;

[SerializableGameData(NotForArchive = true)]
public struct ItemKeyAndCount : ISerializableGameData
{
	[SerializableGameDataField]
	public ItemKey ItemKey;

	[SerializableGameDataField]
	public int Count;

	public static implicit operator ItemKeyAndCount((ItemKey itemKey, int count) tuple)
	{
		ItemKeyAndCount result = default(ItemKeyAndCount);
		(result.ItemKey, result.Count) = tuple;
		return result;
	}

	public static implicit operator ItemKeyAndCount(ItemKey itemKey)
	{
		return new ItemKeyAndCount
		{
			ItemKey = itemKey,
			Count = 1
		};
	}

	public ItemKeyAndCount(ItemKey itemKey, int count)
	{
		ItemKey = itemKey;
		Count = count;
	}

	public void Deconstruct(out ItemKey itemKey, out int count)
	{
		itemKey = ItemKey;
		count = Count;
	}

	public bool IsSerializedSizeFixed()
	{
		return true;
	}

	public int GetSerializedSize()
	{
		int num = 12;
		if (num > 4)
		{
			return (num + 3) / 4 * 4;
		}
		return num;
	}

	public unsafe int Serialize(byte* pData)
	{
		byte* ptr = pData;
		ptr += ItemKey.Serialize(ptr);
		*(int*)ptr = Count;
		ptr += 4;
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
		ptr += ItemKey.Deserialize(ptr);
		Count = *(int*)ptr;
		ptr += 4;
		int num = (int)(ptr - pData);
		if (num > 4)
		{
			return (num + 3) / 4 * 4;
		}
		return num;
	}
}
