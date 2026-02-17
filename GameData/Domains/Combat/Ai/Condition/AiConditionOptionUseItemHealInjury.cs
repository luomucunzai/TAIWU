using System;
using GameData.Domains.Combat.Ai.Selector;

namespace GameData.Domains.Combat.Ai.Condition
{
	// Token: 0x020007A3 RID: 1955
	[AiCondition(EAiConditionType.OptionUseItemHealInjury)]
	public class AiConditionOptionUseItemHealInjury : AiConditionOptionUseItemCommonBase
	{
		// Token: 0x06006A00 RID: 27136 RVA: 0x003BBBA6 File Offset: 0x003B9DA6
		public AiConditionOptionUseItemHealInjury() : base(EItemSelectorType.HealInjury)
		{
		}
	}
}
