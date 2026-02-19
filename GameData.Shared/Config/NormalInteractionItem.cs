using System;
using Config.Common;

namespace Config;

[Serializable]
public class NormalInteractionItem : ConfigItem<NormalInteractionItem, short>
{
	public readonly short TemplateId;

	public readonly string Name;

	public readonly string[] HeadEvent;

	public readonly string[] AgreeAndSuccess;

	public readonly string[] AgreeAndFail;

	public readonly string[] Disagree;

	public NormalInteractionItem(short templateId, int name, int[] headEvent, int[] agreeAndSuccess, int[] agreeAndFail, int[] disagree)
	{
		TemplateId = templateId;
		Name = LocalStringManager.GetConfig("NormalInteraction_language", name);
		HeadEvent = LocalStringManager.ConvertConfigList("NormalInteraction_language", headEvent);
		AgreeAndSuccess = LocalStringManager.ConvertConfigList("NormalInteraction_language", agreeAndSuccess);
		AgreeAndFail = LocalStringManager.ConvertConfigList("NormalInteraction_language", agreeAndFail);
		Disagree = LocalStringManager.ConvertConfigList("NormalInteraction_language", disagree);
	}

	public NormalInteractionItem()
	{
		TemplateId = 0;
		Name = null;
		HeadEvent = null;
		AgreeAndSuccess = null;
		AgreeAndFail = null;
		Disagree = null;
	}

	public NormalInteractionItem(short templateId, NormalInteractionItem other)
	{
		TemplateId = templateId;
		Name = other.Name;
		HeadEvent = other.HeadEvent;
		AgreeAndSuccess = other.AgreeAndSuccess;
		AgreeAndFail = other.AgreeAndFail;
		Disagree = other.Disagree;
	}

	public override int GetTemplateId()
	{
		return TemplateId;
	}

	public override NormalInteractionItem Duplicate(int templateId)
	{
		return new NormalInteractionItem((short)templateId, this);
	}
}
