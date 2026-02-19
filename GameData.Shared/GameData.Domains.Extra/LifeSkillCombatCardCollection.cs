using System.Collections.Generic;
using System.Linq;
using Config;
using GameData.Serializer;

namespace GameData.Domains.Extra;

public struct LifeSkillCombatCardCollection : ISerializableGameData
{
	[SerializableGameDataField]
	public Dictionary<sbyte, int> CardDict;

	public int CountSum => CardDict?.Sum((KeyValuePair<sbyte, int> d) => d.Value) ?? 0;

	public int GetLevelCountSum(int level)
	{
		return CardDict?.Sum((KeyValuePair<sbyte, int> d) => (LifeSkillCombatEffect.Instance[d.Key].Level == level) ? d.Value : 0) ?? 0;
	}

	public LifeSkillCombatCardCollection Clone()
	{
		return new LifeSkillCombatCardCollection
		{
			CardDict = new Dictionary<sbyte, int>(CardDict)
		};
	}

	public bool IsSerializedSizeFixed()
	{
		return false;
	}

	public int GetSerializedSize()
	{
		int num = 0;
		num = ((CardDict == null) ? (num + 4) : (num + (4 + 5 * CardDict.Count)));
		if (num > 4)
		{
			return (num + 3) / 4 * 4;
		}
		return num;
	}

	public unsafe int Serialize(byte* pData)
	{
		byte* ptr = pData;
		if (CardDict != null)
		{
			int count = CardDict.Count;
			*(int*)ptr = count;
			ptr += 4;
			foreach (KeyValuePair<sbyte, int> item in CardDict)
			{
				*ptr = (byte)item.Key;
				ptr++;
				*(int*)ptr = item.Value;
				ptr += 4;
			}
		}
		else
		{
			*(int*)ptr = 0;
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
		uint num = *(uint*)ptr;
		ptr += 4;
		if (num != 0)
		{
			if (CardDict == null)
			{
				CardDict = new Dictionary<sbyte, int>();
			}
			else
			{
				CardDict.Clear();
			}
			for (int i = 0; i < num; i++)
			{
				sbyte key = (sbyte)(*ptr);
				ptr++;
				int value = *(int*)ptr;
				ptr += 4;
				CardDict.Add(key, value);
			}
		}
		else
		{
			CardDict?.Clear();
		}
		int num2 = (int)(ptr - pData);
		if (num2 > 4)
		{
			return (num2 + 3) / 4 * 4;
		}
		return num2;
	}
}
