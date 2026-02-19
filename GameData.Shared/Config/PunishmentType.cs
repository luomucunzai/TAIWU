using System;
using System.Collections.Generic;
using Config.Common;
using GameData.Utilities;

namespace Config;

[Serializable]
public class PunishmentType : ConfigData<PunishmentTypeItem, short>
{
	public static class DefKey
	{
		public const short Default = 43;

		public const short PunishedBecauseOfSpouse = 20;

		public const short XiangshuCompletelyInfected = 39;

		public const short EscapeFromPrison = 40;

		public const short EnemySects = 41;

		public const short EnemyRelationship = 42;
	}

	public static class DefValue
	{
		public static PunishmentTypeItem Default => Instance[(short)43];

		public static PunishmentTypeItem PunishedBecauseOfSpouse => Instance[(short)20];

		public static PunishmentTypeItem XiangshuCompletelyInfected => Instance[(short)39];

		public static PunishmentTypeItem EscapeFromPrison => Instance[(short)40];

		public static PunishmentTypeItem EnemySects => Instance[(short)41];

		public static PunishmentTypeItem EnemyRelationship => Instance[(short)42];
	}

	public static PunishmentType Instance = new PunishmentType();

	private readonly HashSet<string> RequiredFields = new HashSet<string> { "Severity", "SectPunishmentSeverities", "CivilianPunishmentSeverities", "TemplateId", "Name", "ShortName", "DisplayType", "PunishmentDesc" };

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
		_dataArray.Add(new PunishmentTypeItem(0, 3, 4, EPunishmentTypeDisplayType.Criminal, -1, 5, new List<ShortPair>
		{
			new ShortPair(1, 2),
			new ShortPair(2, 1),
			new ShortPair(3, 1),
			new ShortPair(4, 2),
			new ShortPair(5, 1)
		}, new List<ShortPair>
		{
			new ShortPair(1, 3),
			new ShortPair(2, 3),
			new ShortPair(3, 3),
			new ShortPair(4, 3),
			new ShortPair(5, 3),
			new ShortPair(6, 3),
			new ShortPair(7, 3),
			new ShortPair(8, 3),
			new ShortPair(9, 3),
			new ShortPair(10, 3),
			new ShortPair(11, 3),
			new ShortPair(12, 3),
			new ShortPair(13, 3),
			new ShortPair(14, 3),
			new ShortPair(15, 3)
		}, 4000, 0u));
		_dataArray.Add(new PunishmentTypeItem(1, 6, 7, EPunishmentTypeDisplayType.Criminal, -1, 8, new List<ShortPair>
		{
			new ShortPair(7, 0),
			new ShortPair(8, 0),
			new ShortPair(9, 0)
		}, new List<ShortPair>
		{
			new ShortPair(1, 3),
			new ShortPair(2, 3),
			new ShortPair(3, 3),
			new ShortPair(4, 3),
			new ShortPair(5, 3),
			new ShortPair(6, 3),
			new ShortPair(7, 3),
			new ShortPair(8, 3),
			new ShortPair(9, 3),
			new ShortPair(10, 3),
			new ShortPair(11, 3),
			new ShortPair(12, 3),
			new ShortPair(13, 3),
			new ShortPair(14, 3),
			new ShortPair(15, 3)
		}, 4000, 0u));
		_dataArray.Add(new PunishmentTypeItem(2, 9, 10, EPunishmentTypeDisplayType.Criminal, -1, 11, new List<ShortPair>
		{
			new ShortPair(1, 2),
			new ShortPair(2, 1),
			new ShortPair(3, 1),
			new ShortPair(4, 2),
			new ShortPair(5, 1),
			new ShortPair(7, 0),
			new ShortPair(8, 0),
			new ShortPair(9, 0)
		}, new List<ShortPair>
		{
			new ShortPair(1, 3),
			new ShortPair(2, 3),
			new ShortPair(3, 3),
			new ShortPair(4, 3),
			new ShortPair(5, 3),
			new ShortPair(6, 3),
			new ShortPair(7, 3),
			new ShortPair(8, 3),
			new ShortPair(9, 3),
			new ShortPair(10, 3),
			new ShortPair(11, 3),
			new ShortPair(12, 3),
			new ShortPair(13, 3),
			new ShortPair(14, 3),
			new ShortPair(15, 3)
		}, 4000, 0u));
		_dataArray.Add(new PunishmentTypeItem(3, 12, 13, EPunishmentTypeDisplayType.Criminal, -1, 14, new List<ShortPair>
		{
			new ShortPair(1, 0),
			new ShortPair(4, 0)
		}, new List<ShortPair>
		{
			new ShortPair(1, 2),
			new ShortPair(2, 2),
			new ShortPair(3, 2),
			new ShortPair(4, 2),
			new ShortPair(5, 2),
			new ShortPair(6, 2),
			new ShortPair(7, 2),
			new ShortPair(8, 2),
			new ShortPair(9, 2),
			new ShortPair(10, 2),
			new ShortPair(11, 2),
			new ShortPair(12, 2),
			new ShortPair(13, 2),
			new ShortPair(14, 2),
			new ShortPair(15, 2)
		}, 3000, 0u));
		_dataArray.Add(new PunishmentTypeItem(4, 15, 16, EPunishmentTypeDisplayType.Criminal, -1, 17, new List<ShortPair>(), new List<ShortPair>
		{
			new ShortPair(1, 2),
			new ShortPair(2, 2),
			new ShortPair(3, 2),
			new ShortPair(4, 2),
			new ShortPair(5, 2),
			new ShortPair(6, 2),
			new ShortPair(7, 2),
			new ShortPair(8, 2),
			new ShortPair(9, 2),
			new ShortPair(10, 2),
			new ShortPair(11, 2),
			new ShortPair(12, 2),
			new ShortPair(13, 2),
			new ShortPair(14, 2),
			new ShortPair(15, 2)
		}, 3000, 0u));
		_dataArray.Add(new PunishmentTypeItem(5, 18, 19, EPunishmentTypeDisplayType.Criminal, -1, 20, new List<ShortPair>
		{
			new ShortPair(1, 0),
			new ShortPair(4, 0)
		}, new List<ShortPair>
		{
			new ShortPair(1, 2),
			new ShortPair(2, 2),
			new ShortPair(3, 2),
			new ShortPair(4, 2),
			new ShortPair(5, 2),
			new ShortPair(6, 2),
			new ShortPair(7, 2),
			new ShortPair(8, 2),
			new ShortPair(9, 2),
			new ShortPair(10, 2),
			new ShortPair(11, 2),
			new ShortPair(12, 2),
			new ShortPair(13, 2),
			new ShortPair(14, 2),
			new ShortPair(15, 2)
		}, 3000, 0u));
		_dataArray.Add(new PunishmentTypeItem(6, 21, 21, EPunishmentTypeDisplayType.Criminal, -1, 22, new List<ShortPair>(), new List<ShortPair>(), 4000, 0u));
		_dataArray.Add(new PunishmentTypeItem(7, 23, 23, EPunishmentTypeDisplayType.Criminal, -1, 24, new List<ShortPair>(), new List<ShortPair>(), 3000, 0u));
		_dataArray.Add(new PunishmentTypeItem(8, 25, 25, EPunishmentTypeDisplayType.Criminal, -1, 26, new List<ShortPair>(), new List<ShortPair>(), 3000, 0u));
		_dataArray.Add(new PunishmentTypeItem(9, 27, 27, EPunishmentTypeDisplayType.Criminal, -1, 28, new List<ShortPair>
		{
			new ShortPair(1, 0)
		}, new List<ShortPair>
		{
			new ShortPair(1, 0),
			new ShortPair(2, 0),
			new ShortPair(3, 0),
			new ShortPair(4, 0),
			new ShortPair(5, 0),
			new ShortPair(6, 0),
			new ShortPair(7, 0),
			new ShortPair(8, 0),
			new ShortPair(9, 0),
			new ShortPair(10, 0),
			new ShortPair(11, 0),
			new ShortPair(12, 0),
			new ShortPair(13, 0),
			new ShortPair(14, 0),
			new ShortPair(15, 0)
		}, 1000, 0u));
		_dataArray.Add(new PunishmentTypeItem(10, 29, 29, EPunishmentTypeDisplayType.Criminal, -1, 30, new List<ShortPair>
		{
			new ShortPair(1, 0),
			new ShortPair(4, 0),
			new ShortPair(5, 0),
			new ShortPair(8, 0)
		}, new List<ShortPair>
		{
			new ShortPair(1, 0),
			new ShortPair(2, 0),
			new ShortPair(3, 0),
			new ShortPair(4, 0),
			new ShortPair(5, 0),
			new ShortPair(6, 0),
			new ShortPair(7, 0),
			new ShortPair(8, 0),
			new ShortPair(9, 0),
			new ShortPair(10, 0),
			new ShortPair(11, 0),
			new ShortPair(12, 0),
			new ShortPair(13, 0),
			new ShortPair(14, 0),
			new ShortPair(15, 0)
		}, 1000, 0u));
		_dataArray.Add(new PunishmentTypeItem(11, 31, 31, EPunishmentTypeDisplayType.Criminal, -1, 32, new List<ShortPair>
		{
			new ShortPair(1, 1),
			new ShortPair(2, 0),
			new ShortPair(3, 0),
			new ShortPair(4, 0),
			new ShortPair(5, 0),
			new ShortPair(8, 0),
			new ShortPair(9, 0)
		}, new List<ShortPair>
		{
			new ShortPair(1, 1),
			new ShortPair(2, 1),
			new ShortPair(3, 1),
			new ShortPair(4, 1),
			new ShortPair(5, 1),
			new ShortPair(6, 1),
			new ShortPair(7, 1),
			new ShortPair(8, 1),
			new ShortPair(9, 1),
			new ShortPair(10, 1),
			new ShortPair(11, 1),
			new ShortPair(12, 1),
			new ShortPair(13, 1),
			new ShortPair(14, 1),
			new ShortPair(15, 1)
		}, 2000, 0u));
		_dataArray.Add(new PunishmentTypeItem(12, 33, 33, EPunishmentTypeDisplayType.Criminal, -1, 34, new List<ShortPair>
		{
			new ShortPair(1, 1),
			new ShortPair(2, 0),
			new ShortPair(3, 0),
			new ShortPair(4, 1),
			new ShortPair(5, 1),
			new ShortPair(8, 1),
			new ShortPair(9, 0)
		}, new List<ShortPair>
		{
			new ShortPair(1, 1),
			new ShortPair(2, 1),
			new ShortPair(3, 1),
			new ShortPair(4, 1),
			new ShortPair(5, 1),
			new ShortPair(6, 1),
			new ShortPair(7, 1),
			new ShortPair(8, 1),
			new ShortPair(9, 1),
			new ShortPair(10, 1),
			new ShortPair(11, 1),
			new ShortPair(12, 1),
			new ShortPair(13, 1),
			new ShortPair(14, 1),
			new ShortPair(15, 1)
		}, 2000, 0u));
		_dataArray.Add(new PunishmentTypeItem(13, 35, 35, EPunishmentTypeDisplayType.Criminal, -1, 36, new List<ShortPair>
		{
			new ShortPair(1, 0),
			new ShortPair(4, 0)
		}, new List<ShortPair>
		{
			new ShortPair(1, 2),
			new ShortPair(2, 2),
			new ShortPair(3, 2),
			new ShortPair(4, 2),
			new ShortPair(5, 2),
			new ShortPair(6, 2),
			new ShortPair(7, 2),
			new ShortPair(8, 2),
			new ShortPair(9, 2),
			new ShortPair(10, 2),
			new ShortPair(11, 2),
			new ShortPair(12, 2),
			new ShortPair(13, 2),
			new ShortPair(14, 2),
			new ShortPair(15, 2)
		}, 3000, 0u));
		_dataArray.Add(new PunishmentTypeItem(14, 37, 37, EPunishmentTypeDisplayType.Criminal, -1, 38, new List<ShortPair>
		{
			new ShortPair(1, 0),
			new ShortPair(4, 0)
		}, new List<ShortPair>
		{
			new ShortPair(1, 2),
			new ShortPair(2, 2),
			new ShortPair(3, 2),
			new ShortPair(4, 2),
			new ShortPair(5, 2),
			new ShortPair(6, 2),
			new ShortPair(7, 2),
			new ShortPair(8, 2),
			new ShortPair(9, 2),
			new ShortPair(10, 2),
			new ShortPair(11, 2),
			new ShortPair(12, 2),
			new ShortPair(13, 2),
			new ShortPair(14, 2),
			new ShortPair(15, 2)
		}, 3000, 0u));
		_dataArray.Add(new PunishmentTypeItem(15, 39, 39, EPunishmentTypeDisplayType.Criminal, -1, 40, new List<ShortPair>
		{
			new ShortPair(1, 1),
			new ShortPair(2, 0),
			new ShortPair(3, 0),
			new ShortPair(4, 1),
			new ShortPair(5, 0),
			new ShortPair(7, 0),
			new ShortPair(8, 0),
			new ShortPair(9, 0)
		}, new List<ShortPair>(), 1000, 0u));
		_dataArray.Add(new PunishmentTypeItem(16, 41, 41, EPunishmentTypeDisplayType.Criminal, -1, 42, new List<ShortPair>
		{
			new ShortPair(1, 1),
			new ShortPair(2, 0),
			new ShortPair(3, 0),
			new ShortPair(4, 1),
			new ShortPair(5, 0),
			new ShortPair(7, 0),
			new ShortPair(8, 0),
			new ShortPair(9, 0)
		}, new List<ShortPair>(), 1000, 0u));
		_dataArray.Add(new PunishmentTypeItem(17, 43, 43, EPunishmentTypeDisplayType.Criminal, -1, 44, new List<ShortPair>(), new List<ShortPair>(), 2000, 0u));
		_dataArray.Add(new PunishmentTypeItem(18, 45, 46, EPunishmentTypeDisplayType.Criminal, -1, 47, new List<ShortPair>
		{
			new ShortPair(1, 0),
			new ShortPair(4, 0),
			new ShortPair(5, 0),
			new ShortPair(6, 0),
			new ShortPair(9, 0),
			new ShortPair(14, 0)
		}, new List<ShortPair>(), 1000, 0u));
		_dataArray.Add(new PunishmentTypeItem(19, 48, 48, EPunishmentTypeDisplayType.Criminal, -1, 309, new List<ShortPair>
		{
			new ShortPair(5, 0),
			new ShortPair(7, 0),
			new ShortPair(13, 0),
			new ShortPair(1, 0)
		}, new List<ShortPair>(), 1000, 0u));
		_dataArray.Add(new PunishmentTypeItem(20, 50, 51, EPunishmentTypeDisplayType.Criminal, -1, 52, new List<ShortPair>(), new List<ShortPair>(), 2000, 0u));
		_dataArray.Add(new PunishmentTypeItem(21, 53, 54, EPunishmentTypeDisplayType.Criminal, -1, 53, new List<ShortPair>
		{
			new ShortPair(1, 3),
			new ShortPair(2, 2),
			new ShortPair(3, 2),
			new ShortPair(4, 3),
			new ShortPair(5, 2),
			new ShortPair(6, 0),
			new ShortPair(7, 1),
			new ShortPair(8, 1),
			new ShortPair(9, 1),
			new ShortPair(10, 0)
		}, new List<ShortPair>
		{
			new ShortPair(1, 4),
			new ShortPair(2, 4),
			new ShortPair(3, 4),
			new ShortPair(4, 4),
			new ShortPair(5, 4),
			new ShortPair(6, 4),
			new ShortPair(7, 4),
			new ShortPair(8, 4),
			new ShortPair(9, 4),
			new ShortPair(10, 4),
			new ShortPair(11, 4),
			new ShortPair(12, 4),
			new ShortPair(13, 4),
			new ShortPair(14, 4),
			new ShortPair(15, 4)
		}, 5000, 0u));
		_dataArray.Add(new PunishmentTypeItem(22, 55, 56, EPunishmentTypeDisplayType.Criminal, -1, 55, new List<ShortPair>
		{
			new ShortPair(1, 0),
			new ShortPair(4, 0)
		}, new List<ShortPair>
		{
			new ShortPair(1, 3),
			new ShortPair(2, 3),
			new ShortPair(3, 3),
			new ShortPair(4, 3),
			new ShortPair(5, 3),
			new ShortPair(6, 3),
			new ShortPair(7, 3),
			new ShortPair(8, 3),
			new ShortPair(9, 3),
			new ShortPair(10, 3),
			new ShortPair(11, 3),
			new ShortPair(12, 3),
			new ShortPair(13, 3),
			new ShortPair(14, 3),
			new ShortPair(15, 3)
		}, 4000, 0u));
		_dataArray.Add(new PunishmentTypeItem(23, 57, 58, EPunishmentTypeDisplayType.Criminal, -1, 59, new List<ShortPair>(), new List<ShortPair>(), 2000, 0u));
		_dataArray.Add(new PunishmentTypeItem(24, 60, 58, EPunishmentTypeDisplayType.Criminal, -1, 61, new List<ShortPair>
		{
			new ShortPair(11, 0)
		}, new List<ShortPair>(), 2000, 0u));
		_dataArray.Add(new PunishmentTypeItem(25, 62, 58, EPunishmentTypeDisplayType.Criminal, -1, 63, new List<ShortPair>(), new List<ShortPair>(), 2000, 0u));
		_dataArray.Add(new PunishmentTypeItem(26, 64, 58, EPunishmentTypeDisplayType.Criminal, -1, 65, new List<ShortPair>(), new List<ShortPair>(), 2000, 0u));
		_dataArray.Add(new PunishmentTypeItem(27, 66, 58, EPunishmentTypeDisplayType.Criminal, -1, 67, new List<ShortPair>
		{
			new ShortPair(12, 0)
		}, new List<ShortPair>(), 2000, 0u));
		_dataArray.Add(new PunishmentTypeItem(28, 68, 58, EPunishmentTypeDisplayType.Criminal, -1, 69, new List<ShortPair>(), new List<ShortPair>(), 2000, 0u));
		_dataArray.Add(new PunishmentTypeItem(29, 70, 58, EPunishmentTypeDisplayType.Criminal, -1, 71, new List<ShortPair>(), new List<ShortPair>(), 2000, 0u));
		_dataArray.Add(new PunishmentTypeItem(30, 72, 58, EPunishmentTypeDisplayType.Criminal, -1, 73, new List<ShortPair>(), new List<ShortPair>(), 2000, 0u));
		_dataArray.Add(new PunishmentTypeItem(31, 74, 58, EPunishmentTypeDisplayType.Criminal, -1, 75, new List<ShortPair>(), new List<ShortPair>(), 2000, 0u));
		_dataArray.Add(new PunishmentTypeItem(32, 76, 58, EPunishmentTypeDisplayType.Criminal, -1, 77, new List<ShortPair>
		{
			new ShortPair(7, 0)
		}, new List<ShortPair>(), 2000, 0u));
		_dataArray.Add(new PunishmentTypeItem(33, 78, 58, EPunishmentTypeDisplayType.Criminal, -1, 79, new List<ShortPair>(), new List<ShortPair>(), 2000, 0u));
		_dataArray.Add(new PunishmentTypeItem(34, 80, 58, EPunishmentTypeDisplayType.Criminal, -1, 81, new List<ShortPair>
		{
			new ShortPair(5, 0)
		}, new List<ShortPair>(), 2000, 0u));
		_dataArray.Add(new PunishmentTypeItem(35, 82, 58, EPunishmentTypeDisplayType.Criminal, -1, 83, new List<ShortPair>
		{
			new ShortPair(4, 0)
		}, new List<ShortPair>(), 2000, 0u));
		_dataArray.Add(new PunishmentTypeItem(36, 84, 58, EPunishmentTypeDisplayType.Criminal, -1, 85, new List<ShortPair>
		{
			new ShortPair(1, 0)
		}, new List<ShortPair>(), 2000, 0u));
		_dataArray.Add(new PunishmentTypeItem(37, 86, 58, EPunishmentTypeDisplayType.Criminal, -1, 87, new List<ShortPair>(), new List<ShortPair>(), 2000, 0u));
		_dataArray.Add(new PunishmentTypeItem(38, 88, 89, EPunishmentTypeDisplayType.Criminal, -1, 90, new List<ShortPair>(), new List<ShortPair>(), 1000, 0u));
		_dataArray.Add(new PunishmentTypeItem(39, 91, 91, EPunishmentTypeDisplayType.XiangshuInfected, 3, 92, new List<ShortPair>
		{
			new ShortPair(1, 3),
			new ShortPair(2, 3),
			new ShortPair(3, 3),
			new ShortPair(4, 3),
			new ShortPair(5, 3),
			new ShortPair(6, 3),
			new ShortPair(7, 3),
			new ShortPair(8, 3),
			new ShortPair(9, 3),
			new ShortPair(10, 3),
			new ShortPair(11, 3),
			new ShortPair(12, 3),
			new ShortPair(13, 3),
			new ShortPair(14, 3),
			new ShortPair(15, 3)
		}, new List<ShortPair>
		{
			new ShortPair(1, 3),
			new ShortPair(2, 3),
			new ShortPair(3, 3),
			new ShortPair(4, 3),
			new ShortPair(5, 3),
			new ShortPair(6, 3),
			new ShortPair(7, 3),
			new ShortPair(8, 3),
			new ShortPair(9, 3),
			new ShortPair(10, 3),
			new ShortPair(11, 3),
			new ShortPair(12, 3),
			new ShortPair(13, 3),
			new ShortPair(14, 3),
			new ShortPair(15, 3)
		}, 4000, 0u));
		_dataArray.Add(new PunishmentTypeItem(40, 93, 94, EPunishmentTypeDisplayType.Criminal, 3, 95, new List<ShortPair>
		{
			new ShortPair(1, 3),
			new ShortPair(2, 3),
			new ShortPair(3, 3),
			new ShortPair(4, 3),
			new ShortPair(5, 3),
			new ShortPair(6, 3),
			new ShortPair(7, 3),
			new ShortPair(8, 3),
			new ShortPair(9, 3),
			new ShortPair(10, 3),
			new ShortPair(11, 3),
			new ShortPair(12, 3),
			new ShortPair(13, 3),
			new ShortPair(14, 3),
			new ShortPair(15, 3)
		}, new List<ShortPair>
		{
			new ShortPair(1, 3),
			new ShortPair(2, 3),
			new ShortPair(3, 3),
			new ShortPair(4, 3),
			new ShortPair(5, 3),
			new ShortPair(6, 3),
			new ShortPair(7, 3),
			new ShortPair(8, 3),
			new ShortPair(9, 3),
			new ShortPair(10, 3),
			new ShortPair(11, 3),
			new ShortPair(12, 3),
			new ShortPair(13, 3),
			new ShortPair(14, 3),
			new ShortPair(15, 3)
		}, 4000, 0u));
		_dataArray.Add(new PunishmentTypeItem(41, 96, 97, EPunishmentTypeDisplayType.SettlementEnemy, 1, 98, new List<ShortPair>
		{
			new ShortPair(1, 1),
			new ShortPair(2, 1),
			new ShortPair(3, 1),
			new ShortPair(4, 1),
			new ShortPair(5, 1),
			new ShortPair(6, 1),
			new ShortPair(7, 1),
			new ShortPair(8, 1),
			new ShortPair(9, 1),
			new ShortPair(10, 1),
			new ShortPair(11, 1),
			new ShortPair(12, 1),
			new ShortPair(13, 1),
			new ShortPair(14, 1),
			new ShortPair(15, 1)
		}, new List<ShortPair>(), 2000, 0u));
		_dataArray.Add(new PunishmentTypeItem(42, 99, 100, EPunishmentTypeDisplayType.PersonalEnemy, 1, 101, new List<ShortPair>
		{
			new ShortPair(1, 1),
			new ShortPair(2, 1),
			new ShortPair(3, 1),
			new ShortPair(4, 1),
			new ShortPair(5, 1),
			new ShortPair(6, 1),
			new ShortPair(7, 1),
			new ShortPair(8, 1),
			new ShortPair(9, 1),
			new ShortPair(10, 1),
			new ShortPair(11, 1),
			new ShortPair(12, 1),
			new ShortPair(13, 1),
			new ShortPair(14, 1),
			new ShortPair(15, 1)
		}, new List<ShortPair>(), 2000, 0u));
		_dataArray.Add(new PunishmentTypeItem(43, 0, 1, EPunishmentTypeDisplayType.Default, -1, 2, new List<ShortPair>(), new List<ShortPair>(), 0, 0u));
		_dataArray.Add(new PunishmentTypeItem(44, 48, 48, EPunishmentTypeDisplayType.Criminal, -1, 310, new List<ShortPair>
		{
			new ShortPair(10, 0),
			new ShortPair(1, 0)
		}, new List<ShortPair>(), 1000, 0u));
		_dataArray.Add(new PunishmentTypeItem(45, 102, 25, EPunishmentTypeDisplayType.Criminal, -1, 103, new List<ShortPair>
		{
			new ShortPair(1, 3),
			new ShortPair(8, 4)
		}, new List<ShortPair>(), 4000, 0u));
		_dataArray.Add(new PunishmentTypeItem(46, 102, 25, EPunishmentTypeDisplayType.Criminal, -1, 104, new List<ShortPair>
		{
			new ShortPair(2, 2),
			new ShortPair(4, 2),
			new ShortPair(7, 1)
		}, new List<ShortPair>(), 3000, 0u));
		_dataArray.Add(new PunishmentTypeItem(47, 102, 25, EPunishmentTypeDisplayType.Criminal, -1, 105, new List<ShortPair>
		{
			new ShortPair(11, 1),
			new ShortPair(12, 3)
		}, new List<ShortPair>(), 3000, 0u));
		_dataArray.Add(new PunishmentTypeItem(48, 106, 25, EPunishmentTypeDisplayType.Criminal, -1, 107, new List<ShortPair>
		{
			new ShortPair(1, 3)
		}, new List<ShortPair>(), 4000, 0u));
		_dataArray.Add(new PunishmentTypeItem(49, 106, 25, EPunishmentTypeDisplayType.Criminal, -1, 108, new List<ShortPair>
		{
			new ShortPair(2, 2),
			new ShortPair(4, 2),
			new ShortPair(7, 1)
		}, new List<ShortPair>(), 3000, 0u));
		_dataArray.Add(new PunishmentTypeItem(50, 106, 25, EPunishmentTypeDisplayType.Criminal, -1, 109, new List<ShortPair>
		{
			new ShortPair(11, 2),
			new ShortPair(12, 2)
		}, new List<ShortPair>(), 3000, 0u));
		_dataArray.Add(new PunishmentTypeItem(51, 110, 25, EPunishmentTypeDisplayType.Criminal, -1, 111, new List<ShortPair>
		{
			new ShortPair(1, 3),
			new ShortPair(8, 3)
		}, new List<ShortPair>(), 4000, 0u));
		_dataArray.Add(new PunishmentTypeItem(52, 110, 25, EPunishmentTypeDisplayType.Criminal, -1, 112, new List<ShortPair>
		{
			new ShortPair(2, 2),
			new ShortPair(4, 3),
			new ShortPair(7, 2)
		}, new List<ShortPair>(), 3000, 0u));
		_dataArray.Add(new PunishmentTypeItem(53, 110, 25, EPunishmentTypeDisplayType.Criminal, -1, 113, new List<ShortPair>
		{
			new ShortPair(11, 3),
			new ShortPair(12, 3)
		}, new List<ShortPair>(), 4000, 0u));
		_dataArray.Add(new PunishmentTypeItem(54, 114, 25, EPunishmentTypeDisplayType.Criminal, -1, 115, new List<ShortPair>
		{
			new ShortPair(1, 2),
			new ShortPair(8, 2)
		}, new List<ShortPair>(), 3000, 0u));
		_dataArray.Add(new PunishmentTypeItem(55, 114, 25, EPunishmentTypeDisplayType.Criminal, -1, 116, new List<ShortPair>
		{
			new ShortPair(2, 1),
			new ShortPair(4, 1)
		}, new List<ShortPair>(), 2000, 0u));
		_dataArray.Add(new PunishmentTypeItem(56, 114, 25, EPunishmentTypeDisplayType.Criminal, -1, 117, new List<ShortPair>
		{
			new ShortPair(11, 1),
			new ShortPair(12, 1)
		}, new List<ShortPair>(), 2000, 0u));
		_dataArray.Add(new PunishmentTypeItem(57, 118, 25, EPunishmentTypeDisplayType.Criminal, -1, 119, new List<ShortPair>
		{
			new ShortPair(1, 4),
			new ShortPair(8, 4)
		}, new List<ShortPair>(), 5000, 0u));
		_dataArray.Add(new PunishmentTypeItem(58, 118, 25, EPunishmentTypeDisplayType.Criminal, -1, 120, new List<ShortPair>
		{
			new ShortPair(2, 3),
			new ShortPair(4, 3),
			new ShortPair(7, 1)
		}, new List<ShortPair>(), 4000, 0u));
		_dataArray.Add(new PunishmentTypeItem(59, 118, 25, EPunishmentTypeDisplayType.Criminal, -1, 121, new List<ShortPair>
		{
			new ShortPair(11, 2),
			new ShortPair(12, 2)
		}, new List<ShortPair>(), 3000, 0u));
	}

	private void CreateItems1()
	{
		_dataArray.Add(new PunishmentTypeItem(60, 122, 25, EPunishmentTypeDisplayType.Criminal, -1, 123, new List<ShortPair>(), new List<ShortPair>(), 2000, 2305890u));
		_dataArray.Add(new PunishmentTypeItem(61, 122, 25, EPunishmentTypeDisplayType.Criminal, -1, 124, new List<ShortPair>(), new List<ShortPair>(), 2000, 2305890u));
		_dataArray.Add(new PunishmentTypeItem(62, 122, 25, EPunishmentTypeDisplayType.Criminal, -1, 125, new List<ShortPair>(), new List<ShortPair>(), 2000, 2305890u));
		_dataArray.Add(new PunishmentTypeItem(63, 126, 25, EPunishmentTypeDisplayType.Criminal, -1, 115, new List<ShortPair>
		{
			new ShortPair(2, 0),
			new ShortPair(3, 0),
			new ShortPair(4, 0),
			new ShortPair(5, 0)
		}, new List<ShortPair>(), 1000, 0u));
		_dataArray.Add(new PunishmentTypeItem(64, 127, 25, EPunishmentTypeDisplayType.Criminal, -1, 119, new List<ShortPair>
		{
			new ShortPair(2, 2),
			new ShortPair(3, 2),
			new ShortPair(4, 2),
			new ShortPair(5, 2),
			new ShortPair(7, 1),
			new ShortPair(9, 1),
			new ShortPair(10, 0)
		}, new List<ShortPair>
		{
			new ShortPair(1, 2),
			new ShortPair(2, 2),
			new ShortPair(3, 2),
			new ShortPair(4, 2),
			new ShortPair(5, 2),
			new ShortPair(6, 2),
			new ShortPair(7, 2),
			new ShortPair(8, 2),
			new ShortPair(9, 2),
			new ShortPair(10, 2),
			new ShortPair(11, 2),
			new ShortPair(12, 2),
			new ShortPair(13, 2),
			new ShortPair(14, 2),
			new ShortPair(15, 2)
		}, 3000, 0u));
		_dataArray.Add(new PunishmentTypeItem(65, 128, 58, EPunishmentTypeDisplayType.Criminal, -1, 129, new List<ShortPair>
		{
			new ShortPair(14, 0)
		}, new List<ShortPair>(), 2000, 0u));
		_dataArray.Add(new PunishmentTypeItem(66, 130, 58, EPunishmentTypeDisplayType.Criminal, -1, 131, new List<ShortPair>
		{
			new ShortPair(11, 1)
		}, new List<ShortPair>(), 2000, 0u));
		_dataArray.Add(new PunishmentTypeItem(67, 132, 58, EPunishmentTypeDisplayType.Criminal, -1, 133, new List<ShortPair>
		{
			new ShortPair(15, 0)
		}, new List<ShortPair>(), 2000, 0u));
		_dataArray.Add(new PunishmentTypeItem(68, 134, 58, EPunishmentTypeDisplayType.Criminal, -1, 135, new List<ShortPair>
		{
			new ShortPair(13, 0)
		}, new List<ShortPair>(), 2000, 0u));
		_dataArray.Add(new PunishmentTypeItem(69, 136, 58, EPunishmentTypeDisplayType.Criminal, -1, 137, new List<ShortPair>
		{
			new ShortPair(12, 1)
		}, new List<ShortPair>(), 2000, 0u));
		_dataArray.Add(new PunishmentTypeItem(70, 138, 58, EPunishmentTypeDisplayType.Criminal, -1, 139, new List<ShortPair>
		{
			new ShortPair(9, 0)
		}, new List<ShortPair>(), 2000, 0u));
		_dataArray.Add(new PunishmentTypeItem(71, 140, 58, EPunishmentTypeDisplayType.Criminal, -1, 141, new List<ShortPair>
		{
			new ShortPair(6, 0)
		}, new List<ShortPair>(), 2000, 0u));
		_dataArray.Add(new PunishmentTypeItem(72, 142, 58, EPunishmentTypeDisplayType.Criminal, -1, 143, new List<ShortPair>
		{
			new ShortPair(10, 0)
		}, new List<ShortPair>(), 2000, 0u));
		_dataArray.Add(new PunishmentTypeItem(73, 144, 58, EPunishmentTypeDisplayType.Criminal, -1, 145, new List<ShortPair>
		{
			new ShortPair(8, 1)
		}, new List<ShortPair>(), 2000, 0u));
		_dataArray.Add(new PunishmentTypeItem(74, 146, 58, EPunishmentTypeDisplayType.Criminal, -1, 147, new List<ShortPair>
		{
			new ShortPair(7, 0)
		}, new List<ShortPair>(), 2000, 0u));
		_dataArray.Add(new PunishmentTypeItem(75, 148, 58, EPunishmentTypeDisplayType.Criminal, -1, 149, new List<ShortPair>
		{
			new ShortPair(2, 0)
		}, new List<ShortPair>(), 2000, 0u));
		_dataArray.Add(new PunishmentTypeItem(76, 150, 58, EPunishmentTypeDisplayType.Criminal, -1, 151, new List<ShortPair>
		{
			new ShortPair(5, 1)
		}, new List<ShortPair>(), 2000, 0u));
		_dataArray.Add(new PunishmentTypeItem(77, 152, 58, EPunishmentTypeDisplayType.Criminal, -1, 153, new List<ShortPair>
		{
			new ShortPair(4, 1)
		}, new List<ShortPair>(), 2000, 0u));
		_dataArray.Add(new PunishmentTypeItem(78, 154, 58, EPunishmentTypeDisplayType.Criminal, -1, 155, new List<ShortPair>(), new List<ShortPair>(), 2000, 0u));
		_dataArray.Add(new PunishmentTypeItem(79, 156, 58, EPunishmentTypeDisplayType.Criminal, -1, 157, new List<ShortPair>
		{
			new ShortPair(3, 0)
		}, new List<ShortPair>(), 2000, 0u));
		_dataArray.Add(new PunishmentTypeItem(80, 158, 58, EPunishmentTypeDisplayType.Criminal, -1, 159, new List<ShortPair>
		{
			new ShortPair(14, 0)
		}, new List<ShortPair>(), 1000, 0u));
		_dataArray.Add(new PunishmentTypeItem(81, 160, 58, EPunishmentTypeDisplayType.Criminal, -1, 161, new List<ShortPair>
		{
			new ShortPair(11, 1)
		}, new List<ShortPair>(), 1000, 0u));
		_dataArray.Add(new PunishmentTypeItem(82, 162, 58, EPunishmentTypeDisplayType.Criminal, -1, 163, new List<ShortPair>
		{
			new ShortPair(15, 0)
		}, new List<ShortPair>(), 1000, 0u));
		_dataArray.Add(new PunishmentTypeItem(83, 164, 58, EPunishmentTypeDisplayType.Criminal, -1, 165, new List<ShortPair>
		{
			new ShortPair(13, 0)
		}, new List<ShortPair>(), 1000, 0u));
		_dataArray.Add(new PunishmentTypeItem(84, 166, 58, EPunishmentTypeDisplayType.Criminal, -1, 167, new List<ShortPair>
		{
			new ShortPair(12, 1)
		}, new List<ShortPair>(), 1000, 0u));
		_dataArray.Add(new PunishmentTypeItem(85, 168, 58, EPunishmentTypeDisplayType.Criminal, -1, 169, new List<ShortPair>
		{
			new ShortPair(9, 0)
		}, new List<ShortPair>(), 1000, 0u));
		_dataArray.Add(new PunishmentTypeItem(86, 170, 58, EPunishmentTypeDisplayType.Criminal, -1, 171, new List<ShortPair>
		{
			new ShortPair(6, 0)
		}, new List<ShortPair>(), 1000, 0u));
		_dataArray.Add(new PunishmentTypeItem(87, 172, 58, EPunishmentTypeDisplayType.Criminal, -1, 173, new List<ShortPair>
		{
			new ShortPair(10, 0)
		}, new List<ShortPair>(), 1000, 0u));
		_dataArray.Add(new PunishmentTypeItem(88, 174, 58, EPunishmentTypeDisplayType.Criminal, -1, 175, new List<ShortPair>
		{
			new ShortPair(8, 0)
		}, new List<ShortPair>(), 1000, 0u));
		_dataArray.Add(new PunishmentTypeItem(89, 176, 58, EPunishmentTypeDisplayType.Criminal, -1, 177, new List<ShortPair>
		{
			new ShortPair(7, 1)
		}, new List<ShortPair>(), 1000, 0u));
		_dataArray.Add(new PunishmentTypeItem(90, 178, 58, EPunishmentTypeDisplayType.Criminal, -1, 179, new List<ShortPair>
		{
			new ShortPair(2, 0)
		}, new List<ShortPair>(), 1000, 0u));
		_dataArray.Add(new PunishmentTypeItem(91, 180, 58, EPunishmentTypeDisplayType.Criminal, -1, 181, new List<ShortPair>
		{
			new ShortPair(5, 1)
		}, new List<ShortPair>(), 1000, 0u));
		_dataArray.Add(new PunishmentTypeItem(92, 182, 58, EPunishmentTypeDisplayType.Criminal, -1, 183, new List<ShortPair>
		{
			new ShortPair(4, 1)
		}, new List<ShortPair>(), 1000, 0u));
		_dataArray.Add(new PunishmentTypeItem(93, 184, 58, EPunishmentTypeDisplayType.Criminal, -1, 185, new List<ShortPair>
		{
			new ShortPair(1, 1)
		}, new List<ShortPair>(), 1000, 0u));
		_dataArray.Add(new PunishmentTypeItem(94, 186, 58, EPunishmentTypeDisplayType.Criminal, -1, 187, new List<ShortPair>
		{
			new ShortPair(3, 0)
		}, new List<ShortPair>(), 1000, 0u));
		_dataArray.Add(new PunishmentTypeItem(95, 188, 58, EPunishmentTypeDisplayType.Criminal, -1, 189, new List<ShortPair>
		{
			new ShortPair(14, 1)
		}, new List<ShortPair>(), 2000, 0u));
		_dataArray.Add(new PunishmentTypeItem(96, 190, 58, EPunishmentTypeDisplayType.Criminal, -1, 191, new List<ShortPair>
		{
			new ShortPair(11, 2)
		}, new List<ShortPair>(), 2000, 0u));
		_dataArray.Add(new PunishmentTypeItem(97, 192, 58, EPunishmentTypeDisplayType.Criminal, -1, 193, new List<ShortPair>
		{
			new ShortPair(15, 1)
		}, new List<ShortPair>(), 2000, 0u));
		_dataArray.Add(new PunishmentTypeItem(98, 194, 58, EPunishmentTypeDisplayType.Criminal, -1, 195, new List<ShortPair>
		{
			new ShortPair(13, 1)
		}, new List<ShortPair>(), 2000, 0u));
		_dataArray.Add(new PunishmentTypeItem(99, 196, 58, EPunishmentTypeDisplayType.Criminal, -1, 197, new List<ShortPair>
		{
			new ShortPair(12, 2)
		}, new List<ShortPair>(), 2000, 0u));
		_dataArray.Add(new PunishmentTypeItem(100, 198, 58, EPunishmentTypeDisplayType.Criminal, -1, 199, new List<ShortPair>
		{
			new ShortPair(9, 1)
		}, new List<ShortPair>(), 2000, 0u));
		_dataArray.Add(new PunishmentTypeItem(101, 200, 58, EPunishmentTypeDisplayType.Criminal, -1, 201, new List<ShortPair>
		{
			new ShortPair(6, 1)
		}, new List<ShortPair>(), 2000, 0u));
		_dataArray.Add(new PunishmentTypeItem(102, 202, 58, EPunishmentTypeDisplayType.Criminal, -1, 203, new List<ShortPair>
		{
			new ShortPair(10, 1)
		}, new List<ShortPair>(), 2000, 0u));
		_dataArray.Add(new PunishmentTypeItem(103, 204, 58, EPunishmentTypeDisplayType.Criminal, -1, 205, new List<ShortPair>
		{
			new ShortPair(8, 1)
		}, new List<ShortPair>(), 2000, 0u));
		_dataArray.Add(new PunishmentTypeItem(104, 206, 58, EPunishmentTypeDisplayType.Criminal, -1, 207, new List<ShortPair>
		{
			new ShortPair(7, 2)
		}, new List<ShortPair>(), 2000, 0u));
		_dataArray.Add(new PunishmentTypeItem(105, 208, 58, EPunishmentTypeDisplayType.Criminal, -1, 209, new List<ShortPair>
		{
			new ShortPair(2, 1)
		}, new List<ShortPair>(), 2000, 0u));
		_dataArray.Add(new PunishmentTypeItem(106, 210, 58, EPunishmentTypeDisplayType.Criminal, -1, 211, new List<ShortPair>
		{
			new ShortPair(5, 2)
		}, new List<ShortPair>(), 2000, 0u));
		_dataArray.Add(new PunishmentTypeItem(107, 212, 58, EPunishmentTypeDisplayType.Criminal, -1, 213, new List<ShortPair>
		{
			new ShortPair(4, 2)
		}, new List<ShortPair>(), 2000, 0u));
		_dataArray.Add(new PunishmentTypeItem(108, 214, 58, EPunishmentTypeDisplayType.Criminal, -1, 215, new List<ShortPair>
		{
			new ShortPair(1, 2)
		}, new List<ShortPair>(), 2000, 0u));
		_dataArray.Add(new PunishmentTypeItem(109, 216, 58, EPunishmentTypeDisplayType.Criminal, -1, 217, new List<ShortPair>
		{
			new ShortPair(3, 1)
		}, new List<ShortPair>(), 2000, 0u));
		_dataArray.Add(new PunishmentTypeItem(110, 188, 58, EPunishmentTypeDisplayType.Criminal, -1, 218, new List<ShortPair>
		{
			new ShortPair(14, 1)
		}, new List<ShortPair>(), 2000, 0u));
		_dataArray.Add(new PunishmentTypeItem(111, 190, 58, EPunishmentTypeDisplayType.Criminal, -1, 219, new List<ShortPair>
		{
			new ShortPair(11, 2)
		}, new List<ShortPair>(), 2000, 0u));
		_dataArray.Add(new PunishmentTypeItem(112, 192, 58, EPunishmentTypeDisplayType.Criminal, -1, 220, new List<ShortPair>
		{
			new ShortPair(15, 1)
		}, new List<ShortPair>(), 2000, 0u));
		_dataArray.Add(new PunishmentTypeItem(113, 194, 58, EPunishmentTypeDisplayType.Criminal, -1, 221, new List<ShortPair>
		{
			new ShortPair(13, 1)
		}, new List<ShortPair>(), 2000, 0u));
		_dataArray.Add(new PunishmentTypeItem(114, 196, 58, EPunishmentTypeDisplayType.Criminal, -1, 222, new List<ShortPair>
		{
			new ShortPair(12, 2)
		}, new List<ShortPair>(), 2000, 0u));
		_dataArray.Add(new PunishmentTypeItem(115, 198, 58, EPunishmentTypeDisplayType.Criminal, -1, 223, new List<ShortPair>
		{
			new ShortPair(9, 1)
		}, new List<ShortPair>(), 2000, 0u));
		_dataArray.Add(new PunishmentTypeItem(116, 200, 58, EPunishmentTypeDisplayType.Criminal, -1, 224, new List<ShortPair>
		{
			new ShortPair(6, 1)
		}, new List<ShortPair>(), 2000, 0u));
		_dataArray.Add(new PunishmentTypeItem(117, 202, 58, EPunishmentTypeDisplayType.Criminal, -1, 225, new List<ShortPair>
		{
			new ShortPair(10, 1)
		}, new List<ShortPair>(), 2000, 0u));
		_dataArray.Add(new PunishmentTypeItem(118, 204, 58, EPunishmentTypeDisplayType.Criminal, -1, 226, new List<ShortPair>
		{
			new ShortPair(8, 1)
		}, new List<ShortPair>(), 2000, 0u));
		_dataArray.Add(new PunishmentTypeItem(119, 206, 58, EPunishmentTypeDisplayType.Criminal, -1, 227, new List<ShortPair>
		{
			new ShortPair(7, 2)
		}, new List<ShortPair>(), 2000, 0u));
	}

	private void CreateItems2()
	{
		_dataArray.Add(new PunishmentTypeItem(120, 208, 58, EPunishmentTypeDisplayType.Criminal, -1, 228, new List<ShortPair>
		{
			new ShortPair(2, 1)
		}, new List<ShortPair>(), 2000, 0u));
		_dataArray.Add(new PunishmentTypeItem(121, 210, 58, EPunishmentTypeDisplayType.Criminal, -1, 229, new List<ShortPair>
		{
			new ShortPair(5, 2)
		}, new List<ShortPair>(), 2000, 0u));
		_dataArray.Add(new PunishmentTypeItem(122, 212, 58, EPunishmentTypeDisplayType.Criminal, -1, 230, new List<ShortPair>
		{
			new ShortPair(4, 2)
		}, new List<ShortPair>(), 2000, 0u));
		_dataArray.Add(new PunishmentTypeItem(123, 214, 58, EPunishmentTypeDisplayType.Criminal, -1, 231, new List<ShortPair>
		{
			new ShortPair(1, 2)
		}, new List<ShortPair>(), 2000, 0u));
		_dataArray.Add(new PunishmentTypeItem(124, 216, 58, EPunishmentTypeDisplayType.Criminal, -1, 232, new List<ShortPair>
		{
			new ShortPair(3, 1)
		}, new List<ShortPair>(), 2000, 0u));
		_dataArray.Add(new PunishmentTypeItem(125, 233, 58, EPunishmentTypeDisplayType.Criminal, -1, 234, new List<ShortPair>(), new List<ShortPair>(), 2000, 2305890u));
		_dataArray.Add(new PunishmentTypeItem(126, 235, 58, EPunishmentTypeDisplayType.Criminal, -1, 236, new List<ShortPair>(), new List<ShortPair>(), 2000, 2305890u));
		_dataArray.Add(new PunishmentTypeItem(127, 237, 58, EPunishmentTypeDisplayType.Criminal, -1, 238, new List<ShortPair>(), new List<ShortPair>(), 2000, 2305890u));
		_dataArray.Add(new PunishmentTypeItem(128, 239, 58, EPunishmentTypeDisplayType.Criminal, -1, 240, new List<ShortPair>(), new List<ShortPair>(), 2000, 2305890u));
		_dataArray.Add(new PunishmentTypeItem(129, 241, 58, EPunishmentTypeDisplayType.Criminal, -1, 242, new List<ShortPair>(), new List<ShortPair>(), 2000, 2305890u));
		_dataArray.Add(new PunishmentTypeItem(130, 243, 58, EPunishmentTypeDisplayType.Criminal, -1, 244, new List<ShortPair>(), new List<ShortPair>(), 2000, 2305890u));
		_dataArray.Add(new PunishmentTypeItem(131, 245, 58, EPunishmentTypeDisplayType.Criminal, -1, 246, new List<ShortPair>(), new List<ShortPair>(), 2000, 2305890u));
		_dataArray.Add(new PunishmentTypeItem(132, 247, 58, EPunishmentTypeDisplayType.Criminal, -1, 248, new List<ShortPair>(), new List<ShortPair>(), 2000, 2305890u));
		_dataArray.Add(new PunishmentTypeItem(133, 249, 58, EPunishmentTypeDisplayType.Criminal, -1, 250, new List<ShortPair>(), new List<ShortPair>(), 2000, 2305890u));
		_dataArray.Add(new PunishmentTypeItem(134, 251, 58, EPunishmentTypeDisplayType.Criminal, -1, 252, new List<ShortPair>(), new List<ShortPair>(), 2000, 2305890u));
		_dataArray.Add(new PunishmentTypeItem(135, 253, 58, EPunishmentTypeDisplayType.Criminal, -1, 254, new List<ShortPair>(), new List<ShortPair>(), 2000, 2305890u));
		_dataArray.Add(new PunishmentTypeItem(136, 255, 58, EPunishmentTypeDisplayType.Criminal, -1, 256, new List<ShortPair>(), new List<ShortPair>(), 2000, 2305890u));
		_dataArray.Add(new PunishmentTypeItem(137, 257, 58, EPunishmentTypeDisplayType.Criminal, -1, 258, new List<ShortPair>(), new List<ShortPair>(), 2000, 2305890u));
		_dataArray.Add(new PunishmentTypeItem(138, 259, 58, EPunishmentTypeDisplayType.Criminal, -1, 260, new List<ShortPair>(), new List<ShortPair>(), 2000, 2305890u));
		_dataArray.Add(new PunishmentTypeItem(139, 261, 58, EPunishmentTypeDisplayType.Criminal, -1, 262, new List<ShortPair>(), new List<ShortPair>(), 2000, 2305890u));
		_dataArray.Add(new PunishmentTypeItem(140, 263, 58, EPunishmentTypeDisplayType.Criminal, -1, 264, new List<ShortPair>
		{
			new ShortPair(14, 1)
		}, new List<ShortPair>(), 2000, 0u));
		_dataArray.Add(new PunishmentTypeItem(141, 265, 58, EPunishmentTypeDisplayType.Criminal, -1, 266, new List<ShortPair>
		{
			new ShortPair(11, 2)
		}, new List<ShortPair>(), 2000, 0u));
		_dataArray.Add(new PunishmentTypeItem(142, 267, 58, EPunishmentTypeDisplayType.Criminal, -1, 268, new List<ShortPair>
		{
			new ShortPair(15, 1)
		}, new List<ShortPair>(), 2000, 0u));
		_dataArray.Add(new PunishmentTypeItem(143, 269, 58, EPunishmentTypeDisplayType.Criminal, -1, 270, new List<ShortPair>
		{
			new ShortPair(13, 1)
		}, new List<ShortPair>(), 2000, 0u));
		_dataArray.Add(new PunishmentTypeItem(144, 271, 58, EPunishmentTypeDisplayType.Criminal, -1, 272, new List<ShortPair>
		{
			new ShortPair(12, 2)
		}, new List<ShortPair>(), 2000, 0u));
		_dataArray.Add(new PunishmentTypeItem(145, 273, 58, EPunishmentTypeDisplayType.Criminal, -1, 274, new List<ShortPair>
		{
			new ShortPair(9, 1)
		}, new List<ShortPair>(), 2000, 0u));
		_dataArray.Add(new PunishmentTypeItem(146, 275, 58, EPunishmentTypeDisplayType.Criminal, -1, 276, new List<ShortPair>
		{
			new ShortPair(6, 1)
		}, new List<ShortPair>(), 2000, 0u));
		_dataArray.Add(new PunishmentTypeItem(147, 277, 58, EPunishmentTypeDisplayType.Criminal, -1, 278, new List<ShortPair>
		{
			new ShortPair(10, 1)
		}, new List<ShortPair>(), 2000, 0u));
		_dataArray.Add(new PunishmentTypeItem(148, 279, 58, EPunishmentTypeDisplayType.Criminal, -1, 280, new List<ShortPair>
		{
			new ShortPair(8, 1)
		}, new List<ShortPair>(), 2000, 0u));
		_dataArray.Add(new PunishmentTypeItem(149, 281, 58, EPunishmentTypeDisplayType.Criminal, -1, 282, new List<ShortPair>
		{
			new ShortPair(7, 2)
		}, new List<ShortPair>(), 2000, 0u));
		_dataArray.Add(new PunishmentTypeItem(150, 283, 58, EPunishmentTypeDisplayType.Criminal, -1, 284, new List<ShortPair>
		{
			new ShortPair(2, 1)
		}, new List<ShortPair>(), 2000, 0u));
		_dataArray.Add(new PunishmentTypeItem(151, 285, 58, EPunishmentTypeDisplayType.Criminal, -1, 286, new List<ShortPair>
		{
			new ShortPair(5, 2)
		}, new List<ShortPair>(), 2000, 0u));
		_dataArray.Add(new PunishmentTypeItem(152, 287, 58, EPunishmentTypeDisplayType.Criminal, -1, 288, new List<ShortPair>
		{
			new ShortPair(4, 2)
		}, new List<ShortPair>(), 2000, 0u));
		_dataArray.Add(new PunishmentTypeItem(153, 289, 58, EPunishmentTypeDisplayType.Criminal, -1, 290, new List<ShortPair>
		{
			new ShortPair(1, 2)
		}, new List<ShortPair>(), 2000, 0u));
		_dataArray.Add(new PunishmentTypeItem(154, 291, 58, EPunishmentTypeDisplayType.Criminal, -1, 292, new List<ShortPair>
		{
			new ShortPair(3, 1)
		}, new List<ShortPair>(), 2000, 0u));
		_dataArray.Add(new PunishmentTypeItem(155, 293, 21, EPunishmentTypeDisplayType.Criminal, -1, 294, new List<ShortPair>
		{
			new ShortPair(1, 3),
			new ShortPair(2, 2),
			new ShortPair(3, 2),
			new ShortPair(4, 3),
			new ShortPair(5, 2),
			new ShortPair(6, 0),
			new ShortPair(7, 1),
			new ShortPair(8, 1),
			new ShortPair(9, 1),
			new ShortPair(10, 0)
		}, new List<ShortPair>(), 4000, 0u));
		_dataArray.Add(new PunishmentTypeItem(156, 295, 23, EPunishmentTypeDisplayType.Criminal, -1, 296, new List<ShortPair>
		{
			new ShortPair(1, 1),
			new ShortPair(2, 0),
			new ShortPair(3, 0),
			new ShortPair(4, 1),
			new ShortPair(5, 0),
			new ShortPair(7, 0),
			new ShortPair(8, 0),
			new ShortPair(9, 0)
		}, new List<ShortPair>(), 4000, 0u));
		_dataArray.Add(new PunishmentTypeItem(157, 297, 21, EPunishmentTypeDisplayType.Criminal, -1, 298, new List<ShortPair>
		{
			new ShortPair(1, 4),
			new ShortPair(2, 3),
			new ShortPair(3, 3),
			new ShortPair(4, 4),
			new ShortPair(5, 3),
			new ShortPair(6, 1),
			new ShortPair(7, 2),
			new ShortPair(8, 2),
			new ShortPair(9, 2),
			new ShortPair(10, 1)
		}, new List<ShortPair>(), 5000, 0u));
		_dataArray.Add(new PunishmentTypeItem(158, 299, 23, EPunishmentTypeDisplayType.Criminal, -1, 300, new List<ShortPair>
		{
			new ShortPair(1, 1),
			new ShortPair(2, 0),
			new ShortPair(3, 0),
			new ShortPair(4, 1),
			new ShortPair(5, 0),
			new ShortPair(7, 0),
			new ShortPair(8, 0),
			new ShortPair(9, 0)
		}, new List<ShortPair>(), 5000, 0u));
		_dataArray.Add(new PunishmentTypeItem(159, 301, 43, EPunishmentTypeDisplayType.Criminal, -1, 302, new List<ShortPair>
		{
			new ShortPair(1, 0)
		}, new List<ShortPair>
		{
			new ShortPair(1, 0),
			new ShortPair(2, 0),
			new ShortPair(3, 0),
			new ShortPair(4, 0),
			new ShortPair(5, 0),
			new ShortPair(6, 0),
			new ShortPair(7, 0),
			new ShortPair(8, 0),
			new ShortPair(9, 0),
			new ShortPair(10, 0),
			new ShortPair(11, 0),
			new ShortPair(12, 0),
			new ShortPair(13, 0),
			new ShortPair(14, 0),
			new ShortPair(15, 0)
		}, 1000, 0u));
		_dataArray.Add(new PunishmentTypeItem(160, 303, 43, EPunishmentTypeDisplayType.Criminal, -1, 304, new List<ShortPair>
		{
			new ShortPair(1, 0),
			new ShortPair(4, 0),
			new ShortPair(5, 0),
			new ShortPair(8, 0)
		}, new List<ShortPair>
		{
			new ShortPair(1, 1),
			new ShortPair(2, 1),
			new ShortPair(3, 1),
			new ShortPair(4, 1),
			new ShortPair(5, 1),
			new ShortPair(6, 1),
			new ShortPair(7, 1),
			new ShortPair(8, 1),
			new ShortPair(9, 1),
			new ShortPair(10, 1),
			new ShortPair(11, 1),
			new ShortPair(12, 1),
			new ShortPair(13, 1),
			new ShortPair(14, 1),
			new ShortPair(15, 1)
		}, 2000, 0u));
		_dataArray.Add(new PunishmentTypeItem(161, 305, 43, EPunishmentTypeDisplayType.Criminal, -1, 306, new List<ShortPair>
		{
			new ShortPair(1, 1),
			new ShortPair(2, 0),
			new ShortPair(3, 0),
			new ShortPair(4, 0),
			new ShortPair(5, 0),
			new ShortPair(8, 0),
			new ShortPair(9, 0)
		}, new List<ShortPair>
		{
			new ShortPair(1, 0),
			new ShortPair(2, 0),
			new ShortPair(3, 0),
			new ShortPair(4, 0),
			new ShortPair(5, 0),
			new ShortPair(6, 0),
			new ShortPair(7, 0),
			new ShortPair(8, 0),
			new ShortPair(9, 0),
			new ShortPair(10, 0),
			new ShortPair(11, 0),
			new ShortPair(12, 0),
			new ShortPair(13, 0),
			new ShortPair(14, 0),
			new ShortPair(15, 0)
		}, 1000, 0u));
		_dataArray.Add(new PunishmentTypeItem(162, 307, 43, EPunishmentTypeDisplayType.Criminal, -1, 308, new List<ShortPair>
		{
			new ShortPair(1, 1),
			new ShortPair(2, 0),
			new ShortPair(3, 0),
			new ShortPair(4, 1),
			new ShortPair(5, 1),
			new ShortPair(8, 1),
			new ShortPair(9, 0)
		}, new List<ShortPair>
		{
			new ShortPair(1, 1),
			new ShortPair(2, 1),
			new ShortPair(3, 1),
			new ShortPair(4, 1),
			new ShortPair(5, 1),
			new ShortPair(6, 1),
			new ShortPair(7, 1),
			new ShortPair(8, 1),
			new ShortPair(9, 1),
			new ShortPair(10, 1),
			new ShortPair(11, 1),
			new ShortPair(12, 1),
			new ShortPair(13, 1),
			new ShortPair(14, 1),
			new ShortPair(15, 1)
		}, 2000, 0u));
		_dataArray.Add(new PunishmentTypeItem(163, 48, 48, EPunishmentTypeDisplayType.Criminal, -1, 49, new List<ShortPair>(), new List<ShortPair>(), 2000, 0u));
		_dataArray.Add(new PunishmentTypeItem(164, 48, 48, EPunishmentTypeDisplayType.Criminal, -1, 311, new List<ShortPair>
		{
			new ShortPair(2, 0),
			new ShortPair(4, 0)
		}, new List<ShortPair>(), 1000, 0u));
		_dataArray.Add(new PunishmentTypeItem(165, 312, 313, EPunishmentTypeDisplayType.Criminal, 2, 314, new List<ShortPair>
		{
			new ShortPair(1, 2),
			new ShortPair(2, 2),
			new ShortPair(3, 2),
			new ShortPair(4, 2),
			new ShortPair(5, 2),
			new ShortPair(6, 2),
			new ShortPair(7, 2),
			new ShortPair(8, 2),
			new ShortPair(9, 2),
			new ShortPair(10, 2),
			new ShortPair(11, 2),
			new ShortPair(12, 2),
			new ShortPair(13, 2),
			new ShortPair(14, 2),
			new ShortPair(15, 2)
		}, new List<ShortPair>
		{
			new ShortPair(1, 2),
			new ShortPair(2, 2),
			new ShortPair(3, 2),
			new ShortPair(4, 2),
			new ShortPair(5, 2),
			new ShortPair(6, 2),
			new ShortPair(7, 2),
			new ShortPair(8, 2),
			new ShortPair(9, 2),
			new ShortPair(10, 2),
			new ShortPair(11, 2),
			new ShortPair(12, 2),
			new ShortPair(13, 2),
			new ShortPair(14, 2),
			new ShortPair(15, 2)
		}, 3000, 0u));
		_dataArray.Add(new PunishmentTypeItem(166, 315, 316, EPunishmentTypeDisplayType.Criminal, 3, 317, new List<ShortPair>
		{
			new ShortPair(1, 3),
			new ShortPair(2, 3),
			new ShortPair(3, 3),
			new ShortPair(4, 3),
			new ShortPair(5, 3),
			new ShortPair(6, 3),
			new ShortPair(7, 3),
			new ShortPair(8, 3),
			new ShortPair(9, 3),
			new ShortPair(10, 3),
			new ShortPair(11, 3),
			new ShortPair(12, 3),
			new ShortPair(13, 3),
			new ShortPair(14, 3),
			new ShortPair(15, 3)
		}, new List<ShortPair>
		{
			new ShortPair(1, 3),
			new ShortPair(2, 3),
			new ShortPair(3, 3),
			new ShortPair(4, 3),
			new ShortPair(5, 3),
			new ShortPair(6, 3),
			new ShortPair(7, 3),
			new ShortPair(8, 3),
			new ShortPair(9, 3),
			new ShortPair(10, 3),
			new ShortPair(11, 3),
			new ShortPair(12, 3),
			new ShortPair(13, 3),
			new ShortPair(14, 3),
			new ShortPair(15, 3)
		}, 4000, 0u));
		_dataArray.Add(new PunishmentTypeItem(167, 318, 319, EPunishmentTypeDisplayType.Criminal, 4, 320, new List<ShortPair>
		{
			new ShortPair(1, 4),
			new ShortPair(2, 4),
			new ShortPair(3, 4),
			new ShortPair(4, 4),
			new ShortPair(5, 4),
			new ShortPair(6, 4),
			new ShortPair(7, 4),
			new ShortPair(8, 4),
			new ShortPair(9, 4),
			new ShortPair(10, 4),
			new ShortPair(11, 4),
			new ShortPair(12, 4),
			new ShortPair(13, 4),
			new ShortPair(14, 4),
			new ShortPair(15, 4)
		}, new List<ShortPair>
		{
			new ShortPair(1, 4),
			new ShortPair(2, 4),
			new ShortPair(3, 4),
			new ShortPair(4, 4),
			new ShortPair(5, 4),
			new ShortPair(6, 4),
			new ShortPair(7, 4),
			new ShortPair(8, 4),
			new ShortPair(9, 4),
			new ShortPair(10, 4),
			new ShortPair(11, 4),
			new ShortPair(12, 4),
			new ShortPair(13, 4),
			new ShortPair(14, 4),
			new ShortPair(15, 4)
		}, 5000, 0u));
		_dataArray.Add(new PunishmentTypeItem(168, 321, 313, EPunishmentTypeDisplayType.Criminal, 2, 322, new List<ShortPair>
		{
			new ShortPair(1, 2),
			new ShortPair(2, 2),
			new ShortPair(3, 2),
			new ShortPair(4, 2),
			new ShortPair(5, 2),
			new ShortPair(6, 2),
			new ShortPair(7, 2),
			new ShortPair(8, 2),
			new ShortPair(9, 2),
			new ShortPair(10, 2),
			new ShortPair(11, 2),
			new ShortPair(12, 2),
			new ShortPair(13, 2),
			new ShortPair(14, 2),
			new ShortPair(15, 2)
		}, new List<ShortPair>(), 3000, 0u));
		_dataArray.Add(new PunishmentTypeItem(169, 323, 316, EPunishmentTypeDisplayType.Criminal, 3, 324, new List<ShortPair>
		{
			new ShortPair(1, 3),
			new ShortPair(2, 3),
			new ShortPair(3, 3),
			new ShortPair(4, 3),
			new ShortPair(5, 3),
			new ShortPair(6, 3),
			new ShortPair(7, 3),
			new ShortPair(8, 3),
			new ShortPair(9, 3),
			new ShortPair(10, 3),
			new ShortPair(11, 3),
			new ShortPair(12, 3),
			new ShortPair(13, 3),
			new ShortPair(14, 3),
			new ShortPair(15, 3)
		}, new List<ShortPair>(), 4000, 0u));
		_dataArray.Add(new PunishmentTypeItem(170, 325, 319, EPunishmentTypeDisplayType.Criminal, 4, 326, new List<ShortPair>
		{
			new ShortPair(1, 4),
			new ShortPair(2, 4),
			new ShortPair(3, 4),
			new ShortPair(4, 4),
			new ShortPair(5, 4),
			new ShortPair(6, 4),
			new ShortPair(7, 4),
			new ShortPair(8, 4),
			new ShortPair(9, 4),
			new ShortPair(10, 4),
			new ShortPair(11, 4),
			new ShortPair(12, 4),
			new ShortPair(13, 4),
			new ShortPair(14, 4),
			new ShortPair(15, 4)
		}, new List<ShortPair>(), 5000, 0u));
	}

	public override void Init()
	{
		base.Init();
		_dataArray = new List<PunishmentTypeItem>(171);
		CreateItems0();
		CreateItems1();
		CreateItems2();
	}
}
