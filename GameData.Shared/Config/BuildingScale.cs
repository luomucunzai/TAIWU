using System;
using System.Collections.Generic;
using Config.Common;

namespace Config;

[Serializable]
public class BuildingScale : ConfigData<BuildingScaleItem, short>
{
	public static class DefKey
	{
		public const short BuildingSpace = 107;

		public const short StoneRoomCapacity = 108;

		public const short JiaoPoolCount = 109;

		public const short TroughLoad = 110;

		public const short TaiwuShrineEffect = 111;

		public const short WareHouseLoad = 112;

		public const short ResidenceCapacity = 113;

		public const short ComfortableHouseCapacity = 114;

		public const short TeaHorseCaravanSlot = 116;

		public const short PhoenixPlatformPracticalEffect = 225;

		public const short StrategyRoomEffect = 226;

		public const short MakeupRoomPercentEffect = 227;

		public const short BirthDeathStreamerEffect = 228;

		public const short LifeElixirRoomEffect = 229;

		public const short SutraReadingRoomEffect = 230;
	}

	public static class DefValue
	{
		public static BuildingScaleItem BuildingSpace => Instance[(short)107];

		public static BuildingScaleItem StoneRoomCapacity => Instance[(short)108];

		public static BuildingScaleItem JiaoPoolCount => Instance[(short)109];

		public static BuildingScaleItem TroughLoad => Instance[(short)110];

		public static BuildingScaleItem TaiwuShrineEffect => Instance[(short)111];

		public static BuildingScaleItem WareHouseLoad => Instance[(short)112];

		public static BuildingScaleItem ResidenceCapacity => Instance[(short)113];

		public static BuildingScaleItem ComfortableHouseCapacity => Instance[(short)114];

		public static BuildingScaleItem TeaHorseCaravanSlot => Instance[(short)116];

		public static BuildingScaleItem PhoenixPlatformPracticalEffect => Instance[(short)225];

		public static BuildingScaleItem StrategyRoomEffect => Instance[(short)226];

		public static BuildingScaleItem MakeupRoomPercentEffect => Instance[(short)227];

		public static BuildingScaleItem BirthDeathStreamerEffect => Instance[(short)228];

		public static BuildingScaleItem LifeElixirRoomEffect => Instance[(short)229];

		public static BuildingScaleItem SutraReadingRoomEffect => Instance[(short)230];
	}

	public static BuildingScale Instance = new BuildingScale();

