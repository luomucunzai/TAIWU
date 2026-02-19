using System;
using Config.Common;

namespace Config;

[Serializable]
public class EventScriptTypeItem : ConfigItem<EventScriptTypeItem, sbyte>
{
	public readonly sbyte TemplateId;

	public readonly string Name;

	public readonly bool IsConditionList;

	public EventScriptTypeItem(sbyte templateId, int name, bool isConditionList)
	{
		TemplateId = templateId;
		Name = LocalStringManager.GetConfig("EventScriptType_language", name);
		IsConditionList = isConditionList;
	}

	public EventScriptTypeItem()
	{
		TemplateId = 0;
		Name = null;
		IsConditionList = false;
	}

	public EventScriptTypeItem(sbyte templateId, EventScriptTypeItem other)
	{
		TemplateId = templateId;
		Name = other.Name;
		IsConditionList = other.IsConditionList;
	}

	public override int GetTemplateId()
	{
		return TemplateId;
	}

	public override EventScriptTypeItem Duplicate(int templateId)
	{
		return new EventScriptTypeItem((sbyte)templateId, this);
	}
}
