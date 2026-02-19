using System;
using Config.Common;
using GameData.Utilities;

namespace Config;

[Serializable]
public class ClothingItem : ConfigItem<ClothingItem, short>, IItemConfig
{
	public readonly short TemplateId;

	public readonly string Name;

	public readonly sbyte ItemType;

	public readonly short ItemSubType;

	public readonly sbyte Grade;

	public readonly short GroupId;

	public readonly string Icon;

	public readonly string Desc;

	public readonly bool Transferable;

	public readonly bool Stackable;

	public readonly bool Wagerable;

	public readonly bool Refinable;

	public readonly bool Poisonable;

	public readonly bool Repairable;

	public readonly bool Inheritable;

	public readonly bool Detachable;

	public readonly short MaxDurability;

	public readonly int BaseWeight;

	public readonly int BasePrice;

	public readonly int BaseValue;

	public readonly sbyte MerchantLevel;

	public readonly sbyte BaseHappinessChange;

	public readonly int BaseFavorabilityChange;

	public readonly sbyte GiftLevel;

	public readonly bool AllowRandomCreate;

	public readonly sbyte DropRate;

	public readonly bool IsSpecial;

	public readonly sbyte ResourceType;

	public readonly short PreservationDuration;

	public readonly short MakeItemSubType;

	public readonly sbyte EquipmentType;

	public readonly short EquipmentEffectId;

	public readonly short DisplayId;

	public readonly sbyte AgeGroup;

	public readonly bool KeepOnPassing;

	public readonly short WeaveNeedAttainment;

	public readonly sbyte WeaveType;

	public readonly string DlcName;

	public readonly string SmallVillageDesc;

	public readonly short EquipmentCombatPowerValueFactor;

	short IItemConfig.TemplateId => TemplateId;

	sbyte IItemConfig.ItemType => ItemType;

	short IItemConfig.ItemSubType => ItemSubType;

	string IItemConfig.Name => Name;

	sbyte IItemConfig.Grade => Grade;

	short IItemConfig.GroupId => GroupId;

	public ClothingItem(short templateId, int name, sbyte itemType, short itemSubType, sbyte grade, short groupId, string icon, int desc, bool transferable, bool stackable, bool wagerable, bool refinable, bool poisonable, bool repairable, bool inheritable, bool detachable, short maxDurability, int baseWeight, int basePrice, int baseValue, sbyte merchantLevel, sbyte baseHappinessChange, int baseFavorabilityChange, sbyte giftLevel, bool allowRandomCreate, sbyte dropRate, bool isSpecial, sbyte resourceType, short preservationDuration, short makeItemSubType, sbyte equipmentType, short equipmentEffectId, short displayId, sbyte ageGroup, bool keepOnPassing, short weaveNeedAttainment, sbyte weaveType, string dlcName, int smallVillageDesc, short equipmentCombatPowerValueFactor)
	{
		TemplateId = templateId;
		Name = LocalStringManager.GetConfig("Clothing_language", name);
		ItemType = itemType;
		ItemSubType = itemSubType;
		Grade = grade;
		GroupId = groupId;
		Icon = icon;
		Desc = LocalStringManager.GetConfig("Clothing_language", desc);
		Transferable = transferable;
		Stackable = stackable;
		Wagerable = wagerable;
		Refinable = refinable;
		Poisonable = poisonable;
		Repairable = repairable;
		Inheritable = inheritable;
		Detachable = detachable;
		MaxDurability = maxDurability;
		BaseWeight = baseWeight;
		BasePrice = basePrice;
		BaseValue = baseValue;
		MerchantLevel = merchantLevel;
		BaseHappinessChange = baseHappinessChange;
		BaseFavorabilityChange = baseFavorabilityChange;
		GiftLevel = giftLevel;
		AllowRandomCreate = allowRandomCreate;
		DropRate = dropRate;
		IsSpecial = isSpecial;
		ResourceType = resourceType;
		PreservationDuration = preservationDuration;
		MakeItemSubType = makeItemSubType;
		EquipmentType = equipmentType;
		EquipmentEffectId = equipmentEffectId;
		DisplayId = displayId;
		AgeGroup = ageGroup;
		KeepOnPassing = keepOnPassing;
		WeaveNeedAttainment = weaveNeedAttainment;
		WeaveType = weaveType;
		DlcName = dlcName;
		SmallVillageDesc = LocalStringManager.GetConfig("Clothing_language", smallVillageDesc);
		EquipmentCombatPowerValueFactor = equipmentCombatPowerValueFactor;
	}

