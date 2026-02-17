using System;
using GameData.Common;

namespace GameData.Domains.Combat.Ai.Action
{
	// Token: 0x020007E0 RID: 2016
	[AiAction(EAiActionType.NormalAttack)]
	public class AiActionNormalAttack : AiActionCombatBase
	{
		// Token: 0x06006A8A RID: 27274 RVA: 0x003BCC30 File Offset: 0x003BAE30
		public override void Execute(AiMemoryNew memory, CombatCharacter combatChar)
		{
			DataContext context = DomainManager.Combat.Context;
			DomainManager.Combat.NormalAttackImmediate(context, combatChar.IsAlly);
		}
	}
}
