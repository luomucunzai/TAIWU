using System;
using System.Collections.Generic;

namespace GameData.Domains.Combat.Ai.Condition
{
	// Token: 0x02000777 RID: 1911
	[AiCondition(EAiConditionType.AllAcupointMarkCountMoreOrEqual)]
	public class AiConditionAllAcupointMarkCountMoreOrEqual : AiConditionMarkCountMoreOrEqualBase
	{
		// Token: 0x0600699E RID: 27038 RVA: 0x003BA5F8 File Offset: 0x003B87F8
		public AiConditionAllAcupointMarkCountMoreOrEqual(IReadOnlyList<int> ints) : base(ints)
		{
		}

		// Token: 0x0600699F RID: 27039 RVA: 0x003BA603 File Offset: 0x003B8803
		protected override int CalcMarkCount(DefeatMarkCollection marks)
		{
			return marks.GetTotalAcupointCount();
		}
	}
}
