using System;
using System.Collections.Generic;

namespace GameData.Domains.Combat.Ai.Condition
{
	// Token: 0x02000768 RID: 1896
	[AiCondition(EAiConditionType.PoisonMarkCountMoreOrEqual)]
	public class AiConditionPoisonMarkCountMoreOrEqual : AiConditionMarkCountMoreOrEqualBase
	{
		// Token: 0x06006980 RID: 27008 RVA: 0x003BA316 File Offset: 0x003B8516
		public AiConditionPoisonMarkCountMoreOrEqual(IReadOnlyList<int> ints) : base(ints)
		{
			this._poisonType = (sbyte)ints[2];
		}

		// Token: 0x06006981 RID: 27009 RVA: 0x003BA32E File Offset: 0x003B852E
		protected override int CalcMarkCount(DefeatMarkCollection marks)
		{
			return (int)marks.PoisonMarkList[(int)this._poisonType];
		}

		// Token: 0x04001D0F RID: 7439
		private readonly sbyte _poisonType;
	}
}
