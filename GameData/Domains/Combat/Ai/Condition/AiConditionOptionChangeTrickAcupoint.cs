using System;
using System.Collections.Generic;

namespace GameData.Domains.Combat.Ai.Condition
{
	// Token: 0x0200078B RID: 1931
	[AiCondition(EAiConditionType.OptionChangeTrickAcupoint)]
	public class AiConditionOptionChangeTrickAcupoint : AiConditionCombatBase
	{
		// Token: 0x060069C6 RID: 27078 RVA: 0x003BACF1 File Offset: 0x003B8EF1
		public AiConditionOptionChangeTrickAcupoint(IReadOnlyList<int> ints)
		{
			this._bodyPart = (sbyte)ints[0];
		}

		// Token: 0x060069C7 RID: 27079 RVA: 0x003BAD08 File Offset: 0x003B8F08
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
				bool flag2 = combatChar.IsAlly && !DomainManager.Combat.AiOptions.AutoChangeTrick;
				if (flag2)
				{
					result = false;
				}
				else
				{
					bool flag3 = !DomainManager.Combat.CanNormalAttack(combatChar.IsAlly) || !combatChar.GetCanChangeTrick();
					if (flag3)
					{
						result = false;
					}
					else
					{
						CombatCharacter enemyChar = DomainManager.Combat.GetCombatCharacter(!combatChar.IsAlly, false);
						bool flag4 = (int)enemyChar.GetAcupointCount()[(int)this._bodyPart] >= enemyChar.GetMaxAcupointCount();
						if (flag4)
						{
							result = false;
						}
						else
						{
							bool flag5 = !enemyChar.ContainsBodyPart(this._bodyPart);
							if (flag5)
							{
								result = false;
							}
							else
							{
								int costChangeTrickCount = CFormulaHelper.CalcCostChangeTrickCount(combatChar, EFlawOrAcupointType.Acupoint);
								result = ((int)combatChar.GetChangeTrickCount() >= costChangeTrickCount);
							}
						}
					}
				}
			}
			return result;
		}

		// Token: 0x04001D30 RID: 7472
		private readonly sbyte _bodyPart;
	}
}
