using System;

namespace GameData.Domains.Combat.Ai.Action
{
	// Token: 0x020007DC RID: 2012
	[AiAction(EAiActionType.MoveToNearbyEscape)]
	public class AiActionMoveToNearbyEscape : AiActionCombatBase
	{
		// Token: 0x06006A82 RID: 27266 RVA: 0x003BCB30 File Offset: 0x003BAD30
		public override void Execute(AiMemoryNew memory, CombatCharacter combatChar)
		{
			CombatCharacter enemyChar = DomainManager.Combat.GetCombatCharacter(!combatChar.IsAlly, false);
			combatChar.AiTargetDistance = DomainManager.Combat.GetNearlyOutDistance(enemyChar.GetAttackRange());
		}
	}
}
