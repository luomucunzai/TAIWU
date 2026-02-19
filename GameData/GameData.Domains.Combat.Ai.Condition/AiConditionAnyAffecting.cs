using System.Collections.Generic;

namespace GameData.Domains.Combat.Ai.Condition;

public abstract class AiConditionAnyAffecting : AiConditionCombatBase
{
	protected readonly bool IsAlly;

	protected AiConditionAnyAffecting(IReadOnlyList<int> ints)
	{
		IsAlly = ints[0] == 1;
	}

	public override bool Check(AiMemoryNew memory, CombatCharacter combatChar)
	{
		CombatCharacter combatCharacter = DomainManager.Combat.GetCombatCharacter(combatChar.IsAlly == IsAlly);
		return GetAffectingSkillId(combatCharacter) >= 0;
	}

	protected abstract short GetAffectingSkillId(CombatCharacter combatChar);
}
