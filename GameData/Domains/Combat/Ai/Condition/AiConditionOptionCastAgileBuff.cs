using System;
using System.Linq;

namespace GameData.Domains.Combat.Ai.Condition
{
	// Token: 0x02000797 RID: 1943
	[AiCondition(EAiConditionType.OptionCastAgileBuff)]
	public class AiConditionOptionCastAgileBuff : AiConditionCombatBase
	{
		// Token: 0x060069E5 RID: 27109 RVA: 0x003BB737 File Offset: 0x003B9937
		private static bool IsValid(short skillId)
		{
			return skillId >= 0;
		}

		// Token: 0x060069E6 RID: 27110 RVA: 0x003BB740 File Offset: 0x003B9940
		public override bool Check(AiMemoryNew memory, CombatCharacter combatChar)
		{
			bool flag = combatChar.IsAlly && !DomainManager.Combat.AiOptions.AutoCastSkill[1];
			return !flag && combatChar.GetAgileSkillList().Where(new Func<short, bool>(AiConditionOptionCastAgileBuff.IsValid)).Where(new Func<short, bool>(combatChar.AiCanCast)).Any<short>();
		}
	}
}
