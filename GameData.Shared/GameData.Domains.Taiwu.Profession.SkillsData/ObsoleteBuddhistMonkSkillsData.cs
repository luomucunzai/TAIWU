using System;
using System.Collections.Generic;
using GameData.Serializer;

namespace GameData.Domains.Taiwu.Profession.SkillsData;

[Obsolete]
[SerializableGameData(NotForDisplayModule = true)]
public class ObsoleteBuddhistMonkSkillsData : IProfessionSkillsData, ISerializableGameData
{
	[SerializableGameDataField]
	public int _currSavedSoulsCount;

	[SerializableGameDataField]
	public readonly Dictionary<int, int> _directedSamsaraDict;

	public void Initialize()
	{
		_currSavedSoulsCount = 0;
		_directedSamsaraDict.Clear();
	}

	public void InheritFrom(IProfessionSkillsData sourceData)
	{
	}

	public void OfflineAddDirectedSamsara(int motherId, int reincarnatedCharId)
	{
		_directedSamsaraDict.Add(motherId, reincarnatedCharId);
		_currSavedSoulsCount = 0;
	}

	public int GetDirectedSamsara(int motherId)
	{
		if (!_directedSamsaraDict.TryGetValue(motherId, out var value))
		{
			return -1;
		}
		return value;
	}

	public int GetDirectedSamsaraMother(int reincarnatedCharId)
	{
		foreach (KeyValuePair<int, int> item in _directedSamsaraDict)
		{
			if (item.Value == reincarnatedCharId)
			{
				return item.Key;
			}
		}
		return -1;
	}

	public bool IsDirectedSamsaraCharacter(int charId)
	{
		return _directedSamsaraDict.ContainsValue(charId);
	}

	public bool OfflineRemoveDirectedSamsara(int motherId)
	{
		return _directedSamsaraDict.Remove(motherId);
	}

	public void OfflineAddSavedSoulsCount()
	{
		_currSavedSoulsCount++;
	}

	public void OfflineClearSavedSoulsCount()
	{
		_currSavedSoulsCount = 0;
	}

	public void OfflineClearDirectedSamsara()
	{
		_directedSamsaraDict.Clear();
	}

	public int GetSavedSoulsCount()
	{
		return _currSavedSoulsCount;
	}

	public ObsoleteBuddhistMonkSkillsData()
	{
		_directedSamsaraDict = new Dictionary<int, int>();
	}

	public bool IsSerializedSizeFixed()
	{
		return false;
	}

	public int GetSerializedSize()
	{
		int num = 6 + _directedSamsaraDict.Count * 8;
		if (num > 4)
		{
			return (num + 3) / 4 * 4;
		}
		return num;
	}

	public unsafe int Serialize(byte* pData)
	{
		byte* ptr = pData;
		*(int*)ptr = _currSavedSoulsCount;
		ptr += 4;
		*(ushort*)ptr = (ushort)_directedSamsaraDict.Count;
		ptr += 2;
		foreach (KeyValuePair<int, int> item in _directedSamsaraDict)
		{
			*(int*)ptr = item.Key;
			ptr += 4;
			*(int*)ptr = item.Value;
			ptr += 4;
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
		_currSavedSoulsCount = *(int*)ptr;
		ptr += 4;
		ushort num = *(ushort*)ptr;
		ptr += 2;
		for (int i = 0; i < num; i++)
		{
			int key = *(int*)ptr;
			ptr += 4;
			int value = *(int*)ptr;
			ptr += 4;
			_directedSamsaraDict.Add(key, value);
		}
		int num2 = (int)(ptr - pData);
		if (num2 > 4)
		{
			return (num2 + 3) / 4 * 4;
		}
		return num2;
	}
}
