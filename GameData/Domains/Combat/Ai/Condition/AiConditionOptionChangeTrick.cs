using System;

namespace GameData.Domains.Combat.Ai.Condition
{
	// Token: 0x02000789 RID: 1929
	[AiCondition(EAiConditionType.OptionChangeTrick)]
	public class AiConditionOptionChangeTrick : AiConditionCombatBase
	{
		// Token: 0x060069C2 RID: 27074 RVA: 0x003BABD0 File Offset: 0x003B8DD0
		public override bool Check(AiMemoryNew memory, CombatCharacter combatChar)
		{
			bool flag = combatChar.IsAlly && !DomainManager.Combat.AiOptions.AutoAttack;
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				bool flag2 = combatChar.IsAlly && !DomainManager.Combat.AiOptions.AutoChangeTrick;
				result = (!flag2 && DomainManager.Combat.CanNormalAttack(combatChar.IsAlly) && combatChar.GetCanChangeTrick());
			}
			return result;
		}
	}
}
