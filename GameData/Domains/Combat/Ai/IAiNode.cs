using System;
using System.Collections.Generic;

namespace GameData.Domains.Combat.Ai
{
	// Token: 0x02000720 RID: 1824
	public interface IAiNode
	{
		// Token: 0x17000440 RID: 1088
		// (get) Token: 0x060068A2 RID: 26786
		// (set) Token: 0x060068A3 RID: 26787
		int RuntimeId { get; set; }

		// Token: 0x17000441 RID: 1089
		// (get) Token: 0x060068A4 RID: 26788
		bool IsAction { get; }

		// Token: 0x060068A5 RID: 26789
		IEnumerable<int> Update(AiData data);
	}
}
