using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Config;
using GameData.Domains.Organization;
using GameData.Utilities;
using Redzen.Random;

namespace GameData.Domains.Character.Creation
{
	// Token: 0x0200083C RID: 2108
	public static class CharacterCreation
	{
		// Token: 0x060075C0 RID: 30144 RVA: 0x0044CA28 File Offset: 0x0044AC28
		public unsafe static sbyte CalcGrowingSectGradeAndAssignWeights(IRandomSource random, ref IntelligentCharacterCreationInfo creationInfo, sbyte idealSectId)
		{
			bool flag = creationInfo.MotherCharId >= 0 && creationInfo.ActualFatherCharId >= 0;
			sbyte result;
			if (flag)
			{
				MainAttributes motherAttributes = creationInfo.Mother.GetBaseMainAttributes();
				Character actualFather = creationInfo.ActualFather;
				MainAttributes fatherAttributes = (actualFather != null) ? actualFather.GetBaseMainAttributes() : creationInfo.ActualDeadFather.BaseMainAttributes;
				sbyte attrGrade;
				fixed (short* ptr = &creationInfo.ParentMainAttributeValues.Items.FixedElementField)
				{
					short* pAttr = ptr;
					int attrSum = CharacterCreation.GetParentValues(random, &motherAttributes.Items.FixedElementField, &fatherAttributes.Items.FixedElementField, pAttr, 6);
					attrGrade = CharacterCreation.SumToGrade(attrSum, 168, 21);
				}
				LifeSkillShorts motherLifeSkills = *creationInfo.Mother.GetBaseLifeSkillQualifications();
				Character actualFather2 = creationInfo.ActualFather;
				LifeSkillShorts fatherLifeSkills = (actualFather2 != null) ? (*actualFather2.GetBaseLifeSkillQualifications()) : creationInfo.ActualDeadFather.BaseLifeSkillQualifications;
				sbyte lifeSkillGrade;
				fixed (short* ptr2 = &creationInfo.ParentLifeSkillQualificationValues.Items.FixedElementField)
				{
					short* pLifeSkillQualifications = ptr2;
					int lifeSkillSum = CharacterCreation.GetParentValues(random, &motherLifeSkills.Items.FixedElementField, &fatherLifeSkills.Items.FixedElementField, pLifeSkillQualifications, 16);
					lifeSkillGrade = CharacterCreation.SumToGrade(lifeSkillSum, 448, 56);
				}
				CombatSkillShorts motherCombatSkills = *creationInfo.Mother.GetBaseCombatSkillQualifications();
				Character actualFather3 = creationInfo.ActualFather;
				CombatSkillShorts fatherCombatSkills = (actualFather3 != null) ? (*actualFather3.GetBaseCombatSkillQualifications()) : creationInfo.ActualDeadFather.BaseCombatSkillQualifications;
				sbyte combatSkillGrade;
				fixed (short* ptr3 = &creationInfo.ParentCombatSkillQualificationValues.Items.FixedElementField)
				{
					short* pCombatSkillQualifications = ptr3;
					int combatSkillSum = CharacterCreation.GetParentValues(random, &motherCombatSkills.Items.FixedElementField, &fatherCombatSkills.Items.FixedElementField, pCombatSkillQualifications, 14);
					combatSkillGrade = CharacterCreation.SumToGrade(combatSkillSum, 392, 49);
				}
				sbyte grade = (sbyte)Math.Clamp((int)((attrGrade + lifeSkillGrade + combatSkillGrade) / 3), 0, 8);
				bool allowRandomGrowingGradeAdjust = creationInfo.AllowRandomGrowingGradeAdjust;
				if (allowRandomGrowingGradeAdjust)
				{
					sbyte motherGrade = creationInfo.Mother.GetOrganizationInfo().Grade;
					Character father = creationInfo.Father;
					sbyte fatherGrade = (father != null) ? father.GetOrganizationInfo().Grade : 0;
					grade = CharacterCreation.TryAdjustGrade(random, fatherGrade, motherGrade, grade);
				}
				result = grade;
			}
			else
			{
				bool flag2 = creationInfo.MotherCharId >= 0;
				if (flag2)
				{
					creationInfo.ParentMainAttributeValues = creationInfo.Mother.GetBaseMainAttributes();
					int attrSum2 = creationInfo.ParentMainAttributeValues.GetSum();
					sbyte attrGrade2 = CharacterCreation.SumToGrade(attrSum2, 168, 21);
					creationInfo.ParentLifeSkillQualificationValues = *creationInfo.Mother.GetBaseLifeSkillQualifications();
					int lifeSkillSum2 = creationInfo.ParentLifeSkillQualificationValues.GetSum();
					sbyte lifeSkillGrade2 = CharacterCreation.SumToGrade(lifeSkillSum2, 448, 56);
					creationInfo.ParentCombatSkillQualificationValues = *creationInfo.Mother.GetBaseCombatSkillQualifications();
					int combatSkillSum2 = creationInfo.ParentCombatSkillQualificationValues.GetSum();
					sbyte combatSkillGrade2 = CharacterCreation.SumToGrade(combatSkillSum2, 392, 49);
					sbyte grade2 = (sbyte)Math.Clamp((int)((attrGrade2 + lifeSkillGrade2 + combatSkillGrade2) / 3), 0, 8);
					bool allowRandomGrowingGradeAdjust2 = creationInfo.AllowRandomGrowingGradeAdjust;
					if (allowRandomGrowingGradeAdjust2)
					{
						sbyte motherGrade2 = creationInfo.Mother.GetOrganizationInfo().Grade;
						Character father2 = creationInfo.Father;
						sbyte fatherGrade2 = (father2 != null) ? father2.GetOrganizationInfo().Grade : 0;
						grade2 = CharacterCreation.TryAdjustGrade(random, fatherGrade2, motherGrade2, grade2);
					}
					result = grade2;
				}
				else
				{
					sbyte templateGrade = creationInfo.OrgInfo.Grade;
					sbyte growingGrade = (sbyte)RedzenHelper.NormalDistribute(random, (float)creationInfo.OrgInfo.Grade, 1.2f);
					bool flag3 = growingGrade < 0 || growingGrade > 8;
					if (flag3)
					{
						growingGrade = creationInfo.OrgInfo.Grade;
					}
					bool flag4 = creationInfo.GrowingSectId < 0;
					if (flag4)
					{
						switch ((idealSectId >= 0 && idealSectId != creationInfo.OrgInfo.OrgTemplateId) ? (OrganizationDomain.IsSect(creationInfo.OrgInfo.OrgTemplateId) ? RandomUtils.GetRandomIndex(CharacterCreation.SectMemberGenerationTypeWeights, random) : RandomUtils.GetRandomIndex(CharacterCreation.CivilianGenerationTypeWeights, random)) : 0)
						{
						case 0:
						{
							OrganizationMemberItem orgMemberCfg = OrganizationDomain.GetOrgMemberConfig(creationInfo.OrgInfo.OrgTemplateId, templateGrade);
							CharacterCreation.AssignVirtualParentValuesByOrgMember(random, ref creationInfo, orgMemberCfg);
							break;
						}
						case 1:
						{
							OrganizationMemberItem idealSectOrgMemberCfg = OrganizationDomain.GetOrgMemberConfig(idealSectId, templateGrade);
							CharacterCreation.AssignVirtualParentValuesByOrgMember(random, ref creationInfo, idealSectOrgMemberCfg);
							break;
						}
						case 2:
						{
							OrganizationMemberItem orgMemberCfg2 = OrganizationDomain.GetOrgMemberConfig(creationInfo.OrgInfo.OrgTemplateId, templateGrade);
							OrganizationMemberItem idealSectOrgMemberCfg2 = OrganizationDomain.GetOrgMemberConfig(idealSectId, templateGrade);
							CharacterCreation.AssignMergedVirtualParentValuesByOrgMembers(random, ref creationInfo, orgMemberCfg2, idealSectOrgMemberCfg2);
							break;
						}
						}
					}
					else
					{
						OrganizationMemberItem orgMemberCfg3 = OrganizationDomain.GetOrgMemberConfig(creationInfo.GrowingSectId, templateGrade);
						CharacterCreation.AssignVirtualParentValuesByOrgMember(random, ref creationInfo, orgMemberCfg3);
					}
					result = growingGrade;
				}
			}
			return result;
		}

