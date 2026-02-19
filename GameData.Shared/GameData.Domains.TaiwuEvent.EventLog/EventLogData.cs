using System.Collections.Generic;
using GameData.Domains.Character.Display;
using GameData.Domains.CombatSkill;
using GameData.Domains.Information;
using GameData.Domains.Item.Display;
using GameData.Serializer;
using GameData.Utilities;

namespace GameData.Domains.TaiwuEvent.EventLog;

public class EventLogData : ISerializableGameData
{
	[SerializableGameDataField]
	public List<CharacterDisplayData> CharacterList;

	[SerializableGameDataField]
	public List<SecretInformationDisplayData> SecretInformationList;

	[SerializableGameDataField]
	public List<ItemDisplayData> ItemList;

	[SerializableGameDataField]
	public List<CombatSkillDisplayData> CombatSkillList;

	[SerializableGameDataField]
	public List<EventLogResultData> ResultList;

	public EventLogData()
	{
		CharacterList = new List<CharacterDisplayData>();
		ResultList = new List<EventLogResultData>();
		SecretInformationList = new List<SecretInformationDisplayData>();
		ItemList = new List<ItemDisplayData>();
		CombatSkillList = new List<CombatSkillDisplayData>();
	}

	public EventLogData(List<CharacterDisplayData> characterList, List<EventLogResultData> resultList, List<SecretInformationDisplayData> secretInformationList, List<ItemDisplayData> itemList, List<CombatSkillDisplayData> combatSkillList)
	{
		CharacterList = characterList;
		ResultList = resultList;
		SecretInformationList = secretInformationList;
		ItemList = itemList;
		CombatSkillList = combatSkillList;
	}

	public bool IsSerializedSizeFixed()
	{
		return false;
	}

	public int GetSerializedSize()
	{
		int num = 0;
		if (CharacterList != null)
		{
			num += 2;
			int count = CharacterList.Count;
			for (int i = 0; i < count; i++)
			{
				CharacterDisplayData characterDisplayData = CharacterList[i];
				num = ((characterDisplayData == null) ? (num + 2) : (num + (2 + characterDisplayData.GetSerializedSize())));
			}
		}
		else
		{
			num += 2;
		}
		if (SecretInformationList != null)
		{
			num += 2;
			int count2 = SecretInformationList.Count;
			for (int j = 0; j < count2; j++)
			{
				SecretInformationDisplayData secretInformationDisplayData = SecretInformationList[j];
				num = ((secretInformationDisplayData == null) ? (num + 2) : (num + (2 + secretInformationDisplayData.GetSerializedSize())));
			}
		}
		else
		{
			num += 2;
		}
		if (ItemList != null)
		{
			num += 2;
			int count3 = ItemList.Count;
			for (int k = 0; k < count3; k++)
			{
				ItemDisplayData itemDisplayData = ItemList[k];
				num = ((itemDisplayData == null) ? (num + 2) : (num + (2 + itemDisplayData.GetSerializedSize())));
			}
		}
		else
		{
			num += 2;
		}
		if (CombatSkillList != null)
		{
			num += 2;
			int count4 = CombatSkillList.Count;
			for (int l = 0; l < count4; l++)
			{
				CombatSkillDisplayData combatSkillDisplayData = CombatSkillList[l];
				num = ((combatSkillDisplayData == null) ? (num + 2) : (num + (2 + combatSkillDisplayData.GetSerializedSize())));
			}
		}
		else
		{
			num += 2;
		}
		if (ResultList != null)
		{
			num += 2;
			int count5 = ResultList.Count;
			for (int m = 0; m < count5; m++)
			{
				EventLogResultData eventLogResultData = ResultList[m];
				num = ((eventLogResultData == null) ? (num + 2) : (num + (2 + eventLogResultData.GetSerializedSize())));
			}
		}
		else
		{
			num += 2;
		}
		if (num > 4)
		{
			return (num + 3) / 4 * 4;
		}
		return num;
	}

