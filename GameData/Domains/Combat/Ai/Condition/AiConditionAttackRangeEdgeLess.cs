using System;
using System.Collections.Generic;

namespace GameData.Domains.Combat.Ai.Condition
{
	// Token: 0x020007B3 RID: 1971
	[AiCondition(EAiConditionType.AttackRangeEdgeLess)]
	public class AiConditionAttackRangeEdgeLess : AiConditionCheckCharBase
	{
		// Token: 0x06006A1C RID: 27164 RVA: 0x003BBFC9 File Offset: 0x003BA1C9
		public AiConditionAttackRangeEdgeLess(IReadOnlyList<int> ints) : base(ints)
		{
			this._isForward = (ints[1] == 1);
		}

		// Token: 0x06006A1D RID: 27165 RVA: 0x003BBFE4 File Offset: 0x003BA1E4
		protected override bool Check(CombatCharacter checkChar)
		{
			CombatCharacter otherChar = DomainManager.Combat.GetCombatCharacter(!checkChar.IsAlly, false);
			short edgeL = this._isForward ? checkChar.GetAttackRange().Outer : checkChar.GetAttackRange().Inner;
			short edgeR = this._isForward ? otherChar.GetAttackRange().Outer : otherChar.GetAttackRange().Inner;
			return edgeL < edgeR;
		}

		// Token: 0x04001D4A RID: 7498
		private readonly bool _isForward;
	}
}