		// Token: 0x060075C1 RID: 30145 RVA: 0x0044CE8C File Offset: 0x0044B08C
		public static ValueTuple<int, int> Reservoir(IRandomSource random, ValueTuple<int, int> current, ValueTuple<int, int> newSample)
		{
			int total = current.Item2 + newSample.Item2;
			return new ValueTuple<int, int>((random.NextFloat() * (float)total < (float)newSample.Item2) ? newSample.Item1 : current.Item1, total);
		}

		// Token: 0x060075C2 RID: 30146 RVA: 0x0044CED4 File Offset: 0x0044B0D4
		public static int SplitInteger(IRandomSource random, int number, int freeSpace, int max)
		{
			bool flag = number >= freeSpace * max;
			int result;
			if (flag)
			{
				result = freeSpace;
			}
			else
			{
				bool flag2 = number <= freeSpace;
				if (flag2)
				{
					result = ((max == 1) ? freeSpace : 0);
				}
				else
				{
					ValueTuple<int, int> ret = new ValueTuple<int, int>(0, 0);
					int i = Math.Min(number / max, (number - freeSpace) / (max - 1));
					int curr;
					int maxAvail;
					while (i >= 0 && (maxAvail = (freeSpace - i) * (max - 1) - (curr = number - i * max) + 1) > 0)
					{
						ret = CharacterCreation.Reservoir(random, ret, new ValueTuple<int, int>(i, maxAvail * (curr - (freeSpace - i) + 1)));
						i--;
					}
					result = ret.Item1;
				}
			}
			return result;
		}

		// Token: 0x060075C3 RID: 30147 RVA: 0x0044CF80 File Offset: 0x0044B180
		public unsafe static void CreateAttributes(IRandomSource random, out MainAttributes rawValues, ref MainAttributes rawWeights, int sum, int mutateCount = 0, int count = -1)
		{
			fixed (short* ptr = &rawValues.Items.FixedElementField)
			{
				short* values = ptr;
				fixed (short* ptr2 = &rawWeights.Items.FixedElementField)
				{
					short* weights = ptr2;
					CharacterCreation.CreateAttributesImpl(random, values, weights, 6, sum, mutateCount);
				}
			}
		}

		// Token: 0x060075C4 RID: 30148 RVA: 0x0044CFC0 File Offset: 0x0044B1C0
		public unsafe static void CreateAttributes(IRandomSource random, out CombatSkillShorts rawValues, ref CombatSkillShorts rawWeights, int sum, int mutateCount = 0, int count = -1)
		{
			fixed (short* ptr = &rawValues.Items.FixedElementField)
			{
				short* values = ptr;
				fixed (short* ptr2 = &rawWeights.Items.FixedElementField)
				{
					short* weights = ptr2;
					CharacterCreation.CreateAttributesImpl(random, values, weights, 14, sum, mutateCount);
				}
			}
		}

		// Token: 0x060075C5 RID: 30149 RVA: 0x0044D000 File Offset: 0x0044B200
		public unsafe static void CreateAttributes(IRandomSource random, out LifeSkillShorts rawValues, ref LifeSkillShorts rawWeights, int sum, int mutateCount = 0, int count = -1)
		{
			fixed (short* ptr = &rawValues.Items.FixedElementField)
			{
				short* values = ptr;
				fixed (short* ptr2 = &rawWeights.Items.FixedElementField)
				{
					short* weights = ptr2;
					CharacterCreation.CreateAttributesImpl(random, values, weights, 16, sum, mutateCount);
				}
			}
		}

		// Token: 0x060075C6 RID: 30150 RVA: 0x0044D040 File Offset: 0x0044B240
		public unsafe static void CreateAttributesImpl(IRandomSource random, short* values, short* weights, int count, int sum, int mutateCount = 0)
		{
			CharacterCreation.DistributeValues(random, values, weights, count, sum);
			bool flag = mutateCount > 0;
			if (flag)
			{
				CharacterCreation.PerformMutation(random, values, count, mutateCount);
			}
		}

		// Token: 0x060075C7 RID: 30151 RVA: 0x0044D070 File Offset: 0x0044B270
		public static MainAttributes CreateMainAttributes(IRandomSource random, sbyte grade, short[] mainAttributesAdjust)
		{
			return CharacterCreation.CreateMainAttributes(random, grade, mainAttributesAdjust, null, 2, 20);
		}

		// Token: 0x060075C8 RID: 30152 RVA: 0x0044D080 File Offset: 0x0044B280
		public static MainAttributes CreateMainAttributes(IRandomSource random, sbyte grade, short[] mainAttributesAdjust, WeightsSumDistribution weightSumWeightsTable, int maxMutateCount = 2, int mutatePercentProb = 20)
		{
			int sum = CharacterCreation.GradeToSum(grade, 168, 21);
			MainAttributes virtualParentAttributes;
			CharacterCreation.CalculateVirtualParentValues(random, mainAttributesAdjust, out virtualParentAttributes, weightSumWeightsTable);
			MainAttributes mainAttributes;
			CharacterCreation.CreateAttributes(random, out mainAttributes, ref virtualParentAttributes, sum, CharacterCreation.DecodeMutateCount(random, maxMutateCount, mutatePercentProb), -1);
			return mainAttributes;
		}

		// Token: 0x060075C9 RID: 30153 RVA: 0x0044D0C4 File Offset: 0x0044B2C4
		public static MainAttributes CreateMainAttributes(IRandomSource random, ref IntelligentCharacterCreationInfo creationInfo, int maxMutateCount = 2, int mutatePercentProb = 20)
		{
			int sum = CharacterCreation.GradeToSum(creationInfo.GrowingSectGrade, 168, 21);
			MainAttributes parentValues = creationInfo.ParentMainAttributeValues;
			MainAttributes mainAttributes;
			CharacterCreation.CreateAttributes(random, out mainAttributes, ref parentValues, sum, CharacterCreation.DecodeMutateCount(random, maxMutateCount, mutatePercentProb), -1);
			return mainAttributes;
		}

