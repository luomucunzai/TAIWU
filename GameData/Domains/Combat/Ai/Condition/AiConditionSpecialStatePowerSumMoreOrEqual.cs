using System;
using System.Collections.Generic;

namespace GameData.Domains.Combat.Ai.Condition
{
	// Token: 0x0200074A RID: 1866
	[AiCondition(EAiConditionType.SpecialStatePowerSumMoreOrEqual)]
	public class AiConditionSpecialStatePowerSumMoreOrEqual : AiConditionCheckCharStatePowerSumMoreOrEqualBase
	{
		// Token: 0x17000456 RID: 1110
		// (get) Token: 0x06006943 RID: 26947 RVA: 0x003B9AAE File Offset: 0x003B7CAE
		protected override sbyte StateType
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x06006944 RID: 26948 RVA: 0x003B9AB1 File Offset: 0x003B7CB1
		public AiConditionSpecialStatePowerSumMoreOrEqual(IReadOnlyList<string> strings, IReadOnlyList<int> ints) : base(strings, ints)
		{
		}
	}
}
