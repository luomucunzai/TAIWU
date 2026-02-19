using System;
using Config.Common;

namespace Config;

[Serializable]
public class LegacyPointItem : ConfigItem<LegacyPointItem, short>
{
	public readonly short TemplateId;

	public readonly string Name;

	public readonly sbyte Type;

	public readonly short BasePoint;

	public readonly short MaxPoint;

	public readonly byte[] BonusTypes;

	public readonly string ConditionDesc;

	public LegacyPointItem(short templateId, int name, sbyte type, short basePoint, short maxPoint, byte[] bonusTypes, int conditionDesc)
	{
		TemplateId = templateId;
		Name = LocalStringManager.GetConfig("LegacyPoint_language", name);
		Type = type;
		BasePoint = basePoint;
		MaxPoint = maxPoint;
		BonusTypes = bonusTypes;
		ConditionDesc = LocalStringManager.GetConfig("LegacyPoint_language", conditionDesc);
	}

	public LegacyPointItem()
	{
		TemplateId = 0;
		Name = null;
		Type = 0;
		BasePoint = -1;
		MaxPoint = -1;
		BonusTypes = new byte[0];
		ConditionDesc = null;
	}

	public LegacyPointItem(short templateId, LegacyPointItem other)
	{
		TemplateId = templateId;
		Name = other.Name;
		Type = other.Type;
		BasePoint = other.BasePoint;
		MaxPoint = other.MaxPoint;
		BonusTypes = other.BonusTypes;
		ConditionDesc = other.ConditionDesc;
	}

	public override int GetTemplateId()
	{
		return TemplateId;
	}

	public override LegacyPointItem Duplicate(int templateId)
	{
		return new LegacyPointItem((short)templateId, this);
	}
}
