using System;
using Config.Common;

namespace Config;

[Serializable]
public class LifeSkillTypeItem : ConfigItem<LifeSkillTypeItem, sbyte>
{
	public readonly sbyte TemplateId;

	public readonly string Name;

	public readonly string Desc;

	public readonly string Icon;

	public readonly string DisplayIcon;

	public readonly string DisplayIconBig;

	public readonly string BackgroundTexture;

	public readonly string LoadingTexture;

	public readonly string AttainmentEffectTexture;

	public readonly short InformationTemplateId;

	public readonly short[] SkillList;

	public readonly string MakeDesc;

	public readonly string DialogInBattle;

	public readonly byte AvailableOnLoading;

	public LifeSkillTypeItem(sbyte templateId, int name, int desc, string icon, string displayIcon, string displayIconBig, string backgroundTexture, string loadingTexture, string attainmentEffectTexture, short informationTemplateId, short[] skillList, int makeDesc, int dialogInBattle, byte availableOnLoading)
	{
		TemplateId = templateId;
		Name = LocalStringManager.GetConfig("LifeSkillType_language", name);
		Desc = LocalStringManager.GetConfig("LifeSkillType_language", desc);
		Icon = icon;
		DisplayIcon = displayIcon;
		DisplayIconBig = displayIconBig;
		BackgroundTexture = backgroundTexture;
		LoadingTexture = loadingTexture;
		AttainmentEffectTexture = attainmentEffectTexture;
		InformationTemplateId = informationTemplateId;
		SkillList = skillList;
		MakeDesc = LocalStringManager.GetConfig("LifeSkillType_language", makeDesc);
		DialogInBattle = LocalStringManager.GetConfig("LifeSkillType_language", dialogInBattle);
		AvailableOnLoading = availableOnLoading;
	}

	public LifeSkillTypeItem()
	{
		TemplateId = 0;
		Name = null;
		Desc = null;
		Icon = null;
		DisplayIcon = null;
		DisplayIconBig = null;
		BackgroundTexture = null;
		LoadingTexture = null;
		AttainmentEffectTexture = null;
		InformationTemplateId = 0;
		SkillList = null;
		MakeDesc = null;
		DialogInBattle = null;
		AvailableOnLoading = 0;
	}

	public LifeSkillTypeItem(sbyte templateId, LifeSkillTypeItem other)
	{
		TemplateId = templateId;
		Name = other.Name;
		Desc = other.Desc;
		Icon = other.Icon;
		DisplayIcon = other.DisplayIcon;
		DisplayIconBig = other.DisplayIconBig;
		BackgroundTexture = other.BackgroundTexture;
		LoadingTexture = other.LoadingTexture;
		AttainmentEffectTexture = other.AttainmentEffectTexture;
		InformationTemplateId = other.InformationTemplateId;
		SkillList = other.SkillList;
		MakeDesc = other.MakeDesc;
		DialogInBattle = other.DialogInBattle;
		AvailableOnLoading = other.AvailableOnLoading;
	}

	public override int GetTemplateId()
	{
		return TemplateId;
	}

	public override LifeSkillTypeItem Duplicate(int templateId)
	{
		return new LifeSkillTypeItem((sbyte)templateId, this);
	}
}
