using System;
using System.Collections.Generic;
using GameData.Utilities;
using Redzen.Random;

namespace GameData.Domains.Map;

public static class CricketPlaceDataHelper
{
	public static bool BlockAllowCricket(MapBlockData block)
	{
		return block.TemplateId != 126 && block.TemplateId != 125 && !DomainManager.Map.IsLocationInFulongFlameArea(block.GetLocation());
	}

	public static void Init(this CricketPlaceData data, short areaId, IRandomSource random, int minGroupCount = 3, int maxGroupCount = 5, int minDistance = -1, int maxDistance = -1)
	{
		Span<MapBlockData> areaBlocks = DomainManager.Map.GetAreaBlocks(areaId);
		int num = random.Next(minGroupCount, maxGroupCount + 1);
		data.CricketBlocks = new short[3 * num];
		data.CricketTriggered = new bool[3 * num];
		data.RealCircketIdx = new byte[num];
		Location location = DomainManager.Taiwu.GetTaiwu().GetLocation();
		ByteCoordinate byteCoordinate = (location.IsValid() ? DomainManager.Map.GetBlock(location).GetBlockPos() : new ByteCoordinate(0, 0));
		List<MapBlockData> list = ObjectPool<List<MapBlockData>>.Instance.Get();
		List<MapBlockData> blockRandomPool = ObjectPool<List<MapBlockData>>.Instance.Get();
		List<MapBlockData> list2 = ObjectPool<List<MapBlockData>>.Instance.Get();
		list.Clear();
		for (int i = 0; i < areaBlocks.Length; i++)
		{
			MapBlockData mapBlockData = areaBlocks[i];
			if (BlockAllowCricket(mapBlockData) && (areaId != location.AreaId || mapBlockData.BlockId != location.BlockId) && (minDistance < 0 || (location.IsValid() && mapBlockData.GetManhattanDistanceToPos(byteCoordinate.X, byteCoordinate.Y) >= minDistance)) && (maxDistance < 0 || (location.IsValid() && mapBlockData.GetManhattanDistanceToPos(byteCoordinate.X, byteCoordinate.Y) <= maxDistance)))
			{
				list.Add(mapBlockData);
			}
		}
		int num2 = 0;
		int num3 = 0;
		while (num2 < minGroupCount && num3 < 50)
		{
			blockRandomPool.Clear();
			blockRandomPool.AddRange(list);
			CollectionUtils.Shuffle(random, blockRandomPool);
			num2 = 0;
			for (int j = 0; j < num; j++)
			{
				for (int k = 0; k < blockRandomPool.Count; k++)
				{
					MapBlockData center = blockRandomPool[k];
					DomainManager.Map.GetRealNeighborBlocks(center.AreaId, center.BlockId, list2);
					list2.RemoveAll((MapBlockData block) => !blockRandomPool.Contains(block));
					if (list2.Count >= 2)
					{
						CollectionUtils.Shuffle(random, list2);
						data.CricketBlocks[3 * j] = center.BlockId;
						data.CricketBlocks[3 * j + 1] = list2[0].BlockId;
						data.CricketBlocks[3 * j + 2] = list2[1].BlockId;
						data.RealCircketIdx[j] = (byte)random.Next(3);
						blockRandomPool.RemoveAll(delegate(MapBlockData block)
						{
							ByteCoordinate byteCoordinate2 = block.GetBlockPos() - center.GetBlockPos();
							return byteCoordinate2.X <= 3 && byteCoordinate2.Y <= 3;
						});
						num2++;
						break;
					}
				}
			}
			num3++;
		}
		ObjectPool<List<MapBlockData>>.Instance.Return(list);
		ObjectPool<List<MapBlockData>>.Instance.Return(blockRandomPool);
		ObjectPool<List<MapBlockData>>.Instance.Return(list2);
		data.FixInvalidData(areaId);
	}

