using System;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using GameData.Domains.Item;
using GameData.Serializer;

namespace GameData.Domains.Character;

[Serializable]
public struct PoisonInts : ISerializableGameData, ISerializable
{
	public unsafe fixed int Items[6];

	public unsafe ref int this[int index]
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
		fixed (int* items = Items)
		{
			*(long*)items = 0L;
			((long*)items)[1] = 0L;
			((long*)items)[2] = 0L;
		}
	}

	public unsafe PoisonInts(params int[] poisons)
	{
		for (int i = 0; i < 6; i++)
		{
			Items[i] = poisons[i];
		}
	}

	public bool IsSerializedSizeFixed()
	{
		return true;
	}

	public int GetSerializedSize()
	{
		return 24;
	}

	public unsafe int Serialize(byte* pData)
	{
		fixed (int* items = Items)
		{
			*(long*)pData = *(long*)items;
			((long*)pData)[1] = ((long*)items)[1];
			((long*)pData)[2] = ((long*)items)[2];
		}
		return 24;
	}

	public unsafe int Deserialize(byte* pData)
	{
		fixed (int* items = Items)
		{
			*(long*)items = *(long*)pData;
			((long*)items)[1] = ((long*)pData)[1];
			((long*)items)[2] = ((long*)pData)[2];
		}
		return 24;
	}

	public unsafe PoisonInts(SerializationInfo info, StreamingContext context)
	{
		fixed (int* items = Items)
		{
			*(ulong*)items = info.GetUInt64("0");
			((long*)items)[1] = (long)info.GetUInt64("1");
			((long*)items)[2] = (long)info.GetUInt64("2");
		}
	}

	public unsafe void GetObjectData(SerializationInfo info, StreamingContext context)
	{
		fixed (int* items = Items)
		{
			info.AddValue("0", *(ulong*)items);
			info.AddValue("1", ((ulong*)items)[1]);
			info.AddValue("2", ((ulong*)items)[2]);
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
			Items[i] = num;
		}
	}

	public unsafe void Add(ref PoisonInts delta)
	{
		for (int i = 0; i < 6; i++)
		{
			int num = Items[i] + delta.Items[i];
			if (num < 0)
			{
				num = 0;
			}
			Items[i] = num;
		}
	}

	public unsafe PoisonInts Subtract(ref PoisonInts other)
	{
		PoisonInts result = default(PoisonInts);
		for (int i = 0; i < 6; i++)
		{
			result.Items[i] = Items[i] - other.Items[i];
		}
		return result;
	}

	public unsafe PoisonInts GetReversed()
	{
		PoisonInts result = default(PoisonInts);
		for (int i = 0; i < 6; i++)
		{
			result.Items[i] = -Items[i];
		}
		return result;
	}

	public unsafe bool IsNonZero()
	{
		fixed (int* items = Items)
		{
			if (*(long*)items != 0L)
			{
				return true;
			}
			if (((long*)items)[1] != 0L)
			{
				return true;
			}
			if (((long*)items)[2] != 0L)
			{
				return true;
			}
		}
		return false;
	}

	public unsafe sbyte GetLightestType()
	{
		int num = 0;
		sbyte result = 0;
		for (sbyte b = 0; b < 6; b++)
		{
			if (Items[b] < num)
			{
				result = b;
				num = Items[b];
			}
		}
		return result;
	}

	public unsafe int Sum()
	{
		int num = 0;
		for (int i = 0; i < 6; i++)
		{
			num += Items[i];
		}
		return num;
	}

	public unsafe bool Equals(PoisonInts other)
	{
		bool result = true;
		for (int i = 0; i < 6; i++)
		{
			if (Items[i] != other.Items[i])
			{
				result = false;
				break;
			}
		}
		return result;
	}

	public unsafe int Get(int index)
	{
		return Items[index];
	}

	public unsafe PoisonsAndLevels GetPoisonsAndLevels()
	{
		PoisonsAndLevels result = default(PoisonsAndLevels);
		result.Initialize();
		for (int i = 0; i < 6; i++)
		{
			result.Values[i] = (short)Items[i];
			result.Levels[i] = PoisonsAndLevels.CalcPoisonedLevel(Items[i]);
		}
		return result;
	}
}
