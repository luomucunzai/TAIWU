using System.Collections.Generic;

namespace GameData.Domains.Item;

public static class CricketCombatPlanExtensions
{
	public static bool ReplaceCricket(this List<CricketCombatPlan> plans, int srcCricketId, ItemKey dstCricket)
	{
		if (plans == null || plans.Count <= 0)
		{
			return false;
		}
		bool flag = false;
		foreach (CricketCombatPlan plan in plans)
		{
			flag = plan.ReplaceCricket(srcCricketId, dstCricket) || flag;
		}
		return flag;
	}

	public static bool ReplaceCricket(this CricketCombatPlan plan, int srcCricketId, ItemKey dstCricket)
	{
		List<ItemKey> list = plan?.Crickets;
		if (list == null || list.Count <= 0)
		{
			return false;
		}
		bool result = false;
		for (int i = 0; i < plan.Crickets.Count; i++)
		{
			if (plan.Crickets[i].Id == srcCricketId)
			{
				plan.Crickets[i] = dstCricket;
				result = true;
			}
		}
		return result;
	}
}
