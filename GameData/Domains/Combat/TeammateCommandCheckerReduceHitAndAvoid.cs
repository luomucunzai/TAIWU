using System;
using System.Collections.Generic;

namespace GameData.Domains.Combat
{
	// Token: 0x020006EF RID: 1775
	public class TeammateCommandCheckerReduceHitAndAvoid : TeammateCommandCheckerBase
	{
		// Token: 0x060067B0 RID: 26544 RVA: 0x003B222B File Offset: 0x003B042B
		protected override IEnumerable<ETeammateCommandBanReason> Extra(TeammateCommandCheckerContext context)
		{
			yield break;
		}
	}
}
