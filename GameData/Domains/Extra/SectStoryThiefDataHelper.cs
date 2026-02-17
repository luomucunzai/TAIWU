using System;
using System.Collections.Generic;
using GameData.Domains.Map;
using GameData.Utilities;
using Redzen.Random;

namespace GameData.Domains.Extra
{
	// Token: 0x0200068C RID: 1676
	public static class SectStoryThiefDataHelper
	{
		// Token: 0x06005F5D RID: 24413 RVA: 0x00363D4C File Offset: 0x00361F4C
		public static void UpdatePlace(this SectStoryThiefData data, IRandomSource random)
		{
			List<int> notTriggeredThiefIndexes = ObjectPool<List<int>>.Instance.Get();
			for (int i = 0; i < data.ThiefBlockIds.Count; i++)
			{
				bool flag = !data.ThiefTriggered[i];
				if (flag)
				{
					notTriggeredThiefIndexes.Add(i);
				}
			}
			bool flag2 = notTriggeredThiefIndexes.Count > 0;
			if (flag2)
			{
				List<MapBlockData> neighborBlocks = ObjectPool<List<MapBlockData>>.Instance.Get();
				List<MapBlockData> newCenterNeighborBlocks = ObjectPool<List<MapBlockData>>.Instance.Get();
				MapBlockData centerBlock = DomainManager.Map.GetBlock(data.AreaId, data.ThiefBlockIds[notTriggeredThiefIndexes[0]]);
				ByteCoordinate centerBlockPos = centerBlock.GetBlockPos();
				DomainManager.Map.GetRealNeighborBlocks(data.AreaId, centerBlock.BlockId, neighborBlocks, 3, false);
				neighborBlocks.RemoveAll(new Predicate<MapBlockData>(SectStoryThiefDataHelper.IsBlockUnAvailable));
				neighborBlocks.Sort((MapBlockData blockA, MapBlockData blockB) => blockA.GetBlockPos().GetManhattanDistance(centerBlockPos).CompareTo(blockB.GetBlockPos().GetManhattanDistance(centerBlockPos)));
				foreach (MapBlockData block in neighborBlocks)
				{
					DomainManager.Map.GetRealNeighborBlocks(data.AreaId, block.BlockId, newCenterNeighborBlocks, 1, false);
					newCenterNeighborBlocks.RemoveAll(new Predicate<MapBlockData>(SectStoryThiefDataHelper.IsBlockUnAvailable));
					bool flag3 = newCenterNeighborBlocks.Count < notTriggeredThiefIndexes.Count;
					if (!flag3)
					{
						CollectionUtils.Shuffle<MapBlockData>(random, newCenterNeighborBlocks);
						for (int j = 0; j < notTriggeredThiefIndexes.Count; j++)
						{
							data.ThiefBlockIds[notTriggeredThiefIndexes[j]] = newCenterNeighborBlocks[j].BlockId;
						}
						break;
					}
				}
				ObjectPool<List<MapBlockData>>.Instance.Return(neighborBlocks);
				ObjectPool<List<MapBlockData>>.Instance.Return(newCenterNeighborBlocks);
			}
			ObjectPool<List<int>>.Instance.Return(notTriggeredThiefIndexes);
		}

		// Token: 0x06005F5E RID: 24414 RVA: 0x00363F40 File Offset: 0x00362140
		public static bool IsBlockAvailable(MapBlockData blockData)
		{
			bool flag = !blockData.IsPassable() || blockData.IsCityTown();
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				bool flag2 = blockData.GetLocation() == DomainManager.Taiwu.GetTaiwu().GetLocation();
				if (flag2)
				{
					result = false;
				}
				else
				{
					SectStoryThiefData sectStoryThiefData;
					int num;
					bool flag3 = DomainManager.World.TryGetThief(blockData.GetLocation(), out sectStoryThiefData, out num);
					if (flag3)
					{
						result = false;
					}
					else
					{
						bool flag4 = DomainManager.Map.IsCricketInLocation(blockData.GetLocation());
						result = (!flag4 && !DomainManager.Map.IsBlockSpecial(blockData, false));
					}
				}
			}
			return result;
		}

		// Token: 0x06005F5F RID: 24415 RVA: 0x00363FD2 File Offset: 0x003621D2
		public static bool IsBlockUnAvailable(MapBlockData blockData)
		{
			return !SectStoryThiefDataHelper.IsBlockAvailable(blockData);
		}
	}
}
