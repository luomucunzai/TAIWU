using System;

namespace GameData.Domains.Combat.Ai.Condition
{
	// Token: 0x0200079E RID: 1950
	[AiCondition(EAiConditionType.OptionInterruptAffectingDefense)]
	public class AiConditionOptionInterruptAffectingDefense : AiConditionCombatBase
	{
		// Token: 0x060069F4 RID: 27124 RVA: 0x003BBA24 File Offset: 0x003B9C24
		public override bool Check(AiMemoryNew memory, CombatCharacter combatChar)
		{
			bool flag = combatChar.IsAlly && !DomainManager.Combat.AiOptions.AutoClearDefense;
			return !flag && combatChar.GetAffectingDefendSkillId() >= 0;
		}
	}
}
