using System.Collections.Generic;
using GameData.Domains.Map;
using GameData.Utilities;
using Redzen.Random;

namespace GameData.Domains.Taiwu.VillagerRole;

public interface IVillagerRoleSelectLocation
{
	Location SelectNextWorkLocation(IRandomSource random, Location baseLocation)
	{
		if (baseLocation.BlockId >= 0)
		{
			MapBlockData block = DomainManager.Map.GetBlock(baseLocation);
			List<MapBlockData> groupBlockList = block.GroupBlockList;
			return (groupBlockList != null && groupBlockList.Count > 0) ? block.GroupBlockList.GetRandom(random).GetLocation() : baseLocation;
		}
		List<MapBlockData> list = ObjectPool<List<MapBlockData>>.Instance.Get();
		DomainManager.Map.GetMapBlocksInAreaByFilters(baseLocation.AreaId, NextLocationFilter, list);
		if (list.Count == 0)
		{
			ObjectPool<List<MapBlockData>>.Instance.Return(list);
			short stationBlockId = DomainManager.Map.GetElement_Areas(baseLocation.AreaId).StationBlockId;
			return new Location(baseLocation.AreaId, stationBlockId);
		}
		MapBlockData random2 = list.GetRandom(random);
		ObjectPool<List<MapBlockData>>.Instance.Return(list);
		return random2.GetLocation();
	}

	bool NextLocationFilter(MapBlockData block)
	{
		return !block.IsNonDeveloped() && block.CharacterSet != null;
	}
}
