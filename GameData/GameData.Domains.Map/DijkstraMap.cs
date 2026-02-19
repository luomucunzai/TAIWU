using System.Collections.Generic;
using GameData.Common.Algorithm;

namespace GameData.Domains.Map;

public class DijkstraMap : DijkstraAlgorithm<short>
{
	public void Initialize(IEnumerable<short> allAreas, DijkstraAlgorithmGetNeighbors<short> getNeighborsDelegate)
	{
		base.Initialize(allAreas, getNeighborsDelegate, (short)(-1));
	}
}
