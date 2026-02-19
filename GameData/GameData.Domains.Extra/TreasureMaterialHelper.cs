using System;
using System.Collections.Generic;
using Config;
using GameData.Domains.Map;
using GameData.Utilities;
using Redzen.Random;

namespace GameData.Domains.Extra;

internal class TreasureMaterialHelper
{
	private readonly Dictionary<sbyte, Dictionary<short, int>> _complexConfig = new Dictionary<sbyte, Dictionary<short, int>>();

	private readonly List<short> _templateCaches = new List<short>();

	private static bool IsValid(short templateId, int brokenLevel)
	{
		MiscItem miscItem = Misc.Instance[templateId];
		return miscItem.AllowBrokenLevels.Contains(brokenLevel);
	}

	private static bool IsValidExclusive(short templateId, int brokenLevel)
	{
		return IsValid(templateId, brokenLevel) && brokenLevel == Misc.Instance[templateId].AllowBrokenLevels.Min();
	}

	public TreasureMaterialHelper(EMiscGenerateType type)
	{
		if (type == EMiscGenerateType.Invalid)
		{
			return;
		}
		foreach (MiscItem item2 in (IEnumerable<MiscItem>)Misc.Instance)
		{
			if (item2.GenerateType < type)
			{
				continue;
			}
			foreach (TreasureStateInfo item3 in item2.StateBuryAmount)
			{
				if (item3.Amount > 0)
				{
					Dictionary<short, int> orNew = _complexConfig.GetOrNew(item3.MapState);
					orNew[item2.TemplateId] = orNew.GetOrDefault(item2.TemplateId) + item3.Amount;
				}
			}
		}
		foreach (Dictionary<short, int> value in _complexConfig.Values)
		{
			_templateCaches.Clear();
			foreach (var (item, num3) in value)
			{
				if (num3 <= 0)
				{
					_templateCaches.Add(item);
				}
			}
			foreach (short templateCache in _templateCaches)
			{
				value.Remove(templateCache);
			}
		}
	}

	public void RegenerateInState(sbyte stateId, IReadOnlyList<short> templateIds)
	{
		if (templateIds.Count == 0)
		{
			return;
		}
		AdaptableLog.Warning($"Regenerate {templateIds.Count} broken material in state {stateId}");
		Dictionary<short, int> orNew = _complexConfig.GetOrNew(stateId);
		foreach (short templateId in templateIds)
		{
			MiscItem miscItem = Misc.Instance[templateId];
			if (miscItem != null && miscItem.GenerateType != EMiscGenerateType.Invalid)
			{
				orNew[templateId] = orNew.GetOrDefault(templateId) + 1;
			}
		}
	}

	private int CalcPreferPickAmount(sbyte stateId, int brokenLevel)
	{
		if (!_complexConfig.TryGetValue(stateId, out var value))
		{
			return 0;
		}
		int num = 0;
		int num2 = brokenLevel;
		foreach (var (num5, num6) in value)
		{
			if (!IsValid(num5, brokenLevel))
			{
				continue;
			}
			num += num6;
			MiscItem miscItem = Misc.Instance[num5];
			foreach (int allowBrokenLevel in miscItem.AllowBrokenLevels)
			{
				num2 = Math.Min(num2, allowBrokenLevel);
			}
		}
		return num / Math.Max(brokenLevel - num2 + 1, 1);
	}

	private short PickInState(IRandomSource random, sbyte stateId, int brokenLevel)
	{
		if (!_complexConfig.TryGetValue(stateId, out var value))
		{
			return -1;
		}
		_templateCaches.Clear();
		foreach (var (num3, num4) in value)
		{
			if (IsValid(num3, brokenLevel) && num4 > 0)
			{
				_templateCaches.Add(num3);
			}
		}
		if (_templateCaches.Count == 0)
		{
			return -1;
		}
		short random2 = _templateCaches.GetRandom(random);
		value[random2]--;
		if (value[random2] <= 0)
		{
			value.Remove(random2);
		}
		return random2;
	}

	private void PickInStateExclusive(IList<short> picked, sbyte stateId, int brokenLevel)
	{
		if (!_complexConfig.TryGetValue(stateId, out var value))
		{
			return;
		}
		List<short> list = ObjectPool<List<short>>.Instance.Get();
		foreach (var (num3, num4) in value)
		{
			if (IsValidExclusive(num3, brokenLevel))
			{
				for (int i = 0; i < num4; i++)
				{
					picked.Add(num3);
				}
				list.Add(num3);
			}
		}
		foreach (short item in list)
		{
			value.Remove(item);
		}
		ObjectPool<List<short>>.Instance.Return(list);
	}

	public void PickAreaTemplates(IRandomSource random, IList<short> picked, short areaId)
	{
		if (picked == null)
		{
			throw new ArgumentNullException("picked");
		}
		MapDomain map = DomainManager.Map;
		int brokenLevel = map.QueryAreaBrokenLevel(areaId);
		sbyte stateTemplateIdByAreaId = map.GetStateTemplateIdByAreaId(areaId);
		int num = CalcPreferPickAmount(stateTemplateIdByAreaId, brokenLevel);
		PickInStateExclusive(picked, stateTemplateIdByAreaId, brokenLevel);
		for (int i = picked.Count; i < num; i++)
		{
			short num2 = PickInState(random, stateTemplateIdByAreaId, brokenLevel);
			if (num2 >= 0)
			{
				picked.Add(num2);
			}
		}
	}
}
