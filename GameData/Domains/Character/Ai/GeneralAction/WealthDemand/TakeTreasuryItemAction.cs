using System;
using GameData.Common;
using GameData.Domains.Item;
using GameData.Domains.Organization;

namespace GameData.Domains.Character.Ai.GeneralAction.WealthDemand
{
	// Token: 0x0200086E RID: 2158
	public class TakeTreasuryItemAction : IGeneralAction
	{
		// Token: 0x170004A7 RID: 1191
		// (get) Token: 0x06007767 RID: 30567 RVA: 0x0045CBF4 File Offset: 0x0045ADF4
		public sbyte ActionEnergyType
		{
			get
			{
				return 1;
			}
		}

		// Token: 0x06007768 RID: 30568 RVA: 0x0045CBF8 File Offset: 0x0045ADF8
		public bool CheckValid(Character selfChar, Character targetChar)
		{
			OrganizationInfo orgInfo = selfChar.GetOrganizationInfo();
			bool flag = orgInfo.SettlementId < 0;
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				SettlementTreasury treasury = DomainManager.Organization.GetTreasury(orgInfo);
				int amount;
				bool flag2 = !treasury.Inventory.Items.TryGetValue(this.TargetItem, out amount) || amount < this.Amount;
				if (flag2)
				{
					result = false;
				}
				else
				{
					bool flag3 = orgInfo.OrgTemplateId == 16;
					if (flag3)
					{
						result = true;
					}
					else
					{
						int contribution = treasury.GetMemberContribution(selfChar);
						Settlement settlement = DomainManager.Organization.GetSettlement(orgInfo.SettlementId);
						int worth = DomainManager.Organization.CalcItemContribution(settlement, this.TargetItem, this.Amount);
						result = (contribution >= worth);
					}
				}
			}
			return result;
		}

		// Token: 0x06007769 RID: 30569 RVA: 0x0045CCB8 File Offset: 0x0045AEB8
		public void ApplyInitialChangesForTaiwu(DataContext context, Character selfChar, Character targetChar)
		{
			throw new Exception("Current action requires no targetChar.");
		}

		// Token: 0x0600776A RID: 30570 RVA: 0x0045CCC5 File Offset: 0x0045AEC5
		public void ApplyChanges(DataContext context, Character selfChar, Character targetChar)
		{
			DomainManager.Organization.TakeItemFromTreasury(context, selfChar.GetOrganizationInfo().SettlementId, selfChar, this.TargetItem, this.Amount, false);
			selfChar.AddInventoryItem(context, this.TargetItem, this.Amount, false);
		}

		// Token: 0x040020DB RID: 8411
		public ItemKey TargetItem;

		// Token: 0x040020DC RID: 8412
		public int Amount;
	}
}
