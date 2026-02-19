using System;
using Config.Common;

namespace Config;

[Serializable]
public class SettlementPrisonRecordItem : ConfigItem<SettlementPrisonRecordItem, short>
{
	public readonly short TemplateId;

	public readonly string Name;

	public readonly string Desc;

	public readonly string[] Parameters;

	public SettlementPrisonRecordItem(short templateId, int name, int desc, string[] parameters)
	{
		TemplateId = templateId;
		Name = LocalStringManager.GetConfig("SettlementPrisonRecord_language", name);
		Desc = LocalStringManager.GetConfig("SettlementPrisonRecord_language", desc);
		Parameters = parameters;
	}

	public SettlementPrisonRecordItem()
	{
		TemplateId = 0;
		Name = null;
		Desc = null;
		Parameters = new string[6] { "", "", "", "", "", "" };
	}

	public SettlementPrisonRecordItem(short templateId, SettlementPrisonRecordItem other)
	{
		TemplateId = templateId;
		Name = other.Name;
		Desc = other.Desc;
		Parameters = other.Parameters;
	}

	public override int GetTemplateId()
	{
		return TemplateId;
	}

	public override SettlementPrisonRecordItem Duplicate(int templateId)
	{
		return new SettlementPrisonRecordItem((short)templateId, this);
	}
}
