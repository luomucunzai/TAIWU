using System;
using System.Collections.Generic;

namespace GameData.Domains.Combat.Ai.Condition
{
	// Token: 0x0200076E RID: 1902
	[AiCondition(EAiConditionType.HealthMarkCountMoreOrEqual)]
	public class AiConditionHealthMarkCountMoreOrEqual : AiConditionMarkCountMoreOrEqualBase
	{
		// Token: 0x0600698C RID: 27020 RVA: 0x003BA3A6 File Offset: 0x003B85A6
		public AiConditionHealthMarkCountMoreOrEqual(IReadOnlyList<int> ints) : base(ints)
		{
		}

		// Token: 0x0600698D RID: 27021 RVA: 0x003BA3B1 File Offset: 0x003B85B1
		protected override int CalcMarkCount(DefeatMarkCollection marks)
		{
			return (int)marks.HealthMarkCount;
		}
	}
}
