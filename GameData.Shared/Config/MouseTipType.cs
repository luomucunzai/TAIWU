using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Config.Common;
using GameData.Utilities;

namespace Config;

[Serializable]
public class MouseTipType : IEnumerable<MouseTipTypeItem>, IEnumerable, IConfigData
{
	public static class DefKey
	{
		public const short SingleDesc = 0;

		public const short Simple = 1;

		public const short CombatSkill = 2;

		public const short Weapon = 3;

		public const short SkillBook = 4;

		public const short MakingTool = 5;

		public const short Material = 6;

		public const short Cricket = 7;

		public const short Armor = 8;

		public const short Carrier = 9;

		public const short Clothing = 10;

		public const short Food = 11;

		public const short Medicine = 12;

		public const short Sundries = 13;

		public const short TeaWine = 14;

		public const short Accessory = 15;

		public const short LifeRecords = 16;

		public const short Character = 17;

		public const short Resource = 18;

		public const short ResourceHolder = 19;

		public const short EatingItems = 20;

		public const short MaBlock = 21;

		public const short Feature = 22;

		public const short MartialArtTournament = 23;

		public const short SimpleWide = 24;

		public const short MakeItem = 25;

		public const short InnateFiveElements = 26;

		public const short DisassembleItem = 27;

		public const short RepairItem = 28;

		public const short ReadingBook = 29;

		public const short SecretInformation = 30;

		public const short LifeCombatSkillValue = 31;

		public const short BuildingShowItem = 32;

		public const short BuildingShowRecruitPeople = 33;

		public const short SecretInformationBroadcastNotify = 34;

		public const short DebtChange = 35;

		public const short LegendaryBookBonus = 36;

		public const short ProfessionSkill = 37;

		public const short AdventureNode = 38;

		public const short Injury = 39;
	}

	public static MouseTipType Instance = new MouseTipType();

	private readonly Dictionary<string, int> _refNameMap = new Dictionary<string, int>();

	private List<MouseTipTypeItem> _dataArray;

	private readonly Dictionary<int, MouseTipTypeItem> _extraDataMap = new Dictionary<int, MouseTipTypeItem>();

	private readonly HashSet<string> RequiredFields = new HashSet<string> { "TemplateId", "TipsType" };

	public IReadOnlyDictionary<string, int> RefNameMap => _refNameMap;

	public MouseTipTypeItem this[short id] => GetItem(id);

	public MouseTipTypeItem this[int id] => GetItem((short)id);

	public MouseTipTypeItem this[string refName] => this[_refNameMap[refName]];

	public int Count => _dataArray.Count;

	public int CountWithExtra => Count + _extraDataMap.Count;

