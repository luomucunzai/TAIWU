using System;
using System.Collections.Generic;

namespace GameData.Domains.Combat.Ai.Condition
{
	// Token: 0x02000773 RID: 1907
	[AiCondition(EAiConditionType.AllMarkCountMoreOrEqual)]
	public class AiConditionAllMarkCountMoreOrEqual : AiConditionMarkCountMoreOrEqualBase
	{
		// Token: 0x06006996 RID: 27030 RVA: 0x003BA588 File Offset: 0x003B8788
		public AiConditionAllMarkCountMoreOrEqual(IReadOnlyList<int> ints) : base(ints)
		{
		}

		// Token: 0x06006997 RID: 27031 RVA: 0x003BA593 File Offset: 0x003B8793
		protected override int CalcMarkCount(DefeatMarkCollection marks)
		{
			return marks.GetTotalCount();
		}
	}
}
