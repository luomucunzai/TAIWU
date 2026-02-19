using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Redzen.Random;

namespace GameData.Utilities;

public static class RandomUtils
{
	public static void NextBytes(this IRandomSource random, Span<byte> buffer)
	{
		Span<byte> span = buffer;
		while (span.Length >= 8)
		{
			ulong value = random.NextULong();
			Unsafe.WriteUnaligned(ref MemoryMarshal.GetReference(span), value);
			span = span.Slice(8);
		}
		for (int i = 0; i < span.Length; i++)
		{
			span[i] = random.NextByte();
		}
	}

	public static List<T[]> GenerateRandomWeightCellListCommon<T>(IRandomSource random, IList<T[]> core, int count = 1, sbyte weightIdx = 1) where T : IComparable, IConvertible
	{
		int num = 0;
		List<(float, float)> list = new List<(float, float)>();
		for (int i = 0; i < core.Count; i++)
		{
			T[] array = core[i];
			num += array[weightIdx].ToInt32(null);
			list.Add((1f, -1f));
		}
		float num2 = (float)num / (float)core.Count;
		int j;
		for (j = 0; j < core.Count && core[j][weightIdx].ToSingle(null).CompareTo(num2) >= 0; j++)
		{
		}
		if (j < core.Count)
		{
			int k;
			for (k = 0; k < core.Count && core[k][weightIdx].ToSingle(null).CompareTo(num2) < 0; k++)
			{
			}
			(int, float) tuple = (j, (float)core[j][weightIdx].ToInt32(null) / num2);
			(int, float) tuple2 = (k, (float)core[k][weightIdx].ToInt32(null) / num2);
			while (true)
			{
				list[tuple.Item1] = (tuple.Item2, tuple2.Item1);
				tuple2 = (tuple2.Item1, tuple2.Item2 - (1f - tuple.Item2));
				if (tuple2.Item2 < 1f)
				{
					tuple = tuple2;
					for (k++; k < core.Count && core[k][weightIdx].ToSingle(null).CompareTo(num2) < 0; k++)
					{
					}
					if (k >= core.Count)
					{
						break;
					}
					tuple2 = (k, (float)core[k][weightIdx].ToInt32(null) / num2);
				}
				else
				{
					for (j++; j < core.Count && core[j][weightIdx].ToSingle(null).CompareTo(num2) >= 0; j++)
					{
					}
					if (j >= core.Count)
					{
						break;
					}
					tuple = (j, (float)core[j][weightIdx].ToInt32(null) / num2);
				}
			}
		}
		List<T[]> list2 = new List<T[]>();
		for (int l = 0; l < count; l++)
		{
			float num3 = random.NextFloat() * (float)core.Count;
			int num4 = (int)num3;
			(float, float) tuple3 = list[num4];
			if (num3 - (float)num4 > tuple3.Item1)
			{
				list2.Add(core[(int)tuple3.Item2]);
			}
			else
			{
				list2.Add(core[num4]);
			}
		}
		return list2;
	}

	public static int[] GenerateRandomWeightCell(IRandomSource random, IList<int[]> core, int weightIdx = 1)
	{
		int num = 0;
		int[] array = null;
		for (int i = 0; i < core.Count; i++)
		{
			int[] array2 = core[i];
			num += array2[weightIdx];
			if (array == null || array[weightIdx] < array2[weightIdx])
			{
				array = array2;
			}
		}
		int num2 = random.Next(0, num);
		int num3 = 0;
		for (int num4 = core.Count - 1; num4 >= 0; num4--)
		{
			num3 += core[num4][weightIdx];
			if (num2 >= num - num3)
			{
				return core[num4];
			}
		}
		return array;
	}

	public static T[] GenerateRandomWeightCell<T>(IRandomSource random, IList<T[]> core, int weightIdx = 1) where T : IComparable, IConvertible
	{
		int num = 0;
		T[] array = null;
		for (int i = 0; i < core.Count; i++)
		{
			T[] array2 = core[i];
			num += array2[weightIdx].ToInt32(null);
			if (array == null || array[weightIdx].CompareTo(array2[weightIdx]) < 0)
			{
				array = array2;
			}
		}
		int num2 = random.Next(0, num);
		int num3 = 0;
		for (int num4 = core.Count - 1; num4 >= 0; num4--)
		{
			num3 += core[num4][weightIdx].ToInt32(null);
			if (num2 >= num - num3)
			{
				return core[num4];
			}
		}
		return array;
	}

	public static T GetRandomResult<T>(IEnumerable<(T, short)> weights, IRandomSource random)
	{
		int num = 0;
		foreach (var weight in weights)
		{
			num += weight.Item2;
		}
		int num2 = random.Next(0, num);
		foreach (var weight2 in weights)
		{
			num2 -= weight2.Item2;
			if (num2 < 0)
			{
				return weight2.Item1;
			}
		}
		throw new Exception("Error Params");
	}

