using System;
using System.Collections.Generic;
using Config.Common;
using Config.ConfigCells;

namespace Config;

[Serializable]
public class TaskInfoItem : ConfigItem<TaskInfoItem, int>
{
	public readonly int TemplateId;

	public readonly string TaskTitle;

	public readonly string TaskOverview;

	public readonly string TaskDescription;

	public readonly string TaskBubblesContent;

	public readonly string TaskDescriptionMeet;

	public readonly int ShowTime;

	public readonly List<int> RunCondition;

	public readonly List<int> FinishCondition;

	public readonly List<int> BlockCondition;

	public readonly bool IsTriggeredTask;

	public readonly List<int> StartTaskChainsWhenFinish;

	public readonly short MainStoryLineMin;

	public readonly short MainStoryLineMax;

	public readonly short TaskOrder;

	public readonly List<short> CharacterTemplateId;

	public readonly string EventArgBoxKey;

	public readonly string[] CombatSkillIdsEventArgBoxKey;

	public readonly string[] SkillIdsEventArgBoxKey;

	public readonly string FrontEndKey;

	public readonly string[] StringArrayEventArgBoxKey;

	public readonly AutoTriggerMonthlyEvent[] MonthlyEvents;

	public TaskInfoItem(int templateId, int taskTitle, int taskOverview, int taskDescription, int taskBubblesContent, int taskDescriptionMeet, int showTime, List<int> runCondition, List<int> finishCondition, List<int> blockCondition, bool isTriggeredTask, List<int> startTaskChainsWhenFinish, short mainStoryLineMin, short mainStoryLineMax, short taskOrder, List<short> characterTemplateId, string eventArgBoxKey, string[] combatSkillIdsEventArgBoxKey, string[] skillIdsEventArgBoxKey, string frontEndKey, string[] stringArrayEventArgBoxKey, AutoTriggerMonthlyEvent[] monthlyEvents)
	{
		TemplateId = templateId;
		TaskTitle = LocalStringManager.GetConfig("TaskInfo_language", taskTitle);
		TaskOverview = LocalStringManager.GetConfig("TaskInfo_language", taskOverview);
		TaskDescription = LocalStringManager.GetConfig("TaskInfo_language", taskDescription);
		TaskBubblesContent = LocalStringManager.GetConfig("TaskInfo_language", taskBubblesContent);
		TaskDescriptionMeet = LocalStringManager.GetConfig("TaskInfo_language", taskDescriptionMeet);
		ShowTime = showTime;
		RunCondition = runCondition;
		FinishCondition = finishCondition;
		BlockCondition = blockCondition;
		IsTriggeredTask = isTriggeredTask;
		StartTaskChainsWhenFinish = startTaskChainsWhenFinish;
		MainStoryLineMin = mainStoryLineMin;
		MainStoryLineMax = mainStoryLineMax;
		TaskOrder = taskOrder;
		CharacterTemplateId = characterTemplateId;
		EventArgBoxKey = eventArgBoxKey;
		CombatSkillIdsEventArgBoxKey = combatSkillIdsEventArgBoxKey;
		SkillIdsEventArgBoxKey = skillIdsEventArgBoxKey;
		FrontEndKey = frontEndKey;
		StringArrayEventArgBoxKey = stringArrayEventArgBoxKey;
		MonthlyEvents = monthlyEvents;
	}

	public TaskInfoItem()
	{
		TemplateId = 0;
		TaskTitle = null;
		TaskOverview = null;
		TaskDescription = null;
		TaskBubblesContent = null;
		TaskDescriptionMeet = null;
		ShowTime = 120;
		RunCondition = new List<int>();
		FinishCondition = new List<int>();
		BlockCondition = new List<int>();
		IsTriggeredTask = false;
		StartTaskChainsWhenFinish = new List<int>();
		MainStoryLineMin = 0;
		MainStoryLineMax = 99;
		TaskOrder = 0;
		CharacterTemplateId = new List<short>();
		EventArgBoxKey = null;
		CombatSkillIdsEventArgBoxKey = null;
		SkillIdsEventArgBoxKey = null;
		FrontEndKey = null;
		StringArrayEventArgBoxKey = null;
		MonthlyEvents = null;
	}

	public TaskInfoItem(int templateId, TaskInfoItem other)
	{
		TemplateId = templateId;
		TaskTitle = other.TaskTitle;
		TaskOverview = other.TaskOverview;
		TaskDescription = other.TaskDescription;
		TaskBubblesContent = other.TaskBubblesContent;
		TaskDescriptionMeet = other.TaskDescriptionMeet;
		ShowTime = other.ShowTime;
		RunCondition = other.RunCondition;
		FinishCondition = other.FinishCondition;
		BlockCondition = other.BlockCondition;
		IsTriggeredTask = other.IsTriggeredTask;
		StartTaskChainsWhenFinish = other.StartTaskChainsWhenFinish;
		MainStoryLineMin = other.MainStoryLineMin;
		MainStoryLineMax = other.MainStoryLineMax;
		TaskOrder = other.TaskOrder;
		CharacterTemplateId = other.CharacterTemplateId;
		EventArgBoxKey = other.EventArgBoxKey;
		CombatSkillIdsEventArgBoxKey = other.CombatSkillIdsEventArgBoxKey;
		SkillIdsEventArgBoxKey = other.SkillIdsEventArgBoxKey;
		FrontEndKey = other.FrontEndKey;
		StringArrayEventArgBoxKey = other.StringArrayEventArgBoxKey;
		MonthlyEvents = other.MonthlyEvents;
	}

	public override int GetTemplateId()
	{
		return TemplateId;
	}

	public override TaskInfoItem Duplicate(int templateId)
	{
		return new TaskInfoItem(templateId, this);
	}
}
