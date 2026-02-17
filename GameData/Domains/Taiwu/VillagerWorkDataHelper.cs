using System;
using GameData.Domains.Map;
using GameData.Domains.Taiwu.VillagerRole;

namespace GameData.Domains.Taiwu
{
	// Token: 0x02000046 RID: 70
	public static class VillagerWorkDataHelper
	{
		// Token: 0x0600136E RID: 4974 RVA: 0x00138954 File Offset: 0x00136B54
		public static VillagerRoleBase GetVillagerRole(this VillagerWorkData workData)
		{
			return DomainManager.Extra.GetVillagerRole(workData.CharacterId);
		}

		// Token: 0x0600136F RID: 4975 RVA: 0x00138978 File Offset: 0x00136B78
		public static int GetCollectResourceIncome(this VillagerWorkData workData)
		{
			Location blockKey = workData.Location;
			MapBlockData block = DomainManager.Map.GetBlock(blockKey);
			VillagerRoleFarmer farmer = workData.GetVillagerRole() as VillagerRoleFarmer;
			bool flag = farmer != null;
			int result;
			if (flag)
			{
				result = farmer.GetCollectResourceAmount(block, workData.ResourceType);
			}
			else
			{
				int addResource = block.GetCollectResourceAmount(workData.ResourceType);
				result = addResource;
			}
			return result;
		}
	}
}
