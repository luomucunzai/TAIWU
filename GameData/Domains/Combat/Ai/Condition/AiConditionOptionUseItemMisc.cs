using System;
using System.Collections.Generic;
using GameData.Domains.Item;

namespace GameData.Domains.Combat.Ai.Condition
{
	// Token: 0x020007AA RID: 1962
	[AiCondition(EAiConditionType.OptionUseItemMisc)]
	public class AiConditionOptionUseItemMisc : AiConditionOptionUseItemBase
	{
		// Token: 0x06006A09 RID: 27145 RVA: 0x003BBC5C File Offset: 0x003B9E5C
		public AiConditionOptionUseItemMisc(IReadOnlyList<int> ints)
		{
			this._templateId = (short)ints[0];
		}

		// Token: 0x06006A0A RID: 27146 RVA: 0x003BBC74 File Offset: 0x003B9E74
		protected override bool IsValid(CombatCharacter combatChar, ItemKey itemKey)
		{
			return itemKey.ItemType == 12 && itemKey.TemplateId == this._templateId;
		}

		// Token: 0x04001D41 RID: 7489
		private readonly short _templateId;
	}
}
