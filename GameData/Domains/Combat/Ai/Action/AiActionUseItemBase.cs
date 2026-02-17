using System;
using GameData.Common;
using GameData.Domains.Combat.Ai.Selector;
using GameData.Domains.Item;

namespace GameData.Domains.Combat.Ai.Action
{
	// Token: 0x020007F3 RID: 2035
	public abstract class AiActionUseItemBase : AiActionCombatBase
	{
		// Token: 0x06006AB5 RID: 27317 RVA: 0x003BD4B2 File Offset: 0x003BB6B2
		protected AiActionUseItemBase()
		{
			this._selector = new ItemSelector(new ItemSelectorPredicate(this.IsValid), new ItemSelectorComparisonIsPrefer(this.IsPrefer));
		}

		// Token: 0x17000469 RID: 1129
		// (get) Token: 0x06006AB6 RID: 27318 RVA: 0x003BD4E0 File Offset: 0x003BB6E0
		protected virtual sbyte UseType
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x06006AB7 RID: 27319
		protected abstract bool IsValid(CombatCharacter combatChar, ItemKey itemKey);

		// Token: 0x06006AB8 RID: 27320 RVA: 0x003BD4E3 File Offset: 0x003BB6E3
		protected virtual bool IsPrefer(CombatCharacter combatChar, ItemKey itemKey)
		{
			return false;
		}

		// Token: 0x06006AB9 RID: 27321 RVA: 0x003BD4E8 File Offset: 0x003BB6E8
		public sealed override void Execute(AiMemoryNew memory, CombatCharacter combatChar)
		{
			DataContext context = DomainManager.Combat.Context;
			ItemKey itemKey = this._selector.Select(context.Random, combatChar);
			bool flag = itemKey.IsValid();
			if (flag)
			{
				DomainManager.Combat.UseItem(context, itemKey, this.UseType, combatChar.IsAlly, null);
			}
		}

		// Token: 0x04001D7B RID: 7547
		private readonly ItemSelector _selector;
	}
}
