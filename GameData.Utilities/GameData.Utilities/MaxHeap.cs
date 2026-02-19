using System;
using System.Collections.Generic;

namespace GameData.Utilities;

public class MaxHeap<T> where T : IComparable<T>
{
	private T[] _array;

	private int _count;

	public int Count => _count;

	public MaxHeap(int initialCapacity = 4)
	{
		_array = new T[initialCapacity];
	}

	public MaxHeap(IList<T> initialArr)
	{
		_array = new T[initialArr.Count];
		initialArr.CopyTo(_array, 0);
		_count = initialArr.Count;
		Sort();
	}

	public void Push(T element)
	{
		int count = Count;
		EnsureCapacity(count + 1);
		_array[count] = element;
		_count++;
		count = BubbleUp();
		BubbleDown(count);
	}

	public void Remove(T element)
	{
		for (int i = 0; i < Count; i++)
		{
			if (_array[i].CompareTo(element) == 0)
			{
				_array[i] = _array[Count - 1];
				_count--;
				_array[Count - 1] = default(T);
				BubbleDown(i);
				break;
			}
		}
	}

	public T Pop()
	{
		T result = _array[0];
		int num = Count - 1;
		_count--;
		_array[0] = _array[num];
		_array[num] = default(T);
		BubbleDown(0);
		return result;
	}

	public T Peek()
	{
		return _array[0];
	}

	private int BubbleUp()
	{
		int num = Count - 1;
		while (num > 0)
		{
			int num2 = (num - 1) / 2;
			ref readonly T reference = ref _array[num];
			T val = default(T);
			if (val == null)
			{
				val = reference;
				reference = ref val;
			}
			T other = _array[num2];
			if (reference.CompareTo(other) <= 0)
			{
				return num;
			}
			T[] array = _array;
			int num3 = num;
			T[] array2 = _array;
			int num4 = num2;
			val = _array[num2];
			T val2 = _array[num];
			array[num3] = val;
			array2[num4] = val2;
			num = num2;
		}
		return num;
	}

	private void BubbleDown(int index)
	{
		int count = Count;
		while (true)
		{
			int num = index * 2 + 1;
			int num2 = num + 1;
			if (num >= count)
			{
				break;
			}
			int num3;
			T val;
			if (num2 >= count)
			{
				num3 = num;
			}
			else
			{
				ref readonly T reference = ref _array[num];
				val = default(T);
				if (val == null)
				{
					val = reference;
					reference = ref val;
				}
				T other = _array[num2];
				num3 = ((reference.CompareTo(other) <= 0) ? num2 : num);
			}
			ref readonly T reference2 = ref _array[index];
			val = default(T);
			if (val == null)
			{
				val = reference2;
				reference2 = ref val;
			}
			T other2 = _array[num3];
			if (reference2.CompareTo(other2) >= 0)
			{
				break;
			}
			T[] array = _array;
			int num4 = index;
			T[] array2 = _array;
			int num5 = num3;
			val = _array[num3];
			T val2 = _array[index];
			array[num4] = val;
			array2[num5] = val2;
			index = num3;
		}
	}

	private void Sort()
	{
		for (int num = Count / 2; num >= 0; num--)
		{
			BubbleDown(num);
		}
	}

	private void EnsureCapacity(int capacity)
	{
		if (capacity > _array.Length)
		{
			int num = _array.Length * 3 / 2 + 1;
			if (num < capacity)
			{
				num = capacity;
			}
			T[] array = new T[num];
			Array.Copy(_array, array, Count);
			_array = array;
		}
	}
}
