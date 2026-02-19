using GameData.Utilities;

namespace GameData.Domains.Map;

public static class MapExtensions
{
	public static bool IsNearbyLocation(this Location location, Location targetLocation, int steps)
	{
		if (!targetLocation.IsValid())
		{
			return false;
		}
		if (targetLocation.AreaId != location.AreaId)
		{
			return false;
		}
		byte areaSize = DomainManager.Map.GetAreaSize(location.AreaId);
		ByteCoordinate byteCoordinate = ByteCoordinate.IndexToCoordinate(targetLocation.BlockId, areaSize);
		ByteCoordinate byteCoordinate2 = ByteCoordinate.IndexToCoordinate(location.BlockId, areaSize);
		return byteCoordinate.GetManhattanDistance(byteCoordinate2) <= steps;
	}
}
