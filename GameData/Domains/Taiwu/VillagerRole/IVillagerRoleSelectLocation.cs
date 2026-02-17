using System;
using System.Collections.Generic;
using GameData.Domains.Map;
using GameData.Utilities;
using Redzen.Random;

namespace GameData.Domains.Taiwu.VillagerRole
{
	// Token: 0x0200004B RID: 75
	public interface IVillagerRoleSelectLocation
	{
		// Token: 0x0600137C RID: 4988 RVA: 0x00138B08 File Offset: 0x00136D08
		Location SelectNextWorkLocation(IRandomSource random, Location baseLocation)
		{
			bool flag = baseLocation.BlockId >= 0;
			Location result;
			if (flag)
			{
				MapBlockData block = DomainManager.Map.GetBlock(baseLocation);
				List<MapBlockData> groupBlockList = block.GroupBlockList;
				result = ((groupBlockList != null && groupBlockList.Count > 0) ? block.GroupBlockList.GetRandom(random).GetLocation() : baseLocation);
			}
			else
			{
				List<MapBlockData> validBlocks = ObjectPool<List<MapBlockData>>.Instance.Get();
				DomainManager.Map.GetMapBlocksInAreaByFilters(baseLocation.AreaId, new Predicate<MapBlockData>(this.NextLocationFilter), validBlocks);
				bool flag2 = validBlocks.Count == 0;
				if (flag2)
				{
					ObjectPool<List<MapBlockData>>.Instance.Return(validBlocks);
					short stationBlockId = DomainManager.Map.GetElement_Areas((int)baseLocation.AreaId).StationBlockId;
					result = new Location(baseLocation.AreaId, stationBlockId);
				}
				else
				{
					MapBlockData nextBlock = validBlocks.GetRandom(random);
					ObjectPool<List<MapBlockData>>.Instance.Return(validBlocks);
					result = nextBlock.GetLocation();
				}
			}
			return result;
		}

		// Token: 0x0600137D RID: 4989 RVA: 0x00138BF4 File Offset: 0x00136DF4
		bool NextLocationFilter(MapBlockData block)
		{
			return !block.IsNonDeveloped() && block.CharacterSet != null;
		}
	}
}
