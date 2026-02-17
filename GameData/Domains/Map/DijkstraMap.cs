using System;
using System.Collections.Generic;
using GameData.Common.Algorithm;

namespace GameData.Domains.Map
{
	// Token: 0x020008B2 RID: 2226
	public class DijkstraMap : DijkstraAlgorithm<short>
	{
		// Token: 0x060078B1 RID: 30897 RVA: 0x004669BD File Offset: 0x00464BBD
		public void Initialize(IEnumerable<short> allAreas, DijkstraAlgorithm<short>.DijkstraAlgorithmGetNeighbors getNeighborsDelegate)
		{
			base.Initialize(allAreas, getNeighborsDelegate, -1);
		}
	}
}
