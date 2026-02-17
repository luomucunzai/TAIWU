using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using GameData.Common;
using GameData.Domains.Item;

namespace GameData.Domains.Combat.Ai.Action
{
	// Token: 0x020007ED RID: 2029
	[AiAction(EAiActionType.ChangeWeaponSpecial)]
	public class AiActionChangeWeaponSpecial : AiActionCombatBase
	{
		// Token: 0x06006AA5 RID: 27301 RVA: 0x003BD23A File Offset: 0x003BB43A
		private bool IsTarget(ItemKey weaponKey)
		{
			return weaponKey.TemplateId == this._weaponTemplateId;
		}

		// Token: 0x06006AA6 RID: 27302 RVA: 0x003BD24A File Offset: 0x003BB44A
		public AiActionChangeWeaponSpecial(IReadOnlyList<int> ints)
		{
			this._weaponTemplateId = (short)ints[0];
		}

		// Token: 0x06006AA7 RID: 27303 RVA: 0x003BD264 File Offset: 0x003BB464
		public override void Execute(AiMemoryNew memory, CombatCharacter combatChar)
		{
			DataContext context = DomainManager.Combat.Context;
			int index = combatChar.AiCanChangeToWeapons().First(([TupleElementNames(new string[]
			{
				"weaponKey",
				"index"
			})] ValueTuple<ItemKey, int> x) => this.IsTarget(x.Item1)).Item2;
			DomainManager.Combat.ChangeWeapon(context, index, combatChar.IsAlly, false);
		}

		// Token: 0x04001D75 RID: 7541
		private readonly short _weaponTemplateId;
	}
}
