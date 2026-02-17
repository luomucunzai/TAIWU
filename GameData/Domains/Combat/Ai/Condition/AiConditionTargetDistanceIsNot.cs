using System;
using System.Collections.Generic;

namespace GameData.Domains.Combat.Ai.Condition
{
	// Token: 0x02000784 RID: 1924
	[AiCondition(EAiConditionType.TargetDistanceIsNot)]
	public class AiConditionTargetDistanceIsNot : AiConditionCombatBase
	{
		// Token: 0x060069B8 RID: 27064 RVA: 0x003BAABA File Offset: 0x003B8CBA
		public AiConditionTargetDistanceIsNot(IReadOnlyList<int> ints)
		{
			this._targetDistance = ints[0];
		}

		// Token: 0x060069B9 RID: 27065 RVA: 0x003BAAD4 File Offset: 0x003B8CD4
		public override bool Check(AiMemoryNew memory, CombatCharacter combatChar)
		{
			return (int)combatChar.AiTargetDistance != this._targetDistance;
		}

		// Token: 0x04001D2F RID: 7471
		private readonly int _targetDistance;
	}
}
