namespace GameData.Domains.Combat.Ai.Condition;

[AiCondition(EAiConditionType.CheckFleeNormal)]
public class AiConditionCheckFleeNormal : AiConditionCombatBase
{
	public override bool Check(AiMemoryNew memory, CombatCharacter combatChar)
	{
		if (!DomainManager.Combat.IsCharacterHalfFallen(combatChar))
		{
			return false;
		}
		CombatCharacter combatCharacter = DomainManager.Combat.GetCombatCharacter(!combatChar.IsAlly);
		return combatChar.GetDefeatMarkCollection().GetTotalCount() > combatCharacter.GetDefeatMarkCollection().GetTotalCount();
	}
}
