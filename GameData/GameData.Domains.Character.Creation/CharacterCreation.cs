using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Config;
using GameData.Domains.Organization;
using GameData.Utilities;
using Redzen.Random;

namespace GameData.Domains.Character.Creation;

public static class CharacterCreation
{
	private const int BaseMainAttributeSum = 168;

	private const int MainAttributePerLevel = 21;

	private const int BaseLifeSkillSum = 448;

	private const int LifeSkillPerLevel = 56;

	private const int BaseCombatSkillSum = 392;

	private const int CombatSkillPerLevel = 49;

	private const int MutationRate = 20;

	private const int ValueSpan = 20;

	private static readonly sbyte[] SectMemberGenerationTypeWeights = GlobalConfig.CharacterCreationSectMemberGenerationTypeWeights;

	private static readonly sbyte[] CivilianGenerationTypeWeights = GlobalConfig.CharacterCreationCivilianGenerationTypeWeights;

	private static readonly sbyte[] AdjustGradeBaseChances = GlobalConfig.CharacterCreationAdjustGradeBaseChances;

	private static readonly short[][] AdjustGradeWeights = GlobalConfig.CharacterCreationAdjustGradeWeights;

	private const sbyte UseCurrOrg = 0;

	private const sbyte UseIdealSect = 1;

	private const sbyte MergeBoth = 2;

	public const int FeaturesExpectedMaxCount = 16;

	public unsafe static sbyte CalcGrowingSectGradeAndAssignWeights(IRandomSource random, ref IntelligentCharacterCreationInfo creationInfo, sbyte idealSectId)
	{
		if (creationInfo.MotherCharId >= 0 && creationInfo.ActualFatherCharId >= 0)
		{
			MainAttributes baseMainAttributes = creationInfo.Mother.GetBaseMainAttributes();
			MainAttributes mainAttributes = creationInfo.ActualFather?.GetBaseMainAttributes() ?? creationInfo.ActualDeadFather.BaseMainAttributes;
			sbyte b;
			fixed (short* items = creationInfo.ParentMainAttributeValues.Items)
			{
				int parentValues = GetParentValues(random, baseMainAttributes.Items, mainAttributes.Items, items, 6);
				b = SumToGrade(parentValues, 168, 21);
			}
			LifeSkillShorts baseLifeSkillQualifications = creationInfo.Mother.GetBaseLifeSkillQualifications();
			Character actualFather = creationInfo.ActualFather;
			LifeSkillShorts lifeSkillShorts = ((actualFather != null) ? actualFather.GetBaseLifeSkillQualifications() : creationInfo.ActualDeadFather.BaseLifeSkillQualifications);
			sbyte b2;
			fixed (short* items2 = creationInfo.ParentLifeSkillQualificationValues.Items)
			{
				int parentValues2 = GetParentValues(random, baseLifeSkillQualifications.Items, lifeSkillShorts.Items, items2, 16);
				b2 = SumToGrade(parentValues2, 448, 56);
			}
			CombatSkillShorts baseCombatSkillQualifications = creationInfo.Mother.GetBaseCombatSkillQualifications();
			Character actualFather2 = creationInfo.ActualFather;
			CombatSkillShorts combatSkillShorts = ((actualFather2 != null) ? actualFather2.GetBaseCombatSkillQualifications() : creationInfo.ActualDeadFather.BaseCombatSkillQualifications);
			sbyte b3;
			fixed (short* items3 = creationInfo.ParentCombatSkillQualificationValues.Items)
			{
				int parentValues3 = GetParentValues(random, baseCombatSkillQualifications.Items, combatSkillShorts.Items, items3, 14);
				b3 = SumToGrade(parentValues3, 392, 49);
			}
			sbyte b4 = (sbyte)Math.Clamp((b + b2 + b3) / 3, 0, 8);
			if (creationInfo.AllowRandomGrowingGradeAdjust)
			{
				sbyte grade = creationInfo.Mother.GetOrganizationInfo().Grade;
				sbyte fatherGrade = creationInfo.Father?.GetOrganizationInfo().Grade ?? 0;
				b4 = TryAdjustGrade(random, fatherGrade, grade, b4);
			}
			return b4;
		}
		if (creationInfo.MotherCharId >= 0)
		{
			creationInfo.ParentMainAttributeValues = creationInfo.Mother.GetBaseMainAttributes();
			int sum = creationInfo.ParentMainAttributeValues.GetSum();
			sbyte b5 = SumToGrade(sum, 168, 21);
			creationInfo.ParentLifeSkillQualificationValues = creationInfo.Mother.GetBaseLifeSkillQualifications();
			int sum2 = creationInfo.ParentLifeSkillQualificationValues.GetSum();
			sbyte b6 = SumToGrade(sum2, 448, 56);
			creationInfo.ParentCombatSkillQualificationValues = creationInfo.Mother.GetBaseCombatSkillQualifications();
			int sum3 = creationInfo.ParentCombatSkillQualificationValues.GetSum();
			sbyte b7 = SumToGrade(sum3, 392, 49);
			sbyte b8 = (sbyte)Math.Clamp((b5 + b6 + b7) / 3, 0, 8);
			if (creationInfo.AllowRandomGrowingGradeAdjust)
			{
				sbyte grade2 = creationInfo.Mother.GetOrganizationInfo().Grade;
				sbyte fatherGrade2 = creationInfo.Father?.GetOrganizationInfo().Grade ?? 0;
				b8 = TryAdjustGrade(random, fatherGrade2, grade2, b8);
			}
			return b8;
		}
		sbyte grade3 = creationInfo.OrgInfo.Grade;
		sbyte b9 = (sbyte)RedzenHelper.NormalDistribute(random, creationInfo.OrgInfo.Grade, 1.2f);
		if (b9 < 0 || b9 > 8)
		{
			b9 = creationInfo.OrgInfo.Grade;
		}
		if (creationInfo.GrowingSectId < 0)
		{
			switch ((idealSectId >= 0 && idealSectId != creationInfo.OrgInfo.OrgTemplateId) ? (OrganizationDomain.IsSect(creationInfo.OrgInfo.OrgTemplateId) ? RandomUtils.GetRandomIndex(SectMemberGenerationTypeWeights, random) : RandomUtils.GetRandomIndex(CivilianGenerationTypeWeights, random)) : 0)
			{
			case 0:
			{
				OrganizationMemberItem orgMemberConfig4 = OrganizationDomain.GetOrgMemberConfig(creationInfo.OrgInfo.OrgTemplateId, grade3);
				AssignVirtualParentValuesByOrgMember(random, ref creationInfo, orgMemberConfig4);
				break;
			}
			case 1:
			{
				OrganizationMemberItem orgMemberConfig3 = OrganizationDomain.GetOrgMemberConfig(idealSectId, grade3);
				AssignVirtualParentValuesByOrgMember(random, ref creationInfo, orgMemberConfig3);
				break;
			}
			case 2:
			{
				OrganizationMemberItem orgMemberConfig = OrganizationDomain.GetOrgMemberConfig(creationInfo.OrgInfo.OrgTemplateId, grade3);
				OrganizationMemberItem orgMemberConfig2 = OrganizationDomain.GetOrgMemberConfig(idealSectId, grade3);
				AssignMergedVirtualParentValuesByOrgMembers(random, ref creationInfo, orgMemberConfig, orgMemberConfig2);
				break;
			}
			}
		}
		else
		{
			OrganizationMemberItem orgMemberConfig5 = OrganizationDomain.GetOrgMemberConfig(creationInfo.GrowingSectId, grade3);
			AssignVirtualParentValuesByOrgMember(random, ref creationInfo, orgMemberConfig5);
		}
		return b9;
	}

