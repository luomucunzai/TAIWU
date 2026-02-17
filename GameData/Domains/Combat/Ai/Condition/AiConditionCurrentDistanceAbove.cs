using System;
using System.Collections.Generic;

namespace GameData.Domains.Combat.Ai.Condition
{
	// Token: 0x020007B5 RID: 1973
	[AiCondition(EAiConditionType.CurrentDistanceAbove)]
	public class AiConditionCurrentDistanceAbove : AiConditionCombatBase
	{
		// Token: 0x06006A20 RID: 27168 RVA: 0x003BC08C File Offset: 0x003BA28C
		public AiConditionCurrentDistanceAbove(IReadOnlyList<int> ints)
		{
			this._target = ints[0];
		}

		// Token: 0x06006A21 RID: 27169 RVA: 0x003BC0A4 File Offset: 0x003BA2A4
		public override bool Check(AiMemoryNew memory, CombatCharacter combatChar)
		{
			return (int)DomainManager.Combat.GetCurrentDistance() > this._target;
		}

		// Token: 0x04001D4C RID: 7500
		private readonly int _target;
	}
}
