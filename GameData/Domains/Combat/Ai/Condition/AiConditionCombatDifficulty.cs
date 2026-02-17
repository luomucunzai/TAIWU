using System;
using System.Collections.Generic;

namespace GameData.Domains.Combat.Ai.Condition
{
	// Token: 0x0200074E RID: 1870
	[AiCondition(EAiConditionType.CombatDifficulty)]
	public class AiConditionCombatDifficulty : AiConditionCombatBase
	{
		// Token: 0x0600694B RID: 26955 RVA: 0x003B9B7D File Offset: 0x003B7D7D
		public AiConditionCombatDifficulty(IReadOnlyList<int> ints)
		{
			this._combatDifficulty = (byte)ints[0];
		}

		// Token: 0x0600694C RID: 26956 RVA: 0x003B9B98 File Offset: 0x003B7D98
		public override bool Check(AiMemoryNew memory, CombatCharacter combatChar)
		{
			return combatChar.IsAlly || DomainManager.World.GetCombatDifficulty() >= this._combatDifficulty;
		}

		// Token: 0x04001CFD RID: 7421
		private readonly byte _combatDifficulty;
	}
}
