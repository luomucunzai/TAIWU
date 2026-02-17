using System;
using System.Collections.Generic;

namespace GameData.Domains.Combat.Ai.Condition
{
	// Token: 0x02000748 RID: 1864
	[AiCondition(EAiConditionType.BuffStatePowerSumMoreOrEqual)]
	public class AiConditionBuffStatePowerSumMoreOrEqual : AiConditionCheckCharStatePowerSumMoreOrEqualBase
	{
		// Token: 0x17000454 RID: 1108
		// (get) Token: 0x0600693F RID: 26943 RVA: 0x003B9A90 File Offset: 0x003B7C90
		protected override sbyte StateType
		{
			get
			{
				return 1;
			}
		}

		// Token: 0x06006940 RID: 26944 RVA: 0x003B9A93 File Offset: 0x003B7C93
		public AiConditionBuffStatePowerSumMoreOrEqual(IReadOnlyList<string> strings, IReadOnlyList<int> ints) : base(strings, ints)
		{
		}
	}
}
