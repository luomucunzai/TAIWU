using System;
using System.Collections.Generic;
using GameData.Common;

namespace GameData.Domains.Combat.Ai.Action
{
	// Token: 0x020007EC RID: 2028
	[AiAction(EAiActionType.ChangeWeaponIndex)]
	public class AiActionChangeWeaponIndex : AiActionCombatBase
	{
		// Token: 0x06006AA3 RID: 27299 RVA: 0x003BD1EF File Offset: 0x003BB3EF
		public AiActionChangeWeaponIndex(IReadOnlyList<int> ints)
		{
			this._weaponIndex = ints[0];
		}

		// Token: 0x06006AA4 RID: 27300 RVA: 0x003BD208 File Offset: 0x003BB408
		public override void Execute(AiMemoryNew memory, CombatCharacter combatChar)
		{
			DataContext context = DomainManager.Combat.Context;
			DomainManager.Combat.ChangeWeapon(context, this._weaponIndex, combatChar.IsAlly, false);
		}

		// Token: 0x04001D74 RID: 7540
		private readonly int _weaponIndex;
	}
}
