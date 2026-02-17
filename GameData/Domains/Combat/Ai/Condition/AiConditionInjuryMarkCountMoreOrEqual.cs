using System;
using System.Collections.Generic;

namespace GameData.Domains.Combat.Ai.Condition
{
	// Token: 0x02000765 RID: 1893
	[AiCondition(EAiConditionType.InjuryMarkCountMoreOrEqual)]
	public class AiConditionInjuryMarkCountMoreOrEqual : AiConditionMarkCountMoreOrEqualBase
	{
		// Token: 0x0600697A RID: 27002 RVA: 0x003BA276 File Offset: 0x003B8476
		public AiConditionInjuryMarkCountMoreOrEqual(IReadOnlyList<int> ints) : base(ints)
		{
			this._bodyPart = (sbyte)ints[2];
			this._isInner = (ints[3] == 1);
		}

		// Token: 0x0600697B RID: 27003 RVA: 0x003BA29F File Offset: 0x003B849F
		protected override int CalcMarkCount(DefeatMarkCollection marks)
		{
			return (int)(this._isInner ? marks.InnerInjuryMarkList : marks.OuterInjuryMarkList)[(int)this._bodyPart];
		}

		// Token: 0x04001D0B RID: 7435
		private readonly sbyte _bodyPart;

		// Token: 0x04001D0C RID: 7436
		private readonly bool _isInner;
	}
}
