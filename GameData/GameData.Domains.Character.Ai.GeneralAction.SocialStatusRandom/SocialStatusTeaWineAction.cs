using GameData.Common;
using GameData.Domains.Information.Collection;
using GameData.Domains.Item;
using GameData.Domains.LifeRecord;
using GameData.Domains.Map;
using GameData.Domains.World.MonthlyEvent;

namespace GameData.Domains.Character.Ai.GeneralAction.SocialStatusRandom;

public class SocialStatusTeaWineAction : IGeneralAction
{
	public bool Succeed;

	public ItemKey SelfTeaWineItem;

	public ItemKey TargetTeaWineItem;

	public sbyte ActionEnergyType => 4;

	public bool CheckValid(Character selfChar, Character targetChar)
	{
		Inventory inventory = selfChar.GetInventory();
		return inventory.Items.ContainsKey(SelfTeaWineItem) && inventory.Items.ContainsKey(TargetTeaWineItem);
	}

	public void ApplyInitialChangesForTaiwu(DataContext context, Character selfChar, Character targetChar)
	{
		MonthlyEventCollection monthlyEventCollection = DomainManager.World.GetMonthlyEventCollection();
		int id = selfChar.GetId();
		Location location = targetChar.GetLocation();
		monthlyEventCollection.AddAdviseTeaWine(id, location, targetChar.GetId(), (ulong)SelfTeaWineItem, (ulong)TargetTeaWineItem);
		CharacterDomain.AddLockMovementCharSet(id);
	}

	public void ApplyChanges(DataContext context, Character selfChar, Character targetChar)
	{
		int id = selfChar.GetId();
		int id2 = targetChar.GetId();
		sbyte behaviorType = selfChar.GetBehaviorType();
		int currDate = DomainManager.World.GetCurrDate();
		Location location = selfChar.GetLocation();
		LifeRecordCollection lifeRecordCollection = DomainManager.LifeRecord.GetLifeRecordCollection();
		SecretInformationCollection secretInformationCollection = DomainManager.Information.GetSecretInformationCollection();
		if (Succeed)
		{
			selfChar.RemoveInventoryItem(context, SelfTeaWineItem, 1, deleteItem: false);
			if (SelfTeaWineItem.Id != TargetTeaWineItem.Id)
			{
				selfChar.RemoveInventoryItem(context, TargetTeaWineItem, 1, deleteItem: false);
			}
			selfChar.AddEatingItem(context, SelfTeaWineItem);
			targetChar.AddEatingItem(context, TargetTeaWineItem);
			short begSucceedFavorabilityChange = AiHelper.GeneralActionConstants.GetBegSucceedFavorabilityChange(context.Random, behaviorType);
			DomainManager.Character.ChangeFavorabilityOptionalMonthlyEvolution(context, selfChar, targetChar, begSucceedFavorabilityChange);
			lifeRecordCollection.AddInviteToDrinkSucceed(id, currDate, id2, location, SelfTeaWineItem.ItemType, SelfTeaWineItem.TemplateId);
			int dataOffset = secretInformationCollection.AddAcceptRequestDrinking(id2, id);
			int num = DomainManager.Information.AddSecretInformationMetaData(context, dataOffset);
		}
		else
		{
			short begFailFavorabilityChange = AiHelper.GeneralActionConstants.GetBegFailFavorabilityChange(context.Random, behaviorType);
			DomainManager.Character.ChangeFavorabilityOptionalMonthlyEvolution(context, selfChar, targetChar, begFailFavorabilityChange);
			lifeRecordCollection.AddInviteToDrinkFail(id, currDate, id2, location, SelfTeaWineItem.ItemType, SelfTeaWineItem.TemplateId);
			int dataOffset2 = secretInformationCollection.AddRefuseRequestDrinking(id2, id);
			int num2 = DomainManager.Information.AddSecretInformationMetaData(context, dataOffset2);
		}
	}
}
