using System;
using System.Collections.Generic;

namespace GameData.Domains.Combat.Ai.Condition
{
	// Token: 0x02000769 RID: 1897
	[AiCondition(EAiConditionType.MindMarkCountMoreOrEqual)]
	public class AiConditionMindMarkCountMoreOrEqual : AiConditionMarkCountMoreOrEqualBase
	{
		// Token: 0x06006982 RID: 27010 RVA: 0x003BA33D File Offset: 0x003B853D
		public AiConditionMindMarkCountMoreOrEqual(IReadOnlyList<int> ints) : base(ints)
		{
		}

		// Token: 0x06006983 RID: 27011 RVA: 0x003BA348 File Offset: 0x003B8548
		protected override int CalcMarkCount(DefeatMarkCollection marks)
		{
			return marks.MindMarkList.Count;
		}
	}
}
