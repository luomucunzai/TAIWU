using System;
using Config;
using GameData.Utilities;

namespace GameData.Domains.Combat.Ai.Condition
{
	// Token: 0x02000761 RID: 1889
	[AiCondition(EAiConditionType.CheckBanFlee)]
	public class AiConditionCheckBanFlee : AiConditionCombatBase
	{
		// Token: 0x06006971 RID: 26993 RVA: 0x003BA10C File Offset: 0x003B830C
		public override bool Check(AiMemoryNew memory, CombatCharacter combatChar)
		{
			bool flag = !DomainManager.Combat.IsMainCharacter(combatChar);
			bool result;
			if (flag)
			{
				result = true;
			}
			else
			{
				bool flag2 = !Character.Instance[combatChar.GetCharacter().GetTemplateId()].AllowEscape;
				if (flag2)
				{
					result = true;
				}
				else
				{
					bool flag3 = combatChar.GetCharacter().GetXiangshuType() == 1;
					if (flag3)
					{
						result = true;
					}
					else
					{
						bool flag4 = !DomainManager.Combat.CanFlee(combatChar.IsAlly);
						if (flag4)
						{
							result = true;
						}
						else
						{
							sbyte firm = combatChar.GetPersonalityValue(4);
							result = DomainManager.Combat.Context.Random.CheckPercentProb((int)(30 + 60 * firm / 100));
						}
					}
				}
			}
			return result;
		}
	}
}
