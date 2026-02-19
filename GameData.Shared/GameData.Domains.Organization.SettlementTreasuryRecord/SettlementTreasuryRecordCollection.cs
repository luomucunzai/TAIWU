using System.Collections.Generic;
using Config;
using GameData.Domains.LifeRecord.GeneralRecord;
using GameData.Utilities;

namespace GameData.Domains.Organization.SettlementTreasuryRecord;

public class SettlementTreasuryRecordCollection : WriteableRecordCollection
{
	public void GetRenderInfos(List<SettlementTreasuryRecordRenderInfo> renderInfos, ArgumentCollection argumentCollection)
	{
		int index = -1;
		int offset = -1;
		while (Next(ref index, ref offset))
		{
			SettlementTreasuryRecordRenderInfo renderInfo = GetRenderInfo(offset, argumentCollection);
			if (renderInfo != null)
			{
				renderInfos.Add(renderInfo);
			}
		}
	}

	public unsafe short GetRecordType(int offset)
	{
		fixed (byte* rawData = RawData)
		{
			return ((short*)(rawData + offset + 1))[2];
		}
	}

	private unsafe int GetDate(int offset)
	{
		fixed (byte* rawData = RawData)
		{
			return *(int*)(rawData + offset + 1);
		}
	}

	public new unsafe SettlementTreasuryRecordRenderInfo GetRenderInfo(int offset, ArgumentCollection argumentCollection)
	{
		fixed (byte* rawData = RawData)
		{
			byte* ptr = rawData + offset;
			ptr++;
			int date = *(int*)ptr;
			ptr += 4;
			short settlementId = *(short*)ptr;
			ptr += 2;
			short num = *(short*)ptr;
			ptr += 2;
			SettlementTreasuryRecordItem settlementTreasuryRecordItem = Config.SettlementTreasuryRecord.Instance[num];
			if (settlementTreasuryRecordItem == null)
			{
				AdaptableLog.Warning($"Unable to render monthly notification with template id {num}");
				return null;
			}
			string[] parameters = settlementTreasuryRecordItem.Parameters;
			SettlementTreasuryRecordRenderInfo settlementTreasuryRecordRenderInfo = new SettlementTreasuryRecordRenderInfo(num, settlementTreasuryRecordItem.Desc, date, settlementId);
			int i = 0;
			for (int num2 = parameters.Length; i < num2; i++)
			{
				string text = parameters[i];
				if (string.IsNullOrEmpty(text))
				{
					break;
				}
				sbyte b = ParameterType.Parse(text);
				int item = ReadonlyRecordCollection.ReadArgumentAndGetIndex(b, &ptr, argumentCollection);
				settlementTreasuryRecordRenderInfo.Arguments.Add((b, item));
			}
			return settlementTreasuryRecordRenderInfo;
		}
	}

	private unsafe int BeginAddingRecord(int date, short settlementId, short recordType)
	{
		int size = Size;
		int num = Size + 1 + 4 + 2 + 2;
		EnsureCapacity(num);
		Size = num;
		fixed (byte* rawData = RawData)
		{
			byte* num2 = rawData + size;
			*(int*)(num2 + 1) = date;
			((short*)(num2 + 1))[2] = settlementId;
			((short*)(num2 + 1 + 4))[1] = recordType;
		}
		return size;
	}

	public int AddSupplementResource(int date, short settlementId)
	{
		int num = BeginAddingRecord(date, settlementId, 0);
		EndAddingRecord(num);
		return num;
	}

	public int AddSupplementItem(int date, short settlementId)
	{
		int num = BeginAddingRecord(date, settlementId, 1);
		EndAddingRecord(num);
		return num;
	}

	public int AddStorageResource(int date, short settlementId, int charId, sbyte resourceType, int value, int value1)
	{
		int num = BeginAddingRecord(date, settlementId, 2);
		AppendCharacter(charId);
		AppendResource(resourceType);
		AppendInteger(value);
		AppendInteger(value1);
		EndAddingRecord(num);
		return num;
	}

	public int AddStorageItem(int date, short settlementId, int charId, sbyte itemType, short itemTemplateId, int value)
	{
		int num = BeginAddingRecord(date, settlementId, 3);
		AppendCharacter(charId);
		AppendItem(itemType, itemTemplateId);
		AppendInteger(value);
		EndAddingRecord(num);
		return num;
	}

	public int AddTakeOutResource(int date, short settlementId, int charId, sbyte resourceType, int value, int value1)
	{
		int num = BeginAddingRecord(date, settlementId, 4);
		AppendCharacter(charId);
		AppendResource(resourceType);
		AppendInteger(value);
		AppendInteger(value1);
		EndAddingRecord(num);
		return num;
	}

	public int AddTakeOutItem(int date, short settlementId, int charId, sbyte itemType, short itemTemplateId, int value)
	{
		int num = BeginAddingRecord(date, settlementId, 5);
		AppendCharacter(charId);
		AppendItem(itemType, itemTemplateId);
		AppendInteger(value);
		EndAddingRecord(num);
		return num;
	}

