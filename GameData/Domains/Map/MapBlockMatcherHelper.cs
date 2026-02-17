using System;
using Config;

namespace GameData.Domains.Map
{
	// Token: 0x020008B6 RID: 2230
	public static class MapBlockMatcherHelper
	{
		// Token: 0x060078BD RID: 30909 RVA: 0x00466CB4 File Offset: 0x00464EB4
		public static bool Match(this MapBlockMatcherItem matcherItem, MapBlockData mapBlockData)
		{
			bool flag = !matcherItem.MatchType(mapBlockData.BlockType);
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				bool flag2 = !matcherItem.MatchSubType(mapBlockData.BlockSubType);
				if (flag2)
				{
					result = false;
				}
				else
				{
					bool flag3 = matcherItem.ExcludeBlocksWithAdventure && DomainManager.Adventure.GetAdventureSite(mapBlockData.AreaId, mapBlockData.BlockId) != null;
					if (flag3)
					{
						result = false;
					}
					else
					{
						bool flag4 = matcherItem.ExcludeBlocksWithEffect && MapBlockMatcherHelper.MatchBlockWithEffect(mapBlockData);
						result = !flag4;
					}
				}
			}
			return result;
		}

		// Token: 0x060078BE RID: 30910 RVA: 0x00466D3C File Offset: 0x00464F3C
		private static bool MatchBlockWithEffect(MapBlockData mapBlockData)
		{
			Location location = mapBlockData.GetLocation();
			bool flag = DomainManager.Extra.GetSectEmeiBloodLocations().Contains(location);
			bool result;
			if (flag)
			{
				result = true;
			}
			else
			{
				bool flag2 = DomainManager.Extra.GetSectXuehouBloodLightLocations().Contains(location);
				if (flag2)
				{
					result = true;
				}
				else
				{
					bool flag3 = DomainManager.Map.IsLocationInFulongFlameArea(location);
					result = flag3;
				}
			}
			return result;
		}

		// Token: 0x060078BF RID: 30911 RVA: 0x00466D9C File Offset: 0x00464F9C
		private static bool MatchType(this MapBlockMatcherItem matcherItem, EMapBlockType blockType)
		{
			EMapBlockType[] array = matcherItem.ExcludeTypes;
			bool flag = array != null && array.Length > 0;
			if (flag)
			{
				for (int i = 0; i < matcherItem.ExcludeTypes.Length; i++)
				{
					bool flag2 = matcherItem.ExcludeTypes[i] == blockType;
					if (flag2)
					{
						return false;
					}
				}
			}
			array = matcherItem.IncludeTypes;
			bool flag3 = array != null && array.Length > 0;
			bool result;
			if (flag3)
			{
				for (int j = 0; j < matcherItem.IncludeTypes.Length; j++)
				{
					bool flag4 = matcherItem.IncludeTypes[j] == blockType;
					if (flag4)
					{
						return true;
					}
				}
				result = false;
			}
			else
			{
				result = true;
			}
			return result;
		}

		// Token: 0x060078C0 RID: 30912 RVA: 0x00466E48 File Offset: 0x00465048
		private static bool MatchSubType(this MapBlockMatcherItem matcherItem, EMapBlockSubType blockSubType)
		{
			EMapBlockSubType[] array = matcherItem.ExcludeSubTypes;
			bool flag = array != null && array.Length > 0;
			if (flag)
			{
				for (int i = 0; i < matcherItem.ExcludeSubTypes.Length; i++)
				{
					bool flag2 = matcherItem.ExcludeSubTypes[i] == blockSubType;
					if (flag2)
					{
						return false;
					}
				}
			}
			array = matcherItem.IncludeSubTypes;
			bool flag3 = array != null && array.Length > 0;
			bool result;
			if (flag3)
			{
				for (int j = 0; j < matcherItem.IncludeSubTypes.Length; j++)
				{
					bool flag4 = matcherItem.IncludeSubTypes[j] == blockSubType;
					if (flag4)
					{
						return true;
					}
				}
				result = false;
			}
			else
			{
				result = true;
			}
			return result;
		}
	}
}
