using System;

namespace GameData.Domains.Combat.Ai.Condition
{
	// Token: 0x02000785 RID: 1925
	[AiCondition(EAiConditionType.TargetDistanceIsNotFarthest)]
	public class AiConditionTargetDistanceIsNotFarthest : AiConditionCombatBase
	{
		// Token: 0x060069BA RID: 27066 RVA: 0x003BAAF8 File Offset: 0x003B8CF8
		public override bool Check(AiMemoryNew memory, CombatCharacter combatChar)
		{
			return combatChar.AiTargetDistance != (short)DomainManager.Combat.GetDistanceRange().Item2;
		}
	}
}
