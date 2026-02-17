using System;
using GameData.Domains.Combat.Ai.Selector;

namespace GameData.Domains.Combat.Ai.Condition
{
	// Token: 0x020007A4 RID: 1956
	[AiCondition(EAiConditionType.OptionUseItemHealPoison)]
	public class AiConditionOptionUseItemHealPoison : AiConditionOptionUseItemCommonBase
	{
		// Token: 0x06006A01 RID: 27137 RVA: 0x003BBBB1 File Offset: 0x003B9DB1
		public AiConditionOptionUseItemHealPoison() : base(EItemSelectorType.HealPoison)
		{
		}
	}
}
