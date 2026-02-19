using System;
using System.Collections.Generic;
using Config.Common;

namespace Config;

[Serializable]
public class InteractCheckTip : ConfigData<InteractCheckTipItem, short>
{
	public static class DefKey
	{
		public const short ScamActionRecognizeTarget = 0;

		public const short ScamActionStayHidden = 1;

		public const short ScamActionWaitForGoodTiming = 2;

		public const short ScamActionOnTheWay = 3;

		public const short StealActionRecognizeTarget = 4;

		public const short StealActionStayHidden = 5;

		public const short StealActionWaitForGoodTiming = 6;

		public const short StealActionTakeAction = 7;

		public const short StealActionOnTheWay = 8;

		public const short RobActionRecognizeTarget = 9;

		public const short RobActionStayHidden = 10;

		public const short RobActionWaitForGoodTiming = 11;

		public const short RobActionTakeAction = 12;

		public const short RobActionOneTheWay = 13;

		public const short PoisonActionRecognizeTarget = 14;

		public const short PoisonActionStayHidden = 15;

		public const short PoisonActionWaitForGoodTiming = 16;

		public const short PoisonActionTakeAction = 17;

		public const short PoisonActionOneTheWay = 18;

		public const short PlotHarmActionRecognizeTarget = 19;

		public const short PlotHarmActionStayHidden = 20;

		public const short PlotHarmActionWaitForGoodTiming = 21;

		public const short PlotHarmActionTakeAction = 22;

		public const short PlotHarmActionOneTheWay = 23;

		public const short ConfessionLovePureFactor = 24;

		public const short ConfessionLoveSecularFactor = 25;

		public const short StealLifeSkillActionRecognizeTarget = 26;

		public const short StealLifeSkillActionStayHidden = 27;

		public const short StealLifeSkillActionWaitForGoodTiming = 28;

		public const short StealLifeSkillActionTakeAction = 29;

		public const short StealLifeSkillActionOnTheWay = 30;

		public const short StealCombatSkillActionRecognizeTarget = 31;

		public const short StealCombatSkillActionStayHidden = 32;

		public const short StealCombatSkillActionWaitForGoodTiming = 33;

		public const short StealCombatSkillActionTakeAction = 34;

		public const short StealCombatSkillActionOnTheWay = 35;
	}

	public static class DefValue
	{
		public static InteractCheckTipItem ScamActionRecognizeTarget => Instance[(short)0];

		public static InteractCheckTipItem ScamActionStayHidden => Instance[(short)1];

		public static InteractCheckTipItem ScamActionWaitForGoodTiming => Instance[(short)2];

		public static InteractCheckTipItem ScamActionOnTheWay => Instance[(short)3];

		public static InteractCheckTipItem StealActionRecognizeTarget => Instance[(short)4];

		public static InteractCheckTipItem StealActionStayHidden => Instance[(short)5];

		public static InteractCheckTipItem StealActionWaitForGoodTiming => Instance[(short)6];

		public static InteractCheckTipItem StealActionTakeAction => Instance[(short)7];

		public static InteractCheckTipItem StealActionOnTheWay => Instance[(short)8];

		public static InteractCheckTipItem RobActionRecognizeTarget => Instance[(short)9];

		public static InteractCheckTipItem RobActionStayHidden => Instance[(short)10];

		public static InteractCheckTipItem RobActionWaitForGoodTiming => Instance[(short)11];

		public static InteractCheckTipItem RobActionTakeAction => Instance[(short)12];

		public static InteractCheckTipItem RobActionOneTheWay => Instance[(short)13];

		public static InteractCheckTipItem PoisonActionRecognizeTarget => Instance[(short)14];

		public static InteractCheckTipItem PoisonActionStayHidden => Instance[(short)15];

		public static InteractCheckTipItem PoisonActionWaitForGoodTiming => Instance[(short)16];

		public static InteractCheckTipItem PoisonActionTakeAction => Instance[(short)17];

		public static InteractCheckTipItem PoisonActionOneTheWay => Instance[(short)18];

		public static InteractCheckTipItem PlotHarmActionRecognizeTarget => Instance[(short)19];

		public static InteractCheckTipItem PlotHarmActionStayHidden => Instance[(short)20];

