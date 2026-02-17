using System;
using System.Collections.Generic;

namespace GameData.Domains.Combat.Ai.Condition
{
	// Token: 0x020007AB RID: 1963
	public abstract class AiConditionUseItemRepairBase : AiConditionCombatBase
	{
		// Token: 0x06006A0B RID: 27147 RVA: 0x003BBCA4 File Offset: 0x003B9EA4
		public override bool Check(AiMemoryNew memory, CombatCharacter combatChar)
		{
			return !combatChar.IsAlly && combatChar.AiSelectRepairTarget(this.EquipmentSlots).Item1.IsValid();
		}

		// Token: 0x17000459 RID: 1113
		// (get) Token: 0x06006A0C RID: 27148
		protected abstract IEnumerable<sbyte> EquipmentSlots { get; }
	}
}
