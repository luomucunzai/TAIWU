using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using GameData.Domains.Item;

namespace GameData.Domains.Combat.Ai.Condition
{
	// Token: 0x02000790 RID: 1936
	[AiCondition(EAiConditionType.OptionChangeWeaponType)]
	public class AiConditionOptionChangeWeaponType : AiConditionCombatBase
	{
		// Token: 0x060069D1 RID: 27089 RVA: 0x003BB0E4 File Offset: 0x003B92E4
		private bool IsTarget(ItemKey itemKey)
		{
			return ItemTemplateHelper.GetItemSubType(itemKey.ItemType, itemKey.TemplateId) == this._itemSubType;
		}

		// Token: 0x060069D2 RID: 27090 RVA: 0x003BB0FF File Offset: 0x003B92FF
		public AiConditionOptionChangeWeaponType(IReadOnlyList<int> ints)
		{
			this._itemSubType = (short)ints[0];
		}

		// Token: 0x060069D3 RID: 27091 RVA: 0x003BB118 File Offset: 0x003B9318
		public override bool Check(AiMemoryNew memory, CombatCharacter combatChar)
		{
			bool flag = combatChar.IsAlly && !DomainManager.Combat.AiOptions.AutoAttack;
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				bool flag2 = combatChar.IsAlly && !DomainManager.Combat.AiOptions.AutoChangeWeapon;
				if (flag2)
				{
					result = false;
				}
				else
				{
					result = (from x in combatChar.AiCanChangeToWeapons()
					select x.Item1).Where(new Func<ItemKey, bool>(this.IsTarget)).Any<ItemKey>();
				}
			}
			return result;
		}

		// Token: 0x04001D35 RID: 7477
		private readonly short _itemSubType;
	}
}
