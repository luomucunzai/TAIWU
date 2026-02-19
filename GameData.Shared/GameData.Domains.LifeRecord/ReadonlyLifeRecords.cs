using System;
using System.Collections.Generic;
using Config;
using GameData.Domains.Item;
using GameData.Domains.LifeRecord.GeneralRecord;
using GameData.Utilities;

namespace GameData.Domains.LifeRecord;

public class ReadonlyLifeRecords : ReadonlyRecordCollection
{
	public unsafe void GetPartialLifeRecords(int startDate, int monthCount, ref ReadonlyLifeRecords readonlyLifeRecords)
	{
		if (readonlyLifeRecords == null)
		{
			readonlyLifeRecords = new ReadonlyLifeRecords();
		}
		int index = -1;
		int offset = -1;
		int num = startDate + monthCount;
		int num2 = -1;
		int num3 = -1;
		int num4 = -1;
		int num5 = -1;
		while (Next(ref index, ref offset))
		{
			int date = GetDate(offset);
			if (num2 < 0 && date >= startDate)
			{
				num2 = offset;
				num4 = index;
			}
			if (date > num)
			{
				num3 = offset;
				num5 = index;
				break;
			}
		}
		int num6 = num3 - num2;
		readonlyLifeRecords.EnsureCapacity(num6);
		fixed (byte* rawData = RawData)
		{
			fixed (byte* rawData2 = readonlyLifeRecords.RawData)
			{
				Buffer.MemoryCopy(rawData + num2, rawData2, num6, num6);
			}
		}
		readonlyLifeRecords.Count = num5 - num4;
		readonlyLifeRecords.Size = num6;
	}

	public void GetRenderInfos(List<LifeRecordRenderInfo> renderInfos, ArgumentCollection argumentCollection)
	{
		int index = -1;
		int offset = -1;
		while (Next(ref index, ref offset))
		{
			LifeRecordRenderInfo renderInfo = GetRenderInfo(offset, argumentCollection);
			if (renderInfo != null)
			{
				renderInfos.Add(renderInfo);
			}
		}
	}

	public (int, int[]) GetRenderInfosOfDates(List<LifeRecordRenderInfo> renderInfos, ArgumentCollection argumentCollection, int startDate, int monthCount)
	{
		int index = -1;
		int offset = -1;
		int num = 0;
		int num2 = 0;
		int num3 = int.MaxValue;
		int num4 = startDate + monthCount - 1;
		int[] array = new int[monthCount];
		for (int i = 0; i < monthCount; i++)
		{
			array[i] = 50;
		}
		int num5 = 0;
		int num6 = 0;
		int num7 = int.MaxValue;
		int num8 = startDate;
		while (Next(ref index, ref offset))
		{
			int date = GetDate(offset);
			if (date < startDate)
			{
				continue;
			}
			if (date > num4)
			{
				break;
			}
			LifeRecordRenderInfo renderInfo = GetRenderInfo(offset, argumentCollection);
			if (renderInfo == null)
			{
				continue;
			}
			renderInfos.Add(renderInfo);
			if (num8 != date)
			{
				if (num7 == int.MaxValue)
				{
					num7 = ((num5 > 0) ? (num6 / num5) : 50);
				}
				array[num8 - startDate] = num7;
				num6 = 0;
				num5 = 0;
				num7 = int.MaxValue;
				num8 = date;
			}
			LifeRecordItem lifeRecordItem = Config.LifeRecord.Instance[renderInfo.RecordType];
			switch (lifeRecordItem.ScoreType)
			{
			case ELifeRecordScoreType.Normal:
				renderInfo.Score = lifeRecordItem.Score;
				break;
			case ELifeRecordScoreType.Absolute:
				renderInfo.Score = lifeRecordItem.Score;
				num3 = ((num3 > lifeRecordItem.Score) ? lifeRecordItem.Score : num3);
				num7 = ((num7 > lifeRecordItem.Score) ? lifeRecordItem.Score : num7);
				break;
			case ELifeRecordScoreType.Calculated:
				renderInfo.Score = 50;
				break;
			}
			if (renderInfo.Score != 50)
			{
				num2 += renderInfo.Score;
				num++;
				num6 += renderInfo.Score;
				num5++;
			}
		}
		if (num6 > 0)
		{
			if (num7 == int.MaxValue)
			{
				num7 = ((num5 > 0) ? (num6 / num5) : 50);
			}
			array[num8 - startDate] = num7;
		}
		if (num3 == int.MaxValue)
		{
			num3 = ((num > 0) ? (num2 / num) : 50);
		}
		return (num3, array);
	}

