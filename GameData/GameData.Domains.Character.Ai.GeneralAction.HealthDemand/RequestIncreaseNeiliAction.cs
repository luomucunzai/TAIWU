using System;
using Config;
using GameData.Common;
using GameData.Domains.Information.Collection;
using GameData.Domains.Item;
using GameData.Domains.LifeRecord;
using GameData.Domains.Map;
using GameData.Domains.World.MonthlyEvent;

namespace GameData.Domains.Character.Ai.GeneralAction.HealthDemand;

public class RequestIncreaseNeiliAction : IGeneralAction
{
	public ItemKey ItemUsed;

	public bool AgreeToRequest;

	public sbyte ActionEnergyType => 0;

	public bool CheckValid(Character selfChar, Character targetChar)
	{
		return selfChar.GetCurrNeili() < selfChar.GetMaxNeili() && targetChar.GetInventory().Items.ContainsKey(ItemUsed);
	}

	public void ApplyInitialChangesForTaiwu(DataContext context, Character selfChar, Character targetChar)
	{
		MonthlyEventCollection monthlyEventCollection = DomainManager.World.GetMonthlyEventCollection();
		int id = selfChar.GetId();
		Location location = targetChar.GetLocation();
		monthlyEventCollection.AddRequestNeili(id, location, targetChar.GetId(), (ulong)ItemUsed);
		CharacterDomain.AddLockMovementCharSet(id);
	}

	public void ApplyChanges(DataContext context, Character selfChar, Character targetChar)
	{
		int id = selfChar.GetId();
		int id2 = targetChar.GetId();
		LifeRecordCollection lifeRecordCollection = DomainManager.LifeRecord.GetLifeRecordCollection();
		int currDate = DomainManager.World.GetCurrDate();
		Location location = selfChar.GetLocation();
		if (AgreeToRequest)
		{
			if (ItemUsed.ItemType != 12)
			{
				throw new Exception($"Invalid item type {ItemUsed} to restore neili for {selfChar.GetId()}");
			}
			MiscItem miscItem = Config.Misc.Instance[ItemUsed.TemplateId];
			ItemBase baseItem = DomainManager.Item.GetBaseItem(ItemUsed);
			targetChar.RemoveInventoryItem(context, ItemUsed, 1, deleteItem: false);
			selfChar.ChangeCurrNeili(context, miscItem.Neili);
			selfChar.ChangeHappiness(context, baseItem.GetHappinessChange());
			DomainManager.Character.ChangeFavorabilityOptionalMonthlyEvolution(context, selfChar, targetChar, baseItem.GetFavorabilityChange() * 5);
			lifeRecordCollection.AddRequestNeiliSucceed(id, currDate, id2, location, ItemUsed.ItemType, ItemUsed.TemplateId);
			SecretInformationCollection secretInformationCollection = DomainManager.Information.GetSecretInformationCollection();
			int dataOffset = secretInformationCollection.AddAcceptRequestIncreaseNeili(id2, id);
			DomainManager.Information.AddSecretInformationMetaData(context, dataOffset);
		}
		else
		{
			selfChar.ChangeHappiness(context, -3);
			DomainManager.Character.ChangeFavorabilityOptionalMonthlyEvolution(context, selfChar, targetChar, -6000);
			lifeRecordCollection.AddRequestNeiliFail(id, currDate, id2, location, ItemUsed.ItemType, ItemUsed.TemplateId);
			SecretInformationCollection secretInformationCollection2 = DomainManager.Information.GetSecretInformationCollection();
			int dataOffset2 = secretInformationCollection2.AddRefuseRequestIncreaseNeili(id2, id);
			DomainManager.Information.AddSecretInformationMetaData(context, dataOffset2);
		}
	}
}