	public static (int, int) Reservoir(IRandomSource random, (int, int) current, (int, int) newSample)
	{
		int num = current.Item2 + newSample.Item2;
		int item;
		if (random.NextFloat() * (float)num < (float)newSample.Item2)
		{
			(item, _) = newSample;
		}
		else
		{
			(item, _) = current;
		}
		return (item, num);
	}

	public static int SplitInteger(IRandomSource random, int number, int freeSpace, int max)
	{
		if (number >= freeSpace * max)
		{
			return freeSpace;
		}
		if (number <= freeSpace)
		{
			return (max == 1) ? freeSpace : 0;
		}
		(int, int) current = (0, 0);
		int num = Math.Min(number / max, (number - freeSpace) / (max - 1));
		int num2;
		int num3;
		while (num >= 0 && (num2 = (freeSpace - num) * (max - 1) - (num3 = number - num * max) + 1) > 0)
		{
			current = Reservoir(random, current, (num, num2 * (num3 - (freeSpace - num) + 1)));
			num--;
		}
		return current.Item1;
	}

	public unsafe static void CreateAttributes(IRandomSource random, out MainAttributes rawValues, ref MainAttributes rawWeights, int sum, int mutateCount = 0, int count = -1)
	{
		fixed (short* items = rawValues.Items)
		{
			fixed (short* items2 = rawWeights.Items)
			{
				CreateAttributesImpl(random, items, items2, 6, sum, mutateCount);
			}
		}
	}

	public unsafe static void CreateAttributes(IRandomSource random, out CombatSkillShorts rawValues, ref CombatSkillShorts rawWeights, int sum, int mutateCount = 0, int count = -1)
	{
		fixed (short* items = rawValues.Items)
		{
			fixed (short* items2 = rawWeights.Items)
			{
				CreateAttributesImpl(random, items, items2, 14, sum, mutateCount);
			}
		}
	}

	public unsafe static void CreateAttributes(IRandomSource random, out LifeSkillShorts rawValues, ref LifeSkillShorts rawWeights, int sum, int mutateCount = 0, int count = -1)
	{
		fixed (short* items = rawValues.Items)
		{
			fixed (short* items2 = rawWeights.Items)
			{
				CreateAttributesImpl(random, items, items2, 16, sum, mutateCount);
			}
		}
	}

