using System.Collections.Generic;
using GameData.Serializer;
using GameData.Utilities;

namespace GameData.Domains.Taiwu.Profession.SkillsData;

[SerializableGameData(IsExtensible = true)]
public class AristocratSkillsData : IProfessionSkillsData, ISerializableGameData
{
	private static class FieldIds
	{
		public const ushort InfluencePowerBonus = 0;

		public const ushort RecommendedCharIds = 1;

		public const ushort Count = 2;

		public static readonly string[] FieldId2FieldName = new string[2] { "InfluencePowerBonus", "RecommendedCharIds" };
	}

	[SerializableGameDataField]
	private Dictionary<int, short> _influencePowerBonus;

	[SerializableGameDataField]
	private List<int> _recommendedCharIds;

	public void Initialize()
	{
		_influencePowerBonus?.Clear();
		_recommendedCharIds?.Clear();
	}

	public void InheritFrom(IProfessionSkillsData sourceData)
	{
		if (!(sourceData is ObsoleteAristocratSkillsData obsoleteAristocratSkillsData))
		{
			return;
		}
		foreach (KeyValuePair<int, short> influencePowerBonu in obsoleteAristocratSkillsData._influencePowerBonus)
		{
			_influencePowerBonus.Add(influencePowerBonu.Key, influencePowerBonu.Value);
		}
	}

	public short OfflineSetInfluencePowerBonus(int targetCharId, short bonus)
	{
		if (!_influencePowerBonus.TryGetValue(targetCharId, out var value))
		{
			value = 0;
		}
		_influencePowerBonus[targetCharId] = bonus;
		return value;
	}

	public bool OfflineRemoveInfluencePowerBonus(int targetCharId)
	{
		return _influencePowerBonus.Remove(targetCharId);
	}

	public short GetPreviousInfluencePowerBonus(int targetCharId)
	{
		if (!_influencePowerBonus.TryGetValue(targetCharId, out var value))
		{
			return 0;
		}
		return value;
	}

	public void OfflineAddRecommendedCharId(int charId)
	{
		_recommendedCharIds.Add(charId);
	}

	public void OfflineRemoveRecommendedCharId(int charId)
	{
		_recommendedCharIds.Remove(charId);
	}

	public int GetRecommendedCharIdInList(List<int> charIds)
	{
		foreach (int charId in charIds)
		{
			if (_recommendedCharIds.Contains(charId))
			{
				return charId;
			}
		}
		return -1;
	}

	public bool IsCharacterRecommended(int charId)
	{
		return _influencePowerBonus.ContainsKey(charId);
	}

	public AristocratSkillsData()
	{
		_influencePowerBonus = new Dictionary<int, short>();
		_recommendedCharIds = new List<int>();
	}

	public AristocratSkillsData(AristocratSkillsData other)
	{
		_influencePowerBonus = ((other._influencePowerBonus == null) ? null : new Dictionary<int, short>(other._influencePowerBonus));
		_recommendedCharIds = ((other._recommendedCharIds == null) ? null : new List<int>(other._recommendedCharIds));
	}

	public void Assign(AristocratSkillsData other)
	{
		_influencePowerBonus = ((other._influencePowerBonus == null) ? null : new Dictionary<int, short>(other._influencePowerBonus));
		_recommendedCharIds = ((other._recommendedCharIds == null) ? null : new List<int>(other._recommendedCharIds));
	}

	public bool IsSerializedSizeFixed()
	{
		return false;
	}

	public int GetSerializedSize()
	{
		int num = 2;
		num += DictionaryOfBasicTypePair.GetSerializedSize<int, short>((IReadOnlyDictionary<int, short>)_influencePowerBonus);
		num = ((_recommendedCharIds == null) ? (num + 2) : (num + (2 + 4 * _recommendedCharIds.Count)));
		if (num > 4)
		{
			return (num + 3) / 4 * 4;
		}
		return num;
	}

	public unsafe int Serialize(byte* pData)
	{
		byte* ptr = pData;
		*(short*)ptr = 2;
		ptr += 2;
		ptr += DictionaryOfBasicTypePair.Serialize<int, short>(ptr, ref _influencePowerBonus);
		if (_recommendedCharIds != null)
		{
			int count = _recommendedCharIds.Count;
			Tester.Assert(count <= 65535);
			*(ushort*)ptr = (ushort)count;
			ptr += 2;
			for (int i = 0; i < count; i++)
			{
				((int*)ptr)[i] = _recommendedCharIds[i];
			}
			ptr += 4 * count;
		}
		else
		{
			*(short*)ptr = 0;
			ptr += 2;
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
		ushort num = *(ushort*)ptr;
		ptr += 2;
		if (num > 0)
		{
			ptr += DictionaryOfBasicTypePair.Deserialize<int, short>(ptr, ref _influencePowerBonus);
		}
		if (num > 1)
		{
			ushort num2 = *(ushort*)ptr;
			ptr += 2;
			if (num2 > 0)
			{
				if (_recommendedCharIds == null)
				{
					_recommendedCharIds = new List<int>(num2);
				}
				else
				{
					_recommendedCharIds.Clear();
				}
				for (int i = 0; i < num2; i++)
				{
					_recommendedCharIds.Add(((int*)ptr)[i]);
				}
				ptr += 4 * num2;
			}
			else
			{
				_recommendedCharIds?.Clear();
			}
		}
		int num3 = (int)(ptr - pData);
		if (num3 > 4)
		{
			return (num3 + 3) / 4 * 4;
		}
		return num3;
	}
}
