using System.Collections.Generic;
using Config;
using GameData.Domains.LifeRecord.GeneralRecord;
using GameData.Utilities;

namespace GameData.Domains.Building.SamsaraPlatformRecord;

public class SamsaraPlatformRecordCollection : WriteableRecordCollection
{
	public void GetRenderInfos(List<SamsaraPlatformRecordRenderInfo> renderInfos, ArgumentCollection argumentCollection)
	{
		int index = -1;
		int offset = -1;
		while (Next(ref index, ref offset))
		{
			SamsaraPlatformRecordRenderInfo renderInfo = GetRenderInfo(offset, argumentCollection);
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

	public new unsafe SamsaraPlatformRecordRenderInfo GetRenderInfo(int offset, ArgumentCollection argumentCollection)
	{
		fixed (byte* rawData = RawData)
		{
			byte* ptr = rawData + offset;
			ptr++;
			int date = *(int*)ptr;
			ptr += 4;
			short num = *(short*)ptr;
			ptr += 2;
			SamsaraPlatformRecordItem samsaraPlatformRecordItem = Config.SamsaraPlatformRecord.Instance[num];
			if (samsaraPlatformRecordItem == null)
			{
				AdaptableLog.Warning($"Unable to render monthly notification with template id {num}");
				return null;
			}
			string[] parameters = samsaraPlatformRecordItem.Parameters;
			SamsaraPlatformRecordRenderInfo samsaraPlatformRecordRenderInfo = new SamsaraPlatformRecordRenderInfo(num, samsaraPlatformRecordItem.Desc, date);
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
				samsaraPlatformRecordRenderInfo.Arguments.Add((b, item));
			}
			return samsaraPlatformRecordRenderInfo;
		}
	}

	private unsafe int BeginAddingRecord(int date, short recordType)
	{
		int size = Size;
		int num = Size + 1 + 4 + 2;
		EnsureCapacity(num);
		Size = num;
		fixed (byte* rawData = RawData)
		{
			byte* num2 = rawData + size;
			*(int*)(num2 + 1) = date;
			((short*)(num2 + 1))[2] = recordType;
		}
		return size;
	}

	public int AddSamsaraSuccess(int date, int charId, sbyte destinyType, short settlementId, sbyte orgTemplateId, sbyte orgGrade, bool orgPrincipal, sbyte gender, int charId1)
	{
		int num = BeginAddingRecord(date, 0);
		AppendCharacter(charId);
		AppendDestinyType(destinyType);
		AppendSettlement(settlementId);
		AppendOrgGrade(orgTemplateId, orgGrade, orgPrincipal, gender);
		AppendCharacter(charId1);
		EndAddingRecord(num);
		return num;
	}

	public int AddSamsaraFailed(int date, int charId, sbyte destinyType)
	{
		int num = BeginAddingRecord(date, 1);
		AppendCharacter(charId);
		AppendDestinyType(destinyType);
		EndAddingRecord(num);
		return num;
	}
}
