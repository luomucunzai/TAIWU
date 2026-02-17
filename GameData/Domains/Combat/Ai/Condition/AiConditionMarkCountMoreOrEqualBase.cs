using System;
using System.Collections.Generic;

namespace GameData.Domains.Combat.Ai.Condition
{
	// Token: 0x02000764 RID: 1892
	public abstract class AiConditionMarkCountMoreOrEqualBase : AiConditionCheckCharBase
	{
		// Token: 0x06006977 RID: 26999 RVA: 0x003BA246 File Offset: 0x003B8446
		protected AiConditionMarkCountMoreOrEqualBase(IReadOnlyList<int> ints) : base(ints)
		{
			this.MarkCount = ints[1];
		}

		// Token: 0x06006978 RID: 27000 RVA: 0x003BA25D File Offset: 0x003B845D
		protected override bool Check(CombatCharacter checkChar)
		{
			return this.CalcMarkCount(checkChar.GetDefeatMarkCollection()) >= this.MarkCount;
		}

		// Token: 0x06006979 RID: 27001
		protected abstract int CalcMarkCount(DefeatMarkCollection marks);

		// Token: 0x04001D0A RID: 7434
		protected readonly int MarkCount;
	}
}
