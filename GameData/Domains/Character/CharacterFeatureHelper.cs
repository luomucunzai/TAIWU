using System;
using System.Collections.Generic;
using System.Linq;
using Config;

namespace GameData.Domains.Character
{
	// Token: 0x02000809 RID: 2057
	public static class CharacterFeatureHelper
	{
		// Token: 0x06007451 RID: 29777 RVA: 0x00442BF3 File Offset: 0x00440DF3
		public static bool IsGood(this CharacterFeatureItem config)
		{
			return config.Level > 0;
		}

		// Token: 0x06007452 RID: 29778 RVA: 0x00442BFE File Offset: 0x00440DFE
		public static bool IsBad(this CharacterFeatureItem config)
		{
			return config.Level < 0;
		}

		// Token: 0x06007453 RID: 29779 RVA: 0x00442C09 File Offset: 0x00440E09
		public static bool IsNeutral(this CharacterFeatureItem config)
		{
			return config.Level == 0;
		}

		// Token: 0x06007454 RID: 29780 RVA: 0x00442C14 File Offset: 0x00440E14
		public static bool IsNormal(this CharacterFeatureItem config)
		{
			ECharacterFeatureType type = config.Type;
			return type - ECharacterFeatureType.Good <= 1;
		}

		// Token: 0x06007455 RID: 29781 RVA: 0x00442C37 File Offset: 0x00440E37
		public static bool IsLowest(this CharacterFeatureItem config)
		{
			return Math.Abs(config.Level) == 1;
		}

		// Token: 0x06007456 RID: 29782 RVA: 0x00442C47 File Offset: 0x00440E47
		public static bool IsHighest(this CharacterFeatureItem config)
		{
			return Math.Abs(config.Level) == 3;
		}

		// Token: 0x06007457 RID: 29783 RVA: 0x00442C57 File Offset: 0x00440E57
		public static bool IsAllowedForOrganization(this CharacterFeatureItem config, sbyte orgTemplateId)
		{
			return config.RequiredOrganization < 0 || config.RequiredOrganization == orgTemplateId;
		}

		// Token: 0x06007458 RID: 29784 RVA: 0x00442C70 File Offset: 0x00440E70
		public static CharacterFeatureItem Upgrade(this CharacterFeatureItem config)
		{
			bool flag = config.IsNeutral() || config.IsHighest();
			CharacterFeatureItem result;
			if (flag)
			{
				result = null;
			}
			else
			{
				int delta = config.IsGood() ? 1 : -1;
				foreach (CharacterFeatureItem featureConfig in from x in CharacterFeature.Instance
				where x != null
				select x)
				{
					bool flag2 = featureConfig.MutexGroupId == config.MutexGroupId && (int)featureConfig.Level == (int)config.Level + delta;
					if (flag2)
					{
						return featureConfig;
					}
				}
				result = null;
			}
			return result;
		}

		// Token: 0x06007459 RID: 29785 RVA: 0x00442D3C File Offset: 0x00440F3C
		public static CharacterFeatureItem Degrade(this CharacterFeatureItem config)
		{
			bool flag = config.IsNeutral() || config.IsLowest();
			CharacterFeatureItem result;
			if (flag)
			{
				result = null;
			}
			else
			{
				int delta = config.IsGood() ? -1 : 1;
				foreach (CharacterFeatureItem featureConfig in from x in CharacterFeature.Instance
				where x != null
				select x)
				{
					bool flag2 = featureConfig.MutexGroupId == config.MutexGroupId && (int)featureConfig.Level == (int)config.Level + delta;
					if (flag2)
					{
						return featureConfig;
					}
				}
				result = null;
			}
			return result;
		}

		// Token: 0x0600745A RID: 29786 RVA: 0x00442E08 File Offset: 0x00441008
		public static int CompareFeature(short featureIdA, short featureIdB)
		{
			return CharacterFeature.Instance[featureIdA].DisplayPriority.CompareTo(CharacterFeature.Instance[featureIdB].DisplayPriority);
		}

		// Token: 0x04001EC0 RID: 7872
		public static IComparer<short> FeatureComparer = Comparer<short>.Create(new Comparison<short>(CharacterFeatureHelper.CompareFeature));
	}
}
