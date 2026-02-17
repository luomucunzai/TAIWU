using System;
using GameData.Domains.Combat.Ai.Selector;

namespace GameData.Domains.Combat.Ai.Action
{
	// Token: 0x020007F7 RID: 2039
	[AiAction(EAiActionType.UseItemHealQiDisorder)]
	public class AiActionUseItemHealQiDisorder : AiActionUseItemCommonBase
	{
		// Token: 0x06006ABE RID: 27326 RVA: 0x003BD594 File Offset: 0x003BB794
		public AiActionUseItemHealQiDisorder() : base(EItemSelectorType.HealQiDisorder)
		{
		}
	}
}
