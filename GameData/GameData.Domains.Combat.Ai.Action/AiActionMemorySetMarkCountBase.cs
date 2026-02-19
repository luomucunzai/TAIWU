using System.Collections.Generic;

namespace GameData.Domains.Combat.Ai.Action;

public abstract class AiActionMemorySetMarkCountBase : AiActionMemorySetCharValueBase
{
	protected AiActionMemorySetMarkCountBase(IReadOnlyList<string> strings, IReadOnlyList<int> ints)
		: base(strings, ints)
	{
	}

	protected sealed override int GetCharValue(CombatCharacter checkChar)
	{
		DefeatMarkCollection defeatMarkCollection = checkChar.GetDefeatMarkCollection();
		return GetMarkCount(defeatMarkCollection);
	}

	protected abstract int GetMarkCount(DefeatMarkCollection marks);
}
