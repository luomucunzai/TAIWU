using System;
using System.Linq;

namespace GameData.Domains.Combat.Ai.Condition
{
	// Token: 0x02000796 RID: 1942
	[AiCondition(EAiConditionType.OptionCastDefendBlock)]
	public class AiConditionOptionCastDefendBlock : AiConditionCombatBase
	{
		// Token: 0x060069E2 RID: 27106 RVA: 0x003BB6C3 File Offset: 0x003B98C3
		private static bool IsValid(short skillId)
		{
			return false;
		}

		// Token: 0x060069E3 RID: 27107 RVA: 0x003BB6C8 File Offset: 0x003B98C8
		public override bool Check(AiMemoryNew memory, CombatCharacter combatChar)
		{
			bool flag = combatChar.IsAlly && !DomainManager.Combat.AiOptions.AutoCastSkill[2];
			return !flag && combatChar.GetDefenceSkillList().Where(new Func<short, bool>(AiConditionOptionCastDefendBlock.IsValid)).Where(new Func<short, bool>(combatChar.AiCanCast)).Any<short>();
		}
	}
}
