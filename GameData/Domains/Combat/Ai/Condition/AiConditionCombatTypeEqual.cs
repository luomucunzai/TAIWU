using System;
using System.Collections.Generic;

namespace GameData.Domains.Combat.Ai.Condition
{
	// Token: 0x02000756 RID: 1878
	[AiCondition(EAiConditionType.CombatTypeEqual)]
	public class AiConditionCombatTypeEqual : AiConditionCombatBase
	{
		// Token: 0x0600695B RID: 26971 RVA: 0x003B9DC3 File Offset: 0x003B7FC3
		public AiConditionCombatTypeEqual(IReadOnlyList<int> ints)
		{
			this._combatType = (sbyte)ints[0];
		}

		// Token: 0x0600695C RID: 26972 RVA: 0x003B9DDC File Offset: 0x003B7FDC
		public override bool Check(AiMemoryNew memory, CombatCharacter combatChar)
		{
			return DomainManager.Combat.GetCombatType() == this._combatType;
		}

		// Token: 0x04001D06 RID: 7430
		private readonly sbyte _combatType;
	}
}
