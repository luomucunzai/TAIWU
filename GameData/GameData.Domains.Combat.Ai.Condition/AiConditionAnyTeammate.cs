using System.Collections.Generic;

namespace GameData.Domains.Combat.Ai.Condition;

[AiCondition(EAiConditionType.AnyTeammate)]
public class AiConditionAnyTeammate : AiConditionCheckCharBase
{
	public AiConditionAnyTeammate(IReadOnlyList<int> ints)
		: base(ints)
	{
	}

	protected override bool Check(CombatCharacter checkChar)
	{
		int[] characterList = DomainManager.Combat.GetCharacterList(checkChar.IsAlly);
		foreach (int num in characterList)
		{
			if (num >= 0 && num != checkChar.GetId())
			{
				return true;
			}
		}
		return false;
	}
}
