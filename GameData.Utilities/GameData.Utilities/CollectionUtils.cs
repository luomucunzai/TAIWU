using System;
using System.Collections.Generic;
using Redzen.Random;

namespace GameData.Utilities;

public static class CollectionUtils
{
	public unsafe static void SetMemoryToZero(byte* pDest, int count)
	{
		int i = 0;
		for (int num = count / 8; i < num; i++)
		{
			*(long*)pDest = 0L;
			pDest += 8;
		}
		int j = 0;
		for (int num2 = count % 8; j < num2; j++)
		{
			pDest[j] = 0;
		}
	}

	public unsafe static void SetMemoryToMinusOne(byte* pDest, int count)
	{
		int i = 0;
		for (int num = count / 8; i < num; i++)
		{
			*(long*)pDest = -1L;
			pDest += 8;
		}
		int j = 0;
		for (int num2 = count % 8; j < num2; j++)
		{
			pDest[j] = byte.MaxValue;
		}
	}

	public unsafe static bool Equals(byte* pLhs, byte* pRhs, int count)
	{
		int i = 0;
		for (int num = count / 8; i < num; i++)
		{
			if (*(long*)pLhs != *(long*)pRhs)
			{
				return false;
			}
			pLhs += 8;
			pRhs += 8;
		}
		int j = 0;
		for (int num2 = count % 8; j < num2; j++)
		{
			if (pLhs[j] != pRhs[j])
			{
				return false;
			}
		}
		return true;
	}

	public static bool Equals<T>(T[] lhs, T[] rhs, int count) where T : IEquatable<T>
	{
		for (int i = 0; i < count; i++)
		{
			if (!lhs[i].Equals(rhs[i]))
			{
				return false;
			}
		}
		return true;
	}

	public unsafe static bool Contains<T>(T* pCollection, int count, T item) where T : unmanaged, IEquatable<T>
	{
		for (int i = 0; i < count; i++)
		{
			if (item.Equals(pCollection[i]))
			{
				return true;
			}
		}
		return false;
	}

	public static bool Contains<T>(T[] collection, T item) where T : IEquatable<T>
	{
		int i = 0;
		for (int num = collection.Length; i < num; i++)
		{
			if (item.Equals(collection[i]))
			{
				return true;
			}
		}
		return false;
	}

