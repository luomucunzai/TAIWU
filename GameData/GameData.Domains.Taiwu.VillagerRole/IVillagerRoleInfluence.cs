using GameData.Common;
using GameData.Domains.Map;
using GameData.Domains.Organization;

namespace GameData.Domains.Taiwu.VillagerRole;

public interface IVillagerRoleInfluence
{
	int InfluenceSettlementValueChange { get; }

	void ApplyInfluenceAction(DataContext context);

	int GetSettlementInfluenceAuthorityGain(Settlement settlement);

	int GetAreaInfluenceAuthorityGain(short areaId)
	{
		if (areaId < 0)
		{
			return 0;
		}
		MapAreaData element_Areas = DomainManager.Map.GetElement_Areas(areaId);
		SettlementInfo[] settlementInfos = element_Areas.SettlementInfos;
		if (settlementInfos == null || settlementInfos.Length <= 0)
		{
			return 0;
		}
		int num = 0;
		for (int i = 0; i < element_Areas.SettlementInfos.Length; i++)
		{
			SettlementInfo settlementInfo = element_Areas.SettlementInfos[i];
			if (settlementInfo.SettlementId >= 0)
			{
				Settlement settlement = DomainManager.Organization.GetSettlement(settlementInfo.SettlementId);
				num += GetSettlementInfluenceAuthorityGain(settlement);
			}
		}
		return num;
	}
}
