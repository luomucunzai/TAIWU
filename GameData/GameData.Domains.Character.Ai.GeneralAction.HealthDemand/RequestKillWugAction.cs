using System;
using GameData.Common;
using GameData.Domains.Information.Collection;
using GameData.Domains.Item;
using GameData.Domains.LifeRecord;
using GameData.Domains.Map;
using GameData.Domains.World.MonthlyEvent;

namespace GameData.Domains.Character.Ai.GeneralAction.HealthDemand;

public class RequestKillWugAction : IGeneralAction
{
	public ItemKey ItemUsed;

	public sbyte WugType;

	public bool AgreeToRequest;

	public sbyte ActionEnergyType => 0;

	public bool CheckValid(Character selfChar, Character targetChar)
	{
		return selfChar.GetEatingItems().IndexOfWug(WugType) >= 0 && targetChar.GetInventory().Items.ContainsKey(ItemUsed) && selfChar.GetEatingItems().GetAvailableEatingSlotsCount(selfChar.GetCurrMaxEatingSlotsCount()) > 0;
	}

	public unsafe void ApplyInitialChangesForTaiwu(DataContext context, Character selfChar, Character targetChar)
	{
		MonthlyEventCollection monthlyEventCollection = DomainManager.World.GetMonthlyEventCollection();
		int id = selfChar.GetId();
		Location location = targetChar.GetLocation();
		EatingItems eatingItems = selfChar.GetEatingItems();
		ulong itemKey = eatingItems.ItemKeys[eatingItems.IndexOfWug(WugType)];
		monthlyEventCollection.AddRequestKillWug(id, location, targetChar.GetId(), (ulong)ItemUsed, itemKey);
		CharacterDomain.AddLockMovementCharSet(id);
	}

	public unsafe void ApplyChanges(DataContext context, Character selfChar, Character targetChar)
	{
		int id = selfChar.GetId();
		int id2 = targetChar.GetId();
		EatingItems eatingItems = selfChar.GetEatingItems();
		ItemKey itemKey = (ItemKey)eatingItems.ItemKeys[eatingItems.IndexOfWug(WugType)];
		LifeRecordCollection lifeRecordCollection = DomainManager.LifeRecord.GetLifeRecordCollection();
		int currDate = DomainManager.World.GetCurrDate();
		Location location = selfChar.GetLocation();
		if (AgreeToRequest)
		{
			if (ItemUsed.ItemType != 8)
			{
				throw new Exception($"Invalid item type {ItemUsed} to kill wug {WugType} for {selfChar.GetId()}");
			}
			ItemBase baseItem = DomainManager.Item.GetBaseItem(ItemUsed);
			targetChar.RemoveInventoryItem(context, ItemUsed, 1, deleteItem: false);
			selfChar.AddEatingItem(context, ItemUsed);
			selfChar.ChangeHappiness(context, baseItem.GetHappinessChange());
			DomainManager.Character.ChangeFavorabilityOptionalMonthlyEvolution(context, selfChar, targetChar, baseItem.GetFavorabilityChange() * 5);
			lifeRecordCollection.AddRequestKillWugSucceed(id, currDate, id2, location, ItemUsed.ItemType, ItemUsed.TemplateId, itemKey.ItemType, itemKey.TemplateId);
			SecretInformationCollection secretInformationCollection = DomainManager.Information.GetSecretInformationCollection();
			int dataOffset = secretInformationCollection.AddAcceptRequestKillWug(id2, id);
			DomainManager.Information.AddSecretInformationMetaData(context, dataOffset);
		}
		else
		{
			selfChar.ChangeHappiness(context, -3);
			DomainManager.Character.ChangeFavorabilityOptionalMonthlyEvolution(context, selfChar, targetChar, -6000);
			lifeRecordCollection.AddRequestKillWugFail(id, currDate, id2, location, ItemUsed.ItemType, ItemUsed.TemplateId, itemKey.ItemType, itemKey.TemplateId);
			SecretInformationCollection secretInformationCollection2 = DomainManager.Information.GetSecretInformationCollection();
			int dataOffset2 = secretInformationCollection2.AddRefuseRequestKillWug(id2, id);
			DomainManager.Information.AddSecretInformationMetaData(context, dataOffset2);
		}
	}
}
