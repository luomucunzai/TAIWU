using System;
using System.Collections.Generic;
using Config.Common;
using Config.ConfigCells;
using GameData.Utilities;

namespace Config;

[Serializable]
public class TaskConditionItem : ConfigItem<TaskConditionItem, int>
{
	public readonly int TemplateId;

	public readonly ETaskConditionType Type;

	public readonly bool IsReverseCondition;

	public readonly int IntParam;

	public readonly string GlobalArgBoxKey;

	public readonly IntPair ValueRange;

	public readonly short Adventure;

	public readonly List<PresetItemWithCount> Items;

	public readonly short CharacterTemplateId;

	public readonly ETaskConditionCharacterType CharacterType;

	public readonly ETaskConditionAreaType AreaType;

	public readonly sbyte StateTemplateId;

	public readonly List<short> MapBlockList;

	public readonly short Building;

	public readonly int ProfessionSkill;

	public readonly List<int> OrTaskCondition;

	public readonly List<int> AndTaskCondition;

	public TaskConditionItem(int templateId, ETaskConditionType type, bool isReverseCondition, int intParam, string globalArgBoxKey, IntPair valueRange, short adventure, List<PresetItemWithCount> items, short characterTemplateId, ETaskConditionCharacterType characterType, ETaskConditionAreaType areaType, sbyte stateTemplateId, List<short> mapBlockList, short building, int professionSkill, List<int> orTaskCondition, List<int> andTaskCondition)
	{
		TemplateId = templateId;
		Type = type;
		IsReverseCondition = isReverseCondition;
		IntParam = intParam;
		GlobalArgBoxKey = globalArgBoxKey;
		ValueRange = valueRange;
		Adventure = adventure;
		Items = items;
		CharacterTemplateId = characterTemplateId;
		CharacterType = characterType;
		AreaType = areaType;
		StateTemplateId = stateTemplateId;
		MapBlockList = mapBlockList;
		Building = building;
		ProfessionSkill = professionSkill;
		OrTaskCondition = orTaskCondition;
		AndTaskCondition = andTaskCondition;
	}

	public TaskConditionItem()
	{
		TemplateId = 0;
		Type = ETaskConditionType.AdventureVisible;
		IsReverseCondition = false;
		IntParam = 0;
		GlobalArgBoxKey = null;
		ValueRange = new IntPair(0, 0);
		Adventure = 0;
		Items = new List<PresetItemWithCount>();
		CharacterTemplateId = 0;
		CharacterType = ETaskConditionCharacterType.Invalid;
		AreaType = ETaskConditionAreaType.Invalid;
		StateTemplateId = 0;
		MapBlockList = new List<short>();
		Building = 0;
		ProfessionSkill = 0;
		OrTaskCondition = new List<int>();
		AndTaskCondition = new List<int>();
	}

	public TaskConditionItem(int templateId, TaskConditionItem other)
	{
		TemplateId = templateId;
		Type = other.Type;
		IsReverseCondition = other.IsReverseCondition;
		IntParam = other.IntParam;
		GlobalArgBoxKey = other.GlobalArgBoxKey;
		ValueRange = other.ValueRange;
		Adventure = other.Adventure;
		Items = other.Items;
		CharacterTemplateId = other.CharacterTemplateId;
		CharacterType = other.CharacterType;
		AreaType = other.AreaType;
		StateTemplateId = other.StateTemplateId;
		MapBlockList = other.MapBlockList;
		Building = other.Building;
		ProfessionSkill = other.ProfessionSkill;
		OrTaskCondition = other.OrTaskCondition;
		AndTaskCondition = other.AndTaskCondition;
	}

	public override int GetTemplateId()
	{
		return TemplateId;
	}

	public override TaskConditionItem Duplicate(int templateId)
	{
		return new TaskConditionItem(templateId, this);
	}
}
