using System;
using System.Collections.Generic;

namespace GameData.Utilities;

public static class ArrayExtensions
{
	public static T[] ChangeArrType<TA, T>(this TA[] arrSrc, Func<TA, T> changeFunc = null)
	{
		T[] array = new T[arrSrc.Length];
		int i = 0;
		for (int num = arrSrc.Length; i < num; i++)
		{
			if (changeFunc != null)
			{
				array[i] = changeFunc(arrSrc[i]);
			}
			else
			{
				array[i] = (T)Convert.ChangeType(arrSrc[i], typeof(T));
			}
		}
		return array;
	}

	public static void ForEach<T>(this T[] array, Func<int, T, bool> action)
	{
		int i = 0;
		for (int num = array.Length; i < num && !action(i, array[i]); i++)
		{
		}
	}

	public static bool Exist<T>(this IReadOnlyList<T> array, Predicate<T> predicate)
	{
		if (predicate == null)
		{
			return false;
		}
		int i = 0;
		for (int count = array.Count; i < count; i++)
		{
			if (predicate(array[i]))
			{
				return true;
			}
		}
		return false;
	}

	public static bool Exist(this IReadOnlyList<sbyte> array, sbyte value)
	{
		int i = 0;
		for (int count = array.Count; i < count; i++)
		{
			if (array[i] == value)
			{
				return true;
			}
		}
		return false;
	}

	public static bool Exist(this IReadOnlyList<short> array, short value)
	{
		int i = 0;
		for (int count = array.Count; i < count; i++)
		{
			if (array[i] == value)
			{
				return true;
			}
		}
		return false;
	}

	public static bool Exist(this IReadOnlyList<ushort> array, ushort value)
	{
		int i = 0;
		for (int count = array.Count; i < count; i++)
		{
			if (array[i] == value)
			{
				return true;
			}
		}
		return false;
	}

	public static bool Exist(this IReadOnlyList<int> array, int value)
	{
		int i = 0;
		for (int count = array.Count; i < count; i++)
		{
			if (array[i] == value)
			{
				return true;
			}
		}
		return false;
	}

	public static bool Exist(this IReadOnlyList<bool> array, bool value)
	{
		int i = 0;
		for (int count = array.Count; i < count; i++)
		{
			if (array[i] == value)
			{
				return true;
			}
		}
		return false;
	}

	public static bool Exist<T>(this IReadOnlyList<T> array, T val) where T : IEquatable<T>
	{
		int i = 0;
		for (int count = array.Count; i < count; i++)
		{
			if (array[i].Equals(val))
			{
				return true;
			}
		}
		return false;
	}

	public static T Find<T>(this T[] array, Predicate<T> predicate)
	{
		if (predicate == null)
		{
			return default(T);
		}
		int i = 0;
		for (int num = array.Length; i < num; i++)
		{
			if (predicate(array[i]))
			{
				return array[i];
			}
		}
		return default(T);
	}

	public static List<T> FindAll<T>(this T[] array, Predicate<T> predicate)
	{
		if (predicate == null)
		{
			return null;
		}
		List<T> list = new List<T>();
		int i = 0;
		for (int num = array.Length; i < num; i++)
		{
			if (predicate(array[i]))
			{
				list.Add(array[i]);
			}
		}
		return list;
	}

	public static int CountAll<T>(this T[] array, Predicate<T> predicate)
	{
		int num = 0;
		if (predicate == null)
		{
			return num;
		}
		int i = 0;
		for (int num2 = array.Length; i < num2; i++)
		{
			if (predicate(array[i]))
			{
				num++;
			}
		}
		return num;
	}

	public static int IndexOf(this sbyte[] array, sbyte target)
	{
		int i = 0;
		for (int num = array.Length; i < num; i++)
		{
			if (array[i].Equals(target))
			{
				return i;
			}
		}
		return -1;
	}

	public static int IndexOf(this short[] array, short target)
	{
		int i = 0;
		for (int num = array.Length; i < num; i++)
		{
			if (array[i].Equals(target))
			{
				return i;
			}
		}
		return -1;
	}

	public static int IndexOf(this ushort[] array, ushort target)
	{
		int i = 0;
		for (int num = array.Length; i < num; i++)
		{
			if (array[i].Equals(target))
			{
				return i;
			}
		}
		return -1;
	}

	public static int IndexOf(this int[] array, int target)
	{
		int i = 0;
		for (int num = array.Length; i < num; i++)
		{
			if (array[i].Equals(target))
			{
				return i;
			}
		}
		return -1;
	}

	public static int IndexOf<T>(this T[] array, T target) where T : IEquatable<T>
	{
		int i = 0;
		for (int num = array.Length; i < num; i++)
		{
			if (array[i].Equals(target))
			{
				return i;
			}
		}
		return -1;
	}

	public static int IndexOf(this ArraySegmentList<short> array, short target)
	{
		int i = 0;
		for (int count = array.Count; i < count; i++)
		{
			if (array[i].Equals(target))
			{
				return i;
			}
		}
		return -1;
	}

	public static int Sum(this int[] array)
	{
		int num = 0;
		for (int i = 0; i < array.Length; i++)
		{
			num += array[i];
		}
		return num;
	}

	public static int Sum(this sbyte[] array)
	{
		int num = 0;
		for (int i = 0; i < array.Length; i++)
		{
			num += array[i];
		}
		return num;
	}

	public static int Sum(this byte[] array)
	{
		int num = 0;
		for (int i = 0; i < array.Length; i++)
		{
			num += array[i];
		}
		return num;
	}

	public static int Sum(this short[] array)
	{
		int num = 0;
		for (int i = 0; i < array.Length; i++)
		{
			num += array[i];
		}
		return num;
	}
}
