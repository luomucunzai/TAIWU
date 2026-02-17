using System;
using System.Collections.Generic;

namespace GameData.Domains.Combat.Ai.Action
{
	// Token: 0x020007DD RID: 2013
	[AiAction(EAiActionType.MoveToTargetDistance)]
	public class AiActionMoveToTargetDistance : AiActionCombatBase
	{
		// Token: 0x06006A84 RID: 27268 RVA: 0x003BCB72 File Offset: 0x003BAD72
		public AiActionMoveToTargetDistance(IReadOnlyList<int> ints)
		{
			this._target = (short)ints[0];
		}

		// Token: 0x06006A85 RID: 27269 RVA: 0x003BCB8A File Offset: 0x003BAD8A
		public override void Execute(AiMemoryNew memory, CombatCharacter combatChar)
		{
			combatChar.AiTargetDistance = DomainManager.Combat.GetMoveRangeDistance((int)this._target);
		}

		// Token: 0x04001D6A RID: 7530
		private readonly short _target;
	}
}
