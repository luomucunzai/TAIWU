using System;
using System.Collections.Generic;
using System.Linq;
using Config;
using GameData.Serializer;
using GameData.Utilities;

namespace GameData.Domains.Information;

public class NormalInformationCollection : ISerializableGameData
{
	public readonly IDictionary<short, sbyte> ReceivedCounts = new Dictionary<short, sbyte>();

	private IList<NormalInformation> _elements;

	public IList<NormalInformation> GetList()
	{
		if (_elements == null)
		{
			return _elements = new List<NormalInformation>();
		}
		return _elements;
	}

	private short _GetUsedCountIndex(NormalInformation normalInformation)
	{
		InformationItem item = Config.Information.Instance.GetItem(normalInformation.TemplateId);
		if (item != null)
		{
			return (short)(-(item.InfoIds[normalInformation.Level] + 1));
		}
		return short.MinValue;
	}

	public sbyte GetUsedCountMax(NormalInformation normalInformation)
	{
		InformationItem item = Config.Information.Instance.GetItem(normalInformation.TemplateId);
		if (item != null && !item.UsedCountWithMax)
		{
			return GetRemainUsableCount(normalInformation);
		}
		return (sbyte)GlobalConfig.Instance.NormalInformationDefaultCostableMaxUseCount;
	}

	public sbyte GetUsedCount(NormalInformation normalInformation)
	{
		InformationItem item = Config.Information.Instance.GetItem(normalInformation.TemplateId);
		if (item != null && item.UsedCountWithMax)
		{
			short key = _GetUsedCountIndex(normalInformation);
			if (ReceivedCounts.TryGetValue(key, out var value) && value > 0)
			{
				return value;
			}
			return 0;
		}
		return 0;
	}

	public sbyte SetUsedCount(NormalInformation normalInformation, sbyte count)
	{
		InformationItem item = Config.Information.Instance.GetItem(normalInformation.TemplateId);
		short key = _GetUsedCountIndex(normalInformation);
		if (item != null && item.UsedCountWithMax)
		{
			ReceivedCounts[key] = count;
			return count;
		}
		sbyte usedCountMax = GetUsedCountMax(normalInformation);
		usedCountMax -= count;
		SetRemainUsableCount(normalInformation, usedCountMax);
		return 0;
	}

	public sbyte GetRemainUsableCount(NormalInformation normalInformation)
	{
		InformationItem item = Config.Information.Instance.GetItem(normalInformation.TemplateId);
		Tester.Assert(item != null && !item.UsedCountWithMax);
		short key = _GetUsedCountIndex(normalInformation);
		if (ReceivedCounts.TryGetValue(key, out var value))
		{
			if (value <= 0)
			{
				return (sbyte)(-value);
			}
			return (sbyte)(3 - value);
		}
		return (sbyte)GlobalConfig.Instance.NormalInformationDefaultCostableMaxUseCount;
	}

	public sbyte SetRemainUsableCount(NormalInformation normalInformation, sbyte remainCount)
	{
		InformationItem item = Config.Information.Instance.GetItem(normalInformation.TemplateId);
		Tester.Assert(item != null && !item.UsedCountWithMax);
		short key = _GetUsedCountIndex(normalInformation);
		remainCount = Math.Clamp(remainCount, 0, (sbyte)GlobalConfig.Instance.NormalInformationMaxRemainCount);
		ReceivedCounts[key] = (sbyte)(-remainCount);
		return remainCount;
	}

	public bool IsSerializedSizeFixed()
	{
		return false;
	}

	public int GetSerializedSize()
	{
		return 4 + ReceivedCounts.Count * 3 + ((_elements == null) ? 4 : (4 + _elements.Sum((NormalInformation element) => element.GetSerializedSize())));
	}

	public unsafe int Serialize(byte* pData)
	{
		byte* ptr = pData;
		*(int*)ptr = ReceivedCounts.Count;
		ptr += 4;
		foreach (KeyValuePair<short, sbyte> receivedCount in ReceivedCounts)
		{
			*(short*)ptr = receivedCount.Key;
			ptr += 2;
			*ptr = (byte)receivedCount.Value;
			ptr++;
		}
		if (_elements != null)
		{
			*(int*)ptr = _elements.Count;
			ptr += 4;
			foreach (NormalInformation element in _elements)
			{
				ptr += element.Serialize(ptr);
			}
		}
		else
		{
			*(int*)ptr = 0;
			ptr += 4;
		}
		return (int)(ptr - pData);
	}

	public unsafe int Deserialize(byte* pData)
	{
		byte* ptr = pData;
		int num = *(int*)ptr;
		ptr += 4;
		ReceivedCounts.Clear();
		for (int i = 0; i < num; i++)
		{
			short key = *(short*)ptr;
			ptr += 2;
			sbyte value = (sbyte)(*ptr);
			ptr++;
			ReceivedCounts.Add(key, value);
		}
		int num2 = *(int*)ptr;
		ptr += 4;
		if (num2 > 0)
		{
			if (_elements == null)
			{
				_elements = new List<NormalInformation>();
			}
			else
			{
				_elements.Clear();
			}
			for (int j = 0; j < num2; j++)
			{
				NormalInformation item = default(NormalInformation);
				ptr += item.Deserialize(ptr);
				_elements.Add(item);
			}
		}
		else
		{
			_elements?.Clear();
		}
		return (int)(ptr - pData);
	}

	public void ClearUsedCountData(NormalInformation normalInformation)
	{
		short key = _GetUsedCountIndex(normalInformation);
		ReceivedCounts.Remove(key);
	}
}