	public unsafe int Serialize(byte* pData)
	{
		byte* ptr = pData;
		if (CharacterList != null)
		{
			int count = CharacterList.Count;
			Tester.Assert(count <= 65535);
			*(ushort*)ptr = (ushort)count;
			ptr += 2;
			for (int i = 0; i < count; i++)
			{
				CharacterDisplayData characterDisplayData = CharacterList[i];
				if (characterDisplayData != null)
				{
					byte* intPtr = ptr;
					ptr += 2;
					int num = characterDisplayData.Serialize(ptr);
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
		if (SecretInformationList != null)
		{
			int count2 = SecretInformationList.Count;
			Tester.Assert(count2 <= 65535);
			*(ushort*)ptr = (ushort)count2;
			ptr += 2;
			for (int j = 0; j < count2; j++)
			{
				SecretInformationDisplayData secretInformationDisplayData = SecretInformationList[j];
				if (secretInformationDisplayData != null)
				{
					byte* intPtr2 = ptr;
					ptr += 2;
					int num2 = secretInformationDisplayData.Serialize(ptr);
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
		if (ItemList != null)
		{
			int count3 = ItemList.Count;
			Tester.Assert(count3 <= 65535);
			*(ushort*)ptr = (ushort)count3;
			ptr += 2;
			for (int k = 0; k < count3; k++)
			{
				ItemDisplayData itemDisplayData = ItemList[k];
				if (itemDisplayData != null)
				{
					byte* intPtr3 = ptr;
					ptr += 2;
					int num3 = itemDisplayData.Serialize(ptr);
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
		if (CombatSkillList != null)
		{
			int count4 = CombatSkillList.Count;
			Tester.Assert(count4 <= 65535);
			*(ushort*)ptr = (ushort)count4;
			ptr += 2;
			for (int l = 0; l < count4; l++)
			{
				CombatSkillDisplayData combatSkillDisplayData = CombatSkillList[l];
				if (combatSkillDisplayData != null)
				{
					byte* intPtr4 = ptr;
					ptr += 2;
					int num4 = combatSkillDisplayData.Serialize(ptr);
					ptr += num4;
					Tester.Assert(num4 <= 65535);
					*(ushort*)intPtr4 = (ushort)num4;
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
		if (ResultList != null)
		{
			int count5 = ResultList.Count;
			Tester.Assert(count5 <= 65535);
			*(ushort*)ptr = (ushort)count5;
			ptr += 2;
			for (int m = 0; m < count5; m++)
			{
				EventLogResultData eventLogResultData = ResultList[m];
				if (eventLogResultData != null)
				{
					byte* intPtr5 = ptr;
					ptr += 2;
					int num5 = eventLogResultData.Serialize(ptr);
					ptr += num5;
					Tester.Assert(num5 <= 65535);
					*(ushort*)intPtr5 = (ushort)num5;
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
		int num6 = (int)(ptr - pData);
		if (num6 > 4)
		{
			return (num6 + 3) / 4 * 4;
		}
		return num6;
	}

	public unsafe int Deserialize(byte* pData)
	{
		byte* ptr = pData;
		ushort num = *(ushort*)ptr;
		ptr += 2;
		if (num > 0)
		{
			if (CharacterList == null)
			{
				CharacterList = new List<CharacterDisplayData>(num);
			}
			else
			{
				CharacterList.Clear();
			}
			for (int i = 0; i < num; i++)
			{
				ushort num2 = *(ushort*)ptr;
				ptr += 2;
				if (num2 > 0)
				{
					CharacterDisplayData characterDisplayData = new CharacterDisplayData();
					ptr += characterDisplayData.Deserialize(ptr);
					CharacterList.Add(characterDisplayData);
				}
				else
				{
					CharacterList.Add(null);
				}
			}
		}
		else
		{
			CharacterList?.Clear();
		}
		ushort num3 = *(ushort*)ptr;
		ptr += 2;
		if (num3 > 0)
		{
			if (SecretInformationList == null)
			{
				SecretInformationList = new List<SecretInformationDisplayData>(num3);
			}
			else
			{
				SecretInformationList.Clear();
			}
			for (int j = 0; j < num3; j++)
			{
				ushort num4 = *(ushort*)ptr;
				ptr += 2;
				if (num4 > 0)
				{
					SecretInformationDisplayData secretInformationDisplayData = new SecretInformationDisplayData();
					ptr += secretInformationDisplayData.Deserialize(ptr);
					SecretInformationList.Add(secretInformationDisplayData);
				}
				else
				{
					SecretInformationList.Add(null);
				}
			}
		}
		else
		{
			SecretInformationList?.Clear();
		}
		ushort num5 = *(ushort*)ptr;
		ptr += 2;
		if (num5 > 0)
		{
			if (ItemList == null)
			{
				ItemList = new List<ItemDisplayData>(num5);
			}
			else
			{
				ItemList.Clear();
			}
			for (int k = 0; k < num5; k++)
			{
				ushort num6 = *(ushort*)ptr;
				ptr += 2;
				if (num6 > 0)
				{
					ItemDisplayData itemDisplayData = new ItemDisplayData();
					ptr += itemDisplayData.Deserialize(ptr);
					ItemList.Add(itemDisplayData);
				}
				else
				{
					ItemList.Add(null);
				}
			}
		}
		else
		{
			ItemList?.Clear();
		}
		ushort num7 = *(ushort*)ptr;
		ptr += 2;
		if (num7 > 0)
		{
			if (CombatSkillList == null)
			{
				CombatSkillList = new List<CombatSkillDisplayData>(num7);
			}
			else
			{
				CombatSkillList.Clear();
			}
			for (int l = 0; l < num7; l++)
			{
				ushort num8 = *(ushort*)ptr;
				ptr += 2;
				if (num8 > 0)
				{
					CombatSkillDisplayData combatSkillDisplayData = new CombatSkillDisplayData();
					ptr += combatSkillDisplayData.Deserialize(ptr);
					CombatSkillList.Add(combatSkillDisplayData);
				}
				else
				{
					CombatSkillList.Add(null);
				}
			}
		}
		else
		{
			CombatSkillList?.Clear();
		}
		ushort num9 = *(ushort*)ptr;
		ptr += 2;
		if (num9 > 0)
		{
			if (ResultList == null)
			{
				ResultList = new List<EventLogResultData>(num9);
			}
			else
			{
				ResultList.Clear();
			}
			for (int m = 0; m < num9; m++)
			{
				ushort num10 = *(ushort*)ptr;
				ptr += 2;
				if (num10 > 0)
				{
					EventLogResultData eventLogResultData = new EventLogResultData();
					ptr += eventLogResultData.Deserialize(ptr);
					ResultList.Add(eventLogResultData);
				}
				else
				{
					ResultList.Add(null);
				}
			}
		}
		else
		{
			ResultList?.Clear();
		}
		int num11 = (int)(ptr - pData);
		if (num11 > 4)
		{
			return (num11 + 3) / 4 * 4;
		}
		return num11;
	}
}
