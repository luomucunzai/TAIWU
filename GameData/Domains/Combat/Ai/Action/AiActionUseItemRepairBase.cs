using System;
using System.Collections.Generic;
using GameData.Common;
using GameData.Domains.Item;

namespace GameData.Domains.Combat.Ai.Action
{
	// Token: 0x020007FD RID: 2045
	public abstract class AiActionUseItemRepairBase : AiActionCombatBase
	{
		// Token: 0x06006AC7 RID: 27335 RVA: 0x003BD620 File Offset: 0x003BB820
		public override void Execute(AiMemoryNew memory, CombatCharacter combatChar)
		{
			DataContext context = DomainManager.Combat.Context;
			ValueTuple<ItemKey, ItemKey> valueTuple = combatChar.AiSelectRepairTarget(this.EquipmentSlots);
			ItemKey targetKey = valueTuple.Item1;
			ItemKey toolKey = valueTuple.Item2;
			bool flag = targetKey.IsValid();
			if (flag)
			{
				DomainManager.Combat.RepairItem(context, toolKey, targetKey, combatChar.IsAlly);
			}
		}

		// Token: 0x1700046B RID: 1131
		// (get) Token: 0x06006AC8 RID: 27336
		protected abstract IEnumerable<sbyte> EquipmentSlots { get; }
	}
}
