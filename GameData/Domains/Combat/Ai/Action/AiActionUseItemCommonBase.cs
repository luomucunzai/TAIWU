using System;
using GameData.Domains.Combat.Ai.Selector;
using GameData.Domains.Item;

namespace GameData.Domains.Combat.Ai.Action
{
	// Token: 0x020007F4 RID: 2036
	public abstract class AiActionUseItemCommonBase : AiActionUseItemBase
	{
		// Token: 0x06006ABA RID: 27322 RVA: 0x003BD539 File Offset: 0x003BB739
		protected AiActionUseItemCommonBase(EItemSelectorType selectorType)
		{
			ItemSelectorHelper.Predicates.TryGetValue(selectorType, out this._predicate);
		}

		// Token: 0x06006ABB RID: 27323 RVA: 0x003BD558 File Offset: 0x003BB758
		protected sealed override bool IsValid(CombatCharacter combatChar, ItemKey itemKey)
		{
			ItemSelectorPredicate predicate = this._predicate;
			return predicate != null && predicate(combatChar, itemKey);
		}

		// Token: 0x04001D7C RID: 7548
		private readonly ItemSelectorPredicate _predicate;
	}
}
