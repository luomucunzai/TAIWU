using System;
using System.Collections.Generic;
using Redzen.Random;

namespace GameData.Utilities;

public static class Extensions
{
	public static T GetRandom<T>(this IList<T> list, IRandomSource random)
	{
		if (list == null || list.Count <= 0)
		{
			return default(T);
		}
		return list[random.Next(0, list.Count)];
	}

	public static T GetRandomOrDefault<T>(this IList<T> list, IRandomSource random, T defaultValue)
	{
		if (list == null || list.Count <= 0)
		{
			return defaultValue;
		}
		return list[random.Next(0, list.Count)];
	}

	public static T Min<T>(this IReadOnlyList<T> list) where T : IComparable<T>
	{
		if (list.Count > 0)
		{
			T val = list[0];
			for (int i = 1; i < list.Count; i++)
			{
				if (list[i].CompareTo(val) < 0)
				{
					val = list[i];
				}
			}
			return val;
		}
		return default(T);
	}

	public static T Max<T>(this IReadOnlyList<T> list) where T : IComparable<T>
	{
		if (list.Count > 0)
		{
			T val = list[0];
			for (int i = 1; i < list.Count; i++)
			{
				if (list[i].CompareTo(val) > 0)
				{
					val = list[i];
				}
			}
			return val;
		}
		return default(T);
	}

	public static T Max<T>(this IList<T> list, Comparison<T> comparison)
	{
		if (list.Count == 0)
		{
			return default(T);
		}
		T val = list[0];
		for (int i = 1; i < list.Count; i++)
		{
			if (comparison(list[i], val) > 0)
			{
				val = list[i];
			}
		}
		return val;
	}

	public static bool CheckIndex<T>(this IList<T> list, int index)
	{
		if (list == null)
		{
			return false;
		}
		if (index >= 0)
		{
			return index < list.Count;
		}
		return false;
	}

	public static int Count(this IList<byte> list, byte element)
	{
		int num = 0;
		for (int i = 0; i < list.Count; i++)
		{
			if (list[i] == element)
			{
				num++;
			}
		}
		return num;
	}

	public static void Assign<T>(this List<T> self, List<T> other)
	{
		int count = other.Count;
		if (self.Count != count)
		{
			self.Clear();
			self.AddRange(other);
			return;
		}
		for (int i = 0; i < count; i++)
		{
			self[i] = other[i];
		}
	}

	public static bool SequenceEqual<T>(this IReadOnlyList<T> self, IReadOnlyList<T> other) where T : IEquatable<T>
	{
		if (self.Count != other.Count)
		{
			return false;
		}
		for (int i = 0; i < self.Count; i++)
		{
			if (!self[i].Equals(other[i]))
			{
				return false;
			}
		}
		return true;
	}

	public static bool SequenceEqualWithNullParam<T>(this IReadOnlyList<T> self, IReadOnlyList<T> other) where T : IEquatable<T>
	{
		bool flag = self == null;
		bool flag2 = other == null;
		if (flag != flag2)
		{
			return false;
		}
		if (!flag)
		{
			return self.SequenceEqual(other);
		}
		return true;
	}

	public static int Sum(this IList<int> list)
	{
		int num = 0;
		for (int i = 0; i < list.Count; i++)
		{
			num += list[i];
		}
		return num;
	}

	public static bool Contains<T>(this SpanList<T> spanList, T element) where T : unmanaged, IEquatable<T>
	{
		for (int i = 0; i < spanList.Count; i++)
		{
			T val = spanList[i];
			if (val.Equals(element))
			{
				return true;
			}
		}
		return false;
	}

	public static bool Remove<T>(this ref SpanList<T> spanList, T element) where T : unmanaged, IEquatable<T>
	{
		for (int i = 0; i < spanList.Count; i++)
		{
			T val = spanList[i];
			if (val.Equals(element))
			{
				spanList.RemoveAt(i);
				return true;
			}
		}
		return false;
	}

	public static T Min<T>(this Span<T> span) where T : IComparable<T>
	{
		if (span.Length > 0)
		{
			T val = span[0];
			for (int i = 1; i < span.Length; i++)
			{
				if (span[i].CompareTo(val) < 0)
				{
					val = span[i];
				}
			}
			return val;
		}
		return default(T);
	}

	public static T Max<T>(this T[] span) where T : IComparable<T>
	{
		if (span.Length != 0)
		{
			T val = span[0];
			for (int i = 1; i < span.Length; i++)
			{
				if (span[i].CompareTo(val) > 0)
				{
					val = span[i];
				}
			}
			return val;
		}
		return default(T);
	}
}
