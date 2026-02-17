using System;
using System.Collections.Generic;

namespace GameData.Domains.Combat
{
	// Token: 0x020006E7 RID: 1767
	public class TeammateCommandCheckerStopEnemy : TeammateCommandCheckerBase
	{
		// Token: 0x0600679D RID: 26525 RVA: 0x003B2128 File Offset: 0x003B0328
		protected override IEnumerable<ETeammateCommandBanReason> Extra(TeammateCommandCheckerContext context)
		{
			yield break;
		}
	}
}