	public ClothingItem()
	{
		TemplateId = 0;
		Name = null;
		ItemType = 3;
		ItemSubType = 303;
		Grade = 0;
		GroupId = 0;
		Icon = null;
		Desc = null;
		Transferable = true;
		Stackable = false;
		Wagerable = true;
		Refinable = false;
		Poisonable = false;
		Repairable = true;
		Inheritable = true;
		Detachable = true;
		MaxDurability = 0;
		BaseWeight = 0;
		BasePrice = 10;
		BaseValue = 15;
		MerchantLevel = 0;
		BaseHappinessChange = 0;
		BaseFavorabilityChange = 100;
		GiftLevel = 8;
		AllowRandomCreate = true;
		DropRate = 0;
		IsSpecial = false;
		ResourceType = 0;
		PreservationDuration = 12;
		MakeItemSubType = 0;
		EquipmentType = 2;
		EquipmentEffectId = 0;
		DisplayId = 0;
		AgeGroup = 2;
		KeepOnPassing = false;
		WeaveNeedAttainment = 0;
		WeaveType = 0;
		DlcName = null;
		SmallVillageDesc = null;
		EquipmentCombatPowerValueFactor = 0;
	}

	public ClothingItem(short templateId, ClothingItem other)
	{
		TemplateId = templateId;
		Name = other.Name;
		ItemType = other.ItemType;
		ItemSubType = other.ItemSubType;
		Grade = other.Grade;
		GroupId = other.GroupId;
		Icon = other.Icon;
		Desc = other.Desc;
		Transferable = other.Transferable;
		Stackable = other.Stackable;
		Wagerable = other.Wagerable;
		Refinable = other.Refinable;
		Poisonable = other.Poisonable;
		Repairable = other.Repairable;
		Inheritable = other.Inheritable;
		Detachable = other.Detachable;
		MaxDurability = other.MaxDurability;
		BaseWeight = other.BaseWeight;
		BasePrice = other.BasePrice;
		BaseValue = other.BaseValue;
		MerchantLevel = other.MerchantLevel;
		BaseHappinessChange = other.BaseHappinessChange;
		BaseFavorabilityChange = other.BaseFavorabilityChange;
		GiftLevel = other.GiftLevel;
		AllowRandomCreate = other.AllowRandomCreate;
		DropRate = other.DropRate;
		IsSpecial = other.IsSpecial;
		ResourceType = other.ResourceType;
		PreservationDuration = other.PreservationDuration;
		MakeItemSubType = other.MakeItemSubType;
		EquipmentType = other.EquipmentType;
		EquipmentEffectId = other.EquipmentEffectId;
		DisplayId = other.DisplayId;
		AgeGroup = other.AgeGroup;
		KeepOnPassing = other.KeepOnPassing;
		WeaveNeedAttainment = other.WeaveNeedAttainment;
		WeaveType = other.WeaveType;
		DlcName = other.DlcName;
		SmallVillageDesc = other.SmallVillageDesc;
		EquipmentCombatPowerValueFactor = other.EquipmentCombatPowerValueFactor;
	}

	public override int GetTemplateId()
	{
		return TemplateId;
	}

	public override ClothingItem Duplicate(int templateId)
	{
		return new ClothingItem((short)templateId, this);
	}
}
