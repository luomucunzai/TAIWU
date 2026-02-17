using System;
using GameData.Domains.Combat.Ai.Selector;

namespace GameData.Domains.Combat.Ai.Action
{
	// Token: 0x020007FB RID: 2043
	[AiAction(EAiActionType.UseItemWine)]
	public class AiActionUseItemWine : AiActionUseItemCommonBase
	{
		// Token: 0x06006AC4 RID: 27332 RVA: 0x003BD5CB File Offset: 0x003BB7CB
		public AiActionUseItemWine() : base(EItemSelectorType.Wine)
		{
		}
	}
}
