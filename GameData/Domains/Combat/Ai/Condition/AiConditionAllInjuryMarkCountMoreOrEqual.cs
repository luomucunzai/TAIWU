using System;
using System.Collections.Generic;

namespace GameData.Domains.Combat.Ai.Condition
{
	// Token: 0x02000775 RID: 1909
	[AiCondition(EAiConditionType.AllInjuryMarkCountMoreOrEqual)]
	public class AiConditionAllInjuryMarkCountMoreOrEqual : AiConditionMarkCountMoreOrEqualBase
	{
		// Token: 0x0600699A RID: 27034 RVA: 0x003BA5D2 File Offset: 0x003B87D2
		public AiConditionAllInjuryMarkCountMoreOrEqual(IReadOnlyList<int> ints) : base(ints)
		{
		}

		// Token: 0x0600699B RID: 27035 RVA: 0x003BA5DD File Offset: 0x003B87DD
		protected override int CalcMarkCount(DefeatMarkCollection marks)
		{
			return marks.GetTotalInjuryCount();
		}
	}
}
