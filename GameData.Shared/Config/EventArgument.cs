using System;
using System.Collections.Generic;
using Config.Common;
using GameData.Utilities;

namespace Config;

[Serializable]
public class EventArgument : ConfigData<EventArgumentItem, int>
{
	public static class DefKey
	{
		public const int Dynamic = 0;

		public const int Integer = 1;

		public const int Float = 2;

		public const int Bool = 3;

		public const int String = 4;

		public const int Event = 5;

		public const int GlobalScript = 48;

		public const int Character = 6;

		public const int Item = 7;

		public const int ItemTemplate = 8;

		public const int MapBlock = 9;

		public const int Settlement = 10;

		public const int SettlementType = 59;

		public const int Sect = 11;

		public const int MapArea = 12;

		public const int MapState = 47;

		public const int MapBlockMatcher = 58;

		public const int Grade = 13;

		public const int BodyPartType = 14;

		public const int PoisonType = 15;

		public const int WugType = 16;

		public const int BehaviorType = 17;

		public const int PersonalityType = 18;

		public const int MainAttributeType = 19;

		public const int FavorabilityType = 53;

		public const int Gender = 20;

		public const int ResourceType = 21;

		public const int ItemType = 22;

		public const int ItemSubType = 23;

		public const int CombatSkillType = 24;

		public const int LifeSkillType = 25;

		public const int LifeSkill = 51;

		public const int EquipmentSlot = 26;

		public const int CombatSkillEquipType = 27;

		public const int FiveElementsType = 28;

		public const int CharacterFeature = 29;

		public const int LegacyPoint = 30;

		public const int CharacterDeathType = 31;

		public const int CharacterMatcher = 60;

		public const int MainStoryLineProgress = 32;

		public const int WorldFunctionType = 33;

		public const int CricketPartsTemplate = 34;

		public const int CombatSkillTemplate = 35;

		public const int LifeSkillTemplate = 36;

		public const int MerchantType = 37;

		public const int TaskInfo = 38;

		public const int TaskChain = 39;

		public const int AdventureTemplate = 40;

		public const int ConditionOperator = 41;

		public const int CombatConfig = 42;

		public const int CombatResultType = 43;

		public const int EventActorTemplate = 44;

		public const int EnemyCharacterTemplate = 45;

		public const int FixedCharacterTemplate = 46;

		public const int CharacterSearchRange = 49;

		public const int CharacterFilterRules = 50;

		public const int Profession = 52;

		public const int MonthlyActions = 54;

		public const int BuildingBlockTemplate = 55;

		public const int MerchantTemplate = 56;

		public const int AgeGroup = 57;

		public const int WorldFavorability = 63;

		public const int AdventureRemakeElementCoreId = 61;

		public const int AdventureRemakeElementTag = 62;

		public const int EnemyNestTemplate = 64;
	}

	public static class DefValue
	{
		public static EventArgumentItem Dynamic => Instance[0];

		public static EventArgumentItem Integer => Instance[1];

		public static EventArgumentItem Float => Instance[2];

		public static EventArgumentItem Bool => Instance[3];

		public static EventArgumentItem String => Instance[4];

		public static EventArgumentItem Event => Instance[5];

		public static EventArgumentItem GlobalScript => Instance[48];

		public static EventArgumentItem Character => Instance[6];

		public static EventArgumentItem Item => Instance[7];

		public static EventArgumentItem ItemTemplate => Instance[8];

		public static EventArgumentItem MapBlock => Instance[9];

		public static EventArgumentItem Settlement => Instance[10];

		public static EventArgumentItem SettlementType => Instance[59];

		public static EventArgumentItem Sect => Instance[11];

		public static EventArgumentItem MapArea => Instance[12];

		public static EventArgumentItem MapState => Instance[47];

		public static EventArgumentItem MapBlockMatcher => Instance[58];

		public static EventArgumentItem Grade => Instance[13];

		public static EventArgumentItem BodyPartType => Instance[14];

		public static EventArgumentItem PoisonType => Instance[15];

		public static EventArgumentItem WugType => Instance[16];

		public static EventArgumentItem BehaviorType => Instance[17];

		public static EventArgumentItem PersonalityType => Instance[18];

		public static EventArgumentItem MainAttributeType => Instance[19];

		public static EventArgumentItem FavorabilityType => Instance[53];

		public static EventArgumentItem Gender => Instance[20];

		public static EventArgumentItem ResourceType => Instance[21];

		public static EventArgumentItem ItemType => Instance[22];

		public static EventArgumentItem ItemSubType => Instance[23];

		public static EventArgumentItem CombatSkillType => Instance[24];

