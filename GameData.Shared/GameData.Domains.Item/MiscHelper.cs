using System.Collections.Generic;

namespace GameData.Domains.Item;

public static class MiscHelper
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

		public const ushort Neili = 23;

		public const ushort CricketHealInjuryOdds = 24;

		public const ushort ConsumedFeatureMedals = 25;

		public const ushort MaxUseDistance = 26;

		public const ushort BaseHappinessChange = 27;

		public const ushort MakeItemSubType = 28;

		public const ushort StateBuryAmount = 29;

		public const ushort Consumable = 30;

		public const ushort BaseFavorabilityChange = 31;

		public const ushort GiftLevel = 32;

		public const ushort RequireCombatConfig = 33;

		public const ushort GroupId = 34;

		public const ushort IsSpecial = 35;

		public const ushort AllowRandomCreate = 36;

		public const ushort Inheritable = 37;

		public const ushort BreakBonusEffect = 38;

		public const ushort MerchantLevel = 39;

		public const ushort AllowBrokenLevels = 40;

		public const ushort GenerateType = 41;

		public const ushort FilterType = 42;

		public const ushort ReduceEscapeRate = 43;

		public const ushort CombatUseEffect = 44;

		public const ushort CombatPrepareUseEffect = 45;

		public const ushort GainExp = 46;
	}

	public const ushort ArchiveFieldsCount = 5;

	public const ushort CacheFieldsCount = 0;

	public const ushort PureTemplateFieldsCount = 42;

	public const ushort WritableFieldsCount = 5;

	public const ushort ReadonlyFieldsCount = 42;

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
		{ "Neili", 23 },
		{ "CricketHealInjuryOdds", 24 },
		{ "ConsumedFeatureMedals", 25 },
		{ "MaxUseDistance", 26 },
		{ "BaseHappinessChange", 27 },
		{ "MakeItemSubType", 28 },
		{ "StateBuryAmount", 29 },
		{ "Consumable", 30 },
		{ "BaseFavorabilityChange", 31 },
		{ "GiftLevel", 32 },
		{ "RequireCombatConfig", 33 },
		{ "GroupId", 34 },
		{ "IsSpecial", 35 },
		{ "AllowRandomCreate", 36 },
		{ "Inheritable", 37 },
		{ "BreakBonusEffect", 38 },
		{ "MerchantLevel", 39 },
		{ "AllowBrokenLevels", 40 },
		{ "GenerateType", 41 },
		{ "FilterType", 42 },
		{ "ReduceEscapeRate", 43 },
		{ "CombatUseEffect", 44 },
		{ "CombatPrepareUseEffect", 45 },
		{ "GainExp", 46 }
	};

	public static readonly string[] FieldId2FieldName = new string[47]
	{
		"Id", "TemplateId", "MaxDurability", "CurrDurability", "ModificationState", "Name", "ItemType", "ItemSubType", "Grade", "Icon",
		"Desc", "Transferable", "Stackable", "Wagerable", "Refinable", "Poisonable", "Repairable", "BaseWeight", "BaseValue", "BasePrice",
		"DropRate", "ResourceType", "PreservationDuration", "Neili", "CricketHealInjuryOdds", "ConsumedFeatureMedals", "MaxUseDistance", "BaseHappinessChange", "MakeItemSubType", "StateBuryAmount",
		"Consumable", "BaseFavorabilityChange", "GiftLevel", "RequireCombatConfig", "GroupId", "IsSpecial", "AllowRandomCreate", "Inheritable", "BreakBonusEffect", "MerchantLevel",
		"AllowBrokenLevels", "GenerateType", "FilterType", "ReduceEscapeRate", "CombatUseEffect", "CombatPrepareUseEffect", "GainExp"
	};
}
