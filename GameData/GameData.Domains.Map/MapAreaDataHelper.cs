using GameData.Utilities;

namespace GameData.Domains.Map;

public static class MapAreaDataHelper
{
	public static (short index, sbyte direction) GetReferenceSettlementAndDirection(this MapAreaData data, short blockId)
	{
		short areaId = data.GetAreaId();
		byte areaSize = DomainManager.Map.GetAreaSize(areaId);
		ByteCoordinate self = ByteCoordinate.IndexToCoordinate(blockId, areaSize);
		int num = -1;
		ByteCoordinate other = default(ByteCoordinate);
		int num2 = int.MaxValue;
		int i = 0;
		for (int num3 = data.SettlementInfos.Length; i < num3; i++)
		{
			short blockId2 = data.SettlementInfos[i].BlockId;
			if (blockId2 >= 0)
			{
				ByteCoordinate byteCoordinate = ByteCoordinate.IndexToCoordinate(blockId2, areaSize);
				int manhattanDistance = self.GetManhattanDistance(byteCoordinate);
				if (manhattanDistance < num2)
				{
					num = i;
					other = byteCoordinate;
					num2 = manhattanDistance;
				}
			}
		}
		sbyte item = ((num >= 0) ? self.GetDirectionRelatedTo(other) : self.GetDirectionIn(areaSize));
		return (index: (short)num, direction: item);
	}
}
