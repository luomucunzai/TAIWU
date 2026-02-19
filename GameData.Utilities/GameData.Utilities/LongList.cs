using System.Collections.Generic;
using GameData.Serializer;

namespace GameData.Utilities;

public struct LongList : ISerializableGameData
{
	public List<long> Items;

	public static LongList Create()
	{
		LongList result = default(LongList);
		result.Items = new List<long>();
		return result;
	}

	public bool IsSerializedSizeFixed()
	{
		return false;
	}

	public int GetSerializedSize()
	{
		return 4 + 8 * (Items?.Count ?? 0);
	}

	public unsafe int Serialize(byte* pData)
	{
		byte* ptr = pData;
		if (Items != null)
		{
			int count = Items.Count;
			Tester.Assert(count <= 65535);
			*(int*)ptr = count;
			ptr += 4;
			for (int i = 0; i < count; i++)
			{
				((long*)ptr)[i] = Items[i];
			}
			ptr += 8 * count;
		}
		else
		{
			*(int*)ptr = 0;
			ptr += 4;
		}
		return (int)(ptr - pData);
	}

	public unsafe int Deserialize(byte* pData)
	{
		byte* ptr = pData;
		int num = *(int*)ptr;
		ptr += 4;
		if (num > 0)
		{
			if (Items == null)
			{
				Items = new List<long>(num);
			}
			else
			{
				Items.Clear();
			}
			for (int i = 0; i < num; i++)
			{
				Items.Add(((long*)ptr)[i]);
			}
			ptr += 8 * num;
		}
		else
		{
			Items?.Clear();
		}
		return (int)(ptr - pData);
	}
}
