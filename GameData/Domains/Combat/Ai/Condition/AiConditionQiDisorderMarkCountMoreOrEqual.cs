using System;
using System.Collections.Generic;

namespace GameData.Domains.Combat.Ai.Condition
{
	// Token: 0x0200076C RID: 1900
	[AiCondition(EAiConditionType.QiDisorderMarkCountMoreOrEqual)]
	public class AiConditionQiDisorderMarkCountMoreOrEqual : AiConditionMarkCountMoreOrEqualBase
	{
		// Token: 0x06006988 RID: 27016 RVA: 0x003BA380 File Offset: 0x003B8580
		public AiConditionQiDisorderMarkCountMoreOrEqual(IReadOnlyList<int> ints) : base(ints)
		{
		}

		// Token: 0x06006989 RID: 27017 RVA: 0x003BA38B File Offset: 0x003B858B
		protected override int CalcMarkCount(DefeatMarkCollection marks)
		{
			return (int)marks.QiDisorderMarkCount;
		}
	}
}
