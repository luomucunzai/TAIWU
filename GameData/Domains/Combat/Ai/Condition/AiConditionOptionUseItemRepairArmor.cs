using System;
using System.Collections.Generic;

namespace GameData.Domains.Combat.Ai.Condition
{
	// Token: 0x020007AD RID: 1965
	[AiCondition(EAiConditionType.OptionUseItemRepairArmor)]
	public class AiConditionOptionUseItemRepairArmor : AiConditionUseItemRepairBase
	{
		// Token: 0x1700045B RID: 1115
		// (get) Token: 0x06006A10 RID: 27152 RVA: 0x003BBD0C File Offset: 0x003B9F0C
		protected override IEnumerable<sbyte> EquipmentSlots
		{
			get
			{
				yield return 3;
				yield return 5;
				yield return 6;
				yield return 7;
				yield break;
			}
		}
	}
}
