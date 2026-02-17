using System;
using GameData.Domains.Character;

namespace GameData.Domains.Taiwu
{
	// Token: 0x02000043 RID: 67
	[Obsolete]
	public static class TaiwuVillageStorageHelper
	{
		// Token: 0x06001363 RID: 4963 RVA: 0x0013832C File Offset: 0x0013652C
		public static int GetTotalWeight(this TaiwuVillageStorage storage)
		{
			int totalWeight = 0;
			for (int i = 0; i < storage.Inventories.Length; i++)
			{
				totalWeight += storage.Inventories[i].GetTotalWeight();
			}
			return totalWeight;
		}
	}
}
