using System;
using GameData.Common;
using GameData.Domains.Information.Collection;
using GameData.Domains.Item;
using GameData.Domains.LifeRecord;
using GameData.Domains.Map;
using GameData.Domains.World.Notification;

namespace GameData.Domains.Character.Ai.GeneralAction.WealthDemand;

public class RobGraveItemAction : IGeneralAction
{
	public ItemKey TargetItem;

	public int Amount;

	public bool Succeed;

	public int TargetGraveId;

	public sbyte ActionEnergyType => 1;

	public bool CheckValid(Character selfChar, Character targetChar)
	{
		Grave element;
		return DomainManager.Character.TryGetElement_Graves(TargetGraveId, out element) && element.GetInventory().Items.ContainsKey(TargetItem);
	}

	public void ApplyInitialChangesForTaiwu(DataContext context, Character selfChar, Character targetChar)
	{
		throw new Exception("cannot be digging the current Taiwu's grave when he or she is still alive.");
	}

	public void ApplyChanges(DataContext context, Character selfChar, Character targetChar)
	{
		int id = selfChar.GetId();
		MonthlyNotificationCollection monthlyNotificationCollection = DomainManager.World.GetMonthlyNotificationCollection();
		LifeRecordCollection lifeRecordCollection = DomainManager.LifeRecord.GetLifeRecordCollection();
		Location location = selfChar.GetLocation();
		int currDate = DomainManager.World.GetCurrDate();
		int taiwuCharId = DomainManager.Taiwu.GetTaiwuCharId();
		if (id != taiwuCharId)
		{
			selfChar.ChangeCurrMainAttribute(context, 3, -GlobalConfig.Instance.HarmfulActionCost);
		}
		if (DomainManager.Character.IsTaiwuPeople(TargetGraveId))
		{
			monthlyNotificationCollection.AddDigItem(id, location, TargetGraveId, TargetItem.ItemType, TargetItem.TemplateId);
		}
		if (Succeed)
		{
			Grave element_Graves = DomainManager.Character.GetElement_Graves(TargetGraveId);
			element_Graves.RemoveInventoryItem(context, TargetItem, Amount);
			selfChar.AddInventoryItem(context, TargetItem, Amount);
			lifeRecordCollection.AddRobItemFromGraveSucceed(id, currDate, TargetGraveId, location, TargetItem.ItemType, TargetItem.TemplateId);
			SecretInformationCollection secretInformationCollection = DomainManager.Information.GetSecretInformationCollection();
			int dataOffset = secretInformationCollection.AddRobGraveItem(id, TargetGraveId, (ulong)TargetItem);
			DomainManager.Information.AddSecretInformationMetaData(context, dataOffset);
		}
		else
		{
			lifeRecordCollection.AddRobItemFromGraveFail(id, currDate, TargetGraveId, location, TargetItem.ItemType, TargetItem.TemplateId);
		}
	}
}
