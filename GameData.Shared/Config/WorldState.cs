using System;
using System.Collections.Generic;
using Config.Common;

namespace Config;

[Serializable]
public class WorldState : ConfigData<WorldStateItem, sbyte>
{
	public static class DefKey
	{
		public const sbyte XiangshuLevel01 = 46;

		public const sbyte XiangshuLevel02 = 47;

		public const sbyte XiangshuLevel0 = 0;

		public const sbyte XiangshuLevel1 = 1;

		public const sbyte XiangshuLevel2 = 2;

		public const sbyte XiangshuLevel3 = 3;

		public const sbyte XiangshuLevel4 = 4;

		public const sbyte XiangshuLevel5 = 5;

		public const sbyte XiangshuLevel6 = 6;

		public const sbyte XiangshuLevel7 = 48;

		public const sbyte EquipmentOverload = 7;

		public const sbyte InventoryOverload = 8;

		public const sbyte WarehouseOverload = 9;

		public const sbyte ResourceOverload = 10;

		public const sbyte OuterInjury = 11;

		public const sbyte InnerInjury = 12;

		public const sbyte DisorderOfQi = 13;

		public const sbyte Poisons = 14;

		public const sbyte XiangshuPartiallyInfected = 15;

		public const sbyte XiangshuCompletelyInfected = 16;

		public const sbyte XiangshuAvatarAwake = 17;

		public const sbyte XiangshuAvatarAttack = 18;

		public const sbyte MartialArtTournamentPrepare = 19;

		public const sbyte MartialArtTournamentWait = 20;

		public const sbyte MartialArtTournamentOpen = 21;

		public const sbyte SectMainStoryShaolin = 22;

		public const sbyte SectMainStoryEmei = 23;

		public const sbyte SectMainStoryBaihua = 24;

		public const sbyte SectMainStoryWudang = 25;

		public const sbyte SectMainStoryYuanshan = 26;

		public const sbyte SectMainStoryShixiang = 27;

		public const sbyte SectMainStoryRanshan = 28;

		public const sbyte SectMainStoryXuannv = 29;

		public const sbyte SectMainStoryZhujian = 30;

		public const sbyte SectMainStoryKongsang = 31;

		public const sbyte SectMainStoryJingang = 32;

		public const sbyte SectMainStoryWuxian = 33;

		public const sbyte SectMainStoryJieqing = 34;

		public const sbyte SectMainStoryFulong = 35;

		public const sbyte SectMainStoryXuehou = 36;

		public const sbyte TeammateSeverelyInjured = 37;

		public const sbyte ChangeWorldCreation = 38;

		public const sbyte FiveLoongLei = 39;

		public const sbyte FiveLoongHong = 40;

		public const sbyte FiveLoongFeng = 41;

		public const sbyte FiveLoongYan = 42;

		public const sbyte FiveLoongSha = 43;

		public const sbyte TheLandOfFire = 44;

		public const sbyte ThunderStrikeTrial = 45;

		public const sbyte TaiwuWanted = 49;

		public const sbyte WardOffXiangshu = 50;

		public const sbyte TearmateDying = 51;

		public const sbyte HomelessVillager = 52;

		public const sbyte NeiliConflicting = 53;
	}

	public static class DefValue
	{
		public static WorldStateItem XiangshuLevel01 => Instance[(sbyte)46];

		public static WorldStateItem XiangshuLevel02 => Instance[(sbyte)47];

		public static WorldStateItem XiangshuLevel0 => Instance[(sbyte)0];

		public static WorldStateItem XiangshuLevel1 => Instance[(sbyte)1];

		public static WorldStateItem XiangshuLevel2 => Instance[(sbyte)2];

		public static WorldStateItem XiangshuLevel3 => Instance[(sbyte)3];

		public static WorldStateItem XiangshuLevel4 => Instance[(sbyte)4];

		public static WorldStateItem XiangshuLevel5 => Instance[(sbyte)5];

