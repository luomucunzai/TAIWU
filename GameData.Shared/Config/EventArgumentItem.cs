using System;
using Config.Common;
using GameData.Utilities;

namespace Config;

[Serializable]
public class EventArgumentItem : ConfigItem<EventArgumentItem, int>
{
	public readonly int TemplateId;

	public readonly EEventArgumentType Type;

	public readonly string Name;

	public readonly string Desc;

	public readonly string DefaultValue;

	public readonly bool IsExpression;

	public readonly bool AllowSwitchingExpression;

	public readonly string ConfigTable;

	public readonly string[] CustomEnumText;

	public readonly int[] CustomEnumValues;

	public readonly IntPair EnumRange;

	public EventArgumentItem(int templateId, EEventArgumentType type, int name, int desc, string defaultValue, bool isExpression, bool allowSwitchingExpression, string configTable, int[] customEnumText, int[] customEnumValues, IntPair enumRange)
	{
		TemplateId = templateId;
		Type = type;
		Name = LocalStringManager.GetConfig("EventArgument_language", name);
		Desc = LocalStringManager.GetConfig("EventArgument_language", desc);
		DefaultValue = defaultValue;
		IsExpression = isExpression;
		AllowSwitchingExpression = allowSwitchingExpression;
		ConfigTable = configTable;
		CustomEnumText = LocalStringManager.ConvertConfigList("EventArgument_language", customEnumText);
		CustomEnumValues = customEnumValues;
		EnumRange = enumRange;
	}

	public EventArgumentItem()
	{
		TemplateId = 0;
		Type = EEventArgumentType.Invalid;
		Name = null;
		Desc = null;
		DefaultValue = null;
		IsExpression = false;
		AllowSwitchingExpression = true;
		ConfigTable = null;
		CustomEnumText = LocalStringManager.ConvertConfigList("EventArgument_language", null);
		CustomEnumValues = new int[0];
		EnumRange = new IntPair(0, 0);
	}

	public EventArgumentItem(int templateId, EventArgumentItem other)
	{
		TemplateId = templateId;
		Type = other.Type;
		Name = other.Name;
		Desc = other.Desc;
		DefaultValue = other.DefaultValue;
		IsExpression = other.IsExpression;
		AllowSwitchingExpression = other.AllowSwitchingExpression;
		ConfigTable = other.ConfigTable;
		CustomEnumText = other.CustomEnumText;
		CustomEnumValues = other.CustomEnumValues;
		EnumRange = other.EnumRange;
	}

	public override int GetTemplateId()
	{
		return TemplateId;
	}

	public override EventArgumentItem Duplicate(int templateId)
	{
		return new EventArgumentItem(templateId, this);
	}
}
