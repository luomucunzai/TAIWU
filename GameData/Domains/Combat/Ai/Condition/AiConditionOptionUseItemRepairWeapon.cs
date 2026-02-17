using System;
using System.Collections.Generic;

namespace GameData.Domains.Combat.Ai.Condition
{
	// Token: 0x020007AC RID: 1964
	[AiCondition(EAiConditionType.OptionUseItemRepairWeapon)]
	public class AiConditionOptionUseItemRepairWeapon : AiConditionUseItemRepairBase
	{
		// Token: 0x1700045A RID: 1114
		// (get) Token: 0x06006A0E RID: 27150 RVA: 0x003BBCE4 File Offset: 0x003B9EE4
		protected override IEnumerable<sbyte> EquipmentSlots
		{
			get
			{
				yield return 0;
				yield return 1;
				yield return 2;
				yield break;
			}
		}
	}
}
