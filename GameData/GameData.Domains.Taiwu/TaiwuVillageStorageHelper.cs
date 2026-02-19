using System;
using GameData.Domains.Character;

namespace GameData.Domains.Taiwu;

[Obsolete]
public static class TaiwuVillageStorageHelper
{
	public static int GetTotalWeight(this TaiwuVillageStorage storage)
	{
		int num = 0;
		for (int i = 0; i < storage.Inventories.Length; i++)
		{
			num += storage.Inventories[i].GetTotalWeight();
		}
		return num;
	}
}
