using System;
using System.Linq;
using Config;
using GameData.Common;
using GameData.Domains.Information.Collection;
using GameData.Domains.Item;
using GameData.Domains.LifeRecord;
using GameData.Domains.Map;
using GameData.Domains.World.MonthlyEvent;
using GameData.Utilities;

namespace GameData.Domains.Character.Ai.GeneralAction.WealthDemand
{
	// Token: 0x02000877 RID: 2167
	public class RequestAddPoisonToItemAction : IGeneralAction
	{
		// Token: 0x170004B0 RID: 1200
		// (get) Token: 0x06007794 RID: 30612 RVA: 0x0045E209 File Offset: 0x0045C409
		public sbyte ActionEnergyType
		{
			get
			{
				return 1;
			}
		}

		// Token: 0x06007795 RID: 30613 RVA: 0x0045E20C File Offset: 0x0045C40C
		public bool CheckValid(Character selfChar, Character targetChar)
		{
			return selfChar.GetEquipment().Contains(this.TargetItem) && targetChar.GetInventory().Items.ContainsKey(this.PoisonUsed);
		}

		// Token: 0x06007796 RID: 30614 RVA: 0x0045E24C File Offset: 0x0045C44C
		public void ApplyInitialChangesForTaiwu(DataContext context, Character selfChar, Character targetChar)
		{
			int selfCharId = selfChar.GetId();
			int targetCharId = targetChar.GetId();
			Location location = targetChar.GetLocation();
			MonthlyEventCollection monthlyEventCollection = DomainManager.World.GetMonthlyEventCollection();
			monthlyEventCollection.AddRequestAddPoisonToItem(selfCharId, location, targetCharId, (ulong)this.TargetItem, (ulong)this.PoisonUsed);
			CharacterDomain.AddLockMovementCharSet(selfCharId);
		}

		// Token: 0x06007797 RID: 30615 RVA: 0x0045E2A4 File Offset: 0x0045C4A4
		public void ApplyChanges(DataContext context, Character selfChar, Character targetChar)
		{
			int selfCharId = selfChar.GetId();
			int targetCharId = targetChar.GetId();
			LifeRecordCollection lifeRecordCollection = DomainManager.LifeRecord.GetLifeRecordCollection();
			Location location = selfChar.GetLocation();
			int currDate = DomainManager.World.GetCurrDate();
			bool agreeToRequest = this.AgreeToRequest;
			if (agreeToRequest)
			{
				ItemBase itemToAddPoisonOn = DomainManager.Item.GetBaseItem(this.TargetItem);
				MedicineItem poisonConfig = Config.Medicine.Instance[this.PoisonUsed.TemplateId];
				Tester.Assert(poisonConfig.EffectType == EMedicineEffectType.ApplyPoison, "");
				ValueTuple<ItemBase, bool> valueTuple = DomainManager.Item.SetAttachedPoisons(context, itemToAddPoisonOn, this.PoisonUsed.TemplateId, true, null);
				ItemBase newItemObj = valueTuple.Item1;
				bool keyChanged = valueTuple.Item2;
				targetChar.RemoveInventoryItem(context, this.PoisonUsed, 1, true, false);
				bool flag = keyChanged;
				if (flag)
				{
					ItemKey[] equipment = selfChar.GetEquipment();
					for (int i = 0; i < equipment.Length; i++)
					{
						bool flag2 = !equipment[i].Equals(this.TargetItem);
						if (!flag2)
						{
							equipment[i] = newItemObj.GetItemKey();
							selfChar.SetEquipment(equipment, context);
							break;
						}
					}
				}
				int favorabilityChange = itemToAddPoisonOn.GetFavorabilityChange() * 5;
				DomainManager.Character.ChangeFavorabilityOptionalMonthlyEvolution(context, selfChar, targetChar, favorabilityChange);
				selfChar.ChangeHappiness(context, (int)DomainManager.Item.GetBaseItem(this.TargetItem).GetHappinessChange());
				lifeRecordCollection.AddRequestAddPoisonToItemSucceed(selfCharId, currDate, targetCharId, location, this.TargetItem.ItemType, this.TargetItem.TemplateId);
				SecretInformationCollection secretInformationCollection = DomainManager.Information.GetSecretInformationCollection();
				int secretInfoOffset = secretInformationCollection.AddAcceptRequestAddPoisonToItem(targetCharId, selfCharId, (ulong)this.TargetItem, (ulong)this.PoisonUsed);
				int secretInfoId = DomainManager.Information.AddSecretInformationMetaData(context, secretInfoOffset, true);
			}
			else
			{
				DomainManager.Character.ChangeFavorabilityOptionalMonthlyEvolution(context, selfChar, targetChar, -3000);
				selfChar.ChangeHappiness(context, -3);
				lifeRecordCollection.AddRequestAddPoisonToItemFail(selfCharId, currDate, targetCharId, location, this.TargetItem.ItemType, this.TargetItem.TemplateId);
				SecretInformationCollection secretInformationCollection2 = DomainManager.Information.GetSecretInformationCollection();
				int secretInfoOffset2 = secretInformationCollection2.AddRefuseRequestAddPoisonToItem(targetCharId, selfCharId, (ulong)this.TargetItem, (ulong)this.PoisonUsed);
				int secretInfoId2 = DomainManager.Information.AddSecretInformationMetaData(context, secretInfoOffset2, true);
			}
		}

		// Token: 0x040020F8 RID: 8440
		public ItemKey TargetItem;

		// Token: 0x040020F9 RID: 8441
		public bool AgreeToRequest;

		// Token: 0x040020FA RID: 8442
		public ItemKey PoisonUsed;
	}
}
