using System;
using System.Collections.Generic;
using Config.Common;

namespace Config;

[Serializable]
public class DemandInteraction : ConfigData<DemandInteractionItem, short>
{
	public static class DefKey
	{
		public const short RequestHealOuterInjuryByItem = 0;

		public const short RequestHealInnerInjuryByItem = 1;

		public const short RequestHealPoisonByItem = 2;

		public const short RequestHealth = 3;

		public const short RequestHealDisorderOfQi = 4;

		public const short RequestNeili = 5;

		public const short RequestKillWug = 6;

		public const short RequestFood = 7;

		public const short RequestTeaWine = 8;

		public const short RequestResource = 9;

		public const short RequestItem = 10;

		public const short RequestRepairItem = 11;

		public const short RequestAddPoisonToItem = 12;

		public const short RequestInstructionOnReadingLifeSkill = 13;

		public const short RequestInstructionOnReadingCombatSkill = 15;

		public const short RequestInstructionOnBreakout = 14;
	}

	public static class DefValue
	{
		public static DemandInteractionItem RequestHealOuterInjuryByItem => Instance[(short)0];

		public static DemandInteractionItem RequestHealInnerInjuryByItem => Instance[(short)1];

		public static DemandInteractionItem RequestHealPoisonByItem => Instance[(short)2];

		public static DemandInteractionItem RequestHealth => Instance[(short)3];

		public static DemandInteractionItem RequestHealDisorderOfQi => Instance[(short)4];

		public static DemandInteractionItem RequestNeili => Instance[(short)5];

		public static DemandInteractionItem RequestKillWug => Instance[(short)6];

		public static DemandInteractionItem RequestFood => Instance[(short)7];

		public static DemandInteractionItem RequestTeaWine => Instance[(short)8];

		public static DemandInteractionItem RequestResource => Instance[(short)9];

		public static DemandInteractionItem RequestItem => Instance[(short)10];

		public static DemandInteractionItem RequestRepairItem => Instance[(short)11];

		public static DemandInteractionItem RequestAddPoisonToItem => Instance[(short)12];

		public static DemandInteractionItem RequestInstructionOnReadingLifeSkill => Instance[(short)13];

		public static DemandInteractionItem RequestInstructionOnReadingCombatSkill => Instance[(short)15];

		public static DemandInteractionItem RequestInstructionOnBreakout => Instance[(short)14];
	}

	public static DemandInteraction Instance = new DemandInteraction();

	private readonly HashSet<string> RequiredFields = new HashSet<string> { "TemplateId", "Name", "HeadEvent", "AgreeSelect", "AfterAgree" };

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
		_dataArray.Add(new DemandInteractionItem(0, 0, 1, 2, 3));
		_dataArray.Add(new DemandInteractionItem(1, 4, 5, 2, 3));
		_dataArray.Add(new DemandInteractionItem(2, 6, 7, 2, 3));
		_dataArray.Add(new DemandInteractionItem(3, 8, 9, 2, 3));
		_dataArray.Add(new DemandInteractionItem(4, 10, 11, 2, 3));
		_dataArray.Add(new DemandInteractionItem(5, 12, 13, 2, 3));
		_dataArray.Add(new DemandInteractionItem(6, 14, 15, 2, 3));
		_dataArray.Add(new DemandInteractionItem(7, 16, 17, 2, 3));
		_dataArray.Add(new DemandInteractionItem(8, 18, 19, 2, 3));
		_dataArray.Add(new DemandInteractionItem(9, 20, 21, 22, 23));
		_dataArray.Add(new DemandInteractionItem(10, 24, 25, 2, 3));
		_dataArray.Add(new DemandInteractionItem(11, 26, 27, 28, 29));
		_dataArray.Add(new DemandInteractionItem(12, 30, 31, 32, 33));
		_dataArray.Add(new DemandInteractionItem(13, 34, 35, 36, 37));
		_dataArray.Add(new DemandInteractionItem(14, 39, 40, 41, 42));
		_dataArray.Add(new DemandInteractionItem(15, 38, 35, 36, 37));
	}

	public override void Init()
	{
		base.Init();
		_dataArray = new List<DemandInteractionItem>(16);
		CreateItems0();
	}
}