		public static InteractCheckTipItem PlotHarmActionWaitForGoodTiming => Instance[(short)21];

		public static InteractCheckTipItem PlotHarmActionTakeAction => Instance[(short)22];

		public static InteractCheckTipItem PlotHarmActionOneTheWay => Instance[(short)23];

		public static InteractCheckTipItem ConfessionLovePureFactor => Instance[(short)24];

		public static InteractCheckTipItem ConfessionLoveSecularFactor => Instance[(short)25];

		public static InteractCheckTipItem StealLifeSkillActionRecognizeTarget => Instance[(short)26];

		public static InteractCheckTipItem StealLifeSkillActionStayHidden => Instance[(short)27];

		public static InteractCheckTipItem StealLifeSkillActionWaitForGoodTiming => Instance[(short)28];

		public static InteractCheckTipItem StealLifeSkillActionTakeAction => Instance[(short)29];

		public static InteractCheckTipItem StealLifeSkillActionOnTheWay => Instance[(short)30];

		public static InteractCheckTipItem StealCombatSkillActionRecognizeTarget => Instance[(short)31];

		public static InteractCheckTipItem StealCombatSkillActionStayHidden => Instance[(short)32];

		public static InteractCheckTipItem StealCombatSkillActionWaitForGoodTiming => Instance[(short)33];

		public static InteractCheckTipItem StealCombatSkillActionTakeAction => Instance[(short)34];

		public static InteractCheckTipItem StealCombatSkillActionOnTheWay => Instance[(short)35];
	}

	public static InteractCheckTip Instance = new InteractCheckTip();

