using System;
using System.Collections.Generic;
using Config;
using Config.ConfigCells.Character;
using GameData.Serializer;

namespace GameData.Domains.Taiwu;

public class SkillBreakBonusCollection : ISerializableGameData, IEquatable<SkillBreakBonusCollection>
{
	[SerializableGameDataField]
	public Dictionary<short, short> CharacterPropertyBonusDict = new Dictionary<short, short>();

	[SerializableGameDataField]
	public Dictionary<short, short> CombatSkillPropertyBonusDict = new Dictionary<short, short>();

	public void AddBonusType(short bonusTypeTemplateId)
	{
		SkillBreakPlateGridBonusTypeItem skillBreakPlateGridBonusTypeItem = SkillBreakPlateGridBonusType.Instance[bonusTypeTemplateId];
		PropertyAndValue[] characterPropertyBonusList = skillBreakPlateGridBonusTypeItem.CharacterPropertyBonusList;
		for (int i = 0; i < characterPropertyBonusList.Length; i++)
		{
			PropertyAndValue propertyAndValue = characterPropertyBonusList[i];
			if (CharacterPropertyBonusDict.ContainsKey(propertyAndValue.PropertyId))
			{
				int num = CharacterPropertyBonusDict[propertyAndValue.PropertyId] + propertyAndValue.Value;
				if (num == 0)
				{
					CharacterPropertyBonusDict.Remove(propertyAndValue.PropertyId);
				}
				else
				{
					CharacterPropertyBonusDict[propertyAndValue.PropertyId] = (short)num;
				}
			}
			else
			{
				CharacterPropertyBonusDict.Add(propertyAndValue.PropertyId, propertyAndValue.Value);
			}
		}
		characterPropertyBonusList = skillBreakPlateGridBonusTypeItem.CombatSkillPropertyBonusList;
		for (int i = 0; i < characterPropertyBonusList.Length; i++)
		{
			PropertyAndValue propertyAndValue2 = characterPropertyBonusList[i];
			if (CombatSkillPropertyBonusDict.ContainsKey(propertyAndValue2.PropertyId))
			{
				int num2 = CombatSkillPropertyBonusDict[propertyAndValue2.PropertyId] + propertyAndValue2.Value;
				if (num2 == 0)
				{
					CombatSkillPropertyBonusDict.Remove(propertyAndValue2.PropertyId);
				}
				else
				{
					CombatSkillPropertyBonusDict[propertyAndValue2.PropertyId] = (short)num2;
				}
			}
			else
			{
				CombatSkillPropertyBonusDict.Add(propertyAndValue2.PropertyId, propertyAndValue2.Value);
			}
		}
	}

	public void RemoveBonusType(short bonusTypeTemplateId)
	{
		SkillBreakPlateGridBonusTypeItem skillBreakPlateGridBonusTypeItem = SkillBreakPlateGridBonusType.Instance[bonusTypeTemplateId];
		PropertyAndValue[] characterPropertyBonusList = skillBreakPlateGridBonusTypeItem.CharacterPropertyBonusList;
		for (int i = 0; i < characterPropertyBonusList.Length; i++)
		{
			PropertyAndValue propertyAndValue = characterPropertyBonusList[i];
			if (CharacterPropertyBonusDict.ContainsKey(propertyAndValue.PropertyId))
			{
				int num = CharacterPropertyBonusDict[propertyAndValue.PropertyId] - propertyAndValue.Value;
				if (num == 0)
				{
					CharacterPropertyBonusDict.Remove(propertyAndValue.PropertyId);
				}
				else
				{
					CharacterPropertyBonusDict[propertyAndValue.PropertyId] = (short)num;
				}
			}
			else
			{
				CharacterPropertyBonusDict.Add(propertyAndValue.PropertyId, (short)(-propertyAndValue.Value));
			}
		}
		characterPropertyBonusList = skillBreakPlateGridBonusTypeItem.CombatSkillPropertyBonusList;
		for (int i = 0; i < characterPropertyBonusList.Length; i++)
		{
			PropertyAndValue propertyAndValue2 = characterPropertyBonusList[i];
			if (CombatSkillPropertyBonusDict.ContainsKey(propertyAndValue2.PropertyId))
			{
				int num2 = CombatSkillPropertyBonusDict[propertyAndValue2.PropertyId] - propertyAndValue2.Value;
				if (num2 == 0)
				{
					CombatSkillPropertyBonusDict.Remove(propertyAndValue2.PropertyId);
				}
				else
				{
					CombatSkillPropertyBonusDict[propertyAndValue2.PropertyId] = (short)num2;
				}
			}
			else
			{
				CombatSkillPropertyBonusDict.Add(propertyAndValue2.PropertyId, (short)(-propertyAndValue2.Value));
			}
		}
	}

