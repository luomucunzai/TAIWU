using System;
using System.Collections.Generic;

namespace GameData.Domains.Combat.Ai.Action
{
	// Token: 0x020007FE RID: 2046
	[AiAction(EAiActionType.UseItemRepairWeapon)]
	public class AiActionUseItemRepairWeapon : AiActionUseItemRepairBase
	{
		// Token: 0x1700046C RID: 1132
		// (get) Token: 0x06006ACA RID: 27338 RVA: 0x003BD67C File Offset: 0x003BB87C
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
