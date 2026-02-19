using System;
using System.Collections.Generic;
using Config;
using GameData.Utilities;
using Redzen.Random;

namespace GameData.Domains.Character;

public static class CombatSkillHelper
{
	public struct AttainmentSectInfo
	{
		public sbyte OrgTemplateId;

		public sbyte CombatSkillsCount;

		public sbyte MaxGrade;
	}

	public const sbyte NeigongMaxSlotCount = 9;

	public const sbyte AttackMaxSlotCount = 9;

	public const sbyte AgileMaxSlotCount = 9;

	public const sbyte DefenseMaxSlotCount = 9;

	public const sbyte AssistMaxSlotCount = 9;

	public const sbyte GlobalMaxSlotCount = 99;

	public const sbyte SkillMinSlotCost = 1;

	public static readonly sbyte[] MaxSlotCounts = new sbyte[5] { 9, 9, 9, 9, 9 };

	public static readonly sbyte[] SlotBeginIndexes = new sbyte[5] { 0, 9, 18, 27, 36 };

	public static readonly sbyte[] SlotEndIndexes = new sbyte[5] { 9, 18, 27, 36, 45 };

	public const sbyte TotalProactiveSlotCount = 54;

	public const sbyte TotalSlotCount = 45;

	public const sbyte TotalSlotCapacity = 48;

	public static readonly int[] GenericAllocationCostFactor = new int[4] { 1, 2, 2, 3 };

	public static IComparer<short> CombatSkillGradeComparer = Comparer<short>.Create(CompareCombatSkillByGrade);

	public unsafe static void InitializeEquippedSkills(short* pEquippedSkills)
	{
		for (int i = 0; i < 12; i++)
		{
			((long*)pEquippedSkills)[i] = -1L;
		}
	}

	public static int GetGenericAllocationNextCost(sbyte equipType, int currAllocated)
	{
		return 1;
	}

	public static int GetGenericAllocationTotalCost(sbyte equipType, int currAllocated)
	{
		return currAllocated;
	}

	public unsafe static void InitializeEquippedSkills(short[] equippedSkills)
	{
		fixed (short* ptr = equippedSkills)
		{
			for (int i = 0; i < 12; i++)
			{
				((long*)ptr)[i] = -1L;
			}
		}
	}

	public unsafe static bool Equals(short[] lhs, short* pRhs)
	{
		fixed (short* ptr = lhs)
		{
			for (int i = 0; i < 12; i++)
			{
				if (((long*)ptr)[i] != ((long*)pRhs)[i])
				{
					return false;
				}
			}
		}
		return true;
	}

	public static bool Equals(IList<short> lhs, IList<short> rhs)
	{
		if (lhs == null || lhs.Count == 0)
		{
			if (rhs != null)
			{
				return rhs.Count == 0;
			}
			return true;
		}
		if (lhs.Count != rhs.Count)
		{
			return false;
		}
		int count = lhs.Count;
		for (int i = 0; i < count; i++)
		{
			short item = lhs[i];
			if (!rhs.Contains(item))
			{
				return false;
			}
		}
		return true;
	}

	public unsafe static void Copy(short[] dest, short* pSrc)
	{
		fixed (short* ptr = dest)
		{
			for (int i = 0; i < 12; i++)
			{
				((long*)ptr)[i] = ((long*)pSrc)[i];
			}
		}
	}

	public static short GetEquippedSkill(short[] equippedSkills, sbyte equipType, sbyte index)
	{
		sbyte b = SlotBeginIndexes[equipType];
		return equippedSkills[b + index];
	}

	public static bool IsProactiveSkill(sbyte equipType)
	{
		if (equipType != 1 && equipType != 2)
		{
			return equipType == 3;
		}
		return true;
	}

	public static bool IsPassiveSkill(sbyte equipType)
	{
		return equipType == 4;
	}

	public static void CalcAttainments_RecordSectInfo(List<AttainmentSectInfo> sectInfos, sbyte orgTemplateId, sbyte grade)
	{
		int num = -1;
		int i = 0;
		for (int count = sectInfos.Count; i < count; i++)
		{
			if (sectInfos[i].OrgTemplateId == orgTemplateId)
			{
				num = i;
				break;
			}
		}
		if (num >= 0)
		{
			AttainmentSectInfo value = sectInfos[num];
			value.CombatSkillsCount++;
			if (value.MaxGrade < grade)
			{
				value.MaxGrade = grade;
			}
			sectInfos[num] = value;
		}
		else
		{
			AttainmentSectInfo item = default(AttainmentSectInfo);
			item.OrgTemplateId = orgTemplateId;
			item.CombatSkillsCount = 1;
			item.MaxGrade = grade;
			sectInfos.Add(item);
		}
	}

	public static int CalcAttainments_GetPrimarySectIndex(List<AttainmentSectInfo> sectInfos)
	{
		int num = int.MinValue;
		int result = -1;
		int i = 0;
		for (int count = sectInfos.Count; i < count; i++)
		{
			AttainmentSectInfo attainmentSectInfo = sectInfos[i];
			int num2 = (attainmentSectInfo.CombatSkillsCount << 8) + attainmentSectInfo.MaxGrade;
			if (num2 > num)
			{
				num = num2;
				result = i;
			}
		}
		return result;
	}

	public static int CompareCombatSkillByGrade(short skillTemplateIdA, short skillTemplateIdB)
	{
		CombatSkillItem combatSkillItem = Config.CombatSkill.Instance[skillTemplateIdA];
		CombatSkillItem combatSkillItem2 = Config.CombatSkill.Instance[skillTemplateIdB];
		return combatSkillItem.Grade.CompareTo(combatSkillItem2.Grade);
	}

	public unsafe static int CalcBreakoutSuccessRate(short skillTemplateId, ref CombatSkillShorts qualifications)
	{
		CombatSkillItem combatSkillItem = Config.CombatSkill.Instance[skillTemplateId];
		short num = qualifications.Items[combatSkillItem.Type];
		short practiceQualificationRequirement = SkillGradeData.Instance[combatSkillItem.Grade].PracticeQualificationRequirement;
		if (num >= practiceQualificationRequirement)
		{
			return 100;
		}
		return GlobalConfig.Instance.NpcBreakoutBaseSuccessRate + num * (100 - GlobalConfig.Instance.NpcBreakoutBaseSuccessRate) / practiceQualificationRequirement;
	}

	public static int CalcForceBreakoutInjuriesAndDisorderOfQi(IRandomSource random, CombatSkillItem config, ref Injuries injuries, ref short disorderOfQi)
	{
		disorderOfQi += config.GoneMadQiDisorder;
		sbyte random2 = config.GoneMadInjuredPart.GetRandom(random);
		sbyte goneMadInjuryValue = config.GoneMadInjuryValue;
		int val = injuries.Get(random2, config.GoneMadInnerInjury) + goneMadInjuryValue - 6;
		injuries.Change(random2, config.GoneMadInnerInjury, goneMadInjuryValue);
		return Math.Max(val, 0);
	}
}
