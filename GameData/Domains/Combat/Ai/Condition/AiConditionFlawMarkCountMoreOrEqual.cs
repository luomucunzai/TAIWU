using System;
using System.Collections.Generic;

namespace GameData.Domains.Combat.Ai.Condition
{
	// Token: 0x02000766 RID: 1894
	[AiCondition(EAiConditionType.FlawMarkCountMoreOrEqual)]
	public class AiConditionFlawMarkCountMoreOrEqual : AiConditionMarkCountMoreOrEqualBase
	{
		// Token: 0x0600697C RID: 27004 RVA: 0x003BA2BE File Offset: 0x003B84BE
		public AiConditionFlawMarkCountMoreOrEqual(IReadOnlyList<int> ints) : base(ints)
		{
			this._bodyPart = (sbyte)ints[2];
		}

		// Token: 0x0600697D RID: 27005 RVA: 0x003BA2D6 File Offset: 0x003B84D6
		protected override int CalcMarkCount(DefeatMarkCollection marks)
		{
			return marks.FlawMarkList[(int)this._bodyPart].Count;
		}

		// Token: 0x04001D0D RID: 7437
		private readonly sbyte _bodyPart;
	}
}
