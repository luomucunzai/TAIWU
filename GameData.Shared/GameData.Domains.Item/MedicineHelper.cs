using System.Collections.Generic;

namespace GameData.Domains.Item;

public static class MedicineHelper
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

		public const ushort Duration = 23;

		public const ushort InjuryRecoveryTimes = 24;

		public const ushort HitRateStrength = 25;

		public const ushort HitRateTechnique = 26;

		public const ushort HitRateSpeed = 27;

		public const ushort HitRateMind = 28;

		public const ushort PenetrateOfOuter = 29;

		public const ushort PenetrateOfInner = 30;

		public const ushort AvoidRateStrength = 31;

		public const ushort AvoidRateTechnique = 32;

		public const ushort AvoidRateSpeed = 33;

		public const ushort AvoidRateMind = 34;

		public const ushort PenetrateResistOfOuter = 35;

		public const ushort PenetrateResistOfInner = 36;

		public const ushort RecoveryOfStance = 37;

		public const ushort RecoveryOfBreath = 38;

		public const ushort MoveSpeed = 39;

		public const ushort RecoveryOfFlaw = 40;

		public const ushort CastSpeed = 41;

		public const ushort RecoveryOfBlockedAcupoint = 42;

		public const ushort WeaponSwitchSpeed = 43;

		public const ushort AttackSpeed = 44;

		public const ushort InnerRatio = 45;

		public const ushort RecoveryOfQiDisorder = 46;

		public const ushort WugType = 47;

		public const ushort WugGrowthType = 48;

		public const ushort SpecialEffectClass = 49;

		public const ushort ConsumedFeatureMedals = 50;

		public const ushort MaxUseDistance = 51;

		public const ushort SpecialEffectDesc = 52;

		public const ushort BuffAndOtherMedicine = 53;

		public const ushort BaseHappinessChange = 54;

		public const ushort GiftLevel = 55;

		public const ushort SpecialEffectId = 56;

		public const ushort BaseFavorabilityChange = 57;

		public const ushort ResistOfRedPoison = 58;

		public const ushort ResistOfIllusoryPoison = 59;

		public const ushort ResistOfRottenPoison = 60;

		public const ushort ResistOfColdPoison = 61;

		public const ushort ResistOfGloomyPoison = 62;

		public const ushort CanUseMultiple = 63;

		public const ushort BreakBonusEffect = 64;

		public const ushort Inheritable = 65;

		public const ushort MerchantLevel = 66;

		public const ushort AllowRandomCreate = 67;

		public const ushort IsSpecial = 68;

		public const ushort ResistOfHotPoison = 69;

		public const ushort GroupId = 70;

		public const ushort DamageStepBonus = 71;

		public const ushort RequiredMainAttributeValue = 72;

		public const ushort RequiredMainAttributeType = 73;

		public const ushort SideEffectValue = 74;

		public const ushort EffectValue = 75;

		public const ushort EffectThresholdValue = 76;

		public const ushort EffectSubType = 77;

		public const ushort HasNormalEatingEffect = 78;

		public const ushort InstantAffect = 79;

		public const ushort CombatUseEffect = 80;

		public const ushort CombatPrepareUseEffect = 81;

		public const ushort EffectType = 82;
	}

	public const ushort ArchiveFieldsCount = 5;

	public const ushort CacheFieldsCount = 0;

	public const ushort PureTemplateFieldsCount = 78;

	public const ushort WritableFieldsCount = 5;

	public const ushort ReadonlyFieldsCount = 78;

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
		{ "Duration", 23 },
		{ "InjuryRecoveryTimes", 24 },
		{ "HitRateStrength", 25 },
		{ "HitRateTechnique", 26 },
		{ "HitRateSpeed", 27 },
		{ "HitRateMind", 28 },
		{ "PenetrateOfOuter", 29 },
		{ "PenetrateOfInner", 30 },
		{ "AvoidRateStrength", 31 },
		{ "AvoidRateTechnique", 32 },
		{ "AvoidRateSpeed", 33 },
		{ "AvoidRateMind", 34 },
		{ "PenetrateResistOfOuter", 35 },
		{ "PenetrateResistOfInner", 36 },
		{ "RecoveryOfStance", 37 },
		{ "RecoveryOfBreath", 38 },
		{ "MoveSpeed", 39 },
		{ "RecoveryOfFlaw", 40 },
		{ "CastSpeed", 41 },
		{ "RecoveryOfBlockedAcupoint", 42 },
		{ "WeaponSwitchSpeed", 43 },
		{ "AttackSpeed", 44 },
		{ "InnerRatio", 45 },
		{ "RecoveryOfQiDisorder", 46 },
		{ "WugType", 47 },
		{ "WugGrowthType", 48 },
		{ "SpecialEffectClass", 49 },
		{ "ConsumedFeatureMedals", 50 },
		{ "MaxUseDistance", 51 },
		{ "SpecialEffectDesc", 52 },
		{ "BuffAndOtherMedicine", 53 },
		{ "BaseHappinessChange", 54 },
		{ "GiftLevel", 55 },
		{ "SpecialEffectId", 56 },
		{ "BaseFavorabilityChange", 57 },
		{ "ResistOfRedPoison", 58 },
		{ "ResistOfIllusoryPoison", 59 },
		{ "ResistOfRottenPoison", 60 },
		{ "ResistOfColdPoison", 61 },
		{ "ResistOfGloomyPoison", 62 },
		{ "CanUseMultiple", 63 },
		{ "BreakBonusEffect", 64 },
		{ "Inheritable", 65 },
		{ "MerchantLevel", 66 },
		{ "AllowRandomCreate", 67 },
		{ "IsSpecial", 68 },
		{ "ResistOfHotPoison", 69 },
		{ "GroupId", 70 },
		{ "DamageStepBonus", 71 },
		{ "RequiredMainAttributeValue", 72 },
		{ "RequiredMainAttributeType", 73 },
		{ "SideEffectValue", 74 },
		{ "EffectValue", 75 },
		{ "EffectThresholdValue", 76 },
		{ "EffectSubType", 77 },
		{ "HasNormalEatingEffect", 78 },
		{ "InstantAffect", 79 },
		{ "CombatUseEffect", 80 },
		{ "CombatPrepareUseEffect", 81 },
		{ "EffectType", 82 }
	};

	public static readonly string[] FieldId2FieldName = new string[83]
	{
		"Id", "TemplateId", "MaxDurability", "CurrDurability", "ModificationState", "Name", "ItemType", "ItemSubType", "Grade", "Icon",
		"Desc", "Transferable", "Stackable", "Wagerable", "Refinable", "Poisonable", "Repairable", "BaseWeight", "BaseValue", "BasePrice",
		"DropRate", "ResourceType", "PreservationDuration", "Duration", "InjuryRecoveryTimes", "HitRateStrength", "HitRateTechnique", "HitRateSpeed", "HitRateMind", "PenetrateOfOuter",
		"PenetrateOfInner", "AvoidRateStrength", "AvoidRateTechnique", "AvoidRateSpeed", "AvoidRateMind", "PenetrateResistOfOuter", "PenetrateResistOfInner", "RecoveryOfStance", "RecoveryOfBreath", "MoveSpeed",
		"RecoveryOfFlaw", "CastSpeed", "RecoveryOfBlockedAcupoint", "WeaponSwitchSpeed", "AttackSpeed", "InnerRatio", "RecoveryOfQiDisorder", "WugType", "WugGrowthType", "SpecialEffectClass",
		"ConsumedFeatureMedals", "MaxUseDistance", "SpecialEffectDesc", "BuffAndOtherMedicine", "BaseHappinessChange", "GiftLevel", "SpecialEffectId", "BaseFavorabilityChange", "ResistOfRedPoison", "ResistOfIllusoryPoison",
		"ResistOfRottenPoison", "ResistOfColdPoison", "ResistOfGloomyPoison", "CanUseMultiple", "BreakBonusEffect", "Inheritable", "MerchantLevel", "AllowRandomCreate", "IsSpecial", "ResistOfHotPoison",
		"GroupId", "DamageStepBonus", "RequiredMainAttributeValue", "RequiredMainAttributeType", "SideEffectValue", "EffectValue", "EffectThresholdValue", "EffectSubType", "HasNormalEatingEffect", "InstantAffect",
		"CombatUseEffect", "CombatPrepareUseEffect", "EffectType"
	};
}
