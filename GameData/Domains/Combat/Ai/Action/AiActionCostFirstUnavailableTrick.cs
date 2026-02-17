using System;
using System.Collections.Generic;
using GameData.Common;

namespace GameData.Domains.Combat.Ai.Action
{
	// Token: 0x020007E7 RID: 2023
	[AiAction(EAiActionType.CostFirstUnavailableTrick)]
	public class AiActionCostFirstUnavailableTrick : AiActionCombatBase
	{
		// Token: 0x06006A98 RID: 27288 RVA: 0x003BCE0C File Offset: 0x003BB00C
		public override void Execute(AiMemoryNew memory, CombatCharacter combatChar)
		{
			DataContext context = DomainManager.Combat.Context;
			IReadOnlyDictionary<int, sbyte> tricks = combatChar.GetTricks().Tricks;
			foreach (KeyValuePair<int, sbyte> trick in tricks)
			{
				bool flag = !combatChar.IsTrickUsable(trick.Value);
				if (flag)
				{
					DomainManager.SpecialEffect.CostTrickDuringPreparingSkill(context, combatChar.GetId(), trick.Key);
					break;
				}
			}
		}
	}
}
