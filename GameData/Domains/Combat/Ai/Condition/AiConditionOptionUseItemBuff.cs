using System;
using GameData.Domains.Combat.Ai.Selector;

namespace GameData.Domains.Combat.Ai.Condition
{
	// Token: 0x020007A6 RID: 1958
	[AiCondition(EAiConditionType.OptionUseItemBuff)]
	public class AiConditionOptionUseItemBuff : AiConditionOptionUseItemCommonBase
	{
		// Token: 0x06006A03 RID: 27139 RVA: 0x003BBBC7 File Offset: 0x003B9DC7
		public AiConditionOptionUseItemBuff() : base(EItemSelectorType.Buff)
		{
		}
	}
}
