using System.Collections.Generic;

namespace GameData.Domains.Item;

public static class TeaWineHelper
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

		public const ushort DirectChangeOfQiDisorder = 24;

		public const ushort ConsumedFeatureMedals = 25;

		public const ushort HitRateStrength = 26;

		public const ushort HitRateTechnique = 27;

		public const ushort HitRateSpeed = 28;

		public const ushort HitRateMind = 29;

		public const ushort PenetrateOfOuter = 30;

		public const ushort PenetrateOfInner = 31;

		public const ushort AvoidRateStrength = 32;

		public const ushort AvoidRateTechnique = 33;

		public const ushort AvoidRateSpeed = 34;

		public const ushort AvoidRateMind = 35;

		public const ushort PenetrateResistOfOuter = 36;

		public const ushort PenetrateResistOfInner = 37;

		public const ushort InnerRatio = 38;

		public const ushort RecoveryOfQiDisorder = 39;

		public const ushort BaseFavorabilityChange = 40;

		public const ushort BaseHappinessChange = 41;

		public const ushort GiftLevel = 42;

		public const ushort SolarTermType = 43;

		public const ushort EatHappinessChange = 44;

		public const ushort GroupId = 45;

		public const ushort BreakBonusEffect = 46;

		public const ushort AllowRandomCreate = 47;

		public const ushort MerchantLevel = 48;

		public const ushort Inheritable = 49;

		public const ushort ActionPointRecover = 50;

		public const ushort IsSpecial = 51;

		public const ushort BigIcon = 52;
	}

	public const ushort ArchiveFieldsCount = 5;

	public const ushort CacheFieldsCount = 0;

	public const ushort PureTemplateFieldsCount = 48;

	public const ushort WritableFieldsCount = 5;

	public const ushort ReadonlyFieldsCount = 48;

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
		{ "DirectChangeOfQiDisorder", 24 },
		{ "ConsumedFeatureMedals", 25 },
		{ "HitRateStrength", 26 },
		{ "HitRateTechnique", 27 },
		{ "HitRateSpeed", 28 },
		{ "HitRateMind", 29 },
		{ "PenetrateOfOuter", 30 },
		{ "PenetrateOfInner", 31 },
		{ "AvoidRateStrength", 32 },
		{ "AvoidRateTechnique", 33 },
		{ "AvoidRateSpeed", 34 },
		{ "AvoidRateMind", 35 },
		{ "PenetrateResistOfOuter", 36 },
		{ "PenetrateResistOfInner", 37 },
		{ "InnerRatio", 38 },
		{ "RecoveryOfQiDisorder", 39 },
		{ "BaseFavorabilityChange", 40 },
		{ "BaseHappinessChange", 41 },
		{ "GiftLevel", 42 },
		{ "SolarTermType", 43 },
		{ "EatHappinessChange", 44 },
		{ "GroupId", 45 },
		{ "BreakBonusEffect", 46 },
		{ "AllowRandomCreate", 47 },
		{ "MerchantLevel", 48 },
		{ "Inheritable", 49 },
		{ "ActionPointRecover", 50 },
		{ "IsSpecial", 51 },
		{ "BigIcon", 52 }
	};

	public static readonly string[] FieldId2FieldName = new string[53]
	{
		"Id", "TemplateId", "MaxDurability", "CurrDurability", "ModificationState", "Name", "ItemType", "ItemSubType", "Grade", "Icon",
		"Desc", "Transferable", "Stackable", "Wagerable", "Refinable", "Poisonable", "Repairable", "BaseWeight", "BaseValue", "BasePrice",
		"DropRate", "ResourceType", "PreservationDuration", "Duration", "DirectChangeOfQiDisorder", "ConsumedFeatureMedals", "HitRateStrength", "HitRateTechnique", "HitRateSpeed", "HitRateMind",
		"PenetrateOfOuter", "PenetrateOfInner", "AvoidRateStrength", "AvoidRateTechnique", "AvoidRateSpeed", "AvoidRateMind", "PenetrateResistOfOuter", "PenetrateResistOfInner", "InnerRatio", "RecoveryOfQiDisorder",
		"BaseFavorabilityChange", "BaseHappinessChange", "GiftLevel", "SolarTermType", "EatHappinessChange", "GroupId", "BreakBonusEffect", "AllowRandomCreate", "MerchantLevel", "Inheritable",
		"ActionPointRecover", "IsSpecial", "BigIcon"
	};
}
