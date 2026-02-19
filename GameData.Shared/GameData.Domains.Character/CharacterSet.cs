using System.Collections.Generic;
using GameData.Serializer;
using GameData.Utilities;

namespace GameData.Domains.Character;

public struct CharacterSet : ISerializableGameData
{
	private static readonly HashSet<int> Empty = new HashSet<int>();

	private static readonly LocalObjectPool<HashSet<int>> LocalObjectPool = new LocalObjectPool<HashSet<int>>(51200, 51200);

	private HashSet<int> _collection;

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
			*(int*)ptr = _collection.Count;
			ptr += 4;
			foreach (int item in _collection)
			{
				*(int*)ptr = item;
				ptr += 4;
			}
			return (int)(ptr - pData);
		}
		*(int*)pData = 0;
		return 4;
	}

	public unsafe int Deserialize(byte* pData)
	{
		return Deserialize(pData, usePoolObject: true);
	}

	public unsafe int Deserialize(byte* pData, bool usePoolObject)
	{
		byte* ptr = pData;
		int num = *(int*)ptr;
		ptr += 4;
		if (num > 0)
		{
			if (_collection == null || _collection.Count == 0)
			{
				_collection = (usePoolObject ? LocalObjectPool.Get() : new HashSet<int>());
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

	public HashSet<int> GetCollection()
	{
		return _collection ?? Empty;
	}

	public int GetCount()
	{
		return _collection?.Count ?? 0;
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
		_collection.UnionWith(charIds);
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
