using GameData.Common;
using GameData.Domains.Information.Collection;
using GameData.Domains.Item;
using GameData.Domains.LifeRecord;
using GameData.Domains.Map;
using GameData.Domains.World.MonthlyEvent;

namespace GameData.Domains.Character.Ai.GeneralAction.SocialStatusRandom;

public class SocialStatusRepairAction : IGeneralAction
{
	public sbyte ResourceType;

	public int Amount;

	public ItemKey ToolUsed;

	public ItemKey RepairedItem;

	public sbyte ActionEnergyType => 4;

	public bool CheckValid(Character selfChar, Character targetChar)
	{
		return selfChar.GetResource(ResourceType) >= Amount && selfChar.GetInventory().Items.ContainsKey(ToolUsed) && DomainManager.Item.GetBaseItem(ToolUsed).GetCurrDurability() > 0 && targetChar.GetInventory().Items.ContainsKey(RepairedItem);
	}

	public void ApplyInitialChangesForTaiwu(DataContext context, Character selfChar, Character targetChar)
	{
		MonthlyEventCollection monthlyEventCollection = DomainManager.World.GetMonthlyEventCollection();
		int id = selfChar.GetId();
		Location location = targetChar.GetLocation();
		monthlyEventCollection.AddAdviseRepairItem(id, location, targetChar.GetId(), (ulong)RepairedItem, (ulong)ToolUsed, ResourceType, Amount);
		CharacterDomain.AddLockMovementCharSet(selfChar.GetId());
	}

	public void ApplyChanges(DataContext context, Character selfChar, Character targetChar)
	{
		int id = selfChar.GetId();
		int id2 = targetChar.GetId();
		int currDate = DomainManager.World.GetCurrDate();
		Location location = selfChar.GetLocation();
		LifeRecordCollection lifeRecordCollection = DomainManager.LifeRecord.GetLifeRecordCollection();
		selfChar.ChangeResource(context, ResourceType, Amount);
		ItemBase baseItem = DomainManager.Item.GetBaseItem(RepairedItem);
		baseItem.SetCurrDurability(baseItem.GetMaxDurability(), context);
		DomainManager.Character.ChangeFavorabilityOptionalMonthlyEvolution(context, targetChar, selfChar, 3000);
		lifeRecordCollection.AddRepairItemSucceed(id, currDate, id2, location);
		SecretInformationCollection secretInformationCollection = DomainManager.Information.GetSecretInformationCollection();
		int dataOffset = secretInformationCollection.AddRepairItem(id, id2, (ulong)RepairedItem);
		int num = DomainManager.Information.AddSecretInformationMetaData(context, dataOffset);
	}
}