	private readonly HashSet<string> RequiredFields = new HashSet<string> { "CombatSkillType", "LifeSkillType", "ResourceType", "Formula", "TemplateId", "Desc", "Name", "DlcAppId", "LevelEffect" };

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
		_dataArray.Add(new BuildingScaleItem(0, 0, 1, EBuildingScaleClass.LevelEffect, EBuildingScaleType.Int, EBuildingScaleEffect.Invalid, -1, -1, 0u, -1, -1, new List<int>
		{
			21, 22, 23, 24, 25, 26, 27, 28, 29, 30,
			31, 32, 33, 34, 35, 36, 37, 38, 39, 40
		}));
		_dataArray.Add(new BuildingScaleItem(1, 0, 1, EBuildingScaleClass.LevelEffect, EBuildingScaleType.Int, EBuildingScaleEffect.Invalid, -1, -1, 0u, -1, -1, new List<int>
		{
			21, 22, 23, 24, 25, 26, 27, 28, 29, 30,
			31, 32, 33, 34, 35, 36, 37, 38, 39, 40
		}));
		_dataArray.Add(new BuildingScaleItem(2, 0, 1, EBuildingScaleClass.LevelEffect, EBuildingScaleType.Int, EBuildingScaleEffect.Invalid, -1, -1, 0u, -1, -1, new List<int>
		{
			21, 22, 23, 24, 25, 26, 27, 28, 29, 30,
			31, 32, 33, 34, 35, 36, 37, 38, 39, 40
		}));
		_dataArray.Add(new BuildingScaleItem(3, 0, 1, EBuildingScaleClass.LevelEffect, EBuildingScaleType.Int, EBuildingScaleEffect.Invalid, -1, -1, 0u, -1, -1, new List<int>
		{
			21, 22, 23, 24, 25, 26, 27, 28, 29, 30,
			31, 32, 33, 34, 35, 36, 37, 38, 39, 40
		}));
		_dataArray.Add(new BuildingScaleItem(4, 0, 1, EBuildingScaleClass.LevelEffect, EBuildingScaleType.Int, EBuildingScaleEffect.Invalid, -1, -1, 0u, -1, -1, new List<int>
		{
			21, 22, 23, 24, 25, 26, 27, 28, 29, 30,
			31, 32, 33, 34, 35, 36, 37, 38, 39, 40
		}));
		_dataArray.Add(new BuildingScaleItem(5, 0, 1, EBuildingScaleClass.LevelEffect, EBuildingScaleType.Int, EBuildingScaleEffect.Invalid, -1, -1, 0u, -1, -1, new List<int>
		{
			21, 22, 23, 24, 25, 26, 27, 28, 29, 30,
			31, 32, 33, 34, 35, 36, 37, 38, 39, 40
		}));
		_dataArray.Add(new BuildingScaleItem(6, 0, 1, EBuildingScaleClass.LevelEffect, EBuildingScaleType.Int, EBuildingScaleEffect.Invalid, -1, -1, 0u, -1, -1, new List<int>
		{
			21, 22, 23, 24, 25, 26, 27, 28, 29, 30,
			31, 32, 33, 34, 35, 36, 37, 38, 39, 40
		}));
		_dataArray.Add(new BuildingScaleItem(7, 0, 1, EBuildingScaleClass.LevelEffect, EBuildingScaleType.Int, EBuildingScaleEffect.Invalid, -1, -1, 0u, -1, -1, new List<int>
		{
			21, 22, 23, 24, 25, 26, 27, 28, 29, 30,
			31, 32, 33, 34, 35, 36, 37, 38, 39, 40
		}));
		_dataArray.Add(new BuildingScaleItem(8, 0, 1, EBuildingScaleClass.LevelEffect, EBuildingScaleType.Int, EBuildingScaleEffect.Invalid, -1, -1, 0u, -1, -1, new List<int>
		{
			21, 22, 23, 24, 25, 26, 27, 28, 29, 30,
			31, 32, 33, 34, 35, 36, 37, 38, 39, 40
		}));
		_dataArray.Add(new BuildingScaleItem(9, 0, 1, EBuildingScaleClass.LevelEffect, EBuildingScaleType.Int, EBuildingScaleEffect.Invalid, -1, -1, 0u, -1, -1, new List<int>
		{
			21, 22, 23, 24, 25, 26, 27, 28, 29, 30,
			31, 32, 33, 34, 35, 36, 37, 38, 39, 40
		}));
		_dataArray.Add(new BuildingScaleItem(10, 0, 1, EBuildingScaleClass.LevelEffect, EBuildingScaleType.Int, EBuildingScaleEffect.Invalid, -1, -1, 0u, -1, -1, new List<int>
		{
			21, 22, 23, 24, 25, 26, 27, 28, 29, 30,
			31, 32, 33, 34, 35, 36, 37, 38, 39, 40
		}));
		_dataArray.Add(new BuildingScaleItem(11, 0, 1, EBuildingScaleClass.LevelEffect, EBuildingScaleType.Int, EBuildingScaleEffect.Invalid, -1, -1, 0u, -1, -1, new List<int>
		{
			21, 22, 23, 24, 25, 26, 27, 28, 29, 30,
			31, 32, 33, 34, 35, 36, 37, 38, 39, 40
		}));
		_dataArray.Add(new BuildingScaleItem(12, 0, 1, EBuildingScaleClass.LevelEffect, EBuildingScaleType.Int, EBuildingScaleEffect.Invalid, -1, -1, 0u, -1, -1, new List<int>
		{
			21, 22, 23, 24, 25, 26, 27, 28, 29, 30,
			31, 32, 33, 34, 35, 36, 37, 38, 39, 40
		}));
		_dataArray.Add(new BuildingScaleItem(13, 0, 1, EBuildingScaleClass.LevelEffect, EBuildingScaleType.Int, EBuildingScaleEffect.Invalid, -1, -1, 0u, -1, -1, new List<int>
		{
			21, 22, 23, 24, 25, 26, 27, 28, 29, 30,
			31, 32, 33, 34, 35, 36, 37, 38, 39, 40
		}));
		_dataArray.Add(new BuildingScaleItem(14, 0, 1, EBuildingScaleClass.LevelEffect, EBuildingScaleType.Int, EBuildingScaleEffect.Invalid, -1, -1, 0u, -1, -1, new List<int>
		{
			21, 22, 23, 24, 25, 26, 27, 28, 29, 30,
			31, 32, 33, 34, 35, 36, 37, 38, 39, 40
		}));
		_dataArray.Add(new BuildingScaleItem(15, 0, 1, EBuildingScaleClass.LevelEffect, EBuildingScaleType.Int, EBuildingScaleEffect.Invalid, -1, -1, 0u, -1, -1, new List<int>
		{
			21, 22, 23, 24, 25, 26, 27, 28, 29, 30,
			31, 32, 33, 34, 35, 36, 37, 38, 39, 40
		}));
		_dataArray.Add(new BuildingScaleItem(16, 0, 1, EBuildingScaleClass.LevelEffect, EBuildingScaleType.Int, EBuildingScaleEffect.Invalid, -1, -1, 0u, -1, -1, new List<int>
		{
			21, 22, 23, 24, 25, 26, 27, 28, 29, 30,
			31, 32, 33, 34, 35, 36, 37, 38, 39, 40
		}));
		_dataArray.Add(new BuildingScaleItem(17, 0, 1, EBuildingScaleClass.LevelEffect, EBuildingScaleType.Int, EBuildingScaleEffect.Invalid, -1, -1, 0u, -1, -1, new List<int>
		{
			21, 22, 23, 24, 25, 26, 27, 28, 29, 30,
			31, 32, 33, 34, 35, 36, 37, 38, 39, 40
		}));
		_dataArray.Add(new BuildingScaleItem(18, 0, 1, EBuildingScaleClass.LevelEffect, EBuildingScaleType.Int, EBuildingScaleEffect.Invalid, -1, -1, 0u, -1, -1, new List<int>
		{
			21, 22, 23, 24, 25, 26, 27, 28, 29, 30,
			31, 32, 33, 34, 35, 36, 37, 38, 39, 40
		}));
		_dataArray.Add(new BuildingScaleItem(19, 0, 1, EBuildingScaleClass.LevelEffect, EBuildingScaleType.Int, EBuildingScaleEffect.Invalid, -1, -1, 0u, -1, -1, new List<int>
		{
			21, 22, 23, 24, 25, 26, 27, 28, 29, 30,
			31, 32, 33, 34, 35, 36, 37, 38, 39, 40
		}));
		_dataArray.Add(new BuildingScaleItem(20, 2, 3, EBuildingScaleClass.MemberResourceIncome, EBuildingScaleType.Int, EBuildingScaleEffect.Invalid, -1, -1, 0u, 0, 11, new List<int>()));
		_dataArray.Add(new BuildingScaleItem(21, 4, 5, EBuildingScaleClass.MemberResourceIncome, EBuildingScaleType.Int, EBuildingScaleEffect.Invalid, -1, -1, 0u, 2, 13, new List<int>()));
		_dataArray.Add(new BuildingScaleItem(22, 6, 7, EBuildingScaleClass.MemberResourceIncome, EBuildingScaleType.Int, EBuildingScaleEffect.Invalid, -1, -1, 0u, 1, 15, new List<int>()));
		_dataArray.Add(new BuildingScaleItem(23, 4, 5, EBuildingScaleClass.MemberResourceIncome, EBuildingScaleType.Int, EBuildingScaleEffect.Invalid, -1, -1, 0u, 2, 18, new List<int>()));
		_dataArray.Add(new BuildingScaleItem(24, 8, 9, EBuildingScaleClass.MemberResourceIncome, EBuildingScaleType.Int, EBuildingScaleEffect.Invalid, -1, -1, 0u, 3, 18, new List<int>()));
		_dataArray.Add(new BuildingScaleItem(25, 10, 11, EBuildingScaleClass.MemberResourceIncome, EBuildingScaleType.Int, EBuildingScaleEffect.Invalid, -1, -1, 0u, 5, 20, new List<int>()));
		_dataArray.Add(new BuildingScaleItem(26, 10, 11, EBuildingScaleClass.MemberResourceIncome, EBuildingScaleType.Int, EBuildingScaleEffect.Invalid, -1, -1, 0u, 5, 22, new List<int>()));
		_dataArray.Add(new BuildingScaleItem(27, 12, 13, EBuildingScaleClass.MemberResourceIncome, EBuildingScaleType.Int, EBuildingScaleEffect.Invalid, -1, -1, 0u, 4, 24, new List<int>()));
		_dataArray.Add(new BuildingScaleItem(28, 8, 9, EBuildingScaleClass.MemberResourceIncome, EBuildingScaleType.Int, EBuildingScaleEffect.Invalid, -1, -1, 0u, 3, 27, new List<int>()));
		_dataArray.Add(new BuildingScaleItem(29, 2, 3, EBuildingScaleClass.MemberResourceIncome, EBuildingScaleType.Int, EBuildingScaleEffect.Invalid, -1, -1, 0u, 0, 29, new List<int>()));
		_dataArray.Add(new BuildingScaleItem(30, 6, 7, EBuildingScaleClass.MemberResourceIncome, EBuildingScaleType.Int, EBuildingScaleEffect.Invalid, -1, -1, 0u, 1, 29, new List<int>()));
		_dataArray.Add(new BuildingScaleItem(31, 2, 3, EBuildingScaleClass.MemberResourceIncome, EBuildingScaleType.Int, EBuildingScaleEffect.Invalid, -1, -1, 0u, 0, 31, new List<int>()));
		_dataArray.Add(new BuildingScaleItem(32, 12, 13, EBuildingScaleClass.MemberResourceIncome, EBuildingScaleType.Int, EBuildingScaleEffect.Invalid, -1, -1, 0u, 4, 31, new List<int>()));
		_dataArray.Add(new BuildingScaleItem(33, 14, 15, EBuildingScaleClass.Invalid, EBuildingScaleType.BonusPercentage, EBuildingScaleEffect.MigrateSpeedBonusFactor, -1, -1, 0u, -1, 12, new List<int>()));
		_dataArray.Add(new BuildingScaleItem(34, 16, 17, EBuildingScaleClass.MemberResourceIncome, EBuildingScaleType.Int, EBuildingScaleEffect.Invalid, -1, -1, 0u, 7, 14, new List<int>()));
		_dataArray.Add(new BuildingScaleItem(35, 18, 19, EBuildingScaleClass.Invalid, EBuildingScaleType.BonusPercentage, EBuildingScaleEffect.CollectResourceIncomeBonus, -1, -1, 0u, -1, 16, new List<int>()));
		_dataArray.Add(new BuildingScaleItem(36, 20, 21, EBuildingScaleClass.Invalid, EBuildingScaleType.BonusPercentage, EBuildingScaleEffect.CollectResourceGetItemBonus, -1, -1, 0u, -1, 17, new List<int>()));
		_dataArray.Add(new BuildingScaleItem(37, 22, 23, EBuildingScaleClass.Invalid, EBuildingScaleType.BonusPercentage, EBuildingScaleEffect.ProfessionSeniorityBonus, -1, -1, 0u, -1, 19, new List<int>()));
		_dataArray.Add(new BuildingScaleItem(38, 24, 25, EBuildingScaleClass.Invalid, EBuildingScaleType.BonusPercentage, EBuildingScaleEffect.HealthDecreaseReduction, -1, -1, 0u, -1, 21, new List<int>()));
		_dataArray.Add(new BuildingScaleItem(39, 26, 27, EBuildingScaleClass.MemberExpIncome, EBuildingScaleType.Int, EBuildingScaleEffect.Invalid, -1, -1, 0u, -1, 23, new List<int>()));
		_dataArray.Add(new BuildingScaleItem(40, 28, 29, EBuildingScaleClass.Invalid, EBuildingScaleType.BonusPercentage, EBuildingScaleEffect.FavorIncreaseBonus, -1, -1, 0u, -1, 25, new List<int>()));
		_dataArray.Add(new BuildingScaleItem(41, 30, 31, EBuildingScaleClass.Invalid, EBuildingScaleType.BonusPercentage, EBuildingScaleEffect.FavorDecreaseReduction, -1, -1, 0u, -1, 26, new List<int>()));
		_dataArray.Add(new BuildingScaleItem(42, 32, 33, EBuildingScaleClass.MemberResourceIncome, EBuildingScaleType.Int, EBuildingScaleEffect.Invalid, -1, -1, 0u, 6, 28, new List<int>()));
		_dataArray.Add(new BuildingScaleItem(43, 34, 35, EBuildingScaleClass.Invalid, EBuildingScaleType.BonusPercentage, EBuildingScaleEffect.MapResourceRegenBonus, -1, -1, 0u, -1, 30, new List<int>()));
		_dataArray.Add(new BuildingScaleItem(44, 36, 37, EBuildingScaleClass.Invalid, EBuildingScaleType.Int, EBuildingScaleEffect.AnimalProgressDelta, -1, -1, 0u, -1, 32, new List<int>()));
		_dataArray.Add(new BuildingScaleItem(45, 38, 39, EBuildingScaleClass.Invalid, EBuildingScaleType.BonusPercentage, EBuildingScaleEffect.ShopProgressBonus, -1, -1, 0u, -1, 33, new List<int>()));
		_dataArray.Add(new BuildingScaleItem(46, 40, 41, EBuildingScaleClass.Invalid, EBuildingScaleType.BonusPercentage, EBuildingScaleEffect.ArtisanOrderProgressBonus, -1, -1, 0u, -1, 34, new List<int>()));
		_dataArray.Add(new BuildingScaleItem(47, 42, 43, EBuildingScaleClass.Invalid, EBuildingScaleType.BonusPercentage, EBuildingScaleEffect.BuildingMaterialResourceIncomeBonus, -1, -1, 0u, -1, 35, new List<int>()));
		_dataArray.Add(new BuildingScaleItem(48, 44, 45, EBuildingScaleClass.Invalid, EBuildingScaleType.Int, EBuildingScaleEffect.BuildingSpaceBonus, -1, -1, 0u, -1, 36, new List<int>()));
		_dataArray.Add(new BuildingScaleItem(49, 46, 47, EBuildingScaleClass.Invalid, EBuildingScaleType.MovePoint, EBuildingScaleEffect.ActionPointRegenBonus, -1, -1, 0u, -1, 37, new List<int>()));
		_dataArray.Add(new BuildingScaleItem(50, 48, 17, EBuildingScaleClass.Invalid, EBuildingScaleType.BonusPercentage, EBuildingScaleEffect.BuildingAuthorityIncomeBonus, -1, -1, 0u, -1, 38, new List<int>()));
		_dataArray.Add(new BuildingScaleItem(51, 49, 33, EBuildingScaleClass.Invalid, EBuildingScaleType.BonusPercentage, EBuildingScaleEffect.BuildingMoneyIncomeBonus, -1, -1, 0u, -1, 39, new List<int>()));
		_dataArray.Add(new BuildingScaleItem(52, 50, 51, EBuildingScaleClass.Invalid, EBuildingScaleType.Int, EBuildingScaleEffect.CricketProgressDelta, -1, -1, 0u, -1, 40, new List<int>()));
		_dataArray.Add(new BuildingScaleItem(53, 52, 53, EBuildingScaleClass.Invalid, EBuildingScaleType.Int, EBuildingScaleEffect.AdventureProgressDelta, -1, -1, 0u, -1, 41, new List<int>()));
		_dataArray.Add(new BuildingScaleItem(54, 54, 55, EBuildingScaleClass.Invalid, EBuildingScaleType.BonusPercentage, EBuildingScaleEffect.RecruitBonus, -1, -1, 0u, -1, 42, new List<int>()));
		_dataArray.Add(new BuildingScaleItem(55, 56, 57, EBuildingScaleClass.Slot, EBuildingScaleType.Int, EBuildingScaleEffect.Invalid, -1, -1, 0u, -1, -1, new List<int> { 20 }));
		_dataArray.Add(new BuildingScaleItem(56, 56, 57, EBuildingScaleClass.Slot, EBuildingScaleType.Int, EBuildingScaleEffect.Invalid, -1, -1, 0u, -1, -1, new List<int> { 20 }));
		_dataArray.Add(new BuildingScaleItem(57, 56, 57, EBuildingScaleClass.Slot, EBuildingScaleType.Int, EBuildingScaleEffect.Invalid, -1, -1, 0u, -1, -1, new List<int> { 20 }));
		_dataArray.Add(new BuildingScaleItem(58, 56, 57, EBuildingScaleClass.Slot, EBuildingScaleType.Int, EBuildingScaleEffect.Invalid, -1, -1, 0u, -1, -1, new List<int> { 20 }));
		_dataArray.Add(new BuildingScaleItem(59, 56, 57, EBuildingScaleClass.Slot, EBuildingScaleType.Int, EBuildingScaleEffect.Invalid, -1, -1, 0u, -1, -1, new List<int> { 20 }));
	}

	private void CreateItems1()
	{
		_dataArray.Add(new BuildingScaleItem(60, 56, 57, EBuildingScaleClass.Slot, EBuildingScaleType.Int, EBuildingScaleEffect.Invalid, -1, -1, 0u, -1, -1, new List<int> { 20 }));
		_dataArray.Add(new BuildingScaleItem(61, 56, 57, EBuildingScaleClass.Slot, EBuildingScaleType.Int, EBuildingScaleEffect.Invalid, -1, -1, 0u, -1, -1, new List<int> { 20 }));
		_dataArray.Add(new BuildingScaleItem(62, 56, 57, EBuildingScaleClass.Slot, EBuildingScaleType.Int, EBuildingScaleEffect.Invalid, -1, -1, 0u, -1, -1, new List<int> { 20 }));
		_dataArray.Add(new BuildingScaleItem(63, 56, 57, EBuildingScaleClass.Slot, EBuildingScaleType.Int, EBuildingScaleEffect.Invalid, -1, -1, 0u, -1, -1, new List<int> { 20 }));
		_dataArray.Add(new BuildingScaleItem(64, 56, 57, EBuildingScaleClass.Slot, EBuildingScaleType.Int, EBuildingScaleEffect.Invalid, -1, -1, 0u, -1, -1, new List<int> { 20 }));
		_dataArray.Add(new BuildingScaleItem(65, 56, 57, EBuildingScaleClass.Slot, EBuildingScaleType.Int, EBuildingScaleEffect.Invalid, -1, -1, 0u, -1, -1, new List<int> { 20 }));
		_dataArray.Add(new BuildingScaleItem(66, 56, 57, EBuildingScaleClass.Slot, EBuildingScaleType.Int, EBuildingScaleEffect.Invalid, -1, -1, 0u, -1, -1, new List<int> { 20 }));
		_dataArray.Add(new BuildingScaleItem(67, 56, 57, EBuildingScaleClass.Slot, EBuildingScaleType.Int, EBuildingScaleEffect.Invalid, -1, -1, 0u, -1, -1, new List<int> { 20 }));
		_dataArray.Add(new BuildingScaleItem(68, 56, 57, EBuildingScaleClass.Slot, EBuildingScaleType.Int, EBuildingScaleEffect.Invalid, -1, -1, 0u, -1, -1, new List<int> { 20 }));
		_dataArray.Add(new BuildingScaleItem(69, 56, 57, EBuildingScaleClass.Slot, EBuildingScaleType.Int, EBuildingScaleEffect.Invalid, -1, -1, 0u, -1, -1, new List<int> { 20 }));
		_dataArray.Add(new BuildingScaleItem(70, 56, 57, EBuildingScaleClass.Slot, EBuildingScaleType.Int, EBuildingScaleEffect.Invalid, -1, -1, 0u, -1, -1, new List<int> { 20 }));
		_dataArray.Add(new BuildingScaleItem(71, 56, 57, EBuildingScaleClass.Slot, EBuildingScaleType.Int, EBuildingScaleEffect.Invalid, -1, -1, 0u, -1, -1, new List<int> { 20 }));
		_dataArray.Add(new BuildingScaleItem(72, 56, 57, EBuildingScaleClass.Slot, EBuildingScaleType.Int, EBuildingScaleEffect.Invalid, -1, -1, 0u, -1, -1, new List<int> { 20 }));
		_dataArray.Add(new BuildingScaleItem(73, 56, 57, EBuildingScaleClass.Slot, EBuildingScaleType.Int, EBuildingScaleEffect.Invalid, -1, -1, 0u, -1, -1, new List<int> { 20 }));
		_dataArray.Add(new BuildingScaleItem(74, 56, 57, EBuildingScaleClass.Slot, EBuildingScaleType.Int, EBuildingScaleEffect.Invalid, -1, -1, 0u, -1, -1, new List<int> { 20 }));
		_dataArray.Add(new BuildingScaleItem(75, 58, 59, EBuildingScaleClass.Slot, EBuildingScaleType.Int, EBuildingScaleEffect.Invalid, -1, -1, 0u, -1, -1, new List<int> { 10 }));
		_dataArray.Add(new BuildingScaleItem(76, 56, 57, EBuildingScaleClass.Slot, EBuildingScaleType.Int, EBuildingScaleEffect.Invalid, -1, -1, 0u, -1, -1, new List<int> { 10 }));
		_dataArray.Add(new BuildingScaleItem(77, 56, 57, EBuildingScaleClass.Slot, EBuildingScaleType.Int, EBuildingScaleEffect.Invalid, -1, -1, 0u, -1, -1, new List<int> { 10 }));
		_dataArray.Add(new BuildingScaleItem(78, 60, 61, EBuildingScaleClass.Invalid, EBuildingScaleType.Int, EBuildingScaleEffect.BreakOutSteps, -1, -1, 0u, -1, -1, new List<int> { 5 }));
		_dataArray.Add(new BuildingScaleItem(79, 62, 63, EBuildingScaleClass.Invalid, EBuildingScaleType.BonusPercentage, EBuildingScaleEffect.CombatSkillReadingSpeedBonusFactor, 0, -1, 0u, -1, 43, new List<int>()));
		_dataArray.Add(new BuildingScaleItem(80, 62, 63, EBuildingScaleClass.Invalid, EBuildingScaleType.BonusPercentage, EBuildingScaleEffect.CombatSkillReadingSpeedBonusFactor, 1, -1, 0u, -1, 43, new List<int>()));
		_dataArray.Add(new BuildingScaleItem(81, 62, 63, EBuildingScaleClass.Invalid, EBuildingScaleType.BonusPercentage, EBuildingScaleEffect.CombatSkillReadingSpeedBonusFactor, 2, -1, 0u, -1, 43, new List<int>()));
		_dataArray.Add(new BuildingScaleItem(82, 62, 63, EBuildingScaleClass.Invalid, EBuildingScaleType.BonusPercentage, EBuildingScaleEffect.CombatSkillReadingSpeedBonusFactor, 3, -1, 0u, -1, 43, new List<int>()));
		_dataArray.Add(new BuildingScaleItem(83, 62, 63, EBuildingScaleClass.Invalid, EBuildingScaleType.BonusPercentage, EBuildingScaleEffect.CombatSkillReadingSpeedBonusFactor, 4, -1, 0u, -1, 43, new List<int>()));
		_dataArray.Add(new BuildingScaleItem(84, 62, 63, EBuildingScaleClass.Invalid, EBuildingScaleType.BonusPercentage, EBuildingScaleEffect.CombatSkillReadingSpeedBonusFactor, 5, -1, 0u, -1, 43, new List<int>()));
		_dataArray.Add(new BuildingScaleItem(85, 62, 63, EBuildingScaleClass.Invalid, EBuildingScaleType.BonusPercentage, EBuildingScaleEffect.CombatSkillReadingSpeedBonusFactor, 6, -1, 0u, -1, 43, new List<int>()));
		_dataArray.Add(new BuildingScaleItem(86, 62, 63, EBuildingScaleClass.Invalid, EBuildingScaleType.BonusPercentage, EBuildingScaleEffect.CombatSkillReadingSpeedBonusFactor, 7, -1, 0u, -1, 43, new List<int>()));
		_dataArray.Add(new BuildingScaleItem(87, 62, 63, EBuildingScaleClass.Invalid, EBuildingScaleType.BonusPercentage, EBuildingScaleEffect.CombatSkillReadingSpeedBonusFactor, 8, -1, 0u, -1, 43, new List<int>()));
		_dataArray.Add(new BuildingScaleItem(88, 62, 63, EBuildingScaleClass.Invalid, EBuildingScaleType.BonusPercentage, EBuildingScaleEffect.CombatSkillReadingSpeedBonusFactor, 9, -1, 0u, -1, 43, new List<int>()));
		_dataArray.Add(new BuildingScaleItem(89, 62, 63, EBuildingScaleClass.Invalid, EBuildingScaleType.BonusPercentage, EBuildingScaleEffect.CombatSkillReadingSpeedBonusFactor, 10, -1, 0u, -1, 43, new List<int>()));
		_dataArray.Add(new BuildingScaleItem(90, 62, 63, EBuildingScaleClass.Invalid, EBuildingScaleType.BonusPercentage, EBuildingScaleEffect.CombatSkillReadingSpeedBonusFactor, 11, -1, 0u, -1, 43, new List<int>()));
		_dataArray.Add(new BuildingScaleItem(91, 62, 63, EBuildingScaleClass.Invalid, EBuildingScaleType.BonusPercentage, EBuildingScaleEffect.CombatSkillReadingSpeedBonusFactor, 12, -1, 0u, -1, 43, new List<int>()));
		_dataArray.Add(new BuildingScaleItem(92, 62, 63, EBuildingScaleClass.Invalid, EBuildingScaleType.BonusPercentage, EBuildingScaleEffect.CombatSkillReadingSpeedBonusFactor, 13, -1, 0u, -1, 43, new List<int>()));
		_dataArray.Add(new BuildingScaleItem(93, 64, 65, EBuildingScaleClass.Invalid, EBuildingScaleType.BonusPercentage, EBuildingScaleEffect.BreakOutSuccessRate, 0, -1, 0u, -1, -1, new List<int> { 30 }));
		_dataArray.Add(new BuildingScaleItem(94, 64, 65, EBuildingScaleClass.Invalid, EBuildingScaleType.BonusPercentage, EBuildingScaleEffect.BreakOutSuccessRate, 1, -1, 0u, -1, -1, new List<int> { 30 }));
		_dataArray.Add(new BuildingScaleItem(95, 64, 65, EBuildingScaleClass.Invalid, EBuildingScaleType.BonusPercentage, EBuildingScaleEffect.BreakOutSuccessRate, 2, -1, 0u, -1, -1, new List<int> { 30 }));
		_dataArray.Add(new BuildingScaleItem(96, 64, 65, EBuildingScaleClass.Invalid, EBuildingScaleType.BonusPercentage, EBuildingScaleEffect.BreakOutSuccessRate, 3, -1, 0u, -1, -1, new List<int> { 30 }));
		_dataArray.Add(new BuildingScaleItem(97, 64, 65, EBuildingScaleClass.Invalid, EBuildingScaleType.BonusPercentage, EBuildingScaleEffect.BreakOutSuccessRate, 4, -1, 0u, -1, -1, new List<int> { 30 }));
		_dataArray.Add(new BuildingScaleItem(98, 64, 65, EBuildingScaleClass.Invalid, EBuildingScaleType.BonusPercentage, EBuildingScaleEffect.BreakOutSuccessRate, 5, -1, 0u, -1, -1, new List<int> { 30 }));
		_dataArray.Add(new BuildingScaleItem(99, 64, 65, EBuildingScaleClass.Invalid, EBuildingScaleType.BonusPercentage, EBuildingScaleEffect.BreakOutSuccessRate, 6, -1, 0u, -1, -1, new List<int> { 30 }));
		_dataArray.Add(new BuildingScaleItem(100, 64, 65, EBuildingScaleClass.Invalid, EBuildingScaleType.BonusPercentage, EBuildingScaleEffect.BreakOutSuccessRate, 7, -1, 0u, -1, -1, new List<int> { 30 }));
		_dataArray.Add(new BuildingScaleItem(101, 64, 65, EBuildingScaleClass.Invalid, EBuildingScaleType.BonusPercentage, EBuildingScaleEffect.BreakOutSuccessRate, 8, -1, 0u, -1, -1, new List<int> { 30 }));
		_dataArray.Add(new BuildingScaleItem(102, 64, 65, EBuildingScaleClass.Invalid, EBuildingScaleType.BonusPercentage, EBuildingScaleEffect.BreakOutSuccessRate, 9, -1, 0u, -1, -1, new List<int> { 30 }));
		_dataArray.Add(new BuildingScaleItem(103, 64, 65, EBuildingScaleClass.Invalid, EBuildingScaleType.BonusPercentage, EBuildingScaleEffect.BreakOutSuccessRate, 10, -1, 0u, -1, -1, new List<int> { 30 }));
		_dataArray.Add(new BuildingScaleItem(104, 64, 65, EBuildingScaleClass.Invalid, EBuildingScaleType.BonusPercentage, EBuildingScaleEffect.BreakOutSuccessRate, 11, -1, 0u, -1, -1, new List<int> { 30 }));
		_dataArray.Add(new BuildingScaleItem(105, 64, 65, EBuildingScaleClass.Invalid, EBuildingScaleType.BonusPercentage, EBuildingScaleEffect.BreakOutSuccessRate, 12, -1, 0u, -1, -1, new List<int> { 30 }));
		_dataArray.Add(new BuildingScaleItem(106, 64, 65, EBuildingScaleClass.Invalid, EBuildingScaleType.BonusPercentage, EBuildingScaleEffect.BreakOutSuccessRate, 13, -1, 0u, -1, -1, new List<int> { 30 }));
		_dataArray.Add(new BuildingScaleItem(107, 66, 45, EBuildingScaleClass.Invalid, EBuildingScaleType.Int, EBuildingScaleEffect.Invalid, -1, -1, 0u, -1, -1, new List<int>
		{
			33, 36, 39, 42, 45, 48, 51, 54, 57, 60,
			63, 66, 69, 72, 75
		}));
		_dataArray.Add(new BuildingScaleItem(108, 67, 68, EBuildingScaleClass.Invalid, EBuildingScaleType.Int, EBuildingScaleEffect.Invalid, -1, -1, 0u, -1, -1, new List<int>
		{
			25, 30, 35, 40, 45, 50, 55, 60, 65, 70,
			75, 80, 85, 90, 95
		}));
		_dataArray.Add(new BuildingScaleItem(109, 69, 70, EBuildingScaleClass.Invalid, EBuildingScaleType.Int, EBuildingScaleEffect.Invalid, -1, -1, 2764950u, -1, -1, new List<int>
		{
			0, 2, 3, 3, 4, 4, 5, 5, 6, 6,
			7, 7, 8, 8, 9
		}));
		_dataArray.Add(new BuildingScaleItem(110, 71, 72, EBuildingScaleClass.Invalid, EBuildingScaleType.Int, EBuildingScaleEffect.Invalid, -1, -1, 0u, -1, -1, new List<int> { 100 }));
		_dataArray.Add(new BuildingScaleItem(111, 73, 74, EBuildingScaleClass.Invalid, EBuildingScaleType.Int, EBuildingScaleEffect.Invalid, -1, -1, 0u, -1, 7, new List<int>()));
		_dataArray.Add(new BuildingScaleItem(112, 71, 72, EBuildingScaleClass.Invalid, EBuildingScaleType.Int, EBuildingScaleEffect.Invalid, -1, -1, 0u, -1, -1, new List<int> { 50, 100, 150, 200, 250, 300, 350, 400, 450 }));
		_dataArray.Add(new BuildingScaleItem(113, 75, 76, EBuildingScaleClass.Invalid, EBuildingScaleType.Int, EBuildingScaleEffect.Invalid, -1, -1, 0u, -1, -1, new List<int> { 7, 14, 21, 28, 35, 42, 49, 56, 63 }));
		_dataArray.Add(new BuildingScaleItem(114, 77, 78, EBuildingScaleClass.Invalid, EBuildingScaleType.Int, EBuildingScaleEffect.Invalid, -1, -1, 0u, -1, -1, new List<int> { 1, 2, 3 }));
		_dataArray.Add(new BuildingScaleItem(115, 79, 80, EBuildingScaleClass.Invalid, EBuildingScaleType.Int, EBuildingScaleEffect.Invalid, -1, -1, 0u, -1, -1, new List<int> { 1, 2, 3, 4, 5, 6 }));
		_dataArray.Add(new BuildingScaleItem(116, 81, 82, EBuildingScaleClass.Invalid, EBuildingScaleType.Int, EBuildingScaleEffect.Invalid, -1, -1, 0u, -1, -1, new List<int>
		{
			2, 4, 6, 8, 10, 12, 14, 16, 18, 20,
			22, 24, 26, 28, 30, 32, 34, 36, 38, 40
		}));
		_dataArray.Add(new BuildingScaleItem(117, 83, 63, EBuildingScaleClass.Invalid, EBuildingScaleType.BonusPercentage, EBuildingScaleEffect.LifeSkillReadingSpeedBonusFactor, -1, 0, 0u, -1, 44, new List<int>()));
		_dataArray.Add(new BuildingScaleItem(118, 83, 63, EBuildingScaleClass.Invalid, EBuildingScaleType.BonusPercentage, EBuildingScaleEffect.LifeSkillReadingSpeedBonusFactor, -1, 1, 0u, -1, 44, new List<int>()));
		_dataArray.Add(new BuildingScaleItem(119, 83, 63, EBuildingScaleClass.Invalid, EBuildingScaleType.BonusPercentage, EBuildingScaleEffect.LifeSkillReadingSpeedBonusFactor, -1, 2, 0u, -1, 44, new List<int>()));
	}

	private void CreateItems2()
	{
		_dataArray.Add(new BuildingScaleItem(120, 83, 63, EBuildingScaleClass.Invalid, EBuildingScaleType.BonusPercentage, EBuildingScaleEffect.LifeSkillReadingSpeedBonusFactor, -1, 3, 0u, -1, 44, new List<int>()));
		_dataArray.Add(new BuildingScaleItem(121, 83, 63, EBuildingScaleClass.Invalid, EBuildingScaleType.BonusPercentage, EBuildingScaleEffect.LifeSkillReadingSpeedBonusFactor, -1, 4, 0u, -1, 44, new List<int>()));
		_dataArray.Add(new BuildingScaleItem(122, 83, 63, EBuildingScaleClass.Invalid, EBuildingScaleType.BonusPercentage, EBuildingScaleEffect.LifeSkillReadingSpeedBonusFactor, -1, 5, 0u, -1, 44, new List<int>()));
		_dataArray.Add(new BuildingScaleItem(123, 83, 63, EBuildingScaleClass.Invalid, EBuildingScaleType.BonusPercentage, EBuildingScaleEffect.LifeSkillReadingSpeedBonusFactor, -1, 6, 0u, -1, 44, new List<int>()));
		_dataArray.Add(new BuildingScaleItem(124, 83, 63, EBuildingScaleClass.Invalid, EBuildingScaleType.BonusPercentage, EBuildingScaleEffect.LifeSkillReadingSpeedBonusFactor, -1, 7, 0u, -1, 44, new List<int>()));
		_dataArray.Add(new BuildingScaleItem(125, 83, 63, EBuildingScaleClass.Invalid, EBuildingScaleType.BonusPercentage, EBuildingScaleEffect.LifeSkillReadingSpeedBonusFactor, -1, 8, 0u, -1, 44, new List<int>()));
		_dataArray.Add(new BuildingScaleItem(126, 83, 63, EBuildingScaleClass.Invalid, EBuildingScaleType.BonusPercentage, EBuildingScaleEffect.LifeSkillReadingSpeedBonusFactor, -1, 9, 0u, -1, 44, new List<int>()));
		_dataArray.Add(new BuildingScaleItem(127, 83, 63, EBuildingScaleClass.Invalid, EBuildingScaleType.BonusPercentage, EBuildingScaleEffect.LifeSkillReadingSpeedBonusFactor, -1, 10, 0u, -1, 44, new List<int>()));
		_dataArray.Add(new BuildingScaleItem(128, 83, 63, EBuildingScaleClass.Invalid, EBuildingScaleType.BonusPercentage, EBuildingScaleEffect.LifeSkillReadingSpeedBonusFactor, -1, 11, 0u, -1, 44, new List<int>()));
		_dataArray.Add(new BuildingScaleItem(129, 83, 63, EBuildingScaleClass.Invalid, EBuildingScaleType.BonusPercentage, EBuildingScaleEffect.LifeSkillReadingSpeedBonusFactor, -1, 12, 0u, -1, 44, new List<int>()));
		_dataArray.Add(new BuildingScaleItem(130, 83, 63, EBuildingScaleClass.Invalid, EBuildingScaleType.BonusPercentage, EBuildingScaleEffect.LifeSkillReadingSpeedBonusFactor, -1, 13, 0u, -1, 44, new List<int>()));
		_dataArray.Add(new BuildingScaleItem(131, 83, 63, EBuildingScaleClass.Invalid, EBuildingScaleType.BonusPercentage, EBuildingScaleEffect.LifeSkillReadingSpeedBonusFactor, -1, 14, 0u, -1, 44, new List<int>()));
		_dataArray.Add(new BuildingScaleItem(132, 83, 63, EBuildingScaleClass.Invalid, EBuildingScaleType.BonusPercentage, EBuildingScaleEffect.LifeSkillReadingSpeedBonusFactor, -1, 15, 0u, -1, 44, new List<int>()));
		_dataArray.Add(new BuildingScaleItem(133, 84, 85, EBuildingScaleClass.Slot, EBuildingScaleType.Int, EBuildingScaleEffect.Invalid, -1, -1, 0u, -1, -1, new List<int> { 10 }));
		_dataArray.Add(new BuildingScaleItem(134, 84, 85, EBuildingScaleClass.Slot, EBuildingScaleType.Int, EBuildingScaleEffect.Invalid, -1, -1, 0u, -1, -1, new List<int> { 10 }));
		_dataArray.Add(new BuildingScaleItem(135, 84, 85, EBuildingScaleClass.Slot, EBuildingScaleType.Int, EBuildingScaleEffect.Invalid, -1, -1, 0u, -1, -1, new List<int> { 10 }));
		_dataArray.Add(new BuildingScaleItem(136, 84, 85, EBuildingScaleClass.Slot, EBuildingScaleType.Int, EBuildingScaleEffect.Invalid, -1, -1, 0u, -1, -1, new List<int> { 10 }));
		_dataArray.Add(new BuildingScaleItem(137, 84, 85, EBuildingScaleClass.Slot, EBuildingScaleType.Int, EBuildingScaleEffect.Invalid, -1, -1, 0u, -1, -1, new List<int> { 10 }));
		_dataArray.Add(new BuildingScaleItem(138, 84, 85, EBuildingScaleClass.Slot, EBuildingScaleType.Int, EBuildingScaleEffect.Invalid, -1, -1, 0u, -1, -1, new List<int> { 10 }));
		_dataArray.Add(new BuildingScaleItem(139, 84, 85, EBuildingScaleClass.Slot, EBuildingScaleType.Int, EBuildingScaleEffect.Invalid, -1, -1, 0u, -1, -1, new List<int> { 10 }));
		_dataArray.Add(new BuildingScaleItem(140, 84, 85, EBuildingScaleClass.Slot, EBuildingScaleType.Int, EBuildingScaleEffect.Invalid, -1, -1, 0u, -1, -1, new List<int> { 10 }));
		_dataArray.Add(new BuildingScaleItem(141, 84, 85, EBuildingScaleClass.Slot, EBuildingScaleType.Int, EBuildingScaleEffect.Invalid, -1, -1, 0u, -1, -1, new List<int> { 10 }));
		_dataArray.Add(new BuildingScaleItem(142, 84, 85, EBuildingScaleClass.Slot, EBuildingScaleType.Int, EBuildingScaleEffect.Invalid, -1, -1, 0u, -1, -1, new List<int> { 10 }));
		_dataArray.Add(new BuildingScaleItem(143, 56, 57, EBuildingScaleClass.Slot, EBuildingScaleType.Int, EBuildingScaleEffect.Invalid, -1, -1, 0u, -1, -1, new List<int> { 10 }));
		_dataArray.Add(new BuildingScaleItem(144, 56, 57, EBuildingScaleClass.Slot, EBuildingScaleType.Int, EBuildingScaleEffect.Invalid, -1, -1, 0u, -1, -1, new List<int> { 10 }));
		_dataArray.Add(new BuildingScaleItem(145, 56, 57, EBuildingScaleClass.Slot, EBuildingScaleType.Int, EBuildingScaleEffect.Invalid, -1, -1, 0u, -1, -1, new List<int> { 10 }));
		_dataArray.Add(new BuildingScaleItem(146, 56, 57, EBuildingScaleClass.Slot, EBuildingScaleType.Int, EBuildingScaleEffect.Invalid, -1, -1, 0u, -1, -1, new List<int> { 10 }));
		_dataArray.Add(new BuildingScaleItem(147, 56, 57, EBuildingScaleClass.Slot, EBuildingScaleType.Int, EBuildingScaleEffect.Invalid, -1, -1, 0u, -1, -1, new List<int> { 10 }));
		_dataArray.Add(new BuildingScaleItem(148, 56, 57, EBuildingScaleClass.Slot, EBuildingScaleType.Int, EBuildingScaleEffect.Invalid, -1, -1, 0u, -1, -1, new List<int> { 10 }));
		_dataArray.Add(new BuildingScaleItem(149, 56, 57, EBuildingScaleClass.Slot, EBuildingScaleType.Int, EBuildingScaleEffect.Invalid, -1, -1, 0u, -1, -1, new List<int> { 10 }));
		_dataArray.Add(new BuildingScaleItem(150, 56, 57, EBuildingScaleClass.Slot, EBuildingScaleType.Int, EBuildingScaleEffect.Invalid, -1, -1, 0u, -1, -1, new List<int> { 10 }));
		_dataArray.Add(new BuildingScaleItem(151, 56, 57, EBuildingScaleClass.Slot, EBuildingScaleType.Int, EBuildingScaleEffect.Invalid, -1, -1, 0u, -1, -1, new List<int> { 10 }));
		_dataArray.Add(new BuildingScaleItem(152, 56, 57, EBuildingScaleClass.Slot, EBuildingScaleType.Int, EBuildingScaleEffect.Invalid, -1, -1, 0u, -1, -1, new List<int> { 10 }));
		_dataArray.Add(new BuildingScaleItem(153, 56, 57, EBuildingScaleClass.Slot, EBuildingScaleType.Int, EBuildingScaleEffect.Invalid, -1, -1, 0u, -1, -1, new List<int> { 10 }));
		_dataArray.Add(new BuildingScaleItem(154, 56, 57, EBuildingScaleClass.Slot, EBuildingScaleType.Int, EBuildingScaleEffect.Invalid, -1, -1, 0u, -1, -1, new List<int> { 10 }));
		_dataArray.Add(new BuildingScaleItem(155, 56, 57, EBuildingScaleClass.Slot, EBuildingScaleType.Int, EBuildingScaleEffect.Invalid, -1, -1, 0u, -1, -1, new List<int> { 10 }));
		_dataArray.Add(new BuildingScaleItem(156, 56, 57, EBuildingScaleClass.Slot, EBuildingScaleType.Int, EBuildingScaleEffect.Invalid, -1, -1, 0u, -1, -1, new List<int> { 10 }));
		_dataArray.Add(new BuildingScaleItem(157, 84, 57, EBuildingScaleClass.Slot, EBuildingScaleType.Int, EBuildingScaleEffect.Invalid, -1, -1, 0u, -1, -1, new List<int> { 10 }));
		_dataArray.Add(new BuildingScaleItem(158, 56, 57, EBuildingScaleClass.Slot, EBuildingScaleType.Int, EBuildingScaleEffect.Invalid, -1, -1, 0u, -1, -1, new List<int> { 10 }));
		_dataArray.Add(new BuildingScaleItem(159, 56, 57, EBuildingScaleClass.Slot, EBuildingScaleType.Int, EBuildingScaleEffect.Invalid, -1, -1, 0u, -1, -1, new List<int> { 10 }));
		_dataArray.Add(new BuildingScaleItem(160, 84, 85, EBuildingScaleClass.Slot, EBuildingScaleType.Int, EBuildingScaleEffect.Invalid, -1, -1, 0u, -1, -1, new List<int> { 10 }));
		_dataArray.Add(new BuildingScaleItem(161, 56, 57, EBuildingScaleClass.Slot, EBuildingScaleType.Int, EBuildingScaleEffect.Invalid, -1, -1, 0u, -1, -1, new List<int> { 10 }));
		_dataArray.Add(new BuildingScaleItem(162, 84, 85, EBuildingScaleClass.Slot, EBuildingScaleType.Int, EBuildingScaleEffect.Invalid, -1, -1, 0u, -1, -1, new List<int> { 10 }));
		_dataArray.Add(new BuildingScaleItem(163, 84, 85, EBuildingScaleClass.Slot, EBuildingScaleType.Int, EBuildingScaleEffect.Invalid, -1, -1, 0u, -1, -1, new List<int> { 10 }));
		_dataArray.Add(new BuildingScaleItem(164, 84, 85, EBuildingScaleClass.Slot, EBuildingScaleType.Int, EBuildingScaleEffect.Invalid, -1, -1, 0u, -1, -1, new List<int> { 10 }));
		_dataArray.Add(new BuildingScaleItem(165, 84, 85, EBuildingScaleClass.Slot, EBuildingScaleType.Int, EBuildingScaleEffect.Invalid, -1, -1, 0u, -1, -1, new List<int> { 10 }));
		_dataArray.Add(new BuildingScaleItem(166, 84, 85, EBuildingScaleClass.Slot, EBuildingScaleType.Int, EBuildingScaleEffect.Invalid, -1, -1, 0u, -1, -1, new List<int> { 10 }));
		_dataArray.Add(new BuildingScaleItem(167, 84, 85, EBuildingScaleClass.Slot, EBuildingScaleType.Int, EBuildingScaleEffect.Invalid, -1, -1, 0u, -1, -1, new List<int> { 10 }));
		_dataArray.Add(new BuildingScaleItem(168, 58, 59, EBuildingScaleClass.Slot, EBuildingScaleType.Int, EBuildingScaleEffect.Invalid, -1, -1, 0u, -1, -1, new List<int> { 10 }));
		_dataArray.Add(new BuildingScaleItem(169, 58, 59, EBuildingScaleClass.Slot, EBuildingScaleType.Int, EBuildingScaleEffect.Invalid, -1, -1, 0u, -1, -1, new List<int> { 10 }));
		_dataArray.Add(new BuildingScaleItem(170, 58, 59, EBuildingScaleClass.Slot, EBuildingScaleType.Int, EBuildingScaleEffect.Invalid, -1, -1, 0u, -1, -1, new List<int> { 10 }));
		_dataArray.Add(new BuildingScaleItem(171, 58, 59, EBuildingScaleClass.Slot, EBuildingScaleType.Int, EBuildingScaleEffect.Invalid, -1, -1, 0u, -1, -1, new List<int> { 10 }));
		_dataArray.Add(new BuildingScaleItem(172, 58, 59, EBuildingScaleClass.Slot, EBuildingScaleType.Int, EBuildingScaleEffect.Invalid, -1, -1, 0u, -1, -1, new List<int> { 10 }));
		_dataArray.Add(new BuildingScaleItem(173, 58, 59, EBuildingScaleClass.Slot, EBuildingScaleType.Int, EBuildingScaleEffect.Invalid, -1, -1, 0u, -1, -1, new List<int> { 10 }));
		_dataArray.Add(new BuildingScaleItem(174, 58, 59, EBuildingScaleClass.Slot, EBuildingScaleType.Int, EBuildingScaleEffect.Invalid, -1, -1, 0u, -1, -1, new List<int> { 10 }));
		_dataArray.Add(new BuildingScaleItem(175, 58, 59, EBuildingScaleClass.Slot, EBuildingScaleType.Int, EBuildingScaleEffect.Invalid, -1, -1, 0u, -1, -1, new List<int> { 10 }));
		_dataArray.Add(new BuildingScaleItem(176, 58, 59, EBuildingScaleClass.Slot, EBuildingScaleType.Int, EBuildingScaleEffect.Invalid, -1, -1, 0u, -1, -1, new List<int> { 10 }));
		_dataArray.Add(new BuildingScaleItem(177, 58, 59, EBuildingScaleClass.Slot, EBuildingScaleType.Int, EBuildingScaleEffect.Invalid, -1, -1, 0u, -1, -1, new List<int> { 10 }));
		_dataArray.Add(new BuildingScaleItem(178, 58, 59, EBuildingScaleClass.Slot, EBuildingScaleType.Int, EBuildingScaleEffect.Invalid, -1, -1, 0u, -1, -1, new List<int> { 10 }));
		_dataArray.Add(new BuildingScaleItem(179, 58, 59, EBuildingScaleClass.Slot, EBuildingScaleType.Int, EBuildingScaleEffect.Invalid, -1, -1, 0u, -1, -1, new List<int> { 10 }));
	}

	private void CreateItems3()
	{
		_dataArray.Add(new BuildingScaleItem(180, 58, 59, EBuildingScaleClass.Slot, EBuildingScaleType.Int, EBuildingScaleEffect.Invalid, -1, -1, 0u, -1, -1, new List<int> { 10 }));
		_dataArray.Add(new BuildingScaleItem(181, 58, 59, EBuildingScaleClass.Slot, EBuildingScaleType.Int, EBuildingScaleEffect.Invalid, -1, -1, 0u, -1, -1, new List<int> { 10 }));
		_dataArray.Add(new BuildingScaleItem(182, 58, 59, EBuildingScaleClass.Slot, EBuildingScaleType.Int, EBuildingScaleEffect.Invalid, -1, -1, 0u, -1, -1, new List<int> { 10 }));
		_dataArray.Add(new BuildingScaleItem(183, 58, 59, EBuildingScaleClass.Slot, EBuildingScaleType.Int, EBuildingScaleEffect.Invalid, -1, -1, 0u, -1, -1, new List<int> { 10 }));
		_dataArray.Add(new BuildingScaleItem(184, 58, 59, EBuildingScaleClass.Slot, EBuildingScaleType.Int, EBuildingScaleEffect.Invalid, -1, -1, 0u, -1, -1, new List<int> { 10 }));
		_dataArray.Add(new BuildingScaleItem(185, 58, 59, EBuildingScaleClass.Slot, EBuildingScaleType.Int, EBuildingScaleEffect.Invalid, -1, -1, 0u, -1, -1, new List<int> { 20 }));
		_dataArray.Add(new BuildingScaleItem(186, 86, 87, EBuildingScaleClass.Invalid, EBuildingScaleType.BonusPercentage, EBuildingScaleEffect.ShopManagerQualificationImproveRate, -1, 0, 0u, -1, -1, new List<int> { 20 }));
		_dataArray.Add(new BuildingScaleItem(187, 86, 87, EBuildingScaleClass.Invalid, EBuildingScaleType.BonusPercentage, EBuildingScaleEffect.ShopManagerQualificationImproveRate, -1, 1, 0u, -1, -1, new List<int> { 20 }));
		_dataArray.Add(new BuildingScaleItem(188, 86, 87, EBuildingScaleClass.Invalid, EBuildingScaleType.BonusPercentage, EBuildingScaleEffect.ShopManagerQualificationImproveRate, -1, 2, 0u, -1, -1, new List<int> { 20 }));
		_dataArray.Add(new BuildingScaleItem(189, 86, 87, EBuildingScaleClass.Invalid, EBuildingScaleType.BonusPercentage, EBuildingScaleEffect.ShopManagerQualificationImproveRate, -1, 3, 0u, -1, -1, new List<int> { 20 }));
		_dataArray.Add(new BuildingScaleItem(190, 86, 87, EBuildingScaleClass.Invalid, EBuildingScaleType.BonusPercentage, EBuildingScaleEffect.ShopManagerQualificationImproveRate, -1, 4, 0u, -1, -1, new List<int> { 20 }));
		_dataArray.Add(new BuildingScaleItem(191, 86, 87, EBuildingScaleClass.Invalid, EBuildingScaleType.BonusPercentage, EBuildingScaleEffect.ShopManagerQualificationImproveRate, -1, 5, 0u, -1, -1, new List<int> { 20 }));
		_dataArray.Add(new BuildingScaleItem(192, 86, 87, EBuildingScaleClass.Invalid, EBuildingScaleType.BonusPercentage, EBuildingScaleEffect.ShopManagerQualificationImproveRate, -1, 6, 0u, -1, -1, new List<int> { 20 }));
		_dataArray.Add(new BuildingScaleItem(193, 86, 87, EBuildingScaleClass.Invalid, EBuildingScaleType.BonusPercentage, EBuildingScaleEffect.ShopManagerQualificationImproveRate, -1, 7, 0u, -1, -1, new List<int> { 20 }));
		_dataArray.Add(new BuildingScaleItem(194, 86, 87, EBuildingScaleClass.Invalid, EBuildingScaleType.BonusPercentage, EBuildingScaleEffect.ShopManagerQualificationImproveRate, -1, 8, 0u, -1, -1, new List<int> { 20 }));
		_dataArray.Add(new BuildingScaleItem(195, 86, 87, EBuildingScaleClass.Invalid, EBuildingScaleType.BonusPercentage, EBuildingScaleEffect.ShopManagerQualificationImproveRate, -1, 9, 0u, -1, -1, new List<int> { 20 }));
		_dataArray.Add(new BuildingScaleItem(196, 86, 87, EBuildingScaleClass.Invalid, EBuildingScaleType.BonusPercentage, EBuildingScaleEffect.ShopManagerQualificationImproveRate, -1, 10, 0u, -1, -1, new List<int> { 20 }));
		_dataArray.Add(new BuildingScaleItem(197, 86, 87, EBuildingScaleClass.Invalid, EBuildingScaleType.BonusPercentage, EBuildingScaleEffect.ShopManagerQualificationImproveRate, -1, 11, 0u, -1, -1, new List<int> { 20 }));
		_dataArray.Add(new BuildingScaleItem(198, 86, 87, EBuildingScaleClass.Invalid, EBuildingScaleType.BonusPercentage, EBuildingScaleEffect.ShopManagerQualificationImproveRate, -1, 12, 0u, -1, -1, new List<int> { 20 }));
		_dataArray.Add(new BuildingScaleItem(199, 86, 87, EBuildingScaleClass.Invalid, EBuildingScaleType.BonusPercentage, EBuildingScaleEffect.ShopManagerQualificationImproveRate, -1, 13, 0u, -1, -1, new List<int> { 20 }));
		_dataArray.Add(new BuildingScaleItem(200, 86, 87, EBuildingScaleClass.Invalid, EBuildingScaleType.BonusPercentage, EBuildingScaleEffect.ShopManagerQualificationImproveRate, -1, 14, 0u, -1, -1, new List<int> { 20 }));
		_dataArray.Add(new BuildingScaleItem(201, 86, 87, EBuildingScaleClass.Invalid, EBuildingScaleType.BonusPercentage, EBuildingScaleEffect.ShopManagerQualificationImproveRate, -1, 15, 0u, -1, -1, new List<int> { 20 }));
		_dataArray.Add(new BuildingScaleItem(202, 88, 89, EBuildingScaleClass.Invalid, EBuildingScaleType.Int, EBuildingScaleEffect.LifeSkillAttainment, -1, 0, 0u, -1, -1, new List<int> { 100 }));
		_dataArray.Add(new BuildingScaleItem(203, 88, 89, EBuildingScaleClass.Invalid, EBuildingScaleType.Int, EBuildingScaleEffect.LifeSkillAttainment, -1, 1, 0u, -1, -1, new List<int> { 100 }));
		_dataArray.Add(new BuildingScaleItem(204, 88, 89, EBuildingScaleClass.Invalid, EBuildingScaleType.Int, EBuildingScaleEffect.LifeSkillAttainment, -1, 2, 0u, -1, -1, new List<int> { 100 }));
		_dataArray.Add(new BuildingScaleItem(205, 88, 89, EBuildingScaleClass.Invalid, EBuildingScaleType.Int, EBuildingScaleEffect.LifeSkillAttainment, -1, 3, 0u, -1, -1, new List<int> { 100 }));
		_dataArray.Add(new BuildingScaleItem(206, 88, 89, EBuildingScaleClass.Invalid, EBuildingScaleType.Int, EBuildingScaleEffect.LifeSkillAttainment, -1, 4, 0u, -1, -1, new List<int> { 100 }));
		_dataArray.Add(new BuildingScaleItem(207, 88, 89, EBuildingScaleClass.Invalid, EBuildingScaleType.Int, EBuildingScaleEffect.LifeSkillAttainment, -1, 5, 0u, -1, -1, new List<int> { 100 }));
		_dataArray.Add(new BuildingScaleItem(208, 88, 89, EBuildingScaleClass.Invalid, EBuildingScaleType.Int, EBuildingScaleEffect.LifeSkillAttainment, -1, 6, 0u, -1, -1, new List<int> { 100 }));
		_dataArray.Add(new BuildingScaleItem(209, 88, 89, EBuildingScaleClass.Invalid, EBuildingScaleType.Int, EBuildingScaleEffect.LifeSkillAttainment, -1, 7, 0u, -1, -1, new List<int> { 100 }));
		_dataArray.Add(new BuildingScaleItem(210, 88, 89, EBuildingScaleClass.Invalid, EBuildingScaleType.Int, EBuildingScaleEffect.LifeSkillAttainment, -1, 8, 0u, -1, -1, new List<int> { 100 }));
		_dataArray.Add(new BuildingScaleItem(211, 88, 89, EBuildingScaleClass.Invalid, EBuildingScaleType.Int, EBuildingScaleEffect.LifeSkillAttainment, -1, 9, 0u, -1, -1, new List<int> { 100 }));
		_dataArray.Add(new BuildingScaleItem(212, 88, 89, EBuildingScaleClass.Invalid, EBuildingScaleType.Int, EBuildingScaleEffect.LifeSkillAttainment, -1, 10, 0u, -1, -1, new List<int> { 100 }));
		_dataArray.Add(new BuildingScaleItem(213, 88, 89, EBuildingScaleClass.Invalid, EBuildingScaleType.Int, EBuildingScaleEffect.LifeSkillAttainment, -1, 11, 0u, -1, -1, new List<int> { 100 }));
		_dataArray.Add(new BuildingScaleItem(214, 88, 89, EBuildingScaleClass.Invalid, EBuildingScaleType.Int, EBuildingScaleEffect.LifeSkillAttainment, -1, 12, 0u, -1, -1, new List<int> { 100 }));
		_dataArray.Add(new BuildingScaleItem(215, 88, 89, EBuildingScaleClass.Invalid, EBuildingScaleType.Int, EBuildingScaleEffect.LifeSkillAttainment, -1, 13, 0u, -1, -1, new List<int> { 100 }));
		_dataArray.Add(new BuildingScaleItem(216, 88, 89, EBuildingScaleClass.Invalid, EBuildingScaleType.Int, EBuildingScaleEffect.LifeSkillAttainment, -1, 14, 0u, -1, -1, new List<int> { 100 }));
		_dataArray.Add(new BuildingScaleItem(217, 88, 89, EBuildingScaleClass.Invalid, EBuildingScaleType.Int, EBuildingScaleEffect.LifeSkillAttainment, -1, 15, 0u, -1, -1, new List<int> { 100 }));
		_dataArray.Add(new BuildingScaleItem(218, 90, 91, EBuildingScaleClass.Invalid, EBuildingScaleType.ReducePercentage, EBuildingScaleEffect.MakeItemAttainmentRequirementReduction, -1, 6, 0u, -1, -1, new List<int> { 20 }));
		_dataArray.Add(new BuildingScaleItem(219, 92, 91, EBuildingScaleClass.Invalid, EBuildingScaleType.ReducePercentage, EBuildingScaleEffect.MakeItemAttainmentRequirementReduction, -1, 7, 0u, -1, -1, new List<int> { 20 }));
		_dataArray.Add(new BuildingScaleItem(220, 93, 91, EBuildingScaleClass.Invalid, EBuildingScaleType.ReducePercentage, EBuildingScaleEffect.MakeItemAttainmentRequirementReduction, -1, 8, 0u, -1, -1, new List<int> { 20 }));
		_dataArray.Add(new BuildingScaleItem(221, 94, 91, EBuildingScaleClass.Invalid, EBuildingScaleType.ReducePercentage, EBuildingScaleEffect.MakeItemAttainmentRequirementReduction, -1, 9, 0u, -1, -1, new List<int> { 20 }));
		_dataArray.Add(new BuildingScaleItem(222, 95, 91, EBuildingScaleClass.Invalid, EBuildingScaleType.ReducePercentage, EBuildingScaleEffect.MakeItemAttainmentRequirementReduction, -1, 10, 0u, -1, -1, new List<int> { 20 }));
		_dataArray.Add(new BuildingScaleItem(223, 96, 91, EBuildingScaleClass.Invalid, EBuildingScaleType.ReducePercentage, EBuildingScaleEffect.MakeItemAttainmentRequirementReduction, -1, 11, 0u, -1, -1, new List<int> { 20 }));
		_dataArray.Add(new BuildingScaleItem(224, 97, 91, EBuildingScaleClass.Invalid, EBuildingScaleType.ReducePercentage, EBuildingScaleEffect.MakeItemAttainmentRequirementReduction, -1, 14, 0u, -1, -1, new List<int> { 20 }));
		_dataArray.Add(new BuildingScaleItem(225, 98, 99, EBuildingScaleClass.Invalid, EBuildingScaleType.Int, EBuildingScaleEffect.QiDisorderRecovery, -1, -1, 0u, -1, 0, new List<int>()));
		_dataArray.Add(new BuildingScaleItem(226, 100, 101, EBuildingScaleClass.Invalid, EBuildingScaleType.Int, EBuildingScaleEffect.TaiwuGroupMaxCount, -1, -1, 0u, -1, 1, new List<int>()));
		_dataArray.Add(new BuildingScaleItem(227, 102, 103, EBuildingScaleClass.Invalid, EBuildingScaleType.Int, EBuildingScaleEffect.InitialFavorability, -1, -1, 0u, -1, 3, new List<int>()));
		_dataArray.Add(new BuildingScaleItem(228, 104, 105, EBuildingScaleClass.Invalid, EBuildingScaleType.BonusPercentage, EBuildingScaleEffect.LegacyPointBonusFactor, -1, -1, 0u, -1, 4, new List<int>()));
		_dataArray.Add(new BuildingScaleItem(229, 106, 107, EBuildingScaleClass.Invalid, EBuildingScaleType.Int, EBuildingScaleEffect.HealthRecovery, -1, -1, 0u, -1, 6, new List<int>()));
		_dataArray.Add(new BuildingScaleItem(230, 108, 109, EBuildingScaleClass.Invalid, EBuildingScaleType.ReducePercentage, EBuildingScaleEffect.ReadingStrategyCost, -1, -1, 0u, -1, 5, new List<int>()));
		_dataArray.Add(new BuildingScaleItem(231, 56, 57, EBuildingScaleClass.Slot, EBuildingScaleType.Int, EBuildingScaleEffect.Invalid, -1, -1, 0u, -1, -1, new List<int> { 10 }));
		_dataArray.Add(new BuildingScaleItem(232, 56, 57, EBuildingScaleClass.Slot, EBuildingScaleType.Int, EBuildingScaleEffect.Invalid, -1, -1, 0u, -1, -1, new List<int> { 10 }));
		_dataArray.Add(new BuildingScaleItem(233, 56, 57, EBuildingScaleClass.Slot, EBuildingScaleType.Int, EBuildingScaleEffect.Invalid, -1, -1, 0u, -1, -1, new List<int> { 10 }));
		_dataArray.Add(new BuildingScaleItem(234, 56, 57, EBuildingScaleClass.Slot, EBuildingScaleType.Int, EBuildingScaleEffect.Invalid, -1, -1, 0u, -1, -1, new List<int> { 10 }));
		_dataArray.Add(new BuildingScaleItem(235, 56, 57, EBuildingScaleClass.Slot, EBuildingScaleType.Int, EBuildingScaleEffect.Invalid, -1, -1, 0u, -1, -1, new List<int> { 10 }));
		_dataArray.Add(new BuildingScaleItem(236, 56, 57, EBuildingScaleClass.Slot, EBuildingScaleType.Int, EBuildingScaleEffect.Invalid, -1, -1, 0u, -1, -1, new List<int> { 10 }));
		_dataArray.Add(new BuildingScaleItem(237, 56, 57, EBuildingScaleClass.Slot, EBuildingScaleType.Int, EBuildingScaleEffect.Invalid, -1, -1, 0u, -1, -1, new List<int> { 10 }));
		_dataArray.Add(new BuildingScaleItem(238, 56, 57, EBuildingScaleClass.Slot, EBuildingScaleType.Int, EBuildingScaleEffect.Invalid, -1, -1, 0u, -1, -1, new List<int> { 10 }));
		_dataArray.Add(new BuildingScaleItem(239, 56, 57, EBuildingScaleClass.Slot, EBuildingScaleType.Int, EBuildingScaleEffect.Invalid, -1, -1, 0u, -1, -1, new List<int> { 10 }));
	}

	private void CreateItems4()
	{
		_dataArray.Add(new BuildingScaleItem(240, 56, 57, EBuildingScaleClass.Slot, EBuildingScaleType.Int, EBuildingScaleEffect.Invalid, -1, -1, 0u, -1, -1, new List<int> { 10 }));
		_dataArray.Add(new BuildingScaleItem(241, 56, 57, EBuildingScaleClass.Slot, EBuildingScaleType.Int, EBuildingScaleEffect.Invalid, -1, -1, 0u, -1, -1, new List<int> { 10 }));
		_dataArray.Add(new BuildingScaleItem(242, 56, 57, EBuildingScaleClass.Slot, EBuildingScaleType.Int, EBuildingScaleEffect.Invalid, -1, -1, 0u, -1, -1, new List<int> { 10 }));
		_dataArray.Add(new BuildingScaleItem(243, 56, 57, EBuildingScaleClass.Slot, EBuildingScaleType.Int, EBuildingScaleEffect.Invalid, -1, -1, 0u, -1, -1, new List<int> { 10 }));
		_dataArray.Add(new BuildingScaleItem(244, 56, 57, EBuildingScaleClass.Slot, EBuildingScaleType.Int, EBuildingScaleEffect.Invalid, -1, -1, 0u, -1, -1, new List<int> { 10 }));
		_dataArray.Add(new BuildingScaleItem(245, 56, 57, EBuildingScaleClass.Slot, EBuildingScaleType.Int, EBuildingScaleEffect.Invalid, -1, -1, 0u, -1, -1, new List<int> { 20 }));
		_dataArray.Add(new BuildingScaleItem(246, 56, 57, EBuildingScaleClass.Slot, EBuildingScaleType.Int, EBuildingScaleEffect.Invalid, -1, -1, 0u, -1, -1, new List<int> { 20 }));
		_dataArray.Add(new BuildingScaleItem(247, 56, 57, EBuildingScaleClass.Slot, EBuildingScaleType.Int, EBuildingScaleEffect.Invalid, -1, -1, 0u, -1, -1, new List<int> { 20 }));
	}

	public override void Init()
	{
		base.Init();
		_dataArray = new List<BuildingScaleItem>(248);
		CreateItems0();
		CreateItems1();
		CreateItems2();
		CreateItems3();
		CreateItems4();
	}
}
