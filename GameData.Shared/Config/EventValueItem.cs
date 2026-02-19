using System;
using Config.Common;

namespace Config;

[Serializable]
public class EventValueItem : ConfigItem<EventValueItem, int>
{
	public readonly int TemplateId;

	public readonly EEventValueType Type;

	public readonly string Name;

	public readonly string ArgBoxKey;

	public readonly int EventArgument;

	public readonly string Alias;

	public EventValueItem(int templateId, EEventValueType type, int name, string argBoxKey, int eventArgument, string alias)
	{
		TemplateId = templateId;
		Type = type;
		Name = LocalStringManager.GetConfig("EventValue_language", name);
		ArgBoxKey = argBoxKey;
		EventArgument = eventArgument;
		Alias = alias;
	}

	public EventValueItem()
	{
		TemplateId = 0;
		Type = EEventValueType.Invalid;
		Name = null;
		ArgBoxKey = null;
		EventArgument = 0;
		Alias = null;
	}

	public EventValueItem(int templateId, EventValueItem other)
	{
		TemplateId = templateId;
		Type = other.Type;
		Name = other.Name;
		ArgBoxKey = other.ArgBoxKey;
		EventArgument = other.EventArgument;
		Alias = other.Alias;
	}

	public override int GetTemplateId()
	{
		return TemplateId;
	}

	public override EventValueItem Duplicate(int templateId)
	{
		return new EventValueItem(templateId, this);
	}
}
