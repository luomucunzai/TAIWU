using System;
using System.Collections.Generic;
using Config.Common;

namespace Config;

[Serializable]
public class InstantNotificationItem : ConfigItem<InstantNotificationItem, short>
{
	public readonly short TemplateId;

	public readonly EInstantNotificationType Type;

	public readonly string Name;

	public readonly sbyte Importance;

	public readonly string SimpleDesc;

	public readonly string Desc;

	public readonly string[] Parameters;

	public readonly List<sbyte> MergeableParameters;

	public InstantNotificationItem(short templateId, EInstantNotificationType type, int name, sbyte importance, int simpleDesc, int desc, string[] parameters, List<sbyte> mergeableParameters)
	{
		TemplateId = templateId;
		Type = type;
		Name = LocalStringManager.GetConfig("InstantNotification_language", name);
		Importance = importance;
		SimpleDesc = LocalStringManager.GetConfig("InstantNotification_language", simpleDesc);
		Desc = LocalStringManager.GetConfig("InstantNotification_language", desc);
		Parameters = parameters;
		MergeableParameters = mergeableParameters;
	}

	public InstantNotificationItem()
	{
		TemplateId = 0;
		Type = EInstantNotificationType.TaiwuVillage;
		Name = null;
		Importance = 0;
		SimpleDesc = null;
		Desc = null;
		Parameters = new string[4] { "", "", "", "" };
		MergeableParameters = null;
	}

	public InstantNotificationItem(short templateId, InstantNotificationItem other)
	{
		TemplateId = templateId;
		Type = other.Type;
		Name = other.Name;
		Importance = other.Importance;
		SimpleDesc = other.SimpleDesc;
		Desc = other.Desc;
		Parameters = other.Parameters;
		MergeableParameters = other.MergeableParameters;
	}

	public override int GetTemplateId()
	{
		return TemplateId;
	}

	public override InstantNotificationItem Duplicate(int templateId)
	{
		return new InstantNotificationItem((short)templateId, this);
	}
}
