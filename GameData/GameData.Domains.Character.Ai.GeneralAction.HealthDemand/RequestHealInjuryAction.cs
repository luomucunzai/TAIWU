using System;
using Config;
using GameData.Common;
using GameData.Domains.Information.Collection;
using GameData.Domains.Item;
using GameData.Domains.LifeRecord;
using GameData.Domains.Map;
using GameData.Domains.World.MonthlyEvent;

namespace GameData.Domains.Character.Ai.GeneralAction.HealthDemand;

public class RequestHealInjuryAction : IGeneralAction
{
	public ItemKey ItemUsed;

	public bool IsInnerInjury;

	public bool AgreeToRequest;

	public sbyte ActionEnergyType => 0;

	public bool CheckValid(Character selfChar, Character targetChar)
	{
		if (!selfChar.GetInjuries().HasAnyInjury(IsInnerInjury))
		{
			return false;
		}
		if (!targetChar.GetInventory().Items.ContainsKey(ItemUsed))
		{
			return false;
		}
		MedicineItem medicineItem = Config.Medicine.Instance[ItemUsed.TemplateId];
		if (medicineItem.Duration == 0)
		{
			return medicineItem.RequiredMainAttributeType < 0 || selfChar.GetCurrMainAttribute(medicineItem.RequiredMainAttributeType) >= medicineItem.RequiredMainAttributeValue;
		}
		return selfChar.GetEatingItems().GetAvailableEatingSlotsCount(selfChar.GetCurrMaxEatingSlotsCount()) > 0;
	}

	public void ApplyInitialChangesForTaiwu(DataContext context, Character selfChar, Character targetChar)
	{
		MonthlyEventCollection monthlyEventCollection = DomainManager.World.GetMonthlyEventCollection();
		int id = selfChar.GetId();
		int id2 = targetChar.GetId();
		Location location = targetChar.GetLocation();
		if (IsInnerInjury)
		{
			monthlyEventCollection.AddRequestHealInnerInjuryByItem(id, location, id2, (ulong)ItemUsed, -1);
		}
		else
		{
			monthlyEventCollection.AddRequestHealOuterInjuryByItem(id, location, id2, (ulong)ItemUsed, -1);
		}
		CharacterDomain.AddLockMovementCharSet(selfChar.GetId());
	}

	public void ApplyChanges(DataContext context, Character selfChar, Character targetChar)
	{
		int id = selfChar.GetId();
		int id2 = targetChar.GetId();
		SecretInformationCollection secretInformationCollection = DomainManager.Information.GetSecretInformationCollection();
		LifeRecordCollection lifeRecordCollection = DomainManager.LifeRecord.GetLifeRecordCollection();
		int currDate = DomainManager.World.GetCurrDate();
		Location location = selfChar.GetLocation();
		if (AgreeToRequest)
		{
			if (ItemUsed.ItemType != 8)
			{
				throw new Exception($"Invalid item type {ItemUsed} to heal injury for {selfChar.GetId()}");
			}
			ItemBase baseItem = DomainManager.Item.GetBaseItem(ItemUsed);
			targetChar.RemoveInventoryItem(context, ItemUsed, 1, deleteItem: false);
			MedicineItem medicineItem = Config.Medicine.Instance[ItemUsed.TemplateId];
			if (medicineItem.Duration == 0)
			{
				selfChar.ApplyTopicalMedicine(context, ItemUsed);
				DomainManager.Item.RemoveItem(context, ItemUsed);
			}
			else
			{
				selfChar.AddEatingItem(context, ItemUsed);
			}
			selfChar.ChangeHappiness(context, baseItem.GetHappinessChange());
			DomainManager.Character.ChangeFavorabilityOptionalMonthlyEvolution(context, selfChar, targetChar, baseItem.GetFavorabilityChange() * 5);
			if (IsInnerInjury)
			{
				lifeRecordCollection.AddRequestHealInnerInjurySucceed(id, currDate, id2, location, ItemUsed.ItemType, ItemUsed.TemplateId);
			}
			else
			{
				lifeRecordCollection.AddRequestHealOuterInjurySucceed(id, currDate, id2, location, ItemUsed.ItemType, ItemUsed.TemplateId);
			}
			int dataOffset = secretInformationCollection.AddAcceptRequestHealInjury(id2, id);
			int num = DomainManager.Information.AddSecretInformationMetaData(context, dataOffset);
		}
		else
		{
			selfChar.ChangeHappiness(context, -3);
			DomainManager.Character.ChangeFavorabilityOptionalMonthlyEvolution(context, selfChar, targetChar, -6000);
			if (IsInnerInjury)
			{
				lifeRecordCollection.AddRequestHealInnerInjuryFail(id, currDate, id2, location, ItemUsed.ItemType, ItemUsed.TemplateId);
			}
			else
			{
				lifeRecordCollection.AddRequestHealOuterInjuryFail(id, currDate, id2, location, ItemUsed.ItemType, ItemUsed.TemplateId);
			}
			int dataOffset2 = secretInformationCollection.AddRefuseRequestHealInjury(id2, id);
			int num2 = DomainManager.Information.AddSecretInformationMetaData(context, dataOffset2);
		}
	}
}