	public int AddTaiwuStorageResource(int date, short settlementId, int charId, sbyte resourceType, int value)
	{
		int num = BeginAddingRecord(date, settlementId, 6);
		AppendCharacter(charId);
		AppendResource(resourceType);
		AppendInteger(value);
		EndAddingRecord(num);
		return num;
	}

	public int AddTaiwuStorageItem(int date, short settlementId, int charId, sbyte itemType, short itemTemplateId)
	{
		int num = BeginAddingRecord(date, settlementId, 7);
		AppendCharacter(charId);
		AppendItem(itemType, itemTemplateId);
		EndAddingRecord(num);
		return num;
	}

	public int AddTaiwuTakeOutResource(int date, short settlementId, int charId, sbyte resourceType, int value)
	{
		int num = BeginAddingRecord(date, settlementId, 8);
		AppendCharacter(charId);
		AppendResource(resourceType);
		AppendInteger(value);
		EndAddingRecord(num);
		return num;
	}

	public int AddTaiwuTakeOutItem(int date, short settlementId, int charId, sbyte itemType, short itemTemplateId)
	{
		int num = BeginAddingRecord(date, settlementId, 9);
		AppendCharacter(charId);
		AppendItem(itemType, itemTemplateId);
		EndAddingRecord(num);
		return num;
	}

	public int AddDonateSectTreasury(int date, short settlementId, int charId)
	{
		int num = BeginAddingRecord(date, settlementId, 10);
		AppendCharacter(charId);
		EndAddingRecord(num);
		return num;
	}

	public int AddDonateTownTreasury(int date, short settlementId, int charId)
	{
		int num = BeginAddingRecord(date, settlementId, 11);
		AppendCharacter(charId);
		EndAddingRecord(num);
		return num;
	}

	public int AddIntrudeSectTreasury(int date, short settlementId, int charId)
	{
		int num = BeginAddingRecord(date, settlementId, 12);
		AppendCharacter(charId);
		EndAddingRecord(num);
		return num;
	}

	public int AddIntrudeTownTreasury(int date, short settlementId, int charId)
	{
		int num = BeginAddingRecord(date, settlementId, 13);
		AppendCharacter(charId);
		EndAddingRecord(num);
		return num;
	}

	public int AddPlunderSectTreasurySuccess(int date, short settlementId, int charId)
	{
		int num = BeginAddingRecord(date, settlementId, 14);
		AppendCharacter(charId);
		EndAddingRecord(num);
		return num;
	}

	public int AddPlunderTownTreasurySuccess(int date, short settlementId, int charId)
	{
		int num = BeginAddingRecord(date, settlementId, 15);
		AppendCharacter(charId);
		EndAddingRecord(num);
		return num;
	}

	public int AddPlunderSectTreasuryFail(int date, short settlementId, int charId)
	{
		int num = BeginAddingRecord(date, settlementId, 16);
		AppendCharacter(charId);
		EndAddingRecord(num);
		return num;
	}

	public int AddPlunderTownTreasuryFail(int date, short settlementId, int charId)
	{
		int num = BeginAddingRecord(date, settlementId, 17);
		AppendCharacter(charId);
		EndAddingRecord(num);
		return num;
	}

	public int AddConfiscateResource(int date, short settlementId, int charId, sbyte resourceType, int value)
	{
		int num = BeginAddingRecord(date, settlementId, 18);
		AppendCharacter(charId);
		AppendResource(resourceType);
		AppendInteger(value);
		EndAddingRecord(num);
		return num;
	}

	public int AddConfiscateItem(int date, short settlementId, int charId, sbyte itemType, short itemTemplateId)
	{
		int num = BeginAddingRecord(date, settlementId, 19);
		AppendCharacter(charId);
		AppendItem(itemType, itemTemplateId);
		EndAddingRecord(num);
		return num;
	}

	public int AddDistributeItem(int date, short settlementId, int charId, sbyte itemType, short itemTemplateId)
	{
		int num = BeginAddingRecord(date, settlementId, 20);
		AppendCharacter(charId);
		AppendItem(itemType, itemTemplateId);
		EndAddingRecord(num);
		return num;
	}

	public int AddClearRecord(int date, short settlementId)
	{
		int num = BeginAddingRecord(date, settlementId, 21);
		EndAddingRecord(num);
		return num;
	}

	public int AddDistributeResource(int date, short settlementId, int charId, sbyte resourceType, int value)
	{
		int num = BeginAddingRecord(date, settlementId, 22);
		AppendCharacter(charId);
		AppendResource(resourceType);
		AppendInteger(value);
		EndAddingRecord(num);
		return num;
	}

	public int AddSectStoryFulongLooting(int date, short settlementId, sbyte resourceType, int value, sbyte resourceType1, int value1)
	{
		int num = BeginAddingRecord(date, settlementId, 23);
		AppendResource(resourceType);
		AppendInteger(value);
		AppendResource(resourceType1);
		AppendInteger(value1);
		EndAddingRecord(num);
		return num;
	}
}
