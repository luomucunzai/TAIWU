using System;
using System.Collections.Generic;

namespace GameData.Domains.Combat.Ai.Condition
{
	// Token: 0x020007B6 RID: 1974
	[AiCondition(EAiConditionType.CurrentDistanceBelow)]
	public class AiConditionCurrentDistanceBelow : AiConditionCombatBase
	{
		// Token: 0x06006A22 RID: 27170 RVA: 0x003BC0C8 File Offset: 0x003BA2C8
		public AiConditionCurrentDistanceBelow(IReadOnlyList<int> ints)
		{
			this._target = ints[0];
		}

		// Token: 0x06006A23 RID: 27171 RVA: 0x003BC0E0 File Offset: 0x003BA2E0
		public override bool Check(AiMemoryNew memory, CombatCharacter combatChar)
		{
			return (int)DomainManager.Combat.GetCurrentDistance() < this._target;
		}

		// Token: 0x04001D4D RID: 7501
		private readonly int _target;
	}
}
