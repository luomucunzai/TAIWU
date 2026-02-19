using System;
using Config.Common;

namespace Config;

[Serializable]
public class InformationTypeItem : ConfigItem<InformationTypeItem, sbyte>
{
	public readonly sbyte TemplateId;

	public readonly string Name;

	public readonly string Desc;

	public readonly string DescGain;

	public readonly string DescEffect;

	public readonly string DescEffectWay;

	public InformationTypeItem(sbyte templateId, int name, int desc, int descGain, int descEffect, int descEffectWay)
	{
		TemplateId = templateId;
		Name = LocalStringManager.GetConfig("InformationType_language", name);
		Desc = LocalStringManager.GetConfig("InformationType_language", desc);
		DescGain = LocalStringManager.GetConfig("InformationType_language", descGain);
		DescEffect = LocalStringManager.GetConfig("InformationType_language", descEffect);
		DescEffectWay = LocalStringManager.GetConfig("InformationType_language", descEffectWay);
	}

	public InformationTypeItem()
	{
		TemplateId = 0;
		Name = null;
		Desc = null;
		DescGain = null;
		DescEffect = null;
		DescEffectWay = null;
	}

	public InformationTypeItem(sbyte templateId, InformationTypeItem other)
	{
		TemplateId = templateId;
		Name = other.Name;
		Desc = other.Desc;
		DescGain = other.DescGain;
		DescEffect = other.DescEffect;
		DescEffectWay = other.DescEffectWay;
	}

	public override int GetTemplateId()
	{
		return TemplateId;
	}

	public override InformationTypeItem Duplicate(int templateId)
	{
		return new InformationTypeItem((sbyte)templateId, this);
	}
}
