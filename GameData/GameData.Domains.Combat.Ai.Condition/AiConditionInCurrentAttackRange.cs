using System.Collections.Generic;
using GameData.Domains.Character;

namespace GameData.Domains.Combat.Ai.Condition;

[AiCondition(EAiConditionType.InCurrentAttackRange)]
public class AiConditionInCurrentAttackRange : AiConditionCheckCharBase
{
	private readonly int _offset;

	public AiConditionInCurrentAttackRange(IReadOnlyList<int> ints)
		: base(ints)
	{
		_offset = ints[1];
	}

	protected override bool Check(CombatCharacter checkChar)
	{
		if (checkChar.GetCanAttackOutRange())
		{
			return true;
		}
		short moveRangeOffsetCurrentDistance = DomainManager.Combat.GetMoveRangeOffsetCurrentDistance(_offset);
		var (num3, num4) = (OuterAndInnerShorts)(ref checkChar.GetAttackRange());
		return num3 <= moveRangeOffsetCurrentDistance && moveRangeOffsetCurrentDistance <= num4;
	}
}
