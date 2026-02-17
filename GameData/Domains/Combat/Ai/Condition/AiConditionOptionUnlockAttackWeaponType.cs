using System;
using System.Collections.Generic;
using GameData.Domains.Item;

namespace GameData.Domains.Combat.Ai.Condition
{
	// Token: 0x0200079C RID: 1948
	[AiCondition(EAiConditionType.OptionUnlockAttackWeaponType)]
	public class AiConditionOptionUnlockAttackWeaponType : AiConditionCombatBase
	{
		// Token: 0x060069F0 RID: 27120 RVA: 0x003BB916 File Offset: 0x003B9B16
		public AiConditionOptionUnlockAttackWeaponType(IReadOnlyList<int> ints)
		{
			this._weaponSubType = (short)ints[0];
		}

		// Token: 0x060069F1 RID: 27121 RVA: 0x003BB930 File Offset: 0x003B9B30
		public override bool Check(AiMemoryNew memory, CombatCharacter combatChar)
		{
			bool flag = combatChar.IsAlly && !DomainManager.Combat.AiOptions.AutoUnlock;
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				ItemKey[] weapons = combatChar.GetWeapons();
				List<bool> canUnlockAttack = combatChar.GetCanUnlockAttack();
				for (int i = 0; i < 3; i++)
				{
					bool flag2 = weapons[i].IsValid() && ItemTemplateHelper.GetItemSubType(weapons[i].ItemType, weapons[i].TemplateId) == this._weaponSubType && canUnlockAttack[i];
					if (flag2)
					{
						return true;
					}
				}
				result = false;
			}
			return result;
		}

		// Token: 0x04001D3E RID: 7486
		private readonly short _weaponSubType;
	}
}
