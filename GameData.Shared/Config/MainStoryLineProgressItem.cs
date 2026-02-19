using System;
using Config.Common;

namespace Config;

[Serializable]
public class MainStoryLineProgressItem : ConfigItem<MainStoryLineProgressItem, short>
{
	public readonly short TemplateId;

	public readonly string MainStoryName;

	public readonly short MainStoryOrder;

	public MainStoryLineProgressItem(short templateId, int mainStoryName, short mainStoryOrder)
	{
		TemplateId = templateId;
		MainStoryName = LocalStringManager.GetConfig("MainStoryLineProgress_language", mainStoryName);
		MainStoryOrder = mainStoryOrder;
	}

	public MainStoryLineProgressItem()
	{
		TemplateId = 0;
		MainStoryName = null;
		MainStoryOrder = 0;
	}

	public MainStoryLineProgressItem(short templateId, MainStoryLineProgressItem other)
	{
		TemplateId = templateId;
		MainStoryName = other.MainStoryName;
		MainStoryOrder = other.MainStoryOrder;
	}

	public override int GetTemplateId()
	{
		return TemplateId;
	}

	public override MainStoryLineProgressItem Duplicate(int templateId)
	{
		return new MainStoryLineProgressItem((short)templateId, this);
	}
}
