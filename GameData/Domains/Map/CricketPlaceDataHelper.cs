using System;
using System.Collections.Generic;
using GameData.Utilities;
using Redzen.Random;

namespace GameData.Domains.Map
{
	// Token: 0x020008AF RID: 2223
	public static class CricketPlaceDataHelper
	{
		// Token: 0x060078AB RID: 30891 RVA: 0x00465E46 File Offset: 0x00464046
		public static bool BlockAllowCricket(MapBlockData block)
		{
			return block.TemplateId != 126 && block.TemplateId != 125 && !DomainManager.Map.IsLocationInFulongFlameArea(block.GetLocation());
		}

		// Token: 0x060078AC RID: 30892 RVA: 0x00465E74 File Offset: 0x00464074
		public unsafe static void Init(this CricketPlaceData data, short areaId, IRandomSource random, int minGroupCount = 3, int maxGroupCount = 5, int minDistance = -1, int maxDistance = -1)
		{
			Span<MapBlockData> blocks = DomainManager.Map.GetAreaBlocks(areaId);
			int groupCount = random.Next(minGroupCount, maxGroupCount + 1);
			data.CricketBlocks = new short[3 * groupCount];
			data.CricketTriggered = new bool[3 * groupCount];
			data.RealCircketIdx = new byte[groupCount];
			Location taiwuLocation = DomainManager.Taiwu.GetTaiwu().GetLocation();
			ByteCoordinate taiwuPos = taiwuLocation.IsValid() ? DomainManager.Map.GetBlock(taiwuLocation).GetBlockPos() : new ByteCoordinate(0, 0);
			List<MapBlockData> availableBlocks = ObjectPool<List<MapBlockData>>.Instance.Get();
			List<MapBlockData> blockRandomPool = ObjectPool<List<MapBlockData>>.Instance.Get();
			List<MapBlockData> neighborList = ObjectPool<List<MapBlockData>>.Instance.Get();
			availableBlocks.Clear();
			for (int i = 0; i < blocks.Length; i++)
			{
				MapBlockData block2 = *blocks[i];
				bool flag = CricketPlaceDataHelper.BlockAllowCricket(block2) && (areaId != taiwuLocation.AreaId || block2.BlockId != taiwuLocation.BlockId) && (minDistance < 0 || (taiwuLocation.IsValid() && (int)block2.GetManhattanDistanceToPos(taiwuPos.X, taiwuPos.Y) >= minDistance)) && (maxDistance < 0 || (taiwuLocation.IsValid() && (int)block2.GetManhattanDistanceToPos(taiwuPos.X, taiwuPos.Y) <= maxDistance));
				if (flag)
				{
					availableBlocks.Add(block2);
				}
			}
			int generatedGroupCount = 0;
			int rerandomCounter = 0;
			Predicate<MapBlockData> <>9__0;
			while (generatedGroupCount < minGroupCount && rerandomCounter < 50)
			{
				blockRandomPool.Clear();
				blockRandomPool.AddRange(availableBlocks);
				CollectionUtils.Shuffle<MapBlockData>(random, blockRandomPool);
				generatedGroupCount = 0;
				for (int j = 0; j < groupCount; j++)
				{
					for (int k = 0; k < blockRandomPool.Count; k++)
					{
						MapBlockData center = blockRandomPool[k];
						DomainManager.Map.GetRealNeighborBlocks(center.AreaId, center.BlockId, neighborList, 1, false);
						List<MapBlockData> list = neighborList;
						Predicate<MapBlockData> match;
						if ((match = <>9__0) == null)
						{
							match = (<>9__0 = ((MapBlockData block) => !blockRandomPool.Contains(block)));
						}
						list.RemoveAll(match);
						bool flag2 = neighborList.Count < 2;
						if (!flag2)
						{
							CollectionUtils.Shuffle<MapBlockData>(random, neighborList);
							data.CricketBlocks[3 * j] = center.BlockId;
							data.CricketBlocks[3 * j + 1] = neighborList[0].BlockId;
							data.CricketBlocks[3 * j + 2] = neighborList[1].BlockId;
							data.RealCircketIdx[j] = (byte)random.Next(3);
							blockRandomPool.RemoveAll(delegate(MapBlockData block)
							{
								ByteCoordinate offset = block.GetBlockPos() - center.GetBlockPos();
								return offset.X <= 3 && offset.Y <= 3;
							});
							generatedGroupCount++;
							break;
						}
					}
				}
				rerandomCounter++;
			}
			ObjectPool<List<MapBlockData>>.Instance.Return(availableBlocks);
			ObjectPool<List<MapBlockData>>.Instance.Return(blockRandomPool);
			ObjectPool<List<MapBlockData>>.Instance.Return(neighborList);
			data.FixInvalidData(areaId);
		}

