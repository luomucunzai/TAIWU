using System;
using Config;
using GameData.Domains.Item;

namespace GameData.Domains.Combat
{
	// Token: 0x020006B3 RID: 1715
	public static class WugReplaceTypeHelper
	{
		// Token: 0x0600660A RID: 26122 RVA: 0x003AA224 File Offset: 0x003A8424
		public static bool IsMatchWug(this EWugReplaceType replaceType, short wugTemplateId, short targetWugTemplateId = -1)
		{
			bool flag = replaceType == EWugReplaceType.All;
			bool result;
			if (flag)
			{
				result = true;
			}
			else
			{
				bool flag2 = replaceType == EWugReplaceType.None;
				if (flag2)
				{
					result = false;
				}
				else
				{
					bool flag3 = (replaceType & EWugReplaceType.Nonexistent) != EWugReplaceType.None && wugTemplateId == targetWugTemplateId;
					if (flag3)
					{
						result = false;
					}
					else
					{
						sbyte existWugGrowthType = Config.Medicine.Instance[wugTemplateId].WugGrowthType;
						bool flag4 = (replaceType & EWugReplaceType.CombatOnly) != EWugReplaceType.None && !WugGrowthType.IsWugGrowthTypeCombatOnly(existWugGrowthType);
						if (flag4)
						{
							result = false;
						}
						else
						{
							bool flag5 = (replaceType & EWugReplaceType.Ungrown) != EWugReplaceType.None && existWugGrowthType == 4;
							result = !flag5;
						}
					}
				}
			}
			return result;
		}
	}
}
