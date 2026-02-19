using System;
using System.Collections.Generic;
using Config.Common;
using Config.ConfigCells.Character;
using GameData.Domains.Character;
using GameData.Utilities;

namespace Config;

[Serializable]
public class OrganizationMember : ConfigData<OrganizationMemberItem, short>
{
	public static class DefKey
	{
		public const short None = 0;

		public const short XiangshuInfected1 = 1;

		public const short XiangshuInfected2 = 2;

		public const short XiangshuInfected3 = 3;

		public const short XiangshuInfected4 = 4;

		public const short XiangshuInfected5 = 5;

		public const short XiangshuInfected6 = 6;

		public const short XiangshuInfected7 = 7;

		public const short XiangshuInfected8 = 8;

		public const short XiangshuInfected9 = 9;

		public const short Taiwu = 10;

		public const short TianyinPavilionDisciple = 114;

		public const short WuxianLeader = 145;

		public const short WuxianSaintness = 146;
	}

	public static class DefValue
	{
		public static OrganizationMemberItem None => Instance[(short)0];

		public static OrganizationMemberItem XiangshuInfected1 => Instance[(short)1];

		public static OrganizationMemberItem XiangshuInfected2 => Instance[(short)2];

		public static OrganizationMemberItem XiangshuInfected3 => Instance[(short)3];

		public static OrganizationMemberItem XiangshuInfected4 => Instance[(short)4];

		public static OrganizationMemberItem XiangshuInfected5 => Instance[(short)5];

		public static OrganizationMemberItem XiangshuInfected6 => Instance[(short)6];

		public static OrganizationMemberItem XiangshuInfected7 => Instance[(short)7];

		public static OrganizationMemberItem XiangshuInfected8 => Instance[(short)8];

		public static OrganizationMemberItem XiangshuInfected9 => Instance[(short)9];

		public static OrganizationMemberItem Taiwu => Instance[(short)10];

		public static OrganizationMemberItem TianyinPavilionDisciple => Instance[(short)114];

		public static OrganizationMemberItem WuxianLeader => Instance[(short)145];

		public static OrganizationMemberItem WuxianSaintness => Instance[(short)146];
	}

	public static OrganizationMember Instance = new OrganizationMember();

	private readonly HashSet<string> RequiredFields = new HashSet<string>
	{
		"Organization", "FavoriteClothingIds", "HatedClothingIds", "MinionGroupId", "Equipment", "Clothing", "Inventory", "CombatSkills", "PreferProfessions", "CraftTypes",
		"TemplateId", "GradeName", "Grade", "Amount", "UpAmount", "DownAmount", "ResourcesAdjust", "LifeSkillsAdjust", "LifeSkillGradeLimit", "CombatSkillsAdjust",
		"MainAttributesAdjust"
	};

	internal override int ToInt(short value)
	{
		return value;
	}

	internal override short ToTemplateId(int value)
	{
		return (short)value;
	}

	private void CreateItems0()
	{
		List<OrganizationMemberItem> dataArray = _dataArray;
		int[] monasticTitleSuffixes = new int[2] { 1, 2 };
		List<short> list = new List<short>();
		List<short> favoriteClothingIds = list;
		list = new List<short>();
		List<short> hatedClothingIds = list;
		int[] spouseAnonymousTitles = new int[2] { 3, 4 };
		short[] initialAges = new short[4] { -1, -1, -1, -1 };
		PresetEquipmentItemWithProb[] equipment = new PresetEquipmentItemWithProb[8]
		{
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Accessory", -1, 0),
			new PresetEquipmentItemWithProb("Accessory", -1, 0),
			new PresetEquipmentItemWithProb("Accessory", -1, 0),
			new PresetEquipmentItemWithProb("Carrier", -1, 0)
		};
		PresetEquipmentItem clothing = new PresetEquipmentItem("Clothing", -1);
		List<PresetInventoryItem> list2 = new List<PresetInventoryItem>();
		List<PresetInventoryItem> inventory = list2;
		List<PresetOrgMemberCombatSkill> list3 = new List<PresetOrgMemberCombatSkill>();
		List<PresetOrgMemberCombatSkill> combatSkills = list3;
		sbyte[] extraCombatSkillGrids = new sbyte[5];
		short[] resourcesAdjust = new short[8] { -70, -70, -70, -70, -70, -70, -70, -70 };
		short[] lifeSkillsAdjust = new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		};
		short[] combatSkillsAdjust = new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		};
		short[] mainAttributesAdjust = new short[6] { -1, -1, -1, -1, -1, -1 };
		List<sbyte> identityInteractConfig = new List<sbyte>();
		dataArray.Add(new OrganizationMemberItem(0, 0, 0, 0, 0, 0, 0, restrictPrincipalAmount: false, -1, -1, -1, 0, 0, -1, -1, 0, 0, monasticTitleSuffixes, 0, 0, 0, 0, 0, favoriteClothingIds, hatedClothingIds, spouseAnonymousTitles, canStroll: false, -1, initialAges, equipment, clothing, inventory, combatSkills, extraCombatSkillGrids, resourcesAdjust, 5000, 1000, 0, 100, 300, lifeSkillsAdjust, 0, combatSkillsAdjust, mainAttributesAdjust, identityInteractConfig, 3, new ResourceInts(default(int), default(int), default(int), default(int), default(int), default(int), default(int), default(int)), new IntPair[0], null));
		List<OrganizationMemberItem> dataArray2 = _dataArray;
		int[] monasticTitleSuffixes2 = new int[2] { 6, 7 };
		list = new List<short>();
		List<short> favoriteClothingIds2 = list;
		list = new List<short>();
		List<short> hatedClothingIds2 = list;
		int[] spouseAnonymousTitles2 = new int[2] { 8, 9 };
		short[] initialAges2 = new short[4] { -1, -1, -1, -1 };
		PresetEquipmentItemWithProb[] equipment2 = new PresetEquipmentItemWithProb[8]
		{
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Accessory", -1, 0),
			new PresetEquipmentItemWithProb("Accessory", -1, 0),
			new PresetEquipmentItemWithProb("Accessory", -1, 0),
			new PresetEquipmentItemWithProb("Carrier", -1, 0)
		};
		PresetEquipmentItem clothing2 = new PresetEquipmentItem("Clothing", -1);
		list2 = new List<PresetInventoryItem>();
		List<PresetInventoryItem> inventory2 = list2;
		list3 = new List<PresetOrgMemberCombatSkill>();
		List<PresetOrgMemberCombatSkill> combatSkills2 = list3;
		sbyte[] extraCombatSkillGrids2 = new sbyte[5] { 10, 10, 10, 10, 10 };
		short[] resourcesAdjust2 = new short[8];
		short[] lifeSkillsAdjust2 = new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		};
		short[] combatSkillsAdjust2 = new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		};
		short[] mainAttributesAdjust2 = new short[6] { -1, -1, -1, -1, -1, -1 };
		identityInteractConfig = new List<sbyte>();
		dataArray2.Add(new OrganizationMemberItem(1, 5, 20, 8, 0, 0, 0, restrictPrincipalAmount: false, -1, -1, -1, -1, 0, -1, -1, 0, 0, monasticTitleSuffixes2, 0, 0, 0, 0, 0, favoriteClothingIds2, hatedClothingIds2, spouseAnonymousTitles2, canStroll: false, 208, initialAges2, equipment2, clothing2, inventory2, combatSkills2, extraCombatSkillGrids2, resourcesAdjust2, 0, 0, 0, 100, 61500, lifeSkillsAdjust2, 0, combatSkillsAdjust2, mainAttributesAdjust2, identityInteractConfig, 3, new ResourceInts(default(int), default(int), default(int), default(int), default(int), default(int), default(int), default(int)), new IntPair[0], null));
		List<OrganizationMemberItem> dataArray3 = _dataArray;
		int[] monasticTitleSuffixes3 = new int[2] { 10, 11 };
		list = new List<short>();
		List<short> favoriteClothingIds3 = list;
		list = new List<short>();
		List<short> hatedClothingIds3 = list;
		int[] spouseAnonymousTitles3 = new int[2] { 12, 13 };
		short[] initialAges3 = new short[4] { -1, -1, -1, -1 };
		PresetEquipmentItemWithProb[] equipment3 = new PresetEquipmentItemWithProb[8]
		{
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Accessory", -1, 0),
			new PresetEquipmentItemWithProb("Accessory", -1, 0),
			new PresetEquipmentItemWithProb("Accessory", -1, 0),
			new PresetEquipmentItemWithProb("Carrier", -1, 0)
		};
		PresetEquipmentItem clothing3 = new PresetEquipmentItem("Clothing", -1);
		list2 = new List<PresetInventoryItem>();
		List<PresetInventoryItem> inventory3 = list2;
		list3 = new List<PresetOrgMemberCombatSkill>();
		List<PresetOrgMemberCombatSkill> combatSkills3 = list3;
		sbyte[] extraCombatSkillGrids3 = new sbyte[5] { 8, 8, 8, 8, 8 };
		short[] resourcesAdjust3 = new short[8];
		short[] lifeSkillsAdjust3 = new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		};
		short[] combatSkillsAdjust3 = new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		};
		short[] mainAttributesAdjust3 = new short[6] { -1, -1, -1, -1, -1, -1 };
		identityInteractConfig = new List<sbyte>();
		dataArray3.Add(new OrganizationMemberItem(2, 5, 20, 7, 0, 0, 0, restrictPrincipalAmount: false, -1, -1, -1, -1, 0, -1, -1, 0, 0, monasticTitleSuffixes3, 0, 0, 0, 0, 0, favoriteClothingIds3, hatedClothingIds3, spouseAnonymousTitles3, canStroll: false, 209, initialAges3, equipment3, clothing3, inventory3, combatSkills3, extraCombatSkillGrids3, resourcesAdjust3, 0, 0, 0, 100, 42300, lifeSkillsAdjust3, 0, combatSkillsAdjust3, mainAttributesAdjust3, identityInteractConfig, 3, new ResourceInts(default(int), default(int), default(int), default(int), default(int), default(int), default(int), default(int)), new IntPair[0], null));
		List<OrganizationMemberItem> dataArray4 = _dataArray;
		int[] monasticTitleSuffixes4 = new int[2] { 14, 15 };
		list = new List<short>();
		List<short> favoriteClothingIds4 = list;
		list = new List<short>();
		List<short> hatedClothingIds4 = list;
		int[] spouseAnonymousTitles4 = new int[2] { 16, 17 };
		short[] initialAges4 = new short[4] { -1, -1, -1, -1 };
		PresetEquipmentItemWithProb[] equipment4 = new PresetEquipmentItemWithProb[8]
		{
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Accessory", -1, 0),
			new PresetEquipmentItemWithProb("Accessory", -1, 0),
			new PresetEquipmentItemWithProb("Accessory", -1, 0),
			new PresetEquipmentItemWithProb("Carrier", -1, 0)
		};
		PresetEquipmentItem clothing4 = new PresetEquipmentItem("Clothing", -1);
		list2 = new List<PresetInventoryItem>();
		List<PresetInventoryItem> inventory4 = list2;
		list3 = new List<PresetOrgMemberCombatSkill>();
		List<PresetOrgMemberCombatSkill> combatSkills4 = list3;
		sbyte[] extraCombatSkillGrids4 = new sbyte[5] { 8, 8, 8, 8, 8 };
		short[] resourcesAdjust4 = new short[8];
		short[] lifeSkillsAdjust4 = new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		};
		short[] combatSkillsAdjust4 = new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		};
		short[] mainAttributesAdjust4 = new short[6] { -1, -1, -1, -1, -1, -1 };
		identityInteractConfig = new List<sbyte>();
		dataArray4.Add(new OrganizationMemberItem(3, 5, 20, 6, 0, 0, 0, restrictPrincipalAmount: false, -1, -1, -1, -1, 0, -1, -1, 0, 0, monasticTitleSuffixes4, 0, 0, 0, 0, 0, favoriteClothingIds4, hatedClothingIds4, spouseAnonymousTitles4, canStroll: false, 210, initialAges4, equipment4, clothing4, inventory4, combatSkills4, extraCombatSkillGrids4, resourcesAdjust4, 0, 0, 0, 100, 27600, lifeSkillsAdjust4, 0, combatSkillsAdjust4, mainAttributesAdjust4, identityInteractConfig, 3, new ResourceInts(default(int), default(int), default(int), default(int), default(int), default(int), default(int), default(int)), new IntPair[0], null));
		List<OrganizationMemberItem> dataArray5 = _dataArray;
		int[] monasticTitleSuffixes5 = new int[2] { 18, 19 };
		list = new List<short>();
		List<short> favoriteClothingIds5 = list;
		list = new List<short>();
		List<short> hatedClothingIds5 = list;
		int[] spouseAnonymousTitles5 = new int[2] { 20, 21 };
		short[] initialAges5 = new short[4] { -1, -1, -1, -1 };
		PresetEquipmentItemWithProb[] equipment5 = new PresetEquipmentItemWithProb[8]
		{
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Accessory", -1, 0),
			new PresetEquipmentItemWithProb("Accessory", -1, 0),
			new PresetEquipmentItemWithProb("Accessory", -1, 0),
			new PresetEquipmentItemWithProb("Carrier", -1, 0)
		};
		PresetEquipmentItem clothing5 = new PresetEquipmentItem("Clothing", -1);
		list2 = new List<PresetInventoryItem>();
		List<PresetInventoryItem> inventory5 = list2;
		list3 = new List<PresetOrgMemberCombatSkill>();
		List<PresetOrgMemberCombatSkill> combatSkills5 = list3;
		sbyte[] extraCombatSkillGrids5 = new sbyte[5] { 6, 6, 6, 6, 6 };
		short[] resourcesAdjust5 = new short[8];
		short[] lifeSkillsAdjust5 = new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		};
		short[] combatSkillsAdjust5 = new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		};
		short[] mainAttributesAdjust5 = new short[6] { -1, -1, -1, -1, -1, -1 };
		identityInteractConfig = new List<sbyte>();
		dataArray5.Add(new OrganizationMemberItem(4, 5, 20, 5, 0, 0, 0, restrictPrincipalAmount: false, -1, -1, -1, -1, 0, -1, -1, 0, 0, monasticTitleSuffixes5, 0, 0, 0, 0, 0, favoriteClothingIds5, hatedClothingIds5, spouseAnonymousTitles5, canStroll: false, 211, initialAges5, equipment5, clothing5, inventory5, combatSkills5, extraCombatSkillGrids5, resourcesAdjust5, 0, 0, 0, 100, 16800, lifeSkillsAdjust5, 0, combatSkillsAdjust5, mainAttributesAdjust5, identityInteractConfig, 3, new ResourceInts(default(int), default(int), default(int), default(int), default(int), default(int), default(int), default(int)), new IntPair[0], null));
		List<OrganizationMemberItem> dataArray6 = _dataArray;
		int[] monasticTitleSuffixes6 = new int[2] { 22, 23 };
		list = new List<short>();
		List<short> favoriteClothingIds6 = list;
		list = new List<short>();
		List<short> hatedClothingIds6 = list;
		int[] spouseAnonymousTitles6 = new int[2] { 24, 25 };
		short[] initialAges6 = new short[4] { -1, -1, -1, -1 };
		PresetEquipmentItemWithProb[] equipment6 = new PresetEquipmentItemWithProb[8]
		{
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Accessory", -1, 0),
			new PresetEquipmentItemWithProb("Accessory", -1, 0),
			new PresetEquipmentItemWithProb("Accessory", -1, 0),
			new PresetEquipmentItemWithProb("Carrier", -1, 0)
		};
		PresetEquipmentItem clothing6 = new PresetEquipmentItem("Clothing", -1);
		list2 = new List<PresetInventoryItem>();
		List<PresetInventoryItem> inventory6 = list2;
		list3 = new List<PresetOrgMemberCombatSkill>();
		List<PresetOrgMemberCombatSkill> combatSkills6 = list3;
		sbyte[] extraCombatSkillGrids6 = new sbyte[5] { 4, 4, 4, 4, 4 };
		short[] resourcesAdjust6 = new short[8];
		short[] lifeSkillsAdjust6 = new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		};
		short[] combatSkillsAdjust6 = new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		};
		short[] mainAttributesAdjust6 = new short[6] { -1, -1, -1, -1, -1, -1 };
		identityInteractConfig = new List<sbyte>();
		dataArray6.Add(new OrganizationMemberItem(5, 5, 20, 4, 0, 0, 0, restrictPrincipalAmount: false, -1, -1, -1, -1, 0, -1, -1, 0, 0, monasticTitleSuffixes6, 0, 0, 0, 0, 0, favoriteClothingIds6, hatedClothingIds6, spouseAnonymousTitles6, canStroll: false, 212, initialAges6, equipment6, clothing6, inventory6, combatSkills6, extraCombatSkillGrids6, resourcesAdjust6, 0, 0, 0, 100, 9300, lifeSkillsAdjust6, 0, combatSkillsAdjust6, mainAttributesAdjust6, identityInteractConfig, 3, new ResourceInts(default(int), default(int), default(int), default(int), default(int), default(int), default(int), default(int)), new IntPair[0], null));
		List<OrganizationMemberItem> dataArray7 = _dataArray;
		int[] monasticTitleSuffixes7 = new int[2] { 26, 27 };
		list = new List<short>();
		List<short> favoriteClothingIds7 = list;
		list = new List<short>();
		List<short> hatedClothingIds7 = list;
		int[] spouseAnonymousTitles7 = new int[2] { 28, 29 };
		short[] initialAges7 = new short[4] { -1, -1, -1, -1 };
		PresetEquipmentItemWithProb[] equipment7 = new PresetEquipmentItemWithProb[8]
		{
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Accessory", -1, 0),
			new PresetEquipmentItemWithProb("Accessory", -1, 0),
			new PresetEquipmentItemWithProb("Accessory", -1, 0),
			new PresetEquipmentItemWithProb("Carrier", -1, 0)
		};
		PresetEquipmentItem clothing7 = new PresetEquipmentItem("Clothing", -1);
		list2 = new List<PresetInventoryItem>();
		List<PresetInventoryItem> inventory7 = list2;
		list3 = new List<PresetOrgMemberCombatSkill>();
		List<PresetOrgMemberCombatSkill> combatSkills7 = list3;
		sbyte[] extraCombatSkillGrids7 = new sbyte[5] { 4, 4, 4, 4, 4 };
		short[] resourcesAdjust7 = new short[8];
		short[] lifeSkillsAdjust7 = new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		};
		short[] combatSkillsAdjust7 = new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		};
		short[] mainAttributesAdjust7 = new short[6] { -1, -1, -1, -1, -1, -1 };
		identityInteractConfig = new List<sbyte>();
		dataArray7.Add(new OrganizationMemberItem(6, 5, 20, 3, 0, 0, 0, restrictPrincipalAmount: false, -1, -1, -1, -1, 0, -1, -1, 0, 0, monasticTitleSuffixes7, 0, 0, 0, 0, 0, favoriteClothingIds7, hatedClothingIds7, spouseAnonymousTitles7, canStroll: false, 213, initialAges7, equipment7, clothing7, inventory7, combatSkills7, extraCombatSkillGrids7, resourcesAdjust7, 0, 0, 0, 100, 4500, lifeSkillsAdjust7, 0, combatSkillsAdjust7, mainAttributesAdjust7, identityInteractConfig, 3, new ResourceInts(default(int), default(int), default(int), default(int), default(int), default(int), default(int), default(int)), new IntPair[0], null));
		List<OrganizationMemberItem> dataArray8 = _dataArray;
		int[] monasticTitleSuffixes8 = new int[2] { 30, 31 };
		list = new List<short>();
		List<short> favoriteClothingIds8 = list;
		list = new List<short>();
		List<short> hatedClothingIds8 = list;
		int[] spouseAnonymousTitles8 = new int[2] { 32, 33 };
		short[] initialAges8 = new short[4] { -1, -1, -1, -1 };
		PresetEquipmentItemWithProb[] equipment8 = new PresetEquipmentItemWithProb[8]
		{
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Accessory", -1, 0),
			new PresetEquipmentItemWithProb("Accessory", -1, 0),
			new PresetEquipmentItemWithProb("Accessory", -1, 0),
			new PresetEquipmentItemWithProb("Carrier", -1, 0)
		};
		PresetEquipmentItem clothing8 = new PresetEquipmentItem("Clothing", -1);
		list2 = new List<PresetInventoryItem>();
		List<PresetInventoryItem> inventory8 = list2;
		list3 = new List<PresetOrgMemberCombatSkill>();
		List<PresetOrgMemberCombatSkill> combatSkills8 = list3;
		sbyte[] extraCombatSkillGrids8 = new sbyte[5] { 2, 2, 2, 2, 2 };
		short[] resourcesAdjust8 = new short[8];
		short[] lifeSkillsAdjust8 = new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		};
		short[] combatSkillsAdjust8 = new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		};
		short[] mainAttributesAdjust8 = new short[6] { -1, -1, -1, -1, -1, -1 };
		identityInteractConfig = new List<sbyte>();
		dataArray8.Add(new OrganizationMemberItem(7, 5, 20, 2, 0, 0, 0, restrictPrincipalAmount: false, -1, -1, -1, -1, 0, -1, -1, 0, 0, monasticTitleSuffixes8, 0, 0, 0, 0, 0, favoriteClothingIds8, hatedClothingIds8, spouseAnonymousTitles8, canStroll: false, 214, initialAges8, equipment8, clothing8, inventory8, combatSkills8, extraCombatSkillGrids8, resourcesAdjust8, 0, 0, 0, 100, 1800, lifeSkillsAdjust8, 0, combatSkillsAdjust8, mainAttributesAdjust8, identityInteractConfig, 3, new ResourceInts(default(int), default(int), default(int), default(int), default(int), default(int), default(int), default(int)), new IntPair[0], null));
		List<OrganizationMemberItem> dataArray9 = _dataArray;
		int[] monasticTitleSuffixes9 = new int[2] { 34, 35 };
		list = new List<short>();
		List<short> favoriteClothingIds9 = list;
		list = new List<short>();
		List<short> hatedClothingIds9 = list;
		int[] spouseAnonymousTitles9 = new int[2] { 36, 37 };
		short[] initialAges9 = new short[4] { -1, -1, -1, -1 };
		PresetEquipmentItemWithProb[] equipment9 = new PresetEquipmentItemWithProb[8]
		{
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Accessory", -1, 0),
			new PresetEquipmentItemWithProb("Accessory", -1, 0),
			new PresetEquipmentItemWithProb("Accessory", -1, 0),
			new PresetEquipmentItemWithProb("Carrier", -1, 0)
		};
		PresetEquipmentItem clothing9 = new PresetEquipmentItem("Clothing", -1);
		list2 = new List<PresetInventoryItem>();
		List<PresetInventoryItem> inventory9 = list2;
		list3 = new List<PresetOrgMemberCombatSkill>();
		List<PresetOrgMemberCombatSkill> combatSkills9 = list3;
		sbyte[] extraCombatSkillGrids9 = new sbyte[5];
		short[] resourcesAdjust9 = new short[8];
		short[] lifeSkillsAdjust9 = new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		};
		short[] combatSkillsAdjust9 = new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		};
		short[] mainAttributesAdjust9 = new short[6] { -1, -1, -1, -1, -1, -1 };
		identityInteractConfig = new List<sbyte>();
		dataArray9.Add(new OrganizationMemberItem(8, 5, 20, 1, 0, 0, 0, restrictPrincipalAmount: false, -1, -1, -1, -1, 0, -1, -1, 0, 0, monasticTitleSuffixes9, 0, 0, 0, 0, 0, favoriteClothingIds9, hatedClothingIds9, spouseAnonymousTitles9, canStroll: false, 215, initialAges9, equipment9, clothing9, inventory9, combatSkills9, extraCombatSkillGrids9, resourcesAdjust9, 0, 0, 0, 100, 600, lifeSkillsAdjust9, 0, combatSkillsAdjust9, mainAttributesAdjust9, identityInteractConfig, 3, new ResourceInts(default(int), default(int), default(int), default(int), default(int), default(int), default(int), default(int)), new IntPair[0], null));
		List<OrganizationMemberItem> dataArray10 = _dataArray;
		int[] monasticTitleSuffixes10 = new int[2] { 38, 39 };
		list = new List<short>();
		List<short> favoriteClothingIds10 = list;
		list = new List<short>();
		List<short> hatedClothingIds10 = list;
		int[] spouseAnonymousTitles10 = new int[2] { 40, 41 };
		short[] initialAges10 = new short[4] { -1, -1, -1, -1 };
		PresetEquipmentItemWithProb[] equipment10 = new PresetEquipmentItemWithProb[8]
		{
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Accessory", -1, 0),
			new PresetEquipmentItemWithProb("Accessory", -1, 0),
			new PresetEquipmentItemWithProb("Accessory", -1, 0),
			new PresetEquipmentItemWithProb("Carrier", -1, 0)
		};
		PresetEquipmentItem clothing10 = new PresetEquipmentItem("Clothing", -1);
		list2 = new List<PresetInventoryItem>();
		List<PresetInventoryItem> inventory10 = list2;
		list3 = new List<PresetOrgMemberCombatSkill>();
		List<PresetOrgMemberCombatSkill> combatSkills10 = list3;
		sbyte[] extraCombatSkillGrids10 = new sbyte[5];
		short[] resourcesAdjust10 = new short[8];
		short[] lifeSkillsAdjust10 = new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		};
		short[] combatSkillsAdjust10 = new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		};
		short[] mainAttributesAdjust10 = new short[6] { -1, -1, -1, -1, -1, -1 };
		identityInteractConfig = new List<sbyte>();
		dataArray10.Add(new OrganizationMemberItem(9, 5, 20, 0, 0, 0, 0, restrictPrincipalAmount: false, -1, -1, -1, -1, 0, -1, -1, 0, 0, monasticTitleSuffixes10, 0, 0, 0, 0, 0, favoriteClothingIds10, hatedClothingIds10, spouseAnonymousTitles10, canStroll: false, 216, initialAges10, equipment10, clothing10, inventory10, combatSkills10, extraCombatSkillGrids10, resourcesAdjust10, 0, 0, 0, 100, 300, lifeSkillsAdjust10, 0, combatSkillsAdjust10, mainAttributesAdjust10, identityInteractConfig, 3, new ResourceInts(default(int), default(int), default(int), default(int), default(int), default(int), default(int), default(int)), new IntPair[0], null));
		List<OrganizationMemberItem> dataArray11 = _dataArray;
		int[] monasticTitleSuffixes11 = new int[2] { 43, 44 };
		list = new List<short>();
		List<short> favoriteClothingIds11 = list;
		list = new List<short>();
		List<short> hatedClothingIds11 = list;
		int[] spouseAnonymousTitles11 = new int[2] { 45, 46 };
		short[] initialAges11 = new short[4] { -1, -1, -1, -1 };
		PresetEquipmentItemWithProb[] equipment11 = new PresetEquipmentItemWithProb[8]
		{
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Accessory", -1, 0),
			new PresetEquipmentItemWithProb("Accessory", -1, 0),
			new PresetEquipmentItemWithProb("Accessory", -1, 0),
			new PresetEquipmentItemWithProb("Carrier", -1, 0)
		};
		PresetEquipmentItem clothing11 = new PresetEquipmentItem("Clothing", -1);
		list2 = new List<PresetInventoryItem>();
		List<PresetInventoryItem> inventory11 = list2;
		list3 = new List<PresetOrgMemberCombatSkill>();
		List<PresetOrgMemberCombatSkill> combatSkills11 = list3;
		sbyte[] extraCombatSkillGrids11 = new sbyte[5];
		short[] resourcesAdjust11 = new short[8] { -70, -70, -70, -70, -70, -70, -70, -70 };
		short[] lifeSkillsAdjust11 = new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		};
		short[] combatSkillsAdjust11 = new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		};
		short[] mainAttributesAdjust11 = new short[6] { -1, -1, -1, -1, -1, -1 };
		identityInteractConfig = new List<sbyte>();
		dataArray11.Add(new OrganizationMemberItem(10, 42, 16, 8, 1, 1, 1, restrictPrincipalAmount: true, -1, -1, -1, 0, 0, -1, -1, 0, 0, monasticTitleSuffixes11, 0, 0, 0, 0, 0, favoriteClothingIds11, hatedClothingIds11, spouseAnonymousTitles11, canStroll: false, -1, initialAges11, equipment11, clothing11, inventory11, combatSkills11, extraCombatSkillGrids11, resourcesAdjust11, 160000, 576000, 100, 100, 61500, lifeSkillsAdjust11, 0, combatSkillsAdjust11, mainAttributesAdjust11, identityInteractConfig, 0, new ResourceInts(default(int), default(int), default(int), default(int), default(int), default(int), default(int), default(int)), new IntPair[0], null));
		List<OrganizationMemberItem> dataArray12 = _dataArray;
		int[] monasticTitleSuffixes12 = new int[2] { 48, 49 };
		list = new List<short>();
		List<short> favoriteClothingIds12 = list;
		list = new List<short>();
		List<short> hatedClothingIds12 = list;
		int[] spouseAnonymousTitles12 = new int[2] { 50, 51 };
		short[] initialAges12 = new short[4] { 42, 50, 58, 66 };
		PresetEquipmentItemWithProb[] equipment12 = new PresetEquipmentItemWithProb[8]
		{
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Accessory", -1, 0),
			new PresetEquipmentItemWithProb("Accessory", -1, 0),
			new PresetEquipmentItemWithProb("Accessory", -1, 0),
			new PresetEquipmentItemWithProb("Carrier", -1, 0)
		};
		PresetEquipmentItem clothing12 = new PresetEquipmentItem("Clothing", -1);
		list2 = new List<PresetInventoryItem>();
		List<PresetInventoryItem> inventory12 = list2;
		list3 = new List<PresetOrgMemberCombatSkill>();
		dataArray12.Add(new OrganizationMemberItem(11, 47, 16, 7, 1, 1, 1, restrictPrincipalAmount: false, -1, -1, -1, 0, 0, -1, 0, 0, 0, monasticTitleSuffixes12, 0, 14, 10000, 0, -100, favoriteClothingIds12, hatedClothingIds12, spouseAnonymousTitles12, canStroll: false, -1, initialAges12, equipment12, clothing12, inventory12, list3, new sbyte[5], new short[8] { -70, -70, -70, -70, -70, -70, -70, -70 }, 100000, 288000, 35, 100, 42300, new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		}, 6, new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		}, new short[6] { -1, -1, -1, -1, -1, -1 }, new List<sbyte> { 51 }, 3, new ResourceInts(default(int), default(int), default(int), default(int), default(int), default(int), default(int), default(int)), new IntPair[18]
		{
			new IntPair(17, 9),
			new IntPair(8, 9),
			new IntPair(5, 1),
			new IntPair(6, 1),
			new IntPair(15, 9),
			new IntPair(16, 15),
			new IntPair(7, 15),
			new IntPair(14, 1),
			new IntPair(12, 1),
			new IntPair(4, 9),
			new IntPair(3, 1),
			new IntPair(13, 1),
			new IntPair(10, 18),
			new IntPair(2, 3),
			new IntPair(1, 6),
			new IntPair(11, 3),
			new IntPair(0, 3),
			new IntPair(9, 3)
		}, null));
		List<OrganizationMemberItem> dataArray13 = _dataArray;
		int[] monasticTitleSuffixes13 = new int[2] { 53, 54 };
		list = new List<short>();
		List<short> favoriteClothingIds13 = list;
		list = new List<short>();
		List<short> hatedClothingIds13 = list;
		int[] spouseAnonymousTitles13 = new int[2] { 55, 56 };
		short[] initialAges13 = new short[4] { 38, 45, 52, 59 };
		PresetEquipmentItemWithProb[] equipment13 = new PresetEquipmentItemWithProb[8]
		{
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Accessory", -1, 0),
			new PresetEquipmentItemWithProb("Accessory", -1, 0),
			new PresetEquipmentItemWithProb("Accessory", -1, 0),
			new PresetEquipmentItemWithProb("Carrier", -1, 0)
		};
		PresetEquipmentItem clothing13 = new PresetEquipmentItem("Clothing", -1);
		list2 = new List<PresetInventoryItem>();
		List<PresetInventoryItem> inventory13 = list2;
		list3 = new List<PresetOrgMemberCombatSkill>();
		dataArray13.Add(new OrganizationMemberItem(12, 52, 16, 6, 1, 1, 1, restrictPrincipalAmount: false, -1, -1, -1, 0, 0, -1, 0, 0, 0, monasticTitleSuffixes13, 0, 12, 8000, 0, -100, favoriteClothingIds13, hatedClothingIds13, spouseAnonymousTitles13, canStroll: false, -1, initialAges13, equipment13, clothing13, inventory13, list3, new sbyte[5], new short[8] { -70, -70, -70, -70, -70, -70, -70, -70 }, 60000, 144000, 30, 100, 27600, new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		}, 6, new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		}, new short[6] { -1, -1, -1, -1, -1, -1 }, new List<sbyte> { 50 }, 3, new ResourceInts(default(int), default(int), default(int), default(int), default(int), default(int), default(int), default(int)), new IntPair[18]
		{
			new IntPair(17, 1),
			new IntPair(8, 1),
			new IntPair(5, 3),
			new IntPair(6, 3),
			new IntPair(15, 1),
			new IntPair(16, 3),
			new IntPair(7, 27),
			new IntPair(14, 3),
			new IntPair(12, 3),
			new IntPair(4, 3),
			new IntPair(3, 30),
			new IntPair(13, 3),
			new IntPair(10, 3),
			new IntPair(2, 3),
			new IntPair(1, 6),
			new IntPair(11, 6),
			new IntPair(0, 6),
			new IntPair(9, 3)
		}, null));
		List<OrganizationMemberItem> dataArray14 = _dataArray;
		int[] monasticTitleSuffixes14 = new int[2] { 58, 59 };
		list = new List<short>();
		List<short> favoriteClothingIds14 = list;
		list = new List<short>();
		List<short> hatedClothingIds14 = list;
		int[] spouseAnonymousTitles14 = new int[2] { 60, 61 };
		short[] initialAges14 = new short[4] { 28, 34, 40, 46 };
		PresetEquipmentItemWithProb[] equipment14 = new PresetEquipmentItemWithProb[8]
		{
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Accessory", -1, 0),
			new PresetEquipmentItemWithProb("Accessory", -1, 0),
			new PresetEquipmentItemWithProb("Accessory", -1, 0),
			new PresetEquipmentItemWithProb("Carrier", -1, 0)
		};
		PresetEquipmentItem clothing14 = new PresetEquipmentItem("Clothing", -1);
		list2 = new List<PresetInventoryItem>();
		List<PresetInventoryItem> inventory14 = list2;
		list3 = new List<PresetOrgMemberCombatSkill>();
		dataArray14.Add(new OrganizationMemberItem(13, 57, 16, 5, 1, 1, 1, restrictPrincipalAmount: false, -1, -1, -1, 0, 0, -1, 0, 0, 0, monasticTitleSuffixes14, 0, 10, 6000, 0, -100, favoriteClothingIds14, hatedClothingIds14, spouseAnonymousTitles14, canStroll: true, -1, initialAges14, equipment14, clothing14, inventory14, list3, new sbyte[5], new short[8] { -70, -70, -70, -70, -70, -70, -70, -70 }, 40000, 48000, 25, 100, 16800, new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		}, 5, new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		}, new short[6] { -1, -1, -1, -1, -1, -1 }, new List<sbyte> { 49, 58, 3 }, 3, new ResourceInts(default(int), default(int), default(int), default(int), default(int), default(int), default(int), default(int)), new IntPair[18]
		{
			new IntPair(17, 3),
			new IntPair(8, 21),
			new IntPair(5, 1),
			new IntPair(6, 1),
			new IntPair(15, 3),
			new IntPair(16, 21),
			new IntPair(7, 6),
			new IntPair(14, 3),
			new IntPair(12, 3),
			new IntPair(4, 30),
			new IntPair(3, 1),
			new IntPair(13, 3),
			new IntPair(10, 3),
			new IntPair(2, 3),
			new IntPair(1, 1),
			new IntPair(11, 3),
			new IntPair(0, 1),
			new IntPair(9, 1)
		}, null));
		List<OrganizationMemberItem> dataArray15 = _dataArray;
		int[] monasticTitleSuffixes15 = new int[2] { 63, 64 };
		list = new List<short>();
		List<short> favoriteClothingIds15 = list;
		list = new List<short>();
		List<short> hatedClothingIds15 = list;
		int[] spouseAnonymousTitles15 = new int[2] { 65, 66 };
		short[] initialAges15 = new short[4] { 25, 30, 35, 40 };
		PresetEquipmentItemWithProb[] equipment15 = new PresetEquipmentItemWithProb[8]
		{
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Accessory", -1, 0),
			new PresetEquipmentItemWithProb("Accessory", -1, 0),
			new PresetEquipmentItemWithProb("Accessory", -1, 0),
			new PresetEquipmentItemWithProb("Carrier", -1, 0)
		};
		PresetEquipmentItem clothing15 = new PresetEquipmentItem("Clothing", -1);
		list2 = new List<PresetInventoryItem>();
		List<PresetInventoryItem> inventory15 = list2;
		list3 = new List<PresetOrgMemberCombatSkill>();
		dataArray15.Add(new OrganizationMemberItem(14, 62, 16, 4, 2, 2, 2, restrictPrincipalAmount: false, -1, -1, -1, 0, 0, -1, 0, 0, 0, monasticTitleSuffixes15, 0, 8, 4500, 0, -100, favoriteClothingIds15, hatedClothingIds15, spouseAnonymousTitles15, canStroll: true, -1, initialAges15, equipment15, clothing15, inventory15, list3, new sbyte[5], new short[8] { -70, -70, -70, -70, -70, -70, -70, -70 }, 30000, 24000, 20, 100, 9300, new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		}, 5, new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		}, new short[6] { -1, -1, -1, -1, -1, -1 }, new List<sbyte> { 4, 48, 53 }, 3, new ResourceInts(default(int), default(int), default(int), default(int), default(int), default(int), default(int), default(int)), new IntPair[18]
		{
			new IntPair(17, 3),
			new IntPair(8, 3),
			new IntPair(5, 1),
			new IntPair(6, 1),
			new IntPair(15, 30),
			new IntPair(16, 15),
			new IntPair(7, 15),
			new IntPair(14, 1),
			new IntPair(12, 1),
			new IntPair(4, 6),
			new IntPair(3, 1),
			new IntPair(13, 3),
			new IntPair(10, 9),
			new IntPair(2, 3),
			new IntPair(1, 3),
			new IntPair(11, 9),
			new IntPair(0, 3),
			new IntPair(9, 1)
		}, null));
		List<OrganizationMemberItem> dataArray16 = _dataArray;
		int[] monasticTitleSuffixes16 = new int[2] { 68, 69 };
		list = new List<short>();
		List<short> favoriteClothingIds16 = list;
		list = new List<short>();
		List<short> hatedClothingIds16 = list;
		int[] spouseAnonymousTitles16 = new int[2] { 70, 71 };
		short[] initialAges16 = new short[4] { 22, 26, 30, 34 };
		PresetEquipmentItemWithProb[] equipment16 = new PresetEquipmentItemWithProb[8]
		{
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Accessory", -1, 0),
			new PresetEquipmentItemWithProb("Accessory", -1, 0),
			new PresetEquipmentItemWithProb("Accessory", -1, 0),
			new PresetEquipmentItemWithProb("Carrier", -1, 0)
		};
		PresetEquipmentItem clothing16 = new PresetEquipmentItem("Clothing", -1);
		list2 = new List<PresetInventoryItem>();
		List<PresetInventoryItem> inventory16 = list2;
		list3 = new List<PresetOrgMemberCombatSkill>();
		dataArray16.Add(new OrganizationMemberItem(15, 67, 16, 3, 2, 2, 2, restrictPrincipalAmount: false, -1, -1, -1, 0, 0, -1, 0, 0, 0, monasticTitleSuffixes16, 0, 6, 3000, 0, -100, favoriteClothingIds16, hatedClothingIds16, spouseAnonymousTitles16, canStroll: true, -1, initialAges16, equipment16, clothing16, inventory16, list3, new sbyte[5], new short[8] { -70, -70, -70, -70, -70, -70, -70, -70 }, 20000, 12000, 15, 100, 4500, new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		}, 4, new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		}, new short[6] { -1, -1, -1, -1, -1, -1 }, new List<sbyte> { 5, 47, 54 }, 3, new ResourceInts(default(int), default(int), default(int), default(int), default(int), default(int), default(int), default(int)), new IntPair[18]
		{
			new IntPair(17, 1),
			new IntPair(8, 6),
			new IntPair(5, 1),
			new IntPair(6, 1),
			new IntPair(15, 1),
			new IntPair(16, 6),
			new IntPair(7, 1),
			new IntPair(14, 3),
			new IntPair(12, 3),
			new IntPair(4, 12),
			new IntPair(3, 1),
			new IntPair(13, 30),
			new IntPair(10, 18),
			new IntPair(2, 1),
			new IntPair(1, 1),
			new IntPair(11, 9),
			new IntPair(0, 12),
			new IntPair(9, 1)
		}, null));
		List<OrganizationMemberItem> dataArray17 = _dataArray;
		int[] monasticTitleSuffixes17 = new int[2] { 73, 74 };
		list = new List<short>();
		List<short> favoriteClothingIds17 = list;
		list = new List<short>();
		List<short> hatedClothingIds17 = list;
		int[] spouseAnonymousTitles17 = new int[2] { 75, 76 };
		short[] initialAges17 = new short[4] { 16, 19, 22, 25 };
		PresetEquipmentItemWithProb[] equipment17 = new PresetEquipmentItemWithProb[8]
		{
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Accessory", -1, 0),
			new PresetEquipmentItemWithProb("Accessory", -1, 0),
			new PresetEquipmentItemWithProb("Accessory", -1, 0),
			new PresetEquipmentItemWithProb("Carrier", -1, 0)
		};
		PresetEquipmentItem clothing17 = new PresetEquipmentItem("Clothing", -1);
		list2 = new List<PresetInventoryItem>();
		List<PresetInventoryItem> inventory17 = list2;
		list3 = new List<PresetOrgMemberCombatSkill>();
		dataArray17.Add(new OrganizationMemberItem(16, 72, 16, 2, 2, 2, 2, restrictPrincipalAmount: false, -1, -1, -1, 0, 0, -1, 0, 0, 0, monasticTitleSuffixes17, 0, 4, 2000, 0, -100, favoriteClothingIds17, hatedClothingIds17, spouseAnonymousTitles17, canStroll: true, -1, initialAges17, equipment17, clothing17, inventory17, list3, new sbyte[5], new short[8] { -70, -70, -70, -70, -70, -70, -70, -70 }, 15000, 4000, 10, 100, 1800, new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		}, 4, new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		}, new short[6] { -1, -1, -1, -1, -1, -1 }, new List<sbyte> { 6, 46, 61 }, 3, new ResourceInts(default(int), default(int), default(int), default(int), default(int), default(int), default(int), default(int)), new IntPair[18]
		{
			new IntPair(17, 1),
			new IntPair(8, 1),
			new IntPair(5, 1),
			new IntPair(6, 1),
			new IntPair(15, 6),
			new IntPair(16, 3),
			new IntPair(7, 1),
			new IntPair(14, 1),
			new IntPair(12, 1),
			new IntPair(4, 9),
			new IntPair(3, 1),
			new IntPair(13, 3),
			new IntPair(10, 18),
			new IntPair(2, 30),
			new IntPair(1, 9),
			new IntPair(11, 15),
			new IntPair(0, 6),
			new IntPair(9, 1)
		}, null));
		List<OrganizationMemberItem> dataArray18 = _dataArray;
		int[] monasticTitleSuffixes18 = new int[2] { 78, 79 };
		list = new List<short>();
		List<short> favoriteClothingIds18 = list;
		list = new List<short>();
		List<short> hatedClothingIds18 = list;
		int[] spouseAnonymousTitles18 = new int[2] { 80, 81 };
		short[] initialAges18 = new short[4] { 14, 16, 18, 20 };
		PresetEquipmentItemWithProb[] equipment18 = new PresetEquipmentItemWithProb[8]
		{
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Accessory", -1, 0),
			new PresetEquipmentItemWithProb("Accessory", -1, 0),
			new PresetEquipmentItemWithProb("Accessory", -1, 0),
			new PresetEquipmentItemWithProb("Carrier", -1, 0)
		};
		PresetEquipmentItem clothing18 = new PresetEquipmentItem("Clothing", -1);
		list2 = new List<PresetInventoryItem>();
		List<PresetInventoryItem> inventory18 = list2;
		list3 = new List<PresetOrgMemberCombatSkill>();
		dataArray18.Add(new OrganizationMemberItem(17, 77, 16, 1, 3, 3, 3, restrictPrincipalAmount: false, -1, -1, -1, 0, 0, -1, 0, 0, 0, monasticTitleSuffixes18, 0, 2, 1000, 0, -100, favoriteClothingIds18, hatedClothingIds18, spouseAnonymousTitles18, canStroll: true, -1, initialAges18, equipment18, clothing18, inventory18, list3, new sbyte[5], new short[8] { -70, -70, -70, -70, -70, -70, -70, -70 }, 10000, 2000, 5, 100, 600, new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		}, 3, new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		}, new short[6] { -1, -1, -1, -1, -1, -1 }, new List<sbyte> { 45, 59, 62 }, 3, new ResourceInts(default(int), default(int), default(int), default(int), default(int), default(int), default(int), default(int)), new IntPair[18]
		{
			new IntPair(17, 1),
			new IntPair(8, 1),
			new IntPair(5, 1),
			new IntPair(6, 1),
			new IntPair(15, 3),
			new IntPair(16, 1),
			new IntPair(7, 1),
			new IntPair(14, 1),
			new IntPair(12, 1),
			new IntPair(4, 3),
			new IntPair(3, 1),
			new IntPair(13, 3),
			new IntPair(10, 18),
			new IntPair(2, 6),
			new IntPair(1, 15),
			new IntPair(11, 15),
			new IntPair(0, 30),
			new IntPair(9, 6)
		}, null));
		List<OrganizationMemberItem> dataArray19 = _dataArray;
		int[] monasticTitleSuffixes19 = new int[2] { 83, 84 };
		list = new List<short>();
		List<short> favoriteClothingIds19 = list;
		list = new List<short>();
		List<short> hatedClothingIds19 = list;
		int[] spouseAnonymousTitles19 = new int[2] { 85, 86 };
		short[] initialAges19 = new short[4] { 12, 13, 14, 15 };
		PresetEquipmentItemWithProb[] equipment19 = new PresetEquipmentItemWithProb[8]
		{
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Accessory", -1, 0),
			new PresetEquipmentItemWithProb("Accessory", -1, 0),
			new PresetEquipmentItemWithProb("Accessory", -1, 0),
			new PresetEquipmentItemWithProb("Carrier", -1, 0)
		};
		PresetEquipmentItem clothing19 = new PresetEquipmentItem("Clothing", -1);
		list2 = new List<PresetInventoryItem>();
		List<PresetInventoryItem> inventory19 = list2;
		list3 = new List<PresetOrgMemberCombatSkill>();
		List<PresetOrgMemberCombatSkill> combatSkills12 = list3;
		sbyte[] extraCombatSkillGrids12 = new sbyte[5];
		short[] resourcesAdjust12 = new short[8] { -70, -70, -70, -70, -70, -70, -70, -70 };
		short[] lifeSkillsAdjust12 = new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		};
		short[] combatSkillsAdjust12 = new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		};
		short[] mainAttributesAdjust12 = new short[6] { -1, -1, -1, -1, -1, -1 };
		identityInteractConfig = new List<sbyte>();
		dataArray19.Add(new OrganizationMemberItem(18, 82, 16, 0, 3, 3, 3, restrictPrincipalAmount: false, -1, -1, -1, 0, 0, -1, 0, 0, 0, monasticTitleSuffixes19, 0, 0, 500, 0, -100, favoriteClothingIds19, hatedClothingIds19, spouseAnonymousTitles19, canStroll: true, -1, initialAges19, equipment19, clothing19, inventory19, combatSkills12, extraCombatSkillGrids12, resourcesAdjust12, 5000, 1000, 1, 100, 300, lifeSkillsAdjust12, 3, combatSkillsAdjust12, mainAttributesAdjust12, identityInteractConfig, 3, new ResourceInts(default(int), default(int), default(int), default(int), default(int), default(int), default(int), default(int)), new IntPair[18]
		{
			new IntPair(17, 1),
			new IntPair(8, 3),
			new IntPair(5, 1),
			new IntPair(6, 1),
			new IntPair(15, 3),
			new IntPair(16, 1),
			new IntPair(7, 1),
			new IntPair(14, 1),
			new IntPair(12, 1),
			new IntPair(4, 1),
			new IntPair(3, 1),
			new IntPair(13, 3),
			new IntPair(10, 30),
			new IntPair(2, 9),
			new IntPair(1, 12),
			new IntPair(11, 9),
			new IntPair(0, 15),
			new IntPair(9, 15)
		}, null));
		List<OrganizationMemberItem> dataArray20 = _dataArray;
		int[] monasticTitleSuffixes20 = new int[2] { 88, 89 };
		list = new List<short>();
		List<short> favoriteClothingIds20 = list;
		list = new List<short>();
		List<short> hatedClothingIds20 = list;
		int[] spouseAnonymousTitles20 = new int[2] { 90, 91 };
		short[] initialAges20 = new short[4] { -1, -1, -1, -1 };
		PresetEquipmentItemWithProb[] equipment20 = new PresetEquipmentItemWithProb[8]
		{
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Accessory", -1, 0),
			new PresetEquipmentItemWithProb("Accessory", -1, 0),
			new PresetEquipmentItemWithProb("Accessory", -1, 0),
			new PresetEquipmentItemWithProb("Carrier", -1, 0)
		};
		PresetEquipmentItem clothing20 = new PresetEquipmentItem("Clothing", -1);
		list2 = new List<PresetInventoryItem>();
		List<PresetInventoryItem> inventory20 = list2;
		list3 = new List<PresetOrgMemberCombatSkill>();
		List<PresetOrgMemberCombatSkill> combatSkills13 = list3;
		sbyte[] extraCombatSkillGrids13 = new sbyte[5] { 8, 8, 8, 8, 8 };
		short[] resourcesAdjust13 = new short[8];
		short[] lifeSkillsAdjust13 = new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		};
		short[] combatSkillsAdjust13 = new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		};
		short[] mainAttributesAdjust13 = new short[6] { -1, -1, -1, -1, -1, -1 };
		identityInteractConfig = new List<sbyte>();
		dataArray20.Add(new OrganizationMemberItem(19, 87, 17, 8, 0, 0, 0, restrictPrincipalAmount: false, -1, -1, -1, 0, 0, -1, -1, 0, 0, monasticTitleSuffixes20, 0, 0, 0, 0, 0, favoriteClothingIds20, hatedClothingIds20, spouseAnonymousTitles20, canStroll: false, -1, initialAges20, equipment20, clothing20, inventory20, combatSkills13, extraCombatSkillGrids13, resourcesAdjust13, 0, 0, 0, 100, 61500, lifeSkillsAdjust13, 8, combatSkillsAdjust13, mainAttributesAdjust13, identityInteractConfig, 3, new ResourceInts(default(int), default(int), default(int), default(int), default(int), default(int), default(int), default(int)), new IntPair[0], null));
		List<OrganizationMemberItem> dataArray21 = _dataArray;
		int[] monasticTitleSuffixes21 = new int[2] { 93, 94 };
		list = new List<short>();
		List<short> favoriteClothingIds21 = list;
		list = new List<short>();
		List<short> hatedClothingIds21 = list;
		int[] spouseAnonymousTitles21 = new int[2] { 95, 96 };
		short[] initialAges21 = new short[4] { -1, -1, -1, -1 };
		PresetEquipmentItemWithProb[] equipment21 = new PresetEquipmentItemWithProb[8]
		{
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Accessory", -1, 0),
			new PresetEquipmentItemWithProb("Accessory", -1, 0),
			new PresetEquipmentItemWithProb("Accessory", -1, 0),
			new PresetEquipmentItemWithProb("Carrier", -1, 0)
		};
		PresetEquipmentItem clothing21 = new PresetEquipmentItem("Clothing", -1);
		list2 = new List<PresetInventoryItem>();
		List<PresetInventoryItem> inventory21 = list2;
		list3 = new List<PresetOrgMemberCombatSkill>();
		List<PresetOrgMemberCombatSkill> combatSkills14 = list3;
		sbyte[] extraCombatSkillGrids14 = new sbyte[5] { 6, 6, 6, 6, 6 };
		short[] resourcesAdjust14 = new short[8];
		short[] lifeSkillsAdjust14 = new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		};
		short[] combatSkillsAdjust14 = new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		};
		short[] mainAttributesAdjust14 = new short[6] { -1, -1, -1, -1, -1, -1 };
		identityInteractConfig = new List<sbyte>();
		dataArray21.Add(new OrganizationMemberItem(20, 92, 17, 7, 0, 0, 0, restrictPrincipalAmount: false, -1, -1, -1, 0, 0, -1, -1, 0, 0, monasticTitleSuffixes21, 0, 0, 0, 0, 0, favoriteClothingIds21, hatedClothingIds21, spouseAnonymousTitles21, canStroll: false, -1, initialAges21, equipment21, clothing21, inventory21, combatSkills14, extraCombatSkillGrids14, resourcesAdjust14, 0, 0, 0, 100, 42300, lifeSkillsAdjust14, 8, combatSkillsAdjust14, mainAttributesAdjust14, identityInteractConfig, 3, new ResourceInts(default(int), default(int), default(int), default(int), default(int), default(int), default(int), default(int)), new IntPair[0], null));
		List<OrganizationMemberItem> dataArray22 = _dataArray;
		int[] monasticTitleSuffixes22 = new int[2] { 98, 99 };
		list = new List<short>();
		List<short> favoriteClothingIds22 = list;
		list = new List<short>();
		List<short> hatedClothingIds22 = list;
		int[] spouseAnonymousTitles22 = new int[2] { 100, 101 };
		short[] initialAges22 = new short[4] { -1, -1, -1, -1 };
		PresetEquipmentItemWithProb[] equipment22 = new PresetEquipmentItemWithProb[8]
		{
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Accessory", -1, 0),
			new PresetEquipmentItemWithProb("Accessory", -1, 0),
			new PresetEquipmentItemWithProb("Accessory", -1, 0),
			new PresetEquipmentItemWithProb("Carrier", -1, 0)
		};
		PresetEquipmentItem clothing22 = new PresetEquipmentItem("Clothing", -1);
		list2 = new List<PresetInventoryItem>();
		List<PresetInventoryItem> inventory22 = list2;
		list3 = new List<PresetOrgMemberCombatSkill>();
		List<PresetOrgMemberCombatSkill> combatSkills15 = list3;
		sbyte[] extraCombatSkillGrids15 = new sbyte[5] { 6, 6, 6, 6, 6 };
		short[] resourcesAdjust15 = new short[8];
		short[] lifeSkillsAdjust15 = new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		};
		short[] combatSkillsAdjust15 = new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		};
		short[] mainAttributesAdjust15 = new short[6] { -1, -1, -1, -1, -1, -1 };
		identityInteractConfig = new List<sbyte>();
		dataArray22.Add(new OrganizationMemberItem(21, 97, 17, 6, 0, 0, 0, restrictPrincipalAmount: false, -1, -1, -1, 0, 0, -1, -1, 0, 0, monasticTitleSuffixes22, 0, 0, 0, 0, 0, favoriteClothingIds22, hatedClothingIds22, spouseAnonymousTitles22, canStroll: false, -1, initialAges22, equipment22, clothing22, inventory22, combatSkills15, extraCombatSkillGrids15, resourcesAdjust15, 0, 0, 0, 100, 27600, lifeSkillsAdjust15, 7, combatSkillsAdjust15, mainAttributesAdjust15, identityInteractConfig, 3, new ResourceInts(default(int), default(int), default(int), default(int), default(int), default(int), default(int), default(int)), new IntPair[0], null));
		List<OrganizationMemberItem> dataArray23 = _dataArray;
		int[] monasticTitleSuffixes23 = new int[2] { 103, 104 };
		list = new List<short>();
		List<short> favoriteClothingIds23 = list;
		list = new List<short>();
		List<short> hatedClothingIds23 = list;
		int[] spouseAnonymousTitles23 = new int[2] { 105, 106 };
		short[] initialAges23 = new short[4] { -1, -1, -1, -1 };
		PresetEquipmentItemWithProb[] equipment23 = new PresetEquipmentItemWithProb[8]
		{
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Accessory", -1, 0),
			new PresetEquipmentItemWithProb("Accessory", -1, 0),
			new PresetEquipmentItemWithProb("Accessory", -1, 0),
			new PresetEquipmentItemWithProb("Carrier", -1, 0)
		};
		PresetEquipmentItem clothing23 = new PresetEquipmentItem("Clothing", -1);
		list2 = new List<PresetInventoryItem>();
		List<PresetInventoryItem> inventory23 = list2;
		list3 = new List<PresetOrgMemberCombatSkill>();
		List<PresetOrgMemberCombatSkill> combatSkills16 = list3;
		sbyte[] extraCombatSkillGrids16 = new sbyte[5] { 4, 4, 4, 4, 4 };
		short[] resourcesAdjust16 = new short[8];
		short[] lifeSkillsAdjust16 = new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		};
		short[] combatSkillsAdjust16 = new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		};
		short[] mainAttributesAdjust16 = new short[6] { -1, -1, -1, -1, -1, -1 };
		identityInteractConfig = new List<sbyte>();
		dataArray23.Add(new OrganizationMemberItem(22, 102, 17, 5, 0, 0, 0, restrictPrincipalAmount: false, -1, -1, -1, 0, 0, -1, -1, 0, 0, monasticTitleSuffixes23, 0, 0, 0, 0, 0, favoriteClothingIds23, hatedClothingIds23, spouseAnonymousTitles23, canStroll: false, -1, initialAges23, equipment23, clothing23, inventory23, combatSkills16, extraCombatSkillGrids16, resourcesAdjust16, 0, 0, 0, 100, 16800, lifeSkillsAdjust16, 7, combatSkillsAdjust16, mainAttributesAdjust16, identityInteractConfig, 3, new ResourceInts(default(int), default(int), default(int), default(int), default(int), default(int), default(int), default(int)), new IntPair[0], null));
		List<OrganizationMemberItem> dataArray24 = _dataArray;
		int[] monasticTitleSuffixes24 = new int[2] { 108, 109 };
		list = new List<short>();
		List<short> favoriteClothingIds24 = list;
		list = new List<short>();
		List<short> hatedClothingIds24 = list;
		int[] spouseAnonymousTitles24 = new int[2] { 110, 111 };
		short[] initialAges24 = new short[4] { -1, -1, -1, -1 };
		PresetEquipmentItemWithProb[] equipment24 = new PresetEquipmentItemWithProb[8]
		{
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Accessory", -1, 0),
			new PresetEquipmentItemWithProb("Accessory", -1, 0),
			new PresetEquipmentItemWithProb("Accessory", -1, 0),
			new PresetEquipmentItemWithProb("Carrier", -1, 0)
		};
		PresetEquipmentItem clothing24 = new PresetEquipmentItem("Clothing", -1);
		list2 = new List<PresetInventoryItem>();
		List<PresetInventoryItem> inventory24 = list2;
		list3 = new List<PresetOrgMemberCombatSkill>();
		List<PresetOrgMemberCombatSkill> combatSkills17 = list3;
		sbyte[] extraCombatSkillGrids17 = new sbyte[5] { 2, 2, 2, 2, 2 };
		short[] resourcesAdjust17 = new short[8];
		short[] lifeSkillsAdjust17 = new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		};
		short[] combatSkillsAdjust17 = new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		};
		short[] mainAttributesAdjust17 = new short[6] { -1, -1, -1, -1, -1, -1 };
		identityInteractConfig = new List<sbyte>();
		dataArray24.Add(new OrganizationMemberItem(23, 107, 17, 4, 0, 0, 0, restrictPrincipalAmount: false, -1, -1, -1, 0, 0, -1, -1, 0, 0, monasticTitleSuffixes24, 0, 0, 0, 0, 0, favoriteClothingIds24, hatedClothingIds24, spouseAnonymousTitles24, canStroll: false, -1, initialAges24, equipment24, clothing24, inventory24, combatSkills17, extraCombatSkillGrids17, resourcesAdjust17, 0, 0, 0, 100, 9300, lifeSkillsAdjust17, 6, combatSkillsAdjust17, mainAttributesAdjust17, identityInteractConfig, 3, new ResourceInts(default(int), default(int), default(int), default(int), default(int), default(int), default(int), default(int)), new IntPair[0], null));
		List<OrganizationMemberItem> dataArray25 = _dataArray;
		int[] monasticTitleSuffixes25 = new int[2] { 113, 114 };
		list = new List<short>();
		List<short> favoriteClothingIds25 = list;
		list = new List<short>();
		List<short> hatedClothingIds25 = list;
		int[] spouseAnonymousTitles25 = new int[2] { 115, 116 };
		short[] initialAges25 = new short[4] { -1, -1, -1, -1 };
		PresetEquipmentItemWithProb[] equipment25 = new PresetEquipmentItemWithProb[8]
		{
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Accessory", -1, 0),
			new PresetEquipmentItemWithProb("Accessory", -1, 0),
			new PresetEquipmentItemWithProb("Accessory", -1, 0),
			new PresetEquipmentItemWithProb("Carrier", -1, 0)
		};
		PresetEquipmentItem clothing25 = new PresetEquipmentItem("Clothing", -1);
		list2 = new List<PresetInventoryItem>();
		List<PresetInventoryItem> inventory25 = list2;
		list3 = new List<PresetOrgMemberCombatSkill>();
		List<PresetOrgMemberCombatSkill> combatSkills18 = list3;
		sbyte[] extraCombatSkillGrids18 = new sbyte[5] { 2, 2, 2, 2, 2 };
		short[] resourcesAdjust18 = new short[8];
		short[] lifeSkillsAdjust18 = new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		};
		short[] combatSkillsAdjust18 = new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		};
		short[] mainAttributesAdjust18 = new short[6] { -1, -1, -1, -1, -1, -1 };
		identityInteractConfig = new List<sbyte>();
		dataArray25.Add(new OrganizationMemberItem(24, 112, 17, 3, 0, 0, 0, restrictPrincipalAmount: false, -1, -1, -1, 0, 0, -1, -1, 0, 0, monasticTitleSuffixes25, 0, 0, 0, 0, 0, favoriteClothingIds25, hatedClothingIds25, spouseAnonymousTitles25, canStroll: false, -1, initialAges25, equipment25, clothing25, inventory25, combatSkills18, extraCombatSkillGrids18, resourcesAdjust18, 0, 0, 0, 100, 4500, lifeSkillsAdjust18, 5, combatSkillsAdjust18, mainAttributesAdjust18, identityInteractConfig, 3, new ResourceInts(default(int), default(int), default(int), default(int), default(int), default(int), default(int), default(int)), new IntPair[0], null));
		List<OrganizationMemberItem> dataArray26 = _dataArray;
		int[] monasticTitleSuffixes26 = new int[2] { 118, 119 };
		list = new List<short>();
		List<short> favoriteClothingIds26 = list;
		list = new List<short>();
		List<short> hatedClothingIds26 = list;
		int[] spouseAnonymousTitles26 = new int[2] { 120, 121 };
		short[] initialAges26 = new short[4] { -1, -1, -1, -1 };
		PresetEquipmentItemWithProb[] equipment26 = new PresetEquipmentItemWithProb[8]
		{
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Accessory", -1, 0),
			new PresetEquipmentItemWithProb("Accessory", -1, 0),
			new PresetEquipmentItemWithProb("Accessory", -1, 0),
			new PresetEquipmentItemWithProb("Carrier", -1, 0)
		};
		PresetEquipmentItem clothing26 = new PresetEquipmentItem("Clothing", -1);
		list2 = new List<PresetInventoryItem>();
		List<PresetInventoryItem> inventory26 = list2;
		list3 = new List<PresetOrgMemberCombatSkill>();
		List<PresetOrgMemberCombatSkill> combatSkills19 = list3;
		sbyte[] extraCombatSkillGrids19 = new sbyte[5];
		short[] resourcesAdjust19 = new short[8];
		short[] lifeSkillsAdjust19 = new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		};
		short[] combatSkillsAdjust19 = new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		};
		short[] mainAttributesAdjust19 = new short[6] { -1, -1, -1, -1, -1, -1 };
		identityInteractConfig = new List<sbyte>();
		dataArray26.Add(new OrganizationMemberItem(25, 117, 17, 2, 0, 0, 0, restrictPrincipalAmount: false, -1, -1, -1, 0, 0, -1, -1, 0, 0, monasticTitleSuffixes26, 0, 0, 0, 0, 0, favoriteClothingIds26, hatedClothingIds26, spouseAnonymousTitles26, canStroll: false, -1, initialAges26, equipment26, clothing26, inventory26, combatSkills19, extraCombatSkillGrids19, resourcesAdjust19, 0, 0, 0, 100, 1800, lifeSkillsAdjust19, 4, combatSkillsAdjust19, mainAttributesAdjust19, identityInteractConfig, 3, new ResourceInts(default(int), default(int), default(int), default(int), default(int), default(int), default(int), default(int)), new IntPair[0], null));
		List<OrganizationMemberItem> dataArray27 = _dataArray;
		int[] monasticTitleSuffixes27 = new int[2] { 123, 124 };
		list = new List<short>();
		List<short> favoriteClothingIds27 = list;
		list = new List<short>();
		List<short> hatedClothingIds27 = list;
		int[] spouseAnonymousTitles27 = new int[2] { 125, 126 };
		short[] initialAges27 = new short[4] { -1, -1, -1, -1 };
		PresetEquipmentItemWithProb[] equipment27 = new PresetEquipmentItemWithProb[8]
		{
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Accessory", -1, 0),
			new PresetEquipmentItemWithProb("Accessory", -1, 0),
			new PresetEquipmentItemWithProb("Accessory", -1, 0),
			new PresetEquipmentItemWithProb("Carrier", -1, 0)
		};
		PresetEquipmentItem clothing27 = new PresetEquipmentItem("Clothing", -1);
		list2 = new List<PresetInventoryItem>();
		List<PresetInventoryItem> inventory27 = list2;
		list3 = new List<PresetOrgMemberCombatSkill>();
		List<PresetOrgMemberCombatSkill> combatSkills20 = list3;
		sbyte[] extraCombatSkillGrids20 = new sbyte[5];
		short[] resourcesAdjust20 = new short[8];
		short[] lifeSkillsAdjust20 = new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		};
		short[] combatSkillsAdjust20 = new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		};
		short[] mainAttributesAdjust20 = new short[6] { -1, -1, -1, -1, -1, -1 };
		identityInteractConfig = new List<sbyte>();
		dataArray27.Add(new OrganizationMemberItem(26, 122, 17, 1, 0, 0, 0, restrictPrincipalAmount: false, -1, -1, -1, 0, 0, -1, -1, 0, 0, monasticTitleSuffixes27, 0, 0, 0, 0, 0, favoriteClothingIds27, hatedClothingIds27, spouseAnonymousTitles27, canStroll: false, -1, initialAges27, equipment27, clothing27, inventory27, combatSkills20, extraCombatSkillGrids20, resourcesAdjust20, 0, 0, 0, 100, 600, lifeSkillsAdjust20, 3, combatSkillsAdjust20, mainAttributesAdjust20, identityInteractConfig, 3, new ResourceInts(default(int), default(int), default(int), default(int), default(int), default(int), default(int), default(int)), new IntPair[0], null));
		List<OrganizationMemberItem> dataArray28 = _dataArray;
		int[] monasticTitleSuffixes28 = new int[2] { 128, 129 };
		list = new List<short>();
		List<short> favoriteClothingIds28 = list;
		list = new List<short>();
		List<short> hatedClothingIds28 = list;
		int[] spouseAnonymousTitles28 = new int[2] { 130, 131 };
		short[] initialAges28 = new short[4] { -1, -1, -1, -1 };
		PresetEquipmentItemWithProb[] equipment28 = new PresetEquipmentItemWithProb[8]
		{
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Accessory", -1, 0),
			new PresetEquipmentItemWithProb("Accessory", -1, 0),
			new PresetEquipmentItemWithProb("Accessory", -1, 0),
			new PresetEquipmentItemWithProb("Carrier", -1, 0)
		};
		PresetEquipmentItem clothing28 = new PresetEquipmentItem("Clothing", -1);
		list2 = new List<PresetInventoryItem>();
		List<PresetInventoryItem> inventory28 = list2;
		list3 = new List<PresetOrgMemberCombatSkill>();
		List<PresetOrgMemberCombatSkill> combatSkills21 = list3;
		sbyte[] extraCombatSkillGrids21 = new sbyte[5];
		short[] resourcesAdjust21 = new short[8];
		short[] lifeSkillsAdjust21 = new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		};
		short[] combatSkillsAdjust21 = new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		};
		short[] mainAttributesAdjust21 = new short[6] { -1, -1, -1, -1, -1, -1 };
		identityInteractConfig = new List<sbyte>();
		dataArray28.Add(new OrganizationMemberItem(27, 127, 17, 0, 0, 0, 0, restrictPrincipalAmount: false, -1, -1, -1, 0, 0, -1, -1, 0, 0, monasticTitleSuffixes28, 0, 0, 0, 0, 0, favoriteClothingIds28, hatedClothingIds28, spouseAnonymousTitles28, canStroll: false, -1, initialAges28, equipment28, clothing28, inventory28, combatSkills21, extraCombatSkillGrids21, resourcesAdjust21, 0, 0, 0, 100, 300, lifeSkillsAdjust21, 2, combatSkillsAdjust21, mainAttributesAdjust21, identityInteractConfig, 3, new ResourceInts(default(int), default(int), default(int), default(int), default(int), default(int), default(int), default(int)), new IntPair[0], null));
		List<OrganizationMemberItem> dataArray29 = _dataArray;
		int[] monasticTitleSuffixes29 = new int[2] { 133, 134 };
		list = new List<short>();
		List<short> favoriteClothingIds29 = list;
		list = new List<short>();
		List<short> hatedClothingIds29 = list;
		int[] spouseAnonymousTitles29 = new int[2] { 135, 136 };
		short[] initialAges29 = new short[4] { -1, -1, -1, -1 };
		PresetEquipmentItemWithProb[] equipment29 = new PresetEquipmentItemWithProb[8]
		{
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Accessory", -1, 0),
			new PresetEquipmentItemWithProb("Accessory", -1, 0),
			new PresetEquipmentItemWithProb("Accessory", -1, 0),
			new PresetEquipmentItemWithProb("Carrier", -1, 0)
		};
		PresetEquipmentItem clothing29 = new PresetEquipmentItem("Clothing", -1);
		list2 = new List<PresetInventoryItem>();
		List<PresetInventoryItem> inventory29 = list2;
		list3 = new List<PresetOrgMemberCombatSkill>();
		List<PresetOrgMemberCombatSkill> combatSkills22 = list3;
		sbyte[] extraCombatSkillGrids22 = new sbyte[5] { 10, 10, 10, 10, 10 };
		short[] resourcesAdjust22 = new short[8];
		short[] lifeSkillsAdjust22 = new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		};
		short[] combatSkillsAdjust22 = new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		};
		short[] mainAttributesAdjust22 = new short[6] { -1, -1, -1, -1, -1, -1 };
		identityInteractConfig = new List<sbyte>();
		dataArray29.Add(new OrganizationMemberItem(28, 132, 18, 8, 0, 0, 0, restrictPrincipalAmount: false, -1, -1, -1, 0, 0, -1, -1, 0, 0, monasticTitleSuffixes29, 0, 0, 0, 0, 0, favoriteClothingIds29, hatedClothingIds29, spouseAnonymousTitles29, canStroll: false, -1, initialAges29, equipment29, clothing29, inventory29, combatSkills22, extraCombatSkillGrids22, resourcesAdjust22, 0, 0, 0, 100, 61500, lifeSkillsAdjust22, 8, combatSkillsAdjust22, mainAttributesAdjust22, identityInteractConfig, 3, new ResourceInts(default(int), default(int), default(int), default(int), default(int), default(int), default(int), default(int)), new IntPair[0], null));
		List<OrganizationMemberItem> dataArray30 = _dataArray;
		int[] monasticTitleSuffixes30 = new int[2] { 138, 139 };
		list = new List<short>();
		List<short> favoriteClothingIds30 = list;
		list = new List<short>();
		List<short> hatedClothingIds30 = list;
		int[] spouseAnonymousTitles30 = new int[2] { 140, 141 };
		short[] initialAges30 = new short[4] { -1, -1, -1, -1 };
		PresetEquipmentItemWithProb[] equipment30 = new PresetEquipmentItemWithProb[8]
		{
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Accessory", -1, 0),
			new PresetEquipmentItemWithProb("Accessory", -1, 0),
			new PresetEquipmentItemWithProb("Accessory", -1, 0),
			new PresetEquipmentItemWithProb("Carrier", -1, 0)
		};
		PresetEquipmentItem clothing30 = new PresetEquipmentItem("Clothing", -1);
		list2 = new List<PresetInventoryItem>();
		List<PresetInventoryItem> inventory30 = list2;
		list3 = new List<PresetOrgMemberCombatSkill>();
		List<PresetOrgMemberCombatSkill> combatSkills23 = list3;
		sbyte[] extraCombatSkillGrids23 = new sbyte[5] { 8, 8, 8, 8, 8 };
		short[] resourcesAdjust23 = new short[8];
		short[] lifeSkillsAdjust23 = new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		};
		short[] combatSkillsAdjust23 = new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		};
		short[] mainAttributesAdjust23 = new short[6] { -1, -1, -1, -1, -1, -1 };
		identityInteractConfig = new List<sbyte>();
		dataArray30.Add(new OrganizationMemberItem(29, 137, 18, 7, 0, 0, 0, restrictPrincipalAmount: false, -1, -1, -1, 0, 0, -1, -1, 0, 0, monasticTitleSuffixes30, 0, 0, 0, 0, 0, favoriteClothingIds30, hatedClothingIds30, spouseAnonymousTitles30, canStroll: false, -1, initialAges30, equipment30, clothing30, inventory30, combatSkills23, extraCombatSkillGrids23, resourcesAdjust23, 0, 0, 0, 100, 42300, lifeSkillsAdjust23, 8, combatSkillsAdjust23, mainAttributesAdjust23, identityInteractConfig, 3, new ResourceInts(default(int), default(int), default(int), default(int), default(int), default(int), default(int), default(int)), new IntPair[0], null));
		List<OrganizationMemberItem> dataArray31 = _dataArray;
		int[] monasticTitleSuffixes31 = new int[2] { 143, 144 };
		list = new List<short>();
		List<short> favoriteClothingIds31 = list;
		list = new List<short>();
		List<short> hatedClothingIds31 = list;
		int[] spouseAnonymousTitles31 = new int[2] { 145, 146 };
		short[] initialAges31 = new short[4] { -1, -1, -1, -1 };
		PresetEquipmentItemWithProb[] equipment31 = new PresetEquipmentItemWithProb[8]
		{
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Accessory", -1, 0),
			new PresetEquipmentItemWithProb("Accessory", -1, 0),
			new PresetEquipmentItemWithProb("Accessory", -1, 0),
			new PresetEquipmentItemWithProb("Carrier", -1, 0)
		};
		PresetEquipmentItem clothing31 = new PresetEquipmentItem("Clothing", -1);
		list2 = new List<PresetInventoryItem>();
		List<PresetInventoryItem> inventory31 = list2;
		list3 = new List<PresetOrgMemberCombatSkill>();
		List<PresetOrgMemberCombatSkill> combatSkills24 = list3;
		sbyte[] extraCombatSkillGrids24 = new sbyte[5] { 8, 8, 8, 8, 8 };
		short[] resourcesAdjust24 = new short[8];
		short[] lifeSkillsAdjust24 = new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		};
		short[] combatSkillsAdjust24 = new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		};
		short[] mainAttributesAdjust24 = new short[6] { -1, -1, -1, -1, -1, -1 };
		identityInteractConfig = new List<sbyte>();
		dataArray31.Add(new OrganizationMemberItem(30, 142, 18, 6, 0, 0, 0, restrictPrincipalAmount: false, -1, -1, -1, 0, 0, -1, -1, 0, 0, monasticTitleSuffixes31, 0, 0, 0, 0, 0, favoriteClothingIds31, hatedClothingIds31, spouseAnonymousTitles31, canStroll: false, -1, initialAges31, equipment31, clothing31, inventory31, combatSkills24, extraCombatSkillGrids24, resourcesAdjust24, 0, 0, 0, 100, 27600, lifeSkillsAdjust24, 7, combatSkillsAdjust24, mainAttributesAdjust24, identityInteractConfig, 3, new ResourceInts(default(int), default(int), default(int), default(int), default(int), default(int), default(int), default(int)), new IntPair[0], null));
		List<OrganizationMemberItem> dataArray32 = _dataArray;
		int[] monasticTitleSuffixes32 = new int[2] { 148, 149 };
		list = new List<short>();
		List<short> favoriteClothingIds32 = list;
		list = new List<short>();
		List<short> hatedClothingIds32 = list;
		int[] spouseAnonymousTitles32 = new int[2] { 150, 151 };
		short[] initialAges32 = new short[4] { -1, -1, -1, -1 };
		PresetEquipmentItemWithProb[] equipment32 = new PresetEquipmentItemWithProb[8]
		{
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Accessory", -1, 0),
			new PresetEquipmentItemWithProb("Accessory", -1, 0),
			new PresetEquipmentItemWithProb("Accessory", -1, 0),
			new PresetEquipmentItemWithProb("Carrier", -1, 0)
		};
		PresetEquipmentItem clothing32 = new PresetEquipmentItem("Clothing", -1);
		list2 = new List<PresetInventoryItem>();
		List<PresetInventoryItem> inventory32 = list2;
		list3 = new List<PresetOrgMemberCombatSkill>();
		List<PresetOrgMemberCombatSkill> combatSkills25 = list3;
		sbyte[] extraCombatSkillGrids25 = new sbyte[5] { 6, 6, 6, 6, 6 };
		short[] resourcesAdjust25 = new short[8];
		short[] lifeSkillsAdjust25 = new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		};
		short[] combatSkillsAdjust25 = new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		};
		short[] mainAttributesAdjust25 = new short[6] { -1, -1, -1, -1, -1, -1 };
		identityInteractConfig = new List<sbyte>();
		dataArray32.Add(new OrganizationMemberItem(31, 147, 18, 5, 0, 0, 0, restrictPrincipalAmount: false, -1, -1, -1, 0, 0, -1, -1, 0, 0, monasticTitleSuffixes32, 0, 0, 0, 0, 0, favoriteClothingIds32, hatedClothingIds32, spouseAnonymousTitles32, canStroll: false, -1, initialAges32, equipment32, clothing32, inventory32, combatSkills25, extraCombatSkillGrids25, resourcesAdjust25, 0, 0, 0, 100, 16800, lifeSkillsAdjust25, 7, combatSkillsAdjust25, mainAttributesAdjust25, identityInteractConfig, 3, new ResourceInts(default(int), default(int), default(int), default(int), default(int), default(int), default(int), default(int)), new IntPair[0], null));
		List<OrganizationMemberItem> dataArray33 = _dataArray;
		int[] monasticTitleSuffixes33 = new int[2] { 153, 154 };
		list = new List<short>();
		List<short> favoriteClothingIds33 = list;
		list = new List<short>();
		List<short> hatedClothingIds33 = list;
		int[] spouseAnonymousTitles33 = new int[2] { 155, 156 };
		short[] initialAges33 = new short[4] { -1, -1, -1, -1 };
		PresetEquipmentItemWithProb[] equipment33 = new PresetEquipmentItemWithProb[8]
		{
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Accessory", -1, 0),
			new PresetEquipmentItemWithProb("Accessory", -1, 0),
			new PresetEquipmentItemWithProb("Accessory", -1, 0),
			new PresetEquipmentItemWithProb("Carrier", -1, 0)
		};
		PresetEquipmentItem clothing33 = new PresetEquipmentItem("Clothing", -1);
		list2 = new List<PresetInventoryItem>();
		List<PresetInventoryItem> inventory33 = list2;
		list3 = new List<PresetOrgMemberCombatSkill>();
		List<PresetOrgMemberCombatSkill> combatSkills26 = list3;
		sbyte[] extraCombatSkillGrids26 = new sbyte[5] { 4, 4, 4, 4, 4 };
		short[] resourcesAdjust26 = new short[8];
		short[] lifeSkillsAdjust26 = new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		};
		short[] combatSkillsAdjust26 = new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		};
		short[] mainAttributesAdjust26 = new short[6] { -1, -1, -1, -1, -1, -1 };
		identityInteractConfig = new List<sbyte>();
		dataArray33.Add(new OrganizationMemberItem(32, 152, 18, 4, 0, 0, 0, restrictPrincipalAmount: false, -1, -1, -1, 0, 0, -1, -1, 0, 0, monasticTitleSuffixes33, 0, 0, 0, 0, 0, favoriteClothingIds33, hatedClothingIds33, spouseAnonymousTitles33, canStroll: false, -1, initialAges33, equipment33, clothing33, inventory33, combatSkills26, extraCombatSkillGrids26, resourcesAdjust26, 0, 0, 0, 100, 9300, lifeSkillsAdjust26, 6, combatSkillsAdjust26, mainAttributesAdjust26, identityInteractConfig, 3, new ResourceInts(default(int), default(int), default(int), default(int), default(int), default(int), default(int), default(int)), new IntPair[0], null));
		List<OrganizationMemberItem> dataArray34 = _dataArray;
		int[] monasticTitleSuffixes34 = new int[2] { 158, 159 };
		list = new List<short>();
		List<short> favoriteClothingIds34 = list;
		list = new List<short>();
		List<short> hatedClothingIds34 = list;
		int[] spouseAnonymousTitles34 = new int[2] { 160, 161 };
		short[] initialAges34 = new short[4] { -1, -1, -1, -1 };
		PresetEquipmentItemWithProb[] equipment34 = new PresetEquipmentItemWithProb[8]
		{
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Accessory", -1, 0),
			new PresetEquipmentItemWithProb("Accessory", -1, 0),
			new PresetEquipmentItemWithProb("Accessory", -1, 0),
			new PresetEquipmentItemWithProb("Carrier", -1, 0)
		};
		PresetEquipmentItem clothing34 = new PresetEquipmentItem("Clothing", -1);
		list2 = new List<PresetInventoryItem>();
		List<PresetInventoryItem> inventory34 = list2;
		list3 = new List<PresetOrgMemberCombatSkill>();
		List<PresetOrgMemberCombatSkill> combatSkills27 = list3;
		sbyte[] extraCombatSkillGrids27 = new sbyte[5] { 4, 4, 4, 4, 4 };
		short[] resourcesAdjust27 = new short[8];
		short[] lifeSkillsAdjust27 = new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		};
		short[] combatSkillsAdjust27 = new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		};
		short[] mainAttributesAdjust27 = new short[6] { -1, -1, -1, -1, -1, -1 };
		identityInteractConfig = new List<sbyte>();
		dataArray34.Add(new OrganizationMemberItem(33, 157, 18, 3, 0, 0, 0, restrictPrincipalAmount: false, -1, -1, -1, 0, 0, -1, -1, 0, 0, monasticTitleSuffixes34, 0, 0, 0, 0, 0, favoriteClothingIds34, hatedClothingIds34, spouseAnonymousTitles34, canStroll: false, -1, initialAges34, equipment34, clothing34, inventory34, combatSkills27, extraCombatSkillGrids27, resourcesAdjust27, 0, 0, 0, 100, 4500, lifeSkillsAdjust27, 5, combatSkillsAdjust27, mainAttributesAdjust27, identityInteractConfig, 3, new ResourceInts(default(int), default(int), default(int), default(int), default(int), default(int), default(int), default(int)), new IntPair[0], null));
		List<OrganizationMemberItem> dataArray35 = _dataArray;
		int[] monasticTitleSuffixes35 = new int[2] { 163, 164 };
		list = new List<short>();
		List<short> favoriteClothingIds35 = list;
		list = new List<short>();
		List<short> hatedClothingIds35 = list;
		int[] spouseAnonymousTitles35 = new int[2] { 165, 166 };
		short[] initialAges35 = new short[4] { -1, -1, -1, -1 };
		PresetEquipmentItemWithProb[] equipment35 = new PresetEquipmentItemWithProb[8]
		{
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Accessory", -1, 0),
			new PresetEquipmentItemWithProb("Accessory", -1, 0),
			new PresetEquipmentItemWithProb("Accessory", -1, 0),
			new PresetEquipmentItemWithProb("Carrier", -1, 0)
		};
		PresetEquipmentItem clothing35 = new PresetEquipmentItem("Clothing", -1);
		list2 = new List<PresetInventoryItem>();
		List<PresetInventoryItem> inventory35 = list2;
		list3 = new List<PresetOrgMemberCombatSkill>();
		List<PresetOrgMemberCombatSkill> combatSkills28 = list3;
		sbyte[] extraCombatSkillGrids28 = new sbyte[5] { 2, 2, 2, 2, 2 };
		short[] resourcesAdjust28 = new short[8];
		short[] lifeSkillsAdjust28 = new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		};
		short[] combatSkillsAdjust28 = new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		};
		short[] mainAttributesAdjust28 = new short[6] { -1, -1, -1, -1, -1, -1 };
		identityInteractConfig = new List<sbyte>();
		dataArray35.Add(new OrganizationMemberItem(34, 162, 18, 2, 0, 0, 0, restrictPrincipalAmount: false, -1, -1, -1, 0, 0, -1, -1, 0, 0, monasticTitleSuffixes35, 0, 0, 0, 0, 0, favoriteClothingIds35, hatedClothingIds35, spouseAnonymousTitles35, canStroll: false, -1, initialAges35, equipment35, clothing35, inventory35, combatSkills28, extraCombatSkillGrids28, resourcesAdjust28, 0, 0, 0, 100, 1800, lifeSkillsAdjust28, 4, combatSkillsAdjust28, mainAttributesAdjust28, identityInteractConfig, 3, new ResourceInts(default(int), default(int), default(int), default(int), default(int), default(int), default(int), default(int)), new IntPair[0], null));
		List<OrganizationMemberItem> dataArray36 = _dataArray;
		int[] monasticTitleSuffixes36 = new int[2] { 168, 169 };
		list = new List<short>();
		List<short> favoriteClothingIds36 = list;
		list = new List<short>();
		List<short> hatedClothingIds36 = list;
		int[] spouseAnonymousTitles36 = new int[2] { 170, 171 };
		short[] initialAges36 = new short[4] { -1, -1, -1, -1 };
		PresetEquipmentItemWithProb[] equipment36 = new PresetEquipmentItemWithProb[8]
		{
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Accessory", -1, 0),
			new PresetEquipmentItemWithProb("Accessory", -1, 0),
			new PresetEquipmentItemWithProb("Accessory", -1, 0),
			new PresetEquipmentItemWithProb("Carrier", -1, 0)
		};
		PresetEquipmentItem clothing36 = new PresetEquipmentItem("Clothing", -1);
		list2 = new List<PresetInventoryItem>();
		List<PresetInventoryItem> inventory36 = list2;
		list3 = new List<PresetOrgMemberCombatSkill>();
		List<PresetOrgMemberCombatSkill> combatSkills29 = list3;
		sbyte[] extraCombatSkillGrids29 = new sbyte[5];
		short[] resourcesAdjust29 = new short[8];
		short[] lifeSkillsAdjust29 = new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		};
		short[] combatSkillsAdjust29 = new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		};
		short[] mainAttributesAdjust29 = new short[6] { -1, -1, -1, -1, -1, -1 };
		identityInteractConfig = new List<sbyte>();
		dataArray36.Add(new OrganizationMemberItem(35, 167, 18, 1, 0, 0, 0, restrictPrincipalAmount: false, -1, -1, -1, 0, 0, -1, -1, 0, 0, monasticTitleSuffixes36, 0, 0, 0, 0, 0, favoriteClothingIds36, hatedClothingIds36, spouseAnonymousTitles36, canStroll: false, -1, initialAges36, equipment36, clothing36, inventory36, combatSkills29, extraCombatSkillGrids29, resourcesAdjust29, 0, 0, 0, 100, 600, lifeSkillsAdjust29, 3, combatSkillsAdjust29, mainAttributesAdjust29, identityInteractConfig, 3, new ResourceInts(default(int), default(int), default(int), default(int), default(int), default(int), default(int), default(int)), new IntPair[0], null));
		List<OrganizationMemberItem> dataArray37 = _dataArray;
		int[] monasticTitleSuffixes37 = new int[2] { 173, 174 };
		list = new List<short>();
		List<short> favoriteClothingIds37 = list;
		list = new List<short>();
		List<short> hatedClothingIds37 = list;
		int[] spouseAnonymousTitles37 = new int[2] { 175, 176 };
		short[] initialAges37 = new short[4] { -1, -1, -1, -1 };
		PresetEquipmentItemWithProb[] equipment37 = new PresetEquipmentItemWithProb[8]
		{
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Accessory", -1, 0),
			new PresetEquipmentItemWithProb("Accessory", -1, 0),
			new PresetEquipmentItemWithProb("Accessory", -1, 0),
			new PresetEquipmentItemWithProb("Carrier", -1, 0)
		};
		PresetEquipmentItem clothing37 = new PresetEquipmentItem("Clothing", -1);
		list2 = new List<PresetInventoryItem>();
		List<PresetInventoryItem> inventory37 = list2;
		list3 = new List<PresetOrgMemberCombatSkill>();
		List<PresetOrgMemberCombatSkill> combatSkills30 = list3;
		sbyte[] extraCombatSkillGrids30 = new sbyte[5];
		short[] resourcesAdjust30 = new short[8];
		short[] lifeSkillsAdjust30 = new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		};
		short[] combatSkillsAdjust30 = new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		};
		short[] mainAttributesAdjust30 = new short[6] { -1, -1, -1, -1, -1, -1 };
		identityInteractConfig = new List<sbyte>();
		dataArray37.Add(new OrganizationMemberItem(36, 172, 18, 0, 0, 0, 0, restrictPrincipalAmount: false, -1, -1, -1, 0, 0, -1, -1, 0, 0, monasticTitleSuffixes37, 0, 0, 0, 0, 0, favoriteClothingIds37, hatedClothingIds37, spouseAnonymousTitles37, canStroll: false, -1, initialAges37, equipment37, clothing37, inventory37, combatSkills30, extraCombatSkillGrids30, resourcesAdjust30, 0, 0, 0, 100, 300, lifeSkillsAdjust30, 2, combatSkillsAdjust30, mainAttributesAdjust30, identityInteractConfig, 3, new ResourceInts(default(int), default(int), default(int), default(int), default(int), default(int), default(int), default(int)), new IntPair[0], null));
		List<OrganizationMemberItem> dataArray38 = _dataArray;
		int[] monasticTitleSuffixes38 = new int[2] { 178, 179 };
		list = new List<short>();
		List<short> favoriteClothingIds38 = list;
		list = new List<short>();
		List<short> hatedClothingIds38 = list;
		int[] spouseAnonymousTitles38 = new int[2] { 180, 181 };
		short[] initialAges38 = new short[4] { -1, -1, -1, -1 };
		PresetEquipmentItemWithProb[] equipment38 = new PresetEquipmentItemWithProb[8]
		{
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Accessory", -1, 0),
			new PresetEquipmentItemWithProb("Accessory", -1, 0),
			new PresetEquipmentItemWithProb("Accessory", -1, 0),
			new PresetEquipmentItemWithProb("Carrier", -1, 0)
		};
		PresetEquipmentItem clothing38 = new PresetEquipmentItem("Clothing", -1);
		list2 = new List<PresetInventoryItem>();
		List<PresetInventoryItem> inventory38 = list2;
		list3 = new List<PresetOrgMemberCombatSkill>();
		List<PresetOrgMemberCombatSkill> combatSkills31 = list3;
		sbyte[] extraCombatSkillGrids31 = new sbyte[5] { 8, 8, 8, 8, 8 };
		short[] resourcesAdjust31 = new short[8];
		short[] lifeSkillsAdjust31 = new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		};
		short[] combatSkillsAdjust31 = new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		};
		short[] mainAttributesAdjust31 = new short[6] { -1, -1, -1, -1, -1, -1 };
		identityInteractConfig = new List<sbyte>();
		dataArray38.Add(new OrganizationMemberItem(37, 177, 19, 8, 0, 0, 0, restrictPrincipalAmount: false, -1, -1, -1, 0, 0, -1, -1, 0, 0, monasticTitleSuffixes38, 0, 0, 0, 0, 0, favoriteClothingIds38, hatedClothingIds38, spouseAnonymousTitles38, canStroll: false, -1, initialAges38, equipment38, clothing38, inventory38, combatSkills31, extraCombatSkillGrids31, resourcesAdjust31, 0, 0, 0, 100, 61500, lifeSkillsAdjust31, 8, combatSkillsAdjust31, mainAttributesAdjust31, identityInteractConfig, 3, new ResourceInts(default(int), default(int), default(int), default(int), default(int), default(int), default(int), default(int)), new IntPair[0], null));
		List<OrganizationMemberItem> dataArray39 = _dataArray;
		int[] monasticTitleSuffixes39 = new int[2] { 183, 184 };
		list = new List<short>();
		List<short> favoriteClothingIds39 = list;
		list = new List<short>();
		List<short> hatedClothingIds39 = list;
		int[] spouseAnonymousTitles39 = new int[2] { 185, 186 };
		short[] initialAges39 = new short[4] { -1, -1, -1, -1 };
		PresetEquipmentItemWithProb[] equipment39 = new PresetEquipmentItemWithProb[8]
		{
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Accessory", -1, 0),
			new PresetEquipmentItemWithProb("Accessory", -1, 0),
			new PresetEquipmentItemWithProb("Accessory", -1, 0),
			new PresetEquipmentItemWithProb("Carrier", -1, 0)
		};
		PresetEquipmentItem clothing39 = new PresetEquipmentItem("Clothing", -1);
		list2 = new List<PresetInventoryItem>();
		List<PresetInventoryItem> inventory39 = list2;
		list3 = new List<PresetOrgMemberCombatSkill>();
		List<PresetOrgMemberCombatSkill> combatSkills32 = list3;
		sbyte[] extraCombatSkillGrids32 = new sbyte[5] { 6, 6, 6, 6, 6 };
		short[] resourcesAdjust32 = new short[8];
		short[] lifeSkillsAdjust32 = new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		};
		short[] combatSkillsAdjust32 = new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		};
		short[] mainAttributesAdjust32 = new short[6] { -1, -1, -1, -1, -1, -1 };
		identityInteractConfig = new List<sbyte>();
		dataArray39.Add(new OrganizationMemberItem(38, 182, 19, 7, 0, 0, 0, restrictPrincipalAmount: false, -1, -1, -1, 0, 0, -1, -1, 0, 0, monasticTitleSuffixes39, 0, 0, 0, 0, 0, favoriteClothingIds39, hatedClothingIds39, spouseAnonymousTitles39, canStroll: false, -1, initialAges39, equipment39, clothing39, inventory39, combatSkills32, extraCombatSkillGrids32, resourcesAdjust32, 0, 0, 0, 100, 42300, lifeSkillsAdjust32, 8, combatSkillsAdjust32, mainAttributesAdjust32, identityInteractConfig, 3, new ResourceInts(default(int), default(int), default(int), default(int), default(int), default(int), default(int), default(int)), new IntPair[0], null));
		List<OrganizationMemberItem> dataArray40 = _dataArray;
		int[] monasticTitleSuffixes40 = new int[2] { 188, 189 };
		list = new List<short>();
		List<short> favoriteClothingIds40 = list;
		list = new List<short>();
		List<short> hatedClothingIds40 = list;
		int[] spouseAnonymousTitles40 = new int[2] { 190, 191 };
		short[] initialAges40 = new short[4] { -1, -1, -1, -1 };
		PresetEquipmentItemWithProb[] equipment40 = new PresetEquipmentItemWithProb[8]
		{
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Accessory", -1, 0),
			new PresetEquipmentItemWithProb("Accessory", -1, 0),
			new PresetEquipmentItemWithProb("Accessory", -1, 0),
			new PresetEquipmentItemWithProb("Carrier", -1, 0)
		};
		PresetEquipmentItem clothing40 = new PresetEquipmentItem("Clothing", -1);
		list2 = new List<PresetInventoryItem>();
		List<PresetInventoryItem> inventory40 = list2;
		list3 = new List<PresetOrgMemberCombatSkill>();
		List<PresetOrgMemberCombatSkill> combatSkills33 = list3;
		sbyte[] extraCombatSkillGrids33 = new sbyte[5] { 6, 6, 6, 6, 6 };
		short[] resourcesAdjust33 = new short[8];
		short[] lifeSkillsAdjust33 = new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		};
		short[] combatSkillsAdjust33 = new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		};
		short[] mainAttributesAdjust33 = new short[6] { -1, -1, -1, -1, -1, -1 };
		identityInteractConfig = new List<sbyte>();
		dataArray40.Add(new OrganizationMemberItem(39, 187, 19, 6, 0, 0, 0, restrictPrincipalAmount: false, -1, -1, -1, 0, 0, -1, -1, 0, 0, monasticTitleSuffixes40, 0, 0, 0, 0, 0, favoriteClothingIds40, hatedClothingIds40, spouseAnonymousTitles40, canStroll: false, -1, initialAges40, equipment40, clothing40, inventory40, combatSkills33, extraCombatSkillGrids33, resourcesAdjust33, 0, 0, 0, 100, 27600, lifeSkillsAdjust33, 7, combatSkillsAdjust33, mainAttributesAdjust33, identityInteractConfig, 3, new ResourceInts(default(int), default(int), default(int), default(int), default(int), default(int), default(int), default(int)), new IntPair[0], null));
		List<OrganizationMemberItem> dataArray41 = _dataArray;
		int[] monasticTitleSuffixes41 = new int[2] { 193, 194 };
		list = new List<short>();
		List<short> favoriteClothingIds41 = list;
		list = new List<short>();
		List<short> hatedClothingIds41 = list;
		int[] spouseAnonymousTitles41 = new int[2] { 195, 196 };
		short[] initialAges41 = new short[4] { -1, -1, -1, -1 };
		PresetEquipmentItemWithProb[] equipment41 = new PresetEquipmentItemWithProb[8]
		{
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Accessory", -1, 0),
			new PresetEquipmentItemWithProb("Accessory", -1, 0),
			new PresetEquipmentItemWithProb("Accessory", -1, 0),
			new PresetEquipmentItemWithProb("Carrier", -1, 0)
		};
		PresetEquipmentItem clothing41 = new PresetEquipmentItem("Clothing", -1);
		list2 = new List<PresetInventoryItem>();
		List<PresetInventoryItem> inventory41 = list2;
		list3 = new List<PresetOrgMemberCombatSkill>();
		List<PresetOrgMemberCombatSkill> combatSkills34 = list3;
		sbyte[] extraCombatSkillGrids34 = new sbyte[5] { 4, 4, 4, 4, 4 };
		short[] resourcesAdjust34 = new short[8];
		short[] lifeSkillsAdjust34 = new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		};
		short[] combatSkillsAdjust34 = new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		};
		short[] mainAttributesAdjust34 = new short[6] { -1, -1, -1, -1, -1, -1 };
		identityInteractConfig = new List<sbyte>();
		dataArray41.Add(new OrganizationMemberItem(40, 192, 19, 5, 0, 0, 0, restrictPrincipalAmount: false, -1, -1, -1, 0, 0, -1, -1, 0, 0, monasticTitleSuffixes41, 0, 0, 0, 0, 0, favoriteClothingIds41, hatedClothingIds41, spouseAnonymousTitles41, canStroll: false, -1, initialAges41, equipment41, clothing41, inventory41, combatSkills34, extraCombatSkillGrids34, resourcesAdjust34, 0, 0, 0, 100, 16800, lifeSkillsAdjust34, 7, combatSkillsAdjust34, mainAttributesAdjust34, identityInteractConfig, 3, new ResourceInts(default(int), default(int), default(int), default(int), default(int), default(int), default(int), default(int)), new IntPair[0], null));
		List<OrganizationMemberItem> dataArray42 = _dataArray;
		int[] monasticTitleSuffixes42 = new int[2] { 198, 199 };
		list = new List<short>();
		List<short> favoriteClothingIds42 = list;
		list = new List<short>();
		List<short> hatedClothingIds42 = list;
		int[] spouseAnonymousTitles42 = new int[2] { 200, 201 };
		short[] initialAges42 = new short[4] { -1, -1, -1, -1 };
		PresetEquipmentItemWithProb[] equipment42 = new PresetEquipmentItemWithProb[8]
		{
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Accessory", -1, 0),
			new PresetEquipmentItemWithProb("Accessory", -1, 0),
			new PresetEquipmentItemWithProb("Accessory", -1, 0),
			new PresetEquipmentItemWithProb("Carrier", -1, 0)
		};
		PresetEquipmentItem clothing42 = new PresetEquipmentItem("Clothing", -1);
		list2 = new List<PresetInventoryItem>();
		List<PresetInventoryItem> inventory42 = list2;
		list3 = new List<PresetOrgMemberCombatSkill>();
		List<PresetOrgMemberCombatSkill> combatSkills35 = list3;
		sbyte[] extraCombatSkillGrids35 = new sbyte[5] { 2, 2, 2, 2, 2 };
		short[] resourcesAdjust35 = new short[8];
		short[] lifeSkillsAdjust35 = new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		};
		short[] combatSkillsAdjust35 = new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		};
		short[] mainAttributesAdjust35 = new short[6] { -1, -1, -1, -1, -1, -1 };
		identityInteractConfig = new List<sbyte>();
		dataArray42.Add(new OrganizationMemberItem(41, 197, 19, 4, 0, 0, 0, restrictPrincipalAmount: false, -1, -1, -1, 0, 0, -1, -1, 0, 0, monasticTitleSuffixes42, 0, 0, 0, 0, 0, favoriteClothingIds42, hatedClothingIds42, spouseAnonymousTitles42, canStroll: false, -1, initialAges42, equipment42, clothing42, inventory42, combatSkills35, extraCombatSkillGrids35, resourcesAdjust35, 0, 0, 0, 100, 9300, lifeSkillsAdjust35, 6, combatSkillsAdjust35, mainAttributesAdjust35, identityInteractConfig, 3, new ResourceInts(default(int), default(int), default(int), default(int), default(int), default(int), default(int), default(int)), new IntPair[0], null));
		List<OrganizationMemberItem> dataArray43 = _dataArray;
		int[] monasticTitleSuffixes43 = new int[2] { 203, 204 };
		list = new List<short>();
		List<short> favoriteClothingIds43 = list;
		list = new List<short>();
		List<short> hatedClothingIds43 = list;
		int[] spouseAnonymousTitles43 = new int[2] { 205, 206 };
		short[] initialAges43 = new short[4] { -1, -1, -1, -1 };
		PresetEquipmentItemWithProb[] equipment43 = new PresetEquipmentItemWithProb[8]
		{
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Accessory", -1, 0),
			new PresetEquipmentItemWithProb("Accessory", -1, 0),
			new PresetEquipmentItemWithProb("Accessory", -1, 0),
			new PresetEquipmentItemWithProb("Carrier", -1, 0)
		};
		PresetEquipmentItem clothing43 = new PresetEquipmentItem("Clothing", -1);
		list2 = new List<PresetInventoryItem>();
		List<PresetInventoryItem> inventory43 = list2;
		list3 = new List<PresetOrgMemberCombatSkill>();
		List<PresetOrgMemberCombatSkill> combatSkills36 = list3;
		sbyte[] extraCombatSkillGrids36 = new sbyte[5] { 2, 2, 2, 2, 2 };
		short[] resourcesAdjust36 = new short[8];
		short[] lifeSkillsAdjust36 = new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		};
		short[] combatSkillsAdjust36 = new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		};
		short[] mainAttributesAdjust36 = new short[6] { -1, -1, -1, -1, -1, -1 };
		identityInteractConfig = new List<sbyte>();
		dataArray43.Add(new OrganizationMemberItem(42, 202, 19, 3, 0, 0, 0, restrictPrincipalAmount: false, -1, -1, -1, 0, 0, -1, -1, 0, 0, monasticTitleSuffixes43, 0, 0, 0, 0, 0, favoriteClothingIds43, hatedClothingIds43, spouseAnonymousTitles43, canStroll: false, -1, initialAges43, equipment43, clothing43, inventory43, combatSkills36, extraCombatSkillGrids36, resourcesAdjust36, 0, 0, 0, 100, 4500, lifeSkillsAdjust36, 5, combatSkillsAdjust36, mainAttributesAdjust36, identityInteractConfig, 3, new ResourceInts(default(int), default(int), default(int), default(int), default(int), default(int), default(int), default(int)), new IntPair[0], null));
		List<OrganizationMemberItem> dataArray44 = _dataArray;
		int[] monasticTitleSuffixes44 = new int[2] { 208, 209 };
		list = new List<short>();
		List<short> favoriteClothingIds44 = list;
		list = new List<short>();
		List<short> hatedClothingIds44 = list;
		int[] spouseAnonymousTitles44 = new int[2] { 210, 211 };
		short[] initialAges44 = new short[4] { -1, -1, -1, -1 };
		PresetEquipmentItemWithProb[] equipment44 = new PresetEquipmentItemWithProb[8]
		{
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Accessory", -1, 0),
			new PresetEquipmentItemWithProb("Accessory", -1, 0),
			new PresetEquipmentItemWithProb("Accessory", -1, 0),
			new PresetEquipmentItemWithProb("Carrier", -1, 0)
		};
		PresetEquipmentItem clothing44 = new PresetEquipmentItem("Clothing", -1);
		list2 = new List<PresetInventoryItem>();
		List<PresetInventoryItem> inventory44 = list2;
		list3 = new List<PresetOrgMemberCombatSkill>();
		List<PresetOrgMemberCombatSkill> combatSkills37 = list3;
		sbyte[] extraCombatSkillGrids37 = new sbyte[5];
		short[] resourcesAdjust37 = new short[8];
		short[] lifeSkillsAdjust37 = new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		};
		short[] combatSkillsAdjust37 = new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		};
		short[] mainAttributesAdjust37 = new short[6] { -1, -1, -1, -1, -1, -1 };
		identityInteractConfig = new List<sbyte>();
		dataArray44.Add(new OrganizationMemberItem(43, 207, 19, 2, 0, 0, 0, restrictPrincipalAmount: false, -1, -1, -1, 0, 0, -1, -1, 0, 0, monasticTitleSuffixes44, 0, 0, 0, 0, 0, favoriteClothingIds44, hatedClothingIds44, spouseAnonymousTitles44, canStroll: false, -1, initialAges44, equipment44, clothing44, inventory44, combatSkills37, extraCombatSkillGrids37, resourcesAdjust37, 0, 0, 0, 100, 1800, lifeSkillsAdjust37, 4, combatSkillsAdjust37, mainAttributesAdjust37, identityInteractConfig, 3, new ResourceInts(default(int), default(int), default(int), default(int), default(int), default(int), default(int), default(int)), new IntPair[0], null));
		List<OrganizationMemberItem> dataArray45 = _dataArray;
		int[] monasticTitleSuffixes45 = new int[2] { 213, 214 };
		list = new List<short>();
		List<short> favoriteClothingIds45 = list;
		list = new List<short>();
		List<short> hatedClothingIds45 = list;
		int[] spouseAnonymousTitles45 = new int[2] { 215, 216 };
		short[] initialAges45 = new short[4] { -1, -1, -1, -1 };
		PresetEquipmentItemWithProb[] equipment45 = new PresetEquipmentItemWithProb[8]
		{
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Accessory", -1, 0),
			new PresetEquipmentItemWithProb("Accessory", -1, 0),
			new PresetEquipmentItemWithProb("Accessory", -1, 0),
			new PresetEquipmentItemWithProb("Carrier", -1, 0)
		};
		PresetEquipmentItem clothing45 = new PresetEquipmentItem("Clothing", -1);
		list2 = new List<PresetInventoryItem>();
		List<PresetInventoryItem> inventory45 = list2;
		list3 = new List<PresetOrgMemberCombatSkill>();
		List<PresetOrgMemberCombatSkill> combatSkills38 = list3;
		sbyte[] extraCombatSkillGrids38 = new sbyte[5];
		short[] resourcesAdjust38 = new short[8];
		short[] lifeSkillsAdjust38 = new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		};
		short[] combatSkillsAdjust38 = new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		};
		short[] mainAttributesAdjust38 = new short[6] { -1, -1, -1, -1, -1, -1 };
		identityInteractConfig = new List<sbyte>();
		dataArray45.Add(new OrganizationMemberItem(44, 212, 19, 1, 0, 0, 0, restrictPrincipalAmount: false, -1, -1, -1, 0, 0, -1, -1, 0, 0, monasticTitleSuffixes45, 0, 0, 0, 0, 0, favoriteClothingIds45, hatedClothingIds45, spouseAnonymousTitles45, canStroll: false, -1, initialAges45, equipment45, clothing45, inventory45, combatSkills38, extraCombatSkillGrids38, resourcesAdjust38, 0, 0, 0, 100, 600, lifeSkillsAdjust38, 3, combatSkillsAdjust38, mainAttributesAdjust38, identityInteractConfig, 3, new ResourceInts(default(int), default(int), default(int), default(int), default(int), default(int), default(int), default(int)), new IntPair[0], null));
		List<OrganizationMemberItem> dataArray46 = _dataArray;
		int[] monasticTitleSuffixes46 = new int[2] { 218, 219 };
		list = new List<short>();
		List<short> favoriteClothingIds46 = list;
		list = new List<short>();
		List<short> hatedClothingIds46 = list;
		int[] spouseAnonymousTitles46 = new int[2] { 220, 221 };
		short[] initialAges46 = new short[4] { -1, -1, -1, -1 };
		PresetEquipmentItemWithProb[] equipment46 = new PresetEquipmentItemWithProb[8]
		{
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Accessory", -1, 0),
			new PresetEquipmentItemWithProb("Accessory", -1, 0),
			new PresetEquipmentItemWithProb("Accessory", -1, 0),
			new PresetEquipmentItemWithProb("Carrier", -1, 0)
		};
		PresetEquipmentItem clothing46 = new PresetEquipmentItem("Clothing", -1);
		list2 = new List<PresetInventoryItem>();
		List<PresetInventoryItem> inventory46 = list2;
		list3 = new List<PresetOrgMemberCombatSkill>();
		List<PresetOrgMemberCombatSkill> combatSkills39 = list3;
		sbyte[] extraCombatSkillGrids39 = new sbyte[5];
		short[] resourcesAdjust39 = new short[8];
		short[] lifeSkillsAdjust39 = new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		};
		short[] combatSkillsAdjust39 = new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		};
		short[] mainAttributesAdjust39 = new short[6] { -1, -1, -1, -1, -1, -1 };
		identityInteractConfig = new List<sbyte>();
		dataArray46.Add(new OrganizationMemberItem(45, 217, 19, 0, 0, 0, 0, restrictPrincipalAmount: false, -1, -1, -1, 0, 0, -1, -1, 0, 0, monasticTitleSuffixes46, 0, 0, 0, 0, 0, favoriteClothingIds46, hatedClothingIds46, spouseAnonymousTitles46, canStroll: false, -1, initialAges46, equipment46, clothing46, inventory46, combatSkills39, extraCombatSkillGrids39, resourcesAdjust39, 0, 0, 0, 100, 300, lifeSkillsAdjust39, 2, combatSkillsAdjust39, mainAttributesAdjust39, identityInteractConfig, 3, new ResourceInts(default(int), default(int), default(int), default(int), default(int), default(int), default(int), default(int)), new IntPair[0], null));
		_dataArray.Add(new OrganizationMemberItem(46, 222, 1, 8, 1, 1, 1, restrictPrincipalAmount: true, 1, -1, -1, -1, 7, -1, 7, 100, 130, new int[2] { 223, 223 }, 15800, 16, 13000, 30750, 0, new List<short> { 18, 19, 20, 6, 12 }, new List<short> { 58, 59, 60, 81, 82 }, new int[2] { 224, 225 }, canStroll: false, 217, new short[4] { 42, 50, 58, 66 }, new PresetEquipmentItemWithProb[8]
		{
			new PresetEquipmentItemWithProb("Armor", 0, 75),
			new PresetEquipmentItemWithProb("Armor", 333, 100),
			new PresetEquipmentItemWithProb("Armor", 396, 75),
			new PresetEquipmentItemWithProb("Armor", 189, 75),
			new PresetEquipmentItemWithProb("Accessory", 180, 75),
			new PresetEquipmentItemWithProb("Accessory", 153, 50),
			new PresetEquipmentItemWithProb("Accessory", 72, 50),
			new PresetEquipmentItemWithProb("Carrier", 0, 75)
		}, new PresetEquipmentItem("Clothing", 20), new List<PresetInventoryItem>
		{
			new PresetInventoryItem("SkillBook", 117, 3, 40),
			new PresetInventoryItem("SkillBook", 9, 1, 40),
			new PresetInventoryItem("Food", 93, 3, 40),
			new PresetInventoryItem("Medicine", 60, 1, 20),
			new PresetInventoryItem("Medicine", 72, 1, 20),
			new PresetInventoryItem("TeaWine", 18, 1, 20),
			new PresetInventoryItem("TeaWine", 27, 1, 20)
		}, new List<PresetOrgMemberCombatSkill>
		{
			new PresetOrgMemberCombatSkill(0, 8),
			new PresetOrgMemberCombatSkill(106, 5),
			new PresetOrgMemberCombatSkill(201, 8),
			new PresetOrgMemberCombatSkill(321, 7),
			new PresetOrgMemberCombatSkill(400, 5),
			new PresetOrgMemberCombatSkill(623, 8)
		}, new sbyte[5] { 10, 10, 10, 10, 10 }, new short[8] { -80, -70, -70, -70, -70, -70, -80, -80 }, 80000, 288000, 20, 20, 61500, new short[16]
		{
			-1, 9, -1, -1, -1, -1, -1, -1, 9, 2,
			-1, -1, 2, 12, -1, -1
		}, 8, new short[14]
		{
			12, 6, 12, 12, 6, -1, -1, -1, -1, 12,
			-1, -1, 2, -1
		}, new short[6] { 9, -1, 12, 9, -1, -1 }, new List<sbyte> { 9, 24, 25, 55 }, 3, new ResourceInts(default(int), default(int), default(int), default(int), default(int), default(int), default(int), default(int)), new IntPair[18]
		{
			new IntPair(17, 1),
			new IntPair(8, 1),
			new IntPair(5, 0),
			new IntPair(6, 42),
			new IntPair(15, 1),
			new IntPair(16, 3),
			new IntPair(7, 0),
			new IntPair(14, 0),
			new IntPair(12, 42),
			new IntPair(4, 9),
			new IntPair(3, 3),
			new IntPair(13, 12),
			new IntPair(10, 6),
			new IntPair(2, 3),
			new IntPair(1, 6),
			new IntPair(11, 6),
			new IntPair(0, 3),
			new IntPair(9, 6)
		}, null));
		_dataArray.Add(new OrganizationMemberItem(47, 226, 1, 7, 4, 6, 2, restrictPrincipalAmount: false, 1, -1, -1, -1, 7, -1, 7, 100, 130, new int[2] { 223, 223 }, 8600, 14, 10000, 21150, 0, new List<short> { 18, 19, 20, 6, 12 }, new List<short> { 58, 59, 60, 81, 82 }, new int[2] { 227, 228 }, canStroll: false, 218, new short[4] { 38, 45, 52, 59 }, new PresetEquipmentItemWithProb[8]
		{
			new PresetEquipmentItemWithProb("Armor", 0, 75),
			new PresetEquipmentItemWithProb("Armor", 333, 100),
			new PresetEquipmentItemWithProb("Armor", 396, 75),
			new PresetEquipmentItemWithProb("Armor", 189, 75),
			new PresetEquipmentItemWithProb("Accessory", 180, 75),
			new PresetEquipmentItemWithProb("Accessory", 153, 50),
			new PresetEquipmentItemWithProb("Accessory", 72, 50),
			new PresetEquipmentItemWithProb("Carrier", 0, 75)
		}, new PresetEquipmentItem("Clothing", 20), new List<PresetInventoryItem>
		{
			new PresetInventoryItem("SkillBook", 117, 3, 40),
			new PresetInventoryItem("SkillBook", 9, 1, 40),
			new PresetInventoryItem("Food", 93, 3, 40),
			new PresetInventoryItem("Medicine", 60, 1, 20),
			new PresetInventoryItem("Medicine", 72, 1, 20),
			new PresetInventoryItem("TeaWine", 18, 1, 20),
			new PresetInventoryItem("TeaWine", 27, 1, 20)
		}, new List<PresetOrgMemberCombatSkill>
		{
			new PresetOrgMemberCombatSkill(0, 7),
			new PresetOrgMemberCombatSkill(106, 5),
			new PresetOrgMemberCombatSkill(201, 7),
			new PresetOrgMemberCombatSkill(321, 6),
			new PresetOrgMemberCombatSkill(400, 5),
			new PresetOrgMemberCombatSkill(623, 7)
		}, new sbyte[5] { 8, 8, 8, 8, 8 }, new short[8] { -80, -70, -70, -70, -70, -70, -80, -80 }, 50000, 144000, 15, 30, 42300, new short[16]
		{
			-1, 9, -1, -1, -1, -1, -1, -1, 9, 2,
			-1, -1, 2, 12, -1, -1
		}, 8, new short[14]
		{
			12, 6, 12, 12, 6, -1, -1, -1, -1, 12,
			-1, -1, 2, -1
		}, new short[6] { 9, -1, 12, 9, -1, -1 }, new List<sbyte> { 9, 24, 25 }, 3, new ResourceInts(default(int), default(int), default(int), default(int), default(int), default(int), default(int), default(int)), new IntPair[18]
		{
			new IntPair(17, 1),
			new IntPair(8, 1),
			new IntPair(5, 0),
			new IntPair(6, 42),
			new IntPair(15, 1),
			new IntPair(16, 3),
			new IntPair(7, 0),
			new IntPair(14, 0),
			new IntPair(12, 42),
			new IntPair(4, 9),
			new IntPair(3, 3),
			new IntPair(13, 12),
			new IntPair(10, 6),
			new IntPair(2, 3),
			new IntPair(1, 6),
			new IntPair(11, 6),
			new IntPair(0, 3),
			new IntPair(9, 6)
		}, null));
		_dataArray.Add(new OrganizationMemberItem(48, 229, 1, 6, 6, 9, 3, restrictPrincipalAmount: false, 1, -1, -1, -1, 6, -1, 6, 100, 130, new int[2] { 223, 223 }, 6100, 12, 8000, 13800, 0, new List<short> { 18, 19, 20, 6, 12 }, new List<short> { 58, 59, 60, 81, 82 }, new int[2] { 230, 231 }, canStroll: false, 219, new short[4] { 34, 40, 46, 52 }, new PresetEquipmentItemWithProb[8]
		{
			new PresetEquipmentItemWithProb("Armor", 0, 75),
			new PresetEquipmentItemWithProb("Armor", 333, 100),
			new PresetEquipmentItemWithProb("Armor", 396, 75),
			new PresetEquipmentItemWithProb("Armor", 189, 75),
			new PresetEquipmentItemWithProb("Accessory", 180, 75),
			new PresetEquipmentItemWithProb("Accessory", 153, 50),
			new PresetEquipmentItemWithProb("Accessory", 72, 50),
			new PresetEquipmentItemWithProb("Carrier", 0, 75)
		}, new PresetEquipmentItem("Clothing", 19), new List<PresetInventoryItem>
		{
			new PresetInventoryItem("SkillBook", 117, 3, 40),
			new PresetInventoryItem("SkillBook", 9, 1, 40),
			new PresetInventoryItem("Food", 93, 3, 40),
			new PresetInventoryItem("Medicine", 60, 1, 20),
			new PresetInventoryItem("Medicine", 72, 1, 20),
			new PresetInventoryItem("TeaWine", 18, 1, 20),
			new PresetInventoryItem("TeaWine", 27, 1, 20)
		}, new List<PresetOrgMemberCombatSkill>
		{
			new PresetOrgMemberCombatSkill(0, 6),
			new PresetOrgMemberCombatSkill(106, 4),
			new PresetOrgMemberCombatSkill(201, 6),
			new PresetOrgMemberCombatSkill(321, 6),
			new PresetOrgMemberCombatSkill(400, 4),
			new PresetOrgMemberCombatSkill(623, 6)
		}, new sbyte[5] { 8, 8, 8, 8, 8 }, new short[8] { -80, -70, -70, -70, -70, -70, -80, -80 }, 30000, 72000, 15, 40, 27600, new short[16]
		{
			-1, 9, -1, -1, -1, -1, -1, -1, 9, 2,
			-1, -1, 2, 12, -1, -1
		}, 7, new short[14]
		{
			12, 6, 12, 12, 6, -1, -1, -1, -1, 12,
			-1, -1, 2, -1
		}, new short[6] { 9, -1, 12, 9, -1, -1 }, new List<sbyte> { 9, 24, 25 }, 3, new ResourceInts(default(int), default(int), default(int), default(int), default(int), default(int), default(int), default(int)), new IntPair[18]
		{
			new IntPair(17, 1),
			new IntPair(8, 1),
			new IntPair(5, 0),
			new IntPair(6, 42),
			new IntPair(15, 1),
			new IntPair(16, 3),
			new IntPair(7, 0),
			new IntPair(14, 0),
			new IntPair(12, 42),
			new IntPair(4, 9),
			new IntPair(3, 3),
			new IntPair(13, 12),
			new IntPair(10, 6),
			new IntPair(2, 3),
			new IntPair(1, 6),
			new IntPair(11, 6),
			new IntPair(0, 3),
			new IntPair(9, 6)
		}, null));
		List<OrganizationMemberItem> dataArray47 = _dataArray;
		int[] monasticTitleSuffixes47 = new int[2] { 233, 234 };
		List<short> favoriteClothingIds47 = new List<short> { 18, 19, 20, 6, 12 };
		List<short> hatedClothingIds47 = new List<short> { 58, 59, 60, 81, 82 };
		int[] spouseAnonymousTitles47 = new int[2] { 235, 236 };
		short[] initialAges47 = new short[4] { 30, 35, 40, 45 };
		PresetEquipmentItemWithProb[] equipment47 = new PresetEquipmentItemWithProb[8]
		{
			new PresetEquipmentItemWithProb("Armor", 0, 75),
			new PresetEquipmentItemWithProb("Armor", 333, 100),
			new PresetEquipmentItemWithProb("Armor", 396, 75),
			new PresetEquipmentItemWithProb("Armor", 189, 75),
			new PresetEquipmentItemWithProb("Accessory", 180, 75),
			new PresetEquipmentItemWithProb("Accessory", 153, 50),
			new PresetEquipmentItemWithProb("Accessory", 72, 50),
			new PresetEquipmentItemWithProb("Carrier", 0, 75)
		};
		PresetEquipmentItem clothing47 = new PresetEquipmentItem("Clothing", 19);
		List<PresetInventoryItem> inventory47 = new List<PresetInventoryItem>
		{
			new PresetInventoryItem("SkillBook", 117, 3, 30),
			new PresetInventoryItem("SkillBook", 9, 1, 30),
			new PresetInventoryItem("Food", 93, 3, 30),
			new PresetInventoryItem("Medicine", 54, 1, 20),
			new PresetInventoryItem("Medicine", 66, 1, 20),
			new PresetInventoryItem("TeaWine", 18, 1, 20),
			new PresetInventoryItem("TeaWine", 27, 1, 20)
		};
		List<PresetOrgMemberCombatSkill> combatSkills40 = new List<PresetOrgMemberCombatSkill>
		{
			new PresetOrgMemberCombatSkill(0, 5),
			new PresetOrgMemberCombatSkill(106, 4),
			new PresetOrgMemberCombatSkill(201, 5),
			new PresetOrgMemberCombatSkill(321, 5),
			new PresetOrgMemberCombatSkill(400, 4),
			new PresetOrgMemberCombatSkill(623, 5)
		};
		sbyte[] extraCombatSkillGrids40 = new sbyte[5] { 6, 6, 6, 6, 6 };
		short[] resourcesAdjust40 = new short[8] { -80, -70, -70, -70, -70, -70, -80, -80 };
		short[] lifeSkillsAdjust40 = new short[16]
		{
			-1, 9, -1, -1, -1, -1, -1, -1, 9, 2,
			-1, -1, 2, 12, -1, -1
		};
		short[] combatSkillsAdjust40 = new short[14]
		{
			12, 6, 12, 12, 6, -1, -1, -1, -1, 12,
			-1, -1, 2, -1
		};
		short[] mainAttributesAdjust40 = new short[6] { 9, -1, 12, 9, -1, -1 };
		identityInteractConfig = new List<sbyte>();
		dataArray47.Add(new OrganizationMemberItem(49, 232, 1, 5, 18, 27, 9, restrictPrincipalAmount: false, 1, -1, -1, -1, 5, 8, 5, 100, 130, monasticTitleSuffixes47, 4200, 10, 6000, 8400, 0, favoriteClothingIds47, hatedClothingIds47, spouseAnonymousTitles47, canStroll: false, 220, initialAges47, equipment47, clothing47, inventory47, combatSkills40, extraCombatSkillGrids40, resourcesAdjust40, 20000, 24000, 10, 50, 16800, lifeSkillsAdjust40, 6, combatSkillsAdjust40, mainAttributesAdjust40, identityInteractConfig, 3, new ResourceInts(default(int), default(int), default(int), default(int), default(int), default(int), default(int), default(int)), new IntPair[18]
		{
			new IntPair(17, 1),
			new IntPair(8, 1),
			new IntPair(5, 0),
			new IntPair(6, 33),
			new IntPair(15, 1),
			new IntPair(16, 3),
			new IntPair(7, 0),
			new IntPair(14, 0),
			new IntPair(12, 33),
			new IntPair(4, 6),
			new IntPair(3, 6),
			new IntPair(13, 9),
			new IntPair(10, 6),
			new IntPair(2, 3),
			new IntPair(1, 6),
			new IntPair(11, 6),
			new IntPair(0, 3),
			new IntPair(9, 9)
		}, null));
		List<OrganizationMemberItem> dataArray48 = _dataArray;
		int[] monasticTitleSuffixes48 = new int[2] { 238, 239 };
		List<short> favoriteClothingIds48 = new List<short> { 18, 19, 20, 6, 12 };
		List<short> hatedClothingIds48 = new List<short> { 58, 59, 60, 81, 82 };
		int[] spouseAnonymousTitles48 = new int[2] { 240, 241 };
		short[] initialAges48 = new short[4] { 26, 30, 34, 38 };
		PresetEquipmentItemWithProb[] equipment48 = new PresetEquipmentItemWithProb[8]
		{
			new PresetEquipmentItemWithProb("Armor", 0, 75),
			new PresetEquipmentItemWithProb("Armor", 333, 100),
			new PresetEquipmentItemWithProb("Armor", 396, 75),
			new PresetEquipmentItemWithProb("Armor", 189, 75),
			new PresetEquipmentItemWithProb("Accessory", 180, 75),
			new PresetEquipmentItemWithProb("Accessory", 153, 50),
			new PresetEquipmentItemWithProb("Accessory", 72, 50),
			new PresetEquipmentItemWithProb("Carrier", 0, 75)
		};
		PresetEquipmentItem clothing48 = new PresetEquipmentItem("Clothing", 19);
		List<PresetInventoryItem> inventory48 = new List<PresetInventoryItem>
		{
			new PresetInventoryItem("SkillBook", 117, 3, 30),
			new PresetInventoryItem("SkillBook", 9, 1, 30),
			new PresetInventoryItem("Food", 93, 3, 30),
			new PresetInventoryItem("Medicine", 54, 1, 20),
			new PresetInventoryItem("Medicine", 66, 1, 20),
			new PresetInventoryItem("TeaWine", 18, 1, 20),
			new PresetInventoryItem("TeaWine", 27, 1, 20)
		};
		List<PresetOrgMemberCombatSkill> combatSkills41 = new List<PresetOrgMemberCombatSkill>
		{
			new PresetOrgMemberCombatSkill(0, 4),
			new PresetOrgMemberCombatSkill(106, 3),
			new PresetOrgMemberCombatSkill(201, 4),
			new PresetOrgMemberCombatSkill(321, 4),
			new PresetOrgMemberCombatSkill(400, 3),
			new PresetOrgMemberCombatSkill(623, 4)
		};
		sbyte[] extraCombatSkillGrids41 = new sbyte[5] { 4, 4, 4, 4, 4 };
		short[] resourcesAdjust41 = new short[8] { -80, -70, -70, -70, -70, -70, -80, -80 };
		short[] lifeSkillsAdjust41 = new short[16]
		{
			-1, 9, -1, -1, -1, -1, -1, -1, 9, 2,
			-1, -1, 2, 12, -1, -1
		};
		short[] combatSkillsAdjust41 = new short[14]
		{
			12, 6, 12, 12, 6, -1, -1, -1, -1, 12,
			-1, -1, 2, -1
		};
		short[] mainAttributesAdjust41 = new short[6] { 9, -1, 12, 9, -1, -1 };
		identityInteractConfig = new List<sbyte>();
		dataArray48.Add(new OrganizationMemberItem(50, 237, 1, 4, 4, 6, 2, restrictPrincipalAmount: false, 1, -1, -1, -1, 4, 7, 4, 100, 130, monasticTitleSuffixes48, 2800, 8, 4500, 4650, 0, favoriteClothingIds48, hatedClothingIds48, spouseAnonymousTitles48, canStroll: true, 221, initialAges48, equipment48, clothing48, inventory48, combatSkills41, extraCombatSkillGrids41, resourcesAdjust41, 15000, 12000, 10, 60, 9300, lifeSkillsAdjust41, 5, combatSkillsAdjust41, mainAttributesAdjust41, identityInteractConfig, 3, new ResourceInts(default(int), default(int), default(int), default(int), default(int), default(int), default(int), default(int)), new IntPair[18]
		{
			new IntPair(17, 1),
			new IntPair(8, 1),
			new IntPair(5, 0),
			new IntPair(6, 33),
			new IntPair(15, 1),
			new IntPair(16, 3),
			new IntPair(7, 0),
			new IntPair(14, 0),
			new IntPair(12, 33),
			new IntPair(4, 6),
			new IntPair(3, 6),
			new IntPair(13, 9),
			new IntPair(10, 6),
			new IntPair(2, 3),
			new IntPair(1, 6),
			new IntPair(11, 6),
			new IntPair(0, 3),
			new IntPair(9, 9)
		}, null));
		List<OrganizationMemberItem> dataArray49 = _dataArray;
		int[] monasticTitleSuffixes49 = new int[2] { 243, 244 };
		List<short> favoriteClothingIds49 = new List<short> { 18, 19, 20, 6, 12 };
		List<short> hatedClothingIds49 = new List<short> { 58, 59, 60, 81, 82 };
		int[] spouseAnonymousTitles49 = new int[2] { 245, 246 };
		short[] initialAges49 = new short[4] { 22, 25, 28, 31 };
		PresetEquipmentItemWithProb[] equipment49 = new PresetEquipmentItemWithProb[8]
		{
			new PresetEquipmentItemWithProb("Armor", 0, 75),
			new PresetEquipmentItemWithProb("Armor", 333, 100),
			new PresetEquipmentItemWithProb("Armor", 396, 75),
			new PresetEquipmentItemWithProb("Armor", 189, 75),
			new PresetEquipmentItemWithProb("Accessory", 180, 75),
			new PresetEquipmentItemWithProb("Accessory", 153, 50),
			new PresetEquipmentItemWithProb("Accessory", 72, 50),
			new PresetEquipmentItemWithProb("Carrier", 0, 75)
		};
		PresetEquipmentItem clothing49 = new PresetEquipmentItem("Clothing", 18);
		List<PresetInventoryItem> inventory49 = new List<PresetInventoryItem>
		{
			new PresetInventoryItem("SkillBook", 117, 3, 30),
			new PresetInventoryItem("SkillBook", 9, 1, 30),
			new PresetInventoryItem("Food", 93, 3, 30),
			new PresetInventoryItem("Medicine", 54, 1, 20),
			new PresetInventoryItem("Medicine", 66, 1, 20),
			new PresetInventoryItem("TeaWine", 18, 1, 20),
			new PresetInventoryItem("TeaWine", 27, 1, 20)
		};
		List<PresetOrgMemberCombatSkill> combatSkills42 = new List<PresetOrgMemberCombatSkill>
		{
			new PresetOrgMemberCombatSkill(0, 3),
			new PresetOrgMemberCombatSkill(106, 3),
			new PresetOrgMemberCombatSkill(201, 3),
			new PresetOrgMemberCombatSkill(321, 3),
			new PresetOrgMemberCombatSkill(400, 3),
			new PresetOrgMemberCombatSkill(623, 3)
		};
		sbyte[] extraCombatSkillGrids42 = new sbyte[5] { 4, 4, 4, 4, 4 };
		short[] resourcesAdjust42 = new short[8] { -80, -70, -70, -70, -70, -70, -80, -80 };
		short[] lifeSkillsAdjust42 = new short[16]
		{
			-1, 9, -1, -1, -1, -1, -1, -1, 9, 2,
			-1, -1, 2, 12, -1, -1
		};
		short[] combatSkillsAdjust42 = new short[14]
		{
			12, 6, 12, 12, 6, -1, -1, -1, -1, 12,
			-1, -1, 2, -1
		};
		short[] mainAttributesAdjust42 = new short[6] { 9, -1, 12, 9, -1, -1 };
		identityInteractConfig = new List<sbyte>();
		dataArray49.Add(new OrganizationMemberItem(51, 242, 1, 3, 6, 9, 3, restrictPrincipalAmount: false, 1, -1, -1, -1, 3, 6, 3, 100, 130, monasticTitleSuffixes49, 1800, 6, 3000, 2250, 0, favoriteClothingIds49, hatedClothingIds49, spouseAnonymousTitles49, canStroll: true, 222, initialAges49, equipment49, clothing49, inventory49, combatSkills42, extraCombatSkillGrids42, resourcesAdjust42, 10000, 6000, 10, 70, 4500, lifeSkillsAdjust42, 4, combatSkillsAdjust42, mainAttributesAdjust42, identityInteractConfig, 3, new ResourceInts(default(int), default(int), default(int), default(int), default(int), default(int), default(int), default(int)), new IntPair[18]
		{
			new IntPair(17, 1),
			new IntPair(8, 1),
			new IntPair(5, 0),
			new IntPair(6, 33),
			new IntPair(15, 1),
			new IntPair(16, 3),
			new IntPair(7, 0),
			new IntPair(14, 0),
			new IntPair(12, 33),
			new IntPair(4, 6),
			new IntPair(3, 6),
			new IntPair(13, 9),
			new IntPair(10, 6),
			new IntPair(2, 3),
			new IntPair(1, 6),
			new IntPair(11, 6),
			new IntPair(0, 3),
			new IntPair(9, 9)
		}, null));
		List<OrganizationMemberItem> dataArray50 = _dataArray;
		int[] monasticTitleSuffixes50 = new int[2] { 248, 249 };
		List<short> favoriteClothingIds50 = new List<short> { 18, 19, 20, 6, 12 };
		List<short> hatedClothingIds50 = new List<short> { 58, 59, 60, 81, 82 };
		int[] spouseAnonymousTitles50 = new int[2] { 250, 251 };
		short[] initialAges50 = new short[4] { 18, 20, 22, 24 };
		PresetEquipmentItemWithProb[] equipment50 = new PresetEquipmentItemWithProb[8]
		{
			new PresetEquipmentItemWithProb("Armor", 0, 75),
			new PresetEquipmentItemWithProb("Armor", 333, 100),
			new PresetEquipmentItemWithProb("Armor", 396, 75),
			new PresetEquipmentItemWithProb("Armor", 189, 75),
			new PresetEquipmentItemWithProb("Accessory", 180, 75),
			new PresetEquipmentItemWithProb("Accessory", 153, 50),
			new PresetEquipmentItemWithProb("Accessory", 72, 50),
			new PresetEquipmentItemWithProb("Carrier", 0, 75)
		};
		PresetEquipmentItem clothing50 = new PresetEquipmentItem("Clothing", 18);
		List<PresetInventoryItem> inventory50 = new List<PresetInventoryItem>
		{
			new PresetInventoryItem("SkillBook", 117, 3, 20),
			new PresetInventoryItem("SkillBook", 9, 1, 20),
			new PresetInventoryItem("Food", 93, 3, 20)
		};
		List<PresetOrgMemberCombatSkill> combatSkills43 = new List<PresetOrgMemberCombatSkill>
		{
			new PresetOrgMemberCombatSkill(0, 2),
			new PresetOrgMemberCombatSkill(106, 2),
			new PresetOrgMemberCombatSkill(201, 2),
			new PresetOrgMemberCombatSkill(321, 2),
			new PresetOrgMemberCombatSkill(623, 2)
		};
		sbyte[] extraCombatSkillGrids43 = new sbyte[5] { 2, 2, 2, 2, 2 };
		short[] resourcesAdjust43 = new short[8] { -80, -70, -70, -70, -70, -70, -80, -80 };
		short[] lifeSkillsAdjust43 = new short[16]
		{
			-1, 9, -1, -1, -1, -1, -1, -1, 9, 2,
			-1, -1, 2, 12, -1, -1
		};
		short[] combatSkillsAdjust43 = new short[14]
		{
			12, 6, 12, 12, 6, -1, -1, -1, -1, 12,
			-1, -1, 2, -1
		};
		short[] mainAttributesAdjust43 = new short[6] { 9, -1, 12, 9, -1, -1 };
		identityInteractConfig = new List<sbyte>();
		dataArray50.Add(new OrganizationMemberItem(52, 247, 1, 2, 6, 9, 3, restrictPrincipalAmount: false, 1, -1, -1, -1, 2, 6, 2, 100, 130, monasticTitleSuffixes50, 600, 4, 2000, 900, 0, favoriteClothingIds50, hatedClothingIds50, spouseAnonymousTitles50, canStroll: true, 223, initialAges50, equipment50, clothing50, inventory50, combatSkills43, extraCombatSkillGrids43, resourcesAdjust43, 7500, 2000, 5, 80, 1800, lifeSkillsAdjust43, 3, combatSkillsAdjust43, mainAttributesAdjust43, identityInteractConfig, 3, new ResourceInts(default(int), default(int), default(int), default(int), default(int), default(int), default(int), default(int)), new IntPair[18]
		{
			new IntPair(17, 1),
			new IntPair(8, 1),
			new IntPair(5, 0),
			new IntPair(6, 24),
			new IntPair(15, 1),
			new IntPair(16, 3),
			new IntPair(7, 0),
			new IntPair(14, 0),
			new IntPair(12, 24),
			new IntPair(4, 3),
			new IntPair(3, 9),
			new IntPair(13, 6),
			new IntPair(10, 6),
			new IntPair(2, 3),
			new IntPair(1, 6),
			new IntPair(11, 6),
			new IntPair(0, 3),
			new IntPair(9, 12)
		}, null));
		List<OrganizationMemberItem> dataArray51 = _dataArray;
		int[] monasticTitleSuffixes51 = new int[2] { 253, 254 };
		List<short> favoriteClothingIds51 = new List<short> { 18, 19, 20, 6, 12 };
		List<short> hatedClothingIds51 = new List<short> { 58, 59, 60, 81, 82 };
		int[] spouseAnonymousTitles51 = new int[2] { 255, 256 };
		short[] initialAges51 = new short[4] { 14, 15, 16, 17 };
		PresetEquipmentItemWithProb[] equipment51 = new PresetEquipmentItemWithProb[8]
		{
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Armor", 333, 100),
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Armor", 189, 75),
			new PresetEquipmentItemWithProb("Accessory", 180, 75),
			new PresetEquipmentItemWithProb("Accessory", 153, 50),
			new PresetEquipmentItemWithProb("Accessory", -1, 0),
			new PresetEquipmentItemWithProb("Carrier", -1, 0)
		};
		PresetEquipmentItem clothing51 = new PresetEquipmentItem("Clothing", 18);
		List<PresetInventoryItem> inventory51 = new List<PresetInventoryItem>
		{
			new PresetInventoryItem("SkillBook", 117, 3, 20),
			new PresetInventoryItem("SkillBook", 9, 1, 20),
			new PresetInventoryItem("Food", 93, 3, 20)
		};
		List<PresetOrgMemberCombatSkill> combatSkills44 = new List<PresetOrgMemberCombatSkill>
		{
			new PresetOrgMemberCombatSkill(0, 1),
			new PresetOrgMemberCombatSkill(106, 1),
			new PresetOrgMemberCombatSkill(201, 1),
			new PresetOrgMemberCombatSkill(623, 1)
		};
		sbyte[] extraCombatSkillGrids44 = new sbyte[5];
		short[] resourcesAdjust44 = new short[8] { -80, -70, -70, -70, -70, -70, -80, -80 };
		short[] lifeSkillsAdjust44 = new short[16]
		{
			-1, 9, -1, -1, -1, -1, -1, -1, 9, 2,
			-1, -1, 2, 12, -1, -1
		};
		short[] combatSkillsAdjust44 = new short[14]
		{
			12, 6, 12, 12, 6, -1, -1, -1, -1, 12,
			-1, -1, 2, -1
		};
		short[] mainAttributesAdjust44 = new short[6] { 9, -1, 12, 9, -1, -1 };
		identityInteractConfig = new List<sbyte>();
		dataArray51.Add(new OrganizationMemberItem(53, 252, 1, 1, 6, 9, 3, restrictPrincipalAmount: false, 1, -1, -1, -1, 1, 6, 1, 100, 130, monasticTitleSuffixes51, 300, 2, 1000, 300, 0, favoriteClothingIds51, hatedClothingIds51, spouseAnonymousTitles51, canStroll: true, 224, initialAges51, equipment51, clothing51, inventory51, combatSkills44, extraCombatSkillGrids44, resourcesAdjust44, 5000, 1000, 5, 90, 600, lifeSkillsAdjust44, 2, combatSkillsAdjust44, mainAttributesAdjust44, identityInteractConfig, 3, new ResourceInts(default(int), default(int), default(int), default(int), default(int), default(int), default(int), default(int)), new IntPair[18]
		{
			new IntPair(17, 1),
			new IntPair(8, 1),
			new IntPair(5, 0),
			new IntPair(6, 24),
			new IntPair(15, 1),
			new IntPair(16, 3),
			new IntPair(7, 0),
			new IntPair(14, 0),
			new IntPair(12, 24),
			new IntPair(4, 3),
			new IntPair(3, 9),
			new IntPair(13, 6),
			new IntPair(10, 6),
			new IntPair(2, 3),
			new IntPair(1, 6),
			new IntPair(11, 6),
			new IntPair(0, 3),
			new IntPair(9, 12)
		}, null));
		List<OrganizationMemberItem> dataArray52 = _dataArray;
		int[] monasticTitleSuffixes52 = new int[2] { 258, 259 };
		List<short> favoriteClothingIds52 = new List<short> { 18, 19, 20, 6, 12 };
		List<short> hatedClothingIds52 = new List<short> { 58, 59, 60, 81, 82 };
		int[] spouseAnonymousTitles52 = new int[2] { 260, 261 };
		short[] initialAges52 = new short[4] { 10, 10, 10, 10 };
		PresetEquipmentItemWithProb[] equipment52 = new PresetEquipmentItemWithProb[8]
		{
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Armor", 333, 100),
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Armor", 189, 75),
			new PresetEquipmentItemWithProb("Accessory", 180, 75),
			new PresetEquipmentItemWithProb("Accessory", -1, 0),
			new PresetEquipmentItemWithProb("Accessory", -1, 0),
			new PresetEquipmentItemWithProb("Carrier", -1, 0)
		};
		PresetEquipmentItem clothing52 = new PresetEquipmentItem("Clothing", 18);
		List<PresetInventoryItem> inventory52 = new List<PresetInventoryItem>
		{
			new PresetInventoryItem("SkillBook", 117, 3, 20),
			new PresetInventoryItem("SkillBook", 9, 1, 20),
			new PresetInventoryItem("Food", 93, 3, 20)
		};
		List<PresetOrgMemberCombatSkill> combatSkills45 = new List<PresetOrgMemberCombatSkill>
		{
			new PresetOrgMemberCombatSkill(0, 0),
			new PresetOrgMemberCombatSkill(106, 0),
			new PresetOrgMemberCombatSkill(623, 0)
		};
		sbyte[] extraCombatSkillGrids45 = new sbyte[5];
		short[] resourcesAdjust45 = new short[8] { -80, -70, -70, -70, -70, -70, -80, -80 };
		short[] lifeSkillsAdjust45 = new short[16]
		{
			-1, 9, -1, -1, -1, -1, -1, -1, 9, 2,
			-1, -1, 2, 12, -1, -1
		};
		short[] combatSkillsAdjust45 = new short[14]
		{
			12, 6, 12, 12, 6, -1, -1, -1, -1, 12,
			-1, -1, 2, -1
		};
		short[] mainAttributesAdjust45 = new short[6] { 9, -1, 12, 9, -1, -1 };
		identityInteractConfig = new List<sbyte>();
		dataArray52.Add(new OrganizationMemberItem(54, 257, 1, 0, 4, 6, 2, restrictPrincipalAmount: false, 1, -1, -1, -1, 0, 6, 0, 100, 130, monasticTitleSuffixes52, 150, 0, 500, 150, 0, favoriteClothingIds52, hatedClothingIds52, spouseAnonymousTitles52, canStroll: true, 225, initialAges52, equipment52, clothing52, inventory52, combatSkills45, extraCombatSkillGrids45, resourcesAdjust45, 2500, 500, 1, 100, 300, lifeSkillsAdjust45, 1, combatSkillsAdjust45, mainAttributesAdjust45, identityInteractConfig, 3, new ResourceInts(default(int), default(int), default(int), default(int), default(int), default(int), default(int), default(int)), new IntPair[18]
		{
			new IntPair(17, 1),
			new IntPair(8, 1),
			new IntPair(5, 0),
			new IntPair(6, 24),
			new IntPair(15, 1),
			new IntPair(16, 3),
			new IntPair(7, 0),
			new IntPair(14, 0),
			new IntPair(12, 24),
			new IntPair(4, 3),
			new IntPair(3, 9),
			new IntPair(13, 6),
			new IntPair(10, 6),
			new IntPair(2, 3),
			new IntPair(1, 6),
			new IntPair(11, 6),
			new IntPair(0, 3),
			new IntPair(9, 12)
		}, null));
		_dataArray.Add(new OrganizationMemberItem(55, 262, 2, 8, 1, 1, 1, restrictPrincipalAmount: true, -1, -1, 7, 4, 7, -1, 7, 50, 130, new int[2] { 263, 264 }, 15800, 16, 13000, 30750, 0, new List<short> { 21, 22, 23, 6, 12, 5, 14 }, new List<short> { 49, 50, 51 }, new int[2] { 265, 266 }, canStroll: false, 226, new short[4] { 31, 38, 45, 52 }, new PresetEquipmentItemWithProb[8]
		{
			new PresetEquipmentItemWithProb("Armor", 99, 75),
			new PresetEquipmentItemWithProb("Armor", 342, 100),
			new PresetEquipmentItemWithProb("Armor", 495, 75),
			new PresetEquipmentItemWithProb("Armor", 207, 75),
			new PresetEquipmentItemWithProb("Accessory", 117, 75),
			new PresetEquipmentItemWithProb("Accessory", 135, 50),
			new PresetEquipmentItemWithProb("Accessory", 216, 50),
			new PresetEquipmentItemWithProb("Carrier", 18, 75)
		}, new PresetEquipmentItem("Clothing", 23), new List<PresetInventoryItem>
		{
			new PresetInventoryItem("SkillBook", 117, 1, 40),
			new PresetInventoryItem("SkillBook", 108, 1, 40),
			new PresetInventoryItem("SkillBook", 126, 1, 30),
			new PresetInventoryItem("Food", 0, 3, 40),
			new PresetInventoryItem("Food", 93, 3, 40),
			new PresetInventoryItem("Food", 9, 3, 40),
			new PresetInventoryItem("Food", 51, 3, 40),
			new PresetInventoryItem("Medicine", 60, 1, 20),
			new PresetInventoryItem("Medicine", 72, 1, 20),
			new PresetInventoryItem("Medicine", 256, 1, 20),
			new PresetInventoryItem("Medicine", 244, 1, 20),
			new PresetInventoryItem("Medicine", 232, 1, 20),
			new PresetInventoryItem("Medicine", 328, 1, 20),
			new PresetInventoryItem("Medicine", 304, 1, 20),
			new PresetInventoryItem("Medicine", 280, 1, 20),
			new PresetInventoryItem("TeaWine", 18, 1, 20),
			new PresetInventoryItem("TeaWine", 27, 1, 20)
		}, new List<PresetOrgMemberCombatSkill>
		{
			new PresetOrgMemberCombatSkill(9, 7),
			new PresetOrgMemberCombatSkill(112, 6),
			new PresetOrgMemberCombatSkill(210, 8),
			new PresetOrgMemberCombatSkill(329, 6),
			new PresetOrgMemberCombatSkill(406, 8),
			new PresetOrgMemberCombatSkill(525, 6),
			new PresetOrgMemberCombatSkill(648, 7)
		}, new sbyte[5] { 10, 10, 10, 10, 10 }, new short[8] { -60, -60, -70, -70, -70, -70, -70, -60 }, 160000, 576000, 20, 20, 61500, new short[16]
		{
			-1, -1, -1, -1, 2, -1, -1, -1, -1, -1,
			-1, -1, 9, 9, 9, -1
		}, 8, new short[14]
		{
			12, 9, 12, 9, 12, -1, -1, 9, -1, -1,
			12, -1, -1, -1
		}, new short[6] { -1, 12, 9, 2, 9, 9 }, new List<sbyte> { 10, 24, 25, 55 }, 3, new ResourceInts(default(int), default(int), default(int), default(int), default(int), default(int), default(int), default(int)), new IntPair[18]
		{
			new IntPair(17, 1),
			new IntPair(8, 3),
			new IntPair(5, 24),
			new IntPair(6, 24),
			new IntPair(15, 1),
			new IntPair(16, 1),
			new IntPair(7, 0),
			new IntPair(14, 1),
			new IntPair(12, 24),
			new IntPair(4, 15),
			new IntPair(3, 15),
			new IntPair(13, 9),
			new IntPair(10, 3),
			new IntPair(2, 1),
			new IntPair(1, 6),
			new IntPair(11, 6),
			new IntPair(0, 9),
			new IntPair(9, 1)
		}, null));
		_dataArray.Add(new OrganizationMemberItem(56, 229, 2, 7, 4, 6, 2, restrictPrincipalAmount: false, -1, -1, -1, 3, 7, -1, 7, 50, 130, new int[2] { 263, 264 }, 8600, 14, 10000, 21150, 0, new List<short> { 21, 22, 23, 6, 12, 5, 14 }, new List<short> { 49, 50, 51 }, new int[2] { 267, 268 }, canStroll: false, 227, new short[4] { 34, 42, 50, 58 }, new PresetEquipmentItemWithProb[8]
		{
			new PresetEquipmentItemWithProb("Armor", 99, 75),
			new PresetEquipmentItemWithProb("Armor", 342, 100),
			new PresetEquipmentItemWithProb("Armor", 495, 75),
			new PresetEquipmentItemWithProb("Armor", 207, 75),
			new PresetEquipmentItemWithProb("Accessory", 117, 75),
			new PresetEquipmentItemWithProb("Accessory", 135, 50),
			new PresetEquipmentItemWithProb("Accessory", 216, 50),
			new PresetEquipmentItemWithProb("Carrier", 18, 75)
		}, new PresetEquipmentItem("Clothing", 22), new List<PresetInventoryItem>
		{
			new PresetInventoryItem("SkillBook", 117, 1, 40),
			new PresetInventoryItem("SkillBook", 108, 1, 40),
			new PresetInventoryItem("SkillBook", 126, 1, 30),
			new PresetInventoryItem("Food", 0, 3, 40),
			new PresetInventoryItem("Food", 93, 3, 40),
			new PresetInventoryItem("Food", 9, 3, 40),
			new PresetInventoryItem("Food", 51, 3, 40),
			new PresetInventoryItem("Medicine", 60, 1, 20),
			new PresetInventoryItem("Medicine", 72, 1, 20),
			new PresetInventoryItem("Medicine", 256, 1, 20),
			new PresetInventoryItem("Medicine", 244, 1, 20),
			new PresetInventoryItem("Medicine", 232, 1, 20),
			new PresetInventoryItem("Medicine", 328, 1, 20),
			new PresetInventoryItem("Medicine", 304, 1, 20),
			new PresetInventoryItem("Medicine", 280, 1, 20),
			new PresetInventoryItem("TeaWine", 18, 1, 20),
			new PresetInventoryItem("TeaWine", 27, 1, 20)
		}, new List<PresetOrgMemberCombatSkill>
		{
			new PresetOrgMemberCombatSkill(9, 7),
			new PresetOrgMemberCombatSkill(112, 5),
			new PresetOrgMemberCombatSkill(210, 7),
			new PresetOrgMemberCombatSkill(329, 6),
			new PresetOrgMemberCombatSkill(406, 7),
			new PresetOrgMemberCombatSkill(525, 6),
			new PresetOrgMemberCombatSkill(648, 6)
		}, new sbyte[5] { 8, 8, 8, 8, 8 }, new short[8] { -60, -60, -70, -70, -70, -70, -70, -60 }, 100000, 288000, 15, 30, 42300, new short[16]
		{
			-1, -1, -1, -1, 2, -1, -1, -1, -1, -1,
			-1, -1, 9, 9, 9, -1
		}, 8, new short[14]
		{
			12, 9, 12, 9, 12, -1, -1, 9, -1, -1,
			12, -1, -1, -1
		}, new short[6] { -1, 12, 9, 2, 9, 9 }, new List<sbyte> { 10, 24, 25 }, 3, new ResourceInts(default(int), default(int), default(int), default(int), default(int), default(int), default(int), default(int)), new IntPair[18]
		{
			new IntPair(17, 1),
			new IntPair(8, 3),
			new IntPair(5, 24),
			new IntPair(6, 24),
			new IntPair(15, 1),
			new IntPair(16, 1),
			new IntPair(7, 0),
			new IntPair(14, 1),
			new IntPair(12, 24),
			new IntPair(4, 15),
			new IntPair(3, 15),
			new IntPair(13, 9),
			new IntPair(10, 3),
			new IntPair(2, 1),
			new IntPair(1, 6),
			new IntPair(11, 6),
			new IntPair(0, 9),
			new IntPair(9, 1)
		}, null));
		_dataArray.Add(new OrganizationMemberItem(57, 269, 2, 6, 4, 6, 2, restrictPrincipalAmount: false, -1, -1, -1, 2, 5, 8, 6, 50, 130, new int[2] { 263, 264 }, 6100, 12, 8000, 13800, 0, new List<short> { 21, 22, 23, 6, 12, 5, 14 }, new List<short> { 49, 50, 51 }, new int[2] { 270, 271 }, canStroll: false, 228, new short[4] { 28, 34, 40, 46 }, new PresetEquipmentItemWithProb[8]
		{
			new PresetEquipmentItemWithProb("Armor", 99, 75),
			new PresetEquipmentItemWithProb("Armor", 342, 100),
			new PresetEquipmentItemWithProb("Armor", 495, 75),
			new PresetEquipmentItemWithProb("Armor", 207, 75),
			new PresetEquipmentItemWithProb("Accessory", 117, 75),
			new PresetEquipmentItemWithProb("Accessory", 135, 50),
			new PresetEquipmentItemWithProb("Accessory", 216, 50),
			new PresetEquipmentItemWithProb("Carrier", 18, 75)
		}, new PresetEquipmentItem("Clothing", 22), new List<PresetInventoryItem>
		{
			new PresetInventoryItem("SkillBook", 117, 1, 40),
			new PresetInventoryItem("SkillBook", 108, 1, 40),
			new PresetInventoryItem("SkillBook", 126, 1, 30),
			new PresetInventoryItem("Food", 0, 3, 40),
			new PresetInventoryItem("Food", 93, 3, 40),
			new PresetInventoryItem("Food", 9, 3, 40),
			new PresetInventoryItem("Food", 51, 3, 40),
			new PresetInventoryItem("Medicine", 60, 1, 20),
			new PresetInventoryItem("Medicine", 72, 1, 20),
			new PresetInventoryItem("Medicine", 256, 1, 20),
			new PresetInventoryItem("Medicine", 244, 1, 20),
			new PresetInventoryItem("Medicine", 232, 1, 20),
			new PresetInventoryItem("Medicine", 328, 1, 20),
			new PresetInventoryItem("Medicine", 304, 1, 20),
			new PresetInventoryItem("Medicine", 280, 1, 20),
			new PresetInventoryItem("TeaWine", 18, 1, 20),
			new PresetInventoryItem("TeaWine", 27, 1, 20)
		}, new List<PresetOrgMemberCombatSkill>
		{
			new PresetOrgMemberCombatSkill(9, 6),
			new PresetOrgMemberCombatSkill(112, 5),
			new PresetOrgMemberCombatSkill(210, 6),
			new PresetOrgMemberCombatSkill(329, 5),
			new PresetOrgMemberCombatSkill(406, 6),
			new PresetOrgMemberCombatSkill(525, 5),
			new PresetOrgMemberCombatSkill(648, 6)
		}, new sbyte[5] { 8, 8, 8, 8, 8 }, new short[8] { -60, -60, -70, -70, -70, -70, -70, -60 }, 60000, 144000, 15, 40, 27600, new short[16]
		{
			-1, -1, -1, -1, 2, -1, -1, -1, -1, -1,
			-1, -1, 9, 9, 9, -1
		}, 7, new short[14]
		{
			12, 9, 12, 9, 12, -1, -1, 9, -1, -1,
			12, -1, -1, -1
		}, new short[6] { -1, 12, 9, 2, 9, 9 }, new List<sbyte> { 10, 24, 25 }, 3, new ResourceInts(default(int), default(int), default(int), default(int), default(int), default(int), default(int), default(int)), new IntPair[18]
		{
			new IntPair(17, 1),
			new IntPair(8, 3),
			new IntPair(5, 24),
			new IntPair(6, 24),
			new IntPair(15, 1),
			new IntPair(16, 1),
			new IntPair(7, 0),
			new IntPair(14, 1),
			new IntPair(12, 24),
			new IntPair(4, 15),
			new IntPair(3, 15),
			new IntPair(13, 9),
			new IntPair(10, 3),
			new IntPair(2, 1),
			new IntPair(1, 6),
			new IntPair(11, 6),
			new IntPair(0, 9),
			new IntPair(9, 1)
		}, null));
		List<OrganizationMemberItem> dataArray53 = _dataArray;
		int[] monasticTitleSuffixes53 = new int[2] { 273, 274 };
		List<short> favoriteClothingIds53 = new List<short> { 21, 22, 23, 6, 12, 5, 14 };
		List<short> hatedClothingIds53 = new List<short> { 49, 50, 51 };
		int[] spouseAnonymousTitles53 = new int[2] { 275, 276 };
		short[] initialAges53 = new short[4] { 25, 30, 35, 40 };
		PresetEquipmentItemWithProb[] equipment53 = new PresetEquipmentItemWithProb[8]
		{
			new PresetEquipmentItemWithProb("Armor", 99, 75),
			new PresetEquipmentItemWithProb("Armor", 342, 100),
			new PresetEquipmentItemWithProb("Armor", 495, 75),
			new PresetEquipmentItemWithProb("Armor", 207, 75),
			new PresetEquipmentItemWithProb("Accessory", 117, 75),
			new PresetEquipmentItemWithProb("Accessory", 135, 50),
			new PresetEquipmentItemWithProb("Accessory", 216, 50),
			new PresetEquipmentItemWithProb("Carrier", 18, 75)
		};
		PresetEquipmentItem clothing53 = new PresetEquipmentItem("Clothing", 22);
		List<PresetInventoryItem> inventory53 = new List<PresetInventoryItem>
		{
			new PresetInventoryItem("SkillBook", 117, 1, 30),
			new PresetInventoryItem("SkillBook", 108, 1, 30),
			new PresetInventoryItem("SkillBook", 126, 1, 20),
			new PresetInventoryItem("Food", 0, 3, 30),
			new PresetInventoryItem("Food", 93, 3, 30),
			new PresetInventoryItem("Food", 9, 3, 30),
			new PresetInventoryItem("Food", 51, 3, 30),
			new PresetInventoryItem("Medicine", 54, 1, 20),
			new PresetInventoryItem("Medicine", 66, 1, 20),
			new PresetInventoryItem("Medicine", 250, 1, 20),
			new PresetInventoryItem("Medicine", 238, 1, 20),
			new PresetInventoryItem("Medicine", 226, 1, 20),
			new PresetInventoryItem("Medicine", 322, 1, 20),
			new PresetInventoryItem("Medicine", 298, 1, 20),
			new PresetInventoryItem("Medicine", 274, 1, 20),
			new PresetInventoryItem("TeaWine", 18, 1, 20),
			new PresetInventoryItem("TeaWine", 27, 1, 20)
		};
		List<PresetOrgMemberCombatSkill> combatSkills46 = new List<PresetOrgMemberCombatSkill>
		{
			new PresetOrgMemberCombatSkill(9, 5),
			new PresetOrgMemberCombatSkill(112, 4),
			new PresetOrgMemberCombatSkill(210, 5),
			new PresetOrgMemberCombatSkill(329, 5),
			new PresetOrgMemberCombatSkill(406, 5),
			new PresetOrgMemberCombatSkill(525, 5),
			new PresetOrgMemberCombatSkill(648, 5)
		};
		sbyte[] extraCombatSkillGrids46 = new sbyte[5] { 6, 6, 6, 6, 6 };
		short[] resourcesAdjust46 = new short[8] { -60, -60, -70, -70, -70, -70, -70, -60 };
		short[] lifeSkillsAdjust46 = new short[16]
		{
			-1, -1, -1, -1, 2, -1, -1, -1, -1, -1,
			-1, -1, 9, 9, 9, -1
		};
		short[] combatSkillsAdjust46 = new short[14]
		{
			12, 9, 12, 9, 12, -1, -1, 9, -1, -1,
			12, -1, -1, -1
		};
		short[] mainAttributesAdjust46 = new short[6] { -1, 12, 9, 2, 9, 9 };
		identityInteractConfig = new List<sbyte>();
		dataArray53.Add(new OrganizationMemberItem(58, 272, 2, 5, 6, 9, 3, restrictPrincipalAmount: false, -1, -1, -1, 2, 5, 8, 5, 50, 130, monasticTitleSuffixes53, 4200, 10, 6000, 8400, 0, favoriteClothingIds53, hatedClothingIds53, spouseAnonymousTitles53, canStroll: true, 229, initialAges53, equipment53, clothing53, inventory53, combatSkills46, extraCombatSkillGrids46, resourcesAdjust46, 40000, 48000, 10, 50, 16800, lifeSkillsAdjust46, 6, combatSkillsAdjust46, mainAttributesAdjust46, identityInteractConfig, 3, new ResourceInts(default(int), default(int), default(int), default(int), default(int), default(int), default(int), default(int)), new IntPair[18]
		{
			new IntPair(17, 1),
			new IntPair(8, 3),
			new IntPair(5, 18),
			new IntPair(6, 18),
			new IntPair(15, 1),
			new IntPair(16, 1),
			new IntPair(7, 0),
			new IntPair(14, 1),
			new IntPair(12, 18),
			new IntPair(4, 15),
			new IntPair(3, 15),
			new IntPair(13, 6),
			new IntPair(10, 3),
			new IntPair(2, 1),
			new IntPair(1, 6),
			new IntPair(11, 6),
			new IntPair(0, 12),
			new IntPair(9, 1)
		}, null));
		List<OrganizationMemberItem> dataArray54 = _dataArray;
		int[] monasticTitleSuffixes54 = new int[2] { 278, 279 };
		List<short> favoriteClothingIds54 = new List<short> { 21, 22, 23, 6, 12, 5, 14 };
		List<short> hatedClothingIds54 = new List<short> { 49, 50, 51 };
		int[] spouseAnonymousTitles54 = new int[2] { 280, 281 };
		short[] initialAges54 = new short[4] { 22, 26, 30, 34 };
		PresetEquipmentItemWithProb[] equipment54 = new PresetEquipmentItemWithProb[8]
		{
			new PresetEquipmentItemWithProb("Armor", 99, 75),
			new PresetEquipmentItemWithProb("Armor", 342, 100),
			new PresetEquipmentItemWithProb("Armor", 495, 75),
			new PresetEquipmentItemWithProb("Armor", 207, 75),
			new PresetEquipmentItemWithProb("Accessory", 117, 75),
			new PresetEquipmentItemWithProb("Accessory", 135, 50),
			new PresetEquipmentItemWithProb("Accessory", 216, 50),
			new PresetEquipmentItemWithProb("Carrier", 18, 75)
		};
		PresetEquipmentItem clothing54 = new PresetEquipmentItem("Clothing", 21);
		List<PresetInventoryItem> inventory54 = new List<PresetInventoryItem>
		{
			new PresetInventoryItem("SkillBook", 117, 1, 30),
			new PresetInventoryItem("SkillBook", 108, 1, 30),
			new PresetInventoryItem("SkillBook", 126, 1, 20),
			new PresetInventoryItem("Food", 0, 3, 30),
			new PresetInventoryItem("Food", 93, 3, 30),
			new PresetInventoryItem("Food", 9, 3, 30),
			new PresetInventoryItem("Food", 51, 3, 30),
			new PresetInventoryItem("Medicine", 54, 1, 20),
			new PresetInventoryItem("Medicine", 66, 1, 20),
			new PresetInventoryItem("Medicine", 250, 1, 20),
			new PresetInventoryItem("Medicine", 238, 1, 20),
			new PresetInventoryItem("Medicine", 226, 1, 20),
			new PresetInventoryItem("Medicine", 322, 1, 20),
			new PresetInventoryItem("Medicine", 298, 1, 20),
			new PresetInventoryItem("Medicine", 274, 1, 20),
			new PresetInventoryItem("TeaWine", 18, 1, 20),
			new PresetInventoryItem("TeaWine", 27, 1, 20)
		};
		List<PresetOrgMemberCombatSkill> combatSkills47 = new List<PresetOrgMemberCombatSkill>
		{
			new PresetOrgMemberCombatSkill(9, 4),
			new PresetOrgMemberCombatSkill(112, 4),
			new PresetOrgMemberCombatSkill(210, 4),
			new PresetOrgMemberCombatSkill(406, 4),
			new PresetOrgMemberCombatSkill(525, 4),
			new PresetOrgMemberCombatSkill(648, 4)
		};
		sbyte[] extraCombatSkillGrids47 = new sbyte[5] { 4, 4, 4, 4, 4 };
		short[] resourcesAdjust47 = new short[8] { -60, -60, -70, -70, -70, -70, -70, -60 };
		short[] lifeSkillsAdjust47 = new short[16]
		{
			-1, -1, -1, -1, 2, -1, -1, -1, -1, -1,
			-1, -1, 9, 9, 9, -1
		};
		short[] combatSkillsAdjust47 = new short[14]
		{
			12, 9, 12, 9, 12, -1, -1, 9, -1, -1,
			12, -1, -1, -1
		};
		short[] mainAttributesAdjust47 = new short[6] { -1, 12, 9, 2, 9, 9 };
		identityInteractConfig = new List<sbyte>();
		dataArray54.Add(new OrganizationMemberItem(59, 277, 2, 4, 8, 12, 4, restrictPrincipalAmount: false, -1, -1, -1, 1, 4, 7, 4, 50, 130, monasticTitleSuffixes54, 2800, 8, 4500, 4650, 0, favoriteClothingIds54, hatedClothingIds54, spouseAnonymousTitles54, canStroll: true, 230, initialAges54, equipment54, clothing54, inventory54, combatSkills47, extraCombatSkillGrids47, resourcesAdjust47, 30000, 24000, 10, 60, 9300, lifeSkillsAdjust47, 5, combatSkillsAdjust47, mainAttributesAdjust47, identityInteractConfig, 3, new ResourceInts(default(int), default(int), default(int), default(int), default(int), default(int), default(int), default(int)), new IntPair[18]
		{
			new IntPair(17, 1),
			new IntPair(8, 3),
			new IntPair(5, 18),
			new IntPair(6, 18),
			new IntPair(15, 1),
			new IntPair(16, 1),
			new IntPair(7, 0),
			new IntPair(14, 1),
			new IntPair(12, 18),
			new IntPair(4, 15),
			new IntPair(3, 15),
			new IntPair(13, 6),
			new IntPair(10, 3),
			new IntPair(2, 1),
			new IntPair(1, 6),
			new IntPair(11, 6),
			new IntPair(0, 12),
			new IntPair(9, 1)
		}, null));
	}

	private void CreateItems1()
	{
		List<OrganizationMemberItem> dataArray = _dataArray;
		int[] monasticTitleSuffixes = new int[2] { 283, 284 };
		List<short> favoriteClothingIds = new List<short> { 21, 22, 23, 6, 12, 5, 14 };
		List<short> hatedClothingIds = new List<short> { 49, 50, 51 };
		int[] spouseAnonymousTitles = new int[2] { 285, 286 };
		short[] initialAges = new short[4] { 19, 22, 25, 28 };
		PresetEquipmentItemWithProb[] equipment = new PresetEquipmentItemWithProb[8]
		{
			new PresetEquipmentItemWithProb("Armor", 99, 75),
			new PresetEquipmentItemWithProb("Armor", 342, 100),
			new PresetEquipmentItemWithProb("Armor", 495, 75),
			new PresetEquipmentItemWithProb("Armor", 207, 75),
			new PresetEquipmentItemWithProb("Accessory", 117, 75),
			new PresetEquipmentItemWithProb("Accessory", 135, 50),
			new PresetEquipmentItemWithProb("Accessory", 216, 50),
			new PresetEquipmentItemWithProb("Carrier", 18, 75)
		};
		PresetEquipmentItem clothing = new PresetEquipmentItem("Clothing", 21);
		List<PresetInventoryItem> inventory = new List<PresetInventoryItem>
		{
			new PresetInventoryItem("SkillBook", 117, 1, 30),
			new PresetInventoryItem("SkillBook", 108, 1, 30),
			new PresetInventoryItem("SkillBook", 126, 1, 20),
			new PresetInventoryItem("Food", 0, 3, 30),
			new PresetInventoryItem("Food", 93, 3, 30),
			new PresetInventoryItem("Food", 9, 3, 30),
			new PresetInventoryItem("Food", 51, 3, 30),
			new PresetInventoryItem("Medicine", 54, 1, 20),
			new PresetInventoryItem("Medicine", 66, 1, 20),
			new PresetInventoryItem("Medicine", 250, 1, 20),
			new PresetInventoryItem("Medicine", 238, 1, 20),
			new PresetInventoryItem("Medicine", 226, 1, 20),
			new PresetInventoryItem("Medicine", 322, 1, 20),
			new PresetInventoryItem("Medicine", 298, 1, 20),
			new PresetInventoryItem("Medicine", 274, 1, 20),
			new PresetInventoryItem("TeaWine", 18, 1, 20),
			new PresetInventoryItem("TeaWine", 27, 1, 20)
		};
		List<PresetOrgMemberCombatSkill> combatSkills = new List<PresetOrgMemberCombatSkill>
		{
			new PresetOrgMemberCombatSkill(9, 3),
			new PresetOrgMemberCombatSkill(112, 3),
			new PresetOrgMemberCombatSkill(210, 3),
			new PresetOrgMemberCombatSkill(329, 3),
			new PresetOrgMemberCombatSkill(525, 3),
			new PresetOrgMemberCombatSkill(648, 3)
		};
		sbyte[] extraCombatSkillGrids = new sbyte[5] { 4, 4, 4, 4, 4 };
		short[] resourcesAdjust = new short[8] { -60, -60, -70, -70, -70, -70, -70, -60 };
		short[] lifeSkillsAdjust = new short[16]
		{
			-1, -1, -1, -1, 2, -1, -1, -1, -1, -1,
			-1, -1, 9, 9, 9, -1
		};
		short[] combatSkillsAdjust = new short[14]
		{
			12, 9, 12, 9, 12, -1, -1, 9, -1, -1,
			12, -1, -1, -1
		};
		short[] mainAttributesAdjust = new short[6] { -1, 12, 9, 2, 9, 9 };
		List<sbyte> identityInteractConfig = new List<sbyte>();
		dataArray.Add(new OrganizationMemberItem(60, 282, 2, 3, 8, 12, 4, restrictPrincipalAmount: false, -1, -1, -1, 1, 3, 7, 3, 50, 130, monasticTitleSuffixes, 1800, 6, 3000, 2250, 0, favoriteClothingIds, hatedClothingIds, spouseAnonymousTitles, canStroll: true, 231, initialAges, equipment, clothing, inventory, combatSkills, extraCombatSkillGrids, resourcesAdjust, 20000, 12000, 10, 70, 4500, lifeSkillsAdjust, 4, combatSkillsAdjust, mainAttributesAdjust, identityInteractConfig, 3, new ResourceInts(default(int), default(int), default(int), default(int), default(int), default(int), default(int), default(int)), new IntPair[18]
		{
			new IntPair(17, 1),
			new IntPair(8, 3),
			new IntPair(5, 18),
			new IntPair(6, 18),
			new IntPair(15, 1),
			new IntPair(16, 1),
			new IntPair(7, 0),
			new IntPair(14, 1),
			new IntPair(12, 18),
			new IntPair(4, 15),
			new IntPair(3, 15),
			new IntPair(13, 6),
			new IntPair(10, 3),
			new IntPair(2, 1),
			new IntPair(1, 6),
			new IntPair(11, 6),
			new IntPair(0, 12),
			new IntPair(9, 1)
		}, null));
		List<OrganizationMemberItem> dataArray2 = _dataArray;
		int[] monasticTitleSuffixes2 = new int[2] { 288, 289 };
		List<short> favoriteClothingIds2 = new List<short> { 21, 22, 23, 6, 12, 5, 14 };
		List<short> hatedClothingIds2 = new List<short> { 49, 50, 51 };
		int[] spouseAnonymousTitles2 = new int[2] { 290, 291 };
		short[] initialAges2 = new short[4] { 16, 18, 20, 22 };
		PresetEquipmentItemWithProb[] equipment2 = new PresetEquipmentItemWithProb[8]
		{
			new PresetEquipmentItemWithProb("Armor", 99, 75),
			new PresetEquipmentItemWithProb("Armor", 342, 100),
			new PresetEquipmentItemWithProb("Armor", 495, 75),
			new PresetEquipmentItemWithProb("Armor", 207, 75),
			new PresetEquipmentItemWithProb("Accessory", 117, 75),
			new PresetEquipmentItemWithProb("Accessory", 135, 50),
			new PresetEquipmentItemWithProb("Accessory", 216, 50),
			new PresetEquipmentItemWithProb("Carrier", 18, 75)
		};
		PresetEquipmentItem clothing2 = new PresetEquipmentItem("Clothing", 21);
		List<PresetInventoryItem> inventory2 = new List<PresetInventoryItem>
		{
			new PresetInventoryItem("SkillBook", 117, 1, 20),
			new PresetInventoryItem("SkillBook", 108, 1, 20),
			new PresetInventoryItem("SkillBook", 126, 1, 10),
			new PresetInventoryItem("Food", 0, 3, 20),
			new PresetInventoryItem("Food", 93, 3, 20),
			new PresetInventoryItem("Food", 9, 3, 20),
			new PresetInventoryItem("Food", 51, 3, 20),
			new PresetInventoryItem("Medicine", 250, 1, 10),
			new PresetInventoryItem("Medicine", 238, 1, 10),
			new PresetInventoryItem("Medicine", 226, 1, 10),
			new PresetInventoryItem("Medicine", 322, 1, 10),
			new PresetInventoryItem("Medicine", 298, 1, 10),
			new PresetInventoryItem("Medicine", 274, 1, 10),
			new PresetInventoryItem("TeaWine", 18, 1, 10),
			new PresetInventoryItem("TeaWine", 27, 1, 10)
		};
		List<PresetOrgMemberCombatSkill> combatSkills2 = new List<PresetOrgMemberCombatSkill>
		{
			new PresetOrgMemberCombatSkill(9, 2),
			new PresetOrgMemberCombatSkill(112, 2),
			new PresetOrgMemberCombatSkill(210, 2),
			new PresetOrgMemberCombatSkill(406, 2),
			new PresetOrgMemberCombatSkill(525, 2),
			new PresetOrgMemberCombatSkill(648, 2)
		};
		sbyte[] extraCombatSkillGrids2 = new sbyte[5] { 2, 2, 2, 2, 2 };
		short[] resourcesAdjust2 = new short[8] { -60, -60, -70, -70, -70, -70, -70, -60 };
		short[] lifeSkillsAdjust2 = new short[16]
		{
			-1, -1, -1, -1, 2, -1, -1, -1, -1, -1,
			-1, -1, 9, 9, 9, -1
		};
		short[] combatSkillsAdjust2 = new short[14]
		{
			12, 9, 12, 9, 12, -1, -1, 9, -1, -1,
			12, -1, -1, -1
		};
		short[] mainAttributesAdjust2 = new short[6] { -1, 12, 9, 2, 9, 9 };
		identityInteractConfig = new List<sbyte>();
		dataArray2.Add(new OrganizationMemberItem(61, 287, 2, 2, 8, 12, 4, restrictPrincipalAmount: false, -1, -1, -1, 0, 2, 6, 2, 50, 130, monasticTitleSuffixes2, 600, 4, 2000, 900, 0, favoriteClothingIds2, hatedClothingIds2, spouseAnonymousTitles2, canStroll: true, 232, initialAges2, equipment2, clothing2, inventory2, combatSkills2, extraCombatSkillGrids2, resourcesAdjust2, 15000, 4000, 5, 80, 1800, lifeSkillsAdjust2, 3, combatSkillsAdjust2, mainAttributesAdjust2, identityInteractConfig, 3, new ResourceInts(default(int), default(int), default(int), default(int), default(int), default(int), default(int), default(int)), new IntPair[18]
		{
			new IntPair(17, 1),
			new IntPair(8, 3),
			new IntPair(5, 12),
			new IntPair(6, 12),
			new IntPair(15, 1),
			new IntPair(16, 1),
			new IntPair(7, 0),
			new IntPair(14, 1),
			new IntPair(12, 12),
			new IntPair(4, 15),
			new IntPair(3, 15),
			new IntPair(13, 3),
			new IntPair(10, 3),
			new IntPair(2, 1),
			new IntPair(1, 6),
			new IntPair(11, 6),
			new IntPair(0, 15),
			new IntPair(9, 1)
		}, null));
		List<OrganizationMemberItem> dataArray3 = _dataArray;
		int[] monasticTitleSuffixes3 = new int[2] { 293, 294 };
		List<short> favoriteClothingIds3 = new List<short> { 21, 22, 23, 6, 12, 5, 14 };
		List<short> hatedClothingIds3 = new List<short> { 49, 50, 51 };
		int[] spouseAnonymousTitles3 = new int[2] { 295, 296 };
		short[] initialAges3 = new short[4] { 13, 14, 15, 16 };
		PresetEquipmentItemWithProb[] equipment3 = new PresetEquipmentItemWithProb[8]
		{
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Armor", 342, 100),
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Armor", 207, 75),
			new PresetEquipmentItemWithProb("Accessory", 117, 75),
			new PresetEquipmentItemWithProb("Accessory", 135, 50),
			new PresetEquipmentItemWithProb("Accessory", -1, 0),
			new PresetEquipmentItemWithProb("Carrier", -1, 0)
		};
		PresetEquipmentItem clothing3 = new PresetEquipmentItem("Clothing", 21);
		List<PresetInventoryItem> inventory3 = new List<PresetInventoryItem>
		{
			new PresetInventoryItem("SkillBook", 117, 1, 20),
			new PresetInventoryItem("SkillBook", 108, 1, 20),
			new PresetInventoryItem("SkillBook", 126, 1, 10),
			new PresetInventoryItem("Food", 0, 3, 20),
			new PresetInventoryItem("Food", 93, 3, 20),
			new PresetInventoryItem("Food", 9, 3, 20),
			new PresetInventoryItem("Food", 51, 3, 20),
			new PresetInventoryItem("Medicine", 250, 1, 10),
			new PresetInventoryItem("Medicine", 238, 1, 10),
			new PresetInventoryItem("Medicine", 226, 1, 10),
			new PresetInventoryItem("Medicine", 322, 1, 10),
			new PresetInventoryItem("Medicine", 298, 1, 10),
			new PresetInventoryItem("Medicine", 274, 1, 10),
			new PresetInventoryItem("TeaWine", 18, 1, 10),
			new PresetInventoryItem("TeaWine", 27, 1, 10)
		};
		List<PresetOrgMemberCombatSkill> combatSkills3 = new List<PresetOrgMemberCombatSkill>
		{
			new PresetOrgMemberCombatSkill(9, 1),
			new PresetOrgMemberCombatSkill(112, 1),
			new PresetOrgMemberCombatSkill(210, 1),
			new PresetOrgMemberCombatSkill(329, 1),
			new PresetOrgMemberCombatSkill(525, 1)
		};
		sbyte[] extraCombatSkillGrids3 = new sbyte[5];
		short[] resourcesAdjust3 = new short[8] { -60, -60, -70, -70, -70, -70, -70, -60 };
		short[] lifeSkillsAdjust3 = new short[16]
		{
			-1, -1, -1, -1, 2, -1, -1, -1, -1, -1,
			-1, -1, 9, 9, 9, -1
		};
		short[] combatSkillsAdjust3 = new short[14]
		{
			12, 9, 12, 9, 12, -1, -1, 9, -1, -1,
			12, -1, -1, -1
		};
		short[] mainAttributesAdjust3 = new short[6] { -1, 12, 9, 2, 9, 9 };
		identityInteractConfig = new List<sbyte>();
		dataArray3.Add(new OrganizationMemberItem(62, 292, 2, 1, 8, 12, 4, restrictPrincipalAmount: false, -1, -1, -1, 0, 1, 5, 1, 50, 130, monasticTitleSuffixes3, 300, 2, 1000, 300, 0, favoriteClothingIds3, hatedClothingIds3, spouseAnonymousTitles3, canStroll: true, 233, initialAges3, equipment3, clothing3, inventory3, combatSkills3, extraCombatSkillGrids3, resourcesAdjust3, 10000, 2000, 5, 90, 600, lifeSkillsAdjust3, 2, combatSkillsAdjust3, mainAttributesAdjust3, identityInteractConfig, 3, new ResourceInts(default(int), default(int), default(int), default(int), default(int), default(int), default(int), default(int)), new IntPair[18]
		{
			new IntPair(17, 1),
			new IntPair(8, 3),
			new IntPair(5, 12),
			new IntPair(6, 12),
			new IntPair(15, 1),
			new IntPair(16, 1),
			new IntPair(7, 0),
			new IntPair(14, 1),
			new IntPair(12, 12),
			new IntPair(4, 15),
			new IntPair(3, 15),
			new IntPair(13, 3),
			new IntPair(10, 3),
			new IntPair(2, 1),
			new IntPair(1, 6),
			new IntPair(11, 6),
			new IntPair(0, 15),
			new IntPair(9, 1)
		}, null));
		List<OrganizationMemberItem> dataArray4 = _dataArray;
		int[] monasticTitleSuffixes4 = new int[2] { 298, 299 };
		List<short> favoriteClothingIds4 = new List<short> { 21, 22, 23, 6, 12, 5, 14 };
		List<short> hatedClothingIds4 = new List<short> { 49, 50, 51 };
		int[] spouseAnonymousTitles4 = new int[2] { 300, 301 };
		short[] initialAges4 = new short[4] { 10, 10, 10, 10 };
		PresetEquipmentItemWithProb[] equipment4 = new PresetEquipmentItemWithProb[8]
		{
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Armor", 342, 100),
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Armor", 207, 75),
			new PresetEquipmentItemWithProb("Accessory", 117, 75),
			new PresetEquipmentItemWithProb("Accessory", -1, 0),
			new PresetEquipmentItemWithProb("Accessory", -1, 0),
			new PresetEquipmentItemWithProb("Carrier", -1, 0)
		};
		PresetEquipmentItem clothing4 = new PresetEquipmentItem("Clothing", 21);
		List<PresetInventoryItem> inventory4 = new List<PresetInventoryItem>
		{
			new PresetInventoryItem("SkillBook", 117, 1, 20),
			new PresetInventoryItem("SkillBook", 108, 1, 20),
			new PresetInventoryItem("SkillBook", 126, 1, 10),
			new PresetInventoryItem("Food", 0, 3, 20),
			new PresetInventoryItem("Food", 93, 3, 20),
			new PresetInventoryItem("Food", 9, 3, 20),
			new PresetInventoryItem("Food", 51, 3, 20),
			new PresetInventoryItem("Medicine", 250, 1, 10),
			new PresetInventoryItem("Medicine", 238, 1, 10),
			new PresetInventoryItem("Medicine", 226, 1, 10),
			new PresetInventoryItem("Medicine", 322, 1, 10),
			new PresetInventoryItem("Medicine", 298, 1, 10),
			new PresetInventoryItem("Medicine", 274, 1, 10),
			new PresetInventoryItem("TeaWine", 18, 1, 10),
			new PresetInventoryItem("TeaWine", 27, 1, 10)
		};
		List<PresetOrgMemberCombatSkill> combatSkills4 = new List<PresetOrgMemberCombatSkill>
		{
			new PresetOrgMemberCombatSkill(9, 0),
			new PresetOrgMemberCombatSkill(112, 0),
			new PresetOrgMemberCombatSkill(525, 0)
		};
		sbyte[] extraCombatSkillGrids4 = new sbyte[5];
		short[] resourcesAdjust4 = new short[8] { -60, -60, -70, -70, -70, -70, -70, -60 };
		short[] lifeSkillsAdjust4 = new short[16]
		{
			-1, -1, -1, -1, 2, -1, -1, -1, -1, -1,
			-1, -1, 9, 9, 9, -1
		};
		short[] combatSkillsAdjust4 = new short[14]
		{
			12, 9, 12, 9, 12, -1, -1, 9, -1, -1,
			12, -1, -1, -1
		};
		short[] mainAttributesAdjust4 = new short[6] { -1, 12, 9, 2, 9, 9 };
		identityInteractConfig = new List<sbyte>();
		dataArray4.Add(new OrganizationMemberItem(63, 297, 2, 0, 8, 12, 4, restrictPrincipalAmount: false, -1, -1, -1, 0, 0, 4, 0, 50, 130, monasticTitleSuffixes4, 150, 0, 500, 150, 0, favoriteClothingIds4, hatedClothingIds4, spouseAnonymousTitles4, canStroll: true, 234, initialAges4, equipment4, clothing4, inventory4, combatSkills4, extraCombatSkillGrids4, resourcesAdjust4, 5000, 1000, 1, 100, 300, lifeSkillsAdjust4, 1, combatSkillsAdjust4, mainAttributesAdjust4, identityInteractConfig, 3, new ResourceInts(default(int), default(int), default(int), default(int), default(int), default(int), default(int), default(int)), new IntPair[18]
		{
			new IntPair(17, 1),
			new IntPair(8, 3),
			new IntPair(5, 12),
			new IntPair(6, 12),
			new IntPair(15, 1),
			new IntPair(16, 1),
			new IntPair(7, 0),
			new IntPair(14, 1),
			new IntPair(12, 12),
			new IntPair(4, 15),
			new IntPair(3, 15),
			new IntPair(13, 3),
			new IntPair(10, 3),
			new IntPair(2, 1),
			new IntPair(1, 6),
			new IntPair(11, 6),
			new IntPair(0, 15),
			new IntPair(9, 1)
		}, null));
		_dataArray.Add(new OrganizationMemberItem(64, 302, 3, 8, 1, 1, 1, restrictPrincipalAmount: true, -1, -1, 7, 4, 7, -1, 7, 0, 0, new int[2] { 303, 304 }, 15800, 16, 13000, 30750, 0, new List<short> { 24, 25, 26, 13 }, new List<short> { 61, 62, 63 }, new int[2] { 305, 306 }, canStroll: false, 235, new short[4] { 26, 34, 42, 50 }, new PresetEquipmentItemWithProb[8]
		{
			new PresetEquipmentItemWithProb("Armor", 81, 75),
			new PresetEquipmentItemWithProb("Armor", 351, 100),
			new PresetEquipmentItemWithProb("Armor", 486, 75),
			new PresetEquipmentItemWithProb("Armor", 225, 75),
			new PresetEquipmentItemWithProb("Accessory", 162, 75),
			new PresetEquipmentItemWithProb("Accessory", 144, 50),
			new PresetEquipmentItemWithProb("Accessory", 81, 50),
			new PresetEquipmentItemWithProb("Carrier", 9, 75)
		}, new PresetEquipmentItem("Clothing", 26), new List<PresetInventoryItem>
		{
			new PresetInventoryItem("SkillBook", 72, 3, 40),
			new PresetInventoryItem("SkillBook", 27, 1, 30),
			new PresetInventoryItem("SkillBook", 0, 1, 30),
			new PresetInventoryItem("Material", 42, 1, 30),
			new PresetInventoryItem("Material", 49, 1, 30),
			new PresetInventoryItem("CraftTool", 45, 1, 30),
			new PresetInventoryItem("Medicine", 9, 1, 30),
			new PresetInventoryItem("Medicine", 18, 1, 30),
			new PresetInventoryItem("Medicine", 45, 1, 30),
			new PresetInventoryItem("Food", 0, 3, 40),
			new PresetInventoryItem("Food", 93, 3, 40),
			new PresetInventoryItem("Food", 135, 3, 40),
			new PresetInventoryItem("Medicine", 60, 1, 20),
			new PresetInventoryItem("Medicine", 72, 1, 20),
			new PresetInventoryItem("Medicine", 88, 1, 20),
			new PresetInventoryItem("Medicine", 100, 1, 20),
			new PresetInventoryItem("Medicine", 136, 1, 20),
			new PresetInventoryItem("Medicine", 148, 1, 20),
			new PresetInventoryItem("Medicine", 160, 1, 20),
			new PresetInventoryItem("Medicine", 172, 1, 20),
			new PresetInventoryItem("Medicine", 184, 1, 20),
			new PresetInventoryItem("Medicine", 196, 1, 20),
			new PresetInventoryItem("Medicine", 244, 1, 20),
			new PresetInventoryItem("Medicine", 304, 1, 20),
			new PresetInventoryItem("TeaWine", 27, 1, 20),
			new PresetInventoryItem("TeaWine", 9, 1, 20),
			new PresetInventoryItem("Material", 140, 1, 10),
			new PresetInventoryItem("Material", 144, 1, 10),
			new PresetInventoryItem("Material", 148, 1, 10),
			new PresetInventoryItem("Material", 152, 1, 10),
			new PresetInventoryItem("Material", 156, 1, 10),
			new PresetInventoryItem("Material", 160, 1, 10),
			new PresetInventoryItem("Material", 164, 1, 10),
			new PresetInventoryItem("Material", 168, 1, 10),
			new PresetInventoryItem("Material", 172, 1, 10),
			new PresetInventoryItem("Material", 176, 1, 10),
			new PresetInventoryItem("Material", 180, 1, 10),
			new PresetInventoryItem("Material", 184, 1, 10),
			new PresetInventoryItem("Material", 188, 1, 10),
			new PresetInventoryItem("Material", 192, 1, 10),
			new PresetInventoryItem("Material", 196, 1, 10),
			new PresetInventoryItem("Material", 200, 1, 10),
			new PresetInventoryItem("Material", 204, 1, 10),
			new PresetInventoryItem("Material", 208, 1, 10),
			new PresetInventoryItem("Material", 212, 1, 10),
			new PresetInventoryItem("Material", 216, 1, 10),
			new PresetInventoryItem("Material", 220, 1, 10),
			new PresetInventoryItem("Material", 224, 1, 10),
			new PresetInventoryItem("Material", 228, 1, 10),
			new PresetInventoryItem("Material", 232, 1, 10)
		}, new List<PresetOrgMemberCombatSkill>
		{
			new PresetOrgMemberCombatSkill(17, 4),
			new PresetOrgMemberCombatSkill(119, 7),
			new PresetOrgMemberCombatSkill(219, 8),
			new PresetOrgMemberCombatSkill(414, 8),
			new PresetOrgMemberCombatSkill(690, 8),
			new PresetOrgMemberCombatSkill(707, 6)
		}, new sbyte[5] { 10, 10, 10, 10, 10 }, new short[8] { -70, -60, -80, -70, -70, -60, -70, -70 }, 120000, 432000, 20, 20, 61500, new short[16]
		{
			9, -1, -1, 12, 2, -1, 2, 2, 12, 9,
			12, -1, -1, -1, 2, 2
		}, 8, new short[14]
		{
			6, 12, 12, -1, 12, 2, -1, -1, 2, -1,
			-1, -1, 12, 9
		}, new short[6] { 2, 12, 9, -1, -1, 12 }, new List<sbyte> { 5, 11, 24, 25, 55, 64 }, 3, new ResourceInts(default(int), default(int), default(int), default(int), default(int), default(int), default(int), default(int)), new IntPair[18]
		{
			new IntPair(17, 1),
			new IntPair(8, 15),
			new IntPair(5, 1),
			new IntPair(6, 1),
			new IntPair(15, 1),
			new IntPair(16, 1),
			new IntPair(7, 1),
			new IntPair(14, 1),
			new IntPair(12, 1),
			new IntPair(4, 27),
			new IntPair(3, 1),
			new IntPair(13, 54),
			new IntPair(10, 18),
			new IntPair(2, 1),
			new IntPair(1, 1),
			new IntPair(11, 12),
			new IntPair(0, 6),
			new IntPair(9, 1)
		}, null));
		_dataArray.Add(new OrganizationMemberItem(65, 307, 3, 7, 4, 6, 2, restrictPrincipalAmount: false, -1, -1, -1, 3, 7, -1, 7, 0, 0, new int[2] { 308, 309 }, 8600, 14, 10000, 21150, 0, new List<short> { 24, 25, 26, 13 }, new List<short> { 61, 62, 63 }, new int[2] { 310, 311 }, canStroll: false, 236, new short[4] { 24, 31, 38, 45 }, new PresetEquipmentItemWithProb[8]
		{
			new PresetEquipmentItemWithProb("Armor", 81, 75),
			new PresetEquipmentItemWithProb("Armor", 351, 100),
			new PresetEquipmentItemWithProb("Armor", 486, 75),
			new PresetEquipmentItemWithProb("Armor", 225, 75),
			new PresetEquipmentItemWithProb("Accessory", 162, 75),
			new PresetEquipmentItemWithProb("Accessory", 144, 50),
			new PresetEquipmentItemWithProb("Accessory", 81, 50),
			new PresetEquipmentItemWithProb("Carrier", 9, 75)
		}, new PresetEquipmentItem("Clothing", 25), new List<PresetInventoryItem>
		{
			new PresetInventoryItem("SkillBook", 72, 3, 40),
			new PresetInventoryItem("SkillBook", 27, 1, 30),
			new PresetInventoryItem("SkillBook", 0, 1, 30),
			new PresetInventoryItem("Material", 42, 1, 30),
			new PresetInventoryItem("Material", 49, 1, 30),
			new PresetInventoryItem("CraftTool", 45, 1, 30),
			new PresetInventoryItem("Medicine", 9, 1, 30),
			new PresetInventoryItem("Medicine", 18, 1, 30),
			new PresetInventoryItem("Medicine", 45, 1, 30),
			new PresetInventoryItem("Food", 0, 3, 40),
			new PresetInventoryItem("Food", 93, 3, 40),
			new PresetInventoryItem("Food", 135, 3, 40),
			new PresetInventoryItem("Medicine", 60, 1, 20),
			new PresetInventoryItem("Medicine", 72, 1, 20),
			new PresetInventoryItem("Medicine", 88, 1, 20),
			new PresetInventoryItem("Medicine", 100, 1, 20),
			new PresetInventoryItem("Medicine", 136, 1, 20),
			new PresetInventoryItem("Medicine", 148, 1, 20),
			new PresetInventoryItem("Medicine", 160, 1, 20),
			new PresetInventoryItem("Medicine", 172, 1, 20),
			new PresetInventoryItem("Medicine", 184, 1, 20),
			new PresetInventoryItem("Medicine", 196, 1, 20),
			new PresetInventoryItem("Medicine", 244, 1, 20),
			new PresetInventoryItem("Medicine", 304, 1, 20),
			new PresetInventoryItem("TeaWine", 27, 1, 20),
			new PresetInventoryItem("TeaWine", 9, 1, 20),
			new PresetInventoryItem("Material", 140, 1, 10),
			new PresetInventoryItem("Material", 144, 1, 10),
			new PresetInventoryItem("Material", 148, 1, 10),
			new PresetInventoryItem("Material", 152, 1, 10),
			new PresetInventoryItem("Material", 156, 1, 10),
			new PresetInventoryItem("Material", 160, 1, 10),
			new PresetInventoryItem("Material", 164, 1, 10),
			new PresetInventoryItem("Material", 168, 1, 10),
			new PresetInventoryItem("Material", 172, 1, 10),
			new PresetInventoryItem("Material", 176, 1, 10),
			new PresetInventoryItem("Material", 180, 1, 10),
			new PresetInventoryItem("Material", 184, 1, 10),
			new PresetInventoryItem("Material", 188, 1, 10),
			new PresetInventoryItem("Material", 192, 1, 10),
			new PresetInventoryItem("Material", 196, 1, 10),
			new PresetInventoryItem("Material", 200, 1, 10),
			new PresetInventoryItem("Material", 204, 1, 10),
			new PresetInventoryItem("Material", 208, 1, 10),
			new PresetInventoryItem("Material", 212, 1, 10),
			new PresetInventoryItem("Material", 216, 1, 10),
			new PresetInventoryItem("Material", 220, 1, 10),
			new PresetInventoryItem("Material", 224, 1, 10),
			new PresetInventoryItem("Material", 228, 1, 10),
			new PresetInventoryItem("Material", 232, 1, 10)
		}, new List<PresetOrgMemberCombatSkill>
		{
			new PresetOrgMemberCombatSkill(17, 4),
			new PresetOrgMemberCombatSkill(119, 6),
			new PresetOrgMemberCombatSkill(219, 7),
			new PresetOrgMemberCombatSkill(414, 7),
			new PresetOrgMemberCombatSkill(690, 7),
			new PresetOrgMemberCombatSkill(707, 5)
		}, new sbyte[5] { 8, 8, 8, 8, 8 }, new short[8] { -70, -60, -80, -70, -70, -60, -70, -70 }, 75000, 216000, 15, 30, 42300, new short[16]
		{
			9, -1, -1, 12, 2, -1, 2, 2, 12, 9,
			12, -1, -1, -1, 2, 2
		}, 8, new short[14]
		{
			6, 12, 12, -1, 12, 2, -1, -1, 2, -1,
			-1, -1, 12, 9
		}, new short[6] { 2, 12, 9, -1, -1, 12 }, new List<sbyte> { 5, 11, 24, 25, 64 }, 3, new ResourceInts(default(int), default(int), default(int), default(int), default(int), default(int), default(int), default(int)), new IntPair[18]
		{
			new IntPair(17, 1),
			new IntPair(8, 15),
			new IntPair(5, 1),
			new IntPair(6, 1),
			new IntPair(15, 1),
			new IntPair(16, 1),
			new IntPair(7, 1),
			new IntPair(14, 1),
			new IntPair(12, 1),
			new IntPair(4, 27),
			new IntPair(3, 1),
			new IntPair(13, 54),
			new IntPair(10, 18),
			new IntPair(2, 1),
			new IntPair(1, 1),
			new IntPair(11, 12),
			new IntPair(0, 6),
			new IntPair(9, 1)
		}, null));
		_dataArray.Add(new OrganizationMemberItem(66, 312, 3, 6, 4, 6, 2, restrictPrincipalAmount: false, -1, -1, -1, 2, 6, 8, 6, 0, 0, new int[2] { 313, 314 }, 6100, 12, 8000, 13800, 0, new List<short> { 24, 25, 26, 13 }, new List<short> { 61, 62, 63 }, new int[2] { 315, 316 }, canStroll: true, 237, new short[4] { 22, 28, 34, 40 }, new PresetEquipmentItemWithProb[8]
		{
			new PresetEquipmentItemWithProb("Armor", 81, 75),
			new PresetEquipmentItemWithProb("Armor", 351, 100),
			new PresetEquipmentItemWithProb("Armor", 486, 75),
			new PresetEquipmentItemWithProb("Armor", 225, 75),
			new PresetEquipmentItemWithProb("Accessory", 162, 75),
			new PresetEquipmentItemWithProb("Accessory", 144, 50),
			new PresetEquipmentItemWithProb("Accessory", 81, 50),
			new PresetEquipmentItemWithProb("Carrier", 9, 75)
		}, new PresetEquipmentItem("Clothing", 25), new List<PresetInventoryItem>
		{
			new PresetInventoryItem("SkillBook", 72, 3, 40),
			new PresetInventoryItem("SkillBook", 27, 1, 30),
			new PresetInventoryItem("SkillBook", 0, 1, 30),
			new PresetInventoryItem("Material", 42, 1, 30),
			new PresetInventoryItem("Material", 49, 1, 30),
			new PresetInventoryItem("CraftTool", 45, 1, 30),
			new PresetInventoryItem("Medicine", 9, 1, 30),
			new PresetInventoryItem("Medicine", 18, 1, 30),
			new PresetInventoryItem("Medicine", 45, 1, 30),
			new PresetInventoryItem("Food", 0, 3, 40),
			new PresetInventoryItem("Food", 93, 3, 40),
			new PresetInventoryItem("Food", 135, 3, 40),
			new PresetInventoryItem("Medicine", 60, 1, 20),
			new PresetInventoryItem("Medicine", 72, 1, 20),
			new PresetInventoryItem("Medicine", 88, 1, 20),
			new PresetInventoryItem("Medicine", 100, 1, 20),
			new PresetInventoryItem("Medicine", 136, 1, 20),
			new PresetInventoryItem("Medicine", 148, 1, 20),
			new PresetInventoryItem("Medicine", 160, 1, 20),
			new PresetInventoryItem("Medicine", 172, 1, 20),
			new PresetInventoryItem("Medicine", 184, 1, 20),
			new PresetInventoryItem("Medicine", 196, 1, 20),
			new PresetInventoryItem("Medicine", 244, 1, 20),
			new PresetInventoryItem("Medicine", 304, 1, 20),
			new PresetInventoryItem("TeaWine", 27, 1, 20),
			new PresetInventoryItem("TeaWine", 9, 1, 20),
			new PresetInventoryItem("Material", 140, 1, 10),
			new PresetInventoryItem("Material", 144, 1, 10),
			new PresetInventoryItem("Material", 148, 1, 10),
			new PresetInventoryItem("Material", 152, 1, 10),
			new PresetInventoryItem("Material", 156, 1, 10),
			new PresetInventoryItem("Material", 160, 1, 10),
			new PresetInventoryItem("Material", 164, 1, 10),
			new PresetInventoryItem("Material", 168, 1, 10),
			new PresetInventoryItem("Material", 172, 1, 10),
			new PresetInventoryItem("Material", 176, 1, 10),
			new PresetInventoryItem("Material", 180, 1, 10),
			new PresetInventoryItem("Material", 184, 1, 10),
			new PresetInventoryItem("Material", 188, 1, 10),
			new PresetInventoryItem("Material", 192, 1, 10),
			new PresetInventoryItem("Material", 196, 1, 10),
			new PresetInventoryItem("Material", 200, 1, 10),
			new PresetInventoryItem("Material", 204, 1, 10),
			new PresetInventoryItem("Material", 208, 1, 10),
			new PresetInventoryItem("Material", 212, 1, 10),
			new PresetInventoryItem("Material", 216, 1, 10),
			new PresetInventoryItem("Material", 220, 1, 10),
			new PresetInventoryItem("Material", 224, 1, 10),
			new PresetInventoryItem("Material", 228, 1, 10),
			new PresetInventoryItem("Material", 232, 1, 10)
		}, new List<PresetOrgMemberCombatSkill>
		{
			new PresetOrgMemberCombatSkill(17, 4),
			new PresetOrgMemberCombatSkill(119, 6),
			new PresetOrgMemberCombatSkill(219, 6),
			new PresetOrgMemberCombatSkill(414, 6),
			new PresetOrgMemberCombatSkill(690, 6),
			new PresetOrgMemberCombatSkill(707, 5)
		}, new sbyte[5] { 8, 8, 8, 8, 8 }, new short[8] { -70, -60, -80, -70, -70, -60, -70, -70 }, 45000, 108000, 15, 40, 27600, new short[16]
		{
			9, -1, -1, 12, 2, -1, 2, 2, 12, 9,
			12, -1, -1, -1, 2, 2
		}, 7, new short[14]
		{
			6, 12, 12, -1, 12, 2, -1, -1, 2, -1,
			-1, -1, 12, 9
		}, new short[6] { 2, 12, 9, -1, -1, 12 }, new List<sbyte> { 5, 11, 24, 25, 64 }, 3, new ResourceInts(default(int), default(int), default(int), default(int), default(int), default(int), default(int), default(int)), new IntPair[18]
		{
			new IntPair(17, 1),
			new IntPair(8, 15),
			new IntPair(5, 1),
			new IntPair(6, 1),
			new IntPair(15, 1),
			new IntPair(16, 1),
			new IntPair(7, 1),
			new IntPair(14, 1),
			new IntPair(12, 1),
			new IntPair(4, 27),
			new IntPair(3, 1),
			new IntPair(13, 54),
			new IntPair(10, 18),
			new IntPair(2, 1),
			new IntPair(1, 1),
			new IntPair(11, 12),
			new IntPair(0, 6),
			new IntPair(9, 1)
		}, null));
		_dataArray.Add(new OrganizationMemberItem(67, 317, 3, 5, 4, 6, 2, restrictPrincipalAmount: false, -1, -1, -1, 1, 5, 8, 5, 0, 0, new int[2] { 318, 319 }, 4200, 10, 6000, 8400, 0, new List<short> { 24, 25, 26, 13 }, new List<short> { 61, 62, 63 }, new int[2] { 320, 321 }, canStroll: true, 238, new short[4] { 20, 25, 30, 35 }, new PresetEquipmentItemWithProb[8]
		{
			new PresetEquipmentItemWithProb("Armor", 81, 75),
			new PresetEquipmentItemWithProb("Armor", 351, 100),
			new PresetEquipmentItemWithProb("Armor", 486, 75),
			new PresetEquipmentItemWithProb("Armor", 225, 75),
			new PresetEquipmentItemWithProb("Accessory", 162, 75),
			new PresetEquipmentItemWithProb("Accessory", 144, 50),
			new PresetEquipmentItemWithProb("Accessory", 81, 50),
			new PresetEquipmentItemWithProb("Carrier", 9, 75)
		}, new PresetEquipmentItem("Clothing", 25), new List<PresetInventoryItem>
		{
			new PresetInventoryItem("SkillBook", 72, 3, 20),
			new PresetInventoryItem("SkillBook", 27, 1, 20),
			new PresetInventoryItem("SkillBook", 0, 1, 20),
			new PresetInventoryItem("Material", 42, 1, 20),
			new PresetInventoryItem("Material", 49, 1, 20),
			new PresetInventoryItem("CraftTool", 45, 1, 30),
			new PresetInventoryItem("Medicine", 9, 1, 20),
			new PresetInventoryItem("Medicine", 18, 1, 20),
			new PresetInventoryItem("Medicine", 45, 1, 20),
			new PresetInventoryItem("Food", 0, 3, 30),
			new PresetInventoryItem("Food", 93, 3, 30),
			new PresetInventoryItem("Food", 135, 3, 30),
			new PresetInventoryItem("Medicine", 60, 1, 20),
			new PresetInventoryItem("Medicine", 72, 1, 20),
			new PresetInventoryItem("Medicine", 88, 1, 20),
			new PresetInventoryItem("Medicine", 94, 1, 20),
			new PresetInventoryItem("Medicine", 136, 1, 20),
			new PresetInventoryItem("Medicine", 148, 1, 20),
			new PresetInventoryItem("Medicine", 160, 1, 20),
			new PresetInventoryItem("Medicine", 172, 1, 20),
			new PresetInventoryItem("Medicine", 184, 1, 20),
			new PresetInventoryItem("Medicine", 196, 1, 20),
			new PresetInventoryItem("Medicine", 238, 1, 10),
			new PresetInventoryItem("Medicine", 298, 1, 10),
			new PresetInventoryItem("TeaWine", 27, 1, 20),
			new PresetInventoryItem("TeaWine", 9, 1, 20),
			new PresetInventoryItem("Material", 140, 1, 10),
			new PresetInventoryItem("Material", 144, 1, 10),
			new PresetInventoryItem("Material", 148, 1, 10),
			new PresetInventoryItem("Material", 152, 1, 10),
			new PresetInventoryItem("Material", 156, 1, 10),
			new PresetInventoryItem("Material", 160, 1, 10),
			new PresetInventoryItem("Material", 164, 1, 10),
			new PresetInventoryItem("Material", 168, 1, 10),
			new PresetInventoryItem("Material", 172, 1, 10),
			new PresetInventoryItem("Material", 176, 1, 10),
			new PresetInventoryItem("Material", 180, 1, 10),
			new PresetInventoryItem("Material", 184, 1, 10),
			new PresetInventoryItem("Material", 188, 1, 10),
			new PresetInventoryItem("Material", 192, 1, 10),
			new PresetInventoryItem("Material", 196, 1, 10),
			new PresetInventoryItem("Material", 200, 1, 10),
			new PresetInventoryItem("Material", 204, 1, 10),
			new PresetInventoryItem("Material", 208, 1, 10),
			new PresetInventoryItem("Material", 212, 1, 10),
			new PresetInventoryItem("Material", 216, 1, 10),
			new PresetInventoryItem("Material", 220, 1, 10),
			new PresetInventoryItem("Material", 224, 1, 10),
			new PresetInventoryItem("Material", 228, 1, 10),
			new PresetInventoryItem("Material", 232, 1, 10)
		}, new List<PresetOrgMemberCombatSkill>
		{
			new PresetOrgMemberCombatSkill(17, 3),
			new PresetOrgMemberCombatSkill(119, 5),
			new PresetOrgMemberCombatSkill(219, 5),
			new PresetOrgMemberCombatSkill(414, 5),
			new PresetOrgMemberCombatSkill(690, 5),
			new PresetOrgMemberCombatSkill(707, 4)
		}, new sbyte[5] { 6, 6, 6, 6, 6 }, new short[8] { -70, -60, -80, -70, -70, -60, -70, -70 }, 30000, 36000, 10, 50, 16800, new short[16]
		{
			9, -1, -1, 12, 2, -1, 2, 2, 12, 9,
			12, -1, -1, -1, 2, 2
		}, 6, new short[14]
		{
			6, 12, 12, -1, 12, 2, -1, -1, 2, -1,
			-1, -1, 12, 9
		}, new short[6] { 2, 12, 9, -1, -1, 12 }, new List<sbyte> { 5, 64 }, 3, new ResourceInts(default(int), default(int), default(int), default(int), default(int), default(int), default(int), default(int)), new IntPair[18]
		{
			new IntPair(17, 1),
			new IntPair(8, 12),
			new IntPair(5, 1),
			new IntPair(6, 1),
			new IntPair(15, 1),
			new IntPair(16, 1),
			new IntPair(7, 1),
			new IntPair(14, 1),
			new IntPair(12, 1),
			new IntPair(4, 24),
			new IntPair(3, 1),
			new IntPair(13, 42),
			new IntPair(10, 18),
			new IntPair(2, 1),
			new IntPair(1, 1),
			new IntPair(11, 12),
			new IntPair(0, 6),
			new IntPair(9, 1)
		}, null));
		_dataArray.Add(new OrganizationMemberItem(68, 322, 3, 4, 6, 9, 3, restrictPrincipalAmount: false, -1, -1, -1, 0, 4, 7, 4, 0, 0, new int[2] { 323, 324 }, 2800, 8, 4500, 4650, 0, new List<short> { 24, 25, 26, 13 }, new List<short> { 61, 62, 63 }, new int[2] { 325, 326 }, canStroll: true, 239, new short[4] { 18, 22, 26, 30 }, new PresetEquipmentItemWithProb[8]
		{
			new PresetEquipmentItemWithProb("Armor", 81, 75),
			new PresetEquipmentItemWithProb("Armor", 351, 100),
			new PresetEquipmentItemWithProb("Armor", 486, 75),
			new PresetEquipmentItemWithProb("Armor", 225, 75),
			new PresetEquipmentItemWithProb("Accessory", 162, 75),
			new PresetEquipmentItemWithProb("Accessory", 144, 50),
			new PresetEquipmentItemWithProb("Accessory", 81, 50),
			new PresetEquipmentItemWithProb("Carrier", 9, 75)
		}, new PresetEquipmentItem("Clothing", 24), new List<PresetInventoryItem>
		{
			new PresetInventoryItem("SkillBook", 72, 3, 20),
			new PresetInventoryItem("SkillBook", 27, 1, 20),
			new PresetInventoryItem("SkillBook", 0, 1, 20),
			new PresetInventoryItem("Material", 42, 1, 20),
			new PresetInventoryItem("Material", 49, 1, 20),
			new PresetInventoryItem("CraftTool", 45, 1, 30),
			new PresetInventoryItem("Medicine", 9, 1, 20),
			new PresetInventoryItem("Medicine", 18, 1, 20),
			new PresetInventoryItem("Medicine", 45, 1, 20),
			new PresetInventoryItem("Food", 0, 3, 30),
			new PresetInventoryItem("Food", 93, 3, 30),
			new PresetInventoryItem("Food", 135, 3, 30),
			new PresetInventoryItem("Medicine", 60, 1, 20),
			new PresetInventoryItem("Medicine", 72, 1, 20),
			new PresetInventoryItem("Medicine", 88, 1, 20),
			new PresetInventoryItem("Medicine", 94, 1, 20),
			new PresetInventoryItem("Medicine", 136, 1, 20),
			new PresetInventoryItem("Medicine", 148, 1, 20),
			new PresetInventoryItem("Medicine", 160, 1, 20),
			new PresetInventoryItem("Medicine", 172, 1, 20),
			new PresetInventoryItem("Medicine", 184, 1, 20),
			new PresetInventoryItem("Medicine", 196, 1, 20),
			new PresetInventoryItem("Medicine", 238, 1, 10),
			new PresetInventoryItem("Medicine", 298, 1, 10),
			new PresetInventoryItem("TeaWine", 27, 1, 20),
			new PresetInventoryItem("TeaWine", 9, 1, 20),
			new PresetInventoryItem("Material", 140, 1, 10),
			new PresetInventoryItem("Material", 144, 1, 10),
			new PresetInventoryItem("Material", 148, 1, 10),
			new PresetInventoryItem("Material", 152, 1, 10),
			new PresetInventoryItem("Material", 156, 1, 10),
			new PresetInventoryItem("Material", 160, 1, 10),
			new PresetInventoryItem("Material", 164, 1, 10),
			new PresetInventoryItem("Material", 168, 1, 10),
			new PresetInventoryItem("Material", 172, 1, 10),
			new PresetInventoryItem("Material", 176, 1, 10),
			new PresetInventoryItem("Material", 180, 1, 10),
			new PresetInventoryItem("Material", 184, 1, 10),
			new PresetInventoryItem("Material", 188, 1, 10),
			new PresetInventoryItem("Material", 192, 1, 10),
			new PresetInventoryItem("Material", 196, 1, 10),
			new PresetInventoryItem("Material", 200, 1, 10),
			new PresetInventoryItem("Material", 204, 1, 10),
			new PresetInventoryItem("Material", 208, 1, 10),
			new PresetInventoryItem("Material", 212, 1, 10),
			new PresetInventoryItem("Material", 216, 1, 10),
			new PresetInventoryItem("Material", 220, 1, 10),
			new PresetInventoryItem("Material", 224, 1, 10),
			new PresetInventoryItem("Material", 228, 1, 10),
			new PresetInventoryItem("Material", 232, 1, 10)
		}, new List<PresetOrgMemberCombatSkill>
		{
			new PresetOrgMemberCombatSkill(17, 3),
			new PresetOrgMemberCombatSkill(119, 4),
			new PresetOrgMemberCombatSkill(219, 4),
			new PresetOrgMemberCombatSkill(414, 4),
			new PresetOrgMemberCombatSkill(690, 4)
		}, new sbyte[5] { 4, 4, 4, 4, 4 }, new short[8] { -70, -60, -80, -70, -70, -60, -70, -70 }, 22500, 18000, 10, 60, 9300, new short[16]
		{
			9, -1, -1, 12, 2, -1, 2, 2, 12, 9,
			12, -1, -1, -1, 2, 2
		}, 5, new short[14]
		{
			6, 12, 12, -1, 12, 2, -1, -1, 2, -1,
			-1, -1, 12, 9
		}, new short[6] { 2, 12, 9, -1, -1, 12 }, new List<sbyte> { 5, 64 }, 3, new ResourceInts(default(int), default(int), default(int), default(int), default(int), default(int), default(int), default(int)), new IntPair[18]
		{
			new IntPair(17, 1),
			new IntPair(8, 12),
			new IntPair(5, 1),
			new IntPair(6, 1),
			new IntPair(15, 1),
			new IntPair(16, 1),
			new IntPair(7, 1),
			new IntPair(14, 1),
			new IntPair(12, 1),
			new IntPair(4, 24),
			new IntPair(3, 1),
			new IntPair(13, 42),
			new IntPair(10, 18),
			new IntPair(2, 1),
			new IntPair(1, 1),
			new IntPair(11, 12),
			new IntPair(0, 6),
			new IntPair(9, 1)
		}, null));
		_dataArray.Add(new OrganizationMemberItem(69, 327, 3, 3, 6, 9, 3, restrictPrincipalAmount: false, -1, -1, -1, 0, 3, 7, 3, 0, 0, new int[2] { 328, 329 }, 1800, 6, 3000, 2250, 0, new List<short> { 24, 25, 26, 13 }, new List<short> { 61, 62, 63 }, new int[2] { 330, 331 }, canStroll: true, 240, new short[4] { 16, 19, 22, 25 }, new PresetEquipmentItemWithProb[8]
		{
			new PresetEquipmentItemWithProb("Armor", 81, 75),
			new PresetEquipmentItemWithProb("Armor", 351, 100),
			new PresetEquipmentItemWithProb("Armor", 486, 75),
			new PresetEquipmentItemWithProb("Armor", 225, 75),
			new PresetEquipmentItemWithProb("Accessory", 162, 75),
			new PresetEquipmentItemWithProb("Accessory", 144, 50),
			new PresetEquipmentItemWithProb("Accessory", 81, 50),
			new PresetEquipmentItemWithProb("Carrier", 9, 75)
		}, new PresetEquipmentItem("Clothing", 24), new List<PresetInventoryItem>
		{
			new PresetInventoryItem("SkillBook", 72, 3, 20),
			new PresetInventoryItem("SkillBook", 27, 1, 20),
			new PresetInventoryItem("SkillBook", 0, 1, 20),
			new PresetInventoryItem("Material", 42, 1, 20),
			new PresetInventoryItem("Material", 49, 1, 20),
			new PresetInventoryItem("CraftTool", 45, 1, 30),
			new PresetInventoryItem("Medicine", 9, 1, 20),
			new PresetInventoryItem("Medicine", 18, 1, 20),
			new PresetInventoryItem("Medicine", 45, 1, 20),
			new PresetInventoryItem("Food", 0, 3, 30),
			new PresetInventoryItem("Food", 93, 3, 30),
			new PresetInventoryItem("Food", 135, 3, 30),
			new PresetInventoryItem("Medicine", 60, 1, 20),
			new PresetInventoryItem("Medicine", 72, 1, 20),
			new PresetInventoryItem("Medicine", 88, 1, 20),
			new PresetInventoryItem("Medicine", 94, 1, 20),
			new PresetInventoryItem("Medicine", 136, 1, 20),
			new PresetInventoryItem("Medicine", 148, 1, 20),
			new PresetInventoryItem("Medicine", 160, 1, 20),
			new PresetInventoryItem("Medicine", 172, 1, 20),
			new PresetInventoryItem("Medicine", 184, 1, 20),
			new PresetInventoryItem("Medicine", 196, 1, 20),
			new PresetInventoryItem("Medicine", 238, 1, 10),
			new PresetInventoryItem("Medicine", 298, 1, 10),
			new PresetInventoryItem("TeaWine", 27, 1, 20),
			new PresetInventoryItem("TeaWine", 9, 1, 20),
			new PresetInventoryItem("Material", 140, 1, 10),
			new PresetInventoryItem("Material", 144, 1, 10),
			new PresetInventoryItem("Material", 148, 1, 10),
			new PresetInventoryItem("Material", 152, 1, 10),
			new PresetInventoryItem("Material", 156, 1, 10),
			new PresetInventoryItem("Material", 160, 1, 10),
			new PresetInventoryItem("Material", 164, 1, 10),
			new PresetInventoryItem("Material", 168, 1, 10),
			new PresetInventoryItem("Material", 172, 1, 10),
			new PresetInventoryItem("Material", 176, 1, 10),
			new PresetInventoryItem("Material", 180, 1, 10),
			new PresetInventoryItem("Material", 184, 1, 10),
			new PresetInventoryItem("Material", 188, 1, 10),
			new PresetInventoryItem("Material", 192, 1, 10),
			new PresetInventoryItem("Material", 196, 1, 10),
			new PresetInventoryItem("Material", 200, 1, 10),
			new PresetInventoryItem("Material", 204, 1, 10),
			new PresetInventoryItem("Material", 208, 1, 10),
			new PresetInventoryItem("Material", 212, 1, 10),
			new PresetInventoryItem("Material", 216, 1, 10),
			new PresetInventoryItem("Material", 220, 1, 10),
			new PresetInventoryItem("Material", 224, 1, 10),
			new PresetInventoryItem("Material", 228, 1, 10),
			new PresetInventoryItem("Material", 232, 1, 10)
		}, new List<PresetOrgMemberCombatSkill>
		{
			new PresetOrgMemberCombatSkill(17, 2),
			new PresetOrgMemberCombatSkill(119, 3),
			new PresetOrgMemberCombatSkill(219, 3),
			new PresetOrgMemberCombatSkill(414, 3),
			new PresetOrgMemberCombatSkill(690, 3)
		}, new sbyte[5] { 4, 4, 4, 4, 4 }, new short[8] { -70, -60, -80, -70, -70, -60, -70, -70 }, 15000, 9000, 10, 70, 4500, new short[16]
		{
			9, -1, -1, 12, 2, -1, 2, 2, 12, 9,
			12, -1, -1, -1, 2, 2
		}, 4, new short[14]
		{
			6, 12, 12, -1, 12, 2, -1, -1, 2, -1,
			-1, -1, 12, 9
		}, new short[6] { 2, 12, 9, -1, -1, 12 }, new List<sbyte> { 5, 64 }, 3, new ResourceInts(default(int), default(int), default(int), default(int), default(int), default(int), default(int), default(int)), new IntPair[18]
		{
			new IntPair(17, 1),
			new IntPair(8, 12),
			new IntPair(5, 1),
			new IntPair(6, 1),
			new IntPair(15, 1),
			new IntPair(16, 1),
			new IntPair(7, 1),
			new IntPair(14, 1),
			new IntPair(12, 1),
			new IntPair(4, 24),
			new IntPair(3, 1),
			new IntPair(13, 42),
			new IntPair(10, 18),
			new IntPair(2, 1),
			new IntPair(1, 1),
			new IntPair(11, 12),
			new IntPair(0, 6),
			new IntPair(9, 1)
		}, null));
		_dataArray.Add(new OrganizationMemberItem(70, 332, 3, 2, 6, 9, 3, restrictPrincipalAmount: false, -1, -1, -1, 0, 2, 7, 2, 0, 0, new int[2] { 333, 334 }, 600, 4, 2000, 900, 0, new List<short> { 24, 25, 26, 13 }, new List<short> { 61, 62, 63 }, new int[2] { 335, 336 }, canStroll: true, 241, new short[4] { 14, 16, 18, 20 }, new PresetEquipmentItemWithProb[8]
		{
			new PresetEquipmentItemWithProb("Armor", 81, 75),
			new PresetEquipmentItemWithProb("Armor", 351, 100),
			new PresetEquipmentItemWithProb("Armor", 486, 75),
			new PresetEquipmentItemWithProb("Armor", 225, 75),
			new PresetEquipmentItemWithProb("Accessory", 162, 75),
			new PresetEquipmentItemWithProb("Accessory", 144, 50),
			new PresetEquipmentItemWithProb("Accessory", 81, 50),
			new PresetEquipmentItemWithProb("Carrier", 9, 75)
		}, new PresetEquipmentItem("Clothing", 24), new List<PresetInventoryItem>
		{
			new PresetInventoryItem("SkillBook", 72, 3, 10),
			new PresetInventoryItem("SkillBook", 27, 1, 10),
			new PresetInventoryItem("SkillBook", 0, 1, 10),
			new PresetInventoryItem("Material", 42, 1, 10),
			new PresetInventoryItem("Material", 49, 1, 10),
			new PresetInventoryItem("CraftTool", 45, 1, 20),
			new PresetInventoryItem("Medicine", 9, 1, 10),
			new PresetInventoryItem("Medicine", 18, 1, 10),
			new PresetInventoryItem("Medicine", 45, 1, 10),
			new PresetInventoryItem("Food", 0, 3, 20),
			new PresetInventoryItem("Food", 93, 3, 20),
			new PresetInventoryItem("Food", 135, 3, 20),
			new PresetInventoryItem("Medicine", 54, 1, 20),
			new PresetInventoryItem("Medicine", 66, 1, 20),
			new PresetInventoryItem("Medicine", 82, 1, 20),
			new PresetInventoryItem("Medicine", 130, 1, 10),
			new PresetInventoryItem("Medicine", 142, 1, 10),
			new PresetInventoryItem("Medicine", 154, 1, 10),
			new PresetInventoryItem("Medicine", 166, 1, 10),
			new PresetInventoryItem("Medicine", 178, 1, 10),
			new PresetInventoryItem("Medicine", 190, 1, 10),
			new PresetInventoryItem("Material", 140, 1, 10),
			new PresetInventoryItem("Material", 144, 1, 10),
			new PresetInventoryItem("Material", 148, 1, 10),
			new PresetInventoryItem("Material", 152, 1, 10),
			new PresetInventoryItem("Material", 156, 1, 10),
			new PresetInventoryItem("Material", 160, 1, 10),
			new PresetInventoryItem("Material", 164, 1, 10),
			new PresetInventoryItem("Material", 168, 1, 10),
			new PresetInventoryItem("Material", 172, 1, 10),
			new PresetInventoryItem("Material", 176, 1, 10),
			new PresetInventoryItem("Material", 180, 1, 10),
			new PresetInventoryItem("Material", 184, 1, 10),
			new PresetInventoryItem("Material", 188, 1, 10),
			new PresetInventoryItem("Material", 192, 1, 10),
			new PresetInventoryItem("Material", 196, 1, 10),
			new PresetInventoryItem("Material", 200, 1, 10),
			new PresetInventoryItem("Material", 204, 1, 10),
			new PresetInventoryItem("Material", 208, 1, 10),
			new PresetInventoryItem("Material", 212, 1, 10),
			new PresetInventoryItem("Material", 216, 1, 10),
			new PresetInventoryItem("Material", 220, 1, 10),
			new PresetInventoryItem("Material", 224, 1, 10),
			new PresetInventoryItem("Material", 228, 1, 10),
			new PresetInventoryItem("Material", 232, 1, 10)
		}, new List<PresetOrgMemberCombatSkill>
		{
			new PresetOrgMemberCombatSkill(17, 2),
			new PresetOrgMemberCombatSkill(119, 2),
			new PresetOrgMemberCombatSkill(219, 2),
			new PresetOrgMemberCombatSkill(414, 2),
			new PresetOrgMemberCombatSkill(690, 2)
		}, new sbyte[5] { 2, 2, 2, 2, 2 }, new short[8] { -70, -60, -80, -70, -70, -60, -70, -70 }, 11250, 3000, 5, 80, 1800, new short[16]
		{
			9, -1, -1, 12, 2, -1, 2, 2, 12, 9,
			12, -1, -1, -1, 2, 2
		}, 3, new short[14]
		{
			6, 12, 12, -1, 12, 2, -1, -1, 2, -1,
			-1, -1, 12, 9
		}, new short[6] { 2, 12, 9, -1, -1, 12 }, new List<sbyte> { 5, 64 }, 3, new ResourceInts(default(int), default(int), default(int), default(int), default(int), default(int), default(int), default(int)), new IntPair[18]
		{
			new IntPair(17, 1),
			new IntPair(8, 9),
			new IntPair(5, 1),
			new IntPair(6, 1),
			new IntPair(15, 1),
			new IntPair(16, 1),
			new IntPair(7, 1),
			new IntPair(14, 1),
			new IntPair(12, 1),
			new IntPair(4, 21),
			new IntPair(3, 1),
			new IntPair(13, 30),
			new IntPair(10, 18),
			new IntPair(2, 1),
			new IntPair(1, 1),
			new IntPair(11, 12),
			new IntPair(0, 6),
			new IntPair(9, 1)
		}, null));
		_dataArray.Add(new OrganizationMemberItem(71, 337, 3, 1, 6, 9, 3, restrictPrincipalAmount: false, -1, -1, -1, 0, 1, 7, 1, 0, 0, new int[2] { 338, 339 }, 300, 2, 1000, 300, 0, new List<short> { 24, 25, 26, 13 }, new List<short> { 61, 62, 63 }, new int[2] { 340, 341 }, canStroll: true, 242, new short[4] { 12, 13, 14, 15 }, new PresetEquipmentItemWithProb[8]
		{
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Armor", 351, 100),
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Armor", 225, 75),
			new PresetEquipmentItemWithProb("Accessory", 162, 75),
			new PresetEquipmentItemWithProb("Accessory", 144, 50),
			new PresetEquipmentItemWithProb("Accessory", -1, 0),
			new PresetEquipmentItemWithProb("Carrier", -1, 0)
		}, new PresetEquipmentItem("Clothing", 24), new List<PresetInventoryItem>
		{
			new PresetInventoryItem("SkillBook", 72, 3, 10),
			new PresetInventoryItem("SkillBook", 27, 1, 10),
			new PresetInventoryItem("SkillBook", 0, 1, 10),
			new PresetInventoryItem("Material", 42, 1, 10),
			new PresetInventoryItem("Material", 49, 1, 10),
			new PresetInventoryItem("CraftTool", 45, 1, 20),
			new PresetInventoryItem("Medicine", 9, 1, 10),
			new PresetInventoryItem("Medicine", 18, 1, 10),
			new PresetInventoryItem("Medicine", 45, 1, 10),
			new PresetInventoryItem("Food", 0, 3, 20),
			new PresetInventoryItem("Food", 93, 3, 20),
			new PresetInventoryItem("Food", 135, 3, 20),
			new PresetInventoryItem("Medicine", 54, 1, 20),
			new PresetInventoryItem("Medicine", 66, 1, 20),
			new PresetInventoryItem("Medicine", 82, 1, 20),
			new PresetInventoryItem("Medicine", 130, 1, 10),
			new PresetInventoryItem("Medicine", 142, 1, 10),
			new PresetInventoryItem("Medicine", 154, 1, 10),
			new PresetInventoryItem("Medicine", 166, 1, 10),
			new PresetInventoryItem("Medicine", 178, 1, 10),
			new PresetInventoryItem("Medicine", 190, 1, 10),
			new PresetInventoryItem("Material", 140, 1, 10),
			new PresetInventoryItem("Material", 144, 1, 10),
			new PresetInventoryItem("Material", 148, 1, 10),
			new PresetInventoryItem("Material", 152, 1, 10),
			new PresetInventoryItem("Material", 156, 1, 10),
			new PresetInventoryItem("Material", 160, 1, 10),
			new PresetInventoryItem("Material", 164, 1, 10),
			new PresetInventoryItem("Material", 168, 1, 10),
			new PresetInventoryItem("Material", 172, 1, 10),
			new PresetInventoryItem("Material", 176, 1, 10),
			new PresetInventoryItem("Material", 180, 1, 10),
			new PresetInventoryItem("Material", 184, 1, 10),
			new PresetInventoryItem("Material", 188, 1, 10),
			new PresetInventoryItem("Material", 192, 1, 10),
			new PresetInventoryItem("Material", 196, 1, 10),
			new PresetInventoryItem("Material", 200, 1, 10),
			new PresetInventoryItem("Material", 204, 1, 10),
			new PresetInventoryItem("Material", 208, 1, 10),
			new PresetInventoryItem("Material", 212, 1, 10),
			new PresetInventoryItem("Material", 216, 1, 10),
			new PresetInventoryItem("Material", 220, 1, 10),
			new PresetInventoryItem("Material", 224, 1, 10),
			new PresetInventoryItem("Material", 228, 1, 10),
			new PresetInventoryItem("Material", 232, 1, 10)
		}, new List<PresetOrgMemberCombatSkill>
		{
			new PresetOrgMemberCombatSkill(17, 1),
			new PresetOrgMemberCombatSkill(119, 1),
			new PresetOrgMemberCombatSkill(219, 1),
			new PresetOrgMemberCombatSkill(414, 1),
			new PresetOrgMemberCombatSkill(690, 1)
		}, new sbyte[5], new short[8] { -70, -60, -80, -70, -70, -60, -70, -70 }, 7500, 1500, 5, 90, 600, new short[16]
		{
			9, -1, -1, 12, 2, -1, 2, 2, 12, 9,
			12, -1, -1, -1, 2, 2
		}, 2, new short[14]
		{
			6, 12, 12, -1, 12, 2, -1, -1, 2, -1,
			-1, -1, 12, 9
		}, new short[6] { 2, 12, 9, -1, -1, 12 }, new List<sbyte> { 5, 64 }, 3, new ResourceInts(default(int), default(int), default(int), default(int), default(int), default(int), default(int), default(int)), new IntPair[18]
		{
			new IntPair(17, 1),
			new IntPair(8, 9),
			new IntPair(5, 1),
			new IntPair(6, 1),
			new IntPair(15, 1),
			new IntPair(16, 1),
			new IntPair(7, 1),
			new IntPair(14, 1),
			new IntPair(12, 1),
			new IntPair(4, 21),
			new IntPair(3, 1),
			new IntPair(13, 30),
			new IntPair(10, 18),
			new IntPair(2, 1),
			new IntPair(1, 1),
			new IntPair(11, 12),
			new IntPair(0, 6),
			new IntPair(9, 1)
		}, null));
		_dataArray.Add(new OrganizationMemberItem(72, 342, 3, 0, 8, 12, 4, restrictPrincipalAmount: false, -1, -1, -1, 0, 0, 6, 0, 0, 0, new int[2] { 343, 344 }, 150, 0, 500, 150, 0, new List<short> { 24, 25, 26, 13 }, new List<short> { 61, 62, 63 }, new int[2] { 345, 346 }, canStroll: true, 243, new short[4] { 10, 10, 10, 10 }, new PresetEquipmentItemWithProb[8]
		{
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Armor", 351, 100),
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Armor", 225, 75),
			new PresetEquipmentItemWithProb("Accessory", 162, 75),
			new PresetEquipmentItemWithProb("Accessory", -1, 0),
			new PresetEquipmentItemWithProb("Accessory", -1, 0),
			new PresetEquipmentItemWithProb("Carrier", -1, 0)
		}, new PresetEquipmentItem("Clothing", 24), new List<PresetInventoryItem>
		{
			new PresetInventoryItem("SkillBook", 72, 3, 10),
			new PresetInventoryItem("SkillBook", 27, 1, 10),
			new PresetInventoryItem("SkillBook", 0, 1, 10),
			new PresetInventoryItem("Material", 42, 1, 10),
			new PresetInventoryItem("Material", 49, 1, 10),
			new PresetInventoryItem("CraftTool", 45, 1, 20),
			new PresetInventoryItem("Medicine", 9, 1, 10),
			new PresetInventoryItem("Medicine", 18, 1, 10),
			new PresetInventoryItem("Medicine", 45, 1, 10),
			new PresetInventoryItem("Food", 0, 3, 20),
			new PresetInventoryItem("Food", 93, 3, 20),
			new PresetInventoryItem("Food", 135, 3, 20),
			new PresetInventoryItem("Medicine", 54, 1, 20),
			new PresetInventoryItem("Medicine", 66, 1, 20),
			new PresetInventoryItem("Medicine", 82, 1, 20),
			new PresetInventoryItem("Medicine", 130, 1, 10),
			new PresetInventoryItem("Medicine", 142, 1, 10),
			new PresetInventoryItem("Medicine", 154, 1, 10),
			new PresetInventoryItem("Medicine", 166, 1, 10),
			new PresetInventoryItem("Medicine", 178, 1, 10),
			new PresetInventoryItem("Medicine", 190, 1, 10),
			new PresetInventoryItem("Material", 140, 1, 10),
			new PresetInventoryItem("Material", 144, 1, 10),
			new PresetInventoryItem("Material", 148, 1, 10),
			new PresetInventoryItem("Material", 152, 1, 10),
			new PresetInventoryItem("Material", 156, 1, 10),
			new PresetInventoryItem("Material", 160, 1, 10),
			new PresetInventoryItem("Material", 164, 1, 10),
			new PresetInventoryItem("Material", 168, 1, 10),
			new PresetInventoryItem("Material", 172, 1, 10),
			new PresetInventoryItem("Material", 176, 1, 10),
			new PresetInventoryItem("Material", 180, 1, 10),
			new PresetInventoryItem("Material", 184, 1, 10),
			new PresetInventoryItem("Material", 188, 1, 10),
			new PresetInventoryItem("Material", 192, 1, 10),
			new PresetInventoryItem("Material", 196, 1, 10),
			new PresetInventoryItem("Material", 200, 1, 10),
			new PresetInventoryItem("Material", 204, 1, 10),
			new PresetInventoryItem("Material", 208, 1, 10),
			new PresetInventoryItem("Material", 212, 1, 10),
			new PresetInventoryItem("Material", 216, 1, 10),
			new PresetInventoryItem("Material", 220, 1, 10),
			new PresetInventoryItem("Material", 224, 1, 10),
			new PresetInventoryItem("Material", 228, 1, 10),
			new PresetInventoryItem("Material", 232, 1, 10)
		}, new List<PresetOrgMemberCombatSkill>
		{
			new PresetOrgMemberCombatSkill(17, 0),
			new PresetOrgMemberCombatSkill(119, 0),
			new PresetOrgMemberCombatSkill(414, 0)
		}, new sbyte[5], new short[8] { -70, -60, -80, -70, -70, -60, -70, -70 }, 3750, 750, 1, 100, 300, new short[16]
		{
			9, -1, -1, 12, 2, -1, 2, 2, 12, 9,
			12, -1, -1, -1, 2, 2
		}, 1, new short[14]
		{
			6, 12, 12, -1, 12, 2, -1, -1, 2, -1,
			-1, -1, 12, 9
		}, new short[6] { 2, 12, 9, -1, -1, 12 }, new List<sbyte> { 5, 64 }, 3, new ResourceInts(default(int), default(int), default(int), default(int), default(int), default(int), default(int), default(int)), new IntPair[18]
		{
			new IntPair(17, 1),
			new IntPair(8, 9),
			new IntPair(5, 1),
			new IntPair(6, 1),
			new IntPair(15, 1),
			new IntPair(16, 1),
			new IntPair(7, 1),
			new IntPair(14, 1),
			new IntPair(12, 1),
			new IntPair(4, 21),
			new IntPair(3, 1),
			new IntPair(13, 30),
			new IntPair(10, 18),
			new IntPair(2, 1),
			new IntPair(1, 1),
			new IntPair(11, 12),
			new IntPair(0, 6),
			new IntPair(9, 1)
		}, null));
		_dataArray.Add(new OrganizationMemberItem(73, 262, 4, 8, 1, 1, 1, restrictPrincipalAmount: true, -1, -1, 7, 4, 5, -1, 7, 50, 129, new int[2] { 347, 347 }, 15800, 16, 13000, 30750, 0, new List<short> { 27, 28, 29, 30, 5, 14 }, new List<short> { 55, 56, 57 }, new int[2] { 348, 266 }, canStroll: false, 244, new short[4] { 34, 42, 50, 58 }, new PresetEquipmentItemWithProb[8]
		{
			new PresetEquipmentItemWithProb("Armor", 72, 75),
			new PresetEquipmentItemWithProb("Armor", 369, 100),
			new PresetEquipmentItemWithProb("Armor", 450, 75),
			new PresetEquipmentItemWithProb("Armor", 198, 75),
			new PresetEquipmentItemWithProb("Accessory", 171, 75),
			new PresetEquipmentItemWithProb("Accessory", 144, 50),
			new PresetEquipmentItemWithProb("Accessory", 198, 50),
			new PresetEquipmentItemWithProb("Carrier", 18, 75)
		}, new PresetEquipmentItem("Clothing", 29), new List<PresetInventoryItem>
		{
			new PresetInventoryItem("SkillBook", 108, 3, 40),
			new PresetInventoryItem("SkillBook", 18, 1, 30),
			new PresetInventoryItem("Material", 0, 1, 30),
			new PresetInventoryItem("Material", 7, 1, 30),
			new PresetInventoryItem("Food", 93, 3, 40),
			new PresetInventoryItem("Food", 135, 3, 40),
			new PresetInventoryItem("Medicine", 60, 1, 20),
			new PresetInventoryItem("Medicine", 72, 1, 20),
			new PresetInventoryItem("Medicine", 88, 1, 30),
			new PresetInventoryItem("Medicine", 244, 1, 20),
			new PresetInventoryItem("Medicine", 304, 1, 20),
			new PresetInventoryItem("Medicine", 220, 1, 20),
			new PresetInventoryItem("Medicine", 268, 1, 30),
			new PresetInventoryItem("TeaWine", 18, 1, 20),
			new PresetInventoryItem("TeaWine", 27, 1, 20)
		}, new List<PresetOrgMemberCombatSkill>
		{
			new PresetOrgMemberCombatSkill(22, 8),
			new PresetOrgMemberCombatSkill(127, 6),
			new PresetOrgMemberCombatSkill(228, 7),
			new PresetOrgMemberCombatSkill(336, 8),
			new PresetOrgMemberCombatSkill(532, 8),
			new PresetOrgMemberCombatSkill(673, 7)
		}, new sbyte[5] { 10, 10, 10, 10, 10 }, new short[8] { -70, -70, -70, -70, -50, -70, -70, -60 }, 160000, 576000, 20, 20, 61500, new short[16]
		{
			-1, -1, 9, -1, 6, -1, -1, 9, -1, -1,
			-1, -1, 12, 2, -1, -1
		}, 8, new short[14]
		{
			12, 9, 12, 12, -1, -1, 2, 12, -1, -1,
			-1, 12, -1, -1
		}, new short[6] { -1, 9, 9, -1, 12, -1 }, new List<sbyte> { 12, 24, 25, 55 }, 3, new ResourceInts(default(int), default(int), default(int), default(int), default(int), default(int), default(int), default(int)), new IntPair[18]
		{
			new IntPair(17, 1),
			new IntPair(8, 1),
			new IntPair(5, 54),
			new IntPair(6, 0),
			new IntPair(15, 1),
			new IntPair(16, 6),
			new IntPair(7, 1),
			new IntPair(14, 30),
			new IntPair(12, 0),
			new IntPair(4, 27),
			new IntPair(3, 1),
			new IntPair(13, 3),
			new IntPair(10, 1),
			new IntPair(2, 1),
			new IntPair(1, 1),
			new IntPair(11, 3),
			new IntPair(0, 12),
			new IntPair(9, 1)
		}, null));
		_dataArray.Add(new OrganizationMemberItem(74, 229, 4, 7, 4, 6, 2, restrictPrincipalAmount: false, -1, -1, 7, 3, 5, 8, 7, 50, 129, new int[2] { 349, 349 }, 8600, 14, 10000, 21150, 0, new List<short> { 27, 28, 29, 30, 5, 14 }, new List<short> { 55, 56, 57 }, new int[2] { 350, 351 }, canStroll: false, 245, new short[4] { 31, 38, 45, 52 }, new PresetEquipmentItemWithProb[8]
		{
			new PresetEquipmentItemWithProb("Armor", 72, 75),
			new PresetEquipmentItemWithProb("Armor", 369, 100),
			new PresetEquipmentItemWithProb("Armor", 450, 75),
			new PresetEquipmentItemWithProb("Armor", 198, 75),
			new PresetEquipmentItemWithProb("Accessory", 171, 75),
			new PresetEquipmentItemWithProb("Accessory", 144, 50),
			new PresetEquipmentItemWithProb("Accessory", 198, 50),
			new PresetEquipmentItemWithProb("Carrier", 18, 75)
		}, new PresetEquipmentItem("Clothing", 28), new List<PresetInventoryItem>
		{
			new PresetInventoryItem("SkillBook", 108, 3, 40),
			new PresetInventoryItem("SkillBook", 18, 1, 30),
			new PresetInventoryItem("Material", 0, 1, 30),
			new PresetInventoryItem("Material", 7, 1, 30),
			new PresetInventoryItem("Food", 93, 3, 40),
			new PresetInventoryItem("Food", 135, 3, 40),
			new PresetInventoryItem("Medicine", 60, 1, 20),
			new PresetInventoryItem("Medicine", 72, 1, 20),
			new PresetInventoryItem("Medicine", 88, 1, 30),
			new PresetInventoryItem("Medicine", 244, 1, 20),
			new PresetInventoryItem("Medicine", 304, 1, 20),
			new PresetInventoryItem("Medicine", 220, 1, 20),
			new PresetInventoryItem("Medicine", 268, 1, 30),
			new PresetInventoryItem("TeaWine", 18, 1, 20),
			new PresetInventoryItem("TeaWine", 27, 1, 20)
		}, new List<PresetOrgMemberCombatSkill>
		{
			new PresetOrgMemberCombatSkill(22, 7),
			new PresetOrgMemberCombatSkill(127, 5),
			new PresetOrgMemberCombatSkill(228, 6),
			new PresetOrgMemberCombatSkill(336, 7),
			new PresetOrgMemberCombatSkill(532, 7),
			new PresetOrgMemberCombatSkill(673, 6)
		}, new sbyte[5] { 8, 8, 8, 8, 8 }, new short[8] { -70, -70, -70, -70, -50, -70, -70, -60 }, 100000, 288000, 15, 30, 42300, new short[16]
		{
			-1, -1, 9, -1, 6, -1, -1, 9, -1, -1,
			-1, -1, 12, 2, -1, -1
		}, 8, new short[14]
		{
			12, 9, 12, 12, -1, -1, 2, 12, -1, -1,
			-1, 12, -1, -1
		}, new short[6] { -1, 9, 9, -1, 12, -1 }, new List<sbyte> { 12, 24, 25 }, 3, new ResourceInts(default(int), default(int), default(int), default(int), default(int), default(int), default(int), default(int)), new IntPair[18]
		{
			new IntPair(17, 1),
			new IntPair(8, 1),
			new IntPair(5, 54),
			new IntPair(6, 0),
			new IntPair(15, 1),
			new IntPair(16, 6),
			new IntPair(7, 1),
			new IntPair(14, 30),
			new IntPair(12, 0),
			new IntPair(4, 27),
			new IntPair(3, 1),
			new IntPair(13, 3),
			new IntPair(10, 1),
			new IntPair(2, 1),
			new IntPair(1, 1),
			new IntPair(11, 3),
			new IntPair(0, 12),
			new IntPair(9, 1)
		}, null));
		_dataArray.Add(new OrganizationMemberItem(75, 269, 4, 6, 4, 6, 2, restrictPrincipalAmount: false, -1, -1, -1, 2, 5, 8, 6, 50, 129, new int[2] { 349, 349 }, 6100, 12, 8000, 13800, 0, new List<short> { 27, 28, 29, 30, 5, 14 }, new List<short> { 55, 56, 57 }, new int[2] { 352, 353 }, canStroll: false, 246, new short[4] { 28, 34, 40, 46 }, new PresetEquipmentItemWithProb[8]
		{
			new PresetEquipmentItemWithProb("Armor", 72, 75),
			new PresetEquipmentItemWithProb("Armor", 369, 100),
			new PresetEquipmentItemWithProb("Armor", 450, 75),
			new PresetEquipmentItemWithProb("Armor", 198, 75),
			new PresetEquipmentItemWithProb("Accessory", 171, 75),
			new PresetEquipmentItemWithProb("Accessory", 144, 50),
			new PresetEquipmentItemWithProb("Accessory", 198, 50),
			new PresetEquipmentItemWithProb("Carrier", 18, 75)
		}, new PresetEquipmentItem("Clothing", 28), new List<PresetInventoryItem>
		{
			new PresetInventoryItem("SkillBook", 108, 3, 40),
			new PresetInventoryItem("SkillBook", 18, 1, 30),
			new PresetInventoryItem("Material", 0, 1, 30),
			new PresetInventoryItem("Material", 7, 1, 30),
			new PresetInventoryItem("Food", 93, 3, 40),
			new PresetInventoryItem("Food", 135, 3, 40),
			new PresetInventoryItem("Medicine", 60, 1, 20),
			new PresetInventoryItem("Medicine", 72, 1, 20),
			new PresetInventoryItem("Medicine", 88, 1, 30),
			new PresetInventoryItem("Medicine", 244, 1, 20),
			new PresetInventoryItem("Medicine", 304, 1, 20),
			new PresetInventoryItem("Medicine", 220, 1, 20),
			new PresetInventoryItem("Medicine", 268, 1, 30),
			new PresetInventoryItem("TeaWine", 18, 1, 20),
			new PresetInventoryItem("TeaWine", 27, 1, 20)
		}, new List<PresetOrgMemberCombatSkill>
		{
			new PresetOrgMemberCombatSkill(22, 6),
			new PresetOrgMemberCombatSkill(127, 5),
			new PresetOrgMemberCombatSkill(228, 6),
			new PresetOrgMemberCombatSkill(336, 6),
			new PresetOrgMemberCombatSkill(532, 6),
			new PresetOrgMemberCombatSkill(673, 6)
		}, new sbyte[5] { 8, 8, 8, 8, 8 }, new short[8] { -70, -70, -70, -70, -50, -70, -70, -60 }, 60000, 144000, 15, 40, 27600, new short[16]
		{
			-1, -1, 9, -1, 6, -1, -1, 9, -1, -1,
			-1, -1, 12, 2, -1, -1
		}, 7, new short[14]
		{
			12, 9, 12, 12, -1, -1, 2, 12, -1, -1,
			-1, 12, -1, -1
		}, new short[6] { -1, 9, 9, -1, 12, -1 }, new List<sbyte> { 12, 24, 25 }, 3, new ResourceInts(default(int), default(int), default(int), default(int), default(int), default(int), default(int), default(int)), new IntPair[18]
		{
			new IntPair(17, 1),
			new IntPair(8, 1),
			new IntPair(5, 54),
			new IntPair(6, 0),
			new IntPair(15, 1),
			new IntPair(16, 6),
			new IntPair(7, 1),
			new IntPair(14, 30),
			new IntPair(12, 0),
			new IntPair(4, 27),
			new IntPair(3, 1),
			new IntPair(13, 3),
			new IntPair(10, 1),
			new IntPair(2, 1),
			new IntPair(1, 1),
			new IntPair(11, 3),
			new IntPair(0, 12),
			new IntPair(9, 1)
		}, null));
		List<OrganizationMemberItem> dataArray5 = _dataArray;
		int[] monasticTitleSuffixes5 = new int[2] { 354, 355 };
		List<short> favoriteClothingIds5 = new List<short> { 27, 28, 29, 30, 5, 14 };
		List<short> hatedClothingIds5 = new List<short> { 55, 56, 57 };
		int[] spouseAnonymousTitles5 = new int[2] { 356, 357 };
		short[] initialAges5 = new short[4] { 25, 30, 35, 40 };
		PresetEquipmentItemWithProb[] equipment5 = new PresetEquipmentItemWithProb[8]
		{
			new PresetEquipmentItemWithProb("Armor", 72, 75),
			new PresetEquipmentItemWithProb("Armor", 369, 100),
			new PresetEquipmentItemWithProb("Armor", 450, 75),
			new PresetEquipmentItemWithProb("Armor", 198, 75),
			new PresetEquipmentItemWithProb("Accessory", 171, 75),
			new PresetEquipmentItemWithProb("Accessory", 144, 50),
			new PresetEquipmentItemWithProb("Accessory", 198, 50),
			new PresetEquipmentItemWithProb("Carrier", 18, 75)
		};
		PresetEquipmentItem clothing5 = new PresetEquipmentItem("Clothing", 28);
		List<PresetInventoryItem> inventory5 = new List<PresetInventoryItem>
		{
			new PresetInventoryItem("SkillBook", 108, 3, 30),
			new PresetInventoryItem("SkillBook", 18, 1, 20),
			new PresetInventoryItem("Material", 0, 1, 20),
			new PresetInventoryItem("Material", 7, 1, 20),
			new PresetInventoryItem("Food", 93, 3, 30),
			new PresetInventoryItem("Food", 135, 3, 30),
			new PresetInventoryItem("Medicine", 54, 1, 20),
			new PresetInventoryItem("Medicine", 66, 1, 20),
			new PresetInventoryItem("Medicine", 82, 1, 30),
			new PresetInventoryItem("Medicine", 238, 1, 10),
			new PresetInventoryItem("Medicine", 298, 1, 10),
			new PresetInventoryItem("Medicine", 214, 1, 10),
			new PresetInventoryItem("Medicine", 262, 1, 30),
			new PresetInventoryItem("TeaWine", 18, 1, 20),
			new PresetInventoryItem("TeaWine", 27, 1, 20)
		};
		List<PresetOrgMemberCombatSkill> combatSkills5 = new List<PresetOrgMemberCombatSkill>
		{
			new PresetOrgMemberCombatSkill(22, 5),
			new PresetOrgMemberCombatSkill(127, 5),
			new PresetOrgMemberCombatSkill(228, 5),
			new PresetOrgMemberCombatSkill(336, 5),
			new PresetOrgMemberCombatSkill(532, 5),
			new PresetOrgMemberCombatSkill(673, 5)
		};
		sbyte[] extraCombatSkillGrids5 = new sbyte[5] { 6, 6, 6, 6, 6 };
		short[] resourcesAdjust5 = new short[8] { -70, -70, -70, -70, -50, -70, -70, -60 };
		short[] lifeSkillsAdjust5 = new short[16]
		{
			-1, -1, 9, -1, 6, -1, -1, 9, -1, -1,
			-1, -1, 12, 2, -1, -1
		};
		short[] combatSkillsAdjust5 = new short[14]
		{
			12, 9, 12, 12, -1, -1, 2, 12, -1, -1,
			-1, 12, -1, -1
		};
		short[] mainAttributesAdjust5 = new short[6] { -1, 9, 9, -1, 12, -1 };
		identityInteractConfig = new List<sbyte>();
		dataArray5.Add(new OrganizationMemberItem(76, 272, 4, 5, 6, 9, 3, restrictPrincipalAmount: false, -1, -1, -1, 2, 5, 8, 5, 50, 129, monasticTitleSuffixes5, 4200, 10, 6000, 8400, 0, favoriteClothingIds5, hatedClothingIds5, spouseAnonymousTitles5, canStroll: true, 247, initialAges5, equipment5, clothing5, inventory5, combatSkills5, extraCombatSkillGrids5, resourcesAdjust5, 40000, 48000, 10, 50, 16800, lifeSkillsAdjust5, 6, combatSkillsAdjust5, mainAttributesAdjust5, identityInteractConfig, 3, new ResourceInts(default(int), default(int), default(int), default(int), default(int), default(int), default(int), default(int)), new IntPair[18]
		{
			new IntPair(17, 1),
			new IntPair(8, 1),
			new IntPair(5, 42),
			new IntPair(6, 0),
			new IntPair(15, 1),
			new IntPair(16, 6),
			new IntPair(7, 1),
			new IntPair(14, 30),
			new IntPair(12, 0),
			new IntPair(4, 21),
			new IntPair(3, 1),
			new IntPair(13, 3),
			new IntPair(10, 1),
			new IntPair(2, 1),
			new IntPair(1, 1),
			new IntPair(11, 3),
			new IntPair(0, 12),
			new IntPair(9, 1)
		}, null));
		List<OrganizationMemberItem> dataArray6 = _dataArray;
		int[] monasticTitleSuffixes6 = new int[2] { 359, 360 };
		List<short> favoriteClothingIds6 = new List<short> { 27, 28, 29, 30, 5, 14 };
		List<short> hatedClothingIds6 = new List<short> { 55, 56, 57 };
		int[] spouseAnonymousTitles6 = new int[2] { 361, 362 };
		short[] initialAges6 = new short[4] { 22, 26, 30, 34 };
		PresetEquipmentItemWithProb[] equipment6 = new PresetEquipmentItemWithProb[8]
		{
			new PresetEquipmentItemWithProb("Armor", 72, 75),
			new PresetEquipmentItemWithProb("Armor", 369, 100),
			new PresetEquipmentItemWithProb("Armor", 450, 75),
			new PresetEquipmentItemWithProb("Armor", 198, 75),
			new PresetEquipmentItemWithProb("Accessory", 171, 75),
			new PresetEquipmentItemWithProb("Accessory", 144, 50),
			new PresetEquipmentItemWithProb("Accessory", 198, 50),
			new PresetEquipmentItemWithProb("Carrier", 18, 75)
		};
		PresetEquipmentItem clothing6 = new PresetEquipmentItem("Clothing", 27);
		List<PresetInventoryItem> inventory6 = new List<PresetInventoryItem>
		{
			new PresetInventoryItem("SkillBook", 108, 3, 30),
			new PresetInventoryItem("SkillBook", 18, 1, 20),
			new PresetInventoryItem("Material", 0, 1, 20),
			new PresetInventoryItem("Material", 7, 1, 20),
			new PresetInventoryItem("Food", 93, 3, 30),
			new PresetInventoryItem("Food", 135, 3, 30),
			new PresetInventoryItem("Medicine", 54, 1, 20),
			new PresetInventoryItem("Medicine", 66, 1, 20),
			new PresetInventoryItem("Medicine", 82, 1, 30),
			new PresetInventoryItem("Medicine", 238, 1, 10),
			new PresetInventoryItem("Medicine", 298, 1, 10),
			new PresetInventoryItem("Medicine", 214, 1, 10),
			new PresetInventoryItem("Medicine", 262, 1, 30),
			new PresetInventoryItem("TeaWine", 18, 1, 20),
			new PresetInventoryItem("TeaWine", 27, 1, 20)
		};
		List<PresetOrgMemberCombatSkill> combatSkills6 = new List<PresetOrgMemberCombatSkill>
		{
			new PresetOrgMemberCombatSkill(22, 4),
			new PresetOrgMemberCombatSkill(127, 4),
			new PresetOrgMemberCombatSkill(228, 4),
			new PresetOrgMemberCombatSkill(336, 4),
			new PresetOrgMemberCombatSkill(532, 4)
		};
		sbyte[] extraCombatSkillGrids6 = new sbyte[5] { 4, 4, 4, 4, 4 };
		short[] resourcesAdjust6 = new short[8] { -70, -70, -70, -70, -50, -70, -70, -60 };
		short[] lifeSkillsAdjust6 = new short[16]
		{
			-1, -1, 9, -1, 6, -1, -1, 9, -1, -1,
			-1, -1, 12, 2, -1, -1
		};
		short[] combatSkillsAdjust6 = new short[14]
		{
			12, 9, 12, 12, -1, -1, 2, 12, -1, -1,
			-1, 12, -1, -1
		};
		short[] mainAttributesAdjust6 = new short[6] { -1, 9, 9, -1, 12, -1 };
		identityInteractConfig = new List<sbyte>();
		dataArray6.Add(new OrganizationMemberItem(77, 358, 4, 4, 10, 15, 5, restrictPrincipalAmount: false, -1, -1, -1, 1, 4, 7, 4, 50, 129, monasticTitleSuffixes6, 2800, 8, 4500, 4650, 0, favoriteClothingIds6, hatedClothingIds6, spouseAnonymousTitles6, canStroll: true, 248, initialAges6, equipment6, clothing6, inventory6, combatSkills6, extraCombatSkillGrids6, resourcesAdjust6, 30000, 24000, 10, 60, 9300, lifeSkillsAdjust6, 5, combatSkillsAdjust6, mainAttributesAdjust6, identityInteractConfig, 3, new ResourceInts(default(int), default(int), default(int), default(int), default(int), default(int), default(int), default(int)), new IntPair[18]
		{
			new IntPair(17, 1),
			new IntPair(8, 1),
			new IntPair(5, 42),
			new IntPair(6, 0),
			new IntPair(15, 1),
			new IntPair(16, 6),
			new IntPair(7, 1),
			new IntPair(14, 30),
			new IntPair(12, 0),
			new IntPair(4, 21),
			new IntPair(3, 1),
			new IntPair(13, 3),
			new IntPair(10, 1),
			new IntPair(2, 1),
			new IntPair(1, 1),
			new IntPair(11, 3),
			new IntPair(0, 12),
			new IntPair(9, 1)
		}, null));
		List<OrganizationMemberItem> dataArray7 = _dataArray;
		int[] monasticTitleSuffixes7 = new int[2] { 364, 365 };
		List<short> favoriteClothingIds7 = new List<short> { 27, 28, 29, 30, 5, 14 };
		List<short> hatedClothingIds7 = new List<short> { 55, 56, 57 };
		int[] spouseAnonymousTitles7 = new int[2] { 366, 367 };
		short[] initialAges7 = new short[4] { 19, 22, 25, 28 };
		PresetEquipmentItemWithProb[] equipment7 = new PresetEquipmentItemWithProb[8]
		{
			new PresetEquipmentItemWithProb("Armor", 72, 75),
			new PresetEquipmentItemWithProb("Armor", 369, 100),
			new PresetEquipmentItemWithProb("Armor", 450, 75),
			new PresetEquipmentItemWithProb("Armor", 198, 75),
			new PresetEquipmentItemWithProb("Accessory", 171, 75),
			new PresetEquipmentItemWithProb("Accessory", 144, 50),
			new PresetEquipmentItemWithProb("Accessory", 198, 50),
			new PresetEquipmentItemWithProb("Carrier", 18, 75)
		};
		PresetEquipmentItem clothing7 = new PresetEquipmentItem("Clothing", 27);
		List<PresetInventoryItem> inventory7 = new List<PresetInventoryItem>
		{
			new PresetInventoryItem("SkillBook", 108, 3, 30),
			new PresetInventoryItem("SkillBook", 18, 1, 20),
			new PresetInventoryItem("Material", 0, 1, 20),
			new PresetInventoryItem("Material", 7, 1, 20),
			new PresetInventoryItem("Food", 93, 3, 30),
			new PresetInventoryItem("Food", 135, 3, 30),
			new PresetInventoryItem("Medicine", 54, 1, 20),
			new PresetInventoryItem("Medicine", 66, 1, 20),
			new PresetInventoryItem("Medicine", 82, 1, 30),
			new PresetInventoryItem("Medicine", 238, 1, 10),
			new PresetInventoryItem("Medicine", 298, 1, 10),
			new PresetInventoryItem("Medicine", 214, 1, 10),
			new PresetInventoryItem("Medicine", 262, 1, 30),
			new PresetInventoryItem("TeaWine", 18, 1, 20),
			new PresetInventoryItem("TeaWine", 27, 1, 20)
		};
		List<PresetOrgMemberCombatSkill> combatSkills7 = new List<PresetOrgMemberCombatSkill>
		{
			new PresetOrgMemberCombatSkill(22, 3),
			new PresetOrgMemberCombatSkill(127, 3),
			new PresetOrgMemberCombatSkill(228, 3),
			new PresetOrgMemberCombatSkill(336, 3),
			new PresetOrgMemberCombatSkill(532, 3)
		};
		sbyte[] extraCombatSkillGrids7 = new sbyte[5] { 4, 4, 4, 4, 4 };
		short[] resourcesAdjust7 = new short[8] { -70, -70, -70, -70, -50, -70, -70, -60 };
		short[] lifeSkillsAdjust7 = new short[16]
		{
			-1, -1, 9, -1, 6, -1, -1, 9, -1, -1,
			-1, -1, 12, 2, -1, -1
		};
		short[] combatSkillsAdjust7 = new short[14]
		{
			12, 9, 12, 12, -1, -1, 2, 12, -1, -1,
			-1, 12, -1, -1
		};
		short[] mainAttributesAdjust7 = new short[6] { -1, 9, 9, -1, 12, -1 };
		identityInteractConfig = new List<sbyte>();
		dataArray7.Add(new OrganizationMemberItem(78, 363, 4, 3, 10, 15, 5, restrictPrincipalAmount: false, -1, -1, -1, 1, 3, 6, 3, 50, 129, monasticTitleSuffixes7, 1800, 6, 3000, 2250, 0, favoriteClothingIds7, hatedClothingIds7, spouseAnonymousTitles7, canStroll: true, 249, initialAges7, equipment7, clothing7, inventory7, combatSkills7, extraCombatSkillGrids7, resourcesAdjust7, 20000, 12000, 10, 70, 4500, lifeSkillsAdjust7, 4, combatSkillsAdjust7, mainAttributesAdjust7, identityInteractConfig, 3, new ResourceInts(default(int), default(int), default(int), default(int), default(int), default(int), default(int), default(int)), new IntPair[18]
		{
			new IntPair(17, 1),
			new IntPair(8, 1),
			new IntPair(5, 42),
			new IntPair(6, 0),
			new IntPair(15, 1),
			new IntPair(16, 6),
			new IntPair(7, 1),
			new IntPair(14, 30),
			new IntPair(12, 0),
			new IntPair(4, 21),
			new IntPair(3, 1),
			new IntPair(13, 3),
			new IntPair(10, 1),
			new IntPair(2, 1),
			new IntPair(1, 1),
			new IntPair(11, 3),
			new IntPair(0, 12),
			new IntPair(9, 1)
		}, null));
		List<OrganizationMemberItem> dataArray8 = _dataArray;
		int[] monasticTitleSuffixes8 = new int[2] { 369, 370 };
		List<short> favoriteClothingIds8 = new List<short> { 27, 28, 29, 30, 5, 14 };
		List<short> hatedClothingIds8 = new List<short> { 55, 56, 57 };
		int[] spouseAnonymousTitles8 = new int[2] { 371, 372 };
		short[] initialAges8 = new short[4] { 16, 18, 20, 22 };
		PresetEquipmentItemWithProb[] equipment8 = new PresetEquipmentItemWithProb[8]
		{
			new PresetEquipmentItemWithProb("Armor", 72, 75),
			new PresetEquipmentItemWithProb("Armor", 369, 100),
			new PresetEquipmentItemWithProb("Armor", 450, 75),
			new PresetEquipmentItemWithProb("Armor", 198, 75),
			new PresetEquipmentItemWithProb("Accessory", 171, 75),
			new PresetEquipmentItemWithProb("Accessory", 144, 50),
			new PresetEquipmentItemWithProb("Accessory", 198, 50),
			new PresetEquipmentItemWithProb("Carrier", 18, 75)
		};
		PresetEquipmentItem clothing8 = new PresetEquipmentItem("Clothing", 27);
		List<PresetInventoryItem> inventory8 = new List<PresetInventoryItem>
		{
			new PresetInventoryItem("SkillBook", 108, 3, 20),
			new PresetInventoryItem("SkillBook", 18, 1, 10),
			new PresetInventoryItem("Material", 0, 1, 10),
			new PresetInventoryItem("Material", 7, 1, 10),
			new PresetInventoryItem("Material", 7, 1, 20),
			new PresetInventoryItem("Food", 93, 3, 20),
			new PresetInventoryItem("Food", 135, 3, 20),
			new PresetInventoryItem("Medicine", 82, 1, 20),
			new PresetInventoryItem("Medicine", 262, 1, 20)
		};
		List<PresetOrgMemberCombatSkill> combatSkills8 = new List<PresetOrgMemberCombatSkill>
		{
			new PresetOrgMemberCombatSkill(22, 2),
			new PresetOrgMemberCombatSkill(127, 2),
			new PresetOrgMemberCombatSkill(228, 2),
			new PresetOrgMemberCombatSkill(336, 2),
			new PresetOrgMemberCombatSkill(532, 2)
		};
		sbyte[] extraCombatSkillGrids8 = new sbyte[5] { 2, 2, 2, 2, 2 };
		short[] resourcesAdjust8 = new short[8] { -70, -70, -70, -70, -50, -70, -70, -60 };
		short[] lifeSkillsAdjust8 = new short[16]
		{
			-1, -1, 9, -1, 6, -1, -1, 9, -1, -1,
			-1, -1, 12, 2, -1, -1
		};
		short[] combatSkillsAdjust8 = new short[14]
		{
			12, 9, 12, 12, -1, -1, 2, 12, -1, -1,
			-1, 12, -1, -1
		};
		short[] mainAttributesAdjust8 = new short[6] { -1, 9, 9, -1, 12, -1 };
		identityInteractConfig = new List<sbyte>();
		dataArray8.Add(new OrganizationMemberItem(79, 368, 4, 2, 10, 15, 5, restrictPrincipalAmount: false, -1, -1, -1, 0, 2, 5, 2, 50, 129, monasticTitleSuffixes8, 600, 4, 2000, 900, 0, favoriteClothingIds8, hatedClothingIds8, spouseAnonymousTitles8, canStroll: true, 250, initialAges8, equipment8, clothing8, inventory8, combatSkills8, extraCombatSkillGrids8, resourcesAdjust8, 15000, 4000, 5, 80, 1800, lifeSkillsAdjust8, 3, combatSkillsAdjust8, mainAttributesAdjust8, identityInteractConfig, 3, new ResourceInts(default(int), default(int), default(int), default(int), default(int), default(int), default(int), default(int)), new IntPair[18]
		{
			new IntPair(17, 1),
			new IntPair(8, 1),
			new IntPair(5, 30),
			new IntPair(6, 0),
			new IntPair(15, 1),
			new IntPair(16, 6),
			new IntPair(7, 1),
			new IntPair(14, 30),
			new IntPair(12, 0),
			new IntPair(4, 15),
			new IntPair(3, 1),
			new IntPair(13, 3),
			new IntPair(10, 1),
			new IntPair(2, 1),
			new IntPair(1, 1),
			new IntPair(11, 3),
			new IntPair(0, 12),
			new IntPair(9, 1)
		}, null));
		List<OrganizationMemberItem> dataArray9 = _dataArray;
		int[] monasticTitleSuffixes9 = new int[2] { 374, 375 };
		List<short> favoriteClothingIds9 = new List<short> { 27, 28, 29, 30, 5, 14 };
		List<short> hatedClothingIds9 = new List<short> { 55, 56, 57 };
		int[] spouseAnonymousTitles9 = new int[2] { 376, 377 };
		short[] initialAges9 = new short[4] { 13, 14, 15, 16 };
		PresetEquipmentItemWithProb[] equipment9 = new PresetEquipmentItemWithProb[8]
		{
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Armor", 369, 100),
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Armor", 198, 75),
			new PresetEquipmentItemWithProb("Accessory", 171, 75),
			new PresetEquipmentItemWithProb("Accessory", 144, 50),
			new PresetEquipmentItemWithProb("Accessory", -1, 0),
			new PresetEquipmentItemWithProb("Carrier", -1, 0)
		};
		PresetEquipmentItem clothing9 = new PresetEquipmentItem("Clothing", 27);
		List<PresetInventoryItem> inventory9 = new List<PresetInventoryItem>
		{
			new PresetInventoryItem("SkillBook", 108, 3, 20),
			new PresetInventoryItem("SkillBook", 18, 1, 10),
			new PresetInventoryItem("Material", 0, 1, 10),
			new PresetInventoryItem("Material", 7, 1, 10),
			new PresetInventoryItem("Material", 7, 1, 20),
			new PresetInventoryItem("Food", 93, 3, 20),
			new PresetInventoryItem("Food", 135, 3, 20),
			new PresetInventoryItem("Medicine", 82, 1, 20),
			new PresetInventoryItem("Medicine", 262, 1, 20)
		};
		List<PresetOrgMemberCombatSkill> combatSkills9 = new List<PresetOrgMemberCombatSkill>
		{
			new PresetOrgMemberCombatSkill(22, 1),
			new PresetOrgMemberCombatSkill(127, 1),
			new PresetOrgMemberCombatSkill(228, 1),
			new PresetOrgMemberCombatSkill(336, 1)
		};
		sbyte[] extraCombatSkillGrids9 = new sbyte[5];
		short[] resourcesAdjust9 = new short[8] { -70, -70, -70, -70, -50, -70, -70, -60 };
		short[] lifeSkillsAdjust9 = new short[16]
		{
			-1, -1, 9, -1, 6, -1, -1, 9, -1, -1,
			-1, -1, 12, 2, -1, -1
		};
		short[] combatSkillsAdjust9 = new short[14]
		{
			12, 9, 12, 12, -1, -1, 2, 12, -1, -1,
			-1, 12, -1, -1
		};
		short[] mainAttributesAdjust9 = new short[6] { -1, 9, 9, -1, 12, -1 };
		identityInteractConfig = new List<sbyte>();
		dataArray9.Add(new OrganizationMemberItem(80, 373, 4, 1, 6, 9, 3, restrictPrincipalAmount: false, -1, -1, -1, 0, 1, 5, 1, 50, 129, monasticTitleSuffixes9, 300, 2, 1000, 300, 0, favoriteClothingIds9, hatedClothingIds9, spouseAnonymousTitles9, canStroll: true, 251, initialAges9, equipment9, clothing9, inventory9, combatSkills9, extraCombatSkillGrids9, resourcesAdjust9, 10000, 2000, 5, 90, 600, lifeSkillsAdjust9, 2, combatSkillsAdjust9, mainAttributesAdjust9, identityInteractConfig, 3, new ResourceInts(default(int), default(int), default(int), default(int), default(int), default(int), default(int), default(int)), new IntPair[18]
		{
			new IntPair(17, 1),
			new IntPair(8, 1),
			new IntPair(5, 30),
			new IntPair(6, 0),
			new IntPair(15, 1),
			new IntPair(16, 6),
			new IntPair(7, 1),
			new IntPair(14, 30),
			new IntPair(12, 0),
			new IntPair(4, 15),
			new IntPair(3, 1),
			new IntPair(13, 3),
			new IntPair(10, 1),
			new IntPair(2, 1),
			new IntPair(1, 1),
			new IntPair(11, 3),
			new IntPair(0, 12),
			new IntPair(9, 1)
		}, null));
		List<OrganizationMemberItem> dataArray10 = _dataArray;
		int[] monasticTitleSuffixes10 = new int[2] { 379, 380 };
		List<short> favoriteClothingIds10 = new List<short> { 27, 28, 29, 30, 5, 14 };
		List<short> hatedClothingIds10 = new List<short> { 55, 56, 57 };
		int[] spouseAnonymousTitles10 = new int[2] { 381, 382 };
		short[] initialAges10 = new short[4] { 10, 10, 10, 10 };
		PresetEquipmentItemWithProb[] equipment10 = new PresetEquipmentItemWithProb[8]
		{
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Armor", 369, 100),
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Armor", 198, 75),
			new PresetEquipmentItemWithProb("Accessory", 171, 75),
			new PresetEquipmentItemWithProb("Accessory", -1, 0),
			new PresetEquipmentItemWithProb("Accessory", -1, 0),
			new PresetEquipmentItemWithProb("Carrier", -1, 0)
		};
		PresetEquipmentItem clothing10 = new PresetEquipmentItem("Clothing", 27);
		List<PresetInventoryItem> inventory10 = new List<PresetInventoryItem>
		{
			new PresetInventoryItem("SkillBook", 108, 3, 20),
			new PresetInventoryItem("SkillBook", 18, 1, 10),
			new PresetInventoryItem("Material", 0, 1, 10),
			new PresetInventoryItem("Material", 7, 1, 10),
			new PresetInventoryItem("Material", 7, 1, 20),
			new PresetInventoryItem("Food", 93, 3, 20),
			new PresetInventoryItem("Food", 135, 3, 20),
			new PresetInventoryItem("Medicine", 82, 1, 20),
			new PresetInventoryItem("Medicine", 262, 1, 20)
		};
		List<PresetOrgMemberCombatSkill> combatSkills10 = new List<PresetOrgMemberCombatSkill>
		{
			new PresetOrgMemberCombatSkill(22, 0),
			new PresetOrgMemberCombatSkill(127, 0),
			new PresetOrgMemberCombatSkill(336, 0)
		};
		sbyte[] extraCombatSkillGrids10 = new sbyte[5];
		short[] resourcesAdjust10 = new short[8] { -70, -70, -70, -70, -50, -70, -70, -60 };
		short[] lifeSkillsAdjust10 = new short[16]
		{
			-1, -1, 9, -1, 6, -1, -1, 9, -1, -1,
			-1, -1, 12, 2, -1, -1
		};
		short[] combatSkillsAdjust10 = new short[14]
		{
			12, 9, 12, 12, -1, -1, 2, 12, -1, -1,
			-1, 12, -1, -1
		};
		short[] mainAttributesAdjust10 = new short[6] { -1, 9, 9, -1, 12, -1 };
		identityInteractConfig = new List<sbyte>();
		dataArray10.Add(new OrganizationMemberItem(81, 378, 4, 0, 6, 9, 3, restrictPrincipalAmount: false, -1, -1, -1, 0, 0, 5, 0, 50, 129, monasticTitleSuffixes10, 150, 0, 500, 150, 0, favoriteClothingIds10, hatedClothingIds10, spouseAnonymousTitles10, canStroll: true, 252, initialAges10, equipment10, clothing10, inventory10, combatSkills10, extraCombatSkillGrids10, resourcesAdjust10, 5000, 1000, 1, 100, 300, lifeSkillsAdjust10, 1, combatSkillsAdjust10, mainAttributesAdjust10, identityInteractConfig, 3, new ResourceInts(default(int), default(int), default(int), default(int), default(int), default(int), default(int), default(int)), new IntPair[18]
		{
			new IntPair(17, 1),
			new IntPair(8, 1),
			new IntPair(5, 30),
			new IntPair(6, 0),
			new IntPair(15, 1),
			new IntPair(16, 6),
			new IntPair(7, 1),
			new IntPair(14, 30),
			new IntPair(12, 0),
			new IntPair(4, 15),
			new IntPair(3, 1),
			new IntPair(13, 3),
			new IntPair(10, 1),
			new IntPair(2, 1),
			new IntPair(1, 1),
			new IntPair(11, 3),
			new IntPair(0, 12),
			new IntPair(9, 1)
		}, null));
		_dataArray.Add(new OrganizationMemberItem(82, 383, 5, 8, 1, 1, 1, restrictPrincipalAmount: true, -1, -1, 7, 4, 7, -1, 7, 0, 0, new int[2] { 384, 385 }, 15800, 16, 13000, 30750, 0, new List<short> { 31, 32, 33, 0, 1, 2, 9, 10, 11 }, new List<short> { 52, 53, 54 }, new int[2] { 386, 387 }, canStroll: false, 253, new short[4] { 42, 50, 58, 66 }, new PresetEquipmentItemWithProb[8]
		{
			new PresetEquipmentItemWithProb("Armor", 63, 75),
			new PresetEquipmentItemWithProb("Armor", 279, 100),
			new PresetEquipmentItemWithProb("Armor", 441, 75),
			new PresetEquipmentItemWithProb("Armor", 126, 75),
			new PresetEquipmentItemWithProb("Accessory", 180, 75),
			new PresetEquipmentItemWithProb("Accessory", 189, 50),
			new PresetEquipmentItemWithProb("Accessory", 90, 75),
			new PresetEquipmentItemWithProb("Carrier", 0, 75)
		}, new PresetEquipmentItem("Clothing", 33), new List<PresetInventoryItem>
		{
			new PresetInventoryItem("SkillBook", 117, 1, 40),
			new PresetInventoryItem("SkillBook", 108, 1, 40),
			new PresetInventoryItem("Food", 93, 3, 40),
			new PresetInventoryItem("Medicine", 60, 2, 20),
			new PresetInventoryItem("Medicine", 72, 2, 20)
		}, new List<PresetOrgMemberCombatSkill>
		{
			new PresetOrgMemberCombatSkill(30, 4),
			new PresetOrgMemberCombatSkill(134, 3),
			new PresetOrgMemberCombatSkill(236, 6),
			new PresetOrgMemberCombatSkill(541, 7),
			new PresetOrgMemberCombatSkill(581, 7),
			new PresetOrgMemberCombatSkill(469, 8)
		}, new sbyte[5] { 10, 10, 10, 10, 10 }, new short[8] { -70, -70, -50, -70, -70, -80, -70, -50 }, 80000, 288000, 20, 20, 61500, new short[16]
		{
			-1, -1, -1, -1, -1, 2, -1, -1, 6, 6,
			-1, -1, 6, 6, 6, -1
		}, 8, new short[14]
		{
			6, 6, 9, -1, -1, 12, -1, 12, 12, -1,
			-1, -1, -1, 2
		}, new short[6] { 9, -1, 12, 12, -1, 2 }, new List<sbyte> { 13, 24, 25, 55 }, 3, new ResourceInts(default(int), default(int), default(int), default(int), default(int), default(int), default(int), default(int)), new IntPair[18]
		{
			new IntPair(17, 1),
			new IntPair(8, 1),
			new IntPair(5, 24),
			new IntPair(6, 24),
			new IntPair(15, 1),
			new IntPair(16, 1),
			new IntPair(7, 1),
			new IntPair(14, 3),
			new IntPair(12, 15),
			new IntPair(4, 1),
			new IntPair(3, 15),
			new IntPair(13, 3),
			new IntPair(10, 3),
			new IntPair(2, 3),
			new IntPair(1, 3),
			new IntPair(11, 9),
			new IntPair(0, 18),
			new IntPair(9, 18)
		}, null));
		_dataArray.Add(new OrganizationMemberItem(83, 229, 5, 7, 6, 9, 3, restrictPrincipalAmount: false, -1, -1, -1, 3, 7, -1, 7, 0, 0, new int[2] { 388, 389 }, 8600, 14, 10000, 21150, 0, new List<short> { 31, 32, 33, 0, 1, 2, 9, 10, 11 }, new List<short> { 52, 53, 54 }, new int[2] { 390, 391 }, canStroll: false, 254, new short[4] { 38, 45, 52, 59 }, new PresetEquipmentItemWithProb[8]
		{
			new PresetEquipmentItemWithProb("Armor", 63, 75),
			new PresetEquipmentItemWithProb("Armor", 279, 100),
			new PresetEquipmentItemWithProb("Armor", 441, 75),
			new PresetEquipmentItemWithProb("Armor", 126, 75),
			new PresetEquipmentItemWithProb("Accessory", 180, 75),
			new PresetEquipmentItemWithProb("Accessory", 189, 50),
			new PresetEquipmentItemWithProb("Accessory", 90, 75),
			new PresetEquipmentItemWithProb("Carrier", 0, 75)
		}, new PresetEquipmentItem("Clothing", 32), new List<PresetInventoryItem>
		{
			new PresetInventoryItem("SkillBook", 117, 1, 40),
			new PresetInventoryItem("SkillBook", 108, 1, 40),
			new PresetInventoryItem("Food", 93, 3, 40),
			new PresetInventoryItem("Medicine", 60, 2, 20),
			new PresetInventoryItem("Medicine", 72, 2, 20)
		}, new List<PresetOrgMemberCombatSkill>
		{
			new PresetOrgMemberCombatSkill(30, 4),
			new PresetOrgMemberCombatSkill(134, 3),
			new PresetOrgMemberCombatSkill(236, 6),
			new PresetOrgMemberCombatSkill(541, 6),
			new PresetOrgMemberCombatSkill(581, 6),
			new PresetOrgMemberCombatSkill(469, 7)
		}, new sbyte[5] { 8, 8, 8, 8, 8 }, new short[8] { -70, -70, -50, -70, -70, -80, -70, -50 }, 50000, 144000, 15, 30, 42300, new short[16]
		{
			-1, -1, -1, -1, -1, 2, -1, -1, 6, 6,
			-1, -1, 6, 6, 6, -1
		}, 8, new short[14]
		{
			6, 6, 9, -1, -1, 12, -1, 12, 12, -1,
			-1, -1, -1, 2
		}, new short[6] { 9, -1, 12, 12, -1, 2 }, new List<sbyte> { 13, 24, 25 }, 3, new ResourceInts(default(int), default(int), default(int), default(int), default(int), default(int), default(int), default(int)), new IntPair[18]
		{
			new IntPair(17, 1),
			new IntPair(8, 1),
			new IntPair(5, 24),
			new IntPair(6, 24),
			new IntPair(15, 1),
			new IntPair(16, 1),
			new IntPair(7, 1),
			new IntPair(14, 3),
			new IntPair(12, 15),
			new IntPair(4, 1),
			new IntPair(3, 15),
			new IntPair(13, 3),
			new IntPair(10, 3),
			new IntPair(2, 3),
			new IntPair(1, 3),
			new IntPair(11, 9),
			new IntPair(0, 18),
			new IntPair(9, 18)
		}, null));
		_dataArray.Add(new OrganizationMemberItem(84, 392, 5, 6, 6, 9, 3, restrictPrincipalAmount: false, -1, -1, -1, 2, 6, 8, 6, 0, 0, new int[2] { 393, 394 }, 6100, 12, 8000, 13800, 0, new List<short> { 31, 32, 33, 0, 1, 2, 9, 10, 11 }, new List<short> { 52, 53, 54 }, new int[2] { 395, 396 }, canStroll: true, 255, new short[4] { 34, 40, 46, 52 }, new PresetEquipmentItemWithProb[8]
		{
			new PresetEquipmentItemWithProb("Armor", 63, 75),
			new PresetEquipmentItemWithProb("Armor", 279, 100),
			new PresetEquipmentItemWithProb("Armor", 441, 75),
			new PresetEquipmentItemWithProb("Armor", 126, 75),
			new PresetEquipmentItemWithProb("Accessory", 180, 75),
			new PresetEquipmentItemWithProb("Accessory", 189, 50),
			new PresetEquipmentItemWithProb("Accessory", 90, 75),
			new PresetEquipmentItemWithProb("Carrier", 0, 75)
		}, new PresetEquipmentItem("Clothing", 32), new List<PresetInventoryItem>
		{
			new PresetInventoryItem("SkillBook", 117, 1, 40),
			new PresetInventoryItem("SkillBook", 108, 1, 40),
			new PresetInventoryItem("Food", 93, 3, 40),
			new PresetInventoryItem("Medicine", 60, 2, 20),
			new PresetInventoryItem("Medicine", 72, 2, 20)
		}, new List<PresetOrgMemberCombatSkill>
		{
			new PresetOrgMemberCombatSkill(30, 3),
			new PresetOrgMemberCombatSkill(134, 3),
			new PresetOrgMemberCombatSkill(236, 5),
			new PresetOrgMemberCombatSkill(541, 6),
			new PresetOrgMemberCombatSkill(581, 6),
			new PresetOrgMemberCombatSkill(469, 6)
		}, new sbyte[5] { 8, 8, 8, 8, 8 }, new short[8] { -70, -70, -50, -70, -70, -80, -70, -50 }, 30000, 72000, 15, 40, 27600, new short[16]
		{
			-1, -1, -1, -1, -1, 2, -1, -1, 6, 6,
			-1, -1, 6, 6, 6, -1
		}, 7, new short[14]
		{
			6, 6, 9, -1, -1, 12, -1, 12, 12, -1,
			-1, -1, -1, 2
		}, new short[6] { 9, -1, 12, 12, -1, 2 }, new List<sbyte> { 13, 24, 25 }, 3, new ResourceInts(default(int), default(int), default(int), default(int), default(int), default(int), default(int), default(int)), new IntPair[18]
		{
			new IntPair(17, 1),
			new IntPair(8, 1),
			new IntPair(5, 24),
			new IntPair(6, 24),
			new IntPair(15, 1),
			new IntPair(16, 1),
			new IntPair(7, 1),
			new IntPair(14, 3),
			new IntPair(12, 15),
			new IntPair(4, 1),
			new IntPair(3, 15),
			new IntPair(13, 3),
			new IntPair(10, 3),
			new IntPair(2, 3),
			new IntPair(1, 3),
			new IntPair(11, 9),
			new IntPair(0, 18),
			new IntPair(9, 18)
		}, null));
		List<OrganizationMemberItem> dataArray11 = _dataArray;
		int[] monasticTitleSuffixes11 = new int[2] { 398, 399 };
		List<short> favoriteClothingIds11 = new List<short> { 31, 32, 33, 0, 1, 2, 9, 10, 11 };
		List<short> hatedClothingIds11 = new List<short> { 52, 53, 54 };
		int[] spouseAnonymousTitles11 = new int[2] { 400, 401 };
		short[] initialAges11 = new short[4] { 30, 35, 40, 45 };
		PresetEquipmentItemWithProb[] equipment11 = new PresetEquipmentItemWithProb[8]
		{
			new PresetEquipmentItemWithProb("Armor", 63, 75),
			new PresetEquipmentItemWithProb("Armor", 279, 100),
			new PresetEquipmentItemWithProb("Armor", 441, 75),
			new PresetEquipmentItemWithProb("Armor", 126, 75),
			new PresetEquipmentItemWithProb("Accessory", 180, 75),
			new PresetEquipmentItemWithProb("Accessory", 189, 50),
			new PresetEquipmentItemWithProb("Accessory", 90, 75),
			new PresetEquipmentItemWithProb("Carrier", 0, 75)
		};
		PresetEquipmentItem clothing11 = new PresetEquipmentItem("Clothing", 32);
		List<PresetInventoryItem> inventory11 = new List<PresetInventoryItem>
		{
			new PresetInventoryItem("SkillBook", 117, 1, 30),
			new PresetInventoryItem("SkillBook", 108, 1, 30),
			new PresetInventoryItem("Food", 93, 3, 30),
			new PresetInventoryItem("Medicine", 54, 2, 20),
			new PresetInventoryItem("Medicine", 66, 2, 20)
		};
		List<PresetOrgMemberCombatSkill> combatSkills11 = new List<PresetOrgMemberCombatSkill>
		{
			new PresetOrgMemberCombatSkill(30, 3),
			new PresetOrgMemberCombatSkill(134, 2),
			new PresetOrgMemberCombatSkill(236, 5),
			new PresetOrgMemberCombatSkill(541, 5),
			new PresetOrgMemberCombatSkill(581, 5),
			new PresetOrgMemberCombatSkill(469, 5)
		};
		sbyte[] extraCombatSkillGrids11 = new sbyte[5] { 6, 6, 6, 6, 6 };
		short[] resourcesAdjust11 = new short[8] { -70, -70, -50, -70, -70, -80, -70, -50 };
		short[] lifeSkillsAdjust11 = new short[16]
		{
			-1, -1, -1, -1, -1, 2, -1, -1, 6, 6,
			-1, -1, 6, 6, 6, -1
		};
		short[] combatSkillsAdjust11 = new short[14]
		{
			6, 6, 9, -1, -1, 12, -1, 12, 12, -1,
			-1, -1, -1, 2
		};
		short[] mainAttributesAdjust11 = new short[6] { 9, -1, 12, 12, -1, 2 };
		identityInteractConfig = new List<sbyte>();
		dataArray11.Add(new OrganizationMemberItem(85, 397, 5, 5, 6, 9, 3, restrictPrincipalAmount: false, -1, -1, -1, 1, 5, 7, 5, 0, 0, monasticTitleSuffixes11, 4200, 10, 6000, 8400, 0, favoriteClothingIds11, hatedClothingIds11, spouseAnonymousTitles11, canStroll: true, 256, initialAges11, equipment11, clothing11, inventory11, combatSkills11, extraCombatSkillGrids11, resourcesAdjust11, 20000, 24000, 10, 50, 16800, lifeSkillsAdjust11, 6, combatSkillsAdjust11, mainAttributesAdjust11, identityInteractConfig, 3, new ResourceInts(default(int), default(int), default(int), default(int), default(int), default(int), default(int), default(int)), new IntPair[18]
		{
			new IntPair(17, 1),
			new IntPair(8, 1),
			new IntPair(5, 15),
			new IntPair(6, 15),
			new IntPair(15, 1),
			new IntPair(16, 1),
			new IntPair(7, 1),
			new IntPair(14, 3),
			new IntPair(12, 12),
			new IntPair(4, 1),
			new IntPair(3, 12),
			new IntPair(13, 3),
			new IntPair(10, 3),
			new IntPair(2, 3),
			new IntPair(1, 3),
			new IntPair(11, 9),
			new IntPair(0, 21),
			new IntPair(9, 21)
		}, null));
		List<OrganizationMemberItem> dataArray12 = _dataArray;
		int[] monasticTitleSuffixes12 = new int[2] { 403, 404 };
		List<short> favoriteClothingIds12 = new List<short> { 31, 32, 33, 0, 1, 2, 9, 10, 11 };
		List<short> hatedClothingIds12 = new List<short> { 52, 53, 54 };
		int[] spouseAnonymousTitles12 = new int[2] { 405, 406 };
		short[] initialAges12 = new short[4] { 26, 30, 34, 38 };
		PresetEquipmentItemWithProb[] equipment12 = new PresetEquipmentItemWithProb[8]
		{
			new PresetEquipmentItemWithProb("Armor", 63, 75),
			new PresetEquipmentItemWithProb("Armor", 279, 100),
			new PresetEquipmentItemWithProb("Armor", 441, 75),
			new PresetEquipmentItemWithProb("Armor", 126, 75),
			new PresetEquipmentItemWithProb("Accessory", 180, 75),
			new PresetEquipmentItemWithProb("Accessory", 189, 50),
			new PresetEquipmentItemWithProb("Accessory", 90, 75),
			new PresetEquipmentItemWithProb("Carrier", 0, 75)
		};
		PresetEquipmentItem clothing12 = new PresetEquipmentItem("Clothing", 32);
		List<PresetInventoryItem> inventory12 = new List<PresetInventoryItem>
		{
			new PresetInventoryItem("SkillBook", 117, 1, 30),
			new PresetInventoryItem("SkillBook", 108, 1, 30),
			new PresetInventoryItem("Food", 93, 3, 30),
			new PresetInventoryItem("Medicine", 54, 2, 20),
			new PresetInventoryItem("Medicine", 66, 2, 20)
		};
		List<PresetOrgMemberCombatSkill> combatSkills12 = new List<PresetOrgMemberCombatSkill>
		{
			new PresetOrgMemberCombatSkill(30, 3),
			new PresetOrgMemberCombatSkill(134, 2),
			new PresetOrgMemberCombatSkill(236, 4),
			new PresetOrgMemberCombatSkill(541, 4),
			new PresetOrgMemberCombatSkill(581, 4),
			new PresetOrgMemberCombatSkill(469, 4)
		};
		sbyte[] extraCombatSkillGrids12 = new sbyte[5] { 4, 4, 4, 4, 4 };
		short[] resourcesAdjust12 = new short[8] { -70, -70, -50, -70, -70, -80, -70, -50 };
		short[] lifeSkillsAdjust12 = new short[16]
		{
			-1, -1, -1, -1, -1, 2, -1, -1, 6, 6,
			-1, -1, 6, 6, 6, -1
		};
		short[] combatSkillsAdjust12 = new short[14]
		{
			6, 6, 9, -1, -1, 12, -1, 12, 12, -1,
			-1, -1, -1, 2
		};
		short[] mainAttributesAdjust12 = new short[6] { 9, -1, 12, 12, -1, 2 };
		identityInteractConfig = new List<sbyte>();
		dataArray12.Add(new OrganizationMemberItem(86, 402, 5, 4, 6, 9, 3, restrictPrincipalAmount: false, -1, -1, -1, 1, 4, 7, 4, 0, 0, monasticTitleSuffixes12, 2800, 8, 4500, 4650, 0, favoriteClothingIds12, hatedClothingIds12, spouseAnonymousTitles12, canStroll: true, 257, initialAges12, equipment12, clothing12, inventory12, combatSkills12, extraCombatSkillGrids12, resourcesAdjust12, 15000, 12000, 10, 60, 9300, lifeSkillsAdjust12, 5, combatSkillsAdjust12, mainAttributesAdjust12, identityInteractConfig, 3, new ResourceInts(default(int), default(int), default(int), default(int), default(int), default(int), default(int), default(int)), new IntPair[18]
		{
			new IntPair(17, 1),
			new IntPair(8, 1),
			new IntPair(5, 15),
			new IntPair(6, 15),
			new IntPair(15, 1),
			new IntPair(16, 1),
			new IntPair(7, 1),
			new IntPair(14, 3),
			new IntPair(12, 12),
			new IntPair(4, 1),
			new IntPair(3, 12),
			new IntPair(13, 3),
			new IntPair(10, 3),
			new IntPair(2, 3),
			new IntPair(1, 3),
			new IntPair(11, 9),
			new IntPair(0, 21),
			new IntPair(9, 21)
		}, null));
		List<OrganizationMemberItem> dataArray13 = _dataArray;
		int[] monasticTitleSuffixes13 = new int[2] { 408, 409 };
		List<short> favoriteClothingIds13 = new List<short> { 31, 32, 33, 0, 1, 2, 9, 10, 11 };
		List<short> hatedClothingIds13 = new List<short> { 52, 53, 54 };
		int[] spouseAnonymousTitles13 = new int[2] { 410, 411 };
		short[] initialAges13 = new short[4] { 22, 25, 28, 31 };
		PresetEquipmentItemWithProb[] equipment13 = new PresetEquipmentItemWithProb[8]
		{
			new PresetEquipmentItemWithProb("Armor", 63, 75),
			new PresetEquipmentItemWithProb("Armor", 279, 100),
			new PresetEquipmentItemWithProb("Armor", 441, 75),
			new PresetEquipmentItemWithProb("Armor", 126, 75),
			new PresetEquipmentItemWithProb("Accessory", 180, 75),
			new PresetEquipmentItemWithProb("Accessory", 189, 50),
			new PresetEquipmentItemWithProb("Accessory", 90, 75),
			new PresetEquipmentItemWithProb("Carrier", 0, 75)
		};
		PresetEquipmentItem clothing13 = new PresetEquipmentItem("Clothing", 31);
		List<PresetInventoryItem> inventory13 = new List<PresetInventoryItem>
		{
			new PresetInventoryItem("SkillBook", 117, 1, 30),
			new PresetInventoryItem("SkillBook", 108, 1, 30),
			new PresetInventoryItem("Food", 93, 3, 30),
			new PresetInventoryItem("Medicine", 54, 2, 20),
			new PresetInventoryItem("Medicine", 66, 2, 20)
		};
		List<PresetOrgMemberCombatSkill> combatSkills13 = new List<PresetOrgMemberCombatSkill>
		{
			new PresetOrgMemberCombatSkill(30, 2),
			new PresetOrgMemberCombatSkill(134, 2),
			new PresetOrgMemberCombatSkill(236, 3),
			new PresetOrgMemberCombatSkill(541, 3),
			new PresetOrgMemberCombatSkill(581, 3)
		};
		sbyte[] extraCombatSkillGrids13 = new sbyte[5] { 4, 4, 4, 4, 4 };
		short[] resourcesAdjust13 = new short[8] { -70, -70, -50, -70, -70, -80, -70, -50 };
		short[] lifeSkillsAdjust13 = new short[16]
		{
			-1, -1, -1, -1, -1, 2, -1, -1, 6, 6,
			-1, -1, 6, 6, 6, -1
		};
		short[] combatSkillsAdjust13 = new short[14]
		{
			6, 6, 9, -1, -1, 12, -1, 12, 12, -1,
			-1, -1, -1, 2
		};
		short[] mainAttributesAdjust13 = new short[6] { 9, -1, 12, 12, -1, 2 };
		identityInteractConfig = new List<sbyte>();
		dataArray13.Add(new OrganizationMemberItem(87, 407, 5, 3, 8, 12, 4, restrictPrincipalAmount: false, -1, -1, -1, 0, 3, 5, 3, 0, 0, monasticTitleSuffixes13, 1800, 6, 3000, 2250, 0, favoriteClothingIds13, hatedClothingIds13, spouseAnonymousTitles13, canStroll: true, 258, initialAges13, equipment13, clothing13, inventory13, combatSkills13, extraCombatSkillGrids13, resourcesAdjust13, 10000, 6000, 10, 70, 4500, lifeSkillsAdjust13, 4, combatSkillsAdjust13, mainAttributesAdjust13, identityInteractConfig, 3, new ResourceInts(default(int), default(int), default(int), default(int), default(int), default(int), default(int), default(int)), new IntPair[18]
		{
			new IntPair(17, 1),
			new IntPair(8, 1),
			new IntPair(5, 15),
			new IntPair(6, 15),
			new IntPair(15, 1),
			new IntPair(16, 1),
			new IntPair(7, 1),
			new IntPair(14, 3),
			new IntPair(12, 12),
			new IntPair(4, 1),
			new IntPair(3, 12),
			new IntPair(13, 3),
			new IntPair(10, 3),
			new IntPair(2, 3),
			new IntPair(1, 3),
			new IntPair(11, 9),
			new IntPair(0, 21),
			new IntPair(9, 21)
		}, null));
		List<OrganizationMemberItem> dataArray14 = _dataArray;
		int[] monasticTitleSuffixes14 = new int[2] { 413, 414 };
		List<short> favoriteClothingIds14 = new List<short> { 31, 32, 33, 0, 1, 2, 9, 10, 11 };
		List<short> hatedClothingIds14 = new List<short> { 52, 53, 54 };
		int[] spouseAnonymousTitles14 = new int[2] { 415, 416 };
		short[] initialAges14 = new short[4] { 18, 20, 22, 24 };
		PresetEquipmentItemWithProb[] equipment14 = new PresetEquipmentItemWithProb[8]
		{
			new PresetEquipmentItemWithProb("Armor", 63, 75),
			new PresetEquipmentItemWithProb("Armor", 279, 100),
			new PresetEquipmentItemWithProb("Armor", 441, 75),
			new PresetEquipmentItemWithProb("Armor", 126, 75),
			new PresetEquipmentItemWithProb("Accessory", 180, 75),
			new PresetEquipmentItemWithProb("Accessory", 189, 50),
			new PresetEquipmentItemWithProb("Accessory", 90, 75),
			new PresetEquipmentItemWithProb("Carrier", 0, 75)
		};
		PresetEquipmentItem clothing14 = new PresetEquipmentItem("Clothing", 31);
		List<PresetInventoryItem> inventory14 = new List<PresetInventoryItem>
		{
			new PresetInventoryItem("SkillBook", 117, 1, 20),
			new PresetInventoryItem("SkillBook", 108, 1, 20),
			new PresetInventoryItem("Food", 93, 3, 20)
		};
		List<PresetOrgMemberCombatSkill> combatSkills14 = new List<PresetOrgMemberCombatSkill>
		{
			new PresetOrgMemberCombatSkill(30, 2),
			new PresetOrgMemberCombatSkill(134, 1),
			new PresetOrgMemberCombatSkill(236, 2),
			new PresetOrgMemberCombatSkill(541, 2),
			new PresetOrgMemberCombatSkill(581, 2)
		};
		sbyte[] extraCombatSkillGrids14 = new sbyte[5] { 2, 2, 2, 2, 2 };
		short[] resourcesAdjust14 = new short[8] { -70, -70, -50, -70, -70, -80, -70, -50 };
		short[] lifeSkillsAdjust14 = new short[16]
		{
			-1, -1, -1, -1, -1, 2, -1, -1, 6, 6,
			-1, -1, 6, 6, 6, -1
		};
		short[] combatSkillsAdjust14 = new short[14]
		{
			6, 6, 9, -1, -1, 12, -1, 12, 12, -1,
			-1, -1, -1, 2
		};
		short[] mainAttributesAdjust14 = new short[6] { 9, -1, 12, 12, -1, 2 };
		identityInteractConfig = new List<sbyte>();
		dataArray14.Add(new OrganizationMemberItem(88, 412, 5, 2, 8, 12, 4, restrictPrincipalAmount: false, -1, -1, -1, 0, 2, 4, 2, 0, 0, monasticTitleSuffixes14, 600, 4, 2000, 900, 0, favoriteClothingIds14, hatedClothingIds14, spouseAnonymousTitles14, canStroll: true, 259, initialAges14, equipment14, clothing14, inventory14, combatSkills14, extraCombatSkillGrids14, resourcesAdjust14, 7500, 2000, 5, 80, 1800, lifeSkillsAdjust14, 3, combatSkillsAdjust14, mainAttributesAdjust14, identityInteractConfig, 3, new ResourceInts(default(int), default(int), default(int), default(int), default(int), default(int), default(int), default(int)), new IntPair[18]
		{
			new IntPair(17, 1),
			new IntPair(8, 1),
			new IntPair(5, 6),
			new IntPair(6, 6),
			new IntPair(15, 1),
			new IntPair(16, 1),
			new IntPair(7, 1),
			new IntPair(14, 3),
			new IntPair(12, 9),
			new IntPair(4, 1),
			new IntPair(3, 9),
			new IntPair(13, 3),
			new IntPair(10, 3),
			new IntPair(2, 3),
			new IntPair(1, 3),
			new IntPair(11, 9),
			new IntPair(0, 24),
			new IntPair(9, 24)
		}, null));
		List<OrganizationMemberItem> dataArray15 = _dataArray;
		int[] monasticTitleSuffixes15 = new int[2] { 418, 419 };
		List<short> favoriteClothingIds15 = new List<short> { 31, 32, 33, 0, 1, 2, 9, 10, 11 };
		List<short> hatedClothingIds15 = new List<short> { 52, 53, 54 };
		int[] spouseAnonymousTitles15 = new int[2] { 420, 421 };
		short[] initialAges15 = new short[4] { 14, 15, 16, 17 };
		PresetEquipmentItemWithProb[] equipment15 = new PresetEquipmentItemWithProb[8]
		{
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Armor", 279, 100),
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Armor", 126, 75),
			new PresetEquipmentItemWithProb("Accessory", 180, 75),
			new PresetEquipmentItemWithProb("Accessory", 189, 50),
			new PresetEquipmentItemWithProb("Accessory", -1, 0),
			new PresetEquipmentItemWithProb("Carrier", -1, 0)
		};
		PresetEquipmentItem clothing15 = new PresetEquipmentItem("Clothing", 31);
		List<PresetInventoryItem> inventory15 = new List<PresetInventoryItem>
		{
			new PresetInventoryItem("SkillBook", 117, 1, 20),
			new PresetInventoryItem("SkillBook", 108, 1, 20),
			new PresetInventoryItem("Food", 93, 3, 20)
		};
		List<PresetOrgMemberCombatSkill> combatSkills15 = new List<PresetOrgMemberCombatSkill>
		{
			new PresetOrgMemberCombatSkill(30, 1),
			new PresetOrgMemberCombatSkill(134, 1),
			new PresetOrgMemberCombatSkill(236, 1),
			new PresetOrgMemberCombatSkill(541, 1),
			new PresetOrgMemberCombatSkill(581, 1)
		};
		sbyte[] extraCombatSkillGrids15 = new sbyte[5];
		short[] resourcesAdjust15 = new short[8] { -70, -70, -50, -70, -70, -80, -70, -50 };
		short[] lifeSkillsAdjust15 = new short[16]
		{
			-1, -1, -1, -1, -1, 2, -1, -1, 6, 6,
			-1, -1, 6, 6, 6, -1
		};
		short[] combatSkillsAdjust15 = new short[14]
		{
			6, 6, 9, -1, -1, 12, -1, 12, 12, -1,
			-1, -1, -1, 2
		};
		short[] mainAttributesAdjust15 = new short[6] { 9, -1, 12, 12, -1, 2 };
		identityInteractConfig = new List<sbyte>();
		dataArray15.Add(new OrganizationMemberItem(89, 417, 5, 1, 8, 12, 4, restrictPrincipalAmount: false, -1, -1, -1, 0, 1, 4, 1, 0, 0, monasticTitleSuffixes15, 300, 2, 1000, 300, 0, favoriteClothingIds15, hatedClothingIds15, spouseAnonymousTitles15, canStroll: true, 260, initialAges15, equipment15, clothing15, inventory15, combatSkills15, extraCombatSkillGrids15, resourcesAdjust15, 5000, 1000, 5, 90, 600, lifeSkillsAdjust15, 2, combatSkillsAdjust15, mainAttributesAdjust15, identityInteractConfig, 3, new ResourceInts(default(int), default(int), default(int), default(int), default(int), default(int), default(int), default(int)), new IntPair[18]
		{
			new IntPair(17, 1),
			new IntPair(8, 1),
			new IntPair(5, 6),
			new IntPair(6, 6),
			new IntPair(15, 1),
			new IntPair(16, 1),
			new IntPair(7, 1),
			new IntPair(14, 3),
			new IntPair(12, 9),
			new IntPair(4, 1),
			new IntPair(3, 9),
			new IntPair(13, 3),
			new IntPair(10, 3),
			new IntPair(2, 3),
			new IntPair(1, 3),
			new IntPair(11, 9),
			new IntPair(0, 24),
			new IntPair(9, 24)
		}, null));
		List<OrganizationMemberItem> dataArray16 = _dataArray;
		int[] monasticTitleSuffixes16 = new int[2] { 423, 424 };
		List<short> favoriteClothingIds16 = new List<short> { 31, 32, 33, 0, 1, 2, 9, 10, 11 };
		List<short> hatedClothingIds16 = new List<short> { 52, 53, 54 };
		int[] spouseAnonymousTitles16 = new int[2] { 425, 426 };
		short[] initialAges16 = new short[4] { 10, 10, 10, 10 };
		PresetEquipmentItemWithProb[] equipment16 = new PresetEquipmentItemWithProb[8]
		{
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Armor", 279, 100),
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Armor", 126, 75),
			new PresetEquipmentItemWithProb("Accessory", 180, 75),
			new PresetEquipmentItemWithProb("Accessory", -1, 0),
			new PresetEquipmentItemWithProb("Accessory", -1, 0),
			new PresetEquipmentItemWithProb("Carrier", -1, 0)
		};
		PresetEquipmentItem clothing16 = new PresetEquipmentItem("Clothing", 31);
		List<PresetInventoryItem> inventory16 = new List<PresetInventoryItem>
		{
			new PresetInventoryItem("SkillBook", 117, 1, 20),
			new PresetInventoryItem("SkillBook", 108, 1, 20),
			new PresetInventoryItem("Food", 93, 3, 20)
		};
		List<PresetOrgMemberCombatSkill> combatSkills16 = new List<PresetOrgMemberCombatSkill>
		{
			new PresetOrgMemberCombatSkill(30, 0),
			new PresetOrgMemberCombatSkill(134, 0),
			new PresetOrgMemberCombatSkill(469, 0)
		};
		sbyte[] extraCombatSkillGrids16 = new sbyte[5];
		short[] resourcesAdjust16 = new short[8] { -70, -70, -50, -70, -70, -80, -70, -50 };
		short[] lifeSkillsAdjust16 = new short[16]
		{
			-1, -1, -1, -1, -1, 2, -1, -1, 6, 6,
			-1, -1, 6, 6, 6, -1
		};
		short[] combatSkillsAdjust16 = new short[14]
		{
			6, 6, 9, -1, -1, 12, -1, 12, 12, -1,
			-1, -1, -1, 2
		};
		short[] mainAttributesAdjust16 = new short[6] { 9, -1, 12, 12, -1, 2 };
		identityInteractConfig = new List<sbyte>();
		dataArray16.Add(new OrganizationMemberItem(90, 422, 5, 0, 6, 9, 3, restrictPrincipalAmount: false, -1, -1, -1, 0, 0, 4, 0, 0, 0, monasticTitleSuffixes16, 150, 0, 500, 150, 0, favoriteClothingIds16, hatedClothingIds16, spouseAnonymousTitles16, canStroll: true, 261, initialAges16, equipment16, clothing16, inventory16, combatSkills16, extraCombatSkillGrids16, resourcesAdjust16, 2500, 500, 1, 100, 300, lifeSkillsAdjust16, 1, combatSkillsAdjust16, mainAttributesAdjust16, identityInteractConfig, 3, new ResourceInts(default(int), default(int), default(int), default(int), default(int), default(int), default(int), default(int)), new IntPair[18]
		{
			new IntPair(17, 1),
			new IntPair(8, 1),
			new IntPair(5, 6),
			new IntPair(6, 6),
			new IntPair(15, 1),
			new IntPair(16, 1),
			new IntPair(7, 1),
			new IntPair(14, 3),
			new IntPair(12, 9),
			new IntPair(4, 1),
			new IntPair(3, 9),
			new IntPair(13, 3),
			new IntPair(10, 3),
			new IntPair(2, 3),
			new IntPair(1, 3),
			new IntPair(11, 9),
			new IntPair(0, 24),
			new IntPair(9, 24)
		}, null));
		_dataArray.Add(new OrganizationMemberItem(91, 427, 6, 8, 1, 1, 1, restrictPrincipalAmount: true, -1, 1108, 7, 3, 6, -1, 7, 0, 0, new int[2] { 428, 429 }, 15800, 16, 13000, 30750, 0, new List<short> { 34, 35, 36, 3, 7 }, new List<short> { 43, 44, 45 }, new int[2] { 430, 431 }, canStroll: false, 262, new short[4] { 26, 34, 42, 50 }, new PresetEquipmentItemWithProb[8]
		{
			new PresetEquipmentItemWithProb("Armor", 0, 75),
			new PresetEquipmentItemWithProb("Armor", 261, 100),
			new PresetEquipmentItemWithProb("Armor", 396, 75),
			new PresetEquipmentItemWithProb("Armor", 126, 75),
			new PresetEquipmentItemWithProb("Accessory", 99, 75),
			new PresetEquipmentItemWithProb("Accessory", 153, 50),
			new PresetEquipmentItemWithProb("Accessory", 90, 75),
			new PresetEquipmentItemWithProb("Carrier", 18, 75)
		}, new PresetEquipmentItem("Clothing", 36), new List<PresetInventoryItem>
		{
			new PresetInventoryItem("Material", 14, 1, 30),
			new PresetInventoryItem("Material", 21, 1, 30),
			new PresetInventoryItem("Food", 9, 3, 40),
			new PresetInventoryItem("Food", 51, 3, 40),
			new PresetInventoryItem("Medicine", 60, 1, 20),
			new PresetInventoryItem("Medicine", 66, 1, 20),
			new PresetInventoryItem("Medicine", 340, 1, 20),
			new PresetInventoryItem("Medicine", 232, 1, 20),
			new PresetInventoryItem("Medicine", 280, 1, 20),
			new PresetInventoryItem("Medicine", 208, 1, 30),
			new PresetInventoryItem("TeaWine", 0, 1, 20)
		}, new List<PresetOrgMemberCombatSkill>
		{
			new PresetOrgMemberCombatSkill(35, 4),
			new PresetOrgMemberCombatSkill(138, 3),
			new PresetOrgMemberCombatSkill(243, 5),
			new PresetOrgMemberCombatSkill(345, 8),
			new PresetOrgMemberCombatSkill(589, 8),
			new PresetOrgMemberCombatSkill(632, 8)
		}, new sbyte[5] { 10, 10, 10, 10, 10 }, new short[8] { -70, -70, -60, -70, -80, -70, -60, -50 }, 160000, 576000, 20, 20, 61500, new short[16]
		{
			2, 2, 2, 2, -1, -1, 12, 6, -1, -1,
			-1, -1, -1, -1, 12, 6
		}, 8, new short[14]
		{
			6, 6, 9, 12, -1, -1, 2, -1, 12, 12,
			-1, -1, 2, 2
		}, new short[6] { 12, 9, -1, 12, 2, -1 }, new List<sbyte> { 14, 24, 25, 55 }, 3, new ResourceInts(default(int), default(int), default(int), default(int), default(int), default(int), default(int), default(int)), new IntPair[18]
		{
			new IntPair(17, 3),
			new IntPair(8, 3),
			new IntPair(5, 1),
			new IntPair(6, 1),
			new IntPair(15, 6),
			new IntPair(16, 1),
			new IntPair(7, 21),
			new IntPair(14, 1),
			new IntPair(12, 1),
			new IntPair(4, 1),
			new IntPair(3, 54),
			new IntPair(13, 1),
			new IntPair(10, 1),
			new IntPair(2, 3),
			new IntPair(1, 36),
			new IntPair(11, 3),
			new IntPair(0, 6),
			new IntPair(9, 1)
		}, null));
		_dataArray.Add(new OrganizationMemberItem(92, 432, 6, 7, 4, 6, 2, restrictPrincipalAmount: false, -1, -1, 7, 3, 6, -1, 7, 0, 0, new int[2] { 433, 434 }, 8600, 14, 10000, 21150, 0, new List<short> { 34, 35, 36, 3, 7 }, new List<short> { 43, 44, 45 }, new int[2] { 435, 436 }, canStroll: false, 263, new short[4] { 24, 31, 38, 45 }, new PresetEquipmentItemWithProb[8]
		{
			new PresetEquipmentItemWithProb("Armor", 0, 75),
			new PresetEquipmentItemWithProb("Armor", 261, 100),
			new PresetEquipmentItemWithProb("Armor", 396, 75),
			new PresetEquipmentItemWithProb("Armor", 126, 75),
			new PresetEquipmentItemWithProb("Accessory", 99, 75),
			new PresetEquipmentItemWithProb("Accessory", 153, 50),
			new PresetEquipmentItemWithProb("Accessory", 90, 75),
			new PresetEquipmentItemWithProb("Carrier", 18, 75)
		}, new PresetEquipmentItem("Clothing", 36), new List<PresetInventoryItem>
		{
			new PresetInventoryItem("Material", 14, 1, 30),
			new PresetInventoryItem("Material", 21, 1, 30),
			new PresetInventoryItem("Food", 9, 3, 40),
			new PresetInventoryItem("Food", 51, 3, 40),
			new PresetInventoryItem("Medicine", 60, 1, 20),
			new PresetInventoryItem("Medicine", 66, 1, 20),
			new PresetInventoryItem("Medicine", 340, 1, 20),
			new PresetInventoryItem("Medicine", 232, 1, 20),
			new PresetInventoryItem("Medicine", 280, 1, 20),
			new PresetInventoryItem("Medicine", 208, 1, 30),
			new PresetInventoryItem("TeaWine", 0, 1, 20)
		}, new List<PresetOrgMemberCombatSkill>
		{
			new PresetOrgMemberCombatSkill(35, 4),
			new PresetOrgMemberCombatSkill(138, 3),
			new PresetOrgMemberCombatSkill(243, 5),
			new PresetOrgMemberCombatSkill(345, 7),
			new PresetOrgMemberCombatSkill(589, 7),
			new PresetOrgMemberCombatSkill(632, 7)
		}, new sbyte[5] { 8, 8, 8, 8, 8 }, new short[8] { -70, -70, -60, -70, -80, -70, -60, -50 }, 100000, 288000, 15, 30, 42300, new short[16]
		{
			2, 2, 2, 2, -1, -1, 12, 6, -1, -1,
			-1, -1, -1, -1, 12, 6
		}, 8, new short[14]
		{
			6, 6, 9, 12, -1, -1, 2, -1, 12, 12,
			-1, -1, 2, 2
		}, new short[6] { 12, 9, -1, 12, 2, -1 }, new List<sbyte> { 14, 24, 25 }, 3, new ResourceInts(default(int), default(int), default(int), default(int), default(int), default(int), default(int), default(int)), new IntPair[18]
		{
			new IntPair(17, 3),
			new IntPair(8, 3),
			new IntPair(5, 1),
			new IntPair(6, 1),
			new IntPair(15, 6),
			new IntPair(16, 1),
			new IntPair(7, 21),
			new IntPair(14, 1),
			new IntPair(12, 1),
			new IntPair(4, 1),
			new IntPair(3, 54),
			new IntPair(13, 1),
			new IntPair(10, 1),
			new IntPair(2, 3),
			new IntPair(1, 36),
			new IntPair(11, 3),
			new IntPair(0, 6),
			new IntPair(9, 1)
		}, null));
		_dataArray.Add(new OrganizationMemberItem(93, 437, 6, 6, 6, 9, 3, restrictPrincipalAmount: false, -1, -1, 6, 3, 3, 8, 6, 0, 0, new int[2] { 438, 439 }, 6100, 12, 8000, 13800, 0, new List<short> { 34, 35, 36, 3, 7 }, new List<short> { 43, 44, 45 }, new int[2] { 440, 441 }, canStroll: false, 264, new short[4] { 22, 28, 34, 40 }, new PresetEquipmentItemWithProb[8]
		{
			new PresetEquipmentItemWithProb("Armor", 0, 75),
			new PresetEquipmentItemWithProb("Armor", 261, 100),
			new PresetEquipmentItemWithProb("Armor", 396, 75),
			new PresetEquipmentItemWithProb("Armor", 126, 75),
			new PresetEquipmentItemWithProb("Accessory", 99, 75),
			new PresetEquipmentItemWithProb("Accessory", 153, 50),
			new PresetEquipmentItemWithProb("Accessory", 90, 75),
			new PresetEquipmentItemWithProb("Carrier", 18, 75)
		}, new PresetEquipmentItem("Clothing", 35), new List<PresetInventoryItem>
		{
			new PresetInventoryItem("Material", 14, 1, 30),
			new PresetInventoryItem("Material", 21, 1, 30),
			new PresetInventoryItem("Food", 9, 3, 40),
			new PresetInventoryItem("Food", 51, 3, 40),
			new PresetInventoryItem("Medicine", 60, 1, 20),
			new PresetInventoryItem("Medicine", 66, 1, 20),
			new PresetInventoryItem("Medicine", 340, 1, 20),
			new PresetInventoryItem("Medicine", 232, 1, 20),
			new PresetInventoryItem("Medicine", 280, 1, 20),
			new PresetInventoryItem("Medicine", 208, 1, 30),
			new PresetInventoryItem("TeaWine", 0, 1, 20)
		}, new List<PresetOrgMemberCombatSkill>
		{
			new PresetOrgMemberCombatSkill(35, 3),
			new PresetOrgMemberCombatSkill(138, 2),
			new PresetOrgMemberCombatSkill(243, 4),
			new PresetOrgMemberCombatSkill(589, 6)
		}, new sbyte[5] { 8, 8, 8, 8, 8 }, new short[8] { -70, -70, -60, -70, -80, -70, -60, -50 }, 60000, 144000, 15, 40, 27600, new short[16]
		{
			2, 2, 2, 2, -1, -1, 12, 6, -1, -1,
			-1, -1, -1, -1, 12, 6
		}, 7, new short[14]
		{
			6, 6, 9, 12, -1, -1, 2, -1, 12, 12,
			-1, -1, 2, 2
		}, new short[6] { 12, 9, -1, 12, 2, -1 }, new List<sbyte> { 14, 24, 25 }, 3, new ResourceInts(default(int), default(int), default(int), default(int), default(int), default(int), default(int), default(int)), new IntPair[18]
		{
			new IntPair(17, 3),
			new IntPair(8, 3),
			new IntPair(5, 1),
			new IntPair(6, 1),
			new IntPair(15, 6),
			new IntPair(16, 1),
			new IntPair(7, 21),
			new IntPair(14, 1),
			new IntPair(12, 1),
			new IntPair(4, 1),
			new IntPair(3, 54),
			new IntPair(13, 1),
			new IntPair(10, 1),
			new IntPair(2, 3),
			new IntPair(1, 36),
			new IntPair(11, 3),
			new IntPair(0, 6),
			new IntPair(9, 1)
		}, null));
		List<OrganizationMemberItem> dataArray17 = _dataArray;
		int[] monasticTitleSuffixes17 = new int[2] { 443, 444 };
		List<short> favoriteClothingIds17 = new List<short> { 34, 35, 36, 3, 7 };
		List<short> hatedClothingIds17 = new List<short> { 43, 44, 45 };
		int[] spouseAnonymousTitles17 = new int[2] { 445, 446 };
		short[] initialAges17 = new short[4] { 20, 25, 30, 35 };
		PresetEquipmentItemWithProb[] equipment17 = new PresetEquipmentItemWithProb[8]
		{
			new PresetEquipmentItemWithProb("Armor", 0, 75),
			new PresetEquipmentItemWithProb("Armor", 261, 100),
			new PresetEquipmentItemWithProb("Armor", 396, 75),
			new PresetEquipmentItemWithProb("Armor", 126, 75),
			new PresetEquipmentItemWithProb("Accessory", 99, 75),
			new PresetEquipmentItemWithProb("Accessory", 153, 50),
			new PresetEquipmentItemWithProb("Accessory", 90, 75),
			new PresetEquipmentItemWithProb("Carrier", 18, 75)
		};
		PresetEquipmentItem clothing17 = new PresetEquipmentItem("Clothing", 35);
		List<PresetInventoryItem> inventory17 = new List<PresetInventoryItem>
		{
			new PresetInventoryItem("Material", 14, 1, 20),
			new PresetInventoryItem("Material", 21, 1, 20),
			new PresetInventoryItem("Food", 9, 3, 30),
			new PresetInventoryItem("Food", 51, 3, 30),
			new PresetInventoryItem("Medicine", 60, 1, 20),
			new PresetInventoryItem("Medicine", 66, 1, 20),
			new PresetInventoryItem("Medicine", 334, 1, 10),
			new PresetInventoryItem("Medicine", 226, 1, 10),
			new PresetInventoryItem("Medicine", 274, 1, 10),
			new PresetInventoryItem("Medicine", 202, 1, 30),
			new PresetInventoryItem("TeaWine", 0, 1, 20)
		};
		List<PresetOrgMemberCombatSkill> combatSkills17 = new List<PresetOrgMemberCombatSkill>
		{
			new PresetOrgMemberCombatSkill(35, 3),
			new PresetOrgMemberCombatSkill(138, 2),
			new PresetOrgMemberCombatSkill(243, 4),
			new PresetOrgMemberCombatSkill(632, 5)
		};
		sbyte[] extraCombatSkillGrids17 = new sbyte[5] { 6, 6, 6, 6, 6 };
		short[] resourcesAdjust17 = new short[8] { -70, -70, -60, -70, -80, -70, -60, -50 };
		short[] lifeSkillsAdjust17 = new short[16]
		{
			2, 2, 2, 2, -1, -1, 12, 6, -1, -1,
			-1, -1, -1, -1, 12, 6
		};
		short[] combatSkillsAdjust17 = new short[14]
		{
			6, 6, 9, 12, -1, -1, 2, -1, 12, 12,
			-1, -1, 2, 2
		};
		short[] mainAttributesAdjust17 = new short[6] { 12, 9, -1, 12, 2, -1 };
		identityInteractConfig = new List<sbyte>();
		dataArray17.Add(new OrganizationMemberItem(94, 442, 6, 5, 6, 9, 3, restrictPrincipalAmount: false, -1, -1, 5, 2, 2, 8, 5, 0, 0, monasticTitleSuffixes17, 4200, 10, 6000, 8400, 0, favoriteClothingIds17, hatedClothingIds17, spouseAnonymousTitles17, canStroll: false, 265, initialAges17, equipment17, clothing17, inventory17, combatSkills17, extraCombatSkillGrids17, resourcesAdjust17, 40000, 48000, 10, 50, 16800, lifeSkillsAdjust17, 6, combatSkillsAdjust17, mainAttributesAdjust17, identityInteractConfig, 3, new ResourceInts(default(int), default(int), default(int), default(int), default(int), default(int), default(int), default(int)), new IntPair[18]
		{
			new IntPair(17, 3),
			new IntPair(8, 3),
			new IntPair(5, 1),
			new IntPair(6, 1),
			new IntPair(15, 6),
			new IntPair(16, 1),
			new IntPair(7, 15),
			new IntPair(14, 1),
			new IntPair(12, 1),
			new IntPair(4, 1),
			new IntPair(3, 42),
			new IntPair(13, 1),
			new IntPair(10, 1),
			new IntPair(2, 3),
			new IntPair(1, 36),
			new IntPair(11, 3),
			new IntPair(0, 6),
			new IntPair(9, 1)
		}, null));
		List<OrganizationMemberItem> dataArray18 = _dataArray;
		int[] monasticTitleSuffixes18 = new int[2] { 448, 449 };
		List<short> favoriteClothingIds18 = new List<short> { 34, 35, 36, 3, 7 };
		List<short> hatedClothingIds18 = new List<short> { 43, 44, 45 };
		int[] spouseAnonymousTitles18 = new int[2] { 450, 451 };
		short[] initialAges18 = new short[4] { 18, 22, 26, 30 };
		PresetEquipmentItemWithProb[] equipment18 = new PresetEquipmentItemWithProb[8]
		{
			new PresetEquipmentItemWithProb("Armor", 0, 75),
			new PresetEquipmentItemWithProb("Armor", 261, 100),
			new PresetEquipmentItemWithProb("Armor", 396, 75),
			new PresetEquipmentItemWithProb("Armor", 126, 75),
			new PresetEquipmentItemWithProb("Accessory", 99, 75),
			new PresetEquipmentItemWithProb("Accessory", 153, 50),
			new PresetEquipmentItemWithProb("Accessory", 90, 75),
			new PresetEquipmentItemWithProb("Carrier", 18, 75)
		};
		PresetEquipmentItem clothing18 = new PresetEquipmentItem("Clothing", 35);
		List<PresetInventoryItem> inventory18 = new List<PresetInventoryItem>
		{
			new PresetInventoryItem("Material", 14, 1, 20),
			new PresetInventoryItem("Material", 21, 1, 20),
			new PresetInventoryItem("Food", 9, 3, 30),
			new PresetInventoryItem("Food", 51, 3, 30),
			new PresetInventoryItem("Medicine", 60, 1, 20),
			new PresetInventoryItem("Medicine", 66, 1, 20),
			new PresetInventoryItem("Medicine", 334, 1, 10),
			new PresetInventoryItem("Medicine", 226, 1, 10),
			new PresetInventoryItem("Medicine", 274, 1, 10),
			new PresetInventoryItem("Medicine", 202, 1, 30),
			new PresetInventoryItem("TeaWine", 0, 1, 20)
		};
		List<PresetOrgMemberCombatSkill> combatSkills18 = new List<PresetOrgMemberCombatSkill>
		{
			new PresetOrgMemberCombatSkill(35, 3),
			new PresetOrgMemberCombatSkill(138, 2),
			new PresetOrgMemberCombatSkill(243, 4),
			new PresetOrgMemberCombatSkill(345, 4)
		};
		sbyte[] extraCombatSkillGrids18 = new sbyte[5] { 4, 4, 4, 4, 4 };
		short[] resourcesAdjust18 = new short[8] { -70, -70, -60, -70, -80, -70, -60, -50 };
		short[] lifeSkillsAdjust18 = new short[16]
		{
			2, 2, 2, 2, -1, -1, 12, 6, -1, -1,
			-1, -1, -1, -1, 12, 6
		};
		short[] combatSkillsAdjust18 = new short[14]
		{
			6, 6, 9, 12, -1, -1, 2, -1, 12, 12,
			-1, -1, 2, 2
		};
		short[] mainAttributesAdjust18 = new short[6] { 12, 9, -1, 12, 2, -1 };
		identityInteractConfig = new List<sbyte>();
		dataArray18.Add(new OrganizationMemberItem(95, 447, 6, 4, 6, 9, 3, restrictPrincipalAmount: false, -1, -1, 4, 1, 1, 8, 4, 0, 0, monasticTitleSuffixes18, 2800, 8, 4500, 4650, 0, favoriteClothingIds18, hatedClothingIds18, spouseAnonymousTitles18, canStroll: false, 266, initialAges18, equipment18, clothing18, inventory18, combatSkills18, extraCombatSkillGrids18, resourcesAdjust18, 30000, 24000, 10, 60, 9300, lifeSkillsAdjust18, 5, combatSkillsAdjust18, mainAttributesAdjust18, identityInteractConfig, 3, new ResourceInts(default(int), default(int), default(int), default(int), default(int), default(int), default(int), default(int)), new IntPair[18]
		{
			new IntPair(17, 3),
			new IntPair(8, 3),
			new IntPair(5, 1),
			new IntPair(6, 1),
			new IntPair(15, 6),
			new IntPair(16, 1),
			new IntPair(7, 15),
			new IntPair(14, 1),
			new IntPair(12, 1),
			new IntPair(4, 1),
			new IntPair(3, 42),
			new IntPair(13, 1),
			new IntPair(10, 1),
			new IntPair(2, 3),
			new IntPair(1, 36),
			new IntPair(11, 3),
			new IntPair(0, 6),
			new IntPair(9, 1)
		}, null));
		List<OrganizationMemberItem> dataArray19 = _dataArray;
		int[] monasticTitleSuffixes19 = new int[2] { 453, 454 };
		List<short> favoriteClothingIds19 = new List<short> { 34, 35, 36, 3, 7 };
		List<short> hatedClothingIds19 = new List<short> { 43, 44, 45 };
		int[] spouseAnonymousTitles19 = new int[2] { 455, 456 };
		short[] initialAges19 = new short[4] { 16, 19, 22, 25 };
		PresetEquipmentItemWithProb[] equipment19 = new PresetEquipmentItemWithProb[8]
		{
			new PresetEquipmentItemWithProb("Armor", 0, 75),
			new PresetEquipmentItemWithProb("Armor", 261, 100),
			new PresetEquipmentItemWithProb("Armor", 396, 75),
			new PresetEquipmentItemWithProb("Armor", 126, 75),
			new PresetEquipmentItemWithProb("Accessory", 99, 75),
			new PresetEquipmentItemWithProb("Accessory", 153, 50),
			new PresetEquipmentItemWithProb("Accessory", 90, 75),
			new PresetEquipmentItemWithProb("Carrier", 18, 75)
		};
		PresetEquipmentItem clothing19 = new PresetEquipmentItem("Clothing", 34);
		List<PresetInventoryItem> inventory19 = new List<PresetInventoryItem>
		{
			new PresetInventoryItem("Material", 14, 1, 20),
			new PresetInventoryItem("Material", 21, 1, 20),
			new PresetInventoryItem("Food", 9, 3, 30),
			new PresetInventoryItem("Food", 51, 3, 30),
			new PresetInventoryItem("Medicine", 60, 1, 20),
			new PresetInventoryItem("Medicine", 66, 1, 20),
			new PresetInventoryItem("Medicine", 334, 1, 10),
			new PresetInventoryItem("Medicine", 226, 1, 10),
			new PresetInventoryItem("Medicine", 274, 1, 10),
			new PresetInventoryItem("Medicine", 202, 1, 30),
			new PresetInventoryItem("TeaWine", 0, 1, 20)
		};
		List<PresetOrgMemberCombatSkill> combatSkills19 = new List<PresetOrgMemberCombatSkill>
		{
			new PresetOrgMemberCombatSkill(35, 2),
			new PresetOrgMemberCombatSkill(138, 1),
			new PresetOrgMemberCombatSkill(243, 3),
			new PresetOrgMemberCombatSkill(589, 3)
		};
		sbyte[] extraCombatSkillGrids19 = new sbyte[5] { 4, 4, 4, 4, 4 };
		short[] resourcesAdjust19 = new short[8] { -70, -70, -60, -70, -80, -70, -60, -50 };
		short[] lifeSkillsAdjust19 = new short[16]
		{
			2, 2, 2, 2, -1, -1, 12, 6, -1, -1,
			-1, -1, -1, -1, 12, 6
		};
		short[] combatSkillsAdjust19 = new short[14]
		{
			6, 6, 9, 12, -1, -1, 2, -1, 12, 12,
			-1, -1, 2, 2
		};
		short[] mainAttributesAdjust19 = new short[6] { 12, 9, -1, 12, 2, -1 };
		identityInteractConfig = new List<sbyte>();
		dataArray19.Add(new OrganizationMemberItem(96, 452, 6, 3, 8, 12, 4, restrictPrincipalAmount: false, -1, -1, -1, 0, 3, 6, 3, 0, 0, monasticTitleSuffixes19, 1800, 6, 3000, 2250, 0, favoriteClothingIds19, hatedClothingIds19, spouseAnonymousTitles19, canStroll: true, 267, initialAges19, equipment19, clothing19, inventory19, combatSkills19, extraCombatSkillGrids19, resourcesAdjust19, 20000, 12000, 10, 70, 4500, lifeSkillsAdjust19, 4, combatSkillsAdjust19, mainAttributesAdjust19, identityInteractConfig, 3, new ResourceInts(default(int), default(int), default(int), default(int), default(int), default(int), default(int), default(int)), new IntPair[18]
		{
			new IntPair(17, 3),
			new IntPair(8, 3),
			new IntPair(5, 1),
			new IntPair(6, 1),
			new IntPair(15, 6),
			new IntPair(16, 1),
			new IntPair(7, 15),
			new IntPair(14, 1),
			new IntPair(12, 1),
			new IntPair(4, 1),
			new IntPair(3, 42),
			new IntPair(13, 1),
			new IntPair(10, 1),
			new IntPair(2, 3),
			new IntPair(1, 36),
			new IntPair(11, 3),
			new IntPair(0, 6),
			new IntPair(9, 1)
		}, null));
		List<OrganizationMemberItem> dataArray20 = _dataArray;
		int[] monasticTitleSuffixes20 = new int[2] { 458, 459 };
		List<short> favoriteClothingIds20 = new List<short> { 34, 35, 36, 3, 7 };
		List<short> hatedClothingIds20 = new List<short> { 43, 44, 45 };
		int[] spouseAnonymousTitles20 = new int[2] { 460, 461 };
		short[] initialAges20 = new short[4] { 14, 16, 18, 20 };
		PresetEquipmentItemWithProb[] equipment20 = new PresetEquipmentItemWithProb[8]
		{
			new PresetEquipmentItemWithProb("Armor", 0, 75),
			new PresetEquipmentItemWithProb("Armor", 261, 100),
			new PresetEquipmentItemWithProb("Armor", 396, 75),
			new PresetEquipmentItemWithProb("Armor", 126, 75),
			new PresetEquipmentItemWithProb("Accessory", 99, 75),
			new PresetEquipmentItemWithProb("Accessory", 153, 50),
			new PresetEquipmentItemWithProb("Accessory", 90, 75),
			new PresetEquipmentItemWithProb("Carrier", 18, 75)
		};
		PresetEquipmentItem clothing20 = new PresetEquipmentItem("Clothing", 34);
		List<PresetInventoryItem> inventory20 = new List<PresetInventoryItem>
		{
			new PresetInventoryItem("Material", 14, 1, 10),
			new PresetInventoryItem("Material", 21, 1, 10),
			new PresetInventoryItem("Food", 9, 3, 20),
			new PresetInventoryItem("Food", 51, 3, 20),
			new PresetInventoryItem("Medicine", 54, 1, 20),
			new PresetInventoryItem("Medicine", 202, 1, 20)
		};
		List<PresetOrgMemberCombatSkill> combatSkills20 = new List<PresetOrgMemberCombatSkill>
		{
			new PresetOrgMemberCombatSkill(35, 2),
			new PresetOrgMemberCombatSkill(138, 1),
			new PresetOrgMemberCombatSkill(243, 2),
			new PresetOrgMemberCombatSkill(632, 2)
		};
		sbyte[] extraCombatSkillGrids20 = new sbyte[5] { 2, 2, 2, 2, 2 };
		short[] resourcesAdjust20 = new short[8] { -70, -70, -60, -70, -80, -70, -60, -50 };
		short[] lifeSkillsAdjust20 = new short[16]
		{
			2, 2, 2, 2, -1, -1, 12, 6, -1, -1,
			-1, -1, -1, -1, 12, 6
		};
		short[] combatSkillsAdjust20 = new short[14]
		{
			6, 6, 9, 12, -1, -1, 2, -1, 12, 12,
			-1, -1, 2, 2
		};
		short[] mainAttributesAdjust20 = new short[6] { 12, 9, -1, 12, 2, -1 };
		identityInteractConfig = new List<sbyte>();
		dataArray20.Add(new OrganizationMemberItem(97, 457, 6, 2, 8, 12, 4, restrictPrincipalAmount: false, -1, -1, -1, 0, 2, 5, 2, 0, 0, monasticTitleSuffixes20, 600, 4, 2000, 900, 0, favoriteClothingIds20, hatedClothingIds20, spouseAnonymousTitles20, canStroll: true, 268, initialAges20, equipment20, clothing20, inventory20, combatSkills20, extraCombatSkillGrids20, resourcesAdjust20, 15000, 4000, 5, 80, 1800, lifeSkillsAdjust20, 3, combatSkillsAdjust20, mainAttributesAdjust20, identityInteractConfig, 3, new ResourceInts(default(int), default(int), default(int), default(int), default(int), default(int), default(int), default(int)), new IntPair[18]
		{
			new IntPair(17, 3),
			new IntPair(8, 3),
			new IntPair(5, 1),
			new IntPair(6, 1),
			new IntPair(15, 6),
			new IntPair(16, 1),
			new IntPair(7, 9),
			new IntPair(14, 1),
			new IntPair(12, 1),
			new IntPair(4, 1),
			new IntPair(3, 30),
			new IntPair(13, 1),
			new IntPair(10, 1),
			new IntPair(2, 3),
			new IntPair(1, 36),
			new IntPair(11, 3),
			new IntPair(0, 6),
			new IntPair(9, 1)
		}, null));
		List<OrganizationMemberItem> dataArray21 = _dataArray;
		int[] monasticTitleSuffixes21 = new int[2] { 463, 464 };
		List<short> favoriteClothingIds21 = new List<short> { 34, 35, 36, 3, 7 };
		List<short> hatedClothingIds21 = new List<short> { 43, 44, 45 };
		int[] spouseAnonymousTitles21 = new int[2] { 465, 466 };
		short[] initialAges21 = new short[4] { 12, 13, 14, 15 };
		PresetEquipmentItemWithProb[] equipment21 = new PresetEquipmentItemWithProb[8]
		{
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Armor", 261, 100),
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Armor", 126, 75),
			new PresetEquipmentItemWithProb("Accessory", 99, 75),
			new PresetEquipmentItemWithProb("Accessory", 153, 50),
			new PresetEquipmentItemWithProb("Accessory", -1, 0),
			new PresetEquipmentItemWithProb("Carrier", -1, 0)
		};
		PresetEquipmentItem clothing21 = new PresetEquipmentItem("Clothing", 34);
		List<PresetInventoryItem> inventory21 = new List<PresetInventoryItem>
		{
			new PresetInventoryItem("Material", 14, 1, 10),
			new PresetInventoryItem("Material", 21, 1, 10),
			new PresetInventoryItem("Food", 9, 3, 20),
			new PresetInventoryItem("Food", 51, 3, 20),
			new PresetInventoryItem("Medicine", 54, 1, 20),
			new PresetInventoryItem("Medicine", 202, 1, 20)
		};
		List<PresetOrgMemberCombatSkill> combatSkills21 = new List<PresetOrgMemberCombatSkill>
		{
			new PresetOrgMemberCombatSkill(35, 1),
			new PresetOrgMemberCombatSkill(138, 1),
			new PresetOrgMemberCombatSkill(243, 1),
			new PresetOrgMemberCombatSkill(345, 1)
		};
		sbyte[] extraCombatSkillGrids21 = new sbyte[5];
		short[] resourcesAdjust21 = new short[8] { -70, -70, -60, -70, -80, -70, -60, -50 };
		short[] lifeSkillsAdjust21 = new short[16]
		{
			2, 2, 2, 2, -1, -1, 12, 6, -1, -1,
			-1, -1, -1, -1, 12, 6
		};
		short[] combatSkillsAdjust21 = new short[14]
		{
			6, 6, 9, 12, -1, -1, 2, -1, 12, 12,
			-1, -1, 2, 2
		};
		short[] mainAttributesAdjust21 = new short[6] { 12, 9, -1, 12, 2, -1 };
		identityInteractConfig = new List<sbyte>();
		dataArray21.Add(new OrganizationMemberItem(98, 462, 6, 1, 8, 12, 4, restrictPrincipalAmount: false, -1, -1, -1, 0, 1, 4, 1, 0, 0, monasticTitleSuffixes21, 300, 2, 1000, 300, 0, favoriteClothingIds21, hatedClothingIds21, spouseAnonymousTitles21, canStroll: true, 269, initialAges21, equipment21, clothing21, inventory21, combatSkills21, extraCombatSkillGrids21, resourcesAdjust21, 10000, 2000, 5, 90, 600, lifeSkillsAdjust21, 2, combatSkillsAdjust21, mainAttributesAdjust21, identityInteractConfig, 3, new ResourceInts(default(int), default(int), default(int), default(int), default(int), default(int), default(int), default(int)), new IntPair[18]
		{
			new IntPair(17, 3),
			new IntPair(8, 3),
			new IntPair(5, 1),
			new IntPair(6, 1),
			new IntPair(15, 6),
			new IntPair(16, 1),
			new IntPair(7, 9),
			new IntPair(14, 1),
			new IntPair(12, 1),
			new IntPair(4, 1),
			new IntPair(3, 30),
			new IntPair(13, 1),
			new IntPair(10, 1),
			new IntPair(2, 3),
			new IntPair(1, 36),
			new IntPair(11, 3),
			new IntPair(0, 6),
			new IntPair(9, 1)
		}, null));
		List<OrganizationMemberItem> dataArray22 = _dataArray;
		int[] monasticTitleSuffixes22 = new int[2] { 468, 469 };
		List<short> favoriteClothingIds22 = new List<short> { 34, 35, 36, 3, 7 };
		List<short> hatedClothingIds22 = new List<short> { 43, 44, 45 };
		int[] spouseAnonymousTitles22 = new int[2] { 470, 471 };
		short[] initialAges22 = new short[4] { 10, 10, 10, 10 };
		PresetEquipmentItemWithProb[] equipment22 = new PresetEquipmentItemWithProb[8]
		{
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Armor", 261, 100),
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Armor", 126, 75),
			new PresetEquipmentItemWithProb("Accessory", 99, 75),
			new PresetEquipmentItemWithProb("Accessory", -1, 0),
			new PresetEquipmentItemWithProb("Accessory", -1, 0),
			new PresetEquipmentItemWithProb("Carrier", -1, 0)
		};
		PresetEquipmentItem clothing22 = new PresetEquipmentItem("Clothing", 34);
		List<PresetInventoryItem> inventory22 = new List<PresetInventoryItem>
		{
			new PresetInventoryItem("Material", 14, 1, 10),
			new PresetInventoryItem("Material", 21, 1, 10),
			new PresetInventoryItem("Food", 9, 3, 20),
			new PresetInventoryItem("Food", 51, 3, 20),
			new PresetInventoryItem("Medicine", 54, 1, 20),
			new PresetInventoryItem("Medicine", 202, 1, 20)
		};
		List<PresetOrgMemberCombatSkill> combatSkills22 = new List<PresetOrgMemberCombatSkill>
		{
			new PresetOrgMemberCombatSkill(35, 0),
			new PresetOrgMemberCombatSkill(138, 0),
			new PresetOrgMemberCombatSkill(243, 0),
			new PresetOrgMemberCombatSkill(632, 0)
		};
		sbyte[] extraCombatSkillGrids22 = new sbyte[5];
		short[] resourcesAdjust22 = new short[8] { -70, -70, -60, -70, -80, -70, -60, -50 };
		short[] lifeSkillsAdjust22 = new short[16]
		{
			2, 2, 2, 2, -1, -1, 12, 6, -1, -1,
			-1, -1, -1, -1, 12, 6
		};
		short[] combatSkillsAdjust22 = new short[14]
		{
			6, 6, 9, 12, -1, -1, 2, -1, 12, 12,
			-1, -1, 2, 2
		};
		short[] mainAttributesAdjust22 = new short[6] { 12, 9, -1, 12, 2, -1 };
		identityInteractConfig = new List<sbyte>();
		dataArray22.Add(new OrganizationMemberItem(99, 467, 6, 0, 8, 12, 4, restrictPrincipalAmount: false, -1, -1, -1, 0, 0, 7, 0, 0, 0, monasticTitleSuffixes22, 150, 0, 500, 150, 0, favoriteClothingIds22, hatedClothingIds22, spouseAnonymousTitles22, canStroll: true, 270, initialAges22, equipment22, clothing22, inventory22, combatSkills22, extraCombatSkillGrids22, resourcesAdjust22, 5000, 1000, 1, 100, 300, lifeSkillsAdjust22, 1, combatSkillsAdjust22, mainAttributesAdjust22, identityInteractConfig, 3, new ResourceInts(default(int), default(int), default(int), default(int), default(int), default(int), default(int), default(int)), new IntPair[18]
		{
			new IntPair(17, 3),
			new IntPair(8, 3),
			new IntPair(5, 1),
			new IntPair(6, 1),
			new IntPair(15, 6),
			new IntPair(16, 1),
			new IntPair(7, 9),
			new IntPair(14, 1),
			new IntPair(12, 1),
			new IntPair(4, 1),
			new IntPair(3, 30),
			new IntPair(13, 1),
			new IntPair(10, 1),
			new IntPair(2, 3),
			new IntPair(1, 36),
			new IntPair(11, 3),
			new IntPair(0, 6),
			new IntPair(9, 1)
		}, null));
		_dataArray.Add(new OrganizationMemberItem(100, 472, 7, 8, 1, 1, 1, restrictPrincipalAmount: true, -1, -1, 7, 5, 7, -1, 7, 75, 129, new int[2] { 473, 473 }, 15800, 16, 13000, 30750, 0, new List<short> { 37, 38, 39 }, new List<short> { 34, 35, 36 }, new int[2] { 474, 266 }, canStroll: false, 271, new short[4] { 34, 42, 50, 58 }, new PresetEquipmentItemWithProb[8]
		{
			new PresetEquipmentItemWithProb("Armor", 90, 75),
			new PresetEquipmentItemWithProb("Armor", 360, 100),
			new PresetEquipmentItemWithProb("Armor", 459, 75),
			new PresetEquipmentItemWithProb("Armor", 216, 75),
			new PresetEquipmentItemWithProb("Accessory", 162, 75),
			new PresetEquipmentItemWithProb("Accessory", 207, 50),
			new PresetEquipmentItemWithProb("Accessory", 81, 50),
			new PresetEquipmentItemWithProb("Carrier", 9, 75)
		}, new PresetEquipmentItem("Clothing", 39), new List<PresetInventoryItem>
		{
			new PresetInventoryItem("SkillBook", 108, 3, 40),
			new PresetInventoryItem("SkillBook", 18, 1, 30),
			new PresetInventoryItem("SkillBook", 36, 1, 30),
			new PresetInventoryItem("SkillBook", 135, 1, 30),
			new PresetInventoryItem("Material", 28, 1, 30),
			new PresetInventoryItem("Material", 35, 1, 30),
			new PresetInventoryItem("Food", 93, 3, 40),
			new PresetInventoryItem("Medicine", 54, 1, 20),
			new PresetInventoryItem("Medicine", 72, 1, 20),
			new PresetInventoryItem("Medicine", 88, 1, 20),
			new PresetInventoryItem("Medicine", 100, 1, 30),
			new PresetInventoryItem("Medicine", 244, 1, 20),
			new PresetInventoryItem("Medicine", 304, 1, 20),
			new PresetInventoryItem("Medicine", 112, 1, 20),
			new PresetInventoryItem("Medicine", 268, 1, 20),
			new PresetInventoryItem("Medicine", 292, 1, 20),
			new PresetInventoryItem("Medicine", 316, 1, 20),
			new PresetInventoryItem("TeaWine", 9, 1, 20)
		}, new List<PresetOrgMemberCombatSkill>
		{
			new PresetOrgMemberCombatSkill(39, 8),
			new PresetOrgMemberCombatSkill(142, 6),
			new PresetOrgMemberCombatSkill(249, 7),
			new PresetOrgMemberCombatSkill(423, 6),
			new PresetOrgMemberCombatSkill(549, 8),
			new PresetOrgMemberCombatSkill(656, 8)
		}, new sbyte[5] { 10, 10, 10, 10, 10 }, new short[8] { -80, -70, -70, -60, -60, -70, -70, -70 }, 120000, 432000, 20, 20, 61500, new short[16]
		{
			-1, -1, 12, -1, 9, -1, 2, 2, -1, -1,
			-1, 12, 12, 2, 2, 9
		}, 8, new short[14]
		{
			12, 9, 12, -1, 9, 2, -1, 12, -1, -1,
			12, -1, -1, -1
		}, new short[6] { -1, -1, -1, -1, 12, 12 }, new List<sbyte> { 15, 24, 25, 55 }, 3, new ResourceInts(default(int), default(int), default(int), default(int), default(int), default(int), default(int), default(int)), new IntPair[18]
		{
			new IntPair(17, 3),
			new IntPair(8, 3),
			new IntPair(5, 12),
			new IntPair(6, 3),
			new IntPair(15, 3),
			new IntPair(16, 3),
			new IntPair(7, 3),
			new IntPair(14, 36),
			new IntPair(12, 3),
			new IntPair(4, 36),
			new IntPair(3, 3),
			new IntPair(13, 3),
			new IntPair(10, 3),
			new IntPair(2, 3),
			new IntPair(1, 3),
			new IntPair(11, 18),
			new IntPair(0, 3),
			new IntPair(9, 3)
		}, null));
		_dataArray.Add(new OrganizationMemberItem(101, 475, 7, 7, 4, 6, 2, restrictPrincipalAmount: false, -1, -1, -1, 4, 6, 8, 7, 75, 129, new int[2] { 473, 473 }, 8600, 14, 10000, 21150, 0, new List<short> { 37, 38, 39 }, new List<short> { 34, 35, 36 }, new int[2] { 476, 477 }, canStroll: true, 272, new short[4] { 31, 38, 45, 52 }, new PresetEquipmentItemWithProb[8]
		{
			new PresetEquipmentItemWithProb("Armor", 90, 75),
			new PresetEquipmentItemWithProb("Armor", 360, 100),
			new PresetEquipmentItemWithProb("Armor", 459, 75),
			new PresetEquipmentItemWithProb("Armor", 216, 75),
			new PresetEquipmentItemWithProb("Accessory", 162, 75),
			new PresetEquipmentItemWithProb("Accessory", 207, 50),
			new PresetEquipmentItemWithProb("Accessory", 81, 50),
			new PresetEquipmentItemWithProb("Carrier", 9, 75)
		}, new PresetEquipmentItem("Clothing", 38), new List<PresetInventoryItem>
		{
			new PresetInventoryItem("SkillBook", 108, 3, 40),
			new PresetInventoryItem("SkillBook", 18, 1, 30),
			new PresetInventoryItem("SkillBook", 36, 1, 30),
			new PresetInventoryItem("SkillBook", 135, 1, 30),
			new PresetInventoryItem("Material", 28, 1, 30),
			new PresetInventoryItem("Material", 35, 1, 30),
			new PresetInventoryItem("Food", 93, 3, 40),
			new PresetInventoryItem("Medicine", 54, 1, 20),
			new PresetInventoryItem("Medicine", 72, 1, 20),
			new PresetInventoryItem("Medicine", 88, 1, 20),
			new PresetInventoryItem("Medicine", 100, 1, 30),
			new PresetInventoryItem("Medicine", 244, 1, 20),
			new PresetInventoryItem("Medicine", 304, 1, 20),
			new PresetInventoryItem("Medicine", 112, 1, 20),
			new PresetInventoryItem("Medicine", 268, 1, 20),
			new PresetInventoryItem("Medicine", 292, 1, 20),
			new PresetInventoryItem("Medicine", 316, 1, 20),
			new PresetInventoryItem("TeaWine", 9, 1, 20)
		}, new List<PresetOrgMemberCombatSkill>
		{
			new PresetOrgMemberCombatSkill(39, 7),
			new PresetOrgMemberCombatSkill(142, 6),
			new PresetOrgMemberCombatSkill(249, 7),
			new PresetOrgMemberCombatSkill(423, 6),
			new PresetOrgMemberCombatSkill(549, 7),
			new PresetOrgMemberCombatSkill(656, 7)
		}, new sbyte[5] { 8, 8, 8, 8, 8 }, new short[8] { -80, -70, -70, -60, -60, -70, -70, -70 }, 75000, 216000, 15, 30, 42300, new short[16]
		{
			-1, -1, 12, -1, 9, -1, 2, 2, -1, -1,
			-1, 12, 12, 2, 2, 9
		}, 8, new short[14]
		{
			12, 9, 12, -1, 9, 2, -1, 12, -1, -1,
			12, -1, -1, -1
		}, new short[6] { -1, -1, -1, -1, 12, 12 }, new List<sbyte> { 15, 24, 25 }, 3, new ResourceInts(default(int), default(int), default(int), default(int), default(int), default(int), default(int), default(int)), new IntPair[18]
		{
			new IntPair(17, 3),
			new IntPair(8, 3),
			new IntPair(5, 12),
			new IntPair(6, 3),
			new IntPair(15, 3),
			new IntPair(16, 3),
			new IntPair(7, 3),
			new IntPair(14, 36),
			new IntPair(12, 3),
			new IntPair(4, 36),
			new IntPair(3, 3),
			new IntPair(13, 3),
			new IntPair(10, 3),
			new IntPair(2, 3),
			new IntPair(1, 3),
			new IntPair(11, 18),
			new IntPair(0, 3),
			new IntPair(9, 3)
		}, null));
		_dataArray.Add(new OrganizationMemberItem(102, 478, 7, 6, 4, 6, 2, restrictPrincipalAmount: false, -1, -1, -1, 3, 6, 7, 6, 75, 129, new int[2] { 473, 473 }, 6100, 12, 8000, 13800, 0, new List<short> { 37, 38, 39 }, new List<short> { 34, 35, 36 }, new int[2] { 479, 480 }, canStroll: false, 273, new short[4] { 28, 34, 40, 46 }, new PresetEquipmentItemWithProb[8]
		{
			new PresetEquipmentItemWithProb("Armor", 90, 75),
			new PresetEquipmentItemWithProb("Armor", 360, 100),
			new PresetEquipmentItemWithProb("Armor", 459, 75),
			new PresetEquipmentItemWithProb("Armor", 216, 75),
			new PresetEquipmentItemWithProb("Accessory", 162, 75),
			new PresetEquipmentItemWithProb("Accessory", 207, 50),
			new PresetEquipmentItemWithProb("Accessory", 81, 50),
			new PresetEquipmentItemWithProb("Carrier", 9, 75)
		}, new PresetEquipmentItem("Clothing", 38), new List<PresetInventoryItem>
		{
			new PresetInventoryItem("SkillBook", 108, 3, 40),
			new PresetInventoryItem("SkillBook", 18, 1, 30),
			new PresetInventoryItem("SkillBook", 36, 1, 30),
			new PresetInventoryItem("SkillBook", 135, 1, 30),
			new PresetInventoryItem("Material", 28, 1, 30),
			new PresetInventoryItem("Material", 35, 1, 30),
			new PresetInventoryItem("Food", 93, 3, 40),
			new PresetInventoryItem("Medicine", 54, 1, 20),
			new PresetInventoryItem("Medicine", 72, 1, 20),
			new PresetInventoryItem("Medicine", 88, 1, 20),
			new PresetInventoryItem("Medicine", 100, 1, 30),
			new PresetInventoryItem("Medicine", 244, 1, 20),
			new PresetInventoryItem("Medicine", 304, 1, 20),
			new PresetInventoryItem("Medicine", 112, 1, 20),
			new PresetInventoryItem("Medicine", 268, 1, 20),
			new PresetInventoryItem("Medicine", 292, 1, 20),
			new PresetInventoryItem("Medicine", 316, 1, 20),
			new PresetInventoryItem("TeaWine", 9, 1, 20)
		}, new List<PresetOrgMemberCombatSkill>
		{
			new PresetOrgMemberCombatSkill(39, 6),
			new PresetOrgMemberCombatSkill(142, 5),
			new PresetOrgMemberCombatSkill(249, 6),
			new PresetOrgMemberCombatSkill(423, 5),
			new PresetOrgMemberCombatSkill(549, 6),
			new PresetOrgMemberCombatSkill(656, 6)
		}, new sbyte[5] { 8, 8, 8, 8, 8 }, new short[8] { -80, -70, -70, -60, -60, -70, -70, -70 }, 45000, 108000, 15, 40, 27600, new short[16]
		{
			-1, -1, 12, -1, 9, -1, 2, 2, -1, -1,
			-1, 12, 12, 2, 2, 9
		}, 7, new short[14]
		{
			12, 9, 12, -1, 9, 2, -1, 12, -1, -1,
			12, -1, -1, -1
		}, new short[6] { -1, -1, -1, -1, 12, 12 }, new List<sbyte> { 15, 24, 25 }, 3, new ResourceInts(default(int), default(int), default(int), default(int), default(int), default(int), default(int), default(int)), new IntPair[18]
		{
			new IntPair(17, 3),
			new IntPair(8, 3),
			new IntPair(5, 12),
			new IntPair(6, 3),
			new IntPair(15, 3),
			new IntPair(16, 3),
			new IntPair(7, 3),
			new IntPair(14, 36),
			new IntPair(12, 3),
			new IntPair(4, 36),
			new IntPair(3, 3),
			new IntPair(13, 3),
			new IntPair(10, 3),
			new IntPair(2, 3),
			new IntPair(1, 3),
			new IntPair(11, 18),
			new IntPair(0, 3),
			new IntPair(9, 3)
		}, null));
		List<OrganizationMemberItem> dataArray23 = _dataArray;
		int[] monasticTitleSuffixes23 = new int[2] { 482, 483 };
		List<short> favoriteClothingIds23 = new List<short> { 37, 38, 39 };
		List<short> hatedClothingIds23 = new List<short> { 34, 35, 36 };
		int[] spouseAnonymousTitles23 = new int[2] { 484, 485 };
		short[] initialAges23 = new short[4] { 19, 22, 25, 28 };
		PresetEquipmentItemWithProb[] equipment23 = new PresetEquipmentItemWithProb[8]
		{
			new PresetEquipmentItemWithProb("Armor", 90, 75),
			new PresetEquipmentItemWithProb("Armor", 360, 100),
			new PresetEquipmentItemWithProb("Armor", 459, 75),
			new PresetEquipmentItemWithProb("Armor", 216, 75),
			new PresetEquipmentItemWithProb("Accessory", 162, 75),
			new PresetEquipmentItemWithProb("Accessory", 207, 50),
			new PresetEquipmentItemWithProb("Accessory", 81, 50),
			new PresetEquipmentItemWithProb("Carrier", 9, 75)
		};
		PresetEquipmentItem clothing23 = new PresetEquipmentItem("Clothing", 38);
		List<PresetInventoryItem> inventory23 = new List<PresetInventoryItem>
		{
			new PresetInventoryItem("SkillBook", 108, 3, 30),
			new PresetInventoryItem("SkillBook", 18, 1, 20),
			new PresetInventoryItem("SkillBook", 36, 1, 20),
			new PresetInventoryItem("SkillBook", 135, 1, 20),
			new PresetInventoryItem("Material", 28, 1, 20),
			new PresetInventoryItem("Material", 35, 1, 20),
			new PresetInventoryItem("Food", 93, 3, 30),
			new PresetInventoryItem("Medicine", 54, 1, 20),
			new PresetInventoryItem("Medicine", 72, 1, 20),
			new PresetInventoryItem("Medicine", 82, 1, 20),
			new PresetInventoryItem("Medicine", 94, 1, 30),
			new PresetInventoryItem("Medicine", 238, 1, 10),
			new PresetInventoryItem("Medicine", 298, 1, 10),
			new PresetInventoryItem("Medicine", 106, 1, 10),
			new PresetInventoryItem("Medicine", 262, 1, 10),
			new PresetInventoryItem("Medicine", 286, 1, 10),
			new PresetInventoryItem("Medicine", 310, 1, 10),
			new PresetInventoryItem("TeaWine", 9, 1, 20)
		};
		List<PresetOrgMemberCombatSkill> combatSkills23 = new List<PresetOrgMemberCombatSkill>
		{
			new PresetOrgMemberCombatSkill(39, 5),
			new PresetOrgMemberCombatSkill(142, 5),
			new PresetOrgMemberCombatSkill(249, 5),
			new PresetOrgMemberCombatSkill(423, 5),
			new PresetOrgMemberCombatSkill(549, 5),
			new PresetOrgMemberCombatSkill(656, 5)
		};
		sbyte[] extraCombatSkillGrids23 = new sbyte[5] { 6, 6, 6, 6, 6 };
		short[] resourcesAdjust23 = new short[8] { -80, -70, -70, -60, -60, -70, -70, -70 };
		short[] lifeSkillsAdjust23 = new short[16]
		{
			-1, -1, 12, -1, 9, -1, 2, 2, -1, -1,
			-1, 12, 12, 2, 2, 9
		};
		short[] combatSkillsAdjust23 = new short[14]
		{
			12, 9, 12, -1, 9, 2, -1, 12, -1, -1,
			12, -1, -1, -1
		};
		short[] mainAttributesAdjust23 = new short[6] { -1, -1, -1, -1, 12, 12 };
		identityInteractConfig = new List<sbyte>();
		dataArray23.Add(new OrganizationMemberItem(103, 481, 7, 5, 4, 6, 2, restrictPrincipalAmount: false, -1, -1, -1, 2, 5, 6, 5, 75, 129, monasticTitleSuffixes23, 4200, 10, 6000, 8400, 0, favoriteClothingIds23, hatedClothingIds23, spouseAnonymousTitles23, canStroll: true, 274, initialAges23, equipment23, clothing23, inventory23, combatSkills23, extraCombatSkillGrids23, resourcesAdjust23, 30000, 36000, 10, 50, 16800, lifeSkillsAdjust23, 6, combatSkillsAdjust23, mainAttributesAdjust23, identityInteractConfig, 3, new ResourceInts(default(int), default(int), default(int), default(int), default(int), default(int), default(int), default(int)), new IntPair[18]
		{
			new IntPair(17, 3),
			new IntPair(8, 3),
			new IntPair(5, 9),
			new IntPair(6, 3),
			new IntPair(15, 3),
			new IntPair(16, 3),
			new IntPair(7, 3),
			new IntPair(14, 24),
			new IntPair(12, 3),
			new IntPair(4, 24),
			new IntPair(3, 3),
			new IntPair(13, 3),
			new IntPair(10, 6),
			new IntPair(2, 6),
			new IntPair(1, 3),
			new IntPair(11, 21),
			new IntPair(0, 3),
			new IntPair(9, 3)
		}, null));
		List<OrganizationMemberItem> dataArray24 = _dataArray;
		int[] monasticTitleSuffixes24 = new int[2] { 487, 488 };
		List<short> favoriteClothingIds24 = new List<short> { 37, 38, 39 };
		List<short> hatedClothingIds24 = new List<short> { 34, 35, 36 };
		int[] spouseAnonymousTitles24 = new int[2] { 489, 490 };
		short[] initialAges24 = new short[4] { 22, 26, 30, 34 };
		PresetEquipmentItemWithProb[] equipment24 = new PresetEquipmentItemWithProb[8]
		{
			new PresetEquipmentItemWithProb("Armor", 90, 75),
			new PresetEquipmentItemWithProb("Armor", 360, 100),
			new PresetEquipmentItemWithProb("Armor", 459, 75),
			new PresetEquipmentItemWithProb("Armor", 216, 75),
			new PresetEquipmentItemWithProb("Accessory", 162, 75),
			new PresetEquipmentItemWithProb("Accessory", 207, 50),
			new PresetEquipmentItemWithProb("Accessory", 81, 50),
			new PresetEquipmentItemWithProb("Carrier", 9, 75)
		};
		PresetEquipmentItem clothing24 = new PresetEquipmentItem("Clothing", 37);
		List<PresetInventoryItem> inventory24 = new List<PresetInventoryItem>
		{
			new PresetInventoryItem("SkillBook", 108, 3, 30),
			new PresetInventoryItem("SkillBook", 18, 1, 20),
			new PresetInventoryItem("SkillBook", 36, 1, 20),
			new PresetInventoryItem("SkillBook", 135, 1, 20),
			new PresetInventoryItem("Material", 28, 1, 20),
			new PresetInventoryItem("Material", 35, 1, 20),
			new PresetInventoryItem("Food", 93, 3, 30),
			new PresetInventoryItem("Medicine", 54, 1, 20),
			new PresetInventoryItem("Medicine", 72, 1, 20),
			new PresetInventoryItem("Medicine", 82, 1, 20),
			new PresetInventoryItem("Medicine", 94, 1, 30),
			new PresetInventoryItem("Medicine", 238, 1, 10),
			new PresetInventoryItem("Medicine", 298, 1, 10),
			new PresetInventoryItem("Medicine", 106, 1, 10),
			new PresetInventoryItem("Medicine", 262, 1, 10),
			new PresetInventoryItem("Medicine", 286, 1, 10),
			new PresetInventoryItem("Medicine", 310, 1, 10),
			new PresetInventoryItem("TeaWine", 9, 1, 20)
		};
		List<PresetOrgMemberCombatSkill> combatSkills24 = new List<PresetOrgMemberCombatSkill>
		{
			new PresetOrgMemberCombatSkill(39, 4),
			new PresetOrgMemberCombatSkill(142, 4),
			new PresetOrgMemberCombatSkill(249, 4),
			new PresetOrgMemberCombatSkill(656, 4)
		};
		sbyte[] extraCombatSkillGrids24 = new sbyte[5] { 4, 4, 4, 4, 4 };
		short[] resourcesAdjust24 = new short[8] { -80, -70, -70, -60, -60, -70, -70, -70 };
		short[] lifeSkillsAdjust24 = new short[16]
		{
			-1, -1, 12, -1, 9, -1, 2, 2, -1, -1,
			-1, 12, 12, 2, 2, 9
		};
		short[] combatSkillsAdjust24 = new short[14]
		{
			12, 9, 12, -1, 9, 2, -1, 12, -1, -1,
			12, -1, -1, -1
		};
		short[] mainAttributesAdjust24 = new short[6] { -1, -1, -1, -1, 12, 12 };
		identityInteractConfig = new List<sbyte>();
		dataArray24.Add(new OrganizationMemberItem(104, 486, 7, 4, 6, 9, 3, restrictPrincipalAmount: false, -1, -1, -1, 1, 4, 5, 4, 75, 129, monasticTitleSuffixes24, 2800, 8, 4500, 4650, 0, favoriteClothingIds24, hatedClothingIds24, spouseAnonymousTitles24, canStroll: true, 275, initialAges24, equipment24, clothing24, inventory24, combatSkills24, extraCombatSkillGrids24, resourcesAdjust24, 22500, 18000, 10, 60, 9300, lifeSkillsAdjust24, 5, combatSkillsAdjust24, mainAttributesAdjust24, identityInteractConfig, 3, new ResourceInts(default(int), default(int), default(int), default(int), default(int), default(int), default(int), default(int)), new IntPair[18]
		{
			new IntPair(17, 3),
			new IntPair(8, 3),
			new IntPair(5, 9),
			new IntPair(6, 3),
			new IntPair(15, 3),
			new IntPair(16, 3),
			new IntPair(7, 3),
			new IntPair(14, 24),
			new IntPair(12, 3),
			new IntPair(4, 24),
			new IntPair(3, 3),
			new IntPair(13, 3),
			new IntPair(10, 6),
			new IntPair(2, 6),
			new IntPair(1, 3),
			new IntPair(11, 21),
			new IntPair(0, 3),
			new IntPair(9, 3)
		}, null));
		List<OrganizationMemberItem> dataArray25 = _dataArray;
		int[] monasticTitleSuffixes25 = new int[2] { 492, 493 };
		List<short> favoriteClothingIds25 = new List<short> { 37, 38, 39 };
		List<short> hatedClothingIds25 = new List<short> { 34, 35, 36 };
		int[] spouseAnonymousTitles25 = new int[2] { 494, 495 };
		short[] initialAges25 = new short[4] { 22, 26, 30, 34 };
		PresetEquipmentItemWithProb[] equipment25 = new PresetEquipmentItemWithProb[8]
		{
			new PresetEquipmentItemWithProb("Armor", 90, 75),
			new PresetEquipmentItemWithProb("Armor", 360, 100),
			new PresetEquipmentItemWithProb("Armor", 459, 75),
			new PresetEquipmentItemWithProb("Armor", 216, 75),
			new PresetEquipmentItemWithProb("Accessory", 162, 75),
			new PresetEquipmentItemWithProb("Accessory", 207, 50),
			new PresetEquipmentItemWithProb("Accessory", 81, 50),
			new PresetEquipmentItemWithProb("Carrier", 9, 75)
		};
		PresetEquipmentItem clothing25 = new PresetEquipmentItem("Clothing", 37);
		List<PresetInventoryItem> inventory25 = new List<PresetInventoryItem>
		{
			new PresetInventoryItem("SkillBook", 108, 3, 30),
			new PresetInventoryItem("SkillBook", 18, 1, 20),
			new PresetInventoryItem("SkillBook", 36, 1, 20),
			new PresetInventoryItem("SkillBook", 135, 1, 20),
			new PresetInventoryItem("Material", 28, 1, 20),
			new PresetInventoryItem("Material", 35, 1, 20),
			new PresetInventoryItem("Food", 93, 3, 30),
			new PresetInventoryItem("Medicine", 54, 1, 20),
			new PresetInventoryItem("Medicine", 72, 1, 20),
			new PresetInventoryItem("Medicine", 82, 1, 20),
			new PresetInventoryItem("Medicine", 94, 1, 30),
			new PresetInventoryItem("Medicine", 238, 1, 10),
			new PresetInventoryItem("Medicine", 298, 1, 10),
			new PresetInventoryItem("Medicine", 106, 1, 10),
			new PresetInventoryItem("Medicine", 262, 1, 10),
			new PresetInventoryItem("Medicine", 286, 1, 10),
			new PresetInventoryItem("Medicine", 310, 1, 10),
			new PresetInventoryItem("TeaWine", 9, 1, 20)
		};
		List<PresetOrgMemberCombatSkill> combatSkills25 = new List<PresetOrgMemberCombatSkill>
		{
			new PresetOrgMemberCombatSkill(39, 3),
			new PresetOrgMemberCombatSkill(142, 3),
			new PresetOrgMemberCombatSkill(249, 3),
			new PresetOrgMemberCombatSkill(549, 3)
		};
		sbyte[] extraCombatSkillGrids25 = new sbyte[5] { 4, 4, 4, 4, 4 };
		short[] resourcesAdjust25 = new short[8] { -80, -70, -70, -60, -60, -70, -70, -70 };
		short[] lifeSkillsAdjust25 = new short[16]
		{
			-1, -1, 12, -1, 9, -1, 2, 2, -1, -1,
			-1, 12, 12, 2, 2, 9
		};
		short[] combatSkillsAdjust25 = new short[14]
		{
			12, 9, 12, -1, 9, 2, -1, 12, -1, -1,
			12, -1, -1, -1
		};
		short[] mainAttributesAdjust25 = new short[6] { -1, -1, -1, -1, 12, 12 };
		identityInteractConfig = new List<sbyte>();
		dataArray25.Add(new OrganizationMemberItem(105, 491, 7, 3, 6, 9, 3, restrictPrincipalAmount: false, -1, -1, -1, 1, 3, 5, 3, 75, 129, monasticTitleSuffixes25, 1800, 6, 3000, 2250, 0, favoriteClothingIds25, hatedClothingIds25, spouseAnonymousTitles25, canStroll: true, 276, initialAges25, equipment25, clothing25, inventory25, combatSkills25, extraCombatSkillGrids25, resourcesAdjust25, 15000, 9000, 10, 70, 4500, lifeSkillsAdjust25, 4, combatSkillsAdjust25, mainAttributesAdjust25, identityInteractConfig, 3, new ResourceInts(default(int), default(int), default(int), default(int), default(int), default(int), default(int), default(int)), new IntPair[18]
		{
			new IntPair(17, 3),
			new IntPair(8, 3),
			new IntPair(5, 9),
			new IntPair(6, 3),
			new IntPair(15, 3),
			new IntPair(16, 3),
			new IntPair(7, 3),
			new IntPair(14, 24),
			new IntPair(12, 3),
			new IntPair(4, 24),
			new IntPair(3, 3),
			new IntPair(13, 3),
			new IntPair(10, 6),
			new IntPair(2, 6),
			new IntPair(1, 3),
			new IntPair(11, 21),
			new IntPair(0, 3),
			new IntPair(9, 3)
		}, null));
		List<OrganizationMemberItem> dataArray26 = _dataArray;
		int[] monasticTitleSuffixes26 = new int[2] { 497, 498 };
		List<short> favoriteClothingIds26 = new List<short> { 37, 38, 39 };
		List<short> hatedClothingIds26 = new List<short> { 34, 35, 36 };
		int[] spouseAnonymousTitles26 = new int[2] { 499, 500 };
		short[] initialAges26 = new short[4] { 22, 26, 30, 34 };
		PresetEquipmentItemWithProb[] equipment26 = new PresetEquipmentItemWithProb[8]
		{
			new PresetEquipmentItemWithProb("Armor", 90, 75),
			new PresetEquipmentItemWithProb("Armor", 360, 100),
			new PresetEquipmentItemWithProb("Armor", 459, 75),
			new PresetEquipmentItemWithProb("Armor", 216, 75),
			new PresetEquipmentItemWithProb("Accessory", 162, 75),
			new PresetEquipmentItemWithProb("Accessory", 207, 50),
			new PresetEquipmentItemWithProb("Accessory", 81, 50),
			new PresetEquipmentItemWithProb("Carrier", 9, 75)
		};
		PresetEquipmentItem clothing26 = new PresetEquipmentItem("Clothing", 37);
		List<PresetInventoryItem> inventory26 = new List<PresetInventoryItem>
		{
			new PresetInventoryItem("SkillBook", 108, 3, 20),
			new PresetInventoryItem("SkillBook", 18, 1, 10),
			new PresetInventoryItem("SkillBook", 36, 1, 10),
			new PresetInventoryItem("SkillBook", 135, 1, 10),
			new PresetInventoryItem("Material", 28, 1, 10),
			new PresetInventoryItem("Material", 35, 1, 10),
			new PresetInventoryItem("Food", 93, 3, 20),
			new PresetInventoryItem("Medicine", 66, 1, 20),
			new PresetInventoryItem("Medicine", 94, 1, 20)
		};
		List<PresetOrgMemberCombatSkill> combatSkills26 = new List<PresetOrgMemberCombatSkill>
		{
			new PresetOrgMemberCombatSkill(39, 2),
			new PresetOrgMemberCombatSkill(142, 2),
			new PresetOrgMemberCombatSkill(249, 2),
			new PresetOrgMemberCombatSkill(423, 2)
		};
		sbyte[] extraCombatSkillGrids26 = new sbyte[5] { 2, 2, 2, 2, 2 };
		short[] resourcesAdjust26 = new short[8] { -80, -70, -70, -60, -60, -70, -70, -70 };
		short[] lifeSkillsAdjust26 = new short[16]
		{
			-1, -1, 12, -1, 9, -1, 2, 2, -1, -1,
			-1, 12, 12, 2, 2, 9
		};
		short[] combatSkillsAdjust26 = new short[14]
		{
			12, 9, 12, -1, 9, 2, -1, 12, -1, -1,
			12, -1, -1, -1
		};
		short[] mainAttributesAdjust26 = new short[6] { -1, -1, -1, -1, 12, 12 };
		identityInteractConfig = new List<sbyte>();
		dataArray26.Add(new OrganizationMemberItem(106, 496, 7, 2, 6, 9, 3, restrictPrincipalAmount: false, -1, -1, -1, 1, 2, 5, 2, 75, 129, monasticTitleSuffixes26, 600, 4, 2000, 900, 0, favoriteClothingIds26, hatedClothingIds26, spouseAnonymousTitles26, canStroll: true, 277, initialAges26, equipment26, clothing26, inventory26, combatSkills26, extraCombatSkillGrids26, resourcesAdjust26, 11250, 3000, 5, 80, 1800, lifeSkillsAdjust26, 3, combatSkillsAdjust26, mainAttributesAdjust26, identityInteractConfig, 3, new ResourceInts(default(int), default(int), default(int), default(int), default(int), default(int), default(int), default(int)), new IntPair[18]
		{
			new IntPair(17, 3),
			new IntPair(8, 3),
			new IntPair(5, 6),
			new IntPair(6, 3),
			new IntPair(15, 3),
			new IntPair(16, 3),
			new IntPair(7, 3),
			new IntPair(14, 12),
			new IntPair(12, 3),
			new IntPair(4, 12),
			new IntPair(3, 3),
			new IntPair(13, 3),
			new IntPair(10, 9),
			new IntPair(2, 9),
			new IntPair(1, 3),
			new IntPair(11, 24),
			new IntPair(0, 3),
			new IntPair(9, 3)
		}, null));
		List<OrganizationMemberItem> dataArray27 = _dataArray;
		int[] monasticTitleSuffixes27 = new int[2] { 502, 503 };
		List<short> favoriteClothingIds27 = new List<short> { 37, 38, 39 };
		List<short> hatedClothingIds27 = new List<short> { 34, 35, 36 };
		int[] spouseAnonymousTitles27 = new int[2] { 504, 505 };
		short[] initialAges27 = new short[4] { 13, 14, 15, 16 };
		PresetEquipmentItemWithProb[] equipment27 = new PresetEquipmentItemWithProb[8]
		{
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Armor", 360, 100),
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Armor", 216, 75),
			new PresetEquipmentItemWithProb("Accessory", 162, 75),
			new PresetEquipmentItemWithProb("Accessory", 207, 50),
			new PresetEquipmentItemWithProb("Accessory", -1, 0),
			new PresetEquipmentItemWithProb("Carrier", -1, 0)
		};
		PresetEquipmentItem clothing27 = new PresetEquipmentItem("Clothing", 37);
		List<PresetInventoryItem> inventory27 = new List<PresetInventoryItem>
		{
			new PresetInventoryItem("SkillBook", 108, 3, 20),
			new PresetInventoryItem("SkillBook", 18, 1, 10),
			new PresetInventoryItem("SkillBook", 36, 1, 10),
			new PresetInventoryItem("SkillBook", 135, 1, 10),
			new PresetInventoryItem("Material", 28, 1, 10),
			new PresetInventoryItem("Material", 35, 1, 10),
			new PresetInventoryItem("Food", 93, 3, 20),
			new PresetInventoryItem("Medicine", 66, 1, 20),
			new PresetInventoryItem("Medicine", 94, 1, 20)
		};
		List<PresetOrgMemberCombatSkill> combatSkills27 = new List<PresetOrgMemberCombatSkill>
		{
			new PresetOrgMemberCombatSkill(39, 1),
			new PresetOrgMemberCombatSkill(142, 1),
			new PresetOrgMemberCombatSkill(549, 1)
		};
		sbyte[] extraCombatSkillGrids27 = new sbyte[5];
		short[] resourcesAdjust27 = new short[8] { -80, -70, -70, -60, -60, -70, -70, -70 };
		short[] lifeSkillsAdjust27 = new short[16]
		{
			-1, -1, 12, -1, 9, -1, 2, 2, -1, -1,
			-1, 12, 12, 2, 2, 9
		};
		short[] combatSkillsAdjust27 = new short[14]
		{
			12, 9, 12, -1, 9, 2, -1, 12, -1, -1,
			12, -1, -1, -1
		};
		short[] mainAttributesAdjust27 = new short[6] { -1, -1, -1, -1, 12, 12 };
		identityInteractConfig = new List<sbyte>();
		dataArray27.Add(new OrganizationMemberItem(107, 501, 7, 1, 6, 9, 3, restrictPrincipalAmount: false, -1, -1, -1, 0, 1, 5, 1, 75, 129, monasticTitleSuffixes27, 300, 2, 1000, 300, 0, favoriteClothingIds27, hatedClothingIds27, spouseAnonymousTitles27, canStroll: true, 278, initialAges27, equipment27, clothing27, inventory27, combatSkills27, extraCombatSkillGrids27, resourcesAdjust27, 7500, 1500, 5, 90, 600, lifeSkillsAdjust27, 2, combatSkillsAdjust27, mainAttributesAdjust27, identityInteractConfig, 3, new ResourceInts(default(int), default(int), default(int), default(int), default(int), default(int), default(int), default(int)), new IntPair[18]
		{
			new IntPair(17, 3),
			new IntPair(8, 3),
			new IntPair(5, 6),
			new IntPair(6, 3),
			new IntPair(15, 3),
			new IntPair(16, 3),
			new IntPair(7, 3),
			new IntPair(14, 12),
			new IntPair(12, 3),
			new IntPair(4, 12),
			new IntPair(3, 3),
			new IntPair(13, 3),
			new IntPair(10, 9),
			new IntPair(2, 9),
			new IntPair(1, 3),
			new IntPair(11, 24),
			new IntPair(0, 3),
			new IntPair(9, 3)
		}, null));
		List<OrganizationMemberItem> dataArray28 = _dataArray;
		int[] monasticTitleSuffixes28 = new int[2] { 507, 508 };
		List<short> favoriteClothingIds28 = new List<short> { 37, 38, 39 };
		List<short> hatedClothingIds28 = new List<short> { 34, 35, 36 };
		int[] spouseAnonymousTitles28 = new int[2] { 509, 510 };
		short[] initialAges28 = new short[4] { 10, 10, 10, 10 };
		PresetEquipmentItemWithProb[] equipment28 = new PresetEquipmentItemWithProb[8]
		{
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Armor", 360, 100),
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Armor", 216, 75),
			new PresetEquipmentItemWithProb("Accessory", 162, 75),
			new PresetEquipmentItemWithProb("Accessory", -1, 0),
			new PresetEquipmentItemWithProb("Accessory", -1, 0),
			new PresetEquipmentItemWithProb("Carrier", -1, 0)
		};
		PresetEquipmentItem clothing28 = new PresetEquipmentItem("Clothing", 37);
		List<PresetInventoryItem> inventory28 = new List<PresetInventoryItem>
		{
			new PresetInventoryItem("SkillBook", 108, 3, 20),
			new PresetInventoryItem("SkillBook", 18, 1, 10),
			new PresetInventoryItem("SkillBook", 36, 1, 10),
			new PresetInventoryItem("SkillBook", 135, 1, 10),
			new PresetInventoryItem("Material", 28, 1, 10),
			new PresetInventoryItem("Material", 35, 1, 10),
			new PresetInventoryItem("Food", 93, 3, 20),
			new PresetInventoryItem("Medicine", 66, 1, 20),
			new PresetInventoryItem("Medicine", 94, 1, 20)
		};
		List<PresetOrgMemberCombatSkill> combatSkills28 = new List<PresetOrgMemberCombatSkill>
		{
			new PresetOrgMemberCombatSkill(39, 0),
			new PresetOrgMemberCombatSkill(142, 0),
			new PresetOrgMemberCombatSkill(423, 0),
			new PresetOrgMemberCombatSkill(549, 0),
			new PresetOrgMemberCombatSkill(656, 0)
		};
		sbyte[] extraCombatSkillGrids28 = new sbyte[5];
		short[] resourcesAdjust28 = new short[8] { -80, -70, -70, -60, -60, -70, -70, -70 };
		short[] lifeSkillsAdjust28 = new short[16]
		{
			-1, -1, 12, -1, 9, -1, 2, 2, -1, -1,
			-1, 12, 12, 2, 2, 9
		};
		short[] combatSkillsAdjust28 = new short[14]
		{
			12, 9, 12, -1, 9, 2, -1, 12, -1, -1,
			12, -1, -1, -1
		};
		short[] mainAttributesAdjust28 = new short[6] { -1, -1, -1, -1, 12, 12 };
		identityInteractConfig = new List<sbyte>();
		dataArray28.Add(new OrganizationMemberItem(108, 506, 7, 0, 6, 9, 3, restrictPrincipalAmount: false, -1, -1, -1, 0, 0, 5, 0, 75, 129, monasticTitleSuffixes28, 150, 0, 500, 150, 0, favoriteClothingIds28, hatedClothingIds28, spouseAnonymousTitles28, canStroll: true, 279, initialAges28, equipment28, clothing28, inventory28, combatSkills28, extraCombatSkillGrids28, resourcesAdjust28, 3750, 750, 1, 100, 300, lifeSkillsAdjust28, 1, combatSkillsAdjust28, mainAttributesAdjust28, identityInteractConfig, 3, new ResourceInts(default(int), default(int), default(int), default(int), default(int), default(int), default(int), default(int)), new IntPair[18]
		{
			new IntPair(17, 3),
			new IntPair(8, 3),
			new IntPair(5, 6),
			new IntPair(6, 3),
			new IntPair(15, 3),
			new IntPair(16, 3),
			new IntPair(7, 3),
			new IntPair(14, 12),
			new IntPair(12, 3),
			new IntPair(4, 12),
			new IntPair(3, 3),
			new IntPair(13, 3),
			new IntPair(10, 9),
			new IntPair(2, 9),
			new IntPair(1, 3),
			new IntPair(11, 24),
			new IntPair(0, 3),
			new IntPair(9, 3)
		}, null));
		_dataArray.Add(new OrganizationMemberItem(109, 262, 8, 8, 1, 1, 1, restrictPrincipalAmount: true, 0, -1, -1, -1, 6, -1, 7, 0, 0, new int[2] { 511, 512 }, 15800, 16, 13000, 30750, 0, new List<short> { 40, 41, 42 }, new List<short> { 46, 47, 48 }, new int[2] { 513, 514 }, canStroll: false, 280, new short[4] { 24, 31, 38, 45 }, new PresetEquipmentItemWithProb[8]
		{
			new PresetEquipmentItemWithProb("Armor", 117, 75),
			new PresetEquipmentItemWithProb("Armor", 387, 100),
			new PresetEquipmentItemWithProb("Armor", 513, 75),
			new PresetEquipmentItemWithProb("Armor", 243, 75),
			new PresetEquipmentItemWithProb("Accessory", 171, 75),
			new PresetEquipmentItemWithProb("Accessory", 207, 50),
			new PresetEquipmentItemWithProb("Accessory", 126, 50),
			new PresetEquipmentItemWithProb("Carrier", 9, 75)
		}, new PresetEquipmentItem("Clothing", 42), new List<PresetInventoryItem>
		{
			new PresetInventoryItem("SkillBook", 0, 3, 40),
			new PresetInventoryItem("SkillBook", 27, 1, 30),
			new PresetInventoryItem("SkillBook", 45, 1, 30),
			new PresetInventoryItem("Food", 0, 3, 40),
			new PresetInventoryItem("Food", 135, 3, 40),
			new PresetInventoryItem("Medicine", 60, 1, 20),
			new PresetInventoryItem("Medicine", 72, 1, 20),
			new PresetInventoryItem("Medicine", 88, 1, 20),
			new PresetInventoryItem("Medicine", 100, 1, 20),
			new PresetInventoryItem("Medicine", 256, 1, 20),
			new PresetInventoryItem("Medicine", 328, 1, 20),
			new PresetInventoryItem("Medicine", 112, 1, 30),
			new PresetInventoryItem("TeaWine", 27, 1, 20),
			new PresetInventoryItem("TeaWine", 9, 1, 20)
		}, new List<PresetOrgMemberCombatSkill>
		{
			new PresetOrgMemberCombatSkill(47, 8),
			new PresetOrgMemberCombatSkill(149, 8),
			new PresetOrgMemberCombatSkill(257, 6),
			new PresetOrgMemberCombatSkill(354, 6),
			new PresetOrgMemberCombatSkill(430, 7),
			new PresetOrgMemberCombatSkill(714, 8)
		}, new sbyte[5] { 10, 10, 10, 10, 10 }, new short[8] { -70, -70, -80, -50, -70, -70, -70, -70 }, 120000, 432000, 20, 20, 61500, new short[16]
		{
			12, -1, -1, 9, -1, 9, -1, -1, -1, -1,
			-1, -1, 9, -1, 2, 2
		}, 8, new short[14]
		{
			12, 12, 9, 9, 12, -1, -1, -1, 2, 2,
			-1, -1, -1, 12
		}, new short[6] { 2, 12, -1, -1, 12, 9 }, new List<sbyte> { 7, 16, 24, 25, 55 }, 3, new ResourceInts(default(int), default(int), default(int), default(int), default(int), default(int), default(int), default(int)), new IntPair[18]
		{
			new IntPair(17, 1),
			new IntPair(8, 42),
			new IntPair(5, 54),
			new IntPair(6, 0),
			new IntPair(15, 3),
			new IntPair(16, 21),
			new IntPair(7, 1),
			new IntPair(14, 1),
			new IntPair(12, 0),
			new IntPair(4, 12),
			new IntPair(3, 1),
			new IntPair(13, 1),
			new IntPair(10, 3),
			new IntPair(2, 1),
			new IntPair(1, 1),
			new IntPair(11, 1),
			new IntPair(0, 1),
			new IntPair(9, 0)
		}, null));
		_dataArray.Add(new OrganizationMemberItem(110, 515, 8, 7, 4, 6, 2, restrictPrincipalAmount: false, 0, -1, -1, -1, 5, 8, 7, 0, 0, new int[2] { 516, 517 }, 8600, 14, 10000, 21150, 0, new List<short> { 40, 41, 42 }, new List<short> { 46, 47, 48 }, new int[2] { 518, 519 }, canStroll: false, 281, new short[4] { 22, 28, 34, 40 }, new PresetEquipmentItemWithProb[8]
		{
			new PresetEquipmentItemWithProb("Armor", 117, 75),
			new PresetEquipmentItemWithProb("Armor", 387, 100),
			new PresetEquipmentItemWithProb("Armor", 513, 75),
			new PresetEquipmentItemWithProb("Armor", 243, 75),
			new PresetEquipmentItemWithProb("Accessory", 171, 75),
			new PresetEquipmentItemWithProb("Accessory", 207, 50),
			new PresetEquipmentItemWithProb("Accessory", 126, 50),
			new PresetEquipmentItemWithProb("Carrier", 9, 75)
		}, new PresetEquipmentItem("Clothing", 41), new List<PresetInventoryItem>
		{
			new PresetInventoryItem("SkillBook", 0, 3, 40),
			new PresetInventoryItem("SkillBook", 27, 1, 30),
			new PresetInventoryItem("SkillBook", 45, 1, 30),
			new PresetInventoryItem("Food", 0, 3, 40),
			new PresetInventoryItem("Food", 135, 3, 40),
			new PresetInventoryItem("Medicine", 60, 1, 20),
			new PresetInventoryItem("Medicine", 72, 1, 20),
			new PresetInventoryItem("Medicine", 88, 1, 20),
			new PresetInventoryItem("Medicine", 100, 1, 20),
			new PresetInventoryItem("Medicine", 256, 1, 20),
			new PresetInventoryItem("Medicine", 328, 1, 20),
			new PresetInventoryItem("Medicine", 112, 1, 30),
			new PresetInventoryItem("TeaWine", 27, 1, 20),
			new PresetInventoryItem("TeaWine", 9, 1, 20)
		}, new List<PresetOrgMemberCombatSkill>
		{
			new PresetOrgMemberCombatSkill(47, 7),
			new PresetOrgMemberCombatSkill(149, 7),
			new PresetOrgMemberCombatSkill(257, 6),
			new PresetOrgMemberCombatSkill(354, 6),
			new PresetOrgMemberCombatSkill(430, 6),
			new PresetOrgMemberCombatSkill(714, 7)
		}, new sbyte[5] { 8, 8, 8, 8, 8 }, new short[8] { -70, -70, -80, -50, -70, -70, -70, -70 }, 75000, 216000, 15, 30, 42300, new short[16]
		{
			12, -1, -1, 9, -1, 9, -1, -1, -1, -1,
			-1, -1, 9, -1, 2, 2
		}, 8, new short[14]
		{
			12, 12, 9, 9, 12, -1, -1, -1, 2, 2,
			-1, -1, -1, 12
		}, new short[6] { 2, 12, -1, -1, 12, 9 }, new List<sbyte> { 7, 16, 24, 25 }, 3, new ResourceInts(default(int), default(int), default(int), default(int), default(int), default(int), default(int), default(int)), new IntPair[18]
		{
			new IntPair(17, 1),
			new IntPair(8, 42),
			new IntPair(5, 54),
			new IntPair(6, 0),
			new IntPair(15, 3),
			new IntPair(16, 21),
			new IntPair(7, 1),
			new IntPair(14, 1),
			new IntPair(12, 0),
			new IntPair(4, 12),
			new IntPair(3, 1),
			new IntPair(13, 1),
			new IntPair(10, 3),
			new IntPair(2, 1),
			new IntPair(1, 1),
			new IntPair(11, 1),
			new IntPair(0, 1),
			new IntPair(9, 0)
		}, null));
		_dataArray.Add(new OrganizationMemberItem(111, 520, 8, 6, 4, 6, 2, restrictPrincipalAmount: false, 0, -1, -1, -1, 4, 8, 6, 0, 0, new int[2] { 521, 522 }, 6100, 12, 8000, 13800, 0, new List<short> { 40, 41, 42 }, new List<short> { 46, 47, 48 }, new int[2] { 523, 524 }, canStroll: false, 282, new short[4] { 20, 25, 30, 35 }, new PresetEquipmentItemWithProb[8]
		{
			new PresetEquipmentItemWithProb("Armor", 117, 75),
			new PresetEquipmentItemWithProb("Armor", 387, 100),
			new PresetEquipmentItemWithProb("Armor", 513, 75),
			new PresetEquipmentItemWithProb("Armor", 243, 75),
			new PresetEquipmentItemWithProb("Accessory", 171, 75),
			new PresetEquipmentItemWithProb("Accessory", 207, 50),
			new PresetEquipmentItemWithProb("Accessory", 126, 50),
			new PresetEquipmentItemWithProb("Carrier", 9, 75)
		}, new PresetEquipmentItem("Clothing", 41), new List<PresetInventoryItem>
		{
			new PresetInventoryItem("SkillBook", 0, 3, 40),
			new PresetInventoryItem("SkillBook", 27, 1, 30),
			new PresetInventoryItem("SkillBook", 45, 1, 30),
			new PresetInventoryItem("Food", 0, 3, 40),
			new PresetInventoryItem("Food", 135, 3, 40),
			new PresetInventoryItem("Medicine", 60, 1, 20),
			new PresetInventoryItem("Medicine", 72, 1, 20),
			new PresetInventoryItem("Medicine", 88, 1, 20),
			new PresetInventoryItem("Medicine", 100, 1, 20),
			new PresetInventoryItem("Medicine", 256, 1, 20),
			new PresetInventoryItem("Medicine", 328, 1, 20),
			new PresetInventoryItem("Medicine", 112, 1, 30),
			new PresetInventoryItem("TeaWine", 27, 1, 20),
			new PresetInventoryItem("TeaWine", 9, 1, 20)
		}, new List<PresetOrgMemberCombatSkill>
		{
			new PresetOrgMemberCombatSkill(47, 6),
			new PresetOrgMemberCombatSkill(149, 6),
			new PresetOrgMemberCombatSkill(257, 5),
			new PresetOrgMemberCombatSkill(430, 6),
			new PresetOrgMemberCombatSkill(714, 6)
		}, new sbyte[5] { 8, 8, 8, 8, 8 }, new short[8] { -70, -70, -80, -50, -70, -70, -70, -70 }, 45000, 108000, 15, 40, 27600, new short[16]
		{
			12, -1, -1, 9, -1, 9, -1, -1, -1, -1,
			-1, -1, 9, -1, 2, 2
		}, 7, new short[14]
		{
			12, 12, 9, 9, 12, -1, -1, -1, 2, 2,
			-1, -1, -1, 12
		}, new short[6] { 2, 12, -1, -1, 12, 9 }, new List<sbyte> { 7, 16, 24, 25 }, 3, new ResourceInts(default(int), default(int), default(int), default(int), default(int), default(int), default(int), default(int)), new IntPair[18]
		{
			new IntPair(17, 1),
			new IntPair(8, 42),
			new IntPair(5, 54),
			new IntPair(6, 0),
			new IntPair(15, 3),
			new IntPair(16, 21),
			new IntPair(7, 1),
			new IntPair(14, 1),
			new IntPair(12, 0),
			new IntPair(4, 12),
			new IntPair(3, 1),
			new IntPair(13, 1),
			new IntPair(10, 3),
			new IntPair(2, 1),
			new IntPair(1, 1),
			new IntPair(11, 1),
			new IntPair(0, 1),
			new IntPair(9, 0)
		}, null));
		_dataArray.Add(new OrganizationMemberItem(112, 525, 8, 5, 4, 6, 2, restrictPrincipalAmount: false, 0, -1, -1, -1, 4, 8, 5, 0, 0, new int[2] { 526, 527 }, 4200, 10, 6000, 8400, 0, new List<short> { 40, 41, 42 }, new List<short> { 46, 47, 48 }, new int[2] { 528, 529 }, canStroll: true, 283, new short[4] { 18, 22, 26, 30 }, new PresetEquipmentItemWithProb[8]
		{
			new PresetEquipmentItemWithProb("Armor", 117, 75),
			new PresetEquipmentItemWithProb("Armor", 387, 100),
			new PresetEquipmentItemWithProb("Armor", 513, 75),
			new PresetEquipmentItemWithProb("Armor", 243, 75),
			new PresetEquipmentItemWithProb("Accessory", 171, 75),
			new PresetEquipmentItemWithProb("Accessory", 207, 50),
			new PresetEquipmentItemWithProb("Accessory", 126, 50),
			new PresetEquipmentItemWithProb("Carrier", 9, 75)
		}, new PresetEquipmentItem("Clothing", 41), new List<PresetInventoryItem>
		{
			new PresetInventoryItem("SkillBook", 0, 3, 30),
			new PresetInventoryItem("SkillBook", 27, 1, 20),
			new PresetInventoryItem("SkillBook", 45, 1, 20),
			new PresetInventoryItem("Food", 0, 3, 30),
			new PresetInventoryItem("Food", 135, 3, 30),
			new PresetInventoryItem("Medicine", 54, 1, 20),
			new PresetInventoryItem("Medicine", 66, 1, 20),
			new PresetInventoryItem("Medicine", 82, 1, 20),
			new PresetInventoryItem("Medicine", 94, 1, 20),
			new PresetInventoryItem("Medicine", 250, 1, 10),
			new PresetInventoryItem("Medicine", 322, 1, 10),
			new PresetInventoryItem("Medicine", 106, 1, 30),
			new PresetInventoryItem("TeaWine", 27, 1, 20),
			new PresetInventoryItem("TeaWine", 9, 1, 20)
		}, new List<PresetOrgMemberCombatSkill>
		{
			new PresetOrgMemberCombatSkill(47, 5),
			new PresetOrgMemberCombatSkill(149, 5),
			new PresetOrgMemberCombatSkill(257, 5),
			new PresetOrgMemberCombatSkill(354, 5),
			new PresetOrgMemberCombatSkill(714, 5)
		}, new sbyte[5] { 6, 6, 6, 6, 6 }, new short[8] { -70, -70, -80, -50, -70, -70, -70, -70 }, 30000, 36000, 10, 50, 16800, new short[16]
		{
			12, -1, -1, 9, -1, 9, -1, -1, -1, -1,
			-1, -1, 9, -1, 2, 2
		}, 6, new short[14]
		{
			12, 12, 9, 9, 12, -1, -1, -1, 2, 2,
			-1, -1, -1, 12
		}, new short[6] { 2, 12, -1, -1, 12, 9 }, new List<sbyte> { 7 }, 3, new ResourceInts(default(int), default(int), default(int), default(int), default(int), default(int), default(int), default(int)), new IntPair[18]
		{
			new IntPair(17, 1),
			new IntPair(8, 30),
			new IntPair(5, 42),
			new IntPair(6, 0),
			new IntPair(15, 3),
			new IntPair(16, 21),
			new IntPair(7, 1),
			new IntPair(14, 1),
			new IntPair(12, 0),
			new IntPair(4, 18),
			new IntPair(3, 1),
			new IntPair(13, 1),
			new IntPair(10, 3),
			new IntPair(2, 1),
			new IntPair(1, 1),
			new IntPair(11, 1),
			new IntPair(0, 1),
			new IntPair(9, 0)
		}, null));
		_dataArray.Add(new OrganizationMemberItem(113, 530, 8, 4, 6, 9, 3, restrictPrincipalAmount: false, 0, -1, -1, -1, 4, 7, 4, 0, 0, new int[2] { 531, 532 }, 2800, 8, 4500, 4650, 0, new List<short> { 40, 41, 42 }, new List<short> { 46, 47, 48 }, new int[2] { 533, 534 }, canStroll: false, 284, new short[4] { 16, 19, 22, 25 }, new PresetEquipmentItemWithProb[8]
		{
			new PresetEquipmentItemWithProb("Armor", 117, 75),
			new PresetEquipmentItemWithProb("Armor", 387, 100),
			new PresetEquipmentItemWithProb("Armor", 513, 75),
			new PresetEquipmentItemWithProb("Armor", 243, 75),
			new PresetEquipmentItemWithProb("Accessory", 171, 75),
			new PresetEquipmentItemWithProb("Accessory", 207, 50),
			new PresetEquipmentItemWithProb("Accessory", 126, 50),
			new PresetEquipmentItemWithProb("Carrier", 9, 75)
		}, new PresetEquipmentItem("Clothing", 41), new List<PresetInventoryItem>
		{
			new PresetInventoryItem("SkillBook", 0, 3, 30),
			new PresetInventoryItem("SkillBook", 27, 1, 20),
			new PresetInventoryItem("SkillBook", 45, 1, 20),
			new PresetInventoryItem("Food", 0, 3, 30),
			new PresetInventoryItem("Food", 135, 3, 30),
			new PresetInventoryItem("Medicine", 54, 1, 20),
			new PresetInventoryItem("Medicine", 66, 1, 20),
			new PresetInventoryItem("Medicine", 82, 1, 20),
			new PresetInventoryItem("Medicine", 94, 1, 20),
			new PresetInventoryItem("Medicine", 250, 1, 10),
			new PresetInventoryItem("Medicine", 322, 1, 10),
			new PresetInventoryItem("Medicine", 106, 1, 30),
			new PresetInventoryItem("TeaWine", 27, 1, 20),
			new PresetInventoryItem("TeaWine", 9, 1, 20)
		}, new List<PresetOrgMemberCombatSkill>
		{
			new PresetOrgMemberCombatSkill(47, 4),
			new PresetOrgMemberCombatSkill(149, 4),
			new PresetOrgMemberCombatSkill(257, 4),
			new PresetOrgMemberCombatSkill(354, 4),
			new PresetOrgMemberCombatSkill(430, 4)
		}, new sbyte[5] { 4, 4, 4, 4, 4 }, new short[8] { -70, -70, -80, -50, -70, -70, -70, -70 }, 22500, 18000, 10, 60, 9300, new short[16]
		{
			12, -1, -1, 9, -1, 9, -1, -1, -1, -1,
			-1, -1, 9, -1, 2, 2
		}, 5, new short[14]
		{
			12, 12, 9, 9, 12, -1, -1, -1, 2, 2,
			-1, -1, -1, 12
		}, new short[6] { 2, 12, -1, -1, 12, 9 }, new List<sbyte> { 7 }, 3, new ResourceInts(default(int), default(int), default(int), default(int), default(int), default(int), default(int), default(int)), new IntPair[18]
		{
			new IntPair(17, 1),
			new IntPair(8, 30),
			new IntPair(5, 42),
			new IntPair(6, 0),
			new IntPair(15, 3),
			new IntPair(16, 21),
			new IntPair(7, 1),
			new IntPair(14, 1),
			new IntPair(12, 0),
			new IntPair(4, 18),
			new IntPair(3, 1),
			new IntPair(13, 1),
			new IntPair(10, 3),
			new IntPair(2, 1),
			new IntPair(1, 1),
			new IntPair(11, 1),
			new IntPair(0, 1),
			new IntPair(9, 0)
		}, null));
		_dataArray.Add(new OrganizationMemberItem(114, 535, 8, 3, 6, 9, 3, restrictPrincipalAmount: false, 0, -1, -1, -1, 3, 7, 3, 0, 0, new int[2] { 536, 537 }, 1800, 6, 3000, 2250, 0, new List<short> { 40, 41, 42 }, new List<short> { 46, 47, 48 }, new int[2] { 538, 539 }, canStroll: true, 285, new short[4] { 14, 16, 18, 20 }, new PresetEquipmentItemWithProb[8]
		{
			new PresetEquipmentItemWithProb("Armor", 117, 75),
			new PresetEquipmentItemWithProb("Armor", 387, 100),
			new PresetEquipmentItemWithProb("Armor", 513, 75),
			new PresetEquipmentItemWithProb("Armor", 243, 75),
			new PresetEquipmentItemWithProb("Accessory", 171, 75),
			new PresetEquipmentItemWithProb("Accessory", 207, 50),
			new PresetEquipmentItemWithProb("Accessory", 126, 50),
			new PresetEquipmentItemWithProb("Carrier", 9, 75)
		}, new PresetEquipmentItem("Clothing", 40), new List<PresetInventoryItem>
		{
			new PresetInventoryItem("SkillBook", 0, 3, 30),
			new PresetInventoryItem("SkillBook", 27, 1, 20),
			new PresetInventoryItem("SkillBook", 45, 1, 20),
			new PresetInventoryItem("Food", 0, 3, 30),
			new PresetInventoryItem("Food", 135, 3, 30),
			new PresetInventoryItem("Medicine", 54, 1, 20),
			new PresetInventoryItem("Medicine", 66, 1, 20),
			new PresetInventoryItem("Medicine", 82, 1, 20),
			new PresetInventoryItem("Medicine", 94, 1, 20),
			new PresetInventoryItem("Medicine", 250, 1, 10),
			new PresetInventoryItem("Medicine", 322, 1, 10),
			new PresetInventoryItem("Medicine", 106, 1, 30),
			new PresetInventoryItem("TeaWine", 27, 1, 20),
			new PresetInventoryItem("TeaWine", 9, 1, 20)
		}, new List<PresetOrgMemberCombatSkill>
		{
			new PresetOrgMemberCombatSkill(47, 3),
			new PresetOrgMemberCombatSkill(149, 3),
			new PresetOrgMemberCombatSkill(257, 3),
			new PresetOrgMemberCombatSkill(714, 3)
		}, new sbyte[5] { 4, 4, 4, 4, 4 }, new short[8] { -70, -70, -80, -50, -70, -70, -70, -70 }, 15000, 9000, 10, 70, 4500, new short[16]
		{
			12, -1, -1, 9, -1, 9, -1, -1, -1, -1,
			-1, -1, 9, -1, 2, 2
		}, 4, new short[14]
		{
			12, 12, 9, 9, 12, -1, -1, -1, 2, 2,
			-1, -1, -1, 12
		}, new short[6] { 2, 12, -1, -1, 12, 9 }, new List<sbyte> { 7 }, 3, new ResourceInts(default(int), default(int), default(int), default(int), default(int), default(int), default(int), default(int)), new IntPair[18]
		{
			new IntPair(17, 1),
			new IntPair(8, 30),
			new IntPair(5, 42),
			new IntPair(6, 0),
			new IntPair(15, 3),
			new IntPair(16, 21),
			new IntPair(7, 1),
			new IntPair(14, 1),
			new IntPair(12, 0),
			new IntPair(4, 18),
			new IntPair(3, 1),
			new IntPair(13, 1),
			new IntPair(10, 3),
			new IntPair(2, 1),
			new IntPair(1, 1),
			new IntPair(11, 1),
			new IntPair(0, 1),
			new IntPair(9, 0)
		}, null));
		_dataArray.Add(new OrganizationMemberItem(115, 540, 8, 2, 8, 12, 4, restrictPrincipalAmount: false, 0, -1, -1, -1, 2, 4, 2, 0, 0, new int[2] { 541, 542 }, 600, 4, 2000, 900, 0, new List<short> { 40, 41, 42 }, new List<short> { 46, 47, 48 }, new int[2] { 543, 544 }, canStroll: true, 286, new short[4] { 12, 13, 14, 15 }, new PresetEquipmentItemWithProb[8]
		{
			new PresetEquipmentItemWithProb("Armor", 117, 75),
			new PresetEquipmentItemWithProb("Armor", 387, 100),
			new PresetEquipmentItemWithProb("Armor", 513, 75),
			new PresetEquipmentItemWithProb("Armor", 243, 75),
			new PresetEquipmentItemWithProb("Accessory", 171, 75),
			new PresetEquipmentItemWithProb("Accessory", 207, 50),
			new PresetEquipmentItemWithProb("Accessory", 126, 50),
			new PresetEquipmentItemWithProb("Carrier", 9, 75)
		}, new PresetEquipmentItem("Clothing", 40), new List<PresetInventoryItem>
		{
			new PresetInventoryItem("SkillBook", 0, 3, 20),
			new PresetInventoryItem("SkillBook", 27, 1, 10),
			new PresetInventoryItem("SkillBook", 45, 1, 10),
			new PresetInventoryItem("Food", 0, 3, 20),
			new PresetInventoryItem("Food", 135, 3, 20),
			new PresetInventoryItem("Medicine", 106, 1, 20)
		}, new List<PresetOrgMemberCombatSkill>
		{
			new PresetOrgMemberCombatSkill(47, 2),
			new PresetOrgMemberCombatSkill(149, 2),
			new PresetOrgMemberCombatSkill(257, 2),
			new PresetOrgMemberCombatSkill(354, 2),
			new PresetOrgMemberCombatSkill(430, 2)
		}, new sbyte[5] { 2, 2, 2, 2, 2 }, new short[8] { -70, -70, -80, -50, -70, -70, -70, -70 }, 11250, 3000, 5, 80, 1800, new short[16]
		{
			12, -1, -1, 9, -1, 9, -1, -1, -1, -1,
			-1, -1, 9, -1, 2, 2
		}, 3, new short[14]
		{
			12, 12, 9, 9, 12, -1, -1, -1, 2, 2,
			-1, -1, -1, 12
		}, new short[6] { 2, 12, -1, -1, 12, 9 }, new List<sbyte> { 7 }, 3, new ResourceInts(default(int), default(int), default(int), default(int), default(int), default(int), default(int), default(int)), new IntPair[18]
		{
			new IntPair(17, 1),
			new IntPair(8, 18),
			new IntPair(5, 30),
			new IntPair(6, 0),
			new IntPair(15, 3),
			new IntPair(16, 21),
			new IntPair(7, 1),
			new IntPair(14, 1),
			new IntPair(12, 0),
			new IntPair(4, 24),
			new IntPair(3, 1),
			new IntPair(13, 1),
			new IntPair(10, 3),
			new IntPair(2, 1),
			new IntPair(1, 1),
			new IntPair(11, 1),
			new IntPair(0, 1),
			new IntPair(9, 0)
		}, null));
		_dataArray.Add(new OrganizationMemberItem(116, 378, 8, 1, 6, 9, 3, restrictPrincipalAmount: false, 0, -1, -1, -1, 1, 3, 1, 0, 0, new int[2] { 545, 546 }, 300, 2, 1000, 300, 0, new List<short> { 40, 41, 42 }, new List<short> { 46, 47, 48 }, new int[2] { 547, 548 }, canStroll: true, 287, new short[4] { 12, 13, 14, 15 }, new PresetEquipmentItemWithProb[8]
		{
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Armor", 387, 100),
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Armor", 243, 75),
			new PresetEquipmentItemWithProb("Accessory", 171, 75),
			new PresetEquipmentItemWithProb("Accessory", 207, 50),
			new PresetEquipmentItemWithProb("Accessory", -1, 0),
			new PresetEquipmentItemWithProb("Carrier", -1, 0)
		}, new PresetEquipmentItem("Clothing", 40), new List<PresetInventoryItem>
		{
			new PresetInventoryItem("SkillBook", 0, 3, 20),
			new PresetInventoryItem("SkillBook", 27, 1, 10),
			new PresetInventoryItem("SkillBook", 45, 1, 10),
			new PresetInventoryItem("Food", 0, 3, 20),
			new PresetInventoryItem("Food", 135, 3, 20),
			new PresetInventoryItem("Medicine", 106, 1, 20)
		}, new List<PresetOrgMemberCombatSkill>
		{
			new PresetOrgMemberCombatSkill(47, 1),
			new PresetOrgMemberCombatSkill(149, 1),
			new PresetOrgMemberCombatSkill(430, 1)
		}, new sbyte[5], new short[8] { -70, -70, -80, -50, -70, -70, -70, -70 }, 7500, 1500, 5, 90, 600, new short[16]
		{
			12, -1, -1, 9, -1, 9, -1, -1, -1, -1,
			-1, -1, 9, -1, 2, 2
		}, 2, new short[14]
		{
			12, 12, 9, 9, 12, -1, -1, -1, 2, 2,
			-1, -1, -1, 12
		}, new short[6] { 2, 12, -1, -1, 12, 9 }, new List<sbyte> { 7 }, 3, new ResourceInts(default(int), default(int), default(int), default(int), default(int), default(int), default(int), default(int)), new IntPair[18]
		{
			new IntPair(17, 1),
			new IntPair(8, 18),
			new IntPair(5, 30),
			new IntPair(6, 0),
			new IntPair(15, 3),
			new IntPair(16, 21),
			new IntPair(7, 1),
			new IntPair(14, 1),
			new IntPair(12, 0),
			new IntPair(4, 24),
			new IntPair(3, 1),
			new IntPair(13, 1),
			new IntPair(10, 3),
			new IntPair(2, 1),
			new IntPair(1, 1),
			new IntPair(11, 1),
			new IntPair(0, 1),
			new IntPair(9, 0)
		}, null));
		_dataArray.Add(new OrganizationMemberItem(117, 549, 8, 0, 6, 9, 3, restrictPrincipalAmount: false, 0, -1, -1, -1, 0, 3, 0, 0, 0, new int[2] { 550, 551 }, 150, 0, 500, 150, 0, new List<short> { 40, 41, 42 }, new List<short> { 46, 47, 48 }, new int[2] { 552, 553 }, canStroll: true, 288, new short[4] { 10, 10, 10, 10 }, new PresetEquipmentItemWithProb[8]
		{
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Armor", 387, 100),
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Armor", 243, 75),
			new PresetEquipmentItemWithProb("Accessory", 171, 75),
			new PresetEquipmentItemWithProb("Accessory", -1, 0),
			new PresetEquipmentItemWithProb("Accessory", -1, 0),
			new PresetEquipmentItemWithProb("Carrier", -1, 0)
		}, new PresetEquipmentItem("Clothing", 40), new List<PresetInventoryItem>
		{
			new PresetInventoryItem("SkillBook", 0, 3, 20),
			new PresetInventoryItem("SkillBook", 27, 1, 10),
			new PresetInventoryItem("SkillBook", 45, 1, 10),
			new PresetInventoryItem("Food", 0, 3, 20),
			new PresetInventoryItem("Food", 135, 3, 20),
			new PresetInventoryItem("Medicine", 106, 1, 20)
		}, new List<PresetOrgMemberCombatSkill>
		{
			new PresetOrgMemberCombatSkill(47, 0),
			new PresetOrgMemberCombatSkill(149, 0),
			new PresetOrgMemberCombatSkill(354, 0)
		}, new sbyte[5], new short[8] { -70, -70, -80, -50, -70, -70, -70, -70 }, 3750, 750, 1, 100, 300, new short[16]
		{
			12, -1, -1, 9, -1, 9, -1, -1, -1, -1,
			-1, -1, 9, -1, 2, 2
		}, 1, new short[14]
		{
			12, 12, 9, 9, 12, -1, -1, -1, 2, 2,
			-1, -1, -1, 12
		}, new short[6] { 2, 12, -1, -1, 12, 9 }, new List<sbyte> { 7 }, 3, new ResourceInts(default(int), default(int), default(int), default(int), default(int), default(int), default(int), default(int)), new IntPair[18]
		{
			new IntPair(17, 1),
			new IntPair(8, 18),
			new IntPair(5, 30),
			new IntPair(6, 0),
			new IntPair(15, 3),
			new IntPair(16, 21),
			new IntPair(7, 1),
			new IntPair(14, 1),
			new IntPair(12, 0),
			new IntPair(4, 24),
			new IntPair(3, 1),
			new IntPair(13, 1),
			new IntPair(10, 3),
			new IntPair(2, 1),
			new IntPair(1, 1),
			new IntPair(11, 1),
			new IntPair(0, 1),
			new IntPair(9, 0)
		}, null));
		_dataArray.Add(new OrganizationMemberItem(118, 554, 9, 8, 1, 1, 1, restrictPrincipalAmount: true, -1, -1, 7, 4, 7, -1, 7, 0, 0, new int[2] { 555, 556 }, 15800, 16, 13000, 30750, 0, new List<short> { 43, 44, 45, 2 }, new List<short> { 40, 41, 42 }, new int[2] { 557, 558 }, canStroll: false, 289, new short[4] { 42, 50, 58, 66 }, new PresetEquipmentItemWithProb[8]
		{
			new PresetEquipmentItemWithProb("Armor", 18, 75),
			new PresetEquipmentItemWithProb("Armor", 288, 100),
			new PresetEquipmentItemWithProb("Armor", 414, 75),
			new PresetEquipmentItemWithProb("Armor", 144, 75),
			new PresetEquipmentItemWithProb("Accessory", 108, 75),
			new PresetEquipmentItemWithProb("Accessory", 0, 50),
			new PresetEquipmentItemWithProb("Accessory", 9, 50),
			new PresetEquipmentItemWithProb("Carrier", 0, 75)
		}, new PresetEquipmentItem("Clothing", 45), new List<PresetInventoryItem>
		{
			new PresetInventoryItem("SkillBook", 54, 3, 40),
			new PresetInventoryItem("SkillBook", 63, 3, 40),
			new PresetInventoryItem("SkillBook", 90, 1, 40),
			new PresetInventoryItem("SkillBook", 99, 1, 40),
			new PresetInventoryItem("CraftTool", 0, 1, 30),
			new PresetInventoryItem("CraftTool", 9, 1, 30),
			new PresetInventoryItem("CraftTool", 18, 1, 30),
			new PresetInventoryItem("CraftTool", 27, 1, 30),
			new PresetInventoryItem("Food", 51, 3, 40),
			new PresetInventoryItem("Food", 135, 3, 40),
			new PresetInventoryItem("Medicine", 60, 1, 20),
			new PresetInventoryItem("Medicine", 72, 1, 20),
			new PresetInventoryItem("TeaWine", 18, 1, 20),
			new PresetInventoryItem("TeaWine", 0, 1, 20),
			new PresetInventoryItem("Material", 0, 1, 30),
			new PresetInventoryItem("Material", 7, 1, 30),
			new PresetInventoryItem("Material", 14, 1, 30),
			new PresetInventoryItem("Material", 21, 1, 30),
			new PresetInventoryItem("Material", 28, 1, 30),
			new PresetInventoryItem("Material", 35, 1, 30),
			new PresetInventoryItem("Material", 42, 1, 30),
			new PresetInventoryItem("Material", 49, 1, 30)
		}, new List<PresetOrgMemberCombatSkill>
		{
			new PresetOrgMemberCombatSkill(56, 5),
			new PresetOrgMemberCombatSkill(158, 6),
			new PresetOrgMemberCombatSkill(264, 6),
			new PresetOrgMemberCombatSkill(558, 7),
			new PresetOrgMemberCombatSkill(598, 7),
			new PresetOrgMemberCombatSkill(640, 7),
			new PresetOrgMemberCombatSkill(699, 8)
		}, new sbyte[5] { 10, 10, 10, 10, 10 }, new short[8] { -70, -70, -70, -70, -70, -80, -70, -60 }, 160000, 576000, 20, 20, 61500, new short[16]
		{
			-1, -1, -1, -1, -1, -1, 12, 12, 2, 2,
			9, 9, 2, 2, -1, -1
		}, 8, new short[14]
		{
			6, 6, 9, -1, -1, -1, -1, 12, 12, 12,
			-1, -1, 12, -1
		}, new short[6] { 9, -1, -1, 12, -1, 9 }, new List<sbyte> { 6, 17, 24, 25, 55, 63 }, 3, new ResourceInts(default(int), default(int), default(int), default(int), default(int), default(int), default(int), default(int)), new IntPair[18]
		{
			new IntPair(17, 1),
			new IntPair(8, 1),
			new IntPair(5, 1),
			new IntPair(6, 1),
			new IntPair(15, 24),
			new IntPair(16, 9),
			new IntPair(7, 9),
			new IntPair(14, 1),
			new IntPair(12, 1),
			new IntPair(4, 1),
			new IntPair(3, 18),
			new IntPair(13, 1),
			new IntPair(10, 6),
			new IntPair(2, 54),
			new IntPair(1, 3),
			new IntPair(11, 6),
			new IntPair(0, 6),
			new IntPair(9, 1)
		}, null));
		_dataArray.Add(new OrganizationMemberItem(119, 559, 9, 7, 4, 6, 2, restrictPrincipalAmount: false, -1, -1, -1, 3, 7, 8, 7, 0, 0, new int[2] { 560, 561 }, 8600, 14, 10000, 21150, 0, new List<short> { 43, 44, 45, 2 }, new List<short> { 40, 41, 42 }, new int[2] { 562, 563 }, canStroll: false, 290, new short[4] { 38, 45, 52, 59 }, new PresetEquipmentItemWithProb[8]
		{
			new PresetEquipmentItemWithProb("Armor", 18, 75),
			new PresetEquipmentItemWithProb("Armor", 288, 100),
			new PresetEquipmentItemWithProb("Armor", 414, 75),
			new PresetEquipmentItemWithProb("Armor", 144, 75),
			new PresetEquipmentItemWithProb("Accessory", 108, 75),
			new PresetEquipmentItemWithProb("Accessory", 0, 50),
			new PresetEquipmentItemWithProb("Accessory", 9, 50),
			new PresetEquipmentItemWithProb("Carrier", 0, 75)
		}, new PresetEquipmentItem("Clothing", 44), new List<PresetInventoryItem>
		{
			new PresetInventoryItem("SkillBook", 54, 3, 40),
			new PresetInventoryItem("SkillBook", 63, 3, 40),
			new PresetInventoryItem("SkillBook", 90, 1, 40),
			new PresetInventoryItem("SkillBook", 99, 1, 40),
			new PresetInventoryItem("CraftTool", 0, 1, 30),
			new PresetInventoryItem("CraftTool", 9, 1, 30),
			new PresetInventoryItem("CraftTool", 18, 1, 30),
			new PresetInventoryItem("CraftTool", 27, 1, 30),
			new PresetInventoryItem("Food", 51, 3, 40),
			new PresetInventoryItem("Food", 135, 3, 40),
			new PresetInventoryItem("Medicine", 60, 1, 20),
			new PresetInventoryItem("Medicine", 72, 1, 20),
			new PresetInventoryItem("TeaWine", 18, 1, 20),
			new PresetInventoryItem("TeaWine", 0, 1, 20),
			new PresetInventoryItem("Material", 0, 1, 30),
			new PresetInventoryItem("Material", 7, 1, 30),
			new PresetInventoryItem("Material", 14, 1, 30),
			new PresetInventoryItem("Material", 21, 1, 30),
			new PresetInventoryItem("Material", 28, 1, 30),
			new PresetInventoryItem("Material", 35, 1, 30),
			new PresetInventoryItem("Material", 42, 1, 30),
			new PresetInventoryItem("Material", 49, 1, 30)
		}, new List<PresetOrgMemberCombatSkill>
		{
			new PresetOrgMemberCombatSkill(56, 4),
			new PresetOrgMemberCombatSkill(158, 6),
			new PresetOrgMemberCombatSkill(264, 6),
			new PresetOrgMemberCombatSkill(558, 6),
			new PresetOrgMemberCombatSkill(598, 6),
			new PresetOrgMemberCombatSkill(640, 6),
			new PresetOrgMemberCombatSkill(699, 7)
		}, new sbyte[5] { 8, 8, 8, 8, 8 }, new short[8] { -70, -70, -70, -70, -70, -80, -70, -60 }, 100000, 288000, 15, 30, 42300, new short[16]
		{
			-1, -1, -1, -1, -1, -1, 12, 12, 2, 2,
			9, 9, 2, 2, -1, -1
		}, 8, new short[14]
		{
			6, 6, 9, -1, -1, -1, -1, 12, 12, 12,
			-1, -1, 12, -1
		}, new short[6] { 9, -1, -1, 12, -1, 9 }, new List<sbyte> { 6, 17, 24, 25, 63 }, 3, new ResourceInts(default(int), default(int), default(int), default(int), default(int), default(int), default(int), default(int)), new IntPair[18]
		{
			new IntPair(17, 1),
			new IntPair(8, 1),
			new IntPair(5, 1),
			new IntPair(6, 1),
			new IntPair(15, 24),
			new IntPair(16, 9),
			new IntPair(7, 9),
			new IntPair(14, 1),
			new IntPair(12, 1),
			new IntPair(4, 1),
			new IntPair(3, 18),
			new IntPair(13, 1),
			new IntPair(10, 6),
			new IntPair(2, 54),
			new IntPair(1, 3),
			new IntPair(11, 6),
			new IntPair(0, 6),
			new IntPair(9, 1)
		}, null));
	}

	private void CreateItems2()
	{
		_dataArray.Add(new OrganizationMemberItem(120, 564, 9, 6, 4, 6, 2, restrictPrincipalAmount: false, -1, -1, -1, 2, 6, 7, 6, 0, 0, new int[2] { 565, 566 }, 6100, 12, 8000, 13800, 0, new List<short> { 43, 44, 45, 2 }, new List<short> { 40, 41, 42 }, new int[2] { 567, 568 }, canStroll: true, 291, new short[4] { 34, 40, 46, 52 }, new PresetEquipmentItemWithProb[8]
		{
			new PresetEquipmentItemWithProb("Armor", 18, 75),
			new PresetEquipmentItemWithProb("Armor", 288, 100),
			new PresetEquipmentItemWithProb("Armor", 414, 75),
			new PresetEquipmentItemWithProb("Armor", 144, 75),
			new PresetEquipmentItemWithProb("Accessory", 108, 75),
			new PresetEquipmentItemWithProb("Accessory", 0, 50),
			new PresetEquipmentItemWithProb("Accessory", 9, 50),
			new PresetEquipmentItemWithProb("Carrier", 0, 75)
		}, new PresetEquipmentItem("Clothing", 44), new List<PresetInventoryItem>
		{
			new PresetInventoryItem("SkillBook", 54, 3, 40),
			new PresetInventoryItem("SkillBook", 63, 3, 40),
			new PresetInventoryItem("SkillBook", 90, 1, 40),
			new PresetInventoryItem("SkillBook", 99, 1, 40),
			new PresetInventoryItem("CraftTool", 0, 1, 30),
			new PresetInventoryItem("CraftTool", 9, 1, 30),
			new PresetInventoryItem("CraftTool", 18, 1, 30),
			new PresetInventoryItem("CraftTool", 27, 1, 30),
			new PresetInventoryItem("Food", 51, 3, 40),
			new PresetInventoryItem("Food", 135, 3, 40),
			new PresetInventoryItem("Medicine", 60, 1, 20),
			new PresetInventoryItem("Medicine", 72, 1, 20),
			new PresetInventoryItem("TeaWine", 18, 1, 20),
			new PresetInventoryItem("TeaWine", 0, 1, 20),
			new PresetInventoryItem("Material", 0, 1, 30),
			new PresetInventoryItem("Material", 7, 1, 30),
			new PresetInventoryItem("Material", 14, 1, 30),
			new PresetInventoryItem("Material", 21, 1, 30),
			new PresetInventoryItem("Material", 28, 1, 30),
			new PresetInventoryItem("Material", 35, 1, 30),
			new PresetInventoryItem("Material", 42, 1, 30),
			new PresetInventoryItem("Material", 49, 1, 30)
		}, new List<PresetOrgMemberCombatSkill>
		{
			new PresetOrgMemberCombatSkill(56, 4),
			new PresetOrgMemberCombatSkill(158, 5),
			new PresetOrgMemberCombatSkill(264, 5),
			new PresetOrgMemberCombatSkill(558, 6),
			new PresetOrgMemberCombatSkill(699, 6)
		}, new sbyte[5] { 8, 8, 8, 8, 8 }, new short[8] { -70, -70, -70, -70, -70, -80, -70, -60 }, 60000, 144000, 15, 40, 27600, new short[16]
		{
			-1, -1, -1, -1, -1, -1, 12, 12, 2, 2,
			9, 9, 2, 2, -1, -1
		}, 7, new short[14]
		{
			6, 6, 9, -1, -1, -1, -1, 12, 12, 12,
			-1, -1, 12, -1
		}, new short[6] { 9, -1, -1, 12, -1, 9 }, new List<sbyte> { 6, 17, 24, 25, 63 }, 3, new ResourceInts(default(int), default(int), default(int), default(int), default(int), default(int), default(int), default(int)), new IntPair[18]
		{
			new IntPair(17, 1),
			new IntPair(8, 1),
			new IntPair(5, 1),
			new IntPair(6, 1),
			new IntPair(15, 24),
			new IntPair(16, 9),
			new IntPair(7, 9),
			new IntPair(14, 1),
			new IntPair(12, 1),
			new IntPair(4, 1),
			new IntPair(3, 18),
			new IntPair(13, 1),
			new IntPair(10, 6),
			new IntPair(2, 54),
			new IntPair(1, 3),
			new IntPair(11, 6),
			new IntPair(0, 6),
			new IntPair(9, 1)
		}, null));
		_dataArray.Add(new OrganizationMemberItem(121, 569, 9, 5, 4, 6, 2, restrictPrincipalAmount: false, -1, -1, -1, 2, 5, 7, 5, 0, 0, new int[2] { 570, 571 }, 4200, 10, 6000, 8400, 0, new List<short> { 43, 44, 45, 2 }, new List<short> { 40, 41, 42 }, new int[2] { 572, 573 }, canStroll: true, 292, new short[4] { 30, 35, 40, 45 }, new PresetEquipmentItemWithProb[8]
		{
			new PresetEquipmentItemWithProb("Armor", 18, 75),
			new PresetEquipmentItemWithProb("Armor", 288, 100),
			new PresetEquipmentItemWithProb("Armor", 414, 75),
			new PresetEquipmentItemWithProb("Armor", 144, 75),
			new PresetEquipmentItemWithProb("Accessory", 108, 75),
			new PresetEquipmentItemWithProb("Accessory", 0, 50),
			new PresetEquipmentItemWithProb("Accessory", 9, 50),
			new PresetEquipmentItemWithProb("Carrier", 0, 75)
		}, new PresetEquipmentItem("Clothing", 44), new List<PresetInventoryItem>
		{
			new PresetInventoryItem("SkillBook", 54, 3, 30),
			new PresetInventoryItem("SkillBook", 63, 3, 30),
			new PresetInventoryItem("SkillBook", 90, 1, 30),
			new PresetInventoryItem("SkillBook", 99, 1, 30),
			new PresetInventoryItem("CraftTool", 0, 1, 30),
			new PresetInventoryItem("CraftTool", 9, 1, 30),
			new PresetInventoryItem("CraftTool", 18, 1, 30),
			new PresetInventoryItem("CraftTool", 27, 1, 30),
			new PresetInventoryItem("Food", 51, 3, 30),
			new PresetInventoryItem("Food", 135, 3, 30),
			new PresetInventoryItem("Medicine", 54, 1, 20),
			new PresetInventoryItem("Medicine", 66, 1, 20),
			new PresetInventoryItem("TeaWine", 18, 1, 20),
			new PresetInventoryItem("TeaWine", 0, 1, 20),
			new PresetInventoryItem("Material", 0, 1, 20),
			new PresetInventoryItem("Material", 7, 1, 20),
			new PresetInventoryItem("Material", 14, 1, 20),
			new PresetInventoryItem("Material", 21, 1, 20),
			new PresetInventoryItem("Material", 28, 1, 20),
			new PresetInventoryItem("Material", 35, 1, 20),
			new PresetInventoryItem("Material", 42, 1, 20),
			new PresetInventoryItem("Material", 49, 1, 20)
		}, new List<PresetOrgMemberCombatSkill>
		{
			new PresetOrgMemberCombatSkill(56, 3),
			new PresetOrgMemberCombatSkill(158, 5),
			new PresetOrgMemberCombatSkill(264, 5),
			new PresetOrgMemberCombatSkill(598, 5),
			new PresetOrgMemberCombatSkill(699, 5)
		}, new sbyte[5] { 6, 6, 6, 6, 6 }, new short[8] { -70, -70, -70, -70, -70, -80, -70, -60 }, 40000, 48000, 10, 50, 16800, new short[16]
		{
			-1, -1, -1, -1, -1, -1, 12, 12, 2, 2,
			9, 9, 2, 2, -1, -1
		}, 6, new short[14]
		{
			6, 6, 9, -1, -1, -1, -1, 12, 12, 12,
			-1, -1, 12, -1
		}, new short[6] { 9, -1, -1, 12, -1, 9 }, new List<sbyte> { 6, 63 }, 3, new ResourceInts(default(int), default(int), default(int), default(int), default(int), default(int), default(int), default(int)), new IntPair[18]
		{
			new IntPair(17, 1),
			new IntPair(8, 1),
			new IntPair(5, 1),
			new IntPair(6, 1),
			new IntPair(15, 21),
			new IntPair(16, 6),
			new IntPair(7, 6),
			new IntPair(14, 1),
			new IntPair(12, 1),
			new IntPair(4, 1),
			new IntPair(3, 21),
			new IntPair(13, 1),
			new IntPair(10, 6),
			new IntPair(2, 42),
			new IntPair(1, 3),
			new IntPair(11, 6),
			new IntPair(0, 6),
			new IntPair(9, 1)
		}, null));
		_dataArray.Add(new OrganizationMemberItem(122, 574, 9, 4, 6, 9, 3, restrictPrincipalAmount: false, -1, -1, -1, 1, 4, 6, 4, 0, 0, new int[2] { 575, 576 }, 2800, 8, 4500, 4650, 0, new List<short> { 43, 44, 45, 2 }, new List<short> { 40, 41, 42 }, new int[2] { 577, 578 }, canStroll: true, 293, new short[4] { 26, 30, 34, 38 }, new PresetEquipmentItemWithProb[8]
		{
			new PresetEquipmentItemWithProb("Armor", 18, 75),
			new PresetEquipmentItemWithProb("Armor", 288, 100),
			new PresetEquipmentItemWithProb("Armor", 414, 75),
			new PresetEquipmentItemWithProb("Armor", 144, 75),
			new PresetEquipmentItemWithProb("Accessory", 108, 75),
			new PresetEquipmentItemWithProb("Accessory", 0, 50),
			new PresetEquipmentItemWithProb("Accessory", 9, 50),
			new PresetEquipmentItemWithProb("Carrier", 0, 75)
		}, new PresetEquipmentItem("Clothing", 44), new List<PresetInventoryItem>
		{
			new PresetInventoryItem("SkillBook", 54, 3, 30),
			new PresetInventoryItem("SkillBook", 63, 3, 30),
			new PresetInventoryItem("SkillBook", 90, 1, 30),
			new PresetInventoryItem("SkillBook", 99, 1, 30),
			new PresetInventoryItem("CraftTool", 0, 1, 30),
			new PresetInventoryItem("CraftTool", 9, 1, 30),
			new PresetInventoryItem("CraftTool", 18, 1, 30),
			new PresetInventoryItem("CraftTool", 27, 1, 30),
			new PresetInventoryItem("Food", 51, 3, 30),
			new PresetInventoryItem("Food", 135, 3, 30),
			new PresetInventoryItem("Medicine", 54, 1, 20),
			new PresetInventoryItem("Medicine", 66, 1, 20),
			new PresetInventoryItem("TeaWine", 18, 1, 20),
			new PresetInventoryItem("TeaWine", 0, 1, 20),
			new PresetInventoryItem("Material", 0, 1, 20),
			new PresetInventoryItem("Material", 7, 1, 20),
			new PresetInventoryItem("Material", 14, 1, 20),
			new PresetInventoryItem("Material", 21, 1, 20),
			new PresetInventoryItem("Material", 28, 1, 20),
			new PresetInventoryItem("Material", 35, 1, 20),
			new PresetInventoryItem("Material", 42, 1, 20),
			new PresetInventoryItem("Material", 49, 1, 20)
		}, new List<PresetOrgMemberCombatSkill>
		{
			new PresetOrgMemberCombatSkill(56, 3),
			new PresetOrgMemberCombatSkill(158, 4),
			new PresetOrgMemberCombatSkill(264, 4),
			new PresetOrgMemberCombatSkill(640, 4),
			new PresetOrgMemberCombatSkill(699, 4)
		}, new sbyte[5] { 4, 4, 4, 4, 4 }, new short[8] { -70, -70, -70, -70, -70, -80, -70, -60 }, 30000, 24000, 10, 60, 9300, new short[16]
		{
			-1, -1, -1, -1, -1, -1, 12, 12, 2, 2,
			9, 9, 2, 2, -1, -1
		}, 5, new short[14]
		{
			6, 6, 9, -1, -1, -1, -1, 12, 12, 12,
			-1, -1, 12, -1
		}, new short[6] { 9, -1, -1, 12, -1, 9 }, new List<sbyte> { 6, 63 }, 3, new ResourceInts(default(int), default(int), default(int), default(int), default(int), default(int), default(int), default(int)), new IntPair[18]
		{
			new IntPair(17, 1),
			new IntPair(8, 1),
			new IntPair(5, 1),
			new IntPair(6, 1),
			new IntPair(15, 21),
			new IntPair(16, 6),
			new IntPair(7, 6),
			new IntPair(14, 1),
			new IntPair(12, 1),
			new IntPair(4, 1),
			new IntPair(3, 21),
			new IntPair(13, 1),
			new IntPair(10, 6),
			new IntPair(2, 42),
			new IntPair(1, 3),
			new IntPair(11, 6),
			new IntPair(0, 6),
			new IntPair(9, 1)
		}, null));
		_dataArray.Add(new OrganizationMemberItem(123, 579, 9, 3, 6, 9, 3, restrictPrincipalAmount: false, -1, -1, -1, 1, 3, 5, 3, 0, 0, new int[2] { 580, 581 }, 1800, 6, 3000, 2250, 0, new List<short> { 43, 44, 45, 2 }, new List<short> { 40, 41, 42 }, new int[2] { 582, 583 }, canStroll: true, 294, new short[4] { 22, 25, 28, 31 }, new PresetEquipmentItemWithProb[8]
		{
			new PresetEquipmentItemWithProb("Armor", 18, 75),
			new PresetEquipmentItemWithProb("Armor", 288, 100),
			new PresetEquipmentItemWithProb("Armor", 414, 75),
			new PresetEquipmentItemWithProb("Armor", 144, 75),
			new PresetEquipmentItemWithProb("Accessory", 108, 75),
			new PresetEquipmentItemWithProb("Accessory", 0, 50),
			new PresetEquipmentItemWithProb("Accessory", 9, 50),
			new PresetEquipmentItemWithProb("Carrier", 0, 75)
		}, new PresetEquipmentItem("Clothing", 43), new List<PresetInventoryItem>
		{
			new PresetInventoryItem("SkillBook", 54, 3, 30),
			new PresetInventoryItem("SkillBook", 63, 3, 30),
			new PresetInventoryItem("SkillBook", 90, 1, 30),
			new PresetInventoryItem("SkillBook", 99, 1, 30),
			new PresetInventoryItem("CraftTool", 0, 1, 30),
			new PresetInventoryItem("CraftTool", 9, 1, 30),
			new PresetInventoryItem("CraftTool", 18, 1, 30),
			new PresetInventoryItem("CraftTool", 27, 1, 30),
			new PresetInventoryItem("Food", 51, 3, 30),
			new PresetInventoryItem("Food", 135, 3, 30),
			new PresetInventoryItem("Medicine", 54, 1, 20),
			new PresetInventoryItem("Medicine", 66, 1, 20),
			new PresetInventoryItem("TeaWine", 18, 1, 20),
			new PresetInventoryItem("TeaWine", 0, 1, 20),
			new PresetInventoryItem("Material", 0, 1, 20),
			new PresetInventoryItem("Material", 7, 1, 20),
			new PresetInventoryItem("Material", 14, 1, 20),
			new PresetInventoryItem("Material", 21, 1, 20),
			new PresetInventoryItem("Material", 28, 1, 20),
			new PresetInventoryItem("Material", 35, 1, 20),
			new PresetInventoryItem("Material", 42, 1, 20),
			new PresetInventoryItem("Material", 49, 1, 20)
		}, new List<PresetOrgMemberCombatSkill>
		{
			new PresetOrgMemberCombatSkill(56, 2),
			new PresetOrgMemberCombatSkill(158, 3),
			new PresetOrgMemberCombatSkill(264, 3),
			new PresetOrgMemberCombatSkill(558, 3),
			new PresetOrgMemberCombatSkill(598, 3),
			new PresetOrgMemberCombatSkill(640, 3),
			new PresetOrgMemberCombatSkill(699, 3)
		}, new sbyte[5] { 4, 4, 4, 4, 4 }, new short[8] { -70, -70, -70, -70, -70, -80, -70, -60 }, 20000, 12000, 10, 70, 4500, new short[16]
		{
			-1, -1, -1, -1, -1, -1, 12, 12, 2, 2,
			9, 9, 2, 2, -1, -1
		}, 4, new short[14]
		{
			6, 6, 9, -1, -1, -1, -1, 12, 12, 12,
			-1, -1, 12, -1
		}, new short[6] { 9, -1, -1, 12, -1, 9 }, new List<sbyte> { 6, 63 }, 3, new ResourceInts(default(int), default(int), default(int), default(int), default(int), default(int), default(int), default(int)), new IntPair[18]
		{
			new IntPair(17, 1),
			new IntPair(8, 1),
			new IntPair(5, 1),
			new IntPair(6, 1),
			new IntPair(15, 21),
			new IntPair(16, 6),
			new IntPair(7, 6),
			new IntPair(14, 1),
			new IntPair(12, 1),
			new IntPair(4, 1),
			new IntPair(3, 21),
			new IntPair(13, 1),
			new IntPair(10, 6),
			new IntPair(2, 42),
			new IntPair(1, 3),
			new IntPair(11, 6),
			new IntPair(0, 6),
			new IntPair(9, 1)
		}, null));
		_dataArray.Add(new OrganizationMemberItem(124, 584, 9, 2, 6, 9, 3, restrictPrincipalAmount: false, -1, -1, -1, 1, 2, 4, 2, 0, 0, new int[2] { 585, 586 }, 600, 4, 2000, 900, 0, new List<short> { 43, 44, 45, 2 }, new List<short> { 40, 41, 42 }, new int[2] { 587, 588 }, canStroll: true, 295, new short[4] { 18, 20, 22, 24 }, new PresetEquipmentItemWithProb[8]
		{
			new PresetEquipmentItemWithProb("Armor", 18, 75),
			new PresetEquipmentItemWithProb("Armor", 288, 100),
			new PresetEquipmentItemWithProb("Armor", 414, 75),
			new PresetEquipmentItemWithProb("Armor", 144, 75),
			new PresetEquipmentItemWithProb("Accessory", 108, 75),
			new PresetEquipmentItemWithProb("Accessory", 0, 50),
			new PresetEquipmentItemWithProb("Accessory", 9, 50),
			new PresetEquipmentItemWithProb("Carrier", 0, 75)
		}, new PresetEquipmentItem("Clothing", 43), new List<PresetInventoryItem>
		{
			new PresetInventoryItem("SkillBook", 54, 3, 20),
			new PresetInventoryItem("SkillBook", 63, 3, 20),
			new PresetInventoryItem("SkillBook", 90, 1, 20),
			new PresetInventoryItem("SkillBook", 99, 1, 20),
			new PresetInventoryItem("CraftTool", 0, 1, 20),
			new PresetInventoryItem("CraftTool", 9, 1, 20),
			new PresetInventoryItem("CraftTool", 18, 1, 20),
			new PresetInventoryItem("CraftTool", 27, 1, 20),
			new PresetInventoryItem("Food", 51, 3, 20),
			new PresetInventoryItem("Food", 135, 3, 20),
			new PresetInventoryItem("Material", 0, 1, 10),
			new PresetInventoryItem("Material", 7, 1, 10),
			new PresetInventoryItem("Material", 14, 1, 10),
			new PresetInventoryItem("Material", 21, 1, 10),
			new PresetInventoryItem("Material", 28, 1, 10),
			new PresetInventoryItem("Material", 35, 1, 10),
			new PresetInventoryItem("Material", 42, 1, 10),
			new PresetInventoryItem("Material", 49, 1, 10)
		}, new List<PresetOrgMemberCombatSkill>
		{
			new PresetOrgMemberCombatSkill(56, 2),
			new PresetOrgMemberCombatSkill(158, 2),
			new PresetOrgMemberCombatSkill(264, 2),
			new PresetOrgMemberCombatSkill(699, 2)
		}, new sbyte[5] { 2, 2, 2, 2, 2 }, new short[8] { -70, -70, -70, -70, -70, -80, -70, -60 }, 15000, 4000, 5, 80, 1800, new short[16]
		{
			-1, -1, -1, -1, -1, -1, 12, 12, 2, 2,
			9, 9, 2, 2, -1, -1
		}, 3, new short[14]
		{
			6, 6, 9, -1, -1, -1, -1, 12, 12, 12,
			-1, -1, 12, -1
		}, new short[6] { 9, -1, -1, 12, -1, 9 }, new List<sbyte> { 6, 63 }, 3, new ResourceInts(default(int), default(int), default(int), default(int), default(int), default(int), default(int), default(int)), new IntPair[18]
		{
			new IntPair(17, 1),
			new IntPair(8, 1),
			new IntPair(5, 1),
			new IntPair(6, 1),
			new IntPair(15, 18),
			new IntPair(16, 3),
			new IntPair(7, 3),
			new IntPair(14, 1),
			new IntPair(12, 1),
			new IntPair(4, 1),
			new IntPair(3, 24),
			new IntPair(13, 1),
			new IntPair(10, 6),
			new IntPair(2, 30),
			new IntPair(1, 3),
			new IntPair(11, 6),
			new IntPair(0, 6),
			new IntPair(9, 1)
		}, null));
		_dataArray.Add(new OrganizationMemberItem(125, 589, 9, 1, 8, 12, 4, restrictPrincipalAmount: false, -1, -1, -1, 0, 1, 3, 1, 0, 0, new int[2] { 590, 591 }, 300, 2, 1000, 300, 0, new List<short> { 43, 44, 45, 2 }, new List<short> { 40, 41, 42 }, new int[2] { 592, 593 }, canStroll: true, 296, new short[4] { 14, 15, 16, 17 }, new PresetEquipmentItemWithProb[8]
		{
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Armor", 288, 100),
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Armor", 144, 75),
			new PresetEquipmentItemWithProb("Accessory", 108, 75),
			new PresetEquipmentItemWithProb("Accessory", 0, 50),
			new PresetEquipmentItemWithProb("Accessory", -1, 0),
			new PresetEquipmentItemWithProb("Carrier", -1, 0)
		}, new PresetEquipmentItem("Clothing", 43), new List<PresetInventoryItem>
		{
			new PresetInventoryItem("SkillBook", 54, 3, 20),
			new PresetInventoryItem("SkillBook", 63, 3, 20),
			new PresetInventoryItem("SkillBook", 90, 1, 20),
			new PresetInventoryItem("SkillBook", 99, 1, 20),
			new PresetInventoryItem("CraftTool", 0, 1, 20),
			new PresetInventoryItem("CraftTool", 9, 1, 20),
			new PresetInventoryItem("CraftTool", 18, 1, 20),
			new PresetInventoryItem("CraftTool", 27, 1, 20),
			new PresetInventoryItem("Food", 51, 3, 20),
			new PresetInventoryItem("Food", 135, 3, 20),
			new PresetInventoryItem("Material", 0, 1, 10),
			new PresetInventoryItem("Material", 7, 1, 10),
			new PresetInventoryItem("Material", 14, 1, 10),
			new PresetInventoryItem("Material", 21, 1, 10),
			new PresetInventoryItem("Material", 28, 1, 10),
			new PresetInventoryItem("Material", 35, 1, 10),
			new PresetInventoryItem("Material", 42, 1, 10),
			new PresetInventoryItem("Material", 49, 1, 10)
		}, new List<PresetOrgMemberCombatSkill>
		{
			new PresetOrgMemberCombatSkill(56, 1),
			new PresetOrgMemberCombatSkill(158, 1),
			new PresetOrgMemberCombatSkill(558, 1),
			new PresetOrgMemberCombatSkill(598, 1),
			new PresetOrgMemberCombatSkill(640, 1)
		}, new sbyte[5], new short[8] { -70, -70, -70, -70, -70, -80, -70, -60 }, 10000, 2000, 5, 90, 600, new short[16]
		{
			-1, -1, -1, -1, -1, -1, 12, 12, 2, 2,
			9, 9, 2, 2, -1, -1
		}, 2, new short[14]
		{
			6, 6, 9, -1, -1, -1, -1, 12, 12, 12,
			-1, -1, 12, -1
		}, new short[6] { 9, -1, -1, 12, -1, 9 }, new List<sbyte> { 6, 63 }, 3, new ResourceInts(default(int), default(int), default(int), default(int), default(int), default(int), default(int), default(int)), new IntPair[18]
		{
			new IntPair(17, 1),
			new IntPair(8, 1),
			new IntPair(5, 1),
			new IntPair(6, 1),
			new IntPair(15, 18),
			new IntPair(16, 3),
			new IntPair(7, 3),
			new IntPair(14, 1),
			new IntPair(12, 1),
			new IntPair(4, 1),
			new IntPair(3, 24),
			new IntPair(13, 1),
			new IntPair(10, 6),
			new IntPair(2, 30),
			new IntPair(1, 3),
			new IntPair(11, 6),
			new IntPair(0, 6),
			new IntPair(9, 1)
		}, null));
		_dataArray.Add(new OrganizationMemberItem(126, 594, 9, 0, 6, 9, 3, restrictPrincipalAmount: false, -1, -1, -1, 0, 0, 2, 0, 0, 0, new int[2] { 595, 596 }, 150, 0, 500, 150, 0, new List<short> { 43, 44, 45, 2 }, new List<short> { 40, 41, 42 }, new int[2] { 597, 598 }, canStroll: true, 297, new short[4] { 10, 10, 10, 10 }, new PresetEquipmentItemWithProb[8]
		{
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Armor", 288, 100),
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Armor", 144, 75),
			new PresetEquipmentItemWithProb("Accessory", 108, 75),
			new PresetEquipmentItemWithProb("Accessory", -1, 0),
			new PresetEquipmentItemWithProb("Accessory", -1, 0),
			new PresetEquipmentItemWithProb("Carrier", -1, 0)
		}, new PresetEquipmentItem("Clothing", 43), new List<PresetInventoryItem>
		{
			new PresetInventoryItem("SkillBook", 54, 3, 20),
			new PresetInventoryItem("SkillBook", 63, 3, 20),
			new PresetInventoryItem("SkillBook", 90, 1, 20),
			new PresetInventoryItem("SkillBook", 99, 1, 20),
			new PresetInventoryItem("CraftTool", 0, 1, 20),
			new PresetInventoryItem("CraftTool", 9, 1, 20),
			new PresetInventoryItem("CraftTool", 18, 1, 20),
			new PresetInventoryItem("CraftTool", 27, 1, 20),
			new PresetInventoryItem("Food", 51, 3, 20),
			new PresetInventoryItem("Food", 135, 3, 20),
			new PresetInventoryItem("Material", 0, 1, 10),
			new PresetInventoryItem("Material", 7, 1, 10),
			new PresetInventoryItem("Material", 14, 1, 10),
			new PresetInventoryItem("Material", 21, 1, 10),
			new PresetInventoryItem("Material", 28, 1, 10),
			new PresetInventoryItem("Material", 35, 1, 10),
			new PresetInventoryItem("Material", 42, 1, 10),
			new PresetInventoryItem("Material", 49, 1, 10)
		}, new List<PresetOrgMemberCombatSkill>
		{
			new PresetOrgMemberCombatSkill(56, 0),
			new PresetOrgMemberCombatSkill(158, 0),
			new PresetOrgMemberCombatSkill(640, 0)
		}, new sbyte[5], new short[8] { -70, -70, -70, -70, -70, -80, -70, -60 }, 5000, 1000, 1, 100, 300, new short[16]
		{
			-1, -1, -1, -1, -1, -1, 12, 12, 2, 2,
			9, 9, 2, 2, -1, -1
		}, 1, new short[14]
		{
			6, 6, 9, -1, -1, -1, -1, 12, 12, 12,
			-1, -1, 12, -1
		}, new short[6] { 9, -1, -1, 12, -1, 9 }, new List<sbyte> { 6, 63 }, 3, new ResourceInts(default(int), default(int), default(int), default(int), default(int), default(int), default(int), default(int)), new IntPair[18]
		{
			new IntPair(17, 1),
			new IntPair(8, 1),
			new IntPair(5, 1),
			new IntPair(6, 1),
			new IntPair(15, 18),
			new IntPair(16, 3),
			new IntPair(7, 3),
			new IntPair(14, 1),
			new IntPair(12, 1),
			new IntPair(4, 1),
			new IntPair(3, 24),
			new IntPair(13, 1),
			new IntPair(10, 6),
			new IntPair(2, 30),
			new IntPair(1, 3),
			new IntPair(11, 6),
			new IntPair(0, 6),
			new IntPair(9, 1)
		}, null));
		_dataArray.Add(new OrganizationMemberItem(127, 262, 10, 8, 1, 1, 1, restrictPrincipalAmount: true, -1, -1, 7, 5, 6, -1, 7, 0, 0, new int[2] { 599, 600 }, 15800, 16, 13000, 30750, 0, new List<short> { 46, 47, 48 }, new List<short> { 37, 38, 39 }, new int[2] { 265, 266 }, canStroll: false, 298, new short[4] { 42, 50, 58, 66 }, new PresetEquipmentItemWithProb[8]
		{
			new PresetEquipmentItemWithProb("Armor", 54, 75),
			new PresetEquipmentItemWithProb("Armor", 315, 100),
			new PresetEquipmentItemWithProb("Armor", 468, 75),
			new PresetEquipmentItemWithProb("Armor", 171, 75),
			new PresetEquipmentItemWithProb("Accessory", 108, 75),
			new PresetEquipmentItemWithProb("Accessory", 144, 50),
			new PresetEquipmentItemWithProb("Accessory", 126, 50),
			new PresetEquipmentItemWithProb("Carrier", 9, 75)
		}, new PresetEquipmentItem("Clothing", 48), new List<PresetInventoryItem>
		{
			new PresetInventoryItem("SkillBook", 72, 3, 40),
			new PresetInventoryItem("SkillBook", 81, 3, 40),
			new PresetInventoryItem("Material", 42, 1, 30),
			new PresetInventoryItem("Material", 49, 1, 30),
			new PresetInventoryItem("CraftTool", 45, 1, 30),
			new PresetInventoryItem("Food", 93, 3, 40),
			new PresetInventoryItem("Medicine", 0, 1, 30),
			new PresetInventoryItem("Medicine", 9, 1, 30),
			new PresetInventoryItem("Medicine", 18, 1, 30),
			new PresetInventoryItem("Medicine", 27, 1, 30),
			new PresetInventoryItem("Medicine", 36, 1, 30),
			new PresetInventoryItem("Medicine", 45, 1, 30),
			new PresetInventoryItem("Medicine", 60, 1, 20),
			new PresetInventoryItem("Medicine", 72, 1, 20),
			new PresetInventoryItem("Medicine", 88, 1, 20),
			new PresetInventoryItem("Medicine", 100, 1, 20),
			new PresetInventoryItem("Medicine", 136, 1, 20),
			new PresetInventoryItem("Medicine", 148, 1, 20),
			new PresetInventoryItem("Medicine", 160, 1, 20),
			new PresetInventoryItem("Medicine", 172, 1, 20),
			new PresetInventoryItem("Medicine", 184, 1, 20),
			new PresetInventoryItem("Medicine", 196, 1, 20),
			new PresetInventoryItem("Medicine", 232, 1, 20),
			new PresetInventoryItem("Medicine", 244, 1, 20),
			new PresetInventoryItem("Medicine", 256, 1, 20),
			new PresetInventoryItem("Medicine", 280, 1, 20),
			new PresetInventoryItem("Medicine", 304, 1, 20),
			new PresetInventoryItem("Medicine", 328, 1, 20),
			new PresetInventoryItem("TeaWine", 18, 1, 20),
			new PresetInventoryItem("TeaWine", 27, 1, 20),
			new PresetInventoryItem("Material", 140, 1, 10),
			new PresetInventoryItem("Material", 144, 1, 10),
			new PresetInventoryItem("Material", 148, 1, 10),
			new PresetInventoryItem("Material", 152, 1, 10),
			new PresetInventoryItem("Material", 156, 1, 10),
			new PresetInventoryItem("Material", 160, 1, 10),
			new PresetInventoryItem("Material", 164, 1, 10),
			new PresetInventoryItem("Material", 168, 1, 10),
			new PresetInventoryItem("Material", 172, 1, 10),
			new PresetInventoryItem("Material", 176, 1, 10),
			new PresetInventoryItem("Material", 180, 1, 10),
			new PresetInventoryItem("Material", 184, 1, 10),
			new PresetInventoryItem("Material", 188, 1, 10),
			new PresetInventoryItem("Material", 192, 1, 10),
			new PresetInventoryItem("Material", 196, 1, 10),
			new PresetInventoryItem("Material", 200, 1, 10),
			new PresetInventoryItem("Material", 204, 1, 10),
			new PresetInventoryItem("Material", 208, 1, 10),
			new PresetInventoryItem("Material", 212, 1, 10),
			new PresetInventoryItem("Material", 216, 1, 10),
			new PresetInventoryItem("Material", 220, 1, 10),
			new PresetInventoryItem("Material", 224, 1, 10),
			new PresetInventoryItem("Material", 228, 1, 10),
			new PresetInventoryItem("Material", 232, 1, 10),
			new PresetInventoryItem("Material", 236, 1, 10),
			new PresetInventoryItem("Material", 243, 1, 10),
			new PresetInventoryItem("Material", 250, 1, 10),
			new PresetInventoryItem("Material", 257, 1, 10),
			new PresetInventoryItem("Material", 264, 1, 10),
			new PresetInventoryItem("Material", 271, 1, 10)
		}, new List<PresetOrgMemberCombatSkill>
		{
			new PresetOrgMemberCombatSkill(61, 4),
			new PresetOrgMemberCombatSkill(165, 4),
			new PresetOrgMemberCombatSkill(271, 8),
			new PresetOrgMemberCombatSkill(361, 5),
			new PresetOrgMemberCombatSkill(438, 5),
			new PresetOrgMemberCombatSkill(478, 7),
			new PresetOrgMemberCombatSkill(494, 8)
		}, new sbyte[5] { 10, 10, 10, 10, 10 }, new short[8] { -70, -70, -70, -80, -70, -50, -70, -80 }, 160000, 576000, 20, 20, 61500, new short[16]
		{
			-1, -1, -1, -1, -1, 2, -1, -1, 12, 12,
			6, 6, -1, -1, -1, 2
		}, 8, new short[14]
		{
			6, 6, 12, 6, 6, 12, 12, -1, -1, -1,
			-1, -1, -1, -1
		}, new short[6] { -1, 9, -1, 9, 9, 9 }, new List<sbyte> { 5, 18, 24, 25, 55, 64 }, 3, new ResourceInts(default(int), default(int), default(int), default(int), default(int), default(int), default(int), default(int)), new IntPair[18]
		{
			new IntPair(17, 1),
			new IntPair(8, 3),
			new IntPair(5, 1),
			new IntPair(6, 1),
			new IntPair(15, 3),
			new IntPair(16, 3),
			new IntPair(7, 0),
			new IntPair(14, 9),
			new IntPair(12, 1),
			new IntPair(4, 12),
			new IntPair(3, 1),
			new IntPair(13, 54),
			new IntPair(10, 3),
			new IntPair(2, 3),
			new IntPair(1, 9),
			new IntPair(11, 9),
			new IntPair(0, 30),
			new IntPair(9, 1)
		}, null));
		_dataArray.Add(new OrganizationMemberItem(128, 601, 10, 7, 4, 6, 2, restrictPrincipalAmount: false, -1, -1, -1, 4, 6, -1, 7, 0, 0, new int[2] { 602, 603 }, 8600, 14, 10000, 21150, 0, new List<short> { 46, 47, 48 }, new List<short> { 37, 38, 39 }, new int[2] { 604, 605 }, canStroll: false, 299, new short[4] { 38, 45, 52, 59 }, new PresetEquipmentItemWithProb[8]
		{
			new PresetEquipmentItemWithProb("Armor", 54, 75),
			new PresetEquipmentItemWithProb("Armor", 315, 100),
			new PresetEquipmentItemWithProb("Armor", 468, 75),
			new PresetEquipmentItemWithProb("Armor", 171, 75),
			new PresetEquipmentItemWithProb("Accessory", 108, 75),
			new PresetEquipmentItemWithProb("Accessory", 144, 50),
			new PresetEquipmentItemWithProb("Accessory", 126, 50),
			new PresetEquipmentItemWithProb("Carrier", 9, 75)
		}, new PresetEquipmentItem("Clothing", 47), new List<PresetInventoryItem>
		{
			new PresetInventoryItem("SkillBook", 72, 3, 40),
			new PresetInventoryItem("SkillBook", 81, 3, 40),
			new PresetInventoryItem("Material", 42, 1, 30),
			new PresetInventoryItem("Material", 49, 1, 30),
			new PresetInventoryItem("CraftTool", 45, 1, 30),
			new PresetInventoryItem("Food", 93, 3, 40),
			new PresetInventoryItem("Medicine", 0, 1, 30),
			new PresetInventoryItem("Medicine", 9, 1, 30),
			new PresetInventoryItem("Medicine", 18, 1, 30),
			new PresetInventoryItem("Medicine", 27, 1, 30),
			new PresetInventoryItem("Medicine", 36, 1, 30),
			new PresetInventoryItem("Medicine", 45, 1, 30),
			new PresetInventoryItem("Medicine", 60, 1, 20),
			new PresetInventoryItem("Medicine", 72, 1, 20),
			new PresetInventoryItem("Medicine", 88, 1, 20),
			new PresetInventoryItem("Medicine", 100, 1, 20),
			new PresetInventoryItem("Medicine", 136, 1, 20),
			new PresetInventoryItem("Medicine", 148, 1, 20),
			new PresetInventoryItem("Medicine", 160, 1, 20),
			new PresetInventoryItem("Medicine", 172, 1, 20),
			new PresetInventoryItem("Medicine", 184, 1, 20),
			new PresetInventoryItem("Medicine", 196, 1, 20),
			new PresetInventoryItem("Medicine", 232, 1, 20),
			new PresetInventoryItem("Medicine", 244, 1, 20),
			new PresetInventoryItem("Medicine", 256, 1, 20),
			new PresetInventoryItem("Medicine", 280, 1, 20),
			new PresetInventoryItem("Medicine", 304, 1, 20),
			new PresetInventoryItem("Medicine", 328, 1, 20),
			new PresetInventoryItem("TeaWine", 18, 1, 20),
			new PresetInventoryItem("TeaWine", 27, 1, 20),
			new PresetInventoryItem("Material", 140, 1, 10),
			new PresetInventoryItem("Material", 144, 1, 10),
			new PresetInventoryItem("Material", 148, 1, 10),
			new PresetInventoryItem("Material", 152, 1, 10),
			new PresetInventoryItem("Material", 156, 1, 10),
			new PresetInventoryItem("Material", 160, 1, 10),
			new PresetInventoryItem("Material", 164, 1, 10),
			new PresetInventoryItem("Material", 168, 1, 10),
			new PresetInventoryItem("Material", 172, 1, 10),
			new PresetInventoryItem("Material", 176, 1, 10),
			new PresetInventoryItem("Material", 180, 1, 10),
			new PresetInventoryItem("Material", 184, 1, 10),
			new PresetInventoryItem("Material", 188, 1, 10),
			new PresetInventoryItem("Material", 192, 1, 10),
			new PresetInventoryItem("Material", 196, 1, 10),
			new PresetInventoryItem("Material", 200, 1, 10),
			new PresetInventoryItem("Material", 204, 1, 10),
			new PresetInventoryItem("Material", 208, 1, 10),
			new PresetInventoryItem("Material", 212, 1, 10),
			new PresetInventoryItem("Material", 216, 1, 10),
			new PresetInventoryItem("Material", 220, 1, 10),
			new PresetInventoryItem("Material", 224, 1, 10),
			new PresetInventoryItem("Material", 228, 1, 10),
			new PresetInventoryItem("Material", 232, 1, 10),
			new PresetInventoryItem("Material", 236, 1, 10),
			new PresetInventoryItem("Material", 243, 1, 10),
			new PresetInventoryItem("Material", 250, 1, 10),
			new PresetInventoryItem("Material", 257, 1, 10),
			new PresetInventoryItem("Material", 264, 1, 10),
			new PresetInventoryItem("Material", 271, 1, 10)
		}, new List<PresetOrgMemberCombatSkill>
		{
			new PresetOrgMemberCombatSkill(61, 4),
			new PresetOrgMemberCombatSkill(165, 4),
			new PresetOrgMemberCombatSkill(271, 7),
			new PresetOrgMemberCombatSkill(361, 5),
			new PresetOrgMemberCombatSkill(438, 5),
			new PresetOrgMemberCombatSkill(478, 6),
			new PresetOrgMemberCombatSkill(494, 7)
		}, new sbyte[5] { 8, 8, 8, 8, 8 }, new short[8] { -70, -70, -70, -80, -70, -50, -70, -80 }, 100000, 288000, 15, 30, 42300, new short[16]
		{
			-1, -1, -1, -1, -1, 2, -1, -1, 12, 12,
			6, 6, -1, -1, -1, 2
		}, 8, new short[14]
		{
			6, 6, 12, 6, 6, 12, 12, -1, -1, -1,
			-1, -1, -1, -1
		}, new short[6] { -1, 9, -1, 9, 9, 9 }, new List<sbyte> { 5, 18, 24, 25, 64 }, 3, new ResourceInts(default(int), default(int), default(int), default(int), default(int), default(int), default(int), default(int)), new IntPair[18]
		{
			new IntPair(17, 1),
			new IntPair(8, 3),
			new IntPair(5, 1),
			new IntPair(6, 1),
			new IntPair(15, 3),
			new IntPair(16, 3),
			new IntPair(7, 0),
			new IntPair(14, 9),
			new IntPair(12, 1),
			new IntPair(4, 12),
			new IntPair(3, 1),
			new IntPair(13, 54),
			new IntPair(10, 3),
			new IntPair(2, 3),
			new IntPair(1, 9),
			new IntPair(11, 9),
			new IntPair(0, 30),
			new IntPair(9, 1)
		}, null));
		_dataArray.Add(new OrganizationMemberItem(129, 229, 10, 6, 4, 6, 2, restrictPrincipalAmount: false, -1, -1, -1, 3, 6, -1, 6, 0, 0, new int[2] { 606, 607 }, 6100, 12, 8000, 13800, 0, new List<short> { 46, 47, 48 }, new List<short> { 37, 38, 39 }, new int[2] { 608, 609 }, canStroll: false, 300, new short[4] { 34, 40, 46, 52 }, new PresetEquipmentItemWithProb[8]
		{
			new PresetEquipmentItemWithProb("Armor", 54, 75),
			new PresetEquipmentItemWithProb("Armor", 315, 100),
			new PresetEquipmentItemWithProb("Armor", 468, 75),
			new PresetEquipmentItemWithProb("Armor", 171, 75),
			new PresetEquipmentItemWithProb("Accessory", 108, 75),
			new PresetEquipmentItemWithProb("Accessory", 144, 50),
			new PresetEquipmentItemWithProb("Accessory", 126, 50),
			new PresetEquipmentItemWithProb("Carrier", 9, 75)
		}, new PresetEquipmentItem("Clothing", 47), new List<PresetInventoryItem>
		{
			new PresetInventoryItem("SkillBook", 72, 3, 40),
			new PresetInventoryItem("SkillBook", 81, 3, 40),
			new PresetInventoryItem("Material", 42, 1, 30),
			new PresetInventoryItem("Material", 49, 1, 30),
			new PresetInventoryItem("CraftTool", 45, 1, 30),
			new PresetInventoryItem("Food", 93, 3, 40),
			new PresetInventoryItem("Medicine", 0, 1, 30),
			new PresetInventoryItem("Medicine", 9, 1, 30),
			new PresetInventoryItem("Medicine", 18, 1, 30),
			new PresetInventoryItem("Medicine", 27, 1, 30),
			new PresetInventoryItem("Medicine", 36, 1, 30),
			new PresetInventoryItem("Medicine", 45, 1, 30),
			new PresetInventoryItem("Medicine", 60, 1, 20),
			new PresetInventoryItem("Medicine", 72, 1, 20),
			new PresetInventoryItem("Medicine", 88, 1, 20),
			new PresetInventoryItem("Medicine", 100, 1, 20),
			new PresetInventoryItem("Medicine", 136, 1, 20),
			new PresetInventoryItem("Medicine", 148, 1, 20),
			new PresetInventoryItem("Medicine", 160, 1, 20),
			new PresetInventoryItem("Medicine", 172, 1, 20),
			new PresetInventoryItem("Medicine", 184, 1, 20),
			new PresetInventoryItem("Medicine", 196, 1, 20),
			new PresetInventoryItem("Medicine", 232, 1, 20),
			new PresetInventoryItem("Medicine", 244, 1, 20),
			new PresetInventoryItem("Medicine", 256, 1, 20),
			new PresetInventoryItem("Medicine", 280, 1, 20),
			new PresetInventoryItem("Medicine", 304, 1, 20),
			new PresetInventoryItem("Medicine", 328, 1, 20),
			new PresetInventoryItem("TeaWine", 18, 1, 20),
			new PresetInventoryItem("TeaWine", 27, 1, 20),
			new PresetInventoryItem("Material", 140, 1, 10),
			new PresetInventoryItem("Material", 144, 1, 10),
			new PresetInventoryItem("Material", 148, 1, 10),
			new PresetInventoryItem("Material", 152, 1, 10),
			new PresetInventoryItem("Material", 156, 1, 10),
			new PresetInventoryItem("Material", 160, 1, 10),
			new PresetInventoryItem("Material", 164, 1, 10),
			new PresetInventoryItem("Material", 168, 1, 10),
			new PresetInventoryItem("Material", 172, 1, 10),
			new PresetInventoryItem("Material", 176, 1, 10),
			new PresetInventoryItem("Material", 180, 1, 10),
			new PresetInventoryItem("Material", 184, 1, 10),
			new PresetInventoryItem("Material", 188, 1, 10),
			new PresetInventoryItem("Material", 192, 1, 10),
			new PresetInventoryItem("Material", 196, 1, 10),
			new PresetInventoryItem("Material", 200, 1, 10),
			new PresetInventoryItem("Material", 204, 1, 10),
			new PresetInventoryItem("Material", 208, 1, 10),
			new PresetInventoryItem("Material", 212, 1, 10),
			new PresetInventoryItem("Material", 216, 1, 10),
			new PresetInventoryItem("Material", 220, 1, 10),
			new PresetInventoryItem("Material", 224, 1, 10),
			new PresetInventoryItem("Material", 228, 1, 10),
			new PresetInventoryItem("Material", 232, 1, 10),
			new PresetInventoryItem("Material", 236, 1, 10),
			new PresetInventoryItem("Material", 243, 1, 10),
			new PresetInventoryItem("Material", 250, 1, 10),
			new PresetInventoryItem("Material", 257, 1, 10),
			new PresetInventoryItem("Material", 264, 1, 10),
			new PresetInventoryItem("Material", 271, 1, 10)
		}, new List<PresetOrgMemberCombatSkill>
		{
			new PresetOrgMemberCombatSkill(61, 4),
			new PresetOrgMemberCombatSkill(165, 3),
			new PresetOrgMemberCombatSkill(271, 6),
			new PresetOrgMemberCombatSkill(361, 5),
			new PresetOrgMemberCombatSkill(438, 5),
			new PresetOrgMemberCombatSkill(478, 6),
			new PresetOrgMemberCombatSkill(494, 6)
		}, new sbyte[5] { 8, 8, 8, 8, 8 }, new short[8] { -70, -70, -70, -80, -70, -50, -70, -80 }, 60000, 144000, 15, 40, 27600, new short[16]
		{
			-1, -1, -1, -1, -1, 2, -1, -1, 12, 12,
			6, 6, -1, -1, -1, 2
		}, 7, new short[14]
		{
			6, 6, 12, 6, 6, 12, 12, -1, -1, -1,
			-1, -1, -1, -1
		}, new short[6] { -1, 9, -1, 9, 9, 9 }, new List<sbyte> { 5, 18, 24, 25, 64 }, 3, new ResourceInts(default(int), default(int), default(int), default(int), default(int), default(int), default(int), default(int)), new IntPair[18]
		{
			new IntPair(17, 1),
			new IntPair(8, 3),
			new IntPair(5, 1),
			new IntPair(6, 1),
			new IntPair(15, 3),
			new IntPair(16, 3),
			new IntPair(7, 0),
			new IntPair(14, 9),
			new IntPair(12, 1),
			new IntPair(4, 12),
			new IntPair(3, 1),
			new IntPair(13, 54),
			new IntPair(10, 3),
			new IntPair(2, 3),
			new IntPair(1, 9),
			new IntPair(11, 9),
			new IntPair(0, 30),
			new IntPair(9, 1)
		}, null));
		_dataArray.Add(new OrganizationMemberItem(130, 610, 10, 5, 4, 6, 2, restrictPrincipalAmount: false, -1, -1, -1, 2, 5, 8, 5, 0, 0, new int[2] { 611, 612 }, 4200, 10, 6000, 8400, 0, new List<short> { 46, 47, 48 }, new List<short> { 37, 38, 39 }, new int[2] { 613, 614 }, canStroll: true, 301, new short[4] { 30, 35, 40, 45 }, new PresetEquipmentItemWithProb[8]
		{
			new PresetEquipmentItemWithProb("Armor", 54, 75),
			new PresetEquipmentItemWithProb("Armor", 315, 100),
			new PresetEquipmentItemWithProb("Armor", 468, 75),
			new PresetEquipmentItemWithProb("Armor", 171, 75),
			new PresetEquipmentItemWithProb("Accessory", 108, 75),
			new PresetEquipmentItemWithProb("Accessory", 144, 50),
			new PresetEquipmentItemWithProb("Accessory", 126, 50),
			new PresetEquipmentItemWithProb("Carrier", 9, 75)
		}, new PresetEquipmentItem("Clothing", 47), new List<PresetInventoryItem>
		{
			new PresetInventoryItem("SkillBook", 72, 3, 30),
			new PresetInventoryItem("SkillBook", 81, 3, 30),
			new PresetInventoryItem("Material", 42, 1, 20),
			new PresetInventoryItem("Material", 49, 1, 20),
			new PresetInventoryItem("CraftTool", 45, 1, 30),
			new PresetInventoryItem("Food", 93, 3, 30),
			new PresetInventoryItem("Medicine", 0, 1, 20),
			new PresetInventoryItem("Medicine", 9, 1, 20),
			new PresetInventoryItem("Medicine", 18, 1, 20),
			new PresetInventoryItem("Medicine", 27, 1, 20),
			new PresetInventoryItem("Medicine", 36, 1, 20),
			new PresetInventoryItem("Medicine", 45, 1, 20),
			new PresetInventoryItem("Medicine", 60, 1, 20),
			new PresetInventoryItem("Medicine", 72, 1, 20),
			new PresetInventoryItem("Medicine", 88, 1, 20),
			new PresetInventoryItem("Medicine", 100, 1, 20),
			new PresetInventoryItem("Medicine", 130, 1, 30),
			new PresetInventoryItem("Medicine", 142, 1, 30),
			new PresetInventoryItem("Medicine", 154, 1, 30),
			new PresetInventoryItem("Medicine", 166, 1, 30),
			new PresetInventoryItem("Medicine", 178, 1, 30),
			new PresetInventoryItem("Medicine", 190, 1, 30),
			new PresetInventoryItem("Medicine", 232, 1, 20),
			new PresetInventoryItem("Medicine", 244, 1, 20),
			new PresetInventoryItem("Medicine", 256, 1, 20),
			new PresetInventoryItem("Medicine", 280, 1, 20),
			new PresetInventoryItem("Medicine", 304, 1, 20),
			new PresetInventoryItem("Medicine", 328, 1, 20),
			new PresetInventoryItem("TeaWine", 18, 1, 20),
			new PresetInventoryItem("TeaWine", 27, 1, 20),
			new PresetInventoryItem("Material", 140, 1, 10),
			new PresetInventoryItem("Material", 144, 1, 10),
			new PresetInventoryItem("Material", 148, 1, 10),
			new PresetInventoryItem("Material", 152, 1, 10),
			new PresetInventoryItem("Material", 156, 1, 10),
			new PresetInventoryItem("Material", 160, 1, 10),
			new PresetInventoryItem("Material", 164, 1, 10),
			new PresetInventoryItem("Material", 168, 1, 10),
			new PresetInventoryItem("Material", 172, 1, 10),
			new PresetInventoryItem("Material", 176, 1, 10),
			new PresetInventoryItem("Material", 180, 1, 10),
			new PresetInventoryItem("Material", 184, 1, 10),
			new PresetInventoryItem("Material", 188, 1, 10),
			new PresetInventoryItem("Material", 192, 1, 10),
			new PresetInventoryItem("Material", 196, 1, 10),
			new PresetInventoryItem("Material", 200, 1, 10),
			new PresetInventoryItem("Material", 204, 1, 10),
			new PresetInventoryItem("Material", 208, 1, 10),
			new PresetInventoryItem("Material", 212, 1, 10),
			new PresetInventoryItem("Material", 216, 1, 10),
			new PresetInventoryItem("Material", 220, 1, 10),
			new PresetInventoryItem("Material", 224, 1, 10),
			new PresetInventoryItem("Material", 228, 1, 10),
			new PresetInventoryItem("Material", 232, 1, 10),
			new PresetInventoryItem("Material", 236, 1, 10),
			new PresetInventoryItem("Material", 243, 1, 10),
			new PresetInventoryItem("Material", 250, 1, 10),
			new PresetInventoryItem("Material", 257, 1, 10),
			new PresetInventoryItem("Material", 264, 1, 10),
			new PresetInventoryItem("Material", 271, 1, 10)
		}, new List<PresetOrgMemberCombatSkill>
		{
			new PresetOrgMemberCombatSkill(61, 3),
			new PresetOrgMemberCombatSkill(165, 3),
			new PresetOrgMemberCombatSkill(271, 5),
			new PresetOrgMemberCombatSkill(478, 5),
			new PresetOrgMemberCombatSkill(494, 5)
		}, new sbyte[5] { 6, 6, 6, 6, 6 }, new short[8] { -70, -70, -70, -80, -70, -50, -70, -80 }, 40000, 48000, 10, 50, 16800, new short[16]
		{
			-1, -1, -1, -1, -1, 2, -1, -1, 12, 12,
			6, 6, -1, -1, -1, 2
		}, 6, new short[14]
		{
			6, 6, 12, 6, 6, 12, 12, -1, -1, -1,
			-1, -1, -1, -1
		}, new short[6] { -1, 9, -1, 9, 9, 9 }, new List<sbyte> { 5, 64 }, 3, new ResourceInts(default(int), default(int), default(int), default(int), default(int), default(int), default(int), default(int)), new IntPair[18]
		{
			new IntPair(17, 1),
			new IntPair(8, 3),
			new IntPair(5, 1),
			new IntPair(6, 1),
			new IntPair(15, 3),
			new IntPair(16, 3),
			new IntPair(7, 0),
			new IntPair(14, 6),
			new IntPair(12, 1),
			new IntPair(4, 9),
			new IntPair(3, 1),
			new IntPair(13, 42),
			new IntPair(10, 3),
			new IntPair(2, 3),
			new IntPair(1, 9),
			new IntPair(11, 9),
			new IntPair(0, 30),
			new IntPair(9, 1)
		}, null));
		_dataArray.Add(new OrganizationMemberItem(131, 615, 10, 4, 4, 6, 2, restrictPrincipalAmount: false, -1, -1, -1, 1, 4, 7, 4, 0, 0, new int[2] { 616, 617 }, 2800, 8, 4500, 4650, 0, new List<short> { 46, 47, 48 }, new List<short> { 37, 38, 39 }, new int[2] { 618, 619 }, canStroll: true, 302, new short[4] { 26, 30, 34, 38 }, new PresetEquipmentItemWithProb[8]
		{
			new PresetEquipmentItemWithProb("Armor", 54, 75),
			new PresetEquipmentItemWithProb("Armor", 315, 100),
			new PresetEquipmentItemWithProb("Armor", 468, 75),
			new PresetEquipmentItemWithProb("Armor", 171, 75),
			new PresetEquipmentItemWithProb("Accessory", 108, 75),
			new PresetEquipmentItemWithProb("Accessory", 144, 50),
			new PresetEquipmentItemWithProb("Accessory", 126, 50),
			new PresetEquipmentItemWithProb("Carrier", 9, 75)
		}, new PresetEquipmentItem("Clothing", 47), new List<PresetInventoryItem>
		{
			new PresetInventoryItem("SkillBook", 72, 3, 30),
			new PresetInventoryItem("SkillBook", 81, 3, 30),
			new PresetInventoryItem("Material", 42, 1, 20),
			new PresetInventoryItem("Material", 49, 1, 20),
			new PresetInventoryItem("CraftTool", 45, 1, 30),
			new PresetInventoryItem("Food", 93, 3, 30),
			new PresetInventoryItem("Medicine", 0, 1, 20),
			new PresetInventoryItem("Medicine", 9, 1, 20),
			new PresetInventoryItem("Medicine", 18, 1, 20),
			new PresetInventoryItem("Medicine", 27, 1, 20),
			new PresetInventoryItem("Medicine", 36, 1, 20),
			new PresetInventoryItem("Medicine", 45, 1, 20),
			new PresetInventoryItem("Medicine", 60, 1, 20),
			new PresetInventoryItem("Medicine", 72, 1, 20),
			new PresetInventoryItem("Medicine", 88, 1, 20),
			new PresetInventoryItem("Medicine", 100, 1, 20),
			new PresetInventoryItem("Medicine", 130, 1, 30),
			new PresetInventoryItem("Medicine", 142, 1, 30),
			new PresetInventoryItem("Medicine", 154, 1, 30),
			new PresetInventoryItem("Medicine", 166, 1, 30),
			new PresetInventoryItem("Medicine", 178, 1, 30),
			new PresetInventoryItem("Medicine", 190, 1, 30),
			new PresetInventoryItem("Medicine", 232, 1, 20),
			new PresetInventoryItem("Medicine", 244, 1, 20),
			new PresetInventoryItem("Medicine", 256, 1, 20),
			new PresetInventoryItem("Medicine", 280, 1, 20),
			new PresetInventoryItem("Medicine", 304, 1, 20),
			new PresetInventoryItem("Medicine", 328, 1, 20),
			new PresetInventoryItem("TeaWine", 18, 1, 20),
			new PresetInventoryItem("TeaWine", 27, 1, 20),
			new PresetInventoryItem("Material", 140, 1, 10),
			new PresetInventoryItem("Material", 144, 1, 10),
			new PresetInventoryItem("Material", 148, 1, 10),
			new PresetInventoryItem("Material", 152, 1, 10),
			new PresetInventoryItem("Material", 156, 1, 10),
			new PresetInventoryItem("Material", 160, 1, 10),
			new PresetInventoryItem("Material", 164, 1, 10),
			new PresetInventoryItem("Material", 168, 1, 10),
			new PresetInventoryItem("Material", 172, 1, 10),
			new PresetInventoryItem("Material", 176, 1, 10),
			new PresetInventoryItem("Material", 180, 1, 10),
			new PresetInventoryItem("Material", 184, 1, 10),
			new PresetInventoryItem("Material", 188, 1, 10),
			new PresetInventoryItem("Material", 192, 1, 10),
			new PresetInventoryItem("Material", 196, 1, 10),
			new PresetInventoryItem("Material", 200, 1, 10),
			new PresetInventoryItem("Material", 204, 1, 10),
			new PresetInventoryItem("Material", 208, 1, 10),
			new PresetInventoryItem("Material", 212, 1, 10),
			new PresetInventoryItem("Material", 216, 1, 10),
			new PresetInventoryItem("Material", 220, 1, 10),
			new PresetInventoryItem("Material", 224, 1, 10),
			new PresetInventoryItem("Material", 228, 1, 10),
			new PresetInventoryItem("Material", 232, 1, 10),
			new PresetInventoryItem("Material", 236, 1, 10),
			new PresetInventoryItem("Material", 243, 1, 10),
			new PresetInventoryItem("Material", 250, 1, 10),
			new PresetInventoryItem("Material", 257, 1, 10),
			new PresetInventoryItem("Material", 264, 1, 10),
			new PresetInventoryItem("Material", 271, 1, 10)
		}, new List<PresetOrgMemberCombatSkill>
		{
			new PresetOrgMemberCombatSkill(61, 3),
			new PresetOrgMemberCombatSkill(165, 3),
			new PresetOrgMemberCombatSkill(271, 4),
			new PresetOrgMemberCombatSkill(361, 4),
			new PresetOrgMemberCombatSkill(438, 4),
			new PresetOrgMemberCombatSkill(494, 4)
		}, new sbyte[5] { 4, 4, 4, 4, 4 }, new short[8] { -70, -70, -70, -80, -70, -50, -70, -80 }, 30000, 24000, 10, 60, 9300, new short[16]
		{
			-1, -1, -1, -1, -1, 2, -1, -1, 12, 12,
			6, 6, -1, -1, -1, 2
		}, 5, new short[14]
		{
			6, 6, 12, 6, 6, 12, 12, -1, -1, -1,
			-1, -1, -1, -1
		}, new short[6] { -1, 9, -1, 9, 9, 9 }, new List<sbyte> { 5, 64 }, 3, new ResourceInts(default(int), default(int), default(int), default(int), default(int), default(int), default(int), default(int)), new IntPair[18]
		{
			new IntPair(17, 1),
			new IntPair(8, 3),
			new IntPair(5, 1),
			new IntPair(6, 1),
			new IntPair(15, 3),
			new IntPair(16, 3),
			new IntPair(7, 0),
			new IntPair(14, 6),
			new IntPair(12, 1),
			new IntPair(4, 9),
			new IntPair(3, 1),
			new IntPair(13, 42),
			new IntPair(10, 3),
			new IntPair(2, 3),
			new IntPair(1, 9),
			new IntPair(11, 9),
			new IntPair(0, 30),
			new IntPair(9, 1)
		}, null));
		_dataArray.Add(new OrganizationMemberItem(132, 620, 10, 3, 6, 9, 3, restrictPrincipalAmount: false, -1, -1, -1, 1, 3, 6, 3, 0, 0, new int[2] { 621, 622 }, 1800, 6, 3000, 2250, 0, new List<short> { 46, 47, 48 }, new List<short> { 37, 38, 39 }, new int[2] { 623, 624 }, canStroll: true, 303, new short[4] { 22, 25, 28, 31 }, new PresetEquipmentItemWithProb[8]
		{
			new PresetEquipmentItemWithProb("Armor", 54, 75),
			new PresetEquipmentItemWithProb("Armor", 315, 100),
			new PresetEquipmentItemWithProb("Armor", 468, 75),
			new PresetEquipmentItemWithProb("Armor", 171, 75),
			new PresetEquipmentItemWithProb("Accessory", 108, 75),
			new PresetEquipmentItemWithProb("Accessory", 144, 50),
			new PresetEquipmentItemWithProb("Accessory", 126, 50),
			new PresetEquipmentItemWithProb("Carrier", 9, 75)
		}, new PresetEquipmentItem("Clothing", 46), new List<PresetInventoryItem>
		{
			new PresetInventoryItem("SkillBook", 72, 3, 30),
			new PresetInventoryItem("SkillBook", 81, 3, 30),
			new PresetInventoryItem("Material", 42, 1, 20),
			new PresetInventoryItem("Material", 49, 1, 20),
			new PresetInventoryItem("CraftTool", 45, 1, 30),
			new PresetInventoryItem("Food", 93, 3, 30),
			new PresetInventoryItem("Medicine", 0, 1, 20),
			new PresetInventoryItem("Medicine", 9, 1, 20),
			new PresetInventoryItem("Medicine", 18, 1, 20),
			new PresetInventoryItem("Medicine", 27, 1, 20),
			new PresetInventoryItem("Medicine", 36, 1, 20),
			new PresetInventoryItem("Medicine", 45, 1, 20),
			new PresetInventoryItem("Medicine", 60, 1, 20),
			new PresetInventoryItem("Medicine", 72, 1, 20),
			new PresetInventoryItem("Medicine", 88, 1, 20),
			new PresetInventoryItem("Medicine", 100, 1, 20),
			new PresetInventoryItem("Medicine", 130, 1, 30),
			new PresetInventoryItem("Medicine", 142, 1, 30),
			new PresetInventoryItem("Medicine", 154, 1, 30),
			new PresetInventoryItem("Medicine", 166, 1, 30),
			new PresetInventoryItem("Medicine", 178, 1, 30),
			new PresetInventoryItem("Medicine", 190, 1, 30),
			new PresetInventoryItem("Medicine", 232, 1, 20),
			new PresetInventoryItem("Medicine", 244, 1, 20),
			new PresetInventoryItem("Medicine", 256, 1, 20),
			new PresetInventoryItem("Medicine", 280, 1, 20),
			new PresetInventoryItem("Medicine", 304, 1, 20),
			new PresetInventoryItem("Medicine", 328, 1, 20),
			new PresetInventoryItem("TeaWine", 18, 1, 20),
			new PresetInventoryItem("TeaWine", 27, 1, 20),
			new PresetInventoryItem("Material", 140, 1, 10),
			new PresetInventoryItem("Material", 144, 1, 10),
			new PresetInventoryItem("Material", 148, 1, 10),
			new PresetInventoryItem("Material", 152, 1, 10),
			new PresetInventoryItem("Material", 156, 1, 10),
			new PresetInventoryItem("Material", 160, 1, 10),
			new PresetInventoryItem("Material", 164, 1, 10),
			new PresetInventoryItem("Material", 168, 1, 10),
			new PresetInventoryItem("Material", 172, 1, 10),
			new PresetInventoryItem("Material", 176, 1, 10),
			new PresetInventoryItem("Material", 180, 1, 10),
			new PresetInventoryItem("Material", 184, 1, 10),
			new PresetInventoryItem("Material", 188, 1, 10),
			new PresetInventoryItem("Material", 192, 1, 10),
			new PresetInventoryItem("Material", 196, 1, 10),
			new PresetInventoryItem("Material", 200, 1, 10),
			new PresetInventoryItem("Material", 204, 1, 10),
			new PresetInventoryItem("Material", 208, 1, 10),
			new PresetInventoryItem("Material", 212, 1, 10),
			new PresetInventoryItem("Material", 216, 1, 10),
			new PresetInventoryItem("Material", 220, 1, 10),
			new PresetInventoryItem("Material", 224, 1, 10),
			new PresetInventoryItem("Material", 228, 1, 10),
			new PresetInventoryItem("Material", 232, 1, 10),
			new PresetInventoryItem("Material", 236, 1, 10),
			new PresetInventoryItem("Material", 243, 1, 10),
			new PresetInventoryItem("Material", 250, 1, 10),
			new PresetInventoryItem("Material", 257, 1, 10),
			new PresetInventoryItem("Material", 264, 1, 10),
			new PresetInventoryItem("Material", 271, 1, 10)
		}, new List<PresetOrgMemberCombatSkill>
		{
			new PresetOrgMemberCombatSkill(61, 2),
			new PresetOrgMemberCombatSkill(165, 2),
			new PresetOrgMemberCombatSkill(271, 3),
			new PresetOrgMemberCombatSkill(494, 3)
		}, new sbyte[5] { 4, 4, 4, 4, 4 }, new short[8] { -70, -70, -70, -80, -70, -50, -70, -80 }, 20000, 12000, 10, 70, 4500, new short[16]
		{
			-1, -1, -1, -1, -1, 2, -1, -1, 12, 12,
			6, 6, -1, -1, -1, 2
		}, 4, new short[14]
		{
			6, 6, 12, 6, 6, 12, 12, -1, -1, -1,
			-1, -1, -1, -1
		}, new short[6] { -1, 9, -1, 9, 9, 9 }, new List<sbyte> { 5, 64 }, 3, new ResourceInts(default(int), default(int), default(int), default(int), default(int), default(int), default(int), default(int)), new IntPair[18]
		{
			new IntPair(17, 1),
			new IntPair(8, 3),
			new IntPair(5, 1),
			new IntPair(6, 1),
			new IntPair(15, 3),
			new IntPair(16, 3),
			new IntPair(7, 0),
			new IntPair(14, 6),
			new IntPair(12, 1),
			new IntPair(4, 9),
			new IntPair(3, 1),
			new IntPair(13, 42),
			new IntPair(10, 3),
			new IntPair(2, 3),
			new IntPair(1, 9),
			new IntPair(11, 9),
			new IntPair(0, 30),
			new IntPair(9, 1)
		}, null));
		_dataArray.Add(new OrganizationMemberItem(133, 625, 10, 2, 6, 9, 3, restrictPrincipalAmount: false, -1, -1, -1, 1, 2, 5, 2, 0, 0, new int[2] { 626, 627 }, 600, 4, 2000, 900, 0, new List<short> { 46, 47, 48 }, new List<short> { 37, 38, 39 }, new int[2] { 628, 629 }, canStroll: true, 304, new short[4] { 18, 20, 22, 24 }, new PresetEquipmentItemWithProb[8]
		{
			new PresetEquipmentItemWithProb("Armor", 54, 75),
			new PresetEquipmentItemWithProb("Armor", 315, 100),
			new PresetEquipmentItemWithProb("Armor", 468, 75),
			new PresetEquipmentItemWithProb("Armor", 171, 75),
			new PresetEquipmentItemWithProb("Accessory", 108, 75),
			new PresetEquipmentItemWithProb("Accessory", 144, 50),
			new PresetEquipmentItemWithProb("Accessory", 126, 50),
			new PresetEquipmentItemWithProb("Carrier", 9, 75)
		}, new PresetEquipmentItem("Clothing", 46), new List<PresetInventoryItem>
		{
			new PresetInventoryItem("SkillBook", 72, 3, 20),
			new PresetInventoryItem("SkillBook", 81, 3, 20),
			new PresetInventoryItem("Material", 42, 1, 10),
			new PresetInventoryItem("Material", 49, 1, 10),
			new PresetInventoryItem("CraftTool", 45, 1, 20),
			new PresetInventoryItem("Food", 93, 3, 20),
			new PresetInventoryItem("Medicine", 0, 1, 10),
			new PresetInventoryItem("Medicine", 9, 1, 10),
			new PresetInventoryItem("Medicine", 18, 1, 10),
			new PresetInventoryItem("Medicine", 27, 1, 10),
			new PresetInventoryItem("Medicine", 36, 1, 10),
			new PresetInventoryItem("Medicine", 45, 1, 10),
			new PresetInventoryItem("Medicine", 54, 1, 20),
			new PresetInventoryItem("Medicine", 66, 1, 20),
			new PresetInventoryItem("Medicine", 82, 1, 20),
			new PresetInventoryItem("Medicine", 94, 1, 20),
			new PresetInventoryItem("Medicine", 130, 1, 20),
			new PresetInventoryItem("Medicine", 142, 1, 20),
			new PresetInventoryItem("Medicine", 154, 1, 20),
			new PresetInventoryItem("Medicine", 166, 1, 20),
			new PresetInventoryItem("Medicine", 178, 1, 20),
			new PresetInventoryItem("Medicine", 190, 1, 20),
			new PresetInventoryItem("Medicine", 226, 1, 10),
			new PresetInventoryItem("Medicine", 238, 1, 10),
			new PresetInventoryItem("Medicine", 250, 1, 10),
			new PresetInventoryItem("Medicine", 274, 1, 10),
			new PresetInventoryItem("Medicine", 298, 1, 10),
			new PresetInventoryItem("Medicine", 322, 1, 10),
			new PresetInventoryItem("Material", 140, 1, 10),
			new PresetInventoryItem("Material", 144, 1, 10),
			new PresetInventoryItem("Material", 148, 1, 10),
			new PresetInventoryItem("Material", 152, 1, 10),
			new PresetInventoryItem("Material", 156, 1, 10),
			new PresetInventoryItem("Material", 160, 1, 10),
			new PresetInventoryItem("Material", 164, 1, 10),
			new PresetInventoryItem("Material", 168, 1, 10),
			new PresetInventoryItem("Material", 172, 1, 10),
			new PresetInventoryItem("Material", 176, 1, 10),
			new PresetInventoryItem("Material", 180, 1, 10),
			new PresetInventoryItem("Material", 184, 1, 10),
			new PresetInventoryItem("Material", 188, 1, 10),
			new PresetInventoryItem("Material", 192, 1, 10),
			new PresetInventoryItem("Material", 196, 1, 10),
			new PresetInventoryItem("Material", 200, 1, 10),
			new PresetInventoryItem("Material", 204, 1, 10),
			new PresetInventoryItem("Material", 208, 1, 10),
			new PresetInventoryItem("Material", 212, 1, 10),
			new PresetInventoryItem("Material", 216, 1, 10),
			new PresetInventoryItem("Material", 220, 1, 10),
			new PresetInventoryItem("Material", 224, 1, 10),
			new PresetInventoryItem("Material", 228, 1, 10),
			new PresetInventoryItem("Material", 232, 1, 10),
			new PresetInventoryItem("Material", 236, 1, 10),
			new PresetInventoryItem("Material", 243, 1, 10),
			new PresetInventoryItem("Material", 250, 1, 10),
			new PresetInventoryItem("Material", 257, 1, 10),
			new PresetInventoryItem("Material", 264, 1, 10),
			new PresetInventoryItem("Material", 271, 1, 10)
		}, new List<PresetOrgMemberCombatSkill>
		{
			new PresetOrgMemberCombatSkill(61, 2),
			new PresetOrgMemberCombatSkill(165, 2),
			new PresetOrgMemberCombatSkill(271, 2),
			new PresetOrgMemberCombatSkill(478, 2),
			new PresetOrgMemberCombatSkill(494, 2)
		}, new sbyte[5] { 2, 2, 2, 2, 2 }, new short[8] { -70, -70, -70, -80, -70, -50, -70, -80 }, 15000, 4000, 5, 80, 1800, new short[16]
		{
			-1, -1, -1, -1, -1, 2, -1, -1, 12, 12,
			6, 6, -1, -1, -1, 2
		}, 3, new short[14]
		{
			6, 6, 12, 6, 6, 12, 12, -1, -1, -1,
			-1, -1, -1, -1
		}, new short[6] { -1, 9, -1, 9, 9, 9 }, new List<sbyte> { 5, 64 }, 3, new ResourceInts(default(int), default(int), default(int), default(int), default(int), default(int), default(int), default(int)), new IntPair[18]
		{
			new IntPair(17, 1),
			new IntPair(8, 3),
			new IntPair(5, 1),
			new IntPair(6, 1),
			new IntPair(15, 3),
			new IntPair(16, 3),
			new IntPair(7, 0),
			new IntPair(14, 3),
			new IntPair(12, 1),
			new IntPair(4, 6),
			new IntPair(3, 1),
			new IntPair(13, 30),
			new IntPair(10, 3),
			new IntPair(2, 3),
			new IntPair(1, 9),
			new IntPair(11, 9),
			new IntPair(0, 30),
			new IntPair(9, 1)
		}, null));
		_dataArray.Add(new OrganizationMemberItem(134, 630, 10, 1, 6, 9, 3, restrictPrincipalAmount: false, -1, -1, -1, 0, 1, 4, 1, 0, 0, new int[2] { 631, 632 }, 300, 2, 1000, 300, 0, new List<short> { 46, 47, 48 }, new List<short> { 37, 38, 39 }, new int[2] { 633, 634 }, canStroll: true, 305, new short[4] { 14, 15, 16, 17 }, new PresetEquipmentItemWithProb[8]
		{
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Armor", 315, 100),
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Armor", 171, 75),
			new PresetEquipmentItemWithProb("Accessory", 108, 75),
			new PresetEquipmentItemWithProb("Accessory", 144, 50),
			new PresetEquipmentItemWithProb("Accessory", -1, 0),
			new PresetEquipmentItemWithProb("Carrier", -1, 0)
		}, new PresetEquipmentItem("Clothing", 46), new List<PresetInventoryItem>
		{
			new PresetInventoryItem("SkillBook", 72, 3, 20),
			new PresetInventoryItem("SkillBook", 81, 3, 20),
			new PresetInventoryItem("Material", 42, 1, 10),
			new PresetInventoryItem("Material", 49, 1, 10),
			new PresetInventoryItem("CraftTool", 45, 1, 20),
			new PresetInventoryItem("Food", 93, 3, 20),
			new PresetInventoryItem("Medicine", 0, 1, 10),
			new PresetInventoryItem("Medicine", 9, 1, 10),
			new PresetInventoryItem("Medicine", 18, 1, 10),
			new PresetInventoryItem("Medicine", 27, 1, 10),
			new PresetInventoryItem("Medicine", 36, 1, 10),
			new PresetInventoryItem("Medicine", 45, 1, 10),
			new PresetInventoryItem("Medicine", 54, 1, 20),
			new PresetInventoryItem("Medicine", 66, 1, 20),
			new PresetInventoryItem("Medicine", 82, 1, 20),
			new PresetInventoryItem("Medicine", 94, 1, 20),
			new PresetInventoryItem("Medicine", 130, 1, 20),
			new PresetInventoryItem("Medicine", 142, 1, 20),
			new PresetInventoryItem("Medicine", 154, 1, 20),
			new PresetInventoryItem("Medicine", 166, 1, 20),
			new PresetInventoryItem("Medicine", 178, 1, 20),
			new PresetInventoryItem("Medicine", 190, 1, 20),
			new PresetInventoryItem("Medicine", 226, 1, 10),
			new PresetInventoryItem("Medicine", 238, 1, 10),
			new PresetInventoryItem("Medicine", 250, 1, 10),
			new PresetInventoryItem("Medicine", 274, 1, 10),
			new PresetInventoryItem("Medicine", 298, 1, 10),
			new PresetInventoryItem("Medicine", 322, 1, 10),
			new PresetInventoryItem("Material", 140, 1, 10),
			new PresetInventoryItem("Material", 144, 1, 10),
			new PresetInventoryItem("Material", 148, 1, 10),
			new PresetInventoryItem("Material", 152, 1, 10),
			new PresetInventoryItem("Material", 156, 1, 10),
			new PresetInventoryItem("Material", 160, 1, 10),
			new PresetInventoryItem("Material", 164, 1, 10),
			new PresetInventoryItem("Material", 168, 1, 10),
			new PresetInventoryItem("Material", 172, 1, 10),
			new PresetInventoryItem("Material", 176, 1, 10),
			new PresetInventoryItem("Material", 180, 1, 10),
			new PresetInventoryItem("Material", 184, 1, 10),
			new PresetInventoryItem("Material", 188, 1, 10),
			new PresetInventoryItem("Material", 192, 1, 10),
			new PresetInventoryItem("Material", 196, 1, 10),
			new PresetInventoryItem("Material", 200, 1, 10),
			new PresetInventoryItem("Material", 204, 1, 10),
			new PresetInventoryItem("Material", 208, 1, 10),
			new PresetInventoryItem("Material", 212, 1, 10),
			new PresetInventoryItem("Material", 216, 1, 10),
			new PresetInventoryItem("Material", 220, 1, 10),
			new PresetInventoryItem("Material", 224, 1, 10),
			new PresetInventoryItem("Material", 228, 1, 10),
			new PresetInventoryItem("Material", 232, 1, 10),
			new PresetInventoryItem("Material", 236, 1, 10),
			new PresetInventoryItem("Material", 243, 1, 10),
			new PresetInventoryItem("Material", 250, 1, 10),
			new PresetInventoryItem("Material", 257, 1, 10),
			new PresetInventoryItem("Material", 264, 1, 10),
			new PresetInventoryItem("Material", 271, 1, 10)
		}, new List<PresetOrgMemberCombatSkill>
		{
			new PresetOrgMemberCombatSkill(61, 1),
			new PresetOrgMemberCombatSkill(165, 1),
			new PresetOrgMemberCombatSkill(271, 1),
			new PresetOrgMemberCombatSkill(361, 1),
			new PresetOrgMemberCombatSkill(438, 1),
			new PresetOrgMemberCombatSkill(494, 1)
		}, new sbyte[5], new short[8] { -70, -70, -70, -80, -70, -50, -70, -80 }, 10000, 2000, 5, 90, 600, new short[16]
		{
			-1, -1, -1, -1, -1, 2, -1, -1, 12, 12,
			6, 6, -1, -1, -1, 2
		}, 2, new short[14]
		{
			6, 6, 12, 6, 6, 12, 12, -1, -1, -1,
			-1, -1, -1, -1
		}, new short[6] { -1, 9, -1, 9, 9, 9 }, new List<sbyte> { 5, 64 }, 3, new ResourceInts(default(int), default(int), default(int), default(int), default(int), default(int), default(int), default(int)), new IntPair[18]
		{
			new IntPair(17, 1),
			new IntPair(8, 3),
			new IntPair(5, 1),
			new IntPair(6, 1),
			new IntPair(15, 3),
			new IntPair(16, 3),
			new IntPair(7, 0),
			new IntPair(14, 3),
			new IntPair(12, 1),
			new IntPair(4, 6),
			new IntPair(3, 1),
			new IntPair(13, 30),
			new IntPair(10, 3),
			new IntPair(2, 3),
			new IntPair(1, 9),
			new IntPair(11, 9),
			new IntPair(0, 30),
			new IntPair(9, 1)
		}, null));
		_dataArray.Add(new OrganizationMemberItem(135, 635, 10, 0, 6, 9, 3, restrictPrincipalAmount: false, -1, -1, -1, 0, 0, 4, 0, 0, 0, new int[2] { 636, 637 }, 150, 0, 500, 150, 0, new List<short> { 46, 47, 48 }, new List<short> { 37, 38, 39 }, new int[2] { 638, 639 }, canStroll: true, 306, new short[4] { 10, 10, 10, 10 }, new PresetEquipmentItemWithProb[8]
		{
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Armor", 315, 100),
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Armor", 171, 75),
			new PresetEquipmentItemWithProb("Accessory", 108, 75),
			new PresetEquipmentItemWithProb("Accessory", -1, 0),
			new PresetEquipmentItemWithProb("Accessory", -1, 0),
			new PresetEquipmentItemWithProb("Carrier", -1, 0)
		}, new PresetEquipmentItem("Clothing", 46), new List<PresetInventoryItem>
		{
			new PresetInventoryItem("SkillBook", 72, 3, 20),
			new PresetInventoryItem("SkillBook", 81, 3, 20),
			new PresetInventoryItem("Material", 42, 1, 10),
			new PresetInventoryItem("Material", 49, 1, 10),
			new PresetInventoryItem("CraftTool", 45, 1, 20),
			new PresetInventoryItem("Food", 93, 3, 20),
			new PresetInventoryItem("Medicine", 0, 1, 10),
			new PresetInventoryItem("Medicine", 9, 1, 10),
			new PresetInventoryItem("Medicine", 18, 1, 10),
			new PresetInventoryItem("Medicine", 27, 1, 10),
			new PresetInventoryItem("Medicine", 36, 1, 10),
			new PresetInventoryItem("Medicine", 45, 1, 10),
			new PresetInventoryItem("Medicine", 54, 1, 20),
			new PresetInventoryItem("Medicine", 66, 1, 20),
			new PresetInventoryItem("Medicine", 82, 1, 20),
			new PresetInventoryItem("Medicine", 94, 1, 20),
			new PresetInventoryItem("Medicine", 130, 1, 20),
			new PresetInventoryItem("Medicine", 142, 1, 20),
			new PresetInventoryItem("Medicine", 154, 1, 20),
			new PresetInventoryItem("Medicine", 166, 1, 20),
			new PresetInventoryItem("Medicine", 178, 1, 20),
			new PresetInventoryItem("Medicine", 190, 1, 20),
			new PresetInventoryItem("Medicine", 226, 1, 10),
			new PresetInventoryItem("Medicine", 238, 1, 10),
			new PresetInventoryItem("Medicine", 250, 1, 10),
			new PresetInventoryItem("Medicine", 274, 1, 10),
			new PresetInventoryItem("Medicine", 298, 1, 10),
			new PresetInventoryItem("Medicine", 322, 1, 10),
			new PresetInventoryItem("Material", 140, 1, 10),
			new PresetInventoryItem("Material", 144, 1, 10),
			new PresetInventoryItem("Material", 148, 1, 10),
			new PresetInventoryItem("Material", 152, 1, 10),
			new PresetInventoryItem("Material", 156, 1, 10),
			new PresetInventoryItem("Material", 160, 1, 10),
			new PresetInventoryItem("Material", 164, 1, 10),
			new PresetInventoryItem("Material", 168, 1, 10),
			new PresetInventoryItem("Material", 172, 1, 10),
			new PresetInventoryItem("Material", 176, 1, 10),
			new PresetInventoryItem("Material", 180, 1, 10),
			new PresetInventoryItem("Material", 184, 1, 10),
			new PresetInventoryItem("Material", 188, 1, 10),
			new PresetInventoryItem("Material", 192, 1, 10),
			new PresetInventoryItem("Material", 196, 1, 10),
			new PresetInventoryItem("Material", 200, 1, 10),
			new PresetInventoryItem("Material", 204, 1, 10),
			new PresetInventoryItem("Material", 208, 1, 10),
			new PresetInventoryItem("Material", 212, 1, 10),
			new PresetInventoryItem("Material", 216, 1, 10),
			new PresetInventoryItem("Material", 220, 1, 10),
			new PresetInventoryItem("Material", 224, 1, 10),
			new PresetInventoryItem("Material", 228, 1, 10),
			new PresetInventoryItem("Material", 232, 1, 10),
			new PresetInventoryItem("Material", 236, 1, 10),
			new PresetInventoryItem("Material", 243, 1, 10),
			new PresetInventoryItem("Material", 250, 1, 10),
			new PresetInventoryItem("Material", 257, 1, 10),
			new PresetInventoryItem("Material", 264, 1, 10),
			new PresetInventoryItem("Material", 271, 1, 10)
		}, new List<PresetOrgMemberCombatSkill>
		{
			new PresetOrgMemberCombatSkill(61, 0),
			new PresetOrgMemberCombatSkill(165, 0),
			new PresetOrgMemberCombatSkill(494, 0)
		}, new sbyte[5], new short[8] { -70, -70, -70, -80, -70, -50, -70, -80 }, 5000, 1000, 1, 100, 300, new short[16]
		{
			-1, -1, -1, -1, -1, 2, -1, -1, 12, 12,
			6, 6, -1, -1, -1, 2
		}, 1, new short[14]
		{
			6, 6, 12, 6, 6, 12, 12, -1, -1, -1,
			-1, -1, -1, -1
		}, new short[6] { -1, 9, -1, 9, 9, 9 }, new List<sbyte> { 5, 64 }, 3, new ResourceInts(default(int), default(int), default(int), default(int), default(int), default(int), default(int), default(int)), new IntPair[18]
		{
			new IntPair(17, 1),
			new IntPair(8, 3),
			new IntPair(5, 1),
			new IntPair(6, 1),
			new IntPair(15, 3),
			new IntPair(16, 3),
			new IntPair(7, 0),
			new IntPair(14, 3),
			new IntPair(12, 1),
			new IntPair(4, 6),
			new IntPair(3, 1),
			new IntPair(13, 30),
			new IntPair(10, 3),
			new IntPair(2, 3),
			new IntPair(1, 9),
			new IntPair(11, 9),
			new IntPair(0, 30),
			new IntPair(9, 1)
		}, null));
		_dataArray.Add(new OrganizationMemberItem(136, 640, 11, 8, 1, 1, 1, restrictPrincipalAmount: true, 1, -1, -1, -1, 7, -1, 7, 0, 0, new int[2] { 641, 642 }, 15800, 16, 13000, 30750, 0, new List<short> { 49, 50, 51 }, new List<short> { 27, 28, 29, 30 }, new int[2] { 643, 644 }, canStroll: false, 307, new short[4] { 34, 42, 50, 58 }, new PresetEquipmentItemWithProb[8]
		{
			new PresetEquipmentItemWithProb("Armor", 108, 75),
			new PresetEquipmentItemWithProb("Armor", 378, 100),
			new PresetEquipmentItemWithProb("Armor", 504, 75),
			new PresetEquipmentItemWithProb("Armor", 234, 75),
			new PresetEquipmentItemWithProb("Accessory", 99, 75),
			new PresetEquipmentItemWithProb("Accessory", 0, 50),
			new PresetEquipmentItemWithProb("Accessory", 9, 50),
			new PresetEquipmentItemWithProb("Carrier", 18, 75)
		}, new PresetEquipmentItem("Clothing", 51), new List<PresetInventoryItem>
		{
			new PresetInventoryItem("SkillBook", 117, 1, 40),
			new PresetInventoryItem("Food", 9, 3, 40),
			new PresetInventoryItem("Food", 51, 3, 40),
			new PresetInventoryItem("CraftTool", 45, 1, 30),
			new PresetInventoryItem("Medicine", 0, 1, 20),
			new PresetInventoryItem("Medicine", 27, 1, 20),
			new PresetInventoryItem("Medicine", 36, 1, 20),
			new PresetInventoryItem("Medicine", 60, 1, 30),
			new PresetInventoryItem("Medicine", 72, 1, 30),
			new PresetInventoryItem("Medicine", 100, 1, 20),
			new PresetInventoryItem("Medicine", 232, 1, 20),
			new PresetInventoryItem("Medicine", 280, 1, 20),
			new PresetInventoryItem("TeaWine", 0, 1, 20),
			new PresetInventoryItem("TeaWine", 9, 1, 20)
		}, new List<PresetOrgMemberCombatSkill>
		{
			new PresetOrgMemberCombatSkill(66, 8),
			new PresetOrgMemberCombatSkill(170, 3),
			new PresetOrgMemberCombatSkill(280, 7),
			new PresetOrgMemberCombatSkill(367, 7),
			new PresetOrgMemberCombatSkill(606, 8),
			new PresetOrgMemberCombatSkill(665, 7)
		}, new sbyte[5] { 10, 10, 10, 10, 10 }, new short[8] { -70, -80, -70, -70, -70, -70, -50, -60 }, 160000, 576000, 20, 20, 61500, new short[16]
		{
			-1, -1, -1, -1, -1, 6, -1, -1, -1, -1,
			-1, -1, 2, 12, -1, 9
		}, 8, new short[14]
		{
			12, 6, 12, 12, -1, -1, -1, -1, 12, -1,
			12, 2, -1, -1
		}, new short[6] { 12, 2, 9, 12, -1, -1 }, new List<sbyte> { 19, 24, 25, 55 }, 3, new ResourceInts(default(int), default(int), default(int), default(int), default(int), default(int), default(int), default(int)), new IntPair[18]
		{
			new IntPair(17, 30),
			new IntPair(8, 15),
			new IntPair(5, 0),
			new IntPair(6, 36),
			new IntPair(15, 6),
			new IntPair(16, 9),
			new IntPair(7, 15),
			new IntPair(14, 0),
			new IntPair(12, 3),
			new IntPair(4, 1),
			new IntPair(3, 18),
			new IntPair(13, 1),
			new IntPair(10, 1),
			new IntPair(2, 3),
			new IntPair(1, 3),
			new IntPair(11, 1),
			new IntPair(0, 1),
			new IntPair(9, 1)
		}, null));
		_dataArray.Add(new OrganizationMemberItem(137, 645, 11, 7, 4, 6, 2, restrictPrincipalAmount: false, 1, -1, -1, -1, 7, -1, 7, 0, 0, new int[2] { 646, 647 }, 8600, 14, 10000, 21150, 0, new List<short> { 49, 50, 51 }, new List<short> { 27, 28, 29, 30 }, new int[2] { 648, 649 }, canStroll: false, 308, new short[4] { 31, 38, 45, 52 }, new PresetEquipmentItemWithProb[8]
		{
			new PresetEquipmentItemWithProb("Armor", 108, 75),
			new PresetEquipmentItemWithProb("Armor", 378, 100),
			new PresetEquipmentItemWithProb("Armor", 504, 75),
			new PresetEquipmentItemWithProb("Armor", 234, 75),
			new PresetEquipmentItemWithProb("Accessory", 99, 75),
			new PresetEquipmentItemWithProb("Accessory", 0, 50),
			new PresetEquipmentItemWithProb("Accessory", 9, 50),
			new PresetEquipmentItemWithProb("Carrier", 18, 75)
		}, new PresetEquipmentItem("Clothing", 50), new List<PresetInventoryItem>
		{
			new PresetInventoryItem("SkillBook", 117, 1, 40),
			new PresetInventoryItem("Food", 9, 3, 40),
			new PresetInventoryItem("Food", 51, 3, 40),
			new PresetInventoryItem("CraftTool", 45, 1, 30),
			new PresetInventoryItem("Medicine", 0, 1, 20),
			new PresetInventoryItem("Medicine", 27, 1, 20),
			new PresetInventoryItem("Medicine", 36, 1, 20),
			new PresetInventoryItem("Medicine", 60, 1, 30),
			new PresetInventoryItem("Medicine", 72, 1, 30),
			new PresetInventoryItem("Medicine", 100, 1, 20),
			new PresetInventoryItem("Medicine", 232, 1, 20),
			new PresetInventoryItem("Medicine", 280, 1, 20),
			new PresetInventoryItem("TeaWine", 0, 1, 20),
			new PresetInventoryItem("TeaWine", 9, 1, 20)
		}, new List<PresetOrgMemberCombatSkill>
		{
			new PresetOrgMemberCombatSkill(66, 7),
			new PresetOrgMemberCombatSkill(170, 3),
			new PresetOrgMemberCombatSkill(280, 6),
			new PresetOrgMemberCombatSkill(367, 6),
			new PresetOrgMemberCombatSkill(606, 7),
			new PresetOrgMemberCombatSkill(665, 6)
		}, new sbyte[5] { 8, 8, 8, 8, 8 }, new short[8] { -70, -80, -70, -70, -70, -70, -50, -60 }, 100000, 288000, 15, 30, 42300, new short[16]
		{
			-1, -1, -1, -1, -1, 6, -1, -1, -1, -1,
			-1, -1, 2, 12, -1, 9
		}, 8, new short[14]
		{
			12, 6, 12, 12, -1, -1, -1, -1, 12, -1,
			12, 2, -1, -1
		}, new short[6] { 12, 2, 9, 12, -1, -1 }, new List<sbyte> { 19, 24, 25 }, 3, new ResourceInts(default(int), default(int), default(int), default(int), default(int), default(int), default(int), default(int)), new IntPair[18]
		{
			new IntPair(17, 30),
			new IntPair(8, 15),
			new IntPair(5, 0),
			new IntPair(6, 36),
			new IntPair(15, 6),
			new IntPair(16, 9),
			new IntPair(7, 15),
			new IntPair(14, 0),
			new IntPair(12, 3),
			new IntPair(4, 1),
			new IntPair(3, 18),
			new IntPair(13, 1),
			new IntPair(10, 1),
			new IntPair(2, 3),
			new IntPair(1, 3),
			new IntPair(11, 1),
			new IntPair(0, 1),
			new IntPair(9, 1)
		}, null));
		_dataArray.Add(new OrganizationMemberItem(138, 650, 11, 6, 4, 6, 2, restrictPrincipalAmount: false, 1, -1, -1, -1, 6, -1, 6, 0, 0, new int[2] { 651, 652 }, 6100, 12, 8000, 13800, 0, new List<short> { 49, 50, 51 }, new List<short> { 27, 28, 29, 30 }, new int[2] { 653, 654 }, canStroll: false, 309, new short[4] { 28, 34, 40, 46 }, new PresetEquipmentItemWithProb[8]
		{
			new PresetEquipmentItemWithProb("Armor", 108, 75),
			new PresetEquipmentItemWithProb("Armor", 378, 100),
			new PresetEquipmentItemWithProb("Armor", 504, 75),
			new PresetEquipmentItemWithProb("Armor", 234, 75),
			new PresetEquipmentItemWithProb("Accessory", 99, 75),
			new PresetEquipmentItemWithProb("Accessory", 0, 50),
			new PresetEquipmentItemWithProb("Accessory", 9, 50),
			new PresetEquipmentItemWithProb("Carrier", 18, 75)
		}, new PresetEquipmentItem("Clothing", 50), new List<PresetInventoryItem>
		{
			new PresetInventoryItem("SkillBook", 117, 1, 40),
			new PresetInventoryItem("Food", 9, 3, 40),
			new PresetInventoryItem("Food", 51, 3, 40),
			new PresetInventoryItem("CraftTool", 45, 1, 30),
			new PresetInventoryItem("Medicine", 0, 1, 20),
			new PresetInventoryItem("Medicine", 27, 1, 20),
			new PresetInventoryItem("Medicine", 36, 1, 20),
			new PresetInventoryItem("Medicine", 60, 1, 30),
			new PresetInventoryItem("Medicine", 72, 1, 30),
			new PresetInventoryItem("Medicine", 100, 1, 20),
			new PresetInventoryItem("Medicine", 232, 1, 20),
			new PresetInventoryItem("Medicine", 280, 1, 20),
			new PresetInventoryItem("TeaWine", 0, 1, 20),
			new PresetInventoryItem("TeaWine", 9, 1, 20)
		}, new List<PresetOrgMemberCombatSkill>
		{
			new PresetOrgMemberCombatSkill(66, 6),
			new PresetOrgMemberCombatSkill(170, 3),
			new PresetOrgMemberCombatSkill(280, 6),
			new PresetOrgMemberCombatSkill(367, 6),
			new PresetOrgMemberCombatSkill(606, 6),
			new PresetOrgMemberCombatSkill(665, 6)
		}, new sbyte[5] { 8, 8, 8, 8, 8 }, new short[8] { -70, -80, -70, -70, -70, -70, -50, -60 }, 60000, 144000, 15, 40, 27600, new short[16]
		{
			-1, -1, -1, -1, -1, 6, -1, -1, -1, -1,
			-1, -1, 2, 12, -1, 9
		}, 7, new short[14]
		{
			12, 6, 12, 12, -1, -1, -1, -1, 12, -1,
			12, 2, -1, -1
		}, new short[6] { 12, 2, 9, 12, -1, -1 }, new List<sbyte> { 19, 24, 25 }, 3, new ResourceInts(default(int), default(int), default(int), default(int), default(int), default(int), default(int), default(int)), new IntPair[18]
		{
			new IntPair(17, 30),
			new IntPair(8, 15),
			new IntPair(5, 0),
			new IntPair(6, 36),
			new IntPair(15, 6),
			new IntPair(16, 9),
			new IntPair(7, 15),
			new IntPair(14, 0),
			new IntPair(12, 3),
			new IntPair(4, 1),
			new IntPair(3, 18),
			new IntPair(13, 1),
			new IntPair(10, 1),
			new IntPair(2, 3),
			new IntPair(1, 3),
			new IntPair(11, 1),
			new IntPair(0, 1),
			new IntPair(9, 1)
		}, null));
		List<OrganizationMemberItem> dataArray = _dataArray;
		int[] monasticTitleSuffixes = new int[2] { 656, 657 };
		List<short> favoriteClothingIds = new List<short> { 49, 50, 51 };
		List<short> hatedClothingIds = new List<short> { 27, 28, 29, 30 };
		int[] spouseAnonymousTitles = new int[2] { 658, 659 };
		short[] initialAges = new short[4] { 25, 30, 35, 40 };
		PresetEquipmentItemWithProb[] equipment = new PresetEquipmentItemWithProb[8]
		{
			new PresetEquipmentItemWithProb("Armor", 108, 75),
			new PresetEquipmentItemWithProb("Armor", 378, 100),
			new PresetEquipmentItemWithProb("Armor", 504, 75),
			new PresetEquipmentItemWithProb("Armor", 234, 75),
			new PresetEquipmentItemWithProb("Accessory", 99, 75),
			new PresetEquipmentItemWithProb("Accessory", 0, 50),
			new PresetEquipmentItemWithProb("Accessory", 9, 50),
			new PresetEquipmentItemWithProb("Carrier", 18, 75)
		};
		PresetEquipmentItem clothing = new PresetEquipmentItem("Clothing", 50);
		List<PresetInventoryItem> inventory = new List<PresetInventoryItem>
		{
			new PresetInventoryItem("SkillBook", 117, 1, 30),
			new PresetInventoryItem("Food", 9, 3, 30),
			new PresetInventoryItem("Food", 51, 3, 30),
			new PresetInventoryItem("CraftTool", 45, 1, 30),
			new PresetInventoryItem("Medicine", 0, 1, 20),
			new PresetInventoryItem("Medicine", 27, 1, 20),
			new PresetInventoryItem("Medicine", 36, 1, 20),
			new PresetInventoryItem("Medicine", 54, 1, 30),
			new PresetInventoryItem("Medicine", 66, 1, 30),
			new PresetInventoryItem("Medicine", 94, 1, 20),
			new PresetInventoryItem("Medicine", 226, 1, 10),
			new PresetInventoryItem("Medicine", 274, 1, 10),
			new PresetInventoryItem("TeaWine", 0, 1, 20),
			new PresetInventoryItem("TeaWine", 9, 1, 20)
		};
		List<PresetOrgMemberCombatSkill> combatSkills = new List<PresetOrgMemberCombatSkill>
		{
			new PresetOrgMemberCombatSkill(66, 5),
			new PresetOrgMemberCombatSkill(170, 2),
			new PresetOrgMemberCombatSkill(280, 5),
			new PresetOrgMemberCombatSkill(606, 5),
			new PresetOrgMemberCombatSkill(665, 5)
		};
		sbyte[] extraCombatSkillGrids = new sbyte[5] { 6, 6, 6, 6, 6 };
		short[] resourcesAdjust = new short[8] { -70, -80, -70, -70, -70, -70, -50, -60 };
		short[] lifeSkillsAdjust = new short[16]
		{
			-1, -1, -1, -1, -1, 6, -1, -1, -1, -1,
			-1, -1, 2, 12, -1, 9
		};
		short[] combatSkillsAdjust = new short[14]
		{
			12, 6, 12, 12, -1, -1, -1, -1, 12, -1,
			12, 2, -1, -1
		};
		short[] mainAttributesAdjust = new short[6] { 12, 2, 9, 12, -1, -1 };
		List<sbyte> identityInteractConfig = new List<sbyte>();
		dataArray.Add(new OrganizationMemberItem(139, 655, 11, 5, 6, 9, 3, restrictPrincipalAmount: false, -1, -1, -1, 2, 5, 8, 5, 0, 0, monasticTitleSuffixes, 4200, 10, 6000, 8400, 0, favoriteClothingIds, hatedClothingIds, spouseAnonymousTitles, canStroll: false, 310, initialAges, equipment, clothing, inventory, combatSkills, extraCombatSkillGrids, resourcesAdjust, 40000, 48000, 10, 50, 16800, lifeSkillsAdjust, 6, combatSkillsAdjust, mainAttributesAdjust, identityInteractConfig, 3, new ResourceInts(default(int), default(int), default(int), default(int), default(int), default(int), default(int), default(int)), new IntPair[18]
		{
			new IntPair(17, 24),
			new IntPair(8, 12),
			new IntPair(5, 0),
			new IntPair(6, 27),
			new IntPair(15, 6),
			new IntPair(16, 6),
			new IntPair(7, 12),
			new IntPair(14, 0),
			new IntPair(12, 6),
			new IntPair(4, 1),
			new IntPair(3, 21),
			new IntPair(13, 1),
			new IntPair(10, 1),
			new IntPair(2, 3),
			new IntPair(1, 3),
			new IntPair(11, 1),
			new IntPair(0, 1),
			new IntPair(9, 1)
		}, null));
		List<OrganizationMemberItem> dataArray2 = _dataArray;
		int[] monasticTitleSuffixes2 = new int[2] { 661, 662 };
		List<short> favoriteClothingIds2 = new List<short> { 49, 50, 51 };
		List<short> hatedClothingIds2 = new List<short> { 27, 28, 29, 30 };
		int[] spouseAnonymousTitles2 = new int[2] { 663, 664 };
		short[] initialAges2 = new short[4] { 22, 26, 30, 34 };
		PresetEquipmentItemWithProb[] equipment2 = new PresetEquipmentItemWithProb[8]
		{
			new PresetEquipmentItemWithProb("Armor", 108, 75),
			new PresetEquipmentItemWithProb("Armor", 378, 100),
			new PresetEquipmentItemWithProb("Armor", 504, 75),
			new PresetEquipmentItemWithProb("Armor", 234, 75),
			new PresetEquipmentItemWithProb("Accessory", 99, 75),
			new PresetEquipmentItemWithProb("Accessory", 0, 50),
			new PresetEquipmentItemWithProb("Accessory", 9, 50),
			new PresetEquipmentItemWithProb("Carrier", 18, 75)
		};
		PresetEquipmentItem clothing2 = new PresetEquipmentItem("Clothing", 49);
		List<PresetInventoryItem> inventory2 = new List<PresetInventoryItem>
		{
			new PresetInventoryItem("SkillBook", 117, 1, 30),
			new PresetInventoryItem("Food", 9, 3, 30),
			new PresetInventoryItem("Food", 51, 3, 30),
			new PresetInventoryItem("CraftTool", 45, 1, 30),
			new PresetInventoryItem("Medicine", 0, 1, 20),
			new PresetInventoryItem("Medicine", 27, 1, 20),
			new PresetInventoryItem("Medicine", 36, 1, 20),
			new PresetInventoryItem("Medicine", 54, 1, 30),
			new PresetInventoryItem("Medicine", 66, 1, 30),
			new PresetInventoryItem("Medicine", 94, 1, 20),
			new PresetInventoryItem("Medicine", 226, 1, 10),
			new PresetInventoryItem("Medicine", 274, 1, 10),
			new PresetInventoryItem("TeaWine", 0, 1, 20),
			new PresetInventoryItem("TeaWine", 9, 1, 20)
		};
		List<PresetOrgMemberCombatSkill> combatSkills2 = new List<PresetOrgMemberCombatSkill>
		{
			new PresetOrgMemberCombatSkill(66, 4),
			new PresetOrgMemberCombatSkill(170, 2),
			new PresetOrgMemberCombatSkill(280, 4),
			new PresetOrgMemberCombatSkill(367, 4),
			new PresetOrgMemberCombatSkill(665, 4)
		};
		sbyte[] extraCombatSkillGrids2 = new sbyte[5] { 4, 4, 4, 4, 4 };
		short[] resourcesAdjust2 = new short[8] { -70, -80, -70, -70, -70, -70, -50, -60 };
		short[] lifeSkillsAdjust2 = new short[16]
		{
			-1, -1, -1, -1, -1, 6, -1, -1, -1, -1,
			-1, -1, 2, 12, -1, 9
		};
		short[] combatSkillsAdjust2 = new short[14]
		{
			12, 6, 12, 12, -1, -1, -1, -1, 12, -1,
			12, 2, -1, -1
		};
		short[] mainAttributesAdjust2 = new short[6] { 12, 2, 9, 12, -1, -1 };
		identityInteractConfig = new List<sbyte>();
		dataArray2.Add(new OrganizationMemberItem(140, 660, 11, 4, 6, 9, 3, restrictPrincipalAmount: false, -1, -1, -1, 1, 4, 7, 4, 0, 0, monasticTitleSuffixes2, 2800, 8, 4500, 4650, 0, favoriteClothingIds2, hatedClothingIds2, spouseAnonymousTitles2, canStroll: true, 311, initialAges2, equipment2, clothing2, inventory2, combatSkills2, extraCombatSkillGrids2, resourcesAdjust2, 30000, 24000, 10, 60, 9300, lifeSkillsAdjust2, 5, combatSkillsAdjust2, mainAttributesAdjust2, identityInteractConfig, 3, new ResourceInts(default(int), default(int), default(int), default(int), default(int), default(int), default(int), default(int)), new IntPair[18]
		{
			new IntPair(17, 24),
			new IntPair(8, 12),
			new IntPair(5, 0),
			new IntPair(6, 27),
			new IntPair(15, 6),
			new IntPair(16, 6),
			new IntPair(7, 12),
			new IntPair(14, 0),
			new IntPair(12, 6),
			new IntPair(4, 1),
			new IntPair(3, 21),
			new IntPair(13, 1),
			new IntPair(10, 1),
			new IntPair(2, 3),
			new IntPair(1, 3),
			new IntPair(11, 1),
			new IntPair(0, 1),
			new IntPair(9, 1)
		}, null));
		List<OrganizationMemberItem> dataArray3 = _dataArray;
		int[] monasticTitleSuffixes3 = new int[2] { 666, 667 };
		List<short> favoriteClothingIds3 = new List<short> { 49, 50, 51 };
		List<short> hatedClothingIds3 = new List<short> { 27, 28, 29, 30 };
		int[] spouseAnonymousTitles3 = new int[2] { 668, 669 };
		short[] initialAges3 = new short[4] { 19, 22, 25, 28 };
		PresetEquipmentItemWithProb[] equipment3 = new PresetEquipmentItemWithProb[8]
		{
			new PresetEquipmentItemWithProb("Armor", 108, 75),
			new PresetEquipmentItemWithProb("Armor", 378, 100),
			new PresetEquipmentItemWithProb("Armor", 504, 75),
			new PresetEquipmentItemWithProb("Armor", 234, 75),
			new PresetEquipmentItemWithProb("Accessory", 99, 75),
			new PresetEquipmentItemWithProb("Accessory", 0, 50),
			new PresetEquipmentItemWithProb("Accessory", 9, 50),
			new PresetEquipmentItemWithProb("Carrier", 18, 75)
		};
		PresetEquipmentItem clothing3 = new PresetEquipmentItem("Clothing", 49);
		List<PresetInventoryItem> inventory3 = new List<PresetInventoryItem>
		{
			new PresetInventoryItem("SkillBook", 117, 1, 30),
			new PresetInventoryItem("Food", 9, 3, 30),
			new PresetInventoryItem("Food", 51, 3, 30),
			new PresetInventoryItem("CraftTool", 45, 1, 30),
			new PresetInventoryItem("Medicine", 0, 1, 20),
			new PresetInventoryItem("Medicine", 27, 1, 20),
			new PresetInventoryItem("Medicine", 36, 1, 20),
			new PresetInventoryItem("Medicine", 54, 1, 30),
			new PresetInventoryItem("Medicine", 66, 1, 30),
			new PresetInventoryItem("Medicine", 94, 1, 20),
			new PresetInventoryItem("Medicine", 226, 1, 10),
			new PresetInventoryItem("Medicine", 274, 1, 10),
			new PresetInventoryItem("TeaWine", 0, 1, 20),
			new PresetInventoryItem("TeaWine", 9, 1, 20)
		};
		List<PresetOrgMemberCombatSkill> combatSkills3 = new List<PresetOrgMemberCombatSkill>
		{
			new PresetOrgMemberCombatSkill(66, 3),
			new PresetOrgMemberCombatSkill(170, 1),
			new PresetOrgMemberCombatSkill(280, 3),
			new PresetOrgMemberCombatSkill(665, 3)
		};
		sbyte[] extraCombatSkillGrids3 = new sbyte[5] { 4, 4, 4, 4, 4 };
		short[] resourcesAdjust3 = new short[8] { -70, -80, -70, -70, -70, -70, -50, -60 };
		short[] lifeSkillsAdjust3 = new short[16]
		{
			-1, -1, -1, -1, -1, 6, -1, -1, -1, -1,
			-1, -1, 2, 12, -1, 9
		};
		short[] combatSkillsAdjust3 = new short[14]
		{
			12, 6, 12, 12, -1, -1, -1, -1, 12, -1,
			12, 2, -1, -1
		};
		short[] mainAttributesAdjust3 = new short[6] { 12, 2, 9, 12, -1, -1 };
		identityInteractConfig = new List<sbyte>();
		dataArray3.Add(new OrganizationMemberItem(141, 665, 11, 3, 8, 12, 4, restrictPrincipalAmount: false, -1, -1, -1, 0, 3, 6, 3, 0, 0, monasticTitleSuffixes3, 1800, 6, 3000, 2250, 0, favoriteClothingIds3, hatedClothingIds3, spouseAnonymousTitles3, canStroll: true, 312, initialAges3, equipment3, clothing3, inventory3, combatSkills3, extraCombatSkillGrids3, resourcesAdjust3, 20000, 12000, 10, 70, 4500, lifeSkillsAdjust3, 4, combatSkillsAdjust3, mainAttributesAdjust3, identityInteractConfig, 3, new ResourceInts(default(int), default(int), default(int), default(int), default(int), default(int), default(int), default(int)), new IntPair[18]
		{
			new IntPair(17, 24),
			new IntPair(8, 12),
			new IntPair(5, 0),
			new IntPair(6, 27),
			new IntPair(15, 6),
			new IntPair(16, 6),
			new IntPair(7, 12),
			new IntPair(14, 0),
			new IntPair(12, 6),
			new IntPair(4, 1),
			new IntPair(3, 21),
			new IntPair(13, 1),
			new IntPair(10, 1),
			new IntPair(2, 3),
			new IntPair(1, 3),
			new IntPair(11, 1),
			new IntPair(0, 1),
			new IntPair(9, 1)
		}, null));
		List<OrganizationMemberItem> dataArray4 = _dataArray;
		int[] monasticTitleSuffixes4 = new int[2] { 671, 672 };
		List<short> favoriteClothingIds4 = new List<short> { 49, 50, 51 };
		List<short> hatedClothingIds4 = new List<short> { 27, 28, 29, 30 };
		int[] spouseAnonymousTitles4 = new int[2] { 673, 674 };
		short[] initialAges4 = new short[4] { 16, 18, 20, 22 };
		PresetEquipmentItemWithProb[] equipment4 = new PresetEquipmentItemWithProb[8]
		{
			new PresetEquipmentItemWithProb("Armor", 108, 75),
			new PresetEquipmentItemWithProb("Armor", 378, 100),
			new PresetEquipmentItemWithProb("Armor", 504, 75),
			new PresetEquipmentItemWithProb("Armor", 234, 75),
			new PresetEquipmentItemWithProb("Accessory", 99, 75),
			new PresetEquipmentItemWithProb("Accessory", 0, 50),
			new PresetEquipmentItemWithProb("Accessory", 9, 50),
			new PresetEquipmentItemWithProb("Carrier", 18, 75)
		};
		PresetEquipmentItem clothing4 = new PresetEquipmentItem("Clothing", 49);
		List<PresetInventoryItem> inventory4 = new List<PresetInventoryItem>
		{
			new PresetInventoryItem("SkillBook", 117, 1, 20),
			new PresetInventoryItem("Food", 9, 3, 20),
			new PresetInventoryItem("Food", 51, 3, 20),
			new PresetInventoryItem("CraftTool", 45, 1, 30),
			new PresetInventoryItem("Medicine", 0, 1, 20),
			new PresetInventoryItem("Medicine", 27, 1, 20),
			new PresetInventoryItem("Medicine", 36, 1, 20),
			new PresetInventoryItem("Medicine", 54, 1, 20),
			new PresetInventoryItem("Medicine", 66, 1, 20)
		};
		List<PresetOrgMemberCombatSkill> combatSkills4 = new List<PresetOrgMemberCombatSkill>
		{
			new PresetOrgMemberCombatSkill(66, 2),
			new PresetOrgMemberCombatSkill(170, 1),
			new PresetOrgMemberCombatSkill(280, 2),
			new PresetOrgMemberCombatSkill(367, 2)
		};
		sbyte[] extraCombatSkillGrids4 = new sbyte[5] { 2, 2, 2, 2, 2 };
		short[] resourcesAdjust4 = new short[8] { -70, -80, -70, -70, -70, -70, -50, -60 };
		short[] lifeSkillsAdjust4 = new short[16]
		{
			-1, -1, -1, -1, -1, 6, -1, -1, -1, -1,
			-1, -1, 2, 12, -1, 9
		};
		short[] combatSkillsAdjust4 = new short[14]
		{
			12, 6, 12, 12, -1, -1, -1, -1, 12, -1,
			12, 2, -1, -1
		};
		short[] mainAttributesAdjust4 = new short[6] { 12, 2, 9, 12, -1, -1 };
		identityInteractConfig = new List<sbyte>();
		dataArray4.Add(new OrganizationMemberItem(142, 670, 11, 2, 8, 12, 4, restrictPrincipalAmount: false, -1, -1, -1, 0, 2, 5, 2, 0, 0, monasticTitleSuffixes4, 600, 4, 2000, 900, 0, favoriteClothingIds4, hatedClothingIds4, spouseAnonymousTitles4, canStroll: true, 313, initialAges4, equipment4, clothing4, inventory4, combatSkills4, extraCombatSkillGrids4, resourcesAdjust4, 15000, 4000, 5, 80, 1800, lifeSkillsAdjust4, 3, combatSkillsAdjust4, mainAttributesAdjust4, identityInteractConfig, 3, new ResourceInts(default(int), default(int), default(int), default(int), default(int), default(int), default(int), default(int)), new IntPair[18]
		{
			new IntPair(17, 18),
			new IntPair(8, 9),
			new IntPair(5, 0),
			new IntPair(6, 18),
			new IntPair(15, 6),
			new IntPair(16, 3),
			new IntPair(7, 9),
			new IntPair(14, 0),
			new IntPair(12, 9),
			new IntPair(4, 1),
			new IntPair(3, 24),
			new IntPair(13, 1),
			new IntPair(10, 1),
			new IntPair(2, 3),
			new IntPair(1, 3),
			new IntPair(11, 1),
			new IntPair(0, 1),
			new IntPair(9, 1)
		}, null));
		List<OrganizationMemberItem> dataArray5 = _dataArray;
		int[] monasticTitleSuffixes5 = new int[2] { 676, 677 };
		List<short> favoriteClothingIds5 = new List<short> { 49, 50, 51 };
		List<short> hatedClothingIds5 = new List<short> { 27, 28, 29, 30 };
		int[] spouseAnonymousTitles5 = new int[2] { 678, 679 };
		short[] initialAges5 = new short[4] { 13, 14, 15, 16 };
		PresetEquipmentItemWithProb[] equipment5 = new PresetEquipmentItemWithProb[8]
		{
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Armor", 378, 100),
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Armor", 234, 75),
			new PresetEquipmentItemWithProb("Accessory", 99, 75),
			new PresetEquipmentItemWithProb("Accessory", 0, 50),
			new PresetEquipmentItemWithProb("Accessory", -1, 0),
			new PresetEquipmentItemWithProb("Carrier", -1, 0)
		};
		PresetEquipmentItem clothing5 = new PresetEquipmentItem("Clothing", 49);
		List<PresetInventoryItem> inventory5 = new List<PresetInventoryItem>
		{
			new PresetInventoryItem("SkillBook", 117, 1, 20),
			new PresetInventoryItem("Food", 9, 3, 20),
			new PresetInventoryItem("Food", 51, 3, 20),
			new PresetInventoryItem("CraftTool", 45, 1, 30),
			new PresetInventoryItem("Medicine", 0, 1, 20),
			new PresetInventoryItem("Medicine", 27, 1, 20),
			new PresetInventoryItem("Medicine", 36, 1, 20),
			new PresetInventoryItem("Medicine", 54, 1, 20),
			new PresetInventoryItem("Medicine", 66, 1, 20)
		};
		List<PresetOrgMemberCombatSkill> combatSkills5 = new List<PresetOrgMemberCombatSkill>
		{
			new PresetOrgMemberCombatSkill(66, 1),
			new PresetOrgMemberCombatSkill(170, 1),
			new PresetOrgMemberCombatSkill(280, 1),
			new PresetOrgMemberCombatSkill(606, 1)
		};
		sbyte[] extraCombatSkillGrids5 = new sbyte[5];
		short[] resourcesAdjust5 = new short[8] { -70, -80, -70, -70, -70, -70, -50, -60 };
		short[] lifeSkillsAdjust5 = new short[16]
		{
			-1, -1, -1, -1, -1, 6, -1, -1, -1, -1,
			-1, -1, 2, 12, -1, 9
		};
		short[] combatSkillsAdjust5 = new short[14]
		{
			12, 6, 12, 12, -1, -1, -1, -1, 12, -1,
			12, 2, -1, -1
		};
		short[] mainAttributesAdjust5 = new short[6] { 12, 2, 9, 12, -1, -1 };
		identityInteractConfig = new List<sbyte>();
		dataArray5.Add(new OrganizationMemberItem(143, 675, 11, 1, 8, 12, 4, restrictPrincipalAmount: false, -1, -1, -1, 0, 1, 4, 1, 0, 0, monasticTitleSuffixes5, 300, 2, 1000, 300, 0, favoriteClothingIds5, hatedClothingIds5, spouseAnonymousTitles5, canStroll: true, 314, initialAges5, equipment5, clothing5, inventory5, combatSkills5, extraCombatSkillGrids5, resourcesAdjust5, 10000, 2000, 5, 90, 600, lifeSkillsAdjust5, 2, combatSkillsAdjust5, mainAttributesAdjust5, identityInteractConfig, 3, new ResourceInts(default(int), default(int), default(int), default(int), default(int), default(int), default(int), default(int)), new IntPair[18]
		{
			new IntPair(17, 18),
			new IntPair(8, 9),
			new IntPair(5, 0),
			new IntPair(6, 18),
			new IntPair(15, 6),
			new IntPair(16, 3),
			new IntPair(7, 9),
			new IntPair(14, 0),
			new IntPair(12, 9),
			new IntPair(4, 1),
			new IntPair(3, 24),
			new IntPair(13, 1),
			new IntPair(10, 1),
			new IntPair(2, 3),
			new IntPair(1, 3),
			new IntPair(11, 1),
			new IntPair(0, 1),
			new IntPair(9, 1)
		}, null));
		List<OrganizationMemberItem> dataArray6 = _dataArray;
		int[] monasticTitleSuffixes6 = new int[2] { 681, 682 };
		List<short> favoriteClothingIds6 = new List<short> { 49, 50, 51 };
		List<short> hatedClothingIds6 = new List<short> { 27, 28, 29, 30 };
		int[] spouseAnonymousTitles6 = new int[2] { 683, 684 };
		short[] initialAges6 = new short[4] { 10, 10, 10, 10 };
		PresetEquipmentItemWithProb[] equipment6 = new PresetEquipmentItemWithProb[8]
		{
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Armor", 378, 100),
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Armor", 234, 75),
			new PresetEquipmentItemWithProb("Accessory", 99, 75),
			new PresetEquipmentItemWithProb("Accessory", -1, 0),
			new PresetEquipmentItemWithProb("Accessory", -1, 0),
			new PresetEquipmentItemWithProb("Carrier", -1, 0)
		};
		PresetEquipmentItem clothing6 = new PresetEquipmentItem("Clothing", 49);
		List<PresetInventoryItem> inventory6 = new List<PresetInventoryItem>
		{
			new PresetInventoryItem("SkillBook", 117, 1, 20),
			new PresetInventoryItem("Food", 9, 3, 20),
			new PresetInventoryItem("Food", 51, 3, 20),
			new PresetInventoryItem("CraftTool", 45, 1, 30),
			new PresetInventoryItem("Medicine", 0, 1, 20),
			new PresetInventoryItem("Medicine", 27, 1, 20),
			new PresetInventoryItem("Medicine", 36, 1, 20),
			new PresetInventoryItem("Medicine", 54, 1, 20),
			new PresetInventoryItem("Medicine", 66, 1, 20)
		};
		List<PresetOrgMemberCombatSkill> combatSkills6 = new List<PresetOrgMemberCombatSkill>
		{
			new PresetOrgMemberCombatSkill(66, 0),
			new PresetOrgMemberCombatSkill(170, 0),
			new PresetOrgMemberCombatSkill(665, 0)
		};
		sbyte[] extraCombatSkillGrids6 = new sbyte[5];
		short[] resourcesAdjust6 = new short[8] { -70, -80, -70, -70, -70, -70, -50, -60 };
		short[] lifeSkillsAdjust6 = new short[16]
		{
			-1, -1, -1, -1, -1, 6, -1, -1, -1, -1,
			-1, -1, 2, 12, -1, 9
		};
		short[] combatSkillsAdjust6 = new short[14]
		{
			12, 6, 12, 12, -1, -1, -1, -1, 12, -1,
			12, 2, -1, -1
		};
		short[] mainAttributesAdjust6 = new short[6] { 12, 2, 9, 12, -1, -1 };
		identityInteractConfig = new List<sbyte>();
		dataArray6.Add(new OrganizationMemberItem(144, 680, 11, 0, 6, 9, 3, restrictPrincipalAmount: false, -1, -1, -1, 0, 0, 4, 0, 0, 0, monasticTitleSuffixes6, 150, 0, 500, 150, 0, favoriteClothingIds6, hatedClothingIds6, spouseAnonymousTitles6, canStroll: true, 315, initialAges6, equipment6, clothing6, inventory6, combatSkills6, extraCombatSkillGrids6, resourcesAdjust6, 5000, 1000, 1, 100, 300, lifeSkillsAdjust6, 1, combatSkillsAdjust6, mainAttributesAdjust6, identityInteractConfig, 3, new ResourceInts(default(int), default(int), default(int), default(int), default(int), default(int), default(int), default(int)), new IntPair[18]
		{
			new IntPair(17, 18),
			new IntPair(8, 9),
			new IntPair(5, 0),
			new IntPair(6, 18),
			new IntPair(15, 6),
			new IntPair(16, 3),
			new IntPair(7, 9),
			new IntPair(14, 0),
			new IntPair(12, 9),
			new IntPair(4, 1),
			new IntPair(3, 24),
			new IntPair(13, 1),
			new IntPair(10, 1),
			new IntPair(2, 3),
			new IntPair(1, 3),
			new IntPair(11, 1),
			new IntPair(0, 1),
			new IntPair(9, 1)
		}, null));
		_dataArray.Add(new OrganizationMemberItem(145, 685, 12, 8, 1, 1, 1, restrictPrincipalAmount: true, 0, -1, -1, -1, 5, -1, 6, 0, 0, new int[2] { 686, 687 }, 15800, 16, 13000, 30750, 0, new List<short> { 52, 53, 54 }, new List<short> { 18, 19, 20 }, new int[2] { 688, 689 }, canStroll: false, 316, new short[4] { 16, 19, 22, 25 }, new PresetEquipmentItemWithProb[8]
		{
			new PresetEquipmentItemWithProb("Armor", 27, 75),
			new PresetEquipmentItemWithProb("Armor", 297, 100),
			new PresetEquipmentItemWithProb("Armor", 423, 75),
			new PresetEquipmentItemWithProb("Armor", 153, 75),
			new PresetEquipmentItemWithProb("Accessory", 162, 75),
			new PresetEquipmentItemWithProb("Accessory", 135, 50),
			new PresetEquipmentItemWithProb("Accessory", 126, 50),
			new PresetEquipmentItemWithProb("Carrier", 9, 75)
		}, new PresetEquipmentItem("Clothing", 54), new List<PresetInventoryItem>
		{
			new PresetInventoryItem("SkillBook", 81, 3, 40),
			new PresetInventoryItem("Material", 0, 1, 30),
			new PresetInventoryItem("Material", 7, 1, 30),
			new PresetInventoryItem("CraftTool", 45, 1, 30),
			new PresetInventoryItem("Food", 0, 3, 40),
			new PresetInventoryItem("Food", 9, 3, 40),
			new PresetInventoryItem("Medicine", 0, 1, 30),
			new PresetInventoryItem("Medicine", 9, 1, 30),
			new PresetInventoryItem("Medicine", 18, 1, 30),
			new PresetInventoryItem("Medicine", 27, 1, 30),
			new PresetInventoryItem("Medicine", 36, 1, 30),
			new PresetInventoryItem("Medicine", 45, 1, 30),
			new PresetInventoryItem("Medicine", 60, 1, 20),
			new PresetInventoryItem("Medicine", 72, 1, 20),
			new PresetInventoryItem("Medicine", 88, 1, 20),
			new PresetInventoryItem("Medicine", 136, 1, 20),
			new PresetInventoryItem("Medicine", 148, 1, 20),
			new PresetInventoryItem("Medicine", 160, 1, 20),
			new PresetInventoryItem("Medicine", 172, 1, 20),
			new PresetInventoryItem("Medicine", 184, 1, 20),
			new PresetInventoryItem("Medicine", 196, 1, 20),
			new PresetInventoryItem("Medicine", 256, 1, 20),
			new PresetInventoryItem("Medicine", 328, 1, 20),
			new PresetInventoryItem("TeaWine", 0, 1, 20),
			new PresetInventoryItem("TeaWine", 9, 1, 20),
			new PresetInventoryItem("Material", 236, 1, 10),
			new PresetInventoryItem("Material", 243, 1, 10),
			new PresetInventoryItem("Material", 250, 1, 10),
			new PresetInventoryItem("Material", 257, 1, 10),
			new PresetInventoryItem("Material", 264, 1, 10),
			new PresetInventoryItem("Material", 271, 1, 10)
		}, new List<PresetOrgMemberCombatSkill>
		{
			new PresetOrgMemberCombatSkill(75, 6),
			new PresetOrgMemberCombatSkill(174, 6),
			new PresetOrgMemberCombatSkill(288, 8),
			new PresetOrgMemberCombatSkill(375, 7),
			new PresetOrgMemberCombatSkill(444, 8),
			new PresetOrgMemberCombatSkill(566, 6),
			new PresetOrgMemberCombatSkill(681, 8)
		}, new sbyte[5] { 10, 10, 10, 10, 10 }, new short[8] { -70, -50, -70, -80, -70, -70, -70, -70 }, 120000, 432000, 20, 20, 61500, new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, 9, 9, 12,
			-1, -1, 2, 2, -1, -1
		}, 8, new short[14]
		{
			9, 9, 12, 12, 12, -1, -1, 9, -1, 2,
			-1, 12, -1, -1
		}, new short[6] { -1, 12, 2, -1, 9, 12 }, new List<sbyte> { 5, 20, 24, 25, 55, 64 }, 3, new ResourceInts(default(int), default(int), default(int), default(int), default(int), default(int), default(int), default(int)), new IntPair[18]
		{
			new IntPair(17, 1),
			new IntPair(8, 1),
			new IntPair(5, 1),
			new IntPair(6, 1),
			new IntPair(15, 1),
			new IntPair(16, 3),
			new IntPair(7, 3),
			new IntPair(14, 1),
			new IntPair(12, 1),
			new IntPair(4, 15),
			new IntPair(3, 6),
			new IntPair(13, 54),
			new IntPair(10, 12),
			new IntPair(2, 12),
			new IntPair(1, 9),
			new IntPair(11, 1),
			new IntPair(0, 21),
			new IntPair(9, 1)
		}, null));
		_dataArray.Add(new OrganizationMemberItem(146, 690, 12, 7, 4, 6, 2, restrictPrincipalAmount: false, 0, -1, -1, -1, 5, 8, 6, 0, 0, new int[2] { 691, 692 }, 8600, 14, 10000, 21150, 0, new List<short> { 52, 53, 54 }, new List<short> { 18, 19, 20 }, new int[2] { 693, 694 }, canStroll: false, 317, new short[4] { 12, 13, 14, 15 }, new PresetEquipmentItemWithProb[8]
		{
			new PresetEquipmentItemWithProb("Armor", 27, 75),
			new PresetEquipmentItemWithProb("Armor", 297, 100),
			new PresetEquipmentItemWithProb("Armor", 423, 75),
			new PresetEquipmentItemWithProb("Armor", 153, 75),
			new PresetEquipmentItemWithProb("Accessory", 162, 75),
			new PresetEquipmentItemWithProb("Accessory", 135, 50),
			new PresetEquipmentItemWithProb("Accessory", 126, 50),
			new PresetEquipmentItemWithProb("Carrier", 9, 75)
		}, new PresetEquipmentItem("Clothing", 53), new List<PresetInventoryItem>
		{
			new PresetInventoryItem("SkillBook", 81, 3, 40),
			new PresetInventoryItem("Material", 0, 1, 30),
			new PresetInventoryItem("Material", 7, 1, 30),
			new PresetInventoryItem("CraftTool", 45, 1, 30),
			new PresetInventoryItem("Food", 0, 3, 40),
			new PresetInventoryItem("Food", 9, 3, 40),
			new PresetInventoryItem("Medicine", 0, 1, 30),
			new PresetInventoryItem("Medicine", 9, 1, 30),
			new PresetInventoryItem("Medicine", 18, 1, 30),
			new PresetInventoryItem("Medicine", 27, 1, 30),
			new PresetInventoryItem("Medicine", 36, 1, 30),
			new PresetInventoryItem("Medicine", 45, 1, 30),
			new PresetInventoryItem("Medicine", 60, 1, 20),
			new PresetInventoryItem("Medicine", 72, 1, 20),
			new PresetInventoryItem("Medicine", 88, 1, 20),
			new PresetInventoryItem("Medicine", 136, 1, 20),
			new PresetInventoryItem("Medicine", 148, 1, 20),
			new PresetInventoryItem("Medicine", 160, 1, 20),
			new PresetInventoryItem("Medicine", 172, 1, 20),
			new PresetInventoryItem("Medicine", 184, 1, 20),
			new PresetInventoryItem("Medicine", 196, 1, 20),
			new PresetInventoryItem("Medicine", 256, 1, 20),
			new PresetInventoryItem("Medicine", 328, 1, 20),
			new PresetInventoryItem("TeaWine", 0, 1, 20),
			new PresetInventoryItem("TeaWine", 9, 1, 20),
			new PresetInventoryItem("Material", 236, 1, 10),
			new PresetInventoryItem("Material", 243, 1, 10),
			new PresetInventoryItem("Material", 250, 1, 10),
			new PresetInventoryItem("Material", 257, 1, 10),
			new PresetInventoryItem("Material", 264, 1, 10),
			new PresetInventoryItem("Material", 271, 1, 10)
		}, new List<PresetOrgMemberCombatSkill>
		{
			new PresetOrgMemberCombatSkill(75, 6),
			new PresetOrgMemberCombatSkill(174, 6),
			new PresetOrgMemberCombatSkill(288, 7),
			new PresetOrgMemberCombatSkill(375, 6),
			new PresetOrgMemberCombatSkill(444, 7),
			new PresetOrgMemberCombatSkill(566, 6),
			new PresetOrgMemberCombatSkill(681, 7)
		}, new sbyte[5] { 8, 8, 8, 8, 8 }, new short[8] { -70, -50, -70, -80, -70, -70, -70, -70 }, 75000, 216000, 15, 30, 42300, new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, 9, 9, 12,
			-1, -1, 2, 2, -1, -1
		}, 8, new short[14]
		{
			9, 9, 12, 12, 12, -1, -1, 9, -1, 2,
			-1, 12, -1, -1
		}, new short[6] { -1, 12, 2, -1, 9, 12 }, new List<sbyte> { 5, 20, 24, 25, 64 }, 3, new ResourceInts(default(int), default(int), default(int), default(int), default(int), default(int), default(int), default(int)), new IntPair[18]
		{
			new IntPair(17, 1),
			new IntPair(8, 1),
			new IntPair(5, 1),
			new IntPair(6, 1),
			new IntPair(15, 1),
			new IntPair(16, 3),
			new IntPair(7, 3),
			new IntPair(14, 1),
			new IntPair(12, 1),
			new IntPair(4, 15),
			new IntPair(3, 6),
			new IntPair(13, 54),
			new IntPair(10, 12),
			new IntPair(2, 12),
			new IntPair(1, 9),
			new IntPair(11, 1),
			new IntPair(0, 21),
			new IntPair(9, 1)
		}, null));
		_dataArray.Add(new OrganizationMemberItem(147, 695, 12, 6, 4, 6, 2, restrictPrincipalAmount: false, 0, -1, -1, -1, 5, 8, 6, 0, 0, new int[2] { 696, 697 }, 6100, 12, 8000, 13800, 0, new List<short> { 52, 53, 54 }, new List<short> { 18, 19, 20 }, new int[2] { 698, 699 }, canStroll: false, 318, new short[4] { 42, 50, 58, 66 }, new PresetEquipmentItemWithProb[8]
		{
			new PresetEquipmentItemWithProb("Armor", 27, 75),
			new PresetEquipmentItemWithProb("Armor", 297, 100),
			new PresetEquipmentItemWithProb("Armor", 423, 75),
			new PresetEquipmentItemWithProb("Armor", 153, 75),
			new PresetEquipmentItemWithProb("Accessory", 162, 75),
			new PresetEquipmentItemWithProb("Accessory", 135, 50),
			new PresetEquipmentItemWithProb("Accessory", 126, 50),
			new PresetEquipmentItemWithProb("Carrier", 9, 75)
		}, new PresetEquipmentItem("Clothing", 53), new List<PresetInventoryItem>
		{
			new PresetInventoryItem("SkillBook", 81, 3, 40),
			new PresetInventoryItem("Material", 0, 1, 30),
			new PresetInventoryItem("Material", 7, 1, 30),
			new PresetInventoryItem("CraftTool", 45, 1, 30),
			new PresetInventoryItem("Food", 0, 3, 40),
			new PresetInventoryItem("Food", 9, 3, 40),
			new PresetInventoryItem("Medicine", 0, 1, 30),
			new PresetInventoryItem("Medicine", 9, 1, 30),
			new PresetInventoryItem("Medicine", 18, 1, 30),
			new PresetInventoryItem("Medicine", 27, 1, 30),
			new PresetInventoryItem("Medicine", 36, 1, 30),
			new PresetInventoryItem("Medicine", 45, 1, 30),
			new PresetInventoryItem("Medicine", 60, 1, 20),
			new PresetInventoryItem("Medicine", 72, 1, 20),
			new PresetInventoryItem("Medicine", 88, 1, 20),
			new PresetInventoryItem("Medicine", 136, 1, 20),
			new PresetInventoryItem("Medicine", 148, 1, 20),
			new PresetInventoryItem("Medicine", 160, 1, 20),
			new PresetInventoryItem("Medicine", 172, 1, 20),
			new PresetInventoryItem("Medicine", 184, 1, 20),
			new PresetInventoryItem("Medicine", 196, 1, 20),
			new PresetInventoryItem("Medicine", 256, 1, 20),
			new PresetInventoryItem("Medicine", 328, 1, 20),
			new PresetInventoryItem("TeaWine", 0, 1, 20),
			new PresetInventoryItem("TeaWine", 9, 1, 20),
			new PresetInventoryItem("Material", 236, 1, 10),
			new PresetInventoryItem("Material", 243, 1, 10),
			new PresetInventoryItem("Material", 250, 1, 10),
			new PresetInventoryItem("Material", 257, 1, 10),
			new PresetInventoryItem("Material", 264, 1, 10),
			new PresetInventoryItem("Material", 271, 1, 10)
		}, new List<PresetOrgMemberCombatSkill>
		{
			new PresetOrgMemberCombatSkill(75, 5),
			new PresetOrgMemberCombatSkill(174, 5),
			new PresetOrgMemberCombatSkill(288, 6),
			new PresetOrgMemberCombatSkill(375, 6),
			new PresetOrgMemberCombatSkill(444, 6),
			new PresetOrgMemberCombatSkill(566, 5),
			new PresetOrgMemberCombatSkill(681, 6)
		}, new sbyte[5] { 8, 8, 8, 8, 8 }, new short[8] { -70, -50, -70, -80, -70, -70, -70, -70 }, 45000, 108000, 15, 40, 27600, new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, 9, 9, 12,
			-1, -1, 2, 2, -1, -1
		}, 7, new short[14]
		{
			9, 9, 12, 12, 12, -1, -1, 9, -1, 2,
			-1, 12, -1, -1
		}, new short[6] { -1, 12, 2, -1, 9, 12 }, new List<sbyte> { 5, 20, 24, 25, 64 }, 3, new ResourceInts(default(int), default(int), default(int), default(int), default(int), default(int), default(int), default(int)), new IntPair[18]
		{
			new IntPair(17, 1),
			new IntPair(8, 1),
			new IntPair(5, 1),
			new IntPair(6, 1),
			new IntPair(15, 1),
			new IntPair(16, 3),
			new IntPair(7, 3),
			new IntPair(14, 1),
			new IntPair(12, 1),
			new IntPair(4, 15),
			new IntPair(3, 6),
			new IntPair(13, 54),
			new IntPair(10, 12),
			new IntPair(2, 12),
			new IntPair(1, 9),
			new IntPair(11, 1),
			new IntPair(0, 21),
			new IntPair(9, 1)
		}, null));
		_dataArray.Add(new OrganizationMemberItem(148, 700, 12, 5, 4, 6, 2, restrictPrincipalAmount: false, -1, -1, -1, 3, 5, 8, 5, 0, 0, new int[2] { 701, 702 }, 4200, 10, 6000, 8400, 0, new List<short> { 52, 53, 54 }, new List<short> { 18, 19, 20 }, new int[2] { 703, 704 }, canStroll: false, 319, new short[4] { 38, 45, 52, 59 }, new PresetEquipmentItemWithProb[8]
		{
			new PresetEquipmentItemWithProb("Armor", 27, 75),
			new PresetEquipmentItemWithProb("Armor", 297, 100),
			new PresetEquipmentItemWithProb("Armor", 423, 75),
			new PresetEquipmentItemWithProb("Armor", 153, 75),
			new PresetEquipmentItemWithProb("Accessory", 162, 75),
			new PresetEquipmentItemWithProb("Accessory", 135, 50),
			new PresetEquipmentItemWithProb("Accessory", 126, 50),
			new PresetEquipmentItemWithProb("Carrier", 9, 75)
		}, new PresetEquipmentItem("Clothing", 53), new List<PresetInventoryItem>
		{
			new PresetInventoryItem("SkillBook", 81, 3, 30),
			new PresetInventoryItem("Material", 0, 1, 20),
			new PresetInventoryItem("Material", 7, 1, 20),
			new PresetInventoryItem("CraftTool", 45, 1, 30),
			new PresetInventoryItem("Food", 0, 3, 30),
			new PresetInventoryItem("Food", 9, 3, 30),
			new PresetInventoryItem("Medicine", 0, 1, 20),
			new PresetInventoryItem("Medicine", 9, 1, 20),
			new PresetInventoryItem("Medicine", 18, 1, 20),
			new PresetInventoryItem("Medicine", 27, 1, 20),
			new PresetInventoryItem("Medicine", 36, 1, 20),
			new PresetInventoryItem("Medicine", 45, 1, 20),
			new PresetInventoryItem("Medicine", 54, 1, 20),
			new PresetInventoryItem("Medicine", 66, 1, 20),
			new PresetInventoryItem("Medicine", 82, 1, 20),
			new PresetInventoryItem("Medicine", 136, 1, 20),
			new PresetInventoryItem("Medicine", 148, 1, 20),
			new PresetInventoryItem("Medicine", 160, 1, 20),
			new PresetInventoryItem("Medicine", 172, 1, 20),
			new PresetInventoryItem("Medicine", 184, 1, 20),
			new PresetInventoryItem("Medicine", 196, 1, 20),
			new PresetInventoryItem("Medicine", 250, 1, 10),
			new PresetInventoryItem("Medicine", 322, 1, 10),
			new PresetInventoryItem("TeaWine", 0, 1, 20),
			new PresetInventoryItem("TeaWine", 9, 1, 20),
			new PresetInventoryItem("Material", 236, 1, 10),
			new PresetInventoryItem("Material", 243, 1, 10),
			new PresetInventoryItem("Material", 250, 1, 10),
			new PresetInventoryItem("Material", 257, 1, 10),
			new PresetInventoryItem("Material", 264, 1, 10),
			new PresetInventoryItem("Material", 271, 1, 10)
		}, new List<PresetOrgMemberCombatSkill>
		{
			new PresetOrgMemberCombatSkill(75, 5),
			new PresetOrgMemberCombatSkill(174, 5),
			new PresetOrgMemberCombatSkill(288, 5),
			new PresetOrgMemberCombatSkill(375, 5),
			new PresetOrgMemberCombatSkill(444, 5),
			new PresetOrgMemberCombatSkill(566, 5)
		}, new sbyte[5] { 6, 6, 6, 6, 6 }, new short[8] { -70, -50, -70, -80, -70, -70, -70, -70 }, 30000, 36000, 10, 50, 16800, new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, 9, 9, 12,
			-1, -1, 2, 2, -1, -1
		}, 6, new short[14]
		{
			9, 9, 12, 12, 12, -1, -1, 9, -1, 2,
			-1, 12, -1, -1
		}, new short[6] { -1, 12, 2, -1, 9, 12 }, new List<sbyte> { 5, 64 }, 3, new ResourceInts(default(int), default(int), default(int), default(int), default(int), default(int), default(int), default(int)), new IntPair[18]
		{
			new IntPair(17, 1),
			new IntPair(8, 1),
			new IntPair(5, 1),
			new IntPair(6, 1),
			new IntPair(15, 1),
			new IntPair(16, 3),
			new IntPair(7, 3),
			new IntPair(14, 1),
			new IntPair(12, 1),
			new IntPair(4, 9),
			new IntPair(3, 6),
			new IntPair(13, 42),
			new IntPair(10, 12),
			new IntPair(2, 12),
			new IntPair(1, 9),
			new IntPair(11, 1),
			new IntPair(0, 21),
			new IntPair(9, 1)
		}, null));
		_dataArray.Add(new OrganizationMemberItem(149, 705, 12, 4, 6, 9, 3, restrictPrincipalAmount: false, -1, -1, -1, 2, 4, 6, 4, 0, 0, new int[2] { 706, 707 }, 2800, 8, 4500, 4650, 0, new List<short> { 52, 53, 54 }, new List<short> { 18, 19, 20 }, new int[2] { 708, 709 }, canStroll: true, 320, new short[4] { 18, 22, 26, 30 }, new PresetEquipmentItemWithProb[8]
		{
			new PresetEquipmentItemWithProb("Armor", 27, 75),
			new PresetEquipmentItemWithProb("Armor", 297, 100),
			new PresetEquipmentItemWithProb("Armor", 423, 75),
			new PresetEquipmentItemWithProb("Armor", 153, 75),
			new PresetEquipmentItemWithProb("Accessory", 162, 75),
			new PresetEquipmentItemWithProb("Accessory", 135, 50),
			new PresetEquipmentItemWithProb("Accessory", 126, 50),
			new PresetEquipmentItemWithProb("Carrier", 9, 75)
		}, new PresetEquipmentItem("Clothing", 52), new List<PresetInventoryItem>
		{
			new PresetInventoryItem("SkillBook", 81, 3, 30),
			new PresetInventoryItem("Material", 0, 1, 20),
			new PresetInventoryItem("Material", 7, 1, 20),
			new PresetInventoryItem("CraftTool", 45, 1, 30),
			new PresetInventoryItem("Food", 0, 3, 30),
			new PresetInventoryItem("Food", 9, 3, 30),
			new PresetInventoryItem("Medicine", 0, 1, 20),
			new PresetInventoryItem("Medicine", 9, 1, 20),
			new PresetInventoryItem("Medicine", 18, 1, 20),
			new PresetInventoryItem("Medicine", 27, 1, 20),
			new PresetInventoryItem("Medicine", 36, 1, 20),
			new PresetInventoryItem("Medicine", 45, 1, 20),
			new PresetInventoryItem("Medicine", 54, 1, 20),
			new PresetInventoryItem("Medicine", 66, 1, 20),
			new PresetInventoryItem("Medicine", 82, 1, 20),
			new PresetInventoryItem("Medicine", 136, 1, 20),
			new PresetInventoryItem("Medicine", 148, 1, 20),
			new PresetInventoryItem("Medicine", 160, 1, 20),
			new PresetInventoryItem("Medicine", 172, 1, 20),
			new PresetInventoryItem("Medicine", 184, 1, 20),
			new PresetInventoryItem("Medicine", 196, 1, 20),
			new PresetInventoryItem("Medicine", 250, 1, 10),
			new PresetInventoryItem("Medicine", 322, 1, 10),
			new PresetInventoryItem("TeaWine", 0, 1, 20),
			new PresetInventoryItem("TeaWine", 9, 1, 20),
			new PresetInventoryItem("Material", 236, 1, 10),
			new PresetInventoryItem("Material", 243, 1, 10),
			new PresetInventoryItem("Material", 250, 1, 10),
			new PresetInventoryItem("Material", 257, 1, 10),
			new PresetInventoryItem("Material", 264, 1, 10),
			new PresetInventoryItem("Material", 271, 1, 10)
		}, new List<PresetOrgMemberCombatSkill>
		{
			new PresetOrgMemberCombatSkill(75, 4),
			new PresetOrgMemberCombatSkill(174, 4),
			new PresetOrgMemberCombatSkill(288, 4),
			new PresetOrgMemberCombatSkill(444, 4),
			new PresetOrgMemberCombatSkill(566, 4),
			new PresetOrgMemberCombatSkill(681, 4)
		}, new sbyte[5] { 4, 4, 4, 4, 4 }, new short[8] { -70, -50, -70, -80, -70, -70, -70, -70 }, 22500, 18000, 10, 60, 9300, new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, 9, 9, 12,
			-1, -1, 2, 2, -1, -1
		}, 5, new short[14]
		{
			9, 9, 12, 12, 12, -1, -1, 9, -1, 2,
			-1, 12, -1, -1
		}, new short[6] { -1, 12, 2, -1, 9, 12 }, new List<sbyte> { 5, 64 }, 3, new ResourceInts(default(int), default(int), default(int), default(int), default(int), default(int), default(int), default(int)), new IntPair[18]
		{
			new IntPair(17, 1),
			new IntPair(8, 1),
			new IntPair(5, 1),
			new IntPair(6, 1),
			new IntPair(15, 1),
			new IntPair(16, 3),
			new IntPair(7, 3),
			new IntPair(14, 1),
			new IntPair(12, 1),
			new IntPair(4, 9),
			new IntPair(3, 6),
			new IntPair(13, 42),
			new IntPair(10, 12),
			new IntPair(2, 12),
			new IntPair(1, 9),
			new IntPair(11, 1),
			new IntPair(0, 21),
			new IntPair(9, 1)
		}, null));
		_dataArray.Add(new OrganizationMemberItem(150, 710, 12, 3, 6, 9, 3, restrictPrincipalAmount: false, -1, -1, -1, 1, 3, 5, 3, 0, 0, new int[2] { 711, 712 }, 1800, 6, 3000, 2250, 0, new List<short> { 52, 53, 54 }, new List<short> { 18, 19, 20 }, new int[2] { 713, 714 }, canStroll: true, 321, new short[4] { 16, 19, 22, 25 }, new PresetEquipmentItemWithProb[8]
		{
			new PresetEquipmentItemWithProb("Armor", 27, 75),
			new PresetEquipmentItemWithProb("Armor", 297, 100),
			new PresetEquipmentItemWithProb("Armor", 423, 75),
			new PresetEquipmentItemWithProb("Armor", 153, 75),
			new PresetEquipmentItemWithProb("Accessory", 162, 75),
			new PresetEquipmentItemWithProb("Accessory", 135, 50),
			new PresetEquipmentItemWithProb("Accessory", 126, 50),
			new PresetEquipmentItemWithProb("Carrier", 9, 75)
		}, new PresetEquipmentItem("Clothing", 52), new List<PresetInventoryItem>
		{
			new PresetInventoryItem("SkillBook", 81, 3, 30),
			new PresetInventoryItem("Material", 0, 1, 20),
			new PresetInventoryItem("Material", 7, 1, 20),
			new PresetInventoryItem("CraftTool", 45, 1, 30),
			new PresetInventoryItem("Food", 0, 3, 30),
			new PresetInventoryItem("Food", 9, 3, 30),
			new PresetInventoryItem("Medicine", 0, 1, 20),
			new PresetInventoryItem("Medicine", 9, 1, 20),
			new PresetInventoryItem("Medicine", 18, 1, 20),
			new PresetInventoryItem("Medicine", 27, 1, 20),
			new PresetInventoryItem("Medicine", 36, 1, 20),
			new PresetInventoryItem("Medicine", 45, 1, 20),
			new PresetInventoryItem("Medicine", 54, 1, 20),
			new PresetInventoryItem("Medicine", 66, 1, 20),
			new PresetInventoryItem("Medicine", 82, 1, 20),
			new PresetInventoryItem("Medicine", 136, 1, 20),
			new PresetInventoryItem("Medicine", 148, 1, 20),
			new PresetInventoryItem("Medicine", 160, 1, 20),
			new PresetInventoryItem("Medicine", 172, 1, 20),
			new PresetInventoryItem("Medicine", 184, 1, 20),
			new PresetInventoryItem("Medicine", 196, 1, 20),
			new PresetInventoryItem("Medicine", 250, 1, 10),
			new PresetInventoryItem("Medicine", 322, 1, 10),
			new PresetInventoryItem("TeaWine", 0, 1, 20),
			new PresetInventoryItem("TeaWine", 9, 1, 20),
			new PresetInventoryItem("Material", 236, 1, 10),
			new PresetInventoryItem("Material", 243, 1, 10),
			new PresetInventoryItem("Material", 250, 1, 10),
			new PresetInventoryItem("Material", 257, 1, 10),
			new PresetInventoryItem("Material", 264, 1, 10),
			new PresetInventoryItem("Material", 271, 1, 10)
		}, new List<PresetOrgMemberCombatSkill>
		{
			new PresetOrgMemberCombatSkill(75, 3),
			new PresetOrgMemberCombatSkill(174, 3),
			new PresetOrgMemberCombatSkill(288, 3),
			new PresetOrgMemberCombatSkill(375, 3),
			new PresetOrgMemberCombatSkill(566, 3),
			new PresetOrgMemberCombatSkill(681, 3)
		}, new sbyte[5] { 4, 4, 4, 4, 4 }, new short[8] { -70, -50, -70, -80, -70, -70, -70, -70 }, 15000, 9000, 10, 70, 4500, new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, 9, 9, 12,
			-1, -1, 2, 2, -1, -1
		}, 4, new short[14]
		{
			9, 9, 12, 12, 12, -1, -1, 9, -1, 2,
			-1, 12, -1, -1
		}, new short[6] { -1, 12, 2, -1, 9, 12 }, new List<sbyte> { 5, 64 }, 3, new ResourceInts(default(int), default(int), default(int), default(int), default(int), default(int), default(int), default(int)), new IntPair[18]
		{
			new IntPair(17, 1),
			new IntPair(8, 1),
			new IntPair(5, 1),
			new IntPair(6, 1),
			new IntPair(15, 1),
			new IntPair(16, 3),
			new IntPair(7, 3),
			new IntPair(14, 1),
			new IntPair(12, 1),
			new IntPair(4, 9),
			new IntPair(3, 6),
			new IntPair(13, 42),
			new IntPair(10, 12),
			new IntPair(2, 12),
			new IntPair(1, 9),
			new IntPair(11, 1),
			new IntPair(0, 21),
			new IntPair(9, 1)
		}, null));
		_dataArray.Add(new OrganizationMemberItem(151, 715, 12, 2, 8, 12, 4, restrictPrincipalAmount: false, -1, -1, -1, 0, 2, 4, 2, 0, 0, new int[2] { 716, 717 }, 600, 4, 2000, 900, 0, new List<short> { 52, 53, 54 }, new List<short> { 18, 19, 20 }, new int[2] { 718, 719 }, canStroll: true, 322, new short[4] { 14, 16, 18, 20 }, new PresetEquipmentItemWithProb[8]
		{
			new PresetEquipmentItemWithProb("Armor", 27, 75),
			new PresetEquipmentItemWithProb("Armor", 297, 100),
			new PresetEquipmentItemWithProb("Armor", 423, 75),
			new PresetEquipmentItemWithProb("Armor", 153, 75),
			new PresetEquipmentItemWithProb("Accessory", 162, 75),
			new PresetEquipmentItemWithProb("Accessory", 135, 50),
			new PresetEquipmentItemWithProb("Accessory", 126, 50),
			new PresetEquipmentItemWithProb("Carrier", 9, 75)
		}, new PresetEquipmentItem("Clothing", 52), new List<PresetInventoryItem>
		{
			new PresetInventoryItem("SkillBook", 81, 3, 20),
			new PresetInventoryItem("Material", 0, 1, 10),
			new PresetInventoryItem("Material", 7, 1, 10),
			new PresetInventoryItem("CraftTool", 45, 1, 20),
			new PresetInventoryItem("Food", 0, 3, 20),
			new PresetInventoryItem("Food", 9, 3, 20),
			new PresetInventoryItem("Medicine", 0, 1, 10),
			new PresetInventoryItem("Medicine", 9, 1, 10),
			new PresetInventoryItem("Medicine", 18, 1, 10),
			new PresetInventoryItem("Medicine", 27, 1, 10),
			new PresetInventoryItem("Medicine", 36, 1, 10),
			new PresetInventoryItem("Medicine", 45, 1, 10),
			new PresetInventoryItem("Medicine", 130, 1, 10),
			new PresetInventoryItem("Medicine", 142, 1, 10),
			new PresetInventoryItem("Medicine", 154, 1, 10),
			new PresetInventoryItem("Medicine", 166, 1, 10),
			new PresetInventoryItem("Medicine", 178, 1, 10),
			new PresetInventoryItem("Medicine", 190, 1, 10),
			new PresetInventoryItem("Material", 236, 1, 10),
			new PresetInventoryItem("Material", 243, 1, 10),
			new PresetInventoryItem("Material", 250, 1, 10),
			new PresetInventoryItem("Material", 257, 1, 10),
			new PresetInventoryItem("Material", 264, 1, 10),
			new PresetInventoryItem("Material", 271, 1, 10)
		}, new List<PresetOrgMemberCombatSkill>
		{
			new PresetOrgMemberCombatSkill(75, 2),
			new PresetOrgMemberCombatSkill(174, 2),
			new PresetOrgMemberCombatSkill(288, 2),
			new PresetOrgMemberCombatSkill(444, 2),
			new PresetOrgMemberCombatSkill(566, 2),
			new PresetOrgMemberCombatSkill(681, 2)
		}, new sbyte[5] { 2, 2, 2, 2, 2 }, new short[8] { -70, -50, -70, -80, -70, -70, -70, -70 }, 11250, 3000, 5, 80, 1800, new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, 9, 9, 12,
			-1, -1, 2, 2, -1, -1
		}, 3, new short[14]
		{
			9, 9, 12, 12, 12, -1, -1, 9, -1, 2,
			-1, 12, -1, -1
		}, new short[6] { -1, 12, 2, -1, 9, 12 }, new List<sbyte> { 5, 64 }, 3, new ResourceInts(default(int), default(int), default(int), default(int), default(int), default(int), default(int), default(int)), new IntPair[18]
		{
			new IntPair(17, 1),
			new IntPair(8, 1),
			new IntPair(5, 1),
			new IntPair(6, 1),
			new IntPair(15, 1),
			new IntPair(16, 3),
			new IntPair(7, 3),
			new IntPair(14, 1),
			new IntPair(12, 1),
			new IntPair(4, 3),
			new IntPair(3, 6),
			new IntPair(13, 30),
			new IntPair(10, 12),
			new IntPair(2, 12),
			new IntPair(1, 9),
			new IntPair(11, 1),
			new IntPair(0, 21),
			new IntPair(9, 1)
		}, null));
		_dataArray.Add(new OrganizationMemberItem(152, 720, 12, 1, 8, 12, 4, restrictPrincipalAmount: false, -1, -1, -1, 0, 1, 3, 1, 0, 0, new int[2] { 721, 722 }, 300, 2, 1000, 300, 0, new List<short> { 52, 53, 54 }, new List<short> { 18, 19, 20 }, new int[2] { 723, 724 }, canStroll: true, 323, new short[4] { 12, 13, 14, 15 }, new PresetEquipmentItemWithProb[8]
		{
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Armor", 297, 100),
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Armor", 153, 75),
			new PresetEquipmentItemWithProb("Accessory", 162, 75),
			new PresetEquipmentItemWithProb("Accessory", 135, 50),
			new PresetEquipmentItemWithProb("Accessory", -1, 0),
			new PresetEquipmentItemWithProb("Carrier", -1, 0)
		}, new PresetEquipmentItem("Clothing", 52), new List<PresetInventoryItem>
		{
			new PresetInventoryItem("SkillBook", 81, 3, 20),
			new PresetInventoryItem("Material", 0, 1, 10),
			new PresetInventoryItem("Material", 7, 1, 10),
			new PresetInventoryItem("CraftTool", 45, 1, 20),
			new PresetInventoryItem("Food", 0, 3, 20),
			new PresetInventoryItem("Food", 9, 3, 20),
			new PresetInventoryItem("Medicine", 0, 1, 10),
			new PresetInventoryItem("Medicine", 9, 1, 10),
			new PresetInventoryItem("Medicine", 18, 1, 10),
			new PresetInventoryItem("Medicine", 27, 1, 10),
			new PresetInventoryItem("Medicine", 36, 1, 10),
			new PresetInventoryItem("Medicine", 45, 1, 10),
			new PresetInventoryItem("Medicine", 130, 1, 10),
			new PresetInventoryItem("Medicine", 142, 1, 10),
			new PresetInventoryItem("Medicine", 154, 1, 10),
			new PresetInventoryItem("Medicine", 166, 1, 10),
			new PresetInventoryItem("Medicine", 178, 1, 10),
			new PresetInventoryItem("Medicine", 190, 1, 10),
			new PresetInventoryItem("Material", 236, 1, 10),
			new PresetInventoryItem("Material", 243, 1, 10),
			new PresetInventoryItem("Material", 250, 1, 10),
			new PresetInventoryItem("Material", 257, 1, 10),
			new PresetInventoryItem("Material", 264, 1, 10),
			new PresetInventoryItem("Material", 271, 1, 10)
		}, new List<PresetOrgMemberCombatSkill>
		{
			new PresetOrgMemberCombatSkill(75, 1),
			new PresetOrgMemberCombatSkill(174, 1),
			new PresetOrgMemberCombatSkill(288, 1),
			new PresetOrgMemberCombatSkill(375, 1),
			new PresetOrgMemberCombatSkill(444, 1),
			new PresetOrgMemberCombatSkill(681, 1)
		}, new sbyte[5], new short[8] { -70, -50, -70, -80, -70, -70, -70, -70 }, 7500, 1500, 5, 90, 600, new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, 9, 9, 12,
			-1, -1, 2, 2, -1, -1
		}, 2, new short[14]
		{
			9, 9, 12, 12, 12, -1, -1, 9, -1, 2,
			-1, 12, -1, -1
		}, new short[6] { -1, 12, 2, -1, 9, 12 }, new List<sbyte> { 5, 64 }, 3, new ResourceInts(default(int), default(int), default(int), default(int), default(int), default(int), default(int), default(int)), new IntPair[18]
		{
			new IntPair(17, 1),
			new IntPair(8, 1),
			new IntPair(5, 1),
			new IntPair(6, 1),
			new IntPair(15, 1),
			new IntPair(16, 3),
			new IntPair(7, 3),
			new IntPair(14, 1),
			new IntPair(12, 1),
			new IntPair(4, 3),
			new IntPair(3, 6),
			new IntPair(13, 30),
			new IntPair(10, 12),
			new IntPair(2, 12),
			new IntPair(1, 9),
			new IntPair(11, 1),
			new IntPair(0, 21),
			new IntPair(9, 1)
		}, null));
		_dataArray.Add(new OrganizationMemberItem(153, 725, 12, 0, 8, 12, 4, restrictPrincipalAmount: false, -1, -1, -1, 0, 0, 3, 0, 0, 0, new int[2] { 726, 727 }, 150, 0, 500, 150, 0, new List<short> { 52, 53, 54 }, new List<short> { 18, 19, 20 }, new int[2] { 728, 729 }, canStroll: true, 324, new short[4] { 10, 10, 10, 10 }, new PresetEquipmentItemWithProb[8]
		{
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Armor", 297, 100),
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Armor", 153, 75),
			new PresetEquipmentItemWithProb("Accessory", 162, 75),
			new PresetEquipmentItemWithProb("Accessory", -1, 0),
			new PresetEquipmentItemWithProb("Accessory", -1, 0),
			new PresetEquipmentItemWithProb("Carrier", -1, 0)
		}, new PresetEquipmentItem("Clothing", 52), new List<PresetInventoryItem>
		{
			new PresetInventoryItem("SkillBook", 81, 3, 20),
			new PresetInventoryItem("Material", 0, 1, 10),
			new PresetInventoryItem("Material", 7, 1, 10),
			new PresetInventoryItem("CraftTool", 45, 1, 20),
			new PresetInventoryItem("Food", 0, 3, 20),
			new PresetInventoryItem("Food", 9, 3, 20),
			new PresetInventoryItem("Medicine", 0, 1, 10),
			new PresetInventoryItem("Medicine", 9, 1, 10),
			new PresetInventoryItem("Medicine", 18, 1, 10),
			new PresetInventoryItem("Medicine", 27, 1, 10),
			new PresetInventoryItem("Medicine", 36, 1, 10),
			new PresetInventoryItem("Medicine", 45, 1, 10),
			new PresetInventoryItem("Medicine", 130, 1, 10),
			new PresetInventoryItem("Medicine", 142, 1, 10),
			new PresetInventoryItem("Medicine", 154, 1, 10),
			new PresetInventoryItem("Medicine", 166, 1, 10),
			new PresetInventoryItem("Medicine", 178, 1, 10),
			new PresetInventoryItem("Medicine", 190, 1, 10),
			new PresetInventoryItem("Material", 236, 1, 10),
			new PresetInventoryItem("Material", 243, 1, 10),
			new PresetInventoryItem("Material", 250, 1, 10),
			new PresetInventoryItem("Material", 257, 1, 10),
			new PresetInventoryItem("Material", 264, 1, 10),
			new PresetInventoryItem("Material", 271, 1, 10)
		}, new List<PresetOrgMemberCombatSkill>
		{
			new PresetOrgMemberCombatSkill(75, 0),
			new PresetOrgMemberCombatSkill(174, 0),
			new PresetOrgMemberCombatSkill(444, 0)
		}, new sbyte[5], new short[8] { -70, -50, -70, -80, -70, -70, -70, -70 }, 3750, 750, 1, 100, 300, new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, 9, 9, 12,
			-1, -1, 2, 2, -1, -1
		}, 1, new short[14]
		{
			9, 9, 12, 12, 12, -1, -1, 9, -1, 2,
			-1, 12, -1, -1
		}, new short[6] { -1, 12, 2, -1, 9, 12 }, new List<sbyte> { 5, 64 }, 3, new ResourceInts(default(int), default(int), default(int), default(int), default(int), default(int), default(int), default(int)), new IntPair[18]
		{
			new IntPair(17, 1),
			new IntPair(8, 1),
			new IntPair(5, 1),
			new IntPair(6, 1),
			new IntPair(15, 1),
			new IntPair(16, 3),
			new IntPair(7, 3),
			new IntPair(14, 1),
			new IntPair(12, 1),
			new IntPair(4, 3),
			new IntPair(3, 6),
			new IntPair(13, 30),
			new IntPair(10, 12),
			new IntPair(2, 12),
			new IntPair(1, 9),
			new IntPair(11, 1),
			new IntPair(0, 21),
			new IntPair(9, 1)
		}, null));
		_dataArray.Add(new OrganizationMemberItem(154, 730, 13, 8, 1, 1, 1, restrictPrincipalAmount: true, -1, -1, 7, 4, 6, -1, 7, 0, 0, new int[2] { 731, 732 }, 15800, 16, 13000, 30750, 0, new List<short> { 55, 56, 57 }, new List<short> { 31, 32, 33 }, new int[2] { 733, 734 }, canStroll: false, 325, new short[4] { 25, 30, 35, 40 }, new PresetEquipmentItemWithProb[8]
		{
			new PresetEquipmentItemWithProb("Armor", 36, 75),
			new PresetEquipmentItemWithProb("Armor", 324, 100),
			new PresetEquipmentItemWithProb("Armor", 477, 75),
			new PresetEquipmentItemWithProb("Armor", 162, 75),
			new PresetEquipmentItemWithProb("Accessory", 117, 75),
			new PresetEquipmentItemWithProb("Accessory", 135, 50),
			new PresetEquipmentItemWithProb("Accessory", 72, 50),
			new PresetEquipmentItemWithProb("Carrier", 18, 75)
		}, new PresetEquipmentItem("Clothing", 57), new List<PresetInventoryItem>
		{
			new PresetInventoryItem("SkillBook", 9, 1, 40),
			new PresetInventoryItem("SkillBook", 36, 1, 40),
			new PresetInventoryItem("Food", 93, 3, 40),
			new PresetInventoryItem("Medicine", 9, 1, 20),
			new PresetInventoryItem("Medicine", 18, 1, 20),
			new PresetInventoryItem("Medicine", 45, 1, 20),
			new PresetInventoryItem("Medicine", 54, 1, 20),
			new PresetInventoryItem("Medicine", 66, 1, 20),
			new PresetInventoryItem("Medicine", 256, 1, 20),
			new PresetInventoryItem("Medicine", 328, 1, 20),
			new PresetInventoryItem("Medicine", 292, 1, 30),
			new PresetInventoryItem("Medicine", 316, 1, 30)
		}, new List<PresetOrgMemberCombatSkill>
		{
			new PresetOrgMemberCombatSkill(82, 6),
			new PresetOrgMemberCombatSkill(181, 8),
			new PresetOrgMemberCombatSkill(297, 6),
			new PresetOrgMemberCombatSkill(453, 8),
			new PresetOrgMemberCombatSkill(503, 8),
			new PresetOrgMemberCombatSkill(573, 7)
		}, new sbyte[5] { 10, 10, 10, 10, 10 }, new short[8] { -70, -70, -70, -70, -60, -60, -70, -70 }, 160000, 576000, 20, 20, 61500, new short[16]
		{
			-1, 12, -1, -1, 12, 2, -1, -1, 2, 9,
			-1, -1, -1, -1, -1, -1
		}, 8, new short[14]
		{
			6, 12, 6, -1, 12, -1, 12, 12, -1, -1,
			-1, -1, -1, -1
		}, new short[6] { -1, 12, -1, 2, 9, 12 }, new List<sbyte> { 21, 24, 25, 55 }, 3, new ResourceInts(default(int), default(int), default(int), default(int), default(int), default(int), default(int), default(int)), new IntPair[18]
		{
			new IntPair(17, 1),
			new IntPair(8, 1),
			new IntPair(5, 9),
			new IntPair(6, 1),
			new IntPair(15, 3),
			new IntPair(16, 1),
			new IntPair(7, 1),
			new IntPair(14, 54),
			new IntPair(12, 1),
			new IntPair(4, 21),
			new IntPair(3, 15),
			new IntPair(13, 12),
			new IntPair(10, 3),
			new IntPair(2, 3),
			new IntPair(1, 3),
			new IntPair(11, 6),
			new IntPair(0, 3),
			new IntPair(9, 6)
		}, null));
		_dataArray.Add(new OrganizationMemberItem(155, 735, 13, 7, 4, 6, 2, restrictPrincipalAmount: false, -1, -1, -1, 4, 6, 8, 7, 0, 0, new int[2] { 736, 737 }, 8600, 14, 10000, 21150, 0, new List<short> { 55, 56, 57 }, new List<short> { 31, 32, 33 }, new int[2] { 738, 739 }, canStroll: false, 326, new short[4] { 22, 26, 30, 34 }, new PresetEquipmentItemWithProb[8]
		{
			new PresetEquipmentItemWithProb("Armor", 36, 75),
			new PresetEquipmentItemWithProb("Armor", 324, 100),
			new PresetEquipmentItemWithProb("Armor", 477, 75),
			new PresetEquipmentItemWithProb("Armor", 162, 75),
			new PresetEquipmentItemWithProb("Accessory", 117, 75),
			new PresetEquipmentItemWithProb("Accessory", 135, 50),
			new PresetEquipmentItemWithProb("Accessory", 72, 50),
			new PresetEquipmentItemWithProb("Carrier", 18, 75)
		}, new PresetEquipmentItem("Clothing", 56), new List<PresetInventoryItem>
		{
			new PresetInventoryItem("SkillBook", 9, 1, 40),
			new PresetInventoryItem("SkillBook", 36, 1, 40),
			new PresetInventoryItem("Food", 93, 3, 40),
			new PresetInventoryItem("Medicine", 9, 1, 20),
			new PresetInventoryItem("Medicine", 18, 1, 20),
			new PresetInventoryItem("Medicine", 45, 1, 20),
			new PresetInventoryItem("Medicine", 54, 1, 20),
			new PresetInventoryItem("Medicine", 66, 1, 20),
			new PresetInventoryItem("Medicine", 256, 1, 20),
			new PresetInventoryItem("Medicine", 328, 1, 20),
			new PresetInventoryItem("Medicine", 292, 1, 30),
			new PresetInventoryItem("Medicine", 316, 1, 30)
		}, new List<PresetOrgMemberCombatSkill>
		{
			new PresetOrgMemberCombatSkill(82, 6),
			new PresetOrgMemberCombatSkill(181, 7),
			new PresetOrgMemberCombatSkill(297, 6),
			new PresetOrgMemberCombatSkill(453, 7),
			new PresetOrgMemberCombatSkill(503, 7),
			new PresetOrgMemberCombatSkill(573, 6)
		}, new sbyte[5] { 8, 8, 8, 8, 8 }, new short[8] { -70, -70, -70, -70, -60, -60, -70, -70 }, 100000, 288000, 15, 30, 42300, new short[16]
		{
			-1, 12, -1, -1, 12, 2, -1, -1, 2, 9,
			-1, -1, -1, -1, -1, -1
		}, 8, new short[14]
		{
			6, 12, 6, -1, 12, -1, 12, 12, -1, -1,
			-1, -1, -1, -1
		}, new short[6] { -1, 12, -1, 2, 9, 12 }, new List<sbyte> { 21, 24, 25 }, 3, new ResourceInts(default(int), default(int), default(int), default(int), default(int), default(int), default(int), default(int)), new IntPair[18]
		{
			new IntPair(17, 1),
			new IntPair(8, 1),
			new IntPair(5, 9),
			new IntPair(6, 1),
			new IntPair(15, 3),
			new IntPair(16, 1),
			new IntPair(7, 1),
			new IntPair(14, 54),
			new IntPair(12, 1),
			new IntPair(4, 21),
			new IntPair(3, 15),
			new IntPair(13, 12),
			new IntPair(10, 3),
			new IntPair(2, 3),
			new IntPair(1, 3),
			new IntPair(11, 6),
			new IntPair(0, 3),
			new IntPair(9, 6)
		}, null));
		_dataArray.Add(new OrganizationMemberItem(156, 740, 13, 6, 8, 12, 4, restrictPrincipalAmount: false, -1, -1, -1, 3, 6, 7, 6, 0, 0, new int[2] { 741, 742 }, 6100, 12, 8000, 13800, 0, new List<short> { 55, 56, 57 }, new List<short> { 31, 32, 33 }, new int[2] { 743, 744 }, canStroll: true, 327, new short[4] { 19, 22, 25, 28 }, new PresetEquipmentItemWithProb[8]
		{
			new PresetEquipmentItemWithProb("Armor", 36, 75),
			new PresetEquipmentItemWithProb("Armor", 324, 100),
			new PresetEquipmentItemWithProb("Armor", 477, 75),
			new PresetEquipmentItemWithProb("Armor", 162, 75),
			new PresetEquipmentItemWithProb("Accessory", 117, 75),
			new PresetEquipmentItemWithProb("Accessory", 135, 50),
			new PresetEquipmentItemWithProb("Accessory", 72, 50),
			new PresetEquipmentItemWithProb("Carrier", 18, 75)
		}, new PresetEquipmentItem("Clothing", 56), new List<PresetInventoryItem>
		{
			new PresetInventoryItem("SkillBook", 9, 1, 40),
			new PresetInventoryItem("SkillBook", 36, 1, 40),
			new PresetInventoryItem("Food", 93, 3, 40),
			new PresetInventoryItem("Medicine", 9, 1, 20),
			new PresetInventoryItem("Medicine", 18, 1, 20),
			new PresetInventoryItem("Medicine", 45, 1, 20),
			new PresetInventoryItem("Medicine", 54, 1, 20),
			new PresetInventoryItem("Medicine", 66, 1, 20),
			new PresetInventoryItem("Medicine", 256, 1, 20),
			new PresetInventoryItem("Medicine", 328, 1, 20),
			new PresetInventoryItem("Medicine", 292, 1, 30),
			new PresetInventoryItem("Medicine", 316, 1, 30)
		}, new List<PresetOrgMemberCombatSkill>
		{
			new PresetOrgMemberCombatSkill(82, 5),
			new PresetOrgMemberCombatSkill(181, 6),
			new PresetOrgMemberCombatSkill(297, 5),
			new PresetOrgMemberCombatSkill(453, 6),
			new PresetOrgMemberCombatSkill(503, 6),
			new PresetOrgMemberCombatSkill(573, 6)
		}, new sbyte[5] { 8, 8, 8, 8, 8 }, new short[8] { -70, -70, -70, -70, -60, -60, -70, -70 }, 60000, 144000, 15, 40, 27600, new short[16]
		{
			-1, 12, -1, -1, 12, 2, -1, -1, 2, 9,
			-1, -1, -1, -1, -1, -1
		}, 7, new short[14]
		{
			6, 12, 6, -1, 12, -1, 12, 12, -1, -1,
			-1, -1, -1, -1
		}, new short[6] { -1, 12, -1, 2, 9, 12 }, new List<sbyte> { 21, 24, 25 }, 3, new ResourceInts(default(int), default(int), default(int), default(int), default(int), default(int), default(int), default(int)), new IntPair[18]
		{
			new IntPair(17, 1),
			new IntPair(8, 1),
			new IntPair(5, 9),
			new IntPair(6, 1),
			new IntPair(15, 3),
			new IntPair(16, 1),
			new IntPair(7, 1),
			new IntPair(14, 54),
			new IntPair(12, 1),
			new IntPair(4, 21),
			new IntPair(3, 15),
			new IntPair(13, 12),
			new IntPair(10, 3),
			new IntPair(2, 3),
			new IntPair(1, 3),
			new IntPair(11, 6),
			new IntPair(0, 3),
			new IntPair(9, 6)
		}, null));
		List<OrganizationMemberItem> dataArray7 = _dataArray;
		int[] monasticTitleSuffixes7 = new int[2] { 746, 747 };
		List<short> favoriteClothingIds7 = new List<short> { 55, 56, 57 };
		List<short> hatedClothingIds7 = new List<short> { 31, 32, 33 };
		int[] spouseAnonymousTitles7 = new int[2] { 748, 749 };
		short[] initialAges7 = new short[4] { 25, 30, 35, 40 };
		PresetEquipmentItemWithProb[] equipment7 = new PresetEquipmentItemWithProb[8]
		{
			new PresetEquipmentItemWithProb("Armor", 36, 75),
			new PresetEquipmentItemWithProb("Armor", 324, 100),
			new PresetEquipmentItemWithProb("Armor", 477, 75),
			new PresetEquipmentItemWithProb("Armor", 162, 75),
			new PresetEquipmentItemWithProb("Accessory", 117, 75),
			new PresetEquipmentItemWithProb("Accessory", 135, 50),
			new PresetEquipmentItemWithProb("Accessory", 72, 50),
			new PresetEquipmentItemWithProb("Carrier", 18, 75)
		};
		PresetEquipmentItem clothing7 = new PresetEquipmentItem("Clothing", 56);
		List<PresetInventoryItem> inventory7 = new List<PresetInventoryItem>
		{
			new PresetInventoryItem("SkillBook", 9, 1, 30),
			new PresetInventoryItem("SkillBook", 36, 1, 30),
			new PresetInventoryItem("Food", 93, 3, 30),
			new PresetInventoryItem("Medicine", 9, 1, 20),
			new PresetInventoryItem("Medicine", 18, 1, 20),
			new PresetInventoryItem("Medicine", 45, 1, 20),
			new PresetInventoryItem("Medicine", 54, 1, 20),
			new PresetInventoryItem("Medicine", 66, 1, 20),
			new PresetInventoryItem("Medicine", 250, 1, 10),
			new PresetInventoryItem("Medicine", 322, 1, 10),
			new PresetInventoryItem("Medicine", 286, 1, 30),
			new PresetInventoryItem("Medicine", 310, 1, 30)
		};
		List<PresetOrgMemberCombatSkill> combatSkills7 = new List<PresetOrgMemberCombatSkill>
		{
			new PresetOrgMemberCombatSkill(82, 5),
			new PresetOrgMemberCombatSkill(181, 5),
			new PresetOrgMemberCombatSkill(297, 5),
			new PresetOrgMemberCombatSkill(503, 5),
			new PresetOrgMemberCombatSkill(573, 5)
		};
		sbyte[] extraCombatSkillGrids7 = new sbyte[5] { 6, 6, 6, 6, 6 };
		short[] resourcesAdjust7 = new short[8] { -70, -70, -70, -70, -60, -60, -70, -70 };
		short[] lifeSkillsAdjust7 = new short[16]
		{
			-1, 12, -1, -1, 12, 2, -1, -1, 2, 9,
			-1, -1, -1, -1, -1, -1
		};
		short[] combatSkillsAdjust7 = new short[14]
		{
			6, 12, 6, -1, 12, -1, 12, 12, -1, -1,
			-1, -1, -1, -1
		};
		short[] mainAttributesAdjust7 = new short[6] { -1, 12, -1, 2, 9, 12 };
		identityInteractConfig = new List<sbyte>();
		dataArray7.Add(new OrganizationMemberItem(157, 745, 13, 5, 6, 9, 3, restrictPrincipalAmount: false, -1, -1, -1, 3, 5, 7, 5, 0, 0, monasticTitleSuffixes7, 4200, 10, 6000, 8400, 0, favoriteClothingIds7, hatedClothingIds7, spouseAnonymousTitles7, canStroll: true, 328, initialAges7, equipment7, clothing7, inventory7, combatSkills7, extraCombatSkillGrids7, resourcesAdjust7, 40000, 48000, 10, 50, 16800, lifeSkillsAdjust7, 6, combatSkillsAdjust7, mainAttributesAdjust7, identityInteractConfig, 3, new ResourceInts(default(int), default(int), default(int), default(int), default(int), default(int), default(int), default(int)), new IntPair[18]
		{
			new IntPair(17, 1),
			new IntPair(8, 1),
			new IntPair(5, 6),
			new IntPair(6, 1),
			new IntPair(15, 3),
			new IntPair(16, 1),
			new IntPair(7, 1),
			new IntPair(14, 42),
			new IntPair(12, 1),
			new IntPair(4, 18),
			new IntPair(3, 18),
			new IntPair(13, 9),
			new IntPair(10, 3),
			new IntPair(2, 3),
			new IntPair(1, 3),
			new IntPair(11, 6),
			new IntPair(0, 3),
			new IntPair(9, 6)
		}, null));
		List<OrganizationMemberItem> dataArray8 = _dataArray;
		int[] monasticTitleSuffixes8 = new int[2] { 751, 752 };
		List<short> favoriteClothingIds8 = new List<short> { 55, 56, 57 };
		List<short> hatedClothingIds8 = new List<short> { 31, 32, 33 };
		int[] spouseAnonymousTitles8 = new int[2] { 753, 754 };
		short[] initialAges8 = new short[4] { 22, 26, 30, 34 };
		PresetEquipmentItemWithProb[] equipment8 = new PresetEquipmentItemWithProb[8]
		{
			new PresetEquipmentItemWithProb("Armor", 36, 75),
			new PresetEquipmentItemWithProb("Armor", 324, 100),
			new PresetEquipmentItemWithProb("Armor", 477, 75),
			new PresetEquipmentItemWithProb("Armor", 162, 75),
			new PresetEquipmentItemWithProb("Accessory", 117, 75),
			new PresetEquipmentItemWithProb("Accessory", 135, 50),
			new PresetEquipmentItemWithProb("Accessory", 72, 50),
			new PresetEquipmentItemWithProb("Carrier", 18, 75)
		};
		PresetEquipmentItem clothing8 = new PresetEquipmentItem("Clothing", 55);
		List<PresetInventoryItem> inventory8 = new List<PresetInventoryItem>
		{
			new PresetInventoryItem("SkillBook", 9, 1, 30),
			new PresetInventoryItem("SkillBook", 36, 1, 30),
			new PresetInventoryItem("Food", 93, 3, 30),
			new PresetInventoryItem("Medicine", 9, 1, 20),
			new PresetInventoryItem("Medicine", 18, 1, 20),
			new PresetInventoryItem("Medicine", 45, 1, 20),
			new PresetInventoryItem("Medicine", 54, 1, 20),
			new PresetInventoryItem("Medicine", 66, 1, 20),
			new PresetInventoryItem("Medicine", 250, 1, 10),
			new PresetInventoryItem("Medicine", 322, 1, 10),
			new PresetInventoryItem("Medicine", 286, 1, 30),
			new PresetInventoryItem("Medicine", 310, 1, 30)
		};
		List<PresetOrgMemberCombatSkill> combatSkills8 = new List<PresetOrgMemberCombatSkill>
		{
			new PresetOrgMemberCombatSkill(82, 4),
			new PresetOrgMemberCombatSkill(181, 4),
			new PresetOrgMemberCombatSkill(297, 4),
			new PresetOrgMemberCombatSkill(453, 4),
			new PresetOrgMemberCombatSkill(573, 4)
		};
		sbyte[] extraCombatSkillGrids8 = new sbyte[5] { 4, 4, 4, 4, 4 };
		short[] resourcesAdjust8 = new short[8] { -70, -70, -70, -70, -60, -60, -70, -70 };
		short[] lifeSkillsAdjust8 = new short[16]
		{
			-1, 12, -1, -1, 12, 2, -1, -1, 2, 9,
			-1, -1, -1, -1, -1, -1
		};
		short[] combatSkillsAdjust8 = new short[14]
		{
			6, 12, 6, -1, 12, -1, 12, 12, -1, -1,
			-1, -1, -1, -1
		};
		short[] mainAttributesAdjust8 = new short[6] { -1, 12, -1, 2, 9, 12 };
		identityInteractConfig = new List<sbyte>();
		dataArray8.Add(new OrganizationMemberItem(158, 750, 13, 4, 6, 9, 3, restrictPrincipalAmount: false, -1, -1, -1, 2, 4, 5, 4, 0, 0, monasticTitleSuffixes8, 2800, 8, 4500, 4650, 0, favoriteClothingIds8, hatedClothingIds8, spouseAnonymousTitles8, canStroll: true, 329, initialAges8, equipment8, clothing8, inventory8, combatSkills8, extraCombatSkillGrids8, resourcesAdjust8, 30000, 24000, 10, 60, 9300, lifeSkillsAdjust8, 5, combatSkillsAdjust8, mainAttributesAdjust8, identityInteractConfig, 3, new ResourceInts(default(int), default(int), default(int), default(int), default(int), default(int), default(int), default(int)), new IntPair[18]
		{
			new IntPair(17, 1),
			new IntPair(8, 1),
			new IntPair(5, 6),
			new IntPair(6, 1),
			new IntPair(15, 3),
			new IntPair(16, 1),
			new IntPair(7, 1),
			new IntPair(14, 42),
			new IntPair(12, 1),
			new IntPair(4, 18),
			new IntPair(3, 18),
			new IntPair(13, 9),
			new IntPair(10, 3),
			new IntPair(2, 3),
			new IntPair(1, 3),
			new IntPair(11, 6),
			new IntPair(0, 3),
			new IntPair(9, 6)
		}, null));
		List<OrganizationMemberItem> dataArray9 = _dataArray;
		int[] monasticTitleSuffixes9 = new int[2] { 756, 757 };
		List<short> favoriteClothingIds9 = new List<short> { 55, 56, 57 };
		List<short> hatedClothingIds9 = new List<short> { 31, 32, 33 };
		int[] spouseAnonymousTitles9 = new int[2] { 758, 759 };
		short[] initialAges9 = new short[4] { 19, 22, 25, 28 };
		PresetEquipmentItemWithProb[] equipment9 = new PresetEquipmentItemWithProb[8]
		{
			new PresetEquipmentItemWithProb("Armor", 36, 75),
			new PresetEquipmentItemWithProb("Armor", 324, 100),
			new PresetEquipmentItemWithProb("Armor", 477, 75),
			new PresetEquipmentItemWithProb("Armor", 162, 75),
			new PresetEquipmentItemWithProb("Accessory", 117, 75),
			new PresetEquipmentItemWithProb("Accessory", 135, 50),
			new PresetEquipmentItemWithProb("Accessory", 72, 50),
			new PresetEquipmentItemWithProb("Carrier", 18, 75)
		};
		PresetEquipmentItem clothing9 = new PresetEquipmentItem("Clothing", 55);
		List<PresetInventoryItem> inventory9 = new List<PresetInventoryItem>
		{
			new PresetInventoryItem("SkillBook", 9, 1, 30),
			new PresetInventoryItem("SkillBook", 36, 1, 30),
			new PresetInventoryItem("Food", 93, 3, 30),
			new PresetInventoryItem("Medicine", 9, 1, 20),
			new PresetInventoryItem("Medicine", 18, 1, 20),
			new PresetInventoryItem("Medicine", 45, 1, 20),
			new PresetInventoryItem("Medicine", 54, 1, 20),
			new PresetInventoryItem("Medicine", 66, 1, 20),
			new PresetInventoryItem("Medicine", 250, 1, 10),
			new PresetInventoryItem("Medicine", 322, 1, 10),
			new PresetInventoryItem("Medicine", 286, 1, 30),
			new PresetInventoryItem("Medicine", 310, 1, 30)
		};
		List<PresetOrgMemberCombatSkill> combatSkills9 = new List<PresetOrgMemberCombatSkill>
		{
			new PresetOrgMemberCombatSkill(82, 3),
			new PresetOrgMemberCombatSkill(181, 3),
			new PresetOrgMemberCombatSkill(297, 3),
			new PresetOrgMemberCombatSkill(453, 3),
			new PresetOrgMemberCombatSkill(503, 3),
			new PresetOrgMemberCombatSkill(573, 3)
		};
		sbyte[] extraCombatSkillGrids9 = new sbyte[5] { 4, 4, 4, 4, 4 };
		short[] resourcesAdjust9 = new short[8] { -70, -70, -70, -70, -60, -60, -70, -70 };
		short[] lifeSkillsAdjust9 = new short[16]
		{
			-1, 12, -1, -1, 12, 2, -1, -1, 2, 9,
			-1, -1, -1, -1, -1, -1
		};
		short[] combatSkillsAdjust9 = new short[14]
		{
			6, 12, 6, -1, 12, -1, 12, 12, -1, -1,
			-1, -1, -1, -1
		};
		short[] mainAttributesAdjust9 = new short[6] { -1, 12, -1, 2, 9, 12 };
		identityInteractConfig = new List<sbyte>();
		dataArray9.Add(new OrganizationMemberItem(159, 755, 13, 3, 6, 9, 3, restrictPrincipalAmount: false, -1, -1, -1, 2, 3, 5, 3, 0, 0, monasticTitleSuffixes9, 1800, 6, 3000, 2250, 0, favoriteClothingIds9, hatedClothingIds9, spouseAnonymousTitles9, canStroll: true, 330, initialAges9, equipment9, clothing9, inventory9, combatSkills9, extraCombatSkillGrids9, resourcesAdjust9, 20000, 12000, 10, 70, 4500, lifeSkillsAdjust9, 4, combatSkillsAdjust9, mainAttributesAdjust9, identityInteractConfig, 3, new ResourceInts(default(int), default(int), default(int), default(int), default(int), default(int), default(int), default(int)), new IntPair[18]
		{
			new IntPair(17, 1),
			new IntPair(8, 1),
			new IntPair(5, 6),
			new IntPair(6, 1),
			new IntPair(15, 3),
			new IntPair(16, 1),
			new IntPair(7, 1),
			new IntPair(14, 42),
			new IntPair(12, 1),
			new IntPair(4, 18),
			new IntPair(3, 18),
			new IntPair(13, 9),
			new IntPair(10, 3),
			new IntPair(2, 3),
			new IntPair(1, 3),
			new IntPair(11, 6),
			new IntPair(0, 3),
			new IntPair(9, 6)
		}, null));
		List<OrganizationMemberItem> dataArray10 = _dataArray;
		int[] monasticTitleSuffixes10 = new int[2] { 761, 762 };
		List<short> favoriteClothingIds10 = new List<short> { 55, 56, 57 };
		List<short> hatedClothingIds10 = new List<short> { 31, 32, 33 };
		int[] spouseAnonymousTitles10 = new int[2] { 763, 764 };
		short[] initialAges10 = new short[4] { 16, 18, 20, 22 };
		PresetEquipmentItemWithProb[] equipment10 = new PresetEquipmentItemWithProb[8]
		{
			new PresetEquipmentItemWithProb("Armor", 36, 75),
			new PresetEquipmentItemWithProb("Armor", 324, 100),
			new PresetEquipmentItemWithProb("Armor", 477, 75),
			new PresetEquipmentItemWithProb("Armor", 162, 75),
			new PresetEquipmentItemWithProb("Accessory", 117, 75),
			new PresetEquipmentItemWithProb("Accessory", 135, 50),
			new PresetEquipmentItemWithProb("Accessory", 72, 50),
			new PresetEquipmentItemWithProb("Carrier", 18, 75)
		};
		PresetEquipmentItem clothing10 = new PresetEquipmentItem("Clothing", 55);
		List<PresetInventoryItem> inventory10 = new List<PresetInventoryItem>
		{
			new PresetInventoryItem("SkillBook", 9, 1, 20),
			new PresetInventoryItem("SkillBook", 36, 1, 20),
			new PresetInventoryItem("Food", 93, 3, 20),
			new PresetInventoryItem("Medicine", 9, 1, 10),
			new PresetInventoryItem("Medicine", 18, 1, 10),
			new PresetInventoryItem("Medicine", 45, 1, 10),
			new PresetInventoryItem("Medicine", 286, 1, 20),
			new PresetInventoryItem("Medicine", 310, 1, 20)
		};
		List<PresetOrgMemberCombatSkill> combatSkills10 = new List<PresetOrgMemberCombatSkill>
		{
			new PresetOrgMemberCombatSkill(82, 2),
			new PresetOrgMemberCombatSkill(181, 2),
			new PresetOrgMemberCombatSkill(297, 2),
			new PresetOrgMemberCombatSkill(453, 2),
			new PresetOrgMemberCombatSkill(503, 2)
		};
		sbyte[] extraCombatSkillGrids10 = new sbyte[5] { 2, 2, 2, 2, 2 };
		short[] resourcesAdjust10 = new short[8] { -70, -70, -70, -70, -60, -60, -70, -70 };
		short[] lifeSkillsAdjust10 = new short[16]
		{
			-1, 12, -1, -1, 12, 2, -1, -1, 2, 9,
			-1, -1, -1, -1, -1, -1
		};
		short[] combatSkillsAdjust10 = new short[14]
		{
			6, 12, 6, -1, 12, -1, 12, 12, -1, -1,
			-1, -1, -1, -1
		};
		short[] mainAttributesAdjust10 = new short[6] { -1, 12, -1, 2, 9, 12 };
		identityInteractConfig = new List<sbyte>();
		dataArray10.Add(new OrganizationMemberItem(160, 760, 13, 2, 6, 9, 3, restrictPrincipalAmount: false, -1, -1, -1, 1, 2, 5, 2, 0, 0, monasticTitleSuffixes10, 600, 4, 2000, 900, 0, favoriteClothingIds10, hatedClothingIds10, spouseAnonymousTitles10, canStroll: true, 331, initialAges10, equipment10, clothing10, inventory10, combatSkills10, extraCombatSkillGrids10, resourcesAdjust10, 15000, 4000, 5, 80, 1800, lifeSkillsAdjust10, 3, combatSkillsAdjust10, mainAttributesAdjust10, identityInteractConfig, 3, new ResourceInts(default(int), default(int), default(int), default(int), default(int), default(int), default(int), default(int)), new IntPair[18]
		{
			new IntPair(17, 1),
			new IntPair(8, 1),
			new IntPair(5, 3),
			new IntPair(6, 1),
			new IntPair(15, 3),
			new IntPair(16, 1),
			new IntPair(7, 1),
			new IntPair(14, 30),
			new IntPair(12, 1),
			new IntPair(4, 15),
			new IntPair(3, 21),
			new IntPair(13, 6),
			new IntPair(10, 3),
			new IntPair(2, 3),
			new IntPair(1, 3),
			new IntPair(11, 6),
			new IntPair(0, 3),
			new IntPair(9, 6)
		}, null));
		List<OrganizationMemberItem> dataArray11 = _dataArray;
		int[] monasticTitleSuffixes11 = new int[2] { 766, 767 };
		List<short> favoriteClothingIds11 = new List<short> { 55, 56, 57 };
		List<short> hatedClothingIds11 = new List<short> { 31, 32, 33 };
		int[] spouseAnonymousTitles11 = new int[2] { 768, 769 };
		short[] initialAges11 = new short[4] { 13, 14, 15, 16 };
		PresetEquipmentItemWithProb[] equipment11 = new PresetEquipmentItemWithProb[8]
		{
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Armor", 324, 100),
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Armor", 162, 75),
			new PresetEquipmentItemWithProb("Accessory", 117, 75),
			new PresetEquipmentItemWithProb("Accessory", 135, 50),
			new PresetEquipmentItemWithProb("Accessory", -1, 0),
			new PresetEquipmentItemWithProb("Carrier", -1, 0)
		};
		PresetEquipmentItem clothing11 = new PresetEquipmentItem("Clothing", 55);
		List<PresetInventoryItem> inventory11 = new List<PresetInventoryItem>
		{
			new PresetInventoryItem("SkillBook", 9, 1, 20),
			new PresetInventoryItem("SkillBook", 36, 1, 20),
			new PresetInventoryItem("Food", 93, 3, 20),
			new PresetInventoryItem("Medicine", 9, 1, 10),
			new PresetInventoryItem("Medicine", 18, 1, 10),
			new PresetInventoryItem("Medicine", 45, 1, 10),
			new PresetInventoryItem("Medicine", 286, 1, 20),
			new PresetInventoryItem("Medicine", 310, 1, 20)
		};
		List<PresetOrgMemberCombatSkill> combatSkills11 = new List<PresetOrgMemberCombatSkill>
		{
			new PresetOrgMemberCombatSkill(82, 1),
			new PresetOrgMemberCombatSkill(181, 1),
			new PresetOrgMemberCombatSkill(297, 1),
			new PresetOrgMemberCombatSkill(503, 1),
			new PresetOrgMemberCombatSkill(573, 1)
		};
		sbyte[] extraCombatSkillGrids11 = new sbyte[5];
		short[] resourcesAdjust11 = new short[8] { -70, -70, -70, -70, -60, -60, -70, -70 };
		short[] lifeSkillsAdjust11 = new short[16]
		{
			-1, 12, -1, -1, 12, 2, -1, -1, 2, 9,
			-1, -1, -1, -1, -1, -1
		};
		short[] combatSkillsAdjust11 = new short[14]
		{
			6, 12, 6, -1, 12, -1, 12, 12, -1, -1,
			-1, -1, -1, -1
		};
		short[] mainAttributesAdjust11 = new short[6] { -1, 12, -1, 2, 9, 12 };
		identityInteractConfig = new List<sbyte>();
		dataArray11.Add(new OrganizationMemberItem(161, 765, 13, 1, 6, 9, 3, restrictPrincipalAmount: false, -1, -1, -1, 0, 1, 5, 1, 0, 0, monasticTitleSuffixes11, 300, 2, 1000, 300, 0, favoriteClothingIds11, hatedClothingIds11, spouseAnonymousTitles11, canStroll: true, 332, initialAges11, equipment11, clothing11, inventory11, combatSkills11, extraCombatSkillGrids11, resourcesAdjust11, 10000, 2000, 5, 90, 600, lifeSkillsAdjust11, 2, combatSkillsAdjust11, mainAttributesAdjust11, identityInteractConfig, 3, new ResourceInts(default(int), default(int), default(int), default(int), default(int), default(int), default(int), default(int)), new IntPair[18]
		{
			new IntPair(17, 1),
			new IntPair(8, 1),
			new IntPair(5, 3),
			new IntPair(6, 1),
			new IntPair(15, 3),
			new IntPair(16, 1),
			new IntPair(7, 1),
			new IntPair(14, 30),
			new IntPair(12, 1),
			new IntPair(4, 15),
			new IntPair(3, 21),
			new IntPair(13, 6),
			new IntPair(10, 3),
			new IntPair(2, 3),
			new IntPair(1, 3),
			new IntPair(11, 6),
			new IntPair(0, 3),
			new IntPair(9, 6)
		}, null));
		List<OrganizationMemberItem> dataArray12 = _dataArray;
		int[] monasticTitleSuffixes12 = new int[2] { 771, 772 };
		List<short> favoriteClothingIds12 = new List<short> { 55, 56, 57 };
		List<short> hatedClothingIds12 = new List<short> { 31, 32, 33 };
		int[] spouseAnonymousTitles12 = new int[2] { 773, 774 };
		short[] initialAges12 = new short[4] { 10, 10, 10, 10 };
		PresetEquipmentItemWithProb[] equipment12 = new PresetEquipmentItemWithProb[8]
		{
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Armor", 324, 100),
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Armor", 162, 75),
			new PresetEquipmentItemWithProb("Accessory", 117, 75),
			new PresetEquipmentItemWithProb("Accessory", -1, 0),
			new PresetEquipmentItemWithProb("Accessory", -1, 0),
			new PresetEquipmentItemWithProb("Carrier", -1, 0)
		};
		PresetEquipmentItem clothing12 = new PresetEquipmentItem("Clothing", 55);
		List<PresetInventoryItem> inventory12 = new List<PresetInventoryItem>
		{
			new PresetInventoryItem("SkillBook", 9, 1, 20),
			new PresetInventoryItem("SkillBook", 36, 1, 20),
			new PresetInventoryItem("Food", 93, 3, 20),
			new PresetInventoryItem("Medicine", 9, 1, 10),
			new PresetInventoryItem("Medicine", 18, 1, 10),
			new PresetInventoryItem("Medicine", 45, 1, 10),
			new PresetInventoryItem("Medicine", 286, 1, 20),
			new PresetInventoryItem("Medicine", 310, 1, 20)
		};
		List<PresetOrgMemberCombatSkill> combatSkills12 = new List<PresetOrgMemberCombatSkill>
		{
			new PresetOrgMemberCombatSkill(82, 0),
			new PresetOrgMemberCombatSkill(181, 0),
			new PresetOrgMemberCombatSkill(503, 0)
		};
		sbyte[] extraCombatSkillGrids12 = new sbyte[5];
		short[] resourcesAdjust12 = new short[8] { -70, -70, -70, -70, -60, -60, -70, -70 };
		short[] lifeSkillsAdjust12 = new short[16]
		{
			-1, 12, -1, -1, 12, 2, -1, -1, 2, 9,
			-1, -1, -1, -1, -1, -1
		};
		short[] combatSkillsAdjust12 = new short[14]
		{
			6, 12, 6, -1, 12, -1, 12, 12, -1, -1,
			-1, -1, -1, -1
		};
		short[] mainAttributesAdjust12 = new short[6] { -1, 12, -1, 2, 9, 12 };
		identityInteractConfig = new List<sbyte>();
		dataArray12.Add(new OrganizationMemberItem(162, 770, 13, 0, 6, 9, 3, restrictPrincipalAmount: false, -1, -1, -1, 0, 0, 2, 0, 0, 0, monasticTitleSuffixes12, 150, 0, 500, 150, 0, favoriteClothingIds12, hatedClothingIds12, spouseAnonymousTitles12, canStroll: true, 333, initialAges12, equipment12, clothing12, inventory12, combatSkills12, extraCombatSkillGrids12, resourcesAdjust12, 5000, 1000, 1, 100, 300, lifeSkillsAdjust12, 1, combatSkillsAdjust12, mainAttributesAdjust12, identityInteractConfig, 3, new ResourceInts(default(int), default(int), default(int), default(int), default(int), default(int), default(int), default(int)), new IntPair[18]
		{
			new IntPair(17, 1),
			new IntPair(8, 1),
			new IntPair(5, 3),
			new IntPair(6, 1),
			new IntPair(15, 3),
			new IntPair(16, 1),
			new IntPair(7, 1),
			new IntPair(14, 30),
			new IntPair(12, 1),
			new IntPair(4, 15),
			new IntPair(3, 21),
			new IntPair(13, 6),
			new IntPair(10, 3),
			new IntPair(2, 3),
			new IntPair(1, 3),
			new IntPair(11, 6),
			new IntPair(0, 3),
			new IntPair(9, 6)
		}, null));
		_dataArray.Add(new OrganizationMemberItem(163, 775, 14, 8, 1, 1, 1, restrictPrincipalAmount: true, -1, 1119, 7, 5, 6, -1, 7, 0, 0, new int[2] { 776, 777 }, 15800, 16, 13000, 30750, 0, new List<short> { 58, 59, 60, 81, 82 }, new List<short> { 24, 25, 26 }, new int[2] { 778, 779 }, canStroll: false, 334, new short[4] { 42, 50, 58, 66 }, new PresetEquipmentItemWithProb[8]
		{
			new PresetEquipmentItemWithProb("Armor", 9, 75),
			new PresetEquipmentItemWithProb("Armor", 270, 100),
			new PresetEquipmentItemWithProb("Armor", 405, 75),
			new PresetEquipmentItemWithProb("Armor", 135, 75),
			new PresetEquipmentItemWithProb("Accessory", 99, 75),
			new PresetEquipmentItemWithProb("Accessory", 153, 50),
			new PresetEquipmentItemWithProb("Accessory", 9, 50),
			new PresetEquipmentItemWithProb("Carrier", 0, 75)
		}, new PresetEquipmentItem("Clothing", 60), new List<PresetInventoryItem>
		{
			new PresetInventoryItem("SkillBook", 126, 1, 40),
			new PresetInventoryItem("SkillBook", 45, 1, 40),
			new PresetInventoryItem("Material", 14, 1, 30),
			new PresetInventoryItem("Material", 21, 1, 30),
			new PresetInventoryItem("Misc", 73, 1, 30),
			new PresetInventoryItem("CraftTool", 36, 1, 30),
			new PresetInventoryItem("Food", 93, 3, 40),
			new PresetInventoryItem("Food", 9, 3, 40),
			new PresetInventoryItem("Food", 51, 3, 40),
			new PresetInventoryItem("Food", 135, 3, 40),
			new PresetInventoryItem("Medicine", 60, 1, 20),
			new PresetInventoryItem("Medicine", 72, 1, 20),
			new PresetInventoryItem("Medicine", 100, 1, 20),
			new PresetInventoryItem("Medicine", 340, 1, 30),
			new PresetInventoryItem("Medicine", 124, 1, 20),
			new PresetInventoryItem("TeaWine", 0, 1, 30),
			new PresetInventoryItem("TeaWine", 9, 1, 30),
			new PresetInventoryItem("Material", 57, 1, 20),
			new PresetInventoryItem("Material", 64, 1, 20),
			new PresetInventoryItem("Material", 71, 1, 20),
			new PresetInventoryItem("Material", 78, 1, 20)
		}, new List<PresetOrgMemberCombatSkill>
		{
			new PresetOrgMemberCombatSkill(89, 7),
			new PresetOrgMemberCombatSkill(190, 5),
			new PresetOrgMemberCombatSkill(304, 7),
			new PresetOrgMemberCombatSkill(383, 8),
			new PresetOrgMemberCombatSkill(512, 5),
			new PresetOrgMemberCombatSkill(615, 7)
		}, new sbyte[5] { 10, 10, 10, 10, 10 }, new short[8] { -60, -80, -60, -70, -70, -70, -70, -50 }, 160000, 576000, 20, 20, 61500, new short[16]
		{
			-1, -1, -1, -1, -1, 12, 9, -1, -1, -1,
			-1, -1, 2, 2, 12, -1
		}, 8, new short[14]
		{
			12, 6, 12, 12, -1, -1, 6, -1, 12, -1,
			2, -1, -1, -1
		}, new short[6] { 12, -1, -1, 9, 9, -1 }, new List<sbyte> { 22, 24, 25, 55, 65 }, 3, new ResourceInts(default(int), default(int), default(int), default(int), default(int), default(int), default(int), default(int)), new IntPair[18]
		{
			new IntPair(17, 9),
			new IntPair(8, 9),
			new IntPair(5, 1),
			new IntPair(6, 1),
			new IntPair(15, 9),
			new IntPair(16, 6),
			new IntPair(7, 54),
			new IntPair(14, 1),
			new IntPair(12, 1),
			new IntPair(4, 1),
			new IntPair(3, 36),
			new IntPair(13, 1),
			new IntPair(10, 1),
			new IntPair(2, 3),
			new IntPair(1, 3),
			new IntPair(11, 1),
			new IntPair(0, 1),
			new IntPair(9, 6)
		}, null));
		_dataArray.Add(new OrganizationMemberItem(164, 780, 14, 7, 4, 6, 2, restrictPrincipalAmount: false, -1, 1831, 7, 4, 6, -1, 7, 0, 0, new int[2] { 781, 782 }, 8600, 14, 10000, 21150, 0, new List<short> { 58, 59, 60, 81, 82 }, new List<short> { 24, 25, 26 }, new int[2] { 783, 784 }, canStroll: false, 335, new short[4] { 38, 45, 52, 59 }, new PresetEquipmentItemWithProb[8]
		{
			new PresetEquipmentItemWithProb("Armor", 9, 75),
			new PresetEquipmentItemWithProb("Armor", 270, 100),
			new PresetEquipmentItemWithProb("Armor", 405, 75),
			new PresetEquipmentItemWithProb("Armor", 135, 75),
			new PresetEquipmentItemWithProb("Accessory", 99, 75),
			new PresetEquipmentItemWithProb("Accessory", 153, 50),
			new PresetEquipmentItemWithProb("Accessory", 9, 50),
			new PresetEquipmentItemWithProb("Carrier", 0, 75)
		}, new PresetEquipmentItem("Clothing", 59), new List<PresetInventoryItem>
		{
			new PresetInventoryItem("SkillBook", 126, 1, 40),
			new PresetInventoryItem("SkillBook", 45, 1, 40),
			new PresetInventoryItem("Material", 14, 1, 30),
			new PresetInventoryItem("Material", 21, 1, 30),
			new PresetInventoryItem("Misc", 73, 1, 30),
			new PresetInventoryItem("CraftTool", 36, 1, 30),
			new PresetInventoryItem("Food", 93, 3, 40),
			new PresetInventoryItem("Food", 9, 3, 40),
			new PresetInventoryItem("Food", 51, 3, 40),
			new PresetInventoryItem("Food", 135, 3, 40),
			new PresetInventoryItem("Medicine", 60, 1, 20),
			new PresetInventoryItem("Medicine", 72, 1, 20),
			new PresetInventoryItem("Medicine", 100, 1, 20),
			new PresetInventoryItem("Medicine", 340, 1, 30),
			new PresetInventoryItem("Medicine", 124, 1, 20),
			new PresetInventoryItem("TeaWine", 0, 1, 30),
			new PresetInventoryItem("TeaWine", 9, 1, 30),
			new PresetInventoryItem("Material", 57, 1, 20),
			new PresetInventoryItem("Material", 64, 1, 20),
			new PresetInventoryItem("Material", 71, 1, 20),
			new PresetInventoryItem("Material", 78, 1, 20)
		}, new List<PresetOrgMemberCombatSkill>
		{
			new PresetOrgMemberCombatSkill(89, 6),
			new PresetOrgMemberCombatSkill(190, 5),
			new PresetOrgMemberCombatSkill(304, 6),
			new PresetOrgMemberCombatSkill(383, 7),
			new PresetOrgMemberCombatSkill(512, 5),
			new PresetOrgMemberCombatSkill(615, 6)
		}, new sbyte[5] { 8, 8, 8, 8, 8 }, new short[8] { -60, -80, -60, -70, -70, -70, -70, -50 }, 100000, 288000, 15, 30, 42300, new short[16]
		{
			-1, -1, -1, -1, -1, 12, 9, -1, -1, -1,
			-1, -1, 2, 2, 12, -1
		}, 8, new short[14]
		{
			12, 6, 12, 12, -1, -1, 6, -1, 12, -1,
			2, -1, -1, -1
		}, new short[6] { 12, -1, -1, 9, 9, -1 }, new List<sbyte> { 22, 24, 25, 65 }, 3, new ResourceInts(default(int), default(int), default(int), default(int), default(int), default(int), default(int), default(int)), new IntPair[18]
		{
			new IntPair(17, 9),
			new IntPair(8, 9),
			new IntPair(5, 1),
			new IntPair(6, 1),
			new IntPair(15, 9),
			new IntPair(16, 6),
			new IntPair(7, 54),
			new IntPair(14, 1),
			new IntPair(12, 1),
			new IntPair(4, 1),
			new IntPair(3, 36),
			new IntPair(13, 1),
			new IntPair(10, 1),
			new IntPair(2, 3),
			new IntPair(1, 3),
			new IntPair(11, 1),
			new IntPair(0, 1),
			new IntPair(9, 6)
		}, null));
		_dataArray.Add(new OrganizationMemberItem(165, 785, 14, 6, 6, 9, 3, restrictPrincipalAmount: false, -1, 1119, -1, 3, 6, -1, 6, 0, 0, new int[2] { 786, 787 }, 6100, 12, 8000, 13800, 0, new List<short> { 58, 59, 60, 81, 82 }, new List<short> { 24, 25, 26 }, new int[2] { 788, 789 }, canStroll: false, 336, new short[4] { 34, 40, 46, 52 }, new PresetEquipmentItemWithProb[8]
		{
			new PresetEquipmentItemWithProb("Armor", 9, 75),
			new PresetEquipmentItemWithProb("Armor", 270, 100),
			new PresetEquipmentItemWithProb("Armor", 405, 75),
			new PresetEquipmentItemWithProb("Armor", 135, 75),
			new PresetEquipmentItemWithProb("Accessory", 99, 75),
			new PresetEquipmentItemWithProb("Accessory", 153, 50),
			new PresetEquipmentItemWithProb("Accessory", 9, 50),
			new PresetEquipmentItemWithProb("Carrier", 0, 75)
		}, new PresetEquipmentItem("Clothing", 59), new List<PresetInventoryItem>
		{
			new PresetInventoryItem("SkillBook", 126, 1, 40),
			new PresetInventoryItem("SkillBook", 45, 1, 40),
			new PresetInventoryItem("Material", 14, 1, 30),
			new PresetInventoryItem("Material", 21, 1, 30),
			new PresetInventoryItem("Misc", 73, 1, 30),
			new PresetInventoryItem("CraftTool", 36, 1, 30),
			new PresetInventoryItem("Food", 93, 3, 40),
			new PresetInventoryItem("Food", 9, 3, 40),
			new PresetInventoryItem("Food", 51, 3, 40),
			new PresetInventoryItem("Food", 135, 3, 40),
			new PresetInventoryItem("Medicine", 60, 1, 20),
			new PresetInventoryItem("Medicine", 72, 1, 20),
			new PresetInventoryItem("Medicine", 100, 1, 20),
			new PresetInventoryItem("Medicine", 340, 1, 30),
			new PresetInventoryItem("Medicine", 124, 1, 20),
			new PresetInventoryItem("TeaWine", 0, 1, 30),
			new PresetInventoryItem("TeaWine", 9, 1, 30),
			new PresetInventoryItem("Material", 57, 1, 20),
			new PresetInventoryItem("Material", 64, 1, 20),
			new PresetInventoryItem("Material", 71, 1, 20),
			new PresetInventoryItem("Material", 78, 1, 20)
		}, new List<PresetOrgMemberCombatSkill>
		{
			new PresetOrgMemberCombatSkill(89, 6),
			new PresetOrgMemberCombatSkill(190, 4),
			new PresetOrgMemberCombatSkill(304, 6),
			new PresetOrgMemberCombatSkill(383, 6),
			new PresetOrgMemberCombatSkill(512, 5),
			new PresetOrgMemberCombatSkill(615, 6)
		}, new sbyte[5] { 8, 8, 8, 8, 8 }, new short[8] { -60, -80, -60, -70, -70, -70, -70, -50 }, 60000, 144000, 15, 40, 27600, new short[16]
		{
			-1, -1, -1, -1, -1, 12, 9, -1, -1, -1,
			-1, -1, 2, 2, 12, -1
		}, 7, new short[14]
		{
			12, 6, 12, 12, -1, -1, 6, -1, 12, -1,
			2, -1, -1, -1
		}, new short[6] { 12, -1, -1, 9, 9, -1 }, new List<sbyte> { 22, 24, 25, 65 }, 3, new ResourceInts(default(int), default(int), default(int), default(int), default(int), default(int), default(int), default(int)), new IntPair[18]
		{
			new IntPair(17, 9),
			new IntPair(8, 9),
			new IntPair(5, 1),
			new IntPair(6, 1),
			new IntPair(15, 9),
			new IntPair(16, 6),
			new IntPair(7, 54),
			new IntPair(14, 1),
			new IntPair(12, 1),
			new IntPair(4, 1),
			new IntPair(3, 36),
			new IntPair(13, 1),
			new IntPair(10, 1),
			new IntPair(2, 3),
			new IntPair(1, 3),
			new IntPair(11, 1),
			new IntPair(0, 1),
			new IntPair(9, 6)
		}, null));
		_dataArray.Add(new OrganizationMemberItem(166, 790, 14, 5, 6, 9, 3, restrictPrincipalAmount: false, -1, 1831, -1, 2, 5, 8, 5, 0, 0, new int[2] { 791, 792 }, 4200, 10, 6000, 8400, 0, new List<short> { 58, 59, 60, 81, 82 }, new List<short> { 24, 25, 26 }, new int[2] { 793, 794 }, canStroll: true, 337, new short[4] { 30, 35, 40, 45 }, new PresetEquipmentItemWithProb[8]
		{
			new PresetEquipmentItemWithProb("Armor", 9, 75),
			new PresetEquipmentItemWithProb("Armor", 270, 100),
			new PresetEquipmentItemWithProb("Armor", 405, 75),
			new PresetEquipmentItemWithProb("Armor", 135, 75),
			new PresetEquipmentItemWithProb("Accessory", 99, 75),
			new PresetEquipmentItemWithProb("Accessory", 153, 50),
			new PresetEquipmentItemWithProb("Accessory", 9, 50),
			new PresetEquipmentItemWithProb("Carrier", 0, 75)
		}, new PresetEquipmentItem("Clothing", 59), new List<PresetInventoryItem>
		{
			new PresetInventoryItem("SkillBook", 126, 1, 30),
			new PresetInventoryItem("SkillBook", 45, 1, 30),
			new PresetInventoryItem("Material", 14, 1, 20),
			new PresetInventoryItem("Material", 21, 1, 20),
			new PresetInventoryItem("Misc", 73, 1, 30),
			new PresetInventoryItem("CraftTool", 36, 1, 30),
			new PresetInventoryItem("Food", 93, 3, 30),
			new PresetInventoryItem("Food", 9, 3, 30),
			new PresetInventoryItem("Food", 51, 3, 30),
			new PresetInventoryItem("Food", 135, 3, 30),
			new PresetInventoryItem("Medicine", 54, 1, 20),
			new PresetInventoryItem("Medicine", 66, 1, 20),
			new PresetInventoryItem("Medicine", 94, 1, 20),
			new PresetInventoryItem("Medicine", 334, 1, 30),
			new PresetInventoryItem("Medicine", 118, 1, 10),
			new PresetInventoryItem("TeaWine", 0, 1, 30),
			new PresetInventoryItem("TeaWine", 9, 1, 30),
			new PresetInventoryItem("Material", 57, 1, 20),
			new PresetInventoryItem("Material", 64, 1, 20),
			new PresetInventoryItem("Material", 71, 1, 20),
			new PresetInventoryItem("Material", 78, 1, 20)
		}, new List<PresetOrgMemberCombatSkill>
		{
			new PresetOrgMemberCombatSkill(89, 5),
			new PresetOrgMemberCombatSkill(190, 4),
			new PresetOrgMemberCombatSkill(304, 5),
			new PresetOrgMemberCombatSkill(383, 5),
			new PresetOrgMemberCombatSkill(615, 5)
		}, new sbyte[5] { 6, 6, 6, 6, 6 }, new short[8] { -60, -80, -60, -70, -70, -70, -70, -50 }, 40000, 48000, 10, 50, 16800, new short[16]
		{
			-1, -1, -1, -1, -1, 12, 9, -1, -1, -1,
			-1, -1, 2, 2, 12, -1
		}, 6, new short[14]
		{
			12, 6, 12, 12, -1, -1, 6, -1, 12, -1,
			2, -1, -1, -1
		}, new short[6] { 12, -1, -1, 9, 9, -1 }, new List<sbyte> { 65 }, 3, new ResourceInts(default(int), default(int), default(int), default(int), default(int), default(int), default(int), default(int)), new IntPair[18]
		{
			new IntPair(17, 6),
			new IntPair(8, 6),
			new IntPair(5, 1),
			new IntPair(6, 1),
			new IntPair(15, 6),
			new IntPair(16, 6),
			new IntPair(7, 42),
			new IntPair(14, 1),
			new IntPair(12, 1),
			new IntPair(4, 1),
			new IntPair(3, 36),
			new IntPair(13, 1),
			new IntPair(10, 1),
			new IntPair(2, 3),
			new IntPair(1, 3),
			new IntPair(11, 1),
			new IntPair(0, 1),
			new IntPair(9, 9)
		}, null));
		_dataArray.Add(new OrganizationMemberItem(167, 795, 14, 4, 6, 9, 3, restrictPrincipalAmount: false, -1, 1119, -1, 2, 4, 7, 4, 0, 0, new int[2] { 796, 797 }, 2800, 8, 4500, 4650, 0, new List<short> { 58, 59, 60, 81, 82 }, new List<short> { 24, 25, 26 }, new int[2] { 798, 799 }, canStroll: true, 338, new short[4] { 26, 30, 34, 38 }, new PresetEquipmentItemWithProb[8]
		{
			new PresetEquipmentItemWithProb("Armor", 9, 75),
			new PresetEquipmentItemWithProb("Armor", 270, 100),
			new PresetEquipmentItemWithProb("Armor", 405, 75),
			new PresetEquipmentItemWithProb("Armor", 135, 75),
			new PresetEquipmentItemWithProb("Accessory", 99, 75),
			new PresetEquipmentItemWithProb("Accessory", 153, 50),
			new PresetEquipmentItemWithProb("Accessory", 9, 50),
			new PresetEquipmentItemWithProb("Carrier", 0, 75)
		}, new PresetEquipmentItem("Clothing", 59), new List<PresetInventoryItem>
		{
			new PresetInventoryItem("SkillBook", 126, 1, 30),
			new PresetInventoryItem("SkillBook", 45, 1, 30),
			new PresetInventoryItem("Material", 14, 1, 20),
			new PresetInventoryItem("Material", 21, 1, 20),
			new PresetInventoryItem("Misc", 73, 1, 30),
			new PresetInventoryItem("CraftTool", 36, 1, 30),
			new PresetInventoryItem("Food", 93, 3, 30),
			new PresetInventoryItem("Food", 9, 3, 30),
			new PresetInventoryItem("Food", 51, 3, 30),
			new PresetInventoryItem("Food", 135, 3, 30),
			new PresetInventoryItem("Medicine", 54, 1, 20),
			new PresetInventoryItem("Medicine", 66, 1, 20),
			new PresetInventoryItem("Medicine", 94, 1, 20),
			new PresetInventoryItem("Medicine", 334, 1, 30),
			new PresetInventoryItem("Medicine", 118, 1, 10),
			new PresetInventoryItem("TeaWine", 0, 1, 30),
			new PresetInventoryItem("TeaWine", 9, 1, 30),
			new PresetInventoryItem("Material", 57, 1, 20),
			new PresetInventoryItem("Material", 64, 1, 20),
			new PresetInventoryItem("Material", 71, 1, 20),
			new PresetInventoryItem("Material", 78, 1, 20)
		}, new List<PresetOrgMemberCombatSkill>
		{
			new PresetOrgMemberCombatSkill(89, 4),
			new PresetOrgMemberCombatSkill(190, 3),
			new PresetOrgMemberCombatSkill(304, 4),
			new PresetOrgMemberCombatSkill(512, 4),
			new PresetOrgMemberCombatSkill(615, 4)
		}, new sbyte[5] { 4, 4, 4, 4, 4 }, new short[8] { -60, -80, -60, -70, -70, -70, -70, -50 }, 30000, 24000, 10, 60, 9300, new short[16]
		{
			-1, -1, -1, -1, -1, 12, 9, -1, -1, -1,
			-1, -1, 2, 2, 12, -1
		}, 5, new short[14]
		{
			12, 6, 12, 12, -1, -1, 6, -1, 12, -1,
			2, -1, -1, -1
		}, new short[6] { 12, -1, -1, 9, 9, -1 }, new List<sbyte> { 65 }, 3, new ResourceInts(default(int), default(int), default(int), default(int), default(int), default(int), default(int), default(int)), new IntPair[18]
		{
			new IntPair(17, 6),
			new IntPair(8, 6),
			new IntPair(5, 1),
			new IntPair(6, 1),
			new IntPair(15, 6),
			new IntPair(16, 6),
			new IntPair(7, 42),
			new IntPair(14, 1),
			new IntPair(12, 1),
			new IntPair(4, 1),
			new IntPair(3, 36),
			new IntPair(13, 1),
			new IntPair(10, 1),
			new IntPair(2, 3),
			new IntPair(1, 3),
			new IntPair(11, 1),
			new IntPair(0, 1),
			new IntPair(9, 9)
		}, null));
		_dataArray.Add(new OrganizationMemberItem(168, 800, 14, 3, 6, 9, 3, restrictPrincipalAmount: false, -1, 1831, -1, 1, 3, 6, 3, 0, 0, new int[2] { 801, 802 }, 1800, 6, 3000, 2250, 0, new List<short> { 58, 59, 60, 81, 82 }, new List<short> { 24, 25, 26 }, new int[2] { 803, 804 }, canStroll: true, 339, new short[4] { 22, 25, 28, 31 }, new PresetEquipmentItemWithProb[8]
		{
			new PresetEquipmentItemWithProb("Armor", 9, 75),
			new PresetEquipmentItemWithProb("Armor", 270, 100),
			new PresetEquipmentItemWithProb("Armor", 405, 75),
			new PresetEquipmentItemWithProb("Armor", 135, 75),
			new PresetEquipmentItemWithProb("Accessory", 99, 75),
			new PresetEquipmentItemWithProb("Accessory", 153, 50),
			new PresetEquipmentItemWithProb("Accessory", 9, 50),
			new PresetEquipmentItemWithProb("Carrier", 0, 75)
		}, new PresetEquipmentItem("Clothing", 58), new List<PresetInventoryItem>
		{
			new PresetInventoryItem("SkillBook", 126, 1, 30),
			new PresetInventoryItem("SkillBook", 45, 1, 30),
			new PresetInventoryItem("Material", 14, 1, 20),
			new PresetInventoryItem("Material", 21, 1, 20),
			new PresetInventoryItem("Misc", 73, 1, 30),
			new PresetInventoryItem("CraftTool", 36, 1, 30),
			new PresetInventoryItem("Food", 93, 3, 30),
			new PresetInventoryItem("Food", 9, 3, 30),
			new PresetInventoryItem("Food", 51, 3, 30),
			new PresetInventoryItem("Food", 135, 3, 30),
			new PresetInventoryItem("Medicine", 54, 1, 20),
			new PresetInventoryItem("Medicine", 66, 1, 20),
			new PresetInventoryItem("Medicine", 94, 1, 20),
			new PresetInventoryItem("Medicine", 334, 1, 30),
			new PresetInventoryItem("Medicine", 118, 1, 10),
			new PresetInventoryItem("TeaWine", 0, 1, 30),
			new PresetInventoryItem("TeaWine", 9, 1, 30),
			new PresetInventoryItem("Material", 57, 1, 20),
			new PresetInventoryItem("Material", 64, 1, 20),
			new PresetInventoryItem("Material", 71, 1, 20),
			new PresetInventoryItem("Material", 78, 1, 20)
		}, new List<PresetOrgMemberCombatSkill>
		{
			new PresetOrgMemberCombatSkill(89, 3),
			new PresetOrgMemberCombatSkill(190, 3),
			new PresetOrgMemberCombatSkill(304, 3),
			new PresetOrgMemberCombatSkill(383, 3),
			new PresetOrgMemberCombatSkill(615, 3)
		}, new sbyte[5] { 4, 4, 4, 4, 4 }, new short[8] { -60, -80, -60, -70, -70, -70, -70, -50 }, 20000, 12000, 10, 70, 4500, new short[16]
		{
			-1, -1, -1, -1, -1, 12, 9, -1, -1, -1,
			-1, -1, 2, 2, 12, -1
		}, 4, new short[14]
		{
			12, 6, 12, 12, -1, -1, 6, -1, 12, -1,
			2, -1, -1, -1
		}, new short[6] { 12, -1, -1, 9, 9, -1 }, new List<sbyte> { 65 }, 3, new ResourceInts(default(int), default(int), default(int), default(int), default(int), default(int), default(int), default(int)), new IntPair[18]
		{
			new IntPair(17, 6),
			new IntPair(8, 6),
			new IntPair(5, 1),
			new IntPair(6, 1),
			new IntPair(15, 6),
			new IntPair(16, 6),
			new IntPair(7, 42),
			new IntPair(14, 1),
			new IntPair(12, 1),
			new IntPair(4, 1),
			new IntPair(3, 36),
			new IntPair(13, 1),
			new IntPair(10, 1),
			new IntPair(2, 3),
			new IntPair(1, 3),
			new IntPair(11, 1),
			new IntPair(0, 1),
			new IntPair(9, 9)
		}, null));
		_dataArray.Add(new OrganizationMemberItem(169, 417, 14, 2, 8, 12, 4, restrictPrincipalAmount: false, -1, -1, -1, 1, 2, 5, 2, 0, 0, new int[2] { 805, 806 }, 600, 4, 2000, 900, 0, new List<short> { 58, 59, 60, 81, 82 }, new List<short> { 24, 25, 26 }, new int[2] { 807, 808 }, canStroll: true, 340, new short[4] { 18, 20, 22, 24 }, new PresetEquipmentItemWithProb[8]
		{
			new PresetEquipmentItemWithProb("Armor", 9, 75),
			new PresetEquipmentItemWithProb("Armor", 270, 100),
			new PresetEquipmentItemWithProb("Armor", 405, 75),
			new PresetEquipmentItemWithProb("Armor", 135, 75),
			new PresetEquipmentItemWithProb("Accessory", 99, 75),
			new PresetEquipmentItemWithProb("Accessory", 153, 50),
			new PresetEquipmentItemWithProb("Accessory", 9, 50),
			new PresetEquipmentItemWithProb("Carrier", 0, 75)
		}, new PresetEquipmentItem("Clothing", 58), new List<PresetInventoryItem>
		{
			new PresetInventoryItem("SkillBook", 126, 1, 20),
			new PresetInventoryItem("SkillBook", 45, 1, 20),
			new PresetInventoryItem("Material", 14, 1, 10),
			new PresetInventoryItem("Material", 21, 1, 10),
			new PresetInventoryItem("Misc", 73, 1, 30),
			new PresetInventoryItem("CraftTool", 36, 1, 20),
			new PresetInventoryItem("Food", 93, 3, 20),
			new PresetInventoryItem("Food", 9, 3, 20),
			new PresetInventoryItem("Food", 51, 3, 20),
			new PresetInventoryItem("Food", 135, 3, 20),
			new PresetInventoryItem("TeaWine", 0, 1, 20),
			new PresetInventoryItem("TeaWine", 9, 1, 20),
			new PresetInventoryItem("Material", 57, 1, 20),
			new PresetInventoryItem("Material", 64, 1, 20),
			new PresetInventoryItem("Material", 71, 1, 20),
			new PresetInventoryItem("Material", 78, 1, 20)
		}, new List<PresetOrgMemberCombatSkill>
		{
			new PresetOrgMemberCombatSkill(89, 2),
			new PresetOrgMemberCombatSkill(190, 2),
			new PresetOrgMemberCombatSkill(304, 2),
			new PresetOrgMemberCombatSkill(383, 2)
		}, new sbyte[5] { 2, 2, 2, 2, 2 }, new short[8] { -60, -80, -60, -70, -70, -70, -70, -50 }, 15000, 4000, 5, 80, 1800, new short[16]
		{
			-1, -1, -1, -1, -1, 12, 9, -1, -1, -1,
			-1, -1, 2, 2, 12, -1
		}, 3, new short[14]
		{
			12, 6, 12, 12, -1, -1, 6, -1, 12, -1,
			2, -1, -1, -1
		}, new short[6] { 12, -1, -1, 9, 9, -1 }, new List<sbyte> { 65 }, 3, new ResourceInts(default(int), default(int), default(int), default(int), default(int), default(int), default(int), default(int)), new IntPair[18]
		{
			new IntPair(17, 3),
			new IntPair(8, 3),
			new IntPair(5, 1),
			new IntPair(6, 1),
			new IntPair(15, 3),
			new IntPair(16, 6),
			new IntPair(7, 30),
			new IntPair(14, 1),
			new IntPair(12, 1),
			new IntPair(4, 1),
			new IntPair(3, 36),
			new IntPair(13, 1),
			new IntPair(10, 1),
			new IntPair(2, 3),
			new IntPair(1, 3),
			new IntPair(11, 1),
			new IntPair(0, 1),
			new IntPair(9, 12)
		}, null));
		_dataArray.Add(new OrganizationMemberItem(170, 809, 14, 1, 6, 9, 3, restrictPrincipalAmount: false, -1, -1, -1, 0, 1, 3, 1, 0, 0, new int[2] { 810, 811 }, 300, 2, 1000, 300, 0, new List<short> { 58, 59, 60, 81, 82 }, new List<short> { 24, 25, 26 }, new int[2] { 812, 813 }, canStroll: true, 341, new short[4] { 14, 15, 16, 17 }, new PresetEquipmentItemWithProb[8]
		{
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Armor", 270, 100),
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Armor", 135, 75),
			new PresetEquipmentItemWithProb("Accessory", 99, 75),
			new PresetEquipmentItemWithProb("Accessory", 153, 50),
			new PresetEquipmentItemWithProb("Accessory", -1, 0),
			new PresetEquipmentItemWithProb("Carrier", -1, 0)
		}, new PresetEquipmentItem("Clothing", 58), new List<PresetInventoryItem>
		{
			new PresetInventoryItem("SkillBook", 126, 1, 20),
			new PresetInventoryItem("SkillBook", 45, 1, 20),
			new PresetInventoryItem("Material", 14, 1, 10),
			new PresetInventoryItem("Material", 21, 1, 10),
			new PresetInventoryItem("Misc", 73, 1, 30),
			new PresetInventoryItem("CraftTool", 36, 1, 20),
			new PresetInventoryItem("Food", 93, 3, 20),
			new PresetInventoryItem("Food", 9, 3, 20),
			new PresetInventoryItem("Food", 51, 3, 20),
			new PresetInventoryItem("Food", 135, 3, 20),
			new PresetInventoryItem("TeaWine", 0, 1, 20),
			new PresetInventoryItem("TeaWine", 9, 1, 20),
			new PresetInventoryItem("Material", 57, 1, 20),
			new PresetInventoryItem("Material", 64, 1, 20),
			new PresetInventoryItem("Material", 71, 1, 20),
			new PresetInventoryItem("Material", 78, 1, 20)
		}, new List<PresetOrgMemberCombatSkill>
		{
			new PresetOrgMemberCombatSkill(89, 1),
			new PresetOrgMemberCombatSkill(190, 1),
			new PresetOrgMemberCombatSkill(615, 1)
		}, new sbyte[5], new short[8] { -60, -80, -60, -70, -70, -70, -70, -50 }, 10000, 2000, 5, 90, 600, new short[16]
		{
			-1, -1, -1, -1, -1, 12, 9, -1, -1, -1,
			-1, -1, 2, 2, 12, -1
		}, 2, new short[14]
		{
			12, 6, 12, 12, -1, -1, 6, -1, 12, -1,
			2, -1, -1, -1
		}, new short[6] { 12, -1, -1, 9, 9, -1 }, new List<sbyte> { 65 }, 3, new ResourceInts(default(int), default(int), default(int), default(int), default(int), default(int), default(int), default(int)), new IntPair[18]
		{
			new IntPair(17, 3),
			new IntPair(8, 3),
			new IntPair(5, 1),
			new IntPair(6, 1),
			new IntPair(15, 3),
			new IntPair(16, 6),
			new IntPair(7, 30),
			new IntPair(14, 1),
			new IntPair(12, 1),
			new IntPair(4, 1),
			new IntPair(3, 36),
			new IntPair(13, 1),
			new IntPair(10, 1),
			new IntPair(2, 3),
			new IntPair(1, 3),
			new IntPair(11, 1),
			new IntPair(0, 1),
			new IntPair(9, 12)
		}, null));
		_dataArray.Add(new OrganizationMemberItem(171, 814, 14, 0, 6, 9, 3, restrictPrincipalAmount: false, -1, -1, -1, 0, 0, 2, 0, 0, 0, new int[2] { 815, 816 }, 150, 0, 500, 150, 0, new List<short> { 58, 59, 60, 81, 82 }, new List<short> { 24, 25, 26 }, new int[2] { 817, 818 }, canStroll: true, 342, new short[4] { 10, 10, 10, 10 }, new PresetEquipmentItemWithProb[8]
		{
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Armor", 270, 100),
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Armor", 135, 75),
			new PresetEquipmentItemWithProb("Accessory", 99, 75),
			new PresetEquipmentItemWithProb("Accessory", -1, 0),
			new PresetEquipmentItemWithProb("Accessory", -1, 0),
			new PresetEquipmentItemWithProb("Carrier", -1, 0)
		}, new PresetEquipmentItem("Clothing", 58), new List<PresetInventoryItem>
		{
			new PresetInventoryItem("SkillBook", 126, 1, 20),
			new PresetInventoryItem("SkillBook", 45, 1, 20),
			new PresetInventoryItem("Material", 14, 1, 10),
			new PresetInventoryItem("Material", 21, 1, 10),
			new PresetInventoryItem("Misc", 73, 1, 30),
			new PresetInventoryItem("CraftTool", 36, 1, 20),
			new PresetInventoryItem("Food", 93, 3, 20),
			new PresetInventoryItem("Food", 9, 3, 20),
			new PresetInventoryItem("Food", 51, 3, 20),
			new PresetInventoryItem("Food", 135, 3, 20),
			new PresetInventoryItem("TeaWine", 0, 1, 20),
			new PresetInventoryItem("TeaWine", 9, 1, 20),
			new PresetInventoryItem("Material", 57, 1, 20),
			new PresetInventoryItem("Material", 64, 1, 20),
			new PresetInventoryItem("Material", 71, 1, 20),
			new PresetInventoryItem("Material", 78, 1, 20)
		}, new List<PresetOrgMemberCombatSkill>
		{
			new PresetOrgMemberCombatSkill(89, 0),
			new PresetOrgMemberCombatSkill(190, 0),
			new PresetOrgMemberCombatSkill(512, 0)
		}, new sbyte[5], new short[8] { -60, -80, -60, -70, -70, -70, -70, -50 }, 5000, 1000, 1, 100, 300, new short[16]
		{
			-1, -1, -1, -1, -1, 12, 9, -1, -1, -1,
			-1, -1, 2, 2, 12, -1
		}, 1, new short[14]
		{
			12, 6, 12, 12, -1, -1, 6, -1, 12, -1,
			2, -1, -1, -1
		}, new short[6] { 12, -1, -1, 9, 9, -1 }, new List<sbyte> { 65 }, 3, new ResourceInts(default(int), default(int), default(int), default(int), default(int), default(int), default(int), default(int)), new IntPair[18]
		{
			new IntPair(17, 3),
			new IntPair(8, 3),
			new IntPair(5, 1),
			new IntPair(6, 1),
			new IntPair(15, 3),
			new IntPair(16, 6),
			new IntPair(7, 30),
			new IntPair(14, 1),
			new IntPair(12, 1),
			new IntPair(4, 1),
			new IntPair(3, 36),
			new IntPair(13, 1),
			new IntPair(10, 1),
			new IntPair(2, 3),
			new IntPair(1, 3),
			new IntPair(11, 1),
			new IntPair(0, 1),
			new IntPair(9, 12)
		}, null));
		_dataArray.Add(new OrganizationMemberItem(172, 685, 15, 8, 1, 1, 1, restrictPrincipalAmount: true, -1, -1, 7, 5, 6, -1, 7, 0, 0, new int[2] { 819, 820 }, 15800, 16, 13000, 30750, 0, new List<short> { 61, 62, 63, 7, 8, 15, 16, 17 }, new List<short> { 21, 22, 23 }, new int[2] { 821, 822 }, canStroll: false, 343, new short[4] { 22, 28, 34, 40 }, new PresetEquipmentItemWithProb[8]
		{
			new PresetEquipmentItemWithProb("Armor", 45, 75),
			new PresetEquipmentItemWithProb("Armor", 306, 100),
			new PresetEquipmentItemWithProb("Armor", 432, 75),
			new PresetEquipmentItemWithProb("Armor", 180, 75),
			new PresetEquipmentItemWithProb("Accessory", 117, 75),
			new PresetEquipmentItemWithProb("Accessory", 153, 50),
			new PresetEquipmentItemWithProb("Accessory", 81, 50),
			new PresetEquipmentItemWithProb("Carrier", 0, 75)
		}, new PresetEquipmentItem("Clothing", 63), new List<PresetInventoryItem>
		{
			new PresetInventoryItem("SkillBook", 135, 1, 30),
			new PresetInventoryItem("Misc", 0, 3, 40),
			new PresetInventoryItem("CraftTool", 45, 1, 30),
			new PresetInventoryItem("Food", 9, 3, 40),
			new PresetInventoryItem("Food", 51, 3, 40),
			new PresetInventoryItem("Food", 135, 3, 40),
			new PresetInventoryItem("Medicine", 0, 1, 30),
			new PresetInventoryItem("Medicine", 27, 1, 30),
			new PresetInventoryItem("Medicine", 36, 1, 30),
			new PresetInventoryItem("Medicine", 60, 1, 20),
			new PresetInventoryItem("Medicine", 72, 1, 20),
			new PresetInventoryItem("Medicine", 88, 1, 20),
			new PresetInventoryItem("Medicine", 100, 1, 20),
			new PresetInventoryItem("Medicine", 340, 1, 20),
			new PresetInventoryItem("Medicine", 124, 1, 20),
			new PresetInventoryItem("TeaWine", 0, 1, 20),
			new PresetInventoryItem("TeaWine", 9, 1, 20)
		}, new List<PresetOrgMemberCombatSkill>
		{
			new PresetOrgMemberCombatSkill(97, 8),
			new PresetOrgMemberCombatSkill(196, 4),
			new PresetOrgMemberCombatSkill(312, 8),
			new PresetOrgMemberCombatSkill(392, 7),
			new PresetOrgMemberCombatSkill(462, 6),
			new PresetOrgMemberCombatSkill(486, 7),
			new PresetOrgMemberCombatSkill(518, 6)
		}, new sbyte[5] { 10, 10, 10, 10, 10 }, new short[8] { -70, -70, -70, -60, -80, -70, -60, -60 }, 160000, 576000, 20, 20, 61500, new short[16]
		{
			-1, -1, -1, -1, -1, 9, 2, 2, 9, 9,
			-1, -1, 9, -1, -1, 12
		}, 8, new short[14]
		{
			12, 6, 12, 12, 9, 12, 9, -1, -1, -1,
			-1, -1, -1, -1
		}, new short[6] { 12, 12, 2, 9, -1, -1 }, new List<sbyte> { 23, 24, 25, 55 }, 3, new ResourceInts(default(int), default(int), default(int), default(int), default(int), default(int), default(int), default(int)), new IntPair[18]
		{
			new IntPair(17, 42),
			new IntPair(8, 21),
			new IntPair(5, 1),
			new IntPair(6, 1),
			new IntPair(15, 3),
			new IntPair(16, 3),
			new IntPair(7, 3),
			new IntPair(14, 1),
			new IntPair(12, 1),
			new IntPair(4, 1),
			new IntPair(3, 15),
			new IntPair(13, 1),
			new IntPair(10, 1),
			new IntPair(2, 1),
			new IntPair(1, 3),
			new IntPair(11, 3),
			new IntPair(0, 1),
			new IntPair(9, 42)
		}, null));
		_dataArray.Add(new OrganizationMemberItem(173, 823, 15, 7, 4, 6, 2, restrictPrincipalAmount: false, -1, -1, 7, 5, 6, 8, 7, 0, 0, new int[2] { 824, 825 }, 8600, 14, 10000, 21150, 0, new List<short> { 61, 62, 63, 7, 8, 15, 16, 17 }, new List<short> { 21, 22, 23 }, new int[2] { 826, 827 }, canStroll: false, 344, new short[4] { 24, 31, 38, 45 }, new PresetEquipmentItemWithProb[8]
		{
			new PresetEquipmentItemWithProb("Armor", 45, 75),
			new PresetEquipmentItemWithProb("Armor", 306, 100),
			new PresetEquipmentItemWithProb("Armor", 432, 75),
			new PresetEquipmentItemWithProb("Armor", 180, 75),
			new PresetEquipmentItemWithProb("Accessory", 117, 75),
			new PresetEquipmentItemWithProb("Accessory", 153, 50),
			new PresetEquipmentItemWithProb("Accessory", 81, 50),
			new PresetEquipmentItemWithProb("Carrier", 0, 75)
		}, new PresetEquipmentItem("Clothing", 62), new List<PresetInventoryItem>
		{
			new PresetInventoryItem("SkillBook", 135, 1, 30),
			new PresetInventoryItem("Misc", 0, 3, 40),
			new PresetInventoryItem("CraftTool", 45, 1, 30),
			new PresetInventoryItem("Food", 9, 3, 40),
			new PresetInventoryItem("Food", 51, 3, 40),
			new PresetInventoryItem("Food", 135, 3, 40),
			new PresetInventoryItem("Medicine", 0, 1, 30),
			new PresetInventoryItem("Medicine", 27, 1, 30),
			new PresetInventoryItem("Medicine", 36, 1, 30),
			new PresetInventoryItem("Medicine", 60, 1, 20),
			new PresetInventoryItem("Medicine", 72, 1, 20),
			new PresetInventoryItem("Medicine", 88, 1, 20),
			new PresetInventoryItem("Medicine", 100, 1, 20),
			new PresetInventoryItem("Medicine", 340, 1, 20),
			new PresetInventoryItem("Medicine", 124, 1, 20),
			new PresetInventoryItem("TeaWine", 0, 1, 20),
			new PresetInventoryItem("TeaWine", 9, 1, 20)
		}, new List<PresetOrgMemberCombatSkill>
		{
			new PresetOrgMemberCombatSkill(97, 7),
			new PresetOrgMemberCombatSkill(196, 4),
			new PresetOrgMemberCombatSkill(312, 7),
			new PresetOrgMemberCombatSkill(392, 6),
			new PresetOrgMemberCombatSkill(462, 6),
			new PresetOrgMemberCombatSkill(486, 6),
			new PresetOrgMemberCombatSkill(518, 6)
		}, new sbyte[5] { 8, 8, 8, 8, 8 }, new short[8] { -70, -70, -70, -60, -80, -70, -60, -60 }, 100000, 288000, 15, 30, 42300, new short[16]
		{
			-1, -1, -1, -1, -1, 9, 2, 2, 9, 9,
			-1, -1, 9, -1, -1, 12
		}, 8, new short[14]
		{
			12, 6, 12, 12, 9, 12, 9, -1, -1, -1,
			-1, -1, -1, -1
		}, new short[6] { 12, 12, 2, 9, -1, -1 }, new List<sbyte> { 23, 24, 25 }, 3, new ResourceInts(default(int), default(int), default(int), default(int), default(int), default(int), default(int), default(int)), new IntPair[18]
		{
			new IntPair(17, 42),
			new IntPair(8, 21),
			new IntPair(5, 1),
			new IntPair(6, 1),
			new IntPair(15, 3),
			new IntPair(16, 3),
			new IntPair(7, 3),
			new IntPair(14, 1),
			new IntPair(12, 1),
			new IntPair(4, 1),
			new IntPair(3, 15),
			new IntPair(13, 1),
			new IntPair(10, 1),
			new IntPair(2, 1),
			new IntPair(1, 3),
			new IntPair(11, 3),
			new IntPair(0, 1),
			new IntPair(9, 42)
		}, null));
		_dataArray.Add(new OrganizationMemberItem(174, 229, 15, 6, 12, 18, 6, restrictPrincipalAmount: false, -1, -1, -1, 4, 6, 8, 6, 0, 0, new int[2] { 828, 829 }, 6100, 12, 8000, 13800, 0, new List<short> { 61, 62, 63, 7, 8, 15, 16, 17 }, new List<short> { 21, 22, 23 }, new int[2] { 830, 831 }, canStroll: false, 345, new short[4] { 26, 34, 42, 50 }, new PresetEquipmentItemWithProb[8]
		{
			new PresetEquipmentItemWithProb("Armor", 45, 75),
			new PresetEquipmentItemWithProb("Armor", 306, 100),
			new PresetEquipmentItemWithProb("Armor", 432, 75),
			new PresetEquipmentItemWithProb("Armor", 180, 75),
			new PresetEquipmentItemWithProb("Accessory", 117, 75),
			new PresetEquipmentItemWithProb("Accessory", 153, 50),
			new PresetEquipmentItemWithProb("Accessory", 81, 50),
			new PresetEquipmentItemWithProb("Carrier", 0, 75)
		}, new PresetEquipmentItem("Clothing", 62), new List<PresetInventoryItem>
		{
			new PresetInventoryItem("SkillBook", 135, 1, 30),
			new PresetInventoryItem("Misc", 0, 3, 40),
			new PresetInventoryItem("CraftTool", 45, 1, 30),
			new PresetInventoryItem("Food", 9, 3, 40),
			new PresetInventoryItem("Food", 51, 3, 40),
			new PresetInventoryItem("Food", 135, 3, 40),
			new PresetInventoryItem("Medicine", 0, 1, 30),
			new PresetInventoryItem("Medicine", 27, 1, 30),
			new PresetInventoryItem("Medicine", 36, 1, 30),
			new PresetInventoryItem("Medicine", 60, 1, 20),
			new PresetInventoryItem("Medicine", 72, 1, 20),
			new PresetInventoryItem("Medicine", 88, 1, 20),
			new PresetInventoryItem("Medicine", 100, 1, 20),
			new PresetInventoryItem("Medicine", 340, 1, 20),
			new PresetInventoryItem("Medicine", 124, 1, 20),
			new PresetInventoryItem("TeaWine", 0, 1, 20),
			new PresetInventoryItem("TeaWine", 9, 1, 20)
		}, new List<PresetOrgMemberCombatSkill>
		{
			new PresetOrgMemberCombatSkill(97, 6),
			new PresetOrgMemberCombatSkill(196, 3),
			new PresetOrgMemberCombatSkill(312, 6),
			new PresetOrgMemberCombatSkill(392, 6),
			new PresetOrgMemberCombatSkill(462, 5),
			new PresetOrgMemberCombatSkill(486, 6),
			new PresetOrgMemberCombatSkill(518, 5)
		}, new sbyte[5] { 8, 8, 8, 8, 8 }, new short[8] { -70, -70, -70, -60, -80, -70, -60, -60 }, 60000, 144000, 15, 40, 27600, new short[16]
		{
			-1, -1, -1, -1, -1, 9, 2, 2, 9, 9,
			-1, -1, 9, -1, -1, 12
		}, 7, new short[14]
		{
			12, 6, 12, 12, 9, 12, 9, -1, -1, -1,
			-1, -1, -1, -1
		}, new short[6] { 12, 12, 2, 9, -1, -1 }, new List<sbyte> { 23, 24, 25 }, 3, new ResourceInts(default(int), default(int), default(int), default(int), default(int), default(int), default(int), default(int)), new IntPair[18]
		{
			new IntPair(17, 42),
			new IntPair(8, 21),
			new IntPair(5, 1),
			new IntPair(6, 1),
			new IntPair(15, 3),
			new IntPair(16, 3),
			new IntPair(7, 3),
			new IntPair(14, 1),
			new IntPair(12, 1),
			new IntPair(4, 1),
			new IntPair(3, 15),
			new IntPair(13, 1),
			new IntPair(10, 1),
			new IntPair(2, 1),
			new IntPair(1, 3),
			new IntPair(11, 3),
			new IntPair(0, 1),
			new IntPair(9, 42)
		}, null));
		List<OrganizationMemberItem> dataArray13 = _dataArray;
		int[] monasticTitleSuffixes13 = new int[2] { 833, 834 };
		List<short> favoriteClothingIds13 = new List<short> { 61, 62, 63, 7, 8, 15, 16, 17 };
		List<short> hatedClothingIds13 = new List<short> { 21, 22, 23 };
		int[] spouseAnonymousTitles13 = new int[2] { 835, 836 };
		short[] initialAges13 = new short[4] { 20, 25, 30, 35 };
		PresetEquipmentItemWithProb[] equipment13 = new PresetEquipmentItemWithProb[8]
		{
			new PresetEquipmentItemWithProb("Armor", 45, 75),
			new PresetEquipmentItemWithProb("Armor", 306, 100),
			new PresetEquipmentItemWithProb("Armor", 432, 75),
			new PresetEquipmentItemWithProb("Armor", 180, 75),
			new PresetEquipmentItemWithProb("Accessory", 117, 75),
			new PresetEquipmentItemWithProb("Accessory", 153, 50),
			new PresetEquipmentItemWithProb("Accessory", 81, 50),
			new PresetEquipmentItemWithProb("Carrier", 0, 75)
		};
		PresetEquipmentItem clothing13 = new PresetEquipmentItem("Clothing", 62);
		List<PresetInventoryItem> inventory13 = new List<PresetInventoryItem>
		{
			new PresetInventoryItem("SkillBook", 135, 1, 20),
			new PresetInventoryItem("Misc", 0, 3, 30),
			new PresetInventoryItem("CraftTool", 45, 1, 30),
			new PresetInventoryItem("Food", 9, 3, 30),
			new PresetInventoryItem("Food", 51, 3, 30),
			new PresetInventoryItem("Food", 135, 3, 30),
			new PresetInventoryItem("Medicine", 0, 1, 20),
			new PresetInventoryItem("Medicine", 27, 1, 20),
			new PresetInventoryItem("Medicine", 36, 1, 20),
			new PresetInventoryItem("Medicine", 54, 1, 20),
			new PresetInventoryItem("Medicine", 66, 1, 20),
			new PresetInventoryItem("Medicine", 82, 1, 20),
			new PresetInventoryItem("Medicine", 94, 1, 20),
			new PresetInventoryItem("Medicine", 334, 1, 10),
			new PresetInventoryItem("Medicine", 118, 1, 10),
			new PresetInventoryItem("TeaWine", 0, 1, 20),
			new PresetInventoryItem("TeaWine", 9, 1, 20)
		};
		List<PresetOrgMemberCombatSkill> combatSkills13 = new List<PresetOrgMemberCombatSkill>
		{
			new PresetOrgMemberCombatSkill(97, 5),
			new PresetOrgMemberCombatSkill(196, 3),
			new PresetOrgMemberCombatSkill(312, 5),
			new PresetOrgMemberCombatSkill(486, 5),
			new PresetOrgMemberCombatSkill(518, 5)
		};
		sbyte[] extraCombatSkillGrids13 = new sbyte[5] { 6, 6, 6, 6, 6 };
		short[] resourcesAdjust13 = new short[8] { -70, -70, -70, -60, -80, -70, -60, -60 };
		short[] lifeSkillsAdjust13 = new short[16]
		{
			-1, -1, -1, -1, -1, 9, 2, 2, 9, 9,
			-1, -1, 9, -1, -1, 12
		};
		short[] combatSkillsAdjust13 = new short[14]
		{
			12, 6, 12, 12, 9, 12, 9, -1, -1, -1,
			-1, -1, -1, -1
		};
		short[] mainAttributesAdjust13 = new short[6] { 12, 12, 2, 9, -1, -1 };
		identityInteractConfig = new List<sbyte>();
		dataArray13.Add(new OrganizationMemberItem(175, 832, 15, 5, 4, 6, 2, restrictPrincipalAmount: false, -1, -1, -1, 3, 5, 7, 5, 0, 0, monasticTitleSuffixes13, 4200, 10, 6000, 8400, 0, favoriteClothingIds13, hatedClothingIds13, spouseAnonymousTitles13, canStroll: true, 346, initialAges13, equipment13, clothing13, inventory13, combatSkills13, extraCombatSkillGrids13, resourcesAdjust13, 40000, 48000, 10, 50, 16800, lifeSkillsAdjust13, 6, combatSkillsAdjust13, mainAttributesAdjust13, identityInteractConfig, 3, new ResourceInts(default(int), default(int), default(int), default(int), default(int), default(int), default(int), default(int)), new IntPair[18]
		{
			new IntPair(17, 30),
			new IntPair(8, 15),
			new IntPair(5, 1),
			new IntPair(6, 1),
			new IntPair(15, 3),
			new IntPair(16, 3),
			new IntPair(7, 3),
			new IntPair(14, 1),
			new IntPair(12, 1),
			new IntPair(4, 1),
			new IntPair(3, 15),
			new IntPair(13, 1),
			new IntPair(10, 1),
			new IntPair(2, 1),
			new IntPair(1, 3),
			new IntPair(11, 3),
			new IntPair(0, 1),
			new IntPair(9, 42)
		}, null));
		List<OrganizationMemberItem> dataArray14 = _dataArray;
		int[] monasticTitleSuffixes14 = new int[2] { 838, 839 };
		List<short> favoriteClothingIds14 = new List<short> { 61, 62, 63, 7, 8, 15, 16, 17 };
		List<short> hatedClothingIds14 = new List<short> { 21, 22, 23 };
		int[] spouseAnonymousTitles14 = new int[2] { 840, 841 };
		short[] initialAges14 = new short[4] { 18, 22, 26, 30 };
		PresetEquipmentItemWithProb[] equipment14 = new PresetEquipmentItemWithProb[8]
		{
			new PresetEquipmentItemWithProb("Armor", 45, 75),
			new PresetEquipmentItemWithProb("Armor", 306, 100),
			new PresetEquipmentItemWithProb("Armor", 432, 75),
			new PresetEquipmentItemWithProb("Armor", 180, 75),
			new PresetEquipmentItemWithProb("Accessory", 117, 75),
			new PresetEquipmentItemWithProb("Accessory", 153, 50),
			new PresetEquipmentItemWithProb("Accessory", 81, 50),
			new PresetEquipmentItemWithProb("Carrier", 0, 75)
		};
		PresetEquipmentItem clothing14 = new PresetEquipmentItem("Clothing", 62);
		List<PresetInventoryItem> inventory14 = new List<PresetInventoryItem>
		{
			new PresetInventoryItem("SkillBook", 135, 1, 20),
			new PresetInventoryItem("Misc", 0, 3, 30),
			new PresetInventoryItem("CraftTool", 45, 1, 30),
			new PresetInventoryItem("Food", 9, 3, 30),
			new PresetInventoryItem("Food", 51, 3, 30),
			new PresetInventoryItem("Food", 135, 3, 30),
			new PresetInventoryItem("Medicine", 0, 1, 20),
			new PresetInventoryItem("Medicine", 27, 1, 20),
			new PresetInventoryItem("Medicine", 36, 1, 20),
			new PresetInventoryItem("Medicine", 54, 1, 20),
			new PresetInventoryItem("Medicine", 66, 1, 20),
			new PresetInventoryItem("Medicine", 82, 1, 20),
			new PresetInventoryItem("Medicine", 94, 1, 20),
			new PresetInventoryItem("Medicine", 334, 1, 10),
			new PresetInventoryItem("Medicine", 118, 1, 10),
			new PresetInventoryItem("TeaWine", 0, 1, 20),
			new PresetInventoryItem("TeaWine", 9, 1, 20)
		};
		List<PresetOrgMemberCombatSkill> combatSkills14 = new List<PresetOrgMemberCombatSkill>
		{
			new PresetOrgMemberCombatSkill(97, 4),
			new PresetOrgMemberCombatSkill(196, 3),
			new PresetOrgMemberCombatSkill(312, 4),
			new PresetOrgMemberCombatSkill(392, 4),
			new PresetOrgMemberCombatSkill(462, 4),
			new PresetOrgMemberCombatSkill(486, 4)
		};
		sbyte[] extraCombatSkillGrids14 = new sbyte[5] { 4, 4, 4, 4, 4 };
		short[] resourcesAdjust14 = new short[8] { -70, -70, -70, -60, -80, -70, -60, -60 };
		short[] lifeSkillsAdjust14 = new short[16]
		{
			-1, -1, -1, -1, -1, 9, 2, 2, 9, 9,
			-1, -1, 9, -1, -1, 12
		};
		short[] combatSkillsAdjust14 = new short[14]
		{
			12, 6, 12, 12, 9, 12, 9, -1, -1, -1,
			-1, -1, -1, -1
		};
		short[] mainAttributesAdjust14 = new short[6] { 12, 12, 2, 9, -1, -1 };
		identityInteractConfig = new List<sbyte>();
		dataArray14.Add(new OrganizationMemberItem(176, 837, 15, 4, 4, 6, 2, restrictPrincipalAmount: false, -1, -1, -1, 2, 4, 6, 4, 0, 0, monasticTitleSuffixes14, 2800, 8, 4500, 4650, 0, favoriteClothingIds14, hatedClothingIds14, spouseAnonymousTitles14, canStroll: true, 347, initialAges14, equipment14, clothing14, inventory14, combatSkills14, extraCombatSkillGrids14, resourcesAdjust14, 30000, 24000, 10, 60, 9300, lifeSkillsAdjust14, 5, combatSkillsAdjust14, mainAttributesAdjust14, identityInteractConfig, 3, new ResourceInts(default(int), default(int), default(int), default(int), default(int), default(int), default(int), default(int)), new IntPair[18]
		{
			new IntPair(17, 30),
			new IntPair(8, 15),
			new IntPair(5, 1),
			new IntPair(6, 1),
			new IntPair(15, 3),
			new IntPair(16, 3),
			new IntPair(7, 3),
			new IntPair(14, 1),
			new IntPair(12, 1),
			new IntPair(4, 1),
			new IntPair(3, 15),
			new IntPair(13, 1),
			new IntPair(10, 1),
			new IntPair(2, 1),
			new IntPair(1, 3),
			new IntPair(11, 3),
			new IntPair(0, 1),
			new IntPair(9, 42)
		}, null));
		List<OrganizationMemberItem> dataArray15 = _dataArray;
		int[] monasticTitleSuffixes15 = new int[2] { 843, 844 };
		List<short> favoriteClothingIds15 = new List<short> { 61, 62, 63, 7, 8, 15, 16, 17 };
		List<short> hatedClothingIds15 = new List<short> { 21, 22, 23 };
		int[] spouseAnonymousTitles15 = new int[2] { 845, 846 };
		short[] initialAges15 = new short[4] { 16, 19, 22, 25 };
		PresetEquipmentItemWithProb[] equipment15 = new PresetEquipmentItemWithProb[8]
		{
			new PresetEquipmentItemWithProb("Armor", 45, 75),
			new PresetEquipmentItemWithProb("Armor", 306, 100),
			new PresetEquipmentItemWithProb("Armor", 432, 75),
			new PresetEquipmentItemWithProb("Armor", 180, 75),
			new PresetEquipmentItemWithProb("Accessory", 117, 75),
			new PresetEquipmentItemWithProb("Accessory", 153, 50),
			new PresetEquipmentItemWithProb("Accessory", 81, 50),
			new PresetEquipmentItemWithProb("Carrier", 0, 75)
		};
		PresetEquipmentItem clothing15 = new PresetEquipmentItem("Clothing", 61);
		List<PresetInventoryItem> inventory15 = new List<PresetInventoryItem>
		{
			new PresetInventoryItem("SkillBook", 135, 1, 20),
			new PresetInventoryItem("Misc", 0, 3, 30),
			new PresetInventoryItem("CraftTool", 45, 1, 30),
			new PresetInventoryItem("Food", 9, 3, 30),
			new PresetInventoryItem("Food", 51, 3, 30),
			new PresetInventoryItem("Food", 135, 3, 30),
			new PresetInventoryItem("Medicine", 0, 1, 20),
			new PresetInventoryItem("Medicine", 27, 1, 20),
			new PresetInventoryItem("Medicine", 36, 1, 20),
			new PresetInventoryItem("Medicine", 54, 1, 20),
			new PresetInventoryItem("Medicine", 66, 1, 20),
			new PresetInventoryItem("Medicine", 82, 1, 20),
			new PresetInventoryItem("Medicine", 94, 1, 20),
			new PresetInventoryItem("Medicine", 334, 1, 10),
			new PresetInventoryItem("Medicine", 118, 1, 10),
			new PresetInventoryItem("TeaWine", 0, 1, 20),
			new PresetInventoryItem("TeaWine", 9, 1, 20)
		};
		List<PresetOrgMemberCombatSkill> combatSkills15 = new List<PresetOrgMemberCombatSkill>
		{
			new PresetOrgMemberCombatSkill(97, 3),
			new PresetOrgMemberCombatSkill(196, 2),
			new PresetOrgMemberCombatSkill(312, 3),
			new PresetOrgMemberCombatSkill(462, 3),
			new PresetOrgMemberCombatSkill(518, 3)
		};
		sbyte[] extraCombatSkillGrids15 = new sbyte[5] { 4, 4, 4, 4, 4 };
		short[] resourcesAdjust15 = new short[8] { -70, -70, -70, -60, -80, -70, -60, -60 };
		short[] lifeSkillsAdjust15 = new short[16]
		{
			-1, -1, -1, -1, -1, 9, 2, 2, 9, 9,
			-1, -1, 9, -1, -1, 12
		};
		short[] combatSkillsAdjust15 = new short[14]
		{
			12, 6, 12, 12, 9, 12, 9, -1, -1, -1,
			-1, -1, -1, -1
		};
		short[] mainAttributesAdjust15 = new short[6] { 12, 12, 2, 9, -1, -1 };
		identityInteractConfig = new List<sbyte>();
		dataArray15.Add(new OrganizationMemberItem(177, 842, 15, 3, 8, 12, 4, restrictPrincipalAmount: false, -1, -1, -1, 2, 3, 5, 3, 0, 0, monasticTitleSuffixes15, 1800, 6, 3000, 2250, 0, favoriteClothingIds15, hatedClothingIds15, spouseAnonymousTitles15, canStroll: true, 348, initialAges15, equipment15, clothing15, inventory15, combatSkills15, extraCombatSkillGrids15, resourcesAdjust15, 20000, 12000, 10, 70, 4500, lifeSkillsAdjust15, 4, combatSkillsAdjust15, mainAttributesAdjust15, identityInteractConfig, 3, new ResourceInts(default(int), default(int), default(int), default(int), default(int), default(int), default(int), default(int)), new IntPair[18]
		{
			new IntPair(17, 30),
			new IntPair(8, 15),
			new IntPair(5, 1),
			new IntPair(6, 1),
			new IntPair(15, 3),
			new IntPair(16, 3),
			new IntPair(7, 3),
			new IntPair(14, 1),
			new IntPair(12, 1),
			new IntPair(4, 1),
			new IntPair(3, 15),
			new IntPair(13, 1),
			new IntPair(10, 1),
			new IntPair(2, 1),
			new IntPair(1, 3),
			new IntPair(11, 3),
			new IntPair(0, 1),
			new IntPair(9, 42)
		}, null));
		List<OrganizationMemberItem> dataArray16 = _dataArray;
		int[] monasticTitleSuffixes16 = new int[2] { 848, 849 };
		List<short> favoriteClothingIds16 = new List<short> { 61, 62, 63, 7, 8, 15, 16, 17 };
		List<short> hatedClothingIds16 = new List<short> { 21, 22, 23 };
		int[] spouseAnonymousTitles16 = new int[2] { 850, 851 };
		short[] initialAges16 = new short[4] { 14, 16, 18, 20 };
		PresetEquipmentItemWithProb[] equipment16 = new PresetEquipmentItemWithProb[8]
		{
			new PresetEquipmentItemWithProb("Armor", 45, 75),
			new PresetEquipmentItemWithProb("Armor", 306, 100),
			new PresetEquipmentItemWithProb("Armor", 432, 75),
			new PresetEquipmentItemWithProb("Armor", 180, 75),
			new PresetEquipmentItemWithProb("Accessory", 117, 75),
			new PresetEquipmentItemWithProb("Accessory", 153, 50),
			new PresetEquipmentItemWithProb("Accessory", 81, 50),
			new PresetEquipmentItemWithProb("Carrier", 0, 75)
		};
		PresetEquipmentItem clothing16 = new PresetEquipmentItem("Clothing", 61);
		List<PresetInventoryItem> inventory16 = new List<PresetInventoryItem>
		{
			new PresetInventoryItem("SkillBook", 135, 1, 10),
			new PresetInventoryItem("Misc", 0, 3, 20),
			new PresetInventoryItem("CraftTool", 45, 1, 20),
			new PresetInventoryItem("Food", 9, 3, 20),
			new PresetInventoryItem("Food", 51, 3, 20),
			new PresetInventoryItem("Food", 135, 3, 20),
			new PresetInventoryItem("Medicine", 0, 1, 10),
			new PresetInventoryItem("Medicine", 27, 1, 10),
			new PresetInventoryItem("Medicine", 36, 1, 10)
		};
		List<PresetOrgMemberCombatSkill> combatSkills16 = new List<PresetOrgMemberCombatSkill>
		{
			new PresetOrgMemberCombatSkill(97, 2),
			new PresetOrgMemberCombatSkill(196, 2),
			new PresetOrgMemberCombatSkill(312, 2),
			new PresetOrgMemberCombatSkill(392, 2),
			new PresetOrgMemberCombatSkill(518, 2)
		};
		sbyte[] extraCombatSkillGrids16 = new sbyte[5] { 2, 2, 2, 2, 2 };
		short[] resourcesAdjust16 = new short[8] { -70, -70, -70, -60, -80, -70, -60, -60 };
		short[] lifeSkillsAdjust16 = new short[16]
		{
			-1, -1, -1, -1, -1, 9, 2, 2, 9, 9,
			-1, -1, 9, -1, -1, 12
		};
		short[] combatSkillsAdjust16 = new short[14]
		{
			12, 6, 12, 12, 9, 12, 9, -1, -1, -1,
			-1, -1, -1, -1
		};
		short[] mainAttributesAdjust16 = new short[6] { 12, 12, 2, 9, -1, -1 };
		identityInteractConfig = new List<sbyte>();
		dataArray16.Add(new OrganizationMemberItem(178, 847, 15, 2, 8, 12, 4, restrictPrincipalAmount: false, -1, -1, -1, 1, 2, 4, 2, 0, 0, monasticTitleSuffixes16, 600, 4, 2000, 900, 0, favoriteClothingIds16, hatedClothingIds16, spouseAnonymousTitles16, canStroll: true, 349, initialAges16, equipment16, clothing16, inventory16, combatSkills16, extraCombatSkillGrids16, resourcesAdjust16, 15000, 4000, 5, 80, 1800, lifeSkillsAdjust16, 3, combatSkillsAdjust16, mainAttributesAdjust16, identityInteractConfig, 3, new ResourceInts(default(int), default(int), default(int), default(int), default(int), default(int), default(int), default(int)), new IntPair[18]
		{
			new IntPair(17, 18),
			new IntPair(8, 9),
			new IntPair(5, 1),
			new IntPair(6, 1),
			new IntPair(15, 3),
			new IntPair(16, 3),
			new IntPair(7, 3),
			new IntPair(14, 1),
			new IntPair(12, 1),
			new IntPair(4, 1),
			new IntPair(3, 15),
			new IntPair(13, 1),
			new IntPair(10, 1),
			new IntPair(2, 1),
			new IntPair(1, 3),
			new IntPair(11, 3),
			new IntPair(0, 1),
			new IntPair(9, 42)
		}, null));
		List<OrganizationMemberItem> dataArray17 = _dataArray;
		int[] monasticTitleSuffixes17 = new int[2] { 852, 853 };
		List<short> favoriteClothingIds17 = new List<short> { 61, 62, 63, 7, 8, 15, 16, 17 };
		List<short> hatedClothingIds17 = new List<short> { 21, 22, 23 };
		int[] spouseAnonymousTitles17 = new int[2] { 854, 855 };
		short[] initialAges17 = new short[4] { 12, 13, 14, 15 };
		PresetEquipmentItemWithProb[] equipment17 = new PresetEquipmentItemWithProb[8]
		{
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Armor", 306, 100),
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Armor", 180, 75),
			new PresetEquipmentItemWithProb("Accessory", 117, 75),
			new PresetEquipmentItemWithProb("Accessory", 153, 50),
			new PresetEquipmentItemWithProb("Accessory", -1, 0),
			new PresetEquipmentItemWithProb("Carrier", -1, 0)
		};
		PresetEquipmentItem clothing17 = new PresetEquipmentItem("Clothing", 61);
		List<PresetInventoryItem> inventory17 = new List<PresetInventoryItem>
		{
			new PresetInventoryItem("SkillBook", 135, 1, 10),
			new PresetInventoryItem("Misc", 0, 3, 20),
			new PresetInventoryItem("CraftTool", 45, 1, 20),
			new PresetInventoryItem("Food", 9, 3, 20),
			new PresetInventoryItem("Food", 51, 3, 20),
			new PresetInventoryItem("Food", 135, 3, 20),
			new PresetInventoryItem("Medicine", 0, 1, 10),
			new PresetInventoryItem("Medicine", 27, 1, 10),
			new PresetInventoryItem("Medicine", 36, 1, 10)
		};
		List<PresetOrgMemberCombatSkill> combatSkills17 = new List<PresetOrgMemberCombatSkill>
		{
			new PresetOrgMemberCombatSkill(97, 1),
			new PresetOrgMemberCombatSkill(196, 1),
			new PresetOrgMemberCombatSkill(312, 1),
			new PresetOrgMemberCombatSkill(486, 1),
			new PresetOrgMemberCombatSkill(518, 1)
		};
		sbyte[] extraCombatSkillGrids17 = new sbyte[5];
		short[] resourcesAdjust17 = new short[8] { -70, -70, -70, -60, -80, -70, -60, -60 };
		short[] lifeSkillsAdjust17 = new short[16]
		{
			-1, -1, -1, -1, -1, 9, 2, 2, 9, 9,
			-1, -1, 9, -1, -1, 12
		};
		short[] combatSkillsAdjust17 = new short[14]
		{
			12, 6, 12, 12, 9, 12, 9, -1, -1, -1,
			-1, -1, -1, -1
		};
		short[] mainAttributesAdjust17 = new short[6] { 12, 12, 2, 9, -1, -1 };
		identityInteractConfig = new List<sbyte>();
		dataArray17.Add(new OrganizationMemberItem(179, 725, 15, 1, 12, 18, 6, restrictPrincipalAmount: false, -1, -1, -1, 1, 1, 3, 1, 0, 0, monasticTitleSuffixes17, 300, 2, 1000, 300, 0, favoriteClothingIds17, hatedClothingIds17, spouseAnonymousTitles17, canStroll: true, 350, initialAges17, equipment17, clothing17, inventory17, combatSkills17, extraCombatSkillGrids17, resourcesAdjust17, 10000, 2000, 5, 90, 600, lifeSkillsAdjust17, 2, combatSkillsAdjust17, mainAttributesAdjust17, identityInteractConfig, 3, new ResourceInts(default(int), default(int), default(int), default(int), default(int), default(int), default(int), default(int)), new IntPair[18]
		{
			new IntPair(17, 18),
			new IntPair(8, 9),
			new IntPair(5, 1),
			new IntPair(6, 1),
			new IntPair(15, 3),
			new IntPair(16, 3),
			new IntPair(7, 3),
			new IntPair(14, 1),
			new IntPair(12, 1),
			new IntPair(4, 1),
			new IntPair(3, 15),
			new IntPair(13, 1),
			new IntPair(10, 1),
			new IntPair(2, 1),
			new IntPair(1, 3),
			new IntPair(11, 3),
			new IntPair(0, 1),
			new IntPair(9, 42)
		}, null));
	}

	private void CreateItems3()
	{
		List<OrganizationMemberItem> dataArray = _dataArray;
		int[] monasticTitleSuffixes = new int[2] { 857, 858 };
		List<short> favoriteClothingIds = new List<short> { 61, 62, 63, 7, 8, 15, 16, 17 };
		List<short> hatedClothingIds = new List<short> { 21, 22, 23 };
		int[] spouseAnonymousTitles = new int[2] { 859, 860 };
		short[] initialAges = new short[4] { 10, 10, 10, 10 };
		PresetEquipmentItemWithProb[] equipment = new PresetEquipmentItemWithProb[8]
		{
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Armor", 306, 100),
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Armor", 180, 75),
			new PresetEquipmentItemWithProb("Accessory", 117, 75),
			new PresetEquipmentItemWithProb("Accessory", -1, 0),
			new PresetEquipmentItemWithProb("Accessory", -1, 0),
			new PresetEquipmentItemWithProb("Carrier", -1, 0)
		};
		PresetEquipmentItem clothing = new PresetEquipmentItem("Clothing", 61);
		List<PresetInventoryItem> inventory = new List<PresetInventoryItem>
		{
			new PresetInventoryItem("SkillBook", 135, 1, 10),
			new PresetInventoryItem("Misc", 0, 3, 20),
			new PresetInventoryItem("CraftTool", 45, 1, 20),
			new PresetInventoryItem("Food", 9, 3, 20),
			new PresetInventoryItem("Food", 51, 3, 20),
			new PresetInventoryItem("Food", 135, 3, 20),
			new PresetInventoryItem("Medicine", 0, 1, 10),
			new PresetInventoryItem("Medicine", 27, 1, 10),
			new PresetInventoryItem("Medicine", 36, 1, 10)
		};
		List<PresetOrgMemberCombatSkill> combatSkills = new List<PresetOrgMemberCombatSkill>
		{
			new PresetOrgMemberCombatSkill(97, 0),
			new PresetOrgMemberCombatSkill(196, 0),
			new PresetOrgMemberCombatSkill(392, 0)
		};
		sbyte[] extraCombatSkillGrids = new sbyte[5];
		short[] resourcesAdjust = new short[8] { -70, -70, -70, -60, -80, -70, -60, -60 };
		short[] lifeSkillsAdjust = new short[16]
		{
			-1, -1, -1, -1, -1, 9, 2, 2, 9, 9,
			-1, -1, 9, -1, -1, 12
		};
		short[] combatSkillsAdjust = new short[14]
		{
			12, 6, 12, 12, 9, 12, 9, -1, -1, -1,
			-1, -1, -1, -1
		};
		short[] mainAttributesAdjust = new short[6] { 12, 12, 2, 9, -1, -1 };
		List<sbyte> identityInteractConfig = new List<sbyte>();
		dataArray.Add(new OrganizationMemberItem(180, 856, 15, 0, 8, 12, 4, restrictPrincipalAmount: false, -1, -1, -1, 0, 0, 2, 0, 0, 0, monasticTitleSuffixes, 150, 0, 500, 150, 0, favoriteClothingIds, hatedClothingIds, spouseAnonymousTitles, canStroll: true, 351, initialAges, equipment, clothing, inventory, combatSkills, extraCombatSkillGrids, resourcesAdjust, 5000, 1000, 1, 100, 300, lifeSkillsAdjust, 1, combatSkillsAdjust, mainAttributesAdjust, identityInteractConfig, 3, new ResourceInts(default(int), default(int), default(int), default(int), default(int), default(int), default(int), default(int)), new IntPair[18]
		{
			new IntPair(17, 18),
			new IntPair(8, 9),
			new IntPair(5, 1),
			new IntPair(6, 1),
			new IntPair(15, 3),
			new IntPair(16, 3),
			new IntPair(7, 3),
			new IntPair(14, 1),
			new IntPair(12, 1),
			new IntPair(4, 1),
			new IntPair(3, 15),
			new IntPair(13, 1),
			new IntPair(10, 1),
			new IntPair(2, 1),
			new IntPair(1, 3),
			new IntPair(11, 3),
			new IntPair(0, 1),
			new IntPair(9, 42)
		}, null));
		List<OrganizationMemberItem> dataArray2 = _dataArray;
		int[] monasticTitleSuffixes2 = new int[2] { 862, 863 };
		List<short> favoriteClothingIds2 = new List<short> { 7, 8, 16, 17 };
		List<short> hatedClothingIds2 = new List<short> { 0, 1, 2, 3, 9, 10, 11, 12 };
		int[] spouseAnonymousTitles2 = new int[2] { 864, 865 };
		short[] initialAges2 = new short[4] { 28, 37, 46, 55 };
		PresetEquipmentItemWithProb[] equipment2 = new PresetEquipmentItemWithProb[8]
		{
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Accessory", -1, 0),
			new PresetEquipmentItemWithProb("Accessory", -1, 0),
			new PresetEquipmentItemWithProb("Accessory", -1, 0),
			new PresetEquipmentItemWithProb("Carrier", 0, 75)
		};
		PresetEquipmentItem clothing2 = new PresetEquipmentItem("Clothing", 17);
		List<PresetInventoryItem> inventory2 = new List<PresetInventoryItem>
		{
			new PresetInventoryItem("Misc", 82, 1, 30),
			new PresetInventoryItem("Food", 9, 3, 40),
			new PresetInventoryItem("Food", 51, 3, 40),
			new PresetInventoryItem("Food", 135, 3, 40),
			new PresetInventoryItem("Medicine", 100, 1, 20),
			new PresetInventoryItem("TeaWine", 18, 1, 20),
			new PresetInventoryItem("TeaWine", 27, 1, 20),
			new PresetInventoryItem("TeaWine", 0, 1, 20),
			new PresetInventoryItem("TeaWine", 9, 1, 20)
		};
		List<PresetOrgMemberCombatSkill> combatSkills2 = new List<PresetOrgMemberCombatSkill>();
		dataArray2.Add(new OrganizationMemberItem(181, 861, -1, 8, 1, 1, 1, restrictPrincipalAmount: true, -1, -1, 7, 6, 7, -1, 7, 0, 0, monasticTitleSuffixes2, 0, 0, 13000, 21150, -100, favoriteClothingIds2, hatedClothingIds2, spouseAnonymousTitles2, canStroll: false, -1, initialAges2, equipment2, clothing2, inventory2, combatSkills2, new sbyte[5], new short[8] { -60, -60, -60, -60, -60, -60, -60, -60 }, 160000, 576000, 20, 20, 61500, new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		}, 6, new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		}, new short[6] { -1, -1, -1, -1, -1, -1 }, new List<sbyte> { 0, 55 }, 3, new ResourceInts(default(int), default(int), default(int), default(int), default(int), default(int), default(int), default(int)), new IntPair[18]
		{
			new IntPair(17, 30),
			new IntPair(8, 30),
			new IntPair(5, 1),
			new IntPair(6, 1),
			new IntPair(15, 6),
			new IntPair(16, 9),
			new IntPair(7, 9),
			new IntPair(14, 1),
			new IntPair(12, 1),
			new IntPair(4, 6),
			new IntPair(3, 1),
			new IntPair(13, 1),
			new IntPair(10, 3),
			new IntPair(2, 3),
			new IntPair(1, 1),
			new IntPair(11, 1),
			new IntPair(0, 1),
			new IntPair(9, 3)
		}, null));
		List<OrganizationMemberItem> dataArray3 = _dataArray;
		int[] monasticTitleSuffixes3 = new int[2] { 867, 868 };
		List<short> favoriteClothingIds3 = new List<short> { 6, 7, 8, 15, 16, 17 };
		List<short> hatedClothingIds3 = new List<short> { 0, 1, 2, 9, 10, 11 };
		int[] spouseAnonymousTitles3 = new int[2] { 869, 870 };
		short[] initialAges3 = new short[4] { 26, 34, 42, 50 };
		PresetEquipmentItemWithProb[] equipment3 = new PresetEquipmentItemWithProb[8]
		{
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Accessory", -1, 0),
			new PresetEquipmentItemWithProb("Accessory", -1, 0),
			new PresetEquipmentItemWithProb("Accessory", -1, 0),
			new PresetEquipmentItemWithProb("Carrier", 18, 75)
		};
		PresetEquipmentItem clothing3 = new PresetEquipmentItem("Clothing", 8);
		List<PresetInventoryItem> inventory3 = new List<PresetInventoryItem>
		{
			new PresetInventoryItem("Food", 9, 3, 40),
			new PresetInventoryItem("Food", 51, 3, 40),
			new PresetInventoryItem("Food", 135, 3, 40),
			new PresetInventoryItem("Medicine", 100, 1, 20),
			new PresetInventoryItem("TeaWine", 18, 1, 20),
			new PresetInventoryItem("TeaWine", 27, 1, 20),
			new PresetInventoryItem("TeaWine", 0, 1, 20),
			new PresetInventoryItem("TeaWine", 9, 1, 20)
		};
		combatSkills2 = new List<PresetOrgMemberCombatSkill>();
		dataArray3.Add(new OrganizationMemberItem(182, 866, -1, 7, 2, 2, 2, restrictPrincipalAmount: false, -1, -1, -1, 5, 7, -1, 7, 0, 0, monasticTitleSuffixes3, 0, 0, 10000, 13800, -100, favoriteClothingIds3, hatedClothingIds3, spouseAnonymousTitles3, canStroll: false, -1, initialAges3, equipment3, clothing3, inventory3, combatSkills2, new sbyte[5], new short[8] { -70, -70, -70, -70, -70, -70, -70, -50 }, 100000, 288000, 15, 30, 42300, new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		}, 6, new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		}, new short[6] { -1, -1, -1, -1, -1, -1 }, new List<sbyte> { 52, 56 }, 3, new ResourceInts(default(int), default(int), default(int), default(int), default(int), default(int), default(int), default(int)), new IntPair[18]
		{
			new IntPair(17, 24),
			new IntPair(8, 24),
			new IntPair(5, 3),
			new IntPair(6, 3),
			new IntPair(15, 9),
			new IntPair(16, 9),
			new IntPair(7, 9),
			new IntPair(14, 1),
			new IntPair(12, 1),
			new IntPair(4, 6),
			new IntPair(3, 6),
			new IntPair(13, 1),
			new IntPair(10, 3),
			new IntPair(2, 3),
			new IntPair(1, 1),
			new IntPair(11, 1),
			new IntPair(0, 1),
			new IntPair(9, 3)
		}, null));
		List<OrganizationMemberItem> dataArray4 = _dataArray;
		int[] monasticTitleSuffixes4 = new int[2] { 872, 873 };
		List<short> favoriteClothingIds4 = new List<short> { 5, 6, 7, 8, 14, 15, 16, 17 };
		List<short> hatedClothingIds4 = new List<short> { 0, 1, 9, 10 };
		int[] spouseAnonymousTitles4 = new int[2] { 874, 875 };
		short[] initialAges4 = new short[4] { 24, 31, 38, 45 };
		PresetEquipmentItemWithProb[] equipment4 = new PresetEquipmentItemWithProb[8]
		{
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Accessory", -1, 0),
			new PresetEquipmentItemWithProb("Accessory", -1, 0),
			new PresetEquipmentItemWithProb("Accessory", -1, 0),
			new PresetEquipmentItemWithProb("Carrier", 0, 75)
		};
		PresetEquipmentItem clothing4 = new PresetEquipmentItem("Clothing", 16);
		List<PresetInventoryItem> inventory4 = new List<PresetInventoryItem>
		{
			new PresetInventoryItem("Food", 9, 3, 40),
			new PresetInventoryItem("Food", 51, 3, 40),
			new PresetInventoryItem("Food", 135, 3, 40),
			new PresetInventoryItem("Medicine", 94, 1, 20),
			new PresetInventoryItem("TeaWine", 18, 1, 20),
			new PresetInventoryItem("TeaWine", 27, 1, 20),
			new PresetInventoryItem("TeaWine", 0, 1, 20),
			new PresetInventoryItem("TeaWine", 9, 1, 20)
		};
		combatSkills2 = new List<PresetOrgMemberCombatSkill>();
		dataArray4.Add(new OrganizationMemberItem(183, 871, -1, 6, 2, 2, 2, restrictPrincipalAmount: false, -1, -1, -1, 4, 6, -1, 6, 0, 0, monasticTitleSuffixes4, 0, 0, 8000, 8400, -100, favoriteClothingIds4, hatedClothingIds4, spouseAnonymousTitles4, canStroll: false, -1, initialAges4, equipment4, clothing4, inventory4, combatSkills2, new sbyte[5], new short[8] { -70, -70, -70, -70, -70, -70, -50, -70 }, 60000, 144000, 15, 40, 27600, new short[16]
		{
			-1, -1, -1, -1, -1, 12, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		}, 6, new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		}, new short[6] { -1, -1, -1, -1, -1, -1 }, new List<sbyte> { 2, 57 }, 16, new ResourceInts(default(int), default(int), default(int), default(int), default(int), default(int), default(int), default(int)), new IntPair[18]
		{
			new IntPair(17, 21),
			new IntPair(8, 21),
			new IntPair(5, 1),
			new IntPair(6, 1),
			new IntPair(15, 15),
			new IntPair(16, 15),
			new IntPair(7, 15),
			new IntPair(14, 1),
			new IntPair(12, 1),
			new IntPair(4, 3),
			new IntPair(3, 1),
			new IntPair(13, 1),
			new IntPair(10, 3),
			new IntPair(2, 3),
			new IntPair(1, 1),
			new IntPair(11, 1),
			new IntPair(0, 1),
			new IntPair(9, 3)
		}, null));
		List<OrganizationMemberItem> dataArray5 = _dataArray;
		int[] monasticTitleSuffixes5 = new int[2] { 876, 877 };
		List<short> favoriteClothingIds5 = new List<short> { 4 };
		List<short> list = new List<short>();
		List<short> hatedClothingIds5 = list;
		int[] spouseAnonymousTitles5 = new int[2] { 878, 879 };
		short[] initialAges5 = new short[4] { 22, 28, 34, 40 };
		PresetEquipmentItemWithProb[] equipment5 = new PresetEquipmentItemWithProb[8]
		{
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Accessory", -1, 0),
			new PresetEquipmentItemWithProb("Accessory", -1, 0),
			new PresetEquipmentItemWithProb("Accessory", -1, 0),
			new PresetEquipmentItemWithProb("Carrier", 18, 75)
		};
		PresetEquipmentItem clothing5 = new PresetEquipmentItem("Clothing", 4);
		List<PresetInventoryItem> inventory5 = new List<PresetInventoryItem>
		{
			new PresetInventoryItem("Food", 0, 3, 40),
			new PresetInventoryItem("Food", 93, 3, 40),
			new PresetInventoryItem("Food", 135, 3, 40),
			new PresetInventoryItem("TeaWine", 18, 1, 20),
			new PresetInventoryItem("TeaWine", 27, 1, 20),
			new PresetInventoryItem("TeaWine", 0, 1, 20),
			new PresetInventoryItem("TeaWine", 9, 1, 20)
		};
		combatSkills2 = new List<PresetOrgMemberCombatSkill>();
		dataArray5.Add(new OrganizationMemberItem(184, 57, -1, 5, 2, 2, 2, restrictPrincipalAmount: false, -1, -1, -1, 5, 5, -1, 5, 0, 0, monasticTitleSuffixes5, 0, 0, 6000, 4650, -75, favoriteClothingIds5, hatedClothingIds5, spouseAnonymousTitles5, canStroll: true, -1, initialAges5, equipment5, clothing5, inventory5, combatSkills2, new sbyte[5], new short[8] { -80, -80, -80, -80, -80, -80, -80, -50 }, 40000, 48000, 10, 50, 16800, new short[16]
		{
			12, 12, 12, 12, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		}, 7, new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		}, new short[6] { -1, -1, -1, -1, -1, -1 }, new List<sbyte> { 3, 58, 66 }, 16, new ResourceInts(default(int), default(int), default(int), default(int), default(int), default(int), default(int), default(int)), new IntPair[18]
		{
			new IntPair(17, 3),
			new IntPair(8, 21),
			new IntPair(5, 1),
			new IntPair(6, 1),
			new IntPair(15, 3),
			new IntPair(16, 21),
			new IntPair(7, 6),
			new IntPair(14, 3),
			new IntPair(12, 3),
			new IntPair(4, 30),
			new IntPair(3, 1),
			new IntPair(13, 3),
			new IntPair(10, 3),
			new IntPair(2, 3),
			new IntPair(1, 1),
			new IntPair(11, 3),
			new IntPair(0, 1),
			new IntPair(9, 1)
		}, null));
		List<OrganizationMemberItem> dataArray6 = _dataArray;
		int[] monasticTitleSuffixes6 = new int[2] { 880, 881 };
		List<short> favoriteClothingIds6 = new List<short> { 15 };
		list = new List<short>();
		List<short> hatedClothingIds6 = list;
		int[] spouseAnonymousTitles6 = new int[2] { 882, 883 };
		short[] initialAges6 = new short[4] { 20, 25, 30, 35 };
		PresetEquipmentItemWithProb[] equipment6 = new PresetEquipmentItemWithProb[8]
		{
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Accessory", -1, 0),
			new PresetEquipmentItemWithProb("Accessory", -1, 0),
			new PresetEquipmentItemWithProb("Accessory", -1, 0),
			new PresetEquipmentItemWithProb("Carrier", 0, 75)
		};
		PresetEquipmentItem clothing6 = new PresetEquipmentItem("Clothing", 15);
		List<PresetInventoryItem> inventory6 = new List<PresetInventoryItem>
		{
			new PresetInventoryItem("Food", 9, 3, 40),
			new PresetInventoryItem("Food", 135, 3, 40),
			new PresetInventoryItem("TeaWine", 18, 1, 20),
			new PresetInventoryItem("TeaWine", 27, 1, 20),
			new PresetInventoryItem("TeaWine", 0, 1, 20),
			new PresetInventoryItem("TeaWine", 9, 1, 20)
		};
		combatSkills2 = new List<PresetOrgMemberCombatSkill>();
		dataArray6.Add(new OrganizationMemberItem(185, 62, -1, 4, 4, 4, 4, restrictPrincipalAmount: false, -1, -1, -1, 4, 4, -1, 4, 0, 0, monasticTitleSuffixes6, 0, 0, 4500, 2250, -50, favoriteClothingIds6, hatedClothingIds6, spouseAnonymousTitles6, canStroll: true, -1, initialAges6, equipment6, clothing6, inventory6, combatSkills2, new sbyte[5], new short[8] { -80, -80, -80, -80, -80, -80, -50, -80 }, 30000, 24000, 10, 60, 9300, new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, 9
		}, 5, new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		}, new short[6] { -1, -1, -1, -1, -1, -1 }, new List<sbyte> { 4, 53 }, 14, new ResourceInts(default(int), default(int), default(int), default(int), default(int), default(int), default(int), default(int)), new IntPair[18]
		{
			new IntPair(17, 3),
			new IntPair(8, 3),
			new IntPair(5, 1),
			new IntPair(6, 1),
			new IntPair(15, 30),
			new IntPair(16, 15),
			new IntPair(7, 15),
			new IntPair(14, 1),
			new IntPair(12, 1),
			new IntPair(4, 6),
			new IntPair(3, 1),
			new IntPair(13, 3),
			new IntPair(10, 9),
			new IntPair(2, 3),
			new IntPair(1, 3),
			new IntPair(11, 9),
			new IntPair(0, 3),
			new IntPair(9, 1)
		}, null));
		List<OrganizationMemberItem> dataArray7 = _dataArray;
		int[] monasticTitleSuffixes7 = new int[2] { 884, 885 };
		List<short> favoriteClothingIds7 = new List<short> { 13 };
		list = new List<short>();
		List<short> hatedClothingIds7 = list;
		int[] spouseAnonymousTitles7 = new int[2] { 886, 887 };
		short[] initialAges7 = new short[4] { 18, 22, 26, 30 };
		PresetEquipmentItemWithProb[] equipment7 = new PresetEquipmentItemWithProb[8]
		{
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Accessory", -1, 0),
			new PresetEquipmentItemWithProb("Accessory", -1, 0),
			new PresetEquipmentItemWithProb("Accessory", -1, 0),
			new PresetEquipmentItemWithProb("Carrier", 0, 75)
		};
		PresetEquipmentItem clothing7 = new PresetEquipmentItem("Clothing", 13);
		List<PresetInventoryItem> inventory7 = new List<PresetInventoryItem>
		{
			new PresetInventoryItem("CraftTool", 45, 1, 30),
			new PresetInventoryItem("Food", 135, 3, 40),
			new PresetInventoryItem("Medicine", 54, 1, 20),
			new PresetInventoryItem("Medicine", 66, 1, 20),
			new PresetInventoryItem("Medicine", 82, 1, 20),
			new PresetInventoryItem("Medicine", 94, 1, 20),
			new PresetInventoryItem("Material", 140, 1, 10),
			new PresetInventoryItem("Material", 144, 1, 10),
			new PresetInventoryItem("Material", 148, 1, 10),
			new PresetInventoryItem("Material", 152, 1, 10),
			new PresetInventoryItem("Material", 156, 1, 10),
			new PresetInventoryItem("Material", 160, 1, 10),
			new PresetInventoryItem("Material", 164, 1, 10),
			new PresetInventoryItem("Material", 168, 1, 10),
			new PresetInventoryItem("Material", 172, 1, 10),
			new PresetInventoryItem("Material", 176, 1, 10),
			new PresetInventoryItem("Material", 180, 1, 10),
			new PresetInventoryItem("Material", 184, 1, 10),
			new PresetInventoryItem("Material", 188, 1, 10),
			new PresetInventoryItem("Material", 192, 1, 10),
			new PresetInventoryItem("Material", 196, 1, 10),
			new PresetInventoryItem("Material", 200, 1, 10),
			new PresetInventoryItem("Material", 204, 1, 10),
			new PresetInventoryItem("Material", 208, 1, 10),
			new PresetInventoryItem("Material", 212, 1, 10),
			new PresetInventoryItem("Material", 216, 1, 10),
			new PresetInventoryItem("Material", 220, 1, 10),
			new PresetInventoryItem("Material", 224, 1, 10),
			new PresetInventoryItem("Material", 228, 1, 10),
			new PresetInventoryItem("Material", 232, 1, 10)
		};
		combatSkills2 = new List<PresetOrgMemberCombatSkill>();
		dataArray7.Add(new OrganizationMemberItem(186, 67, -1, 3, 4, 4, 4, restrictPrincipalAmount: false, -1, -1, -1, 3, 3, -1, 3, 0, 0, monasticTitleSuffixes7, 0, 0, 3000, 900, -25, favoriteClothingIds7, hatedClothingIds7, spouseAnonymousTitles7, canStroll: true, -1, initialAges7, equipment7, clothing7, inventory7, combatSkills2, new sbyte[5], new short[8] { -80, -80, -80, -80, -80, -50, -80, -80 }, 20000, 12000, 10, 70, 4500, new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, 12, 12,
			-1, -1, -1, -1, -1, -1
		}, 7, new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		}, new short[6] { -1, -1, -1, -1, -1, -1 }, new List<sbyte> { 5, 54, 64 }, 14, new ResourceInts(default(int), default(int), default(int), default(int), default(int), default(int), default(int), default(int)), new IntPair[18]
		{
			new IntPair(17, 1),
			new IntPair(8, 6),
			new IntPair(5, 1),
			new IntPair(6, 1),
			new IntPair(15, 1),
			new IntPair(16, 6),
			new IntPair(7, 1),
			new IntPair(14, 3),
			new IntPair(12, 3),
			new IntPair(4, 12),
			new IntPair(3, 1),
			new IntPair(13, 30),
			new IntPair(10, 18),
			new IntPair(2, 1),
			new IntPair(1, 1),
			new IntPair(11, 9),
			new IntPair(0, 12),
			new IntPair(9, 1)
		}, null));
		List<OrganizationMemberItem> dataArray8 = _dataArray;
		int[] monasticTitleSuffixes8 = new int[2] { 889, 890 };
		List<short> favoriteClothingIds8 = new List<short> { 0, 1, 2, 9, 10, 11 };
		List<short> hatedClothingIds8 = new List<short> { 7, 8, 16, 17 };
		int[] spouseAnonymousTitles8 = new int[2] { 891, 892 };
		short[] initialAges8 = new short[4] { 16, 19, 22, 25 };
		PresetEquipmentItemWithProb[] equipment8 = new PresetEquipmentItemWithProb[8]
		{
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Accessory", -1, 0),
			new PresetEquipmentItemWithProb("Accessory", -1, 0),
			new PresetEquipmentItemWithProb("Accessory", -1, 0),
			new PresetEquipmentItemWithProb("Carrier", 0, 75)
		};
		PresetEquipmentItem clothing8 = new PresetEquipmentItem("Clothing", 11);
		List<PresetInventoryItem> inventory8 = new List<PresetInventoryItem>
		{
			new PresetInventoryItem("CraftTool", 0, 1, 30),
			new PresetInventoryItem("CraftTool", 9, 1, 30),
			new PresetInventoryItem("CraftTool", 18, 1, 30),
			new PresetInventoryItem("CraftTool", 27, 1, 30),
			new PresetInventoryItem("Food", 93, 3, 40),
			new PresetInventoryItem("Material", 0, 1, 20),
			new PresetInventoryItem("Material", 7, 1, 20),
			new PresetInventoryItem("Material", 14, 1, 20),
			new PresetInventoryItem("Material", 21, 1, 20),
			new PresetInventoryItem("Material", 28, 1, 20),
			new PresetInventoryItem("Material", 35, 1, 20),
			new PresetInventoryItem("Material", 42, 1, 20),
			new PresetInventoryItem("Material", 49, 1, 20)
		};
		combatSkills2 = new List<PresetOrgMemberCombatSkill>();
		dataArray8.Add(new OrganizationMemberItem(187, 888, -1, 2, 4, 4, 4, restrictPrincipalAmount: false, -1, -1, -1, 2, 2, -1, 2, 0, 0, monasticTitleSuffixes8, 0, 0, 2000, 300, 0, favoriteClothingIds8, hatedClothingIds8, spouseAnonymousTitles8, canStroll: true, -1, initialAges8, equipment8, clothing8, inventory8, combatSkills2, new sbyte[5], new short[8] { -90, -50, -50, -50, -50, -90, -90, -90 }, 15000, 4000, 5, 80, 1800, new short[16]
		{
			-1, -1, -1, -1, -1, -1, 12, 12, -1, -1,
			12, 12, -1, -1, -1, -1
		}, 7, new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		}, new short[6] { -1, -1, -1, -1, -1, -1 }, new List<sbyte> { 6, 61, 63 }, 12, new ResourceInts(default(int), default(int), default(int), default(int), default(int), default(int), default(int), default(int)), new IntPair[18]
		{
			new IntPair(17, 1),
			new IntPair(8, 1),
			new IntPair(5, 1),
			new IntPair(6, 1),
			new IntPair(15, 9),
			new IntPair(16, 3),
			new IntPair(7, 1),
			new IntPair(14, 1),
			new IntPair(12, 1),
			new IntPair(4, 9),
			new IntPair(3, 1),
			new IntPair(13, 3),
			new IntPair(10, 15),
			new IntPair(2, 30),
			new IntPair(1, 9),
			new IntPair(11, 15),
			new IntPair(0, 6),
			new IntPair(9, 1)
		}, null));
		List<OrganizationMemberItem> dataArray9 = _dataArray;
		int[] monasticTitleSuffixes9 = new int[2] { 894, 895 };
		List<short> favoriteClothingIds9 = new List<short> { 0, 1, 9, 10 };
		List<short> hatedClothingIds9 = new List<short> { 6, 7, 8, 15, 16, 17 };
		int[] spouseAnonymousTitles9 = new int[2] { 896, 897 };
		short[] initialAges9 = new short[4] { 14, 16, 18, 20 };
		PresetEquipmentItemWithProb[] equipment9 = new PresetEquipmentItemWithProb[8]
		{
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Accessory", -1, 0),
			new PresetEquipmentItemWithProb("Accessory", -1, 0),
			new PresetEquipmentItemWithProb("Accessory", -1, 0),
			new PresetEquipmentItemWithProb("Carrier", 18, 75)
		};
		PresetEquipmentItem clothing9 = new PresetEquipmentItem("Clothing", 10);
		List<PresetInventoryItem> inventory9 = new List<PresetInventoryItem>
		{
			new PresetInventoryItem("CraftTool", 36, 1, 30),
			new PresetInventoryItem("Food", 0, 3, 40),
			new PresetInventoryItem("Food", 93, 3, 40),
			new PresetInventoryItem("Material", 57, 1, 20),
			new PresetInventoryItem("Material", 64, 1, 20),
			new PresetInventoryItem("Material", 71, 1, 20),
			new PresetInventoryItem("Material", 78, 1, 20)
		};
		combatSkills2 = new List<PresetOrgMemberCombatSkill>();
		dataArray9.Add(new OrganizationMemberItem(188, 893, -1, 1, 3, 3, 3, restrictPrincipalAmount: false, -1, -1, -1, 1, 1, -1, 1, 0, 0, monasticTitleSuffixes9, 0, 0, 1000, 150, 0, favoriteClothingIds9, hatedClothingIds9, spouseAnonymousTitles9, canStroll: true, -1, initialAges9, equipment9, clothing9, inventory9, combatSkills2, new sbyte[5], new short[8] { -50, -90, -90, -90, -90, -90, -90, -90 }, 10000, 2000, 5, 90, 600, new short[16]
		{
			-1, -1, -1, -1, -1, 12, -1, -1, -1, -1,
			-1, -1, -1, -1, 12, -1
		}, 4, new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		}, new short[6] { -1, -1, -1, -1, -1, -1 }, new List<sbyte> { 7, 60 }, 12, new ResourceInts(default(int), default(int), default(int), default(int), default(int), default(int), default(int), default(int)), new IntPair[18]
		{
			new IntPair(17, 3),
			new IntPair(8, 3),
			new IntPair(5, 1),
			new IntPair(6, 1),
			new IntPair(15, 6),
			new IntPair(16, 3),
			new IntPair(7, 3),
			new IntPair(14, 1),
			new IntPair(12, 1),
			new IntPair(4, 3),
			new IntPair(3, 1),
			new IntPair(13, 1),
			new IntPair(10, 30),
			new IntPair(2, 12),
			new IntPair(1, 9),
			new IntPair(11, 9),
			new IntPair(0, 3),
			new IntPair(9, 18)
		}, null));
		List<OrganizationMemberItem> dataArray10 = _dataArray;
		int[] monasticTitleSuffixes10 = new int[2] { 899, 900 };
		List<short> favoriteClothingIds10 = new List<short> { 0, 9 };
		List<short> hatedClothingIds10 = new List<short> { 5, 6, 7, 8, 14, 15, 16, 17 };
		int[] spouseAnonymousTitles10 = new int[2] { 901, 902 };
		short[] initialAges10 = new short[4] { 12, 13, 14, 15 };
		PresetEquipmentItemWithProb[] equipment10 = new PresetEquipmentItemWithProb[8]
		{
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Accessory", -1, 0),
			new PresetEquipmentItemWithProb("Accessory", -1, 0),
			new PresetEquipmentItemWithProb("Accessory", -1, 0),
			new PresetEquipmentItemWithProb("Carrier", -1, 0)
		};
		PresetEquipmentItem clothing10 = new PresetEquipmentItem("Clothing", 9);
		List<PresetInventoryItem> inventory10 = new List<PresetInventoryItem>
		{
			new PresetInventoryItem("Misc", 10, 1, 10),
			new PresetInventoryItem("Misc", 11, 1, 10),
			new PresetInventoryItem("Misc", 12, 1, 10),
			new PresetInventoryItem("Misc", 13, 1, 10),
			new PresetInventoryItem("Misc", 14, 1, 10),
			new PresetInventoryItem("Misc", 15, 1, 10),
			new PresetInventoryItem("Misc", 16, 1, 10),
			new PresetInventoryItem("Misc", 17, 1, 10),
			new PresetInventoryItem("Misc", 18, 1, 10),
			new PresetInventoryItem("Misc", 19, 1, 10),
			new PresetInventoryItem("Misc", 20, 1, 10),
			new PresetInventoryItem("Misc", 21, 1, 10),
			new PresetInventoryItem("Misc", 22, 1, 10),
			new PresetInventoryItem("Misc", 23, 1, 10),
			new PresetInventoryItem("Misc", 24, 1, 10),
			new PresetInventoryItem("Misc", 25, 1, 10),
			new PresetInventoryItem("Misc", 26, 1, 10),
			new PresetInventoryItem("Misc", 27, 1, 10)
		};
		combatSkills2 = new List<PresetOrgMemberCombatSkill>();
		dataArray10.Add(new OrganizationMemberItem(189, 898, -1, 0, 3, 3, 3, restrictPrincipalAmount: false, -1, -1, -1, 0, 0, -1, 0, 0, 0, monasticTitleSuffixes10, 0, 0, 500, 0, 0, favoriteClothingIds10, hatedClothingIds10, spouseAnonymousTitles10, canStroll: true, -1, initialAges10, equipment10, clothing10, inventory10, combatSkills2, new sbyte[5], new short[8] { -90, -90, -90, -90, -90, -90, -90, -90 }, 5000, 1000, 1, 100, 300, new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, 12
		}, 3, new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		}, new short[6] { -1, -1, -1, -1, -1, -1 }, new List<sbyte> { 8, 44 }, 3, new ResourceInts(default(int), default(int), default(int), default(int), default(int), default(int), default(int), default(int)), new IntPair[18]
		{
			new IntPair(17, 3),
			new IntPair(8, 3),
			new IntPair(5, 1),
			new IntPair(6, 1),
			new IntPair(15, 1),
			new IntPair(16, 3),
			new IntPair(7, 3),
			new IntPair(14, 1),
			new IntPair(12, 1),
			new IntPair(4, 3),
			new IntPair(3, 1),
			new IntPair(13, 3),
			new IntPair(10, 27),
			new IntPair(2, 6),
			new IntPair(1, 9),
			new IntPair(11, 9),
			new IntPair(0, 3),
			new IntPair(9, 30)
		}, null));
		List<OrganizationMemberItem> dataArray11 = _dataArray;
		int[] monasticTitleSuffixes11 = new int[2] { 904, 905 };
		List<short> favoriteClothingIds11 = new List<short> { 6, 7, 8, 15, 16, 17 };
		List<short> hatedClothingIds11 = new List<short> { 0, 1, 9, 10 };
		int[] spouseAnonymousTitles11 = new int[2] { 906, 907 };
		short[] initialAges11 = new short[4] { 28, 37, 46, 55 };
		PresetEquipmentItemWithProb[] equipment11 = new PresetEquipmentItemWithProb[8]
		{
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Accessory", -1, 0),
			new PresetEquipmentItemWithProb("Accessory", -1, 0),
			new PresetEquipmentItemWithProb("Accessory", -1, 0),
			new PresetEquipmentItemWithProb("Carrier", 18, 75)
		};
		PresetEquipmentItem clothing11 = new PresetEquipmentItem("Clothing", 13);
		List<PresetInventoryItem> inventory11 = new List<PresetInventoryItem>
		{
			new PresetInventoryItem("Misc", 82, 1, 30),
			new PresetInventoryItem("Food", 9, 3, 40),
			new PresetInventoryItem("Food", 51, 3, 40),
			new PresetInventoryItem("Food", 135, 3, 40),
			new PresetInventoryItem("Medicine", 100, 1, 20),
			new PresetInventoryItem("TeaWine", 18, 1, 20),
			new PresetInventoryItem("TeaWine", 27, 1, 20),
			new PresetInventoryItem("TeaWine", 0, 1, 20),
			new PresetInventoryItem("TeaWine", 9, 1, 20)
		};
		combatSkills2 = new List<PresetOrgMemberCombatSkill>();
		dataArray11.Add(new OrganizationMemberItem(190, 903, 36, 8, 1, 1, 1, restrictPrincipalAmount: true, -1, -1, 7, 6, 7, -1, 7, 0, 0, monasticTitleSuffixes11, 0, 0, 13000, 21150, -100, favoriteClothingIds11, hatedClothingIds11, spouseAnonymousTitles11, canStroll: false, -1, initialAges11, equipment11, clothing11, inventory11, combatSkills2, new sbyte[5], new short[8] { -60, -60, -60, -60, -60, -60, -60, -60 }, 80000, 288000, 20, 20, 61500, new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		}, 6, new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		}, new short[6] { -1, -1, -1, -1, -1, -1 }, new List<sbyte> { 0 }, 3, new ResourceInts(default(int), default(int), default(int), default(int), default(int), default(int), default(int), default(int)), new IntPair[18]
		{
			new IntPair(17, 18),
			new IntPair(8, 18),
			new IntPair(5, 1),
			new IntPair(6, 1),
			new IntPair(15, 6),
			new IntPair(16, 15),
			new IntPair(7, 15),
			new IntPair(14, 1),
			new IntPair(12, 1),
			new IntPair(4, 6),
			new IntPair(3, 1),
			new IntPair(13, 1),
			new IntPair(10, 6),
			new IntPair(2, 3),
			new IntPair(1, 6),
			new IntPair(11, 3),
			new IntPair(0, 3),
			new IntPair(9, 3)
		}, null));
		List<OrganizationMemberItem> dataArray12 = _dataArray;
		int[] monasticTitleSuffixes12 = new int[2] { 908, 909 };
		List<short> favoriteClothingIds12 = new List<short> { 5, 6, 7, 8, 14, 15, 16, 17 };
		List<short> hatedClothingIds12 = new List<short> { 0, 9 };
		int[] spouseAnonymousTitles12 = new int[2] { 910, 911 };
		short[] initialAges12 = new short[4] { 26, 34, 42, 50 };
		PresetEquipmentItemWithProb[] equipment12 = new PresetEquipmentItemWithProb[8]
		{
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Accessory", -1, 0),
			new PresetEquipmentItemWithProb("Accessory", -1, 0),
			new PresetEquipmentItemWithProb("Accessory", -1, 0),
			new PresetEquipmentItemWithProb("Carrier", 18, 75)
		};
		PresetEquipmentItem clothing12 = new PresetEquipmentItem("Clothing", 4);
		List<PresetInventoryItem> inventory12 = new List<PresetInventoryItem>
		{
			new PresetInventoryItem("Food", 9, 3, 40),
			new PresetInventoryItem("Food", 51, 3, 40),
			new PresetInventoryItem("Food", 135, 3, 40),
			new PresetInventoryItem("Medicine", 100, 1, 20),
			new PresetInventoryItem("TeaWine", 18, 1, 20),
			new PresetInventoryItem("TeaWine", 27, 1, 20),
			new PresetInventoryItem("TeaWine", 0, 1, 20),
			new PresetInventoryItem("TeaWine", 9, 1, 20)
		};
		combatSkills2 = new List<PresetOrgMemberCombatSkill>();
		dataArray12.Add(new OrganizationMemberItem(191, 866, 36, 7, 2, 2, 2, restrictPrincipalAmount: false, -1, -1, -1, 5, 7, -1, 7, 0, 0, monasticTitleSuffixes12, 0, 0, 10000, 13800, -100, favoriteClothingIds12, hatedClothingIds12, spouseAnonymousTitles12, canStroll: false, -1, initialAges12, equipment12, clothing12, inventory12, combatSkills2, new sbyte[5], new short[8] { -70, -70, -70, -70, -70, -70, -70, -50 }, 50000, 144000, 15, 30, 42300, new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		}, 6, new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		}, new short[6] { -1, -1, -1, -1, -1, -1 }, new List<sbyte> { 52, 56 }, 3, new ResourceInts(default(int), default(int), default(int), default(int), default(int), default(int), default(int), default(int)), new IntPair[18]
		{
			new IntPair(17, 15),
			new IntPair(8, 15),
			new IntPair(5, 3),
			new IntPair(6, 3),
			new IntPair(15, 9),
			new IntPair(16, 12),
			new IntPair(7, 12),
			new IntPair(14, 1),
			new IntPair(12, 1),
			new IntPair(4, 6),
			new IntPair(3, 6),
			new IntPair(13, 1),
			new IntPair(10, 6),
			new IntPair(2, 3),
			new IntPair(1, 6),
			new IntPair(11, 3),
			new IntPair(0, 3),
			new IntPair(9, 3)
		}, null));
		List<OrganizationMemberItem> dataArray13 = _dataArray;
		int[] monasticTitleSuffixes13 = new int[2] { 912, 913 };
		List<short> favoriteClothingIds13 = new List<short> { 4, 5, 6, 7, 8, 13, 14, 15, 16, 17 };
		list = new List<short>();
		List<short> hatedClothingIds13 = list;
		int[] spouseAnonymousTitles13 = new int[2] { 914, 915 };
		short[] initialAges13 = new short[4] { 24, 31, 38, 45 };
		PresetEquipmentItemWithProb[] equipment13 = new PresetEquipmentItemWithProb[8]
		{
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Accessory", -1, 0),
			new PresetEquipmentItemWithProb("Accessory", -1, 0),
			new PresetEquipmentItemWithProb("Accessory", -1, 0),
			new PresetEquipmentItemWithProb("Carrier", 18, 75)
		};
		PresetEquipmentItem clothing13 = new PresetEquipmentItem("Clothing", 13);
		List<PresetInventoryItem> inventory13 = new List<PresetInventoryItem>
		{
			new PresetInventoryItem("Food", 9, 3, 40),
			new PresetInventoryItem("Food", 51, 3, 40),
			new PresetInventoryItem("Food", 135, 3, 40),
			new PresetInventoryItem("Medicine", 94, 1, 20),
			new PresetInventoryItem("TeaWine", 18, 1, 20),
			new PresetInventoryItem("TeaWine", 27, 1, 20),
			new PresetInventoryItem("TeaWine", 0, 1, 20),
			new PresetInventoryItem("TeaWine", 9, 1, 20)
		};
		combatSkills2 = new List<PresetOrgMemberCombatSkill>();
		dataArray13.Add(new OrganizationMemberItem(192, 871, 36, 6, 2, 2, 2, restrictPrincipalAmount: false, -1, -1, -1, 4, 6, -1, 6, 0, 0, monasticTitleSuffixes13, 0, 0, 8000, 8400, -100, favoriteClothingIds13, hatedClothingIds13, spouseAnonymousTitles13, canStroll: false, -1, initialAges13, equipment13, clothing13, inventory13, combatSkills2, new sbyte[5], new short[8] { -70, -70, -70, -70, -70, -70, -50, -70 }, 30000, 72000, 15, 40, 27600, new short[16]
		{
			-1, -1, -1, -1, -1, 12, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		}, 6, new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		}, new short[6] { -1, -1, -1, -1, -1, -1 }, new List<sbyte> { 2, 57 }, 16, new ResourceInts(default(int), default(int), default(int), default(int), default(int), default(int), default(int), default(int)), new IntPair[18]
		{
			new IntPair(17, 12),
			new IntPair(8, 12),
			new IntPair(5, 1),
			new IntPair(6, 1),
			new IntPair(15, 15),
			new IntPair(16, 18),
			new IntPair(7, 18),
			new IntPair(14, 1),
			new IntPair(12, 1),
			new IntPair(4, 3),
			new IntPair(3, 1),
			new IntPair(13, 1),
			new IntPair(10, 6),
			new IntPair(2, 3),
			new IntPair(1, 6),
			new IntPair(11, 3),
			new IntPair(0, 3),
			new IntPair(9, 3)
		}, null));
		List<OrganizationMemberItem> dataArray14 = _dataArray;
		int[] monasticTitleSuffixes14 = new int[2] { 916, 917 };
		List<short> favoriteClothingIds14 = new List<short> { 4 };
		list = new List<short>();
		List<short> hatedClothingIds14 = list;
		int[] spouseAnonymousTitles14 = new int[2] { 918, 919 };
		short[] initialAges14 = new short[4] { 22, 28, 34, 40 };
		PresetEquipmentItemWithProb[] equipment14 = new PresetEquipmentItemWithProb[8]
		{
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Accessory", -1, 0),
			new PresetEquipmentItemWithProb("Accessory", -1, 0),
			new PresetEquipmentItemWithProb("Accessory", -1, 0),
			new PresetEquipmentItemWithProb("Carrier", 18, 75)
		};
		PresetEquipmentItem clothing14 = new PresetEquipmentItem("Clothing", 10);
		List<PresetInventoryItem> inventory14 = new List<PresetInventoryItem>
		{
			new PresetInventoryItem("Food", 0, 3, 40),
			new PresetInventoryItem("Food", 93, 3, 40),
			new PresetInventoryItem("Food", 135, 3, 40),
			new PresetInventoryItem("TeaWine", 18, 1, 20),
			new PresetInventoryItem("TeaWine", 27, 1, 20),
			new PresetInventoryItem("TeaWine", 0, 1, 20),
			new PresetInventoryItem("TeaWine", 9, 1, 20)
		};
		combatSkills2 = new List<PresetOrgMemberCombatSkill>();
		dataArray14.Add(new OrganizationMemberItem(193, 57, 36, 5, 2, 2, 2, restrictPrincipalAmount: false, -1, -1, -1, 5, 5, -1, 5, 0, 0, monasticTitleSuffixes14, 0, 0, 6000, 4650, -75, favoriteClothingIds14, hatedClothingIds14, spouseAnonymousTitles14, canStroll: true, -1, initialAges14, equipment14, clothing14, inventory14, combatSkills2, new sbyte[5], new short[8] { -80, -80, -80, -80, -80, -80, -80, -50 }, 10000, 24000, 10, 50, 16800, new short[16]
		{
			12, 12, 12, 12, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		}, 7, new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		}, new short[6] { -1, -1, -1, -1, -1, -1 }, new List<sbyte> { 3, 58, 66 }, 16, new ResourceInts(default(int), default(int), default(int), default(int), default(int), default(int), default(int), default(int)), new IntPair[18]
		{
			new IntPair(17, 3),
			new IntPair(8, 21),
			new IntPair(5, 1),
			new IntPair(6, 1),
			new IntPair(15, 3),
			new IntPair(16, 21),
			new IntPair(7, 6),
			new IntPair(14, 3),
			new IntPair(12, 3),
			new IntPair(4, 30),
			new IntPair(3, 1),
			new IntPair(13, 3),
			new IntPair(10, 3),
			new IntPair(2, 3),
			new IntPair(1, 1),
			new IntPair(11, 3),
			new IntPair(0, 1),
			new IntPair(9, 1)
		}, new sbyte[1] { 5 }));
		List<OrganizationMemberItem> dataArray15 = _dataArray;
		int[] monasticTitleSuffixes15 = new int[2] { 920, 921 };
		List<short> favoriteClothingIds15 = new List<short> { 15 };
		list = new List<short>();
		List<short> hatedClothingIds15 = list;
		int[] spouseAnonymousTitles15 = new int[2] { 922, 923 };
		short[] initialAges15 = new short[4] { 20, 25, 30, 35 };
		PresetEquipmentItemWithProb[] equipment15 = new PresetEquipmentItemWithProb[8]
		{
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Accessory", -1, 0),
			new PresetEquipmentItemWithProb("Accessory", -1, 0),
			new PresetEquipmentItemWithProb("Accessory", -1, 0),
			new PresetEquipmentItemWithProb("Carrier", 18, 75)
		};
		PresetEquipmentItem clothing15 = new PresetEquipmentItem("Clothing", 11);
		List<PresetInventoryItem> inventory15 = new List<PresetInventoryItem>
		{
			new PresetInventoryItem("Food", 9, 3, 40),
			new PresetInventoryItem("Food", 135, 3, 40),
			new PresetInventoryItem("TeaWine", 18, 1, 20),
			new PresetInventoryItem("TeaWine", 27, 1, 20),
			new PresetInventoryItem("TeaWine", 0, 1, 20),
			new PresetInventoryItem("TeaWine", 9, 1, 20)
		};
		combatSkills2 = new List<PresetOrgMemberCombatSkill>();
		dataArray15.Add(new OrganizationMemberItem(194, 62, 36, 4, 2, 2, 2, restrictPrincipalAmount: false, -1, -1, -1, 4, 4, -1, 4, 0, 0, monasticTitleSuffixes15, 0, 0, 4500, 2250, -50, favoriteClothingIds15, hatedClothingIds15, spouseAnonymousTitles15, canStroll: true, -1, initialAges15, equipment15, clothing15, inventory15, combatSkills2, new sbyte[5], new short[8] { -80, -80, -80, -80, -80, -80, -50, -80 }, 20000, 12000, 10, 60, 9300, new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, 9
		}, 5, new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		}, new short[6] { -1, -1, -1, -1, -1, -1 }, new List<sbyte> { 4, 53 }, 14, new ResourceInts(default(int), default(int), default(int), default(int), default(int), default(int), default(int), default(int)), new IntPair[18]
		{
			new IntPair(17, 3),
			new IntPair(8, 3),
			new IntPair(5, 1),
			new IntPair(6, 1),
			new IntPair(15, 30),
			new IntPair(16, 15),
			new IntPair(7, 15),
			new IntPair(14, 1),
			new IntPair(12, 1),
			new IntPair(4, 6),
			new IntPair(3, 1),
			new IntPair(13, 3),
			new IntPair(10, 9),
			new IntPair(2, 3),
			new IntPair(1, 3),
			new IntPair(11, 9),
			new IntPair(0, 3),
			new IntPair(9, 1)
		}, null));
		List<OrganizationMemberItem> dataArray16 = _dataArray;
		int[] monasticTitleSuffixes16 = new int[2] { 924, 925 };
		List<short> favoriteClothingIds16 = new List<short> { 13 };
		list = new List<short>();
		List<short> hatedClothingIds16 = list;
		int[] spouseAnonymousTitles16 = new int[2] { 926, 927 };
		short[] initialAges16 = new short[4] { 18, 22, 26, 30 };
		PresetEquipmentItemWithProb[] equipment16 = new PresetEquipmentItemWithProb[8]
		{
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Accessory", -1, 0),
			new PresetEquipmentItemWithProb("Accessory", -1, 0),
			new PresetEquipmentItemWithProb("Accessory", -1, 0),
			new PresetEquipmentItemWithProb("Carrier", 18, 75)
		};
		PresetEquipmentItem clothing16 = new PresetEquipmentItem("Clothing", 10);
		List<PresetInventoryItem> inventory16 = new List<PresetInventoryItem>
		{
			new PresetInventoryItem("CraftTool", 45, 1, 30),
			new PresetInventoryItem("Food", 135, 3, 40),
			new PresetInventoryItem("Medicine", 54, 1, 20),
			new PresetInventoryItem("Medicine", 66, 1, 20),
			new PresetInventoryItem("Medicine", 82, 1, 20),
			new PresetInventoryItem("Medicine", 94, 1, 20),
			new PresetInventoryItem("Material", 140, 1, 10),
			new PresetInventoryItem("Material", 144, 1, 10),
			new PresetInventoryItem("Material", 148, 1, 10),
			new PresetInventoryItem("Material", 152, 1, 10),
			new PresetInventoryItem("Material", 156, 1, 10),
			new PresetInventoryItem("Material", 160, 1, 10),
			new PresetInventoryItem("Material", 164, 1, 10),
			new PresetInventoryItem("Material", 168, 1, 10),
			new PresetInventoryItem("Material", 172, 1, 10),
			new PresetInventoryItem("Material", 176, 1, 10),
			new PresetInventoryItem("Material", 180, 1, 10),
			new PresetInventoryItem("Material", 184, 1, 10),
			new PresetInventoryItem("Material", 188, 1, 10),
			new PresetInventoryItem("Material", 192, 1, 10),
			new PresetInventoryItem("Material", 196, 1, 10),
			new PresetInventoryItem("Material", 200, 1, 10),
			new PresetInventoryItem("Material", 204, 1, 10),
			new PresetInventoryItem("Material", 208, 1, 10),
			new PresetInventoryItem("Material", 212, 1, 10),
			new PresetInventoryItem("Material", 216, 1, 10),
			new PresetInventoryItem("Material", 220, 1, 10),
			new PresetInventoryItem("Material", 224, 1, 10),
			new PresetInventoryItem("Material", 228, 1, 10),
			new PresetInventoryItem("Material", 232, 1, 10)
		};
		combatSkills2 = new List<PresetOrgMemberCombatSkill>();
		dataArray16.Add(new OrganizationMemberItem(195, 67, 36, 3, 2, 2, 2, restrictPrincipalAmount: false, -1, -1, -1, 3, 3, -1, 3, 0, 0, monasticTitleSuffixes16, 0, 0, 3000, 900, -25, favoriteClothingIds16, hatedClothingIds16, spouseAnonymousTitles16, canStroll: true, -1, initialAges16, equipment16, clothing16, inventory16, combatSkills2, new sbyte[5], new short[8] { -80, -80, -80, -80, -80, -50, -80, -80 }, 15000, 6000, 10, 70, 4500, new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, 12, 12,
			-1, -1, -1, -1, -1, -1
		}, 7, new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		}, new short[6] { -1, -1, -1, -1, -1, -1 }, new List<sbyte> { 5, 54, 64 }, 14, new ResourceInts(default(int), default(int), default(int), default(int), default(int), default(int), default(int), default(int)), new IntPair[18]
		{
			new IntPair(17, 1),
			new IntPair(8, 6),
			new IntPair(5, 1),
			new IntPair(6, 1),
			new IntPair(15, 1),
			new IntPair(16, 6),
			new IntPair(7, 1),
			new IntPair(14, 3),
			new IntPair(12, 3),
			new IntPair(4, 12),
			new IntPair(3, 1),
			new IntPair(13, 30),
			new IntPair(10, 18),
			new IntPair(2, 1),
			new IntPair(1, 1),
			new IntPair(11, 9),
			new IntPair(0, 12),
			new IntPair(9, 1)
		}, new sbyte[2] { 8, 9 }));
		List<OrganizationMemberItem> dataArray17 = _dataArray;
		int[] monasticTitleSuffixes17 = new int[2] { 928, 929 };
		List<short> favoriteClothingIds17 = new List<short> { 0, 1, 2, 3, 9, 10, 11, 12 };
		list = new List<short>();
		List<short> hatedClothingIds17 = list;
		int[] spouseAnonymousTitles17 = new int[2] { 930, 931 };
		short[] initialAges17 = new short[4] { 16, 19, 22, 25 };
		PresetEquipmentItemWithProb[] equipment17 = new PresetEquipmentItemWithProb[8]
		{
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Accessory", -1, 0),
			new PresetEquipmentItemWithProb("Accessory", -1, 0),
			new PresetEquipmentItemWithProb("Accessory", -1, 0),
			new PresetEquipmentItemWithProb("Carrier", 18, 75)
		};
		PresetEquipmentItem clothing17 = new PresetEquipmentItem("Clothing", 1);
		List<PresetInventoryItem> inventory17 = new List<PresetInventoryItem>
		{
			new PresetInventoryItem("CraftTool", 0, 1, 30),
			new PresetInventoryItem("CraftTool", 9, 1, 30),
			new PresetInventoryItem("CraftTool", 18, 1, 30),
			new PresetInventoryItem("CraftTool", 27, 1, 30),
			new PresetInventoryItem("Food", 93, 3, 40),
			new PresetInventoryItem("Material", 0, 1, 20),
			new PresetInventoryItem("Material", 7, 1, 20),
			new PresetInventoryItem("Material", 14, 1, 20),
			new PresetInventoryItem("Material", 21, 1, 20),
			new PresetInventoryItem("Material", 28, 1, 20),
			new PresetInventoryItem("Material", 35, 1, 20),
			new PresetInventoryItem("Material", 42, 1, 20),
			new PresetInventoryItem("Material", 49, 1, 20)
		};
		combatSkills2 = new List<PresetOrgMemberCombatSkill>();
		dataArray17.Add(new OrganizationMemberItem(196, 888, 36, 2, 2, 2, 2, restrictPrincipalAmount: false, -1, -1, -1, 2, 2, -1, 2, 0, 0, monasticTitleSuffixes17, 0, 0, 2000, 300, 0, favoriteClothingIds17, hatedClothingIds17, spouseAnonymousTitles17, canStroll: true, -1, initialAges17, equipment17, clothing17, inventory17, combatSkills2, new sbyte[5], new short[8] { -90, -50, -50, -50, -50, -90, -90, -90 }, 7500, 2000, 5, 80, 1800, new short[16]
		{
			-1, -1, -1, -1, -1, -1, 12, 12, -1, -1,
			12, 12, -1, -1, -1, -1
		}, 7, new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		}, new short[6] { -1, -1, -1, -1, -1, -1 }, new List<sbyte> { 6, 61, 63 }, 12, new ResourceInts(default(int), default(int), default(int), default(int), default(int), default(int), default(int), default(int)), new IntPair[18]
		{
			new IntPair(17, 1),
			new IntPair(8, 1),
			new IntPair(5, 1),
			new IntPair(6, 1),
			new IntPair(15, 6),
			new IntPair(16, 3),
			new IntPair(7, 1),
			new IntPair(14, 1),
			new IntPair(12, 1),
			new IntPair(4, 9),
			new IntPair(3, 1),
			new IntPair(13, 3),
			new IntPair(10, 18),
			new IntPair(2, 30),
			new IntPair(1, 9),
			new IntPair(11, 15),
			new IntPair(0, 6),
			new IntPair(9, 1)
		}, new sbyte[4] { 6, 7, 10, 11 }));
		List<OrganizationMemberItem> dataArray18 = _dataArray;
		int[] monasticTitleSuffixes18 = new int[2] { 932, 933 };
		List<short> favoriteClothingIds18 = new List<short> { 0, 1, 2, 9, 10, 11 };
		List<short> hatedClothingIds18 = new List<short> { 8, 17 };
		int[] spouseAnonymousTitles18 = new int[2] { 934, 935 };
		short[] initialAges18 = new short[4] { 14, 16, 18, 20 };
		PresetEquipmentItemWithProb[] equipment18 = new PresetEquipmentItemWithProb[8]
		{
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Accessory", -1, 0),
			new PresetEquipmentItemWithProb("Accessory", -1, 0),
			new PresetEquipmentItemWithProb("Accessory", -1, 0),
			new PresetEquipmentItemWithProb("Carrier", -1, 0)
		};
		PresetEquipmentItem clothing18 = new PresetEquipmentItem("Clothing", 0);
		List<PresetInventoryItem> inventory18 = new List<PresetInventoryItem>
		{
			new PresetInventoryItem("CraftTool", 36, 1, 30),
			new PresetInventoryItem("Food", 0, 3, 40),
			new PresetInventoryItem("Food", 93, 3, 40),
			new PresetInventoryItem("Material", 57, 1, 20),
			new PresetInventoryItem("Material", 64, 1, 20),
			new PresetInventoryItem("Material", 71, 1, 20),
			new PresetInventoryItem("Material", 78, 1, 20)
		};
		combatSkills2 = new List<PresetOrgMemberCombatSkill>();
		dataArray18.Add(new OrganizationMemberItem(197, 77, 36, 1, 2, 2, 2, restrictPrincipalAmount: false, -1, -1, -1, 1, 1, -1, 1, 0, 0, monasticTitleSuffixes18, 0, 0, 1000, 150, 0, favoriteClothingIds18, hatedClothingIds18, spouseAnonymousTitles18, canStroll: true, -1, initialAges18, equipment18, clothing18, inventory18, combatSkills2, new sbyte[5], new short[8] { -50, -90, -90, -90, -90, -90, -90, -90 }, 5000, 1000, 5, 90, 600, new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, 12, -1
		}, 4, new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		}, new short[6] { -1, -1, -1, -1, -1, -1 }, new List<sbyte> { 59, 62, 65 }, 12, new ResourceInts(default(int), default(int), default(int), default(int), default(int), default(int), default(int), default(int)), new IntPair[18]
		{
			new IntPair(17, 1),
			new IntPair(8, 1),
			new IntPair(5, 1),
			new IntPair(6, 1),
			new IntPair(15, 3),
			new IntPair(16, 1),
			new IntPair(7, 1),
			new IntPair(14, 1),
			new IntPair(12, 1),
			new IntPair(4, 3),
			new IntPair(3, 1),
			new IntPair(13, 3),
			new IntPair(10, 18),
			new IntPair(2, 6),
			new IntPair(1, 15),
			new IntPair(11, 15),
			new IntPair(0, 30),
			new IntPair(9, 6)
		}, new sbyte[1] { 14 }));
		List<OrganizationMemberItem> dataArray19 = _dataArray;
		int[] monasticTitleSuffixes19 = new int[2] { 936, 937 };
		List<short> favoriteClothingIds19 = new List<short> { 0, 1, 9, 10 };
		List<short> hatedClothingIds19 = new List<short> { 7, 8, 16, 17 };
		int[] spouseAnonymousTitles19 = new int[2] { 938, 939 };
		short[] initialAges19 = new short[4] { 12, 13, 14, 15 };
		PresetEquipmentItemWithProb[] equipment19 = new PresetEquipmentItemWithProb[8]
		{
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Accessory", -1, 0),
			new PresetEquipmentItemWithProb("Accessory", -1, 0),
			new PresetEquipmentItemWithProb("Accessory", -1, 0),
			new PresetEquipmentItemWithProb("Carrier", -1, 0)
		};
		PresetEquipmentItem clothing19 = new PresetEquipmentItem("Clothing", 9);
		List<PresetInventoryItem> inventory19 = new List<PresetInventoryItem>
		{
			new PresetInventoryItem("Misc", 10, 1, 10),
			new PresetInventoryItem("Misc", 11, 1, 10),
			new PresetInventoryItem("Misc", 12, 1, 10),
			new PresetInventoryItem("Misc", 13, 1, 10),
			new PresetInventoryItem("Misc", 14, 1, 10),
			new PresetInventoryItem("Misc", 15, 1, 10),
			new PresetInventoryItem("Misc", 16, 1, 10),
			new PresetInventoryItem("Misc", 17, 1, 10),
			new PresetInventoryItem("Misc", 18, 1, 10),
			new PresetInventoryItem("Misc", 19, 1, 10),
			new PresetInventoryItem("Misc", 20, 1, 10),
			new PresetInventoryItem("Misc", 21, 1, 10),
			new PresetInventoryItem("Misc", 22, 1, 10),
			new PresetInventoryItem("Misc", 23, 1, 10),
			new PresetInventoryItem("Misc", 24, 1, 10),
			new PresetInventoryItem("Misc", 25, 1, 10),
			new PresetInventoryItem("Misc", 26, 1, 10),
			new PresetInventoryItem("Misc", 27, 1, 10)
		};
		combatSkills2 = new List<PresetOrgMemberCombatSkill>();
		dataArray19.Add(new OrganizationMemberItem(198, 898, 36, 0, 2, 2, 2, restrictPrincipalAmount: false, -1, -1, -1, 0, 0, -1, 0, 0, 0, monasticTitleSuffixes19, 0, 0, 500, 0, 0, favoriteClothingIds19, hatedClothingIds19, spouseAnonymousTitles19, canStroll: true, -1, initialAges19, equipment19, clothing19, inventory19, combatSkills2, new sbyte[5], new short[8] { -90, -90, -90, -90, -90, -90, -90, -90 }, 500, 500, 1, 100, 300, new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, 12
		}, 3, new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		}, new short[6] { -1, -1, -1, -1, -1, -1 }, new List<sbyte> { 8, 44 }, 3, new ResourceInts(default(int), default(int), default(int), default(int), default(int), default(int), default(int), default(int)), new IntPair[18]
		{
			new IntPair(17, 3),
			new IntPair(8, 3),
			new IntPair(5, 1),
			new IntPair(6, 1),
			new IntPair(15, 1),
			new IntPair(16, 3),
			new IntPair(7, 3),
			new IntPair(14, 1),
			new IntPair(12, 1),
			new IntPair(4, 3),
			new IntPair(3, 1),
			new IntPair(13, 3),
			new IntPair(10, 27),
			new IntPair(2, 6),
			new IntPair(1, 9),
			new IntPair(11, 9),
			new IntPair(0, 3),
			new IntPair(9, 30)
		}, null));
		List<OrganizationMemberItem> dataArray20 = _dataArray;
		int[] monasticTitleSuffixes20 = new int[2] { 941, 942 };
		List<short> favoriteClothingIds20 = new List<short> { 6, 7, 8, 15, 16, 17 };
		List<short> hatedClothingIds20 = new List<short> { 0, 1, 2, 9, 10, 11 };
		int[] spouseAnonymousTitles20 = new int[2] { 943, 944 };
		short[] initialAges20 = new short[4] { 28, 37, 46, 55 };
		PresetEquipmentItemWithProb[] equipment20 = new PresetEquipmentItemWithProb[8]
		{
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Accessory", -1, 0),
			new PresetEquipmentItemWithProb("Accessory", -1, 0),
			new PresetEquipmentItemWithProb("Accessory", -1, 0),
			new PresetEquipmentItemWithProb("Carrier", 0, 75)
		};
		PresetEquipmentItem clothing20 = new PresetEquipmentItem("Clothing", 16);
		List<PresetInventoryItem> inventory20 = new List<PresetInventoryItem>
		{
			new PresetInventoryItem("Misc", 82, 1, 30),
			new PresetInventoryItem("Food", 9, 3, 40),
			new PresetInventoryItem("Food", 51, 3, 40),
			new PresetInventoryItem("Food", 135, 3, 40),
			new PresetInventoryItem("Medicine", 100, 1, 20),
			new PresetInventoryItem("TeaWine", 18, 1, 20),
			new PresetInventoryItem("TeaWine", 27, 1, 20),
			new PresetInventoryItem("TeaWine", 0, 1, 20),
			new PresetInventoryItem("TeaWine", 9, 1, 20)
		};
		combatSkills2 = new List<PresetOrgMemberCombatSkill>();
		dataArray20.Add(new OrganizationMemberItem(199, 940, 37, 8, 1, 1, 1, restrictPrincipalAmount: true, -1, -1, 7, 6, 7, -1, 7, 0, 0, monasticTitleSuffixes20, 0, 0, 13000, 21150, -100, favoriteClothingIds20, hatedClothingIds20, spouseAnonymousTitles20, canStroll: false, -1, initialAges20, equipment20, clothing20, inventory20, combatSkills2, new sbyte[5], new short[8] { -60, -60, -60, -60, -60, -60, -60, -60 }, 120000, 432000, 20, 20, 61500, new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		}, 6, new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		}, new short[6] { -1, -1, -1, -1, -1, -1 }, new List<sbyte> { 0 }, 3, new ResourceInts(default(int), default(int), default(int), default(int), default(int), default(int), default(int), default(int)), new IntPair[18]
		{
			new IntPair(17, 24),
			new IntPair(8, 24),
			new IntPair(5, 1),
			new IntPair(6, 1),
			new IntPair(15, 6),
			new IntPair(16, 15),
			new IntPair(7, 15),
			new IntPair(14, 1),
			new IntPair(12, 1),
			new IntPair(4, 6),
			new IntPair(3, 1),
			new IntPair(13, 1),
			new IntPair(10, 3),
			new IntPair(2, 3),
			new IntPair(1, 1),
			new IntPair(11, 1),
			new IntPair(0, 1),
			new IntPair(9, 3)
		}, null));
		List<OrganizationMemberItem> dataArray21 = _dataArray;
		int[] monasticTitleSuffixes21 = new int[2] { 945, 946 };
		List<short> favoriteClothingIds21 = new List<short> { 5, 6, 7, 8, 14, 15, 16, 17 };
		List<short> hatedClothingIds21 = new List<short> { 0, 1, 9, 10 };
		int[] spouseAnonymousTitles21 = new int[2] { 947, 948 };
		short[] initialAges21 = new short[4] { 26, 34, 42, 50 };
		PresetEquipmentItemWithProb[] equipment21 = new PresetEquipmentItemWithProb[8]
		{
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Accessory", -1, 0),
			new PresetEquipmentItemWithProb("Accessory", -1, 0),
			new PresetEquipmentItemWithProb("Accessory", -1, 0),
			new PresetEquipmentItemWithProb("Carrier", 18, 75)
		};
		PresetEquipmentItem clothing21 = new PresetEquipmentItem("Clothing", 16);
		List<PresetInventoryItem> inventory21 = new List<PresetInventoryItem>
		{
			new PresetInventoryItem("Food", 9, 3, 40),
			new PresetInventoryItem("Food", 51, 3, 40),
			new PresetInventoryItem("Food", 135, 3, 40),
			new PresetInventoryItem("Medicine", 100, 1, 20),
			new PresetInventoryItem("TeaWine", 18, 1, 20),
			new PresetInventoryItem("TeaWine", 27, 1, 20),
			new PresetInventoryItem("TeaWine", 0, 1, 20),
			new PresetInventoryItem("TeaWine", 9, 1, 20)
		};
		combatSkills2 = new List<PresetOrgMemberCombatSkill>();
		dataArray21.Add(new OrganizationMemberItem(200, 866, 37, 7, 2, 2, 2, restrictPrincipalAmount: false, -1, -1, -1, 5, 7, -1, 7, 0, 0, monasticTitleSuffixes21, 0, 0, 10000, 13800, -100, favoriteClothingIds21, hatedClothingIds21, spouseAnonymousTitles21, canStroll: false, -1, initialAges21, equipment21, clothing21, inventory21, combatSkills2, new sbyte[5], new short[8] { -70, -70, -70, -70, -70, -70, -70, -50 }, 75000, 216000, 15, 30, 42300, new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		}, 6, new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		}, new short[6] { -1, -1, -1, -1, -1, -1 }, new List<sbyte> { 52, 56 }, 3, new ResourceInts(default(int), default(int), default(int), default(int), default(int), default(int), default(int), default(int)), new IntPair[18]
		{
			new IntPair(17, 21),
			new IntPair(8, 21),
			new IntPair(5, 3),
			new IntPair(6, 3),
			new IntPair(15, 9),
			new IntPair(16, 12),
			new IntPair(7, 12),
			new IntPair(14, 1),
			new IntPair(12, 1),
			new IntPair(4, 6),
			new IntPair(3, 6),
			new IntPair(13, 1),
			new IntPair(10, 3),
			new IntPair(2, 3),
			new IntPair(1, 1),
			new IntPair(11, 1),
			new IntPair(0, 1),
			new IntPair(9, 3)
		}, null));
		List<OrganizationMemberItem> dataArray22 = _dataArray;
		int[] monasticTitleSuffixes22 = new int[2] { 949, 950 };
		List<short> favoriteClothingIds22 = new List<short> { 4, 5, 6, 7, 8, 13, 14, 15, 16, 17 };
		List<short> hatedClothingIds22 = new List<short> { 0, 9 };
		int[] spouseAnonymousTitles22 = new int[2] { 951, 952 };
		short[] initialAges22 = new short[4] { 24, 31, 38, 45 };
		PresetEquipmentItemWithProb[] equipment22 = new PresetEquipmentItemWithProb[8]
		{
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Accessory", -1, 0),
			new PresetEquipmentItemWithProb("Accessory", -1, 0),
			new PresetEquipmentItemWithProb("Accessory", -1, 0),
			new PresetEquipmentItemWithProb("Carrier", 0, 75)
		};
		PresetEquipmentItem clothing22 = new PresetEquipmentItem("Clothing", 15);
		List<PresetInventoryItem> inventory22 = new List<PresetInventoryItem>
		{
			new PresetInventoryItem("Food", 9, 3, 40),
			new PresetInventoryItem("Food", 51, 3, 40),
			new PresetInventoryItem("Food", 135, 3, 40),
			new PresetInventoryItem("Medicine", 94, 1, 20),
			new PresetInventoryItem("TeaWine", 18, 1, 20),
			new PresetInventoryItem("TeaWine", 27, 1, 20),
			new PresetInventoryItem("TeaWine", 0, 1, 20),
			new PresetInventoryItem("TeaWine", 9, 1, 20)
		};
		combatSkills2 = new List<PresetOrgMemberCombatSkill>();
		dataArray22.Add(new OrganizationMemberItem(201, 871, 37, 6, 2, 2, 2, restrictPrincipalAmount: false, -1, -1, -1, 4, 6, -1, 6, 0, 0, monasticTitleSuffixes22, 0, 0, 8000, 8400, -100, favoriteClothingIds22, hatedClothingIds22, spouseAnonymousTitles22, canStroll: false, -1, initialAges22, equipment22, clothing22, inventory22, combatSkills2, new sbyte[5], new short[8] { -70, -70, -70, -70, -70, -70, -50, -70 }, 45000, 108000, 15, 40, 27600, new short[16]
		{
			-1, -1, -1, -1, -1, 12, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		}, 6, new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		}, new short[6] { -1, -1, -1, -1, -1, -1 }, new List<sbyte> { 2, 57 }, 16, new ResourceInts(default(int), default(int), default(int), default(int), default(int), default(int), default(int), default(int)), new IntPair[18]
		{
			new IntPair(17, 18),
			new IntPair(8, 18),
			new IntPair(5, 1),
			new IntPair(6, 1),
			new IntPair(15, 15),
			new IntPair(16, 18),
			new IntPair(7, 18),
			new IntPair(14, 1),
			new IntPair(12, 1),
			new IntPair(4, 3),
			new IntPair(3, 1),
			new IntPair(13, 1),
			new IntPair(10, 3),
			new IntPair(2, 3),
			new IntPair(1, 1),
			new IntPair(11, 1),
			new IntPair(0, 1),
			new IntPair(9, 3)
		}, null));
		List<OrganizationMemberItem> dataArray23 = _dataArray;
		int[] monasticTitleSuffixes23 = new int[2] { 953, 954 };
		List<short> favoriteClothingIds23 = new List<short> { 4 };
		list = new List<short>();
		List<short> hatedClothingIds23 = list;
		int[] spouseAnonymousTitles23 = new int[2] { 955, 956 };
		short[] initialAges23 = new short[4] { 22, 28, 34, 40 };
		PresetEquipmentItemWithProb[] equipment23 = new PresetEquipmentItemWithProb[8]
		{
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Accessory", -1, 0),
			new PresetEquipmentItemWithProb("Accessory", -1, 0),
			new PresetEquipmentItemWithProb("Accessory", -1, 0),
			new PresetEquipmentItemWithProb("Carrier", 18, 75)
		};
		PresetEquipmentItem clothing23 = new PresetEquipmentItem("Clothing", 4);
		List<PresetInventoryItem> inventory23 = new List<PresetInventoryItem>
		{
			new PresetInventoryItem("Food", 0, 3, 40),
			new PresetInventoryItem("Food", 93, 3, 40),
			new PresetInventoryItem("Food", 135, 3, 40),
			new PresetInventoryItem("TeaWine", 18, 1, 20),
			new PresetInventoryItem("TeaWine", 27, 1, 20),
			new PresetInventoryItem("TeaWine", 0, 1, 20),
			new PresetInventoryItem("TeaWine", 9, 1, 20)
		};
		combatSkills2 = new List<PresetOrgMemberCombatSkill>();
		dataArray23.Add(new OrganizationMemberItem(202, 57, 37, 5, 2, 2, 2, restrictPrincipalAmount: false, -1, -1, -1, 5, 5, -1, 5, 0, 0, monasticTitleSuffixes23, 0, 0, 6000, 4650, -75, favoriteClothingIds23, hatedClothingIds23, spouseAnonymousTitles23, canStroll: true, -1, initialAges23, equipment23, clothing23, inventory23, combatSkills2, new sbyte[5], new short[8] { -80, -80, -80, -80, -80, -80, -80, -50 }, 30000, 36000, 10, 50, 16800, new short[16]
		{
			12, 12, 12, 12, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		}, 7, new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		}, new short[6] { -1, -1, -1, -1, -1, -1 }, new List<sbyte> { 3, 58, 66 }, 16, new ResourceInts(default(int), default(int), default(int), default(int), default(int), default(int), default(int), default(int)), new IntPair[18]
		{
			new IntPair(17, 3),
			new IntPair(8, 21),
			new IntPair(5, 1),
			new IntPair(6, 1),
			new IntPair(15, 3),
			new IntPair(16, 21),
			new IntPair(7, 6),
			new IntPair(14, 3),
			new IntPair(12, 3),
			new IntPair(4, 30),
			new IntPair(3, 1),
			new IntPair(13, 3),
			new IntPair(10, 3),
			new IntPair(2, 3),
			new IntPair(1, 1),
			new IntPair(11, 3),
			new IntPair(0, 1),
			new IntPair(9, 1)
		}, null));
		List<OrganizationMemberItem> dataArray24 = _dataArray;
		int[] monasticTitleSuffixes24 = new int[2] { 957, 958 };
		List<short> favoriteClothingIds24 = new List<short> { 15 };
		list = new List<short>();
		List<short> hatedClothingIds24 = list;
		int[] spouseAnonymousTitles24 = new int[2] { 959, 960 };
		short[] initialAges24 = new short[4] { 20, 25, 30, 35 };
		PresetEquipmentItemWithProb[] equipment24 = new PresetEquipmentItemWithProb[8]
		{
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Accessory", -1, 0),
			new PresetEquipmentItemWithProb("Accessory", -1, 0),
			new PresetEquipmentItemWithProb("Accessory", -1, 0),
			new PresetEquipmentItemWithProb("Carrier", 0, 75)
		};
		PresetEquipmentItem clothing24 = new PresetEquipmentItem("Clothing", 15);
		List<PresetInventoryItem> inventory24 = new List<PresetInventoryItem>
		{
			new PresetInventoryItem("Food", 9, 3, 40),
			new PresetInventoryItem("Food", 135, 3, 40),
			new PresetInventoryItem("TeaWine", 18, 1, 20),
			new PresetInventoryItem("TeaWine", 27, 1, 20),
			new PresetInventoryItem("TeaWine", 0, 1, 20),
			new PresetInventoryItem("TeaWine", 9, 1, 20)
		};
		combatSkills2 = new List<PresetOrgMemberCombatSkill>();
		dataArray24.Add(new OrganizationMemberItem(203, 62, 37, 4, 3, 3, 3, restrictPrincipalAmount: false, -1, -1, -1, 4, 4, -1, 4, 0, 0, monasticTitleSuffixes24, 0, 0, 4500, 2250, -50, favoriteClothingIds24, hatedClothingIds24, spouseAnonymousTitles24, canStroll: true, -1, initialAges24, equipment24, clothing24, inventory24, combatSkills2, new sbyte[5], new short[8] { -80, -80, -80, -80, -80, -80, -50, -80 }, 22500, 18000, 10, 60, 9300, new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, 9
		}, 5, new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		}, new short[6] { -1, -1, -1, -1, -1, -1 }, new List<sbyte> { 4, 53 }, 14, new ResourceInts(default(int), default(int), default(int), default(int), default(int), default(int), default(int), default(int)), new IntPair[18]
		{
			new IntPair(17, 3),
			new IntPair(8, 3),
			new IntPair(5, 1),
			new IntPair(6, 1),
			new IntPair(15, 30),
			new IntPair(16, 15),
			new IntPair(7, 15),
			new IntPair(14, 1),
			new IntPair(12, 1),
			new IntPair(4, 6),
			new IntPair(3, 1),
			new IntPair(13, 3),
			new IntPair(10, 9),
			new IntPair(2, 3),
			new IntPair(1, 3),
			new IntPair(11, 9),
			new IntPair(0, 3),
			new IntPair(9, 1)
		}, null));
		List<OrganizationMemberItem> dataArray25 = _dataArray;
		int[] monasticTitleSuffixes25 = new int[2] { 961, 962 };
		List<short> favoriteClothingIds25 = new List<short> { 13 };
		list = new List<short>();
		List<short> hatedClothingIds25 = list;
		int[] spouseAnonymousTitles25 = new int[2] { 963, 964 };
		short[] initialAges25 = new short[4] { 18, 22, 26, 30 };
		PresetEquipmentItemWithProb[] equipment25 = new PresetEquipmentItemWithProb[8]
		{
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Accessory", -1, 0),
			new PresetEquipmentItemWithProb("Accessory", -1, 0),
			new PresetEquipmentItemWithProb("Accessory", -1, 0),
			new PresetEquipmentItemWithProb("Carrier", 0, 75)
		};
		PresetEquipmentItem clothing25 = new PresetEquipmentItem("Clothing", 13);
		List<PresetInventoryItem> inventory25 = new List<PresetInventoryItem>
		{
			new PresetInventoryItem("CraftTool", 45, 1, 30),
			new PresetInventoryItem("Food", 135, 3, 40),
			new PresetInventoryItem("Medicine", 54, 1, 20),
			new PresetInventoryItem("Medicine", 66, 1, 20),
			new PresetInventoryItem("Medicine", 82, 1, 20),
			new PresetInventoryItem("Medicine", 94, 1, 20),
			new PresetInventoryItem("Material", 140, 1, 10),
			new PresetInventoryItem("Material", 144, 1, 10),
			new PresetInventoryItem("Material", 148, 1, 10),
			new PresetInventoryItem("Material", 152, 1, 10),
			new PresetInventoryItem("Material", 156, 1, 10),
			new PresetInventoryItem("Material", 160, 1, 10),
			new PresetInventoryItem("Material", 164, 1, 10),
			new PresetInventoryItem("Material", 168, 1, 10),
			new PresetInventoryItem("Material", 172, 1, 10),
			new PresetInventoryItem("Material", 176, 1, 10),
			new PresetInventoryItem("Material", 180, 1, 10),
			new PresetInventoryItem("Material", 184, 1, 10),
			new PresetInventoryItem("Material", 188, 1, 10),
			new PresetInventoryItem("Material", 192, 1, 10),
			new PresetInventoryItem("Material", 196, 1, 10),
			new PresetInventoryItem("Material", 200, 1, 10),
			new PresetInventoryItem("Material", 204, 1, 10),
			new PresetInventoryItem("Material", 208, 1, 10),
			new PresetInventoryItem("Material", 212, 1, 10),
			new PresetInventoryItem("Material", 216, 1, 10),
			new PresetInventoryItem("Material", 220, 1, 10),
			new PresetInventoryItem("Material", 224, 1, 10),
			new PresetInventoryItem("Material", 228, 1, 10),
			new PresetInventoryItem("Material", 232, 1, 10)
		};
		combatSkills2 = new List<PresetOrgMemberCombatSkill>();
		dataArray25.Add(new OrganizationMemberItem(204, 67, 37, 3, 3, 3, 3, restrictPrincipalAmount: false, -1, -1, -1, 3, 3, -1, 3, 0, 0, monasticTitleSuffixes25, 0, 0, 3000, 900, -25, favoriteClothingIds25, hatedClothingIds25, spouseAnonymousTitles25, canStroll: true, -1, initialAges25, equipment25, clothing25, inventory25, combatSkills2, new sbyte[5], new short[8] { -80, -80, -80, -80, -80, -50, -80, -80 }, 15000, 9000, 10, 70, 4500, new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, 12, 12,
			-1, -1, -1, -1, -1, -1
		}, 7, new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		}, new short[6] { -1, -1, -1, -1, -1, -1 }, new List<sbyte> { 5, 54, 64 }, 14, new ResourceInts(default(int), default(int), default(int), default(int), default(int), default(int), default(int), default(int)), new IntPair[18]
		{
			new IntPair(17, 1),
			new IntPair(8, 6),
			new IntPair(5, 1),
			new IntPair(6, 1),
			new IntPair(15, 1),
			new IntPair(16, 6),
			new IntPair(7, 1),
			new IntPair(14, 3),
			new IntPair(12, 3),
			new IntPair(4, 12),
			new IntPair(3, 1),
			new IntPair(13, 30),
			new IntPair(10, 18),
			new IntPair(2, 1),
			new IntPair(1, 1),
			new IntPair(11, 9),
			new IntPair(0, 12),
			new IntPair(9, 1)
		}, null));
		List<OrganizationMemberItem> dataArray26 = _dataArray;
		int[] monasticTitleSuffixes26 = new int[2] { 965, 966 };
		List<short> favoriteClothingIds26 = new List<short> { 0, 1, 2, 3, 9, 10, 11, 12 };
		List<short> hatedClothingIds26 = new List<short> { 8, 17 };
		int[] spouseAnonymousTitles26 = new int[2] { 967, 968 };
		short[] initialAges26 = new short[4] { 16, 19, 22, 25 };
		PresetEquipmentItemWithProb[] equipment26 = new PresetEquipmentItemWithProb[8]
		{
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Accessory", -1, 0),
			new PresetEquipmentItemWithProb("Accessory", -1, 0),
			new PresetEquipmentItemWithProb("Accessory", -1, 0),
			new PresetEquipmentItemWithProb("Carrier", 0, 75)
		};
		PresetEquipmentItem clothing26 = new PresetEquipmentItem("Clothing", 11);
		List<PresetInventoryItem> inventory26 = new List<PresetInventoryItem>
		{
			new PresetInventoryItem("CraftTool", 0, 1, 30),
			new PresetInventoryItem("CraftTool", 9, 1, 30),
			new PresetInventoryItem("CraftTool", 18, 1, 30),
			new PresetInventoryItem("CraftTool", 27, 1, 30),
			new PresetInventoryItem("Food", 93, 3, 40),
			new PresetInventoryItem("Material", 0, 1, 20),
			new PresetInventoryItem("Material", 7, 1, 20),
			new PresetInventoryItem("Material", 14, 1, 20),
			new PresetInventoryItem("Material", 21, 1, 20),
			new PresetInventoryItem("Material", 28, 1, 20),
			new PresetInventoryItem("Material", 35, 1, 20),
			new PresetInventoryItem("Material", 42, 1, 20),
			new PresetInventoryItem("Material", 49, 1, 20)
		};
		combatSkills2 = new List<PresetOrgMemberCombatSkill>();
		dataArray26.Add(new OrganizationMemberItem(205, 888, 37, 2, 3, 3, 3, restrictPrincipalAmount: false, -1, -1, -1, 2, 2, -1, 2, 0, 0, monasticTitleSuffixes26, 0, 0, 2000, 300, 0, favoriteClothingIds26, hatedClothingIds26, spouseAnonymousTitles26, canStroll: true, -1, initialAges26, equipment26, clothing26, inventory26, combatSkills2, new sbyte[5], new short[8] { -90, -50, -50, -50, -50, -90, -90, -90 }, 11250, 3000, 5, 80, 1800, new short[16]
		{
			-1, -1, -1, -1, -1, -1, 12, 12, -1, -1,
			12, 12, -1, -1, -1, -1
		}, 7, new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		}, new short[6] { -1, -1, -1, -1, -1, -1 }, new List<sbyte> { 6, 61, 63 }, 12, new ResourceInts(default(int), default(int), default(int), default(int), default(int), default(int), default(int), default(int)), new IntPair[18]
		{
			new IntPair(17, 1),
			new IntPair(8, 1),
			new IntPair(5, 1),
			new IntPair(6, 1),
			new IntPair(15, 9),
			new IntPair(16, 3),
			new IntPair(7, 1),
			new IntPair(14, 1),
			new IntPair(12, 1),
			new IntPair(4, 9),
			new IntPair(3, 1),
			new IntPair(13, 3),
			new IntPair(10, 15),
			new IntPair(2, 30),
			new IntPair(1, 9),
			new IntPair(11, 15),
			new IntPair(0, 6),
			new IntPair(9, 1)
		}, null));
		List<OrganizationMemberItem> dataArray27 = _dataArray;
		int[] monasticTitleSuffixes27 = new int[2] { 969, 970 };
		List<short> favoriteClothingIds27 = new List<short> { 0, 1, 2, 9, 10, 11 };
		List<short> hatedClothingIds27 = new List<short> { 7, 8, 16, 17 };
		int[] spouseAnonymousTitles27 = new int[2] { 971, 972 };
		short[] initialAges27 = new short[4] { 14, 16, 18, 20 };
		PresetEquipmentItemWithProb[] equipment27 = new PresetEquipmentItemWithProb[8]
		{
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Accessory", -1, 0),
			new PresetEquipmentItemWithProb("Accessory", -1, 0),
			new PresetEquipmentItemWithProb("Accessory", -1, 0),
			new PresetEquipmentItemWithProb("Carrier", 18, 75)
		};
		PresetEquipmentItem clothing27 = new PresetEquipmentItem("Clothing", 10);
		List<PresetInventoryItem> inventory27 = new List<PresetInventoryItem>
		{
			new PresetInventoryItem("CraftTool", 36, 1, 30),
			new PresetInventoryItem("Food", 0, 3, 40),
			new PresetInventoryItem("Food", 93, 3, 40),
			new PresetInventoryItem("Material", 57, 1, 20),
			new PresetInventoryItem("Material", 64, 1, 20),
			new PresetInventoryItem("Material", 71, 1, 20),
			new PresetInventoryItem("Material", 78, 1, 20)
		};
		combatSkills2 = new List<PresetOrgMemberCombatSkill>();
		dataArray27.Add(new OrganizationMemberItem(206, 893, 37, 1, 2, 2, 2, restrictPrincipalAmount: false, -1, -1, -1, 1, 1, -1, 1, 0, 0, monasticTitleSuffixes27, 0, 0, 1000, 150, 0, favoriteClothingIds27, hatedClothingIds27, spouseAnonymousTitles27, canStroll: true, -1, initialAges27, equipment27, clothing27, inventory27, combatSkills2, new sbyte[5], new short[8] { -50, -90, -90, -90, -90, -90, -90, -90 }, 7500, 1500, 5, 90, 600, new short[16]
		{
			-1, -1, -1, -1, -1, 12, -1, -1, -1, -1,
			-1, -1, -1, -1, 12, -1
		}, 4, new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		}, new short[6] { -1, -1, -1, -1, -1, -1 }, new List<sbyte> { 7, 60 }, 12, new ResourceInts(default(int), default(int), default(int), default(int), default(int), default(int), default(int), default(int)), new IntPair[18]
		{
			new IntPair(17, 3),
			new IntPair(8, 3),
			new IntPair(5, 1),
			new IntPair(6, 1),
			new IntPair(15, 6),
			new IntPair(16, 3),
			new IntPair(7, 3),
			new IntPair(14, 1),
			new IntPair(12, 1),
			new IntPair(4, 3),
			new IntPair(3, 1),
			new IntPair(13, 1),
			new IntPair(10, 30),
			new IntPair(2, 12),
			new IntPair(1, 9),
			new IntPair(11, 9),
			new IntPair(0, 3),
			new IntPair(9, 18)
		}, null));
		List<OrganizationMemberItem> dataArray28 = _dataArray;
		int[] monasticTitleSuffixes28 = new int[2] { 973, 974 };
		List<short> favoriteClothingIds28 = new List<short> { 0, 1, 9, 10 };
		List<short> hatedClothingIds28 = new List<short> { 6, 7, 8, 15, 16, 17 };
		int[] spouseAnonymousTitles28 = new int[2] { 975, 976 };
		short[] initialAges28 = new short[4] { 12, 13, 14, 15 };
		PresetEquipmentItemWithProb[] equipment28 = new PresetEquipmentItemWithProb[8]
		{
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Accessory", -1, 0),
			new PresetEquipmentItemWithProb("Accessory", -1, 0),
			new PresetEquipmentItemWithProb("Accessory", -1, 0),
			new PresetEquipmentItemWithProb("Carrier", -1, 0)
		};
		PresetEquipmentItem clothing28 = new PresetEquipmentItem("Clothing", 9);
		List<PresetInventoryItem> inventory28 = new List<PresetInventoryItem>
		{
			new PresetInventoryItem("Misc", 10, 1, 10),
			new PresetInventoryItem("Misc", 11, 1, 10),
			new PresetInventoryItem("Misc", 12, 1, 10),
			new PresetInventoryItem("Misc", 13, 1, 10),
			new PresetInventoryItem("Misc", 14, 1, 10),
			new PresetInventoryItem("Misc", 15, 1, 10),
			new PresetInventoryItem("Misc", 16, 1, 10),
			new PresetInventoryItem("Misc", 17, 1, 10),
			new PresetInventoryItem("Misc", 18, 1, 10),
			new PresetInventoryItem("Misc", 19, 1, 10),
			new PresetInventoryItem("Misc", 20, 1, 10),
			new PresetInventoryItem("Misc", 21, 1, 10),
			new PresetInventoryItem("Misc", 22, 1, 10),
			new PresetInventoryItem("Misc", 23, 1, 10),
			new PresetInventoryItem("Misc", 24, 1, 10),
			new PresetInventoryItem("Misc", 25, 1, 10),
			new PresetInventoryItem("Misc", 26, 1, 10),
			new PresetInventoryItem("Misc", 27, 1, 10)
		};
		combatSkills2 = new List<PresetOrgMemberCombatSkill>();
		dataArray28.Add(new OrganizationMemberItem(207, 898, 37, 0, 2, 2, 2, restrictPrincipalAmount: false, -1, -1, -1, 0, 0, -1, 0, 0, 0, monasticTitleSuffixes28, 0, 0, 500, 0, 0, favoriteClothingIds28, hatedClothingIds28, spouseAnonymousTitles28, canStroll: true, -1, initialAges28, equipment28, clothing28, inventory28, combatSkills2, new sbyte[5], new short[8] { -90, -90, -90, -90, -90, -90, -90, -90 }, 3750, 750, 1, 100, 300, new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, 12
		}, 3, new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		}, new short[6] { -1, -1, -1, -1, -1, -1 }, new List<sbyte> { 8, 44 }, 3, new ResourceInts(default(int), default(int), default(int), default(int), default(int), default(int), default(int), default(int)), new IntPair[18]
		{
			new IntPair(17, 3),
			new IntPair(8, 3),
			new IntPair(5, 1),
			new IntPair(6, 1),
			new IntPair(15, 1),
			new IntPair(16, 3),
			new IntPair(7, 3),
			new IntPair(14, 1),
			new IntPair(12, 1),
			new IntPair(4, 3),
			new IntPair(3, 1),
			new IntPair(13, 3),
			new IntPair(10, 27),
			new IntPair(2, 6),
			new IntPair(1, 9),
			new IntPair(11, 9),
			new IntPair(0, 3),
			new IntPair(9, 30)
		}, null));
		List<OrganizationMemberItem> dataArray29 = _dataArray;
		int[] monasticTitleSuffixes29 = new int[2] { 978, 979 };
		List<short> favoriteClothingIds29 = new List<short> { 6, 7, 8, 15, 16, 17 };
		List<short> hatedClothingIds29 = new List<short> { 0, 1, 2, 9, 10, 11 };
		int[] spouseAnonymousTitles29 = new int[2] { 980, 981 };
		short[] initialAges29 = new short[4] { 28, 37, 46, 55 };
		PresetEquipmentItemWithProb[] equipment29 = new PresetEquipmentItemWithProb[8]
		{
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Accessory", -1, 0),
			new PresetEquipmentItemWithProb("Accessory", -1, 0),
			new PresetEquipmentItemWithProb("Accessory", -1, 0),
			new PresetEquipmentItemWithProb("Carrier", 18, 75)
		};
		PresetEquipmentItem clothing29 = new PresetEquipmentItem("Clothing", 16);
		List<PresetInventoryItem> inventory29 = new List<PresetInventoryItem>
		{
			new PresetInventoryItem("Misc", 82, 1, 30),
			new PresetInventoryItem("Food", 9, 3, 40),
			new PresetInventoryItem("Food", 51, 3, 40),
			new PresetInventoryItem("Food", 135, 3, 40),
			new PresetInventoryItem("Medicine", 100, 1, 20),
			new PresetInventoryItem("TeaWine", 18, 1, 20),
			new PresetInventoryItem("TeaWine", 27, 1, 20),
			new PresetInventoryItem("TeaWine", 0, 1, 20),
			new PresetInventoryItem("TeaWine", 9, 1, 20)
		};
		combatSkills2 = new List<PresetOrgMemberCombatSkill>();
		dataArray29.Add(new OrganizationMemberItem(208, 977, 38, 8, 1, 1, 1, restrictPrincipalAmount: true, -1, -1, 7, 6, 6, -1, 7, 0, 0, monasticTitleSuffixes29, 0, 0, 13000, 21150, -100, favoriteClothingIds29, hatedClothingIds29, spouseAnonymousTitles29, canStroll: false, -1, initialAges29, equipment29, clothing29, inventory29, combatSkills2, new sbyte[5], new short[8] { -60, -60, -60, -60, -60, -60, -60, -60 }, 120000, 432000, 20, 20, 61500, new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		}, 6, new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		}, new short[6] { -1, -1, -1, -1, -1, -1 }, new List<sbyte> { 0 }, 3, new ResourceInts(default(int), default(int), default(int), default(int), default(int), default(int), default(int), default(int)), new IntPair[18]
		{
			new IntPair(17, 12),
			new IntPair(8, 12),
			new IntPair(5, 1),
			new IntPair(6, 1),
			new IntPair(15, 6),
			new IntPair(16, 15),
			new IntPair(7, 15),
			new IntPair(14, 1),
			new IntPair(12, 1),
			new IntPair(4, 6),
			new IntPair(3, 1),
			new IntPair(13, 1),
			new IntPair(10, 6),
			new IntPair(2, 3),
			new IntPair(1, 9),
			new IntPair(11, 6),
			new IntPair(0, 9),
			new IntPair(9, 3)
		}, null));
		List<OrganizationMemberItem> dataArray30 = _dataArray;
		int[] monasticTitleSuffixes30 = new int[2] { 983, 984 };
		List<short> favoriteClothingIds30 = new List<short> { 5, 6, 7, 8, 14, 15, 16, 17 };
		List<short> hatedClothingIds30 = new List<short> { 0, 1, 9, 10 };
		int[] spouseAnonymousTitles30 = new int[2] { 985, 986 };
		short[] initialAges30 = new short[4] { 26, 34, 42, 50 };
		PresetEquipmentItemWithProb[] equipment30 = new PresetEquipmentItemWithProb[8]
		{
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Accessory", -1, 0),
			new PresetEquipmentItemWithProb("Accessory", -1, 0),
			new PresetEquipmentItemWithProb("Accessory", -1, 0),
			new PresetEquipmentItemWithProb("Carrier", 18, 75)
		};
		PresetEquipmentItem clothing30 = new PresetEquipmentItem("Clothing", 7);
		List<PresetInventoryItem> inventory30 = new List<PresetInventoryItem>
		{
			new PresetInventoryItem("Food", 9, 3, 40),
			new PresetInventoryItem("Food", 51, 3, 40),
			new PresetInventoryItem("Food", 135, 3, 40),
			new PresetInventoryItem("Medicine", 100, 1, 20),
			new PresetInventoryItem("TeaWine", 18, 1, 20),
			new PresetInventoryItem("TeaWine", 27, 1, 20),
			new PresetInventoryItem("TeaWine", 0, 1, 20),
			new PresetInventoryItem("TeaWine", 9, 1, 20)
		};
		combatSkills2 = new List<PresetOrgMemberCombatSkill>();
		dataArray30.Add(new OrganizationMemberItem(209, 982, 38, 7, 2, 2, 2, restrictPrincipalAmount: false, -1, -1, 7, 5, 5, -1, 7, 0, 0, monasticTitleSuffixes30, 0, 0, 10000, 13800, -100, favoriteClothingIds30, hatedClothingIds30, spouseAnonymousTitles30, canStroll: false, -1, initialAges30, equipment30, clothing30, inventory30, combatSkills2, new sbyte[5], new short[8] { -70, -70, -70, -70, -70, -70, -70, -50 }, 75000, 216000, 15, 30, 42300, new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		}, 6, new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		}, new short[6] { -1, -1, -1, -1, -1, -1 }, new List<sbyte> { 52, 56 }, 3, new ResourceInts(default(int), default(int), default(int), default(int), default(int), default(int), default(int), default(int)), new IntPair[18]
		{
			new IntPair(17, 9),
			new IntPair(8, 9),
			new IntPair(5, 3),
			new IntPair(6, 3),
			new IntPair(15, 9),
			new IntPair(16, 12),
			new IntPair(7, 12),
			new IntPair(14, 1),
			new IntPair(12, 1),
			new IntPair(4, 6),
			new IntPair(3, 6),
			new IntPair(13, 1),
			new IntPair(10, 6),
			new IntPair(2, 3),
			new IntPair(1, 9),
			new IntPair(11, 6),
			new IntPair(0, 9),
			new IntPair(9, 3)
		}, null));
		List<OrganizationMemberItem> dataArray31 = _dataArray;
		int[] monasticTitleSuffixes31 = new int[2] { 987, 988 };
		List<short> favoriteClothingIds31 = new List<short> { 4, 5, 6, 7, 8, 13, 14, 15, 16, 17 };
		List<short> hatedClothingIds31 = new List<short> { 0, 9 };
		int[] spouseAnonymousTitles31 = new int[2] { 989, 990 };
		short[] initialAges31 = new short[4] { 24, 31, 38, 45 };
		PresetEquipmentItemWithProb[] equipment31 = new PresetEquipmentItemWithProb[8]
		{
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Accessory", -1, 0),
			new PresetEquipmentItemWithProb("Accessory", -1, 0),
			new PresetEquipmentItemWithProb("Accessory", -1, 0),
			new PresetEquipmentItemWithProb("Carrier", 18, 75)
		};
		PresetEquipmentItem clothing31 = new PresetEquipmentItem("Clothing", 15);
		List<PresetInventoryItem> inventory31 = new List<PresetInventoryItem>
		{
			new PresetInventoryItem("Food", 9, 3, 40),
			new PresetInventoryItem("Food", 51, 3, 40),
			new PresetInventoryItem("Food", 135, 3, 40),
			new PresetInventoryItem("Medicine", 94, 1, 20),
			new PresetInventoryItem("TeaWine", 18, 1, 20),
			new PresetInventoryItem("TeaWine", 27, 1, 20),
			new PresetInventoryItem("TeaWine", 0, 1, 20),
			new PresetInventoryItem("TeaWine", 9, 1, 20)
		};
		combatSkills2 = new List<PresetOrgMemberCombatSkill>();
		dataArray31.Add(new OrganizationMemberItem(210, 871, 38, 6, 2, 2, 2, restrictPrincipalAmount: false, -1, -1, -1, 4, 4, -1, 6, 0, 0, monasticTitleSuffixes31, 0, 0, 8000, 8400, -100, favoriteClothingIds31, hatedClothingIds31, spouseAnonymousTitles31, canStroll: false, -1, initialAges31, equipment31, clothing31, inventory31, combatSkills2, new sbyte[5], new short[8] { -70, -70, -70, -70, -70, -70, -50, -70 }, 45000, 108000, 15, 40, 27600, new short[16]
		{
			-1, -1, -1, -1, -1, 12, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		}, 6, new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		}, new short[6] { -1, -1, -1, -1, -1, -1 }, new List<sbyte> { 2, 57 }, 16, new ResourceInts(default(int), default(int), default(int), default(int), default(int), default(int), default(int), default(int)), new IntPair[18]
		{
			new IntPair(17, 6),
			new IntPair(8, 6),
			new IntPair(5, 1),
			new IntPair(6, 1),
			new IntPair(15, 15),
			new IntPair(16, 18),
			new IntPair(7, 18),
			new IntPair(14, 1),
			new IntPair(12, 1),
			new IntPair(4, 3),
			new IntPair(3, 1),
			new IntPair(13, 1),
			new IntPair(10, 6),
			new IntPair(2, 3),
			new IntPair(1, 9),
			new IntPair(11, 6),
			new IntPair(0, 9),
			new IntPair(9, 3)
		}, null));
		List<OrganizationMemberItem> dataArray32 = _dataArray;
		int[] monasticTitleSuffixes32 = new int[2] { 991, 992 };
		List<short> favoriteClothingIds32 = new List<short> { 4 };
		list = new List<short>();
		List<short> hatedClothingIds32 = list;
		int[] spouseAnonymousTitles32 = new int[2] { 993, 994 };
		short[] initialAges32 = new short[4] { 22, 28, 34, 40 };
		PresetEquipmentItemWithProb[] equipment32 = new PresetEquipmentItemWithProb[8]
		{
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Accessory", -1, 0),
			new PresetEquipmentItemWithProb("Accessory", -1, 0),
			new PresetEquipmentItemWithProb("Accessory", -1, 0),
			new PresetEquipmentItemWithProb("Carrier", 18, 75)
		};
		PresetEquipmentItem clothing32 = new PresetEquipmentItem("Clothing", 4);
		List<PresetInventoryItem> inventory32 = new List<PresetInventoryItem>
		{
			new PresetInventoryItem("Food", 0, 3, 40),
			new PresetInventoryItem("Food", 93, 3, 40),
			new PresetInventoryItem("Food", 135, 3, 40),
			new PresetInventoryItem("TeaWine", 18, 1, 20),
			new PresetInventoryItem("TeaWine", 27, 1, 20),
			new PresetInventoryItem("TeaWine", 0, 1, 20),
			new PresetInventoryItem("TeaWine", 9, 1, 20)
		};
		combatSkills2 = new List<PresetOrgMemberCombatSkill>();
		dataArray32.Add(new OrganizationMemberItem(211, 57, 38, 5, 2, 2, 2, restrictPrincipalAmount: false, -1, -1, -1, 5, 5, -1, 5, 0, 0, monasticTitleSuffixes32, 0, 0, 6000, 4650, -75, favoriteClothingIds32, hatedClothingIds32, spouseAnonymousTitles32, canStroll: true, -1, initialAges32, equipment32, clothing32, inventory32, combatSkills2, new sbyte[5], new short[8] { -80, -80, -80, -80, -80, -80, -80, -50 }, 30000, 36000, 10, 50, 16800, new short[16]
		{
			12, 12, 12, 12, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		}, 7, new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		}, new short[6] { -1, -1, -1, -1, -1, -1 }, new List<sbyte> { 3, 58, 66 }, 16, new ResourceInts(default(int), default(int), default(int), default(int), default(int), default(int), default(int), default(int)), new IntPair[18]
		{
			new IntPair(17, 3),
			new IntPair(8, 21),
			new IntPair(5, 1),
			new IntPair(6, 1),
			new IntPair(15, 3),
			new IntPair(16, 21),
			new IntPair(7, 6),
			new IntPair(14, 3),
			new IntPair(12, 3),
			new IntPair(4, 30),
			new IntPair(3, 1),
			new IntPair(13, 3),
			new IntPair(10, 3),
			new IntPair(2, 3),
			new IntPair(1, 1),
			new IntPair(11, 3),
			new IntPair(0, 1),
			new IntPair(9, 1)
		}, null));
		List<OrganizationMemberItem> dataArray33 = _dataArray;
		int[] monasticTitleSuffixes33 = new int[2] { 995, 996 };
		List<short> favoriteClothingIds33 = new List<short> { 15 };
		list = new List<short>();
		List<short> hatedClothingIds33 = list;
		int[] spouseAnonymousTitles33 = new int[2] { 997, 998 };
		short[] initialAges33 = new short[4] { 20, 25, 30, 35 };
		PresetEquipmentItemWithProb[] equipment33 = new PresetEquipmentItemWithProb[8]
		{
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Accessory", -1, 0),
			new PresetEquipmentItemWithProb("Accessory", -1, 0),
			new PresetEquipmentItemWithProb("Accessory", -1, 0),
			new PresetEquipmentItemWithProb("Carrier", 18, 75)
		};
		PresetEquipmentItem clothing33 = new PresetEquipmentItem("Clothing", 15);
		List<PresetInventoryItem> inventory33 = new List<PresetInventoryItem>
		{
			new PresetInventoryItem("Food", 9, 3, 40),
			new PresetInventoryItem("Food", 135, 3, 40),
			new PresetInventoryItem("TeaWine", 18, 1, 20),
			new PresetInventoryItem("TeaWine", 27, 1, 20),
			new PresetInventoryItem("TeaWine", 0, 1, 20),
			new PresetInventoryItem("TeaWine", 9, 1, 20)
		};
		combatSkills2 = new List<PresetOrgMemberCombatSkill>();
		dataArray33.Add(new OrganizationMemberItem(212, 62, 38, 4, 3, 3, 3, restrictPrincipalAmount: false, -1, -1, -1, 4, 4, -1, 4, 0, 0, monasticTitleSuffixes33, 0, 0, 4500, 2250, -50, favoriteClothingIds33, hatedClothingIds33, spouseAnonymousTitles33, canStroll: true, -1, initialAges33, equipment33, clothing33, inventory33, combatSkills2, new sbyte[5], new short[8] { -80, -80, -80, -80, -80, -80, -50, -80 }, 22500, 18000, 10, 60, 9300, new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, 9
		}, 5, new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		}, new short[6] { -1, -1, -1, -1, -1, -1 }, new List<sbyte> { 4, 53 }, 14, new ResourceInts(default(int), default(int), default(int), default(int), default(int), default(int), default(int), default(int)), new IntPair[18]
		{
			new IntPair(17, 3),
			new IntPair(8, 3),
			new IntPair(5, 1),
			new IntPair(6, 1),
			new IntPair(15, 27),
			new IntPair(16, 15),
			new IntPair(7, 15),
			new IntPair(14, 1),
			new IntPair(12, 1),
			new IntPair(4, 6),
			new IntPair(3, 1),
			new IntPair(13, 3),
			new IntPair(10, 6),
			new IntPair(2, 3),
			new IntPair(1, 1),
			new IntPair(11, 9),
			new IntPair(0, 3),
			new IntPair(9, 1)
		}, null));
		List<OrganizationMemberItem> dataArray34 = _dataArray;
		int[] monasticTitleSuffixes34 = new int[2] { 999, 1000 };
		List<short> favoriteClothingIds34 = new List<short> { 13 };
		list = new List<short>();
		List<short> hatedClothingIds34 = list;
		int[] spouseAnonymousTitles34 = new int[2] { 1001, 1002 };
		short[] initialAges34 = new short[4] { 18, 22, 26, 30 };
		PresetEquipmentItemWithProb[] equipment34 = new PresetEquipmentItemWithProb[8]
		{
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Accessory", -1, 0),
			new PresetEquipmentItemWithProb("Accessory", -1, 0),
			new PresetEquipmentItemWithProb("Accessory", -1, 0),
			new PresetEquipmentItemWithProb("Carrier", 18, 75)
		};
		PresetEquipmentItem clothing34 = new PresetEquipmentItem("Clothing", 13);
		List<PresetInventoryItem> inventory34 = new List<PresetInventoryItem>
		{
			new PresetInventoryItem("CraftTool", 45, 1, 30),
			new PresetInventoryItem("Food", 135, 3, 40),
			new PresetInventoryItem("Medicine", 54, 1, 20),
			new PresetInventoryItem("Medicine", 66, 1, 20),
			new PresetInventoryItem("Medicine", 82, 1, 20),
			new PresetInventoryItem("Medicine", 94, 1, 20),
			new PresetInventoryItem("Material", 140, 1, 10),
			new PresetInventoryItem("Material", 144, 1, 10),
			new PresetInventoryItem("Material", 148, 1, 10),
			new PresetInventoryItem("Material", 152, 1, 10),
			new PresetInventoryItem("Material", 156, 1, 10),
			new PresetInventoryItem("Material", 160, 1, 10),
			new PresetInventoryItem("Material", 164, 1, 10),
			new PresetInventoryItem("Material", 168, 1, 10),
			new PresetInventoryItem("Material", 172, 1, 10),
			new PresetInventoryItem("Material", 176, 1, 10),
			new PresetInventoryItem("Material", 180, 1, 10),
			new PresetInventoryItem("Material", 184, 1, 10),
			new PresetInventoryItem("Material", 188, 1, 10),
			new PresetInventoryItem("Material", 192, 1, 10),
			new PresetInventoryItem("Material", 196, 1, 10),
			new PresetInventoryItem("Material", 200, 1, 10),
			new PresetInventoryItem("Material", 204, 1, 10),
			new PresetInventoryItem("Material", 208, 1, 10),
			new PresetInventoryItem("Material", 212, 1, 10),
			new PresetInventoryItem("Material", 216, 1, 10),
			new PresetInventoryItem("Material", 220, 1, 10),
			new PresetInventoryItem("Material", 224, 1, 10),
			new PresetInventoryItem("Material", 228, 1, 10),
			new PresetInventoryItem("Material", 232, 1, 10)
		};
		combatSkills2 = new List<PresetOrgMemberCombatSkill>();
		dataArray34.Add(new OrganizationMemberItem(213, 67, 38, 3, 3, 3, 3, restrictPrincipalAmount: false, -1, -1, -1, 3, 3, -1, 3, 0, 0, monasticTitleSuffixes34, 0, 0, 3000, 900, -25, favoriteClothingIds34, hatedClothingIds34, spouseAnonymousTitles34, canStroll: true, -1, initialAges34, equipment34, clothing34, inventory34, combatSkills2, new sbyte[5], new short[8] { -80, -80, -80, -80, -80, -50, -80, -80 }, 15000, 9000, 10, 70, 4500, new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, 12, 12,
			-1, -1, -1, -1, -1, -1
		}, 7, new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		}, new short[6] { -1, -1, -1, -1, -1, -1 }, new List<sbyte> { 5, 54, 64 }, 14, new ResourceInts(default(int), default(int), default(int), default(int), default(int), default(int), default(int), default(int)), new IntPair[18]
		{
			new IntPair(17, 1),
			new IntPair(8, 6),
			new IntPair(5, 1),
			new IntPair(6, 1),
			new IntPair(15, 1),
			new IntPair(16, 6),
			new IntPair(7, 1),
			new IntPair(14, 3),
			new IntPair(12, 3),
			new IntPair(4, 12),
			new IntPair(3, 1),
			new IntPair(13, 30),
			new IntPair(10, 18),
			new IntPair(2, 1),
			new IntPair(1, 1),
			new IntPair(11, 9),
			new IntPair(0, 12),
			new IntPair(9, 1)
		}, null));
		List<OrganizationMemberItem> dataArray35 = _dataArray;
		int[] monasticTitleSuffixes35 = new int[2] { 1003, 1004 };
		List<short> favoriteClothingIds35 = new List<short> { 0, 1, 2, 3, 9, 10, 11, 12 };
		List<short> hatedClothingIds35 = new List<short> { 8, 17 };
		int[] spouseAnonymousTitles35 = new int[2] { 1005, 1006 };
		short[] initialAges35 = new short[4] { 16, 19, 22, 25 };
		PresetEquipmentItemWithProb[] equipment35 = new PresetEquipmentItemWithProb[8]
		{
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Accessory", -1, 0),
			new PresetEquipmentItemWithProb("Accessory", -1, 0),
			new PresetEquipmentItemWithProb("Accessory", -1, 0),
			new PresetEquipmentItemWithProb("Carrier", 18, 75)
		};
		PresetEquipmentItem clothing35 = new PresetEquipmentItem("Clothing", 11);
		List<PresetInventoryItem> inventory35 = new List<PresetInventoryItem>
		{
			new PresetInventoryItem("CraftTool", 0, 1, 30),
			new PresetInventoryItem("CraftTool", 9, 1, 30),
			new PresetInventoryItem("CraftTool", 18, 1, 30),
			new PresetInventoryItem("CraftTool", 27, 1, 30),
			new PresetInventoryItem("Food", 93, 3, 40),
			new PresetInventoryItem("Material", 0, 1, 20),
			new PresetInventoryItem("Material", 7, 1, 20),
			new PresetInventoryItem("Material", 14, 1, 20),
			new PresetInventoryItem("Material", 21, 1, 20),
			new PresetInventoryItem("Material", 28, 1, 20),
			new PresetInventoryItem("Material", 35, 1, 20),
			new PresetInventoryItem("Material", 42, 1, 20),
			new PresetInventoryItem("Material", 49, 1, 20)
		};
		combatSkills2 = new List<PresetOrgMemberCombatSkill>();
		dataArray35.Add(new OrganizationMemberItem(214, 888, 38, 2, 3, 3, 3, restrictPrincipalAmount: false, -1, -1, -1, 2, 2, -1, 2, 0, 0, monasticTitleSuffixes35, 0, 0, 2000, 300, 0, favoriteClothingIds35, hatedClothingIds35, spouseAnonymousTitles35, canStroll: true, -1, initialAges35, equipment35, clothing35, inventory35, combatSkills2, new sbyte[5], new short[8] { -90, -50, -50, -50, -50, -90, -90, -90 }, 11250, 3000, 5, 80, 1800, new short[16]
		{
			-1, -1, -1, -1, -1, -1, 12, 12, -1, -1,
			12, 12, -1, -1, -1, -1
		}, 7, new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		}, new short[6] { -1, -1, -1, -1, -1, -1 }, new List<sbyte> { 6, 61, 63 }, 12, new ResourceInts(default(int), default(int), default(int), default(int), default(int), default(int), default(int), default(int)), new IntPair[18]
		{
			new IntPair(17, 1),
			new IntPair(8, 1),
			new IntPair(5, 1),
			new IntPair(6, 1),
			new IntPair(15, 6),
			new IntPair(16, 3),
			new IntPair(7, 1),
			new IntPair(14, 1),
			new IntPair(12, 1),
			new IntPair(4, 9),
			new IntPair(3, 1),
			new IntPair(13, 3),
			new IntPair(10, 18),
			new IntPair(2, 30),
			new IntPair(1, 9),
			new IntPair(11, 15),
			new IntPair(0, 6),
			new IntPair(9, 1)
		}, null));
		List<OrganizationMemberItem> dataArray36 = _dataArray;
		int[] monasticTitleSuffixes36 = new int[2] { 1007, 1008 };
		List<short> favoriteClothingIds36 = new List<short> { 0, 1, 2, 9, 10, 11 };
		List<short> hatedClothingIds36 = new List<short> { 7, 8, 16, 17 };
		int[] spouseAnonymousTitles36 = new int[2] { 1009, 1010 };
		short[] initialAges36 = new short[4] { 14, 16, 18, 20 };
		PresetEquipmentItemWithProb[] equipment36 = new PresetEquipmentItemWithProb[8]
		{
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Accessory", -1, 0),
			new PresetEquipmentItemWithProb("Accessory", -1, 0),
			new PresetEquipmentItemWithProb("Accessory", -1, 0),
			new PresetEquipmentItemWithProb("Carrier", -1, 0)
		};
		PresetEquipmentItem clothing36 = new PresetEquipmentItem("Clothing", 10);
		List<PresetInventoryItem> inventory36 = new List<PresetInventoryItem>
		{
			new PresetInventoryItem("CraftTool", 36, 1, 30),
			new PresetInventoryItem("Food", 0, 3, 40),
			new PresetInventoryItem("Food", 93, 3, 40),
			new PresetInventoryItem("Material", 57, 1, 20),
			new PresetInventoryItem("Material", 64, 1, 20),
			new PresetInventoryItem("Material", 71, 1, 20),
			new PresetInventoryItem("Material", 78, 1, 20)
		};
		combatSkills2 = new List<PresetOrgMemberCombatSkill>();
		dataArray36.Add(new OrganizationMemberItem(215, 893, 38, 1, 2, 2, 2, restrictPrincipalAmount: false, -1, -1, -1, 1, 1, -1, 1, 0, 0, monasticTitleSuffixes36, 0, 0, 1000, 150, 0, favoriteClothingIds36, hatedClothingIds36, spouseAnonymousTitles36, canStroll: true, -1, initialAges36, equipment36, clothing36, inventory36, combatSkills2, new sbyte[5], new short[8] { -50, -90, -90, -90, -90, -90, -90, -90 }, 7500, 1500, 5, 90, 600, new short[16]
		{
			-1, -1, -1, -1, -1, 12, -1, -1, -1, -1,
			-1, -1, -1, -1, 12, -1
		}, 4, new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		}, new short[6] { -1, -1, -1, -1, -1, -1 }, new List<sbyte> { 7, 60 }, 12, new ResourceInts(default(int), default(int), default(int), default(int), default(int), default(int), default(int), default(int)), new IntPair[18]
		{
			new IntPair(17, 3),
			new IntPair(8, 3),
			new IntPair(5, 1),
			new IntPair(6, 1),
			new IntPair(15, 6),
			new IntPair(16, 3),
			new IntPair(7, 3),
			new IntPair(14, 1),
			new IntPair(12, 1),
			new IntPair(4, 3),
			new IntPair(3, 1),
			new IntPair(13, 1),
			new IntPair(10, 30),
			new IntPair(2, 12),
			new IntPair(1, 9),
			new IntPair(11, 9),
			new IntPair(0, 3),
			new IntPair(9, 18)
		}, null));
		List<OrganizationMemberItem> dataArray37 = _dataArray;
		int[] monasticTitleSuffixes37 = new int[2] { 1011, 1012 };
		List<short> favoriteClothingIds37 = new List<short> { 0, 1, 9, 10 };
		List<short> hatedClothingIds37 = new List<short> { 6, 7, 8, 15, 16, 17 };
		int[] spouseAnonymousTitles37 = new int[2] { 1013, 1014 };
		short[] initialAges37 = new short[4] { 12, 13, 14, 15 };
		PresetEquipmentItemWithProb[] equipment37 = new PresetEquipmentItemWithProb[8]
		{
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Accessory", -1, 0),
			new PresetEquipmentItemWithProb("Accessory", -1, 0),
			new PresetEquipmentItemWithProb("Accessory", -1, 0),
			new PresetEquipmentItemWithProb("Carrier", -1, 0)
		};
		PresetEquipmentItem clothing37 = new PresetEquipmentItem("Clothing", 9);
		List<PresetInventoryItem> inventory37 = new List<PresetInventoryItem>
		{
			new PresetInventoryItem("Misc", 10, 1, 10),
			new PresetInventoryItem("Misc", 11, 1, 10),
			new PresetInventoryItem("Misc", 12, 1, 10),
			new PresetInventoryItem("Misc", 13, 1, 10),
			new PresetInventoryItem("Misc", 14, 1, 10),
			new PresetInventoryItem("Misc", 15, 1, 10),
			new PresetInventoryItem("Misc", 16, 1, 10),
			new PresetInventoryItem("Misc", 17, 1, 10),
			new PresetInventoryItem("Misc", 18, 1, 10),
			new PresetInventoryItem("Misc", 19, 1, 10),
			new PresetInventoryItem("Misc", 20, 1, 10),
			new PresetInventoryItem("Misc", 21, 1, 10),
			new PresetInventoryItem("Misc", 22, 1, 10),
			new PresetInventoryItem("Misc", 23, 1, 10),
			new PresetInventoryItem("Misc", 24, 1, 10),
			new PresetInventoryItem("Misc", 25, 1, 10),
			new PresetInventoryItem("Misc", 26, 1, 10),
			new PresetInventoryItem("Misc", 27, 1, 10)
		};
		combatSkills2 = new List<PresetOrgMemberCombatSkill>();
		dataArray37.Add(new OrganizationMemberItem(216, 898, 38, 0, 2, 2, 2, restrictPrincipalAmount: false, -1, -1, -1, 0, 0, -1, 0, 0, 0, monasticTitleSuffixes37, 0, 0, 500, 0, 0, favoriteClothingIds37, hatedClothingIds37, spouseAnonymousTitles37, canStroll: true, -1, initialAges37, equipment37, clothing37, inventory37, combatSkills2, new sbyte[5], new short[8] { -90, -90, -90, -90, -90, -90, -90, -90 }, 3750, 750, 1, 100, 300, new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, 12
		}, 3, new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		}, new short[6] { -1, -1, -1, -1, -1, -1 }, new List<sbyte> { 8, 44 }, 3, new ResourceInts(default(int), default(int), default(int), default(int), default(int), default(int), default(int), default(int)), new IntPair[18]
		{
			new IntPair(17, 3),
			new IntPair(8, 3),
			new IntPair(5, 1),
			new IntPair(6, 1),
			new IntPair(15, 1),
			new IntPair(16, 3),
			new IntPair(7, 3),
			new IntPair(14, 1),
			new IntPair(12, 1),
			new IntPair(4, 3),
			new IntPair(3, 1),
			new IntPair(13, 3),
			new IntPair(10, 27),
			new IntPair(2, 6),
			new IntPair(1, 9),
			new IntPair(11, 9),
			new IntPair(0, 3),
			new IntPair(9, 30)
		}, null));
		List<OrganizationMemberItem> dataArray38 = _dataArray;
		int[] monasticTitleSuffixes38 = new int[2] { 1016, 1017 };
		list = new List<short>();
		List<short> favoriteClothingIds38 = list;
		list = new List<short>();
		List<short> hatedClothingIds38 = list;
		int[] spouseAnonymousTitles38 = new int[2] { 1018, 1019 };
		short[] initialAges38 = new short[4] { -1, -1, -1, -1 };
		PresetEquipmentItemWithProb[] equipment38 = new PresetEquipmentItemWithProb[8]
		{
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Accessory", -1, 0),
			new PresetEquipmentItemWithProb("Accessory", -1, 0),
			new PresetEquipmentItemWithProb("Accessory", -1, 0),
			new PresetEquipmentItemWithProb("Carrier", -1, 0)
		};
		PresetEquipmentItem clothing38 = new PresetEquipmentItem("Clothing", -1);
		List<PresetInventoryItem> list2 = new List<PresetInventoryItem>();
		List<PresetInventoryItem> inventory38 = list2;
		combatSkills2 = new List<PresetOrgMemberCombatSkill>();
		List<PresetOrgMemberCombatSkill> combatSkills3 = combatSkills2;
		sbyte[] extraCombatSkillGrids2 = new sbyte[5] { 10, 10, 10, 10, 10 };
		short[] resourcesAdjust2 = new short[8];
		short[] lifeSkillsAdjust2 = new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		};
		short[] combatSkillsAdjust2 = new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		};
		short[] mainAttributesAdjust2 = new short[6] { -1, -1, -1, -1, -1, -1 };
		identityInteractConfig = new List<sbyte>();
		dataArray38.Add(new OrganizationMemberItem(217, 1015, 39, 8, 0, 0, 0, restrictPrincipalAmount: false, -1, -1, -1, 0, 0, -1, -1, 0, 0, monasticTitleSuffixes38, 0, 0, 0, 0, 0, favoriteClothingIds38, hatedClothingIds38, spouseAnonymousTitles38, canStroll: false, -1, initialAges38, equipment38, clothing38, inventory38, combatSkills3, extraCombatSkillGrids2, resourcesAdjust2, 0, 0, 0, 100, 61500, lifeSkillsAdjust2, 8, combatSkillsAdjust2, mainAttributesAdjust2, identityInteractConfig, 3, new ResourceInts(default(int), default(int), default(int), default(int), default(int), default(int), default(int), default(int)), new IntPair[0], null));
		List<OrganizationMemberItem> dataArray39 = _dataArray;
		int[] monasticTitleSuffixes39 = new int[2] { 1020, 1021 };
		list = new List<short>();
		List<short> favoriteClothingIds39 = list;
		list = new List<short>();
		List<short> hatedClothingIds39 = list;
		int[] spouseAnonymousTitles39 = new int[2] { 1022, 1023 };
		short[] initialAges39 = new short[4] { -1, -1, -1, -1 };
		PresetEquipmentItemWithProb[] equipment39 = new PresetEquipmentItemWithProb[8]
		{
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Accessory", -1, 0),
			new PresetEquipmentItemWithProb("Accessory", -1, 0),
			new PresetEquipmentItemWithProb("Accessory", -1, 0),
			new PresetEquipmentItemWithProb("Carrier", -1, 0)
		};
		PresetEquipmentItem clothing39 = new PresetEquipmentItem("Clothing", -1);
		list2 = new List<PresetInventoryItem>();
		List<PresetInventoryItem> inventory39 = list2;
		combatSkills2 = new List<PresetOrgMemberCombatSkill>();
		List<PresetOrgMemberCombatSkill> combatSkills4 = combatSkills2;
		sbyte[] extraCombatSkillGrids3 = new sbyte[5] { 8, 8, 8, 8, 8 };
		short[] resourcesAdjust3 = new short[8];
		short[] lifeSkillsAdjust3 = new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		};
		short[] combatSkillsAdjust3 = new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		};
		short[] mainAttributesAdjust3 = new short[6] { -1, -1, -1, -1, -1, -1 };
		identityInteractConfig = new List<sbyte>();
		dataArray39.Add(new OrganizationMemberItem(218, 1015, 39, 7, 0, 0, 0, restrictPrincipalAmount: false, -1, -1, -1, 0, 0, -1, -1, 0, 0, monasticTitleSuffixes39, 0, 0, 0, 0, 0, favoriteClothingIds39, hatedClothingIds39, spouseAnonymousTitles39, canStroll: false, -1, initialAges39, equipment39, clothing39, inventory39, combatSkills4, extraCombatSkillGrids3, resourcesAdjust3, 0, 0, 0, 100, 42300, lifeSkillsAdjust3, 8, combatSkillsAdjust3, mainAttributesAdjust3, identityInteractConfig, 3, new ResourceInts(default(int), default(int), default(int), default(int), default(int), default(int), default(int), default(int)), new IntPair[0], null));
		List<OrganizationMemberItem> dataArray40 = _dataArray;
		int[] monasticTitleSuffixes40 = new int[2] { 1024, 1025 };
		list = new List<short>();
		List<short> favoriteClothingIds40 = list;
		list = new List<short>();
		List<short> hatedClothingIds40 = list;
		int[] spouseAnonymousTitles40 = new int[2] { 1026, 1027 };
		short[] initialAges40 = new short[4] { -1, -1, -1, -1 };
		PresetEquipmentItemWithProb[] equipment40 = new PresetEquipmentItemWithProb[8]
		{
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Accessory", -1, 0),
			new PresetEquipmentItemWithProb("Accessory", -1, 0),
			new PresetEquipmentItemWithProb("Accessory", -1, 0),
			new PresetEquipmentItemWithProb("Carrier", -1, 0)
		};
		PresetEquipmentItem clothing40 = new PresetEquipmentItem("Clothing", -1);
		list2 = new List<PresetInventoryItem>();
		List<PresetInventoryItem> inventory40 = list2;
		combatSkills2 = new List<PresetOrgMemberCombatSkill>();
		List<PresetOrgMemberCombatSkill> combatSkills5 = combatSkills2;
		sbyte[] extraCombatSkillGrids4 = new sbyte[5] { 8, 8, 8, 8, 8 };
		short[] resourcesAdjust4 = new short[8];
		short[] lifeSkillsAdjust4 = new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		};
		short[] combatSkillsAdjust4 = new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		};
		short[] mainAttributesAdjust4 = new short[6] { -1, -1, -1, -1, -1, -1 };
		identityInteractConfig = new List<sbyte>();
		dataArray40.Add(new OrganizationMemberItem(219, 1015, 39, 6, 0, 0, 0, restrictPrincipalAmount: false, -1, -1, -1, 0, 0, -1, -1, 0, 0, monasticTitleSuffixes40, 0, 0, 0, 0, 0, favoriteClothingIds40, hatedClothingIds40, spouseAnonymousTitles40, canStroll: false, -1, initialAges40, equipment40, clothing40, inventory40, combatSkills5, extraCombatSkillGrids4, resourcesAdjust4, 0, 0, 0, 100, 27600, lifeSkillsAdjust4, 7, combatSkillsAdjust4, mainAttributesAdjust4, identityInteractConfig, 3, new ResourceInts(default(int), default(int), default(int), default(int), default(int), default(int), default(int), default(int)), new IntPair[0], null));
		List<OrganizationMemberItem> dataArray41 = _dataArray;
		int[] monasticTitleSuffixes41 = new int[2] { 1028, 1029 };
		list = new List<short>();
		List<short> favoriteClothingIds41 = list;
		list = new List<short>();
		List<short> hatedClothingIds41 = list;
		int[] spouseAnonymousTitles41 = new int[2] { 1030, 1031 };
		short[] initialAges41 = new short[4] { -1, -1, -1, -1 };
		PresetEquipmentItemWithProb[] equipment41 = new PresetEquipmentItemWithProb[8]
		{
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Accessory", -1, 0),
			new PresetEquipmentItemWithProb("Accessory", -1, 0),
			new PresetEquipmentItemWithProb("Accessory", -1, 0),
			new PresetEquipmentItemWithProb("Carrier", -1, 0)
		};
		PresetEquipmentItem clothing41 = new PresetEquipmentItem("Clothing", -1);
		list2 = new List<PresetInventoryItem>();
		List<PresetInventoryItem> inventory41 = list2;
		combatSkills2 = new List<PresetOrgMemberCombatSkill>();
		List<PresetOrgMemberCombatSkill> combatSkills6 = combatSkills2;
		sbyte[] extraCombatSkillGrids5 = new sbyte[5] { 6, 6, 6, 6, 6 };
		short[] resourcesAdjust5 = new short[8];
		short[] lifeSkillsAdjust5 = new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		};
		short[] combatSkillsAdjust5 = new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		};
		short[] mainAttributesAdjust5 = new short[6] { -1, -1, -1, -1, -1, -1 };
		identityInteractConfig = new List<sbyte>();
		dataArray41.Add(new OrganizationMemberItem(220, 1015, 39, 5, 0, 0, 0, restrictPrincipalAmount: false, -1, -1, -1, 0, 0, -1, -1, 0, 0, monasticTitleSuffixes41, 0, 0, 0, 0, 0, favoriteClothingIds41, hatedClothingIds41, spouseAnonymousTitles41, canStroll: false, -1, initialAges41, equipment41, clothing41, inventory41, combatSkills6, extraCombatSkillGrids5, resourcesAdjust5, 0, 0, 0, 100, 16800, lifeSkillsAdjust5, 7, combatSkillsAdjust5, mainAttributesAdjust5, identityInteractConfig, 3, new ResourceInts(default(int), default(int), default(int), default(int), default(int), default(int), default(int), default(int)), new IntPair[0], null));
		List<OrganizationMemberItem> dataArray42 = _dataArray;
		int[] monasticTitleSuffixes42 = new int[2] { 1032, 1033 };
		list = new List<short>();
		List<short> favoriteClothingIds42 = list;
		list = new List<short>();
		List<short> hatedClothingIds42 = list;
		int[] spouseAnonymousTitles42 = new int[2] { 1034, 1035 };
		short[] initialAges42 = new short[4] { -1, -1, -1, -1 };
		PresetEquipmentItemWithProb[] equipment42 = new PresetEquipmentItemWithProb[8]
		{
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Accessory", -1, 0),
			new PresetEquipmentItemWithProb("Accessory", -1, 0),
			new PresetEquipmentItemWithProb("Accessory", -1, 0),
			new PresetEquipmentItemWithProb("Carrier", -1, 0)
		};
		PresetEquipmentItem clothing42 = new PresetEquipmentItem("Clothing", -1);
		list2 = new List<PresetInventoryItem>();
		List<PresetInventoryItem> inventory42 = list2;
		combatSkills2 = new List<PresetOrgMemberCombatSkill>();
		List<PresetOrgMemberCombatSkill> combatSkills7 = combatSkills2;
		sbyte[] extraCombatSkillGrids6 = new sbyte[5] { 4, 4, 4, 4, 4 };
		short[] resourcesAdjust6 = new short[8];
		short[] lifeSkillsAdjust6 = new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		};
		short[] combatSkillsAdjust6 = new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		};
		short[] mainAttributesAdjust6 = new short[6] { -1, -1, -1, -1, -1, -1 };
		identityInteractConfig = new List<sbyte>();
		dataArray42.Add(new OrganizationMemberItem(221, 1015, 39, 4, 0, 0, 0, restrictPrincipalAmount: false, -1, -1, -1, 0, 0, -1, -1, 0, 0, monasticTitleSuffixes42, 0, 0, 0, 0, 0, favoriteClothingIds42, hatedClothingIds42, spouseAnonymousTitles42, canStroll: false, -1, initialAges42, equipment42, clothing42, inventory42, combatSkills7, extraCombatSkillGrids6, resourcesAdjust6, 0, 0, 0, 100, 9300, lifeSkillsAdjust6, 6, combatSkillsAdjust6, mainAttributesAdjust6, identityInteractConfig, 3, new ResourceInts(default(int), default(int), default(int), default(int), default(int), default(int), default(int), default(int)), new IntPair[0], null));
		List<OrganizationMemberItem> dataArray43 = _dataArray;
		int[] monasticTitleSuffixes43 = new int[2] { 1036, 1037 };
		list = new List<short>();
		List<short> favoriteClothingIds43 = list;
		list = new List<short>();
		List<short> hatedClothingIds43 = list;
		int[] spouseAnonymousTitles43 = new int[2] { 1038, 1039 };
		short[] initialAges43 = new short[4] { -1, -1, -1, -1 };
		PresetEquipmentItemWithProb[] equipment43 = new PresetEquipmentItemWithProb[8]
		{
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Accessory", -1, 0),
			new PresetEquipmentItemWithProb("Accessory", -1, 0),
			new PresetEquipmentItemWithProb("Accessory", -1, 0),
			new PresetEquipmentItemWithProb("Carrier", -1, 0)
		};
		PresetEquipmentItem clothing43 = new PresetEquipmentItem("Clothing", -1);
		list2 = new List<PresetInventoryItem>();
		List<PresetInventoryItem> inventory43 = list2;
		combatSkills2 = new List<PresetOrgMemberCombatSkill>();
		List<PresetOrgMemberCombatSkill> combatSkills8 = combatSkills2;
		sbyte[] extraCombatSkillGrids7 = new sbyte[5] { 4, 4, 4, 4, 4 };
		short[] resourcesAdjust7 = new short[8];
		short[] lifeSkillsAdjust7 = new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		};
		short[] combatSkillsAdjust7 = new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		};
		short[] mainAttributesAdjust7 = new short[6] { -1, -1, -1, -1, -1, -1 };
		identityInteractConfig = new List<sbyte>();
		dataArray43.Add(new OrganizationMemberItem(222, 1015, 39, 3, 0, 0, 0, restrictPrincipalAmount: false, -1, -1, -1, 0, 0, -1, -1, 0, 0, monasticTitleSuffixes43, 0, 0, 0, 0, 0, favoriteClothingIds43, hatedClothingIds43, spouseAnonymousTitles43, canStroll: false, -1, initialAges43, equipment43, clothing43, inventory43, combatSkills8, extraCombatSkillGrids7, resourcesAdjust7, 0, 0, 0, 100, 4500, lifeSkillsAdjust7, 5, combatSkillsAdjust7, mainAttributesAdjust7, identityInteractConfig, 3, new ResourceInts(default(int), default(int), default(int), default(int), default(int), default(int), default(int), default(int)), new IntPair[0], null));
		List<OrganizationMemberItem> dataArray44 = _dataArray;
		int[] monasticTitleSuffixes44 = new int[2] { 1040, 1041 };
		list = new List<short>();
		List<short> favoriteClothingIds44 = list;
		list = new List<short>();
		List<short> hatedClothingIds44 = list;
		int[] spouseAnonymousTitles44 = new int[2] { 1042, 1043 };
		short[] initialAges44 = new short[4] { -1, -1, -1, -1 };
		PresetEquipmentItemWithProb[] equipment44 = new PresetEquipmentItemWithProb[8]
		{
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Accessory", -1, 0),
			new PresetEquipmentItemWithProb("Accessory", -1, 0),
			new PresetEquipmentItemWithProb("Accessory", -1, 0),
			new PresetEquipmentItemWithProb("Carrier", -1, 0)
		};
		PresetEquipmentItem clothing44 = new PresetEquipmentItem("Clothing", -1);
		list2 = new List<PresetInventoryItem>();
		List<PresetInventoryItem> inventory44 = list2;
		combatSkills2 = new List<PresetOrgMemberCombatSkill>();
		List<PresetOrgMemberCombatSkill> combatSkills9 = combatSkills2;
		sbyte[] extraCombatSkillGrids8 = new sbyte[5] { 2, 2, 2, 2, 2 };
		short[] resourcesAdjust8 = new short[8];
		short[] lifeSkillsAdjust8 = new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		};
		short[] combatSkillsAdjust8 = new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		};
		short[] mainAttributesAdjust8 = new short[6] { -1, -1, -1, -1, -1, -1 };
		identityInteractConfig = new List<sbyte>();
		dataArray44.Add(new OrganizationMemberItem(223, 1015, 39, 2, 0, 0, 0, restrictPrincipalAmount: false, -1, -1, -1, 0, 0, -1, -1, 0, 0, monasticTitleSuffixes44, 0, 0, 0, 0, 0, favoriteClothingIds44, hatedClothingIds44, spouseAnonymousTitles44, canStroll: false, -1, initialAges44, equipment44, clothing44, inventory44, combatSkills9, extraCombatSkillGrids8, resourcesAdjust8, 0, 0, 0, 100, 1800, lifeSkillsAdjust8, 4, combatSkillsAdjust8, mainAttributesAdjust8, identityInteractConfig, 3, new ResourceInts(default(int), default(int), default(int), default(int), default(int), default(int), default(int), default(int)), new IntPair[0], null));
		List<OrganizationMemberItem> dataArray45 = _dataArray;
		int[] monasticTitleSuffixes45 = new int[2] { 1044, 1045 };
		list = new List<short>();
		List<short> favoriteClothingIds45 = list;
		list = new List<short>();
		List<short> hatedClothingIds45 = list;
		int[] spouseAnonymousTitles45 = new int[2] { 1046, 1047 };
		short[] initialAges45 = new short[4] { -1, -1, -1, -1 };
		PresetEquipmentItemWithProb[] equipment45 = new PresetEquipmentItemWithProb[8]
		{
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Accessory", -1, 0),
			new PresetEquipmentItemWithProb("Accessory", -1, 0),
			new PresetEquipmentItemWithProb("Accessory", -1, 0),
			new PresetEquipmentItemWithProb("Carrier", -1, 0)
		};
		PresetEquipmentItem clothing45 = new PresetEquipmentItem("Clothing", -1);
		list2 = new List<PresetInventoryItem>();
		List<PresetInventoryItem> inventory45 = list2;
		combatSkills2 = new List<PresetOrgMemberCombatSkill>();
		List<PresetOrgMemberCombatSkill> combatSkills10 = combatSkills2;
		sbyte[] extraCombatSkillGrids9 = new sbyte[5];
		short[] resourcesAdjust9 = new short[8];
		short[] lifeSkillsAdjust9 = new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		};
		short[] combatSkillsAdjust9 = new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		};
		short[] mainAttributesAdjust9 = new short[6] { -1, -1, -1, -1, -1, -1 };
		identityInteractConfig = new List<sbyte>();
		dataArray45.Add(new OrganizationMemberItem(224, 1015, 39, 1, 0, 0, 0, restrictPrincipalAmount: false, -1, -1, -1, 0, 0, -1, -1, 0, 0, monasticTitleSuffixes45, 0, 0, 0, 0, 0, favoriteClothingIds45, hatedClothingIds45, spouseAnonymousTitles45, canStroll: false, -1, initialAges45, equipment45, clothing45, inventory45, combatSkills10, extraCombatSkillGrids9, resourcesAdjust9, 0, 0, 0, 100, 600, lifeSkillsAdjust9, 3, combatSkillsAdjust9, mainAttributesAdjust9, identityInteractConfig, 3, new ResourceInts(default(int), default(int), default(int), default(int), default(int), default(int), default(int), default(int)), new IntPair[0], null));
		List<OrganizationMemberItem> dataArray46 = _dataArray;
		int[] monasticTitleSuffixes46 = new int[2] { 1048, 1049 };
		list = new List<short>();
		List<short> favoriteClothingIds46 = list;
		list = new List<short>();
		List<short> hatedClothingIds46 = list;
		int[] spouseAnonymousTitles46 = new int[2] { 1050, 1051 };
		short[] initialAges46 = new short[4] { -1, -1, -1, -1 };
		PresetEquipmentItemWithProb[] equipment46 = new PresetEquipmentItemWithProb[8]
		{
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Accessory", -1, 0),
			new PresetEquipmentItemWithProb("Accessory", -1, 0),
			new PresetEquipmentItemWithProb("Accessory", -1, 0),
			new PresetEquipmentItemWithProb("Carrier", -1, 0)
		};
		PresetEquipmentItem clothing46 = new PresetEquipmentItem("Clothing", -1);
		list2 = new List<PresetInventoryItem>();
		List<PresetInventoryItem> inventory46 = list2;
		combatSkills2 = new List<PresetOrgMemberCombatSkill>();
		List<PresetOrgMemberCombatSkill> combatSkills11 = combatSkills2;
		sbyte[] extraCombatSkillGrids10 = new sbyte[5];
		short[] resourcesAdjust10 = new short[8];
		short[] lifeSkillsAdjust10 = new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		};
		short[] combatSkillsAdjust10 = new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		};
		short[] mainAttributesAdjust10 = new short[6] { -1, -1, -1, -1, -1, -1 };
		identityInteractConfig = new List<sbyte>();
		dataArray46.Add(new OrganizationMemberItem(225, 1015, 39, 0, 0, 0, 0, restrictPrincipalAmount: false, -1, -1, -1, 0, 0, -1, -1, 0, 0, monasticTitleSuffixes46, 0, 0, 0, 0, 0, favoriteClothingIds46, hatedClothingIds46, spouseAnonymousTitles46, canStroll: false, -1, initialAges46, equipment46, clothing46, inventory46, combatSkills11, extraCombatSkillGrids10, resourcesAdjust10, 0, 0, 0, 100, 300, lifeSkillsAdjust10, 2, combatSkillsAdjust10, mainAttributesAdjust10, identityInteractConfig, 3, new ResourceInts(default(int), default(int), default(int), default(int), default(int), default(int), default(int), default(int)), new IntPair[0], null));
		List<OrganizationMemberItem> dataArray47 = _dataArray;
		int[] monasticTitleSuffixes47 = new int[2] { 1053, 1054 };
		list = new List<short>();
		List<short> favoriteClothingIds47 = list;
		list = new List<short>();
		List<short> hatedClothingIds47 = list;
		int[] spouseAnonymousTitles47 = new int[2] { 1055, 1056 };
		short[] initialAges47 = new short[4] { -1, -1, -1, -1 };
		PresetEquipmentItemWithProb[] equipment47 = new PresetEquipmentItemWithProb[8]
		{
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Accessory", -1, 0),
			new PresetEquipmentItemWithProb("Accessory", -1, 0),
			new PresetEquipmentItemWithProb("Accessory", -1, 0),
			new PresetEquipmentItemWithProb("Carrier", -1, 0)
		};
		PresetEquipmentItem clothing47 = new PresetEquipmentItem("Clothing", -1);
		list2 = new List<PresetInventoryItem>();
		List<PresetInventoryItem> inventory47 = list2;
		combatSkills2 = new List<PresetOrgMemberCombatSkill>();
		List<PresetOrgMemberCombatSkill> combatSkills12 = combatSkills2;
		sbyte[] extraCombatSkillGrids11 = new sbyte[5];
		short[] resourcesAdjust11 = new short[8];
		short[] lifeSkillsAdjust11 = new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		};
		short[] combatSkillsAdjust11 = new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		};
		short[] mainAttributesAdjust11 = new short[6] { -1, -1, -1, -1, -1, -1 };
		identityInteractConfig = new List<sbyte>();
		dataArray47.Add(new OrganizationMemberItem(226, 1052, 40, 8, 0, 0, 0, restrictPrincipalAmount: false, -1, -1, -1, 0, 0, -1, -1, 0, 0, monasticTitleSuffixes47, 0, 0, 0, 0, 0, favoriteClothingIds47, hatedClothingIds47, spouseAnonymousTitles47, canStroll: false, -1, initialAges47, equipment47, clothing47, inventory47, combatSkills12, extraCombatSkillGrids11, resourcesAdjust11, 0, 0, 0, 100, 61500, lifeSkillsAdjust11, 8, combatSkillsAdjust11, mainAttributesAdjust11, identityInteractConfig, 3, new ResourceInts(default(int), default(int), default(int), default(int), default(int), default(int), default(int), default(int)), new IntPair[0], null));
		List<OrganizationMemberItem> dataArray48 = _dataArray;
		int[] monasticTitleSuffixes48 = new int[2] { 1057, 1058 };
		list = new List<short>();
		List<short> favoriteClothingIds48 = list;
		list = new List<short>();
		List<short> hatedClothingIds48 = list;
		int[] spouseAnonymousTitles48 = new int[2] { 1059, 1060 };
		short[] initialAges48 = new short[4] { -1, -1, -1, -1 };
		PresetEquipmentItemWithProb[] equipment48 = new PresetEquipmentItemWithProb[8]
		{
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Accessory", -1, 0),
			new PresetEquipmentItemWithProb("Accessory", -1, 0),
			new PresetEquipmentItemWithProb("Accessory", -1, 0),
			new PresetEquipmentItemWithProb("Carrier", -1, 0)
		};
		PresetEquipmentItem clothing48 = new PresetEquipmentItem("Clothing", -1);
		list2 = new List<PresetInventoryItem>();
		List<PresetInventoryItem> inventory48 = list2;
		combatSkills2 = new List<PresetOrgMemberCombatSkill>();
		List<PresetOrgMemberCombatSkill> combatSkills13 = combatSkills2;
		sbyte[] extraCombatSkillGrids12 = new sbyte[5];
		short[] resourcesAdjust12 = new short[8];
		short[] lifeSkillsAdjust12 = new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		};
		short[] combatSkillsAdjust12 = new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		};
		short[] mainAttributesAdjust12 = new short[6] { -1, -1, -1, -1, -1, -1 };
		identityInteractConfig = new List<sbyte>();
		dataArray48.Add(new OrganizationMemberItem(227, 1052, 40, 7, 0, 0, 0, restrictPrincipalAmount: false, -1, -1, -1, 0, 0, -1, -1, 0, 0, monasticTitleSuffixes48, 0, 0, 0, 0, 0, favoriteClothingIds48, hatedClothingIds48, spouseAnonymousTitles48, canStroll: false, -1, initialAges48, equipment48, clothing48, inventory48, combatSkills13, extraCombatSkillGrids12, resourcesAdjust12, 0, 0, 0, 100, 42300, lifeSkillsAdjust12, 8, combatSkillsAdjust12, mainAttributesAdjust12, identityInteractConfig, 3, new ResourceInts(default(int), default(int), default(int), default(int), default(int), default(int), default(int), default(int)), new IntPair[0], null));
		List<OrganizationMemberItem> dataArray49 = _dataArray;
		int[] monasticTitleSuffixes49 = new int[2] { 1061, 1062 };
		list = new List<short>();
		List<short> favoriteClothingIds49 = list;
		list = new List<short>();
		List<short> hatedClothingIds49 = list;
		int[] spouseAnonymousTitles49 = new int[2] { 1063, 1064 };
		short[] initialAges49 = new short[4] { -1, -1, -1, -1 };
		PresetEquipmentItemWithProb[] equipment49 = new PresetEquipmentItemWithProb[8]
		{
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Accessory", -1, 0),
			new PresetEquipmentItemWithProb("Accessory", -1, 0),
			new PresetEquipmentItemWithProb("Accessory", -1, 0),
			new PresetEquipmentItemWithProb("Carrier", -1, 0)
		};
		PresetEquipmentItem clothing49 = new PresetEquipmentItem("Clothing", -1);
		list2 = new List<PresetInventoryItem>();
		List<PresetInventoryItem> inventory49 = list2;
		combatSkills2 = new List<PresetOrgMemberCombatSkill>();
		List<PresetOrgMemberCombatSkill> combatSkills14 = combatSkills2;
		sbyte[] extraCombatSkillGrids13 = new sbyte[5];
		short[] resourcesAdjust13 = new short[8];
		short[] lifeSkillsAdjust13 = new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		};
		short[] combatSkillsAdjust13 = new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		};
		short[] mainAttributesAdjust13 = new short[6] { -1, -1, -1, -1, -1, -1 };
		identityInteractConfig = new List<sbyte>();
		dataArray49.Add(new OrganizationMemberItem(228, 1052, 40, 6, 0, 0, 0, restrictPrincipalAmount: false, -1, -1, -1, 0, 0, -1, -1, 0, 0, monasticTitleSuffixes49, 0, 0, 0, 0, 0, favoriteClothingIds49, hatedClothingIds49, spouseAnonymousTitles49, canStroll: false, -1, initialAges49, equipment49, clothing49, inventory49, combatSkills14, extraCombatSkillGrids13, resourcesAdjust13, 0, 0, 0, 100, 27600, lifeSkillsAdjust13, 7, combatSkillsAdjust13, mainAttributesAdjust13, identityInteractConfig, 3, new ResourceInts(default(int), default(int), default(int), default(int), default(int), default(int), default(int), default(int)), new IntPair[0], null));
		List<OrganizationMemberItem> dataArray50 = _dataArray;
		int[] monasticTitleSuffixes50 = new int[2] { 1066, 1067 };
		list = new List<short>();
		List<short> favoriteClothingIds50 = list;
		list = new List<short>();
		List<short> hatedClothingIds50 = list;
		int[] spouseAnonymousTitles50 = new int[2] { 1068, 1069 };
		short[] initialAges50 = new short[4] { -1, -1, -1, -1 };
		PresetEquipmentItemWithProb[] equipment50 = new PresetEquipmentItemWithProb[8]
		{
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Accessory", -1, 0),
			new PresetEquipmentItemWithProb("Accessory", -1, 0),
			new PresetEquipmentItemWithProb("Accessory", -1, 0),
			new PresetEquipmentItemWithProb("Carrier", -1, 0)
		};
		PresetEquipmentItem clothing50 = new PresetEquipmentItem("Clothing", -1);
		list2 = new List<PresetInventoryItem>();
		List<PresetInventoryItem> inventory50 = list2;
		combatSkills2 = new List<PresetOrgMemberCombatSkill>();
		List<PresetOrgMemberCombatSkill> combatSkills15 = combatSkills2;
		sbyte[] extraCombatSkillGrids14 = new sbyte[5];
		short[] resourcesAdjust14 = new short[8];
		short[] lifeSkillsAdjust14 = new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		};
		short[] combatSkillsAdjust14 = new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		};
		short[] mainAttributesAdjust14 = new short[6] { -1, -1, -1, -1, -1, -1 };
		identityInteractConfig = new List<sbyte>();
		dataArray50.Add(new OrganizationMemberItem(229, 1065, 40, 5, 0, 0, 0, restrictPrincipalAmount: false, -1, -1, -1, 0, 0, -1, -1, 0, 0, monasticTitleSuffixes50, 0, 0, 0, 0, 0, favoriteClothingIds50, hatedClothingIds50, spouseAnonymousTitles50, canStroll: false, -1, initialAges50, equipment50, clothing50, inventory50, combatSkills15, extraCombatSkillGrids14, resourcesAdjust14, 0, 0, 0, 100, 16800, lifeSkillsAdjust14, 7, combatSkillsAdjust14, mainAttributesAdjust14, identityInteractConfig, 3, new ResourceInts(default(int), default(int), default(int), default(int), default(int), default(int), default(int), default(int)), new IntPair[0], null));
		List<OrganizationMemberItem> dataArray51 = _dataArray;
		int[] monasticTitleSuffixes51 = new int[2] { 1070, 1071 };
		list = new List<short>();
		List<short> favoriteClothingIds51 = list;
		list = new List<short>();
		List<short> hatedClothingIds51 = list;
		int[] spouseAnonymousTitles51 = new int[2] { 1072, 1073 };
		short[] initialAges51 = new short[4] { -1, -1, -1, -1 };
		PresetEquipmentItemWithProb[] equipment51 = new PresetEquipmentItemWithProb[8]
		{
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Accessory", -1, 0),
			new PresetEquipmentItemWithProb("Accessory", -1, 0),
			new PresetEquipmentItemWithProb("Accessory", -1, 0),
			new PresetEquipmentItemWithProb("Carrier", -1, 0)
		};
		PresetEquipmentItem clothing51 = new PresetEquipmentItem("Clothing", -1);
		list2 = new List<PresetInventoryItem>();
		List<PresetInventoryItem> inventory51 = list2;
		combatSkills2 = new List<PresetOrgMemberCombatSkill>();
		List<PresetOrgMemberCombatSkill> combatSkills16 = combatSkills2;
		sbyte[] extraCombatSkillGrids15 = new sbyte[5];
		short[] resourcesAdjust15 = new short[8];
		short[] lifeSkillsAdjust15 = new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		};
		short[] combatSkillsAdjust15 = new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		};
		short[] mainAttributesAdjust15 = new short[6] { -1, -1, -1, -1, -1, -1 };
		identityInteractConfig = new List<sbyte>();
		dataArray51.Add(new OrganizationMemberItem(230, 1065, 40, 4, 0, 0, 0, restrictPrincipalAmount: false, -1, -1, -1, 0, 0, -1, -1, 0, 0, monasticTitleSuffixes51, 0, 0, 0, 0, 0, favoriteClothingIds51, hatedClothingIds51, spouseAnonymousTitles51, canStroll: false, -1, initialAges51, equipment51, clothing51, inventory51, combatSkills16, extraCombatSkillGrids15, resourcesAdjust15, 0, 0, 0, 100, 9300, lifeSkillsAdjust15, 6, combatSkillsAdjust15, mainAttributesAdjust15, identityInteractConfig, 3, new ResourceInts(default(int), default(int), default(int), default(int), default(int), default(int), default(int), default(int)), new IntPair[0], null));
		List<OrganizationMemberItem> dataArray52 = _dataArray;
		int[] monasticTitleSuffixes52 = new int[2] { 1075, 1076 };
		list = new List<short>();
		List<short> favoriteClothingIds52 = list;
		list = new List<short>();
		List<short> hatedClothingIds52 = list;
		int[] spouseAnonymousTitles52 = new int[2] { 1077, 1078 };
		short[] initialAges52 = new short[4] { -1, -1, -1, -1 };
		PresetEquipmentItemWithProb[] equipment52 = new PresetEquipmentItemWithProb[8]
		{
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Accessory", -1, 0),
			new PresetEquipmentItemWithProb("Accessory", -1, 0),
			new PresetEquipmentItemWithProb("Accessory", -1, 0),
			new PresetEquipmentItemWithProb("Carrier", -1, 0)
		};
		PresetEquipmentItem clothing52 = new PresetEquipmentItem("Clothing", -1);
		list2 = new List<PresetInventoryItem>();
		List<PresetInventoryItem> inventory52 = list2;
		combatSkills2 = new List<PresetOrgMemberCombatSkill>();
		List<PresetOrgMemberCombatSkill> combatSkills17 = combatSkills2;
		sbyte[] extraCombatSkillGrids16 = new sbyte[5];
		short[] resourcesAdjust16 = new short[8];
		short[] lifeSkillsAdjust16 = new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		};
		short[] combatSkillsAdjust16 = new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		};
		short[] mainAttributesAdjust16 = new short[6] { -1, -1, -1, -1, -1, -1 };
		identityInteractConfig = new List<sbyte>();
		dataArray52.Add(new OrganizationMemberItem(231, 1074, 40, 3, 0, 0, 0, restrictPrincipalAmount: false, -1, -1, -1, 0, 0, -1, -1, 0, 0, monasticTitleSuffixes52, 0, 0, 0, 0, 0, favoriteClothingIds52, hatedClothingIds52, spouseAnonymousTitles52, canStroll: false, -1, initialAges52, equipment52, clothing52, inventory52, combatSkills17, extraCombatSkillGrids16, resourcesAdjust16, 0, 0, 0, 100, 4500, lifeSkillsAdjust16, 5, combatSkillsAdjust16, mainAttributesAdjust16, identityInteractConfig, 3, new ResourceInts(default(int), default(int), default(int), default(int), default(int), default(int), default(int), default(int)), new IntPair[0], null));
		List<OrganizationMemberItem> dataArray53 = _dataArray;
		int[] monasticTitleSuffixes53 = new int[2] { 1079, 1080 };
		list = new List<short>();
		List<short> favoriteClothingIds53 = list;
		list = new List<short>();
		List<short> hatedClothingIds53 = list;
		int[] spouseAnonymousTitles53 = new int[2] { 1081, 1082 };
		short[] initialAges53 = new short[4] { -1, -1, -1, -1 };
		PresetEquipmentItemWithProb[] equipment53 = new PresetEquipmentItemWithProb[8]
		{
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Accessory", -1, 0),
			new PresetEquipmentItemWithProb("Accessory", -1, 0),
			new PresetEquipmentItemWithProb("Accessory", -1, 0),
			new PresetEquipmentItemWithProb("Carrier", -1, 0)
		};
		PresetEquipmentItem clothing53 = new PresetEquipmentItem("Clothing", -1);
		list2 = new List<PresetInventoryItem>();
		List<PresetInventoryItem> inventory53 = list2;
		combatSkills2 = new List<PresetOrgMemberCombatSkill>();
		List<PresetOrgMemberCombatSkill> combatSkills18 = combatSkills2;
		sbyte[] extraCombatSkillGrids17 = new sbyte[5];
		short[] resourcesAdjust17 = new short[8];
		short[] lifeSkillsAdjust17 = new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		};
		short[] combatSkillsAdjust17 = new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		};
		short[] mainAttributesAdjust17 = new short[6] { -1, -1, -1, -1, -1, -1 };
		identityInteractConfig = new List<sbyte>();
		dataArray53.Add(new OrganizationMemberItem(232, 1074, 40, 2, 0, 0, 0, restrictPrincipalAmount: false, -1, -1, -1, 0, 0, -1, -1, 0, 0, monasticTitleSuffixes53, 0, 0, 0, 0, 0, favoriteClothingIds53, hatedClothingIds53, spouseAnonymousTitles53, canStroll: false, -1, initialAges53, equipment53, clothing53, inventory53, combatSkills18, extraCombatSkillGrids17, resourcesAdjust17, 0, 0, 0, 100, 1800, lifeSkillsAdjust17, 4, combatSkillsAdjust17, mainAttributesAdjust17, identityInteractConfig, 3, new ResourceInts(default(int), default(int), default(int), default(int), default(int), default(int), default(int), default(int)), new IntPair[0], null));
		List<OrganizationMemberItem> dataArray54 = _dataArray;
		int[] monasticTitleSuffixes54 = new int[2] { 1084, 1085 };
		list = new List<short>();
		List<short> favoriteClothingIds54 = list;
		list = new List<short>();
		List<short> hatedClothingIds54 = list;
		int[] spouseAnonymousTitles54 = new int[2] { 1086, 1087 };
		short[] initialAges54 = new short[4] { -1, -1, -1, -1 };
		PresetEquipmentItemWithProb[] equipment54 = new PresetEquipmentItemWithProb[8]
		{
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Accessory", -1, 0),
			new PresetEquipmentItemWithProb("Accessory", -1, 0),
			new PresetEquipmentItemWithProb("Accessory", -1, 0),
			new PresetEquipmentItemWithProb("Carrier", -1, 0)
		};
		PresetEquipmentItem clothing54 = new PresetEquipmentItem("Clothing", -1);
		list2 = new List<PresetInventoryItem>();
		List<PresetInventoryItem> inventory54 = list2;
		combatSkills2 = new List<PresetOrgMemberCombatSkill>();
		List<PresetOrgMemberCombatSkill> combatSkills19 = combatSkills2;
		sbyte[] extraCombatSkillGrids18 = new sbyte[5];
		short[] resourcesAdjust18 = new short[8];
		short[] lifeSkillsAdjust18 = new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		};
		short[] combatSkillsAdjust18 = new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		};
		short[] mainAttributesAdjust18 = new short[6] { -1, -1, -1, -1, -1, -1 };
		identityInteractConfig = new List<sbyte>();
		dataArray54.Add(new OrganizationMemberItem(233, 1083, 40, 1, 0, 0, 0, restrictPrincipalAmount: false, -1, -1, -1, 0, 0, -1, -1, 0, 0, monasticTitleSuffixes54, 0, 0, 0, 0, 0, favoriteClothingIds54, hatedClothingIds54, spouseAnonymousTitles54, canStroll: false, -1, initialAges54, equipment54, clothing54, inventory54, combatSkills19, extraCombatSkillGrids18, resourcesAdjust18, 0, 0, 0, 100, 600, lifeSkillsAdjust18, 3, combatSkillsAdjust18, mainAttributesAdjust18, identityInteractConfig, 3, new ResourceInts(default(int), default(int), default(int), default(int), default(int), default(int), default(int), default(int)), new IntPair[0], null));
		List<OrganizationMemberItem> dataArray55 = _dataArray;
		int[] monasticTitleSuffixes55 = new int[2] { 1088, 1089 };
		list = new List<short>();
		List<short> favoriteClothingIds55 = list;
		list = new List<short>();
		List<short> hatedClothingIds55 = list;
		int[] spouseAnonymousTitles55 = new int[2] { 1090, 1091 };
		short[] initialAges55 = new short[4] { -1, -1, -1, -1 };
		PresetEquipmentItemWithProb[] equipment55 = new PresetEquipmentItemWithProb[8]
		{
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Accessory", -1, 0),
			new PresetEquipmentItemWithProb("Accessory", -1, 0),
			new PresetEquipmentItemWithProb("Accessory", -1, 0),
			new PresetEquipmentItemWithProb("Carrier", -1, 0)
		};
		PresetEquipmentItem clothing55 = new PresetEquipmentItem("Clothing", -1);
		list2 = new List<PresetInventoryItem>();
		List<PresetInventoryItem> inventory55 = list2;
		combatSkills2 = new List<PresetOrgMemberCombatSkill>();
		List<PresetOrgMemberCombatSkill> combatSkills20 = combatSkills2;
		sbyte[] extraCombatSkillGrids19 = new sbyte[5];
		short[] resourcesAdjust19 = new short[8];
		short[] lifeSkillsAdjust19 = new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		};
		short[] combatSkillsAdjust19 = new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		};
		short[] mainAttributesAdjust19 = new short[6] { -1, -1, -1, -1, -1, -1 };
		identityInteractConfig = new List<sbyte>();
		dataArray55.Add(new OrganizationMemberItem(234, 1083, 40, 0, 0, 0, 0, restrictPrincipalAmount: false, -1, -1, -1, 0, 0, -1, -1, 0, 0, monasticTitleSuffixes55, 0, 0, 0, 0, 0, favoriteClothingIds55, hatedClothingIds55, spouseAnonymousTitles55, canStroll: false, -1, initialAges55, equipment55, clothing55, inventory55, combatSkills20, extraCombatSkillGrids19, resourcesAdjust19, 0, 0, 0, 100, 300, lifeSkillsAdjust19, 2, combatSkillsAdjust19, mainAttributesAdjust19, identityInteractConfig, 3, new ResourceInts(default(int), default(int), default(int), default(int), default(int), default(int), default(int), default(int)), new IntPair[0], null));
		List<OrganizationMemberItem> dataArray56 = _dataArray;
		int[] monasticTitleSuffixes56 = new int[2] { 1093, 1094 };
		list = new List<short>();
		List<short> favoriteClothingIds56 = list;
		list = new List<short>();
		List<short> hatedClothingIds56 = list;
		int[] spouseAnonymousTitles56 = new int[2] { 1095, 1096 };
		short[] initialAges56 = new short[4] { -1, -1, -1, -1 };
		PresetEquipmentItemWithProb[] equipment56 = new PresetEquipmentItemWithProb[8]
		{
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Accessory", -1, 0),
			new PresetEquipmentItemWithProb("Accessory", -1, 0),
			new PresetEquipmentItemWithProb("Accessory", -1, 0),
			new PresetEquipmentItemWithProb("Carrier", -1, 0)
		};
		PresetEquipmentItem clothing56 = new PresetEquipmentItem("Clothing", -1);
		list2 = new List<PresetInventoryItem>();
		List<PresetInventoryItem> inventory56 = list2;
		combatSkills2 = new List<PresetOrgMemberCombatSkill>();
		List<PresetOrgMemberCombatSkill> combatSkills21 = combatSkills2;
		sbyte[] extraCombatSkillGrids20 = new sbyte[5];
		short[] resourcesAdjust20 = new short[8];
		short[] lifeSkillsAdjust20 = new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		};
		short[] combatSkillsAdjust20 = new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		};
		short[] mainAttributesAdjust20 = new short[6] { -1, -1, -1, -1, -1, -1 };
		identityInteractConfig = new List<sbyte>();
		dataArray56.Add(new OrganizationMemberItem(235, 1092, 41, 8, 0, 0, 0, restrictPrincipalAmount: false, -1, -1, -1, 0, 0, -1, -1, 0, 0, monasticTitleSuffixes56, 0, 0, 0, 0, 0, favoriteClothingIds56, hatedClothingIds56, spouseAnonymousTitles56, canStroll: false, -1, initialAges56, equipment56, clothing56, inventory56, combatSkills21, extraCombatSkillGrids20, resourcesAdjust20, 0, 0, 0, 100, 61500, lifeSkillsAdjust20, 8, combatSkillsAdjust20, mainAttributesAdjust20, identityInteractConfig, 3, new ResourceInts(default(int), default(int), default(int), default(int), default(int), default(int), default(int), default(int)), new IntPair[0], null));
		List<OrganizationMemberItem> dataArray57 = _dataArray;
		int[] monasticTitleSuffixes57 = new int[2] { 1097, 1098 };
		list = new List<short>();
		List<short> favoriteClothingIds57 = list;
		list = new List<short>();
		List<short> hatedClothingIds57 = list;
		int[] spouseAnonymousTitles57 = new int[2] { 1099, 1100 };
		short[] initialAges57 = new short[4] { -1, -1, -1, -1 };
		PresetEquipmentItemWithProb[] equipment57 = new PresetEquipmentItemWithProb[8]
		{
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Accessory", -1, 0),
			new PresetEquipmentItemWithProb("Accessory", -1, 0),
			new PresetEquipmentItemWithProb("Accessory", -1, 0),
			new PresetEquipmentItemWithProb("Carrier", -1, 0)
		};
		PresetEquipmentItem clothing57 = new PresetEquipmentItem("Clothing", -1);
		list2 = new List<PresetInventoryItem>();
		List<PresetInventoryItem> inventory57 = list2;
		combatSkills2 = new List<PresetOrgMemberCombatSkill>();
		List<PresetOrgMemberCombatSkill> combatSkills22 = combatSkills2;
		sbyte[] extraCombatSkillGrids21 = new sbyte[5];
		short[] resourcesAdjust21 = new short[8];
		short[] lifeSkillsAdjust21 = new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		};
		short[] combatSkillsAdjust21 = new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		};
		short[] mainAttributesAdjust21 = new short[6] { -1, -1, -1, -1, -1, -1 };
		identityInteractConfig = new List<sbyte>();
		dataArray57.Add(new OrganizationMemberItem(236, 1092, 41, 7, 0, 0, 0, restrictPrincipalAmount: false, -1, -1, -1, 0, 0, -1, -1, 0, 0, monasticTitleSuffixes57, 0, 0, 0, 0, 0, favoriteClothingIds57, hatedClothingIds57, spouseAnonymousTitles57, canStroll: false, -1, initialAges57, equipment57, clothing57, inventory57, combatSkills22, extraCombatSkillGrids21, resourcesAdjust21, 0, 0, 0, 100, 42300, lifeSkillsAdjust21, 8, combatSkillsAdjust21, mainAttributesAdjust21, identityInteractConfig, 3, new ResourceInts(default(int), default(int), default(int), default(int), default(int), default(int), default(int), default(int)), new IntPair[0], null));
		List<OrganizationMemberItem> dataArray58 = _dataArray;
		int[] monasticTitleSuffixes58 = new int[2] { 1101, 1102 };
		list = new List<short>();
		List<short> favoriteClothingIds58 = list;
		list = new List<short>();
		List<short> hatedClothingIds58 = list;
		int[] spouseAnonymousTitles58 = new int[2] { 1103, 1104 };
		short[] initialAges58 = new short[4] { -1, -1, -1, -1 };
		PresetEquipmentItemWithProb[] equipment58 = new PresetEquipmentItemWithProb[8]
		{
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Accessory", -1, 0),
			new PresetEquipmentItemWithProb("Accessory", -1, 0),
			new PresetEquipmentItemWithProb("Accessory", -1, 0),
			new PresetEquipmentItemWithProb("Carrier", -1, 0)
		};
		PresetEquipmentItem clothing58 = new PresetEquipmentItem("Clothing", -1);
		list2 = new List<PresetInventoryItem>();
		List<PresetInventoryItem> inventory58 = list2;
		combatSkills2 = new List<PresetOrgMemberCombatSkill>();
		List<PresetOrgMemberCombatSkill> combatSkills23 = combatSkills2;
		sbyte[] extraCombatSkillGrids22 = new sbyte[5];
		short[] resourcesAdjust22 = new short[8];
		short[] lifeSkillsAdjust22 = new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		};
		short[] combatSkillsAdjust22 = new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		};
		short[] mainAttributesAdjust22 = new short[6] { -1, -1, -1, -1, -1, -1 };
		identityInteractConfig = new List<sbyte>();
		dataArray58.Add(new OrganizationMemberItem(237, 1092, 41, 6, 0, 0, 0, restrictPrincipalAmount: false, -1, -1, -1, 0, 0, -1, -1, 0, 0, monasticTitleSuffixes58, 0, 0, 0, 0, 0, favoriteClothingIds58, hatedClothingIds58, spouseAnonymousTitles58, canStroll: false, -1, initialAges58, equipment58, clothing58, inventory58, combatSkills23, extraCombatSkillGrids22, resourcesAdjust22, 0, 0, 0, 100, 27600, lifeSkillsAdjust22, 7, combatSkillsAdjust22, mainAttributesAdjust22, identityInteractConfig, 3, new ResourceInts(default(int), default(int), default(int), default(int), default(int), default(int), default(int), default(int)), new IntPair[0], null));
		List<OrganizationMemberItem> dataArray59 = _dataArray;
		int[] monasticTitleSuffixes59 = new int[2] { 1105, 1106 };
		list = new List<short>();
		List<short> favoriteClothingIds59 = list;
		list = new List<short>();
		List<short> hatedClothingIds59 = list;
		int[] spouseAnonymousTitles59 = new int[2] { 1107, 1108 };
		short[] initialAges59 = new short[4] { -1, -1, -1, -1 };
		PresetEquipmentItemWithProb[] equipment59 = new PresetEquipmentItemWithProb[8]
		{
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Accessory", -1, 0),
			new PresetEquipmentItemWithProb("Accessory", -1, 0),
			new PresetEquipmentItemWithProb("Accessory", -1, 0),
			new PresetEquipmentItemWithProb("Carrier", -1, 0)
		};
		PresetEquipmentItem clothing59 = new PresetEquipmentItem("Clothing", -1);
		list2 = new List<PresetInventoryItem>();
		List<PresetInventoryItem> inventory59 = list2;
		combatSkills2 = new List<PresetOrgMemberCombatSkill>();
		List<PresetOrgMemberCombatSkill> combatSkills24 = combatSkills2;
		sbyte[] extraCombatSkillGrids23 = new sbyte[5];
		short[] resourcesAdjust23 = new short[8];
		short[] lifeSkillsAdjust23 = new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		};
		short[] combatSkillsAdjust23 = new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		};
		short[] mainAttributesAdjust23 = new short[6] { -1, -1, -1, -1, -1, -1 };
		identityInteractConfig = new List<sbyte>();
		dataArray59.Add(new OrganizationMemberItem(238, 1092, 41, 5, 0, 0, 0, restrictPrincipalAmount: false, -1, -1, -1, 0, 0, -1, -1, 0, 0, monasticTitleSuffixes59, 0, 0, 0, 0, 0, favoriteClothingIds59, hatedClothingIds59, spouseAnonymousTitles59, canStroll: false, -1, initialAges59, equipment59, clothing59, inventory59, combatSkills24, extraCombatSkillGrids23, resourcesAdjust23, 0, 0, 0, 100, 16800, lifeSkillsAdjust23, 7, combatSkillsAdjust23, mainAttributesAdjust23, identityInteractConfig, 3, new ResourceInts(default(int), default(int), default(int), default(int), default(int), default(int), default(int), default(int)), new IntPair[0], null));
		List<OrganizationMemberItem> dataArray60 = _dataArray;
		int[] monasticTitleSuffixes60 = new int[2] { 1109, 1110 };
		list = new List<short>();
		List<short> favoriteClothingIds60 = list;
		list = new List<short>();
		List<short> hatedClothingIds60 = list;
		int[] spouseAnonymousTitles60 = new int[2] { 1111, 1112 };
		short[] initialAges60 = new short[4] { -1, -1, -1, -1 };
		PresetEquipmentItemWithProb[] equipment60 = new PresetEquipmentItemWithProb[8]
		{
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Accessory", -1, 0),
			new PresetEquipmentItemWithProb("Accessory", -1, 0),
			new PresetEquipmentItemWithProb("Accessory", -1, 0),
			new PresetEquipmentItemWithProb("Carrier", -1, 0)
		};
		PresetEquipmentItem clothing60 = new PresetEquipmentItem("Clothing", -1);
		list2 = new List<PresetInventoryItem>();
		List<PresetInventoryItem> inventory60 = list2;
		combatSkills2 = new List<PresetOrgMemberCombatSkill>();
		List<PresetOrgMemberCombatSkill> combatSkills25 = combatSkills2;
		sbyte[] extraCombatSkillGrids24 = new sbyte[5];
		short[] resourcesAdjust24 = new short[8];
		short[] lifeSkillsAdjust24 = new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		};
		short[] combatSkillsAdjust24 = new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		};
		short[] mainAttributesAdjust24 = new short[6] { -1, -1, -1, -1, -1, -1 };
		identityInteractConfig = new List<sbyte>();
		dataArray60.Add(new OrganizationMemberItem(239, 1092, 41, 4, 0, 0, 0, restrictPrincipalAmount: false, -1, -1, -1, 0, 0, -1, -1, 0, 0, monasticTitleSuffixes60, 0, 0, 0, 0, 0, favoriteClothingIds60, hatedClothingIds60, spouseAnonymousTitles60, canStroll: false, -1, initialAges60, equipment60, clothing60, inventory60, combatSkills25, extraCombatSkillGrids24, resourcesAdjust24, 0, 0, 0, 100, 9300, lifeSkillsAdjust24, 6, combatSkillsAdjust24, mainAttributesAdjust24, identityInteractConfig, 3, new ResourceInts(default(int), default(int), default(int), default(int), default(int), default(int), default(int), default(int)), new IntPair[0], null));
	}

	private void CreateItems4()
	{
		_dataArray.Add(new OrganizationMemberItem(240, 1092, 41, 3, 0, 0, 0, restrictPrincipalAmount: false, -1, -1, -1, 0, 0, -1, -1, 0, 0, new int[2] { 1113, 1114 }, 0, 0, 0, 0, 0, new List<short>(), new List<short>(), new int[2] { 1115, 1116 }, canStroll: false, -1, new short[4] { -1, -1, -1, -1 }, new PresetEquipmentItemWithProb[8]
		{
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Accessory", -1, 0),
			new PresetEquipmentItemWithProb("Accessory", -1, 0),
			new PresetEquipmentItemWithProb("Accessory", -1, 0),
			new PresetEquipmentItemWithProb("Carrier", -1, 0)
		}, new PresetEquipmentItem("Clothing", -1), new List<PresetInventoryItem>(), new List<PresetOrgMemberCombatSkill>(), new sbyte[5], new short[8], 0, 0, 0, 100, 4500, new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		}, 5, new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		}, new short[6] { -1, -1, -1, -1, -1, -1 }, new List<sbyte>(), 3, new ResourceInts(default(int), default(int), default(int), default(int), default(int), default(int), default(int), default(int)), new IntPair[0], null));
		_dataArray.Add(new OrganizationMemberItem(241, 1092, 41, 2, 0, 0, 0, restrictPrincipalAmount: false, -1, -1, -1, 0, 0, -1, -1, 0, 0, new int[2] { 1117, 1118 }, 0, 0, 0, 0, 0, new List<short>(), new List<short>(), new int[2] { 1119, 1120 }, canStroll: false, -1, new short[4] { -1, -1, -1, -1 }, new PresetEquipmentItemWithProb[8]
		{
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Accessory", -1, 0),
			new PresetEquipmentItemWithProb("Accessory", -1, 0),
			new PresetEquipmentItemWithProb("Accessory", -1, 0),
			new PresetEquipmentItemWithProb("Carrier", -1, 0)
		}, new PresetEquipmentItem("Clothing", -1), new List<PresetInventoryItem>(), new List<PresetOrgMemberCombatSkill>(), new sbyte[5], new short[8], 0, 0, 0, 100, 1800, new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		}, 4, new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		}, new short[6] { -1, -1, -1, -1, -1, -1 }, new List<sbyte>(), 3, new ResourceInts(default(int), default(int), default(int), default(int), default(int), default(int), default(int), default(int)), new IntPair[0], null));
		_dataArray.Add(new OrganizationMemberItem(242, 1092, 41, 1, 0, 0, 0, restrictPrincipalAmount: false, -1, -1, -1, 0, 0, -1, -1, 0, 0, new int[2] { 1121, 1122 }, 0, 0, 0, 0, 0, new List<short>(), new List<short>(), new int[2] { 1123, 1124 }, canStroll: false, -1, new short[4] { -1, -1, -1, -1 }, new PresetEquipmentItemWithProb[8]
		{
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Accessory", -1, 0),
			new PresetEquipmentItemWithProb("Accessory", -1, 0),
			new PresetEquipmentItemWithProb("Accessory", -1, 0),
			new PresetEquipmentItemWithProb("Carrier", -1, 0)
		}, new PresetEquipmentItem("Clothing", -1), new List<PresetInventoryItem>(), new List<PresetOrgMemberCombatSkill>(), new sbyte[5], new short[8], 0, 0, 0, 100, 600, new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		}, 3, new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		}, new short[6] { -1, -1, -1, -1, -1, -1 }, new List<sbyte>(), 3, new ResourceInts(default(int), default(int), default(int), default(int), default(int), default(int), default(int), default(int)), new IntPair[0], null));
		_dataArray.Add(new OrganizationMemberItem(243, 1092, 41, 0, 0, 0, 0, restrictPrincipalAmount: false, -1, -1, -1, 0, 0, -1, -1, 0, 0, new int[2] { 1125, 1126 }, 0, 0, 0, 0, 0, new List<short>(), new List<short>(), new int[2] { 1127, 1128 }, canStroll: false, -1, new short[4] { -1, -1, -1, -1 }, new PresetEquipmentItemWithProb[8]
		{
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Armor", -1, 0),
			new PresetEquipmentItemWithProb("Accessory", -1, 0),
			new PresetEquipmentItemWithProb("Accessory", -1, 0),
			new PresetEquipmentItemWithProb("Accessory", -1, 0),
			new PresetEquipmentItemWithProb("Carrier", -1, 0)
		}, new PresetEquipmentItem("Clothing", -1), new List<PresetInventoryItem>(), new List<PresetOrgMemberCombatSkill>(), new sbyte[5], new short[8], 0, 0, 0, 100, 300, new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		}, 2, new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		}, new short[6] { -1, -1, -1, -1, -1, -1 }, new List<sbyte>(), 3, new ResourceInts(default(int), default(int), default(int), default(int), default(int), default(int), default(int), default(int)), new IntPair[0], null));
	}

	public override void Init()
	{
		base.Init();
		_dataArray = new List<OrganizationMemberItem>(244);
		CreateItems0();
		CreateItems1();
		CreateItems2();
		CreateItems3();
		CreateItems4();
	}
}
