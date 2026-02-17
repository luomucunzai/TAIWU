using System;
using System.Collections.Generic;
using GameData.Domains.Item;
using GameData.Utilities;

namespace GameData.Domains.Combat.Ai.Condition
{
	// Token: 0x0200078E RID: 1934
	[AiCondition(EAiConditionType.OptionChangeWeaponIndex)]
	public class AiConditionOptionChangeWeaponIndex : AiConditionCombatBase
	{
		// Token: 0x060069CC RID: 27084 RVA: 0x003BAF47 File Offset: 0x003B9147
		public AiConditionOptionChangeWeaponIndex(IReadOnlyList<int> ints)
		{
			this._weaponIndex = ints[0];
		}

		// Token: 0x060069CD RID: 27085 RVA: 0x003BAF60 File Offset: 0x003B9160
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
					ItemKey[] weapons = combatChar.GetWeapons();
					bool flag3 = !weapons.CheckIndex(this._weaponIndex);
					if (flag3)
					{
						result = false;
					}
					else
					{
						ItemKey weaponKey = weapons[this._weaponIndex];
						bool flag4 = !weaponKey.IsValid();
						if (flag4)
						{
							result = false;
						}
						else
						{
							CombatWeaponData weaponData = DomainManager.Combat.GetElement_WeaponDataDict(weaponKey.Id);
							result = weaponData.GetCanChangeTo();
						}
					}
				}
			}
			return result;
		}

		// Token: 0x04001D33 RID: 7475
		private readonly int _weaponIndex;
	}
}
