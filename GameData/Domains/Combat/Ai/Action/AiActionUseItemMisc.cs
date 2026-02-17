using System;
using System.Collections.Generic;
using GameData.Domains.Item;

namespace GameData.Domains.Combat.Ai.Action
{
	// Token: 0x020007FC RID: 2044
	[AiAction(EAiActionType.UseItemMisc)]
	public class AiActionUseItemMisc : AiActionUseItemBase
	{
		// Token: 0x06006AC5 RID: 27333 RVA: 0x003BD5D6 File Offset: 0x003BB7D6
		public AiActionUseItemMisc(IReadOnlyList<int> ints)
		{
			this._templateId = (short)ints[0];
		}

		// Token: 0x06006AC6 RID: 27334 RVA: 0x003BD5F0 File Offset: 0x003BB7F0
		protected override bool IsValid(CombatCharacter combatChar, ItemKey itemKey)
		{
			return itemKey.ItemType == 12 && itemKey.TemplateId == this._templateId;
		}

		// Token: 0x04001D7D RID: 7549
		private readonly short _templateId;
	}
}
