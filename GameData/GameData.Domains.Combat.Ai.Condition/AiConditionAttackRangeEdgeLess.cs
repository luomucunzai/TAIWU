using System.Collections.Generic;

namespace GameData.Domains.Combat.Ai.Condition;

[AiCondition(EAiConditionType.AttackRangeEdgeLess)]
public class AiConditionAttackRangeEdgeLess : AiConditionCheckCharBase
{
	private readonly bool _isForward;

	public AiConditionAttackRangeEdgeLess(IReadOnlyList<int> ints)
		: base(ints)
	{
		_isForward = ints[1] == 1;
	}

	protected override bool Check(CombatCharacter checkChar)
	{
		CombatCharacter combatCharacter = DomainManager.Combat.GetCombatCharacter(!checkChar.IsAlly);
		short num = (_isForward ? checkChar.GetAttackRange().Outer : checkChar.GetAttackRange().Inner);
		short num2 = (_isForward ? combatCharacter.GetAttackRange().Outer : combatCharacter.GetAttackRange().Inner);
		return num < num2;
	}
}
