using System;
using GameData.Common;
using GameData.Domains.Item;
using GameData.Domains.LifeRecord;
using GameData.Domains.Map;

namespace GameData.Domains.Character.Ai.GeneralAction.WealthDemand;

public class PurchaseItemAction : IGeneralAction
{
	public int MoneyCost;

	public TemplateKey PurchasedItem;

	public int ItemAmount;

	public sbyte ActionEnergyType => 1;

	public bool CheckValid(Character selfChar, Character targetChar)
	{
		return selfChar.IsInRegularSettlementRange() && selfChar.GetResource(6) >= MoneyCost;
	}

	public void ApplyInitialChangesForTaiwu(DataContext context, Character selfChar, Character targetChar)
	{
		throw new Exception("Current action requires no targetChar.");
	}

	public void ApplyChanges(DataContext context, Character selfChar, Character targetChar)
	{
		LifeRecordCollection lifeRecordCollection = DomainManager.LifeRecord.GetLifeRecordCollection();
		int id = selfChar.GetId();
		int currDate = DomainManager.World.GetCurrDate();
		Location validLocation = selfChar.GetValidLocation();
		Location location = DomainManager.Map.GetBelongSettlementBlock(validLocation).GetLocation();
		selfChar.ChangeResource(context, 6, -MoneyCost);
		lifeRecordCollection.AddPurchaseItem1(id, currDate, location, PurchasedItem.ItemType, PurchasedItem.TemplateId);
		if (targetChar != null)
		{
			targetChar.CreateInventoryItem(context, PurchasedItem.ItemType, PurchasedItem.TemplateId, ItemAmount);
			lifeRecordCollection.AddGiveItem(id, currDate, targetChar.GetId(), location, PurchasedItem.ItemType, PurchasedItem.TemplateId);
		}
		else
		{
			selfChar.CreateInventoryItem(context, PurchasedItem.ItemType, PurchasedItem.TemplateId, ItemAmount);
		}
	}
}
