using System;
using GameData.Common;
using GameData.Domains.Item;
using GameData.Domains.LifeRecord;
using GameData.Domains.Map;
using GameData.Domains.World.Notification;

namespace GameData.Domains.Character.Ai.GeneralAction.BehaviorAction
{
	// Token: 0x020008A8 RID: 2216
	public class GiveItemAction : IGeneralAction
	{
		// Token: 0x170004E1 RID: 1249
		// (get) Token: 0x06007889 RID: 30857 RVA: 0x00464C9E File Offset: 0x00462E9E
		public sbyte ActionEnergyType
		{
			get
			{
				return 3;
			}
		}

		// Token: 0x0600788A RID: 30858 RVA: 0x00464CA4 File Offset: 0x00462EA4
		public bool CheckValid(Character selfChar, Character targetChar)
		{
			int amount;
			return selfChar.GetInventory().Items.TryGetValue(this.TargetItem, out amount) && amount >= this.Amount;
		}

		// Token: 0x0600788B RID: 30859 RVA: 0x00464CE0 File Offset: 0x00462EE0
		public void ApplyInitialChangesForTaiwu(DataContext context, Character selfChar, Character targetChar)
		{
			int selfCharId = selfChar.GetId();
			int targetCharId = targetChar.GetId();
			Location location = selfChar.GetLocation();
			MonthlyNotificationCollection monthlyNotifications = DomainManager.World.GetMonthlyNotificationCollection();
			monthlyNotifications.AddGivePresentItem(selfCharId, location, this.TargetItem.ItemType, this.TargetItem.TemplateId, targetCharId);
			this.ApplyChanges(context, selfChar, targetChar);
		}

		// Token: 0x0600788C RID: 30860 RVA: 0x00464D38 File Offset: 0x00462F38
		public void ApplyChanges(DataContext context, Character selfChar, Character targetChar)
		{
			int selfCharId = selfChar.GetId();
			int targetCharId = targetChar.GetId();
			int currDate = DomainManager.World.GetCurrDate();
			Location location = selfChar.GetLocation();
			LifeRecordCollection lifeRecordCollection = DomainManager.LifeRecord.GetLifeRecordCollection();
			bool refusePoisonousItem = this.RefusePoisonousItem;
			if (refusePoisonousItem)
			{
				lifeRecordCollection.AddRefusePoisonousGift(selfCharId, currDate, targetCharId, location, this.TargetItem.ItemType, this.TargetItem.TemplateId);
			}
			else
			{
				lifeRecordCollection.AddGiveItem(selfCharId, currDate, targetCharId, location, this.TargetItem.ItemType, this.TargetItem.TemplateId);
				DomainManager.Character.TransferInventoryItem(context, selfChar, targetChar, this.TargetItem, this.Amount);
				ItemBase baseItem = DomainManager.Item.GetBaseItem(this.TargetItem);
				int favorChange = baseItem.GetFavorabilityChange();
				DomainManager.Character.ChangeFavorabilityOptionalMonthlyEvolution(context, targetChar, selfChar, favorChange);
				targetChar.ChangeHappiness(context, (int)baseItem.GetHappinessChange());
			}
		}

		// Token: 0x0400217F RID: 8575
		public ItemKey TargetItem;

		// Token: 0x04002180 RID: 8576
		public int Amount;

		// Token: 0x04002181 RID: 8577
		public bool RefusePoisonousItem;
	}
}
