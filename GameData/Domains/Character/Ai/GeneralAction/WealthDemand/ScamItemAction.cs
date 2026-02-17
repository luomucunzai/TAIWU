using System;
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

namespace GameData.Domains.Character.Ai.GeneralAction.WealthDemand
{
	// Token: 0x02000872 RID: 2162
	public class ScamItemAction : IGeneralAction
	{
		// Token: 0x170004AB RID: 1195
		// (get) Token: 0x0600777B RID: 30587 RVA: 0x0045D583 File Offset: 0x0045B783
		public sbyte ActionEnergyType
		{
			get
			{
				return 1;
			}
		}

		// Token: 0x0600777C RID: 30588 RVA: 0x0045D588 File Offset: 0x0045B788
		public bool CheckValid(Character selfChar, Character targetChar)
		{
			return targetChar.GetInventory().Items.ContainsKey(this.TargetItem) || targetChar.GetEquipment().Contains(this.TargetItem);
		}

		// Token: 0x0600777D RID: 30589 RVA: 0x0045D5C8 File Offset: 0x0045B7C8
		public void ApplyInitialChangesForTaiwu(DataContext context, Character selfChar, Character targetChar)
		{
			int selfCharId = selfChar.GetId();
			int targetCharId = targetChar.GetId();
			Location location = selfChar.GetLocation();
			MonthlyEventCollection monthlyEventCollection = DomainManager.World.GetMonthlyEventCollection();
			MonthlyNotificationCollection monthlyNotificationCollection = DomainManager.World.GetMonthlyNotificationCollection();
			bool flag = this.Phase <= 2;
			if (flag)
			{
				monthlyNotificationCollection.AddCheatItemFailure(selfCharId, location, targetCharId, this.TargetItem.ItemType, this.TargetItem.TemplateId);
				this.ApplyChanges(context, selfChar, targetChar);
			}
			else
			{
				bool flag2 = ItemTemplateHelper.GetItemSubType(this.TargetItem.ItemType, this.TargetItem.TemplateId) == 1202;
				if (flag2)
				{
					monthlyEventCollection.AddScamLegendaryBook(selfCharId, location, targetCharId, (ulong)this.TargetItem, (int)this.Phase);
				}
				else
				{
					monthlyEventCollection.AddScamItem(selfCharId, location, targetCharId, (ulong)this.TargetItem);
				}
				CharacterDomain.AddLockMovementCharSet(selfCharId);
			}
		}

		// Token: 0x0600777E RID: 30590 RVA: 0x0045D6A8 File Offset: 0x0045B8A8
		public void ApplyChanges(DataContext context, Character selfChar, Character targetChar)
		{
			int selfCharId = selfChar.GetId();
			int targetCharId = targetChar.GetId();
			LifeRecordCollection lifeRecordCollection = DomainManager.LifeRecord.GetLifeRecordCollection();
			int currDate = DomainManager.World.GetCurrDate();
			Location location = selfChar.GetLocation();
			int taiwuCharId = DomainManager.Taiwu.GetTaiwuCharId();
			bool flag = selfCharId != taiwuCharId;
			if (flag)
			{
				selfChar.ChangeCurrMainAttribute(context, 2, (int)(-(int)GlobalConfig.Instance.HarmfulActionCost));
			}
			bool flag2 = this.Phase >= 4;
			if (flag2)
			{
				SecretInformationCollection secretInformationCollection = DomainManager.Information.GetSecretInformationCollection();
				int secretInfoOffset = secretInformationCollection.AddScamItem(selfCharId, targetCharId, (ulong)this.TargetItem);
				int secretInfoId = DomainManager.Information.AddSecretInformationMetaData(context, secretInfoOffset, true);
			}
			switch (this.Phase)
			{
			case 0:
				lifeRecordCollection.AddScamItemFail1(selfCharId, currDate, targetCharId, location, this.TargetItem.ItemType, this.TargetItem.TemplateId);
				break;
			case 1:
				lifeRecordCollection.AddScamItemFail2(selfCharId, currDate, targetCharId, location, this.TargetItem.ItemType, this.TargetItem.TemplateId);
				break;
			case 2:
				lifeRecordCollection.AddScamItemFail3(selfCharId, currDate, targetCharId, location, this.TargetItem.ItemType, this.TargetItem.TemplateId);
				break;
			case 3:
				lifeRecordCollection.AddScamItemFail4(selfCharId, currDate, targetCharId, location, this.TargetItem.ItemType, this.TargetItem.TemplateId);
				break;
			case 4:
			{
				lifeRecordCollection.AddScamItemSucceed(selfCharId, currDate, targetCharId, location, this.TargetItem.ItemType, this.TargetItem.TemplateId);
				bool flag3 = targetCharId == taiwuCharId;
				if (!flag3)
				{
					AiHelper.NpcCombatResultType combatResultType = DomainManager.Character.SimulateCharacterCombat(context, targetChar, selfChar, CombatType.Beat, true, 1);
					bool flag4 = combatResultType - AiHelper.NpcCombatResultType.MajorDefeat <= 1;
					bool flag5 = flag4;
					if (flag5)
					{
						int slotIndex = targetChar.GetEquipment().IndexOf(this.TargetItem);
						bool flag6 = slotIndex >= 0;
						if (flag6)
						{
							targetChar.ChangeEquipment(context, (sbyte)slotIndex, -1, ItemKey.Invalid);
						}
						DomainManager.Character.TransferInventoryItem(context, targetChar, selfChar, this.TargetItem, this.Amount);
						targetChar.ChangeHappiness(context, (int)DomainManager.Item.GetBaseItem(this.TargetItem).GetHappinessChange());
						DomainManager.Character.SimulateCharacterCombatResult(context, selfChar, targetChar, -40, -20, 0);
					}
					else
					{
						lifeRecordCollection.AddScamItemFailAndBeatenUp(selfCharId, currDate, targetCharId, location, this.TargetItem.ItemType, this.TargetItem.TemplateId);
						DomainManager.Character.SimulateCharacterCombatResult(context, targetChar, selfChar, -40, -20, 0);
					}
				}
				break;
			}
			default:
			{
				int slotIndex2 = targetChar.GetEquipment().IndexOf(this.TargetItem);
				bool flag7 = slotIndex2 >= 0;
				if (flag7)
				{
					targetChar.ChangeEquipment(context, (sbyte)slotIndex2, -1, ItemKey.Invalid);
				}
				bool flag8 = this.PoisonsToAdd != null;
				if (flag8)
				{
					ValueTuple<ItemKey, bool> valueTuple = targetChar.AttachPoisonsToInventoryItem(context, this.TargetItem, this.PoisonsToAdd);
					this.TargetItem = valueTuple.Item1;
				}
				DomainManager.Character.TransferInventoryItem(context, targetChar, selfChar, this.TargetItem, this.Amount);
				targetChar.ChangeHappiness(context, (int)DomainManager.Item.GetBaseItem(this.TargetItem).GetHappinessChange());
				lifeRecordCollection.AddScamItemSucceedAndEscaped(selfCharId, currDate, targetCharId, location, this.TargetItem.ItemType, this.TargetItem.TemplateId);
				break;
			}
			}
		}

		// Token: 0x040020E8 RID: 8424
		public ItemKey TargetItem;

		// Token: 0x040020E9 RID: 8425
		public int Amount;

		// Token: 0x040020EA RID: 8426
		public sbyte Phase;

		// Token: 0x040020EB RID: 8427
		public ItemKey[] PoisonsToAdd;
	}
}
