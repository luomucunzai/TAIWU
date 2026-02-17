using System;
using System.Collections.Generic;

namespace GameData.Domains.Combat.Ai.Condition
{
	// Token: 0x02000767 RID: 1895
	[AiCondition(EAiConditionType.AcupointMarkCountMoreOrEqual)]
	public class AiConditionAcupointMarkCountMoreOrEqual : AiConditionMarkCountMoreOrEqualBase
	{
		// Token: 0x0600697E RID: 27006 RVA: 0x003BA2EA File Offset: 0x003B84EA
		public AiConditionAcupointMarkCountMoreOrEqual(IReadOnlyList<int> ints) : base(ints)
		{
			this._bodyPart = (sbyte)ints[2];
		}

		// Token: 0x0600697F RID: 27007 RVA: 0x003BA302 File Offset: 0x003B8502
		protected override int CalcMarkCount(DefeatMarkCollection marks)
		{
			return marks.AcupointMarkList[(int)this._bodyPart].Count;
		}

		// Token: 0x04001D0E RID: 7438
		private readonly sbyte _bodyPart;
	}
}
