namespace GameData.Domains.Combat.Ai.Condition;

[AiCondition(EAiConditionType.OptionChangeTrickFlaw)]
public class AiConditionOptionChangeTrickFlaw : AiConditionCombatBase
{
	public override bool Check(AiMemoryNew memory, CombatCharacter combatChar)
	{
		if (combatChar.IsAlly && !DomainManager.Combat.AiOptions.AutoAttack)
		{
			return false;
		}
		if (combatChar.IsAlly && !DomainManager.Combat.AiOptions.AutoChangeTrick)
		{
			return false;
		}
		if (!DomainManager.Combat.CanNormalAttack(combatChar.IsAlly) || !combatChar.GetCanChangeTrick())
		{
			return false;
		}
		int num = CFormulaHelper.CalcCostChangeTrickCount(combatChar, EFlawOrAcupointType.Flaw);
		return combatChar.GetChangeTrickCount() >= num;
	}
}