		public static EventArgumentItem LifeSkillType => Instance[25];

		public static EventArgumentItem LifeSkill => Instance[51];

		public static EventArgumentItem EquipmentSlot => Instance[26];

		public static EventArgumentItem CombatSkillEquipType => Instance[27];

		public static EventArgumentItem FiveElementsType => Instance[28];

		public static EventArgumentItem CharacterFeature => Instance[29];

		public static EventArgumentItem LegacyPoint => Instance[30];

		public static EventArgumentItem CharacterDeathType => Instance[31];

		public static EventArgumentItem CharacterMatcher => Instance[60];

		public static EventArgumentItem MainStoryLineProgress => Instance[32];

		public static EventArgumentItem WorldFunctionType => Instance[33];

		public static EventArgumentItem CricketPartsTemplate => Instance[34];

		public static EventArgumentItem CombatSkillTemplate => Instance[35];

		public static EventArgumentItem LifeSkillTemplate => Instance[36];

		public static EventArgumentItem MerchantType => Instance[37];

		public static EventArgumentItem TaskInfo => Instance[38];

		public static EventArgumentItem TaskChain => Instance[39];

		public static EventArgumentItem AdventureTemplate => Instance[40];

		public static EventArgumentItem ConditionOperator => Instance[41];

		public static EventArgumentItem CombatConfig => Instance[42];

		public static EventArgumentItem CombatResultType => Instance[43];

		public static EventArgumentItem EventActorTemplate => Instance[44];

		public static EventArgumentItem EnemyCharacterTemplate => Instance[45];

		public static EventArgumentItem FixedCharacterTemplate => Instance[46];

		public static EventArgumentItem CharacterSearchRange => Instance[49];

		public static EventArgumentItem CharacterFilterRules => Instance[50];

		public static EventArgumentItem Profession => Instance[52];

		public static EventArgumentItem MonthlyActions => Instance[54];

		public static EventArgumentItem BuildingBlockTemplate => Instance[55];

		public static EventArgumentItem MerchantTemplate => Instance[56];

		public static EventArgumentItem AgeGroup => Instance[57];

		public static EventArgumentItem WorldFavorability => Instance[63];

		public static EventArgumentItem AdventureRemakeElementCoreId => Instance[61];

		public static EventArgumentItem AdventureRemakeElementTag => Instance[62];

		public static EventArgumentItem EnemyNestTemplate => Instance[64];
	}

	public static EventArgument Instance = new EventArgument();

	private readonly HashSet<string> RequiredFields = new HashSet<string> { "TemplateId", "Name", "Desc", "DefaultValue", "ConfigTable" };

	internal override int ToInt(int value)
	{
		return value;
	}

	internal override int ToTemplateId(int value)
	{
		return value;
	}

