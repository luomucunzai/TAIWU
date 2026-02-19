using System;
using Config.Common;

namespace Config;

[Serializable]
public class BodyPartItem : ConfigItem<BodyPartItem, sbyte>
{
	public readonly sbyte TemplateId;

	public readonly string Name;

	public readonly string AcupointDesc;

	public readonly int[] AcupointParam;

	public readonly int[] AcupointTime;

	public readonly string OuterInjuryIcon;

	public readonly string InnerInjuryIcon;

	public BodyPartItem(sbyte templateId, int name, int acupointDesc, int[] acupointParam, int[] acupointTime, string outerInjuryIcon, string innerInjuryIcon)
	{
		TemplateId = templateId;
		Name = LocalStringManager.GetConfig("BodyPart_language", name);
		AcupointDesc = LocalStringManager.GetConfig("BodyPart_language", acupointDesc);
		AcupointParam = acupointParam;
		AcupointTime = acupointTime;
		OuterInjuryIcon = outerInjuryIcon;
		InnerInjuryIcon = innerInjuryIcon;
	}

	public BodyPartItem()
	{
		TemplateId = 0;
		Name = null;
		AcupointDesc = null;
		AcupointParam = null;
		AcupointTime = new int[3] { 0, 50, 75 };
		OuterInjuryIcon = null;
		InnerInjuryIcon = null;
	}

	public BodyPartItem(sbyte templateId, BodyPartItem other)
	{
		TemplateId = templateId;
		Name = other.Name;
		AcupointDesc = other.AcupointDesc;
		AcupointParam = other.AcupointParam;
		AcupointTime = other.AcupointTime;
		OuterInjuryIcon = other.OuterInjuryIcon;
		InnerInjuryIcon = other.InnerInjuryIcon;
	}

	public override int GetTemplateId()
	{
		return TemplateId;
	}

	public override BodyPartItem Duplicate(int templateId)
	{
		return new BodyPartItem((sbyte)templateId, this);
	}
}
