using System;

namespace GameData.Domains.Combat.Ai.Condition
{
	// Token: 0x020007A0 RID: 1952
	[AiCondition(EAiConditionType.OptionAutoCostTrick)]
	public class AiConditionOptionAutoCostTrick : AiConditionCombatBase
	{
		// Token: 0x060069F8 RID: 27128 RVA: 0x003BBABC File Offset: 0x003B9CBC
		public override bool Check(AiMemoryNew memory, CombatCharacter combatChar)
		{
			bool flag = combatChar.IsAlly && !DomainManager.Combat.AiOptions.AutoCostTrick;
			return !flag && DomainManager.SpecialEffect.CanCostTrickDuringPreparingSkill(combatChar.GetId(), combatChar.GetPreparingSkillId());
		}
	}
}
