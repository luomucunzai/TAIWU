using GameData.Common;
using GameData.Domains.Information.Collection;
using GameData.Domains.Item;
using GameData.Domains.LifeRecord;
using GameData.Domains.Map;
using GameData.Domains.World.MonthlyEvent;

namespace GameData.Domains.Character.Ai.GeneralAction.WealthDemand;

public class RequestItemAction : IGeneralAction
{
	public ItemKey TargetItem;

	public int Amount;

	public bool AgreeToRequest;

	public ItemKey[] PoisonsToAdd;

	public sbyte ActionEnergyType => 1;

	public bool CheckValid(Character selfChar, Character targetChar)
	{
		return targetChar.GetInventory().Items.ContainsKey(TargetItem);
	}

	public void ApplyInitialChangesForTaiwu(DataContext context, Character selfChar, Character targetChar)
	{
		int id = selfChar.GetId();
		int id2 = targetChar.GetId();
		Location location = targetChar.GetLocation();
		MonthlyEventCollection monthlyEventCollection = DomainManager.World.GetMonthlyEventCollection();
		monthlyEventCollection.AddRequestItem(id, location, id2, (ulong)TargetItem, Amount);
		CharacterDomain.AddLockMovementCharSet(id);
	}

	public void ApplyChanges(DataContext context, Character selfChar, Character targetChar)
	{
		int id = selfChar.GetId();
		int id2 = targetChar.GetId();
		LifeRecordCollection lifeRecordCollection = DomainManager.LifeRecord.GetLifeRecordCollection();
		Location location = selfChar.GetLocation();
		int currDate = DomainManager.World.GetCurrDate();
		if (AgreeToRequest)
		{
			if (PoisonsToAdd != null)
			{
				(TargetItem, _) = targetChar.AttachPoisonsToInventoryItem(context, TargetItem, PoisonsToAdd);
			}
			DomainManager.Character.TransferInventoryItem(context, targetChar, selfChar, TargetItem, Amount);
			ItemBase baseItem = DomainManager.Item.GetBaseItem(TargetItem);
			int baseDelta = baseItem.GetFavorabilityChange() * 2;
			DomainManager.Character.ChangeFavorabilityOptionalMonthlyEvolution(context, selfChar, targetChar, baseDelta);
			selfChar.ChangeHappiness(context, baseItem.GetHappinessChange());
			lifeRecordCollection.AddRequestItemSucceed(id, currDate, id2, location, TargetItem.ItemType, TargetItem.TemplateId);
			SecretInformationCollection secretInformationCollection = DomainManager.Information.GetSecretInformationCollection();
			int dataOffset = secretInformationCollection.AddAcceptRequestItem(id, id2, (ulong)TargetItem);
			int num = DomainManager.Information.AddSecretInformationMetaData(context, dataOffset);
		}
		else
		{
			DomainManager.Character.ChangeFavorabilityOptionalMonthlyEvolution(context, selfChar, targetChar, -3000);
			selfChar.ChangeHappiness(context, -3);
			lifeRecordCollection.AddRequestItemFail(id, currDate, id2, location, TargetItem.ItemType, TargetItem.TemplateId);
			SecretInformationCollection secretInformationCollection2 = DomainManager.Information.GetSecretInformationCollection();
			int dataOffset2 = secretInformationCollection2.AddRefuseRequestItem(id, id2, (ulong)TargetItem);
			int num2 = DomainManager.Information.AddSecretInformationMetaData(context, dataOffset2);
		}
	}
}
