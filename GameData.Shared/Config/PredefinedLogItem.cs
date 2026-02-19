using System;
using Config.Common;

namespace Config;

[Serializable]
public class PredefinedLogItem : ConfigItem<PredefinedLogItem, short>
{
	public readonly short TemplateId;

	public readonly string Name;

	public readonly string Info;

	public PredefinedLogItem(short templateId, int name, int info)
	{
		TemplateId = templateId;
		Name = LocalStringManager.GetConfig("PredefinedLog_language", name);
		Info = LocalStringManager.GetConfig("PredefinedLog_language", info);
	}

	public PredefinedLogItem()
	{
		TemplateId = 0;
		Name = null;
		Info = null;
	}

	public PredefinedLogItem(short templateId, PredefinedLogItem other)
	{
		TemplateId = templateId;
		Name = other.Name;
		Info = other.Info;
	}

	public override int GetTemplateId()
	{
		return TemplateId;
	}

	public override PredefinedLogItem Duplicate(int templateId)
	{
		return new PredefinedLogItem((short)templateId, this);
	}
}
