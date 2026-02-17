using System;
using System.Collections.Generic;
using GameData.Common;
using GameData.Domains.Item;

namespace GameData.Domains.Combat.Ai.Action
{
	// Token: 0x020007E6 RID: 2022
	[AiAction(EAiActionType.UnlockAttackWeaponType)]
	public class AiActionUnlockAttackWeaponType : AiActionCombatBase
	{
		// Token: 0x06006A96 RID: 27286 RVA: 0x003BCD50 File Offset: 0x003BAF50
		public AiActionUnlockAttackWeaponType(IReadOnlyList<int> ints)
		{
			this._weaponSubType = (short)ints[0];
		}

		// Token: 0x06006A97 RID: 27287 RVA: 0x003BCD68 File Offset: 0x003BAF68
		public override void Execute(AiMemoryNew memory, CombatCharacter combatChar)
		{
			DataContext context = DomainManager.Combat.Context;
			ItemKey[] weapons = combatChar.GetWeapons();
			List<bool> canUnlockAttack = combatChar.GetCanUnlockAttack();
			for (int i = 0; i < 3; i++)
			{
				bool flag = !weapons[i].IsValid() || ItemTemplateHelper.GetItemSubType(weapons[i].ItemType, weapons[i].TemplateId) != this._weaponSubType;
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

		// Token: 0x04001D6F RID: 7535
		private readonly short _weaponSubType;
	}
}
