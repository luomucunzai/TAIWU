using System;

namespace GameData.Domains.Combat.Ai.Condition
{
	// Token: 0x0200079D RID: 1949
	[AiCondition(EAiConditionType.OptionInterruptCasting)]
	public class AiConditionOptionInterruptCasting : AiConditionCombatBase
	{
		// Token: 0x060069F2 RID: 27122 RVA: 0x003BB9D8 File Offset: 0x003B9BD8
		public override bool Check(AiMemoryNew memory, CombatCharacter combatChar)
		{
			bool flag = combatChar.IsAlly && !DomainManager.Combat.AiOptions.AutoInterrupt;
			return !flag && combatChar.GetPreparingSkillId() >= 0;
		}
	}
}
