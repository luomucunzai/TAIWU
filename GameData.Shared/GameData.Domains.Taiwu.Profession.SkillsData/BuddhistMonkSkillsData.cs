using System.Collections.Generic;
using GameData.Serializer;

namespace GameData.Domains.Taiwu.Profession.SkillsData;

[SerializableGameData(IsExtensible = true)]
public class BuddhistMonkSkillsData : IProfessionSkillsData, ISerializableGameData
{
	private static class FieldIds
	{
		public const ushort CurrSavedSoulsCount = 0;

		public const ushort DirectedSamsaraDict = 1;

		public const ushort SamsaraFeatureDict = 2;

		public const ushort Count = 3;

		public static readonly string[] FieldId2FieldName = new string[3] { "CurrSavedSoulsCount", "DirectedSamsaraDict", "SamsaraFeatureDict" };
	}

	[SerializableGameDataField]
	private int _currSavedSoulsCount;

	[SerializableGameDataField]
	private Dictionary<int, int> _directedSamsaraDict;

	[SerializableGameDataField]
	private Dictionary<int, short> _samsaraFeatureDict;

	public void Initialize()
	{
		_currSavedSoulsCount = 0;
		_directedSamsaraDict?.Clear();
		_samsaraFeatureDict?.Clear();
	}

	public void InheritFrom(IProfessionSkillsData sourceData)
	{
		if (!(sourceData is ObsoleteBuddhistMonkSkillsData obsoleteBuddhistMonkSkillsData))
		{
			return;
		}
		_currSavedSoulsCount = obsoleteBuddhistMonkSkillsData._currSavedSoulsCount;
		foreach (KeyValuePair<int, int> item in obsoleteBuddhistMonkSkillsData._directedSamsaraDict)
		{
			_directedSamsaraDict.TryAdd(item.Key, item.Value);
		}
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

	public void OfflineAddSamsaraFeature(int reincarnatedCharId, short featureId)
	{
		_samsaraFeatureDict[reincarnatedCharId] = featureId;
	}

	public bool TryGetSamaraFeature(int reincarnatedCharId, out short featureId)
	{
		return _samsaraFeatureDict.TryGetValue(reincarnatedCharId, out featureId);
	}

	public bool OfflineRemoveSamsaraFeature(int reincarnatedCharId)
	{
		return _samsaraFeatureDict.Remove(reincarnatedCharId);
	}

	public BuddhistMonkSkillsData()
	{
		_directedSamsaraDict = new Dictionary<int, int>();
		_samsaraFeatureDict = new Dictionary<int, short>();
	}

	public bool IsSerializedSizeFixed()
	{
		return false;
	}

	public int GetSerializedSize()
	{
		int num = 6;
		num += DictionaryOfBasicTypePair.GetSerializedSize<int, int>((IReadOnlyDictionary<int, int>)_directedSamsaraDict);
		num += DictionaryOfBasicTypePair.GetSerializedSize<int, short>((IReadOnlyDictionary<int, short>)_samsaraFeatureDict);
		if (num > 4)
		{
			return (num + 3) / 4 * 4;
		}
		return num;
	}

	public unsafe int Serialize(byte* pData)
	{
		*(short*)pData = 3;
		byte* num = pData + 2;
		*(int*)num = _currSavedSoulsCount;
		byte* num2 = num + 4;
		byte* num3 = num2 + DictionaryOfBasicTypePair.Serialize<int, int>(num2, ref _directedSamsaraDict);
		int num4 = (int)(num3 + DictionaryOfBasicTypePair.Serialize<int, short>(num3, ref _samsaraFeatureDict) - pData);
		if (num4 > 4)
		{
			return (num4 + 3) / 4 * 4;
		}
		return num4;
	}

	public unsafe int Deserialize(byte* pData)
	{
		byte* ptr = pData;
		ushort num = *(ushort*)ptr;
		ptr += 2;
		if (num > 0)
		{
			_currSavedSoulsCount = *(int*)ptr;
			ptr += 4;
		}
		if (num > 1)
		{
			ptr += DictionaryOfBasicTypePair.Deserialize<int, int>(ptr, ref _directedSamsaraDict);
		}
		if (num > 2)
		{
			ptr += DictionaryOfBasicTypePair.Deserialize<int, short>(ptr, ref _samsaraFeatureDict);
		}
		int num2 = (int)(ptr - pData);
		if (num2 > 4)
		{
			return (num2 + 3) / 4 * 4;
		}
		return num2;
	}
}
