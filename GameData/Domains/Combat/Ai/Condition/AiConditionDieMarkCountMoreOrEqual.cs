using System;
using System.Collections.Generic;

namespace GameData.Domains.Combat.Ai.Condition
{
	// Token: 0x0200076B RID: 1899
	[AiCondition(EAiConditionType.DieMarkCountMoreOrEqual)]
	public class AiConditionDieMarkCountMoreOrEqual : AiConditionMarkCountMoreOrEqualBase
	{
		// Token: 0x06006986 RID: 27014 RVA: 0x003BA368 File Offset: 0x003B8568
		public AiConditionDieMarkCountMoreOrEqual(IReadOnlyList<int> ints) : base(ints)
		{
		}

		// Token: 0x06006987 RID: 27015 RVA: 0x003BA373 File Offset: 0x003B8573
		protected override int CalcMarkCount(DefeatMarkCollection marks)
		{
			return marks.DieMarkList.Count;
		}
	}
}
