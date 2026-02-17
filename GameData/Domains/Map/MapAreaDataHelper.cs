using System;
using System.Runtime.CompilerServices;
using GameData.Utilities;

namespace GameData.Domains.Map
{
	// Token: 0x020008B4 RID: 2228
	public static class MapAreaDataHelper
	{
		// Token: 0x060078BB RID: 30907 RVA: 0x00466B30 File Offset: 0x00464D30
		[return: TupleElementNames(new string[]
		{
			"index",
			"direction"
		})]
		public static ValueTuple<short, sbyte> GetReferenceSettlementAndDirection(this MapAreaData data, short blockId)
		{
			short areaId = data.GetAreaId();
			byte mapSize = DomainManager.Map.GetAreaSize(areaId);
			ByteCoordinate coord = ByteCoordinate.IndexToCoordinate(blockId, mapSize);
			int selectedIndex = -1;
			ByteCoordinate selectedCoord = default(ByteCoordinate);
			int selectedDistance = int.MaxValue;
			int i = 0;
			int count = data.SettlementInfos.Length;
			while (i < count)
			{
				short settlementBlockId = data.SettlementInfos[i].BlockId;
				bool flag = settlementBlockId < 0;
				if (!flag)
				{
					ByteCoordinate settlementCoord = ByteCoordinate.IndexToCoordinate(settlementBlockId, mapSize);
					int distance = coord.GetManhattanDistance(settlementCoord);
					bool flag2 = distance < selectedDistance;
					if (flag2)
					{
						selectedIndex = i;
						selectedCoord = settlementCoord;
						selectedDistance = distance;
					}
				}
				i++;
			}
			sbyte direction = (selectedIndex >= 0) ? coord.GetDirectionRelatedTo(selectedCoord) : coord.GetDirectionIn(mapSize);
			return new ValueTuple<short, sbyte>((short)selectedIndex, direction);
		}
	}
}
