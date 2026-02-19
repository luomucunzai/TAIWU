using GameData.Common;
using GameData.Domains.Item;
using GameData.Domains.LifeRecord;
using GameData.Domains.Map;
using GameData.Domains.World.Notification;

namespace GameData.Domains.Character.Ai.GeneralAction.BehaviorAction;

public class GiveItemAction : IGeneralAction
{
	public ItemKey TargetItem;

	public int Amount;

	public bool RefusePoisonousItem;

	public sbyte ActionEnergyType => 3;

	public bool CheckValid(Character selfChar, Character targetChar)
	{
		int value;
		return selfChar.GetInventory().Items.TryGetValue(TargetItem, out value) && value >= Amount;
	}

	public void ApplyInitialChangesForTaiwu(DataContext context, Character selfChar, Character targetChar)
	{
		int id = selfChar.GetId();
		int id2 = targetChar.GetId();
		Location location = selfChar.GetLocation();
		MonthlyNotificationCollection monthlyNotificationCollection = DomainManager.World.GetMonthlyNotificationCollection();
		monthlyNotificationCollection.AddGivePresentItem(id, location, TargetItem.ItemType, TargetItem.TemplateId, id2);
		ApplyChanges(context, selfChar, targetChar);
	}

	public void ApplyChanges(DataContext context, Character selfChar, Character targetChar)
	{
		int id = selfChar.GetId();
		int id2 = targetChar.GetId();
		int currDate = DomainManager.World.GetCurrDate();
		Location location = selfChar.GetLocation();
		LifeRecordCollection lifeRecordCollection = DomainManager.LifeRecord.GetLifeRecordCollection();
		if (RefusePoisonousItem)
		{
			lifeRecordCollection.AddRefusePoisonousGift(id, currDate, id2, location, TargetItem.ItemType, TargetItem.TemplateId);
			return;
		}
		lifeRecordCollection.AddGiveItem(id, currDate, id2, location, TargetItem.ItemType, TargetItem.TemplateId);
		DomainManager.Character.TransferInventoryItem(context, selfChar, targetChar, TargetItem, Amount);
		ItemBase baseItem = DomainManager.Item.GetBaseItem(TargetItem);
		int favorabilityChange = baseItem.GetFavorabilityChange();
		DomainManager.Character.ChangeFavorabilityOptionalMonthlyEvolution(context, targetChar, selfChar, favorabilityChange);
		targetChar.ChangeHappiness(context, baseItem.GetHappinessChange());
	}
}
