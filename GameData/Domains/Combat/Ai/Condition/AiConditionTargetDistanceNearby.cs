using System;
using System.Collections.Generic;

namespace GameData.Domains.Combat.Ai.Condition
{
	// Token: 0x02000783 RID: 1923
	[AiCondition(EAiConditionType.TargetDistanceNearby)]
	public class AiConditionTargetDistanceNearby : AiConditionCombatBase
	{
		// Token: 0x060069B6 RID: 27062 RVA: 0x003BAA57 File Offset: 0x003B8C57
		public AiConditionTargetDistanceNearby(IReadOnlyList<int> ints)
		{
			this._isForward = (ints[0] == 1);
		}

		// Token: 0x060069B7 RID: 27063 RVA: 0x003BAA70 File Offset: 0x003B8C70
		public override bool Check(AiMemoryNew memory, CombatCharacter combatChar)
		{
			short targetDistance = combatChar.GetTargetDistance();
			bool flag = targetDistance < 0;
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				short currentDistance = DomainManager.Combat.GetCurrentDistance();
				bool flag2 = currentDistance == targetDistance;
				result = (!flag2 && this._isForward == currentDistance > targetDistance);
			}
			return result;
		}

		// Token: 0x04001D2E RID: 7470
		private readonly bool _isForward;
	}
}
