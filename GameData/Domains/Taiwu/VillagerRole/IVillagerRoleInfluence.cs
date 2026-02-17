using System;
using GameData.Common;
using GameData.Domains.Map;
using GameData.Domains.Organization;

namespace GameData.Domains.Taiwu.VillagerRole
{
	// Token: 0x0200004A RID: 74
	public interface IVillagerRoleInfluence
	{
		// Token: 0x06001378 RID: 4984
		void ApplyInfluenceAction(DataContext context);

		// Token: 0x1700002D RID: 45
		// (get) Token: 0x06001379 RID: 4985
		int InfluenceSettlementValueChange { get; }

		// Token: 0x0600137A RID: 4986
		int GetSettlementInfluenceAuthorityGain(Settlement settlement);

		// Token: 0x0600137B RID: 4987 RVA: 0x00138A54 File Offset: 0x00136C54
		int GetAreaInfluenceAuthorityGain(short areaId)
		{
			bool flag = areaId < 0;
			int result;
			if (flag)
			{
				result = 0;
			}
			else
			{
				MapAreaData areaData = DomainManager.Map.GetElement_Areas((int)areaId);
				SettlementInfo[] settlementInfos = areaData.SettlementInfos;
				bool flag2 = settlementInfos == null || settlementInfos.Length <= 0;
				if (flag2)
				{
					result = 0;
				}
				else
				{
					int authorityGain = 0;
					for (int i = 0; i < areaData.SettlementInfos.Length; i++)
					{
						SettlementInfo settlementInfo = areaData.SettlementInfos[i];
						bool flag3 = settlementInfo.SettlementId < 0;
						if (!flag3)
						{
							Settlement settlement = DomainManager.Organization.GetSettlement(settlementInfo.SettlementId);
							authorityGain += this.GetSettlementInfluenceAuthorityGain(settlement);
						}
					}
					result = authorityGain;
				}
			}
			return result;
		}
	}
}
