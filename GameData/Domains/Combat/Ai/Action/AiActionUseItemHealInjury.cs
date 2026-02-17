using System;
using GameData.Domains.Combat.Ai.Selector;

namespace GameData.Domains.Combat.Ai.Action
{
	// Token: 0x020007F5 RID: 2037
	[AiAction(EAiActionType.UseItemHealInjury)]
	public class AiActionUseItemHealInjury : AiActionUseItemCommonBase
	{
		// Token: 0x06006ABC RID: 27324 RVA: 0x003BD57E File Offset: 0x003BB77E
		public AiActionUseItemHealInjury() : base(EItemSelectorType.HealInjury)
		{
		}
	}
}
