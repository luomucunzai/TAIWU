using System;
using System.Collections.Generic;

namespace GameData.Domains.Combat.Ai.Action
{
	// Token: 0x020007FF RID: 2047
	[AiAction(EAiActionType.UseItemRepairArmor)]
	public class AiActionUseItemRepairArmor : AiActionUseItemRepairBase
	{
		// Token: 0x1700046D RID: 1133
		// (get) Token: 0x06006ACC RID: 27340 RVA: 0x003BD6A4 File Offset: 0x003BB8A4
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
