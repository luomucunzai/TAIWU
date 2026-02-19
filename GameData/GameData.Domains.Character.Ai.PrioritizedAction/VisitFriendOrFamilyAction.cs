using GameData.Common;
using GameData.Domains.Character.Relation;
using GameData.Domains.LifeRecord;
using GameData.Domains.Map;
using GameData.Serializer;

namespace GameData.Domains.Character.Ai.PrioritizedAction;

[SerializableGameData(NotForDisplayModule = true)]
public class VisitFriendOrFamilyAction : BasePrioritizedAction
{
	public override short ActionType => 5;

	public override bool CheckValid(Character selfChar)
	{
		if (!base.CheckValid(selfChar))
		{
			return false;
		}
		if (!DomainManager.Character.TryGetElement_Objects(Target.TargetCharId, out var element))
		{
			return false;
		}
		if (element.IsCompletelyInfected())
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
		LifeRecordCollection lifeRecordCollection = DomainManager.LifeRecord.GetLifeRecordCollection();
		int currDate = DomainManager.World.GetCurrDate();
		Location location = selfChar.GetLocation();
		lifeRecordCollection.AddDecideToVisit(id, currDate, Target.TargetCharId, location);
	}

	public override void OnInterrupt(DataContext context, Character selfChar)
	{
		int id = selfChar.GetId();
		LifeRecordCollection lifeRecordCollection = DomainManager.LifeRecord.GetLifeRecordCollection();
		int currDate = DomainManager.World.GetCurrDate();
		Location location = selfChar.GetLocation();
		lifeRecordCollection.AddFinishVisit(id, currDate, Target.TargetCharId, location);
	}

	public override bool Execute(DataContext context, Character selfChar)
	{
		PersonalNeed personalNeed = PersonalNeed.CreatePersonalNeed(19, Target.TargetCharId);
		selfChar.AddPersonalNeed(context, personalNeed);
		return false;
	}
}
