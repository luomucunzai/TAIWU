using System;
using System.Collections.Generic;

namespace GameData.Domains.Combat.Ai.Condition
{
	// Token: 0x02000760 RID: 1888
	[AiCondition(EAiConditionType.IsCharacterHalfFallen)]
	public class AiConditionIsCharacterHalfFallen : AiConditionCombatBase
	{
		// Token: 0x0600696F RID: 26991 RVA: 0x003BA0BB File Offset: 0x003B82BB
		public AiConditionIsCharacterHalfFallen(IReadOnlyList<int> ints)
		{
			this._isAlly = (ints[0] == 1);
		}

		// Token: 0x06006970 RID: 26992 RVA: 0x003BA0D4 File Offset: 0x003B82D4
		public override bool Check(AiMemoryNew memory, CombatCharacter combatChar)
		{
			CombatCharacter checkChar = DomainManager.Combat.GetCombatCharacter(combatChar.IsAlly == this._isAlly, false);
			return DomainManager.Combat.IsCharacterHalfFallen(checkChar);
		}

		// Token: 0x04001D09 RID: 7433
		private readonly bool _isAlly;
	}
}
