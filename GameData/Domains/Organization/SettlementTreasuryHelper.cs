using System;
using GameData.Domains.Character;

namespace GameData.Domains.Organization
{
	// Token: 0x0200064F RID: 1615
	public static class SettlementTreasuryHelper
	{
		// Token: 0x06004870 RID: 18544 RVA: 0x0028D13C File Offset: 0x0028B33C
		public static int GetMemberContribution(this SettlementTreasury settlementTreasury, Character character)
		{
			int charId = character.GetId();
			OrganizationInfo orgInfo = character.GetOrganizationInfo();
			return settlementTreasury.GetMemberContribution(charId, orgInfo);
		}

		// Token: 0x06004871 RID: 18545 RVA: 0x0028D164 File Offset: 0x0028B364
		public static void OfflineChangeContribution(this SettlementTreasury settlementTreasury, Character character, int delta)
		{
			int charId = character.GetId();
			int presetContribution = character.GetContributionPerMonth();
			settlementTreasury.OfflineChangeContribution(charId, presetContribution, delta);
		}
	}
}
