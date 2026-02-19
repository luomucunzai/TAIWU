using System.Collections.Generic;

namespace GameData.Domains.Item;

public static class SkillBookHelper
{
	public static class FieldIds
	{
		public const ushort Id = 0;

		public const ushort TemplateId = 1;

		public const ushort MaxDurability = 2;

		public const ushort CurrDurability = 3;

		public const ushort ModificationState = 4;

		public const ushort PageTypes = 5;

		public const ushort PageIncompleteState = 6;

		public const ushort Name = 7;

		public const ushort ItemType = 8;

		public const ushort ItemSubType = 9;

		public const ushort Grade = 10;

		public const ushort Icon = 11;

		public const ushort Desc = 12;

		public const ushort Transferable = 13;

		public const ushort Stackable = 14;

		public const ushort Wagerable = 15;

		public const ushort Refinable = 16;

		public const ushort Poisonable = 17;

		public const ushort Repairable = 18;

		public const ushort BaseWeight = 19;

		public const ushort BaseValue = 20;

		public const ushort BasePrice = 21;

		public const ushort DropRate = 22;

		public const ushort ResourceType = 23;

		public const ushort PreservationDuration = 24;

		public const ushort LifeSkillType = 25;

		public const ushort LifeSkillTemplateId = 26;

		public const ushort CombatSkillType = 27;

		public const ushort CombatSkillTemplateId = 28;

		public const ushort LegacyPoint = 29;

		public const ushort ReferenceBooksWithBonus = 30;

		public const ushort GiftLevel = 31;

		public const ushort BaseFavorabilityChange = 32;

		public const ushort BaseHappinessChange = 33;

		public const ushort AllowRandomCreate = 34;

		public const ushort IsSpecial = 35;

		public const ushort GroupId = 36;

		public const ushort Inheritable = 37;

		public const ushort BreakBonusEffect = 38;

		public const ushort MerchantLevel = 39;
	}

	public const ushort ArchiveFieldsCount = 7;

	public const ushort CacheFieldsCount = 0;

	public const ushort PureTemplateFieldsCount = 33;

	public const ushort WritableFieldsCount = 7;

	public const ushort ReadonlyFieldsCount = 33;

	public static readonly Dictionary<string, ushort> FieldName2FieldId = new Dictionary<string, ushort>
	{
		{ "Id", 0 },
		{ "TemplateId", 1 },
		{ "MaxDurability", 2 },
		{ "CurrDurability", 3 },
		{ "ModificationState", 4 },
		{ "PageTypes", 5 },
		{ "PageIncompleteState", 6 },
		{ "Name", 7 },
		{ "ItemType", 8 },
		{ "ItemSubType", 9 },
		{ "Grade", 10 },
		{ "Icon", 11 },
		{ "Desc", 12 },
		{ "Transferable", 13 },
		{ "Stackable", 14 },
		{ "Wagerable", 15 },
		{ "Refinable", 16 },
		{ "Poisonable", 17 },
		{ "Repairable", 18 },
		{ "BaseWeight", 19 },
		{ "BaseValue", 20 },
		{ "BasePrice", 21 },
		{ "DropRate", 22 },
		{ "ResourceType", 23 },
		{ "PreservationDuration", 24 },
		{ "LifeSkillType", 25 },
		{ "LifeSkillTemplateId", 26 },
		{ "CombatSkillType", 27 },
		{ "CombatSkillTemplateId", 28 },
		{ "LegacyPoint", 29 },
		{ "ReferenceBooksWithBonus", 30 },
		{ "GiftLevel", 31 },
		{ "BaseFavorabilityChange", 32 },
		{ "BaseHappinessChange", 33 },
		{ "AllowRandomCreate", 34 },
		{ "IsSpecial", 35 },
		{ "GroupId", 36 },
		{ "Inheritable", 37 },
		{ "BreakBonusEffect", 38 },
		{ "MerchantLevel", 39 }
	};

	public static readonly string[] FieldId2FieldName = new string[40]
	{
		"Id", "TemplateId", "MaxDurability", "CurrDurability", "ModificationState", "PageTypes", "PageIncompleteState", "Name", "ItemType", "ItemSubType",
		"Grade", "Icon", "Desc", "Transferable", "Stackable", "Wagerable", "Refinable", "Poisonable", "Repairable", "BaseWeight",
		"BaseValue", "BasePrice", "DropRate", "ResourceType", "PreservationDuration", "LifeSkillType", "LifeSkillTemplateId", "CombatSkillType", "CombatSkillTemplateId", "LegacyPoint",
		"ReferenceBooksWithBonus", "GiftLevel", "BaseFavorabilityChange", "BaseHappinessChange", "AllowRandomCreate", "IsSpecial", "GroupId", "Inheritable", "BreakBonusEffect", "MerchantLevel"
	};
}
