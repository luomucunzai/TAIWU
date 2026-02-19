using System;
using System.Collections.Generic;
using GameData.Serializer;

namespace GameData.Domains.Taiwu.Profession.SkillsData;

[Obsolete]
[SerializableGameData(NotForDisplayModule = true)]
public class ObsoleteAristocratSkillsData : IProfessionSkillsData, ISerializableGameData
{
	public readonly Dictionary<int, short> _influencePowerBonus;

	public void Initialize()
	{
		_influencePowerBonus.Clear();
	}

	public void InheritFrom(IProfessionSkillsData sourceData)
	{
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

	public ObsoleteAristocratSkillsData()
	{
		_influencePowerBonus = new Dictionary<int, short>();
	}

	public bool IsSerializedSizeFixed()
	{
		return false;
	}

	public int GetSerializedSize()
	{
		int num = 2 + _influencePowerBonus.Count * 6;
		if (num > 4)
		{
			return (num + 3) / 4 * 4;
		}
		return num;
	}

	public unsafe int Serialize(byte* pData)
	{
		byte* ptr = pData;
		*(ushort*)ptr = (ushort)_influencePowerBonus.Count;
		ptr += 2;
		foreach (KeyValuePair<int, short> influencePowerBonu in _influencePowerBonus)
		{
			*(int*)ptr = influencePowerBonu.Key;
			ptr += 4;
			*(short*)ptr = influencePowerBonu.Value;
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
		for (int i = 0; i < num; i++)
		{
			int key = *(int*)ptr;
			ptr += 4;
			short value = *(short*)ptr;
			ptr += 2;
			_influencePowerBonus.Add(key, value);
		}
		int num2 = (int)(ptr - pData);
		if (num2 > 4)
		{
			return (num2 + 3) / 4 * 4;
		}
		return num2;
	}
}