	public static (T, V) GetRandomResult<T, V>(IEnumerable<(T, short, V)> weights, IRandomSource random)
	{
		int num = 0;
		foreach (var weight in weights)
		{
			num += weight.Item2;
		}
		int num2 = random.Next(0, num);
		foreach (var weight2 in weights)
		{
			num2 -= weight2.Item2;
			if (num2 < 0)
			{
				return (weight2.Item1, weight2.Item3);
			}
		}
		throw new Exception("Error Params");
	}

	public static int GetRandomIndex(IReadOnlyList<short> weights, IRandomSource random)
	{
		int num = 0;
		foreach (short weight in weights)
		{
			num += weight;
		}
		int num2 = random.Next(0, num);
		for (int i = 0; i < weights.Count; i++)
		{
			num2 -= weights[i];
			if (num2 < 0)
			{
				return i;
			}
		}
		throw new Exception("Error Params");
	}

	public static int GetRandomIndex(IReadOnlyList<sbyte> weights, IRandomSource random)
	{
		int num = 0;
		foreach (sbyte weight in weights)
		{
			num += weight;
		}
		int num2 = random.Next(0, num);
		for (int i = 0; i < weights.Count; i++)
		{
			num2 -= weights[i];
			if (num2 < 0)
			{
				return i;
			}
		}
		throw new Exception("Error Params");
	}

	public static int GetRandomIndex(IReadOnlyList<int> weights, IRandomSource random)
	{
		int num = 0;
		foreach (int weight in weights)
		{
			num += weight;
		}
		int num2 = random.Next(0, num);
		for (int i = 0; i < weights.Count; i++)
		{
			num2 -= weights[i];
			if (num2 < 0)
			{
				return i;
			}
		}
		throw new Exception("Error Params");
	}

	public static int GetRandomIndex(Span<int> weights, IRandomSource random)
	{
		int num = 0;
		Span<int> span = weights;
		for (int i = 0; i < span.Length; i++)
		{
			int num2 = span[i];
			num += num2;
		}
		int num3 = random.Next(0, num);
		for (int j = 0; j < weights.Length; j++)
		{
			num3 -= weights[j];
			if (num3 < 0)
			{
				return j;
			}
		}
		throw new Exception("Error Params");
	}

	public static int GetRandomIndex<T>(IReadOnlyList<T> weights, IRandomSource random) where T : ITuple
	{
		int num = 0;
		T val;
		foreach (T weight in weights)
		{
			T current = weight;
			ref T reference = ref current;
			ref T reference2 = ref reference;
			val = default(T);
			if (val == null)
			{
				val = reference2;
				reference2 = ref val;
			}
			int index = reference.Length - 1;
			if (reference2[index] is short num2)
			{
				num += num2;
				continue;
			}
			throw new Exception("The last element of each tuple must be a short value");
		}
		int num3 = random.Next(0, num);
		for (int i = 0; i < weights.Count; i++)
		{
			int num4 = num3;
			val = weights[i];
			ref T reference = ref val;
			num3 = num4 - ((reference[reference.Length - 1] is short num5) ? num5 : 0);
			if (num3 < 0)
			{
				return i;
			}
		}
		throw new Exception("Error Params");
	}

	public static int GetRandomIndex<T>(IReadOnlyList<(T, short)> weights, IRandomSource random)
	{
		int num = 0;
		foreach (var weight in weights)
		{
			num += weight.Item2;
		}
		int num2 = random.Next(0, num);
		for (int i = 0; i < weights.Count; i++)
		{
			num2 -= weights[i].Item2;
			if (num2 < 0)
			{
				return i;
			}
		}
		throw new Exception("Error Params");
	}

	public static void GenerateRandomWeightCellList(IRandomSource random, IList<short[]> core, int count, ref List<short[]> result, int weightIdx = 1)
	{
		GenerateRandomList(count, core.Select((short[] a) => (a: a, a[weightIdx])), random, ref result);
	}

	public static void GenerateRandomList<T>(int count, IEnumerable<(T, short)> paras, IRandomSource random, ref List<T> result)
	{
		result.Clear();
		paras = paras.Where(((T, short) a) => a.Item2 != 0);
		float num = 0f;
		float num2 = 0f;
		foreach (var para in paras)
		{
			num += (float)para.Item2;
		}
		float num3 = num / (float)count;
		List<(T, short)> list = new List<(T, short)>(paras);
		while (list.Count > 0 && result.Count < count)
		{
			int index = random.Next(0, list.Count);
			(T, short) tuple = list[index];
			list.RemoveAt(index);
			short item = tuple.Item2;
			num2 += (float)count * ((float)item / num);
			int num4 = (int)Math.Round(num2) - result.Count;
			if (num4 == 0 && (float)item < num3)
			{
				if (RandomCheck(random, (float)item / num))
				{
					result.Add(tuple.Item1);
				}
			}
			else
			{
				for (int num5 = 0; num5 < num4; num5++)
				{
					result.Add(tuple.Item1);
				}
			}
		}
		CollectionUtils.Shuffle(random, result);
	}