		// Token: 0x060075CA RID: 30154 RVA: 0x0044D106 File Offset: 0x0044B306
		public static LifeSkillShorts CreateLifeSkillQualifications(IRandomSource random, sbyte grade, short[] lifeSkillsAdjust)
		{
			return CharacterCreation.CreateLifeSkillQualifications(random, grade, lifeSkillsAdjust, null, 4, 20);
		}

		// Token: 0x060075CB RID: 30155 RVA: 0x0044D114 File Offset: 0x0044B314
		public static LifeSkillShorts CreateLifeSkillQualifications(IRandomSource random, sbyte grade, short[] lifeSkillsAdjust, WeightsSumDistribution weightSumWeightsTable, int maxMutateCount = 4, int mutatePercentProb = 20)
		{
			int sum = CharacterCreation.GradeToSum(grade, 448, 56);
			LifeSkillShorts virtualParentValues;
			CharacterCreation.CalculateVirtualParentValues(random, lifeSkillsAdjust, out virtualParentValues, weightSumWeightsTable);
			LifeSkillShorts lifeSkillQualifications;
			CharacterCreation.CreateAttributes(random, out lifeSkillQualifications, ref virtualParentValues, sum, CharacterCreation.DecodeMutateCount(random, maxMutateCount, mutatePercentProb), -1);
			return lifeSkillQualifications;
		}

		// Token: 0x060075CC RID: 30156 RVA: 0x0044D158 File Offset: 0x0044B358
		public unsafe static LifeSkillShorts CreateLifeSkillQualifications(IRandomSource random, ref IntelligentCharacterCreationInfo creationInfo, int maxMutateCount = 4, int mutatePercentProb = 20)
		{
			int sum = CharacterCreation.GradeToSum(creationInfo.GrowingSectGrade, 448, 56);
			LifeSkillShorts lifeSkillQualifications;
			CharacterCreation.CreateAttributes(random, out lifeSkillQualifications, ref creationInfo.ParentLifeSkillQualificationValues, sum, CharacterCreation.DecodeMutateCount(random, maxMutateCount, mutatePercentProb), -1);
			bool flag = creationInfo.LifeSkillsLowerBound != null;
			if (flag)
			{
				for (int lifeSkillType = 0; lifeSkillType < 16; lifeSkillType++)
				{
					short lowerBound = creationInfo.LifeSkillsLowerBound[lifeSkillType];
					short currVal = *(ref lifeSkillQualifications.Items.FixedElementField + (IntPtr)lifeSkillType * 2);
					bool flag2 = lowerBound <= currVal;
					if (!flag2)
					{
						*(ref lifeSkillQualifications.Items.FixedElementField + (IntPtr)lifeSkillType * 2) = (short)((int)lowerBound + random.Next(11));
					}
				}
			}
			return lifeSkillQualifications;
		}

		// Token: 0x060075CD RID: 30157 RVA: 0x0044D20A File Offset: 0x0044B40A
		public static CombatSkillShorts CreateCombatSkillQualifications(IRandomSource random, sbyte grade, short[] combatSkillsAdjust)
		{
			return CharacterCreation.CreateCombatSkillQualifications(random, grade, combatSkillsAdjust, null, 3, 20);
		}

		// Token: 0x060075CE RID: 30158 RVA: 0x0044D218 File Offset: 0x0044B418
		public static CombatSkillShorts CreateCombatSkillQualifications(IRandomSource random, sbyte grade, short[] combatSkillsAdjust, WeightsSumDistribution weightSumWeightsTable, int maxMutateCount = 3, int mutatePercentProb = 20)
		{
			int sum = CharacterCreation.GradeToSum(grade, 392, 49);
			CombatSkillShorts virtualParentValues;
			CharacterCreation.CalculateVirtualParentValues(random, combatSkillsAdjust, out virtualParentValues, weightSumWeightsTable);
			CombatSkillShorts combatSkillQualifications;
			CharacterCreation.CreateAttributes(random, out combatSkillQualifications, ref virtualParentValues, sum, CharacterCreation.DecodeMutateCount(random, maxMutateCount, mutatePercentProb), -1);
			return combatSkillQualifications;
		}

		// Token: 0x060075CF RID: 30159 RVA: 0x0044D25C File Offset: 0x0044B45C
		public unsafe static CombatSkillShorts CreateCombatSkillQualifications(IRandomSource random, ref IntelligentCharacterCreationInfo creationInfo, int maxMutateCount = 3, int mutatePercentProb = 20)
		{
			int sum = CharacterCreation.GradeToSum(creationInfo.GrowingSectGrade, 392, 49);
			CombatSkillShorts combatSkillQualifications;
			CharacterCreation.CreateAttributes(random, out combatSkillQualifications, ref creationInfo.ParentCombatSkillQualificationValues, sum, CharacterCreation.DecodeMutateCount(random, maxMutateCount, mutatePercentProb), -1);
			bool flag = creationInfo.CombatSkillsLowerBound != null;
			if (flag)
			{
				for (int combatSkillType = 0; combatSkillType < 14; combatSkillType++)
				{
					short lowerBound = creationInfo.CombatSkillsLowerBound[combatSkillType];
					short currVal = *(ref combatSkillQualifications.Items.FixedElementField + (IntPtr)combatSkillType * 2);
					bool flag2 = lowerBound <= currVal;
					if (!flag2)
					{
						*(ref combatSkillQualifications.Items.FixedElementField + (IntPtr)combatSkillType * 2) = (short)((int)lowerBound + random.Next(11));
					}
				}
			}
			return combatSkillQualifications;
		}

		// Token: 0x060075D0 RID: 30160 RVA: 0x0044D310 File Offset: 0x0044B510
		public static short[] GetRandomEnemyMainAttributesAdjust(IRandomSource random, [TupleElementNames(new string[]
		{
			"Sect",
			"Member"
		})] List<ValueTuple<OrganizationItem, OrganizationMemberItem>> relatedSectsAndMembers)
		{
			short relatedSectsCount = (short)relatedSectsAndMembers.Count;
			bool flag = relatedSectsCount > 1;
			short[] mainAttributesAdjust;
			if (flag)
			{
				mainAttributesAdjust = new short[6];
				for (int i = 0; i < (int)relatedSectsCount; i++)
				{
					short[] currAdjust = relatedSectsAndMembers[i].Item2.MainAttributesAdjust;
					CharacterCreation.MergeAdjusts(currAdjust, mainAttributesAdjust);
				}
			}
			else
			{
				mainAttributesAdjust = relatedSectsAndMembers[0].Item2.MainAttributesAdjust;
			}
			return mainAttributesAdjust;
		}

