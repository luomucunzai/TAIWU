using System;
using GameData.Domains.Combat.Ai.Selector;
using GameData.Domains.Item;

namespace GameData.Domains.Combat.Ai.Condition
{
	// Token: 0x020007A1 RID: 1953
	public abstract class AiConditionOptionUseItemBase : AiConditionCombatBase
	{
		// Token: 0x060069FA RID: 27130 RVA: 0x003BBB12 File Offset: 0x003B9D12
		protected AiConditionOptionUseItemBase()
		{
			this._selector = new ItemSelector(new ItemSelectorPredicate(this.IsValid));
		}

		// Token: 0x060069FB RID: 27131 RVA: 0x003BBB34 File Offset: 0x003B9D34
		public sealed override bool Check(AiMemoryNew memory, CombatCharacter combatChar)
		{
			return this.ExtraCheck(combatChar) && this._selector.AnyMatch(combatChar);
		}

		// Token: 0x060069FC RID: 27132 RVA: 0x003BBB5E File Offset: 0x003B9D5E
		protected virtual bool ExtraCheck(CombatCharacter combatChar)
		{
			return true;
		}

		// Token: 0x060069FD RID: 27133
		protected abstract bool IsValid(CombatCharacter combatChar, ItemKey itemKey);

		// Token: 0x04001D3F RID: 7487
		private readonly ItemSelector _selector;
	}
}
