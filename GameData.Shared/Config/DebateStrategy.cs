using System;
using System.Collections.Generic;
using Config.Common;
using GameData.Utilities;

namespace Config;

[Serializable]
public class DebateStrategy : ConfigData<DebateStrategyItem, short>
{
	public static DebateStrategy Instance = new DebateStrategy();

	private readonly HashSet<string> RequiredFields = new HashSet<string>
	{
		"LifeSkillType", "DebateRecord", "EffectList", "TargetList", "TargetRestrict", "EarlyLimits", "MidLimits", "LateLimits", "TemplateId", "Name",
		"Level", "Desc", "StyleDesc", "PawnEffectDesc", "NoTargetTip", "Image", "UsedCost", "EarlyLimitParams", "MidLimitParams", "LateLimitParams"
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
		_dataArray.Add(new DebateStrategyItem(0, 0, 1, 0, 1, 2, 3, 4, "LifeSkillCombatCard_Chahua_0", EDebateStrategyMarkType.Attach, 1, isOneTime: false, 19, EDebateStrategyTriggerType.Invalid, new List<IntPair>
		{
			new IntPair(0, -30)
		}, new List<short[]> { new short[3] { 3, 1, 1 } }, -1, 1, useBeforeMakeMove: false, avoidCheckMate: false, null, null, null, null, null, null));
		_dataArray.Add(new DebateStrategyItem(1, 5, 2, 0, 6, 7, 3, 8, "LifeSkillCombatCard_Chahua_0", EDebateStrategyMarkType.Attach, 2, isOneTime: false, 20, EDebateStrategyTriggerType.Invalid, new List<IntPair>
		{
			new IntPair(0, -25)
		}, new List<short[]> { new short[3] { 3, 1, 3 } }, -1, 1, useBeforeMakeMove: false, avoidCheckMate: false, new List<EDebateStrategyAiCheckType> { EDebateStrategyAiCheckType.TargetCountGreater }, new List<int> { 2 }, new List<EDebateStrategyAiCheckType> { EDebateStrategyAiCheckType.TargetCountGreater }, new List<int> { 1 }, null, null));
		_dataArray.Add(new DebateStrategyItem(2, 9, 3, 0, 10, 11, 3, 12, "LifeSkillCombatCard_Chahua_0", EDebateStrategyMarkType.Attach, 3, isOneTime: false, 21, EDebateStrategyTriggerType.Invalid, new List<IntPair>
		{
			new IntPair(0, -20)
		}, new List<short[]> { new short[3] { 2, 1, 6 } }, -1, 1, useBeforeMakeMove: false, avoidCheckMate: false, new List<EDebateStrategyAiCheckType> { EDebateStrategyAiCheckType.TargetCountGreater }, new List<int> { 5 }, new List<EDebateStrategyAiCheckType> { EDebateStrategyAiCheckType.TargetCountGreater }, new List<int> { 2 }, null, null));
		_dataArray.Add(new DebateStrategyItem(3, 13, 1, 1, 14, 15, 16, 17, "LifeSkillCombatCard_Chahua_1", EDebateStrategyMarkType.Other, 2, isOneTime: false, 22, EDebateStrategyTriggerType.Instant, new List<IntPair>
		{
			new IntPair(1, 1)
		}, new List<short[]>
		{
			new short[3] { 8, 1, 1 },
			new short[3] { 9, 1, 1 }
		}, -1, 1, useBeforeMakeMove: false, avoidCheckMate: false, new List<EDebateStrategyAiCheckType> { EDebateStrategyAiCheckType.SelfBasesGreater }, new List<int> { 15 }, new List<EDebateStrategyAiCheckType> { EDebateStrategyAiCheckType.SelfBasesGreater }, new List<int> { 10 }, new List<EDebateStrategyAiCheckType> { EDebateStrategyAiCheckType.SelfBasesGreater }, new List<int> { 5 }));
		_dataArray.Add(new DebateStrategyItem(4, 18, 2, 1, 19, 20, 21, 22, "LifeSkillCombatCard_Chahua_1", EDebateStrategyMarkType.Affect, 1, isOneTime: false, 23, EDebateStrategyTriggerType.Instant, new List<IntPair>
		{
			new IntPair(2, 1)
		}, new List<short[]>
		{
			new short[3] { 1, 1, 1 },
			new short[3] { 1, 1, 1 }
		}, -1, 1, useBeforeMakeMove: false, avoidCheckMate: false, null, null, null, null, null, null));
		_dataArray.Add(new DebateStrategyItem(5, 23, 3, 1, 24, 25, 26, 27, "LifeSkillCombatCard_Chahua_1", EDebateStrategyMarkType.Affect, 3, isOneTime: false, 24, EDebateStrategyTriggerType.Instant, new List<IntPair>
		{
			new IntPair(3, 1)
		}, new List<short[]>
		{
			new short[3] { 1, 1, 1 },
			new short[3] { 6, 1, 1 }
		}, -1, 1, useBeforeMakeMove: false, avoidCheckMate: false, null, null, null, null, null, null));
		_dataArray.Add(new DebateStrategyItem(6, 28, 1, 2, 29, 30, 31, 32, "LifeSkillCombatCard_Chahua_2", EDebateStrategyMarkType.Attach, 1, isOneTime: false, 25, EDebateStrategyTriggerType.PawnDamage, new List<IntPair>
		{
			new IntPair(4, 1)
		}, new List<short[]> { new short[3] { 1, 1, 1 } }, -1, 1, useBeforeMakeMove: false, avoidCheckMate: false, null, null, null, null, null, null));
		_dataArray.Add(new DebateStrategyItem(7, 33, 2, 2, 34, 35, 36, 37, "LifeSkillCombatCard_Chahua_2", EDebateStrategyMarkType.Attach, 2, isOneTime: false, 26, EDebateStrategyTriggerType.ConflictWin, new List<IntPair>
		{
			new IntPair(5, 1)
		}, new List<short[]> { new short[3] { 1, 1, 2 } }, -1, 1, useBeforeMakeMove: false, avoidCheckMate: false, new List<EDebateStrategyAiCheckType> { EDebateStrategyAiCheckType.TargetCountGreater }, new List<int> { 1 }, null, null, null, null));
		_dataArray.Add(new DebateStrategyItem(8, 38, 3, 2, 39, 40, 41, 42, "LifeSkillCombatCard_Chahua_2", EDebateStrategyMarkType.Attach, 3, isOneTime: false, 18, EDebateStrategyTriggerType.Invalid, new List<IntPair>
		{
			new IntPair(6, 50)
		}, new List<short[]> { new short[3] { 1, 1, 3 } }, -1, 1, useBeforeMakeMove: false, avoidCheckMate: false, new List<EDebateStrategyAiCheckType> { EDebateStrategyAiCheckType.TargetCountGreater }, new List<int> { 2 }, new List<EDebateStrategyAiCheckType> { EDebateStrategyAiCheckType.TargetCountGreater }, new List<int> { 1 }, null, null));
		_dataArray.Add(new DebateStrategyItem(9, 43, 1, 3, 44, 45, 46, 47, "LifeSkillCombatCard_Chahua_3", EDebateStrategyMarkType.Attach, 1, isOneTime: false, 28, EDebateStrategyTriggerType.Invalid, new List<IntPair>
		{
			new IntPair(0, 30)
		}, new List<short[]> { new short[3] { 1, 1, 1 } }, -1, 1, useBeforeMakeMove: false, avoidCheckMate: false, null, null, null, null, null, null));
		_dataArray.Add(new DebateStrategyItem(10, 48, 2, 3, 49, 50, 46, 51, "LifeSkillCombatCard_Chahua_3", EDebateStrategyMarkType.Attach, 2, isOneTime: false, 29, EDebateStrategyTriggerType.Invalid, new List<IntPair>
		{
			new IntPair(0, 25)
		}, new List<short[]> { new short[3] { 1, 1, 3 } }, -1, 1, useBeforeMakeMove: false, avoidCheckMate: false, new List<EDebateStrategyAiCheckType> { EDebateStrategyAiCheckType.TargetCountGreater }, new List<int> { 2 }, new List<EDebateStrategyAiCheckType> { EDebateStrategyAiCheckType.TargetCountGreater }, new List<int> { 1 }, null, null));
		_dataArray.Add(new DebateStrategyItem(11, 52, 3, 3, 53, 54, 46, 55, "LifeSkillCombatCard_Chahua_3", EDebateStrategyMarkType.Attach, 3, isOneTime: false, 30, EDebateStrategyTriggerType.Invalid, new List<IntPair>
		{
			new IntPair(0, 20)
		}, new List<short[]> { new short[3] { 0, 1, 6 } }, -1, 1, useBeforeMakeMove: false, avoidCheckMate: false, new List<EDebateStrategyAiCheckType> { EDebateStrategyAiCheckType.TargetCountGreater }, new List<int> { 5 }, new List<EDebateStrategyAiCheckType> { EDebateStrategyAiCheckType.TargetCountGreater }, new List<int> { 2 }, null, null));
		_dataArray.Add(new DebateStrategyItem(12, 56, 1, 4, 57, 58, 59, 60, "LifeSkillCombatCard_Chahua_4", EDebateStrategyMarkType.Other, 1, isOneTime: false, 31, EDebateStrategyTriggerType.Instant, new List<IntPair>
		{
			new IntPair(7, 1)
		}, new List<short[]>(), 15, 1, useBeforeMakeMove: true, avoidCheckMate: false, new List<EDebateStrategyAiCheckType>
		{
			EDebateStrategyAiCheckType.OpponentOwnedCardCountGreater,
			EDebateStrategyAiCheckType.SelfCanUseCardCountGreater
		}, new List<int> { 0, 3 }, new List<EDebateStrategyAiCheckType> { EDebateStrategyAiCheckType.OpponentOwnedCardCountGreater }, new List<int> { 0 }, null, null));
		_dataArray.Add(new DebateStrategyItem(13, 61, 2, 4, 62, 63, 64, 65, "LifeSkillCombatCard_Chahua_4", EDebateStrategyMarkType.Attach, 2, isOneTime: false, 32, EDebateStrategyTriggerType.Invalid, new List<IntPair>
		{
			new IntPair(8, 1)
		}, new List<short[]> { new short[3] { 5, 1, 3 } }, -1, 1, useBeforeMakeMove: false, avoidCheckMate: false, null, null, null, null, null, null));
		_dataArray.Add(new DebateStrategyItem(14, 66, 3, 4, 67, 68, 69, 70, "LifeSkillCombatCard_Chahua_4", EDebateStrategyMarkType.Affect, 3, isOneTime: false, 33, EDebateStrategyTriggerType.Instant, new List<IntPair>
		{
			new IntPair(3, 1)
		}, new List<short[]>
		{
			new short[3] { 3, 1, 1 },
			new short[3] { 7, 1, 1 }
		}, -1, 1, useBeforeMakeMove: false, avoidCheckMate: true, null, null, null, null, null, null));
		_dataArray.Add(new DebateStrategyItem(15, 71, 1, 5, 72, 73, 74, 75, "LifeSkillCombatCard_Chahua_5", EDebateStrategyMarkType.Attach, 0, isOneTime: false, 34, EDebateStrategyTriggerType.RoundStart, new List<IntPair>
		{
			new IntPair(12, -10)
		}, new List<short[]> { new short[3] { 3, 1, 1 } }, -1, 1, useBeforeMakeMove: false, avoidCheckMate: false, null, null, null, null, null, null));
		_dataArray.Add(new DebateStrategyItem(16, 76, 2, 5, 77, 78, 79, 80, "LifeSkillCombatCard_Chahua_5", EDebateStrategyMarkType.Attach, 0, isOneTime: false, 35, EDebateStrategyTriggerType.RoundStart, new List<IntPair>
		{
			new IntPair(13, -1)
		}, new List<short[]> { new short[3] { 3, 1, 1 } }, -1, 1, useBeforeMakeMove: false, avoidCheckMate: false, null, null, null, null, null, null));
		_dataArray.Add(new DebateStrategyItem(17, 81, 3, 5, 82, 83, 84, 85, "LifeSkillCombatCard_Chahua_5", EDebateStrategyMarkType.Attach, 3, isOneTime: false, 36, EDebateStrategyTriggerType.PawnDamage, new List<IntPair>
		{
			new IntPair(16, 1)
		}, new List<short[]> { new short[3] { 1, 1, 2 } }, -1, 1, useBeforeMakeMove: false, avoidCheckMate: false, new List<EDebateStrategyAiCheckType> { EDebateStrategyAiCheckType.TargetCountGreater }, new List<int> { 1 }, null, null, null, null));
		_dataArray.Add(new DebateStrategyItem(18, 86, 1, 6, 87, 88, 89, 90, "LifeSkillCombatCard_Chahua_6", EDebateStrategyMarkType.Attach, 1, isOneTime: true, 37, EDebateStrategyTriggerType.ConflictWin, new List<IntPair>
		{
			new IntPair(17, 1)
		}, new List<short[]> { new short[3] { 1, 1, 1 } }, -1, 1, useBeforeMakeMove: false, avoidCheckMate: false, null, null, null, null, null, null));
		_dataArray.Add(new DebateStrategyItem(19, 91, 2, 6, 92, 93, 94, 95, "LifeSkillCombatCard_Chahua_6", EDebateStrategyMarkType.Attach, 1, isOneTime: true, 38, EDebateStrategyTriggerType.ConflictLose, new List<IntPair>
		{
			new IntPair(18, 1)
		}, new List<short[]> { new short[3] { 1, 1, 1 } }, -1, 1, useBeforeMakeMove: false, avoidCheckMate: false, null, null, null, null, null, null));
		_dataArray.Add(new DebateStrategyItem(20, 96, 3, 6, 97, 98, 99, 100, "LifeSkillCombatCard_Chahua_6", EDebateStrategyMarkType.Attach, 3, isOneTime: false, 39, EDebateStrategyTriggerType.PawnForward, new List<IntPair>
		{
			new IntPair(19, 20)
		}, new List<short[]> { new short[3] { 1, 1, 3 } }, -1, 1, useBeforeMakeMove: false, avoidCheckMate: false, new List<EDebateStrategyAiCheckType> { EDebateStrategyAiCheckType.TargetCountGreater }, new List<int> { 2 }, new List<EDebateStrategyAiCheckType> { EDebateStrategyAiCheckType.TargetCountGreater }, new List<int> { 1 }, null, null));
		_dataArray.Add(new DebateStrategyItem(21, 101, 1, 7, 102, 103, 104, 105, "LifeSkillCombatCard_Chahua_7", EDebateStrategyMarkType.Attach, 0, isOneTime: false, 40, EDebateStrategyTriggerType.RoundStart, new List<IntPair>
		{
			new IntPair(9, 10)
		}, new List<short[]> { new short[3] { 1, 1, 1 } }, -1, 1, useBeforeMakeMove: false, avoidCheckMate: false, null, null, null, null, null, null));
		_dataArray.Add(new DebateStrategyItem(22, 106, 2, 7, 107, 108, 109, 110, "LifeSkillCombatCard_Chahua_7", EDebateStrategyMarkType.Attach, 0, isOneTime: false, 41, EDebateStrategyTriggerType.RoundStart, new List<IntPair>
		{
			new IntPair(10, 1)
		}, new List<short[]> { new short[3] { 1, 1, 1 } }, -1, 1, useBeforeMakeMove: false, avoidCheckMate: false, null, null, null, null, null, null));
		_dataArray.Add(new DebateStrategyItem(23, 111, 3, 7, 112, 113, 114, 115, "LifeSkillCombatCard_Chahua_7", EDebateStrategyMarkType.Affect, 2, isOneTime: false, 42, EDebateStrategyTriggerType.Instant, new List<IntPair>
		{
			new IntPair(5, 1)
		}, new List<short[]> { new short[3] { 1, 1, 3 } }, 11, 1, useBeforeMakeMove: false, avoidCheckMate: false, new List<EDebateStrategyAiCheckType> { EDebateStrategyAiCheckType.TargetCountGreater }, new List<int> { 2 }, new List<EDebateStrategyAiCheckType> { EDebateStrategyAiCheckType.TargetCountGreater }, new List<int> { 1 }, null, null));
		_dataArray.Add(new DebateStrategyItem(24, 116, 1, 8, 117, 118, 119, 120, "LifeSkillCombatCard_Chahua_8", EDebateStrategyMarkType.Affect, 1, isOneTime: false, 43, EDebateStrategyTriggerType.Instant, new List<IntPair>
		{
			new IntPair(20, 1)
		}, new List<short[]> { new short[3] { 3, 1, 3 } }, 12, 1, useBeforeMakeMove: true, avoidCheckMate: false, null, null, null, null, null, null));
		_dataArray.Add(new DebateStrategyItem(25, 121, 2, 8, 122, 123, 124, 125, "LifeSkillCombatCard_Chahua_8", EDebateStrategyMarkType.Affect, 3, isOneTime: false, 44, EDebateStrategyTriggerType.Instant, new List<IntPair>
		{
			new IntPair(21, 1),
			new IntPair(15, 1)
		}, new List<short[]> { new short[3] { 5, 1, 2 } }, -1, 1, useBeforeMakeMove: false, avoidCheckMate: false, new List<EDebateStrategyAiCheckType>
		{
			EDebateStrategyAiCheckType.OpponentGamePointGreater,
			EDebateStrategyAiCheckType.SelfNotCheckMate
		}, new List<int> { 0, 0 }, new List<EDebateStrategyAiCheckType>
		{
			EDebateStrategyAiCheckType.OpponentGamePointGreater,
			EDebateStrategyAiCheckType.SelfNotCheckMate
		}, new List<int> { 0, 0 }, new List<EDebateStrategyAiCheckType>
		{
			EDebateStrategyAiCheckType.OpponentGamePointGreater,
			EDebateStrategyAiCheckType.SelfNotCheckMate
		}, new List<int> { 0, 0 }));
		_dataArray.Add(new DebateStrategyItem(26, 126, 3, 8, 127, 128, 129, 130, "LifeSkillCombatCard_Chahua_8", EDebateStrategyMarkType.Attach, 3, isOneTime: false, 46, EDebateStrategyTriggerType.PawnDamage, new List<IntPair>
		{
			new IntPair(11, 1)
		}, new List<short[]> { new short[3] { 1, 1, 3 } }, -1, 1, useBeforeMakeMove: false, avoidCheckMate: false, new List<EDebateStrategyAiCheckType>
		{
			EDebateStrategyAiCheckType.SelfGamePointSmaller,
			EDebateStrategyAiCheckType.TargetCountGreater
		}, new List<int> { 6, 2 }, new List<EDebateStrategyAiCheckType>
		{
			EDebateStrategyAiCheckType.SelfGamePointSmaller,
			EDebateStrategyAiCheckType.TargetCountGreater
		}, new List<int> { 6, 1 }, null, null));
		_dataArray.Add(new DebateStrategyItem(27, 131, 1, 9, 132, 133, 134, 135, "LifeSkillCombatCard_Chahua_9", EDebateStrategyMarkType.Attach, 1, isOneTime: false, -1, EDebateStrategyTriggerType.Invalid, new List<IntPair>
		{
			new IntPair(23, 1)
		}, new List<short[]> { new short[3] { 3, 1, 1 } }, -1, 1, useBeforeMakeMove: false, avoidCheckMate: false, null, null, null, null, null, null));
		_dataArray.Add(new DebateStrategyItem(28, 136, 2, 9, 137, 138, 139, 140, "LifeSkillCombatCard_Chahua_9", EDebateStrategyMarkType.Attach, 2, isOneTime: false, 47, EDebateStrategyTriggerType.PawnDead, new List<IntPair>
		{
			new IntPair(22, 1)
		}, new List<short[]>
		{
			new short[3] { 5, 1, 1 },
			new short[3] { 5, 1, 1 }
		}, -1, 1, useBeforeMakeMove: false, avoidCheckMate: false, null, null, null, null, null, null));
		_dataArray.Add(new DebateStrategyItem(29, 141, 3, 9, 142, 143, 144, 145, "LifeSkillCombatCard_Chahua_9", EDebateStrategyMarkType.Attach, 3, isOneTime: false, 48, EDebateStrategyTriggerType.PawnDamage, new List<IntPair>
		{
			new IntPair(11, -1)
		}, new List<short[]> { new short[3] { 3, 1, 3 } }, -1, 1, useBeforeMakeMove: false, avoidCheckMate: false, null, null, null, null, null, null));
		_dataArray.Add(new DebateStrategyItem(30, 146, 1, 10, 147, 148, 149, 150, "LifeSkillCombatCard_Chahua_10", EDebateStrategyMarkType.Attach, 2, isOneTime: false, 49, EDebateStrategyTriggerType.Invalid, new List<IntPair>
		{
			new IntPair(24, 20)
		}, new List<short[]> { new short[3] { 1, 1, 1 } }, -1, 1, useBeforeMakeMove: false, avoidCheckMate: false, new List<EDebateStrategyAiCheckType> { EDebateStrategyAiCheckType.TargetCountGreater }, new List<int> { 2 }, new List<EDebateStrategyAiCheckType> { EDebateStrategyAiCheckType.TargetCountGreater }, new List<int> { 1 }, null, null));
		_dataArray.Add(new DebateStrategyItem(31, 151, 2, 10, 152, 153, 154, 155, "LifeSkillCombatCard_Chahua_10", EDebateStrategyMarkType.Attach, 2, isOneTime: false, 50, EDebateStrategyTriggerType.ConflictWin, new List<IntPair>
		{
			new IntPair(25, 1)
		}, new List<short[]> { new short[3] { 1, 1, 1 } }, -1, 1, useBeforeMakeMove: false, avoidCheckMate: false, null, null, null, null, null, null));
		_dataArray.Add(new DebateStrategyItem(32, 156, 3, 10, 157, 158, 159, 160, "LifeSkillCombatCard_Chahua_10", EDebateStrategyMarkType.Attach, 3, isOneTime: false, 51, EDebateStrategyTriggerType.Invalid, new List<IntPair>
		{
			new IntPair(26, 1)
		}, new List<short[]> { new short[3] { 1, 1, 3 } }, -1, 1, useBeforeMakeMove: false, avoidCheckMate: false, new List<EDebateStrategyAiCheckType> { EDebateStrategyAiCheckType.TargetCountGreater }, new List<int> { 2 }, new List<EDebateStrategyAiCheckType> { EDebateStrategyAiCheckType.TargetCountGreater }, new List<int> { 1 }, new List<EDebateStrategyAiCheckType> { EDebateStrategyAiCheckType.TargetCountGreater }, new List<int> { 1 }));
		_dataArray.Add(new DebateStrategyItem(33, 161, 1, 11, 162, 163, 164, 165, "LifeSkillCombatCard_Chahua_11", EDebateStrategyMarkType.Affect, 1, isOneTime: false, 52, EDebateStrategyTriggerType.Instant, new List<IntPair>
		{
			new IntPair(27, 1)
		}, new List<short[]> { new short[3] { 5, 1, 1 } }, -1, 1, useBeforeMakeMove: false, avoidCheckMate: true, new List<EDebateStrategyAiCheckType> { EDebateStrategyAiCheckType.OpponentTargetCountGreater }, new List<int> { 0 }, new List<EDebateStrategyAiCheckType> { EDebateStrategyAiCheckType.OpponentTargetCountGreater }, new List<int> { 0 }, new List<EDebateStrategyAiCheckType> { EDebateStrategyAiCheckType.OpponentTargetCountGreater }, new List<int> { 0 }));
		_dataArray.Add(new DebateStrategyItem(34, 166, 2, 11, 167, 168, 169, 170, "LifeSkillCombatCard_Chahua_11", EDebateStrategyMarkType.Attach, 1, isOneTime: false, 18, EDebateStrategyTriggerType.Invalid, new List<IntPair>
		{
			new IntPair(28, 1)
		}, new List<short[]> { new short[3] { 4, 1, 3 } }, -1, 1, useBeforeMakeMove: false, avoidCheckMate: false, new List<EDebateStrategyAiCheckType> { EDebateStrategyAiCheckType.TargetCountGreater }, new List<int> { 2 }, new List<EDebateStrategyAiCheckType> { EDebateStrategyAiCheckType.TargetCountGreater }, new List<int> { 1 }, null, null));
		_dataArray.Add(new DebateStrategyItem(35, 171, 3, 11, 172, 173, 174, 175, "LifeSkillCombatCard_Chahua_11", EDebateStrategyMarkType.Attach, 3, isOneTime: false, 55, EDebateStrategyTriggerType.PawnDamage, new List<IntPair>
		{
			new IntPair(14, 3)
		}, new List<short[]> { new short[3] { 5, 1, 1 } }, -1, 1, useBeforeMakeMove: false, avoidCheckMate: false, new List<EDebateStrategyAiCheckType>
		{
			EDebateStrategyAiCheckType.OpponentGamePointNotGreaterThanSelf,
			EDebateStrategyAiCheckType.SelfGamePointGreater
		}, new List<int> { 0, 3 }, new List<EDebateStrategyAiCheckType>
		{
			EDebateStrategyAiCheckType.OpponentGamePointNotGreaterThanSelf,
			EDebateStrategyAiCheckType.SelfGamePointGreater
		}, new List<int> { 0, 3 }, new List<EDebateStrategyAiCheckType>
		{
			EDebateStrategyAiCheckType.OpponentGamePointNotGreaterThanSelf,
			EDebateStrategyAiCheckType.SelfGamePointGreater
		}, new List<int> { 0, 3 }));
		_dataArray.Add(new DebateStrategyItem(36, 176, 1, 12, 177, 178, 179, 180, "LifeSkillCombatCard_Chahua_12", EDebateStrategyMarkType.Affect, 1, isOneTime: false, 56, EDebateStrategyTriggerType.Instant, new List<IntPair>
		{
			new IntPair(29, 3)
		}, new List<short[]> { new short[3] { 5, 1, 1 } }, 11, -3, useBeforeMakeMove: false, avoidCheckMate: true, null, null, null, null, null, null));
		_dataArray.Add(new DebateStrategyItem(37, 181, 2, 12, 182, 183, 184, 185, "LifeSkillCombatCard_Chahua_12", EDebateStrategyMarkType.Other, 0, isOneTime: false, 57, EDebateStrategyTriggerType.Instant, new List<IntPair>
		{
			new IntPair(30, 1)
		}, new List<short[]> { new short[3] { 10, 1, 3 } }, -1, 1, useBeforeMakeMove: false, avoidCheckMate: false, new List<EDebateStrategyAiCheckType> { EDebateStrategyAiCheckType.SelfStrategyPointSmaller }, new List<int> { 3 }, null, null, null, null));
		_dataArray.Add(new DebateStrategyItem(38, 186, 3, 12, 187, 188, 189, 190, "LifeSkillCombatCard_Chahua_12", EDebateStrategyMarkType.Other, 2, isOneTime: false, 58, EDebateStrategyTriggerType.Instant, new List<IntPair>
		{
			new IntPair(31, 3)
		}, new List<short[]>(), 16, 1, useBeforeMakeMove: true, avoidCheckMate: false, new List<EDebateStrategyAiCheckType>
		{
			EDebateStrategyAiCheckType.OpponentUsedCardCountGreater,
			EDebateStrategyAiCheckType.SelfCanUseCardCountGreater
		}, new List<int> { 2, 1 }, new List<EDebateStrategyAiCheckType>
		{
			EDebateStrategyAiCheckType.OpponentUsedCardCountGreater,
			EDebateStrategyAiCheckType.SelfCanUseCardCountGreater
		}, new List<int> { 1, 3 }, null, null));
		_dataArray.Add(new DebateStrategyItem(39, 191, 1, 13, 192, 193, 194, 195, "LifeSkillCombatCard_Chahua_13", EDebateStrategyMarkType.Affect, 1, isOneTime: false, 59, EDebateStrategyTriggerType.Instant, new List<IntPair>
		{
			new IntPair(32, 1)
		}, new List<short[]> { new short[3] { 5, 1, 3 } }, 13, 1, useBeforeMakeMove: true, avoidCheckMate: false, new List<EDebateStrategyAiCheckType> { EDebateStrategyAiCheckType.TargetCountGreater }, new List<int> { 2 }, new List<EDebateStrategyAiCheckType> { EDebateStrategyAiCheckType.TargetCountGreater }, new List<int> { 1 }, null, null));
		_dataArray.Add(new DebateStrategyItem(40, 196, 2, 13, 197, 198, 199, 200, "LifeSkillCombatCard_Chahua_13", EDebateStrategyMarkType.Attach, 3, isOneTime: false, 60, EDebateStrategyTriggerType.ConflictStart, new List<IntPair>
		{
			new IntPair(33, 1)
		}, new List<short[]> { new short[3] { 5, 1, 1 } }, -1, 1, useBeforeMakeMove: false, avoidCheckMate: false, null, null, null, null, null, null));
		_dataArray.Add(new DebateStrategyItem(41, 201, 3, 13, 202, 203, 204, 205, "LifeSkillCombatCard_Chahua_13", EDebateStrategyMarkType.Attach, 3, isOneTime: false, -1, EDebateStrategyTriggerType.PawnActing, new List<IntPair>
		{
			new IntPair(34, 1)
		}, new List<short[]> { new short[3] { 3, 1, 3 } }, -1, 1, useBeforeMakeMove: false, avoidCheckMate: false, new List<EDebateStrategyAiCheckType> { EDebateStrategyAiCheckType.TargetCountGreater }, new List<int> { 2 }, new List<EDebateStrategyAiCheckType> { EDebateStrategyAiCheckType.TargetCountGreater }, new List<int> { 1 }, null, null));
		_dataArray.Add(new DebateStrategyItem(42, 206, 1, 14, 207, 208, 209, 210, "LifeSkillCombatCard_Chahua_14", EDebateStrategyMarkType.Attach, 1, isOneTime: false, 62, EDebateStrategyTriggerType.Invalid, new List<IntPair>
		{
			new IntPair(35, 1)
		}, new List<short[]> { new short[3] { 4, 1, 3 } }, -1, 1, useBeforeMakeMove: false, avoidCheckMate: false, null, null, null, null, null, null));
		_dataArray.Add(new DebateStrategyItem(43, 211, 2, 14, 212, 213, 214, 215, "LifeSkillCombatCard_Chahua_14", EDebateStrategyMarkType.Affect, 3, isOneTime: false, 63, EDebateStrategyTriggerType.Instant, new List<IntPair>
		{
			new IntPair(36, 1)
		}, new List<short[]>
		{
			new short[3] { 5, 1, 1 },
			new short[3] { 5, 1, 1 }
		}, -1, 1, useBeforeMakeMove: false, avoidCheckMate: true, null, null, null, null, null, null));
		_dataArray.Add(new DebateStrategyItem(44, 216, 3, 14, 217, 218, 219, 220, "LifeSkillCombatCard_Chahua_14", EDebateStrategyMarkType.Affect, 3, isOneTime: false, 64, EDebateStrategyTriggerType.Instant, new List<IntPair>
		{
			new IntPair(37, 3)
		}, new List<short[]> { new short[3] { 5, 1, 1 } }, 11, 0, useBeforeMakeMove: false, avoidCheckMate: true, null, null, null, null, null, null));
		_dataArray.Add(new DebateStrategyItem(45, 221, 1, 15, 222, 223, 224, 225, "LifeSkillCombatCard_Chahua_15", EDebateStrategyMarkType.Affect, 2, isOneTime: false, 65, EDebateStrategyTriggerType.Instant, new List<IntPair>
		{
			new IntPair(38, 1)
		}, new List<short[]> { new short[3] { 5, 1, 1 } }, 14, 1, useBeforeMakeMove: false, avoidCheckMate: false, null, null, null, null, null, null));
		_dataArray.Add(new DebateStrategyItem(46, 226, 2, 15, 227, 228, 229, 230, "LifeSkillCombatCard_Chahua_15", EDebateStrategyMarkType.Affect, 3, isOneTime: false, 66, EDebateStrategyTriggerType.Instant, new List<IntPair>
		{
			new IntPair(39, 1)
		}, new List<short[]>
		{
			new short[3] { 5, 1, 1 },
			new short[3] { 5, 1, 1 }
		}, -1, 1, useBeforeMakeMove: false, avoidCheckMate: false, null, null, null, null, null, null));
		_dataArray.Add(new DebateStrategyItem(47, 231, 3, 15, 232, 233, 234, 235, "LifeSkillCombatCard_Chahua_15", EDebateStrategyMarkType.Other, 1, isOneTime: false, 67, EDebateStrategyTriggerType.Instant, new List<IntPair>
		{
			new IntPair(40, 1)
		}, new List<short[]>(), 17, 1, useBeforeMakeMove: false, avoidCheckMate: false, new List<EDebateStrategyAiCheckType> { EDebateStrategyAiCheckType.SelfCanUseCardCountSmaller }, new List<int> { 2 }, new List<EDebateStrategyAiCheckType> { EDebateStrategyAiCheckType.SelfCanUseCardCountSmaller }, new List<int> { 2 }, null, null));
	}

	public override void Init()
	{
		base.Init();
		_dataArray = new List<DebateStrategyItem>(48);
		CreateItems0();
	}
}
