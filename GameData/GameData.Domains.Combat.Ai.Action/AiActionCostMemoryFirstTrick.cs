using System.Collections.Generic;
using System.Linq;
using Config;
using GameData.Common;

namespace GameData.Domains.Combat.Ai.Action;

[AiAction(EAiActionType.CostMemoryFirstTrick)]
public class AiActionCostMemoryFirstTrick : AiActionCombatBase
{
	private readonly string _key;

	public AiActionCostMemoryFirstTrick(IReadOnlyList<string> strings)
	{
		_key = strings[0];
	}

	public override void Execute(AiMemoryNew memory, CombatCharacter combatChar)
	{
		if (!memory.Ints.TryGetValue(_key, out var value) || value < 0)
		{
			return;
		}
		DataContext context = DomainManager.Combat.Context;
		IReadOnlyDictionary<int, sbyte> tricks = combatChar.GetTricks().Tricks;
		List<NeedTrick> trickCost = Config.CombatSkill.Instance[value].TrickCost;
		List<sbyte> list = trickCost.Select((NeedTrick trick) => trick.TrickType).ToList();
		foreach (KeyValuePair<int, sbyte> item in tricks)
		{
			if (!list.Contains(item.Value))
			{
				DomainManager.SpecialEffect.CostTrickDuringPreparingSkill(context, combatChar.GetId(), item.Key);
				break;
			}
		}
	}
}
