using System.Collections.Generic;
using GameData.Domains.Map;
using GameData.Utilities;
using Redzen.Random;

namespace GameData.Domains.Extra;

public static class SectStoryThiefDataHelper
{
	public static void UpdatePlace(this SectStoryThiefData data, IRandomSource random)
	{
		List<int> list = ObjectPool<List<int>>.Instance.Get();
		for (int i = 0; i < data.ThiefBlockIds.Count; i++)
		{
			if (!data.ThiefTriggered[i])
			{
				list.Add(i);
			}
		}
		if (list.Count > 0)
		{
			List<MapBlockData> list2 = ObjectPool<List<MapBlockData>>.Instance.Get();
			List<MapBlockData> list3 = ObjectPool<List<MapBlockData>>.Instance.Get();
			MapBlockData block = DomainManager.Map.GetBlock(data.AreaId, data.ThiefBlockIds[list[0]]);
			ByteCoordinate centerBlockPos = block.GetBlockPos();
			DomainManager.Map.GetRealNeighborBlocks(data.AreaId, block.BlockId, list2, 3);
			list2.RemoveAll(IsBlockUnAvailable);
			list2.Sort((MapBlockData blockA, MapBlockData blockB) => blockA.GetBlockPos().GetManhattanDistance(centerBlockPos).CompareTo(blockB.GetBlockPos().GetManhattanDistance(centerBlockPos)));
			foreach (MapBlockData item in list2)
			{
				DomainManager.Map.GetRealNeighborBlocks(data.AreaId, item.BlockId, list3);
				list3.RemoveAll(IsBlockUnAvailable);
				if (list3.Count < list.Count)
				{
					continue;
				}
				CollectionUtils.Shuffle(random, list3);
				for (int num = 0; num < list.Count; num++)
				{
					data.ThiefBlockIds[list[num]] = list3[num].BlockId;
				}
				break;
			}
			ObjectPool<List<MapBlockData>>.Instance.Return(list2);
			ObjectPool<List<MapBlockData>>.Instance.Return(list3);
		}
		ObjectPool<List<int>>.Instance.Return(list);
	}

	public static bool IsBlockAvailable(MapBlockData blockData)
	{
		if (!blockData.IsPassable() || blockData.IsCityTown())
		{
			return false;
		}
		if (blockData.GetLocation() == DomainManager.Taiwu.GetTaiwu().GetLocation())
		{
			return false;
		}
		if (DomainManager.World.TryGetThief(blockData.GetLocation(), out var _, out var _))
		{
			return false;
		}
		if (DomainManager.Map.IsCricketInLocation(blockData.GetLocation()))
		{
			return false;
		}
		return !DomainManager.Map.IsBlockSpecial(blockData, strictCheck: false);
	}

	public static bool IsBlockUnAvailable(MapBlockData blockData)
	{
		return !IsBlockAvailable(blockData);
	}
}
