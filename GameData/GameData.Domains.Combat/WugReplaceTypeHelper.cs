using Config;
using GameData.Domains.Item;

namespace GameData.Domains.Combat;

public static class WugReplaceTypeHelper
{
	public static bool IsMatchWug(this EWugReplaceType replaceType, short wugTemplateId, short targetWugTemplateId = -1)
	{
		switch (replaceType)
		{
		case EWugReplaceType.All:
			return true;
		case EWugReplaceType.None:
			return false;
		default:
		{
			if ((replaceType & EWugReplaceType.Nonexistent) != EWugReplaceType.None && wugTemplateId == targetWugTemplateId)
			{
				return false;
			}
			sbyte wugGrowthType = Config.Medicine.Instance[wugTemplateId].WugGrowthType;
			if ((replaceType & EWugReplaceType.CombatOnly) != EWugReplaceType.None && !WugGrowthType.IsWugGrowthTypeCombatOnly(wugGrowthType))
			{
				return false;
			}
			if ((replaceType & EWugReplaceType.Ungrown) != EWugReplaceType.None && wugGrowthType == 4)
			{
				return false;
			}
			return true;
		}
		}
	}
}