		// Token: 0x060078AD RID: 30893 RVA: 0x004661BC File Offset: 0x004643BC
		public static void ChangePlace(this CricketPlaceData data, short areaId, int index)
		{
			int minDistance = 1;
			int maxDistance = 3;
			MapBlockData[] blocks = DomainManager.Map.GetAreaBlocks(areaId).ToArray();
			int groupIndex = index / 3;
			MapBlockData centerBlock = blocks[(int)data.CricketBlocks[groupIndex * 3]];
			ByteCoordinate centerBlockPos = centerBlock.GetBlockPos();
			Location taiwuLocation = DomainManager.Taiwu.GetTaiwu().GetLocation();
			List<short> remainIndexList = ObjectPool<List<short>>.Instance.Get();
			List<short> otherGroupBlocks = ObjectPool<List<short>>.Instance.Get();
			List<MapBlockData> blockList = ObjectPool<List<MapBlockData>>.Instance.Get();
			List<MapBlockData> blockList2 = ObjectPool<List<MapBlockData>>.Instance.Get();
			remainIndexList.Clear();
			for (int i = 0; i < 3; i++)
			{
				bool flag = !data.CricketTriggered[groupIndex * 3 + i];
				if (flag)
				{
					remainIndexList.Add((short)(groupIndex * 3 + i));
				}
			}
			otherGroupBlocks.Clear();
			for (int j = 0; j < data.RealCircketIdx.Length; j++)
			{
				bool flag2 = j != groupIndex && !data.CricketTriggered[j * 3 + (int)data.RealCircketIdx[j]];
				if (flag2)
				{
					otherGroupBlocks.Add(data.CricketBlocks[j * 3]);
				}
			}
			DomainManager.Map.GetRealNeighborBlocks(areaId, centerBlock.BlockId, blockList, maxDistance, false);
			DomainManager.Map.GetRealNeighborBlocks(areaId, centerBlock.BlockId, blockList2, minDistance, false);
			blockList.RemoveAll((MapBlockData block) => blockList2.Contains(block) || block.IsCityTown() || block.TemplateId == 126 || block.TemplateId == 125 || Array.IndexOf<short>(data.CricketBlocks, block.BlockId) >= 0 || (areaId == taiwuLocation.AreaId && block.BlockId == taiwuLocation.BlockId));
			blockList.RemoveAll((MapBlockData block) => otherGroupBlocks.Exists(delegate(short blockId)
			{
				ByteCoordinate offset = blocks[(int)blockId].GetBlockPos() - block.GetBlockPos();
				return (int)offset.X <= minDistance && (int)offset.Y <= minDistance;
			}));
			blockList.Sort((MapBlockData blockA, MapBlockData blockB) => blockA.GetBlockPos().GetManhattanDistance(centerBlockPos).CompareTo(blockB.GetBlockPos().GetManhattanDistance(centerBlockPos)));
			Predicate<MapBlockData> <>9__4;
			foreach (MapBlockData block2 in blockList)
			{
				DomainManager.Map.GetRealNeighborBlocks(areaId, block2.BlockId, blockList2, 1, false);
				List<MapBlockData> blockList3 = blockList2;
				Predicate<MapBlockData> match;
				if ((match = <>9__4) == null)
				{
					match = (<>9__4 = ((MapBlockData block) => block.IsCityTown() || block.TemplateId == 126 || Array.IndexOf<short>(data.CricketBlocks, block.BlockId) >= 0 || (areaId == taiwuLocation.AreaId && block.BlockId == taiwuLocation.BlockId)));
				}
				blockList3.RemoveAll(match);
				blockList2.Insert(0, block2);
				bool flag3 = blockList2.Count < remainIndexList.Count;
				if (!flag3)
				{
					for (int k = 0; k < remainIndexList.Count; k++)
					{
						data.CricketBlocks[(int)remainIndexList[k]] = blockList2[k].BlockId;
					}
					break;
				}
			}
			ObjectPool<List<short>>.Instance.Return(remainIndexList);
			ObjectPool<List<short>>.Instance.Return(otherGroupBlocks);
			ObjectPool<List<MapBlockData>>.Instance.Return(blockList);
			ObjectPool<List<MapBlockData>>.Instance.Return(blockList2);
		}

		// Token: 0x060078AE RID: 30894 RVA: 0x004664F8 File Offset: 0x004646F8
		internal unsafe static void FixInvalidData(this CricketPlaceData data, short areaId)
		{
			Span<MapBlockData> blocks = DomainManager.Map.GetAreaBlocks(areaId);
			int i = 0;
			int len = data.CricketBlocks.Length;
			while (i < len)
			{
				short blockId = data.CricketBlocks[i];
				bool flag = blockId >= 0 && (int)blockId < blocks.Length;
				if (flag)
				{
					MapBlockData block = *blocks[(int)blockId];
					bool flag2 = block.TemplateId == 126;
					if (flag2)
					{
						data.CricketTriggered[i] = true;
					}
					bool flag3 = block.IsCityTown();
					if (flag3)
					{
						data.CricketTriggered[i] = true;
					}
				}
				else
				{
					data.CricketTriggered[i] = true;
				}
				i++;
			}
		}
	}
}
