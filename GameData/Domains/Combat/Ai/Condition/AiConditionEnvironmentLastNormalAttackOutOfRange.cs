using System;

namespace GameData.Domains.Combat.Ai.Condition
{
	// Token: 0x0200075E RID: 1886
	[AiCondition(EAiConditionType.EnvironmentLastNormalAttackOutOfRange)]
	public class AiConditionEnvironmentLastNormalAttackOutOfRange : AiConditionCombatBase
	{
		// Token: 0x0600696B RID: 26987 RVA: 0x003BA054 File Offset: 0x003B8254
		public override bool Check(AiMemoryNew memory, CombatCharacter combatChar)
		{
			ValueTuple<int, AiEnvironment.ENormalAttackType> lastNormalAttack = combatChar.AiController.Environment.LastNormalAttack;
			return combatChar.GetUsingWeaponIndex() == lastNormalAttack.Item1 && lastNormalAttack.Item2 == AiEnvironment.ENormalAttackType.OutOfRange;
		}
	}
}
