using System;
using System.Collections.Generic;

namespace GameData.Domains.Combat.Ai.Condition
{
	// Token: 0x020007B2 RID: 1970
	[AiCondition(EAiConditionType.AttackRangeEdgeMore)]
	public class AiConditionAttackRangeEdgeMore : AiConditionCheckCharBase
	{
		// Token: 0x06006A1A RID: 27162 RVA: 0x003BBF3F File Offset: 0x003BA13F
		public AiConditionAttackRangeEdgeMore(IReadOnlyList<int> ints) : base(ints)
		{
			this._isForward = (ints[1] == 1);
		}

		// Token: 0x06006A1B RID: 27163 RVA: 0x003BBF5C File Offset: 0x003BA15C
		protected override bool Check(CombatCharacter checkChar)
		{
			CombatCharacter otherChar = DomainManager.Combat.GetCombatCharacter(!checkChar.IsAlly, false);
			short edgeL = this._isForward ? checkChar.GetAttackRange().Outer : checkChar.GetAttackRange().Inner;
			short edgeR = this._isForward ? otherChar.GetAttackRange().Outer : otherChar.GetAttackRange().Inner;
			return edgeL > edgeR;
		}

		// Token: 0x04001D49 RID: 7497
		private readonly bool _isForward;
	}
}
