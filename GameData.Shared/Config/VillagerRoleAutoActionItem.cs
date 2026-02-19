using System;
using Config.Common;

namespace Config;

[Serializable]
public class VillagerRoleAutoActionItem : ConfigItem<VillagerRoleAutoActionItem, short>
{
	public readonly short TemplateId;

	public readonly short VillagerRole;

	public readonly string ShortName;

	public readonly string Name;

	public readonly string DisplayIcon;

	public readonly string DisplayIcon2;

	public readonly string Desc;

	public readonly bool UnlockByChicken;

	public VillagerRoleAutoActionItem(short templateId, short villagerRole, int shortName, int name, string displayIcon, string displayIcon2, int desc, bool unlockByChicken)
	{
		TemplateId = templateId;
		VillagerRole = villagerRole;
		ShortName = LocalStringManager.GetConfig("VillagerRoleAutoAction_language", shortName);
		Name = LocalStringManager.GetConfig("VillagerRoleAutoAction_language", name);
		DisplayIcon = displayIcon;
		DisplayIcon2 = displayIcon2;
		Desc = LocalStringManager.GetConfig("VillagerRoleAutoAction_language", desc);
		UnlockByChicken = unlockByChicken;
	}

	public VillagerRoleAutoActionItem()
	{
		TemplateId = 0;
		VillagerRole = 0;
		ShortName = null;
		Name = null;
		DisplayIcon = null;
		DisplayIcon2 = null;
		Desc = null;
		UnlockByChicken = false;
	}

	public VillagerRoleAutoActionItem(short templateId, VillagerRoleAutoActionItem other)
	{
		TemplateId = templateId;
		VillagerRole = other.VillagerRole;
		ShortName = other.ShortName;
		Name = other.Name;
		DisplayIcon = other.DisplayIcon;
		DisplayIcon2 = other.DisplayIcon2;
		Desc = other.Desc;
		UnlockByChicken = other.UnlockByChicken;
	}

	public override int GetTemplateId()
	{
		return TemplateId;
	}

	public override VillagerRoleAutoActionItem Duplicate(int templateId)
	{
		return new VillagerRoleAutoActionItem((short)templateId, this);
	}
}
