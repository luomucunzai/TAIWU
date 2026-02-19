using GameData.Domains.Character;
using GameData.Domains.Character.Display;
using GameData.Serializer;
using GameData.Utilities;

namespace GameData.Domains.Organization.Display;

[SerializableGameData(NoCopyConstructors = true)]
public struct SettlementTreasuryDisplayData : ISerializableGameData
{
	[SerializableGameDataField(SubDataMaxCount = int.MaxValue)]
	public SettlementTreasury SettlementTreasury;

	[SerializableGameDataField]
	public int SupplyLevel;

	[SerializableGameDataField]
	public int DebtOrSupport;

	[SerializableGameDataField]
	public CharacterDisplayData[] GuardianCharacterDisplayDataLow;

	[SerializableGameDataField]
	public CharacterDisplayData[] GuardianCharacterDisplayDataMid;

	[SerializableGameDataField]
	public CharacterDisplayData[] GuardianCharacterDisplayDataHigh;

	[SerializableGameDataField]
	public int OrgTemplateId;

	[SerializableGameDataField]
	public sbyte SectStoryEnding;

	[SerializableGameDataField]
	public bool MartialArtTournamentResult;

	[SerializableGameDataField]
	public Inventory SupplyItems;

	[SerializableGameDataField]
	public byte AlertTime;

	[SerializableGameDataField]
	public byte InfluenceRefreshTime;

	[SerializableGameDataField]
	public sbyte ResourceStatus;

	[SerializableGameDataField]
	public SettlementNameRelatedData SettlementNameRelatedData;

	public bool IsSerializedSizeFixed()
	{
		return false;
	}

	public int GetSerializedSize()
	{
		int num = 21;
		num = ((SettlementTreasury == null) ? (num + 4) : (num + (4 + SettlementTreasury.GetSerializedSize())));
		if (GuardianCharacterDisplayDataLow != null)
		{
			num += 2;
			int num2 = GuardianCharacterDisplayDataLow.Length;
			for (int i = 0; i < num2; i++)
			{
				CharacterDisplayData characterDisplayData = GuardianCharacterDisplayDataLow[i];
				num = ((characterDisplayData == null) ? (num + 2) : (num + (2 + characterDisplayData.GetSerializedSize())));
			}
		}
		else
		{
			num += 2;
		}
		if (GuardianCharacterDisplayDataMid != null)
		{
			num += 2;
			int num3 = GuardianCharacterDisplayDataMid.Length;
			for (int j = 0; j < num3; j++)
			{
				CharacterDisplayData characterDisplayData2 = GuardianCharacterDisplayDataMid[j];
				num = ((characterDisplayData2 == null) ? (num + 2) : (num + (2 + characterDisplayData2.GetSerializedSize())));
			}
		}
		else
		{
			num += 2;
		}
		if (GuardianCharacterDisplayDataHigh != null)
		{
			num += 2;
			int num4 = GuardianCharacterDisplayDataHigh.Length;
			for (int k = 0; k < num4; k++)
			{
				CharacterDisplayData characterDisplayData3 = GuardianCharacterDisplayDataHigh[k];
				num = ((characterDisplayData3 == null) ? (num + 2) : (num + (2 + characterDisplayData3.GetSerializedSize())));
			}
		}
		else
		{
			num += 2;
		}
		num = ((SupplyItems == null) ? (num + 2) : (num + (2 + SupplyItems.GetSerializedSize())));
		if (num > 4)
		{
			return (num + 3) / 4 * 4;
		}
		return num;
	}

