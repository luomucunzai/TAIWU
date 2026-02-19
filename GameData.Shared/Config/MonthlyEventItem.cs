using System;
using System.Collections.Generic;
using Config.Common;

namespace Config;

[Serializable]
public class MonthlyEventItem : ConfigItem<MonthlyEventItem, short>
{
	public readonly short TemplateId;

	public readonly string Name;

	public readonly EMonthlyEventType Type;

	public readonly string Event;

	public readonly string Icon;

	public readonly string Desc;

	public readonly string[] Parameters;

	public readonly List<sbyte> MergeableParameters;

	public readonly int Score;

	public readonly bool Node;

	public MonthlyEventItem(short templateId, int name, EMonthlyEventType type, string stringEvent, string icon, int desc, string[] parameters, List<sbyte> mergeableParameters, int score, bool node)
	{
		TemplateId = templateId;
		Name = LocalStringManager.GetConfig("MonthlyEvent_language", name);
		Type = type;
		Event = stringEvent;
		Icon = icon;
		Desc = LocalStringManager.GetConfig("MonthlyEvent_language", desc);
		Parameters = parameters;
		MergeableParameters = mergeableParameters;
		Score = score;
		Node = node;
	}

	public MonthlyEventItem()
	{
		TemplateId = 0;
		Name = null;
		Type = EMonthlyEventType.Invalid;
		Event = null;
		Icon = null;
		Desc = null;
		Parameters = new string[7] { "", "", "", "", "", "", "" };
		MergeableParameters = null;
		Score = 0;
		Node = false;
	}

	public MonthlyEventItem(short templateId, MonthlyEventItem other)
	{
		TemplateId = templateId;
		Name = other.Name;
		Type = other.Type;
		Event = other.Event;
		Icon = other.Icon;
		Desc = other.Desc;
		Parameters = other.Parameters;
		MergeableParameters = other.MergeableParameters;
		Score = other.Score;
		Node = other.Node;
	}

	public override int GetTemplateId()
	{
		return TemplateId;
	}

	public override MonthlyEventItem Duplicate(int templateId)
	{
		return new MonthlyEventItem((short)templateId, this);
	}
}
