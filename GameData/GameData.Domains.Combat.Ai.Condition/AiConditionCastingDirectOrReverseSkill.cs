using System.Collections.Generic;

namespace GameData.Domains.Combat.Ai.Condition;

[AiCondition(EAiConditionType.CastingDirectOrReverseSkill)]
public class AiConditionCastingDirectOrReverseSkill : AiConditionCheckCharBase
{
	private readonly bool _isDirect;

	private readonly short _skillId;

	public AiConditionCastingDirectOrReverseSkill(IReadOnlyList<int> ints)
		: base(ints)
	{
		_isDirect = ints[1] == 1;
		_skillId = (short)ints[2];
	}

	protected override bool Check(CombatCharacter checkChar)
	{
		short preparingSkillId = checkChar.GetPreparingSkillId();
		if (preparingSkillId != _skillId)
		{
			return false;
		}
		if (!DomainManager.CombatSkill.TryGetElement_CombatSkills((charId: checkChar.GetId(), skillId: preparingSkillId), out var element))
		{
			return false;
		}
		return (int)element.GetDirection() == ((!_isDirect) ? 1 : 0);
	}
}
