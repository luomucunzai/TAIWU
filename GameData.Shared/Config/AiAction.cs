using System;
using System.Collections.Generic;
using Config.Common;

namespace Config;

[Serializable]
public class AiAction : ConfigData<AiActionItem, int>
{
	public static AiAction Instance = new AiAction();

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
		_dataArray.Add(new AiActionItem(0, EAiActionType.NormalAttack, 0, 1, null, null, 1));
		_dataArray.Add(new AiActionItem(1, EAiActionType.ChangeTrick, 2, 3, new List<int> { 4 }, null, 1));
		_dataArray.Add(new AiActionItem(2, EAiActionType.ChangeTrickFlaw, 4, 5, new List<int> { 4 }, null, 1));
		_dataArray.Add(new AiActionItem(3, EAiActionType.ChangeTrickAcupoint, 6, 7, new List<int> { 4 }, new List<int> { 10 }, 1));
		_dataArray.Add(new AiActionItem(4, EAiActionType.ChangeTrickNeiliType, 8, 9, new List<int> { 4 }, null, 1));
		_dataArray.Add(new AiActionItem(5, EAiActionType.CastSkill, 10, 11, null, new List<int> { 2 }, 1));
		_dataArray.Add(new AiActionItem(6, EAiActionType.CastSkillAttackBest, 12, 13, null, null, 1));
		_dataArray.Add(new AiActionItem(7, EAiActionType.CastSkillDefendBest, 14, 15, null, null, 1));
		_dataArray.Add(new AiActionItem(8, EAiActionType.CastSkillDefendBlock, 16, 17, null, null, 1));
		_dataArray.Add(new AiActionItem(9, EAiActionType.CastSkillAgileBuff, 18, 19, null, null, 1));
		_dataArray.Add(new AiActionItem(10, EAiActionType.CastSkillAgileSpeed, 20, 21, null, null, 1));
		_dataArray.Add(new AiActionItem(11, EAiActionType.CastSkillCastBoost, 22, 23, null, null, 1));
		_dataArray.Add(new AiActionItem(12, EAiActionType.MoveToAttackRangeCenter, 24, 25, null, new List<int> { 0 }, 1));
		_dataArray.Add(new AiActionItem(13, EAiActionType.MoveToNearbyEscape, 26, 27, null, null, 1));
		_dataArray.Add(new AiActionItem(14, EAiActionType.MoveToTargetDistance, 28, 29, null, new List<int> { 0 }, 1));
		_dataArray.Add(new AiActionItem(15, EAiActionType.MoveToFarthest, 30, 31, null, null, 1));
		_dataArray.Add(new AiActionItem(16, EAiActionType.MemorySetString, 32, 33, new List<int> { 4, 4 }, null, 0));
		_dataArray.Add(new AiActionItem(17, EAiActionType.MemorySetBoolean, 34, 33, new List<int> { 4 }, new List<int> { 1 }, 0));
		_dataArray.Add(new AiActionItem(18, EAiActionType.MemorySet, 35, 33, new List<int> { 4, 11 }, null, 0));
		_dataArray.Add(new AiActionItem(19, EAiActionType.UseTeammateCommand, 36, 37, null, new List<int> { 6 }, 1));
		_dataArray.Add(new AiActionItem(20, EAiActionType.ChangeWeaponAuto, 38, 39, null, new List<int> { 0, 0 }, 1));
		_dataArray.Add(new AiActionItem(21, EAiActionType.UseOtherAction, 40, 41, null, new List<int> { 8 }, 1));
		_dataArray.Add(new AiActionItem(22, EAiActionType.UseItemHealInjury, 42, 42, null, null, 1));
		_dataArray.Add(new AiActionItem(23, EAiActionType.UseItemHealPoison, 43, 43, null, null, 1));
		_dataArray.Add(new AiActionItem(24, EAiActionType.UseItemHealQiDisorder, 44, 44, null, null, 1));
		_dataArray.Add(new AiActionItem(25, EAiActionType.UseItemBuff, 45, 45, null, null, 1));
		_dataArray.Add(new AiActionItem(26, EAiActionType.UseItemPoison, 46, 46, null, null, 1));
		_dataArray.Add(new AiActionItem(27, EAiActionType.UseItemNeili, 47, 47, null, null, 1));
		_dataArray.Add(new AiActionItem(28, EAiActionType.UseItemWine, 48, 48, null, null, 1));
		_dataArray.Add(new AiActionItem(29, EAiActionType.UseItemRepairWeapon, 49, 49, null, null, 1));
		_dataArray.Add(new AiActionItem(30, EAiActionType.UseItemRepairArmor, 50, 50, null, null, 1));
		_dataArray.Add(new AiActionItem(31, EAiActionType.ChangeWeaponIndex, 51, 52, null, new List<int> { 0 }, 1));
		_dataArray.Add(new AiActionItem(32, EAiActionType.ChangeWeaponSpecial, 53, 54, null, new List<int> { 20 }, 1));
		_dataArray.Add(new AiActionItem(33, EAiActionType.ChangeWeaponType, 55, 56, null, new List<int> { 21 }, 1));
		_dataArray.Add(new AiActionItem(34, EAiActionType.MemoryAdd, 57, 58, new List<int> { 4, 11 }, null, 0));
		_dataArray.Add(new AiActionItem(35, EAiActionType.InterruptCasting, 59, 60, null, null, 1));
		_dataArray.Add(new AiActionItem(36, EAiActionType.InterruptAffectingDefense, 61, 62, null, null, 1));
		_dataArray.Add(new AiActionItem(37, EAiActionType.MemoryInternalSetString, 63, 64, new List<int> { 4, 4 }, null, 0));
		_dataArray.Add(new AiActionItem(38, EAiActionType.MemoryInternalSetBoolean, 65, 66, new List<int> { 4, 4 }, null, 0));
		_dataArray.Add(new AiActionItem(39, EAiActionType.MemoryInternalSet, 67, 68, new List<int> { 4, 4 }, null, 0));
		_dataArray.Add(new AiActionItem(40, EAiActionType.MemorySetAllMarkCount, 69, 70, new List<int> { 4 }, new List<int> { 3 }, 1));
		_dataArray.Add(new AiActionItem(41, EAiActionType.MemorySetInjuryMarkCount, 71, 72, new List<int> { 4 }, new List<int> { 3 }, 1));
		_dataArray.Add(new AiActionItem(42, EAiActionType.MemorySetFlawMarkCount, 73, 74, new List<int> { 4 }, new List<int> { 3 }, 1));
		_dataArray.Add(new AiActionItem(43, EAiActionType.MemorySetAcupointMarkCount, 75, 76, new List<int> { 4 }, new List<int> { 3 }, 1));
		_dataArray.Add(new AiActionItem(44, EAiActionType.MemorySetPoisonMarkCount, 77, 78, new List<int> { 4 }, new List<int> { 3 }, 1));
		_dataArray.Add(new AiActionItem(45, EAiActionType.MemorySetMindMarkCount, 79, 80, new List<int> { 4 }, new List<int> { 3 }, 1));
		_dataArray.Add(new AiActionItem(46, EAiActionType.MemorySetFatalMarkCount, 81, 82, new List<int> { 4 }, new List<int> { 3 }, 1));
		_dataArray.Add(new AiActionItem(47, EAiActionType.MemorySetChangeTrickCountByFlawCost, 83, 84, new List<int> { 4 }, new List<int> { 3 }, 1));
		_dataArray.Add(new AiActionItem(48, EAiActionType.MemorySetSpecialCombatSkill, 85, 86, new List<int> { 4 }, new List<int> { 2 }, 1));
		_dataArray.Add(new AiActionItem(49, EAiActionType.MoveToAttackRangeEdge, 87, 88, null, new List<int> { 9, 0 }, 1));
		_dataArray.Add(new AiActionItem(50, EAiActionType.MemorySetLastPrepareCombatSkill, 89, 90, new List<int> { 4 }, new List<int> { 3 }, 1));
		_dataArray.Add(new AiActionItem(51, EAiActionType.PrioritySetHigh, 91, 92, new List<int> { 4 }, null, 1));
		_dataArray.Add(new AiActionItem(52, EAiActionType.PrioritySetLow, 93, 94, new List<int> { 4 }, null, 1));
		_dataArray.Add(new AiActionItem(53, EAiActionType.PriorityReset, 95, 96, new List<int> { 4 }, null, 1));
		_dataArray.Add(new AiActionItem(54, EAiActionType.MemorySetBestAttackCombatSkill, 97, 98, new List<int> { 4 }, null, 1));
		_dataArray.Add(new AiActionItem(55, EAiActionType.CastSkillByMemory, 99, 100, new List<int> { 4 }, null, 1));
		_dataArray.Add(new AiActionItem(56, EAiActionType.InterruptAffectingMove, 101, 102, null, null, 1));
		_dataArray.Add(new AiActionItem(57, EAiActionType.UnlockAttackWeapon, 103, 104, null, new List<int> { 20 }, 1));
		_dataArray.Add(new AiActionItem(58, EAiActionType.UnlockAttackWeaponType, 105, 106, null, new List<int> { 21 }, 1));
		_dataArray.Add(new AiActionItem(59, EAiActionType.CostFirstUnavailableTrick, 107, 107, null, null, 1));
	}

	private void CreateItems1()
	{
		_dataArray.Add(new AiActionItem(60, EAiActionType.CostMemoryFirstTrick, 108, 109, new List<int> { 4 }, null, 1));
		_dataArray.Add(new AiActionItem(61, EAiActionType.CostFirstAnyTrick, 110, 110, null, null, 1));
		_dataArray.Add(new AiActionItem(62, EAiActionType.UseItemMisc, 111, 112, null, new List<int> { 25 }, 1));
	}

	public override void Init()
	{
		base.Init();
		_dataArray = new List<AiActionItem>(63);
		CreateItems0();
		CreateItems1();
	}
}
