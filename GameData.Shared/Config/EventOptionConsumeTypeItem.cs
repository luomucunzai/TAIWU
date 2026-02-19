using System;
using Config.Common;

namespace Config;

[Serializable]
public class EventOptionConsumeTypeItem : ConfigItem<EventOptionConsumeTypeItem, sbyte>
{
	public readonly sbyte TemplateId;

	public EventOptionConsumeTypeItem(sbyte templateId)
	{
		TemplateId = templateId;
	}

	public EventOptionConsumeTypeItem()
	{
		TemplateId = 0;
	}

	public EventOptionConsumeTypeItem(sbyte templateId, EventOptionConsumeTypeItem other)
	{
		TemplateId = templateId;
	}

	public override int GetTemplateId()
	{
		return TemplateId;
	}

	public override EventOptionConsumeTypeItem Duplicate(int templateId)
	{
		return new EventOptionConsumeTypeItem((sbyte)templateId, this);
	}
}
