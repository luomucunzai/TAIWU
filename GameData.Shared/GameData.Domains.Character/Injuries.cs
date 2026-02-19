using System;
using System.Runtime.Serialization;
using GameData.Serializer;
using GameData.Utilities;

namespace GameData.Domains.Character;

[Serializable]
public struct Injuries : ISerializableGameData, ISerializable
{
	public const sbyte MaxLevel = 6;

	private const int Capacity = 8;

	public unsafe fixed sbyte Items[16];

	public unsafe void Initialize()
	{
		fixed (sbyte* items = Items)
		{
			*(long*)items = 0L;
			((long*)items)[1] = 0L;
		}
	}

	public unsafe Injuries(params sbyte[] injuries)
	{
		for (int i = 0; i < 14; i++)
		{
			sbyte b = injuries[i];
			if (b > 6)
			{
				throw new Exception("Invalid injury levels: " + string.Join(", ", injuries));
			}
			Items[i] = b;
		}
	}

	public bool IsSerializedSizeFixed()
	{
		return true;
	}

	public int GetSerializedSize()
	{
		return 16;
	}

	public unsafe int Serialize(byte* pData)
	{
		fixed (sbyte* items = Items)
		{
			*(long*)pData = *(long*)items;
			((long*)pData)[1] = ((long*)items)[1];
		}
		return 16;
	}

	public unsafe int Deserialize(byte* pData)
	{
		fixed (sbyte* items = Items)
		{
			*(long*)items = *(long*)pData;
			((long*)items)[1] = ((long*)pData)[1];
		}
		return 16;
	}

	public unsafe Injuries(SerializationInfo info, StreamingContext context)
	{
		fixed (sbyte* items = Items)
		{
			*(ulong*)items = info.GetUInt64("0");
			((long*)items)[1] = (long)info.GetUInt64("1");
		}
	}

	public unsafe void GetObjectData(SerializationInfo info, StreamingContext context)
	{
		fixed (sbyte* items = Items)
		{
			info.AddValue("0", *(ulong*)items);
			info.AddValue("1", ((ulong*)items)[1]);
		}
	}

	public unsafe (sbyte outer, sbyte inner) Get(sbyte bodyPartType)
	{
		int num = bodyPartType * 2;
		return (outer: Items[num], inner: Items[num + 1]);
	}

	public unsafe sbyte Get(sbyte bodyPartType, bool isInnerInjury)
	{
		return Items[bodyPartType * 2 + (isInnerInjury ? 1 : 0)];
	}

	public unsafe int GetSum()
	{
		int num = 0;
		for (int i = 0; i < 14; i++)
		{
			num += Items[i];
		}
		return num;
	}

	public (sbyte outer, sbyte inner) GetBothSum()
	{
		sbyte b = 0;
		sbyte b2 = 0;
		for (sbyte b3 = 0; b3 < 7; b3++)
		{
			(sbyte outer, sbyte inner) tuple = Get(b3);
			sbyte item = tuple.outer;
			sbyte item2 = tuple.inner;
			b += item;
			b2 += item2;
		}
		return (outer: b, inner: b2);
	}

	public unsafe bool HasAnyInjury()
	{
		fixed (sbyte* items = Items)
		{
			if (*(long*)items == 0L)
			{
				return ((long*)items)[1] != 0;
			}
			return true;
		}
	}

	public unsafe bool HasAnyInjury(bool isInnerInjury)
	{
		for (int i = (isInnerInjury ? 1 : 0); i < 14; i += 2)
		{
			if (Items[i] > 0)
			{
				return true;
			}
		}
		return false;
	}

	public bool AllPartsFully(bool isInnerInjury)
	{
		for (sbyte b = 0; b < 7; b++)
		{
			if (Get(b, isInnerInjury) < 6)
			{
				return false;
			}
		}
		return true;
	}

	public sbyte GetLightestPart(bool isInner, bool mustCanChanged = true)
	{
		sbyte result = -1;
		sbyte b = sbyte.MaxValue;
		for (sbyte b2 = 0; b2 < 7; b2++)
		{
			sbyte b3 = Get(b2, isInner);
			if ((!mustCanChanged || b3 < 6) && b3 < b)
			{
				result = b2;
				b = b3;
			}
		}
		return result;
	}

	public unsafe void Set(sbyte bodyPartType, bool isInnerInjury, sbyte value)
	{
		int num = bodyPartType * 2 + (isInnerInjury ? 1 : 0);
		Items[num] = MathUtils.Clamp(value, (sbyte)0, (sbyte)6);
	}

	public unsafe void Change(sbyte bodyPartType, bool isInnerInjury, int delta)
	{
		int num = bodyPartType * 2 + (isInnerInjury ? 1 : 0);
		Items[num] = (sbyte)MathUtils.Clamp(Items[num] + delta, 0, 6);
	}

	public unsafe void Change(int internalIndex, sbyte delta)
	{
		Items[internalIndex] = (sbyte)MathUtils.Clamp(Items[internalIndex] + delta, 0, 6);
	}

	public unsafe void Change(Injuries delta)
	{
		for (int i = 0; i < 14; i++)
		{
			Items[i] = (sbyte)MathUtils.Clamp(Items[i] + delta.Items[i], 0, 6);
		}
	}

	public unsafe void Change(Injuries delta, bool outerInjuryImmunity, bool innerInjuryImmunity)
	{
		for (int i = 0; i < 7; i++)
		{
			int num = i * 2;
			if (!outerInjuryImmunity)
			{
				Items[num] = (sbyte)MathUtils.Clamp(Items[num] + delta.Items[num], 0, 6);
			}
			int num2 = num + 1;
			if (!innerInjuryImmunity)
			{
				Items[num2] = (sbyte)MathUtils.Clamp(Items[num2] + delta.Items[num2], 0, 6);
			}
		}
	}

	public unsafe Injuries Subtract(Injuries other)
	{
		Injuries result = default(Injuries);
		for (int i = 0; i < 14; i++)
		{
			result.Items[i] = (sbyte)(Items[i] - other.Items[i]);
		}
		return result;
	}

	public unsafe Injuries GetReversed()
	{
		Injuries result = default(Injuries);
		for (int i = 0; i < 14; i++)
		{
			result.Items[i] = (sbyte)(-Items[i]);
		}
		return result;
	}

	public unsafe bool Equals(Injuries other)
	{
		bool result = true;
		for (int i = 0; i < 14; i++)
		{
			if (Items[i] != other.Items[i])
			{
				result = false;
				break;
			}
		}
		return result;
	}
}
