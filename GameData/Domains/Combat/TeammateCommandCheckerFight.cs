using System;
using System.Collections.Generic;

namespace GameData.Domains.Combat
{
	// Token: 0x020006DA RID: 1754
	public class TeammateCommandCheckerFight : TeammateCommandCheckerBase
	{
		// Token: 0x17000410 RID: 1040
		// (get) Token: 0x06006778 RID: 26488 RVA: 0x003B1F67 File Offset: 0x003B0167
		protected override bool CheckTeammateBoth
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06006779 RID: 26489 RVA: 0x003B1F6A File Offset: 0x003B016A
		protected override IEnumerable<ETeammateCommandBanReason> Extra(TeammateCommandCheckerContext context)
		{
			yield break;
		}
	}
}
