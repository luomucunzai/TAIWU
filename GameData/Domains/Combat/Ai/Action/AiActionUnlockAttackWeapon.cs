using System;
using System.Collections.Generic;
using GameData.Common;
using GameData.Domains.Item;

namespace GameData.Domains.Combat.Ai.Action
{
	// Token: 0x020007E5 RID: 2021
	[AiAction(EAiActionType.UnlockAttackWeapon)]
	public class AiActionUnlockAttackWeapon : AiActionCombatBase
	{
		// Token: 0x06006A94 RID: 27284 RVA: 0x003BCCB6 File Offset: 0x003BAEB6
		public AiActionUnlockAttackWeapon(IReadOnlyList<int> ints)
		{
			this._weaponTemplateId = (short)ints[0];
		}

		// Token: 0x06006A95 RID: 27285 RVA: 0x003BCCD0 File Offset: 0x003BAED0
		public override void Execute(AiMemoryNew memory, CombatCharacter combatChar)
		{
			DataContext context = DomainManager.Combat.Context;
			ItemKey[] weapons = combatChar.GetWeapons();
			List<bool> canUnlockAttack = combatChar.GetCanUnlockAttack();
			for (int i = 0; i < 3; i++)
			{
				bool flag = weapons[i].TemplateId != this._weaponTemplateId;
				if (!flag)
				{
					bool flag2 = !canUnlockAttack[i];
					if (!flag2)
					{
						DomainManager.Combat.UnlockAttack(context, i, combatChar.IsAlly);
						break;
					}
				}
			}
		}

		// Token: 0x04001D6E RID: 7534
		private readonly short _weaponTemplateId;
	}
}
