using System;
using GameData.Domains.Combat.Ai.Selector;

namespace GameData.Domains.Combat.Ai.Action
{
	// Token: 0x020007F6 RID: 2038
	[AiAction(EAiActionType.UseItemHealPoison)]
	public class AiActionUseItemHealPoison : AiActionUseItemCommonBase
	{
		// Token: 0x06006ABD RID: 27325 RVA: 0x003BD589 File Offset: 0x003BB789
		public AiActionUseItemHealPoison() : base(EItemSelectorType.HealPoison)
		{
		}
	}
}
