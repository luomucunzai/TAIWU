using System;
using GameData.Common;

namespace GameData.Domains.Combat.Ai.Action
{
	// Token: 0x020007C7 RID: 1991
	[AiAction(EAiActionType.InterruptAffectingDefense)]
	public class AiActionInterruptAffectingDefense : AiActionCombatBase
	{
		// Token: 0x06006A58 RID: 27224 RVA: 0x003BC7A0 File Offset: 0x003BA9A0
		public override void Execute(AiMemoryNew memory, CombatCharacter combatChar)
		{
			DataContext context = DomainManager.Combat.Context;
			DomainManager.Combat.ClearAffectingDefenseSkillManual(context, combatChar.IsAlly);
		}
	}
}