	private readonly HashSet<string> RequiredFields = new HashSet<string>
	{
		"SelfCheckCharacterProperty", "TargetCheckCharacterProperty", "SelfCheckAttainmentCombatSkillType", "TargetCheckAttainmentCombatSkillType", "SelfCheckAttainmentLifeSkillType", "TargetCheckAttainmentLifeSkillType", "TemplateId", "PhaseName", "PhaseDesc", "PhaseIcon",
		"CheckDesc", "CheckResultProb", "FactorDesc"
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
		_dataArray.Add(new InteractCheckTipItem(0, 0, 1, "tex_taiwuevent_judge_1_0", 2, 2, -1, -1, -1, -1, -1, EInteractCheckTipSpecialValueDisplayType.Invalid, 3, EInteractCheckTipConfessionLoveFactorType.Invalid, null, EInteractCheckTipSpecialLineDisplayType.Invalid));
		_dataArray.Add(new InteractCheckTipItem(1, 4, 5, "tex_taiwuevent_judge_1_1", 6, -1, -1, 1, 1, -1, -1, EInteractCheckTipSpecialValueDisplayType.Invalid, 7, EInteractCheckTipConfessionLoveFactorType.Invalid, new int[1] { 8 }, EInteractCheckTipSpecialLineDisplayType.Alert));
		_dataArray.Add(new InteractCheckTipItem(2, 9, 10, "tex_taiwuevent_judge_1_2", 11, 9, 15, -1, -1, -1, -1, EInteractCheckTipSpecialValueDisplayType.Invalid, 12, EInteractCheckTipConfessionLoveFactorType.Invalid, new int[1] { 8 }, EInteractCheckTipSpecialLineDisplayType.Alert));
		_dataArray.Add(new InteractCheckTipItem(3, 13, 14, "tex_taiwuevent_judge_1_4", 15, 15, 9, -1, -1, -1, -1, EInteractCheckTipSpecialValueDisplayType.Invalid, 16, EInteractCheckTipConfessionLoveFactorType.Invalid, new int[1] { 8 }, EInteractCheckTipSpecialLineDisplayType.Alert));
		_dataArray.Add(new InteractCheckTipItem(4, 0, 17, "tex_taiwuevent_judge_1_0", 18, 1, -1, -1, -1, -1, -1, EInteractCheckTipSpecialValueDisplayType.Invalid, 3, EInteractCheckTipConfessionLoveFactorType.Invalid, null, EInteractCheckTipSpecialLineDisplayType.Invalid));
		_dataArray.Add(new InteractCheckTipItem(5, 4, 19, "tex_taiwuevent_judge_1_1", 20, -1, -1, 1, 1, -1, -1, EInteractCheckTipSpecialValueDisplayType.Invalid, 7, EInteractCheckTipConfessionLoveFactorType.Invalid, new int[1] { 8 }, EInteractCheckTipSpecialLineDisplayType.Alert));
		_dataArray.Add(new InteractCheckTipItem(6, 9, 21, "tex_taiwuevent_judge_1_2", 22, 8, 14, -1, -1, -1, -1, EInteractCheckTipSpecialValueDisplayType.Invalid, 12, EInteractCheckTipConfessionLoveFactorType.Invalid, new int[1] { 8 }, EInteractCheckTipSpecialLineDisplayType.Alert));
		_dataArray.Add(new InteractCheckTipItem(7, 23, 24, "tex_taiwuevent_judge_1_3", 25, 20, 20, -1, -1, -1, -1, EInteractCheckTipSpecialValueDisplayType.Invalid, 26, EInteractCheckTipConfessionLoveFactorType.Invalid, new int[1] { 8 }, EInteractCheckTipSpecialLineDisplayType.Alert));
		_dataArray.Add(new InteractCheckTipItem(8, 13, 27, "tex_taiwuevent_judge_1_4", 28, 14, 8, -1, -1, -1, -1, EInteractCheckTipSpecialValueDisplayType.Invalid, 16, EInteractCheckTipConfessionLoveFactorType.Invalid, new int[1] { 8 }, EInteractCheckTipSpecialLineDisplayType.Alert));
		_dataArray.Add(new InteractCheckTipItem(9, 0, 29, "tex_taiwuevent_judge_1_0", 30, 0, -1, -1, -1, -1, -1, EInteractCheckTipSpecialValueDisplayType.Invalid, 3, EInteractCheckTipConfessionLoveFactorType.Invalid, null, EInteractCheckTipSpecialLineDisplayType.Invalid));
		_dataArray.Add(new InteractCheckTipItem(10, 4, 31, "tex_taiwuevent_judge_1_1", 32, -1, -1, 1, 1, -1, -1, EInteractCheckTipSpecialValueDisplayType.Invalid, 7, EInteractCheckTipConfessionLoveFactorType.Invalid, new int[1] { 8 }, EInteractCheckTipSpecialLineDisplayType.Alert));
		_dataArray.Add(new InteractCheckTipItem(11, 9, 33, "tex_taiwuevent_judge_1_2", 34, 6, 12, -1, -1, -1, -1, EInteractCheckTipSpecialValueDisplayType.Invalid, 12, EInteractCheckTipConfessionLoveFactorType.Invalid, new int[1] { 8 }, EInteractCheckTipSpecialLineDisplayType.Alert));
		_dataArray.Add(new InteractCheckTipItem(12, 23, 35, "tex_taiwuevent_judge_1_3", 36, -1, -1, -1, -1, -1, -1, EInteractCheckTipSpecialValueDisplayType.Invalid, 26, EInteractCheckTipConfessionLoveFactorType.Invalid, new int[2] { 8, 37 }, EInteractCheckTipSpecialLineDisplayType.AlertAndPower));
		_dataArray.Add(new InteractCheckTipItem(13, 13, 38, "tex_taiwuevent_judge_1_4", 39, 12, 6, -1, -1, -1, -1, EInteractCheckTipSpecialValueDisplayType.Invalid, 16, EInteractCheckTipConfessionLoveFactorType.Invalid, new int[1] { 8 }, EInteractCheckTipSpecialLineDisplayType.Alert));
		_dataArray.Add(new InteractCheckTipItem(14, 0, 40, "tex_taiwuevent_judge_1_0", 41, -1, -1, -1, -1, 9, 9, EInteractCheckTipSpecialValueDisplayType.Invalid, 3, EInteractCheckTipConfessionLoveFactorType.Invalid, null, EInteractCheckTipSpecialLineDisplayType.Invalid));
		_dataArray.Add(new InteractCheckTipItem(15, 4, 42, "tex_taiwuevent_judge_1_1", 43, -1, -1, 1, 1, -1, -1, EInteractCheckTipSpecialValueDisplayType.Invalid, 7, EInteractCheckTipConfessionLoveFactorType.Invalid, null, EInteractCheckTipSpecialLineDisplayType.Invalid));
		_dataArray.Add(new InteractCheckTipItem(16, 9, 44, "tex_taiwuevent_judge_1_2", 45, 11, 17, -1, -1, -1, -1, EInteractCheckTipSpecialValueDisplayType.Invalid, 12, EInteractCheckTipConfessionLoveFactorType.Invalid, null, EInteractCheckTipSpecialLineDisplayType.Invalid));
		_dataArray.Add(new InteractCheckTipItem(17, 23, 46, "tex_taiwuevent_judge_1_3", 47, 22, 22, -1, -1, -1, -1, EInteractCheckTipSpecialValueDisplayType.Invalid, 26, EInteractCheckTipConfessionLoveFactorType.Invalid, null, EInteractCheckTipSpecialLineDisplayType.Invalid));
		_dataArray.Add(new InteractCheckTipItem(18, 13, 48, "tex_taiwuevent_judge_1_4", 49, 17, 11, -1, -1, -1, -1, EInteractCheckTipSpecialValueDisplayType.Invalid, 16, EInteractCheckTipConfessionLoveFactorType.Invalid, null, EInteractCheckTipSpecialLineDisplayType.Invalid));
		_dataArray.Add(new InteractCheckTipItem(19, 0, 50, "tex_taiwuevent_judge_1_0", 51, -1, -1, -1, -1, 8, 8, EInteractCheckTipSpecialValueDisplayType.Invalid, 3, EInteractCheckTipConfessionLoveFactorType.Invalid, null, EInteractCheckTipSpecialLineDisplayType.Invalid));
		_dataArray.Add(new InteractCheckTipItem(20, 4, 52, "tex_taiwuevent_judge_1_1", 43, -1, -1, 1, 1, -1, -1, EInteractCheckTipSpecialValueDisplayType.Invalid, 7, EInteractCheckTipConfessionLoveFactorType.Invalid, null, EInteractCheckTipSpecialLineDisplayType.Invalid));
		_dataArray.Add(new InteractCheckTipItem(21, 9, 53, "tex_taiwuevent_judge_1_2", 54, 10, 16, -1, -1, -1, -1, EInteractCheckTipSpecialValueDisplayType.Invalid, 12, EInteractCheckTipConfessionLoveFactorType.Invalid, null, EInteractCheckTipSpecialLineDisplayType.Invalid));
		_dataArray.Add(new InteractCheckTipItem(22, 23, 55, "tex_taiwuevent_judge_1_3", 56, 25, 25, -1, -1, -1, -1, EInteractCheckTipSpecialValueDisplayType.Invalid, 26, EInteractCheckTipConfessionLoveFactorType.Invalid, null, EInteractCheckTipSpecialLineDisplayType.Invalid));
		_dataArray.Add(new InteractCheckTipItem(23, 13, 57, "tex_taiwuevent_judge_1_4", 58, 16, 10, -1, -1, -1, -1, EInteractCheckTipSpecialValueDisplayType.Invalid, 16, EInteractCheckTipConfessionLoveFactorType.Invalid, null, EInteractCheckTipSpecialLineDisplayType.Invalid));
		_dataArray.Add(new InteractCheckTipItem(24, 59, 60, "tex_taiwuevent_judge_0_1", 61, -1, -1, -1, -1, -1, -1, EInteractCheckTipSpecialValueDisplayType.Invalid, 62, EInteractCheckTipConfessionLoveFactorType.ConfessionLovePure, new int[7] { 63, 64, 65, 66, 67, 68, 69 }, EInteractCheckTipSpecialLineDisplayType.LovePure));
		_dataArray.Add(new InteractCheckTipItem(25, 70, 71, "tex_taiwuevent_judge_0_2", 72, -1, -1, -1, -1, -1, -1, EInteractCheckTipSpecialValueDisplayType.Invalid, 62, EInteractCheckTipConfessionLoveFactorType.ConfessionLoveSecular, new int[7] { 73, 74, 75, 76, 77, 78, 79 }, EInteractCheckTipSpecialLineDisplayType.LoveSecular));
		_dataArray.Add(new InteractCheckTipItem(26, 0, 80, "tex_taiwuevent_judge_1_0", 81, 5, -1, -1, -1, -1, -1, EInteractCheckTipSpecialValueDisplayType.Invalid, 3, EInteractCheckTipConfessionLoveFactorType.Invalid, null, EInteractCheckTipSpecialLineDisplayType.Invalid));
		_dataArray.Add(new InteractCheckTipItem(27, 4, 82, "tex_taiwuevent_judge_1_1", 83, -1, -1, 1, 1, -1, -1, EInteractCheckTipSpecialValueDisplayType.Invalid, 7, EInteractCheckTipConfessionLoveFactorType.Invalid, new int[1] { 8 }, EInteractCheckTipSpecialLineDisplayType.Alert));
		_dataArray.Add(new InteractCheckTipItem(28, 9, 84, "tex_taiwuevent_judge_1_2", 85, 7, 13, -1, -1, -1, -1, EInteractCheckTipSpecialValueDisplayType.Invalid, 12, EInteractCheckTipConfessionLoveFactorType.Invalid, new int[1] { 8 }, EInteractCheckTipSpecialLineDisplayType.Alert));
		_dataArray.Add(new InteractCheckTipItem(29, 23, 86, "tex_taiwuevent_judge_1_3", 87, -1, -1, -1, -1, -1, -1, EInteractCheckTipSpecialValueDisplayType.StealSkillLifeSkillQualities, 26, EInteractCheckTipConfessionLoveFactorType.Invalid, new int[1] { 8 }, EInteractCheckTipSpecialLineDisplayType.Alert));
		_dataArray.Add(new InteractCheckTipItem(30, 13, 88, "tex_taiwuevent_judge_1_4", 89, 13, 7, -1, -1, -1, -1, EInteractCheckTipSpecialValueDisplayType.Invalid, 16, EInteractCheckTipConfessionLoveFactorType.Invalid, new int[1] { 8 }, EInteractCheckTipSpecialLineDisplayType.Alert));
		_dataArray.Add(new InteractCheckTipItem(31, 0, 80, "tex_taiwuevent_judge_1_0", 81, 5, -1, -1, -1, -1, -1, EInteractCheckTipSpecialValueDisplayType.Invalid, 3, EInteractCheckTipConfessionLoveFactorType.Invalid, null, EInteractCheckTipSpecialLineDisplayType.Invalid));
		_dataArray.Add(new InteractCheckTipItem(32, 4, 82, "tex_taiwuevent_judge_1_1", 83, -1, -1, 1, 1, -1, -1, EInteractCheckTipSpecialValueDisplayType.Invalid, 7, EInteractCheckTipConfessionLoveFactorType.Invalid, new int[1] { 8 }, EInteractCheckTipSpecialLineDisplayType.Alert));
		_dataArray.Add(new InteractCheckTipItem(33, 9, 84, "tex_taiwuevent_judge_1_2", 85, 7, 13, -1, -1, -1, -1, EInteractCheckTipSpecialValueDisplayType.Invalid, 12, EInteractCheckTipConfessionLoveFactorType.Invalid, new int[1] { 8 }, EInteractCheckTipSpecialLineDisplayType.Alert));
		_dataArray.Add(new InteractCheckTipItem(34, 23, 86, "tex_taiwuevent_judge_1_3", 87, -1, -1, -1, -1, -1, -1, EInteractCheckTipSpecialValueDisplayType.StealSkillCombatSkillQualities, 26, EInteractCheckTipConfessionLoveFactorType.Invalid, new int[1] { 8 }, EInteractCheckTipSpecialLineDisplayType.Alert));
		_dataArray.Add(new InteractCheckTipItem(35, 13, 88, "tex_taiwuevent_judge_1_4", 89, 13, 7, -1, -1, -1, -1, EInteractCheckTipSpecialValueDisplayType.Invalid, 16, EInteractCheckTipConfessionLoveFactorType.Invalid, new int[1] { 8 }, EInteractCheckTipSpecialLineDisplayType.Alert));
	}

	public override void Init()
	{
		base.Init();
		_dataArray = new List<InteractCheckTipItem>(36);
		CreateItems0();
	}
}
