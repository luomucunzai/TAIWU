using System.Collections.Generic;
using GameData.Common;

namespace GameData.Domains.Combat.Ai.Action;

[AiAction(EAiActionType.CostFirstUnavailableTrick)]
public class AiActionCostFirstUnavailableTrick : AiActionCombatBase
{
	public override void Execute(AiMemoryNew memory, CombatCharacter combatChar)
	{
		DataContext context = DomainManager.Combat.Context;
		IReadOnlyDictionary<int, sbyte> tricks = combatChar.GetTricks().Tricks;
		foreach (KeyValuePair<int, sbyte> item in tricks)
		{
			if (!combatChar.IsTrickUsable(item.Value))
			{
				DomainManager.SpecialEffect.CostTrickDuringPreparingSkill(context, combatChar.GetId(), item.Key);
				break;
			}
		}
	}
}
