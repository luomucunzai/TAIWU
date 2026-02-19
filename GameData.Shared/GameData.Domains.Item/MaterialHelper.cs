using System.Collections.Generic;

namespace GameData.Domains.Item;

public static class MaterialHelper
{
	public static class FieldIds
	{
		public const ushort Id = 0;

		public const ushort TemplateId = 1;

		public const ushort MaxDurability = 2;

		public const ushort CurrDurability = 3;

		public const ushort ModificationState = 4;

		public const ushort Name = 5;

		public const ushort ItemType = 6;

		public const ushort ItemSubType = 7;

		public const ushort Grade = 8;

		public const ushort Icon = 9;

		public const ushort Desc = 10;

		public const ushort Transferable = 11;

		public const ushort Stackable = 12;

		public const ushort Wagerable = 13;

		public const ushort Refinable = 14;

		public const ushort Poisonable = 15;

		public const ushort Repairable = 16;

		public const ushort BaseWeight = 17;

		public const ushort BaseValue = 18;

		public const ushort BasePrice = 19;

		public const ushort DropRate = 20;

		public const ushort ResourceType = 21;

		public const ushort PreservationDuration = 22;

		public const ushort RefiningEffect = 23;

		public const ushort ResourceAmount = 24;

		public const ushort RequiredLifeSkillType = 25;

		public const ushort RequiredAttainment = 26;

		public const ushort RequiredResourceAmount = 27;

		public const ushort CraftableItemTypes = 28;

		public const ushort InnatePoisons = 29;

		public const ushort BaseHappinessChange = 30;

		public const ushort GiftLevel = 31;

		public const ushort BaseFavorabilityChange = 32;

		public const ushort PenetrateResistOfInner = 33;

		public const ushort AvoidRateMind = 34;

		public const ushort PenetrateResistOfOuter = 35;

		public const ushort AvoidRateSpeed = 36;

		public const ushort RecoveryOfBreath = 37;

		public const ushort MoveSpeed = 38;

		public const ushort RecoveryOfFlaw = 39;

		public const ushort CastSpeed = 40;

		public const ushort RecoveryOfBlockedAcupoint = 41;

		public const ushort RecoveryOfStance = 42;

		public const ushort AvoidRateTechnique = 43;

		public const ushort GroupId = 44;

		public const ushort AttackSpeed = 45;

		public const ushort InnerRatio = 46;

		public const ushort RecoveryOfQiDisorder = 47;

		public const ushort ResistOfHotPoison = 48;

		public const ushort ResistOfGloomyPoison = 49;

		public const ushort AvoidRateStrength = 50;

		public const ushort ResistOfColdPoison = 51;

		public const ushort ResistOfRedPoison = 52;

		public const ushort ResistOfRottenPoison = 53;

		public const ushort ResistOfIllusoryPoison = 54;

		public const ushort ConsumedFeatureMedals = 55;

		public const ushort WeaponSwitchSpeed = 56;

		public const ushort PenetrateOfInner = 57;

		public const ushort PrimaryEffectValue = 58;

		public const ushort HitRateMind = 59;

		public const ushort Inheritable = 60;

		public const ushort MerchantLevel = 61;

		public const ushort AllowRandomCreate = 62;

		public const ushort IsSpecial = 63;

		public const ushort Property = 64;

		public const ushort BreakBonusEffect = 65;

		public const ushort DisassembleResultItemList = 66;

		public const ushort DisassembleResultCount = 67;

		public const ushort Duration = 68;

		public const ushort BaseMaxHealthDelta = 69;

		public const ushort PrimaryEffectType = 70;

		public const ushort PrimaryEffectThresholdValue = 71;

		public const ushort PrimaryInjuryRecoveryTimes = 72;

		public const ushort PrimaryRecoverAllInjuries = 73;

		public const ushort SecondaryEffectType = 74;

		public const ushort SecondaryEffectSubType = 75;

		public const ushort SecondaryEffectThresholdValue = 76;

		public const ushort SecondaryEffectValue = 77;

		public const ushort SecondaryInjuryRecoveryTimes = 78;

		public const ushort SecondaryRecoverAllInjuries = 79;

		public const ushort HitRateStrength = 80;

		public const ushort HitRateTechnique = 81;

		public const ushort HitRateSpeed = 82;

		public const ushort PenetrateOfOuter = 83;

		public const ushort PrimaryEffectSubType = 84;

		public const ushort FilterType = 85;

		public const ushort FilterHardness = 86;
	}

	public const ushort ArchiveFieldsCount = 5;

	public const ushort CacheFieldsCount = 0;

	public const ushort PureTemplateFieldsCount = 82;

	public const ushort WritableFieldsCount = 5;

	public const ushort ReadonlyFieldsCount = 82;

