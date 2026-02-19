using System;
using Config.Common;

namespace Config;

[Serializable]
public class ProtagonistFeatureItem : ConfigItem<ProtagonistFeatureItem, short>
{
	public readonly short TemplateId;

	public readonly sbyte Type;

	public readonly sbyte Cost;

	public readonly sbyte PrerequisiteCost;

	public readonly string Name;

	public readonly string Desc;

	public ProtagonistFeatureItem(short templateId, sbyte type, sbyte cost, sbyte prerequisiteCost, int name, int desc)
	{
		TemplateId = templateId;
		Type = type;
		Cost = cost;
		PrerequisiteCost = prerequisiteCost;
		Name = LocalStringManager.GetConfig("ProtagonistFeature_language", name);
		Desc = LocalStringManager.GetConfig("ProtagonistFeature_language", desc);
	}

	public ProtagonistFeatureItem()
	{
		TemplateId = 0;
		Type = 0;
		Cost = 0;
		PrerequisiteCost = 0;
		Name = null;
		Desc = null;
	}

	public ProtagonistFeatureItem(short templateId, ProtagonistFeatureItem other)
	{
		TemplateId = templateId;
		Type = other.Type;
		Cost = other.Cost;
		PrerequisiteCost = other.PrerequisiteCost;
		Name = other.Name;
		Desc = other.Desc;
	}

	public override int GetTemplateId()
	{
		return TemplateId;
	}

	public override ProtagonistFeatureItem Duplicate(int templateId)
	{
		return new ProtagonistFeatureItem((short)templateId, this);
	}
}
