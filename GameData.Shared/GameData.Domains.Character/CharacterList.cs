using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using GameData.Serializer;
using GameData.Utilities;

namespace GameData.Domains.Character;

public struct CharacterList : ISerializableGameData
{
	private static readonly List<int> Empty = new List<int>();

	private static readonly LocalObjectPool<List<int>> LocalObjectPool = new LocalObjectPool<List<int>>(1024, 1024);

	private List<int> _collection;

	public int this[int index]
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			List<int> collection = GetCollection();
			if (index < 0 || index >= collection.Count)
			{
				throw new IndexOutOfRangeException($"index {index} is out of range [0,{collection.Count})");
			}
			return collection[index];
		}
	}

	public bool IsSerializedSizeFixed()
	{
		return false;
	}

	public int GetSerializedSize()
	{
		if (_collection == null)
		{
			return 4;
		}
		return 4 + 4 * _collection.Count;
	}

	public unsafe int Serialize(byte* pData)
	{
		if (_collection != null)
		{
			byte* ptr = pData;
			int num = (*(int*)ptr = _collection.Count);
			ptr += 4;
			for (int i = 0; i < num; i++)
			{
				*(int*)ptr = _collection[i];
				ptr += 4;
			}
			return (int)(ptr - pData);
		}
		*(int*)pData = 0;
		return 4;
	}

	public unsafe int Deserialize(byte* pData)
	{
		byte* ptr = pData;
		int num = *(int*)ptr;
		ptr += 4;
		if (num > 0)
		{
			if (_collection == null)
			{
				_collection = LocalObjectPool.Get();
			}
			else
			{
				_collection.Clear();
			}
			for (int i = 0; i < num; i++)
			{
				int item = *(int*)ptr;
				ptr += 4;
				_collection.Add(item);
			}
		}
		else
		{
			_collection?.Clear();
		}
		return (int)(ptr - pData);
	}

	public List<int> GetCollection()
	{
		return _collection ?? Empty;
	}

	public int GetCount()
	{
		return _collection?.Count ?? 0;
	}

	public int GetRealCount()
	{
		if (_collection == null)
		{
			return 0;
		}
		int num = 0;
		for (int i = 0; i < _collection.Count; i++)
		{
			if (_collection[i] >= 0)
			{
				num++;
			}
		}
		return num;
	}

	public bool Contains(int charId)
	{
		if (_collection != null)
		{
			return _collection.Contains(charId);
		}
		return false;
	}

	public bool Add(int charId)
	{
		bool result = false;
		if (_collection == null || _collection.Count == 0)
		{
			_collection = LocalObjectPool.Get();
			result = true;
		}
		_collection.Add(charId);
		return result;
	}

	public bool AddRange(IEnumerable<int> charIds)
	{
		bool result = false;
		if (_collection == null || _collection.Count == 0)
		{
			_collection = LocalObjectPool.Get();
			result = true;
		}
		_collection.AddRange(charIds);
		return result;
	}

	public (bool, bool) Remove(int charId)
	{
		if (_collection != null && _collection.Remove(charId))
		{
			if (_collection.Count <= 0)
			{
				LocalObjectPool.Return(_collection);
				_collection = null;
				return (true, true);
			}
			return (false, true);
		}
		return (false, false);
	}

	public bool Clear()
	{
		if (_collection != null)
		{
			_collection.Clear();
			LocalObjectPool.Return(_collection);
			_collection = null;
			return true;
		}
		return false;
	}
}
