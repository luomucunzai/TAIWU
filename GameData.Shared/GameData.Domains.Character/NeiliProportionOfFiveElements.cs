using System;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using GameData.Domains.CombatSkill;
using GameData.Serializer;
using GameData.Utilities;

namespace GameData.Domains.Character;

[Serializable]
public struct NeiliProportionOfFiveElements : ISerializableGameData, ISerializable
{
	public const sbyte MinValue = 0;

	public const sbyte MaxValue = 100;

	public unsafe fixed sbyte Items[5];

	public unsafe ref sbyte this[int index]
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			if (index < 0 || index >= 5)
			{
				throw new IndexOutOfRangeException($"index {index} is out of range [0,{5})");
			}
			return ref Items[index];
		}
	}

	public unsafe void Initialize()
	{
		fixed (sbyte* items = Items)
		{
			*(int*)items = 0;
			items[4] = 0;
		}
	}

	public unsafe NeiliProportionOfFiveElements(params sbyte[] proportions)
	{
		for (int i = 0; i < 5; i++)
		{
			Items[i] = proportions[i];
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
		fixed (sbyte* items = Items)
		{
			*(int*)pData = *(int*)items;
			((int*)pData)[1] = items[4];
		}
		return 8;
	}

	public unsafe int Deserialize(byte* pData)
	{
		fixed (sbyte* items = Items)
		{
			*(int*)items = *(int*)pData;
			items[4] = (sbyte)(byte)((int*)pData)[1];
		}
		return 8;
	}

	public unsafe NeiliProportionOfFiveElements(SerializationInfo info, StreamingContext context)
	{
		fixed (sbyte* items = Items)
		{
			*(uint*)items = info.GetUInt32("0");
			items[4] = (sbyte)info.GetByte("1");
		}
	}

	public unsafe void GetObjectData(SerializationInfo info, StreamingContext context)
	{
		fixed (sbyte* items = Items)
		{
			info.AddValue("0", *(uint*)items);
			info.AddValue("1", (byte)items[4]);
		}
	}

	public unsafe void Transfer(sbyte destType, sbyte transferType, int amount)
	{
		Tester.Assert(SumCheck() == 100);
		Tester.Assert(amount > 0);
		sbyte val = (sbyte)(100 - Items[destType]);
		sbyte b = (sbyte)Math.Min(amount, val);
		if (b <= 0)
		{
			return;
		}
		sbyte[] transferSources = GetTransferSources(transferType);
		sbyte b2 = destType;
		sbyte b3 = b;
		while (true)
		{
			b2 = transferSources[b2];
			sbyte b4 = Items[b2];
			if (b4 >= b3)
			{
				break;
			}
			if (b4 > 0)
			{
				Items[b2] = 0;
				b3 -= b4;
			}
		}
		ref sbyte reference = ref Items[b2];
		reference -= b3;
		ref sbyte reference2 = ref Items[destType];
		reference2 += b;
	}

	public unsafe int Sum()
	{
		int num = 0;
		for (int i = 0; i < 5; i++)
		{
			num += Items[i];
		}
		return num;
	}

	public unsafe int SumCheck()
	{
		for (int i = 0; i < 5; i++)
		{
			Tester.Assert(Items[i] >= 0 && Items[i] <= 100);
		}
		return Sum();
	}

	private static sbyte[] GetTransferSources(sbyte transferType)
	{
		return transferType switch
		{
			0 => FiveElementsType.Countered, 
			1 => FiveElementsType.Countering, 
			2 => FiveElementsType.Produced, 
			3 => FiveElementsType.Producing, 
			_ => throw new Exception($"Unsupported NeiliProportionTransferType: {transferType}"), 
		};
	}

	public static sbyte GetTransferSource(sbyte transferType, sbyte destFiveElementType)
	{
		return GetTransferSources(transferType)[destFiveElementType];
	}

	public static NeiliProportionOfFiveElements GetTotal(Span<NeiliProportionOfFiveElements> array)
	{
		NeiliProportionOfFiveElements result = default(NeiliProportionOfFiveElements);
		result.Initialize();
		Span<int> span = stackalloc int[5];
		GetSum(array, span);
		int num = 0;
		Span<int> span2 = span;
		for (int i = 0; i < span2.Length; i++)
		{
			int num2 = span2[i];
			num += num2;
		}
		if (num > 0)
		{
			for (int j = 0; j < 5; j++)
			{
				result[j] = (sbyte)(span[j] * 100 / num);
			}
		}
		return result;
	}

	public static void GetSum(Span<NeiliProportionOfFiveElements> array, Span<int> sumElements)
	{
		for (int i = 0; i < sumElements.Length; i++)
		{
			sumElements[i] = 0;
		}
		Span<NeiliProportionOfFiveElements> span = array;
		for (int j = 0; j < span.Length; j++)
		{
			NeiliProportionOfFiveElements neiliProportionOfFiveElements = span[j];
			for (int k = 0; k < 5; k++)
			{
				sumElements[k] += neiliProportionOfFiveElements[k];
			}
		}
	}
}
