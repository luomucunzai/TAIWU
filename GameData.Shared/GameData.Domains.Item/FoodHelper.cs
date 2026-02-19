using System.Collections.Generic;

namespace GameData.Domains.Item;

public static class FoodHelper
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

		public const ushort ConsumedFeatureMedals = 24;

		public const ushort MainAttributesRegen = 25;

		public const ushort Strength = 26;

		public const ushort Dexterity = 27;

		public const ushort Concentration = 28;

		public const ushort Vitality = 29;

		public const ushort Energy = 30;

		public const ushort Intelligence = 31;

		public const ushort HitRateStrength = 32;

		public const ushort HitRateTechnique = 33;

		public const ushort HitRateSpeed = 34;

		public const ushort HitRateMind = 35;

		public const ushort PenetrateOfOuter = 36;

		public const ushort PenetrateOfInner = 37;

		public const ushort AvoidRateStrength = 38;

		public const ushort AvoidRateTechnique = 39;

		public const ushort AvoidRateSpeed = 40;

		public const ushort AvoidRateMind = 41;

		public const ushort PenetrateResistOfOuter = 42;

		public const ushort PenetrateResistOfInner = 43;

		public const ushort RecoveryOfStance = 44;

		public const ushort RecoveryOfBreath = 45;

		public const ushort MoveSpeed = 46;

		public const ushort RecoveryOfFlaw = 47;

		public const ushort CastSpeed = 48;

		public const ushort RecoveryOfBlockedAcupoint = 49;

		public const ushort WeaponSwitchSpeed = 50;

		public const ushort AttackSpeed = 51;

		public const ushort InnerRatio = 52;

		public const ushort RecoveryOfQiDisorder = 53;

		public const ushort ResistOfHotPoison = 54;

		public const ushort ResistOfGloomyPoison = 55;

		public const ushort ResistOfColdPoison = 56;

		public const ushort ResistOfRedPoison = 57;

		public const ushort ResistOfRottenPoison = 58;

		public const ushort ResistOfIllusoryPoison = 59;

		public const ushort BaseFavorabilityChange = 60;

		public const ushort BaseHappinessChange = 61;

		public const ushort GiftLevel = 62;

		public const ushort Inheritable = 63;

		public const ushort IsSpecial = 64;

		public const ushort MerchantLevel = 65;

		public const ushort AllowRandomCreate = 66;

		public const ushort BreakBonusEffect = 67;

		public const ushort GroupId = 68;

		public const ushort FoodType = 69;

		public const ushort BigIcon = 70;
	}

	public const ushort ArchiveFieldsCount = 5;

	public const ushort CacheFieldsCount = 0;

	public const ushort PureTemplateFieldsCount = 66;

	public const ushort WritableFieldsCount = 5;

	public const ushort ReadonlyFieldsCount = 66;

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
		{ "ConsumedFeatureMedals", 24 },
		{ "MainAttributesRegen", 25 },
		{ "Strength", 26 },
		{ "Dexterity", 27 },
		{ "Concentration", 28 },
		{ "Vitality", 29 },
		{ "Energy", 30 },
		{ "Intelligence", 31 },
		{ "HitRateStrength", 32 },
		{ "HitRateTechnique", 33 },
		{ "HitRateSpeed", 34 },
		{ "HitRateMind", 35 },
		{ "PenetrateOfOuter", 36 },
		{ "PenetrateOfInner", 37 },
		{ "AvoidRateStrength", 38 },
		{ "AvoidRateTechnique", 39 },
		{ "AvoidRateSpeed", 40 },
		{ "AvoidRateMind", 41 },
		{ "PenetrateResistOfOuter", 42 },
		{ "PenetrateResistOfInner", 43 },
		{ "RecoveryOfStance", 44 },
		{ "RecoveryOfBreath", 45 },
		{ "MoveSpeed", 46 },
		{ "RecoveryOfFlaw", 47 },
		{ "CastSpeed", 48 },
		{ "RecoveryOfBlockedAcupoint", 49 },
		{ "WeaponSwitchSpeed", 50 },
		{ "AttackSpeed", 51 },
		{ "InnerRatio", 52 },
		{ "RecoveryOfQiDisorder", 53 },
		{ "ResistOfHotPoison", 54 },
		{ "ResistOfGloomyPoison", 55 },
		{ "ResistOfColdPoison", 56 },
		{ "ResistOfRedPoison", 57 },
		{ "ResistOfRottenPoison", 58 },
		{ "ResistOfIllusoryPoison", 59 },
		{ "BaseFavorabilityChange", 60 },
		{ "BaseHappinessChange", 61 },
		{ "GiftLevel", 62 },
		{ "Inheritable", 63 },
		{ "IsSpecial", 64 },
		{ "MerchantLevel", 65 },
		{ "AllowRandomCreate", 66 },
		{ "BreakBonusEffect", 67 },
		{ "GroupId", 68 },
		{ "FoodType", 69 },
		{ "BigIcon", 70 }
	};

	public static readonly string[] FieldId2FieldName = new string[71]
	{
		"Id", "TemplateId", "MaxDurability", "CurrDurability", "ModificationState", "Name", "ItemType", "ItemSubType", "Grade", "Icon",
		"Desc", "Transferable", "Stackable", "Wagerable", "Refinable", "Poisonable", "Repairable", "BaseWeight", "BaseValue", "BasePrice",
		"DropRate", "ResourceType", "PreservationDuration", "Duration", "ConsumedFeatureMedals", "MainAttributesRegen", "Strength", "Dexterity", "Concentration", "Vitality",
		"Energy", "Intelligence", "HitRateStrength", "HitRateTechnique", "HitRateSpeed", "HitRateMind", "PenetrateOfOuter", "PenetrateOfInner", "AvoidRateStrength", "AvoidRateTechnique",
		"AvoidRateSpeed", "AvoidRateMind", "PenetrateResistOfOuter", "PenetrateResistOfInner", "RecoveryOfStance", "RecoveryOfBreath", "MoveSpeed", "RecoveryOfFlaw", "CastSpeed", "RecoveryOfBlockedAcupoint",
		"WeaponSwitchSpeed", "AttackSpeed", "InnerRatio", "RecoveryOfQiDisorder", "ResistOfHotPoison", "ResistOfGloomyPoison", "ResistOfColdPoison", "ResistOfRedPoison", "ResistOfRottenPoison", "ResistOfIllusoryPoison",
		"BaseFavorabilityChange", "BaseHappinessChange", "GiftLevel", "Inheritable", "IsSpecial", "MerchantLevel", "AllowRandomCreate", "BreakBonusEffect", "GroupId", "FoodType",
		"BigIcon"
	};
}
