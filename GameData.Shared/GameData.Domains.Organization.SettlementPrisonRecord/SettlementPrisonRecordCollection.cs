using System.Collections.Generic;
using Config;
using GameData.Domains.LifeRecord.GeneralRecord;
using GameData.Utilities;

namespace GameData.Domains.Organization.SettlementPrisonRecord;

public class SettlementPrisonRecordCollection : WriteableRecordCollection
{
	public void GetRenderInfos(List<SettlementPrisonRecordRenderInfo> renderInfos, ArgumentCollection argumentCollection)
	{
		int index = -1;
		int offset = -1;
		while (Next(ref index, ref offset))
		{
			SettlementPrisonRecordRenderInfo renderInfo = GetRenderInfo(offset, argumentCollection);
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

	public new unsafe SettlementPrisonRecordRenderInfo GetRenderInfo(int offset, ArgumentCollection argumentCollection)
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
			SettlementPrisonRecordItem settlementPrisonRecordItem = Config.SettlementPrisonRecord.Instance[num];
			if (settlementPrisonRecordItem == null)
			{
				AdaptableLog.Warning($"Unable to render monthly notification with template id {num}");
				return null;
			}
			string[] parameters = settlementPrisonRecordItem.Parameters;
			SettlementPrisonRecordRenderInfo settlementPrisonRecordRenderInfo = new SettlementPrisonRecordRenderInfo(num, settlementPrisonRecordItem.Desc, date, settlementId);
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
				settlementPrisonRecordRenderInfo.Arguments.Add((b, item));
			}
			return settlementPrisonRecordRenderInfo;
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

	public int AddIntrudePrison(int date, short settlementId, int charId)
	{
		int num = BeginAddingRecord(date, settlementId, 0);
		AppendCharacter(charId);
		EndAddingRecord(num);
		return num;
	}

	public int AddIntrudePrisonAndSentToPrisonTaiwu(int date, short settlementId, int charId, int charId1)
	{
		int num = BeginAddingRecord(date, settlementId, 1);
		AppendCharacter(charId);
		AppendCharacter(charId1);
		EndAddingRecord(num);
		return num;
	}

	public int AddIntrudePrisonAndPrisonRobbery(int date, short settlementId, int charId, int charId1)
	{
		int num = BeginAddingRecord(date, settlementId, 2);
		AppendCharacter(charId);
		AppendCharacter(charId1);
		EndAddingRecord(num);
		return num;
	}

	public int AddSendingToPrisonTaiwu(int date, short settlementId, int charId, int charId1)
	{
		int num = BeginAddingRecord(date, settlementId, 3);
		AppendCharacter(charId);
		AppendCharacter(charId1);
		EndAddingRecord(num);
		return num;
	}

	public int AddPrisonRobbery(int date, short settlementId, int charId, int charId1)
	{
		int num = BeginAddingRecord(date, settlementId, 4);
		AppendCharacter(charId);
		AppendCharacter(charId1);
		EndAddingRecord(num);
		return num;
	}

	public int AddPrisonBail(int date, short settlementId, int charId, int charId1)
	{
		int num = BeginAddingRecord(date, settlementId, 5);
		AppendCharacter(charId);
		AppendCharacter(charId1);
		EndAddingRecord(num);
		return num;
	}

	public int AddImprisonedVoluntarily(int date, short settlementId, int charId, short punishmentType)
	{
		int num = BeginAddingRecord(date, settlementId, 6);
		AppendCharacter(charId);
		AppendPunishmentType(punishmentType);
		EndAddingRecord(num);
		return num;
	}

	public int AddImprisonedByArrested(int date, short settlementId, int charId, short punishmentType)
	{
		int num = BeginAddingRecord(date, settlementId, 7);
		AppendCharacter(charId);
		AppendPunishmentType(punishmentType);
		EndAddingRecord(num);
		return num;
	}

	public int AddBeReleasedUponCompletionOfASentence(int date, short settlementId, int charId)
	{
		int num = BeginAddingRecord(date, settlementId, 8);
		AppendCharacter(charId);
		EndAddingRecord(num);
		return num;
	}

	public int AddPrisonBreak(int date, short settlementId, int charId)
	{
		int num = BeginAddingRecord(date, settlementId, 9);
		AppendCharacter(charId);
		EndAddingRecord(num);
		return num;
	}

	public int AddSentToPrisonTaiwu(int date, short settlementId, int charId, int charId1)
	{
		int num = BeginAddingRecord(date, settlementId, 10);
		AppendCharacter(charId);
		AppendCharacter(charId1);
		EndAddingRecord(num);
		return num;
	}

	public int AddPrisonerBeReleaseByAristocrat(int date, short settlementId, int charId, int charId1)
	{
		int num = BeginAddingRecord(date, settlementId, 11);
		AppendCharacter(charId);
		AppendCharacter(charId1);
		EndAddingRecord(num);
		return num;
	}
}
