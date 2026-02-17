using System;
using System.Collections.Generic;

namespace GameData.Domains.Combat
{
	// Token: 0x020006D8 RID: 1752
	public interface ITeammateCommandChecker
	{
		// Token: 0x06006771 RID: 26481
		IEnumerable<ETeammateCommandBanReason> Check(int index, TeammateCommandCheckerContext context);
	}
}
