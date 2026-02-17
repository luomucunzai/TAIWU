using System;
using GameData.Common;
using GameData.Domains.Organization;

namespace GameData.Domains.Character.Ai.GeneralAction.WealthDemand
{
	// Token: 0x02000879 RID: 2169
	public class TakeTreasuryResourceAction : IGeneralAction
	{
		// Token: 0x170004B2 RID: 1202
		// (get) Token: 0x0600779E RID: 30622 RVA: 0x0045E77C File Offset: 0x0045C97C
		public sbyte ActionEnergyType
		{
			get
			{
				return 1;
			}
		}

		// Token: 0x0600779F RID: 30623 RVA: 0x0045E780 File Offset: 0x0045C980
		public unsafe bool CheckValid(Character selfChar, Character targetChar)
		{
			OrganizationInfo orgInfo = selfChar.GetOrganizationInfo();
			short settlementId = orgInfo.SettlementId;
			bool flag = settlementId < 0;
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				SettlementTreasury treasury = DomainManager.Organization.GetTreasury(orgInfo);
				int worth = DomainManager.Organization.CalcResourceContribution(orgInfo.OrgTemplateId, this.ResourceType, this.Amount);
				result = (*treasury.Resources[(int)this.ResourceType] >= this.Amount && treasury.GetMemberContribution(selfChar) >= worth);
			}
			return result;
		}

		// Token: 0x060077A0 RID: 30624 RVA: 0x0045E804 File Offset: 0x0045CA04
		public void ApplyInitialChangesForTaiwu(DataContext context, Character selfChar, Character targetChar)
		{
			throw new Exception("Current action requires no targetChar.");
		}

		// Token: 0x060077A1 RID: 30625 RVA: 0x0045E811 File Offset: 0x0045CA11
		public void ApplyChanges(DataContext context, Character selfChar, Character targetChar)
		{
			DomainManager.Organization.TakeResourceFromTreasury(context, selfChar.GetOrganizationInfo().SettlementId, selfChar, this.ResourceType, this.Amount, -1);
			selfChar.ChangeResource(context, this.ResourceType, this.Amount);
		}

		// Token: 0x04002101 RID: 8449
		public sbyte ResourceType;

		// Token: 0x04002102 RID: 8450
		public int Amount;
	}
}
