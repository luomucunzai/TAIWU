using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Config;
using GameData.Utilities;

namespace GameData.Domains.Character;

public static class SharedMethods
{
	public static int CalcQualificationGrade(short qualification)
	{
		if (qualification >= 100)
		{
			return 8;
		}
		if (qualification >= 90)
		{
			return 7;
		}
		if (qualification >= 80)
		{
			return 6;
		}
		if (qualification >= 70)
		{
			return 5;
		}
		if (qualification >= 60)
		{
			return 4;
		}
		if (qualification >= 50)
		{
			return 3;
		}
		if (qualification >= 40)
		{
			return 2;
		}
		if (qualification >= 30)
		{
			return 1;
		}
		return 0;
	}

	public static int GetSectFeatureFameBonus(short featureId, bool isTaiwu, OrganizationInfo charcterOrgInfo)
	{
		int num = 0;
		OrganizationItem organizationItem = Config.Organization.Instance.FirstOrDefault((OrganizationItem o) => o.MemberFeature == featureId);
		if (organizationItem == null)
		{
			return num;
		}
		CharacterFeatureItem characterFeatureItem = CharacterFeature.Instance[featureId];
		if (isTaiwu)
		{
			return num + characterFeatureItem.TaiwuFameBonu;
		}
		if (organizationItem.TemplateId == charcterOrgInfo.OrgTemplateId)
		{
			return num + characterFeatureItem.SectFameBonus[charcterOrgInfo.Grade];
		}
		return num + characterFeatureItem.NotSectFameBonu;
	}

	public static bool IsAbleToGrowAvatarElement(sbyte growableElementType, byte monkType, short physiologicalAge, sbyte gender, bool transgender, List<short> featureIds)
	{
		return growableElementType switch
		{
			0 => IsAbleToGrowHair(monkType), 
			1 => IsAbleToGrowBeard1(physiologicalAge, gender, transgender, featureIds), 
			2 => IsAbleToGrowBeard2(physiologicalAge, gender, transgender, featureIds), 
			3 => IsAbleToGrowWrinkle1(physiologicalAge), 
			4 => IsAbleToGrowWrinkle2(physiologicalAge), 
			5 => IsAbleToGrowWrinkle3(physiologicalAge), 
			6 => IsAbleToGrowEyebrow(), 
			_ => throw new Exception($"Unsupported AvatarGrowableElementType: {growableElementType}"), 
		};
	}

	public static bool IsAbleToGrowHair(byte monkType)
	{
		return monkType != 130;
	}

	public static (bool beard1, bool beard2) IsAbleToGrowBeards(short physiologicalAge, sbyte gender, bool transgender, List<short> featureIds)
	{
		if (gender != 1 || transgender || featureIds.Contains(168))
		{
			return (beard1: false, beard2: false);
		}
		return (beard1: physiologicalAge >= GlobalConfig.Instance.AgeShowBeard1, beard2: physiologicalAge >= GlobalConfig.Instance.AgeShowBeard2);
	}

	public static bool IsAbleToGrowBeard1(short physiologicalAge, sbyte gender, bool transgender, List<short> featureIds)
	{
		if (gender == 1 && physiologicalAge >= GlobalConfig.Instance.AgeShowBeard1 && !transgender)
		{
			return !featureIds.Contains(168);
		}
		return false;
	}

	public static bool IsAbleToGrowBeard2(short physiologicalAge, sbyte gender, bool transgender, List<short> featureIds)
	{
		if (gender == 1 && physiologicalAge >= GlobalConfig.Instance.AgeShowBeard2 && !transgender)
		{
			return !featureIds.Contains(168);
		}
		return false;
	}

	public static bool IsAbleToGrowWrinkle1(short physiologicalAge)
	{
		return physiologicalAge >= GlobalConfig.Instance.AgeShowWrinkle1;
	}

	public static bool IsAbleToGrowWrinkle2(short physiologicalAge)
	{
		return physiologicalAge >= GlobalConfig.Instance.AgeShowWrinkle2;
	}

	public static bool IsAbleToGrowWrinkle3(short physiologicalAge)
	{
		return physiologicalAge >= GlobalConfig.Instance.AgeShowWrinkle3;
	}

	public static bool IsAbleToGrowEyebrow()
	{
		return true;
	}

	public static sbyte GetInnateFiveElementsType(sbyte birthMonth)
	{
		return Month.Instance[birthMonth].FiveElementsType;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public unsafe static bool HasPoisonImmunity(sbyte poisonType, CharacterItem characterCfg, ref PoisonInts poisonResists, byte poisonImmunities)
	{
		if (!characterCfg.PoisonImmunities[poisonType] && poisonResists.Items[poisonType] < 1000)
		{
			return BitOperation.GetBit(poisonImmunities, (int)poisonType);
		}
		return true;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool HasPoisonImmunity(sbyte poisonType, CharacterItem characterCfg, ref int[] poisonResists, byte poisonImmunities)
	{
		if (!characterCfg.PoisonImmunities[poisonType] && poisonResists[poisonType] < 1000)
		{
			return BitOperation.GetBit(poisonImmunities, (int)poisonType);
		}
		return true;
	}

	public static int CalcFeatureMedalValue(IEnumerable<short> featureIds, sbyte medalType)
	{
		int num = 0;
		int num2 = 0;
		foreach (short featureId in featureIds)
		{
			List<sbyte> values = CharacterFeature.Instance[featureId].FeatureMedals[medalType].Values;
			for (int i = 0; i < values.Count; i++)
			{
				switch (values[i])
				{
				case 0:
					num++;
					break;
				case 1:
					num--;
					break;
				case 2:
					num2++;
					break;
				case 3:
					num2 -= 3;
					break;
				}
			}
		}
		if (num == 0)
		{
			return 0;
		}
		bool flag = num > 0;
		int num3 = num + (flag ? num2 : (-num2));
		if (num3 > 0 != flag)
		{
			return 0;
		}
		if (!flag)
		{
			return Math.Max(num3, -8);
		}
		return Math.Min(num3, 8);
	}

	public static bool IsMedalMatchTeammateCommand(IReadOnlyList<int> medalCounts, sbyte cmdType)
	{
		TeammateCommandItem teammateCommandItem = TeammateCommand.Instance[cmdType];
		if (teammateCommandItem.MedalType < 0)
		{
			return false;
		}
		ETeammateCommandType type = teammateCommandItem.Type;
		if (((uint)type > 1u && type != ETeammateCommandType.GearMate) || 1 == 0)
		{
			return false;
		}
		return medalCounts.GetOrDefault(teammateCommandItem.MedalType) >= teammateCommandItem.MedalCount;
	}
}
