using System;

namespace Config;

[Serializable]
public class SettlementTreasuryLimitItem
{
	public readonly sbyte TemplateId;

	public readonly string Name;

	public SettlementTreasuryLimitItem(sbyte templateId, int name)
	{
		TemplateId = templateId;
		Name = LocalStringManager.GetConfig("SettlementTreasuryLimit_language", name);
	}

	public SettlementTreasuryLimitItem()
	{
		TemplateId = 0;
		Name = null;
	}
}
