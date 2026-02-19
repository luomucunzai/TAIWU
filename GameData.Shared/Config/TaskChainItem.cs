using System;
using System.Collections.Generic;
using Config.Common;

namespace Config;

[Serializable]
public class TaskChainItem : ConfigItem<TaskChainItem, int>
{
	public readonly int TemplateId;

	public readonly ETaskChainGroup Group;

	public readonly ETaskChainType Type;

	public readonly short MainStoryLineMin;

	public readonly short MainStoryLineMax;

	public readonly int NextTaskChain;

	public readonly List<int> TaskList;

	public readonly List<int> StartConditions;

	public readonly List<int> RemoveCondtions;

	public readonly string Name;

	public readonly sbyte Sect;

	public TaskChainItem(int templateId, ETaskChainGroup group, ETaskChainType type, short mainStoryLineMin, short mainStoryLineMax, int nextTaskChain, List<int> taskList, List<int> startConditions, List<int> removeCondtions, int name, sbyte sect)
	{
		TemplateId = templateId;
		Group = group;
		Type = type;
		MainStoryLineMin = mainStoryLineMin;
		MainStoryLineMax = mainStoryLineMax;
		NextTaskChain = nextTaskChain;
		TaskList = taskList;
		StartConditions = startConditions;
		RemoveCondtions = removeCondtions;
		Name = LocalStringManager.GetConfig("TaskChain_language", name);
		Sect = sect;
	}

	public TaskChainItem()
	{
		TemplateId = 0;
		Group = ETaskChainGroup.MainStory;
		Type = ETaskChainType.Line;
		MainStoryLineMin = 0;
		MainStoryLineMax = 99;
		NextTaskChain = 0;
		TaskList = new List<int>();
		StartConditions = new List<int>();
		RemoveCondtions = new List<int>();
		Name = null;
		Sect = 0;
	}

	public TaskChainItem(int templateId, TaskChainItem other)
	{
		TemplateId = templateId;
		Group = other.Group;
		Type = other.Type;
		MainStoryLineMin = other.MainStoryLineMin;
		MainStoryLineMax = other.MainStoryLineMax;
		NextTaskChain = other.NextTaskChain;
		TaskList = other.TaskList;
		StartConditions = other.StartConditions;
		RemoveCondtions = other.RemoveCondtions;
		Name = other.Name;
		Sect = other.Sect;
	}

	public override int GetTemplateId()
	{
		return TemplateId;
	}

	public override TaskChainItem Duplicate(int templateId)
	{
		return new TaskChainItem(templateId, this);
	}
}