	public unsafe static void CreateAttributesImpl(IRandomSource random, short* values, short* weights, int count, int sum, int mutateCount = 0)
	{
		DistributeValues(random, values, weights, count, sum);
		if (mutateCount > 0)
		{
			PerformMutation(random, values, count, mutateCount);
		}
	}

	public static MainAttributes CreateMainAttributes(IRandomSource random, sbyte grade, short[] mainAttributesAdjust)
	{
		return CreateMainAttributes(random, grade, mainAttributesAdjust, null);
	}

	public static MainAttributes CreateMainAttributes(IRandomSource random, sbyte grade, short[] mainAttributesAdjust, WeightsSumDistribution weightSumWeightsTable, int maxMutateCount = 2, int mutatePercentProb = 20)
	{
		int sum = GradeToSum(grade, 168, 21);
		CalculateVirtualParentValues(random, mainAttributesAdjust, out MainAttributes resultValues, weightSumWeightsTable);
		CreateAttributes(random, out var rawValues, ref resultValues, sum, DecodeMutateCount(random, maxMutateCount, mutatePercentProb));
		return rawValues;
	}

	public static MainAttributes CreateMainAttributes(IRandomSource random, ref IntelligentCharacterCreationInfo creationInfo, int maxMutateCount = 2, int mutatePercentProb = 20)
	{
		int sum = GradeToSum(creationInfo.GrowingSectGrade, 168, 21);
		MainAttributes rawWeights = creationInfo.ParentMainAttributeValues;
		CreateAttributes(random, out var rawValues, ref rawWeights, sum, DecodeMutateCount(random, maxMutateCount, mutatePercentProb));
		return rawValues;
	}

	public static LifeSkillShorts CreateLifeSkillQualifications(IRandomSource random, sbyte grade, short[] lifeSkillsAdjust)
	{
		return CreateLifeSkillQualifications(random, grade, lifeSkillsAdjust, null);
	}

	public static LifeSkillShorts CreateLifeSkillQualifications(IRandomSource random, sbyte grade, short[] lifeSkillsAdjust, WeightsSumDistribution weightSumWeightsTable, int maxMutateCount = 4, int mutatePercentProb = 20)
	{
		int sum = GradeToSum(grade, 448, 56);
		CalculateVirtualParentValues(random, lifeSkillsAdjust, out LifeSkillShorts resultValues, weightSumWeightsTable);
		CreateAttributes(random, out var rawValues, ref resultValues, sum, DecodeMutateCount(random, maxMutateCount, mutatePercentProb));
		return rawValues;
	}

	public unsafe static LifeSkillShorts CreateLifeSkillQualifications(IRandomSource random, ref IntelligentCharacterCreationInfo creationInfo, int maxMutateCount = 4, int mutatePercentProb = 20)
	{
		int sum = GradeToSum(creationInfo.GrowingSectGrade, 448, 56);
		CreateAttributes(random, out var rawValues, ref creationInfo.ParentLifeSkillQualificationValues, sum, DecodeMutateCount(random, maxMutateCount, mutatePercentProb));
		if (creationInfo.LifeSkillsLowerBound != null)
		{
			for (int i = 0; i < 16; i++)
			{
				short num = creationInfo.LifeSkillsLowerBound[i];
				short num2 = rawValues.Items[i];
				if (num > num2)
				{
					rawValues.Items[i] = (short)(num + random.Next(11));
				}
			}
		}
		return rawValues;
	}

	public static CombatSkillShorts CreateCombatSkillQualifications(IRandomSource random, sbyte grade, short[] combatSkillsAdjust)
	{
		return CreateCombatSkillQualifications(random, grade, combatSkillsAdjust, null);
	}

	public static CombatSkillShorts CreateCombatSkillQualifications(IRandomSource random, sbyte grade, short[] combatSkillsAdjust, WeightsSumDistribution weightSumWeightsTable, int maxMutateCount = 3, int mutatePercentProb = 20)
	{
		int sum = GradeToSum(grade, 392, 49);
		CalculateVirtualParentValues(random, combatSkillsAdjust, out CombatSkillShorts resultValues, weightSumWeightsTable);
		CreateAttributes(random, out var rawValues, ref resultValues, sum, DecodeMutateCount(random, maxMutateCount, mutatePercentProb));
		return rawValues;
	}

	public unsafe static CombatSkillShorts CreateCombatSkillQualifications(IRandomSource random, ref IntelligentCharacterCreationInfo creationInfo, int maxMutateCount = 3, int mutatePercentProb = 20)
	{
		int sum = GradeToSum(creationInfo.GrowingSectGrade, 392, 49);
		CreateAttributes(random, out var rawValues, ref creationInfo.ParentCombatSkillQualificationValues, sum, DecodeMutateCount(random, maxMutateCount, mutatePercentProb));
		if (creationInfo.CombatSkillsLowerBound != null)
		{
			for (int i = 0; i < 14; i++)
			{
				short num = creationInfo.CombatSkillsLowerBound[i];
				short num2 = rawValues.Items[i];
				if (num > num2)
				{
					rawValues.Items[i] = (short)(num + random.Next(11));
				}
			}
		}
		return rawValues;
	}

