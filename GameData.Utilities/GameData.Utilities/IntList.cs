using System.Collections.Generic;
using GameData.Serializer;

namespace GameData.Utilities;

public struct IntList : ISerializableGameData
{
	public List<int> Items;

	public static IntList Create()
	{
		IntList result = default(IntList);
		result.Items = new List<int>();
		return result;
	}

	public IntList(IntList other)
	{
		Items = ((other.Items == null) ? null : new List<int>(other.Items));
	}

	public bool IsSerializedSizeFixed()
	{
		return false;
	}

	public int GetSerializedSize()
	{
		return 4 + 4 * (Items?.Count ?? 0);
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
				((int*)ptr)[i] = Items[i];
			}
			ptr += 4 * count;
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
				Items = new List<int>(num);
			}
			else
			{
				Items.Clear();
			}
			for (int i = 0; i < num; i++)
			{
				Items.Add(((int*)ptr)[i]);
			}
			ptr += 4 * num;
		}
		else
		{
			Items?.Clear();
		}
		return (int)(ptr - pData);
	}
}