	public static bool RandomCheck(IRandomSource random, float rate, float totalRate = 1f)
	{
		return random.Next(0, (int)(totalRate * 1000f) + 1) <= (int)(rate * 1000f);
	}

	public unsafe static void GetRandomUnrepeated(IRandomSource random, int minValue, int maxValue, int* resultIndices, int amount)
	{
		Span<int> span = new Span<int>(resultIndices, amount);
		span.Fill(-1);
		int num = 0;
		int num2 = maxValue - minValue + 1;
		while (num < amount && num < num2)
		{
			int num3 = random.Next(minValue, maxValue + 1);
			if (span.IndexOf(num3) < 0)
			{
				span[num] = num3;
				num++;
			}
		}
	}

	public static void GetRandomUnrepeated(IRandomSource random, int minValue, int maxValue, Span<int> resultIndices)
	{
		resultIndices.Fill(-1);
		int num = 0;
		int num2 = maxValue - minValue + 1;
		while (num < resultIndices.Length && num < num2)
		{
			int num3 = random.Next(minValue, maxValue + 1);
			if (resultIndices.IndexOf(num3) < 0)
			{
				resultIndices[num] = num3;
				num++;
			}
		}
	}

	public static IEnumerable<T> GetRandomUnrepeated<T>(IRandomSource random, int maxCount, [DisallowNull] IReadOnlyList<T> preferredItems, [AllowNull] IReadOnlyList<T> normalItems = null)
	{
		int preferredCount = Math.Min(maxCount, preferredItems.Count);
		maxCount -= preferredCount;
		for (int i = 0; i < preferredItems.Count; i++)
		{
			if (random.CheckProb(preferredCount, preferredItems.Count - i))
			{
				yield return preferredItems[i];
				preferredCount--;
			}
		}
		if (normalItems == null)
		{
			yield break;
		}
		int normalCount = Math.Min(maxCount - preferredCount, normalItems.Count);
		for (int i = 0; i < normalItems.Count; i++)
		{
			if (random.CheckProb(normalCount, normalItems.Count - i))
			{
				yield return normalItems[i];
				normalCount--;
			}
		}
	}

	public static IEnumerable<ulong> GetRandomUnrepeated(IRandomSource random, ulong maxCount)
	{
		switch (maxCount)
		{
		case 1uL:
			yield return 0uL;
			yield break;
		case 0uL:
			yield break;
		}
		ulong modFlag = maxCount | (maxCount >> 1);
		modFlag |= modFlag >> 2;
		modFlag |= modFlag >> 4;
		modFlag |= modFlag >> 8;
		modFlag |= modFlag >> 16;
		modFlag |= modFlag >> 32;
		ulong mul = (1 | ((ulong)((double)modFlag * (random.NextDouble() * 0.3 + 0.2)) << 2)) & modFlag;
		ulong add = (((ulong)((double)maxCount * random.NextDouble()) << 1) | 1) & modFlag;
		ulong scout = (ulong)((double)maxCount * random.NextDouble()) & modFlag;
		if (scout < maxCount)
		{
			yield return scout;
		}
		for (ulong curr = (scout * mul + add) & modFlag; curr != scout; curr = (curr * mul + add) & modFlag)
		{
			if (curr < maxCount)
			{
				yield return curr;
			}
		}
	}

	public static IEnumerable<(int, int)> GetRandomUnrepeatedIntPair(IRandomSource random, int max)
	{
		return GetRandomUnrepeated(random, (ulong)((long)max * (long)(max - 1)) >> 1).Select(DecomposeTriangleNumber);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static (int, int) DecomposeTriangleNumber(ulong index)
	{
		int num = (int)Math.Sqrt(index * 2);
		int num2 = (int)((long)index - (num * ((long)num - 1L) >>> 1));
		if (num2 < num)
		{
			return (num2, num);
		}
		num2 -= num;
		num++;
		return (num2, num);
	}

	public static int[] DistributeNIntoKBuckets(IRandomSource random, int n, int k)
	{
		int[] array = new int[k];
		for (int i = 0; i < k - 1; i++)
		{
			array[i] = random.Next(0, n + 1);
			n -= array[i];
		}
		array[k - 1] = n;
		return array;
	}
}
