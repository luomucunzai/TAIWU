using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using GameData.Common;
using GameData.Domains.Item;

namespace GameData.Domains.Combat.Ai.Action
{
	// Token: 0x020007EE RID: 2030
	[AiAction(EAiActionType.ChangeWeaponType)]
	public class AiActionChangeWeaponType : AiActionCombatBase
	{
		// Token: 0x06006AA9 RID: 27305 RVA: 0x003BD2BC File Offset: 0x003BB4BC
		private bool IsTarget(ItemKey itemKey)
		{
			return ItemTemplateHelper.GetItemSubType(itemKey.ItemType, itemKey.TemplateId) == this._itemSubType;
		}

		// Token: 0x06006AAA RID: 27306 RVA: 0x003BD2D7 File Offset: 0x003BB4D7
		public AiActionChangeWeaponType(IReadOnlyList<int> ints)
		{
			this._itemSubType = (short)ints[0];
		}

		// Token: 0x06006AAB RID: 27307 RVA: 0x003BD2F0 File Offset: 0x003BB4F0
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

		// Token: 0x04001D76 RID: 7542
		private readonly short _itemSubType;
	}
}
