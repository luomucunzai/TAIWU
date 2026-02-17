using System;
using System.Collections.Generic;

namespace GameData.Domains.Combat.Ai.Condition
{
	// Token: 0x0200078D RID: 1933
	[AiCondition(EAiConditionType.OptionChangeWeapon)]
	public class AiConditionOptionChangeWeapon : AiConditionCombatBase
	{
		// Token: 0x060069CA RID: 27082 RVA: 0x003BAE99 File Offset: 0x003B9099
		public AiConditionOptionChangeWeapon(IReadOnlyList<int> ints)
		{
			this._minIndex = ints[0];
			this._maxIndex = ints[1];
		}

		// Token: 0x060069CB RID: 27083 RVA: 0x003BAEC0 File Offset: 0x003B90C0
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
					bool flag3 = DomainManager.Combat.InAttackRange(combatChar);
					result = (!flag3 && combatChar.AiGetFirstChangeableWeaponIndex(this._minIndex, this._maxIndex) >= 0);
				}
			}
			return result;
		}

		// Token: 0x04001D31 RID: 7473
		private readonly int _minIndex;

		// Token: 0x04001D32 RID: 7474
		private readonly int _maxIndex;
	}
}
