using System;
using GameData.Domains.Combat.Ai.Selector;

namespace GameData.Domains.Combat.Ai.Condition
{
	// Token: 0x020007A8 RID: 1960
	[AiCondition(EAiConditionType.OptionUseItemNeili)]
	public class AiConditionOptionUseItemNeili : AiConditionOptionUseItemCommonBase
	{
		// Token: 0x06006A06 RID: 27142 RVA: 0x003BBC13 File Offset: 0x003B9E13
		public AiConditionOptionUseItemNeili() : base(EItemSelectorType.Neili)
		{
		}
	}
}
