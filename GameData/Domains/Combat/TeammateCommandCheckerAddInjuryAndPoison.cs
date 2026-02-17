using System;
using System.Collections.Generic;

namespace GameData.Domains.Combat
{
	// Token: 0x020006EE RID: 1774
	public class TeammateCommandCheckerAddInjuryAndPoison : TeammateCommandCheckerBase
	{
		// Token: 0x060067AE RID: 26542 RVA: 0x003B220B File Offset: 0x003B040B
		protected override IEnumerable<ETeammateCommandBanReason> Extra(TeammateCommandCheckerContext context)
		{
			yield break;
		}
	}
}
