using System;
using Config.Common;

namespace Config;

[Serializable]
public class WorldStateItem : ConfigItem<WorldStateItem, sbyte>
{
	public readonly sbyte TemplateId;

	public readonly string Name;

	public readonly string Desc;

	public readonly string Icon;

	public readonly string[] SectStoryCondition;

	public readonly sbyte Sect;

	public readonly short TriggerArea;

	public readonly short[] MonthlyEvents;

	public WorldStateItem(sbyte templateId, int name, int desc, string icon, int[] sectStoryCondition, sbyte sect, short triggerArea, short[] monthlyEvents)
	{
		TemplateId = templateId;
		Name = LocalStringManager.GetConfig("WorldState_language", name);
		Desc = LocalStringManager.GetConfig("WorldState_language", desc);
		Icon = icon;
		SectStoryCondition = LocalStringManager.ConvertConfigList("WorldState_language", sectStoryCondition);
		Sect = sect;
		TriggerArea = triggerArea;
		MonthlyEvents = monthlyEvents;
	}

	public WorldStateItem()
	{
		TemplateId = 0;
		Name = null;
		Desc = null;
		Icon = null;
		SectStoryCondition = LocalStringManager.ConvertConfigList("WorldState_language", null);
		Sect = 0;
		TriggerArea = 0;
		MonthlyEvents = null;
	}

	public WorldStateItem(sbyte templateId, WorldStateItem other)
	{
		TemplateId = templateId;
		Name = other.Name;
		Desc = other.Desc;
		Icon = other.Icon;
		SectStoryCondition = other.SectStoryCondition;
		Sect = other.Sect;
		TriggerArea = other.TriggerArea;
		MonthlyEvents = other.MonthlyEvents;
	}

	public override int GetTemplateId()
	{
		return TemplateId;
	}

	public override WorldStateItem Duplicate(int templateId)
	{
		return new WorldStateItem((sbyte)templateId, this);
	}
}
