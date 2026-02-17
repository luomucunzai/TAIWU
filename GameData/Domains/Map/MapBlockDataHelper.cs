using System;
using GameData.Common;

namespace GameData.Domains.Map
{
	// Token: 0x020008B5 RID: 2229
	public static class MapBlockDataHelper
	{
		// Token: 0x060078BC RID: 30908 RVA: 0x00466C04 File Offset: 0x00464E04
		public static void SetVisible(this MapBlockData blockData, bool visible, DataContext context)
		{
			blockData.Visible = visible;
			DomainManager.Map.SetBlockData(context, blockData);
			if (visible)
			{
				MapBlockData rootBlockData = blockData.GetRootBlock();
				bool flag = rootBlockData.GroupBlockList != null;
				if (flag)
				{
					foreach (MapBlockData groupBlock in rootBlockData.GroupBlockList)
					{
						groupBlock.Visible = true;
						DomainManager.Map.SetBlockData(context, groupBlock);
					}
				}
				rootBlockData.Visible = true;
				DomainManager.Map.SetBlockData(context, rootBlockData);
			}
		}
	}
}
