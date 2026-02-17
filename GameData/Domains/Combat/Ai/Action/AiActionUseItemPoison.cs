using System;
using GameData.Domains.Character;
using GameData.Domains.Combat.Ai.Selector;
using GameData.Domains.Item;

namespace GameData.Domains.Combat.Ai.Action
{
	// Token: 0x020007F9 RID: 2041
	[AiAction(EAiActionType.UseItemPoison)]
	public class AiActionUseItemPoison : AiActionUseItemCommonBase
	{
		// Token: 0x1700046A RID: 1130
		// (get) Token: 0x06006AC0 RID: 27328 RVA: 0x003BD5AA File Offset: 0x003BB7AA
		protected override sbyte UseType
		{
			get
			{
				return 1;
			}
		}

		// Token: 0x06006AC1 RID: 27329 RVA: 0x003BD5AD File Offset: 0x003BB7AD
		public AiActionUseItemPoison() : base(EItemSelectorType.ThrowPoison)
		{
		}

		// Token: 0x06006AC2 RID: 27330 RVA: 0x003BD5B8 File Offset: 0x003BB7B8
		protected override bool IsPrefer(CombatCharacter combatChar, ItemKey itemKey)
		{
			return EatingItems.IsWugKing(itemKey);
		}
	}
}
