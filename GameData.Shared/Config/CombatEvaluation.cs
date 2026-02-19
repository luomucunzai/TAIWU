using System;
using System.Collections.Generic;
using Config.Common;
using GameData.Domains.Taiwu;

namespace Config;

[Serializable]
public class CombatEvaluation : ConfigData<CombatEvaluationItem, sbyte>
{
	public static class DefKey
	{
		public const sbyte SaveInfection0 = 23;

		public const sbyte SaveInfection1 = 24;

		public const sbyte TaiZuChangQuan = 32;

		public const sbyte ReadInCombat = 33;

		public const sbyte QiArtInCombat = 43;

		public const sbyte SurrenderInCombat = 45;
	}

	public static class DefValue
	{
		public static CombatEvaluationItem SaveInfection0 => Instance[(sbyte)23];

		public static CombatEvaluationItem SaveInfection1 => Instance[(sbyte)24];

		public static CombatEvaluationItem TaiZuChangQuan => Instance[(sbyte)32];

		public static CombatEvaluationItem ReadInCombat => Instance[(sbyte)33];

		public static CombatEvaluationItem QiArtInCombat => Instance[(sbyte)43];

		public static CombatEvaluationItem SurrenderInCombat => Instance[(sbyte)45];
	}

	public static CombatEvaluation Instance = new CombatEvaluation();

