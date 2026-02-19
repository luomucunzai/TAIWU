using System;
using GameData.Common;
using GameData.Domains.Information.Collection;
using GameData.Domains.Item;
using GameData.Domains.LifeRecord;
using GameData.Domains.Map;
using GameData.Domains.World.MonthlyEvent;

namespace GameData.Domains.Character.Ai.GeneralAction.HealthDemand;

public class RequestDetoxPoisonAction : IGeneralAction
{
	public ItemKey ItemUsed;

	public sbyte PoisonType;

	public bool AgreeToRequest;

	public sbyte ActionEnergyType => 0;

	public unsafe bool CheckValid(Character selfChar, Character targetChar)
	{
		PoisonInts poisoned = selfChar.GetPoisoned();
		return poisoned.Items[PoisonType] > 0 && targetChar.GetInventory().Items.ContainsKey(ItemUsed) && selfChar.GetEatingItems().GetAvailableEatingSlotsCount(selfChar.GetCurrMaxEatingSlotsCount()) > 0;
	}

	public void ApplyInitialChangesForTaiwu(DataContext context, Character selfChar, Character targetChar)
	{
		MonthlyEventCollection monthlyEventCollection = DomainManager.World.GetMonthlyEventCollection();
		int id = selfChar.GetId();
		Location location = targetChar.GetLocation();
		monthlyEventCollection.AddRequestHealPoisonByItem(id, location, targetChar.GetId(), (ulong)ItemUsed, PoisonType);
		CharacterDomain.AddLockMovementCharSet(id);
	}

	public void ApplyChanges(DataContext context, Character selfChar, Character targetChar)
	{
		int id = selfChar.GetId();
		int id2 = targetChar.GetId();
		LifeRecordCollection lifeRecordCollection = DomainManager.LifeRecord.GetLifeRecordCollection();
		SecretInformationCollection secretInformationCollection = DomainManager.Information.GetSecretInformationCollection();
		int currDate = DomainManager.World.GetCurrDate();
		Location location = selfChar.GetLocation();
		if (AgreeToRequest)
		{
			if (ItemUsed.ItemType != 8)
			{
				throw new Exception($"Invalid item type {ItemUsed} to detox poison for {selfChar.GetId()}");
			}
			ItemBase baseItem = DomainManager.Item.GetBaseItem(ItemUsed);
			targetChar.RemoveInventoryItem(context, ItemUsed, 1, deleteItem: false);
			selfChar.AddEatingItem(context, ItemUsed);
			selfChar.ChangeHappiness(context, baseItem.GetHappinessChange());
			DomainManager.Character.ChangeFavorabilityOptionalMonthlyEvolution(context, selfChar, targetChar, baseItem.GetFavorabilityChange() * 5);
			lifeRecordCollection.AddRequestDetoxPoisonSucceed(id, currDate, id2, location, ItemUsed.ItemType, ItemUsed.TemplateId, PoisonType);
			int dataOffset = secretInformationCollection.AddAcceptRequestDetoxPoison(id2, id);
			int num = DomainManager.Information.AddSecretInformationMetaData(context, dataOffset);
		}
		else
		{
			selfChar.ChangeHappiness(context, -3);
			DomainManager.Character.ChangeFavorabilityOptionalMonthlyEvolution(context, selfChar, targetChar, -6000);
			lifeRecordCollection.AddRequestDetoxPoisonFail(id, currDate, id2, location, ItemUsed.ItemType, ItemUsed.TemplateId, PoisonType);
			int dataOffset2 = secretInformationCollection.AddRefuseRequestDetoxPoison(id2, id);
			int num2 = DomainManager.Information.AddSecretInformationMetaData(context, dataOffset2);
		}
	}
}
