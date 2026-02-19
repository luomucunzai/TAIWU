using System;
using System.Collections.Generic;
using Config.Common;

namespace Config;

[Serializable]
public class EventScriptType : ConfigData<EventScriptTypeItem, sbyte>
{
	public static class DefKey
	{
		public const sbyte GlobalScript = 0;

		public const sbyte EventEnterScript = 1;

		public const sbyte EventConditionList = 2;

		public const sbyte OptionScript = 3;

		public const sbyte OptionAvailableConditionList = 4;

		public const sbyte OptionVisibleConditionList = 5;

		public const sbyte AdventureRemakeTriggerCondition = 6;
	}

	public static class DefValue
	{
		public static EventScriptTypeItem GlobalScript => Instance[(sbyte)0];

		public static EventScriptTypeItem EventEnterScript => Instance[(sbyte)1];

		public static EventScriptTypeItem EventConditionList => Instance[(sbyte)2];

		public static EventScriptTypeItem OptionScript => Instance[(sbyte)3];

		public static EventScriptTypeItem OptionAvailableConditionList => Instance[(sbyte)4];

		public static EventScriptTypeItem OptionVisibleConditionList => Instance[(sbyte)5];

		public static EventScriptTypeItem AdventureRemakeTriggerCondition => Instance[(sbyte)6];
	}

	public static EventScriptType Instance = new EventScriptType();

	private readonly HashSet<string> RequiredFields = new HashSet<string> { "TemplateId", "Name", "IsConditionList" };

	internal override int ToInt(sbyte value)
	{
		return value;
	}

	internal override sbyte ToTemplateId(int value)
	{
		return (sbyte)value;
	}

	private void CreateItems0()
	{
		_dataArray.Add(new EventScriptTypeItem(0, 0, isConditionList: false));
		_dataArray.Add(new EventScriptTypeItem(1, 1, isConditionList: false));
		_dataArray.Add(new EventScriptTypeItem(2, 2, isConditionList: true));
		_dataArray.Add(new EventScriptTypeItem(3, 3, isConditionList: false));
		_dataArray.Add(new EventScriptTypeItem(4, 4, isConditionList: true));
		_dataArray.Add(new EventScriptTypeItem(5, 5, isConditionList: true));
		_dataArray.Add(new EventScriptTypeItem(6, 6, isConditionList: true));
	}

	public override void Init()
	{
		base.Init();
		_dataArray = new List<EventScriptTypeItem>(7);
		CreateItems0();
	}
}
