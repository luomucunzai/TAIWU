using System;
using GameData.Common;

namespace GameData.Domains.Character.Ai.GeneralAction.BehaviorAction
{
	// Token: 0x020008A9 RID: 2217
	public class StoreTreasuryResourceAction : IGeneralAction
	{
		// Token: 0x170004E2 RID: 1250
		// (get) Token: 0x0600788E RID: 30862 RVA: 0x00464E25 File Offset: 0x00463025
		public sbyte ActionEnergyType
		{
			get
			{
				return 3;
			}
		}

		// Token: 0x0600788F RID: 30863 RVA: 0x00464E28 File Offset: 0x00463028
		public bool CheckValid(Character selfChar, Character targetChar)
		{
			return selfChar.GetResource(this.ResourceType) >= this.Amount;
		}

		// Token: 0x06007890 RID: 30864 RVA: 0x00464E51 File Offset: 0x00463051
		public void ApplyInitialChangesForTaiwu(DataContext context, Character selfChar, Character targetChar)
		{
			throw new Exception("Current action requires no targetChar.");
		}

		// Token: 0x06007891 RID: 30865 RVA: 0x00464E5E File Offset: 0x0046305E
		public void ApplyChanges(DataContext context, Character selfChar, Character targetChar)
		{
			DomainManager.Organization.StoreResourceInTreasury(context, selfChar.GetOrganizationInfo().SettlementId, selfChar, this.ResourceType, this.Amount, -1);
			selfChar.ChangeResource(context, this.ResourceType, -this.Amount);
		}

		// Token: 0x04002182 RID: 8578
		public sbyte ResourceType;

		// Token: 0x04002183 RID: 8579
		public int Amount;
	}
}
