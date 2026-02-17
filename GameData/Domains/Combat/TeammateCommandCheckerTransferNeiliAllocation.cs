using System;
using System.Collections.Generic;

namespace GameData.Domains.Combat
{
	// Token: 0x020006E8 RID: 1768
	public class TeammateCommandCheckerTransferNeiliAllocation : TeammateCommandCheckerBase
	{
		// Token: 0x1700041B RID: 1051
		// (get) Token: 0x0600679F RID: 26527 RVA: 0x003B2148 File Offset: 0x003B0348
		protected override bool CheckTeammateAfter
		{
			get
			{
				return true;
			}
		}

		// Token: 0x060067A0 RID: 26528 RVA: 0x003B214B File Offset: 0x003B034B
		protected override IEnumerable<ETeammateCommandBanReason> Extra(TeammateCommandCheckerContext context)
		{
			bool flag = context.TeammateChar.GetNeiliAllocation().GetTotal() <= 0;
			if (flag)
			{
				yield return ETeammateCommandBanReason.TransferNeiliAllocationLack;
			}
			yield break;
		}
	}
}
