using GameData.Common;
using GameData.Domains.LifeRecord;
using GameData.Domains.Map;
using GameData.Domains.Organization;
using GameData.Serializer;
using GameData.Utilities;

namespace GameData.Domains.Character.Ai.PrioritizedAction;

[SerializableGameData(NotForDisplayModule = true, IsExtensible = true, NoCopyConstructors = true)]
public class EscapeFromPrisonAction : ExtensiblePrioritizedAction
{
	public override short ActionType => 18;

	public override bool CheckValid(Character selfChar)
	{
		if (selfChar.GetOrganizationInfo().OrgTemplateId != 0)
		{
			return false;
		}
		if (DomainManager.Organization.GetFugitiveBountySect(selfChar.GetId()) < 0)
		{
			return false;
		}
		return base.CheckValid(selfChar);
	}

	public override void OnStart(DataContext context, Character selfChar)
	{
		string name = DomainManager.Map.GetElement_Areas(selfChar.GetLocation().AreaId).GetConfig().Name;
		string name2 = DomainManager.Map.GetElement_Areas(Target.GetRealTargetLocation().AreaId).GetConfig().Name;
		AdaptableLog.Info($"{selfChar} 开始畏罪潜逃: {name} => {name2}.");
		if (selfChar.GetLeaderId() >= 0)
		{
			DomainManager.Character.LeaveGroup(context, selfChar);
		}
		int id = selfChar.GetId();
		int currDate = DomainManager.World.GetCurrDate();
		Location location = selfChar.GetLocation();
		sbyte fugitiveBountySect = DomainManager.Organization.GetFugitiveBountySect(selfChar.GetId());
		Sect sect = (Sect)DomainManager.Organization.GetSettlementByOrgTemplateId(fugitiveBountySect);
		short id2 = sect.GetId();
		SettlementBounty bounty = sect.Prison.GetBounty(id);
		Location location2 = new Location(Target.GetRealTargetLocation().AreaId, -1);
		LifeRecordCollection lifeRecordCollection = DomainManager.LifeRecord.GetLifeRecordCollection();
		lifeRecordCollection.AddDecideToEscapePunishment(id, currDate, location, bounty.PunishmentType, id2, location2);
	}

	public override void OnInterrupt(DataContext context, Character selfChar)
	{
		AdaptableLog.Info($"{selfChar} 结束了畏罪潜逃.");
		int id = selfChar.GetId();
		Location location = selfChar.GetLocation();
		int currDate = DomainManager.World.GetCurrDate();
		LifeRecordCollection lifeRecordCollection = DomainManager.LifeRecord.GetLifeRecordCollection();
		lifeRecordCollection.AddFinishEscapePunishment(id, currDate, location);
		sbyte idealSect = selfChar.GetIdealSect();
		if (idealSect >= 0 && idealSect != DomainManager.Organization.GetFugitiveBountySect(id))
		{
			selfChar.AddPersonalNeed(context, PersonalNeed.CreatePersonalNeed(26, idealSect));
		}
	}

	public override void OnArrival(DataContext context, Character selfChar)
	{
		AdaptableLog.Info($"{selfChar} 到达目的地: {DomainManager.Map.GetElement_Areas(selfChar.GetValidLocation().AreaId).GetConfig().Name}");
		base.OnArrival(context, selfChar);
	}

	public override bool Execute(DataContext context, Character selfChar)
	{
		return false;
	}
}
