using System;
using System.Collections.Generic;
using GameData.Domains.Character;

namespace GameData.Domains.Combat.Ai.Action
{
	// Token: 0x020007DB RID: 2011
	[AiAction(EAiActionType.MoveToAttackRangeCenter)]
	public class AiActionMoveToAttackRangeCenter : AiActionCombatBase
	{
		// Token: 0x06006A80 RID: 27264 RVA: 0x003BCAD6 File Offset: 0x003BACD6
		public AiActionMoveToAttackRangeCenter(IReadOnlyList<int> ints)
		{
			this._offset = ints[0];
		}

		// Token: 0x06006A81 RID: 27265 RVA: 0x003BCAF0 File Offset: 0x003BACF0
		public override void Execute(AiMemoryNew memory, CombatCharacter combatChar)
		{
			OuterAndInnerShorts attackRange = combatChar.GetAttackRange();
			int center = (int)((attackRange.Outer + attackRange.Inner) / 2);
			combatChar.AiTargetDistance = DomainManager.Combat.GetMoveRangeDistance(center + this._offset);
		}

		// Token: 0x04001D69 RID: 7529
		private readonly int _offset;
	}
}
