using System.Collections.Generic;
using GameData.Serializer;

namespace GameData.Utilities;

public struct SByteList : ISerializableGameData
{
	public List<sbyte> Items;

	public static SByteList Create()
	{
		SByteList result = default(SByteList);
		result.Items = new List<sbyte>();
		return result;
	}

	public SByteList(SByteList other)
	{
		Items = ((other.Items == null) ? null : new List<sbyte>(other.Items));
	}

	public SByteList(IEnumerable<sbyte> items)
	{
		Items = new List<sbyte>();
		if (items != null)
		{
			Items.AddRange(items);
		}
	}

	public bool IsSerializedSizeFixed()
	{
		return false;
	}

	public int GetSerializedSize()
	{
		return 4 + (Items?.Count ?? 0);
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
				ptr[i] = (byte)Items[i];
			}
			ptr += count;
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
				Items = new List<sbyte>(num);
			}
			else
			{
				Items.Clear();
			}
			for (int i = 0; i < num; i++)
			{
				Items.Add((sbyte)ptr[i]);
			}
			ptr += num;
		}
		else
		{
			Items?.Clear();
		}
		return (int)(ptr - pData);
	}
}
