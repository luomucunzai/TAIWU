using System;

namespace GameData.Domains.Combat.Ai.Condition
{
	// Token: 0x0200079F RID: 1951
	[AiCondition(EAiConditionType.OptionInterruptAffectingMove)]
	public class AiConditionOptionInterruptAffectingMove : AiConditionCombatBase
	{
		// Token: 0x060069F6 RID: 27126 RVA: 0x003BBA70 File Offset: 0x003B9C70
		public override bool Check(AiMemoryNew memory, CombatCharacter combatChar)
		{
			bool flag = combatChar.IsAlly && !DomainManager.Combat.AiOptions.AutoClearAgile;
			return !flag && combatChar.GetAffectingMoveSkillId() >= 0;
		}
	}
}
