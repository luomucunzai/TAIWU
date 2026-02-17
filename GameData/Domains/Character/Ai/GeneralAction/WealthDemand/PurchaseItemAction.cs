using System;
using GameData.Common;
using GameData.Domains.Item;
using GameData.Domains.LifeRecord;
using GameData.Domains.Map;

namespace GameData.Domains.Character.Ai.GeneralAction.WealthDemand
{
	// Token: 0x0200086F RID: 2159
	public class PurchaseItemAction : IGeneralAction
	{
		// Token: 0x170004A8 RID: 1192
		// (get) Token: 0x0600776C RID: 30572 RVA: 0x0045CD0B File Offset: 0x0045AF0B
		public sbyte ActionEnergyType
		{
			get
			{
				return 1;
			}
		}

		// Token: 0x0600776D RID: 30573 RVA: 0x0045CD10 File Offset: 0x0045AF10
		public bool CheckValid(Character selfChar, Character targetChar)
		{
			return selfChar.IsInRegularSettlementRange() && selfChar.GetResource(6) >= this.MoneyCost;
		}

		// Token: 0x0600776E RID: 30574 RVA: 0x0045CD3F File Offset: 0x0045AF3F
		public void ApplyInitialChangesForTaiwu(DataContext context, Character selfChar, Character targetChar)
		{
			throw new Exception("Current action requires no targetChar.");
		}

		// Token: 0x0600776F RID: 30575 RVA: 0x0045CD4C File Offset: 0x0045AF4C
		public void ApplyChanges(DataContext context, Character selfChar, Character targetChar)
		{
			LifeRecordCollection lifeRecordCollection = DomainManager.LifeRecord.GetLifeRecordCollection();
			int selfCharId = selfChar.GetId();
			int currDate = DomainManager.World.GetCurrDate();
			Location location = selfChar.GetValidLocation();
			Location settlementLocation = DomainManager.Map.GetBelongSettlementBlock(location).GetLocation();
			selfChar.ChangeResource(context, 6, -this.MoneyCost);
			lifeRecordCollection.AddPurchaseItem1(selfCharId, currDate, settlementLocation, this.PurchasedItem.ItemType, this.PurchasedItem.TemplateId);
			bool flag = targetChar != null;
			if (flag)
			{
				targetChar.CreateInventoryItem(context, this.PurchasedItem.ItemType, this.PurchasedItem.TemplateId, this.ItemAmount);
				lifeRecordCollection.AddGiveItem(selfCharId, currDate, targetChar.GetId(), settlementLocation, this.PurchasedItem.ItemType, this.PurchasedItem.TemplateId);
			}
			else
			{
				selfChar.CreateInventoryItem(context, this.PurchasedItem.ItemType, this.PurchasedItem.TemplateId, this.ItemAmount);
			}
		}

		// Token: 0x040020DD RID: 8413
		public int MoneyCost;

		// Token: 0x040020DE RID: 8414
		public TemplateKey PurchasedItem;

		// Token: 0x040020DF RID: 8415
		public int ItemAmount;
	}
}
