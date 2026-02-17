using System;
using System.Collections.Generic;
using GameData.Combat.Math;
using GameData.Domains.Item;
using GameData.Utilities;

namespace GameData.Domains.Combat.Ai.Condition
{
	// Token: 0x02000757 RID: 1879
	[AiCondition(EAiConditionType.UnlockAttackValuePercentMoreOrEqual)]
	public class AiConditionUnlockAttackValuePercentMoreOrEqual : AiConditionCheckCharExpressionBase
	{
		// Token: 0x0600695D RID: 26973 RVA: 0x003B9E00 File Offset: 0x003B8000
		public AiConditionUnlockAttackValuePercentMoreOrEqual(IReadOnlyList<string> strings, IReadOnlyList<int> ints) : base(strings, ints)
		{
			this._weaponTemplateId = (short)ints[1];
		}

		// Token: 0x0600695E RID: 26974 RVA: 0x003B9E1C File Offset: 0x003B801C
		protected override bool Check(CombatCharacter checkChar, int expressionResult)
		{
			int maxPercent = 0;
			ItemKey[] weapons = checkChar.GetWeapons();
			List<int> unlockPrepareValue = checkChar.GetUnlockPrepareValue();
			for (int i = 0; i < 3; i++)
			{
				bool flag = weapons[i].TemplateId == this._weaponTemplateId && unlockPrepareValue.CheckIndex(i);
				if (flag)
				{
					maxPercent = Math.Max(maxPercent, CValuePercent.ParseInt(unlockPrepareValue[i], GlobalConfig.Instance.UnlockAttackUnit));
				}
			}
			return maxPercent >= expressionResult;
		}

		// Token: 0x04001D07 RID: 7431
		private readonly short _weaponTemplateId;
	}
}
