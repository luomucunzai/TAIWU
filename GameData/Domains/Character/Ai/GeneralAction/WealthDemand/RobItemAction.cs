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
	// Token: 0x02000873 RID: 2163
	public class RobItemAction : IGeneralAction
	{
		// Token: 0x170004AC RID: 1196
		// (get) Token: 0x06007780 RID: 30592 RVA: 0x0045DA02 File Offset: 0x0045BC02
		public sbyte ActionEnergyType
		{
			get
			{
				return 1;
			}
		}

		// Token: 0x06007781 RID: 30593 RVA: 0x0045DA08 File Offset: 0x0045BC08
		public bool CheckValid(Character selfChar, Character targetChar)
		{
			return targetChar.GetInventory().Items.ContainsKey(this.TargetItem) || targetChar.GetEquipment().Contains(this.TargetItem);
		}

		// Token: 0x06007782 RID: 30594 RVA: 0x0045DA48 File Offset: 0x0045BC48
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
				monthlyNotificationCollection.AddRobItemFailure(selfCharId, location, targetCharId, this.TargetItem.ItemType, this.TargetItem.TemplateId);
				this.ApplyChanges(context, selfChar, targetChar);
			}
			else
			{
				bool flag2 = ItemTemplateHelper.GetItemSubType(this.TargetItem.ItemType, this.TargetItem.TemplateId) == 1202;
				if (flag2)
				{
					monthlyEventCollection.AddRobLegendaryBook(selfCharId, location, targetCharId, (ulong)this.TargetItem, (int)this.Phase);
				}
				else
				{
					monthlyEventCollection.AddRobItem(selfCharId, location, targetCharId, (ulong)this.TargetItem);
				}
				CharacterDomain.AddLockMovementCharSet(selfCharId);
			}
		}

		// Token: 0x06007783 RID: 30595 RVA: 0x0045DB28 File Offset: 0x0045BD28
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
				selfChar.ChangeCurrMainAttribute(context, 0, (int)(-(int)GlobalConfig.Instance.HarmfulActionCost));
			}
			bool flag2 = this.Phase >= 4;
			if (flag2)
			{
				SecretInformationCollection secretInformationCollection = DomainManager.Information.GetSecretInformationCollection();
				int secretInfoOffset = secretInformationCollection.AddRobItem(selfCharId, targetCharId, (ulong)this.TargetItem);
				int secretInfoId = DomainManager.Information.AddSecretInformationMetaData(context, secretInfoOffset, true);
			}
			switch (this.Phase)
			{
			case 0:
				lifeRecordCollection.AddRobItemFail1(selfCharId, currDate, targetCharId, location, this.TargetItem.ItemType, this.TargetItem.TemplateId);
				break;
			case 1:
				lifeRecordCollection.AddRobItemFail2(selfCharId, currDate, targetCharId, location, this.TargetItem.ItemType, this.TargetItem.TemplateId);
				break;
			case 2:
				lifeRecordCollection.AddRobItemFail3(selfCharId, currDate, targetCharId, location, this.TargetItem.ItemType, this.TargetItem.TemplateId);
				break;
			case 3:
				lifeRecordCollection.AddRobItemFail4(selfCharId, currDate, targetCharId, location, this.TargetItem.ItemType, this.TargetItem.TemplateId);
				break;
			case 4:
			{
				lifeRecordCollection.AddRobItemSucceed(selfCharId, currDate, targetCharId, location, this.TargetItem.ItemType, this.TargetItem.TemplateId);
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
						lifeRecordCollection.AddRobItemFailAndBeatenUp(selfCharId, currDate, targetCharId, location, this.TargetItem.ItemType, this.TargetItem.TemplateId);
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
				lifeRecordCollection.AddRobItemSucceedAndEscaped(selfCharId, currDate, targetCharId, location, this.TargetItem.ItemType, this.TargetItem.TemplateId);
				break;
			}
			}
		}

		// Token: 0x040020EC RID: 8428
		public ItemKey TargetItem;

		// Token: 0x040020ED RID: 8429
		public int Amount;

		// Token: 0x040020EE RID: 8430
		public sbyte Phase;

		// Token: 0x040020EF RID: 8431
		public ItemKey[] PoisonsToAdd;
	}
}
