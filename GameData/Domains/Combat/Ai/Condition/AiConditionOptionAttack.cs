using System;

namespace GameData.Domains.Combat.Ai.Condition
{
	// Token: 0x02000788 RID: 1928
	[AiCondition(EAiConditionType.OptionAttack)]
	public class AiConditionOptionAttack : AiConditionCombatBase
	{
		// Token: 0x060069C0 RID: 27072 RVA: 0x003BAB74 File Offset: 0x003B8D74
		public override bool Check(AiMemoryNew memory, CombatCharacter combatChar)
		{
			bool flag = combatChar.IsAlly && !DomainManager.Combat.AiOptions.AutoAttack;
			return !flag && DomainManager.Combat.CanNormalAttack(combatChar.IsAlly) && combatChar.CanNormalAttackImmediate;
		}
	}
}