	public static void ChangePlace(this CricketPlaceData data, short areaId, int index)
	{
		int minDistance = 1;
		int maxSteps = 3;
		MapBlockData[] blocks = DomainManager.Map.GetAreaBlocks(areaId).ToArray();
		int num = index / 3;
		MapBlockData mapBlockData = blocks[data.CricketBlocks[num * 3]];
		ByteCoordinate centerBlockPos = mapBlockData.GetBlockPos();
		Location taiwuLocation = DomainManager.Taiwu.GetTaiwu().GetLocation();
		List<short> list = ObjectPool<List<short>>.Instance.Get();
		List<short> otherGroupBlocks = ObjectPool<List<short>>.Instance.Get();
		List<MapBlockData> list2 = ObjectPool<List<MapBlockData>>.Instance.Get();
		List<MapBlockData> blockList2 = ObjectPool<List<MapBlockData>>.Instance.Get();
		list.Clear();
		for (int i = 0; i < 3; i++)
		{
			if (!data.CricketTriggered[num * 3 + i])
			{
				list.Add((short)(num * 3 + i));
			}
		}
		otherGroupBlocks.Clear();
		for (int j = 0; j < data.RealCircketIdx.Length; j++)
		{
			if (j != num && !data.CricketTriggered[j * 3 + data.RealCircketIdx[j]])
			{
				otherGroupBlocks.Add(data.CricketBlocks[j * 3]);
			}
		}
		DomainManager.Map.GetRealNeighborBlocks(areaId, mapBlockData.BlockId, list2, maxSteps);
		DomainManager.Map.GetRealNeighborBlocks(areaId, mapBlockData.BlockId, blockList2, minDistance);
		list2.RemoveAll((MapBlockData block) => blockList2.Contains(block) || block.IsCityTown() || block.TemplateId == 126 || block.TemplateId == 125 || Array.IndexOf(data.CricketBlocks, block.BlockId) >= 0 || (areaId == taiwuLocation.AreaId && block.BlockId == taiwuLocation.BlockId));
		list2.RemoveAll((MapBlockData block) => otherGroupBlocks.Exists(delegate(short blockId)
		{
			ByteCoordinate byteCoordinate = blocks[blockId].GetBlockPos() - block.GetBlockPos();
			return byteCoordinate.X <= minDistance && byteCoordinate.Y <= minDistance;
		}));
		list2.Sort((MapBlockData blockA, MapBlockData blockB) => blockA.GetBlockPos().GetManhattanDistance(centerBlockPos).CompareTo(blockB.GetBlockPos().GetManhattanDistance(centerBlockPos)));
		foreach (MapBlockData item in list2)
		{
			DomainManager.Map.GetRealNeighborBlocks(areaId, item.BlockId, blockList2);
			blockList2.RemoveAll((MapBlockData block) => block.IsCityTown() || block.TemplateId == 126 || Array.IndexOf(data.CricketBlocks, block.BlockId) >= 0 || (areaId == taiwuLocation.AreaId && block.BlockId == taiwuLocation.BlockId));
			blockList2.Insert(0, item);
			if (blockList2.Count < list.Count)
			{
				continue;
			}
			for (int num2 = 0; num2 < list.Count; num2++)
			{
				data.CricketBlocks[list[num2]] = blockList2[num2].BlockId;
			}
			break;
		}
		ObjectPool<List<short>>.Instance.Return(list);
		ObjectPool<List<short>>.Instance.Return(otherGroupBlocks);
		ObjectPool<List<MapBlockData>>.Instance.Return(list2);
		ObjectPool<List<MapBlockData>>.Instance.Return(blockList2);
	}

	internal static void FixInvalidData(this CricketPlaceData data, short areaId)
	{
		Span<MapBlockData> areaBlocks = DomainManager.Map.GetAreaBlocks(areaId);
		int i = 0;
		for (int num = data.CricketBlocks.Length; i < num; i++)
		{
			short num2 = data.CricketBlocks[i];
			if (num2 >= 0 && num2 < areaBlocks.Length)
			{
				MapBlockData mapBlockData = areaBlocks[num2];
				if (mapBlockData.TemplateId == 126)
				{
					data.CricketTriggered[i] = true;
				}
				if (mapBlockData.IsCityTown())
				{
					data.CricketTriggered[i] = true;
				}
			}
			else
			{
				data.CricketTriggered[i] = true;
			}
		}
	}
}