	private readonly HashSet<string> RequiredFields = new HashSet<string> { "RequireCombatConfigs", "FameAction", "AddLegacyPoint", "TemplateId", "Name", "Desc", "SmallVillageDesc" };

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
		_dataArray.Add(new CombatEvaluationItem(0, 0, 1, 2, new List<short>(), new sbyte[4] { 0, 1, 2, 3 }, needWin: false, availableInPlayground: false, ECombatEvaluationExtraCheck.Fail, 0, -25, 0, -100, allowProficiency: true, 0, -1, -50, -50, new List<LegacyPointReference>()));
		_dataArray.Add(new CombatEvaluationItem(1, 3, 4, 5, new List<short>(), new sbyte[4] { 0, 1, 2, 3 }, needWin: false, availableInPlayground: false, ECombatEvaluationExtraCheck.Draw, 0, 0, 0, 0, allowProficiency: true, 0, -1, 0, 0, new List<LegacyPointReference>()));
		_dataArray.Add(new CombatEvaluationItem(2, 6, 7, 8, new List<short>(), new sbyte[4] { 0, 1, 2, 3 }, needWin: false, availableInPlayground: false, ECombatEvaluationExtraCheck.Flee, 0, -50, 0, -100, allowProficiency: true, 0, 33, -1000, -1000, new List<LegacyPointReference>()));
		_dataArray.Add(new CombatEvaluationItem(3, 9, 10, 11, new List<short>(), new sbyte[4] { 0, 1, 2, 3 }, needWin: true, availableInPlayground: false, ECombatEvaluationExtraCheck.Win, 0, 0, 0, 0, allowProficiency: true, 0, -1, 0, 0, new List<LegacyPointReference>()));
		_dataArray.Add(new CombatEvaluationItem(4, 12, 13, 14, new List<short>(), new sbyte[1], needWin: false, availableInPlayground: false, ECombatEvaluationExtraCheck.FightSameLevel, 0, 0, 0, 0, allowProficiency: true, 0, -1, 0, 0, new List<LegacyPointReference>
		{
			new LegacyPointReference(8, 50, 0),
			new LegacyPointReference(9, -9999, 0),
			new LegacyPointReference(10, -9999, 0),
			new LegacyPointReference(11, -9999, 0)
		}));
		_dataArray.Add(new CombatEvaluationItem(5, 15, 16, 17, new List<short>(), new sbyte[1] { 3 }, needWin: false, availableInPlayground: false, ECombatEvaluationExtraCheck.FightSameLevel, 0, 25, 0, 25, allowProficiency: true, 0, -1, 0, 0, new List<LegacyPointReference>
		{
			new LegacyPointReference(8, -9999, 0),
			new LegacyPointReference(9, 50, 0),
			new LegacyPointReference(10, -9999, 0),
			new LegacyPointReference(11, -9999, 0)
		}));
		_dataArray.Add(new CombatEvaluationItem(6, 18, 19, 20, new List<short>(), new sbyte[1] { 1 }, needWin: false, availableInPlayground: false, ECombatEvaluationExtraCheck.FightSameLevel, 0, 50, 0, 50, allowProficiency: true, 0, -1, 0, 0, new List<LegacyPointReference>
		{
			new LegacyPointReference(8, -9999, 0),
			new LegacyPointReference(9, -9999, 0),
			new LegacyPointReference(10, 50, 0),
			new LegacyPointReference(11, -9999, 0)
		}));
		_dataArray.Add(new CombatEvaluationItem(7, 21, 22, 23, new List<short>(), new sbyte[1] { 2 }, needWin: false, availableInPlayground: false, ECombatEvaluationExtraCheck.FightSameLevel, 0, 100, 0, 100, allowProficiency: true, 0, -1, 0, 0, new List<LegacyPointReference>
		{
			new LegacyPointReference(8, -9999, 0),
			new LegacyPointReference(9, -9999, 0),
			new LegacyPointReference(10, -9999, 0),
			new LegacyPointReference(11, 50, 0)
		}));
		_dataArray.Add(new CombatEvaluationItem(8, 24, 25, 26, new List<short>(), new sbyte[4] { 0, 1, 2, 3 }, needWin: true, availableInPlayground: false, ECombatEvaluationExtraCheck.BeatXiangShu, 5000, 0, 5000, 0, allowProficiency: true, 0, -1, 100, 100, new List<LegacyPointReference>
		{
			new LegacyPointReference(41, 100, 0)
		}));
		_dataArray.Add(new CombatEvaluationItem(9, 27, 28, 29, new List<short>(), new sbyte[2] { 1, 2 }, needWin: true, availableInPlayground: false, ECombatEvaluationExtraCheck.WinLess, -25, 0, -25, 0, allowProficiency: true, 0, -1, -25, -25, new List<LegacyPointReference>()));
		_dataArray.Add(new CombatEvaluationItem(10, 30, 31, 32, new List<short>(), new sbyte[2] { 1, 2 }, needWin: true, availableInPlayground: false, ECombatEvaluationExtraCheck.WinChild, -25, 0, -25, 0, allowProficiency: true, 0, 34, -25, -25, new List<LegacyPointReference>()));
		_dataArray.Add(new CombatEvaluationItem(11, 33, 34, 35, new List<short>(), new sbyte[2] { 1, 2 }, needWin: true, availableInPlayground: false, ECombatEvaluationExtraCheck.WinWorseEquip, -25, 0, -25, 0, allowProficiency: true, 0, -1, -25, -25, new List<LegacyPointReference>()));
		_dataArray.Add(new CombatEvaluationItem(12, 36, 37, 38, new List<short>(), new sbyte[2] { 1, 2 }, needWin: true, availableInPlayground: false, ECombatEvaluationExtraCheck.WinLessNeili, -25, 0, -25, 0, allowProficiency: true, 0, -1, -25, -25, new List<LegacyPointReference>()));
		_dataArray.Add(new CombatEvaluationItem(13, 39, 40, 41, new List<short>(), new sbyte[2] { 1, 2 }, needWin: true, availableInPlayground: false, ECombatEvaluationExtraCheck.WinWorseSkill, -25, 0, -25, 0, allowProficiency: true, 0, -1, -25, -25, new List<LegacyPointReference>()));
		_dataArray.Add(new CombatEvaluationItem(14, 42, 43, 44, new List<short>(), new sbyte[2] { 1, 2 }, needWin: true, availableInPlayground: false, ECombatEvaluationExtraCheck.WinLessConsummate, -50, 0, -50, 0, allowProficiency: true, 0, -1, -25, -25, new List<LegacyPointReference>()));
		_dataArray.Add(new CombatEvaluationItem(15, 45, 46, 47, new List<short>(), new sbyte[2] { 1, 2 }, needWin: true, availableInPlayground: false, ECombatEvaluationExtraCheck.WinPregnant, -75, 0, -75, 0, allowProficiency: true, 0, 35, -25, -25, new List<LegacyPointReference>()));
		_dataArray.Add(new CombatEvaluationItem(16, 48, 49, 50, new List<short>(), new sbyte[2] { 1, 2 }, needWin: true, availableInPlayground: false, ECombatEvaluationExtraCheck.WinMore, 25, 0, 25, 0, allowProficiency: true, 0, -1, 25, 25, new List<LegacyPointReference>()));
		_dataArray.Add(new CombatEvaluationItem(17, 51, 52, 53, new List<short>(), new sbyte[2] { 1, 2 }, needWin: true, availableInPlayground: false, ECombatEvaluationExtraCheck.WinOlder, 25, 0, 25, 0, allowProficiency: true, 0, -1, 25, 25, new List<LegacyPointReference>
		{
			new LegacyPointReference(8, 20, 0),
			new LegacyPointReference(9, 20, 0),
			new LegacyPointReference(10, 20, 0),
			new LegacyPointReference(11, 20, 0)
		}));
		_dataArray.Add(new CombatEvaluationItem(18, 54, 55, 56, new List<short>(), new sbyte[2] { 1, 2 }, needWin: true, availableInPlayground: false, ECombatEvaluationExtraCheck.WinBetterEquip, 25, 0, 25, 0, allowProficiency: true, 0, -1, 25, 25, new List<LegacyPointReference>
		{
			new LegacyPointReference(8, 20, 0),
			new LegacyPointReference(9, 20, 0),
			new LegacyPointReference(10, 20, 0),
			new LegacyPointReference(11, 20, 0)
		}));
		_dataArray.Add(new CombatEvaluationItem(19, 57, 58, 59, new List<short>(), new sbyte[2] { 1, 2 }, needWin: true, availableInPlayground: false, ECombatEvaluationExtraCheck.WinMoreNeili, 25, 0, 25, 0, allowProficiency: true, 0, -1, 25, 25, new List<LegacyPointReference>
		{
			new LegacyPointReference(8, 20, 0),
			new LegacyPointReference(9, 20, 0),
			new LegacyPointReference(10, 20, 0),
			new LegacyPointReference(11, 20, 0)
		}));
		_dataArray.Add(new CombatEvaluationItem(20, 60, 61, 62, new List<short>(), new sbyte[2] { 1, 2 }, needWin: true, availableInPlayground: false, ECombatEvaluationExtraCheck.WinBetterSkill, 25, 0, 25, 0, allowProficiency: true, 0, -1, 25, 25, new List<LegacyPointReference>
		{
			new LegacyPointReference(8, 20, 0),
			new LegacyPointReference(9, 20, 0),
			new LegacyPointReference(10, 20, 0),
			new LegacyPointReference(11, 20, 0)
		}));
		_dataArray.Add(new CombatEvaluationItem(21, 63, 64, 65, new List<short>(), new sbyte[2] { 1, 2 }, needWin: true, availableInPlayground: false, ECombatEvaluationExtraCheck.WinMoreConsummate, 50, 0, 50, 0, allowProficiency: true, 0, -1, 25, 25, new List<LegacyPointReference>
		{
			new LegacyPointReference(8, 20, 0),
			new LegacyPointReference(9, 20, 0),
			new LegacyPointReference(10, 20, 0),
			new LegacyPointReference(11, 20, 0)
		}));
		_dataArray.Add(new CombatEvaluationItem(22, 66, 67, 68, new List<short>(), new sbyte[2] { 1, 2 }, needWin: true, availableInPlayground: false, ECombatEvaluationExtraCheck.WinInPregnant, 75, 0, 75, 0, allowProficiency: true, 0, -1, 25, 25, new List<LegacyPointReference>()));
		_dataArray.Add(new CombatEvaluationItem(23, 69, 70, 71, new List<short>(), new sbyte[4] { 0, 1, 2, 3 }, needWin: true, availableInPlayground: false, ECombatEvaluationExtraCheck.Extern, 25, 0, 25, 0, allowProficiency: true, 25, -1, 0, 0, new List<LegacyPointReference>()));
		_dataArray.Add(new CombatEvaluationItem(24, 69, 72, 73, new List<short>(), new sbyte[4] { 0, 1, 2, 3 }, needWin: true, availableInPlayground: false, ECombatEvaluationExtraCheck.Extern, 100, 0, 100, 0, allowProficiency: true, 100, -1, 0, 0, new List<LegacyPointReference>()));
		_dataArray.Add(new CombatEvaluationItem(25, 74, 75, 76, new List<short>(), new sbyte[4] { 0, 1, 2, 3 }, needWin: true, availableInPlayground: false, ECombatEvaluationExtraCheck.KillBad0, 0, 0, 50, 0, allowProficiency: true, 0, 59, 0, 0, new List<LegacyPointReference>()));
		_dataArray.Add(new CombatEvaluationItem(26, 77, 78, 79, new List<short>(), new sbyte[4] { 0, 1, 2, 3 }, needWin: true, availableInPlayground: false, ECombatEvaluationExtraCheck.KillBad1, 0, 0, 100, 0, allowProficiency: true, 0, 59, 0, 0, new List<LegacyPointReference>()));
		_dataArray.Add(new CombatEvaluationItem(27, 80, 81, 82, new List<short>(), new sbyte[4] { 0, 1, 2, 3 }, needWin: true, availableInPlayground: false, ECombatEvaluationExtraCheck.KillGood0, 0, 0, -100, 0, allowProficiency: true, 0, 60, 0, 0, new List<LegacyPointReference>()));
		_dataArray.Add(new CombatEvaluationItem(28, 83, 84, 85, new List<short>(), new sbyte[4] { 0, 1, 2, 3 }, needWin: true, availableInPlayground: false, ECombatEvaluationExtraCheck.KillGood1, 0, 0, -50, 0, allowProficiency: true, 0, 60, 0, 0, new List<LegacyPointReference>()));
		_dataArray.Add(new CombatEvaluationItem(29, 86, 87, 88, new List<short>(), new sbyte[4] { 0, 1, 2, 3 }, needWin: true, availableInPlayground: false, ECombatEvaluationExtraCheck.ShixiangBuff0, 0, 0, 50, 0, allowProficiency: true, 0, -1, 0, 0, new List<LegacyPointReference>()));
		_dataArray.Add(new CombatEvaluationItem(30, 86, 87, 89, new List<short>(), new sbyte[4] { 0, 1, 2, 3 }, needWin: true, availableInPlayground: false, ECombatEvaluationExtraCheck.ShixiangBuff1, 0, 0, 75, 0, allowProficiency: true, 0, -1, 0, 0, new List<LegacyPointReference>()));
		_dataArray.Add(new CombatEvaluationItem(31, 86, 87, 90, new List<short>(), new sbyte[4] { 0, 1, 2, 3 }, needWin: true, availableInPlayground: false, ECombatEvaluationExtraCheck.ShixiangBuff2, 0, 0, 100, 0, allowProficiency: true, 0, -1, 0, 0, new List<LegacyPointReference>()));
		_dataArray.Add(new CombatEvaluationItem(32, 91, 92, 93, new List<short>(), new sbyte[4] { 0, 1, 2, 3 }, needWin: true, availableInPlayground: false, ECombatEvaluationExtraCheck.Extern, 0, 0, 100, 0, allowProficiency: true, 0, -1, 0, 0, new List<LegacyPointReference>()));
		_dataArray.Add(new CombatEvaluationItem(33, 94, 95, 96, new List<short>(), new sbyte[4] { 0, 1, 2, 3 }, needWin: true, availableInPlayground: false, ECombatEvaluationExtraCheck.Extern, 0, 0, 0, 0, allowProficiency: true, 0, -1, 0, 0, new List<LegacyPointReference>()));
		_dataArray.Add(new CombatEvaluationItem(34, 97, 98, 99, new List<short>(), new sbyte[4] { 0, 1, 2, 3 }, needWin: false, availableInPlayground: false, ECombatEvaluationExtraCheck.PuppetCombat, 0, -75, 0, -100, allowProficiency: true, 0, -1, 50, 50, new List<LegacyPointReference>()));
		_dataArray.Add(new CombatEvaluationItem(35, 100, 101, 102, new List<short>(), new sbyte[4] { 0, 1, 2, 3 }, needWin: true, availableInPlayground: false, ECombatEvaluationExtraCheck.WinLoong, 1500, 0, 1500, 0, allowProficiency: true, 0, 78, 100, 100, new List<LegacyPointReference>()));
		_dataArray.Add(new CombatEvaluationItem(36, 103, 104, 105, new List<short>(), new sbyte[4] { 0, 1, 2, 3 }, needWin: false, availableInPlayground: false, ECombatEvaluationExtraCheck.CombatHard, 100, 0, 100, 0, allowProficiency: true, 0, -1, 25, 25, new List<LegacyPointReference>()));
		_dataArray.Add(new CombatEvaluationItem(37, 106, 107, 108, new List<short>(), new sbyte[4] { 0, 1, 2, 3 }, needWin: false, availableInPlayground: false, ECombatEvaluationExtraCheck.CombatVeryHard, 200, 0, 200, 0, allowProficiency: true, 0, -1, 50, 50, new List<LegacyPointReference>()));
		_dataArray.Add(new CombatEvaluationItem(38, 109, 110, 111, new List<short>(), new sbyte[4] { 0, 1, 2, 3 }, needWin: false, availableInPlayground: false, ECombatEvaluationExtraCheck.OutBossCombat, 500, 0, 500, 0, allowProficiency: true, 0, -1, 20, 20, new List<LegacyPointReference>
		{
			new LegacyPointReference(40, 100, 50)
		}));
		_dataArray.Add(new CombatEvaluationItem(39, 112, 113, 114, new List<short>(), new sbyte[1], needWin: false, availableInPlayground: false, ECombatEvaluationExtraCheck.FightLessLevel, 0, -75, 0, -75, allowProficiency: true, 0, -1, 0, 0, new List<LegacyPointReference>
		{
			new LegacyPointReference(8, -9999, 0),
			new LegacyPointReference(9, -9999, 0),
			new LegacyPointReference(10, -9999, 0),
			new LegacyPointReference(11, -9999, 0)
		}));
		_dataArray.Add(new CombatEvaluationItem(40, 115, 116, 117, new List<short>(), new sbyte[1] { 3 }, needWin: false, availableInPlayground: false, ECombatEvaluationExtraCheck.FightLessLevel, 0, -50, 0, -50, allowProficiency: true, 0, -1, 0, 0, new List<LegacyPointReference>
		{
			new LegacyPointReference(8, -9999, 0),
			new LegacyPointReference(9, -9999, 0),
			new LegacyPointReference(10, -9999, 0),
			new LegacyPointReference(11, -9999, 0)
		}));
		_dataArray.Add(new CombatEvaluationItem(41, 118, 119, 120, new List<short>(), new sbyte[1] { 1 }, needWin: false, availableInPlayground: false, ECombatEvaluationExtraCheck.FightLessLevel, 0, -25, 0, -25, allowProficiency: true, 0, -1, 0, 0, new List<LegacyPointReference>
		{
			new LegacyPointReference(8, -9999, 0),
			new LegacyPointReference(9, -9999, 0),
			new LegacyPointReference(10, -9999, 0),
			new LegacyPointReference(11, -9999, 0)
		}));
		_dataArray.Add(new CombatEvaluationItem(42, 121, 122, 123, new List<short>(), new sbyte[1] { 2 }, needWin: false, availableInPlayground: false, ECombatEvaluationExtraCheck.FightLessLevel, 0, 0, 0, 0, allowProficiency: true, 0, -1, 0, 0, new List<LegacyPointReference>
		{
			new LegacyPointReference(8, -9999, 0),
			new LegacyPointReference(9, -9999, 0),
			new LegacyPointReference(10, -9999, 0),
			new LegacyPointReference(11, -9999, 0)
		}));
		_dataArray.Add(new CombatEvaluationItem(43, 124, 125, 126, new List<short>(), new sbyte[4] { 0, 1, 2, 3 }, needWin: true, availableInPlayground: false, ECombatEvaluationExtraCheck.Extern, 0, 0, 0, 0, allowProficiency: true, 0, -1, 0, 0, new List<LegacyPointReference>()));
		_dataArray.Add(new CombatEvaluationItem(44, 127, 128, 129, new List<short>(), new sbyte[4] { 0, 1, 2, 3 }, needWin: false, availableInPlayground: true, ECombatEvaluationExtraCheck.None, 0, 0, 0, 0, allowProficiency: true, 0, -1, 0, 0, new List<LegacyPointReference>()));
		_dataArray.Add(new CombatEvaluationItem(45, 130, 131, 132, new List<short>(), new sbyte[4] { 0, 1, 2, 3 }, needWin: false, availableInPlayground: false, ECombatEvaluationExtraCheck.Extern, 0, -75, 0, -100, allowProficiency: true, 0, 84, -1000, -1000, new List<LegacyPointReference>()));
		_dataArray.Add(new CombatEvaluationItem(46, 133, 134, 135, new List<short>(), new sbyte[4] { 0, 1, 2, 3 }, needWin: true, availableInPlayground: false, ECombatEvaluationExtraCheck.KillMinion0, 25, 0, 25, 0, allowProficiency: true, 0, -1, 0, 0, new List<LegacyPointReference>()));
		_dataArray.Add(new CombatEvaluationItem(47, 133, 136, 137, new List<short>(), new sbyte[4] { 0, 1, 2, 3 }, needWin: true, availableInPlayground: false, ECombatEvaluationExtraCheck.KillMinion1, 100, 0, 100, 0, allowProficiency: true, 0, -1, 0, 0, new List<LegacyPointReference>()));
		_dataArray.Add(new CombatEvaluationItem(48, 138, 139, 140, new List<short>
		{
			211, 212, 213, 214, 215, 216, 217, 218, 219, 220,
			221, 222
		}, new sbyte[4] { 0, 1, 2, 3 }, needWin: false, availableInPlayground: false, ECombatEvaluationExtraCheck.Fail, 0, -1000, 0, -1000, allowProficiency: false, 0, -1, 0, 0, new List<LegacyPointReference>()));
	}

	public override void Init()
	{
		base.Init();
		_dataArray = new List<CombatEvaluationItem>(49);
		CreateItems0();
	}
}
