using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using GameData.Domains.Item;

namespace GameData.Domains.Combat.Ai.Condition
{
	// Token: 0x0200078F RID: 1935
	[AiCondition(EAiConditionType.OptionChangeWeaponSpecial)]
	public class AiConditionOptionChangeWeaponSpecial : AiConditionCombatBase
	{
		// Token: 0x060069CE RID: 27086 RVA: 0x003BB01F File Offset: 0x003B921F
		private bool IsTarget(ItemKey itemKey)
		{
			return itemKey.TemplateId == this._weaponTemplateId;
		}

		// Token: 0x060069CF RID: 27087 RVA: 0x003BB02F File Offset: 0x003B922F
		public AiConditionOptionChangeWeaponSpecial(IReadOnlyList<int> ints)
		{
			this._weaponTemplateId = (short)ints[0];
		}

		// Token: 0x060069D0 RID: 27088 RVA: 0x003BB048 File Offset: 0x003B9248
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

		// Token: 0x04001D34 RID: 7476
		private readonly short _weaponTemplateId;
	}
}
