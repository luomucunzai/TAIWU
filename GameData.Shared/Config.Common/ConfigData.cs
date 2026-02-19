using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using GameData.Utilities;

namespace Config.Common;

[Serializable]
public abstract class ConfigData<T, TKey> : IConfigData, IReadOnlyList<T>, IEnumerable<T>, IEnumerable, IReadOnlyCollection<T> where T : ConfigItem<T, TKey>
{
	protected readonly Dictionary<string, int> _refNameMap = new Dictionary<string, int>();

	protected List<T> _dataArray;

	protected readonly Dictionary<int, T> _extraDataMap = new Dictionary<int, T>();

	public IReadOnlyDictionary<string, int> RefNameMap => _refNameMap;

	public T this[int index] => GetItem(ToTemplateId(index));

	public T this[TKey index] => GetItem(index);

	public T this[string refName] => GetItem(ToTemplateId(_refNameMap[refName]));

	public int Count => _dataArray?.Count ?? 0;

	public int CountWithExtra => Count + _extraDataMap.Count;

	internal abstract int ToInt(TKey value);

	internal abstract TKey ToTemplateId(int value);

	public virtual void Init()
	{
		_refNameMap.Clear();
		_refNameMap.Load(GetType().Name);
		_extraDataMap.Clear();
	}

	public int GetItemId(string refName)
	{
		if (_refNameMap.TryGetValue(refName, out var value))
		{
			return value;
		}
		throw new Exception(refName + " not found.");
	}

	public int AddExtraItem(string identifier, string refName, object configItem)
	{
		T val = (T)configItem;
		int templateId = val.GetTemplateId();
		if (templateId < _dataArray.Count)
		{
			throw new Exception($"CharacterFeature template id {templateId} created by {identifier} already exist.");
		}
		if (_extraDataMap.ContainsKey(templateId))
		{
			throw new Exception($"CharacterFeature extra template id {templateId} created by {identifier} already exist.");
		}
		if (_refNameMap.TryGetValue(refName, out var value))
		{
			throw new Exception($"CharacterFeature template reference name {refName}(id = {templateId}) created by {identifier} already exist with templateId {value}).");
		}
		_refNameMap.Add(refName, templateId);
		_extraDataMap.Add(templateId, val);
		return templateId;
	}

	public int AddOrModifyItem(T configItem)
	{
		int templateId = configItem.GetTemplateId();
		if (templateId < _dataArray.Count)
		{
			_dataArray[templateId] = configItem;
			return templateId;
		}
		if (templateId == -1)
		{
			_dataArray.Add(configItem.Duplicate(_dataArray.Count));
			return _dataArray.Count - 1;
		}
		throw new Exception($"template id {templateId} in {configItem} exceeds _dataArray.Count = {_dataArray.Count}, please use -1 instead.");
	}

	public T GetItem(TKey id)
	{
		int num = ToInt(id);
		if (num < 0)
		{
			return null;
		}
		if (num < _dataArray.Count)
		{
			return _dataArray[num];
		}
		if (_extraDataMap.TryGetValue(num, out var value))
		{
			return value;
		}
		AdaptableLog.TagWarning(GetType().FullName, $"index {id} is not in range [0, {_dataArray.Count}) and is not defined in _extraDataMap (count: {_extraDataMap.Count})");
		return null;
	}

	public List<TKey> GetAllKeys()
	{
		return this.Select((T item) => ToTemplateId(item.GetTemplateId())).ToList();
	}

	public void Iterate(Func<T, bool> iterateFunc)
	{
		if (iterateFunc == null)
		{
			return;
		}
		foreach (T item in (IEnumerable<T>)this)
		{
			if (!iterateFunc(item))
			{
				break;
			}
		}
	}

	IEnumerator<T> IEnumerable<T>.GetEnumerator()
	{
		foreach (T item in _dataArray)
		{
			yield return item;
		}
		foreach (T value in _extraDataMap.Values)
		{
			yield return value;
		}
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		return ((IEnumerable<T>)this).GetEnumerator();
	}
}
