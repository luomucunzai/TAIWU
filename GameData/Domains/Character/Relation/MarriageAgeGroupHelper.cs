using System;

namespace GameData.Domains.Character.Relation
{
	// Token: 0x02000822 RID: 2082
	public static class MarriageAgeGroupHelper
	{
		// Token: 0x06007550 RID: 30032 RVA: 0x00449820 File Offset: 0x00447A20
		public static EMarriageAgeGroup GetMarriageAgeGroup(int age)
		{
			bool flag = MarriageAgeGroupHelper.CheckInRange(age, GlobalConfig.Instance.GoodMarriageAgeRange);
			EMarriageAgeGroup result;
			if (flag)
			{
				result = EMarriageAgeGroup.GoodAge;
			}
			else
			{
				bool flag2 = MarriageAgeGroupHelper.CheckInRange(age, GlobalConfig.Instance.LateMarriageAgeRange);
				if (flag2)
				{
					result = EMarriageAgeGroup.LateAge;
				}
				else
				{
					result = EMarriageAgeGroup.ExceedAge;
				}
			}
			return result;
		}

		// Token: 0x06007551 RID: 30033 RVA: 0x00449864 File Offset: 0x00447A64
		public static int GetDistantMarriageChance(this EMarriageAgeGroup ageGroup)
		{
			if (!true)
			{
			}
			int result;
			if (ageGroup != EMarriageAgeGroup.GoodAge)
			{
				if (ageGroup != EMarriageAgeGroup.LateAge)
				{
					result = 0;
				}
				else
				{
					result = (int)GlobalConfig.Instance.DistantMarriageLateAgeChance;
				}
			}
			else
			{
				result = (int)GlobalConfig.Instance.DistantMarriageGoodAgeChance;
			}
			if (!true)
			{
			}
			return result;
		}

		// Token: 0x06007552 RID: 30034 RVA: 0x004498A8 File Offset: 0x00447AA8
		public static bool CheckMarriageTargetAgeInRange(this EMarriageAgeGroup selfMarriageAgeGroup, sbyte targetGender, int targetAge)
		{
			if (!true)
			{
			}
			bool result;
			if (selfMarriageAgeGroup != EMarriageAgeGroup.GoodAge)
			{
				result = (selfMarriageAgeGroup == EMarriageAgeGroup.LateAge && MarriageAgeGroupHelper.CheckInRange(targetAge, GlobalConfig.Instance.DistantMarriageLateAgeTargetAgeRange[(int)targetGender]));
			}
			else
			{
				result = MarriageAgeGroupHelper.CheckInRange(targetAge, GlobalConfig.Instance.DistantMarriageGoodAgeTargetAgeRange[(int)targetGender]);
			}
			if (!true)
			{
			}
			return result;
		}

		// Token: 0x06007553 RID: 30035 RVA: 0x004498F8 File Offset: 0x00447AF8
		public static bool CheckMarriageTargetGradeInRange(this EMarriageAgeGroup selfMarriageAgeGroup, sbyte selfGrade, sbyte targetGrade)
		{
			if (!true)
			{
			}
			bool result;
			if (selfMarriageAgeGroup != EMarriageAgeGroup.GoodAge)
			{
				result = (selfMarriageAgeGroup == EMarriageAgeGroup.LateAge && MarriageAgeGroupHelper.CheckInRange(targetGrade - selfGrade, GlobalConfig.Instance.DistantMarriageLateAgeTargetGradeRange));
			}
			else
			{
				result = MarriageAgeGroupHelper.CheckInRange(targetGrade - selfGrade, GlobalConfig.Instance.DistantMarriageGoodAgeTargetGradeRange);
			}
			if (!true)
			{
			}
			return result;
		}

		// Token: 0x06007554 RID: 30036 RVA: 0x00449949 File Offset: 0x00447B49
		private static bool CheckInRange(int value, int[] range)
		{
			return value >= range[0] && value <= range[1];
		}

		// Token: 0x06007555 RID: 30037 RVA: 0x0044995D File Offset: 0x00447B5D
		private static bool CheckInRange(sbyte value, sbyte[] range)
		{
			return value >= range[0] && value <= range[1];
		}
	}
}
