using System;
using GameData.Common;
using GameData.Domains.Combat.Ai.Selector;

namespace GameData.Domains.Combat.Ai.Action
{
	// Token: 0x020007BA RID: 1978
	public abstract class AiActionCastSkillBase : AiActionCombatBase
	{
		// Token: 0x17000461 RID: 1121
		// (get) Token: 0x06006A33 RID: 27187
		protected abstract CombatSkillSelector Selector { get; }

		// Token: 0x06006A34 RID: 27188 RVA: 0x003BC218 File Offset: 0x003BA418
		public override void Execute(AiMemoryNew memory, CombatCharacter combatChar)
		{
			DataContext context = DomainManager.Combat.Context;
			short skillId = this.Selector.Select(memory, combatChar);
			bool flag = skillId > 0;
			if (flag)
			{
				DomainManager.Combat.StartPrepareSkill(context, skillId, combatChar.IsAlly);
			}
		}
	}
}
