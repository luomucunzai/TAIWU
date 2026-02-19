using System.Collections.Generic;
using GameData.Serializer;

namespace GameData.Utilities;

public struct ShortList : ISerializableGameData
{
	public List<short> Items;

	public static ShortList Create()
	{
		ShortList result = default(ShortList);
		result.Items = new List<short>();
		return result;
	}

	public ShortList(ShortList other)
	{
		Items = ((other.Items != null) ? new List<short>(other.Items) : null);
	}

	public bool IsSerializedSizeFixed()
	{
		return false;
	}

	public int GetSerializedSize()
	{
		return 2 + 2 * (Items?.Count ?? 0);
	}

	public unsafe int Serialize(byte* pData)
	{
		byte* ptr = pData;
		if (Items != null)
		{
			int count = Items.Count;
			Tester.Assert(count <= 65535);
			*(ushort*)ptr = (ushort)count;
			ptr += 2;
			for (int i = 0; i < count; i++)
			{
				((short*)ptr)[i] = Items[i];
			}
			ptr += 2 * count;
		}
		else
		{
			*(short*)ptr = 0;
			ptr += 2;
		}
		return (int)(ptr - pData);
	}

	public unsafe int Deserialize(byte* pData)
	{
		byte* ptr = pData;
		ushort num = *(ushort*)ptr;
		ptr += 2;
		if (num > 0)
		{
			if (Items == null)
			{
				Items = new List<short>(num);
			}
			else
			{
				Items.Clear();
			}
			for (int i = 0; i < num; i++)
			{
				Items.Add(((short*)ptr)[i]);
			}
			ptr += 2 * num;
		}
		else
		{
			Items?.Clear();
		}
		return (int)(ptr - pData);
	}
}