	public static short[] GetRandomEnemyMainAttributesAdjust(IRandomSource random, List<(OrganizationItem Sect, OrganizationMemberItem Member)> relatedSectsAndMembers)
	{
		short num = (short)relatedSectsAndMembers.Count;
		short[] array;
		if (num > 1)
		{
			array = new short[6];
			for (int i = 0; i < num; i++)
			{
				short[] mainAttributesAdjust = relatedSectsAndMembers[i].Member.MainAttributesAdjust;
				MergeAdjusts(mainAttributesAdjust, array);
			}
		}
		else
		{
			array = relatedSectsAndMembers[0].Member.MainAttributesAdjust;
		}
		return array;
	}

	public static short[] GetRandomEnemyLifeSkillsAdjust(IRandomSource random, List<(OrganizationItem Sect, OrganizationMemberItem Member)> relatedSectsAndMembers)
	{
		short num = (short)relatedSectsAndMembers.Count;
		short[] array;
		if (num > 1)
		{
			array = new short[16];
			for (int i = 0; i < num; i++)
			{
				short[] lifeSkillsAdjust = relatedSectsAndMembers[i].Member.LifeSkillsAdjust;
				MergeAdjusts(lifeSkillsAdjust, array);
			}
		}
		else
		{
			array = relatedSectsAndMembers[0].Member.LifeSkillsAdjust;
		}
		return array;
	}

	public static short[] GetRandomEnemyCombatSkillsAdjust(IRandomSource random, List<(OrganizationItem Sect, OrganizationMemberItem Member)> relatedSectsAndMembers)
	{
		short num = (short)relatedSectsAndMembers.Count;
		short[] array;
		if (num > 1)
		{
			array = new short[14];
			Array.Fill(array, (short)(-1));
			for (int i = 0; i < num; i++)
			{
				short[] combatSkillsAdjust = relatedSectsAndMembers[i].Member.CombatSkillsAdjust;
				MergeAdjusts(combatSkillsAdjust, array);
			}
		}
		else
		{
			array = relatedSectsAndMembers[0].Member.CombatSkillsAdjust;
		}
		return array;
	}

	public static void MergeAdjusts(short[] fromAdjusts, short[] toAdjusts)
	{
		for (int num = toAdjusts.Length - 1; num >= 0; num--)
		{
			toAdjusts[num] = Math.Max(toAdjusts[num], fromAdjusts[num]);
		}
	}

	public static short[] MergeAdjusts(IEnumerable<short[]> allAdjusts, int length)
	{
		short[] array = new short[length];
		Array.Fill(array, (short)(-1));
		foreach (short[] allAdjust in allAdjusts)
		{
			MergeAdjusts(allAdjust, array);
		}
		return array;
	}

	public static sbyte GetMainAttributeGrade(int sum)
	{
		return SumToGrade(sum, 168, 21);
	}

	public static sbyte GetCombatSkillQualificationGrade(int sum)
	{
		return SumToGrade(sum, 392, 49);
	}

	public static sbyte GetLifeSkillQualificationGrade(int sum)
	{
		return SumToGrade(sum, 448, 56);
	}

	private static int GradeToSum(sbyte grade, int baseVal, int valPerGrade)
	{
		return baseVal + grade * valPerGrade;
	}

	private static int NormalDistribute(IRandomSource random, int num, int span)
	{
		int min = num - span;
		int max = num + span;
		float stdDev = (float)span / 2.1f;
		return RedzenHelper.NormalDistribute(random, num, stdDev, min, max);
	}

	private static sbyte SumToGrade(int sum, int baseVal, int valPerGrade)
	{
		return (sbyte)Math.Clamp((sum - baseVal) / valPerGrade, 0, 127);
	}

	[Obsolete]
	private static int GenerateRandomGradeAdjust(IRandomSource random)
	{
		int num = random.Next(100);
		if (1 == 0)
		{
		}
		int result = ((num < 75) ? ((num < 5) ? (-2) : ((num < 25) ? (-1) : 0)) : ((num < 95) ? 1 : 2));
		if (1 == 0)
		{
		}
		return result;
	}

	private static sbyte TryAdjustGrade(IRandomSource random, sbyte fatherGrade, sbyte motherGrade, sbyte growingGrade)
	{
		sbyte b = Math.Max(fatherGrade, motherGrade);
		int percentProb = AdjustGradeBaseChances[b] + (b - growingGrade) * 5;
		if (!random.CheckPercentProb(percentProb))
		{
			return growingGrade;
		}
		return (sbyte)RandomUtils.GetRandomIndex(AdjustGradeWeights[b], random);
	}

