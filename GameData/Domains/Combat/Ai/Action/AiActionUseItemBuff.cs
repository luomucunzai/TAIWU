using System;
using GameData.Domains.Combat.Ai.Selector;

namespace GameData.Domains.Combat.Ai.Action
{
	// Token: 0x020007F8 RID: 2040
	[AiAction(EAiActionType.UseItemBuff)]
	public class AiActionUseItemBuff : AiActionUseItemCommonBase
	{
		// Token: 0x06006ABF RID: 27327 RVA: 0x003BD59F File Offset: 0x003BB79F
		public AiActionUseItemBuff() : base(EItemSelectorType.Buff)
		{
		}
	}
}