	public unsafe int Serialize(byte* pData)
	{
		byte* ptr = pData;
		if (SettlementTreasury != null)
		{
			byte* intPtr = ptr;
			ptr += 4;
			int num = SettlementTreasury.Serialize(ptr);
			ptr += num;
			Tester.Assert(num <= int.MaxValue);
			*(int*)intPtr = num;
		}
		else
		{
			*(int*)ptr = 0;
			ptr += 4;
		}
		*(int*)ptr = SupplyLevel;
		ptr += 4;
		*(int*)ptr = DebtOrSupport;
		ptr += 4;
		if (GuardianCharacterDisplayDataLow != null)
		{
			int num2 = GuardianCharacterDisplayDataLow.Length;
			Tester.Assert(num2 <= 65535);
			*(ushort*)ptr = (ushort)num2;
			ptr += 2;
			for (int i = 0; i < num2; i++)
			{
				CharacterDisplayData characterDisplayData = GuardianCharacterDisplayDataLow[i];
				if (characterDisplayData != null)
				{
					byte* intPtr2 = ptr;
					ptr += 2;
					int num3 = characterDisplayData.Serialize(ptr);
					ptr += num3;
					Tester.Assert(num3 <= 65535);
					*(ushort*)intPtr2 = (ushort)num3;
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
		if (GuardianCharacterDisplayDataMid != null)
		{
			int num4 = GuardianCharacterDisplayDataMid.Length;
			Tester.Assert(num4 <= 65535);
			*(ushort*)ptr = (ushort)num4;
			ptr += 2;
			for (int j = 0; j < num4; j++)
			{
				CharacterDisplayData characterDisplayData2 = GuardianCharacterDisplayDataMid[j];
				if (characterDisplayData2 != null)
				{
					byte* intPtr3 = ptr;
					ptr += 2;
					int num5 = characterDisplayData2.Serialize(ptr);
					ptr += num5;
					Tester.Assert(num5 <= 65535);
					*(ushort*)intPtr3 = (ushort)num5;
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
		if (GuardianCharacterDisplayDataHigh != null)
		{
			int num6 = GuardianCharacterDisplayDataHigh.Length;
			Tester.Assert(num6 <= 65535);
			*(ushort*)ptr = (ushort)num6;
			ptr += 2;
			for (int k = 0; k < num6; k++)
			{
				CharacterDisplayData characterDisplayData3 = GuardianCharacterDisplayDataHigh[k];
				if (characterDisplayData3 != null)
				{
					byte* intPtr4 = ptr;
					ptr += 2;
					int num7 = characterDisplayData3.Serialize(ptr);
					ptr += num7;
					Tester.Assert(num7 <= 65535);
					*(ushort*)intPtr4 = (ushort)num7;
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
		*(int*)ptr = OrgTemplateId;
		ptr += 4;
		*ptr = (byte)SectStoryEnding;
		ptr++;
		*ptr = (MartialArtTournamentResult ? ((byte)1) : ((byte)0));
		ptr++;
		if (SupplyItems != null)
		{
			byte* intPtr5 = ptr;
			ptr += 2;
			int num8 = SupplyItems.Serialize(ptr);
			ptr += num8;
			Tester.Assert(num8 <= 65535);
			*(ushort*)intPtr5 = (ushort)num8;
		}
		else
		{
			*(short*)ptr = 0;
			ptr += 2;
		}
		*ptr = AlertTime;
		ptr++;
		*ptr = InfluenceRefreshTime;
		ptr++;
		*ptr = (byte)ResourceStatus;
		ptr++;
		ptr += SettlementNameRelatedData.Serialize(ptr);
		int num9 = (int)(ptr - pData);
		if (num9 > 4)
		{
			return (num9 + 3) / 4 * 4;
		}
		return num9;
	}

	public unsafe int Deserialize(byte* pData)
	{
		byte* ptr = pData;
		int num = *(int*)ptr;
		ptr += 4;
		if (num > 0)
		{
			if (SettlementTreasury == null)
			{
				SettlementTreasury = new SettlementTreasury();
			}
			ptr += SettlementTreasury.Deserialize(ptr);
		}
		else
		{
			SettlementTreasury = null;
		}
		SupplyLevel = *(int*)ptr;
		ptr += 4;
		DebtOrSupport = *(int*)ptr;
		ptr += 4;
		ushort num2 = *(ushort*)ptr;
		ptr += 2;
		if (num2 > 0)
		{
			if (GuardianCharacterDisplayDataLow == null || GuardianCharacterDisplayDataLow.Length != num2)
			{
				GuardianCharacterDisplayDataLow = new CharacterDisplayData[num2];
			}
			for (int i = 0; i < num2; i++)
			{
				ushort num3 = *(ushort*)ptr;
				ptr += 2;
				if (num3 > 0)
				{
					CharacterDisplayData characterDisplayData = GuardianCharacterDisplayDataLow[i] ?? new CharacterDisplayData();
					ptr += characterDisplayData.Deserialize(ptr);
					GuardianCharacterDisplayDataLow[i] = characterDisplayData;
				}
				else
				{
					GuardianCharacterDisplayDataLow[i] = null;
				}
			}
		}
		else
		{
			GuardianCharacterDisplayDataLow = null;
		}
		ushort num4 = *(ushort*)ptr;
		ptr += 2;
		if (num4 > 0)
		{
			if (GuardianCharacterDisplayDataMid == null || GuardianCharacterDisplayDataMid.Length != num4)
			{
				GuardianCharacterDisplayDataMid = new CharacterDisplayData[num4];
			}
			for (int j = 0; j < num4; j++)
			{
				ushort num5 = *(ushort*)ptr;
				ptr += 2;
				if (num5 > 0)
				{
					CharacterDisplayData characterDisplayData2 = GuardianCharacterDisplayDataMid[j] ?? new CharacterDisplayData();
					ptr += characterDisplayData2.Deserialize(ptr);
					GuardianCharacterDisplayDataMid[j] = characterDisplayData2;
				}
				else
				{
					GuardianCharacterDisplayDataMid[j] = null;
				}
			}
		}
		else
		{
			GuardianCharacterDisplayDataMid = null;
		}
		ushort num6 = *(ushort*)ptr;
		ptr += 2;
		if (num6 > 0)
		{
			if (GuardianCharacterDisplayDataHigh == null || GuardianCharacterDisplayDataHigh.Length != num6)
			{
				GuardianCharacterDisplayDataHigh = new CharacterDisplayData[num6];
			}
			for (int k = 0; k < num6; k++)
			{
				ushort num7 = *(ushort*)ptr;
				ptr += 2;
				if (num7 > 0)
				{
					CharacterDisplayData characterDisplayData3 = GuardianCharacterDisplayDataHigh[k] ?? new CharacterDisplayData();
					ptr += characterDisplayData3.Deserialize(ptr);
					GuardianCharacterDisplayDataHigh[k] = characterDisplayData3;
				}
				else
				{
					GuardianCharacterDisplayDataHigh[k] = null;
				}
			}
		}
		else
		{
			GuardianCharacterDisplayDataHigh = null;
		}
		OrgTemplateId = *(int*)ptr;
		ptr += 4;
		SectStoryEnding = (sbyte)(*ptr);
		ptr++;
		MartialArtTournamentResult = *ptr != 0;
		ptr++;
		ushort num8 = *(ushort*)ptr;
		ptr += 2;
		if (num8 > 0)
		{
			if (SupplyItems == null)
			{
				SupplyItems = new Inventory();
			}
			ptr += SupplyItems.Deserialize(ptr);
		}
		else
		{
			SupplyItems = null;
		}
		AlertTime = *ptr;
		ptr++;
		InfluenceRefreshTime = *ptr;
		ptr++;
		ResourceStatus = (sbyte)(*ptr);
		ptr++;
		ptr += SettlementNameRelatedData.Deserialize(ptr);
		int num9 = (int)(ptr - pData);
		if (num9 > 4)
		{
			return (num9 + 3) / 4 * 4;
		}
		return num9;
	}
}
