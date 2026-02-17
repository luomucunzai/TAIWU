using System;
using System.Collections.Generic;
using GameData.Domains.Item;

namespace GameData.Domains.Combat.Ai.Condition
{
	// Token: 0x0200079B RID: 1947
	[AiCondition(EAiConditionType.OptionUnlockAttackWeapon)]
	public class AiConditionOptionUnlockAttackWeapon : AiConditionCombatBase
	{
		// Token: 0x060069EE RID: 27118 RVA: 0x003BB876 File Offset: 0x003B9A76
		public AiConditionOptionUnlockAttackWeapon(IReadOnlyList<int> ints)
		{
			this._weaponTemplateId = (short)ints[0];
		}

		// Token: 0x060069EF RID: 27119 RVA: 0x003BB890 File Offset: 0x003B9A90
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
					bool flag2 = weapons[i].TemplateId == this._weaponTemplateId && canUnlockAttack[i];
					if (flag2)
					{
						return true;
					}
				}
				result = false;
			}
			return result;
		}

		// Token: 0x04001D3D RID: 7485
		private readonly short _weaponTemplateId;
	}
}
