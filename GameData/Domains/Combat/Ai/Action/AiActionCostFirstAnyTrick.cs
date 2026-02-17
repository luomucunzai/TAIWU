using System;
using System.Collections.Generic;
using GameData.Common;

namespace GameData.Domains.Combat.Ai.Action
{
	// Token: 0x020007E9 RID: 2025
	[AiAction(EAiActionType.CostFirstAnyTrick)]
	public class AiActionCostFirstAnyTrick : AiActionCombatBase
	{
		// Token: 0x06006A9C RID: 27292 RVA: 0x003BCFB8 File Offset: 0x003BB1B8
		public override void Execute(AiMemoryNew memory, CombatCharacter combatChar)
		{
			DataContext context = DomainManager.Combat.Context;
			IReadOnlyDictionary<int, sbyte> tricks = combatChar.GetTricks().Tricks;
			using (IEnumerator<KeyValuePair<int, sbyte>> enumerator = tricks.GetEnumerator())
			{
				if (enumerator.MoveNext())
				{
					KeyValuePair<int, sbyte> trick = enumerator.Current;
					DomainManager.SpecialEffect.CostTrickDuringPreparingSkill(context, combatChar.GetId(), trick.Key);
				}
			}
		}
	}
}
