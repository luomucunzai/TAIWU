using System;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using GameData.Serializer;

namespace GameData.Domains.Character;

[Serializable]
public struct ResourceInts : ISerializableGameData, ISerializable
{
	public unsafe fixed int Items[8];

	public unsafe ref int this[int index]
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			if (index < 0 || index >= 8)
			{
				throw new IndexOutOfRangeException($"index {index} is out of range [0,{8})");
			}
			return ref Items[index];
		}
	}

	public int Get(int index)
	{
		return this[index];
	}

	public int Set(int index, int value)
	{
		return this[index] = value;
	}

	public int Change(int index, int delta)
	{
		return this[index] += delta;
	}

	public unsafe void Initialize()
	{
		fixed (int* items = Items)
		{
			*(long*)items = 0L;
			((long*)items)[1] = 0L;
			((long*)items)[2] = 0L;
			((long*)items)[3] = 0L;
		}
	}

	public unsafe ResourceInts(params int[] amounts)
	{
		for (int i = 0; i < 8; i++)
		{
			Items[i] = amounts[i];
		}
	}

	public bool IsSerializedSizeFixed()
	{
		return true;
	}

	public int GetSerializedSize()
	{
		return 32;
	}

	public unsafe int Serialize(byte* pData)
	{
		fixed (int* items = Items)
		{
			*(long*)pData = *(long*)items;
			((long*)pData)[1] = ((long*)items)[1];
			((long*)pData)[2] = ((long*)items)[2];
			((long*)pData)[3] = ((long*)items)[3];
		}
		return 32;
	}

	public unsafe int Deserialize(byte* pData)
	{
		fixed (int* items = Items)
		{
			*(long*)items = *(long*)pData;
			((long*)items)[1] = ((long*)pData)[1];
			((long*)items)[2] = ((long*)pData)[2];
			((long*)items)[3] = ((long*)pData)[3];
		}
		return 32;
	}

	public unsafe ResourceInts(SerializationInfo info, StreamingContext context)
	{
		fixed (int* items = Items)
		{
			*(ulong*)items = info.GetUInt64("0");
			((long*)items)[1] = (long)info.GetUInt64("1");
			((long*)items)[2] = (long)info.GetUInt64("2");
			((long*)items)[3] = (long)info.GetUInt64("3");
		}
	}

	public unsafe void GetObjectData(SerializationInfo info, StreamingContext context)
	{
		fixed (int* items = Items)
		{
			info.AddValue("0", *(ulong*)items);
			info.AddValue("1", ((ulong*)items)[1]);
			info.AddValue("2", ((ulong*)items)[2]);
			info.AddValue("3", ((ulong*)items)[3]);
		}
	}

	public unsafe void Add(ref ResourceInts delta)
	{
		for (int i = 0; i < 8; i++)
		{
			Add((sbyte)i, delta.Items[i]);
		}
	}

	public unsafe void Add(sbyte type, int value)
	{
		int num = Items[type] + value;
		if (value < 0)
		{
			throw new Exception($"Resource amount cannot be negative: {type}, {value}");
		}
		Items[type] = num;
	}

	public unsafe void Subtract(sbyte type, int value)
	{
		int num = Items[type] - value;
		if (value < 0)
		{
			throw new Exception($"Resource amount cannot be negative: {type}, {value}");
		}
		Items[type] = num;
	}

	public unsafe ResourceInts Subtract(ref ResourceInts other)
	{
		ResourceInts result = default(ResourceInts);
		for (int i = 0; i < 8; i++)
		{
			result.Items[i] = Items[i] - other.Items[i];
		}
		return result;
	}

	public unsafe ResourceInts GetReversed()
	{
		ResourceInts result = default(ResourceInts);
		for (int i = 0; i < 8; i++)
		{
			result.Items[i] = -Items[i];
		}
		return result;
	}

	public unsafe bool IsNonZero()
	{
		for (int i = 0; i < 8; i++)
		{
			if (Items[i] != 0)
			{
				return true;
			}
		}
		return false;
	}

	public unsafe bool CheckIsMeet(ref ResourceInts needResources)
	{
		for (int i = 0; i < 8; i++)
		{
			if (Items[i] < needResources.Items[i])
			{
				return false;
			}
		}
		return true;
	}

	public unsafe bool CheckIsMeet(sbyte type, int value)
	{
		return Items[type] >= value;
	}

	public unsafe int GetSum()
	{
		int num = 0;
		for (int i = 0; i < 8; i++)
		{
			num += Items[i];
		}
		return num;
	}
}
