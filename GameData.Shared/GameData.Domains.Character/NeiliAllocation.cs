using System;
using GameData.Serializer;

namespace GameData.Domains.Character;

public struct NeiliAllocation : ISerializableGameData
{
	public unsafe fixed short Items[4];

	public unsafe ref short this[int neiliAllocationType]
	{
		get
		{
			if ((neiliAllocationType < 0 || neiliAllocationType >= 4) ? true : false)
			{
				throw new IndexOutOfRangeException("neiliAllocationType");
			}
			return ref Items[neiliAllocationType];
		}
	}

	public unsafe void Initialize()
	{
		fixed (short* items = Items)
		{
			*(long*)items = 0L;
		}
	}

	public bool IsSerializedSizeFixed()
	{
		return true;
	}

	public int GetSerializedSize()
	{
		return 8;
	}

	public unsafe int Serialize(byte* pData)
	{
		fixed (short* items = Items)
		{
			*(long*)pData = *(long*)items;
		}
		return 8;
	}

	public unsafe int Deserialize(byte* pData)
	{
		fixed (short* items = Items)
		{
			*(long*)items = *(long*)pData;
		}
		return 8;
	}

	public unsafe static bool Equals(NeiliAllocation lhs, NeiliAllocation rhs)
	{
		return *(long*)(&lhs) == *(long*)(&rhs);
	}

	public unsafe short GetTotal()
	{
		return (short)(Items[0] + Items[1] + Items[2] + Items[3]);
	}

	public unsafe NeiliAllocation Subtract(NeiliAllocation other)
	{
		NeiliAllocation result = default(NeiliAllocation);
		for (int i = 0; i < 4; i++)
		{
			result.Items[i] = (short)(Items[i] - other.Items[i]);
		}
		return result;
	}

	public unsafe NeiliAllocation GetReversed()
	{
		NeiliAllocation result = default(NeiliAllocation);
		for (int i = 0; i < 4; i++)
		{
			result.Items[i] = (short)(-Items[i]);
		}
		return result;
	}

	public unsafe NeiliAllocation GetHalf()
	{
		NeiliAllocation result = default(NeiliAllocation);
		for (int i = 0; i < 4; i++)
		{
			result.Items[i] = (short)(Items[i] / 2);
		}
		return result;
	}

	public unsafe byte GetMaxType()
	{
		byte b = 0;
		short num = Items[(int)b];
		for (byte b2 = 1; b2 < 4; b2++)
		{
			if (Items[(int)b2] > num)
			{
				b = b2;
				num = Items[(int)b2];
			}
		}
		return b;
	}

	public unsafe int Sum()
	{
		int num = 0;
		for (int i = 0; i < 4; i++)
		{
			num += Items[i];
		}
		return num;
	}

	public unsafe override string ToString()
	{
		return $"NeiliAllocation{{{Items[0]}, {Items[1]}, {Items[2]}, {Items[3]}}}";
	}
}
