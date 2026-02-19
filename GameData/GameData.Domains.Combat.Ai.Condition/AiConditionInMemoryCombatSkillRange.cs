using System.Collections.Generic;
using GameData.Domains.Character;

namespace GameData.Domains.Combat.Ai.Condition;

[AiCondition(EAiConditionType.InMemoryCombatSkillRange)]
public class AiConditionInMemoryCombatSkillRange : AiConditionCombatBase
{
	private readonly string _key;

	private readonly bool _isAlly;

	private readonly int _offset;

	public AiConditionInMemoryCombatSkillRange(IReadOnlyList<string> strings, IReadOnlyList<int> ints)
	{
		_key = strings[0];
		_isAlly = ints[0] == 1;
		_offset = ints[1];
	}

	public override bool Check(AiMemoryNew memory, CombatCharacter combatChar)
	{
		if (!memory.Ints.TryGetValue(_key, out var value) || value < 0)
		{
			return false;
		}
		CombatCharacter combatCharacter = DomainManager.Combat.GetCombatCharacter(combatChar.IsAlly == _isAlly);
		short moveRangeOffsetCurrentDistance = DomainManager.Combat.GetMoveRangeOffsetCurrentDistance(_offset);
		var (num3, num4) = (OuterAndInnerShorts)(ref combatCharacter.CalcAttackRangeImmediate((short)value));
		return num3 <= moveRangeOffsetCurrentDistance && moveRangeOffsetCurrentDistance <= num4;
	}
}
