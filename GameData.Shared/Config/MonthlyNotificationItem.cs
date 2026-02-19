using System;
using System.Collections.Generic;
using Config.Common;

namespace Config;

[Serializable]
public class MonthlyNotificationItem : ConfigItem<MonthlyNotificationItem, short>
{
	public readonly short TemplateId;

	public readonly string Name;

	public readonly string Icon;

	public readonly string Desc;

	public readonly string[] Parameters;

	public readonly List<sbyte> MergeableParameters;

	public readonly EMonthlyNotificationSectionType SectionType;

	public readonly sbyte DisplaySize;

	public readonly short SortingGroup;

	public readonly List<sbyte> ValueCheckParameters;

	public readonly int MergeLimit;

	public readonly string MergeDesc;

	public readonly sbyte MergeType;

	public MonthlyNotificationItem(short templateId, int name, string icon, int desc, string[] parameters, List<sbyte> mergeableParameters, EMonthlyNotificationSectionType sectionType, sbyte displaySize, short sortingGroup, List<sbyte> valueCheckParameters, int mergeLimit, int mergeDesc, sbyte mergeType)
	{
		TemplateId = templateId;
		Name = LocalStringManager.GetConfig("MonthlyNotification_language", name);
		Icon = icon;
		Desc = LocalStringManager.GetConfig("MonthlyNotification_language", desc);
		Parameters = parameters;
		MergeableParameters = mergeableParameters;
		SectionType = sectionType;
		DisplaySize = displaySize;
		SortingGroup = sortingGroup;
		ValueCheckParameters = valueCheckParameters;
		MergeLimit = mergeLimit;
		MergeDesc = LocalStringManager.GetConfig("MonthlyNotification_language", mergeDesc);
		MergeType = mergeType;
	}

	public MonthlyNotificationItem()
	{
		TemplateId = 0;
		Name = null;
		Icon = null;
		Desc = null;
		Parameters = new string[6] { "", "", "", "", "", "" };
		MergeableParameters = null;
		SectionType = EMonthlyNotificationSectionType.None;
		DisplaySize = 0;
		SortingGroup = 0;
		ValueCheckParameters = null;
		MergeLimit = -1;
		MergeDesc = null;
		MergeType = 0;
	}

	public MonthlyNotificationItem(short templateId, MonthlyNotificationItem other)
	{
		TemplateId = templateId;
		Name = other.Name;
		Icon = other.Icon;
		Desc = other.Desc;
		Parameters = other.Parameters;
		MergeableParameters = other.MergeableParameters;
		SectionType = other.SectionType;
		DisplaySize = other.DisplaySize;
		SortingGroup = other.SortingGroup;
		ValueCheckParameters = other.ValueCheckParameters;
		MergeLimit = other.MergeLimit;
		MergeDesc = other.MergeDesc;
		MergeType = other.MergeType;
	}

	public override int GetTemplateId()
	{
		return TemplateId;
	}

	public override MonthlyNotificationItem Duplicate(int templateId)
	{
		return new MonthlyNotificationItem((short)templateId, this);
	}
}
