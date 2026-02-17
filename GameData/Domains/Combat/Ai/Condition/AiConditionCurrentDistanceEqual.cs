using System;
using System.Collections.Generic;

namespace GameData.Domains.Combat.Ai.Condition
{
	// Token: 0x020007B4 RID: 1972
	[AiCondition(EAiConditionType.CurrentDistanceEqual)]
	public class AiConditionCurrentDistanceEqual : AiConditionCombatBase
	{
		// Token: 0x06006A1E RID: 27166 RVA: 0x003BC051 File Offset: 0x003BA251
		public AiConditionCurrentDistanceEqual(IReadOnlyList<int> ints)
		{
			this._target = ints[0];
		}

		// Token: 0x06006A1F RID: 27167 RVA: 0x003BC068 File Offset: 0x003BA268
		public override bool Check(AiMemoryNew memory, CombatCharacter combatChar)
		{
			return (int)DomainManager.Combat.GetCurrentDistance() == this._target;
		}

		// Token: 0x04001D4B RID: 7499
		private readonly int _target;
	}
}
