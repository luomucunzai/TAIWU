using System;

namespace GameData.Domains.Combat.Ai.Condition
{
	// Token: 0x0200074D RID: 1869
	[AiCondition(EAiConditionType.IsTaiwu)]
	public class AiConditionIsTaiwu : AiConditionCombatBase
	{
		// Token: 0x06006949 RID: 26953 RVA: 0x003B9B5C File Offset: 0x003B7D5C
		public override bool Check(AiMemoryNew memory, CombatCharacter combatChar)
		{
			return combatChar.IsTaiwu;
		}
	}
}