	private void CreateItems0()
	{
		_dataArray.Add(new EventArgumentItem(0, EEventArgumentType.Basic, 0, 1, "0", isExpression: true, allowSwitchingExpression: true, null, new int[0], new int[0], new IntPair(0, 0)));
		_dataArray.Add(new EventArgumentItem(1, EEventArgumentType.Basic, 2, 3, "0", isExpression: true, allowSwitchingExpression: true, null, new int[0], new int[0], new IntPair(0, 0)));
		_dataArray.Add(new EventArgumentItem(2, EEventArgumentType.Basic, 4, 5, "0", isExpression: true, allowSwitchingExpression: true, null, new int[0], new int[0], new IntPair(0, 0)));
		_dataArray.Add(new EventArgumentItem(3, EEventArgumentType.Basic, 6, 7, "0", isExpression: false, allowSwitchingExpression: true, null, new int[0], new int[0], new IntPair(0, 0)));
		_dataArray.Add(new EventArgumentItem(4, EEventArgumentType.Basic, 8, 9, null, isExpression: false, allowSwitchingExpression: true, null, new int[0], new int[0], new IntPair(0, 0)));
		_dataArray.Add(new EventArgumentItem(5, EEventArgumentType.Object, 10, 11, null, isExpression: false, allowSwitchingExpression: true, null, new int[0], new int[0], new IntPair(0, 0)));
		_dataArray.Add(new EventArgumentItem(6, EEventArgumentType.Object, 14, 15, null, isExpression: false, allowSwitchingExpression: true, null, new int[0], new int[0], new IntPair(0, 0)));
		_dataArray.Add(new EventArgumentItem(7, EEventArgumentType.Object, 16, 17, null, isExpression: false, allowSwitchingExpression: true, null, new int[0], new int[0], new IntPair(0, 0)));
		_dataArray.Add(new EventArgumentItem(8, EEventArgumentType.Object, 18, 19, null, isExpression: false, allowSwitchingExpression: true, null, new int[0], new int[0], new IntPair(0, 0)));
		_dataArray.Add(new EventArgumentItem(9, EEventArgumentType.Object, 20, 21, null, isExpression: true, allowSwitchingExpression: false, null, new int[0], new int[0], new IntPair(0, 0)));
		_dataArray.Add(new EventArgumentItem(10, EEventArgumentType.Object, 22, 23, null, isExpression: false, allowSwitchingExpression: true, null, new int[0], new int[0], new IntPair(0, 0)));
		_dataArray.Add(new EventArgumentItem(11, EEventArgumentType.Enum, 31, 33, null, isExpression: false, allowSwitchingExpression: true, "Organization", new int[0], new int[0], new IntPair(1, 15)));
		_dataArray.Add(new EventArgumentItem(12, EEventArgumentType.Enum, 34, 35, null, isExpression: false, allowSwitchingExpression: true, "MapArea", new int[0], new int[0], new IntPair(0, 0)));
		_dataArray.Add(new EventArgumentItem(13, EEventArgumentType.Enum, 40, 41, null, isExpression: false, allowSwitchingExpression: true, null, new int[0], new int[0], new IntPair(0, 8)));
		_dataArray.Add(new EventArgumentItem(14, EEventArgumentType.Enum, 42, 43, null, isExpression: false, allowSwitchingExpression: true, "BodyPart", new int[0], new int[0], new IntPair(0, 0)));
		_dataArray.Add(new EventArgumentItem(15, EEventArgumentType.Enum, 44, 45, null, isExpression: false, allowSwitchingExpression: true, "Poison", new int[0], new int[0], new IntPair(0, 0)));
		_dataArray.Add(new EventArgumentItem(16, EEventArgumentType.Enum, 46, 47, null, isExpression: false, allowSwitchingExpression: true, null, new int[8] { 48, 49, 50, 51, 52, 53, 54, 55 }, new int[0], new IntPair(0, 7)));
		_dataArray.Add(new EventArgumentItem(17, EEventArgumentType.Enum, 56, 57, null, isExpression: false, allowSwitchingExpression: true, "BehaviorType", new int[0], new int[0], new IntPair(0, 0)));
		_dataArray.Add(new EventArgumentItem(18, EEventArgumentType.Enum, 58, 59, null, isExpression: false, allowSwitchingExpression: true, null, new int[7] { 60, 61, 62, 63, 64, 65, 66 }, new int[0], new IntPair(0, 0)));
		_dataArray.Add(new EventArgumentItem(19, EEventArgumentType.Enum, 67, 68, null, isExpression: false, allowSwitchingExpression: true, null, new int[6] { 69, 70, 71, 72, 73, 74 }, new int[0], new IntPair(0, 0)));
		_dataArray.Add(new EventArgumentItem(20, EEventArgumentType.Enum, 90, 91, null, isExpression: false, allowSwitchingExpression: true, null, new int[2] { 92, 93 }, new int[0], new IntPair(0, 0)));
		_dataArray.Add(new EventArgumentItem(21, EEventArgumentType.Enum, 94, 95, null, isExpression: false, allowSwitchingExpression: true, "ResourceType", new int[0], new int[0], new IntPair(0, 0)));
		_dataArray.Add(new EventArgumentItem(22, EEventArgumentType.Enum, 96, 97, null, isExpression: false, allowSwitchingExpression: true, null, new int[0], new int[0], new IntPair(0, 0)));
		_dataArray.Add(new EventArgumentItem(23, EEventArgumentType.Enum, 98, 99, null, isExpression: false, allowSwitchingExpression: true, null, new int[0], new int[0], new IntPair(0, 0)));
		_dataArray.Add(new EventArgumentItem(24, EEventArgumentType.Enum, 100, 101, null, isExpression: false, allowSwitchingExpression: true, "CombatSkillType", new int[0], new int[0], new IntPair(0, 0)));
		_dataArray.Add(new EventArgumentItem(25, EEventArgumentType.Enum, 102, 103, null, isExpression: false, allowSwitchingExpression: true, "LifeSkillType", new int[0], new int[0], new IntPair(0, 0)));
		_dataArray.Add(new EventArgumentItem(26, EEventArgumentType.Enum, 106, 107, null, isExpression: false, allowSwitchingExpression: true, null, new int[0], new int[0], new IntPair(0, 0)));
		_dataArray.Add(new EventArgumentItem(27, EEventArgumentType.Enum, 108, 109, null, isExpression: false, allowSwitchingExpression: true, null, new int[0], new int[0], new IntPair(0, 0)));
		_dataArray.Add(new EventArgumentItem(28, EEventArgumentType.Enum, 110, 111, null, isExpression: false, allowSwitchingExpression: true, null, new int[0], new int[0], new IntPair(0, 0)));
		_dataArray.Add(new EventArgumentItem(29, EEventArgumentType.Enum, 112, 113, null, isExpression: false, allowSwitchingExpression: true, "CharacterFeature", new int[0], new int[0], new IntPair(0, 0)));
		_dataArray.Add(new EventArgumentItem(30, EEventArgumentType.Enum, 114, 115, null, isExpression: false, allowSwitchingExpression: true, "LegacyPoint", new int[0], new int[0], new IntPair(0, 0)));
		_dataArray.Add(new EventArgumentItem(31, EEventArgumentType.Enum, 116, 117, null, isExpression: false, allowSwitchingExpression: true, "CharacterDeathType", new int[0], new int[0], new IntPair(0, 0)));
		_dataArray.Add(new EventArgumentItem(32, EEventArgumentType.Enum, 120, 121, null, isExpression: false, allowSwitchingExpression: true, "MainStoryLineProgress", new int[0], new int[0], new IntPair(0, 0)));
		_dataArray.Add(new EventArgumentItem(33, EEventArgumentType.Enum, 122, 123, null, isExpression: false, allowSwitchingExpression: true, null, new int[0], new int[0], new IntPair(0, 0)));
		_dataArray.Add(new EventArgumentItem(34, EEventArgumentType.Enum, 124, 125, null, isExpression: false, allowSwitchingExpression: true, "CricketParts", new int[0], new int[0], new IntPair(0, 0)));
		_dataArray.Add(new EventArgumentItem(35, EEventArgumentType.Enum, 126, 127, null, isExpression: false, allowSwitchingExpression: true, "CombatSkill", new int[0], new int[0], new IntPair(0, 0)));
		_dataArray.Add(new EventArgumentItem(36, EEventArgumentType.Enum, 128, 129, null, isExpression: false, allowSwitchingExpression: true, "LifeSkillType", new int[0], new int[0], new IntPair(0, 0)));
		_dataArray.Add(new EventArgumentItem(37, EEventArgumentType.Enum, 130, 131, null, isExpression: false, allowSwitchingExpression: true, "MerchantType", new int[0], new int[0], new IntPair(0, 0)));
		_dataArray.Add(new EventArgumentItem(38, EEventArgumentType.Enum, 132, 133, null, isExpression: false, allowSwitchingExpression: true, "TaskInfo", new int[0], new int[0], new IntPair(0, 0)));
		_dataArray.Add(new EventArgumentItem(39, EEventArgumentType.Enum, 134, 135, null, isExpression: false, allowSwitchingExpression: true, "TaskChain", new int[0], new int[0], new IntPair(0, 0)));
		_dataArray.Add(new EventArgumentItem(40, EEventArgumentType.Enum, 136, 137, null, isExpression: false, allowSwitchingExpression: true, "Adventure", new int[0], new int[0], new IntPair(0, 0)));
		_dataArray.Add(new EventArgumentItem(41, EEventArgumentType.Enum, 138, 139, null, isExpression: false, allowSwitchingExpression: false, "EventConditionOperator", new int[0], new int[0], new IntPair(0, 0)));
		_dataArray.Add(new EventArgumentItem(42, EEventArgumentType.Enum, 140, 141, null, isExpression: false, allowSwitchingExpression: true, "CombatConfig", new int[0], new int[0], new IntPair(0, 0)));
		_dataArray.Add(new EventArgumentItem(43, EEventArgumentType.Enum, 142, 143, null, isExpression: false, allowSwitchingExpression: true, null, new int[6] { 144, 145, 146, 147, 148, 149 }, new int[0], new IntPair(0, 0)));
		_dataArray.Add(new EventArgumentItem(44, EEventArgumentType.Enum, 150, 151, null, isExpression: false, allowSwitchingExpression: true, "EventActors", new int[0], new int[0], new IntPair(0, 0)));
		_dataArray.Add(new EventArgumentItem(45, EEventArgumentType.Enum, 152, 153, null, isExpression: false, allowSwitchingExpression: true, "Character", new int[0], new int[0], new IntPair(0, 0)));
		_dataArray.Add(new EventArgumentItem(46, EEventArgumentType.Enum, 154, 155, null, isExpression: false, allowSwitchingExpression: true, "Character", new int[0], new int[0], new IntPair(0, 0)));
		_dataArray.Add(new EventArgumentItem(47, EEventArgumentType.Enum, 36, 37, null, isExpression: false, allowSwitchingExpression: true, "MapState", new int[0], new int[0], new IntPair(0, 0)));
		_dataArray.Add(new EventArgumentItem(48, EEventArgumentType.Object, 12, 13, null, isExpression: false, allowSwitchingExpression: true, null, new int[0], new int[0], new IntPair(0, 0)));
		_dataArray.Add(new EventArgumentItem(49, EEventArgumentType.Enum, 156, 157, null, isExpression: false, allowSwitchingExpression: true, null, new int[3] { 158, 159, 160 }, new int[0], new IntPair(0, 0)));
		_dataArray.Add(new EventArgumentItem(50, EEventArgumentType.Enum, 161, 162, null, isExpression: false, allowSwitchingExpression: true, "CharacterFilterRules", new int[0], new int[0], new IntPair(0, 0)));
		_dataArray.Add(new EventArgumentItem(51, EEventArgumentType.Enum, 104, 105, null, isExpression: false, allowSwitchingExpression: true, "LifeSkill", new int[0], new int[0], new IntPair(0, 0)));
		_dataArray.Add(new EventArgumentItem(52, EEventArgumentType.Enum, 163, 164, null, isExpression: false, allowSwitchingExpression: true, "Profession", new int[0], new int[0], new IntPair(0, 0)));
		_dataArray.Add(new EventArgumentItem(53, EEventArgumentType.Enum, 75, 76, null, isExpression: false, allowSwitchingExpression: true, null, new int[13]
		{
			77, 78, 79, 80, 81, 82, 83, 84, 85, 86,
			87, 88, 89
		}, new int[13]
		{
			-6, -5, -4, -3, -2, -1, 0, 1, 2, 3,
			4, 5, 6
		}, new IntPair(0, 0)));
		_dataArray.Add(new EventArgumentItem(54, EEventArgumentType.Enum, 165, 166, null, isExpression: false, allowSwitchingExpression: false, "MonthlyActions", new int[0], new int[0], new IntPair(0, 0)));
		_dataArray.Add(new EventArgumentItem(55, EEventArgumentType.Enum, 167, 168, null, isExpression: false, allowSwitchingExpression: true, "BuildingBlock", new int[0], new int[0], new IntPair(0, 0)));
		_dataArray.Add(new EventArgumentItem(56, EEventArgumentType.Enum, 169, 170, null, isExpression: false, allowSwitchingExpression: true, "Merchant", new int[0], new int[0], new IntPair(0, 0)));
		_dataArray.Add(new EventArgumentItem(57, EEventArgumentType.Enum, 171, 172, null, isExpression: false, allowSwitchingExpression: true, null, new int[3] { 173, 174, 175 }, new int[0], new IntPair(0, 0)));
		_dataArray.Add(new EventArgumentItem(58, EEventArgumentType.Enum, 38, 39, null, isExpression: false, allowSwitchingExpression: true, "MapBlockMatcher", new int[0], new int[0], new IntPair(0, 0)));
		_dataArray.Add(new EventArgumentItem(59, EEventArgumentType.Enum, 24, 25, null, isExpression: false, allowSwitchingExpression: true, null, new int[7] { 26, 27, 28, 29, 30, 31, 32 }, new int[7] { -1, 0, 1, 2, 3, 4, 5 }, new IntPair(0, 0)));
	}

