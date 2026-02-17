using System;
using System.Collections.Generic;
using System.Linq;
using Config;
using GameData.Common;

namespace GameData.Domains.Combat.Ai.Action
{
	// Token: 0x020007E8 RID: 2024
	[AiAction(EAiActionType.CostMemoryFirstTrick)]
	public class AiActionCostMemoryFirstTrick : AiActionCombatBase
	{
		// Token: 0x06006A9A RID: 27290 RVA: 0x003BCEA5 File Offset: 0x003BB0A5
		public AiActionCostMemoryFirstTrick(IReadOnlyList<string> strings)
		{
			this._key = strings[0];
		}

		// Token: 0x06006A9B RID: 27291 RVA: 0x003BCEBC File Offset: 0x003BB0BC
		public override void Execute(AiMemoryNew memory, CombatCharacter combatChar)
		{
			int skillId;
			bool flag = !memory.Ints.TryGetValue(this._key, out skillId) || skillId < 0;
			if (!flag)
			{
				DataContext context = DomainManager.Combat.Context;
				IReadOnlyDictionary<int, sbyte> tricks = combatChar.GetTricks().Tricks;
				List<NeedTrick> trickCost = CombatSkill.Instance[skillId].TrickCost;
				List<sbyte> trickTypeList = (from trick in trickCost
				select trick.TrickType).ToList<sbyte>();
				foreach (KeyValuePair<int, sbyte> trick2 in tricks)
				{
					bool flag2 = !trickTypeList.Contains(trick2.Value);
					if (flag2)
					{
						DomainManager.SpecialEffect.CostTrickDuringPreparingSkill(context, combatChar.GetId(), trick2.Key);
						break;
					}
				}
			}
		}

		// Token: 0x04001D70 RID: 7536
		private readonly string _key;
	}
}
