using System.Collections.Generic;

namespace GameData.Domains.Combat.Ai.Condition;

public abstract class AiConditionSpecialAffecting : AiConditionAnyAffecting
{
	protected readonly bool IsDirect;

	protected readonly short SkillId;

	protected AiConditionSpecialAffecting(IReadOnlyList<int> ints)
		: base(ints)
	{
		IsDirect = ints[1] == 1;
		SkillId = (short)ints[2];
	}

	public override bool Check(AiMemoryNew memory, CombatCharacter combatChar)
	{
		CombatCharacter combatCharacter = DomainManager.Combat.GetCombatCharacter(combatChar.IsAlly == IsAlly);
		if (combatCharacter.GetAffectingMoveSkillId() != SkillId)
		{
			return false;
		}
		if (!DomainManager.CombatSkill.TryGetElement_CombatSkills((charId: combatCharacter.GetId(), skillId: SkillId), out var element))
		{
			return false;
		}
		return (int)element.GetDirection() == ((!IsDirect) ? 1 : 0);
	}
}
