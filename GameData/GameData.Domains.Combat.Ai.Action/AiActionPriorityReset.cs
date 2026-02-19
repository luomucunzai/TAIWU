using System.Collections.Generic;
using Config;

namespace GameData.Domains.Combat.Ai.Action;

[AiAction(EAiActionType.PriorityReset)]
public class AiActionPriorityReset : AiActionCombatBase
{
	private readonly string _key;

	public AiActionPriorityReset(IReadOnlyList<string> strings)
	{
		_key = strings[0];
	}

	public override void Execute(AiMemoryNew memory, CombatCharacter combatChar)
	{
		if (memory.Ints.TryGetValue(_key, out var value) && value >= 0 && value < Config.CombatSkill.Instance.Count)
		{
			memory.SetPriority((short)value, EAiPriority.Middle);
		}
	}
}
