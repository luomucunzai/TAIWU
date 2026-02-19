using System;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using GameData.Serializer;
using Redzen.Random;

namespace GameData.Domains.Character;

[Serializable]
public struct LifeSkillShorts : ISerializableGameData, ISerializable
{
	public unsafe fixed short Items[16];

	public unsafe ref short this[int index]
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			if (index < 0 || index >= 16)
			{
				throw new IndexOutOfRangeException($"index {index} is out of range [0,{16})");
			}
			return ref Items[index];
		}
	}

	public short Get(int index)
	{
		return this[index];
	}

	public short Set(int index, short value)
	{
		return this[index] = value;
	}

	public short Change(int index, short delta)
	{
		return this[index] += delta;
	}

	public unsafe void Initialize()
	{
		fixed (short* items = Items)
		{
			*(long*)items = 0L;
			((long*)items)[1] = 0L;
			((long*)items)[2] = 0L;
			((long*)items)[3] = 0L;
		}
	}

	public unsafe LifeSkillShorts(params short[] values)
	{
		for (int i = 0; i < 16; i++)
		{
			Items[i] = values[i];
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
		fixed (short* items = Items)
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
		fixed (short* items = Items)
		{
			*(long*)items = *(long*)pData;
			((long*)items)[1] = ((long*)pData)[1];
			((long*)items)[2] = ((long*)pData)[2];
			((long*)items)[3] = ((long*)pData)[3];
		}
		return 32;
	}

	public unsafe LifeSkillShorts(SerializationInfo info, StreamingContext context)
	{
		fixed (short* items = Items)
		{
			*(ulong*)items = info.GetUInt64("0");
			((long*)items)[1] = (long)info.GetUInt64("1");
			((long*)items)[2] = (long)info.GetUInt64("2");
			((long*)items)[3] = (long)info.GetUInt64("3");
		}
	}

	public unsafe void GetObjectData(SerializationInfo info, StreamingContext context)
	{
		fixed (short* items = Items)
		{
			info.AddValue("0", *(ulong*)items);
			info.AddValue("1", ((ulong*)items)[1]);
			info.AddValue("2", ((ulong*)items)[2]);
			info.AddValue("3", ((ulong*)items)[3]);
		}
	}

	public unsafe int GetSum()
	{
		int num = 0;
		for (int i = 0; i < 16; i++)
		{
			num += Items[i];
		}
		return num;
	}

	public unsafe LifeSkillShorts Subtract(ref LifeSkillShorts other)
	{
		LifeSkillShorts result = default(LifeSkillShorts);
		for (int i = 0; i < 16; i++)
		{
			result.Items[i] = (short)(Items[i] - other.Items[i]);
		}
		return result;
	}

	public unsafe LifeSkillShorts GetReversed()
	{
		LifeSkillShorts result = default(LifeSkillShorts);
		for (int i = 0; i < 16; i++)
		{
			result.Items[i] = (short)(-Items[i]);
		}
		return result;
	}

	public unsafe short GetMaxLifeSkillValue()
	{
		short num = short.MinValue;
		for (sbyte b = 0; b < 16; b++)
		{
			if (Items[b] > num)
			{
				num = Items[b];
			}
		}
		return num;
	}

	public unsafe sbyte GetMaxLifeSkillType()
	{
		short num = Items[0];
		sbyte result = 0;
		for (sbyte b = 1; b < 16; b++)
		{
			if (Items[b] > num)
			{
				num = Items[b];
				result = b;
			}
		}
		return result;
	}

	public unsafe sbyte GetMaxLifeSkillType(IRandomSource random)
	{
		short num = short.MinValue;
		for (sbyte b = 0; b < 16; b++)
		{
			if (Items[b] > num)
			{
				num = Items[b];
			}
		}
		sbyte* ptr = stackalloc sbyte[16];
		int num2 = 0;
		for (sbyte b2 = 0; b2 < 16; b2++)
		{
			if (Items[b2] == num)
			{
				ptr[num2] = b2;
				num2++;
			}
		}
		return ptr[random.Next(0, num2)];
	}
}
