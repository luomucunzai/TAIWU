using System;
using GameData.Common;
using GameData.Domains.Item;
using GameData.Domains.LifeRecord;

namespace GameData.Domains.Character.Ai.GeneralAction.WealthDemand
{
	// Token: 0x02000875 RID: 2165
	public class PurchaseBuybackItemAction : IGeneralAction
	{
		// Token: 0x170004AE RID: 1198
		// (get) Token: 0x0600778A RID: 30602 RVA: 0x0045E05C File Offset: 0x0045C25C
		public sbyte ActionEnergyType
		{
			get
			{
				return 1;
			}
		}

		// Token: 0x0600778B RID: 30603 RVA: 0x0045E060 File Offset: 0x0045C260
		public bool CheckValid(Character selfChar, Character targetChar)
		{
			return true;
		}

		// Token: 0x0600778C RID: 30604 RVA: 0x0045E073 File Offset: 0x0045C273
		public void ApplyInitialChangesForTaiwu(DataContext context, Character selfChar, Character targetChar)
		{
		}

		// Token: 0x0600778D RID: 30605 RVA: 0x0045E078 File Offset: 0x0045C278
		public void ApplyChanges(DataContext context, Character selfChar, Character targetChar)
		{
			bool flag = !DomainManager.Merchant.RemoveBuyBackItem(this.TargetItem);
			if (!flag)
			{
				selfChar.AddInventoryItem(context, this.TargetItem, 1, false);
				LifeRecordCollection lifeRecordCollection = DomainManager.LifeRecord.GetLifeRecordCollection();
				int currDate = DomainManager.World.GetCurrDate();
				lifeRecordCollection.AddAcquisitionDiscard(selfChar.GetId(), currDate, this.TargetItem.ItemType, this.TargetItem.TemplateId);
			}
		}

		// Token: 0x040020F4 RID: 8436
		public ItemKey TargetItem;
	}
}
