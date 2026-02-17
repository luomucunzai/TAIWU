using System;
using GameData.Domains.Combat.Ai.Selector;

namespace GameData.Domains.Combat.Ai.Action
{
	// Token: 0x020007C0 RID: 1984
	[AiAction(EAiActionType.CastSkillAttackBest)]
	public class AiActionCastSkillAttackBest : AiActionCastSkillBase
	{
		// Token: 0x17000462 RID: 1122
		// (get) Token: 0x06006A43 RID: 27203 RVA: 0x003BC40B File Offset: 0x003BA60B
		protected override CombatSkillSelector Selector { get; } = new CombatSkillSelector(1);
	}
}