	public static T ElementAt<T>(HashSet<T> collection, int index)
	{
		using (HashSet<T>.Enumerator enumerator = collection.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				if (index <= 0)
				{
					return enumerator.Current;
				}
				index--;
			}
		}
		throw new ArgumentOutOfRangeException();
	}

	public static int BinarySearch(IList<long> list, int index, int count, long value)
	{
		int num = index;
		int num2 = index + count - 1;
		while (num <= num2)
		{
			int num3 = num + (num2 - num >> 1);
			long num4 = list[num3] - value;
			if (num4 == 0L)
			{
				return num3;
			}
			if (num4 < 0)
			{
				num = num3 + 1;
			}
			else
			{
				num2 = num3 - 1;
			}
		}
		return ~num;
	}

	public unsafe static int BinarySearch(short* pList, int index, int count, int value)
	{
		int num = index;
		int num2 = index + count - 1;
		while (num <= num2)
		{
			int num3 = num + (num2 - num >> 1);
			int num4 = pList[num3] - value;
			if (num4 == 0)
			{
				return num3;
			}
			if (num4 < 0)
			{
				num = num3 + 1;
			}
			else
			{
				num2 = num3 - 1;
			}
		}
		return ~num;
	}

	public unsafe static int BinarySearch(int* pList, int index, int count, int value)
	{
		int num = index;
		int num2 = index + count - 1;
		while (num <= num2)
		{
			int num3 = num + (num2 - num >> 1);
			int num4 = pList[num3] - value;
			if (num4 == 0)
			{
				return num3;
			}
			if (num4 < 0)
			{
				num = num3 + 1;
			}
			else
			{
				num2 = num3 - 1;
			}
		}
		return ~num;
	}

	public unsafe static int GetMaxIndex(int* pList, int count)
	{
		int result = 0;
		int num = *pList;
		for (int i = 1; i < count; i++)
		{
			int num2 = pList[i];
			if (num2 > num)
			{
				num = num2;
				result = i;
			}
		}
		return result;
	}

	public unsafe static int GetMaxIndex(short* pList, int count)
	{
		int result = 0;
		short num = *pList;
		for (int i = 1; i < count; i++)
		{
			short num2 = pList[i];
			if (num2 > num)
			{
				num = num2;
				result = i;
			}
		}
		return result;
	}

	public static int GetMaxIndex(Span<int> pList)
	{
		int result = 0;
		int num = pList[0];
		for (int i = 1; i < pList.Length; i++)
		{
			int num2 = pList[i];
			if (num2 > num)
			{
				num = num2;
				result = i;
			}
		}
		return result;
	}

	public unsafe static int GetMinIndex(int* pList, int count)
	{
		int result = 0;
		int num = *pList;
		for (int i = 1; i < count; i++)
		{
			int num2 = pList[i];
			if (num2 < num)
			{
				num = num2;
				result = i;
			}
		}
		return result;
	}

	public unsafe static int GetMinIndex(short* pList, int count)
	{
		int result = 0;
		short num = *pList;
		for (int i = 1; i < count; i++)
		{
			short num2 = pList[i];
			if (num2 < num)
			{
				num = num2;
				result = i;
			}
		}
		return result;
	}

	public unsafe static int GetSum(int* pList, int count)
	{
		int num = 0;
		for (int i = 0; i < count; i++)
		{
			num += pList[i];
		}
		return num;
	}

	public unsafe static int GetSum(short* pList, int count)
	{
		int num = 0;
		for (int i = 0; i < count; i++)
		{
			num += pList[i];
		}
		return num;
	}

	public unsafe static void Sort(long* pList, int count)
	{
		if (count <= 30)
		{
			BubbleSort(pList, count);
		}
		QuickSort(pList, 0, count - 1);
	}

	private unsafe static void BubbleSort(long* pList, int count)
	{
		for (int i = 0; i < count - 1; i++)
		{
			for (int j = 0; j < count - i - 1; j++)
			{
				if (pList[j] > pList[j + 1])
				{
					ref long reference = ref pList[j];
					long* num = pList + (j + 1);
					long num2 = pList[j + 1];
					long num3 = pList[j];
					reference = num2;
					*num = num3;
				}
			}
		}
	}

	private unsafe static void QuickSort(long* arr, int low, int high)
	{
		if (low < high)
		{
			int num = Partition(arr, low, high);
			QuickSort(arr, low, num - 1);
			QuickSort(arr, num + 1, high);
		}
	}

	private unsafe static int Partition(long* arr, int low, int high)
	{
		long num = arr[high];
		int num2 = low - 1;
		ref long reference;
		long num5;
		long num4;
		for (int i = low; i <= high - 1; i++)
		{
			if (arr[i] < num)
			{
				num2++;
				reference = ref arr[num2];
				long* num3 = arr + i;
				num4 = arr[i];
				num5 = arr[num2];
				reference = num4;
				*num3 = num5;
			}
		}
		reference = ref arr[num2 + 1];
		long* num6 = arr + high;
		num5 = arr[high];
		num4 = arr[num2 + 1];
		reference = num5;
		*num6 = num4;
		return num2 + 1;
	}

	public unsafe static void Sort(int* pList, int count)
	{
		if (count <= 30)
		{
			BubbleSort(pList, count);
		}
		QuickSort(pList, 0, count - 1);
	}

	private unsafe static void BubbleSort(int* pList, int count)
	{
		for (int i = 0; i < count - 1; i++)
		{
			for (int j = 0; j < count - i - 1; j++)
			{
				if (pList[j] > pList[j + 1])
				{
					ref int reference = ref pList[j];
					int* num = pList + (j + 1);
					int num2 = pList[j + 1];
					int num3 = pList[j];
					reference = num2;
					*num = num3;
				}
			}
		}
	}

	private unsafe static void QuickSort(int* arr, int low, int high)
	{
		if (low < high)
		{
			int num = Partition(arr, low, high);
			QuickSort(arr, low, num - 1);
			QuickSort(arr, num + 1, high);
		}
	}

	private unsafe static int Partition(int* arr, int low, int high)
	{
		int num = arr[high];
		int num2 = low - 1;
		ref int reference;
		int num5;
		int num4;
		for (int i = low; i <= high - 1; i++)
		{
			if (arr[i] < num)
			{
				num2++;
				reference = ref arr[num2];
				int* num3 = arr + i;
				num4 = arr[i];
				num5 = arr[num2];
				reference = num4;
				*num3 = num5;
			}
		}
		reference = ref arr[num2 + 1];
		int* num6 = arr + high;
		num5 = arr[high];
		num4 = arr[num2 + 1];
		reference = num5;
		*num6 = num4;
		return num2 + 1;
	}

	public static void Sort<T>(IList<T> pList, Func<T, T, int> compareTo)
	{
		if (pList.Count <= 30)
		{
			BubbleSort(pList, pList.Count, compareTo);
		}
		QuickSort(pList, 0, pList.Count - 1, compareTo);
	}

	private static void BubbleSort<T>(IList<T> pList, int count, Func<T, T, int> compareTo)
	{
		for (int i = 0; i < count - 1; i++)
		{
			for (int j = 0; j < count - i - 1; j++)
			{
				if (compareTo(pList[j], pList[j + 1]) > 0)
				{
					int index = j;
					int index2 = j + 1;
					T value = pList[j + 1];
					T value2 = pList[j];
					pList[index] = value;
					pList[index2] = value2;
				}
			}
		}
	}

	private static void QuickSort<T>(IList<T> arr, int low, int high, Func<T, T, int> compareTo)
	{
		if (low < high)
		{
			int num = Partition(arr, low, high, compareTo);
			QuickSort(arr, low, num - 1, compareTo);
			QuickSort(arr, num + 1, high, compareTo);
		}
	}

	private static int Partition<T>(IList<T> arr, int low, int high, Func<T, T, int> compareTo)
	{
		T arg = arr[high];
		int num = low - 1;
		IList<T> list;
		int index2;
		int index;
		T value2;
		T value;
		for (int i = low; i <= high - 1; i++)
		{
			if (compareTo(arr[i], arg) < 0)
			{
				num++;
				list = arr;
				index = num;
				index2 = i;
				value = arr[i];
				value2 = arr[num];
				list[index] = value;
				arr[index2] = value2;
			}
		}
		list = arr;
		index2 = num + 1;
		index = high;
		value2 = arr[high];
		value = arr[num + 1];
		list[index2] = value2;
		arr[index] = value;
		return num + 1;
	}

	public unsafe static void Shuffle<T>(IRandomSource random, T* pArray, int arrayCount) where T : unmanaged
	{
		for (int num = arrayCount - 1; num > 0; num--)
		{
			int num2 = random.Next(num + 1);
			ref T reference = ref pArray[num2];
			T* num3 = pArray + num;
			T val = pArray[num];
			T val2 = pArray[num2];
			reference = val;
			*num3 = val2;
		}
	}

	public static void Shuffle<T>(IRandomSource random, Span<T> pArray, int arrayCount) where T : unmanaged
	{
		for (int num = arrayCount - 1; num > 0; num--)
		{
			int index = random.Next(num + 1);
			ref T reference = ref pArray[index];
			ref T reference2 = ref pArray[num];
			T val = pArray[num];
			T val2 = pArray[index];
			reference = val;
			reference2 = val2;
		}
	}

	public static void Shuffle<T>(IRandomSource random, T[] array)
	{
		for (int num = array.Length - 1; num > 0; num--)
		{
			int num2 = random.Next(num + 1);
			int num3 = num2;
			int num4 = num;
			T val = array[num];
			T val2 = array[num2];
			array[num3] = val;
			array[num4] = val2;
		}
	}

	public static void Shuffle<T>(IRandomSource random, List<T> list)
	{
		for (int num = list.Count - 1; num > 0; num--)
		{
			int num2 = random.Next(num + 1);
			int index = num2;
			int index2 = num;
			T value = list[num];
			T value2 = list[num2];
			list[index] = value;
			list[index2] = value2;
		}
	}

	public static void Shuffle<T>(IRandomSource random, IList<T> list)
	{
		for (int num = list.Count - 1; num > 0; num--)
		{
			int num2 = random.Next(num + 1);
			int index = num2;
			int index2 = num;
			T value = list[num];
			T value2 = list[num2];
			list[index] = value;
			list[index2] = value2;
		}
	}

	public static void Shuffle<T>(IRandomSource random, SpanList<T> list) where T : unmanaged
	{
		for (int num = list.Count - 1; num > 0; num--)
		{
			int index = random.Next(num + 1);
			ref T reference = ref list[index];
			ref T reference2 = ref list[num];
			T val = list[num];
			T val2 = list[index];
			reference = val;
			reference2 = val2;
		}
	}

	public unsafe static T* Shuffle<T>(IRandomSource random, T* pArray, int arrayCount, int shuffleCount) where T : unmanaged
	{
		for (int num = arrayCount - 1; num > arrayCount - 1 - shuffleCount; num--)
		{
			int num2 = random.Next(num + 1);
			ref T reference = ref pArray[num2];
			T* num3 = pArray + num;
			T val = pArray[num];
			T val2 = pArray[num2];
			reference = val;
			*num3 = val2;
		}
		return pArray + arrayCount - shuffleCount;
	}

	public static int Shuffle<T>(IRandomSource random, T[] array, int shuffleCount)
	{
		int num = array.Length;
		for (int num2 = num - 1; num2 > num - 1 - shuffleCount; num2--)
		{
			int num3 = random.Next(num2 + 1);
			int num4 = num3;
			int num5 = num2;
			T val = array[num2];
			T val2 = array[num3];
			array[num4] = val;
			array[num5] = val2;
		}
		return num - shuffleCount;
	}

	public static int Shuffle<T>(IRandomSource random, List<T> list, int shuffleCount)
	{
		int count = list.Count;
		for (int num = count - 1; num > count - 1 - shuffleCount; num--)
		{
			int num2 = random.Next(num + 1);
			int index = num2;
			int index2 = num;
			T value = list[num];
			T value2 = list[num2];
			list[index] = value;
			list[index2] = value2;
		}
		return count - shuffleCount;
	}

	public unsafe static T GetRandomWeightedElement<T>(IRandomSource random, (T value, short weight)* weights, int elementCount) where T : unmanaged
	{
		int num = 0;
		for (int i = 0; i < elementCount; i++)
		{
			num += weights[i].Item2;
		}
		int num2 = random.Next(num);
		for (int j = 0; j < elementCount; j++)
		{
			short item = weights[j].Item2;
			if (num2 < item)
			{
				return weights[j].Item1;
			}
			num2 -= item;
		}
		return weights[elementCount - 1].Item1;
	}

	public unsafe static int GetRandomWeightedElement(IRandomSource random, sbyte* pWeightedElements, int elementsCount, int totalWeight)
	{
		int num = random.Next(totalWeight);
		for (int i = 0; i < elementsCount; i++)
		{
			sbyte b = pWeightedElements[i];
			if (num < b)
			{
				return i;
			}
			num -= b;
		}
		return elementsCount;
	}

	public static int GetRandomWeightedElement(IRandomSource random, Span<int> pWeightedElements)
	{
		if (pWeightedElements.Length <= 0)
		{
			return -1;
		}
		int num = 0;
		Span<int> span = pWeightedElements;
		for (int i = 0; i < span.Length; i++)
		{
			int num2 = span[i];
			num += num2;
		}
		if (num == 0)
		{
			return random.Next(pWeightedElements.Length);
		}
		int num3 = random.Next(num);
		for (int j = 0; j < pWeightedElements.Length; j++)
		{
			int num4 = pWeightedElements[j];
			if (num3 < num4)
			{
				return j;
			}
			num3 -= num4;
		}
		return pWeightedElements.Length;
	}

	public unsafe static int GetRandomAccumulativeWeightedElement(IRandomSource random, int* pAccumulativeWeightedElements, int elementsCount)
	{
		int num = pAccumulativeWeightedElements[elementsCount - 1];
		int num2 = 1 + random.Next(num);
		int num3 = BinarySearch(pAccumulativeWeightedElements, 0, elementsCount, num2);
		if (num3 < 0)
		{
			return ~num3;
		}
		while (num3 - 1 >= 0 && pAccumulativeWeightedElements[num3 - 1] == num2)
		{
			num3--;
		}
		return num3;
	}

	public static void MoveIndexToFirst<T>(this IList<T> list, int index)
	{
		if (list == null)
		{
			throw new ArgumentNullException("list");
		}
		if (index < 0 || index >= list.Count)
		{
			throw new ArgumentOutOfRangeException("index", index, $"count={list.Count}");
		}
		if (index != 0)
		{
			T value = list[index];
			for (int num = index; num > 0; num--)
			{
				list[num] = list[num - 1];
			}
			list[0] = value;
		}
	}

	public static void MoveLastToFirst<T>(this IList<T> list, int count)
	{
		if (list == null)
		{
			throw new ArgumentNullException("list");
		}
		if (count < 0 || count > list.Count)
		{
			throw new ArgumentOutOfRangeException("count", count, $"count={list.Count}");
		}
		if (count != 0 && count != list.Count)
		{
			list.Reverse();
			list.Reverse(0, count);
			list.Reverse(count, list.Count - count);
		}
	}

	public static void Reverse<T>(this IList<T> list)
	{
		list.Reverse(0, list.Count);
	}

	public static void Reverse<T>(this IList<T> list, int startIndex, int count)
	{
		if (list == null)
		{
			throw new ArgumentNullException("list");
		}
		if (startIndex < 0 || startIndex >= list.Count)
		{
			throw new ArgumentOutOfRangeException("startIndex", startIndex, $"count={list.Count}");
		}
		if (count < 0 || startIndex + count > list.Count)
		{
			throw new ArgumentOutOfRangeException("count", count, $"count={list.Count} index={startIndex}");
		}
		for (int i = 0; i < count / 2; i++)
		{
			int index = startIndex + i;
			int index2 = startIndex + count - i - 1;
			T value = list[startIndex + count - i - 1];
			T value2 = list[startIndex + i];
			list[index] = value;
			list[index2] = value2;
		}
	}

	public static int TryInsertTopK<T>(this ref SpanList<(T, int worth)> topK, int k, T element, int worth) where T : unmanaged
	{
		if (topK.Count > 0)
		{
			if (topK.Count == k)
			{
				if (topK[topK.Count - 1].worth >= worth)
				{
					return -1;
				}
			}
			for (int num = topK.Count - 1; num >= 0; num--)
			{
				if (topK[num].worth >= worth)
				{
					if (topK.Count == k)
					{
						topK.RemoveAt(k - 1);
					}
					int num2 = num + 1;
					if (num2 < topK.Count)
					{
						topK.Insert(num2, (element, worth));
					}
					else
					{
						topK.Add((element, worth));
					}
					return num2;
				}
			}
			if (topK.Count == k)
			{
				topK.RemoveAt(k - 1);
			}
			if (topK.Count == 0)
			{
				topK.Add((element, worth));
			}
			else
			{
				topK.Insert(0, (element, worth));
			}
			return 0;
		}
		topK.Add((element, worth));
		return topK.Count - 1;
	}

	public static void SwapAndRemove<T>(List<T> list, int index)
	{
		int index2 = list.Count - 1;
		list[index] = list[index2];
		list.RemoveAt(index2);
	}

	public static void RemoveDuplicateKeys<TKey, TValue1, TValue2>(this IDictionary<TKey, TValue1> destination, IReadOnlyDictionary<TKey, TValue2> source, IList<TKey> cache = null)
	{
		if (destination == null || destination.Count <= 0 || source == null || source.Count <= 0)
		{
			return;
		}
		if (cache == null)
		{
			cache = new List<TKey>();
		}
		cache.Clear();
		foreach (TKey key in destination.Keys)
		{
			if (source.ContainsKey(key))
			{
				cache.Add(key);
			}
		}
		foreach (TKey item in cache)
		{
			destination.Remove(item);
		}
	}

	public unsafe static void CopyTo<T>(HashSet<T> collection, T* pMemory) where T : unmanaged
	{
		int num = 0;
		foreach (T item in collection)
		{
			pMemory[num++] = item;
		}
	}

	public static TValue GetOrNew<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key) where TValue : class, new()
	{
		if (dictionary.TryGetValue(key, out var value))
		{
			return value;
		}
		return dictionary[key] = new TValue();
	}

	public static TValue GetOrDefault<TKey, TValue>(this IReadOnlyDictionary<TKey, TValue> dictionary, TKey key)
	{
		return dictionary.GetValueOrDefault(key);
	}

	public static TValue GetOrDefault<TKey, TValue>(this IReadOnlyDictionary<TKey, TValue> dictionary, TKey key, TValue defaultValue)
	{
		return dictionary.GetValueOrDefault(key, defaultValue);
	}

	public static T GetOrDefault<T>(this IReadOnlyList<T> list, int index)
	{
		return list.GetOrDefault(index, default(T));
	}

	public static T GetOrDefault<T>(this IReadOnlyList<T> list, int index, T defaultValue)
	{
		if (index >= 0 && index < list.Count)
		{
			return list[index];
		}
		return defaultValue;
	}

	public static T GetClampedIndexValue<T>(this IReadOnlyList<T> list, int index)
	{
		if (list == null || list.Count <= 0)
		{
			throw new ArgumentException("The list is empty or null.");
		}
		index = MathUtils.Clamp(index, 0, list.Count - 1);
		return list[index];
	}

	public static T GetClampedIndexValueWithWarning<T>(this IReadOnlyList<T> list, int index, string warningMessage)
	{
		T clampedIndexValue = list.GetClampedIndexValue(index);
		if (index < 0 || index >= list.Count)
		{
			AdaptableLog.Warning($"Index {index} out of range 0~{list.Count} by fallback {warningMessage}", appendWarningMessage: true);
		}
		return clampedIndexValue;
	}

	public static T GetOrLast<T>(this IReadOnlyList<T> list, int index)
	{
		if (list == null || list.Count == 0)
		{
			throw new ArgumentException("The list is empty or null.");
		}
		if (index >= list.Count)
		{
			return list[list.Count - 1];
		}
		return list[index];
	}

	public static bool All<T>(this IEnumerable<T> collection, T item) where T : IEquatable<T>
	{
		foreach (T item2 in collection)
		{
			if (!item2.Equals(item))
			{
				return false;
			}
		}
		return true;
	}

	public static void ClearAndAddRange<T>(this List<T> list, IEnumerable<T> append)
	{
		list.Clear();
		list.AddRange(append);
	}

	public static T SetOrAdd<T>(this IList<T> list, int index, T value, T defaultValue = default(T))
	{
		for (int i = list.Count; i < index; i++)
		{
			list.Add(defaultValue);
		}
		if (list.CheckIndex(index))
		{
			list[index] = value;
		}
		else
		{
			list.Add(value);
		}
		return value;
	}
}