	private static void AssignVirtualParentValuesByOrgMember(IRandomSource random, ref IntelligentCharacterCreationInfo creationInfo, OrganizationMemberItem orgMemberCfg)
	{
		CalculateVirtualParentValues(random, orgMemberCfg.MainAttributesAdjust, out creationInfo.ParentMainAttributeValues, (WeightsSumDistribution)null);
		CalculateVirtualParentValues(random, orgMemberCfg.CombatSkillsAdjust, out creationInfo.ParentCombatSkillQualificationValues, (WeightsSumDistribution)null);
		CalculateVirtualParentValues(random, orgMemberCfg.LifeSkillsAdjust, out creationInfo.ParentLifeSkillQualificationValues, (WeightsSumDistribution)null);
	}

	private static void AssignMergedVirtualParentValuesByOrgMembers(IRandomSource random, ref IntelligentCharacterCreationInfo creationInfo, OrganizationMemberItem orgMemberCfgA, OrganizationMemberItem orgMemberCfgB)
	{
		MergeVirtualParentValues(random, orgMemberCfgA.MainAttributesAdjust, orgMemberCfgB.MainAttributesAdjust, out creationInfo.ParentMainAttributeValues, (WeightsSumDistribution)null);
		MergeVirtualParentValues(random, orgMemberCfgA.CombatSkillsAdjust, orgMemberCfgB.CombatSkillsAdjust, out creationInfo.ParentCombatSkillQualificationValues, (WeightsSumDistribution)null);
		MergeVirtualParentValues(random, orgMemberCfgA.LifeSkillsAdjust, orgMemberCfgB.LifeSkillsAdjust, out creationInfo.ParentLifeSkillQualificationValues, (WeightsSumDistribution)null);
	}

	public unsafe static void CalculateVirtualParentValues(IRandomSource random, short[] weights, out MainAttributes resultValues, WeightsSumDistribution weightAndSums = null)
	{
		fixed (short* items = resultValues.Items)
		{
			CalculateVirtualParentValues(random, weights, 6, weightAndSums ?? GlobalConfig.Instance.MainAttributeWeightsTable, items);
		}
	}

	public unsafe static void CalculateVirtualParentValues(IRandomSource random, short[] weights, out CombatSkillShorts resultValues, WeightsSumDistribution weightAndSums = null)
	{
		fixed (short* items = resultValues.Items)
		{
			CalculateVirtualParentValues(random, weights, 14, weightAndSums ?? GlobalConfig.Instance.CombatSkillQualificationWeightsTable, items);
		}
	}

	public unsafe static void CalculateVirtualParentValues(IRandomSource random, short[] weights, out LifeSkillShorts resultValues, WeightsSumDistribution weightAndSums = null)
	{
		fixed (short* items = resultValues.Items)
		{
			CalculateVirtualParentValues(random, weights, 16, weightAndSums ?? GlobalConfig.Instance.LifeSkillQualificationWeightsTable, items);
		}
	}

	public unsafe static void MergeVirtualParentValues(IRandomSource random, short[] configValuesA, short[] configValuesB, out MainAttributes resultValues, WeightsSumDistribution weightAndSums = null)
	{
		CalculateVirtualParentValues(random, configValuesA, out MainAttributes resultValues2, weightAndSums ?? GlobalConfig.Instance.MainAttributeWeightsTable);
		CalculateVirtualParentValues(random, configValuesB, out MainAttributes resultValues3, weightAndSums ?? GlobalConfig.Instance.MainAttributeWeightsTable);
		for (int i = 0; i < 6; i++)
		{
			resultValues.Items[i] = (short)(resultValues2[i] + resultValues3[i]);
		}
	}

	public unsafe static void MergeVirtualParentValues(IRandomSource random, short[] configValuesA, short[] configValuesB, out CombatSkillShorts resultValues, WeightsSumDistribution weightAndSums = null)
	{
		CalculateVirtualParentValues(random, configValuesA, out CombatSkillShorts resultValues2, weightAndSums ?? GlobalConfig.Instance.CombatSkillQualificationWeightsTable);
		CalculateVirtualParentValues(random, configValuesB, out CombatSkillShorts resultValues3, weightAndSums ?? GlobalConfig.Instance.CombatSkillQualificationWeightsTable);
		for (int i = 0; i < 14; i++)
		{
			resultValues.Items[i] = (short)(resultValues2.Items[i] + resultValues3.Items[i]);
		}
	}

	public unsafe static void MergeVirtualParentValues(IRandomSource random, short[] configValuesA, short[] configValuesB, out LifeSkillShorts resultValues, WeightsSumDistribution weightAndSums = null)
	{
		CalculateVirtualParentValues(random, configValuesA, out LifeSkillShorts resultValues2, weightAndSums ?? GlobalConfig.Instance.LifeSkillQualificationWeightsTable);
		CalculateVirtualParentValues(random, configValuesB, out LifeSkillShorts resultValues3, weightAndSums ?? GlobalConfig.Instance.LifeSkillQualificationWeightsTable);
		for (int i = 0; i < 16; i++)
		{
			resultValues.Items[i] = (short)(resultValues2.Items[i] + resultValues3.Items[i]);
		}
	}

