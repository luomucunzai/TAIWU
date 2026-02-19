using System;
using System.Collections.Generic;
using GameData.Utilities;

namespace GameData.Common;

public class DataModificationHandlerGroup
{
	private readonly Dictionary<string, int> _indices;

	private readonly List<(string key, Action<DataContext, DataUid> handler)> _handlers;

	private readonly List<Action<DataContext, DataUid>> _executionQueue;

	public int Count => _handlers.Count;

	public DataModificationHandlerGroup()
	{
		_indices = new Dictionary<string, int>();
		_handlers = new List<(string, Action<DataContext, DataUid>)>();
		_executionQueue = new List<Action<DataContext, DataUid>>();
	}

	public void RegisterHandler(string key, Action<DataContext, DataUid> handler)
	{
		int count = _handlers.Count;
		_indices.Add(key, count);
		_handlers.Add((key, handler));
	}

	public bool UnregisterHandler(string key)
	{
		if (!_indices.TryGetValue(key, out var value))
		{
			return false;
		}
		_indices.Remove(key);
		int num = _handlers.Count - 1;
		if (num != value)
		{
			string item = _handlers[num].key;
			CollectionUtils.SwapAndRemove(_handlers, value);
			_indices[item] = value;
		}
		else
		{
			_handlers.RemoveAt(value);
		}
		return true;
	}

	public void ExecuteAll(DataContext context, DataUid uid)
	{
		foreach (var (text, item) in _handlers)
		{
			_executionQueue.Add(item);
		}
		foreach (Action<DataContext, DataUid> item2 in _executionQueue)
		{
			item2(context, uid);
		}
		_executionQueue.Clear();
	}
}
