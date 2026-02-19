using GameData.Domains.Character;

namespace GameData.Domains.Organization;

public static class SettlementTreasuryHelper
{
	public static int GetMemberContribution(this SettlementTreasury settlementTreasury, GameData.Domains.Character.Character character)
	{
		int id = character.GetId();
		OrganizationInfo organizationInfo = character.GetOrganizationInfo();
		return settlementTreasury.GetMemberContribution(id, organizationInfo);
	}

	public static void OfflineChangeContribution(this SettlementTreasury settlementTreasury, GameData.Domains.Character.Character character, int delta)
	{
		int id = character.GetId();
		int contributionPerMonth = character.GetContributionPerMonth();
		settlementTreasury.OfflineChangeContribution(id, contributionPerMonth, delta);
	}
}
