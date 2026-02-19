using System.Collections.Generic;
using GameData.Domains.Character;
using GameData.Domains.Character.Display;
using GameData.Serializer;

namespace GameData.Domains.LegendaryBook;

public class LegendaryBookOwnerData : ISerializableGameData
{
	[SerializableGameDataField]
	public Dictionary<sbyte, int> BookMap;

	[SerializableGameDataField]
	public Dictionary<int, CharacterDisplayData> CharacterDisplayDataMap;

	[SerializableGameDataField]
	public Dictionary<int, sbyte> CharacterHappinessMap;

	[SerializableGameDataField]
	public Dictionary<int, MainAttributes> CharacterAttributeMap;

	[SerializableGameDataField]
	public Dictionary<int, short> CharacterHealthMap;

	[SerializableGameDataField]
	public Dictionary<int, short> CharacterLeftMaxHealthMap;

	public LegendaryBookOwnerData()
	{
		BookMap = new Dictionary<sbyte, int>();
		CharacterDisplayDataMap = new Dictionary<int, CharacterDisplayData>();
		CharacterHappinessMap = new Dictionary<int, sbyte>();
		CharacterAttributeMap = new Dictionary<int, MainAttributes>();
		CharacterHealthMap = new Dictionary<int, short>();
		CharacterLeftMaxHealthMap = new Dictionary<int, short>();
	}

	public LegendaryBookOwnerData(LegendaryBookOwnerData other)
	{
		BookMap = ((other.BookMap == null) ? null : new Dictionary<sbyte, int>(other.BookMap));
		if (other.CharacterDisplayDataMap != null)
		{
			Dictionary<int, CharacterDisplayData> characterDisplayDataMap = other.CharacterDisplayDataMap;
			int count = characterDisplayDataMap.Count;
			CharacterDisplayDataMap = new Dictionary<int, CharacterDisplayData>(count);
			foreach (KeyValuePair<int, CharacterDisplayData> item in characterDisplayDataMap)
			{
				CharacterDisplayDataMap.Add(item.Key, new CharacterDisplayData(item.Value));
			}
		}
		else
		{
			CharacterDisplayDataMap = null;
		}
		CharacterHappinessMap = ((other.CharacterHappinessMap == null) ? null : new Dictionary<int, sbyte>(other.CharacterHappinessMap));
		CharacterAttributeMap = ((other.CharacterAttributeMap == null) ? null : new Dictionary<int, MainAttributes>(other.CharacterAttributeMap));
		CharacterHealthMap = ((other.CharacterHealthMap == null) ? null : new Dictionary<int, short>(other.CharacterHealthMap));
		CharacterLeftMaxHealthMap = ((other.CharacterLeftMaxHealthMap == null) ? null : new Dictionary<int, short>(other.CharacterLeftMaxHealthMap));
	}

	public void Assign(LegendaryBookOwnerData other)
	{
		BookMap = ((other.BookMap == null) ? null : new Dictionary<sbyte, int>(other.BookMap));
		if (other.CharacterDisplayDataMap != null)
		{
			Dictionary<int, CharacterDisplayData> characterDisplayDataMap = other.CharacterDisplayDataMap;
			int count = characterDisplayDataMap.Count;
			CharacterDisplayDataMap = new Dictionary<int, CharacterDisplayData>(count);
			foreach (KeyValuePair<int, CharacterDisplayData> item in characterDisplayDataMap)
			{
				CharacterDisplayDataMap.Add(item.Key, new CharacterDisplayData(item.Value));
			}
		}
		else
		{
			CharacterDisplayDataMap = null;
		}
		CharacterHappinessMap = ((other.CharacterHappinessMap == null) ? null : new Dictionary<int, sbyte>(other.CharacterHappinessMap));
		CharacterAttributeMap = ((other.CharacterAttributeMap == null) ? null : new Dictionary<int, MainAttributes>(other.CharacterAttributeMap));
		CharacterHealthMap = ((other.CharacterHealthMap == null) ? null : new Dictionary<int, short>(other.CharacterHealthMap));
		CharacterLeftMaxHealthMap = ((other.CharacterLeftMaxHealthMap == null) ? null : new Dictionary<int, short>(other.CharacterLeftMaxHealthMap));
	}

	public bool IsSerializedSizeFixed()
	{
		return false;
	}

	public int GetSerializedSize()
	{
		int num = 0;
		num += DictionaryOfBasicTypePair.GetSerializedSize<sbyte, int>((IReadOnlyDictionary<sbyte, int>)BookMap);
		num += DictionaryOfBasicTypeCustomTypePair.GetSerializedSize<int, CharacterDisplayData>(CharacterDisplayDataMap);
		num += DictionaryOfBasicTypePair.GetSerializedSize<int, sbyte>((IReadOnlyDictionary<int, sbyte>)CharacterHappinessMap);
		num += DictionaryOfBasicTypeCustomTypePair.GetSerializedSize<int, MainAttributes>(CharacterAttributeMap);
		num += DictionaryOfBasicTypePair.GetSerializedSize<int, short>((IReadOnlyDictionary<int, short>)CharacterHealthMap);
		num += DictionaryOfBasicTypePair.GetSerializedSize<int, short>((IReadOnlyDictionary<int, short>)CharacterLeftMaxHealthMap);
		if (num > 4)
		{
			return (num + 3) / 4 * 4;
		}
		return num;
	}

	public unsafe int Serialize(byte* pData)
	{
		byte* num = pData + DictionaryOfBasicTypePair.Serialize<sbyte, int>(pData, ref BookMap);
		byte* num2 = num + DictionaryOfBasicTypeCustomTypePair.Serialize<int, CharacterDisplayData>(num, ref CharacterDisplayDataMap);
		byte* num3 = num2 + DictionaryOfBasicTypePair.Serialize<int, sbyte>(num2, ref CharacterHappinessMap);
		byte* num4 = num3 + DictionaryOfBasicTypeCustomTypePair.Serialize<int, MainAttributes>(num3, ref CharacterAttributeMap);
		byte* num5 = num4 + DictionaryOfBasicTypePair.Serialize<int, short>(num4, ref CharacterHealthMap);
		int num6 = (int)(num5 + DictionaryOfBasicTypePair.Serialize<int, short>(num5, ref CharacterLeftMaxHealthMap) - pData);
		if (num6 > 4)
		{
			return (num6 + 3) / 4 * 4;
		}
		return num6;
	}

	public unsafe int Deserialize(byte* pData)
	{
		byte* num = pData + DictionaryOfBasicTypePair.Deserialize<sbyte, int>(pData, ref BookMap);
		byte* num2 = num + DictionaryOfBasicTypeCustomTypePair.Deserialize<int, CharacterDisplayData>(num, ref CharacterDisplayDataMap);
		byte* num3 = num2 + DictionaryOfBasicTypePair.Deserialize<int, sbyte>(num2, ref CharacterHappinessMap);
		byte* num4 = num3 + DictionaryOfBasicTypeCustomTypePair.Deserialize<int, MainAttributes>(num3, ref CharacterAttributeMap);
		byte* num5 = num4 + DictionaryOfBasicTypePair.Deserialize<int, short>(num4, ref CharacterHealthMap);
		int num6 = (int)(num5 + DictionaryOfBasicTypePair.Deserialize<int, short>(num5, ref CharacterLeftMaxHealthMap) - pData);
		if (num6 > 4)
		{
			return (num6 + 3) / 4 * 4;
		}
		return num6;
	}
}
