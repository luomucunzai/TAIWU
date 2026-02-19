using System.Linq;
using GameData.Common;
using GameData.Domains.Combat;
using GameData.Domains.Information.Collection;
using GameData.Domains.Item;
using GameData.Domains.LifeRecord;
using GameData.Domains.Map;
using GameData.Domains.World.MonthlyEvent;
using GameData.Domains.World.Notification;
using GameData.Utilities;

namespace GameData.Domains.Character.Ai.GeneralAction.WealthDemand;

public class StealItemAction : IGeneralAction
{
	public ItemKey TargetItem;

	public int Amount;

	public sbyte Phase;

	public ItemKey[] PoisonsToAdd;

	public sbyte ActionEnergyType => 1;

	public bool CheckValid(Character selfChar, Character targetChar)
	{
		return targetChar.GetInventory().Items.ContainsKey(TargetItem) || targetChar.GetEquipment().Contains(TargetItem);
	}

	public void ApplyInitialChangesForTaiwu(DataContext context, Character selfChar, Character targetChar)
	{
		int id = selfChar.GetId();
		int id2 = targetChar.GetId();
		Location location = selfChar.GetLocation();
		MonthlyEventCollection monthlyEventCollection = DomainManager.World.GetMonthlyEventCollection();
		MonthlyNotificationCollection monthlyNotificationCollection = DomainManager.World.GetMonthlyNotificationCollection();
		if (Phase <= 3)
		{
			monthlyNotificationCollection.AddStealItemFailure(id, location, id2, TargetItem.ItemType, TargetItem.TemplateId);
			ApplyChanges(context, selfChar, targetChar);
			return;
		}
		monthlyNotificationCollection.AddStealItemSuccess(id, location, id2, TargetItem.ItemType, TargetItem.TemplateId);
		if (Phase == 4)
		{
			if (ItemTemplateHelper.GetItemSubType(TargetItem.ItemType, TargetItem.TemplateId) == 1202)
			{
				monthlyEventCollection.AddStealLegendaryBookGotCaught(id, location, id2, (ulong)TargetItem, Phase);
			}
			else
			{
				monthlyEventCollection.AddStealItemButBeCaught(id, location, id2, (ulong)TargetItem);
			}
		}
		else
		{
			if (ItemTemplateHelper.GetItemSubType(TargetItem.ItemType, TargetItem.TemplateId) == 1202)
			{
				monthlyEventCollection.AddStealLegendaryBookAndEscape(id, location, id2, (ulong)TargetItem, Phase);
			}
			else
			{
				monthlyEventCollection.AddStealItemAndEscape(id, location, id2, (ulong)TargetItem);
			}
			ApplyChanges(context, selfChar, targetChar);
		}
		CharacterDomain.AddLockMovementCharSet(id);
	}

	public void ApplyChanges(DataContext context, Character selfChar, Character targetChar)
	{
		int id = selfChar.GetId();
		int id2 = targetChar.GetId();
		LifeRecordCollection lifeRecordCollection = DomainManager.LifeRecord.GetLifeRecordCollection();
		int currDate = DomainManager.World.GetCurrDate();
		Location location = selfChar.GetLocation();
		int taiwuCharId = DomainManager.Taiwu.GetTaiwuCharId();
		if (id != taiwuCharId)
		{
			selfChar.ChangeCurrMainAttribute(context, 1, -GlobalConfig.Instance.HarmfulActionCost);
		}
		if (Phase >= 4)
		{
			SecretInformationCollection secretInformationCollection = DomainManager.Information.GetSecretInformationCollection();
			int dataOffset = secretInformationCollection.AddStealItem(id, id2, (ulong)TargetItem);
			int num = DomainManager.Information.AddSecretInformationMetaData(context, dataOffset);
		}
		switch (Phase)
		{
		case 0:
			lifeRecordCollection.AddStealItemFail1(id, currDate, id2, location, TargetItem.ItemType, TargetItem.TemplateId);
			break;
		case 1:
			lifeRecordCollection.AddStealItemFail2(id, currDate, id2, location, TargetItem.ItemType, TargetItem.TemplateId);
			break;
		case 2:
			lifeRecordCollection.AddStealItemFail3(id, currDate, id2, location, TargetItem.ItemType, TargetItem.TemplateId);
			break;
		case 3:
			lifeRecordCollection.AddStealItemFail4(id, currDate, id2, location, TargetItem.ItemType, TargetItem.TemplateId);
			break;
		case 4:
		{
			lifeRecordCollection.AddStealItemSucceed(id, currDate, id2, location, TargetItem.ItemType, TargetItem.TemplateId);
			if (id2 == taiwuCharId)
			{
				break;
			}
			AiHelper.NpcCombatResultType npcCombatResultType = DomainManager.Character.SimulateCharacterCombat(context, targetChar, selfChar, CombatType.Beat);
			if ((uint)(npcCombatResultType - 2) <= 1u)
			{
				int num3 = targetChar.GetEquipment().IndexOf(TargetItem);
				if (num3 >= 0)
				{
					targetChar.ChangeEquipment(context, (sbyte)num3, -1, ItemKey.Invalid);
				}
				DomainManager.Character.TransferInventoryItem(context, targetChar, selfChar, TargetItem, Amount);
				targetChar.ChangeHappiness(context, DomainManager.Item.GetBaseItem(TargetItem).GetHappinessChange());
				DomainManager.Character.SimulateCharacterCombatResult(context, selfChar, targetChar, -40, -20, 0);
			}
			else
			{
				lifeRecordCollection.AddStealItemSucceedAndBeatenUp(id, currDate, id2, location, TargetItem.ItemType, TargetItem.TemplateId);
				DomainManager.Character.SimulateCharacterCombatResult(context, targetChar, selfChar, -40, -20, 0);
			}
			break;
		}
		default:
		{
			int num2 = targetChar.GetEquipment().IndexOf(TargetItem);
			if (num2 >= 0)
			{
				targetChar.ChangeEquipment(context, (sbyte)num2, -1, ItemKey.Invalid);
			}
			if (PoisonsToAdd != null)
			{
				(TargetItem, _) = targetChar.AttachPoisonsToInventoryItem(context, TargetItem, PoisonsToAdd);
			}
			DomainManager.Character.TransferInventoryItem(context, targetChar, selfChar, TargetItem, Amount);
			targetChar.ChangeHappiness(context, DomainManager.Item.GetBaseItem(TargetItem).GetHappinessChange());
			lifeRecordCollection.AddStealItemSucceedAndEscaped(id, currDate, id2, location, TargetItem.ItemType, TargetItem.TemplateId);
			break;
		}
		}
	}
}
