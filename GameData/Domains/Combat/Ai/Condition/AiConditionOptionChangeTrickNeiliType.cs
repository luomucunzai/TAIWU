using System;
using System.Collections.Generic;
using System.Linq;

namespace GameData.Domains.Combat.Ai.Condition
{
	// Token: 0x0200078C RID: 1932
	[AiCondition(EAiConditionType.OptionChangeTrickNeiliType)]
	public class AiConditionOptionChangeTrickNeiliType : AiConditionCombatBase
	{
		// Token: 0x060069C8 RID: 27080 RVA: 0x003BADF8 File Offset: 0x003B8FF8
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
					Dictionary<SkillEffectKey, short> effectDict = combatChar.GetSkillEffectCollection().EffectDict;
					bool flag3;
					if (effectDict != null)
					{
						flag3 = effectDict.Keys.Any((SkillEffectKey x) => x.EffectConfig.TransferProportion > 0);
					}
					else
					{
						flag3 = false;
					}
					result = flag3;
				}
			}
			return result;
		}
	}
}
