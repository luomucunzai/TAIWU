using System;
using System.Collections.Generic;

namespace GameData.Domains.Item
{
	// Token: 0x02000666 RID: 1638
	public static class CricketCombatPlanExtensions
	{
		// Token: 0x06004F89 RID: 20361 RVA: 0x002B4F50 File Offset: 0x002B3150
		public static bool ReplaceCricket(this List<CricketCombatPlan> plans, int srcCricketId, ItemKey dstCricket)
		{
			bool flag = plans == null || plans.Count <= 0;
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				bool anyChanged = false;
				foreach (CricketCombatPlan plan in plans)
				{
					anyChanged = (plan.ReplaceCricket(srcCricketId, dstCricket) || anyChanged);
				}
				result = anyChanged;
			}
			return result;
		}

		// Token: 0x06004F8A RID: 20362 RVA: 0x002B4FC8 File Offset: 0x002B31C8
		public static bool ReplaceCricket(this CricketCombatPlan plan, int srcCricketId, ItemKey dstCricket)
		{
			List<ItemKey> list = (plan != null) ? plan.Crickets : null;
			bool flag = list == null || list.Count <= 0;
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				bool anyChanged = false;
				for (int i = 0; i < plan.Crickets.Count; i++)
				{
					bool flag2 = plan.Crickets[i].Id != srcCricketId;
					if (!flag2)
					{
						plan.Crickets[i] = dstCricket;
						anyChanged = true;
					}
				}
				result = anyChanged;
			}
			return result;
		}
	}
}