		// Token: 0x060075D1 RID: 30161 RVA: 0x0044D384 File Offset: 0x0044B584
		public static short[] GetRandomEnemyLifeSkillsAdjust(IRandomSource random, [TupleElementNames(new string[]
		{
			"Sect",
			"Member"
		})] List<ValueTuple<OrganizationItem, OrganizationMemberItem>> relatedSectsAndMembers)
		{
			short relatedSectsCount = (short)relatedSectsAndMembers.Count;
			bool flag = relatedSectsCount > 1;
			short[] lifeSkillsAdjust;
			if (flag)
			{
				lifeSkillsAdjust = new short[16];
				for (int i = 0; i < (int)relatedSectsCount; i++)
				{
					short[] currAdjust = relatedSectsAndMembers[i].Item2.LifeSkillsAdjust;
					CharacterCreation.MergeAdjusts(currAdjust, lifeSkillsAdjust);
				}
			}
			else
			{
				lifeSkillsAdjust = relatedSectsAndMembers[0].Item2.LifeSkillsAdjust;
			}
			return lifeSkillsAdjust;
		}

		// Token: 0x060075D2 RID: 30162 RVA: 0x0044D3F8 File Offset: 0x0044B5F8
		public static short[] GetRandomEnemyCombatSkillsAdjust(IRandomSource random, [TupleElementNames(new string[]
		{
			"Sect",
			"Member"
		})] List<ValueTuple<OrganizationItem, OrganizationMemberItem>> relatedSectsAndMembers)
		{
			short relatedSectsCount = (short)relatedSectsAndMembers.Count;
			bool flag = relatedSectsCount > 1;
			short[] combatSkillAdjusts;
			if (flag)
			{
				combatSkillAdjusts = new short[14];
				Array.Fill<short>(combatSkillAdjusts, -1);
				for (int i = 0; i < (int)relatedSectsCount; i++)
				{
					short[] currAdjust = relatedSectsAndMembers[i].Item2.CombatSkillsAdjust;
					CharacterCreation.MergeAdjusts(currAdjust, combatSkillAdjusts);
				}
			}
			else
			{
				combatSkillAdjusts = relatedSectsAndMembers[0].Item2.CombatSkillsAdjust;
			}
			return combatSkillAdjusts;
		}

		// Token: 0x060075D3 RID: 30163 RVA: 0x0044D474 File Offset: 0x0044B674
		public static void MergeAdjusts(short[] fromAdjusts, short[] toAdjusts)
		{
			for (int i = toAdjusts.Length - 1; i >= 0; i--)
			{
				toAdjusts[i] = Math.Max(toAdjusts[i], fromAdjusts[i]);
			}
		}

		// Token: 0x060075D4 RID: 30164 RVA: 0x0044D4A8 File Offset: 0x0044B6A8
		public static short[] MergeAdjusts(IEnumerable<short[]> allAdjusts, int length)
		{
			short[] adjusts = new short[length];
			Array.Fill<short>(adjusts, -1);
			foreach (short[] currAdjusts in allAdjusts)
			{
				CharacterCreation.MergeAdjusts(currAdjusts, adjusts);
			}
			return adjusts;
		}

		// Token: 0x060075D5 RID: 30165 RVA: 0x0044D508 File Offset: 0x0044B708
		public static sbyte GetMainAttributeGrade(int sum)
		{
			return CharacterCreation.SumToGrade(sum, 168, 21);
		}

		// Token: 0x060075D6 RID: 30166 RVA: 0x0044D517 File Offset: 0x0044B717
		public static sbyte GetCombatSkillQualificationGrade(int sum)
		{
			return CharacterCreation.SumToGrade(sum, 392, 49);
		}

		// Token: 0x060075D7 RID: 30167 RVA: 0x0044D526 File Offset: 0x0044B726
		public static sbyte GetLifeSkillQualificationGrade(int sum)
		{
			return CharacterCreation.SumToGrade(sum, 448, 56);
		}

		// Token: 0x060075D8 RID: 30168 RVA: 0x0044D535 File Offset: 0x0044B735
		private static int GradeToSum(sbyte grade, int baseVal, int valPerGrade)
		{
			return baseVal + (int)grade * valPerGrade;
		}

		// Token: 0x060075D9 RID: 30169 RVA: 0x0044D53C File Offset: 0x0044B73C
		private static int NormalDistribute(IRandomSource random, int num, int span)
		{
			int softMin = num - span;
			int softMax = num + span;
			float stdDev = (float)span / 2.1f;
			return RedzenHelper.NormalDistribute(random, (float)num, stdDev, softMin, softMax);
		}

		// Token: 0x060075DA RID: 30170 RVA: 0x0044D56A File Offset: 0x0044B76A
		private static sbyte SumToGrade(int sum, int baseVal, int valPerGrade)
		{
			return (sbyte)Math.Clamp((sum - baseVal) / valPerGrade, 0, 127);
		}

		// Token: 0x060075DB RID: 30171 RVA: 0x0044D57C File Offset: 0x0044B77C
		[Obsolete]
		private static int GenerateRandomGradeAdjust(IRandomSource random)
		{
			int num = random.Next(100);
			if (!true)
			{
			}
			int result;
			if (num < 75)
			{
				if (num >= 5)
				{
					if (num >= 25)
					{
						result = 0;
					}
					else
					{
						result = -1;
					}
				}
				else
				{
					result = -2;
				}
			}
			else if (num >= 95)
			{
				result = 2;
			}
			else
			{
				result = 1;
			}
			if (!true)
			{
			}
			return result;
		}

		// Token: 0x060075DC RID: 30172 RVA: 0x0044D5C8 File Offset: 0x0044B7C8
		private static sbyte TryAdjustGrade(IRandomSource random, sbyte fatherGrade, sbyte motherGrade, sbyte growingGrade)
		{
			sbyte highestGrade = Math.Max(fatherGrade, motherGrade);
			int chance = (int)(CharacterCreation.AdjustGradeBaseChances[(int)highestGrade] + (highestGrade - growingGrade) * 5);
			bool flag = !random.CheckPercentProb(chance);
			sbyte result;
			if (flag)
			{
				result = growingGrade;
			}
			else
			{
				sbyte adjustedGrade = (sbyte)RandomUtils.GetRandomIndex(CharacterCreation.AdjustGradeWeights[(int)highestGrade], random);
				result = adjustedGrade;
			}
			return result;
		}

		// Token: 0x060075DD RID: 30173 RVA: 0x0044D615 File Offset: 0x0044B815
		private static void AssignVirtualParentValuesByOrgMember(IRandomSource random, ref IntelligentCharacterCreationInfo creationInfo, OrganizationMemberItem orgMemberCfg)
		{
			CharacterCreation.CalculateVirtualParentValues(random, orgMemberCfg.MainAttributesAdjust, out creationInfo.ParentMainAttributeValues, null);
			CharacterCreation.CalculateVirtualParentValues(random, orgMemberCfg.CombatSkillsAdjust, out creationInfo.ParentCombatSkillQualificationValues, null);
			CharacterCreation.CalculateVirtualParentValues(random, orgMemberCfg.LifeSkillsAdjust, out creationInfo.ParentLifeSkillQualificationValues, null);
		}

