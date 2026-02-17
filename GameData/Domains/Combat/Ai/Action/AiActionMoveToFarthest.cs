using System;

namespace GameData.Domains.Combat.Ai.Action
{
	// Token: 0x020007DE RID: 2014
	[AiAction(EAiActionType.MoveToFarthest)]
	public class AiActionMoveToFarthest : AiActionCombatBase
	{
		// Token: 0x06006A86 RID: 27270 RVA: 0x003BCBA3 File Offset: 0x003BADA3
		public override void Execute(AiMemoryNew memory, CombatCharacter combatChar)
		{
			combatChar.AiTargetDistance = (short)DomainManager.Combat.GetDistanceRange().Item2;
		}
	}
}