		public static WorldStateItem XiangshuLevel6 => Instance[(sbyte)6];

		public static WorldStateItem XiangshuLevel7 => Instance[(sbyte)48];

		public static WorldStateItem EquipmentOverload => Instance[(sbyte)7];

		public static WorldStateItem InventoryOverload => Instance[(sbyte)8];

		public static WorldStateItem WarehouseOverload => Instance[(sbyte)9];

		public static WorldStateItem ResourceOverload => Instance[(sbyte)10];

		public static WorldStateItem OuterInjury => Instance[(sbyte)11];

		public static WorldStateItem InnerInjury => Instance[(sbyte)12];

		public static WorldStateItem DisorderOfQi => Instance[(sbyte)13];

		public static WorldStateItem Poisons => Instance[(sbyte)14];

		public static WorldStateItem XiangshuPartiallyInfected => Instance[(sbyte)15];

		public static WorldStateItem XiangshuCompletelyInfected => Instance[(sbyte)16];

		public static WorldStateItem XiangshuAvatarAwake => Instance[(sbyte)17];

		public static WorldStateItem XiangshuAvatarAttack => Instance[(sbyte)18];

		public static WorldStateItem MartialArtTournamentPrepare => Instance[(sbyte)19];

		public static WorldStateItem MartialArtTournamentWait => Instance[(sbyte)20];

		public static WorldStateItem MartialArtTournamentOpen => Instance[(sbyte)21];

		public static WorldStateItem SectMainStoryShaolin => Instance[(sbyte)22];

		public static WorldStateItem SectMainStoryEmei => Instance[(sbyte)23];

		public static WorldStateItem SectMainStoryBaihua => Instance[(sbyte)24];

		public static WorldStateItem SectMainStoryWudang => Instance[(sbyte)25];

		public static WorldStateItem SectMainStoryYuanshan => Instance[(sbyte)26];

		public static WorldStateItem SectMainStoryShixiang => Instance[(sbyte)27];

		public static WorldStateItem SectMainStoryRanshan => Instance[(sbyte)28];

		public static WorldStateItem SectMainStoryXuannv => Instance[(sbyte)29];

		public static WorldStateItem SectMainStoryZhujian => Instance[(sbyte)30];

		public static WorldStateItem SectMainStoryKongsang => Instance[(sbyte)31];

		public static WorldStateItem SectMainStoryJingang => Instance[(sbyte)32];

		public static WorldStateItem SectMainStoryWuxian => Instance[(sbyte)33];

		public static WorldStateItem SectMainStoryJieqing => Instance[(sbyte)34];

		public static WorldStateItem SectMainStoryFulong => Instance[(sbyte)35];

		public static WorldStateItem SectMainStoryXuehou => Instance[(sbyte)36];

		public static WorldStateItem TeammateSeverelyInjured => Instance[(sbyte)37];

		public static WorldStateItem ChangeWorldCreation => Instance[(sbyte)38];

		public static WorldStateItem FiveLoongLei => Instance[(sbyte)39];

		public static WorldStateItem FiveLoongHong => Instance[(sbyte)40];

		public static WorldStateItem FiveLoongFeng => Instance[(sbyte)41];

		public static WorldStateItem FiveLoongYan => Instance[(sbyte)42];

		public static WorldStateItem FiveLoongSha => Instance[(sbyte)43];

		public static WorldStateItem TheLandOfFire => Instance[(sbyte)44];

		public static WorldStateItem ThunderStrikeTrial => Instance[(sbyte)45];

		public static WorldStateItem TaiwuWanted => Instance[(sbyte)49];

		public static WorldStateItem WardOffXiangshu => Instance[(sbyte)50];

		public static WorldStateItem TearmateDying => Instance[(sbyte)51];

		public static WorldStateItem HomelessVillager => Instance[(sbyte)52];

		public static WorldStateItem NeiliConflicting => Instance[(sbyte)53];
	}

	public static WorldState Instance = new WorldState();

