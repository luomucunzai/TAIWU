using System;
using GameData.Domains.Combat.Ai.Selector;

namespace GameData.Domains.Combat.Ai.Condition
{
	// Token: 0x020007A5 RID: 1957
	[AiCondition(EAiConditionType.OptionUseItemHealQiDisorder)]
	public class AiConditionOptionUseItemHealQiDisorder : AiConditionOptionUseItemCommonBase
	{
		// Token: 0x06006A02 RID: 27138 RVA: 0x003BBBBC File Offset: 0x003B9DBC
		public AiConditionOptionUseItemHealQiDisorder() : base(EItemSelectorType.HealQiDisorder)
		{
		}
	}
}
