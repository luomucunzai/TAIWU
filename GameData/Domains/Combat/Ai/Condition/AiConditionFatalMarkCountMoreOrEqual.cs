using System;
using System.Collections.Generic;

namespace GameData.Domains.Combat.Ai.Condition
{
	// Token: 0x0200076A RID: 1898
	[AiCondition(EAiConditionType.FatalMarkCountMoreOrEqual)]
	public class AiConditionFatalMarkCountMoreOrEqual : AiConditionMarkCountMoreOrEqualBase
	{
		// Token: 0x06006984 RID: 27012 RVA: 0x003BA355 File Offset: 0x003B8555
		public AiConditionFatalMarkCountMoreOrEqual(IReadOnlyList<int> ints) : base(ints)
		{
		}

		// Token: 0x06006985 RID: 27013 RVA: 0x003BA360 File Offset: 0x003B8560
		protected override int CalcMarkCount(DefeatMarkCollection marks)
		{
			return marks.FatalDamageMarkCount;
		}
	}
}