	private readonly HashSet<string> RequiredFields = new HashSet<string> { "Sect", "TriggerArea", "MonthlyEvents", "TemplateId", "Name", "Desc", "Icon" };

	internal override int ToInt(sbyte value)
	{
		return value;
	}

	internal override sbyte ToTemplateId(int value)
	{
		return (sbyte)value;
	}

	private void CreateItems0()
	{
		_dataArray.Add(new WorldStateItem(0, 4, 5, "worldstate_icon_22", new int[0], -1, -1, null));
		_dataArray.Add(new WorldStateItem(1, 6, 7, "worldstate_icon_21", new int[0], -1, -1, null));
		_dataArray.Add(new WorldStateItem(2, 8, 9, "worldstate_icon_20", new int[0], -1, -1, null));
		_dataArray.Add(new WorldStateItem(3, 10, 11, "worldstate_icon_19", new int[0], -1, -1, null));
		_dataArray.Add(new WorldStateItem(4, 12, 13, "worldstate_icon_23", new int[0], -1, -1, null));
		_dataArray.Add(new WorldStateItem(5, 14, 15, "worldstate_icon_24", new int[0], -1, -1, null));
		_dataArray.Add(new WorldStateItem(6, 16, 17, "worldstate_icon_18", new int[0], -1, -1, null));
		_dataArray.Add(new WorldStateItem(7, 20, 21, null, new int[0], -1, -1, null));
		_dataArray.Add(new WorldStateItem(8, 22, 23, "worldstate_icon_7", new int[0], -1, -1, null));
		_dataArray.Add(new WorldStateItem(9, 24, 25, "worldstate_icon_5", new int[0], -1, -1, null));
		_dataArray.Add(new WorldStateItem(10, 26, 27, "worldstate_icon_6", new int[0], -1, -1, null));
		_dataArray.Add(new WorldStateItem(11, 28, 29, "worldstate_icon_13", new int[0], -1, -1, null));
		_dataArray.Add(new WorldStateItem(12, 30, 31, "worldstate_icon_12", new int[0], -1, -1, null));
		_dataArray.Add(new WorldStateItem(13, 32, 33, "worldstate_icon_8", new int[0], -1, -1, null));
		_dataArray.Add(new WorldStateItem(14, 34, 35, "worldstate_icon_9", new int[0], -1, -1, null));
		_dataArray.Add(new WorldStateItem(15, 36, 37, "worldstate_icon_17", new int[0], -1, -1, null));
		_dataArray.Add(new WorldStateItem(16, 38, 39, "worldstate_icon_16", new int[0], -1, -1, null));
		_dataArray.Add(new WorldStateItem(17, 40, 41, "worldstate_icon_15", new int[0], -1, -1, null));
		_dataArray.Add(new WorldStateItem(18, 42, 43, "worldstate_icon_14", new int[0], -1, -1, null));
		_dataArray.Add(new WorldStateItem(19, 44, 45, "worldstate_icon_3", new int[0], -1, -1, null));
		_dataArray.Add(new WorldStateItem(20, 46, 47, "worldstate_icon_3", new int[0], -1, -1, null));
		_dataArray.Add(new WorldStateItem(21, 48, 49, "worldstate_icon_3", new int[0], -1, -1, null));
		_dataArray.Add(new WorldStateItem(22, 50, 51, "worldstate_icon_77", new int[2] { 52, 53 }, 1, 16, null));
		_dataArray.Add(new WorldStateItem(23, 54, 55, "worldstate_icon_67", new int[2] { 56, 57 }, 2, 17, null));
		_dataArray.Add(new WorldStateItem(24, 58, 59, "worldstate_icon_70", new int[2] { 60, 61 }, 3, 18, null));
		_dataArray.Add(new WorldStateItem(25, 62, 63, "worldstate_icon_73", new int[2] { 64, 65 }, 4, 19, null));
		_dataArray.Add(new WorldStateItem(26, 66, 67, "worldstate_icon_82", new int[3] { 68, 69, 70 }, 5, 20, null));
		_dataArray.Add(new WorldStateItem(27, 71, 72, "worldstate_icon_78", new int[2] { 73, 74 }, 6, -1, null));
		_dataArray.Add(new WorldStateItem(28, 75, 76, "worldstate_icon_69", new int[3] { 77, 78, 79 }, 7, -1, null));
		_dataArray.Add(new WorldStateItem(29, 80, 81, "worldstate_icon_75", new int[2] { 82, 83 }, 8, 23, null));
		_dataArray.Add(new WorldStateItem(30, 84, 85, "worldstate_icon_71", new int[2] { 86, 87 }, 9, -1, null));
		_dataArray.Add(new WorldStateItem(31, 88, 89, "worldstate_icon_72", new int[2] { 90, 91 }, 10, 25, null));
		_dataArray.Add(new WorldStateItem(32, 92, 93, "worldstate_icon_79", new int[2] { 94, 95 }, 11, -1, null));
		_dataArray.Add(new WorldStateItem(33, 96, 97, "worldstate_icon_76", new int[2] { 98, 99 }, 12, 27, null));
		_dataArray.Add(new WorldStateItem(34, 100, 101, "worldstate_icon_26", new int[0], 13, -1, null));
		_dataArray.Add(new WorldStateItem(35, 102, 103, "worldstate_icon_74", new int[2] { 104, 105 }, 14, 29, null));
		_dataArray.Add(new WorldStateItem(36, 106, 107, "worldstate_icon_68", new int[1] { 108 }, 15, 15, null));
		_dataArray.Add(new WorldStateItem(37, 109, 110, "worldstate_icon_27", new int[0], -1, -1, null));
		_dataArray.Add(new WorldStateItem(38, 111, 112, "worldstate_icon_28", new int[0], -1, -1, null));
		_dataArray.Add(new WorldStateItem(39, 113, 114, "worldstate_icon_29", new int[0], -1, -1, null));
		_dataArray.Add(new WorldStateItem(40, 115, 116, "worldstate_icon_30", new int[0], -1, -1, null));
		_dataArray.Add(new WorldStateItem(41, 117, 118, "worldstate_icon_32", new int[0], -1, -1, null));
		_dataArray.Add(new WorldStateItem(42, 119, 120, "worldstate_icon_31", new int[0], -1, -1, null));
		_dataArray.Add(new WorldStateItem(43, 121, 122, "worldstate_icon_33", new int[0], -1, -1, null));
		_dataArray.Add(new WorldStateItem(44, 123, 124, "worldstate_icon_34", new int[0], -1, -1, null));
		_dataArray.Add(new WorldStateItem(45, 125, 126, "worldstate_icon_37", new int[0], -1, -1, null));
		_dataArray.Add(new WorldStateItem(46, 0, 1, "worldstate_icon_63", new int[0], -1, -1, null));
		_dataArray.Add(new WorldStateItem(47, 2, 3, "worldstate_icon_64", new int[0], -1, -1, null));
		_dataArray.Add(new WorldStateItem(48, 18, 19, "worldstate_icon_65", new int[0], -1, -1, null));
		_dataArray.Add(new WorldStateItem(49, 127, 128, "worldstate_icon_66", new int[0], -1, -1, null));
		_dataArray.Add(new WorldStateItem(50, 129, 130, "worldstate_icon_38", new int[0], -1, -1, null));
		_dataArray.Add(new WorldStateItem(51, 131, 132, "worldstate_icon_36", new int[0], -1, -1, null));
		_dataArray.Add(new WorldStateItem(52, 133, 134, "worldstate_icon_81", new int[0], -1, -1, null));
		_dataArray.Add(new WorldStateItem(53, 135, 136, "worldstate_icon_80", new int[0], -1, -1, null));
	}

	public override void Init()
	{
		base.Init();
		_dataArray = new List<WorldStateItem>(54);
		CreateItems0();
	}
}