		// Token: 0x060075DE RID: 30174 RVA: 0x0044D654 File Offset: 0x0044B854
		private static void AssignMergedVirtualParentValuesByOrgMembers(IRandomSource random, ref IntelligentCharacterCreationInfo creationInfo, OrganizationMemberItem orgMemberCfgA, OrganizationMemberItem orgMemberCfgB)
		{
			CharacterCreation.MergeVirtualParentValues(random, orgMemberCfgA.MainAttributesAdjust, orgMemberCfgB.MainAttributesAdjust, out creationInfo.ParentMainAttributeValues, null);
			CharacterCreation.MergeVirtualParentValues(random, orgMemberCfgA.CombatSkillsAdjust, orgMemberCfgB.CombatSkillsAdjust, out creationInfo.ParentCombatSkillQualificationValues, null);
			CharacterCreation.MergeVirtualParentValues(random, orgMemberCfgA.LifeSkillsAdjust, orgMemberCfgB.LifeSkillsAdjust, out creationInfo.ParentLifeSkillQualificationValues, null);
		}

		// Token: 0x060075DF RID: 30175 RVA: 0x0044D6B0 File Offset: 0x0044B8B0
		public unsafe static void CalculateVirtualParentValues(IRandomSource random, short[] weights, out MainAttributes resultValues, WeightsSumDistribution weightAndSums = null)
		{
			fixed (short* ptr = &resultValues.Items.FixedElementField)
			{
				short* values = ptr;
				CharacterCreation.CalculateVirtualParentValues(random, weights, 6, weightAndSums ?? GlobalConfig.Instance.MainAttributeWeightsTable, values);
			}
		}

		// Token: 0x060075E0 RID: 30176 RVA: 0x0044D6EC File Offset: 0x0044B8EC
		public unsafe static void CalculateVirtualParentValues(IRandomSource random, short[] weights, out CombatSkillShorts resultValues, WeightsSumDistribution weightAndSums = null)
		{
			fixed (short* ptr = &resultValues.Items.FixedElementField)
			{
				short* values = ptr;
				CharacterCreation.CalculateVirtualParentValues(random, weights, 14, weightAndSums ?? GlobalConfig.Instance.CombatSkillQualificationWeightsTable, values);
			}
		}

		// Token: 0x060075E1 RID: 30177 RVA: 0x0044D728 File Offset: 0x0044B928
		public unsafe static void CalculateVirtualParentValues(IRandomSource random, short[] weights, out LifeSkillShorts resultValues, WeightsSumDistribution weightAndSums = null)
		{
			fixed (short* ptr = &resultValues.Items.FixedElementField)
			{
				short* values = ptr;
				CharacterCreation.CalculateVirtualParentValues(random, weights, 16, weightAndSums ?? GlobalConfig.Instance.LifeSkillQualificationWeightsTable, values);
			}
		}

		// Token: 0x060075E2 RID: 30178 RVA: 0x0044D764 File Offset: 0x0044B964
		public unsafe static void MergeVirtualParentValues(IRandomSource random, short[] configValuesA, short[] configValuesB, out MainAttributes resultValues, WeightsSumDistribution weightAndSums = null)
		{
			MainAttributes configA;
			CharacterCreation.CalculateVirtualParentValues(random, configValuesA, out configA, weightAndSums ?? GlobalConfig.Instance.MainAttributeWeightsTable);
			MainAttributes configB;
			CharacterCreation.CalculateVirtualParentValues(random, configValuesB, out configB, weightAndSums ?? GlobalConfig.Instance.MainAttributeWeightsTable);
			for (int i = 0; i < 6; i++)
			{
				*(ref resultValues.Items.FixedElementField + (IntPtr)i * 2) = *configA[i] + *configB[i];
			}
		}

		// Token: 0x060075E3 RID: 30179 RVA: 0x0044D7DC File Offset: 0x0044B9DC
		public unsafe static void MergeVirtualParentValues(IRandomSource random, short[] configValuesA, short[] configValuesB, out CombatSkillShorts resultValues, WeightsSumDistribution weightAndSums = null)
		{
			CombatSkillShorts configA;
			CharacterCreation.CalculateVirtualParentValues(random, configValuesA, out configA, weightAndSums ?? GlobalConfig.Instance.CombatSkillQualificationWeightsTable);
			CombatSkillShorts configB;
			CharacterCreation.CalculateVirtualParentValues(random, configValuesB, out configB, weightAndSums ?? GlobalConfig.Instance.CombatSkillQualificationWeightsTable);
			for (int i = 0; i < 14; i++)
			{
				*(ref resultValues.Items.FixedElementField + (IntPtr)i * 2) = *(ref configA.Items.FixedElementField + (IntPtr)i * 2) + *(ref configB.Items.FixedElementField + (IntPtr)i * 2);
			}
		}

		// Token: 0x060075E4 RID: 30180 RVA: 0x0044D868 File Offset: 0x0044BA68
		public unsafe static void MergeVirtualParentValues(IRandomSource random, short[] configValuesA, short[] configValuesB, out LifeSkillShorts resultValues, WeightsSumDistribution weightAndSums = null)
		{
			LifeSkillShorts configA;
			CharacterCreation.CalculateVirtualParentValues(random, configValuesA, out configA, weightAndSums ?? GlobalConfig.Instance.LifeSkillQualificationWeightsTable);
			LifeSkillShorts configB;
			CharacterCreation.CalculateVirtualParentValues(random, configValuesB, out configB, weightAndSums ?? GlobalConfig.Instance.LifeSkillQualificationWeightsTable);
			for (int i = 0; i < 16; i++)
			{
				*(ref resultValues.Items.FixedElementField + (IntPtr)i * 2) = *(ref configA.Items.FixedElementField + (IntPtr)i * 2) + *(ref configB.Items.FixedElementField + (IntPtr)i * 2);
			}
		}

		// Token: 0x060075E5 RID: 30181 RVA: 0x0044D8F4 File Offset: 0x0044BAF4
		public unsafe static void CalculateVirtualParentValues(IRandomSource random, short[] weights, int count, WeightsSumDistribution weightAndSums, short* resultValues)
		{
			for (int i = 0; i < count; i++)
			{
				resultValues[i] = weights[i];
			}
			int[] weights2 = weightAndSums.Weights;
			int[] weights3 = weightAndSums.Weights;
			int offset = Array.BinarySearch<int>(weights2, random.Next(weights3[weights3.Length - 1]));
			bool flag = offset < 0;
			if (flag)
			{
				offset = ~offset;
			}
			int targetWeights = weightAndSums.Min + offset;
			Span<int> span = new Span<int>(stackalloc byte[(UIntPtr)64], 16);
			Span<int> indexes = span;
			int randomWeights = 0;
			for (int j = 0; j < count; j++)
			{
				bool flag2 = resultValues[j] < 0;
				if (flag2)
				{
					*indexes[randomWeights++] = j;
				}
				else
				{
					targetWeights -= (int)resultValues[j];
				}
			}
			bool flag3 = randomWeights > 0;
			if (flag3)
			{
				for (short k = 9; k > 0; k -= 1)
				{
					int counter = CharacterCreation.SplitInteger(random, targetWeights, randomWeights, (int)k);
					targetWeights -= (int)k * counter;
					while (counter > 0)
					{
						int rndIdx = random.Next(randomWeights--);
						resultValues[*indexes[rndIdx]] = k;
						*indexes[rndIdx] = *indexes[randomWeights];
						counter--;
					}
				}
			}
		}

