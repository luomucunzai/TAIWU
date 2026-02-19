namespace GameData.Domains.Combat.Ai.Action;

[AiAction(EAiActionType.MoveToNearbyEscape)]
public class AiActionMoveToNearbyEscape : AiActionCombatBase
{
	public override void Execute(AiMemoryNew memory, CombatCharacter combatChar)
	{
		CombatCharacter combatCharacter = DomainManager.Combat.GetCombatCharacter(!combatChar.IsAlly);
		combatChar.AiTargetDistance = DomainManager.Combat.GetNearlyOutDistance(combatCharacter.GetAttackRange());
	}
}
