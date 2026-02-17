using System;

namespace GameData.Domains.Combat.Ai.Condition
{
	// Token: 0x02000763 RID: 1891
	[AiCondition(EAiConditionType.InHazard)]
	public class AiConditionInHazard : AiConditionCombatBase
	{
		// Token: 0x06006975 RID: 26997 RVA: 0x003BA220 File Offset: 0x003B8420
		public override bool Check(AiMemoryNew memory, CombatCharacter combatChar)
		{
			return combatChar.AiController.IsHazard();
		}
	}
}