		// Token: 0x060075E6 RID: 30182 RVA: 0x0044DA38 File Offset: 0x0044BC38
		[Obsolete("Use `CalculateVirtualParentValues` and its interfaces instead.")]
		private unsafe static void CreateVirtualParentValues(IRandomSource random, short[] configValues, short* resultValues, int count)
		{
			for (int index = 0; index < count; index++)
			{
				resultValues[index] = ((configValues[index] >= 0) ? configValues[index] : CharacterCreation.GenerateRandomWeight(random));
			}
		}

		// Token: 0x060075E7 RID: 30183 RVA: 0x0044DA70 File Offset: 0x0044BC70
		private unsafe static void MergeCreateVirtualParentValues(IRandomSource random, short[] configValuesA, short[] configValuesB, short* resultValues, int count)
		{
			for (int index = 0; index < count; index++)
			{
				short valueA = (configValuesA[index] >= 0) ? configValuesA[index] : CharacterCreation.GenerateRandomWeight(random);
				short valueB = (configValuesB[index] >= 0) ? configValuesB[index] : CharacterCreation.GenerateRandomWeight(random);
				resultValues[index] = valueA + valueB;
			}
		}

		// Token: 0x060075E8 RID: 30184 RVA: 0x0044DAC0 File Offset: 0x0044BCC0
		private unsafe static int GetParentValues(IRandomSource random, short* motherValues, short* fatherValues, short* resultValues, int count)
		{
			int sum = 0;
			int useMaxCount = count / 2;
			for (int index = 0; index < count; index++)
			{
				short motherVal = motherValues[index];
				short fatherVal = fatherValues[index];
				bool flag = useMaxCount > 0 && random.NextBool();
				if (flag)
				{
					resultValues[index] = Math.Max(motherVal, fatherVal);
					sum += (int)resultValues[index];
					useMaxCount--;
				}
				else
				{
					resultValues[index] = Math.Min(motherVal, fatherVal);
					sum += (int)resultValues[index];
				}
			}
			return sum;
		}

		// Token: 0x060075E9 RID: 30185 RVA: 0x0044DB54 File Offset: 0x0044BD54
		private unsafe static void DistributeValues(IRandomSource random, short* values, short* weights, int count, int sum)
		{
			int weightSum = CharacterCreation.AdjustWeightsAndGetSum(weights, count);
			for (int i = 0; i < count; i++)
			{
				short weight = weights[i];
				int value = sum * (int)weight / weightSum;
				values[i] = (short)CharacterCreation.NormalDistribute(random, value, value * 20 / 100);
			}
		}

		// Token: 0x060075EA RID: 30186 RVA: 0x0044DBA4 File Offset: 0x0044BDA4
		private unsafe static int AdjustWeightsAndGetSum(short* weight, int count)
		{
			int sum = 1;
			for (int i = 0; i < count; i++)
			{
				weight[i] = (short)Math.Min((int)(weight[i] * 10 + 1), 10000);
				sum += (int)weight[i];
			}
			short minWeight = (short)(sum / 100);
			sum = 0;
			for (int j = 0; j < count; j++)
			{
				weight[j] = Math.Max(weight[j], minWeight);
				sum += (int)weight[j];
			}
			return sum;
		}

		// Token: 0x060075EB RID: 30187 RVA: 0x0044DC38 File Offset: 0x0044BE38
		private static short GenerateRandomWeight(IRandomSource random)
		{
			return (short)RedzenHelper.SkewDistribute(random, 6f, 1.5f, -2f, 2, 12);
		}

		// Token: 0x060075EC RID: 30188 RVA: 0x0044DC64 File Offset: 0x0044BE64
		private unsafe static void PerformMutation(IRandomSource random, short* values, int count, int mutateCount)
		{
			short maxVal = values[CollectionUtils.GetMaxIndex(values, count)];
			short minVal = values[CollectionUtils.GetMinIndex(values, count)];
			sbyte* normalIndices = stackalloc sbyte[(UIntPtr)(count - 2)];
			sbyte* extremeIndices = stackalloc sbyte[(UIntPtr)count];
			int extremeCount = 0;
			int validCount = 0;
			sbyte i = 0;
			while ((int)i < count)
			{
				short value = values[i];
				bool flag = value == maxVal || value == minVal;
				if (flag)
				{
					extremeIndices[extremeCount] = i;
					extremeCount++;
				}
				else
				{
					normalIndices[validCount] = i;
					validCount++;
				}
				i += 1;
			}
			bool flag2 = validCount == 0;
			if (!flag2)
			{
				for (int j = 0; j < mutateCount; j++)
				{
					int extremeIndexOffset = random.Next(extremeCount);
					int normalIndexOffset = random.Next(validCount);
					sbyte extremeIndex = extremeIndices[extremeIndexOffset];
					sbyte normalIndex = normalIndices[normalIndexOffset];
					ref short ptr = ref values[extremeIndex];
					ref short ptr2 = ref values[normalIndex];
					short num = values[normalIndex];
					short num2 = values[extremeIndex];
					ptr = num;
					ptr2 = num2;
					extremeIndices[extremeIndexOffset] = normalIndex;
					normalIndices[normalIndexOffset] = extremeIndex;
				}
			}
		}

		// Token: 0x060075ED RID: 30189 RVA: 0x0044DD77 File Offset: 0x0044BF77
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int DecodeMutateCount(IRandomSource random, int maxMutateCount, int percentProb = 20)
		{
			return (maxMutateCount > 0 && random.CheckPercentProb(percentProb)) ? (1 + random.Next(maxMutateCount)) : 0;
		}

		// Token: 0x060075EE RID: 30190 RVA: 0x0044DD94 File Offset: 0x0044BF94
		public static void CreateFeatures(IRandomSource random, Character character, ref IntelligentCharacterCreationInfo creationInfo)
		{
			FeatureCreationContext context = new FeatureCreationContext(character, ref creationInfo);
			CharacterCreation.CreateFeatures(random, ref context);
		}

		// Token: 0x060075EF RID: 30191 RVA: 0x0044DDB4 File Offset: 0x0044BFB4
		public static void CreateFeatures(IRandomSource random, Character character)
		{
			FeatureCreationContext context = new FeatureCreationContext(character);
			CharacterCreation.CreateFeatures(random, ref context);
		}