	private void CreateItems0()
	{
		_dataArray.Add(new MouseTipTypeItem(0, 0));
		_dataArray.Add(new MouseTipTypeItem(1, 1));
		_dataArray.Add(new MouseTipTypeItem(2, 2));
		_dataArray.Add(new MouseTipTypeItem(3, 3));
		_dataArray.Add(new MouseTipTypeItem(4, 5));
		_dataArray.Add(new MouseTipTypeItem(5, 6));
		_dataArray.Add(new MouseTipTypeItem(6, 7));
		_dataArray.Add(new MouseTipTypeItem(7, 8));
		_dataArray.Add(new MouseTipTypeItem(8, 10));
		_dataArray.Add(new MouseTipTypeItem(9, 11));
		_dataArray.Add(new MouseTipTypeItem(10, 12));
		_dataArray.Add(new MouseTipTypeItem(11, 13));
		_dataArray.Add(new MouseTipTypeItem(12, 14));
		_dataArray.Add(new MouseTipTypeItem(13, 15));
		_dataArray.Add(new MouseTipTypeItem(14, 16));
		_dataArray.Add(new MouseTipTypeItem(15, 17));
		_dataArray.Add(new MouseTipTypeItem(16, 18));
		_dataArray.Add(new MouseTipTypeItem(17, 19));
		_dataArray.Add(new MouseTipTypeItem(18, 20));
		_dataArray.Add(new MouseTipTypeItem(19, 21));
		_dataArray.Add(new MouseTipTypeItem(20, 22));
		_dataArray.Add(new MouseTipTypeItem(21, 23));
		_dataArray.Add(new MouseTipTypeItem(22, 24));
		_dataArray.Add(new MouseTipTypeItem(23, 25));
		_dataArray.Add(new MouseTipTypeItem(24, 26));
		_dataArray.Add(new MouseTipTypeItem(25, 27));
		_dataArray.Add(new MouseTipTypeItem(26, 28));
		_dataArray.Add(new MouseTipTypeItem(27, 29));
		_dataArray.Add(new MouseTipTypeItem(28, 30));
		_dataArray.Add(new MouseTipTypeItem(29, 31));
		_dataArray.Add(new MouseTipTypeItem(30, 32));
		_dataArray.Add(new MouseTipTypeItem(31, 33));
		_dataArray.Add(new MouseTipTypeItem(32, 34));
		_dataArray.Add(new MouseTipTypeItem(33, 35));
		_dataArray.Add(new MouseTipTypeItem(34, 36));
		_dataArray.Add(new MouseTipTypeItem(35, 37));
		_dataArray.Add(new MouseTipTypeItem(36, 38));
		_dataArray.Add(new MouseTipTypeItem(37, 39));
		_dataArray.Add(new MouseTipTypeItem(38, 40));
		_dataArray.Add(new MouseTipTypeItem(39, 42));
	}

	public void Init()
	{
		_refNameMap.Clear();
		_refNameMap.Load("MouseTipType");
		_extraDataMap.Clear();
		_dataArray = new List<MouseTipTypeItem>(40);
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
		MouseTipTypeItem mouseTipTypeItem = (MouseTipTypeItem)configItem;
		int templateId = mouseTipTypeItem.TemplateId;
		if (templateId < _dataArray.Count)
		{
			throw new Exception($"MouseTipType template id {mouseTipTypeItem.TemplateId} created by {identifier} already exist.");
		}
		if (_extraDataMap.ContainsKey(templateId))
		{
			throw new Exception($"MouseTipType extra template id {mouseTipTypeItem.TemplateId} created by {identifier} already exist.");
		}
		if (_refNameMap.TryGetValue(refName, out var value))
		{
			throw new Exception($"MouseTipType template reference name {refName}(id = {mouseTipTypeItem.TemplateId}) created by {identifier} already exist with templateId {value}).");
		}
		_refNameMap.Add(refName, templateId);
		_extraDataMap.Add(templateId, mouseTipTypeItem);
		return templateId;
	}

	public MouseTipTypeItem GetItem(short id)
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

	public List<short> GetAllKeys()
	{
		List<short> list = new List<short>();
		list.AddRange(from item in _dataArray
			where item != null
			select item.TemplateId);
		list.AddRange(from item in _extraDataMap.Values
			where item != null
			select item.TemplateId);
		return list;
	}

	public void Iterate(Func<MouseTipTypeItem, bool> iterateFunc)
	{
		if (iterateFunc == null)
		{
			return;
		}
		foreach (MouseTipTypeItem item in _dataArray)
		{
			if (item != null && !iterateFunc(item))
			{
				break;
			}
		}
		foreach (MouseTipTypeItem value in _extraDataMap.Values)
		{
			if (value != null && !iterateFunc(value))
			{
				break;
			}
		}
	}

	IEnumerator<MouseTipTypeItem> IEnumerable<MouseTipTypeItem>.GetEnumerator()
	{
		foreach (MouseTipTypeItem item in _dataArray)
		{
			yield return item;
		}
		foreach (MouseTipTypeItem value in _extraDataMap.Values)
		{
			yield return value;
		}
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		foreach (MouseTipTypeItem item in _dataArray)
		{
			yield return item;
		}
		foreach (MouseTipTypeItem value in _extraDataMap.Values)
		{
			yield return value;
		}
	}
}