	public unsafe static void CalculateVirtualParentValues(IRandomSource random, short[] weights, int count, WeightsSumDistribution weightAndSums, short* resultValues)
	{
		for (int i = 0; i < count; i++)
		{
			resultValues[i] = weights[i];
		}
		int num = Array.BinarySearch(weightAndSums.Weights, random.Next(weightAndSums.Weights[^1]));
		if (num < 0)
		{
			num = ~num;
		}
		int num2 = weightAndSums.Min + num;
		Span<int> span = stackalloc int[16];
		int num3 = 0;
		for (int j = 0; j < count; j++)
		{
			if (resultValues[j] < 0)
			{
				span[num3++] = j;
			}
			else
			{
				num2 -= resultValues[j];
			}
		}
		if (num3 <= 0)
		{
			return;
		}
		for (short num4 = 9; num4 > 0; num4--)
		{
			int num5 = SplitInteger(random, num2, num3, num4);
			num2 -= num4 * num5;
			while (num5 > 0)
			{
				int index = random.Next(num3--);
				resultValues[span[index]] = num4;
				span[index] = span[num3];
				num5--;
			}
		}
	}

	[Obsolete("Use `CalculateVirtualParentValues` and its interfaces instead.")]
	private unsafe static void CreateVirtualParentValues(IRandomSource random, short[] configValues, short* resultValues, int count)
	{
		for (int i = 0; i < count; i++)
		{
			resultValues[i] = ((configValues[i] >= 0) ? configValues[i] : GenerateRandomWeight(random));
		}
	}

	private unsafe static void MergeCreateVirtualParentValues(IRandomSource random, short[] configValuesA, short[] configValuesB, short* resultValues, int count)
	{
		for (int i = 0; i < count; i++)
		{
			short num = ((configValuesA[i] >= 0) ? configValuesA[i] : GenerateRandomWeight(random));
			short num2 = ((configValuesB[i] >= 0) ? configValuesB[i] : GenerateRandomWeight(random));
			resultValues[i] = (short)(num + num2);
		}
	}

	private unsafe static int GetParentValues(IRandomSource random, short* motherValues, short* fatherValues, short* resultValues, int count)
	{
		int num = 0;
		int num2 = count / 2;
		for (int i = 0; i < count; i++)
		{
			short val = motherValues[i];
			short val2 = fatherValues[i];
			if (num2 > 0 && random.NextBool())
			{
				resultValues[i] = Math.Max(val, val2);
				num += resultValues[i];
				num2--;
			}
			else
			{
				resultValues[i] = Math.Min(val, val2);
				num += resultValues[i];
			}
		}
		return num;
	}

	private unsafe static void DistributeValues(IRandomSource random, short* values, short* weights, int count, int sum)
	{
		int num = AdjustWeightsAndGetSum(weights, count);
		for (int i = 0; i < count; i++)
		{
			short num2 = weights[i];
			int num3 = sum * num2 / num;
			values[i] = (short)NormalDistribute(random, num3, num3 * 20 / 100);
		}
	}

	private unsafe static int AdjustWeightsAndGetSum(short* weight, int count)
	{
		int num = 1;
		for (int i = 0; i < count; i++)
		{
			weight[i] = (short)Math.Min(weight[i] * 10 + 1, 10000);
			num += weight[i];
		}
		short val = (short)(num / 100);
		num = 0;
		for (int j = 0; j < count; j++)
		{
			weight[j] = Math.Max(weight[j], val);
			num += weight[j];
		}
		return num;
	}

	private static short GenerateRandomWeight(IRandomSource random)
	{
		return (short)RedzenHelper.SkewDistribute(random, 6f, 1.5f, -2f, 2, 12);
	}

