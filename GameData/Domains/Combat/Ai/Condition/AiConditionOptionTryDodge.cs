using System;

namespace GameData.Domains.Combat.Ai.Condition
{
	// Token: 0x02000791 RID: 1937
	[AiCondition(EAiConditionType.OptionTryDodge)]
	public class AiConditionOptionTryDodge : AiConditionCombatBase
	{
		// Token: 0x060069D4 RID: 27092 RVA: 0x003BB1B4 File Offset: 0x003B93B4
		public override bool Check(AiMemoryNew memory, CombatCharacter combatChar)
		{
			bool flag = combatChar.IsAlly && !DomainManager.Combat.AiOptions.AutoMove;
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				bool flag2 = combatChar.IsAlly && !DomainManager.Combat.AiOptions.TryDodge;
				result = !flag2;
			}
			return result;
		}
	}
}
