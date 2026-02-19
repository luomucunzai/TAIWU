using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Config.Common;
using GameData.Utilities;

namespace Config;

[Serializable]
public class SettlementTreasuryLimit : IEnumerable<SettlementTreasuryLimitItem>, IEnumerable, IConfigData
{
	public static class DefKey
	{
		public const sbyte Shaolin = 0;

		public const sbyte Emei = 1;

		public const sbyte Baihua = 2;

		public const sbyte Wudang = 3;

		public const sbyte Yuanshan = 4;

		public const sbyte Shixiang = 5;

		public const sbyte Ranshan = 6;

		public const sbyte Xuannv = 7;

		public const sbyte Zhujian = 8;

		public const sbyte Kongsang = 9;

		public const sbyte Jingang = 10;

		public const sbyte Wuxian = 11;

		public const sbyte Jieqing = 12;

		public const sbyte Fulong = 13;

		public const sbyte Xuehou = 14;

		public const sbyte Jingcheng = 15;

		public const sbyte Chengdu = 16;

		public const sbyte Guizhou = 17;

		public const sbyte Xiangyang = 18;

		public const sbyte Taiyuan = 19;

		public const sbyte Guangzhou = 20;

		public const sbyte Qingzhou = 21;

		public const sbyte Jiangling = 22;

		public const sbyte Fuzhou = 23;

		public const sbyte Liaoyang = 24;

		public const sbyte Qinzhou = 25;

		public const sbyte Dali = 26;

		public const sbyte Shouchun = 27;

		public const sbyte Hangzhou = 28;

		public const sbyte Yangzhou = 29;
	}

	public static class DefValue
	{
		public static SettlementTreasuryLimitItem Shaolin => Instance[(sbyte)0];

		public static SettlementTreasuryLimitItem Emei => Instance[(sbyte)1];

		public static SettlementTreasuryLimitItem Baihua => Instance[(sbyte)2];

		public static SettlementTreasuryLimitItem Wudang => Instance[(sbyte)3];

		public static SettlementTreasuryLimitItem Yuanshan => Instance[(sbyte)4];

		public static SettlementTreasuryLimitItem Shixiang => Instance[(sbyte)5];

		public static SettlementTreasuryLimitItem Ranshan => Instance[(sbyte)6];

		public static SettlementTreasuryLimitItem Xuannv => Instance[(sbyte)7];

		public static SettlementTreasuryLimitItem Zhujian => Instance[(sbyte)8];

		public static SettlementTreasuryLimitItem Kongsang => Instance[(sbyte)9];

		public static SettlementTreasuryLimitItem Jingang => Instance[(sbyte)10];

		public static SettlementTreasuryLimitItem Wuxian => Instance[(sbyte)11];

		public static SettlementTreasuryLimitItem Jieqing => Instance[(sbyte)12];

		public static SettlementTreasuryLimitItem Fulong => Instance[(sbyte)13];

		public static SettlementTreasuryLimitItem Xuehou => Instance[(sbyte)14];

		public static SettlementTreasuryLimitItem Jingcheng => Instance[(sbyte)15];

		public static SettlementTreasuryLimitItem Chengdu => Instance[(sbyte)16];

		public static SettlementTreasuryLimitItem Guizhou => Instance[(sbyte)17];

		public static SettlementTreasuryLimitItem Xiangyang => Instance[(sbyte)18];

		public static SettlementTreasuryLimitItem Taiyuan => Instance[(sbyte)19];

		public static SettlementTreasuryLimitItem Guangzhou => Instance[(sbyte)20];

		public static SettlementTreasuryLimitItem Qingzhou => Instance[(sbyte)21];

		public static SettlementTreasuryLimitItem Jiangling => Instance[(sbyte)22];

		public static SettlementTreasuryLimitItem Fuzhou => Instance[(sbyte)23];

		public static SettlementTreasuryLimitItem Liaoyang => Instance[(sbyte)24];

		public static SettlementTreasuryLimitItem Qinzhou => Instance[(sbyte)25];

		public static SettlementTreasuryLimitItem Dali => Instance[(sbyte)26];

		public static SettlementTreasuryLimitItem Shouchun => Instance[(sbyte)27];

		public static SettlementTreasuryLimitItem Hangzhou => Instance[(sbyte)28];

		public static SettlementTreasuryLimitItem Yangzhou => Instance[(sbyte)29];
	}

	public static SettlementTreasuryLimit Instance = new SettlementTreasuryLimit();

	private readonly Dictionary<string, int> _refNameMap = new Dictionary<string, int>();

	private List<SettlementTreasuryLimitItem> _dataArray;

	private readonly Dictionary<int, SettlementTreasuryLimitItem> _extraDataMap = new Dictionary<int, SettlementTreasuryLimitItem>();

	private readonly HashSet<string> RequiredFields = new HashSet<string> { "TemplateId", "Name" };

	public IReadOnlyDictionary<string, int> RefNameMap => _refNameMap;

	public SettlementTreasuryLimitItem this[sbyte id] => GetItem(id);

	public SettlementTreasuryLimitItem this[int id] => GetItem((sbyte)id);

	public SettlementTreasuryLimitItem this[string refName] => this[_refNameMap[refName]];

	public int Count => _dataArray.Count;

	public int CountWithExtra => Count + _extraDataMap.Count;

