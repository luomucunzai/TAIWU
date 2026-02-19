using GameData.Domains.Map;
using GameData.Domains.Taiwu.VillagerRole;

namespace GameData.Domains.Taiwu;

public static class VillagerWorkDataHelper
{
	public static VillagerRoleBase GetVillagerRole(this VillagerWorkData workData)
	{
		return DomainManager.Extra.GetVillagerRole(workData.CharacterId);
	}

	public static int GetCollectResourceIncome(this VillagerWorkData workData)
	{
		Location location = workData.Location;
		MapBlockData block = DomainManager.Map.GetBlock(location);
		if (workData.GetVillagerRole() is VillagerRoleFarmer villagerRoleFarmer)
		{
			return villagerRoleFarmer.GetCollectResourceAmount(block, workData.ResourceType);
		}
		return block.GetCollectResourceAmount(workData.ResourceType);
	}
}
