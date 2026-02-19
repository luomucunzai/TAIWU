using System.Collections.Generic;
using GameData.Domains.Character.Display;
using GameData.Domains.Item;
using GameData.Domains.Item.Display;
using GameData.Serializer;
using GameData.Utilities;

namespace GameData.Domains.TaiwuEvent.DisplayEvent;

[SerializableGameData(NoCopyConstructors = true)]
public class EventCricketBettingData : ISerializableGameData
{
	public bool IsComplete;

	public bool IsConfirmed;

	public string SelectForEventGuid;

	public string SelectForOptionKey;

	public Wager Wager;

	public int Index;

	[SerializableGameDataField]
	public bool IsValid;

	[SerializableGameDataField]
	public List<CricketWagerData> BetRewards;

	[SerializableGameDataField]
	public List<ItemDisplayData> BetItems;

	[SerializableGameDataField]
	public List<CharacterDisplayData> BetCharacters;

	[SerializableGameDataField]
	public Dictionary<int, long> BetCharacterValueMap;

	public EventCricketBettingData()
	{
		IsValid = false;
		IsComplete = false;
		IsConfirmed = false;
		BetCharacterValueMap = new Dictionary<int, long>();
	}

	public bool IsSerializedSizeFixed()
	{
		return false;
	}

	public int GetSerializedSize()
	{
		int num = 1;
		if (BetRewards != null)
		{
			num += 2;
			int count = BetRewards.Count;
			for (int i = 0; i < count; i++)
			{
				CricketWagerData cricketWagerData = BetRewards[i];
				num = ((cricketWagerData == null) ? (num + 2) : (num + (2 + cricketWagerData.GetSerializedSize())));
			}
		}
		else
		{
			num += 2;
		}
		if (BetItems != null)
		{
			num += 2;
			int count2 = BetItems.Count;
			for (int j = 0; j < count2; j++)
			{
				ItemDisplayData itemDisplayData = BetItems[j];
				num = ((itemDisplayData == null) ? (num + 2) : (num + (2 + itemDisplayData.GetSerializedSize())));
			}
		}
		else
		{
			num += 2;
		}
		if (BetCharacters != null)
		{
			num += 2;
			int count3 = BetCharacters.Count;
			for (int k = 0; k < count3; k++)
			{
				CharacterDisplayData characterDisplayData = BetCharacters[k];
				num = ((characterDisplayData == null) ? (num + 2) : (num + (2 + characterDisplayData.GetSerializedSize())));
			}
		}
		else
		{
			num += 2;
		}
		num += DictionaryOfBasicTypePair.GetSerializedSize<int, long>((IReadOnlyDictionary<int, long>)BetCharacterValueMap);
		if (num > 4)
		{
			return (num + 3) / 4 * 4;
		}
		return num;
	}

