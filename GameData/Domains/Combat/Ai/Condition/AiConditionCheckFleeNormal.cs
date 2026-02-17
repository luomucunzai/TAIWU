using System;

namespace GameData.Domains.Combat.Ai.Condition
{
	// Token: 0x02000762 RID: 1890
	[AiCondition(EAiConditionType.CheckFleeNormal)]
	public class AiConditionCheckFleeNormal : AiConditionCombatBase
	{
		// Token: 0x06006973 RID: 26995 RVA: 0x003BA1C0 File Offset: 0x003B83C0
		public override bool Check(AiMemoryNew memory, CombatCharacter combatChar)
		{
			bool flag = !DomainManager.Combat.IsCharacterHalfFallen(combatChar);
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				CombatCharacter enemyChar = DomainManager.Combat.GetCombatCharacter(!combatChar.IsAlly, false);
				result = (combatChar.GetDefeatMarkCollection().GetTotalCount() > enemyChar.GetDefeatMarkCollection().GetTotalCount());
			}
			return result;
		}
	}
}
