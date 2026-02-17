using System;
using System.Collections.Generic;
using GameData.Common;

namespace GameData.Domains.Combat.Ai.Action
{
	// Token: 0x020007EB RID: 2027
	[AiAction(EAiActionType.ChangeWeaponAuto)]
	public class AiActionChangeWeaponAuto : AiActionCombatBase
	{
		// Token: 0x06006AA1 RID: 27297 RVA: 0x003BD17E File Offset: 0x003BB37E
		public AiActionChangeWeaponAuto(IReadOnlyList<int> ints)
		{
			this._minIndex = ints[0];
			this._maxIndex = ints[1];
		}

		// Token: 0x06006AA2 RID: 27298 RVA: 0x003BD1A4 File Offset: 0x003BB3A4
		public override void Execute(AiMemoryNew memory, CombatCharacter combatChar)
		{
			DataContext context = DomainManager.Combat.Context;
			int index = combatChar.AiGetFirstChangeableWeaponIndex(this._minIndex, this._maxIndex);
			bool flag = index >= 0;
			if (flag)
			{
				DomainManager.Combat.ChangeWeapon(context, index, combatChar.IsAlly, false);
			}
		}

		// Token: 0x04001D72 RID: 7538
		private readonly int _minIndex;

		// Token: 0x04001D73 RID: 7539
		private readonly int _maxIndex;
	}
}
