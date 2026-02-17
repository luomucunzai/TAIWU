using System;
using GameData.Common;
using GameData.Domains.Item;
using GameData.Domains.LifeRecord;
using GameData.Domains.Map;

namespace GameData.Domains.Character.Ai.GeneralAction.BehaviorAction
{
	// Token: 0x020008A7 RID: 2215
	public class SellItemAction : IGeneralAction
	{
		// Token: 0x170004E0 RID: 1248
		// (get) Token: 0x06007884 RID: 30852 RVA: 0x00464B9F File Offset: 0x00462D9F
		public sbyte ActionEnergyType
		{
			get
			{
				return 3;
			}
		}

		// Token: 0x06007885 RID: 30853 RVA: 0x00464BA4 File Offset: 0x00462DA4
		public bool CheckValid(Character selfChar, Character targetChar)
		{
			int amount;
			return selfChar.IsInRegularSettlementRange() && selfChar.GetInventory().Items.TryGetValue(this.TargetItem, out amount) && amount >= this.Amount;
		}

		// Token: 0x06007886 RID: 30854 RVA: 0x00464BE7 File Offset: 0x00462DE7
		public void ApplyInitialChangesForTaiwu(DataContext context, Character selfChar, Character targetChar)
		{
			throw new Exception("Current action requires no targetChar.");
		}

		// Token: 0x06007887 RID: 30855 RVA: 0x00464BF4 File Offset: 0x00462DF4
		public void ApplyChanges(DataContext context, Character selfChar, Character targetChar)
		{
			int price = DomainManager.Item.GetValue(this.TargetItem);
			int moneyGain = this.Amount * price / 5;
			selfChar.RemoveInventoryItem(context, this.TargetItem, this.Amount, true, false);
			selfChar.ChangeResource(context, 6, moneyGain);
			LifeRecordCollection lifeRecordCollection = DomainManager.LifeRecord.GetLifeRecordCollection();
			int currDate = DomainManager.World.GetCurrDate();
			Location location = selfChar.GetValidLocation();
			Location settlementLocation = DomainManager.Map.GetBelongSettlementBlock(location).GetLocation();
			lifeRecordCollection.AddSellItem1(selfChar.GetId(), currDate, settlementLocation, this.TargetItem.ItemType, this.TargetItem.TemplateId);
		}

		// Token: 0x0400217D RID: 8573
		public ItemKey TargetItem;

		// Token: 0x0400217E RID: 8574
		public int Amount;
	}
}