	public static readonly Dictionary<string, ushort> FieldName2FieldId = new Dictionary<string, ushort>
	{
		{ "Id", 0 },
		{ "TemplateId", 1 },
		{ "MaxDurability", 2 },
		{ "CurrDurability", 3 },
		{ "ModificationState", 4 },
		{ "Name", 5 },
		{ "ItemType", 6 },
		{ "ItemSubType", 7 },
		{ "Grade", 8 },
		{ "Icon", 9 },
		{ "Desc", 10 },
		{ "Transferable", 11 },
		{ "Stackable", 12 },
		{ "Wagerable", 13 },
		{ "Refinable", 14 },
		{ "Poisonable", 15 },
		{ "Repairable", 16 },
		{ "BaseWeight", 17 },
		{ "BaseValue", 18 },
		{ "BasePrice", 19 },
		{ "DropRate", 20 },
		{ "ResourceType", 21 },
		{ "PreservationDuration", 22 },
		{ "RefiningEffect", 23 },
		{ "ResourceAmount", 24 },
		{ "RequiredLifeSkillType", 25 },
		{ "RequiredAttainment", 26 },
		{ "RequiredResourceAmount", 27 },
		{ "CraftableItemTypes", 28 },
		{ "InnatePoisons", 29 },
		{ "BaseHappinessChange", 30 },
		{ "GiftLevel", 31 },
		{ "BaseFavorabilityChange", 32 },
		{ "PenetrateResistOfInner", 33 },
		{ "AvoidRateMind", 34 },
		{ "PenetrateResistOfOuter", 35 },
		{ "AvoidRateSpeed", 36 },
		{ "RecoveryOfBreath", 37 },
		{ "MoveSpeed", 38 },
		{ "RecoveryOfFlaw", 39 },
		{ "CastSpeed", 40 },
		{ "RecoveryOfBlockedAcupoint", 41 },
		{ "RecoveryOfStance", 42 },
		{ "AvoidRateTechnique", 43 },
		{ "GroupId", 44 },
		{ "AttackSpeed", 45 },
		{ "InnerRatio", 46 },
		{ "RecoveryOfQiDisorder", 47 },
		{ "ResistOfHotPoison", 48 },
		{ "ResistOfGloomyPoison", 49 },
		{ "AvoidRateStrength", 50 },
		{ "ResistOfColdPoison", 51 },
		{ "ResistOfRedPoison", 52 },
		{ "ResistOfRottenPoison", 53 },
		{ "ResistOfIllusoryPoison", 54 },
		{ "ConsumedFeatureMedals", 55 },
		{ "WeaponSwitchSpeed", 56 },
		{ "PenetrateOfInner", 57 },
		{ "PrimaryEffectValue", 58 },
		{ "HitRateMind", 59 },
		{ "Inheritable", 60 },
		{ "MerchantLevel", 61 },
		{ "AllowRandomCreate", 62 },
		{ "IsSpecial", 63 },
		{ "Property", 64 },
		{ "BreakBonusEffect", 65 },
		{ "DisassembleResultItemList", 66 },
		{ "DisassembleResultCount", 67 },
		{ "Duration", 68 },
		{ "BaseMaxHealthDelta", 69 },
		{ "PrimaryEffectType", 70 },
		{ "PrimaryEffectThresholdValue", 71 },
		{ "PrimaryInjuryRecoveryTimes", 72 },
		{ "PrimaryRecoverAllInjuries", 73 },
		{ "SecondaryEffectType", 74 },
		{ "SecondaryEffectSubType", 75 },
		{ "SecondaryEffectThresholdValue", 76 },
		{ "SecondaryEffectValue", 77 },
		{ "SecondaryInjuryRecoveryTimes", 78 },
		{ "SecondaryRecoverAllInjuries", 79 },
		{ "HitRateStrength", 80 },
		{ "HitRateTechnique", 81 },
		{ "HitRateSpeed", 82 },
		{ "PenetrateOfOuter", 83 },
		{ "PrimaryEffectSubType", 84 },
		{ "FilterType", 85 },
		{ "FilterHardness", 86 }
	};

	public static readonly string[] FieldId2FieldName = new string[87]
	{
		"Id", "TemplateId", "MaxDurability", "CurrDurability", "ModificationState", "Name", "ItemType", "ItemSubType", "Grade", "Icon",
		"Desc", "Transferable", "Stackable", "Wagerable", "Refinable", "Poisonable", "Repairable", "BaseWeight", "BaseValue", "BasePrice",
		"DropRate", "ResourceType", "PreservationDuration", "RefiningEffect", "ResourceAmount", "RequiredLifeSkillType", "RequiredAttainment", "RequiredResourceAmount", "CraftableItemTypes", "InnatePoisons",
		"BaseHappinessChange", "GiftLevel", "BaseFavorabilityChange", "PenetrateResistOfInner", "AvoidRateMind", "PenetrateResistOfOuter", "AvoidRateSpeed", "RecoveryOfBreath", "MoveSpeed", "RecoveryOfFlaw",
		"CastSpeed", "RecoveryOfBlockedAcupoint", "RecoveryOfStance", "AvoidRateTechnique", "GroupId", "AttackSpeed", "InnerRatio", "RecoveryOfQiDisorder", "ResistOfHotPoison", "ResistOfGloomyPoison",
		"AvoidRateStrength", "ResistOfColdPoison", "ResistOfRedPoison", "ResistOfRottenPoison", "ResistOfIllusoryPoison", "ConsumedFeatureMedals", "WeaponSwitchSpeed", "PenetrateOfInner", "PrimaryEffectValue", "HitRateMind",
		"Inheritable", "MerchantLevel", "AllowRandomCreate", "IsSpecial", "Property", "BreakBonusEffect", "DisassembleResultItemList", "DisassembleResultCount", "Duration", "BaseMaxHealthDelta",
		"PrimaryEffectType", "PrimaryEffectThresholdValue", "PrimaryInjuryRecoveryTimes", "PrimaryRecoverAllInjuries", "SecondaryEffectType", "SecondaryEffectSubType", "SecondaryEffectThresholdValue", "SecondaryEffectValue", "SecondaryInjuryRecoveryTimes", "SecondaryRecoverAllInjuries",
		"HitRateStrength", "HitRateTechnique", "HitRateSpeed", "PenetrateOfOuter", "PrimaryEffectSubType", "FilterType", "FilterHardness"
	};
}
