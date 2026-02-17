using System;
using System.Collections.Generic;

namespace GameData.Domains.Combat.Ai.Condition
{
	// Token: 0x02000735 RID: 1845
	public abstract class AiConditionCheckCharBase : AiConditionCombatBase
	{
		// Token: 0x0600690C RID: 26892 RVA: 0x003B92E2 File Offset: 0x003B74E2
		protected AiConditionCheckCharBase(IReadOnlyList<int> ints)
		{
			this.IsAlly = (ints[0] == 1);
		}

		// Token: 0x0600690D RID: 26893 RVA: 0x003B92FC File Offset: 0x003B74FC
		public override bool Check(AiMemoryNew memory, CombatCharacter combatChar)
		{
			CombatCharacter checkChar = DomainManager.Combat.GetCombatCharacter(combatChar.IsAlly == this.IsAlly, false);
			return this.Check(checkChar);
		}

		// Token: 0x0600690E RID: 26894
		protected abstract bool Check(CombatCharacter checkChar);

		// Token: 0x04001CE5 RID: 7397
		protected readonly bool IsAlly;
	}
}
