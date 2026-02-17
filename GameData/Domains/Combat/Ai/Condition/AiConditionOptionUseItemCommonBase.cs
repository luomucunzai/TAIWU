using System;
using GameData.Domains.Combat.Ai.Selector;
using GameData.Domains.Item;

namespace GameData.Domains.Combat.Ai.Condition
{
	// Token: 0x020007A2 RID: 1954
	public abstract class AiConditionOptionUseItemCommonBase : AiConditionOptionUseItemBase
	{
		// Token: 0x060069FE RID: 27134 RVA: 0x003BBB61 File Offset: 0x003B9D61
		protected AiConditionOptionUseItemCommonBase(EItemSelectorType selectorType)
		{
			ItemSelectorHelper.Predicates.TryGetValue(selectorType, out this._predicate);
		}

		// Token: 0x060069FF RID: 27135 RVA: 0x003BBB80 File Offset: 0x003B9D80
		protected sealed override bool IsValid(CombatCharacter combatChar, ItemKey itemKey)
		{
			ItemSelectorPredicate predicate = this._predicate;
			return predicate != null && predicate(combatChar, itemKey);
		}

		// Token: 0x04001D40 RID: 7488
		private readonly ItemSelectorPredicate _predicate;
	}
}
