using System;
using System.Collections.Generic;
using Config.Common;

namespace Config;

[Serializable]
public class LifeRecordItem : ConfigItem<LifeRecordItem, short>
{
	public readonly short TemplateId;

	public readonly string Name;

	public readonly string Desc;

	public readonly string[] Parameters;

	public readonly bool IsSourceRecord;

	public readonly List<short> RelatedIds;

	public readonly short RequiredFavorability;

	public readonly ELifeRecordScoreType ScoreType;

	public readonly short Score;

	public readonly sbyte DreamBackEventPriority;

	public readonly ELifeRecordDisplayType DisplayType;

	public LifeRecordItem(short templateId, int name, int desc, string[] parameters, bool isSourceRecord, List<short> relatedIds, short requiredFavorability, ELifeRecordScoreType scoreType, short score, sbyte dreamBackEventPriority, ELifeRecordDisplayType displayType)
	{
		TemplateId = templateId;
		Name = LocalStringManager.GetConfig("LifeRecord_language", name);
		Desc = LocalStringManager.GetConfig("LifeRecord_language", desc);
		Parameters = parameters;
		IsSourceRecord = isSourceRecord;
		RelatedIds = relatedIds;
		RequiredFavorability = requiredFavorability;
		ScoreType = scoreType;
		Score = score;
		DreamBackEventPriority = dreamBackEventPriority;
		DisplayType = displayType;
	}

	public LifeRecordItem()
	{
		TemplateId = 0;
		Name = null;
		Desc = null;
		Parameters = new string[6] { "", "", "", "", "", "" };
		IsSourceRecord = true;
		RelatedIds = new List<short>();
		RequiredFavorability = -30000;
		ScoreType = ELifeRecordScoreType.Invalid;
		Score = -1;
		DreamBackEventPriority = -1;
		DisplayType = ELifeRecordDisplayType.None;
	}

	public LifeRecordItem(short templateId, LifeRecordItem other)
	{
		TemplateId = templateId;
		Name = other.Name;
		Desc = other.Desc;
		Parameters = other.Parameters;
		IsSourceRecord = other.IsSourceRecord;
		RelatedIds = other.RelatedIds;
		RequiredFavorability = other.RequiredFavorability;
		ScoreType = other.ScoreType;
		Score = other.Score;
		DreamBackEventPriority = other.DreamBackEventPriority;
		DisplayType = other.DisplayType;
	}

	public override int GetTemplateId()
	{
		return TemplateId;
	}

	public override LifeRecordItem Duplicate(int templateId)
	{
		return new LifeRecordItem((short)templateId, this);
	}
}
