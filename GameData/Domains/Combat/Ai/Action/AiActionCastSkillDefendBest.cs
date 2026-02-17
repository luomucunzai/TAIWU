using System;
using GameData.Domains.Combat.Ai.Selector;

namespace GameData.Domains.Combat.Ai.Action
{
	// Token: 0x020007C1 RID: 1985
	[AiAction(EAiActionType.CastSkillDefendBest)]
	public class AiActionCastSkillDefendBest : AiActionCastSkillBase
	{
		// Token: 0x17000463 RID: 1123
		// (get) Token: 0x06006A45 RID: 27205 RVA: 0x003BC428 File Offset: 0x003BA628
		protected override CombatSkillSelector Selector { get; } = new CombatSkillSelector(3);
	}
}