		// Token: 0x060075F0 RID: 30192 RVA: 0x0044DDD4 File Offset: 0x0044BFD4
		public static void CreateFeatures(IRandomSource random, ref FeatureCreationContext creationContext)
		{
			Dictionary<short, short> featureGroup2Id = new Dictionary<short, short>(16);
			foreach (short featureId in creationContext.FeatureIds)
			{
				CharacterCreation.AddFeature(featureGroup2Id, featureId);
			}
			creationContext.FeatureIds.Clear();
			CharacterCreation.GenerateFixedFeatures(ref creationContext, featureGroup2Id);
			bool flag = creationContext.DestinyType >= 0;
			if (flag)
			{
				CharacterCreation.AddFeature(featureGroup2Id, DestinyType.Instance[creationContext.DestinyType].Feature);
			}
			bool randomFeaturesAtCreating = creationContext.RandomFeaturesAtCreating;
			if (randomFeaturesAtCreating)
			{
				CharacterCreation.GenerateGeneticFeatures(ref creationContext, random, featureGroup2Id);
				CharacterCreation.AddFeature(featureGroup2Id, CharacterDomain.GetBirthdayFeatureId(creationContext.BirthMonth));
				bool flag2 = creationContext.CurrAge >= 1 && random.CheckPercentProb(50);
				if (flag2)
				{
					CharacterCreation.AddFeature(featureGroup2Id, CharacterDomain.GenerateOneYearOldCatchFeature(random));
				}
				CharacterCreation.GenerateRandomBasicFeatures(ref creationContext, featureGroup2Id, random);
			}
			CharacterCreation.ApplyFeatureIds(ref creationContext, featureGroup2Id);
		}

		// Token: 0x060075F1 RID: 30193 RVA: 0x0044DED4 File Offset: 0x0044C0D4
		private static void ApplyFeatureIds(ref FeatureCreationContext context, Dictionary<short, short> featureGroup2Id)
		{
			List<short> featureIds = context.FeatureIds;
			List<short> potentialFeatureIds = context.PotentialFeatureIds;
			short potentialFeaturesAge = context.PotentialFeaturesAge;
			CharacterCreation.ApplyFeatureGroup(featureGroup2Id, featureIds, 216);
			CharacterCreation.ApplyFeatureGroup(featureGroup2Id, featureIds, 195);
			CharacterCreation.ApplyFeatureGroup(featureGroup2Id, featureIds, 183);
			CharacterCreation.ApplyFeatureGroup(featureGroup2Id, featureIds, 171);
			foreach (KeyValuePair<short, short> entry in featureGroup2Id)
			{
				short featureId = entry.Value;
				bool flag = potentialFeaturesAge >= 0 && CharacterFeature.Instance[featureId].Mergeable;
				if (flag)
				{
					potentialFeatureIds.Add(featureId);
				}
				else
				{
					featureIds.Add(featureId);
				}
			}
			bool flag2 = potentialFeaturesAge >= 0;
			if (flag2)
			{
				int affectedFeaturesCount = potentialFeatureIds.Count * (int)potentialFeaturesAge / 16;
				for (int i = 0; i < affectedFeaturesCount; i++)
				{
					featureIds.Add(potentialFeatureIds[i]);
				}
			}
			featureIds.Sort(CharacterFeatureHelper.FeatureComparer);
		}

		// Token: 0x060075F2 RID: 30194 RVA: 0x0044DFF4 File Offset: 0x0044C1F4
		private static void ApplyFeatureGroup(Dictionary<short, short> featureGroup2Id, List<short> featureIds, short groupId)
		{
			short featureId;
			bool flag = featureGroup2Id.TryGetValue(groupId, out featureId);
			if (flag)
			{
				featureIds.Add(featureId);
				featureGroup2Id.Remove(groupId);
			}
		}

		// Token: 0x060075F3 RID: 30195 RVA: 0x0044E024 File Offset: 0x0044C224
		private static void GenerateFixedFeatures(ref FeatureCreationContext context, Dictionary<short, short> featureGroup2Id)
		{
			short virginityFeatureId = 195;
			short xiangshuStateFeatureId = 216;
			List<short> featureIds = context.FeatureIds;
			int featureIdsCount = featureIds.Count;
			for (int i = 0; i < featureIdsCount; i++)
			{
				short featureId = featureIds[i];
				short groupId = CharacterFeature.Instance[featureId].MutexGroupId;
				bool flag = groupId == 195;
				if (flag)
				{
					virginityFeatureId = featureId;
				}
				else
				{
					bool flag2 = groupId == 216;
					if (flag2)
					{
						xiangshuStateFeatureId = featureId;
					}
				}
			}
			CharacterCreation.AddFeature(featureGroup2Id, virginityFeatureId);
			CharacterCreation.AddFeature(featureGroup2Id, xiangshuStateFeatureId);
			for (int j = 0; j < featureIdsCount; j++)
			{
				CharacterCreation.AddFeature(featureGroup2Id, featureIds[j]);
			}
		}

		// Token: 0x060075F4 RID: 30196 RVA: 0x0044E0DC File Offset: 0x0044C2DC
		private static void GenerateGeneticFeatures(ref FeatureCreationContext context, IRandomSource random, Dictionary<short, short> featureGroup2Id)
		{
			Character mother = context.Mother;
			Character father = context.Father;
			DeadCharacter deadFather = context.DeadFather;
			PregnantState pregnantState = context.PregnantState;
			bool flag = mother == null && father == null && deadFather == null;
			if (!flag)
			{
				List<short> motherFeatureIds = null;
				List<short> fatherFeatureIds = null;
				bool flag2 = pregnantState != null;
				if (flag2)
				{
					motherFeatureIds = pregnantState.MotherFeatureIds;
					fatherFeatureIds = pregnantState.FatherFeatureIds;
				}
				else
				{
					bool flag3 = mother != null;
					if (flag3)
					{
						motherFeatureIds = mother.GetFeatureIds();
					}
					bool flag4 = father != null;
					if (flag4)
					{
						fatherFeatureIds = father.GetFeatureIds();
					}
					else
					{
						bool flag5 = deadFather != null;
						if (flag5)
						{
							fatherFeatureIds = deadFather.FeatureIds;
						}
					}
				}
				Dictionary<short, ValueTuple<short, short>> mergeableFeaturePairs = new Dictionary<short, ValueTuple<short, short>>();
				bool flag6 = fatherFeatureIds != null;
				if (flag6)
				{
					int fatherFeatureIdsCount = fatherFeatureIds.Count;
					for (int i = 0; i < fatherFeatureIdsCount; i++)
					{
						short featureId = fatherFeatureIds[i];
						CharacterFeatureItem template = CharacterFeature.Instance[featureId];
						short groupId = template.MutexGroupId;
						sbyte geneticProb = template.GeneticProb;
						bool mergeable = template.Mergeable;
						if (mergeable)
						{
							mergeableFeaturePairs[groupId] = new ValueTuple<short, short>(-1, featureId);
						}
						else
						{
							bool flag7 = geneticProb > 0;
							if (flag7)
							{
								bool flag8 = !featureGroup2Id.ContainsKey(groupId) && random.CheckPercentProb((int)geneticProb);
								if (flag8)
								{
									featureGroup2Id.Add(groupId, featureId);
								}
							}
						}
					}
				}
				bool flag9 = motherFeatureIds != null;
				if (flag9)
				{
					int motherFeatureIdsCount = motherFeatureIds.Count;
					for (int j = 0; j < motherFeatureIdsCount; j++)
					{
						short featureId2 = motherFeatureIds[j];
						CharacterFeatureItem template2 = CharacterFeature.Instance[featureId2];
						short groupId2 = template2.MutexGroupId;
						sbyte geneticProb2 = template2.GeneticProb;
						bool mergeable2 = template2.Mergeable;
						if (mergeable2)
						{
							ValueTuple<short, short> pair;
							bool flag10 = mergeableFeaturePairs.TryGetValue(groupId2, out pair);
							if (flag10)
							{
								mergeableFeaturePairs[groupId2] = new ValueTuple<short, short>(featureId2, pair.Item2);
							}
						}
						else
						{
							bool flag11 = geneticProb2 > 0;
							if (flag11)
							{
								bool flag12 = !featureGroup2Id.ContainsKey(groupId2) && random.CheckPercentProb((int)geneticProb2);
								if (flag12)
								{
									featureGroup2Id.Add(groupId2, featureId2);
								}
							}
						}
					}
				}
				foreach (KeyValuePair<short, ValueTuple<short, short>> entry in mergeableFeaturePairs)
				{
					short groupId3 = entry.Key;
					ValueTuple<short, short> value = entry.Value;
					short motherFeatureId = value.Item1;
					short fatherFeatureId = value.Item2;
					bool flag13 = motherFeatureId < 0 || fatherFeatureId < 0;
					if (!flag13)
					{
						bool flag14 = featureGroup2Id.ContainsKey(groupId3);
						if (!flag14)
						{
							sbyte motherLevel = CharacterFeature.Instance[motherFeatureId].Level;
							sbyte fatherLevel = CharacterFeature.Instance[fatherFeatureId].Level;
							sbyte mergedLevel = (motherLevel + fatherLevel) / 2;
							bool flag15 = motherLevel > 0 && fatherLevel > 0;
							if (flag15)
							{
								int upgradeProb = (int)((3 - mergedLevel) * 40);
								bool flag16 = random.CheckPercentProb(upgradeProb);
								if (flag16)
								{
									mergedLevel += 1;
								}
							}
							else
							{
								bool flag17 = motherLevel < 0 && fatherLevel < 0;
								if (flag17)
								{
									int downgradeProb = (int)((3 + mergedLevel) * 40);
									bool flag18 = random.CheckPercentProb(downgradeProb);
									if (flag18)
									{
										mergedLevel -= 1;
									}
								}
							}
							bool flag19 = mergedLevel == 0;
							if (!flag19)
							{
								short mergedFeatureId = CharacterDomain.GetMergeableFeatureIdByLevel(groupId3, mergedLevel);
								featureGroup2Id.Add(groupId3, mergedFeatureId);
							}
						}
					}
				}
			}
		}

