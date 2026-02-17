using System;
using System.Collections.Generic;

namespace GameData.Domains.Combat.Ai.Condition
{
	// Token: 0x02000792 RID: 1938
	[AiCondition(EAiConditionType.OptionOtherAction)]
	public class AiConditionOptionOtherAction : AiConditionCombatBase
	{
		// Token: 0x060069D6 RID: 27094 RVA: 0x003BB21A File Offset: 0x003B941A
		public AiConditionOptionOtherAction(IReadOnlyList<int> ints)
		{
			this._otherActionType = (sbyte)ints[0];
		}

		// Token: 0x060069D7 RID: 27095 RVA: 0x003BB234 File Offset: 0x003B9434
		public override bool Check(AiMemoryNew memory, CombatCharacter combatChar)
		{
			sbyte otherActionType = this._otherActionType;
			if (!true)
			{
			}
			int num;
			switch (otherActionType)
			{
			case 0:
				num = 0;
				break;
			case 1:
				num = 1;
				break;
			case 2:
				num = 2;
				break;
			default:
				num = -1;
				break;
			}
			if (!true)
			{
			}
			int index = num;
			bool flag = combatChar.IsAlly && (index < 0 || !DomainManager.Combat.AiOptions.AutoUseOtherAction[index]);
			return !flag && combatChar.GetOtherActionCanUse()[(int)this._otherActionType];
		}

		// Token: 0x04001D36 RID: 7478
		private readonly sbyte _otherActionType;
	}
}
