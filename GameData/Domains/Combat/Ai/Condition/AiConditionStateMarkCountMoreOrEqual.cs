using System;
using System.Collections.Generic;

namespace GameData.Domains.Combat.Ai.Condition
{
	// Token: 0x0200076D RID: 1901
	[AiCondition(EAiConditionType.StateMarkCountMoreOrEqual)]
	public class AiConditionStateMarkCountMoreOrEqual : AiConditionMarkCountMoreOrEqualBase
	{
		// Token: 0x0600698A RID: 27018 RVA: 0x003BA393 File Offset: 0x003B8593
		public AiConditionStateMarkCountMoreOrEqual(IReadOnlyList<int> ints) : base(ints)
		{
		}

		// Token: 0x0600698B RID: 27019 RVA: 0x003BA39E File Offset: 0x003B859E
		protected override int CalcMarkCount(DefeatMarkCollection marks)
		{
			return (int)marks.StateMarkCount;
		}
	}
}
