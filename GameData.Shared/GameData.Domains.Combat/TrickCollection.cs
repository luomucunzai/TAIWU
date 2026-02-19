using System.Collections.Generic;
using GameData.Serializer;
using GameData.Utilities;

namespace GameData.Domains.Combat;

public class TrickCollection : ISerializableGameData
{
	private int _nextTrickIndex;

	[SerializableGameDataField]
	private SortedDictionary<int, sbyte> _tricks = new SortedDictionary<int, sbyte>();

	[SerializableGameDataField]
	private List<int> _avoidTricks = new List<int>();

	public IReadOnlyDictionary<int, sbyte> Tricks => _tricks;

	public bool ContainsTrick(sbyte type)
	{
		return _tricks.ContainsKey(type);
	}

	public void ReplaceTrick(int index, sbyte type)
	{
		_tricks[index] = type;
	}

	public void AppendTrick(sbyte type, bool addByAvoid)
	{
		_tricks.Add(_nextTrickIndex, type);
		if (addByAvoid)
		{
			_avoidTricks.Add(_nextTrickIndex);
		}
		_nextTrickIndex++;
	}

	public void RemoveTrick(int trickIndex)
	{
		_tricks.Remove(trickIndex);
		_avoidTricks.Remove(trickIndex);
	}

	public void ClearTricks()
	{
		_tricks.Clear();
		_avoidTricks.Clear();
	}

	public bool IsAvoidTrick(int index)
	{
		return _avoidTricks.Contains(index);
	}

	public void RearrangeTrick(sbyte type)
	{
		List<int> list = ObjectPool<List<int>>.Instance.Get();
		List<sbyte> list2 = ObjectPool<List<sbyte>>.Instance.Get();
		List<bool> list3 = ObjectPool<List<bool>>.Instance.Get();
		List<bool> list4 = ObjectPool<List<bool>>.Instance.Get();
		foreach (var (item, b2) in _tricks)
		{
			list.Add(item);
			if (b2 == type)
			{
				list3.Add(_avoidTricks.Contains(item));
				continue;
			}
			list2.Add(b2);
			list4.Add(_avoidTricks.Contains(item));
		}
		_avoidTricks.Clear();
		for (int i = 0; i < list.Count; i++)
		{
			int num2 = list[list.Count - 1 - i];
			sbyte value = ((i < list2.Count) ? list2[list2.Count - 1 - i] : type);
			_tricks[num2] = value;
			if ((i < list4.Count) ? list4[list4.Count - 1 - i] : list3[list3.Count - 1 - (i - list4.Count)])
			{
				_avoidTricks.Add(num2);
			}
		}
		ObjectPool<List<int>>.Instance.Return(list);
		ObjectPool<List<sbyte>>.Instance.Return(list2);
		ObjectPool<List<bool>>.Instance.Return(list3);
		ObjectPool<List<bool>>.Instance.Return(list4);
	}

	public TrickCollection()
	{
	}

	public TrickCollection(TrickCollection other)
	{
		_tricks = ((other._tricks == null) ? null : new SortedDictionary<int, sbyte>(other._tricks));
		_avoidTricks = ((other._avoidTricks == null) ? null : new List<int>(other._avoidTricks));
	}

	public void Assign(TrickCollection other)
	{
		_tricks = ((other._tricks == null) ? null : new SortedDictionary<int, sbyte>(other._tricks));
		_avoidTricks = ((other._avoidTricks == null) ? null : new List<int>(other._avoidTricks));
	}

	public bool IsSerializedSizeFixed()
	{
		return false;
	}

	public int GetSerializedSize()
	{
		int num = 0;
		num += DictionaryOfBasicTypePair.GetSerializedSize<int, sbyte>((IReadOnlyDictionary<int, sbyte>)_tricks);
		num = ((_avoidTricks == null) ? (num + 2) : (num + (2 + 4 * _avoidTricks.Count)));
		if (num > 4)
		{
			return (num + 3) / 4 * 4;
		}
		return num;
	}

	public unsafe int Serialize(byte* pData)
	{
		byte* ptr = pData;
		ptr += DictionaryOfBasicTypePair.Serialize<int, sbyte, SortedDictionary<int, sbyte>>(ptr, ref _tricks);
		if (_avoidTricks != null)
		{
			int count = _avoidTricks.Count;
			Tester.Assert(count <= 65535);
			*(ushort*)ptr = (ushort)count;
			ptr += 2;
			for (int i = 0; i < count; i++)
			{
				((int*)ptr)[i] = _avoidTricks[i];
			}
			ptr += 4 * count;
		}
		else
		{
			*(short*)ptr = 0;
			ptr += 2;
		}
		int num = (int)(ptr - pData);
		if (num > 4)
		{
			return (num + 3) / 4 * 4;
		}
		return num;
	}

	public unsafe int Deserialize(byte* pData)
	{
		byte* ptr = pData;
		ptr += DictionaryOfBasicTypePair.Deserialize<int, sbyte, SortedDictionary<int, sbyte>>(ptr, ref _tricks);
		ushort num = *(ushort*)ptr;
		ptr += 2;
		if (num > 0)
		{
			if (_avoidTricks == null)
			{
				_avoidTricks = new List<int>(num);
			}
			else
			{
				_avoidTricks.Clear();
			}
			for (int i = 0; i < num; i++)
			{
				_avoidTricks.Add(((int*)ptr)[i]);
			}
			ptr += 4 * num;
		}
		else
		{
			_avoidTricks?.Clear();
		}
		int num2 = (int)(ptr - pData);
		if (num2 > 4)
		{
			return (num2 + 3) / 4 * 4;
		}
		return num2;
	}
}
