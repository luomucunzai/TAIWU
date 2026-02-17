using System;
using GameData.Common;

namespace GameData.Domains.Combat.Ai.Action
{
	// Token: 0x020007C8 RID: 1992
	[AiAction(EAiActionType.InterruptAffectingMove)]
	public class AiActionInterruptAffectingMove : AiActionCombatBase
	{
		// Token: 0x06006A5A RID: 27226 RVA: 0x003BC7D4 File Offset: 0x003BA9D4
		public override void Execute(AiMemoryNew memory, CombatCharacter combatChar)
		{
			DataContext context = DomainManager.Combat.Context;
			DomainManager.Combat.ClearAffectingMoveSkillManual(context, combatChar.IsAlly);
		}
	}
}
