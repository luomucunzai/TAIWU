using GameData.Common;
using GameData.Domains.LifeRecord;
using GameData.Domains.Map;
using GameData.Domains.World.Notification;

namespace GameData.Domains.Character.Ai.GeneralAction.BehaviorAction;

public class GiveResourceAction : IGeneralAction
{
	public sbyte ResourceType;

	public int Amount;

	public sbyte ActionEnergyType => 3;

	public bool CheckValid(Character selfChar, Character targetChar)
	{
		return selfChar.GetResource(ResourceType) >= Amount;
	}

	public void ApplyInitialChangesForTaiwu(DataContext context, Character selfChar, Character targetChar)
	{
		int id = selfChar.GetId();
		int id2 = targetChar.GetId();
		Location location = selfChar.GetLocation();
		MonthlyNotificationCollection monthlyNotificationCollection = DomainManager.World.GetMonthlyNotificationCollection();
		monthlyNotificationCollection.AddGivePresentResource(id, location, ResourceType, id2);
		ApplyChanges(context, selfChar, targetChar);
	}

	public void ApplyChanges(DataContext context, Character selfChar, Character targetChar)
	{
		int id = selfChar.GetId();
		int id2 = targetChar.GetId();
		int currDate = DomainManager.World.GetCurrDate();
		Location location = selfChar.GetLocation();
		selfChar.ChangeResource(context, ResourceType, -Amount);
		targetChar.ChangeResource(context, ResourceType, Amount);
		short resourceFavorabilityChange = AiHelper.GeneralActionConstants.GetResourceFavorabilityChange(ResourceType, Amount);
		sbyte resourceHappinessChange = AiHelper.GeneralActionConstants.GetResourceHappinessChange(ResourceType, Amount);
		DomainManager.Character.ChangeFavorabilityOptionalMonthlyEvolution(context, targetChar, selfChar, resourceFavorabilityChange);
		targetChar.ChangeHappiness(context, resourceHappinessChange);
		LifeRecordCollection lifeRecordCollection = DomainManager.LifeRecord.GetLifeRecordCollection();
		lifeRecordCollection.AddGiveResource(id, currDate, id2, location, ResourceType, Amount);
	}
}
