using System;
using System.Collections.Generic;
using GameData.Domains.Character;

namespace GameData.Domains.Combat.Ai.Action
{
	// Token: 0x020007DF RID: 2015
	[AiAction(EAiActionType.MoveToAttackRangeEdge)]
	public class AiActionMoveToAttackRangeEdge : AiActionCombatBase
	{
		// Token: 0x06006A88 RID: 27272 RVA: 0x003BCBC4 File Offset: 0x003BADC4
		public AiActionMoveToAttackRangeEdge(IReadOnlyList<int> ints)
		{
			this._isForward = (ints[0] == 1);
			this._offset = ints[1];
		}

		// Token: 0x06006A89 RID: 27273 RVA: 0x003BCBEC File Offset: 0x003BADEC
		public override void Execute(AiMemoryNew memory, CombatCharacter combatChar)
		{
			OuterAndInnerShorts attackRange = combatChar.GetAttackRange();
			short edge = this._isForward ? attackRange.Outer : attackRange.Inner;
			combatChar.AiTargetDistance = DomainManager.Combat.GetMoveRangeDistance((int)edge + this._offset);
		}

		// Token: 0x04001D6B RID: 7531
		private readonly bool _isForward;

		// Token: 0x04001D6C RID: 7532
		private readonly int _offset;
	}
}
