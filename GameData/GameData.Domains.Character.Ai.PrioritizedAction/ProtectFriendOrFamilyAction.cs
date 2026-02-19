using GameData.Common;
using GameData.Domains.Character.Relation;
using GameData.Domains.LifeRecord;
using GameData.Domains.Map;
using GameData.Domains.World.Notification;
using GameData.Serializer;

namespace GameData.Domains.Character.Ai.PrioritizedAction;

[SerializableGameData(NotForDisplayModule = true)]
public class ProtectFriendOrFamilyAction : BasePrioritizedAction
{
	public override short ActionType => 2;

	public override bool CheckValid(Character selfChar)
	{
		if (!base.CheckValid(selfChar))
		{
			return false;
		}
		if (!DomainManager.Character.IsCharacterAlive(Target.TargetCharId))
		{
			return false;
		}
		short favorability = DomainManager.Character.GetFavorability(selfChar.GetId(), Target.TargetCharId);
		sbyte favorabilityType = FavorabilityType.GetFavorabilityType(favorability);
		return favorabilityType >= AiHelper.PrioritizedActionConstants.PrioritizedActionMinFavorType[ActionType];
	}

	public override void OnStart(DataContext context, Character selfChar)
	{
		int id = selfChar.GetId();
		int leaderId = selfChar.GetLeaderId();
		if (leaderId >= 0 && leaderId != id)
		{
			DomainManager.Character.LeaveGroup(context, selfChar);
		}
		LifeRecordCollection lifeRecordCollection = DomainManager.LifeRecord.GetLifeRecordCollection();
		int currDate = DomainManager.World.GetCurrDate();
		Location location = selfChar.GetLocation();
		lifeRecordCollection.AddDecideToProtect(id, currDate, Target.TargetCharId, location);
		if (DomainManager.Character.IsTaiwuPeople(id) || DomainManager.Character.IsTaiwuPeople(Target.TargetCharId))
		{
			MonthlyNotificationCollection monthlyNotificationCollection = DomainManager.World.GetMonthlyNotificationCollection();
			monthlyNotificationCollection.AddGoToProtect(id, Target.TargetCharId);
		}
	}

	public override void OnInterrupt(DataContext context, Character selfChar)
	{
		int id = selfChar.GetId();
		LifeRecordCollection lifeRecordCollection = DomainManager.LifeRecord.GetLifeRecordCollection();
		int currDate = DomainManager.World.GetCurrDate();
		Location location = selfChar.GetLocation();
		lifeRecordCollection.AddFinishProtection(id, currDate, Target.TargetCharId, location);
	}

	public override bool Execute(DataContext context, Character selfChar)
	{
		return false;
	}
}
