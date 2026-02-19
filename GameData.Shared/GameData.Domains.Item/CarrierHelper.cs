using System.Collections.Generic;

namespace GameData.Domains.Item;

public static class CarrierHelper
{
	public static class FieldIds
	{
		public const ushort Id = 0;

		public const ushort TemplateId = 1;

		public const ushort MaxDurability = 2;

		public const ushort EquipmentEffectId = 3;

		public const ushort CurrDurability = 4;

		public const ushort ModificationState = 5;

		public const ushort EquippedCharId = 6;

		public const ushort MaterialResources = 7;

		public const ushort Name = 8;

		public const ushort ItemType = 9;

		public const ushort ItemSubType = 10;

		public const ushort Grade = 11;

		public const ushort Icon = 12;

		public const ushort Desc = 13;

		public const ushort Transferable = 14;

		public const ushort Stackable = 15;

		public const ushort Wagerable = 16;

		public const ushort Refinable = 17;

		public const ushort Poisonable = 18;

		public const ushort Repairable = 19;

		public const ushort BaseWeight = 20;

		public const ushort BaseValue = 21;

		public const ushort BasePrice = 22;

		public const ushort DropRate = 23;

		public const ushort ResourceType = 24;

		public const ushort PreservationDuration = 25;

		public const ushort EquipmentType = 26;

		public const ushort IsFlying = 27;

		public const ushort MakeItemSubType = 28;

		public const ushort BaseHappinessChange = 29;

		public const ushort BaseFavorabilityChange = 30;

		public const ushort GiftLevel = 31;

		public const ushort HateFoodType = 32;

		public const ushort TamePoint = 33;

		public const ushort CombatState = 34;

		public const ushort BaseCaptureRateBonus = 35;

		public const ushort BaseMaxKidnapSlotCountBonus = 36;

		public const ushort BaseMaxInventoryLoadBonus = 37;

		public const ushort BaseTravelTimeReduction = 38;

		public const ushort LoveFoodType = 39;

		public const ushort BaseDropRateBonus = 40;

		public const ushort TamePersonalities = 41;

		public const ushort Detachable = 42;

		public const ushort CharacterIdInCombat = 43;

		public const ushort MerchantLevel = 44;

		public const ushort AllowRandomCreate = 45;

		public const ushort TravelSkeleton = 46;

		public const ushort StandDisplay = 47;

		public const ushort GroupId = 48;

		public const ushort InnatePoisons = 49;

		public const ushort IsSpecial = 50;

		public const ushort Inheritable = 51;

		public const ushort EquipmentCombatPowerValueFactor = 52;

		public const ushort BaseExploreBonusRate = 53;
	}

	public const ushort ArchiveFieldsCount = 8;

	public const ushort CacheFieldsCount = 0;

	public const ushort PureTemplateFieldsCount = 46;

	public const ushort WritableFieldsCount = 8;

	public const ushort ReadonlyFieldsCount = 46;

	public static readonly Dictionary<string, ushort> FieldName2FieldId = new Dictionary<string, ushort>
	{
		{ "Id", 0 },
		{ "TemplateId", 1 },
		{ "MaxDurability", 2 },
		{ "EquipmentEffectId", 3 },
		{ "CurrDurability", 4 },
		{ "ModificationState", 5 },
		{ "EquippedCharId", 6 },
		{ "MaterialResources", 7 },
		{ "Name", 8 },
		{ "ItemType", 9 },
		{ "ItemSubType", 10 },
		{ "Grade", 11 },
		{ "Icon", 12 },
		{ "Desc", 13 },
		{ "Transferable", 14 },
		{ "Stackable", 15 },
		{ "Wagerable", 16 },
		{ "Refinable", 17 },
		{ "Poisonable", 18 },
		{ "Repairable", 19 },
		{ "BaseWeight", 20 },
		{ "BaseValue", 21 },
		{ "BasePrice", 22 },
		{ "DropRate", 23 },
		{ "ResourceType", 24 },
		{ "PreservationDuration", 25 },
		{ "EquipmentType", 26 },
		{ "IsFlying", 27 },
		{ "MakeItemSubType", 28 },
		{ "BaseHappinessChange", 29 },
		{ "BaseFavorabilityChange", 30 },
		{ "GiftLevel", 31 },
		{ "HateFoodType", 32 },
		{ "TamePoint", 33 },
		{ "CombatState", 34 },
		{ "BaseCaptureRateBonus", 35 },
		{ "BaseMaxKidnapSlotCountBonus", 36 },
		{ "BaseMaxInventoryLoadBonus", 37 },
		{ "BaseTravelTimeReduction", 38 },
		{ "LoveFoodType", 39 },
		{ "BaseDropRateBonus", 40 },
		{ "TamePersonalities", 41 },
		{ "Detachable", 42 },
		{ "CharacterIdInCombat", 43 },
		{ "MerchantLevel", 44 },
		{ "AllowRandomCreate", 45 },
		{ "TravelSkeleton", 46 },
		{ "StandDisplay", 47 },
		{ "GroupId", 48 },
		{ "InnatePoisons", 49 },
		{ "IsSpecial", 50 },
		{ "Inheritable", 51 },
		{ "EquipmentCombatPowerValueFactor", 52 },
		{ "BaseExploreBonusRate", 53 }
	};

	public static readonly string[] FieldId2FieldName = new string[54]
	{
		"Id", "TemplateId", "MaxDurability", "EquipmentEffectId", "CurrDurability", "ModificationState", "EquippedCharId", "MaterialResources", "Name", "ItemType",
		"ItemSubType", "Grade", "Icon", "Desc", "Transferable", "Stackable", "Wagerable", "Refinable", "Poisonable", "Repairable",
		"BaseWeight", "BaseValue", "BasePrice", "DropRate", "ResourceType", "PreservationDuration", "EquipmentType", "IsFlying", "MakeItemSubType", "BaseHappinessChange",
		"BaseFavorabilityChange", "GiftLevel", "HateFoodType", "TamePoint", "CombatState", "BaseCaptureRateBonus", "BaseMaxKidnapSlotCountBonus", "BaseMaxInventoryLoadBonus", "BaseTravelTimeReduction", "LoveFoodType",
		"BaseDropRateBonus", "TamePersonalities", "Detachable", "CharacterIdInCombat", "MerchantLevel", "AllowRandomCreate", "TravelSkeleton", "StandDisplay", "GroupId", "InnatePoisons",
		"IsSpecial", "Inheritable", "EquipmentCombatPowerValueFactor", "BaseExploreBonusRate"
	};
}