	public void Clear()
	{
		CharacterPropertyBonusDict.Clear();
		CombatSkillPropertyBonusDict.Clear();
	}

	public bool IsSerializedSizeFixed()
	{
		return false;
	}

	public int GetSerializedSize()
	{
		int num = 0;
		num += 2 + 4 * CharacterPropertyBonusDict.Count;
		num += 2 + 4 * CombatSkillPropertyBonusDict.Count;
		if (num > 4)
		{
			return (num + 3) / 4 * 4;
		}
		return num;
	}

	public unsafe int Serialize(byte* pData)
	{
		byte* ptr = pData;
		*(short*)ptr = (short)CharacterPropertyBonusDict.Count;
		ptr += 2;
		foreach (KeyValuePair<short, short> item in CharacterPropertyBonusDict)
		{
			*(short*)ptr = item.Key;
			ptr += 2;
			*(short*)ptr = item.Value;
			ptr += 2;
		}
		*(short*)ptr = (short)CombatSkillPropertyBonusDict.Count;
		ptr += 2;
		foreach (KeyValuePair<short, short> item2 in CombatSkillPropertyBonusDict)
		{
			*(short*)ptr = item2.Key;
			ptr += 2;
			*(short*)ptr = item2.Value;
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
		short num = *(short*)ptr;
		ptr += 2;
		CharacterPropertyBonusDict.Clear();
		for (int i = 0; i < num; i++)
		{
			short key = *(short*)ptr;
			ptr += 2;
			short value = *(short*)ptr;
			ptr += 2;
			CharacterPropertyBonusDict.Add(key, value);
		}
		num = *(short*)ptr;
		ptr += 2;
		CombatSkillPropertyBonusDict.Clear();
		for (int j = 0; j < num; j++)
		{
			short key2 = *(short*)ptr;
			ptr += 2;
			short value2 = *(short*)ptr;
			ptr += 2;
			CombatSkillPropertyBonusDict.Add(key2, value2);
		}
		int num2 = (int)(ptr - pData);
		if (num2 > 4)
		{
			return (num2 + 3) / 4 * 4;
		}
		return num2;
	}

	public bool Equals(SkillBreakBonusCollection other)
	{
		if (other == null)
		{
			return false;
		}
		if (CharacterPropertyBonusDict.Count != other.CharacterPropertyBonusDict.Count)
		{
			return false;
		}
		if (CombatSkillPropertyBonusDict.Count != other.CombatSkillPropertyBonusDict.Count)
		{
			return false;
		}
		foreach (KeyValuePair<short, short> item in CharacterPropertyBonusDict)
		{
			if (!other.CharacterPropertyBonusDict.TryGetValue(item.Key, out var value) || value != item.Value)
			{
				return false;
			}
		}
		foreach (KeyValuePair<short, short> item2 in CombatSkillPropertyBonusDict)
		{
			if (!other.CombatSkillPropertyBonusDict.TryGetValue(item2.Key, out var value2) || value2 != item2.Value)
			{
				return false;
			}
		}
		return true;
	}
}
