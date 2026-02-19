using GameData.Common;
using GameData.Domains.LifeRecord;
using GameData.Domains.Map;
using GameData.Domains.Organization;
using GameData.Serializer;

namespace GameData.Domains.Character.Ai.PrioritizedAction;

[SerializableGameData(NotForDisplayModule = true)]
public class GuardTreasuryAction : BasePrioritizedAction
{
	public override short ActionType => 15;

	public override bool CheckValid(Character selfChar)
	{
		short settlementId = selfChar.GetOrganizationInfo().SettlementId;
		if (settlementId < 0)
		{
			return false;
		}
		Settlement settlement = DomainManager.Organization.GetSettlement(settlementId);
		if (!settlement.Treasuries.IsGuard(selfChar.GetId()))
		{
			return false;
		}
		if (DomainManager.LegendaryBook.IsCharacterLegendaryBookOwnerOrContest(selfChar.GetId()))
		{
			return false;
		}
		return base.CheckValid(selfChar);
	}

	public override void OnStart(DataContext context, Character selfChar)
	{
		LifeRecordCollection lifeRecordCollection = DomainManager.LifeRecord.GetLifeRecordCollection();
		int id = selfChar.GetId();
		int currDate = DomainManager.World.GetCurrDate();
		short settlementId = selfChar.GetOrganizationInfo().SettlementId;
		Location location = selfChar.GetLocation();
		lifeRecordCollection.AddDecideToGuardTreasury(id, currDate, location, settlementId);
		if (selfChar.GetLeaderId() != selfChar.GetId())
		{
			DomainManager.Character.LeaveGroup(context, selfChar);
		}
	}

	public override void OnInterrupt(DataContext context, Character selfChar)
	{
		LifeRecordCollection lifeRecordCollection = DomainManager.LifeRecord.GetLifeRecordCollection();
		int id = selfChar.GetId();
		int currDate = DomainManager.World.GetCurrDate();
		short id2 = DomainManager.Organization.GetSettlementByLocation(Target.GetRealTargetLocation()).GetId();
		Location location = selfChar.GetLocation();
		lifeRecordCollection.AddFinishGuardingTreasury(id, currDate, location, id2);
		selfChar.RemoveFeatureGroup(context, 536);
	}

	public override bool Execute(DataContext context, Character selfChar)
	{
		return false;
	}
}