	private void CreateItems0()
	{
		_dataArray.Add(new SettlementTreasuryLimitItem(0, 0));
		_dataArray.Add(new SettlementTreasuryLimitItem(1, 1));
		_dataArray.Add(new SettlementTreasuryLimitItem(2, 2));
		_dataArray.Add(new SettlementTreasuryLimitItem(3, 3));
		_dataArray.Add(new SettlementTreasuryLimitItem(4, 4));
		_dataArray.Add(new SettlementTreasuryLimitItem(5, 5));
		_dataArray.Add(new SettlementTreasuryLimitItem(6, 6));
		_dataArray.Add(new SettlementTreasuryLimitItem(7, 7));
		_dataArray.Add(new SettlementTreasuryLimitItem(8, 8));
		_dataArray.Add(new SettlementTreasuryLimitItem(9, 9));
		_dataArray.Add(new SettlementTreasuryLimitItem(10, 10));
		_dataArray.Add(new SettlementTreasuryLimitItem(11, 11));
		_dataArray.Add(new SettlementTreasuryLimitItem(12, 12));
		_dataArray.Add(new SettlementTreasuryLimitItem(13, 13));
		_dataArray.Add(new SettlementTreasuryLimitItem(14, 14));
		_dataArray.Add(new SettlementTreasuryLimitItem(15, 15));
		_dataArray.Add(new SettlementTreasuryLimitItem(16, 16));
		_dataArray.Add(new SettlementTreasuryLimitItem(17, 17));
		_dataArray.Add(new SettlementTreasuryLimitItem(18, 18));
		_dataArray.Add(new SettlementTreasuryLimitItem(19, 19));
		_dataArray.Add(new SettlementTreasuryLimitItem(20, 20));
		_dataArray.Add(new SettlementTreasuryLimitItem(21, 21));
		_dataArray.Add(new SettlementTreasuryLimitItem(22, 22));
		_dataArray.Add(new SettlementTreasuryLimitItem(23, 23));
		_dataArray.Add(new SettlementTreasuryLimitItem(24, 24));
		_dataArray.Add(new SettlementTreasuryLimitItem(25, 25));
		_dataArray.Add(new SettlementTreasuryLimitItem(26, 26));
		_dataArray.Add(new SettlementTreasuryLimitItem(27, 27));
		_dataArray.Add(new SettlementTreasuryLimitItem(28, 28));
		_dataArray.Add(new SettlementTreasuryLimitItem(29, 29));
	}

	public void Init()
	{
		_refNameMap.Clear();
		_refNameMap.Load("SettlementTreasuryLimit");
		_extraDataMap.Clear();
		_dataArray = new List<SettlementTreasuryLimitItem>(30);
		CreateItems0();
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
		SettlementTreasuryLimitItem settlementTreasuryLimitItem = (SettlementTreasuryLimitItem)configItem;
		int templateId = settlementTreasuryLimitItem.TemplateId;
		if (templateId < _dataArray.Count)
		{
			throw new Exception($"SettlementTreasuryLimit template id {settlementTreasuryLimitItem.TemplateId} created by {identifier} already exist.");
		}
		if (_extraDataMap.ContainsKey(templateId))
		{
			throw new Exception($"SettlementTreasuryLimit extra template id {settlementTreasuryLimitItem.TemplateId} created by {identifier} already exist.");
		}
		if (_refNameMap.TryGetValue(refName, out var value))
		{
			throw new Exception($"SettlementTreasuryLimit template reference name {refName}(id = {settlementTreasuryLimitItem.TemplateId}) created by {identifier} already exist with templateId {value}).");
		}
		_refNameMap.Add(refName, templateId);
		_extraDataMap.Add(templateId, settlementTreasuryLimitItem);
		return templateId;
	}

	public SettlementTreasuryLimitItem GetItem(sbyte id)
	{
		if (id < 0)
		{
			return null;
		}
		if (id < _dataArray.Count)
		{
			return _dataArray[id];
		}
		if (_extraDataMap.TryGetValue(id, out var value))
		{
			return value;
		}
		AdaptableLog.TagWarning(GetType().FullName, $"index {id} is not in range [0, {_dataArray.Count}) and is not defined in _extraDataMap (count: {_extraDataMap.Count})");
		return null;
	}

	public List<sbyte> GetAllKeys()
	{
		List<sbyte> list = new List<sbyte>();
		list.AddRange(from item in _dataArray
			where item != null
			select item.TemplateId);
		list.AddRange(from item in _extraDataMap.Values
			where item != null
			select item.TemplateId);
		return list;
	}

	public void Iterate(Func<SettlementTreasuryLimitItem, bool> iterateFunc)
	{
		if (iterateFunc == null)
		{
			return;
		}
		foreach (SettlementTreasuryLimitItem item in _dataArray)
		{
			if (item != null && !iterateFunc(item))
			{
				break;
			}
		}
		foreach (SettlementTreasuryLimitItem value in _extraDataMap.Values)
		{
			if (value != null && !iterateFunc(value))
			{
				break;
			}
		}
	}

	IEnumerator<SettlementTreasuryLimitItem> IEnumerable<SettlementTreasuryLimitItem>.GetEnumerator()
	{
		foreach (SettlementTreasuryLimitItem item in _dataArray)
		{
			yield return item;
		}
		foreach (SettlementTreasuryLimitItem value in _extraDataMap.Values)
		{
			yield return value;
		}
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		foreach (SettlementTreasuryLimitItem item in _dataArray)
		{
			yield return item;
		}
		foreach (SettlementTreasuryLimitItem value in _extraDataMap.Values)
		{
			yield return value;
		}
	}
}
