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
	// Token: 0x02000871 RID: 2161
	public class StealItemAction : IGeneralAction
	{
		// Token: 0x170004AA RID: 1194
		// (get) Token: 0x06007776 RID: 30582 RVA: 0x0045D068 File Offset: 0x0045B268
		public sbyte ActionEnergyType
		{
			get
			{
				return 1;
			}
		}

		// Token: 0x06007777 RID: 30583 RVA: 0x0045D06C File Offset: 0x0045B26C
		public bool CheckValid(Character selfChar, Character targetChar)
		{
			return targetChar.GetInventory().Items.ContainsKey(this.TargetItem) || targetChar.GetEquipment().Contains(this.TargetItem);
		}

		// Token: 0x06007778 RID: 30584 RVA: 0x0045D0AC File Offset: 0x0045B2AC
		public void ApplyInitialChangesForTaiwu(DataContext context, Character selfChar, Character targetChar)
		{
			int selfCharId = selfChar.GetId();
			int targetCharId = targetChar.GetId();
			Location location = selfChar.GetLocation();
			MonthlyEventCollection monthlyEventCollection = DomainManager.World.GetMonthlyEventCollection();
			MonthlyNotificationCollection monthlyNotificationCollection = DomainManager.World.GetMonthlyNotificationCollection();
			bool flag = this.Phase <= 3;
			if (flag)
			{
				monthlyNotificationCollection.AddStealItemFailure(selfCharId, location, targetCharId, this.TargetItem.ItemType, this.TargetItem.TemplateId);
				this.ApplyChanges(context, selfChar, targetChar);
			}
			else
			{
				monthlyNotificationCollection.AddStealItemSuccess(selfCharId, location, targetCharId, this.TargetItem.ItemType, this.TargetItem.TemplateId);
				bool flag2 = this.Phase == 4;
				if (flag2)
				{
					bool flag3 = ItemTemplateHelper.GetItemSubType(this.TargetItem.ItemType, this.TargetItem.TemplateId) == 1202;
					if (flag3)
					{
						monthlyEventCollection.AddStealLegendaryBookGotCaught(selfCharId, location, targetCharId, (ulong)this.TargetItem, (int)this.Phase);
					}
					else
					{
						monthlyEventCollection.AddStealItemButBeCaught(selfCharId, location, targetCharId, (ulong)this.TargetItem);
					}
				}
				else
				{
					bool flag4 = ItemTemplateHelper.GetItemSubType(this.TargetItem.ItemType, this.TargetItem.TemplateId) == 1202;
					if (flag4)
					{
						monthlyEventCollection.AddStealLegendaryBookAndEscape(selfCharId, location, targetCharId, (ulong)this.TargetItem, (int)this.Phase);
					}
					else
					{
						monthlyEventCollection.AddStealItemAndEscape(selfCharId, location, targetCharId, (ulong)this.TargetItem);
					}
					this.ApplyChanges(context, selfChar, targetChar);
				}
				CharacterDomain.AddLockMovementCharSet(selfCharId);
			}
		}

		// Token: 0x06007779 RID: 30585 RVA: 0x0045D228 File Offset: 0x0045B428
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
				selfChar.ChangeCurrMainAttribute(context, 1, (int)(-(int)GlobalConfig.Instance.HarmfulActionCost));
			}
			bool flag2 = this.Phase >= 4;
			if (flag2)
			{
				SecretInformationCollection secretInformationCollection = DomainManager.Information.GetSecretInformationCollection();
				int secretInfoOffset = secretInformationCollection.AddStealItem(selfCharId, targetCharId, (ulong)this.TargetItem);
				int secretInfoId = DomainManager.Information.AddSecretInformationMetaData(context, secretInfoOffset, true);
			}
			switch (this.Phase)
			{
			case 0:
				lifeRecordCollection.AddStealItemFail1(selfCharId, currDate, targetCharId, location, this.TargetItem.ItemType, this.TargetItem.TemplateId);
				break;
			case 1:
				lifeRecordCollection.AddStealItemFail2(selfCharId, currDate, targetCharId, location, this.TargetItem.ItemType, this.TargetItem.TemplateId);
				break;
			case 2:
				lifeRecordCollection.AddStealItemFail3(selfCharId, currDate, targetCharId, location, this.TargetItem.ItemType, this.TargetItem.TemplateId);
				break;
			case 3:
				lifeRecordCollection.AddStealItemFail4(selfCharId, currDate, targetCharId, location, this.TargetItem.ItemType, this.TargetItem.TemplateId);
				break;
			case 4:
			{
				lifeRecordCollection.AddStealItemSucceed(selfCharId, currDate, targetCharId, location, this.TargetItem.ItemType, this.TargetItem.TemplateId);
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
						lifeRecordCollection.AddStealItemSucceedAndBeatenUp(selfCharId, currDate, targetCharId, location, this.TargetItem.ItemType, this.TargetItem.TemplateId);
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
				lifeRecordCollection.AddStealItemSucceedAndEscaped(selfCharId, currDate, targetCharId, location, this.TargetItem.ItemType, this.TargetItem.TemplateId);
				break;
			}
			}
		}

		// Token: 0x040020E4 RID: 8420
		public ItemKey TargetItem;

		// Token: 0x040020E5 RID: 8421
		public int Amount;

		// Token: 0x040020E6 RID: 8422
		public sbyte Phase;

		// Token: 0x040020E7 RID: 8423
		public ItemKey[] PoisonsToAdd;
	}
}