		// Token: 0x060075F5 RID: 30197 RVA: 0x0044E464 File Offset: 0x0044C664
		private static void GenerateRandomBasicFeatures(ref FeatureCreationContext context, Dictionary<short, short> featureGroup2Id, IRandomSource random)
		{
			int basicFeaturesCount = context.AllGoodBasicFeature ? 5 : CharacterCreation.GenerateRandomBasicFeaturesCount(random);
			foreach (KeyValuePair<short, short> entry in featureGroup2Id)
			{
				short featureId = entry.Value;
				bool basic = CharacterFeature.Instance[featureId].Basic;
				if (basic)
				{
					basicFeaturesCount--;
				}
			}
			bool flag = basicFeaturesCount <= 0;
			if (!flag)
			{
				int goodFeaturesPotential = random.Next(101);
				sbyte gender = context.Gender;
				int i = 0;
				while (i < basicFeaturesCount)
				{
					bool flag2 = context.AllGoodBasicFeature || random.CheckPercentProb(goodFeaturesPotential);
					if (flag2)
					{
						ValueTuple<short, short> randomBasicFeature = CharacterDomain.GetRandomBasicFeature(random, context.IsProtagonist, context.Gender, true, featureGroup2Id);
						short groupId = randomBasicFeature.Item1;
						short featureId2 = randomBasicFeature.Item2;
						bool flag3 = featureId2 < 0;
						if (!flag3)
						{
							featureGroup2Id.Add(groupId, featureId2);
							goodFeaturesPotential -= 20;
						}
					}
					else
					{
						ValueTuple<short, short> randomBasicFeature2 = CharacterDomain.GetRandomBasicFeature(random, context.IsProtagonist, context.Gender, false, featureGroup2Id);
						short groupId2 = randomBasicFeature2.Item1;
						short featureId3 = randomBasicFeature2.Item2;
						bool flag4 = featureId3 < 0;
						if (!flag4)
						{
							featureGroup2Id.Add(groupId2, featureId3);
							goodFeaturesPotential += 20;
						}
					}
					IL_130:
					i++;
					continue;
					goto IL_130;
				}
			}
		}

		// Token: 0x060075F6 RID: 30198 RVA: 0x0044E5C8 File Offset: 0x0044C7C8
		public static int GenerateRandomBasicFeaturesCount(IRandomSource random)
		{
			return RedzenHelper.SkewDistribute(random, 4f, 0.333f, 3f, 3, 7);
		}

		// Token: 0x060075F7 RID: 30199 RVA: 0x0044E5F4 File Offset: 0x0044C7F4
		private static void AddFeature(Dictionary<short, short> featureGroup2Id, short featureId)
		{
			short groupId = CharacterFeature.Instance[featureId].MutexGroupId;
			featureGroup2Id.TryAdd(groupId, featureId);
		}

		// Token: 0x04001FF8 RID: 8184
		private const int BaseMainAttributeSum = 168;

		// Token: 0x04001FF9 RID: 8185
		private const int MainAttributePerLevel = 21;

		// Token: 0x04001FFA RID: 8186
		private const int BaseLifeSkillSum = 448;

		// Token: 0x04001FFB RID: 8187
		private const int LifeSkillPerLevel = 56;

		// Token: 0x04001FFC RID: 8188
		private const int BaseCombatSkillSum = 392;

		// Token: 0x04001FFD RID: 8189
		private const int CombatSkillPerLevel = 49;

		// Token: 0x04001FFE RID: 8190
		private const int MutationRate = 20;

		// Token: 0x04001FFF RID: 8191
		private const int ValueSpan = 20;

		// Token: 0x04002000 RID: 8192
		private static readonly sbyte[] SectMemberGenerationTypeWeights = GlobalConfig.CharacterCreationSectMemberGenerationTypeWeights;

		// Token: 0x04002001 RID: 8193
		private static readonly sbyte[] CivilianGenerationTypeWeights = GlobalConfig.CharacterCreationCivilianGenerationTypeWeights;

		// Token: 0x04002002 RID: 8194
		private static readonly sbyte[] AdjustGradeBaseChances = GlobalConfig.CharacterCreationAdjustGradeBaseChances;

		// Token: 0x04002003 RID: 8195
		private static readonly short[][] AdjustGradeWeights = GlobalConfig.CharacterCreationAdjustGradeWeights;

		// Token: 0x04002004 RID: 8196
		private const sbyte UseCurrOrg = 0;

		// Token: 0x04002005 RID: 8197
		private const sbyte UseIdealSect = 1;

		// Token: 0x04002006 RID: 8198
		private const sbyte MergeBoth = 2;

		// Token: 0x04002007 RID: 8199
		public const int FeaturesExpectedMaxCount = 16;
	}
}
