using System;

namespace GameData.Domains.Combat.Ai.Condition
{
	// Token: 0x0200075D RID: 1885
	[AiCondition(EAiConditionType.EnvironmentLastNormalAttackAnyMiss)]
	public class AiConditionEnvironmentLastNormalAttackAnyMiss : AiConditionCombatBase
	{
		// Token: 0x06006969 RID: 26985 RVA: 0x003BA00C File Offset: 0x003B820C
		public override bool Check(AiMemoryNew memory, CombatCharacter combatChar)
		{
			ValueTuple<int, AiEnvironment.ENormalAttackType> lastNormalAttack = combatChar.AiController.Environment.LastNormalAttack;
			return combatChar.GetUsingWeaponIndex() == lastNormalAttack.Item1 && lastNormalAttack.Item2 == AiEnvironment.ENormalAttackType.Miss;
		}
	}
}
