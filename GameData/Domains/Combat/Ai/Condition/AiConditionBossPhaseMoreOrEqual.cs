using System;
using System.Collections.Generic;

namespace GameData.Domains.Combat.Ai.Condition
{
	// Token: 0x02000750 RID: 1872
	[AiCondition(EAiConditionType.BossPhaseMoreOrEqual)]
	public class AiConditionBossPhaseMoreOrEqual : AiConditionCombatBase
	{
		// Token: 0x0600694F RID: 26959 RVA: 0x003B9C17 File Offset: 0x003B7E17
		public AiConditionBossPhaseMoreOrEqual(IReadOnlyList<int> ints)
		{
			this._phase = ints[0];
		}

		// Token: 0x06006950 RID: 26960 RVA: 0x003B9C30 File Offset: 0x003B7E30
		public override bool Check(AiMemoryNew memory, CombatCharacter combatChar)
		{
			return (int)DomainManager.Combat.GetCombatCharacter(false, false).GetBossPhase() >= this._phase;
		}

		// Token: 0x04001CFF RID: 7423
		private readonly int _phase;
	}
}
