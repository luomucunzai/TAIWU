using System;
using GameData.Utilities;

namespace GameData.Domains.Map
{
	// Token: 0x020008B8 RID: 2232
	public static class MapExtensions
	{
		// Token: 0x06007BA7 RID: 31655 RVA: 0x0048E868 File Offset: 0x0048CA68
		public static bool IsNearbyLocation(this Location location, Location targetLocation, int steps)
		{
			bool flag = !targetLocation.IsValid();
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				bool flag2 = targetLocation.AreaId != location.AreaId;
				if (flag2)
				{
					result = false;
				}
				else
				{
					byte areaData = DomainManager.Map.GetAreaSize(location.AreaId);
					ByteCoordinate targetCoordinate = ByteCoordinate.IndexToCoordinate(targetLocation.BlockId, areaData);
					ByteCoordinate selfCoordinate = ByteCoordinate.IndexToCoordinate(location.BlockId, areaData);
					result = (targetCoordinate.GetManhattanDistance(selfCoordinate) <= steps);
				}
			}
			return result;
		}
	}
}
