using System;
using System.Collections.Generic;

namespace GameData.Domains.Combat.Ai.Condition
{
	// Token: 0x0200073E RID: 1854
	public abstract class AiConditionAnyAffecting : AiConditionCombatBase
	{
		// Token: 0x0600692A RID: 26922 RVA: 0x003B97AC File Offset: 0x003B79AC
		protected AiConditionAnyAffecting(IReadOnlyList<int> ints)
		{
			this.IsAlly = (ints[0] == 1);
		}

		// Token: 0x0600692B RID: 26923 RVA: 0x003B97C8 File Offset: 0x003B79C8
		public override bool Check(AiMemoryNew memory, CombatCharacter combatChar)
		{
			CombatCharacter checkChar = DomainManager.Combat.GetCombatCharacter(combatChar.IsAlly == this.IsAlly, false);
			return this.GetAffectingSkillId(checkChar) >= 0;
		}

		// Token: 0x0600692C RID: 26924
		protected abstract short GetAffectingSkillId(CombatCharacter combatChar);

		// Token: 0x04001CF2 RID: 7410
		protected readonly bool IsAlly;
	}
}
