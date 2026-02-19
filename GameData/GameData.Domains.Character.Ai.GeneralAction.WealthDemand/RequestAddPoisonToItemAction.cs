using System.Linq;
using Config;
using GameData.Common;
using GameData.Domains.Information.Collection;
using GameData.Domains.Item;
using GameData.Domains.LifeRecord;
using GameData.Domains.Map;
using GameData.Domains.World.MonthlyEvent;
using GameData.Utilities;

namespace GameData.Domains.Character.Ai.GeneralAction.WealthDemand;

public class RequestAddPoisonToItemAction : IGeneralAction
{
	public ItemKey TargetItem;

	public bool AgreeToRequest;

	public ItemKey PoisonUsed;

	public sbyte ActionEnergyType => 1;

	public bool CheckValid(Character selfChar, Character targetChar)
	{
		return selfChar.GetEquipment().Contains(TargetItem) && targetChar.GetInventory().Items.ContainsKey(PoisonUsed);
	}

	public void ApplyInitialChangesForTaiwu(DataContext context, Character selfChar, Character targetChar)
	{
		int id = selfChar.GetId();
		int id2 = targetChar.GetId();
		Location location = targetChar.GetLocation();
		MonthlyEventCollection monthlyEventCollection = DomainManager.World.GetMonthlyEventCollection();
		monthlyEventCollection.AddRequestAddPoisonToItem(id, location, id2, (ulong)TargetItem, (ulong)PoisonUsed);
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
			ItemBase baseItem = DomainManager.Item.GetBaseItem(TargetItem);
			MedicineItem medicineItem = Config.Medicine.Instance[PoisonUsed.TemplateId];
			Tester.Assert(medicineItem.EffectType == EMedicineEffectType.ApplyPoison);
			var (itemBase, flag) = DomainManager.Item.SetAttachedPoisons(context, baseItem, PoisonUsed.TemplateId, add: true);
			targetChar.RemoveInventoryItem(context, PoisonUsed, 1, deleteItem: true);
			if (flag)
			{
				ItemKey[] equipment = selfChar.GetEquipment();
				for (int i = 0; i < equipment.Length; i++)
				{
					if (equipment[i].Equals(TargetItem))
					{
						equipment[i] = itemBase.GetItemKey();
						selfChar.SetEquipment(equipment, context);
						break;
					}
				}
			}
			int baseDelta = baseItem.GetFavorabilityChange() * 5;
			DomainManager.Character.ChangeFavorabilityOptionalMonthlyEvolution(context, selfChar, targetChar, baseDelta);
			selfChar.ChangeHappiness(context, DomainManager.Item.GetBaseItem(TargetItem).GetHappinessChange());
			lifeRecordCollection.AddRequestAddPoisonToItemSucceed(id, currDate, id2, location, TargetItem.ItemType, TargetItem.TemplateId);
			SecretInformationCollection secretInformationCollection = DomainManager.Information.GetSecretInformationCollection();
			int dataOffset = secretInformationCollection.AddAcceptRequestAddPoisonToItem(id2, id, (ulong)TargetItem, (ulong)PoisonUsed);
			int num = DomainManager.Information.AddSecretInformationMetaData(context, dataOffset);
		}
		else
		{
			DomainManager.Character.ChangeFavorabilityOptionalMonthlyEvolution(context, selfChar, targetChar, -3000);
			selfChar.ChangeHappiness(context, -3);
			lifeRecordCollection.AddRequestAddPoisonToItemFail(id, currDate, id2, location, TargetItem.ItemType, TargetItem.TemplateId);
			SecretInformationCollection secretInformationCollection2 = DomainManager.Information.GetSecretInformationCollection();
			int dataOffset2 = secretInformationCollection2.AddRefuseRequestAddPoisonToItem(id2, id, (ulong)TargetItem, (ulong)PoisonUsed);
			int num2 = DomainManager.Information.AddSecretInformationMetaData(context, dataOffset2);
		}
	}
}