	public unsafe int Serialize(byte* pData)
	{
		byte* ptr = pData;
		*ptr = (IsValid ? ((byte)1) : ((byte)0));
		ptr++;
		if (BetRewards != null)
		{
			int count = BetRewards.Count;
			Tester.Assert(count <= 65535);
			*(ushort*)ptr = (ushort)count;
			ptr += 2;
			for (int i = 0; i < count; i++)
			{
				CricketWagerData cricketWagerData = BetRewards[i];
				if (cricketWagerData != null)
				{
					byte* intPtr = ptr;
					ptr += 2;
					int num = cricketWagerData.Serialize(ptr);
					ptr += num;
					Tester.Assert(num <= 65535);
					*(ushort*)intPtr = (ushort)num;
				}
				else
				{
					*(short*)ptr = 0;
					ptr += 2;
				}
			}
		}
		else
		{
			*(short*)ptr = 0;
			ptr += 2;
		}
		if (BetItems != null)
		{
			int count2 = BetItems.Count;
			Tester.Assert(count2 <= 65535);
			*(ushort*)ptr = (ushort)count2;
			ptr += 2;
			for (int j = 0; j < count2; j++)
			{
				ItemDisplayData itemDisplayData = BetItems[j];
				if (itemDisplayData != null)
				{
					byte* intPtr2 = ptr;
					ptr += 2;
					int num2 = itemDisplayData.Serialize(ptr);
					ptr += num2;
					Tester.Assert(num2 <= 65535);
					*(ushort*)intPtr2 = (ushort)num2;
				}
				else
				{
					*(short*)ptr = 0;
					ptr += 2;
				}
			}
		}
		else
		{
			*(short*)ptr = 0;
			ptr += 2;
		}
		if (BetCharacters != null)
		{
			int count3 = BetCharacters.Count;
			Tester.Assert(count3 <= 65535);
			*(ushort*)ptr = (ushort)count3;
			ptr += 2;
			for (int k = 0; k < count3; k++)
			{
				CharacterDisplayData characterDisplayData = BetCharacters[k];
				if (characterDisplayData != null)
				{
					byte* intPtr3 = ptr;
					ptr += 2;
					int num3 = characterDisplayData.Serialize(ptr);
					ptr += num3;
					Tester.Assert(num3 <= 65535);
					*(ushort*)intPtr3 = (ushort)num3;
				}
				else
				{
					*(short*)ptr = 0;
					ptr += 2;
				}
			}
		}
		else
		{
			*(short*)ptr = 0;
			ptr += 2;
		}
		ptr += DictionaryOfBasicTypePair.Serialize<int, long>(ptr, ref BetCharacterValueMap);
		int num4 = (int)(ptr - pData);
		if (num4 > 4)
		{
			return (num4 + 3) / 4 * 4;
		}
		return num4;
	}

	public unsafe int Deserialize(byte* pData)
	{
		byte* ptr = pData;
		IsValid = *ptr != 0;
		ptr++;
		ushort num = *(ushort*)ptr;
		ptr += 2;
		if (num > 0)
		{
			if (BetRewards == null)
			{
				BetRewards = new List<CricketWagerData>(num);
			}
			else
			{
				BetRewards.Clear();
			}
			for (int i = 0; i < num; i++)
			{
				ushort num2 = *(ushort*)ptr;
				ptr += 2;
				if (num2 > 0)
				{
					CricketWagerData cricketWagerData = new CricketWagerData();
					ptr += cricketWagerData.Deserialize(ptr);
					BetRewards.Add(cricketWagerData);
				}
				else
				{
					BetRewards.Add(null);
				}
			}
		}
		else
		{
			BetRewards?.Clear();
		}
		ushort num3 = *(ushort*)ptr;
		ptr += 2;
		if (num3 > 0)
		{
			if (BetItems == null)
			{
				BetItems = new List<ItemDisplayData>(num3);
			}
			else
			{
				BetItems.Clear();
			}
			for (int j = 0; j < num3; j++)
			{
				ushort num4 = *(ushort*)ptr;
				ptr += 2;
				if (num4 > 0)
				{
					ItemDisplayData itemDisplayData = new ItemDisplayData();
					ptr += itemDisplayData.Deserialize(ptr);
					BetItems.Add(itemDisplayData);
				}
				else
				{
					BetItems.Add(null);
				}
			}
		}
		else
		{
			BetItems?.Clear();
		}
		ushort num5 = *(ushort*)ptr;
		ptr += 2;
		if (num5 > 0)
		{
			if (BetCharacters == null)
			{
				BetCharacters = new List<CharacterDisplayData>(num5);
			}
			else
			{
				BetCharacters.Clear();
			}
			for (int k = 0; k < num5; k++)
			{
				ushort num6 = *(ushort*)ptr;
				ptr += 2;
				if (num6 > 0)
				{
					CharacterDisplayData characterDisplayData = new CharacterDisplayData();
					ptr += characterDisplayData.Deserialize(ptr);
					BetCharacters.Add(characterDisplayData);
				}
				else
				{
					BetCharacters.Add(null);
				}
			}
		}
		else
		{
			BetCharacters?.Clear();
		}
		ptr += DictionaryOfBasicTypePair.Deserialize<int, long>(ptr, ref BetCharacterValueMap);
		int num7 = (int)(ptr - pData);
		if (num7 > 4)
		{
			return (num7 + 3) / 4 * 4;
		}
		return num7;
	}
}
