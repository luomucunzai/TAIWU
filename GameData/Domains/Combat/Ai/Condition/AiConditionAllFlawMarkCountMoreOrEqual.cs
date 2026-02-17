using System;
using System.Collections.Generic;

namespace GameData.Domains.Combat.Ai.Condition
{
	// Token: 0x02000776 RID: 1910
	[AiCondition(EAiConditionType.AllFlawMarkCountMoreOrEqual)]
	public class AiConditionAllFlawMarkCountMoreOrEqual : AiConditionMarkCountMoreOrEqualBase
	{
		// Token: 0x0600699C RID: 27036 RVA: 0x003BA5E5 File Offset: 0x003B87E5
		public AiConditionAllFlawMarkCountMoreOrEqual(IReadOnlyList<int> ints) : base(ints)
		{
		}

		// Token: 0x0600699D RID: 27037 RVA: 0x003BA5F0 File Offset: 0x003B87F0
		protected override int CalcMarkCount(DefeatMarkCollection marks)
		{
			return marks.GetTotalFlawCount();
		}
	}
}
