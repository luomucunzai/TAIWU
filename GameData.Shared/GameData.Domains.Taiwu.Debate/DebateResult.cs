using System.Collections.Generic;
using GameData.Domains.Character.Display;
using GameData.Serializer;
using GameData.Utilities;

namespace GameData.Domains.Taiwu.Debate;

public class DebateResult : ISerializableGameData
{
	[SerializableGameDataField]
	public bool IsTaiwuWin;

	[SerializableGameDataField]
	public IntPair Exp = new IntPair(0, 0);

	[SerializableGameDataField]
	public IntPair Authority = new IntPair(0, 0);

	[SerializableGameDataField]
	public bool ShowReadingEvent;

	[SerializableGameDataField]
	public bool ShowReadingEvent2;

	[SerializableGameDataField]
	public bool ShowLoopingEvent;

	[SerializableGameDataField]
	public bool ShowLoopingEvent2;

	[SerializableGameDataField]
	public List<short> Evaluations = new List<short>();

	[SerializableGameDataField]
	public Dictionary<short, int> TaiwuComments = new Dictionary<short, int>();

	[SerializableGameDataField]
	public Dictionary<short, int> NpcComments = new Dictionary<short, int>();

	[SerializableGameDataField]
	public Dictionary<int, IntPair> Favorability = new Dictionary<int, IntPair>();

	[SerializableGameDataField]
	public Dictionary<int, IntPair> Happiness = new Dictionary<int, IntPair>();

	[SerializableGameDataField]
	public Dictionary<int, CharacterDisplayData> CharacterDisplayDataMap = new Dictionary<int, CharacterDisplayData>();

	public int ExpA;

	public int ExpB = 100;

	public int ExpCMax;

	public int ExpCMin;

	public int AuthorityA;

	public int AuthorityB = 100;

	public int AuthorityCMax;

	public int AuthorityCMin;

	public int FavorA;

	public int FavorIncreaseB = 100;

	public int FavorDecreaseB = 100;

	public int FavorIncreaseCMax;

	public int FavorIncreaseCMin;

	public int FavorDecreaseCMax;

	public int FavorDecreaseCMin;

	public int ReadRate;

	public int LoopRate;

	public int HappinessDelta;

	public int GetHappiness(int charId)
	{
		if (!Happiness.TryGetValue(charId, out var value))
		{
			return 0;
		}
		return value.First;
	}

	public void AddHappiness(int charId, int delta)
	{
		Happiness[charId] = new IntPair(GetHappiness(charId) + delta, 0);
	}

	public DebateResult()
	{
	}

	public DebateResult(DebateResult other)
	{
		IsTaiwuWin = other.IsTaiwuWin;
		Exp = other.Exp;
		Authority = other.Authority;
		ShowReadingEvent = other.ShowReadingEvent;
		ShowReadingEvent2 = other.ShowReadingEvent2;
		ShowLoopingEvent = other.ShowLoopingEvent;
		ShowLoopingEvent2 = other.ShowLoopingEvent2;
		Evaluations = ((other.Evaluations == null) ? null : new List<short>(other.Evaluations));
		TaiwuComments = ((other.TaiwuComments == null) ? null : new Dictionary<short, int>(other.TaiwuComments));
		NpcComments = ((other.NpcComments == null) ? null : new Dictionary<short, int>(other.NpcComments));
		Favorability = ((other.Favorability == null) ? null : new Dictionary<int, IntPair>(other.Favorability));
		Happiness = ((other.Happiness == null) ? null : new Dictionary<int, IntPair>(other.Happiness));
		if (other.CharacterDisplayDataMap != null)
		{
			Dictionary<int, CharacterDisplayData> characterDisplayDataMap = other.CharacterDisplayDataMap;
			int count = characterDisplayDataMap.Count;
			CharacterDisplayDataMap = new Dictionary<int, CharacterDisplayData>(count);
			{
				foreach (KeyValuePair<int, CharacterDisplayData> item in characterDisplayDataMap)
				{
					CharacterDisplayDataMap.Add(item.Key, new CharacterDisplayData(item.Value));
				}
				return;
			}
		}
		CharacterDisplayDataMap = null;
	}

	public void Assign(DebateResult other)
	{
		IsTaiwuWin = other.IsTaiwuWin;
		Exp = other.Exp;
		Authority = other.Authority;
		ShowReadingEvent = other.ShowReadingEvent;
		ShowReadingEvent2 = other.ShowReadingEvent2;
		ShowLoopingEvent = other.ShowLoopingEvent;
		ShowLoopingEvent2 = other.ShowLoopingEvent2;
		Evaluations = ((other.Evaluations == null) ? null : new List<short>(other.Evaluations));
		TaiwuComments = ((other.TaiwuComments == null) ? null : new Dictionary<short, int>(other.TaiwuComments));
		NpcComments = ((other.NpcComments == null) ? null : new Dictionary<short, int>(other.NpcComments));
		Favorability = ((other.Favorability == null) ? null : new Dictionary<int, IntPair>(other.Favorability));
		Happiness = ((other.Happiness == null) ? null : new Dictionary<int, IntPair>(other.Happiness));
		if (other.CharacterDisplayDataMap != null)
		{
			Dictionary<int, CharacterDisplayData> characterDisplayDataMap = other.CharacterDisplayDataMap;
			int count = characterDisplayDataMap.Count;
			CharacterDisplayDataMap = new Dictionary<int, CharacterDisplayData>(count);
			{
				foreach (KeyValuePair<int, CharacterDisplayData> item in characterDisplayDataMap)
				{
					CharacterDisplayDataMap.Add(item.Key, new CharacterDisplayData(item.Value));
				}
				return;
			}
		}
		CharacterDisplayDataMap = null;
	}

