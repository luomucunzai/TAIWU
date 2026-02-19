using System;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using GameData.Serializer;

namespace GameData.Domains.Character;

[Serializable]
public struct PoisonShorts : ISerializableGameData, ISerializable
{
	public unsafe fixed short Items[6];

	public unsafe ref short this[int index]
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			if (index < 0 || index >= 6)
			{
				throw new IndexOutOfRangeException($"index {index} is out of range [0,{(sbyte)6})");
			}
			return ref Items[index];
		}
	}

	public unsafe void Initialize()
	{
		fixed (short* items = Items)
		{
			*(long*)items = 0L;
			((int*)items)[2] = 0;
		}
	}

	public unsafe PoisonShorts(params int[] poisons)
	{
		for (int i = 0; i < 6; i++)
		{
			Items[i] = (short)poisons[i];
		}
	}

	public bool IsSerializedSizeFixed()
	{
		return true;
	}

	public int GetSerializedSize()
	{
		return 12;
	}

	public unsafe int Serialize(byte* pData)
	{
		fixed (short* items = Items)
		{
			*(long*)pData = *(long*)items;
			((int*)pData)[2] = ((int*)items)[2];
		}
		return 12;
	}

	public unsafe int Deserialize(byte* pData)
	{
		fixed (short* items = Items)
		{
			*(long*)items = *(long*)pData;
			((int*)items)[2] = ((int*)pData)[2];
		}
		return 12;
	}

	public unsafe PoisonShorts(SerializationInfo info, StreamingContext context)
	{
		fixed (short* items = Items)
		{
			*(ulong*)items = info.GetUInt64("0");
			((int*)items)[2] = (int)info.GetUInt32("1");
		}
	}

	public unsafe void GetObjectData(SerializationInfo info, StreamingContext context)
	{
		fixed (short* items = Items)
		{
			info.AddValue("0", *(ulong*)items);
			info.AddValue("1", ((uint*)items)[2]);
		}
	}

	public unsafe void Add(PoisonShorts delta)
	{
		for (int i = 0; i < 6; i++)
		{
			int num = Items[i] + delta.Items[i];
			if (num < 0)
			{
				num = 0;
			}
			Items[i] = (short)num;
		}
	}

	public unsafe PoisonShorts Subtract(PoisonShorts other)
	{
		PoisonShorts result = default(PoisonShorts);
		for (int i = 0; i < 6; i++)
		{
			result.Items[i] = (short)(Items[i] - other.Items[i]);
		}
		return result;
	}

	public unsafe PoisonShorts GetReversed()
	{
		PoisonShorts result = default(PoisonShorts);
		for (int i = 0; i < 6; i++)
		{
			result.Items[i] = (short)(-Items[i]);
		}
		return result;
	}

	public unsafe bool IsNonZero()
	{
		fixed (short* items = Items)
		{
			if (*(long*)items != 0L)
			{
				return true;
			}
			if (((uint*)items)[2] != 0)
			{
				return true;
			}
		}
		return false;
	}

	public unsafe short Sum()
	{
		short num = 0;
		for (int i = 0; i < 6; i++)
		{
			num += Items[i];
		}
		return num;
	}
}
