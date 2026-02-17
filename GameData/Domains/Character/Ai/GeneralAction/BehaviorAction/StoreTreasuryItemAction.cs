using System;
using GameData.Common;
using GameData.Domains.Item;

namespace GameData.Domains.Character.Ai.GeneralAction.BehaviorAction
{
	// Token: 0x020008A6 RID: 2214
	public class StoreTreasuryItemAction : IGeneralAction
	{
		// Token: 0x170004DF RID: 1247
		// (get) Token: 0x0600787F RID: 30847 RVA: 0x00464B0B File Offset: 0x00462D0B
		public sbyte ActionEnergyType
		{
			get
			{
				return 3;
			}
		}

		// Token: 0x06007880 RID: 30848 RVA: 0x00464B10 File Offset: 0x00462D10
		public bool CheckValid(Character selfChar, Character targetChar)
		{
			int amount;
			return selfChar.GetInventory().Items.TryGetValue(this.TargetItem, out amount) && amount >= this.Amount;
		}

		// Token: 0x06007881 RID: 30849 RVA: 0x00464B4B File Offset: 0x00462D4B
		public void ApplyInitialChangesForTaiwu(DataContext context, Character selfChar, Character targetChar)
		{
			throw new Exception("Current action requires no targetChar.");
		}

		// Token: 0x06007882 RID: 30850 RVA: 0x00464B58 File Offset: 0x00462D58
		public void ApplyChanges(DataContext context, Character selfChar, Character targetChar)
		{
			selfChar.RemoveInventoryItem(context, this.TargetItem, this.Amount, false, false);
			DomainManager.Organization.StoreItemInTreasury(context, selfChar.GetOrganizationInfo().SettlementId, selfChar, this.TargetItem, this.Amount, -1);
		}

		// Token: 0x0400217B RID: 8571
		public ItemKey TargetItem;

		// Token: 0x0400217C RID: 8572
		public int Amount;
	}
}
