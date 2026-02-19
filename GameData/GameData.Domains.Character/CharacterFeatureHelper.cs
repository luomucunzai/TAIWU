using System;
using System.Collections.Generic;
using System.Linq;
using Config;

namespace GameData.Domains.Character;

public static class CharacterFeatureHelper
{
	public static IComparer<short> FeatureComparer = Comparer<short>.Create(CompareFeature);

	public static bool IsGood(this CharacterFeatureItem config)
	{
		return config.Level > 0;
	}

	public static bool IsBad(this CharacterFeatureItem config)
	{
		return config.Level < 0;
	}

	public static bool IsNeutral(this CharacterFeatureItem config)
	{
		return config.Level == 0;
	}

	public static bool IsNormal(this CharacterFeatureItem config)
	{
		ECharacterFeatureType type = config.Type;
		if ((uint)(type - 1) <= 1u)
		{
			return true;
		}
		return false;
	}

	public static bool IsLowest(this CharacterFeatureItem config)
	{
		return Math.Abs(config.Level) == 1;
	}

	public static bool IsHighest(this CharacterFeatureItem config)
	{
		return Math.Abs(config.Level) == 3;
	}

	public static bool IsAllowedForOrganization(this CharacterFeatureItem config, sbyte orgTemplateId)
	{
		return config.RequiredOrganization < 0 || config.RequiredOrganization == orgTemplateId;
	}

	public static CharacterFeatureItem Upgrade(this CharacterFeatureItem config)
	{
		if (config.IsNeutral() || config.IsHighest())
		{
			return null;
		}
		int num = (config.IsGood() ? 1 : (-1));
		foreach (CharacterFeatureItem item in CharacterFeature.Instance.Where((CharacterFeatureItem x) => x != null))
		{
			if (item.MutexGroupId == config.MutexGroupId && item.Level == config.Level + num)
			{
				return item;
			}
		}
		return null;
	}

	public static CharacterFeatureItem Degrade(this CharacterFeatureItem config)
	{
		if (config.IsNeutral() || config.IsLowest())
		{
			return null;
		}
		int num = ((!config.IsGood()) ? 1 : (-1));
		foreach (CharacterFeatureItem item in CharacterFeature.Instance.Where((CharacterFeatureItem x) => x != null))
		{
			if (item.MutexGroupId == config.MutexGroupId && item.Level == config.Level + num)
			{
				return item;
			}
		}
		return null;
	}

	public static int CompareFeature(short featureIdA, short featureIdB)
	{
		return CharacterFeature.Instance[featureIdA].DisplayPriority.CompareTo(CharacterFeature.Instance[featureIdB].DisplayPriority);
	}
}
