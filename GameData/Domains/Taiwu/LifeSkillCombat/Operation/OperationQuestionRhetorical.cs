using System;
using System.Collections.Generic;

namespace GameData.Domains.Taiwu.LifeSkillCombat.Operation
{
	// Token: 0x02000069 RID: 105
	public class OperationQuestionRhetorical : OperationQuestion
	{
		// Token: 0x060015DA RID: 5594 RVA: 0x0014C9AD File Offset: 0x0014ABAD
		public OperationQuestionRhetorical()
		{
		}

		// Token: 0x060015DB RID: 5595 RVA: 0x0014C9B7 File Offset: 0x0014ABB7
		public OperationQuestionRhetorical(sbyte playerId, int stamp, int gridIndex, int basePoint, IEnumerable<sbyte> wantUseEffectCards) : base(playerId, stamp, gridIndex, basePoint, wantUseEffectCards)
		{
		}
	}
}
