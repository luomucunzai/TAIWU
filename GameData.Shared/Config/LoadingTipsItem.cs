using System;
using Config.Common;

namespace Config;

[Serializable]
public class LoadingTipsItem : ConfigItem<LoadingTipsItem, int>
{
	public readonly int TemplateId;

	public readonly string Content;

	public readonly byte AvailableOnLoading;

	public LoadingTipsItem(int templateId, int content, byte availableOnLoading)
	{
		TemplateId = templateId;
		Content = LocalStringManager.GetConfig("LoadingTips_language", content);
		AvailableOnLoading = availableOnLoading;
	}

	public LoadingTipsItem()
	{
		TemplateId = 0;
		Content = null;
		AvailableOnLoading = 0;
	}

	public LoadingTipsItem(int templateId, LoadingTipsItem other)
	{
		TemplateId = templateId;
		Content = other.Content;
		AvailableOnLoading = other.AvailableOnLoading;
	}

	public override int GetTemplateId()
	{
		return TemplateId;
	}

	public override LoadingTipsItem Duplicate(int templateId)
	{
		return new LoadingTipsItem(templateId, this);
	}
}
