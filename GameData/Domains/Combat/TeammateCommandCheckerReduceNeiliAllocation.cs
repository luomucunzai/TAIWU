using System;
using System.Collections.Generic;

namespace GameData.Domains.Combat
{
	// Token: 0x020006F1 RID: 1777
	public class TeammateCommandCheckerReduceNeiliAllocation : TeammateCommandCheckerBase
	{
		// Token: 0x060067B4 RID: 26548 RVA: 0x003B226B File Offset: 0x003B046B
		protected override IEnumerable<ETeammateCommandBanReason> Extra(TeammateCommandCheckerContext context)
		{
			bool flag = context.CurrChar.GetNeiliAllocation().GetTotal() <= 0;
			if (flag)
			{
				yield return ETeammateCommandBanReason.Negative;
			}
			yield break;
		}
	}
}
