using System;
using GameData.Common;

namespace GameData.Domains.Combat.Ai.Action
{
	// Token: 0x020007C6 RID: 1990
	[AiAction(EAiActionType.InterruptCasting)]
	public class AiActionInterruptCasting : AiActionCombatBase
	{
		// Token: 0x06006A56 RID: 27222 RVA: 0x003BC76C File Offset: 0x003BA96C
		public override void Execute(AiMemoryNew memory, CombatCharacter combatChar)
		{
			DataContext context = DomainManager.Combat.Context;
			DomainManager.Combat.InterruptSkillManual(context, combatChar.IsAlly);
		}
	}
}
