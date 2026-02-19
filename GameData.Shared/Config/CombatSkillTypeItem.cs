using System;
using System.Collections.Generic;
using Config.Common;

namespace Config;

[Serializable]
public class CombatSkillTypeItem : ConfigItem<CombatSkillTypeItem, sbyte>
{
	public readonly sbyte TemplateId;

	public readonly string Name;

	public readonly string BackgroundTexture;

	public readonly string LoadingTexture;

	public readonly string Icon;

	public readonly string DisplayIcon;

	public readonly string DisplayIconBig;

	public readonly string TipsIcon;

	public readonly string Desc;

	public readonly short LegendaryBookWeaponSlot;

	public readonly List<short> LegendaryBookSkillSlots;

	public readonly List<short> LegendaryBookWeaponSlotItemSubTypes;

	public readonly List<short> LegendaryBookAddPropertyYin;

	public readonly List<short> LegendaryBookAddPropertyYang;

	public readonly List<sbyte> LegendaryBookEffectSlotYin;

	public readonly List<sbyte> LegendaryBookEffectSlotYang;

	public readonly short LegendaryBookFeature;

	public readonly short LegendaryBookTaiwuFeature;

	public readonly short LegendaryBookConsumedFeature;

	public readonly byte AvailableOnLoading;

	public CombatSkillTypeItem(sbyte templateId, int name, string backgroundTexture, string loadingTexture, string icon, string displayIcon, string displayIconBig, string tipsIcon, int desc, short legendaryBookWeaponSlot, List<short> legendaryBookSkillSlots, List<short> legendaryBookWeaponSlotItemSubTypes, List<short> legendaryBookAddPropertyYin, List<short> legendaryBookAddPropertyYang, List<sbyte> legendaryBookEffectSlotYin, List<sbyte> legendaryBookEffectSlotYang, short legendaryBookFeature, short legendaryBookTaiwuFeature, short legendaryBookConsumedFeature, byte availableOnLoading)
	{
		TemplateId = templateId;
		Name = LocalStringManager.GetConfig("CombatSkillType_language", name);
		BackgroundTexture = backgroundTexture;
		LoadingTexture = loadingTexture;
		Icon = icon;
		DisplayIcon = displayIcon;
		DisplayIconBig = displayIconBig;
		TipsIcon = tipsIcon;
		Desc = LocalStringManager.GetConfig("CombatSkillType_language", desc);
		LegendaryBookWeaponSlot = legendaryBookWeaponSlot;
		LegendaryBookSkillSlots = legendaryBookSkillSlots;
		LegendaryBookWeaponSlotItemSubTypes = legendaryBookWeaponSlotItemSubTypes;
		LegendaryBookAddPropertyYin = legendaryBookAddPropertyYin;
		LegendaryBookAddPropertyYang = legendaryBookAddPropertyYang;
		LegendaryBookEffectSlotYin = legendaryBookEffectSlotYin;
		LegendaryBookEffectSlotYang = legendaryBookEffectSlotYang;
		LegendaryBookFeature = legendaryBookFeature;
		LegendaryBookTaiwuFeature = legendaryBookTaiwuFeature;
		LegendaryBookConsumedFeature = legendaryBookConsumedFeature;
		AvailableOnLoading = availableOnLoading;
	}

	public CombatSkillTypeItem()
	{
		TemplateId = 0;
		Name = null;
		BackgroundTexture = null;
		LoadingTexture = null;
		Icon = null;
		DisplayIcon = null;
		DisplayIconBig = null;
		TipsIcon = null;
		Desc = null;
		LegendaryBookWeaponSlot = 0;
		LegendaryBookSkillSlots = new List<short>();
		LegendaryBookWeaponSlotItemSubTypes = null;
		LegendaryBookAddPropertyYin = null;
		LegendaryBookAddPropertyYang = null;
		LegendaryBookEffectSlotYin = null;
		LegendaryBookEffectSlotYang = null;
		LegendaryBookFeature = 0;
		LegendaryBookTaiwuFeature = 0;
		LegendaryBookConsumedFeature = 0;
		AvailableOnLoading = 0;
	}

	public CombatSkillTypeItem(sbyte templateId, CombatSkillTypeItem other)
	{
		TemplateId = templateId;
		Name = other.Name;
		BackgroundTexture = other.BackgroundTexture;
		LoadingTexture = other.LoadingTexture;
		Icon = other.Icon;
		DisplayIcon = other.DisplayIcon;
		DisplayIconBig = other.DisplayIconBig;
		TipsIcon = other.TipsIcon;
		Desc = other.Desc;
		LegendaryBookWeaponSlot = other.LegendaryBookWeaponSlot;
		LegendaryBookSkillSlots = other.LegendaryBookSkillSlots;
		LegendaryBookWeaponSlotItemSubTypes = other.LegendaryBookWeaponSlotItemSubTypes;
		LegendaryBookAddPropertyYin = other.LegendaryBookAddPropertyYin;
		LegendaryBookAddPropertyYang = other.LegendaryBookAddPropertyYang;
		LegendaryBookEffectSlotYin = other.LegendaryBookEffectSlotYin;
		LegendaryBookEffectSlotYang = other.LegendaryBookEffectSlotYang;
		LegendaryBookFeature = other.LegendaryBookFeature;
		LegendaryBookTaiwuFeature = other.LegendaryBookTaiwuFeature;
		LegendaryBookConsumedFeature = other.LegendaryBookConsumedFeature;
		AvailableOnLoading = other.AvailableOnLoading;
	}

	public override int GetTemplateId()
	{
		return TemplateId;
	}

	public override CombatSkillTypeItem Duplicate(int templateId)
	{
		return new CombatSkillTypeItem((sbyte)templateId, this);
	}
}
