using System.Collections.Generic;
using GameData.Domains.Character.Display;
using GameData.Serializer;
using GameData.Utilities;

namespace GameData.Domains.Organization.Display;

[SerializableGameData(NoCopyConstructors = true)]
public class SettlementPrisonDisplayData : ISerializableGameData
{
	[SerializableGameDataField]
	public int OrgTemplateId;

	[SerializableGameDataField]
	public int DebtOrSupport;

	[SerializableGameDataField]
	public CharacterDisplayData[] GuardianCharacterDisplayDataLow;

	[SerializableGameDataField]
	public CharacterDisplayData[] GuardianCharacterDisplayDataMid;

	[SerializableGameDataField]
	public CharacterDisplayData[] GuardianCharacterDisplayDataHigh;

	[SerializableGameDataField]
	public Dictionary<int, CharacterDisplayDataForSettlementPrisoner> PrisonerCharacterDisplayDataDict;

	public bool IsSerializedSizeFixed()
	{
		return false;
	}

	public int GetSerializedSize()
	{
		int num = 8;
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
		num += DictionaryOfBasicTypeCustomTypePair.GetSerializedSize<int, CharacterDisplayDataForSettlementPrisoner>(PrisonerCharacterDisplayDataDict);
		if (num > 4)
		{
			return (num + 3) / 4 * 4;
		}
		return num;
	}

	public unsafe int Serialize(byte* pData)
	{
		byte* ptr = pData;
		*(int*)ptr = OrgTemplateId;
		ptr += 4;
		*(int*)ptr = DebtOrSupport;
		ptr += 4;
		if (GuardianCharacterDisplayDataLow != null)
		{
			int num = GuardianCharacterDisplayDataLow.Length;
			Tester.Assert(num <= 65535);
			*(ushort*)ptr = (ushort)num;
			ptr += 2;
			for (int i = 0; i < num; i++)
			{
				CharacterDisplayData characterDisplayData = GuardianCharacterDisplayDataLow[i];
				if (characterDisplayData != null)
				{
					byte* intPtr = ptr;
					ptr += 2;
					int num2 = characterDisplayData.Serialize(ptr);
					ptr += num2;
					Tester.Assert(num2 <= 65535);
					*(ushort*)intPtr = (ushort)num2;
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
			int num3 = GuardianCharacterDisplayDataMid.Length;
			Tester.Assert(num3 <= 65535);
			*(ushort*)ptr = (ushort)num3;
			ptr += 2;
			for (int j = 0; j < num3; j++)
			{
				CharacterDisplayData characterDisplayData2 = GuardianCharacterDisplayDataMid[j];
				if (characterDisplayData2 != null)
				{
					byte* intPtr2 = ptr;
					ptr += 2;
					int num4 = characterDisplayData2.Serialize(ptr);
					ptr += num4;
					Tester.Assert(num4 <= 65535);
					*(ushort*)intPtr2 = (ushort)num4;
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
			int num5 = GuardianCharacterDisplayDataHigh.Length;
			Tester.Assert(num5 <= 65535);
			*(ushort*)ptr = (ushort)num5;
			ptr += 2;
			for (int k = 0; k < num5; k++)
			{
				CharacterDisplayData characterDisplayData3 = GuardianCharacterDisplayDataHigh[k];
				if (characterDisplayData3 != null)
				{
					byte* intPtr3 = ptr;
					ptr += 2;
					int num6 = characterDisplayData3.Serialize(ptr);
					ptr += num6;
					Tester.Assert(num6 <= 65535);
					*(ushort*)intPtr3 = (ushort)num6;
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
		ptr += DictionaryOfBasicTypeCustomTypePair.Serialize<int, CharacterDisplayDataForSettlementPrisoner>(ptr, ref PrisonerCharacterDisplayDataDict);
		int num7 = (int)(ptr - pData);
		if (num7 > 4)
		{
			return (num7 + 3) / 4 * 4;
		}
		return num7;
	}

	public unsafe int Deserialize(byte* pData)
	{
		byte* ptr = pData;
		OrgTemplateId = *(int*)ptr;
		ptr += 4;
		DebtOrSupport = *(int*)ptr;
		ptr += 4;
		ushort num = *(ushort*)ptr;
		ptr += 2;
		if (num > 0)
		{
			if (GuardianCharacterDisplayDataLow == null || GuardianCharacterDisplayDataLow.Length != num)
			{
				GuardianCharacterDisplayDataLow = new CharacterDisplayData[num];
			}
			for (int i = 0; i < num; i++)
			{
				ushort num2 = *(ushort*)ptr;
				ptr += 2;
				if (num2 > 0)
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
		ushort num3 = *(ushort*)ptr;
		ptr += 2;
		if (num3 > 0)
		{
			if (GuardianCharacterDisplayDataMid == null || GuardianCharacterDisplayDataMid.Length != num3)
			{
				GuardianCharacterDisplayDataMid = new CharacterDisplayData[num3];
			}
			for (int j = 0; j < num3; j++)
			{
				ushort num4 = *(ushort*)ptr;
				ptr += 2;
				if (num4 > 0)
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
		ushort num5 = *(ushort*)ptr;
		ptr += 2;
		if (num5 > 0)
		{
			if (GuardianCharacterDisplayDataHigh == null || GuardianCharacterDisplayDataHigh.Length != num5)
			{
				GuardianCharacterDisplayDataHigh = new CharacterDisplayData[num5];
			}
			for (int k = 0; k < num5; k++)
			{
				ushort num6 = *(ushort*)ptr;
				ptr += 2;
				if (num6 > 0)
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
		ptr += DictionaryOfBasicTypeCustomTypePair.Deserialize<int, CharacterDisplayDataForSettlementPrisoner>(ptr, ref PrisonerCharacterDisplayDataDict);
		int num7 = (int)(ptr - pData);
		if (num7 > 4)
		{
			return (num7 + 3) / 4 * 4;
		}
		return num7;
	}
}
