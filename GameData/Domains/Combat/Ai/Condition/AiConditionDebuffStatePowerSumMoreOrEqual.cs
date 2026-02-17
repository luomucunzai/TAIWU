using System;
using System.Collections.Generic;

namespace GameData.Domains.Combat.Ai.Condition
{
	// Token: 0x02000749 RID: 1865
	[AiCondition(EAiConditionType.DebuffStatePowerSumMoreOrEqual)]
	public class AiConditionDebuffStatePowerSumMoreOrEqual : AiConditionCheckCharStatePowerSumMoreOrEqualBase
	{
		// Token: 0x17000455 RID: 1109
		// (get) Token: 0x06006941 RID: 26945 RVA: 0x003B9A9F File Offset: 0x003B7C9F
		protected override sbyte StateType
		{
			get
			{
				return 2;
			}
		}

		// Token: 0x06006942 RID: 26946 RVA: 0x003B9AA2 File Offset: 0x003B7CA2
		public AiConditionDebuffStatePowerSumMoreOrEqual(IReadOnlyList<string> strings, IReadOnlyList<int> ints) : base(strings, ints)
		{
		}
	}
}
