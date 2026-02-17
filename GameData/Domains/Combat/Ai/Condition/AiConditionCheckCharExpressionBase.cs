using System;
using System.Collections.Generic;
using GameData.Combat.Math;

namespace GameData.Domains.Combat.Ai.Condition
{
	// Token: 0x02000736 RID: 1846
	public abstract class AiConditionCheckCharExpressionBase : AiConditionCombatBase
	{
		// Token: 0x0600690F RID: 26895 RVA: 0x003B932F File Offset: 0x003B752F
		protected AiConditionCheckCharExpressionBase(IReadOnlyList<string> strings, IReadOnlyList<int> ints)
		{
			this.Expression = CExpression.FromBase64(strings[0]);
			this.IsAlly = (ints[0] == 1);
		}

		// Token: 0x06006910 RID: 26896 RVA: 0x003B935C File Offset: 0x003B755C
		public override bool Check(AiMemoryNew memory, CombatCharacter combatChar)
		{
			CombatCharacter checkChar = DomainManager.Combat.GetCombatCharacter(combatChar.IsAlly == this.IsAlly, false);
			int expressionResult = this.Expression.Calc(combatChar);
			return this.Check(checkChar, expressionResult);
		}

		// Token: 0x06006911 RID: 26897
		protected abstract bool Check(CombatCharacter checkChar, int expressionResult);

		// Token: 0x04001CE6 RID: 7398
		protected readonly CExpression Expression;

		// Token: 0x04001CE7 RID: 7399
		protected readonly bool IsAlly;
	}
}