	public bool IsSerializedSizeFixed()
	{
		return false;
	}

	public int GetSerializedSize()
	{
		int num = 21;
		num = ((Evaluations == null) ? (num + 2) : (num + (2 + 2 * Evaluations.Count)));
		num += DictionaryOfBasicTypePair.GetSerializedSize<short, int>((IReadOnlyDictionary<short, int>)TaiwuComments);
		num += DictionaryOfBasicTypePair.GetSerializedSize<short, int>((IReadOnlyDictionary<short, int>)NpcComments);
		num += DictionaryOfBasicTypeCustomTypePair.GetSerializedSize<int, IntPair>(Favorability);
		num += DictionaryOfBasicTypeCustomTypePair.GetSerializedSize<int, IntPair>(Happiness);
		num += DictionaryOfBasicTypeCustomTypePair.GetSerializedSize<int, CharacterDisplayData>(CharacterDisplayDataMap);
		if (num > 4)
		{
			return (num + 3) / 4 * 4;
		}
		return num;
	}

	public unsafe int Serialize(byte* pData)
	{
		byte* ptr = pData;
		*ptr = (IsTaiwuWin ? ((byte)1) : ((byte)0));
		ptr++;
		ptr += Exp.Serialize(ptr);
		ptr += Authority.Serialize(ptr);
		*ptr = (ShowReadingEvent ? ((byte)1) : ((byte)0));
		ptr++;
		*ptr = (ShowReadingEvent2 ? ((byte)1) : ((byte)0));
		ptr++;
		*ptr = (ShowLoopingEvent ? ((byte)1) : ((byte)0));
		ptr++;
		*ptr = (ShowLoopingEvent2 ? ((byte)1) : ((byte)0));
		ptr++;
		if (Evaluations != null)
		{
			int count = Evaluations.Count;
			Tester.Assert(count <= 65535);
			*(ushort*)ptr = (ushort)count;
			ptr += 2;
			for (int i = 0; i < count; i++)
			{
				((short*)ptr)[i] = Evaluations[i];
			}
			ptr += 2 * count;
		}
		else
		{
			*(short*)ptr = 0;
			ptr += 2;
		}
		ptr += DictionaryOfBasicTypePair.Serialize<short, int>(ptr, ref TaiwuComments);
		ptr += DictionaryOfBasicTypePair.Serialize<short, int>(ptr, ref NpcComments);
		ptr += DictionaryOfBasicTypeCustomTypePair.Serialize<int, IntPair>(ptr, ref Favorability);
		ptr += DictionaryOfBasicTypeCustomTypePair.Serialize<int, IntPair>(ptr, ref Happiness);
		ptr += DictionaryOfBasicTypeCustomTypePair.Serialize<int, CharacterDisplayData>(ptr, ref CharacterDisplayDataMap);
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
		IsTaiwuWin = *ptr != 0;
		ptr++;
		ptr += Exp.Deserialize(ptr);
		ptr += Authority.Deserialize(ptr);
		ShowReadingEvent = *ptr != 0;
		ptr++;
		ShowReadingEvent2 = *ptr != 0;
		ptr++;
		ShowLoopingEvent = *ptr != 0;
		ptr++;
		ShowLoopingEvent2 = *ptr != 0;
		ptr++;
		ushort num = *(ushort*)ptr;
		ptr += 2;
		if (num > 0)
		{
			if (Evaluations == null)
			{
				Evaluations = new List<short>(num);
			}
			else
			{
				Evaluations.Clear();
			}
			for (int i = 0; i < num; i++)
			{
				Evaluations.Add(((short*)ptr)[i]);
			}
			ptr += 2 * num;
		}
		else
		{
			Evaluations?.Clear();
		}
		ptr += DictionaryOfBasicTypePair.Deserialize<short, int>(ptr, ref TaiwuComments);
		ptr += DictionaryOfBasicTypePair.Deserialize<short, int>(ptr, ref NpcComments);
		ptr += DictionaryOfBasicTypeCustomTypePair.Deserialize<int, IntPair>(ptr, ref Favorability);
		ptr += DictionaryOfBasicTypeCustomTypePair.Deserialize<int, IntPair>(ptr, ref Happiness);
		ptr += DictionaryOfBasicTypeCustomTypePair.Deserialize<int, CharacterDisplayData>(ptr, ref CharacterDisplayDataMap);
		int num2 = (int)(ptr - pData);
		if (num2 > 4)
		{
			return (num2 + 3) / 4 * 4;
		}
		return num2;
	}
}