	private void CreateItems1()
	{
		_dataArray.Add(new EventArgumentItem(60, EEventArgumentType.Enum, 118, 119, null, isExpression: false, allowSwitchingExpression: true, "CharacterMatcher", new int[0], new int[0], new IntPair(0, 0)));
		_dataArray.Add(new EventArgumentItem(61, EEventArgumentType.Enum, 178, 179, null, isExpression: false, allowSwitchingExpression: true, null, new int[0], new int[0], new IntPair(0, 0)));
		_dataArray.Add(new EventArgumentItem(62, EEventArgumentType.Enum, 180, 181, null, isExpression: false, allowSwitchingExpression: true, null, new int[0], new int[0], new IntPair(0, 0)));
		_dataArray.Add(new EventArgumentItem(63, EEventArgumentType.Enum, 176, 177, null, isExpression: false, allowSwitchingExpression: true, "WorldFavorability", new int[0], new int[0], new IntPair(0, 0)));
		_dataArray.Add(new EventArgumentItem(64, EEventArgumentType.Enum, 182, 183, null, isExpression: false, allowSwitchingExpression: true, "EnemyNest", new int[0], new int[0], new IntPair(0, 0)));
	}

	public override void Init()
	{
		base.Init();
		_dataArray = new List<EventArgumentItem>(65);
		CreateItems0();
		CreateItems1();
	}
}
