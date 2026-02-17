using System;

namespace GameData.Domains.Combat
{
	// Token: 0x020006B2 RID: 1714
	[Flags]
	public enum EWugReplaceType
	{
		// Token: 0x04001B97 RID: 7063
		None = 0,
		// Token: 0x04001B98 RID: 7064
		Nonexistent = 1,
		// Token: 0x04001B99 RID: 7065
		CombatOnly = 2,
		// Token: 0x04001B9A RID: 7066
		Ungrown = 4,
		// Token: 0x04001B9B RID: 7067
		All = -1
	}
}
