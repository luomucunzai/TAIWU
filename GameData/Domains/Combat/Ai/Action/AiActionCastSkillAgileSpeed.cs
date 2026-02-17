using System;
using Config;
using GameData.Domains.Combat.Ai.Selector;

namespace GameData.Domains.Combat.Ai.Action
{
	// Token: 0x020007C4 RID: 1988
	[AiAction(EAiActionType.CastSkillAgileSpeed)]
	public class AiActionCastSkillAgileSpeed : AiActionCastSkillBase
	{
		// Token: 0x17000466 RID: 1126
		// (get) Token: 0x06006A4F RID: 27215 RVA: 0x003BC544 File Offset: 0x003BA744
		protected override CombatSkillSelector Selector { get; } = new CombatSkillSelector(2, new CombatSkillSelectorPredicate(AiActionCastSkillAgileSpeed.Predicate), new CombatSkillSelectorComparison(AiActionCastSkillAgileSpeed.Comparison));

		// Token: 0x06006A50 RID: 27216 RVA: 0x003BC54C File Offset: 0x003BA74C
		private static bool Predicate(CombatSkillItem config)
		{
			return true;
		}

		// Token: 0x06006A51 RID: 27217 RVA: 0x003BC550 File Offset: 0x003BA750
		private static int Comparison(CombatSkillItem configA, CombatSkillItem configB)
		{
			bool flag = configA.AddPercentMoveSpeedOnCast != configB.AddPercentMoveSpeedOnCast;
			int result;
			if (flag)
			{
				result = configB.AddPercentMoveSpeedOnCast.CompareTo(configA.AddPercentMoveSpeedOnCast);
			}
			else
			{
				result = ((configA.AddMoveSpeedOnCast != configB.AddMoveSpeedOnCast) ? configB.AddMoveSpeedOnCast.CompareTo(configA.AddMoveSpeedOnCast) : 0);
			}
			return result;
		}
	}
}
