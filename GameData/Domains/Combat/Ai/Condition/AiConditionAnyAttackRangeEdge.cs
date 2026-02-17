using System;
using System.Collections.Generic;

namespace GameData.Domains.Combat.Ai.Condition
{
	// Token: 0x020007B1 RID: 1969
	[AiCondition(EAiConditionType.AnyAttackRangeEdge)]
	public class AiConditionAnyAttackRangeEdge : AiConditionCombatBase
	{
		// Token: 0x06006A18 RID: 27160 RVA: 0x003BBEEB File Offset: 0x003BA0EB
		public AiConditionAnyAttackRangeEdge(IReadOnlyList<int> ints)
		{
			this._isAlly = (ints[0] == 1);
		}

		// Token: 0x06006A19 RID: 27161 RVA: 0x003BBF08 File Offset: 0x003BA108
		public override bool Check(AiMemoryNew memory, CombatCharacter combatChar)
		{
			CombatCharacter checkChar = DomainManager.Combat.GetCombatCharacter(this._isAlly == combatChar.IsAlly, false);
			return DomainManager.Combat.AnyAttackRangeEdge(checkChar);
		}

		// Token: 0x04001D48 RID: 7496
		private readonly bool _isAlly;
	}
}
