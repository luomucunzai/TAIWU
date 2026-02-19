using System;
using Config.Common;

namespace Config;

[Serializable]
public class VillagerRoleArrangementItem : ConfigItem<VillagerRoleArrangementItem, short>
{
	public readonly short TemplateId;

	public readonly short VillagerRole;

	public readonly string ShortName;

	public readonly string Name;

	public readonly string DisplayIcon;

	public readonly string DisplayIcon2;

	public readonly string Desc;

	public readonly bool UnlockByChicken;

	public readonly bool InvisibleInGui;

	public VillagerRoleArrangementItem(short templateId, short villagerRole, int shortName, int name, string displayIcon, string displayIcon2, int desc, bool unlockByChicken, bool invisibleInGui)
	{
		TemplateId = templateId;
		VillagerRole = villagerRole;
		ShortName = LocalStringManager.GetConfig("VillagerRoleArrangement_language", shortName);
		Name = LocalStringManager.GetConfig("VillagerRoleArrangement_language", name);
		DisplayIcon = displayIcon;
		DisplayIcon2 = displayIcon2;
		Desc = LocalStringManager.GetConfig("VillagerRoleArrangement_language", desc);
		UnlockByChicken = unlockByChicken;
		InvisibleInGui = invisibleInGui;
	}

	public VillagerRoleArrangementItem()
	{
		TemplateId = 0;
		VillagerRole = 0;
		ShortName = null;
		Name = null;
		DisplayIcon = null;
		DisplayIcon2 = null;
		Desc = null;
		UnlockByChicken = false;
		InvisibleInGui = false;
	}

	public VillagerRoleArrangementItem(short templateId, VillagerRoleArrangementItem other)
	{
		TemplateId = templateId;
		VillagerRole = other.VillagerRole;
		ShortName = other.ShortName;
		Name = other.Name;
		DisplayIcon = other.DisplayIcon;
		DisplayIcon2 = other.DisplayIcon2;
		Desc = other.Desc;
		UnlockByChicken = other.UnlockByChicken;
		InvisibleInGui = other.InvisibleInGui;
	}

	public override int GetTemplateId()
	{
		return TemplateId;
	}

	public override VillagerRoleArrangementItem Duplicate(int templateId)
	{
		return new VillagerRoleArrangementItem((short)templateId, this);
	}
}
