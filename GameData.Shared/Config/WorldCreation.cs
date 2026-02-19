using System;
using System.Collections.Generic;
using Config.Common;

namespace Config;

[Serializable]
public class WorldCreation : ConfigData<WorldCreationItem, byte>
{
	public static class DefKey
	{
		public const byte CharacterLifeSpan = 0;

		public const byte CombatDifficulty = 1;

		public const byte ReadingDifficulty = 8;

		public const byte BreakoutDifficulty = 9;

		public const byte NeigongLoopingDifficulty = 10;

		public const byte HereticsAmount = 2;

		public const byte BossInvasionSpeed = 3;

		public const byte WorldResourceAmount = 4;

		public const byte WorldPopulation = 5;

		public const byte RestrictOptionsBehavior = 6;

		public const byte AllowRandomTaiwuHeir = 7;

		public const byte EnemyPracticeLevel = 11;

		public const byte FavorabilityChange = 12;

		public const byte ProfessionUpgrade = 13;

		public const byte LootYield = 14;
	}

	public static class DefValue
	{
		public static WorldCreationItem CharacterLifeSpan => Instance[(byte)0];

		public static WorldCreationItem CombatDifficulty => Instance[(byte)1];

		public static WorldCreationItem ReadingDifficulty => Instance[(byte)8];

		public static WorldCreationItem BreakoutDifficulty => Instance[(byte)9];

		public static WorldCreationItem NeigongLoopingDifficulty => Instance[(byte)10];

		public static WorldCreationItem HereticsAmount => Instance[(byte)2];

		public static WorldCreationItem BossInvasionSpeed => Instance[(byte)3];

		public static WorldCreationItem WorldResourceAmount => Instance[(byte)4];

		public static WorldCreationItem WorldPopulation => Instance[(byte)5];

		public static WorldCreationItem RestrictOptionsBehavior => Instance[(byte)6];

		public static WorldCreationItem AllowRandomTaiwuHeir => Instance[(byte)7];

		public static WorldCreationItem EnemyPracticeLevel => Instance[(byte)11];

		public static WorldCreationItem FavorabilityChange => Instance[(byte)12];

		public static WorldCreationItem ProfessionUpgrade => Instance[(byte)13];

		public static WorldCreationItem LootYield => Instance[(byte)14];
	}

	public static WorldCreation Instance = new WorldCreation();

	private readonly HashSet<string> RequiredFields = new HashSet<string> { "TemplateId", "Name", "Desc", "Icons", "Options", "ShowInLegacy", "DifficultyPreset", "SaveFileKey" };

	internal override int ToInt(byte value)
	{
		return value;
	}

	internal override byte ToTemplateId(int value)
	{
		return (byte)value;
	}

