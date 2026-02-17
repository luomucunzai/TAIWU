using System;
using Config;
using GameData.Domains.Combat.Ai.Selector;
using GameData.Utilities;

namespace GameData.Domains.Combat.Ai.Action
{
	// Token: 0x020007C3 RID: 1987
	[AiAction(EAiActionType.CastSkillAgileBuff)]
	public class AiActionCastSkillAgileBuff : AiActionCastSkillBase
	{
		// Token: 0x17000465 RID: 1125
		// (get) Token: 0x06006A4B RID: 27211 RVA: 0x003BC4A4 File Offset: 0x003BA6A4
		protected override CombatSkillSelector Selector { get; } = new CombatSkillSelector(2, new CombatSkillSelectorPredicate(AiActionCastSkillAgileBuff.Predicate), new CombatSkillSelectorComparison(AiActionCastSkillAgileBuff.Comparison));

		// Token: 0x06006A4C RID: 27212 RVA: 0x003BC4AC File Offset: 0x003BA6AC
		private static bool Predicate(CombatSkillItem config)
		{
			return config.AddHitOnCast.Sum() > 0;
		}

		// Token: 0x06006A4D RID: 27213 RVA: 0x003BC4CC File Offset: 0x003BA6CC
		private static int Comparison(CombatSkillItem configA, CombatSkillItem configB)
		{
			return (configA.AddHitOnCast.Sum() != configB.AddHitOnCast.Sum()) ? configB.AddHitOnCast.Sum().CompareTo(configA.AddHitOnCast.Sum()) : 0;
		}
	}
}
