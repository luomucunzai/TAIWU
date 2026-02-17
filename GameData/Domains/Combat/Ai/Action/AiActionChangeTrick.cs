using System;
using System.Collections.Generic;

namespace GameData.Domains.Combat.Ai.Action
{
	// Token: 0x020007E1 RID: 2017
	[AiAction(EAiActionType.ChangeTrick)]
	public class AiActionChangeTrick : AiActionChangeTrickBase
	{
		// Token: 0x06006A8C RID: 27276 RVA: 0x003BCC64 File Offset: 0x003BAE64
		public AiActionChangeTrick(IReadOnlyList<string> strings) : base(strings)
		{
		}
	}
}
