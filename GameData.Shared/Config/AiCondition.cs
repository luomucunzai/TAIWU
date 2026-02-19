using System;
using System.Collections.Generic;
using Config.Common;

namespace Config;

[Serializable]
public class AiCondition : ConfigData<AiConditionItem, int>
{
	public static AiCondition Instance = new AiCondition();

	private readonly HashSet<string> RequiredFields = new HashSet<string> { "ParamStrings", "ParamInts", "GroupId", "TemplateId", "Type", "Name", "Desc" };

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
		_dataArray.Add(new AiConditionItem(0, EAiConditionType.Delay, 0, 1, new List<int> { 11 }, null, 0));
		_dataArray.Add(new AiConditionItem(1, EAiConditionType.CheckPercentProb, 2, 3, new List<int> { 11 }, null, 0));
		_dataArray.Add(new AiConditionItem(2, EAiConditionType.First, 4, 5, null, null, 0));
		_dataArray.Add(new AiConditionItem(3, EAiConditionType.EquipCombatSkill, 6, 7, null, new List<int> { 3, 2 }, 1));
		_dataArray.Add(new AiConditionItem(4, EAiConditionType.BreakCombatSkill, 8, 9, null, new List<int> { 3, 2, 12 }, 1));
		_dataArray.Add(new AiConditionItem(5, EAiConditionType.LearnCombatSkill, 10, 11, null, new List<int> { 3, 2 }, 1));
		_dataArray.Add(new AiConditionItem(6, EAiConditionType.InCurrentAttackRange, 12, 13, null, new List<int> { 3, 0 }, 1));
		_dataArray.Add(new AiConditionItem(7, EAiConditionType.InCombatSkillRange, 14, 15, null, new List<int> { 3, 0, 2 }, 1));
		_dataArray.Add(new AiConditionItem(8, EAiConditionType.AnyAttackRangeEdge, 16, 17, null, new List<int> { 3 }, 1));
		_dataArray.Add(new AiConditionItem(9, EAiConditionType.MemoryEqualString, 18, 19, new List<int> { 4, 4 }, null, 0));
		_dataArray.Add(new AiConditionItem(10, EAiConditionType.MemoryEqualBoolean, 20, 19, new List<int> { 4 }, new List<int> { 1 }, 0));
		_dataArray.Add(new AiConditionItem(11, EAiConditionType.MemoryEqual, 21, 19, new List<int> { 4, 11 }, null, 0));
		_dataArray.Add(new AiConditionItem(12, EAiConditionType.MemoryNotEqualString, 22, 23, new List<int> { 4, 4 }, null, 0));
		_dataArray.Add(new AiConditionItem(13, EAiConditionType.MemoryNotEqualBoolean, 24, 23, new List<int> { 4 }, new List<int> { 1 }, 0));
		_dataArray.Add(new AiConditionItem(14, EAiConditionType.MemoryNotEqual, 25, 23, new List<int> { 4, 11 }, null, 0));
		_dataArray.Add(new AiConditionItem(15, EAiConditionType.MemoryAbove, 26, 27, new List<int> { 4, 11 }, null, 0));
		_dataArray.Add(new AiConditionItem(16, EAiConditionType.MemoryBelow, 28, 29, new List<int> { 4, 11 }, null, 0));
		_dataArray.Add(new AiConditionItem(17, EAiConditionType.CombatDifficulty, 30, 31, null, new List<int> { 5 }, 1));
		_dataArray.Add(new AiConditionItem(18, EAiConditionType.TargetDistanceNearby, 32, 33, null, new List<int> { 9 }, 1));
		_dataArray.Add(new AiConditionItem(19, EAiConditionType.TargetDistanceIsNot, 34, 35, null, new List<int> { 0 }, 1));
		_dataArray.Add(new AiConditionItem(20, EAiConditionType.TargetDistanceIsNotFarthest, 36, 37, null, null, 1));
		_dataArray.Add(new AiConditionItem(21, EAiConditionType.CastingSkillType, 38, 39, null, new List<int> { 3, 7 }, 1));
		_dataArray.Add(new AiConditionItem(22, EAiConditionType.CastingSkill, 40, 41, null, new List<int> { 3, 2 }, 1));
		_dataArray.Add(new AiConditionItem(23, EAiConditionType.CastingProgressMoreOrEqual, 42, 43, new List<int> { 11 }, new List<int> { 3 }, 1));
		_dataArray.Add(new AiConditionItem(24, EAiConditionType.CastingProgressLess, 44, 45, new List<int> { 11 }, new List<int> { 3 }, 1));
		_dataArray.Add(new AiConditionItem(25, EAiConditionType.BlockPercentLess, 46, 47, new List<int> { 11 }, new List<int> { 3 }, 1));
		_dataArray.Add(new AiConditionItem(26, EAiConditionType.IsCharacterHalfFallen, 48, 49, null, new List<int> { 3 }, 1));
		_dataArray.Add(new AiConditionItem(27, EAiConditionType.CheckBanFlee, 50, 51, null, null, 1));
		_dataArray.Add(new AiConditionItem(28, EAiConditionType.CheckFleeNormal, 52, 53, null, null, 1));
		_dataArray.Add(new AiConditionItem(29, EAiConditionType.InHazard, 54, 55, null, null, 1));
		_dataArray.Add(new AiConditionItem(30, EAiConditionType.OptionAttack, 56, 56, null, null, 1));
		_dataArray.Add(new AiConditionItem(31, EAiConditionType.OptionChangeTrick, 57, 57, null, null, 1));
		_dataArray.Add(new AiConditionItem(32, EAiConditionType.OptionChangeTrickFlaw, 58, 58, null, null, 1));
		_dataArray.Add(new AiConditionItem(33, EAiConditionType.OptionChangeTrickAcupoint, 59, 60, null, new List<int> { 10 }, 1));
		_dataArray.Add(new AiConditionItem(34, EAiConditionType.OptionChangeTrickNeiliType, 61, 61, null, null, 1));
		_dataArray.Add(new AiConditionItem(35, EAiConditionType.OptionChangeWeapon, 62, 63, null, new List<int> { 0, 0 }, 1));
		_dataArray.Add(new AiConditionItem(36, EAiConditionType.OptionTryDodge, 64, 64, null, null, 1));
		_dataArray.Add(new AiConditionItem(37, EAiConditionType.OptionOtherAction, 65, 66, null, new List<int> { 8 }, 1));
		_dataArray.Add(new AiConditionItem(38, EAiConditionType.OptionTeammateCommand, 67, 68, null, new List<int> { 6 }, 1));
		_dataArray.Add(new AiConditionItem(39, EAiConditionType.OptionProactiveSkillType, 69, 70, null, new List<int> { 7 }, 1));
		_dataArray.Add(new AiConditionItem(40, EAiConditionType.OptionCastBoost, 71, 71, null, null, 1));
		_dataArray.Add(new AiConditionItem(41, EAiConditionType.OptionCastDefendBlock, 72, 72, null, null, 1));
		_dataArray.Add(new AiConditionItem(42, EAiConditionType.OptionCastAgileBuff, 73, 73, null, null, 1));
		_dataArray.Add(new AiConditionItem(43, EAiConditionType.OptionUseItemHealInjury, 74, 74, null, null, 1));
		_dataArray.Add(new AiConditionItem(44, EAiConditionType.OptionUseItemHealPoison, 75, 75, null, null, 1));
		_dataArray.Add(new AiConditionItem(45, EAiConditionType.OptionUseItemHealQiDisorder, 76, 76, null, null, 1));
		_dataArray.Add(new AiConditionItem(46, EAiConditionType.OptionUseItemBuff, 77, 77, null, null, 1));
		_dataArray.Add(new AiConditionItem(47, EAiConditionType.OptionUseItemPoison, 78, 78, null, null, 1));
		_dataArray.Add(new AiConditionItem(48, EAiConditionType.OptionUseItemNeili, 79, 79, null, null, 1));
		_dataArray.Add(new AiConditionItem(49, EAiConditionType.OptionUseItemWine, 80, 80, null, null, 1));
		_dataArray.Add(new AiConditionItem(50, EAiConditionType.OptionUseItemRepairWeapon, 81, 81, null, null, 1));
		_dataArray.Add(new AiConditionItem(51, EAiConditionType.OptionUseItemRepairArmor, 82, 82, null, null, 1));
		_dataArray.Add(new AiConditionItem(52, EAiConditionType.CombatTypeEqual, 83, 84, null, new List<int> { 23 }, 1));
		_dataArray.Add(new AiConditionItem(53, EAiConditionType.AnyAffectingAgile, 85, 86, null, new List<int> { 3 }, 1));
		_dataArray.Add(new AiConditionItem(54, EAiConditionType.SpecialAffectingAgile, 87, 88, null, new List<int> { 3, 12, 2 }, 1));
		_dataArray.Add(new AiConditionItem(55, EAiConditionType.AnyAffectingDefense, 89, 90, null, new List<int> { 3 }, 1));
		_dataArray.Add(new AiConditionItem(56, EAiConditionType.SpecialAffectingDefense, 91, 92, null, new List<int> { 3, 12, 2 }, 1));
		_dataArray.Add(new AiConditionItem(57, EAiConditionType.IsTaiwu, 93, 94, null, null, 1));
		_dataArray.Add(new AiConditionItem(58, EAiConditionType.InjuryMarkCountMoreOrEqual, 95, 96, null, new List<int> { 3, 0, 10, 13 }, 1));
		_dataArray.Add(new AiConditionItem(59, EAiConditionType.FlawMarkCountMoreOrEqual, 97, 98, null, new List<int> { 3, 0, 10 }, 1));
	}

	private void CreateItems1()
	{
		_dataArray.Add(new AiConditionItem(60, EAiConditionType.AcupointMarkCountMoreOrEqual, 99, 100, null, new List<int> { 3, 0, 10 }, 1));
		_dataArray.Add(new AiConditionItem(61, EAiConditionType.PoisonMarkCountMoreOrEqual, 101, 102, null, new List<int> { 3, 0, 14 }, 1));
		_dataArray.Add(new AiConditionItem(62, EAiConditionType.MindMarkCountMoreOrEqual, 103, 104, null, new List<int> { 3, 0 }, 1));
		_dataArray.Add(new AiConditionItem(63, EAiConditionType.FatalMarkCountMoreOrEqual, 105, 106, null, new List<int> { 3, 0 }, 1));
		_dataArray.Add(new AiConditionItem(64, EAiConditionType.DieMarkCountMoreOrEqual, 107, 108, null, new List<int> { 3, 0 }, 1));
		_dataArray.Add(new AiConditionItem(65, EAiConditionType.QiDisorderMarkCountMoreOrEqual, 109, 110, null, new List<int> { 3, 0 }, 1));
		_dataArray.Add(new AiConditionItem(66, EAiConditionType.StateMarkCountMoreOrEqual, 111, 112, null, new List<int> { 3, 0 }, 1));
		_dataArray.Add(new AiConditionItem(67, EAiConditionType.HealthMarkCountMoreOrEqual, 113, 114, null, new List<int> { 3, 0 }, 1));
		_dataArray.Add(new AiConditionItem(68, EAiConditionType.HasGrowingWug, 115, 116, null, new List<int> { 3, 15, 16, 17 }, 1));
		_dataArray.Add(new AiConditionItem(69, EAiConditionType.HasGrownWug, 117, 118, null, new List<int> { 3, 17 }, 1));
		_dataArray.Add(new AiConditionItem(70, EAiConditionType.HasKingWug, 119, 120, null, new List<int> { 3, 17 }, 1));
		_dataArray.Add(new AiConditionItem(71, EAiConditionType.NeiliAllocationPercentMoreOrEqual, 121, 122, null, new List<int> { 3, 18, 0 }, 1));
		_dataArray.Add(new AiConditionItem(72, EAiConditionType.CombatSkillEffectCountMoreOrEqual, 123, 124, null, new List<int> { 3, 12, 2, 0 }, 1));
		_dataArray.Add(new AiConditionItem(73, EAiConditionType.MobilityPercentMoreOrEqual, 125, 126, new List<int> { 11 }, new List<int> { 3 }, 1));
		_dataArray.Add(new AiConditionItem(74, EAiConditionType.MobilityLocking, 127, 128, null, new List<int> { 3 }, 1));
		_dataArray.Add(new AiConditionItem(75, EAiConditionType.ConsummateLevelMoreOrEqual, 129, 130, null, new List<int> { 3, 0 }, 1));
		_dataArray.Add(new AiConditionItem(76, EAiConditionType.BossPhaseMoreOrEqual, 131, 132, null, new List<int> { 0 }, 1));
		_dataArray.Add(new AiConditionItem(77, EAiConditionType.TrickCountMoreOrEqual, 133, 134, null, new List<int> { 3, 19, 0 }, 1));
		_dataArray.Add(new AiConditionItem(78, EAiConditionType.OptionChangeWeaponIndex, 135, 136, null, new List<int> { 0 }, 1));
		_dataArray.Add(new AiConditionItem(79, EAiConditionType.OptionChangeWeaponSpecial, 137, 138, null, new List<int> { 20 }, 1));
		_dataArray.Add(new AiConditionItem(80, EAiConditionType.OptionChangeWeaponType, 139, 140, null, new List<int> { 21 }, 1));
		_dataArray.Add(new AiConditionItem(81, EAiConditionType.CurrentWeaponIsSpecial, 141, 142, null, new List<int> { 3, 20 }, 1));
		_dataArray.Add(new AiConditionItem(82, EAiConditionType.CurrentWeaponIsType, 143, 144, null, new List<int> { 3, 21 }, 1));
		_dataArray.Add(new AiConditionItem(83, EAiConditionType.NeiliTypeFiveElementEqual, 145, 146, null, new List<int> { 3, 22 }, 1));
		_dataArray.Add(new AiConditionItem(84, EAiConditionType.AllMarkCountMoreOrEqual, 147, 148, null, new List<int> { 3, 0 }, 1));
		_dataArray.Add(new AiConditionItem(85, EAiConditionType.OuterOrInnerInjuryMarkCountMoreOrEqual, 149, 150, null, new List<int> { 3, 0, 13 }, 1));
		_dataArray.Add(new AiConditionItem(86, EAiConditionType.AllInjuryMarkCountMoreOrEqual, 151, 152, null, new List<int> { 3, 0 }, 1));
		_dataArray.Add(new AiConditionItem(87, EAiConditionType.AllFlawMarkCountMoreOrEqual, 153, 154, null, new List<int> { 3, 0 }, 1));
		_dataArray.Add(new AiConditionItem(88, EAiConditionType.AllAcupointMarkCountMoreOrEqual, 155, 156, null, new List<int> { 3, 0 }, 1));
		_dataArray.Add(new AiConditionItem(89, EAiConditionType.MemoryInternalAbove, 157, 158, new List<int> { 4, 4 }, null, 0));
		_dataArray.Add(new AiConditionItem(90, EAiConditionType.MemoryInternalBelow, 159, 160, new List<int> { 4, 4 }, null, 0));
		_dataArray.Add(new AiConditionItem(91, EAiConditionType.MemoryEqualCasting, 161, 162, new List<int> { 4 }, new List<int> { 3 }, 1));
		_dataArray.Add(new AiConditionItem(92, EAiConditionType.AttackRangeEdgeMore, 163, 164, null, new List<int> { 3, 9 }, 1));
		_dataArray.Add(new AiConditionItem(93, EAiConditionType.AttackRangeEdgeLess, 165, 166, null, new List<int> { 3, 9 }, 1));
		_dataArray.Add(new AiConditionItem(94, EAiConditionType.EnvironmentLastNormalAttackAnyMiss, 167, 168, null, null, 1));
		_dataArray.Add(new AiConditionItem(95, EAiConditionType.CurrentWeaponIsIndex, 169, 170, null, new List<int> { 3, 0 }, 1));
		_dataArray.Add(new AiConditionItem(96, EAiConditionType.CastingDirectOrReverseSkill, 171, 172, null, new List<int> { 3, 12, 2 }, 1));
		_dataArray.Add(new AiConditionItem(97, EAiConditionType.OptionCastSpecialCombatSkill, 173, 174, null, new List<int> { 2 }, 1));
		_dataArray.Add(new AiConditionItem(98, EAiConditionType.OptionCastDirectOrReverseCombatSkill, 175, 176, null, new List<int> { 2, 12 }, 1));
		_dataArray.Add(new AiConditionItem(99, EAiConditionType.BuffStatePowerSumMoreOrEqual, 177, 178, new List<int> { 24 }, new List<int> { 3, 0 }, 1));
		_dataArray.Add(new AiConditionItem(100, EAiConditionType.DebuffStatePowerSumMoreOrEqual, 179, 180, new List<int> { 24 }, new List<int> { 3, 0 }, 1));
		_dataArray.Add(new AiConditionItem(101, EAiConditionType.SpecialStatePowerSumMoreOrEqual, 181, 182, new List<int> { 24 }, new List<int> { 3, 0 }, 1));
		_dataArray.Add(new AiConditionItem(102, EAiConditionType.InMemoryCombatSkillRange, 183, 184, new List<int> { 4 }, new List<int> { 3, 0 }, 1));
		_dataArray.Add(new AiConditionItem(103, EAiConditionType.OptionCastMemoryCombatSkill, 185, 186, new List<int> { 4 }, null, 1));
		_dataArray.Add(new AiConditionItem(104, EAiConditionType.CurrentDistanceEqual, 187, 188, null, new List<int> { 0 }, 1));
		_dataArray.Add(new AiConditionItem(105, EAiConditionType.CurrentDistanceAbove, 189, 190, null, new List<int> { 0 }, 1));
		_dataArray.Add(new AiConditionItem(106, EAiConditionType.CurrentDistanceBelow, 191, 192, null, new List<int> { 0 }, 1));
		_dataArray.Add(new AiConditionItem(107, EAiConditionType.EnvironmentLastNormalAttackOutOfRange, 193, 194, null, null, 1));
		_dataArray.Add(new AiConditionItem(108, EAiConditionType.OptionUnlockAttackWeapon, 195, 196, null, new List<int> { 20 }, 1));
		_dataArray.Add(new AiConditionItem(109, EAiConditionType.OptionUnlockAttackWeaponType, 197, 198, null, new List<int> { 21 }, 1));
		_dataArray.Add(new AiConditionItem(110, EAiConditionType.UnlockAttackValuePercentMoreOrEqual, 199, 200, new List<int> { 11 }, new List<int> { 3, 20 }, 1));
		_dataArray.Add(new AiConditionItem(111, EAiConditionType.OptionInterruptCasting, 201, 202, null, null, 1));
		_dataArray.Add(new AiConditionItem(112, EAiConditionType.OptionInterruptAffectingDefense, 203, 204, null, null, 1));
		_dataArray.Add(new AiConditionItem(113, EAiConditionType.OptionInterruptAffectingMove, 205, 206, null, null, 1));
		_dataArray.Add(new AiConditionItem(114, EAiConditionType.OptionAutoCostTrick, 207, 207, null, null, 1));
		_dataArray.Add(new AiConditionItem(115, EAiConditionType.OptionUseItemMisc, 208, 209, null, new List<int> { 25 }, 1));
		_dataArray.Add(new AiConditionItem(116, EAiConditionType.AnyNotInfinitySilenceSkill, 210, 211, null, new List<int> { 3 }, 1));
		_dataArray.Add(new AiConditionItem(117, EAiConditionType.AnyTeammate, 212, 213, null, new List<int> { 3 }, 1));
		_dataArray.Add(new AiConditionItem(118, EAiConditionType.BreathPercentMoreOrEqual, 214, 215, new List<int> { 11 }, new List<int> { 3 }, 1));
		_dataArray.Add(new AiConditionItem(119, EAiConditionType.StancePercentMoreOrEqual, 216, 217, new List<int> { 11 }, new List<int> { 3 }, 1));
	}

	public override void Init()
	{
		base.Init();
		_dataArray = new List<AiConditionItem>(120);
		CreateItems0();
		CreateItems1();
	}
}
