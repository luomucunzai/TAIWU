using System;
using System.Collections.Generic;
using GameData.Utilities;

namespace GameData.Domains.Combat.Ai.Condition
{
	// Token: 0x02000774 RID: 1908
	[AiCondition(EAiConditionType.OuterOrInnerInjuryMarkCountMoreOrEqual)]
	public class AiConditionOuterOrInnerInjuryMarkCountMoreOrEqual : AiConditionMarkCountMoreOrEqualBase
	{
		// Token: 0x06006998 RID: 27032 RVA: 0x003BA59B File Offset: 0x003B879B
		public AiConditionOuterOrInnerInjuryMarkCountMoreOrEqual(IReadOnlyList<int> ints) : base(ints)
		{
			this._isInner = (ints[0] == 1);
		}

		// Token: 0x06006999 RID: 27033 RVA: 0x003BA5B5 File Offset: 0x003B87B5
		protected override int CalcMarkCount(DefeatMarkCollection marks)
		{
			return (this._isInner ? marks.InnerInjuryMarkList : marks.OuterInjuryMarkList).Sum();
		}

		// Token: 0x04001D17 RID: 7447
		private readonly bool _isInner;
	}
}
