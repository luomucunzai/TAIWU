using System;
using Config.Common;

namespace Config;

[Serializable]
public class ImplementedDlcItem : ConfigItem<ImplementedDlcItem, byte>
{
	public readonly byte TemplateId;

	public readonly uint AppId;

	public readonly string Name;

	public ImplementedDlcItem(byte templateId, uint appId, string name)
	{
		TemplateId = templateId;
		AppId = appId;
		Name = name;
	}

	public ImplementedDlcItem()
	{
		TemplateId = 0;
		AppId = 0u;
		Name = null;
	}

	public ImplementedDlcItem(byte templateId, ImplementedDlcItem other)
	{
		TemplateId = templateId;
		AppId = other.AppId;
		Name = other.Name;
	}

	public override int GetTemplateId()
	{
		return TemplateId;
	}

	public override ImplementedDlcItem Duplicate(int templateId)
	{
		return new ImplementedDlcItem((byte)templateId, this);
	}
}
