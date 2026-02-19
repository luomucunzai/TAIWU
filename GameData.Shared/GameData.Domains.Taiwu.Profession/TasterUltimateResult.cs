using System;
using System.Collections.Generic;
using GameData.Domains.Character.Display;
using GameData.Serializer;
using GameData.Utilities;

namespace GameData.Domains.Taiwu.Profession;

[SerializableGameData]
public class TasterUltimateResult : ISerializableGameData
{
	[SerializableGameDataField]
	public Dictionary<int, CharacterDisplayData> Characters = new Dictionary<int, CharacterDisplayData>();

	[SerializableGameDataField]
	public List<short> Books = new List<short>();

	[Obsolete]
	[SerializableGameDataField]
	public Dictionary<IntPair, IntPair> PracticeLevelData = new Dictionary<IntPair, IntPair>();

	[SerializableGameDataField]
	public Dictionary<IntPair, byte> ReadBookPageData = new Dictionary<IntPair, byte>();

	[SerializableGameDataField]
	public Dictionary<IntPair, bool> FavorabilityChangeData = new Dictionary<IntPair, bool>();

	[SerializableGameDataField]
	public Dictionary<IntPair, ushort> RelationChangeData = new Dictionary<IntPair, ushort>();

	public TasterUltimateResult()
	{
	}

	public TasterUltimateResult(TasterUltimateResult other)
	{
		if (other.Characters != null)
		{
			Dictionary<int, CharacterDisplayData> characters = other.Characters;
			int count = characters.Count;
			Characters = new Dictionary<int, CharacterDisplayData>(count);
			foreach (KeyValuePair<int, CharacterDisplayData> item in characters)
			{
				Characters.Add(item.Key, new CharacterDisplayData(item.Value));
			}
		}
		else
		{
			Characters = null;
		}
		Books = ((other.Books == null) ? null : new List<short>(other.Books));
		PracticeLevelData = ((other.PracticeLevelData == null) ? null : new Dictionary<IntPair, IntPair>(other.PracticeLevelData));
		ReadBookPageData = ((other.ReadBookPageData == null) ? null : new Dictionary<IntPair, byte>(other.ReadBookPageData));
		FavorabilityChangeData = ((other.FavorabilityChangeData == null) ? null : new Dictionary<IntPair, bool>(other.FavorabilityChangeData));
		RelationChangeData = ((other.RelationChangeData == null) ? null : new Dictionary<IntPair, ushort>(other.RelationChangeData));
	}

	public void Assign(TasterUltimateResult other)
	{
		if (other.Characters != null)
		{
			Dictionary<int, CharacterDisplayData> characters = other.Characters;
			int count = characters.Count;
			Characters = new Dictionary<int, CharacterDisplayData>(count);
			foreach (KeyValuePair<int, CharacterDisplayData> item in characters)
			{
				Characters.Add(item.Key, new CharacterDisplayData(item.Value));
			}
		}
		else
		{
			Characters = null;
		}
		Books = ((other.Books == null) ? null : new List<short>(other.Books));
		PracticeLevelData = ((other.PracticeLevelData == null) ? null : new Dictionary<IntPair, IntPair>(other.PracticeLevelData));
		ReadBookPageData = ((other.ReadBookPageData == null) ? null : new Dictionary<IntPair, byte>(other.ReadBookPageData));
		FavorabilityChangeData = ((other.FavorabilityChangeData == null) ? null : new Dictionary<IntPair, bool>(other.FavorabilityChangeData));
		RelationChangeData = ((other.RelationChangeData == null) ? null : new Dictionary<IntPair, ushort>(other.RelationChangeData));
	}

	public bool IsSerializedSizeFixed()
	{
		return false;
	}

	public int GetSerializedSize()
	{
		int num = 0;
		num += DictionaryOfBasicTypeCustomTypePair.GetSerializedSize<int, CharacterDisplayData>(Characters);
		num = ((Books == null) ? (num + 2) : (num + (2 + 2 * Books.Count)));
		num += DictionaryOfCustomTypePair.GetSerializedSize<IntPair, IntPair>(PracticeLevelData);
		num += DictionaryOfCustomTypeBasicTypePair.GetSerializedSize<IntPair, byte>(ReadBookPageData);
		num += DictionaryOfCustomTypeBasicTypePair.GetSerializedSize<IntPair, bool>(FavorabilityChangeData);
		num += DictionaryOfCustomTypeBasicTypePair.GetSerializedSize<IntPair, ushort>(RelationChangeData);
		if (num > 4)
		{
			return (num + 3) / 4 * 4;
		}
		return num;
	}

	public unsafe int Serialize(byte* pData)
	{
		byte* ptr = pData;
		ptr += DictionaryOfBasicTypeCustomTypePair.Serialize<int, CharacterDisplayData>(ptr, ref Characters);
		if (Books != null)
		{
			int count = Books.Count;
			Tester.Assert(count <= 65535);
			*(ushort*)ptr = (ushort)count;
			ptr += 2;
			for (int i = 0; i < count; i++)
			{
				((short*)ptr)[i] = Books[i];
			}
			ptr += 2 * count;
		}
		else
		{
			*(short*)ptr = 0;
			ptr += 2;
		}
		ptr += DictionaryOfCustomTypePair.Serialize<IntPair, IntPair>(ptr, ref PracticeLevelData);
		ptr += DictionaryOfCustomTypeBasicTypePair.Serialize<IntPair, byte>(ptr, ref ReadBookPageData);
		ptr += DictionaryOfCustomTypeBasicTypePair.Serialize<IntPair, bool>(ptr, ref FavorabilityChangeData);
		ptr += DictionaryOfCustomTypeBasicTypePair.Serialize<IntPair, ushort>(ptr, ref RelationChangeData);
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
		ptr += DictionaryOfBasicTypeCustomTypePair.Deserialize<int, CharacterDisplayData>(ptr, ref Characters);
		ushort num = *(ushort*)ptr;
		ptr += 2;
		if (num > 0)
		{
			if (Books == null)
			{
				Books = new List<short>(num);
			}
			else
			{
				Books.Clear();
			}
			for (int i = 0; i < num; i++)
			{
				Books.Add(((short*)ptr)[i]);
			}
			ptr += 2 * num;
		}
		else
		{
			Books?.Clear();
		}
		ptr += DictionaryOfCustomTypePair.Deserialize<IntPair, IntPair>(ptr, ref PracticeLevelData);
		ptr += DictionaryOfCustomTypeBasicTypePair.Deserialize<IntPair, byte>(ptr, ref ReadBookPageData);
		ptr += DictionaryOfCustomTypeBasicTypePair.Deserialize<IntPair, bool>(ptr, ref FavorabilityChangeData);
		ptr += DictionaryOfCustomTypeBasicTypePair.Deserialize<IntPair, ushort>(ptr, ref RelationChangeData);
		int num2 = (int)(ptr - pData);
		if (num2 > 4)
		{
			return (num2 + 3) / 4 * 4;
		}
		return num2;
	}
}
