using System;

namespace GameData.Domains.Combat.Ai.Condition
{
	// Token: 0x0200078A RID: 1930
	[AiCondition(EAiConditionType.OptionChangeTrickFlaw)]
	public class AiConditionOptionChangeTrickFlaw : AiConditionCombatBase
	{
		// Token: 0x060069C4 RID: 27076 RVA: 0x003BAC50 File Offset: 0x003B8E50
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
						int costChangeTrickCount = CFormulaHelper.CalcCostChangeTrickCount(combatChar, EFlawOrAcupointType.Flaw);
						result = ((int)combatChar.GetChangeTrickCount() >= costChangeTrickCount);
					}
				}
			}
			return result;
		}
	}
}
