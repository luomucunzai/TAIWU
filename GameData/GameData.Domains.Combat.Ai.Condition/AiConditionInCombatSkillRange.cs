using System.Collections.Generic;
using GameData.Domains.Character;

namespace GameData.Domains.Combat.Ai.Condition;

[AiCondition(EAiConditionType.InCombatSkillRange)]
public class AiConditionInCombatSkillRange : AiConditionCheckCharBase
{
	private readonly int _offset;

	private readonly short _skillId;

	public AiConditionInCombatSkillRange(IReadOnlyList<int> ints)
		: base(ints)
	{
		_offset = ints[1];
		_skillId = (short)ints[2];
	}

	protected override bool Check(CombatCharacter checkChar)
	{
		short moveRangeOffsetCurrentDistance = DomainManager.Combat.GetMoveRangeOffsetCurrentDistance(_offset);
		var (num3, num4) = (OuterAndInnerShorts)(ref checkChar.CalcAttackRangeImmediate(_skillId));
		return num3 <= moveRangeOffsetCurrentDistance && moveRangeOffsetCurrentDistance <= num4;
	}
}
