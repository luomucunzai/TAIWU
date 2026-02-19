using GameData.Common;
using GameData.Domains.Information.Collection;
using GameData.Domains.Item;
using GameData.Domains.LifeRecord;
using GameData.Domains.Map;
using GameData.Domains.World.MonthlyEvent;

namespace GameData.Domains.Character.Ai.GeneralAction.WealthDemand;

public class RequestRepairItemAction : IGeneralAction
{
	public ItemKey TargetItem;

	public bool AgreeToRequest;

	public ItemKey ToolUsed;

	public sbyte ResourceType;

	public int ResourceAmount;

	public short ToolDurabilityCost;

	public sbyte ActionEnergyType => 1;

	public bool CheckValid(Character selfChar, Character targetChar)
	{
		return selfChar.GetInventory().Items.ContainsKey(TargetItem) && targetChar.GetInventory().Items.ContainsKey(ToolUsed) && targetChar.GetResource(ResourceType) >= ResourceAmount;
	}

	public void ApplyInitialChangesForTaiwu(DataContext context, Character selfChar, Character targetChar)
	{
		int id = selfChar.GetId();
		int id2 = targetChar.GetId();
		Location location = selfChar.GetLocation();
		MonthlyEventCollection monthlyEventCollection = DomainManager.World.GetMonthlyEventCollection();
		monthlyEventCollection.AddRequestRepairItem(id, location, id2, (ulong)TargetItem, (ulong)ToolUsed, ResourceAmount, ResourceType);
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
			CraftTool element_CraftTools = DomainManager.Item.GetElement_CraftTools(ToolUsed.Id);
			ItemBase baseItem = DomainManager.Item.GetBaseItem(TargetItem);
			ItemBase.OfflineRepairItem(element_CraftTools, baseItem, baseItem.GetMaxDurability(), ToolDurabilityCost);
			baseItem.SetCurrDurability(baseItem.GetCurrDurability(), context);
			element_CraftTools.SetCurrDurability(element_CraftTools.GetCurrDurability(), context);
			targetChar.ChangeResource(context, ResourceType, -ResourceAmount);
			int favorabilityChange = baseItem.GetFavorabilityChange();
			DomainManager.Character.ChangeFavorabilityOptionalMonthlyEvolution(context, selfChar, targetChar, favorabilityChange);
			selfChar.ChangeHappiness(context, DomainManager.Item.GetBaseItem(TargetItem).GetHappinessChange());
			lifeRecordCollection.AddRequestRepairItemSucceed(id, currDate, id2, location, TargetItem.ItemType, TargetItem.TemplateId);
			SecretInformationCollection secretInformationCollection = DomainManager.Information.GetSecretInformationCollection();
			int dataOffset = secretInformationCollection.AddAcceptRequestRepairItem(id2, id, (ulong)TargetItem);
			int num = DomainManager.Information.AddSecretInformationMetaData(context, dataOffset);
		}
		else
		{
			DomainManager.Character.ChangeFavorabilityOptionalMonthlyEvolution(context, selfChar, targetChar, -3000);
			selfChar.ChangeHappiness(context, -3);
			lifeRecordCollection.AddRequestRepairItemFail(id, currDate, id2, location, TargetItem.ItemType, TargetItem.TemplateId);
			SecretInformationCollection secretInformationCollection2 = DomainManager.Information.GetSecretInformationCollection();
			int dataOffset2 = secretInformationCollection2.AddRefuseRequestRepairItem(id2, id, (ulong)TargetItem);
			int num2 = DomainManager.Information.AddSecretInformationMetaData(context, dataOffset2);
		}
	}
}