	private int GetCalculatedLifeRecordScore(LifeRecordRenderInfo renderInfo, ArgumentCollection argumentCollection)
	{
		switch (renderInfo.RecordType)
		{
		case 16:
		case 17:
			var (itemType2, templateId2) = argumentCollection.Items[renderInfo.Arguments[1].index];
			return 50 + (ItemTemplateHelper.GetGrade(itemType2, templateId2) + 1) * 3;
		case 18:
		{
			short index4 = argumentCollection.CombatSkills[renderInfo.Arguments[1].index];
			return 50 + (Config.CombatSkill.Instance[index4].Grade + 1) * 3;
		}
		case 19:
		{
			short index3 = argumentCollection.CombatSkills[renderInfo.Arguments[1].index];
			return 50 - (Config.CombatSkill.Instance[index3].Grade + 1) * 3;
		}
		case 20:
		{
			short index2 = argumentCollection.CombatSkills[renderInfo.Arguments[1].index];
			return 50 + (Config.CombatSkill.Instance[index2].Grade + 1) * 3;
		}
		case 21:
		{
			short index = argumentCollection.LifeSkills[renderInfo.Arguments[1].index];
			return 50 + (LifeSkill.Instance[index].Grade + 1) * 3;
		}
		case 81:
		case 83:
			var (itemType, templateId) = argumentCollection.Items[renderInfo.Arguments[1].index];
			return 50 + (ItemTemplateHelper.GetGrade(itemType, templateId) + 1) * 3;
		default:
			return 50;
		}
	}

	private unsafe int GetDate(int offset)
	{
		fixed (byte* rawData = RawData)
		{
			return *(int*)(rawData + offset + 1);
		}
	}

	public new unsafe LifeRecordRenderInfo GetRenderInfo(int offset, ArgumentCollection argumentCollection)
	{
		fixed (byte* rawData = RawData)
		{
			byte* ptr = rawData + offset;
			int date = *(int*)(ptr + 1);
			short num = ((short*)(ptr + 1))[2];
			ptr += 7;
			LifeRecordItem lifeRecordItem = Config.LifeRecord.Instance[num];
			if (lifeRecordItem == null)
			{
				AdaptableLog.Warning($"Unable to render monthly notification with template id {num}");
				return null;
			}
			string[] parameters = GetParameters(lifeRecordItem);
			LifeRecordRenderInfo lifeRecordRenderInfo = new LifeRecordRenderInfo(num, lifeRecordItem.Desc, date);
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
				lifeRecordRenderInfo.Arguments.Add((b, item));
			}
			return lifeRecordRenderInfo;
		}
	}

	public unsafe (int, short) GetDreamBackRelatedCharacterId(int offset)
	{
		fixed (byte* rawData = RawData)
		{
			byte* ptr = rawData + offset;
			short num = ((short*)(ptr + 1))[2];
			ptr += 7;
			LifeRecordItem lifeRecordItem = Config.LifeRecord.Instance[num];
			if (lifeRecordItem == null || lifeRecordItem.DreamBackEventPriority < 0)
			{
				return (-1, num);
			}
			Tester.Assert(ParameterType.Parse(GetParameters(lifeRecordItem)[0]) == 0);
			return (*(int*)ptr, num);
		}
	}

	private static string[] GetParameters(LifeRecordItem config)
	{
		if (config.IsSourceRecord)
		{
			return config.Parameters;
		}
		short index = config.RelatedIds[0];
		return Config.LifeRecord.Instance[index].Parameters;
	}
}