	private void CreateItems0()
	{
		_dataArray.Add(new WorldCreationItem(0, 0, 1, new string[4] { "newgame_shijie_icon_0_0", "newgame_shijie_icon_0_0", "newgame_shijie_icon_0_0", "newgame_shijie_icon_0_0" }, new int[4] { 2, 3, 4, 5 }, new short[4] { 50, 75, 100, 125 }, new short[0], new short[0], showInLegacy: false, new sbyte[4] { 1, 1, 1, 1 }, "CharacterLifespanType"));
		_dataArray.Add(new WorldCreationItem(1, 6, 7, new string[4] { "newgame_shijie_icon_1_0", "newgame_shijie_icon_1_1", "newgame_shijie_icon_1_2", "newgame_shijie_icon_1_3" }, new int[4] { 8, 9, 10, 11 }, new short[0], new short[4] { 0, 10, 20, 40 }, new short[0], showInLegacy: true, new sbyte[4] { 0, 1, 2, 3 }, "CombatDifficulty"));
		_dataArray.Add(new WorldCreationItem(2, 27, 28, new string[4] { "newgame_shijie_icon_2_0", "newgame_shijie_icon_2_1", "newgame_shijie_icon_2_2", "newgame_shijie_icon_2_3" }, new int[4] { 29, 15, 30, 31 }, new short[4] { 50, 75, 100, 200 }, new short[4] { 0, 5, 10, 20 }, new short[0], showInLegacy: true, new sbyte[4] { 0, 1, 2, 3 }, "HereticsAmountType"));
		_dataArray.Add(new WorldCreationItem(3, 32, 33, new string[4] { "newgame_shijie_icon_3_0", "newgame_shijie_icon_3_1", "newgame_shijie_icon_3_2", "newgame_shijie_icon_3_3" }, new int[4] { 34, 15, 35, 36 }, new short[4] { 120, 36, 24, 12 }, new short[4] { 0, 10, 20, 40 }, new short[0], showInLegacy: true, new sbyte[4] { 0, 1, 2, 3 }, "BossInvasionSpeedType"));
		_dataArray.Add(new WorldCreationItem(4, 37, 38, new string[4] { "newgame_shijie_icon_4_0", "newgame_shijie_icon_4_1", "newgame_shijie_icon_4_2", "newgame_shijie_icon_4_3" }, new int[4] { 39, 15, 40, 41 }, new short[4] { 4, 3, 2, 1 }, new short[4] { 0, 10, 20, 40 }, new short[4] { 150, 100, 50, 25 }, showInLegacy: true, new sbyte[4] { 0, 1, 2, 3 }, "WorldResourceAmountType"));
		_dataArray.Add(new WorldCreationItem(5, 42, 43, new string[4] { "newgame_shijie_icon_5_0", "newgame_shijie_icon_5_0", "newgame_shijie_icon_5_0", "newgame_shijie_icon_5_0" }, new int[4] { 44, 45, 46, 47 }, new short[4] { 100, 125, 150, 175 }, new short[0], new short[0], showInLegacy: false, new sbyte[4] { 1, 1, 1, 1 }, "WorldPopulation"));
		_dataArray.Add(new WorldCreationItem(6, 48, 49, new string[2] { "newgame_shijie_icon_9_0", "newgame_shijie_icon_9_0" }, new int[2] { 50, 51 }, new short[0], new short[0], new short[0], showInLegacy: true, new sbyte[4] { 1, 1, 1, 1 }, "RestrictOptionsBehaviorType"));
		_dataArray.Add(new WorldCreationItem(7, 52, 53, new string[2] { "newgame_shijie_icon_10_0", "newgame_shijie_icon_10_0" }, new int[2] { 50, 51 }, new short[0], new short[0], new short[0], showInLegacy: true, new sbyte[4] { 1, 1, 1, 1 }, "AllowRandomTaiwuHeir"));
		_dataArray.Add(new WorldCreationItem(8, 12, 13, new string[4] { "newgame_shijie_icon_6_0", "newgame_shijie_icon_6_1", "newgame_shijie_icon_6_2", "newgame_shijie_icon_6_3" }, new int[4] { 14, 15, 16, 17 }, new short[4] { 200, 100, 75, 50 }, new short[4] { 0, 5, 10, 20 }, new short[0], showInLegacy: true, new sbyte[4] { 0, 1, 2, 3 }, "ReadingDifficulty"));
		_dataArray.Add(new WorldCreationItem(9, 18, 19, new string[4] { "newgame_shijie_icon_7_0", "newgame_shijie_icon_7_1", "newgame_shijie_icon_7_2", "newgame_shijie_icon_7_3" }, new int[4] { 20, 15, 21, 22 }, new short[4] { 20, 0, -10, -20 }, new short[4] { 0, 10, 20, 40 }, new short[0], showInLegacy: true, new sbyte[4] { 0, 1, 2, 3 }, "BreakoutDifficulty"));
		_dataArray.Add(new WorldCreationItem(10, 23, 24, new string[4] { "newgame_shijie_icon_8_0", "newgame_shijie_icon_8_1", "newgame_shijie_icon_8_2", "newgame_shijie_icon_8_3" }, new int[4] { 25, 15, 16, 26 }, new short[4] { 200, 100, 75, 50 }, new short[4] { 0, 5, 10, 20 }, new short[0], showInLegacy: true, new sbyte[4] { 0, 1, 2, 3 }, "NeigongLoopingDifficulty"));
		_dataArray.Add(new WorldCreationItem(11, 54, 55, new string[4] { "newgame_shijie_icon_12_0", "newgame_shijie_icon_12_1", "newgame_shijie_icon_12_2", "newgame_shijie_icon_12_3" }, new int[4] { 8, 56, 57, 58 }, new short[4] { 0, 1, 3, 5 }, new short[4] { 0, 5, 10, 20 }, new short[4] { 0, 20, 35, 50 }, showInLegacy: true, new sbyte[4] { 0, 1, 2, 3 }, "EnemyPracticeLevel"));
		_dataArray.Add(new WorldCreationItem(12, 59, 60, new string[4] { "newgame_shijie_icon_11_0", "newgame_shijie_icon_11_1", "newgame_shijie_icon_11_2", "newgame_shijie_icon_11_3" }, new int[4] { 20, 15, 21, 61 }, new short[4] { 100, 75, 50, 25 }, new short[4] { 0, 10, 20, 40 }, new short[0], showInLegacy: true, new sbyte[4] { 0, 1, 2, 3 }, "FavorabilityChange"));
		_dataArray.Add(new WorldCreationItem(13, 62, 63, new string[4] { "newgame_shijie_icon_13_0", "newgame_shijie_icon_13_1", "newgame_shijie_icon_13_2", "newgame_shijie_icon_13_3" }, new int[4] { 25, 15, 16, 64 }, new short[4] { 200, 100, 75, 50 }, new short[4] { 0, 10, 20, 40 }, new short[0], showInLegacy: true, new sbyte[4] { 0, 1, 2, 3 }, "ProfessionUpgrade"));
		_dataArray.Add(new WorldCreationItem(14, 65, 66, new string[4] { "newgame_shijie_icon_14_0", "newgame_shijie_icon_14_1", "newgame_shijie_icon_14_2", "newgame_shijie_icon_14_3" }, new int[4] { 67, 68, 69, 70 }, new short[4] { 200, 100, 50, 25 }, new short[4] { 0, 10, 20, 40 }, new short[0], showInLegacy: true, new sbyte[4] { 0, 1, 2, 3 }, "LootYield"));
	}

	public override void Init()
	{
		base.Init();
		_dataArray = new List<WorldCreationItem>(15);
		CreateItems0();
	}
}
