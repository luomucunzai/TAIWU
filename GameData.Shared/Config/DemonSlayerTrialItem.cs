using System;
using Config.Common;

namespace Config;

[Serializable]
public class DemonSlayerTrialItem : ConfigItem<DemonSlayerTrialItem, int>
{
	public readonly int TemplateId;

	public readonly string Desc;

	public readonly short CharacterId;

	public readonly short FirstTimeRewards;

	public DemonSlayerTrialItem(int templateId, int desc, short characterId, short firstTimeRewards)
	{
		TemplateId = templateId;
		Desc = LocalStringManager.GetConfig("DemonSlayerTrial_language", desc);
		CharacterId = characterId;
		FirstTimeRewards = firstTimeRewards;
	}

	public DemonSlayerTrialItem()
	{
		TemplateId = 0;
		Desc = null;
		CharacterId = 0;
		FirstTimeRewards = 0;
	}

	public DemonSlayerTrialItem(int templateId, DemonSlayerTrialItem other)
	{
		TemplateId = templateId;
		Desc = other.Desc;
		CharacterId = other.CharacterId;
		FirstTimeRewards = other.FirstTimeRewards;
	}

	public override int GetTemplateId()
	{
		return TemplateId;
	}

	public override DemonSlayerTrialItem Duplicate(int templateId)
	{
		return new DemonSlayerTrialItem(templateId, this);
	}
}
