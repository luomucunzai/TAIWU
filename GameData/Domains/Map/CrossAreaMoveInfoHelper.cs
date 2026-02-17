using System;
using GameData.Domains.Character;

namespace GameData.Domains.Map
{
	// Token: 0x020008B0 RID: 2224
	public static class CrossAreaMoveInfoHelper
	{
		// Token: 0x060078AF RID: 30895 RVA: 0x0046659C File Offset: 0x0046479C
		public static void AutoCheckFreeTravel(this CrossAreaMoveInfo crossAreaMoveInfo)
		{
			Character taiwuChar = DomainManager.Taiwu.GetTaiwu();
			bool flag = taiwuChar.GetResource(6) >= crossAreaMoveInfo.MoneyCost;
			if (!flag)
			{
				for (int i = 0; i < crossAreaMoveInfo.Route.CostList.Count; i++)
				{
					crossAreaMoveInfo.Route.CostList[i] = (short)((int)crossAreaMoveInfo.Route.CostList[i] * SharedConstValue.FreeTravelCostTimeRate);
				}
				crossAreaMoveInfo.MoneyCost = 0;
			}
		}
	}
}
