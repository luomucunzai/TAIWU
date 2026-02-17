using System;
using GameData.Domains.Combat.Ai.Selector;

namespace GameData.Domains.Combat.Ai.Action
{
	// Token: 0x020007FA RID: 2042
	[AiAction(EAiActionType.UseItemNeili)]
	public class AiActionUseItemNeili : AiActionUseItemCommonBase
	{
		// Token: 0x06006AC3 RID: 27331 RVA: 0x003BD5C0 File Offset: 0x003BB7C0
		public AiActionUseItemNeili() : base(EItemSelectorType.Neili)
		{
		}
	}
}
