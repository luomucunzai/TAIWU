using System;
using Config;
using GameData.Domains.Combat.Ai.Selector;

namespace GameData.Domains.Combat.Ai.Action
{
	// Token: 0x020007C2 RID: 1986
	[AiAction(EAiActionType.CastSkillDefendBlock)]
	public class AiActionCastSkillDefendBlock : AiActionCastSkillBase
	{
		// Token: 0x17000464 RID: 1124
		// (get) Token: 0x06006A47 RID: 27207 RVA: 0x003BC445 File Offset: 0x003BA645
		protected override CombatSkillSelector Selector { get; } = new CombatSkillSelector(3, new CombatSkillSelectorPredicate(AiActionCastSkillDefendBlock.Predicate), new CombatSkillSelectorComparison(AiActionCastSkillDefendBlock.Comparison));

		// Token: 0x06006A48 RID: 27208 RVA: 0x003BC450 File Offset: 0x003BA650
		private static bool Predicate(CombatSkillItem config)
		{
			return false;
		}

		// Token: 0x06006A49 RID: 27209 RVA: 0x003BC464 File Offset: 0x003BA664
		private static int Comparison(CombatSkillItem configA, CombatSkillItem configB)
		{
			return 0;
		}
	}
}
