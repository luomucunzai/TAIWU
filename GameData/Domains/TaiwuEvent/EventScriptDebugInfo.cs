using System;
using System.Collections.Generic;

namespace GameData.Domains.TaiwuEvent
{
	// Token: 0x0200007A RID: 122
	public class EventScriptDebugInfo
	{
		// Token: 0x04000484 RID: 1156
		public bool PauseOnStart;

		// Token: 0x04000485 RID: 1157
		public Dictionary<int, Func<bool>> BreakPoints;
	}
}
