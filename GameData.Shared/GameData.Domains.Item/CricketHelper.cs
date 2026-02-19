using System.Collections.Generic;

namespace GameData.Domains.Item;

public static class CricketHelper
{
	public static class FieldIds
	{
		public const ushort Id = 0;

		public const ushort TemplateId = 1;

		public const ushort MaxDurability = 2;

		public const ushort CurrDurability = 3;

		public const ushort ModificationState = 4;

		public const ushort ColorId = 5;

		public const ushort PartId = 6;

		public const ushort Injuries = 7;

		public const ushort WinsCount = 8;

		public const ushort LossesCount = 9;

		public const ushort BestEnemyColorId = 10;

		public const ushort BestEnemyPartId = 11;

		public const ushort Age = 12;

		public const ushort Name = 13;

		public const ushort ItemType = 14;

		public const ushort ItemSubType = 15;

		public const ushort Grade = 16;

		public const ushort Icon = 17;

		public const ushort Desc = 18;

		public const ushort Transferable = 19;

		public const ushort Stackable = 20;

		public const ushort Wagerable = 21;

		public const ushort Refinable = 22;

		public const ushort Poisonable = 23;

		public const ushort Repairable = 24;

		public const ushort BaseWeight = 25;

		public const ushort BaseValue = 26;

		public const ushort BasePrice = 27;

		public const ushort DropRate = 28;

		public const ushort ResourceType = 29;

		public const ushort PreservationDuration = 30;

		public const ushort GiftLevel = 31;

		public const ushort BaseFavorabilityChange = 32;

		public const ushort BaseHappinessChange = 33;

		public const ushort IsSpecial = 34;

		public const ushort AllowRandomCreate = 35;

		public const ushort Inheritable = 36;

		public const ushort GroupId = 37;

		public const ushort MerchantLevel = 38;
	}

	public const ushort ArchiveFieldsCount = 13;

	public const ushort CacheFieldsCount = 0;

	public const ushort PureTemplateFieldsCount = 26;

	public const ushort WritableFieldsCount = 13;

	public const ushort ReadonlyFieldsCount = 26;

	public static readonly Dictionary<string, ushort> FieldName2FieldId = new Dictionary<string, ushort>
	{
		{ "Id", 0 },
		{ "TemplateId", 1 },
		{ "MaxDurability", 2 },
		{ "CurrDurability", 3 },
		{ "ModificationState", 4 },
		{ "ColorId", 5 },
		{ "PartId", 6 },
		{ "Injuries", 7 },
		{ "WinsCount", 8 },
		{ "LossesCount", 9 },
		{ "BestEnemyColorId", 10 },
		{ "BestEnemyPartId", 11 },
		{ "Age", 12 },
		{ "Name", 13 },
		{ "ItemType", 14 },
		{ "ItemSubType", 15 },
		{ "Grade", 16 },
		{ "Icon", 17 },
		{ "Desc", 18 },
		{ "Transferable", 19 },
		{ "Stackable", 20 },
		{ "Wagerable", 21 },
		{ "Refinable", 22 },
		{ "Poisonable", 23 },
		{ "Repairable", 24 },
		{ "BaseWeight", 25 },
		{ "BaseValue", 26 },
		{ "BasePrice", 27 },
		{ "DropRate", 28 },
		{ "ResourceType", 29 },
		{ "PreservationDuration", 30 },
		{ "GiftLevel", 31 },
		{ "BaseFavorabilityChange", 32 },
		{ "BaseHappinessChange", 33 },
		{ "IsSpecial", 34 },
		{ "AllowRandomCreate", 35 },
		{ "Inheritable", 36 },
		{ "GroupId", 37 },
		{ "MerchantLevel", 38 }
	};

	public static readonly string[] FieldId2FieldName = new string[39]
	{
		"Id", "TemplateId", "MaxDurability", "CurrDurability", "ModificationState", "ColorId", "PartId", "Injuries", "WinsCount", "LossesCount",
		"BestEnemyColorId", "BestEnemyPartId", "Age", "Name", "ItemType", "ItemSubType", "Grade", "Icon", "Desc", "Transferable",
		"Stackable", "Wagerable", "Refinable", "Poisonable", "Repairable", "BaseWeight", "BaseValue", "BasePrice", "DropRate", "ResourceType",
		"PreservationDuration", "GiftLevel", "BaseFavorabilityChange", "BaseHappinessChange", "IsSpecial", "AllowRandomCreate", "Inheritable", "GroupId", "MerchantLevel"
	};
}