	private unsafe static void PerformMutation(IRandomSource random, short* values, int count, int mutateCount)
	{
		short num = values[CollectionUtils.GetMaxIndex(values, count)];
		short num2 = values[CollectionUtils.GetMinIndex(values, count)];
		sbyte* ptr = stackalloc sbyte[(int)(uint)(count - 2)];
		sbyte* ptr2 = stackalloc sbyte[(int)(uint)count];
		int num3 = 0;
		int num4 = 0;
		for (sbyte b = 0; b < count; b++)
		{
			short num5 = values[b];
			if (num5 == num || num5 == num2)
			{
				ptr2[num3] = b;
				num3++;
			}
			else
			{
				ptr[num4] = b;
				num4++;
			}
		}
		if (num4 != 0)
		{
			for (int i = 0; i < mutateCount; i++)
			{
				int num6 = random.Next(num3);
				int num7 = random.Next(num4);
				sbyte b2 = ptr2[num6];
				sbyte b3 = ptr[num7];
				ref short reference = ref values[b2];
				short* num8 = values + b3;
				short num9 = values[b3];
				short num10 = values[b2];
				reference = num9;
				*num8 = num10;
				ptr2[num6] = b3;
				ptr[num7] = b2;
			}
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int DecodeMutateCount(IRandomSource random, int maxMutateCount, int percentProb = 20)
	{
		return (maxMutateCount > 0 && random.CheckPercentProb(percentProb)) ? (1 + random.Next(maxMutateCount)) : 0;
	}

	public static void CreateFeatures(IRandomSource random, Character character, ref IntelligentCharacterCreationInfo creationInfo)
	{
		FeatureCreationContext creationContext = new FeatureCreationContext(character, ref creationInfo);
		CreateFeatures(random, ref creationContext);
	}

	public static void CreateFeatures(IRandomSource random, Character character)
	{
		FeatureCreationContext creationContext = new FeatureCreationContext(character);
		CreateFeatures(random, ref creationContext);
	}

	public static void CreateFeatures(IRandomSource random, ref FeatureCreationContext creationContext)
	{
		Dictionary<short, short> featureGroup2Id = new Dictionary<short, short>(16);
		foreach (short featureId in creationContext.FeatureIds)
		{
			AddFeature(featureGroup2Id, featureId);
		}
		creationContext.FeatureIds.Clear();
		GenerateFixedFeatures(ref creationContext, featureGroup2Id);
		if (creationContext.DestinyType >= 0)
		{
			AddFeature(featureGroup2Id, DestinyType.Instance[creationContext.DestinyType].Feature);
		}
		if (creationContext.RandomFeaturesAtCreating)
		{
			GenerateGeneticFeatures(ref creationContext, random, featureGroup2Id);
			AddFeature(featureGroup2Id, CharacterDomain.GetBirthdayFeatureId(creationContext.BirthMonth));
			if (creationContext.CurrAge >= 1 && random.CheckPercentProb(50))
			{
				AddFeature(featureGroup2Id, CharacterDomain.GenerateOneYearOldCatchFeature(random));
			}
			GenerateRandomBasicFeatures(ref creationContext, featureGroup2Id, random);
		}
		ApplyFeatureIds(ref creationContext, featureGroup2Id);
	}

	private static void ApplyFeatureIds(ref FeatureCreationContext context, Dictionary<short, short> featureGroup2Id)
	{
		List<short> featureIds = context.FeatureIds;
		List<short> potentialFeatureIds = context.PotentialFeatureIds;
		short potentialFeaturesAge = context.PotentialFeaturesAge;
		ApplyFeatureGroup(featureGroup2Id, featureIds, 216);
		ApplyFeatureGroup(featureGroup2Id, featureIds, 195);
		ApplyFeatureGroup(featureGroup2Id, featureIds, 183);
		ApplyFeatureGroup(featureGroup2Id, featureIds, 171);
		foreach (KeyValuePair<short, short> item in featureGroup2Id)
		{
			short value = item.Value;
			if (potentialFeaturesAge >= 0 && CharacterFeature.Instance[value].Mergeable)
			{
				potentialFeatureIds.Add(value);
			}
			else
			{
				featureIds.Add(value);
			}
		}
		if (potentialFeaturesAge >= 0)
		{
			int num = potentialFeatureIds.Count * potentialFeaturesAge / 16;
			for (int i = 0; i < num; i++)
			{
				featureIds.Add(potentialFeatureIds[i]);
			}
		}
		featureIds.Sort(CharacterFeatureHelper.FeatureComparer);
	}

	private static void ApplyFeatureGroup(Dictionary<short, short> featureGroup2Id, List<short> featureIds, short groupId)
	{
		if (featureGroup2Id.TryGetValue(groupId, out var value))
		{
			featureIds.Add(value);
			featureGroup2Id.Remove(groupId);
		}
	}

	private static void GenerateFixedFeatures(ref FeatureCreationContext context, Dictionary<short, short> featureGroup2Id)
	{
		short featureId = 195;
		short featureId2 = 216;
		List<short> featureIds = context.FeatureIds;
		int count = featureIds.Count;
		for (int i = 0; i < count; i++)
		{
			short num = featureIds[i];
			switch (CharacterFeature.Instance[num].MutexGroupId)
			{
			case 195:
				featureId = num;
				break;
			case 216:
				featureId2 = num;
				break;
			}
		}
		AddFeature(featureGroup2Id, featureId);
		AddFeature(featureGroup2Id, featureId2);
		for (int j = 0; j < count; j++)
		{
			AddFeature(featureGroup2Id, featureIds[j]);
		}
	}

	private static void GenerateGeneticFeatures(ref FeatureCreationContext context, IRandomSource random, Dictionary<short, short> featureGroup2Id)
	{
		Character mother = context.Mother;
		Character father = context.Father;
		DeadCharacter deadFather = context.DeadFather;
		PregnantState pregnantState = context.PregnantState;
		if (mother == null && father == null && deadFather == null)
		{
			return;
		}
		List<short> list = null;
		List<short> list2 = null;
		if (pregnantState != null)
		{
			list = pregnantState.MotherFeatureIds;
			list2 = pregnantState.FatherFeatureIds;
		}
		else
		{
			if (mother != null)
			{
				list = mother.GetFeatureIds();
			}
			if (father != null)
			{
				list2 = father.GetFeatureIds();
			}
			else if (deadFather != null)
			{
				list2 = deadFather.FeatureIds;
			}
		}
		Dictionary<short, (short, short)> dictionary = new Dictionary<short, (short, short)>();
		if (list2 != null)
		{
			int count = list2.Count;
			for (int i = 0; i < count; i++)
			{
				short num = list2[i];
				CharacterFeatureItem characterFeatureItem = CharacterFeature.Instance[num];
				short mutexGroupId = characterFeatureItem.MutexGroupId;
				sbyte geneticProb = characterFeatureItem.GeneticProb;
				if (characterFeatureItem.Mergeable)
				{
					dictionary[mutexGroupId] = (-1, num);
				}
				else if (geneticProb > 0 && !featureGroup2Id.ContainsKey(mutexGroupId) && random.CheckPercentProb(geneticProb))
				{
					featureGroup2Id.Add(mutexGroupId, num);
				}
			}
		}
		if (list != null)
		{
			int count2 = list.Count;
			for (int j = 0; j < count2; j++)
			{
				short num2 = list[j];
				CharacterFeatureItem characterFeatureItem2 = CharacterFeature.Instance[num2];
				short mutexGroupId2 = characterFeatureItem2.MutexGroupId;
				sbyte geneticProb2 = characterFeatureItem2.GeneticProb;
				if (characterFeatureItem2.Mergeable)
				{
					if (dictionary.TryGetValue(mutexGroupId2, out var value))
					{
						dictionary[mutexGroupId2] = (num2, value.Item2);
					}
				}
				else if (geneticProb2 > 0 && !featureGroup2Id.ContainsKey(mutexGroupId2) && random.CheckPercentProb(geneticProb2))
				{
					featureGroup2Id.Add(mutexGroupId2, num2);
				}
			}
		}
		foreach (KeyValuePair<short, (short, short)> item in dictionary)
		{
			short key = item.Key;
			var (num3, num4) = item.Value;
			if (num3 < 0 || num4 < 0 || featureGroup2Id.ContainsKey(key))
			{
				continue;
			}
			sbyte level = CharacterFeature.Instance[num3].Level;
			sbyte level2 = CharacterFeature.Instance[num4].Level;
			sbyte b = (sbyte)((level + level2) / 2);
			if (level > 0 && level2 > 0)
			{
				int percentProb = (3 - b) * 40;
				if (random.CheckPercentProb(percentProb))
				{
					b++;
				}
			}
			else if (level < 0 && level2 < 0)
			{
				int percentProb2 = (3 + b) * 40;
				if (random.CheckPercentProb(percentProb2))
				{
					b--;
				}
			}
			if (b != 0)
			{
				short mergeableFeatureIdByLevel = CharacterDomain.GetMergeableFeatureIdByLevel(key, b);
				featureGroup2Id.Add(key, mergeableFeatureIdByLevel);
			}
		}
	}

	private static void GenerateRandomBasicFeatures(ref FeatureCreationContext context, Dictionary<short, short> featureGroup2Id, IRandomSource random)
	{
		int num = (context.AllGoodBasicFeature ? 5 : GenerateRandomBasicFeaturesCount(random));
		foreach (KeyValuePair<short, short> item in featureGroup2Id)
		{
			short value = item.Value;
			if (CharacterFeature.Instance[value].Basic)
			{
				num--;
			}
		}
		if (num <= 0)
		{
			return;
		}
		int num2 = random.Next(101);
		sbyte gender = context.Gender;
		for (int i = 0; i < num; i++)
		{
			if (context.AllGoodBasicFeature || random.CheckPercentProb(num2))
			{
				var (key, num3) = CharacterDomain.GetRandomBasicFeature(random, context.IsProtagonist, context.Gender, isPositive: true, featureGroup2Id);
				if (num3 >= 0)
				{
					featureGroup2Id.Add(key, num3);
					num2 -= 20;
				}
			}
			else
			{
				var (key2, num4) = CharacterDomain.GetRandomBasicFeature(random, context.IsProtagonist, context.Gender, isPositive: false, featureGroup2Id);
				if (num4 >= 0)
				{
					featureGroup2Id.Add(key2, num4);
					num2 += 20;
				}
			}
		}
	}

	public static int GenerateRandomBasicFeaturesCount(IRandomSource random)
	{
		return RedzenHelper.SkewDistribute(random, 4f, 0.333f, 3f, 3, 7);
	}

	private static void AddFeature(Dictionary<short, short> featureGroup2Id, short featureId)
	{
		short mutexGroupId = CharacterFeature.Instance[featureId].MutexGroupId;
		featureGroup2Id.TryAdd(mutexGroupId, featureId);
	}
}
